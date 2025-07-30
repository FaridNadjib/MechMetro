using UnityEngine;

//this is acomponent attached to each "wheel" of the mech, which applies torque based on player input
[RequireComponent(typeof(Rigidbody))]
public class MechWheel : MonoBehaviour
{
    public float torqueAmount = 50f;

    private Rigidbody rb;
    private PlayerControlledEntity playerControlledEntity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerControlledEntity = GetComponentInParent<PlayerControlledEntity>();
        if (playerControlledEntity == null)
        {
            Debug.LogWarning("No PlayerControlledEntity found on parent for MechWheel");
        }
    }

    void FixedUpdate()
    {
        if (playerControlledEntity == null)
            return;

        float forwardInput = Input.GetAxis("Vertical");
        float rightInput = Input.GetAxis("Horizontal");

        // If no input, dampen angular velocity
        if (Mathf.Approximately(forwardInput, 0f) && Mathf.Approximately(rightInput, 0f))
        {
            rb.angularVelocity *= 0.9f;
            return;
        }

        // combine inputs to get the desired torque direction
        Vector3 torqueDir = playerControlledEntity.InputAdjustedRightVector.normalized;

        rb.AddTorque(torqueDir * torqueAmount, ForceMode.Force);
    }
}