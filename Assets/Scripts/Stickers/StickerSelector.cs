using UnityEngine;
using UnityEngine.InputSystem;

public class StickerSelector : MonoBehaviour
{
    [SerializeField] InteractableObject _interactableObject;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] StickerSelectorController _controller;
    [SerializeField] StickerSO _stickerSO;
    [Header("Selector")]
    [SerializeField] GameObject _selectedGO;
    [SerializeField] GameObject _hoveredGO;

    Camera _camera;

    public void Init()
    {
        if (_stickerSO.StickerSprite != _spriteRenderer.sprite)
        {
            Debug.LogError($"Sprite mismatch on {gameObject.name}");
        }
        _interactableObject.OnClick += StickerClicked;
        _interactableObject.OnHoverChanged += UpdateState;
        _camera = Camera.main;
        UpdateState();
    }

    void Update()
    {
        UpdateState();
    }

    void StickerClicked()
    {
        _controller.SelectSticker(_stickerSO);
    }

    public void UpdateState()
    {
        if (GameManager.CustomerController.Phone == null)
        {
            _selectedGO.SetActive(false);
            _hoveredGO.SetActive(false);
            return;
        }
        if(GameManager.CustomerController.Phone.StoredStickersSO.Contains(_stickerSO))
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
                if (hit.collider.TryGetComponent(out StickerSelector selector) && selector == this)
                {
                    _hoveredGO.SetActive(true);
                    return;
                }
            }

            _hoveredGO.SetActive(false);
        }
    }
}
