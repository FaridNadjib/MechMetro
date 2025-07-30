using System.ComponentModel;
using System.Security.Cryptography;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;

// Adapted from: https://discussions.unity.com/t/third-person-camera/792628/4
// Original author: John_Leorid
// Retrieved on: July 29, 2025
//Notes: modified to work with PlayerControlledEntity
class DirtCrusherThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform PositonSnapTo;
    [SerializeField] Vector2 RotationSpeed = new Vector2(1, 1);
    [SerializeField] bool hideCursor = true;
    [SerializeField]PlayerControlledEntity playerControlledEntity;

    [Header("Camera Constraints")]
    [SerializeField] float minVerticalAngle = -30f;
    [SerializeField] float maxVerticalAngle = 60f;

    bool _cursorVisible = true;
    Vector3 _offset;
    Quaternion controlRotation;
    float _rotationX = 0f;
    float _rotationY = 0f;

    void Start()
    {
        if (PositonSnapTo == null)
        {
            Debug.LogError("PositonSnapTo is not assigned in the DirtCrusherThirdPersonCam script.");
            return;
        }
        else
        {
            _offset = transform.position - PositonSnapTo.position;
            transform.position = PositonSnapTo.position + _offset;

            // Initialize rotation values based on current camera orientation
            Vector3 angles = transform.eulerAngles;
            _rotationY = angles.y;
            _rotationX = angles.x;

            playerControlledEntity = PositonSnapTo.parent.GetComponent<PlayerControlledEntity>();
            if (playerControlledEntity != null)
            {
                playerControlledEntity.PlayerControllerForwardVector = transform.forward;
            }
        }

        SetCursorVisibility();
    }

    void FixedUpdate()
    {
        OrbitCameraRotation();
        transform.position = PositonSnapTo.position + _offset;
    }

    void Update()
    {
        ChangeCursorVisibility();

        if (playerControlledEntity != null)
        {
            playerControlledEntity.PlayerControllerForwardVector = transform.forward;
        }
    }

    void OrbitCameraRotation()
    {
        float hAxis = Input.GetAxis("Mouse X");
        float vAxis = -Input.GetAxis("Mouse Y");

        // update rotation values
        _rotationY += hAxis * RotationSpeed.x;
        _rotationX += vAxis * RotationSpeed.y;

        _rotationX = Mathf.Clamp(_rotationX, minVerticalAngle, maxVerticalAngle);
        controlRotation = Quaternion.Euler(_rotationX, _rotationY, 0);

        // rotate the offset vector around the target position
        _offset = controlRotation * Vector3.back * _offset.magnitude;

        transform.rotation = controlRotation;
    }

    void ChangeCursorVisibility()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetMouseButtonDown(2))
        {
            _cursorVisible = !_cursorVisible;
            SetCursorVisibility();
        }
    }

    void SetCursorVisibility()
    {
        Cursor.visible = _cursorVisible;
        Cursor.lockState = _cursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}