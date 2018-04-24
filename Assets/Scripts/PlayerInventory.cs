using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerInventory : MonoBehaviour {

    public int numBatteries = 0;
    public int numOil = 0;
    public int numKeys = 0;
    public List<int> lockNum; // List of all locks that we can unlock w/ the keys we have picked up.
    private int prevBat;
    private int prevOil;
    private int prevLvl;

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

    public void RevertInventory()
    {
        numOil = prevOil;
        numBatteries = prevBat;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLoad;
    }

    private void OnLoad(Scene scene, LoadSceneMode mode)
    {
        prevBat = numBatteries;
        prevOil = numOil;
    }

    private void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        textContainerTransform = GameObject.FindGameObjectWithTag("ResourceTextContainer").transform;
        batteryText = textContainerTransform.Find("BatteryText").GetComponent<Text>();
        oilText = textContainerTransform.Find("OilText").GetComponent<Text>();
        keysText = textContainerTransform.Find("KeysText").GetComponent<Text>();

        batteryText.text = "Batteries: " + numBatteries.ToString();
        oilText.text = "Oil: " + numOil.ToString();
        //keysText.text = "Keys: " + numKeys.ToString();
        keysText.text = "";
    }

    private void Update()
    {
        UpdateUI();
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
