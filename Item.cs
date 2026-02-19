using System;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
	public bool DoItem(Enums.EventActivation _theEvent, CardData _cardData, string _item, Character _character, Character _target, int _auxInt, string _auxString, int order, CardData castedCard, bool onlyCheckItemActivation = false, bool forceActivate = false)
	{
		if (MatchManager.Instance == null)
		{
			return false;
		}
		switch (_item)
		{
		case "surprisebox":
			if (!onlyCheckItemActivation)
			{
				surprisebox(_character, isRare: false, _cardData.CardName);
			}
			return true;
		case "surpriseboxrare":
			if (!onlyCheckItemActivation)
			{
				surprisebox(_character, isRare: true, _cardData.CardName);
			}
			return true;
		case "surprisegiftbox":
			if (!onlyCheckItemActivation)
			{
				surprisegiftbox(_character, isRare: false, _cardData.CardName);
			}
			return true;
		case "surprisegiftboxrare":
			if (!onlyCheckItemActivation)
			{
				surprisegiftbox(_character, isRare: true, _cardData.CardName);
			}
			return true;
		default:
		{
			int num = 0;
			Enums.EventActivation theEvent = _theEvent;
			num = _auxInt;
			ItemData itemData = ((!(_cardData?.Item != null)) ? _cardData?.ItemEnchantment : _cardData.Item);
			if (itemData == null)
			{
				return false;
			}
			string cardName = _cardData.CardName;
			string itemType = Enum.GetName(typeof(Enums.CardType), _cardData.CardType).ToLower();
			if ((_theEvent == Enums.EventActivation.PreFinishCast || _theEvent == Enums.EventActivation.FinishCast || _theEvent == Enums.EventActivation.FinishFinishCast) && MatchManager.Instance != null && MatchManager.Instance.IsBeginTournPhase)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(cardName + " Broke because we have not began the turn phase", "item");
				}
				return false;
			}
			if ((_theEvent == Enums.EventActivation.PreFinishCast || _theEvent == Enums.EventActivation.FinishCast || _theEvent == Enums.EventActivation.FinishFinishCast) && castedCard != null && (castedCard.AutoplayDraw || castedCard.AutoplayEndTurn) && (castedCard.CardClass == Enums.CardClass.Injury || castedCard.CardClass == Enums.CardClass.Boon || castedCard.CardClass == Enums.CardClass.Monster))
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(cardName + " Broke because just casted an autoplay boon/injury/monster card " + castedCard.Id, "item");
				}
				return false;
			}
			if ((_theEvent == Enums.EventActivation.PreFinishCast || _theEvent == Enums.EventActivation.FinishCast || _theEvent == Enums.EventActivation.FinishFinishCast) && castedCard != null && castedCard.ItemEnchantment != null && !(itemData.Id == "endlessbag") && !(itemData.Id == "endlessbagrare") && !(itemData.Id == "mirrorofkalandra") && !(itemData.Id == "mirrorofkalandrarare") && !(itemData.Id == "manaloop") && !(itemData.Id == "manalooprare"))
			{
				if (!castedCard.ItemEnchantment.CastEnchantmentOnFinishSelfCast && castedCard.ItemEnchantment.Id == _cardData.Id)
				{
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD(cardName + " Broke because Item Enchantment just casted", "item");
					}
					return false;
				}
				if (castedCard.ItemEnchantment.CastEnchantmentOnFinishSelfCast && castedCard.ItemEnchantment.Id != _cardData.Id)
				{
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD(cardName + " Broke because this is not the Item Enchantment card", "item");
					}
					return false;
				}
			}
			if (_theEvent == Enums.EventActivation.DamagedSecondary)
			{
				if (!(itemData.LowerOrEqualPercentHP > 0f) || !(itemData.LowerOrEqualPercentHP < 100f))
				{
					return false;
				}
				theEvent = Enums.EventActivation.Damaged;
			}
			if (itemData.UsedEnergy && MatchManager.Instance.energyJustWastedByHero < 1)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(cardName + " Broke because not used energy", "item");
				}
				return false;
			}
			if (itemData.ExactRound > 0 && MatchManager.Instance.GetCurrentRound() != itemData.ExactRound)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(cardName + " Broke because Not exact round", "item");
				}
				return false;
			}
			if (itemData.RoundCycle > 0 && MatchManager.Instance.GetCurrentRound() % itemData.RoundCycle != 0 && !forceActivate)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(cardName + " Broke because Not round cycle", "item");
				}
				return false;
			}
			bool flag = true;
			if (itemData.AuraCurseSetted != null)
			{
				AuraCurseData auraCurseData = Globals.Instance.GetAuraCurseData(_auxString);
				if ((bool)auraCurseData && auraCurseData.Id != itemData.AuraCurseSetted.Id)
				{
					flag = false;
				}
			}
			if (itemData.AuraCurseSetted2 != null && !flag)
			{
				AuraCurseData auraCurseData2 = Globals.Instance.GetAuraCurseData(_auxString);
				flag = ((!auraCurseData2 || !(auraCurseData2.Id != itemData.AuraCurseSetted2.Id)) ? true : false);
			}
			if (itemData.AuraCurseSetted3 != null && !flag)
			{
				AuraCurseData auraCurseData3 = Globals.Instance.GetAuraCurseData(_auxString);
				flag = ((!auraCurseData3 || !(auraCurseData3.Id != itemData.AuraCurseSetted3.Id)) ? true : false);
			}
			if (!flag)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(cardName + " Broke because Not aura setted", "item");
				}
				return false;
			}
			if (itemData.CastedCardType != Enums.CardType.None)
			{
				if (!(castedCard != null))
				{
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD(cardName + " Broke because  Not casted card", "item");
					}
					return false;
				}
				if (!castedCard.GetCardTypes().Contains(itemData.CastedCardType))
				{
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD(cardName + " Broke because Not casted card type", "item");
					}
					return false;
				}
			}
			int cardsWaitingForReset = MatchManager.Instance.CardsWaitingForReset;
			int num2 = MatchManager.Instance.CountHeroHand(_character.HeroIndex);
			if (itemData.EmptyHand)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("isBeginTournPhase -> " + MatchManager.Instance.IsBeginTournPhase, "item");
				}
				if (MatchManager.Instance.IsBeginTournPhase)
				{
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD(cardName + " Broke because is begin tourn phase", "item");
					}
					return false;
				}
				bool flag2 = true;
				if (num2 > 0)
				{
					flag2 = false;
				}
				if (cardsWaitingForReset > 0)
				{
					flag2 = false;
				}
				if (num2 == 0 && cardsWaitingForReset == 1 && castedCard != null && castedCard.CardClass == Enums.CardClass.Injury)
				{
					flag2 = true;
				}
				if (!flag2)
				{
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD(cardName + " Broke because Not empty hand valid item");
					}
					return false;
				}
			}
			if (num2 == 10 && ((itemData.CardNum > 0 && itemData.CardPlace == Enums.CardPlace.Hand) || itemData.DrawCards > 0))
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(cardName + " Broke because player at max cards", "item");
				}
				return false;
			}
			if (itemData.TimesPerTurn > 0 && !MatchManager.Instance.CanExecuteItemInThisTurn(_character.Id, itemData.Id, itemData.TimesPerTurn))
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(cardName + " Broke because TimesPerTurn", "item");
				}
				return false;
			}
			if (itemData.TimesPerCombat > 0 && !MatchManager.Instance.CanExecuteItemInThisCombat(_character.Id, itemData.Id, itemData.TimesPerCombat))
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(cardName + " Broke because TimesPerCombat", "item");
				}
				return false;
			}
			if (itemData.Activation == Enums.EventActivation.Damaged && _character == _target)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(cardName + " Broke because Caster equal to Target", "item");
				}
				return false;
			}
			if (itemData.Activation == Enums.EventActivation.Damaged && _character.GetHpPercent() > itemData.LowerOrEqualPercentHP)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(cardName + " Broke because LowerOrEqualPercentHP", "item");
				}
				return false;
			}
			if (itemData.ConvertReceivedDebuffsIntoDamage != Enums.DamageType.None)
			{
				GameUtils.ConvertReceivedCursesToDamage(itemData, num);
			}
			if (onlyCheckItemActivation)
			{
				return true;
			}
			if (itemData.TimesPerTurn > 0)
			{
				MatchManager.Instance.ItemExecuteForThisTurn(_character.Id, itemData.Id, itemData.TimesPerTurn, itemType);
			}
			if (itemData.TimesPerCombat > 0)
			{
				MatchManager.Instance.ItemExecuteForThisCombat(_character.Id, itemData.Id, itemData.TimesPerCombat, itemType);
				ShowCombatText(itemType, cardName, _character, MatchManager.Instance.ItemExecutedInThisCombat(_character.Id, itemData.Id), itemData.TimesPerCombat);
			}
			MatchManager.Instance.itemTimeout[order] = 0f;
			DoItemData(_target, cardName, num, _cardData, itemType, itemData, _character, order, castedCard, theEvent);
			return true;
		}
		}
	}

	public bool DoItemOnReceive(Enums.EventActivation theEvent, CardData cardData, Character character, Character target, int auxInt, string auxString, int order, CardData castedCard, ItemData itemData)
	{
		if (itemData.ConvertReceivedDebuffsIntoDamage != Enums.DamageType.None)
		{
			GameUtils.ConvertReceivedCursesToDamage(itemData, auxInt);
		}
		if (itemData.ConvertReceivedDebuffsIntoCurse)
		{
			GameUtils.ConvertReceivedDebuffsIntoCurseForEnchant(itemData, auxInt);
		}
		DoItemData(target, itemData.name, auxInt, cardData, "Enchantment".ToLower(), itemData, character, order, castedCard, theEvent);
		return true;
	}

	private void DoItemData(Character target, string itemName, int auxInt, CardData cardItem, string itemType, ItemData itemData, Character character, int order, CardData castedCard = null, Enums.EventActivation theEvent = Enums.EventActivation.None)
	{
		string text = "";
		if (castedCard != null)
		{
			text = castedCard.Id;
		}
		int charges = -1;
		int num = -1;
		bool flag = false;
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD(itemName + " DoItemData", "item");
		}
		if (MatchManager.Instance != null)
		{
			if (itemData.DestroyAfterUses > 0 && itemData.Activation != Enums.EventActivation.Killed)
			{
				num = itemData.DestroyAfterUses - 1;
				charges = MatchManager.Instance.EnchantmentExecutedTimes(character.Id, itemData.Id);
				if (charges <= num)
				{
					MatchManager.Instance.EnchantmentExecute(character.Id, itemData.Id);
					character.EnchantmentExecute(itemData.Id);
				}
				if (charges == num)
				{
					if (character.IsHero)
					{
						AtOManager.Instance.RemoveItemFromHero(_isHero: true, character.HeroIndex, "", itemData.Id);
					}
					else
					{
						AtOManager.Instance.RemoveItemFromHero(_isHero: false, character.NPCIndex, "", itemData.Id);
					}
					MatchManager.Instance.RedrawCardsDescriptionPrecalculated();
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("Destroyed because DestroyAfterUses", "item");
					}
				}
				else if (charges > num)
				{
					return;
				}
				charges++;
			}
			if (!itemData.IsEnchantment && itemData.DrawCards > 0)
			{
				int num2 = itemData.DrawCards;
				if (itemData.DrawMultiplyByEnergyUsed)
				{
					num2 *= MatchManager.Instance.energyJustWastedByHero;
				}
				if (num2 > 0)
				{
					if (GameManager.Instance.IsMultiplayer())
					{
						MatchManager.Instance.itemTimeout[order] = 0.5f;
					}
					else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
					{
						MatchManager.Instance.itemTimeout[order] = 0.5f;
					}
					else
					{
						MatchManager.Instance.itemTimeout[order] = 0.4f;
					}
					if (MatchManager.Instance.CountHeroDeck() == 0)
					{
						MatchManager.Instance.itemTimeout[order] += 0.7f;
					}
					MatchManager.Instance.NewCard(num2, Enums.CardFrom.Deck);
				}
			}
			if (itemData.AddVanishToDeck)
			{
				int index = (character.IsHero ? character.HeroIndex : character.NPCIndex);
				MatchManager.Instance.AddVanishToDeck(index, character.IsHero);
			}
		}
		List<Character> list = new List<Character>();
		if (itemData.ItemTarget == Enums.ItemTarget.Self || itemData.ItemTarget == Enums.ItemTarget.SelfEnemy)
		{
			list.Add(character);
		}
		else if (itemData.ItemTarget == Enums.ItemTarget.RandomHero)
		{
			list.Add(GetRandomHero());
		}
		else if (itemData.ItemTarget == Enums.ItemTarget.RandomEnemy)
		{
			list.Add(GetRandomNPC(itemData.DontTargetBoss));
		}
		else if (itemData.ItemTarget == Enums.ItemTarget.Random)
		{
			list.Add(GetRandomCharacter(itemData.DontTargetBoss));
		}
		else if (itemData.ItemTarget == Enums.ItemTarget.FrontEnemy)
		{
			if (GetFrontNPC(itemData.DontTargetBoss) != null)
			{
				list.Add(GetFrontNPC(itemData.DontTargetBoss));
			}
		}
		else if (itemData.ItemTarget == Enums.ItemTarget.AllHero)
		{
			list = GetAllHeroList();
		}
		else if (itemData.ItemTarget == Enums.ItemTarget.AllEnemy)
		{
			list = GetAllNPCList();
		}
		else if (itemData.ItemTarget == Enums.ItemTarget.CurrentTarget)
		{
			list.Add(target);
		}
		else if (itemData.ItemTarget == Enums.ItemTarget.HighestFlatHpHero)
		{
			list.Add(GetFlatHPCharacter(highestHp: true, isHero: true));
		}
		else if (itemData.ItemTarget == Enums.ItemTarget.HighestFlatHpEnemy)
		{
			list.Add(GetFlatHPCharacter(highestHp: true, isHero: false));
		}
		else if (itemData.ItemTarget == Enums.ItemTarget.LowestFlatHpHero)
		{
			list.Add(GetFlatHPCharacter(highestHp: false, isHero: true));
		}
		else if (itemData.ItemTarget == Enums.ItemTarget.LowestFlatHpEnemy)
		{
			list.Add(GetFlatHPCharacter(highestHp: false, isHero: false));
		}
		if (list.Count == 0)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD(itemName + " Break item execution: No target for " + itemData.ItemTarget, "item");
			}
			return;
		}
		Hero[] teamHero;
		if (theEvent == Enums.EventActivation.Damage)
		{
			if (itemData.healSelfTeamPerDamageDonePercent)
			{
				teamHero = MatchManager.Instance.GetTeamHero();
				foreach (Hero hero in teamHero)
				{
					if (hero != null && hero.Alive)
					{
						hero.ModifyHp(Functions.FuncRoundToInt((float)auxInt * itemData.healSelfPerDamageDonePercent * 0.01f));
					}
				}
			}
			else if (target != null)
			{
				foreach (Character item in list)
				{
					item.ModifyHp(Functions.FuncRoundToInt((float)auxInt * itemData.healSelfPerDamageDonePercent * 0.01f));
				}
			}
		}
		if (itemData.DamageToTarget1 > 0 || itemData.DttSpecialValues1.Use)
		{
			int num3 = 0;
			num3 = ((!character.IsHero || cardItem.CardClass != Enums.CardClass.Monster) ? character.DamageWithCharacterBonus(itemData.GetModifiedValue(itemData.DamageToTarget1, itemData.DttSpecialValues1), itemData.DamageToTargetType1, Enums.CardClass.Item) : itemData.GetModifiedValue(itemData.DamageToTarget1, itemData.DttSpecialValues1));
			if (itemData.DttMultiplyByEnergyUsed)
			{
				num3 *= MatchManager.Instance.energyJustWastedByHero;
			}
			if (num3 > -1)
			{
				for (int j = 0; j < list.Count; j++)
				{
					Character character2 = list[j];
					if (itemData.DontTargetBoss && character2.NpcData != null && character2.NpcData.IsBoss)
					{
						break;
					}
					int num4 = num3;
					if (character2 != null)
					{
						int num5 = character2.IncreasedCursedDamagePerStack(itemData.DamageToTargetType1);
						num4 += num5;
						character2.IndirectDamage(itemData.DamageToTargetType1, num4, character);
					}
				}
			}
			flag = true;
		}
		if (itemData.DamageToTarget2 > 0 || itemData.DttSpecialValues2.Use)
		{
			int num6 = 0;
			num6 = ((!character.IsHero || cardItem.CardClass != Enums.CardClass.Monster) ? character.DamageWithCharacterBonus(itemData.GetModifiedValue(itemData.DamageToTarget2, itemData.DttSpecialValues2), itemData.DamageToTargetType2, Enums.CardClass.Item) : itemData.GetModifiedValue(itemData.DamageToTarget2, itemData.DttSpecialValues2));
			if (itemData.DttMultiplyByEnergyUsed)
			{
				num6 *= MatchManager.Instance.energyJustWastedByHero;
			}
			if (num6 > -1)
			{
				for (int k = 0; k < list.Count; k++)
				{
					Character character3 = list[k];
					if (itemData.DontTargetBoss && character3.NpcData != null && character3.NpcData.IsBoss)
					{
						break;
					}
					int num7 = num6;
					if (character3 != null)
					{
						int num8 = character3.IncreasedCursedDamagePerStack(itemData.DamageToTargetType2);
						num7 += num8;
						character3.IndirectDamage(itemData.DamageToTargetType2, num7, character);
					}
				}
			}
			flag = true;
		}
		int num9 = 0;
		if (itemData.AuracurseBonusValue1 != 0)
		{
			character.ModifyAuraCurseQuantity(cost: (itemData.AuraCurseNumForOneEvent <= 0) ? itemData.AuracurseBonusValue1 : (Functions.FuncRoundToInt(auxInt / itemData.AuraCurseNumForOneEvent) * itemData.AuracurseBonusValue1), id: itemData.AuracurseBonus1.Id);
		}
		if (itemData.AuracurseBonusValue2 != 0)
		{
			character.ModifyAuraCurseQuantity(cost: (itemData.AuraCurseNumForOneEvent <= 0) ? itemData.AuracurseBonusValue2 : (Functions.FuncRoundToInt(auxInt / itemData.AuraCurseNumForOneEvent) * itemData.AuracurseBonusValue2), id: itemData.AuracurseBonus2.Id);
		}
		bool flag2 = false;
		bool flag3 = false;
		if (itemData.ChanceToDispel > 0)
		{
			int num10 = 0;
			if (itemData.ChanceToDispel < 100)
			{
				num10 = MatchManager.Instance.GetRandomIntRange(0, 100, "item");
			}
			Character character4 = character;
			if (itemData.ItemTarget == Enums.ItemTarget.CurrentTarget && target != null)
			{
				character4 = target;
			}
			if (num10 < itemData.ChanceToDispel)
			{
				character4.HealCurses(itemData.ChanceToDispelNum);
				MatchManager.Instance.ItemActivationDisplay(itemType);
			}
			flag = true;
		}
		if (itemData.ChanceToPurge > 0)
		{
			int num11 = 0;
			if (itemData.ChanceToPurge < 100)
			{
				num11 = MatchManager.Instance.GetRandomIntRange(0, 100, "item");
			}
			Character character5 = character;
			if (itemData.ItemTarget == Enums.ItemTarget.CurrentTarget && target != null)
			{
				character5 = target;
			}
			if (num11 < itemData.ChanceToPurge)
			{
				character5.DispelAuras(itemData.ChanceToPurgeNum);
				MatchManager.Instance.ItemActivationDisplay(itemType);
			}
			flag = true;
		}
		if (itemData.ChanceToDispelSelf > 0 && itemData.ChanceToDispelNumSelf > 0)
		{
			int num12 = 0;
			if (itemData.ChanceToDispelSelf < 100)
			{
				num12 = MatchManager.Instance.GetRandomIntRange(0, 100, "item");
			}
			if (num12 < itemData.ChanceToDispelSelf)
			{
				character.HealCurses(itemData.ChanceToDispelNumSelf);
				MatchManager.Instance.ItemActivationDisplay(itemType);
			}
			flag = true;
		}
		int num13 = 0;
		if ((bool)itemData.AuraCurseSetted)
		{
			num13++;
			if ((bool)itemData.AuraCurseSetted2)
			{
				num13++;
				if ((bool)itemData.AuraCurseSetted3)
				{
					num13++;
				}
			}
		}
		for (int l = 0; l < list.Count; l++)
		{
			Character character6 = list[l];
			if (itemData.DontTargetBoss && character6 != null && character6.NpcData != null && character6.NpcData.IsBoss)
			{
				break;
			}
			if (character6 == null)
			{
				continue;
			}
			Character target2 = target;
			if (theEvent == Enums.EventActivation.Damaged && !character.IsHero)
			{
				target2 = character;
			}
			bool flag4;
			bool flag5;
			if (itemData.AuracurseGain1 != null)
			{
				num9 = ((itemData.AuraCurseNumForOneEvent <= 0) ? itemData.GetModifiedValue(itemData.AuracurseGainValue1, itemData.AuracurseGain1SpecialValue, itemData, target2) : (Functions.FuncRoundToInt(auxInt / itemData.AuraCurseNumForOneEvent) * itemData.GetModifiedValue(itemData.AuracurseGainValue1, itemData.AuracurseGain1SpecialValue, itemData, target2)));
				if (itemData.Acg1MultiplyByEnergyUsed)
				{
					num9 *= MatchManager.Instance.energyJustWastedByHero;
				}
				if (itemType == "corruption")
				{
					character6.SetAuraTrait(null, itemData.AuracurseGain1.Id, num9);
					goto IL_0a68;
				}
				flag4 = character.IsHero;
				if (flag4)
				{
					Enums.CardClass? cardClass = cardItem?.CardClass;
					if (cardClass.HasValue)
					{
						Enums.CardClass valueOrDefault = cardClass.GetValueOrDefault();
						if ((uint)(valueOrDefault - 5) <= 2u)
						{
							flag5 = true;
							goto IL_0a31;
						}
					}
					flag5 = false;
					goto IL_0a31;
				}
				goto IL_0a35;
			}
			goto IL_0a6d;
			IL_0a68:
			flag2 = true;
			flag = true;
			goto IL_0a6d;
			IL_0a6d:
			if (itemData.AuracurseGain2 != null)
			{
				num9 = ((itemData.AuraCurseNumForOneEvent <= 0) ? itemData.GetModifiedValue(itemData.AuracurseGainValue2, itemData.AuracurseGain2SpecialValue, itemData, target2) : (Functions.FuncRoundToInt(auxInt / itemData.AuraCurseNumForOneEvent) * itemData.GetModifiedValue(itemData.AuracurseGainValue2, itemData.AuracurseGain2SpecialValue, itemData, target2)));
				if (itemData.Acg2MultiplyByEnergyUsed)
				{
					num9 *= MatchManager.Instance.energyJustWastedByHero;
				}
				if (itemType == "corruption")
				{
					character6.SetAuraTrait(null, itemData.AuracurseGain2.Id, num9);
					goto IL_0b95;
				}
				flag4 = character.IsHero;
				if (flag4)
				{
					Enums.CardClass? cardClass = cardItem?.CardClass;
					if (cardClass.HasValue)
					{
						Enums.CardClass valueOrDefault = cardClass.GetValueOrDefault();
						if ((uint)(valueOrDefault - 5) <= 2u)
						{
							flag5 = true;
							goto IL_0b5e;
						}
					}
					flag5 = false;
					goto IL_0b5e;
				}
				goto IL_0b62;
			}
			goto IL_0b9a;
			IL_0b5e:
			flag4 = flag5;
			goto IL_0b62;
			IL_0b62:
			if (flag4)
			{
				character6.SetAuraTrait(null, itemData.AuracurseGain2.Id, num9);
			}
			else
			{
				character6.SetAuraTrait(character, itemData.AuracurseGain2.Id, num9);
			}
			goto IL_0b95;
			IL_0b95:
			flag2 = true;
			flag = true;
			goto IL_0b9a;
			IL_0a35:
			if (flag4)
			{
				character6.SetAuraTrait(null, itemData.AuracurseGain1.Id, num9);
			}
			else
			{
				character6.SetAuraTrait(character, itemData.AuracurseGain1.Id, num9);
			}
			goto IL_0a68;
			IL_0b9a:
			if (itemData.AuracurseGain3 != null)
			{
				num9 = ((itemData.AuraCurseNumForOneEvent <= 0) ? itemData.GetModifiedValue(itemData.AuracurseGainValue3, itemData.AuracurseGain3SpecialValue, itemData, target2) : (Functions.FuncRoundToInt(auxInt / itemData.AuraCurseNumForOneEvent) * itemData.GetModifiedValue(itemData.AuracurseGainValue3, itemData.AuracurseGain3SpecialValue, itemData, target2)));
				if (itemData.Acg3MultiplyByEnergyUsed)
				{
					num9 *= MatchManager.Instance.energyJustWastedByHero;
				}
				if (itemType == "corruption")
				{
					character6.SetAuraTrait(null, itemData.AuracurseGain3.Id, num9);
				}
				else if (character.IsHero && (cardItem.CardClass == Enums.CardClass.Monster || cardItem.CardClass == Enums.CardClass.Injury || cardItem.CardClass == Enums.CardClass.Boon))
				{
					character6.SetAuraTrait(null, itemData.AuracurseGain3.Id, num9);
				}
				else
				{
					character6.SetAuraTrait(character, itemData.AuracurseGain3.Id, num9);
				}
				flag2 = true;
				flag = true;
			}
			if (itemData.HealQuantity > 0 || itemData.HealQuantitySpecialValue.Use)
			{
				character6.ModifyHp(itemData.GetModifiedValue(itemData.HealQuantity, itemData.HealQuantitySpecialValue));
				CastResolutionForCombatText castResolutionForCombatText = new CastResolutionForCombatText();
				castResolutionForCombatText.heal = itemData.HealQuantity;
				if (character6.HeroItem != null)
				{
					character6.HeroItem.ScrollCombatTextDamageNew(castResolutionForCombatText);
				}
				else
				{
					character6.NPCItem.ScrollCombatTextDamageNew(castResolutionForCombatText);
				}
				flag = true;
			}
			else if (itemData.HealQuantity < 0)
			{
				int num14 = itemData.HealQuantity;
				if (itemData.UsedEnergy && castedCard.EnergyCost > 0)
				{
					num14 *= MatchManager.Instance.energyJustWastedByHero;
				}
				character6.ModifyHp(num14);
				CastResolutionForCombatText castResolutionForCombatText2 = new CastResolutionForCombatText();
				castResolutionForCombatText2.damage = itemData.HealQuantity;
				if (character6.HeroItem != null)
				{
					character6.HeroItem.ScrollCombatTextDamageNew(castResolutionForCombatText2);
				}
				else
				{
					character6.NPCItem.ScrollCombatTextDamageNew(castResolutionForCombatText2);
				}
				flag = true;
			}
			if (itemData.HealPercentQuantity != 0)
			{
				if (itemData.Activation == Enums.EventActivation.Killed)
				{
					if (itemData.HealPercentQuantity > 0)
					{
						if (character6.GetHp() > 0)
						{
							return;
						}
						character6.Resurrect(itemData.HealPercentQuantity);
						flag2 = true;
						MatchManager.Instance.itemTimeout[order] = 0.5f;
					}
				}
				else if (character6.GetHp() > 0 && itemData.HealPercentQuantity != 0)
				{
					character6.PercentHeal(itemData.HealPercentQuantity, _includeInStats: true);
					flag2 = true;
				}
				flag = true;
			}
			if (itemData.HealPercentQuantitySelf != 0)
			{
				if (itemData.Activation == Enums.EventActivation.Killed)
				{
					if (itemData.HealPercentQuantitySelf > 0)
					{
						if (character.GetHp() > 0)
						{
							return;
						}
						character.Resurrect(itemData.HealPercentQuantitySelf);
						flag2 = true;
						MatchManager.Instance.itemTimeout[order] = 0.5f;
					}
				}
				else if (character.GetHp() > 0 && itemData.HealPercentQuantitySelf != 0 && !flag3)
				{
					flag3 = true;
					character.PercentHeal(itemData.HealPercentQuantitySelf, _includeInStats: true);
					flag2 = true;
				}
				flag = true;
			}
			if (itemData.EnergyQuantity > 0)
			{
				int num15 = itemData.EnergyQuantity;
				if (itemData.UsedEnergy && cardItem.EnergyCost > 0)
				{
					num15 *= MatchManager.Instance.energyJustWastedByHero;
				}
				character6.ModifyEnergy(num15, showScrollCombatText: true);
				flag = true;
			}
			continue;
			IL_0a31:
			flag4 = flag5;
			goto IL_0a35;
		}
		if (!itemData.ChooseOneACToGain)
		{
			if (itemData.AuracurseGainSelf1 != null)
			{
				if (itemType == "corruption")
				{
					character.SetAuraTrait(null, itemData.AuracurseGainSelf1.Id, itemData.AuracurseGainSelfValue1);
				}
				else if (character.IsHero && (cardItem.CardClass == Enums.CardClass.Monster || cardItem.CardClass == Enums.CardClass.Injury || cardItem.CardClass == Enums.CardClass.Boon))
				{
					character.SetAuraTrait(null, itemData.AuracurseGainSelf1.Id, itemData.AuracurseGainSelfValue1);
				}
				else
				{
					character.SetAuraTrait(character, itemData.AuracurseGainSelf1.Id, itemData.AuracurseGainSelfValue1);
				}
				flag2 = true;
				flag = true;
			}
			if (itemData.AuracurseGainSelf2 != null)
			{
				if (itemType == "corruption")
				{
					character.SetAuraTrait(null, itemData.AuracurseGainSelf2.Id, itemData.AuracurseGainSelfValue2);
				}
				else if (character.IsHero && (cardItem.CardClass == Enums.CardClass.Monster || cardItem.CardClass == Enums.CardClass.Injury || cardItem.CardClass == Enums.CardClass.Boon))
				{
					character.SetAuraTrait(null, itemData.AuracurseGainSelf2.Id, itemData.AuracurseGainSelfValue2);
				}
				else
				{
					character.SetAuraTrait(character, itemData.AuracurseGainSelf2.Id, itemData.AuracurseGainSelfValue2);
				}
				flag2 = true;
				flag = true;
			}
			if (itemData.AuracurseGainSelf3 != null)
			{
				if (itemType == "corruption")
				{
					character.SetAuraTrait(null, itemData.AuracurseGainSelf3.Id, itemData.AuracurseGainSelfValue3);
				}
				else if (character.IsHero && (cardItem.CardClass == Enums.CardClass.Monster || cardItem.CardClass == Enums.CardClass.Injury || cardItem.CardClass == Enums.CardClass.Boon))
				{
					character.SetAuraTrait(null, itemData.AuracurseGainSelf3.Id, itemData.AuracurseGainSelfValue3);
				}
				else
				{
					character.SetAuraTrait(character, itemData.AuracurseGainSelf3.Id, itemData.AuracurseGainSelfValue3);
				}
				flag2 = true;
				flag = true;
			}
		}
		else
		{
			List<AuraCurseData> list2 = new List<AuraCurseData>();
			List<int> list3 = new List<int>();
			if ((bool)itemData.AuracurseGainSelf1)
			{
				list2.Add(itemData.AuracurseGainSelf1);
				list3.Add(itemData.AuracurseGainSelfValue1);
			}
			if ((bool)itemData.AuracurseGainSelf2)
			{
				list2.Add(itemData.AuracurseGainSelf2);
				list3.Add(itemData.AuracurseGainSelfValue2);
			}
			if ((bool)itemData.AuracurseGainSelf3)
			{
				list2.Add(itemData.AuracurseGainSelf3);
				list3.Add(itemData.AuracurseGainSelfValue3);
			}
			int index2 = UnityEngine.Random.Range(0, list2.Count);
			if (list2[index2] != null)
			{
				if (itemType == "corruption")
				{
					character.SetAuraTrait(null, list2[index2].Id, list3[index2]);
				}
				else if (character.IsHero && (cardItem.CardClass == Enums.CardClass.Monster || cardItem.CardClass == Enums.CardClass.Injury || cardItem.CardClass == Enums.CardClass.Boon))
				{
					character.SetAuraTrait(null, list2[index2].Id, list3[index2]);
				}
				else
				{
					character.SetAuraTrait(character, list2[index2].Id, list3[index2]);
				}
				flag2 = true;
				flag = true;
			}
		}
		Character character7 = character;
		if (itemData.auracurseHeal1 != null)
		{
			if (itemData.AcHealFromTarget && itemData.ItemTarget == Enums.ItemTarget.CurrentTarget && target != null)
			{
				character7 = target;
			}
			character7.HealAuraCurse(itemData.auracurseHeal1);
			if (itemData.auracurseHeal2 != null)
			{
				character7.HealAuraCurse(itemData.auracurseHeal2);
				if (itemData.auracurseHeal3 != null)
				{
					character7.HealAuraCurse(itemData.auracurseHeal3);
				}
			}
		}
		if (character != null && theEvent == itemData.Activation)
		{
			List<string> list4 = new List<string>();
			List<int> list5 = new List<int>();
			int num16 = 0;
			Character character8 = target;
			if (character8 == null && itemData.ItemTarget == Enums.ItemTarget.RandomEnemy)
			{
				character8 = GetRandomNPC();
			}
			if (character8 != null)
			{
				for (int m = 0; m < character8.AuraList.Count; m++)
				{
					if (num16 >= itemData.StealAuras)
					{
						break;
					}
					if (character8.AuraList[m] != null && character8.AuraList[m].ACData != null && character8.AuraList[m].ACData.IsAura && character8.AuraList[m].ACData.Removable && character8.AuraList[m].GetCharges() > 0 && (!(character8.AuraList[m].ACData.Id == "invulnerable") || !character8.CharacterIsDraculaBat()))
					{
						list4.Add(character8.AuraList[m].ACData.Id);
						list5.Add(character8.AuraList[m].GetCharges());
						num16++;
					}
				}
			}
			if (num16 > 0)
			{
				character8.HealCursesName(list4);
				for (int n = 0; n < list4.Count; n++)
				{
					if (character != null && character.Alive)
					{
						character.SetAuraTrait(character, list4[n], list5[n]);
					}
				}
			}
		}
		teamHero = MatchManager.Instance.GetTeamHero();
		foreach (Hero hero2 in teamHero)
		{
			if (hero2 != null && hero2.Alive)
			{
				hero2.GetAurasAuraCurseModifiers();
			}
		}
		MatchManager.Instance.RefreshStatusEffects();
		int num17 = MatchManager.Instance.CountHeroHand(character.HeroIndex);
		if (itemData.CardNum > 0 && (itemData.CardPlace != Enums.CardPlace.Hand || num17 < 10) && (itemData.CardToGain != null || itemData.CardToGainList != null || itemData.DuplicateActive))
		{
			int num18 = itemData.CardNum;
			if (character.IsHero && itemData.CardPlace == Enums.CardPlace.Hand && MatchManager.Instance.CountHeroHand() + num18 > 10)
			{
				num18 = 10 - MatchManager.Instance.CountHeroHand();
			}
			for (int num19 = 0; num19 < num18; num19++)
			{
				string id = "";
				if (itemData.DuplicateActive)
				{
					id = text;
				}
				else if (itemData.CardToGainList.Count <= 0)
				{
					id = ((!(itemData.CardToGain != null)) ? Functions.GetRandomCardIdByTypeAndRandomRarity(itemData.CardToGainType) : itemData.CardToGain.Id);
				}
				else
				{
					bool flag6 = false;
					while (!flag6)
					{
						int randomIntRange = MatchManager.Instance.GetRandomIntRange(0, itemData.CardToGainList.Count, "item");
						if (itemData.CardToGainList[randomIntRange] != null)
						{
							id = itemData.CardToGainList[randomIntRange].Id;
							flag6 = true;
						}
					}
				}
				string text2 = MatchManager.Instance.CreateCardInDictionary(id);
				CardData cardData = MatchManager.Instance.GetCardData(text2);
				if (itemData.DuplicateActive)
				{
					cardData = MatchManager.Instance.DuplicateCardData(cardData, MatchManager.Instance.GetCardData(id));
				}
				cardData.Vanish = itemData.Vanish;
				if (itemData.Permanent)
				{
					if (itemData.CostZero)
					{
						cardData.EnergyReductionToZeroPermanent = true;
					}
					else
					{
						cardData.EnergyReductionPermanent += itemData.CostReduction;
					}
				}
				else if (itemData.CostZero)
				{
					cardData.EnergyReductionToZeroTemporal = true;
				}
				else
				{
					cardData.EnergyReductionTemporal += itemData.CostReduction;
				}
				MatchManager.Instance.ModifyCardInDictionary(text2, cardData);
				if (character.IsHero)
				{
					if (itemData.CardPlace == Enums.CardPlace.Cast)
					{
						MatchManager.Instance.itemTimeout[order] = 0.5f;
					}
					else
					{
						MatchManager.Instance.itemTimeout[order] = 0.5f;
					}
					MatchManager.Instance.GenerateNewCard(1, text2, createCard: false, itemData.CardPlace, null, null, character.HeroIndex, isHero: true, num19);
				}
				else
				{
					MatchManager.Instance.GenerateNewCard(1, text2, createCard: false, itemData.CardPlace, null, null, character.NPCIndex, isHero: false);
				}
			}
		}
		if (itemData.CardsReduced > 0 && character.HeroData != null)
		{
			List<string> heroHand = MatchManager.Instance.GetHeroHand(character.HeroIndex);
			List<CardData> list6 = new List<CardData>();
			CardData cardData2 = null;
			int costReduceReduction = itemData.CostReduceReduction;
			for (int num20 = 0; num20 < heroHand.Count; num20++)
			{
				cardData2 = MatchManager.Instance.GetCardData(heroHand[num20]);
				if (cardData2 != null && ((costReduceReduction > 0 && cardData2.GetCardFinalCost() > 0) || costReduceReduction < 0) && (itemData.CardToReduceType == Enums.CardType.None || cardData2.HasCardType(itemData.CardToReduceType)) && (itemData.CostReduceEnergyRequirement == 0 || cardData2.GetCardFinalCost() >= itemData.CostReduceEnergyRequirement))
				{
					list6.Add(cardData2);
				}
			}
			if (list6.Count > 0)
			{
				cardData2 = null;
				int num21 = itemData.CardsReduced;
				if (num21 > list6.Count)
				{
					num21 = list6.Count;
				}
				List<string> list7 = new List<string>();
				for (int num22 = 0; num22 < num21; num22++)
				{
					bool flag7 = false;
					int num23 = 0;
					while (!flag7 && num23 < 100)
					{
						int num24 = 0;
						if (itemData.ReduceHighestCost)
						{
							int num25 = -1;
							int num26 = -1;
							CardData cardData3 = null;
							for (int num27 = 0; num27 < list6.Count; num27++)
							{
								int cardFinalCost = list6[num27].GetCardFinalCost();
								if (cardFinalCost > num25)
								{
									num25 = cardFinalCost;
								}
							}
							List<CardData> list8 = new List<CardData>();
							List<int> list9 = new List<int>();
							for (int num28 = 0; num28 < list6.Count; num28++)
							{
								if (list6[num28].GetCardFinalCost() == num25)
								{
									list8.Add(list6[num28]);
									list9.Add(num28);
								}
							}
							int index3 = UnityEngine.Random.Range(0, list8.Count);
							cardData3 = list8[index3];
							num26 = list9[index3];
							if (num26 > -1)
							{
								list6.RemoveAt(num26);
							}
							if (cardData3 != null)
							{
								cardData2 = cardData3;
							}
						}
						else
						{
							num24 = MatchManager.Instance.GetRandomIntRange(0, list6.Count, "item");
							cardData2 = list6[num24];
						}
						if (cardData2 != null && !list7.Contains(cardData2.Id))
						{
							if (itemData.CostReducePermanent)
							{
								cardData2.EnergyReductionPermanent += costReduceReduction;
							}
							else
							{
								cardData2.EnergyReductionTemporal += costReduceReduction;
							}
							MatchManager.Instance.UpdateHandCards();
							CardItem cardFromTableByIndex = MatchManager.Instance.GetCardFromTableByIndex(cardData2.InternalId);
							cardFromTableByIndex.PlayDissolveParticle();
							cardFromTableByIndex.ShowEnergyModification(-costReduceReduction);
							list7.Add(cardData2.Id);
							MatchManager.Instance.CreateLogCardModification(cardData2.InternalId, MatchManager.Instance.GetHero(character.HeroIndex));
							flag7 = true;
						}
						num23++;
					}
				}
				MatchManager.Instance.ItemActivationDisplay(itemType);
			}
		}
		if (MatchManager.Instance != null && itemData.IsEnchantment && itemData.DrawCards > 0)
		{
			int num29 = itemData.DrawCards;
			if (itemData.DrawMultiplyByEnergyUsed)
			{
				num29 *= MatchManager.Instance.energyJustWastedByHero;
			}
			if (num29 > 0)
			{
				if (GameManager.Instance.IsMultiplayer())
				{
					MatchManager.Instance.itemTimeout[order] = 0.5f;
				}
				else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
				{
					MatchManager.Instance.itemTimeout[order] = 0.5f;
				}
				else
				{
					MatchManager.Instance.itemTimeout[order] = 0.4f;
				}
				if (MatchManager.Instance.CountHeroDeck() == 0)
				{
					MatchManager.Instance.itemTimeout[order] += 0.7f;
				}
				MatchManager.Instance.NewCard(num29, Enums.CardFrom.Deck);
			}
		}
		if (itemData.DestroyAfterUse)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD(itemName + " itemData.DestroyAfterUse", "item");
			}
			if (character.HeroData != null)
			{
				if (character.HeroIndex == MatchManager.Instance.GetHeroActive())
				{
					MatchManager.Instance.ItemActivationDisplay(itemType);
				}
				if (character.IsHero)
				{
					AtOManager.Instance.RemoveItemFromHero(_isHero: true, character.HeroIndex, "", itemData.Id);
				}
				else
				{
					AtOManager.Instance.RemoveItemFromHero(_isHero: false, character.HeroIndex, "", itemData.Id);
				}
				if (cardItem != null && cardItem.Item != null)
				{
					MatchManager.Instance.DestroyedItemInThisTurn(character.HeroIndex, cardItem.Id);
				}
				MatchManager.Instance.RefreshItems();
			}
			else if (character.NpcData != null)
			{
				MatchManager.Instance.RemoveCorruptionItemFromNPC(character.NPCIndex);
			}
		}
		if (itemData.ExactRound > 0 || itemData.RoundCycle > 0 || itemData.TimesPerTurn > 0)
		{
			MatchManager.Instance.ItemActivationDisplay(itemType);
		}
		if (!(MatchManager.Instance != null))
		{
			return;
		}
		if (itemData.EffectCaster != "" && character != null)
		{
			if (character.HeroItem != null)
			{
				EffectsManager.Instance.PlayEffectAC(itemData.EffectCaster, isHero: true, character.HeroItem.CharImageT, flip: false, itemData.EffectCasterDelay);
			}
			else if (character.NPCItem != null)
			{
				EffectsManager.Instance.PlayEffectAC(itemData.EffectCaster, isHero: true, character.NPCItem.CharImageT, flip: false, itemData.EffectCasterDelay);
			}
		}
		if (itemData.EffectTarget != "")
		{
			for (int num30 = 0; num30 < list.Count; num30++)
			{
				Character character9 = list[num30];
				if (itemData.DontTargetBoss && character9 != null && character9.NpcData != null && character9.NpcData.IsBoss)
				{
					break;
				}
				if (character9 != null)
				{
					if (character9.HeroItem != null)
					{
						EffectsManager.Instance.PlayEffectAC(itemData.EffectTarget, isHero: true, character9.HeroItem.CharImageT, flip: false, itemData.EffectTargetDelay);
					}
					else if (character9.NPCItem != null)
					{
						EffectsManager.Instance.PlayEffectAC(itemData.EffectTarget, isHero: true, character9.NPCItem.CharImageT, flip: false, itemData.EffectTargetDelay);
					}
				}
			}
		}
		if (itemData.ItemSound != null)
		{
			GameManager.Instance.PlayAudio(itemData.ItemSound, 0.25f);
		}
		if (flag)
		{
			MatchManager.Instance.ReDrawInitiatives();
		}
		if (flag2)
		{
			ShowCombatText(itemType, itemName, character, charges, num);
		}
		if (itemType == "corruption" && MatchManager.Instance != null && !MatchManager.Instance.IsBeginTournPhase && theEvent != Enums.EventActivation.PreFinishCast && theEvent != Enums.EventActivation.FinishCast && theEvent != Enums.EventActivation.FinishFinishCast)
		{
			MatchManager.Instance.DoItemEventDelay();
		}
		if (itemData.PetActivation == Enums.ActivePets.AllTeam)
		{
			teamHero = MatchManager.Instance.GetTeamHero();
			foreach (Hero hero3 in teamHero)
			{
				if (hero3 != null && hero3.Alive)
				{
					hero3.ActivateItem(Enums.EventActivation.None, null, 0, hero3.Pet, Enums.ActivationManual.None, forceActivate: true);
					AtOManager.Instance.RaisePetActivationEvent(character, hero3, hero3.Pet, 0, fromPlayerCard: true);
				}
			}
		}
		if (itemData.PetActivation == Enums.ActivePets.Self)
		{
			character.ActivateItem(Enums.EventActivation.None, null, 0, character.Pet, Enums.ActivationManual.None, forceActivate: true);
			AtOManager.Instance.RaisePetActivationEvent(character, character, character.Pet, 0, fromPlayerCard: true);
		}
		if (itemData.IncreaseAurasSelf <= 0)
		{
			return;
		}
		Character character10 = null;
		if (character != null && character.Alive)
		{
			character10 = character;
		}
		if (character10 == null || itemData.IncreaseAurasSelf <= 0)
		{
			return;
		}
		List<string> list10 = new List<string>();
		List<int> list11 = new List<int>();
		bool flag8 = false;
		for (int num31 = 0; num31 < character10.AuraList.Count; num31++)
		{
			if (character10.AuraList[num31] != null && character10.AuraList[num31].ACData != null && character10.AuraList[num31].GetCharges() > 0 && !(character10.AuraList[num31].ACData.Id == "furnace") && !(character10.AuraList[num31].ACData.Id == "spellsword") && !character10.AuraList[num31].ACData.Id.StartsWith("rune"))
			{
				flag8 = false;
				if (character10.AuraList[num31].ACData.IsAura)
				{
					flag8 = true;
				}
				if (flag8)
				{
					list10.Add(character10.AuraList[num31].ACData.Id);
					list11.Add(character10.AuraList[num31].GetCharges());
				}
			}
		}
		if (list10.Count <= 0)
		{
			return;
		}
		for (int num32 = 0; num32 < list10.Count; num32++)
		{
			int num33 = 0;
			num33 = Functions.FuncRoundToInt((float)list11[num32] * (float)itemData.IncreaseAurasSelf / 100f);
			AuraCurseData auraCurseData = AtOManager.Instance.GlobalAuraCurseModificationByTraitsAndItems("set", list10[num32], character, character10);
			if (auraCurseData != null)
			{
				int maxCharges = auraCurseData.GetMaxCharges();
				if (maxCharges > -1 && list11[num32] + num33 > maxCharges)
				{
					num33 = maxCharges - list11[num32];
				}
				character10.SetAura(character, auraCurseData, num33, fromTrait: false, cardItem.CardClass, useCharacterMods: false, canBePreventable: false);
			}
		}
	}

	private void ShowCombatText(string itemType, string itemName, Character character, int charges = -1, int chargesTotal = -1)
	{
		if (MatchManager.Instance != null)
		{
			Enums.CombatScrollEffectType type = itemType switch
			{
				"weapon" => Enums.CombatScrollEffectType.Weapon, 
				"armor" => Enums.CombatScrollEffectType.Armor, 
				"jewelry" => Enums.CombatScrollEffectType.Jewelry, 
				"accesory" => Enums.CombatScrollEffectType.Accesory, 
				_ => Enums.CombatScrollEffectType.Corruption, 
			};
			if (character.HeroItem != null)
			{
				character.HeroItem.ScrollCombatText(itemName, type);
			}
			else
			{
				character.NPCItem.ScrollCombatText(itemName, type);
			}
		}
	}

	private Hero GetRandomHero()
	{
		Hero[] teamHero = MatchManager.Instance.GetTeamHero();
		List<int> list = new List<int>();
		for (int i = 0; i < teamHero.Length; i++)
		{
			if (teamHero[i] != null && teamHero[i].HeroData != null && teamHero[i].Alive)
			{
				list.Add(i);
			}
		}
		if (list.Count > 0)
		{
			bool flag = false;
			int num = 0;
			while (!flag)
			{
				int randomIntRange = MatchManager.Instance.GetRandomIntRange(0, list.Count, "item");
				if (teamHero[list[randomIntRange]] != null && teamHero[list[randomIntRange]].Alive)
				{
					return teamHero[list[randomIntRange]];
				}
				num++;
				if (num > 10)
				{
					flag = true;
				}
			}
		}
		return null;
	}

	private NPC GetRandomNPC(bool noBoss = false)
	{
		NPC[] teamNPC = MatchManager.Instance.GetTeamNPC();
		List<int> list = new List<int>();
		for (int i = 0; i < teamNPC.Length; i++)
		{
			if (teamNPC[i] != null && teamNPC[i].Alive)
			{
				list.Add(i);
			}
		}
		if (list.Count > 0)
		{
			bool flag = false;
			int num = MatchManager.Instance.GetRandomIntRange(0, list.Count, "item");
			int num2 = 0;
			while (!flag)
			{
				if (num < list.Count && teamNPC[list[num]] != null && teamNPC[list[num]].Alive)
				{
					if (!noBoss)
					{
						return teamNPC[list[num]];
					}
					if (teamNPC[list[num]].NpcData != null && !teamNPC[list[num]].NpcData.IsBoss)
					{
						return teamNPC[list[num]];
					}
				}
				num++;
				num %= teamNPC.Length;
				num2++;
				if (num2 > teamNPC.Length)
				{
					return null;
				}
			}
		}
		return null;
	}

	private NPC GetFrontNPC(bool noBoss = false)
	{
		NPC[] teamNPC = MatchManager.Instance.GetTeamNPC();
		for (int i = 0; i < teamNPC.Length; i++)
		{
			if (teamNPC[i] != null && teamNPC[i].Alive)
			{
				if (!noBoss)
				{
					return teamNPC[i];
				}
				if (!teamNPC[i].NPCIsBoss())
				{
					return teamNPC[i];
				}
			}
		}
		return null;
	}

	private Character GetRandomCharacter(bool noBoss = false)
	{
		if (MatchManager.Instance.GetRandomIntRange(0, 2, "item") == 0)
		{
			return GetRandomHero();
		}
		return GetRandomNPC(noBoss);
	}

	private List<Character> GetAllHeroList()
	{
		Character[] teamHero = MatchManager.Instance.GetTeamHero();
		Character[] array = teamHero;
		List<Character> list = new List<Character>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null && array[i].HeroData != null && array[i].Alive)
			{
				list.Add(array[i]);
			}
		}
		return list;
	}

	private List<Character> GetAllNPCList()
	{
		Character[] teamNPC = MatchManager.Instance.GetTeamNPC();
		Character[] array = teamNPC;
		List<Character> list = new List<Character>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null && array[i].Alive)
			{
				list.Add(array[i]);
			}
		}
		return list;
	}

	private Character GetFlatHPCharacter(bool highestHp, bool isHero)
	{
		int num = 0;
		if (!highestHp)
		{
			num = 10000;
		}
		List<Character> list = new List<Character>();
		List<Character> list2 = ((!isHero) ? GetAllNPCList() : GetAllHeroList());
		for (int i = 0; i < list2.Count; i++)
		{
			if (highestHp)
			{
				if (list2[i].HpCurrent >= num)
				{
					list.Add(list2[i]);
					num = list2[i].HpCurrent;
				}
			}
			else if (list2[i].HpCurrent <= num)
			{
				list.Add(list2[i]);
				num = list2[i].HpCurrent;
			}
		}
		if (list.Count > 1)
		{
			for (int num2 = list.Count - 1; num2 >= 0; num2--)
			{
				if (highestHp)
				{
					if (list[num2].HpCurrent < num)
					{
						list.RemoveAt(num2);
					}
				}
				else if (list[num2].HpCurrent > num)
				{
					list.RemoveAt(num2);
				}
			}
		}
		if (list.Count > 1)
		{
			return list[MatchManager.Instance.GetRandomIntRange(0, list.Count, "item")];
		}
		if (list.Count == 1)
		{
			return list[0];
		}
		return null;
	}

	public void surprisebox(Character theCharacter, bool isRare, string itemName)
	{
		if (MatchManager.Instance != null)
		{
			int randomIntRange = MatchManager.Instance.GetRandomIntRange(0, 6, "item");
			int charges = 2;
			if (isRare)
			{
				charges = 4;
			}
			switch (randomIntRange)
			{
			case 0:
				theCharacter.SetAuraTrait(theCharacter, "fast", charges);
				break;
			case 1:
				theCharacter.SetAuraTrait(theCharacter, "powerful", charges);
				break;
			case 2:
				theCharacter.SetAuraTrait(theCharacter, "bless", charges);
				break;
			case 3:
				theCharacter.SetAuraTrait(theCharacter, "slow", charges);
				break;
			case 4:
				theCharacter.SetAuraTrait(theCharacter, "vulnerable", charges);
				break;
			default:
				theCharacter.SetAuraTrait(theCharacter, "mark", charges);
				break;
			}
			theCharacter.HeroItem.ScrollCombatText(itemName, Enums.CombatScrollEffectType.Accesory);
		}
	}

	public void surprisegiftbox(Character theCharacter, bool isRare, string itemName)
	{
		if (!(MatchManager.Instance != null))
		{
			return;
		}
		int randomIntRange = MatchManager.Instance.GetRandomIntRange(0, 6, "item");
		int charges = 2;
		if (isRare)
		{
			charges = 4;
		}
		string text = "";
		text = randomIntRange switch
		{
			0 => "fast", 
			1 => "powerful", 
			2 => "bless", 
			3 => "slow", 
			4 => "vulnerable", 
			_ => "mark", 
		};
		Hero[] teamHero = MatchManager.Instance.GetTeamHero();
		for (int i = 0; i < teamHero.Length; i++)
		{
			if (teamHero[i] != null && teamHero[i].HeroData != null && teamHero[i].Alive)
			{
				teamHero[i].SetAuraTrait(theCharacter, text, charges);
			}
		}
		theCharacter.HeroItem.ScrollCombatText(itemName, Enums.CombatScrollEffectType.Accesory);
	}
}
