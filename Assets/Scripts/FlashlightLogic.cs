using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightLogic : MonoBehaviour
{

    private Light flashlightLight; // Nice name
    public LayerMask flashlightLM;

    private float defaultIntensity = 1;
    private float enhancedIntensity = 3;
    private float lightLerpRate = 5;

    [HideInInspector]
    public bool lightEnhanced = false;
    private bool canEnhance = true;

    private float maxBattery = 100;
    [SerializeField]
    private float currentBattery;
    private float batteryDrainAmt = 35f;
    private float batteryDrainRate = 1f;
    private float batteryActivationThreshold = 20f;


    // Use this for initialization
    void Start()
    {
        currentBattery = maxBattery;
        flashlightLight = transform.GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canEnhance)
        {
            lightEnhanced = true;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            lightEnhanced = false;
        }

        if (lightEnhanced)
        {
            flashlightLight.intensity = Mathf.Lerp(flashlightLight.intensity, enhancedIntensity, lightLerpRate * Time.deltaTime);
            //RaycastLogic();
        }
        else
            flashlightLight.intensity = Mathf.Lerp(flashlightLight.intensity, defaultIntensity, lightLerpRate * Time.deltaTime);

        BatteryLogic();
    }

    private void RaycastLogic()
    {
        Debug.Log("Raycasting");
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, flashlightLM))
        {
            // Looking at monster
            Debug.LogWarning("Looking at monster");
        }
    }

    private void BatteryLogic()
    {
        if (lightEnhanced)
            currentBattery = Mathf.Clamp(currentBattery - batteryDrainAmt * Time.deltaTime, 0, maxBattery);
        else
            currentBattery = Mathf.Clamp(currentBattery + batteryDrainAmt * Time.deltaTime, 0, maxBattery);

        if(currentBattery <= 0)
        {
            lightEnhanced = false;
            canEnhance = false;
        }
        else if(currentBattery >= batteryActivationThreshold)
        {
            canEnhance = true;
        }


    }
}
