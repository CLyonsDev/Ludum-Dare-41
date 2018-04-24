using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItem : MonoBehaviour {

    public LayerMask collectionLM;
    public LayerMask companionLM;
    public LayerMask doorLM;
    public LayerMask unlockerLM;
    public LayerMask chargingStationLM;

    private Transform player;

    public float collectDist = 1f;
    public bool lookingAtItem = false;
    public bool lookingAtCompanion = false;
    public bool lookingAtDoor = false;
    public bool lookingAtChargingStation = false;
    public bool lookingAtUnlocker = false;

    private CompanionSoundManager soundManager;

    public static CollectItem _Instance;
    [HideInInspector]
    public Vector3 station;

    private void Awake()
    {
        if (_Instance == null)
            _Instance = this;
        else
            Destroy(this);
    }
    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        soundManager = CompanionSoundManager._Instance;
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, collectDist, collectionLM))
        {
            if (lookingAtCompanion || lookingAtDoor || lookingAtUnlocker)
                return;

            CursorManager._Instance.ChangeCursorState(CursorManager.CursorStates.c_collect);
            lookingAtItem = true;
            if (Input.GetButtonDown("Fire1"))
            {
                if (hit.transform.tag.Equals("Radio"))
                {
                    Destroy(hit.transform.parent.parent.gameObject);
                    PlayerSoundManager._Instance.PlaySound(PlayerSoundManager._Instance.radioIntro);
                    return;
                }

                // Pick up object
                switch (hit.transform.root.GetComponent<Pickup>().pickupType)
                {
                    case Pickup.PickupTypeList.Battery:
                        Debug.Log("Picked up battery.");
                        Destroy(hit.transform.root.gameObject);
                        PlayerInventory._Instance.AddItem(Pickup.PickupTypeList.Battery);
                        break;
                    case Pickup.PickupTypeList.Oil:
                        Debug.Log("Picked up some oil.");
                        Destroy(hit.transform.root.gameObject);
                        PlayerInventory._Instance.AddItem(Pickup.PickupTypeList.Oil);
                        break;
                    case Pickup.PickupTypeList.Key:
                        Debug.Log("Picked up a key.");
                        Destroy(hit.transform.root.gameObject);
                        PlayerInventory._Instance.AddItem(Pickup.PickupTypeList.Key, hit.transform.root.GetComponent<Pickup>().keyID);
                        break;
                }
            }
        }else if (lookingAtItem && !lookingAtCompanion && !lookingAtDoor && !lookingAtUnlocker && !lookingAtChargingStation)
        {
            CursorManager._Instance.ChangeCursorState(CursorManager.CursorStates.c_default);
            lookingAtItem = false;
        }

        if (Physics.Raycast(ray, out hit, collectDist, companionLM))
        {
            if (lookingAtItem || lookingAtDoor)
                return;

            lookingAtCompanion = true;

            if (PlayerInventory._Instance.numOil > 0)
            {
                CursorManager._Instance.ChangeCursorState(CursorManager.CursorStates.c_oil);
                if (Input.GetButtonDown("Fire1"))
                {
                    Debug.Log("Refilling Companion");
                    GameObject.FindGameObjectWithTag("NeedsContainer").GetComponent<CompanionNeeds>().AddFood(100);
                    PlayerInventory._Instance.RemoveItem(Pickup.PickupTypeList.Oil);
                    soundManager.PlaySound(soundManager.giveFood);
                }
            }
            else
            {
                lookingAtCompanion = true;
                CursorManager._Instance.ChangeCursorState(CursorManager.CursorStates.c_oil_depleted);
            }
        }
        else if (lookingAtCompanion && !lookingAtItem && !lookingAtDoor && !lookingAtUnlocker && !lookingAtChargingStation)
        {
            CursorManager._Instance.ChangeCursorState(CursorManager.CursorStates.c_default);
            lookingAtCompanion = false;
        }

        if (Physics.Raycast(ray, out hit, collectDist, doorLM))
        {
            if (lookingAtCompanion || lookingAtItem)
                return;

            CursorManager._Instance.ChangeCursorState(CursorManager.CursorStates.c_door);
            lookingAtDoor = true;
            if (Input.GetButtonDown("Fire1"))
            {
                hit.transform.root.GetComponent<DoorLock>().Open();
            }
        }
        else if (!lookingAtCompanion && !lookingAtItem && lookingAtDoor && !lookingAtUnlocker && !lookingAtChargingStation)
        {
            CursorManager._Instance.ChangeCursorState(CursorManager.CursorStates.c_default);
            lookingAtDoor = false;
        }

        if(Physics.Raycast(ray, out hit, collectDist, unlockerLM))
        {
            if (lookingAtCompanion || lookingAtItem)
                return;

            CursorManager._Instance.ChangeCursorState(CursorManager.CursorStates.c_door);

            lookingAtUnlocker = true;
            if (Input.GetButtonDown("Fire1"))
            {
                hit.transform.root.GetComponent<RemoteUnlock>().UnlockDoor();
            }
        }else if (lookingAtUnlocker)
        {
            lookingAtUnlocker = false;
            CursorManager._Instance.ChangeCursorState(CursorManager.CursorStates.c_default);
        }

        if(Physics.Raycast(ray, out hit, collectDist, chargingStationLM))
        {
            if (lookingAtCompanion || lookingAtItem)
                return;

            CursorManager._Instance.ChangeCursorState(CursorManager.CursorStates.c_collect);
            lookingAtChargingStation = true;
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("Recharging");
                station = hit.transform.root.Find("Refill Point").position;
                CompanionState._Instance.transform.GetComponent<CompanionMovement>().rechargingStationLoc = station;
                CompanionState._Instance.transform.GetComponent<CompanionState>().SetState(CompanionState.CompanionStateList.movingToRecharge);
            }
        }else if(lookingAtChargingStation)
        {
            lookingAtChargingStation = false;
            CursorManager._Instance.ChangeCursorState(CursorManager.CursorStates.c_default);
        }
    }
}
