using UnityEngine;

public class AttachmentHolder : MonoBehaviour
{
    [SerializeField] Transform _attachmentsHolder;
    public Collider Collider;

    public void AttachAttachment(IAttachable attachable)
    {
        attachable.Transform.SetParent(_attachmentsHolder);
    }
}