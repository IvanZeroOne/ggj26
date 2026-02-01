using System.Collections.Generic;
using UnityEngine;

public class StickerSelectorController : MonoBehaviour
{
    [SerializeField] List<StickerSelector> _stickerSelectors;

    public void Init()
    {
        for (int i = 0; i < _stickerSelectors.Count; i++)
        {
            _stickerSelectors[i].Init();
        }
    }

    public void SelectSticker(StickerSO stickerSO)
    {
        GameManager.CustomerController.Phone.EquipSticker(stickerSO);
    }
}
