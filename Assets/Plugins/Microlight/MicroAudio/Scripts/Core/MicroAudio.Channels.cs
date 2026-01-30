//#define LITE_MODE
//#define CUSTOM_MODE

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Microlight.MicroAudio
{
    // ****************************************************************************************************
    // Partial class of MicroAudio, responsible for all the available channels.
    // Allows for customizable experience of available channels
    // ****************************************************************************************************
    public partial class MicroAudio
    {
        readonly List<MicroChannel> _channelsInUse = new();

        // These must always be included
        [Header("Channels")]
        [SerializeField] AudioMixer _mixer;
        public static AudioMixer Mixer => Instance._mixer;

        MicroChannelMusic _musicChannel;
        public static MicroChannelMusic Music => Instance._musicChannel;

#if LITE_MODE && !CUSTOM_MODE
        // If using a lite version of the mixer
        [SerializeField] MicroChannelSO _masterChannelDataLite;
        [SerializeField] MicroChannelSO _musicChannelDataLite;
        [SerializeField] MicroChannelSO _soundsChannelDataLite;

        MicroChannelMaster _masterChannel;
        MicroChannelSounds _soundsChannel;

        public static MicroChannelMaster Master => Instance._masterChannel;
        public static MicroChannelSounds Sounds => Instance._soundsChannel;

        void InitializeChannels()
        {
            _masterChannel = new MicroChannelMaster(_masterChannelDataLite);
            _musicChannel = new MicroChannelMusic(_musicChannelDataLite);
            _soundsChannel = new MicroChannelSounds(_soundsChannelDataLite);
            _channelsInUse.Add(_masterChannel);
            _channelsInUse.Add(_musicChannel);
            _channelsInUse.Add(_soundsChannel);
        }

#elif !LITE_MODE && !CUSTOM_MODE

        // If using a full version of the mixer
        [SerializeField] MicroChannelSO _masterChannelData;
        [SerializeField] MicroChannelSO _musicChannelData;
        [SerializeField] MicroChannelSO _ambienceChannelData;
        [SerializeField] MicroChannelSO _soundsChannelData;
        [SerializeField] MicroChannelSO _sfxChannelData;
        [SerializeField] MicroChannelSO _uiChannelData;
        [SerializeField] MicroChannelSO _dialogueChannelData;

        MicroChannelMaster _masterChannel;
        MicroChannelSounds _ambienceChannel;
        MicroChannelMaster _soundsChannel;
        MicroChannelSounds _sfxChannel;
        MicroChannelSounds _uiChannel;
        MicroChannelSounds _dialogueChannel;

        public static MicroChannelMaster Master => Instance._masterChannel;
        public static MicroChannelSounds Ambience => Instance._ambienceChannel;
        public static MicroChannelMaster SoundsMaster => Instance._soundsChannel;
        public static MicroChannelSounds SFX => Instance._sfxChannel;
        public static MicroChannelSounds UI => Instance._uiChannel;
        public static MicroChannelSounds Dialogue => Instance._dialogueChannel;

        void InitializeChannels()
        {
            // Master and music channels
            _masterChannel = new MicroChannelMaster(_masterChannelData);
            _musicChannel = new MicroChannelMusic(_musicChannelData);
            _soundsChannel = new MicroChannelMaster(_soundsChannelData);

            // Sounds channels
            _ambienceChannel = new MicroChannelSounds(_ambienceChannelData);
            _sfxChannel = new MicroChannelSounds(_sfxChannelData);
            _uiChannel = new MicroChannelSounds(_uiChannelData);
            _dialogueChannel = new MicroChannelSounds(_dialogueChannelData);

            _channelsInUse.Add(_masterChannel);
            _channelsInUse.Add(_musicChannel);
            _channelsInUse.Add(_soundsChannel);
            _channelsInUse.Add(_ambienceChannel);
            _channelsInUse.Add(_sfxChannel);
            _channelsInUse.Add(_uiChannel);
            _channelsInUse.Add(_dialogueChannel);
        }

        // ---------- Controls ----------
        /// <summary>
        /// Pauses all sounds
        /// </summary>
        public static void PauseAllSounds()
        {
            if (Instance == null) return;

            Instance._ambienceChannel.PauseAll();
            Instance._sfxChannel.PauseAll();
            Instance._uiChannel.PauseAll();
            Instance._dialogueChannel.PauseAll();
        }

        /// <summary>
        /// Resumes playing of all sounds
        /// </summary>
        public static void ResumeAllSounds()
        {
            if (Instance == null) return;

            Instance._ambienceChannel.ResumeAll();
            Instance._sfxChannel.ResumeAll();
            Instance._uiChannel.ResumeAll();
            Instance._dialogueChannel.ResumeAll();
        }

        /// <summary>
        /// Stops all sounds from playing
        /// </summary>
        public static void StopAllSounds()
        {
            if (Instance == null) return;

            Instance._ambienceChannel.StopAll();
            Instance._sfxChannel.StopAll();
            Instance._uiChannel.StopAll();
            Instance._dialogueChannel.StopAll();
        }

#elif !LITE_MODE && CUSTOM_MODE
        // Tip: Place for a custom version of the mixer
        void InitializeChannels() { }

#endif
    }
}