using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class CollectableSpawner : MonoBehaviour
{
    [Header("Base info:")]
    [SerializeField]    GameObject[] prefabs;
    [SerializeField] bool useForce = true;
    [SerializeField] Vector2 forceRange = new(0.5f, 2f);
    [SerializeField] Transform spawnDirection;
    [Header("Spawn info:")]
    [SerializeField] float initialSpawnDelay = 3f;
    [SerializeField] Vector2 burstSpawnDelayRange = new(0.05f,0.25f);
    [SerializeField] int burstSpawnAmount = 5;
    [SerializeField] float spawnInterval = 20f;
    [SerializeField] int maxSpawnAmount = 15;
    [SerializeField] bool spawnOnAwake = true;
    [SerializeField] Vector2 spawnAngleLimits = new(-50f,50f);
    List<GameObject> spawnedObjects;

    bool isSpawning = false;
    Quaternion initialSpawnerRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnedObjects = new List<GameObject>();
        initialSpawnerRotation = spawnDirection.rotation;
        if (spawnOnAwake) { SpawnObjects(); }
    }

    public void SpawnObjects()
    {
        if(!isSpawning) { StartCoroutine(SpawnRoutine()); }
    }

    IEnumerator SpawnRoutine()
    {
        isSpawning = true;
        yield return new WaitForSeconds(initialSpawnDelay);

        while (isSpawning)
        {
            for (int i = spawnedObjects.Count -1; i >=0; i--)
                if (spawnedObjects[i] == null) spawnedObjects.RemoveAt(i);

            if (spawnedObjects.Count < maxSpawnAmount)
                for (int i = 0; i < burstSpawnAmount; i++)
                {
                    if (spawnedObjects.Count < maxSpawnAmount)
                    {
                        spawnDirection.rotation = initialSpawnerRotation * Quaternion.AngleAxis(Random.Range(spawnAngleLimits.x, spawnAngleLimits.y), spawnDirection.forward);
                        spawnDirection.rotation *= Quaternion.AngleAxis(Random.Range(spawnAngleLimits.x, spawnAngleLimits.y), spawnDirection.up);
                        var tmp = Instantiate(prefabs[Random.Range(0, prefabs.Length)], spawnDirection.position, spawnDirection.rotation);
                        if (useForce && tmp.TryGetComponent(out Rigidbody rb)) { rb.AddForce(spawnDirection.forward * Random.Range(forceRange.x, forceRange.y),ForceMode.Impulse); }
                        spawnedObjects.Add(tmp);
                        yield return new WaitForSeconds(Random.Range(burstSpawnDelayRange.x, burstSpawnDelayRange.y));
                    }  
                }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
