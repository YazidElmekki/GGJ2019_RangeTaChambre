
public enum AxisActionEnum
{
	MOVE_RIGHT,
	MOVE_UP,
}

[System.Serializable]
public struct AxisAction
{
	public AxisActionEnum Action;
	public string AxisName;
	public string XInputAxisName;
}
