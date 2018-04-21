using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionState : MonoBehaviour {

    public enum CompanionStateList
    {
        idle,
        following,
        fetching,
        alert
    };

    public CompanionStateList currentState;

    public static CompanionState _Instance;

    private void Awake()
    {
        if (_Instance == null)
            _Instance = this;
        else
            Destroy(this);
    }

    // Use this for initialization
    void Start () {
        currentState = CompanionStateList.following;
	}
	
    public void SetState(CompanionStateList newState)
    {
        currentState = newState;
        Debug.Log("Switching state to " + newState);
    }
}
