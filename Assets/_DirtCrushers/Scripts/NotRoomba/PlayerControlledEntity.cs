using Unity.VisualScripting;
using UnityEngine;

// this component is used as a storage for data defined by the player controller that can be then reused in calculations
class PlayerControlledEntity : MonoBehaviour
{
    public Vector3 PlayerControllerForwardVector;
    public Vector3 InputAdjustedForwardVector; // a combination of the PlayerControllerForwardVector and the input direction
    public Vector3 InputAdjustedRightVector; // a combination of the PlayerControllerForwardVector and the input direction

    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        float rightInput = Input.GetAxis("Horizontal");

        if (Mathf.Approximately(forwardInput, 0f) && Mathf.Approximately(rightInput, 0f))
        {
            InputAdjustedForwardVector = PlayerControllerForwardVector;
        }
        else
        {
            // Calculate the adjusted forward vector based on input
            Vector3 forward = PlayerControllerForwardVector.normalized;
            Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;

            InputAdjustedRightVector = right * forwardInput + (-forward) * rightInput;
            InputAdjustedForwardVector = forward * forwardInput + right * rightInput;

            if (InputAdjustedForwardVector.sqrMagnitude > 0.0001f)
                InputAdjustedForwardVector.Normalize();
        }
    }
}