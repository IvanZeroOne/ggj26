using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static DraggingController DraggingController => Instance._draggingController;
    DraggingController _draggingController;

    [Header("Controllers")]
    [SerializeField] Phone _phone;
    [SerializeField] CaseSelectorController _caseSelectorController;
    [SerializeField] CasePatternSelectorController _casePatternSelectorController;

    public static Phone Phone => Instance._phone;

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        _draggingController = new DraggingController();
    }

    void Start()
    {
        _phone.Init();
        _caseSelectorController.Init();
        _casePatternSelectorController.Init();
    }
}
