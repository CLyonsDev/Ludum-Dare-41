using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterTrigger_PlaySound : MonoBehaviour {

    public AudioClip clip;
    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.root.tag == "Player" && !hasPlayed)
        {
            PlayerSoundManager._Instance.PlaySound(new AudioClip[] { clip });
            hasPlayed = true;
        }
    }
}
