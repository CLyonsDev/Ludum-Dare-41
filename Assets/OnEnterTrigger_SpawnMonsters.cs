using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterTrigger_SpawnMonsters : MonoBehaviour {

    public GameObject monsterPrefab;
    private GameObject[] spawnLocs;
    private float minDelay = 1f;
    private float maxDelay = 3f;

    private bool triggered = false;

	// Use this for initialization
	void Start () {
        spawnLocs = GameObject.FindGameObjectsWithTag("MonsterSpawn");
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag == "Player" && !triggered)
        {
            triggered = true;
            StartCoroutine(SpawnRoutine());
        }
    }

    private IEnumerator SpawnRoutine()
    {
        for (int i = 0; i < spawnLocs.Length; i++)
        {
            Instantiate(monsterPrefab, spawnLocs[i].transform.position, spawnLocs[i].transform.rotation);
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }
}
