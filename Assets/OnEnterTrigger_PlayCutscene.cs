using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterTrigger_PlayCutscene : MonoBehaviour {

    public GameObject orient;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.tag == "Player")
        {
            //other.transform.root.rotation = orient.transform.rotation;
            //other.transform.root.SetParent(this.transform, true);
            //transform.root.GetComponent<Animator>().SetTrigger("StartFly");
        }
    }
}
