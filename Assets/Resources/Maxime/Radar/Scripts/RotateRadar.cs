using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotateRadar : MonoBehaviour
{
    private float lastAngle;
    private bool isDragging = false;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Gestion du toucher tactile
        if (Input.touchSupported && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (IsTouchOnObject(touch.position))
                {
                    BeginInteraction(touch.position);
                    isDragging = true;
                }
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                UpdateRotation(touch.position);
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
        // Gestion de la souris
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (IsMouseOnObject())
                {
                    BeginInteraction(Input.mousePosition);
                    isDragging = true;
                }
            }
            else if (Input.GetMouseButton(0) && isDragging)
            {
                UpdateRotation(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
        }
    }

    private void BeginInteraction(Vector2 inputPosition)
    {
        Vector2 objectPos = GetScreenPosition();
        Vector2 dir = inputPosition - objectPos;
        lastAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    private void UpdateRotation(Vector2 inputPosition)
    {
        Vector2 objectPos = GetScreenPosition();
        Vector2 dir = inputPosition - objectPos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float angleDelta = Mathf.DeltaAngle(lastAngle, angle) * 0.8f;
        transform.Rotate(Vector3.up, -angleDelta);
        lastAngle = angle;
    }

    private Vector2 GetScreenPosition()
    {
        Vector3 objectPos3D = mainCamera.WorldToScreenPoint(transform.position);
        return new Vector2(objectPos3D.x, objectPos3D.y);
    }

    private bool IsTouchOnObject(Vector2 touchPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(touchPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == transform)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsMouseOnObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == transform)
            {
                return true;
            }
        }
        return false;
    }
}



