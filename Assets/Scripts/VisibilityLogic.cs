using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityLogic : MonoBehaviour
{

    public GameObject placeholder;

    private Transform playerTransform;
    public Transform safetySpawnPoint;
    private FlashlightLogic flashlight;
    private MonsterHealth healthScript;
    private MonsterMovement moveScript;

    public bool isSeen = false;     // Are we in camera view?
    public bool isBurned = false;   // Are we being burned?
    private bool wasBurned = false;

    private float minRelocationDist = 25f;
    private float maxRelocationDist = 40f;

    // Use this for initialization
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        safetySpawnPoint = GameObject.FindGameObjectWithTag("MonsterSafetySpawn").transform;
        flashlight = playerTransform.GetComponentInChildren<FlashlightLogic>();
        moveScript = GetComponent<MonsterMovement>();
        healthScript = GetComponent<MonsterHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (flashlight == null)
            return;
        //Debug.Log(Camera.main.WorldToViewportPoint(new Vector3(transform.position.x, transform.position.y, transform.position.z)));
        if (GetComponentInChildren<Renderer>().isVisible)
        {
            isSeen = true;
            if (flashlight.lightEnhanced)
            {
                isBurned = true;
                wasBurned = true;

                healthScript.StartDamage(flashlight.flashlightInterval, flashlight.flashlightDamage);

            }
            else
            {
                isBurned = false;
                healthScript.StopDamage();
            }
            //Debug.Log("Visible");
        }
        else
        {
            healthScript.StopDamage();
            Reposition(false);
        }
    }

    public IEnumerator Reposition(bool overrideTests)
    {
        moveScript.spawned = false;

        if((isSeen && wasBurned) || overrideTests)
        {
            bool b = false;
            int loops = 0;
            int maxLoops = 100;
            while (!b)
            {
                Vector2 circle = Random.insideUnitCircle * maxRelocationDist;
                //Random.Range(-20, 20)
                Vector3 pos = new Vector3(circle.x, Random.Range(-20, 20), circle.y);
                GameObject g = (GameObject)Instantiate(placeholder, pos, Quaternion.identity);

                if(Vector3.Distance(g.transform.position, playerTransform.position) <= minRelocationDist)
                {
                    Destroy(g);
                }
                else
                {
                    yield return 0;
                    if (g.GetComponent<Renderer>().isVisible)
                    {
                        Destroy(g);
                    }
                    else
                    {
                        Ray r = new Ray(new Vector3(pos.x, pos.y + 2, pos.z), Vector3.down * 2);
                        RaycastHit hit;
                        Debug.DrawRay(new Vector3(pos.x, pos.y + 2, pos.z), Vector3.down * 2, Color.red, 25f);
                        if (Physics.Raycast(r, out hit))
                        {
                            Debug.Log("Location Found.");
                            pos.y = hit.point.y;
                            this.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(pos);
                            Destroy(g);
                            b = true;
                            break;
                        }
                        else
                        {
                            Destroy(g);
                        }
                    }
                }

                loops++;

                if(loops >= maxLoops)
                {
                    Debug.LogError("Error. No suitable locations found. Aborting...");
                    this.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(safetySpawnPoint.position);
                    break;
                }
            }

            // Debug.Log("Reposition(). Override: " + overrideTests);
            isSeen = false;
            wasBurned = false;
        }
    }
}
