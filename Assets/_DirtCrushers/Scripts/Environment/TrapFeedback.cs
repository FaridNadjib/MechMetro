using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

[SelectionBase]
public class TrapFeedback : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] private Transform collisionFeedbackPoint;
    [SerializeField] private Transform chargeFeedbackPoint;
    [SerializeField] private AudioClip collisionClip;
    [SerializeField] private ParticleSystem collisionPS;
    [SerializeField] private AudioClip chargeClip;
    [SerializeField] private ParticleSystem chargePS;
    [SerializeField] private float collisionTriggerDelay = 1.5f;
    [SerializeField] private float damageCheckRadius = 0.5f;
    [SerializeField] private float damageForce = 4f;
    private bool isCollisionChecking = false;

    public void CheckForTrapFeedback()
    {
        if (!isCollisionChecking) { StartCoroutine(CheckingRoutine()); }
    }

    private IEnumerator CheckingRoutine()
    {
        isCollisionChecking = true;

        yield return new WaitForSeconds(collisionTriggerDelay);
        if (collisionClip != null) AudioManager.Instance.PlaySfxClipPooled(collisionClip, collisionFeedbackPoint.position);
        if (collisionPS != null && !collisionPS.isPlaying) { collisionPS.Play(); }
        if (impulseSource != null) impulseSource.GenerateImpulse();
        // ToDO: check for player in range. maybe within timeframe.
        var cols = Physics.OverlapSphere(collisionFeedbackPoint.position, damageCheckRadius);
        bool gotPlayer = false;
        if (cols != null && cols.Length > 0)
        {
            foreach (var col in cols)
            {
                if (col.gameObject.TryGetComponent(out Rigidbody rb))
                {
                    if (!gotPlayer && rb.gameObject.CompareTag("Player"))
                    {
                        rb.AddExplosionForce(damageForce, collisionFeedbackPoint.position, damageCheckRadius, 1f, ForceMode.Impulse);
                        gotPlayer = true;
                    }
                    else
                    {
                        rb.AddExplosionForce(damageForce, collisionFeedbackPoint.position, damageCheckRadius, 1f, ForceMode.Impulse);

                    }
                }
            }
        }

        isCollisionChecking = false;
    }

    public void ChargeTrap()
    {
        if (chargeClip != null) AudioManager.Instance.PlaySfxClipPooled(chargeClip, chargeFeedbackPoint.position);
        if (chargePS != null && !collisionPS.isPlaying) { chargePS.Play(); }
    }
}