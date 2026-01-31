using System.Collections.Generic;
using UnityEngine;

public class CaseSelectorController : MonoBehaviour
{
    [SerializeField] List<CaseSelector> _caseSelectors;
    [SerializeField] List<CaseVariantSO> _caseVariants;

    public void Init()
    {
        if (_caseSelectors.Count != _caseVariants.Count)
        {
            Debug.LogError("Not the same number of case selectors and variants");
        }

        for (int i = 0; i < _caseSelectors.Count; i++)
        {
            _caseSelectors[i].Init(_caseVariants[i]);
        }
    }

    public void SelectCaseVariant(CaseVariantSO caseVariantSO)
    {
        GameManager.Phone.EquipCase(caseVariantSO);
    }
}