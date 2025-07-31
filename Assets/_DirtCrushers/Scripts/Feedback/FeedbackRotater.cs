using DG.Tweening;
using UnityEngine;

public class FeedbackRotater : FeedbackController
{
    [Header("Rotation settings:")]
    [SerializeField] private Vector3 targetRotation = new Vector3(0f, 0f, 360f);
    [SerializeField] private RotateMode rotateMode = RotateMode.FastBeyond360;
    [SerializeField] bool setRelative = true;

    protected override void InitializeFeedback()
    {
        if (loops == -1) { waitForFinish = false; }
        tweener = transform.DORotate(targetRotation, duration, rotateMode).SetEase(ease).SetLoops(loops, loopType).SetRelative(setRelative)
            .OnComplete(() => { followUpMethod?.Invoke(); isFinished = true; }).Pause();
    }
}