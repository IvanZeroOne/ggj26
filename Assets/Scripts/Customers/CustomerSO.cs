using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "Scriptable Objects/Character")]
public class CustomerSO : ScriptableObject
{
    public string CharacterName;
    public string IntroLine;
    public string NoCosmeticsLine;
    public List<string> PositiveLines;
    public List<string> NegativeLines;
    public string SatisfiedEnd;
    public string DissatisfiedEnd;
    public string NeutralEnd;
    public List<CustomerAccessoryLikenessData> CustomerAccessoryLikenessData;
}