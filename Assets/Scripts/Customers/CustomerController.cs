using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CustomerController : MonoBehaviour
{
    [Header("Customer")]
    [SerializeField] Transform _customerHolder;
    [SerializeField] Phone _phonePrefab;
    [SerializeField] List<CustomerSO> _customers;
    [Header("Intro")]
    [SerializeField] Animator _doorAnimator;
    [SerializeField] Transform _phoneTargetPosition;
    [SerializeField] TMP_Text _phoneName;
    public CustomerSO CustomerSO => _customers[_currentCustomerIndex];
    public Phone Phone { get; private set; }

    int _currentCustomerIndex = -1;

    public void Init()
    {
        _customerHolder.DestroyAllChildren();
        _phoneTargetPosition.DestroyAllChildren();
    }

    public void SpawnNextCustomer()
    {
        _customerHolder.DestroyAllChildren();
        _currentCustomerIndex++;
        Phone = Instantiate(_phonePrefab, _customerHolder);
        Phone.Init();
        Phone.SelectDefaultValues();
        IntroSequence();
        _phoneName.text = CustomerSO.CharacterName;
    }

    void IntroSequence()
    {
        GameManager.Interactable = false;
        _doorAnimator.SetTrigger("Open");
        Phone.SetAnim("Walk");
        Phone.MoveTo(_phoneTargetPosition, PhoneArrived);
    }

    void PhoneArrived()
    {
        GameManager.Interactable = true;
        Phone.Speak(CustomerSO.IntroLine, "Talk");
        //Phone.SetAnim("Idle");
    }
}
