using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour {

    [SerializeField]
    float RoundTimer; // en seconde
    [SerializeField]
    Text timer;
    [SerializeField]
    float MinTimeDaronne; // en seconde

    private static Timer instance;

    public static Timer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Timer>();
            }

            return instance;
        }
    }

    public event Action DaronneIntervention;
    public event Action EndRound;

    float MiRound;
    float ChangingMiRound;
    int IntMiRound;
    bool HalfTime;

    // Use this for initialization
    void Start () {
        MiRound = RoundTimer / 2;
        ChangingMiRound = MiRound;
        IntMiRound = (int)ChangingMiRound;
        HalfTime = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (IntMiRound > 0)
        {
            ChangingMiRound -= Time.deltaTime;
            IntMiRound = (int)ChangingMiRound;
            timer.text = IntMiRound.ToString();
        }

        else
        {
            if (HalfTime)
            {
                if (EndRound != null)
                {
                    EndRound();
                    ChangingMiRound = MiRound / 2;
                    if (ChangingMiRound <= MinTimeDaronne)
                        ChangingMiRound = MinTimeDaronne;
                    IntMiRound = (int)ChangingMiRound;


                };
                return;
            }

            if (DaronneIntervention != null)
                DaronneIntervention();

            ChangingMiRound = MiRound;
            IntMiRound = (int)ChangingMiRound;

            HalfTime = true;
        }
    }
}
