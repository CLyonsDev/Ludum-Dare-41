using UnityEngine;

public class Pickup : MonoBehaviour {

    public enum PickupTypeList
    {
        Battery,
        Oil,
        Key
    };

    public PickupTypeList pickupType;

    public int keyID = -1;
}
