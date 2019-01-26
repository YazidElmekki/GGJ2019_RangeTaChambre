using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
	public Vector2 gravityModifier = new Vector2(0.0f, 1.0f);

	protected Vector2 targetVelocity;

	protected Vector2 velocity;
	public Vector2 Velocity { get { return velocity; } }

	protected Rigidbody2D objectRigidBody;
	protected ContactFilter2D contactFilter;
	protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
	protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

	protected const float minMoveDistance = 0.001f;
	protected const float shellRadius = 0.01f;

	private void OnEnable()
	{
		objectRigidBody = GetComponent<Rigidbody2D>();
	}

	void Start()
	{
		contactFilter.useTriggers = false;
		contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
		contactFilter.useLayerMask = true;
	}

	protected virtual void ComputeVelocity()
	{
	}

	void Update()
	{
		targetVelocity = Vector2.zero;
		ComputeVelocity();
	}

	protected virtual void ApplyGravity()
	{
		velocity += gravityModifier * Physics2D.gravity * Time.fixedDeltaTime;
		velocity.x = targetVelocity.x + gravityModifier.x;
		velocity.y = targetVelocity.y + gravityModifier.y;
	}

	private void FixedUpdate()
	{
		ApplyGravity();

		Vector2 deltaPosition = velocity * Time.fixedDeltaTime;

		Vector2 moveHorizontal = new Vector2(deltaPosition.x, 0.0f);
		Vector2 moveVertical = new Vector2(0.0f, deltaPosition.y);

		Movement(moveHorizontal);
		Movement(moveVertical);
	}

	void Movement(Vector2 move)
	{
		float distance = move.magnitude;

		if (distance > minMoveDistance)
		{
			int hitCount = objectRigidBody.Cast(move, contactFilter, hitBuffer, distance + shellRadius);

			hitBufferList.Clear();

			for (int i = 0; i < hitCount; ++i)
			{
				hitBufferList.Add(hitBuffer[i]);
			}

			for (int i = 0; i < hitBufferList.Count; ++i)
			{
				//Vector2 currentNormal = hitBufferList[i].normal;
				//currentNormal.y *= Mathf.Sign(gravityModifier.y);

				//float projection = Vector2.Dot(velocity, currentNormal);

				//if (projection < 0.0f)
				//{
				//	velocity = velocity - projection * currentNormal;
				//}

				float modifiedDistance = hitBufferList[i].distance - shellRadius;
				distance = modifiedDistance < distance ? modifiedDistance : distance;
			}
		}

		objectRigidBody.position = objectRigidBody.position + move.normalized * distance;
	}
}
