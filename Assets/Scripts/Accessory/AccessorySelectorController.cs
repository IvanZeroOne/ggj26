using System.Collections.Generic;
using UnityEngine;

public class AccessorySelectorController : MonoBehaviour
{
    [SerializeField] List<AccessorySelector> _accessorySelectors;

    public void Init()
    {
        for (int i = 0; i < _accessorySelectors.Count; i++)
        {
            _accessorySelectors[i].Init();
        }
    }

    public void SelectAccessory(AccessorySO accessorySO)
    {
        GameManager.CustomerController.Phone.EquipAccessory(accessorySO);
        Microlight.MicroAudio.MicroAudio.Sounds.PlaySound(GameManager.Instance.UIClicked);
        foreach (AccessorySelector selector in _accessorySelectors)
        {
            selector.UpdateState();
        }
    }
}