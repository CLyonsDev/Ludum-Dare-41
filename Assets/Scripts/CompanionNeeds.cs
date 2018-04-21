using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionNeeds : MonoBehaviour {

    private int maxFood = 100;
    [SerializeField]
    private float currentFood;
    private float foodLossPerTick = 0.1f;

    private int maxHappiness = 100;
    [SerializeField]
    private float currentHappiness;
    private float happinessLossPerTick = 1;

    private int maxEnergy = 100;
    [SerializeField]
    private float currentEnergy;
    private float energyLossPerTick = 0.1f; // 0.1f
    private int energyRechargeRate = 1;
    private float reactivateThreshold = 25f;

    [SerializeField]
    public bool isResting = false;

    private float tickRate = 0.5f;

    public static CompanionNeeds _Instance;

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
                CheckStats();
            }
        }
    }

    private IEnumerator Recharge()
    {
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
