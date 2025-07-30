using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MechSpeedBoost : MonoBehaviour
{
    [Header("Boost Settings")]
    public float boostForce = 20f;
    public float cooldownDuration = 3f;
    public KeyCode boostKey = KeyCode.LeftShift;

    [Header("Visual Feedback")]
    public bool showDebugGizmos = true;
    public Color boostReadyColor = Color.green;
    public Color boostCooldownColor = Color.red;

    private Rigidbody rb;
    private PlayerControlledEntity playerControlledEntity;
    private float cooldownTimer;
    private bool canBoost = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerControlledEntity = GetComponent<PlayerControlledEntity>();
        
        if (playerControlledEntity == null)
        {
            Debug.LogWarning("No PlayerControlledEntity found on the same GameObject as MechSpeedBoost");
        }
    }

    void Update()
    {
        // update cooldown
        if (!canBoost)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                canBoost = true;
            }
        }

        if (Input.GetKeyDown(boostKey) && canBoost && playerControlledEntity != null)
        {
            ApplyBoost();
        }
    }

    void ApplyBoost()
    {
        Vector3 boostDirection = playerControlledEntity.InputAdjustedForwardVector.normalized;
        rb.AddForce(boostDirection * boostForce, ForceMode.Impulse);

        // start cooldown
        canBoost = false;
        cooldownTimer = cooldownDuration;
    }

    // here we show the cooldown status in the top-left corner
    void OnGUI()
    {
        if (showDebugGizmos)
        {
            GUI.color = canBoost ? boostReadyColor : boostCooldownColor;
            string status = canBoost ? "Boost Ready" : $"Cooldown: {cooldownTimer:F1}s";
            GUI.Label(new Rect(10, 10, 200, 20), status);
        }
    }
}
