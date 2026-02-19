using System;
using System.Collections.Generic;
using System.Text;

public static class Perk
{
	public static int GetMaxHealth(string _perk)
	{
		PerkData perkData = Globals.Instance.GetPerkData(_perk);
		if (perkData == null)
		{
			return 0;
		}
		return perkData.MaxHealth;
	}

	public static int GetSpeed(string _perk)
	{
		PerkData perkData = Globals.Instance.GetPerkData(_perk);
		if (perkData == null)
		{
			return 0;
		}
		return perkData.SpeedQuantity;
	}

	public static int GetEnergyBegin(string _perk)
	{
		PerkData perkData = Globals.Instance.GetPerkData(_perk);
		if (perkData == null)
		{
			return 0;
		}
		return perkData.EnergyBegin;
	}

	public static int GetDamageBonus(string _perk, Enums.DamageType _dmgType)
	{
		PerkData perkData = Globals.Instance.GetPerkData(_perk);
		if (perkData == null)
		{
			return 0;
		}
		if (perkData.DamageFlatBonus == _dmgType || perkData.DamageFlatBonus == Enums.DamageType.All)
		{
			return perkData.DamageFlatBonusValue;
		}
		return 0;
	}

	public static int GetHealBonus(string _perk)
	{
		PerkData perkData = Globals.Instance.GetPerkData(_perk);
		if (perkData == null)
		{
			return 0;
		}
		return perkData.HealQuantity;
	}

	public static int GetAuraCurseBonus(string _perk, string _auraCurse)
	{
		PerkData perkData = Globals.Instance.GetPerkData(_perk);
		if (perkData == null)
		{
			return 0;
		}
		if (perkData.AuracurseBonus.Id == _auraCurse)
		{
			return perkData.AuracurseBonusValue;
		}
		return 0;
	}

	public static Dictionary<string, int> GetAuraCurseBonusDict(string _hero)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		List<string> heroPerks = PlayerManager.Instance.GetHeroPerks(_hero);
		if (heroPerks != null)
		{
			for (int i = 0; i < heroPerks.Count; i++)
			{
				PerkData perkData = Globals.Instance.GetPerkData(heroPerks[i]);
				if (perkData != null && perkData.AuracurseBonus != null)
				{
					if (dictionary.ContainsKey(perkData.AuracurseBonus.Id))
					{
						dictionary[perkData.AuracurseBonus.Id] += perkData.AuracurseBonusValue;
					}
					else
					{
						dictionary.Add(perkData.AuracurseBonus.Id, perkData.AuracurseBonusValue);
					}
				}
			}
		}
		return dictionary;
	}

	public static int GetResistBonus(string _perk, Enums.DamageType _dmgType)
	{
		PerkData perkData = Globals.Instance.GetPerkData(_perk);
		if (perkData == null)
		{
			return 0;
		}
		if (perkData.ResistModified == _dmgType || perkData.ResistModified == Enums.DamageType.All)
		{
			return perkData.ResistModifiedValue;
		}
		return 0;
	}

	public static int GetCurrencyBonus(string _perk)
	{
		PerkData perkData = Globals.Instance.GetPerkData(_perk);
		if (perkData == null)
		{
			return 0;
		}
		return perkData.AdditionalCurrency;
	}

	public static int GetShardsBonus(string _perk)
	{
		PerkData perkData = Globals.Instance.GetPerkData(_perk);
		if (perkData == null)
		{
			return 0;
		}
		return perkData.AdditionalShards;
	}

	public static int GetPointsNeededForIndex(int _index)
	{
		return _index switch
		{
			0 => 0, 
			1 => 3, 
			2 => 6, 
			3 => 10, 
			4 => 15, 
			_ => 20, 
		};
	}

	public static string RomanLevel(int level)
	{
		return level switch
		{
			1 => "I", 
			2 => "II", 
			3 => "III", 
			4 => "IV", 
			5 => "V", 
			6 => "VI", 
			7 => "VII", 
			_ => "", 
		};
	}

	public static string PerkDescription(PerkData perkData, bool doPopup = false, int _index = -1, int pointsAvailable = -1, bool enabled = false, bool active = false)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (perkData.MaxHealth > 0)
		{
			stringBuilder.Append(string.Format(Functions.UppercaseFirst(Texts.Instance.GetText("itemMaxHp")), perkData.IconTextValue));
		}
		else if (perkData.AdditionalCurrency > 0)
		{
			stringBuilder.Append(string.Format(Functions.UppercaseFirst(Texts.Instance.GetText("itemInitialCurrency")), perkData.IconTextValue));
		}
		else if (perkData.SpeedQuantity > 0)
		{
			stringBuilder.Append(string.Format(Functions.UppercaseFirst(Texts.Instance.GetText("itemSpeed")), perkData.IconTextValue));
		}
		else if (perkData.HealQuantity > 0)
		{
			stringBuilder.Append(string.Format(Functions.UppercaseFirst(Texts.Instance.GetText("itemHealDone").Replace("<c>", "")), perkData.IconTextValue));
		}
		else if (perkData.EnergyBegin > 0)
		{
			stringBuilder.Append(string.Format(Functions.UppercaseFirst(Texts.Instance.GetText("itemInitialEnergy")), perkData.IconTextValue));
		}
		else if (perkData.AuracurseBonus != null)
		{
			stringBuilder.Append(string.Format(Functions.UppercaseFirst(Texts.Instance.GetText("perkAuraDescription")), perkData.IconTextValue));
		}
		else if (perkData.ResistModified == Enums.DamageType.All)
		{
			stringBuilder.Append(string.Format(Functions.UppercaseFirst(Texts.Instance.GetText("itemAllResistances")), perkData.IconTextValue));
		}
		else if (perkData.DamageFlatBonus == Enums.DamageType.All)
		{
			stringBuilder.Append(string.Format(Functions.UppercaseFirst(Texts.Instance.GetText("itemAllDamages")), perkData.IconTextValue));
		}
		else if (perkData.DamageFlatBonus == Enums.DamageType.All)
		{
			stringBuilder.Append(string.Format(Functions.UppercaseFirst(Texts.Instance.GetText("itemAllDamages")), perkData.IconTextValue));
		}
		else if (perkData.DamageFlatBonus != Enums.DamageType.None && perkData.DamageFlatBonus != Enums.DamageType.All)
		{
			stringBuilder.Append(string.Format(Functions.UppercaseFirst(Texts.Instance.GetText("itemSingleDamage")), Enum.GetName(typeof(Enums.DamageType), perkData.DamageFlatBonus), perkData.IconTextValue));
		}
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			if (stringBuilder.Length > 0 && doPopup)
			{
				if (!enabled)
				{
					stringBuilder.Append("<br></size><line-height=5><br><color=#FF6666>");
					stringBuilder.Append(string.Format(Texts.Instance.GetText("requiredPoints"), GetPointsNeededForIndex(_index)));
				}
				else if (!active)
				{
					if (pointsAvailable > 0)
					{
						stringBuilder.Append("<br></size><line-height=5><br><color=#66FF66>");
						stringBuilder.Append(Texts.Instance.GetText("rankPerkPress"));
					}
					else
					{
						stringBuilder.Append("<br></size><line-height=5><br><color=#FF6666>");
						stringBuilder.Append(Texts.Instance.GetText("rankPerkNotEnough"));
						enabled = false;
					}
				}
				stringBuilder.Insert(0, "<color=#FFF><size=+5>");
			}
		}
		else if (stringBuilder.Length > 0 && doPopup)
		{
			stringBuilder.Insert(0, "<color=#FFF><size=+5>");
		}
		if (stringBuilder.Length > 0)
		{
			stringBuilder.Replace("<c>", "");
			stringBuilder.Replace("</c>", "");
			return stringBuilder.ToString();
		}
		return "";
	}
}
