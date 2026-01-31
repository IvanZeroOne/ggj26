using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour
{
    [SerializeField] InteractableObject _interactable;

    Camera _camera;
    IAttachable _attachable;

    public event Action DragStartedEvent;
    public event Action DragEndedEvent;
    public event Action<PointerEventData> DragStepEvent;

    const float MAX_RAY_DISTANCE = 1000f;

    public void Init()
    {
        _interactable.OnDragStarted += OnStartedDrag;
        _interactable.OnDragEnded += OnEndedDrag;
        _interactable.OnDragStep += OnDragStep;

        _camera = Camera.main;
        _attachable = GetComponent<IAttachable>();
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
        if (_attachable != null)
        {
            bool positionedSuccessfully = PositionAttachable(pointerData, out IAttachmentHolder holder);
            if (positionedSuccessfully) _attachable.AttachAttachment(holder);
            else _attachable.FailedAttachment();
        }
        DragEndedEvent?.Invoke();
        GameManager.DraggingController.RemoveDraggedObject();
    }

    void OnDragStep(PointerEventData pointerData)
    {
        if(GameManager.DraggingController.DraggedObject == null) return;
        PositionAttachable(pointerData, out _);
        DragStepEvent?.Invoke(pointerData);
    }

    bool PositionAttachable(PointerEventData pointerData, out IAttachmentHolder holder)
    {
        holder = null;
        if (_attachable == null) return false;
        Ray ray = _camera.ScreenPointToRay(pointerData.position);
        if (Physics.Raycast(ray, out RaycastHit hit, MAX_RAY_DISTANCE))
        {
            if (hit.collider.transform.parent.TryGetComponent(out holder))
            {
                transform.position = hit.point;
                _attachable.PositionAttachment(holder);
                return true;
            }
        }
        return false;
    }
}