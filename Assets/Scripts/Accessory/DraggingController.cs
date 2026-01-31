using UnityEngine;
using UnityEngine.EventSystems;

public class DraggingController : MonoBehaviour
{
    [SerializeField] Transform _draggedObject;
    [SerializeField] InteractableObject _interactable;

    public void Init()
    {
        _interactable.OnDragStarted += OnStartedDrag;
        _interactable.OnDragEnded += OnEndedDrag;
        _interactable.OnDragStep += OnDragStep;
    }

    // ---------- Dragging ----------

    void OnStartedDrag()
    {
        Debug.Log("Started drag");
    }

    void OnEndedDrag()
    {
        Debug.Log("End drag");
    }

    void OnDragStep(PointerEventData pointerData)
    {
        _draggedObject.position = pointerData.position;
    }
}