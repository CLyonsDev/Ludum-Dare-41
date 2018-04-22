using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItem : MonoBehaviour {

    public LayerMask collectionLM;
    public LayerMask companionLM;
    public LayerMask doorLM;

    private Transform player;

    public float collectDist = 0.5f;
    public bool lookingAtItem = false;
    public bool lookingAtCompanion = false;
    public bool lookingAtDoor = false;

    private CompanionSoundManager soundManager;

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
        soundManager = CompanionSoundManager._Instance;
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
        }else if (lookingAtItem && !lookingAtCompanion && !lookingAtDoor)
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
                    hit.transform.root.GetComponent<CompanionNeeds>().AddEnergy(100);
                    PlayerInventory._Instance.RemoveItem(Pickup.PickupTypeList.Oil);
                    soundManager.PlaySound(soundManager.giveFood);
                }
            }
            else
            {
                CursorManager._Instance.ChangeCursorState(CursorManager.CursorStates.c_oil_depleted);
            }
        }
        else if (lookingAtCompanion && !lookingAtItem && !lookingAtDoor)
        {
            CursorManager._Instance.ChangeCursorState(CursorManager.CursorStates.c_default);
            lookingAtCompanion = false;
        }

        if(Physics.Raycast(ray, out hit, collectDist, doorLM))
        {
            CursorManager._Instance.ChangeCursorState(CursorManager.CursorStates.c_door);
            lookingAtDoor = true;
            if (Input.GetButtonDown("Fire1"))
            {
                Animator anim = hit.transform.root.GetComponent<Animator>();
                bool b = anim.GetBool("isOpen");
                anim.SetBool("isOpen", !b);
                anim.SetTrigger("interact");
            }
        }
        else if (!lookingAtCompanion && !lookingAtItem && lookingAtDoor)
        {
            CursorManager._Instance.ChangeCursorState(CursorManager.CursorStates.c_default);
            lookingAtDoor = false;
        }
    }
}
