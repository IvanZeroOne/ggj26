using System.Collections.Generic;
using UnityEngine;

namespace Microlight.MicroAudio
{
    // ****************************************************************************************************
    // Handles pooling of MicroSource, AudioSource, MicroDelay and MicroFade objects
    // ****************************************************************************************************
    internal class MicroAudioPool
    {
        readonly MicroAudio _controller;

        readonly Queue<MicroSource> _availableMicroSources = new();
        readonly Queue<AudioSource> _availableAudioSources = new();
        readonly Queue<MicroDelay> _availableDelays = new();
        readonly Queue<MicroFade> _availableFades = new();

        internal int SpawnedMicroSources { get; private set; }
        internal int SpawnedAudioSources { get; private set; }
        internal int SpawnedDelays { get; private set; }
        internal int SpawnedFades { get; private set; }

        internal MicroAudioPool(int prewarmAmount)
        {
            _controller = MicroAudio.Instance;

            for (int i = 0; i < prewarmAmount; i++)
            {
                _availableMicroSources.Enqueue(SpawnNewMicroSource());
                _availableAudioSources.Enqueue(SpawnNewAudioSource());
                _availableDelays.Enqueue(SpawnNewDelay());
                _availableFades.Enqueue(SpawnNewFade());
            }

            _controller.Handles.ConnectPoolToHandles(TakeMicroSource, ReturnMicroSource);
        }

        #region MicroSource
        // ---------- MicroSource ----------
        // If custom AudioSource wants to be used, it has to be passed as an argument here
        // This can still return null, so it needs to be checked
        MicroSource TakeMicroSource(AudioSource audioSource)
        {
            MicroSource microSource = GetFreeMicroSource();
            if (microSource == null) return null;

            // Take audio source if its needed
            bool returnAudioSourceToPool = audioSource == null;
            if (returnAudioSourceToPool) audioSource = GetFreeAudioSource();

            microSource.TakenFromPool(audioSource, returnAudioSourceToPool);
            return microSource;
        }

        void ReturnMicroSource(MicroSource microSource)
        {
            if (microSource == null) return;

            if (microSource.IsAudioSourceFromPool) ReturnAudioSource(microSource.GetAudioSource());
            microSource.Clear();
            _availableMicroSources.Enqueue(microSource);
        }

        MicroSource GetFreeMicroSource()
        {
            if (_availableMicroSources.TryDequeue(out MicroSource microSource))
            {
                return microSource;
            }

            return SpawnNewMicroSource();
        }

        MicroSource SpawnNewMicroSource()
        {
            if (_controller.MaxSoundSources > 0 && SpawnedMicroSources >= _controller.MaxSoundSources)
            {
                Debug.LogWarning("[MicroAudio] Maximum number of sound sources reached. No more MicroSources can be created");
                return null;
            }

            MicroSource newMicroSource = new();
            SpawnedMicroSources++;
            return newMicroSource;
        }
        #endregion

        #region AudioSource
        // ---------- AudioSource ----------
        // Audio source can be taken only by the MicroSource creation
        void ReturnAudioSource(AudioSource audioSource)
        {
            if (audioSource == null) return;
            audioSource.Clear();
            _availableAudioSources.Enqueue(audioSource);
        }

        // You can always get a new AudioSource
        AudioSource GetFreeAudioSource()
        {
            if (_availableAudioSources.TryDequeue(out AudioSource audioSource))
            {
                return audioSource;
            }

            return SpawnNewAudioSource();
        }

        AudioSource SpawnNewAudioSource()
        {
            GameObject spawnedObject = new($"SoundPlayer {SpawnedAudioSources}", typeof(AudioSource));
            spawnedObject.transform.SetParent(_controller.transform);
            AudioSource audioSource = spawnedObject.GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            SpawnedAudioSources++;
            return audioSource;
        }
        #endregion

        #region Delay
        // ---------- Delay ----------
        internal MicroDelay TakeDelay()
        {
            if (_availableDelays.TryDequeue(out MicroDelay delay))
            {
                return delay;
            }

            return SpawnNewDelay();
        }

        internal void ReturnDelay(MicroDelay delay)
        {
            delay.Clear();
            _availableDelays.Enqueue(delay);
        }

        MicroDelay SpawnNewDelay()
        {
            SpawnedDelays++;
            return new MicroDelay();
        }
        #endregion

        #region Fade
        // ---------- Fade ----------
        internal MicroFade TakeFade()
        {
            if (_availableFades.TryDequeue(out MicroFade fade))
            {
                return fade;
            }

            return SpawnNewFade();
        }

        internal void ReturnFade(MicroFade fade)
        {
            fade.Clear();
            _availableFades.Enqueue(fade);
        }

        MicroFade SpawnNewFade()
        {
            SpawnedFades++;
            return new MicroFade();
        }
        #endregion

        #region Debug
        // ---------- Debug ----------
        internal int AvailableMicroSources => _availableMicroSources.Count;
        internal int AvailableAudioSources => _availableAudioSources.Count;
        internal int AvailableDelays => _availableDelays.Count;
        internal int AvailableFades => _availableFades.Count;
        #endregion
    }
}