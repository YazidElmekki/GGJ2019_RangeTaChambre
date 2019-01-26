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

				if (FirstValidDrop && throwingPlayerIndex != PlayerIndex && CurrentZoneIndex != PlayerIndex)
					GameManager.Instance.PlayerScored(throwingPlayerIndex, Points);
			}
		}
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (CurrentObjectType == ObjectType.SMALL)
		{
			if (collision.otherCollider.gameObject.tag == "Wall")
			{
				state = State.DOWN;

				if (FirstValidDrop && throwingPlayerIndex != PlayerIndex && CurrentZoneIndex != PlayerIndex)
					GameManager.Instance.PlayerScored(throwingPlayerIndex, Points);
			}

			else if (collision.otherCollider.gameObject.tag == "Player")
			{
				//TODO STUN OTHER PLAYER
				Debug.Log("STUN");

				Destroy(gameObject);
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
		Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, GetComponent<BoxCollider2D>().size, 0.0f);

		int otherCollider = 0;
		
		for (int i = 0; i < colliders.Length; ++i)
		{
			if ((colliders[i].gameObject.tag == "PlayerOneChest" && PlayerIndex == 0) || (colliders[i].gameObject.tag == "PlayerTwoChest" && PlayerIndex == 1))
				return DropResult.CHEST;

			if (colliders[i].gameObject != gameObject && colliders[i].gameObject.layer != LayerMask.NameToLayer("PlayerZone") && colliders[i].gameObject != GameManager.Instance.GetPlayer(PlayerIndex).gameObject && colliders[i].isTrigger == false)
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
    }

    public void Drop()
    {
        state = State.DOWN;

        GameObject[] chests;

        chests = GameObject.FindGameObjectsWithTag("Chest");

        if ((transform.position - chests[0].transform.position).magnitude <= distanceMinToTidyUp)
        {
            if (PlayerIndex != chests[0].GetComponent<Chest>().playerIndex && Origin != PlayerIndex)
            {
                Destroy(gameObject);
                return;
            }
        }
        
        else if ((transform.position - chests[1].transform.position).magnitude <= distanceMinToTidyUp)
        {
            if (PlayerIndex != chests[1].GetComponent<Chest>().playerIndex && Origin != PlayerIndex)
            {
                Destroy(gameObject);
                return;
            }
        }

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
