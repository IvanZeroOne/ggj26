using System;
using UnityEngine;

namespace Microlight.MicroAudio
{
    // ****************************************************************************************************
    // Internal MicroAudio class which handles delays
    // ****************************************************************************************************
    public class MicroDelay
    {
        MicroSource _microSource;
        float _delay;
        float _timer;
        bool _paused;
        bool _finished = true;

        // Events
        public event Action<MicroDelay> OnPaused;
        public event Action<MicroDelay> OnResumed;
        public event Action<MicroDelay> OnCompleted; // Invoked when delay reaches the end naturally
        public event Action<MicroDelay> OnEnded; // Invoked always (even if killed)

        internal MicroDelay() { }

        internal void Start(MicroSource microSource, float delay)
        {
            _microSource = microSource;
            _delay = delay;

            _timer = 0;
            _paused = false;
            _finished = false;
        }

        // ---------- Getters ----------
        public bool IsPaused() => _paused;
        public bool IsFinished() => _finished;
        public float GetDuration() => _delay;
        public float GetElapsedTime() => _timer;

        /// <summary>
        /// Returns true only if it's progressing currently. Pausing the delay returns false
        /// </summary>
        public bool IsActive() => IsFinished() == false && IsPaused() == false;

        /// <summary>
        /// Returns the MicroSource that is being controlled by this delay
        /// Note: MicroSource is pooled, so it is recommended to read the README first and get familiar with MicroAudio
        /// </summary>
        public MicroSource GetMicroSource() => _microSource;

        /// <summary>
        /// Returns value between 0 and 1 based on how far the delay has progressed
        /// </summary>
        public float GetProgress() => Mathf.Clamp01(_timer / _delay);

        // ---------- Control ----------
        /// <summary>
        /// Stops the delay process and won't play the sound
        /// Doesn't trigger OnCompleted but triggers OnEnded
        /// </summary>
        public void Kill() => Ended();

        /// <summary>
        /// Skips the delay process and plays the sound
        /// Triggers OnCompleted and OnEnded
        /// </summary>
        public void SkipToEnd() => Completed();

        public void ResetTimer() => _timer = 0;

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

            _timer += Time.deltaTime;
            if (_timer >= _delay)
            {
                Completed();
            }
        }

        // ---------- End ----------
        void Completed()
        {
            _microSource.Play();
            _finished = true;
            OnCompleted?.Invoke(this);
            Ended();
        }

        void Ended()
        {
            _finished = true;
            OnEnded?.Invoke(this);
            _microSource.DelayEnded();
            MicroAudio.Instance.Pool.ReturnDelay(this); // Pool automatically clears the delay
        }

        // Should be called from the Pool only
        internal void Clear()
        {
            _microSource = null;
            _delay = 0;
            _timer = 0;
            _paused = false;
            _finished = true;

            OnPaused = null;
            OnResumed = null;
            OnCompleted = null;
            OnEnded = null;
        }
    }
}