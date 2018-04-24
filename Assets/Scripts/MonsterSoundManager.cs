using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSoundManager : SoundManagerBase {

    public AudioClip closeSound;
    public AudioClip midSound;
    public AudioClip farSound;

    public static MonsterSoundManager _Instance;

    private void Awake()
    {
        if (_Instance == null)
            _Instance = this;
        else
            Destroy(this);

        source = GetComponent<AudioSource>();
    }

    public void SetLoop(AudioClip clip)
    {
        if(clip == null)
        {
            source.Stop();
            source.clip = null;
            return;
        }

        source.clip = clip;
        source.Play();
    }
}
