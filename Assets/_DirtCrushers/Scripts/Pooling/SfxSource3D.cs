using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class SfxSource3D : MonoBehaviour
{
    private AudioSource source;
    [SerializeField] private Vector2 pitchRange = new Vector2(0.85f, 1.05f);
    private ObjectPool<SfxSource3D> pool;

    private void Awake() => source = GetComponent<AudioSource>();

    public void PlayClip(AudioClip clip) => StartCoroutine(PlayingRoutine(clip));

    private IEnumerator PlayingRoutine(AudioClip clip)
    {
        source.pitch = Random.Range(pitchRange.x, pitchRange.y);
        source.clip = clip;
        source.Play();
        while (source.isPlaying)
        {
            yield return null;
        }
        pool.Release(this);
    }

    public void SetPool(ObjectPool<SfxSource3D> pool) => this.pool = pool;
}