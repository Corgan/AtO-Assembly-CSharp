using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class StatsWindowUI : MonoBehaviour
{
	public GameObject BuffPrefab;

	public GameObject GO_Buffs;

	public GameObject GO_Immunities;

	public GameObject GO_AuraCurse;

	public Transform[] dmageTypeT;

	private TMP_Text[] damageTypeText;

	private TMP_Text[] resistanceText;

	private TMP_Text[] ddText;

	private TMP_Text[] dtText;

	private PopupText[] resistancePop;

	private PopupText[] damagedonePop;

	private PopupText[] damagetakenPop;

	public TMP_Text statsName;

	public TMP_Text statsHealth;

	public TMP_Text statsEnergy;

	public TMP_Text statsSpeed;

	public TMP_Text statsCards;

	public TMP_Text globalDamageDonePercent;

	public PopupText globalDamageDonePop;

	public TMP_Text globalHealingDonePercent;

	public PopupText globalHealingDonePercentPop;

	public TMP_Text globalHealingDoneFlat;

	public PopupText globalHealingDoneFlatPop;

	public TMP_Text globalHealingTakenPercent;

	public PopupText globalHealingTakenPercentPop;

	public TMP_Text globalHealingTakenFlat;

	public PopupText globalHealingTakenFlatPop;

	private string colorRed = "#C34738";

	private string colorGreen = "#2FBA23";

	public Transform notEffects;

	public Transform notImmune;

	public Transform yesImmune;

	public Transform notCharges;

	private Character character;

	private void Awake()
	{
		damageTypeText = new TMP_Text[dmageTypeT.Length];
		resistanceText = new TMP_Text[dmageTypeT.Length];
		resistancePop = new PopupText[dmageTypeT.Length];
		damagedonePop = new PopupText[dmageTypeT.Length];
		damagetakenPop = new PopupText[dmageTypeT.Length];
		ddText = new TMP_Text[dmageTypeT.Length];
		dtText = new TMP_Text[dmageTypeT.Length];
		for (int i = 0; i < dmageTypeT.Length; i++)
		{
			damageTypeText[i] = dmageTypeT[i].GetChild(0).GetComponent<TMP_Text>();
			resistancePop[i] = dmageTypeT[i].GetChild(2).GetComponent<PopupText>();
			resistanceText[i] = dmageTypeT[i].GetChild(3).GetComponent<TMP_Text>();
			damagedonePop[i] = dmageTypeT[i].GetChild(4).GetComponent<PopupText>();
			ddText[i] = dmageTypeT[i].GetChild(5).GetComponent<TMP_Text>();
			damagetakenPop[i] = dmageTypeT[i].GetChild(6).GetComponent<PopupText>();
			dtText[i] = dmageTypeT[i].GetChild(7).GetComponent<TMP_Text>();
		}
	}

	private void Start()
	{
		damageTypeText[0].text = GetTextDT("slash");
		damageTypeText[1].text = GetTextDT("blunt");
		damageTypeText[2].text = GetTextDT("piercing");
		damageTypeText[3].text = GetTextDT("fire");
		damageTypeText[4].text = GetTextDT("cold");
		damageTypeText[5].text = GetTextDT("lightning");
		damageTypeText[6].text = GetTextDT("holy");
		damageTypeText[7].text = GetTextDT("shadow");
		damageTypeText[8].text = GetTextDT("mind");
	}

	public void DoStats(Character _character)
	{
		character = _character;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(_character.HpCurrent);
		stringBuilder.Append(" <color=#AAA><size=1.7>/");
		stringBuilder.Append(_character.Hp);
		stringBuilder.Append("</size></color>");
		statsHealth.text = stringBuilder.ToString();
		stringBuilder.Clear();
		int[] speed = _character.GetSpeed();
		stringBuilder.Append(speed[0].ToString());
		stringBuilder.Append(" <color=#AAA><size=1.7>/");
		stringBuilder.Append(speed[1].ToString());
		stringBuilder.Append("</size></color>");
		statsSpeed.text = stringBuilder.ToString();
		stringBuilder.Clear();
		if (_character.IsHero)
		{
			statsEnergy.transform.parent.gameObject.SetActive(value: true);
			statsCards.transform.parent.gameObject.SetActive(value: true);
			statsName.gameObject.SetActive(value: false);
			if (MatchManager.Instance != null)
			{
				stringBuilder.Append(_character.EnergyCurrent);
			}
			else
			{
				stringBuilder.Append(_character.Energy);
			}
			stringBuilder.Append(" <color=#AAA><size=1.7>");
			stringBuilder.Append(Texts.Instance.GetText("dataPerTurn").Replace("<%>", _character.EnergyTurn.ToString()));
			stringBuilder.Append("</size></color>");
			statsEnergy.text = stringBuilder.ToString();
			stringBuilder.Clear();
			stringBuilder.Append(_character.GetDrawCardsTurnForDisplayInDeck());
			stringBuilder.Append(" <color=#AAA><size=1.7>");
			stringBuilder.Append(Texts.Instance.GetText("dataPerTurn").Replace("<%>", ""));
			stringBuilder.Append("</size></color>");
			statsCards.text = stringBuilder.ToString();
		}
		else
		{
			statsName.gameObject.SetActive(value: true);
			statsName.text = _character.SourceName;
			statsEnergy.transform.parent.gameObject.SetActive(value: false);
			statsCards.transform.parent.gameObject.SetActive(value: false);
		}
		int num = (int)_character.GetTraitDamagePercentModifiers(Enums.DamageType.All);
		int num2 = 0;
		Dictionary<string, int> itemDamageDonePercentDictionary = _character.GetItemDamageDonePercentDictionary(Enums.DamageType.All);
		foreach (KeyValuePair<string, int> item in itemDamageDonePercentDictionary)
		{
			num2 += item.Value;
		}
		int num3 = 0;
		Dictionary<string, int> auraDamageDonePercentDictionary = _character.GetAuraDamageDonePercentDictionary(Enums.DamageType.All);
		foreach (KeyValuePair<string, int> item2 in auraDamageDonePercentDictionary)
		{
			num3 += item2.Value;
		}
		int num4 = num + num2 + num3;
		if (num4 < -50)
		{
			num4 = -50;
		}
		globalDamageDonePercent.text = FormatPercent(num4, num3);
		DoPopupGeneral(itemDamageDonePercentDictionary, auraDamageDonePercentDictionary, globalDamageDonePop, "percent", showBase: true, num);
		float[] array = _character.HealBonus(0);
		int num5 = (int)array[1];
		int num6 = (int)_character.GetTraitHealPercentBonus();
		int num7 = (int)_character.GetItemHealPercentBonus();
		num5 += num6 + num7;
		globalHealingDonePercent.text = FormatPercent(num5, (int)array[1]);
		DoPopupGeneral(_character.GetItemHealPercentDictionary(), _character.GetAuraHealPercentDictionary(), globalHealingDonePercentPop, "percent", showBase: true, num6 + num7);
		int num8 = (int)array[0];
		int traitHealFlatBonus = _character.GetTraitHealFlatBonus();
		int itemHealFlatBonus = _character.GetItemHealFlatBonus();
		int num9 = 0;
		if (_character.IsHero)
		{
			num9 = PlayerManager.Instance.GetPerkHealBonus(_character.HeroData.HeroSubClass.Id);
		}
		num8 += traitHealFlatBonus + itemHealFlatBonus + num9;
		globalHealingDoneFlat.text = FormatSum(num8, (int)array[0], notLessThanZero: false);
		DoPopupGeneral(_character.GetItemHealFlatDictionary(), _character.GetAuraHealFlatDictionary(), globalHealingDoneFlatPop, "flat", showBase: true, traitHealFlatBonus + itemHealFlatBonus);
		float[] array2 = _character.HealReceivedBonus();
		int num10 = (int)array2[1];
		int num11 = (int)_character.GetTraitHealReceivedPercentBonus();
		int num12 = (int)_character.GetItemHealReceivedPercentBonus();
		num10 += num11 + num12;
		globalHealingTakenPercent.text = FormatPercent(num10, (int)array2[1]);
		DoPopupGeneral(null, _character.GetAuraHealReceivedPercentDictionary(), globalHealingTakenPercentPop, "percent", showBase: true, num11);
		int num13 = (int)array2[0];
		int traitHealReceivedFlatBonus = _character.GetTraitHealReceivedFlatBonus();
		int itemHealReceivedFlatBonus = _character.GetItemHealReceivedFlatBonus();
		num13 += traitHealReceivedFlatBonus + itemHealReceivedFlatBonus;
		globalHealingTakenFlat.text = FormatSum(num13, (int)array2[0]);
		DoPopupGeneral(null, _character.GetAuraHealReceivedFlatDictionary(), globalHealingTakenFlatPop, "flat", showBase: true, traitHealReceivedFlatBonus);
		int num14 = 0;
		int num15 = 0;
		int num16 = 0;
		int num17 = 0;
		int num18 = 0;
		int num19 = 0;
		int num20 = 0;
		int num21 = 0;
		Enums.DamageType damageType = Enums.DamageType.Slashing;
		num21 = 0;
		num14 = _character.BonusResists(damageType);
		num15 = _character.GetAuraResistModifiers(damageType);
		num16 = _character.GetItemResistModifiers(damageType);
		DoPopupResistance(num21, damageType, _character.ResistSlashing, num15, num16);
		resistanceText[num21].text = FormatResistance(num14, num15);
		num19 = _character.TotalDamageWithCharacterFlatBonus(damageType);
		num17 = (int)_character.DamageBonus(damageType)[0];
		num18 = _character.GetItemDamageFlatModifiers(damageType);
		DoPopupDamageDone(num21, damageType, num19, num17, num18);
		ddText[num21].text = FormatSum(num19, num17);
		num20 = _character.IncreasedCursedDamagePerStack(damageType);
		DoPopupDamageTaken(num21, damageType, num20);
		dtText[num21].text = FormatTaken(num20);
		damageType = Enums.DamageType.Blunt;
		num21 = 1;
		num14 = _character.BonusResists(damageType);
		num15 = _character.GetAuraResistModifiers(damageType);
		num16 = _character.GetItemResistModifiers(damageType);
		DoPopupResistance(num21, damageType, _character.ResistBlunt, num15, num16);
		resistanceText[num21].text = FormatResistance(num14, num15);
		num19 = _character.TotalDamageWithCharacterFlatBonus(damageType);
		num17 = (int)_character.DamageBonus(damageType)[0];
		num18 = _character.GetItemDamageFlatModifiers(damageType);
		DoPopupDamageDone(num21, damageType, num19, num17, num18);
		ddText[num21].text = FormatSum(num19, num17);
		num20 = _character.IncreasedCursedDamagePerStack(damageType);
		DoPopupDamageTaken(num21, damageType, num20);
		dtText[num21].text = FormatTaken(num20);
		damageType = Enums.DamageType.Piercing;
		num21 = 2;
		num14 = _character.BonusResists(damageType);
		num15 = _character.GetAuraResistModifiers(damageType);
		num16 = _character.GetItemResistModifiers(damageType);
		DoPopupResistance(num21, damageType, _character.ResistPiercing, num15, num16);
		resistanceText[num21].text = FormatResistance(num14, num15);
		num19 = _character.TotalDamageWithCharacterFlatBonus(damageType);
		num17 = (int)_character.DamageBonus(damageType)[0];
		num18 = _character.GetItemDamageFlatModifiers(damageType);
		DoPopupDamageDone(num21, damageType, num19, num17, num18);
		ddText[num21].text = FormatSum(num19, num17);
		num20 = _character.IncreasedCursedDamagePerStack(damageType);
		DoPopupDamageTaken(num21, damageType, num20);
		dtText[num21].text = FormatTaken(num20);
		damageType = Enums.DamageType.Fire;
		num21 = 3;
		num14 = _character.BonusResists(damageType);
		num15 = _character.GetAuraResistModifiers(damageType);
		num16 = _character.GetItemResistModifiers(damageType);
		DoPopupResistance(num21, damageType, _character.ResistFire, num15, num16);
		resistanceText[num21].text = FormatResistance(num14, num15);
		num19 = _character.TotalDamageWithCharacterFlatBonus(damageType);
		num17 = (int)_character.DamageBonus(damageType)[0];
		num18 = _character.GetItemDamageFlatModifiers(damageType);
		DoPopupDamageDone(num21, damageType, num19, num17, num18);
		ddText[num21].text = FormatSum(num19, num17);
		num20 = _character.IncreasedCursedDamagePerStack(damageType);
		DoPopupDamageTaken(num21, damageType, num20);
		dtText[num21].text = FormatTaken(num20);
		damageType = Enums.DamageType.Cold;
		num21 = 4;
		num14 = _character.BonusResists(damageType);
		num15 = _character.GetAuraResistModifiers(damageType);
		num16 = _character.GetItemResistModifiers(damageType);
		DoPopupResistance(num21, damageType, _character.ResistCold, num15, num16);
		resistanceText[num21].text = FormatResistance(num14, num15);
		num19 = _character.TotalDamageWithCharacterFlatBonus(damageType);
		num17 = (int)_character.DamageBonus(damageType)[0];
		num18 = _character.GetItemDamageFlatModifiers(damageType);
		DoPopupDamageDone(num21, damageType, num19, num17, num18);
		ddText[num21].text = FormatSum(num19, num17);
		num20 = _character.IncreasedCursedDamagePerStack(damageType);
		DoPopupDamageTaken(num21, damageType, num20);
		dtText[num21].text = FormatTaken(num20);
		damageType = Enums.DamageType.Lightning;
		num21 = 5;
		num14 = _character.BonusResists(damageType);
		num15 = _character.GetAuraResistModifiers(damageType);
		num16 = _character.GetItemResistModifiers(damageType);
		DoPopupResistance(num21, damageType, _character.ResistLightning, num15, num16);
		resistanceText[num21].text = FormatResistance(num14, num15);
		num19 = _character.TotalDamageWithCharacterFlatBonus(damageType);
		num17 = (int)_character.DamageBonus(damageType)[0];
		num18 = _character.GetItemDamageFlatModifiers(damageType);
		DoPopupDamageDone(num21, damageType, num19, num17, num18);
		ddText[num21].text = FormatSum(num19, num17);
		num20 = _character.IncreasedCursedDamagePerStack(damageType);
		DoPopupDamageTaken(num21, damageType, num20);
		dtText[num21].text = FormatTaken(num20);
		damageType = Enums.DamageType.Holy;
		num21 = 6;
		num14 = _character.BonusResists(damageType);
		num15 = _character.GetAuraResistModifiers(damageType);
		num16 = _character.GetItemResistModifiers(damageType);
		DoPopupResistance(num21, damageType, _character.ResistHoly, num15, num16);
		resistanceText[num21].text = FormatResistance(num14, num15);
		num19 = _character.TotalDamageWithCharacterFlatBonus(damageType);
		num17 = (int)_character.DamageBonus(damageType)[0];
		num18 = _character.GetItemDamageFlatModifiers(damageType);
		DoPopupDamageDone(num21, damageType, num19, num17, num18);
		ddText[num21].text = FormatSum(num19, num17);
		num20 = _character.IncreasedCursedDamagePerStack(damageType);
		DoPopupDamageTaken(num21, damageType, num20);
		dtText[num21].text = FormatTaken(num20);
		damageType = Enums.DamageType.Shadow;
		num21 = 7;
		num14 = _character.BonusResists(damageType);
		num15 = _character.GetAuraResistModifiers(damageType);
		num16 = _character.GetItemResistModifiers(damageType);
		DoPopupResistance(num21, damageType, _character.ResistShadow, num15, num16);
		resistanceText[num21].text = FormatResistance(num14, num15);
		num19 = _character.TotalDamageWithCharacterFlatBonus(damageType);
		num17 = (int)_character.DamageBonus(damageType)[0];
		num18 = _character.GetItemDamageFlatModifiers(damageType);
		DoPopupDamageDone(num21, damageType, num19, num17, num18);
		ddText[num21].text = FormatSum(num19, num17);
		num20 = _character.IncreasedCursedDamagePerStack(damageType);
		DoPopupDamageTaken(num21, damageType, num20);
		dtText[num21].text = FormatTaken(num20);
		damageType = Enums.DamageType.Mind;
		num21 = 8;
		num14 = _character.BonusResists(damageType);
		num15 = _character.GetAuraResistModifiers(damageType);
		num16 = _character.GetItemResistModifiers(damageType);
		DoPopupResistance(num21, damageType, _character.ResistMind, num15, num16);
		resistanceText[num21].text = FormatResistance(num14, num15);
		num19 = _character.TotalDamageWithCharacterFlatBonus(damageType);
		num17 = (int)_character.DamageBonus(damageType)[0];
		num18 = _character.GetItemDamageFlatModifiers(damageType);
		DoPopupDamageDone(num21, damageType, num19, num17, num18);
		ddText[num21].text = FormatSum(num19, num17);
		num20 = _character.IncreasedCursedDamagePerStack(damageType);
		DoPopupDamageTaken(num21, damageType, num20);
		dtText[num21].text = FormatTaken(num20);
		for (int i = 0; i < GO_Buffs.transform.childCount; i++)
		{
			Object.Destroy(GO_Buffs.transform.GetChild(i).gameObject);
		}
		int count = _character.AuraList.Count;
		if (count > 0)
		{
			notEffects.transform.gameObject.SetActive(value: false);
			for (int j = 0; j < count; j++)
			{
				Aura aura = _character.AuraList[j];
				if (aura != null && aura.ACData.Id != "stealthbonus" && aura.ACData.Id != "furnace")
				{
					GameObject obj = Object.Instantiate(BuffPrefab, Vector3.zero, Quaternion.identity, GO_Buffs.transform);
					obj.GetComponent<Buff>().SetBuff(aura.ACData, aura.GetCharges(), "", _character.Id);
					obj.GetComponent<Buff>().SetBuffInStats();
				}
			}
		}
		else
		{
			notEffects.transform.gameObject.SetActive(value: true);
		}
		for (int k = 0; k < GO_Immunities.transform.childCount; k++)
		{
			Object.Destroy(GO_Immunities.transform.GetChild(k).gameObject);
		}
		List<string> list = new List<string>();
		if (character.AuracurseImmune.Count > 0)
		{
			for (int l = 0; l < character.AuracurseImmune.Count; l++)
			{
				list.Add(character.AuracurseImmune[l]);
			}
		}
		List<string> list2 = character.AuraCurseImmunitiesByItemsList();
		for (int m = 0; m < list2.Count; m++)
		{
			if (!list.Contains(list2[m]))
			{
				list.Add(list2[m]);
			}
		}
		if (list.Count > 0)
		{
			for (int n = 0; n < list.Count; n++)
			{
				GameObject obj2 = Object.Instantiate(BuffPrefab, Vector3.zero, Quaternion.identity, GO_Immunities.transform);
				obj2.GetComponent<Buff>().SetBuff(Globals.Instance.GetAuraCurseData(list[n]), 1, "", _character.Id);
				obj2.GetComponent<Buff>().SetBuffInStats(_auraImmunity: true);
			}
			notImmune.gameObject.SetActive(value: false);
		}
		else
		{
			yesImmune.gameObject.SetActive(value: false);
			notImmune.gameObject.SetActive(value: true);
		}
		Dictionary<string, int> dictionary = character.AuraCurseModification;
		Dictionary<string, int> itemAuraCurseModifiers = character.GetItemAuraCurseModifiers();
		Dictionary<string, int> traitAuraCurseModifiers = character.GetTraitAuraCurseModifiers();
		Dictionary<string, int> aurasAuraCurseModifiers = character.GetAurasAuraCurseModifiers();
		Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
		if (character.HeroData != null && character.HeroData.HeroSubClass != null)
		{
			dictionary = Perk.GetAuraCurseBonusDict(character.HeroData.HeroSubClass.Id);
		}
		foreach (KeyValuePair<string, int> item3 in itemAuraCurseModifiers)
		{
			if (item3.Key != "")
			{
				if (dictionary2.ContainsKey(item3.Key))
				{
					dictionary2[item3.Key] += item3.Value;
				}
				else
				{
					dictionary2.Add(item3.Key, item3.Value);
				}
			}
		}
		foreach (KeyValuePair<string, int> item4 in traitAuraCurseModifiers)
		{
			if (item4.Key != "")
			{
				if (dictionary2.ContainsKey(item4.Key) && item4.Value > 0)
				{
					dictionary2[item4.Key] += item4.Value;
				}
				else if (item4.Value > 0)
				{
					dictionary2.Add(item4.Key, item4.Value);
				}
			}
		}
		foreach (KeyValuePair<string, int> item5 in dictionary)
		{
			if (item5.Key != "")
			{
				if (dictionary2.ContainsKey(item5.Key) && item5.Value > 0)
				{
					dictionary2[item5.Key] += item5.Value;
				}
				else if (item5.Value > 0)
				{
					dictionary2.Add(item5.Key, item5.Value);
				}
			}
		}
		foreach (KeyValuePair<string, int> item6 in aurasAuraCurseModifiers)
		{
			if (item6.Key != "")
			{
				if (dictionary2.ContainsKey(item6.Key))
				{
					dictionary2[item6.Key] += item6.Value;
				}
				else if (item6.Value > 0)
				{
					dictionary2.Add(item6.Key, item6.Value);
				}
			}
		}
		for (int num22 = 0; num22 < GO_AuraCurse.transform.childCount; num22++)
		{
			Object.Destroy(GO_AuraCurse.transform.GetChild(num22).gameObject);
		}
		if (dictionary2.Count > 0)
		{
			foreach (KeyValuePair<string, int> item7 in dictionary2)
			{
				if (item7.Value == 0)
				{
					continue;
				}
				GameObject gameObject = Object.Instantiate(BuffPrefab, Vector3.zero, Quaternion.identity, GO_AuraCurse.transform);
				string text = "";
				if (item7.Value > 0)
				{
					text = "+" + item7.Value;
				}
				else
				{
					if (item7.Value >= 0)
					{
						continue;
					}
					text = item7.Value.ToString() ?? "";
				}
				gameObject.GetComponent<Buff>().SetBuff(Globals.Instance.GetAuraCurseData(item7.Key), item7.Value, text, _character.Id);
				gameObject.GetComponent<Buff>().SetBuffInStatsCharges();
			}
			notCharges.gameObject.SetActive(value: false);
		}
		else
		{
			notCharges.gameObject.SetActive(value: true);
		}
	}

	private void DoPopupResistance(int resistanceIndex, Enums.DamageType resistanceType, int resistanceValue, int resistanceMod, int resistanceItems)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<line-height=140%><size=+2>");
		stringBuilder.Append("<size=+4><sprite name=man></size> <color=#FFF>");
		stringBuilder.Append(Texts.Instance.GetText("baseResist"));
		stringBuilder.Append(":</color> ");
		stringBuilder.Append(resistanceValue);
		if (Functions.SpaceBeforePercentSign())
		{
			stringBuilder.Append(" ");
		}
		stringBuilder.Append("%");
		stringBuilder.Append("\n");
		int num = 0;
		if (resistanceMod != 0 || resistanceItems != 0)
		{
			if (resistanceItems != 0)
			{
				foreach (KeyValuePair<string, int> item in character.GetItemResistModifiersDictionary(resistanceType))
				{
					string[] array = item.Key.Split('_');
					if (array.Length > 1)
					{
						if (num > 0)
						{
							stringBuilder.Append("\n");
						}
						stringBuilder.Append("<size=+4><sprite name=");
						if (array[1].ToLower() == "enchantment")
						{
							stringBuilder.Append("card");
						}
						else
						{
							stringBuilder.Append(array[1].ToLower());
						}
						stringBuilder.Append("></size> ");
						stringBuilder.Append("<color=#A9815D>");
						stringBuilder.Append(array[0]);
						stringBuilder.Append(":</color> ");
						if (item.Value > 0)
						{
							stringBuilder.Append("+");
							stringBuilder.Append(item.Value);
						}
						else
						{
							stringBuilder.Append(item.Value);
						}
						if (Functions.SpaceBeforePercentSign())
						{
							stringBuilder.Append(" ");
						}
						stringBuilder.Append("%");
						num++;
					}
				}
			}
			if (resistanceMod != 0)
			{
				foreach (KeyValuePair<string, int> item2 in character.GetAuraResistModifiersDictionary(resistanceType))
				{
					if (num > 0)
					{
						stringBuilder.Append("\n");
					}
					stringBuilder.Append("<size=+4><sprite name=");
					if (item2.Key == "enchantment")
					{
						stringBuilder.Append("card");
					}
					else
					{
						stringBuilder.Append(item2.Key);
					}
					stringBuilder.Append("></size> ");
					if (item2.Value > 0)
					{
						stringBuilder.Append("<color=#5D82A8>");
					}
					else if (item2.Value < 0)
					{
						stringBuilder.Append("<color=#A85D6A>");
					}
					else
					{
						stringBuilder.Append("<color=#FFF>");
					}
					if (Texts.Instance.GetText(item2.Key) != "")
					{
						stringBuilder.Append(Functions.UppercaseFirst(Texts.Instance.GetText(item2.Key)));
					}
					else
					{
						stringBuilder.Append(Functions.UppercaseFirst(item2.Key));
					}
					stringBuilder.Append(":</color> ");
					if (item2.Value > 0)
					{
						stringBuilder.Append("+");
						stringBuilder.Append(item2.Value);
					}
					else
					{
						stringBuilder.Append(item2.Value);
					}
					if (Functions.SpaceBeforePercentSign())
					{
						stringBuilder.Append(" ");
					}
					stringBuilder.Append("%");
					num++;
				}
			}
		}
		if (!character.IsHero)
		{
			if (AtOManager.Instance.IsChallengeTraitActive("vulnerablemonsters"))
			{
				if (num > 0)
				{
					stringBuilder.Append("\n");
				}
				stringBuilder.Append("<color=#DE96C2>");
				stringBuilder.Append(Texts.Instance.GetText("vulnerablemonsters"));
				stringBuilder.Append(":</color> -15");
				if (Functions.SpaceBeforePercentSign())
				{
					stringBuilder.Append(" ");
				}
				stringBuilder.Append("%");
			}
			if (resistanceType == Enums.DamageType.Fire && AtOManager.Instance.AliveTeamHaveTrait("crimsonripple"))
			{
				int auraCharges = character.GetAuraCharges("bleed");
				if (auraCharges > 0)
				{
					if (num > 0)
					{
						stringBuilder.Append("\n");
					}
					stringBuilder.Append("<color=#DE96C2>Crimson Ripple</color> -");
					stringBuilder.Append(auraCharges / 2);
					if (Functions.SpaceBeforePercentSign())
					{
						stringBuilder.Append(" ");
					}
					stringBuilder.Append("%");
				}
			}
		}
		if (resistancePop != null && resistanceIndex < resistancePop.Length)
		{
			resistancePop[resistanceIndex].text = stringBuilder.ToString();
		}
	}

	private void DoPopupDamageDone(int resistanceIndex, Enums.DamageType damageType, int damageBase, int damageAura, int damageItems)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = damageBase - damageAura - damageItems;
		if (num != 0 || damageAura != 0 || damageItems != 0)
		{
			int num2 = 0;
			stringBuilder.Append("<line-height=140%><size=+2>");
			if (num >= 0)
			{
				stringBuilder.Append("<size=+4><sprite name=man></size> <color=#FFF>");
				stringBuilder.Append(Texts.Instance.GetText("baseDamage"));
				stringBuilder.Append(":</color> ");
				if (num > 0)
				{
					stringBuilder.Append("+");
					stringBuilder.Append(num);
				}
				else
				{
					stringBuilder.Append(num);
				}
				stringBuilder.Append("\n");
			}
			if (damageItems != 0)
			{
				foreach (KeyValuePair<string, int> item in character.GetItemDamageDoneDictionary(damageType))
				{
					string[] array = item.Key.Split('_');
					if (num2 > 0)
					{
						stringBuilder.Append("\n");
					}
					stringBuilder.Append("<size=+4><sprite name=");
					if (array[1].ToLower() == "enchantment")
					{
						stringBuilder.Append("card");
					}
					else
					{
						stringBuilder.Append(array[1].ToLower());
					}
					stringBuilder.Append("></size> ");
					stringBuilder.Append("<color=#A9815D>");
					stringBuilder.Append(array[0]);
					stringBuilder.Append(":</color> ");
					if (item.Value > 0)
					{
						stringBuilder.Append("+");
						stringBuilder.Append(item.Value);
					}
					else
					{
						stringBuilder.Append(item.Value);
					}
					num2++;
				}
			}
			if (damageAura != 0)
			{
				foreach (KeyValuePair<string, int> item2 in character.GetAuraDamageDoneDictionary(damageType))
				{
					if (num2 > 0)
					{
						stringBuilder.Append("\n");
					}
					stringBuilder.Append("<size=+4><sprite name=");
					stringBuilder.Append(item2.Key);
					stringBuilder.Append("></size> ");
					if (item2.Value > 0)
					{
						stringBuilder.Append("<color=#5D82A8>");
					}
					else if (item2.Value < 0)
					{
						stringBuilder.Append("<color=#A85D6A>");
					}
					else
					{
						stringBuilder.Append("<color=#FFF>");
					}
					stringBuilder.Append(Functions.UppercaseFirst(item2.Key));
					stringBuilder.Append(":</color> ");
					if (item2.Value > 0)
					{
						stringBuilder.Append("+");
						stringBuilder.Append(item2.Value);
					}
					else
					{
						stringBuilder.Append(item2.Value);
					}
					num2++;
				}
			}
		}
		damagedonePop[resistanceIndex].text = stringBuilder.ToString();
	}

	private void DoPopupDamageTaken(int resistanceIndex, Enums.DamageType damageType, int damageAura)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		Dictionary<string, int> auraDamageTakenDictionary = character.GetAuraDamageTakenDictionary(damageType);
		int num2 = 0;
		foreach (KeyValuePair<string, int> item in auraDamageTakenDictionary)
		{
			if (num > 0)
			{
				stringBuilder.Append("\n");
			}
			stringBuilder.Append("<size=+4><sprite name=");
			stringBuilder.Append(item.Key);
			stringBuilder.Append("></size> ");
			if (item.Value > 0)
			{
				stringBuilder.Append("<color=#5D82A8>");
			}
			else if (item.Value < 0)
			{
				stringBuilder.Append("<color=#A85D6A>");
			}
			else
			{
				stringBuilder.Append("<color=#FFF>");
			}
			stringBuilder.Append(Functions.UppercaseFirst(item.Key));
			stringBuilder.Append(":</color> ");
			if (item.Value > 0)
			{
				stringBuilder.Append("+");
				stringBuilder.Append(item.Value);
			}
			else
			{
				stringBuilder.Append(item.Value);
			}
			num++;
			num2 += item.Value;
		}
		if (num2 != 0 || damageAura != 0)
		{
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append("<line-height=140%><size=+2><size=+4><sprite name=man></size> <color=#FFF>");
			stringBuilder2.Append(Texts.Instance.GetText("baseDT"));
			stringBuilder2.Append(":</color> ");
			stringBuilder2.Append(-1 * (num2 - damageAura));
			stringBuilder2.Append("</size></line-height>\n");
			stringBuilder.Insert(0, stringBuilder2.ToString());
		}
		if (stringBuilder.Length > 0)
		{
			stringBuilder.Insert(0, "<line-height=140%><size=+2>");
		}
		damagetakenPop[resistanceIndex].text = stringBuilder.ToString();
	}

	private void DoPopupGeneral(Dictionary<string, int> itemValues, Dictionary<string, int> auraValues, PopupText popText, string type = "percent", bool showBase = false, int valueBase = 0)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if ((itemValues != null && itemValues.Count > 0) || (auraValues != null && auraValues.Count > 0))
		{
			if (showBase && valueBase != 0)
			{
				stringBuilder.Append("<line-height=140%><size=+2>");
				stringBuilder.Append("<size=+4><sprite name=man></size> <color=#FFF>");
				stringBuilder.Append(Texts.Instance.GetText("baseValue"));
				stringBuilder.Append(":</color> ");
				stringBuilder.Append(valueBase);
				if (type == "percent")
				{
					if (Functions.SpaceBeforePercentSign())
					{
						stringBuilder.Append(" ");
					}
					stringBuilder.Append("%");
				}
				stringBuilder.Append("\n");
			}
			int num = 0;
			if (!showBase)
			{
				stringBuilder.Append("<line-height=140%><size=+2>");
			}
			if (itemValues != null)
			{
				foreach (KeyValuePair<string, int> itemValue in itemValues)
				{
					string[] array = itemValue.Key.Split('_');
					if (num > 0)
					{
						stringBuilder.Append("\n");
					}
					stringBuilder.Append("<size=+4><sprite name=");
					if (array[1].ToLower() == "enchantment")
					{
						stringBuilder.Append("card");
					}
					else
					{
						stringBuilder.Append(array[1].ToLower());
					}
					stringBuilder.Append("></size> ");
					stringBuilder.Append("<color=#A9815D>");
					stringBuilder.Append(array[0]);
					stringBuilder.Append(":</color> ");
					if (itemValue.Value > 0)
					{
						stringBuilder.Append("+");
						stringBuilder.Append(itemValue.Value);
					}
					else
					{
						stringBuilder.Append(itemValue.Value);
					}
					if (type == "percent")
					{
						if (Functions.SpaceBeforePercentSign())
						{
							stringBuilder.Append(" ");
						}
						stringBuilder.Append("%");
					}
					num++;
				}
			}
			if (auraValues != null)
			{
				foreach (KeyValuePair<string, int> auraValue in auraValues)
				{
					if (num > 0)
					{
						stringBuilder.Append("\n");
					}
					stringBuilder.Append("<size=+4><sprite name=");
					stringBuilder.Append(auraValue.Key);
					stringBuilder.Append("></size> ");
					if (auraValue.Value > 0)
					{
						stringBuilder.Append("<color=#5D82A8>");
					}
					else if (auraValue.Value < 0)
					{
						stringBuilder.Append("<color=#A85D6A>");
					}
					else
					{
						stringBuilder.Append("<color=#FFF>");
					}
					stringBuilder.Append(Functions.UppercaseFirst(auraValue.Key));
					stringBuilder.Append(":</color> ");
					if (auraValue.Value > 0)
					{
						stringBuilder.Append("+");
						stringBuilder.Append(auraValue.Value);
					}
					else
					{
						stringBuilder.Append(auraValue.Value);
					}
					if (type == "percent")
					{
						if (Functions.SpaceBeforePercentSign())
						{
							stringBuilder.Append(" ");
						}
						stringBuilder.Append("%");
					}
					num++;
				}
			}
		}
		popText.text = stringBuilder.ToString();
	}

	private string FormatResistance(int value, int mod)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (mod > 0)
		{
			stringBuilder.Append("<color=");
			stringBuilder.Append(colorGreen);
			stringBuilder.Append(">");
		}
		else if (mod < 0)
		{
			stringBuilder.Append("<color=");
			stringBuilder.Append(colorRed);
			stringBuilder.Append(">");
		}
		stringBuilder.Append(value);
		if (Functions.SpaceBeforePercentSign())
		{
			stringBuilder.Append(" ");
		}
		stringBuilder.Append("%");
		if (mod != 0)
		{
			stringBuilder.Append("</color>");
		}
		return stringBuilder.ToString();
	}

	private string FormatSum(int value, int mod, bool notLessThanZero = true)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (mod > 0)
		{
			stringBuilder.Append("<color=");
			stringBuilder.Append(colorGreen);
			stringBuilder.Append(">");
		}
		else if (mod < 0)
		{
			stringBuilder.Append("<color=");
			stringBuilder.Append(colorRed);
			stringBuilder.Append(">");
		}
		if (notLessThanZero && value < 0)
		{
			value = 0;
		}
		if (value > 0)
		{
			stringBuilder.Append("+");
			stringBuilder.Append(value);
		}
		else if (value < 0)
		{
			stringBuilder.Append(value);
		}
		else
		{
			stringBuilder.Append("--");
		}
		if (mod != 0)
		{
			stringBuilder.Append("</color>");
		}
		return stringBuilder.ToString();
	}

	private string FormatTaken(int value)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (value > 0)
		{
			stringBuilder.Append("<color=");
			stringBuilder.Append(colorRed);
			stringBuilder.Append(">");
			stringBuilder.Append("+");
		}
		if (value == 0)
		{
			stringBuilder.Append("--");
		}
		else
		{
			stringBuilder.Append(value);
		}
		if (value > 0)
		{
			stringBuilder.Append("</color>");
		}
		return stringBuilder.ToString();
	}

	private string FormatPercent(int value, int mod)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (mod > 0)
		{
			stringBuilder.Append("<color=");
			stringBuilder.Append(colorGreen);
			stringBuilder.Append(">");
			stringBuilder.Append("+");
		}
		else if (mod < 0)
		{
			stringBuilder.Append("<color=");
			stringBuilder.Append(colorRed);
			stringBuilder.Append(">");
		}
		else
		{
			stringBuilder.Append("+");
		}
		stringBuilder.Append(value);
		if (Functions.SpaceBeforePercentSign())
		{
			stringBuilder.Append(" ");
		}
		stringBuilder.Append("%");
		if (mod != 0)
		{
			stringBuilder.Append("</color>");
		}
		return stringBuilder.ToString();
	}

	private string GetTextDT(string type)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<size=4><sprite name=");
		stringBuilder.Append(type);
		stringBuilder.Append("></size> ");
		stringBuilder.Append(Texts.Instance.GetText(type));
		return stringBuilder.ToString();
	}

	public bool IsActive()
	{
		return base.gameObject.activeSelf;
	}
}
