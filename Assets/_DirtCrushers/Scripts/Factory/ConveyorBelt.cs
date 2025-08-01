
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VHierarchy.Libs;

public class ConveyorBelt : MonoBehaviour
{
    public GameObject tileMesh;
    public float speed = 2.0f;
    public float length = 10.0f;
    public float tileSpacing = 0.1f;

    public float rigidBodyImpulseForce = 10.0f;

    private List<Transform> tiles = new List<Transform>();
    private HashSet<Rigidbody> objectsOnBelt = new HashSet<Rigidbody>();

    void Start()
    {
        if (tileMesh == null)
        {
            Debug.LogError("Conveyor Belt is missing its tile mesh!");
            return;
        }

        float tileLength = tileMesh.transform.localScale.z;
        int numberOfTiles = Mathf.CeilToInt(length / (tileLength + tileSpacing));

        for (int i = 0; i < numberOfTiles; i++)
        {
            GameObject tile = Instantiate(tileMesh, transform);
            float position = i * (tileLength + tileSpacing);
            tile.transform.localPosition = new Vector3(0, 0, position);
            tiles.Add(tile.transform);
        }

        tileMesh.SetActive(false); // hide the original tile mesh
    }

    void Update()
    {
        float tileLength = tileMesh.transform.localScale.z;
        float totalTileLength = tiles.Count * (tileLength + tileSpacing);

        foreach (Transform tile in tiles)
        {
            tile.localPosition += new Vector3(0, 0, speed * Time.deltaTime);

            if (tile.localPosition.z > length)
            {
                tile.localPosition -= new Vector3(0, 0, totalTileLength);
            }
        }


        List<Rigidbody> objectsToRemove = new List<Rigidbody>();
        // apply impulse to all registered rigidbodies
        foreach (var rb in objectsOnBelt)
        {
            if (rb != null)
            {
                rb.AddForce(transform.forward * rigidBodyImpulseForce, ForceMode.Acceleration);
            }
            else
            {
                objectsToRemove.Add(rb);
            }
        }
        // remove null rigidbodies from the set
        foreach (var rb in objectsToRemove)
        {
            objectsOnBelt.Remove(rb);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody)
        {
            objectsOnBelt.Add(other.attachedRigidbody);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody)
        {
            objectsOnBelt.Remove(other.attachedRigidbody);
        }
    }
}
