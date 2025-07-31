using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] int greyScrewValue = 1;
    [SerializeField] AudioClip collectingClip;
    [SerializeField] ParticleSystem collectionPS;
    bool gotUsed = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !gotUsed)
        {
            GameManager.Instance.PlayerStats.AddGreyScrews(greyScrewValue);
            if (collectingClip != null)
            {
                AudioManager.Instance.PlaySfxClip(collectingClip);
            }
            if(collectionPS != null) { Instantiate(collectionPS, transform.position,Quaternion.identity); }
            gotUsed = true;
            Destroy(gameObject); // ToDO: pool it.
        }
    }
}
