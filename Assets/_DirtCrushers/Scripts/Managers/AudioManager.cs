using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] Vector2 defaultPitchRange = new (0.85f,1.05f);

    [SerializeField] AudioSource musicSource1;
    [SerializeField] AudioSource musicSource2;
    [SerializeField] AudioSource audioSource1;
    [SerializeField] AudioSource audioSource2;
    [SerializeField] AudioSource sfxSource2D;

    private void Awake()
    {
        if(Instance == null) { Instance = this; }
        else { Destroy(this); }
    }

    public void PlaySfxClip(AudioClip clip)
    {
        sfxSource2D.pitch = Random.Range(defaultPitchRange.x,defaultPitchRange.y);
        sfxSource2D.PlayOneShot(clip);
    }
    public void PlaySfxClip(AudioClip clip,Vector2 pitchRange)
    {
        sfxSource2D.pitch = Random.Range(pitchRange.x, pitchRange.y);
        sfxSource2D.PlayOneShot(clip);
    }

    // ToDO: fading music and ambiance.
}
