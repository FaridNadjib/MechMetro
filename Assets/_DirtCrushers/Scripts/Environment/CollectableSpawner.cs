using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class CollectableSpawner : MonoBehaviour
{
    [Header("Base info:")]
    [SerializeField] private bool useForce = true;
    [SerializeField] private Vector2 forceRange = new(0.5f, 2f);
    [SerializeField] private Transform spawnDirection;

    [Header("Spawn info:")]
    [SerializeField] private float initialSpawnDelay = 3f;
    [SerializeField] private Vector2 burstSpawnDelayRange = new(0.05f, 0.25f);
    [SerializeField] private int burstSpawnAmount = 5;
    [SerializeField] private float spawnInterval = 20f;
    [SerializeField] private int maxSpawnAmount = 15;
    [SerializeField] private bool spawnOnAwake = true;
    [SerializeField] private Vector2 spawnAngleLimits = new(-50f, 50f);

    [Header("Feedback related:")]
    [SerializeField] private AudioClip spawnClip;

    private List<Collectable> spawnedObjects;

    private bool isSpawning = false;
    private Quaternion initialSpawnerRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        spawnedObjects = new List<Collectable>();
        initialSpawnerRotation = spawnDirection.rotation;
        if (spawnOnAwake) { SpawnObjects(); }
    }

    public void SpawnObjects()
    {
        if (!isSpawning) { StartCoroutine(SpawnRoutine()); }
    }

    private IEnumerator SpawnRoutine()
    {
        isSpawning = true;
        yield return new WaitForSeconds(initialSpawnDelay);

        while (isSpawning)
        {
            for (int i = spawnedObjects.Count - 1; i >= 0; i--)
                if (spawnedObjects[i] == null) spawnedObjects.RemoveAt(i);

            if (spawnedObjects.Count < maxSpawnAmount)
            {
                if (spawnClip != null) { AudioManager.Instance.PlaySfxClipPooled(spawnClip, transform.position); }
                for (int i = 0; i < burstSpawnAmount; i++)
                {
                    if (spawnedObjects.Count < maxSpawnAmount)
                    {
                        spawnDirection.rotation = initialSpawnerRotation * Quaternion.AngleAxis(Random.Range(spawnAngleLimits.x, spawnAngleLimits.y), spawnDirection.forward);
                        spawnDirection.rotation *= Quaternion.AngleAxis(Random.Range(spawnAngleLimits.x, spawnAngleLimits.y), spawnDirection.up);
                        //var tmp = Instantiate(prefabs[Random.Range(0, prefabs.Length)], spawnDirection.position, spawnDirection.rotation);
                        var tmp = ObjectPooler.Instance.PoolCollectables.Get();
                        tmp.transform.position = spawnDirection.position;
                        tmp.transform.rotation = spawnDirection.rotation;
                        tmp.OnReleased += CountActiveSpawns;
                        if (useForce && tmp.gameObject.TryGetComponent(out Rigidbody rb)) { rb.AddForce(spawnDirection.forward * Random.Range(forceRange.x, forceRange.y), ForceMode.Impulse); }
                        spawnedObjects.Add(tmp);
                        yield return new WaitForSeconds(Random.Range(burstSpawnDelayRange.x, burstSpawnDelayRange.y));
                    }
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void CountActiveSpawns(Collectable obj)
    {
        obj.OnReleased -= CountActiveSpawns;
        spawnedObjects.Remove(obj);
    }

    private void OnDestroy()
    {
        foreach (var item in spawnedObjects)
        {
            item.OnReleased -= CountActiveSpawns;
        }
    }
}