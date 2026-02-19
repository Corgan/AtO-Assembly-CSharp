using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameUtils
{
	private static readonly string[] Suffixes = new string[3] { "", "a", "b" };

	public static readonly string[] MasterReweaverKeys = Suffixes.Select((string suffix) => "masterreweaver" + suffix).ToArray();

	public static int GetMaxPlaceholderFormattedStringIndex(string text)
	{
		int num = -1;
		for (int i = 0; i < text.Length; i++)
		{
			if (text[i] != '{')
			{
				continue;
			}
			int num2 = i + 1;
			int num3 = text.IndexOf('}', num2);
			if (num3 > num2)
			{
				if (int.TryParse(text.Substring(num2, num3 - num2), out var result))
				{
					num = Math.Max(num, result);
				}
				i = num3;
			}
		}
		return num;
	}

	public static void ConvertAllDebuffsIntoSelectedCurse(CardData cardActive, Character target)
	{
		int debuffsChargesTotal = GetDebuffsChargesTotal(target);
		if (debuffsChargesTotal > 0)
		{
			AuraCurseData curse = cardActive.Curse;
			float num = (Mathf.Approximately(cardActive.SpecialValueModifierGlobal, 0f) ? 1f : cardActive.SpecialValueModifierGlobal);
			if (!(curse == null))
			{
				cardActive.CurseCharges = (int)((float)debuffsChargesTotal * num);
			}
		}
		else
		{
			cardActive.CurseCharges = 0;
		}
	}

	public static void ConvertReceivedCursesToDamage(ItemData enchantItem, int curseCharges)
	{
		if (curseCharges > 0)
		{
			enchantItem.DamageToTargetType1 = enchantItem.ConvertReceivedDebuffsIntoDamage;
			enchantItem.DamageToTarget1 = curseCharges;
		}
		else
		{
			enchantItem.DamageToTargetType1 = Enums.DamageType.None;
			enchantItem.DamageToTarget1 = 0;
		}
	}

	public static void ConvertReceivedDebuffsIntoCurseForEnchant(ItemData enchantItem, int curseCharges)
	{
		if (curseCharges > 0)
		{
			if (!(enchantItem.AuracurseGain1 == null))
			{
				enchantItem.AuracurseGainValue1 = curseCharges;
			}
		}
		else
		{
			enchantItem.AuracurseGainValue1 = 0;
		}
	}

	private static int GetDebuffsChargesTotal(Character target)
	{
		int num = 0;
		foreach (Aura aura in target.AuraList)
		{
			if (aura != null && aura.ACData != null && !aura.ACData.IsAura && aura.GetCharges() > 0)
			{
				num += aura.GetCharges();
			}
		}
		return num;
	}

	public static List<ItemData> GetEnchantments(Character target)
	{
		List<ItemData> list = new List<ItemData>();
		if (!string.IsNullOrEmpty(target.Enchantment))
		{
			list.Add(Globals.Instance.GetItemData(target.Enchantment));
		}
		if (!string.IsNullOrEmpty(target.Enchantment2))
		{
			list.Add(Globals.Instance.GetItemData(target.Enchantment2));
		}
		if (!string.IsNullOrEmpty(target.Enchantment3))
		{
			list.Add(Globals.Instance.GetItemData(target.Enchantment3));
		}
		return list;
	}

	public static CharacterItem GetCharacterItem(Character character)
	{
		if (!character.IsHero)
		{
			return character.NPCItem;
		}
		return character.HeroItem;
	}
}
