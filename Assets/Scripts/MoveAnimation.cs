using UnityEngine;
using DG.Tweening;

public class MoveAnimation : MonoBehaviour
{
    public float moveDistance = 2f; // Distance to move in one direction
    public float moveDuration = 2f; // Duration of the movement
    public int direction = 1; // Direction of movement: 1 for positive, -1 for negative
    public float moveSpeed = 1f; // Speed of the movement
    public Vector3 axis = Vector3.right; // Axis of movement (e.g., Vector3.right for horizontal, Vector3.up for vertical)

    private Tweener moveTween;

    void OnEnable()
    {
        StartMoving();
    }

    void OnDisable()
    {
        StopMoving();
    }

    void StartMoving()
    {
        // Calculate the target position based on the current position, direction, and axis
        Vector3 targetPosition = transform.position + axis * direction * moveDistance;
        moveTween = transform.DOMove(targetPosition, moveDuration / moveSpeed)
                             .SetLoops(-1, LoopType.Yoyo)
                             .SetEase(Ease.Linear);
    }

    void StopMoving()
    {
        if (moveTween != null)
        {
            moveTween.Kill();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            direction *= -1;
            StopMoving();
            StartMoving();
        }
    }
}