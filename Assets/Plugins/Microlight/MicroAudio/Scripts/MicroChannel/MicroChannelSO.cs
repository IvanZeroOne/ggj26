using UnityEngine;
using UnityEngine.Audio;

namespace Microlight.MicroAudio
{
    // ****************************************************************************************************
    // ScriptableObject that stores data about the audio channel
    // ****************************************************************************************************
    [CreateAssetMenu(fileName = "MicroChannel", menuName = "Microlight/Micro Audio/Micro Channel")]
    public class MicroChannelSO : ScriptableObject
    {
        [SerializeField] AudioMixerGroup _mixerGroup;
        [SerializeField] string _volumeString;
        [SerializeField] string _volumePlayerPrefsKey;
        [SerializeField] string _mutePlayerPrefsKey;

        internal AudioMixerGroup MixerGroup => _mixerGroup;
        internal string VolumeString => _volumeString;
        internal string VolumePlayerPrefsKey => _volumePlayerPrefsKey;
        internal string MutePlayerPrefsKey => _mutePlayerPrefsKey;
    }
}