using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static DraggingController DraggingController => Instance._draggingController;
    DraggingController _draggingController;

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        _draggingController = new DraggingController();
    }

    public List<CasePatternVariantSO> CasePatternVariants;
}
