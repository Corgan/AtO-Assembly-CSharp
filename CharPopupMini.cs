using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using WebSocketSharp;

public class CharPopupMini : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer portrait;

	[SerializeField]
	private SpriteRenderer bgGradient;

	[SerializeField]
	private TMP_Text heroNameTxt;

	[SerializeField]
	private TMP_Text heroClassTxt;

	[SerializeField]
	private TMP_Text rankTxt;

	[SerializeField]
	private TMP_Text descriptionTxt;

	[SerializeField]
	private TMP_Text[] resistTxts;

	[SerializeField]
	private TMP_Text healthTxt;

	[SerializeField]
	private TMP_Text energyTxt;

	[SerializeField]
	private TMP_Text speedTxt;

	[SerializeField]
	private GameObject DlcLogo;

	[SerializeField]
	private TMP_Text traitListTxt;

	[HideInInspector]
	public SubClassData subClassData;

	private Dictionary<string, string> heroClassIcons;

	private Color colorWhite = Color.white;

	private Color colorGray = new Color(0.665f, 0.665f, 0.665f);

	private void Awake()
	{
		heroClassIcons = new Dictionary<string, string>();
		heroClassIcons.Add("warrior", "block");
		heroClassIcons.Add("scout", "piercing");
		heroClassIcons.Add("healer", "regeneration");
		heroClassIcons.Add("mage", "fire");
		if (Globals.Instance?.CurrentLang == "zh-CN" || Globals.Instance?.CurrentLang == "zh-TW" || Globals.Instance?.CurrentLang == "jp")
		{
			traitListTxt.lineSpacing = 16f;
		}
	}

	private void Start()
	{
		Debug.Log("heloo");
	}

	public void SetSubClassData(SubClassData subclass)
	{
		subClassData = subclass;
		heroNameTxt.text = Texts.Instance.GetText(subclass.SubClassName.ToLower().Replace(" ", "") + "_name", "class");
		SetPortrait(subClassData);
		SetBGColor(subClassData);
		SetNameAndClass(subClassData);
		SetRank(subClassData);
		SetDescription(subClassData);
		SetResistances(subClassData);
		HeroSelectionManager.Instance.CardsandItemsTabUpdate(subClassData);
		HeroSelectionManager.Instance.TraitsTabUpdate(subClassData);
		SetHealth(subClassData);
		SetEnergy(subClassData);
		SetSpeed(subClassData);
		base.gameObject.SetActive(value: true);
		if (!subClassData.Sku.IsNullOrEmpty())
		{
			DlcLogo.SetActive(value: true);
			if (!subClassData.Sku.IsNullOrEmpty())
			{
				DlcLogo.SetActive(value: true);
				DlcLogo.GetComponent<PopupText>().text = string.Format(Texts.Instance.GetText("requiredDLC").Replace("#FFF", "#CEA843"), SteamManager.Instance.GetDLCName(subClassData.Sku));
			}
		}
		else
		{
			DlcLogo.SetActive(value: false);
		}
		HeroSelectionManager.Instance.charPopup.Init(subClassData, doStats: true, showNothing: true);
		HeroSelectionManager.Instance.charPopup.ShowStats();
		HeroSelectionManager.Instance.charPopup.AllowAllButtons();
		HeroSelectionManager.Instance.charPopup.Close();
	}

	private void SetNameAndClass(SubClassData subClassData)
	{
		heroClassTxt.text = Functions.UppercaseFirst(Texts.Instance.GetText(subClassData.SubClassName));
		string text = Functions.ClassIconFromString(Enum.GetName(typeof(Enums.HeroClass), subClassData.HeroClass));
		heroClassTxt.color = GetClassColor(subClassData);
		TMP_Text tMP_Text = heroClassTxt;
		tMP_Text.text = tMP_Text.text + " <sprite name=" + text + ">";
		if (subClassData.HeroClassSecondary != Enums.HeroClass.None)
		{
			text = Functions.ClassIconFromString(Enum.GetName(typeof(Enums.HeroClass), subClassData.HeroClassSecondary));
			TMP_Text tMP_Text2 = heroClassTxt;
			tMP_Text2.text = tMP_Text2.text + "<sprite name=" + text + ">";
		}
		if (subClassData.HeroClassThird != Enums.HeroClass.None)
		{
			text = Functions.ClassIconFromString(Enum.GetName(typeof(Enums.HeroClass), subClassData.HeroClassSecondary));
			TMP_Text tMP_Text3 = heroClassTxt;
			tMP_Text3.text = tMP_Text3.text + "<sprite name=" + text + ">";
		}
	}

	private Color GetClassColor(SubClassData subClassData)
	{
		return Functions.HexToColor(Globals.Instance.ClassColor[Enum.GetName(typeof(Enums.HeroClass), subClassData.HeroClass)]);
	}

	private void SetRank(SubClassData subClassData)
	{
		rankTxt.text = string.Format(Texts.Instance.GetText("rankProgress"), PlayerManager.Instance.GetPerkRank(subClassData.Id));
	}

	private void SetBGColor(SubClassData subClassData)
	{
		Color classColor = GetClassColor(subClassData);
		bgGradient.color = new Color(classColor.r, classColor.g, classColor.b, 0.2f);
	}

	private void SetDescription(SubClassData subClassData)
	{
		descriptionTxt.text = Texts.Instance.GetText(subClassData.Id + "_strength", "class");
	}

	private void SetResistances(SubClassData _scd)
	{
		SetResistText(_scd, resistTxts[0], Enums.DamageType.Slashing);
		SetResistText(_scd, resistTxts[1], Enums.DamageType.Blunt);
		SetResistText(_scd, resistTxts[2], Enums.DamageType.Piercing);
		SetResistText(_scd, resistTxts[3], Enums.DamageType.Fire);
		SetResistText(_scd, resistTxts[4], Enums.DamageType.Cold);
		SetResistText(_scd, resistTxts[5], Enums.DamageType.Lightning);
		SetResistText(_scd, resistTxts[6], Enums.DamageType.Mind);
		SetResistText(_scd, resistTxts[7], Enums.DamageType.Holy);
		SetResistText(_scd, resistTxts[8], Enums.DamageType.Shadow);
	}

	private void SetResistText(SubClassData _scd, TMP_Text resistText, Enums.DamageType damageType)
	{
		int num = 0;
		switch (damageType)
		{
		case Enums.DamageType.Slashing:
			num = _scd.ResistSlashing;
			break;
		case Enums.DamageType.Blunt:
			num = _scd.ResistBlunt;
			break;
		case Enums.DamageType.Piercing:
			num = _scd.ResistPiercing;
			break;
		case Enums.DamageType.Fire:
			num = _scd.ResistFire;
			break;
		case Enums.DamageType.Cold:
			num = _scd.ResistCold;
			break;
		case Enums.DamageType.Lightning:
			num = _scd.ResistLightning;
			break;
		case Enums.DamageType.Mind:
			num = _scd.ResistMind;
			break;
		case Enums.DamageType.Holy:
			num = _scd.ResistHoly;
			break;
		case Enums.DamageType.Shadow:
			num = _scd.ResistShadow;
			break;
		}
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			num += PlayerManager.Instance.GetPerkResistBonus(_scd.Id, damageType);
		}
		if (Functions.SpaceBeforePercentSign())
		{
			resistText.text = num + " %";
		}
		else
		{
			resistText.text = num + "%";
		}
		if (num > 0)
		{
			resistText.color = colorWhite;
		}
		else
		{
			resistText.color = colorGray;
		}
	}

	private void SetHealth(SubClassData _scd)
	{
		int num = _scd.Hp;
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			num += PlayerManager.Instance.GetPerkMaxHealth(_scd.Id);
		}
		healthTxt.text = num.ToString();
	}

	private void SetEnergy(SubClassData _scd)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = _scd.Energy;
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			num += PlayerManager.Instance.GetPerkEnergyBegin(_scd.Id);
		}
		stringBuilder.Append(num);
		stringBuilder.Append(" <size=1.3>");
		stringBuilder.Append(Texts.Instance.GetText("dataPerTurn").Replace("<%>", _scd.EnergyTurn.ToString()));
		stringBuilder.Append("</size>");
		energyTxt.text = stringBuilder.ToString();
	}

	private void SetSpeed(SubClassData _scd)
	{
		int num = _scd.Speed;
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			num += PlayerManager.Instance.GetPerkSpeed(_scd.Id);
		}
		speedTxt.text = num.ToString();
	}

	public void SetPortrait(SubClassData subClassData)
	{
		string activeSkin = PlayerManager.Instance.GetActiveSkin(subClassData.Id);
		if (activeSkin != "")
		{
			SkinData skinData = Globals.Instance.GetSkinData(activeSkin);
			if (skinData == null)
			{
				activeSkin = Globals.Instance.GetSkinBaseIdBySubclass(subClassData.Id);
				skinData = Globals.Instance.GetSkinData(activeSkin);
			}
			portrait.sprite = skinData.SpriteSiluetaGrande;
		}
		else
		{
			portrait.sprite = subClassData.SpriteBorder;
		}
	}

	public void ClosePopup()
	{
		base.gameObject.SetActive(value: false);
	}
}
