using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCompanionTrigger : MonoBehaviour {

    private bool played = false;

    private void Start()
    {
        CompanionState._Instance.activated = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.tag == "Player" && !played)
        {
            played = true;
            CompanionState._Instance.activated = true;
            CompanionState._Instance.SetState(CompanionState.CompanionStateList.following);
            CompanionSoundManager._Instance.PlaySound(CompanionSoundManager._Instance.intro);
        }
    }
}
