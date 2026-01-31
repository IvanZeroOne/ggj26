using UnityEngine;

public class Phone : MonoBehaviour
{
    [SerializeField] PhoneVisuals _phoneVisuals;
    [SerializeField] Transform _caseHolder;

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
