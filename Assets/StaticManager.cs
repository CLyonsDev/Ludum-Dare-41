using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticManager : MonoBehaviour {

    void Start () {
        transform.SetParent(null);  // Become Batman.
        DontDestroyOnLoad(this.gameObject);
    }
}
