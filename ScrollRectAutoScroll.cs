using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectAutoScroll : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public float scrollSpeed = 10f;

	private bool mouseOver;

	private List<Selectable> m_Selectables = new List<Selectable>();

	private ScrollRect m_ScrollRect;

	private Vector2 m_NextScrollPosition = Vector2.up;

	private void OnEnable()
	{
		if ((bool)m_ScrollRect)
		{
			m_ScrollRect.content.GetComponentsInChildren(m_Selectables);
		}
	}

	private void Awake()
	{
		m_ScrollRect = GetComponent<ScrollRect>();
	}

	private void Start()
	{
		if ((bool)m_ScrollRect)
		{
			m_ScrollRect.content.GetComponentsInChildren(m_Selectables);
		}
		ScrollToSelected(quickScroll: true);
	}

	private void Update()
	{
		if (SystemInfo.deviceType != DeviceType.Handheld || Gamepad.all.Count > 1)
		{
			InputScroll();
			if (!mouseOver)
			{
				m_ScrollRect.normalizedPosition = Vector2.Lerp(m_ScrollRect.normalizedPosition, m_NextScrollPosition, scrollSpeed * Time.unscaledDeltaTime);
			}
			else
			{
				m_NextScrollPosition = m_ScrollRect.normalizedPosition;
			}
		}
	}

	private void InputScroll()
	{
		if (m_Selectables.Count > 0)
		{
			Keyboard current = Keyboard.current;
			Gamepad current2 = Gamepad.current;
			if (current != null && (Keyboard.current.upArrowKey.isPressed || Keyboard.current.downArrowKey.isPressed))
			{
				ScrollToSelected(quickScroll: false);
			}
			if (current2 != null && (Gamepad.current.dpad.up.isPressed || Gamepad.current.dpad.down.isPressed || Gamepad.current.leftStick.up.isPressed || Gamepad.current.leftStick.down.isPressed))
			{
				ScrollToSelected(quickScroll: false);
			}
		}
	}

	private void ScrollToSelected(bool quickScroll)
	{
		int num = -1;
		Selectable selectable = (EventSystem.current.currentSelectedGameObject ? EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>() : null);
		if ((bool)selectable)
		{
			num = m_Selectables.IndexOf(selectable);
		}
		if (num > -1)
		{
			if (quickScroll)
			{
				m_ScrollRect.normalizedPosition = new Vector2(0f, 1f - (float)num / ((float)m_Selectables.Count - 1f));
				m_NextScrollPosition = m_ScrollRect.normalizedPosition;
			}
			else
			{
				m_NextScrollPosition = new Vector2(0f, 1f - (float)num / ((float)m_Selectables.Count - 1f));
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		mouseOver = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		mouseOver = false;
		ScrollToSelected(quickScroll: false);
	}
}
