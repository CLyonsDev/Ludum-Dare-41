using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour {

    public float slowRotSpeed = 50;
    public float medRotSpeed = 100;
    public float fastRotSpeed = 200;

    private float rotSpeed;

    private void Start()
    {
        SetRotSpeed(0);
    }

    // 0 = slow, 1 = med, 2 = fast
    public void SetRotSpeed(int index)
    {
        switch (index)
        {
            case 0:
                rotSpeed = slowRotSpeed;
                break;
            case 1:
                rotSpeed = medRotSpeed;
                break;
            case 2:
                rotSpeed = fastRotSpeed;
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        transform.Rotate(transform.up, rotSpeed * Time.fixedDeltaTime);
	}
}
