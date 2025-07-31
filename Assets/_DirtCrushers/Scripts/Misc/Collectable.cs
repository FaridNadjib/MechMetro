using System;
using UnityEngine;
using UnityEngine.Pool;

public class Collectable : MonoBehaviour
{
    [SerializeField] private int greyScrewValue = 1;
    [SerializeField] private AudioClip collectingClip;
    [SerializeField] private ParticleSystem collectionPS;
    [HideInInspector] public bool GotUsed = false;

    private ObjectPool<Collectable> pool;

    public event Action<Collectable> OnReleased;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !GotUsed)
        {
            GameManager.Instance.PlayerStats.AddGreyScrews(greyScrewValue);
            if (collectingClip != null)
            {
                AudioManager.Instance.PlaySfxClip(collectingClip);
            }
            if (collectionPS != null) { Instantiate(collectionPS, transform.position, Quaternion.identity); }
            GotUsed = true;
            OnReleased?.Invoke(this);
            pool.Release(this);
        }
    }

    public void SetPool(ObjectPool<Collectable> pool) => this.pool = pool;
}