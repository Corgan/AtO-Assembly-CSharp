using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

public class Trait
{
	private List<string> restrictedTraits = new List<string>();

	public void DoTrait(Enums.EventActivation _theEvent, string _trait, Character _character, Character _target, int _auxInt, string _auxString, CardData _castedCard)
	{
		if (!(MatchManager.Instance == null))
		{
			UnityEngine.Random.InitState(DateTime.Now.Millisecond);
			MethodInfo method = GetType().GetMethod(_trait);
			if ((!MatchManager.Instance.ItemTraitsActivatedSinceLastCardCast.Contains(_trait + _character.SourceName) || !restrictedTraits.Contains(_trait)) && method != null)
			{
				method.Invoke(this, new object[7] { _theEvent, _character, _target, _auxInt, _auxString, _castedCard, _trait });
			}
		}
	}

	private void DoCombatLogEntry(Enums.EventActivation _theEvent, string _trait, Character _character, Character _target, int _auxInt, string _auxString, CardData _castedCard)
	{
		string text = "trait:" + _trait;
		if ((bool)MatchManager.Instance)
		{
			text = text + "_" + MatchManager.Instance.LogDictionary.Count;
		}
		if (_theEvent != Enums.EventActivation.AuraCurseSet && (bool)MatchManager.Instance)
		{
			MatchManager.Instance.CreateLogEntry(_initial: true, text, _trait, null, null, null, null, MatchManager.Instance.GameRound(), Enums.EventActivation.TraitActivation);
		}
		else if ((bool)MatchManager.Instance && _theEvent == Enums.EventActivation.AuraCurseSet)
		{
			switch (_trait)
			{
			case "reaping":
			case "replenishing":
			case "glareofdoom":
			case "runicempowerment":
				MatchManager.Instance.CreateLogEntry(_initial: true, text, _trait, null, null, null, null, MatchManager.Instance.GameRound(), Enums.EventActivation.TraitActivation);
				break;
			}
		}
	}

	public void vampirismbloodmage(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (auxString.ToLower() == "bleed" && !target.IsHero)
		{
			target.SetAuraTrait(character, "leech", 1);
		}
	}

	public void runeweaver(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (auxInt >= 2)
		{
			CardData cardData = MatchManager.Instance.GetCardData(auxString);
			if (isAttackCard(cardData))
			{
				character.SetAuraTrait(character, "runered", auxInt / 2);
				PlayRuneVFX(character, "runered");
			}
			if (isDefenseCard(cardData))
			{
				character.SetAuraTrait(character, "runeblue", auxInt / 2);
				PlayRuneVFX(character, "runeblue");
			}
			if (isHealCard(cardData))
			{
				character.SetAuraTrait(character, "runegreen", auxInt / 2);
				PlayRuneVFX(character, "runegreen");
			}
		}
		MatchManager.Instance.RedrawCardsDescriptionPrecalculated();
	}

	private void PlayRuneVFX(Character character, string rune)
	{
		if (character.GetAuraCharges(rune) < Globals.Instance.GetAuraCurseData(rune).MaxCharges)
		{
			EffectsManager.Instance.PlayEffectAC(rune, isHero: true, character.HeroItem.CharImageT, flip: false);
		}
	}

	private bool isAttackCard(CardData cardData)
	{
		if (!cardData)
		{
			return false;
		}
		bool result = false;
		if (cardData.CardType == Enums.CardType.Attack || cardData.CardTypeAux.Contains(Enums.CardType.Attack) || cardData.CardType == Enums.CardType.Lightning_Spell || cardData.CardTypeAux.Contains(Enums.CardType.Lightning_Spell) || cardData.CardType == Enums.CardType.Fire_Spell || cardData.CardTypeAux.Contains(Enums.CardType.Fire_Spell) || cardData.CardType == Enums.CardType.Cold_Spell || cardData.CardTypeAux.Contains(Enums.CardType.Cold_Spell) || cardData.CardType == Enums.CardType.Shadow_Spell || cardData.CardTypeAux.Contains(Enums.CardType.Shadow_Spell) || cardData.CardType == Enums.CardType.Curse_Spell || cardData.CardTypeAux.Contains(Enums.CardType.Curse_Spell))
		{
			result = true;
		}
		else if (cardData.DamageType != Enums.DamageType.None && (cardData.Damage > 0 || cardData.DamageSpecialValueGlobal || cardData.DamageSpecialValue1 || cardData.DamageSpecialValue2))
		{
			result = true;
		}
		else if (cardData.DamageType2 != Enums.DamageType.None && (cardData.Damage2 > 0 || cardData.Damage2SpecialValueGlobal || cardData.Damage2SpecialValue1 || cardData.Damage2SpecialValue2))
		{
			result = true;
		}
		return result;
	}

	private bool isHealCard(CardData cardData)
	{
		if (!cardData)
		{
			return false;
		}
		if (cardData.CardType == Enums.CardType.Healing_Spell || cardData.CardTypeAux.Contains(Enums.CardType.Healing_Spell))
		{
			return true;
		}
		return false;
	}

	private bool isDefenseCard(CardData cardData)
	{
		if (!cardData)
		{
			return false;
		}
		if (cardData.CardType == Enums.CardType.Defense || cardData.CardTypeAux.Contains(Enums.CardType.Defense))
		{
			return true;
		}
		return false;
	}

	private bool isSkillCard(CardData cardData)
	{
		if (!cardData)
		{
			return false;
		}
		if (cardData.CardType == Enums.CardType.Skill || cardData.CardTypeAux.Contains(Enums.CardType.Skill))
		{
			return true;
		}
		return false;
	}

	public void crimsonripple(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(auxString.ToLower() == "bleed") || target.IsHero)
		{
			return;
		}
		if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey("crimsonripple"))
		{
			int num = Globals.Instance.GetTraitData("crimsonripple").TimesPerTurn - 1;
			if (MatchManager.Instance.activatedTraits["crimsonripple"] > num)
			{
				return;
			}
		}
		foreach (NPC nPCSide in MatchManager.Instance.GetNPCSides(target.Position))
		{
			nPCSide.SetAuraTrait(character, "burn", 1);
			nPCSide.SetAuraTrait(character, "dark", 1);
		}
		MatchManager.Instance.ItemTraitsActivatedSinceLastCardCast.Add(trait + character.SourceName);
		if (!MatchManager.Instance.activatedTraits.ContainsKey("crimsonripple"))
		{
			MatchManager.Instance.activatedTraits.Add("crimsonripple", 1);
		}
		else
		{
			MatchManager.Instance.activatedTraits["crimsonripple"]++;
		}
		MatchManager.Instance.SetTraitInfoText();
	}

	public void reaping(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		ReduceDeckCostOn3Runes(theEvent, character, target, auxInt, auxString, castedCard, trait, "runered", isAttackCard);
	}

	private void ReduceDeckCostOn3Runes(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait, string rune, Func<CardData, bool> CardTypeCondition)
	{
		TraitData traitData = Globals.Instance.GetTraitData(trait);
		if (theEvent == Enums.EventActivation.ResetDeck && character.GetAuraCharges(rune) >= 3)
		{
			ReduceDeckCost(character, 1, playParticles: true, CardTypeCondition, skipHand: true);
			DisplayTraitScrollText(theEvent, character, target, auxInt, auxString, castedCard, trait);
			DoCombatLogEntry(theEvent, trait, character, target, auxInt, auxString, castedCard);
		}
		if (theEvent == Enums.EventActivation.EndTurn || (theEvent == Enums.EventActivation.AuraCurseRemoved && auxString == rune))
		{
			ResetDeckCost(character, CardTypeCondition);
			return;
		}
		switch (theEvent)
		{
		case Enums.EventActivation.DrawCard:
		{
			CardData cardData = MatchManager.Instance.GetCardData(auxString, createInDict: false);
			if (CardTypeCondition(cardData) && character.GetAuraCharges(rune) >= 3)
			{
				cardData.EnergyReductionTemporal++;
				MatchManager.Instance.UpdateHandCards();
				CardItem cardFromTableByIndex = MatchManager.Instance.GetCardFromTableByIndex(auxString);
				if ((bool)cardFromTableByIndex)
				{
					cardFromTableByIndex.PlayDissolveParticle();
					cardFromTableByIndex.ShowEnergyModification(-1);
				}
			}
			break;
		}
		case Enums.EventActivation.BeginTurn:
			if (!MatchManager.Instance.activatedTraits.ContainsKey(trait))
			{
				MatchManager.Instance.activatedTraits.Add(trait, 0);
			}
			else
			{
				MatchManager.Instance.activatedTraits[trait] = 0;
			}
			break;
		default:
			if ((theEvent == traitData.Activation && character.GetAuraCharges(rune) >= 3) || (theEvent == Enums.EventActivation.AuraCurseSet && auxString == rune && character.GetAuraCharges(rune) < 3 && character.GetAuraCharges(rune) + auxInt >= 3))
			{
				ReduceDeckCost(character, 1, playParticles: true, CardTypeCondition);
				DisplayTraitScrollText(theEvent, character, target, auxInt, auxString, castedCard, trait);
				DoCombatLogEntry(theEvent, trait, character, target, auxInt, auxString, castedCard);
			}
			break;
		}
		MatchManager.Instance.SetTraitInfoText();
	}

	public void UpdateCostIf3Runes(int heroActive, CardData card)
	{
		Hero hero = MatchManager.Instance.GetTeamHero()[heroActive];
		if (hero == null || !hero.Alive)
		{
			return;
		}
		if (AtOManager.Instance.CharacterHaveTrait(hero.SubclassName, "reaping") && hero.GetAuraCharges("runered") >= 3)
		{
			if (isAttackCard(card))
			{
				card.EnergyReductionTemporal += -1;
				MatchManager.Instance.UpdateHandCards();
				CardItem cardFromTableByIndex = MatchManager.Instance.GetCardFromTableByIndex(card.InternalId);
				if ((bool)cardFromTableByIndex)
				{
					cardFromTableByIndex.PlayDissolveParticle();
					cardFromTableByIndex.ShowEnergyModification(1);
				}
			}
		}
		else if (AtOManager.Instance.CharacterHaveTrait(hero.SubclassName, "replenishing") && hero.GetAuraCharges("runegreen") >= 3 && isHealCard(card))
		{
			card.EnergyReductionTemporal += -1;
			MatchManager.Instance.UpdateHandCards();
			CardItem cardFromTableByIndex2 = MatchManager.Instance.GetCardFromTableByIndex(card.InternalId);
			if ((bool)cardFromTableByIndex2)
			{
				cardFromTableByIndex2.PlayDissolveParticle();
				cardFromTableByIndex2.ShowEnergyModification(1);
			}
		}
	}

	private void ReduceDeckCost(Character character, int reduction, bool playParticles, Func<CardData, bool> CardTypeCondition, bool skipHand = false)
	{
		List<string> heroDeck = MatchManager.Instance.GetHeroDeck(character.HeroIndex);
		List<string> heroHand = MatchManager.Instance.GetHeroHand(character.HeroIndex);
		List<CardData> list = new List<CardData>();
		for (int i = 0; i < heroDeck.Count; i++)
		{
			CardData cardData = MatchManager.Instance.GetCardData(heroDeck[i]);
			if (cardData.EnergyCostOriginal > 0)
			{
				list.Add(cardData);
			}
		}
		if (!skipHand)
		{
			for (int j = 0; j < heroHand.Count; j++)
			{
				CardData cardData = MatchManager.Instance.GetCardData(heroHand[j]);
				if (cardData.EnergyCostOriginal > 0)
				{
					list.Add(cardData);
				}
			}
		}
		for (int k = 0; k < list.Count; k++)
		{
			CardData cardData = list[k];
			if (CardTypeCondition(cardData))
			{
				cardData.EnergyReductionTemporal += reduction;
				MatchManager.Instance.UpdateHandCards();
				CardItem cardFromTableByIndex = MatchManager.Instance.GetCardFromTableByIndex(cardData.InternalId);
				if (playParticles && (bool)cardFromTableByIndex)
				{
					cardFromTableByIndex.PlayDissolveParticle();
					cardFromTableByIndex.ShowEnergyModification(-reduction);
				}
			}
		}
	}

	private void DisplayTraitScrollText(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		TraitData traitData = Globals.Instance.GetTraitData(trait);
		if (!traitData)
		{
			string traitName = traitData.TraitName;
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText(traitName), Enums.CombatScrollEffectType.Trait);
		}
	}

	private void ResetDeckCost(Character character, Func<CardData, bool> CardTypeCondition)
	{
		List<string> heroDeck = MatchManager.Instance.GetHeroDeck(character.HeroIndex);
		for (int i = 0; i < heroDeck.Count; i++)
		{
			CardData cardData = MatchManager.Instance.GetCardData(heroDeck[i]);
			if (CardTypeCondition(cardData) && cardData.EnergyCostOriginal > 0)
			{
				cardData.EnergyReductionTemporal = 0;
			}
		}
		List<string> heroHand = MatchManager.Instance.GetHeroHand(character.HeroIndex);
		for (int j = 0; j < heroHand.Count; j++)
		{
			CardData cardData = MatchManager.Instance.GetCardData(heroHand[j]);
			if (CardTypeCondition(cardData) && cardData.EnergyCostOriginal > 0)
			{
				cardData.EnergyReductionTemporal = 0;
			}
		}
		MatchManager.Instance.RedrawCardsDescriptionPrecalculated();
	}

	public void replenishing(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		ReduceDeckCostOn3Runes(theEvent, character, target, auxInt, auxString, castedCard, trait, "runegreen", isHealCard);
	}

	public void energyspike(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
	}

	public void glareofdoom(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		ReduceDeckCostOn3Runes(theEvent, character, target, auxInt, auxString, castedCard, trait, "runeblue", isSkillCard);
	}

	public void runicempowerment(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if ((theEvent == Enums.EventActivation.AuraCurseSet && auxString == "reinforce") || auxString == "courage")
		{
			if (character != null && character.IsHero && MatchManager.Instance.GetIndexInCharOrder(character.HeroItem.GetComponent<HeroItem>().Hero) != 0)
			{
				return;
			}
			TraitData traitData = Globals.Instance.GetTraitData(trait);
			if (!character.HaveTrait(trait) || (MatchManager.Instance.activatedTraits.ContainsKey(trait) && MatchManager.Instance.activatedTraits[trait] >= traitData.TimesPerTurn))
			{
				return;
			}
			Character[] teamHero = MatchManager.Instance.GetTeamHero();
			character.SetAuraCurseTeam(character, teamHero, "infuse", 1);
			EffectsManager.Instance.PlayEffectAC("healerself2", isHero: true, character.HeroItem.CharImageT, flip: false);
			Hero[] teamHero2 = MatchManager.Instance.GetTeamHero();
			foreach (Hero hero in teamHero2)
			{
				if (hero != null && hero.Alive)
				{
					EffectsManager.Instance.PlayEffectAC("dispel", isHero: true, hero.HeroItem.CharImageT, flip: false, 0.4f);
				}
			}
			GameManager.Instance.PlayLibraryAudio("magicdispell");
			if (!MatchManager.Instance.activatedTraits.ContainsKey(trait))
			{
				MatchManager.Instance.activatedTraits.Add(trait, 1);
			}
			else
			{
				MatchManager.Instance.activatedTraits[trait]++;
			}
			MatchManager.Instance.SetTraitInfoText();
		}
		else
		{
			ReduceDeckCostOn3Runes(theEvent, character, target, auxInt, auxString, castedCard, trait, "runeblue", isDefenseCard);
		}
	}

	public void hemostasis(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
	}

	public void accurateshots(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (target != null && target.Alive)
		{
			target.SetAuraTrait(character, "sight", 2);
			target.SetAuraTrait(character, "bleed", 1);
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Accurate Shots"), Enums.CombatScrollEffectType.Trait);
		}
	}

	public void ardent(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		character.SetAuraTrait(character, "powerful", 2);
		character.SetAuraTrait(character, "burn", 2);
		if (character.HeroItem != null)
		{
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Ardent"), Enums.CombatScrollEffectType.Trait);
			EffectsManager.Instance.PlayEffectAC("powerful", isHero: true, character.HeroItem.CharImageT, flip: false);
			EffectsManager.Instance.PlayEffectAC("burnsmall", isHero: true, character.HeroItem.CharImageT, flip: false);
		}
	}

	public void blessed(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		character.SetAuraTrait(character, "bless", 1);
		if (character.HeroItem != null)
		{
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Blessed"), Enums.CombatScrollEffectType.Trait);
			EffectsManager.Instance.PlayEffectAC("bless", isHero: true, character.HeroItem.CharImageT, flip: false);
		}
	}

	public void bloody(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (target != null && target.Alive)
		{
			target.SetAuraTrait(character, "bleed", 2);
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Bloody"), Enums.CombatScrollEffectType.Trait);
		}
	}

	public void broodmother(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		Hero[] teamHero = MatchManager.Instance.GetTeamHero();
		string id = "spiderling";
		for (int i = 0; i < teamHero.Length; i++)
		{
			if (character.HaveTrait("templelurkers"))
			{
				id = ((!character.HaveTrait("spiderqueen")) ? "templelurker" : "templelurkerrare");
				break;
			}
			if (character.HaveTrait("mentalscavengers"))
			{
				id = ((!character.HaveTrait("spiderqueen")) ? "mentalscavengers" : "mentalscavengersrare");
				break;
			}
		}
		for (int j = 0; j < teamHero.Length; j++)
		{
			if (teamHero[j] != null && teamHero[j].HeroData != null && teamHero[j].Alive)
			{
				string text = MatchManager.Instance.CreateCardInDictionary(id);
				MatchManager.Instance.GetCardData(text);
				MatchManager.Instance.GenerateNewCard(1, text, createCard: false, Enums.CardPlace.RandomDeck, null, null, teamHero[j].HeroIndex);
			}
		}
		character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_BroodMother"), Enums.CombatScrollEffectType.Trait);
		MatchManager.Instance.ItemTraitActivated();
	}

	public void butcher(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (target == null || target.IsHero || !MatchManager.Instance.AnyNPCAlive())
		{
			return;
		}
		if (!character.HaveTrait("threestarchef"))
		{
			switch (MatchManager.Instance.GetRandomIntRange(0, 3, "trait"))
			{
			case 0:
			{
				string text3 = MatchManager.Instance.CreateCardInDictionary("premiummeat");
				MatchManager.Instance.GetCardData(text3);
				MatchManager.Instance.GenerateNewCard(1, text3, createCard: false, Enums.CardPlace.RandomDeck, null, null, character.HeroIndex);
				break;
			}
			case 1:
			{
				string text2 = MatchManager.Instance.CreateCardInDictionary("meat");
				MatchManager.Instance.GetCardData(text2);
				MatchManager.Instance.GenerateNewCard(1, text2, createCard: false, Enums.CardPlace.RandomDeck, null, null, character.HeroIndex);
				break;
			}
			default:
			{
				string text = MatchManager.Instance.CreateCardInDictionary("spoiledmeat");
				MatchManager.Instance.GetCardData(text);
				MatchManager.Instance.GenerateNewCard(1, text, createCard: false, Enums.CardPlace.RandomDeck, null, null, character.HeroIndex);
				break;
			}
			}
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Butcher"), Enums.CombatScrollEffectType.Trait);
		}
		else
		{
			string text4 = MatchManager.Instance.CreateCardInDictionary("gourmetmeat");
			MatchManager.Instance.GetCardData(text4);
			MatchManager.Instance.GenerateNewCard(1, text4, createCard: false, Enums.CardPlace.RandomDeck, null, null, character.HeroIndex);
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_threestarchef"), Enums.CombatScrollEffectType.Trait);
		}
		MatchManager.Instance.ItemTraitActivated();
	}

	public void cantor(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (character.GetAuraCharges("stanzai") == 0 || character.GetAuraCharges("stanzaii") == 0 || character.GetAuraCharges("stanzaiii") == 0)
		{
			character.SetAuraTrait(character, "stanzai", 1);
			if (character.HeroItem != null)
			{
				character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Cantor"), Enums.CombatScrollEffectType.Trait);
				EffectsManager.Instance.PlayEffectAC("songself", isHero: true, character.HeroItem.CharImageT, flip: false);
			}
		}
	}

	public void cautious(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		character.SetAuraTrait(character, "buffer", 2);
		character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Cautious"), Enums.CombatScrollEffectType.Trait);
	}

	public void chastise(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || !(castedCard != null))
		{
			return;
		}
		TraitData traitData = Globals.Instance.GetTraitData("chastise");
		if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey("chastise") && MatchManager.Instance.activatedTraits["chastise"] > traitData.TimesPerTurn - 1)
		{
			return;
		}
		if (MatchManager.Instance.CountHeroHand() == 10)
		{
			Debug.Log("[TRAIT EXECUTION] Broke because player at max cards");
		}
		else if (castedCard.GetCardTypes().Contains(Enums.CardType.Holy_Spell) && character.HeroData != null)
		{
			if (!MatchManager.Instance.activatedTraits.ContainsKey("chastise"))
			{
				MatchManager.Instance.activatedTraits.Add("chastise", 1);
			}
			else
			{
				MatchManager.Instance.activatedTraits["chastise"]++;
			}
			MatchManager.Instance.SetTraitInfoText();
			string text = "holysmite";
			int randomIntRange = MatchManager.Instance.GetRandomIntRange(0, 100, "trait");
			text = ((randomIntRange < 45) ? (text + "a") : ((randomIntRange >= 90) ? (text + "rare") : (text + "b")));
			string text2 = MatchManager.Instance.CreateCardInDictionary(text);
			CardData cardData = MatchManager.Instance.GetCardData(text2);
			cardData.Vanish = true;
			cardData.EnergyReductionToZeroPermanent = true;
			MatchManager.Instance.GenerateNewCard(1, text2, createCard: false, Enums.CardPlace.Hand);
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Chastise") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["chastise"], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
			MatchManager.Instance.ItemTraitActivated();
			MatchManager.Instance.CreateLogCardModification(cardData.InternalId, MatchManager.Instance.GetHero(character.HeroIndex));
		}
	}

	public void choir(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || MatchManager.Instance.GetCurrentRound() != 1)
		{
			return;
		}
		Hero[] teamHero = MatchManager.Instance.GetTeamHero();
		for (int i = 0; i < teamHero.Length; i++)
		{
			if (teamHero[i] != null && teamHero[i].HeroData != null && teamHero[i].Alive)
			{
				teamHero[i].SetAuraTrait(character, "stanzai", 1);
			}
		}
		character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Choir"), Enums.CombatScrollEffectType.Trait);
	}

	public void clever(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		character.SetAuraTrait(character, "inspire", 1);
		character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Clever"), Enums.CombatScrollEffectType.Trait);
	}

	public void combatready(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		character.SetAuraTrait(character, "energize", 1);
		character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Combat Ready"), Enums.CombatScrollEffectType.Trait);
	}

	public void corrosion(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(character.HeroData != null))
		{
			return;
		}
		NPC[] teamNPC = MatchManager.Instance.GetTeamNPC();
		for (int i = 0; i < teamNPC.Length; i++)
		{
			if (teamNPC[i] != null && teamNPC[i].Alive)
			{
				bool flag = false;
				int auraCharges = teamNPC[i].GetAuraCharges("wet");
				if (auraCharges > 0)
				{
					flag = true;
					teamNPC[i].SetAuraTrait(character, "poison", auraCharges);
				}
				auraCharges = teamNPC[i].GetAuraCharges("rust");
				if (auraCharges > 0)
				{
					flag = true;
					teamNPC[i].SetAuraTrait(character, "vulnerable", auraCharges);
				}
				if (teamNPC[i].NPCItem != null && flag)
				{
					teamNPC[i].NPCItem.ScrollCombatText(Texts.Instance.GetText("traits_Corrosion"), Enums.CombatScrollEffectType.Trait);
					EffectsManager.Instance.PlayEffectAC("poison", isHero: true, teamNPC[i].NPCItem.CharImageT, flip: false);
				}
			}
		}
	}

	public void countermeasures(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null))
		{
			return;
		}
		TraitData traitData = Globals.Instance.GetTraitData("countermeasures");
		if ((MatchManager.Instance.activatedTraitsRound == null || !MatchManager.Instance.activatedTraitsRound.ContainsKey("countermeasures") || MatchManager.Instance.activatedTraitsRound["countermeasures"] <= traitData.TimesPerRound - 1) && character != null && character.Alive && character.HeroItem != null)
		{
			if (!MatchManager.Instance.activatedTraitsRound.ContainsKey("countermeasures"))
			{
				MatchManager.Instance.activatedTraitsRound.Add("countermeasures", 1);
			}
			else
			{
				MatchManager.Instance.activatedTraitsRound["countermeasures"]++;
			}
			MatchManager.Instance.SetTraitInfoText();
			character.SetAuraTrait(character, "thorns", 3);
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Countermeasures") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraitsRound["countermeasures"], traitData.TimesPerRound), Enums.CombatScrollEffectType.Trait);
			EffectsManager.Instance.PlayEffectAC("thorns", isHero: true, character.HeroItem.CharImageT, flip: false);
		}
	}

	public void darkfeast(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(character.HeroData != null))
		{
			return;
		}
		int num = Mathf.FloorToInt(character.EffectCharges("dark") / 8);
		if (num <= 0)
		{
			return;
		}
		List<string> heroHand = MatchManager.Instance.GetHeroHand(character.HeroIndex);
		List<CardData> list = new List<CardData>();
		for (int i = 0; i < heroHand.Count; i++)
		{
			CardData cardData = MatchManager.Instance.GetCardData(heroHand[i]);
			if (cardData.GetCardFinalCost() > 0)
			{
				list.Add(cardData);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			CardData cardData = list[j];
			cardData.EnergyReductionTemporal += num;
			MatchManager.Instance.UpdateHandCards();
			CardItem cardFromTableByIndex = MatchManager.Instance.GetCardFromTableByIndex(cardData.InternalId);
			cardFromTableByIndex.PlayDissolveParticle();
			cardFromTableByIndex.ShowEnergyModification(-num);
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Dark Feast"), Enums.CombatScrollEffectType.Trait);
			MatchManager.Instance.CreateLogCardModification(cardData.InternalId, MatchManager.Instance.GetHero(character.HeroIndex));
		}
	}

	public void defensemastery(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(character.HeroData != null))
		{
			return;
		}
		int num = 1;
		if (num <= 0)
		{
			return;
		}
		List<string> heroHand = MatchManager.Instance.GetHeroHand(character.HeroIndex);
		List<CardData> list = new List<CardData>();
		for (int i = 0; i < heroHand.Count; i++)
		{
			CardData cardData = MatchManager.Instance.GetCardData(heroHand[i]);
			if (cardData != null && cardData.GetCardFinalCost() > 0 && cardData.HasCardType(Enums.CardType.Defense))
			{
				list.Add(cardData);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			CardData cardData = list[j];
			if (cardData != null)
			{
				cardData.EnergyReductionTemporal += num;
				MatchManager.Instance.UpdateHandCards();
				CardItem cardFromTableByIndex = MatchManager.Instance.GetCardFromTableByIndex(cardData.InternalId);
				cardFromTableByIndex.PlayDissolveParticle();
				cardFromTableByIndex.ShowEnergyModification(-num);
				character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Defense Mastery"), Enums.CombatScrollEffectType.Trait);
				MatchManager.Instance.CreateLogCardModification(cardData.InternalId, MatchManager.Instance.GetHero(character.HeroIndex));
			}
		}
	}

	public void dinnerisready(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		Hero[] teamHero = MatchManager.Instance.GetTeamHero();
		for (int i = 0; i < teamHero.Length; i++)
		{
			if (teamHero[i] == null || !(teamHero[i].HeroData != null) || !teamHero[i].Alive)
			{
				continue;
			}
			if (!character.HaveTrait("threestarchef"))
			{
				switch (MatchManager.Instance.GetRandomIntRange(0, 3, "trait"))
				{
				case 0:
				{
					string text3 = MatchManager.Instance.CreateCardInDictionary("premiummeat");
					MatchManager.Instance.GetCardData(text3);
					MatchManager.Instance.GenerateNewCard(1, text3, createCard: false, Enums.CardPlace.RandomDeck, null, null, teamHero[i].HeroIndex);
					break;
				}
				case 1:
				{
					string text2 = MatchManager.Instance.CreateCardInDictionary("meat");
					MatchManager.Instance.GetCardData(text2);
					MatchManager.Instance.GenerateNewCard(1, text2, createCard: false, Enums.CardPlace.RandomDeck, null, null, teamHero[i].HeroIndex);
					break;
				}
				default:
				{
					string text = MatchManager.Instance.CreateCardInDictionary("spoiledmeat");
					MatchManager.Instance.GetCardData(text);
					MatchManager.Instance.GenerateNewCard(1, text, createCard: false, Enums.CardPlace.RandomDeck, null, null, teamHero[i].HeroIndex);
					break;
				}
				}
			}
			else
			{
				switch (MatchManager.Instance.GetRandomIntRange(0, 3, "trait"))
				{
				case 0:
				{
					string text6 = MatchManager.Instance.CreateCardInDictionary("premiummeat");
					MatchManager.Instance.GetCardData(text6);
					MatchManager.Instance.GenerateNewCard(1, text6, createCard: false, Enums.CardPlace.RandomDeck, null, null, teamHero[i].HeroIndex);
					break;
				}
				case 1:
				{
					string text5 = MatchManager.Instance.CreateCardInDictionary("meat");
					MatchManager.Instance.GetCardData(text5);
					MatchManager.Instance.GenerateNewCard(1, text5, createCard: false, Enums.CardPlace.RandomDeck, null, null, teamHero[i].HeroIndex);
					break;
				}
				default:
				{
					string text4 = MatchManager.Instance.CreateCardInDictionary("gourmetmeat");
					MatchManager.Instance.GetCardData(text4);
					MatchManager.Instance.GenerateNewCard(1, text4, createCard: false, Enums.CardPlace.RandomDeck, null, null, teamHero[i].HeroIndex);
					break;
				}
				}
			}
		}
		character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_DinnerIsReady"), Enums.CombatScrollEffectType.Trait);
		MatchManager.Instance.ItemTraitActivated();
	}

	public void domeoflight(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || !(castedCard != null))
		{
			return;
		}
		TraitData traitData = Globals.Instance.GetTraitData("domeoflight");
		if ((MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey("domeoflight") && MatchManager.Instance.activatedTraits["domeoflight"] > traitData.TimesPerTurn - 1) || !castedCard.GetCardTypes().Contains(Enums.CardType.Defense))
		{
			return;
		}
		if (!MatchManager.Instance.activatedTraits.ContainsKey("domeoflight"))
		{
			MatchManager.Instance.activatedTraits.Add("domeoflight", 1);
		}
		else
		{
			MatchManager.Instance.activatedTraits["domeoflight"]++;
		}
		MatchManager.Instance.SetTraitInfoText();
		Hero[] teamHero = MatchManager.Instance.GetTeamHero();
		for (int i = 0; i < teamHero.Length; i++)
		{
			if (teamHero[i] != null && teamHero[i].HeroData != null && teamHero[i].Alive)
			{
				teamHero[i].SetAuraTrait(character, "shield", 9);
				if (teamHero[i].HeroItem != null)
				{
					EffectsManager.Instance.PlayEffectAC("shield1", isHero: true, teamHero[i].HeroItem.CharImageT, flip: false);
				}
			}
		}
		character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Dome Of Light") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["domeoflight"], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
	}

	public void eldritch(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (MatchManager.Instance != null && castedCard != null && character.HeroData != null)
		{
			int num = 2;
			bool flag = true;
			if (character.HaveTrait("frostswords"))
			{
				num = 1;
			}
			if (character.HaveTrait("timeloop") || character.HaveTrait("timeparadox"))
			{
				flag = false;
			}
			if (MatchManager.Instance.energyJustWastedByHero >= num && (!flag || castedCard.GetCardTypes().Contains(Enums.CardType.Spell)))
			{
				character.SetAuraTrait(character, "spellsword", 1);
				character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Eldritch"), Enums.CombatScrollEffectType.Trait);
			}
		}
	}

	public void elementalproliferation(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (target != null && target.Alive)
		{
			target.SetAuraTrait(character, "burn", 2);
			target.SetAuraTrait(character, "chill", 2);
			target.SetAuraTrait(character, "spark", 2);
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Elemental Proliferation"), Enums.CombatScrollEffectType.Trait);
		}
	}

	public void elementalweaver(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || !(castedCard != null))
		{
			return;
		}
		_ = castedCard.InternalId;
		int num = 0;
		int num2 = 0;
		if (castedCard.GetCardTypes().Contains(Enums.CardType.Fire_Spell))
		{
			for (int i = 0; i < character.CardsPlayedRound.Count; i++)
			{
				if (character.CardsPlayedRound[i] != null && character.CardsPlayedRound[i].HasCardType(Enums.CardType.Fire_Spell))
				{
					num2++;
				}
			}
			if (num2 <= 1)
			{
				num++;
			}
		}
		num2 = 0;
		if (castedCard.GetCardTypes().Contains(Enums.CardType.Cold_Spell))
		{
			for (int j = 0; j < character.CardsPlayedRound.Count; j++)
			{
				if (character.CardsPlayedRound[j] != null && character.CardsPlayedRound[j].HasCardType(Enums.CardType.Cold_Spell))
				{
					num2++;
				}
			}
			if (num2 <= 1)
			{
				num++;
			}
		}
		num2 = 0;
		if (castedCard.GetCardTypes().Contains(Enums.CardType.Lightning_Spell))
		{
			for (int k = 0; k < character.CardsPlayedRound.Count; k++)
			{
				if (character.CardsPlayedRound[k] != null && character.CardsPlayedRound[k].HasCardType(Enums.CardType.Lightning_Spell))
				{
					num2++;
				}
			}
			if (num2 <= 1)
			{
				num++;
			}
		}
		if (num <= 0)
		{
			return;
		}
		character.ModifyEnergy(num, showScrollCombatText: true);
		if (character.HeroItem != null)
		{
			if (!MatchManager.Instance.activatedTraits.ContainsKey("elementalweaver"))
			{
				MatchManager.Instance.activatedTraits.Add("elementalweaver", num);
			}
			else
			{
				MatchManager.Instance.activatedTraits["elementalweaver"] += num;
			}
			MatchManager.Instance.SetTraitInfoText();
			TraitData traitData = Globals.Instance.GetTraitData("elementalweaver");
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Elemental Weaver") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["elementalweaver"], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
			EffectsManager.Instance.PlayEffectAC("energy", isHero: true, character.HeroItem.CharImageT, flip: false);
		}
	}

	public void envenom(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (target != null && target.Alive)
		{
			target.SetAuraTrait(character, "poison", 3);
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Envenom"), Enums.CombatScrollEffectType.Trait);
		}
	}

	public void elusive(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (character != null && character.Alive)
		{
			character.SetAuraTrait(character, "evasion", 2);
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Elusive"), Enums.CombatScrollEffectType.Trait);
		}
	}

	public void exoskeleton(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || !(castedCard != null))
		{
			return;
		}
		TraitData traitData = Globals.Instance.GetTraitData("exoskeleton");
		if ((MatchManager.Instance.activatedTraits == null || !MatchManager.Instance.activatedTraits.ContainsKey("exoskeleton") || MatchManager.Instance.activatedTraits["exoskeleton"] <= traitData.TimesPerTurn - 1) && MatchManager.Instance.energyJustWastedByHero > 0 && castedCard.GetCardTypes().Contains(Enums.CardType.Defense) && character.HeroData != null)
		{
			if (!MatchManager.Instance.activatedTraits.ContainsKey("exoskeleton"))
			{
				MatchManager.Instance.activatedTraits.Add("exoskeleton", 1);
			}
			else
			{
				MatchManager.Instance.activatedTraits["exoskeleton"]++;
			}
			MatchManager.Instance.SetTraitInfoText();
			character.SetAuraTrait(character, "fortify", 1);
			character.ModifyEnergy(1, showScrollCombatText: true);
			if (character.HeroItem != null)
			{
				character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Exoskeleton") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["exoskeleton"], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
				EffectsManager.Instance.PlayEffectAC("parry", isHero: true, character.HeroItem.CharImageT, flip: false);
				EffectsManager.Instance.PlayEffectAC("energy", isHero: true, character.HeroItem.CharImageT, flip: false);
			}
		}
	}

	public void fanfare(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || !(castedCard != null))
		{
			return;
		}
		TraitData traitData = Globals.Instance.GetTraitData("fanfare");
		if ((MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey("fanfare") && MatchManager.Instance.activatedTraits["fanfare"] > traitData.TimesPerTurn - 1) || MatchManager.Instance.energyJustWastedByHero <= 0 || !castedCard.GetCardTypes().Contains(Enums.CardType.Song) || !(character.HeroData != null))
		{
			return;
		}
		if (!MatchManager.Instance.activatedTraits.ContainsKey("fanfare"))
		{
			MatchManager.Instance.activatedTraits.Add("fanfare", 1);
		}
		else
		{
			MatchManager.Instance.activatedTraits["fanfare"]++;
		}
		MatchManager.Instance.SetTraitInfoText();
		character.ModifyEnergy(1, showScrollCombatText: true);
		if (character.HeroItem != null)
		{
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Fanfare") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["fanfare"], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
			EffectsManager.Instance.PlayEffectAC("energy", isHero: true, character.HeroItem.CharImageT, flip: false);
		}
		Hero lowestHealthHero = GetLowestHealthHero(theEvent, character, target, auxInt, auxString, castedCard, trait);
		if (lowestHealthHero != null && lowestHealthHero.Alive)
		{
			lowestHealthHero.SetAuraTrait(character, "regeneration", 2);
			if (lowestHealthHero.HeroItem != null)
			{
				EffectsManager.Instance.PlayEffectAC("regeneration", isHero: true, lowestHealthHero.HeroItem.CharImageT, flip: false);
			}
		}
	}

	public void firestarter(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || !(castedCard != null))
		{
			return;
		}
		TraitData traitData = Globals.Instance.GetTraitData("firestarter");
		if ((MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey("firestarter") && MatchManager.Instance.activatedTraits["firestarter"] > traitData.TimesPerTurn - 1) || MatchManager.Instance.energyJustWastedByHero <= 0 || !castedCard.GetCardTypes().Contains(Enums.CardType.Fire_Spell) || !(character.HeroData != null))
		{
			return;
		}
		if (!MatchManager.Instance.activatedTraits.ContainsKey("firestarter"))
		{
			MatchManager.Instance.activatedTraits.Add("firestarter", 1);
		}
		else
		{
			MatchManager.Instance.activatedTraits["firestarter"]++;
		}
		MatchManager.Instance.SetTraitInfoText();
		character.ModifyEnergy(1, showScrollCombatText: true);
		if (character.HeroItem != null)
		{
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Firestarter") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["firestarter"], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
			EffectsManager.Instance.PlayEffectAC("energy", isHero: true, character.HeroItem.CharImageT, flip: false);
		}
		int num = 0;
		bool flag = false;
		NPC[] teamNPC = MatchManager.Instance.GetTeamNPC();
		while (!flag)
		{
			num = MatchManager.Instance.GetRandomIntRange(0, teamNPC.Length, "trait");
			if (teamNPC[num] != null && teamNPC[num].Alive)
			{
				teamNPC[num].SetAuraTrait(character, "burn", 1);
				if (teamNPC[num].NPCItem != null)
				{
					EffectsManager.Instance.PlayEffectAC("burnsmall", isHero: true, teamNPC[num].NPCItem.CharImageT, flip: false);
				}
				flag = true;
			}
		}
	}

	public void furious(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		character.SetAuraTrait(character, "fury", 2);
		if (character.HeroItem != null)
		{
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Furious"), Enums.CombatScrollEffectType.Trait);
			EffectsManager.Instance.PlayEffectAC("fury", isHero: true, character.HeroItem.CharImageT, flip: false);
		}
	}

	public void glasscannon(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
	}

	public void gluttony(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || !(castedCard != null))
		{
			return;
		}
		TraitData traitData = Globals.Instance.GetTraitData("gluttony");
		if ((MatchManager.Instance.activatedTraits == null || !MatchManager.Instance.activatedTraits.ContainsKey("gluttony") || MatchManager.Instance.activatedTraits["gluttony"] <= traitData.TimesPerTurn - 1) && castedCard.GetCardTypes().Contains(Enums.CardType.Food) && character.HeroData != null)
		{
			if (!MatchManager.Instance.activatedTraits.ContainsKey("gluttony"))
			{
				MatchManager.Instance.activatedTraits.Add("gluttony", 1);
			}
			else
			{
				MatchManager.Instance.activatedTraits["gluttony"]++;
			}
			MatchManager.Instance.SetTraitInfoText();
			character.SetAuraTrait(character, "vitality", 2);
			character.ModifyEnergy(1, showScrollCombatText: true);
			if (character.HeroItem != null)
			{
				character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Gluttony") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["gluttony"], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
				EffectsManager.Instance.PlayEffectAC("heart", isHero: true, character.HeroItem.CharImageT, flip: false);
				EffectsManager.Instance.PlayEffectAC("energy", isHero: true, character.HeroItem.CharImageT, flip: false);
			}
		}
	}

	public void healerduality(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || !(castedCard != null))
		{
			return;
		}
		TraitData traitData = Globals.Instance.GetTraitData("healerduality");
		if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey("healerduality"))
		{
			int num = traitData.TimesPerTurn - 1;
			if (character.HaveTrait("philosophersstone"))
			{
				num++;
			}
			if (MatchManager.Instance.activatedTraits["healerduality"] > num)
			{
				return;
			}
		}
		for (int i = 0; i < 2; i++)
		{
			Enums.CardClass cardClass;
			Enums.CardClass cardClass2;
			if (i == 0)
			{
				cardClass = Enums.CardClass.Scout;
				cardClass2 = Enums.CardClass.Healer;
			}
			else
			{
				cardClass = Enums.CardClass.Healer;
				cardClass2 = Enums.CardClass.Scout;
			}
			if (castedCard.CardClass != cardClass)
			{
				continue;
			}
			if (MatchManager.Instance.CountHeroHand() == 0 || !(character.HeroData != null))
			{
				break;
			}
			List<CardData> list = new List<CardData>();
			List<string> heroHand = MatchManager.Instance.GetHeroHand(character.HeroIndex);
			int num2 = 0;
			for (int j = 0; j < heroHand.Count; j++)
			{
				CardData cardData = MatchManager.Instance.GetCardData(heroHand[j]);
				if (cardData != null && cardData.CardClass == cardClass2 && character.GetCardFinalCost(cardData) > num2)
				{
					num2 = character.GetCardFinalCost(cardData);
				}
			}
			if (num2 <= 0)
			{
				break;
			}
			for (int k = 0; k < heroHand.Count; k++)
			{
				CardData cardData2 = MatchManager.Instance.GetCardData(heroHand[k]);
				if (cardData2 != null && cardData2.CardClass == cardClass2 && character.GetCardFinalCost(cardData2) >= num2)
				{
					list.Add(cardData2);
				}
			}
			if (list.Count <= 0)
			{
				break;
			}
			CardData cardData3 = null;
			cardData3 = ((list.Count != 1) ? list[MatchManager.Instance.GetRandomIntRange(0, list.Count, "trait")] : list[0]);
			if (cardData3 != null)
			{
				if (!MatchManager.Instance.activatedTraits.ContainsKey("healerduality"))
				{
					MatchManager.Instance.activatedTraits.Add("healerduality", 1);
				}
				else
				{
					MatchManager.Instance.activatedTraits["healerduality"]++;
				}
				MatchManager.Instance.SetTraitInfoText();
				int num3 = 1;
				cardData3.EnergyReductionTemporal += num3;
				MatchManager.Instance.GetCardFromTableByIndex(cardData3.InternalId).ShowEnergyModification(-num3);
				MatchManager.Instance.UpdateHandCards();
				if (!character.HaveTrait("philosophersstone"))
				{
					character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Healer Duality") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["healerduality"], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
				}
				else
				{
					character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Healer Duality") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["healerduality"], traitData.TimesPerTurn + 1), Enums.CombatScrollEffectType.Trait);
				}
				MatchManager.Instance.CreateLogCardModification(cardData3.InternalId, MatchManager.Instance.GetHero(character.HeroIndex));
			}
			break;
		}
	}

	public void healingbrew(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || !(castedCard != null))
		{
			return;
		}
		TraitData traitData = Globals.Instance.GetTraitData("healingbrew");
		if ((MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey("healingbrew") && MatchManager.Instance.activatedTraits["healingbrew"] > traitData.TimesPerTurn - 1) || MatchManager.Instance.energyJustWastedByHero <= 0 || !castedCard.GetCardTypes().Contains(Enums.CardType.Healing_Spell) || !(character.HeroData != null))
		{
			return;
		}
		if (!MatchManager.Instance.activatedTraits.ContainsKey("healingbrew"))
		{
			MatchManager.Instance.activatedTraits.Add("healingbrew", 1);
		}
		else
		{
			MatchManager.Instance.activatedTraits["healingbrew"]++;
		}
		MatchManager.Instance.SetTraitInfoText();
		character.ModifyEnergy(1, showScrollCombatText: true);
		if (character.HeroItem != null)
		{
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Healing Brew") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["healingbrew"], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
			EffectsManager.Instance.PlayEffectAC("energy", isHero: true, character.HeroItem.CharImageT, flip: false);
		}
		Hero lowestHealthHero = GetLowestHealthHero(theEvent, character, target, auxInt, auxString, castedCard, trait);
		if (lowestHealthHero != null && lowestHealthHero.Alive)
		{
			lowestHealthHero.SetAuraTrait(character, "regeneration", 2);
			if (lowestHealthHero.HeroItem != null)
			{
				EffectsManager.Instance.PlayEffectAC("regeneration", isHero: true, lowestHealthHero.HeroItem.CharImageT, flip: false);
			}
		}
	}

	public void healingsurge(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || !(castedCard != null))
		{
			return;
		}
		TraitData traitData = Globals.Instance.GetTraitData("healingsurge");
		if ((MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey("healingsurge") && MatchManager.Instance.activatedTraits["healingsurge"] > traitData.TimesPerTurn - 1) || MatchManager.Instance.energyJustWastedByHero <= 0 || !castedCard.GetCardTypes().Contains(Enums.CardType.Healing_Spell) || !(character.HeroData != null))
		{
			return;
		}
		if (!MatchManager.Instance.activatedTraits.ContainsKey("healingsurge"))
		{
			MatchManager.Instance.activatedTraits.Add("healingsurge", 1);
		}
		else
		{
			MatchManager.Instance.activatedTraits["healingsurge"]++;
		}
		MatchManager.Instance.SetTraitInfoText();
		character.ModifyEnergy(1, showScrollCombatText: true);
		if (character.HeroItem != null)
		{
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Healing Surge") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["healingsurge"], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
			EffectsManager.Instance.PlayEffectAC("energy", isHero: true, character.HeroItem.CharImageT, flip: false);
		}
		Hero lowestHealthHero = GetLowestHealthHero(theEvent, character, target, auxInt, auxString, castedCard, trait);
		if (lowestHealthHero != null && lowestHealthHero.Alive)
		{
			lowestHealthHero.SetAuraTrait(character, "bless", 1);
			if (lowestHealthHero.HeroItem != null)
			{
				EffectsManager.Instance.PlayEffectAC("bless", isHero: true, lowestHealthHero.HeroItem.CharImageT, flip: false);
			}
		}
	}

	public void hermit(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (character == null || !character.Alive)
		{
			return;
		}
		bool flag = false;
		NPC[] teamNPC = MatchManager.Instance.GetTeamNPC();
		int num = 0;
		while (!flag)
		{
			num = MatchManager.Instance.GetRandomIntRange(0, teamNPC.Length, "trait");
			if (teamNPC[num] != null && teamNPC[num].NpcData != null && teamNPC[num].Alive)
			{
				teamNPC[num].SetAuraTrait(character, "poison", 1);
				teamNPC[num].SetAuraTrait(character, "rust", 1);
				flag = true;
			}
		}
		teamNPC[num].NPCItem.ScrollCombatText(Texts.Instance.GetText("traits_Hermit"), Enums.CombatScrollEffectType.Trait);
		EffectsManager.Instance.PlayEffectAC("waterimpact1", isHero: true, teamNPC[num].NPCItem.CharImageT, flip: false);
	}

	public void hexmastery(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || !(castedCard != null))
		{
			return;
		}
		TraitData traitData = Globals.Instance.GetTraitData("hexmastery");
		if ((MatchManager.Instance.activatedTraits == null || !MatchManager.Instance.activatedTraits.ContainsKey("hexmastery") || MatchManager.Instance.activatedTraits["hexmastery"] <= traitData.TimesPerTurn - 1) && MatchManager.Instance.energyJustWastedByHero > 0 && castedCard.GetCardTypes().Contains(Enums.CardType.Curse_Spell) && character.HeroData != null)
		{
			if (!MatchManager.Instance.activatedTraits.ContainsKey("hexmastery"))
			{
				MatchManager.Instance.activatedTraits.Add("hexmastery", 1);
			}
			else
			{
				MatchManager.Instance.activatedTraits["hexmastery"]++;
			}
			MatchManager.Instance.SetTraitInfoText();
			character.SetAuraTrait(character, "powerful", 1);
			character.ModifyEnergy(1, showScrollCombatText: true);
			if (character.HeroItem != null)
			{
				character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Hex Mastery") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["hexmastery"], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
				EffectsManager.Instance.PlayEffectAC("powerful", isHero: true, character.HeroItem.CharImageT, flip: false);
				EffectsManager.Instance.PlayEffectAC("energy", isHero: true, character.HeroItem.CharImageT, flip: false);
			}
		}
	}

	public void incantation(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		bool flag = false;
		Hero[] teamHero = MatchManager.Instance.GetTeamHero();
		int num = 0;
		while (!flag)
		{
			num = MatchManager.Instance.GetRandomIntRange(0, teamHero.Length, "trait");
			if (teamHero[num] != null && teamHero[num].HeroData != null && teamHero[num].Alive)
			{
				switch (MatchManager.Instance.GetRandomIntRange(0, 4, "trait"))
				{
				case 0:
					teamHero[num].SetAuraTrait(character, "courage", 2);
					break;
				case 1:
					teamHero[num].SetAuraTrait(character, "insulate", 2);
					break;
				case 2:
					teamHero[num].SetAuraTrait(character, "reinforce", 2);
					break;
				default:
					teamHero[num].SetAuraTrait(character, "energize", 1);
					break;
				}
				flag = true;
			}
		}
		teamHero[num].HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Incantion"), Enums.CombatScrollEffectType.Trait);
	}

	public void incendiary(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (target != null && target.Alive)
		{
			target.SetAuraTrait(character, "burn", 3);
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Incendiary"), Enums.CombatScrollEffectType.Trait);
		}
	}

	public void indomitable(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (character != null && character.Alive)
		{
			character.SetAuraTrait(character, "shield", Functions.FuncRoundToInt((float)auxInt * 0.5f));
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Indomitable"), Enums.CombatScrollEffectType.Trait);
		}
	}

	public void inventor(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		bool flag = false;
		Hero[] teamHero = MatchManager.Instance.GetTeamHero();
		int num = 0;
		while (!flag)
		{
			num = MatchManager.Instance.GetRandomIntRange(0, teamHero.Length, "trait");
			if (teamHero[num] != null && teamHero[num].HeroData != null && teamHero[num].Alive)
			{
				teamHero[num].SetAuraTrait(character, "energize", 1);
				flag = true;
			}
		}
		teamHero[num].HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Inventor"), Enums.CombatScrollEffectType.Trait);
		EffectsManager.Instance.PlayEffectAC("energy", isHero: true, teamHero[num].HeroItem.CharImageT, flip: false);
	}

	public void ironfurnace(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || !(castedCard != null))
		{
			return;
		}
		TraitData traitData = Globals.Instance.GetTraitData("ironfurnace");
		if ((MatchManager.Instance.activatedTraits == null || !MatchManager.Instance.activatedTraits.ContainsKey("ironfurnace") || MatchManager.Instance.activatedTraits["ironfurnace"] <= traitData.TimesPerTurn - 1) && castedCard.GetCardTypes().Contains(Enums.CardType.Attack))
		{
			if (!MatchManager.Instance.activatedTraits.ContainsKey("ironfurnace"))
			{
				MatchManager.Instance.activatedTraits.Add("ironfurnace", 1);
			}
			else
			{
				MatchManager.Instance.activatedTraits["ironfurnace"]++;
			}
			MatchManager.Instance.SetTraitInfoText();
			character.SetAuraTrait(character, "block", 8);
			character.SetAuraTrait(character, "furnace", 1);
			if (character.HeroItem != null)
			{
				character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Iron Furnace") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["ironfurnace"], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
				EffectsManager.Instance.PlayEffectAC("firepower", isHero: true, character.HeroItem.CharImageT, flip: false);
			}
		}
	}

	public void jinx(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (target != null && target.Alive)
		{
			target.SetAuraTrait(character, "dark", 2);
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Jinx"), Enums.CombatScrollEffectType.Trait);
		}
	}

	public void keensenses(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (character.HeroData != null)
		{
			NPC[] teamNPC = MatchManager.Instance.GetTeamNPC();
			for (int i = 0; i < teamNPC.Length; i++)
			{
				if (teamNPC[i] != null && teamNPC[i].Alive)
				{
					teamNPC[i].SetAuraTrait(character, "mark", 1);
				}
			}
		}
		else
		{
			Hero[] teamHero = MatchManager.Instance.GetTeamHero();
			for (int j = 0; j < teamHero.Length; j++)
			{
				if (teamHero[j] != null && teamHero[j].HeroData != null && teamHero[j].Alive)
				{
					teamHero[j].SetAuraTrait(character, "mark", 1);
				}
			}
		}
		character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Keen Senses"), Enums.CombatScrollEffectType.Trait);
	}

	public void lethalshots(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (target != null && target.Alive)
		{
			switch (MatchManager.Instance.GetRandomIntRange(0, 3, "trait"))
			{
			case 0:
				target.SetAuraTrait(character, "bleed", 3);
				break;
			case 1:
				target.SetAuraTrait(character, "poison", 3);
				break;
			default:
				target.SetAuraTrait(character, "vulnerable", 1);
				break;
			}
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Lethal Shots"), Enums.CombatScrollEffectType.Trait);
		}
	}

	public void loadedgun(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(character.HeroData != null))
		{
			return;
		}
		int num = 2;
		List<string> heroHand = MatchManager.Instance.GetHeroHand(character.HeroIndex);
		List<CardData> list = new List<CardData>();
		for (int i = 0; i < heroHand.Count; i++)
		{
			CardData cardData = MatchManager.Instance.GetCardData(heroHand[i]);
			if (cardData.GetCardFinalCost() > 0 && cardData.HasCardType(Enums.CardType.Ranged_Attack))
			{
				list.Add(cardData);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			CardData cardData = list[j];
			cardData.EnergyReductionTemporal += num;
			MatchManager.Instance.UpdateHandCards();
			CardItem cardFromTableByIndex = MatchManager.Instance.GetCardFromTableByIndex(cardData.InternalId);
			cardFromTableByIndex.PlayDissolveParticle();
			cardFromTableByIndex.ShowEnergyModification(-num);
			MatchManager.Instance.CreateLogCardModification(cardData.InternalId, MatchManager.Instance.GetHero(character.HeroIndex));
		}
		if (list.Count > 0)
		{
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Loaded Gun"), Enums.CombatScrollEffectType.Trait);
		}
	}

	public void maledict(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		bool flag = false;
		NPC[] array = null;
		Hero[] array2 = null;
		if (character.HeroItem != null)
		{
			array = MatchManager.Instance.GetTeamNPC();
		}
		else
		{
			array2 = MatchManager.Instance.GetTeamHero();
		}
		int num = 0;
		Character character2 = null;
		while (!flag)
		{
			if (character.HeroItem != null)
			{
				num = MatchManager.Instance.GetRandomIntRange(0, array.Length, "trait");
				character2 = array[num];
			}
			else
			{
				num = MatchManager.Instance.GetRandomIntRange(0, array2.Length, "trait");
				character2 = array2[num];
				if (character2.HeroData == null)
				{
					character2 = null;
				}
			}
			if (character2 != null && character2.Alive)
			{
				switch (MatchManager.Instance.GetRandomIntRange(0, 3, "trait"))
				{
				case 0:
					character2.SetAuraTrait(character, "dark", 2);
					break;
				case 1:
					character2.SetAuraTrait(character, "slow", 1);
					break;
				default:
					character2.SetAuraTrait(character, "vulnerable", 1);
					break;
				}
				flag = true;
			}
		}
		if (character2 != null)
		{
			if (character2.NPCItem != null)
			{
				character2.NPCItem.ScrollCombatText(Texts.Instance.GetText("traits_Maledict"), Enums.CombatScrollEffectType.Trait);
			}
			else
			{
				character2.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Maledict"), Enums.CombatScrollEffectType.Trait);
			}
		}
	}

	public void magicduality(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || !(castedCard != null))
		{
			return;
		}
		TraitData traitData = Globals.Instance.GetTraitData("magicduality");
		if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey("magicduality"))
		{
			int num = traitData.TimesPerTurn - 1;
			if (character.HaveTrait("unholyblight"))
			{
				num++;
			}
			if (MatchManager.Instance.activatedTraits["magicduality"] > num)
			{
				return;
			}
		}
		Enums.CardClass cardClass = Enums.CardClass.None;
		Enums.CardClass cardClass2 = Enums.CardClass.None;
		for (int i = 0; i < 2; i++)
		{
			if (i == 0)
			{
				cardClass = Enums.CardClass.Warrior;
				cardClass2 = Enums.CardClass.Mage;
			}
			else
			{
				cardClass = Enums.CardClass.Mage;
				cardClass2 = Enums.CardClass.Warrior;
			}
			if (castedCard.CardClass != cardClass)
			{
				continue;
			}
			if (MatchManager.Instance.CountHeroHand() == 0 || !(character.HeroData != null))
			{
				break;
			}
			List<CardData> list = new List<CardData>();
			List<string> heroHand = MatchManager.Instance.GetHeroHand(character.HeroIndex);
			int num2 = 0;
			for (int j = 0; j < heroHand.Count; j++)
			{
				CardData cardData = MatchManager.Instance.GetCardData(heroHand[j]);
				if (cardData != null && cardData.CardClass == cardClass2 && character.GetCardFinalCost(cardData) > num2)
				{
					num2 = character.GetCardFinalCost(cardData);
				}
			}
			if (num2 <= 0)
			{
				break;
			}
			for (int k = 0; k < heroHand.Count; k++)
			{
				CardData cardData2 = MatchManager.Instance.GetCardData(heroHand[k]);
				if (cardData2 != null && cardData2.CardClass == cardClass2 && character.GetCardFinalCost(cardData2) >= num2)
				{
					list.Add(cardData2);
				}
			}
			if (list.Count <= 0)
			{
				break;
			}
			CardData cardData3 = null;
			cardData3 = ((list.Count != 1) ? list[MatchManager.Instance.GetRandomIntRange(0, list.Count, "trait")] : list[0]);
			if (cardData3 != null)
			{
				if (!MatchManager.Instance.activatedTraits.ContainsKey("magicduality"))
				{
					MatchManager.Instance.activatedTraits.Add("magicduality", 1);
				}
				else
				{
					MatchManager.Instance.activatedTraits["magicduality"]++;
				}
				MatchManager.Instance.SetTraitInfoText();
				int num3 = 1;
				cardData3.EnergyReductionTemporal += num3;
				MatchManager.Instance.GetCardFromTableByIndex(cardData3.InternalId).ShowEnergyModification(-num3);
				MatchManager.Instance.UpdateHandCards();
				if (!character.HaveTrait("unholyblight"))
				{
					character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Magic Duality") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["magicduality"], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
				}
				else
				{
					character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Magic Duality") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["magicduality"], traitData.TimesPerTurn + 1), Enums.CombatScrollEffectType.Trait);
				}
				MatchManager.Instance.CreateLogCardModification(cardData3.InternalId, MatchManager.Instance.GetHero(character.HeroIndex));
			}
			break;
		}
	}

	public void marksmanship(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (MatchManager.Instance != null && castedCard != null && castedCard.GetCardTypes().Contains(Enums.CardType.Ranged_Attack))
		{
			character.SetAuraTrait(character, "sharp", 2);
			if (character.HeroItem != null)
			{
				character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Marksmanship"), Enums.CombatScrollEffectType.Trait);
				EffectsManager.Instance.PlayEffectAC("sharp", isHero: true, character.HeroItem.CharImageT, flip: false);
			}
		}
	}

	public void mentalist(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (MatchManager.Instance != null && castedCard != null && castedCard.GetCardTypes().Contains(Enums.CardType.Spell))
		{
			character.SetAuraTrait(character, "powerful", 2);
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Mentalist"), Enums.CombatScrollEffectType.Trait);
		}
	}

	public void minddevourer(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		mentalleech(theEvent, character, target, auxInt, auxString, castedCard, trait);
	}

	public void mentalleech(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		bool flag = false;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		Hero[] teamHero = MatchManager.Instance.GetTeamHero();
		NPC[] teamNPC = MatchManager.Instance.GetTeamNPC();
		for (int i = 0; i < teamNPC.Length; i++)
		{
			if (teamNPC[i] != null && teamNPC[i].Alive)
			{
				teamNPC[i].SetAuraTrait(character, "insane", 1);
				if (teamNPC[i].NPCItem != null)
				{
					EffectsManager.Instance.PlayEffectAC("mindimpact2", isHero: true, teamNPC[i].NPCItem.CharImageT, flip: false);
				}
			}
		}
		for (int j = 0; j < teamHero.Length; j++)
		{
			if (teamHero[j] != null && teamHero[j].HeroData != null && teamHero[j].Alive)
			{
				num2 += teamHero[j].GetAuraCharges("insane");
				num3 += teamHero[j].GetAuraCharges("sight");
			}
		}
		for (int k = 0; k < teamNPC.Length; k++)
		{
			if (teamNPC[k] != null && teamNPC[k].Alive)
			{
				num2 += teamNPC[k].GetAuraCharges("insane");
				num3 += teamNPC[k].GetAuraCharges("sight");
			}
		}
		num = Functions.FuncRoundToInt((float)num2 * 0.2f + (float)num3 * 0.1f);
		for (int l = 0; l < teamHero.Length; l++)
		{
			if (teamHero[l] == null || !(teamHero[l].HeroData != null) || !teamHero[l].Alive)
			{
				continue;
			}
			int num4 = teamHero[l].HealReceivedFinal(num);
			int num5 = num4;
			if (teamHero[l].GetHpLeftForMax() < num4)
			{
				num5 = teamHero[l].GetHpLeftForMax();
			}
			if (num5 > 0)
			{
				teamHero[l].ModifyHp(num5, _includeInStats: false);
				AtOManager.Instance.combatStats[character.HeroIndex, 3] += num5;
				AtOManager.Instance.combatStatsCurrent[character.HeroIndex, 3] += num5;
				CastResolutionForCombatText castResolutionForCombatText = new CastResolutionForCombatText();
				castResolutionForCombatText.heal = num5;
				teamHero[l].HeroItem.ScrollCombatTextDamageNew(castResolutionForCombatText);
				teamHero[l].SetEvent(Enums.EventActivation.Healed);
				character.SetEvent(Enums.EventActivation.Heal, teamHero[l]);
				flag = true;
				if (teamHero[l].HeroItem != null)
				{
					EffectsManager.Instance.PlayEffectAC("healimpactsmall", isHero: true, teamHero[l].HeroItem.CharImageT, flip: false);
				}
			}
		}
		if (flag)
		{
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Mentalleech"), Enums.CombatScrollEffectType.Trait);
		}
	}

	public void mindflayer(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (MatchManager.Instance.CountHeroHand() == 10)
		{
			Debug.Log("[TRAIT EXECUTION] Broke because player at max cards");
			return;
		}
		bool flag = false;
		Hero[] teamHero = MatchManager.Instance.GetTeamHero();
		int num = 0;
		while (!flag)
		{
			num = MatchManager.Instance.GetRandomIntRange(0, teamHero.Length, "trait");
			if (teamHero[num] != null && teamHero[num].HeroData != null && teamHero[num].Alive)
			{
				flag = true;
			}
		}
		string text = MatchManager.Instance.CreateCardInDictionary("friendlytadpole");
		MatchManager.Instance.GetCardData(text);
		MatchManager.Instance.GenerateNewCard(1, text, createCard: false, Enums.CardPlace.RandomDeck, null, null, num);
		teamHero[num].HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Mind Flayer"), Enums.CombatScrollEffectType.Trait);
		MatchManager.Instance.ItemTraitActivated();
	}

	public void mojo(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || !(castedCard != null) || !castedCard.GetCardTypes().Contains(Enums.CardType.Healing_Spell))
		{
			return;
		}
		Hero[] teamHero = MatchManager.Instance.GetTeamHero();
		for (int i = 0; i < teamHero.Length; i++)
		{
			if (teamHero[i] != null && teamHero[i].HeroData != null && teamHero[i].Alive)
			{
				teamHero[i].HealCurses(1);
				if (teamHero[i].HeroItem != null)
				{
					teamHero[i].HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Mojo"), Enums.CombatScrollEffectType.Trait);
					EffectsManager.Instance.PlayEffectAC("dispel", isHero: true, teamHero[i].HeroItem.CharImageT, flip: false);
				}
			}
		}
	}

	public void momentum(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || !(castedCard != null))
		{
			return;
		}
		TraitData traitData = Globals.Instance.GetTraitData("momentum");
		if ((MatchManager.Instance.activatedTraits == null || !MatchManager.Instance.activatedTraits.ContainsKey("momentum") || MatchManager.Instance.activatedTraits["momentum"] <= traitData.TimesPerTurn - 1) && MatchManager.Instance.energyJustWastedByHero > 0 && castedCard.GetCardTypes().Contains(Enums.CardType.Melee_Attack) && character.HeroData != null)
		{
			if (!MatchManager.Instance.activatedTraits.ContainsKey("momentum"))
			{
				MatchManager.Instance.activatedTraits.Add("momentum", 1);
			}
			else
			{
				MatchManager.Instance.activatedTraits["momentum"]++;
			}
			MatchManager.Instance.SetTraitInfoText();
			character.SetAuraTrait(character, "powerful", 1);
			character.ModifyEnergy(1, showScrollCombatText: true);
			if (character.HeroItem != null)
			{
				character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Momentum") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["momentum"], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
				EffectsManager.Instance.PlayEffectAC("powerful", isHero: true, character.HeroItem.CharImageT, flip: false);
				EffectsManager.Instance.PlayEffectAC("energy", isHero: true, character.HeroItem.CharImageT, flip: false);
			}
		}
	}

	public void mountedcannon(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (MatchManager.Instance != null && castedCard != null)
		{
			if (MatchManager.Instance.CountHeroHand() == 10)
			{
				Debug.Log("[TRAIT EXECUTION] Broke because player at max cards");
			}
			else if (MatchManager.Instance.energyJustWastedByHero > 0 && castedCard.GetCardTypes().Contains(Enums.CardType.Defense) && character.HeroData != null)
			{
				string id = "blast";
				string text = MatchManager.Instance.CreateCardInDictionary(id);
				CardData cardData = MatchManager.Instance.GetCardData(text);
				MatchManager.Instance.GenerateNewCard(1, text, createCard: false, Enums.CardPlace.Hand);
				character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Mounted Cannon"), Enums.CombatScrollEffectType.Trait);
				MatchManager.Instance.ItemTraitActivated();
				MatchManager.Instance.CreateLogCardModification(cardData.InternalId, MatchManager.Instance.GetHero(character.HeroIndex));
			}
		}
	}

	public void nightstalker(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		character.SetAuraTrait(character, "stealth", 2);
		if (character.HeroItem != null)
		{
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Nightstalker"), Enums.CombatScrollEffectType.Trait);
			EffectsManager.Instance.PlayEffectAC("stealth", isHero: true, character.HeroItem.CharImageT, flip: false);
		}
	}

	public void offensemastery(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(character.HeroData != null))
		{
			return;
		}
		int num = 1;
		if (num <= 0)
		{
			return;
		}
		List<string> heroHand = MatchManager.Instance.GetHeroHand(character.HeroIndex);
		List<CardData> list = new List<CardData>();
		for (int i = 0; i < heroHand.Count; i++)
		{
			CardData cardData = MatchManager.Instance.GetCardData(heroHand[i]);
			if (cardData.GetCardFinalCost() > 0 && cardData.HasCardType(Enums.CardType.Attack))
			{
				list.Add(cardData);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			CardData cardData = list[j];
			cardData.EnergyReductionTemporal += num;
			MatchManager.Instance.UpdateHandCards();
			CardItem cardFromTableByIndex = MatchManager.Instance.GetCardFromTableByIndex(cardData.InternalId);
			cardFromTableByIndex.PlayDissolveParticle();
			cardFromTableByIndex.ShowEnergyModification(-num);
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Offense Mastery"), Enums.CombatScrollEffectType.Trait);
			MatchManager.Instance.CreateLogCardModification(cardData.InternalId, MatchManager.Instance.GetHero(character.HeroIndex));
		}
	}

	public void overflow(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		int heal = Functions.FuncRoundToInt((float)auxInt * 0.35f);
		if (AtOManager.Instance.TeamHaveTrait("beaconoflight"))
		{
			heal = Functions.FuncRoundToInt((float)auxInt * 0.7f);
		}
		heal = target.HealReceivedFinal(heal);
		if (target.GetHpLeftForMax() < heal)
		{
			heal = target.GetHpLeftForMax();
		}
		if (heal > 0)
		{
			target.ModifyHp(heal);
			CastResolutionForCombatText castResolutionForCombatText = new CastResolutionForCombatText();
			castResolutionForCombatText.heal = heal;
			if (target.HeroItem != null)
			{
				target.HeroItem.ScrollCombatTextDamageNew(castResolutionForCombatText);
				EffectsManager.Instance.PlayEffectAC("healimpactsmall", isHero: true, target.HeroItem.CharImageT, flip: false);
			}
			else
			{
				target.NPCItem.ScrollCombatTextDamageNew(castResolutionForCombatText);
				EffectsManager.Instance.PlayEffectAC("healimpactsmall", isHero: true, target.NPCItem.CharImageT, flip: false);
			}
			target.SetEvent(Enums.EventActivation.Healed);
			character.SetEvent(Enums.EventActivation.Heal, target);
			target.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Overflow"), Enums.CombatScrollEffectType.Trait);
		}
	}

	public void pestilent(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (character.HeroData != null)
		{
			character.SetAuraTrait(character, "dark", 1);
			NPC[] teamNPC = MatchManager.Instance.GetTeamNPC();
			for (int i = 0; i < teamNPC.Length; i++)
			{
				if (teamNPC[i] != null && teamNPC[i].Alive)
				{
					teamNPC[i].SetAuraTrait(character, "dark", 1);
					if (teamNPC[i].NPCItem != null)
					{
						teamNPC[i].NPCItem.ScrollCombatText(Texts.Instance.GetText("traits_Pestilent"), Enums.CombatScrollEffectType.Trait);
						EffectsManager.Instance.PlayEffectAC("shadowimpactsmall", isHero: true, teamNPC[i].NPCItem.CharImageT, flip: false);
					}
				}
			}
		}
		else
		{
			Hero[] teamHero = MatchManager.Instance.GetTeamHero();
			for (int j = 0; j < teamHero.Length; j++)
			{
				if (teamHero[j] != null && teamHero[j].HeroData != null && teamHero[j].Alive)
				{
					teamHero[j].SetAuraTrait(character, "dark", 1);
					if (teamHero[j].HeroItem != null)
					{
						teamHero[j].HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Pestilent"), Enums.CombatScrollEffectType.Trait);
						EffectsManager.Instance.PlayEffectAC("shadowimpactsmall", isHero: true, teamHero[j].HeroItem.CharImageT, flip: false);
					}
				}
			}
		}
		if (character.HeroItem != null)
		{
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Pestilent"), Enums.CombatScrollEffectType.Trait);
			EffectsManager.Instance.PlayEffectAC("shadowimpactsmall", isHero: true, character.HeroItem.CharImageT, flip: false);
		}
	}

	public void philosophersstone(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(character.HeroData != null) || !character.Alive)
		{
			return;
		}
		int auraCharges = character.GetAuraCharges("rust");
		auraCharges += 2;
		if (auraCharges <= 0)
		{
			return;
		}
		Hero[] teamHero = MatchManager.Instance.GetTeamHero();
		for (int i = 0; i < teamHero.Length; i++)
		{
			if (teamHero[i] != null && teamHero[i].HeroData != null && teamHero[i].Alive)
			{
				teamHero[i].SetAuraTrait(character, "bless", auraCharges);
				teamHero[i].SetAuraTrait(character, "vitality", auraCharges);
				if (teamHero[i].HeroItem != null)
				{
					EffectsManager.Instance.PlayEffectAC("bless", isHero: true, teamHero[i].HeroItem.CharImageT, flip: false);
				}
			}
		}
		if (character.HeroItem != null)
		{
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Philosophers Stone"), Enums.CombatScrollEffectType.Trait);
		}
	}

	public void premonition(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (character.HeroData != null)
		{
			NPC[] teamNPC = MatchManager.Instance.GetTeamNPC();
			for (int i = 0; i < teamNPC.Length; i++)
			{
				if (teamNPC[i] != null && teamNPC[i].Alive)
				{
					teamNPC[i].SetAuraTrait(character, "sight", 2);
				}
			}
		}
		else
		{
			Hero[] teamHero = MatchManager.Instance.GetTeamHero();
			for (int j = 0; j < teamHero.Length; j++)
			{
				if (teamHero[j] != null && teamHero[j].HeroData != null && teamHero[j].Alive)
				{
					teamHero[j].SetAuraTrait(character, "sight", 2);
				}
			}
		}
		character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Premonition"), Enums.CombatScrollEffectType.Trait);
	}

	public void rangedmastery(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(character.HeroData != null))
		{
			return;
		}
		int num = 1;
		if (num <= 0)
		{
			return;
		}
		List<string> heroHand = MatchManager.Instance.GetHeroHand(character.HeroIndex);
		List<CardData> list = new List<CardData>();
		for (int i = 0; i < heroHand.Count; i++)
		{
			CardData cardData = MatchManager.Instance.GetCardData(heroHand[i]);
			if (cardData.GetCardFinalCost() > 0 && cardData.HasCardType(Enums.CardType.Ranged_Attack))
			{
				list.Add(cardData);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			CardData cardData = list[j];
			cardData.EnergyReductionTemporal += num;
			MatchManager.Instance.UpdateHandCards();
			CardItem cardFromTableByIndex = MatchManager.Instance.GetCardFromTableByIndex(cardData.InternalId);
			cardFromTableByIndex.PlayDissolveParticle();
			cardFromTableByIndex.ShowEnergyModification(-num);
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Ranged Mastery"), Enums.CombatScrollEffectType.Trait);
			MatchManager.Instance.CreateLogCardModification(cardData.InternalId, MatchManager.Instance.GetHero(character.HeroIndex));
		}
	}

	public void resourceful(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || !(castedCard != null))
		{
			return;
		}
		TraitData traitData = Globals.Instance.GetTraitData("resourceful");
		if ((MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey("resourceful") && MatchManager.Instance.activatedTraits["resourceful"] > traitData.TimesPerTurn - 1) || MatchManager.Instance.energyJustWastedByHero <= 0 || !castedCard.GetCardTypes().Contains(Enums.CardType.Skill) || !(character.HeroData != null))
		{
			return;
		}
		if (!MatchManager.Instance.activatedTraits.ContainsKey("resourceful"))
		{
			MatchManager.Instance.activatedTraits.Add("resourceful", 1);
		}
		else
		{
			MatchManager.Instance.activatedTraits["resourceful"]++;
		}
		MatchManager.Instance.SetTraitInfoText();
		character.ModifyEnergy(1, showScrollCombatText: true);
		if (character.HeroItem != null)
		{
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Resourceful") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["resourceful"], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
			EffectsManager.Instance.PlayEffectAC("energy", isHero: true, character.HeroItem.CharImageT, flip: false);
		}
		NPC[] teamNPC = MatchManager.Instance.GetTeamNPC();
		for (int i = 0; i < teamNPC.Length; i++)
		{
			if (teamNPC[i] != null && teamNPC[i].Alive)
			{
				teamNPC[i].SetAuraTrait(character, "sight", 1);
			}
		}
	}

	public void reverberation(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || !(castedCard != null))
		{
			return;
		}
		TraitData traitData = Globals.Instance.GetTraitData("reverberation");
		if ((MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey("reverberation") && MatchManager.Instance.activatedTraits["reverberation"] > traitData.TimesPerTurn - 1) || !castedCard.GetCardTypes().Contains(Enums.CardType.Holy_Spell) || !(character.HeroData != null))
		{
			return;
		}
		List<CardData> list = new List<CardData>();
		List<string> heroHand = MatchManager.Instance.GetHeroHand(character.HeroIndex);
		int num = 0;
		for (int i = 0; i < heroHand.Count; i++)
		{
			CardData cardData = MatchManager.Instance.GetCardData(heroHand[i]);
			if (cardData != null && cardData.GetCardTypes().Contains(Enums.CardType.Holy_Spell) && character.GetCardFinalCost(cardData) > num)
			{
				num = character.GetCardFinalCost(cardData);
			}
		}
		if (num <= 0)
		{
			return;
		}
		for (int j = 0; j < heroHand.Count; j++)
		{
			CardData cardData2 = MatchManager.Instance.GetCardData(heroHand[j]);
			if (cardData2 != null && cardData2.GetCardTypes().Contains(Enums.CardType.Holy_Spell) && character.GetCardFinalCost(cardData2) >= num)
			{
				list.Add(cardData2);
			}
		}
		if (list.Count <= 0)
		{
			return;
		}
		CardData cardData3 = null;
		cardData3 = ((list.Count != 1) ? list[MatchManager.Instance.GetRandomIntRange(0, list.Count, "trait")] : list[0]);
		if (cardData3 != null)
		{
			if (!MatchManager.Instance.activatedTraits.ContainsKey("reverberation"))
			{
				MatchManager.Instance.activatedTraits.Add("reverberation", 1);
			}
			else
			{
				MatchManager.Instance.activatedTraits["reverberation"]++;
			}
			MatchManager.Instance.SetTraitInfoText();
			int num2 = 3;
			cardData3.EnergyReductionTemporal += num2;
			MatchManager.Instance.GetCardFromTableByIndex(cardData3.InternalId).ShowEnergyModification(-num2);
			MatchManager.Instance.UpdateHandCards();
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Reverberation") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["reverberation"], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
			MatchManager.Instance.CreateLogCardModification(cardData3.InternalId, MatchManager.Instance.GetHero(character.HeroIndex));
		}
	}

	public void scholar(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || !(castedCard != null))
		{
			return;
		}
		TraitData traitData = Globals.Instance.GetTraitData("scholar");
		if ((MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey("scholar") && MatchManager.Instance.activatedTraits["scholar"] > traitData.TimesPerTurn - 1) || !castedCard.GetCardTypes().Contains(Enums.CardType.Book) || MatchManager.Instance.CountHeroHand() == 0 || !(character.HeroData != null))
		{
			return;
		}
		List<CardData> list = new List<CardData>();
		List<string> heroHand = MatchManager.Instance.GetHeroHand(character.HeroIndex);
		int num = 0;
		for (int i = 0; i < heroHand.Count; i++)
		{
			CardData cardData = MatchManager.Instance.GetCardData(heroHand[i]);
			if (cardData != null && character.GetCardFinalCost(cardData) > num)
			{
				num = character.GetCardFinalCost(cardData);
			}
		}
		if (num <= 0)
		{
			return;
		}
		for (int j = 0; j < heroHand.Count; j++)
		{
			CardData cardData2 = MatchManager.Instance.GetCardData(heroHand[j]);
			if (cardData2 != null && character.GetCardFinalCost(cardData2) >= num)
			{
				list.Add(cardData2);
			}
		}
		if (list.Count <= 0)
		{
			return;
		}
		CardData cardData3 = null;
		cardData3 = ((list.Count != 1) ? list[MatchManager.Instance.GetRandomIntRange(0, list.Count, "trait")] : list[0]);
		if (cardData3 != null)
		{
			if (!MatchManager.Instance.activatedTraits.ContainsKey("scholar"))
			{
				MatchManager.Instance.activatedTraits.Add("scholar", 1);
			}
			else
			{
				MatchManager.Instance.activatedTraits["scholar"]++;
			}
			MatchManager.Instance.SetTraitInfoText();
			int num2 = 1;
			cardData3.EnergyReductionPermanent += num2;
			MatchManager.Instance.GetCardFromTableByIndex(cardData3.InternalId).ShowEnergyModification(-num2);
			MatchManager.Instance.UpdateHandCards();
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Scholar") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["scholar"], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
			MatchManager.Instance.CreateLogCardModification(cardData3.InternalId, MatchManager.Instance.GetHero(character.HeroIndex));
		}
	}

	public void scoutduality(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || !(castedCard != null))
		{
			return;
		}
		TraitData traitData = Globals.Instance.GetTraitData("scoutduality");
		if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey("scoutduality"))
		{
			int num = traitData.TimesPerTurn - 1;
			if (character.HaveTrait("valhalla"))
			{
				num++;
			}
			if (MatchManager.Instance.activatedTraits["scoutduality"] > num)
			{
				return;
			}
		}
		for (int i = 0; i < 2; i++)
		{
			Enums.CardClass cardClass;
			Enums.CardClass cardClass2;
			if (i == 0)
			{
				cardClass = Enums.CardClass.Scout;
				cardClass2 = Enums.CardClass.Mage;
			}
			else
			{
				cardClass = Enums.CardClass.Mage;
				cardClass2 = Enums.CardClass.Scout;
			}
			if (castedCard.CardClass != cardClass)
			{
				continue;
			}
			if (MatchManager.Instance.CountHeroHand() == 0 || !(character.HeroData != null))
			{
				break;
			}
			List<CardData> list = new List<CardData>();
			List<string> heroHand = MatchManager.Instance.GetHeroHand(character.HeroIndex);
			int num2 = 0;
			for (int j = 0; j < heroHand.Count; j++)
			{
				CardData cardData = MatchManager.Instance.GetCardData(heroHand[j]);
				if (cardData != null && cardData.CardClass == cardClass2 && character.GetCardFinalCost(cardData) > num2)
				{
					num2 = character.GetCardFinalCost(cardData);
				}
			}
			if (num2 <= 0)
			{
				break;
			}
			for (int k = 0; k < heroHand.Count; k++)
			{
				CardData cardData2 = MatchManager.Instance.GetCardData(heroHand[k]);
				if (cardData2 != null && cardData2.CardClass == cardClass2 && character.GetCardFinalCost(cardData2) >= num2)
				{
					list.Add(cardData2);
				}
			}
			if (list.Count <= 0)
			{
				break;
			}
			CardData cardData3 = null;
			cardData3 = ((list.Count != 1) ? list[MatchManager.Instance.GetRandomIntRange(0, list.Count, "trait")] : list[0]);
			if (cardData3 != null)
			{
				if (!MatchManager.Instance.activatedTraits.ContainsKey("scoutduality"))
				{
					MatchManager.Instance.activatedTraits.Add("scoutduality", 1);
				}
				else
				{
					MatchManager.Instance.activatedTraits["scoutduality"]++;
				}
				MatchManager.Instance.SetTraitInfoText();
				int num3 = 1;
				cardData3.EnergyReductionTemporal += num3;
				MatchManager.Instance.GetCardFromTableByIndex(cardData3.InternalId).ShowEnergyModification(-num3);
				MatchManager.Instance.UpdateHandCards();
				if (!character.HaveTrait("valhalla"))
				{
					character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Scout Duality") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["scoutduality"], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
				}
				else
				{
					character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Scout Duality") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["scoutduality"], traitData.TimesPerTurn + 1), Enums.CombatScrollEffectType.Trait);
				}
				MatchManager.Instance.CreateLogCardModification(cardData3.InternalId, MatchManager.Instance.GetHero(character.HeroIndex));
			}
			break;
		}
	}

	public void shielder(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(auxString == "shield") || target == null || !target.IsHero)
		{
			return;
		}
		int heal = Functions.FuncRoundToInt((float)auxInt * 0.3f);
		heal = target.HealReceivedFinal(heal);
		int num = heal;
		if (target.GetHpLeftForMax() < heal)
		{
			num = target.GetHpLeftForMax();
		}
		if (num > 0)
		{
			target.ModifyHp(num);
			CastResolutionForCombatText castResolutionForCombatText = new CastResolutionForCombatText();
			castResolutionForCombatText.heal = num;
			if (target.HeroItem != null)
			{
				target.HeroItem.ScrollCombatTextDamageNew(castResolutionForCombatText);
				EffectsManager.Instance.PlayEffectAC("healimpactsmall", isHero: true, target.HeroItem.CharImageT, flip: false);
			}
			else
			{
				target.NPCItem.ScrollCombatTextDamageNew(castResolutionForCombatText);
				EffectsManager.Instance.PlayEffectAC("healimpactsmall", isHero: true, target.NPCItem.CharImageT, flip: false);
			}
			target.SetEvent(Enums.EventActivation.Healed);
			character.SetEvent(Enums.EventActivation.Heal, target);
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Shielder"), Enums.CombatScrollEffectType.Trait);
		}
	}

	public void shieldexpert(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (MatchManager.Instance != null && castedCard != null && castedCard.GetCardTypes().Contains(Enums.CardType.Attack))
		{
			character.SetAuraTrait(character, "block", 9);
			if (character.HeroItem != null)
			{
				character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Shield Expert"), Enums.CombatScrollEffectType.Trait);
				EffectsManager.Instance.PlayEffectAC("guardsmall", isHero: true, character.HeroItem.CharImageT, flip: false);
			}
		}
	}

	public void ragnarok(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!character.HaveTrait("spellsinger"))
		{
			spellsinger(theEvent, character, target, auxInt, auxString, castedCard, trait);
		}
	}

	public void spellsinger(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || !(castedCard != null))
		{
			return;
		}
		int num = Globals.Instance.GetTraitData("spellsinger").TimesPerTurn - 1;
		if (character.HaveTrait("spellsinger") && character.HaveTrait("ragnarok"))
		{
			num++;
		}
		if ((MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey("spellsinger") && MatchManager.Instance.activatedTraits["spellsinger"] > num) || !castedCard.GetCardTypes().Contains(Enums.CardType.Song))
		{
			return;
		}
		List<string> list = new List<string>();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Enum.GetName(typeof(Enums.HeroClass), Enums.HeroClass.Mage));
		stringBuilder.Append("_");
		stringBuilder.Append(Enum.GetName(typeof(Enums.CardType), Enums.CardType.Fire_Spell));
		for (int i = 0; i < Globals.Instance.CardListByClassType[stringBuilder.ToString()].Count; i++)
		{
			list.Add(Globals.Instance.CardListByClassType[stringBuilder.ToString()][i]);
		}
		stringBuilder.Clear();
		stringBuilder.Append(Enum.GetName(typeof(Enums.HeroClass), Enums.HeroClass.Mage));
		stringBuilder.Append("_");
		stringBuilder.Append(Enum.GetName(typeof(Enums.CardType), Enums.CardType.Lightning_Spell));
		for (int j = 0; j < Globals.Instance.CardListByClassType[stringBuilder.ToString()].Count; j++)
		{
			list.Add(Globals.Instance.CardListByClassType[stringBuilder.ToString()][j]);
		}
		stringBuilder.Clear();
		stringBuilder.Append(Enum.GetName(typeof(Enums.HeroClass), Enums.HeroClass.Mage));
		stringBuilder.Append("_");
		stringBuilder.Append(Enum.GetName(typeof(Enums.CardType), Enums.CardType.Cold_Spell));
		for (int k = 0; k < Globals.Instance.CardListByClassType[stringBuilder.ToString()].Count; k++)
		{
			list.Add(Globals.Instance.CardListByClassType[stringBuilder.ToString()][k]);
		}
		int num2 = MatchManager.Instance.energyJustWastedByHero + 1;
		if (character.HaveTrait("spellsinger") && character.HaveTrait("ragnarok"))
		{
			if (character.EffectCharges("stanzaii") > 0)
			{
				num2++;
			}
			else if (character.EffectCharges("stanzaiii") > 0)
			{
				num2 += 2;
			}
		}
		if (num2 > 10)
		{
			num2 = 10;
		}
		bool flag = false;
		string id = "";
		int num3 = 0;
		while (!flag && num3 < 500)
		{
			int randomIntRange = MatchManager.Instance.GetRandomIntRange(0, list.Count, "trait");
			id = list[randomIntRange];
			if (Globals.Instance.GetCardData(id, instantiate: false).EnergyCostOriginal == num2)
			{
				flag = true;
				break;
			}
			num3++;
		}
		string text = MatchManager.Instance.CreateCardInDictionary(id);
		CardData cardData = MatchManager.Instance.GetCardData(text);
		cardData.Vanish = true;
		cardData.EnergyReductionToZeroPermanent = true;
		MatchManager.Instance.GenerateNewCard(1, text, createCard: false, Enums.CardPlace.Hand);
		if (character.HeroItem != null)
		{
			if (!MatchManager.Instance.activatedTraits.ContainsKey("spellsinger"))
			{
				MatchManager.Instance.activatedTraits.Add("spellsinger", 1);
			}
			else
			{
				MatchManager.Instance.activatedTraits["spellsinger"]++;
			}
			MatchManager.Instance.SetTraitInfoText();
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Spell Singer") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["spellsinger"], num + 1), Enums.CombatScrollEffectType.Trait);
		}
		MatchManager.Instance.ItemTraitActivated();
		MatchManager.Instance.CreateLogCardModification(cardData.InternalId, MatchManager.Instance.GetHero(character.HeroIndex));
	}

	public void spiky(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		character.SetAuraTrait(character, "thorns", 5);
		if (character.HeroItem != null)
		{
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Spiky"), Enums.CombatScrollEffectType.Trait);
			EffectsManager.Instance.PlayEffectAC("thorns", isHero: true, character.HeroItem.CharImageT, flip: false);
		}
	}

	public void tactician(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(character.HeroData != null))
		{
			return;
		}
		int num = 1;
		if (num <= 0)
		{
			return;
		}
		List<string> heroHand = MatchManager.Instance.GetHeroHand(character.HeroIndex);
		List<CardData> list = new List<CardData>();
		for (int i = 0; i < heroHand.Count; i++)
		{
			CardData cardData = MatchManager.Instance.GetCardData(heroHand[i]);
			if (cardData.GetCardFinalCost() > 0 && cardData.HasCardType(Enums.CardType.Skill))
			{
				list.Add(cardData);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			CardData cardData = list[j];
			cardData.EnergyReductionTemporal += num;
			MatchManager.Instance.UpdateHandCards();
			CardItem cardFromTableByIndex = MatchManager.Instance.GetCardFromTableByIndex(cardData.InternalId);
			cardFromTableByIndex.PlayDissolveParticle();
			cardFromTableByIndex.ShowEnergyModification(-num);
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Tactician"), Enums.CombatScrollEffectType.Trait);
			MatchManager.Instance.CreateLogCardModification(cardData.InternalId, MatchManager.Instance.GetHero(character.HeroIndex));
		}
	}

	public void temperate(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		character.SetAuraTrait(character, "Insulate", 1);
		if (character.HeroItem != null)
		{
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Temperate"), Enums.CombatScrollEffectType.Trait);
			EffectsManager.Instance.PlayEffectAC("insulate", isHero: true, character.HeroItem.CharImageT, flip: false);
		}
	}

	public void trailblazer(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		character.SetAuraTrait(character, "Fast", 1);
		if (character.HeroItem != null)
		{
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Trailblazer"), Enums.CombatScrollEffectType.Trait);
			EffectsManager.Instance.PlayEffectAC("speed1", isHero: true, character.HeroItem.CharImageT, flip: false);
		}
	}

	public void trickster(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (MatchManager.Instance != null && castedCard != null && castedCard.GetCardTypes().Contains(Enums.CardType.Song))
		{
			character.SetAuraTrait(character, "sharp", 2);
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Trickster"), Enums.CombatScrollEffectType.Trait);
		}
	}

	public void unbreakable(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		Hero[] teamHero = MatchManager.Instance.GetTeamHero();
		for (int i = 0; i < teamHero.Length; i++)
		{
			if (teamHero[i] != null && teamHero[i].HeroData != null && teamHero[i].Alive)
			{
				teamHero[i].SetAuraTrait(character, "block", 12);
				teamHero[i].SetAuraTrait(character, "fortify", 1);
				if (teamHero[i].HeroItem != null)
				{
					EffectsManager.Instance.PlayEffectAC("intercept", isHero: true, teamHero[i].HeroItem.CharImageT, flip: false);
				}
			}
		}
		character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Unbreakable"), Enums.CombatScrollEffectType.Trait);
	}

	public void unlimitedblades(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (character.HeroData != null)
		{
			int num = character.EffectCharges("spellsword");
			if (num < 3)
			{
				character.SetAuraTrait(character, "spellsword", 3 - num);
			}
		}
		if (character.HeroItem != null)
		{
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Unlimited blades"), Enums.CombatScrollEffectType.Trait);
		}
	}

	public void vampirism(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (character != null && character.GetHp() > 0)
		{
			int heal = Functions.FuncRoundToInt((float)auxInt * 0.3f);
			Enums.CardClass cC = Enums.CardClass.None;
			if (castedCard != null)
			{
				cC = castedCard.CardClass;
			}
			heal = character.HealWithCharacterBonus(heal, cC);
			heal = character.HealReceivedFinal(heal);
			character.ModifyHp(heal);
			CastResolutionForCombatText castResolutionForCombatText = new CastResolutionForCombatText();
			castResolutionForCombatText.heal = heal;
			if (character.HeroItem != null)
			{
				character.HeroItem.ScrollCombatTextDamageNew(castResolutionForCombatText);
			}
			else if (character.NPCItem != null)
			{
				character.NPCItem.ScrollCombatTextDamageNew(castResolutionForCombatText);
			}
		}
	}

	public void veilofshadows(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null))
		{
			return;
		}
		TraitData traitData = Globals.Instance.GetTraitData("veilofshadows");
		if ((MatchManager.Instance.activatedTraits == null || !MatchManager.Instance.activatedTraits.ContainsKey("veilofshadows") || MatchManager.Instance.activatedTraits["veilofshadows"] <= traitData.TimesPerTurn - 1) && character.HeroData != null)
		{
			if (!MatchManager.Instance.activatedTraits.ContainsKey("veilofshadows"))
			{
				MatchManager.Instance.activatedTraits.Add("veilofshadows", 1);
			}
			else
			{
				MatchManager.Instance.activatedTraits["veilofshadows"]++;
			}
			MatchManager.Instance.SetTraitInfoText();
			if (character.HeroItem != null)
			{
				character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Veilofshadows") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["veilofshadows"], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
			}
		}
	}

	public void versatile(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (MatchManager.Instance != null && castedCard != null)
		{
			bool flag = false;
			_ = castedCard.InternalId;
			if (castedCard.GetCardTypes().Contains(Enums.CardType.Fire_Spell))
			{
				flag = true;
				character.SetAuraTrait(character, "powerful", 2);
			}
			if (castedCard.GetCardTypes().Contains(Enums.CardType.Cold_Spell))
			{
				flag = true;
				character.SetAuraTrait(character, "block", 6);
			}
			if (castedCard.GetCardTypes().Contains(Enums.CardType.Lightning_Spell))
			{
				flag = true;
				character.HealCurses(1);
			}
			if (flag)
			{
				character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Versatile"), Enums.CombatScrollEffectType.Trait);
			}
		}
	}

	public void voodoo(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(auxString == "dark"))
		{
			return;
		}
		Hero lowestHealthHero = GetLowestHealthHero(theEvent, character, target, auxInt, auxString, castedCard, trait);
		if (lowestHealthHero == null || !lowestHealthHero.Alive)
		{
			return;
		}
		int num = lowestHealthHero.HealReceivedFinal(auxInt);
		int num2 = num;
		if (lowestHealthHero.GetHpLeftForMax() < num)
		{
			num2 = lowestHealthHero.GetHpLeftForMax();
		}
		if (num2 > 0)
		{
			lowestHealthHero.ModifyHp(num2, _includeInStats: false);
			AtOManager.Instance.combatStats[character.HeroIndex, 3] += num2;
			AtOManager.Instance.combatStatsCurrent[character.HeroIndex, 3] += num2;
			CastResolutionForCombatText castResolutionForCombatText = new CastResolutionForCombatText();
			castResolutionForCombatText.heal = num2;
			lowestHealthHero.HeroItem.ScrollCombatTextDamageNew(castResolutionForCombatText);
			lowestHealthHero.SetEvent(Enums.EventActivation.Healed);
			character.SetEvent(Enums.EventActivation.Heal, lowestHealthHero);
			if (lowestHealthHero.HeroItem != null)
			{
				EffectsManager.Instance.PlayEffectAC("healimpactsmall", isHero: true, lowestHealthHero.HeroItem.CharImageT, flip: false);
			}
		}
	}

	public void warmaiden(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (character != null && character.Alive)
		{
			int num = character.EffectCharges("stanzaiii");
			int num2 = character.EffectCharges("stanzaii");
			if (num > 0)
			{
				character.SetAuraTrait(character, "powerful", 3);
			}
			else if (num2 > 0)
			{
				character.SetAuraTrait(character, "powerful", 2);
			}
			else
			{
				character.SetAuraTrait(character, "powerful", 1);
			}
			if (character.HeroItem != null)
			{
				character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Warmaiden"), Enums.CombatScrollEffectType.Trait);
				EffectsManager.Instance.PlayEffectAC("powerful", isHero: true, character.HeroItem.CharImageT, flip: false);
			}
		}
	}

	public void warriorduality(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || !(castedCard != null))
		{
			return;
		}
		TraitData traitData = Globals.Instance.GetTraitData("warriorduality");
		if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey("warriorduality"))
		{
			int num = traitData.TimesPerTurn - 1;
			if (character.HaveTrait("beaconoflight"))
			{
				num++;
			}
			if (MatchManager.Instance.activatedTraits["warriorduality"] > num)
			{
				return;
			}
		}
		Enums.CardClass cardClass = Enums.CardClass.None;
		Enums.CardClass cardClass2 = Enums.CardClass.None;
		for (int i = 0; i < 2; i++)
		{
			if (i == 0)
			{
				cardClass = Enums.CardClass.Warrior;
				cardClass2 = Enums.CardClass.Healer;
			}
			else
			{
				cardClass = Enums.CardClass.Healer;
				cardClass2 = Enums.CardClass.Warrior;
			}
			if (castedCard.CardClass != cardClass)
			{
				continue;
			}
			if (MatchManager.Instance.CountHeroHand() == 0 || !(character.HeroData != null))
			{
				break;
			}
			List<CardData> list = new List<CardData>();
			List<string> heroHand = MatchManager.Instance.GetHeroHand(character.HeroIndex);
			int num2 = 0;
			for (int j = 0; j < heroHand.Count; j++)
			{
				CardData cardData = MatchManager.Instance.GetCardData(heroHand[j]);
				if (cardData != null && cardData.CardClass == cardClass2 && character.GetCardFinalCost(cardData) > num2)
				{
					num2 = character.GetCardFinalCost(cardData);
				}
			}
			if (num2 <= 0)
			{
				break;
			}
			for (int k = 0; k < heroHand.Count; k++)
			{
				CardData cardData2 = MatchManager.Instance.GetCardData(heroHand[k]);
				if (cardData2 != null && cardData2.CardClass == cardClass2 && character.GetCardFinalCost(cardData2) >= num2)
				{
					list.Add(cardData2);
				}
			}
			if (list.Count <= 0)
			{
				break;
			}
			CardData cardData3 = null;
			cardData3 = ((list.Count != 1) ? list[MatchManager.Instance.GetRandomIntRange(0, list.Count, "trait")] : list[0]);
			if (cardData3 != null)
			{
				if (!MatchManager.Instance.activatedTraits.ContainsKey("warriorduality"))
				{
					MatchManager.Instance.activatedTraits.Add("warriorduality", 1);
				}
				else
				{
					MatchManager.Instance.activatedTraits["warriorduality"]++;
				}
				MatchManager.Instance.SetTraitInfoText();
				int num3 = 1;
				cardData3.EnergyReductionTemporal += num3;
				MatchManager.Instance.GetCardFromTableByIndex(cardData3.InternalId).ShowEnergyModification(-num3);
				MatchManager.Instance.UpdateHandCards();
				if (!character.HaveTrait("beaconoflight"))
				{
					character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Warrior Duality") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["warriorduality"], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
				}
				else
				{
					character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Warrior Duality") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["warriorduality"], traitData.TimesPerTurn + 1), Enums.CombatScrollEffectType.Trait);
				}
				MatchManager.Instance.CreateLogCardModification(cardData3.InternalId, MatchManager.Instance.GetHero(character.HeroIndex));
			}
			break;
		}
	}

	public void weaponexpert(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
	}

	public void webweaver(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || MatchManager.Instance.GetCurrentRound() != 1)
		{
			return;
		}
		NPC[] teamNPC = MatchManager.Instance.GetTeamNPC();
		for (int i = 0; i < teamNPC.Length; i++)
		{
			if (teamNPC[i] != null && teamNPC[i].Alive)
			{
				teamNPC[i].SetAuraTrait(character, "insane", 6);
				teamNPC[i].SetAuraTrait(character, "poison", 6);
				teamNPC[i].SetAuraTrait(character, "shackle", 1);
				EffectsManager.Instance.PlayEffectAC("poisonneuro", isHero: true, teamNPC[i].NPCItem.CharImageT, flip: false);
			}
		}
		character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Webweaver"), Enums.CombatScrollEffectType.Trait);
	}

	public void welltrained(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		character.SetAuraTrait(character, "reinforce", 2);
		if (character.HeroItem != null)
		{
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Well Trained"), Enums.CombatScrollEffectType.Trait);
			EffectsManager.Instance.PlayEffectAC("reinforce", isHero: true, character.HeroItem.CharImageT, flip: false);
		}
	}

	public void widesleeves(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		if (!(MatchManager.Instance != null) || !(castedCard != null))
		{
			return;
		}
		TraitData traitData = Globals.Instance.GetTraitData("widesleeves");
		if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey("widesleeves") && MatchManager.Instance.activatedTraits["widesleeves"] > traitData.TimesPerTurn - 1)
		{
			return;
		}
		if (MatchManager.Instance.CountHeroHand() == 10)
		{
			Debug.Log("[TRAIT EXECUTION] Broke because player at max cards");
		}
		else if (castedCard.GetCardTypes().Contains(Enums.CardType.Small_Weapon) && character.HeroData != null)
		{
			if (!MatchManager.Instance.activatedTraits.ContainsKey("widesleeves"))
			{
				MatchManager.Instance.activatedTraits.Add("widesleeves", 1);
			}
			else
			{
				MatchManager.Instance.activatedTraits["widesleeves"]++;
			}
			MatchManager.Instance.SetTraitInfoText();
			int randomIntRange = MatchManager.Instance.GetRandomIntRange(0, 100, "trait");
			int randomIntRange2 = MatchManager.Instance.GetRandomIntRange(0, Globals.Instance.CardListByType[Enums.CardType.Small_Weapon].Count, "trait");
			string id = (id = Globals.Instance.CardListByType[Enums.CardType.Small_Weapon][randomIntRange2]);
			id = Functions.GetCardByRarity(randomIntRange, Globals.Instance.GetCardData(id, instantiate: false));
			string text = MatchManager.Instance.CreateCardInDictionary(id);
			CardData cardData = MatchManager.Instance.GetCardData(text);
			cardData.Vanish = true;
			cardData.EnergyReductionToZeroPermanent = true;
			MatchManager.Instance.GenerateNewCard(1, text, createCard: false, Enums.CardPlace.Hand);
			character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Wide Sleeves") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits["widesleeves"], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
			MatchManager.Instance.ItemTraitActivated();
			MatchManager.Instance.CreateLogCardModification(cardData.InternalId, MatchManager.Instance.GetHero(character.HeroIndex));
		}
	}

	private Hero GetLowestHealthHero(Enums.EventActivation theEvent, Character character, Character target, int auxInt, string auxString, CardData castedCard, string trait)
	{
		int num = -1;
		float num2 = 99.9999f;
		Hero[] teamHero = MatchManager.Instance.GetTeamHero();
		for (int i = 0; i < teamHero.Length; i++)
		{
			if (teamHero[i] != null && teamHero[i].HeroData != null && teamHero[i].Alive)
			{
				float hpPercent = teamHero[i].GetHpPercent();
				if (hpPercent <= num2)
				{
					num2 = hpPercent;
					num = i;
				}
			}
		}
		if (num > -1)
		{
			List<int> list = new List<int>();
			list.Add(num);
			for (int j = 0; j < teamHero.Length; j++)
			{
				if (j != num && teamHero[j] != null && teamHero[j].HeroData != null && teamHero[j].Alive && teamHero[j].GetHpPercent() == num2)
				{
					list.Add(j);
				}
			}
			if (list.Count > 0)
			{
				for (int k = 0; k < 10; k++)
				{
					num = ((list.Count <= 1) ? list[0] : list[MatchManager.Instance.GetRandomIntRange(0, list.Count, "trait")]);
					if (k == 9)
					{
						num = list[0];
					}
					if (num < teamHero.Length && teamHero[num] != null && teamHero[num].HeroData != null)
					{
						return teamHero[num];
					}
				}
			}
		}
		return null;
	}
}
