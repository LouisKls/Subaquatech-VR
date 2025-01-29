using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float radius = 25f;
    public float innerRadius = 5f;
    public float moveDuration = 2f;
    private Vector2 currentPosition;
    private Vector2 targetPosition;
    private float moveTime;

    void Start() {
        currentPosition = new Vector2(transform.position.x, transform.position.z);
        targetPosition = GenerateRandomPointInCircle();
        moveTime = 0f;
    }

    void Update() {
        moveTime += Time.deltaTime;

        currentPosition = Vector2.Lerp(currentPosition, targetPosition, moveTime / moveDuration);
        transform.position = new Vector3(currentPosition.x, transform.position.y, currentPosition.y);

        if(moveTime >= moveDuration) {
            targetPosition = GenerateRandomPointInCircle();
            moveTime = 0f;
        }
    }

    private Vector2 GenerateRandomPointInCircle() {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float distance = Mathf.Sqrt(Random.Range(innerRadius * innerRadius, radius * radius));

        float x = distance * Mathf.Cos(angle);
        float y = distance * Mathf.Sin(angle);

        return new Vector2(x, y);
    }
}
