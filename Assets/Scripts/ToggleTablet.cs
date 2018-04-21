using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleTablet : MonoBehaviour {

    private Animator anim;
    private bool isEnabled = false;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isEnabled = !isEnabled;

            if (isEnabled)
            {
                anim.SetTrigger("Deploy");
                CursorManager._Instance.SetCursor(false);
            }
            else
            {
                anim.SetTrigger("Retract");
                CursorManager._Instance.SetCursor(true);
            }
        }
	}
}
