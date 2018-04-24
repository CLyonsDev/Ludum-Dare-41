using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CompanionMovement : MonoBehaviour {

    [HideInInspector]
    public Transform playerTransform;
    private Transform ballTransform;
    private NavMeshAgent agent;

    private float bestSpeed = 3.5f;
    private float worstSpeed = 0.85f;

    private float stopDist;
    private float followDist = 6;

    public bool reachedDest = false;

    CompanionState state;
    [HideInInspector]
    public Vector3 rechargingStationLoc;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        state = CompanionState._Instance;
        stopDist = agent.stoppingDistance;
        UpdateSpeed(true);

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        ballTransform = playerTransform.GetComponent<EquipmentManager>().equipment[1].transform;
        //ballTransform = GameObject.FindGameObjectWithTag("Ball").transform;
        // ballTransform.gameObject.SetActive(false);
        // agent.SetDestination(playerTransform.position);
	}

    public void SetRechargeGoal(Vector3 v)
    {
        rechargingStationLoc = v;
    }
	
    public void StopMoving()
    {
        Debug.LogError("Stop moving");
        agent.SetDestination(transform.position);
    }

    public void UpdateSpeed(bool notStarving)
    {
        if (notStarving)
            agent.speed = bestSpeed;
        else
            agent.speed = worstSpeed;
    }

	// Update is called once per frame
	void Update () {
        if (!CompanionState._Instance.activated)
            return;

        float distToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Debug.Log("Moving to player: " + distToPlayer);

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
                ballTransform.GetComponent<ThrowBall>().ReturnBall(false);
            }
            float distToBall = Vector3.Distance(transform.position, ballTransform.position);
            agent.stoppingDistance = 1.2f;
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
                ballTransform.GetComponent<ThrowBall>().grabbedByCompanion = true;
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
                    ballTransform.GetComponent<ThrowBall>().ReturnBall(true);
                }
            }
        }
        else if(state.currentState == CompanionState.CompanionStateList.movingToRecharge)
        {
            agent.stoppingDistance = 0.25f;
            if(Vector3.Distance(transform.position, rechargingStationLoc) <= agent.stoppingDistance)
            {
                agent.stoppingDistance = stopDist;
                CompanionNeeds._Instance.StartRecharge(true, true);
            }
            else
            {
                agent.SetDestination(rechargingStationLoc);
            }
        }
    }
}
