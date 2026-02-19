using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour
{
	public TMP_Text buttonText;

	private bool IsMouseOverThis()
	{
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
		pointerEventData.position = Input.mousePosition;
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData, list);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].gameObject.name == base.gameObject.name)
			{
				return true;
			}
		}
		return false;
	}

	public void HoverOn()
	{
		GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonHover, 0.1f);
		buttonText.margin = new Vector4(6f, 0f, 0f, 0f);
		buttonText.color = new Color(1f, 0.6f, 0f);
		GameManager.Instance.SetCursorHover();
	}

	public void HoverOff()
	{
		buttonText.margin = new Vector4(0f, 0f, 0f, 0f);
		buttonText.color = Color.white;
		GameManager.Instance.SetCursorPlain();
	}
}
