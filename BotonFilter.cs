using UnityEngine;

public class BotonFilter : MonoBehaviour
{
	public Transform border;

	public Transform bg;

	public string id;

	public string type;

	private bool active;

	private void Awake()
	{
		ShowBorder(state: false);
		select(state: false);
	}

	private void ShowBorder(bool state)
	{
		border.gameObject.SetActive(state);
	}

	private void ShowBackground(bool state)
	{
		bg.gameObject.SetActive(state);
	}

	public void select(bool state)
	{
		active = state;
		ShowBackground(state);
	}

	private void OnMouseEnter()
	{
		ShowBorder(state: true);
		if (id != "")
		{
			PopupManager.Instance.SetText(Texts.Instance.GetText(id), fast: true);
		}
	}

	private void OnMouseExit()
	{
		ShowBorder(state: false);
		PopupManager.Instance.ClosePopup();
	}

	private void OnMouseUp()
	{
		select(!active);
		CardCraftManager.Instance.SelectFilter(type, id, active);
	}
}
