using System.Collections.Generic;
using UnityEngine;

public class CasePatternSelectorController : MonoBehaviour
{
    [SerializeField] List<CasePatternSelector> _casePatternSelectors;
    [SerializeField] List<CasePatternVariantSO> _casePatternVariants;

    public void Init()
    {
        if (_casePatternSelectors.Count != _casePatternVariants.Count)
        {
            Debug.LogError("Not the same number of case selectors and variants");
            return;
        }

        for (int i = 0; i < _casePatternSelectors.Count; i++)
        {
            _casePatternSelectors[i].Init(_casePatternVariants[i]);
        }
    }

    public void SelectCasePatternVariant(CasePatternVariantSO casePatternVariantSO)
    {
        GameManager.CustomerController.Phone.EquipPattern(casePatternVariantSO);
        Microlight.MicroAudio.MicroAudio.Sounds.PlaySound(GameManager.Instance.UIClicked);
    }
}