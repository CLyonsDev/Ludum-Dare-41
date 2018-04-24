using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteUnlock : MonoBehaviour {

    public DoorLock[] doorsToUnlock;

    public void UnlockDoor()
    {
        foreach (DoorLock lockedDoor in doorsToUnlock)
        {
            lockedDoor.Unlock();
        }
    }
}
