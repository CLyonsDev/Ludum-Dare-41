using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour {

    public GameObject[] equipment;  // 0 = Flashlight, 1 = Ball

	// Use this for initialization
	void Start () {
        SetEquipmentAsActive(0);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetEquipmentAsActive(0);
        }else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetEquipmentAsActive(1);
        }
	}

    private void SetEquipmentAsActive(int index)
    {
        for (int i = 0; i < equipment.Length; i++)
        {
            if (i == index)
                equipment[i].SetActive(true);
            else
            {
                if (i != 1)
                    equipment[i].SetActive(false);
                else
                    equipment[i].GetComponent<ThrowBall>().DeactivateBall();
            }
        }
    }
}
