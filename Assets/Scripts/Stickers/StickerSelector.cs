using UnityEngine;

public class StickerSelector : MonoBehaviour
{
    [SerializeField] InteractableObject _interactableObject;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] StickerSelectorController _controller;
    [SerializeField] StickerSO _stickerSO;

    public void Init()
    {
        if (_stickerSO.StickerSprite != _spriteRenderer.sprite)
        {
            Debug.LogError($"Sprite mismatch on {gameObject.name}");
        }
        _interactableObject.OnClick += StickerClicked;
    }

    void StickerClicked()
    {
        _controller.SelectSticker(_stickerSO);
    }
}
