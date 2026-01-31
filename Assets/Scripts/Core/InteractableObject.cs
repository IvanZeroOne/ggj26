using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // Const
    const float CLICK_TIME = 0.25f;

    // Settings
    [Header("InteractableObject")]
    [SerializeField] bool _interactable = true;
    [SerializeField] bool _draggable;
    [SerializeField] bool _activeWhileDragged;
    [SerializeField] Collider _collider;

    // Variables
    public bool Hovered { get; private set; }
    public bool Pressed { get; private set; }
    public bool BeingDragged { get; private set; }
    float _clickTimer;

    public bool Interactable
    {
        get => _interactable;
        set
        {
            _interactable = value;
            if (_collider != null)
            {
                _collider.enabled = _interactable;
            }
        }
    }

    // Events
    public event Action OnHoverChanged;
    public event Action<PointerEventData> OnDragStarted;
    public event Action<PointerEventData> OnDragEnded;
    public event Action<PointerEventData> OnDragStep;
    public event Action OnClick;

    #region Flow
    protected void Awake()
    {
        if (_collider != null)
        {
            _collider.enabled = _interactable;
        }
    }

    protected void Update()
    {
        _clickTimer -= Time.deltaTime;
    }
    #endregion

    #region Clicking
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Interactable == false) return;
        Pressed = true;
        //OnPress?.Invoke();
        _clickTimer = CLICK_TIME;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
        if (Interactable == false) return;
        //OnRelease?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Interactable == false) return;
        if (_clickTimer < 0) return;
        OnClick?.Invoke();
    }

    public void SimulateClick()
    {
        if (Interactable == false) return;
        _clickTimer = CLICK_TIME;
        OnPointerClick(null);
    }
    #endregion

    #region Hovering
    public void OnPointerEnter(PointerEventData eventData)
    {
        Hovered = true;
        OnHoverChanged?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Hovered = false;
        Pressed = false;
        OnHoverChanged?.Invoke();
    }
    #endregion

    #region Dragging
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_draggable == false) return;
        if (Interactable == false) return;
        BeingDragged = true;
        OnDragStarted?.Invoke(eventData);

        if (_activeWhileDragged == false && _collider != null)
        {
            _collider.enabled = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        BeingDragged = false;

        if (Interactable == false) return;
        if (_draggable == false) return;
        OnDragEnded?.Invoke(eventData);
        if (_collider != null)
        {
            _collider.enabled = _interactable;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Interactable == false) return;
        if (_draggable == false) return;
        OnDragStep?.Invoke(eventData);
    }
    #endregion
}