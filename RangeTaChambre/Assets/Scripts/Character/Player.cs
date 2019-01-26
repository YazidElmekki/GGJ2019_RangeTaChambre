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

    public Toy toyHasTaken;
    Toy objectToPickUp;

    public int PlayerIndex { get { return playerIndex; } }

    public bool HasObject = false;

	public int CurrentZoneIndex { get; set; }

    Vector3 newDistance = new Vector3(10, 10, 10);

    void Update ()
	{
		if (InputManager.Instance.GetButtonActionDown(ButtonActionEnum.TAKE_X_OBJECT, playerIndex) == true)
		{
            chest.TakeObject(1, playerIndex);
		}
		if (InputManager.Instance.GetButtonActionDown(ButtonActionEnum.TAKE_Y_OBJECT, playerIndex) == true)
		{
            chest.TakeObject(2, playerIndex);
		}
		if (InputManager.Instance.GetButtonActionDown(ButtonActionEnum.TAKE_B_OBJECT, playerIndex) == true)
		{
            chest.TakeObject(3, playerIndex);
		}
		if (InputManager.Instance.GetButtonActionDown(ButtonActionEnum.TRHOW_OBJECT, playerIndex) == true)
		{
            if (HasObject)
            {
                //Call Matthieru function
            }

            else
            {
                Vector3 center = transform.position + transform.forward;
                Vector2 size = new Vector2(2.0f, 2.0f);
                Collider2D[] results = new Collider2D[10];
                ContactFilter2D contactFilter = new ContactFilter2D();
                contactFilter.layerMask = LayerMask.GetMask("BigToy");
                Physics2D.OverlapBox(center, size, 0, contactFilter, results);

                foreach (Collider2D collide in results)
                {
                    Vector3 distance;

                    distance = collide.gameObject.transform.position - transform.position;

                    if (distance.magnitude < newDistance.magnitude)
                    {
                        objectToPickUp = collide.gameObject.GetComponent<Toy>();
                    }
                }

                objectToPickUp.Taken();
            }
		}
	}
}
