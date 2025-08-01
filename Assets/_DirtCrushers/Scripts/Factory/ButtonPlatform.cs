
using UnityEngine;

public class ButtonPlatform : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform spawnPoint;
    public Transform buttonMesh;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerControlledEntity>() != null)
        {
            if (prefabToSpawn != null && spawnPoint != null)
            {
                Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
            }

            if (buttonMesh != null)
            {
                // optional: add button animation here
            }
        }
    }
}
