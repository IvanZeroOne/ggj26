using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Phone : MonoBehaviour
{
    [Header("Phone")]
    [SerializeField] Transform _caseHolder;
    [SerializeField] Animator _phoneAnimator;
    [Header("Phone Screen")]
    [SerializeField] Image _backgroundImage;
    [SerializeField] Image _wallpaperImage;
    [SerializeField] Image _faceImage;
    [Header("Faces")]
    [SerializeField] Sprite _happyFaceSprite;
    [SerializeField] Sprite _sadFaceSprite;
    [SerializeField] Sprite _neutralFaceSprite;
    [Header("Default Values")]
    [SerializeField] CaseVariantSO _storedCaseVariantSO;
    [SerializeField] CasePatternVariantSO _storedCasePatternVariantSO;
    public List<StickerSO> StoredStickersSO;
    public List<AccessorySO> StoredAccessoriesSO;
    [Header("Speech")]
    [SerializeField] GameObject _bubble;
    [SerializeField] TMP_Text _bubbleText;

    public CaseVariantSO StoredCaseVariant => _storedCaseVariantSO;
    public CasePatternVariantSO StoredCasePatternVariant => _storedCasePatternVariantSO;

    public Transform RotationTransform;

    CaseVisuals _caseVisuals;

    public void Init()
    {
        _caseHolder.DestroyAllChildren();
        _wallpaperImage.sprite = GameManager.CustomerController.CustomerSO.CustomerWallpaper;
        _backgroundImage.color = GameManager.CustomerController.CustomerSO.CustomerBackground;
        _bubble.SetActive(false);
        _faceImage.sprite = _neutralFaceSprite;
    }

    public void EquipCase(CaseVariantSO caseVariantSO)
    {
        _caseHolder.DestroyAllChildren();
        _caseVisuals = Instantiate(caseVariantSO.CaseVisualPrefab, _caseHolder);
        _storedCaseVariantSO = caseVariantSO;
        _caseVisuals.ChangeMaterial(_storedCasePatternVariantSO);
        foreach (StickerVisual stickerVisual in _caseVisuals.Stickers)
        {
            if(StoredStickersSO.Contains(stickerVisual.StickerSO)) stickerVisual.EnableSticker();
            else stickerVisual.DisableSticker();
        }
        foreach (AccessoryVisual accessoryVisual in _caseVisuals.Accessories)
        {
            if(StoredAccessoriesSO.Contains(accessoryVisual.AccessorySO)) accessoryVisual.EnableAccessory();
            else accessoryVisual.DisableAccessory();
        }
    }

    public void EquipPattern(CasePatternVariantSO casePatternVariantSO)
    {
        _caseVisuals.ChangeMaterial(casePatternVariantSO);
        _storedCasePatternVariantSO = casePatternVariantSO;
    }

    public void EquipSticker(StickerSO stickerSO)
    {
        if (StoredStickersSO.Contains(stickerSO))
        {
            StoredStickersSO.Remove(stickerSO);
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
            StoredStickersSO.Add(stickerSO);
            foreach (StickerVisual sticker in _caseVisuals.Stickers)
            {
                if (sticker.StickerSO == stickerSO)
                {
                    sticker.EnableSticker();
                    break;
                }
            }
        }

        if (StoredStickersSO.Contains(stickerSO))
        {
            CustomerSO customer = GameManager.CustomerController.CustomerSO;
            foreach (CustomerAccessoryLikenessData likenessData in customer.CustomerAccessoryLikenessData)
            {
                if (likenessData.AccessoryID == stickerSO.StickerID)
                {
                    int score = likenessData.Rating;
                    Response(score);
                    break;
                }
            }
        }
    }

    public void EquipAccessory(AccessorySO accessorySO)
    {
        bool contained = StoredAccessoriesSO.Contains(accessorySO);
        for (int i = 0; i < StoredAccessoriesSO.Count; i++)
        {
            if (StoredAccessoriesSO[i].AccessoryType == accessorySO.AccessoryType)
            {
                StoredAccessoriesSO.RemoveAt(i);
                i--;
            }
        }
        if (contained == false)
        {
            StoredAccessoriesSO.Add(accessorySO);
        }

        foreach (AccessoryVisual accessory in _caseVisuals.Accessories)
        {
            if (StoredAccessoriesSO.Contains(accessory.AccessorySO))
            {
                accessory.EnableAccessory();
            }
            else
            {
                accessory.DisableAccessory();
            }
        }

        if (StoredAccessoriesSO.Contains(accessorySO))
        {
            CustomerSO customer = GameManager.CustomerController.CustomerSO;
            foreach (CustomerAccessoryLikenessData likenessData in customer.CustomerAccessoryLikenessData)
            {
                if (likenessData.AccessoryID == accessorySO.AccessoryID)
                {
                    int score = likenessData.Rating;
                    Response(score);
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

    public int TotalRating()
    {
        CustomerSO customer = GameManager.CustomerController.CustomerSO;
        int score = 0;
        foreach (StickerSO sticker in StoredStickersSO)
        {
            foreach (CustomerAccessoryLikenessData likenessData in customer.CustomerAccessoryLikenessData)
            {
                if (likenessData.AccessoryID == sticker.StickerID)
                {
                    score += likenessData.Rating;
                    break;
                }
            }
        }
        foreach (AccessorySO accessory in StoredAccessoriesSO)
        {
            foreach (CustomerAccessoryLikenessData likenessData in customer.CustomerAccessoryLikenessData)
            {
                if (likenessData.AccessoryID == accessory.AccessoryID)
                {
                    score += likenessData.Rating;
                    break;
                }
            }
        }
        return score;
    }

    #region Speech
    Tween _speechTween;

    int _positiveResponseCounter;
    int _negativeResponseCounter;
    void Response(int score)
    {
        if (score > 0)
        {
            Speak(GameManager.CustomerController.CustomerSO.PositiveLines[_positiveResponseCounter], "Positive");
            _positiveResponseCounter++;
            if(_positiveResponseCounter == GameManager.CustomerController.CustomerSO.PositiveLines.Count) _positiveResponseCounter = 0;
            _faceImage.sprite = _happyFaceSprite;
        }
        else if (score < 0)
        {
            Speak(GameManager.CustomerController.CustomerSO.NegativeLines[_negativeResponseCounter], "Negative");
            _negativeResponseCounter++;
            if(_negativeResponseCounter == GameManager.CustomerController.CustomerSO.NegativeLines.Count) _negativeResponseCounter = 0;
            _faceImage.sprite = _sadFaceSprite;
        }
    }

    public void Speak(string message, string animName, float speakDuration = 3f)
    {
        _speechTween.Kill();
        _bubble.SetActive(true);
        _bubbleText.text = message;
        SetAnim(animName);
        _speechTween = DOVirtual.DelayedCall(speakDuration, HideSpeak);
    }

    public void HideSpeak()
    {
        _bubble.SetActive(false);
        SetAnim("Idle");
        _faceImage.sprite = _neutralFaceSprite;
    }
    #endregion
}
