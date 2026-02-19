using UnityEngine;

public static class ControllerManager
{
	public static void ControllerUp()
	{
		DoControl(0f, 1f);
	}

	public static void ControllerDown()
	{
		DoControl(0f, -1f);
	}

	public static void ControllerLeft()
	{
		DoControl(-1f, 0f);
	}

	public static void ControllerRight()
	{
		DoControl(1f, 0f);
	}

	private static void DoControl(float _x, float _y)
	{
		Debug.Log("Do Control " + _x + "," + _y);
		_ = (bool)MainMenuManager.Instance;
	}
}
