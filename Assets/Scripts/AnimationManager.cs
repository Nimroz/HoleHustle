using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnimationManager : MonoBehaviour
{

    public Button[] buttons; // Array to hold your UI buttons
    public float scaleAmount = 0.1f; // Amount to scale up and down
    public float duration = 0.5f; // Duration of each scale up/down
    public GameObject popUpMessages; // Reference to the PopUpMessages GameObject
    public float moveDuration = 1f; // Duration of the move animation
    public Vector3 moveDistance = new Vector3(0, 100, 0); // Distance to move the text
    public float fadeDuration = 0.5f; // Duration of the fade animation

    // Start is called before the first frame update
    void Start()
    {
        buttons = FindObjectsOfType<Button>(true);
        popUpMessages = FindObjectOfType<PopUpMessages>(true).gameObject;
        foreach (Button button in buttons)
        {
            AnimateButton(button);
        }

    }


    void AnimateButton(Button button)
    {
        RectTransform rectTransform = button.GetComponent<RectTransform>();

        // Create a looped scale animation
        rectTransform.DOScale(Vector3.one + new Vector3(scaleAmount, scaleAmount, 0), duration)
                     .SetLoops(-1, LoopType.Yoyo)
                     .SetEase(Ease.InOutSine);
    }

    public void PlayMissAnimation()
    {
        // Disable all text objects
        foreach (Transform child in popUpMessages.transform)
        {
            child.gameObject.SetActive(false);
        }

        // Get a random child text from PopUpMessages
        int randomIndex = Random.Range(0, popUpMessages.transform.childCount);
        Text randomText = popUpMessages.transform.GetChild(randomIndex).GetComponent<Text>();
        randomText.gameObject.SetActive(true);

        // Reset the text's position and opacity
        RectTransform textTransform = randomText.GetComponent<RectTransform>();
        textTransform.anchoredPosition = Vector3.zero;
        randomText.color = new Color(randomText.color.r, randomText.color.g, randomText.color.b, 1);

        // Play the move and fade animation
        textTransform.DOAnchorPos(moveDistance, moveDuration).SetEase(Ease.OutCubic);
        randomText.DOFade(0, fadeDuration).SetDelay(moveDuration - fadeDuration).OnComplete(() =>
        {
            // Disable the text object after the animation completes
            randomText.gameObject.SetActive(false);
        });
    }

}
