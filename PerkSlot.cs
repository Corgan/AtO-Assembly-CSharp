using TMPro;
using UnityEngine;

public class PerkSlot : MonoBehaviour
{
	public TMP_Text title;

	public TMP_Text cards;

	public SpriteRenderer background;

	public Transform backgroundDisable;

	public Transform numContainer;

	public int slot;

	public Transform saveButton;

	public Transform deleteButton;

	private string colorActive = "#37303C";

	private string colorHover = "#735687";

	private string colorEmpty = "#54475E";

	private BoxCollider2D collider;

	private bool initiated;

	private void Init()
	{
		collider = GetComponent<BoxCollider2D>();
		saveButton.GetComponent<BotonGeneric>().auxInt = slot;
		deleteButton.GetComponent<BotonGeneric>().auxInt = slot;
	}

	public void SetDisable(bool _state)
	{
		if (!initiated)
		{
			Init();
		}
		backgroundDisable.gameObject.SetActive(_state);
	}

	public void SetEmpty(bool state)
	{
		if (!initiated)
		{
			Init();
		}
		title.text = Texts.Instance.GetText("emptySave");
		background.color = Functions.HexToColor(colorEmpty);
		numContainer.gameObject.SetActive(value: false);
		saveButton.gameObject.SetActive(value: true);
		deleteButton.gameObject.SetActive(value: false);
		collider.enabled = false;
		if (state)
		{
			saveButton.GetComponent<BotonGeneric>().Enable();
		}
		else
		{
			saveButton.GetComponent<BotonGeneric>().Disable();
		}
	}

	public void SetActive(string _title, string _num)
	{
		if (!initiated)
		{
			Init();
		}
		title.text = _title;
		cards.text = _num;
		background.color = Functions.HexToColor(colorActive);
		numContainer.gameObject.SetActive(value: true);
		saveButton.gameObject.SetActive(value: false);
		deleteButton.gameObject.SetActive(value: true);
		collider.enabled = true;
		background.color = Functions.HexToColor(colorActive);
	}

	public void OnMouseEnter()
	{
		if (!AlertManager.Instance.IsActive() && !SettingsManager.Instance.IsActive())
		{
			background.color = Functions.HexToColor(colorHover);
			PopupManager.Instance.SetText(string.Format(Texts.Instance.GetText("loadThisPerkConf"), cards.text), fast: true);
		}
	}

	public void OnMouseExit()
	{
		background.color = Functions.HexToColor(colorActive);
		PopupManager.Instance.ClosePopup();
	}

	public void OnMouseUp()
	{
		if (!AlertManager.Instance.IsActive() && !SettingsManager.Instance.IsActive())
		{
			PerkTree.Instance.LoadPerkConfig(slot);
			PopupManager.Instance.ClosePopup();
		}
	}
}
