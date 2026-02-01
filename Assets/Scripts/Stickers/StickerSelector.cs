using UnityEngine;

public class StickerSelector : MonoBehaviour
{
    [SerializeField] InteractableObject _interactableObject;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] StickerSelectorController _controller;
    [SerializeField] StickerSO _stickerSO;
    [Header("Selector")]
    [SerializeField] GameObject _selectedGO;
    [SerializeField] GameObject _hoveredGO;

    public void Init()
    {
        if (_stickerSO.StickerSprite != _spriteRenderer.sprite)
        {
            Debug.LogError($"Sprite mismatch on {gameObject.name}");
        }
        _interactableObject.OnClick += StickerClicked;
        _interactableObject.OnHoverChanged += UpdateState;
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
            _hoveredGO.SetActive(_interactableObject.Hovered);
        }
    }
}
