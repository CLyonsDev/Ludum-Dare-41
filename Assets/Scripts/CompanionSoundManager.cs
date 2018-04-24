using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionSoundManager : SoundManagerBase {

    public AudioClip[] intro;
    public AudioClip[] warningSounds;
    public AudioClip[] dangerSounds;
    public AudioClip[] lowEnergy;
    public AudioClip[] lowFun;
    public AudioClip[] lowFood;
    public AudioClip[] recharge;
    public AudioClip[] rechargeAtPad;
    public AudioClip[] fetch;
    public AudioClip[] giveFood;
    public AudioClip[] lockedDoor;

    public static CompanionSoundManager _Instance;

    private void Awake()
    {
        if (_Instance == null)
            _Instance = this;
        else
            Destroy(this);

        source = GetComponent<AudioSource>();
    }
}
