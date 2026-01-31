using UnityEngine;

public class AttachmentHolder : MonoBehaviour
{
    [SerializeField] Transform _attachmentsHolder;
    public Collider Collider;

    public void AttachAttachment(Attachable attachable)
    {
        attachable.transform.SetParent(_attachmentsHolder);
    }
}