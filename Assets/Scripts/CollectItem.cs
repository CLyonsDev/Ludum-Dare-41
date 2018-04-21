using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItem : MonoBehaviour {

    public LayerMask collectionLM;
    public LayerMask companionLM;

    private Transform player;

    public float collectDist = 0.5f;
    public bool lookingAtItem = false;
    public bool lookingAtCompanion = false;

    public static CollectItem _Instance;

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
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, collectDist, collectionLM))
        {
            CursorManager._Instance.ChangeCursorState(CursorManager.CursorStates.c_collect);
            lookingAtItem = true;
            if (Input.GetButtonDown("Fire1"))
            {
                // Pick up object
                Debug.Log("Picked up item");
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
                }
            }
        }else if (lookingAtItem && !lookingAtCompanion)
        {
            CursorManager._Instance.ChangeCursorState(CursorManager.CursorStates.c_default);
            lookingAtItem = false;
        }

        if(Physics.Raycast(ray, out hit, collectDist, companionLM))
        {
            lookingAtCompanion = true;

            if (PlayerInventory._Instance.numOil > 0)
            {
                CursorManager._Instance.ChangeCursorState(CursorManager.CursorStates.c_oil);
                if (Input.GetButton("Fire1"))
                {
                    Debug.Log("Refilling Companion");
                    hit.transform.root.GetComponent<CompanionNeeds>().AddEnergy(75);
                    PlayerInventory._Instance.RemoveItem(Pickup.PickupTypeList.Oil);
                }
            }
            else
            {
                CursorManager._Instance.ChangeCursorState(CursorManager.CursorStates.c_oil_depleted);
            }
        }
        else if (lookingAtCompanion && !lookingAtItem)
        {
            CursorManager._Instance.ChangeCursorState(CursorManager.CursorStates.c_default);
            lookingAtCompanion = false;
        }
    }
}
