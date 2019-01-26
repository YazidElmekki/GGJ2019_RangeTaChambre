using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumObject : MonoBehaviour {

    enum State
    {
        CARRIED = 0,
        DOWN,
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Drop()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.name == "")
    }
}
