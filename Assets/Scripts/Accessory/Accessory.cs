using UnityEngine;
using UnityEngine.EventSystems;

public class Accessory : MonoBehaviour
{
    [SerializeField] DraggableObject _draggableObject;
    [SerializeField] Attachable _attachable;

    Camera _camera;
    Collider _attachableCollider;

    const float MAX_RAY_DISTANCE = 1000f;

    void Start()
    {
        _draggableObject.Init();
        _camera = Camera.main;
        _attachableCollider = _attachable.GetComponent<Collider>();

        _draggableObject.DragStepEvent += DragStep;
        _draggableObject.DragStartedEvent += StartedDragging;
        _draggableObject.DragEndedEvent += EndedDragging;
    }

    void StartedDragging()
    {
        Debug.Log("Started Dragging");
    }

    void EndedDragging(PointerEventData pointerData)
    {
        Ray ray = _camera.ScreenPointToRay(pointerData.position);
        if (Physics.Raycast(ray, out RaycastHit hit, MAX_RAY_DISTANCE))
        {
            if (hit.collider.TryGetComponent(out AttachmentHolder holder))
            {
                transform.position = hit.point;
                PositionAttachment(holder);
                holder.AttachAttachment(_attachable);
            }
        }
    }

    void DragStep(PointerEventData pointerData)
    {
        Ray ray = _camera.ScreenPointToRay(pointerData.position);
        if (Physics.Raycast(ray, out RaycastHit hit, MAX_RAY_DISTANCE))
        {
            if (hit.collider.TryGetComponent(out AttachmentHolder holder))
            {
                transform.position = hit.point;
                PositionAttachment(holder);
            }
        }
    }

    void PositionAttachment(AttachmentHolder holder)
    {
        Vector3 myPos = _attachable.Parent.position;
        Quaternion myRot = _attachable.Parent.rotation;

        Vector3 otherPos = holder.Parent.position;
        Quaternion otherRot = holder.Parent.rotation;
        Collider otherCol = holder.GetComponent<Collider>();

        const float skin = 0.01f;

        _attachableCollider.enabled = true;
        if (Physics.ComputePenetration(
                otherCol, otherPos, otherRot,
                _attachableCollider, myPos, myRot,
                out Vector3 dir, out float dist))
        {
            Debug.Log($"dir: {dir}, dist: {dist}");
            Vector3 delta = -dir * (dist + skin);
            _attachable.Parent.position += delta;
        }
        _attachableCollider.enabled = false;
    }
}
