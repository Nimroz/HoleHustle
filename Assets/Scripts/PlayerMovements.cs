using System.Collections;
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

    private AnimationManager animationManager;
    private Camera mainCam;
    private bool isDragging;
    private bool isLifeRemove = false;
    private Vector3 initialPosition;
    private bool hasReachedWinPoint = false;
    private bool hasBeenThrown = false; // New variable to check if the pot has been thrown
    private float stillTime = 0f; // Time the pot has been still
    private float stillThreshold = 1f; // Time threshold to consider the pot as still
    #endregion

    #region UnityMethods
    private void OnEnable()
    {
        GameManager = FindObjectOfType<GameManager>();
        uiController = GameManager?.uiControllerRef;
        LevelDefaults = GetComponentInParent<LevelDefaults>(true);
        mainCam = Camera.main;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);
        lineRenderer.enabled = false;
        initialPosition = transform.position; // Set initial position
        animationManager = FindObjectOfType<AnimationManager>(true);
    }

    private void Update()
    {
        if (transform.position.y > .5f)
        {
            transform.position = new Vector3(transform.position.x, .3f, transform.position.z);
        }

        if (!LevelDefaults.isGameOver)
        {
            if (Input.GetMouseButtonDown(0) && !isDragging && IsMouseOverPlayer())
            {
                DragStart();
            }
            if (isDragging)
            {
                Drag();
            }
            if (Input.GetMouseButtonUp(0) && isDragging && !IsMouseOverPlayer())
            {
                DragEnd();
            }
        }

        if (hasBeenThrown && PlayerBody.velocity.magnitude < 0.1f)
        {
            stillTime += Time.deltaTime;
            if (stillTime > stillThreshold)
            {
                animationManager.PlayMissAnimation();
                ResetPosition();
                stillTime = 0f;
                hasBeenThrown = false; // Reset the thrown flag
            }
        }
        else
        {
            stillTime = 0f;
        }

        if (LevelDefaults.Lives < 0 && !LevelDefaults.isWin)
        {
            GameManager.OnGameOver?.Invoke();
            LevelDefaults.Lives = 1;
            Debug.Log("GameOver");
        }
    }
    #endregion

    #region CustomMethods
    private void DragStart()
    {
        isDragging = true;
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, MousePosition);
        isLifeRemove = true;
    }

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

        lineRenderer.enabled = !IsMouseOverPlayer();
    }

    private void DragEnd()
    {
        isDragging = false;
        lineRenderer.enabled = false;

        Vector3 startPosition = lineRenderer.GetPosition(0);
        Vector3 currentPosition = lineRenderer.GetPosition(1);
        Vector3 distance = currentPosition - startPosition;
        Vector3 finalForce = distance * ForceToAdd;

        PlayerBody.AddForce(new Vector3(-finalForce.x, 0, -finalForce.z), ForceMode.Impulse);
        hasBeenThrown = true; // Mark the pot as thrown
    }

    private void ResetPosition()
    {
        transform.position = initialPosition;
        PlayerBody.velocity = Vector3.zero;
        PlayerBody.angularVelocity = Vector3.zero;
        Debug.Log("Reset to initial position");
    }

    private bool IsMouseOverPlayer()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.collider.gameObject == gameObject;
        }
        return false;
    }

    Vector3 MousePosition
    {
        get
        {
            Vector3 pos = Input.mousePosition;
            pos.z = Mathf.Abs(mainCam.transform.position.z - transform.position.z);
            return mainCam.ScreenToWorldPoint(pos);
        }
    }
    #endregion

    private void Vibrate()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidVibrate();
#elif UNITY_IOS && !UNITY_EDITOR
        IOSVibrate();
#else
        Debug.Log("Vibration triggered! (Not on Android or iOS)");
#endif
    }

    private void AndroidVibrate()
    {
        try
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator"))
                    {
                        if (vibrator.Call<bool>("hasVibrator"))
                        {
                            vibrator.Call("vibrate", 500); // Vibrate for 500 milliseconds
                        }
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Android vibration failed: " + e.Message);
        }
    }

    private void IOSVibrate()
    {
        try
        {
            Handheld.Vibrate(); // This should work for basic vibration on iOS
        }
        catch (System.Exception e)
        {
            Debug.LogError("iOS vibration failed: " + e.Message);
        }
    }



    ///// <summary>
    ///// Creates the LineRenderer for the trajectory
    ///// </summary>
    //private void CreateTrajectoryRenderer()
    //{
    //    GameObject trajectoryObj = new GameObject("TrajectoryRenderer");
    //    trajectoryRenderer = trajectoryObj.AddComponent<LineRenderer>();
    //    trajectoryRenderer.material = new Material(Shader.Find("Sprites/Default"));
    //    trajectoryRenderer.widthMultiplier = 0.1f;
    //    trajectoryRenderer.positionCount = 0;
    //    trajectoryRenderer.startColor = Color.white;
    //    trajectoryRenderer.endColor = Color.white;
    //    trajectoryRenderer.startWidth = 0.1f;
    //    trajectoryRenderer.endWidth = 0.1f;
    //    trajectoryRenderer.useWorldSpace = true;

    //    // Set the dotted line pattern
    //    trajectoryRenderer.textureMode = LineTextureMode.Tile;
    //    trajectoryRenderer.material.mainTexture = GenerateDottedTexture();
    //    trajectoryRenderer.material.SetTextureScale("_MainTex", new Vector2(1, 1));
    //}

    ///// <summary>
    ///// Generates a dotted texture for the LineRenderer
    ///// </summary>
    ///// <returns>Texture2D</returns>
    //private Texture2D GenerateDottedTexture()
    //{
    //    Texture2D texture = new Texture2D(2, 1);
    //    texture.SetPixel(0, 0, Color.white);
    //    texture.SetPixel(1, 0, Color.clear);
    //    texture.Apply();
    //    return texture;
    //}

    ///// <summary>
    ///// This Method Shows the Trajectory in a dotted way using LineRenderer
    ///// </summary>
    //private void ShowTrajectory()
    //{
    //    Vector3 startPosition = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
    //    Vector3 currentPosition = lineRenderer.GetPosition(1);
    //    Vector3 distance = currentPosition - lineRenderer.GetPosition(0);
    //    Vector3 initialForce = distance * ForceToAdd;

    //    int maxSegments = 20; // Increase the number of segments for a longer trajectory
    //    float segmentLength = 0.2f; // Length of each segment
    //    trajectoryRenderer.positionCount = maxSegments + 1;

    //    Vector3[] trajectoryPoints = new Vector3[maxSegments + 1];
    //    trajectoryPoints[0] = startPosition;

    //    // Calculate initial velocity
    //    Vector3 initialVelocity = new Vector3(-initialForce.x, 0, -initialForce.z) / PlayerBody.mass;

    //    Vector3 currentPoint = trajectoryPoints[0];
    //    Vector3 currentVelocity = initialVelocity;

    //    for (int i = 1; i <= maxSegments; i++)
    //    {
    //        Vector3 nextPoint = currentPoint + currentVelocity * segmentLength;
    //        nextPoint.y = 0.5f; // Ensure all points are at a fixed height

    //        RaycastHit hit;
    //        if (Physics.Raycast(currentPoint, currentVelocity.normalized, out hit, segmentLength))
    //        {
    //            if (hit.collider.CompareTag("Wall"))
    //            {
    //                currentVelocity = Vector3.Reflect(currentVelocity, hit.normal);
    //                nextPoint = hit.point;
    //                nextPoint.y = 0.5f; // Ensure the hit point is also at the fixed height
    //            }
    //        }

    //        trajectoryPoints[i] = nextPoint;
    //        currentPoint = nextPoint;
    //    }

    //    trajectoryRenderer.SetPositions(trajectoryPoints);
    //}

}