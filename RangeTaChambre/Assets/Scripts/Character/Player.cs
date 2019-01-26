using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	private int playerIndex = 0;

	public int PlayerIndex { get { return playerIndex; } }

	void Update ()
	{
		if (InputManager.Instance.GetButtonActionDown(ButtonActionEnum.TAKE_X_OBJECT, playerIndex) == true)
		{
			Debug.Log("Take X Object " + playerIndex);
		}
		if (InputManager.Instance.GetButtonActionDown(ButtonActionEnum.TAKE_Y_OBJECT, playerIndex) == true)
		{
			Debug.Log("Take Y object " + playerIndex);
		}
		if (InputManager.Instance.GetButtonActionDown(ButtonActionEnum.TAKE_B_OBJECT, playerIndex) == true)
		{
			Debug.Log("Take B object " + playerIndex);
		}
		if (InputManager.Instance.GetButtonActionDown(ButtonActionEnum.TRHOW_OBJECT, playerIndex) == true)
		{
			Debug.Log("Throw object " + playerIndex);
		}
	}
}
