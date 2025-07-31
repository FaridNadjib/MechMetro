using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MechJump : MechAbility
{
    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.2f;

    [Header("Upside Down Recovery")]
    public float upsideDownJumpForce = 8f;
    public float upsideDownTorque = 20f;
    public float torqueDelay = 0.2f;

    private Rigidbody rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        if (activationKey == KeyCode.None)
            activationKey = KeyCode.Space;
    }


    protected override void ActivateAbility()
    {
        if (playerControlledEntity == null) return;
        if (mechRef == null) return;
        if (mechRef.IsUpsideDown())
        {
            // Apply initial upside down jump force
            rb.AddForce(Vector3.up * upsideDownJumpForce, ForceMode.Impulse);
            
            // Start a coroutine to apply torque after a short delay
            StartCoroutine(ApplyRecoveryTorque());
        }
        else
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        StartCooldown();
    }

    private System.Collections.IEnumerator ApplyRecoveryTorque()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(torqueDelay);

        // Calculate the rotation axis (perpendicular to current up vector)
        Vector3 currentUp = transform.up;
        Vector3 desiredUp = Vector3.up;
        Vector3 rotationAxis = Vector3.Cross(currentUp, desiredUp).normalized;

        // Apply torque to rotate the mech back upright
        rb.AddTorque(rotationAxis * upsideDownTorque, ForceMode.Impulse);
    }
}
