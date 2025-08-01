using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private bool lockCursor = true;
    [field: SerializeField] public PlayerStats PlayerStats { get; private set; }
    [field: SerializeField] public Transform Player { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Destroy(gameObject); }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        PlayerStats.InitializeStats();
        var playerTags = GameObject.FindGameObjectsWithTag("Player");
        foreach (var tag in playerTags) { if (tag.transform.parent == null) {Player = tag.transform; break; } }
        if (lockCursor)
            Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        PlayerStats.UpdateTimer();

        if (lockCursor)
            Cursor.lockState = CursorLockMode.Locked;
    }
}