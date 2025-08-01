
using UnityEngine;

public class Shredder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Shreddable>() != null)
        {
            Destroy(other.gameObject);
        }
    }
}
