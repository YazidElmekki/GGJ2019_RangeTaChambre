using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toy : MonoBehaviour {

    [SerializeField]
    ObjectType objectType;

    bool FirstValidDrop = true;
    float WalkSpeed;
    int PlayerIndex;
    int Points;

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
        //GameManager.Instance.ScoreChanged += ChangeScore;

        state = State.DOWN;

        Debug.Log(state);
        Debug.Log(objectType);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Taken()
    {
        state = State.CARRIED;
    }

    void Drop()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if ((collision.gameObject.name == "ZonePlayerTwo") && (IsFristTimeIsDown))
        //    ChangeScore(Points, Points);
        //      IsFirstTimeIsDown = false;
    }

    void ChangeScore(int PointsPlayerOne, int PointsPlayerTwo)
    {

    }
}
