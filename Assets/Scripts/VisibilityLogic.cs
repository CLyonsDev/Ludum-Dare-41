using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityLogic : MonoBehaviour
{

    public GameObject placeholder;

    private Transform playerTransform;
    private FlashlightLogic flashlight;
    private MonsterHealth healthScript;
    private MonsterMovement moveScript;

    public bool isSeen = false;     // Are we in camera view?
    public bool isBurned = false;   // Are we being burned?
    private bool wasBurned = false;

    private float minRelocationDist = 30f;
    private float maxRelocationDist = 50f;

    // Use this for initialization
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
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
    }

    public void Reposition(bool overrideTests)
    {
        if((isSeen && wasBurned) || overrideTests)
        {
            // Debug.Log("Reposition(). Override: " + overrideTests);
            isSeen = false;
            wasBurned = false;

            bool b = false;
            while (!b)
            {
                Vector3 pos = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)) * Random.Range(minRelocationDist, maxRelocationDist);
                pos += playerTransform.position;

                if (Vector3.Distance(pos, playerTransform.position) >= minRelocationDist)
                {
                    Vector3 point = Camera.main.WorldToViewportPoint(pos);
                    bool isOnScreen = (point.z > 0 && point.x > 0 && point.x < 1 && point.y > 0 && point.y < 1);
                    if (true) //!isOnScreen
                    {
                        //Ray r = new Ray(new Vector3(pos.x, pos.y + 1f, pos.z), Vector3.down);
                        if (Physics.Raycast(new Vector3(pos.x, pos.y + 1f, pos.z), Vector3.down, Mathf.Infinity))
                        {
                            GameObject g = (GameObject)Instantiate(placeholder, pos, Quaternion.identity);
                            if (!g.GetComponent<Renderer>().isVisible)
                            {
                                if(Vector3.Distance(g.transform.position, playerTransform.position) >= minRelocationDist)
                                {
                                    // Debug.LogWarning(pos);
                                    transform.position = pos;
                                    b = true;
                                    GameObject.Destroy(g);
                                    break;
                                    // Debug.Log(Vector3.Distance(pos, playerTransform.position));
                                }
                            }
                            GameObject.Destroy(g);
                        }
                    }
                }
            }
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
