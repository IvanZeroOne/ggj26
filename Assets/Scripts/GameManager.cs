using UnityEngine;

public class GameManager : MonoBehaviour
{
    public DraggingController DraggingController { get; private set; }

    public static GameManager Instance { get; private set; }

    void Awake()
    {
        Instance = this;

        DraggingController = new DraggingController();
    }
}
