using UnityEngine;

public abstract class MechAbility : MonoBehaviour
{
    [Header("Ability Settings")]
    public float cooldownDuration = 3f;
    public KeyCode activationKey = KeyCode.None;

    [Header("Visual Feedback")]
    public bool showDebugGizmos = true;
    public Color readyColor = Color.green;
    public Color cooldownColor = Color.red;

    protected float cooldownTimer;
    protected bool canActivate = true;
    protected PlayerControlledEntity playerControlledEntity;

    protected Mech mechRef;

    protected virtual void Start()
    {
        playerControlledEntity = GetComponent<PlayerControlledEntity>();
        if (playerControlledEntity == null)
        {
            Debug.LogWarning($"No PlayerControlledEntity found on {gameObject.name} for {GetType().Name}");
        }

        mechRef = GetComponent<Mech>();
        if (mechRef == null)
        {
            Debug.LogWarning($"No Mech component found on {gameObject.name} for {GetType().Name}");
        }
    }

    protected virtual void Update()
    {
        if (!canActivate)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                canActivate = true;
            }
        }

        if (Input.GetKeyDown(activationKey) && canActivate && playerControlledEntity != null)
        {
            ActivateAbility();
        }
    }

    protected abstract void ActivateAbility();

    protected virtual void OnGUI()
    {
        if (showDebugGizmos)
        {
            GUI.color = canActivate ? readyColor : cooldownColor;
            string status = canActivate ? $"{GetType().Name} Ready" : $"Cooldown: {cooldownTimer:F1}s";
            GUI.Label(new Rect(10, 30 + GetType().Name.GetHashCode() % 100, 200, 20), status);
        }
    }

    protected virtual void StartCooldown()
    {
        canActivate = false;
        cooldownTimer = cooldownDuration;
    }
}
