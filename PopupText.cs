using UnityEngine;
using WebSocketSharp;

public class PopupText : MonoBehaviour
{
	public string id;

	public string position = "";

	public string text = "";

	public void SetId(string _id)
	{
		id = _id;
	}

	private void OnMouseEnter()
	{
		if ((AlertManager.Instance.IsActive() || GameManager.Instance.IsTutorialActive() || SettingsManager.Instance.IsActive() || DamageMeterManager.Instance.IsActive()) && id.Trim() != "optionalTelemetryTooltip")
		{
			return;
		}
		if (id.Trim() != "")
		{
			string text = Texts.Instance.GetText(id);
			if (!text.IsNullOrEmpty())
			{
				PopupManager.Instance.SetText(text, fast: true, position, alwaysCenter: true);
			}
		}
		else if (this.text.Trim() != "")
		{
			PopupManager.Instance.SetText(this.text.Trim(), fast: true, position, alwaysCenter: true);
		}
	}

	private void OnMouseDown()
	{
		if (id.Trim() == "charMiniPopupShow")
		{
			_ = base.transform.parent;
			HeroSelectionManager.Instance?.charPopupMini.SetSubClassData(GetComponent<HeroSelectionInfoHover>().heroSelection.subClassData);
		}
	}

	private void OnMouseExit()
	{
		PopupManager.Instance.ClosePopup();
	}

	public void OnMouseOver()
	{
		if (Input.GetMouseButtonUp(1))
		{
			HeroSelectionManager.Instance.charPopup.Init(GetComponent<HeroSelectionInfoHover>().heroSelection.subClassData);
			HeroSelectionManager.Instance.charPopup.Show();
		}
	}
}
