using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasLogic : MonoBehaviour
{

    public Image hungerBar, happyBar, energyBar;
    CompanionNeeds needs;
    private bool isActive = true;

    // Use this for initialization
    void Start()
    {
        needs = CompanionNeeds._Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            float[] stats = needs.GetStats();
            hungerBar.fillAmount = stats[0];
            happyBar.fillAmount = stats[1];
            energyBar.fillAmount = stats[2];
        }

    }
}
