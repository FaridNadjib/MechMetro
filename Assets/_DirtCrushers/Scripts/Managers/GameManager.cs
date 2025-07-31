using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;



    [SerializeField] bool lockCursor = true;
    [field:SerializeField] public PlayerStats PlayerStats { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Destroy(gameObject); }

        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerStats.InitializeStats();
        if(lockCursor)
            Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        PlayerStats.UpdateTimer();

        if (lockCursor)
            Cursor.lockState = CursorLockMode.Locked;
    }
}
