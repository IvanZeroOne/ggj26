/*using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Microlight.MicroAudio
{
    // ****************************************************************************************************
    // Instance of MicroInfinity sound effect
    // Lets user control the flow of the infinity sound effect
    // ****************************************************************************************************
    public class MicroInfinityInstance
    {
        // Variables
        MicroInfinitySoundGroup _infinityGroup;
        AudioMixerGroup _mixerGroup;
        bool _isPaused = false;

        // Timer
        float _timer;   // Timer for triggering random sound
        float _nextRandomSoundTimestamp;   // When will next random sound trigger

        // Play variables
        MicroSource _loopMicroSource;
        MicroSource _startMicroSource;
        MicroSource _endMicroSource;
        List<MicroSource> _randomsMicroSource = new List<MicroSource>();

        // Events
        public event Action<MicroInfinityInstance> OnResumed;
        public event Action<MicroInfinityInstance> OnPaused;
        public event Action<MicroInfinityInstance> OnEnded;

        #region Initialization

        internal MicroInfinityInstance(MicroInfinitySoundGroup infiniteGroup, AudioMixerGroup mixerGroup)
        {
            _infinityGroup = infiniteGroup;
            _mixerGroup = mixerGroup;

            if (_infinityGroup.AmountOfRandomClips < 1) _timer = -1f;

            Start();
        }

        void OnDestroy()
        {
            OnEnded = null;
            OnPaused = null;
            OnResumed = null;

            _infinityGroup = null;
        }

        #endregion

        #region Pause

        public bool IsPaused() => _isPaused;

        /// <summary>
        /// Pauses infinity sound effect
        /// </summary>
        public void Pause()
        {
            if (_isPaused == true) return;
            _isPaused = true;

            PauseSounds();

            OnPaused?.Invoke(this);
        }

        /// <summary>
        /// Resumes playing infinity sound effect
        /// </summary>
        public void Resume()
        {
            if (_isPaused == false) return;
            _isPaused = false;

            ResumeSounds();

            OnResumed?.Invoke(this);
        }

        void PauseSounds()
        {
            if (_startMicroSource != null) _startMicroSource.Pause();
            if (_endMicroSource != null) _endMicroSource.Pause();
            if (_loopMicroSource != null) _loopMicroSource.Pause();
            foreach (MicroSource microSource in _randomsMicroSource)
            {
                if (microSource != null) microSource.Pause();
            }
        }

        void ResumeSounds()
        {
            if (_startMicroSource != null) _startMicroSource.Resume();
            if (_endMicroSource != null) _endMicroSource.Resume();
            if (_loopMicroSource != null) _loopMicroSource.Resume();
            foreach (MicroSource microSource in _randomsMicroSource)
            {
                if (microSource != null) microSource.Resume();
            }
        }

        #endregion

        #region Stop

        /// <summary>
        /// Stops playing infinity sound and play end clip if defined in infinity group
        /// </summary>
        public void Stop(bool forceNoEndSound = false)
        {
            StopAllSounds();
            if (forceNoEndSound == false) PlayEndSound();
            OnEnded?.Invoke(this);
            _timer = -1;
            OnDestroy();
        }

        void StopAllSounds()
        {
            if (_loopMicroSource != null) _loopMicroSource.Stop();
            if (_startMicroSource != null) _startMicroSource.Stop();
            if (_endMicroSource != null) _endMicroSource.Stop();
            while (_randomsMicroSource.Count > 0)
            {
                if (_randomsMicroSource[0] == null) _randomsMicroSource.RemoveAt(0);
                else _randomsMicroSource[0].Stop();
            }
        }

        void StartSourceFinished(MicroSource microSource) => _startMicroSource = null;
        void LoopSourceFinished(MicroSource microSource) => _loopMicroSource = null;
        void EndSourceFinished(MicroSource microSource) => _endMicroSource = null;
        void RandomSourceFinished(MicroSource microSource)
        {
            _randomsMicroSource.Remove(microSource);
        }

        #endregion

        #region Sounds

        void Start()
        {
            if (_infinityGroup == null) return;
            PlayStartSound();
            PlayLoopSound();

            // Determine how will random sound play
            if (_startMicroSource == null)
            {
                if (_infinityGroup.DelayFirstRandomClip)
                {
                    SetNewRandomClipTime(0f);
                }
                else
                {
                    _nextRandomSoundTimestamp = 0f;
                }
            }
        }

        void PlayLoopSound()
        {
            if (_infinityGroup == null) return;
            if (_infinityGroup.LoopClip == null) return;

            _loopMicroSource = MicroAudio.PlaySound(_infinityGroup.LoopClip, _mixerGroup, 0f, _infinityGroup.LoopClipVolume, _infinityGroup.LoopClipPitch, true, null);
            _loopMicroSource.OnEnded += LoopSourceFinished;
        }

        void PlayRandomSound()
        {
            if (_infinityGroup == null) return;
            MicroSource source = MicroAudio.PlaySound(_infinityGroup.GetRandomClip, _mixerGroup, 0f, _infinityGroup.RandomClipsVolume, _infinityGroup.RandomClipsPitch, false, null);
            if (source == null)
            {
                SetNewRandomClipTime(1f);
                return;
            }

            _randomsMicroSource.Add(source);
            source.OnEnded += RandomSourceFinished;
            SetNewRandomClipTime(source.GetLength() / source.pitch);
        }

        void PlayStartSound()
        {
            if (_infinityGroup == null) return;
            if (_infinityGroup.StartClip == null) return;

            _startMicroSource = MicroAudio.PlaySound(_infinityGroup.StartClip, _mixerGroup, 0f, _infinityGroup.StartClipVolume, _infinityGroup.StartClipPitch, false, null);
            _startMicroSource.OnEnded += StartSourceFinished;
            if (_startMicroSource != null) SetNewRandomClipTime(_startMicroSource.GetLength());
        }

        void PlayEndSound()
        {
            if (_infinityGroup == null) return;
            if (_infinityGroup.EndClip == null) return;

            _endMicroSource = MicroAudio.PlaySound(_infinityGroup.EndClip, _mixerGroup, 0f, _infinityGroup.EndClipVolume, _infinityGroup.EndClipPitch, false, null);
            _endMicroSource.OnEnded += EndSourceFinished;
        }

        #endregion

        #region Lifetime

        internal void Update()
        {
            if (_timer == -1) return;
            if (_isPaused) return;
            if (_infinityGroup == null) return;
            _timer += Time.deltaTime;

            if (_timer >= _nextRandomSoundTimestamp)
            {
                PlayRandomSound();
            }
        }

        void SetNewRandomClipTime(float previousClipLength)
        {
            if (_infinityGroup == null) return;
            _nextRandomSoundTimestamp =
                _timer +
                previousClipLength +
                UnityEngine.Random.Range(_infinityGroup.TimeBetweenClips[0], _infinityGroup.TimeBetweenClips[1]);
        }

        #endregion
    }
}*/