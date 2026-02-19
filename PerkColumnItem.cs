using TMPro;
using UnityEngine;

public class PerkColumnItem : MonoBehaviour
{
	private SpriteRenderer perkIcon;

	private TMP_Text perkText;

	private Transform perkBg;

	private BoxCollider2D boxCollider;

	private PerkData perkData;

	private new bool enabled;

	private bool active;

	private string textPopup = "";

	private string colorAvailable = "#226529";

	private string colorUnavailable = "#652523";

	private int pointsUsed;

	private int pointsAvailable;

	private string heroName;

	private string className;

	private string perkName;

	private void AwakeInit()
	{
		boxCollider = base.transform.GetComponent<BoxCollider2D>();
		perkIcon = base.transform.GetChild(0).transform.GetComponent<SpriteRenderer>();
		perkText = base.transform.GetChild(1).transform.GetComponent<TMP_Text>();
		perkBg = base.transform.GetChild(2).transform;
		perkBg.gameObject.SetActive(value: false);
	}

	public void EnablePerk(bool state)
	{
		if (perkIcon == null)
		{
			AwakeInit();
		}
		if (state)
		{
			SpriteRenderer spriteRenderer = perkIcon;
			Color color = (perkText.color = new Color(1f, 1f, 1f, 1f));
			spriteRenderer.color = color;
		}
		else
		{
			SpriteRenderer spriteRenderer2 = perkIcon;
			Color color = (perkText.color = new Color(0.6f, 0.6f, 0.6f, 1f));
			spriteRenderer2.color = color;
		}
		enabled = state;
	}

	public void SetPerk(string _className, string _heroName, int _index, int _subindex, int _pointsAvailable, int _pointsUsed)
	{
		heroName = _heroName;
		className = _className;
		pointsUsed = _pointsUsed;
		pointsAvailable = _pointsAvailable;
		perkName = _className + (_index + 1);
		switch (_subindex)
		{
		case 0:
			perkName += "a";
			break;
		case 1:
			perkName += "b";
			break;
		case 2:
			perkName += "c";
			break;
		case 3:
			perkName += "d";
			break;
		case 4:
			perkName += "e";
			break;
		case 5:
			perkName += "f";
			break;
		}
		active = PlayerManager.Instance.HeroHavePerk(_heroName, perkName);
		SetActive();
		perkData = Globals.Instance.GetPerkData(perkName);
		if (perkData != null)
		{
			if (perkData.Icon != null)
			{
				perkIcon.sprite = perkData.Icon;
			}
			perkText.text = perkData.IconTextValue;
			textPopup = Perk.PerkDescription(perkData, doPopup: true, _index, pointsAvailable, enabled, active);
		}
	}

	private void SetActive()
	{
		if (perkBg == null)
		{
			AwakeInit();
		}
		if (active)
		{
			perkBg.gameObject.SetActive(value: true);
			perkBg.GetComponent<SpriteRenderer>().color = new Color(0.39f, 0.2f, 0.52f, 1f);
		}
		else
		{
			perkBg.gameObject.SetActive(value: false);
			perkBg.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
		}
	}

	private void AssignPerk()
	{
		PlayerManager.Instance.AssignPerk(heroName, perkName);
		HeroSelectionManager.Instance.RefreshPerkPoints(heroName);
	}

	private void OnMouseUp()
	{
		Debug.Log(enabled);
		if (GameManager.Instance.IsLoadingGame())
		{
			AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("cantModifyPerkMP"));
			return;
		}
		if (enabled)
		{
			AssignPerk();
		}
		DoPopup();
	}

	private void DoPopup()
	{
		if (textPopup != "" && !(perkData.Icon == null))
		{
			AuraCurseData auraCurseData = Globals.Instance.GetAuraCurseData(perkData.Icon.name);
			string keynote = "";
			if (auraCurseData != null)
			{
				keynote = Functions.UppercaseFirst(perkData.Icon.name);
			}
			PopupManager.Instance.SetPerk(perkName, textPopup, keynote);
			if (enabled)
			{
				PopupManager.Instance.SetBackgroundColor(colorAvailable);
			}
			else
			{
				PopupManager.Instance.SetBackgroundColor(colorUnavailable);
			}
		}
	}

	private void OnMouseEnter()
	{
		DoPopup();
		if (!active)
		{
			perkBg.gameObject.SetActive(value: true);
		}
	}

	private void OnMouseExit()
	{
		if (!active)
		{
			perkBg.gameObject.SetActive(value: false);
		}
		PopupManager.Instance.ClosePopup();
	}
}
