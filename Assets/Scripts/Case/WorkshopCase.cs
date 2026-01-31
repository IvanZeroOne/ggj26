using UnityEngine;

public class WorkshopCase : MonoBehaviour, IAttachable
{
    [SerializeField] Attachable _attachable;
    [SerializeField] Transform _caseVisuals;

    public void AttachAttachment(AttachmentHolder holder)
    {
        holder.AttachAttachment(_attachable);
    }

    public void FailedAttachment()
    {
        Destroy(gameObject);
    }

    public void PositionAttachment(AttachmentHolder holder)
    {
        Vector3 myPos = _attachable.transform.position;
        Quaternion myRot = _attachable.transform.rotation;

        Vector3 otherPos = holder.transform.position;
        Quaternion otherRot = holder.transform.rotation;
        Collider otherCol = holder.Collider;

        const float skin = 0.01f;

        _attachable.Collider.enabled = true;
        if (Physics.ComputePenetration(
                otherCol, otherPos, otherRot,
                _attachable.Collider, myPos, myRot,
                out Vector3 dir, out float dist))
        {
            Vector3 delta = -dir * (dist + skin);
            _attachable.transform.position += delta;
        }
        _attachable.Collider.enabled = false;
    }
}