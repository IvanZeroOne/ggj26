using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PhoneRotator : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerDownHandler, IPointerClickHandler
{
    [SerializeField] Image _image;

    Camera _camera;
    float _dragStartPointerX;
    Quaternion _dragStartPhoneRotation;
    float _clickTimer;

    const float CLICK_TIME = 0.25f;

    public void Init()
    {
        _camera = Camera.main;
    }

    protected void Update()
    {
        _clickTimer -= Time.deltaTime;
    }

    public void Disable()
    {
        _image.enabled = false;
    }

    public void Enable()
    {
        _image.enabled = true;
    }

    #region Dragging

    public void OnBeginDrag(PointerEventData pointerData)
    {
        if(GameManager.Interactable == false) return;
        _dragStartPointerX = pointerData.position.x;
        _dragStartPhoneRotation = GameManager.CustomerController.Phone.RotationTransform.localRotation;
    }

    public void OnDrag(PointerEventData pointerData)
    {
        if(GameManager.Interactable == false) return;
        // Rotate phone based on how much of the screen width has been dragged (X axis).
        // 30% of screen width => 180 degrees, scaled linearly (e.g. 15% => 90 degrees).
        float screenWidth = Screen.width;
        float targetPixelsForFullTurn = screenWidth * 0.30f;

        float draggedPixelsX = pointerData.position.x - _dragStartPointerX;
        float t = (targetPixelsForFullTurn <= 0.0001f) ? 0f : (draggedPixelsX / targetPixelsForFullTurn);

        //float yAngleDelta = Mathf.Clamp(t, -1f, 1f) * 180f; // To clamp how much it can be rotated in one
        float yAngleDelta = t * -180f;

        GameManager.CustomerController.Phone.RotationTransform.localRotation = _dragStartPhoneRotation * Quaternion.Euler(0f, yAngleDelta, 0f);
    }

    #endregion

    public void OnPointerDown(PointerEventData eventData)
    {
        _clickTimer = CLICK_TIME;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(GameManager.Interactable == false) return;
        if(_clickTimer < 0f) return;
        Ray ray = _camera.ScreenPointToRay(eventData.position);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            InteractableObject interactableObject = hit.collider.GetComponent<InteractableObject>();
            if (interactableObject != null && interactableObject.Interactable)
            {
                interactableObject.SimulateClick();
                return;
            }
        }
    }
}