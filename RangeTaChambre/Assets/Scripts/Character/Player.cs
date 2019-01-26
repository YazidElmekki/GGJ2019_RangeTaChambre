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

	public int PlayerIndex { get { return playerIndex; } }

    public bool HasObject = false;

	public int CurrentZoneIndex { get; set; }

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
				toyHasTaken.Drop();

				if (toyHasTaken.PlayerIndex == PlayerIndex && toyHasTaken.FirstValidDrop)
				{
					toyHasTaken.FirstValidDrop = false;
					GameManager.Instance.PlayerScored(playerIndex, toyHasTaken.Points);
				}

				toyHasTaken = null;
				HasObject = false;
            }
		}
	}
}
