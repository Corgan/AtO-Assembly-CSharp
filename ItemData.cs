using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item Data", order = 57)]
public class ItemData : ScriptableObject
{
	[SerializeField]
	private bool dropOnly;

	[SerializeField]
	private bool questItem;

	[SerializeField]
	private string id = "";

	[SerializeField]
	private bool cursedItem;

	[SerializeField]
	private Enums.EventActivation activation;

	[SerializeField]
	private Enums.ActivationManual activationManual;

	[SerializeField]
	private bool activationOnlyOnHeroes;

	[SerializeField]
	private bool activateOnReceive;

	[SerializeField]
	private bool preventApplyForHeroTarget;

	[SerializeField]
	private Sprite spriteBossDrop;

	[SerializeField]
	private bool isEnchantment;

	[Header("REQUISITE")]
	[SerializeField]
	private int exactRound;

	[SerializeField]
	private int roundCycle;

	[SerializeField]
	private int timesPerTurn;

	[SerializeField]
	private int timesPerCombat;

	[SerializeField]
	private AuraCurseData auraCurseSetted;

	[SerializeField]
	private AuraCurseData auraCurseSetted2;

	[SerializeField]
	private AuraCurseData auraCurseSetted3;

	[SerializeField]
	private int auraCurseNumForOneEvent;

	[SerializeField]
	private Enums.CardType castedCardType;

	[SerializeField]
	private bool emptyHand;

	[SerializeField]
	private bool usedEnergy;

	[SerializeField]
	private float lowerOrEqualPercentHP = 100f;

	[Header("Not show character bonus for aura/curse gain")]
	[SerializeField]
	private bool notShowCharacterBonus;

	[Header("Destroy after being used")]
	[SerializeField]
	private bool destroyAfterUse;

	[Header("Target for the weapon effects")]
	[SerializeField]
	private Enums.ItemTarget itemTarget;

	[SerializeField]
	private bool dontTargetBoss;

	[SerializeField]
	private Enums.ItemTarget overrideTargetText = Enums.ItemTarget.None;

	[Header("Pet Activation")]
	public Enums.ActivePets PetActivation;

	[Header("Draw Cards")]
	[SerializeField]
	private int drawCards;

	[SerializeField]
	private bool drawMultiplyByEnergyUsed;

	[Header("Gain Card ID")]
	[SerializeField]
	private int cardNum;

	[SerializeField]
	private CardData cardToGain;

	[SerializeField]
	private List<CardData> cardToGainList;

	[SerializeField]
	private Enums.CardType cardToGainType;

	[SerializeField]
	private Enums.CardPlace cardPlace;

	[SerializeField]
	private bool costZero;

	[SerializeField]
	private int costReduction;

	[SerializeField]
	private bool vanish;

	[SerializeField]
	private bool permanent;

	[SerializeField]
	private bool duplicateActive;

	[Header("Event")]
	[SerializeField]
	private bool passSingleAndCharacterRolls;

	[SerializeField]
	private Enums.DamageType convertReceivedDebuffsIntoDamage;

	[SerializeField]
	private bool convertReceivedDebuffsIntoCurse;

	[Header("Corruption")]
	[SerializeField]
	private bool onlyAddItemToNPCs;

	[Header("Reduce/Increase Card Cost (to increase, castReduceReduction must be negative)")]
	[SerializeField]
	private int cardsReduced;

	[SerializeField]
	private Enums.CardType cardToReduceType;

	[SerializeField]
	private int costReduceReduction;

	[SerializeField]
	private int costReduceEnergyRequirement;

	[SerializeField]
	private bool costReducePermanent;

	[SerializeField]
	private bool reduceHighestCost;

	[Header("Stat modification for wielder (all combat)")]
	[SerializeField]
	private int maxHealth;

	[SerializeField]
	private Enums.CharacterStat characterStatModified;

	[SerializeField]
	private int characterStatModifiedValue;

	[SerializeField]
	private Enums.CharacterStat characterStatModified2;

	[SerializeField]
	private int characterStatModifiedValue2;

	[SerializeField]
	private Enums.CharacterStat characterStatModified3;

	[SerializeField]
	private int characterStatModifiedValue3;

	[Header("Heal bonus for wielder")]
	[SerializeField]
	private int healFlatBonus;

	[SerializeField]
	private float healPercentBonus;

	[SerializeField]
	private int healReceivedFlatBonus;

	[SerializeField]
	private float healReceivedPercentBonus;

	[Header("Damage value bonus for wielder (all combat)")]
	[SerializeField]
	private Enums.DamageType damageFlatBonus;

	[SerializeField]
	private int damageFlatBonusValue;

	[SerializeField]
	private Enums.DamageType damageFlatBonus2;

	[SerializeField]
	private int damageFlatBonusValue2;

	[SerializeField]
	private Enums.DamageType damageFlatBonus3;

	[SerializeField]
	private int damageFlatBonusValue3;

	[SerializeField]
	private Enums.DamageType damagePercentBonus;

	[SerializeField]
	private float damagePercentBonusValue;

	[SerializeField]
	private Enums.DamageType damagePercentBonus2;

	[SerializeField]
	private float damagePercentBonusValue2;

	[SerializeField]
	private Enums.DamageType damagePercentBonus3;

	[SerializeField]
	private float damagePercentBonusValue3;

	[Header("Heal for target")]
	[SerializeField]
	private int healQuantity;

	[SerializeField]
	private SpecialValues healQuantitySpecialValue;

	[SerializeField]
	private int healPercentQuantity;

	[SerializeField]
	private int healPercentQuantitySelf;

	[SerializeField]
	internal float healSelfPerDamageDonePercent;

	[SerializeField]
	internal bool healSelfTeamPerDamageDonePercent;

	[SerializeField]
	internal int healBasedOnAuraCurse;

	[Header("Gain Energy for target")]
	[SerializeField]
	private int energyQuantity;

	[Header("AuraCurse bonus for target")]
	[SerializeField]
	private AuraCurseData auracurseBonus1;

	[SerializeField]
	private int auracurseBonusValue1;

	[SerializeField]
	private AuraCurseData auracurseBonus2;

	[SerializeField]
	private int auracurseBonusValue2;

	public int IncreaseAurasSelf;

	[Header("AuraCurse immunity for wielder")]
	[SerializeField]
	private AuraCurseData auracurseImmune1;

	[SerializeField]
	private AuraCurseData auracurseImmune2;

	[Header("Resist modification for target")]
	[SerializeField]
	private Enums.DamageType resistModified1;

	[SerializeField]
	private int resistModifiedValue1;

	[SerializeField]
	private Enums.DamageType resistModified2;

	[SerializeField]
	private int resistModifiedValue2;

	[SerializeField]
	private Enums.DamageType resistModified3;

	[SerializeField]
	private int resistModifiedValue3;

	[Header("AuraCurse gained on target")]
	[SerializeField]
	private AuraCurseData auracurseGain1;

	[SerializeField]
	private int auracurseGainValue1;

	[SerializeField]
	internal SpecialValues auracurseGain1SpecialValue;

	[SerializeField]
	private bool acg1MultiplyByEnergyUsed;

	[SerializeField]
	private AuraCurseData auracurseGain2;

	[SerializeField]
	private int auracurseGainValue2;

	[SerializeField]
	internal SpecialValues auracurseGain2SpecialValue;

	[SerializeField]
	private bool acg2MultiplyByEnergyUsed;

	[SerializeField]
	private AuraCurseData auracurseGain3;

	[SerializeField]
	private int auracurseGainValue3;

	[SerializeField]
	internal SpecialValues auracurseGain3SpecialValue;

	[SerializeField]
	private bool acg3MultiplyByEnergyUsed;

	[Header("AuraCurse gained on self.")]
	[SerializeField]
	private bool chooseOneACToGain;

	[SerializeField]
	private AuraCurseData auracurseGainSelf1;

	[SerializeField]
	private int auracurseGainSelfValue1;

	[SerializeField]
	private AuraCurseData auracurseGainSelf2;

	[SerializeField]
	private int auracurseGainSelfValue2;

	[SerializeField]
	private AuraCurseData auracurseGainSelf3;

	[SerializeField]
	private int auracurseGainSelfValue3;

	[Header("AuraCurse to dispel/purge")]
	[SerializeField]
	internal AuraCurseData auracurseHeal1;

	[SerializeField]
	internal AuraCurseData auracurseHeal2;

	[SerializeField]
	internal AuraCurseData auracurseHeal3;

	[SerializeField]
	public bool AcHealFromTarget;

	[SerializeField]
	public int StealAuras;

	[Header("Chance to dispel curses from target")]
	[SerializeField]
	private int chanceToDispel;

	[SerializeField]
	private int chanceToDispelNum;

	[Header("Chance to purge auras from target")]
	[SerializeField]
	private int chanceToPurge;

	[SerializeField]
	private int chanceToPurgeNum;

	[Header("Chance to dispel from self")]
	[SerializeField]
	private int chanceToDispelSelf;

	[SerializeField]
	private int chanceToDispelNumSelf;

	[Header("Rewards and discounts")]
	[SerializeField]
	private int percentRetentionEndGame;

	[SerializeField]
	private int percentDiscountShop;

	[Header("ENCHANT DATA")]
	[SerializeField]
	private bool useTheNextInsteadWhenYouPlay;

	[SerializeField]
	private int destroyAfterUses;

	[SerializeField]
	private bool destroyStartOfTurn;

	[SerializeField]
	private bool destroyEndOfTurn;

	[SerializeField]
	[Tooltip("This must be activated for enchantments that need to be activated on self cast")]
	private bool castEnchantmentOnFinishSelfCast;

	[SerializeField]
	private Enums.DamageType modifiedDamageType;

	[SerializeField]
	private Enums.DamageType damageToTargetType;

	[SerializeField]
	private int damageToTarget;

	[SerializeField]
	private bool dttMultiplyByEnergyUsed;

	[SerializeField]
	private SpecialValues dttSpecialValues;

	[SerializeField]
	private Enums.DamageType damageToTargetType2;

	[SerializeField]
	private int damageToTarget2;

	[SerializeField]
	private SpecialValues dttSpecialValues2;

	[SerializeField]
	private bool addVanishToDeck;

	[Header("GRAPHICAL & SOUND EFFECTS")]
	[SerializeField]
	private string effectItemOwner = "";

	[SerializeField]
	private string effectCaster = "";

	[SerializeField]
	private float effectCasterDelay;

	[SerializeField]
	private string effectTarget = "";

	[SerializeField]
	private float effectTargetDelay;

	[SerializeField]
	private AudioClip itemSound;

	[Header("CUSTOM for AuraCurse modifications")]
	[SerializeField]
	private string auracurseCustomString;

	[SerializeField]
	private AuraCurseData auracurseCustomAC;

	[SerializeField]
	private int auracurseCustomModValue1;

	[SerializeField]
	private int auracurseCustomModValue2;

	public bool DropOnly
	{
		get
		{
			return dropOnly;
		}
		set
		{
			dropOnly = value;
		}
	}

	public Enums.EventActivation Activation
	{
		get
		{
			return activation;
		}
		set
		{
			activation = value;
		}
	}

	public Enums.ActivationManual ActivationManual
	{
		get
		{
			return activationManual;
		}
		set
		{
			activationManual = value;
		}
	}

	public int RoundCycle
	{
		get
		{
			return roundCycle;
		}
		set
		{
			roundCycle = value;
		}
	}

	public Enums.ItemTarget ItemTarget
	{
		get
		{
			return itemTarget;
		}
		set
		{
			itemTarget = value;
		}
	}

	public Enums.ItemTarget OverrideTargetText
	{
		get
		{
			return overrideTargetText;
		}
		set
		{
			overrideTargetText = value;
		}
	}

	public Enums.CharacterStat CharacterStatModified
	{
		get
		{
			return characterStatModified;
		}
		set
		{
			characterStatModified = value;
		}
	}

	public int CharacterStatModifiedValue
	{
		get
		{
			return characterStatModifiedValue;
		}
		set
		{
			characterStatModifiedValue = value;
		}
	}

	public Enums.CharacterStat CharacterStatModified2
	{
		get
		{
			return characterStatModified2;
		}
		set
		{
			characterStatModified2 = value;
		}
	}

	public int CharacterStatModifiedValue2
	{
		get
		{
			return characterStatModifiedValue2;
		}
		set
		{
			characterStatModifiedValue2 = value;
		}
	}

	public Enums.CharacterStat CharacterStatModified3
	{
		get
		{
			return characterStatModified3;
		}
		set
		{
			characterStatModified3 = value;
		}
	}

	public int CharacterStatModifiedValue3
	{
		get
		{
			return characterStatModifiedValue3;
		}
		set
		{
			characterStatModifiedValue3 = value;
		}
	}

	public int HealFlatBonus
	{
		get
		{
			return healFlatBonus;
		}
		set
		{
			healFlatBonus = value;
		}
	}

	public float HealPercentBonus
	{
		get
		{
			return healPercentBonus;
		}
		set
		{
			healPercentBonus = value;
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

	public bool HealSelfTeamPerDamageDonePercent
	{
		get
		{
			return healSelfTeamPerDamageDonePercent;
		}
		set
		{
			healSelfTeamPerDamageDonePercent = value;
		}
	}

	public Enums.DamageType DamageFlatBonus
	{
		get
		{
			return damageFlatBonus;
		}
		set
		{
			damageFlatBonus = value;
		}
	}

	public int DamageFlatBonusValue
	{
		get
		{
			return damageFlatBonusValue;
		}
		set
		{
			damageFlatBonusValue = value;
		}
	}

	public Enums.DamageType DamageFlatBonus2
	{
		get
		{
			return damageFlatBonus2;
		}
		set
		{
			damageFlatBonus2 = value;
		}
	}

	public int DamageFlatBonusValue2
	{
		get
		{
			return damageFlatBonusValue2;
		}
		set
		{
			damageFlatBonusValue2 = value;
		}
	}

	public Enums.DamageType DamageFlatBonus3
	{
		get
		{
			return damageFlatBonus3;
		}
		set
		{
			damageFlatBonus3 = value;
		}
	}

	public int DamageFlatBonusValue3
	{
		get
		{
			return damageFlatBonusValue3;
		}
		set
		{
			damageFlatBonusValue3 = value;
		}
	}

	public Enums.DamageType DamagePercentBonus
	{
		get
		{
			return damagePercentBonus;
		}
		set
		{
			damagePercentBonus = value;
		}
	}

	public float DamagePercentBonusValue
	{
		get
		{
			return damagePercentBonusValue;
		}
		set
		{
			damagePercentBonusValue = value;
		}
	}

	public Enums.DamageType DamagePercentBonus2
	{
		get
		{
			return damagePercentBonus2;
		}
		set
		{
			damagePercentBonus2 = value;
		}
	}

	public float DamagePercentBonusValue2
	{
		get
		{
			return damagePercentBonusValue2;
		}
		set
		{
			damagePercentBonusValue2 = value;
		}
	}

	public Enums.DamageType DamagePercentBonus3
	{
		get
		{
			return damagePercentBonus3;
		}
		set
		{
			damagePercentBonus3 = value;
		}
	}

	public float DamagePercentBonusValue3
	{
		get
		{
			return damagePercentBonusValue3;
		}
		set
		{
			damagePercentBonusValue3 = value;
		}
	}

	public AuraCurseData AuracurseBonus1
	{
		get
		{
			return auracurseBonus1;
		}
		set
		{
			auracurseBonus1 = value;
		}
	}

	public int AuracurseBonusValue1
	{
		get
		{
			return auracurseBonusValue1;
		}
		set
		{
			auracurseBonusValue1 = value;
		}
	}

	public AuraCurseData AuracurseBonus2
	{
		get
		{
			return auracurseBonus2;
		}
		set
		{
			auracurseBonus2 = value;
		}
	}

	public int AuracurseBonusValue2
	{
		get
		{
			return auracurseBonusValue2;
		}
		set
		{
			auracurseBonusValue2 = value;
		}
	}

	public AuraCurseData AuracurseImmune1
	{
		get
		{
			return auracurseImmune1;
		}
		set
		{
			auracurseImmune1 = value;
		}
	}

	public AuraCurseData AuracurseImmune2
	{
		get
		{
			return auracurseImmune2;
		}
		set
		{
			auracurseImmune2 = value;
		}
	}

	public Enums.DamageType ResistModified1
	{
		get
		{
			return resistModified1;
		}
		set
		{
			resistModified1 = value;
		}
	}

	public int ResistModifiedValue1
	{
		get
		{
			return resistModifiedValue1;
		}
		set
		{
			resistModifiedValue1 = value;
		}
	}

	public Enums.DamageType ResistModified2
	{
		get
		{
			return resistModified2;
		}
		set
		{
			resistModified2 = value;
		}
	}

	public int ResistModifiedValue2
	{
		get
		{
			return resistModifiedValue2;
		}
		set
		{
			resistModifiedValue2 = value;
		}
	}

	public Enums.DamageType ResistModified3
	{
		get
		{
			return resistModified3;
		}
		set
		{
			resistModified3 = value;
		}
	}

	public int ResistModifiedValue3
	{
		get
		{
			return resistModifiedValue3;
		}
		set
		{
			resistModifiedValue3 = value;
		}
	}

	public AuraCurseData AuracurseGain1
	{
		get
		{
			return auracurseGain1;
		}
		set
		{
			auracurseGain1 = value;
		}
	}

	public int AuracurseGainValue1
	{
		get
		{
			return auracurseGainValue1;
		}
		set
		{
			auracurseGainValue1 = value;
		}
	}

	public AuraCurseData AuracurseGain2
	{
		get
		{
			return auracurseGain2;
		}
		set
		{
			auracurseGain2 = value;
		}
	}

	public int AuracurseGainValue2
	{
		get
		{
			return auracurseGainValue2;
		}
		set
		{
			auracurseGainValue2 = value;
		}
	}

	public int HealQuantity
	{
		get
		{
			return healQuantity;
		}
		set
		{
			healQuantity = value;
		}
	}

	public SpecialValues HealQuantitySpecialValue
	{
		get
		{
			return healQuantitySpecialValue;
		}
		set
		{
			healQuantitySpecialValue = value;
		}
	}

	public AuraCurseData AuraCurseSetted
	{
		get
		{
			return auraCurseSetted;
		}
		set
		{
			auraCurseSetted = value;
		}
	}

	public AuraCurseData AuraCurseSetted2
	{
		get
		{
			return auraCurseSetted2;
		}
		set
		{
			auraCurseSetted2 = value;
		}
	}

	public AuraCurseData AuraCurseSetted3
	{
		get
		{
			return auraCurseSetted3;
		}
		set
		{
			auraCurseSetted3 = value;
		}
	}

	public int AuraCurseNumForOneEvent
	{
		get
		{
			return auraCurseNumForOneEvent;
		}
		set
		{
			auraCurseNumForOneEvent = value;
		}
	}

	public Enums.CardType CastedCardType
	{
		get
		{
			return castedCardType;
		}
		set
		{
			castedCardType = value;
		}
	}

	public int ExactRound
	{
		get
		{
			return exactRound;
		}
		set
		{
			exactRound = value;
		}
	}

	public int EnergyQuantity
	{
		get
		{
			return energyQuantity;
		}
		set
		{
			energyQuantity = value;
		}
	}

	public int TimesPerTurn
	{
		get
		{
			return timesPerTurn;
		}
		set
		{
			timesPerTurn = value;
		}
	}

	public int DrawCards
	{
		get
		{
			return drawCards;
		}
		set
		{
			drawCards = value;
		}
	}

	public int MaxHealth
	{
		get
		{
			return maxHealth;
		}
		set
		{
			maxHealth = value;
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

	public int PercentRetentionEndGame
	{
		get
		{
			return percentRetentionEndGame;
		}
		set
		{
			percentRetentionEndGame = value;
		}
	}

	public int PercentDiscountShop
	{
		get
		{
			return percentDiscountShop;
		}
		set
		{
			percentDiscountShop = value;
		}
	}

	public bool CursedItem
	{
		get
		{
			return cursedItem;
		}
		set
		{
			cursedItem = value;
		}
	}

	public AuraCurseData AuracurseGainSelf1
	{
		get
		{
			return auracurseGainSelf1;
		}
		set
		{
			auracurseGainSelf1 = value;
		}
	}

	public int AuracurseGainSelfValue1
	{
		get
		{
			return auracurseGainSelfValue1;
		}
		set
		{
			auracurseGainSelfValue1 = value;
		}
	}

	public AuraCurseData AuracurseGainSelf2
	{
		get
		{
			return auracurseGainSelf2;
		}
		set
		{
			auracurseGainSelf2 = value;
		}
	}

	public int AuracurseGainSelfValue2
	{
		get
		{
			return auracurseGainSelfValue2;
		}
		set
		{
			auracurseGainSelfValue2 = value;
		}
	}

	public AuraCurseData AuracurseGainSelf3
	{
		get
		{
			return auracurseGainSelf3;
		}
		set
		{
			auracurseGainSelf3 = value;
		}
	}

	public int AuracurseGainSelfValue3
	{
		get
		{
			return auracurseGainSelfValue3;
		}
		set
		{
			auracurseGainSelfValue3 = value;
		}
	}

	public int CardNum
	{
		get
		{
			return cardNum;
		}
		set
		{
			cardNum = value;
		}
	}

	public CardData CardToGain
	{
		get
		{
			return cardToGain;
		}
		set
		{
			cardToGain = value;
		}
	}

	public Enums.CardPlace CardPlace
	{
		get
		{
			return cardPlace;
		}
		set
		{
			cardPlace = value;
		}
	}

	public bool CostZero
	{
		get
		{
			return costZero;
		}
		set
		{
			costZero = value;
		}
	}

	public int CostReduction
	{
		get
		{
			return costReduction;
		}
		set
		{
			costReduction = value;
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

	public bool Permanent
	{
		get
		{
			return permanent;
		}
		set
		{
			permanent = value;
		}
	}

	public Enums.CardType CardToGainType
	{
		get
		{
			return cardToGainType;
		}
		set
		{
			cardToGainType = value;
		}
	}

	public Enums.CardType CardToReduceType
	{
		get
		{
			return cardToReduceType;
		}
		set
		{
			cardToReduceType = value;
		}
	}

	public int CardsReduced
	{
		get
		{
			return cardsReduced;
		}
		set
		{
			cardsReduced = value;
		}
	}

	public int CostReduceReduction
	{
		get
		{
			return costReduceReduction;
		}
		set
		{
			costReduceReduction = value;
		}
	}

	public bool CostReducePermanent
	{
		get
		{
			return costReducePermanent;
		}
		set
		{
			costReducePermanent = value;
		}
	}

	public bool EmptyHand
	{
		get
		{
			return emptyHand;
		}
		set
		{
			emptyHand = value;
		}
	}

	public int ChanceToDispel
	{
		get
		{
			return chanceToDispel;
		}
		set
		{
			chanceToDispel = value;
		}
	}

	public int ChanceToPurge
	{
		get
		{
			return chanceToPurge;
		}
		set
		{
			chanceToPurge = value;
		}
	}

	public int ChanceToDispelSelf
	{
		get
		{
			return chanceToDispelSelf;
		}
		set
		{
			chanceToDispelSelf = value;
		}
	}

	public int ChanceToDispelNum
	{
		get
		{
			return chanceToDispelNum;
		}
		set
		{
			chanceToDispelNum = value;
		}
	}

	public int ChanceToPurgeNum
	{
		get
		{
			return chanceToPurgeNum;
		}
		set
		{
			chanceToPurgeNum = value;
		}
	}

	public int ChanceToDispelNumSelf
	{
		get
		{
			return chanceToDispelNumSelf;
		}
		set
		{
			chanceToDispelNumSelf = value;
		}
	}

	public bool DestroyAfterUse
	{
		get
		{
			return destroyAfterUse;
		}
		set
		{
			destroyAfterUse = value;
		}
	}

	public int HealPercentQuantity
	{
		get
		{
			return healPercentQuantity;
		}
		set
		{
			healPercentQuantity = value;
		}
	}

	public int HealPercentQuantitySelf
	{
		get
		{
			return healPercentQuantitySelf;
		}
		set
		{
			healPercentQuantitySelf = value;
		}
	}

	public bool OnlyAddItemToNPCs
	{
		get
		{
			return onlyAddItemToNPCs;
		}
		set
		{
			onlyAddItemToNPCs = value;
		}
	}

	public Sprite SpriteBossDrop
	{
		get
		{
			return spriteBossDrop;
		}
		set
		{
			spriteBossDrop = value;
		}
	}

	public bool DuplicateActive
	{
		get
		{
			return duplicateActive;
		}
		set
		{
			duplicateActive = value;
		}
	}

	public Enums.DamageType ModifiedDamageType
	{
		get
		{
			return modifiedDamageType;
		}
		set
		{
			modifiedDamageType = value;
		}
	}

	public bool DestroyStartOfTurn
	{
		get
		{
			return destroyStartOfTurn;
		}
		set
		{
			destroyStartOfTurn = value;
		}
	}

	public int DamageToTarget1
	{
		get
		{
			return damageToTarget;
		}
		set
		{
			damageToTarget = value;
		}
	}

	public Enums.DamageType DamageToTargetType1
	{
		get
		{
			return damageToTargetType;
		}
		set
		{
			damageToTargetType = value;
		}
	}

	public bool ReduceHighestCost
	{
		get
		{
			return reduceHighestCost;
		}
		set
		{
			reduceHighestCost = value;
		}
	}

	public bool CastEnchantmentOnFinishSelfCast
	{
		get
		{
			return castEnchantmentOnFinishSelfCast;
		}
		set
		{
			castEnchantmentOnFinishSelfCast = value;
		}
	}

	public bool QuestItem
	{
		get
		{
			return questItem;
		}
		set
		{
			questItem = value;
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

	public string EffectItemOwner
	{
		get
		{
			return effectItemOwner;
		}
		set
		{
			effectItemOwner = value;
		}
	}

	public int DestroyAfterUses
	{
		get
		{
			return destroyAfterUses;
		}
		set
		{
			destroyAfterUses = value;
		}
	}

	public bool NotShowCharacterBonus
	{
		get
		{
			return notShowCharacterBonus;
		}
		set
		{
			notShowCharacterBonus = value;
		}
	}

	public AuraCurseData AuracurseGain3
	{
		get
		{
			return auracurseGain3;
		}
		set
		{
			auracurseGain3 = value;
		}
	}

	public int AuracurseGainValue3
	{
		get
		{
			return auracurseGainValue3;
		}
		set
		{
			auracurseGainValue3 = value;
		}
	}

	public AudioClip ItemSound
	{
		get
		{
			return itemSound;
		}
		set
		{
			itemSound = value;
		}
	}

	public bool Acg1MultiplyByEnergyUsed
	{
		get
		{
			return acg1MultiplyByEnergyUsed;
		}
		set
		{
			acg1MultiplyByEnergyUsed = value;
		}
	}

	public bool Acg2MultiplyByEnergyUsed
	{
		get
		{
			return acg2MultiplyByEnergyUsed;
		}
		set
		{
			acg2MultiplyByEnergyUsed = value;
		}
	}

	public bool Acg3MultiplyByEnergyUsed
	{
		get
		{
			return acg3MultiplyByEnergyUsed;
		}
		set
		{
			acg3MultiplyByEnergyUsed = value;
		}
	}

	public bool DttMultiplyByEnergyUsed
	{
		get
		{
			return dttMultiplyByEnergyUsed;
		}
		set
		{
			dttMultiplyByEnergyUsed = value;
		}
	}

	public bool DrawMultiplyByEnergyUsed
	{
		get
		{
			return drawMultiplyByEnergyUsed;
		}
		set
		{
			drawMultiplyByEnergyUsed = value;
		}
	}

	public bool UsedEnergy
	{
		get
		{
			return usedEnergy;
		}
		set
		{
			usedEnergy = value;
		}
	}

	public float LowerOrEqualPercentHP
	{
		get
		{
			return lowerOrEqualPercentHP;
		}
		set
		{
			lowerOrEqualPercentHP = value;
		}
	}

	public int HealReceivedFlatBonus
	{
		get
		{
			return healReceivedFlatBonus;
		}
		set
		{
			healReceivedFlatBonus = value;
		}
	}

	public float HealReceivedPercentBonus
	{
		get
		{
			return healReceivedPercentBonus;
		}
		set
		{
			healReceivedPercentBonus = value;
		}
	}

	public bool IsEnchantment
	{
		get
		{
			return isEnchantment;
		}
		set
		{
			isEnchantment = value;
		}
	}

	public bool UseTheNextInsteadWhenYouPlay
	{
		get
		{
			return useTheNextInsteadWhenYouPlay;
		}
		set
		{
			useTheNextInsteadWhenYouPlay = value;
		}
	}

	public string AuracurseCustomString
	{
		get
		{
			return auracurseCustomString;
		}
		set
		{
			auracurseCustomString = value;
		}
	}

	public AuraCurseData AuracurseCustomAC
	{
		get
		{
			return auracurseCustomAC;
		}
		set
		{
			auracurseCustomAC = value;
		}
	}

	public int AuracurseCustomModValue1
	{
		get
		{
			return auracurseCustomModValue1;
		}
		set
		{
			auracurseCustomModValue1 = value;
		}
	}

	public int AuracurseCustomModValue2
	{
		get
		{
			return auracurseCustomModValue2;
		}
		set
		{
			auracurseCustomModValue2 = value;
		}
	}

	public bool PassSingleAndCharacterRolls
	{
		get
		{
			return passSingleAndCharacterRolls;
		}
		set
		{
			passSingleAndCharacterRolls = value;
		}
	}

	public Enums.DamageType ConvertReceivedDebuffsIntoDamage
	{
		get
		{
			return convertReceivedDebuffsIntoDamage;
		}
		set
		{
			convertReceivedDebuffsIntoDamage = value;
		}
	}

	public bool ConvertReceivedDebuffsIntoCurse
	{
		get
		{
			return convertReceivedDebuffsIntoCurse;
		}
		set
		{
			convertReceivedDebuffsIntoCurse = value;
		}
	}

	public bool DestroyEndOfTurn
	{
		get
		{
			return destroyEndOfTurn;
		}
		set
		{
			destroyEndOfTurn = value;
		}
	}

	public int TimesPerCombat
	{
		get
		{
			return timesPerCombat;
		}
		set
		{
			timesPerCombat = value;
		}
	}

	public List<CardData> CardToGainList
	{
		get
		{
			return cardToGainList;
		}
		set
		{
			cardToGainList = value;
		}
	}

	public bool ActivationOnlyOnHeroes
	{
		get
		{
			return activationOnlyOnHeroes;
		}
		set
		{
			activationOnlyOnHeroes = value;
		}
	}

	public bool PreventApplyForHeroTarget => preventApplyForHeroTarget;

	public bool ActivateOnReceive
	{
		get
		{
			return activateOnReceive;
		}
		set
		{
			activateOnReceive = value;
		}
	}

	public float EffectCasterDelay
	{
		get
		{
			return effectCasterDelay;
		}
		set
		{
			effectCasterDelay = value;
		}
	}

	public float EffectTargetDelay
	{
		get
		{
			return effectTargetDelay;
		}
		set
		{
			effectTargetDelay = value;
		}
	}

	public int CostReduceEnergyRequirement
	{
		get
		{
			return costReduceEnergyRequirement;
		}
		set
		{
			costReduceEnergyRequirement = value;
		}
	}

	public SpecialValues AuracurseGain1SpecialValue
	{
		get
		{
			return auracurseGain1SpecialValue;
		}
		set
		{
			auracurseGain1SpecialValue = value;
		}
	}

	public SpecialValues AuracurseGain2SpecialValue
	{
		get
		{
			return auracurseGain2SpecialValue;
		}
		set
		{
			auracurseGain2SpecialValue = value;
		}
	}

	public SpecialValues AuracurseGain3SpecialValue
	{
		get
		{
			return auracurseGain3SpecialValue;
		}
		set
		{
			auracurseGain3SpecialValue = value;
		}
	}

	public SpecialValues DttSpecialValues1
	{
		get
		{
			return dttSpecialValues;
		}
		set
		{
			dttSpecialValues = value;
		}
	}

	public int DamageToTarget2
	{
		get
		{
			return damageToTarget2;
		}
		set
		{
			damageToTarget2 = value;
		}
	}

	public Enums.DamageType DamageToTargetType2
	{
		get
		{
			return damageToTargetType2;
		}
		set
		{
			damageToTargetType2 = value;
		}
	}

	public SpecialValues DttSpecialValues2
	{
		get
		{
			return dttSpecialValues2;
		}
		set
		{
			dttSpecialValues2 = value;
		}
	}

	public int HealBasedOnAuraCurse
	{
		get
		{
			return healBasedOnAuraCurse;
		}
		set
		{
			healBasedOnAuraCurse = value;
		}
	}

	public AuraCurseData AuracurseHeal1
	{
		get
		{
			return auracurseHeal1;
		}
		set
		{
			auracurseHeal1 = value;
		}
	}

	public AuraCurseData AuracurseHeal2
	{
		get
		{
			return auracurseHeal2;
		}
		set
		{
			auracurseHeal2 = value;
		}
	}

	public AuraCurseData AuracurseHeal3
	{
		get
		{
			return auracurseHeal3;
		}
		set
		{
			auracurseHeal3 = value;
		}
	}

	public bool ChooseOneACToGain
	{
		get
		{
			return chooseOneACToGain;
		}
		set
		{
			chooseOneACToGain = value;
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

	public bool DontTargetBoss
	{
		get
		{
			return dontTargetBoss;
		}
		set
		{
			dontTargetBoss = value;
		}
	}

	public int GetModifiedValue(int value, SpecialValues specialValues, ItemData itemData = null, Character target = null)
	{
		if (specialValues.Use)
		{
			switch (specialValues.Name)
			{
			case Enums.SpecialValueModifierName.RuneCharges:
				if ((bool)MatchManager.Instance)
				{
					Character characterActive2 = MatchManager.Instance.GetCharacterActive();
					if (characterActive2 != null)
					{
						return (int)((float)(characterActive2.GetAuraCharges("runered") + characterActive2.GetAuraCharges("runeblue") + characterActive2.GetAuraCharges("runegreen")) * specialValues.Multiplier);
					}
				}
				break;
			case Enums.SpecialValueModifierName.DamageDealt:
				if ((bool)MatchManager.Instance)
				{
					MatchManager.Instance.GetCharacterActive();
				}
				break;
			case Enums.SpecialValueModifierName.AuracurseSetCharges:
				if ((bool)MatchManager.Instance && target != null)
				{
					return (int)((float)target.GetAuraCharges(itemData.AuraCurseSetted.Id) * specialValues.Multiplier);
				}
				break;
			case Enums.SpecialValueModifierName.AuraCurseYours:
				if ((bool)MatchManager.Instance)
				{
					Character characterActive = MatchManager.Instance.GetCharacterActive();
					if (characterActive != null)
					{
						return (int)((float)characterActive.GetAuraCharges(itemData.AuraCurseSetted.Id) * specialValues.Multiplier);
					}
				}
				break;
			}
		}
		return value;
	}
}
