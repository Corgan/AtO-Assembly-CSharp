using TMPro;
using UnityEngine;

public class DeckSlot : MonoBehaviour
{
	public Transform icon;

	public TMP_Text title;

	public TMP_Text cards;

	public SpriteRenderer background;

	public Transform numContainer;

	public int slot;

	public Transform saveButton;

	public Transform deleteButton;

	private string colorActive = "#AA580A";

	private string colorHover = "#00EDFF";

	private string colorEmpty = "#999999";

	private BoxCollider2D collider;

	private void Awake()
	{
		collider = GetComponent<BoxCollider2D>();
		saveButton.GetComponent<BotonGeneric>().auxInt = slot;
		deleteButton.GetComponent<BotonGeneric>().auxInt = slot;
	}

	public void SetEmpty(bool state)
	{
		title.text = Texts.Instance.GetText("emptySave");
		background.color = Functions.HexToColor(colorEmpty);
		icon.gameObject.SetActive(value: false);
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
		title.text = _title.ToUpper();
		cards.text = _num;
		background.color = Functions.HexToColor(colorActive);
		icon.gameObject.SetActive(value: true);
		numContainer.gameObject.SetActive(value: true);
		saveButton.gameObject.SetActive(value: false);
		deleteButton.gameObject.SetActive(value: true);
		collider.enabled = true;
		background.color = Functions.HexToColor(colorActive);
	}

	public void OnMouseEnter()
	{
		background.color = Functions.HexToColor(colorHover);
	}

	public void OnMouseExit()
	{
		background.color = Functions.HexToColor(colorActive);
	}

	public void OnMouseUp()
	{
		CardCraftManager.Instance.LoadDeck(slot);
	}
}
