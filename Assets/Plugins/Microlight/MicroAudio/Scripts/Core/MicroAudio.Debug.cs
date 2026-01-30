namespace Microlight.MicroAudio
{
    // ****************************************************************************************************
    // Partial class of MicroAudio, responsible for some of the Debug API calls
    // Tip: Can be safely removed from the project
    // ****************************************************************************************************
    public partial class MicroAudio
    {
        public static int GetMaxNumberOfSpawnedSources() => Instance._maxSoundSources;

        // Pool
        public static int GetAmountOfAvailableMicroSources() => Instance.Pool.AvailableMicroSources;
        public static int GetAmountOfAvailableAudioSources() => Instance.Pool.AvailableAudioSources;
        public static int GetAmountOfAvailableDelays() => Instance.Pool.AvailableDelays;
        public static int GetAmountOfAvailableFades() => Instance.Pool.AvailableFades;

        public static int GetAmountOfSpawnedMicroSources() => Instance.Pool.SpawnedMicroSources;
        public static int GetAmountOfSpawnedAudioSources() => Instance.Pool.SpawnedAudioSources;
        public static int GetAmountOfSpawnedDelays() => Instance.Pool.SpawnedDelays;
        public static int GetAmountOfSpawnedFades() => Instance.Pool.SpawnedFades;

        // Handles
        public static uint GetHandleIDCounter() => Instance.Handles.IdCounter;
        public static int GetAmountOfActiveHandles() => Instance.Handles.ActiveHandlesCount;
    }
}