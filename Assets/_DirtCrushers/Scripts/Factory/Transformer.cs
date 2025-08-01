
using UnityEngine;

public class Transformer : MonoBehaviour
{
    public Transform outputSpawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        Transformable transformable = other.GetComponent<Transformable>();
        if (transformable != null)
        {
            if (transformable.nextPrefab != null && outputSpawnPoint != null)
            {
                Instantiate(transformable.nextPrefab, outputSpawnPoint.position, outputSpawnPoint.rotation);
            }
            Destroy(other.gameObject);
        }
    }
}
