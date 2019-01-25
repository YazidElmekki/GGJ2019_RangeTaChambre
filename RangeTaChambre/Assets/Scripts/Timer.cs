using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    [SerializeField]
    float RoundTimer; // en seconde
    [SerializeField]
    Text timer;

    float MiRound;
    float ChangingMiRound;

	// Use this for initialization
	void Start () {
        MiRound = RoundTimer / 2;
        ChangingMiRound = MiRound;
    }
	
	// Update is called once per frame
	void Update ()
    {
        ChangingMiRound -= Time.deltaTime;
        int IntMiRound = (int)ChangingMiRound;
        timer.text = IntMiRound.ToString();

        if (RoundTimer < 0)
        {
            //InterventionDaronne;
        }
    }
}
