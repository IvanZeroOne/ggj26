using UnityEngine;

public class WorkshopCase : MonoBehaviour, IAttachable
{
    [SerializeField] Collider _collider;
    [SerializeField] Transform _caseVisuals;

    // ---------- IAttachable ----------
    public Collider Collider => _collider;
    public Transform Transform => transform;

    public void AttachAttachment(IAttachmentHolder holder)
    {
        holder.AttachAttachment(this);
    }

    public void FailedAttachment()
    {
        Destroy(gameObject);
    }

    public void PositionAttachment(IAttachmentHolder holder)
    {
        Vector3 myPos = transform.position;
        Quaternion myRot = transform.rotation;

        Vector3 otherPos = holder.Transform.position;
        Quaternion otherRot = holder.Transform.rotation;
        Collider otherCol = holder.Collider;

        const float skin = 0.01f;

        _collider.enabled = true;
        if (Physics.ComputePenetration(
                otherCol, otherPos, otherRot,
                _collider, myPos, myRot,
                out Vector3 dir, out float dist))
        {
            Vector3 delta = -dir * (dist + skin);
            transform.position += delta;
        }
        _collider.enabled = false;
    }
}