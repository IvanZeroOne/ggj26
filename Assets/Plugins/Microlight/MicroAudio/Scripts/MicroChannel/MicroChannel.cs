using UnityEngine;
using UnityEngine.Audio;

namespace Microlight.MicroAudio
{
    // ****************************************************************************************************
    // Base class for the audio channels
    // ****************************************************************************************************
    public abstract class MicroChannel
    {
        protected readonly MicroChannelSO _channelData;
        readonly AudioMixer _mixer;

        internal MicroChannelSO ChannelData => _channelData;

        internal MicroChannel(MicroChannelSO channelData)
        {
            _channelData = channelData;
            _mixer = MicroAudio.Mixer;

            if (_channelData == null)
            {
                Debug.LogError("[MicroAudio] Channel data during audio channel creation is null]");
                return;
            }

            if (_channelData.MixerGroup == null)
            {
                Debug.LogError("[MicroAudio] Mixer group in channel data during audio channel creation is null]");
            }

            if (string.IsNullOrEmpty(_channelData.VolumeString))
            {
                Debug.LogError("[MicroAudio] Volume string in channel data during audio channel creation is null or empty]");
            }

            if (string.IsNullOrEmpty(_channelData.VolumePlayerPrefsKey))
            {
                Debug.LogError("[MicroAudio] VolumePlayerPrefsKey in channel data during audio channel creation is null or empty]");
            }

            if (string.IsNullOrEmpty(_channelData.MutePlayerPrefsKey))
            {
                Debug.LogError("[MicroAudio] MutePlayerPrefsKey in channel data during audio channel creation is null or empty]");
            }
        }

        #region Controls
        bool _muted;
        public bool Muted
        {
            get => _muted;
            set
            {
                _muted = value;
                SetMixerVolume(_volume, _muted);
            }
        }

        float _volume;
        public float Volume
        {
            get => _volume;
            set
            {
                value = Mathf.Clamp(value, 0f, 1f);
                _volume = value;
                SetMixerVolume(_volume, _muted);
            }
        }

        void SetMixerVolume(float volume, bool muteStatus)
        {
            volume = Mathf.Clamp(volume, MicroAudio.LOWEST_VOLUME, 1f);
            if (muteStatus || Mathf.Approximately(volume, MicroAudio.LOWEST_VOLUME))
            {
                _mixer.SetFloat(_channelData.VolumeString, -80f);
            }
            else
            {
                _mixer.SetFloat(_channelData.VolumeString, Mathf.Log10(volume) * 20);
            }
        }
        #endregion

        #region Save/Load
        // Tip: Custom save and load logic can be implemented without the use of PlayerPrefs
        // Just modify how SaveSettings and LoadSettings behave

        /// <summary>
        /// Saves current volume and mute settings for the audio channel
        /// </summary>
        public void SaveSettings()
        {
            PlayerPrefs.SetFloat(_channelData.VolumePlayerPrefsKey, _volume);
            PlayerPrefs.SetInt(_channelData.MutePlayerPrefsKey, _muted ? 1 : 0);
        }

        /// <summary>
        /// Loads volume and mute settings for the audio channel
        /// </summary>
        public void LoadSettings()
        {
            _volume = PlayerPrefs.GetFloat(_channelData.VolumePlayerPrefsKey, 1f);
            _muted = PlayerPrefs.GetInt(_channelData.MutePlayerPrefsKey, 0) > 0;
            SetMixerVolume(_volume, _muted);
        }
        #endregion
    }
}