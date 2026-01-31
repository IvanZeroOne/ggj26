using UnityEngine;
using UnityEngine.UI;

public class Phone : MonoBehaviour
{
    [Header("Phone")]
    [SerializeField] Transform _caseHolder;
    [SerializeField] Animator _phoneAnimator;
    [SerializeField] PhoneRotator _phoneRotator;
    [Header("Phone Screen")]
    [SerializeField] Image _backgroundImage;
    [SerializeField] Image _wallpaperImage;
    [SerializeField] Image _faceImage;
    [Header("Default Values")]
    [SerializeField] CaseVariantSO _storedCaseVariantSO;
    [SerializeField] CasePatternVariantSO _storedCasePatternVariantSO;

    CaseVisuals _caseVisuals;

    public void Init()
    {
        _caseHolder.DestroyAllChildren();
        _phoneRotator.Init();
    }

    public void EquipCase(CaseVariantSO caseVariantSO)
    {
        _caseHolder.DestroyAllChildren();
        _caseVisuals = Instantiate(caseVariantSO.CaseVisualPrefab, _caseHolder);
        _storedCaseVariantSO = caseVariantSO;
        _caseVisuals.ChangeMaterial(_storedCasePatternVariantSO);
    }

    public void EquipPattern(CasePatternVariantSO casePatternVariantSO)
    {
        _caseVisuals.ChangeMaterial(casePatternVariantSO);
        _storedCasePatternVariantSO = casePatternVariantSO;
    }

    public void SelectDefaultValues()
    {
        EquipCase(_storedCaseVariantSO);
    }
}
