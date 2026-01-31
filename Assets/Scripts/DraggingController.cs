public class DraggingController
{
    public IDraggable DraggedObject { get; private set; }

    public void SetDraggedObject(IDraggable draggable)
    {
        DraggedObject = draggable;
    }

    public void RemoveDraggedObject()
    {
        DraggedObject = null;
    }
}