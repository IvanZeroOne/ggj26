using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttachmentHolder : MonoBehaviour
{
    [SerializeField] Transform _parent;
    [SerializeField] Transform _attachmentsHolder;

    public Transform Parent => _parent;

    public void AttachAttachment(Attachable attachable)
    {
        attachable.Parent.transform.SetParent(_attachmentsHolder);
    }
}