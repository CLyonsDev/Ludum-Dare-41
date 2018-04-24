using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : MonoBehaviour {

    public int lockNum = -1;
    [SerializeField]
    private bool remoteUnlocked = false;  // Unlocked via remote? (monitor)

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Open()
    {
        if (PlayerInventory._Instance.lockNum.Contains(lockNum) || lockNum == -1 || remoteUnlocked == true)
        {
            Animator anim = GetComponentInChildren<Animator>();
            bool b = anim.GetBool("isOpen");
            anim.SetBool("isOpen", !b);
            anim.SetTrigger("interact");
        }
        else
        {
            CompanionSoundManager._Instance.PlaySound(CompanionSoundManager._Instance.lockedDoor);
        }
    }

    public void Unlock()
    {
        remoteUnlocked = true;
    }

    public void Lock()
    {
        remoteUnlocked = false;
    }
}
