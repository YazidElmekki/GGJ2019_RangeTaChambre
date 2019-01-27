using UnityEngine;
using UnityEngine.UI;

public class RuckusGauge : MonoBehaviour
{
	[SerializeField]
	RectTransform player1Gauge;
	[SerializeField]
	RectTransform player2Gauge;

	private void Start()
	{
		GameManager.Instance.ScoreChanged += OnScoreChanged;

		player1Gauge.localScale = new Vector3(0.5f, 1f, 1f);
		player2Gauge.localScale = new Vector3(0.5f, 1f, 1f);
	}

	private void OnScoreChanged(int player1Score, int player2Score)
	{
		float player1Ratio = 0.0f;

		if (player1Score == 0 && player2Score == 0)
		{
			player1Ratio = 0.5f;
		}
		else
		{ 
			player1Ratio = (float)player1Score / (player1Score + player2Score);
		}

		player1Gauge.localScale = new Vector3(player1Ratio, 1f, 1f);
		player2Gauge.localScale = new Vector3(1f - player1Ratio, 1f, 1f);
	}
}