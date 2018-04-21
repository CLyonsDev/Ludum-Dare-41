using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CompanionMovement : MonoBehaviour {

    private Transform playerTransform;
    private Transform ballTransform;
    private NavMeshAgent agent;

    private float stopDist;
    private float followDist = 6;

    public bool reachedDest = false;

    CompanionState state;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        state = CompanionState._Instance;
        stopDist = agent.stoppingDistance;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        ballTransform = GameObject.FindGameObjectWithTag("Ball").transform;
        ballTransform.gameObject.SetActive(false);
        agent.SetDestination(playerTransform.position);
	}
	
	// Update is called once per frame
	void Update () {
        float distToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (state.currentState == CompanionState.CompanionStateList.following)
        {
            agent.stoppingDistance = stopDist;
            
            // Companion follows player up until it reaches the stopping distance.
            if (distToPlayer > stopDist && !reachedDest)
            {
                agent.SetDestination(playerTransform.position);
            }
            else
            {
                // Companion then does not follow player.
                if (!reachedDest)
                    reachedDest = true;
                transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z));
            }

            // Companion resumes following player if player gets followDist away from companion
            if (distToPlayer > followDist && reachedDest)
                reachedDest = false;
        }else if (state.currentState == CompanionState.CompanionStateList.fetching)
        {
            // Debug.Log(agent.pathStatus);
            if(agent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                ballTransform.GetComponent<ThrowBall>().ReturnBall();
            }
            float distToBall = Vector3.Distance(transform.position, ballTransform.position);
            agent.stoppingDistance = 0.75f;
            if(distToBall > agent.stoppingDistance && !reachedDest)
            {
                agent.SetDestination(ballTransform.position);
                //Debug.Log(string.Format("M {0} : {1}", distToBall, agent.stoppingDistance));
            }
            else
            {
                if (!ballTransform.GetComponent<ThrowBall>().isGrabbable)
                    return;

                // Companion has reached the ball
                if (!reachedDest)
                {
                    Debug.Log("Reached Destination");
                    reachedDest = true;
                }

                agent.stoppingDistance = 1.5f;

                ballTransform.GetComponent<Rigidbody>().isKinematic = true;
                ballTransform.SetParent(transform.Find("MountLoc"));
                ballTransform.transform.localPosition = Vector3.zero;

                if(distToPlayer > agent.stoppingDistance)
                {
                    //Debug.Log(string.Format("M {0} : {1}", distToPlayer, agent.stoppingDistance));
                    agent.SetDestination(playerTransform.position);
                }
                else
                {
                    Debug.Log("Ball Returned.");
                    // Ball has been returned.
                    ballTransform.GetComponent<ThrowBall>().ReturnBall();
                }
            }
        }
    }
}
