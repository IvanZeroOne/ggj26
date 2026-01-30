using UnityEngine;

namespace Microlight.MicroAudio
{
    // ****************************************************************************************************
    // MicroHandle is the exposed part of the MicroSource that allows for an error-free way of interacting with the MicroAudio
    // ****************************************************************************************************
    public readonly struct MicroHandle
    {
        readonly uint _handleId;
        readonly MicroAudio _controller;

        internal MicroHandle(uint handleId)
        {
            _handleId = handleId;
            _controller = MicroAudio.Instance;
        }

        public bool IsValid => _controller != null && _controller.Handles.IsHandleValid(_handleId);

        /// <summary>
        /// Releases a handle so that its objects may be returned to the pool
        /// </summary>
        internal void Release()
        {
            if (IsValid == false) return;
            _controller.Handles.ReleaseHandle(_handleId);
        }

        // ---------- Getters ----------
        /// <summary>
        /// Returns MicroSource handled with this handle. Returns null if a handle expired
        /// Note: MicroSource is pooled, so it is recommended to read the README first and get familiar with MicroAudio
        /// </summary>
        public MicroSource GetMicroSource()
        {
            if (IsValid == false)
            {
                Debug.LogWarning($"[MicroAudio] Handle {_handleId} is invalid");
                return null;
            }

            return _controller.Handles.GetMicroSource(_handleId);
        }

        /// <summary>
        /// Returns AudioSource being controlled by this handle MicroSource
        /// Note: AudioSource might be pooled, so it is recommended to read the README first and get familiar with MicroAudio
        /// </summary>
        public AudioSource GetAudioSource()
        {
            MicroSource microSource = GetMicroSource();
            if (microSource == null) return null;
            return microSource.GetAudioSource();
        }

        /// <summary>
        /// Returns AudioClip being played by this handle
        /// </summary>
        public AudioClip GetAudioClip()
        {
            MicroSource microSource = GetMicroSource();
            if (microSource == null) return null;
            return microSource.GetAudioClip();
        }

        /// <summary>
        /// Returns MicroDelay object handling the delay of the sound. Returns null if there is no delay
        /// Note: MicroDelay is pooled, so it is recommended to read the README first and get familiar with MicroAudio
        /// </summary>
        public MicroDelay GetDelay()
        {
            MicroSource microSource = GetMicroSource();
            if (microSource == null) return null;
            return microSource.GetDelay();
        }

        /// <summary>
        /// Returns MicroFade object handling the fading of the sound. Returns null if there is no fading
        /// Note: MicroFade is pooled, so it is recommended to read the README first and get familiar with MicroAudio
        /// </summary>
        public MicroFade GetFade()
        {
            MicroSource microSource = GetMicroSource();
            if (microSource == null) return null;
            return microSource.GetFade();
        }

        /// <summary>
        /// Returns the length of the sound in seconds (modified by the pitch but no other effects if there are any)
        /// </summary>
        public float GetLength()
        {
            MicroSource microSource = GetMicroSource();
            if (microSource == null) return 0f;
            return microSource.GetLength();
        }

        /// <summary>
        /// Returns value between 0 and 1 of the current clip progress
        /// </summary>
        public float GetProgress()
        {
            MicroSource microSource = GetMicroSource();
            if (microSource == null) return 0f;
            return microSource.GetProgress();
        }

        public bool IsPaused()
        {
            MicroSource microSource = GetMicroSource();
            if (microSource == null) return false;
            return microSource.IsPaused();
        }

        /// <summary>
        /// Is the sound currently playing?
        /// </summary>
        public bool IsPlaying()
        {
            MicroSource microSource = GetMicroSource();
            if (microSource == null) return false;
            return microSource.IsPlaying();
        }

        // ---------- Control ----------
        public void Stop()
        {
            MicroSource microSource = GetMicroSource();
            if (microSource == null) return;
            microSource.Stop();
        }

        public void Pause()
        {
            MicroSource microSource = GetMicroSource();
            if (microSource == null) return;
            microSource.Pause();
        }

        public void Resume()
        {
            MicroSource microSource = GetMicroSource();
            if (microSource == null) return;
            microSource.Resume();
        }

        /// <summary>
        /// Fades the sound from the current volume to the specified volume over a period of the time
        /// </summary>
        public void Fade(float endVolume, float duration, float delay = 0)
        {
            MicroSource microSource = GetMicroSource();
            if (microSource == null) return;
            microSource.Fade(endVolume, duration, delay);
        }

        /// <summary>
        /// Will fade out sound right before the end so that at the end of the sound volume will be 0
        /// </summary>
        public void FadeOutAtEnd(float duration)
        {
            MicroSource microSource = GetMicroSource();
            if (microSource == null) return;
            microSource.FadeOutAtEnd(duration);
        }
    }
}