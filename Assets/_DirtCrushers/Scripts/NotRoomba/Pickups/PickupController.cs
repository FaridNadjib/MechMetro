
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public Transform pickupHolder;
    private Pickupable heldItem;

    private void OnTriggerEnter(Collider other)
    {
        if (heldItem == null)
        {
            Pickupable pickupable = other.GetComponent<Pickupable>();
            if (pickupable != null && pickupable.canBePickedUp)
            {
                heldItem = pickupable;
                heldItem.Pickup(pickupHolder);
            }
        }
    } 

    private void Update()
    {
        if (heldItem != null)
        {
            if (Input.GetMouseButtonDown(0)) // Left-click
            {
                heldItem.OnPrimaryUse();
            }

            if (Input.GetMouseButtonDown(1)) // Right-click
            {
                heldItem.Drop();
                heldItem = null;
            }
        }
    }
}
