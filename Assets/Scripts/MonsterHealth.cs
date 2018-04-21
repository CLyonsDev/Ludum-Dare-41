using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealth : MonoBehaviour {

    private VisibilityLogic visLogic;

    public float maxHealth = 100;
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
            Debug.Log("Taking Damage");
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
        isDead = true;
        Debug.Log("MONSTER DOWN!");
        yield return new WaitForSeconds(1);
        Debug.Log("Respawning");
        visLogic.Reposition(false);
        isDead = false;
        currentHealth = maxHealth;
    }
}
