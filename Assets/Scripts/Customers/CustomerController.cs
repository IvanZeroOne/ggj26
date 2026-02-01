using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Phone_1 = Phone;

public class CustomerController : MonoBehaviour
{
    [Header("Customer")]
    [SerializeField] Transform _customerHolder;
    [SerializeField] Phone _phonePrefab;
    [SerializeField] List<CustomerSO> _customers;
    [Header("Intro")]
    [SerializeField] Animator _doorAnimator;
    [SerializeField] Transform _phoneTargetPosition;
    [SerializeField] Transform _phoneExitPosition;
    [SerializeField] TMP_Text _phoneName;
    [SerializeField] Button _doneButton;
    public CustomerSO CustomerSO => _customers[_currentCustomerIndex];
    public Phone Phone { get; private set; }

    int _currentCustomerIndex = -1;

    public void Init()
    {
        _customerHolder.DestroyAllChildren();
        _phoneTargetPosition.DestroyAllChildren();
        _phoneExitPosition.DestroyAllChildren();
        _doneButton.onClick.AddListener(IamDone);
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

    void IamDone()
    {
        if(GameManager.Interactable == false) return;

        OutroSequence();
    }

    void OutroSequence()
    {
        GameManager.Interactable = false;
        int score = Phone.TotalRating();
        CustomerSO customer = GameManager.CustomerController.CustomerSO;
        if (score < 0)
        {
            Phone.Speak(customer.DissatisfiedEnd, "Negative");
        }
        else
        {
            Phone.Speak(customer.SatisfiedEnd, "Positive");
        }

        DOVirtual.DelayedCall(3f, () =>
        {
            Phone.MoveTo(_phoneExitPosition, PhoneExited);
        });
    }

    void PhoneArrived()
    {
        GameManager.Interactable = true;
        Phone.Speak(CustomerSO.IntroLine, "Talk");
        //Phone.SetAnim("Idle");
    }

    void PhoneExited()
    {
        SpawnNextCustomer();
    }
}
