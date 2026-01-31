using UnityEngine;

public class PhoneCase : MonoBehaviour, IAttachmentHolder
{
    [SerializeField] Collider _collider;
    [SerializeField] Transform _attachmentsHolder;
    [SerializeField] Transform _caseHolder;

    // ---------- IAttachmentHolder ----------
    public Collider Collider => _collider;
    public Transform Transform => transform;
    public Transform AttachmentsHolder => _attachmentsHolder;

    public void AttachAttachment(IAttachable attachable)
    {
        attachable.Transform.SetParent(_attachmentsHolder);
    }
}