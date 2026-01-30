using System;
using System.Collections.Generic;
using UnityEngine;

namespace Microlight.MicroAudio
{
    // ****************************************************************************************************
    // Handles handles for MicroSource
    // ****************************************************************************************************
    internal class MicroAudioHandles
    {
        // Dictionary which stores MicroSources and to which ID they are saved
        // With ID, users access their MicroSource through MicroHandle
        readonly Dictionary<uint, MicroSource> _sourceHandlesDict = new();
        readonly List<uint> _sourcesForRelease = new(); // Handles that will be released in the LateUpdate method
        readonly List<uint> _updateList = new(); // Handles that will be updated in the Update method

        internal uint IdCounter { get; private set; }
        internal IReadOnlyDictionary<uint, MicroSource> SourceHandlesDict => _sourceHandlesDict;

        // Pool connection
        Func<AudioSource, MicroSource> _takeMicroSourceCallback;
        Action<MicroSource> _returnMicroSourceCallback;

        internal void ConnectPoolToHandles(
            Func<AudioSource, MicroSource> takeMicroSourceCallback,
            Action<MicroSource> returnMicroSourceCallback)
        {
            _takeMicroSourceCallback = takeMicroSourceCallback;
            _returnMicroSourceCallback = returnMicroSourceCallback;
        }

        // ---------- Handle lifespan ----------
        internal MicroHandle CreateNewHandle(AudioSource audioSource)
        {
            MicroSource microSource = _takeMicroSourceCallback(audioSource);
            if (microSource == null)
            {
                Debug.LogWarning("[MicroAudio] Couldn't create new handle because it can't get MicroSource from pool");
                return default;
            }

            IdCounter++;
            _sourceHandlesDict.Add(IdCounter, microSource);
            microSource.AssignedToHandle(IdCounter);
            return new MicroHandle(IdCounter);
        }

        // Called to signal that MicroSource finished and handle can be deactivated
        internal void ReleaseHandle(uint handleId)
        {
            _sourcesForRelease.Add(handleId);
        }

        internal void Update()
        {
            // Will not update newly created sources
            _updateList.Clear();
            _updateList.AddRange(_sourceHandlesDict.Keys);
            foreach (uint handleId in _updateList)
            {
                _sourceHandlesDict[handleId].Update();
            }
        }

        internal void LateUpdate()
        {
            foreach (uint handleId in _sourcesForRelease)
            {
                if (_sourceHandlesDict.TryGetValue(handleId, out MicroSource microSource))
                {
                    _returnMicroSourceCallback(microSource);
                    _sourceHandlesDict.Remove(handleId);
                }
            }

            _sourcesForRelease.Clear();
        }

        #region Handle interaction
        // ---------- Handle interaction ----------
        internal bool IsHandleValid(uint handleId)
        {
            return _sourceHandlesDict.ContainsKey(handleId);
        }

        internal MicroSource GetMicroSource(uint handleId)
        {
            return _sourceHandlesDict.GetValueOrDefault(handleId);
        }
        #endregion

        #region Debug
        // ---------- Debug ----------
        internal int ActiveHandlesCount => _sourceHandlesDict.Count;
        #endregion
    }
}