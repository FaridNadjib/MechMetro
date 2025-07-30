using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Stompers : MonoBehaviour
{
    [SerializeField] Transform[] stompers;
    Tweener[] stomperTweeners;


    [SerializeField] float startDelay = 1f;
    [SerializeField] float startDelayStep = 0.5f;
    [SerializeField] Vector3 endValue = new Vector3(0f, 3f, 0f);
    [SerializeField] float duration = 1f;
    [SerializeField] Ease ease = Ease.Linear;
    [SerializeField] int loops = 1;
    [SerializeField] LoopType loopType = LoopType.Restart;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitForSeconds(startDelay);

        stomperTweeners = new Tweener[stompers.Length];
        for (int i = 0; i < stompers.Length; i++)
        {
            stomperTweeners[i] = stompers[i].transform.DOMove(endValue, duration).SetEase(ease).SetLoops(loops, loopType).SetRelative();
            yield return new WaitForSeconds(startDelayStep);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
