using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour {

    public int numBatteries = 0;
    public int numOil = 0;
    public int numKeys = 0;
    public List<int> lockNum; // List of all locks that we can unlock w/ the keys we have picked up.

    private Transform textContainerTransform;
    private Text batteryText, oilText, keysText;

    public static PlayerInventory _Instance;

    private void Awake()
    {
        if (_Instance == null)
            _Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        textContainerTransform = GameObject.FindGameObjectWithTag("ResourceTextContainer").transform;
        batteryText = textContainerTransform.Find("BatteryText").GetComponent<Text>();
        oilText = textContainerTransform.Find("OilText").GetComponent<Text>();
        keysText = textContainerTransform.Find("KeysText").GetComponent<Text>();
        UpdateUI();
    }

    private void UpdateUI()
    {
        batteryText.text = "Batteries: " + numBatteries.ToString();
        oilText.text = "Oil: " + numOil.ToString();
        keysText.text = "Keys: " + numKeys.ToString();
    }

    public void AddItem(Pickup.PickupTypeList itemType, int keyID = -1)
    {
        switch (itemType)
        {
            case Pickup.PickupTypeList.Battery:
                numBatteries++;
                break;
            case Pickup.PickupTypeList.Oil:
                numOil++;
                break;
            case Pickup.PickupTypeList.Key:
                numKeys++;
                lockNum.Add(keyID);
                break;
        }
        UpdateUI();
    }

    public void RemoveItem(Pickup.PickupTypeList itemType, int keyID = -1)
    {
        switch (itemType)
        {
            case Pickup.PickupTypeList.Battery:
                numBatteries--;
                break;
            case Pickup.PickupTypeList.Oil:
                numOil--;
                break;
            case Pickup.PickupTypeList.Key:
                numKeys--;
                lockNum.Remove(keyID);
                break;
        }
        UpdateUI();
    }
}
