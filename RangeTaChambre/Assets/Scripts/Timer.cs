﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour {

    [SerializeField]
    float RoundTimer; // en seconde
    [SerializeField]
    Text timerTxt;
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

	[SerializeField]
	private float bedHelpRemainingTime = 5.0f;

	public event Action OnShowBedHelp;

    float moitiederound;
    float time;
    int timeinInt;
    bool HalfTime;

    // Use this for initialization
    void Start () {
        moitiederound = RoundTimer / 2.0f;
        time = moitiederound;
        timeinInt = (int)time;
        HalfTime = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            timeinInt = (int)time;
            timerTxt.text = timeinInt.ToString();

			if (time < bedHelpRemainingTime)
			{
				OnShowBedHelp?.Invoke();
			}
        }

        else
        {
			if (HalfTime)
			{
				if (EndRound != null)
					EndRound();

				time = moitiederound / 2;

				if (time <= MinTimeDaronne)
					time = MinTimeDaronne;
			}
			else
			{
				if (DaronneIntervention != null)
					DaronneIntervention();

				time = moitiederound;

				HalfTime = true;
			}
        }
    }
}
