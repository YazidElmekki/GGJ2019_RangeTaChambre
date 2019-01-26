public enum ButtonActionEnum
{
	PAUSE,
	TAKE_X_OBJECT,
	TAKE_Y_OBJECT,
	TAKE_B_OBJECT,
	TRHOW_OBJECT
}

[System.Serializable]
public struct ButtonAction
{
	public ButtonActionEnum Action;
	public string ButtonName;
	public string XInputButtonName;
}