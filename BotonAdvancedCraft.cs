using UnityEngine;

public class BotonAdvancedCraft : MonoBehaviour
{
	public Transform circleOn;

	public Transform circleOff;

	public void SetState(bool state)
	{
		if (state)
		{
			circleOn.gameObject.SetActive(value: true);
			circleOff.gameObject.SetActive(value: false);
		}
		else
		{
			circleOn.gameObject.SetActive(value: false);
			circleOff.gameObject.SetActive(value: true);
		}
	}
}
