using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toy : MonoBehaviour, IPlayerZoneTracker
{
	public enum ObjectType
	{
		MEDIUM = 0,
		SMALL,
		BIG,
	}

	public enum State
	{
		CARRIED = 0,
		DOWN,
		THROWN
	}

	[SerializeField]
    private ObjectType objectType;
	public ObjectType CurrentObjectType { get { return objectType; } }

    [SerializeField]
    public float WalkSlowdown;

    public bool FirstValidDrop = true;
    public int PlayerIndex { get; set; }

	public int CurrentZoneIndex
	{
		get;
		set;
	}

	[SerializeField]
	float ThrowDistance = 5;
	[SerializeField]
	float throwSpeed = 10f;
	Vector3 throwStartPos;
	Vector3 throwDirection;
	int throwingPlayerIndex;

    public int Points;

    public bool IsInChest = true;

    State state;

	[SerializeField]
	private Sprite toySprite;
	public Sprite ToySprite { get { return toySprite; } }
    [SerializeField]
    float distanceMinToTidyUp = 4;

    public int Origin;

	// Use this for initialization
	void Start()
    {
        state = State.DOWN;

        GetComponent<Collider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
		if (state == State.THROWN)
		{
			transform.position += throwDirection * Time.deltaTime * throwSpeed;

			if (Vector3.Distance(transform.position, throwStartPos) > ThrowDistance)
			{
				transform.position = throwStartPos + throwDirection * ThrowDistance;
				state = State.DOWN;

				Collider2D[] overlapColliders = Physics2D.OverlapBoxAll(transform.position, GetComponent<BoxCollider2D>().size * 1.25f, 0.0f);

				for (int i = 0; i < overlapColliders.Length; ++i)
				{
					PlayerZone playerZone = overlapColliders[i].GetComponent<PlayerZone>();
					if (playerZone != null && playerZone.ZoneIndex == throwingPlayerIndex && FirstValidDrop)
					{
						GameManager.Instance.PlayerScored(throwingPlayerIndex, Points);
						FirstValidDrop = false;
						GetComponent<Collider2D>().enabled = false;
						break;
					}
				}
			}
		}

        if (objectType == ObjectType.BIG)
        {
            if ((GameManager.Instance.GetPlayer(0).transform.position.y > transform.position.y) || (GameManager.Instance.GetPlayer(1).transform.position.y > transform.position.y))
                GetComponent<SpriteRenderer>().sortingOrder = 11;
            else
                GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (CurrentObjectType == ObjectType.SMALL && state == State.THROWN)
		{
			state = State.DOWN;

			Collider2D[] overlapColliders = Physics2D.OverlapBoxAll(transform.position, GetComponent<BoxCollider2D>().size * 1.25f, 0.0f);

			bool canScore = true;
			PlayerZone currentPlayerZone = null;

			for (int i = 0; i < overlapColliders.Length; ++i)
			{
				if (overlapColliders[i].gameObject.tag == "Player")
				{
					PlayerMovement playerMovement = overlapColliders[i].gameObject.GetComponent<PlayerMovement>();
					playerMovement.Stun();
					Debug.Log("STUN");
					Destroy(gameObject);
					canScore = false;
					break;
				}
				else
				{
					PlayerZone playerZone = overlapColliders[i].GetComponent<PlayerZone>();
					if (playerZone != null)
					{
						currentPlayerZone = playerZone;
					}
				}
			}

			if (canScore == true && currentPlayerZone != null && currentPlayerZone.ZoneIndex == throwingPlayerIndex && FirstValidDrop)
			{
				GetComponent<Collider2D>().enabled = false;
				FirstValidDrop = false;
				GameManager.Instance.PlayerScored(throwingPlayerIndex, Points);
			}
		}
	}

	public enum DropResult
	{
		CHEST,
		NONE,
		ON_COLLIDER
	}

	public DropResult CanDrop()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius);

		int otherCollider = 0;
		
		for (int i = 0; i < colliders.Length; ++i)
		{
			if (colliders[i].gameObject.tag == "Chest")
			{
				Chest chest = colliders[i].GetComponent<Chest>();

				if (chest.playerIndex == PlayerIndex && FirstValidDrop == false)
				{
					return DropResult.CHEST;
				}
			}

			if (colliders[i].gameObject != gameObject && colliders[i].gameObject.layer != LayerMask.NameToLayer("PlayerZone") && colliders[i].gameObject != GameManager.Instance.GetPlayer(PlayerIndex).gameObject)
			{
				Debug.Log("Collider : " + colliders[i].gameObject.name);
				++otherCollider;
			}
		}

		if (otherCollider == 0)
		{
			return DropResult.NONE;
		}

		return DropResult.ON_COLLIDER;
	}

    public void Taken(int playerIndex)
    {
        state = State.CARRIED;

        PlayerIndex = playerIndex;
        IsInChest = false;

        GameManager.Instance.GetPlayer(PlayerIndex).toyHasTaken = GetComponent<Toy>();

        if (objectType == ObjectType.BIG)
            GetComponent<Collider2D>().enabled = false;
    }

    public void Drop()
    {
        state = State.DOWN;

        if (objectType == ObjectType.BIG)
			GetComponent<Collider2D>().enabled = true;
	}

	public void Throw(Vector3 direction, int playerIndex)
	{
		state = State.THROWN;

		GetComponent<Collider2D>().enabled = true;

		throwStartPos = transform.position;
		throwDirection = direction;
		throwingPlayerIndex = playerIndex;
	}

}
