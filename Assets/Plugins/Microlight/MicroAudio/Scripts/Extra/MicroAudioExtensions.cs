using System;
using UnityEngine;

namespace Microlight.MicroAudio
{
    public static class MicroAudioExtensions
    {
        // ---------- Audio Source ----------
        internal static void Clear(this AudioSource audioSource)
        {
            audioSource.Stop();
            audioSource.clip = null;
            audioSource.volume = 1;
            audioSource.pitch = 1;
            audioSource.loop = false;
        }

        // ---------- Lifespan ----------
        /// <summary>
        /// Only when sound naturally completed its clip
        /// </summary>
        public static void OnComplete(this MicroHandle handle, Action<MicroSource> callback)
        {
            if (handle.IsValid == false)
            {
                Debug.LogWarning("[MicroAudio] Tried to register callback on invalid handle");
                return;
            }

            handle.GetMicroSource().OnCompleted += callback;
        }

        /// <summary>
        /// Always when a source stops playing, stopped or naturally finished
        /// </summary>
        public static void OnEnd(this MicroHandle handle, Action<MicroSource> callback)
        {
            if (handle.IsValid == false)
            {
                Debug.LogWarning("[MicroAudio] Tried to register callback on invalid handle");
                return;
            }

            handle.GetMicroSource().OnEnded += callback;
        }
    }
}