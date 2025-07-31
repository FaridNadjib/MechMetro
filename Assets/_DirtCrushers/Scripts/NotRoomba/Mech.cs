using UnityEngine;

public class Mech : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("How quickly the mech rotates to face movement direction")]
    public float rotationSpeed = 10f;
    private Rigidbody rb;
    private PlayerControlledEntity playerControlledEntity;

    private bool bIsGrounded = false;
    private bool bIsGrinding = false;

    private BoxCollider GroundCheckBoxCollider;

    private int NumGroundCheckOverlaps = 0;

    /// <summary>
    /// Checks if the mech is upside down by comparing its up vector to world up and optionally raycasting.
    /// </summary>
    /// <param name="distance">Distance to check for ground below the mech (default 1.0f).</param>
    /// <param name="dotThreshold">Dot product threshold to consider as upside down (default -0.7f).</param>
    /// <returns>True if the mech is upside down, false otherwise.</returns>
    public bool IsUpsideDown(float distance = 1.0f, float dotThreshold = -0.7f)
    {
        // Check if the up vector is pointing away from world up
        float dot = Vector3.Dot(transform.up, Vector3.up);
        if (dot > dotThreshold)
            return false;

        // Optionally, raycast in the up direction to see if ground is above
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.up, out hit, distance))
        {
            // If we hit something close above, likely upside down
            return true;
        }

        // If dot is low but nothing above, still consider upside down
        return true;
    }

    /// <summary>
    /// Checks if the mech is grounded using the ground check collider.
    /// </summary>
    /// <returns>True if the ground check collider is overlapping with ground, false otherwise.</returns>
    public bool IsGroundedByWheels()
    {
        return bIsGrounded;
    }

    public void StartGrinding()
    {
        bIsGrinding = true;
        if (playerControlledEntity != null)
        {
            playerControlledEntity.enabled = false;
        }
    }

    public void EndGrinding()
    {
        bIsGrinding = false;
        if (playerControlledEntity != null)
        {
            playerControlledEntity.enabled = true;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerControlledEntity = GetComponent<PlayerControlledEntity>();
        if (playerControlledEntity == null)
        {
            Debug.LogWarning("No PlayerControlledEntity found on Mech");
        }

        GroundCheckBoxCollider = GetComponent<BoxCollider>();
        if (GroundCheckBoxCollider == null)
        {
            Debug.LogWarning("No BoxCollider found on Mech for ground checking");
        }
    }

    void OnTriggerEnter(Collider other)
    {

        Debug.Log("Mech grounded: " + other.gameObject.name);
        NumGroundCheckOverlaps++;
        bIsGrounded = true;
    }

    void OnTriggerExit(Collider other)
    {
        NumGroundCheckOverlaps--;
        if (NumGroundCheckOverlaps <= 0)
        {
            bIsGrounded = false;
        }
        Debug.Log("Mech no longer grounded: " + other.gameObject.name);
    }

    void FixedUpdate()
    {
        if (playerControlledEntity == null) return;
        if (bIsGrinding) return; // Skip normal movement when grinding

        // Only proceed if we're grounded
        if (!IsGroundedByWheels()) return;
        if (IsUpsideDown()) return;

        // Only rotate if there's movement input
        float forwardInput = Input.GetAxis("Vertical");
        float rightInput = Input.GetAxis("Horizontal");
        
        // Get the desired forward direction from player input
        Vector3 targetForward = playerControlledEntity.InputAdjustedForwardVector;
        targetForward.y = 0f; // Keep rotation only on Y axis

        if (targetForward.magnitude > 0.1f)
        {
            // Get current forward direction (projected onto horizontal plane)
            Vector3 currentForward = transform.forward;
            currentForward.y = 0f;
            currentForward.Normalize();

            // Calculate the signed angle between current and target direction
            float signedAngle = Vector3.SignedAngle(currentForward, targetForward, Vector3.up);
            
            // Get the current angular velocity around the up axis
            float currentYawVelocity = Vector3.Dot(rb.angularVelocity, Vector3.up);
            
            // Calculate desired angular velocity based on the angle we need to turn
            float desiredYawVelocity = signedAngle;
            
            // Calculate the difference between desired and current velocity
            float yawVelocityDiff = desiredYawVelocity - currentYawVelocity;
            
            // Apply torque proportional to the velocity difference
            Vector3 torque = Vector3.up * yawVelocityDiff * rotationSpeed;
            rb.AddTorque(torque, ForceMode.Acceleration);
        }
    }
}
