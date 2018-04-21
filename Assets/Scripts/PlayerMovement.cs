using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody characterRigidbody;

    private float moveSpeed = 5f;


	// Use this for initialization
	void Start () {
        characterRigidbody = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {
        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        Vector3 horizV3 = transform.right * horiz;
        Vector3 vertV3 = transform.forward * vert;
        Vector3 finalV3 = horizV3 + vertV3;

        characterRigidbody.MovePosition(transform.position + finalV3 * moveSpeed * Time.fixedDeltaTime);
	}
}
