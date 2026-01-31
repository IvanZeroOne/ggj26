using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PhoneRotator : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerClickHandler
{
    [SerializeField] Transform _phoneRotationTransform;
    [SerializeField] Image _image;

    Camera _camera;
    Vector2 _pointerPreviousPos;
    float _dragStartPointerX;
    Quaternion _dragStartPhoneRotation;

    public void Init()
    {
        _camera = Camera.main;
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
        _pointerPreviousPos = pointerData.position;
        _dragStartPointerX = pointerData.position.x;
        _dragStartPhoneRotation = _phoneRotationTransform.localRotation;
    }

    public void OnDrag(PointerEventData pointerData)
    {
        // Rotate phone based on how much of the screen width has been dragged (X axis).
        // 30% of screen width => 180 degrees, scaled linearly (e.g. 15% => 90 degrees).
        float screenWidth = Screen.width;
        float targetPixelsForFullTurn = screenWidth * 0.30f;

        float draggedPixelsX = pointerData.position.x - _dragStartPointerX;
        float t = (targetPixelsForFullTurn <= 0.0001f) ? 0f : (draggedPixelsX / targetPixelsForFullTurn);

        //float yAngleDelta = Mathf.Clamp(t, -1f, 1f) * 180f; // To clamp how much it can be rotated in one
        float yAngleDelta = t * 180f;

        _phoneRotationTransform.localRotation = _dragStartPhoneRotation * Quaternion.Euler(0f, yAngleDelta, 0f);
        _pointerPreviousPos = pointerData.position;
    }

    #endregion

    public void OnPointerClick(PointerEventData eventData)
    {
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