using System.Collections.Generic;
using UnityEngine;

namespace Microlight.MicroAudio
{
    // ****************************************************************************************************
    // MicroPlaylist handles which track is currently playing from the list of clips and in which order
    // ****************************************************************************************************
    internal class MicroPlaylist
    {
        readonly MicroChannelMusic _musicChannel;
        List<AudioClip> _clipList = new();
        int _currentIndex;
        bool _shuffle;
        bool _loop;

        internal MicroPlaylist(MicroChannelMusic musicChannel)
        {
            _musicChannel = musicChannel;
        }

        internal IReadOnlyList<AudioClip> GetClipList() => _clipList;
        internal int GetCurrentIndex() => _currentIndex;

        internal void AssignPlaylist(IReadOnlyList<AudioClip> clipList, bool shuffle, bool loop)
        {
            _clipList.Clear();
            _clipList.AddRange(clipList);
            _currentIndex = 0;
            _shuffle = false;
            _loop = loop;
            if (shuffle) ShufflePlaylist();

            ApplyTrack();
        }

        // ---------- Control ----------
        internal void NextTrack()
        {
            _currentIndex++;

            if (_currentIndex >= _clipList.Count)
            {
                if (_loop)
                {
                    _currentIndex = 0;
                    if(_shuffle) ShufflePlaylist();
                }
                else
                {
                    Clear();
                    return;
                }
            }

            ApplyTrack();
        }

        internal void PreviousTrack()
        {
            _currentIndex--;
            if (_currentIndex < 0) _currentIndex = 0;
            ApplyTrack();
        }

        internal void SelectTrack(int newIndex)
        {
            if (newIndex < 0 || newIndex >= _clipList.Count)
            {
                Debug.LogWarning($"[MicroAudio] Tried to change to the track with the index of {newIndex}, but that is out of range for the playlist of size of {_clipList.Count} clips");
                return;
            }
            _currentIndex = newIndex;
            ApplyTrack();
        }

        // ---------- Processing ----------
        void ApplyTrack()
        {
            if (_clipList.Count < 1) return;
            _musicChannel.PlayMusicClip(_clipList[_currentIndex]);
        }

        void ShufflePlaylist()
        {
            if (_clipList.Count < 2) return;

            for (int i = 0; i < _clipList.Count; i++)
            {
                int randomIndex = Random.Range(i, _clipList.Count);
                (_clipList[i], _clipList[randomIndex]) = (_clipList[randomIndex], _clipList[i]);
            }

            _shuffle = true;
        }

        internal void Clear()
        {
            _clipList.Clear();
            _currentIndex = 0;
            _shuffle = false;
            _loop = false;
        }
    }
}