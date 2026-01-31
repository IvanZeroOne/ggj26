using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static DraggingController DraggingController => Instance._draggingController;
    DraggingController _draggingController;

    public static Phone Phone => Instance._phone;

    [Header("Controllers")]
    [SerializeField] Phone _phone;
    [SerializeField] CaseSelectorController _caseSelectorController;

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        _draggingController = new DraggingController();
    }

    void Start()
    {
        _caseSelectorController.Init();
    }
}
