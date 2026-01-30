using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Microlight.MicroAudio
{
    // ****************************************************************************************************
    // Sounds version of the channel which allows playing of the sounds
    // ****************************************************************************************************
    public class MicroChannelSounds : MicroChannel
    {
        readonly List<MicroSource> _takenMicroSources = new(); // TODO: this could be MicroHandle also
        readonly MicroAudio _controller;

        internal MicroChannelSounds(MicroChannelSO channelData) : base(channelData)
        {
            _controller = MicroAudio.Instance;
        }

        #region API
        /// <summary>
        /// Play sound
        /// </summary>
        public MicroHandle PlaySound(AudioClip clip, float delay = 0f, float volume = 1f, float pitch = 1f, bool loop = false)
        {
            if (clip == null)
            {
                Debug.LogWarning("[MicroAudio] Tried to play sound with the null clip");
                return default;
            }
            return PlaySound(clip, _channelData.MixerGroup, null, delay, volume, pitch, loop);
        }

        /// <summary>
        /// Play sound on a custom audio source
        /// Will use audio source values for the volume, pitch and loop
        /// </summary>
        public MicroHandle PlaySound(AudioClip clip, AudioSource audioSource, float delay = 0f)
        {
            if (audioSource == null)
            {
                Debug.LogWarning("[MicroAudio] Tried to play sound from the null audio source");
                return default;
            }

            if (clip == null)
            {
                Debug.LogWarning("[MicroAudio] Tried to play sound with the null clip");
                return default;
            }
            return PlaySound(clip, _channelData.MixerGroup, audioSource, delay, audioSource.volume, audioSource.pitch, audioSource.loop);
        }

        /// <summary>
        /// Pauses all sounds on this audio channel
        /// </summary>
        public void PauseAll()
        {
            foreach (MicroSource microSource in _takenMicroSources)
            {
                if (microSource != null) microSource.Pause();
            }
        }

        /// <summary>
        /// Resumes all sounds on this audio channel
        /// </summary>
        public void ResumeAll()
        {
            foreach (MicroSource microSource in _takenMicroSources)
            {
                if (microSource != null) microSource.Resume();
            }
        }

        /// <summary>
        /// Stops all sounds on this audio channel
        /// </summary>
        public void StopAll()
        {
            foreach (MicroSource microSource in _takenMicroSources)
            {
                if (microSource != null) microSource.Stop();
            }
        }
        #endregion

        MicroHandle PlaySound(
            AudioClip clip,
            AudioMixerGroup mixerGroup,
            AudioSource audioSource,
            float delay,
            float volume,
            float pitch,
            bool loop)
        {
            if (mixerGroup == null) return default;
            if (IsThereTooManyInstancesOfClip(clip)) return default;

            // Handle source
            MicroHandle handle = _controller.Handles.CreateNewHandle(audioSource);
            if (handle.IsValid == false)
            {
                Debug.LogWarning("[MicroAudio] Handle creation failed. Sound will not be played");
                return default;
            }
            MicroSource microSource = handle.GetMicroSource();
            _takenMicroSources.Add(microSource);
            microSource.OnEnded += MicroSourceEnded;

            audioSource = microSource.GetAudioSource();
            audioSource.clip = clip;
            audioSource.volume = Mathf.Clamp01(volume);
            audioSource.outputAudioMixerGroup = mixerGroup;
            audioSource.loop = loop;
            audioSource.pitch = pitch;

            if (delay > 0f)
            {
                microSource.PlayAfterDelay(delay);
            }
            else
            {
                microSource.Play();
            }
            return handle;
        }

        void MicroSourceEnded(MicroSource microSource)
        {
            _takenMicroSources.Remove(microSource);
        }

        bool IsThereTooManyInstancesOfClip(AudioClip clipToPlay)
        {
            if (_controller.MaxInstancesOfSameClip <= 0) return false;

            int sameClipCount = 0;
            foreach (MicroSource microSource in _controller.Handles.SourceHandlesDict.Values)
            {
                AudioSource audioSource = microSource.GetAudioSource();
                if (audioSource == null) continue;
                if (audioSource.clip == clipToPlay) sameClipCount++;
                if (sameClipCount >= _controller.MaxInstancesOfSameClip)
                {
                    Debug.LogWarning("[MicroAudio] Too many instances of the same clip. Sound will not be played");
                    return true;
                }
            }

            return false;
        }
    }
}