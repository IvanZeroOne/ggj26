using UnityEngine;
using UnityEngine.InputSystem;

public class AccessorySelector : MonoBehaviour
{
    [SerializeField] InteractableObject _interactableObject;
    [SerializeField] AccessorySelectorController _controller;
    [SerializeField] AccessorySO _accessorySO;

    [Header("Selector")]
    [SerializeField] GameObject _selectedGO;
    [SerializeField] GameObject _hoveredGO;

    Camera _camera;

    public void Init()
    {
        _camera = Camera.main;
        _interactableObject.OnClick += AccessoryClicked;
    }

    void Update()
    {
        UpdateState();
    }

    void AccessoryClicked()
    {
        _controller.SelectAccessory(_accessorySO);
    }

    public void UpdateState()
    {
        if (GameManager.CustomerController.Phone == null)
        {
            _selectedGO.SetActive(false);
            _hoveredGO.SetActive(false);
            return;
        }
        if(GameManager.CustomerController.Phone.StoredAccessoriesSO.Contains(_accessorySO))
        {
            _selectedGO.SetActive(true);
            _hoveredGO.SetActive(false);
        }
        else
        {
            _selectedGO.SetActive(false);
            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out AccessorySelector selector) && selector == this)
                {
                    _hoveredGO.SetActive(true);
                    return;
                }
            }

            _hoveredGO.SetActive(false);
        }
    }
}