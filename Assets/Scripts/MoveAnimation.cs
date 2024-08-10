using UnityEngine;
using DG.Tweening;

public class MoveAnimation : MonoBehaviour
{
    public float moveDistance = 2f; // Distance to move in one direction
    public float moveSpeed = 1f; // Speed of the movement
    public Vector3 axis = Vector3.right; // Axis of movement (e.g., Vector3.right for horizontal, Vector3.up for vertical)
    public int initialDirection = 1; // Initial direction of movement: 1 for positive, -1 for negative

    private int currentDirection; // Current direction of movement
    private Tweener moveTween;

    void OnEnable()
    {
        currentDirection = initialDirection; // Set the direction to the initial direction
        StartMoving();
    }

    void OnDisable()
    {
        StopMoving();
    }

    void StartMoving()
    {
        // Calculate the target position based on the current position, direction, and axis
        Vector3 targetPosition = transform.position + axis * currentDirection * moveDistance;
        moveTween = transform.DOMove(targetPosition, moveDistance / moveSpeed)
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Reverse the direction and restart the movement
            currentDirection *= -1;
            StopMoving();
            StartMoving();
        }
    }
}
