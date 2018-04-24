using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionAlert : MonoBehaviour {

    private Transform lightContainer;
    private Transform monsterTransform;

    public Light[] allLights;
    private Light[] eyeLights;

    private RotateOverTime dogLight;
    private CompanionSoundManager soundManager;

    [SerializeField]
    private Color[] lightColors;
    private Color prevColor;

    private Material alertLightMaterial;
    public Color[] alertLightColors; // 0 = Normal, 1 = Alert, 2 = Danger

    private int state = -1; // 0 = Normal, 1 = Alert, 2 = Danger

    private float warningDist;
    private float dangerDist;

    private float bestWarningDist = 30f;
    private float bestDangerDist = 17f;
    private float worstDangerDist = 15f;
    private float worstWarningDist = 7f;

	// Use this for initialization
	void Start () {
        allLights = GameObject.FindGameObjectWithTag("CompanionAlertLights").GetComponentsInChildren<Light>();
        eyeLights = GameObject.FindGameObjectWithTag("CompanionEyeLights").GetComponentsInChildren<Light>();
        Material[] mats = transform.Find("Graphics").Find("robot_dog_ludum").Find("AlertLight").GetComponent<Renderer>().materials;
        foreach (Material m in mats)
        {
            if(m.name.Equals("Companion_Alarm (Instance)"))
            {
                // Debug.Log("Set Light.");
                alertLightMaterial = m;
                break;
            }
        }
        dogLight = GetComponentInChildren<RotateOverTime>();
        soundManager = CompanionSoundManager._Instance;

        SetAlertStatus(true);
        ChangeLights(0);
    }

    // Update is called once per frame
    void Update () {
        if(monsterTransform == null)
            monsterTransform = GameObject.FindGameObjectWithTag("Monster").transform;

        float dist = Vector3.Distance(transform.position, monsterTransform.position);
        // Debug.Log(dist);

        // Warning!
        if(dist <= warningDist && dist > dangerDist && state != 1)
        {
            ChangeLights(1);
            dogLight.SetRotSpeed(1);
            soundManager.PlaySound(soundManager.warningSounds);
        }else if(dist <= dangerDist && state != 2)
        {
            // DANGER!!
            ChangeLights(2);
            dogLight.SetRotSpeed(2);
            soundManager.PlaySound(soundManager.dangerSounds);
        }
        else if(dist > warningDist && state != 0)
        {
            // Safe.
            ChangeLights(0);
            dogLight.SetRotSpeed(0);
        }


        foreach (Light light in allLights)
        {
            prevColor = Color.Lerp(prevColor, lightColors[state], 20 * Time.deltaTime);
            light.color = prevColor;
        }
    }

    private void ChangeLights(int s)
    {
        // Debug.Log("Chaning Lights");
        if (state == s)
            return;

        if (state == -1)
            state = 0;

        Debug.Log("Changing State to " + s);

        alertLightMaterial.color = alertLightColors[s];
        alertLightMaterial.SetColor("_EmissionColor", alertLightColors[s]);
        prevColor = lightColors[state];
        state = s;
    }

    public void SetAlertStatus(bool happy)
    {
        if (happy)
        {
            warningDist = bestWarningDist;
            dangerDist = bestDangerDist;
        }
        else
        {
            warningDist = worstWarningDist;
            dangerDist = worstDangerDist;
        }
    }
}
