using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class MechSpeedBoost : MechAbility
{
    [Header("Boost Settings")]
    public float boostForce = 20f;

    public LayerMask groundLayer;
    public float groundCheckDistance = 0.2f;

    private Rigidbody rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        if (activationKey == KeyCode.None)
            activationKey = KeyCode.LeftShift;
    }

    protected override void ActivateAbility()
    {
        if (playerControlledEntity == null || !mechRef.IsGroundedByWheels(groundLayer, groundCheckDistance)) return;
        Vector3 boostDirection = playerControlledEntity.InputAdjustedForwardVector.normalized;
        rb.AddForce(boostDirection * boostForce, ForceMode.Impulse);
        StartCooldown();
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + 0.1f, groundLayer);
    }
}
