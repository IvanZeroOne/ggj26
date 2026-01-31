using UnityEngine;
using UnityEngine.UI;

public class Phone : MonoBehaviour
{
    [Header("Phone")]
    [SerializeField] PhoneVisuals _phoneVisuals;
    [SerializeField] Transform _caseHolder;
    [Header("Phone Screen")]
    [SerializeField] Image _backgroundImage;
    [SerializeField] Image _wallpaperImage;
    [SerializeField] Image _faceImage;

    public void Init()
    {
        _caseHolder.DestroyAllChildren();
    }

    public void EquipCase(CaseVariantSO caseVariantSO)
    {
        _caseHolder.DestroyAllChildren();
        Instantiate(caseVariantSO.CaseVisualPrefab, _caseHolder);
    }
}
