using UnityEngine;

public class BumperMesh : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("How quickly the bumper rotates to face movement direction (higher = faster)")]
    public float rotationSpeed = 15f;
    
    [Tooltip("Minimum input magnitude required to rotate")]
    public float minInputThreshold = 0.1f;
    
    private PlayerControlledEntity playerControlledEntity;

    void Start()
    {
        playerControlledEntity = GetComponentInParent<PlayerControlledEntity>();
        if (playerControlledEntity == null)
        {
            Debug.LogWarning("No PlayerControlledEntity found on BumperMesh parent");
        }
    }

    void Update()
    {
        if (playerControlledEntity == null) return;

        // Get the desired forward direction from player input
        Vector3 targetForward = playerControlledEntity.InputAdjustedForwardVector;
        // targetForward.y = 0f; // Keep rotation only on Y axis
        
        // Calculate target rotation
        Quaternion targetRotation = Quaternion.LookRotation(targetForward, Vector3.up);
        targetRotation.x = transform.parent.transform.rotation.x; // Prevent tilting on X axis
        targetRotation.z = transform.parent.transform.rotation.z; // Prevent tilting on Z axis

        // Smoothly interpolate to target rotation

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
