using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //TODO: Actually do something ""Scary"" when you die.
    public void Die()
    {
        Debug.LogError("You died.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
