using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {

    private bool isDead = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //TODO: Actually do something ""Scary"" when you die.
    public void Die()
    {
        if(!isDead)
        {
            isDead = true;
            Debug.LogError("You died.");
            CompanionNeeds._Instance.RevertNeeds();
            PlayerInventory._Instance.RevertInventory();
            LevelManager._Instance.Restart();
        }
    }
}
