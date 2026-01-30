/*namespace Microlight.MicroAudio
{
    // TODO: this file should be deleted when we transfer and implement the music and infinity again
    public partial class MicroAudio
    {
        // ---------- Modules ----------
        //MicroInfinityManager _microInfinityManager;

        #region Infinity Sounds
        /// <summary>
        /// Plays infinity sound on SFX channel
        /// </summary>
        /// <param name="group">Infinity group</param>
        /// <returns>Instance of infinity sound. Allows for control of sound and stopping effect</returns>
        public static MicroInfinityInstance PlayInfinityEffectSound(MicroInfinitySoundGroup group)
        {
            return PlayInfinitySound(group, SFXMixerGroup);
        }

        /// <summary>
        /// Plays infinity sound on UI channel
        /// </summary>
        /// <param name="group">Infinity group</param>
        /// <returns>Instance of infinity sound. Allows for control of sound and stopping effect</returns>
        public static MicroInfinityInstance PlayInfinityUISound(MicroInfinitySoundGroup group)
        {
            return PlayInfinitySound(group, UIMixerGroup);
        }

        /// <summary>
        /// Plays infinity sound on Ambience channel
        /// </summary>
        /// <param name="group">Infinity group</param>
        /// <returns>Instance of infinity sound. Allows for control of sound and stopping effect</returns>
        public static MicroInfinityInstance PlayInfinityAmbienceSound(MicroInfinitySoundGroup group)
        {
            return PlayInfinitySound(group, AmbienceMixerGroup);
        }

        static MicroInfinityInstance PlayInfinitySound(MicroInfinitySoundGroup group, AudioMixerGroup mixerGroup)
        {
            if (Instance == null || Instance._microSounds == null || Instance._microInfinityManager == null)
            {
                return null;
            }
            return Instance._microInfinityManager.PlayInfinitySound(group, mixerGroup);
        }

        public static void StopAllInfinitySounds()
        {
            if (Instance == null || Instance._microSounds == null || Instance._microInfinityManager == null)
            {
                return;
            }
            Instance._microInfinityManager.StopAllInfinitySounds();
        }
        #endregion
    }
}*/