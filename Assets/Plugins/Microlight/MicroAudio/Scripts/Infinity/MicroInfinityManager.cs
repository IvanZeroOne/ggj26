/*using System.Collections.Generic;
using UnityEngine.Audio;

namespace Microlight.MicroAudio
{
    // ****************************************************************************************************
    // Manager for playing MicroInfinity sound effects
    // ****************************************************************************************************
    public class MicroInfinityManager
    {
        readonly List<MicroInfinityInstance> _instanceList = new();

        internal MicroInfinityManager() { }

        internal void Update()
        {
            foreach (MicroInfinityInstance instance in _instanceList)
            {
                instance.Update();
            }
        }

        internal MicroInfinityInstance PlayInfinitySound(MicroInfinitySoundGroup infinityGroup, AudioMixerGroup mixerGroup)
        {
            // TODO: this should be pooled
            MicroInfinityInstance newInstance = new(infinityGroup, mixerGroup);
            _instanceList.Add(newInstance);
            newInstance.OnEnded += InstanceEnded;
            return newInstance;
        }

        public void StopAllInfinitySounds()
        {
            while (_instanceList.Count > 0)
            {
                _instanceList[0].Stop();
            }
        }

        void InstanceEnded(MicroInfinityInstance instance)
        {
            _instanceList.Remove(instance);
        }
    }

    // Special class which
    public static class MicroInfinityExtensions
    {
        /// <summary>
        /// Plays infinitely repeating sound with randomized sounds in between on the channel
        /// Returns instance which needs to be stored and stopped when not needed anymore
        /// </summary>
        public static MicroInfinityInstance PlayInfinitySound(this MicroChannelSounds microChannel, MicroInfinitySoundGroup infinitySoundGroup) =>
            MicroAudio.Instance.Infinity.PlayInfinitySound(infinitySoundGroup, microChannel.ChannelData.MixerGroup);
    }
}
*/