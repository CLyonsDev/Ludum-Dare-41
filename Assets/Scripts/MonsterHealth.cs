using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealth : MonoBehaviour {

    private VisibilityLogic visLogic;

    public float maxHealth = 100;
    private float respawnTime = 20f;
    public float currentHealth;
    private bool takingDamage = false;
    public bool isDead = false;

    private Coroutine dotCoroutine;

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
        visLogic = GetComponent<VisibilityLogic>();
	}

    public void StartDamage(float delay, float amt)
    {
        if (dotCoroutine != null || takingDamage)
            return;

        StopDamage();

        dotCoroutine = StartCoroutine(TakeDamageOverTime(delay, amt));
    }

    public void StopDamage()
    {
        if (dotCoroutine != null)
            StopCoroutine(dotCoroutine);

        dotCoroutine = null;
        takingDamage = false;
    }


    private IEnumerator TakeDamageOverTime(float delay, float amt)
    {
        takingDamage = true;

        while (true && !isDead)
        {
            // Debug.Log("Taking Damage");
            currentHealth -= amt;
            if (currentHealth <= 0)
            {
                StartCoroutine(Respawn());
                StopDamage();
                break;
            }
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator Respawn()
    {
        if(visLogic == null)
        {
            Debug.LogError("Error. VisLogic.cs not found. Aborting.");
            yield return null;
        }

        isDead = true;
        Debug.Log("MONSTER DOWN!");
        // yield return new WaitForSeconds(1);
        // Debug.Log("Respawning");
        MonsterSoundManager._Instance.SetLoop(MonsterSoundManager._Instance.farSound);
        GetComponent<MonsterMovement>().seenPlayer = false;
        MonsterSoundManager._Instance.SetLoop(null);
        StartCoroutine(visLogic.Reposition(false));

        yield return new WaitForSeconds(respawnTime);

        isDead = false;
        currentHealth = maxHealth;
    }
}
