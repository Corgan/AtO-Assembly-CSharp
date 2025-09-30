// Decompiled with JetBrains decompiler
// Type: Trait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

#nullable disable
public class Trait
{
  private Enums.EventActivation theEvent;
  private Character character;
  private Character target;
  private int auxInt;
  private string auxString = "";
  private CardData castedCard;
  private string trait = "";
  private int leechAppliedByBloodBound;

  public void DoTrait(
    Enums.EventActivation _theEvent,
    string _trait,
    Character _character,
    Character _target,
    int _auxInt,
    string _auxString,
    CardData _castedCard)
  {
    if ((UnityEngine.Object) MatchManager.Instance == (UnityEngine.Object) null)
      return;
    UnityEngine.Random.InitState(DateTime.Now.Millisecond);
    this.character = _character;
    this.target = _target;
    this.theEvent = _theEvent;
    this.auxInt = _auxInt;
    this.auxString = _auxString;
    this.castedCard = _castedCard;
    this.trait = _trait;
    MethodInfo method = this.GetType().GetMethod(_trait);
    if (!(method != (MethodInfo) null))
      return;
    method.Invoke((object) this, (object[]) null);
  }

  public void bloodbound()
  {
    if (!(this.auxString.ToLower() == "bleed") || this.target.IsHero)
      return;
    this.target.SetAuraTrait(this.character, "leech", 1);
  }

  public void runeweaver()
  {
    if (this.auxInt < 2)
      return;
    CardData cardData = MatchManager.Instance.GetCardData(this.auxString);
    if (this.isAttackCard(cardData))
    {
      this.character.SetAuraTrait(this.character, "runered", this.auxInt / 2);
      EffectsManager.Instance.PlayEffectAC("runered", true, this.character.HeroItem.CharImageT, false, casterInCenter: cardData.MoveToCenter);
    }
    if (cardData.CardType == Enums.CardType.Defense || ((IEnumerable<Enums.CardType>) cardData.CardTypeAux).Contains<Enums.CardType>(Enums.CardType.Defense))
    {
      this.character.SetAuraTrait(this.character, "runeblue", this.auxInt / 2);
      EffectsManager.Instance.PlayEffectAC("runeblue", true, this.character.HeroItem.CharImageT, false);
    }
    if (cardData.CardType != Enums.CardType.Healing_Spell && !((IEnumerable<Enums.CardType>) cardData.CardTypeAux).Contains<Enums.CardType>(Enums.CardType.Healing_Spell))
      return;
    this.character.SetAuraTrait(this.character, "runegreen", this.auxInt / 2);
    EffectsManager.Instance.PlayEffectAC("runegreen", true, this.character.HeroItem.CharImageT, false);
  }

  private bool isAttackCard(CardData cardData)
  {
    return (bool) (UnityEngine.Object) cardData && (cardData.CardType == Enums.CardType.Attack || ((IEnumerable<Enums.CardType>) cardData.CardTypeAux).Contains<Enums.CardType>(Enums.CardType.Attack) || cardData.CardType == Enums.CardType.Lightning_Spell || ((IEnumerable<Enums.CardType>) cardData.CardTypeAux).Contains<Enums.CardType>(Enums.CardType.Lightning_Spell) || cardData.CardType == Enums.CardType.Fire_Spell || ((IEnumerable<Enums.CardType>) cardData.CardTypeAux).Contains<Enums.CardType>(Enums.CardType.Fire_Spell) || cardData.CardType == Enums.CardType.Cold_Spell || ((IEnumerable<Enums.CardType>) cardData.CardTypeAux).Contains<Enums.CardType>(Enums.CardType.Cold_Spell) || cardData.CardType == Enums.CardType.Shadow_Spell || ((IEnumerable<Enums.CardType>) cardData.CardTypeAux).Contains<Enums.CardType>(Enums.CardType.Shadow_Spell) || cardData.CardType == Enums.CardType.Curse_Spell || ((IEnumerable<Enums.CardType>) cardData.CardTypeAux).Contains<Enums.CardType>(Enums.CardType.Curse_Spell));
  }

  private bool isHealCard(CardData cardData)
  {
    return (bool) (UnityEngine.Object) cardData && (cardData.CardType == Enums.CardType.Healing_Spell || ((IEnumerable<Enums.CardType>) cardData.CardTypeAux).Contains<Enums.CardType>(Enums.CardType.Healing_Spell));
  }

  public void crimsonripple()
  {
    if (!(this.auxString.ToLower() == "bleed") || this.target.IsHero)
      return;
    foreach (NPC npcSide in MatchManager.Instance.GetNPCSides(this.target.Position))
    {
      npcSide.SetAuraTrait(this.character, "burn", 1);
      npcSide.SetAuraTrait(this.character, "dark", 1);
    }
  }

  public void wrathfulresonance()
  {
    this.DeathKnightResonance("runered", new Func<CardData, bool>(this.isAttackCard));
  }

  private void DeathKnightResonance(string rune, Func<CardData, bool> CardTypeCondition)
  {
    TraitData traitData = Globals.Instance.GetTraitData(this.trait);
    if (MatchManager.Instance.activatedTraits.ContainsKey(this.trait) && MatchManager.Instance.activatedTraits[this.trait] >= traitData.TimesPerTurn)
      return;
    if (this.theEvent == Enums.EventActivation.DrawCard)
    {
      CardData cardData = MatchManager.Instance.GetCardData(this.auxString, false);
      if (CardTypeCondition(cardData) && this.character.GetAuraCharges(rune) >= 3)
      {
        ++cardData.EnergyReductionTemporal;
        MatchManager.Instance.UpdateHandCards();
        CardItem fromTableByIndex = MatchManager.Instance.GetCardFromTableByIndex(this.auxString);
        if ((bool) (UnityEngine.Object) fromTableByIndex)
        {
          fromTableByIndex.PlayDissolveParticle();
          fromTableByIndex.ShowEnergyModification(-1);
        }
      }
    }
    if (this.theEvent == Enums.EventActivation.CastCard)
    {
      CardData cardData = Globals.Instance.GetCardData(this.auxString, false);
      if (CardTypeCondition(cardData) && this.character.GetAuraCharges(rune) >= 3)
      {
        if (!MatchManager.Instance.activatedTraits.ContainsKey(this.trait))
          MatchManager.Instance.activatedTraits.Add(this.trait, 1);
        else
          MatchManager.Instance.activatedTraits[this.trait]++;
      }
      if (MatchManager.Instance.activatedTraits.ContainsKey(this.trait) && MatchManager.Instance.activatedTraits[this.trait] >= traitData.TimesPerTurn)
        this.ReduceDeckCost(-1, false, CardTypeCondition);
    }
    if (this.theEvent == Enums.EventActivation.BeginTurn)
    {
      if (!MatchManager.Instance.activatedTraits.ContainsKey(this.trait))
        MatchManager.Instance.activatedTraits.Add(this.trait, 0);
      else
        MatchManager.Instance.activatedTraits[this.trait] = 0;
    }
    if ((this.theEvent == traitData.Activation && this.character.GetAuraCharges(rune) >= 3 || this.theEvent == Enums.EventActivation.AuraCurseSet && this.auxString == rune && this.character.GetAuraCharges(rune) == 2) && MatchManager.Instance.activatedTraits[this.trait] <= traitData.TimesPerTurn)
      this.ReduceDeckCost(1, true, CardTypeCondition);
    MatchManager.Instance.SetTraitInfoText();
  }

  private void ReduceDeckCost(
    int reduction,
    bool playParticles,
    Func<CardData, bool> CardTypeCondition)
  {
    List<string> heroDeck = MatchManager.Instance.GetHeroDeck(this.character.HeroIndex);
    List<string> heroHand = MatchManager.Instance.GetHeroHand(this.character.HeroIndex);
    List<CardData> cardDataList = new List<CardData>();
    for (int index = 0; index < heroDeck.Count; ++index)
    {
      CardData cardData = MatchManager.Instance.GetCardData(heroDeck[index]);
      if (cardData.GetCardFinalCost() > 0)
        cardDataList.Add(cardData);
    }
    for (int index = 0; index < heroHand.Count; ++index)
    {
      CardData cardData = MatchManager.Instance.GetCardData(heroHand[index]);
      if (cardData.GetCardFinalCost() > 0)
        cardDataList.Add(cardData);
    }
    for (int index = 0; index < cardDataList.Count; ++index)
    {
      CardData cardData = cardDataList[index];
      if (CardTypeCondition(cardData))
      {
        cardData.EnergyReductionTemporal += reduction;
        MatchManager.Instance.UpdateHandCards();
        CardItem fromTableByIndex = MatchManager.Instance.GetCardFromTableByIndex(cardData.InternalId);
        if (playParticles && (bool) (UnityEngine.Object) fromTableByIndex)
        {
          fromTableByIndex.PlayDissolveParticle();
          fromTableByIndex.ShowEnergyModification(-reduction);
        }
        this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText(this.trait == "wrathfulresonance" ? "traits_Wrathful Resonance" : "traits_Vital Resonance"), Enums.CombatScrollEffectType.Trait);
        MatchManager.Instance.CreateLogCardModification(cardData.InternalId, MatchManager.Instance.GetHero(this.character.HeroIndex));
      }
    }
  }

  public void vitalresonance()
  {
    this.DeathKnightResonance("runegreen", new Func<CardData, bool>(this.isHealCard));
  }

  public void ApplyVitalResonance()
  {
    Hero[] teamHero = MatchManager.Instance.GetTeamHero();
    EffectsManager.Instance.PlayEffectAC("healerself", true, this.character.HeroItem.CharImageT, false);
    foreach (Hero hero in teamHero)
    {
      if (hero != null && hero.Alive)
      {
        if (hero.CacheGetTraitHealReceivedPercentBonus.Count > 0)
          hero.CacheGetTraitHealReceivedPercentBonus[0] += 20f;
        else
          hero.CacheGetTraitHealReceivedPercentBonus.Add(20f);
        hero.SetAuraTrait(this.character, "block", 10);
        hero.SetAuraTrait(this.character, "vitality", 2);
        EffectsManager.Instance.PlayEffectAC("shield2", true, hero.HeroItem.CharImageT, false);
      }
    }
  }

  public void bloodpact()
  {
  }

  public void runicfocus() => Globals.Instance.GetTraitData("wrathfulresonance").TimesPerTurn = 4;

  public void runicpower() => Globals.Instance.GetTraitData("vitalresonance").TimesPerTurn = 4;

  public void satiatingblood()
  {
  }

  public void accurateshots()
  {
    if (this.target == null || !this.target.Alive)
      return;
    this.target.SetAuraTrait(this.character, "sight", 2);
    this.target.SetAuraTrait(this.character, "bleed", 1);
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Accurate Shots"), Enums.CombatScrollEffectType.Trait);
  }

  public void ardent()
  {
    this.character.SetAuraTrait(this.character, "powerful", 2);
    this.character.SetAuraTrait(this.character, "burn", 2);
    if (!((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null))
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Ardent"), Enums.CombatScrollEffectType.Trait);
    EffectsManager.Instance.PlayEffectAC("powerful", true, this.character.HeroItem.CharImageT, false);
    EffectsManager.Instance.PlayEffectAC("burnsmall", true, this.character.HeroItem.CharImageT, false);
  }

  public void blessed()
  {
    this.character.SetAuraTrait(this.character, "bless", 1);
    if (!((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null))
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Blessed"), Enums.CombatScrollEffectType.Trait);
    EffectsManager.Instance.PlayEffectAC("bless", true, this.character.HeroItem.CharImageT, false);
  }

  public void bloody()
  {
    if (this.target == null || !this.target.Alive)
      return;
    this.target.SetAuraTrait(this.character, "bleed", 2);
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Bloody"), Enums.CombatScrollEffectType.Trait);
  }

  public void broodmother()
  {
    Hero[] teamHero = MatchManager.Instance.GetTeamHero();
    string _id = "spiderling";
    for (int index = 0; index < teamHero.Length; ++index)
    {
      if (this.character.HaveTrait("templelurkers"))
      {
        _id = !this.character.HaveTrait("spiderqueen") ? "templelurker" : "templelurkerrare";
        break;
      }
      if (this.character.HaveTrait("mentalscavengers"))
      {
        _id = !this.character.HaveTrait("spiderqueen") ? "mentalscavengers" : "mentalscavengersrare";
        break;
      }
    }
    for (int index = 0; index < teamHero.Length; ++index)
    {
      if (teamHero[index] != null && (UnityEngine.Object) teamHero[index].HeroData != (UnityEngine.Object) null && teamHero[index].Alive)
      {
        string cardInDictionary = MatchManager.Instance.CreateCardInDictionary(_id);
        MatchManager.Instance.GetCardData(cardInDictionary);
        MatchManager.Instance.GenerateNewCard(1, cardInDictionary, false, Enums.CardPlace.RandomDeck, heroIndex: teamHero[index].HeroIndex);
      }
    }
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_BroodMother"), Enums.CombatScrollEffectType.Trait);
    MatchManager.Instance.ItemTraitActivated();
  }

  public void butcher()
  {
    if (this.target == null || this.target.IsHero || !MatchManager.Instance.AnyNPCAlive())
      return;
    if (!this.character.HaveTrait("threestarchef"))
    {
      switch (MatchManager.Instance.GetRandomIntRange(0, 3, "trait"))
      {
        case 0:
          string cardInDictionary1 = MatchManager.Instance.CreateCardInDictionary("premiummeat");
          MatchManager.Instance.GetCardData(cardInDictionary1);
          MatchManager.Instance.GenerateNewCard(1, cardInDictionary1, false, Enums.CardPlace.RandomDeck, heroIndex: this.character.HeroIndex);
          break;
        case 1:
          string cardInDictionary2 = MatchManager.Instance.CreateCardInDictionary("meat");
          MatchManager.Instance.GetCardData(cardInDictionary2);
          MatchManager.Instance.GenerateNewCard(1, cardInDictionary2, false, Enums.CardPlace.RandomDeck, heroIndex: this.character.HeroIndex);
          break;
        default:
          string cardInDictionary3 = MatchManager.Instance.CreateCardInDictionary("spoiledmeat");
          MatchManager.Instance.GetCardData(cardInDictionary3);
          MatchManager.Instance.GenerateNewCard(1, cardInDictionary3, false, Enums.CardPlace.RandomDeck, heroIndex: this.character.HeroIndex);
          break;
      }
      this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Butcher"), Enums.CombatScrollEffectType.Trait);
    }
    else
    {
      string cardInDictionary = MatchManager.Instance.CreateCardInDictionary("gourmetmeat");
      MatchManager.Instance.GetCardData(cardInDictionary);
      MatchManager.Instance.GenerateNewCard(1, cardInDictionary, false, Enums.CardPlace.RandomDeck, heroIndex: this.character.HeroIndex);
      this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_threestarchef"), Enums.CombatScrollEffectType.Trait);
    }
    MatchManager.Instance.ItemTraitActivated();
  }

  public void cantor()
  {
    if (this.character.GetAuraCharges("stanzai") != 0 && this.character.GetAuraCharges("stanzaii") != 0 && this.character.GetAuraCharges("stanzaiii") != 0)
      return;
    this.character.SetAuraTrait(this.character, "stanzai", 1);
    if (!((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null))
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Cantor"), Enums.CombatScrollEffectType.Trait);
    EffectsManager.Instance.PlayEffectAC("songself", true, this.character.HeroItem.CharImageT, false);
  }

  public void cautious()
  {
    this.character.SetAuraTrait(this.character, "buffer", 2);
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Cautious"), Enums.CombatScrollEffectType.Trait);
  }

  public void chastise()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null))
      return;
    TraitData traitData = Globals.Instance.GetTraitData(nameof (chastise));
    if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey(nameof (chastise)) && MatchManager.Instance.activatedTraits[nameof (chastise)] > traitData.TimesPerTurn - 1)
      return;
    if (MatchManager.Instance.CountHeroHand() == 10)
    {
      Debug.Log((object) "[TRAIT EXECUTION] Broke because player at max cards");
    }
    else
    {
      if (!this.castedCard.GetCardTypes().Contains(Enums.CardType.Holy_Spell) || !((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
        return;
      if (!MatchManager.Instance.activatedTraits.ContainsKey(nameof (chastise)))
        MatchManager.Instance.activatedTraits.Add(nameof (chastise), 1);
      else
        ++MatchManager.Instance.activatedTraits[nameof (chastise)];
      MatchManager.Instance.SetTraitInfoText();
      string str = "holysmite";
      int randomIntRange = MatchManager.Instance.GetRandomIntRange(0, 100, "trait");
      string cardInDictionary = MatchManager.Instance.CreateCardInDictionary(randomIntRange >= 45 ? (randomIntRange >= 90 ? str + "rare" : str + "b") : str + "a");
      CardData cardData = MatchManager.Instance.GetCardData(cardInDictionary);
      cardData.Vanish = true;
      cardData.EnergyReductionToZeroPermanent = true;
      MatchManager.Instance.GenerateNewCard(1, cardInDictionary, false, Enums.CardPlace.Hand);
      this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Chastise") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (chastise)], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
      MatchManager.Instance.ItemTraitActivated();
      MatchManager.Instance.CreateLogCardModification(cardData.InternalId, MatchManager.Instance.GetHero(this.character.HeroIndex));
    }
  }

  public void choir()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || MatchManager.Instance.GetCurrentRound() != 1)
      return;
    Hero[] teamHero = MatchManager.Instance.GetTeamHero();
    for (int index = 0; index < teamHero.Length; ++index)
    {
      if (teamHero[index] != null && (UnityEngine.Object) teamHero[index].HeroData != (UnityEngine.Object) null && teamHero[index].Alive)
        teamHero[index].SetAuraTrait(this.character, "stanzai", 1);
    }
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Choir"), Enums.CombatScrollEffectType.Trait);
  }

  public void clever()
  {
    this.character.SetAuraTrait(this.character, "inspire", 1);
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Clever"), Enums.CombatScrollEffectType.Trait);
  }

  public void combatready()
  {
    this.character.SetAuraTrait(this.character, "energize", 1);
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Combat Ready"), Enums.CombatScrollEffectType.Trait);
  }

  public void corrosion()
  {
    if (!((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
      return;
    NPC[] teamNpc = MatchManager.Instance.GetTeamNPC();
    for (int index = 0; index < teamNpc.Length; ++index)
    {
      if (teamNpc[index] != null && teamNpc[index].Alive)
      {
        bool flag = false;
        int auraCharges1 = teamNpc[index].GetAuraCharges("wet");
        if (auraCharges1 > 0)
        {
          flag = true;
          teamNpc[index].SetAuraTrait(this.character, "poison", auraCharges1);
        }
        int auraCharges2 = teamNpc[index].GetAuraCharges("rust");
        if (auraCharges2 > 0)
        {
          flag = true;
          teamNpc[index].SetAuraTrait(this.character, "vulnerable", auraCharges2);
        }
        if ((UnityEngine.Object) teamNpc[index].NPCItem != (UnityEngine.Object) null & flag)
        {
          teamNpc[index].NPCItem.ScrollCombatText(Texts.Instance.GetText("traits_Corrosion"), Enums.CombatScrollEffectType.Trait);
          EffectsManager.Instance.PlayEffectAC("poison", true, teamNpc[index].NPCItem.CharImageT, false);
        }
      }
    }
  }

  public void countermeasures()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null))
      return;
    TraitData traitData = Globals.Instance.GetTraitData(nameof (countermeasures));
    if (MatchManager.Instance.activatedTraitsRound != null && MatchManager.Instance.activatedTraitsRound.ContainsKey(nameof (countermeasures)) && MatchManager.Instance.activatedTraitsRound[nameof (countermeasures)] > traitData.TimesPerRound - 1 || this.character == null || !this.character.Alive || !((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null))
      return;
    if (!MatchManager.Instance.activatedTraitsRound.ContainsKey(nameof (countermeasures)))
      MatchManager.Instance.activatedTraitsRound.Add(nameof (countermeasures), 1);
    else
      ++MatchManager.Instance.activatedTraitsRound[nameof (countermeasures)];
    MatchManager.Instance.SetTraitInfoText();
    this.character.SetAuraTrait(this.character, "thorns", 3);
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Countermeasures") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraitsRound[nameof (countermeasures)], traitData.TimesPerRound), Enums.CombatScrollEffectType.Trait);
    EffectsManager.Instance.PlayEffectAC("thorns", true, this.character.HeroItem.CharImageT, false);
  }

  public void darkfeast()
  {
    if (!((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
      return;
    int num = Mathf.FloorToInt((float) (this.character.EffectCharges("dark") / 8));
    if (num <= 0)
      return;
    List<string> heroHand = MatchManager.Instance.GetHeroHand(this.character.HeroIndex);
    List<CardData> cardDataList = new List<CardData>();
    for (int index = 0; index < heroHand.Count; ++index)
    {
      CardData cardData = MatchManager.Instance.GetCardData(heroHand[index]);
      if (cardData.GetCardFinalCost() > 0)
        cardDataList.Add(cardData);
    }
    for (int index = 0; index < cardDataList.Count; ++index)
    {
      CardData cardData = cardDataList[index];
      cardData.EnergyReductionTemporal += num;
      MatchManager.Instance.UpdateHandCards();
      CardItem fromTableByIndex = MatchManager.Instance.GetCardFromTableByIndex(cardData.InternalId);
      fromTableByIndex.PlayDissolveParticle();
      fromTableByIndex.ShowEnergyModification(-num);
      this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Dark Feast"), Enums.CombatScrollEffectType.Trait);
      MatchManager.Instance.CreateLogCardModification(cardData.InternalId, MatchManager.Instance.GetHero(this.character.HeroIndex));
    }
  }

  public void defensemastery()
  {
    if (!((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
      return;
    int num = 1;
    if (num <= 0)
      return;
    List<string> heroHand = MatchManager.Instance.GetHeroHand(this.character.HeroIndex);
    List<CardData> cardDataList = new List<CardData>();
    for (int index = 0; index < heroHand.Count; ++index)
    {
      CardData cardData = MatchManager.Instance.GetCardData(heroHand[index]);
      if ((UnityEngine.Object) cardData != (UnityEngine.Object) null && cardData.GetCardFinalCost() > 0 && cardData.HasCardType(Enums.CardType.Defense))
        cardDataList.Add(cardData);
    }
    for (int index = 0; index < cardDataList.Count; ++index)
    {
      CardData cardData = cardDataList[index];
      if ((UnityEngine.Object) cardData != (UnityEngine.Object) null)
      {
        cardData.EnergyReductionTemporal += num;
        MatchManager.Instance.UpdateHandCards();
        CardItem fromTableByIndex = MatchManager.Instance.GetCardFromTableByIndex(cardData.InternalId);
        fromTableByIndex.PlayDissolveParticle();
        fromTableByIndex.ShowEnergyModification(-num);
        this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Defense Mastery"), Enums.CombatScrollEffectType.Trait);
        MatchManager.Instance.CreateLogCardModification(cardData.InternalId, MatchManager.Instance.GetHero(this.character.HeroIndex));
      }
    }
  }

  public void dinnerisready()
  {
    Hero[] teamHero = MatchManager.Instance.GetTeamHero();
    for (int index = 0; index < teamHero.Length; ++index)
    {
      if (teamHero[index] != null && (UnityEngine.Object) teamHero[index].HeroData != (UnityEngine.Object) null && teamHero[index].Alive)
      {
        if (!this.character.HaveTrait("threestarchef"))
        {
          switch (MatchManager.Instance.GetRandomIntRange(0, 3, "trait"))
          {
            case 0:
              string cardInDictionary1 = MatchManager.Instance.CreateCardInDictionary("premiummeat");
              MatchManager.Instance.GetCardData(cardInDictionary1);
              MatchManager.Instance.GenerateNewCard(1, cardInDictionary1, false, Enums.CardPlace.RandomDeck, heroIndex: teamHero[index].HeroIndex);
              continue;
            case 1:
              string cardInDictionary2 = MatchManager.Instance.CreateCardInDictionary("meat");
              MatchManager.Instance.GetCardData(cardInDictionary2);
              MatchManager.Instance.GenerateNewCard(1, cardInDictionary2, false, Enums.CardPlace.RandomDeck, heroIndex: teamHero[index].HeroIndex);
              continue;
            default:
              string cardInDictionary3 = MatchManager.Instance.CreateCardInDictionary("spoiledmeat");
              MatchManager.Instance.GetCardData(cardInDictionary3);
              MatchManager.Instance.GenerateNewCard(1, cardInDictionary3, false, Enums.CardPlace.RandomDeck, heroIndex: teamHero[index].HeroIndex);
              continue;
          }
        }
        else
        {
          switch (MatchManager.Instance.GetRandomIntRange(0, 3, "trait"))
          {
            case 0:
              string cardInDictionary4 = MatchManager.Instance.CreateCardInDictionary("premiummeat");
              MatchManager.Instance.GetCardData(cardInDictionary4);
              MatchManager.Instance.GenerateNewCard(1, cardInDictionary4, false, Enums.CardPlace.RandomDeck, heroIndex: teamHero[index].HeroIndex);
              continue;
            case 1:
              string cardInDictionary5 = MatchManager.Instance.CreateCardInDictionary("meat");
              MatchManager.Instance.GetCardData(cardInDictionary5);
              MatchManager.Instance.GenerateNewCard(1, cardInDictionary5, false, Enums.CardPlace.RandomDeck, heroIndex: teamHero[index].HeroIndex);
              continue;
            default:
              string cardInDictionary6 = MatchManager.Instance.CreateCardInDictionary("gourmetmeat");
              MatchManager.Instance.GetCardData(cardInDictionary6);
              MatchManager.Instance.GenerateNewCard(1, cardInDictionary6, false, Enums.CardPlace.RandomDeck, heroIndex: teamHero[index].HeroIndex);
              continue;
          }
        }
      }
    }
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_DinnerIsReady"), Enums.CombatScrollEffectType.Trait);
    MatchManager.Instance.ItemTraitActivated();
  }

  public void domeoflight()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null))
      return;
    TraitData traitData = Globals.Instance.GetTraitData(nameof (domeoflight));
    if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey(nameof (domeoflight)) && MatchManager.Instance.activatedTraits[nameof (domeoflight)] > traitData.TimesPerTurn - 1 || !this.castedCard.GetCardTypes().Contains(Enums.CardType.Defense))
      return;
    if (!MatchManager.Instance.activatedTraits.ContainsKey(nameof (domeoflight)))
      MatchManager.Instance.activatedTraits.Add(nameof (domeoflight), 1);
    else
      ++MatchManager.Instance.activatedTraits[nameof (domeoflight)];
    MatchManager.Instance.SetTraitInfoText();
    Hero[] teamHero = MatchManager.Instance.GetTeamHero();
    for (int index = 0; index < teamHero.Length; ++index)
    {
      if (teamHero[index] != null && (UnityEngine.Object) teamHero[index].HeroData != (UnityEngine.Object) null && teamHero[index].Alive)
      {
        teamHero[index].SetAuraTrait(this.character, "shield", 9);
        if ((UnityEngine.Object) teamHero[index].HeroItem != (UnityEngine.Object) null)
          EffectsManager.Instance.PlayEffectAC("shield1", true, teamHero[index].HeroItem.CharImageT, false);
      }
    }
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Dome Of Light") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (domeoflight)], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
  }

  public void eldritch()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null) || !((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
      return;
    int num = 2;
    bool flag = true;
    if (this.character.HaveTrait("frostswords"))
      num = 1;
    if (this.character.HaveTrait("timeloop") || this.character.HaveTrait("timeparadox"))
      flag = false;
    if (MatchManager.Instance.energyJustWastedByHero < num || flag && !this.castedCard.GetCardTypes().Contains(Enums.CardType.Spell))
      return;
    this.character.SetAuraTrait(this.character, "spellsword", 1);
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Eldritch"), Enums.CombatScrollEffectType.Trait);
  }

  public void elementalproliferation()
  {
    if (this.target == null || !this.target.Alive)
      return;
    this.target.SetAuraTrait(this.character, "burn", 2);
    this.target.SetAuraTrait(this.character, "chill", 2);
    this.target.SetAuraTrait(this.character, "spark", 2);
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Elemental Proliferation"), Enums.CombatScrollEffectType.Trait);
  }

  public void elementalweaver()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null))
      return;
    string internalId = this.castedCard.InternalId;
    int _energy = 0;
    int num1 = 0;
    if (this.castedCard.GetCardTypes().Contains(Enums.CardType.Fire_Spell))
    {
      for (int index = 0; index < this.character.CardsPlayedRound.Count; ++index)
      {
        if ((UnityEngine.Object) this.character.CardsPlayedRound[index] != (UnityEngine.Object) null && this.character.CardsPlayedRound[index].HasCardType(Enums.CardType.Fire_Spell))
          ++num1;
      }
      if (num1 <= 1)
        ++_energy;
    }
    int num2 = 0;
    if (this.castedCard.GetCardTypes().Contains(Enums.CardType.Cold_Spell))
    {
      for (int index = 0; index < this.character.CardsPlayedRound.Count; ++index)
      {
        if ((UnityEngine.Object) this.character.CardsPlayedRound[index] != (UnityEngine.Object) null && this.character.CardsPlayedRound[index].HasCardType(Enums.CardType.Cold_Spell))
          ++num2;
      }
      if (num2 <= 1)
        ++_energy;
    }
    int num3 = 0;
    if (this.castedCard.GetCardTypes().Contains(Enums.CardType.Lightning_Spell))
    {
      for (int index = 0; index < this.character.CardsPlayedRound.Count; ++index)
      {
        if ((UnityEngine.Object) this.character.CardsPlayedRound[index] != (UnityEngine.Object) null && this.character.CardsPlayedRound[index].HasCardType(Enums.CardType.Lightning_Spell))
          ++num3;
      }
      if (num3 <= 1)
        ++_energy;
    }
    if (_energy <= 0)
      return;
    this.character.ModifyEnergy(_energy, true);
    if (!((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null))
      return;
    if (!MatchManager.Instance.activatedTraits.ContainsKey(nameof (elementalweaver)))
      MatchManager.Instance.activatedTraits.Add(nameof (elementalweaver), _energy);
    else
      MatchManager.Instance.activatedTraits[nameof (elementalweaver)] += _energy;
    MatchManager.Instance.SetTraitInfoText();
    TraitData traitData = Globals.Instance.GetTraitData(nameof (elementalweaver));
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Elemental Weaver") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (elementalweaver)], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
    EffectsManager.Instance.PlayEffectAC("energy", true, this.character.HeroItem.CharImageT, false);
  }

  public void envenom()
  {
    if (this.target == null || !this.target.Alive)
      return;
    this.target.SetAuraTrait(this.character, "poison", 3);
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Envenom"), Enums.CombatScrollEffectType.Trait);
  }

  public void elusive()
  {
    if (this.character == null || !this.character.Alive)
      return;
    this.character.SetAuraTrait(this.character, "evasion", 2);
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Elusive"), Enums.CombatScrollEffectType.Trait);
  }

  public void exoskeleton()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null))
      return;
    TraitData traitData = Globals.Instance.GetTraitData(nameof (exoskeleton));
    if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey(nameof (exoskeleton)) && MatchManager.Instance.activatedTraits[nameof (exoskeleton)] > traitData.TimesPerTurn - 1 || MatchManager.Instance.energyJustWastedByHero <= 0 || !this.castedCard.GetCardTypes().Contains(Enums.CardType.Defense) || !((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
      return;
    if (!MatchManager.Instance.activatedTraits.ContainsKey(nameof (exoskeleton)))
      MatchManager.Instance.activatedTraits.Add(nameof (exoskeleton), 1);
    else
      ++MatchManager.Instance.activatedTraits[nameof (exoskeleton)];
    MatchManager.Instance.SetTraitInfoText();
    this.character.SetAuraTrait(this.character, "fortify", 1);
    this.character.ModifyEnergy(1, true);
    if (!((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null))
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Exoskeleton") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (exoskeleton)], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
    EffectsManager.Instance.PlayEffectAC("parry", true, this.character.HeroItem.CharImageT, false);
    EffectsManager.Instance.PlayEffectAC("energy", true, this.character.HeroItem.CharImageT, false);
  }

  public void fanfare()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null))
      return;
    TraitData traitData = Globals.Instance.GetTraitData(nameof (fanfare));
    if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey(nameof (fanfare)) && MatchManager.Instance.activatedTraits[nameof (fanfare)] > traitData.TimesPerTurn - 1 || MatchManager.Instance.energyJustWastedByHero <= 0 || !this.castedCard.GetCardTypes().Contains(Enums.CardType.Song) || !((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
      return;
    if (!MatchManager.Instance.activatedTraits.ContainsKey(nameof (fanfare)))
      MatchManager.Instance.activatedTraits.Add(nameof (fanfare), 1);
    else
      ++MatchManager.Instance.activatedTraits[nameof (fanfare)];
    MatchManager.Instance.SetTraitInfoText();
    this.character.ModifyEnergy(1, true);
    if ((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null)
    {
      this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Fanfare") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (fanfare)], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
      EffectsManager.Instance.PlayEffectAC("energy", true, this.character.HeroItem.CharImageT, false);
    }
    Hero lowestHealthHero = this.GetLowestHealthHero();
    if (lowestHealthHero == null || !lowestHealthHero.Alive)
      return;
    lowestHealthHero.SetAuraTrait(this.character, "regeneration", 2);
    if (!((UnityEngine.Object) lowestHealthHero.HeroItem != (UnityEngine.Object) null))
      return;
    EffectsManager.Instance.PlayEffectAC("regeneration", true, lowestHealthHero.HeroItem.CharImageT, false);
  }

  public void firestarter()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null))
      return;
    TraitData traitData = Globals.Instance.GetTraitData(nameof (firestarter));
    if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey(nameof (firestarter)) && MatchManager.Instance.activatedTraits[nameof (firestarter)] > traitData.TimesPerTurn - 1 || MatchManager.Instance.energyJustWastedByHero <= 0 || !this.castedCard.GetCardTypes().Contains(Enums.CardType.Fire_Spell) || !((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
      return;
    if (!MatchManager.Instance.activatedTraits.ContainsKey(nameof (firestarter)))
      MatchManager.Instance.activatedTraits.Add(nameof (firestarter), 1);
    else
      ++MatchManager.Instance.activatedTraits[nameof (firestarter)];
    MatchManager.Instance.SetTraitInfoText();
    this.character.ModifyEnergy(1, true);
    if ((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null)
    {
      this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Firestarter") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (firestarter)], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
      EffectsManager.Instance.PlayEffectAC("energy", true, this.character.HeroItem.CharImageT, false);
    }
    bool flag = false;
    NPC[] teamNpc = MatchManager.Instance.GetTeamNPC();
    while (!flag)
    {
      int randomIntRange = MatchManager.Instance.GetRandomIntRange(0, teamNpc.Length, "trait");
      if (teamNpc[randomIntRange] != null && teamNpc[randomIntRange].Alive)
      {
        teamNpc[randomIntRange].SetAuraTrait(this.character, "burn", 1);
        if ((UnityEngine.Object) teamNpc[randomIntRange].NPCItem != (UnityEngine.Object) null)
          EffectsManager.Instance.PlayEffectAC("burnsmall", true, teamNpc[randomIntRange].NPCItem.CharImageT, false);
        flag = true;
      }
    }
  }

  public void furious()
  {
    this.character.SetAuraTrait(this.character, "fury", 2);
    if (!((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null))
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Furious"), Enums.CombatScrollEffectType.Trait);
    EffectsManager.Instance.PlayEffectAC("fury", true, this.character.HeroItem.CharImageT, false);
  }

  public void glasscannon()
  {
  }

  public void gluttony()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null))
      return;
    TraitData traitData = Globals.Instance.GetTraitData(nameof (gluttony));
    if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey(nameof (gluttony)) && MatchManager.Instance.activatedTraits[nameof (gluttony)] > traitData.TimesPerTurn - 1 || !this.castedCard.GetCardTypes().Contains(Enums.CardType.Food) || !((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
      return;
    if (!MatchManager.Instance.activatedTraits.ContainsKey(nameof (gluttony)))
      MatchManager.Instance.activatedTraits.Add(nameof (gluttony), 1);
    else
      ++MatchManager.Instance.activatedTraits[nameof (gluttony)];
    MatchManager.Instance.SetTraitInfoText();
    this.character.SetAuraTrait(this.character, "vitality", 2);
    this.character.ModifyEnergy(1, true);
    if (!((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null))
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Gluttony") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (gluttony)], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
    EffectsManager.Instance.PlayEffectAC("heart", true, this.character.HeroItem.CharImageT, false);
    EffectsManager.Instance.PlayEffectAC("energy", true, this.character.HeroItem.CharImageT, false);
  }

  public void healerduality()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null))
      return;
    TraitData traitData = Globals.Instance.GetTraitData(nameof (healerduality));
    if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey(nameof (healerduality)))
    {
      int num = traitData.TimesPerTurn - 1;
      if (this.character.HaveTrait("philosophersstone"))
        ++num;
      if (MatchManager.Instance.activatedTraits[nameof (healerduality)] > num)
        return;
    }
    for (int index1 = 0; index1 < 2; ++index1)
    {
      Enums.CardClass cardClass1;
      Enums.CardClass cardClass2;
      if (index1 == 0)
      {
        cardClass1 = Enums.CardClass.Scout;
        cardClass2 = Enums.CardClass.Healer;
      }
      else
      {
        cardClass1 = Enums.CardClass.Healer;
        cardClass2 = Enums.CardClass.Scout;
      }
      if (this.castedCard.CardClass == cardClass1)
      {
        if (MatchManager.Instance.CountHeroHand() == 0 || !((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
          break;
        List<CardData> cardDataList = new List<CardData>();
        List<string> heroHand = MatchManager.Instance.GetHeroHand(this.character.HeroIndex);
        int num1 = 0;
        for (int index2 = 0; index2 < heroHand.Count; ++index2)
        {
          CardData cardData = MatchManager.Instance.GetCardData(heroHand[index2]);
          if ((UnityEngine.Object) cardData != (UnityEngine.Object) null && cardData.CardClass == cardClass2 && this.character.GetCardFinalCost(cardData) > num1)
            num1 = this.character.GetCardFinalCost(cardData);
        }
        if (num1 <= 0)
          break;
        for (int index3 = 0; index3 < heroHand.Count; ++index3)
        {
          CardData cardData = MatchManager.Instance.GetCardData(heroHand[index3]);
          if ((UnityEngine.Object) cardData != (UnityEngine.Object) null && cardData.CardClass == cardClass2 && this.character.GetCardFinalCost(cardData) >= num1)
            cardDataList.Add(cardData);
        }
        if (cardDataList.Count <= 0)
          break;
        CardData cardData1 = cardDataList.Count != 1 ? cardDataList[MatchManager.Instance.GetRandomIntRange(0, cardDataList.Count, "trait")] : cardDataList[0];
        if (!((UnityEngine.Object) cardData1 != (UnityEngine.Object) null))
          break;
        if (!MatchManager.Instance.activatedTraits.ContainsKey(nameof (healerduality)))
          MatchManager.Instance.activatedTraits.Add(nameof (healerduality), 1);
        else
          ++MatchManager.Instance.activatedTraits[nameof (healerduality)];
        MatchManager.Instance.SetTraitInfoText();
        int num2 = 1;
        cardData1.EnergyReductionTemporal += num2;
        MatchManager.Instance.GetCardFromTableByIndex(cardData1.InternalId).ShowEnergyModification(-num2);
        MatchManager.Instance.UpdateHandCards();
        if (!this.character.HaveTrait("philosophersstone"))
          this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Healer Duality") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (healerduality)], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
        else
          this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Healer Duality") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (healerduality)], traitData.TimesPerTurn + 1), Enums.CombatScrollEffectType.Trait);
        MatchManager.Instance.CreateLogCardModification(cardData1.InternalId, MatchManager.Instance.GetHero(this.character.HeroIndex));
        break;
      }
    }
  }

  public void healingbrew()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null))
      return;
    TraitData traitData = Globals.Instance.GetTraitData(nameof (healingbrew));
    if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey(nameof (healingbrew)) && MatchManager.Instance.activatedTraits[nameof (healingbrew)] > traitData.TimesPerTurn - 1 || MatchManager.Instance.energyJustWastedByHero <= 0 || !this.castedCard.GetCardTypes().Contains(Enums.CardType.Healing_Spell) || !((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
      return;
    if (!MatchManager.Instance.activatedTraits.ContainsKey(nameof (healingbrew)))
      MatchManager.Instance.activatedTraits.Add(nameof (healingbrew), 1);
    else
      ++MatchManager.Instance.activatedTraits[nameof (healingbrew)];
    MatchManager.Instance.SetTraitInfoText();
    this.character.ModifyEnergy(1, true);
    if ((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null)
    {
      this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Healing Brew") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (healingbrew)], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
      EffectsManager.Instance.PlayEffectAC("energy", true, this.character.HeroItem.CharImageT, false);
    }
    Hero lowestHealthHero = this.GetLowestHealthHero();
    if (lowestHealthHero == null || !lowestHealthHero.Alive)
      return;
    lowestHealthHero.SetAuraTrait(this.character, "regeneration", 2);
    if (!((UnityEngine.Object) lowestHealthHero.HeroItem != (UnityEngine.Object) null))
      return;
    EffectsManager.Instance.PlayEffectAC("regeneration", true, lowestHealthHero.HeroItem.CharImageT, false);
  }

  public void healingsurge()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null))
      return;
    TraitData traitData = Globals.Instance.GetTraitData(nameof (healingsurge));
    if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey(nameof (healingsurge)) && MatchManager.Instance.activatedTraits[nameof (healingsurge)] > traitData.TimesPerTurn - 1 || MatchManager.Instance.energyJustWastedByHero <= 0 || !this.castedCard.GetCardTypes().Contains(Enums.CardType.Healing_Spell) || !((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
      return;
    if (!MatchManager.Instance.activatedTraits.ContainsKey(nameof (healingsurge)))
      MatchManager.Instance.activatedTraits.Add(nameof (healingsurge), 1);
    else
      ++MatchManager.Instance.activatedTraits[nameof (healingsurge)];
    MatchManager.Instance.SetTraitInfoText();
    this.character.ModifyEnergy(1, true);
    if ((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null)
    {
      this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Healing Surge") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (healingsurge)], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
      EffectsManager.Instance.PlayEffectAC("energy", true, this.character.HeroItem.CharImageT, false);
    }
    Hero lowestHealthHero = this.GetLowestHealthHero();
    if (lowestHealthHero == null || !lowestHealthHero.Alive)
      return;
    lowestHealthHero.SetAuraTrait(this.character, "bless", 1);
    if (!((UnityEngine.Object) lowestHealthHero.HeroItem != (UnityEngine.Object) null))
      return;
    EffectsManager.Instance.PlayEffectAC("bless", true, lowestHealthHero.HeroItem.CharImageT, false);
  }

  public void hermit()
  {
    if (this.character == null || !this.character.Alive)
      return;
    bool flag = false;
    NPC[] teamNpc = MatchManager.Instance.GetTeamNPC();
    int index = 0;
    while (!flag)
    {
      index = MatchManager.Instance.GetRandomIntRange(0, teamNpc.Length, "trait");
      if (teamNpc[index] != null && (UnityEngine.Object) teamNpc[index].NpcData != (UnityEngine.Object) null && teamNpc[index].Alive)
      {
        teamNpc[index].SetAuraTrait(this.character, "poison", 1);
        teamNpc[index].SetAuraTrait(this.character, "rust", 1);
        flag = true;
      }
    }
    teamNpc[index].NPCItem.ScrollCombatText(Texts.Instance.GetText("traits_Hermit"), Enums.CombatScrollEffectType.Trait);
    EffectsManager.Instance.PlayEffectAC("waterimpact1", true, teamNpc[index].NPCItem.CharImageT, false);
  }

  public void hexmastery()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null))
      return;
    TraitData traitData = Globals.Instance.GetTraitData(nameof (hexmastery));
    if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey(nameof (hexmastery)) && MatchManager.Instance.activatedTraits[nameof (hexmastery)] > traitData.TimesPerTurn - 1 || MatchManager.Instance.energyJustWastedByHero <= 0 || !this.castedCard.GetCardTypes().Contains(Enums.CardType.Curse_Spell) || !((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
      return;
    if (!MatchManager.Instance.activatedTraits.ContainsKey(nameof (hexmastery)))
      MatchManager.Instance.activatedTraits.Add(nameof (hexmastery), 1);
    else
      ++MatchManager.Instance.activatedTraits[nameof (hexmastery)];
    MatchManager.Instance.SetTraitInfoText();
    this.character.SetAuraTrait(this.character, "powerful", 1);
    this.character.ModifyEnergy(1, true);
    if (!((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null))
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Hex Mastery") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (hexmastery)], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
    EffectsManager.Instance.PlayEffectAC("powerful", true, this.character.HeroItem.CharImageT, false);
    EffectsManager.Instance.PlayEffectAC("energy", true, this.character.HeroItem.CharImageT, false);
  }

  public void incantation()
  {
    bool flag = false;
    Hero[] teamHero = MatchManager.Instance.GetTeamHero();
    int index = 0;
    while (!flag)
    {
      index = MatchManager.Instance.GetRandomIntRange(0, teamHero.Length, "trait");
      if (teamHero[index] != null && (UnityEngine.Object) teamHero[index].HeroData != (UnityEngine.Object) null && teamHero[index].Alive)
      {
        switch (MatchManager.Instance.GetRandomIntRange(0, 4, "trait"))
        {
          case 0:
            teamHero[index].SetAuraTrait(this.character, "courage", 2);
            break;
          case 1:
            teamHero[index].SetAuraTrait(this.character, "insulate", 2);
            break;
          case 2:
            teamHero[index].SetAuraTrait(this.character, "reinforce", 2);
            break;
          default:
            teamHero[index].SetAuraTrait(this.character, "energize", 1);
            break;
        }
        flag = true;
      }
    }
    teamHero[index].HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Incantion"), Enums.CombatScrollEffectType.Trait);
  }

  public void incendiary()
  {
    if (this.target == null || !this.target.Alive)
      return;
    this.target.SetAuraTrait(this.character, "burn", 3);
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Incendiary"), Enums.CombatScrollEffectType.Trait);
  }

  public void indomitable()
  {
    if (this.character == null || !this.character.Alive)
      return;
    this.character.SetAuraTrait(this.character, "shield", Functions.FuncRoundToInt((float) this.auxInt * 0.5f));
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Indomitable"), Enums.CombatScrollEffectType.Trait);
  }

  public void inventor()
  {
    bool flag = false;
    Hero[] teamHero = MatchManager.Instance.GetTeamHero();
    int index = 0;
    while (!flag)
    {
      index = MatchManager.Instance.GetRandomIntRange(0, teamHero.Length, "trait");
      if (teamHero[index] != null && (UnityEngine.Object) teamHero[index].HeroData != (UnityEngine.Object) null && teamHero[index].Alive)
      {
        teamHero[index].SetAuraTrait(this.character, "energize", 1);
        flag = true;
      }
    }
    teamHero[index].HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Inventor"), Enums.CombatScrollEffectType.Trait);
    EffectsManager.Instance.PlayEffectAC("energy", true, teamHero[index].HeroItem.CharImageT, false);
  }

  public void ironfurnace()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null))
      return;
    TraitData traitData = Globals.Instance.GetTraitData(nameof (ironfurnace));
    if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey(nameof (ironfurnace)) && MatchManager.Instance.activatedTraits[nameof (ironfurnace)] > traitData.TimesPerTurn - 1 || !this.castedCard.GetCardTypes().Contains(Enums.CardType.Attack))
      return;
    if (!MatchManager.Instance.activatedTraits.ContainsKey(nameof (ironfurnace)))
      MatchManager.Instance.activatedTraits.Add(nameof (ironfurnace), 1);
    else
      ++MatchManager.Instance.activatedTraits[nameof (ironfurnace)];
    MatchManager.Instance.SetTraitInfoText();
    this.character.SetAuraTrait(this.character, "block", 8);
    this.character.SetAuraTrait(this.character, "furnace", 1);
    if (!((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null))
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Iron Furnace") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (ironfurnace)], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
    EffectsManager.Instance.PlayEffectAC("firepower", true, this.character.HeroItem.CharImageT, false);
  }

  public void jinx()
  {
    if (this.target == null || !this.target.Alive)
      return;
    this.target.SetAuraTrait(this.character, "dark", 2);
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Jinx"), Enums.CombatScrollEffectType.Trait);
  }

  public void keensenses()
  {
    if ((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null)
    {
      NPC[] teamNpc = MatchManager.Instance.GetTeamNPC();
      for (int index = 0; index < teamNpc.Length; ++index)
      {
        if (teamNpc[index] != null && teamNpc[index].Alive)
          teamNpc[index].SetAuraTrait(this.character, "mark", 1);
      }
    }
    else
    {
      Hero[] teamHero = MatchManager.Instance.GetTeamHero();
      for (int index = 0; index < teamHero.Length; ++index)
      {
        if (teamHero[index] != null && (UnityEngine.Object) teamHero[index].HeroData != (UnityEngine.Object) null && teamHero[index].Alive)
          teamHero[index].SetAuraTrait(this.character, "mark", 1);
      }
    }
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Keen Senses"), Enums.CombatScrollEffectType.Trait);
  }

  public void lethalshots()
  {
    if (this.target == null || !this.target.Alive)
      return;
    switch (MatchManager.Instance.GetRandomIntRange(0, 3, "trait"))
    {
      case 0:
        this.target.SetAuraTrait(this.character, "bleed", 3);
        break;
      case 1:
        this.target.SetAuraTrait(this.character, "poison", 3);
        break;
      default:
        this.target.SetAuraTrait(this.character, "vulnerable", 1);
        break;
    }
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Lethal Shots"), Enums.CombatScrollEffectType.Trait);
  }

  public void loadedgun()
  {
    if (!((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
      return;
    int num = 2;
    List<string> heroHand = MatchManager.Instance.GetHeroHand(this.character.HeroIndex);
    List<CardData> cardDataList = new List<CardData>();
    for (int index = 0; index < heroHand.Count; ++index)
    {
      CardData cardData = MatchManager.Instance.GetCardData(heroHand[index]);
      if (cardData.GetCardFinalCost() > 0 && cardData.HasCardType(Enums.CardType.Ranged_Attack))
        cardDataList.Add(cardData);
    }
    for (int index = 0; index < cardDataList.Count; ++index)
    {
      CardData cardData = cardDataList[index];
      cardData.EnergyReductionTemporal += num;
      MatchManager.Instance.UpdateHandCards();
      CardItem fromTableByIndex = MatchManager.Instance.GetCardFromTableByIndex(cardData.InternalId);
      fromTableByIndex.PlayDissolveParticle();
      fromTableByIndex.ShowEnergyModification(-num);
      MatchManager.Instance.CreateLogCardModification(cardData.InternalId, MatchManager.Instance.GetHero(this.character.HeroIndex));
    }
    if (cardDataList.Count <= 0)
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Loaded Gun"), Enums.CombatScrollEffectType.Trait);
  }

  public void maledict()
  {
    bool flag = false;
    NPC[] npcArray = (NPC[]) null;
    Hero[] heroArray = (Hero[]) null;
    if ((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null)
      npcArray = MatchManager.Instance.GetTeamNPC();
    else
      heroArray = MatchManager.Instance.GetTeamHero();
    Character character = (Character) null;
    while (!flag)
    {
      if ((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null)
      {
        int randomIntRange = MatchManager.Instance.GetRandomIntRange(0, npcArray.Length, "trait");
        character = (Character) npcArray[randomIntRange];
      }
      else
      {
        int randomIntRange = MatchManager.Instance.GetRandomIntRange(0, heroArray.Length, "trait");
        character = (Character) heroArray[randomIntRange];
        if ((UnityEngine.Object) character.HeroData == (UnityEngine.Object) null)
          character = (Character) null;
      }
      if (character != null && character.Alive)
      {
        switch (MatchManager.Instance.GetRandomIntRange(0, 3, "trait"))
        {
          case 0:
            character.SetAuraTrait(this.character, "dark", 2);
            break;
          case 1:
            character.SetAuraTrait(this.character, "slow", 1);
            break;
          default:
            character.SetAuraTrait(this.character, "vulnerable", 1);
            break;
        }
        flag = true;
      }
    }
    if (character == null)
      return;
    if ((UnityEngine.Object) character.NPCItem != (UnityEngine.Object) null)
      character.NPCItem.ScrollCombatText(Texts.Instance.GetText("traits_Maledict"), Enums.CombatScrollEffectType.Trait);
    else
      character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Maledict"), Enums.CombatScrollEffectType.Trait);
  }

  public void magicduality()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null))
      return;
    TraitData traitData = Globals.Instance.GetTraitData(nameof (magicduality));
    if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey(nameof (magicduality)))
    {
      int num = traitData.TimesPerTurn - 1;
      if (this.character.HaveTrait("unholyblight"))
        ++num;
      if (MatchManager.Instance.activatedTraits[nameof (magicduality)] > num)
        return;
    }
    for (int index1 = 0; index1 < 2; ++index1)
    {
      Enums.CardClass cardClass1;
      Enums.CardClass cardClass2;
      if (index1 == 0)
      {
        cardClass1 = Enums.CardClass.Warrior;
        cardClass2 = Enums.CardClass.Mage;
      }
      else
      {
        cardClass1 = Enums.CardClass.Mage;
        cardClass2 = Enums.CardClass.Warrior;
      }
      if (this.castedCard.CardClass == cardClass1)
      {
        if (MatchManager.Instance.CountHeroHand() == 0 || !((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
          break;
        List<CardData> cardDataList = new List<CardData>();
        List<string> heroHand = MatchManager.Instance.GetHeroHand(this.character.HeroIndex);
        int num1 = 0;
        for (int index2 = 0; index2 < heroHand.Count; ++index2)
        {
          CardData cardData = MatchManager.Instance.GetCardData(heroHand[index2]);
          if ((UnityEngine.Object) cardData != (UnityEngine.Object) null && cardData.CardClass == cardClass2 && this.character.GetCardFinalCost(cardData) > num1)
            num1 = this.character.GetCardFinalCost(cardData);
        }
        if (num1 <= 0)
          break;
        for (int index3 = 0; index3 < heroHand.Count; ++index3)
        {
          CardData cardData = MatchManager.Instance.GetCardData(heroHand[index3]);
          if ((UnityEngine.Object) cardData != (UnityEngine.Object) null && cardData.CardClass == cardClass2 && this.character.GetCardFinalCost(cardData) >= num1)
            cardDataList.Add(cardData);
        }
        if (cardDataList.Count <= 0)
          break;
        CardData cardData1 = cardDataList.Count != 1 ? cardDataList[MatchManager.Instance.GetRandomIntRange(0, cardDataList.Count, "trait")] : cardDataList[0];
        if (!((UnityEngine.Object) cardData1 != (UnityEngine.Object) null))
          break;
        if (!MatchManager.Instance.activatedTraits.ContainsKey(nameof (magicduality)))
          MatchManager.Instance.activatedTraits.Add(nameof (magicduality), 1);
        else
          ++MatchManager.Instance.activatedTraits[nameof (magicduality)];
        MatchManager.Instance.SetTraitInfoText();
        int num2 = 1;
        cardData1.EnergyReductionTemporal += num2;
        MatchManager.Instance.GetCardFromTableByIndex(cardData1.InternalId).ShowEnergyModification(-num2);
        MatchManager.Instance.UpdateHandCards();
        if (!this.character.HaveTrait("unholyblight"))
          this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Magic Duality") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (magicduality)], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
        else
          this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Magic Duality") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (magicduality)], traitData.TimesPerTurn + 1), Enums.CombatScrollEffectType.Trait);
        MatchManager.Instance.CreateLogCardModification(cardData1.InternalId, MatchManager.Instance.GetHero(this.character.HeroIndex));
        break;
      }
    }
  }

  public void marksmanship()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null) || !this.castedCard.GetCardTypes().Contains(Enums.CardType.Ranged_Attack))
      return;
    this.character.SetAuraTrait(this.character, "sharp", 2);
    if (!((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null))
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Marksmanship"), Enums.CombatScrollEffectType.Trait);
    EffectsManager.Instance.PlayEffectAC("sharp", true, this.character.HeroItem.CharImageT, false);
  }

  public void mentalist()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null) || !this.castedCard.GetCardTypes().Contains(Enums.CardType.Spell))
      return;
    this.character.SetAuraTrait(this.character, "powerful", 2);
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Mentalist"), Enums.CombatScrollEffectType.Trait);
  }

  public void minddevourer() => this.mentalleech();

  public void mentalleech()
  {
    bool flag = false;
    int num1 = 0;
    int num2 = 0;
    Hero[] teamHero = MatchManager.Instance.GetTeamHero();
    NPC[] teamNpc = MatchManager.Instance.GetTeamNPC();
    for (int index = 0; index < teamNpc.Length; ++index)
    {
      if (teamNpc[index] != null && teamNpc[index].Alive)
      {
        teamNpc[index].SetAuraTrait(this.character, "insane", 1);
        if ((UnityEngine.Object) teamNpc[index].NPCItem != (UnityEngine.Object) null)
          EffectsManager.Instance.PlayEffectAC("mindimpact2", true, teamNpc[index].NPCItem.CharImageT, false);
      }
    }
    for (int index = 0; index < teamHero.Length; ++index)
    {
      if (teamHero[index] != null && (UnityEngine.Object) teamHero[index].HeroData != (UnityEngine.Object) null && teamHero[index].Alive)
      {
        num1 += teamHero[index].GetAuraCharges("insane");
        num2 += teamHero[index].GetAuraCharges("sight");
      }
    }
    for (int index = 0; index < teamNpc.Length; ++index)
    {
      if (teamNpc[index] != null && teamNpc[index].Alive)
      {
        num1 += teamNpc[index].GetAuraCharges("insane");
        num2 += teamNpc[index].GetAuraCharges("sight");
      }
    }
    int heal = Functions.FuncRoundToInt((float) ((double) num1 * 0.20000000298023224 + (double) num2 * 0.10000000149011612));
    for (int index = 0; index < teamHero.Length; ++index)
    {
      if (teamHero[index] != null && (UnityEngine.Object) teamHero[index].HeroData != (UnityEngine.Object) null && teamHero[index].Alive)
      {
        int num3 = teamHero[index].HealReceivedFinal(heal);
        int _hp = num3;
        if (teamHero[index].GetHpLeftForMax() < num3)
          _hp = teamHero[index].GetHpLeftForMax();
        if (_hp > 0)
        {
          teamHero[index].ModifyHp(_hp, false);
          AtOManager.Instance.combatStats[this.character.HeroIndex, 3] += _hp;
          AtOManager.Instance.combatStatsCurrent[this.character.HeroIndex, 3] += _hp;
          teamHero[index].HeroItem.ScrollCombatTextDamageNew(new CastResolutionForCombatText()
          {
            heal = _hp
          });
          teamHero[index].SetEvent(Enums.EventActivation.Healed);
          this.character.SetEvent(Enums.EventActivation.Heal, (Character) teamHero[index]);
          flag = true;
          if ((UnityEngine.Object) teamHero[index].HeroItem != (UnityEngine.Object) null)
            EffectsManager.Instance.PlayEffectAC("healimpactsmall", true, teamHero[index].HeroItem.CharImageT, false);
        }
      }
    }
    if (!flag)
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Mentalleech"), Enums.CombatScrollEffectType.Trait);
  }

  public void mindflayer()
  {
    if (MatchManager.Instance.CountHeroHand() == 10)
    {
      Debug.Log((object) "[TRAIT EXECUTION] Broke because player at max cards");
    }
    else
    {
      bool flag = false;
      Hero[] teamHero = MatchManager.Instance.GetTeamHero();
      int heroIndex = 0;
      while (!flag)
      {
        heroIndex = MatchManager.Instance.GetRandomIntRange(0, teamHero.Length, "trait");
        if (teamHero[heroIndex] != null && (UnityEngine.Object) teamHero[heroIndex].HeroData != (UnityEngine.Object) null && teamHero[heroIndex].Alive)
          flag = true;
      }
      string cardInDictionary = MatchManager.Instance.CreateCardInDictionary("friendlytadpole");
      MatchManager.Instance.GetCardData(cardInDictionary);
      MatchManager.Instance.GenerateNewCard(1, cardInDictionary, false, Enums.CardPlace.RandomDeck, heroIndex: heroIndex);
      teamHero[heroIndex].HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Mind Flayer"), Enums.CombatScrollEffectType.Trait);
      MatchManager.Instance.ItemTraitActivated();
    }
  }

  public void mojo()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null) || !this.castedCard.GetCardTypes().Contains(Enums.CardType.Healing_Spell))
      return;
    Hero[] teamHero = MatchManager.Instance.GetTeamHero();
    for (int index = 0; index < teamHero.Length; ++index)
    {
      if (teamHero[index] != null && (UnityEngine.Object) teamHero[index].HeroData != (UnityEngine.Object) null && teamHero[index].Alive)
      {
        teamHero[index].HealCurses(1);
        if ((UnityEngine.Object) teamHero[index].HeroItem != (UnityEngine.Object) null)
        {
          teamHero[index].HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Mojo"), Enums.CombatScrollEffectType.Trait);
          EffectsManager.Instance.PlayEffectAC("dispel", true, teamHero[index].HeroItem.CharImageT, false);
        }
      }
    }
  }

  public void momentum()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null))
      return;
    TraitData traitData = Globals.Instance.GetTraitData(nameof (momentum));
    if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey(nameof (momentum)) && MatchManager.Instance.activatedTraits[nameof (momentum)] > traitData.TimesPerTurn - 1 || MatchManager.Instance.energyJustWastedByHero <= 0 || !this.castedCard.GetCardTypes().Contains(Enums.CardType.Melee_Attack) || !((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
      return;
    if (!MatchManager.Instance.activatedTraits.ContainsKey(nameof (momentum)))
      MatchManager.Instance.activatedTraits.Add(nameof (momentum), 1);
    else
      ++MatchManager.Instance.activatedTraits[nameof (momentum)];
    MatchManager.Instance.SetTraitInfoText();
    this.character.SetAuraTrait(this.character, "powerful", 1);
    this.character.ModifyEnergy(1, true);
    if (!((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null))
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Momentum") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (momentum)], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
    EffectsManager.Instance.PlayEffectAC("powerful", true, this.character.HeroItem.CharImageT, false);
    EffectsManager.Instance.PlayEffectAC("energy", true, this.character.HeroItem.CharImageT, false);
  }

  public void mountedcannon()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null))
      return;
    if (MatchManager.Instance.CountHeroHand() == 10)
    {
      Debug.Log((object) "[TRAIT EXECUTION] Broke because player at max cards");
    }
    else
    {
      if (MatchManager.Instance.energyJustWastedByHero <= 0 || !this.castedCard.GetCardTypes().Contains(Enums.CardType.Defense) || !((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
        return;
      string cardInDictionary = MatchManager.Instance.CreateCardInDictionary("blast");
      CardData cardData = MatchManager.Instance.GetCardData(cardInDictionary);
      MatchManager.Instance.GenerateNewCard(1, cardInDictionary, false, Enums.CardPlace.Hand);
      this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Mounted Cannon"), Enums.CombatScrollEffectType.Trait);
      MatchManager.Instance.ItemTraitActivated();
      MatchManager.Instance.CreateLogCardModification(cardData.InternalId, MatchManager.Instance.GetHero(this.character.HeroIndex));
    }
  }

  public void nightstalker()
  {
    this.character.SetAuraTrait(this.character, "stealth", 2);
    if (!((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null))
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Nightstalker"), Enums.CombatScrollEffectType.Trait);
    EffectsManager.Instance.PlayEffectAC("stealth", true, this.character.HeroItem.CharImageT, false);
  }

  public void offensemastery()
  {
    if (!((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
      return;
    int num = 1;
    if (num <= 0)
      return;
    List<string> heroHand = MatchManager.Instance.GetHeroHand(this.character.HeroIndex);
    List<CardData> cardDataList = new List<CardData>();
    for (int index = 0; index < heroHand.Count; ++index)
    {
      CardData cardData = MatchManager.Instance.GetCardData(heroHand[index]);
      if (cardData.GetCardFinalCost() > 0 && cardData.HasCardType(Enums.CardType.Attack))
        cardDataList.Add(cardData);
    }
    for (int index = 0; index < cardDataList.Count; ++index)
    {
      CardData cardData = cardDataList[index];
      cardData.EnergyReductionTemporal += num;
      MatchManager.Instance.UpdateHandCards();
      CardItem fromTableByIndex = MatchManager.Instance.GetCardFromTableByIndex(cardData.InternalId);
      fromTableByIndex.PlayDissolveParticle();
      fromTableByIndex.ShowEnergyModification(-num);
      this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Offense Mastery"), Enums.CombatScrollEffectType.Trait);
      MatchManager.Instance.CreateLogCardModification(cardData.InternalId, MatchManager.Instance.GetHero(this.character.HeroIndex));
    }
  }

  public void overflow()
  {
    int heal = Functions.FuncRoundToInt((float) this.auxInt * 0.35f);
    if (AtOManager.Instance.TeamHaveTrait("beaconoflight"))
      heal = Functions.FuncRoundToInt((float) this.auxInt * 0.7f);
    int _hp = this.target.HealReceivedFinal(heal);
    if (this.target.GetHpLeftForMax() < _hp)
      _hp = this.target.GetHpLeftForMax();
    if (_hp <= 0)
      return;
    this.target.ModifyHp(_hp);
    CastResolutionForCombatText _cast = new CastResolutionForCombatText();
    _cast.heal = _hp;
    if ((UnityEngine.Object) this.target.HeroItem != (UnityEngine.Object) null)
    {
      this.target.HeroItem.ScrollCombatTextDamageNew(_cast);
      EffectsManager.Instance.PlayEffectAC("healimpactsmall", true, this.target.HeroItem.CharImageT, false);
    }
    else
    {
      this.target.NPCItem.ScrollCombatTextDamageNew(_cast);
      EffectsManager.Instance.PlayEffectAC("healimpactsmall", true, this.target.NPCItem.CharImageT, false);
    }
    this.target.SetEvent(Enums.EventActivation.Healed);
    this.character.SetEvent(Enums.EventActivation.Heal, this.target);
    this.target.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Overflow"), Enums.CombatScrollEffectType.Trait);
  }

  public void pestilent()
  {
    if ((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null)
    {
      this.character.SetAuraTrait(this.character, "dark", 1);
      NPC[] teamNpc = MatchManager.Instance.GetTeamNPC();
      for (int index = 0; index < teamNpc.Length; ++index)
      {
        if (teamNpc[index] != null && teamNpc[index].Alive)
        {
          teamNpc[index].SetAuraTrait(this.character, "dark", 1);
          if ((UnityEngine.Object) teamNpc[index].NPCItem != (UnityEngine.Object) null)
          {
            teamNpc[index].NPCItem.ScrollCombatText(Texts.Instance.GetText("traits_Pestilent"), Enums.CombatScrollEffectType.Trait);
            EffectsManager.Instance.PlayEffectAC("shadowimpactsmall", true, teamNpc[index].NPCItem.CharImageT, false);
          }
        }
      }
    }
    else
    {
      Hero[] teamHero = MatchManager.Instance.GetTeamHero();
      for (int index = 0; index < teamHero.Length; ++index)
      {
        if (teamHero[index] != null && (UnityEngine.Object) teamHero[index].HeroData != (UnityEngine.Object) null && teamHero[index].Alive)
        {
          teamHero[index].SetAuraTrait(this.character, "dark", 1);
          if ((UnityEngine.Object) teamHero[index].HeroItem != (UnityEngine.Object) null)
          {
            teamHero[index].HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Pestilent"), Enums.CombatScrollEffectType.Trait);
            EffectsManager.Instance.PlayEffectAC("shadowimpactsmall", true, teamHero[index].HeroItem.CharImageT, false);
          }
        }
      }
    }
    if (!((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null))
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Pestilent"), Enums.CombatScrollEffectType.Trait);
    EffectsManager.Instance.PlayEffectAC("shadowimpactsmall", true, this.character.HeroItem.CharImageT, false);
  }

  public void philosophersstone()
  {
    if (!((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null) || !this.character.Alive)
      return;
    int charges = this.character.GetAuraCharges("rust") + 2;
    if (charges <= 0)
      return;
    Hero[] teamHero = MatchManager.Instance.GetTeamHero();
    for (int index = 0; index < teamHero.Length; ++index)
    {
      if (teamHero[index] != null && (UnityEngine.Object) teamHero[index].HeroData != (UnityEngine.Object) null && teamHero[index].Alive)
      {
        teamHero[index].SetAuraTrait(this.character, "bless", charges);
        teamHero[index].SetAuraTrait(this.character, "vitality", charges);
        if ((UnityEngine.Object) teamHero[index].HeroItem != (UnityEngine.Object) null)
          EffectsManager.Instance.PlayEffectAC("bless", true, teamHero[index].HeroItem.CharImageT, false);
      }
    }
    if (!((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null))
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Philosophers Stone"), Enums.CombatScrollEffectType.Trait);
  }

  public void premonition()
  {
    if ((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null)
    {
      NPC[] teamNpc = MatchManager.Instance.GetTeamNPC();
      for (int index = 0; index < teamNpc.Length; ++index)
      {
        if (teamNpc[index] != null && teamNpc[index].Alive)
          teamNpc[index].SetAuraTrait(this.character, "sight", 2);
      }
    }
    else
    {
      Hero[] teamHero = MatchManager.Instance.GetTeamHero();
      for (int index = 0; index < teamHero.Length; ++index)
      {
        if (teamHero[index] != null && (UnityEngine.Object) teamHero[index].HeroData != (UnityEngine.Object) null && teamHero[index].Alive)
          teamHero[index].SetAuraTrait(this.character, "sight", 2);
      }
    }
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Premonition"), Enums.CombatScrollEffectType.Trait);
  }

  public void rangedmastery()
  {
    if (!((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
      return;
    int num = 1;
    if (num <= 0)
      return;
    List<string> heroHand = MatchManager.Instance.GetHeroHand(this.character.HeroIndex);
    List<CardData> cardDataList = new List<CardData>();
    for (int index = 0; index < heroHand.Count; ++index)
    {
      CardData cardData = MatchManager.Instance.GetCardData(heroHand[index]);
      if (cardData.GetCardFinalCost() > 0 && cardData.HasCardType(Enums.CardType.Ranged_Attack))
        cardDataList.Add(cardData);
    }
    for (int index = 0; index < cardDataList.Count; ++index)
    {
      CardData cardData = cardDataList[index];
      cardData.EnergyReductionTemporal += num;
      MatchManager.Instance.UpdateHandCards();
      CardItem fromTableByIndex = MatchManager.Instance.GetCardFromTableByIndex(cardData.InternalId);
      fromTableByIndex.PlayDissolveParticle();
      fromTableByIndex.ShowEnergyModification(-num);
      this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Ranged Mastery"), Enums.CombatScrollEffectType.Trait);
      MatchManager.Instance.CreateLogCardModification(cardData.InternalId, MatchManager.Instance.GetHero(this.character.HeroIndex));
    }
  }

  public void resourceful()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null))
      return;
    TraitData traitData = Globals.Instance.GetTraitData(nameof (resourceful));
    if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey(nameof (resourceful)) && MatchManager.Instance.activatedTraits[nameof (resourceful)] > traitData.TimesPerTurn - 1 || MatchManager.Instance.energyJustWastedByHero <= 0 || !this.castedCard.GetCardTypes().Contains(Enums.CardType.Skill) || !((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
      return;
    if (!MatchManager.Instance.activatedTraits.ContainsKey(nameof (resourceful)))
      MatchManager.Instance.activatedTraits.Add(nameof (resourceful), 1);
    else
      ++MatchManager.Instance.activatedTraits[nameof (resourceful)];
    MatchManager.Instance.SetTraitInfoText();
    this.character.ModifyEnergy(1, true);
    if ((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null)
    {
      this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Resourceful") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (resourceful)], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
      EffectsManager.Instance.PlayEffectAC("energy", true, this.character.HeroItem.CharImageT, false);
    }
    NPC[] teamNpc = MatchManager.Instance.GetTeamNPC();
    for (int index = 0; index < teamNpc.Length; ++index)
    {
      if (teamNpc[index] != null && teamNpc[index].Alive)
        teamNpc[index].SetAuraTrait(this.character, "sight", 1);
    }
  }

  public void reverberation()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null))
      return;
    TraitData traitData = Globals.Instance.GetTraitData(nameof (reverberation));
    if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey(nameof (reverberation)) && MatchManager.Instance.activatedTraits[nameof (reverberation)] > traitData.TimesPerTurn - 1 || !this.castedCard.GetCardTypes().Contains(Enums.CardType.Holy_Spell) || !((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
      return;
    List<CardData> cardDataList = new List<CardData>();
    List<string> heroHand = MatchManager.Instance.GetHeroHand(this.character.HeroIndex);
    int num1 = 0;
    for (int index = 0; index < heroHand.Count; ++index)
    {
      CardData cardData = MatchManager.Instance.GetCardData(heroHand[index]);
      if ((UnityEngine.Object) cardData != (UnityEngine.Object) null && cardData.GetCardTypes().Contains(Enums.CardType.Holy_Spell) && this.character.GetCardFinalCost(cardData) > num1)
        num1 = this.character.GetCardFinalCost(cardData);
    }
    if (num1 <= 0)
      return;
    for (int index = 0; index < heroHand.Count; ++index)
    {
      CardData cardData = MatchManager.Instance.GetCardData(heroHand[index]);
      if ((UnityEngine.Object) cardData != (UnityEngine.Object) null && cardData.GetCardTypes().Contains(Enums.CardType.Holy_Spell) && this.character.GetCardFinalCost(cardData) >= num1)
        cardDataList.Add(cardData);
    }
    if (cardDataList.Count <= 0)
      return;
    CardData cardData1 = cardDataList.Count != 1 ? cardDataList[MatchManager.Instance.GetRandomIntRange(0, cardDataList.Count, "trait")] : cardDataList[0];
    if (!((UnityEngine.Object) cardData1 != (UnityEngine.Object) null))
      return;
    if (!MatchManager.Instance.activatedTraits.ContainsKey(nameof (reverberation)))
      MatchManager.Instance.activatedTraits.Add(nameof (reverberation), 1);
    else
      ++MatchManager.Instance.activatedTraits[nameof (reverberation)];
    MatchManager.Instance.SetTraitInfoText();
    int num2 = 3;
    cardData1.EnergyReductionTemporal += num2;
    MatchManager.Instance.GetCardFromTableByIndex(cardData1.InternalId).ShowEnergyModification(-num2);
    MatchManager.Instance.UpdateHandCards();
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Reverberation") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (reverberation)], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
    MatchManager.Instance.CreateLogCardModification(cardData1.InternalId, MatchManager.Instance.GetHero(this.character.HeroIndex));
  }

  public void scholar()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null))
      return;
    TraitData traitData = Globals.Instance.GetTraitData(nameof (scholar));
    if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey(nameof (scholar)) && MatchManager.Instance.activatedTraits[nameof (scholar)] > traitData.TimesPerTurn - 1 || !this.castedCard.GetCardTypes().Contains(Enums.CardType.Book) || MatchManager.Instance.CountHeroHand() == 0 || !((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
      return;
    List<CardData> cardDataList = new List<CardData>();
    List<string> heroHand = MatchManager.Instance.GetHeroHand(this.character.HeroIndex);
    int num1 = 0;
    for (int index = 0; index < heroHand.Count; ++index)
    {
      CardData cardData = MatchManager.Instance.GetCardData(heroHand[index]);
      if ((UnityEngine.Object) cardData != (UnityEngine.Object) null && this.character.GetCardFinalCost(cardData) > num1)
        num1 = this.character.GetCardFinalCost(cardData);
    }
    if (num1 <= 0)
      return;
    for (int index = 0; index < heroHand.Count; ++index)
    {
      CardData cardData = MatchManager.Instance.GetCardData(heroHand[index]);
      if ((UnityEngine.Object) cardData != (UnityEngine.Object) null && this.character.GetCardFinalCost(cardData) >= num1)
        cardDataList.Add(cardData);
    }
    if (cardDataList.Count <= 0)
      return;
    CardData cardData1 = cardDataList.Count != 1 ? cardDataList[MatchManager.Instance.GetRandomIntRange(0, cardDataList.Count, "trait")] : cardDataList[0];
    if (!((UnityEngine.Object) cardData1 != (UnityEngine.Object) null))
      return;
    if (!MatchManager.Instance.activatedTraits.ContainsKey(nameof (scholar)))
      MatchManager.Instance.activatedTraits.Add(nameof (scholar), 1);
    else
      ++MatchManager.Instance.activatedTraits[nameof (scholar)];
    MatchManager.Instance.SetTraitInfoText();
    int num2 = 1;
    cardData1.EnergyReductionPermanent += num2;
    MatchManager.Instance.GetCardFromTableByIndex(cardData1.InternalId).ShowEnergyModification(-num2);
    MatchManager.Instance.UpdateHandCards();
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Scholar") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (scholar)], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
    MatchManager.Instance.CreateLogCardModification(cardData1.InternalId, MatchManager.Instance.GetHero(this.character.HeroIndex));
  }

  public void scoutduality()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null))
      return;
    TraitData traitData = Globals.Instance.GetTraitData(nameof (scoutduality));
    if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey(nameof (scoutduality)))
    {
      int num = traitData.TimesPerTurn - 1;
      if (this.character.HaveTrait("valhalla"))
        ++num;
      if (MatchManager.Instance.activatedTraits[nameof (scoutduality)] > num)
        return;
    }
    for (int index1 = 0; index1 < 2; ++index1)
    {
      Enums.CardClass cardClass1;
      Enums.CardClass cardClass2;
      if (index1 == 0)
      {
        cardClass1 = Enums.CardClass.Scout;
        cardClass2 = Enums.CardClass.Mage;
      }
      else
      {
        cardClass1 = Enums.CardClass.Mage;
        cardClass2 = Enums.CardClass.Scout;
      }
      if (this.castedCard.CardClass == cardClass1)
      {
        if (MatchManager.Instance.CountHeroHand() == 0 || !((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
          break;
        List<CardData> cardDataList = new List<CardData>();
        List<string> heroHand = MatchManager.Instance.GetHeroHand(this.character.HeroIndex);
        int num1 = 0;
        for (int index2 = 0; index2 < heroHand.Count; ++index2)
        {
          CardData cardData = MatchManager.Instance.GetCardData(heroHand[index2]);
          if ((UnityEngine.Object) cardData != (UnityEngine.Object) null && cardData.CardClass == cardClass2 && this.character.GetCardFinalCost(cardData) > num1)
            num1 = this.character.GetCardFinalCost(cardData);
        }
        if (num1 <= 0)
          break;
        for (int index3 = 0; index3 < heroHand.Count; ++index3)
        {
          CardData cardData = MatchManager.Instance.GetCardData(heroHand[index3]);
          if ((UnityEngine.Object) cardData != (UnityEngine.Object) null && cardData.CardClass == cardClass2 && this.character.GetCardFinalCost(cardData) >= num1)
            cardDataList.Add(cardData);
        }
        if (cardDataList.Count <= 0)
          break;
        CardData cardData1 = cardDataList.Count != 1 ? cardDataList[MatchManager.Instance.GetRandomIntRange(0, cardDataList.Count, "trait")] : cardDataList[0];
        if (!((UnityEngine.Object) cardData1 != (UnityEngine.Object) null))
          break;
        if (!MatchManager.Instance.activatedTraits.ContainsKey(nameof (scoutduality)))
          MatchManager.Instance.activatedTraits.Add(nameof (scoutduality), 1);
        else
          ++MatchManager.Instance.activatedTraits[nameof (scoutduality)];
        MatchManager.Instance.SetTraitInfoText();
        int num2 = 1;
        cardData1.EnergyReductionTemporal += num2;
        MatchManager.Instance.GetCardFromTableByIndex(cardData1.InternalId).ShowEnergyModification(-num2);
        MatchManager.Instance.UpdateHandCards();
        if (!this.character.HaveTrait("valhalla"))
          this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Scout Duality") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (scoutduality)], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
        else
          this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Scout Duality") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (scoutduality)], traitData.TimesPerTurn + 1), Enums.CombatScrollEffectType.Trait);
        MatchManager.Instance.CreateLogCardModification(cardData1.InternalId, MatchManager.Instance.GetHero(this.character.HeroIndex));
        break;
      }
    }
  }

  public void shielder()
  {
    if (!(this.auxString == "shield") || this.target == null || !this.target.IsHero)
      return;
    int num = this.target.HealReceivedFinal(Functions.FuncRoundToInt((float) this.auxInt * 0.3f));
    int _hp = num;
    if (this.target.GetHpLeftForMax() < num)
      _hp = this.target.GetHpLeftForMax();
    if (_hp <= 0)
      return;
    this.target.ModifyHp(_hp);
    CastResolutionForCombatText _cast = new CastResolutionForCombatText();
    _cast.heal = _hp;
    if ((UnityEngine.Object) this.target.HeroItem != (UnityEngine.Object) null)
    {
      this.target.HeroItem.ScrollCombatTextDamageNew(_cast);
      EffectsManager.Instance.PlayEffectAC("healimpactsmall", true, this.target.HeroItem.CharImageT, false);
    }
    else
    {
      this.target.NPCItem.ScrollCombatTextDamageNew(_cast);
      EffectsManager.Instance.PlayEffectAC("healimpactsmall", true, this.target.NPCItem.CharImageT, false);
    }
    this.target.SetEvent(Enums.EventActivation.Healed);
    this.character.SetEvent(Enums.EventActivation.Heal, this.target);
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Shielder"), Enums.CombatScrollEffectType.Trait);
  }

  public void shieldexpert()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null) || !this.castedCard.GetCardTypes().Contains(Enums.CardType.Attack))
      return;
    this.character.SetAuraTrait(this.character, "block", 9);
    if (!((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null))
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Shield Expert"), Enums.CombatScrollEffectType.Trait);
    EffectsManager.Instance.PlayEffectAC("guardsmall", true, this.character.HeroItem.CharImageT, false);
  }

  public void ragnarok()
  {
    if (this.character.HaveTrait("spellsinger"))
      return;
    this.spellsinger();
  }

  public void spellsinger()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null))
      return;
    int num1 = Globals.Instance.GetTraitData(nameof (spellsinger)).TimesPerTurn - 1;
    if (this.character.HaveTrait(nameof (spellsinger)) && this.character.HaveTrait("ragnarok"))
      ++num1;
    if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey(nameof (spellsinger)) && MatchManager.Instance.activatedTraits[nameof (spellsinger)] > num1 || !this.castedCard.GetCardTypes().Contains(Enums.CardType.Song))
      return;
    List<string> stringList = new List<string>();
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(Enum.GetName(typeof (Enums.HeroClass), (object) Enums.HeroClass.Mage));
    stringBuilder.Append("_");
    stringBuilder.Append(Enum.GetName(typeof (Enums.CardType), (object) Enums.CardType.Fire_Spell));
    for (int index = 0; index < Globals.Instance.CardListByClassType[stringBuilder.ToString()].Count; ++index)
      stringList.Add(Globals.Instance.CardListByClassType[stringBuilder.ToString()][index]);
    stringBuilder.Clear();
    stringBuilder.Append(Enum.GetName(typeof (Enums.HeroClass), (object) Enums.HeroClass.Mage));
    stringBuilder.Append("_");
    stringBuilder.Append(Enum.GetName(typeof (Enums.CardType), (object) Enums.CardType.Lightning_Spell));
    for (int index = 0; index < Globals.Instance.CardListByClassType[stringBuilder.ToString()].Count; ++index)
      stringList.Add(Globals.Instance.CardListByClassType[stringBuilder.ToString()][index]);
    stringBuilder.Clear();
    stringBuilder.Append(Enum.GetName(typeof (Enums.HeroClass), (object) Enums.HeroClass.Mage));
    stringBuilder.Append("_");
    stringBuilder.Append(Enum.GetName(typeof (Enums.CardType), (object) Enums.CardType.Cold_Spell));
    for (int index = 0; index < Globals.Instance.CardListByClassType[stringBuilder.ToString()].Count; ++index)
      stringList.Add(Globals.Instance.CardListByClassType[stringBuilder.ToString()][index]);
    int num2 = MatchManager.Instance.energyJustWastedByHero + 1;
    if (this.character.HaveTrait(nameof (spellsinger)) && this.character.HaveTrait("ragnarok"))
    {
      if (this.character.EffectCharges("stanzaii") > 0)
        ++num2;
      else if (this.character.EffectCharges("stanzaiii") > 0)
        num2 += 2;
    }
    if (num2 > 10)
      num2 = 10;
    bool flag = false;
    string str = "";
    for (int index = 0; !flag && index < 500; ++index)
    {
      int randomIntRange = MatchManager.Instance.GetRandomIntRange(0, stringList.Count, "trait");
      str = stringList[randomIntRange];
      if (Globals.Instance.GetCardData(str, false).EnergyCostOriginal == num2)
        break;
    }
    string cardInDictionary = MatchManager.Instance.CreateCardInDictionary(str);
    CardData cardData = MatchManager.Instance.GetCardData(cardInDictionary);
    cardData.Vanish = true;
    cardData.EnergyReductionToZeroPermanent = true;
    MatchManager.Instance.GenerateNewCard(1, cardInDictionary, false, Enums.CardPlace.Hand);
    if ((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null)
    {
      if (!MatchManager.Instance.activatedTraits.ContainsKey(nameof (spellsinger)))
        MatchManager.Instance.activatedTraits.Add(nameof (spellsinger), 1);
      else
        ++MatchManager.Instance.activatedTraits[nameof (spellsinger)];
      MatchManager.Instance.SetTraitInfoText();
      this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Spell Singer") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (spellsinger)], num1 + 1), Enums.CombatScrollEffectType.Trait);
    }
    MatchManager.Instance.ItemTraitActivated();
    MatchManager.Instance.CreateLogCardModification(cardData.InternalId, MatchManager.Instance.GetHero(this.character.HeroIndex));
  }

  public void spiky()
  {
    this.character.SetAuraTrait(this.character, "thorns", 5);
    if (!((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null))
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Spiky"), Enums.CombatScrollEffectType.Trait);
    EffectsManager.Instance.PlayEffectAC("thorns", true, this.character.HeroItem.CharImageT, false);
  }

  public void tactician()
  {
    if (!((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
      return;
    int num = 1;
    if (num <= 0)
      return;
    List<string> heroHand = MatchManager.Instance.GetHeroHand(this.character.HeroIndex);
    List<CardData> cardDataList = new List<CardData>();
    for (int index = 0; index < heroHand.Count; ++index)
    {
      CardData cardData = MatchManager.Instance.GetCardData(heroHand[index]);
      if (cardData.GetCardFinalCost() > 0 && cardData.HasCardType(Enums.CardType.Skill))
        cardDataList.Add(cardData);
    }
    for (int index = 0; index < cardDataList.Count; ++index)
    {
      CardData cardData = cardDataList[index];
      cardData.EnergyReductionTemporal += num;
      MatchManager.Instance.UpdateHandCards();
      CardItem fromTableByIndex = MatchManager.Instance.GetCardFromTableByIndex(cardData.InternalId);
      fromTableByIndex.PlayDissolveParticle();
      fromTableByIndex.ShowEnergyModification(-num);
      this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Tactician"), Enums.CombatScrollEffectType.Trait);
      MatchManager.Instance.CreateLogCardModification(cardData.InternalId, MatchManager.Instance.GetHero(this.character.HeroIndex));
    }
  }

  public void temperate()
  {
    this.character.SetAuraTrait(this.character, "Insulate", 1);
    if (!((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null))
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Temperate"), Enums.CombatScrollEffectType.Trait);
    EffectsManager.Instance.PlayEffectAC("insulate", true, this.character.HeroItem.CharImageT, false);
  }

  public void trailblazer()
  {
    this.character.SetAuraTrait(this.character, "Fast", 1);
    if (!((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null))
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Trailblazer"), Enums.CombatScrollEffectType.Trait);
    EffectsManager.Instance.PlayEffectAC("speed1", true, this.character.HeroItem.CharImageT, false);
  }

  public void trickster()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null) || !this.castedCard.GetCardTypes().Contains(Enums.CardType.Song))
      return;
    this.character.SetAuraTrait(this.character, "sharp", 2);
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Trickster"), Enums.CombatScrollEffectType.Trait);
  }

  public void unbreakable()
  {
    Hero[] teamHero = MatchManager.Instance.GetTeamHero();
    for (int index = 0; index < teamHero.Length; ++index)
    {
      if (teamHero[index] != null && (UnityEngine.Object) teamHero[index].HeroData != (UnityEngine.Object) null && teamHero[index].Alive)
      {
        teamHero[index].SetAuraTrait(this.character, "block", 12);
        teamHero[index].SetAuraTrait(this.character, "fortify", 1);
        if ((UnityEngine.Object) teamHero[index].HeroItem != (UnityEngine.Object) null)
          EffectsManager.Instance.PlayEffectAC("intercept", true, teamHero[index].HeroItem.CharImageT, false);
      }
    }
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Unbreakable"), Enums.CombatScrollEffectType.Trait);
  }

  public void unlimitedblades()
  {
    if ((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null)
    {
      int num = this.character.EffectCharges("spellsword");
      if (num < 3)
        this.character.SetAuraTrait(this.character, "spellsword", 3 - num);
    }
    if (!((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null))
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Unlimited blades"), Enums.CombatScrollEffectType.Trait);
  }

  public void vampirism()
  {
    if (this.character == null || this.character.GetHp() <= 0)
      return;
    int heal = Functions.FuncRoundToInt((float) this.auxInt * 0.3f);
    Enums.CardClass CC = Enums.CardClass.None;
    if ((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null)
      CC = this.castedCard.CardClass;
    int _hp = this.character.HealReceivedFinal(this.character.HealWithCharacterBonus(heal, CC));
    this.character.ModifyHp(_hp);
    CastResolutionForCombatText _cast = new CastResolutionForCombatText();
    _cast.heal = _hp;
    if ((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null)
    {
      this.character.HeroItem.ScrollCombatTextDamageNew(_cast);
    }
    else
    {
      if (!((UnityEngine.Object) this.character.NPCItem != (UnityEngine.Object) null))
        return;
      this.character.NPCItem.ScrollCombatTextDamageNew(_cast);
    }
  }

  public void veilofshadows()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null))
      return;
    TraitData traitData = Globals.Instance.GetTraitData(nameof (veilofshadows));
    if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey(nameof (veilofshadows)) && MatchManager.Instance.activatedTraits[nameof (veilofshadows)] > traitData.TimesPerTurn - 1 || !((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
      return;
    if (!MatchManager.Instance.activatedTraits.ContainsKey(nameof (veilofshadows)))
      MatchManager.Instance.activatedTraits.Add(nameof (veilofshadows), 1);
    else
      ++MatchManager.Instance.activatedTraits[nameof (veilofshadows)];
    MatchManager.Instance.SetTraitInfoText();
    if (!((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null))
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Veilofshadows") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (veilofshadows)], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
  }

  public void versatile()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null))
      return;
    bool flag = false;
    string internalId = this.castedCard.InternalId;
    if (this.castedCard.GetCardTypes().Contains(Enums.CardType.Fire_Spell))
    {
      flag = true;
      this.character.SetAuraTrait(this.character, "powerful", 2);
    }
    if (this.castedCard.GetCardTypes().Contains(Enums.CardType.Cold_Spell))
    {
      flag = true;
      this.character.SetAuraTrait(this.character, "block", 6);
    }
    if (this.castedCard.GetCardTypes().Contains(Enums.CardType.Lightning_Spell))
    {
      flag = true;
      this.character.HealCurses(1);
    }
    if (!flag)
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Versatile"), Enums.CombatScrollEffectType.Trait);
  }

  public void voodoo()
  {
    if (!(this.auxString == "dark"))
      return;
    Hero lowestHealthHero = this.GetLowestHealthHero();
    if (lowestHealthHero == null || !lowestHealthHero.Alive)
      return;
    int num = lowestHealthHero.HealReceivedFinal(this.auxInt);
    int _hp = num;
    if (lowestHealthHero.GetHpLeftForMax() < num)
      _hp = lowestHealthHero.GetHpLeftForMax();
    if (_hp <= 0)
      return;
    lowestHealthHero.ModifyHp(_hp, false);
    AtOManager.Instance.combatStats[this.character.HeroIndex, 3] += _hp;
    AtOManager.Instance.combatStatsCurrent[this.character.HeroIndex, 3] += _hp;
    lowestHealthHero.HeroItem.ScrollCombatTextDamageNew(new CastResolutionForCombatText()
    {
      heal = _hp
    });
    lowestHealthHero.SetEvent(Enums.EventActivation.Healed);
    this.character.SetEvent(Enums.EventActivation.Heal, (Character) lowestHealthHero);
    if (!((UnityEngine.Object) lowestHealthHero.HeroItem != (UnityEngine.Object) null))
      return;
    EffectsManager.Instance.PlayEffectAC("healimpactsmall", true, lowestHealthHero.HeroItem.CharImageT, false);
  }

  public void warmaiden()
  {
    if (this.character == null || !this.character.Alive)
      return;
    int num1 = this.character.EffectCharges("stanzaiii");
    int num2 = this.character.EffectCharges("stanzaii");
    if (num1 > 0)
      this.character.SetAuraTrait(this.character, "powerful", 3);
    else if (num2 > 0)
      this.character.SetAuraTrait(this.character, "powerful", 2);
    else
      this.character.SetAuraTrait(this.character, "powerful", 1);
    if (!((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null))
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Warmaiden"), Enums.CombatScrollEffectType.Trait);
    EffectsManager.Instance.PlayEffectAC("powerful", true, this.character.HeroItem.CharImageT, false);
  }

  public void warriorduality()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null))
      return;
    TraitData traitData = Globals.Instance.GetTraitData(nameof (warriorduality));
    if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey(nameof (warriorduality)))
    {
      int num = traitData.TimesPerTurn - 1;
      if (this.character.HaveTrait("beaconoflight"))
        ++num;
      if (MatchManager.Instance.activatedTraits[nameof (warriorduality)] > num)
        return;
    }
    for (int index1 = 0; index1 < 2; ++index1)
    {
      Enums.CardClass cardClass1;
      Enums.CardClass cardClass2;
      if (index1 == 0)
      {
        cardClass1 = Enums.CardClass.Warrior;
        cardClass2 = Enums.CardClass.Healer;
      }
      else
      {
        cardClass1 = Enums.CardClass.Healer;
        cardClass2 = Enums.CardClass.Warrior;
      }
      if (this.castedCard.CardClass == cardClass1)
      {
        if (MatchManager.Instance.CountHeroHand() == 0 || !((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
          break;
        List<CardData> cardDataList = new List<CardData>();
        List<string> heroHand = MatchManager.Instance.GetHeroHand(this.character.HeroIndex);
        int num1 = 0;
        for (int index2 = 0; index2 < heroHand.Count; ++index2)
        {
          CardData cardData = MatchManager.Instance.GetCardData(heroHand[index2]);
          if ((UnityEngine.Object) cardData != (UnityEngine.Object) null && cardData.CardClass == cardClass2 && this.character.GetCardFinalCost(cardData) > num1)
            num1 = this.character.GetCardFinalCost(cardData);
        }
        if (num1 <= 0)
          break;
        for (int index3 = 0; index3 < heroHand.Count; ++index3)
        {
          CardData cardData = MatchManager.Instance.GetCardData(heroHand[index3]);
          if ((UnityEngine.Object) cardData != (UnityEngine.Object) null && cardData.CardClass == cardClass2 && this.character.GetCardFinalCost(cardData) >= num1)
            cardDataList.Add(cardData);
        }
        if (cardDataList.Count <= 0)
          break;
        CardData cardData1 = cardDataList.Count != 1 ? cardDataList[MatchManager.Instance.GetRandomIntRange(0, cardDataList.Count, "trait")] : cardDataList[0];
        if (!((UnityEngine.Object) cardData1 != (UnityEngine.Object) null))
          break;
        if (!MatchManager.Instance.activatedTraits.ContainsKey(nameof (warriorduality)))
          MatchManager.Instance.activatedTraits.Add(nameof (warriorduality), 1);
        else
          ++MatchManager.Instance.activatedTraits[nameof (warriorduality)];
        MatchManager.Instance.SetTraitInfoText();
        int num2 = 1;
        cardData1.EnergyReductionTemporal += num2;
        MatchManager.Instance.GetCardFromTableByIndex(cardData1.InternalId).ShowEnergyModification(-num2);
        MatchManager.Instance.UpdateHandCards();
        if (!this.character.HaveTrait("beaconoflight"))
          this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Warrior Duality") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (warriorduality)], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
        else
          this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Warrior Duality") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (warriorduality)], traitData.TimesPerTurn + 1), Enums.CombatScrollEffectType.Trait);
        MatchManager.Instance.CreateLogCardModification(cardData1.InternalId, MatchManager.Instance.GetHero(this.character.HeroIndex));
        break;
      }
    }
  }

  public void weaponexpert()
  {
  }

  public void webweaver()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || MatchManager.Instance.GetCurrentRound() != 1)
      return;
    NPC[] teamNpc = MatchManager.Instance.GetTeamNPC();
    for (int index = 0; index < teamNpc.Length; ++index)
    {
      if (teamNpc[index] != null && teamNpc[index].Alive)
      {
        teamNpc[index].SetAuraTrait(this.character, "insane", 6);
        teamNpc[index].SetAuraTrait(this.character, "poison", 6);
        teamNpc[index].SetAuraTrait(this.character, "shackle", 1);
        EffectsManager.Instance.PlayEffectAC("poisonneuro", true, teamNpc[index].NPCItem.CharImageT, false);
      }
    }
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Webweaver"), Enums.CombatScrollEffectType.Trait);
  }

  public void welltrained()
  {
    this.character.SetAuraTrait(this.character, "reinforce", 2);
    if (!((UnityEngine.Object) this.character.HeroItem != (UnityEngine.Object) null))
      return;
    this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Well Trained"), Enums.CombatScrollEffectType.Trait);
    EffectsManager.Instance.PlayEffectAC("reinforce", true, this.character.HeroItem.CharImageT, false);
  }

  public void widesleeves()
  {
    if (!((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) this.castedCard != (UnityEngine.Object) null))
      return;
    TraitData traitData = Globals.Instance.GetTraitData(nameof (widesleeves));
    if (MatchManager.Instance.activatedTraits != null && MatchManager.Instance.activatedTraits.ContainsKey(nameof (widesleeves)) && MatchManager.Instance.activatedTraits[nameof (widesleeves)] > traitData.TimesPerTurn - 1)
      return;
    if (MatchManager.Instance.CountHeroHand() == 10)
    {
      Debug.Log((object) "[TRAIT EXECUTION] Broke because player at max cards");
    }
    else
    {
      if (!this.castedCard.GetCardTypes().Contains(Enums.CardType.Small_Weapon) || !((UnityEngine.Object) this.character.HeroData != (UnityEngine.Object) null))
        return;
      if (!MatchManager.Instance.activatedTraits.ContainsKey(nameof (widesleeves)))
        MatchManager.Instance.activatedTraits.Add(nameof (widesleeves), 1);
      else
        ++MatchManager.Instance.activatedTraits[nameof (widesleeves)];
      MatchManager.Instance.SetTraitInfoText();
      int randomIntRange1 = MatchManager.Instance.GetRandomIntRange(0, 100, "trait");
      int randomIntRange2 = MatchManager.Instance.GetRandomIntRange(0, Globals.Instance.CardListByType[Enums.CardType.Small_Weapon].Count, "trait");
      string str;
      CardData cardData1 = Globals.Instance.GetCardData(str = Globals.Instance.CardListByType[Enums.CardType.Small_Weapon][randomIntRange2], false);
      string cardInDictionary = MatchManager.Instance.CreateCardInDictionary(Functions.GetCardByRarity(randomIntRange1, cardData1));
      CardData cardData2 = MatchManager.Instance.GetCardData(cardInDictionary);
      cardData2.Vanish = true;
      cardData2.EnergyReductionToZeroPermanent = true;
      MatchManager.Instance.GenerateNewCard(1, cardInDictionary, false, Enums.CardPlace.Hand);
      this.character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Wide Sleeves") + Functions.TextChargesLeft(MatchManager.Instance.activatedTraits[nameof (widesleeves)], traitData.TimesPerTurn), Enums.CombatScrollEffectType.Trait);
      MatchManager.Instance.ItemTraitActivated();
      MatchManager.Instance.CreateLogCardModification(cardData2.InternalId, MatchManager.Instance.GetHero(this.character.HeroIndex));
    }
  }

  private Hero GetLowestHealthHero()
  {
    int num1 = -1;
    float num2 = 99.9999f;
    Hero[] teamHero = MatchManager.Instance.GetTeamHero();
    for (int index = 0; index < teamHero.Length; ++index)
    {
      if (teamHero[index] != null && (UnityEngine.Object) teamHero[index].HeroData != (UnityEngine.Object) null && teamHero[index].Alive)
      {
        float hpPercent = teamHero[index].GetHpPercent();
        if ((double) hpPercent <= (double) num2)
        {
          num2 = hpPercent;
          num1 = index;
        }
      }
    }
    if (num1 > -1)
    {
      List<int> intList = new List<int>();
      intList.Add(num1);
      for (int index = 0; index < teamHero.Length; ++index)
      {
        if (index != num1 && teamHero[index] != null && (UnityEngine.Object) teamHero[index].HeroData != (UnityEngine.Object) null && teamHero[index].Alive && (double) teamHero[index].GetHpPercent() == (double) num2)
          intList.Add(index);
      }
      if (intList.Count > 0)
      {
        int num3 = 0;
        int auxInt = this.auxInt;
        for (; num3 < 10; ++num3)
        {
          int index = intList.Count <= 1 ? intList[0] : intList[MatchManager.Instance.GetRandomIntRange(0, intList.Count, "trait")];
          if (num3 == 9)
            index = intList[0];
          if (index < teamHero.Length && teamHero[index] != null && (UnityEngine.Object) teamHero[index].HeroData != (UnityEngine.Object) null)
            return teamHero[index];
        }
      }
    }
    return (Hero) null;
  }
}
