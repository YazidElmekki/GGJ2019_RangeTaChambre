using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IPlayerZoneTracker
{
	[SerializeField, Range(0f, 1f)]
	private int playerIndex = 0;
    [SerializeField]
    private Chest chest;
	public Chest AssignedChest { get { return chest; } }
    [SerializeField]
    float sizeToPickUpObject = 5;
    [SerializeField]
    GameObject bed;

	[SerializeField]
	private float distanceToHide = 2.0f;

    public bool isHidden = false;

    public Toy toyHasTaken;

    public int PlayerIndex { get { return playerIndex; } }

	public int CurrentZoneIndex { get; set; }

	BoxCollider2D boxCollider;
	[SerializeField]
	private float takenObjectAdditionalDistance = 0.25f;

	private PlayerMovement playerMovement;

	Animator playerAnimator;

	private void Start()
	{
		boxCollider = GetComponent<BoxCollider2D>();
		playerMovement = GetComponent<PlayerMovement>();
		playerAnimator = GetComponent<Animator>();
	}

	private void SetDefaultObjectPosition()
	{
		Vector3 defaultDir = Vector3.down;
		defaultDir *= Mathf.Max(boxCollider.size.x, boxCollider.size.y) + takenObjectAdditionalDistance;

		toyHasTaken.transform.position = transform.position + defaultDir;

		if (toyHasTaken.CurrentObjectType == Toy.ObjectType.BIG)
		{
			playerAnimator.SetBool("IsBigObject", true);
		}
		else
		{
			playerAnimator.SetBool("IsBigObject", false);
		}
	}

	void Update ()
	{
		if (playerMovement.CurrentState == PlayerMovement.MoveState.IDLE || playerMovement.CurrentState == PlayerMovement.MoveState.MOVING)
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
					Toy.DropResult dropResult = toyHasTaken.CanDrop();

					if (dropResult == Toy.DropResult.NONE)
					{
						playerAnimator.SetBool("IsBigObject", false);

						if (toyHasTaken.CurrentObjectType == Toy.ObjectType.SMALL)
						{
							Vector3 throwDir = toyHasTaken.transform.position - transform.position;
							throwDir.Normalize();

							toyHasTaken.Throw(throwDir, PlayerIndex);
						}
						else
							toyHasTaken.Drop();

						if (toyHasTaken.PlayerIndex == PlayerIndex && toyHasTaken.FirstValidDrop)
						{
							Collider2D[] overlapColliders = Physics2D.OverlapPointAll(toyHasTaken.transform.position);

							bool canScore = false;

							for (int i = 0; i < overlapColliders.Length; ++i)
							{
								if (overlapColliders[i].gameObject.layer == LayerMask.NameToLayer("PlayerZone"))
								{
									if ((playerIndex == 0 && overlapColliders[i].gameObject.name == "Player1Zone" && playerIndex == toyHasTaken.originalPlayerIndex) || (playerIndex == 1 && overlapColliders[i].gameObject.name == "Player2Zone" && playerIndex == toyHasTaken.originalPlayerIndex))
									{
										canScore = true;
										break;
									}
								}
							}


							if (canScore == true)
							{
								toyHasTaken.FirstValidDrop = false;
								GameManager.Instance.PlayerScored(playerIndex, toyHasTaken.Points);
							}
						}

						toyHasTaken = null;
					}
					else if (dropResult == Toy.DropResult.CHEST)
					{
						GameManager.Instance.PlayerScored(playerIndex, toyHasTaken.Points);
						Destroy(toyHasTaken.gameObject);
						toyHasTaken = null;
						return;
					}
				}
				else
				{
					if (Vector3.Distance(transform.position, bed.transform.position) < distanceToHide)
					{
						bed.GetComponent<Collider2D>().enabled = false;
						transform.position = bed.transform.position;
						isHidden = true;

						playerMovement.Hide(bed, bed.GetComponent<BedWakeUpZone>().WakeUpZone);
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
						if (objectToPickUp != null)// && objectToPickUp.FirstValidDrop == false)
						{
							if (gameObject == GameManager.Instance.PlayerOne.gameObject && GameManager.Instance.PlayerTwo.toyHasTaken == objectToPickUp)
							{
								GameManager.Instance.PlayerTwo.toyHasTaken = null;
							}
							else if (gameObject == GameManager.Instance.PlayerTwo.gameObject && GameManager.Instance.PlayerOne.toyHasTaken == objectToPickUp)
							{
								GameManager.Instance.PlayerOne.toyHasTaken = null;
							}

							objectToPickUp.Taken(playerIndex);
							SetDefaultObjectPosition();
						}
					}
				}
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
