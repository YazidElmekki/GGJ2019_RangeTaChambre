using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Daronne daronne;
    [SerializeField]
    public Player PlayerOne, PlayerTwo;

	[SerializeField]
	private Text player1ScoreText;

	[SerializeField]
	private Text player2ScoreText;

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
		player1ScoreText.text = "P1 : " + player1Score;
		player2ScoreText.text = "P2 : " + player2Score;
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
        daronne.Intervention();

        if (player1Score == player2Score)
            return;

        daronne.EndRound();

        if (player1Score > player2Score)
            Win(PlayerOne.PlayerIndex);
        else if (player1Score < player2Score)
            Win(PlayerTwo.PlayerIndex);
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

    void Win(int playerIndex)
    {
        Debug.Log("EndGame, Add final screen here");

        //Time.timeScale = 0.1f;

        //yield return new WaitForSeconds(5);

        SceneManager.LoadScene("MainMenu");

        //Load final Screen of win
    }
}