using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    [SerializeField]
    GameObject PrefabBigObject, PrefabMediumObject, PrefabSmallObject;
    [SerializeField]
    int playerIndex;

	[SerializeField]
	private Sprite closedSprite;

	[SerializeField]
	private Sprite openSprite;

	[Header("SD Audio clips")]
	[SerializeField]
	private AudioClip openChestAudioClip;

	[SerializeField]
	private AudioClip closedChestAudioClip;

	private AudioSource chestAudioSource;

	private SpriteRenderer spriteRenderer;

    Toy[] Toys;

    GameObject XObjectPos;
    GameObject YObjectPos;
    GameObject BObjectPos;

    int random;

    // Use this for initialization
    void Start ()
    {
		chestAudioSource = GetComponent<AudioSource>();

		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = closedSprite;

        Toys = new Toy[3];

        for (int i = 0; i < 3; ++i)
            GenerateNewObject(i);
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    public void TakeObject(int enumObject, int playerIndex)
    {
        if ((playerIndex != this.playerIndex) || (GameManager.Instance.GetPlayer(playerIndex).HasObject) || ((GameManager.Instance.GetPlayer(playerIndex).transform.position - transform.position).sqrMagnitude > 10))
            return;

        GameManager.Instance.GetPlayer(playerIndex).HasObject = true;

        Toys[enumObject - 1].Taken();

        GameManager.Instance.GetPlayer(playerIndex).toyHasTaken = Toys[enumObject - 1];

        //Toys[enumObject - 1].GetComponent<Renderer>().enabled = true;

        GenerateNewObject(enumObject - 1); // Doit rajouter les objets dans le coffre apres en avoir pris un, a playtester !!
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
        else if (random >= 75)
        {
            Toys[index] = Instantiate(PrefabBigObject, transform.position, Quaternion.identity).GetComponent<Toy>();
        }

        //Toys[index].GetComponent<Renderer>().enabled = false;

        /*
         * Index :
         * 0 = X
         * 1 = Y
         * 2 = B
         */

        Toys[index].PlayerIndex = playerIndex;
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			Player player = collision.gameObject.GetComponent<Player>();

			if (player.AssignedChest == this)
			{
				spriteRenderer.sprite = openSprite;
				chestAudioSource.PlayOneShot(openChestAudioClip);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			Player player = collision.gameObject.GetComponent<Player>();

			if (player.AssignedChest == this)
			{
				spriteRenderer.sprite = closedSprite;
				chestAudioSource.PlayOneShot(closedChestAudioClip);
			}
		}
	}
}
