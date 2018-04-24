using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : SoundManagerBase {

    public AudioClip[] radioIntro;

    public static PlayerSoundManager _Instance;

    private void Awake()
    {
        if (_Instance == null)
            _Instance = this;
        else
            Destroy(this);

        source = GetComponent<AudioSource>();
    }
}
