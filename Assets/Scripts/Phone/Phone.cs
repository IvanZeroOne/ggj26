using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Phone : MonoBehaviour
{
    [Header("Phone")]
    [SerializeField] Transform _caseHolder;
    [SerializeField] Animator _phoneAnimator;
    [Header("Phone Screen")]
    [SerializeField] Image _backgroundImage;
    [SerializeField] Image _wallpaperImage;
    [SerializeField] Image _faceImage;
    [Header("Default Values")]
    [SerializeField] CaseVariantSO _storedCaseVariantSO;
    [SerializeField] CasePatternVariantSO _storedCasePatternVariantSO;
    [SerializeField] List<StickerSO> _storedStickersSO;

    public Transform RotationTransform;

    CaseVisuals _caseVisuals;

    public void Init()
    {
        _caseHolder.DestroyAllChildren();
    }

    public void EquipCase(CaseVariantSO caseVariantSO)
    {
        _caseHolder.DestroyAllChildren();
        _caseVisuals = Instantiate(caseVariantSO.CaseVisualPrefab, _caseHolder);
        _storedCaseVariantSO = caseVariantSO;
        _caseVisuals.ChangeMaterial(_storedCasePatternVariantSO);
        foreach (StickerVisual stickerVisual in _caseVisuals.Stickers)
        {
            if(_storedStickersSO.Contains(stickerVisual.StickerSO)) stickerVisual.EnableSticker();
            else stickerVisual.DisableSticker();
        }
    }

    public void EquipPattern(CasePatternVariantSO casePatternVariantSO)
    {
        _caseVisuals.ChangeMaterial(casePatternVariantSO);
        _storedCasePatternVariantSO = casePatternVariantSO;
    }

    public void EquipSticker(StickerSO stickerSO)
    {
        if (_storedStickersSO.Contains(stickerSO))
        {
            _storedStickersSO.Remove(stickerSO);
            foreach (StickerVisual sticker in _caseVisuals.Stickers)
            {
                if (sticker.StickerSO == stickerSO)
                {
                    sticker.DisableSticker();
                    break;
                }
            }
        }
        else
        {
            _storedStickersSO.Add(stickerSO);
            foreach (StickerVisual sticker in _caseVisuals.Stickers)
            {
                if (sticker.StickerSO == stickerSO)
                {
                    sticker.EnableSticker();
                    break;
                }
            }
        }
    }

    public void SelectDefaultValues()
    {
        EquipCase(_storedCaseVariantSO);
    }

    public void MoveTo(Transform moveToTransform, Action onArrived)
    {
        Vector3[] path = new[]
        {
            transform.position,
            moveToTransform.position
        };

        transform.DOKill();
        transform.DOPath(path, 0.8f, PathType.CatmullRom)
            .SetSpeedBased(true)
            .SetLookAt(0f)
            .SetEase(Ease.Linear)
            .SetOptions(AxisConstraint.None, AxisConstraint.X | AxisConstraint.Z)
            .OnComplete(() =>
            {
                transform.rotation = moveToTransform.rotation;
                onArrived?.Invoke();
            });
    }

    public void SetAnim(string animName)
    {
        _phoneAnimator.SetTrigger(animName);
    }
}
