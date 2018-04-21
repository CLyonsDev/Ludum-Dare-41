using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : MonoBehaviour {

    // Right now this will be a static variable, but how about having the throw force charge up?
    private float throwForce = 10f;
    private float travelTime = 0f;
    private float distMod = 7f;
    private bool isThrown = false;
    public bool isGrabbable = false;
    private Rigidbody rb;
    private Transform hand;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        hand = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1") && !isThrown)
        {
            travelTime = 0.0f;
            isThrown = true;
            transform.SetParent(null);  // Become Batman

            rb.isKinematic = false;
            rb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
            CompanionState._Instance.SetState(CompanionState.CompanionStateList.fetching);


            CompanionNeeds._Instance.AddHappy(10);

            //! DONT DO THIS EVER. OFFICIAL GAMEJAM WORKAROUND(tm)(c)
            CompanionNeeds._Instance.transform.GetComponent<CompanionMovement>().reachedDest = false;
        }

        if (isThrown)
        {
            travelTime += Time.deltaTime;
        }
	}

    private void OnTriggerEnter(Collider col)
    {
        if(col.transform.tag == "Player" && isThrown)
        {
            ReturnBall();
        }else if(col.transform.tag == "Companion" && isThrown)
        {

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.root.tag != "Companion")
        {
            // Debug.Log("Grabbable");
            isGrabbable = true;
        }
    }

    public void ReturnBall()
    {
        Debug.LogWarning(travelTime * distMod);

        CompanionNeeds._Instance.AddHappy(travelTime * distMod);
        CompanionState._Instance.currentState = CompanionState.CompanionStateList.following;

        isGrabbable = false;
        isThrown = false;
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        rb.isKinematic = true;
    }
}
