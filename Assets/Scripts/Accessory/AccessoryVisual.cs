using UnityEngine;

public class AccessoryVisual : MonoBehaviour
{
    [SerializeField] GameObject _model;
    public AccessorySO AccessorySO;

    void Start()
    {
        if (AccessorySO.name != _model.name)
        {
            //Debug.LogError($"Model mismatch on {gameObject.name}");
        }
    }

    public void EnableAccessory()
    {
        _model.SetActive(true);
    }

    public void DisableAccessory()
    {
        _model.SetActive(false);
    }
}