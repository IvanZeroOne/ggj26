using UnityEngine;

public class StickerVisual : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    public StickerSO StickerSO;

    void Start()
    {
        if (StickerSO.StickerSprite != _spriteRenderer.sprite)
        {
            Debug.LogError($"Sprite mismatch on {gameObject.name}");
        }
    }

    public void EnableSticker()
    {
        _spriteRenderer.enabled = true;
    }

    public void DisableSticker()
    {
        _spriteRenderer.enabled = false;
    }
}
