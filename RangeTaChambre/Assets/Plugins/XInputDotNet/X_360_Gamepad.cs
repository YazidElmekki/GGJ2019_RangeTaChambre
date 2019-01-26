using UnityEngine;
using XInputDotNetPure;
using System.Collections;
using System.Collections.Generic;

public struct XButton
{
	public ButtonState PrevState;
	public ButtonState State;
}

public struct TriggerState
{
	public float PrevValue;
	public float CurrentValue;
}

class XRumble
{
	public float Timer;
	public float FadeTime;
	public Vector2 Power;
	
	public void Update()
	{
		Timer -= Time.unscaledDeltaTime;
	}
}

public class X_360_Gamepad
{
	private GamePadState m_previousState;
	private GamePadState m_currentState;

	private int m_gamepadIndex;
	public int Index { get { return m_gamepadIndex; } }

	private PlayerIndex m_playerIndex;
	private List<XRumble> m_rumbleEvents;

	private Dictionary<string, XButton> m_inputMap;

	private XButton A, B, X, Y;
	private XButton DPadUp, DPadDown, DPadLeft, DPadRight;

	private XButton Guide;
	private XButton Back, Start;
	private XButton L3, R3;
	private XButton LB, RB;
	private TriggerState LT, RT;

	public bool IsConnected { get { return m_currentState.IsConnected; } }

	public X_360_Gamepad(int index)
	{
		m_gamepadIndex = index - 1;
		m_playerIndex = (PlayerIndex)m_gamepadIndex;

		m_rumbleEvents = new List<XRumble>();
		m_inputMap = new Dictionary<string, XButton>();
	}

	public void DesactivateVibration()
	{
		GamePad.SetVibration(m_playerIndex, 0.0f, 0.0f);
	}

	public void Update()
	{
		m_currentState = GamePad.GetState(m_playerIndex);

		if (m_currentState.IsConnected == true)
		{
			A.State = m_currentState.Buttons.A;
			B.State = m_currentState.Buttons.B;
			X.State = m_currentState.Buttons.X;
			Y.State = m_currentState.Buttons.Y;

			DPadUp.State = m_currentState.DPad.Up;
			DPadDown.State = m_currentState.DPad.Down;
			DPadLeft.State = m_currentState.DPad.Left;
			DPadRight.State = m_currentState.DPad.Right;

			Guide.State = m_currentState.Buttons.Guide;
			Back.State = m_currentState.Buttons.Back;
			Start.State = m_currentState.Buttons.Start;

			L3.State = m_currentState.Buttons.LeftStick;
			R3.State = m_currentState.Buttons.RightStick;

			LB.State = m_currentState.Buttons.LeftShoulder;
			RB.State = m_currentState.Buttons.RightShoulder;

			LT.CurrentValue = m_currentState.Triggers.Left;
			RT.CurrentValue = m_currentState.Triggers.Right;

			UpdateInputMap();
		}
	}

	public void Refresh()
	{
		m_previousState = m_currentState;

		if (m_currentState.IsConnected == true)
		{
			A.PrevState = m_previousState.Buttons.A;
			B.PrevState = m_previousState.Buttons.B;
			X.PrevState = m_previousState.Buttons.X;
			Y.PrevState = m_previousState.Buttons.Y;

			DPadUp.PrevState = m_previousState.DPad.Up;
			DPadDown.PrevState = m_previousState.DPad.Down;
			DPadLeft.PrevState = m_previousState.DPad.Left;
			DPadRight.PrevState = m_previousState.DPad.Right;

			Guide.PrevState = m_previousState.Buttons.Guide;
			Back.PrevState = m_previousState.Buttons.Back;
			Start.PrevState = m_previousState.Buttons.Start;

			L3.PrevState = m_previousState.Buttons.LeftStick;
			R3.PrevState = m_previousState.Buttons.RightStick;

			LB.State = m_currentState.Buttons.LeftShoulder;
			RB.State = m_currentState.Buttons.RightShoulder;

			LT.PrevValue = m_previousState.Triggers.Left;
			RT.PrevValue = m_currentState.Triggers.Right;

			UpdateInputMap();
			HandleRumble();
		}
	}

	void UpdateInputMap()
	{
		m_inputMap["A"] = A;
		m_inputMap["B"] = B;
		m_inputMap["X"] = X;
		m_inputMap["Y"] = Y;

		m_inputMap["DPadUp"] = DPadUp;
		m_inputMap["DPadDown"] = DPadDown;
		m_inputMap["DPadLeft"] = DPadLeft;
		m_inputMap["DPadRight"] = DPadRight;

		m_inputMap["Guide"] = Guide;
		m_inputMap["Back"] = Back;
		m_inputMap["Start"] = Start;

		m_inputMap["L3"] = L3;
		m_inputMap["R3"] = R3;

		m_inputMap["LB"] = LB;
		m_inputMap["RB"] = RB;
	}

	public bool GetButton(string button)
	{
		return m_inputMap[button].State == ButtonState.Pressed ? true : false;
	}

	public bool GetButtonDown(string button)
	{
		return IsConnected == true ? (m_inputMap[button].State == ButtonState.Pressed && m_inputMap[button].PrevState == ButtonState.Released) ? true : false : false;
	}

	public bool GetButtonUp(string button)
	{
		return IsConnected == true ? (m_inputMap[button].State == ButtonState.Released && m_inputMap[button].PrevState == ButtonState.Pressed) ? true : false : false;
	}

	public void AddRumble(float timer, Vector2 power, float fadeTime)
	{
		XRumble rumble = new XRumble();

		rumble.Timer = timer;
		rumble.Power = power;
		rumble.FadeTime = fadeTime;

		m_rumbleEvents.Add(rumble);
	}

	private void HandleRumble()
	{
		if (m_rumbleEvents.Count > 0)
		{
			Vector2 currentPower = Vector2.zero;

			for (int i = 0; i < m_rumbleEvents.Count; ++i)
			{
				if (m_rumbleEvents[i].Timer > 0.0f)
				{
					float timeLeft = Mathf.Clamp(m_rumbleEvents[i].Timer / m_rumbleEvents[i].FadeTime, 0.0f, 1.0f);

					currentPower = new Vector2(
						Mathf.Max(m_rumbleEvents[i].Power.x * timeLeft, currentPower.x),
						Mathf.Max(m_rumbleEvents[i].Power.y * timeLeft, currentPower.y));

					GamePad.SetVibration(m_playerIndex, currentPower.x, currentPower.y);
					m_rumbleEvents[i].Update();
				}
				else
				{
					GamePad.SetVibration(m_playerIndex, 0.0f, 0.0f);
					m_rumbleEvents.Remove(m_rumbleEvents[i]);
				}
			}
		}
	}

	public GamePadThumbSticks.StickValue GetLeftStickValues()
	{
		return m_currentState.ThumbSticks.Left;
	}

	public GamePadThumbSticks.StickValue GetRightStickValues()
	{
		return m_currentState.ThumbSticks.Right;
	}

	public float GetLeftTriggerValue()
	{
		return m_currentState.Triggers.Left;
	}

	public float GetRightTriggerValue()
	{
		return m_currentState.Triggers.Right;
	}

	public bool GetLeftTriggerTap()
	{
		return (LT.PrevValue == 0.0f && LT.CurrentValue >= 0.1f) ? true : false;
	}

	public bool GetRightTriggerTap()
	{
		return (RT.PrevValue == 0.0f && RT.CurrentValue >= 0.1f) ? true : false;
	}


}