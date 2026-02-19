using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Hero : Character
{
	[SerializeField]
	private GameObject gameObjectAnimated;

	private Sprite heroSprite;

	private Sprite borderSprite;

	public Enums.DamageType PetBonusDamageType;

	public int PetBonusDamageAmount;

	public Sprite HeroSprite
	{
		get
		{
			return heroSprite;
		}
		set
		{
			heroSprite = value;
		}
	}

	public Sprite BorderSprite
	{
		get
		{
			return borderSprite;
		}
		set
		{
			borderSprite = value;
		}
	}

	public GameObject GameObjectAnimated
	{
		get
		{
			return gameObjectAnimated;
		}
		set
		{
			gameObjectAnimated = value;
		}
	}

	public void RedoSubclassFromSkin()
	{
		if (!(base.HeroData == null))
		{
			SubClassData heroSubClass = base.HeroData.HeroSubClass;
			heroSprite = heroSubClass.Sprite;
			borderSprite = heroSubClass.SpriteBorder;
			if (borderSprite == null)
			{
				Debug.LogError("Error with borderSprite");
			}
			base.SpriteSpeed = heroSubClass.SpriteSpeed;
			base.SpritePortrait = heroSubClass.SpritePortrait;
			gameObjectAnimated = heroSubClass.GameObjectAnimated;
		}
	}

	public void InitGeneralData()
	{
		if (!(base.HeroData == null))
		{
			SubClassData heroSubClass = base.HeroData.HeroSubClass;
			heroSprite = heroSubClass.Sprite;
			borderSprite = heroSubClass.SpriteBorder;
			if (borderSprite == null)
			{
				Debug.LogError("Error with borderSprite");
			}
			base.SpriteSpeed = heroSubClass.SpriteSpeed;
			base.SpritePortrait = heroSubClass.SpritePortrait;
			gameObjectAnimated = heroSubClass.GameObjectAnimated;
			base.SourceName = heroSubClass.CharacterName;
			base.ClassName = base.HeroData.HeroName.ToLower();
			base.SubclassName = heroSubClass.Id;
			if (MatchManager.Instance != null)
			{
				base.InternalId = MatchManager.Instance.GetRandomString();
			}
			else
			{
				base.InternalId = Functions.RandomString(6f);
			}
		}
	}

	public void InitHP()
	{
		if (!(base.HeroData == null))
		{
			SubClassData heroSubClass = base.HeroData.HeroSubClass;
			int num = (base.HpCurrent = heroSubClass.Hp);
			base.Hp = num;
			base.Hp += PlayerManager.Instance.GetPerkMaxHealth(heroSubClass.Id);
			base.HpCurrent += PlayerManager.Instance.GetPerkMaxHealth(heroSubClass.Id);
		}
	}

	public void InitEnergy()
	{
		if (base.HeroData == null)
		{
			return;
		}
		SubClassData heroSubClass = base.HeroData.HeroSubClass;
		base.Energy = heroSubClass.Energy + PlayerManager.Instance.GetPerkEnergyBegin(heroSubClass.Id);
		base.EnergyCurrent = base.Energy;
		if (AtOManager.Instance.IsChallengeTraitActive("energizedheroeslow"))
		{
			base.Energy++;
			base.EnergyCurrent++;
		}
		if (AtOManager.Instance.IsChallengeTraitActive("energizedheroes"))
		{
			base.Energy++;
			base.EnergyCurrent++;
		}
		int ngPlus = AtOManager.Instance.GetNgPlus();
		if (ngPlus > 3)
		{
			switch (ngPlus)
			{
			case 4:
				base.Energy--;
				base.EnergyCurrent--;
				break;
			case 5:
				base.Energy--;
				base.EnergyCurrent--;
				break;
			case 6:
				base.Energy--;
				base.EnergyCurrent--;
				break;
			case 7:
				base.Energy -= 2;
				base.EnergyCurrent -= 2;
				break;
			case 8:
				base.Energy -= 2;
				base.EnergyCurrent -= 2;
				break;
			case 9:
				base.Energy -= 2;
				base.EnergyCurrent -= 2;
				break;
			}
			if (base.Energy < 0)
			{
				base.Energy = 0;
			}
			if (base.EnergyCurrent < 0)
			{
				base.EnergyCurrent = 0;
			}
		}
		base.EnergyTurn = heroSubClass.EnergyTurn;
		if (base.Traits != null)
		{
			for (int i = 0; i < base.Traits.Length; i++)
			{
				if (base.Traits[i] != null && base.Traits[i] != "")
				{
					TraitData traitData = Globals.Instance.GetTraitData(base.Traits[i]);
					if (traitData != null && traitData.CharacterStatModified == Enums.CharacterStat.EnergyTurn)
					{
						base.EnergyTurn += traitData.CharacterStatModifiedValue;
					}
				}
			}
		}
		if (SandboxManager.Instance.IsEnabled() && AtOManager.Instance.Sandbox_startingEnergy != 0)
		{
			base.Energy += AtOManager.Instance.Sandbox_startingEnergy;
			base.EnergyCurrent += AtOManager.Instance.Sandbox_startingEnergy;
		}
	}

	public void InitSpeed()
	{
		if (!(base.HeroData == null))
		{
			SubClassData heroSubClass = base.HeroData.HeroSubClass;
			base.Speed = heroSubClass.Speed;
			base.Speed += PlayerManager.Instance.GetPerkSpeed(heroSubClass.Id);
			if (AtOManager.Instance.IsChallengeTraitActive("slowheroes"))
			{
				base.Speed--;
			}
			if (AtOManager.Instance.IsChallengeTraitActive("fastheroes"))
			{
				base.Speed++;
			}
			if (SandboxManager.Instance.IsEnabled() && AtOManager.Instance.Sandbox_startingSpeed != 0)
			{
				base.Speed += AtOManager.Instance.Sandbox_startingSpeed;
			}
		}
	}

	public void InitResist()
	{
		if (!(base.HeroData == null))
		{
			SubClassData heroSubClass = base.HeroData.HeroSubClass;
			base.ResistSlashing = heroSubClass.ResistSlashing + PlayerManager.Instance.GetPerkResistBonus(heroSubClass.Id, Enums.DamageType.Slashing);
			if (base.ResistSlashing >= 100)
			{
				base.ImmuneSlashing = true;
			}
			base.ResistBlunt = heroSubClass.ResistBlunt + PlayerManager.Instance.GetPerkResistBonus(heroSubClass.Id, Enums.DamageType.Blunt);
			if (base.ResistBlunt >= 100)
			{
				base.ImmuneBlunt = true;
			}
			base.ResistPiercing = heroSubClass.ResistPiercing + PlayerManager.Instance.GetPerkResistBonus(heroSubClass.Id, Enums.DamageType.Piercing);
			if (base.ResistPiercing >= 100)
			{
				base.ImmunePiercing = true;
			}
			base.ResistFire = heroSubClass.ResistFire + PlayerManager.Instance.GetPerkResistBonus(heroSubClass.Id, Enums.DamageType.Fire);
			if (base.ResistFire >= 100)
			{
				base.ImmuneFire = true;
			}
			base.ResistCold = heroSubClass.ResistCold + PlayerManager.Instance.GetPerkResistBonus(heroSubClass.Id, Enums.DamageType.Cold);
			if (base.ResistCold >= 100)
			{
				base.ImmuneCold = true;
			}
			base.ResistLightning = heroSubClass.ResistLightning + PlayerManager.Instance.GetPerkResistBonus(heroSubClass.Id, Enums.DamageType.Lightning);
			if (base.ResistLightning >= 100)
			{
				base.ImmuneLightning = true;
			}
			base.ResistMind = heroSubClass.ResistMind + PlayerManager.Instance.GetPerkResistBonus(heroSubClass.Id, Enums.DamageType.Mind);
			if (base.ResistMind >= 100)
			{
				base.ImmuneMind = true;
			}
			base.ResistHoly = heroSubClass.ResistHoly + PlayerManager.Instance.GetPerkResistBonus(heroSubClass.Id, Enums.DamageType.Holy);
			if (base.ResistHoly >= 100)
			{
				base.ImmuneHoly = true;
			}
			base.ResistShadow = heroSubClass.ResistShadow + PlayerManager.Instance.GetPerkResistBonus(heroSubClass.Id, Enums.DamageType.Shadow);
			if (base.ResistShadow >= 100)
			{
				base.ImmuneShadow = true;
			}
		}
	}

	public override void InitData()
	{
		if (!(base.HeroData != null))
		{
			return;
		}
		InitGeneralData();
		SubClassData heroSubClass = base.HeroData.HeroSubClass;
		base.Id = heroSubClass.Id + "_" + base.InternalId;
		InitHP();
		InitEnergy();
		InitSpeed();
		base.Experience = 0;
		base.Level = 1;
		base.IsHero = true;
		InitResist();
		base.Alive = true;
		base.AuraList = new List<Aura>();
		base.Traits = new string[5];
		SetInitialItems(heroSubClass.Item, 0);
		if (GameManager.Instance.IsObeliskChallenge())
		{
			ReassignInitialItem();
		}
		base.Cards = new List<string>();
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			if (GameManager.Instance.IsGameAdventure())
			{
				SetInitialCards(base.HeroData);
			}
			else
			{
				SetInitialCardsSingularity(base.HeroData);
			}
		}
		PetBonusDamageType = Enums.DamageType.None;
		PetBonusDamageAmount = 0;
	}

	public void AssignOwner(string nickName)
	{
		base.Owner = nickName;
		if (base.OwnerOriginal == "")
		{
			base.OwnerOriginal = NetworkManager.Instance.GetPlayerNickReal(nickName);
		}
	}

	public override void BeginTurn()
	{
		base.BeginTurn();
	}

	public override void BeginTurnPerks()
	{
		base.BeginTurnPerks();
	}

	public bool AssignTrait(string traitName)
	{
		TraitData traitData = Globals.Instance.GetTraitData(traitName);
		if (traitData != null)
		{
			if (traitData.Id == "eternalbond")
			{
				base.Pet = "harleyrare";
			}
			else if (traitData.Id == "templelurkers")
			{
				if (base.Pet.StartsWith("cocoon", StringComparison.OrdinalIgnoreCase) || base.Pet.StartsWith("liante", StringComparison.OrdinalIgnoreCase))
				{
					base.Pet = "templelurkerpet";
				}
			}
			else if (traitData.Id == "mentalscavengers")
			{
				if (base.Pet.StartsWith("cocoon", StringComparison.OrdinalIgnoreCase) || base.Pet.StartsWith("liante", StringComparison.OrdinalIgnoreCase))
				{
					base.Pet = "mentalscavengerspet";
				}
			}
			else if (traitData.Id == "spiderqueen")
			{
				if (base.Pet == "templelurkerpet")
				{
					base.Pet = "templelurkerpetrare";
				}
				else if (base.Pet == "mentalscavengerspet")
				{
					base.Pet = "mentalscavengerspetrare";
				}
			}
			int traitLevel = base.HeroData.HeroSubClass.GetTraitLevel(traitName);
			if (traitLevel < 0)
			{
				return false;
			}
			if (base.Traits[traitLevel] != null && base.Traits[traitLevel] != "")
			{
				return false;
			}
			base.Traits[traitLevel] = traitName.ToLower();
			if (traitData.CharacterStatModified != Enums.CharacterStat.None)
			{
				if (traitData.CharacterStatModified == Enums.CharacterStat.Hp)
				{
					if (base.Hp == base.HpCurrent)
					{
						base.HpCurrent += traitData.CharacterStatModifiedValue;
					}
					base.Hp += traitData.CharacterStatModifiedValue;
				}
				else if (traitData.CharacterStatModified == Enums.CharacterStat.Speed)
				{
					base.Speed += traitData.CharacterStatModifiedValue;
				}
				else if (traitData.CharacterStatModified == Enums.CharacterStat.Energy)
				{
					base.Energy += traitData.CharacterStatModifiedValue;
				}
				else if (traitData.CharacterStatModified == Enums.CharacterStat.EnergyTurn)
				{
					base.EnergyTurn += traitData.CharacterStatModifiedValue;
				}
			}
			if (traitData.ResistModified1 != Enums.DamageType.None)
			{
				ModifyResist(this, traitData.ResistModified1, traitData.ResistModifiedValue1);
			}
			if (traitData.ResistModified2 != Enums.DamageType.None)
			{
				ModifyResist(this, traitData.ResistModified2, traitData.ResistModifiedValue2);
			}
			if (traitData.ResistModified3 != Enums.DamageType.None)
			{
				ModifyResist(this, traitData.ResistModified3, traitData.ResistModifiedValue3);
			}
			if (traitData.AuracurseImmune1.Trim() != "")
			{
				base.AuracurseImmune.Add(traitData.AuracurseImmune1.Trim().ToLower());
			}
			if (traitData.AuracurseImmune2.Trim() != "")
			{
				base.AuracurseImmune.Add(traitData.AuracurseImmune2.Trim().ToLower());
			}
			if (traitData.AuracurseImmune3.Trim() != "")
			{
				base.AuracurseImmune.Add(traitData.AuracurseImmune3.Trim().ToLower());
			}
			ClearCaches();
			return true;
		}
		return false;
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
		if (base.HeroData == null)
		{
			return;
		}
		SubClassData heroSubClass = heroData.HeroSubClass;
		if (heroSubClass.Cards == null)
		{
			return;
		}
		int rankTMHidden = base.PerkRank;
		if (HeroSelectionManager.Instance != null)
		{
			rankTMHidden = HeroSelectionManager.Instance.heroSelectionDictionary[base.SubclassName].rankTMHidden;
		}
		int characterTier = PlayerManager.Instance.GetCharacterTier("", "card", rankTMHidden);
		for (int i = 0; i < heroSubClass.Cards.Length; i++)
		{
			if (heroSubClass.Cards[i] == null)
			{
				continue;
			}
			for (int j = 0; j < heroSubClass.Cards[i].UnitsInDeck; j++)
			{
				if (heroSubClass.Cards[i] == null || !(heroSubClass.Cards[i].Card != null))
				{
					continue;
				}
				string item = heroSubClass.Cards[i].Card.Id;
				if (heroSubClass.Cards[i].Card.Starter)
				{
					switch (characterTier)
					{
					case 1:
						item = Globals.Instance.GetCardData(item, instantiate: false).UpgradesTo1;
						break;
					case 2:
						item = Globals.Instance.GetCardData(item, instantiate: false).UpgradesTo2;
						break;
					}
				}
				base.Cards.Add(item);
			}
		}
	}

	private void SetInitialCardsSingularity(HeroData heroData)
	{
		if (base.HeroData == null)
		{
			return;
		}
		SubClassData heroSubClass = heroData.HeroSubClass;
		int num = 0;
		if (heroSubClass.CardsSingularity != null)
		{
			int rankTMHidden = base.PerkRank;
			if (HeroSelectionManager.Instance != null)
			{
				rankTMHidden = HeroSelectionManager.Instance.heroSelectionDictionary[base.SubclassName].rankTMHidden;
			}
			int characterTier = PlayerManager.Instance.GetCharacterTier("", "card", rankTMHidden);
			for (int i = 0; i < heroSubClass.CardsSingularity.Length; i++)
			{
				if (!(heroSubClass.CardsSingularity[i] != null) || !(heroSubClass.CardsSingularity[i] != null) || !(heroSubClass.CardsSingularity[i] != null))
				{
					continue;
				}
				string item = heroSubClass.CardsSingularity[i].Id;
				if (heroSubClass.CardsSingularity[i].Starter)
				{
					switch (characterTier)
					{
					case 1:
						item = Globals.Instance.GetCardData(item, instantiate: false).UpgradesTo1;
						break;
					case 2:
						item = Globals.Instance.GetCardData(item, instantiate: false).UpgradesTo2;
						break;
					}
				}
				base.Cards.Add(item);
				num++;
			}
		}
		if (num >= 15)
		{
			return;
		}
		UnityEngine.Random.InitState((AtOManager.Instance.GetGameId() + heroSubClass.Id).GetDeterministicHashCode());
		Enums.CardClass result = Enums.CardClass.None;
		Enum.TryParse<Enums.CardClass>(Enum.GetName(typeof(Enums.HeroClass), heroData.HeroClass), out result);
		Enums.CardClass result2 = Enums.CardClass.None;
		Enum.TryParse<Enums.CardClass>(Enum.GetName(typeof(Enums.HeroClass), heroData.HeroSubClass.HeroClassSecondary), out result2);
		for (int j = 0; j < 15 - num; j++)
		{
			List<string> list = Globals.Instance.CardListNotUpgradedByClass[result];
			List<string> list2 = ((result2 == Enums.CardClass.None) ? new List<string>() : Globals.Instance.CardListNotUpgradedByClass[result2]);
			int num2 = UnityEngine.Random.Range(0, 100);
			string text;
			do
			{
				text = list[UnityEngine.Random.Range(0, list.Count)];
				text = ((list2.Count <= 0 || num2 % 2 != 0) ? list[UnityEngine.Random.Range(0, list.Count)] : list2[UnityEngine.Random.Range(0, list2.Count)]);
			}
			while (base.Cards.Contains(text));
			base.Cards.Add(text);
		}
	}

	private void SetInitialItems(CardData _cardData, int _rankLevel)
	{
		if (_cardData == null || !(_cardData.Item != null))
		{
			return;
		}
		string text = _cardData.Id;
		switch (_rankLevel)
		{
		case 1:
			text = Globals.Instance.GetCardData(text, instantiate: false).UpgradesTo1;
			break;
		case 2:
			text = Globals.Instance.GetCardData(text, instantiate: false).UpgradesTo2;
			break;
		}
		if (_cardData.CardType == Enums.CardType.Weapon)
		{
			base.Weapon = text;
		}
		else if (_cardData.CardType == Enums.CardType.Armor)
		{
			base.Armor = text;
		}
		else if (_cardData.CardType == Enums.CardType.Jewelry)
		{
			base.Jewelry = text;
		}
		else if (_cardData.CardType == Enums.CardType.Accesory)
		{
			base.Accesory = text;
		}
		else if (_cardData.CardType == Enums.CardType.Pet)
		{
			base.Pet = text;
		}
		CardData cardData = Globals.Instance.GetCardData(text, instantiate: false);
		if (cardData != null)
		{
			ItemData item = cardData.Item;
			if (item != null && item.MaxHealth != 0)
			{
				ModifyMaxHP(item.MaxHealth);
			}
		}
	}

	public void ReassignInitialItem()
	{
		if (base.HeroData == null)
		{
			return;
		}
		int num = 0;
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			int rankTMHidden = base.PerkRank;
			if (HeroSelectionManager.Instance != null)
			{
				rankTMHidden = HeroSelectionManager.Instance.heroSelectionDictionary[base.SubclassName].rankTMHidden;
			}
			num = PlayerManager.Instance.GetCharacterTier("", "item", rankTMHidden);
		}
		else if (GameManager.Instance.IsWeeklyChallenge())
		{
			num = 2;
		}
		else
		{
			int obeliskMadness = AtOManager.Instance.GetObeliskMadness();
			if (obeliskMadness >= 5)
			{
				num = ((obeliskMadness < 8) ? 1 : 2);
			}
		}
		switch (num)
		{
		case 1:
			SetInitialItems(base.HeroData.HeroSubClass.Item, 1);
			break;
		case 2:
			SetInitialItems(base.HeroData.HeroSubClass.Item, 2);
			break;
		}
	}

	public CardData GetPet()
	{
		if (base.Pet != null && base.Pet != "")
		{
			CardData cardData = Globals.Instance.GetCardData(base.Pet);
			if (cardData != null && cardData.CardType == Enums.CardType.Pet)
			{
				return cardData;
			}
		}
		return null;
	}

	public int GetTotalCardsInDeck(bool excludeInjuriesAndBoons = false)
	{
		int num = base.Cards.Count;
		if (excludeInjuriesAndBoons)
		{
			for (int i = 0; i < base.Cards.Count; i++)
			{
				CardData cardData = Globals.Instance.GetCardData(base.Cards[i], instantiate: false);
				if (cardData.CardType == Enums.CardType.Boon || cardData.CardType == Enums.CardType.Injury)
				{
					num--;
				}
			}
		}
		return num;
	}

	public override void CreateOverDeck(bool getCardFromDeck, bool maxOneCard = false)
	{
	}
}
