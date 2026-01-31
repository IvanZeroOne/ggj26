using UnityEngine;

public interface IAttachable
{
    Collider Collider { get; }
    Transform Transform { get; }

    void PositionAttachment(IAttachmentHolder holder);
    void AttachAttachment(IAttachmentHolder holder);
    void FailedAttachment();
}