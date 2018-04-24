using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour {

    private NavMeshAgent agent;
    private Transform playerTransform;
    private VisibilityLogic vis;
    private MonsterHealth healthScript;

    public LayerMask PlayerSearchLayermask;
    public Transform rayParent;
    private Transform[] rayTransforms;

    private bool canMove = true;
    public bool moveEnabled = true;

    public bool seenPlayer = false;

    private float runDist = 25f;
    private float sprintDist = 12f;

    private float wanderDist = 10f;

    private float wanderSpeed = 3f;
    private float walkSpeed = 4.5f;
    private float runSpeed = 5.5f;
    private float sprintSpeed = 7f;

    private Vector3 wanderDest;
    private bool wandering = true;
    private bool validPath = false;
    public bool spawned = true;

    private float maxWaitTime = 35f;
    private float waitTime = 0;
    

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        vis = GetComponent<VisibilityLogic>();
        healthScript = GetComponent<MonsterHealth>();

        rayParent = transform.Find("Ray Transforms");
        rayTransforms = rayParent.GetComponentsInChildren<Transform>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (!moveEnabled)
            return;

        waitTime += Time.deltaTime;
        if(waitTime >= maxWaitTime && !vis.isBurned && !seenPlayer && canMove && moveEnabled)
        {
            Debug.Log("Borring, repositioning.");
            waitTime = 0f;
            vis.StartCoroutine(vis.Reposition(true));
        }

        canMove = !vis.isBurned;

        MovementLogic();
	}

    //TODO: Graphic rotate to face player. We don't want to rotate the root.
    private void MovementLogic()
    {
        if (!canMove || healthScript.isDead)
        {
            agent.SetDestination(transform.position);
            if(agent.pathStatus != NavMeshPathStatus.PathComplete)
            {
                Debug.Log("Cannot reach player. Repositioning...");
                vis.StartCoroutine(vis.Reposition(true));
            }
            return;
        }
        else if(seenPlayer)
        {
            // Chase
            float dist = Vector3.Distance(transform.position, playerTransform.position);

            if(dist <= sprintDist && agent.speed != sprintSpeed)
            {
                Debug.Log("Sprint");
                agent.speed = sprintSpeed;
                MonsterSoundManager._Instance.SetLoop(MonsterSoundManager._Instance.closeSound);
            }else if(dist > sprintDist && dist <= runDist && agent.speed != runSpeed)
            {
                Debug.Log("Run");
                MonsterSoundManager._Instance.SetLoop(MonsterSoundManager._Instance.midSound);
                agent.speed = runSpeed;
            }
            else if(agent.speed != walkSpeed && dist > sprintDist && dist > runDist)
            {
                Debug.Log("Walk");
                MonsterSoundManager._Instance.SetLoop(null);
                agent.speed = walkSpeed;
            }

            if (dist <= agent.stoppingDistance)
            {
                // We caught the player! Game Over.
                playerTransform.GetComponent<PlayerHealth>().Die();
            }
            else
            {
                agent.SetDestination(playerTransform.position);
            }
        }
        else
        {
            MonsterSoundManager._Instance.SetLoop(null);
            // """"Wander"""""
            // Technically we're wandering but with the intention of always eventually finding the player.
            agent.speed = wanderSpeed;
            foreach (Transform t in rayTransforms)
            {
                Ray ray = new Ray(t.position, (playerTransform.position - transform.position));
                Debug.DrawLine(transform.position, (playerTransform.position - transform.position) * 100, Color.blue, 0.2f);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (hit.transform.root.gameObject.CompareTag("Player"))
                    {
                        seenPlayer = true;
                        break;
                    }
                }
            }

            if(Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance && !wandering)
            {
                wandering = true;
            }

            if (wandering)
            {
                agent.SetDestination(playerTransform.position);
            }
        }

        if(!spawned)
        {
            NavMeshPath pathToPlayer = new NavMeshPath();
            agent.CalculatePath(agent.destination, pathToPlayer);
            if (pathToPlayer.status == NavMeshPathStatus.PathComplete)
            {
                //Debug.Log("Good path");
            }
            else
            {
                Debug.LogWarning("Bad path");
                // vis.StartCoroutine(vis.Reposition(true));
            }
            if (!agent.isOnNavMesh)
            {
                //vis.StartCoroutine(vis.Reposition(true));
                agent.Warp(vis.safetySpawnPoint.position);
            }
        }   
    }
}
