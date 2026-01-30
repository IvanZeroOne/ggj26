using UnityEngine;

namespace Microlight.MicroAudio
{
    // ****************************************************************************************************
    // Manager for MicroAudio, handles settings and intermediate between modules
    // Should be on the first scene because it has DontDestroyOnLoad flag
    // ****************************************************************************************************
    [DefaultExecutionOrder(-1)] // So it is handled before other scripts, so it is ready for other scripts
    public partial class MicroAudio : MonoBehaviour
    {
        // ---------- Settings ----------
        internal const float LOWEST_VOLUME = 0.001f;

        [Space]
        [Header("Audio Sources Settings")]
        [SerializeField] int _maxSoundSources = 128;
        [SerializeField] int _maxInstancesOfSameClip;
        [SerializeField] int _prewarmPoolAmount = 5;

        internal int MaxSoundSources => _maxSoundSources;
        internal int MaxInstancesOfSameClip => _maxInstancesOfSameClip;

        // ---------- Modules ----------

        internal MicroAudioPool Pool { get; private set; }
        internal MicroAudioHandles Handles { get; private set; }

        // ---------- Singleton ----------
        internal static MicroAudio Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogWarning("[MicroAudio] Detected duplicate instance of MicroAudio. Destroying the new one");
                Destroy(gameObject);
                return;
            }

            InitializeChannels();
            LoadSettings();

            Handles = new MicroAudioHandles();
            Pool = new MicroAudioPool(_prewarmPoolAmount); // Must go after Handles because it needs to connect to Handles
        }

        void OnApplicationQuit()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            Handles.Update();
        }

        void LateUpdate()
        {
            Handles.LateUpdate();
        }

        #region SaveLoad
        /// <summary>
        /// Helper method to save the settings of all the channels. Individual channel settings can still be used
        /// </summary>
        public static void SaveSettings()
        {
            foreach (MicroChannel microChannel in Instance._channelsInUse)
            {
                microChannel.SaveSettings();
            }
        }

        /// <summary>
        /// Helper method to load the settings of all the channels. Individual channel settings can still be used
        /// </summary>
        public static void LoadSettings()
        {
            foreach (MicroChannel microChannel in Instance._channelsInUse)
            {
                microChannel.LoadSettings();
            }
        }
        #endregion
    }
}