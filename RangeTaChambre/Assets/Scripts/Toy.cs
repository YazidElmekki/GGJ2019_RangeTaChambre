using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toy : MonoBehaviour {

    [SerializeField]
    ObjectType objectType;
    [SerializeField]
    public float WalkSlowdown;

    bool FirstValidDrop = true;
    public int PlayerIndex;
    int Points;

    //public int playerIndex;

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

        GetComponent<Collider2D>().enabled = false;
    }

    void Drop()
    {
        state = State.DOWN;

        GetComponent<Collider2D>().enabled = true;
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
