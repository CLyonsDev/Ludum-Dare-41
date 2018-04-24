using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class PlayAudioCutscene : MonoBehaviour {

    public bool nextSceneOnCompletion = false;

    private AudioSource source;
    public AudioClip[] clips;
    int i = 0;

	void Start () {
        source = GetComponent<AudioSource>();
        source.clip = clips[0];
        source.Play();
	}

    private void Update()
    {
        if (!source.isPlaying)
        {
            i++;
            if(i < clips.Length)
            {
                source.clip = clips[i];
                source.Play();
            }
            else
            {
                if (nextSceneOnCompletion)
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

                Debug.LogWarning("Cutscene done.");
            }
        }
    }
}
