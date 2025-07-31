using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public class FeedbackPathMover : FeedbackController
{
    [Header("Path settings:")]
    [SerializeField,Min(2)] int resolution = 20;
    [SerializeField] PathType pathType = PathType.CatmullRom;
    [SerializeField] bool setRelative = true;
    [SerializeField] bool useLookAhead = true;
    [SerializeField,Min(0)] float lookAhead = 0.1f;
    [SerializeField,Tooltip("If none, it searches this object.")] SplineContainer splineContainer;
    protected override void InitializeFeedback()
    {
        if (splineContainer == null)
            splineContainer = GetComponent<SplineContainer>();
        if(splineContainer == null) { splineContainer = GetComponentInChildren<SplineContainer>(); }
        if(splineContainer == null) { Debug.Log("No splinecontainer found for a path moving go.",gameObject); return; }

        var spline = splineContainer.Spline;
        Vector3[] sampledPoints = new Vector3[resolution];

        for (int i = 0; i < resolution; i++)
        {
            float t = (float)i / (resolution - 1);
            sampledPoints[i] = spline.EvaluatePosition(t);
        }

        if (loops == -1) { waitForFinish = false; }
        if(useLookAhead)
        {
            tweener = transform.DOPath(sampledPoints, duration, pathType, PathMode.Full3D).SetEase(ease).SetLoops(loops, loopType).SetLookAt(lookAhead).SetRelative(setRelative)
            .OnComplete(() => { followUpMethod?.Invoke(); isFinished = true; }).Pause();
        }
        else
        {
            tweener = transform.DOPath(sampledPoints, duration, pathType, PathMode.Full3D).SetEase(ease).SetLoops(loops, loopType).SetRelative(setRelative)
            .OnComplete(() => { followUpMethod?.Invoke(); isFinished = true; }).Pause();
        }        
    }
}
