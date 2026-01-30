using UnityEngine;
using UnityEditor;

namespace Microlight.MicroAudio
{
    // ****************************************************************************************************
    // Custom editor
    // ****************************************************************************************************
    public class MicroAudio_Editor : Editor
    {
        // ---------- Manager Creation ----------
        [MenuItem("Plugins/Microlight/Micro Audio/Micro Audio Manager")]
        static void AddMicroAudioManagerTopMenu() => AddMicroAudioManager();
        [MenuItem("GameObject/Microlight/Micro Audio/Micro Audio Manager")]
        static void AddMicroAudioManager()
        {
            // Get prefab
            GameObject go = MicrolightAssetUtilities.GetPrefab("MicroAudio");
            if (go == null)
            {
                Debug.LogError("[MicroAudio] Error when instantiating prefab. You can always drag and drop prefab from Prefabs folder.");
                return;
            }

            PrefabUtility.InstantiatePrefab(go);
        }
    }
}