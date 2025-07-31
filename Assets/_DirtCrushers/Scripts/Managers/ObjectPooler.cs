using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    [Header("Bullet prefabs:")]
    [SerializeField] private SfxSource3D sfxSource3DPrefab;
    [SerializeField] private Collectable screwPrefab;

    [SerializeField] private int defaultPoolSize = 360;
    [SerializeField] private int maxPoolSize = 700;

    // Bullet pools.
    public ObjectPool<SfxSource3D> PoolSfx3D { get; private set; }
    public ObjectPool<Collectable> PoolCollectables { get; private set; }

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        PoolSfx3D = new ObjectPool<SfxSource3D>(CreateSfxSource3D, OnTakeSfxSource3DFromPool, OnReturnSfxSource3DToPool, OnDestroySfxSource3D, true,
            defaultPoolSize, maxPoolSize);
        PoolCollectables = new ObjectPool<Collectable>(CreateCollectable, OnTakeCollectableFromPool, OnReturnCollectableToPool, OnDestroyCollectable, true,
            defaultPoolSize, maxPoolSize);
    }

    #region SfxPool
    private SfxSource3D CreateSfxSource3D()
    {
        SfxSource3D obj = Instantiate(sfxSource3DPrefab, transform.position, Quaternion.identity);
        obj.SetPool(PoolSfx3D);
        obj.transform.parent = transform;
        return obj;
    }
    private void OnTakeSfxSource3DFromPool(SfxSource3D obj)
    {
        obj.gameObject.SetActive(true);
    }
    private void OnReturnSfxSource3DToPool(SfxSource3D obj) => obj.gameObject.SetActive(false);
    private void OnDestroySfxSource3D(SfxSource3D obj)
    {
        if (obj != null) Destroy(obj.gameObject);
    }
    #endregion SfxPool

    #region Collectable
    private Collectable CreateCollectable()
    {
        Collectable collectable = Instantiate(screwPrefab, transform.position, Quaternion.identity);
        collectable.SetPool(PoolCollectables);
        collectable.transform.parent = transform;
        return collectable;
    }
    private void OnTakeCollectableFromPool(Collectable collectable)
    {
        collectable.GotUsed = false;
        foreach(Transform child in collectable.transform) { child.gameObject.SetActive(false); }
        collectable.transform.GetChild(Random.Range(0,collectable.transform.childCount)).gameObject.SetActive(true);
        collectable.gameObject.SetActive(true);
    }
    private void OnReturnCollectableToPool(Collectable collectable) => collectable.gameObject.SetActive(false);
    private void OnDestroyCollectable(Collectable collectable) {if(collectable != null) Destroy(collectable.gameObject); }
    #endregion Collectable
}