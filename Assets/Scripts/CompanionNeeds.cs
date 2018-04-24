using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class CompanionNeeds : MonoBehaviour {

    private float startingFood, startingHappy, startingEnergy;

    public float defaultLightIntensity = 2f;

    private int maxFood = 100;
    [SerializeField]
    private float currentFood = 100;
    private float foodLossPerTick = 0.1f; // 0.1f

    private int maxHappiness = 100;
    [SerializeField]
    private float currentHappiness = 100;
    private float happinessLossPerTick = 0.3f; // 1.5f

    private int maxEnergy = 100;
    [SerializeField]
    private float currentEnergy = 100;
    private float energyLossPerTick = 0.2f; // 0.2f
    private int energyRechargeRate = 1;
    private int energyRechargeRateFast = 2;
    private float reactivateThreshold = 40f;

    private float alertPercent = 0.25f;

    private bool alertFood = false, alertEnergy = false, alertHappy = false, alertRecharge = false;

    [SerializeField]
    public bool isResting = false;

    private float tickRate = 0.5f;

    private CompanionSoundManager soundManager;
    private GameObject warningUIContainer;

    private CompanionAlert alertScript;
    private NavMeshAgent agent;
    private CompanionMovement movement;

    public static CompanionNeeds _Instance;

    public void RevertNeeds()
    {
        currentEnergy = startingEnergy;
        currentFood = startingFood;
        currentHappiness = startingHappy;
    }

    private void OnEnable()
    {
        if (_Instance == null)
        {
            _Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene s, LoadSceneMode m)
    {
        if (s.buildIndex == 0)
        {
            currentEnergy = maxEnergy;
            currentFood = maxFood;
            currentHappiness = maxHappiness;
            return;
        }


        startingEnergy = currentEnergy;
        startingFood = currentFood;
        startingHappy = currentHappiness;

        Debug.Log("Grabbing References");
        warningUIContainer = GameObject.FindGameObjectWithTag("CompanionWarningContainer");
        GameObject companion = GameObject.FindGameObjectWithTag("Companion");
        soundManager = companion.gameObject.GetComponent<CompanionSoundManager>();
        alertScript = companion.gameObject.GetComponent<CompanionAlert>();
        movement = companion.gameObject.GetComponent<CompanionMovement>();
        agent = companion.gameObject.GetComponent<NavMeshAgent>();
        StartCoroutine(Tick());
    }

    // Use this for initialization
    void Start () {
    }

    private IEnumerator Tick()
    {
        while (true)
        {
            if (movement == null)
                yield return 0;

            yield return new WaitForSeconds(tickRate);
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(movement.playerTransform.position, path);
            if (!isResting && CompanionState._Instance.activated && path.status == NavMeshPathStatus.PathComplete)
            {
                currentFood -= foodLossPerTick;

                if (CompanionState._Instance.currentState != CompanionState.CompanionStateList.fetching)
                    currentHappiness -= happinessLossPerTick;
                if (CompanionState._Instance.currentState != CompanionState.CompanionStateList.movingToRecharge)
                    currentEnergy -= energyLossPerTick;

                CheckThresholds();
                CheckStats();
            }
            else
            {
                Debug.Log(string.Format("{0} {1} {2}", isResting, CompanionState._Instance.activated, path.status));
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
            warningUIContainer.transform.Find("Food Warning").gameObject.SetActive(true);
        }else if (energy <= alertPercent && !alertEnergy)
        {
            soundManager.PlaySound(soundManager.lowEnergy);
            alertEnergy = true;
            warningUIContainer.transform.Find("Energy Warning").gameObject.SetActive(true);
        }
        else if (happy <= alertPercent && !alertHappy)
        {
            soundManager.PlaySound(soundManager.lowFun);
            alertHappy = true;
            warningUIContainer.transform.Find("Happiness Warning").gameObject.SetActive(true);
        }
        else
        {
            if (alertFood && food > alertPercent)
            {
                warningUIContainer.transform.Find("Food Warning").gameObject.SetActive(false);
                alertFood = false;
            }
            if (alertEnergy && energy > alertPercent)
            {
                alertEnergy = false;
                warningUIContainer.transform.Find("Energy Warning").gameObject.SetActive(false);
            }
            if (alertHappy && happy > alertPercent)
            {
                alertHappy = false;
                warningUIContainer.transform.Find("Happiness Warning").gameObject.SetActive(false);
            }
        }
    }

    public void StartRecharge(bool fast, bool toFull)
    {
        if (isResting)
            return;

        StartCoroutine(Recharge(fast, toFull));
    }

    private IEnumerator Recharge(bool fast, bool toFull)
    {
        if (!alertRecharge)
        {
            if (!toFull)
                soundManager.PlaySound(soundManager.recharge);
            else
                soundManager.PlaySound(soundManager.rechargeAtPad);
        }

        alertRecharge = true;
        isResting = true;
        CompanionState._Instance.SetState(CompanionState.CompanionStateList.idle);
        Light[] lights = CompanionState._Instance.gameObject.GetComponent<CompanionAlert>().allLights;
        foreach (Light l in lights)
        {
            l.intensity = 0.5f;
        }
        CompanionSoundManager._Instance.gameObject.GetComponent<CompanionMovement>().StopMoving();
        if (!toFull)
        {
            while (currentEnergy < reactivateThreshold)
            {
                if (fast)
                    yield return new WaitForSeconds(0.15f);
                else
                    yield return new WaitForSeconds(0.2f);

                currentEnergy = Mathf.Clamp(currentEnergy + energyRechargeRate, 0, maxEnergy);
            }
        }
        else
        {
            while (currentEnergy < maxEnergy)
            {
                if(fast)
                    yield return new WaitForSeconds(0.15f);
                else
                    yield return new WaitForSeconds(0.2f);

                currentEnergy = Mathf.Clamp(currentEnergy + energyRechargeRateFast, 0, maxEnergy);
            }
        }
        
        CompanionState._Instance.SetState(CompanionState.CompanionStateList.following);
        foreach (Light l in lights)
        {
            l.intensity = 1.5f;
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
            StartRecharge(false, false);
        }

        if(currentHappiness <= 0)
        {
            // Debug.LogError("Out of happiness! I am anger........");
            alertScript.SetAlertStatus(false);
        }
        else
        {
            alertScript.SetAlertStatus(true);
        }

        if(currentFood <= 0)
        {
            // I AM STARVINGGGGGG
            alertScript.gameObject.GetComponent<CompanionMovement>().UpdateSpeed(false);
        }
        else
        {
            alertScript.gameObject.GetComponent<CompanionMovement>().UpdateSpeed(true);
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
