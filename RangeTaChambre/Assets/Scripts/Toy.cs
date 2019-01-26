using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toy : MonoBehaviour
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
	}

	[SerializeField]
    private ObjectType objectType;
	public ObjectType CurrentObjectType { get { return objectType; } }

    [SerializeField]
    public float WalkSlowdown;

    public bool FirstValidDrop = true;
    public int PlayerIndex { get; set; }
    public int Points;

    public bool IsInChest = true;

    State state;

	[SerializeField]
	private Sprite toySprite;
	public Sprite ToySprite { get { return toySprite; } }


	// Use this for initialization
	void Start()
    {
        state = State.DOWN;

        GetComponent<Collider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (state == State.CARRIED)
        //    transform.position = GameManager.Instance.GetPlayer(PlayerIndex).transform.position;
    }

	public bool CanDrop()
	{
		Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, GetComponent<BoxCollider2D>().size, 0.0f);

		int otherCollider = 0;
		
		for (int i = 0; i < colliders.Length; ++i)
		{
			if (colliders[i].gameObject != gameObject && colliders[i].gameObject.layer != LayerMask.NameToLayer("PlayerZone"))
			{
				++otherCollider;
			}
		}

		if (otherCollider == 0)
		{
			return true;
		}

		return false;
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

        //if (PlayerIndex != )

		if (objectType == ObjectType.BIG)
			GetComponent<Collider2D>().enabled = true;
    }

}
