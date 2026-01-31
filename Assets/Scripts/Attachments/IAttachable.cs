using UnityEngine;

public interface IAttachable
{
    void PositionAttachment(AttachmentHolder holder);
    void AttachAttachment(AttachmentHolder holder);
    void FailedAttachment();

    Collider Collider { get; }
    Transform Transform { get; }
}