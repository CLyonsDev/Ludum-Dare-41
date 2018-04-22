using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionAlert : MonoBehaviour {

    private Transform lightContainer;
    private Transform monsterTransform;

    public Light[] allLights;

    private RotateOverTime dogLight;
    private CompanionSoundManager soundManager;

    [SerializeField]
    private Color[] lightColors;
    private Color prevColor;

    private int state = 0; // 0 = Normal, 1 = Alert, 2 = Danger

    private float warningDist;
    private float dangerDist;

    private float bestWarningDist = 25f;
    private float bestDangerDist = 12f;
    private float worstDangerDist = 15f;
    private float worstWarningDist = 7f;

	// Use this for initialization
	void Start () {
        allLights = GetComponentsInChildren<Light>();
        dogLight = GetComponentInChildren<RotateOverTime>();
        soundManager = CompanionSoundManager._Instance;

        SetAlertStatus(true);
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

        Debug.Log("Changing State to " + s);

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
