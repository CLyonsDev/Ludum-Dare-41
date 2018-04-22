using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityLogic : MonoBehaviour
{

    public GameObject placeholder;

    private Transform playerTransform;
    private Transform safetySpawnPoint;
    private FlashlightLogic flashlight;
    private MonsterHealth healthScript;
    private MonsterMovement moveScript;

    public bool isSeen = false;     // Are we in camera view?
    public bool isBurned = false;   // Are we being burned?
    private bool wasBurned = false;

    private float minRelocationDist = 15f;
    private float maxRelocationDist = 50f;

    // Use this for initialization
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        safetySpawnPoint = GameObject.FindGameObjectWithTag("MonsterSafetySpawn").transform;
        flashlight = playerTransform.GetComponentInChildren<FlashlightLogic>();
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

        //TODO: Remove this
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Reposition(true);
            StartCoroutine(Reposition(true));
        }
    }

    public IEnumerator Reposition(bool overrideTests)
    {
        if((isSeen && wasBurned) || overrideTests)
        {
            bool b = false;
            int loops = 0;
            int maxLoops = 100;
            while (!b)
            {
                Vector2 circle = Random.insideUnitCircle * maxRelocationDist;
                Vector3 pos = new Vector3(circle.x, 0, circle.y);
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
                        Ray r = new Ray(new Vector3(pos.x, pos.y + 2, pos.z), Vector3.down * 100);
                        Debug.DrawRay(new Vector3(pos.x, pos.y + 2, pos.z), Vector3.down * 100, Color.red, 25f);
                        if (Physics.Raycast(r))
                        {
                            Debug.Log("Location Found.");
                            this.GetComponent<UnityEngine.AI.NavMeshAgent>().Warp(pos);
                            Destroy(g);
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

    /*public void Reposition(bool overrideTests)
    {
        if ((isSeen && wasBurned) || overrideTests)
        {
            Debug.Log("Reposition(). Override: " + overrideTests);
            // bool b = false;
            while (true)
            {
                // We to pick a point that is further away than min, but closer than max
                // Random.InsideUnitCircle is from 0-1 with 0.5 being the center.
                // Vector3 pos = new Vector3(Random.Range(-maxRelocationDist, maxRelocationDist), 0 , Random.Range(-maxRelocationDist, maxRelocationDist));
                float range = Random.Range(minRelocationDist, maxRelocationDist);
                float angle = Random.Range(0.0f, Mathf.PI * 2);
                Vector3 pos = new Vector3(Mathf.Sign(angle), 0, Mathf.Cos(angle));
                pos *= range;
                pos += player.transform.position;

                if (overrideTests)
                    Debug.Log(pos);

                Ray r = new Ray(new Vector3(pos.x, pos.y + 1f, pos.z), Vector3.down);
                if (Physics.Raycast(r))
                {
                    if (overrideTests)
                        Debug.Log("Passes raytest");

                    Vector3 prevPos = transform.position;
                    //transform.position = pos;
                    /*if (GetComponentInChildren<Renderer>().isVisible && !overrideTests)
                    {
                        if (overrideTests)
                            Debug.Log("Bad Position.");

                        transform.position = prevPos;
                    }*/
                    /*Vector3 viewAngle = Camera.main.WorldToViewportPoint(transform.position);
                    if (viewAngle.x > -0.3f)
                    {
                        Debug.Log("Bad Position.");
                    }
                    else
                    {
                        Debug.Log("Good Position! " + pos);
                        Debug.Log(viewAngle);
                        isSeen = false;
                        isBurned = false;
                        wasBurned = false;
                        break;
                    }
                }
            }
        }

        // Debug.Log("End");
        isSeen = false;
        isBurned = false;
        wasBurned = false;
    }*/
}
