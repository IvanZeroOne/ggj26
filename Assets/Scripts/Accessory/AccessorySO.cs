using UnityEngine;

[CreateAssetMenu(fileName = "AccessorySO", menuName = "Scriptable Objects/Accessory")]
public class AccessorySO : ScriptableObject
{
    public string AccessoryID;
    public GameObject AccessoryPrefab;
}