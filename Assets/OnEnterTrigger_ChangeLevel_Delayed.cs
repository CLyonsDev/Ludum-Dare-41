using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterTrigger_ChangeLevel_Delayed : MonoBehaviour
{

    public float delay = 3f;
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player") && !triggered)
        {
            triggered = true;
            StartCoroutine(ChangeLevelDelayed());
        }
    }

    private IEnumerator ChangeLevelDelayed()
    {
        yield return new WaitForSeconds(delay);
        LevelManager._Instance.NextLevel();
    }
}
