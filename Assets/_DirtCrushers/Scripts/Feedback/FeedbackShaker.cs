using DG.Tweening;
using UnityEngine;

public class FeedbackShaker : FeedbackController
{
    enum ShakeMode { Position, Rotation, Scale }
    [Header("Shake settings:")]
    [SerializeField] private ShakeMode shakeMode = ShakeMode.Position;
    [SerializeField] Vector3 strength = new Vector3(1f, 1f, 1f);
    [SerializeField] int vibrato = 10;
    [SerializeField] float randomness = 90.0f;
    [SerializeField] bool snapping = false;
    [SerializeField] bool fadeOut = true;
    [SerializeField] ShakeRandomnessMode randomnessMode = ShakeRandomnessMode.Harmonic;
    [SerializeField] bool setRelative = true;

    protected override void InitializeFeedback()
    {
        if (loops == -1) { waitForFinish = false; }

        switch (shakeMode)
        {
            case ShakeMode.Position:
                tweener = transform.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut, randomnessMode)
                    .SetEase(ease).SetLoops(loops, loopType).SetRelative(setRelative).OnComplete(() => { followUpMethod?.Invoke(); isFinished = true; }).Pause();
                break;
            case ShakeMode.Rotation:
                tweener = transform.DOShakeRotation(duration, strength, vibrato, randomness, fadeOut, randomnessMode)
                    .SetEase(ease).SetLoops(loops, loopType).SetRelative(setRelative).OnComplete(() => { followUpMethod?.Invoke(); isFinished = true; }).Pause();
                break;
            case ShakeMode.Scale:
                tweener = transform.DOShakeScale(duration, strength, vibrato, randomness, fadeOut, randomnessMode)
                    .SetEase(ease).SetLoops(loops, loopType).SetRelative(setRelative).OnComplete(() => { followUpMethod?.Invoke(); isFinished = true; }).Pause();
                break;
        }        
    }
}
