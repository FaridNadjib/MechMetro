using UnityEngine;

public class Mech : MonoBehaviour
{
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
    /// Checks if any MechWheel attached to this mech is grounded by raycasting down from each wheel's position.
    /// </summary>
    /// <param name="groundLayer">LayerMask for ground detection.</param>
    /// <param name="groundCheckDistance">Distance to check for ground below each wheel.</param>
    /// <returns>True if any wheel is grounded, false otherwise.</returns>
    public bool IsGroundedByWheels(LayerMask groundLayer, float groundCheckDistance)
    {
        var wheels = GetComponentsInChildren<MechWheel>();
        foreach (var wheel in wheels)
        {
            Vector3 origin = wheel.transform.position;
            Vector3 down = -transform.up;
            if (Physics.Raycast(origin, down, groundCheckDistance + 0.1f, groundLayer))
                return true;
        }
        return false;
    }
}
