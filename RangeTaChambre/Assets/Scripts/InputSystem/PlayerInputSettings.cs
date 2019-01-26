using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInputSettings
{
	[SerializeField]
	private ButtonAction[] m_buttonActions;

	[SerializeField]
	public AxisAction[] m_axisActions;

	public string GetButtonActionButtonName(ButtonActionEnum action, bool useXInputName = false)
	{
		for (int i = 0; i < m_buttonActions.Length; ++i)
		{
			if (m_buttonActions[i].Action == action)
			{
				return useXInputName == true ? m_buttonActions[i].XInputButtonName : m_buttonActions[i].ButtonName;
			}
		}

		return null;
	}

	public string GetAxisActionAxisName(AxisActionEnum action, bool useXInputName = false)
	{
		for (int i = 0; i < m_axisActions.Length; ++i)
		{
			if (m_axisActions[i].Action == action)
			{
				return m_axisActions[i].AxisName;
			}
		}

		return null;
	}
}