using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Microlight.MicroAudio
{
    // ****************************************************************************************************
    // Music version of the audio channel which is designed to play soundtracks
    // ****************************************************************************************************
    public class MicroChannelMusic : MicroChannel
    {
        readonly MicroAudio _controller;

        // Sources
        float _savedSourceVolume = 1f;
        MicroHandle _mainHandle;
        MicroHandle _crossfadeHandle;

        bool _paused;
        float _crossfadeDuration;
        readonly MicroPlaylist _playlist;

        // Events
        public event Action OnTrackStarted;
        public event Action OnTrackCompleted;
        public event Action OnCrossfadeStarted;
        public event Action OnCrossfadeCompleted;
        public event Action OnPaused;
        public event Action OnResumed;
        public event Action OnStopped;

        internal MicroChannelMusic(MicroChannelSO channelData) : base(channelData)
        {
            _controller = MicroAudio.Instance;
            _playlist = new MicroPlaylist(this);
        }

        #region Getters
        // ---------- Getters ----------
        public bool IsPaused() => _paused;

        /// <summary>
        /// Gets the handle of currently playing track
        /// </summary>
        public MicroHandle GetTrackHandle() => _mainHandle;

        /// <summary>
        /// Handle of the track that is currently fading out while the main track is fading in.
        /// </summary>
        public MicroHandle GetCrossfadeHandle() => _crossfadeHandle;

        /// <summary>
        /// Returns true if there is music playing, even if is paused
        /// </summary>
        public bool IsActive()
        {
            return _mainHandle.IsValid;
        }

        /// <summary>
        /// Returns true if tracks are crossfading, even if is paused
        /// </summary>
        public bool IsCrossfadeActive()
        {
            return _crossfadeHandle.IsValid;
        }

        /// <summary>
        /// Returns the list of the clips in order that they will be played
        /// </summary>
        public IReadOnlyList<AudioClip> GetPlaylistClips() => _playlist.GetClipList();

        /// <summary>
        /// Returns the index of the clip that is currently playing from the list of playlist clips
        /// </summary>
        public int GetCurrentTrackIndex() => _playlist.GetCurrentIndex();

        /// <summary>
        /// Returns value from 0 to 1 of the current track progress
        /// </summary>
        public float GetTrackProgress()
        {
            if (_mainHandle.IsValid == false) return 0;

            if (_crossfadeHandle.IsValid) return _crossfadeHandle.GetProgress();
            return _mainHandle.GetProgress();
        }

        /// <summary>
        /// Returns value from 0 to 1 of the fade progress that handles crossfade
        /// </summary>
        public float GetCrossfadeProgress()
        {
            if (_crossfadeHandle.IsValid == false) return 0;
            if (_crossfadeHandle.GetFade() == null) return 0;
            return _crossfadeHandle.GetFade().GetProgress();
        }
        #endregion

        #region Music Clips/Playlists
        /// <summary>
        /// Plays a single clip on the music channel
        /// </summary>
        public void PlayTrack(AudioClip clip, bool loop = false, float volume = -1f)
        {
            if (clip == null)
            {
                Debug.LogWarning("[MicroAudio] Passed audio clip is null");
                return;
            }

            PlayPlaylist(new List<AudioClip> { clip }, loop, volume);
        }

        /// <summary>
        /// Helper method to allow inserting MicroPlaylistSO directly instead of the list of the clips
        /// </summary>
        /// <param name="playlistSO">Scriptable object containing a list of clips to be played</param>
        /// <param name="loop">Will the playlist play again when finished. If it was shuffled, it will be shuffled again</param>
        /// <param name="volume">Volume of the playlist, not the actual setting in MicroAudio</param>
        /// <param name="shuffle">Will the playlist be shuffled</param>
        public void PlayPlaylist(MicroPlaylistSO playlistSO, bool loop = false, float volume = -1f, bool shuffle = false)
        {
            if (playlistSO == null)
            {
                Debug.LogWarning("[MicroAudio] Passed MicroPlaylistSO object is null");
                return;
            }

            if (playlistSO.ClipList == null)
            {
                Debug.LogWarning("[MicroAudio] Passed clip list in MicroPlaylistSO object is null");
                return;
            }

            PlayPlaylist(playlistSO.ClipList, loop, volume, shuffle);
        }

        /// <summary>
        /// Plays the list of clips
        /// </summary>
        /// <param name="listOfTracks">List containing clips to be used for the playlist</param>
        /// <param name="loop">Will the playlist play again when finished. If it was shuffled, it will be shuffled again</param>
        /// <param name="volume">Volume of the playlist, not the actual setting in MicroAudio</param>
        /// <param name="shuffle">Will the playlist be shuffled</param>
        public void PlayPlaylist(IReadOnlyList<AudioClip> listOfTracks, bool loop = false, float volume = -1f, bool shuffle = false)
        {
            if (listOfTracks == null)
            {
                Debug.LogWarning("[MicroAudio] Passed list of clips is null");
                return;
            }

            if (listOfTracks.Count < 1)
            {
                Debug.LogWarning("[MicroAudio] Passed list of clips is empty");
                return;
            }

            Resume();
            SaveSourceVolume(volume);
            _playlist.AssignPlaylist(listOfTracks, shuffle, loop);
        }

        internal void PlayMusicClip(AudioClip clip)
        {
            MainSourceToCrossfade();
            MicroHandle handle = _controller.Handles.CreateNewHandle(null);
            if (handle.IsValid == false)
            {
                Debug.LogWarning("[MicroAudio] Handle creation failed when trying to play music clip");
                return;
            }

            _mainHandle = handle;
            MicroSource microSource = handle.GetMicroSource();
            AudioSource audioSource = microSource.GetAudioSource();

            audioSource.clip = clip;
            audioSource.loop = false;
            audioSource.volume = _savedSourceVolume;
            audioSource.outputAudioMixerGroup = _channelData.MixerGroup;
            audioSource.Play();
            microSource.OnCompleted += TrackCompleted;
            microSource.OnFadeOut += TrackCompleted;

            if (_crossfadeDuration > 0f) handle.FadeOutAtEnd(_crossfadeDuration);
            CrossfadeTrackIn();

            OnTrackStarted?.Invoke();
        }
        #endregion

        #region Sources
        void StopAllAudioSources()
        {
            _playlist.Clear();
            if (_crossfadeHandle.IsValid) _crossfadeHandle.Stop();
            if (_mainHandle.IsValid) _mainHandle.Stop();
        }

        void MainSourceToCrossfade()
        {
            _crossfadeHandle.Release();
            _crossfadeHandle = _mainHandle;
            _mainHandle = default;
        }

        void SaveSourceVolume(float volume)
        {
            if (volume >= 0)
            {
                _savedSourceVolume = Mathf.Clamp01(volume);
            }
        }

        void UnsubscribeFromSourceEvents(MicroSource microSource)
        {
            microSource.OnCompleted -= TrackCompleted;
            microSource.OnFadeOut -= TrackCompleted;
        }
        #endregion

        #region Track Control
        public void NextTrack()
        {
            ManualCrossfadeTrackOut();
            OnTrackCompleted?.Invoke();
            _playlist.NextTrack();
        }

        public void PreviousTrack()
        {
            ManualCrossfadeTrackOut();
            OnTrackCompleted?.Invoke();
            _playlist.PreviousTrack();
        }

        /// <summary>
        /// Selects which track to play based on the index in the playlist
        /// </summary>
        public void SelectTrack(int index)
        {
            ManualCrossfadeTrackOut();
            OnTrackCompleted?.Invoke();
            _playlist.SelectTrack(index);
        }

        /// <summary>
        /// Selects which track to play based on the clip in the playlist
        /// </summary>
        public void SelectTrack(AudioClip clip)
        {
            if (clip == null)
            {
                Debug.LogWarning("[MicroAudio] Tried to select track with null clip");
                return;
            }

            IReadOnlyList<AudioClip> clipList = _playlist.GetClipList();
            for (int i = 0; i < clipList.Count; i++)
            {
                if (clipList[i] == clip)
                {
                    ManualCrossfadeTrackOut();
                    OnTrackCompleted?.Invoke();
                    _playlist.SelectTrack(i);
                    return;
                }
            }
        }

        void TrackCompleted(MicroSource microSource)
        {
            UnsubscribeFromSourceEvents(microSource);
            OnTrackCompleted?.Invoke();
            _playlist.NextTrack();
        }
        #endregion

        #region Pause/Stop
        public void Stop()
        {
            StopAllAudioSources();
            OnStopped?.Invoke();
        }

        /// <summary>
        /// Stops music playback but fades it out
        /// Leaving duration to -1 will take the default crossfade duration
        /// </summary>
        public void StopMusicWithFadeOut(float duration = -1f)
        {
            if (duration < 0f) duration = _crossfadeDuration;
            if (duration <= 0f)
            {
                Stop();
                return;
            }

            if (_mainHandle.IsValid)
            {
                UnsubscribeFromSourceEvents(_mainHandle.GetMicroSource());
                _mainHandle.FadeOutAtEnd(0f);
                _mainHandle.Fade(0f, duration);
                _mainHandle.GetFade().OnCompleted += x => StopAllAudioSources();
            }
            OnStopped?.Invoke();
        }

        public void Pause()
        {
            SetIsPaused(true);
        }

        public void Resume()
        {
            SetIsPaused(false);
        }

        public void TogglePause()
        {
            SetIsPaused(!_paused);
        }

        void SetIsPaused(bool paused)
        {
            if (_paused == paused) return;

            _paused = paused;
            if (_paused)
            {
                if (_mainHandle.IsValid) _mainHandle.Pause();
                if (_crossfadeHandle.IsValid) _crossfadeHandle.Pause();
                OnPaused?.Invoke();
            }
            else
            {
                if (_mainHandle.IsValid) _mainHandle.Resume();
                if (_crossfadeHandle.IsValid) _crossfadeHandle.Resume();
                OnResumed?.Invoke();
            }
        }
        #endregion

        #region Crossfade
        public void SetCrossfadeDuration(float duration)
        {
            if (duration < 0) return;
            _crossfadeDuration = duration;
            if (_mainHandle.IsValid) _mainHandle.FadeOutAtEnd(_crossfadeDuration);
        }

        public float GetCrossfadeDuration() => _crossfadeDuration;

        void CrossfadeTrackIn()
        {
            if (_crossfadeDuration <= 0) return;
            if (_mainHandle.IsValid == false) return;

            _mainHandle.GetMicroSource().GetAudioSource().volume = 0;
            _mainHandle.Fade(_savedSourceVolume, _crossfadeDuration);
            _mainHandle.GetFade().OnCompleted += x => OnCrossfadeCompleted?.Invoke();
            OnCrossfadeStarted?.Invoke();
        }

        void ManualCrossfadeTrackOut()
        {
            if (_crossfadeDuration <= 0) return;
            if (_mainHandle.IsValid == false) return;

            MicroSource microSource = _mainHandle.GetMicroSource();
            UnsubscribeFromSourceEvents(microSource);
            microSource.FadeOutAtEnd(0f);
            _mainHandle.Fade(0, _crossfadeDuration);
        }
        #endregion
    }
}