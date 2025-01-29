using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledSpot : MonoBehaviour
{

    public float rotationSpeed = 30f;
    private float targetAngle;

    void Start() {
        targetAngle = transform.eulerAngles.y;
    }

    void Update() {
        float currentAngle = transform.eulerAngles.y;
        if(currentAngle == targetAngle) return;

        float shortestAngle = Mathf.DeltaAngle(currentAngle, targetAngle);
        float angleStep = rotationSpeed * Time.deltaTime;

        if(Mathf.Abs(shortestAngle) > angleStep) {
            float newAngle = currentAngle + Mathf.Sign(shortestAngle) * angleStep;
            transform.rotation = Quaternion.AngleAxis(newAngle, Vector3.up);
        } else {
            transform.rotation = Quaternion.AngleAxis(targetAngle, Vector3.up);
        }
    }

    public void SetTargetAngle(float angle) {
        targetAngle = angle;
    }
}
