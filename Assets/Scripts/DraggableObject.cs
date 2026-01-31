using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, IDraggable
{
    [SerializeField] Transform _draggedObject;
    [SerializeField] InteractableObject _interactable;

    const float MAX_RAY_DISTANCE = 1000f;

    Camera _camera;

    void Awake()
    {
        _camera = Camera.main;
    }

    void Start()
    {
        _interactable.OnDragStarted += OnStartedDrag;
        _interactable.OnDragEnded += OnEndedDrag;
        _interactable.OnDragStep += OnDragStep;
    }

    // ---------- Dragging ----------

    void OnStartedDrag()
    {
        GameManager.Instance.DraggingController.SetDraggedObject(this);
    }

    void OnEndedDrag()
    {
        GameManager.Instance.DraggingController.RemoveDraggedObject();
    }

    void OnDragStep(PointerEventData pointerData)
    {
        if (_draggedObject == null) return;

        Ray ray = _camera.ScreenPointToRay(pointerData.position);
        if (Physics.Raycast(ray, out RaycastHit hit, MAX_RAY_DISTANCE))
        {
            _draggedObject.position = hit.point;
            Debug.Log($"Positioning to {hit.point}");
        }
        else
        {
            _draggedObject.position = Vector3.zero;
            Debug.Log("No Raycast");
        }
    }
}