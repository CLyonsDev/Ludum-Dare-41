using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerBase : MonoBehaviour {

    public AudioSource source;

    // Use this for initialization
    void Start () {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip[] clip, bool overrideRandom = false, int index = -1)
    {
        if (CompanionNeeds._Instance.isResting || CompanionState._Instance.currentState  == CompanionState.CompanionStateList.idle)
            return;

        if (overrideRandom)
        {
            source.PlayOneShot(clip[index]);
        }
        else
        {
            source.PlayOneShot(clip[Random.Range(0, clip.Length)]);
        }
    }
}
