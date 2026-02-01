using UnityEngine;

public class AccessorySelector : MonoBehaviour
{
    [SerializeField] InteractableObject _interactableObject;
    [SerializeField] AccessorySelectorController _controller;
    [SerializeField] AccessorySO _accessorySO;

    public void Init()
    {
        _interactableObject.OnClick += AccessoryClicked;
    }

    void AccessoryClicked()
    {
        _controller.SelectAccessory(_accessorySO);
    }
}