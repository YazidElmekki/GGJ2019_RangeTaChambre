using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    [SerializeField]
    GameObject PrefabBigObject, PrefabMediumObject, PrefabSmallObject;

    Toy[] Toys;

    GameObject XObjectPos;
    GameObject YObjectPos;
    GameObject BObjectPos;

    int random;

    // Use this for initialization
    void Start ()
    {
        Toys = new Toy[3];

        for (int i = 0; i < 3; ++i)
            GenerateNewObject(i);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void TakeObject(int indexEnum)
    {
        //if (GameManager.Instance.GetPlayer(playerIndex).HasObject)
        //    return;

        if (indexEnum == 1)
        {
            //GameManager.Instance.GetPlayer(playerIndex).HasObject = true
        }
    }

    void GenerateNewObject(int index)
    {
        random = Random.Range(0, 99);

        if (random < 50)
        {
            Toys[index] = Instantiate(PrefabMediumObject, transform.position, Quaternion.identity).GetComponent<Toy>();
        }
        else if (random >= 50 && random < 75)
        {
            Toys[index] = Instantiate(PrefabSmallObject, transform.position, Quaternion.identity).GetComponent<Toy>();
        }
        else
        {
            Toys[index] = Instantiate(PrefabBigObject, transform.position, Quaternion.identity).GetComponent<Toy>();
        }
    }
}
