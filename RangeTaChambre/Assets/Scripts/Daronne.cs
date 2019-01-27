using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daronne : MonoBehaviour
{
	[Range(int.MinValue, 0)]
	[SerializeField]
	private int loosenPointOnAngryMother = -5;

	private Animator animatorController;

	[SerializeField]
	private AudioClip motherShoutClip;

	private AudioSource motherAudioSource;

	private void Start()
	{
		animatorController = GetComponent<Animator>();
		motherAudioSource = gameObject.AddComponent<AudioSource>();
	}

	public void Intervention()
    {
		animatorController.SetTrigger("AngryTrigger");

		Player[] players = { GameManager.Instance.PlayerOne, GameManager.Instance.PlayerTwo };

		foreach (Player player in players)
		{
			PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

			if (playerMovement.CurrentState != PlayerMovement.MoveState.HIDDEN && playerMovement.CurrentState != PlayerMovement.MoveState.STUN)
			{
				playerMovement.Stun();
				GameManager.Instance.PlayerScored(player.PlayerIndex, loosenPointOnAngryMother);
			}
		}

        //PlayAnim
        Debug.Log("Add animation here || Mi round");
    }

    public void EndRound()
    {
		animatorController.SetTrigger("AngryTrigger");
		//PlayAnim
		Debug.Log("Add animation here || End round");
    }

	private void OnShout()
	{
		motherAudioSource.PlayOneShot(motherShoutClip);
	}
}
