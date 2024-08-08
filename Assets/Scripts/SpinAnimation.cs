using UnityEngine;
using DG.Tweening;

public class SpinAnimation : MonoBehaviour
{
    public float spinDuration = 2f; // Duration of one full spin
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
        // Create a looped rotation animation
        spinTween = transform.DORotate(new Vector3(0, 360, 0), spinDuration, RotateMode.FastBeyond360)
                             .SetLoops(-1, LoopType.Restart)
                             .SetEase(Ease.Linear);
    }

    void StopSpinning()
    {
        if (spinTween != null)
        {
            spinTween.Kill();
        }
    }
}
