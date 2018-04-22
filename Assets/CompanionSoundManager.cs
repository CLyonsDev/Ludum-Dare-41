using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionSoundManager : MonoBehaviour {

    public AudioClip[] warningSounds;
    public AudioClip[] dangerSounds;
    public AudioClip[] lowEnergy;
    public AudioClip[] lowFun;
    public AudioClip[] lowFood;
    public AudioClip[] recharge;
    public AudioClip[] fetch;
    public AudioClip[] giveFood;


    private AudioSource source;

    public static CompanionSoundManager _Instance;

    private void Awake()
    {
        if (_Instance == null)
            _Instance = this;
        else
            Destroy(this);

        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip[] clip, bool overrideRandom = false, int index = -1)
    {
        if (CompanionNeeds._Instance.isResting)
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
