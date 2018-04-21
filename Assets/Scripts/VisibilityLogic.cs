using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityLogic : MonoBehaviour {

    private FlashlightLogic flashlight;
    public bool isSeen = false;

	// Use this for initialization
	void Start () {
        flashlight = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<FlashlightLogic>();
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponentInChildren<Renderer>().isVisible && flashlight.lightEnhanced)
        {
            isSeen = true;
            Debug.Log("Visible");
        }
        else
        {
            isSeen = false;
        }
	}
}
