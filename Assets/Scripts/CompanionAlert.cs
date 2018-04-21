using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionAlert : MonoBehaviour {

    private Transform lightContainer;
    private Transform monsterTransform;

    Light[] allLights;

    [SerializeField]
    private Color[] lightColors;
    private Color prevColor;

    private int state = 0; // 0 = Normal, 1 = Alert, 2 = Danger

    private float warningDist = 25f;
    private float dangerDist = 12f;

	// Use this for initialization
	void Start () {
        lightContainer = transform.Find("Lights");
        allLights = lightContainer.GetComponentsInChildren<Light>();
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
        }else if(dist <= dangerDist)
        {
            ChangeLights(2);
        }
        else
        {
            ChangeLights(0);
        }


        foreach (Light light in allLights)
        {
            prevColor = Color.Lerp(prevColor, lightColors[state], 20 * Time.deltaTime);
            light.color = prevColor;
        }
    }

    private void ChangeLights(int s)
    {
        if (state == s)
            return;

        prevColor = lightColors[state];
        state = s;
    }
}
