using System;
using UnityEngine;

namespace Microlight.MicroAudio
{
    // ****************************************************************************************************
    // Internal MicroAudio class, which handles fading of the sound volume over time
    // ****************************************************************************************************
    public class MicroFade
    {
        MicroSource _microSource;
        AudioSource _audioSource;
        float _startVolume;
        float _endVolume;
        float _overSeconds;
        float _timer;
        bool _paused;
        bool _finished = true;

        // Events
        public event Action<MicroFade> OnStarted;
        public event Action<MicroFade> OnPaused;
        public event Action<MicroFade> OnResumed;
        public event Action<MicroFade> OnCompleted; // Invoked when fade reaches the end naturally
        public event Action<MicroFade> OnEnded; // Invoked always (even if killed)

        internal MicroFade() { }

        internal void Start(MicroSource microSource, float startVolume, float endVolume, float overSeconds, float delay)
        {
            _microSource = microSource;
            _audioSource = microSource.GetAudioSource();
            _startVolume = Mathf.Clamp01(startVolume);
            _endVolume = Mathf.Clamp01(endVolume);
            _overSeconds = overSeconds;

            _timer = Mathf.Abs(delay) * -1;
            _paused = false;
            _finished = false;
        }

        // ---------- Getters ----------
        public bool IsPaused() => _paused;
        public bool IsFinished() => _finished;
        public float GetStartVolume() => _startVolume;
        public float GetEndVolume() => _endVolume;
        public float GetDuration() => _overSeconds;
        public float GetElapsedTime() => _timer;

        /// <summary>
        /// Returns true only if it's progressing currently. Pausing the fade returns false
        /// </summary>
        public bool IsActive() => IsFinished() == false && IsPaused() == false;

        /// <summary>
        /// Returns the MicroSource that is being controlled by this fade
        /// Note: MicroSource is pooled, so it is recommended to read the README first and get familiar with MicroAudio
        /// </summary>
        public MicroSource GetMicroSource() => _microSource;

        /// <summary>
        /// Returns value between 0 and 1 based on how far the fade has progressed
        /// </summary>
        public float GetProgress() => Mathf.Clamp01(_timer / _overSeconds);

        // ---------- Control ----------
        /// <summary>
        /// Stops the fade on current progress
        /// Doesn't trigger OnCompleted but triggers OnEnded
        /// </summary>
        public void Kill() => Ended();

        /// <summary>
        /// Skips fade to end and sets the source to the final volume.
        /// Triggers OnCompleted and OnEnded
        /// </summary>
        public void SkipToEnd() => Completed();

        public void Pause()
        {
            if (_paused) return;
            _paused = true;
            OnPaused?.Invoke(this);
        }

        public void Resume()
        {
            if (_paused == false) return;
            _paused = false;
            OnResumed?.Invoke(this);
        }

        // ---------- Update ----------
        internal void Update()
        {
            if (_paused) return;
            if (_microSource.IsPaused()) return;
            if (_microSource.GetDelay() != null) return;

            // This is the case for the delayed fade only
            float newTimer = _timer + Time.deltaTime;
            if (_timer <= 0f && newTimer > 0f)
            {
                OnStarted?.Invoke(this);
            }

            _timer = newTimer;
            if (_timer >= _overSeconds)
            {
                Completed();
            }
            else if (_timer >= 0f)
            {
                _audioSource.volume = Mathf.Clamp01(Mathf.Lerp(_startVolume, _endVolume, _timer / _overSeconds));
            }
        }

        // ---------- End ----------
        void Completed()
        {
            _audioSource.volume = _endVolume;
            _finished = true;
            OnCompleted?.Invoke(this);
            Ended();
        }

        void Ended()
        {
            _finished = true;
            OnEnded?.Invoke(this);
            _microSource.FadeEnded();
            MicroAudio.Instance.Pool.ReturnFade(this); // Pool automatically clears the fade
        }

        // Should be called from the Pool only
        internal void Clear()
        {
            _microSource = null;
            _startVolume = 0;
            _endVolume = 0;
            _overSeconds = 0;
            _timer = 0;
            _paused = false;
            _finished = true;

            OnStarted = null;
            OnPaused = null;
            OnResumed = null;
            OnCompleted = null;
            OnEnded = null;
        }
    }
}