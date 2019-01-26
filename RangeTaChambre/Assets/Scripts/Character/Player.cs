using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField, Range(0f, 1f)]
	private int playerIndex = 0;
    [SerializeField]
    private Chest chest;
	public Chest AssignedChest { get { return chest; } }
    [SerializeField]
    float sizeToPickUpObject = 5;

    public Toy toyHasTaken;

    public int PlayerIndex { get { return playerIndex; } }

	public int CurrentZoneIndex { get; set; }

	BoxCollider2D boxCollider;
	[SerializeField]
	private float takenObjectAdditionalDistance = 0.25f;

	private PlayerMovement playerMovement;

	private void Start()
	{
		boxCollider = GetComponent<BoxCollider2D>();
		playerMovement = GetComponent<PlayerMovement>();
	}

	private void SetDefaultObjectPosition()
	{
		Vector3 defaultDir = Vector3.down;
		defaultDir *= Mathf.Max(boxCollider.size.x, boxCollider.size.y) + takenObjectAdditionalDistance;

		toyHasTaken.transform.position = transform.position + defaultDir;
	}

	void Update ()
	{
		if (InputManager.Instance.GetButtonActionDown(ButtonActionEnum.TAKE_X_OBJECT, playerIndex) == true)
		{
            if (chest.TakeObject(1, playerIndex))
				SetDefaultObjectPosition();
		}
		if (InputManager.Instance.GetButtonActionDown(ButtonActionEnum.TAKE_Y_OBJECT, playerIndex) == true)
		{
			if (chest.TakeObject(2, playerIndex))
				SetDefaultObjectPosition();
		}
		if (InputManager.Instance.GetButtonActionDown(ButtonActionEnum.TAKE_B_OBJECT, playerIndex) == true)
		{
            if (chest.TakeObject(3, playerIndex))
				SetDefaultObjectPosition();
		}
		if (InputManager.Instance.GetButtonActionDown(ButtonActionEnum.TRHOW_OBJECT, playerIndex) == true)
		{
            if (toyHasTaken != null)
            {
				if (toyHasTaken.CanDrop())
				{
					toyHasTaken.Drop();

					if (toyHasTaken.PlayerIndex == PlayerIndex && toyHasTaken.FirstValidDrop)
					{
						toyHasTaken.FirstValidDrop = false;
						GameManager.Instance.PlayerScored(playerIndex, toyHasTaken.Points);
					}

					toyHasTaken = null;
				}
			}
            else
            {
                GameObject[] objects;

                objects = GameObject.FindGameObjectsWithTag("Toy");
                Toy objectToPickUp = null;
                Vector3 newDistance = new Vector3(10, 10, 10);

                foreach (GameObject Object in objects)
                {
                    Vector3 distance;

                    distance = Object.transform.position - transform.position;

                    if (distance.magnitude < newDistance.magnitude && distance.magnitude < sizeToPickUpObject)
                    {
                        newDistance = distance;

                        if (!Object.gameObject.GetComponent<Toy>().IsInChest)
                            objectToPickUp = Object.gameObject.GetComponent<Toy>();
                    }
                }

                if (objectToPickUp != null)
                    objectToPickUp.Taken(playerIndex);
            }
        }


		UpdateObjectPosition();
	}

	private void UpdateObjectPosition()
	{
		if (toyHasTaken != null)
		{
			if (playerMovement.Velocity.x != 0.0f || playerMovement.Velocity.y != 0.0f)
			{
				Vector3 defaultDir = playerMovement.Velocity.normalized * (Mathf.Max(boxCollider.size.x, boxCollider.size.y) + takenObjectAdditionalDistance);
				toyHasTaken.transform.position = transform.position + defaultDir;
			}
		}
	}
}
