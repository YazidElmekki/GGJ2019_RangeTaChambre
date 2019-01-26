using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toy : MonoBehaviour {

    [SerializeField]
    ObjectType objectType;
    [SerializeField]
    public float WalkSlowdown;

    public bool FirstValidDrop = true;
    public int PlayerIndex;
    public int Points;

    enum ObjectType
    {
        MEDIUM = 0,
        SMALL,
        BIG,
    }

    enum State
    {
        CARRIED = 0,
        DOWN,
    }

    State state;

    // Use this for initialization
    void Start()
    {
        state = State.DOWN;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.CARRIED)
            transform.position = GameManager.Instance.GetPlayer(PlayerIndex).transform.position;
    }

    public void Taken()
    {
        state = State.CARRIED;
    }

    public void Drop()
    {
        state = State.DOWN;

		if (objectType == ObjectType.BIG)
		{
			if (PlayerIndex == 0)
				gameObject.layer = LayerMask.NameToLayer("BigToyPlayer1");
			else
				gameObject.layer = LayerMask.NameToLayer("BigToyPlayer1");
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		gameObject.layer = LayerMask.NameToLayer("BigToy");
	}
}
