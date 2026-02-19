using TMPro;
using UnityEngine;

public class CardCraftSelectorEnergy : MonoBehaviour
{
	public TMP_Text tText;

	public Transform tOn;

	public Transform tOff;

	private new bool enabled;

	private void Start()
	{
		SetEnable(state: false);
	}

	public void SetText(string text)
	{
		tText.text = text;
	}

	public void SetEnable(bool state)
	{
		enabled = state;
		if (state)
		{
			tOn.gameObject.SetActive(value: true);
			tOff.gameObject.SetActive(value: false);
		}
		else
		{
			tOn.gameObject.SetActive(value: false);
			tOff.gameObject.SetActive(value: true);
		}
	}

	private void OnMouseEnter()
	{
		if (!enabled)
		{
			tOn.gameObject.SetActive(value: true);
			tOff.gameObject.SetActive(value: false);
		}
	}

	private void OnMouseExit()
	{
		if (!enabled)
		{
			tOn.gameObject.SetActive(value: false);
			tOff.gameObject.SetActive(value: true);
		}
	}

	private void OnMouseUp()
	{
		SetEnable(state: true);
		CardCraftManager.Instance.CraftSelectorEnergy(this, tText.text);
	}
}
