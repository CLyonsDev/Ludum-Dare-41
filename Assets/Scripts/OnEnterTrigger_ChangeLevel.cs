using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterTrigger_ChangeLevel : MonoBehaviour {

    public float lerpSpeed = 0.1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            LevelManager._Instance.lerpSpeed = this.lerpSpeed;
            Debug.Log("Level Completed.");
            LevelManager._Instance.NextLevel();
        }
    }
}
