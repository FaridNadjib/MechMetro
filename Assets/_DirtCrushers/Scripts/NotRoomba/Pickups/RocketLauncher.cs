
using UnityEngine;
using System.Collections;

public class RocketLauncher : Pickupable
{
    [Header("Rocket Properties")]
    [SerializeField] private float speed = 50f; // how fast this thing flies
    [SerializeField] private float lifetime = 5f; // how long it lives before poofing
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float explosionForce = 1000f;

    [Header("Recoil")]
    [SerializeField] private float recoilImpulse = 10f;
    [SerializeField] private float recoilTorque = 10f;

    private PlayerControlledEntity playerControlledEntity;
    private GameObject owner; // who shot this thing
    private bool hasExploded = false;
    private bool hasBeenLaunched = false;
    private float launchTime;
    private Vector3 launchDirection; // The direction we're flying

    protected override void Awake()
    {
        base.Awake();
        playerControlledEntity = GetComponentInParent<PlayerControlledEntity>();
    }

    public override void OnPrimaryUse()
    {
        // gotta find the player if we don't have it
        if (playerControlledEntity == null)
        {
            playerControlledEntity = GetComponentInParent<PlayerControlledEntity>();
            if (playerControlledEntity == null)
            {
                Debug.LogWarning("No PlayerControlledEntity found for RocketLauncher");
                return;
            }
        }
        
        // keep track of who shot us so we don't blow them up by accident
        owner = playerControlledEntity.gameObject;

        // grab the player's forward direction before we do anything else
        launchDirection = playerControlledEntity.transform.forward;

        // apply some kickback
        ApplyRecoil();

        // disable the pickup trigger so we don't pick it up again mid-flight
        SphereCollider pickupSphere = GetComponent<SphereCollider>();
        if (pickupSphere != null)
        {
            pickupSphere.enabled = false;
        }

        // drop the rocket and get it ready to fly
        Drop();

        // turn off physics so we can fly straight
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        
        // also turn off the main collider for the same reason
        GetComponent<Collider>().enabled = false;

        hasBeenLaunched = true;
        launchTime = Time.time;
    }

    private void ApplyRecoil()
    {
        Mech mech = playerControlledEntity.GetComponent<Mech>();
        if (mech != null)
        {
            Rigidbody mechRb = mech.GetComponent<Rigidbody>();
            if (mechRb != null)
            {
                // push the mech back
                mechRb.AddForce(-launchDirection * recoilImpulse, ForceMode.Impulse);

                // if we're in the air, make it spin
                if (!mech.IsGroundedByWheels())
                {
                    mechRb.AddTorque(-playerControlledEntity.transform.right * recoilTorque, ForceMode.Impulse);
                }
            }
        }
    }

    public override void OnSecondaryUse()
    {
        // nuthin to see here
    }

    private void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        // find everything nearby and make it go boom
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in colliders)
        {
            // if it has a rigidbody, push it
            Rigidbody hitRb = hit.GetComponent<Rigidbody>();
            if (hitRb != null)
            {
                hitRb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        // bye bye rocket
        Destroy(gameObject);
    }

    protected override void Update()
    {
        base.Update(); // this handles the cooldown stuff

        if (!hasBeenLaunched || hasExploded)
        {
            return;
        }

        // check if we've been flying for too long
        if (Time.time > launchTime + lifetime)
        {
            Explode();
            return;
        }

        // move forward
        float distanceThisFrame = speed * Time.deltaTime;
        transform.position += launchDirection * distanceThisFrame;

        // check if we hit anything
        RaycastHit hit;
        // we use a spherecast to make sure we don't miss small things
        if (Physics.SphereCast(transform.position - launchDirection * distanceThisFrame, 0.5f, launchDirection, out hit, distanceThisFrame))
        {
            // make sure we didn't hit the person who shot us
            if (hit.transform.root != owner.transform.root)
            {
                transform.position = hit.point; // move to the impact point
                Explode();
            }
        }
    }
}
