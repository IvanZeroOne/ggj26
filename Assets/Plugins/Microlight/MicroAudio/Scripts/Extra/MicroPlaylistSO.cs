using System.Collections.Generic;
using UnityEngine;

namespace Microlight.MicroAudio
{
    // ****************************************************************************************************
    // ScriptableObject used for storing playlists
    // ****************************************************************************************************
    [CreateAssetMenu(fileName = "MicroPlaylist", menuName = "Microlight/Micro Audio/Playlist")]
    public class MicroPlaylistSO : ScriptableObject
    {
        [SerializeField] List<AudioClip> _clipList;
        public IReadOnlyList<AudioClip> ClipList => _clipList;
    }
}