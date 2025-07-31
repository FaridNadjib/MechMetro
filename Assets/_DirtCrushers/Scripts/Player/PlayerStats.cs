using System;
using UnityEngine;

[CreateAssetMenu(menuName ="PlayerStats",fileName ="PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("Screws related:")]
    [SerializeField] int startingScrews = 50;
    [SerializeField] int screwRevivalCost = 15;
    [SerializeField] int maxGoldScrews = 6;
    [Header("Energy related:")]
    [SerializeField] float maxEnergy = 100f;
    [Header("Timer related:")]
    [SerializeField] float maxTimeInSeconds = 900f;
    float currentGameTimer;

    [Header("Debug related(dont bother to change):")]
    [SerializeField] float currentEnergyLevel;
    [SerializeField] int currentGreyScrews;
    [SerializeField] int currentGoldScrews;

    bool energyDepleted;

    public event Action<string> OnGoldScrewsChanged;
    public event Action<string> OnGreyScrewsChanged;
    public event Action<float> OnEnergyChanged;
    public event Action<bool> OnEnergyDepleted;
    public event Action OnDied;
    public event Action<string> OnTimerChanged;
    public event Action OnTimerUp;

    public void InitializeStats()
    {
        currentGreyScrews = startingScrews;
        currentGoldScrews = 0;
        currentEnergyLevel = maxEnergy;
        energyDepleted = false;
        currentGameTimer = 0f;

        OnEnergyChanged?.Invoke(currentEnergyLevel / maxEnergy);
        OnGoldScrewsChanged?.Invoke($"{currentGoldScrews}/{maxGoldScrews}");
        OnGreyScrewsChanged?.Invoke($"{currentGreyScrews}");

    }

    public void AddGoldScrew() 
    {
        currentGoldScrews++;
        OnGoldScrewsChanged?.Invoke($"{currentGoldScrews}/{maxGoldScrews}");
    }

    public void AddGreyScrews(int amount)
    {
        currentGreyScrews += amount;
        OnGreyScrewsChanged?.Invoke($"{currentGreyScrews}");
    }
    public void SpendGreyScrews(int amount)
    {
        currentGreyScrews -= amount;
        if(currentGreyScrews < 0) { currentGreyScrews = 0; }
        OnGreyScrewsChanged?.Invoke($"{currentGreyScrews}");
    }

    public void TryRevivePlayer()
    {
        if (currentGreyScrews >= screwRevivalCost)
        {
            SpendGreyScrews(screwRevivalCost);
            //ToDO: move the player to the position, blending.
        }
        else
        {
            OnDied?.Invoke();
            // ToDO: handle death.
        }
    }

    public void AddEnergy(float amount)
    {
        currentEnergyLevel += amount;
        if (currentEnergyLevel >= maxEnergy) currentEnergyLevel = maxEnergy;
        OnEnergyChanged?.Invoke(currentEnergyLevel / maxEnergy);
        if (energyDepleted)
        {
            energyDepleted = false;
            OnEnergyDepleted?.Invoke(energyDepleted);
        }
    }
    public void SpendEnergy(float amount)
    {
        currentEnergyLevel -= amount;
        if (currentEnergyLevel < 0f) { currentEnergyLevel = 0f;
            energyDepleted = true;
            OnEnergyDepleted?.Invoke(energyDepleted);
        }
        OnEnergyChanged?.Invoke(currentEnergyLevel/maxEnergy);
    }

    public void UpdateTimer()
    {
        currentGameTimer = Mathf.Clamp(currentGameTimer + Time.fixedDeltaTime,0f,maxTimeInSeconds);
        if(currentGameTimer >= maxTimeInSeconds)
        {
            OnTimerUp?.Invoke();
        }
        else
        {
            OnTimerChanged?.Invoke($"{currentGameTimer:0.00}/{maxTimeInSeconds}");
        }
        
    }
}
