using UnityEngine;

public class OnTouchDown : MonoBehaviour
{
	private void Update()
	{
		RaycastHit hitInfo = default(RaycastHit);
		for (int i = 0; i < Input.touchCount; i++)
		{
			if (Input.GetTouch(i).phase.Equals(TouchPhase.Began))
			{
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(i).position), out hitInfo))
				{
					hitInfo.transform.gameObject.SendMessage("OnMouseDown");
				}
			}
			else if (Input.GetTouch(i).phase.Equals(TouchPhase.Ended))
			{
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(i).position), out hitInfo))
				{
					hitInfo.transform.gameObject.SendMessage("OnMouseUp");
				}
			}
			else if (Input.GetTouch(i).phase.Equals(TouchPhase.Moved) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(i).position), out hitInfo))
			{
				hitInfo.transform.gameObject.SendMessage("OnMouseDrag");
			}
		}
	}
}
