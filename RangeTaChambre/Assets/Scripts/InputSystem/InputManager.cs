using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
	[SerializeField]
	private bool m_useXInput = false;

	[SerializeField]
	private InputSettings m_inputSettings = null;
	static private InputManager m_instance = null;
	static public InputManager Instance
	{
		get
		{
			if (m_instance != null)
			{
				return m_instance;
			}
			else
			{
				m_instance = FindObjectOfType<InputManager>();

				if (m_instance == null)
				{
					Debug.Log("No instance of Input manager in the scene");
				}

				return m_instance;
			}
		}
	}

	private bool m_isPaused = false;
	public bool IsPaused { get { return m_isPaused; } }

	private bool m_canPause = true;
	public bool CanPause { get { return m_canPause; } set { m_canPause = value; } }

	[HideInInspector]
	public bool IsListeningToInput = true;

	private X_360_Gamepad[] m_X360_Gamepads = new X_360_Gamepad[] { new X_360_Gamepad(1), new X_360_Gamepad(2) };


	public void Update()
	{
		for (int i = 0; i < m_X360_Gamepads.Length; ++i)
		{
			m_X360_Gamepads[i].Update();
		}

		if (IsPauseButtonDown() == true && IsListeningToInput == true)
		{
			if (m_isPaused == false)
			{
				Pause();
			}
			else
			{
				StartCoroutine(OnUnpause());
				//Unpause();
			}
		}
	}

	public void LateUpdate()
	{
		for (int i = 0; i < m_X360_Gamepads.Length; ++i)
		{
			m_X360_Gamepads[i].Refresh();
		}
	}

	public IEnumerator OnUnpause()
	{
		yield return new WaitForEndOfFrame();

		Unpause();
	}

	public void Pause(bool showPauseMenu = true)
	{
		if (m_canPause)
		{
			m_isPaused = true;

			Time.timeScale = 0.0f;
		}
	}

	public void Unpause()
	{
		m_isPaused = false;

		Time.timeScale = 1.0f;
	}

	private void OnDestroy()
	{
		Time.timeScale = 1.0f;

		for (int i = 0; i < m_X360_Gamepads.Length; ++i)
		{
			m_X360_Gamepads[i].DesactivateVibration();
		}
	}

	public bool IsPauseButtonDown()
	{
		string buttonName = string.Empty;

		for (int i = 0; i < m_inputSettings.PlayerInputSetting.Length; ++i)
		{
			bool isUsingXInputAndConnected = IsUsingXInputAndGamepadConnected(i);

			buttonName = m_inputSettings.PlayerInputSetting[i].GetButtonActionButtonName(ButtonActionEnum.PAUSE, isUsingXInputAndConnected);

			if (buttonName.Length > 0)
			{
				if (isUsingXInputAndConnected == false)
				{
					if (Input.GetButtonDown(buttonName) == true)
					{
						return true;
					}
				}
				else
				{
					if (m_X360_Gamepads[i].GetButtonDown(buttonName) == true)
					{
						return true;
					}
				}
			}
			else
			{
				PrintButtonErrorMessage();
			}
		}

		return false;
	}

	public bool IsUsingXInputAndGamepadConnected(int playerIndex)
	{
		return m_useXInput && m_X360_Gamepads[playerIndex].IsConnected;
	}

	public bool GetButtonActionDown(ButtonActionEnum action, int playerIndex)
	{
		if (m_isPaused == false && IsListeningToInput == true && playerIndex < m_inputSettings.PlayerInputSetting.Length)
		{
			string buttonName = m_inputSettings.PlayerInputSetting[playerIndex].GetButtonActionButtonName(action, IsUsingXInputAndGamepadConnected(playerIndex));

			if (buttonName.Length > 0)
			{
				return IsUsingXInputAndGamepadConnected(playerIndex) ? m_X360_Gamepads[playerIndex].GetButtonDown(buttonName) : Input.GetButtonDown(buttonName);
			}
			else
			{
				PrintButtonErrorMessage();
			}
		}

		return false;
	}

	public bool GetButtonActionUp(ButtonActionEnum action, int playerIndex)
	{
		if (m_isPaused == false && IsListeningToInput == true && playerIndex < m_inputSettings.PlayerInputSetting.Length)
		{
			string buttonName = m_inputSettings.PlayerInputSetting[playerIndex].GetButtonActionButtonName(action, IsUsingXInputAndGamepadConnected(playerIndex));

			if (buttonName.Length > 0)
			{
				return IsUsingXInputAndGamepadConnected(playerIndex) ? m_X360_Gamepads[playerIndex].GetButtonUp(buttonName) : Input.GetButtonUp(buttonName);
			}
			else
			{
				PrintButtonErrorMessage();
			}
		}

		return false;
	}

	public bool GetButtonAction(ButtonActionEnum action, int playerIndex)
	{
		if (m_isPaused == false && IsListeningToInput == true && playerIndex < m_inputSettings.PlayerInputSetting.Length)
		{
			string buttonName = m_inputSettings.PlayerInputSetting[playerIndex].GetButtonActionButtonName(action, IsUsingXInputAndGamepadConnected(playerIndex));

			if (buttonName.Length > 0)
			{
				return IsUsingXInputAndGamepadConnected(playerIndex) ? m_X360_Gamepads[playerIndex].GetButton(buttonName) : Input.GetButton(buttonName);
			}
			else
			{
				PrintButtonErrorMessage();
			}
		}

		return false;
	}

	public float GetAxisAction(AxisActionEnum action, int playerIndex)
	{
		if (m_isPaused == false && IsListeningToInput == true && playerIndex < m_inputSettings.PlayerInputSetting.Length)
		{
			if (IsUsingXInputAndGamepadConnected(playerIndex) == true)
			{
				switch (action)
				{
					case AxisActionEnum.MOVE_RIGHT:
						return m_X360_Gamepads[playerIndex].GetLeftStickValues().X;
					case AxisActionEnum.MOVE_UP:
						return m_X360_Gamepads[playerIndex].GetLeftStickValues().Y;
					default:
						PrintAxisErrorMessage();
						break;
				}
			}
			else
			{
				string axisName = m_inputSettings.PlayerInputSetting[playerIndex].GetAxisActionAxisName(action);

				if (axisName.Length > 0)
				{
					return Input.GetAxis(axisName);

				}
				else
				{
					PrintAxisErrorMessage();
				}
			}
		}

		return 0.0f;
	}

	public void ActivateGamepadVibration(int playerIndex, float vibrationTimer, Vector2 power, float fadeTime)
	{
		if (playerIndex < m_X360_Gamepads.Length)
		{
			m_X360_Gamepads[playerIndex].AddRumble(vibrationTimer, power, fadeTime);
		}
	}

	private void PrintButtonErrorMessage()
	{
		Debug.Log("The requested action have not been binded to a correct button in the input settings or the action have not been assigned in the PlayerInputSetting's array");
	}

	private void PrintAxisErrorMessage()
	{
		Debug.Log("The requested action have not been binded to a correct axis in the input setting or the action have not been assigned int PlayerInputSetting's array");
	}
}