using UnityEngine;

public class CardCraftSelectorRarity : MonoBehaviour
{
	public Transform tCommon;

	public Transform tUncommon;

	public Transform tRare;

	public Transform tEpic;

	public Transform tMythic;

	public Transform tLock;

	public Enums.CardRarity rarity;

	public Transform tOn;

	public Transform tOff;

	private new bool enabled;

	private bool locked;

	private void Start()
	{
		SetEnable(state: false);
		SetRarity(rarity);
	}

	public void SetRarity(Enums.CardRarity rarity)
	{
		tCommon.gameObject.SetActive(value: false);
		tUncommon.gameObject.SetActive(value: false);
		tRare.gameObject.SetActive(value: false);
		tEpic.gameObject.SetActive(value: false);
		tMythic.gameObject.SetActive(value: false);
		switch (rarity)
		{
		case Enums.CardRarity.Common:
			tCommon.gameObject.SetActive(value: true);
			break;
		case Enums.CardRarity.Uncommon:
			tUncommon.gameObject.SetActive(value: true);
			break;
		case Enums.CardRarity.Rare:
			tRare.gameObject.SetActive(value: true);
			break;
		case Enums.CardRarity.Epic:
			tEpic.gameObject.SetActive(value: true);
			break;
		default:
			tMythic.gameObject.SetActive(value: true);
			break;
		}
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
		if (!locked && !enabled)
		{
			tOn.gameObject.SetActive(value: true);
			tOff.gameObject.SetActive(value: false);
		}
	}

	private void OnMouseExit()
	{
		if (!locked && !enabled)
		{
			tOn.gameObject.SetActive(value: false);
			tOff.gameObject.SetActive(value: true);
		}
	}

	private void OnMouseUp()
	{
		if (!locked)
		{
			SetEnable(state: true);
			CardCraftManager.Instance.CraftSelectorRarity(this, rarity);
		}
	}
}
