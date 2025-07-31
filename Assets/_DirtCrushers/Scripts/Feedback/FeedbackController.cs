using DG.Tweening;
using UnityEngine.Events;
using UnityEngine;

public abstract class FeedbackController : MonoBehaviour
{
    [SerializeField] protected bool playOnAwake = false;
    [SerializeField] protected bool waitForFinish = false;
    [SerializeField] protected float duration = 1f;
    [SerializeField] protected int loops = -1;
    [SerializeField] protected LoopType loopType = LoopType.Yoyo;
    [SerializeField] protected Ease ease = Ease.Linear;
    [SerializeField] protected UnityEvent followUpMethod;

    protected bool isFinished = true;

    [ContextMenu("PlayFeedback")]
    public abstract void PlayFeedback();
}