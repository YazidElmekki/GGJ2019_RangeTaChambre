using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	private int playerIndex = 0;
    [SerializeField]
    Chest chest;

    public Toy toyHasTaken;

	public int PlayerIndex { get { return playerIndex; } }

    public bool HasObject = false;

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
		}
	}
}
