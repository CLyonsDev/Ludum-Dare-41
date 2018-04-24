using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioLoops : MonoBehaviour {

    public AudioClip[] clips;
    private float pause = 4f;
    List<int> lastClips = new List<int>();


	// Use this for initialization
	void Start () {
        StartCoroutine(PlaySounds());	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator PlaySounds()
    {
        while(true)
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                yield return new WaitForSeconds(pause);

                int i = Random.Range(0, clips.Length);
                while (lastClips.Contains(i))
                {
                    i = Random.Range(0, clips.Length);
                }

                if (lastClips.Count >= 2)
                {
                    lastClips.RemoveAt(0);
                    lastClips.Add(i);
                }

                GetComponent<AudioSource>().clip = clips[i];
                GetComponent<AudioSource>().Play();
            }
            else
            {
                yield return 0;
            }
        }
    }
}
