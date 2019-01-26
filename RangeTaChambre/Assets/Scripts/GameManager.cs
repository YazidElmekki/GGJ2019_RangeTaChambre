using UnityEngine;

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

	public void PlayerScored(int playerIndex, int points)
	{
		if (playerIndex == 0)
			player1Score += points;
		else
			player2Score += points;

		ScoreChanged(player1Score, player2Score);
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