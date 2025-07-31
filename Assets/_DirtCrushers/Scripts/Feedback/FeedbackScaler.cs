using DG.Tweening;
using UnityEngine;

public class FeedbackScaler : FeedbackController
{
    [Header("Scaling settings:")]
    [SerializeField] Vector3 endScale = Vector3.one;
    [SerializeField, Tooltip("Treated as ratio, 1 being 100% of starting scale.")] Vector3 startScale = Vector3.one;

    protected override void InitializeFeedback()
    {
        Vector3 tmpScale = transform.localScale;
        tmpScale.x *= startScale.x;
        tmpScale.y *= startScale.y;
        tmpScale.z *= startScale.z;
        startScale = tmpScale;

        if (loops == -1) { waitForFinish = false; }

        tweener = transform.DOScale(endScale, duration).From(startScale).SetEase(ease).SetLoops(loops, loopType)
            .OnComplete(() => { followUpMethod?.Invoke(); isFinished = true; }).Pause();
    }
}
