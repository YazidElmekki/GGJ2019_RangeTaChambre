using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	private int playerIndex = 0;
    [SerializeField]
    Chest chest;

	public int PlayerIndex { get { return playerIndex; } }

    bool HasObject = false;

	void Update ()
	{
		if (InputManager.Instance.GetButtonActionDown(ButtonActionEnum.TAKE_X_OBJECT, playerIndex) == true)
		{
            chest.TakeObject(1);
			Debug.Log("Take X Object " + playerIndex);
		}
		if (InputManager.Instance.GetButtonActionDown(ButtonActionEnum.TAKE_Y_OBJECT, playerIndex) == true)
		{
            chest.TakeObject(2);
            Debug.Log("Take Y object " + playerIndex);
		}
		if (InputManager.Instance.GetButtonActionDown(ButtonActionEnum.TAKE_B_OBJECT, playerIndex) == true)
		{
            chest.TakeObject(3);
            Debug.Log("Take B object " + playerIndex);
		}
		if (InputManager.Instance.GetButtonActionDown(ButtonActionEnum.TRHOW_OBJECT, playerIndex) == true)
		{
			Debug.Log("Throw object " + playerIndex);
		}
	}
}
