using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {

    public float mouseClamp = 80f;
    public float sensitivity = 2f;
    public float lerpRate = 3f;

    private bool mouseIsLocked = false;

    private Vector2 mouse;
    private Vector2 smoothMouse;

    private Vector2 dir;
    private Vector2 bodyDir;

    private Transform body;

	// Use this for initialization
	void Start () {
        body = transform.root;

        dir = transform.localRotation.eulerAngles;
        bodyDir = body.transform.localRotation.eulerAngles;

        MouseLock(0);
	}
	
	// Update is called once per frame
	void Update () {
        ReadInputs();

        if (mouseIsLocked)
            MouseLookLogic();
	}

    private void MouseLookLogic()
    {
        Quaternion orientation = Quaternion.Euler(dir);
        Quaternion bodyOrientation = Quaternion.Euler(bodyDir);

        // Get mouse movement
        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // Scale by mouse sensitivity
        mouseInput = Vector2.Scale(mouseInput, Vector2.one * lerpRate * sensitivity);

        // Lerp mouse movement
        smoothMouse.x = Mathf.Lerp(smoothMouse.x, mouseInput.x, 1f / lerpRate);
        smoothMouse.y = Mathf.Lerp(smoothMouse.y, mouseInput.y, 1f / lerpRate);

        // Find how far our mouse has moved from 0
        mouse += smoothMouse;

        // Clamp mouse rot (if necessary)
        if (mouseClamp < 180)
            mouse.y = Mathf.Clamp(mouse.y, -mouseClamp, mouseClamp);

        // Rotate our mouse up or down based on our input
        transform.localRotation = Quaternion.AngleAxis(-mouse.y, orientation * Vector3.right) * orientation;

        // Rotate our body based on the X input
        if (body != null)
        {
            Rigidbody rb = body.GetComponent<Rigidbody>();
            // Rotation around the Y axis based on mouseX.
            Quaternion yRot = Quaternion.AngleAxis(mouse.x, Vector3.up);

            // Rotate the body using our new variable
            //body.localRotation = yRot * bodyOrientation;
            rb.MoveRotation(bodyOrientation * yRot);
        }
    }

    private void ReadInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            MouseLock(-1);
        else if (Input.GetMouseButtonDown(0))
            MouseLock(0);
    }

    public void MouseLock(int lockOverride = -1){

        switch (lockOverride)
        {
            case -1:
                mouseIsLocked = !mouseIsLocked;
                break;
            case 0:
                mouseIsLocked = true;
                break;
            case 1:
                mouseIsLocked = false;
                break;
        }

        Cursor.visible = !mouseIsLocked;

        if (mouseIsLocked)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }
}
