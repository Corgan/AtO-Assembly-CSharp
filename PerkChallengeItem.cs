using TMPro;
using UnityEngine;

public class PerkChallengeItem : MonoBehaviour
{
	private SpriteRenderer perkIcon;

	private TMP_Text perkText;

	private Transform perkBg;

	private Transform perkActive;

	private BoxCollider2D boxCollider;

	private PerkData perkData;

	private new bool enabled;

	private bool active;

	private string textPopup = "";

	private string colorAvailable = "#226529";

	private string colorUnavailable = "#652523";

	private int heroId;

	private int index = -1;

	private void Awake()
	{
		boxCollider = base.transform.GetComponent<BoxCollider2D>();
		perkIcon = base.transform.GetChild(0).transform.GetComponent<SpriteRenderer>();
		perkText = base.transform.GetChild(1).transform.GetComponent<TMP_Text>();
		perkBg = base.transform.GetChild(2).transform;
		perkActive = base.transform.GetChild(4).transform;
		if (perkBg.gameObject.activeSelf)
		{
			perkBg.gameObject.SetActive(value: false);
		}
	}

	public void EnablePerk(bool state)
	{
		enabled = state;
	}

	public void SetPerk(int _heroId, int _index, string _perkId)
	{
		heroId = _heroId;
		index = _index;
		if (perkBg.gameObject.activeSelf)
		{
			perkBg.gameObject.SetActive(value: false);
		}
		SetActive(state: false);
		perkData = Globals.Instance.GetPerkData(_perkId);
		if (perkData != null)
		{
			if (perkData.Icon != null)
			{
				perkIcon.sprite = perkData.Icon;
			}
			perkText.text = perkData.IconTextValue;
			textPopup = Perk.PerkDescription(perkData, doPopup: true, _index, 0, enabled, active);
			EnablePerk(state: true);
		}
	}

	public void SetActive(bool state)
	{
		if (perkActive.gameObject.activeSelf != state)
		{
			perkActive.gameObject.SetActive(state);
		}
		active = state;
	}

	private void AssignPerk()
	{
		ChallengeSelectionManager.Instance.AssignPerk(heroId, index);
	}

	public void OnMouseUp()
	{
		if (enabled)
		{
			GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
			AssignPerk();
		}
		DoPopup();
	}

	private void DoPopup()
	{
		if (textPopup != "")
		{
			AuraCurseData auraCurseData = Globals.Instance.GetAuraCurseData(perkData.Icon.name);
			string keynote = "";
			if (auraCurseData != null)
			{
				keynote = Functions.UppercaseFirst(perkData.Icon.name);
			}
			PopupManager.Instance.SetPerk("", textPopup, keynote);
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
		if (!active && !perkBg.gameObject.activeSelf)
		{
			perkBg.gameObject.SetActive(value: true);
		}
	}

	private void OnMouseExit()
	{
		if (!active && perkBg.gameObject.activeSelf)
		{
			perkBg.gameObject.SetActive(value: false);
		}
		PopupManager.Instance.ClosePopup();
	}
}
