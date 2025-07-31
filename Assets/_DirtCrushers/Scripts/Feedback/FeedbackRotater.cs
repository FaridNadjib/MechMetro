using DG.Tweening;
using UnityEngine;

public class FeedbackRotater : FeedbackController
{
    [Header("Rotation settings:")]
    [SerializeField] private Vector3 targetRotation = new Vector3(0f, 0f, 360f);
    [SerializeField] private RotateMode rotateMode = RotateMode.FastBeyond360;

    public override void PlayFeedback()
    {
        if (waitForFinish && !isFinished) return;
        isFinished = false;
        transform.DORotate(targetRotation, duration, rotateMode).SetEase(ease).SetLoops(loops, loopType).SetRelative()
            .OnComplete(() => { followUpMethod?.Invoke(); isFinished = true; });
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (loops == -1) { waitForFinish = false; }
        if (playOnAwake)
        {
            PlayFeedback();
        }
    }

    private void OnDisable()
    {
        transform.DOKill();
    }
}