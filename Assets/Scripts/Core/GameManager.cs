using UnityEngine;

public class GameManager : MonoBehaviour
{
    //public static DraggingController DraggingController => Instance._draggingController;
    //DraggingController _draggingController;

    [Header("Controllers")]
    [SerializeField] Phone _phone;
    [SerializeField] CaseSelectorController _caseSelectorController;
    [SerializeField] CasePatternSelectorController _casePatternSelectorController;
    [SerializeField] StickerSelectorController _stickerSelectorController;

    public static Phone Phone => Instance._phone;
    public static bool Interactable;

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        //_draggingController = new DraggingController();
    }

    void Start()
    {
        _phone.Init();
        _caseSelectorController.Init();
        _casePatternSelectorController.Init();
        _stickerSelectorController.Init();

        _phone.SelectDefaultValues();
    }
}
