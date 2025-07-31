using UnityEngine;
using UnityEngine.Events;
using Unity.Mathematics;
using UnityEngine.Splines;

[RequireComponent(typeof(BoxCollider))]
public class SplineGrindTrigger : MonoBehaviour
{
    [Header("Spline Settings")]
    [Tooltip("Reference to the spline that defines the grind path")]
    public SplineContainer splinePath;
    
    [Tooltip("How fast the mech moves along the spline")]
    public float grindSpeed = 10f;
    
    [Header("Entry Settings")]
    [Tooltip("How close to the spline start the mech needs to be to start grinding")]
    public float snapDistance = 1.0f;
    
    [Header("Events")]
    public UnityEvent onGrindStart;
    public UnityEvent onGrindEnd;
    
    private BoxCollider triggerBox;
    private Mech currentMech;
    private float currentSplineDistance = 0f;
    private bool isGrinding = false;
    
    private void Start()
    {
        triggerBox = GetComponent<BoxCollider>();
        triggerBox.isTrigger = true; // Ensure it's set as trigger
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Check if it's a mech
        Mech mech = other.GetComponent<Mech>();
        if (mech == null) return;
        
        // Check if the mech is airborne (not grounded)
        if (mech.IsGroundedByWheels()) return;
        
        // Check if we're close enough to the start of the spline
        Vector3 splineStart = splinePath.transform.position + (Vector3)splinePath.Spline.EvaluatePosition(0f);
        if (Vector3.Distance(mech.transform.position, splineStart) > snapDistance) return;
        
        // Start grinding
        StartGrinding(mech);
    }
    
    private void StartGrinding(Mech mech)
    {
        currentMech = mech;
        isGrinding = true;
        currentSplineDistance = 0f;
        
        // Disable mech physics and control
        Rigidbody rb = mech.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        
        // Snap to start position
        mech.transform.position = splinePath.transform.position + (Vector3)splinePath.Spline.EvaluatePosition(0f);
        
        // Notify the mech that it's grinding
        mech.StartGrinding();
        
        onGrindStart?.Invoke();
    }
    
    private void Update()
    {
        if (!isGrinding || currentMech == null) return;
        
        // Move along spline
        currentSplineDistance += grindSpeed * Time.deltaTime;
        
        // Get position along spline (this is a simplified version - you'll need to implement proper spline logic)
        Vector3 newPosition = GetSplinePosition(currentSplineDistance);
        currentMech.transform.position = newPosition;
        
        // Get spline direction for orientation
        Vector3 splineDirection = GetSplineDirection(currentSplineDistance);
        if (splineDirection != Vector3.zero)
        {
            currentMech.transform.forward = splineDirection;
        }
        
        // Check if we've reached the end
        if (HasReachedEnd())
        {
            EndGrinding();
        }
    }
    
    private void EndGrinding()
    {
        if (currentMech != null)
        {
            // Re-enable physics and control
            Rigidbody rb = currentMech.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            
            // Add some forward momentum
            rb.linearVelocity = currentMech.transform.forward * grindSpeed;
            
            // Notify the mech that grinding is done
            currentMech.EndGrinding();
        }
        
        isGrinding = false;
        currentMech = null;
        onGrindEnd?.Invoke();
    }
    
    private Vector3 GetSplinePosition(float distance)
    {
        // Convert distance to normalized t value (0-1)
        float splineLength = splinePath.Spline.GetLength();
        float t = Mathf.Clamp01(distance / splineLength);
        
        // Get position along spline
        return splinePath.transform.position + (Vector3)splinePath.Spline.EvaluatePosition(t);
    }
    
    private Vector3 GetSplineDirection(float distance)
    {
        // Convert distance to normalized t value (0-1)
        float splineLength = splinePath.Spline.GetLength();
        float t = Mathf.Clamp01(distance / splineLength);
        
        // Get tangent direction at point
        float3 tangent = splinePath.Spline.EvaluateTangent(t);
        return ((Vector3)tangent).normalized;
    }

    private bool HasReachedEnd()
    {
        float splineLength = splinePath.Spline.GetLength();
        return currentSplineDistance >= splineLength;
    }
}
