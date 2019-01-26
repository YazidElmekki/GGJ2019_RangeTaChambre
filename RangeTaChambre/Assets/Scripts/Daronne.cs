using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daronne : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        //Timer.Instance.DaronneIntervention += Intervention;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Intervention()
    {
        /*
         * if (!PlayerOne.IsHidden)
         *      PlayerOne.Point -= x;
         * 
         * if (!PlayerTwo.isHidden)
         *      PlayerTwo.Point -= x;
         */
    }

    private void OnDestroy()
    {
        //Timer.Instance.DaronneIntervention -= Intervention;
    }
}
