using UnityEngine;
using DG.Tweening;

public class SpinAnimation : MonoBehaviour
{
    public float spinSpeed = 360f; // Speed of the spin in degrees per second
    private Tweener spinTween; // Reference to the spin tweener

    void OnEnable()
    {
        StartSpinning();
    }

    void OnDisable()
    {
        StopSpinning();
    }

    void StartSpinning()
    {
        // Calculate the duration of one full spin based on the spin speed
        float duration = 360f / spinSpeed;

        // Create a continuous rotation animation without jerks
        spinTween = transform.DORotate(new Vector3(0, 360, 0), duration, RotateMode.FastBeyond360)
                             .SetEase(Ease.Linear)
                             .SetLoops(-1, LoopType.Incremental);
    }

    void StopSpinning()
    {
        if (spinTween != null)
        {
            spinTween.Kill();
        }
    }
}
