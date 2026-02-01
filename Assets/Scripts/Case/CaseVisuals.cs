using System.Collections.Generic;
using UnityEngine;

public class CaseVisuals : MonoBehaviour
{
    [SerializeField] MeshRenderer _renderer;
    public List<StickerVisual> Stickers;
    public List<AccessoryVisual> Accessories;

    void Awake()
    {
        foreach (StickerVisual sticker in Stickers)
        {
            sticker.DisableSticker();
        }
    }

    public void ChangeMaterial(CasePatternVariantSO casePatternVariantSO)
    {
        _renderer.material = casePatternVariantSO.Material;
    }

    public void DisableAllAccessoriesAndStickers()
    {
        foreach (AccessoryVisual accessory in Accessories)
        {
            accessory.DisableAccessory();
        }

        foreach (StickerVisual sticker in Stickers)
        {
            sticker.DisableSticker();
        }
    }
}