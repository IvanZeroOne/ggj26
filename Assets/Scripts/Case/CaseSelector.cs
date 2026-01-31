using UnityEngine;

public class CaseSelector : MonoBehaviour
{
    [SerializeField] InteractableObject _interactableObject;
    [SerializeField] Transform _modelHolder;
    [SerializeField] CaseSelectorController _controller;
    CaseVariantSO _caseVariantSO;

    public void Init(CaseVariantSO caseVariantSO)
    {
        _caseVariantSO = caseVariantSO;
        SpawnVariant();

        _interactableObject.OnClick += CaseClicked;
    }

    void SpawnVariant()
    {
        _modelHolder.DestroyAllChildren();
        Instantiate(_caseVariantSO.CaseVisualPrefab, _modelHolder);
    }

    void CaseClicked()
    {
        _controller.SelectCaseVariant(_caseVariantSO);
    }
}