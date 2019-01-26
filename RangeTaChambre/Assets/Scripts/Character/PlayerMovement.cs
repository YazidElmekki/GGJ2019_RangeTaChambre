using UnityEngine;

public class PlayerMovement : PhysicsObject
{
	private Player player;

	enum MoveState
	{
		NO_MOVE,
		IDLE,
		MOVING
	}

	private MoveState currentState = MoveState.IDLE;

	[SerializeField]
	private float moveSpeed = 5.0f;

	private void Start()
	{
		player = GetComponent<Player>();
	}

	protected override void ComputeVelocity()
	{
		if (currentState != MoveState.NO_MOVE)
		{
			Vector2 move = Vector2.zero;

			move.x = InputManager.Instance.GetAxisAction(AxisActionEnum.MOVE_RIGHT, player.PlayerIndex);
			move.y = InputManager.Instance.GetAxisAction(AxisActionEnum.MOVE_UP, player.PlayerIndex);

			targetVelocity = move * moveSpeed;

			UpdateState();
		}
	}

	private void UpdateState()
	{
		if (currentState == MoveState.IDLE && targetVelocity.x != 0.0f && targetVelocity.y != 0.0f)
		{
			currentState = MoveState.MOVING;
		}
		else if (currentState == MoveState.MOVING && targetVelocity.x == 0.0f && targetVelocity.y == 0.0f)
		{
			currentState = MoveState.IDLE;
		}
	}
}