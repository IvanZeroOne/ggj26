using UnityEngine;

public class CasePatternSelector : MonoBehaviour
{
    [SerializeField] InteractableObject _interactableObject;
    [SerializeField] Transform _modelHolder;
    [SerializeField] CasePatternSelectorController _controller;
    CasePatternVariantSO _casePatternVariantSO;

    public void Init(CasePatternVariantSO casePatternVariantSO)
    {
        _casePatternVariantSO = casePatternVariantSO;
        SpawnVariant();
        _interactableObject.OnClick += CaseClicked;
    }

    void SpawnVariant()
    {
        _modelHolder.DestroyAllChildren();
        Instantiate(_casePatternVariantSO.PatternSelectorPrefab, _modelHolder);
    }

    void CaseClicked()
    {
        _controller.SelectCasePatternVariant(_casePatternVariantSO);
    }
}