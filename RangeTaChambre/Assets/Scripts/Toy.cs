﻿using System.Collections;
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

    State state;

    // Use this for initialization
    void Start()
    {
        state = State.DOWN;
    }

    // Update is called once per frame
    void Update()
    {
        //if (state == State.CARRIED)
        //    transform.position = GameManager.Instance.GetPlayer(PlayerIndex).transform.position;
    }

	public bool CanDrop()
	{
		Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position, LayerMask.GetMask("PlayerZone"));

		int otherCollider = 0;
		
		for (int i = 0; i < colliders.Length; ++i)
		{
			if (colliders[i].gameObject != gameObject)
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

    public void Taken()
    {
        state = State.CARRIED;

		////if (objectType == ObjectType.BIG)
		//{
		//	if (PlayerIndex == 0)
		//		gameObject.layer = LayerMask.NameToLayer("BigToyPlayer1");
		//	else
		//		gameObject.layer = LayerMask.NameToLayer("BigToyPlayer2");
		//}
	}

    public void Drop()
    {
        state = State.DOWN;

		if (objectType == ObjectType.BIG)
			GetComponent<Collider2D>().enabled = true;
	}
}
