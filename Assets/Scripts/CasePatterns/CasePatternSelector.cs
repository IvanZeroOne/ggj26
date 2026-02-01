using UnityEngine;
using UnityEngine.InputSystem;

public class CasePatternSelector : MonoBehaviour
{
    [SerializeField] InteractableObject _interactableObject;
    [SerializeField] Transform _modelHolder;
    [SerializeField] CasePatternSelectorController _controller;
    CasePatternVariantSO _casePatternVariantSO;

    [Header("Selector")]
    [SerializeField] GameObject _selectedGO;
    [SerializeField] GameObject _hoveredGO;

    Camera _camera;

    public void Init(CasePatternVariantSO casePatternVariantSO)
    {
        _camera = Camera.main;
        _casePatternVariantSO = casePatternVariantSO;
        _interactableObject.OnClick += CaseClicked;
    }

    void Update()
    {
        UpdateState();
    }

    void CaseClicked()
    {
        _controller.SelectCasePatternVariant(_casePatternVariantSO);
    }

    public void UpdateState()
    {
        if (GameManager.CustomerController.Phone == null)
        {
            _selectedGO.SetActive(false);
            _hoveredGO.SetActive(false);
            return;
        }
        if(GameManager.CustomerController.Phone.StoredCasePatternVariant == _casePatternVariantSO)
        {
            _selectedGO.SetActive(true);
            _hoveredGO.SetActive(false);
        }
        else
        {
            _selectedGO.SetActive(false);
            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out CasePatternSelector selector) && selector == this)
                {
                    _hoveredGO.SetActive(true);
                    return;
                }
            }

            _hoveredGO.SetActive(false);
        }
    }
}