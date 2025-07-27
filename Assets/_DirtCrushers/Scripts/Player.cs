using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
public class Player : MonoBehaviour
{
    [Header("Movement related:")]
    [SerializeField] float accelerationForce = 5f;
    [SerializeField] float rotationSpeed = 90f;
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform navigationPoint;
    [SerializeField] float groundCheckDistance = 1f;
    [SerializeField] LayerMask groundLayer;

    float rotationInput;
    float acceleration;
    Vector3 currentDirection;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        acceleration = Input.GetAxis("Vertical") * accelerationForce;
        //rb.linearVelocity = transform.forward * Input.GetAxis("Vertical") * accelerationForce;
        transform.Rotate(transform.up, Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime);


        if(Physics.Raycast(navigationPoint.position,-transform.up,out RaycastHit hitInfo, groundCheckDistance, groundLayer))
        {
            currentDirection = Vector3.ProjectOnPlane(transform.forward,hitInfo.normal);
        }
        else
        {
            currentDirection = transform.forward;
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(currentDirection * Input.GetAxis("Vertical") * acceleration);
    }
}
