using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MovementAnimationController : MonoBehaviour
{
    private Animator animator;
    private Vector3 lastPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        lastPosition = transform.position;
    }

    void Update()
    {
        float deltaX = Mathf.Abs(transform.position.x - lastPosition.x);
        float deltaZ = Mathf.Abs(transform.position.z - lastPosition.z);

        bool isMoving = (deltaX > 0.001f || deltaZ > 0.001f);
        Debug.Log(isMoving);

        animator.SetBool("IsWalking", isMoving);

        lastPosition = transform.position;
    }
}
