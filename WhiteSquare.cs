using UnityEngine;

public class WhiteSquare : MonoBehaviour
{
	public bool isControllerAttached;

	public GameObject square;

	private void Update()
	{
		if (isControllerAttached)
		{
			if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
			{
				if (!square.activeSelf)
				{
					square.SetActive(value: true);
				}
			}
			else if (square.activeSelf)
			{
				square.SetActive(value: false);
			}
		}
		else if (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f)
		{
			if (square.activeSelf)
			{
				square.SetActive(value: false);
			}
		}
		else if (!square.activeSelf)
		{
			square.SetActive(value: true);
		}
	}
}
