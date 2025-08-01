using UnityEngine;

public abstract class Pickupable : MonoBehaviour
{
    public bool isPickedUp { get; private set; }
    protected Rigidbody rb;
    private float pickupCooldown = 0.5f;  // Time in seconds before item can be picked up again
    private float cooldownTimer = 0f;
    public bool canBePickedUp => cooldownTimer <= 0f && !isPickedUp;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        //TODO: if we are picked and something is overlapping the trigger (except for the player who picked us up), Drop();
    }

    protected virtual void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public virtual void Pickup(Transform holder)
    {
        isPickedUp = true;
        if (rb != null)
        {
            rb.isKinematic = true;
            // rb.detectCollisions = false;
        }
        transform.SetParent(holder);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public virtual void Drop()
    {
        isPickedUp = false;
        cooldownTimer = pickupCooldown;  // Start the cooldown
        if (rb != null)
        {
            rb.isKinematic = false;
            // rb.detectCollisions = true;
        }
        transform.SetParent(null);
    }

    public abstract void OnPrimaryUse();
    public abstract void OnSecondaryUse();
}
