using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

[SelectionBase]
public class Player : MonoBehaviour
{
    [Header("Movement related:")]
    [SerializeField] float accelerationForce = 5f;
    [SerializeField] float rotationSpeed = 90f;
    [SerializeField,Range(0f,30f)] float rotationAlignSpeed = 0.02f;
    [SerializeField] float gravityMultiplier = -9.81f;
    [SerializeField] float jumpForce = 20f;
    [SerializeField] float jumpBlockDuration = 0.3f;
    [SerializeField] float boostForce = 10f;
    [SerializeField] float boostDuration = 0.5f;
    [SerializeField] float boostBlockDuration = 0.2f;
    public bool isBoosting = false;

    [Header("Collision related:")]
    [SerializeField] float collisionSpeedThreshold = 4f;
    [SerializeField,Min(0.1f)] float collisionRecoverDuration = 0.5f;

    Rigidbody rb;
    [Header("Ground checks:")]
    [SerializeField] Transform navigationPoint;
    [SerializeField] Vector3 groundCheckOffset = new Vector3(0f,-0.1f,0f);
    [SerializeField] float groundCheckDistance = 1f;
    [SerializeField] float skateSplineCheckDistance = 0.35f;
    [SerializeField] LayerMask groundLayer;

    float rotationInput;
    float acceleration;
    Vector3 currentDirection;

    public bool movementBlocked = false;
    bool isSkating = false;
    SplineContainer currentSkateSpline;
    public float startSplineRatio;
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
        if (movementBlocked) return;

        acceleration = Input.GetAxis("Vertical") * accelerationForce;

        transform.Rotate(transform.up, Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime);

        if (isBoosting) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            var cols = Physics.OverlapSphere(transform.position + groundCheckOffset, skateSplineCheckDistance, groundLayer);
            if (cols != null && cols.Length > 0)
            {
                foreach (var col in cols)
                {
                    if (col.gameObject.TryGetComponent(out SkateSpline skateSpline))
                    {
                        isSkating = true;
                        currentSkateSpline = skateSpline.Path;
                        startSplineRatio = GetClosestPositionOnSpline();
                        break;
                    }
                }
            }

        }
        if(isSkating && currentSkateSpline != null)
        {
            if(Input.GetKeyUp(KeyCode.E))
            {
                isSkating = false;
            }
            //rb.linearVelocity = currentSkateSpline.EvaluatePosition(0f);
            transform.position = currentSkateSpline.EvaluatePosition(startSplineRatio);
            rb.position = transform.position;
            rb.linearVelocity = Vector3.zero;
        }
        else if (Physics.Raycast(navigationPoint.position,-transform.up,out RaycastHit hitInfo, groundCheckDistance, groundLayer))
        {
            currentDirection = Vector3.ProjectOnPlane(transform.forward,hitInfo.normal);

            if(Input.GetKeyDown(KeyCode.Space))
            {
                if (!movementBlocked) StartCoroutine(MovementBlockedRoutine(jumpBlockDuration));
                rb.AddForce(Vector3.up * jumpForce,ForceMode.Impulse);
            }else if (Input.GetKeyDown(KeyCode.LeftControl) && !isBoosting)
            {
                if (!movementBlocked) StartCoroutine(MovementBlockedRoutine(boostBlockDuration));
                StartCoroutine(BoostRoutine());
            }
            else
            {
            }
            rb.linearVelocity = currentDirection * acceleration;
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hitInfo.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationAlignSpeed);
            
        } 
        else
        {
            currentDirection = transform.forward;
            if(rb.linearVelocity.y < -0.1f) { rb.AddForce(Vector3.down *gravityMultiplier); }
        }
        
        //transform.forward = Vector3.Slerp(transform.forward, rb.linearVelocity, rotationAlignSpeed * Time.deltaTime);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rb.linearVelocity), rotationAlignSpeed);
    }
    IEnumerator BoostRoutine()
    {
        isBoosting = true;
        rb.AddForce(transform.forward * boostForce, ForceMode.Impulse);
        float timer = 0f;
        while (timer < boostDuration)
        {
            timer += Time.deltaTime;
            
            yield return null;
        }
        isBoosting = false;
    }
    IEnumerator MovementBlockedRoutine(float duration)
    {
        movementBlocked = true;
        yield return new WaitForSeconds(duration);
        movementBlocked = false;
    }

    float GetClosestPositionOnSpline()
    {
        float closestT = 0f;
        float closestDistance = Mathf.Infinity;
        Vector3 closestPoint = Vector3.zero;

        var spline = currentSkateSpline.Spline;

        for (int i = 0; i <= 100; i++)
        {
            float t = (float)i / 100f;
            Vector3 point = (Vector3)spline.EvaluatePosition(t) + currentSkateSpline.transform.position;
            float dist = Vector3.Distance(transform.position ,point);

            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestT = t;
                closestPoint = point;
            }
        }
        Debug.Log(closestPoint);
        return closestT;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(rb.linearVelocity.magnitude > collisionSpeedThreshold) {if(!movementBlocked) StartCoroutine(MovementBlockedRoutine(collisionRecoverDuration)); }
    }
}
