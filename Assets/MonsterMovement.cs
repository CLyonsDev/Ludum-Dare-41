using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour {

    private NavMeshAgent agent;
    private Transform playerTransform;
    private VisibilityLogic vis;

    private bool canMove = true;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        vis = GetComponent<VisibilityLogic>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        canMove = !vis.isSeen;

        MovementLogic();
	}

    private void MovementLogic()
    {
        if (!canMove)
        {
            agent.SetDestination(transform.position);
            return;
        }
        else
        {
            agent.SetDestination(playerTransform.position);
            transform.LookAt(playerTransform);
        }
    }
}
