using DG.Tweening;
using UnityEngine;

public class FeedbackScaler : FeedbackController
{
    [SerializeField] Vector3 endScale = Vector3.one;
    [SerializeField, Tooltip("Treated as ratio, 1 being 100% of starting scale.")] Vector3 startScale = Vector3.one;

    public override void PlayFeedback()
    {
        if (waitForFinish && !isFinished) return;
        isFinished = false;
        transform.DOScale(endScale, duration).From(startScale).SetEase(ease).SetLoops(loops, loopType)
            .OnComplete(() => { followUpMethod?.Invoke(); isFinished = true; });
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Vector3 tmpScale = transform.localScale;
        tmpScale.x *= startScale.x;
        tmpScale.y *= startScale.y;
        tmpScale.z *= startScale.z;
        startScale = tmpScale;

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
