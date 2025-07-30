using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class GearGate : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 540f;
    [SerializeField] Transform[] gears;
    Tweener[] gearTweeners;
    [SerializeField] Transform triggerObject;
    [SerializeField] float startDelay = 1f;

    [SerializeField] float startRotationDelayStep = 0.5f;
    [SerializeField] Vector3 endValue = new Vector3 (0f, 0f, 360f);
    [SerializeField] float rotationDuration = 1f;
    [SerializeField] RotateMode rotateMode = RotateMode.FastBeyond360;
    [SerializeField] Ease ease = Ease.Linear;
    [SerializeField] int loops = 1;


    bool isGearsBlocked;
    public int blockingCollidersAmount;
    public int baseCollidersAmount;

    event Action<bool> OnActivate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitForSeconds(startDelay);
        baseCollidersAmount = blockingCollidersAmount;

        gearTweeners = new Tweener[gears.Length];
        for (int i = 0; i < gears.Length; i++)
        {
            gearTweeners[i] = gears[i].DORotate(endValue, rotationDuration, rotateMode).SetEase(ease).SetLoops(loops).SetRelative();
            yield return new WaitForSeconds(startRotationDelayStep);
        }

        OnActivate += ActivateBehaviour;
    }

    void ActivateBehaviour(bool activate)
    {
        if(activate)
        {
            triggerObject.transform.DOMoveY(triggerObject.position.y + 5f, 5f);
            foreach(var t in gearTweeners)
            {
                t.Pause();
            }
        }
        else
        {
            foreach (var t in gearTweeners)
            {
                t.Play();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        blockingCollidersAmount++;
        if(blockingCollidersAmount > baseCollidersAmount && !isGearsBlocked) { isGearsBlocked = true; OnActivate?.Invoke(true); }
        else { isGearsBlocked = false; OnActivate?.Invoke(false); }
    }
    private void OnTriggerExit(Collider other)
    {
        blockingCollidersAmount--;
        if (blockingCollidersAmount > baseCollidersAmount) { isGearsBlocked = true; OnActivate?.Invoke(true); }
        else { isGearsBlocked = false; OnActivate?.Invoke(false); }
    }

    private void OnDestroy()
    {
        OnActivate -= ActivateBehaviour;
    }
}
