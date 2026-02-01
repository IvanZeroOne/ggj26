using UnityEngine;

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

    public static bool Interactable;

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

        _customerController.SpawnNextCustomer();
    }
}
