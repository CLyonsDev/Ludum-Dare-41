using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour {

    private NavMeshAgent agent;
    private Transform playerTransform;
    private VisibilityLogic vis;
    private MonsterHealth healthScript;

    private bool canMove = true;
    public bool moveEnabled = true;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        vis = GetComponent<VisibilityLogic>();
        healthScript = GetComponent<MonsterHealth>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (!moveEnabled)
            return;

        canMove = !vis.isBurned;

        MovementLogic();
	}

    //TODO: Graphic rotate to face player. We don't want to rotate the root.
    private void MovementLogic()
    {
        if (!canMove || healthScript.isDead)
        {
            agent.SetDestination(transform.position);
            return;
        }
        else
        {
            if(Vector3.Distance(transform.position, playerTransform.position) <= agent.stoppingDistance)
            {
                // We caught the player! Game Over.
                playerTransform.GetComponent<PlayerHealth>().Die();
            }
            else
            {
                agent.SetDestination(playerTransform.position);
            }
        }
    }
}
