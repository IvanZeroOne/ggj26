using UnityEngine;

public class CaseVisuals : MonoBehaviour
{
    [SerializeField] MeshRenderer _renderer;

    public void ChangeMaterial(CasePatternVariantSO casePatternVariantSO)
    {
        _renderer.material = casePatternVariantSO.Material;
    }
}