using UnityEngine;

public class CasePatternSelector : MonoBehaviour
{
    [SerializeField] InteractableObject _interactableObject;
    [SerializeField] Transform _modelHolder;
    [SerializeField] CasePatternSelectorController _controller;
    CasePatternVariantSO _casePatternVariantSO;

    public void Init(CasePatternVariantSO casePatternVariantSO)
    {
        _interactableObject.OnClick += CaseClicked;
        _casePatternVariantSO = casePatternVariantSO;
        SpawnVariant();
    }

    void SpawnVariant()
    {
        _modelHolder.DestroyAllChildren();
        Instantiate(_casePatternVariantSO.CaseVisualPrefab, _modelHolder);
    }

    void CaseClicked()
    {
        _controller.SelectCasePatternVariant(_casePatternVariantSO);
    }
}