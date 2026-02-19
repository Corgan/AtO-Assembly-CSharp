using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cards;
using UnityEngine;

[Serializable]
public class Character
{
	internal Trait traitClass = new Trait();

	private Item itemClass = new Item();

	[SerializeField]
	private string internalId = "";

	[SerializeField]
	private string owner;

	[SerializeField]
	private string ownerOriginal = "";

	[SerializeField]
	private int perkRank;

	[SerializeField]
	private List<string> perkList;

	[SerializeField]
	private HeroData heroData;

	[SerializeField]
	private float heroGold;

	[SerializeField]
	private float heroDust;

	[SerializeField]
	private int heroIndex;

	[SerializeField]
	private NPCData npcData;

	[SerializeField]
	private int npcIndex;

	[SerializeField]
	private string id;

	[SerializeField]
	private string className;

	[SerializeField]
	private string subclassName;

	[SerializeField]
	private string scriptableObjectName;

	[SerializeField]
	private string sourceName;

	[SerializeField]
	private string gameName;

	[SerializeField]
	private Sprite spriteSpeed;

	[SerializeField]
	private Sprite spritePortrait;

	[SerializeField]
	private int position;

	[SerializeField]
	private int level;

	[SerializeField]
	private int experience;

	[SerializeField]
	private int hp;

	[SerializeField]
	private int hpCurrent;

	[SerializeField]
	private int energy;

	[SerializeField]
	private int energyCurrent;

	[SerializeField]
	private int energyTurn;

	[SerializeField]
	private int block;

	public bool consumeBlock = true;

	[SerializeField]
	private int speed;

	[SerializeField]
	private int drawModifier;

	[SerializeField]
	private List<string> cards;

	[SerializeField]
	private string[] traits;

	[SerializeField]
	private string weapon = "";

	[SerializeField]
	private string armor = "";

	[SerializeField]
	private string jewelry = "";

	[SerializeField]
	private string accesory = "";

	[SerializeField]
	private string corruption = "";

	[SerializeField]
	private string pet = "";

	[SerializeField]
	private string enchantment = "";

	[SerializeField]
	private string enchantment2 = "";

	[SerializeField]
	private string enchantment3 = "";

	[SerializeField]
	private int resistSlashing;

	private bool immuneSlashing;

	[SerializeField]
	private int resistBlunt;

	private bool immuneBlunt;

	[SerializeField]
	private int resistPiercing;

	private bool immunePiercing;

	[SerializeField]
	private int resistFire;

	private bool immuneFire;

	[SerializeField]
	private int resistCold;

	private bool immuneCold;

	[SerializeField]
	private int resistLightning;

	private bool immuneLightning;

	[SerializeField]
	private int resistMind;

	private bool immuneMind;

	[SerializeField]
	private int resistHoly;

	private bool immuneHoly;

	[SerializeField]
	private int resistShadow;

	private bool immuneShadow;

	[SerializeField]
	private bool alive;

	[SerializeField]
	private int totalDeaths;

	[SerializeField]
	private List<Aura> auraList = new List<Aura>();

	[SerializeField]
	private List<string> auracurseImmune = new List<string>();

	[SerializeField]
	private bool isHero;

	[SerializeField]
	private HeroItem heroItem;

	[SerializeField]
	private NPCItem npcItem;

	[SerializeField]
	private int roundMoved;

	[SerializeField]
	private GameObject gO;

	[SerializeField]
	private int craftRemainingUses;

	[SerializeField]
	private int cardsUpgraded;

	[SerializeField]
	private int cardsRemoved;

	[SerializeField]
	private int cardsCrafted;

	[SerializeField]
	private string skinUsed = "";

	[SerializeField]
	private string cardbackUsed = "";

	private Dictionary<Enums.CardType, int> cardsCostModification = new Dictionary<Enums.CardType, int>();

	private Dictionary<string, int> auraCurseModification = new Dictionary<string, int>();

	private Dictionary<string, int> auraCurseModificationDueToAuras = new Dictionary<string, int>();

	private List<CardData> cardsPlayed = new List<CardData>();

	private List<CardData> cardsPlayedRound = new List<CardData>();

	private Dictionary<Enums.DamageType, int> ResistModsWithItems = new Dictionary<Enums.DamageType, int>();

	private CardData cardCasted;

	private int itemSlots = 9;

	private ItemData[] itemDataBySlot = new ItemData[9];

	private CardData[] cardDataBySlot = new CardData[9];

	private int perk_sparkCharges;

	private int spellswordTMP = -1;

	private Dictionary<string, int> cacheGetAuraResistModifiers = new Dictionary<string, int>();

	private Dictionary<string, int> cacheGetAuraStatModifiers = new Dictionary<string, int>();

	private Dictionary<string, int> cachePerkGetAuraCurseBonusDict = new Dictionary<string, int>();

	private Dictionary<string, int> cacheGetTraitAuraCurseModifiers = new Dictionary<string, int>();

	private Dictionary<Enums.DamageType, int> cacheGetTraitDamageFlatModifiers = new Dictionary<Enums.DamageType, int>();

	private Dictionary<Enums.DamageType, float> cacheGetTraitDamagePercentModifiers = new Dictionary<Enums.DamageType, float>();

	private List<int> cacheGetTraitHealFlatBonus = new List<int>();

	private List<float> cacheGetTraitHealPercentBonus = new List<float>();

	private List<int> cacheGetTraitHealReceivedFlatBonus = new List<int>();

	private List<float> cacheGetTraitHealReceivedPercentBonus = new List<float>();

	internal List<float> CacheGetHealRecievedPercentBonusFromAllyTrait = new List<float>();

	private Dictionary<string, bool> cacheAuraCurseImmuneByItems = new Dictionary<string, bool>();

	private Dictionary<string, int> cacheGetItemAuraCurseModifiers = new Dictionary<string, int>();

	private Dictionary<Enums.CharacterStat, int> cacheGetItemStatModifiers = new Dictionary<Enums.CharacterStat, int>();

	private Dictionary<Enums.DamageType, int> cacheGetItemResistModifiers = new Dictionary<Enums.DamageType, int>();

	private Dictionary<Enums.DamageType, int> cacheGetItemDamageFlatModifiers = new Dictionary<Enums.DamageType, int>();

	private Dictionary<Enums.DamageType, float> cacheGetItemDamagePercentModifiers = new Dictionary<Enums.DamageType, float>();

	private List<int> cacheGetItemHealFlatBonus = new List<int>();

	private List<float> cacheGetItemHealPercentBonus = new List<float>();

	private List<int> cacheGetItemHealReceivedFlatBonus = new List<int>();

	private List<float> cacheGetItemHealReceivedPercentBonus = new List<float>();

	private bool useCache = true;

	private int blockChargesLastChecked;

	private int bleedRecievedByBloodMage;

	public bool TransformedModel;

	private bool isIllusion;

	private bool isIllusionExposed;

	private Character illusionCharacter;

	private bool isSummon;

	public string InternalId
	{
		get
		{
			return internalId;
		}
		set
		{
			internalId = value;
		}
	}

	public bool IsIllusion
	{
		get
		{
			return isIllusion;
		}
		set
		{
			isIllusion = value;
		}
	}

	public bool IsIllusionExposed
	{
		get
		{
			return isIllusionExposed;
		}
		set
		{
			isIllusionExposed = value;
		}
	}

	public Character IllusionCharacter
	{
		get
		{
			return illusionCharacter;
		}
		set
		{
			illusionCharacter = value;
		}
	}

	public bool IsSummon
	{
		get
		{
			return isSummon;
		}
		set
		{
			isSummon = value;
		}
	}

	public string Owner
	{
		get
		{
			return owner;
		}
		set
		{
			owner = value;
		}
	}

	public HeroData HeroData
	{
		get
		{
			return heroData;
		}
		set
		{
			heroData = value;
		}
	}

	public NPCData NpcData
	{
		get
		{
			return npcData;
		}
		set
		{
			npcData = value;
		}
	}

	public int HeroIndex
	{
		get
		{
			return heroIndex;
		}
		set
		{
			heroIndex = value;
		}
	}

	public int NPCIndex
	{
		get
		{
			return npcIndex;
		}
		set
		{
			npcIndex = value;
		}
	}

	public string Id
	{
		get
		{
			return id;
		}
		set
		{
			id = value;
		}
	}

	public string SubclassName
	{
		get
		{
			return subclassName;
		}
		set
		{
			subclassName = value;
		}
	}

	public string ClassName
	{
		get
		{
			return className;
		}
		set
		{
			className = value;
		}
	}

	public string SourceName
	{
		get
		{
			return sourceName;
		}
		set
		{
			sourceName = value;
		}
	}

	public string GameName
	{
		get
		{
			return gameName;
		}
		set
		{
			gameName = value;
		}
	}

	public int Position
	{
		get
		{
			return position;
		}
		set
		{
			position = value;
		}
	}

	public int Experience
	{
		get
		{
			return experience;
		}
		set
		{
			experience = value;
		}
	}

	public int Level
	{
		get
		{
			return level;
		}
		set
		{
			level = value;
		}
	}

	public int Hp
	{
		get
		{
			return hp;
		}
		set
		{
			hp = value;
		}
	}

	public int HpCurrent
	{
		get
		{
			return hpCurrent;
		}
		set
		{
			hpCurrent = value;
		}
	}

	public int Energy
	{
		get
		{
			return energy;
		}
		set
		{
			energy = value;
		}
	}

	public int EnergyCurrent
	{
		get
		{
			return energyCurrent;
		}
		set
		{
			energyCurrent = value;
		}
	}

	public int EnergyTurn
	{
		get
		{
			return energyTurn;
		}
		set
		{
			energyTurn = value;
		}
	}

	public int Speed
	{
		get
		{
			return speed;
		}
		set
		{
			speed = value;
		}
	}

	public bool Alive
	{
		get
		{
			return alive;
		}
		set
		{
			alive = value;
		}
	}

	public List<string> Cards
	{
		get
		{
			return cards;
		}
		set
		{
			cards = value;
		}
	}

	public string[] Traits
	{
		get
		{
			return traits;
		}
		set
		{
			traits = value;
		}
	}

	public List<Aura> AuraList
	{
		get
		{
			return auraList;
		}
		set
		{
			auraList = value;
		}
	}

	public List<string> AuracurseImmune
	{
		get
		{
			return auracurseImmune;
		}
		set
		{
			auracurseImmune = value;
		}
	}

	public bool IsHero
	{
		get
		{
			return isHero;
		}
		set
		{
			isHero = value;
		}
	}

	public HeroItem HeroItem
	{
		get
		{
			return heroItem;
		}
		set
		{
			heroItem = value;
		}
	}

	public NPCItem NPCItem
	{
		get
		{
			return npcItem;
		}
		set
		{
			npcItem = value;
		}
	}

	public int RoundMoved
	{
		get
		{
			return roundMoved;
		}
		set
		{
			roundMoved = value;
		}
	}

	public GameObject GO
	{
		get
		{
			return gO;
		}
		set
		{
			gO = value;
		}
	}

	public int ResistSlashing
	{
		get
		{
			return resistSlashing;
		}
		set
		{
			resistSlashing = value;
		}
	}

	public bool ImmuneSlashing
	{
		get
		{
			return immuneSlashing;
		}
		set
		{
			immuneSlashing = value;
		}
	}

	public int ResistBlunt
	{
		get
		{
			return resistBlunt;
		}
		set
		{
			resistBlunt = value;
		}
	}

	public bool ImmuneBlunt
	{
		get
		{
			return immuneBlunt;
		}
		set
		{
			immuneBlunt = value;
		}
	}

	public int ResistPiercing
	{
		get
		{
			return resistPiercing;
		}
		set
		{
			resistPiercing = value;
		}
	}

	public bool ImmunePiercing
	{
		get
		{
			return immunePiercing;
		}
		set
		{
			immunePiercing = value;
		}
	}

	public int ResistFire
	{
		get
		{
			return resistFire;
		}
		set
		{
			resistFire = value;
		}
	}

	public bool ImmuneFire
	{
		get
		{
			return immuneFire;
		}
		set
		{
			immuneFire = value;
		}
	}

	public int ResistCold
	{
		get
		{
			return resistCold;
		}
		set
		{
			resistCold = value;
		}
	}

	public bool ImmuneCold
	{
		get
		{
			return immuneCold;
		}
		set
		{
			immuneCold = value;
		}
	}

	public int ResistLightning
	{
		get
		{
			return resistLightning;
		}
		set
		{
			resistLightning = value;
		}
	}

	public bool ImmuneLightning
	{
		get
		{
			return immuneLightning;
		}
		set
		{
			immuneLightning = value;
		}
	}

	public int ResistMind
	{
		get
		{
			return resistMind;
		}
		set
		{
			resistMind = value;
		}
	}

	public bool ImmuneMind
	{
		get
		{
			return immuneMind;
		}
		set
		{
			immuneMind = value;
		}
	}

	public int ResistHoly
	{
		get
		{
			return resistHoly;
		}
		set
		{
			resistHoly = value;
		}
	}

	public bool ImmuneHoly
	{
		get
		{
			return immuneHoly;
		}
		set
		{
			immuneHoly = value;
		}
	}

	public int ResistShadow
	{
		get
		{
			return resistShadow;
		}
		set
		{
			resistShadow = value;
		}
	}

	public bool ImmuneShadow
	{
		get
		{
			return immuneShadow;
		}
		set
		{
			immuneShadow = value;
		}
	}

	public Sprite SpriteSpeed
	{
		get
		{
			return spriteSpeed;
		}
		set
		{
			spriteSpeed = value;
		}
	}

	public Sprite SpritePortrait
	{
		get
		{
			return spritePortrait;
		}
		set
		{
			spritePortrait = value;
		}
	}

	public Dictionary<string, int> AuraCurseModification
	{
		get
		{
			return auraCurseModification;
		}
		set
		{
			auraCurseModification = value;
		}
	}

	public Dictionary<string, int> AuraCurseModificationDueToAuras
	{
		get
		{
			return auraCurseModificationDueToAuras;
		}
		set
		{
			auraCurseModificationDueToAuras = value;
		}
	}

	public List<CardData> CardsPlayed
	{
		get
		{
			return cardsPlayed;
		}
		set
		{
			cardsPlayed = value;
		}
	}

	public List<CardData> CardsPlayedRound
	{
		get
		{
			return cardsPlayedRound;
		}
		set
		{
			cardsPlayedRound = value;
		}
	}

	public int CraftRemainingUses
	{
		get
		{
			return craftRemainingUses;
		}
		set
		{
			craftRemainingUses = value;
		}
	}

	public int CardsUpgraded
	{
		get
		{
			return cardsUpgraded;
		}
		set
		{
			cardsUpgraded = value;
		}
	}

	public int CardsRemoved
	{
		get
		{
			return cardsRemoved;
		}
		set
		{
			cardsRemoved = value;
		}
	}

	public int CardsCrafted
	{
		get
		{
			return cardsCrafted;
		}
		set
		{
			cardsCrafted = value;
		}
	}

	public string Weapon
	{
		get
		{
			return weapon;
		}
		set
		{
			weapon = value;
		}
	}

	public string Armor
	{
		get
		{
			return armor;
		}
		set
		{
			armor = value;
		}
	}

	public string Jewelry
	{
		get
		{
			return jewelry;
		}
		set
		{
			jewelry = value;
		}
	}

	public string Accesory
	{
		get
		{
			return accesory;
		}
		set
		{
			accesory = value;
		}
	}

	public string Corruption
	{
		get
		{
			return corruption;
		}
		set
		{
			corruption = value;
		}
	}

	public string Pet
	{
		get
		{
			return pet;
		}
		set
		{
			pet = value;
		}
	}

	public string Enchantment
	{
		get
		{
			return enchantment;
		}
		set
		{
			enchantment = value;
		}
	}

	public string Enchantment2
	{
		get
		{
			return enchantment2;
		}
		set
		{
			enchantment2 = value;
		}
	}

	public string Enchantment3
	{
		get
		{
			return enchantment3;
		}
		set
		{
			enchantment3 = value;
		}
	}

	public int PerkRank
	{
		get
		{
			return perkRank;
		}
		set
		{
			perkRank = value;
		}
	}

	public List<string> PerkList
	{
		get
		{
			return perkList;
		}
		set
		{
			perkList = value;
		}
	}

	public float HeroGold
	{
		get
		{
			return heroGold;
		}
		set
		{
			heroGold = value;
		}
	}

	public float HeroDust
	{
		get
		{
			return heroDust;
		}
		set
		{
			heroDust = value;
		}
	}

	public CardData CardCasted
	{
		get
		{
			return cardCasted;
		}
		set
		{
			cardCasted = value;
		}
	}

	public string SkinUsed
	{
		get
		{
			return skinUsed;
		}
		set
		{
			skinUsed = value;
		}
	}

	public string OwnerOriginal
	{
		get
		{
			return ownerOriginal;
		}
		set
		{
			ownerOriginal = value;
		}
	}

	public int TotalDeaths
	{
		get
		{
			return totalDeaths;
		}
		set
		{
			totalDeaths = value;
		}
	}

	public string CardbackUsed
	{
		get
		{
			return cardbackUsed;
		}
		set
		{
			cardbackUsed = value;
		}
	}

	public string ScriptableObjectName
	{
		get
		{
			return scriptableObjectName;
		}
		set
		{
			scriptableObjectName = value;
		}
	}

	public int BleedRecievedByBloodMage
	{
		get
		{
			return bleedRecievedByBloodMage;
		}
		set
		{
			bleedRecievedByBloodMage = value;
		}
	}

	public List<float> CacheGetTraitHealReceivedPercentBonus
	{
		get
		{
			return cacheGetTraitHealReceivedPercentBonus;
		}
		set
		{
			cacheGetTraitHealReceivedPercentBonus = value;
		}
	}

	public void ResetDataForNewCombat(bool clear = true)
	{
		if (clear)
		{
			auraCurseModification.Clear();
			cardsCostModification.Clear();
		}
		if (heroData != null)
		{
			auraCurseModification = Perk.GetAuraCurseBonusDict(heroData.HeroSubClass.Id);
		}
		ResistModsWithItems.Clear();
		RemoveAuraCurses();
		energyCurrent = energy;
		if (hpCurrent > hp)
		{
			hpCurrent = hp;
		}
		roundMoved = 0;
		block = 0;
		drawModifier = 0;
		consumeBlock = true;
		corruption = "";
		enchantment = "";
		enchantment2 = "";
		enchantment3 = "";
		spellswordTMP = -1;
		itemDataBySlot = new ItemData[itemSlots];
		cardDataBySlot = new CardData[itemSlots];
		ClearCaches();
	}

	public void ClearCaches()
	{
		ClearCacheItems();
		ClearCacheTraits();
		ClearCachePerks();
		ClearAuraModifiers();
	}

	private void ClearAuraModifiers()
	{
		cacheGetAuraResistModifiers.Clear();
		cacheGetAuraStatModifiers.Clear();
	}

	private void ClearCachePerks()
	{
		cachePerkGetAuraCurseBonusDict.Clear();
	}

	private void ClearCacheItems()
	{
		cacheAuraCurseImmuneByItems.Clear();
		cacheGetItemAuraCurseModifiers.Clear();
		cacheGetItemStatModifiers.Clear();
		cacheGetItemResistModifiers.Clear();
		cacheGetItemDamageFlatModifiers.Clear();
		cacheGetItemDamagePercentModifiers.Clear();
		cacheGetItemHealFlatBonus.Clear();
		cacheGetItemHealPercentBonus.Clear();
		cacheGetItemHealReceivedFlatBonus.Clear();
		cacheGetItemHealReceivedPercentBonus.Clear();
	}

	private void ClearCacheTraits()
	{
		cacheGetTraitAuraCurseModifiers.Clear();
		cacheGetTraitDamageFlatModifiers.Clear();
		cacheGetTraitDamagePercentModifiers.Clear();
		cacheGetTraitHealFlatBonus.Clear();
		cacheGetTraitHealPercentBonus.Clear();
		cacheGetTraitHealReceivedFlatBonus.Clear();
		cacheGetTraitHealReceivedPercentBonus.Clear();
		CacheGetHealRecievedPercentBonusFromAllyTrait.Clear();
	}

	public void ModifyCardsCost(Enums.CardType type, int cost)
	{
		if (!cardsCostModification.ContainsKey(type))
		{
			cardsCostModification[type] = cost;
		}
		else
		{
			cardsCostModification[type] += cost;
		}
	}

	public int GetCardsCostModification(List<Enums.CardType> types)
	{
		int num = 0;
		for (int i = 0; i < types.Count; i++)
		{
			if (cardsCostModification.ContainsKey(types[i]))
			{
				num += cardsCostModification[types[i]];
			}
		}
		return num;
	}

	public void ModifyAuraCurseCharges(string _id, int _charges)
	{
		id = id.Trim().ToLower();
		for (int i = 0; i < auraList.Count; i++)
		{
			if (auraList[i].ACData != null && auraList[i].ACData.Id == _id)
			{
				auraList[i].AuraCharges = _charges;
				break;
			}
		}
	}

	public void ModifyAuraCurseQuantity(string id, int cost)
	{
		id = id.Trim().ToLower();
		if (!auraCurseModification.ContainsKey(id))
		{
			auraCurseModification[id] = cost;
		}
		else
		{
			auraCurseModification[id] += cost;
		}
	}

	public int GetAuraCurseQuantityModification(string id, Enums.CardClass CC)
	{
		id = id.Trim().ToLower();
		if (isHero && (CC == Enums.CardClass.Monster || CC == Enums.CardClass.Injury || CC == Enums.CardClass.Boon))
		{
			return 0;
		}
		if (!isHero)
		{
			switch (id)
			{
			case "doom":
			case "paralyze":
			case "invulnerable":
			case "stress":
			case "fatigue":
			case "invulnerableunremovable":
			case "sleep":
				return 0;
			default:
			{
				int num = 0;
				Dictionary<string, int> traitAuraCurseModifiers = GetTraitAuraCurseModifiers();
				if (traitAuraCurseModifiers != null && traitAuraCurseModifiers.ContainsKey(id))
				{
					num += traitAuraCurseModifiers[id];
				}
				if (MadnessManager.Instance.IsMadnessTraitActive("overchargedmonsters") || AtOManager.Instance.IsChallengeTraitActive("overchargedmonsters"))
				{
					num++;
				}
				return num;
			}
			}
		}
		if ((bool)TownManager.Instance || (bool)MapManager.Instance || (bool)MatchManager.Instance || (bool)RewardsManager.Instance || (bool)LootManager.Instance)
		{
			Dictionary<string, int> auraCurseBonusDict;
			if ((bool)MatchManager.Instance && useCache)
			{
				if (cachePerkGetAuraCurseBonusDict.Count == 0)
				{
					cachePerkGetAuraCurseBonusDict = Perk.GetAuraCurseBonusDict(heroData.HeroSubClass.Id);
				}
				auraCurseBonusDict = cachePerkGetAuraCurseBonusDict;
			}
			else
			{
				auraCurseBonusDict = Perk.GetAuraCurseBonusDict(heroData.HeroSubClass.Id);
			}
			Dictionary<string, int> itemAuraCurseModifiers = GetItemAuraCurseModifiers();
			Dictionary<string, int> traitAuraCurseModifiers2 = GetTraitAuraCurseModifiers();
			Dictionary<string, int> aurasAuraCurseModifiers = GetAurasAuraCurseModifiers();
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			foreach (KeyValuePair<string, int> item in itemAuraCurseModifiers)
			{
				if (dictionary.ContainsKey(item.Key))
				{
					dictionary[item.Key] += item.Value;
				}
				else
				{
					dictionary.Add(item.Key, item.Value);
				}
			}
			foreach (KeyValuePair<string, int> item2 in traitAuraCurseModifiers2)
			{
				if (dictionary.ContainsKey(item2.Key))
				{
					dictionary[item2.Key] += item2.Value;
				}
				else
				{
					dictionary.Add(item2.Key, item2.Value);
				}
			}
			foreach (KeyValuePair<string, int> item3 in aurasAuraCurseModifiers)
			{
				if (dictionary.ContainsKey(item3.Key))
				{
					dictionary[item3.Key] += item3.Value;
				}
				else
				{
					dictionary.Add(item3.Key, item3.Value);
				}
			}
			foreach (KeyValuePair<string, int> item4 in auraCurseBonusDict)
			{
				if (dictionary.ContainsKey(item4.Key))
				{
					dictionary[item4.Key] += item4.Value;
				}
				else
				{
					dictionary.Add(item4.Key, item4.Value);
				}
			}
			if (dictionary.ContainsKey(id))
			{
				return dictionary[id];
			}
		}
		return 0;
	}

	public void SetHeroIndex(int index)
	{
		heroIndex = index;
	}

	public void SetNPCIndex(int index)
	{
		npcIndex = index;
	}

	public void SetEvent(Enums.EventActivation theEvent, Character target = null, int auxInt = 0, string auxString = "", Character caster = null)
	{
		if (MatchManager.Instance != null)
		{
			MatchManager.Instance.SetEventCo(this, theEvent, target, auxInt, auxString, caster);
			return;
		}
		if (isHero && theEvent != Enums.EventActivation.Killed)
		{
			ActivateTrait(theEvent, target, auxInt, auxString);
		}
		ActivateItem(theEvent, target, auxInt, auxString);
		FinishSetEvent(theEvent, target, auxInt, auxString);
	}

	public void FinishSetEvent(Enums.EventActivation theEvent, Character target = null, int auxInt = 0, string auxString = "")
	{
		switch (theEvent)
		{
		default:
			_ = 58;
			break;
		case Enums.EventActivation.BeginCombat:
			cardsPlayed.Clear();
			break;
		case Enums.EventActivation.BeginTurn:
			cardsPlayedRound.Clear();
			break;
		case Enums.EventActivation.CastCard:
			cardsPlayed.Add(MatchManager.Instance.CardActive);
			cardsPlayedRound.Add(MatchManager.Instance.CardActive);
			break;
		case Enums.EventActivation.Damage:
			if (target.IsHero)
			{
				break;
			}
			if (GetAuraCharges("runered") >= 3)
			{
				List<NPC> nPCSides = MatchManager.Instance.GetNPCSides(target.Position);
				target.SetAuraTrait(this, "mark", 1);
				{
					foreach (NPC item in nPCSides)
					{
						item.SetAuraTrait(this, "mark", 1);
					}
					break;
				}
			}
			if (GetAuraCharges("runered") >= 2)
			{
				target.SetAuraTrait(this, "mark", 1);
			}
			break;
		case Enums.EventActivation.Blocked:
			if (target.IsHero)
			{
				int num = target.GetAuraCurseQuantityModification("shield", Enums.CardClass.None);
				if (target == this)
				{
					num = 0;
				}
				if (target.GetAuraCharges("runeblue") >= 3)
				{
					Character[] teamHero = MatchManager.Instance.GetTeamHero();
					SetAuraCurseTeam(this, teamHero, "shield", 10, getModificationsManually: true, num);
				}
				else if (target.GetAuraCharges("runeblue") >= 2)
				{
					target.SetAuraTrait(this, "shield", 10 + num);
				}
			}
			break;
		case Enums.EventActivation.Heal:
			if (GetAuraCharges("runegreen") >= 3)
			{
				Character[] teamHero = MatchManager.Instance.GetTeamHero();
				SetAuraCurseTeam(this, teamHero, "vitality", 2);
			}
			else if (GetAuraCharges("runegreen") >= 2)
			{
				target.SetAuraTrait(this, "vitality", 2);
			}
			break;
		case Enums.EventActivation.FinishCast:
			MatchManager.Instance.RefreshStatusEffects();
			MatchManager.Instance.ItemTraitsActivatedSinceLastCardCast.Clear();
			break;
		case Enums.EventActivation.AuraCurseSet:
			if (subclassName == "bloodmage" && auxString == "bleed")
			{
				target.BleedRecievedByBloodMage += auxInt;
			}
			break;
		case Enums.EventActivation.BeginRound:
		case Enums.EventActivation.EndTurn:
		case Enums.EventActivation.EndRound:
		case Enums.EventActivation.DrawCard:
		case Enums.EventActivation.Hit:
		case Enums.EventActivation.Hitted:
		case Enums.EventActivation.Block:
		case Enums.EventActivation.Evade:
		case Enums.EventActivation.Evaded:
		case Enums.EventActivation.Healed:
		case Enums.EventActivation.BeginCombatEnd:
		case Enums.EventActivation.CharacterAssign:
		case Enums.EventActivation.Damaged:
		case Enums.EventActivation.PreBeginCombat:
		case Enums.EventActivation.BeginTurnCardsDealt:
		case Enums.EventActivation.CastFightCard:
			break;
		}
	}

	public void ActivateItem(Enums.EventActivation theEvent, Character target, int auxInt, string auxString, Enums.ActivationManual activationManual = Enums.ActivationManual.None, bool forceActivate = false)
	{
		if (theEvent == Enums.EventActivation.None && auxString != "" && forceActivate && MatchManager.Instance != null)
		{
			CardData cardData = Globals.Instance.GetCardData(auxString);
			if (cardData != null)
			{
				MatchManager.Instance.DoItem(this, theEvent, cardData, auxString, target, auxInt, auxString, 0, forceActivate);
			}
			ClearCacheItems();
		}
		else
		{
			if (forceActivate)
			{
				return;
			}
			if (theEvent == Enums.EventActivation.DestroyItem)
			{
				CardData cardData2 = Globals.Instance.GetCardData(auxString);
				if (cardData2 != null)
				{
					itemClass.DoItem(theEvent, cardData2, auxString, this, this, auxInt, auxString, 0, null);
				}
				ClearCacheItems();
			}
			else
			{
				if (MatchManager.Instance == null)
				{
					return;
				}
				int num = -1;
				for (int i = 0; i < itemSlots; i++)
				{
					string text = "";
					CardData cardData3 = null;
					switch (theEvent)
					{
					case Enums.EventActivation.BeginTurn:
						if (i == 0 && corruption != "")
						{
							text = corruption;
						}
						if (i == 1 && weapon != "")
						{
							text = weapon;
						}
						else if (i == 2 && armor != "")
						{
							text = armor;
						}
						else if (i == 3 && jewelry != "")
						{
							text = jewelry;
						}
						else if (i == 4 && accesory != "")
						{
							text = accesory;
						}
						else if (i == 5 && pet != "")
						{
							text = pet;
						}
						else if (i == 6 && enchantment != "")
						{
							text = enchantment;
						}
						else if (i == 7 && enchantment2 != "")
						{
							text = enchantment2;
						}
						else if (i == 8 && enchantment3 != "")
						{
							text = enchantment3;
						}
						break;
					default:
						if (i == 0 && weapon != "")
						{
							text = weapon;
						}
						else if (i == 1 && armor != "")
						{
							text = armor;
						}
						else if (i == 2 && jewelry != "")
						{
							text = jewelry;
						}
						else if (i == 3 && accesory != "")
						{
							text = accesory;
						}
						else if (i == 4 && corruption != "")
						{
							text = corruption;
						}
						else if (i == 5 && pet != "")
						{
							text = pet;
						}
						else if (i == 6 && enchantment != "")
						{
							text = enchantment;
						}
						else if (i == 7 && enchantment2 != "")
						{
							text = enchantment2;
						}
						else if (i == 8 && enchantment3 != "")
						{
							text = enchantment3;
						}
						break;
					case Enums.EventActivation.Killed:
						if (i == 0 && enchantment != "")
						{
							text = enchantment;
						}
						else if (i == 1 && enchantment2 != "")
						{
							text = enchantment2;
						}
						else if (i == 2 && enchantment3 != "")
						{
							text = enchantment3;
						}
						else if (i == 3 && weapon != "")
						{
							text = weapon;
						}
						else if (i == 4 && armor != "")
						{
							text = armor;
						}
						else if (i == 5 && jewelry != "")
						{
							text = jewelry;
						}
						else if (i == 6 && accesory != "")
						{
							text = accesory;
						}
						else if (i == 7 && corruption != "")
						{
							text = corruption;
						}
						else if (i == 8 && pet != "")
						{
							text = pet;
						}
						break;
					}
					if (!(text != ""))
					{
						continue;
					}
					cardData3 = Globals.Instance.GetCardData(text, instantiate: false);
					if (!(cardData3 != null))
					{
						continue;
					}
					ItemData itemData = new ItemData();
					if (!(cardData3.Item != null))
					{
						itemData = ((!(cardData3.ItemEnchantment != null)) ? null : cardData3.ItemEnchantment);
					}
					else
					{
						itemData = UnityEngine.Object.Instantiate(cardData3.Item);
						if (cardData3.CardType == Enums.CardType.Pet)
						{
							if (AtOManager.Instance.TeamHaveTrait("spiritlink"))
							{
								itemData.IncreaseAurasSelf = 5;
								HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_spiritlink"), Enums.CombatScrollEffectType.Trait);
							}
							if (AtOManager.Instance.CharacterHaveTrait(subclassName, "primalawakening"))
							{
								itemData.IncreaseAurasSelf = 10;
								HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_primalawakening"), Enums.CombatScrollEffectType.Trait);
							}
						}
					}
					if (!(itemData != null) || (itemData.PreventApplyForHeroTarget && target != null && target.isHero) || (itemData.ActivationOnlyOnHeroes && !isHero))
					{
						continue;
					}
					bool num2 = theEvent == Enums.EventActivation.CastFightCard;
					bool flag = itemData.Activation == Enums.EventActivation.Manual && activationManual == itemData.ActivationManual;
					if (!(num2 || flag) && itemData.Activation != theEvent && !DamagedWithDamagedSecondary(theEvent, itemData))
					{
						continue;
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("[Character/ActivateItem] Checking if " + text + " will activate", "item");
					}
					if (itemClass.DoItem(theEvent, cardData3, text, this, target, auxInt, auxString, 0, cardCasted, onlyCheckItemActivation: true))
					{
						num++;
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("[Character/ActivateItem] " + text + "-> OK", "item");
						}
						MatchManager.Instance.DoItem(this, theEvent, cardData3, text, target, auxInt, auxString, num);
					}
					else if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD(text + " -> XXXXXX", "item");
					}
				}
				if (theEvent == Enums.EventActivation.None || auxInt <= 0 || !TryGetActivateOnReceiveEnchantments(target, theEvent, out var enchantments))
				{
					return;
				}
				foreach (ItemData item in enchantments)
				{
					if ((!item.ActivationOnlyOnHeroes || isHero) && itemClass.DoItemOnReceive(theEvent, cardCasted, target, target, auxInt, auxString, 0, cardCasted, item))
					{
						num++;
						MatchManager.Instance.DoItem(target, theEvent, cardCasted, item.name, target, auxInt, auxString, num);
					}
				}
			}
		}
	}

	private static bool DamagedWithDamagedSecondary(Enums.EventActivation theEvent, ItemData itemData)
	{
		if (itemData.Activation == Enums.EventActivation.Damaged)
		{
			return theEvent == Enums.EventActivation.DamagedSecondary;
		}
		return false;
	}

	private bool TryGetActivateOnReceiveEnchantments(Character target, Enums.EventActivation activationEvent, out List<ItemData> enchantments)
	{
		if (target != null && target.alive && this != target)
		{
			enchantments = (from enchant in GameUtils.GetEnchantments(target)
				where enchant != null && enchant.ActivateOnReceive && enchant.Activation == activationEvent
				select enchant).ToList();
			return enchantments.Count > 0;
		}
		enchantments = null;
		return false;
	}

	public bool HasTrait(TraitEnum targetTrait)
	{
		bool result = false;
		if (!(this is Hero))
		{
			return false;
		}
		string[] array = Traits;
		for (int i = 0; i < array.Length; i++)
		{
			if (string.Equals(array[i], targetTrait.ToString(), StringComparison.InvariantCultureIgnoreCase))
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public void AssignEnchantmentManual(string _id, int _slot)
	{
		switch (_slot)
		{
		case 0:
			enchantment = _id;
			break;
		case 1:
			enchantment2 = _id;
			break;
		case 2:
			enchantment3 = _id;
			break;
		}
		ClearCacheItems();
	}

	public bool HasEnchantment(string id)
	{
		if (!enchantment.StartsWith(id, StringComparison.OrdinalIgnoreCase) && !enchantment2.StartsWith(id, StringComparison.OrdinalIgnoreCase))
		{
			return enchantment3.StartsWith(id, StringComparison.OrdinalIgnoreCase);
		}
		return true;
	}

	public bool HasThreeEnchantments()
	{
		if (!string.IsNullOrEmpty(enchantment) && !string.IsNullOrEmpty(enchantment2))
		{
			return !string.IsNullOrEmpty(enchantment3);
		}
		return false;
	}

	public void AssignEnchantment(string _id)
	{
		MatchManager.Instance.CleanEnchantmentExecutedTimes(id, _id);
		if (enchantment == _id)
		{
			enchantment = "";
		}
		else if (enchantment2 == _id)
		{
			enchantment2 = "";
		}
		else if (enchantment3 == _id)
		{
			enchantment3 = "";
		}
		ReorganizeEnchantments();
		if (enchantment == "")
		{
			enchantment = _id;
		}
		else if (enchantment2 == "")
		{
			enchantment2 = _id;
		}
		else if (enchantment3 == "")
		{
			enchantment3 = _id;
		}
		else if (isHero)
		{
			if (heroItem != null)
			{
				if (enchantment != "oblivionmnp")
				{
					MatchManager.Instance.RemovePetEnchantment(heroItem.gameObject, enchantment);
					MakeSpaceForNew(IsOblivion: true);
				}
				else
				{
					MatchManager.Instance.RemovePetEnchantment(heroItem.gameObject, enchantment2);
					MakeSpaceForNew(IsOblivion: false);
				}
			}
		}
		else if (npcItem != null)
		{
			MatchManager.Instance.RemovePetEnchantment(npcItem.gameObject, enchantment);
			MakeSpaceForNew(IsOblivion: false);
		}
		ClearCacheItems();
		UpdateAuraCurseFunctions();
		void MakeSpaceForNew(bool IsOblivion)
		{
			if (IsOblivion)
			{
				enchantment = "";
				ReorganizeEnchantments();
				enchantment3 = _id;
			}
			else
			{
				enchantment2 = "";
				ReorganizeEnchantments();
				enchantment3 = _id;
			}
		}
	}

	public void ReorganizeEnchantments()
	{
		if ((isHero && heroItem == null) || (!isHero && npcItem == null))
		{
			return;
		}
		string text = "";
		if (enchantment == "")
		{
			if (enchantment2 != "")
			{
				text = enchantment2;
				enchantment = text;
				enchantment2 = "";
			}
			else if (enchantment3 != "")
			{
				text = enchantment3;
				enchantment = text;
				enchantment3 = "";
			}
		}
		if (enchantment2 == "" && enchantment3 != "")
		{
			text = enchantment3;
			enchantment2 = text;
			enchantment3 = "";
		}
		if (isHero)
		{
			heroItem.ShowEnchantments();
		}
		else
		{
			npcItem.ShowEnchantments();
		}
		ClearCacheItems();
	}

	public void ShowPetsFromEnchantments()
	{
		if (enchantment == "" && enchantment2 == "" && enchantment3 == "")
		{
			return;
		}
		Hero hero = null;
		NPC npc = null;
		CharacterItem characterItem;
		if (isHero)
		{
			characterItem = heroItem;
			hero = heroItem.Hero;
		}
		else
		{
			characterItem = npcItem;
			npc = npcItem.NPC;
		}
		if (enchantment != "")
		{
			CardData cardData = Globals.Instance.GetCardData(enchantment);
			if (cardData != null && cardData.PetModel != null)
			{
				MatchManager.Instance.CreatePet(cardData, characterItem.gameObject, hero, npc, _fromEnchant: true, enchantment);
			}
		}
		if (enchantment2 != "")
		{
			CardData cardData = Globals.Instance.GetCardData(enchantment2);
			if (cardData != null && cardData.PetModel != null)
			{
				MatchManager.Instance.CreatePet(cardData, characterItem.gameObject, hero, npc, _fromEnchant: true, enchantment2);
			}
		}
		if (enchantment3 != "")
		{
			CardData cardData = Globals.Instance.GetCardData(enchantment3);
			if (cardData != null && cardData.PetModel != null)
			{
				MatchManager.Instance.CreatePet(cardData, characterItem.gameObject, hero, npc, _fromEnchant: true, enchantment3);
			}
		}
	}

	public void EnchantmentExecute(string enchantmentName)
	{
		if (heroItem != null)
		{
			if (enchantment == enchantmentName)
			{
				heroItem.EnchantmentExecute(0);
			}
			else if (enchantment2 == enchantmentName)
			{
				heroItem.EnchantmentExecute(1);
			}
			else if (enchantment3 == enchantmentName)
			{
				heroItem.EnchantmentExecute(2);
			}
		}
		else if (npcItem != null)
		{
			if (enchantment == enchantmentName)
			{
				npcItem.EnchantmentExecute(0);
			}
			else if (enchantment2 == enchantmentName)
			{
				npcItem.EnchantmentExecute(1);
			}
			else if (enchantment3 == enchantmentName)
			{
				npcItem.EnchantmentExecute(2);
			}
		}
		ClearCacheItems();
	}

	public void SetCastedCard(CardData _castedCard)
	{
		if (!(_castedCard == null) && !_castedCard.AutoplayDraw && !_castedCard.AutoplayEndTurn)
		{
			cardCasted = _castedCard;
		}
	}

	public void DoItem(Enums.EventActivation theEvent, CardData cardData, string item, Character target, int auxInt, string auxString, int order, CardData _castedCard, bool forceActivate = false)
	{
		string text = "item:" + item;
		if (_castedCard != null)
		{
			text += _castedCard.InternalId;
		}
		text += MatchManager.Instance.LogDictionary.Count;
		if ((bool)MatchManager.Instance)
		{
			switch (theEvent)
			{
			case Enums.EventActivation.CorruptionCombatStart:
				MatchManager.Instance.CreateLogEntry(_initial: true, text, item, null, null, null, null, MatchManager.Instance.GameRound(), Enums.EventActivation.CorruptionBeginRound);
				break;
			default:
				MatchManager.Instance.CreateLogEntry(_initial: true, text, item, null, null, null, null, MatchManager.Instance.GameRound(), Enums.EventActivation.ItemActivation);
				break;
			case Enums.EventActivation.PreBeginCombat:
			case Enums.EventActivation.AuraCurseSet:
				break;
			}
		}
		itemClass.DoItem(theEvent, cardData, item, this, target, auxInt, auxString, order, _castedCard, onlyCheckItemActivation: false, forceActivate);
		if ((bool)MatchManager.Instance)
		{
			switch (theEvent)
			{
			case Enums.EventActivation.CorruptionCombatStart:
				MatchManager.Instance.CreateLogEntry(_initial: false, text, item, null, null, null, null, MatchManager.Instance.GameRound(), Enums.EventActivation.CorruptionBeginRound);
				break;
			default:
				MatchManager.Instance.CreateLogEntry(_initial: false, text, item, null, null, null, null, MatchManager.Instance.GameRound(), Enums.EventActivation.ItemActivation);
				break;
			case Enums.EventActivation.PreBeginCombat:
			case Enums.EventActivation.AuraCurseSet:
				break;
			}
		}
	}

	public bool HaveTraitToActivate(Enums.EventActivation theEvent)
	{
		for (int i = 0; i < traits.Length; i++)
		{
			if (traits[i] != null && traits[i] != "")
			{
				TraitData traitData = Globals.Instance.GetTraitData(traits[i]);
				if (traitData != null && traitData.Activation == theEvent)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool ActivateTrait(Enums.EventActivation theEvent, Character target, int auxInt, string auxString)
	{
		bool result = false;
		if (traits == null)
		{
			return false;
		}
		for (int i = 0; i < traits.Length; i++)
		{
			if (traits[i] == null || !(traits[i] != ""))
			{
				continue;
			}
			TraitData traitData = Globals.Instance.GetTraitData(traits[i]);
			if (traitData != null)
			{
				if (traitData.ActivateOnRuneTypeAdded && RuneTypeAdded(theEvent, target, auxInt, auxString))
				{
					DoTrait(theEvent, traits[i], target, auxInt, auxString);
					result = true;
				}
				else if (traitData.Activation == theEvent || traitData.TryActivateOnEveryEvent)
				{
					DoTrait(theEvent, traits[i], target, auxInt, auxString);
					result = true;
				}
			}
		}
		return result;
	}

	private bool RuneTypeAdded(Enums.EventActivation theEvent, Character target, int auxInt, string auxString)
	{
		if (theEvent == Enums.EventActivation.AuraCurseSet && auxInt > 0)
		{
			switch (auxString)
			{
			case "runered":
			case "runeblue":
			case "runegreen":
				if (target.GetAuraCharges(auxString) == 0)
				{
					return true;
				}
				break;
			}
		}
		return false;
	}

	private void DoTrait(Enums.EventActivation theEvent, string trait, Character target, int auxInt, string auxString)
	{
		string text = "trait:" + trait;
		if (trait != "replenishing" && trait != "reaping" && trait != "runicempowerment" && trait != "glareofdoom")
		{
			if ((bool)MatchManager.Instance)
			{
				text = text + "_" + MatchManager.Instance.LogDictionary.Count;
			}
			if (theEvent != Enums.EventActivation.AuraCurseSet && (bool)MatchManager.Instance)
			{
				MatchManager.Instance.CreateLogEntry(_initial: true, text, trait, null, null, null, null, MatchManager.Instance.GameRound(), Enums.EventActivation.TraitActivation);
			}
		}
		traitClass.DoTrait(theEvent, trait, this, target, auxInt, auxString, cardCasted);
		if (trait != "replenishing" && trait != "reaping" && trait != "runicempowerment" && trait != "glareofdoom" && theEvent != Enums.EventActivation.AuraCurseSet && (bool)MatchManager.Instance)
		{
			MatchManager.Instance.CreateLogEntry(_initial: false, text, trait, null, null, null, null, MatchManager.Instance.GameRound(), Enums.EventActivation.TraitActivation);
		}
	}

	public void DoTraitFunction(string funcName)
	{
		traitClass.DoTrait(Enums.EventActivation.None, funcName, this, null, 0, "", null);
	}

	public bool HaveTrait(string traitId)
	{
		if (traits != null && traits.Length != 0)
		{
			traitId = traitId.ToLower();
			for (int i = 0; i < traits.Length; i++)
			{
				if (traits[i] != null && traits[i] == traitId)
				{
					return true;
				}
			}
		}
		return false;
	}

	public int GetHp()
	{
		return hpCurrent;
	}

	public int GetHpLeftForMax()
	{
		return GetMaxHP() - GetHp();
	}

	public float GetHpPercent()
	{
		return (float)GetHp() / (float)GetMaxHP() * 100f;
	}

	public void HealToMax()
	{
		hpCurrent = GetMaxHP();
	}

	public void ModifyHp(int _hp, bool _includeInStats = true, bool _refreshHP = true, Character casterCharacter = null)
	{
		int heroActive = MatchManager.Instance.GetHeroActive();
		int num = 0;
		int num2 = 0;
		int maxHP = GetMaxHP();
		if (AtOManager.Instance.combatStats == null)
		{
			AtOManager.Instance.InitCombatStats();
		}
		if (_hp < 0)
		{
			num = Mathf.Abs(_hp);
			if (num > hpCurrent)
			{
				num = hpCurrent;
			}
			if (_includeInStats && heroActive > -1)
			{
				AtOManager.Instance.combatStats[heroActive, 0] += num;
				AtOManager.Instance.combatStatsCurrent[heroActive, 0] += num;
			}
			if (isHero)
			{
				AtOManager.Instance.combatStats[heroIndex, 1] += num;
				AtOManager.Instance.combatStatsCurrent[heroIndex, 1] += num;
			}
			TryApplyMindCollapse(casterCharacter);
		}
		else if (_hp > 0)
		{
			num2 = _hp;
			if (num2 > maxHP - hpCurrent)
			{
				num2 = maxHP - hpCurrent;
			}
			if (_includeInStats && heroActive > -1)
			{
				AtOManager.Instance.combatStats[heroActive, 3] += num2;
				AtOManager.Instance.combatStatsCurrent[heroActive, 3] += num2;
			}
			if (isHero)
			{
				AtOManager.Instance.combatStats[heroIndex, 4] += num2;
				AtOManager.Instance.combatStatsCurrent[heroIndex, 4] += num2;
			}
		}
		hpCurrent += _hp;
		if (hpCurrent > maxHP)
		{
			hpCurrent = maxHP;
		}
		if (hpCurrent <= 0)
		{
			alive = false;
			hpCurrent = 0;
		}
		else
		{
			alive = true;
		}
		tryIncreaseEnergyForRevealingPresenceTrait();
		if (_hp < 0)
		{
			if (_hp < -15 && MatchManager.Instance.CardActive != null && MatchManager.Instance.CardActive.TargetType != Enums.CardTargetType.Global)
			{
				GameManager.Instance.cameraManager.Shake();
			}
			if (npcItem != null)
			{
				npcItem.CharacterHitted(fromHit: true);
			}
			else if (heroItem != null)
			{
				heroItem.CharacterHitted(fromHit: true);
			}
		}
		if (_refreshHP)
		{
			SetHP();
			MatchManager.Instance.BossNpc?.OnCharacterDamaged(this, _hp, hpCurrent);
		}
	}

	private bool tryIncreaseEnergyForRevealingPresenceTrait()
	{
		if (alive)
		{
			return false;
		}
		if (!isIllusion && !isSummon)
		{
			return false;
		}
		Hero[] teamHero = MatchManager.Instance.GetTeamHero();
		foreach (Hero hero in teamHero)
		{
			if (hero.alive && hero.HasTrait(TraitEnum.RevealingPresence))
			{
				hero.ModifyEnergy(1, showScrollCombatText: true);
				GameUtils.GetCharacterItem(hero).ScrollCombatText(Texts.Instance.GetText("traits_revealingpresence"), Enums.CombatScrollEffectType.Trait);
				return true;
			}
		}
		return false;
	}

	private bool TryApplyMindCollapse(Character casterCharacter)
	{
		if (HasTrait(TraitEnum.MindCollapse))
		{
			if (casterCharacter == null || casterCharacter.IsHero)
			{
				return false;
			}
			if (!alive || !casterCharacter.alive || !traitClass.CanUseTrait("mindcollapse"))
			{
				return false;
			}
			casterCharacter.SetAuraTrait(this, "sleep", 1);
			GameUtils.GetCharacterItem(casterCharacter).ScrollCombatText(Texts.Instance.GetText("traits_mindcollapse"), Enums.CombatScrollEffectType.Trait);
			EffectsManager.Instance.PlayEffectAC("shadowimpactdecay", isHero: false, GameUtils.GetCharacterItem(casterCharacter).CharImageT, flip: false);
			traitClass.IncrementTraitUsageCount("mindcollapse");
		}
		return true;
	}

	private Character GetAliveCharacterFromTeam(Character[] charTeam)
	{
		int num = UnityEngine.Random.Range(0, charTeam.Length);
		int num2 = num;
		while (charTeam[num] == null || !charTeam[num].Alive)
		{
			num++;
			num %= charTeam.Length;
			if (num2 == num)
			{
				return null;
			}
		}
		return charTeam[num];
	}

	public void SetAuraCurseTeam(Character caster, Character[] charTeam, string auracurse, int charges, bool getModificationsManually = false, int bonus = 0)
	{
		foreach (Character character in charTeam)
		{
			if (character != null && character.Alive)
			{
				character.SetAuraTrait(caster, auracurse, charges + bonus);
			}
		}
	}

	public bool HaveItem(string _itemId, int _itemSlot = -1, bool _checkRareToo = false)
	{
		for (int i = 0; i < itemSlots; i++)
		{
			if (_itemSlot > -1 && i != _itemSlot)
			{
				continue;
			}
			ItemData itemData = GetItemDataBySlot(i);
			if (itemData != null && itemData.Id == _itemId)
			{
				return true;
			}
			if (itemData != null && _checkRareToo)
			{
				CardData cardData = Globals.Instance.GetCardData(itemData.Id, instantiate: false);
				if (cardData != null && cardData.CardUpgraded == Enums.CardUpgraded.Rare && cardData.UpgradedFrom.ToLower() == _itemId)
				{
					return true;
				}
			}
		}
		return false;
	}

	public string GetItemToPassEventRoll()
	{
		for (int i = 0; i < itemSlots; i++)
		{
			ItemData itemData = GetItemDataBySlot(i);
			if (itemData != null && itemData.PassSingleAndCharacterRolls)
			{
				return itemData.Id;
			}
		}
		return "";
	}

	public bool HaveItemToActivate(Enums.EventActivation _theEvent, bool _checkForItems = false, bool _checkForCorruption = false)
	{
		for (int i = 0; i < itemSlots; i++)
		{
			if (!_checkForCorruption && heroData != null && i == 4)
			{
				continue;
			}
			ItemData itemData = GetItemDataBySlot(i);
			if (!(itemData != null) || itemData.Activation != _theEvent)
			{
				continue;
			}
			if (_theEvent == Enums.EventActivation.BeginTurnAboutToDealCards || _theEvent == Enums.EventActivation.BeginTurn)
			{
				if (itemData.ExactRound > 0 && MatchManager.Instance.GetCurrentRound() != itemData.ExactRound)
				{
					return false;
				}
				if (itemData.RoundCycle > 0 && MatchManager.Instance.GetCurrentRound() % itemData.RoundCycle != 0)
				{
					return false;
				}
				if (_checkForItems && itemData.CardNum == 0 && itemData.CardToGain == null)
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	public bool HaveCastCardItem()
	{
		for (int i = 0; i < itemSlots; i++)
		{
			if (!(heroData != null) || i != 4)
			{
				ItemData itemData = GetItemDataBySlot(i);
				if (itemData != null && itemData.CardPlace == Enums.CardPlace.Cast)
				{
					return true;
				}
			}
		}
		return false;
	}

	public int HaveAboutToDealCardsItemNum()
	{
		int num = 0;
		for (int i = 0; i < itemSlots; i++)
		{
			if (!(heroData != null) || i != 4)
			{
				ItemData itemData = GetItemDataBySlot(i);
				if (itemData != null && itemData.Activation == Enums.EventActivation.BeginTurnAboutToDealCards)
				{
					num++;
				}
			}
		}
		return num;
	}

	public int HaveCastCardItemNum()
	{
		int num = 0;
		for (int i = 0; i < itemSlots; i++)
		{
			if (!(heroData != null) || i != 4)
			{
				ItemData itemData = GetItemDataBySlot(i);
				if (itemData != null && itemData.CardPlace == Enums.CardPlace.Cast)
				{
					num++;
				}
			}
		}
		return num;
	}

	public bool HaveResurrectItem()
	{
		for (int i = 0; i < itemSlots; i++)
		{
			if (heroData != null && i == 4)
			{
				continue;
			}
			ItemData itemData = GetItemDataBySlot(i, useCache: false);
			if (itemData != null && itemData.HealPercentQuantity > 0 && itemData.Activation == Enums.EventActivation.Killed)
			{
				if (itemData.TimesPerCombat == 0)
				{
					return true;
				}
				if (itemData.TimesPerCombat > 0 && MatchManager.Instance.ItemExecutedInThisCombat(Id, itemData.Id) < itemData.TimesPerCombat)
				{
					return true;
				}
			}
		}
		if (ItemHasResurrect(enchantment))
		{
			return true;
		}
		if (ItemHasResurrect(enchantment2))
		{
			return true;
		}
		if (ItemHasResurrect(enchantment3))
		{
			return true;
		}
		return false;
	}

	private bool ItemHasResurrect(string itemName)
	{
		CardData cardData = new CardData();
		if (itemName != "")
		{
			cardData = Globals.Instance.GetCardData(itemName, instantiate: false);
			if (cardData != null)
			{
				ItemData itemData = null;
				if (cardData.Item != null)
				{
					itemData = cardData.Item;
				}
				else if (cardData.ItemEnchantment != null)
				{
					itemData = cardData.ItemEnchantment;
				}
				if (itemData != null && itemData.Activation == Enums.EventActivation.Killed)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void Resurrect(float percent)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("-- RESURRECT --", "trace");
		}
		alive = true;
		auraList = new List<Aura>();
		hpCurrent = Functions.FuncRoundToInt((float)GetMaxHP() * percent * 0.01f);
		UpdateAuraCurseFunctions();
	}

	public void KillCharacterFromOutside()
	{
		alive = false;
		hp = (hpCurrent = 0);
		if (npcItem != null)
		{
			npcItem.KillCharacterFromOutside();
		}
		else if (heroItem != null)
		{
			heroItem.KillCharacterFromOutside();
		}
	}

	public void SetHP()
	{
		if (npcItem != null)
		{
			npcItem.SetHP();
		}
		else if (heroItem != null)
		{
			heroItem.SetHP();
		}
	}

	public int GetMaxHP()
	{
		int num = hp + GetAuraStatModifiers(0, Enums.CharacterStat.Hp) + GetItemStatModifiers(Enums.CharacterStat.Hp);
		if (num <= 0)
		{
			num = 1;
		}
		if (hpCurrent > num)
		{
			hpCurrent = num;
		}
		return num;
	}

	public void ModifyMaxHP(int quantity)
	{
		hpCurrent += quantity;
		hp += quantity;
		if (hpCurrent <= 0)
		{
			hpCurrent = 1;
		}
		if (hp <= 0)
		{
			hp = 1;
		}
		if (hpCurrent > hp)
		{
			hpCurrent = hp;
		}
		AtOManager.Instance.SideBarRefresh();
	}

	public void DecreaseMaxHP(int quantity)
	{
		hp -= quantity;
		if (hp <= 0)
		{
			hp = 1;
		}
		if (hpCurrent > hp)
		{
			hpCurrent = hp;
		}
		AtOManager.Instance.SideBarRefresh();
	}

	public int CalculateRewardForCharacter(int _experience)
	{
		if (AtOManager.Instance.IsChallengeTraitActive("smartheroes"))
		{
			_experience += Functions.FuncRoundToInt((float)_experience * 0.5f);
		}
		if (GameManager.Instance.IsObeliskChallenge())
		{
			int num = 5;
			if (experience + _experience > Globals.Instance.GetExperienceByLevel(num))
			{
				_experience = Globals.Instance.GetExperienceByLevel(num) - experience;
			}
		}
		else
		{
			float num2 = 0.1f;
			if (level > AtOManager.Instance.GetTownTier() + 1)
			{
				return Functions.FuncRoundToInt((float)_experience * num2);
			}
			if (experience >= Globals.Instance.GetExperienceByLevel(level))
			{
				return Functions.FuncRoundToInt((float)_experience * num2);
			}
			if (experience + _experience > Globals.Instance.GetExperienceByLevel(level))
			{
				int num3 = _experience - (Globals.Instance.GetExperienceByLevel(level) - experience);
				return Globals.Instance.GetExperienceByLevel(level) - experience + Functions.FuncRoundToInt((float)num3 * num2);
			}
		}
		return _experience;
	}

	public void GrantExperience(int _experience)
	{
		experience += _experience;
		PlayerManager.Instance.ExpGainedSum(_experience);
		AtOManager.Instance.SideBarRefresh();
	}

	public bool IsReadyForLevelUp()
	{
		return experience >= Globals.Instance.GetExperienceByLevel(level);
	}

	public void LevelUp()
	{
		if (level < heroData.HeroSubClass.MaxHp.Length)
		{
			hpCurrent += heroData.HeroSubClass.MaxHp[level];
			hp += heroData.HeroSubClass.MaxHp[level];
		}
		level++;
	}

	public int ModifyBlock(int _dmg)
	{
		int num = GetBlock();
		int num2 = 0;
		if (AtOManager.Instance.combatStats == null)
		{
			AtOManager.Instance.InitCombatStats();
		}
		if (num > 0)
		{
			if (_dmg > num)
			{
				num2 = num;
				_dmg -= num;
				CheckBlockHP();
			}
			else
			{
				num2 = _dmg;
				_dmg = 0;
			}
			ConsumeEffectCharges("block", num2);
		}
		if (num > 0 && _dmg > 0)
		{
			SetEvent(Enums.EventActivation.BlockReachedZero);
		}
		if (num2 > 0 && isHero)
		{
			AtOManager.Instance.combatStats[heroIndex, 2] += num2;
			AtOManager.Instance.combatStatsCurrent[heroIndex, 2] += num2;
		}
		return _dmg;
	}

	private void RemoveBlock()
	{
		string text = "-" + Functions.UppercaseFirst(Texts.Instance.GetText("block"));
		if (heroItem != null)
		{
			heroItem.ScrollCombatText(text, Enums.CombatScrollEffectType.Block);
		}
		else if (npcItem != null)
		{
			npcItem.ScrollCombatText(text, Enums.CombatScrollEffectType.Block);
		}
	}

	public void SetBlock()
	{
		int num = GetBlock();
		if (num > 0 && num > block)
		{
			GameManager.Instance.PlayAudio(Globals.Instance.GetAuraCurseData("block").GetSound(), 0.5f);
		}
		block = num;
		CheckBlockHP();
	}

	public void CheckBlockHP()
	{
		bool state = GetBlock() > 0;
		if (heroItem != null)
		{
			heroItem.DrawBlock(state);
		}
		else if (npcItem != null)
		{
			npcItem.DrawBlock(state);
		}
	}

	public int GetBlock()
	{
		return GetAuraCharges("block");
	}

	public int GetEnergy()
	{
		return EnergyCurrent;
	}

	public int GetEnergyTurn()
	{
		int num = GetAuraStatModifiers(EnergyTurn, Enums.CharacterStat.Energy) + GetItemStatModifiers(Enums.CharacterStat.EnergyTurn);
		if (num < 0)
		{
			num = 0;
		}
		return num;
	}

	public void GainEnergyTurn()
	{
		int num = GetEnergyTurn();
		ModifyEnergy(num, showScrollCombatText: true);
		if (MatchManager.Instance.GetCurrentRound() == 2 && AtOManager.Instance.CharacterHavePerk(SubclassName, "mainperkenergy2b"))
		{
			ModifyEnergy(1, showScrollCombatText: true);
		}
		else if (MatchManager.Instance.GetCurrentRound() == 4 && AtOManager.Instance.CharacterHavePerk(SubclassName, "mainperkenergy2c"))
		{
			ModifyEnergy(2, showScrollCombatText: true);
		}
	}

	public void ModifyEnergy(int _energy, bool showScrollCombatText = false)
	{
		if (npcItem != null || heroItem == null)
		{
			return;
		}
		int num = EnergyCurrent;
		EnergyCurrent += _energy;
		if (EnergyCurrent > 10)
		{
			EnergyCurrent = 10;
		}
		else if (EnergyCurrent < 0)
		{
			EnergyCurrent = 0;
		}
		if (EnergyCurrent > num)
		{
			GameManager.Instance.PlayAudio(Globals.Instance.GetAuraCurseData("energize").GetSound(), 0.5f);
			MatchManager.Instance.ShowEnergyCounterParticles();
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (showScrollCombatText)
		{
			if (_energy > 0)
			{
				stringBuilder.Append("+");
				stringBuilder.Append(_energy);
			}
			else
			{
				stringBuilder.Append(_energy);
			}
		}
		if (heroItem != null)
		{
			if (showScrollCombatText)
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(Functions.UppercaseFirst(Texts.Instance.GetText("energy")));
				heroItem.ScrollCombatText(stringBuilder.ToString(), Enums.CombatScrollEffectType.Energy);
			}
			heroItem.DrawEnergy();
		}
		MatchManager.Instance.SetEnergyCounter(EnergyCurrent, _energy);
		MatchManager.Instance.RedrawCardsBorder();
	}

	public int GetDrawCardsTurnForDisplayInDeck()
	{
		int num = 5 + GetAuraDrawModifiers(writeVar: false);
		if (num > 10)
		{
			num = 10;
		}
		else if (num < 0)
		{
			num = 0;
		}
		return num;
	}

	public int GetDrawCardsTurn()
	{
		int num = 5 + drawModifier;
		if (num > 10)
		{
			num = 10;
		}
		else if (num < 0)
		{
			num = 0;
		}
		return num;
	}

	public List<string> GetAuraCurseByOrder(int type, int max = 1, bool onlyDispeleable = false, bool giveAll = false)
	{
		List<string> list = new List<string>();
		int num = 0;
		for (int i = 0; i < auraList.Count; i++)
		{
			if (auraList[i].ACData.Id == "invulnerableunremovable")
			{
				continue;
			}
			if ((type == 0 || type == 2) && auraList[i].ACData != null && auraList[i].ACData.IsAura && (auraList[i].ACData.Removable == onlyDispeleable || giveAll) && !list.Contains(auraList[i].ACData.Id))
			{
				list.Add(auraList[i].ACData.Id);
				num++;
				if (num >= max)
				{
					break;
				}
			}
			if ((type == 1 || type == 2) && auraList[i].ACData != null && !auraList[i].ACData.IsAura && (auraList[i].ACData.Removable == onlyDispeleable || giveAll) && !list.Contains(auraList[i].ACData.Id))
			{
				list.Add(auraList[i].ACData.Id);
				num++;
				if (num >= max)
				{
					break;
				}
			}
		}
		return list;
	}

	public virtual void BeginRound()
	{
		MatchManager.Instance.ConsumeAuraCurse("BeginRound", this);
		if (MatchManager.Instance.GetCurrentRound() == 1)
		{
			if (AtOManager.Instance.CharacterHavePerk(SubclassName, "mainperkreinforce0"))
			{
				SetAuraTrait(this, "reinforce", 1);
			}
			if (AtOManager.Instance.CharacterHavePerk(SubclassName, "mainperkinsulate0"))
			{
				SetAuraTrait(this, "insulate", 1);
			}
			if (AtOManager.Instance.CharacterHavePerk(SubclassName, "mainperkcourage0"))
			{
				SetAuraTrait(this, "courage", 1);
			}
			if (AtOManager.Instance.CharacterHavePerk(SubclassName, "mainperkinfuse0a"))
			{
				SetAuraTrait(this, "infuse", 1);
			}
		}
	}

	public virtual void EndRound()
	{
		int auraCharges = GetAuraCharges("fortify");
		SetEvent(Enums.EventActivation.EndRound, this);
		MatchManager.Instance.ConsumeAuraCurse("EndRound", this);
		if (isHero && AtOManager.Instance.TeamHavePerk("mainperkfortify1c") && auraCharges > 0)
		{
			SetAuraTrait(this, "block", auraCharges * 10);
		}
	}

	public virtual void BeginTurn()
	{
		RemoveEnchantsStartTurn();
		ReorganizeEnchantments();
		GainEnergyTurn();
		GetAuraDrawModifiers();
		perk_sparkCharges = GetAuraCharges("spark");
		MatchManager.Instance.ConsumeAuraCurse("BeginTurn", this);
	}

	public virtual void BeginTurnPerks()
	{
		if (!isHero && perk_sparkCharges > 0 && AtOManager.Instance.TeamHavePerk("mainperkspark2b"))
		{
			List<NPC> nPCSides = MatchManager.Instance.GetNPCSides(position);
			for (int i = 0; i < nPCSides.Count; i++)
			{
				nPCSides[i].SetAura(null, Globals.Instance.GetAuraCurseData("spark"), Functions.FuncRoundToInt((float)perk_sparkCharges * 0.3f));
			}
		}
		if (MatchManager.Instance.GetCurrentRound() == 1)
		{
			if (AtOManager.Instance.CharacterHavePerk(SubclassName, "mainperkfortify1b"))
			{
				SetAuraTrait(this, "fortify", 1);
			}
			if (AtOManager.Instance.CharacterHavePerk(SubclassName, "mainperkinspire0a"))
			{
				SetAuraTrait(this, "inspire", 1);
			}
		}
		MatchManager.Instance.SetWaitExecution(status: false);
	}

	public virtual void EndTurn()
	{
		if ((isHero && heroItem == null) || (!isHero && npcItem == null))
		{
			return;
		}
		int auraCharges = GetAuraCharges("chill");
		int auraCharges2 = GetAuraCharges("spark");
		int auraCharges3 = GetAuraCharges("crack");
		RemoveEnchantsEndTurn();
		ReorganizeEnchantments();
		if (AtOManager.Instance.CharacterHavePerk(SubclassName, "mainperkchill2d"))
		{
			int charges = Mathf.FloorToInt(1f / 14f * (float)auraCharges);
			SetAuraTrait(this, "reinforce", charges);
		}
		if (!isHero)
		{
			if (AtOManager.Instance.TeamHavePerk("mainperkspark2c"))
			{
				int charges2 = Mathf.FloorToInt(1f / 14f * (float)auraCharges2);
				SetAuraTrait(null, "slow", charges2);
			}
			if (AtOManager.Instance.TeamHavePerk("mainperkcrack2c"))
			{
				int charges3 = Mathf.FloorToInt(1f / 14f * (float)auraCharges3);
				SetAuraTrait(null, "vulnerable", charges3);
			}
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[CHARACTER ENDTURN] Alive -> " + alive);
		}
		MatchManager.Instance.ConsumeAuraCurse("EndTurn", this);
	}

	public int[] GetSpeed()
	{
		int num = Speed + GetItemStatModifiers(Enums.CharacterStat.Speed);
		int auraStatModifiers = GetAuraStatModifiers(num, Enums.CharacterStat.Speed);
		return new int[3]
		{
			auraStatModifiers,
			num,
			auraStatModifiers - num
		};
	}

	public void IndirectDamage(Enums.DamageType damageType, int damage, Character casterCharacter, AudioClip sound = null, string effect = "")
	{
		CastResolutionForCombatText castResolutionForCombatText = new CastResolutionForCombatText();
		if (heroItem != null)
		{
			new Dictionary<HeroItem, CastResolutionForCombatText>()[heroItem] = castResolutionForCombatText;
		}
		else if (npcItem != null)
		{
			new Dictionary<NPCItem, CastResolutionForCombatText>()[npcItem] = castResolutionForCombatText;
		}
		if (effect != "")
		{
			castResolutionForCombatText.effect = effect;
		}
		bool flag = false;
		int num = 0;
		float num2 = 0f;
		int num3 = 0;
		if (IsInvulnerable())
		{
			castResolutionForCombatText.invulnerable = true;
		}
		else if (damage > 0)
		{
			if (damageType != Enums.DamageType.None)
			{
				num = damage;
				damage = ModifyBlock(damage);
				num -= damage;
				castResolutionForCombatText.blocked = num;
				if (damage == 0)
				{
					castResolutionForCombatText.fullblocked = true;
				}
				if (IsImmune(damageType))
				{
					flag = true;
					num3 = 0;
					castResolutionForCombatText.immune = true;
				}
			}
			if (!flag)
			{
				if (damageType != Enums.DamageType.None)
				{
					num2 = -1 * BonusResists(damageType);
				}
				num3 = Functions.FuncRoundToInt((float)damage + (float)damage * num2 * 0.01f);
				num3 = MatchManager.Instance.IncreaseDamage(this, casterCharacter, num3);
			}
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		else
		{
			if (sound != null)
			{
				GameManager.Instance.PlayAudio(sound, 0.5f);
			}
			if (isHero)
			{
				Hero[] teamHero = MatchManager.Instance.GetTeamHero();
				foreach (Hero hero in teamHero)
				{
					if (hero != null && hero.Alive)
					{
						hero.SetEvent(Enums.EventActivation.DamagedDueToACConsumption, this, num3, effect);
					}
				}
			}
		}
		int num4 = Mathf.Abs(num3);
		if (num4 > hpCurrent)
		{
			num4 = hpCurrent;
		}
		if (effect != "")
		{
			ModifyHp(-num3, _includeInStats: false);
		}
		else
		{
			ModifyHp(-num3);
		}
		if (num4 > 0)
		{
			string text = casterCharacter.id.Trim().ToLower();
			if (effect.ToLower() == "spark")
			{
				MatchManager.Instance.DamageStatusFromCombatStats(text, effect, num4);
			}
			else if (effect.ToLower() == "thorns")
			{
				MatchManager.Instance.DamageStatusFromCombatStats(text, effect, num4);
			}
			else if (effect.ToLower() == "dark" && text != "")
			{
				MatchManager.Instance.DamageStatusFromCombatStats(text, effect, num4);
			}
			else
			{
				MatchManager.Instance.DamageStatusFromCombatStats(id, effect, num4);
			}
		}
		castResolutionForCombatText.damage += num3;
		castResolutionForCombatText.damageType = damageType;
		if (heroItem != null)
		{
			heroItem.ScrollCombatTextDamageNew(castResolutionForCombatText);
		}
		else if (npcItem != null)
		{
			npcItem.ScrollCombatTextDamageNew(castResolutionForCombatText);
		}
	}

	public void IndirectHeal(int heal, AudioClip sound = null, string effect = "", string sourceCharacterName = "")
	{
		CastResolutionForCombatText castResolutionForCombatText = new CastResolutionForCombatText();
		if (heroItem != null)
		{
			new Dictionary<HeroItem, CastResolutionForCombatText>()[heroItem] = castResolutionForCombatText;
		}
		else if (npcItem != null)
		{
			new Dictionary<NPCItem, CastResolutionForCombatText>()[npcItem] = castResolutionForCombatText;
		}
		if (effect != "")
		{
			castResolutionForCombatText.effect = effect;
		}
		heal = HealReceivedFinal(heal);
		if (GetHpLeftForMax() < heal)
		{
			heal = GetHpLeftForMax();
		}
		ModifyHp(heal, _includeInStats: false);
		if (effect.ToLower() == "regeneration")
		{
			MatchManager.Instance.DamageStatusFromCombatStats(id, effect, heal);
		}
		castResolutionForCombatText.heal = heal;
		if (sound != null)
		{
			GameManager.Instance.PlayAudio(sound, 0.5f);
		}
		if (sourceCharacterName == "")
		{
			sourceCharacterName = gameName;
		}
		_ = className;
		_ = effect != "";
		if (heroItem != null)
		{
			heroItem.ScrollCombatTextDamageNew(castResolutionForCombatText);
		}
		else if (npcItem != null)
		{
			npcItem.ScrollCombatTextDamageNew(castResolutionForCombatText);
		}
	}

	public void UpdateAuraCurseFunctions(AuraCurseData auraIncluded = null, int auraIncludedCharges = 0, int previousCharges = -1)
	{
		Functions.DebugLogGD("[UPDATEAURACURSEFUNCTIONS]", "trace");
		if (!Alive)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[UPDATEAURACURSEFUNCTIONS] Exit because !Alive");
			}
			return;
		}
		if (HpCurrent <= 0)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[UPDATEAURACURSEFUNCTIONS] Exit because HpCurrent <= 0");
			}
			return;
		}
		if (GetAuraCharges("rust") > 0)
		{
			for (int i = 0; i < auraList.Count; i++)
			{
				if (auraList[i].AuraCharges > 0)
				{
					switch (auraList[i].ACData.Id)
					{
					case "crack":
					case "poison":
					case "slow":
					case "fast":
					case "sharp":
					case "wet":
						auraList[i].ACData = AtOManager.Instance.GlobalAuraCurseModificationByTraitsAndItems("set", auraList[i].ACData.Id, this, this);
						break;
					}
				}
			}
		}
		foreach (Aura aura in auraList)
		{
			if (aura.ACData.Id == "powerful" || aura.ACData.Id == "fury")
			{
				aura.ACData = AtOManager.Instance.GlobalAuraCurseModificationByTraitsAndItems("set", aura.ACData.Id, this, this);
			}
		}
		for (int j = 0; j < auraList.Count; j++)
		{
			if (auraList[j].AuraCharges > 0)
			{
				string text = auraList[j].ACData.Id;
				switch (text)
				{
				case "courage":
				case "insulate":
				case "reinforce":
					auraList[j].ACData = AtOManager.Instance.GlobalAuraCurseModificationByTraitsAndItems("set", text, this, this);
					break;
				}
			}
		}
		if (heroItem != null)
		{
			heroItem.DrawEnergy();
			SetHP();
			SetBlock();
			SetTaunt();
			SetStealth();
			SetParalyze();
			SetOverDebuff();
			heroItem.SetDoomIcon();
			heroItem.DrawBuffs(auraIncluded, auraIncludedCharges, previousCharges);
			SetSpellswordSwords();
		}
		else if (npcItem != null)
		{
			npcItem.DrawEnergy();
			npcItem.NPC.RedrawRevealedCards();
			npcItem.SetDoomIcon();
			SetHP();
			SetBlock();
			SetTaunt();
			SetStealth();
			SetParalyze();
			npcItem.DrawBuffs(auraIncluded, auraIncludedCharges, previousCharges);
		}
	}

	public void SetAuraTrait(Character theCaster, string auracurse, int charges)
	{
		if (charges > 0)
		{
			SetAura(theCaster, Globals.Instance.GetAuraCurseData(auracurse), charges, fromTrait: true);
		}
	}

	public string GetAuraListString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		int count = AuraList.Count;
		for (int i = 0; i < count; i++)
		{
			if (AuraList[i] != null && AuraList[i].ACData != null)
			{
				stringBuilder.Append(AuraList[i].ACData.Id);
				stringBuilder.Append(AuraList[i].AuraCharges);
			}
		}
		return stringBuilder.ToString();
	}

	public int SetAura(Character theCaster, AuraCurseData _acData, int charges, bool fromTrait = false, Enums.CardClass CC = Enums.CardClass.None, bool useCharacterMods = true, bool canBePreventable = true)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("SetAura " + Id + " | " + _acData.Id + "(" + charges + ")", "trace");
		}
		if (charges == 0 || _acData == null)
		{
			return 0;
		}
		if (!Alive || HpCurrent <= 0)
		{
			return 0;
		}
		if (_acData.Id.ToLower() == "sight" && isHero)
		{
			return 0;
		}
		if (_acData.Id == "invulnerable" && GetAuraCharges("invulnerableunremovable") > 0)
		{
			return 0;
		}
		if (_acData.Id.ToLower() == "stanzai")
		{
			if (GetAuraCharges("stanzai") > 0 || GetAuraCharges("stanzaii") > 0 || GetAuraCharges("stanzaiii") > 0)
			{
				return 0;
			}
		}
		else if (_acData.Id.ToLower() == "stanzaii")
		{
			if (GetAuraCharges("stanzaii") > 0 || GetAuraCharges("stanzaiii") > 0)
			{
				return 0;
			}
		}
		else if (_acData.Id.ToLower() == "stanzaiii" && (GetAuraCharges("stanzaiii") > 0 || AtOManager.Instance.CharacterHavePerk(subclassName, "mainperkstanza0c")))
		{
			return 0;
		}
		if (_acData.Id.ToLower() == "block")
		{
			if (subclassName == "warden" && AtOManager.Instance.CharacterHaveTrait(subclassName, "queenofthorns"))
			{
				_acData = Globals.Instance.GetAuraCurseData("thorns");
				charges = Functions.FuncRoundToInt((float)charges * 0.3f);
			}
			if (!isHero && AtOManager.Instance.TeamHavePerk("mainperkcrack2b") && GetAuraCharges("crack") > 0)
			{
				charges -= Functions.FuncRoundToInt((float)GetAuraCharges("crack") * 0.5f);
			}
		}
		if (_acData.Id.ToLower() == "vitality" && isHero && AtOManager.Instance.CharacterHavePerk(subclassName, "mainperkvitality1b"))
		{
			HealCursesName(null, "bleed");
		}
		if (_acData.Id.ToLower() == "sight" && !isHero && AtOManager.Instance.TeamHavePerk("mainperksight1b"))
		{
			HealCursesName(null, "stealth");
		}
		if (_acData.Id.ToLower() == "spellsword" && isHero && GetAuraCharges("spellsword") == 4 && charges > 0)
		{
			if (AtOManager.Instance.CharacterHaveTrait(subclassName, "frostswords"))
			{
				MatchManager.Instance.GenerateNewCard(1, "frostdischarge", createCard: true, Enums.CardPlace.Hand);
			}
			if (AtOManager.Instance.CharacterHaveTrait(subclassName, "timeloop") || AtOManager.Instance.CharacterHaveTrait(subclassName, "timeparadox"))
			{
				int num = 1;
				if (AtOManager.Instance.CharacterHaveTrait(subclassName, "timeloop") && AtOManager.Instance.CharacterHaveTrait(subclassName, "timeparadox"))
				{
					num = 2;
				}
				if (!MatchManager.Instance.ItemExecuteForThisCombat(subclassName, "timeloop", num, ""))
				{
					SetAuraTrait(this, "inspire", 1);
					MatchManager.Instance.MoveCards(Enums.CardPlace.Vanish, Enums.CardPlace.RandomDeck, Enums.CardClass.Mage, 3);
					heroItem.ScrollCombatText(Texts.Instance.GetText("traits_Time Loop") + Functions.TextChargesLeft(MatchManager.Instance.ItemExecutedInThisCombat(subclassName, "timeloop"), num), Enums.CombatScrollEffectType.Trait);
				}
				MatchManager.Instance.SetTraitInfoText();
			}
		}
		AuraCurseData auraCurseData = AtOManager.Instance.GlobalAuraCurseModificationByTraitsAndItems("set", _acData.Id, theCaster, this);
		if (auraCurseData == null)
		{
			auraCurseData = Globals.Instance.GetAuraCurseData(_acData.Id);
		}
		if (auraCurseData == null)
		{
			return 0;
		}
		if (!auraCurseData.IsAura && IsInvulnerable() && auraCurseData.Id.ToLower() != "doom")
		{
			return 0;
		}
		int num2 = 0;
		if (theCaster != null && useCharacterMods)
		{
			charges += theCaster.GetAuraCurseQuantityModification(auraCurseData.Id, CC);
		}
		tryApplySlowIfHasSleepImmunity(theCaster, _acData, auraCurseData);
		string text = "";
		if ((auraCurseData.Preventable && canBePreventable) || auraCurseData.CanBeAddedToImmunityDespiteNotBeingPreventable)
		{
			if (AuracurseImmune.Contains(auraCurseData.Id) || AuraCurseImmuneByItems(auraCurseData.Id))
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("<s>");
				stringBuilder.Append(Functions.UppercaseFirst(auraCurseData.ACName));
				stringBuilder.Append("</s>");
				text = stringBuilder.ToString();
				Enums.CombatScrollEffectType type = ((!auraCurseData.IsAura) ? Enums.CombatScrollEffectType.Curse : Enums.CombatScrollEffectType.Aura);
				if (heroItem != null)
				{
					heroItem.ScrollCombatText(text, type);
				}
				else if (npcItem != null)
				{
					npcItem.ScrollCombatText(text, type);
				}
				return 0;
			}
			for (int num3 = auraList.Count - 1; num3 >= 0; num3--)
			{
				if (auraList[num3] != null && auraList[num3].ACData != null && !auraCurseData.IsAura && auraCurseData.Preventable && auraList[num3].ACData.CursePreventedPerStack > 0 && auraList[num3].GetCharges() > 0)
				{
					if (auraList[num3].ACData.Id == "buffer")
					{
						if (heroItem != null)
						{
							EffectsManager.Instance.PlayEffectAC("buffer", isHero: true, heroItem.CharImageT, flip: false);
						}
						else if (npcItem != null)
						{
							EffectsManager.Instance.PlayEffectAC("buffer", isHero: false, npcItem.CharImageT, flip: true);
						}
						ConsumeEffectCharges(auraList[num3].ACData.Id, 1);
					}
					else
					{
						ConsumeEffectCharges(auraList[num3].ACData.Id, 1);
					}
					StringBuilder stringBuilder2 = new StringBuilder();
					stringBuilder2.Append("<s>");
					stringBuilder2.Append(Functions.UppercaseFirst(auraCurseData.ACName));
					stringBuilder2.Append("</s>");
					text = stringBuilder2.ToString();
					Enums.CombatScrollEffectType type = ((!auraCurseData.IsAura) ? Enums.CombatScrollEffectType.Curse : Enums.CombatScrollEffectType.Aura);
					if (heroItem != null)
					{
						heroItem.ScrollCombatText(text, type);
					}
					else if (npcItem != null)
					{
						npcItem.ScrollCombatText(text, type);
					}
					return 0;
				}
			}
			for (int num4 = auraList.Count - 1; num4 >= 0; num4--)
			{
				if (auraList[num4] != null && auraList[num4].ACData != null && auraList[num4] != null && auraList[num4].ACData != null && auraList[num4].ACData.PreventedAuraCurse != null && auraList[num4].ACData.PreventedAuraCurse.Id == auraCurseData.Id && !isHero && auraList[num4].ACData.Id == "mark" && auraCurseData.Id == "stealth" && AtOManager.Instance.TeamHavePerk("mainperkmark1b"))
				{
					if (npcItem != null)
					{
						StringBuilder stringBuilder3 = new StringBuilder();
						stringBuilder3.Append("<s>");
						stringBuilder3.Append(Functions.UppercaseFirst(auraCurseData.ACName));
						stringBuilder3.Append("</s>");
						text = stringBuilder3.ToString();
						Enums.CombatScrollEffectType type = Enums.CombatScrollEffectType.Aura;
						npcItem.ScrollCombatText(text, type);
					}
					return 0;
				}
			}
			for (int num5 = auraList.Count - 1; num5 >= 0; num5--)
			{
				if (auraList[num5] != null && auraList[num5].ACData != null && auraList[num5] != null && auraList[num5].ACData != null && auraList[num5].ACData.PreventedAuraCurse != null && auraList[num5].ACData.PreventedAuraCurse.Id == auraCurseData.Id)
				{
					int num6 = Functions.FuncRoundToInt(auraList[num5].GetCharges() * auraList[num5].ACData.PreventedAuraCurseStackPerStack);
					if (num6 >= charges)
					{
						ConsumeEffectCharges(auraList[num5].ACData.Id, Functions.FuncRoundToInt(charges / auraList[num5].ACData.PreventedAuraCurseStackPerStack));
						StringBuilder stringBuilder4 = new StringBuilder();
						stringBuilder4.Append("<s>");
						stringBuilder4.Append(Functions.UppercaseFirst(auraCurseData.ACName));
						stringBuilder4.Append("</s>");
						text = stringBuilder4.ToString();
						Enums.CombatScrollEffectType type = ((!auraCurseData.IsAura) ? Enums.CombatScrollEffectType.Curse : Enums.CombatScrollEffectType.Aura);
						if (heroItem != null)
						{
							heroItem.ScrollCombatText(text, type);
						}
						else if (npcItem != null)
						{
							npcItem.ScrollCombatText(text, type);
						}
						return 0;
					}
					SetEvent(Enums.EventActivation.AuraCurseRemoved, this, 0, auraList[num5].ACData.Id);
					auraList.RemoveAt(num5);
					UpdateAuraCurseFunctions();
					charges -= num6;
				}
			}
		}
		if (auraCurseData.RemoveAuraCurse != null || auraCurseData.RemoveAuraCurse2 != null)
		{
			int num7 = 0;
			for (int num8 = auraList.Count - 1; num8 >= 0; num8--)
			{
				if (auraList[num8] != null && auraList[num8].ACData != null && ((auraCurseData.RemoveAuraCurse != null && auraList[num8].ACData.Id == auraCurseData.RemoveAuraCurse.Id) || (auraCurseData.RemoveAuraCurse2 != null && auraList[num8].ACData.Id == auraCurseData.RemoveAuraCurse2.Id)))
				{
					StringBuilder stringBuilder5 = new StringBuilder();
					stringBuilder5.Append("<s>");
					stringBuilder5.Append(Functions.UppercaseFirst(auraList[num8].ACData.ACName));
					stringBuilder5.Append("</s>");
					string text2 = stringBuilder5.ToString();
					Enums.CombatScrollEffectType type2 = ((!auraList[num8].ACData.IsAura) ? Enums.CombatScrollEffectType.Curse : Enums.CombatScrollEffectType.Aura);
					if (heroItem != null)
					{
						heroItem.ScrollCombatText(text2, type2);
					}
					else if (npcItem != null)
					{
						npcItem.ScrollCombatText(text2, type2);
					}
					if (!AuraList[num8].ACData.IsAura)
					{
						num7++;
					}
					SetEvent(Enums.EventActivation.AuraCurseRemoved, this, 0, auraList[num8].ACData.Id);
					auraList.RemoveAt(num8);
				}
			}
			if (num7 > 0)
			{
				SetEvent(Enums.EventActivation.CurseRemoved, this, num7);
			}
		}
		theCaster?.SetEvent(Enums.EventActivation.AuraCurseSet, this, charges, auraCurseData.Id);
		if (auraCurseData.ExplodeAtStacks > 0 && charges >= auraCurseData.ExplodeAtStacks)
		{
			bool flag = false;
			for (int i = 0; i < auraList.Count; i++)
			{
				if (auraList[i].ACData.Id == auraCurseData.Id)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Aura aura = new Aura();
				aura.SetAura(auraCurseData, 0);
				auraList.Add(aura);
			}
		}
		int num9 = -1;
		AtOManager.Instance.GetNgPlus();
		if ((bool)MatchManager.Instance && auraCurseData != null && theCaster != null)
		{
			MatchManager.Instance.StoreStatusForCombatStats(id, auraCurseData.Id, theCaster.Id, charges);
		}
		for (int j = 0; j < auraList.Count; j++)
		{
			if (auraList[j] == null || !(auraList[j].ACData != null) || !(auraList[j].ACData.Id == auraCurseData.Id))
			{
				continue;
			}
			int num10 = 0;
			int charges2 = auraList[j].GetCharges();
			num9 = auraCurseData.GetMaxCharges();
			if (num9 > 0 && num9 < charges2)
			{
				num9 = charges2;
			}
			num2 = charges;
			if (auraCurseData.GainCharges)
			{
				if (num9 > -1 && num2 + charges2 > num9)
				{
					num2 = num9 - charges2;
				}
				auraList[j].AddCharges(num2);
			}
			else if (charges2 < charges)
			{
				num2 = charges - charges2;
				if (num9 > -1 && num2 + charges2 > num9)
				{
					num2 = num9 - charges2;
				}
				auraList[j].AddCharges(num2);
			}
			else
			{
				num2 = 0;
			}
			if (auraCurseData.Id == "vitality")
			{
				if (isHero && (MadnessManager.Instance.IsMadnessTraitActive("decadence") || AtOManager.Instance.IsChallengeTraitActive("decadence")))
				{
					hpCurrent += Functions.FuncRoundToInt(auraCurseData.CharacterStatModifiedValuePerStack * (float)charges * 0.5f);
				}
				else
				{
					hpCurrent += Functions.FuncRoundToInt(auraCurseData.CharacterStatModifiedValuePerStack * (float)charges);
				}
			}
			charges2 = auraList[j].GetCharges();
			if (auraCurseData.RevealCardsPerCharge > 0 && npcItem != null)
			{
				npcItem.NPC.CheckRevealCardsCurse();
			}
			if (auraCurseData.ExplodeAtStacks > 0 && auraCurseData.ExplodeAtStacks <= charges2)
			{
				if (!Alive || HpCurrent <= 0)
				{
					return 0;
				}
				if (auraCurseData.EffectTick != "")
				{
					if (npcItem != null)
					{
						EffectsManager.Instance.PlayEffectAC(auraCurseData.EffectTick, isHero: false, npcItem.CharImageT, flip: false);
					}
					else if (heroItem != null)
					{
						EffectsManager.Instance.PlayEffectAC(auraCurseData.EffectTick, isHero: true, heroItem.CharImageT, flip: false);
					}
				}
				if (auraCurseData.DamageSidesWhenConsumed > 0 || auraCurseData.DamageSidesWhenConsumedPerCharge > 0)
				{
					num10 = 0;
					num10 += auraCurseData.DamageSidesWhenConsumed;
					num10 += auraCurseData.DamageSidesWhenConsumedPerCharge * charges2;
					if (heroItem != null)
					{
						List<Hero> heroSides = MatchManager.Instance.GetHeroSides(position);
						for (int k = 0; k < heroSides.Count; k++)
						{
							if (auraCurseData.EffectTickSides != "" && heroSides[k].HeroItem != null)
							{
								EffectsManager.Instance.PlayEffectAC(auraCurseData.EffectTickSides, isHero: true, heroSides[k].HeroItem.CharImageT, flip: false, 0.2f);
							}
							heroSides[k].IndirectDamage(auraCurseData.DamageTypeWhenConsumed, num10, theCaster, null, auraCurseData.Id);
						}
					}
					else if (npcItem != null)
					{
						List<NPC> nPCSides = MatchManager.Instance.GetNPCSides(position);
						for (int l = 0; l < nPCSides.Count; l++)
						{
							if (auraCurseData.EffectTickSides != "" && nPCSides[l].NPCItem != null)
							{
								EffectsManager.Instance.PlayEffectAC(auraCurseData.EffectTickSides, isHero: false, nPCSides[l].NPCItem.CharImageT, flip: false, 0.2f);
							}
							nPCSides[l].IndirectDamage(auraCurseData.DamageTypeWhenConsumed, num10, theCaster, null, auraCurseData.Id);
						}
					}
				}
				if (auraCurseData.DamageWhenConsumed > 0 || auraCurseData.DamageWhenConsumedPerCharge > 0f)
				{
					num10 = 0;
					num10 += auraCurseData.DamageWhenConsumed;
					int num11 = charges2;
					if (auraCurseData.Id == "dark")
					{
						SetEvent(Enums.EventActivation.CurseExploded, this, num11, auraCurseData.Id, theCaster);
					}
					if (auraCurseData.ConsumedDamageChargesBasedOnACCharges != null)
					{
						num11 = GetAuraCharges(auraCurseData.ConsumedDamageChargesBasedOnACCharges.Id);
					}
					if (auraCurseData.ConsumeDamageChargesIfACApplied != null && GetAuraCharges(auraCurseData.ConsumeDamageChargesIfACApplied.Id) <= 0)
					{
						num11 = 0;
					}
					num10 += Functions.FuncRoundToInt(auraCurseData.DamageWhenConsumedPerCharge * (float)num11);
					if (!IsHero && _acData.Id == "scourge")
					{
						if (AtOManager.Instance.TeamHavePerk("mainperkscourge0b"))
						{
							num10 += GetAuraCharges("burn");
						}
						if (AtOManager.Instance.TeamHavePerk("mainperkscourge0c"))
						{
							num10 += GetAuraCharges("insane");
						}
					}
					if (auraCurseData.Id == "dark")
					{
						int auraCharges = GetAuraCharges("scourge");
						if (auraCharges > 0)
						{
							num10 += Functions.FuncRoundToInt((float)num10 * 0.1f * (float)auraCharges);
						}
					}
					IndirectDamage(auraCurseData.DamageTypeWhenConsumed, num10, theCaster, null, auraCurseData.Id);
				}
				if (auraCurseData.HealTargetOnExplode != Enums.AuraCurseExplodeHealTarget.None)
				{
					int healTotalOnExplode = auraCurseData.HealTotalOnExplode;
					healTotalOnExplode += Functions.FuncRoundToInt(auraCurseData.HealPerChargeOnExplode * (float)charges2);
					Character[] charactersToHeal = GetCharactersToHeal(theCaster, auraCurseData.HealTargetOnExplode);
					if (charactersToHeal != null)
					{
						if (theCaster != null && theCaster.alive && theCaster.isHero)
						{
							AtOManager.Instance.combatStats[theCaster.heroIndex, 3] += healTotalOnExplode;
						}
						HealCharacters(charactersToHeal, healTotalOnExplode, auraCurseData.GetSound(), auraCurseData.Id);
					}
					else if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("Healing Target (on explode) is null.", "error");
					}
				}
				if (auraCurseData.ACOnExplode != null || auraCurseData.Id == "leech")
				{
					int charges3 = auraCurseData.ACChargesPerStackChargeOnExplode * charges2;
					if (auraCurseData.Id == "leech")
					{
						AtOManager.Instance.DoLeachExplosion(this);
					}
					if (!IsHero)
					{
						if (AtOManager.Instance.AliveTeamHaveTrait("voracious") && auraCurseData.Id == "leech")
						{
							Character[] teamHero = MatchManager.Instance.GetTeamHero();
							SetAuraCurseTeam(theCaster, teamHero, "regeneration", 1);
						}
						if (AtOManager.Instance.AliveTeamHaveTrait("energyspike") && auraCurseData.Id == "leech")
						{
							Character[] teamHero = MatchManager.Instance.GetTeamHero();
							SetAuraCurseTeam(theCaster, teamHero, "energize", 1);
							Hero[] teamHero2 = MatchManager.Instance.GetTeamHero();
							foreach (Hero hero in teamHero2)
							{
								if (hero != null && hero != null && hero.alive)
								{
									EffectsManager.Instance.PlayEffectAC("mageself2", isHero: true, hero.HeroItem.CharImageT, flip: false);
									if (hero.Traits.Contains("energyspike"))
									{
										EffectsManager.Instance.PlayEffectAC("energyblue", isHero: true, hero.HeroItem?.CharImageT, flip: false, 0.4f);
									}
								}
							}
							GameManager.Instance.PlayLibraryAudio("energy");
						}
						if (AtOManager.Instance.AliveTeamHaveTrait("hemostasis") && auraCurseData.Id == "leech")
						{
							Character[] teamHero = MatchManager.Instance.GetTeamHero();
							SetAuraCurseTeam(theCaster, teamHero, "vitality", 1);
							teamHero = MatchManager.Instance.GetTeamHero();
							SetAuraCurseTeam(theCaster, teamHero, "inspire", 1);
							Hero[] teamHero2 = MatchManager.Instance.GetTeamHero();
							foreach (Hero hero2 in teamHero2)
							{
								if (hero2 != null && hero2 != null && hero2.alive)
								{
									EffectsManager.Instance.PlayEffectAC("songsere1", isHero: true, hero2.HeroItem.CharImageT, flip: false, 0.5f);
									if (hero2.Traits.Contains("hemostasis"))
									{
										EffectsManager.Instance.PlayEffectAC("songself3", isHero: true, hero2.HeroItem.CharImageT, flip: false);
									}
								}
							}
							GameManager.Instance.PlayLibraryAudio("heal");
						}
					}
					if (auraCurseData.ACOnExplode != null)
					{
						SetAura(this, Globals.Instance.GetAuraCurseData(auraCurseData.ACOnExplode.Id), charges3);
					}
				}
				ConsumeEffectCharges(auraCurseData.Id);
				return 3;
			}
			if (num2 > 0)
			{
				UpdateAuraCurseFunctions(auraList[j].ACData, num2, charges2);
			}
			else
			{
				UpdateAuraCurseFunctions();
			}
			if (auraCurseData.CombatlogShow)
			{
				StringBuilder stringBuilder6 = new StringBuilder();
				stringBuilder6.Append("<sprite name=");
				stringBuilder6.Append(auraCurseData.Id);
				stringBuilder6.Append(">");
				stringBuilder6.Append(Functions.UppercaseFirst(auraCurseData.ACName));
				text = stringBuilder6.ToString();
				Enums.CombatScrollEffectType type = ((!auraCurseData.IsAura) ? Enums.CombatScrollEffectType.Curse : Enums.CombatScrollEffectType.Aura);
				if (!fromTrait)
				{
					if (heroItem != null)
					{
						heroItem.ScrollCombatText(text, type);
					}
					else if (npcItem != null)
					{
						npcItem.ScrollCombatText(text, type);
					}
				}
			}
			return 2;
		}
		if (auraCurseData.ConsumedAtCast && auraCurseData.ConsumeAll)
		{
			UpdateAuraCurseFunctions();
			return 3;
		}
		Aura aura2 = new Aura();
		num2 = charges;
		num9 = auraCurseData.GetMaxCharges();
		if (num9 > -1 && num2 > num9)
		{
			num2 = num9;
		}
		aura2.SetAura(auraCurseData, num2);
		auraList.Add(aura2);
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("added aura " + auraCurseData.Id + " charges " + num2, "trace");
		}
		if (auraList.Count > 0)
		{
			List<int> list = new List<int>();
			int num12 = -10;
			int num13 = -1;
			for (int n = 0; n < auraList.Count; n++)
			{
				for (int num14 = 0; num14 < auraList.Count; num14++)
				{
					if (auraList[num14] != null && !list.Contains(num14) && auraList[num14].ACData != null && auraList[num14].ACData.PriorityOnConsumption > num12)
					{
						num13 = num14;
						num12 = auraList[num14].ACData.PriorityOnConsumption;
					}
				}
				num12 = -10;
				if (num13 > -1)
				{
					list.Add(num13);
				}
			}
			List<Aura> list2 = new List<Aura>();
			for (int num15 = 0; num15 < list.Count; num15++)
			{
				list2.Add(auraList[list[num15]]);
			}
			auraList = list2;
		}
		if (auraCurseData.Id == "vitality")
		{
			if (isHero && (MadnessManager.Instance.IsMadnessTraitActive("decadence") || AtOManager.Instance.IsChallengeTraitActive("decadence")))
			{
				hpCurrent += Functions.FuncRoundToInt(auraCurseData.CharacterStatModifiedValuePerStack * (float)charges * 0.5f);
			}
			else
			{
				hpCurrent += Functions.FuncRoundToInt(auraCurseData.CharacterStatModifiedValuePerStack * (float)charges);
			}
		}
		if (auraCurseData.RevealCardsPerCharge > 0)
		{
			if (npcItem == null)
			{
				return 0;
			}
			npcItem.NPC.CheckRevealCardsCurse();
		}
		UpdateAuraCurseFunctions(auraCurseData, num2);
		if (auraCurseData.CombatlogShow && !fromTrait)
		{
			StringBuilder stringBuilder7 = new StringBuilder();
			stringBuilder7.Append("<sprite name=");
			stringBuilder7.Append(auraCurseData.Id);
			stringBuilder7.Append(">");
			stringBuilder7.Append(Functions.UppercaseFirst(auraCurseData.ACName));
			text = stringBuilder7.ToString();
			Enums.CombatScrollEffectType type = ((!auraCurseData.IsAura) ? Enums.CombatScrollEffectType.Curse : Enums.CombatScrollEffectType.Aura);
			if (heroItem != null)
			{
				heroItem.ScrollCombatText(text, type);
			}
			else if (npcItem != null)
			{
				npcItem.ScrollCombatText(text, type);
			}
		}
		return 1;
	}

	private void tryApplySlowIfHasSleepImmunity(Character theCaster, AuraCurseData _acData, AuraCurseData acData)
	{
		if (_acData.Id.Equals("sleep", StringComparison.OrdinalIgnoreCase) && AuracurseImmune.Contains(acData.Id))
		{
			SetAura(theCaster, Globals.Instance.GetAuraCurseData("slow"), 1);
		}
	}

	private void HealTeam(Character[] team, int healAmount, AudioClip sound = null, string effect = "")
	{
		foreach (Character character in team)
		{
			if (character != null && character.Alive)
			{
				if (character.HaveTrait("voracious"))
				{
					healAmount = (int)((float)healAmount * 1.1f);
				}
				character.IndirectHeal(healAmount, sound, effect, gameName);
			}
		}
	}

	private Character[] GetCasterTeam()
	{
		if (heroItem != null)
		{
			return MatchManager.Instance.GetTeamNPC();
		}
		if (npcItem != null)
		{
			return MatchManager.Instance.GetTeamHero();
		}
		return null;
	}

	private Character[] GetOwnerTeam()
	{
		if (heroItem == null)
		{
			return MatchManager.Instance.GetTeamNPC();
		}
		if (npcItem == null)
		{
			return MatchManager.Instance.GetTeamHero();
		}
		return null;
	}

	private void DamageRandomMonster(int damageAmount, AudioClip sound = null, string effect = "")
	{
		NPC[] teamNPC = MatchManager.Instance.GetTeamNPC();
		int num = 0;
		do
		{
			num = Functions.Random(0, teamNPC.Length, Functions.RandomString(5f));
		}
		while (teamNPC[num] == null || !teamNPC[num].Alive);
		teamNPC[num].IndirectDamage(Enums.DamageType.None, damageAmount, this, sound, effect);
	}

	private Character[] GetCharactersToHeal(Character theCaster, Enums.AuraCurseExplodeHealTarget healTarget)
	{
		return healTarget switch
		{
			Enums.AuraCurseExplodeHealTarget.Caster => new Character[1] { theCaster }, 
			Enums.AuraCurseExplodeHealTarget.CasterTeam => GetCasterTeam(), 
			Enums.AuraCurseExplodeHealTarget.Target => new Character[1] { this }, 
			Enums.AuraCurseExplodeHealTarget.TargetTeam => GetOwnerTeam(), 
			_ => null, 
		};
	}

	private void HealCharacters(Character[] team, int healAmount, AudioClip sound = null, string effect = "", bool skipCaster = false)
	{
		if (AtOManager.Instance.AliveTeamHaveTrait("voracious") && effect == "leech")
		{
			healAmount = (int)((float)healAmount * 1.5f);
		}
		foreach (Character character in team)
		{
			if (character != null && character.Alive && (!skipCaster || character != this))
			{
				character.IndirectHeal(healAmount, sound, effect, gameName);
			}
		}
	}

	public void RemoveAuraCurses()
	{
		auraList.Clear();
	}

	public List<string> CharacterImmunitiesList()
	{
		List<string> list = new List<string>();
		if (AuracurseImmune.Count > 0)
		{
			for (int i = 0; i < AuracurseImmune.Count; i++)
			{
				list.Add(AuracurseImmune[i]);
			}
		}
		List<string> list2 = AuraCurseImmunitiesByItemsList();
		for (int j = 0; j < list2.Count; j++)
		{
			if (!list.Contains(list2[j]))
			{
				list.Add(list2[j]);
			}
		}
		return list;
	}

	public List<string> AuraCurseImmunitiesByItemsList()
	{
		List<string> list = new List<string>();
		for (int i = 0; i < itemSlots; i++)
		{
			if (heroData != null && i == 4)
			{
				continue;
			}
			ItemData itemData = GetItemDataBySlot(i);
			if (itemData != null)
			{
				if (itemData.AuracurseImmune1 != null && !list.Contains(itemData.AuracurseImmune1.Id))
				{
					list.Add(itemData.AuracurseImmune1.Id);
				}
				if (itemData.AuracurseImmune2 != null && !list.Contains(itemData.AuracurseImmune2.Id))
				{
					list.Add(itemData.AuracurseImmune2.Id);
				}
			}
		}
		if (isHero && list.Contains("bleed") && AtOManager.Instance.CharacterHavePerk(subclassName, "mainperkfury1c"))
		{
			list.Remove("bleed");
		}
		return list;
	}

	public bool AuraCurseImmuneByItems(string acName)
	{
		if (acName == "bleed" && AtOManager.Instance.CharacterHavePerk(subclassName, "mainperkfury1c"))
		{
			return false;
		}
		if ((bool)MatchManager.Instance && useCache && cacheAuraCurseImmuneByItems.ContainsKey(acName))
		{
			return cacheAuraCurseImmuneByItems[acName];
		}
		for (int i = 0; i < itemSlots; i++)
		{
			if (heroData != null && i == 4)
			{
				continue;
			}
			ItemData itemData = GetItemDataBySlot(i);
			if (itemData != null)
			{
				if (itemData.AuracurseImmune1 != null && itemData.AuracurseImmune1.Id == acName)
				{
					cacheAuraCurseImmuneByItems.Add(acName, value: true);
					return true;
				}
				if (itemData.AuracurseImmune2 != null && itemData.AuracurseImmune2.Id == acName)
				{
					cacheAuraCurseImmuneByItems.Add(acName, value: true);
					return true;
				}
			}
		}
		if ((bool)MatchManager.Instance && useCache)
		{
			cacheAuraCurseImmuneByItems.Add(acName, value: false);
		}
		return false;
	}

	public bool HasEffect(string effect)
	{
		effect = effect.ToLower();
		for (int i = 0; i < auraList.Count; i++)
		{
			if (i < auraList.Count && auraList[i] != null && auraList[i].ACData != null)
			{
				if (auraList[i].ACData.Id == effect)
				{
					return true;
				}
				if (effect == "invulnerable" && auraList[i].ACData.Id == "invulnerableunremovable")
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool HasAnyAura()
	{
		if (auraList != null)
		{
			for (int i = 0; i < auraList.Count; i++)
			{
				if (auraList[i].ACData.IsAura)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool HasAnyCurse()
	{
		if (auraList != null)
		{
			return auraList.Any((Aura aura) => !aura.ACData.IsAura);
		}
		return false;
	}

	public bool HasAuraCurse(string auraCurseId)
	{
		if (auraList != null)
		{
			return auraList.Any((Aura aura) => aura.ACData.Id.Equals(auraCurseId, StringComparison.OrdinalIgnoreCase));
		}
		return false;
	}

	public bool TryRemoveAuraCurse(string auraCurseId)
	{
		if (auraList == null || string.IsNullOrWhiteSpace(auraCurseId))
		{
			return false;
		}
		Aura aura = auraList.FirstOrDefault((Aura a) => a.ACData.Id.Equals(auraCurseId, StringComparison.OrdinalIgnoreCase));
		if (aura == null)
		{
			return false;
		}
		auraList.Remove(aura);
		SetEvent(Enums.EventActivation.AuraCurseRemoved, this, 0, aura.ACData.Id);
		return true;
	}

	public List<string> GetCurseList()
	{
		List<string> list = new List<string>();
		if (auraList != null)
		{
			for (int i = 0; i < auraList.Count; i++)
			{
				if (!auraList[i].ACData.IsAura)
				{
					list.Add(auraList[i].ACData.Id);
				}
			}
		}
		return list;
	}

	public bool HasEffectSkipsTurn()
	{
		for (int i = 0; i < auraList.Count; i++)
		{
			if (auraList[i] != null && auraList[i].ACData != null && auraList[i].ACData.SkipsNextTurn)
			{
				return true;
			}
		}
		return false;
	}

	public int EffectCharges(string effect)
	{
		effect = effect.ToLower();
		for (int i = 0; i < auraList.Count; i++)
		{
			if (auraList[i] != null && auraList[i].ACData != null && auraList[i].ACData.Id == effect)
			{
				return auraList[i].AuraCharges;
			}
		}
		return 0;
	}

	public void ConsumeEffectCharges(string effect, int charges = -10000)
	{
		effect = effect.ToLower();
		for (int i = 0; i < auraList.Count; i++)
		{
			if (auraList[i] == null || !(auraList[i].ACData != null) || !(auraList[i].ACData.Id == effect))
			{
				continue;
			}
			if (charges == -10000)
			{
				charges = auraList[i].AuraCharges;
			}
			auraList[i].AuraCharges -= charges;
			if (effect == "block")
			{
				if (auraList[i].AuraCharges <= 0)
				{
					auraList[i].AuraCharges = 0;
				}
				UpdateAuraCurseFunctions();
			}
			else if (auraList[i].AuraCharges <= 0)
			{
				if (effect != "block" && effect != "dark")
				{
					auraList[i].AuraCharges = 1;
				}
				MatchManager.Instance.ConsumeAuraCurse("Now", this, effect);
			}
			else
			{
				UpdateAuraCurseFunctions();
			}
			break;
		}
	}

	public void HealCurses(int numCurses)
	{
		bool flag = false;
		if (numCurses == -1)
		{
			numCurses = auraList.Count;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<s>");
		int num = 0;
		for (int i = 0; i < numCurses; i++)
		{
			for (int j = 0; j < auraList.Count; j++)
			{
				if (!auraList[j].ACData.IsAura && auraList[j].ACData.Removable)
				{
					stringBuilder.Append(Functions.UppercaseFirst(auraList[j].ACData.ACName));
					stringBuilder.Append("\n");
					num++;
					SetEvent(Enums.EventActivation.AuraCurseRemoved, this, 0, auraList[j].ACData.Id);
					auraList.RemoveAt(j);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				break;
			}
		}
		if (num > 0)
		{
			SetEvent(Enums.EventActivation.CurseRemoved, this, num);
		}
		if (flag)
		{
			stringBuilder.Append("</s>");
			if (heroItem != null)
			{
				heroItem.ScrollCombatText(stringBuilder.ToString(), Enums.CombatScrollEffectType.Curse);
			}
			else if (npcItem != null)
			{
				npcItem.ScrollCombatText(stringBuilder.ToString(), Enums.CombatScrollEffectType.Curse);
			}
			UpdateAuraCurseFunctions();
		}
	}

	public void HealAuraCurse(AuraCurseData AC)
	{
		int num = 0;
		for (int num2 = auraList.Count - 1; num2 >= 0; num2--)
		{
			if (auraList[num2] != null && auraList[num2].ACData != null && auraList[num2].ACData.Id == AC.Id)
			{
				SetEvent(Enums.EventActivation.AuraCurseRemoved, this, 0, auraList[num2].ACData.Id);
				if (!AuraList[num2].ACData.IsAura)
				{
					num++;
				}
				auraList.RemoveAt(num2);
				UpdateAuraCurseFunctions();
				return;
			}
		}
		if (num > 0)
		{
			SetEvent(Enums.EventActivation.CurseRemoved, this, num);
		}
	}

	public void HealCursesName(List<string> curseList = null, string singleCurse = "")
	{
		bool flag = false;
		if (curseList == null && singleCurse != "")
		{
			curseList = new List<string>();
			curseList.Add(singleCurse);
		}
		int num = 0;
		for (int num2 = auraList.Count - 1; num2 >= 0; num2--)
		{
			if (auraList[num2] != null && auraList[num2].ACData != null && curseList.Contains(auraList[num2].ACData.Id))
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("<s>");
				stringBuilder.Append(Functions.UppercaseFirst(auraList[num2].ACData.ACName));
				stringBuilder.Append("</s>");
				if (heroItem != null)
				{
					heroItem.ScrollCombatText(stringBuilder.ToString(), Enums.CombatScrollEffectType.Curse);
				}
				else if (npcItem != null)
				{
					npcItem.ScrollCombatText(stringBuilder.ToString(), Enums.CombatScrollEffectType.Curse);
				}
				SetEvent(Enums.EventActivation.AuraCurseRemoved, this, 0, auraList[num2].ACData.Id);
				if (AuraList[num2].ACData.Id == "block")
				{
					SetEvent(Enums.EventActivation.BlockReachedZero);
				}
				if (!AuraList[num2].ACData.IsAura)
				{
					num++;
				}
				auraList.RemoveAt(num2);
				flag = true;
			}
		}
		if (num > 0)
		{
			SetEvent(Enums.EventActivation.CurseRemoved, this, num);
		}
		if (flag)
		{
			UpdateAuraCurseFunctions();
		}
	}

	public void DispelAuras(int numAuras)
	{
		bool flag = false;
		if (numAuras == -1)
		{
			numAuras = auraList.Count;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<s>");
		int num = 0;
		for (int i = 0; i < numAuras; i++)
		{
			for (int j = 0; j < auraList.Count; j++)
			{
				if (auraList[j].ACData.IsAura && auraList[j].ACData.Removable && !(auraList[j].ACData.Id == "invulnerableunremovable"))
				{
					stringBuilder.Append(Functions.UppercaseFirst(auraList[j].ACData.ACName));
					stringBuilder.Append("\n");
					if (!AuraList[j].ACData.IsAura)
					{
						num++;
					}
					SetEvent(Enums.EventActivation.AuraCurseRemoved, this, 0, auraList[j].ACData.Id);
					auraList.RemoveAt(j);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				break;
			}
		}
		if (num > 0)
		{
			SetEvent(Enums.EventActivation.CurseRemoved, this, num);
		}
		if (flag)
		{
			stringBuilder.Append("</s>");
			if (heroItem != null)
			{
				heroItem.ScrollCombatText(stringBuilder.ToString(), Enums.CombatScrollEffectType.Curse);
			}
			else if (npcItem != null)
			{
				npcItem.ScrollCombatText(stringBuilder.ToString(), Enums.CombatScrollEffectType.Curse);
			}
			MatchManager.Instance.RefreshStatusEffects();
		}
	}

	public bool CharacterIsDraculaBat()
	{
		if (npcData?.Id == "shadowbat" || npcData?.Id == "bloodbat" || npcData?.Id == "firebat")
		{
			return true;
		}
		return false;
	}

	private void CheckGainBlockCharges(int charges)
	{
		for (int i = 0; i < auraList.Count; i++)
		{
			if (auraList[i].ACData.BlockChargesGainedPerStack > 0)
			{
				SetAura(this, Globals.Instance.GetAuraCurseData("block"), auraList[i].ACData.BlockChargesGainedPerStack * charges);
				break;
			}
		}
	}

	public void DamageReflected(Hero theCasterHero, NPC theCasterNPC, int damageAmount = 0, int blockedAmount = 0)
	{
		if (isHero && theCasterHero != null)
		{
			return;
		}
		for (int i = 0; i < auraList.Count; i++)
		{
			if (auraList[i] == null)
			{
				continue;
			}
			AuraCurseData aCData = auraList[i].ACData;
			if (!(aCData != null) || aCData.DamageReflectedMultiplier <= 0 || GetAuraCharges(aCData.Id) < aCData.ChargesPreReqForDamageReflection)
			{
				continue;
			}
			switch (aCData.DamageReflectedModifierType)
			{
			case Enums.RefectedDamageModifierType.DamagePerAuraCharge:
			{
				if (aCData.Id == "thorns" && theCasterNPC != null && AtOManager.Instance.CharacterHavePerk(subclassName, "mainperkthorns1c"))
				{
					theCasterNPC.SetAura(this, Globals.Instance.GetAuraCurseData("poison"), Functions.FuncRoundToInt((float)auraList[i].AuraCharges * 0.5f));
					break;
				}
				int num = aCData.DamageReflectedMultiplier * auraList[i].AuraCharges;
				if (theCasterHero != null)
				{
					theCasterHero.IndirectDamage(aCData.DamageReflectedType, num, theCasterHero, null, aCData.Id);
				}
				else if (theCasterNPC != null)
				{
					if (aCData.Id == "thorns" && AtOManager.Instance.TeamHavePerk("mainperkrust0c") && theCasterNPC.GetAuraCharges("rust") > 0)
					{
						float num2 = 1f + (float)theCasterNPC.GetAuraCharges("rust") * 0.05f;
						num = Functions.FuncRoundToInt((float)num * num2);
					}
					theCasterNPC.IndirectDamage(aCData.DamageReflectedType, num, theCasterNPC, null, aCData.Id);
				}
				break;
			}
			case Enums.RefectedDamageModifierType.DamagePerDamageReceived:
				damageAmount *= aCData.DamageReflectedMultiplier;
				if (theCasterHero != null)
				{
					theCasterHero.IndirectDamage(aCData.DamageReflectedType, damageAmount, theCasterHero, null, aCData.Id);
				}
				else
				{
					theCasterNPC?.IndirectDamage(aCData.DamageReflectedType, damageAmount, theCasterNPC, null, aCData.Id);
				}
				break;
			case Enums.RefectedDamageModifierType.DamagePerDamageBlocked:
				blockedAmount *= aCData.DamageReflectedMultiplier;
				if (theCasterHero != null)
				{
					theCasterHero.IndirectDamage(aCData.DamageReflectedType, blockedAmount, theCasterHero, null, aCData.Id);
				}
				else
				{
					theCasterNPC?.IndirectDamage(aCData.DamageReflectedType, blockedAmount, theCasterNPC, null, aCData.Id);
				}
				break;
			}
			if (aCData.DamageReflectedConsumeCharges > 0)
			{
				ConsumeEffectCharges(aCData.Id, aCData.DamageReflectedConsumeCharges);
			}
		}
	}

	public void GrantBlockToTeam(Hero theCasterHero, NPC theCasterNPC, int damageAmount = 0, int blockedAmount = 0)
	{
		if (isHero && theCasterHero != null)
		{
			return;
		}
		for (int i = 0; i < auraList.Count; i++)
		{
			if (auraList[i] == null)
			{
				continue;
			}
			AuraCurseData aCData = auraList[i].ACData;
			if (!(aCData != null) || !aCData.GrantBlockToTeamForAmountOfDamageBlocked || GetAuraCharges(aCData.Id) < aCData.ChargesPreReqForGrantBlockToTeamForAmountOfDamageBlocked)
			{
				continue;
			}
			Hero[] teamHero = MatchManager.Instance.GetTeamHero();
			foreach (Hero hero in teamHero)
			{
				if (hero != null && hero.alive)
				{
					hero.SetAuraTrait(this, "block", blockedAmount);
				}
			}
		}
	}

	public void SimpleHeal(int heal)
	{
		if (Alive && heal > 0)
		{
			ModifyHp(heal, _includeInStats: false);
			CastResolutionForCombatText castResolutionForCombatText = new CastResolutionForCombatText();
			castResolutionForCombatText.heal = heal;
			if (heroItem != null)
			{
				heroItem.ScrollCombatTextDamageNew(castResolutionForCombatText);
			}
			else if (npcItem != null)
			{
				npcItem.ScrollCombatTextDamageNew(castResolutionForCombatText);
			}
		}
	}

	public void PercentHeal(float _healPercent, bool _includeInStats)
	{
		if (Alive)
		{
			int num = Functions.FuncRoundToInt((float)GetMaxHP() * _healPercent * 0.01f);
			if (num != 0)
			{
				ModifyHp(num, _includeInStats);
			}
		}
	}

	public void HealAttacker(Hero theCasterHero, NPC theCasterNPC)
	{
		if ((isHero && theCasterHero != null) || (theCasterHero != null && !theCasterHero.Alive) || (theCasterNPC != null && !theCasterNPC.Alive))
		{
			return;
		}
		new Dictionary<Enums.DamageType, int>();
		for (int i = 0; i < auraList.Count; i++)
		{
			if (auraList[i] == null)
			{
				continue;
			}
			AuraCurseData aCData = auraList[i].ACData;
			if (!(aCData != null) || aCData.HealAttackerPerStack <= 0)
			{
				continue;
			}
			int num = aCData.HealAttackerPerStack * auraList[i].AuraCharges;
			int num2 = num;
			if (theCasterHero != null)
			{
				if (theCasterHero.GetHpLeftForMax() > 0)
				{
					num2 = theCasterHero.HealReceivedFinal(num, isIndirect: true);
					if (theCasterHero.GetHpLeftForMax() < num)
					{
						num2 = theCasterHero.GetHpLeftForMax();
					}
					if (num2 > 0)
					{
						MatchManager.Instance.DamageStatusFromCombatStats(id, "sanctify", num2);
						theCasterHero.SimpleHeal(num2);
					}
					if (aCData.Id == "sanctify" && theCasterHero.SubclassName == "paladin" && AtOManager.Instance.TeamHaveTrait("beaconoflight"))
					{
						List<Hero> heroSides = MatchManager.Instance.GetHeroSides(theCasterHero.position);
						for (int j = 0; j < heroSides.Count; j++)
						{
							traitClass.DoTrait(Enums.EventActivation.None, "overflow", theCasterHero, heroSides[j], theCasterHero.HealReceivedFinal(num, isIndirect: true), "", null);
						}
					}
				}
				else if (aCData.Id == "sanctify" && theCasterHero.SubclassName == "paladin" && AtOManager.Instance.TeamHaveTrait("overflow"))
				{
					List<Hero> heroSides2 = MatchManager.Instance.GetHeroSides(theCasterHero.position);
					for (int k = 0; k < heroSides2.Count; k++)
					{
						traitClass.DoTrait(Enums.EventActivation.None, "overflow", theCasterHero, heroSides2[k], theCasterHero.HealReceivedFinal(num, isIndirect: true), "", null);
					}
				}
			}
			else if (theCasterNPC != null)
			{
				num2 = theCasterNPC.HealReceivedFinal(num, isIndirect: true);
				if (theCasterNPC.GetHpLeftForMax() < num)
				{
					num2 = theCasterNPC.GetHpLeftForMax();
				}
				if (num2 > 0)
				{
					MatchManager.Instance.DamageStatusFromCombatStats(id, "sanctify", num2);
					theCasterNPC.SimpleHeal(num2);
				}
			}
			if (aCData.HealAttackerConsumeCharges > 0)
			{
				ConsumeEffectCharges(aCData.Id, aCData.HealAttackerConsumeCharges);
			}
		}
	}

	public void SetStealth()
	{
		if (!IsParalyzed() && !IsSleep())
		{
			if (heroItem != null)
			{
				heroItem.SetStealth(HasEffect("stealth"));
			}
			else if (npcItem != null)
			{
				npcItem.SetStealth(HasEffect("stealth"));
			}
		}
	}

	public void SetParalyze()
	{
		if (heroItem != null)
		{
			heroItem.SetParalyze(IsParalyzed());
		}
		else if (npcItem != null)
		{
			npcItem.SetParalyze(IsParalyzed());
		}
		NPC[] array = MatchManager.Instance?.GetTeamNPC();
		for (int i = 0; i < array.Length; i++)
		{
			array[i]?.UpdateOverDeck();
		}
	}

	private void SetOverDebuff()
	{
		if (!(heroItem != null))
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (HasEffect("disarm"))
		{
			stringBuilder.Append("<size=+.5><sprite name=disarm></size>");
			stringBuilder.Append(Texts.Instance.GetText("disarm"));
		}
		if (HasEffect("silence"))
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append("<br>");
			}
			stringBuilder.Append("<size=+.5><sprite name=silence></size>");
			stringBuilder.Append(Texts.Instance.GetText("silence"));
		}
		if (HasEffect("reloading"))
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append("<br>");
			}
			stringBuilder.Append("<size=3><sprite name=reloading>");
			stringBuilder.Append(Texts.Instance.GetText("reloading"));
			stringBuilder.Append("</size>");
			stringBuilder.Append("<br><br>");
		}
		heroItem.SetOverDebuff(stringBuilder.ToString());
	}

	public void SetTaunt()
	{
		if (!IsParalyzed() && !IsSleep())
		{
			if (heroItem != null)
			{
				heroItem.SetTaunt(IsTaunted());
			}
			else if (npcItem != null)
			{
				npcItem.SetTaunt(IsTaunted());
			}
		}
	}

	public bool IsStealthed()
	{
		return HasEffect("stealth");
	}

	public bool IsTaunted()
	{
		return HasEffect("taunt");
	}

	public bool IsParalyzed()
	{
		return HasEffect("paralyze");
	}

	public bool IsInvulnerable()
	{
		return HasEffect("invulnerable");
	}

	public bool IsSleep()
	{
		return HasEffect("sleep");
	}

	public bool IsImmune(Enums.DamageType damageType)
	{
		switch (damageType)
		{
		case Enums.DamageType.Slashing:
			if (immuneSlashing)
			{
				return true;
			}
			break;
		case Enums.DamageType.Blunt:
			if (immuneBlunt)
			{
				return true;
			}
			break;
		case Enums.DamageType.Piercing:
			if (immunePiercing)
			{
				return true;
			}
			break;
		case Enums.DamageType.Fire:
			if (immuneFire)
			{
				return true;
			}
			break;
		case Enums.DamageType.Cold:
			if (immuneCold)
			{
				return true;
			}
			break;
		case Enums.DamageType.Lightning:
			if (immuneLightning)
			{
				return true;
			}
			break;
		case Enums.DamageType.Mind:
			if (immuneMind)
			{
				return true;
			}
			break;
		case Enums.DamageType.Holy:
			if (immuneHoly)
			{
				return true;
			}
			break;
		case Enums.DamageType.Shadow:
			if (immuneShadow)
			{
				return true;
			}
			break;
		}
		return false;
	}

	public int GetCardCostModifiers()
	{
		int num = 0;
		for (int i = 0; i < auraList.Count; i++)
		{
			if (auraList[i] != null)
			{
				AuraCurseData aCData = auraList[i].ACData;
				if (aCData != null && aCData.ModifyCardCostPerChargeNeededForOne > 0)
				{
					num += Mathf.FloorToInt(1f / (float)aCData.ModifyCardCostPerChargeNeededForOne * (float)auraList[i].AuraCharges);
				}
			}
		}
		return num;
	}

	public int GetCardFinalCost(CardData cardData)
	{
		if (cardData != null)
		{
			return cardData.GetCardFinalCost();
		}
		return 0;
	}

	public int GetAuraCurseTotal(bool _auras, bool _curses)
	{
		int num = 0;
		for (int i = 0; i < auraList.Count; i++)
		{
			if (auraList[i] == null)
			{
				continue;
			}
			if (_auras && _curses)
			{
				num++;
				continue;
			}
			AuraCurseData aCData = auraList[i].ACData;
			if (aCData != null)
			{
				if (aCData.IsAura && _auras)
				{
					num++;
				}
				else if (!aCData.IsAura && _curses)
				{
					num++;
				}
			}
		}
		return num;
	}

	public int GetAuraDrawModifiers(bool writeVar = true)
	{
		int num = 0;
		for (int i = 0; i < auraList.Count; i++)
		{
			if (auraList[i] != null)
			{
				AuraCurseData aCData = auraList[i].ACData;
				if (aCData != null && aCData.CardsDrawPerStack != 0)
				{
					num += aCData.CardsDrawPerStack * auraList[i].AuraCharges;
				}
			}
		}
		if (writeVar)
		{
			drawModifier = num;
		}
		return num;
	}

	public bool CanPlayCard(CardData cardData)
	{
		List<Enums.CardType> auraDisabledCardTypes = GetAuraDisabledCardTypes();
		if (auraDisabledCardTypes == null || auraDisabledCardTypes.Count == 0)
		{
			return true;
		}
		if (auraDisabledCardTypes.Contains(cardData.CardType))
		{
			return false;
		}
		for (int i = 0; i < cardData.CardTypeAux.Length; i++)
		{
			if (auraDisabledCardTypes.Contains(cardData.CardTypeAux[i]))
			{
				return false;
			}
		}
		return true;
	}

	public bool CanPlayCardSummon(CardData cardData)
	{
		if (cardData.SummonUnit != null && !cardData.Evolve && !cardData.Metamorph && MatchManager.Instance.GetNPCAvailablePosition() == -1)
		{
			return false;
		}
		return true;
	}

	public List<Enums.CardType> GetAuraDisabledCardTypes()
	{
		List<Enums.CardType> list = new List<Enums.CardType>();
		for (int i = 0; i < auraList.Count; i++)
		{
			if (auraList[i] == null)
			{
				continue;
			}
			AuraCurseData aCData = auraList[i].ACData;
			if (aCData != null && aCData.DisabledCardTypes != null && aCData.DisabledCardTypes.Length != 0)
			{
				for (int j = 0; j < aCData.DisabledCardTypes.Length; j++)
				{
					list.Add(aCData.DisabledCardTypes[j]);
				}
			}
		}
		return list;
	}

	public int GetAuraCharges(string ACName)
	{
		if (auraList != null)
		{
			for (int i = 0; i < auraList.Count; i++)
			{
				if (auraList[i] != null && auraList[i].ACData != null && auraList[i].ACData.Id == ACName.ToLower())
				{
					return auraList[i].AuraCharges;
				}
			}
			if (ACName.ToLower() == "stealthbonus")
			{
				for (int j = 0; j < auraList.Count; j++)
				{
					if (auraList[j] != null && auraList[j].ACData != null && auraList[j].ACData.Id == "stealth")
					{
						return auraList[j].AuraCharges;
					}
				}
			}
		}
		return 0;
	}

	public bool HasCardDataString(string _cardDataId, bool _checkUpgrades)
	{
		_cardDataId = _cardDataId.ToLower();
		List<string> idListVarsFromCard = Functions.GetIdListVarsFromCard(_cardDataId);
		for (int i = 0; i < cards.Count; i++)
		{
			if (cards[i] == _cardDataId)
			{
				return true;
			}
			if (_checkUpgrades && idListVarsFromCard != null && idListVarsFromCard.Contains(cards[i]))
			{
				return true;
			}
		}
		return false;
	}

	public bool HasItemCardData(CardData itemCardData)
	{
		string text = itemCardData.Id;
		if (weapon != "" && Globals.Instance.GetCardData(weapon, instantiate: false) != null && Globals.Instance.GetCardData(weapon, instantiate: false).Id == text)
		{
			return true;
		}
		if (armor != "" && Globals.Instance.GetCardData(armor, instantiate: false) != null && Globals.Instance.GetCardData(armor, instantiate: false).Id == text)
		{
			return true;
		}
		if (jewelry != "" && Globals.Instance.GetCardData(jewelry, instantiate: false) != null && Globals.Instance.GetCardData(jewelry, instantiate: false).Id == text)
		{
			return true;
		}
		if (accesory != "" && Globals.Instance.GetCardData(accesory, instantiate: false) != null && Globals.Instance.GetCardData(accesory, instantiate: false).Id == text)
		{
			return true;
		}
		if (pet != "" && Globals.Instance.GetCardData(pet, instantiate: false) != null && Globals.Instance.GetCardData(pet, instantiate: false).Id == text)
		{
			return true;
		}
		if (enchantment != "" && Globals.Instance.GetCardData(enchantment, instantiate: false) != null && Globals.Instance.GetCardData(enchantment, instantiate: false).Id == text)
		{
			return true;
		}
		if (enchantment2 != "" && Globals.Instance.GetCardData(enchantment2, instantiate: false) != null && Globals.Instance.GetCardData(enchantment2, instantiate: false).Id == text)
		{
			return true;
		}
		if (enchantment3 != "" && Globals.Instance.GetCardData(enchantment3, instantiate: false) != null && Globals.Instance.GetCardData(enchantment3, instantiate: false).Id == text)
		{
			return true;
		}
		return false;
	}

	public CardData GetCardDataBySlot(int slot, bool useCache = true)
	{
		if (useCache && slot != 6 && slot != 4 && cardDataBySlot[slot] != null)
		{
			return cardDataBySlot[slot];
		}
		string text = "";
		switch (slot)
		{
		case 0:
			text = weapon;
			break;
		case 1:
			text = armor;
			break;
		case 2:
			text = jewelry;
			break;
		case 3:
			text = accesory;
			break;
		case 4:
			text = corruption;
			break;
		case 5:
			text = pet;
			break;
		case 6:
			text = enchantment;
			break;
		case 7:
			text = enchantment2;
			break;
		case 8:
			text = enchantment3;
			break;
		}
		if (text != "")
		{
			CardData cardData = Globals.Instance.GetCardData(text);
			if (cardData != null)
			{
				cardDataBySlot[slot] = cardData;
				return cardData;
			}
		}
		return null;
	}

	public void RefreshItems(int slot = -1)
	{
		if (slot == -1)
		{
			for (int i = 0; i < 8; i++)
			{
				GetItemDataBySlot(i, useCache: false);
			}
		}
		else
		{
			GetItemDataBySlot(slot, useCache: false);
		}
	}

	public void ResetItemDataBySlotCache(int slot = -1)
	{
		if (slot == -1)
		{
			itemDataBySlot = new ItemData[9];
			cardDataBySlot = new CardData[9];
		}
		else
		{
			itemDataBySlot[slot] = null;
			cardDataBySlot[slot] = null;
		}
	}

	public ItemData GetItemDataBySlot(int slot, bool useCache = true)
	{
		string text = "";
		switch (slot)
		{
		case 0:
			text = weapon;
			break;
		case 1:
			text = armor;
			break;
		case 2:
			text = jewelry;
			break;
		case 3:
			text = accesory;
			break;
		case 4:
			text = corruption;
			break;
		case 5:
			text = pet;
			break;
		case 6:
			text = enchantment;
			break;
		case 7:
			text = enchantment2;
			break;
		case 8:
			text = enchantment3;
			break;
		}
		if (text != "")
		{
			CardData cardData = Globals.Instance.GetCardData(text, instantiate: false);
			if (cardData != null)
			{
				if (cardData.CardType == Enums.CardType.Enchantment)
				{
					itemDataBySlot[slot] = cardData.ItemEnchantment;
				}
				else
				{
					itemDataBySlot[slot] = cardData.Item;
				}
				return itemDataBySlot[slot];
			}
		}
		return null;
	}

	public int GetItemsMaxHPModifier()
	{
		int num = 0;
		for (int i = 0; i < itemSlots; i++)
		{
			if (!(heroData != null) || i != 4)
			{
				ItemData itemData = GetItemDataBySlot(i);
				if (itemData != null && itemData.MaxHealth != 0)
				{
					num += itemData.MaxHealth;
				}
			}
		}
		return num;
	}

	public int GetItemHealFlatBonus()
	{
		if ((bool)MatchManager.Instance && useCache && cacheGetItemHealFlatBonus.Count > 0)
		{
			return cacheGetItemHealFlatBonus[0];
		}
		int num = 0;
		for (int i = 0; i < itemSlots; i++)
		{
			if (!(heroData != null) || i != 4)
			{
				ItemData itemData = GetItemDataBySlot(i);
				if (itemData != null && itemData.HealFlatBonus != 0)
				{
					num += itemData.HealFlatBonus;
				}
			}
		}
		if ((bool)MatchManager.Instance && useCache)
		{
			cacheGetItemHealFlatBonus.Add(num);
		}
		return num;
	}

	public float GetItemHealPercentBonus()
	{
		if ((bool)MatchManager.Instance && useCache && cacheGetItemHealPercentBonus.Count > 0)
		{
			return cacheGetItemHealPercentBonus[0];
		}
		float num = 0f;
		for (int i = 0; i < itemSlots; i++)
		{
			if (!(heroData != null) || i != 4)
			{
				ItemData itemData = GetItemDataBySlot(i);
				if (itemData != null && itemData.HealPercentBonus != 0f)
				{
					num += itemData.HealPercentBonus;
				}
			}
		}
		if ((bool)MatchManager.Instance && useCache)
		{
			cacheGetItemHealPercentBonus.Add(num);
		}
		return num;
	}

	public int GetItemHealReceivedFlatBonus()
	{
		if ((bool)MatchManager.Instance && useCache && cacheGetItemHealReceivedFlatBonus.Count > 0)
		{
			return cacheGetItemHealReceivedFlatBonus[0];
		}
		int num = 0;
		for (int i = 0; i < itemSlots; i++)
		{
			if (!(heroData != null) || i != 4)
			{
				ItemData itemData = GetItemDataBySlot(i);
				if (itemData != null && itemData.HealReceivedFlatBonus != 0)
				{
					num += itemData.HealReceivedFlatBonus;
				}
			}
		}
		if ((bool)MatchManager.Instance && useCache)
		{
			cacheGetItemHealReceivedFlatBonus.Add(num);
		}
		return num;
	}

	public float GetItemHealReceivedPercentBonus()
	{
		if ((bool)MatchManager.Instance && useCache && cacheGetItemHealReceivedPercentBonus.Count > 0)
		{
			return cacheGetItemHealReceivedPercentBonus[0];
		}
		float num = 0f;
		for (int i = 0; i < itemSlots; i++)
		{
			if (!(heroData != null) || i != 4)
			{
				ItemData itemData = GetItemDataBySlot(i);
				if (itemData != null && itemData.HealReceivedPercentBonus != 0f)
				{
					num += itemData.HealReceivedPercentBonus;
				}
			}
		}
		if ((bool)MatchManager.Instance && useCache)
		{
			cacheGetItemHealReceivedPercentBonus.Add(num);
		}
		return num;
	}

	public Dictionary<string, int> GetItemHealPercentDictionary()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		float num = 0f;
		for (int i = 0; i < itemSlots; i++)
		{
			if (heroData != null && i == 4)
			{
				continue;
			}
			num = 0f;
			ItemData itemData = GetItemDataBySlot(i);
			if (itemData != null)
			{
				if (itemData.HealPercentBonus != 0f)
				{
					num += itemData.HealPercentBonus;
				}
				if (num != 0f)
				{
					StringBuilder stringBuilder = new StringBuilder();
					CardData cardData = GetCardDataBySlot(i);
					stringBuilder.Append(cardData.CardName);
					stringBuilder.Append("_");
					stringBuilder.Append(cardData.CardType);
					dictionary.Add(stringBuilder.ToString(), (int)num);
				}
			}
		}
		return dictionary;
	}

	public Dictionary<string, int> GetItemHealFlatDictionary()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		float num = 0f;
		for (int i = 0; i < itemSlots; i++)
		{
			if (heroData != null && i == 4)
			{
				continue;
			}
			num = 0f;
			ItemData itemData = GetItemDataBySlot(i);
			if (itemData != null)
			{
				if (itemData.HealFlatBonus != 0)
				{
					num += (float)itemData.HealFlatBonus;
				}
				if (num != 0f)
				{
					StringBuilder stringBuilder = new StringBuilder();
					CardData cardData = GetCardDataBySlot(i);
					stringBuilder.Append(cardData.CardName);
					stringBuilder.Append("_");
					stringBuilder.Append(cardData.CardType);
					dictionary.Add(stringBuilder.ToString(), (int)num);
				}
			}
		}
		return dictionary;
	}

	public Dictionary<string, int> GetItemAuraCurseModifiers()
	{
		if ((bool)MatchManager.Instance && useCache && cacheGetItemAuraCurseModifiers.Count > 0)
		{
			return cacheGetItemAuraCurseModifiers;
		}
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		for (int i = 0; i < itemSlots; i++)
		{
			if (heroData != null && i == 4)
			{
				continue;
			}
			ItemData itemData = GetItemDataBySlot(i);
			if (!(itemData != null))
			{
				continue;
			}
			if (itemData.AuracurseBonus1 != null)
			{
				if (dictionary.ContainsKey(itemData.AuracurseBonus1.Id))
				{
					dictionary[itemData.AuracurseBonus1.Id] += itemData.AuracurseBonusValue1;
				}
				else
				{
					dictionary[itemData.AuracurseBonus1.Id] = itemData.AuracurseBonusValue1;
				}
			}
			if (itemData.AuracurseBonus2 != null)
			{
				if (dictionary.ContainsKey(itemData.AuracurseBonus2.Id))
				{
					dictionary[itemData.AuracurseBonus2.Id] += itemData.AuracurseBonusValue2;
				}
				else
				{
					dictionary[itemData.AuracurseBonus2.Id] = itemData.AuracurseBonusValue2;
				}
			}
		}
		if ((bool)MatchManager.Instance && useCache)
		{
			cacheGetItemAuraCurseModifiers = dictionary;
		}
		return dictionary;
	}

	public Dictionary<string, int> GetTraitAuraCurseModifiers()
	{
		if ((bool)MatchManager.Instance && useCache && cacheGetTraitAuraCurseModifiers.Count > 0)
		{
			return cacheGetTraitAuraCurseModifiers;
		}
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		if (traits != null)
		{
			for (int i = 0; i < traits.Length; i++)
			{
				if (traits[i] == null)
				{
					continue;
				}
				TraitData traitData = Globals.Instance.GetTraitData(traits[i]);
				if (!(traitData != null))
				{
					continue;
				}
				if (traitData.AuracurseBonus1 != null)
				{
					if (dictionary.ContainsKey(traitData.AuracurseBonus1.Id))
					{
						dictionary[traitData.AuracurseBonus1.Id] += traitData.AuracurseBonusValue1;
					}
					else
					{
						dictionary[traitData.AuracurseBonus1.Id] = traitData.AuracurseBonusValue1;
					}
				}
				if (traitData.AuracurseBonus2 != null)
				{
					if (dictionary.ContainsKey(traitData.AuracurseBonus2.Id))
					{
						dictionary[traitData.AuracurseBonus2.Id] += traitData.AuracurseBonusValue2;
					}
					else
					{
						dictionary[traitData.AuracurseBonus2.Id] = traitData.AuracurseBonusValue2;
					}
				}
				if (traitData.AuracurseBonus3 != null)
				{
					if (dictionary.ContainsKey(traitData.AuracurseBonus3.Id))
					{
						dictionary[traitData.AuracurseBonus3.Id] += traitData.AuracurseBonusValue3;
					}
					else
					{
						dictionary[traitData.AuracurseBonus3.Id] = traitData.AuracurseBonusValue3;
					}
				}
			}
		}
		if (AtOManager.Instance.IsChallengeTraitActive("icydeluge"))
		{
			if (dictionary.ContainsKey("wet"))
			{
				dictionary["wet"]++;
			}
			else
			{
				dictionary["wet"] = 1;
			}
		}
		if (AtOManager.Instance.IsChallengeTraitActive("hemorrhage"))
		{
			if (dictionary.ContainsKey("bleed"))
			{
				dictionary["bleed"] += 2;
			}
			else
			{
				dictionary["bleed"] = 2;
			}
		}
		if ((bool)MatchManager.Instance && useCache)
		{
			if (dictionary.Count > 0)
			{
				cacheGetTraitAuraCurseModifiers = dictionary;
			}
			else
			{
				cacheGetTraitAuraCurseModifiers.Add("", 0);
			}
		}
		return dictionary;
	}

	public Dictionary<string, int> GetAurasAuraCurseModifiers()
	{
		auraCurseModificationDueToAuras.Clear();
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		AtOManager.Instance.doublePowerfulEffect = false;
		AtOManager.Instance.doubleFuryEffect = false;
		foreach (Aura aura in AuraList)
		{
			foreach (AuraCurseData.AuraCurseChargesBonus aCBonusDatum in aura.ACData.ACBonusData)
			{
				if (aura.GetCharges() >= aCBonusDatum.requiredChargesForBonus)
				{
					if (aCBonusDatum.acData.Id == "powerful")
					{
						AtOManager.Instance.doublePowerfulEffect = true;
						continue;
					}
					if (aCBonusDatum.acData.Id == "fury")
					{
						AtOManager.Instance.doubleFuryEffect = true;
						continue;
					}
					if (dictionary.ContainsKey(aCBonusDatum.acData.Id))
					{
						if (aCBonusDatum.bonusType == AuraCurseData.AuraCurseChargesBonus.BonusAmountType.flatBonus)
						{
							dictionary[aCBonusDatum.acData.Id] += aCBonusDatum.bonusCharges;
							auraCurseModificationDueToAuras[aCBonusDatum.acData.Id] += aCBonusDatum.bonusCharges;
						}
						else if (aCBonusDatum.bonusType == AuraCurseData.AuraCurseChargesBonus.BonusAmountType.percentageBonus)
						{
							dictionary[aCBonusDatum.acData.Id] = GetAuraCharges(aCBonusDatum.acData.Id) * aCBonusDatum.bonusCharges / 100;
							auraCurseModificationDueToAuras[aCBonusDatum.acData.Id] = GetAuraCharges(aCBonusDatum.acData.Id) * aCBonusDatum.bonusCharges / 100;
						}
						else if (aCBonusDatum.bonusType == AuraCurseData.AuraCurseChargesBonus.BonusAmountType.bonusPerCharge)
						{
							dictionary[aCBonusDatum.acData.Id] = GetAuraCharges(aura.ACData.Id) * aCBonusDatum.bonusCharges;
							auraCurseModificationDueToAuras[aCBonusDatum.acData.Id] = GetAuraCharges(aura.ACData.Id) * aCBonusDatum.bonusCharges;
						}
					}
					else if (aCBonusDatum.bonusType == AuraCurseData.AuraCurseChargesBonus.BonusAmountType.flatBonus)
					{
						dictionary.Add(aCBonusDatum.acData.Id, aCBonusDatum.bonusCharges);
						auraCurseModificationDueToAuras.Add(aCBonusDatum.acData.Id, aCBonusDatum.bonusCharges);
					}
					else if (aCBonusDatum.bonusType == AuraCurseData.AuraCurseChargesBonus.BonusAmountType.percentageBonus)
					{
						dictionary.Add(aCBonusDatum.acData.Id, GetAuraCharges(aCBonusDatum.acData.Id) * aCBonusDatum.bonusCharges / 100);
						auraCurseModificationDueToAuras.Add(aCBonusDatum.acData.Id, GetAuraCharges(aCBonusDatum.acData.Id) * aCBonusDatum.bonusCharges / 100);
					}
					else if (aCBonusDatum.bonusType == AuraCurseData.AuraCurseChargesBonus.BonusAmountType.bonusPerCharge)
					{
						dictionary.Add(aCBonusDatum.acData.Id, GetAuraCharges(aura.ACData.Id) * aCBonusDatum.bonusCharges);
						auraCurseModificationDueToAuras[aCBonusDatum.acData.Id] = GetAuraCharges(aura.ACData.Id) * aCBonusDatum.bonusCharges;
					}
					ModifyAuraCurseQuantity(aCBonusDatum.acData.Id, aCBonusDatum.bonusCharges);
				}
				else if (aCBonusDatum.acData.Id == "powerful")
				{
					AtOManager.Instance.doublePowerfulEffect = false;
				}
				else if (aCBonusDatum.acData.Id == "fury")
				{
					AtOManager.Instance.doubleFuryEffect = false;
				}
			}
		}
		MatchManager.Instance?.RefreshStatusEffects();
		return dictionary;
	}

	public int GetItemDamageFlatModifiers(Enums.DamageType DamageType)
	{
		if ((bool)MatchManager.Instance && useCache && cacheGetItemDamageFlatModifiers.ContainsKey(DamageType))
		{
			return cacheGetItemDamageFlatModifiers[DamageType];
		}
		int num = 0;
		for (int i = 0; i < itemSlots; i++)
		{
			if (heroData != null && i == 4)
			{
				continue;
			}
			ItemData itemData = GetItemDataBySlot(i);
			if (itemData != null)
			{
				if (itemData.DamageFlatBonus == DamageType || itemData.DamageFlatBonus == Enums.DamageType.All)
				{
					num += itemData.DamageFlatBonusValue;
				}
				if (itemData.DamageFlatBonus2 == DamageType || itemData.DamageFlatBonus2 == Enums.DamageType.All)
				{
					num += itemData.DamageFlatBonusValue2;
				}
				if (itemData.DamageFlatBonus3 == DamageType || itemData.DamageFlatBonus3 == Enums.DamageType.All)
				{
					num += itemData.DamageFlatBonusValue3;
				}
			}
		}
		if ((bool)MatchManager.Instance && useCache)
		{
			cacheGetItemDamageFlatModifiers.Add(DamageType, num);
		}
		return num;
	}

	public Dictionary<string, int> GetItemDamageDoneDictionary(Enums.DamageType DamageType)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		int num = 0;
		for (int i = 0; i < itemSlots; i++)
		{
			if (heroData != null && i == 4)
			{
				continue;
			}
			num = 0;
			ItemData itemData = GetItemDataBySlot(i);
			if (!(itemData != null))
			{
				continue;
			}
			if (itemData.DamageFlatBonus == DamageType || itemData.DamageFlatBonus == Enums.DamageType.All)
			{
				num += itemData.DamageFlatBonusValue;
			}
			if (itemData.DamageFlatBonus2 == DamageType || itemData.DamageFlatBonus2 == Enums.DamageType.All)
			{
				num += itemData.DamageFlatBonusValue2;
			}
			if (itemData.DamageFlatBonus3 == DamageType || itemData.DamageFlatBonus3 == Enums.DamageType.All)
			{
				num += itemData.DamageFlatBonusValue3;
			}
			if (num != 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				CardData cardData = GetCardDataBySlot(i);
				if (cardData != null)
				{
					stringBuilder.Append(cardData.CardName);
					stringBuilder.Append("_");
					stringBuilder.Append(cardData.CardType);
					dictionary.Add(stringBuilder.ToString(), num);
				}
			}
		}
		return dictionary;
	}

	public float GetItemDamagePercentModifiers(Enums.DamageType DamageType)
	{
		if ((bool)MatchManager.Instance && useCache && cacheGetItemDamagePercentModifiers.ContainsKey(DamageType))
		{
			return cacheGetItemDamagePercentModifiers[DamageType];
		}
		float num = 0f;
		for (int i = 0; i < itemSlots; i++)
		{
			if (heroData != null && i == 4)
			{
				continue;
			}
			ItemData itemData = GetItemDataBySlot(i);
			if (itemData != null)
			{
				if (itemData.DamagePercentBonus == DamageType || itemData.DamagePercentBonus == Enums.DamageType.All)
				{
					num += itemData.DamagePercentBonusValue;
				}
				if (itemData.DamagePercentBonus2 == DamageType || itemData.DamagePercentBonus2 == Enums.DamageType.All)
				{
					num += itemData.DamagePercentBonusValue2;
				}
				if (itemData.DamagePercentBonus3 == DamageType || itemData.DamagePercentBonus3 == Enums.DamageType.All)
				{
					num += itemData.DamagePercentBonusValue3;
				}
			}
		}
		if ((bool)MatchManager.Instance && useCache)
		{
			cacheGetItemDamagePercentModifiers.Add(DamageType, num);
		}
		return num;
	}

	public Dictionary<string, int> GetItemDamageDonePercentDictionary(Enums.DamageType DamageType)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		float num = 0f;
		for (int i = 0; i < itemSlots; i++)
		{
			if (heroData != null && i == 4)
			{
				continue;
			}
			num = 0f;
			ItemData itemData = GetItemDataBySlot(i);
			if (itemData != null)
			{
				if (itemData.DamagePercentBonus == DamageType || itemData.DamagePercentBonus == Enums.DamageType.All)
				{
					num += itemData.DamagePercentBonusValue;
				}
				if (itemData.DamagePercentBonus2 == DamageType || itemData.DamagePercentBonus2 == Enums.DamageType.All)
				{
					num += itemData.DamagePercentBonusValue2;
				}
				if (itemData.DamagePercentBonus2 == DamageType || itemData.DamagePercentBonus3 == Enums.DamageType.All)
				{
					num += itemData.DamagePercentBonusValue3;
				}
				if (num != 0f)
				{
					StringBuilder stringBuilder = new StringBuilder();
					CardData cardData = GetCardDataBySlot(i);
					stringBuilder.Append(cardData.CardName);
					stringBuilder.Append("_");
					stringBuilder.Append(cardData.CardType);
					dictionary.Add(stringBuilder.ToString(), (int)num);
				}
			}
		}
		return dictionary;
	}

	public int GetItemResistModifiers(Enums.DamageType type)
	{
		if ((bool)MatchManager.Instance && useCache && cacheGetItemResistModifiers.ContainsKey(type))
		{
			return cacheGetItemResistModifiers[type];
		}
		int num = 0;
		for (int i = 0; i < itemSlots; i++)
		{
			if (heroData != null && i == 4)
			{
				continue;
			}
			ItemData itemData = GetItemDataBySlot(i);
			if (itemData != null)
			{
				if (itemData.ResistModified1 == Enums.DamageType.All || itemData.ResistModified1 == type)
				{
					num += itemData.ResistModifiedValue1;
				}
				if (itemData.ResistModified2 == Enums.DamageType.All || itemData.ResistModified2 == type)
				{
					num += itemData.ResistModifiedValue2;
				}
				if (itemData.ResistModified3 == Enums.DamageType.All || itemData.ResistModified3 == type)
				{
					num += itemData.ResistModifiedValue3;
				}
			}
		}
		if ((bool)MatchManager.Instance && useCache)
		{
			cacheGetItemResistModifiers.Add(type, num);
		}
		return num;
	}

	public Dictionary<string, int> GetItemResistModifiersDictionary(Enums.DamageType type)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		int num = 0;
		for (int i = 0; i < itemSlots; i++)
		{
			if (heroData != null && i == 4)
			{
				continue;
			}
			ItemData itemData = GetItemDataBySlot(i);
			if (!(itemData != null))
			{
				continue;
			}
			num = 0;
			if (itemData.ResistModified1 == Enums.DamageType.All || itemData.ResistModified1 == type)
			{
				num += itemData.ResistModifiedValue1;
			}
			if (itemData.ResistModified2 == Enums.DamageType.All || itemData.ResistModified2 == type)
			{
				num += itemData.ResistModifiedValue2;
			}
			if (itemData.ResistModified3 == Enums.DamageType.All || itemData.ResistModified3 == type)
			{
				num += itemData.ResistModifiedValue3;
			}
			if (num != 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				CardData cardData = GetCardDataBySlot(i);
				if (cardData != null)
				{
					stringBuilder.Append(cardData.CardName);
					stringBuilder.Append("_");
					stringBuilder.Append(cardData.CardType);
				}
				if (!dictionary.ContainsKey(stringBuilder.ToString()))
				{
					dictionary.Add(stringBuilder.ToString(), num);
				}
				else
				{
					dictionary[stringBuilder.ToString()] += num;
				}
			}
		}
		return dictionary;
	}

	public int GetItemStatModifiers(Enums.CharacterStat stat)
	{
		if ((bool)MatchManager.Instance && useCache && cacheGetItemStatModifiers.ContainsKey(stat))
		{
			return cacheGetItemStatModifiers[stat];
		}
		int num = 0;
		for (int i = 0; i < itemSlots; i++)
		{
			if (heroData != null && i == 4)
			{
				continue;
			}
			ItemData itemData = GetItemDataBySlot(i);
			if (itemData != null)
			{
				if (itemData.CharacterStatModified == stat)
				{
					num += itemData.CharacterStatModifiedValue;
				}
				if (itemData.CharacterStatModified2 == stat)
				{
					num += itemData.CharacterStatModifiedValue2;
				}
				if (itemData.CharacterStatModified3 == stat)
				{
					num += itemData.CharacterStatModifiedValue3;
				}
			}
		}
		if ((bool)MatchManager.Instance && useCache)
		{
			if (!cacheGetItemStatModifiers.ContainsKey(stat))
			{
				cacheGetItemStatModifiers.Add(stat, num);
			}
			else
			{
				cacheGetItemStatModifiers[stat] = num;
			}
		}
		return num;
	}

	public int GetAuraStatModifiers(int source, Enums.CharacterStat stat)
	{
		if ((bool)MatchManager.Instance && useCache)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(GetAuraListString());
			stringBuilder.Append("-");
			stringBuilder.Append(source);
			stringBuilder.Append("-");
			stringBuilder.Append(stat);
			string key = Functions.Md5Sum(stringBuilder.ToString());
			if (cacheGetAuraStatModifiers.ContainsKey(key))
			{
				return cacheGetAuraStatModifiers[key];
			}
		}
		int num = 0;
		num = source;
		bool flag = false;
		for (int i = 0; i < auraList.Count; i++)
		{
			if (auraList[i] == null)
			{
				continue;
			}
			AuraCurseData aCData = auraList[i].ACData;
			if (!(aCData != null) || aCData.CharacterStatModified != stat)
			{
				continue;
			}
			aCData = AtOManager.Instance.GlobalAuraCurseModificationByTraitsAndItems("set", aCData.Id, this, this);
			if (aCData.CharacterStatAbsolute)
			{
				if (!(aCData.Id == "shackle") || stat != Enums.CharacterStat.Speed)
				{
					num = aCData.CharacterStatAbsoluteValue + aCData.CharacterStatAbsoluteValuePerStack * auraList[i].AuraCharges;
					break;
				}
				flag = true;
			}
			else
			{
				num += aCData.CharacterStatModifiedValue;
				int num2 = 0;
				float num3 = 1f / (float)aCData.CharacterStatChargesMultiplierNeededForOne;
				num2 = ((!(aCData.CharacterStatModifiedValuePerStack < 0f)) ? Mathf.FloorToInt(num3 * (float)auraList[i].AuraCharges * aCData.CharacterStatModifiedValuePerStack) : (-1 * Mathf.FloorToInt(Mathf.Abs(num3 * (float)auraList[i].AuraCharges * aCData.CharacterStatModifiedValuePerStack))));
				num += num2;
			}
		}
		if (num > 0 && flag)
		{
			num = 0;
		}
		if ((bool)MatchManager.Instance && useCache)
		{
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append(GetAuraListString());
			stringBuilder2.Append("-");
			stringBuilder2.Append(source);
			stringBuilder2.Append("-");
			stringBuilder2.Append(stat);
			string key2 = Functions.Md5Sum(stringBuilder2.ToString());
			if (!cacheGetAuraStatModifiers.ContainsKey(key2))
			{
				cacheGetAuraStatModifiers.Add(key2, num);
			}
			else
			{
				cacheGetAuraStatModifiers[key2] = num;
			}
		}
		return num;
	}

	public Dictionary<string, int> GetAuraResistModifiersDictionary(Enums.DamageType damageType)
	{
		int num = 0;
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		for (int i = 0; i < auraList.Count; i++)
		{
			AuraCurseData aCData = auraList[i].ACData;
			if (aCData != null)
			{
				num = auraList[i].AuraCharges;
				int num2 = 0;
				if (aCData.IncreasedDamageReceivedType == damageType || aCData.IncreasedDamageReceivedType == Enums.DamageType.All)
				{
					num2 -= aCData.IncreasedPercentDamageReceivedPerStack * num;
				}
				if (aCData.IncreasedDamageReceivedType2 == damageType)
				{
					num2 -= aCData.IncreasedPercentDamageReceivedPerStack2 * num;
				}
				if (aCData.ResistModified == Enums.DamageType.All)
				{
					num2 += Functions.FuncRoundToInt(aCData.ResistModifiedPercentagePerStack * (float)num);
					num2 += Functions.FuncRoundToInt(aCData.ResistModifiedValue);
				}
				if (aCData.ResistModified == damageType)
				{
					num2 += Functions.FuncRoundToInt(aCData.ResistModifiedPercentagePerStack * (float)num);
					num2 += Functions.FuncRoundToInt(aCData.ResistModifiedValue);
				}
				if (aCData.ResistModified2 == damageType)
				{
					num2 += Functions.FuncRoundToInt(aCData.ResistModifiedPercentagePerStack2 * (float)num);
					num2 += Functions.FuncRoundToInt(aCData.ResistModifiedValue2);
				}
				if (aCData.ResistModified3 == damageType)
				{
					num2 += Functions.FuncRoundToInt(aCData.ResistModifiedPercentagePerStack3 * (float)num);
					num2 += Functions.FuncRoundToInt(aCData.ResistModifiedValue3);
				}
				if (num2 != 0)
				{
					dictionary.Add(aCData.Id, num2);
				}
			}
		}
		return dictionary;
	}

	public int GetAuraResistModifiers(Enums.DamageType damageType, string acId = "", bool countChargesConsumedPre = false, bool countChargesConsumedPost = false, CardData cardAux = null)
	{
		if ((bool)MatchManager.Instance && useCache)
		{
			string value = ((cardAux == null) ? "" : cardAux.Id);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(GetAuraListString());
			stringBuilder.Append("-");
			stringBuilder.Append(damageType);
			stringBuilder.Append("-");
			stringBuilder.Append(acId);
			stringBuilder.Append("-");
			stringBuilder.Append(countChargesConsumedPre);
			stringBuilder.Append("-");
			stringBuilder.Append(countChargesConsumedPost);
			stringBuilder.Append("-");
			stringBuilder.Append(value);
			string key = Functions.Md5Sum(stringBuilder.ToString());
			if (cacheGetAuraResistModifiers.ContainsKey(key))
			{
				return cacheGetAuraResistModifiers[key];
			}
		}
		int num = 0;
		int num2 = 0;
		bool flag = false;
		for (int i = 0; i < auraList.Count; i++)
		{
			AuraCurseData aCData = auraList[i].ACData;
			if (!(aCData != null) || (cardAux != null && cardAux.IsGoingToPurgeThisAC(aCData.Id)))
			{
				continue;
			}
			if (aCData.Id == acId)
			{
				flag = true;
			}
			num2 = auraList[i].AuraCharges;
			if (num2 <= 0)
			{
				continue;
			}
			if (!flag && countChargesConsumedPre)
			{
				if (aCData.ConsumedAtTurnBegin && num2 <= aCData.AuraConsumed)
				{
					continue;
				}
				num2 -= aCData.AuraConsumed;
			}
			if (!flag && countChargesConsumedPost)
			{
				if (aCData.ConsumedAtTurn && num2 <= aCData.AuraConsumed)
				{
					continue;
				}
				num2 -= aCData.AuraConsumed;
			}
			if (aCData.IncreasedDamageReceivedType == damageType || aCData.IncreasedDamageReceivedType == Enums.DamageType.All)
			{
				num -= aCData.IncreasedPercentDamageReceivedPerStack * num2;
			}
			if (aCData.IncreasedDamageReceivedType2 == damageType)
			{
				num -= aCData.IncreasedPercentDamageReceivedPerStack2 * num2;
			}
			if (aCData.ResistModified == Enums.DamageType.All)
			{
				num += Functions.FuncRoundToInt(aCData.ResistModifiedPercentagePerStack * (float)num2);
				num += Functions.FuncRoundToInt(aCData.ResistModifiedValue);
			}
			if (aCData.ResistModified == damageType)
			{
				num += Functions.FuncRoundToInt(aCData.ResistModifiedPercentagePerStack * (float)num2);
				num += Functions.FuncRoundToInt(aCData.ResistModifiedValue);
			}
			if (aCData.ResistModified2 == damageType)
			{
				num += Functions.FuncRoundToInt(aCData.ResistModifiedPercentagePerStack2 * (float)num2);
				num += Functions.FuncRoundToInt(aCData.ResistModifiedValue2);
			}
			if (aCData.ResistModified3 == damageType)
			{
				num += Functions.FuncRoundToInt(aCData.ResistModifiedPercentagePerStack3 * (float)num2);
				num += Functions.FuncRoundToInt(aCData.ResistModifiedValue3);
			}
		}
		if ((bool)MatchManager.Instance && useCache)
		{
			string value2 = ((cardAux == null) ? "" : cardAux.Id);
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append(GetAuraListString());
			stringBuilder2.Append("-");
			stringBuilder2.Append(damageType);
			stringBuilder2.Append("-");
			stringBuilder2.Append(acId);
			stringBuilder2.Append("-");
			stringBuilder2.Append(countChargesConsumedPre);
			stringBuilder2.Append("-");
			stringBuilder2.Append(countChargesConsumedPost);
			stringBuilder2.Append("-");
			stringBuilder2.Append(value2);
			string key2 = Functions.Md5Sum(stringBuilder2.ToString());
			cacheGetAuraResistModifiers.Add(key2, num);
		}
		return num;
	}

	public int BonusResists(Enums.DamageType damageType, string acId = "", bool countChargesConsumedPre = false, bool countChargesConsumedPost = false, CardData cardAux = null)
	{
		int num = 0;
		switch (damageType)
		{
		case Enums.DamageType.Slashing:
			if (immuneSlashing)
			{
				num = 1000;
				break;
			}
			num += resistSlashing;
			if (!isHero && AtOManager.Instance.IsChallengeTraitActive("reinforcedmonsters"))
			{
				num += 10;
			}
			break;
		case Enums.DamageType.Blunt:
			if (immuneBlunt)
			{
				num = 1000;
				break;
			}
			num += resistBlunt;
			if (!isHero && AtOManager.Instance.IsChallengeTraitActive("reinforcedmonsters"))
			{
				num += 10;
			}
			break;
		case Enums.DamageType.Piercing:
			if (immunePiercing)
			{
				num = 1000;
				break;
			}
			num += resistPiercing;
			if (!isHero && AtOManager.Instance.IsChallengeTraitActive("reinforcedmonsters"))
			{
				num += 10;
			}
			break;
		case Enums.DamageType.Fire:
			if (immuneFire)
			{
				num = 1000;
				break;
			}
			num += resistFire;
			if (!isHero && AtOManager.Instance.IsChallengeTraitActive("elementalmonsters"))
			{
				num += 10;
			}
			if (!isHero && AtOManager.Instance.AliveTeamHaveTrait("crimsonripple"))
			{
				int auraCharges = GetAuraCharges("bleed");
				num -= auraCharges / 2;
			}
			break;
		case Enums.DamageType.Cold:
			if (immuneCold)
			{
				num = 1000;
				break;
			}
			num += resistCold;
			if (!isHero && AtOManager.Instance.IsChallengeTraitActive("elementalmonsters"))
			{
				num += 10;
			}
			break;
		case Enums.DamageType.Lightning:
			if (immuneLightning)
			{
				num = 1000;
				break;
			}
			num += resistLightning;
			if (!isHero && AtOManager.Instance.IsChallengeTraitActive("elementalmonsters"))
			{
				num += 10;
			}
			break;
		case Enums.DamageType.Mind:
			if (immuneMind)
			{
				num = 1000;
				break;
			}
			num += resistMind;
			if (!isHero && AtOManager.Instance.IsChallengeTraitActive("spiritualmonsters"))
			{
				num += 10;
			}
			break;
		case Enums.DamageType.Holy:
			if (immuneHoly)
			{
				num = 1000;
				break;
			}
			num += resistHoly;
			if (!isHero && AtOManager.Instance.IsChallengeTraitActive("spiritualmonsters"))
			{
				num += 10;
			}
			break;
		case Enums.DamageType.Shadow:
			if (immuneShadow)
			{
				num = 1000;
				break;
			}
			num += resistShadow;
			if (!isHero && AtOManager.Instance.IsChallengeTraitActive("spiritualmonsters"))
			{
				num += 10;
			}
			break;
		}
		if (num < 1000)
		{
			if (!isHero)
			{
				if (MadnessManager.Instance.IsMadnessTraitActive("resistantmonsters") || AtOManager.Instance.IsChallengeTraitActive("resistantmonsters"))
				{
					num += 10;
				}
				if (AtOManager.Instance.IsChallengeTraitActive("vulnerablemonsters"))
				{
					num -= 15;
				}
			}
			num += GetItemResistModifiers(damageType);
			num += GetAuraResistModifiers(damageType, acId, countChargesConsumedPre, countChargesConsumedPost, cardAux);
		}
		return Mathf.Clamp(num, -95, 95);
	}

	public int IncreasedCursedDamagePerStack(Enums.DamageType damageType, CardData cardAux = null)
	{
		int num = 0;
		for (int i = 0; i < auraList.Count; i++)
		{
			if (auraList[i] == null)
			{
				continue;
			}
			Aura aura = auraList[i];
			if (aura.ACData != null && (!(cardAux != null) || !cardAux.IsGoingToPurgeThisAC(aura.ACData.Id)))
			{
				if (aura.ACData.IncreasedDamageReceivedType == damageType || aura.ACData.IncreasedDamageReceivedType == Enums.DamageType.All)
				{
					float num2 = 1f / (float)aura.ACData.IncreasedDirectDamageChargesMultiplierNeededForOne;
					int num3 = 0;
					num3 = ((!(aura.ACData.IncreasedDirectDamageReceivedPerStack < 0f)) ? Mathf.FloorToInt(num2 * (float)aura.AuraCharges * aura.ACData.IncreasedDirectDamageReceivedPerStack) : (-1 * Mathf.FloorToInt(Mathf.Abs(num2 * (float)aura.AuraCharges * aura.ACData.IncreasedDirectDamageReceivedPerStack))));
					num += num3;
				}
				if (aura.ACData.IncreasedDamageReceivedType2 == damageType)
				{
					float num4 = 1f / (float)aura.ACData.IncreasedDirectDamageChargesMultiplierNeededForOne2;
					int num5 = 0;
					num5 = ((!(aura.ACData.IncreasedDirectDamageReceivedPerStack2 < 0f)) ? Mathf.FloorToInt(num4 * (float)aura.AuraCharges * aura.ACData.IncreasedDirectDamageReceivedPerStack2) : (-1 * Mathf.FloorToInt(Mathf.Abs(num4 * (float)aura.AuraCharges * aura.ACData.IncreasedDirectDamageReceivedPerStack2))));
					num += num5;
				}
			}
		}
		int ngPlus = AtOManager.Instance.GetNgPlus();
		if (!isHero)
		{
			switch (ngPlus)
			{
			case 2:
				if (AtOManager.Instance.GetTownTier() == 0)
				{
					num--;
				}
				else if (AtOManager.Instance.GetTownTier() == 1)
				{
					num--;
				}
				else if (AtOManager.Instance.GetTownTier() == 2)
				{
					num--;
				}
				else if (AtOManager.Instance.GetTownTier() == 3)
				{
					num--;
				}
				break;
			case 3:
				if (AtOManager.Instance.GetTownTier() == 0)
				{
					num--;
				}
				else if (AtOManager.Instance.GetTownTier() == 1)
				{
					num--;
				}
				else if (AtOManager.Instance.GetTownTier() == 2)
				{
					num -= 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 3)
				{
					num -= 2;
				}
				break;
			case 4:
				if (AtOManager.Instance.GetTownTier() == 0)
				{
					num--;
				}
				else if (AtOManager.Instance.GetTownTier() == 1)
				{
					num -= 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 2)
				{
					num -= 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 3)
				{
					num -= 2;
				}
				break;
			case 5:
				if (AtOManager.Instance.GetTownTier() == 0)
				{
					num -= 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 1)
				{
					num -= 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 2)
				{
					num -= 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 3)
				{
					num -= 2;
				}
				break;
			case 6:
				if (AtOManager.Instance.GetTownTier() == 0)
				{
					num -= 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 1)
				{
					num -= 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 2)
				{
					num -= 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 3)
				{
					num -= 3;
				}
				break;
			case 7:
				if (AtOManager.Instance.GetTownTier() == 0)
				{
					num -= 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 1)
				{
					num -= 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 2)
				{
					num -= 3;
				}
				else if (AtOManager.Instance.GetTownTier() == 3)
				{
					num -= 3;
				}
				break;
			case 8:
				if (AtOManager.Instance.GetTownTier() == 0)
				{
					num -= 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 1)
				{
					num -= 3;
				}
				else if (AtOManager.Instance.GetTownTier() == 2)
				{
					num -= 3;
				}
				else if (AtOManager.Instance.GetTownTier() == 3)
				{
					num -= 3;
				}
				break;
			case 9:
				if (AtOManager.Instance.GetTownTier() == 0)
				{
					num -= 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 1)
				{
					num -= 3;
				}
				else if (AtOManager.Instance.GetTownTier() == 2)
				{
					num -= 3;
				}
				else if (AtOManager.Instance.GetTownTier() == 3)
				{
					num -= 4;
				}
				break;
			}
			if (AtOManager.Instance.IsChallengeTraitActive("ironcladmonsters"))
			{
				num -= 3;
			}
		}
		return num;
	}

	public Dictionary<string, int> GetAuraDamageTakenDictionary(Enums.DamageType damageType)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		int num = 0;
		for (int i = 0; i < auraList.Count; i++)
		{
			Aura aura = auraList[i];
			num = 0;
			if (aura.ACData.IncreasedDamageReceivedType == damageType || aura.ACData.IncreasedDamageReceivedType == Enums.DamageType.All)
			{
				float num2 = 1f / (float)aura.ACData.IncreasedDirectDamageChargesMultiplierNeededForOne;
				int num3 = 0;
				num3 = ((!(aura.ACData.IncreasedDirectDamageReceivedPerStack < 0f)) ? Mathf.FloorToInt(num2 * (float)aura.AuraCharges * aura.ACData.IncreasedDirectDamageReceivedPerStack) : (-1 * Mathf.FloorToInt(Mathf.Abs(num2 * (float)aura.AuraCharges * aura.ACData.IncreasedDirectDamageReceivedPerStack))));
				num += num3;
			}
			if (aura.ACData.IncreasedDamageReceivedType2 == damageType)
			{
				float num4 = 1f / (float)aura.ACData.IncreasedDirectDamageChargesMultiplierNeededForOne2;
				int num5 = 0;
				num5 = Functions.FuncRoundToInt(num4 * (float)aura.AuraCharges * aura.ACData.IncreasedDirectDamageReceivedPerStack2);
				num += num5;
			}
			if (num != 0)
			{
				dictionary.Add(aura.ACData.Id, num);
			}
		}
		return dictionary;
	}

	public float[] DamageBonus(Enums.DamageType DT, int energyCost = 0)
	{
		int num = 0;
		int num2 = 0;
		float[] array = new float[2];
		if (false)
		{
			array[0] = num;
			array[1] = num2;
			return array;
		}
		num = 0;
		num2 = 0;
		for (int i = 0; i < auraList.Count; i++)
		{
			if (i >= auraList.Count || auraList[i] == null)
			{
				continue;
			}
			AuraCurseData aCData = auraList[i].ACData;
			if (!(aCData != null))
			{
				continue;
			}
			int auraCharges = auraList[i].AuraCharges;
			if (aCData.AuraDamageType == DT || aCData.AuraDamageType == Enums.DamageType.All)
			{
				if (aCData.AuraDamageChargesBasedOnACCharges != null)
				{
					auraCharges = GetAuraCharges(aCData.AuraDamageChargesBasedOnACCharges.Id);
				}
				if (aCData.AuraDamageIncreasedTotal != 0)
				{
					num += aCData.AuraDamageIncreasedTotal;
				}
				num += Functions.FuncRoundToInt(aCData.AuraDamageIncreasedPerStack * (float)auraCharges);
				if (aCData.AuraDamageIncreasedPercent != 0)
				{
					num2 += aCData.AuraDamageIncreasedPercent;
				}
				if (aCData.AuraDamageIncreasedPercentPerStack != 0f)
				{
					float auraDamageIncreasedPercentPerStack = aCData.AuraDamageIncreasedPercentPerStack;
					auraDamageIncreasedPercentPerStack += aCData.AuraDamageIncreasedPercentPerStackPerEnergy * (float)energyCost;
					num2 += Functions.FuncRoundToInt(auraDamageIncreasedPercentPerStack * (float)auraCharges);
				}
			}
			auraCharges = auraList[i].AuraCharges;
			if (aCData.AuraDamageType2 == DT || aCData.AuraDamageType2 == Enums.DamageType.All)
			{
				if (aCData.AuraDamageIncreasedTotal2 != 0)
				{
					num += aCData.AuraDamageIncreasedTotal2;
				}
				num += Functions.FuncRoundToInt(aCData.AuraDamageIncreasedPerStack2 * (float)auraCharges);
				if (aCData.AuraDamageIncreasedPercent2 != 0)
				{
					num2 += aCData.AuraDamageIncreasedPercent2;
				}
				if (aCData.AuraDamageIncreasedPercentPerStack2 != 0f)
				{
					float auraDamageIncreasedPercentPerStack2 = aCData.AuraDamageIncreasedPercentPerStack2;
					auraDamageIncreasedPercentPerStack2 += aCData.AuraDamageIncreasedPercentPerStackPerEnergy2 * (float)energyCost;
					num2 += Functions.FuncRoundToInt(auraDamageIncreasedPercentPerStack2 * (float)auraCharges);
				}
			}
			if (aCData.AuraDamageType3 == DT || aCData.AuraDamageType3 == Enums.DamageType.All)
			{
				if (aCData.AuraDamageIncreasedTotal3 != 0)
				{
					num += aCData.AuraDamageIncreasedTotal3;
				}
				num += Functions.FuncRoundToInt(aCData.AuraDamageIncreasedPerStack3 * (float)auraCharges);
				if (aCData.AuraDamageIncreasedPercent3 != 0)
				{
					num2 += aCData.AuraDamageIncreasedPercent3;
				}
				if (aCData.AuraDamageIncreasedPercentPerStack3 != 0f)
				{
					float auraDamageIncreasedPercentPerStack3 = aCData.AuraDamageIncreasedPercentPerStack3;
					auraDamageIncreasedPercentPerStack3 += aCData.AuraDamageIncreasedPercentPerStackPerEnergy3 * (float)energyCost;
					num2 += Functions.FuncRoundToInt(auraDamageIncreasedPercentPerStack3 * (float)auraCharges);
				}
			}
			if (aCData.AuraDamageType4 == DT || aCData.AuraDamageType4 == Enums.DamageType.All)
			{
				if (aCData.AuraDamageIncreasedTotal4 != 0)
				{
					num += aCData.AuraDamageIncreasedTotal4;
				}
				num += Functions.FuncRoundToInt(aCData.AuraDamageIncreasedPerStack4 * (float)auraCharges);
				if (aCData.AuraDamageIncreasedPercent4 != 0)
				{
					num2 += aCData.AuraDamageIncreasedPercent4;
				}
				if (aCData.AuraDamageIncreasedPercentPerStack4 != 0f)
				{
					float auraDamageIncreasedPercentPerStack4 = aCData.AuraDamageIncreasedPercentPerStack4;
					auraDamageIncreasedPercentPerStack4 += aCData.AuraDamageIncreasedPercentPerStackPerEnergy4 * (float)energyCost;
					num2 += Functions.FuncRoundToInt(auraDamageIncreasedPercentPerStack4 * (float)auraCharges);
				}
			}
			for (int j = 0; j < aCData.AuraDamageConditionalBonuses.Length; j++)
			{
				if ((aCData.AuraDamageConditionalBonuses[j].AuraDamageType == DT || aCData.AuraDamageConditionalBonuses[j].AuraDamageType == Enums.DamageType.All) && aCData.AuraDamageConditionalBonuses[j].AuraDamageBasedOnAC != null && GetAuraCharges(aCData.AuraDamageConditionalBonuses[j].AuraDamageBasedOnAC.Id) > 0)
				{
					num += aCData.AuraDamageConditionalBonuses[j].AuraDamageIncreasedTotal;
					num2 += aCData.AuraDamageConditionalBonuses[j].AuraDamageIncreasedPercent;
					num += Functions.FuncRoundToInt(aCData.AuraDamageConditionalBonuses[j].AuraDamageIncreasedPerStack * (float)auraCharges);
					if (aCData.AuraDamageConditionalBonuses[j].AuraDamageIncreasedPercentPerStack != 0f)
					{
						float auraDamageIncreasedPercentPerStack5 = aCData.AuraDamageConditionalBonuses[j].AuraDamageIncreasedPercentPerStack;
						auraDamageIncreasedPercentPerStack5 += aCData.AuraDamageConditionalBonuses[j].AuraDamageIncreasedPercentPerStackPerEnergy * (float)energyCost;
						num2 += Functions.FuncRoundToInt(auraDamageIncreasedPercentPerStack5 * (float)auraCharges);
					}
				}
			}
		}
		if (num2 < -50)
		{
			num2 = -50;
		}
		array[0] = num;
		array[1] = num2;
		return array;
	}

	public Dictionary<string, int> GetAuraDamageDoneDictionary(Enums.DamageType DamageType)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		for (int i = 0; i < auraList.Count; i++)
		{
			AuraCurseData aCData = auraList[i].ACData;
			int auraCharges = auraList[i].AuraCharges;
			int num = 0;
			if (aCData.AuraDamageType == DamageType || aCData.AuraDamageType == Enums.DamageType.All)
			{
				if (aCData.AuraDamageIncreasedTotal != 0)
				{
					num += aCData.AuraDamageIncreasedTotal;
				}
				num += Functions.FuncRoundToInt(aCData.AuraDamageIncreasedPerStack * (float)auraCharges);
			}
			if (aCData.AuraDamageType2 == DamageType || aCData.AuraDamageType2 == Enums.DamageType.All)
			{
				if (aCData.AuraDamageIncreasedTotal2 != 0)
				{
					num += aCData.AuraDamageIncreasedTotal2;
				}
				num += Functions.FuncRoundToInt(aCData.AuraDamageIncreasedPerStack2 * (float)auraCharges);
			}
			if (aCData.AuraDamageType3 == DamageType || aCData.AuraDamageType3 == Enums.DamageType.All)
			{
				if (aCData.AuraDamageIncreasedTotal3 != 0)
				{
					num += aCData.AuraDamageIncreasedTotal3;
				}
				num += Functions.FuncRoundToInt(aCData.AuraDamageIncreasedPerStack3 * (float)auraCharges);
			}
			if (aCData.AuraDamageType4 == DamageType || aCData.AuraDamageType4 == Enums.DamageType.All)
			{
				if (aCData.AuraDamageIncreasedTotal4 != 0)
				{
					num += aCData.AuraDamageIncreasedTotal4;
				}
				num += Functions.FuncRoundToInt(aCData.AuraDamageIncreasedPerStack4 * (float)auraCharges);
			}
			for (int j = 0; j < aCData.AuraDamageConditionalBonuses.Length; j++)
			{
				if ((aCData.AuraDamageConditionalBonuses[j].AuraDamageType == DamageType || aCData.AuraDamageConditionalBonuses[j].AuraDamageType == Enums.DamageType.All) && aCData.AuraDamageConditionalBonuses[j].AuraDamageBasedOnAC != null && GetAuraCharges(aCData.AuraDamageConditionalBonuses[j].AuraDamageBasedOnAC.Id) > 0)
				{
					num += aCData.AuraDamageConditionalBonuses[j].AuraDamageIncreasedTotal;
					num += Functions.FuncRoundToInt(aCData.AuraDamageConditionalBonuses[j].AuraDamageIncreasedPerStack * (float)auraCharges);
				}
			}
			if (num != 0)
			{
				dictionary.Add(aCData.Id, num);
			}
		}
		return dictionary;
	}

	public Dictionary<string, int> GetAuraDamageDonePercentDictionary(Enums.DamageType DamageType)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		for (int i = 0; i < auraList.Count; i++)
		{
			if (auraList[i] == null)
			{
				continue;
			}
			AuraCurseData aCData = auraList[i].ACData;
			if (!(aCData != null))
			{
				continue;
			}
			int auraCharges = auraList[i].AuraCharges;
			int num = 0;
			if (aCData.AuraDamageType == DamageType || aCData.AuraDamageType == Enums.DamageType.All)
			{
				if (aCData.AuraDamageChargesBasedOnACCharges != null)
				{
					auraCharges = GetAuraCharges(aCData.AuraDamageChargesBasedOnACCharges.Id);
				}
				if (aCData.AuraDamageIncreasedPercent != 0)
				{
					num += aCData.AuraDamageIncreasedPercent;
				}
				if (aCData.AuraDamageIncreasedPercentPerStack != 0f)
				{
					num += Functions.FuncRoundToInt(aCData.AuraDamageIncreasedPercentPerStack * (float)auraCharges);
				}
			}
			auraCharges = auraList[i].AuraCharges;
			if (aCData.AuraDamageType2 == DamageType || aCData.AuraDamageType2 == Enums.DamageType.All)
			{
				if (aCData.AuraDamageIncreasedPercent2 != 0)
				{
					num += aCData.AuraDamageIncreasedPercent2;
				}
				if (aCData.AuraDamageIncreasedPercentPerStack2 != 0f)
				{
					num += Functions.FuncRoundToInt(aCData.AuraDamageIncreasedPercentPerStack2 * (float)auraCharges);
				}
			}
			if (aCData.AuraDamageType3 == DamageType || aCData.AuraDamageType3 == Enums.DamageType.All)
			{
				if (aCData.AuraDamageIncreasedPercent3 != 0)
				{
					num += aCData.AuraDamageIncreasedPercent3;
				}
				if (aCData.AuraDamageIncreasedPercentPerStack3 != 0f)
				{
					num += Functions.FuncRoundToInt(aCData.AuraDamageIncreasedPercentPerStack3 * (float)auraCharges);
				}
			}
			if (aCData.AuraDamageType4 == DamageType || aCData.AuraDamageType4 == Enums.DamageType.All)
			{
				if (aCData.AuraDamageIncreasedPercent4 != 0)
				{
					num += aCData.AuraDamageIncreasedPercent4;
				}
				if (aCData.AuraDamageIncreasedPercentPerStack4 != 0f)
				{
					num += Functions.FuncRoundToInt(aCData.AuraDamageIncreasedPercentPerStack4 * (float)auraCharges);
				}
			}
			for (int j = 0; j < aCData.AuraDamageConditionalBonuses.Length; j++)
			{
				if ((aCData.AuraDamageConditionalBonuses[j].AuraDamageType == DamageType || aCData.AuraDamageConditionalBonuses[j].AuraDamageType == Enums.DamageType.All) && aCData.AuraDamageConditionalBonuses[j].AuraDamageBasedOnAC != null && GetAuraCharges(aCData.AuraDamageConditionalBonuses[j].AuraDamageBasedOnAC.Id) > 0)
				{
					num += aCData.AuraDamageConditionalBonuses[j].AuraDamageIncreasedPercent;
					num += Functions.FuncRoundToInt(aCData.AuraDamageConditionalBonuses[j].AuraDamageIncreasedPercentPerStack * (float)auraCharges);
				}
			}
			if (num != 0)
			{
				dictionary.Add(aCData.Id, num);
			}
		}
		return dictionary;
	}

	private int GetTraitDamageFlatModifiers(Enums.DamageType DamageType)
	{
		if ((bool)MatchManager.Instance && useCache && cacheGetTraitDamageFlatModifiers.ContainsKey(DamageType))
		{
			return cacheGetTraitDamageFlatModifiers[DamageType];
		}
		int num = 0;
		if (traits != null)
		{
			for (int i = 0; i < traits.Length; i++)
			{
				if (traits[i] == null)
				{
					continue;
				}
				TraitData traitData = Globals.Instance.GetTraitData(traits[i]);
				if (traitData != null)
				{
					if (traitData.DamageBonusFlat == DamageType || traitData.DamageBonusFlat == Enums.DamageType.All)
					{
						num += traitData.DamageBonusFlatValue;
					}
					if (traitData.DamageBonusFlat2 == DamageType || traitData.DamageBonusFlat2 == Enums.DamageType.All)
					{
						num += traitData.DamageBonusFlatValue2;
					}
					if (traitData.DamageBonusFlat3 == DamageType || traitData.DamageBonusFlat3 == Enums.DamageType.All)
					{
						num += traitData.DamageBonusFlatValue3;
					}
				}
			}
		}
		if ((bool)MatchManager.Instance && useCache)
		{
			if (!cacheGetTraitDamageFlatModifiers.ContainsKey(DamageType))
			{
				cacheGetTraitDamageFlatModifiers.Add(DamageType, num);
			}
			else
			{
				cacheGetTraitDamageFlatModifiers[DamageType] = num;
			}
		}
		return num;
	}

	public float GetTraitDamagePercentModifiers(Enums.DamageType DamageType)
	{
		if ((bool)MatchManager.Instance && useCache && cacheGetTraitDamagePercentModifiers.ContainsKey(DamageType))
		{
			return cacheGetTraitDamagePercentModifiers[DamageType];
		}
		bool flag = true;
		float num = 0f;
		if (traits != null)
		{
			for (int i = 0; i < traits.Length; i++)
			{
				if (traits[i] == null)
				{
					continue;
				}
				TraitData traitData = Globals.Instance.GetTraitData(traits[i]);
				if (!(traitData != null))
				{
					continue;
				}
				if (traitData.DamageBonusPercent == DamageType || traitData.DamageBonusPercent == Enums.DamageType.All)
				{
					num += traitData.DamageBonusPercentValue;
				}
				if (traitData.DamageBonusPercent2 == DamageType || traitData.DamageBonusPercent2 == Enums.DamageType.All)
				{
					num += traitData.DamageBonusPercentValue2;
				}
				if (traitData.DamageBonusPercent3 == DamageType || traitData.DamageBonusPercent3 == Enums.DamageType.All)
				{
					num += traitData.DamageBonusPercentValue3;
				}
				if (traitData.Id == "bigbadwolf")
				{
					flag = false;
					if (hpCurrent > 100)
					{
						float num2 = hpCurrent - 100;
						if (num2 > 0f)
						{
							num += (float)Functions.FuncRoundToInt(num2 / 3f);
						}
					}
				}
				else
				{
					if (!(traitData.Id == "hatred"))
					{
						continue;
					}
					flag = false;
					if (GetHpPercent() < 50f)
					{
						float num3 = 50f - GetHpPercent();
						float num4 = 30f;
						float num5 = 4f;
						if (HaveTrait("unrelentingresentment"))
						{
							num4 = 60f;
							num5 = 6f;
						}
						num += num4 + num3 * num5;
					}
				}
			}
		}
		if (flag && (bool)MatchManager.Instance && useCache)
		{
			if (!cacheGetTraitDamagePercentModifiers.ContainsKey(DamageType))
			{
				cacheGetTraitDamagePercentModifiers.Add(DamageType, num);
			}
			else
			{
				cacheGetTraitDamagePercentModifiers[DamageType] = num;
			}
		}
		return num;
	}

	public int DamageWithCharacterBonus(int value, Enums.DamageType DT, Enums.CardClass CC, int energyCost = 0, int additionalDamage = -1000)
	{
		if (isHero && (CC == Enums.CardClass.Monster || CC == Enums.CardClass.Injury || CC == Enums.CardClass.Boon))
		{
			return value;
		}
		int num = value;
		if (additionalDamage != -1000)
		{
			num += additionalDamage;
		}
		int num2 = 0;
		num += num2;
		float[] array = DamageBonus(DT, energyCost);
		float num3 = (float)(num + GetTraitDamageFlatModifiers(DT) + GetItemDamageFlatModifiers(DT)) + array[0];
		if (heroData != null)
		{
			num3 += (float)PlayerManager.Instance.GetPerkDamageBonus(heroData.HeroSubClass.Id, DT);
		}
		int ngPlus = AtOManager.Instance.GetNgPlus();
		if (!isHero)
		{
			switch (ngPlus)
			{
			case 1:
				if (AtOManager.Instance.GetTownTier() == 0)
				{
					num3 -= 1f;
				}
				else if (AtOManager.Instance.GetTownTier() == 1)
				{
					num3 -= 1f;
				}
				break;
			case 3:
				if (AtOManager.Instance.GetTownTier() == 0)
				{
					num3 += 1f;
				}
				else if (AtOManager.Instance.GetTownTier() == 1)
				{
					num3 += 1f;
				}
				else if (AtOManager.Instance.GetTownTier() == 2)
				{
					num3 += 1f;
				}
				else if (AtOManager.Instance.GetTownTier() == 3)
				{
					num3 += 1f;
				}
				break;
			case 4:
				if (AtOManager.Instance.GetTownTier() == 0)
				{
					num3 += 1f;
				}
				else if (AtOManager.Instance.GetTownTier() == 1)
				{
					num3 += 1f;
				}
				else if (AtOManager.Instance.GetTownTier() == 2)
				{
					num3 += 2f;
				}
				else if (AtOManager.Instance.GetTownTier() == 3)
				{
					num3 += 2f;
				}
				break;
			case 5:
				if (AtOManager.Instance.GetTownTier() == 0)
				{
					num3 += 1f;
				}
				else if (AtOManager.Instance.GetTownTier() == 1)
				{
					num3 += 2f;
				}
				else if (AtOManager.Instance.GetTownTier() == 2)
				{
					num3 += 2f;
				}
				else if (AtOManager.Instance.GetTownTier() == 3)
				{
					num3 += 2f;
				}
				break;
			case 6:
				if (AtOManager.Instance.GetTownTier() == 0)
				{
					num3 += 2f;
				}
				else if (AtOManager.Instance.GetTownTier() == 1)
				{
					num3 += 2f;
				}
				else if (AtOManager.Instance.GetTownTier() == 2)
				{
					num3 += 2f;
				}
				else if (AtOManager.Instance.GetTownTier() == 3)
				{
					num3 += 2f;
				}
				break;
			case 7:
				if (AtOManager.Instance.GetTownTier() == 0)
				{
					num3 += 2f;
				}
				else if (AtOManager.Instance.GetTownTier() == 1)
				{
					num3 += 2f;
				}
				else if (AtOManager.Instance.GetTownTier() == 2)
				{
					num3 += 2f;
				}
				else if (AtOManager.Instance.GetTownTier() == 3)
				{
					num3 += 3f;
				}
				break;
			case 8:
				if (AtOManager.Instance.GetTownTier() == 0)
				{
					num3 += 2f;
				}
				else if (AtOManager.Instance.GetTownTier() == 1)
				{
					num3 += 2f;
				}
				else if (AtOManager.Instance.GetTownTier() == 2)
				{
					num3 += 3f;
				}
				else if (AtOManager.Instance.GetTownTier() == 3)
				{
					num3 += 3f;
				}
				break;
			case 9:
				if (AtOManager.Instance.GetTownTier() == 0)
				{
					num3 += 2f;
				}
				else if (AtOManager.Instance.GetTownTier() == 1)
				{
					num3 += 3f;
				}
				else if (AtOManager.Instance.GetTownTier() == 2)
				{
					num3 += 3f;
				}
				else if (AtOManager.Instance.GetTownTier() == 3)
				{
					num3 += 4f;
				}
				break;
			}
			if (AtOManager.Instance.IsChallengeTraitActive("dangerousmonsters"))
			{
				num3 += 3f;
			}
		}
		float num4 = array[1] + GetTraitDamagePercentModifiers(DT) + GetItemDamagePercentModifiers(DT);
		num = Functions.FuncRoundToInt(num3 + num3 * num4 * 0.01f);
		if (!isHero && AtOManager.Instance.Sandbox_additionalMonsterDamage != 0)
		{
			num += Functions.FuncRoundToInt((float)(num * AtOManager.Instance.Sandbox_additionalMonsterDamage) * 0.01f);
		}
		return num;
	}

	public int TotalDamageWithCharacterFlatBonus(Enums.DamageType DT)
	{
		float num = (float)(GetTraitDamageFlatModifiers(DT) + GetItemDamageFlatModifiers(DT)) + DamageBonus(DT)[0];
		if (heroData != null)
		{
			num += (float)PlayerManager.Instance.GetPerkDamageBonus(heroData.HeroSubClass.Id, DT);
		}
		if (!isHero)
		{
			int ngPlus = AtOManager.Instance.GetNgPlus();
			int num2 = 0;
			switch (ngPlus)
			{
			case 3:
				if (AtOManager.Instance.GetTownTier() == 0)
				{
					num2++;
				}
				else if (AtOManager.Instance.GetTownTier() == 1)
				{
					num2++;
				}
				else if (AtOManager.Instance.GetTownTier() == 2)
				{
					num2++;
				}
				else if (AtOManager.Instance.GetTownTier() == 3)
				{
					num2++;
				}
				break;
			case 4:
				if (AtOManager.Instance.GetTownTier() == 0)
				{
					num2++;
				}
				else if (AtOManager.Instance.GetTownTier() == 1)
				{
					num2++;
				}
				else if (AtOManager.Instance.GetTownTier() == 2)
				{
					num2 += 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 3)
				{
					num2 += 2;
				}
				break;
			case 5:
				if (AtOManager.Instance.GetTownTier() == 0)
				{
					num2++;
				}
				else if (AtOManager.Instance.GetTownTier() == 1)
				{
					num2 += 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 2)
				{
					num2 += 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 3)
				{
					num2 += 2;
				}
				break;
			case 6:
				if (AtOManager.Instance.GetTownTier() == 0)
				{
					num2 += 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 1)
				{
					num2 += 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 2)
				{
					num2 += 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 3)
				{
					num2 += 2;
				}
				break;
			case 7:
				if (AtOManager.Instance.GetTownTier() == 0)
				{
					num2 += 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 1)
				{
					num2 += 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 2)
				{
					num2 += 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 3)
				{
					num2 += 3;
				}
				break;
			case 8:
				if (AtOManager.Instance.GetTownTier() == 0)
				{
					num2 += 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 1)
				{
					num2 += 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 2)
				{
					num2 += 3;
				}
				else if (AtOManager.Instance.GetTownTier() == 3)
				{
					num2 += 3;
				}
				break;
			case 9:
				if (AtOManager.Instance.GetTownTier() == 0)
				{
					num2 += 2;
				}
				else if (AtOManager.Instance.GetTownTier() == 1)
				{
					num2 += 3;
				}
				else if (AtOManager.Instance.GetTownTier() == 2)
				{
					num2 += 3;
				}
				else if (AtOManager.Instance.GetTownTier() == 3)
				{
					num2 += 4;
				}
				break;
			}
			num += (float)num2;
		}
		return Functions.FuncRoundToInt(num);
	}

	public float[] HealReceivedBonus(bool isIndirect = false, CardData cardAux = null)
	{
		int num = 0;
		int num2 = 0;
		float[] array = new float[2];
		num = 0;
		num2 = 0;
		if (isHero && MadnessManager.Instance.IsMadnessTraitActive("decadence"))
		{
			num2 -= 25;
		}
		if (isHero && AtOManager.Instance.IsChallengeTraitActive("decadence"))
		{
			num2 -= 20;
		}
		for (int i = 0; i < auraList.Count; i++)
		{
			if (auraList[i] == null || !(auraList[i].ACData != null))
			{
				continue;
			}
			AuraCurseData aCData = auraList[i].ACData;
			if (!(cardAux != null) || !cardAux.IsGoingToPurgeThisAC(aCData.Id))
			{
				int auraCharges = auraList[i].AuraCharges;
				if (aCData.HealReceivedTotal != 0)
				{
					num += aCData.HealReceivedTotal;
				}
				if (aCData.HealReceivedPerStack != 0)
				{
					num += aCData.HealReceivedPerStack * auraCharges;
				}
				if (aCData.HealReceivedPercent != 0)
				{
					num2 += aCData.HealReceivedPercent;
				}
				if (aCData.HealReceivedPercentPerStack != 0)
				{
					num2 += aCData.HealReceivedPercentPerStack * auraCharges;
				}
				if (isIndirect && aCData.Id == "zeal")
				{
					num2 = ((!AtOManager.Instance.CharacterHavePerk(subclassName, "mainperkzeal0c")) ? (num2 + 20 * auraCharges) : (num2 + 12 * auraCharges));
				}
			}
		}
		array[0] = num;
		array[1] = num2;
		return array;
	}

	public int HealReceivedFinal(int heal, bool isIndirect = false, CardData cardAux = null)
	{
		int num = heal;
		float[] array = HealReceivedBonus(isIndirect, cardAux);
		float num2 = (float)num + array[0] + (float)GetTraitHealReceivedFlatBonus() + (float)GetItemHealReceivedFlatBonus();
		float num3 = array[1] + GetTraitHealReceivedPercentBonus() + GetItemHealReceivedPercentBonus();
		num = Functions.FuncRoundToInt(num2 + num2 * num3 * 0.01f);
		if (num < 0)
		{
			num = 0;
		}
		return num;
	}

	public float[] HealBonus(int energyCost)
	{
		int num = 0;
		int num2 = 0;
		float[] array = new float[2];
		for (int i = 0; i < auraList.Count; i++)
		{
			if (auraList[i] == null)
			{
				continue;
			}
			AuraCurseData aCData = auraList[i].ACData;
			if (aCData != null)
			{
				int auraCharges = auraList[i].AuraCharges;
				if (aCData.HealDoneTotal != 0)
				{
					num += aCData.HealDoneTotal;
				}
				if (aCData.HealDonePerStack != 0)
				{
					num += aCData.HealDonePerStack * auraCharges;
				}
				if (aCData.HealDonePercent != 0)
				{
					num2 += aCData.HealDonePercent;
				}
				if (aCData.HealDonePercentPerStack != 0)
				{
					float num3 = aCData.HealDonePercentPerStack;
					num3 += (float)(aCData.HealDonePercentPerStackPerEnergy * energyCost);
					num2 += Functions.FuncRoundToInt(num3 * (float)auraCharges);
				}
			}
		}
		if (isHero && MadnessManager.Instance.IsMadnessTraitActive("decadence"))
		{
			num -= 3;
		}
		if (!isHero && AtOManager.Instance.NodeIsObeliskFinal())
		{
			num -= 4;
		}
		array[0] = num;
		array[1] = num2;
		return array;
	}

	public Dictionary<string, int> GetAuraHealPercentDictionary()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < auraList.Count; i++)
		{
			AuraCurseData aCData = auraList[i].ACData;
			num = 0;
			num2 = auraList[i].AuraCharges;
			if (aCData.HealDonePercent != 0)
			{
				num += aCData.HealDonePercent;
			}
			if (aCData.HealDonePercentPerStack != 0)
			{
				num += aCData.HealDonePercentPerStack * num2;
			}
			if (num != 0)
			{
				dictionary.Add(aCData.Id, num);
			}
		}
		return dictionary;
	}

	public Dictionary<string, int> GetAuraHealFlatDictionary()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < auraList.Count; i++)
		{
			AuraCurseData aCData = auraList[i].ACData;
			num = 0;
			num2 = auraList[i].AuraCharges;
			if (aCData.HealDoneTotal != 0)
			{
				num += aCData.HealDoneTotal;
			}
			if (aCData.HealDonePerStack != 0)
			{
				num += aCData.HealDonePerStack * num2;
			}
			if (num != 0)
			{
				dictionary.Add(aCData.Id, num);
			}
		}
		return dictionary;
	}

	public Dictionary<string, int> GetAuraHealReceivedPercentDictionary()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < auraList.Count; i++)
		{
			AuraCurseData aCData = auraList[i].ACData;
			num = 0;
			num2 = auraList[i].AuraCharges;
			if (aCData.HealDonePercent != 0)
			{
				num += aCData.HealReceivedPercent;
			}
			if (aCData.HealDonePercentPerStack != 0)
			{
				num += aCData.HealReceivedPercentPerStack * num2;
			}
			if (num != 0)
			{
				dictionary.Add(aCData.Id, num);
			}
		}
		return dictionary;
	}

	public Dictionary<string, int> GetAuraHealReceivedFlatDictionary()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < auraList.Count; i++)
		{
			AuraCurseData aCData = auraList[i].ACData;
			num = 0;
			num2 = auraList[i].AuraCharges;
			if (aCData.HealDoneTotal != 0)
			{
				num += aCData.HealReceivedTotal;
			}
			if (aCData.HealDonePerStack != 0)
			{
				num += aCData.HealReceivedPerStack * num2;
			}
			if (num != 0)
			{
				dictionary.Add(aCData.Id, num);
			}
		}
		return dictionary;
	}

	public int HealWithCharacterBonus(int heal, Enums.CardClass CC, int energyCost = 0)
	{
		if (isHero && (CC == Enums.CardClass.Monster || CC == Enums.CardClass.Injury || CC == Enums.CardClass.Boon))
		{
			return heal;
		}
		int num = 0;
		int num2 = heal + num;
		float[] array = HealBonus(energyCost);
		float num3 = (float)num2 + array[0] + (float)GetTraitHealFlatBonus() + (float)GetItemHealFlatBonus();
		float num4 = array[1] + GetTraitHealPercentBonus() + GetItemHealPercentBonus();
		if (heroData != null)
		{
			num3 += (float)PlayerManager.Instance.GetPerkHealBonus(heroData.HeroSubClass.Id);
		}
		return Functions.FuncRoundToInt(num3 + num3 * num4 * 0.01f);
	}

	public int GetTraitHealFlatBonus()
	{
		if ((bool)MatchManager.Instance && useCache && cacheGetTraitHealFlatBonus.Count > 0)
		{
			return cacheGetTraitHealFlatBonus[0];
		}
		int num = 0;
		if (traits != null)
		{
			for (int i = 0; i < traits.Length; i++)
			{
				if (traits[i] != null)
				{
					TraitData traitData = Globals.Instance.GetTraitData(traits[i]);
					if (traitData != null && traitData.HealFlatBonus != 0)
					{
						num += traitData.HealFlatBonus;
					}
				}
			}
		}
		if ((bool)MatchManager.Instance && useCache)
		{
			cacheGetTraitHealFlatBonus.Add(num);
		}
		return num;
	}

	public float GetTraitHealPercentBonus()
	{
		if ((bool)MatchManager.Instance && useCache && cacheGetTraitHealPercentBonus.Count > 0)
		{
			return cacheGetTraitHealPercentBonus[0];
		}
		float num = 0f;
		if (traits != null)
		{
			for (int i = 0; i < traits.Length; i++)
			{
				if (traits[i] != null)
				{
					TraitData traitData = Globals.Instance.GetTraitData(traits[i]);
					if (traitData != null && traitData.HealPercentBonus != 0f)
					{
						num += traitData.HealPercentBonus;
					}
				}
			}
		}
		if ((bool)MatchManager.Instance && useCache)
		{
			cacheGetTraitHealPercentBonus.Add(num);
		}
		return num;
	}

	public int GetTraitHealReceivedFlatBonus()
	{
		if ((bool)MatchManager.Instance && useCache && cacheGetTraitHealReceivedFlatBonus.Count > 0)
		{
			return cacheGetTraitHealReceivedFlatBonus[0];
		}
		int num = 0;
		if (traits != null)
		{
			for (int i = 0; i < traits.Length; i++)
			{
				if (traits[i] != null)
				{
					TraitData traitData = Globals.Instance.GetTraitData(traits[i]);
					if (traitData != null && traitData.HealReceivedFlatBonus != 0)
					{
						num += traitData.HealReceivedFlatBonus;
					}
				}
			}
		}
		if ((bool)MatchManager.Instance && useCache)
		{
			cacheGetTraitHealReceivedFlatBonus.Add(num);
		}
		return num;
	}

	public float GetTraitHealReceivedPercentBonus()
	{
		if ((bool)MatchManager.Instance && useCache && cacheGetTraitHealReceivedPercentBonus.Count > 0)
		{
			return cacheGetTraitHealReceivedPercentBonus[0];
		}
		float num = 0f;
		if (traits != null)
		{
			for (int i = 0; i < traits.Length; i++)
			{
				if (traits[i] != null)
				{
					TraitData traitData = Globals.Instance.GetTraitData(traits[i]);
					if (traitData != null && traitData.HealReceivedPercentBonus != 0f)
					{
						num += traitData.HealReceivedPercentBonus;
					}
				}
			}
		}
		foreach (float item in CacheGetHealRecievedPercentBonusFromAllyTrait)
		{
			num += item;
		}
		if ((bool)MatchManager.Instance && useCache)
		{
			cacheGetTraitHealReceivedPercentBonus.Add(num);
		}
		return num;
	}

	public int GetItemFinalRewardRetentionModification()
	{
		int num = 0;
		for (int i = 0; i < itemSlots; i++)
		{
			ItemData itemData = GetItemDataBySlot(i, useCache: false);
			if (itemData != null && itemData.PercentRetentionEndGame != 0)
			{
				num += itemData.PercentRetentionEndGame;
			}
		}
		return num;
	}

	public int GetItemDiscountModification()
	{
		int num = 0;
		for (int i = 0; i < itemSlots; i++)
		{
			ItemData itemData = GetItemDataBySlot(i, useCache: false);
			if (itemData != null && itemData.PercentDiscountShop != 0)
			{
				num += itemData.PercentDiscountShop;
			}
		}
		return num;
	}

	public void DestroyCharacter()
	{
		if (heroItem != null && heroItem.gameObject != null)
		{
			UnityEngine.Object.Destroy(heroItem.gameObject);
		}
		else if (npcItem != null && npcItem.gameObject != null)
		{
			UnityEngine.Object.Destroy(npcItem.gameObject);
		}
		UnityEngine.Object.Destroy(gO);
		npcItem = null;
		heroItem = null;
		auraList.Clear();
		int num = (IsHero ? HeroIndex : npcIndex);
		MatchManager.Instance.BossNpc?.OnCharacterKilled(NpcData, HeroData, num);
		NPC[] array = MatchManager.Instance?.GetTeamNPC();
		for (int i = 0; i < array.Length; i++)
		{
			array[i]?.UpdateOverDeck();
		}
	}

	public virtual void CreateOverDeck(bool getCardFromDeck, bool maxOneCard = false)
	{
	}

	public void RemoveEnchantsStartTurn()
	{
		ItemData itemData = null;
		for (int i = 0; i < 3; i++)
		{
			switch (i)
			{
			case 0:
				itemData = Globals.Instance.GetItemData(enchantment);
				if (itemData != null && itemData.DestroyStartOfTurn)
				{
					enchantment = "";
				}
				break;
			case 1:
				itemData = Globals.Instance.GetItemData(enchantment2);
				if (itemData != null && itemData.DestroyStartOfTurn)
				{
					enchantment2 = "";
				}
				break;
			case 2:
				itemData = Globals.Instance.GetItemData(enchantment3);
				if (itemData != null && itemData.DestroyStartOfTurn)
				{
					enchantment3 = "";
				}
				break;
			}
		}
	}

	public void RemoveEnchantsEndTurn()
	{
		ItemData itemData = null;
		for (int i = 0; i < 3; i++)
		{
			switch (i)
			{
			case 0:
				itemData = Globals.Instance.GetItemData(enchantment);
				if (itemData != null && itemData.DestroyEndOfTurn)
				{
					enchantment = "";
				}
				break;
			case 1:
				itemData = Globals.Instance.GetItemData(enchantment2);
				if (itemData != null && itemData.DestroyEndOfTurn)
				{
					enchantment2 = "";
				}
				break;
			case 2:
				itemData = Globals.Instance.GetItemData(enchantment3);
				if (itemData != null && itemData.DestroyEndOfTurn)
				{
					enchantment3 = "";
				}
				break;
			}
		}
	}

	public Enums.DamageType GetEnchantModifiedDamageType()
	{
		ItemData itemData = null;
		Enums.DamageType result = Enums.DamageType.None;
		for (int i = 0; i < 3; i++)
		{
			switch (i)
			{
			case 0:
				itemData = Globals.Instance.GetItemData(enchantment);
				break;
			case 1:
				itemData = Globals.Instance.GetItemData(enchantment2);
				break;
			case 2:
				itemData = Globals.Instance.GetItemData(enchantment3);
				break;
			}
			if (itemData != null && itemData.ModifiedDamageType != Enums.DamageType.None)
			{
				result = itemData.ModifiedDamageType;
			}
		}
		return result;
	}

	public Enums.DamageType GetItemModifiedDamageType()
	{
		ItemData itemData = null;
		Enums.DamageType result = Enums.DamageType.None;
		for (int i = 0; i < 4; i++)
		{
			string text = "";
			if (i == 0 && weapon != "")
			{
				text = weapon;
			}
			else if (i == 1 && armor != "")
			{
				text = armor;
			}
			else if (i == 2 && jewelry != "")
			{
				text = jewelry;
			}
			else if (i == 3 && accesory != "")
			{
				text = accesory;
			}
			if (text != "")
			{
				itemData = Globals.Instance.GetItemData(text);
			}
			if (itemData != null && itemData.ModifiedDamageType != Enums.DamageType.None)
			{
				result = itemData.ModifiedDamageType;
			}
		}
		return result;
	}

	public void SetSpellswordSwords()
	{
		if (!(subclassName == "queen") || !(heroItem != null))
		{
			return;
		}
		int auraCharges = GetAuraCharges("spellsword");
		if (auraCharges == spellswordTMP)
		{
			return;
		}
		spellswordTMP = auraCharges;
		heroItem.CleanSwordSprites();
		if (auraCharges < 1)
		{
			return;
		}
		heroItem.ShowHideSwordSprite(0, state: true);
		if (auraCharges < 2)
		{
			return;
		}
		heroItem.ShowHideSwordSprite(4, state: true);
		if (auraCharges < 3)
		{
			return;
		}
		heroItem.ShowHideSwordSprite(2, state: true);
		if (auraCharges >= 4)
		{
			heroItem.ShowHideSwordSprite(1, state: true);
			if (auraCharges >= 5)
			{
				heroItem.ShowHideSwordSprite(3, state: true);
			}
		}
	}

	public virtual void InitData()
	{
	}
}
