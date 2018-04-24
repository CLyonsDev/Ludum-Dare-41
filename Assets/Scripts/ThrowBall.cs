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
    public bool grabbedByCompanion = false;
    private Rigidbody rb;
    private Transform hand;
    CompanionSoundManager soundManager;

    private bool shouldSwitch = false;
    public bool canSwitch = true;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        hand = transform.parent;
        soundManager = CompanionSoundManager._Instance;

    }
	
	// Update is called once per frame
	void Update () {
        if (CollectItem._Instance.lookingAtItem || CollectItem._Instance.lookingAtCompanion)
            return;

        if (Input.GetButtonDown("Fire1") && !isThrown && !CollectItem._Instance.lookingAtItem)
        {
            soundManager.PlaySound(soundManager.fetch);
            travelTime = 0.0f;
            isThrown = true;
            transform.SetParent(null);  // Become Batman

            rb.isKinematic = false;
            rb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
            CompanionState._Instance.SetState(CompanionState.CompanionStateList.fetching);


            CompanionNeeds._Instance.AddHappy(10);

            //! DONT DO THIS EVER. OFFICIAL GAMEJAM WORKAROUND(tm)(c)
            CompanionSoundManager._Instance.transform.GetComponent<CompanionMovement>().reachedDest = false;

            canSwitch = false;
        }

        if (isThrown)
        {
            travelTime += Time.deltaTime;
        }

        if(transform.position.y <= -30)
        {
            // We have probably fallen out of the map.
            Debug.Log("Ball has fallen out of map!");
            ReturnBall(false);
        }
	}

    private void OnTriggerEnter(Collider col)
    {
        if(col.transform.tag == "Player" && isGrabbable && !grabbedByCompanion)
        {
            ReturnBall(false);
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

    public void DeactivateBall()
    {
        Debug.Log("Deactivate Ball");
        shouldSwitch = true;
        if (canSwitch)
        {
            this.gameObject.SetActive(false);
            shouldSwitch = false;
        }
    }

    public void ReturnBall(bool grabbedByCompanion)
    {
        if (grabbedByCompanion) {
            Debug.LogWarning(travelTime * distMod);
            CompanionNeeds._Instance.AddHappy(travelTime * distMod);
            soundManager.PlaySound(soundManager.fetch);
            CompanionState._Instance.currentState = CompanionState.CompanionStateList.following;
        }

        isGrabbable = false;
        isThrown = false;
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        rb.isKinematic = true;
        grabbedByCompanion = false;

        canSwitch = true;
        if (shouldSwitch)
        {
            Debug.Log("ShouldSwitch");
            this.gameObject.SetActive(false);
            shouldSwitch = false;
        }
    }
}
