using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "Scriptable Objects/Character")]
public class CharacterSO : ScriptableObject
{
    public string CharacterName;
    public List<string> PositiveLines;
    public List<string> NegativeLines;
}