using UnityEngine;

/// <summary>
/// Author: Nimroz Baloch
/// Date: 16-Jul-2024
/// Purpose: Player Hit towards the Direction, Add Force, Show Force Indication(LineRenderer)
/// </summary>
public class PlayerMovements : MonoBehaviour
{
    #region Variables
    public float DragLimit = 3.0f;
    public float ForceToAdd = 10f;
    public LineRenderer lineRenderer;
    public Rigidbody PlayerBody;
    public UiController uiController;
    public GameManager GameManager;
    public LevelDefaults LevelDefaults;

    private Camera mainCam;
    private bool isDragging;
    private LineRenderer trajectoryRenderer; // New LineRenderer for the trajectory
    private int maxBounceCount = 2; // Maximum number of bounces to calculate
    #endregion

    #region UnityMethods
    private void OnEnable()
    {
        GameManager = FindObjectOfType<GameManager>();
        uiController = GameManager.uiControllerRef;
        if (uiController != null)
        {
            GameManager.OnGameOver += GameManager.OnGameIsOver;
            GameManager.OnMiss += GameManager.LivesRemoves;
        }
    }
    private void OnDisable()
    {
        if (uiController != null)
        {
            GameManager.OnGameOver -= GameManager.OnGameIsOver;
            GameManager.OnMiss -= GameManager.LivesRemoves;
        }
    }
    private void Start()
    {
        LevelDefaults = GetComponentInParent<LevelDefaults>(true);
        LevelDefaults.isGameOver = false;
        mainCam = Camera.main;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);
        lineRenderer.enabled = false;

        CreateTrajectoryRenderer(); // Initialize the trajectory renderer
    }

    private void Update()
    {
        if (!LevelDefaults.isGameOver)
        {
            if ((Input.GetMouseButtonDown(0) && !isDragging && IsMouseOverPlayer()))
            {
                DragStart();
            }
            if (isDragging)
            {
                Drag();
                ShowTrajectory(); // Show the trajectory during dragging
            }
            if (Input.GetMouseButtonUp(0) && isDragging && !IsMouseOverPlayer())
            {
                DragEnd();
            }
        }

        if (LevelDefaults.Lives < 0 && LevelDefaults.isWin == false/* && LevelDefaults.isGameOver == true*/)
        {
            GameManager.OnGameOver?.Invoke();
            LevelDefaults.Lives = 1;
            Debug.Log("GameOver");
        }
    }
    #endregion

    #region CustomMethods
    Vector3 MousePosition
    {
        get
        {
            Vector3 pos = Input.mousePosition;
            pos.z = Mathf.Abs(mainCam.transform.position.z - transform.position.z); // Adjust z-coordinate
            return mainCam.ScreenToWorldPoint(pos);
        }
    }

    /// <summary>
    /// This Method Starts the Drag enables the lineRenderer
    /// </summary>
    private void DragStart()
    {
        isDragging = true;
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, MousePosition);
    }

    /// <summary>
    /// This Method Calculate the Drag.
    /// </summary>
    private void Drag()
    {
        Vector3 startPosition = lineRenderer.GetPosition(0);
        Vector3 currentPosition = MousePosition;
        Vector3 distance = currentPosition - startPosition;

        if (distance.magnitude <= DragLimit)
        {
            lineRenderer.SetPosition(1, currentPosition);
        }
        else
        {
            Vector3 limitVector = startPosition + (distance.normalized * DragLimit);
            lineRenderer.SetPosition(1, limitVector);
        }

        // Disable line renderer only if mouse returned to the start position and left mouse button is released
        if (IsMouseOverPlayer())
        {
            lineRenderer.enabled = false;
        }
        else
        {
            lineRenderer.enabled = true; // Ensure renderer is enabled during dragging
        }
    }
    /// <summary>
    /// This Method Add Force to the Object. Ends the Drag and disable line Renderer.
    /// </summary>
    private void DragEnd()
    {
        isDragging = false;
        lineRenderer.enabled = false;

        Vector3 startPosition = lineRenderer.GetPosition(0);
        Vector3 currentPosition = lineRenderer.GetPosition(1);
        Vector3 distance = currentPosition - startPosition;
        Vector3 finalForce = distance * ForceToAdd;

        PlayerBody.AddForce(new Vector3(-finalForce.x, 0, -finalForce.z), ForceMode.Impulse);
        GameManager.OnMiss?.Invoke(LevelDefaults.Lives);
        LevelDefaults.Lives -= 1;

        trajectoryRenderer.positionCount = 0; // Clear the trajectory when drag ends
    }

    /// <summary>
    /// Checks if the mouse is over the player
    /// </summary>
    /// <returns> true/false </returns>
    private bool IsMouseOverPlayer()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject == gameObject;
        }
        return false;
    }

    /// <summary>
    /// Creates the LineRenderer for the trajectory
    /// </summary>
    private void CreateTrajectoryRenderer()
    {
        GameObject trajectoryObj = new GameObject("TrajectoryRenderer");
        trajectoryRenderer = trajectoryObj.AddComponent<LineRenderer>();
        trajectoryRenderer.material = new Material(Shader.Find("Sprites/Default"));
        trajectoryRenderer.widthMultiplier = 0.1f;
        trajectoryRenderer.positionCount = 0;
        trajectoryRenderer.startColor = Color.green;
        trajectoryRenderer.endColor = Color.green;
        trajectoryRenderer.startWidth = 0.1f;
        trajectoryRenderer.endWidth = 0.1f;
        trajectoryRenderer.useWorldSpace = true;

        // Set the dotted line pattern
        trajectoryRenderer.textureMode = LineTextureMode.Tile;
        trajectoryRenderer.material.mainTexture = GenerateDottedTexture();
    }

    /// <summary>
    /// Generates a dotted texture for the LineRenderer
    /// </summary>
    /// <returns>Texture2D</returns>
    private Texture2D GenerateDottedTexture()
    {
        Texture2D texture = new Texture2D(2, 2);
        texture.SetPixel(0, 0, Color.black);
        texture.SetPixel(1, 0, Color.white);
        texture.SetPixel(0, 1, Color.clear);
        texture.SetPixel(1, 1, Color.white);
        texture.Apply();
        return texture;
    }

    /// <summary>
    /// This Method Shows the Trajectory in a dotted way using LineRenderer
    /// </summary>
    private void ShowTrajectory()
    {
        Vector3 startPosition = transform.position;
        Vector3 currentPosition = lineRenderer.GetPosition(1);
        Vector3 distance = currentPosition - lineRenderer.GetPosition(0);
        Vector3 initialForce = distance * ForceToAdd;

        int maxSegments = 3; // Reduce the number of segments for a shorter trajectory
        float segmentLength = 0.1f; // Length of each segment
        trajectoryRenderer.positionCount = maxSegments + 1;

        Vector3[] trajectoryPoints = new Vector3[maxSegments + 1];
        trajectoryPoints[0] = startPosition;

        Vector3 currentPoint = trajectoryPoints[0];
        Vector3 currentVelocity = new Vector3(-initialForce.x, 0, -initialForce.z) / PlayerBody.mass;

        for (int i = 1; i <= maxSegments; i++)
        {
            Vector3 nextPoint = currentPoint + currentVelocity * segmentLength;
            RaycastHit hit;
            if (Physics.Raycast(currentPoint, currentVelocity.normalized, out hit, segmentLength))
            {
                if (hit.collider.CompareTag("Wall"))
                {
                    currentVelocity = Vector3.Reflect(currentVelocity, hit.normal);
                    nextPoint = hit.point;
                }
            }
            trajectoryPoints[i] = nextPoint;
            currentPoint = nextPoint;
        }

        trajectoryRenderer.SetPositions(trajectoryPoints);
    }

    #endregion
}