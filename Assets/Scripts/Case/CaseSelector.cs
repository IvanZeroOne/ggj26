using UnityEngine;
using UnityEngine.InputSystem;

public class CaseSelector : MonoBehaviour
{
    [SerializeField] InteractableObject _interactableObject;
    [SerializeField] Transform _modelHolder;
    [SerializeField] CaseSelectorController _controller;
    [SerializeField] CaseVisuals _caseVisuals;
    CaseVariantSO _caseVariantSO;

    [Header("Selector")]
    [SerializeField] GameObject _selectedGO;
    [SerializeField] GameObject _hoveredGO;

    Camera _camera;

    public void Init(CaseVariantSO caseVariantSO)
    {
        _camera = Camera.main;
        _caseVariantSO = caseVariantSO;
        SpawnVariant();

        _interactableObject.OnClick += CaseClicked;
        _caseVisuals.DisableAllAccessoriesAndStickers();
    }

    void Update()
    {
        UpdateState();
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

    public void UpdateState()
    {
        if (GameManager.CustomerController.Phone == null)
        {
            _selectedGO.SetActive(false);
            _hoveredGO.SetActive(false);
            return;
        }
        if(GameManager.CustomerController.Phone.StoredCaseVariant == _caseVariantSO)
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
                if (hit.collider.TryGetComponent(out CaseSelector selector) && selector == this)
                {
                    _hoveredGO.SetActive(true);
                    return;
                }
            }

            _hoveredGO.SetActive(false);
        }
    }
}