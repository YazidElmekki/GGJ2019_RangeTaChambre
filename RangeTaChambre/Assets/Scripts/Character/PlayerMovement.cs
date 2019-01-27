using UnityEngine;
using System.Collections;

public class PlayerMovement : PhysicsObject
{
	private Player player;

	enum MoveState
	{
		NO_MOVE,
		IDLE,
		MOVING,
		STUN
	}

	private MoveState currentState = MoveState.IDLE;

	[SerializeField]
	private float moveSpeed = 5.0f;

	private Animator playerAnimator;

	[SerializeField]
	private float stunTime = 3.0f;
	private Coroutine stunCouroutine = null;

	[Header("SD audio clips")]
	[SerializeField]
	private AudioClip footStepAudioClip;

	[SerializeField]
	private AudioClip slowFootStepAudioClip;

	[SerializeField]
	private AudioClip stunAudioClip;

	private AudioSource playerAudioSource;

	private void Start()
	{
		player = GetComponent<Player>();
		playerAnimator = GetComponent<Animator>();
		playerAudioSource = GetComponent<AudioSource>();
	}

	protected override void ComputeVelocity()
	{
		if (currentState != MoveState.NO_MOVE && currentState != MoveState.STUN)
		{
			Vector2 move = Vector2.zero;

			move.x = InputManager.Instance.GetAxisAction(AxisActionEnum.MOVE_RIGHT, player.PlayerIndex);
			move.y = InputManager.Instance.GetAxisAction(AxisActionEnum.MOVE_UP, player.PlayerIndex);

            if (player.toyHasTaken == null)
                targetVelocity = move * moveSpeed;
            else
                targetVelocity = move * (moveSpeed - player.toyHasTaken.WalkSlowdown);

			UpdateState();
		}
	}

	private void UpdateState()
	{
		if (currentState == MoveState.IDLE && (targetVelocity.x != 0.0f || targetVelocity.y != 0.0f))
		{
			currentState = MoveState.MOVING;
			playerAnimator.ResetTrigger("IdleTrigger");
			playerAnimator.SetTrigger("MoveTrigger");
		}
		else if (currentState == MoveState.MOVING && (targetVelocity.x == 0.0f && targetVelocity.y == 0.0f))
		{
			currentState = MoveState.IDLE;
			playerAnimator.ResetTrigger("MoveTrigger");
			playerAnimator.SetTrigger("IdleTrigger");
		}
	}

	private void OnFootStep()
	{
		if (playerAudioSource.isPlaying == false)
		{
			if (player.toyHasTaken != null && player.toyHasTaken.CurrentObjectType == Toy.ObjectType.BIG)
			{
				playerAudioSource.PlayOneShot(slowFootStepAudioClip);
			}
			else
			{
				playerAudioSource.PlayOneShot(footStepAudioClip);
			}

		}
	}

	public void Stun()
	{
		if (stunCouroutine == null && currentState != MoveState.STUN)
		{
			if (currentState == MoveState.MOVING)
			{
				playerAnimator.ResetTrigger("MoveTrigger");
				playerAnimator.SetTrigger("IdleTrigger");
			}

			currentState = MoveState.STUN;
			StartCoroutine(ExitFromStun());
			playerAudioSource.PlayOneShot(stunAudioClip);
		}
	}

	private IEnumerator ExitFromStun()
	{
		yield return new WaitForSeconds(stunTime);
		currentState = MoveState.IDLE;
		stunCouroutine = null;
	}
}