using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EventReplyData
{
	[Header("Repeat this reply for all characters")]
	[SerializeField]
	private bool repeatForAllCharacters;

	[Header("Repeat this reply for all characters (of this class)")]
	[SerializeField]
	private bool repeatForAllWarriors;

	[SerializeField]
	private bool repeatForAllScouts;

	[SerializeField]
	private bool repeatForAllMages;

	[SerializeField]
	private bool repeatForAllHealers;

	[Header("Requirements")]
	[SerializeField]
	private SubClassData requiredClass;

	[SerializeField]
	private int indexForAnswerTranslation;

	[SerializeField]
	private EventRequirementData requirement;

	[SerializeField]
	private EventRequirementData requirementBlocked;

	[SerializeField]
	private CardData requirementItem;

	public List<CardData> RequirementItems;

	[SerializeField]
	private List<CardData> requirementCard;

	[SerializeField]
	private bool requirementMultiplayer;

	[SerializeField]
	private string requirementSku;

	[Header("Reply")]
	[SerializeField]
	[TextArea]
	private string replyText;

	[SerializeField]
	private Enums.EventAction replyActionText;

	[SerializeField]
	private CardData replyShowCard;

	[Header("Gold Cost")]
	[SerializeField]
	private int goldCost;

	[Header("Dust Cost")]
	[SerializeField]
	private int dustCost;

	[Header("Rolls")]
	[SerializeField]
	private bool ssRoll;

	[SerializeField]
	private int ssRollNumber;

	[SerializeField]
	private int ssRollNumberCritical = -1;

	[SerializeField]
	private int ssRollNumberCriticalFail = -1;

	[SerializeField]
	private Enums.RollMode ssRollMode;

	[SerializeField]
	private Enums.RollTarget ssRollTarget;

	[SerializeField]
	private Enums.CardType ssRollCard;

	[Header("Success")]
	[SerializeField]
	private PerkData ssPerkData;

	[SerializeField]
	private PerkData ssPerkData1;

	[SerializeField]
	private NodeData ssNodeTravel;

	[SerializeField]
	[TextArea]
	private string ssRewardText;

	[SerializeField]
	private float ssRewardHealthPercent;

	[SerializeField]
	private int ssRewardHealthFlat;

	[SerializeField]
	private int ssGoldReward;

	[SerializeField]
	private int ssDustReward;

	[SerializeField]
	private int ssSupplyReward;

	[SerializeField]
	private int ssExperienceReward;

	[SerializeField]
	private EventRequirementData ssRequirementUnlock;

	[SerializeField]
	private EventRequirementData ssRequirementUnlock2;

	[SerializeField]
	private EventRequirementData ssRequirementLock;

	[SerializeField]
	private EventRequirementData ssRequirementLock2;

	[SerializeField]
	private CardData ssAddCard1;

	[SerializeField]
	private CardData ssAddCard2;

	[SerializeField]
	private CardData ssAddCard3;

	[SerializeField]
	private CombatData ssCombat;

	[SerializeField]
	private EventData ssEvent;

	[SerializeField]
	private TierRewardData ssRewardTier;

	[SerializeField]
	private int ssDiscount;

	[SerializeField]
	private int ssMaxQuantity;

	[SerializeField]
	private bool ssHealerUI;

	[SerializeField]
	private bool ssUpgradeUI;

	[SerializeField]
	private bool ssCraftUI;

	[SerializeField]
	private bool ssCorruptionUI;

	[SerializeField]
	private bool ssItemCorruptionUI;

	[SerializeField]
	private Enums.CardRarity ssCraftUIMaxType;

	[SerializeField]
	private bool ssMerchantUI;

	[SerializeField]
	private LootData ssShopList;

	[SerializeField]
	private LootData ssLootList;

	[SerializeField]
	private SubClassData ssUnlockClass;

	[SerializeField]
	private bool ssUpgradeRandomCard;

	[SerializeField]
	private CardData ssAddItem;

	[SerializeField]
	private Enums.ItemSlot ssRemoveItemSlot;

	[SerializeField]
	private Enums.ItemSlot ssCorruptItemSlot;

	[SerializeField]
	private bool ssCardPlayerGame;

	[SerializeField]
	private CardPlayerPackData ssCardPlayerGamePackData;

	[SerializeField]
	private bool ssCardPlayerPairsGame;

	[SerializeField]
	private CardPlayerPairsPackData ssCardPlayerPairsGamePackData;

	[SerializeField]
	private bool ssFinishGame;

	[SerializeField]
	private bool ssFinishEarlyAccess;

	[SerializeField]
	private bool ssFinishObeliskMap;

	[SerializeField]
	private string ssUnlockSteamAchievement;

	[SerializeField]
	private string ssSteamStat;

	[SerializeField]
	private SkinData ssUnlockSkin;

	[Header("Character Replacement")]
	[SerializeField]
	private SubClassData ssCharacterReplacement;

	[SerializeField]
	private int ssCharacterReplacementPosition;

	[Header("Fail")]
	[SerializeField]
	private NodeData flNodeTravel;

	[SerializeField]
	[TextArea]
	private string flRewardText;

	[SerializeField]
	private float flRewardHealthPercent;

	[SerializeField]
	private int flRewardHealthFlat;

	[SerializeField]
	private int flGoldReward;

	[SerializeField]
	private int flDustReward;

	[SerializeField]
	private int flSupplyReward;

	[SerializeField]
	private int flExperienceReward;

	[SerializeField]
	private EventRequirementData flRequirementUnlock;

	[SerializeField]
	private EventRequirementData flRequirementUnlock2;

	[SerializeField]
	private EventRequirementData flRequirementLock;

	[SerializeField]
	private CardData flAddCard1;

	[SerializeField]
	private CardData flAddCard2;

	[SerializeField]
	private CardData flAddCard3;

	[SerializeField]
	private CombatData flCombat;

	[SerializeField]
	private EventData flEvent;

	[SerializeField]
	private TierRewardData flRewardTier;

	[SerializeField]
	private int flDiscount;

	[SerializeField]
	private bool flHealerUI;

	[SerializeField]
	private int flMaxQuantity;

	[SerializeField]
	private bool flUpgradeUI;

	[SerializeField]
	private bool flCraftUI;

	[SerializeField]
	private Enums.CardRarity flCraftUIMaxType;

	[SerializeField]
	private bool flCorruptionUI;

	[SerializeField]
	private bool flItemCorruptionUI;

	[SerializeField]
	private bool flMerchantUI;

	[SerializeField]
	private LootData flShopList;

	[SerializeField]
	private LootData flLootList;

	[SerializeField]
	private bool flUpgradeRandomCard;

	[SerializeField]
	private CardData flAddItem;

	[SerializeField]
	private Enums.ItemSlot flRemoveItemSlot;

	[SerializeField]
	private Enums.ItemSlot flCorruptItemSlot;

	[SerializeField]
	private bool flCardPlayerGame;

	[SerializeField]
	private CardPlayerPackData flCardPlayerGamePackData;

	[SerializeField]
	private bool flCardPlayerPairsGame;

	[SerializeField]
	private CardPlayerPairsPackData flCardPlayerPairsGamePackData;

	[SerializeField]
	private SubClassData flUnlockClass;

	[SerializeField]
	private string flUnlockSteamAchievement;

	[Header("Critical Success")]
	[SerializeField]
	private NodeData sscNodeTravel;

	[SerializeField]
	[TextArea]
	private string sscRewardText;

	[SerializeField]
	private float sscRewardHealthPercent;

	[SerializeField]
	private int sscRewardHealthFlat;

	[SerializeField]
	private int sscGoldReward;

	[SerializeField]
	private int sscDustReward;

	[SerializeField]
	private int sscSupplyReward;

	[SerializeField]
	private int sscExperienceReward;

	[SerializeField]
	private EventRequirementData sscRequirementUnlock;

	[SerializeField]
	private EventRequirementData sscRequirementUnlock2;

	[SerializeField]
	private EventRequirementData sscRequirementLock;

	[SerializeField]
	private CardData sscAddCard1;

	[SerializeField]
	private CardData sscAddCard2;

	[SerializeField]
	private CardData sscAddCard3;

	[SerializeField]
	private CombatData sscCombat;

	[SerializeField]
	private EventData sscEvent;

	[SerializeField]
	private TierRewardData sscRewardTier;

	[SerializeField]
	private int sscDiscount;

	[SerializeField]
	private int sscMaxQuantity;

	[SerializeField]
	private bool sscHealerUI;

	[SerializeField]
	private bool sscUpgradeUI;

	[SerializeField]
	private bool sscCraftUI;

	[SerializeField]
	private Enums.CardRarity sscCraftUIMaxType;

	[SerializeField]
	private bool sscCorruptionUI;

	[SerializeField]
	private bool sscItemCorruptionUI;

	[SerializeField]
	private bool sscMerchantUI;

	[SerializeField]
	private LootData sscShopList;

	[SerializeField]
	private LootData sscLootList;

	[SerializeField]
	private SubClassData sscUnlockClass;

	[SerializeField]
	private bool sscUpgradeRandomCard;

	[SerializeField]
	private CardData sscAddItem;

	[SerializeField]
	private Enums.ItemSlot sscRemoveItemSlot;

	[SerializeField]
	private Enums.ItemSlot sscCorruptItemSlot;

	[SerializeField]
	private bool sscCardPlayerGame;

	[SerializeField]
	private CardPlayerPackData sscCardPlayerGamePackData;

	[SerializeField]
	private bool sscCardPlayerPairsGame;

	[SerializeField]
	private CardPlayerPairsPackData sscCardPlayerPairsGamePackData;

	[SerializeField]
	private bool sscFinishGame;

	[SerializeField]
	private bool sscFinishEarlyAccess;

	[SerializeField]
	private string sscUnlockSteamAchievement;

	[Header("Critical Fail")]
	[SerializeField]
	private NodeData flcNodeTravel;

	[SerializeField]
	[TextArea]
	private string flcRewardText;

	[SerializeField]
	private float flcRewardHealthPercent;

	[SerializeField]
	private int flcRewardHealthFlat;

	[SerializeField]
	private int flcGoldReward;

	[SerializeField]
	private int flcDustReward;

	[SerializeField]
	private int flcSupplyReward;

	[SerializeField]
	private int flcExperienceReward;

	[SerializeField]
	private EventRequirementData flcRequirementUnlock;

	[SerializeField]
	private EventRequirementData flcRequirementUnlock2;

	[SerializeField]
	private EventRequirementData flcRequirementLock;

	[SerializeField]
	private CardData flcAddCard1;

	[SerializeField]
	private CardData flcAddCard2;

	[SerializeField]
	private CardData flcAddCard3;

	[SerializeField]
	private CombatData flcCombat;

	[SerializeField]
	private EventData flcEvent;

	[SerializeField]
	private TierRewardData flcRewardTier;

	[SerializeField]
	private int flcDiscount;

	[SerializeField]
	private bool flcHealerUI;

	[SerializeField]
	private int flcMaxQuantity;

	[SerializeField]
	private bool flcUpgradeUI;

	[SerializeField]
	private bool flcCraftUI;

	[SerializeField]
	private Enums.CardRarity flcCraftUIMaxType;

	[SerializeField]
	private bool flcCorruptionUI;

	[SerializeField]
	private bool flcItemCorruptionUI;

	[SerializeField]
	private bool flcMerchantUI;

	[SerializeField]
	private LootData flcShopList;

	[SerializeField]
	private LootData flcLootList;

	[SerializeField]
	private bool flcUpgradeRandomCard;

	[SerializeField]
	private CardData flcAddItem;

	[SerializeField]
	private Enums.ItemSlot flcRemoveItemSlot;

	[SerializeField]
	private Enums.ItemSlot flcCorruptItemSlot;

	[SerializeField]
	private bool flcCardPlayerGame;

	[SerializeField]
	private CardPlayerPackData flcCardPlayerGamePackData;

	[SerializeField]
	private bool flcCardPlayerPairsGame;

	[SerializeField]
	private CardPlayerPairsPackData flcCardPlayerPairsGamePackData;

	[SerializeField]
	private SubClassData flcUnlockClass;

	[SerializeField]
	private string flcUnlockSteamAchievement;

	public SubClassData RequiredClass
	{
		get
		{
			return requiredClass;
		}
		set
		{
			requiredClass = value;
		}
	}

	public EventRequirementData Requirement
	{
		get
		{
			return requirement;
		}
		set
		{
			requirement = value;
		}
	}

	public EventRequirementData RequirementBlocked
	{
		get
		{
			return requirementBlocked;
		}
		set
		{
			requirementBlocked = value;
		}
	}

	public string ReplyText
	{
		get
		{
			return replyText;
		}
		set
		{
			replyText = value;
		}
	}

	public int GoldCost
	{
		get
		{
			return goldCost;
		}
		set
		{
			goldCost = value;
		}
	}

	public bool SsRoll
	{
		get
		{
			return ssRoll;
		}
		set
		{
			ssRoll = value;
		}
	}

	public int SsRollNumber
	{
		get
		{
			return ssRollNumber;
		}
		set
		{
			ssRollNumber = value;
		}
	}

	public Enums.RollMode SsRollMode
	{
		get
		{
			return ssRollMode;
		}
		set
		{
			ssRollMode = value;
		}
	}

	public Enums.RollTarget SsRollTarget
	{
		get
		{
			return ssRollTarget;
		}
		set
		{
			ssRollTarget = value;
		}
	}

	public string SsRewardText
	{
		get
		{
			return ssRewardText;
		}
		set
		{
			ssRewardText = value;
		}
	}

	public float SsRewardHealthPercent
	{
		get
		{
			return ssRewardHealthPercent;
		}
		set
		{
			ssRewardHealthPercent = value;
		}
	}

	public int SsRewardHealthFlat
	{
		get
		{
			return ssRewardHealthFlat;
		}
		set
		{
			ssRewardHealthFlat = value;
		}
	}

	public int SsGoldReward
	{
		get
		{
			return ssGoldReward;
		}
		set
		{
			ssGoldReward = value;
		}
	}

	public int SsDustReward
	{
		get
		{
			return ssDustReward;
		}
		set
		{
			ssDustReward = value;
		}
	}

	public int SsExperienceReward
	{
		get
		{
			return ssExperienceReward;
		}
		set
		{
			ssExperienceReward = value;
		}
	}

	public EventRequirementData SsRequirementUnlock
	{
		get
		{
			return ssRequirementUnlock;
		}
		set
		{
			ssRequirementUnlock = value;
		}
	}

	public EventRequirementData SsRequirementUnlock2
	{
		get
		{
			return ssRequirementUnlock2;
		}
		set
		{
			ssRequirementUnlock2 = value;
		}
	}

	public EventRequirementData SsRequirementLock
	{
		get
		{
			return ssRequirementLock;
		}
		set
		{
			ssRequirementLock = value;
		}
	}

	public EventRequirementData SsRequirementLock2
	{
		get
		{
			return ssRequirementLock2;
		}
		set
		{
			ssRequirementLock2 = value;
		}
	}

	public CardData SsAddCard1
	{
		get
		{
			return ssAddCard1;
		}
		set
		{
			ssAddCard1 = value;
		}
	}

	public CardData SsAddCard2
	{
		get
		{
			return ssAddCard2;
		}
		set
		{
			ssAddCard2 = value;
		}
	}

	public CardData SsAddCard3
	{
		get
		{
			return ssAddCard3;
		}
		set
		{
			ssAddCard3 = value;
		}
	}

	public CombatData SsCombat
	{
		get
		{
			return ssCombat;
		}
		set
		{
			ssCombat = value;
		}
	}

	public EventData SsEvent
	{
		get
		{
			return ssEvent;
		}
		set
		{
			ssEvent = value;
		}
	}

	public TierRewardData SsRewardTier
	{
		get
		{
			return ssRewardTier;
		}
		set
		{
			ssRewardTier = value;
		}
	}

	public int SsDiscount
	{
		get
		{
			return ssDiscount;
		}
		set
		{
			ssDiscount = value;
		}
	}

	public bool SsHealerUI
	{
		get
		{
			return ssHealerUI;
		}
		set
		{
			ssHealerUI = value;
		}
	}

	public int SsMaxQuantity
	{
		get
		{
			return ssMaxQuantity;
		}
		set
		{
			ssMaxQuantity = value;
		}
	}

	public bool SsUpgradeUI
	{
		get
		{
			return ssUpgradeUI;
		}
		set
		{
			ssUpgradeUI = value;
		}
	}

	public bool SsMerchantUI
	{
		get
		{
			return ssMerchantUI;
		}
		set
		{
			ssMerchantUI = value;
		}
	}

	public Enums.EventAction ReplyActionText
	{
		get
		{
			return replyActionText;
		}
		set
		{
			replyActionText = value;
		}
	}

	public string FlRewardText
	{
		get
		{
			return flRewardText;
		}
		set
		{
			flRewardText = value;
		}
	}

	public float FlRewardHealthPercent
	{
		get
		{
			return flRewardHealthPercent;
		}
		set
		{
			flRewardHealthPercent = value;
		}
	}

	public int FlRewardHealthFlat
	{
		get
		{
			return flRewardHealthFlat;
		}
		set
		{
			flRewardHealthFlat = value;
		}
	}

	public int FlGoldReward
	{
		get
		{
			return flGoldReward;
		}
		set
		{
			flGoldReward = value;
		}
	}

	public int FlDustReward
	{
		get
		{
			return flDustReward;
		}
		set
		{
			flDustReward = value;
		}
	}

	public int FlExperienceReward
	{
		get
		{
			return flExperienceReward;
		}
		set
		{
			flExperienceReward = value;
		}
	}

	public CombatData FlCombat
	{
		get
		{
			return flCombat;
		}
		set
		{
			flCombat = value;
		}
	}

	public EventData FlEvent
	{
		get
		{
			return flEvent;
		}
		set
		{
			flEvent = value;
		}
	}

	public EventRequirementData FlRequirementUnlock
	{
		get
		{
			return flRequirementUnlock;
		}
		set
		{
			flRequirementUnlock = value;
		}
	}

	public EventRequirementData FlRequirementUnlock2
	{
		get
		{
			return flRequirementUnlock2;
		}
		set
		{
			flRequirementUnlock2 = value;
		}
	}

	public EventRequirementData FlRequirementLock
	{
		get
		{
			return flRequirementLock;
		}
		set
		{
			flRequirementLock = value;
		}
	}

	public CardData FlAddCard1
	{
		get
		{
			return flAddCard1;
		}
		set
		{
			flAddCard1 = value;
		}
	}

	public CardData FlAddCard2
	{
		get
		{
			return flAddCard2;
		}
		set
		{
			flAddCard2 = value;
		}
	}

	public CardData FlAddCard3
	{
		get
		{
			return flAddCard3;
		}
		set
		{
			flAddCard3 = value;
		}
	}

	public TierRewardData FlRewardTier
	{
		get
		{
			return flRewardTier;
		}
		set
		{
			flRewardTier = value;
		}
	}

	public int FlDiscount
	{
		get
		{
			return flDiscount;
		}
		set
		{
			flDiscount = value;
		}
	}

	public bool FlHealerUI
	{
		get
		{
			return flHealerUI;
		}
		set
		{
			flHealerUI = value;
		}
	}

	public int FlMaxQuantity
	{
		get
		{
			return flMaxQuantity;
		}
		set
		{
			flMaxQuantity = value;
		}
	}

	public bool FlUpgradeUI
	{
		get
		{
			return flUpgradeUI;
		}
		set
		{
			flUpgradeUI = value;
		}
	}

	public bool FlMerchantUI
	{
		get
		{
			return flMerchantUI;
		}
		set
		{
			flMerchantUI = value;
		}
	}

	public bool RequirementMultiplayer
	{
		get
		{
			return requirementMultiplayer;
		}
		set
		{
			requirementMultiplayer = value;
		}
	}

	public NodeData SsNodeTravel
	{
		get
		{
			return ssNodeTravel;
		}
		set
		{
			ssNodeTravel = value;
		}
	}

	public NodeData FlNodeTravel
	{
		get
		{
			return flNodeTravel;
		}
		set
		{
			flNodeTravel = value;
		}
	}

	public SubClassData FlUnlockClass
	{
		get
		{
			return flUnlockClass;
		}
		set
		{
			flUnlockClass = value;
		}
	}

	public SubClassData SsUnlockClass
	{
		get
		{
			return ssUnlockClass;
		}
		set
		{
			ssUnlockClass = value;
		}
	}

	public bool SsFinishGame
	{
		get
		{
			return ssFinishGame;
		}
		set
		{
			ssFinishGame = value;
		}
	}

	public LootData SsShopList
	{
		get
		{
			return ssShopList;
		}
		set
		{
			ssShopList = value;
		}
	}

	public LootData FlShopList
	{
		get
		{
			return flShopList;
		}
		set
		{
			flShopList = value;
		}
	}

	public LootData SsLootList
	{
		get
		{
			return ssLootList;
		}
		set
		{
			ssLootList = value;
		}
	}

	public LootData FlLootList
	{
		get
		{
			return flLootList;
		}
		set
		{
			flLootList = value;
		}
	}

	public bool SsCraftUI
	{
		get
		{
			return ssCraftUI;
		}
		set
		{
			ssCraftUI = value;
		}
	}

	public bool FlCraftUI
	{
		get
		{
			return flCraftUI;
		}
		set
		{
			flCraftUI = value;
		}
	}

	public bool SsUpgradeRandomCard
	{
		get
		{
			return ssUpgradeRandomCard;
		}
		set
		{
			ssUpgradeRandomCard = value;
		}
	}

	public bool FlUpgradeRandomCard
	{
		get
		{
			return flUpgradeRandomCard;
		}
		set
		{
			flUpgradeRandomCard = value;
		}
	}

	public CardData FlAddItem
	{
		get
		{
			return flAddItem;
		}
		set
		{
			flAddItem = value;
		}
	}

	public CardData SsAddItem
	{
		get
		{
			return ssAddItem;
		}
		set
		{
			ssAddItem = value;
		}
	}

	public int SsSupplyReward
	{
		get
		{
			return ssSupplyReward;
		}
		set
		{
			ssSupplyReward = value;
		}
	}

	public int FlSupplyReward
	{
		get
		{
			return flSupplyReward;
		}
		set
		{
			flSupplyReward = value;
		}
	}

	public bool SsFinishEarlyAccess
	{
		get
		{
			return ssFinishEarlyAccess;
		}
		set
		{
			ssFinishEarlyAccess = value;
		}
	}

	public string FlUnlockSteamAchievement
	{
		get
		{
			return flUnlockSteamAchievement;
		}
		set
		{
			flUnlockSteamAchievement = value;
		}
	}

	public string SsUnlockSteamAchievement
	{
		get
		{
			return ssUnlockSteamAchievement;
		}
		set
		{
			ssUnlockSteamAchievement = value;
		}
	}

	public int SsRollNumberCritical
	{
		get
		{
			return ssRollNumberCritical;
		}
		set
		{
			ssRollNumberCritical = value;
		}
	}

	public int SsRollNumberCriticalFail
	{
		get
		{
			return ssRollNumberCriticalFail;
		}
		set
		{
			ssRollNumberCriticalFail = value;
		}
	}

	public NodeData SscNodeTravel
	{
		get
		{
			return sscNodeTravel;
		}
		set
		{
			sscNodeTravel = value;
		}
	}

	public string SscRewardText
	{
		get
		{
			return sscRewardText;
		}
		set
		{
			sscRewardText = value;
		}
	}

	public float SscRewardHealthPercent
	{
		get
		{
			return sscRewardHealthPercent;
		}
		set
		{
			sscRewardHealthPercent = value;
		}
	}

	public int SscRewardHealthFlat
	{
		get
		{
			return sscRewardHealthFlat;
		}
		set
		{
			sscRewardHealthFlat = value;
		}
	}

	public int SscGoldReward
	{
		get
		{
			return sscGoldReward;
		}
		set
		{
			sscGoldReward = value;
		}
	}

	public int SscDustReward
	{
		get
		{
			return sscDustReward;
		}
		set
		{
			sscDustReward = value;
		}
	}

	public int SscSupplyReward
	{
		get
		{
			return sscSupplyReward;
		}
		set
		{
			sscSupplyReward = value;
		}
	}

	public int SscExperienceReward
	{
		get
		{
			return sscExperienceReward;
		}
		set
		{
			sscExperienceReward = value;
		}
	}

	public EventRequirementData SscRequirementUnlock
	{
		get
		{
			return sscRequirementUnlock;
		}
		set
		{
			sscRequirementUnlock = value;
		}
	}

	public EventRequirementData SscRequirementUnlock2
	{
		get
		{
			return sscRequirementUnlock2;
		}
		set
		{
			sscRequirementUnlock2 = value;
		}
	}

	public EventRequirementData SscRequirementLock
	{
		get
		{
			return sscRequirementLock;
		}
		set
		{
			sscRequirementLock = value;
		}
	}

	public CardData SscAddCard1
	{
		get
		{
			return sscAddCard1;
		}
		set
		{
			sscAddCard1 = value;
		}
	}

	public CardData SscAddCard2
	{
		get
		{
			return sscAddCard2;
		}
		set
		{
			sscAddCard2 = value;
		}
	}

	public CardData SscAddCard3
	{
		get
		{
			return sscAddCard3;
		}
		set
		{
			sscAddCard3 = value;
		}
	}

	public CombatData SscCombat
	{
		get
		{
			return sscCombat;
		}
		set
		{
			sscCombat = value;
		}
	}

	public EventData SscEvent
	{
		get
		{
			return sscEvent;
		}
		set
		{
			sscEvent = value;
		}
	}

	public TierRewardData SscRewardTier
	{
		get
		{
			return sscRewardTier;
		}
		set
		{
			sscRewardTier = value;
		}
	}

	public int SscDiscount
	{
		get
		{
			return sscDiscount;
		}
		set
		{
			sscDiscount = value;
		}
	}

	public int SscMaxQuantity
	{
		get
		{
			return sscMaxQuantity;
		}
		set
		{
			sscMaxQuantity = value;
		}
	}

	public bool SscHealerUI
	{
		get
		{
			return sscHealerUI;
		}
		set
		{
			sscHealerUI = value;
		}
	}

	public bool SscUpgradeUI
	{
		get
		{
			return sscUpgradeUI;
		}
		set
		{
			sscUpgradeUI = value;
		}
	}

	public bool SscCraftUI
	{
		get
		{
			return sscCraftUI;
		}
		set
		{
			sscCraftUI = value;
		}
	}

	public bool SscMerchantUI
	{
		get
		{
			return sscMerchantUI;
		}
		set
		{
			sscMerchantUI = value;
		}
	}

	public LootData SscShopList
	{
		get
		{
			return sscShopList;
		}
		set
		{
			sscShopList = value;
		}
	}

	public LootData SscLootList
	{
		get
		{
			return sscLootList;
		}
		set
		{
			sscLootList = value;
		}
	}

	public SubClassData SscUnlockClass
	{
		get
		{
			return sscUnlockClass;
		}
		set
		{
			sscUnlockClass = value;
		}
	}

	public bool SscUpgradeRandomCard
	{
		get
		{
			return sscUpgradeRandomCard;
		}
		set
		{
			sscUpgradeRandomCard = value;
		}
	}

	public CardData SscAddItem
	{
		get
		{
			return sscAddItem;
		}
		set
		{
			sscAddItem = value;
		}
	}

	public bool SscFinishGame
	{
		get
		{
			return sscFinishGame;
		}
		set
		{
			sscFinishGame = value;
		}
	}

	public bool SscFinishEarlyAccess
	{
		get
		{
			return sscFinishEarlyAccess;
		}
		set
		{
			sscFinishEarlyAccess = value;
		}
	}

	public bool SsFinishObeliskMap
	{
		get
		{
			return ssFinishObeliskMap;
		}
		set
		{
			ssFinishObeliskMap = value;
		}
	}

	public string SscUnlockSteamAchievement
	{
		get
		{
			return sscUnlockSteamAchievement;
		}
		set
		{
			sscUnlockSteamAchievement = value;
		}
	}

	public NodeData FlcNodeTravel
	{
		get
		{
			return flcNodeTravel;
		}
		set
		{
			flcNodeTravel = value;
		}
	}

	public string FlcRewardText
	{
		get
		{
			return flcRewardText;
		}
		set
		{
			flcRewardText = value;
		}
	}

	public float FlcRewardHealthPercent
	{
		get
		{
			return flcRewardHealthPercent;
		}
		set
		{
			flcRewardHealthPercent = value;
		}
	}

	public int FlcRewardHealthFlat
	{
		get
		{
			return flcRewardHealthFlat;
		}
		set
		{
			flcRewardHealthFlat = value;
		}
	}

	public int FlcGoldReward
	{
		get
		{
			return flcGoldReward;
		}
		set
		{
			flcGoldReward = value;
		}
	}

	public int FlcDustReward
	{
		get
		{
			return flcDustReward;
		}
		set
		{
			flcDustReward = value;
		}
	}

	public int FlcSupplyReward
	{
		get
		{
			return flcSupplyReward;
		}
		set
		{
			flcSupplyReward = value;
		}
	}

	public int FlcExperienceReward
	{
		get
		{
			return flcExperienceReward;
		}
		set
		{
			flcExperienceReward = value;
		}
	}

	public EventRequirementData FlcRequirementUnlock
	{
		get
		{
			return flcRequirementUnlock;
		}
		set
		{
			flcRequirementUnlock = value;
		}
	}

	public EventRequirementData FlcRequirementUnlock2
	{
		get
		{
			return flcRequirementUnlock2;
		}
		set
		{
			flcRequirementUnlock2 = value;
		}
	}

	public EventRequirementData FlcRequirementLock
	{
		get
		{
			return flcRequirementLock;
		}
		set
		{
			flcRequirementLock = value;
		}
	}

	public CardData FlcAddCard1
	{
		get
		{
			return flcAddCard1;
		}
		set
		{
			flcAddCard1 = value;
		}
	}

	public CardData FlcAddCard2
	{
		get
		{
			return flcAddCard2;
		}
		set
		{
			flcAddCard2 = value;
		}
	}

	public CardData FlcAddCard3
	{
		get
		{
			return flcAddCard3;
		}
		set
		{
			flcAddCard3 = value;
		}
	}

	public CombatData FlcCombat
	{
		get
		{
			return flcCombat;
		}
		set
		{
			flcCombat = value;
		}
	}

	public EventData FlcEvent
	{
		get
		{
			return flcEvent;
		}
		set
		{
			flcEvent = value;
		}
	}

	public TierRewardData FlcRewardTier
	{
		get
		{
			return flcRewardTier;
		}
		set
		{
			flcRewardTier = value;
		}
	}

	public int FlcDiscount
	{
		get
		{
			return flcDiscount;
		}
		set
		{
			flcDiscount = value;
		}
	}

	public bool FlcHealerUI
	{
		get
		{
			return flcHealerUI;
		}
		set
		{
			flcHealerUI = value;
		}
	}

	public int FlcMaxQuantity
	{
		get
		{
			return flcMaxQuantity;
		}
		set
		{
			flcMaxQuantity = value;
		}
	}

	public bool FlcUpgradeUI
	{
		get
		{
			return flcUpgradeUI;
		}
		set
		{
			flcUpgradeUI = value;
		}
	}

	public bool FlcCraftUI
	{
		get
		{
			return flcCraftUI;
		}
		set
		{
			flcCraftUI = value;
		}
	}

	public bool FlcMerchantUI
	{
		get
		{
			return flcMerchantUI;
		}
		set
		{
			flcMerchantUI = value;
		}
	}

	public LootData FlcShopList
	{
		get
		{
			return flcShopList;
		}
		set
		{
			flcShopList = value;
		}
	}

	public LootData FlcLootList
	{
		get
		{
			return flcLootList;
		}
		set
		{
			flcLootList = value;
		}
	}

	public bool FlcUpgradeRandomCard
	{
		get
		{
			return flcUpgradeRandomCard;
		}
		set
		{
			flcUpgradeRandomCard = value;
		}
	}

	public CardData FlcAddItem
	{
		get
		{
			return flcAddItem;
		}
		set
		{
			flcAddItem = value;
		}
	}

	public SubClassData FlcUnlockClass
	{
		get
		{
			return flcUnlockClass;
		}
		set
		{
			flcUnlockClass = value;
		}
	}

	public string FlcUnlockSteamAchievement
	{
		get
		{
			return flcUnlockSteamAchievement;
		}
		set
		{
			flcUnlockSteamAchievement = value;
		}
	}

	public Enums.CardType SsRollCard
	{
		get
		{
			return ssRollCard;
		}
		set
		{
			ssRollCard = value;
		}
	}

	public CardData ReplyShowCard
	{
		get
		{
			return replyShowCard;
		}
		set
		{
			replyShowCard = value;
		}
	}

	public CardData RequirementItem
	{
		get
		{
			return requirementItem;
		}
		set
		{
			requirementItem = value;
		}
	}

	public bool RepeatForAllCharacters
	{
		get
		{
			return repeatForAllCharacters;
		}
		set
		{
			repeatForAllCharacters = value;
		}
	}

	public int DustCost
	{
		get
		{
			return dustCost;
		}
		set
		{
			dustCost = value;
		}
	}

	public PerkData SsPerkData
	{
		get
		{
			return ssPerkData;
		}
		set
		{
			ssPerkData = value;
		}
	}

	public PerkData SsPerkData1
	{
		get
		{
			return ssPerkData1;
		}
		set
		{
			ssPerkData1 = value;
		}
	}

	public Enums.CardRarity FlcCraftUIMaxType
	{
		get
		{
			return flcCraftUIMaxType;
		}
		set
		{
			flcCraftUIMaxType = value;
		}
	}

	public Enums.CardRarity SscCraftUIMaxType
	{
		get
		{
			return sscCraftUIMaxType;
		}
		set
		{
			sscCraftUIMaxType = value;
		}
	}

	public Enums.CardRarity FlCraftUIMaxType
	{
		get
		{
			return flCraftUIMaxType;
		}
		set
		{
			flCraftUIMaxType = value;
		}
	}

	public Enums.CardRarity SsCraftUIMaxType
	{
		get
		{
			return ssCraftUIMaxType;
		}
		set
		{
			ssCraftUIMaxType = value;
		}
	}

	public bool SsCardPlayerGame
	{
		get
		{
			return ssCardPlayerGame;
		}
		set
		{
			ssCardPlayerGame = value;
		}
	}

	public CardPlayerPackData SsCardPlayerGamePackData
	{
		get
		{
			return ssCardPlayerGamePackData;
		}
		set
		{
			ssCardPlayerGamePackData = value;
		}
	}

	public bool FlCardPlayerGame
	{
		get
		{
			return flCardPlayerGame;
		}
		set
		{
			flCardPlayerGame = value;
		}
	}

	public CardPlayerPackData FlCardPlayerGamePackData
	{
		get
		{
			return flCardPlayerGamePackData;
		}
		set
		{
			flCardPlayerGamePackData = value;
		}
	}

	public bool SscCardPlayerGame
	{
		get
		{
			return sscCardPlayerGame;
		}
		set
		{
			sscCardPlayerGame = value;
		}
	}

	public CardPlayerPackData SscCardPlayerGamePackData
	{
		get
		{
			return sscCardPlayerGamePackData;
		}
		set
		{
			sscCardPlayerGamePackData = value;
		}
	}

	public bool FlcCardPlayerGame
	{
		get
		{
			return flcCardPlayerGame;
		}
		set
		{
			flcCardPlayerGame = value;
		}
	}

	public CardPlayerPackData FlcCardPlayerGamePackData
	{
		get
		{
			return flcCardPlayerGamePackData;
		}
		set
		{
			flcCardPlayerGamePackData = value;
		}
	}

	public bool SsCorruptionUI
	{
		get
		{
			return ssCorruptionUI;
		}
		set
		{
			ssCorruptionUI = value;
		}
	}

	public bool SsItemCorruptionUI
	{
		get
		{
			return ssItemCorruptionUI;
		}
		set
		{
			ssItemCorruptionUI = value;
		}
	}

	public bool FlCorruptionUI
	{
		get
		{
			return flCorruptionUI;
		}
		set
		{
			flCorruptionUI = value;
		}
	}

	public bool FlItemCorruptionUI
	{
		get
		{
			return flItemCorruptionUI;
		}
		set
		{
			flItemCorruptionUI = value;
		}
	}

	public bool SscCorruptionUI
	{
		get
		{
			return sscCorruptionUI;
		}
		set
		{
			sscCorruptionUI = value;
		}
	}

	public bool SscItemCorruptionUI
	{
		get
		{
			return sscItemCorruptionUI;
		}
		set
		{
			sscItemCorruptionUI = value;
		}
	}

	public bool FlcCorruptionUI
	{
		get
		{
			return flcCorruptionUI;
		}
		set
		{
			flcCorruptionUI = value;
		}
	}

	public bool FlcItemCorruptionUI
	{
		get
		{
			return flcItemCorruptionUI;
		}
		set
		{
			flcItemCorruptionUI = value;
		}
	}

	public int IndexForAnswerTranslation
	{
		get
		{
			return indexForAnswerTranslation;
		}
		set
		{
			indexForAnswerTranslation = value;
		}
	}

	public Enums.ItemSlot SsRemoveItemSlot
	{
		get
		{
			return ssRemoveItemSlot;
		}
		set
		{
			ssRemoveItemSlot = value;
		}
	}

	public Enums.ItemSlot SsCorruptItemSlot
	{
		get
		{
			return ssCorruptItemSlot;
		}
		set
		{
			ssCorruptItemSlot = value;
		}
	}

	public Enums.ItemSlot SscRemoveItemSlot
	{
		get
		{
			return sscRemoveItemSlot;
		}
		set
		{
			sscRemoveItemSlot = value;
		}
	}

	public Enums.ItemSlot SscCorruptItemSlot
	{
		get
		{
			return sscCorruptItemSlot;
		}
		set
		{
			sscCorruptItemSlot = value;
		}
	}

	public Enums.ItemSlot FlRemoveItemSlot
	{
		get
		{
			return flRemoveItemSlot;
		}
		set
		{
			flRemoveItemSlot = value;
		}
	}

	public Enums.ItemSlot FlCorruptItemSlot
	{
		get
		{
			return flCorruptItemSlot;
		}
		set
		{
			flCorruptItemSlot = value;
		}
	}

	public Enums.ItemSlot FlcRemoveItemSlot
	{
		get
		{
			return flcRemoveItemSlot;
		}
		set
		{
			flcRemoveItemSlot = value;
		}
	}

	public Enums.ItemSlot FlcCorruptItemSlot
	{
		get
		{
			return flcCorruptItemSlot;
		}
		set
		{
			flcCorruptItemSlot = value;
		}
	}

	public string SsSteamStat
	{
		get
		{
			return ssSteamStat;
		}
		set
		{
			ssSteamStat = value;
		}
	}

	public SubClassData SsCharacterReplacement
	{
		get
		{
			return ssCharacterReplacement;
		}
		set
		{
			ssCharacterReplacement = value;
		}
	}

	public int SsCharacterReplacementPosition
	{
		get
		{
			return ssCharacterReplacementPosition;
		}
		set
		{
			ssCharacterReplacementPosition = value;
		}
	}

	public SkinData SsUnlockSkin
	{
		get
		{
			return ssUnlockSkin;
		}
		set
		{
			ssUnlockSkin = value;
		}
	}

	public string RequirementSku
	{
		get
		{
			return requirementSku;
		}
		set
		{
			requirementSku = value;
		}
	}

	public List<CardData> RequirementCard
	{
		get
		{
			return requirementCard;
		}
		set
		{
			requirementCard = value;
		}
	}

	public bool FlCardPlayerPairsGame
	{
		get
		{
			return flCardPlayerPairsGame;
		}
		set
		{
			flCardPlayerPairsGame = value;
		}
	}

	public CardPlayerPairsPackData FlCardPlayerPairsGamePackData
	{
		get
		{
			return flCardPlayerPairsGamePackData;
		}
		set
		{
			flCardPlayerPairsGamePackData = value;
		}
	}

	public bool FlcCardPlayerPairsGame
	{
		get
		{
			return flcCardPlayerPairsGame;
		}
		set
		{
			flcCardPlayerPairsGame = value;
		}
	}

	public CardPlayerPairsPackData FlcCardPlayerPairsGamePackData
	{
		get
		{
			return flcCardPlayerPairsGamePackData;
		}
		set
		{
			flcCardPlayerPairsGamePackData = value;
		}
	}

	public bool SsCardPlayerPairsGame
	{
		get
		{
			return ssCardPlayerPairsGame;
		}
		set
		{
			ssCardPlayerPairsGame = value;
		}
	}

	public CardPlayerPairsPackData SsCardPlayerPairsGamePackData
	{
		get
		{
			return ssCardPlayerPairsGamePackData;
		}
		set
		{
			ssCardPlayerPairsGamePackData = value;
		}
	}

	public bool SscCardPlayerPairsGame
	{
		get
		{
			return sscCardPlayerPairsGame;
		}
		set
		{
			sscCardPlayerPairsGame = value;
		}
	}

	public CardPlayerPairsPackData SscCardPlayerPairsGamePackData
	{
		get
		{
			return sscCardPlayerPairsGamePackData;
		}
		set
		{
			sscCardPlayerPairsGamePackData = value;
		}
	}

	public bool RepeatForAllWarriors
	{
		get
		{
			return repeatForAllWarriors;
		}
		set
		{
			repeatForAllWarriors = value;
		}
	}

	public bool RepeatForAllScouts
	{
		get
		{
			return repeatForAllScouts;
		}
		set
		{
			repeatForAllScouts = value;
		}
	}

	public bool RepeatForAllMages
	{
		get
		{
			return repeatForAllMages;
		}
		set
		{
			repeatForAllMages = value;
		}
	}

	public bool RepeatForAllHealers
	{
		get
		{
			return repeatForAllHealers;
		}
		set
		{
			repeatForAllHealers = value;
		}
	}

	public EventReplyData ShallowCopy()
	{
		return (EventReplyData)MemberwiseClone();
	}
}
