using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour
{
    [SerializeField] InteractableObject _interactable;

    public event Action DragStartedEvent;
    public event Action<PointerEventData> DragEndedEvent;
    public event Action<PointerEventData> DragStepEvent;

    public void Init()
    {
        _interactable.OnDragStarted += OnStartedDrag;
        _interactable.OnDragEnded += OnEndedDrag;
        _interactable.OnDragStep += OnDragStep;
    }

    // ---------- Dragging ----------

    void OnStartedDrag()
    {
        if(GameManager.DraggingController.DraggedObject != null) return;
        GameManager.DraggingController.SetDraggedObject(this);
        DragStartedEvent?.Invoke();
    }

    void OnEndedDrag(PointerEventData pointerData)
    {
        DragEndedEvent?.Invoke(pointerData);
        GameManager.DraggingController.RemoveDraggedObject();
    }

    void OnDragStep(PointerEventData pointerData)
    {
        if(GameManager.DraggingController.DraggedObject == null) return;
        DragStepEvent?.Invoke(pointerData);
    }
}