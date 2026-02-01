using UnityEngine;
using UnityEngine.UI;
using Microlight.MicroAudio;

public class GameManager : MonoBehaviour
{
    //public static DraggingController DraggingController => Instance._draggingController;
    //DraggingController _draggingController;

    [Header("Controllers")]
    [SerializeField] PhoneRotator _phoneRotator;
    [SerializeField] CustomerController _customerController;
    [SerializeField] CaseSelectorController _caseSelectorController;
    [SerializeField] CasePatternSelectorController _casePatternSelectorController;
    [SerializeField] StickerSelectorController _stickerSelectorController;
    [SerializeField] AccessorySelectorController _accessorySelectorController;

    [Header("MainMenu")]
    [SerializeField] Button _quitButton;
    [SerializeField] Button _playButton;
    [SerializeField] GameObject _menu;

    public static bool Interactable;

    [Header("Sounds")]
    public AudioClip DissatisfiedEnd;
    public AudioClip SatisfiedEnd;
    public AudioClip DoorOpen;
    public AudioClip DoorClose;
    public AudioClip Talking;
    public AudioClip UIClicked;
    public AudioClip AmbientMusic;

    public static GameManager Instance { get; private set; }
    public static CustomerController CustomerController => Instance._customerController;

    void Awake()
    {
        Instance = this;
        //_draggingController = new DraggingController();
    }

    void Start()
    {
        _phoneRotator.Init();
        _customerController.Init();
        _caseSelectorController.Init();
        _casePatternSelectorController.Init();
        _stickerSelectorController.Init();
        _accessorySelectorController.Init();

        _playButton.onClick.AddListener(Play);
        _quitButton.onClick.AddListener(Quit);

        MicroAudio.Sounds.PlaySound(AmbientMusic, loop: true);
    }

    void Play()
    {
        _menu.SetActive(false);
        _customerController.SpawnNextCustomer();
    }

    void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
