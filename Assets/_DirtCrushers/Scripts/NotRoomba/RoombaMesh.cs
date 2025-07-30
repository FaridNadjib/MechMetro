using UnityEngine;

// Romba Mesh should always try to face in the direction we are trying to go, even if external forces are spinning it.
class RoombaMesh : MonoBehaviour
{
    PlayerControlledEntity _playerControlledEntity;

    private void Start()
    {
        _playerControlledEntity = GetComponentInParent<PlayerControlledEntity>();
    }

    void FixedUpdate()
    {
        if (_playerControlledEntity == null)
        {
            _playerControlledEntity = GetComponentInParent<PlayerControlledEntity>();
            if (_playerControlledEntity == null)
            {
                Debug.LogWarning("No PlayerControlledEntity found on parent for RoombaMesh");
                return;
            }
        }

        // Align the mesh's yaw rotation with the forward vector of the player-controlled entity
        Vector3 forward = _playerControlledEntity.InputAdjustedForwardVector.normalized;
        if (forward.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(forward, Vector3.up);
            float rotationY = (Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f)).eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
        }
        else
        {
            float rotationY = (Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * 5f)).eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
        }
    }
}