using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionNeeds : MonoBehaviour {

    private int maxFood = 100;
    [SerializeField]
    private float currentFood;
    private float foodLossPerTick = 0.1f; // 0.1f

    private int maxHappiness = 100;
    [SerializeField]
    private float currentHappiness;
    private float happinessLossPerTick = 1.5f; // 1.5f

    private int maxEnergy = 100;
    [SerializeField]
    private float currentEnergy;
    private float energyLossPerTick = 0.5f; // 0.5f
    private int energyRechargeRate = 1;
    private float reactivateThreshold = 40f;

    private float alertPercent = 0.25f;

    private bool alertFood = false, alertEnergy = false, alertHappy = false, alertRecharge = false;

    [SerializeField]
    public bool isResting = false;

    private float tickRate = 0.5f;

    public static CompanionNeeds _Instance;
    CompanionSoundManager soundManager;

    private void Awake()
    {
        if (_Instance == null)
            _Instance = this;
        else
            Destroy(this);
    }

    // Use this for initialization
    void Start () {
        currentFood = maxFood;
        currentHappiness = maxHappiness;
        currentEnergy = maxEnergy;

        soundManager = GetComponent<CompanionSoundManager>();

        StartCoroutine(Tick());
    }

    private IEnumerator Tick()
    {
        while (true)
        {
            yield return new WaitForSeconds(tickRate);
            if (!isResting)
            {
                currentFood -= foodLossPerTick;
                currentHappiness -= happinessLossPerTick;
                currentEnergy -= energyLossPerTick;
                CheckThresholds();
                CheckStats();
            }
        }
    }

    private void CheckThresholds()
    {
        float food = currentFood / maxFood;
        float energy = currentEnergy / maxEnergy;
        float happy = currentHappiness / maxHappiness;

        if (food <= alertPercent && !alertFood)
        {
            soundManager.PlaySound(soundManager.lowFood);
            alertFood = true;
        }else if (energy <= alertPercent && !alertEnergy)
        {
            soundManager.PlaySound(soundManager.lowEnergy);
            alertEnergy = true;
        }else if (happy <= alertPercent && !alertHappy)
        {
            soundManager.PlaySound(soundManager.lowFun);
            alertHappy = true;
        }
        else
        {
            if (alertFood && food > alertPercent)
                alertFood = false;
            if (alertEnergy && energy > alertPercent)
                alertEnergy = false;
            if (alertHappy && happy > alertPercent)
                alertHappy = false;
        }
    }

    private IEnumerator Recharge()
    {
        if(!alertRecharge)
            soundManager.PlaySound(soundManager.recharge);

        alertRecharge = true;
        isResting = true;
        CompanionState._Instance.SetState(CompanionState.CompanionStateList.idle);
        Light[] lights = GetComponent<CompanionAlert>().allLights;
        foreach (Light l in lights)
        {
            l.intensity = 0.5f;
        }
        GetComponent<CompanionMovement>().StopMoving();
        while (currentEnergy < reactivateThreshold)
        {
            yield return new WaitForSeconds(0.2f);
            currentEnergy = Mathf.Clamp(currentEnergy + energyRechargeRate, 0, maxEnergy);
        }
        CompanionState._Instance.SetState(CompanionState.CompanionStateList.following);
        foreach (Light l in lights)
        {
            l.intensity = 1f;
        }
        isResting = false;
        alertEnergy = false;
        alertRecharge = false;
    }

    private void CheckStats()
    {
        if(currentEnergy <= 0)
        {
            Debug.LogError("Out of energy! Recharging...");
            StartCoroutine(Recharge());
        }

        if(currentHappiness <= 0)
        {
            // Debug.LogError("Out of happiness! I am anger........");
            GetComponent<CompanionAlert>().SetAlertStatus(false);
        }
        else
        {
            GetComponent<CompanionAlert>().SetAlertStatus(true);
        }

        if(currentFood <= 0)
        {
            // I AM STARVINGGGGGG
            GetComponent<CompanionMovement>().UpdateSpeed(false);
        }
        else
        {
            GetComponent<CompanionMovement>().UpdateSpeed(true);
        }
    }

    #region Status Functions
    public void AddFood(float amt)
    {
        currentFood = Mathf.Clamp(currentFood + amt, 0f, maxFood);
    }

    public void AddHappy(float amt)
    {
        currentHappiness = Mathf.Clamp(currentHappiness + amt, 0f, maxHappiness);
    }

    public void AddEnergy(float amt)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amt, 0f, maxEnergy);
    }

    public float[] GetStats()
    {
        return new float[] { currentFood / maxFood, currentHappiness / maxHappiness, currentEnergy / maxEnergy};
    }
    #endregion
}
