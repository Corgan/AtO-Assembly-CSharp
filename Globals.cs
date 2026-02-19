using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Globals : MonoBehaviour
{
	public Sprite pdxLogo;

	public Sprite switchLogo;

	public Sprite psLogo;

	public Sprite xboxLogo;

	private DateTime initialWeeklyDate = new DateTime(2022, 8, 18, 16, 0, 0);

	public DateTime finishHalloweenEvent = new DateTime(2023, 11, 7, 19, 0, 0);

	private bool showDebug;

	private Enums.Platform configPlatform;

	private string currentLang = "en";

	private string defaultNickName = "AtODefaultPlayer";

	private bool saveAurasText;

	private string auraText = "";

	private bool saveClassText;

	private string classText = "";

	private bool saveMonsterText;

	private string monsterText = "";

	private bool saveKeynotesText;

	private string keynotesText = "";

	private bool parseEvents;

	private bool saveEventsText;

	private string eventText = "";

	private Dictionary<string, string> savedEventLines = new Dictionary<string, string>();

	private bool saveRequirementsText;

	private string requirementsText = "";

	private bool saveTraitsText;

	private string traitsText = "";

	private bool saveNodesText;

	private string nodesText = "";

	private bool saveCardsText;

	private string cardsText = "";

	private string cardsFluffText = "";

	private int normalFPS = 120;

	private List<CardData> _InstantiatedCardData;

	private Dictionary<string, string> _IncompatibleVersion;

	private Dictionary<float, WaitForSeconds> _WaitForSecondsDict;

	[SerializeField]
	private Dictionary<string, CardData> _Cards;

	[SerializeField]
	private Dictionary<string, CardData> _CardsSource;

	[SerializeField]
	private Dictionary<string, HeroData> _Heroes;

	[SerializeField]
	private Dictionary<string, NPCData> _NPCs;

	[SerializeField]
	private Dictionary<string, NPCData> _NPCsNamed;

	[SerializeField]
	private Dictionary<string, NPCData> _NPCsSource;

	[SerializeField]
	private Dictionary<string, AuraCurseData> _AurasCurses;

	[SerializeField]
	private Dictionary<string, AuraCurseData> _AurasCursesSource;

	[SerializeField]
	private List<string> _AurasCursesIndex;

	[SerializeField]
	private Dictionary<string, SubClassData> _SubClass;

	[SerializeField]
	private Dictionary<string, SubClassData> _SubClassSource;

	[SerializeField]
	private Dictionary<string, TraitData> _Traits;

	[SerializeField]
	private Dictionary<string, TraitData> _TraitsSource;

	[SerializeField]
	private Dictionary<string, PerkData> _PerksSource;

	[SerializeField]
	private Dictionary<string, PerkNodeData> _PerksNodesSource;

	[SerializeField]
	private Dictionary<string, EventData> _Events;

	[SerializeField]
	private Dictionary<string, CinematicData> _Cinematics;

	[SerializeField]
	private Dictionary<string, EventRequirementData> _Requirements;

	[SerializeField]
	private SortedDictionary<string, KeyNotesData> _KeyNotes;

	[SerializeField]
	private Dictionary<string, CombatData> _CombatDataSource;

	[SerializeField]
	private Dictionary<string, NodeData> _NodeDataSource;

	private Dictionary<string, string> _NodeCombatEventRelation;

	[SerializeField]
	private Dictionary<int, TierRewardData> _TierRewardDataSource;

	[SerializeField]
	private Dictionary<string, ItemData> _ItemDataSource;

	[SerializeField]
	private Dictionary<string, LootData> _LootDataSource;

	[SerializeField]
	private Dictionary<string, PackData> _PackDataSource;

	[SerializeField]
	private Dictionary<string, SkinData> _SkinDataSource;

	[SerializeField]
	private Dictionary<string, CardbackData> _CardbackDataSource;

	[SerializeField]
	private Dictionary<string, CardPlayerPackData> _CardPlayerPackDataSource;

	[SerializeField]
	private Dictionary<string, CardPlayerPairsPackData> _CardPlayerPairsPackDataSource;

	[SerializeField]
	private Dictionary<string, CorruptionPackData> _CorruptionPackDataSource;

	[SerializeField]
	private Dictionary<string, ZoneData> _ZoneDataSource;

	[SerializeField]
	private Dictionary<string, ChallengeTrait> _ChallengeTraitsSource;

	[SerializeField]
	private Dictionary<string, ChallengeData> _WeeklyDataSource;

	[SerializeField]
	private Dictionary<string, int> _UpgradeCost;

	[SerializeField]
	private Dictionary<string, int> _CraftCost;

	[SerializeField]
	private Dictionary<string, int> _DivinationCost;

	[SerializeField]
	private Dictionary<int, int> _DivinationTier;

	[SerializeField]
	private Dictionary<string, int> _ItemCost;

	[SerializeField]
	private Dictionary<string, int> _CardEnergyCost;

	public const string petShopListId = "petShop";

	public const string tutorialSeed = "tuto";

	public const int townTutorialStepCraft = 0;

	public const string tutorialCraftItem = "fireball";

	public const string tutorialCraftCharacterName = "Evelyn";

	public const int townTutorialStepUpgrade = 1;

	public const string tutorialUpgradeItem = "faststrike";

	public const string tutorialUpgradeCharacterName = "Magnus";

	public const int townTutorialStepItem = 2;

	public const string tutorialLootItem = "spyglass";

	public const string tutorialLootCharacterName = "Andrin";

	public const string thePetEnchantmentName = "thePetEnchantment";

	public const int rankNeededForObelisk = 3;

	public const int rankNeededForSingularity = 3;

	public const int upgradeLimitSingularity = 1;

	public const int npcHandicapDamageBonus = 25;

	public const int npcHandicapResistanceBonus = 25;

	public const float sourceWidth = 1920f;

	public const float sourceHeight = 1080f;

	public float sizeW;

	public float sizeH;

	public float multiplierX;

	public float multiplierY;

	public Vector3 scaleV = new Vector3(1f, 1f, 1f);

	public float scale = 1f;

	public const float npcActionDelay = 0.8f;

	public const int teamMaxChars = 4;

	public const int initialCopyCards = 4;

	public const int handCards = 5;

	public const int maxHandCards = 10;

	public const int minDeckCards = 15;

	public const float cardSmallScale = 0.75f;

	public const int maxHeroEnergy = 10;

	public const int initGold = 300;

	public const int initDust = 300;

	public const int initGoldMP = 75;

	public const int initDustMP = 75;

	public const int dmgPercentLowest = -50;

	public const int resistanceLimit = 95;

	public const int globalResurrectPercent = 70;

	public const float InjuryGenerationCardDelay = 4f;

	public const float InitiativePortraitWidth = 0.48f;

	public const float InitiativePortraitDistance = 0.24f;

	public Color MapArrow = new Color(0f, 0.97f, 1f, 0.6f);

	public Color MapArrowHighlight = new Color(0f, 0f, 1f, 0.6f);

	public Color MapArrowTemp = new Color(0f, 0f, 0f, 0.6f);

	public Color MapArrowTempChallenge = new Color(1f, 0.46f, 0.82f, 0.6f);

	public const float effectCastFixedY = 1.4f;

	private Dictionary<string, string> _SkillColor;

	private Dictionary<string, string> _ClassColor;

	private Dictionary<string, Color> _ColorColor;

	private Dictionary<string, Color> _RarityColor;

	public const int HalloweenWeek = 17;

	private int[] _CorruptionBasePercent;

	public const int CorruptionIncrementPercent = 10;

	public const int CorruptionDustCost = 400;

	public const int CorruptionSpeedCost = 2;

	public const int CorruptionLifeCost = 20;

	public const int CorruptionResistCost = 10;

	public const int ScarabChance = 7;

	public const int PetCommonCost = 72;

	public const int PetUncommonCost = 156;

	public const int PetRareCost = 348;

	public const int PetEpicCost = 744;

	public const int PetMythicCost = 1200;

	private Dictionary<string, string> _CardsDescriptionNormalized;

	private Dictionary<Enums.CardType, List<string>> _CardListByType;

	private Dictionary<Enums.CardClass, List<string>> _CardListByClass;

	private Dictionary<Enums.CardClass, List<string>> _CardListNotUpgradedByClass;

	private Dictionary<Enums.CardType, List<string>> _CardItemByType;

	private Dictionary<string, List<string>> _CardsListSearch;

	private Dictionary<string, bool> _CardsListSearched;

	private List<string> _CardListNotUpgraded;

	private Dictionary<string, List<string>> _CardListByClassType;

	private Dictionary<int, int> _ExperienceByCardCost;

	private Dictionary<int, int> _ExperienceByLevel;

	private List<int> _PerkLevel;

	private List<int> _RankLevel;

	private List<string> _SkuAvailable;

	private const int maxPerkPoints = 50;

	private Dictionary<string, string> _IsoToEnglishCountryNames;

	private Dictionary<string, GameObject> vfxAux;

	public static Globals Instance { get; private set; }

	public Dictionary<string, CardData> Cards
	{
		get
		{
			return _Cards;
		}
		set
		{
			_Cards = value;
		}
	}

	public Dictionary<string, HeroData> Heroes
	{
		get
		{
			return _Heroes;
		}
		set
		{
			_Heroes = value;
		}
	}

	public Dictionary<string, NPCData> NPCs => _NPCs;

	public Dictionary<string, string> SkillColor
	{
		get
		{
			return _SkillColor;
		}
		set
		{
			_SkillColor = value;
		}
	}

	public Dictionary<string, string> ClassColor
	{
		get
		{
			return _ClassColor;
		}
		set
		{
			_ClassColor = value;
		}
	}

	public Dictionary<string, Color> ColorColor
	{
		get
		{
			return _ColorColor;
		}
		set
		{
			_ColorColor = value;
		}
	}

	public Dictionary<string, Color> RarityColor
	{
		get
		{
			return _RarityColor;
		}
		set
		{
			_RarityColor = value;
		}
	}

	public Dictionary<string, string> CardsDescriptionNormalized
	{
		get
		{
			return _CardsDescriptionNormalized;
		}
		set
		{
			_CardsDescriptionNormalized = value;
		}
	}

	public Dictionary<string, SubClassData> SubClass
	{
		get
		{
			return _SubClass;
		}
		set
		{
			_SubClass = value;
		}
	}

	public Dictionary<Enums.CardType, List<string>> CardListByType
	{
		get
		{
			return _CardListByType;
		}
		set
		{
			_CardListByType = value;
		}
	}

	public Dictionary<Enums.CardClass, List<string>> CardListByClass
	{
		get
		{
			return _CardListByClass;
		}
		set
		{
			_CardListByClass = value;
		}
	}

	public List<string> CardListNotUpgraded
	{
		get
		{
			return _CardListNotUpgraded;
		}
		set
		{
			_CardListNotUpgraded = value;
		}
	}

	public Dictionary<Enums.CardClass, List<string>> CardListNotUpgradedByClass
	{
		get
		{
			return _CardListNotUpgradedByClass;
		}
		set
		{
			_CardListNotUpgradedByClass = value;
		}
	}

	public Dictionary<string, List<string>> CardListByClassType
	{
		get
		{
			return _CardListByClassType;
		}
		set
		{
			_CardListByClassType = value;
		}
	}

	public Dictionary<string, string> IsoToEnglishCountryNames => _IsoToEnglishCountryNames;

	public string CurrentLang
	{
		get
		{
			return currentLang;
		}
		set
		{
			currentLang = value;
		}
	}

	public Dictionary<string, EventRequirementData> Requirements
	{
		get
		{
			return _Requirements;
		}
		set
		{
			_Requirements = value;
		}
	}

	public Dictionary<string, NodeData> NodeDataSource
	{
		get
		{
			return _NodeDataSource;
		}
		set
		{
			_NodeDataSource = value;
		}
	}

	public Dictionary<Enums.CardType, List<string>> CardItemByType
	{
		get
		{
			return _CardItemByType;
		}
		set
		{
			_CardItemByType = value;
		}
	}

	public Dictionary<string, PackData> PackDataSource
	{
		get
		{
			return _PackDataSource;
		}
		set
		{
			_PackDataSource = value;
		}
	}

	public Dictionary<string, SkinData> SkinDataSource
	{
		get
		{
			return _SkinDataSource;
		}
		set
		{
			_SkinDataSource = value;
		}
	}

	public List<int> PerkLevel
	{
		get
		{
			return _PerkLevel;
		}
		set
		{
			_PerkLevel = value;
		}
	}

	public Dictionary<string, CorruptionPackData> CorruptionPackDataSource
	{
		get
		{
			return _CorruptionPackDataSource;
		}
		set
		{
			_CorruptionPackDataSource = value;
		}
	}

	public SortedDictionary<string, KeyNotesData> KeyNotes
	{
		get
		{
			return _KeyNotes;
		}
		set
		{
			_KeyNotes = value;
		}
	}

	public Dictionary<string, int> CardEnergyCost
	{
		get
		{
			return _CardEnergyCost;
		}
		set
		{
			_CardEnergyCost = value;
		}
	}

	public Dictionary<string, List<string>> CardsListSearch
	{
		get
		{
			return _CardsListSearch;
		}
		set
		{
			_CardsListSearch = value;
		}
	}

	public Dictionary<string, ZoneData> ZoneDataSource
	{
		get
		{
			return _ZoneDataSource;
		}
		set
		{
			_ZoneDataSource = value;
		}
	}

	public Dictionary<string, EventData> Events
	{
		get
		{
			return _Events;
		}
		set
		{
			_Events = value;
		}
	}

	public int NormalFPS
	{
		get
		{
			return normalFPS;
		}
		set
		{
			normalFPS = value;
		}
	}

	public DateTime InitialWeeklyDate
	{
		get
		{
			return initialWeeklyDate;
		}
		set
		{
			initialWeeklyDate = value;
		}
	}

	public Dictionary<string, CardPlayerPackData> CardPlayerPackDataSource
	{
		get
		{
			return _CardPlayerPackDataSource;
		}
		set
		{
			_CardPlayerPackDataSource = value;
		}
	}

	public string DefaultNickName
	{
		get
		{
			return defaultNickName;
		}
		set
		{
			defaultNickName = value;
		}
	}

	public Dictionary<string, string> IncompatibleVersion
	{
		get
		{
			return _IncompatibleVersion;
		}
		set
		{
			_IncompatibleVersion = value;
		}
	}

	public Dictionary<string, ChallengeTrait> ChallengeTraitsSource
	{
		get
		{
			return _ChallengeTraitsSource;
		}
		set
		{
			_ChallengeTraitsSource = value;
		}
	}

	public int[] CorruptionBasePercent
	{
		get
		{
			return _CorruptionBasePercent;
		}
		set
		{
			_CorruptionBasePercent = value;
		}
	}

	public List<int> RankLevel
	{
		get
		{
			return _RankLevel;
		}
		set
		{
			_RankLevel = value;
		}
	}

	public static int MaxPerkPoints => 50;

	public Dictionary<string, CardbackData> CardbackDataSource
	{
		get
		{
			return _CardbackDataSource;
		}
		set
		{
			_CardbackDataSource = value;
		}
	}

	public Dictionary<string, PerkNodeData> PerksNodesSource
	{
		get
		{
			return _PerksNodesSource;
		}
		set
		{
			_PerksNodesSource = value;
		}
	}

	public Dictionary<string, ChallengeData> WeeklyDataSource
	{
		get
		{
			return _WeeklyDataSource;
		}
		set
		{
			_WeeklyDataSource = value;
		}
	}

	public bool ShowDebug
	{
		get
		{
			return showDebug;
		}
		set
		{
			showDebug = value;
		}
	}

	public List<string> SkuAvailable
	{
		get
		{
			return _SkuAvailable;
		}
		set
		{
			_SkuAvailable = value;
		}
	}

	public Dictionary<string, string> NodeCombatEventRelation
	{
		get
		{
			return _NodeCombatEventRelation;
		}
		set
		{
			_NodeCombatEventRelation = value;
		}
	}

	public Enums.Platform ConfigPlatform
	{
		get
		{
			return configPlatform;
		}
		set
		{
			configPlatform = value;
		}
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		_WaitForSecondsDict = new Dictionary<float, WaitForSeconds>();
		_InstantiatedCardData = new List<CardData>();
		_IncompatibleVersion = new Dictionary<string, string>();
		_IncompatibleVersion.Add("0.6.83", "0.6.82");
		_IncompatibleVersion.Add("0.7.9", "0.7.52");
		_IncompatibleVersion.Add("0.8.1", "0.8.01");
		_IncompatibleVersion.Add("0.8.95", "0.8.7");
		_IncompatibleVersion.Add("1.0.0", "0.9.99");
		_IncompatibleVersion.Add("1.1.20", "1.1.10");
		_IncompatibleVersion.Add("1.3.0", "1.2.5");
		_CorruptionBasePercent = new int[3];
		_CorruptionBasePercent[0] = 70;
		_CorruptionBasePercent[1] = 60;
		_CorruptionBasePercent[2] = 50;
		_CardsDescriptionNormalized = new Dictionary<string, string>();
		_SkillColor = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		_SkillColor.Add("Damage", "red");
		_SkillColor.Add("Heal", "green");
		_SkillColor.Add("Stun", "yellow");
		_SkillColor.Add("DamageTurn", "red");
		_SkillColor.Add("HealTurn", "green");
		_ClassColor = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		_ClassColor.Add("warrior", "#F3404E");
		_ClassColor.Add("mage", "#3298FF");
		_ClassColor.Add("healer", "#BBBBBB");
		_ClassColor.Add("scout", "#34FF46");
		_ClassColor.Add("magicknight", "#D07FFF");
		_ClassColor.Add("monster", "#888888");
		_ClassColor.Add("injury", "#B35248");
		_ClassColor.Add("boon", "#12FFEF");
		_ClassColor.Add("item", "#8A4400");
		_ClassColor.Add("special", "#cea067");
		_ClassColor.Add("pet", "#DB641E");
		_ClassColor.Add("enchantment", "#cea067");
		_RarityColor = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase);
		_RarityColor.Add("uncommon", new Color(0f, 0.7f, 0f, 1f));
		_RarityColor.Add("rare", new Color(0f, 0.6f, 1f, 1f));
		_RarityColor.Add("mythic", new Color(1f, 0.7f, 0f, 1f));
		_RarityColor.Add("epic", new Color(0.8f, 0f, 1f, 1f));
		_ColorColor = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase);
		_ColorColor.Add("greenCard", new Color(0f, 0.79f, 0.06f, 1f));
		_ColorColor.Add("redCard", new Color(1f, 0f, 0.31f, 1f));
		_ColorColor.Add("white", new Color(1f, 1f, 1f, 1f));
		_ColorColor.Add("blueCardTitle", new Color(0.4f, 0.73f, 0.92f, 1f));
		_ColorColor.Add("goldCardTitle", new Color(1f, 0.7f, 0.06f, 1f));
		_ColorColor.Add("purple", new Color(0.85f, 0.45f, 1f, 1f));
		_ColorColor.Add("grey", new Color(0.6f, 0.6f, 0.6f, 1f));
		_ExperienceByLevel = new Dictionary<int, int>();
		_ExperienceByLevel.Add(1, 160);
		_ExperienceByLevel.Add(2, 480);
		_ExperienceByLevel.Add(3, 900);
		_ExperienceByLevel.Add(4, 1500);
		_ExperienceByCardCost = new Dictionary<int, int>();
		_ExperienceByCardCost.Add(0, 1);
		_ExperienceByCardCost.Add(1, 1);
		_ExperienceByCardCost.Add(2, 2);
		_ExperienceByCardCost.Add(3, 4);
		_ExperienceByCardCost.Add(4, 6);
		_ExperienceByCardCost.Add(5, 9);
		_ExperienceByCardCost.Add(6, 12);
		_ExperienceByCardCost.Add(7, 16);
		_ExperienceByCardCost.Add(8, 20);
		_ExperienceByCardCost.Add(9, 26);
		_ExperienceByCardCost.Add(10, 32);
		_UpgradeCost = new Dictionary<string, int>();
		_UpgradeCost.Add("Upgrade_Common", 60);
		_UpgradeCost.Add("Upgrade_Uncommon", 180);
		_UpgradeCost.Add("Upgrade_Rare", 420);
		_UpgradeCost.Add("Upgrade_Epic", 1260);
		_UpgradeCost.Add("Upgrade_Mythic", 1940);
		_UpgradeCost.Add("Transform_Common", 60);
		_UpgradeCost.Add("Transform_Uncommon", 180);
		_UpgradeCost.Add("Transform_Rare", 420);
		_UpgradeCost.Add("Transform_Epic", 1260);
		_UpgradeCost.Add("Transform_Mythic", 1940);
		_CraftCost = new Dictionary<string, int>();
		_CraftCost.Add("Common", 60);
		_CraftCost.Add("Uncommon", 180);
		_CraftCost.Add("Rare", 420);
		_CraftCost.Add("Epic", 1260);
		_CraftCost.Add("Mythic", 1940);
		_DivinationCost = new Dictionary<string, int>();
		_DivinationCost.Add("0", 400);
		_DivinationCost.Add("1", 800);
		_DivinationCost.Add("2", 1600);
		_DivinationCost.Add("3", 3200);
		_DivinationCost.Add("4", 5000);
		_DivinationTier = new Dictionary<int, int>();
		_DivinationTier.Add(0, 2);
		_DivinationTier.Add(1, 5);
		_DivinationTier.Add(2, 6);
		_DivinationTier.Add(3, 8);
		_DivinationTier.Add(4, 10);
		_ItemCost = new Dictionary<string, int>();
		_ItemCost.Add("Weapon_Common", 114);
		_ItemCost.Add("Weapon_Uncommon", 247);
		_ItemCost.Add("Weapon_Rare", 551);
		_ItemCost.Add("Weapon_Epic", 1178);
		_ItemCost.Add("Weapon_Mythic", 2375);
		_ItemCost.Add("Armor_Common", 120);
		_ItemCost.Add("Armor_Uncommon", 260);
		_ItemCost.Add("Armor_Rare", 580);
		_ItemCost.Add("Armor_Epic", 1240);
		_ItemCost.Add("Armor_Mythic", 2500);
		_ItemCost.Add("Jewelry_Common", 132);
		_ItemCost.Add("Jewelry_Uncommon", 286);
		_ItemCost.Add("Jewelry_Rare", 638);
		_ItemCost.Add("Jewelry_Epic", 1364);
		_ItemCost.Add("Jewelry_Mythic", 2750);
		_ItemCost.Add("Accesory_Common", 144);
		_ItemCost.Add("Accesory_Uncommon", 312);
		_ItemCost.Add("Accesory_Rare", 696);
		_ItemCost.Add("Accesory_Epic", 1488);
		_ItemCost.Add("Accesory_Mythic", 3000);
		_ItemCost.Add("Pet_Common", 72);
		_ItemCost.Add("Pet_Uncommon", 156);
		_ItemCost.Add("Pet_Rare", 348);
		_ItemCost.Add("Pet_Epic", 744);
		_ItemCost.Add("Pet_Mythic", 1200);
		_PerkLevel = new List<int>();
		_PerkLevel.Add(300);
		_PerkLevel.Add(600);
		_PerkLevel.Add(1000);
		_PerkLevel.Add(1500);
		_PerkLevel.Add(2100);
		_PerkLevel.Add(2800);
		_PerkLevel.Add(3600);
		_PerkLevel.Add(4500);
		_PerkLevel.Add(5500);
		_PerkLevel.Add(6500);
		_PerkLevel.Add(7500);
		_PerkLevel.Add(8700);
		_PerkLevel.Add(9900);
		_PerkLevel.Add(11100);
		_PerkLevel.Add(12500);
		_PerkLevel.Add(14000);
		_PerkLevel.Add(15500);
		_PerkLevel.Add(17000);
		_PerkLevel.Add(18500);
		_PerkLevel.Add(20000);
		_PerkLevel.Add(21500);
		_PerkLevel.Add(23000);
		_PerkLevel.Add(24500);
		_PerkLevel.Add(26000);
		_PerkLevel.Add(27500);
		_PerkLevel.Add(29000);
		_PerkLevel.Add(30700);
		_PerkLevel.Add(32500);
		_PerkLevel.Add(34500);
		_PerkLevel.Add(36500);
		_PerkLevel.Add(39000);
		_PerkLevel.Add(41500);
		_PerkLevel.Add(44000);
		_PerkLevel.Add(46500);
		_PerkLevel.Add(49000);
		_PerkLevel.Add(51500);
		_PerkLevel.Add(54000);
		_PerkLevel.Add(56500);
		_PerkLevel.Add(58000);
		_PerkLevel.Add(61000);
		_PerkLevel.Add(64000);
		_PerkLevel.Add(67000);
		_PerkLevel.Add(70500);
		_PerkLevel.Add(74000);
		_PerkLevel.Add(77500);
		_PerkLevel.Add(81000);
		_PerkLevel.Add(84500);
		_PerkLevel.Add(88000);
		_PerkLevel.Add(91500);
		_PerkLevel.Add(95500);
		_RankLevel = new List<int>();
		_RankLevel.Add(4);
		_RankLevel.Add(8);
		_RankLevel.Add(12);
		_RankLevel.Add(16);
		_RankLevel.Add(20);
		_RankLevel.Add(24);
		_RankLevel.Add(28);
		_RankLevel.Add(32);
		_RankLevel.Add(36);
		_RankLevel.Add(40);
		_RankLevel.Add(44);
		_SkuAvailable = new List<string>(13);
		_SkuAvailable.Add("2168960");
		_SkuAvailable.Add("2325780");
		_SkuAvailable.Add("2511580");
		_SkuAvailable.Add("2666340");
		_SkuAvailable.Add("2879690");
		_SkuAvailable.Add("2879680");
		_SkuAvailable.Add("3185630");
		_SkuAvailable.Add("3185650");
		_SkuAvailable.Add("3185640");
		_SkuAvailable.Add("3473720");
		_SkuAvailable.Add("3473700");
		_SkuAvailable.Add("4013420");
		_SkuAvailable.Add("3875470");
		_IsoToEnglishCountryNames = new Dictionary<string, string>
		{
			{ "AD", "Andorra" },
			{ "AE", "United Arab Emirates" },
			{ "AF", "Afghanistan" },
			{ "AG", "Antigua and Barbuda" },
			{ "AI", "Anguilla" },
			{ "AL", "Albania" },
			{ "AM", "Armenia" },
			{ "AO", "Angola" },
			{ "AQ", "Antarctica" },
			{ "AR", "Argentina" },
			{ "AS", "American Samoa" },
			{ "AT", "Austria" },
			{ "AU", "Australia" },
			{ "AW", "Aruba" },
			{ "AX", "Åland Islands" },
			{ "AZ", "Azerbaijan" },
			{ "BA", "Bosnia and Herzegovina" },
			{ "BB", "Barbados" },
			{ "BD", "Bangladesh" },
			{ "BE", "Belgium" },
			{ "BF", "Burkina Faso" },
			{ "BG", "Bulgaria" },
			{ "BH", "Bahrain" },
			{ "BI", "Burundi" },
			{ "BJ", "Benin" },
			{ "BL", "Saint Barthélemy" },
			{ "BM", "Bermuda" },
			{ "BN", "Brunei Darussalam" },
			{ "BO", "Bolivia, Plurinational State of" },
			{ "BQ", "Bonaire, Sint Eustatius and Saba" },
			{ "BR", "Brazil" },
			{ "BS", "Bahamas" },
			{ "BT", "Bhutan" },
			{ "BV", "Bouvet Island" },
			{ "BW", "Botswana" },
			{ "BY", "Belarus" },
			{ "BZ", "Belize" },
			{ "CA", "Canada" },
			{ "CC", "Cocos (Keeling) Islands" },
			{ "CD", "Congo, the Democratic Republic of the" },
			{ "CF", "Central African Republic" },
			{ "CG", "Congo" },
			{ "CH", "Switzerland" },
			{ "CI", "Côte d'Ivoire" },
			{ "CK", "Cook Islands" },
			{ "CL", "Chile" },
			{ "CM", "Cameroon" },
			{ "CN", "China" },
			{ "CO", "Colombia" },
			{ "CR", "Costa Rica" },
			{ "CU", "Cuba" },
			{ "CV", "Cabo Verde" },
			{ "CW", "Curaçao" },
			{ "CX", "Christmas Island" },
			{ "CY", "Cyprus" },
			{ "CZ", "Czech Republic" },
			{ "DE", "Germany" },
			{ "DJ", "Djibouti" },
			{ "DK", "Denmark" },
			{ "DM", "Dominica" },
			{ "DO", "Dominican Republic" },
			{ "DZ", "Algeria" },
			{ "EC", "Ecuador" },
			{ "EE", "Estonia" },
			{ "EG", "Egypt" },
			{ "EH", "Western Sahara" },
			{ "ER", "Eritrea" },
			{ "ES", "Spain" },
			{ "ET", "Ethiopia" },
			{ "FI", "Finland" },
			{ "FJ", "Fiji" },
			{ "FK", "Falkland Islands (Malvinas)" },
			{ "FM", "Micronesia, Federated States of" },
			{ "FO", "Faroe Islands" },
			{ "FR", "France" },
			{ "GA", "Gabon" },
			{ "GB", "United Kingdom" },
			{ "GD", "Grenada" },
			{ "GE", "Georgia" },
			{ "GF", "French Guiana" },
			{ "GG", "Guernsey" },
			{ "GH", "Ghana" },
			{ "GI", "Gibraltar" },
			{ "GL", "Greenland" },
			{ "GM", "Gambia" },
			{ "GN", "Guinea" },
			{ "GP", "Guadeloupe" },
			{ "GQ", "Equatorial Guinea" },
			{ "GR", "Greece" },
			{ "GS", "South Georgia and the South Sandwich Islands" },
			{ "GT", "Guatemala" },
			{ "GU", "Guam" },
			{ "GW", "Guinea-Bissau" },
			{ "GY", "Guyana" },
			{ "HK", "Hong Kong" },
			{ "HM", "Heard Island and McDonald Islands" },
			{ "HN", "Honduras" },
			{ "HR", "Croatia" },
			{ "HT", "Haiti" },
			{ "HU", "Hungary" },
			{ "ID", "Indonesia" },
			{ "IE", "Ireland" },
			{ "IL", "Israel" },
			{ "IM", "Isle of Man" },
			{ "IN", "India" },
			{ "IO", "British Indian Ocean Territory" },
			{ "IQ", "Iraq" },
			{ "IR", "Iran, Islamic Republic of" },
			{ "IS", "Iceland" },
			{ "IT", "Italy" },
			{ "JE", "Jersey" },
			{ "JM", "Jamaica" },
			{ "JO", "Jordan" },
			{ "JP", "Japan" },
			{ "KE", "Kenya" },
			{ "KG", "Kyrgyzstan" },
			{ "KH", "Cambodia" },
			{ "KI", "Kiribati" },
			{ "KM", "Comoros" },
			{ "KN", "Saint Kitts and Nevis" },
			{ "KP", "Korea, Democratic People's Republic of" },
			{ "KR", "Korea, Republic of" },
			{ "KW", "Kuwait" },
			{ "KY", "Cayman Islands" },
			{ "KZ", "Kazakhstan" },
			{ "LA", "Lao People's Democratic Republic" },
			{ "LB", "Lebanon" },
			{ "LC", "Saint Lucia" },
			{ "LI", "Liechtenstein" },
			{ "LK", "Sri Lanka" },
			{ "LR", "Liberia" },
			{ "LS", "Lesotho" },
			{ "LT", "Lithuania" },
			{ "LU", "Luxembourg" },
			{ "LV", "Latvia" },
			{ "LY", "Libya" },
			{ "MA", "Morocco" },
			{ "MC", "Monaco" },
			{ "MD", "Moldova, Republic of" },
			{ "ME", "Montenegro" },
			{ "MF", "Saint Martin (French part)" },
			{ "MG", "Madagascar" },
			{ "MH", "Marshall Islands" },
			{ "MK", "North Macedonia" },
			{ "ML", "Mali" },
			{ "MM", "Myanmar" },
			{ "MN", "Mongolia" },
			{ "MO", "Macao" },
			{ "MP", "Northern Mariana Islands" },
			{ "MQ", "Martinique" },
			{ "MR", "Mauritania" },
			{ "MS", "Montserrat" },
			{ "MT", "Malta" },
			{ "MU", "Mauritius" },
			{ "MV", "Maldives" },
			{ "MW", "Malawi" },
			{ "MX", "Mexico" },
			{ "MY", "Malaysia" },
			{ "MZ", "Mozambique" },
			{ "NA", "Namibia" },
			{ "NC", "New Caledonia" },
			{ "NE", "Niger" },
			{ "NF", "Norfolk Island" },
			{ "NG", "Nigeria" },
			{ "NI", "Nicaragua" },
			{ "NL", "Netherlands" },
			{ "NO", "Norway" },
			{ "NP", "Nepal" },
			{ "NR", "Nauru" },
			{ "NU", "Niue" },
			{ "NZ", "New Zealand" },
			{ "OM", "Oman" },
			{ "PA", "Panama" },
			{ "PE", "Peru" },
			{ "PF", "French Polynesia" },
			{ "PG", "Papua New Guinea" },
			{ "PH", "Philippines" },
			{ "PK", "Pakistan" },
			{ "PL", "Poland" },
			{ "PM", "Saint Pierre and Miquelon" },
			{ "PN", "Pitcairn" },
			{ "PR", "Puerto Rico" },
			{ "PS", "Palestine, State of" },
			{ "PT", "Portugal" },
			{ "PW", "Palau" },
			{ "PY", "Paraguay" },
			{ "QA", "Qatar" },
			{ "RE", "Réunion" },
			{ "RO", "Romania" },
			{ "RS", "Serbia" },
			{ "RU", "Russian Federation" },
			{ "RW", "Rwanda" },
			{ "SA", "Saudi Arabia" },
			{ "SB", "Solomon Islands" },
			{ "SC", "Seychelles" },
			{ "SD", "Sudan" },
			{ "SE", "Sweden" },
			{ "SG", "Singapore" },
			{ "SH", "Saint Helena, Ascension and Tristan da Cunha" },
			{ "SI", "Slovenia" },
			{ "SJ", "Svalbard and Jan Mayen" },
			{ "SK", "Slovakia" },
			{ "SL", "Sierra Leone" },
			{ "SM", "San Marino" },
			{ "SN", "Senegal" },
			{ "SO", "Somalia" },
			{ "SR", "Suriname" },
			{ "SS", "South Sudan" },
			{ "ST", "Sao Tome and Principe" },
			{ "SV", "El Salvador" },
			{ "SX", "Sint Maarten (Dutch part)" },
			{ "SY", "Syrian Arab Republic" },
			{ "SZ", "Eswatini" },
			{ "TC", "Turks and Caicos Islands" },
			{ "TD", "Chad" },
			{ "TF", "French Southern Territories" },
			{ "TG", "Togo" },
			{ "TH", "Thailand" },
			{ "TJ", "Tajikistan" },
			{ "TK", "Tokelau" },
			{ "TL", "Timor-Leste" },
			{ "TM", "Turkmenistan" },
			{ "TN", "Tunisia" },
			{ "TO", "Tonga" },
			{ "TR", "Turkey" },
			{ "TT", "Trinidad and Tobago" },
			{ "TV", "Tuvalu" },
			{ "TW", "Taiwan, Province of China" },
			{ "TZ", "Tanzania, United Republic of" },
			{ "UA", "Ukraine" },
			{ "UG", "Uganda" },
			{ "UM", "United States Minor Outlying Islands" },
			{ "US", "United States" },
			{ "UY", "Uruguay" },
			{ "UZ", "Uzbekistan" },
			{ "VA", "Holy See (Vatican City State)" },
			{ "VC", "Saint Vincent and the Grenadines" },
			{ "VE", "Venezuela, Bolivarian Republic of" },
			{ "VG", "Virgin Islands, British" },
			{ "VI", "Virgin Islands, U.S." },
			{ "VN", "Viet Nam" },
			{ "VU", "Vanuatu" },
			{ "WF", "Wallis and Futuna" },
			{ "WS", "Samoa" },
			{ "YE", "Yemen" },
			{ "YT", "Mayotte" },
			{ "ZA", "South Africa" },
			{ "ZM", "Zambia" },
			{ "ZW", "Zimbabwe" }
		};
	}

	public void CreateGameContent()
	{
		_CardItemByType = new Dictionary<Enums.CardType, List<string>>();
		_CardEnergyCost = new Dictionary<string, int>();
		KeyNotesData[] array = Resources.LoadAll<KeyNotesData>("KeyNotes");
		KeyNotes = new SortedDictionary<string, KeyNotesData>();
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i].KeynoteName.Replace(" ", "").ToLower();
			array[i].Id = text;
			KeyNotes.Add(text, UnityEngine.Object.Instantiate(array[i]));
			if (saveKeynotesText)
			{
				keynotesText = keynotesText + text + "_description=" + Functions.NormalizeTextForArchive(array[i].Description) + "\n";
				keynotesText = keynotesText + text + "_descriptionExtended=" + Functions.NormalizeTextForArchive(array[i].DescriptionExtended) + "\n";
			}
			KeyNotes[text].KeynoteName = Texts.Instance.GetText(KeyNotes[text].KeynoteName);
			string text2 = Texts.Instance.GetText(text + "_description", "keynotes");
			if (text2 != "")
			{
				KeyNotes[text].Description = text2;
			}
			string text3 = Texts.Instance.GetText(text + "_descriptionExtended", "keynotes");
			if (text3 != "")
			{
				KeyNotes[text].DescriptionExtended = text3;
			}
		}
		CardData[] array2 = Resources.LoadAll<CardData>("Cards");
		if (array2.Any((CardData c) => c.Id == "bruteforce"))
		{
			Debug.Log("bruteforce found");
		}
		if (array2.Any((CardData c) => c.Id == "bruteforcenp"))
		{
			Debug.Log("bruteforce np found");
		}
		_CardsSource = new Dictionary<string, CardData>();
		for (int num = 0; num < array2.Length; num++)
		{
			array2[num].Id = array2[num].name.ToLower();
			if (_CardsSource.ContainsKey(array2[num].Id))
			{
				continue;
			}
			_CardsSource.Add(array2[num].Id, UnityEngine.Object.Instantiate(array2[num]));
			if (saveCardsText)
			{
				if (array2[num].CardUpgraded == Enums.CardUpgraded.No && array2[num].CardClass != Enums.CardClass.MagicKnight)
				{
					cardsText = cardsText + "c_" + TruncateNecropolisCardId(array2[num].Id) + "_name=" + Functions.NormalizeTextForArchive(array2[num].CardName) + "\n";
					cardsText.Replace("v2", "");
				}
				if (array2[num].Fluff != "")
				{
					cardsFluffText = cardsFluffText + "c_" + TruncateNecropolisCardId(array2[num].Id) + "_fluff=" + Functions.NormalizeTextForArchive(array2[num].Fluff) + "\n";
				}
			}
		}
		HeroData[] array3 = Resources.LoadAll<HeroData>("Heroes");
		_Heroes = new Dictionary<string, HeroData>();
		for (int num2 = 0; num2 < array3.Length; num2++)
		{
			_Heroes.Add(array3[num2].Id, UnityEngine.Object.Instantiate(array3[num2]));
		}
		SubClassData[] array4 = Resources.LoadAll<SubClassData>("SubClass");
		_SubClassSource = new Dictionary<string, SubClassData>();
		for (int num3 = 0; num3 < array4.Length; num3++)
		{
			_SubClassSource.Add(array4[num3].SubClassName.Replace(" ", "").ToLower(), UnityEngine.Object.Instantiate(array4[num3]));
		}
		NPCData[] array5 = Resources.LoadAll<NPCData>("NPCs");
		_NPCsSource = new Dictionary<string, NPCData>();
		for (int num4 = 0; num4 < array5.Length; num4++)
		{
			_NPCsSource.Add(array5[num4].Id, UnityEngine.Object.Instantiate(array5[num4]));
		}
		CreateCharClones();
		CreateAuraCurses();
		CreateCardClones();
		_TraitsSource = new Dictionary<string, TraitData>();
		TraitData[] array6 = Resources.LoadAll<TraitData>("Traits");
		for (int num5 = 0; num5 < array6.Length; num5++)
		{
			array6[num5].Init();
			_TraitsSource.Add(array6[num5].Id, UnityEngine.Object.Instantiate(array6[num5]));
			if (saveTraitsText && array6[num5].TraitCard == null)
			{
				traitsText = traitsText + array6[num5].Id + "=" + Functions.NormalizeTextForArchive(array6[num5].TraitName) + "\n";
				traitsText = traitsText + array6[num5].Id + "_description=" + Functions.NormalizeTextForArchive(array6[num5].Description) + "\n";
			}
		}
		CreateTraitClones();
		_PerksSource = new Dictionary<string, PerkData>();
		PerkData[] array7 = Resources.LoadAll<PerkData>("Perks");
		for (int num6 = 0; num6 < array7.Length; num6++)
		{
			array7[num6].Init();
			_PerksSource.Add(array7[num6].Id, UnityEngine.Object.Instantiate(array7[num6]));
		}
		_PerksNodesSource = new Dictionary<string, PerkNodeData>();
		PerkNodeData[] array8 = Resources.LoadAll<PerkNodeData>("PerkNode");
		for (int num7 = 0; num7 < array8.Length; num7++)
		{
			_PerksNodesSource.Add(array8[num7].Id.ToLower(), UnityEngine.Object.Instantiate(array8[num7]));
		}
		if (parseEvents)
		{
			EventsParser.ParseAll();
		}
		EventData[] array9 = Resources.LoadAll<EventData>("World/Events");
		_Events = new Dictionary<string, EventData>();
		for (int num8 = 0; num8 < array9.Length; num8++)
		{
			EventData eventData = UnityEngine.Object.Instantiate(array9[num8]);
			if (saveEventsText)
			{
				string text4 = eventData.EventId.ToLower();
				eventText = eventText + text4 + "_nm=" + EventLineForSave(eventData.EventName, text4 + "_nm") + "\n";
				eventText = eventText + text4 + "_dsc=" + EventLineForSave(eventData.Description, text4 + "_dsc") + "\n";
				eventText = eventText + text4 + "_dsca=" + EventLineForSave(eventData.DescriptionAction, text4 + "_dsca") + "\n";
				for (int num9 = 0; num9 < eventData.Replys.Length; num9++)
				{
					eventText = eventText + text4 + "_rp" + num9 + "=" + EventLineForSave(eventData.Replys[num9].ReplyText, text4 + "_rp" + num9) + "\n";
					eventText = eventText + text4 + "_rp" + num9 + "_s=" + EventLineForSave(eventData.Replys[num9].SsRewardText, text4 + "_rp" + num9 + "_s") + "\n";
					if (eventData.Replys[num9].SscRewardText != "")
					{
						eventText = eventText + text4 + "_rp" + num9 + "_sc=" + EventLineForSave(eventData.Replys[num9].SscRewardText, text4 + "_rp" + num9 + "_sc") + "\n";
					}
					if (eventData.Replys[num9].FlRewardText != "")
					{
						eventText = eventText + text4 + "_rp" + num9 + "_f=" + EventLineForSave(eventData.Replys[num9].FlRewardText, text4 + "_rp" + num9 + "_f") + "\n";
					}
					if (eventData.Replys[num9].FlcRewardText != "")
					{
						eventText = eventText + text4 + "_rp" + num9 + "_fc=" + EventLineForSave(eventData.Replys[num9].FlcRewardText, text4 + "_rp" + num9 + "_fc") + "\n";
					}
				}
			}
			eventData.Init();
			_Events.Add(eventData.EventId.ToLower(), eventData);
		}
		EventRequirementData[] array10 = Resources.LoadAll<EventRequirementData>("World/Events/Requirements");
		_Requirements = new Dictionary<string, EventRequirementData>();
		for (int num10 = 0; num10 < array10.Length; num10++)
		{
			string text5 = array10[num10].RequirementId.ToLower();
			_Requirements.Add(text5, UnityEngine.Object.Instantiate(array10[num10]));
			if (_Requirements[text5].ItemTrack || _Requirements[text5].RequirementTrack)
			{
				if (saveRequirementsText)
				{
					requirementsText = requirementsText + text5 + "_name=" + _Requirements[text5].RequirementName + "\n";
					requirementsText = requirementsText + text5 + "_description=" + _Requirements[text5].Description + "\n";
				}
				string text6 = Texts.Instance.GetText(text5 + "_name", "requirements");
				if (text6 != "")
				{
					_Requirements[text5].RequirementName = text6;
				}
				string text7 = Texts.Instance.GetText(text5 + "_description", "requirements");
				if (text7 != "")
				{
					_Requirements[text5].Description = text7;
				}
			}
		}
		CombatData[] array11 = Resources.LoadAll<CombatData>("World/Combats");
		_CombatDataSource = new Dictionary<string, CombatData>();
		for (int num11 = 0; num11 < array11.Length; num11++)
		{
			_CombatDataSource.Add(array11[num11].CombatId.Replace(" ", "").ToLower(), UnityEngine.Object.Instantiate(array11[num11]));
		}
		NodeData[] array12 = Resources.LoadAll<NodeData>("World/MapNodes");
		_NodeDataSource = new Dictionary<string, NodeData>();
		_NodeCombatEventRelation = new Dictionary<string, string>();
		for (int num12 = 0; num12 < array12.Length; num12++)
		{
			string text8 = array12[num12].NodeId.ToLower();
			_NodeDataSource.Add(text8, UnityEngine.Object.Instantiate(array12[num12]));
			if (saveNodesText)
			{
				if (array12[num12].NodeName == "Obelisk Challenge")
				{
					continue;
				}
				nodesText = nodesText + array12[num12].NodeId + "_name=" + Functions.NormalizeTextForArchive(array12[num12].NodeName) + "\n";
			}
			_NodeDataSource[text8].SourceNodeName = _NodeDataSource[text8].NodeName;
			_NodeDataSource[text8].NodeName = Texts.Instance.GetText(_NodeDataSource[text8].NodeId + "_name", "nodes");
			if (!_NodeCombatEventRelation.ContainsKey(text8))
			{
				_NodeCombatEventRelation.Add(text8, text8);
			}
			for (int num13 = 0; num13 < array12[num12].NodeCombat.Length; num13++)
			{
				if (array12[num12].NodeCombat[num13] != null && !_NodeCombatEventRelation.ContainsKey(array12[num12].NodeCombat[num13].CombatId))
				{
					_NodeCombatEventRelation.Add(array12[num12].NodeCombat[num13].CombatId, text8);
				}
			}
			for (int num14 = 0; num14 < array12[num12].NodeEvent.Length; num14++)
			{
				if (array12[num12].NodeEvent[num14] != null && !_NodeCombatEventRelation.ContainsKey(array12[num12].NodeEvent[num14].EventId))
				{
					_NodeCombatEventRelation.Add(array12[num12].NodeEvent[num14].EventId, text8);
				}
			}
		}
		TierRewardData[] array13 = Resources.LoadAll<TierRewardData>("Rewards");
		_TierRewardDataSource = new Dictionary<int, TierRewardData>();
		for (int num15 = 0; num15 < array13.Length; num15++)
		{
			_TierRewardDataSource.Add(array13[num15].TierNum, UnityEngine.Object.Instantiate(array13[num15]));
		}
		ItemData[] array14 = Resources.LoadAll<ItemData>("Items");
		_ItemDataSource = new Dictionary<string, ItemData>();
		for (int num16 = 0; num16 < array14.Length; num16++)
		{
			array14[num16].Id = array14[num16].name.ToLower();
			_ItemDataSource.Add(array14[num16].Id, UnityEngine.Object.Instantiate(array14[num16]));
		}
		LootData[] array15 = Resources.LoadAll<LootData>("Loot");
		_LootDataSource = new Dictionary<string, LootData>();
		for (int num17 = 0; num17 < array15.Length; num17++)
		{
			_LootDataSource.Add(array15[num17].Id.ToLower(), UnityEngine.Object.Instantiate(array15[num17]));
		}
		PackData[] array16 = Resources.LoadAll<PackData>("Packs");
		_PackDataSource = new Dictionary<string, PackData>();
		for (int num18 = 0; num18 < array16.Length; num18++)
		{
			_PackDataSource.Add(array16[num18].PackId.ToLower(), UnityEngine.Object.Instantiate(array16[num18]));
		}
		SkinData[] array17 = Resources.LoadAll<SkinData>("Skins");
		_SkinDataSource = new Dictionary<string, SkinData>();
		for (int num19 = 0; num19 < array17.Length; num19++)
		{
			_SkinDataSource.Add(array17[num19].SkinId.ToLower(), UnityEngine.Object.Instantiate(array17[num19]));
		}
		CardbackData[] array18 = Resources.LoadAll<CardbackData>("Cardbacks");
		_CardbackDataSource = new Dictionary<string, CardbackData>();
		for (int num20 = 0; num20 < array18.Length; num20++)
		{
			_CardbackDataSource.Add(array18[num20].CardbackId.ToLower(), UnityEngine.Object.Instantiate(array18[num20]));
		}
		CorruptionPackData[] array19 = Resources.LoadAll<CorruptionPackData>("CorruptionRewards");
		_CorruptionPackDataSource = new Dictionary<string, CorruptionPackData>();
		for (int num21 = 0; num21 < array19.Length; num21++)
		{
			_CorruptionPackDataSource.Add(array19[num21].name, UnityEngine.Object.Instantiate(array19[num21]));
		}
		CardPlayerPackData[] array20 = Resources.LoadAll<CardPlayerPackData>("CardPlayer");
		_CardPlayerPackDataSource = new Dictionary<string, CardPlayerPackData>();
		for (int num22 = 0; num22 < array20.Length; num22++)
		{
			_CardPlayerPackDataSource.Add(array20[num22].PackId.ToLower(), UnityEngine.Object.Instantiate(array20[num22]));
		}
		CardPlayerPairsPackData[] array21 = Resources.LoadAll<CardPlayerPairsPackData>("CardPlayerPairs");
		_CardPlayerPairsPackDataSource = new Dictionary<string, CardPlayerPairsPackData>();
		for (int num23 = 0; num23 < array21.Length; num23++)
		{
			_CardPlayerPairsPackDataSource.Add(array21[num23].PackId.ToLower(), UnityEngine.Object.Instantiate(array21[num23]));
		}
		CinematicData[] array22 = Resources.LoadAll<CinematicData>("Cinematics");
		_Cinematics = new Dictionary<string, CinematicData>();
		for (int num24 = 0; num24 < array22.Length; num24++)
		{
			_Cinematics.Add(array22[num24].CinematicId.Replace(" ", "").ToLower(), UnityEngine.Object.Instantiate(array22[num24]));
		}
		ZoneData[] array23 = Resources.LoadAll<ZoneData>("World/Zones");
		_ZoneDataSource = new Dictionary<string, ZoneData>();
		for (int num25 = 0; num25 < array23.Length; num25++)
		{
			_ZoneDataSource.Add(array23[num25].ZoneId.ToLower(), UnityEngine.Object.Instantiate(array23[num25]));
		}
		ChallengeTrait[] array24 = Resources.LoadAll<ChallengeTrait>("Challenge/Traits");
		_ChallengeTraitsSource = new Dictionary<string, ChallengeTrait>();
		for (int num26 = 0; num26 < array24.Length; num26++)
		{
			_ChallengeTraitsSource.Add(array24[num26].Id.ToLower(), UnityEngine.Object.Instantiate(array24[num26]));
		}
		ChallengeData[] array25 = Resources.LoadAll<ChallengeData>("Challenge/Weeks");
		_WeeklyDataSource = new Dictionary<string, ChallengeData>();
		for (int num27 = 0; num27 < array25.Length; num27++)
		{
			_WeeklyDataSource.Add(array25[num27].Id.ToLower(), UnityEngine.Object.Instantiate(array25[num27]));
		}
		if (saveAurasText)
		{
			SaveText.SaveAuras(auraText);
		}
		if (saveCardsText)
		{
			SaveText.SaveCards(cardsText);
			SaveText.SaveCardsFluff(cardsFluffText);
		}
		if (saveClassText)
		{
			SaveText.SaveClass(classText);
		}
		if (saveMonsterText)
		{
			SaveText.SaveMonster(monsterText);
		}
		if (saveEventsText)
		{
			SaveText.SaveEvents(eventText);
		}
		if (saveKeynotesText)
		{
			SaveText.SaveKeynotes(keynotesText);
		}
		if (saveNodesText)
		{
			SaveText.SaveNodes(nodesText);
		}
		if (saveTraitsText)
		{
			SaveText.SaveTraits(traitsText);
		}
		if (saveRequirementsText)
		{
			SaveText.SaveRequirements(requirementsText);
		}
		vfxAux = new Dictionary<string, GameObject>();
		GameObject[] array26 = Resources.LoadAll<GameObject>("Effects");
		foreach (GameObject gameObject in array26)
		{
			if (!vfxAux.ContainsKey(gameObject.name))
			{
				vfxAux.Add(gameObject.name, gameObject);
			}
		}
	}

	private string TruncateNecropolisCardId(string ID)
	{
		ID = ID.ToLower();
		if (ID.EndsWith("mnp") && !ID.Contains("cataclysm"))
		{
			return ID.Substring(0, ID.Length - 3);
		}
		if (ID.EndsWith("np"))
		{
			return ID.Substring(0, ID.Length - 2);
		}
		return ID;
	}

	public void SetLang(int _langIndex)
	{
		switch (_langIndex)
		{
		case 1:
			currentLang = "es";
			break;
		case 2:
			currentLang = "zh-CN";
			break;
		case 3:
			currentLang = "ko";
			break;
		case 4:
			currentLang = "jp";
			break;
		case 5:
			currentLang = "zh-TW";
			break;
		case 6:
			currentLang = "de";
			break;
		case 7:
			currentLang = "fr";
			break;
		default:
			currentLang = "en";
			break;
		}
	}

	private string EventLineForSave(string _line, string _id)
	{
		foreach (KeyValuePair<string, string> savedEventLine in savedEventLines)
		{
			if (savedEventLine.Value == _line)
			{
				return "rptd_" + savedEventLine.Key;
			}
		}
		savedEventLines.Add(_id, _line);
		return Functions.NormalizeTextForArchive(_line);
	}

	public int GetExperienceByLevel(int level)
	{
		if (_ExperienceByLevel.ContainsKey(level))
		{
			return _ExperienceByLevel[level];
		}
		return 10000;
	}

	public int GetExperienceByCardCost(int cardCost)
	{
		if (_ExperienceByCardCost.ContainsKey(cardCost))
		{
			return _ExperienceByCardCost[cardCost];
		}
		return 0;
	}

	public void CreateCharClones()
	{
		_NPCs = new Dictionary<string, NPCData>();
		_NPCsNamed = new Dictionary<string, NPCData>();
		SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
		foreach (string key in _NPCsSource.Keys)
		{
			if (!sortedDictionary.ContainsKey(key))
			{
				sortedDictionary.Add(key, _NPCsSource[key].NPCName);
			}
			_NPCs.Add(key, UnityEngine.Object.Instantiate(_NPCsSource[key]));
			string text = Texts.Instance.GetText(key + "_name", "monsters");
			if (text != "")
			{
				_NPCs[key].NPCName = text;
			}
			if (_NPCsSource[key].IsNamed && _NPCsSource[key].Difficulty > -1)
			{
				_NPCsNamed.Add(key, UnityEngine.Object.Instantiate(_NPCsSource[key]));
				text = Texts.Instance.GetText(key + "_name", "monsters");
				if (text != "")
				{
					_NPCsNamed[key].NPCName = text;
				}
			}
		}
		if (saveMonsterText)
		{
			foreach (KeyValuePair<string, string> item in sortedDictionary)
			{
				monsterText = monsterText + item.Key + "_name=" + Functions.NormalizeTextForArchive(item.Value) + "\n";
			}
		}
		_SubClass = new Dictionary<string, SubClassData>();
		foreach (string key2 in _SubClassSource.Keys)
		{
			_SubClass.Add(key2, UnityEngine.Object.Instantiate(_SubClassSource[key2]));
			if (saveClassText)
			{
				classText = classText + key2 + "_name=" + Functions.NormalizeTextForArchive(_SubClassSource[key2].CharacterName) + "\n";
				classText = classText + key2 + "_description=" + Functions.NormalizeTextForArchive(_SubClassSource[key2].CharacterDescription) + "\n";
				classText = classText + key2 + "_strength=" + Functions.NormalizeTextForArchive(_SubClassSource[key2].CharacterDescriptionStrength) + "\n";
			}
			_SubClass[key2].CharacterName = Texts.Instance.GetText(key2 + "_name", "class");
			_SubClass[key2].CharacterDescription = Texts.Instance.GetText(key2 + "_description", "class");
			_SubClass[key2].CharacterDescriptionStrength = Texts.Instance.GetText(key2 + "_strength", "class");
		}
	}

	public void CreateAuraCurses()
	{
		_AurasCursesSource = new Dictionary<string, AuraCurseData>();
		_AurasCursesIndex = new List<string>();
		AuraCurseData[] array = Resources.LoadAll<AuraCurseData>("Auras");
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Init();
			_AurasCursesSource.Add(array[i].Id, UnityEngine.Object.Instantiate(array[i]));
			_AurasCursesIndex.Add(array[i].Id.ToLower());
		}
		array = Resources.LoadAll<AuraCurseData>("Curses");
		for (int j = 0; j < array.Length; j++)
		{
			array[j].Init();
			_AurasCursesSource.Add(array[j].Id, UnityEngine.Object.Instantiate(array[j]));
			_AurasCursesIndex.Add(array[j].Id.ToLower());
		}
		_AurasCurses = new Dictionary<string, AuraCurseData>();
		foreach (string key in _AurasCursesSource.Keys)
		{
			_AurasCurses.Add(key, _AurasCursesSource[key]);
			_AurasCurses[key].Init();
			if (saveAurasText)
			{
				auraText = auraText + key + "_description=" + Functions.NormalizeTextForArchive(_AurasCursesSource[key].Description) + "\n";
			}
			_AurasCurses[key].ACName = Texts.Instance.GetText(_AurasCurses[key].Id);
			string text = Texts.Instance.GetText(key + "_description", "auracurse");
			if (text != "")
			{
				_AurasCurses[key].Description = text;
			}
		}
	}

	public void ModifyAuraCurseKey(string _ac, string _key, string _valueStr = "", int _valueInt = 0)
	{
		AuraCurseData auraCurseData = _AurasCurses[_ac];
		if (!(auraCurseData == null))
		{
			if (_valueStr != "")
			{
				typeof(AuraCurseData).GetProperty(_key).SetValue(auraCurseData, _valueStr, null);
			}
			else
			{
				typeof(AuraCurseData).GetProperty(_key).SetValue(auraCurseData, _valueInt, null);
			}
		}
	}

	public void IncludeInSearch(string _term, string id, bool includeFullTerm = true)
	{
		if (SceneStatic.GetSceneName() != "Game")
		{
			return;
		}
		_term = _term.ToLower();
		string[] array = _term.Split(' ');
		id = id.ToLower();
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (text.Trim() != "")
			{
				if (!_CardsListSearch.ContainsKey(text))
				{
					_CardsListSearch.Add(text, new List<string>());
				}
				if (!_CardsListSearch[text].Contains(id))
				{
					_CardsListSearch[text].Add(id);
				}
			}
		}
		if (includeFullTerm)
		{
			if (!_CardsListSearch.ContainsKey(_term))
			{
				_CardsListSearch.Add(_term, new List<string>());
			}
			if (!_CardsListSearch[_term].Contains(id))
			{
				_CardsListSearch[_term].Add(id);
			}
		}
	}

	public bool IsInSearch(string _term, string _id)
	{
		_term = _term.ToLower();
		_id = _id.ToLower();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(_term);
		stringBuilder.Append('_');
		stringBuilder.Append(_id);
		if (_CardsListSearched == null)
		{
			_CardsListSearched = new Dictionary<string, bool>();
		}
		else if (_CardsListSearched.ContainsKey(stringBuilder.ToString()))
		{
			return _CardsListSearched[stringBuilder.ToString()];
		}
		string[] terms = _term.Split(' ');
		bool flag = false;
		int i;
		for (i = 0; i < terms.Length; i++)
		{
			flag = false;
			foreach (string item in _CardsListSearch.Keys.Where((string key) => key.Contains(terms[i])).ToList())
			{
				if (_CardsListSearch[item].Contains(_id))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				break;
			}
		}
		if (flag)
		{
			_CardsListSearched.Add(stringBuilder.ToString(), value: true);
		}
		else
		{
			_CardsListSearched.Add(stringBuilder.ToString(), value: false);
		}
		return flag;
	}

	public void CreateCardClones()
	{
		_CardsListSearch = new Dictionary<string, List<string>>();
		_CardListByType = new Dictionary<Enums.CardType, List<string>>();
		_CardListByClass = new Dictionary<Enums.CardClass, List<string>>();
		_CardListNotUpgraded = new List<string>();
		_CardListNotUpgradedByClass = new Dictionary<Enums.CardClass, List<string>>();
		_CardListByClassType = new Dictionary<string, List<string>>();
		_CardEnergyCost = new Dictionary<string, int>();
		foreach (Enums.CardType value in Enum.GetValues(typeof(Enums.CardType)))
		{
			if (value != Enums.CardType.None)
			{
				_CardListByType[value] = new List<string>();
			}
		}
		foreach (Enums.CardClass value2 in Enum.GetValues(typeof(Enums.CardClass)))
		{
			_CardListByClass[value2] = new List<string>();
			_CardListNotUpgradedByClass[value2] = new List<string>();
		}
		_Cards = new Dictionary<string, CardData>();
		foreach (string key in _CardsSource.Keys)
		{
			_Cards.Add(key, _CardsSource[key]);
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (string key2 in _CardsSource.Keys)
		{
			stringBuilder.Clear();
			_Cards[key2].InitClone(key2);
			CardData cardData = _Cards[key2];
			string text = "";
			string text2 = "";
			if (cardData.UpgradedFrom != "")
			{
				stringBuilder.Append("c_");
				stringBuilder.Append(TruncateNecropolisCardId(cardData.UpgradedFrom.Replace("v2", "")));
				stringBuilder.Append("_name");
				text = Texts.Instance.GetText(stringBuilder.ToString(), "cards");
			}
			else
			{
				stringBuilder.Append("c_");
				stringBuilder.Append(TruncateNecropolisCardId(cardData.Id.Replace("v2", "")));
				stringBuilder.Append("_name");
				text = Texts.Instance.GetText(stringBuilder.ToString(), "cards");
			}
			if (text != "")
			{
				cardData.CardName = text;
			}
			stringBuilder.Clear();
			stringBuilder.Append("c_");
			stringBuilder.Append(TruncateNecropolisCardId(cardData.Id.Replace("v2", "")));
			stringBuilder.Append("_fluff");
			text2 = Texts.Instance.GetText(stringBuilder.ToString(), "cards");
			if (text2 != "")
			{
				cardData.Fluff = text2;
			}
			if ((cardData.CardClass == Enums.CardClass.Item && cardData.Item != null && cardData.Item.QuestItem) || !cardData.ShowInTome)
			{
				continue;
			}
			_CardEnergyCost.Add(cardData.Id, cardData.EnergyCost);
			IncludeInSearch(cardData.CardName, cardData.Id);
			_CardListByClass[cardData.CardClass].Add(cardData.Id);
			if (cardData.CardUpgraded == Enums.CardUpgraded.No)
			{
				_CardListNotUpgradedByClass[cardData.CardClass].Add(cardData.Id);
				_CardListNotUpgraded.Add(cardData.Id);
				if (cardData.CardClass == Enums.CardClass.Item)
				{
					if (!_CardItemByType.ContainsKey(cardData.CardType))
					{
						_CardItemByType.Add(cardData.CardType, new List<string>());
					}
					_CardItemByType[cardData.CardType].Add(cardData.Id);
				}
			}
			string text3 = "";
			List<Enums.CardType> cardTypes = cardData.GetCardTypes();
			for (int i = 0; i < cardTypes.Count; i++)
			{
				_CardListByType[cardTypes[i]].Add(cardData.Id);
				text3 = Enum.GetName(typeof(Enums.CardClass), cardData.CardClass) + "_" + Enum.GetName(typeof(Enums.CardType), cardTypes[i]);
				if (!_CardListByClassType.ContainsKey(text3))
				{
					_CardListByClassType[text3] = new List<string>();
				}
				_CardListByClassType[text3].Add(cardData.Id);
				IncludeInSearch(Texts.Instance.GetText(Enum.GetName(typeof(Enums.CardType), cardTypes[i])), cardData.Id);
			}
		}
		foreach (string key3 in _Cards.Keys)
		{
			_Cards[key3].InitClone2();
		}
		_CardListNotUpgraded.Sort();
	}

	public void CreateTraitClones()
	{
		_Traits = new Dictionary<string, TraitData>();
		foreach (string key in _TraitsSource.Keys)
		{
			_Traits.Add(key, UnityEngine.Object.Instantiate(_TraitsSource[key]));
			_Traits[key].SetNameAndDescription();
		}
	}

	public EventData GetEventData(string id)
	{
		id = id.ToLower();
		if (_Events.ContainsKey(id))
		{
			return _Events[id];
		}
		return null;
	}

	public CinematicData GetCinematicData(string id)
	{
		id = id.ToLower();
		if (_Cinematics.ContainsKey(id))
		{
			return _Cinematics[id];
		}
		return null;
	}

	public CombatData GetCombatData(string id)
	{
		if (id != null && id != "")
		{
			id = id.ToLower();
			if (_CombatDataSource.ContainsKey(id))
			{
				return _CombatDataSource[id];
			}
		}
		return null;
	}

	public NodeData GetNodeData(string id)
	{
		if (id != null && id != "")
		{
			id = id.ToLower();
			if (_NodeDataSource.ContainsKey(id))
			{
				return _NodeDataSource[id];
			}
		}
		return null;
	}

	public TierRewardData GetTierRewardData(int num)
	{
		if (num < 0)
		{
			num = 0;
		}
		if (_TierRewardDataSource[num] != null)
		{
			return _TierRewardDataSource[num];
		}
		return null;
	}

	public ItemData GetItemData(string id)
	{
		if (_ItemDataSource.ContainsKey(id))
		{
			return _ItemDataSource[id];
		}
		return null;
	}

	public LootData GetLootData(string id)
	{
		if (id != null && id != "")
		{
			id = id.ToLower();
			if (_LootDataSource.ContainsKey(id))
			{
				return _LootDataSource[id];
			}
		}
		return null;
	}

	public SubClassData GetSubClassData(string id)
	{
		if (id != null && id != "")
		{
			id = id.Replace(" ", "").ToLower();
			if (_SubClass.ContainsKey(id))
			{
				return _SubClass[id];
			}
		}
		return null;
	}

	public AuraCurseData GetAuraCurseData(string id)
	{
		if (id != null && id != "")
		{
			id = id.ToLower();
			if (_AurasCurses.ContainsKey(id))
			{
				return _AurasCurses[id];
			}
		}
		return null;
	}

	public int GetAuraCurseIndex(string id)
	{
		if (id != null && id != "")
		{
			id = id.ToLower();
			if (_AurasCursesIndex.Contains(id))
			{
				return _AurasCursesIndex.IndexOf(id);
			}
		}
		return -1;
	}

	public string GetAuraCurseFromIndex(int index)
	{
		if (index > -1 && index < _AurasCursesIndex.Count)
		{
			return _AurasCursesIndex[index];
		}
		return "";
	}

	public TraitData GetTraitData(string id)
	{
		if (id != null && id != "")
		{
			id = Functions.Sanitize(id);
			if (id == "engineer")
			{
				id = "inventor";
			}
			if (_Traits != null && _Traits.ContainsKey(id))
			{
				return _Traits[id];
			}
		}
		return null;
	}

	public PackData GetPackData(string id)
	{
		if (id != null && id != "")
		{
			id = id.ToLower();
			if (_PackDataSource.ContainsKey(id))
			{
				return _PackDataSource[id];
			}
		}
		return null;
	}

	public SkinData GetSkinData(string id)
	{
		if (id != null && id != "")
		{
			id = id.ToLower();
			if (_SkinDataSource.ContainsKey(id))
			{
				return _SkinDataSource[id];
			}
		}
		return null;
	}

	public List<SkinData> GetSkinsBySubclass(string id)
	{
		List<SkinData> list = new List<SkinData>();
		id = id.ToLower();
		foreach (KeyValuePair<string, SkinData> item in _SkinDataSource)
		{
			if (item.Value.SkinSubclass.Id.ToLower() == id)
			{
				list.Add(item.Value);
			}
		}
		return list;
	}

	public string GetSkinBaseIdBySubclass(string id)
	{
		if (id != null && id != "")
		{
			id = id.ToLower();
			foreach (KeyValuePair<string, SkinData> item in _SkinDataSource)
			{
				if (item.Value.SkinSubclass.Id.ToLower() == id && item.Value.BaseSkin)
				{
					return item.Value.SkinId.ToLower().Trim();
				}
			}
		}
		return "";
	}

	public CardbackData GetCardbackData(string id)
	{
		id = id.ToLower();
		if (_CardbackDataSource.ContainsKey(id))
		{
			return _CardbackDataSource[id];
		}
		return null;
	}

	public string GetCardbackBaseIdBySubclass(string id)
	{
		id = id.ToLower();
		if (id != null && id != "" && _CardbackDataSource != null)
		{
			foreach (KeyValuePair<string, CardbackData> item in _CardbackDataSource)
			{
				if (item.Value.CardbackSubclass != null && item.Value.CardbackSubclass.Id.ToLower() == id && item.Value.BaseCardback)
				{
					return item.Value.CardbackId.ToLower().Trim();
				}
			}
		}
		return "";
	}

	public CardPlayerPackData GetCardPlayerPackData(string id)
	{
		id = id.ToLower();
		if (_CardPlayerPackDataSource.ContainsKey(id))
		{
			return _CardPlayerPackDataSource[id];
		}
		return null;
	}

	public CardPlayerPairsPackData GetCardPlayerPairsPackData(string id)
	{
		id = id.ToLower();
		if (_CardPlayerPairsPackDataSource.ContainsKey(id))
		{
			return _CardPlayerPairsPackDataSource[id];
		}
		return null;
	}

	public ChallengeData GetWeeklyById(string _id)
	{
		_id = _id.ToLower();
		foreach (KeyValuePair<string, ChallengeData> item in _WeeklyDataSource)
		{
			_ = item;
			if (WeeklyDataSource.ContainsKey(_id))
			{
				return WeeklyDataSource[_id];
			}
		}
		return null;
	}

	public ChallengeData GetWeeklyData(int _week, bool _getSecondary = false)
	{
		string text = "week" + _week;
		if (_WeeklyDataSource.ContainsKey(text) && !_getSecondary)
		{
			Debug.Log("** " + text);
			return _WeeklyDataSource[text];
		}
		int num = WeeklyDataSource.Count;
		foreach (KeyValuePair<string, ChallengeData> item in WeeklyDataSource)
		{
			if (item.Value.IsDateFixed())
			{
				num--;
			}
		}
		if (num < 1)
		{
			num = 20;
		}
		int num2 = _week % num;
		if (num2 == 0)
		{
			num2 = num;
		}
		text = "week" + num2;
		if (_WeeklyDataSource.ContainsKey(text))
		{
			if (_getSecondary)
			{
				foreach (KeyValuePair<string, ChallengeData> item2 in WeeklyDataSource)
				{
					if (item2.Value.IsDateFixed())
					{
						num++;
					}
				}
				int num3 = Functions.GetConsistentRandom(_week, 1, num);
				string text2 = "";
				int num4 = num2 - 1;
				if (num4 < 1)
				{
					num4 = 20;
				}
				int num5 = num4;
				string key = "week" + num5;
				int consistentRandom = Functions.GetConsistentRandom(num4, 1, num);
				string key2 = "week" + consistentRandom;
				bool flag = false;
				int num6 = 0;
				while (!flag && num6 < 1000)
				{
					num6++;
					text2 = "week" + num3;
					if (num3 != _week && num3 != num5 && num3 != consistentRandom && _WeeklyDataSource.ContainsKey(text2) && _WeeklyDataSource.ContainsKey(key) && _WeeklyDataSource.ContainsKey(key2) && _WeeklyDataSource[text].IdSteam != _WeeklyDataSource[text2].IdSteam && _WeeklyDataSource[key].IdSteam != _WeeklyDataSource[text2].IdSteam && _WeeklyDataSource[key2].IdSteam != _WeeklyDataSource[text2].IdSteam)
					{
						flag = true;
						text = text2;
						continue;
					}
					num3--;
					if (num3 < 1)
					{
						num3 = num;
					}
				}
				_ = 999;
			}
			return _WeeklyDataSource[text];
		}
		return null;
	}

	public List<PerkData> GetPerkDataClass(Enums.CardClass cardClass)
	{
		List<PerkData> list = new List<PerkData>();
		foreach (KeyValuePair<string, PerkData> item in _PerksSource)
		{
			if (item.Value.CardClass == cardClass)
			{
				list.Add(item.Value);
			}
		}
		return list;
	}

	public PerkData GetPerkData(string id)
	{
		id = Functions.Sanitize(id).ToLower();
		if (_PerksSource.ContainsKey(id))
		{
			return _PerksSource[id];
		}
		return null;
	}

	public CardData GetCardData(string id, bool instantiate = true)
	{
		id = Functions.Sanitize(id).ToLower().Trim()
			.Split("_")[0];
		string text = id;
		if (IsCorruptedByNightmareEcho(text))
		{
			id = id.Split('-')[0];
		}
		CardData cardData = null;
		if (!string.IsNullOrEmpty(id) && _Cards.ContainsKey(id))
		{
			if (instantiate)
			{
				CardData cardData2 = UnityEngine.Object.Instantiate(_Cards[id]);
				_InstantiatedCardData.Add(cardData2);
				cardData = cardData2;
			}
			else
			{
				cardData = _Cards[id];
			}
		}
		if (IsCorruptedByNightmareEcho(text))
		{
			string id2 = text.Split('-').Last();
			CardData cardData3 = MatchManager.Instance.GetCardData(id2);
			MatchManager.Instance.CopyCardDataFromAnotherCard(cardData3, cardData, cardData?.CopyConfig);
		}
		return cardData;
	}

	private bool IsCorruptedByNightmareEcho(string id)
	{
		if (id.Contains("-"))
		{
			return id.StartsWith("NightmareEchoTemplate", StringComparison.OrdinalIgnoreCase);
		}
		return false;
	}

	public void CleanInstantiatedCardData()
	{
		for (int num = _InstantiatedCardData.Count - 1; num >= 0; num--)
		{
			UnityEngine.Object.Destroy(_InstantiatedCardData[num], 1f);
		}
		_InstantiatedCardData.Clear();
	}

	public int InstantiatedCardDataCount()
	{
		return _InstantiatedCardData.Count;
	}

	public HeroData GetHeroData(string id)
	{
		if (!_Heroes.TryGetValue(id.ToLower(), out var value))
		{
			return null;
		}
		return value;
	}

	public KeyNotesData GetKeyNotesData(string id)
	{
		if (!KeyNotes.TryGetValue(id.ToLower(), out var value))
		{
			return null;
		}
		return value;
	}

	public NPCData GetNPC(string id)
	{
		if (ShowDebug)
		{
			Functions.DebugLogGD("GetNPC -> " + id, "trace");
		}
		if (!_NPCs.TryGetValue(id.ToLower(), out var value))
		{
			return null;
		}
		return value;
	}

	public NPCData GetNPCForRandom(bool _rare, int position, Enums.CombatTier _ct, NPCData[] _teamNPC)
	{
		bool flag = false;
		int num = 0;
		int num2 = UnityEngine.Random.Range(0, 100);
		switch (_ct)
		{
		case Enums.CombatTier.T1:
			num = ((!_rare) ? 1 : 2);
			break;
		case Enums.CombatTier.T2:
			num = ((!_rare) ? 3 : 4);
			break;
		case Enums.CombatTier.T3:
			num = ((!_rare) ? ((num2 >= 80) ? 7 : 5) : 6);
			break;
		case Enums.CombatTier.T4:
			num = ((!_rare) ? ((num2 >= 15) ? ((num2 >= 85) ? 9 : 7) : 5) : 8);
			break;
		case Enums.CombatTier.T5:
			num = ((!_rare) ? ((num2 >= 20) ? 9 : 7) : 10);
			break;
		case Enums.CombatTier.T6:
			num = ((!_rare) ? ((num2 >= 60) ? 7 : 5) : ((num2 >= 50) ? 8 : 6));
			break;
		case Enums.CombatTier.T7:
			num = ((!_rare) ? ((num2 >= 30) ? 9 : 7) : ((num2 >= 50) ? 10 : 8));
			break;
		case Enums.CombatTier.T8:
			if (_rare)
			{
				if (GameManager.Instance.IsWeeklyChallenge())
				{
					ChallengeData weeklyData2 = GetWeeklyData(AtOManager.Instance.GetWeekly());
					if (weeklyData2 != null && weeklyData2.Boss1 != null)
					{
						return weeklyData2.Boss1;
					}
				}
				num = 15;
			}
			else
			{
				num = ((num2 >= 70) ? 5 : 3);
			}
			break;
		case Enums.CombatTier.T9:
			if (_rare)
			{
				if (GameManager.Instance.IsWeeklyChallenge())
				{
					ChallengeData weeklyData = GetWeeklyData(AtOManager.Instance.GetWeekly());
					if (weeklyData != null && weeklyData.Boss2 != null)
					{
						return weeklyData.Boss2;
					}
				}
				num = 16;
			}
			else
			{
				num = ((num2 >= 30) ? 9 : 7);
			}
			break;
		case Enums.CombatTier.T10:
			num = ((!_rare) ? ((num2 >= 20) ? 11 : 9) : ((num2 >= 50) ? 12 : 10));
			break;
		case Enums.CombatTier.T11:
			num = ((!_rare) ? ((num2 >= 70) ? 13 : 11) : 12);
			break;
		case Enums.CombatTier.T12:
			num = ((!_rare) ? ((num2 >= 30) ? 13 : 11) : 12);
			break;
		}
		NPCData nPCData = null;
		string text = "";
		int num3 = 0;
		while (!flag)
		{
			num3++;
			if (num3 > 10000)
			{
				return null;
			}
			if (_rare)
			{
				text = _NPCsNamed.Keys.ToArray()[UnityEngine.Random.Range(0, _NPCsNamed.Keys.Count)];
				nPCData = _NPCsNamed[text];
			}
			else
			{
				text = _NPCs.Keys.ToArray()[UnityEngine.Random.Range(0, _NPCs.Keys.Count)];
				nPCData = _NPCs[text];
			}
			bool flag2 = true;
			if (_teamNPC != null)
			{
				int num4 = 0;
				for (int i = 0; i < _teamNPC.Length; i++)
				{
					if (_teamNPC[i] != null && _teamNPC[i].SpriteSpeed != null && nPCData.SpriteSpeed != null && _teamNPC[i].SpriteSpeed.name == nPCData.SpriteSpeed.name)
					{
						num4++;
						if (num4 >= 1)
						{
							flag2 = false;
							break;
						}
					}
				}
			}
			if (!flag2 || nPCData.Difficulty != num)
			{
				continue;
			}
			if (!_rare)
			{
				switch (position)
				{
				case 0:
					break;
				case 1:
				{
					Enums.CardTargetPosition preferredPosition = nPCData.PreferredPosition;
					if (preferredPosition == Enums.CardTargetPosition.Anywhere || preferredPosition == Enums.CardTargetPosition.Front)
					{
						flag = true;
					}
					continue;
				}
				case -1:
				{
					Enums.CardTargetPosition preferredPosition = nPCData.PreferredPosition;
					if (preferredPosition == Enums.CardTargetPosition.Anywhere || preferredPosition == Enums.CardTargetPosition.Back)
					{
						flag = true;
					}
					continue;
				}
				default:
					continue;
				}
			}
			flag = true;
		}
		if ((_ct == Enums.CombatTier.T6 || _ct == Enums.CombatTier.T7 || _ct == Enums.CombatTier.T9 || _ct == Enums.CombatTier.T10) && nPCData.UpgradedMob != null)
		{
			nPCData = nPCData.UpgradedMob;
		}
		return nPCData;
	}

	public EventRequirementData GetRequirementData(string id)
	{
		if (!_Requirements.TryGetValue(id.ToLower(), out var value))
		{
			return null;
		}
		return value;
	}

	public int GetRemoveCost(Enums.CardType type, Enums.CardRarity rarity)
	{
		int num = 0;
		int num2 = 100;
		if (type == Enums.CardType.Injury || type == Enums.CardType.Boon)
		{
			return rarity switch
			{
				Enums.CardRarity.Common => num2, 
				Enums.CardRarity.Uncommon => num2 + 50, 
				Enums.CardRarity.Rare => num2 + 100, 
				Enums.CardRarity.Epic => num2 + 150, 
				_ => num2 + 200, 
			};
		}
		return AtOManager.Instance.GetTownTier() switch
		{
			0 => 30, 
			1 => 90, 
			2 => 120, 
			3 => 160, 
			_ => 200, 
		};
	}

	public int GetUpgradeCost(string action, string rarity)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(action);
		stringBuilder.Append("_");
		stringBuilder.Append(rarity);
		if (_UpgradeCost.ContainsKey(stringBuilder.ToString()))
		{
			return _UpgradeCost[stringBuilder.ToString()];
		}
		return -1;
	}

	public int GetCraftCost(string cardId, float discountCraft = 0f, float discountUpgrade = 0f, int zoneTier = 0)
	{
		CardData cardData = GetCardData(cardId, instantiate: false);
		int num = 0;
		StringBuilder stringBuilder = new StringBuilder();
		if (cardData.CardUpgraded == Enums.CardUpgraded.No)
		{
			stringBuilder.Append(Enum.GetName(typeof(Enums.CardRarity), cardData.CardRarity));
			if (_CraftCost.ContainsKey(stringBuilder.ToString()))
			{
				num = _CraftCost[stringBuilder.ToString()];
			}
			if (discountCraft != 0f)
			{
				num -= Functions.FuncRoundToInt((float)num * discountCraft);
			}
			if (GameManager.Instance.IsSingularity() && zoneTier == 0 && AtOManager.Instance.CharInTown())
			{
				num = 0;
			}
			else if (AtOManager.Instance.Sandbox_cardCraftPrice != 0)
			{
				num += Functions.FuncRoundToInt((float)num * (float)AtOManager.Instance.Sandbox_cardCraftPrice * 0.01f);
			}
		}
		else
		{
			CardData cardData2 = GetCardData(cardData.UpgradedFrom, instantiate: false);
			stringBuilder.Append(Enum.GetName(typeof(Enums.CardRarity), cardData2.CardRarity));
			if (_CraftCost.ContainsKey(stringBuilder.ToString()))
			{
				num = _CraftCost[stringBuilder.ToString()];
				if (discountCraft != 0f)
				{
					num -= Functions.FuncRoundToInt((float)num * discountCraft);
				}
				if (GameManager.Instance.IsSingularity() && zoneTier == 0 && AtOManager.Instance.CharInTown())
				{
					num = 0;
				}
				else if (AtOManager.Instance.Sandbox_cardCraftPrice != 0)
				{
					num += Functions.FuncRoundToInt((float)num * (float)AtOManager.Instance.Sandbox_cardCraftPrice * 0.01f);
				}
				float num2 = GetUpgradeCost("Upgrade", Enum.GetName(typeof(Enums.CardRarity), cardData.CardRarity));
				if (discountUpgrade != 0f)
				{
					num2 -= (float)Functions.FuncRoundToInt(num2 * discountUpgrade);
				}
				num += Functions.FuncRoundToInt(num2);
			}
		}
		if (num >= 0)
		{
			return num;
		}
		return -1;
	}

	public int GetItemCost(string cardId)
	{
		CardData cardData = GetCardData(cardId, instantiate: false);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Enum.GetName(typeof(Enums.CardType), cardData.CardType));
		stringBuilder.Append("_");
		stringBuilder.Append(Enum.GetName(typeof(Enums.CardRarity), cardData.CardRarity));
		if (_ItemCost.ContainsKey(stringBuilder.ToString()))
		{
			return _ItemCost[stringBuilder.ToString()];
		}
		return -1;
	}

	public bool ExistsCraftRarity(Enums.CardRarity rarity, int zoneTier)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(zoneTier);
		stringBuilder.Append("_");
		stringBuilder.Append(Enum.GetName(typeof(Enums.CardRarity), rarity));
		if (_CraftCost.ContainsKey(stringBuilder.ToString()))
		{
			return true;
		}
		return false;
	}

	public int GetDivinationCost(string divinationTier)
	{
		if (_DivinationCost.ContainsKey(divinationTier))
		{
			int num = _DivinationCost[divinationTier];
			if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_4_5"))
			{
				num -= Functions.FuncRoundToInt((float)num * 0.4f);
			}
			else if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_4_3"))
			{
				num -= Functions.FuncRoundToInt((float)num * 0.25f);
			}
			else if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_4_1"))
			{
				num -= Functions.FuncRoundToInt((float)num * 0.1f);
			}
			if (AtOManager.Instance.Sandbox_divinationsPrice != 0)
			{
				num += Functions.FuncRoundToInt((float)num * (float)AtOManager.Instance.Sandbox_divinationsPrice * 0.01f);
			}
			return num;
		}
		return -1;
	}

	public int GetDivinationTier(int divinationIndex)
	{
		if (_DivinationTier.ContainsKey(divinationIndex))
		{
			return _DivinationTier[divinationIndex];
		}
		return 0;
	}

	public int GetCostReroll()
	{
		if (AtOManager.Instance.Sandbox_freeRerolls)
		{
			return 0;
		}
		int townTier = AtOManager.Instance.GetTownTier();
		int num = 0;
		num = townTier switch
		{
			0 => 150, 
			1 => 200, 
			2 => 250, 
			_ => 300, 
		};
		int num2 = 4;
		if (GameManager.Instance.IsMultiplayer())
		{
			num2 = 0;
			Hero[] team = AtOManager.Instance.GetTeam();
			if (team != null)
			{
				for (int i = 0; i < 4; i++)
				{
					if (team[i] != null && team[i].Owner == NetworkManager.Instance.GetPlayerNick())
					{
						num2++;
					}
				}
			}
		}
		num *= num2;
		if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_5_4"))
		{
			num -= Functions.FuncRoundToInt((float)num * 0.5f);
		}
		return num;
	}

	public WaitForSeconds WaitForSeconds(float _time)
	{
		if (_WaitForSecondsDict.ContainsKey(_time))
		{
			return _WaitForSecondsDict[_time];
		}
		WaitForSeconds waitForSeconds = new WaitForSeconds(_time);
		_WaitForSecondsDict.Add(_time, waitForSeconds);
		return waitForSeconds;
	}

	public GameObject GetResourceEffect(string _effect)
	{
		if (vfxAux.ContainsKey(_effect))
		{
			return vfxAux[_effect];
		}
		return null;
	}
}
