using DG.Tweening;
using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] PlayerStats stats;

    [SerializeField] TextMeshProUGUI greyScrews;
    [SerializeField] TextMeshProUGUI goldScrews;
    [SerializeField] TextMeshProUGUI timer;

    private void Awake()
    {
        stats.OnGreyScrewsChanged += (info) => { greyScrews.text = info; greyScrews.transform.DOScale(1f, .2f).From(0.8f).SetEase(Ease.OutBounce); };
        stats.OnGoldScrewsChanged += (info) => { goldScrews.text = info; goldScrews.transform.DOScale(1f, .2f).From(0.8f).SetEase(Ease.OutBounce); };
        stats.OnTimerChanged += (info) => { timer.text = info; };
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        greyScrews.transform.DOKill();
        goldScrews.transform.DOKill();
        timer.transform.DOKill();
    }
}
