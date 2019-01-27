using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;


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

    [SerializeField]
    int loossingScoreWhenDaronne = 2;

	[SerializeField]
	private float winTime = 3.0f;

	[SerializeField]
	private GameObject winScreenObject;

	[SerializeField]
	private Text winText;

	[Header("SD sound")]
	[SerializeField]
	private AudioClip scorePointAudioClip;

	[SerializeField]
	private AudioClip loosePointAudioClip;

	[SerializeField]
	private AudioClip playerWinSFX;

	[SerializeField]
	AudioSource SFXAudioSource;


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
		SFXAudioSource = GetComponent<AudioSource>();

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
		if (SFXAudioSource.isPlaying == false)
		{
			if (points > 0)
			{
				SFXAudioSource.PlayOneShot(scorePointAudioClip);
			}
			else if (points < 0)
			{
				SFXAudioSource.PlayOneShot(loosePointAudioClip);
			}
		}

		if (playerIndex == 0)
			player1Score += points;
		else
			player2Score += points;

		player1Score = Mathf.Max(0, player1Score);
		player2Score = Mathf.Max(0, player2Score);


		ScoreChanged(player1Score, player2Score);
	}

    void UseDaronneIntervention()
    {
        daronne.Intervention();

        if (!PlayerOne.isHidden)
            player1Score /= loossingScoreWhenDaronne;
        
        if (!PlayerTwo.isHidden)
            player2Score /= loossingScoreWhenDaronne;

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

		StartCoroutine(WinCoroutine(playerIndex));

        //Time.timeScale = 0.1f;

        //yield return new WaitForSeconds(5);


        //Load final Screen of win
    }


	private IEnumerator WinCoroutine(int winPlayerIndex)
	{
		winText.text = "PLAYER " + (winPlayerIndex +1) + " WIN !";
		winScreenObject.SetActive(true);

		yield return new WaitForSeconds(winTime);

		SceneManager.LoadScene("MainMenu");
	}
}