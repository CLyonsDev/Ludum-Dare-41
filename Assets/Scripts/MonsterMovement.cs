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

    private bool seenPlayer = false;

    private float runDist = 25f;
    private float sprintDist = 12f;

    private float wanderDist = 10f;

    private float wanderSpeed = 1f;
    private float walkSpeed = 2f;
    private float runSpeed = 4f;
    private float sprintSpeed = 6f;

    private Vector3 wanderDest;
    private bool wandering = true;

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
        else if(seenPlayer)
        {
            // Chase
            float dist = Vector3.Distance(transform.position, playerTransform.position);

            if(dist <= sprintDist)
            {
                agent.speed = sprintSpeed;
            }else if(dist > sprintDist && dist <= runDist)
            {
                agent.speed = runSpeed;
            }
            else
            {
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

            if(Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance)
            {
                wandering = true;
            }

            if (wandering)
            {
                // God please have mercy on my soul
                while (true)
                {
                    Vector2 circle = Random.insideUnitCircle * 10f;
                    Vector3 pos = new Vector3(circle.x, 0, circle.y);
                    pos += playerTransform.position;

                    Ray r = new Ray(new Vector3(pos.x, pos.y + 2, pos.z), Vector3.down * 100);
                    RaycastHit hit;
                    Debug.DrawRay(new Vector3(pos.x, pos.y + 2, pos.z), Vector3.down * 100, Color.yellow, 25f);
                    if (Physics.Raycast(r, out hit))
                    {
                        Debug.Log("Location Found.");
                        pos.y = hit.point.y;
                        wanderDest = pos;
                        Debug.DrawLine(pos, new Vector3(pos.x, pos.y + 100, pos.z), Color.green, 15f);
                        agent.SetDestination(wanderDest);
                        wandering = false;
                        break;
                    }
                }
            }
        }
    }
}
