using DG.Tweening;
using UnityEngine.Events;
using UnityEngine;
using VInspector;
using System.Collections;

public abstract class FeedbackController : MonoBehaviour
{
    [SerializeField] bool playOnAwake = true;
    [SerializeField] float startDelay = 0f;
    [SerializeField] protected bool waitForFinish = false;
    [SerializeField] protected float duration = 1f;
    [SerializeField] protected int loops = -1;
    [SerializeField] protected LoopType loopType = LoopType.Yoyo;
    [SerializeField] protected Ease ease = Ease.Linear;
    [SerializeField] UnityEvent startUpMethod;
    [SerializeField] protected UnityEvent followUpMethod;
    protected Tweener tweener;
    protected bool isFinished = true;

    private void Awake()
    {
        InitializeFeedback();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(startDelay);
        if (playOnAwake)
        {
            PlayFeedback();
        }
    }

    protected abstract void InitializeFeedback();

    [Button("Play Feedback")]
    public virtual void PlayFeedback()
    {
        if (waitForFinish && !isFinished) return;
        isFinished = false;
        startUpMethod?.Invoke();
        tweener.Play();
    }
    [Button("Play Feedback Reinitialized")]
    public virtual void PlayFeedbackReinitialized() 
    {
        transform.DOKill();
        InitializeFeedback();

        if (waitForFinish && !isFinished) return;
        isFinished = false;
        startUpMethod?.Invoke();
        tweener.Play();
    }
    public virtual void PlayFeedback(bool reinitialializeFeedback = false)
    {
        if (reinitialializeFeedback)
        {
            transform.DOKill(); InitializeFeedback();
        }

        if (waitForFinish && !isFinished) return;
        isFinished = false;
        startUpMethod?.Invoke();
        tweener.Play();
    }

    [Button("Stop Feedback")]
    public virtual void StopFeedback()
    {
        tweener.Rewind();
    }

    private void OnDisable()
    {
        transform.DOKill();
    }
}