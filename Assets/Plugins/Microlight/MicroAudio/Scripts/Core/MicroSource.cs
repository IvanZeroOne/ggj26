using System;
using UnityEngine;

namespace Microlight.MicroAudio
{
    // ****************************************************************************************************
    // QoL wrapper for the UnityEngine AudioSource for easier control
    // ****************************************************************************************************
    public class MicroSource
    {
        readonly MicroAudio _controller;

        AudioSource _audioSource;
        MicroDelay _delay;
        MicroFade _fade;
        float _fadeOutDuration; // To determine when the fade out should activate (must be larger than 0)
        float _fadeOutStartTime;
        bool _isPaused;
        float _lastFrameTime;   // Audio Source time last time, used to determine when the source has finished playing
        bool _didStartPlaying;
        internal bool IsAudioSourceFromPool { get; private set; }

        // Events
        public event Action<MicroSource> OnStarted;   // When it begins reproduction of the sound
        public event Action<MicroSource> OnPaused;
        public event Action<MicroSource> OnResumed;
        public event Action<MicroSource> OnStopped; // Invoked when reproduction is stopped intentionally
        public event Action<MicroSource> OnCompleted; // Invoked when the clip reaches the end naturally
        public event Action<MicroSource> OnEnded; // Invoked always (even if killed)
        public event Action<MicroSource> OnFadeOut;

        #region Core
        internal MicroSource()
        {
            _controller = MicroAudio.Instance;
        }

        internal void Update()
        {
            if (_delay != null) _delay.Update();
            if (_fade != null) _fade.Update();

            // If somehow the audio source is null
            if (_audioSource == null)
            {
                Debug.LogError("[MicroAudio] MicroSource during update has null AudioSource");
                Ended();
                return;
            }

            // If for some reason the clip is null
            if (_audioSource.clip == null)
            {
                Debug.LogError("[MicroAudio] MicroSource during update has null clip in AudioSource");
                Ended();
                return;
            }

            // If sound finished playing this frame
            if (_audioSource.isPlaying == false && _audioSource.time < _lastFrameTime)
            {
                _audioSource.Stop();
                Completed();
                return;
            }

            // Handle fade out starting before the end
            if (_fadeOutStartTime > 0 && _audioSource.time >= _fadeOutStartTime && _lastFrameTime < _fadeOutStartTime)
            {
                Fade(0f, _fadeOutDuration, 0f);
                OnFadeOut?.Invoke(this);
            }

            _lastFrameTime = _audioSource.time;
        }
        #endregion

        #region Pooling
        // ---------- Pooling ----------
        uint _handleId;

        internal void AssignedToHandle(uint handleId)
        {
            _handleId = handleId;
        }

        internal void TakenFromPool(AudioSource audioSource, bool returnAudioSourceToPool)
        {
            _audioSource = audioSource;
            IsAudioSourceFromPool = returnAudioSourceToPool;
        }
        #endregion

        #region Getters
        // ---------- Getters ----------
        internal AudioSource GetAudioSource() => _audioSource;
        internal MicroDelay GetDelay() => _delay;
        internal MicroFade GetFade() => _fade;
        internal bool IsPaused() => _isPaused;

        internal AudioClip GetAudioClip()
        {
            if (_audioSource == null) return null;
            return _audioSource.clip;
        }

        internal bool IsPlaying()
        {
            if (_audioSource == null) return false;
            if (_isPaused) return false;
            if (_didStartPlaying == false) return false;

            return _audioSource.isPlaying;
        }

        internal float GetLength()
        {
            if (_audioSource == null || _audioSource.clip == null) return 0f;
            return _audioSource.clip.length / Mathf.Abs(_audioSource.pitch);
        }
        internal float GetProgress()
        {
            if (_audioSource == null || _audioSource.clip == null) return 0f;
            return Mathf.Clamp01(_audioSource.time / GetLength());
        }
        #endregion

        #region Control
        // ---------- Control ----------
        // Starts playing the sound. Can be called only once
        internal void Play()
        {
            if (_audioSource == null) return;
            if (_audioSource.clip == null) return;
            if (_didStartPlaying) return;

            _audioSource.Play();
            _didStartPlaying = true;
            OnStarted?.Invoke(this);
        }

        internal void Stop()
        {
            if (_audioSource == null) return;
            if (_audioSource.clip == null) return;

            _audioSource.Stop();

            OnStopped?.Invoke(this);
            Ended();
        }

        internal void Pause()
        {
            if (_isPaused) return;
            if (_audioSource == null) return;
            if (_audioSource.clip == null) return;

            _audioSource.Pause();
            _isPaused = true;
            OnPaused?.Invoke(this);
        }

        internal void Resume()
        {
            if (_isPaused == false) return;
            if (_audioSource == null) return;
            if (_audioSource.clip == null) return;

            _audioSource.UnPause();
            _isPaused = false;
            OnResumed?.Invoke(this);
        }
        #endregion

        #region Delay
        // ---------- Delay ----------
        internal void PlayAfterDelay(float delay)
        {
            if (delay <= 0f)
            {
                Play();
                return;
            }

            _delay = _controller.Pool.TakeDelay();
            _delay.Start(this, delay);
        }

        internal void DelayEnded() => _delay = null;
        #endregion

        #region Fade
        // ---------- Fade ----------
        internal MicroFade Fade(float endVolume, float overSeconds, float delay)
        {
            if (_audioSource == null) return null;
            if (_fade != null) _fade.Kill();
            if (overSeconds <= 0) return null;

            _fade = _controller.Pool.TakeFade();
            _fade.Start(this, _audioSource.volume, endVolume, overSeconds, delay);
            return _fade;
        }

        internal void FadeOutAtEnd(float fadeOutDuration)
        {
            _fadeOutDuration = fadeOutDuration;
            if (_fadeOutDuration > 0)
            {
                _fadeOutStartTime = GetLength() - _fadeOutDuration;
            }
            else
            {
                _fadeOutStartTime = 0;
            }
        }

        internal void FadeEnded() => _fade = null;
        #endregion

        #region End
        // ---------- End ----------
        void Completed()
        {
            OnCompleted?.Invoke(this);
            Ended();
        }

        void Ended()
        {
            OnEnded?.Invoke(this);
            _controller.Handles.ReleaseHandle(_handleId);
        }

        // Should be called from the Pool only
        internal void Clear()
        {
            // Need to ensure that every component that MicroSource uses is returned to the pool
            _audioSource = null;
            if (_delay != null) _controller.Pool.ReturnDelay(_delay);
            _delay = null;
            if (_fade != null) _controller.Pool.ReturnFade(_fade);
            _fade = null;

            _fadeOutDuration = 0f;
            _isPaused = false;
            _lastFrameTime = 0f;
            _didStartPlaying = false;
            IsAudioSourceFromPool = false;

            _handleId = 0;

            OnStarted = null;
            OnPaused = null;
            OnResumed = null;
            OnStopped = null;
            OnCompleted = null;
            OnEnded = null;
            OnFadeOut = null;
        }
        #endregion
    }
}