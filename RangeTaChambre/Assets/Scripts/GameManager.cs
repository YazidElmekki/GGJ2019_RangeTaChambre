﻿using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Daronne daronne;
    [SerializeField]
    public Player PlayerOne, PlayerTwo;

    public static GameManager Instance
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<GameManager>();

			return instance;
		}
	}

	private static GameManager instance;

	int player1Score;
	int player2Score;

	public event System.Action<int, int> ScoreChanged;

    private void Start()
    {
		if (Timer.Instance != null)
		{
			Timer.Instance.DaronneIntervention += UseDaronneIntervention;
			Timer.Instance.EndRound += UseEndRound;
		}
    }

    private void Update()
	{
		if (Input.GetKeyDown(KeyCode.O))
			OnPlayer1Scored(5);

		if (Input.GetKeyDown(KeyCode.P))
			OnPlayer2Scored(5);
	}

	public void OnPlayer1Scored(int points)
	{
		player1Score += points;
		ScoreChanged?.Invoke(player1Score, player2Score);
	}

	public void OnPlayer2Scored(int points)
	{
		player2Score += points;
		ScoreChanged?.Invoke(player1Score, player2Score);
	}

    void UseDaronneIntervention()
    {
        daronne.Intervention();
        /*
         * if (!PlayerOne.IsHidden)
         *      player1Score -= x;
         * 
         * if (!PlayerTwo.isHidden)
         *      player2Score -= x;
         */
    }

    void UseEndRound()
    {
        if (player1Score == player2Score)
            return;

        //if (player1Score > player2Score)
        //    //Player1Win
        //else
        //    //Player2Win
        daronne.EndRound();
    }

    public Player GetPlayer(int playerIndex)
    {
        if (PlayerOne.PlayerIndex == playerIndex)
            return PlayerOne;

        else if (PlayerTwo.PlayerIndex == playerIndex)
            return PlayerTwo;

        else
            return null;
    }
}