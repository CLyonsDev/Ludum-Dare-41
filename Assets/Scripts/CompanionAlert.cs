using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionAlert : MonoBehaviour {

    private Transform lightContainer;
    private Transform monsterTransform;

    public Light[] allLights;

    private RotateOverTime dogLight;

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

        SetAlertStatus(true);
    }

    // Update is called once per frame
    void Update () {
        if(monsterTransform == null)
            monsterTransform = GameObject.FindGameObjectWithTag("Monster").transform;

        float dist = Vector3.Distance(transform.position, monsterTransform.position);
        // Debug.Log(dist);

        if(dist <= warningDist && dist > dangerDist)
        {
            ChangeLights(1);
            dogLight.SetRotSpeed(1);
        }else if(dist <= dangerDist)
        {
            ChangeLights(2);
            dogLight.SetRotSpeed(2);
        }
        else
        {
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
