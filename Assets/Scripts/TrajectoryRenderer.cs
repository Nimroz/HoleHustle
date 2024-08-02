using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour
{
    private LineRenderer trajectoryRenderer;

    private void Awake()
    {
        trajectoryRenderer = GetComponent<LineRenderer>();
    }

    public void ShowTrajectory(Vector3 startPosition, Vector3 initialForce, float mass, float forceMultiplier, int segments = 20)
    {
        // Your trajectory calculation code
        trajectoryRenderer.positionCount = segments + 1;
        Vector3[] trajectoryPoints = new Vector3[segments + 1];
        trajectoryPoints[0] = startPosition;

        Vector3 currentPoint = trajectoryPoints[0];
        Vector3 currentVelocity = initialForce / mass;

        for (int i = 1; i <= segments; i++)
        {
            Vector3 nextPoint = currentPoint + currentVelocity * (forceMultiplier / segments);
            // Add gravity effect or other physics calculations if needed
            currentVelocity += Physics.gravity * (forceMultiplier / segments);
            trajectoryPoints[i] = nextPoint;
            currentPoint = nextPoint;
        }

        trajectoryRenderer.SetPositions(trajectoryPoints);
    }

    public void ClearTrajectory()
    {
        trajectoryRenderer.positionCount = 0;
    }
}
