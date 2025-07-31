using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private bool lockCursor = true;
    [field: SerializeField] public PlayerStats PlayerStats { get; private set; }

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