using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CasePatternVariant", menuName = "Scriptable Objects/CasePatternVariant")]
public class CasePatternVariantSO : ScriptableObject
{
    public Material Material;
    public GameObject PatternSelectorPrefab;
}
