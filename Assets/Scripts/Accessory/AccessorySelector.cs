using UnityEngine;

public class AccessorySelector : MonoBehaviour
{
    [SerializeField] InteractableObject _interactableObject;
    [SerializeField] GameObject _gameObject;
    [SerializeField] AccessorySelectorController _controller;
    [SerializeField] AccessorySO _accessorySO;

    public void Init()
    {
        if (_accessorySO.AccessoryPrefab.name != _gameObject.name)
        {
            Debug.LogError($"Model mismatch on {gameObject.name}");
        }
        _interactableObject.OnClick += AccessoryClicked;
    }

    void AccessoryClicked()
    {
        _controller.SelectAccessory(_accessorySO);
    }
}