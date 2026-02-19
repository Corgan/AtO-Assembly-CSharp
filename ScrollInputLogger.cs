using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollInputLogger : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private bool isHovered;

	private void Update()
	{
		if (isHovered)
		{
			float axis = Input.GetAxis("Mouse ScrollWheel");
			if (axis > 0f)
			{
				CombatToolManager.Instance.MovePage(-1);
			}
			else if (axis < 0f)
			{
				CombatToolManager.Instance.MovePage(1);
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		isHovered = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		isHovered = false;
	}
}
