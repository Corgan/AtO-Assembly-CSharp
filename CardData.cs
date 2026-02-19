using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Cards;
using UnityEngine;
using WebSocketSharp;

[Serializable]
[CreateAssetMenu(fileName = "New Card", menuName = "Card Data", order = 51)]
public class CardData : ScriptableObject
{
	[Serializable]
	public class AuraBuffs
	{
		public AuraCurseData aura;

		public AuraCurseData auraSelf;

		public int auraCharges;

		public bool auraChargesSpecialValueGlobal;

		public bool auraChargesSpecialValue1;

		public bool auraChargesSpecialValue2;
	}

	[Serializable]
	public class CurseDebuffs
	{
		public AuraCurseData curse;

		public AuraCurseData curseSelf;

		public int curseCharges;

		public int curseChargesSides;

		public bool curseChargesSpecialValueGlobal;

		public bool curseChargesSpecialValue1;

		public bool curseChargesSpecialValue2;
	}

	[Serializable]
	public struct CardToGainTypeBasedOnHeroClass
	{
		public Enums.HeroClass heroClass;

		public List<Enums.CardType> cardTypes;
	}

	[Serializable]
	public struct CardToGainListBasedOnHeroClass
	{
		public Enums.HeroClass heroClass;

		public List<CardData> cardsList;
	}

	[Header("General Attributes")]
	[SerializeField]
	private string cardName;

	[SerializeField]
	private string id;

	[SerializeField]
	[HideInInspector]
	private string internalId;

	[SerializeField]
	[HideInInspector]
	private bool visible;

	[SerializeField]
	private string upgradesTo1 = "";

	[SerializeField]
	private string upgradesTo2 = "";

	[SerializeField]
	private Enums.CardUpgraded cardUpgraded;

	[SerializeField]
	private string upgradedFrom = "";

	[SerializeField]
	private string baseCard = "";

	[SerializeField]
	private int cardNumber;

	[SerializeField]
	private SpecialCardEnum specialCardEnum;

	[SerializeField]
	private CardData upgradesToRare;

	[Header("Description and Image")]
	[TextArea]
	[SerializeField]
	private string description = "";

	[SerializeField]
	private string fluff = "";

	[SerializeField]
	private float fluffPercent;

	[SerializeField]
	private string descriptionNormalized = "";

	[SerializeField]
	private string relatedCard;

	[SerializeField]
	private string relatedCard2;

	[SerializeField]
	private string relatedCard3;

	[SerializeField]
	private List<KeyNotesData> keyNotes;

	[SerializeField]
	private Sprite sprite;

	[SerializeField]
	private bool flipSprite;

	[SerializeField]
	private bool showInTome = true;

	[Header("DLC Requeriment")]
	[SerializeField]
	private string sku;

	[Header("Description CUSTOM")]
	[SerializeField]
	private string preDescriptionId = "";

	[SerializeField]
	private string descriptionId = "";

	[SerializeField]
	private string postDescriptionId = "";

	[SerializeField]
	private bool useDescriptionFromCard;

	[SerializeField]
	private string[] preDescriptionArgs;

	[SerializeField]
	private string[] descriptionArgs;

	[SerializeField]
	private string[] postDescriptionArgs;

	[Header("PET Data")]
	[SerializeField]
	private GameObject petModel;

	[SerializeField]
	private bool petFront = true;

	[SerializeField]
	private bool petInvert = true;

	[SerializeField]
	private Vector2 petOffset = new Vector2(0f, 0f);

	[SerializeField]
	private Vector2 petSize = new Vector2(1f, 1f);

	public Enums.ActivePets PetActivation;

	public Enums.DamageType PetBonusDamageType;

	public int PetBonusDamageAmount;

	[Header("PET Effect")]
	[SerializeField]
	private bool isPetAttack;

	[SerializeField]
	private bool isPetCast;

	[Header("Kill PET")]
	[SerializeField]
	private bool killPet;

	[Header("Temporal PET")]
	[SerializeField]
	private bool petTemporal;

	[SerializeField]
	private bool petTemporalAttack;

	[SerializeField]
	private bool petTemporalCast;

	[SerializeField]
	private bool petTemporalMoveToCenter;

	[SerializeField]
	private bool petTemporalMoveToBack;

	[SerializeField]
	private float petTemporalFadeOutDelay;

	[Header("Game Parameters")]
	[SerializeField]
	private int maxInDeck;

	[SerializeField]
	private Enums.CardRarity cardRarity;

	[SerializeField]
	private Enums.CardType cardType;

	[SerializeField]
	private Enums.CardType[] cardTypeAux;

	private List<Enums.CardType> cardTypeList;

	[SerializeField]
	private Enums.CardClass cardClass;

	[SerializeField]
	private ItemData item;

	[SerializeField]
	private ItemData itemEnchantment;

	[SerializeField]
	private int energyCost;

	[SerializeField]
	private int exhaustCounter;

	[SerializeField]
	[HideInInspector]
	private int energyReductionPermanent;

	[SerializeField]
	[HideInInspector]
	private int energyReductionTemporal;

	[SerializeField]
	[HideInInspector]
	private bool energyReductionToZeroPermanent;

	[SerializeField]
	[HideInInspector]
	private bool energyReductionToZeroTemporal;

	[SerializeField]
	[HideInInspector]
	private int energyCostOriginal;

	[SerializeField]
	[HideInInspector]
	private int energyCostForShow;

	[SerializeField]
	private bool playable = true;

	[SerializeField]
	private bool autoplayDraw;

	[SerializeField]
	private bool autoplayEndTurn;

	[Header("Required Conditions")]
	[SerializeField]
	private string effectRequired = "";

	[Header("Custom")]
	[SerializeField]
	private bool vanish;

	[SerializeField]
	private bool innate;

	[SerializeField]
	private bool lazy;

	[SerializeField]
	private bool endTurn;

	[SerializeField]
	private bool moveToCenter;

	[SerializeField]
	private bool corrupted;

	[SerializeField]
	private bool starter;

	[SerializeField]
	private bool onlyInWeekly;

	[SerializeField]
	[HideInInspector]
	private bool modifiedByTrait;

	[Header("Self Kill Hidden (seconds)")]
	[SerializeField]
	private float selfKillHiddenSeconds;

	[Header("Target Data")]
	[SerializeField]
	private Enums.CardTargetType targetType;

	[SerializeField]
	private Enums.CardTargetSide targetSide;

	[SerializeField]
	private Enums.CardTargetPosition targetPosition;

	[Header("Special Value")]
	[SerializeField]
	private Enums.CardSpecialValue specialValueGlobal;

	[SerializeField]
	private AuraCurseData specialAuraCurseNameGlobal;

	[SerializeField]
	private float specialValueModifierGlobal;

	[SerializeField]
	private Enums.CardSpecialValue specialValue1;

	[SerializeField]
	private AuraCurseData specialAuraCurseName1;

	[SerializeField]
	private float specialValueModifier1;

	[SerializeField]
	private Enums.CardSpecialValue specialValue2;

	[SerializeField]
	private AuraCurseData specialAuraCurseName2;

	[SerializeField]
	private float specialValueModifier2;

	[Header("Repeat Cast")]
	[SerializeField]
	private int effectRepeat = 1;

	[SerializeField]
	private float effectRepeatDelay;

	[SerializeField]
	private int effectRepeatEnergyBonus;

	[SerializeField]
	private int effectRepeatMaxBonus;

	[SerializeField]
	private Enums.EffectRepeatTarget effectRepeatTarget;

	[SerializeField]
	private int effectRepeatModificator;

	[Header("Damage")]
	[SerializeField]
	private Enums.DamageType damageType;

	[SerializeField]
	private Enums.DamageType damageTypeOriginal;

	[SerializeField]
	private int damage;

	[SerializeField]
	[HideInInspector]
	private int damagePreCalculated;

	[SerializeField]
	private int damageSides;

	[SerializeField]
	[HideInInspector]
	private int damageSidesPreCalculated;

	[SerializeField]
	private int damageSelf;

	[SerializeField]
	private bool damageSpecialValueGlobal;

	[SerializeField]
	private bool damageSpecialValue1;

	[SerializeField]
	private bool damageSpecialValue2;

	[SerializeField]
	[HideInInspector]
	private int damageSelfPreCalculated;

	[SerializeField]
	[HideInInspector]
	private int damageSelfPreCalculated2;

	[SerializeField]
	private bool ignoreBlock;

	[SerializeField]
	private Enums.DamageType damageType2;

	[SerializeField]
	private Enums.DamageType damageType2Original;

	[SerializeField]
	private int damage2;

	[SerializeField]
	[HideInInspector]
	private int damagePreCalculated2;

	[SerializeField]
	private int damageSides2;

	[SerializeField]
	[HideInInspector]
	private int damageSidesPreCalculated2;

	[SerializeField]
	private int damageSelf2;

	[SerializeField]
	private bool damage2SpecialValueGlobal;

	[SerializeField]
	private bool damage2SpecialValue1;

	[SerializeField]
	private bool damage2SpecialValue2;

	[SerializeField]
	private bool ignoreBlock2;

	[SerializeField]
	private int selfHealthLoss;

	[SerializeField]
	private bool selfHealthLossSpecialGlobal;

	[SerializeField]
	private bool selfHealthLossSpecialValue1;

	[SerializeField]
	private bool selfHealthLossSpecialValue2;

	private int damagePreCalculatedCombined;

	[Header("Damage Energy Bonus")]
	[SerializeField]
	private int damageEnergyBonus;

	[Header("Aura/Curse Energy Bonus")]
	[SerializeField]
	private AuraCurseData acEnergyBonus;

	[SerializeField]
	private int acEnergyBonusQuantity;

	[SerializeField]
	private AuraCurseData acEnergyBonus2;

	[SerializeField]
	private int acEnergyBonus2Quantity;

	[Header("Heal")]
	[SerializeField]
	private int heal;

	[SerializeField]
	[HideInInspector]
	private int healPreCalculated;

	[SerializeField]
	private int healSides;

	[SerializeField]
	private int healSelf;

	[SerializeField]
	[HideInInspector]
	private int healSelfPreCalculated;

	[SerializeField]
	private int healEnergyBonus;

	[SerializeField]
	private float healSelfPerDamageDonePercent;

	[SerializeField]
	private bool healSpecialValueGlobal;

	[SerializeField]
	private bool healSpecialValue1;

	[SerializeField]
	private bool healSpecialValue2;

	[SerializeField]
	private bool healSelfSpecialValueGlobal;

	[SerializeField]
	private bool healSelfSpecialValue1;

	[SerializeField]
	private bool healSelfSpecialValue2;

	[Header("Aura Curse Dispels")]
	[SerializeField]
	private int healCurses;

	[SerializeField]
	private int dispelAuras;

	[SerializeField]
	private int transferCurses;

	[SerializeField]
	private int stealAuras;

	[SerializeField]
	private int reduceCurses;

	[SerializeField]
	private int reduceAuras;

	[SerializeField]
	private int increaseCurses;

	[SerializeField]
	private int increaseAuras;

	[SerializeField]
	private AuraCurseData healAuraCurseSelf;

	[SerializeField]
	private AuraCurseData healAuraCurseName;

	[SerializeField]
	private AuraCurseData healAuraCurseName2;

	[SerializeField]
	private AuraCurseData healAuraCurseName3;

	[SerializeField]
	private AuraCurseData healAuraCurseName4;

	[Header("Energy")]
	[SerializeField]
	private int energyRecharge;

	[SerializeField]
	private bool energyRechargeSpecialValueGlobal;

	[Header("Aura / Buffs")]
	[SerializeField]
	private bool chooseOneOfAvailableAuras;

	[SerializeField]
	private AuraCurseData aura;

	[SerializeField]
	private AuraCurseData auraSelf;

	[SerializeField]
	private int auraCharges;

	[SerializeField]
	private bool auraChargesSpecialValueGlobal;

	[SerializeField]
	private bool auraChargesSpecialValue1;

	[SerializeField]
	private bool auraChargesSpecialValue2;

	[SerializeField]
	private AuraCurseData aura2;

	[SerializeField]
	private AuraCurseData auraSelf2;

	[SerializeField]
	private int auraCharges2;

	[SerializeField]
	private bool auraCharges2SpecialValueGlobal;

	[SerializeField]
	private bool auraCharges2SpecialValue1;

	[SerializeField]
	private bool auraCharges2SpecialValue2;

	[SerializeField]
	private AuraCurseData aura3;

	[SerializeField]
	private AuraCurseData auraSelf3;

	[SerializeField]
	private int auraCharges3;

	[SerializeField]
	private bool auraCharges3SpecialValueGlobal;

	[SerializeField]
	private bool auraCharges3SpecialValue1;

	[SerializeField]
	private bool auraCharges3SpecialValue2;

	[SerializeField]
	private AuraBuffs[] auras;

	[Header("Curse / DeBuffs")]
	[SerializeField]
	private AuraCurseData curse;

	[SerializeField]
	private AuraCurseData curseSelf;

	[SerializeField]
	private int curseCharges;

	[SerializeField]
	private int curseChargesSides;

	[SerializeField]
	private bool curseChargesSpecialValueGlobal;

	[SerializeField]
	private bool curseChargesSpecialValue1;

	[SerializeField]
	private bool curseChargesSpecialValue2;

	[SerializeField]
	private AuraCurseData curse2;

	[SerializeField]
	private AuraCurseData curseSelf2;

	[SerializeField]
	private int curseCharges2;

	[SerializeField]
	private bool curseCharges2SpecialValueGlobal;

	[SerializeField]
	private bool curseCharges2SpecialValue1;

	[SerializeField]
	private bool curseCharges2SpecialValue2;

	[SerializeField]
	private AuraCurseData curse3;

	[SerializeField]
	private AuraCurseData curseSelf3;

	[SerializeField]
	private int curseCharges3;

	[SerializeField]
	private bool curseCharges3SpecialValueGlobal;

	[SerializeField]
	private bool curseCharges3SpecialValue1;

	[SerializeField]
	private bool curseCharges3SpecialValue2;

	[SerializeField]
	private CurseDebuffs[] curses;

	[Header("Character Interation")]
	[SerializeField]
	private int pushTarget;

	[SerializeField]
	private int pullTarget;

	[Header("Card Management")]
	[SerializeField]
	private int drawCard;

	[SerializeField]
	private bool drawCardSpecialValueGlobal;

	[SerializeField]
	private int discardCard;

	[SerializeField]
	private Enums.CardType discardCardType;

	[SerializeField]
	private Enums.CardType[] discardCardTypeAux;

	[SerializeField]
	private bool discardCardAutomatic;

	[SerializeField]
	private Enums.CardPlace discardCardPlace;

	[SerializeField]
	private int addCard;

	[SerializeField]
	private string addCardId = "";

	[SerializeField]
	private Enums.CardType addCardType;

	[SerializeField]
	private bool addCardOnlyCheckAuxTypes;

	[SerializeField]
	public List<CardToGainTypeBasedOnHeroClass> AddCardTypeBasedOnHeroClass;

	[SerializeField]
	private Enums.CardType[] addCardTypeAux;

	[SerializeField]
	public List<CardToGainListBasedOnHeroClass> AddCardListBasedOnHeroClass;

	[SerializeField]
	private CardData[] addCardList;

	public bool AddCardFromVanishPile;

	[SerializeField]
	private int addCardChoose;

	[SerializeField]
	private Enums.CardFrom addCardFrom;

	[SerializeField]
	private Enums.CardPlace addCardPlace;

	[SerializeField]
	private int addCardReducedCost;

	[SerializeField]
	private bool addCardCostTurn;

	[SerializeField]
	private bool addCardVanish;

	[SerializeField]
	private CardIdProvider addCardForModify;

	[SerializeField]
	private CopyConfig copyConfig;

	[Header("Look cards")]
	[SerializeField]
	private int lookCards;

	[SerializeField]
	private int lookCardsDiscardUpTo;

	[SerializeField]
	private int lookCardsVanishUpTo;

	[Header("Vanish cards")]
	[SerializeField]
	private bool addVanishToDeck;

	[Header("Summon Units")]
	[SerializeField]
	private NPCData summonUnit;

	[SerializeField]
	private int summonUnitNum;

	[Header("Summon Unit State")]
	[Tooltip("Clean all their status (f.e. Rebirth into a new creature)")]
	[SerializeField]
	private bool metamorph;

	[Tooltip("Keep all their status/hp%/etc...")]
	[SerializeField]
	private bool evolve;

	[Header("Summon Unit Auras")]
	[SerializeField]
	private AuraCurseData summonAura;

	[SerializeField]
	private int summonAuraCharges;

	[SerializeField]
	private AuraCurseData summonAura2;

	[SerializeField]
	private int summonAuraCharges2;

	[SerializeField]
	private AuraCurseData summonAura3;

	[SerializeField]
	private int summonAuraCharges3;

	[Header("Sounds")]
	[SerializeField]
	private AudioClip sound;

	[SerializeField]
	private AudioClip soundPreAction;

	[SerializeField]
	private AudioClip soundPreActionFemale;

	[Header("Sounds (new)")]
	[SerializeField]
	private AudioClip soundDragRework;

	[SerializeField]
	private AudioClip soundReleaseRework;

	[SerializeField]
	private AudioClip soundHitRework;

	[SerializeField]
	private float soundHitReworkDelay;

	[Header("Sound Release exceptions (new)")]
	[SerializeField]
	private List<SubClassData> srException0Class;

	[SerializeField]
	private AudioClip srException0Audio;

	[SerializeField]
	private List<SubClassData> srException1Class;

	[SerializeField]
	private AudioClip srException1Audio;

	[SerializeField]
	private List<SubClassData> srException2Class;

	[SerializeField]
	private AudioClip srException2Audio;

	[Header("InGame Effects")]
	[SerializeField]
	private string effectPreAction = "";

	[SerializeField]
	private string effectCaster = "";

	[SerializeField]
	private bool effectCasterRepeat;

	[SerializeField]
	private float effectPostCastDelay;

	[SerializeField]
	private bool effectCastCenter;

	[SerializeField]
	private string effectTrail = "";

	[SerializeField]
	private bool effectTrailRepeat;

	[SerializeField]
	private float effectTrailSpeed = 1f;

	[SerializeField]
	private Enums.EffectTrailAngle effectTrailAngle;

	[SerializeField]
	private string effectTarget = "";

	[SerializeField]
	private float effectPostTargetDelay;

	[Header("InGame Effects")]
	[SerializeField]
	private int goldGainQuantity;

	[SerializeField]
	private int shardsGainQuantity;

	[SerializeField]
	[HideInInspector]
	private string target = "";

	[SerializeField]
	[HideInInspector]
	private int enchantDamagePreCalculated1;

	[SerializeField]
	[HideInInspector]
	private int enchantDamagePreCalculated2;

	private string colorUpgradePlain = "5E3016";

	private string colorUpgradeGold = "875700";

	private string colorUpgradeBlue = "215382";

	private string colorUpgradeRare = "7F15A6";

	private string colorUpgradePlainLight = "AE7F65";

	private string colorUpgradeBlueLight = "99DEFB";

	private string colorUpgradeGoldLight = "E5B04D";

	private string colorUpgradeRareLight = "E08BFF";

	private bool _tempAttackSelf;

	public static readonly Dictionary<string, Func<CardData, string>> CardValues = new Dictionary<string, Func<CardData, string>>
	{
		{
			"$DestroyAfterUses",
			(CardData data) => (!data.TryGetItem(out var theItem)) ? string.Empty : theItem.DestroyAfterUses.ToString()
		},
		{
			"$SpecialValueModifierGlobal",
			(CardData data) => data.specialValueModifierGlobal.ToString("F0")
		}
	};

	public bool TempAttackSelf
	{
		get
		{
			return _tempAttackSelf;
		}
		set
		{
			_tempAttackSelf = value;
		}
	}

	public CardIdProvider AddCardForModify => addCardForModify;

	public CopyConfig CopyConfig => copyConfig;

	public SpecialCardEnum SpecialCardEnum
	{
		get
		{
			return specialCardEnum;
		}
		set
		{
			specialCardEnum = value;
		}
	}

	public string CardName
	{
		get
		{
			return cardName;
		}
		set
		{
			cardName = value;
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

	public bool Visible
	{
		get
		{
			return visible;
		}
		set
		{
			visible = value;
		}
	}

	public string UpgradesTo1
	{
		get
		{
			return upgradesTo1;
		}
		set
		{
			upgradesTo1 = value;
		}
	}

	public string UpgradesTo2
	{
		get
		{
			return upgradesTo2;
		}
		set
		{
			upgradesTo2 = value;
		}
	}

	public Enums.CardUpgraded CardUpgraded
	{
		get
		{
			return cardUpgraded;
		}
		set
		{
			cardUpgraded = value;
		}
	}

	public string UpgradedFrom
	{
		get
		{
			return upgradedFrom;
		}
		set
		{
			upgradedFrom = value;
		}
	}

	public string BaseCard
	{
		get
		{
			return baseCard;
		}
		set
		{
			baseCard = value;
		}
	}

	public int CardNumber
	{
		get
		{
			return cardNumber;
		}
		set
		{
			cardNumber = value;
		}
	}

	public string Description
	{
		get
		{
			return description;
		}
		set
		{
			description = value;
		}
	}

	public string Fluff
	{
		get
		{
			return fluff;
		}
		set
		{
			fluff = value;
		}
	}

	public string DescriptionNormalized
	{
		get
		{
			return descriptionNormalized;
		}
		set
		{
			descriptionNormalized = value;
		}
	}

	public List<KeyNotesData> KeyNotes
	{
		get
		{
			return keyNotes;
		}
		set
		{
			keyNotes = value;
		}
	}

	public Sprite Sprite
	{
		get
		{
			return sprite;
		}
		set
		{
			sprite = value;
		}
	}

	public AudioClip Sound
	{
		get
		{
			return sound;
		}
		set
		{
			sound = value;
		}
	}

	public AudioClip SoundPreAction
	{
		get
		{
			return soundPreAction;
		}
		set
		{
			soundPreAction = value;
		}
	}

	public string EffectPreAction
	{
		get
		{
			return effectPreAction;
		}
		set
		{
			effectPreAction = value;
		}
	}

	public string EffectCaster
	{
		get
		{
			return effectCaster;
		}
		set
		{
			effectCaster = value;
		}
	}

	public float EffectPostCastDelay
	{
		get
		{
			return effectPostCastDelay;
		}
		set
		{
			effectPostCastDelay = value;
		}
	}

	public bool EffectCasterRepeat
	{
		get
		{
			return effectCasterRepeat;
		}
		set
		{
			effectCasterRepeat = value;
		}
	}

	public bool EffectCastCenter
	{
		get
		{
			return effectCastCenter;
		}
		set
		{
			effectCastCenter = value;
		}
	}

	public string EffectTrail
	{
		get
		{
			return effectTrail;
		}
		set
		{
			effectTrail = value;
		}
	}

	public bool EffectTrailRepeat
	{
		get
		{
			return effectTrailRepeat;
		}
		set
		{
			effectTrailRepeat = value;
		}
	}

	public float EffectTrailSpeed
	{
		get
		{
			return effectTrailSpeed;
		}
		set
		{
			effectTrailSpeed = value;
		}
	}

	public Enums.EffectTrailAngle EffectTrailAngle
	{
		get
		{
			return effectTrailAngle;
		}
		set
		{
			effectTrailAngle = value;
		}
	}

	public string EffectTarget
	{
		get
		{
			return effectTarget;
		}
		set
		{
			effectTarget = value;
		}
	}

	public int MaxInDeck
	{
		get
		{
			return maxInDeck;
		}
		set
		{
			maxInDeck = value;
		}
	}

	public Enums.CardRarity CardRarity
	{
		get
		{
			return cardRarity;
		}
		set
		{
			cardRarity = value;
		}
	}

	public Enums.CardType CardType
	{
		get
		{
			return cardType;
		}
		set
		{
			cardType = value;
		}
	}

	public Enums.CardType[] CardTypeAux
	{
		get
		{
			return cardTypeAux;
		}
		set
		{
			cardTypeAux = value;
		}
	}

	public Enums.CardClass CardClass
	{
		get
		{
			return cardClass;
		}
		set
		{
			cardClass = value;
		}
	}

	public int EnergyCost
	{
		get
		{
			return energyCost;
		}
		set
		{
			energyCost = value;
		}
	}

	public int EnergyCostOriginal
	{
		get
		{
			return energyCostOriginal;
		}
		set
		{
			energyCostOriginal = value;
		}
	}

	public int EnergyCostForShow
	{
		get
		{
			return energyCostForShow;
		}
		set
		{
			energyCostForShow = value;
		}
	}

	public bool Playable
	{
		get
		{
			return playable;
		}
		set
		{
			playable = value;
		}
	}

	public bool AutoplayDraw
	{
		get
		{
			return autoplayDraw;
		}
		set
		{
			autoplayDraw = value;
		}
	}

	public bool AutoplayEndTurn
	{
		get
		{
			return autoplayEndTurn;
		}
		set
		{
			autoplayEndTurn = value;
		}
	}

	public Enums.CardTargetType TargetType
	{
		get
		{
			return targetType;
		}
		set
		{
			targetType = value;
		}
	}

	public Enums.CardTargetSide TargetSide
	{
		get
		{
			return targetSide;
		}
		set
		{
			targetSide = value;
		}
	}

	public Enums.CardTargetPosition TargetPosition
	{
		get
		{
			return targetPosition;
		}
		set
		{
			targetPosition = value;
		}
	}

	public string EffectRequired
	{
		get
		{
			return effectRequired;
		}
		set
		{
			effectRequired = value;
		}
	}

	public int EffectRepeat
	{
		get
		{
			return effectRepeat;
		}
		set
		{
			effectRepeat = value;
		}
	}

	public float EffectRepeatDelay
	{
		get
		{
			return effectRepeatDelay;
		}
		set
		{
			effectRepeatDelay = value;
		}
	}

	public int EffectRepeatEnergyBonus
	{
		get
		{
			return effectRepeatEnergyBonus;
		}
		set
		{
			effectRepeatEnergyBonus = value;
		}
	}

	public int EffectRepeatMaxBonus
	{
		get
		{
			return effectRepeatMaxBonus;
		}
		set
		{
			effectRepeatMaxBonus = value;
		}
	}

	public Enums.EffectRepeatTarget EffectRepeatTarget
	{
		get
		{
			return effectRepeatTarget;
		}
		set
		{
			effectRepeatTarget = value;
		}
	}

	public int EffectRepeatModificator
	{
		get
		{
			return effectRepeatModificator;
		}
		set
		{
			effectRepeatModificator = value;
		}
	}

	public Enums.DamageType DamageType
	{
		get
		{
			return damageType;
		}
		set
		{
			damageType = value;
		}
	}

	public int Damage
	{
		get
		{
			return damage;
		}
		set
		{
			damage = value;
		}
	}

	public int DamagePreCalculated
	{
		get
		{
			return damagePreCalculated;
		}
		set
		{
			damagePreCalculated = value;
		}
	}

	public int DamageSides
	{
		get
		{
			return damageSides;
		}
		set
		{
			damageSides = value;
		}
	}

	public int DamageSidesPreCalculated
	{
		get
		{
			return damageSidesPreCalculated;
		}
		set
		{
			damageSidesPreCalculated = value;
		}
	}

	public int DamageSelf
	{
		get
		{
			return damageSelf;
		}
		set
		{
			damageSelf = value;
		}
	}

	public int DamageSelfPreCalculated
	{
		get
		{
			return damageSelfPreCalculated;
		}
		set
		{
			damageSelfPreCalculated = value;
		}
	}

	public int DamageSelfPreCalculated2
	{
		get
		{
			return damageSelfPreCalculated2;
		}
		set
		{
			damageSelfPreCalculated2 = value;
		}
	}

	public bool IgnoreBlock
	{
		get
		{
			return ignoreBlock;
		}
		set
		{
			ignoreBlock = value;
		}
	}

	public Enums.DamageType DamageType2
	{
		get
		{
			return damageType2;
		}
		set
		{
			damageType2 = value;
		}
	}

	public int Damage2
	{
		get
		{
			return damage2;
		}
		set
		{
			damage2 = value;
		}
	}

	public int DamagePreCalculated2
	{
		get
		{
			return damagePreCalculated2;
		}
		set
		{
			damagePreCalculated2 = value;
		}
	}

	public int DamageSides2
	{
		get
		{
			return damageSides2;
		}
		set
		{
			damageSides2 = value;
		}
	}

	public int DamageSidesPreCalculated2
	{
		get
		{
			return damageSidesPreCalculated2;
		}
		set
		{
			damageSidesPreCalculated2 = value;
		}
	}

	public int DamageSelf2
	{
		get
		{
			return damageSelf2;
		}
		set
		{
			damageSelf2 = value;
		}
	}

	public bool IgnoreBlock2
	{
		get
		{
			return ignoreBlock2;
		}
		set
		{
			ignoreBlock2 = value;
		}
	}

	public int SelfHealthLoss
	{
		get
		{
			return selfHealthLoss;
		}
		set
		{
			selfHealthLoss = value;
		}
	}

	public int DamageEnergyBonus
	{
		get
		{
			return damageEnergyBonus;
		}
		set
		{
			damageEnergyBonus = value;
		}
	}

	public int Heal
	{
		get
		{
			return heal;
		}
		set
		{
			heal = value;
		}
	}

	public int HealSides
	{
		get
		{
			return healSides;
		}
		set
		{
			healSides = value;
		}
	}

	public int HealSelf
	{
		get
		{
			return healSelf;
		}
		set
		{
			healSelf = value;
		}
	}

	public int HealEnergyBonus
	{
		get
		{
			return healEnergyBonus;
		}
		set
		{
			healEnergyBonus = value;
		}
	}

	public float HealSelfPerDamageDonePercent
	{
		get
		{
			return healSelfPerDamageDonePercent;
		}
		set
		{
			healSelfPerDamageDonePercent = value;
		}
	}

	public int HealCurses
	{
		get
		{
			return healCurses;
		}
		set
		{
			healCurses = value;
		}
	}

	public AuraCurseData HealAuraCurseSelf
	{
		get
		{
			return healAuraCurseSelf;
		}
		set
		{
			healAuraCurseSelf = value;
		}
	}

	public AuraCurseData HealAuraCurseName
	{
		get
		{
			return healAuraCurseName;
		}
		set
		{
			healAuraCurseName = value;
		}
	}

	public AuraCurseData HealAuraCurseName2
	{
		get
		{
			return healAuraCurseName2;
		}
		set
		{
			healAuraCurseName2 = value;
		}
	}

	public AuraCurseData HealAuraCurseName3
	{
		get
		{
			return healAuraCurseName3;
		}
		set
		{
			healAuraCurseName3 = value;
		}
	}

	public AuraCurseData HealAuraCurseName4
	{
		get
		{
			return healAuraCurseName4;
		}
		set
		{
			healAuraCurseName4 = value;
		}
	}

	public int DispelAuras
	{
		get
		{
			return dispelAuras;
		}
		set
		{
			dispelAuras = value;
		}
	}

	public int EnergyRecharge
	{
		get
		{
			return energyRecharge;
		}
		set
		{
			energyRecharge = value;
		}
	}

	public AuraCurseData Aura
	{
		get
		{
			return aura;
		}
		set
		{
			aura = value;
		}
	}

	public AuraCurseData AuraSelf
	{
		get
		{
			return auraSelf;
		}
		set
		{
			auraSelf = value;
		}
	}

	public int AuraCharges
	{
		get
		{
			return auraCharges;
		}
		set
		{
			auraCharges = value;
		}
	}

	public AuraCurseData Aura2
	{
		get
		{
			return aura2;
		}
		set
		{
			aura2 = value;
		}
	}

	public AuraCurseData AuraSelf2
	{
		get
		{
			return auraSelf2;
		}
		set
		{
			auraSelf2 = value;
		}
	}

	public int AuraCharges2
	{
		get
		{
			return auraCharges2;
		}
		set
		{
			auraCharges2 = value;
		}
	}

	public AuraCurseData Aura3
	{
		get
		{
			return aura3;
		}
		set
		{
			aura3 = value;
		}
	}

	public AuraCurseData AuraSelf3
	{
		get
		{
			return auraSelf3;
		}
		set
		{
			auraSelf3 = value;
		}
	}

	public int AuraCharges3
	{
		get
		{
			return auraCharges3;
		}
		set
		{
			auraCharges3 = value;
		}
	}

	public AuraCurseData Curse
	{
		get
		{
			return curse;
		}
		set
		{
			curse = value;
		}
	}

	public AuraCurseData CurseSelf
	{
		get
		{
			return curseSelf;
		}
		set
		{
			curseSelf = value;
		}
	}

	public int CurseCharges
	{
		get
		{
			return curseCharges;
		}
		set
		{
			curseCharges = value;
		}
	}

	public AuraCurseData Curse2
	{
		get
		{
			return curse2;
		}
		set
		{
			curse2 = value;
		}
	}

	public AuraCurseData CurseSelf2
	{
		get
		{
			return curseSelf2;
		}
		set
		{
			curseSelf2 = value;
		}
	}

	public int CurseCharges2
	{
		get
		{
			return curseCharges2;
		}
		set
		{
			curseCharges2 = value;
		}
	}

	public AuraCurseData Curse3
	{
		get
		{
			return curse3;
		}
		set
		{
			curse3 = value;
		}
	}

	public AuraCurseData CurseSelf3
	{
		get
		{
			return curseSelf3;
		}
		set
		{
			curseSelf3 = value;
		}
	}

	public int CurseCharges3
	{
		get
		{
			return curseCharges3;
		}
		set
		{
			curseCharges3 = value;
		}
	}

	public int PushTarget
	{
		get
		{
			return pushTarget;
		}
		set
		{
			pushTarget = value;
		}
	}

	public int PullTarget
	{
		get
		{
			return pullTarget;
		}
		set
		{
			pullTarget = value;
		}
	}

	public int DrawCard
	{
		get
		{
			return drawCard;
		}
		set
		{
			drawCard = value;
		}
	}

	public int DiscardCard
	{
		get
		{
			return discardCard;
		}
		set
		{
			discardCard = value;
		}
	}

	public Enums.CardType DiscardCardType
	{
		get
		{
			return discardCardType;
		}
		set
		{
			discardCardType = value;
		}
	}

	public Enums.CardType[] DiscardCardTypeAux
	{
		get
		{
			return discardCardTypeAux;
		}
		set
		{
			discardCardTypeAux = value;
		}
	}

	public bool DiscardCardAutomatic
	{
		get
		{
			return discardCardAutomatic;
		}
		set
		{
			discardCardAutomatic = value;
		}
	}

	public Enums.CardPlace DiscardCardPlace
	{
		get
		{
			return discardCardPlace;
		}
		set
		{
			discardCardPlace = value;
		}
	}

	public int AddCard
	{
		get
		{
			return addCard;
		}
		set
		{
			addCard = value;
		}
	}

	public string AddCardId
	{
		get
		{
			return addCardId;
		}
		set
		{
			addCardId = value;
		}
	}

	public Enums.CardType AddCardType
	{
		get
		{
			return addCardType;
		}
		set
		{
			addCardType = value;
		}
	}

	public Enums.CardType[] AddCardTypeAux
	{
		get
		{
			return addCardTypeAux;
		}
		set
		{
			addCardTypeAux = value;
		}
	}

	public int AddCardChoose
	{
		get
		{
			return addCardChoose;
		}
		set
		{
			addCardChoose = value;
		}
	}

	public Enums.CardFrom AddCardFrom
	{
		get
		{
			return addCardFrom;
		}
		set
		{
			addCardFrom = value;
		}
	}

	public Enums.CardPlace AddCardPlace
	{
		get
		{
			return addCardPlace;
		}
		set
		{
			addCardPlace = value;
		}
	}

	public int AddCardReducedCost
	{
		get
		{
			return addCardReducedCost;
		}
		set
		{
			addCardReducedCost = value;
		}
	}

	public bool AddCardCostTurn
	{
		get
		{
			return addCardCostTurn;
		}
		set
		{
			addCardCostTurn = value;
		}
	}

	public bool AddCardVanish
	{
		get
		{
			return addCardVanish;
		}
		set
		{
			addCardVanish = value;
		}
	}

	public int LookCards
	{
		get
		{
			return lookCards;
		}
		set
		{
			lookCards = value;
		}
	}

	public int LookCardsDiscardUpTo
	{
		get
		{
			return lookCardsDiscardUpTo;
		}
		set
		{
			lookCardsDiscardUpTo = value;
		}
	}

	public NPCData SummonUnit
	{
		get
		{
			return summonUnit;
		}
		set
		{
			summonUnit = value;
		}
	}

	public int SummonUnitNum
	{
		get
		{
			return summonUnitNum;
		}
		set
		{
			summonUnitNum = value;
		}
	}

	public bool Vanish
	{
		get
		{
			return vanish;
		}
		set
		{
			vanish = value;
		}
	}

	public bool Lazy
	{
		get
		{
			return lazy;
		}
		set
		{
			lazy = value;
		}
	}

	public bool Innate
	{
		get
		{
			return innate;
		}
		set
		{
			innate = value;
		}
	}

	public bool Corrupted
	{
		get
		{
			return corrupted;
		}
		set
		{
			corrupted = value;
		}
	}

	public bool EndTurn
	{
		get
		{
			return endTurn;
		}
		set
		{
			endTurn = value;
		}
	}

	public bool MoveToCenter
	{
		get
		{
			return moveToCenter;
		}
		set
		{
			moveToCenter = value;
		}
	}

	public bool ModifiedByTrait
	{
		get
		{
			return modifiedByTrait;
		}
		set
		{
			modifiedByTrait = value;
		}
	}

	public float EffectPostTargetDelay
	{
		get
		{
			return effectPostTargetDelay;
		}
		set
		{
			effectPostTargetDelay = value;
		}
	}

	public Enums.CardSpecialValue SpecialValueGlobal
	{
		get
		{
			return specialValueGlobal;
		}
		set
		{
			specialValueGlobal = value;
		}
	}

	public float SpecialValueModifierGlobal
	{
		get
		{
			return specialValueModifierGlobal;
		}
		set
		{
			specialValueModifierGlobal = value;
		}
	}

	public Enums.CardSpecialValue SpecialValue1
	{
		get
		{
			return specialValue1;
		}
		set
		{
			specialValue1 = value;
		}
	}

	public float SpecialValueModifier1
	{
		get
		{
			return specialValueModifier1;
		}
		set
		{
			specialValueModifier1 = value;
		}
	}

	public Enums.CardSpecialValue SpecialValue2
	{
		get
		{
			return specialValue2;
		}
		set
		{
			specialValue2 = value;
		}
	}

	public float SpecialValueModifier2
	{
		get
		{
			return specialValueModifier2;
		}
		set
		{
			specialValueModifier2 = value;
		}
	}

	public bool DamageSpecialValueGlobal
	{
		get
		{
			return damageSpecialValueGlobal;
		}
		set
		{
			damageSpecialValueGlobal = value;
		}
	}

	public bool DamageSpecialValue1
	{
		get
		{
			return damageSpecialValue1;
		}
		set
		{
			damageSpecialValue1 = value;
		}
	}

	public bool DamageSpecialValue2
	{
		get
		{
			return damageSpecialValue2;
		}
		set
		{
			damageSpecialValue2 = value;
		}
	}

	public bool Damage2SpecialValueGlobal
	{
		get
		{
			return damage2SpecialValueGlobal;
		}
		set
		{
			damage2SpecialValueGlobal = value;
		}
	}

	public bool Damage2SpecialValue1
	{
		get
		{
			return damage2SpecialValue1;
		}
		set
		{
			damage2SpecialValue1 = value;
		}
	}

	public bool Damage2SpecialValue2
	{
		get
		{
			return damage2SpecialValue2;
		}
		set
		{
			damage2SpecialValue2 = value;
		}
	}

	public AuraCurseData SpecialAuraCurseNameGlobal
	{
		get
		{
			return specialAuraCurseNameGlobal;
		}
		set
		{
			specialAuraCurseNameGlobal = value;
		}
	}

	public AuraCurseData SpecialAuraCurseName1
	{
		get
		{
			return specialAuraCurseName1;
		}
		set
		{
			specialAuraCurseName1 = value;
		}
	}

	public AuraCurseData SpecialAuraCurseName2
	{
		get
		{
			return specialAuraCurseName2;
		}
		set
		{
			specialAuraCurseName2 = value;
		}
	}

	public bool AuraChargesSpecialValue1
	{
		get
		{
			return auraChargesSpecialValue1;
		}
		set
		{
			auraChargesSpecialValue1 = value;
		}
	}

	public bool AuraChargesSpecialValue2
	{
		get
		{
			return auraChargesSpecialValue2;
		}
		set
		{
			auraChargesSpecialValue2 = value;
		}
	}

	public bool AuraChargesSpecialValueGlobal
	{
		get
		{
			return auraChargesSpecialValueGlobal;
		}
		set
		{
			auraChargesSpecialValueGlobal = value;
		}
	}

	public bool AuraCharges2SpecialValue1
	{
		get
		{
			return auraCharges2SpecialValue1;
		}
		set
		{
			auraCharges2SpecialValue1 = value;
		}
	}

	public bool AuraCharges2SpecialValue2
	{
		get
		{
			return auraCharges2SpecialValue2;
		}
		set
		{
			auraCharges2SpecialValue2 = value;
		}
	}

	public bool AuraCharges2SpecialValueGlobal
	{
		get
		{
			return auraCharges2SpecialValueGlobal;
		}
		set
		{
			auraCharges2SpecialValueGlobal = value;
		}
	}

	public bool AuraCharges3SpecialValue1
	{
		get
		{
			return auraCharges3SpecialValue1;
		}
		set
		{
			auraCharges3SpecialValue1 = value;
		}
	}

	public bool AuraCharges3SpecialValue2
	{
		get
		{
			return auraCharges3SpecialValue2;
		}
		set
		{
			auraCharges3SpecialValue2 = value;
		}
	}

	public bool AuraCharges3SpecialValueGlobal
	{
		get
		{
			return auraCharges3SpecialValueGlobal;
		}
		set
		{
			auraCharges3SpecialValueGlobal = value;
		}
	}

	public bool CurseChargesSpecialValue1
	{
		get
		{
			return curseChargesSpecialValue1;
		}
		set
		{
			curseChargesSpecialValue1 = value;
		}
	}

	public bool CurseChargesSpecialValue2
	{
		get
		{
			return curseChargesSpecialValue2;
		}
		set
		{
			curseChargesSpecialValue2 = value;
		}
	}

	public bool CurseChargesSpecialValueGlobal
	{
		get
		{
			return curseChargesSpecialValueGlobal;
		}
		set
		{
			curseChargesSpecialValueGlobal = value;
		}
	}

	public bool CurseCharges2SpecialValue1
	{
		get
		{
			return curseCharges2SpecialValue1;
		}
		set
		{
			curseCharges2SpecialValue1 = value;
		}
	}

	public bool CurseCharges2SpecialValue2
	{
		get
		{
			return curseCharges2SpecialValue2;
		}
		set
		{
			curseCharges2SpecialValue2 = value;
		}
	}

	public bool CurseCharges2SpecialValueGlobal
	{
		get
		{
			return curseCharges2SpecialValueGlobal;
		}
		set
		{
			curseCharges2SpecialValueGlobal = value;
		}
	}

	public bool CurseCharges3SpecialValue1
	{
		get
		{
			return curseCharges3SpecialValue1;
		}
		set
		{
			curseCharges3SpecialValue1 = value;
		}
	}

	public bool CurseCharges3SpecialValue2
	{
		get
		{
			return curseCharges3SpecialValue2;
		}
		set
		{
			curseCharges3SpecialValue2 = value;
		}
	}

	public bool CurseCharges3SpecialValueGlobal
	{
		get
		{
			return curseCharges3SpecialValueGlobal;
		}
		set
		{
			curseCharges3SpecialValueGlobal = value;
		}
	}

	public bool HealSpecialValueGlobal
	{
		get
		{
			return healSpecialValueGlobal;
		}
		set
		{
			healSpecialValueGlobal = value;
		}
	}

	public bool HealSpecialValue1
	{
		get
		{
			return healSpecialValue1;
		}
		set
		{
			healSpecialValue1 = value;
		}
	}

	public bool HealSpecialValue2
	{
		get
		{
			return healSpecialValue2;
		}
		set
		{
			healSpecialValue2 = value;
		}
	}

	public bool SelfHealthLossSpecialGlobal
	{
		get
		{
			return selfHealthLossSpecialGlobal;
		}
		set
		{
			selfHealthLossSpecialGlobal = value;
		}
	}

	public bool SelfHealthLossSpecialValue1
	{
		get
		{
			return selfHealthLossSpecialValue1;
		}
		set
		{
			selfHealthLossSpecialValue1 = value;
		}
	}

	public bool SelfHealthLossSpecialValue2
	{
		get
		{
			return selfHealthLossSpecialValue2;
		}
		set
		{
			selfHealthLossSpecialValue2 = value;
		}
	}

	public float FluffPercent
	{
		get
		{
			return fluffPercent;
		}
		set
		{
			fluffPercent = value;
		}
	}

	public ItemData Item
	{
		get
		{
			return item;
		}
		set
		{
			item = value;
		}
	}

	public AuraCurseData SummonAura
	{
		get
		{
			return summonAura;
		}
		set
		{
			summonAura = value;
		}
	}

	public int SummonAuraCharges
	{
		get
		{
			return summonAuraCharges;
		}
		set
		{
			summonAuraCharges = value;
		}
	}

	public AuraCurseData SummonAura2
	{
		get
		{
			return summonAura2;
		}
		set
		{
			summonAura2 = value;
		}
	}

	public int SummonAuraCharges2
	{
		get
		{
			return summonAuraCharges2;
		}
		set
		{
			summonAuraCharges2 = value;
		}
	}

	public AuraCurseData SummonAura3
	{
		get
		{
			return summonAura3;
		}
		set
		{
			summonAura3 = value;
		}
	}

	public int SummonAuraCharges3
	{
		get
		{
			return summonAuraCharges3;
		}
		set
		{
			summonAuraCharges3 = value;
		}
	}

	public bool HealSelfSpecialValueGlobal
	{
		get
		{
			return healSelfSpecialValueGlobal;
		}
		set
		{
			healSelfSpecialValueGlobal = value;
		}
	}

	public bool HealSelfSpecialValue1
	{
		get
		{
			return healSelfSpecialValue1;
		}
		set
		{
			healSelfSpecialValue1 = value;
		}
	}

	public bool HealSelfSpecialValue2
	{
		get
		{
			return healSelfSpecialValue2;
		}
		set
		{
			healSelfSpecialValue2 = value;
		}
	}

	public GameObject PetModel
	{
		get
		{
			return petModel;
		}
		set
		{
			petModel = value;
		}
	}

	public bool PetFront
	{
		get
		{
			return petFront;
		}
		set
		{
			petFront = value;
		}
	}

	public Vector2 PetOffset
	{
		get
		{
			return petOffset;
		}
		set
		{
			petOffset = value;
		}
	}

	public Vector2 PetSize
	{
		get
		{
			return petSize;
		}
		set
		{
			petSize = value;
		}
	}

	public bool PetInvert
	{
		get
		{
			return petInvert;
		}
		set
		{
			petInvert = value;
		}
	}

	public bool IsPetAttack
	{
		get
		{
			return isPetAttack;
		}
		set
		{
			isPetAttack = value;
		}
	}

	public bool IsPetCast
	{
		get
		{
			return isPetCast;
		}
		set
		{
			isPetCast = value;
		}
	}

	public CardData UpgradesToRare
	{
		get
		{
			return upgradesToRare;
		}
		set
		{
			upgradesToRare = value;
		}
	}

	public int ExhaustCounter
	{
		get
		{
			return exhaustCounter;
		}
		set
		{
			exhaustCounter = value;
		}
	}

	public bool Starter
	{
		get
		{
			return starter;
		}
		set
		{
			starter = value;
		}
	}

	public string Target
	{
		get
		{
			return target;
		}
		set
		{
			target = value;
		}
	}

	public ItemData ItemEnchantment
	{
		get
		{
			return itemEnchantment;
		}
		set
		{
			itemEnchantment = value;
		}
	}

	public CardData[] AddCardList
	{
		get
		{
			return addCardList;
		}
		set
		{
			addCardList = value;
		}
	}

	public bool ShowInTome
	{
		get
		{
			return showInTome;
		}
		set
		{
			showInTome = value;
		}
	}

	public int LookCardsVanishUpTo
	{
		get
		{
			return lookCardsVanishUpTo;
		}
		set
		{
			lookCardsVanishUpTo = value;
		}
	}

	public int TransferCurses
	{
		get
		{
			return transferCurses;
		}
		set
		{
			transferCurses = value;
		}
	}

	public bool KillPet
	{
		get
		{
			return killPet;
		}
		set
		{
			killPet = value;
		}
	}

	public int ReduceCurses
	{
		get
		{
			return reduceCurses;
		}
		set
		{
			reduceCurses = value;
		}
	}

	public AuraCurseData AcEnergyBonus
	{
		get
		{
			return acEnergyBonus;
		}
		set
		{
			acEnergyBonus = value;
		}
	}

	public int AcEnergyBonusQuantity
	{
		get
		{
			return acEnergyBonusQuantity;
		}
		set
		{
			acEnergyBonusQuantity = value;
		}
	}

	public int EnergyReductionPermanent
	{
		get
		{
			return energyReductionPermanent;
		}
		set
		{
			energyReductionPermanent = value;
		}
	}

	public int EnergyReductionTemporal
	{
		get
		{
			return energyReductionTemporal;
		}
		set
		{
			energyReductionTemporal = value;
		}
	}

	public bool EnergyReductionToZeroPermanent
	{
		get
		{
			return energyReductionToZeroPermanent;
		}
		set
		{
			energyReductionToZeroPermanent = value;
		}
	}

	public bool EnergyReductionToZeroTemporal
	{
		get
		{
			return energyReductionToZeroTemporal;
		}
		set
		{
			energyReductionToZeroTemporal = value;
		}
	}

	public AuraCurseData AcEnergyBonus2
	{
		get
		{
			return acEnergyBonus2;
		}
		set
		{
			acEnergyBonus2 = value;
		}
	}

	public int AcEnergyBonus2Quantity
	{
		get
		{
			return acEnergyBonus2Quantity;
		}
		set
		{
			acEnergyBonus2Quantity = value;
		}
	}

	public int StealAuras
	{
		get
		{
			return stealAuras;
		}
		set
		{
			stealAuras = value;
		}
	}

	public bool FlipSprite
	{
		get
		{
			return flipSprite;
		}
		set
		{
			flipSprite = value;
		}
	}

	public AudioClip SoundPreActionFemale
	{
		get
		{
			return soundPreActionFemale;
		}
		set
		{
			soundPreActionFemale = value;
		}
	}

	public int ReduceAuras
	{
		get
		{
			return reduceAuras;
		}
		set
		{
			reduceAuras = value;
		}
	}

	public int IncreaseCurses
	{
		get
		{
			return increaseCurses;
		}
		set
		{
			increaseCurses = value;
		}
	}

	public int IncreaseAuras
	{
		get
		{
			return increaseAuras;
		}
		set
		{
			increaseAuras = value;
		}
	}

	public bool OnlyInWeekly
	{
		get
		{
			return onlyInWeekly;
		}
		set
		{
			onlyInWeekly = value;
		}
	}

	public string RelatedCard
	{
		get
		{
			return relatedCard;
		}
		set
		{
			relatedCard = value;
		}
	}

	public string RelatedCard2
	{
		get
		{
			return relatedCard2;
		}
		set
		{
			relatedCard2 = value;
		}
	}

	public string RelatedCard3
	{
		get
		{
			return relatedCard3;
		}
		set
		{
			relatedCard3 = value;
		}
	}

	public int GoldGainQuantity
	{
		get
		{
			return goldGainQuantity;
		}
		set
		{
			goldGainQuantity = value;
		}
	}

	public int ShardsGainQuantity
	{
		get
		{
			return shardsGainQuantity;
		}
		set
		{
			shardsGainQuantity = value;
		}
	}

	public string Sku
	{
		get
		{
			return sku;
		}
		set
		{
			sku = value;
		}
	}

	public bool EnergyRechargeSpecialValueGlobal
	{
		get
		{
			return energyRechargeSpecialValueGlobal;
		}
		set
		{
			energyRechargeSpecialValueGlobal = value;
		}
	}

	public bool DrawCardSpecialValueGlobal
	{
		get
		{
			return drawCardSpecialValueGlobal;
		}
		set
		{
			drawCardSpecialValueGlobal = value;
		}
	}

	public float SelfKillHiddenSeconds
	{
		get
		{
			return selfKillHiddenSeconds;
		}
		set
		{
			selfKillHiddenSeconds = value;
		}
	}

	public float SoundHitReworkDelay
	{
		get
		{
			return soundHitReworkDelay;
		}
		set
		{
			soundHitReworkDelay = value;
		}
	}

	public bool Evolve
	{
		get
		{
			return evolve;
		}
		set
		{
			evolve = value;
		}
	}

	public bool Metamorph
	{
		get
		{
			return metamorph;
		}
		set
		{
			metamorph = value;
		}
	}

	public bool PetTemporal
	{
		get
		{
			return petTemporal;
		}
		set
		{
			petTemporal = value;
		}
	}

	public bool PetTemporalAttack
	{
		get
		{
			return petTemporalAttack;
		}
		set
		{
			petTemporalAttack = value;
		}
	}

	public bool PetTemporalCast
	{
		get
		{
			return petTemporalCast;
		}
		set
		{
			petTemporalCast = value;
		}
	}

	public bool PetTemporalMoveToCenter
	{
		get
		{
			return petTemporalMoveToCenter;
		}
		set
		{
			petTemporalMoveToCenter = value;
		}
	}

	public bool PetTemporalMoveToBack
	{
		get
		{
			return petTemporalMoveToBack;
		}
		set
		{
			petTemporalMoveToBack = value;
		}
	}

	public float PetTemporalFadeOutDelay
	{
		get
		{
			return petTemporalFadeOutDelay;
		}
		set
		{
			petTemporalFadeOutDelay = value;
		}
	}

	public int CurseChargesSides
	{
		get
		{
			return curseChargesSides;
		}
		set
		{
			curseChargesSides = value;
		}
	}

	public bool ChooseOneOfAvailableAuras
	{
		get
		{
			return chooseOneOfAvailableAuras;
		}
		set
		{
			chooseOneOfAvailableAuras = value;
		}
	}

	public bool AddVanishToDeck
	{
		get
		{
			return addVanishToDeck;
		}
		set
		{
			addVanishToDeck = value;
		}
	}

	public bool AddCardOnlyCheckAuxTypes
	{
		get
		{
			return addCardOnlyCheckAuxTypes;
		}
		set
		{
			addCardOnlyCheckAuxTypes = value;
		}
	}

	public AudioClip SoundDragRework
	{
		get
		{
			return soundDragRework;
		}
		set
		{
			soundDragRework = value;
		}
	}

	public AudioClip SoundReleaseRework
	{
		get
		{
			return soundReleaseRework;
		}
		set
		{
			soundReleaseRework = value;
		}
	}

	public AudioClip SoundHitRework
	{
		get
		{
			return soundHitRework;
		}
		set
		{
			soundHitRework = value;
		}
	}

	public AuraBuffs[] Auras
	{
		get
		{
			return auras;
		}
		set
		{
			auras = value;
		}
	}

	public CurseDebuffs[] Curses
	{
		get
		{
			return curses;
		}
		set
		{
			curses = value;
		}
	}

	public void Init(string newId)
	{
		id = newId;
	}

	public void InitClone(string _internalId)
	{
		id = _internalId;
		internalId = _internalId;
		if (energyCostOriginal == 0)
		{
			energyCostOriginal = energyCost;
		}
		damageTypeOriginal = damageType;
		damageType2Original = damageType2;
		damagePreCalculated = damage;
		damagePreCalculated2 = damage2;
		damageSelfPreCalculated = damageSelf;
		damageSidesPreCalculated = damageSides;
		damageSidesPreCalculated2 = damageSides2;
		damageSelfPreCalculated2 = damageSelf2;
		effectRequired = effectRequired.ToLower();
		if (itemEnchantment != null)
		{
			enchantDamagePreCalculated1 = itemEnchantment.DamageToTarget1;
			enchantDamagePreCalculated2 = itemEnchantment.DamageToTarget2;
		}
		else if (item != null)
		{
			enchantDamagePreCalculated1 = item.DamageToTarget1;
			enchantDamagePreCalculated2 = item.DamageToTarget2;
		}
		healPreCalculated = heal;
		healSelfPreCalculated = healSelf;
		if (innate)
		{
			KeyNotes.Add(Globals.Instance.GetKeyNotesData("innate"));
			Globals.Instance.IncludeInSearch(Texts.Instance.GetText("innate"), id);
		}
		if (vanish)
		{
			KeyNotes.Add(Globals.Instance.GetKeyNotesData("vanish"));
			Globals.Instance.IncludeInSearch(Texts.Instance.GetText("vanish"), id);
		}
		if (energyRecharge > 0 || energyRechargeSpecialValueGlobal)
		{
			KeyNotes.Add(Globals.Instance.GetKeyNotesData("energy"));
			Globals.Instance.IncludeInSearch(Texts.Instance.GetText("energy"), id);
		}
		if (aura != null)
		{
			KeyNotes.Add(Globals.Instance.GetKeyNotesData(aura.Id));
		}
		if (aura2 != null)
		{
			KeyNotes.Add(Globals.Instance.GetKeyNotesData(aura2.Id));
		}
		if (aura3 != null)
		{
			KeyNotes.Add(Globals.Instance.GetKeyNotesData(aura3.Id));
		}
		if (auras != null && auras.Length != 0)
		{
			AuraBuffs[] array = auras;
			foreach (AuraBuffs auraBuffs in array)
			{
				if (auraBuffs != null && auraBuffs.aura != null)
				{
					KeyNotes.Add(Globals.Instance.GetKeyNotesData(auraBuffs.aura.Id));
				}
			}
		}
		if (auraSelf != null)
		{
			KeyNotes.Add(Globals.Instance.GetKeyNotesData(auraSelf.Id));
		}
		if (auraSelf2 != null)
		{
			KeyNotes.Add(Globals.Instance.GetKeyNotesData(auraSelf2.Id));
		}
		if (auraSelf3 != null)
		{
			KeyNotes.Add(Globals.Instance.GetKeyNotesData(auraSelf3.Id));
		}
		if (auras != null && auras.Length != 0)
		{
			AuraBuffs[] array = auras;
			foreach (AuraBuffs auraBuffs2 in array)
			{
				if (auraBuffs2 != null && auraBuffs2.auraSelf != null)
				{
					KeyNotes.Add(Globals.Instance.GetKeyNotesData(auraBuffs2.auraSelf.Id));
				}
			}
		}
		if (curse != null)
		{
			KeyNotes.Add(Globals.Instance.GetKeyNotesData(curse.Id));
		}
		if (curse2 != null)
		{
			KeyNotes.Add(Globals.Instance.GetKeyNotesData(curse2.Id));
		}
		if (curse3 != null)
		{
			KeyNotes.Add(Globals.Instance.GetKeyNotesData(curse3.Id));
		}
		if (curses != null && curses.Length != 0)
		{
			CurseDebuffs[] array2 = curses;
			foreach (CurseDebuffs curseDebuffs in array2)
			{
				if (curseDebuffs != null && curseDebuffs.curse != null)
				{
					KeyNotes.Add(Globals.Instance.GetKeyNotesData(curseDebuffs.curse.Id));
				}
			}
		}
		if (curseSelf != null)
		{
			KeyNotes.Add(Globals.Instance.GetKeyNotesData(curseSelf.Id));
		}
		if (curseSelf2 != null)
		{
			KeyNotes.Add(Globals.Instance.GetKeyNotesData(curseSelf2.Id));
		}
		if (curseSelf3 != null)
		{
			KeyNotes.Add(Globals.Instance.GetKeyNotesData(curseSelf3.Id));
		}
		if (curses != null && curses.Length != 0)
		{
			CurseDebuffs[] array2 = curses;
			foreach (CurseDebuffs curseDebuffs2 in array2)
			{
				if (curseDebuffs2 != null && curseDebuffs2.curseSelf != null)
				{
					KeyNotes.Add(Globals.Instance.GetKeyNotesData(curseDebuffs2.curseSelf.Id));
				}
			}
		}
		if (healAuraCurseSelf != null)
		{
			KeyNotes.Add(Globals.Instance.GetKeyNotesData(healAuraCurseSelf.Id));
		}
		if (healAuraCurseName != null)
		{
			KeyNotes.Add(Globals.Instance.GetKeyNotesData(healAuraCurseName.Id));
		}
		if (healAuraCurseName2 != null)
		{
			KeyNotes.Add(Globals.Instance.GetKeyNotesData(healAuraCurseName2.Id));
		}
		if (healAuraCurseName3 != null)
		{
			KeyNotes.Add(Globals.Instance.GetKeyNotesData(healAuraCurseName3.Id));
		}
		if (healAuraCurseName4 != null)
		{
			KeyNotes.Add(Globals.Instance.GetKeyNotesData(healAuraCurseName4.Id));
		}
	}

	public void InitClone2()
	{
		if (effectRequired != "")
		{
			KeyNotes.Add(Globals.Instance.GetKeyNotesData(effectRequired));
			Globals.Instance.IncludeInSearch(Texts.Instance.GetText(effectRequired), id);
		}
		SetDescriptionNew();
		SetTarget();
		SetRarity();
	}

	public void ModifyDamageType(Enums.DamageType dt = Enums.DamageType.None, Character ch = null)
	{
		if (damageType != Enums.DamageType.None || damageType2 != Enums.DamageType.None)
		{
			_ = damageType;
			if (dt == Enums.DamageType.None)
			{
				damageType = damageTypeOriginal;
				damageType2 = damageType2Original;
			}
			else
			{
				damageType = dt;
				damageType2 = dt;
			}
		}
	}

	public void SetEnchantDamagePrecalculated1(int damage)
	{
		enchantDamagePreCalculated1 = damage;
	}

	public void SetEnchantDamagePrecalculated2(int damage)
	{
		enchantDamagePreCalculated2 = damage;
	}

	public void SetDamagePrecalculated(int damage)
	{
		damagePreCalculated = damage;
	}

	public void SetDamagePrecalculated2(int damage)
	{
		damagePreCalculated2 = damage;
	}

	public void SetDamagePrecalculatedCombined(int damage)
	{
		damagePreCalculatedCombined = damage;
	}

	public void SetDamageSelfPrecalculated(int damage)
	{
		damageSelfPreCalculated = damage;
	}

	public void SetDamageSelfPrecalculated2(int damage)
	{
		damageSelfPreCalculated2 = damage;
	}

	public void SetDamageSidesPrecalculated(int damage)
	{
		damageSidesPreCalculated = damage;
	}

	public void SetDamageSidesPrecalculated2(int damage)
	{
		damageSidesPreCalculated2 = damage;
	}

	public void SetHealPrecalculated(int heal)
	{
		healPreCalculated = heal;
	}

	public void SetHealSelfPrecalculated(int heal)
	{
		healSelfPreCalculated = heal;
	}

	private string ColorTextArray(string type, params string[] text)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<nobr>");
		switch (type)
		{
		case "damage":
			stringBuilder.Append("<color=#B00A00>");
			break;
		case "heal":
			stringBuilder.Append("<color=#1E650F>");
			break;
		case "aura":
			stringBuilder.Append("<color=#263ABC>");
			break;
		case "curse":
			stringBuilder.Append("<color=#720070>");
			break;
		case "system":
			stringBuilder.Append("<color=#5E3016>");
			break;
		default:
			stringBuilder.Append("<color=#5E3016");
			stringBuilder.Append(">");
			break;
		case "":
			break;
		}
		int num = 0;
		foreach (string value in text)
		{
			if (num > 0)
			{
				stringBuilder.Append(" ");
			}
			stringBuilder.Append(value);
			num++;
		}
		if (type != "")
		{
			stringBuilder.Append("</color>");
		}
		stringBuilder.Append("</nobr> ");
		return stringBuilder.ToString();
	}

	private int GetFinalAuraCharges(string acId, int charges, Character character = null, bool returnDefault = false)
	{
		if (character == null || returnDefault)
		{
			return charges;
		}
		return charges + character.GetAuraCurseQuantityModification(acId, cardClass);
	}

	private string SpriteText(string sprite)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string text = sprite.ToLower().Replace(" ", "");
		switch (text)
		{
		case "block":
		case "card":
			stringBuilder.Append("<space=.2>");
			break;
		case "piercing":
			stringBuilder.Append("<space=.4>");
			break;
		case "bleed":
			stringBuilder.Append("<space=.1>");
			break;
		case "bless":
			stringBuilder.Append("<space=.1>");
			break;
		default:
			stringBuilder.Append("<space=.3>");
			break;
		}
		stringBuilder.Append(" <space=-.2>");
		stringBuilder.Append("<size=+.1><sprite name=");
		stringBuilder.Append(text);
		stringBuilder.Append("></size>");
		switch (text)
		{
		case "bleed":
			stringBuilder.Append("<space=-.4>");
			break;
		case "card":
			stringBuilder.Append("<space=-.2>");
			break;
		case "powerful":
		case "fury":
			stringBuilder.Append("<space=-.1>");
			break;
		default:
			stringBuilder.Append("<space=-.2>");
			break;
		case "reinforce":
		case "fire":
			break;
		}
		return stringBuilder.ToString();
	}

	public string GetRequireText()
	{
		string text = "";
		if (effectRequired != "")
		{
			text = string.Format(Texts.Instance.GetText("requireSkill"), Globals.Instance.GetKeyNotesData(effectRequired).KeynoteName);
			if (effectRequired.ToLower() == "stanzai" || effectRequired.ToLower() == "stanzaii")
			{
				text += "+";
			}
		}
		return text;
	}

	private void SetRarity()
	{
		if (cardRarity == Enums.CardRarity.Common)
		{
			Globals.Instance.IncludeInSearch(Texts.Instance.GetText("common"), id);
		}
		else if (cardRarity == Enums.CardRarity.Uncommon)
		{
			Globals.Instance.IncludeInSearch(Texts.Instance.GetText("uncommon"), id);
		}
		else if (cardRarity == Enums.CardRarity.Rare)
		{
			Globals.Instance.IncludeInSearch(Texts.Instance.GetText("rare"), id);
		}
		else if (cardRarity == Enums.CardRarity.Epic)
		{
			Globals.Instance.IncludeInSearch(Texts.Instance.GetText("epic"), id);
		}
		else if (cardRarity == Enums.CardRarity.Mythic)
		{
			Globals.Instance.IncludeInSearch(Texts.Instance.GetText("mythic"), id);
		}
	}

	public void SetTarget()
	{
		if (autoplayDraw)
		{
			target = Texts.Instance.GetText("onDraw");
		}
		else if (autoplayEndTurn)
		{
			target = Texts.Instance.GetText("onEndTurn");
		}
		else if (targetType == Enums.CardTargetType.Global && targetSide == Enums.CardTargetSide.Anyone)
		{
			target = Texts.Instance.GetText("global");
		}
		else if (targetSide == Enums.CardTargetSide.Self)
		{
			target = Texts.Instance.GetText("self");
		}
		else if (targetSide == Enums.CardTargetSide.Anyone && targetPosition == Enums.CardTargetPosition.Random)
		{
			target = Texts.Instance.GetText("random");
		}
		else if (targetType == Enums.CardTargetType.Single && targetSide == Enums.CardTargetSide.Anyone && targetPosition == Enums.CardTargetPosition.Anywhere)
		{
			target = Texts.Instance.GetText("anyone");
		}
		else if (cardClass != Enums.CardClass.Monster)
		{
			if (targetSide == Enums.CardTargetSide.Friend)
			{
				if (targetType == Enums.CardTargetType.Global)
				{
					target = Texts.Instance.GetText("allHeroes");
				}
				else if (targetPosition == Enums.CardTargetPosition.Random)
				{
					target = Texts.Instance.GetText("randomHero");
				}
				else if (targetPosition == Enums.CardTargetPosition.Front)
				{
					target = Texts.Instance.GetText("frontHero");
				}
				else if (targetPosition == Enums.CardTargetPosition.Back)
				{
					target = Texts.Instance.GetText("backHero");
				}
				else
				{
					target = Texts.Instance.GetText("hero");
				}
			}
			else if (targetSide == Enums.CardTargetSide.FriendNotSelf)
			{
				target = Texts.Instance.GetText("otherHero");
			}
			else if (targetSide == Enums.CardTargetSide.Enemy)
			{
				if (targetType == Enums.CardTargetType.Global)
				{
					target = Texts.Instance.GetText("allMonsters");
				}
				else if (targetPosition == Enums.CardTargetPosition.Random)
				{
					target = Texts.Instance.GetText("randomMonster");
				}
				else if (targetPosition == Enums.CardTargetPosition.Front)
				{
					target = Texts.Instance.GetText("frontMonster");
				}
				else if (targetPosition == Enums.CardTargetPosition.Back)
				{
					target = Texts.Instance.GetText("backMonster");
				}
				else
				{
					target = Texts.Instance.GetText("monster");
				}
			}
		}
		else if (cardClass == Enums.CardClass.Monster)
		{
			if (targetSide == Enums.CardTargetSide.Friend)
			{
				if (targetType == Enums.CardTargetType.Global)
				{
					target = Texts.Instance.GetText("allMonsters");
				}
				else if (targetPosition == Enums.CardTargetPosition.Random)
				{
					target = Texts.Instance.GetText("randomMonster");
				}
				else if (targetPosition == Enums.CardTargetPosition.Front)
				{
					target = Texts.Instance.GetText("frontMonster");
				}
				else if (targetPosition == Enums.CardTargetPosition.Back)
				{
					target = Texts.Instance.GetText("backMonster");
				}
				else if (targetPosition == Enums.CardTargetPosition.Middle)
				{
					target = Texts.Instance.GetText("middleMonster");
				}
				else if (targetPosition == Enums.CardTargetPosition.Slowest)
				{
					target = Texts.Instance.GetText("slowestMonster");
				}
				else if (targetPosition == Enums.CardTargetPosition.Fastest)
				{
					target = Texts.Instance.GetText("fastestMonster");
				}
				else if (targetPosition == Enums.CardTargetPosition.LeastHP)
				{
					target = Texts.Instance.GetText("leastHPMonster");
				}
				else if (targetPosition == Enums.CardTargetPosition.MostHP)
				{
					target = Texts.Instance.GetText("mostHPMonster");
				}
				else
				{
					target = Texts.Instance.GetText("monster");
				}
			}
			else if (targetSide == Enums.CardTargetSide.FriendNotSelf)
			{
				target = Texts.Instance.GetText("otherMonster");
			}
			else if (targetSide == Enums.CardTargetSide.Enemy)
			{
				if (targetType == Enums.CardTargetType.Global)
				{
					target = Texts.Instance.GetText("allHeroes");
				}
				else if (targetPosition == Enums.CardTargetPosition.Random)
				{
					target = Texts.Instance.GetText("randomHero");
				}
				else if (targetPosition == Enums.CardTargetPosition.Front)
				{
					target = Texts.Instance.GetText("frontHero");
				}
				else if (targetPosition == Enums.CardTargetPosition.Back)
				{
					target = Texts.Instance.GetText("backHero");
				}
				else if (targetPosition == Enums.CardTargetPosition.Middle)
				{
					target = Texts.Instance.GetText("middleHero");
				}
				else if (targetPosition == Enums.CardTargetPosition.Slowest)
				{
					target = Texts.Instance.GetText("slowestHero");
				}
				else if (targetPosition == Enums.CardTargetPosition.Fastest)
				{
					target = Texts.Instance.GetText("fastestHero");
				}
				else if (targetPosition == Enums.CardTargetPosition.LeastHP)
				{
					target = Texts.Instance.GetText("leastHPHero");
				}
				else if (targetPosition == Enums.CardTargetPosition.MostHP)
				{
					target = Texts.Instance.GetText("mostHPHero");
				}
				else
				{
					target = Texts.Instance.GetText("hero");
				}
			}
		}
		Globals.Instance.IncludeInSearch(target, id);
	}

	public void SetDescriptionNew(bool forceDescription = false, Character character = null, bool includeInSearch = true)
	{
		if (forceDescription || !Globals.Instance.CardsDescriptionNormalized.ContainsKey(id))
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder aux = new StringBuilder();
			string br = "<line-height=15%><br></line-height>";
			string grColor = "<color=#444><size=-.15>";
			string endColor = "</size></color>";
			string goldColor = "<color=#5E3016><size=-.15>";
			if (!string.IsNullOrEmpty(preDescriptionId))
			{
				AddFormattedDescription(stringBuilder, preDescriptionId, preDescriptionArgs);
			}
			if (!string.IsNullOrEmpty(descriptionId))
			{
				AddFormattedDescription(stringBuilder, descriptionId, descriptionArgs);
			}
			else if ((item == null && itemEnchantment == null) || useDescriptionFromCard)
			{
				AppendCardDescription(character, stringBuilder, aux, grColor, endColor, br, goldColor);
			}
			else
			{
				AppendItemDescription(character, stringBuilder, aux, grColor, endColor, goldColor);
			}
			if (!string.IsNullOrEmpty(postDescriptionId))
			{
				AddFormattedDescription(stringBuilder, postDescriptionId, postDescriptionArgs);
			}
			stringBuilder.Replace("<c>", "<color=#5E3016>");
			stringBuilder.Replace("</c>", "</color>");
			stringBuilder.Replace("<nb>", "<nobr>");
			stringBuilder.Replace("</nb>", "</nobr>");
			stringBuilder.Replace("<br1>", "<br><line-height=15%><br></line-height>");
			stringBuilder.Replace("<br2>", "<br><line-height=30%><br></line-height>");
			stringBuilder.Replace("<br3>", "<br><line-height=50%><br></line-height>");
			stringBuilder.Replace("\n", "<br>");
			stringBuilder.Replace("\r>", "<br>");
			descriptionNormalized = stringBuilder.ToString();
			descriptionNormalized = Regex.Replace(descriptionNormalized, "[,][ ]*(<(.*?)>)*(.)", (Match m) => m.ToString().ToLower());
			descriptionNormalized = Regex.Replace(descriptionNormalized, "<br>\\w", (Match m) => m.ToString().ToUpper());
			Globals.Instance.CardsDescriptionNormalized[id] = stringBuilder.ToString();
			if (includeInSearch)
			{
				string input = Regex.Replace(descriptionNormalized, "<sprite name=(.*?)>", (Match m) => Texts.Instance.GetText(m.Groups[1].Value));
				input = Regex.Replace(input, "(<(.*?)>)*", "");
				Globals.Instance.IncludeInSearch(input, id, includeFullTerm: false);
			}
			stringBuilder = null;
			aux = null;
		}
		else
		{
			descriptionNormalized = Globals.Instance.CardsDescriptionNormalized[id];
		}
	}

	private void AddFormattedDescription(StringBuilder builder, string descriptionId, string[] descriptionArgs)
	{
		if (descriptionArgs.Length != 0)
		{
			List<object> list = new List<object>();
			for (int i = 0; i < descriptionArgs.Length; i++)
			{
				string text = descriptionArgs[i].Trim();
				if (CardValues.TryGetValue(text, out var value))
				{
					string text2 = value(this);
					list.Add(text2);
					continue;
				}
				if (!text.StartsWith("$"))
				{
					Debug.LogWarning("Argument \"" + text + "\" not found. Did you mean \"$" + text + "\"?");
				}
				Debug.LogError("Argument \"" + text + "\" not recognized in the description arguments dictionary.");
			}
			string text3 = Texts.Instance.GetText(descriptionId);
			int num = CardUtils.GetMaxPlaceholderFormattedStringIndex(text3) + 1;
			if (list.Count == num)
			{
				string text4 = string.Format(text3, list.ToArray());
				builder.Append(Functions.FormatStringCard(text4));
			}
		}
		else
		{
			builder.Append(Functions.FormatStringCard(Texts.Instance.GetText(descriptionId)));
		}
	}

	private bool TryGetItem(out ItemData theItem)
	{
		if (item != null)
		{
			theItem = item;
			return true;
		}
		if (itemEnchantment != null)
		{
			theItem = itemEnchantment;
			return true;
		}
		theItem = null;
		return false;
	}

	private void AppendItemDescription(Character character, StringBuilder builder, StringBuilder aux, string grColor, string endColor, string goldColor)
	{
		if (!TryGetItem(out var theItem))
		{
			return;
		}
		if (theItem.MaxHealth != 0)
		{
			builder.Append(string.Format(Texts.Instance.GetText("itemMaxHp"), NumFormatItem(theItem.MaxHealth, plus: true)));
			builder.Append("\n");
		}
		if (theItem.ResistModified1 == Enums.DamageType.All)
		{
			builder.Append(string.Format(Texts.Instance.GetText("itemAllResistances"), NumFormatItem(theItem.ResistModifiedValue1, plus: true, percent: true)));
			builder.Append("\n");
		}
		int num = 0;
		int num2 = 0;
		if (theItem.ResistModified1 != Enums.DamageType.None && theItem.ResistModified1 != Enums.DamageType.All)
		{
			aux.Append(SpriteText(Enum.GetName(typeof(Enums.DamageType), theItem.ResistModified1)));
			num2 = theItem.ResistModifiedValue1;
			num++;
		}
		if (theItem.ResistModified2 != Enums.DamageType.None && theItem.ResistModified2 != Enums.DamageType.All)
		{
			aux.Append(SpriteText(Enum.GetName(typeof(Enums.DamageType), theItem.ResistModified2)));
			if (num2 == 0)
			{
				num2 = theItem.ResistModifiedValue2;
			}
			num++;
		}
		if (theItem.ResistModified3 != Enums.DamageType.None && theItem.ResistModified3 != Enums.DamageType.All)
		{
			aux.Append(SpriteText(Enum.GetName(typeof(Enums.DamageType), theItem.ResistModified3)));
			if (num2 == 0)
			{
				num2 = theItem.ResistModifiedValue3;
			}
			num++;
		}
		if (num > 0)
		{
			if (num > 1)
			{
				aux.Append("\n");
			}
			builder.Append(string.Format(Texts.Instance.GetText("itemXResistances"), aux.ToString(), NumFormatItem(num2, plus: true, percent: true)));
			builder.Append("\n");
			aux.Clear();
		}
		aux.Clear();
		if (theItem.CharacterStatModified == Enums.CharacterStat.Speed)
		{
			builder.Append(string.Format(Texts.Instance.GetText("itemSpeed"), NumFormatItem(theItem.CharacterStatModifiedValue, plus: true)));
			builder.Append("\n");
		}
		if (theItem.CharacterStatModified == Enums.CharacterStat.EnergyTurn)
		{
			builder.Append(string.Format(Texts.Instance.GetText("itemEnergyRegeneration"), SpriteText("energy"), NumFormatItem(theItem.CharacterStatModifiedValue, plus: true)));
			builder.Append("\n");
		}
		if (theItem.CharacterStatModified2 == Enums.CharacterStat.Speed)
		{
			builder.Append(string.Format(Texts.Instance.GetText("itemSpeed"), NumFormatItem(theItem.CharacterStatModifiedValue2, plus: true)));
			builder.Append("\n");
		}
		if (theItem.CharacterStatModified2 == Enums.CharacterStat.EnergyTurn)
		{
			builder.Append(string.Format(Texts.Instance.GetText("itemEnergyRegeneration"), SpriteText("energy"), NumFormatItem(theItem.CharacterStatModifiedValue2, plus: true)));
			builder.Append("\n");
		}
		aux.Clear();
		if (theItem.DamageFlatBonus == Enums.DamageType.All)
		{
			builder.Append(string.Format(Texts.Instance.GetText("itemAllDamages"), NumFormatItem(theItem.DamageFlatBonusValue, plus: true)));
			builder.Append("\n");
		}
		num = 0;
		int num3 = 0;
		if (theItem.DamageFlatBonus != Enums.DamageType.None && theItem.DamageFlatBonus != Enums.DamageType.All)
		{
			aux.Append(SpriteText(Enum.GetName(typeof(Enums.DamageType), theItem.DamageFlatBonus)));
			num3 = theItem.DamageFlatBonusValue;
			num++;
		}
		if (theItem.DamageFlatBonus2 != Enums.DamageType.None && theItem.DamageFlatBonus2 != Enums.DamageType.All)
		{
			aux.Append(SpriteText(Enum.GetName(typeof(Enums.DamageType), theItem.DamageFlatBonus2)));
			num++;
		}
		if (theItem.DamageFlatBonus3 != Enums.DamageType.None && theItem.DamageFlatBonus3 != Enums.DamageType.All)
		{
			aux.Append(SpriteText(Enum.GetName(typeof(Enums.DamageType), theItem.DamageFlatBonus3)));
			num++;
		}
		if (theItem.DamagePercentBonus == Enums.DamageType.All)
		{
			builder.Append(string.Format(Texts.Instance.GetText("itemAllDamages"), NumFormatItem(Functions.FuncRoundToInt(theItem.DamagePercentBonusValue), plus: true, percent: true)));
			builder.Append("\n");
		}
		if (num > 0)
		{
			builder.Append(string.Format(Texts.Instance.GetText("itemXDamages"), aux.ToString(), NumFormatItem(num3, plus: true)));
			builder.Append("\n");
			aux.Clear();
		}
		if (theItem.UseTheNextInsteadWhenYouPlay && theItem.HealPercentBonus != 0f)
		{
			string arg = "";
			if (theItem.DestroyAfterUses > 1)
			{
				arg = "(" + theItem.DestroyAfterUses + ") ";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<size=-.15><color=#444>[");
			stringBuilder.Append(Texts.Instance.GetText("itemTheNext"));
			stringBuilder.Append("]</color></size><br>");
			aux.Append("<color=#5E3016>");
			aux.Append(Texts.Instance.GetText(Enum.GetName(typeof(Enums.CardType), theItem.CastedCardType)));
			aux.Append("</color>");
			builder.Append(string.Format(stringBuilder.ToString(), arg, aux.ToString()));
			aux.Clear();
			stringBuilder.Clear();
		}
		if (theItem.HealFlatBonus != 0)
		{
			builder.Append(SpriteText("heal"));
			builder.Append(" ");
			builder.Append(Functions.LowercaseFirst(Texts.Instance.GetText("healDone")));
			builder.Append(NumFormatItem(theItem.HealFlatBonus, plus: true));
			builder.Append("\n");
		}
		if (theItem.HealPercentBonus != 0f)
		{
			builder.Append(SpriteText("heal"));
			builder.Append(" ");
			builder.Append(Functions.LowercaseFirst(Texts.Instance.GetText("healDone")));
			builder.Append(NumFormatItem(Functions.FuncRoundToInt(theItem.HealPercentBonus), plus: true, percent: true));
			builder.Append("\n");
		}
		if (theItem.HealReceivedFlatBonus != 0)
		{
			builder.Append(SpriteText("heal"));
			builder.Append(" ");
			builder.Append(Functions.LowercaseFirst(Texts.Instance.GetText("healTaken")));
			builder.Append(NumFormatItem(Functions.FuncRoundToInt(theItem.HealReceivedFlatBonus), plus: true));
			builder.Append("\n");
		}
		if (theItem.HealReceivedPercentBonus != 0f)
		{
			builder.Append(SpriteText("heal"));
			builder.Append(" ");
			builder.Append(Functions.LowercaseFirst(Texts.Instance.GetText("healTaken")));
			builder.Append(NumFormatItem(Functions.FuncRoundToInt(theItem.HealReceivedPercentBonus), plus: true, percent: true));
			builder.Append("\n");
		}
		aux.Clear();
		if (theItem.AuracurseBonus1 != null && theItem.AuracurseBonusValue1 > 0 && theItem.AuracurseBonus2 != null && theItem.AuracurseBonusValue2 > 0 && theItem.AuracurseBonusValue1 == theItem.AuracurseBonusValue2)
		{
			aux.Append(SpriteText(theItem.AuracurseBonus1.ACName));
			aux.Append(SpriteText(theItem.AuracurseBonus2.ACName));
			builder.Append(string.Format(Texts.Instance.GetText("itemCharges"), aux.ToString(), NumFormatItem(theItem.AuracurseBonusValue1, plus: true)));
			builder.Append("\n");
			aux.Clear();
		}
		else
		{
			if (theItem.AuracurseBonus1 != null && theItem.AuracurseBonusValue1 > 0)
			{
				builder.Append(string.Format(Texts.Instance.GetText("itemCharges"), SpriteText(theItem.AuracurseBonus1.ACName), NumFormatItem(theItem.AuracurseBonusValue1, plus: true)));
				builder.Append("\n");
			}
			if (theItem.AuracurseBonus2 != null && theItem.AuracurseBonusValue2 > 0)
			{
				builder.Append(string.Format(Texts.Instance.GetText("itemCharges"), SpriteText(theItem.AuracurseBonus2.ACName), NumFormatItem(theItem.AuracurseBonusValue2, plus: true)));
				builder.Append("\n");
			}
		}
		num = 0;
		if (theItem.AuracurseImmune1 != null)
		{
			num++;
			aux.Append(ColorTextArray("curse", SpriteText(theItem.AuracurseImmune1.Id)));
		}
		if (theItem.AuracurseImmune2 != null)
		{
			num++;
			aux.Append(ColorTextArray("curse", SpriteText(theItem.AuracurseImmune2.Id)));
		}
		if (num > 0)
		{
			builder.Append(string.Format(Texts.Instance.GetText("cardsImmuneTo"), aux.ToString()));
			builder.Append("\n");
			aux.Clear();
		}
		if (theItem.PercentDiscountShop != 0)
		{
			builder.Append(string.Format(Texts.Instance.GetText("itemDiscount"), NumFormatItem(theItem.PercentDiscountShop, plus: true, percent: true)));
			builder.Append("\n");
		}
		if (theItem.PercentRetentionEndGame != 0)
		{
			builder.Append(string.Format(Texts.Instance.GetText("itemDieRetain"), NumFormatItem(theItem.PercentRetentionEndGame, plus: true, percent: true)));
			builder.Append("\n");
		}
		if (theItem.AuracurseCustomString != "" && theItem.AuracurseCustomAC != null)
		{
			StringBuilder stringBuilder2 = new StringBuilder();
			if ((theItem.AuracurseCustomString == "itemCustomTextMaxChargesIncrasedOnEnemies" || theItem.AuracurseCustomString == "itemCustomTextMaxChargesIncrasedOnHeroes") && theItem.AuracurseCustomModValue1 > 0)
			{
				stringBuilder2.Append("+");
			}
			stringBuilder2.Append(theItem.AuracurseCustomModValue1);
			builder.Append(string.Format(Texts.Instance.GetText(theItem.AuracurseCustomString), ColorTextArray("aura", SpriteText(theItem.AuracurseCustomAC.Id)), stringBuilder2.ToString(), theItem.AuracurseCustomModValue2));
			builder.Append("\n");
			stringBuilder2 = null;
		}
		if (theItem.Id == "harleyrare" || theItem.Id == "templelurkerpetrare" || theItem.Id == "mentalscavengerspetrare")
		{
			builder.Append(Texts.Instance.GetText("immortal"));
			builder.Append("\n");
		}
		if (theItem.ModifiedDamageType != Enums.DamageType.None)
		{
			builder.Append("<nobr>");
			builder.Append(string.Format(Texts.Instance.GetText("cardsTransformDamage"), SpriteText(Enum.GetName(typeof(Enums.DamageType), theItem.ModifiedDamageType))));
			builder.Append("</nobr>");
			builder.Append("\n");
		}
		if (theItem.IsEnchantment && (theItem.CastedCardType != Enums.CardType.None || ((theItem.Activation == Enums.EventActivation.PreFinishCast || theItem.Activation == Enums.EventActivation.FinishCast || theItem.Activation == Enums.EventActivation.FinishFinishCast) && !theItem.EmptyHand)))
		{
			if (theItem.CastedCardType != Enums.CardType.None)
			{
				aux.Append("<color=#5E3016>");
				aux.Append(Texts.Instance.GetText(Enum.GetName(typeof(Enums.CardType), theItem.CastedCardType)));
				aux.Append("</color>");
			}
			else
			{
				aux.Append(" <sprite name=cards>");
			}
			if (theItem.UseTheNextInsteadWhenYouPlay)
			{
				if (theItem.HealPercentBonus == 0f)
				{
					string arg2 = "";
					if (theItem.DestroyAfterUses > 1)
					{
						arg2 = "(" + theItem.DestroyAfterUses + ") ";
					}
					StringBuilder stringBuilder3 = new StringBuilder();
					stringBuilder3.Append("<size=-.15><color=#444>[");
					stringBuilder3.Append(Texts.Instance.GetText("itemTheNext"));
					stringBuilder3.Append("]</color></size><br>");
					builder.Append(string.Format(stringBuilder3.ToString(), arg2, aux.ToString()));
					stringBuilder3 = null;
				}
			}
			else
			{
				builder.Append("<size=-.15>");
				builder.Append("<color=#444>[");
				builder.Append(string.Format(Texts.Instance.GetText("itemWhenYouPlay"), aux.ToString()));
				builder.Append("]</color>");
				builder.Append("</size><br>");
			}
			aux.Clear();
		}
		aux.Clear();
		if (theItem.Activation != Enums.EventActivation.None && theItem.Activation != Enums.EventActivation.PreBeginCombat)
		{
			if (builder.Length > 0)
			{
				builder.Append("<line-height=15%><br></line-height>");
			}
			StringBuilder stringBuilder4 = new StringBuilder();
			if (theItem.TimesPerTurn == 1)
			{
				stringBuilder4.Append(Texts.Instance.GetText("itemOncePerTurn"));
			}
			else if (theItem.TimesPerTurn == 2)
			{
				stringBuilder4.Append(Texts.Instance.GetText("itemTwicePerTurn"));
			}
			else if (theItem.TimesPerTurn == 3)
			{
				stringBuilder4.Append(Texts.Instance.GetText("itemThricePerTurn"));
			}
			else if (theItem.TimesPerTurn == 4)
			{
				stringBuilder4.Append(Texts.Instance.GetText("itemFourPerTurn"));
			}
			else if (theItem.TimesPerTurn == 5)
			{
				stringBuilder4.Append(Texts.Instance.GetText("itemFivePerTurn"));
			}
			else if (theItem.TimesPerTurn == 6)
			{
				stringBuilder4.Append(Texts.Instance.GetText("itemSixPerTurn"));
			}
			else if (theItem.TimesPerTurn == 7)
			{
				stringBuilder4.Append(Texts.Instance.GetText("itemSevenPerTurn"));
			}
			else if (theItem.TimesPerTurn == 8)
			{
				stringBuilder4.Append(Texts.Instance.GetText("itemEightPerTurn"));
			}
			StringBuilder stringBuilder5 = new StringBuilder();
			if (theItem.Activation == Enums.EventActivation.BeginCombat)
			{
				stringBuilder5.Append(Texts.Instance.GetText("itemCombatStart"));
			}
			else if (theItem.Activation == Enums.EventActivation.BeginCombatEnd)
			{
				stringBuilder5.Append(Texts.Instance.GetText("itemCombatEnd"));
			}
			else if (theItem.Activation == Enums.EventActivation.BeginTurnAboutToDealCards || theItem.Activation == Enums.EventActivation.BeginTurnCardsDealt)
			{
				if (theItem.RoundCycle > 1)
				{
					stringBuilder5.Append(string.Format(Texts.Instance.GetText("itemEveryNRounds"), theItem.RoundCycle.ToString()));
				}
				else if (theItem.ExactRound == 1)
				{
					stringBuilder5.Append(Texts.Instance.GetText("itemFirstTurn"));
				}
				else
				{
					stringBuilder5.Append(Texts.Instance.GetText("itemEveryRound"));
				}
			}
			else if (theItem.Activation == Enums.EventActivation.Damage)
			{
				stringBuilder5.Append(Texts.Instance.GetText("itemDamageDone"));
			}
			else if (theItem.Activation == Enums.EventActivation.OnLeechExplosion)
			{
				stringBuilder5.Append(Texts.Instance.GetText("itemLeechExplosion"));
			}
			else if (theItem.Activation == Enums.EventActivation.Damaged)
			{
				if (theItem.LowerOrEqualPercentHP < 100f)
				{
					stringBuilder5.Append(string.Format(Texts.Instance.GetText("itemWhenDamagedBelow"), theItem.LowerOrEqualPercentHP + (Functions.SpaceBeforePercentSign() ? " " : "") + "%"));
				}
				else
				{
					stringBuilder5.Append(Texts.Instance.GetText("itemWhenDamaged"));
				}
			}
			else if (theItem.Activation == Enums.EventActivation.Hitted)
			{
				stringBuilder5.Append(Texts.Instance.GetText("itemWhenHitted"));
			}
			else if (theItem.Activation == Enums.EventActivation.Block)
			{
				stringBuilder5.Append(Texts.Instance.GetText("itemWhenBlock"));
			}
			else if (theItem.Activation == Enums.EventActivation.Heal)
			{
				stringBuilder5.Append(Texts.Instance.GetText("itemHealDoneAction"));
			}
			else if (theItem.Activation == Enums.EventActivation.Healed)
			{
				stringBuilder5.Append(Texts.Instance.GetText("itemWhenHealed"));
			}
			else if (theItem.Activation == Enums.EventActivation.Evaded)
			{
				stringBuilder5.Append(Texts.Instance.GetText("itemWhenEvaded"));
			}
			else if (theItem.Activation == Enums.EventActivation.Evade)
			{
				stringBuilder5.Append(Texts.Instance.GetText("itemWhenEvade"));
			}
			else if (theItem.Activation == Enums.EventActivation.BeginRound)
			{
				if (theItem.RoundCycle > 1)
				{
					stringBuilder5.Append(string.Format(Texts.Instance.GetText("itemEveryRoundRoundN"), theItem.RoundCycle.ToString()));
				}
				else
				{
					stringBuilder5.Append(Texts.Instance.GetText("itemEveryRoundRound"));
				}
			}
			else if (theItem.Activation == Enums.EventActivation.BeginTurn || theItem.Activation == Enums.EventActivation.AfterDealCards)
			{
				if (theItem.RoundCycle > 1)
				{
					stringBuilder5.Append(string.Format(Texts.Instance.GetText("itemEveryNRounds"), theItem.RoundCycle.ToString()));
				}
				else
				{
					stringBuilder5.Append(Texts.Instance.GetText("itemEveryRound"));
				}
			}
			else if (theItem.Activation == Enums.EventActivation.Killed)
			{
				stringBuilder5.Append(Texts.Instance.GetText("itemWhenKilled"));
			}
			else if (theItem.Activation == Enums.EventActivation.BlockReachedZero)
			{
				stringBuilder5.Append(Texts.Instance.GetText("itemWhenBlockReachesZero"));
			}
			else if (theItem.AuraCurseSetted != null && theItem.AuraCurseNumForOneEvent == 0)
			{
				string text = SpriteText(theItem.AuraCurseSetted.Id);
				if ((bool)theItem.AuraCurseSetted2)
				{
					text += SpriteText(theItem.AuraCurseSetted2.Id);
					if ((bool)theItem.AuraCurseSetted3)
					{
						text += SpriteText(theItem.AuraCurseSetted3.Id);
					}
				}
				stringBuilder5.Append(string.Format(Texts.Instance.GetText("itemWhenYouApply"), ColorTextArray("curse", text)));
			}
			if (stringBuilder5.Length > 0)
			{
				builder.Append("<size=-.15>");
				builder.Append("<color=#444>[");
				builder.Append(stringBuilder5.ToString());
				builder.Append("]</color>");
				builder.Append("</size><br>");
			}
			stringBuilder5 = null;
			if (stringBuilder4.Length > 0)
			{
				builder.Append("<size=-.15>");
				builder.Append("<color=#444>[");
				builder.Append(stringBuilder4.ToString());
				builder.Append("]</color>");
				builder.Append("</size><br>");
			}
			stringBuilder4 = null;
			if (theItem.UsedEnergy)
			{
				builder.Append(string.Format(Texts.Instance.GetText("itemApplyForEnergyUsed"), ColorTextArray("system", SpriteText("energy"))));
				builder.Append("\n");
			}
			if (theItem.EmptyHand)
			{
				builder.Append(Texts.Instance.GetText("itemWhenHandEmpty"));
				builder.Append(":<br>");
			}
			if (theItem.ChanceToDispel > 0)
			{
				if (theItem.ChanceToDispelNum > 0)
				{
					if (theItem.ChanceToDispel < 100)
					{
						builder.Append(string.Format(Texts.Instance.GetText("itemChanceToDispel"), ColorTextArray("aura", NumFormatItem(theItem.ChanceToDispel, plus: false, percent: true)), ColorTextArray("curse", NumFormatItem(theItem.ChanceToDispelNum))));
					}
					else
					{
						builder.Append(string.Format(Texts.Instance.GetText("cardsDispel"), ColorTextArray("curse", NumFormatItem(theItem.ChanceToDispelNum))));
					}
				}
				else if (theItem.ChanceToDispelNum == -1)
				{
					builder.Append(string.Format(Texts.Instance.GetText("cardsDispelAll")));
				}
				builder.Append("\n");
			}
			if (theItem.ChanceToPurge > 0)
			{
				if (theItem.ChanceToPurgeNum > 0)
				{
					if (theItem.ChanceToPurge < 100)
					{
						builder.Append(string.Format(Texts.Instance.GetText("itemChanceToPurge"), ColorTextArray("aura", NumFormatItem(theItem.ChanceToPurge, plus: false, percent: true)), ColorTextArray("curse", NumFormatItem(theItem.ChanceToPurgeNum))));
					}
					else
					{
						builder.Append(string.Format(Texts.Instance.GetText("cardsPurge"), ColorTextArray("curse", NumFormatItem(theItem.ChanceToPurgeNum))));
					}
				}
				else if (theItem.ChanceToDispelNum == -1)
				{
					builder.Append(string.Format(Texts.Instance.GetText("cardsPurgeAll")));
				}
				builder.Append("\n");
			}
			if (theItem.ChanceToDispelSelf > 0 && theItem.ChanceToDispelNumSelf > 0)
			{
				if (theItem.ChanceToDispelSelf < 100)
				{
					builder.Append(string.Format(Texts.Instance.GetText("itemChanceToDispelSelf"), ColorTextArray("aura", NumFormatItem(theItem.ChanceToDispelSelf, plus: false, percent: true)), ColorTextArray("curse", NumFormatItem(theItem.ChanceToDispelNumSelf))));
				}
				else
				{
					builder.Append(string.Format(Texts.Instance.GetText("cardsDispelSelf"), ColorTextArray("curse", NumFormatItem(theItem.ChanceToDispelNumSelf))));
				}
				builder.Append("\n");
			}
			if (!theItem.IsEnchantment && (theItem.Activation == Enums.EventActivation.PreFinishCast || theItem.Activation == Enums.EventActivation.FinishCast || theItem.Activation == Enums.EventActivation.FinishFinishCast) && !theItem.EmptyHand)
			{
				StringBuilder stringBuilder6 = new StringBuilder();
				if (theItem.CastedCardType != Enums.CardType.None)
				{
					stringBuilder6.Append("<color=#5E3016>");
					stringBuilder6.Append(Texts.Instance.GetText(Enum.GetName(typeof(Enums.CardType), theItem.CastedCardType)));
					stringBuilder6.Append("</color>");
				}
				else
				{
					stringBuilder6.Append(" <sprite name=cards>");
				}
				builder.Append("<size=-.15>");
				builder.Append("<color=#444>[");
				builder.Append(string.Format(Texts.Instance.GetText("itemWhenYouPlay"), stringBuilder6.ToString()));
				builder.Append("]</color>");
				builder.Append("</size><br>");
			}
			else if (theItem.IsEnchantment && theItem.CastedCardType == Enums.CardType.None && theItem.Activation == Enums.EventActivation.CastCard)
			{
				builder.Append(string.Format(Texts.Instance.GetText("itemWhenYouPlay"), "  <sprite name=cards>"));
				builder.Append(":\n");
			}
			if (theItem.DrawCards > 0)
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsDraw"), ColorTextArray("", NumFormat(theItem.DrawCards), SpriteText("card"))));
				builder.Append("<br>");
			}
			if (theItem.HealQuantitySpecialValue.Use)
			{
				aux.Append("<color=#111111>X</color>");
				if (theItem.ItemTarget == Enums.ItemTarget.AllHero)
				{
					builder.Append(string.Format(Texts.Instance.GetText("itemRecoverHeroes"), aux.ToString()));
				}
				else if (theItem.ItemTarget == Enums.ItemTarget.Self)
				{
					if (theItem.Activation == Enums.EventActivation.Killed)
					{
						builder.Append(string.Format(Texts.Instance.GetText("itemResurrectHP"), aux.ToString()));
					}
					else
					{
						builder.Append(string.Format(Texts.Instance.GetText("itemRecoverSelf"), aux.ToString()));
					}
				}
				else if (theItem.ItemTarget == Enums.ItemTarget.AllEnemy)
				{
					builder.Append(string.Format(Texts.Instance.GetText("itemRecoverMonsters"), aux.ToString()));
				}
				builder.Append("<br>");
				aux.Clear();
			}
			else if (theItem.HealQuantity > 0)
			{
				aux.Append("<color=#111111>");
				aux.Append(NumFormatItem(theItem.HealQuantity, plus: true));
				aux.Append("</color>");
				if (theItem.ItemTarget == Enums.ItemTarget.AllHero)
				{
					builder.Append(string.Format(Texts.Instance.GetText("itemRecoverHeroes"), aux.ToString()));
				}
				else if (theItem.ItemTarget == Enums.ItemTarget.Self)
				{
					if (theItem.Activation == Enums.EventActivation.Killed)
					{
						builder.Append(string.Format(Texts.Instance.GetText("itemResurrectHP"), aux.ToString()));
					}
					else
					{
						builder.Append(string.Format(Texts.Instance.GetText("itemRecoverSelf"), aux.ToString()));
					}
				}
				else if (theItem.ItemTarget == Enums.ItemTarget.AllEnemy)
				{
					builder.Append(string.Format(Texts.Instance.GetText("itemRecoverMonsters"), aux.ToString()));
				}
				builder.Append("<br>");
				aux.Clear();
			}
			else if (theItem.HealQuantity < 0)
			{
				aux.Append("<color=#B00A00>");
				aux.Append(NumFormatItem(theItem.HealQuantity, plus: true));
				aux.Append("</color>");
				builder.Append(string.Format(Texts.Instance.GetText("itemLoseHPSelf"), aux.ToString()));
				builder.Append("<br>");
				aux.Clear();
			}
			if (theItem.EnergyQuantity > 0)
			{
				if (theItem.ItemTarget == Enums.ItemTarget.Self)
				{
					builder.Append(string.Format(Texts.Instance.GetText("cardsGain"), ColorTextArray("system", NumFormat(theItem.EnergyQuantity), SpriteText("energy"))));
				}
				builder.Append("<br>");
			}
			if (theItem.HealPercentQuantity > 0)
			{
				aux.Append("<color=#111111>");
				aux.Append(NumFormatItem(theItem.HealPercentQuantity, plus: true, percent: true));
				aux.Append("</color>");
				if (theItem.ItemTarget == Enums.ItemTarget.AllHero)
				{
					builder.Append(string.Format(Texts.Instance.GetText("itemRecoverHeroes"), aux.ToString()));
				}
				else if (theItem.ItemTarget == Enums.ItemTarget.Self)
				{
					if (theItem.Activation == Enums.EventActivation.Killed)
					{
						builder.Append(string.Format(Texts.Instance.GetText("itemResurrectHP"), aux.ToString()));
					}
					else
					{
						builder.Append(string.Format(Texts.Instance.GetText("itemRecoverSelf"), aux.ToString()));
					}
				}
				else if (theItem.ItemTarget == Enums.ItemTarget.LowestFlatHpEnemy)
				{
					builder.Append(string.Format(Texts.Instance.GetText("itemRecoverLowestHPMonster"), aux.ToString()));
				}
				else if (theItem.ItemTarget == Enums.ItemTarget.LowestFlatHpHero)
				{
					builder.Append(string.Format(Texts.Instance.GetText("itemRecoverLowestHPHero"), aux.ToString()));
				}
				else if (theItem.ItemTarget == Enums.ItemTarget.AllEnemy)
				{
					builder.Append(string.Format(Texts.Instance.GetText("itemRecoverMonsters"), aux.ToString()));
				}
				else if (theItem.ItemTarget == Enums.ItemTarget.RandomHero)
				{
					builder.Append(string.Format(Texts.Instance.GetText("itemRecoverRandomHPHero"), aux.ToString()));
				}
				else if (theItem.ItemTarget == Enums.ItemTarget.RandomEnemy)
				{
					builder.Append(string.Format(Texts.Instance.GetText("itemRecoverRandomHPMonster"), aux.ToString()));
				}
				builder.Append("<br>");
				aux.Clear();
			}
			if (theItem.HealPercentQuantitySelf < 0)
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsYouLose"), ColorTextArray("damage", NumFormat(Mathf.Abs(theItem.HealPercentQuantitySelf)), Functions.SpaceBeforePercentSign() ? "" : "<space=-.1>", "% HP")));
				builder.Append("<br>");
			}
			if (theItem.HealBasedOnAuraCurse > 0)
			{
				StringBuilder stringBuilder7 = new StringBuilder();
				stringBuilder7.Append("Heal ");
				stringBuilder7.Append(ColorTextArray("heal", "X", SpriteText("heal")));
				stringBuilder7.Append("\n");
				stringBuilder7.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYour"), SpriteText(theItem.AuraCurseSetted.ACName), " charges"));
				builder.Append(stringBuilder7);
				builder.Append("<br>");
			}
			if (theItem.healSelfPerDamageDonePercent > 0f)
			{
				string text2 = "cardsHealSelfPerDamage";
				if (theItem.ItemTarget == Enums.ItemTarget.AllHero || theItem.healSelfTeamPerDamageDonePercent)
				{
					text2 = "cardHealAllHeroesPerDamage";
				}
				builder.Append(string.Format(Texts.Instance.GetText(text2), theItem.healSelfPerDamageDonePercent.ToString()));
				builder.Append("\n");
			}
			string text3 = "";
			if (theItem.DamageToTarget1 > 0)
			{
				text3 += ColorTextArray("damage", NumFormat(enchantDamagePreCalculated1), SpriteText(Enum.GetName(typeof(Enums.DamageType), theItem.DamageToTargetType1)));
			}
			else if (theItem.DttSpecialValues1.Use)
			{
				text3 += ColorTextArray("damage", "X", SpriteText(Enum.GetName(typeof(Enums.DamageType), theItem.DamageToTargetType1)));
			}
			if (theItem.DamageToTarget2 > 0)
			{
				text3 += ColorTextArray("damage", NumFormat(enchantDamagePreCalculated2), SpriteText(Enum.GetName(typeof(Enums.DamageType), theItem.DamageToTargetType2)));
			}
			else if (theItem.DttSpecialValues2.Use)
			{
				text3 += ColorTextArray("damage", "X", SpriteText(Enum.GetName(typeof(Enums.DamageType), theItem.DamageToTargetType2)));
			}
			if (!text3.IsNullOrEmpty())
			{
				string text4 = "cardsDealDamage";
				if (theItem.ItemTarget == Enums.ItemTarget.AllHero)
				{
					text4 = "dealDamageToAllHeroes";
				}
				builder.Append(string.Format(Texts.Instance.GetText(text4), text3));
				builder.Append("\n");
			}
			num = 0;
			bool flag = true;
			aux.Clear();
			if (theItem.AuracurseGain1 != null && (theItem.AuracurseGainValue1 > 0 || theItem.AuracurseGain1SpecialValue.Use))
			{
				num++;
				if (!theItem.AuracurseGain1SpecialValue.Use)
				{
					if (theItem.NotShowCharacterBonus)
					{
						aux.Append(ColorTextArray("aura", NumFormat(theItem.AuracurseGainValue1), SpriteText(theItem.AuracurseGain1.Id)));
					}
					else
					{
						aux.Append(ColorTextArray("aura", NumFormat(GetFinalAuraCharges(theItem.AuracurseGain1.Id, theItem.AuracurseGainValue1, character, theItem.AuraCurseNumForOneEvent > 0)), SpriteText(theItem.AuracurseGain1.Id)));
					}
				}
				else
				{
					aux.Append(ColorTextArray("aura", "X", SpriteText(theItem.AuracurseGain1.Id)));
				}
				if (!theItem.AuracurseGain1.IsAura)
				{
					flag = false;
				}
			}
			if (theItem.AuracurseGain2 != null && theItem.AuracurseGainValue2 > 0)
			{
				num++;
				if (!theItem.AuracurseGain2SpecialValue.Use)
				{
					if (theItem.NotShowCharacterBonus)
					{
						aux.Append(ColorTextArray("aura", NumFormat(theItem.AuracurseGainValue2), SpriteText(theItem.AuracurseGain2.Id)));
					}
					else
					{
						aux.Append(ColorTextArray("aura", NumFormat(GetFinalAuraCharges(theItem.AuracurseGain2.Id, theItem.AuracurseGainValue2, character, theItem.AuraCurseNumForOneEvent > 0)), SpriteText(theItem.AuracurseGain2.Id)));
					}
				}
				else
				{
					aux.Append(ColorTextArray("aura", "X", SpriteText(theItem.AuracurseGain2.Id)));
				}
			}
			if (theItem.AuracurseGain3 != null && theItem.AuracurseGainValue3 > 0)
			{
				num++;
				if (!theItem.AuracurseGain3SpecialValue.Use)
				{
					if (theItem.NotShowCharacterBonus)
					{
						aux.Append(ColorTextArray("aura", NumFormat(theItem.AuracurseGainValue3), SpriteText(theItem.AuracurseGain3.Id)));
					}
					else
					{
						aux.Append(ColorTextArray("aura", NumFormat(GetFinalAuraCharges(theItem.AuracurseGain3.Id, theItem.AuracurseGainValue3, character, theItem.AuraCurseNumForOneEvent > 0)), SpriteText(theItem.AuracurseGain3.Id)));
					}
				}
				else
				{
					aux.Append(ColorTextArray("aura", "X", SpriteText(theItem.AuracurseGain3.Id)));
				}
			}
			if (num > 0)
			{
				if (theItem.ItemTarget == Enums.ItemTarget.Self || theItem.ItemTarget == Enums.ItemTarget.SelfEnemy)
				{
					if (theItem.HealQuantity > 0 || theItem.EnergyQuantity > 0 || theItem.HealPercentQuantity > 0)
					{
						StringBuilder stringBuilder8 = new StringBuilder();
						if (flag)
						{
							stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsAnd"), builder.ToString(), string.Format(Functions.LowercaseFirst(Texts.Instance.GetText("cardsGain")), aux.ToString())));
						}
						else
						{
							stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsAnd"), builder.ToString(), string.Format(Functions.LowercaseFirst(Texts.Instance.GetText("cardsSuffer")), aux.ToString())));
						}
						builder.Clear();
						builder.Append(stringBuilder8.ToString());
					}
					else
					{
						if (theItem.AuraCurseSetted != null && theItem.AuraCurseNumForOneEvent > 0)
						{
							string text5 = SpriteText(theItem.AuraCurseSetted.Id);
							if ((bool)theItem.AuraCurseSetted2)
							{
								text5 += SpriteText(theItem.AuraCurseSetted2.Id);
								if ((bool)theItem.AuraCurseSetted3)
								{
									text5 += SpriteText(theItem.AuraCurseSetted3.Id);
								}
							}
							stringBuilder5 = stringBuilder5 ?? new StringBuilder();
							stringBuilder5.Append(string.Format(Texts.Instance.GetText("itemForEveryCharge"), ColorTextArray("curse", (theItem.AuraCurseNumForOneEvent > 1) ? theItem.AuraCurseNumForOneEvent.ToString() : "", text5)));
						}
						if (stringBuilder5 != null && stringBuilder5.Length > 0)
						{
							builder.Append(stringBuilder5.ToString() + " ");
						}
						stringBuilder5 = null;
						if (flag)
						{
							string text6 = builder.ToString();
							if (text6.Length > 8 && text6.Substring(text6.Length - 9) == "<c>, </c>")
							{
								builder.Append(string.Format(Functions.LowercaseFirst(Texts.Instance.GetText("cardsGain")), aux.ToString()));
							}
							else
							{
								builder.Append(string.Format(Texts.Instance.GetText("cardsGain"), aux.ToString()));
							}
						}
						else if (builder.Length > 8 && builder.ToString().Substring(builder.ToString().Length - 9) == "<c>, </c>")
						{
							builder.Append(string.Format(Functions.LowercaseFirst(Texts.Instance.GetText("cardsSuffer")), aux.ToString()));
						}
						else
						{
							builder.Append(string.Format(Texts.Instance.GetText("cardsSuffer"), aux.ToString()));
						}
					}
				}
				else if (theItem.ItemTarget == Enums.ItemTarget.AllEnemy)
				{
					builder.Append(string.Format(Texts.Instance.GetText("itemApplyEnemies"), aux.ToString()));
				}
				else if (theItem.ItemTarget == Enums.ItemTarget.AllHero)
				{
					if (cardClass == Enums.CardClass.Monster)
					{
						builder.Append(string.Format(Texts.Instance.GetText("itemApplyHeroesFromMonster"), aux.ToString()));
					}
					else
					{
						builder.Append(string.Format(Texts.Instance.GetText("itemApplyHeroes"), aux.ToString()));
					}
				}
				else if (theItem.ItemTarget == Enums.ItemTarget.RandomHero)
				{
					if (theItem.AuraCurseSetted != null && theItem.AuraCurseNumForOneEvent > 0)
					{
						string text7 = SpriteText(theItem.AuraCurseSetted.Id);
						if ((bool)theItem.AuraCurseSetted2)
						{
							text7 += SpriteText(theItem.AuraCurseSetted2.Id);
							if ((bool)theItem.AuraCurseSetted3)
							{
								text7 += SpriteText(theItem.AuraCurseSetted3.Id);
							}
						}
						builder.Append(string.Format(Texts.Instance.GetText("itemForEveryCharge"), ColorTextArray("curse", text7)));
					}
					builder.Append(string.Format(Texts.Instance.GetText("itemApplyRandomHero"), aux.ToString()));
				}
				else if (theItem.ItemTarget == Enums.ItemTarget.RandomEnemy)
				{
					if (theItem.AuraCurseSetted != null && theItem.AuraCurseNumForOneEvent > 0)
					{
						string text8 = SpriteText(theItem.AuraCurseSetted.Id);
						if ((bool)theItem.AuraCurseSetted2)
						{
							text8 += SpriteText(theItem.AuraCurseSetted2.Id);
							if ((bool)theItem.AuraCurseSetted3)
							{
								text8 += SpriteText(theItem.AuraCurseSetted3.Id);
							}
						}
						builder.Append(string.Format(Texts.Instance.GetText("itemForEveryCharge"), ColorTextArray("curse", text8)));
					}
					builder.Append(string.Format(Texts.Instance.GetText("itemApplyRandomEnemy"), aux.ToString()));
				}
				else if (theItem.ItemTarget == Enums.ItemTarget.HighestFlatHpEnemy)
				{
					if (theItem.AuraCurseSetted != null && theItem.AuraCurseNumForOneEvent > 0)
					{
						string text9 = SpriteText(theItem.AuraCurseSetted.Id);
						if ((bool)theItem.AuraCurseSetted2)
						{
							text9 += SpriteText(theItem.AuraCurseSetted2.Id);
							if ((bool)theItem.AuraCurseSetted3)
							{
								text9 += SpriteText(theItem.AuraCurseSetted3.Id);
							}
						}
						builder.Append(string.Format(Texts.Instance.GetText("itemForEveryCharge"), ColorTextArray("curse", text9)));
					}
					builder.Append(string.Format(Texts.Instance.GetText("itemApplyHighestFlatHpEnemy"), aux.ToString()));
				}
				else if (theItem.ItemTarget == Enums.ItemTarget.LowestFlatHpEnemy)
				{
					if (theItem.AuraCurseSetted != null && theItem.AuraCurseNumForOneEvent > 0)
					{
						string text10 = SpriteText(theItem.AuraCurseSetted.Id);
						if ((bool)theItem.AuraCurseSetted2)
						{
							text10 += SpriteText(theItem.AuraCurseSetted2.Id);
							if ((bool)theItem.AuraCurseSetted3)
							{
								text10 += SpriteText(theItem.AuraCurseSetted3.Id);
							}
						}
						builder.Append(string.Format(Texts.Instance.GetText("itemForEveryCharge"), ColorTextArray("curse", (theItem.AuraCurseNumForOneEvent > 1) ? theItem.AuraCurseNumForOneEvent.ToString() : "", text10)));
					}
					builder.Append(string.Format(Texts.Instance.GetText("itemApplyLowestFlatHpEnemy"), aux.ToString()));
				}
				else if (theItem.ItemTarget == Enums.ItemTarget.HighestFlatHpHero)
				{
					if (theItem.AuraCurseSetted != null && theItem.AuraCurseNumForOneEvent > 0)
					{
						string text11 = SpriteText(theItem.AuraCurseSetted.Id);
						if ((bool)theItem.AuraCurseSetted2)
						{
							text11 += SpriteText(theItem.AuraCurseSetted2.Id);
							if ((bool)theItem.AuraCurseSetted3)
							{
								text11 += SpriteText(theItem.AuraCurseSetted3.Id);
							}
						}
						builder.Append(string.Format(Texts.Instance.GetText("itemForEveryCharge"), ColorTextArray("curse", (theItem.AuraCurseNumForOneEvent > 1) ? theItem.AuraCurseNumForOneEvent.ToString() : "", text11)));
					}
					builder.Append(string.Format(Texts.Instance.GetText("itemApplyHighestFlatHpHero"), aux.ToString()));
				}
				else if (theItem.ItemTarget == Enums.ItemTarget.LowestFlatHpHero)
				{
					if (theItem.AuraCurseSetted != null && theItem.AuraCurseNumForOneEvent > 0)
					{
						string text12 = SpriteText(theItem.AuraCurseSetted.Id);
						if ((bool)theItem.AuraCurseSetted2)
						{
							text12 += SpriteText(theItem.AuraCurseSetted2.Id);
							if ((bool)theItem.AuraCurseSetted3)
							{
								text12 += SpriteText(theItem.AuraCurseSetted3.Id);
							}
						}
						builder.Append(string.Format(Texts.Instance.GetText("itemForEveryCharge"), ColorTextArray("curse", (theItem.AuraCurseNumForOneEvent > 1) ? theItem.AuraCurseNumForOneEvent.ToString() : "", text12)));
					}
					builder.Append(string.Format(Texts.Instance.GetText("itemApplyLowestFlatHpHero"), aux.ToString()));
				}
				else if (targetSide == Enums.CardTargetSide.Enemy || theItem.ItemTarget == Enums.ItemTarget.CurrentTarget)
				{
					if (theItem.AuraCurseSetted != null && theItem.AuraCurseNumForOneEvent > 0)
					{
						string text13 = SpriteText(theItem.AuraCurseSetted.Id);
						if ((bool)theItem.AuraCurseSetted2)
						{
							text13 += SpriteText(theItem.AuraCurseSetted2.Id);
							if ((bool)theItem.AuraCurseSetted3)
							{
								text13 += SpriteText(theItem.AuraCurseSetted3.Id);
							}
						}
						builder.Append(string.Format(Texts.Instance.GetText("itemApplyForEvery"), ColorTextArray("curse", (theItem.AuraCurseNumForOneEvent > 1) ? theItem.AuraCurseNumForOneEvent.ToString() : "", text13), aux.ToString()));
					}
					else if (theItem.ItemTarget == Enums.ItemTarget.Random)
					{
						builder.Append(string.Format(Texts.Instance.GetText("itemApplyRandom"), aux.ToString()));
					}
					else
					{
						builder.Append(string.Format(Texts.Instance.GetText("cardsApply"), aux.ToString()));
					}
				}
				else
				{
					builder.Append(string.Format(Texts.Instance.GetText("cardsGrant"), aux.ToString()));
				}
				builder.Append("\n");
				aux.Clear();
			}
			GetXEqualsDecriptionValue();
			num = 0;
			flag = true;
			if (theItem.AuracurseGainSelf1 != null && theItem.AuracurseGainSelfValue1 > 0)
			{
				num++;
				aux.Append(ColorTextArray("aura", NumFormat(GetFinalAuraCharges(theItem.AuracurseGainSelf1.Id, theItem.AuracurseGainSelfValue1, character)), SpriteText(theItem.AuracurseGainSelf1.Id)));
				if (!theItem.AuracurseGainSelf1.IsAura)
				{
					flag = false;
				}
			}
			if (theItem.AuracurseGainSelf2 != null && theItem.AuracurseGainSelfValue2 > 0)
			{
				num++;
				aux.Append(ColorTextArray("aura", NumFormat(GetFinalAuraCharges(theItem.AuracurseGainSelf2.Id, theItem.AuracurseGainSelfValue2, character)), SpriteText(theItem.AuracurseGainSelf2.Id)));
			}
			if (theItem.AuracurseGainSelf3 != null && theItem.AuracurseGainSelfValue3 > 0)
			{
				num++;
				aux.Append(ColorTextArray("aura", NumFormat(GetFinalAuraCharges(theItem.AuracurseGainSelf3.Id, theItem.AuracurseGainSelfValue3, character)), SpriteText(theItem.AuracurseGainSelf3.Id)));
			}
			string auraSprites = "";
			string curseSprites = "";
			SetAuraCurseSpritesText(ref auraSprites, ref curseSprites, theItem.auracurseHeal1);
			SetAuraCurseSpritesText(ref auraSprites, ref curseSprites, theItem.auracurseHeal2);
			SetAuraCurseSpritesText(ref auraSprites, ref curseSprites, theItem.auracurseHeal3);
			if (!string.IsNullOrEmpty(auraSprites))
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsPurge"), auraSprites));
				builder.Append("\n");
			}
			if (!string.IsNullOrEmpty(curseSprites))
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsDispel"), curseSprites));
				builder.Append("\n");
			}
			if (num > 0)
			{
				if (theItem.HealQuantity > 0 || theItem.EnergyQuantity > 0 || theItem.HealPercentQuantity > 0)
				{
					StringBuilder stringBuilder9 = new StringBuilder();
					if (flag)
					{
						stringBuilder9.Append(string.Format(Texts.Instance.GetText("cardsAnd"), builder.ToString(), string.Format(Functions.LowercaseFirst(Texts.Instance.GetText("cardsGain")), aux.ToString())));
					}
					else
					{
						stringBuilder9.Append(string.Format(Texts.Instance.GetText("cardsAnd"), builder.ToString(), string.Format(Functions.LowercaseFirst(Texts.Instance.GetText("cardsSuffer")), aux.ToString())));
					}
					builder.Clear();
					builder.Append(stringBuilder9.ToString());
					stringBuilder9 = null;
				}
				else if (flag)
				{
					if (theItem.ChooseOneACToGain)
					{
						builder.Append(string.Format(Texts.Instance.GetText("cardsGainOneOf"), aux.ToString()));
					}
					else
					{
						builder.Append(string.Format(Texts.Instance.GetText("cardsGain"), aux.ToString()));
					}
				}
				else if (theItem.ChooseOneACToGain)
				{
					builder.Append(string.Format(Texts.Instance.GetText("cardsSufferOneOf"), aux.ToString()));
				}
				else
				{
					builder.Append(string.Format(Texts.Instance.GetText("cardsSuffer"), aux.ToString()));
				}
				builder.Append("\n");
				aux.Clear();
			}
			if (theItem.CardNum > 0)
			{
				string text14 = "";
				if (theItem.CardToGain != null)
				{
					if (theItem.CardNum > 1)
					{
						aux.Append(ColorTextArray("", NumFormat(theItem.CardNum), SpriteText("card")));
					}
					else
					{
						aux.Append(SpriteText("card"));
					}
					CardData cardData = Globals.Instance.GetCardData(theItem.CardToGain.Id, instantiate: false);
					if (cardData != null)
					{
						aux.Append(ColorFromCardDataRarity(cardData));
						aux.Append(cardData.CardName);
						aux.Append("</color>");
					}
					text14 = aux.ToString();
					aux.Clear();
				}
				else
				{
					if (theItem.CardNum > 1)
					{
						aux.Append(ColorTextArray("", NumFormat(theItem.CardNum), SpriteText("card")));
					}
					else
					{
						aux.Append(SpriteText("card"));
					}
					if (theItem.CardToGainType != Enums.CardType.None)
					{
						aux.Append("<color=#5E3016>");
						aux.Append(Texts.Instance.GetText(Enum.GetName(typeof(Enums.CardType), theItem.CardToGainType)));
						aux.Append("</color>");
					}
					text14 = aux.ToString();
					aux.Clear();
				}
				string text15 = "";
				if (theItem.Permanent)
				{
					if (theItem.Vanish)
					{
						if (theItem.CostZero)
						{
							text15 = string.Format(Texts.Instance.GetText("cardsAddCostVanish"), 0);
						}
						else if (theItem.CostReduction > 0)
						{
							text15 = string.Format(Texts.Instance.GetText("cardsAddCostReducedVanish"), NumFormatItem(theItem.CostReduction, plus: true));
						}
					}
					else if (theItem.CostZero)
					{
						text15 = string.Format(Texts.Instance.GetText("cardsAddCost"), 0);
					}
					else if (theItem.CostReduction > 0)
					{
						text15 = string.Format(Texts.Instance.GetText("cardsAddCostReduced"), NumFormatItem(theItem.CostReduction, plus: true));
					}
				}
				else if (theItem.Vanish)
				{
					if (theItem.CostZero)
					{
						text15 = string.Format(Texts.Instance.GetText("cardsAddCostVanishTurn"), 0);
					}
					else if (theItem.CostReduction > 0)
					{
						text15 = string.Format(Texts.Instance.GetText("cardsAddCostReducedVanishTurn"), NumFormatItem(theItem.CostReduction, plus: true));
					}
				}
				else if (theItem.CostZero)
				{
					text15 = string.Format(Texts.Instance.GetText("cardsAddCostTurn"), 0);
				}
				else if (theItem.CostReduction > 0)
				{
					text15 = string.Format(Texts.Instance.GetText("cardsAddCostReducedTurn"), NumFormatItem(theItem.CostReduction, plus: true));
				}
				if (theItem.DuplicateActive)
				{
					if (theItem.CardPlace == Enums.CardPlace.Hand)
					{
						builder.Append(Texts.Instance.GetText("cardsDuplicateHand"));
					}
				}
				else if (theItem.CardPlace == Enums.CardPlace.RandomDeck)
				{
					builder.Append(string.Format(Texts.Instance.GetText("cardsIDShuffleDeck"), text14));
				}
				else if (theItem.CardPlace == Enums.CardPlace.Cast)
				{
					if (theItem.CardToGain != null)
					{
						CardData cardData2 = Globals.Instance.GetCardData(theItem.CardToGain.Id, instantiate: false);
						if (cardData2 != null)
						{
							aux.Append(ColorFromCardDataRarity(cardData2));
							aux.Append(cardData2.CardName);
							aux.Append("</color>");
						}
					}
					builder.Append(string.Format(Texts.Instance.GetText("cardsCast"), aux.ToString()));
					aux.Clear();
				}
				else if (theItem.CardPlace == Enums.CardPlace.Hand || theItem.CardPlace == Enums.CardPlace.TopDeck)
				{
					if (theItem.CardNum > 1)
					{
						aux.Append(ColorTextArray("", NumFormat(theItem.CardNum), SpriteText("card")));
					}
					else
					{
						aux.Append(SpriteText("card"));
					}
					if (theItem.CardToGain != null)
					{
						CardData cardData3 = Globals.Instance.GetCardData(theItem.CardToGain.Id, instantiate: false);
						if (cardData3 != null)
						{
							aux.Append(ColorFromCardDataRarity(cardData3));
							aux.Append(cardData3.CardName);
							aux.Append("</color>");
						}
					}
					aux.Clear();
					string text16 = "cardsIDPlaceHand";
					if (theItem.CardPlace == Enums.CardPlace.TopDeck)
					{
						text16 = "cardsIDPlaceTopDeck";
					}
					builder.Append(string.Format(Texts.Instance.GetText(text16), text14));
				}
				if (text15 != "")
				{
					builder.Append(" ");
					builder.Append(grColor);
					builder.Append(text15);
					builder.Append(endColor);
				}
				if (theItem.CardsReduced == 0)
				{
					builder.Append("\n");
				}
				else
				{
					builder.Append(" ");
				}
			}
			if (theItem.CardsReduced > 0)
			{
				StringBuilder stringBuilder10 = new StringBuilder();
				stringBuilder10.Append("<color=#5E3016>");
				stringBuilder10.Append(theItem.CardsReduced);
				stringBuilder10.Append("</color>");
				string text17 = stringBuilder10.ToString();
				string text18;
				if (theItem.CardToReduceType == Enums.CardType.None)
				{
					text18 = "  <sprite name=cards>";
				}
				else
				{
					stringBuilder10.Clear();
					stringBuilder10.Append("<color=#5E3016>");
					stringBuilder10.Append(Texts.Instance.GetText(Enum.GetName(typeof(Enums.CardType), theItem.CardToReduceType)));
					stringBuilder10.Append("</color>");
					text18 = stringBuilder10.ToString();
				}
				stringBuilder10.Clear();
				stringBuilder10.Append("<color=#111111>");
				stringBuilder10.Append(Mathf.Abs(theItem.CostReduceReduction));
				stringBuilder10.Append("</color>");
				string text19 = stringBuilder10.ToString();
				string text20 = ((theItem.CostReduceEnergyRequirement <= 0) ? "<space=-.2>" : ("<color=#444><size=-.2>" + string.Format(Texts.Instance.GetText("itemReduceCost"), theItem.CostReduceEnergyRequirement) + "</size></color>"));
				if (theItem.CostReducePermanent && theItem.ReduceHighestCost)
				{
					text17 = ((theItem.CardsReduced != 1) ? ("<color=#111111>(" + theItem.CardsReduced + ")</color> ") : "");
					builder.Append(string.Format(Texts.Instance.GetText("itemReduceHighestPermanent"), text17, text18, text19, text20));
				}
				else if (theItem.CostReducePermanent)
				{
					if (theItem.CardsReduced != 10)
					{
						if (theItem.CostReduceReduction > 0)
						{
							builder.Append(string.Format(Texts.Instance.GetText("itemReduce"), text17, text18, text19, text20));
						}
						else
						{
							builder.Append(string.Format(Texts.Instance.GetText("itemIncrease"), text17, text18, text19, text20));
						}
					}
					else if (theItem.CostReduceReduction > 0)
					{
						builder.Append(string.Format(Texts.Instance.GetText("itemReduceAll"), text18, text19, text20));
					}
					else
					{
						builder.Append(string.Format(Texts.Instance.GetText("itemIncreaseAll"), text18, text19, text20));
					}
				}
				else if (theItem.ReduceHighestCost)
				{
					text17 = ((theItem.CardsReduced != 1) ? ("<color=#111111>(" + theItem.CardsReduced + ")</color> ") : "");
					if (theItem.CostReduceReduction > 0)
					{
						builder.Append(string.Format(Texts.Instance.GetText("itemReduceHighestTurn"), text17, text18, text19, text20));
					}
					else
					{
						builder.Append(string.Format(Texts.Instance.GetText("itemIncreaseHighestDiscarded"), text17, text18, text19, text20));
					}
				}
				else if (theItem.CardsReduced != 10)
				{
					if (theItem.CostReduceReduction > 0)
					{
						builder.Append(string.Format(Texts.Instance.GetText("itemReduceTurn"), text17, text18, text19, text20));
					}
					else
					{
						builder.Append(string.Format(Texts.Instance.GetText("itemIncreaseTurn"), text17, text18, text19, text20));
					}
				}
				else if (theItem.CostReduceReduction > 0)
				{
					builder.Append(string.Format(Texts.Instance.GetText("itemReduceTurnAll"), text18, text19, text20));
				}
				else
				{
					builder.Append(string.Format(Texts.Instance.GetText("itemIncreaseTurnAll"), text18, text19, text20));
				}
				builder.Append("\n");
			}
		}
		if (theItem.PetActivation == Enums.ActivePets.Self)
		{
			builder.Append(Texts.Instance.GetText("itemPetActivateSelf"));
			builder.Append("\n");
		}
		else if (theItem.PetActivation == Enums.ActivePets.AllTeam)
		{
			builder.Append(Texts.Instance.GetText("itemPetActivateAllTeam"));
			builder.Append("\n");
		}
		if (theItem.IncreaseAurasSelf > 0)
		{
			builder.Append(string.Format(Texts.Instance.GetText("cardsIncreaseAura"), increaseAuras.ToString()));
			builder.Append("\n");
		}
		if (!string.IsNullOrEmpty(descriptionId))
		{
			List<object> list = new List<object>();
			string[] array = descriptionArgs;
			for (int i = 0; i < array.Length; i++)
			{
				if (string.Equals(array[i].Trim(), "DestroyAfterUses", StringComparison.OrdinalIgnoreCase))
				{
					list.Add(theItem.DestroyAfterUses);
				}
			}
			if (list.Count > 0)
			{
				builder.Append(string.Format(Texts.Instance.GetText(descriptionId), list.ToArray()));
			}
		}
		if (theItem.DestroyStartOfTurn || theItem.DestroyEndOfTurn)
		{
			builder.Append("<voffset=-.1><size=-.05><color=#1A505A>- ");
			builder.Append(Texts.Instance.GetText("itemDestroyStartTurn"));
			builder.Append(" -</color></size>");
		}
		if (theItem.DestroyAfterUses > 0 && !theItem.UseTheNextInsteadWhenYouPlay)
		{
			builder.Append("<nobr><size=-.05><color=#1A505A>- ");
			if (theItem.DestroyAfterUses > 1)
			{
				builder.Append(string.Format(Texts.Instance.GetText("itemLastUses"), theItem.DestroyAfterUses));
			}
			else
			{
				builder.Append(Texts.Instance.GetText("itemLastUse"));
			}
			builder.Append(" -</color></size></nobr>");
		}
		if (theItem.TimesPerCombat > 0)
		{
			builder.Append("<nobr><size=-.05><color=#1A505A>- ");
			if (theItem.TimesPerCombat == 1)
			{
				builder.Append(Texts.Instance.GetText("itemOncePerCombat"));
			}
			else if (theItem.TimesPerCombat == 2)
			{
				builder.Append(Texts.Instance.GetText("itemTwicePerCombat"));
			}
			else if (theItem.TimesPerCombat == 3)
			{
				builder.Append(Texts.Instance.GetText("itemThricePerCombat"));
			}
			builder.Append(" -</color></size></nobr>");
		}
		if (theItem.StealAuras > 0)
		{
			string text21 = "cardsStealAuras";
			if (theItem.ItemTarget == Enums.ItemTarget.RandomEnemy)
			{
				text21 = "cardsStealAurasFromRandomEnemy";
			}
			builder.Append(string.Format(Texts.Instance.GetText(text21), theItem.StealAuras.ToString()));
			builder.Append("\n");
		}
		if (theItem.PassSingleAndCharacterRolls)
		{
			builder.Append(Texts.Instance.GetText("cardsPassEventRoll"));
			builder.Append("\n");
		}
		if (theItem.DontTargetBoss)
		{
			builder.Append("<size=-.15><color=#444>");
			builder.Append(Texts.Instance.GetText("dontApplyOnBosses"));
			builder.Append("</color></size><br>");
		}
		void GetXEqualsDecriptionValue()
		{
			SpecialValues specialValues = default(SpecialValues);
			if (theItem.AuracurseGain1SpecialValue.Use)
			{
				specialValues = theItem.AuracurseGain1SpecialValue;
			}
			if (theItem.AuracurseGain2SpecialValue.Use)
			{
				specialValues = theItem.AuracurseGain2SpecialValue;
			}
			if (theItem.AuracurseGain3SpecialValue.Use)
			{
				specialValues = theItem.AuracurseGain3SpecialValue;
			}
			if (theItem.DttSpecialValues1.Use)
			{
				specialValues = theItem.DttSpecialValues1;
			}
			if (theItem.DttSpecialValues2.Use)
			{
				specialValues = theItem.DttSpecialValues2;
			}
			if (theItem.HealQuantitySpecialValue.Use)
			{
				specialValues = theItem.HealQuantitySpecialValue;
			}
			if (specialValues.Use)
			{
				builder.Append(goldColor);
				builder.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYour"), SpecialModifierDescription(specialValues, item ?? ItemEnchantment), $" x{specialValues.Multiplier}"));
				builder.Append(endColor);
				builder.Append("\n");
			}
		}
		void SetAuraCurseSpritesText(ref string reference, ref string reference2, AuraCurseData acData)
		{
			if ((bool)acData)
			{
				if (acData.IsAura)
				{
					reference += SpriteText(acData.Id);
				}
				else
				{
					reference2 += SpriteText(acData.Id);
				}
			}
		}
	}

	private void AppendCardDescription(Character character, StringBuilder builder, StringBuilder aux, string grColor, string endColor, string br1, string goldColor)
	{
		int num = 0;
		string text = "";
		string text2 = "";
		string text3 = "";
		float equalsMultiplier = 0f;
		string equalsMultiplierString = "";
		bool isShare = false;
		bool isTransform = false;
		bool flag = true;
		bool allowPurgeDispelText = false;
		StringBuilder stringBuilder = new StringBuilder();
		if (damage > 0 || damage2 > 0 || damageSelf > 0 || DamageSelf2 > 0)
		{
			flag = false;
		}
		if (drawCard != 0 && !drawCardSpecialValueGlobal)
		{
			builder.Append(string.Format(Texts.Instance.GetText("cardsDraw"), ColorTextArray("", NumFormat(drawCard), SpriteText("card"))));
			builder.Append("<br>");
		}
		if (specialValueGlobal == Enums.CardSpecialValue.DiscardedCards)
		{
			if (discardCardPlace == Enums.CardPlace.Vanish)
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsVanish"), ColorTextArray("", "X", SpriteText("card"))));
			}
			else
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsDiscard"), ColorTextArray("", "X", SpriteText("card"))));
			}
			builder.Append("\n");
			AppendXEqualsString();
		}
		if (drawCardSpecialValueGlobal)
		{
			builder.Append(string.Format(Texts.Instance.GetText("cardsDraw"), ColorTextArray("aura", "X", SpriteText("card"))));
			builder.Append("<br>");
		}
		if (addCard != 0)
		{
			string text4 = "";
			if (addCardId != "")
			{
				aux.Append(ColorTextArray("", NumFormat(addCard), SpriteText("card")));
				CardData cardData = Globals.Instance.GetCardData(addCardId, instantiate: false);
				if (cardData != null)
				{
					aux.Append(ColorFromCardDataRarity(cardData));
					aux.Append(cardData.CardName);
					aux.Append("</color>");
				}
				text4 = aux.ToString();
				aux.Clear();
			}
			else
			{
				if (addCardChoose > 0)
				{
					aux.Append(ColorTextArray("", NumFormat(addCardChoose), SpriteText("card")));
				}
				else
				{
					aux.Append(ColorTextArray("", NumFormat(addCard), SpriteText("card")));
				}
				if (addCardType != Enums.CardType.None)
				{
					aux.Append("<color=#5E3016>");
					aux.Append(Texts.Instance.GetText(Enum.GetName(typeof(Enums.CardType), addCardType)));
					aux.Append("</color>");
				}
				text4 = aux.ToString();
				aux.Clear();
			}
			string text5 = "";
			if (addCardReducedCost == -1)
			{
				text5 = (addCardVanish ? ((!addCardCostTurn) ? string.Format(Texts.Instance.GetText("cardsAddCostVanish"), 0) : string.Format(Texts.Instance.GetText("cardsAddCostVanish"), 0)) : ((!addCardCostTurn) ? string.Format(Texts.Instance.GetText("cardsAddCost"), 0) : string.Format(Texts.Instance.GetText("cardsAddCostTurn"), 0)));
			}
			else if (addCardReducedCost > 0)
			{
				text5 = (addCardVanish ? ((!addCardCostTurn) ? string.Format(Texts.Instance.GetText("cardsAddCostReducedVanish"), addCardReducedCost) : string.Format(Texts.Instance.GetText("cardsAddCostReducedVanishTurn"), addCardReducedCost)) : ((!addCardCostTurn) ? string.Format(Texts.Instance.GetText("cardsAddCostReduced"), addCardReducedCost) : string.Format(Texts.Instance.GetText("cardsAddCostReducedTurn"), addCardReducedCost)));
			}
			string text6 = "";
			if (addCardId != "")
			{
				if (addCardPlace == Enums.CardPlace.RandomDeck)
				{
					text6 = ((targetSide != Enums.CardTargetSide.Self && (targetSide != Enums.CardTargetSide.Enemy || cardClass == Enums.CardClass.Monster)) ? "cardsIDShuffleTargetDeck" : "cardsIDShuffleDeck");
				}
				else if (addCardPlace == Enums.CardPlace.Hand)
				{
					text6 = "cardsIDPlaceHand";
				}
				else if (addCardPlace == Enums.CardPlace.TopDeck)
				{
					text6 = ((targetSide != Enums.CardTargetSide.Self) ? "cardsIDPlaceTargetTopDeck" : "cardsIDPlaceTopDeck");
				}
				else if (addCardPlace == Enums.CardPlace.Discard)
				{
					text6 = ((targetSide != Enums.CardTargetSide.Self && (targetSide != Enums.CardTargetSide.Enemy || cardClass == Enums.CardClass.Monster)) ? "cardsIDPlaceTargetDiscard" : "cardsIDPlaceDiscard");
				}
			}
			else if (addCardFrom == Enums.CardFrom.Game)
			{
				if (addCardPlace == Enums.CardPlace.RandomDeck)
				{
					text6 = ((addCardChoose != 0) ? "cardsDiscoverNumberToDeck" : "cardsDiscoverToDeck");
				}
				else if (addCardPlace == Enums.CardPlace.Hand)
				{
					text6 = ((addCardChoose != 0) ? "cardsDiscoverNumberToHand" : "cardsDiscoverToHand");
				}
				else if (addCardPlace == Enums.CardPlace.TopDeck && addCardChoose != 0)
				{
					text6 = "cardsDiscoverNumberToTopDeck";
				}
			}
			else if (addCardFrom == Enums.CardFrom.Deck)
			{
				if (addCardPlace == Enums.CardPlace.Hand)
				{
					text6 = ((addCardChoose > 0) ? "cardsRevealNumberToHand" : ((addCard <= 1) ? "cardsRevealItToHand" : "cardsRevealThemToHand"));
				}
				else if (addCardPlace == Enums.CardPlace.TopDeck)
				{
					text6 = ((addCardChoose > 0) ? "cardsRevealNumberToTopDeck" : ((addCard <= 1) ? "cardsRevealItToTopDeck" : "cardsRevealThemToTopDeck"));
				}
			}
			else if (addCardFrom == Enums.CardFrom.Discard)
			{
				if (addCardPlace == Enums.CardPlace.TopDeck)
				{
					text6 = "cardsPickToTop";
				}
				else if (addCardPlace == Enums.CardPlace.Hand)
				{
					text6 = "cardsPickToHand";
				}
			}
			else if (addCardFrom == Enums.CardFrom.Hand)
			{
				if (targetSide == Enums.CardTargetSide.Friend)
				{
					if (addCardPlace == Enums.CardPlace.TopDeck)
					{
						text6 = "cardsDuplicateToTargetTopDeck";
					}
					else if (addCardPlace == Enums.CardPlace.RandomDeck)
					{
						text6 = "cardsDuplicateToTargetRandomDeck";
					}
				}
				else if (addCardPlace == Enums.CardPlace.Hand)
				{
					text6 = "cardsDuplicateToHand";
				}
			}
			else if (addCardFrom == Enums.CardFrom.Vanish)
			{
				if (addCardPlace == Enums.CardPlace.TopDeck)
				{
					text6 = "cardsFromVanishToTop";
				}
				else if (addCardPlace == Enums.CardPlace.Hand)
				{
					text6 = "cardsFromVanishToHand";
				}
			}
			builder.Append(string.Format(Texts.Instance.GetText(text6), text4, ColorTextArray("", NumFormat(addCard))));
			if (text5 != "")
			{
				builder.Append(" ");
				builder.Append(grColor);
				builder.Append(text5);
				builder.Append(endColor);
			}
			builder.Append("\n");
		}
		if (discardCard != 0)
		{
			if (discardCardType != Enums.CardType.None)
			{
				aux.Append("<color=#5E3016>");
				aux.Append(Texts.Instance.GetText(Enum.GetName(typeof(Enums.CardType), discardCardType)));
				aux.Append("</color>");
			}
			if (discardCardPlace == Enums.CardPlace.Discard)
			{
				if (!discardCardAutomatic)
				{
					builder.Append(string.Format(Texts.Instance.GetText("cardsDiscard"), ColorTextArray("", NumFormat(discardCard), SpriteText("card"))));
					builder.Append(aux);
					builder.Append("\n");
				}
				else
				{
					builder.Append(string.Format(Texts.Instance.GetText("cardsDiscard"), ColorTextArray("", NumFormat(discardCard), SpriteText("cardrandom"))));
					builder.Append(aux);
					builder.Append("\n");
				}
			}
			else if (discardCardPlace == Enums.CardPlace.TopDeck)
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsPlaceToTop"), ColorTextArray("", NumFormat(discardCard), SpriteText("card"), aux.ToString().Trim())));
				builder.Append("\n");
			}
			else if (discardCardPlace == Enums.CardPlace.Vanish)
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsVanish"), ColorTextArray("", NumFormat(discardCard), SpriteText("card"), aux.ToString().Trim())));
				builder.Append("\n");
			}
			aux.Clear();
		}
		if (lookCards != 0)
		{
			builder.Append(string.Format(Texts.Instance.GetText("cardsLook"), ColorTextArray("", NumFormat(lookCards), SpriteText("card"))));
			builder.Append("\n");
			if (lookCardsDiscardUpTo == -1)
			{
				builder.Append(Texts.Instance.GetText("cardsDiscardAny"));
				builder.Append("\n");
			}
			else if (lookCardsDiscardUpTo > 0)
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsDiscardUpTo"), ColorTextArray("", NumFormat(lookCardsDiscardUpTo))));
				builder.Append("\n");
			}
			else if (lookCardsVanishUpTo > 0)
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsVanishUpTo"), ColorTextArray("", NumFormat(lookCardsVanishUpTo))));
				builder.Append("\n");
			}
		}
		num = 0;
		if (summonUnit != null && summonUnitNum > 0)
		{
			aux.Append("\n<color=#5E3016>");
			if (summonUnitNum > 1)
			{
				aux.Append(summonUnitNum);
				aux.Append(" ");
			}
			NPCData nPC = Globals.Instance.GetNPC(summonUnit?.Id);
			if (nPC != null)
			{
				aux.Append(nPC.NPCName);
			}
			if (metamorph)
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsMetamorph"), aux.ToString()));
			}
			else if (evolve)
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsEvolve"), aux.ToString()));
			}
			else
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsSummon"), aux.ToString()));
			}
			builder.Append("</color>\n");
			aux.Clear();
		}
		num = 0;
		StringBuilder stringBuilder2 = new StringBuilder();
		StringBuilder stringBuilder3 = new StringBuilder();
		if (id == "compassion")
		{
			Debug.Log("X");
		}
		if (healAuraCurseName != null && !AuraCurseIsPartOfTransformAura(HealAuraCurseName.Id))
		{
			if (healAuraCurseName.IsAura)
			{
				stringBuilder2.Append(SpriteText(healAuraCurseName.ACName));
			}
			else
			{
				stringBuilder3.Append(SpriteText(healAuraCurseName.ACName));
			}
		}
		if (healAuraCurseName2 != null && !AuraCurseIsPartOfTransformAura(HealAuraCurseName2.Id))
		{
			if (healAuraCurseName2.IsAura)
			{
				stringBuilder2.Append(SpriteText(healAuraCurseName2.ACName));
			}
			else
			{
				stringBuilder3.Append(SpriteText(healAuraCurseName2.ACName));
			}
		}
		if (healAuraCurseName3 != null && !AuraCurseIsPartOfTransformAura(HealAuraCurseName3.Id))
		{
			if (healAuraCurseName3.IsAura)
			{
				stringBuilder2.Append(SpriteText(healAuraCurseName3.ACName));
			}
			else
			{
				stringBuilder3.Append(SpriteText(healAuraCurseName3.ACName));
			}
		}
		if (healAuraCurseName4 != null && !AuraCurseIsPartOfTransformAura(HealAuraCurseName4.Id))
		{
			if (healAuraCurseName4.IsAura)
			{
				stringBuilder2.Append(SpriteText(healAuraCurseName4.ACName));
			}
			else
			{
				stringBuilder3.Append(SpriteText(healAuraCurseName4.ACName));
			}
		}
		if (stringBuilder2.Length > 0)
		{
			string text7 = "cardsPurge";
			if (targetSide == Enums.CardTargetSide.Self)
			{
				text7 = "cardsPurgeYour";
			}
			builder.Append(string.Format(Texts.Instance.GetText(text7), stringBuilder2.ToString()));
			builder.Append("\n");
		}
		if (stringBuilder3.Length > 0)
		{
			string text8 = "cardsDispel";
			if (targetSide == Enums.CardTargetSide.Self)
			{
				text8 = "cardsDispelYour";
			}
			builder.Append(string.Format(Texts.Instance.GetText(text8), stringBuilder3.ToString()));
			builder.Append("\n");
		}
		if (id == "gardenofthorns")
		{
			Debug.Log("XXX");
		}
		if (healAuraCurseSelf != null)
		{
			string text9 = "cardsPurgeYour";
			if (!healAuraCurseSelf.IsAura)
			{
				text9 = "cardsDispelYour";
			}
			if (text9 == "cardsPurgeYour" || text9 == "cardsDispelYour")
			{
				if (targetSide == Enums.CardTargetSide.Friend || targetSide == Enums.CardTargetSide.FriendNotSelf)
				{
					if ((SpecialValueGlobal != Enums.CardSpecialValue.AuraCurseYours || !(SpecialAuraCurseNameGlobal != null) || !(SpecialValueModifierGlobal > 0f)) && !AuraCurseIsPartOfTransformAura(healAuraCurseSelf.Id))
					{
						builder.Append(string.Format(Texts.Instance.GetText(text9), SpriteText(healAuraCurseSelf.ACName)));
					}
				}
				else if (!AuraCurseIsPartOfTransformAura(healAuraCurseSelf.Id))
				{
					builder.Append(string.Format(Texts.Instance.GetText(text9), SpriteText(healAuraCurseSelf.ACName)));
				}
			}
			builder.Append("\n");
		}
		if (healCurses > 0)
		{
			builder.Append(string.Format(Texts.Instance.GetText("cardsDispel"), ColorTextArray("curse", healCurses.ToString())));
			builder.Append("\n");
		}
		if (healCurses == -1)
		{
			builder.Append(Texts.Instance.GetText("cardsDispelAll"));
			builder.Append("\n");
		}
		if (dispelAuras > 0)
		{
			builder.Append(string.Format(Texts.Instance.GetText("cardsPurge"), ColorTextArray("aura", dispelAuras.ToString())));
			builder.Append("\n");
		}
		else if (dispelAuras == -1)
		{
			builder.Append(Texts.Instance.GetText("cardsPurgeAll"));
			builder.Append("\n");
		}
		if (transferCurses > 0)
		{
			builder.Append(string.Format(Texts.Instance.GetText("cardsTransferCurse"), transferCurses.ToString()));
			builder.Append("\n");
		}
		if (stealAuras > 0)
		{
			builder.Append(string.Format(Texts.Instance.GetText("cardsStealAuras"), stealAuras.ToString()));
			builder.Append("\n");
		}
		if (increaseAuras > 0)
		{
			builder.Append(string.Format(Texts.Instance.GetText("cardsIncreaseAura"), increaseAuras.ToString()));
			builder.Append("\n");
		}
		if (increaseCurses > 0)
		{
			builder.Append(string.Format(Texts.Instance.GetText("cardsIncreaseCurse"), increaseCurses.ToString()));
			builder.Append("\n");
		}
		if (reduceAuras > 0)
		{
			builder.Append(string.Format(Texts.Instance.GetText("cardsReduceAura"), reduceAuras.ToString()));
			builder.Append("\n");
		}
		if (reduceCurses > 0)
		{
			builder.Append(string.Format(Texts.Instance.GetText("cardsReduceCurse"), reduceCurses.ToString()));
			builder.Append("\n");
		}
		bool flag2 = false;
		if (damage > 0)
		{
			if (damage > 0 && damage2 > 0 && damageType == damageType2 && !damageSpecialValueGlobal && !damageSpecialValue1 && !damageSpecialValue2 && !damage2SpecialValueGlobal && !damage2SpecialValue1 && !damage2SpecialValue2)
			{
				aux.Append(ColorTextArray("damage", NumFormat(damagePreCalculatedCombined), SpriteText(Enum.GetName(typeof(Enums.DamageType), damageType))));
				flag2 = true;
			}
			else
			{
				if (damage == 1 && (damageSpecialValueGlobal || damageSpecialValue1 || damageSpecialValue2))
				{
					aux.Append(ColorTextArray("damage", "X", SpriteText(Enum.GetName(typeof(Enums.DamageType), damageType))));
				}
				else
				{
					aux.Append(ColorTextArray("damage", NumFormat(damagePreCalculated), SpriteText(Enum.GetName(typeof(Enums.DamageType), damageType))));
				}
				if (damage2 > 0)
				{
					flag2 = true;
					if (damage2 == 1 && (damage2SpecialValue1 || damage2SpecialValue2 || damage2SpecialValueGlobal))
					{
						if (damageType == damageType2)
						{
							aux.Append("<space=-.3>");
							aux.Append("+");
							aux.Append(ColorTextArray("damage", "X", SpriteText(Enum.GetName(typeof(Enums.DamageType), damageType))));
						}
						else
						{
							aux.Append(ColorTextArray("damage", "X", SpriteText(Enum.GetName(typeof(Enums.DamageType), damageType2))));
						}
					}
					else
					{
						aux.Append(ColorTextArray("damage", NumFormat(damagePreCalculated2), SpriteText(Enum.GetName(typeof(Enums.DamageType), damageType2))));
					}
				}
			}
			builder.Append(string.Format(Texts.Instance.GetText("cardsDealDamage"), aux.ToString()));
			builder.Append("\n");
			aux.Clear();
			if (damageSpecialValue1 || damageSpecialValue2 || damageSpecialValueGlobal || damage2SpecialValue1 || damage2SpecialValue2 || damage2SpecialValueGlobal)
			{
				AppendXEqualsString();
			}
		}
		if (!flag2 && damage2 > 0)
		{
			if (damage2 == 1 && (damage2SpecialValueGlobal || damage2SpecialValue1 || damage2SpecialValue2))
			{
				aux.Append(ColorTextArray("damage", "X", SpriteText(Enum.GetName(typeof(Enums.DamageType), damageType2))));
			}
			else
			{
				aux.Append(ColorTextArray("damage", NumFormat(damagePreCalculated2), SpriteText(Enum.GetName(typeof(Enums.DamageType), damageType2))));
			}
			builder.Append(string.Format(Texts.Instance.GetText("cardsDealDamage"), aux.ToString()));
			builder.Append("\n");
			aux.Clear();
			if (damageSpecialValue1 || damageSpecialValue2 || damageSpecialValueGlobal || damage2SpecialValue1 || damage2SpecialValue2 || damage2SpecialValueGlobal)
			{
				AppendXEqualsString();
			}
		}
		StringBuilder stringBuilder4 = new StringBuilder();
		if (damageSides > 0)
		{
			if (damageSpecialValueGlobal)
			{
				stringBuilder4.Append(ColorTextArray("damage", "X", SpriteText(Enum.GetName(typeof(Enums.DamageType), damageType))));
			}
			else
			{
				stringBuilder4.Append(ColorTextArray("damage", NumFormat(damageSidesPreCalculated), SpriteText(Enum.GetName(typeof(Enums.DamageType), damageType))));
			}
		}
		if (damageSides2 > 0)
		{
			if (damage2SpecialValueGlobal)
			{
				stringBuilder4.Append(ColorTextArray("damage", "X", SpriteText(Enum.GetName(typeof(Enums.DamageType), damageType2))));
			}
			else
			{
				stringBuilder4.Append(ColorTextArray("damage", NumFormat(damageSidesPreCalculated2), SpriteText(Enum.GetName(typeof(Enums.DamageType), damageType2))));
			}
		}
		if (stringBuilder4.Length > 0)
		{
			builder.Append(string.Format(Texts.Instance.GetText("cardsTargetSides"), stringBuilder4.ToString()));
			stringBuilder4.Clear();
			builder.Append("\n");
			AppendXEqualsString();
		}
		num = 0;
		if (damageSelf > 0)
		{
			num++;
			if (damageSpecialValueGlobal || damageSpecialValue1 || damageSpecialValue2)
			{
				aux.Append(ColorTextArray("damage", "X", SpriteText(Enum.GetName(typeof(Enums.DamageType), damageType))));
			}
			else
			{
				aux.Append(ColorTextArray("damage", NumFormat(damageSelfPreCalculated), SpriteText(Enum.GetName(typeof(Enums.DamageType), damageType))));
			}
		}
		if (damageSelf2 > 0)
		{
			num++;
			if (damage2SpecialValueGlobal || damage2SpecialValue1 || damage2SpecialValue2)
			{
				aux.Append(ColorTextArray("damage", "X", SpriteText(Enum.GetName(typeof(Enums.DamageType), damageType2))));
			}
			else
			{
				aux.Append(ColorTextArray("damage", NumFormat(damageSelfPreCalculated2), SpriteText(Enum.GetName(typeof(Enums.DamageType), damageType2))));
			}
		}
		if (num > 0)
		{
			if (num > 2)
			{
				aux.Insert(0, br1);
				aux.Insert(0, "\n");
			}
			if (targetSide == Enums.CardTargetSide.Self)
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsSuffer"), aux.ToString()));
				builder.Append("\n");
			}
			else
			{
				stringBuilder.Append(string.Format(Texts.Instance.GetText("cardsYouSuffer"), aux.ToString()));
				stringBuilder.Append("\n");
			}
			aux.Clear();
			AppendXEqualsString();
		}
		if (stringBuilder.Length > 0)
		{
			builder.Append(stringBuilder.ToString());
			stringBuilder.Clear();
			AppendXEqualsString();
		}
		if (healSelfPerDamageDonePercent > 0f)
		{
			builder.Append(string.Format(Texts.Instance.GetText("cardsHealSelfPerDamage"), healSelfPerDamageDonePercent.ToString()));
			builder.Append("\n");
		}
		if (selfHealthLoss != 0 && !selfHealthLossSpecialGlobal)
		{
			if (selfHealthLossSpecialValue1)
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsLoseHp"), ColorTextArray("damage", "X HP")));
			}
			else
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsLoseHp"), ColorTextArray("damage", NumFormat(selfHealthLoss), "HP")));
			}
			builder.Append("\n");
		}
		if ((targetSide == Enums.CardTargetSide.Friend || targetSide == Enums.CardTargetSide.FriendNotSelf) && SpecialValueGlobal == Enums.CardSpecialValue.AuraCurseYours && SpecialAuraCurseNameGlobal != null && SpecialValueModifierGlobal > 0f)
		{
			isShare = true;
			builder.Append(string.Format(Texts.Instance.GetText("cardsShareYour"), SpriteText(SpecialAuraCurseNameGlobal.ACName)));
			builder.Append("\n");
		}
		isTransform = false;
		if (!Damage2SpecialValue1 && !isShare && (specialValueGlobal != Enums.CardSpecialValue.None || specialValue1 != Enums.CardSpecialValue.None || specialValue2 != Enums.CardSpecialValue.None))
		{
			if (!damageSpecialValueGlobal && !damageSpecialValue1 && !damageSpecialValue2)
			{
				if (targetSide == Enums.CardTargetSide.Self && (specialValueGlobal == Enums.CardSpecialValue.AuraCurseTarget || specialValueGlobal == Enums.CardSpecialValue.AuraCurseYours))
				{
					if (specialAuraCurseNameGlobal != null)
					{
						text = SpriteText(specialAuraCurseNameGlobal.ACName);
					}
					if (auraChargesSpecialValueGlobal)
					{
						if (aura != null)
						{
							text2 = SpriteText(aura.ACName);
						}
						if (aura2 != null && auraCharges2SpecialValueGlobal)
						{
							text3 = SpriteText(aura2.ACName);
						}
					}
					else if (curseChargesSpecialValueGlobal)
					{
						if (curse != null)
						{
							text2 = SpriteText(curse.ACName);
						}
						if (curse2 != null && curseCharges2SpecialValueGlobal)
						{
							text3 = SpriteText(curse2.ACName);
						}
					}
				}
				else if (specialValue1 == Enums.CardSpecialValue.AuraCurseTarget)
				{
					if (specialAuraCurseName1 != null)
					{
						text = SpriteText(specialAuraCurseName1.ACName);
					}
					if (auraChargesSpecialValue1)
					{
						if (aura != null)
						{
							text2 = SpriteText(aura.ACName);
						}
						if (aura2 != null && auraCharges2SpecialValue1)
						{
							text3 = SpriteText(aura2.ACName);
						}
					}
					else if (curseChargesSpecialValue1)
					{
						if (curse != null)
						{
							text2 = SpriteText(curse.ACName);
						}
						if (curse2 != null && CurseCharges2SpecialValue1)
						{
							text3 = SpriteText(curse2.ACName);
						}
					}
				}
				if (text != "" && text2 != "")
				{
					isTransform = true;
					if (text == text2)
					{
						if (specialValueGlobal == Enums.CardSpecialValue.AuraCurseTarget)
						{
							if (specialValueModifierGlobal == 100f)
							{
								builder.Append(string.Format(Texts.Instance.GetText("cardsDoubleTarget"), text));
								builder.Append("\n");
							}
							else if (specialValueModifierGlobal == 200f)
							{
								builder.Append(string.Format(Texts.Instance.GetText("cardsTripleTarget"), text));
								builder.Append("\n");
							}
							else if (specialValueModifierGlobal < 100f)
							{
								builder.Append(string.Format(Texts.Instance.GetText("cardsLosePercentTarget"), 100f - specialValueModifierGlobal, text));
								builder.Append("\n");
							}
						}
						else if (specialValueGlobal == Enums.CardSpecialValue.AuraCurseYours)
						{
							if (specialValueModifierGlobal == 100f)
							{
								builder.Append(string.Format(Texts.Instance.GetText("cardsDoubleYour"), text));
								builder.Append("\n");
							}
							else if (specialValueModifierGlobal == 200f)
							{
								builder.Append(string.Format(Texts.Instance.GetText("cardsTripleYour"), text));
								builder.Append("\n");
							}
							else if (specialValueModifierGlobal < 100f)
							{
								builder.Append(string.Format(Texts.Instance.GetText("cardsLosePercentYour"), 100f - specialValueModifierGlobal, text));
								builder.Append("\n");
							}
						}
						else if (specialValue1 == Enums.CardSpecialValue.AuraCurseTarget)
						{
							if (specialValueModifier1 == 100f)
							{
								builder.Append(string.Format(Texts.Instance.GetText("cardsDoubleTarget"), text));
								builder.Append("\n");
							}
							else if (specialValueModifier1 == 200f)
							{
								builder.Append(string.Format(Texts.Instance.GetText("cardsTripleTarget"), text));
								builder.Append("\n");
							}
							else if (specialValueModifier1 < 100f && healAuraCurseName != null)
							{
								builder.Append(string.Format(Texts.Instance.GetText("cardsLosePercentTarget"), 100f - specialValueModifier1, text));
								builder.Append("\n");
							}
							else
							{
								isTransform = false;
							}
						}
						else if (specialValue1 == Enums.CardSpecialValue.AuraCurseYours)
						{
							if (specialValueModifier1 == 100f)
							{
								builder.Append(string.Format(Texts.Instance.GetText("cardsDoubleYour"), text));
								builder.Append("\n");
							}
							else if (specialValueModifier1 == 200f)
							{
								builder.Append(string.Format(Texts.Instance.GetText("cardsTripleYour"), text));
								builder.Append("\n");
							}
							else if (specialValueModifier1 < 100f)
							{
								builder.Append(string.Format(Texts.Instance.GetText("cardsLosePercentYour"), 100f - specialValueModifier1, text));
								builder.Append("\n");
							}
						}
					}
					else
					{
						aux.Append(text2);
						if (specialValueModifier1 > 0f)
						{
							equalsMultiplier = specialValueModifier1 / 100f;
						}
						if (equalsMultiplier > 0f && equalsMultiplier != 1f)
						{
							equalsMultiplierString = "x " + equalsMultiplier;
						}
						if (equalsMultiplierString != "")
						{
							aux.Append(" <c>");
							aux.Append(equalsMultiplierString);
							aux.Append("</c>");
						}
						if (text3 != "")
						{
							builder.Append(string.Format(Texts.Instance.GetText("cardsTransformIntoAnd"), text, aux.ToString(), text3));
						}
						else
						{
							builder.Append(string.Format(Texts.Instance.GetText("cardsTransformInto"), text, aux.ToString()));
						}
						builder.Append("\n");
						aux.Clear();
						equalsMultiplier = 0f;
						equalsMultiplierString = "";
					}
				}
			}
			text = "";
			text2 = "";
			text3 = "";
		}
		if (energyRechargeSpecialValueGlobal)
		{
			aux.Append(ColorTextArray("aura", "X", SpriteText("energy")));
		}
		AuraCurseData auraCurseData = null;
		if (!isShare && !isTransform)
		{
			num = 0;
			if (aura != null && (auraChargesSpecialValue1 || auraChargesSpecialValueGlobal))
			{
				num++;
				aux.Append(ColorTextArray("aura", "X", SpriteText(aura.ACName)));
				auraCurseData = aura;
			}
			if (aura2 != null && (auraCharges2SpecialValue1 || auraCharges2SpecialValueGlobal))
			{
				num++;
				if (aura != null && aura == aura2)
				{
					aux.Append(ColorTextArray("aura", NumFormat(GetFinalAuraCharges(aura.Id, auraCharges, character)), SpriteText(aura.ACName)));
					aux.Append("+");
				}
				aux.Append(ColorTextArray("aura", "X", SpriteText(aura2.ACName)));
				auraCurseData = aura2;
			}
			if (aura3 != null && (auraCharges3SpecialValue1 || auraCharges3SpecialValueGlobal))
			{
				num++;
				if (aura != null && aura == aura3)
				{
					aux.Append(ColorTextArray("aura", NumFormat(GetFinalAuraCharges(aura.Id, auraCharges, character)), SpriteText(aura.ACName)));
					aux.Append("+");
				}
				if (aura2 != null && aura2 == aura3)
				{
					aux.Append(ColorTextArray("aura", NumFormat(GetFinalAuraCharges(aura2.Id, auraCharges2, character)), SpriteText(aura3.ACName)));
					aux.Append("+");
				}
				aux.Append(ColorTextArray("aura", "X", SpriteText(aura3.ACName)));
				auraCurseData = aura3;
			}
			if (num > 0)
			{
				if (targetSide == Enums.CardTargetSide.Self)
				{
					builder.Append(string.Format(Texts.Instance.GetText("cardsGain"), aux.ToString()));
				}
				else
				{
					builder.Append(string.Format(Texts.Instance.GetText("cardsGrant"), aux.ToString()));
				}
				builder.Append("\n");
				aux.Clear();
				AppendXEqualsString();
			}
		}
		StringBuilder stringBuilder5 = new StringBuilder();
		new StringBuilder();
		if (!isShare && !isTransform)
		{
			num = 0;
			if (curse != null && (curseChargesSpecialValue1 || curseChargesSpecialValueGlobal))
			{
				num++;
				aux.Append(ColorTextArray("curse", "X", SpriteText(curse.ACName)));
			}
			if (curse2 != null && (curseCharges2SpecialValue1 || curseCharges2SpecialValueGlobal))
			{
				num++;
				aux.Append(ColorTextArray("curse", "X", SpriteText(curse2.ACName)));
			}
			if (curse3 != null && (curseCharges3SpecialValue1 || curseCharges3SpecialValueGlobal))
			{
				num++;
				aux.Append(ColorTextArray("curse", "X", SpriteText(curse3.ACName)));
			}
			if (num > 0)
			{
				if (targetSide == Enums.CardTargetSide.Self)
				{
					builder.Append(string.Format(Texts.Instance.GetText("cardsSuffer"), aux.ToString()));
					builder.Append("\n");
					AppendXEqualsString();
				}
				else
				{
					stringBuilder5.Append(aux);
				}
				aux.Clear();
			}
			num = 0;
			if (curseSelf != null && (curseChargesSpecialValue1 || curseChargesSpecialValueGlobal))
			{
				num++;
				aux.Append(ColorTextArray("curse", "X", SpriteText(curseSelf.ACName)));
			}
			if (curseSelf2 != null && (curseCharges2SpecialValue1 || curseCharges2SpecialValueGlobal))
			{
				num++;
				aux.Append(ColorTextArray("curse", "X", SpriteText(curseSelf2.ACName)));
			}
			if (curseSelf3 != null && (curseCharges3SpecialValue1 || curseCharges3SpecialValueGlobal))
			{
				num++;
				aux.Append(ColorTextArray("curse", "X", SpriteText(curseSelf3.ACName)));
			}
			if (num > 0)
			{
				if (targetSide == Enums.CardTargetSide.Self || curseSelf != null || curseSelf2 != null || curseSelf3 != null)
				{
					if (targetSide == Enums.CardTargetSide.Self)
					{
						builder.Append(string.Format(Texts.Instance.GetText("cardsSuffer"), aux.ToString()));
						builder.Append("\n");
					}
					else if (!flag)
					{
						stringBuilder.Append(string.Format(Texts.Instance.GetText("cardsYouSuffer"), aux.ToString()));
						stringBuilder.Append("\n");
					}
					else
					{
						builder.Append(string.Format(Texts.Instance.GetText("cardsYouSuffer"), aux.ToString()));
						builder.Append("\n");
					}
				}
				else
				{
					builder.Append(string.Format(Texts.Instance.GetText("cardsApply"), aux.ToString()));
					builder.Append("\n");
				}
				aux.Clear();
				AppendXEqualsString();
			}
		}
		num = 0;
		if (heal > 0 && (healSpecialValue1 || healSpecialValueGlobal))
		{
			aux.Append(ColorTextArray("heal", "X", SpriteText("heal")));
			num++;
		}
		if (aux.Length > 0)
		{
			builder.Append(string.Format(Texts.Instance.GetText("cardsHeal"), aux.ToString()));
			builder.Append("\n");
			aux.Clear();
			AppendXEqualsString(alwaysShow: true);
		}
		num = 0;
		if (healSelf > 0 && (healSelfSpecialValue1 || healSelfSpecialValueGlobal))
		{
			aux.Append(ColorTextArray("heal", "X", SpriteText("heal")));
			num++;
		}
		if (aux.Length > 0)
		{
			builder.Append(string.Format(Texts.Instance.GetText("cardsHealSelf"), aux.ToString()));
			builder.Append("\n");
			aux.Clear();
			AppendXEqualsString();
		}
		if (selfHealthLoss > 0 && selfHealthLossSpecialGlobal)
		{
			builder.Append(string.Format(Texts.Instance.GetText("cardsYouLose"), ColorTextArray("damage", "X", "HP")));
			builder.Append("\n");
			aux.Clear();
			AppendXEqualsString();
		}
		num = 0;
		if (curseSelf != null && curseCharges > 0)
		{
			num++;
			aux.Append(ColorTextArray("curse", NumFormat(GetFinalAuraCharges(curseSelf.Id, curseCharges, character)), SpriteText(curseSelf.ACName)));
		}
		if (curseSelf2 != null && curseCharges2 > 0)
		{
			num++;
			aux.Append(ColorTextArray("curse", NumFormat(GetFinalAuraCharges(curseSelf2.Id, curseCharges2, character)), SpriteText(curseSelf2.ACName)));
		}
		if (curseSelf3 != null && curseCharges3 > 0)
		{
			num++;
			aux.Append(ColorTextArray("curse", NumFormat(GetFinalAuraCharges(curseSelf3.Id, curseCharges3, character)), SpriteText(curseSelf3.ACName)));
		}
		if (num > 0)
		{
			if (num > 2)
			{
				aux.Insert(0, br1);
				aux.Insert(0, "\n");
			}
			if (targetSide == Enums.CardTargetSide.Self)
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsSuffer"), aux.ToString()));
				builder.Append("\n");
			}
			else
			{
				stringBuilder.Append(string.Format(Texts.Instance.GetText("cardsYouSuffer"), aux.ToString()));
				stringBuilder.Append("\n");
			}
			aux.Clear();
		}
		num = 0;
		if (energyRecharge < 0)
		{
			builder.Append(string.Format(Texts.Instance.GetText("cardsLoseHp"), aux.Append(ColorTextArray("system", NumFormat(Mathf.Abs(energyRecharge)), SpriteText("energy")))));
			builder.Append("\n");
			aux.Clear();
		}
		if (energyRecharge > 0)
		{
			num++;
			aux.Append(ColorTextArray("system", NumFormat(energyRecharge), SpriteText("energy")));
		}
		if (aura != null && auraCharges > 0 && aura != auraCurseData)
		{
			num++;
			aux.Append(ColorTextArray("aura", NumFormat(GetFinalAuraCharges(aura.Id, auraCharges, character)), SpriteText(aura.ACName)));
		}
		if (aura2 != null && auraCharges2 > 0 && aura2 != auraCurseData)
		{
			num++;
			aux.Append(ColorTextArray("aura", NumFormat(GetFinalAuraCharges(aura2.Id, auraCharges2, character)), SpriteText(aura2.ACName)));
		}
		if (aura3 != null && auraCharges3 > 0 && aura3 != auraCurseData)
		{
			num++;
			aux.Append(ColorTextArray("aura", NumFormat(GetFinalAuraCharges(aura3.Id, auraCharges3, character)), SpriteText(aura3.ACName)));
		}
		if (auras != null && auras.Length != 0)
		{
			AuraBuffs[] array = auras;
			foreach (AuraBuffs auraBuffs in array)
			{
				if (auraBuffs != null && auraBuffs.aura != null && auraBuffs.auraCharges > 0 && auraBuffs.aura != auraCurseData)
				{
					num++;
					aux.Append(ColorTextArray("aura", NumFormat(GetFinalAuraCharges(auraBuffs.aura.Id, auraBuffs.auraCharges, character)), SpriteText(auraBuffs.aura.ACName)));
				}
			}
		}
		if (num > 0)
		{
			if (num > 2)
			{
				aux.Insert(0, br1);
				aux.Insert(0, "\n");
			}
			if (targetSide == Enums.CardTargetSide.Self)
			{
				if (ChooseOneOfAvailableAuras)
				{
					builder.Append(string.Format(Texts.Instance.GetText("cardsGainOneOf"), aux.ToString()));
				}
				else
				{
					builder.Append(string.Format(Texts.Instance.GetText("cardsGain"), aux.ToString()));
				}
			}
			else if (ChooseOneOfAvailableAuras)
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsGrantOneOf"), aux.ToString()));
			}
			else
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsGrant"), aux.ToString()));
			}
			builder.Append("\n");
			aux.Clear();
		}
		num = 0;
		if (auraSelf != null && auraCharges > 0)
		{
			num++;
			aux.Append(ColorTextArray("aura", NumFormat(GetFinalAuraCharges(auraSelf.Id, auraCharges, character)), SpriteText(auraSelf.ACName)));
		}
		if (auraSelf2 != null && auraCharges2 > 0)
		{
			num++;
			aux.Append(ColorTextArray("aura", NumFormat(GetFinalAuraCharges(auraSelf2.Id, auraCharges2, character)), SpriteText(auraSelf2.ACName)));
		}
		if (auraSelf3 != null && auraCharges3 > 0)
		{
			num++;
			aux.Append(ColorTextArray("aura", NumFormat(GetFinalAuraCharges(auraSelf3.Id, auraCharges3, character)), SpriteText(auraSelf3.ACName)));
		}
		if (auras != null && auras.Length != 0)
		{
			AuraBuffs[] array = auras;
			foreach (AuraBuffs auraBuffs2 in array)
			{
				if (auraBuffs2 != null && auraBuffs2.auraSelf != null && auraBuffs2.auraCharges > 0)
				{
					num++;
					aux.Append(ColorTextArray("aura", NumFormat(GetFinalAuraCharges(auraBuffs2.auraSelf.Id, auraBuffs2.auraCharges, character)), SpriteText(auraBuffs2.auraSelf.ACName)));
				}
			}
		}
		if (!isShare)
		{
			if (auraSelf != null && (auraChargesSpecialValue1 || auraChargesSpecialValueGlobal))
			{
				num++;
				aux.Append(ColorTextArray("aura", "X", SpriteText(auraSelf.ACName)));
			}
			if (auraSelf2 != null && (auraCharges2SpecialValue1 || auraCharges2SpecialValueGlobal))
			{
				num++;
				aux.Append(ColorTextArray("aura", "X", SpriteText(auraSelf2.ACName)));
			}
			if (auraSelf3 != null && (auraCharges3SpecialValue1 || auraCharges3SpecialValueGlobal))
			{
				num++;
				aux.Append(ColorTextArray("aura", "X", SpriteText(auraSelf3.ACName)));
			}
		}
		if (num > 0)
		{
			if (num > 2)
			{
				aux.Insert(0, br1);
				aux.Insert(0, "\n");
			}
			if (targetSide == Enums.CardTargetSide.Self)
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsGain"), aux.ToString()));
				builder.Append("\n");
			}
			else
			{
				stringBuilder.Append(string.Format(Texts.Instance.GetText("cardsYouGain"), aux.ToString()));
				stringBuilder.Append("\n");
			}
			aux.Clear();
			AppendXEqualsString();
		}
		if (stringBuilder.Length > 0)
		{
			builder.Append(stringBuilder.ToString());
		}
		num = 0;
		if (curseCharges > 0 && curse != null)
		{
			num++;
			aux.Append(ColorTextArray("curse", NumFormat(GetFinalAuraCharges(curse.Id, curseCharges, character)), SpriteText(curse.ACName)));
		}
		if (curseCharges2 > 0 && curse2 != null)
		{
			num++;
			aux.Append(ColorTextArray("curse", NumFormat(GetFinalAuraCharges(curse2.Id, curseCharges2, character)), SpriteText(curse2.ACName)));
		}
		if (curseCharges3 > 0 && curse3 != null)
		{
			num++;
			aux.Append(ColorTextArray("curse", NumFormat(GetFinalAuraCharges(curse3.Id, curseCharges3, character)), SpriteText(curse3.ACName)));
		}
		if (stringBuilder5.Length > 0)
		{
			aux.Append(stringBuilder5);
		}
		if (num > 0)
		{
			if (num > 2)
			{
				aux.Insert(0, br1);
				aux.Insert(0, "\n");
			}
			if (targetSide == Enums.CardTargetSide.Self)
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsSuffer"), aux.ToString()));
			}
			else
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsApply"), aux.ToString()));
			}
			builder.Append("\n");
			aux.Clear();
			AppendXEqualsString();
		}
		else if (stringBuilder5.Length > 0)
		{
			if (targetSide == Enums.CardTargetSide.Self)
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsSuffer"), aux.ToString()));
			}
			else
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsApply"), aux.ToString()));
			}
			builder.Append("\n");
			aux.Clear();
			AppendXEqualsString();
		}
		if (curseChargesSides > 0)
		{
			builder.Append(string.Format(Texts.Instance.GetText("cardsTargetSides"), ColorTextArray("curse", NumFormat(GetFinalAuraCharges(curse.Id, curseChargesSides, character)), SpriteText(curse.ACName))));
			builder.Append("\n");
			aux.Clear();
		}
		if (heal > 0 && !healSpecialValue1 && !healSpecialValueGlobal)
		{
			aux.Append(ColorTextArray("heal", NumFormat(healPreCalculated), SpriteText("heal")));
			builder.Append(string.Format(Texts.Instance.GetText("cardsHeal"), aux.ToString()));
			builder.Append("\n");
			aux.Clear();
		}
		if (healSelf > 0 && !healSelfSpecialValue1 && !healSelfSpecialValueGlobal)
		{
			aux.Append(ColorTextArray("heal", NumFormat(healSelfPreCalculated), SpriteText("heal")));
			builder.Append(string.Format(Texts.Instance.GetText("cardsHealSelf"), aux.ToString()));
			builder.Append("\n");
			aux.Clear();
		}
		if (allowPurgeDispelText)
		{
			if (healAuraCurseName != null && AuraCurseIsPartOfTransformAura(healAuraCurseName.Id))
			{
				if (targetSide == Enums.CardTargetSide.Self)
				{
					if (healAuraCurseName.IsAura)
					{
						builder.Append(string.Format(Texts.Instance.GetText("cardsPurgeYour"), SpriteText(healAuraCurseName.ACName)));
					}
					else
					{
						builder.Append(string.Format(Texts.Instance.GetText("cardsDispelYour"), SpriteText(healAuraCurseName.ACName)));
					}
				}
				else if (healAuraCurseName.IsAura)
				{
					builder.Append(string.Format(Texts.Instance.GetText("cardsPurge"), SpriteText(healAuraCurseName.ACName)));
				}
				else
				{
					builder.Append(string.Format(Texts.Instance.GetText("cardsDispel"), SpriteText(healAuraCurseName.ACName)));
				}
				builder.Append("\n");
			}
			if (healAuraCurseSelf != null && AuraCurseIsPartOfTransformAura(healAuraCurseSelf.Id))
			{
				if (healAuraCurseSelf.IsAura)
				{
					builder.Append(string.Format(Texts.Instance.GetText("cardsPurgeYour"), SpriteText(healAuraCurseSelf.ACName)));
				}
				else
				{
					builder.Append(string.Format(Texts.Instance.GetText("cardsDispelYour"), SpriteText(healAuraCurseSelf.ACName)));
				}
				builder.Append("\n");
			}
		}
		if (PetActivation == Enums.ActivePets.CurrentTarget)
		{
			builder.Append(Texts.Instance.GetText("cardsActivateAHeroPet"));
			builder.Append("\n");
		}
		if (PetBonusDamageType != Enums.DamageType.None)
		{
			builder.Append(string.Format(Texts.Instance.GetText("cardsAddPetBonusDamage"), ColorTextArray("damage", NumFormat(PetBonusDamageAmount), SpriteText(Enum.GetName(typeof(Enums.DamageType), PetBonusDamageType)))));
			builder.Append("\n");
		}
		if (killPet)
		{
			builder.Append(Texts.Instance.GetText("killPet"));
			builder.Append("\n");
		}
		if (damageEnergyBonus > 0 || healEnergyBonus > 0 || acEnergyBonus != null)
		{
			StringBuilder stringBuilder6 = new StringBuilder();
			stringBuilder6.Append("<line-height=30%><br></line-height>");
			StringBuilder stringBuilder7 = new StringBuilder();
			stringBuilder7.Append(grColor);
			stringBuilder7.Append("[");
			stringBuilder7.Append(Texts.Instance.GetText("overchargeAcronym"));
			stringBuilder7.Append("]");
			stringBuilder7.Append(endColor);
			stringBuilder7.Append("  ");
			if (damageEnergyBonus > 0)
			{
				stringBuilder6.Append(stringBuilder7.ToString());
				stringBuilder6.Append(string.Format(Texts.Instance.GetText("cardsDealDamage"), ColorTextArray("damage", NumFormat(damageEnergyBonus), SpriteText(Enum.GetName(typeof(Enums.DamageType), damageType)))));
				stringBuilder6.Append("\n");
			}
			if (acEnergyBonus != null)
			{
				aux.Append(ColorTextArray("aura", NumFormat(acEnergyBonusQuantity), SpriteText(acEnergyBonus.ACName)));
				if (acEnergyBonus2 != null)
				{
					aux.Append(" ");
					aux.Append(ColorTextArray("aura", NumFormat(acEnergyBonus2Quantity), SpriteText(acEnergyBonus2.ACName)));
				}
				if (acEnergyBonus.IsAura)
				{
					if (targetSide == Enums.CardTargetSide.Self)
					{
						stringBuilder6.Append(stringBuilder7.ToString());
						stringBuilder6.Append(string.Format(Texts.Instance.GetText("cardsGain"), aux.ToString()));
					}
					else
					{
						stringBuilder6.Append(stringBuilder7.ToString());
						stringBuilder6.Append(string.Format(Texts.Instance.GetText("cardsGrant"), aux.ToString()));
					}
				}
				else if (!acEnergyBonus.IsAura)
				{
					if (targetSide == Enums.CardTargetSide.Self)
					{
						stringBuilder6.Append(stringBuilder7.ToString());
						stringBuilder6.Append(string.Format(Texts.Instance.GetText("cardsSuffer"), aux.ToString()));
					}
					else
					{
						stringBuilder6.Append(stringBuilder7.ToString());
						stringBuilder6.Append(string.Format(Texts.Instance.GetText("cardsApply"), aux.ToString()));
					}
				}
				stringBuilder6.Append("\n");
			}
			if (healEnergyBonus > 0)
			{
				stringBuilder6.Append(stringBuilder7.ToString());
				stringBuilder6.Append(string.Format(Texts.Instance.GetText("cardsHeal"), ColorTextArray("heal", NumFormat(healEnergyBonus), SpriteText("heal"))));
				stringBuilder6.Append("\n");
			}
			builder.Append(stringBuilder6.ToString());
			aux.Clear();
			stringBuilder6.Clear();
		}
		if (effectRepeat > 1 || effectRepeatMaxBonus > 0)
		{
			builder.Append(br1);
			builder.Append("<nobr><size=-.05><color=#1A505A>- ");
			if (effectRepeatMaxBonus > 0)
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsRepeatUpTo"), effectRepeatMaxBonus));
			}
			else if (effectRepeatTarget == Enums.EffectRepeatTarget.Chain)
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsRepeatChain"), effectRepeat - 1));
			}
			else if (effectRepeatTarget == Enums.EffectRepeatTarget.NoRepeat)
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsRepeatJump"), effectRepeat - 1));
			}
			else
			{
				builder.Append(string.Format(Texts.Instance.GetText("cardsRepeat"), effectRepeat - 1));
			}
			if (effectRepeatModificator != 0 && effectRepeatTarget != Enums.EffectRepeatTarget.Chain)
			{
				builder.Append(" (");
				if (effectRepeatModificator > 0)
				{
					builder.Append("+");
				}
				builder.Append(effectRepeatModificator);
				if (Functions.SpaceBeforePercentSign())
				{
					builder.Append(" ");
				}
				builder.Append("%)");
			}
			builder.Append(" -</color></size></nobr>");
			builder.Append("\n");
			aux.Clear();
		}
		if (ignoreBlock || ignoreBlock2)
		{
			builder.Append(br1);
			builder.Append(grColor);
			builder.Append(Texts.Instance.GetText("cardsIgnoreBlock"));
			builder.Append(endColor);
			builder.Append("\n");
			aux.Clear();
		}
		if (goldGainQuantity != 0 && shardsGainQuantity != 0)
		{
			builder.Append(string.Format(Texts.Instance.GetText("customGainPerHeroAnd"), ColorTextArray("aura", goldGainQuantity.ToString(), SpriteText("gold")), ColorTextArray("aura", shardsGainQuantity.ToString(), SpriteText("dust"))));
			builder.Append("\n");
		}
		else if (goldGainQuantity != 0)
		{
			builder.Append(string.Format(Texts.Instance.GetText("customGainPerHero"), ColorTextArray("aura", goldGainQuantity.ToString(), SpriteText("gold"))));
			builder.Append("\n");
		}
		else if (shardsGainQuantity != 0)
		{
			builder.Append(string.Format(Texts.Instance.GetText("customGainPerHero"), ColorTextArray("aura", shardsGainQuantity.ToString(), SpriteText("dust"))));
			builder.Append("\n");
		}
		if (selfKillHiddenSeconds > 0f)
		{
			builder.Append(Texts.Instance.GetText("escapes"));
			builder.Append("\n");
		}
		stringBuilder = null;
		void AppendXEqualsString(bool alwaysShow = false)
		{
			StringBuilder stringBuilder8 = new StringBuilder();
			if ((!isShare && !isTransform) || alwaysShow)
			{
				if (specialValueModifierGlobal > 0f)
				{
					equalsMultiplier = specialValueModifierGlobal / 100f;
				}
				else if (specialValueModifier1 > 0f)
				{
					equalsMultiplier = specialValueModifier1 / 100f;
				}
				if (equalsMultiplier > 0f && equalsMultiplier != 1f)
				{
					equalsMultiplierString = "x" + equalsMultiplier;
				}
				if (equalsMultiplierString == "")
				{
					equalsMultiplierString = "<space=-.1>";
				}
				if (specialValue1 == Enums.CardSpecialValue.AuraCurseYours)
				{
					stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYour"), SpriteText(SpecialAuraCurseName1.ACName), equalsMultiplierString));
				}
				else if (specialValue1 == Enums.CardSpecialValue.AuraCurseTarget)
				{
					stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsXEqualsTarget"), SpriteText(SpecialAuraCurseName1.ACName), equalsMultiplierString));
				}
				else if (specialValueGlobal == Enums.CardSpecialValue.AuraCurseYours)
				{
					stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYour"), SpriteText(SpecialAuraCurseNameGlobal.ACName), equalsMultiplierString));
				}
				else if (specialValueGlobal == Enums.CardSpecialValue.AuraCurseTarget)
				{
					stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsXEqualsTarget"), SpriteText(SpecialAuraCurseNameGlobal.ACName), equalsMultiplierString));
				}
				if (specialValueGlobal == Enums.CardSpecialValue.HealthYours)
				{
					stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYourHp"), equalsMultiplierString));
				}
				else if (specialValue1 == Enums.CardSpecialValue.HealthYours)
				{
					stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYourHp"), equalsMultiplierString));
				}
				else if (specialValue1 == Enums.CardSpecialValue.HealthTarget)
				{
					stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsXEqualsTargetHp"), equalsMultiplierString));
				}
				if (specialValueGlobal == Enums.CardSpecialValue.SpeedYours)
				{
					stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYourSpeed"), equalsMultiplierString));
				}
				else if (specialValueGlobal == Enums.CardSpecialValue.SpeedTarget)
				{
					stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsXEqualsTargetSpeed"), equalsMultiplierString));
				}
				else if (specialValueGlobal == Enums.CardSpecialValue.SpeedDifference || specialValue1 == Enums.CardSpecialValue.SpeedDifference)
				{
					stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsXEqualsDifferenceSpeed"), equalsMultiplierString));
				}
				else if (specialValueGlobal == Enums.CardSpecialValue.DiscardedCards)
				{
					if (discardCardPlace == Enums.CardPlace.Vanish)
					{
						stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYourVanishedCards"), equalsMultiplierString));
					}
					else
					{
						stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYourDiscardedCards"), equalsMultiplierString));
					}
				}
				else if (specialValueGlobal == Enums.CardSpecialValue.VanishedCards)
				{
					stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYourVanishedCards"), equalsMultiplierString));
				}
				if (specialValueGlobal == Enums.CardSpecialValue.CardsHand || specialValue1 == Enums.CardSpecialValue.CardsHand || specialValue2 == Enums.CardSpecialValue.CardsHand)
				{
					stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYourHand"), SpriteText("card"), equalsMultiplierString));
				}
				else if (specialValueGlobal == Enums.CardSpecialValue.CardsDeck || specialValue1 == Enums.CardSpecialValue.CardsDeck || specialValue2 == Enums.CardSpecialValue.CardsDeck)
				{
					stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYourDeck"), SpriteText("card"), equalsMultiplierString));
				}
				else if (specialValueGlobal == Enums.CardSpecialValue.CardsDiscard || specialValue1 == Enums.CardSpecialValue.CardsDiscard || specialValue2 == Enums.CardSpecialValue.CardsDiscard)
				{
					stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYourDiscard"), SpriteText("card"), equalsMultiplierString));
				}
				else if (specialValueGlobal == Enums.CardSpecialValue.CardsVanish || specialValue1 == Enums.CardSpecialValue.CardsVanish || specialValue2 == Enums.CardSpecialValue.CardsVanish)
				{
					stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYourVanish"), SpriteText("card"), equalsMultiplierString));
				}
				else if (specialValueGlobal == Enums.CardSpecialValue.CardsDeckTarget || specialValue1 == Enums.CardSpecialValue.CardsDeckTarget || specialValue2 == Enums.CardSpecialValue.CardsDeckTarget)
				{
					stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsXEqualsTargetDeck"), SpriteText("card"), equalsMultiplierString));
				}
				else if (specialValueGlobal == Enums.CardSpecialValue.CardsDiscardTarget || specialValue1 == Enums.CardSpecialValue.CardsDiscardTarget || specialValue2 == Enums.CardSpecialValue.CardsDiscardTarget)
				{
					stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsXEqualsTargetDiscard"), SpriteText("card"), equalsMultiplierString));
				}
				else if (specialValueGlobal == Enums.CardSpecialValue.CardsVanishTarget || specialValue1 == Enums.CardSpecialValue.CardsVanishTarget || specialValue2 == Enums.CardSpecialValue.CardsVanishTarget)
				{
					stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsXEqualsTargetVanish"), SpriteText("card"), equalsMultiplierString));
				}
				else if (specialValueGlobal == Enums.CardSpecialValue.MissingHealthYours || specialValue1 == Enums.CardSpecialValue.MissingHealthYours)
				{
					stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYourMissingHealth"), equalsMultiplierString));
				}
				else if (specialValueGlobal == Enums.CardSpecialValue.MissingHealthTarget || specialValue1 == Enums.CardSpecialValue.MissingHealthTarget)
				{
					stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsXEqualsTargetMissingHealth"), equalsMultiplierString));
				}
				if (stringBuilder8.Length > 0)
				{
					allowPurgeDispelText = !alwaysShow;
					StringBuilder stringBuilder9 = new StringBuilder();
					stringBuilder9.Append(goldColor);
					stringBuilder9.Append(stringBuilder8);
					stringBuilder9.Append(endColor);
					stringBuilder9.Append("\n");
					builder = builder.Replace(stringBuilder9.ToString(), "");
					builder.Append(stringBuilder9);
					stringBuilder8.Clear();
					if (id == "bouncingshield")
					{
						Debug.Log("X");
					}
				}
			}
		}
		bool AuraCurseIsPartOfTransformAura(string auracurseId)
		{
			if (SpecialAuraCurseNameGlobal?.Id == auracurseId && (specialValueGlobal == Enums.CardSpecialValue.AuraCurseTarget || specialValueGlobal == Enums.CardSpecialValue.AuraCurseYours))
			{
				return true;
			}
			if (specialAuraCurseName1?.Id == auracurseId && (specialValue1 == Enums.CardSpecialValue.AuraCurseTarget || specialValue1 == Enums.CardSpecialValue.AuraCurseYours))
			{
				return true;
			}
			return false;
		}
	}

	public string SpecialModifierDescription(SpecialValues specialValues, ItemData itemData = null)
	{
		string result = "";
		switch (specialValues.Name)
		{
		case Enums.SpecialValueModifierName.RuneCharges:
			result = SpriteText("runered") + SpriteText("runeblue") + SpriteText("runegreen");
			break;
		case Enums.SpecialValueModifierName.DamageDealt:
			result = "Damage Dealt";
			break;
		case Enums.SpecialValueModifierName.AuracurseSetCharges:
		case Enums.SpecialValueModifierName.AuraCurseYours:
			result = SpriteText(itemData.AuraCurseSetted.Id);
			break;
		}
		return result;
	}

	private string NumFormatItem(int num, bool plus = false, bool percent = false)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(" <nobr>");
		if (num > 0)
		{
			stringBuilder.Append("<color=#263ABC><size=+.1>");
			if (plus)
			{
				stringBuilder.Append("+");
			}
		}
		else
		{
			stringBuilder.Append("<color=#720070><size=+.1>");
			if (plus)
			{
				stringBuilder.Append("-");
			}
		}
		stringBuilder.Append(Mathf.Abs(num));
		if (percent)
		{
			if (Functions.SpaceBeforePercentSign())
			{
				stringBuilder.Append(" ");
			}
			stringBuilder.Append("%");
		}
		stringBuilder.Append("</color></size></nobr>");
		return stringBuilder.ToString();
	}

	private string NumFormat(int num)
	{
		if (num < 0)
		{
			num = 0;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<size=+.1>");
		stringBuilder.Append(num);
		stringBuilder.Append("</size>");
		return stringBuilder.ToString();
	}

	public List<Enums.CardType> GetCardTypes()
	{
		if (cardTypeList == null)
		{
			cardTypeList = new List<Enums.CardType>();
			if (cardType != Enums.CardType.None)
			{
				cardTypeList.Add(cardType);
				for (int i = 0; i < cardTypeAux.Length; i++)
				{
					if (cardTypeAux[i] != Enums.CardType.None)
					{
						cardTypeList.Add(cardTypeAux[i]);
					}
				}
			}
		}
		return cardTypeList;
	}

	public bool HasCardType(Enums.CardType type)
	{
		if (cardType == type)
		{
			return true;
		}
		for (int i = 0; i < cardTypeAux.Length; i++)
		{
			if (cardTypeAux[i] == type)
			{
				return true;
			}
		}
		return false;
	}

	public void DoExhaust()
	{
		if ((bool)MatchManager.Instance)
		{
			Hero heroHeroActive = MatchManager.Instance.GetHeroHeroActive();
			if (heroHeroActive != null)
			{
				exhaustCounter += heroHeroActive.GetAuraCharges("Exhaust");
			}
		}
	}

	public bool GetIgnoreBlockBecausePurge()
	{
		return IsGoingToPurgeThisAC("block");
	}

	public bool IsGoingToPurgeThisAC(string acId)
	{
		acId = acId.ToLower();
		if (HealAuraCurseName?.Id == acId)
		{
			return true;
		}
		if (HealAuraCurseName2?.Id == acId)
		{
			return true;
		}
		if (HealAuraCurseName3?.Id == acId)
		{
			return true;
		}
		if (HealAuraCurseName4?.Id == acId)
		{
			return true;
		}
		return false;
	}

	public bool GetIgnoreBlock(int _index = 0)
	{
		if ((bool)MatchManager.Instance)
		{
			Hero heroHeroActive = MatchManager.Instance.GetHeroHeroActive();
			if (heroHeroActive != null && heroHeroActive.SubclassName == "archer" && AtOManager.Instance.CharacterHaveTrait("archer", "perforatingshots") && HasCardType(Enums.CardType.Ranged_Attack))
			{
				return true;
			}
		}
		return _index switch
		{
			0 => ignoreBlock, 
			1 => ignoreBlock2, 
			_ => false, 
		};
	}

	public void ResetExhaust()
	{
		exhaustCounter = 0;
	}

	public int GetCardFinalCost()
	{
		int num = EnergyCostOriginal;
		if ((bool)MatchManager.Instance)
		{
			if (EnergyReductionToZeroPermanent || EnergyReductionToZeroTemporal)
			{
				num = 0;
			}
			num = num - EnergyReductionPermanent - EnergyReductionTemporal;
			num = (((CardClass != Enums.CardClass.Special || Playable) && CardClass != Enums.CardClass.Boon && CardClass != Enums.CardClass.Injury && (CardClass != Enums.CardClass.Monster || Playable)) ? (num + ExhaustCounter) : 0);
			if (num < 0)
			{
				num = 0;
			}
		}
		return num;
	}

	public string ColorFromCardDataRarity(CardData _cData = null, bool _useLightVersion = false)
	{
		if (_cData == null)
		{
			_cData = this;
		}
		if (_cData != null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<color=#");
			if (_cData.cardUpgraded == Enums.CardUpgraded.A)
			{
				if (!_useLightVersion)
				{
					stringBuilder.Append(colorUpgradeBlue);
				}
				else
				{
					stringBuilder.Append(colorUpgradeBlueLight);
				}
			}
			else if (_cData.cardUpgraded == Enums.CardUpgraded.B)
			{
				if (!_useLightVersion)
				{
					stringBuilder.Append(colorUpgradeGold);
				}
				else
				{
					stringBuilder.Append(colorUpgradeGoldLight);
				}
			}
			else if (_cData.cardUpgraded == Enums.CardUpgraded.Rare)
			{
				if (!_useLightVersion)
				{
					stringBuilder.Append(colorUpgradeRare);
				}
				else
				{
					stringBuilder.Append(colorUpgradeRareLight);
				}
			}
			else if (!_useLightVersion)
			{
				stringBuilder.Append(colorUpgradePlain);
			}
			else
			{
				stringBuilder.Append(colorUpgradePlainLight);
			}
			stringBuilder.Append(">");
			return stringBuilder.ToString();
		}
		return "<color=#5E3016>";
	}

	public AudioClip GetSoundRelease(Hero hero, NPC npc)
	{
		bool flag = GameManager.Instance.ConfigUseLegacySounds;
		if (!flag && soundHitRework == null && soundReleaseRework == null)
		{
			flag = true;
		}
		if (flag)
		{
			if (hero != null)
			{
				if (hero.HeroData.HeroSubClass.Female && soundPreActionFemale != null)
				{
					return soundPreActionFemale;
				}
			}
			else if (npc != null && npc.NpcData.Female && soundPreActionFemale != null)
			{
				return soundPreActionFemale;
			}
			return soundPreAction;
		}
		if (hero != null)
		{
			if (srException2Class != null)
			{
				for (int i = 0; i < srException2Class.Count; i++)
				{
					if (srException2Class[i] != null && srException2Class[i].Id == hero.HeroData.HeroSubClass.Id)
					{
						return srException2Audio;
					}
				}
			}
			if (srException1Class != null)
			{
				for (int j = 0; j < srException1Class.Count; j++)
				{
					if (srException1Class[j] != null && srException1Class[j].Id == hero.HeroData.HeroSubClass.Id)
					{
						return srException1Audio;
					}
				}
			}
			if (srException0Class != null)
			{
				for (int k = 0; k < srException0Class.Count; k++)
				{
					if (srException0Class[k] != null && srException0Class[k].Id == hero.HeroData.HeroSubClass.Id)
					{
						return srException0Audio;
					}
				}
			}
		}
		return soundReleaseRework;
	}

	public AudioClip GetSoundHit()
	{
		bool flag = GameManager.Instance.ConfigUseLegacySounds;
		if (!flag && soundHitRework == null && soundReleaseRework == null)
		{
			flag = true;
		}
		if (flag)
		{
			return sound;
		}
		return soundHitRework;
	}

	public AudioClip GetSoundDrag()
	{
		if (GameManager.Instance.ConfigUseLegacySounds)
		{
			return null;
		}
		if (soundDragRework != null)
		{
			return soundDragRework;
		}
		return AudioManager.Instance.soundCardDrag;
	}
}
