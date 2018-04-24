using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MotorSoundManager : MonoBehaviour {

    public AudioClip rampupClip;
    public AudioClip rampdownClip;
    public AudioClip loopSound;

    public AudioClip[] clips;

    private AudioSource source;

    private bool isMoving = false;
    private NavMeshAgent agent;

    private Coroutine motorRoutine;

	// Use this for initialization
	void Start () {
        agent = transform.root.GetComponent<NavMeshAgent>();
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(transform.position, agent.destination) >= agent.stoppingDistance && !isMoving)
        {
            // Start the movement audio
            SetMovementAudio(true);

        }else if(Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance && isMoving)
        {
            // Stop movement audio
            SetMovementAudio(false);
        }
	}

    void SetMovementAudio(bool start)
    {
        if (start)
        {
            isMoving = true;
            source.clip = loopSound;
            source.Play();
        }
        else
        {
            isMoving = false;
            source.Stop();
        }
    }
}
