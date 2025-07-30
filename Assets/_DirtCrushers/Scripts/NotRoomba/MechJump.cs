using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MechJump : MechAbility
{
    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.2f;

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
        if (!IsGrounded()) return;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        StartCooldown();
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + 0.1f, groundLayer);
    }
}
