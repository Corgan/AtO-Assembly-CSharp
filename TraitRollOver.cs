using System.Text;
using TMPro;
using UnityEngine;

public class TraitRollOver : MonoBehaviour
{
	private Color actColor;

	public TMP_Text traitName;

	public string traitId;

	public SpriteRenderer background;

	private TraitData td;

	private int traitLevel;

	private CardItem CI;

	private void Start()
	{
		actColor = background.color;
	}

	public void SetTrait(string _traitId, int _traitLevel = 0)
	{
		td = Globals.Instance.GetTraitData(_traitId);
		if (!(td != null))
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (td.TraitCard != null)
		{
			stringBuilder.Append("<voffset=-.15><size=2.6><sprite name=cards></size></voffset>");
			stringBuilder.Append(Globals.Instance.GetCardData(td.TraitCard.Id, instantiate: false).CardName);
			CI = GetComponent<CardItem>();
			if (CI != null)
			{
				CI.cardoutsidelibary = true;
				CI.cardoutsidecombat = true;
			}
		}
		else
		{
			stringBuilder.Append(td.TraitName);
			CI = GetComponent<CardItem>();
			if (CI != null)
			{
				CI.RemoveCardData();
			}
		}
		traitName.text = stringBuilder.ToString();
		traitId = _traitId;
		traitLevel = _traitLevel;
	}

	private void OnMouseEnter()
	{
		if (td != null)
		{
			if (td.TraitCard == null)
			{
				PopupManager.Instance.SetTrait(td);
			}
			else
			{
				string id = td.TraitCard.Id;
				if (traitLevel == 0)
				{
					CI.CreateAmplifyOutsideCard(Globals.Instance.GetCardData(id, instantiate: false), GetComponent<BoxCollider2D>());
				}
				else if (traitLevel == 1)
				{
					CI.CreateAmplifyOutsideCard(Globals.Instance.GetCardData(Globals.Instance.GetCardData(id, instantiate: false).UpgradesTo1), GetComponent<BoxCollider2D>());
				}
				else if (traitLevel == 2)
				{
					CI.CreateAmplifyOutsideCard(Globals.Instance.GetCardData(Globals.Instance.GetCardData(id, instantiate: false).UpgradesTo2), GetComponent<BoxCollider2D>());
				}
			}
		}
		if (TomeManager.Instance.IsActive())
		{
			background.color = new Color(0f, 0f, 0f, 0.5f);
		}
		else
		{
			background.color = new Color(1f, 0.5f, 0.13f, 0.25f);
		}
	}

	private void OnMouseExit()
	{
		GameManager.Instance.CleanTempContainer();
		PopupManager.Instance.ClosePopup();
		background.color = actColor;
	}
}
