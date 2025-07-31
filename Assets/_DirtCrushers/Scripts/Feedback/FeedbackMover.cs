using DG.Tweening;
using UnityEngine;

public class FeedbackMover : FeedbackController
{
    [Header("Moving settings:")]
    [SerializeField] Vector3 endValue = Vector3.one;
    [SerializeField] bool setRelative = true;
    [SerializeField] bool snapping = false;
    protected override void InitializeFeedback()
    {
        if (loops == -1) { waitForFinish = false; }
        tweener = transform.DOMove(endValue,duration,snapping).SetEase(ease).SetLoops(loops, loopType).SetRelative(setRelative)
            .OnComplete(() => { followUpMethod?.Invoke(); isFinished = true; }).Pause();
    }
}
