// Decompiled with JetBrains decompiler
// Type: Hero
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class Hero : Character
{
  [SerializeField]
  private GameObject gameObjectAnimated;
  private Sprite heroSprite;
  private Sprite borderSprite;

  public void RedoSubclassFromSkin()
  {
    if ((UnityEngine.Object) this.HeroData == (UnityEngine.Object) null)
      return;
    SubClassData heroSubClass = this.HeroData.HeroSubClass;
    this.heroSprite = heroSubClass.Sprite;
    this.borderSprite = heroSubClass.SpriteBorder;
    if ((UnityEngine.Object) this.borderSprite == (UnityEngine.Object) null)
      Debug.LogError((object) "Error with borderSprite");
    this.SpriteSpeed = heroSubClass.SpriteSpeed;
    this.SpritePortrait = heroSubClass.SpritePortrait;
    this.gameObjectAnimated = heroSubClass.GameObjectAnimated;
  }

  public void InitGeneralData()
  {
    if ((UnityEngine.Object) this.HeroData == (UnityEngine.Object) null)
      return;
    SubClassData heroSubClass = this.HeroData.HeroSubClass;
    this.heroSprite = heroSubClass.Sprite;
    this.borderSprite = heroSubClass.SpriteBorder;
    if ((UnityEngine.Object) this.borderSprite == (UnityEngine.Object) null)
      Debug.LogError((object) "Error with borderSprite");
    this.SpriteSpeed = heroSubClass.SpriteSpeed;
    this.SpritePortrait = heroSubClass.SpritePortrait;
    this.gameObjectAnimated = heroSubClass.GameObjectAnimated;
    this.SourceName = heroSubClass.CharacterName;
    this.ClassName = this.HeroData.HeroName.ToLower();
    this.SubclassName = heroSubClass.Id;
    if ((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) null)
      this.InternalId = MatchManager.Instance.GetRandomString();
    else
      this.InternalId = Functions.RandomString(6f);
  }

  public void InitHP()
  {
    if ((UnityEngine.Object) this.HeroData == (UnityEngine.Object) null)
      return;
    SubClassData heroSubClass = this.HeroData.HeroSubClass;
    this.Hp = this.HpCurrent = heroSubClass.Hp;
    this.Hp += PlayerManager.Instance.GetPerkMaxHealth(heroSubClass.Id);
    this.HpCurrent += PlayerManager.Instance.GetPerkMaxHealth(heroSubClass.Id);
  }

  public void InitEnergy()
  {
    if ((UnityEngine.Object) this.HeroData == (UnityEngine.Object) null)
      return;
    SubClassData heroSubClass = this.HeroData.HeroSubClass;
    this.Energy = heroSubClass.Energy + PlayerManager.Instance.GetPerkEnergyBegin(heroSubClass.Id);
    this.EnergyCurrent = this.Energy;
    if (AtOManager.Instance.IsChallengeTraitActive("energizedheroeslow"))
    {
      ++this.Energy;
      ++this.EnergyCurrent;
    }
    if (AtOManager.Instance.IsChallengeTraitActive("energizedheroes"))
    {
      ++this.Energy;
      ++this.EnergyCurrent;
    }
    int ngPlus = AtOManager.Instance.GetNgPlus();
    if (ngPlus > 3)
    {
      switch (ngPlus)
      {
        case 4:
          --this.Energy;
          --this.EnergyCurrent;
          break;
        case 5:
          --this.Energy;
          --this.EnergyCurrent;
          break;
        case 6:
          --this.Energy;
          --this.EnergyCurrent;
          break;
        case 7:
          this.Energy -= 2;
          this.EnergyCurrent -= 2;
          break;
        case 8:
          this.Energy -= 2;
          this.EnergyCurrent -= 2;
          break;
        case 9:
          this.Energy -= 2;
          this.EnergyCurrent -= 2;
          break;
      }
      if (this.Energy < 0)
        this.Energy = 0;
      if (this.EnergyCurrent < 0)
        this.EnergyCurrent = 0;
    }
    this.EnergyTurn = heroSubClass.EnergyTurn;
    if (this.Traits != null)
    {
      for (int index = 0; index < this.Traits.Length; ++index)
      {
        if (this.Traits[index] != null && this.Traits[index] != "")
        {
          TraitData traitData = Globals.Instance.GetTraitData(this.Traits[index]);
          if ((UnityEngine.Object) traitData != (UnityEngine.Object) null && traitData.CharacterStatModified == Enums.CharacterStat.EnergyTurn)
            this.EnergyTurn += traitData.CharacterStatModifiedValue;
        }
      }
    }
    if (!SandboxManager.Instance.IsEnabled() || AtOManager.Instance.Sandbox_startingEnergy == 0)
      return;
    this.Energy += AtOManager.Instance.Sandbox_startingEnergy;
    this.EnergyCurrent += AtOManager.Instance.Sandbox_startingEnergy;
  }

  public void InitSpeed()
  {
    if ((UnityEngine.Object) this.HeroData == (UnityEngine.Object) null)
      return;
    SubClassData heroSubClass = this.HeroData.HeroSubClass;
    this.Speed = heroSubClass.Speed;
    this.Speed += PlayerManager.Instance.GetPerkSpeed(heroSubClass.Id);
    if (AtOManager.Instance.IsChallengeTraitActive("slowheroes"))
      --this.Speed;
    if (AtOManager.Instance.IsChallengeTraitActive("fastheroes"))
      ++this.Speed;
    if (!SandboxManager.Instance.IsEnabled() || AtOManager.Instance.Sandbox_startingSpeed == 0)
      return;
    this.Speed += AtOManager.Instance.Sandbox_startingSpeed;
  }

  public void InitResist()
  {
    if ((UnityEngine.Object) this.HeroData == (UnityEngine.Object) null)
      return;
    SubClassData heroSubClass = this.HeroData.HeroSubClass;
    this.ResistSlashing = heroSubClass.ResistSlashing + PlayerManager.Instance.GetPerkResistBonus(heroSubClass.Id, Enums.DamageType.Slashing);
    if (this.ResistSlashing >= 100)
      this.ImmuneSlashing = true;
    this.ResistBlunt = heroSubClass.ResistBlunt + PlayerManager.Instance.GetPerkResistBonus(heroSubClass.Id, Enums.DamageType.Blunt);
    if (this.ResistBlunt >= 100)
      this.ImmuneBlunt = true;
    this.ResistPiercing = heroSubClass.ResistPiercing + PlayerManager.Instance.GetPerkResistBonus(heroSubClass.Id, Enums.DamageType.Piercing);
    if (this.ResistPiercing >= 100)
      this.ImmunePiercing = true;
    this.ResistFire = heroSubClass.ResistFire + PlayerManager.Instance.GetPerkResistBonus(heroSubClass.Id, Enums.DamageType.Fire);
    if (this.ResistFire >= 100)
      this.ImmuneFire = true;
    this.ResistCold = heroSubClass.ResistCold + PlayerManager.Instance.GetPerkResistBonus(heroSubClass.Id, Enums.DamageType.Cold);
    if (this.ResistCold >= 100)
      this.ImmuneCold = true;
    this.ResistLightning = heroSubClass.ResistLightning + PlayerManager.Instance.GetPerkResistBonus(heroSubClass.Id, Enums.DamageType.Lightning);
    if (this.ResistLightning >= 100)
      this.ImmuneLightning = true;
    this.ResistMind = heroSubClass.ResistMind + PlayerManager.Instance.GetPerkResistBonus(heroSubClass.Id, Enums.DamageType.Mind);
    if (this.ResistMind >= 100)
      this.ImmuneMind = true;
    this.ResistHoly = heroSubClass.ResistHoly + PlayerManager.Instance.GetPerkResistBonus(heroSubClass.Id, Enums.DamageType.Holy);
    if (this.ResistHoly >= 100)
      this.ImmuneHoly = true;
    this.ResistShadow = heroSubClass.ResistShadow + PlayerManager.Instance.GetPerkResistBonus(heroSubClass.Id, Enums.DamageType.Shadow);
    if (this.ResistShadow < 100)
      return;
    this.ImmuneShadow = true;
  }

  public override void InitData()
  {
    if (!((UnityEngine.Object) this.HeroData != (UnityEngine.Object) null))
      return;
    this.InitGeneralData();
    SubClassData heroSubClass = this.HeroData.HeroSubClass;
    this.Id = heroSubClass.Id + "_" + this.InternalId;
    this.InitHP();
    this.InitEnergy();
    this.InitSpeed();
    this.Experience = 0;
    this.Level = 1;
    this.IsHero = true;
    this.InitResist();
    this.Alive = true;
    this.AuraList = new List<Aura>();
    this.Traits = new string[5];
    this.SetInitialItems(heroSubClass.Item, 0);
    if (GameManager.Instance.IsObeliskChallenge())
      this.ReassignInitialItem();
    this.Cards = new List<string>();
    if (GameManager.Instance.IsObeliskChallenge())
      return;
    if (GameManager.Instance.IsGameAdventure())
      this.SetInitialCards(this.HeroData);
    else
      this.SetInitialCardsSingularity(this.HeroData);
  }

  public void AssignOwner(string nickName)
  {
    this.Owner = nickName;
    if (!(this.OwnerOriginal == ""))
      return;
    this.OwnerOriginal = NetworkManager.Instance.GetPlayerNickReal(nickName);
  }

  public override void BeginTurn() => base.BeginTurn();

  public override void BeginTurnPerks() => base.BeginTurnPerks();

  public bool AssignTrait(string traitName)
  {
    TraitData traitData = Globals.Instance.GetTraitData(traitName);
    if (!((UnityEngine.Object) traitData != (UnityEngine.Object) null))
      return false;
    if (traitData.Id == "eternalbond")
      this.Pet = "harleyrare";
    else if (traitData.Id == "templelurkers")
    {
      if (this.Pet.StartsWith("cocoon", StringComparison.OrdinalIgnoreCase) || this.Pet.StartsWith("liante", StringComparison.OrdinalIgnoreCase))
        this.Pet = "templelurkerpet";
    }
    else if (traitData.Id == "mentalscavengers")
    {
      if (this.Pet.StartsWith("cocoon", StringComparison.OrdinalIgnoreCase) || this.Pet.StartsWith("liante", StringComparison.OrdinalIgnoreCase))
        this.Pet = "mentalscavengerspet";
    }
    else if (traitData.Id == "spiderqueen")
    {
      if (this.Pet == "templelurkerpet")
        this.Pet = "templelurkerpetrare";
      else if (this.Pet == "mentalscavengerspet")
        this.Pet = "mentalscavengerspetrare";
    }
    int traitLevel = this.HeroData.HeroSubClass.GetTraitLevel(traitName);
    if (traitLevel < 0 || this.Traits[traitLevel] != null && this.Traits[traitLevel] != "")
      return false;
    this.Traits[traitLevel] = traitName.ToLower();
    if (traitData.CharacterStatModified != Enums.CharacterStat.None)
    {
      if (traitData.CharacterStatModified == Enums.CharacterStat.Hp)
      {
        if (this.Hp == this.HpCurrent)
          this.HpCurrent += traitData.CharacterStatModifiedValue;
        this.Hp += traitData.CharacterStatModifiedValue;
      }
      else if (traitData.CharacterStatModified == Enums.CharacterStat.Speed)
        this.Speed += traitData.CharacterStatModifiedValue;
      else if (traitData.CharacterStatModified == Enums.CharacterStat.Energy)
        this.Energy += traitData.CharacterStatModifiedValue;
      else if (traitData.CharacterStatModified == Enums.CharacterStat.EnergyTurn)
        this.EnergyTurn += traitData.CharacterStatModifiedValue;
    }
    if (traitData.ResistModified1 != Enums.DamageType.None)
      this.ModifyResist(this, traitData.ResistModified1, traitData.ResistModifiedValue1);
    if (traitData.ResistModified2 != Enums.DamageType.None)
      this.ModifyResist(this, traitData.ResistModified2, traitData.ResistModifiedValue2);
    if (traitData.ResistModified3 != Enums.DamageType.None)
      this.ModifyResist(this, traitData.ResistModified3, traitData.ResistModifiedValue3);
    if (traitData.AuracurseImmune1.Trim() != "")
      this.AuracurseImmune.Add(traitData.AuracurseImmune1.Trim().ToLower());
    if (traitData.AuracurseImmune2.Trim() != "")
      this.AuracurseImmune.Add(traitData.AuracurseImmune2.Trim().ToLower());
    if (traitData.AuracurseImmune3.Trim() != "")
      this.AuracurseImmune.Add(traitData.AuracurseImmune3.Trim().ToLower());
    this.ClearCaches();
    return true;
  }

  private void ModifyResist(Hero hero, Enums.DamageType damageType, int value)
  {
    switch (damageType)
    {
      case Enums.DamageType.Slashing:
        hero.ResistSlashing += value;
        break;
      case Enums.DamageType.Blunt:
        hero.ResistBlunt += value;
        break;
      case Enums.DamageType.Piercing:
        hero.ResistPiercing += value;
        break;
      case Enums.DamageType.Fire:
        hero.ResistFire += value;
        break;
      case Enums.DamageType.Cold:
        hero.ResistCold += value;
        break;
      case Enums.DamageType.Lightning:
        hero.ResistLightning += value;
        break;
      case Enums.DamageType.Mind:
        hero.ResistMind += value;
        break;
      case Enums.DamageType.Holy:
        hero.ResistHoly += value;
        break;
      case Enums.DamageType.Shadow:
        hero.ResistShadow += value;
        break;
      case Enums.DamageType.All:
        hero.ResistSlashing += value;
        hero.ResistBlunt += value;
        hero.ResistPiercing += value;
        hero.ResistFire += value;
        hero.ResistCold += value;
        hero.ResistLightning += value;
        hero.ResistMind += value;
        hero.ResistHoly += value;
        hero.ResistShadow += value;
        break;
    }
  }

  private void SetInitialCards(HeroData heroData)
  {
    if ((UnityEngine.Object) this.HeroData == (UnityEngine.Object) null)
      return;
    SubClassData heroSubClass = heroData.HeroSubClass;
    if (heroSubClass.Cards == null)
      return;
    int _rank = this.PerkRank;
    if ((UnityEngine.Object) HeroSelectionManager.Instance != (UnityEngine.Object) null)
      _rank = HeroSelectionManager.Instance.heroSelectionDictionary[this.SubclassName].rankTMHidden;
    int characterTier = PlayerManager.Instance.GetCharacterTier("", "card", _rank);
    for (int index1 = 0; index1 < heroSubClass.Cards.Length; ++index1)
    {
      if (heroSubClass.Cards[index1] != null)
      {
        for (int index2 = 0; index2 < heroSubClass.Cards[index1].UnitsInDeck; ++index2)
        {
          if (heroSubClass.Cards[index1] != null && (UnityEngine.Object) heroSubClass.Cards[index1].Card != (UnityEngine.Object) null)
          {
            string id = heroSubClass.Cards[index1].Card.Id;
            if (heroSubClass.Cards[index1].Card.Starter)
            {
              switch (characterTier)
              {
                case 1:
                  id = Globals.Instance.GetCardData(id, false).UpgradesTo1;
                  break;
                case 2:
                  id = Globals.Instance.GetCardData(id, false).UpgradesTo2;
                  break;
              }
            }
            this.Cards.Add(id);
          }
        }
      }
    }
  }

  private void SetInitialCardsSingularity(HeroData heroData)
  {
    if ((UnityEngine.Object) this.HeroData == (UnityEngine.Object) null)
      return;
    SubClassData heroSubClass = heroData.HeroSubClass;
    int num1 = 0;
    if (heroSubClass.CardsSingularity != null)
    {
      int _rank = this.PerkRank;
      if ((UnityEngine.Object) HeroSelectionManager.Instance != (UnityEngine.Object) null)
        _rank = HeroSelectionManager.Instance.heroSelectionDictionary[this.SubclassName].rankTMHidden;
      int characterTier = PlayerManager.Instance.GetCharacterTier("", "card", _rank);
      for (int index = 0; index < heroSubClass.CardsSingularity.Length; ++index)
      {
        if ((UnityEngine.Object) heroSubClass.CardsSingularity[index] != (UnityEngine.Object) null && (UnityEngine.Object) heroSubClass.CardsSingularity[index] != (UnityEngine.Object) null && (UnityEngine.Object) heroSubClass.CardsSingularity[index] != (UnityEngine.Object) null)
        {
          string id = heroSubClass.CardsSingularity[index].Id;
          if (heroSubClass.CardsSingularity[index].Starter)
          {
            switch (characterTier)
            {
              case 1:
                id = Globals.Instance.GetCardData(id, false).UpgradesTo1;
                break;
              case 2:
                id = Globals.Instance.GetCardData(id, false).UpgradesTo2;
                break;
            }
          }
          this.Cards.Add(id);
          ++num1;
        }
      }
    }
    if (num1 >= 15)
      return;
    UnityEngine.Random.InitState((AtOManager.Instance.GetGameId() + heroSubClass.Id).GetDeterministicHashCode());
    Enums.CardClass result1 = Enums.CardClass.None;
    Enum.TryParse<Enums.CardClass>(Enum.GetName(typeof (Enums.HeroClass), (object) heroData.HeroClass), out result1);
    Enums.CardClass result2 = Enums.CardClass.None;
    Enum.TryParse<Enums.CardClass>(Enum.GetName(typeof (Enums.HeroClass), (object) heroData.HeroSubClass.HeroClassSecondary), out result2);
    for (int index = 0; index < 15 - num1; ++index)
    {
      List<string> stringList1 = Globals.Instance.CardListNotUpgradedByClass[result1];
      List<string> stringList2 = result2 == Enums.CardClass.None ? new List<string>() : Globals.Instance.CardListNotUpgradedByClass[result2];
      int num2 = UnityEngine.Random.Range(0, 100);
      string str1;
      do
      {
        string str2 = stringList1[UnityEngine.Random.Range(0, stringList1.Count)];
        str1 = stringList2.Count <= 0 || num2 % 2 != 0 ? stringList1[UnityEngine.Random.Range(0, stringList1.Count)] : stringList2[UnityEngine.Random.Range(0, stringList2.Count)];
      }
      while (this.Cards.Contains(str1));
      this.Cards.Add(str1);
    }
  }

  private void SetInitialItems(CardData _cardData, int _rankLevel)
  {
    if ((UnityEngine.Object) _cardData == (UnityEngine.Object) null || !((UnityEngine.Object) _cardData.Item != (UnityEngine.Object) null))
      return;
    string id = _cardData.Id;
    switch (_rankLevel)
    {
      case 1:
        id = Globals.Instance.GetCardData(id, false).UpgradesTo1;
        break;
      case 2:
        id = Globals.Instance.GetCardData(id, false).UpgradesTo2;
        break;
    }
    if (_cardData.CardType == Enums.CardType.Weapon)
      this.Weapon = id;
    else if (_cardData.CardType == Enums.CardType.Armor)
      this.Armor = id;
    else if (_cardData.CardType == Enums.CardType.Jewelry)
      this.Jewelry = id;
    else if (_cardData.CardType == Enums.CardType.Accesory)
      this.Accesory = id;
    else if (_cardData.CardType == Enums.CardType.Pet)
      this.Pet = id;
    CardData cardData = Globals.Instance.GetCardData(id, false);
    if (!((UnityEngine.Object) cardData != (UnityEngine.Object) null))
      return;
    ItemData itemData = cardData.Item;
    if (!((UnityEngine.Object) itemData != (UnityEngine.Object) null) || itemData.MaxHealth == 0)
      return;
    this.ModifyMaxHP(itemData.MaxHealth);
  }

  public void ReassignInitialItem()
  {
    if ((UnityEngine.Object) this.HeroData == (UnityEngine.Object) null)
      return;
    int num = 0;
    if (!GameManager.Instance.IsObeliskChallenge())
    {
      int _rank = this.PerkRank;
      if ((UnityEngine.Object) HeroSelectionManager.Instance != (UnityEngine.Object) null)
        _rank = HeroSelectionManager.Instance.heroSelectionDictionary[this.SubclassName].rankTMHidden;
      num = PlayerManager.Instance.GetCharacterTier("", "item", _rank);
    }
    else if (GameManager.Instance.IsWeeklyChallenge())
    {
      num = 2;
    }
    else
    {
      int obeliskMadness = AtOManager.Instance.GetObeliskMadness();
      if (obeliskMadness >= 5)
        num = obeliskMadness > 8 ? 2 : 1;
    }
    if (num == 1)
    {
      this.SetInitialItems(this.HeroData.HeroSubClass.Item, 1);
    }
    else
    {
      if (num != 2)
        return;
      this.SetInitialItems(this.HeroData.HeroSubClass.Item, 2);
    }
  }

  public CardData GetPet()
  {
    if (this.Pet != null && this.Pet != "")
    {
      CardData cardData = Globals.Instance.GetCardData(this.Pet);
      if ((UnityEngine.Object) cardData != (UnityEngine.Object) null && cardData.CardType == Enums.CardType.Pet)
        return cardData;
    }
    return (CardData) null;
  }

  public int GetTotalCardsInDeck(bool excludeInjuriesAndBoons = false)
  {
    int count = this.Cards.Count;
    if (excludeInjuriesAndBoons)
    {
      for (int index = 0; index < this.Cards.Count; ++index)
      {
        CardData cardData = Globals.Instance.GetCardData(this.Cards[index], false);
        if (cardData.CardType == Enums.CardType.Boon || cardData.CardType == Enums.CardType.Injury)
          --count;
      }
    }
    return count;
  }

  public override void CreateOverDeck(bool getCardFromDeck)
  {
  }

  public Sprite HeroSprite
  {
    get => this.heroSprite;
    set => this.heroSprite = value;
  }

  public Sprite BorderSprite
  {
    get => this.borderSprite;
    set => this.borderSprite = value;
  }

  public GameObject GameObjectAnimated
  {
    get => this.gameObjectAnimated;
    set => this.gameObjectAnimated = value;
  }
}
