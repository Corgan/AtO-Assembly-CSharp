using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New AuraCurse", menuName = "AuraCurse Data", order = 56)]
public class AuraCurseData : ScriptableObject
{
	[Serializable]
	public struct AuraDamageBonus
	{
		public Enums.DamageType AuraDamageType;

		public AuraCurseData AuraDamageBasedOnAC;

		public int AuraDamageIncreasedTotal;

		public float AuraDamageIncreasedPerStack;

		public int AuraDamageIncreasedPercent;

		public float AuraDamageIncreasedPercentPerStack;

		public float AuraDamageIncreasedPercentPerStackPerEnergy;
	}

	[Serializable]
	public struct AuraCurseChargesBonus
	{
		public enum BonusAmountType
		{
			flatBonus,
			percentageBonus,
			bonusPerCharge
		}

		public int requiredChargesForBonus;

		public AuraCurseData acData;

		public BonusAmountType bonusType;

		public int bonusCharges;
	}

	[Serializable]
	public struct ConsumeDamageBasedOnAuraCurse
	{
		public Enums.DamageType DamageTypeWhenConsumed;

		public AuraCurseData ConsumedDamageChargesBasedOnACCharges;

		public int DamageWhenConsumedPerCharge;
	}

	[Header("General Attributes")]
	[SerializeField]
	private string acName;

	[SerializeField]
	private string id;

	[SerializeField]
	private bool isAura;

	[SerializeField]
	private bool skipEndTurnRemovalIfNoBegin;

	[SerializeField]
	private int maxCharges = -1;

	[SerializeField]
	private int maxMadnessCharges = -1;

	[SerializeField]
	private int auraConsumed = 1;

	[TextArea]
	[SerializeField]
	private string description;

	[HideInInspector]
	public int CustomAuxValue;

	[Tooltip("Field to use for plain charges, 1/1. Introduce the value for each charge, f.e. stealth 20%/charge => 20")]
	[SerializeField]
	private int chargesMultiplierDescription = 1;

	[Tooltip("First Field to use for a 1/N format. Introduce the number of charges for one bonus, f.e. 1/3 => 3")]
	[SerializeField]
	private float chargesAuxNeedForOne1 = 1f;

	[Tooltip("Second Field to use for a 1/N format. Introduce the number of charges for one bonus, f.e. 1/3 => 3")]
	[SerializeField]
	private int chargesAuxNeedForOne2 = 1;

	[SerializeField]
	private Sprite sprite;

	[SerializeField]
	private string effectTick;

	[SerializeField]
	private string effectTickSides;

	[Header("Sound")]
	[SerializeField]
	private AudioClip sound;

	[Header("Sound (new)")]
	[SerializeField]
	private AudioClip soundRework;

	[Header("Config")]
	[SerializeField]
	private bool removable = true;

	[SerializeField]
	private bool gainCharges = true;

	[SerializeField]
	private bool iconShow = true;

	[SerializeField]
	private bool combatlogShow = true;

	[SerializeField]
	private bool preventable = true;

	[SerializeField]
	private bool canBeAddedToImmunityDespiteNotBeingPreventable;

	[Header("Expiration")]
	[SerializeField]
	private int priorityOnConsumption;

	[SerializeField]
	private bool consumeAll;

	[SerializeField]
	private bool consumedAtCast;

	[SerializeField]
	private bool consumedAtTurnBegin;

	[SerializeField]
	private bool consumedAtTurn;

	[SerializeField]
	private bool consumedAtRoundBegin;

	[SerializeField]
	private bool consumedAtRound;

	[SerializeField]
	private bool produceDamageWhenConsumed;

	[SerializeField]
	private bool produceHealWhenConsumed;

	[SerializeField]
	private bool dieWhenConsumedAll;

	[Header("Aura Bonusses")]
	[SerializeField]
	private List<AuraCurseChargesBonus> acBonusData;

	[Header("Aura Damage Bonus")]
	[SerializeField]
	private Enums.DamageType auraDamageType;

	[SerializeField]
	private AuraCurseData auraDamageChargesBasedOnACCharges;

	[SerializeField]
	private int auraDamageIncreasedTotal;

	[SerializeField]
	private float auraDamageIncreasedPerStack;

	[SerializeField]
	private int auraDamageIncreasedPercent;

	[SerializeField]
	private float auraDamageIncreasedPercentPerStack;

	[SerializeField]
	private float auraDamageIncreasedPercentPerStackPerEnergy;

	[SerializeField]
	private Enums.DamageType auraDamageType2;

	[SerializeField]
	private int auraDamageIncreasedTotal2;

	[SerializeField]
	private float auraDamageIncreasedPerStack2;

	[SerializeField]
	private int auraDamageIncreasedPercent2;

	[SerializeField]
	private float auraDamageIncreasedPercentPerStack2;

	[SerializeField]
	private float auraDamageIncreasedPercentPerStackPerEnergy2;

	[SerializeField]
	private Enums.DamageType auraDamageType3;

	[SerializeField]
	private int auraDamageIncreasedTotal3;

	[SerializeField]
	private float auraDamageIncreasedPerStack3;

	[SerializeField]
	private int auraDamageIncreasedPercent3;

	[SerializeField]
	private float auraDamageIncreasedPercentPerStack3;

	[SerializeField]
	private float auraDamageIncreasedPercentPerStackPerEnergy3;

	[SerializeField]
	private Enums.DamageType auraDamageType4;

	[SerializeField]
	private int auraDamageIncreasedTotal4;

	[SerializeField]
	private float auraDamageIncreasedPerStack4;

	[SerializeField]
	private int auraDamageIncreasedPercent4;

	[SerializeField]
	private float auraDamageIncreasedPercentPerStack4;

	[SerializeField]
	private float auraDamageIncreasedPercentPerStackPerEnergy4;

	[SerializeField]
	private AuraDamageBonus[] auraDamageConditionalBonuses;

	[Header("Aura Heal Bonus")]
	[SerializeField]
	private int healDoneTotal;

	[SerializeField]
	private int healDonePerStack;

	[SerializeField]
	private int healDonePercent;

	[SerializeField]
	private int healDonePercentPerStack;

	[SerializeField]
	private int healDonePercentPerStackPerEnergy;

	[SerializeField]
	private int healReceivedTotal;

	[SerializeField]
	private int healReceivedPerStack;

	[SerializeField]
	private int healReceivedPercent;

	[SerializeField]
	private int healReceivedPercentPerStack;

	[Header("Aura Draw Bonus")]
	[SerializeField]
	private int cardsDrawPerStack;

	[Header("Aura Damage Reflected")]
	[SerializeField]
	private int chargesPreReqForDamageReflection;

	[SerializeField]
	private Enums.RefectedDamageModifierType damageReflectedModifierType;

	[SerializeField]
	[FormerlySerializedAs("damageReflectedPerStack")]
	private int damageReflectedMultiplier;

	[SerializeField]
	private Enums.DamageType damageReflectedType;

	[SerializeField]
	private int damageReflectedConsumeCharges;

	[Header("Block")]
	[SerializeField]
	private int blockChargesGainedPerStack;

	[SerializeField]
	private bool noRemoveBlockAtTurnEnd;

	[SerializeField]
	private bool grantBlockToTeamForAmountOfDamageBlocked;

	[SerializeField]
	private int chargesPreReqForGrantBlockToTeamForAmountOfDamageBlocked;

	[Header("Prevention")]
	[SerializeField]
	private int damagePreventedPerStack;

	[SerializeField]
	private int cursePreventedPerStack;

	[SerializeField]
	private AuraCurseData preventedAuraCurse;

	[SerializeField]
	private int preventedAuraCurseStackPerStack;

	[Header("Damage received")]
	[SerializeField]
	private Enums.DamageType increasedDamageReceivedType;

	[SerializeField]
	private int increasedDirectDamageChargesMultiplierNeededForOne = 1;

	[SerializeField]
	private int increasedDirectDamageReceivedPerTurn;

	[SerializeField]
	private float increasedDirectDamageReceivedPerStack;

	[SerializeField]
	private int increasedPercentDamageReceivedPerTurn;

	[SerializeField]
	private int increasedPercentDamageReceivedPerStack;

	[SerializeField]
	private Enums.DamageType increasedDamageReceivedType2;

	[SerializeField]
	private int increasedDirectDamageChargesMultiplierNeededForOne2 = 1;

	[SerializeField]
	private int increasedDirectDamageReceivedPerTurn2;

	[SerializeField]
	private float increasedDirectDamageReceivedPerStack2;

	[SerializeField]
	private int increasedPercentDamageReceivedPerTurn2;

	[SerializeField]
	private int increasedPercentDamageReceivedPerStack2;

	[Header("Damage prevented")]
	[SerializeField]
	private Enums.DamageType preventedDamageTypePerStack;

	[SerializeField]
	private int preventedDamagePerStack;

	[Header("Heal attacker")]
	[SerializeField]
	private int healAttackerPerStack;

	[SerializeField]
	private int healAttackerConsumeCharges;

	[Header("Character stat modification")]
	[SerializeField]
	private Enums.CharacterStat characterStatModified;

	[SerializeField]
	private int characterStatChargesMultiplierNeededForOne = 1;

	[SerializeField]
	private int characterStatModifiedValue;

	[SerializeField]
	private float characterStatModifiedValuePerStack;

	[SerializeField]
	private bool characterStatAbsolute;

	[SerializeField]
	private int characterStatAbsoluteValue;

	[SerializeField]
	private int characterStatAbsoluteValuePerStack;

	[Header("Resist modification")]
	[SerializeField]
	private Enums.DamageType resistModified;

	[SerializeField]
	private float resistModifiedValue;

	[SerializeField]
	private float resistModifiedPercentagePerStack;

	[SerializeField]
	private Enums.DamageType resistModified2;

	[SerializeField]
	private float resistModifiedValue2;

	[SerializeField]
	private float resistModifiedPercentagePerStack2;

	[SerializeField]
	private Enums.DamageType resistModified3;

	[SerializeField]
	private float resistModifiedValue3;

	[SerializeField]
	private float resistModifiedPercentagePerStack3;

	[Header("Explode at stacks")]
	[SerializeField]
	private int explodeAtStacks;

	[Header("Explode Effects")]
	[SerializeField]
	private int healTotalOnExplode;

	[SerializeField]
	private float healPerChargeOnExplode;

	[SerializeField]
	private Enums.AuraCurseExplodeHealTarget healTargetOnExplode;

	[SerializeField]
	private AuraCurseData acOnExplode;

	[SerializeField]
	private int acTotalChargesOnExplode;

	[SerializeField]
	private int acChargesPerStackChargeOnExplodeOnExplode;

	[Header("Consume damage")]
	[SerializeField]
	private Enums.DamageType damageTypeWhenConsumed;

	[SerializeField]
	private AuraCurseData consumedDamageChargesBasedOnACCharges;

	[SerializeField]
	private AuraCurseData consumeDamageChargesIfACApplied;

	[SerializeField]
	private int damageWhenConsumed;

	[SerializeField]
	private float damageWhenConsumedPerCharge;

	[SerializeField]
	private int damageSidesWhenConsumed;

	[SerializeField]
	private int damageSidesWhenConsumedPerCharge;

	[SerializeField]
	private int doubleDamageIfCursesLessThan;

	[Header("Consume heal")]
	[SerializeField]
	private int healWhenConsumed;

	[SerializeField]
	private float healWhenConsumedPerCharge;

	[SerializeField]
	private int healSidesWhenConsumed;

	[SerializeField]
	private float healSidesWhenConsumedPerCharge;

	[Header("Remove Aura Curse")]
	[SerializeField]
	private AuraCurseData removeAuraCurse;

	[SerializeField]
	private AuraCurseData removeAuraCurse2;

	[Header("Aura Curse Gained on Consumption")]
	[SerializeField]
	private AuraCurseData gainAuraCurseConsumption;

	[SerializeField]
	private int gainAuraCurseConsumptionPerCharge;

	[SerializeField]
	private AuraCurseData gainChargesFromThisAuraCurse;

	[SerializeField]
	private AuraCurseData gainAuraCurseConsumption2;

	[SerializeField]
	private int gainAuraCurseConsumptionPerCharge2;

	[SerializeField]
	private AuraCurseData gainChargesFromThisAuraCurse2;

	[Header("Reveal Cards")]
	[SerializeField]
	private int revealCardsPerCharge;

	[Header("Cost Cards")]
	[SerializeField]
	private int modifyCardCostPerChargeNeededForOne;

	[Header("Disabled Cards")]
	[SerializeField]
	private Enums.CardType[] disabledCardTypes;

	[Header("Misc")]
	[SerializeField]
	private bool invulnerable;

	[SerializeField]
	private bool stealth;

	[SerializeField]
	private bool taunt;

	[SerializeField]
	private bool skipsNextTurn;

	public string ACName
	{
		get
		{
			return acName;
		}
		set
		{
			acName = value;
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

	public bool IsAura
	{
		get
		{
			return isAura;
		}
		set
		{
			isAura = value;
		}
	}

	public int MaxCharges
	{
		get
		{
			return maxCharges;
		}
		set
		{
			maxCharges = value;
		}
	}

	public int AuraConsumed
	{
		get
		{
			return auraConsumed;
		}
		set
		{
			auraConsumed = value;
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

	public int ChargesMultiplierDescription
	{
		get
		{
			return chargesMultiplierDescription;
		}
		set
		{
			chargesMultiplierDescription = value;
		}
	}

	public float ChargesAuxNeedForOne1
	{
		get
		{
			return chargesAuxNeedForOne1;
		}
		set
		{
			chargesAuxNeedForOne1 = value;
		}
	}

	public int ChargesAuxNeedForOne2
	{
		get
		{
			return chargesAuxNeedForOne2;
		}
		set
		{
			chargesAuxNeedForOne2 = value;
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

	public string EffectTick
	{
		get
		{
			return effectTick;
		}
		set
		{
			effectTick = value;
		}
	}

	public string EffectTickSides
	{
		get
		{
			return effectTickSides;
		}
		set
		{
			effectTickSides = value;
		}
	}

	public bool Removable
	{
		get
		{
			return removable;
		}
		set
		{
			removable = value;
		}
	}

	public bool GainCharges
	{
		get
		{
			return gainCharges;
		}
		set
		{
			gainCharges = value;
		}
	}

	public bool IconShow
	{
		get
		{
			return iconShow;
		}
		set
		{
			iconShow = value;
		}
	}

	public bool CombatlogShow
	{
		get
		{
			return combatlogShow;
		}
		set
		{
			combatlogShow = value;
		}
	}

	public int PriorityOnConsumption
	{
		get
		{
			return priorityOnConsumption;
		}
		set
		{
			priorityOnConsumption = value;
		}
	}

	public bool ConsumeAll
	{
		get
		{
			return consumeAll;
		}
		set
		{
			consumeAll = value;
		}
	}

	public bool ConsumedAtCast
	{
		get
		{
			return consumedAtCast;
		}
		set
		{
			consumedAtCast = value;
		}
	}

	public bool ConsumedAtTurnBegin
	{
		get
		{
			return consumedAtTurnBegin;
		}
		set
		{
			consumedAtTurnBegin = value;
		}
	}

	public bool ConsumedAtTurn
	{
		get
		{
			return consumedAtTurn;
		}
		set
		{
			consumedAtTurn = value;
		}
	}

	public bool ConsumedAtRoundBegin
	{
		get
		{
			return consumedAtRoundBegin;
		}
		set
		{
			consumedAtRoundBegin = value;
		}
	}

	public bool ConsumedAtRound
	{
		get
		{
			return consumedAtRound;
		}
		set
		{
			consumedAtRound = value;
		}
	}

	public bool ProduceDamageWhenConsumed
	{
		get
		{
			return produceDamageWhenConsumed;
		}
		set
		{
			produceDamageWhenConsumed = value;
		}
	}

	public bool ProduceHealWhenConsumed
	{
		get
		{
			return produceHealWhenConsumed;
		}
		set
		{
			produceHealWhenConsumed = value;
		}
	}

	public bool DieWhenConsumedAll
	{
		get
		{
			return dieWhenConsumedAll;
		}
		set
		{
			dieWhenConsumedAll = value;
		}
	}

	public Enums.DamageType AuraDamageType
	{
		get
		{
			return auraDamageType;
		}
		set
		{
			auraDamageType = value;
		}
	}

	public int AuraDamageIncreasedTotal
	{
		get
		{
			return auraDamageIncreasedTotal;
		}
		set
		{
			auraDamageIncreasedTotal = value;
		}
	}

	public float AuraDamageIncreasedPerStack
	{
		get
		{
			return auraDamageIncreasedPerStack;
		}
		set
		{
			auraDamageIncreasedPerStack = value;
		}
	}

	public int AuraDamageIncreasedPercent
	{
		get
		{
			return auraDamageIncreasedPercent;
		}
		set
		{
			auraDamageIncreasedPercent = value;
		}
	}

	public float AuraDamageIncreasedPercentPerStack
	{
		get
		{
			return auraDamageIncreasedPercentPerStack;
		}
		set
		{
			auraDamageIncreasedPercentPerStack = value;
		}
	}

	public Enums.DamageType AuraDamageType2
	{
		get
		{
			return auraDamageType2;
		}
		set
		{
			auraDamageType2 = value;
		}
	}

	public int AuraDamageIncreasedTotal2
	{
		get
		{
			return auraDamageIncreasedTotal2;
		}
		set
		{
			auraDamageIncreasedTotal2 = value;
		}
	}

	public float AuraDamageIncreasedPerStack2
	{
		get
		{
			return auraDamageIncreasedPerStack2;
		}
		set
		{
			auraDamageIncreasedPerStack2 = value;
		}
	}

	public int AuraDamageIncreasedPercent2
	{
		get
		{
			return auraDamageIncreasedPercent2;
		}
		set
		{
			auraDamageIncreasedPercent2 = value;
		}
	}

	public float AuraDamageIncreasedPercentPerStack2
	{
		get
		{
			return auraDamageIncreasedPercentPerStack2;
		}
		set
		{
			auraDamageIncreasedPercentPerStack2 = value;
		}
	}

	public Enums.DamageType AuraDamageType3
	{
		get
		{
			return auraDamageType3;
		}
		set
		{
			auraDamageType3 = value;
		}
	}

	public int AuraDamageIncreasedTotal3
	{
		get
		{
			return auraDamageIncreasedTotal3;
		}
		set
		{
			auraDamageIncreasedTotal3 = value;
		}
	}

	public float AuraDamageIncreasedPerStack3
	{
		get
		{
			return auraDamageIncreasedPerStack3;
		}
		set
		{
			auraDamageIncreasedPerStack3 = value;
		}
	}

	public int AuraDamageIncreasedPercent3
	{
		get
		{
			return auraDamageIncreasedPercent3;
		}
		set
		{
			auraDamageIncreasedPercent3 = value;
		}
	}

	public float AuraDamageIncreasedPercentPerStack3
	{
		get
		{
			return auraDamageIncreasedPercentPerStack3;
		}
		set
		{
			auraDamageIncreasedPercentPerStack3 = value;
		}
	}

	public Enums.DamageType AuraDamageType4
	{
		get
		{
			return auraDamageType4;
		}
		set
		{
			auraDamageType4 = value;
		}
	}

	public int AuraDamageIncreasedTotal4
	{
		get
		{
			return auraDamageIncreasedTotal4;
		}
		set
		{
			auraDamageIncreasedTotal4 = value;
		}
	}

	public float AuraDamageIncreasedPerStack4
	{
		get
		{
			return auraDamageIncreasedPerStack4;
		}
		set
		{
			auraDamageIncreasedPerStack4 = value;
		}
	}

	public int AuraDamageIncreasedPercent4
	{
		get
		{
			return auraDamageIncreasedPercent4;
		}
		set
		{
			auraDamageIncreasedPercent4 = value;
		}
	}

	public float AuraDamageIncreasedPercentPerStack4
	{
		get
		{
			return auraDamageIncreasedPercentPerStack4;
		}
		set
		{
			auraDamageIncreasedPercentPerStack4 = value;
		}
	}

	public AuraDamageBonus[] AuraDamageConditionalBonuses
	{
		get
		{
			return auraDamageConditionalBonuses;
		}
		set
		{
			auraDamageConditionalBonuses = value;
		}
	}

	public int HealDoneTotal
	{
		get
		{
			return healDoneTotal;
		}
		set
		{
			healDoneTotal = value;
		}
	}

	public int HealDonePerStack
	{
		get
		{
			return healDonePerStack;
		}
		set
		{
			healDonePerStack = value;
		}
	}

	public int HealDonePercent
	{
		get
		{
			return healDonePercent;
		}
		set
		{
			healDonePercent = value;
		}
	}

	public int HealDonePercentPerStack
	{
		get
		{
			return healDonePercentPerStack;
		}
		set
		{
			healDonePercentPerStack = value;
		}
	}

	public int HealReceivedTotal
	{
		get
		{
			return healReceivedTotal;
		}
		set
		{
			healReceivedTotal = value;
		}
	}

	public int HealReceivedPerStack
	{
		get
		{
			return healReceivedPerStack;
		}
		set
		{
			healReceivedPerStack = value;
		}
	}

	public int HealReceivedPercent
	{
		get
		{
			return healReceivedPercent;
		}
		set
		{
			healReceivedPercent = value;
		}
	}

	public int HealReceivedPercentPerStack
	{
		get
		{
			return healReceivedPercentPerStack;
		}
		set
		{
			healReceivedPercentPerStack = value;
		}
	}

	public int CardsDrawPerStack
	{
		get
		{
			return cardsDrawPerStack;
		}
		set
		{
			cardsDrawPerStack = value;
		}
	}

	public int DamageReflectedMultiplier
	{
		get
		{
			return damageReflectedMultiplier;
		}
		set
		{
			damageReflectedMultiplier = value;
		}
	}

	public int ChargesPreReqForDamageReflection
	{
		get
		{
			return chargesPreReqForDamageReflection;
		}
		set
		{
			chargesPreReqForDamageReflection = value;
		}
	}

	public Enums.RefectedDamageModifierType DamageReflectedModifierType
	{
		get
		{
			return damageReflectedModifierType;
		}
		set
		{
			damageReflectedModifierType = value;
		}
	}

	public Enums.DamageType DamageReflectedType
	{
		get
		{
			return damageReflectedType;
		}
		set
		{
			damageReflectedType = value;
		}
	}

	public int BlockChargesGainedPerStack
	{
		get
		{
			return blockChargesGainedPerStack;
		}
		set
		{
			blockChargesGainedPerStack = value;
		}
	}

	public bool NoRemoveBlockAtTurnEnd
	{
		get
		{
			return noRemoveBlockAtTurnEnd;
		}
		set
		{
			noRemoveBlockAtTurnEnd = value;
		}
	}

	public int DamagePreventedPerStack
	{
		get
		{
			return damagePreventedPerStack;
		}
		set
		{
			damagePreventedPerStack = value;
		}
	}

	public int CursePreventedPerStack
	{
		get
		{
			return cursePreventedPerStack;
		}
		set
		{
			cursePreventedPerStack = value;
		}
	}

	public Enums.DamageType IncreasedDamageReceivedType
	{
		get
		{
			return increasedDamageReceivedType;
		}
		set
		{
			increasedDamageReceivedType = value;
		}
	}

	public int IncreasedDirectDamageChargesMultiplierNeededForOne
	{
		get
		{
			return increasedDirectDamageChargesMultiplierNeededForOne;
		}
		set
		{
			increasedDirectDamageChargesMultiplierNeededForOne = value;
		}
	}

	public int IncreasedDirectDamageReceivedPerTurn
	{
		get
		{
			return increasedDirectDamageReceivedPerTurn;
		}
		set
		{
			increasedDirectDamageReceivedPerTurn = value;
		}
	}

	public float IncreasedDirectDamageReceivedPerStack
	{
		get
		{
			return increasedDirectDamageReceivedPerStack;
		}
		set
		{
			increasedDirectDamageReceivedPerStack = value;
		}
	}

	public int IncreasedPercentDamageReceivedPerTurn
	{
		get
		{
			return increasedPercentDamageReceivedPerTurn;
		}
		set
		{
			increasedPercentDamageReceivedPerTurn = value;
		}
	}

	public int IncreasedPercentDamageReceivedPerStack
	{
		get
		{
			return increasedPercentDamageReceivedPerStack;
		}
		set
		{
			increasedPercentDamageReceivedPerStack = value;
		}
	}

	public Enums.DamageType PreventedDamageTypePerStack
	{
		get
		{
			return preventedDamageTypePerStack;
		}
		set
		{
			preventedDamageTypePerStack = value;
		}
	}

	public int PreventedDamagePerStack
	{
		get
		{
			return preventedDamagePerStack;
		}
		set
		{
			preventedDamagePerStack = value;
		}
	}

	public AuraCurseData PreventedAuraCurse
	{
		get
		{
			return preventedAuraCurse;
		}
		set
		{
			preventedAuraCurse = value;
		}
	}

	public int PreventedAuraCurseStackPerStack
	{
		get
		{
			return preventedAuraCurseStackPerStack;
		}
		set
		{
			preventedAuraCurseStackPerStack = value;
		}
	}

	public int HealAttackerPerStack
	{
		get
		{
			return healAttackerPerStack;
		}
		set
		{
			healAttackerPerStack = value;
		}
	}

	public int HealAttackerConsumeCharges
	{
		get
		{
			return healAttackerConsumeCharges;
		}
		set
		{
			healAttackerConsumeCharges = value;
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

	public int CharacterStatChargesMultiplierNeededForOne
	{
		get
		{
			return characterStatChargesMultiplierNeededForOne;
		}
		set
		{
			characterStatChargesMultiplierNeededForOne = value;
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

	public float CharacterStatModifiedValuePerStack
	{
		get
		{
			return characterStatModifiedValuePerStack;
		}
		set
		{
			characterStatModifiedValuePerStack = value;
		}
	}

	public bool CharacterStatAbsolute
	{
		get
		{
			return characterStatAbsolute;
		}
		set
		{
			characterStatAbsolute = value;
		}
	}

	public int CharacterStatAbsoluteValue
	{
		get
		{
			return characterStatAbsoluteValue;
		}
		set
		{
			characterStatAbsoluteValue = value;
		}
	}

	public int CharacterStatAbsoluteValuePerStack
	{
		get
		{
			return characterStatAbsoluteValuePerStack;
		}
		set
		{
			characterStatAbsoluteValuePerStack = value;
		}
	}

	public Enums.DamageType ResistModified
	{
		get
		{
			return resistModified;
		}
		set
		{
			resistModified = value;
		}
	}

	public float ResistModifiedValue
	{
		get
		{
			return resistModifiedValue;
		}
		set
		{
			resistModifiedValue = value;
		}
	}

	public float ResistModifiedPercentagePerStack
	{
		get
		{
			return resistModifiedPercentagePerStack;
		}
		set
		{
			resistModifiedPercentagePerStack = value;
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

	public float ResistModifiedValue2
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

	public float ResistModifiedPercentagePerStack2
	{
		get
		{
			return resistModifiedPercentagePerStack2;
		}
		set
		{
			resistModifiedPercentagePerStack2 = value;
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

	public float ResistModifiedValue3
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

	public float ResistModifiedPercentagePerStack3
	{
		get
		{
			return resistModifiedPercentagePerStack3;
		}
		set
		{
			resistModifiedPercentagePerStack3 = value;
		}
	}

	public int ExplodeAtStacks
	{
		get
		{
			return explodeAtStacks;
		}
		set
		{
			explodeAtStacks = value;
		}
	}

	public Enums.DamageType DamageTypeWhenConsumed
	{
		get
		{
			return damageTypeWhenConsumed;
		}
		set
		{
			damageTypeWhenConsumed = value;
		}
	}

	public int DamageWhenConsumed
	{
		get
		{
			return damageWhenConsumed;
		}
		set
		{
			damageWhenConsumed = value;
		}
	}

	public float DamageWhenConsumedPerCharge
	{
		get
		{
			return damageWhenConsumedPerCharge;
		}
		set
		{
			damageWhenConsumedPerCharge = value;
		}
	}

	public int DamageSidesWhenConsumed
	{
		get
		{
			return damageSidesWhenConsumed;
		}
		set
		{
			damageSidesWhenConsumed = value;
		}
	}

	public int DamageSidesWhenConsumedPerCharge
	{
		get
		{
			return damageSidesWhenConsumedPerCharge;
		}
		set
		{
			damageSidesWhenConsumedPerCharge = value;
		}
	}

	public int HealWhenConsumed
	{
		get
		{
			return healWhenConsumed;
		}
		set
		{
			healWhenConsumed = value;
		}
	}

	public float HealWhenConsumedPerCharge
	{
		get
		{
			return healWhenConsumedPerCharge;
		}
		set
		{
			healWhenConsumedPerCharge = value;
		}
	}

	public int HealSidesWhenConsumed
	{
		get
		{
			return healSidesWhenConsumed;
		}
		set
		{
			healSidesWhenConsumed = value;
		}
	}

	public AuraCurseData RemoveAuraCurse
	{
		get
		{
			return removeAuraCurse;
		}
		set
		{
			removeAuraCurse = value;
		}
	}

	public AuraCurseData RemoveAuraCurse2
	{
		get
		{
			return removeAuraCurse2;
		}
		set
		{
			removeAuraCurse2 = value;
		}
	}

	public AuraCurseData GainAuraCurseConsumption
	{
		get
		{
			return gainAuraCurseConsumption;
		}
		set
		{
			gainAuraCurseConsumption = value;
		}
	}

	public int GainAuraCurseConsumptionPerCharge
	{
		get
		{
			return gainAuraCurseConsumptionPerCharge;
		}
		set
		{
			gainAuraCurseConsumptionPerCharge = value;
		}
	}

	public AuraCurseData GainChargesFromThisAuraCurse
	{
		get
		{
			return gainChargesFromThisAuraCurse;
		}
		set
		{
			gainChargesFromThisAuraCurse = value;
		}
	}

	public AuraCurseData GainAuraCurseConsumption2
	{
		get
		{
			return gainAuraCurseConsumption2;
		}
		set
		{
			gainAuraCurseConsumption2 = value;
		}
	}

	public int GainAuraCurseConsumptionPerCharge2
	{
		get
		{
			return gainAuraCurseConsumptionPerCharge2;
		}
		set
		{
			gainAuraCurseConsumptionPerCharge2 = value;
		}
	}

	public AuraCurseData GainChargesFromThisAuraCurse2
	{
		get
		{
			return gainChargesFromThisAuraCurse2;
		}
		set
		{
			gainChargesFromThisAuraCurse2 = value;
		}
	}

	public int RevealCardsPerCharge
	{
		get
		{
			return revealCardsPerCharge;
		}
		set
		{
			revealCardsPerCharge = value;
		}
	}

	public int ModifyCardCostPerChargeNeededForOne
	{
		get
		{
			return modifyCardCostPerChargeNeededForOne;
		}
		set
		{
			modifyCardCostPerChargeNeededForOne = value;
		}
	}

	public Enums.CardType[] DisabledCardTypes
	{
		get
		{
			return disabledCardTypes;
		}
		set
		{
			disabledCardTypes = value;
		}
	}

	public bool Invulnerable
	{
		get
		{
			return invulnerable;
		}
		set
		{
			invulnerable = value;
		}
	}

	public bool Stealth
	{
		get
		{
			return stealth;
		}
		set
		{
			stealth = value;
		}
	}

	public bool Taunt
	{
		get
		{
			return taunt;
		}
		set
		{
			taunt = value;
		}
	}

	public bool SkipsNextTurn
	{
		get
		{
			return skipsNextTurn;
		}
		set
		{
			skipsNextTurn = value;
		}
	}

	public int DamageReflectedConsumeCharges
	{
		get
		{
			return damageReflectedConsumeCharges;
		}
		set
		{
			damageReflectedConsumeCharges = value;
		}
	}

	public int MaxMadnessCharges
	{
		get
		{
			return maxMadnessCharges;
		}
		set
		{
			maxMadnessCharges = value;
		}
	}

	public bool Preventable
	{
		get
		{
			return preventable;
		}
		set
		{
			preventable = value;
		}
	}

	public bool CanBeAddedToImmunityDespiteNotBeingPreventable
	{
		get
		{
			return canBeAddedToImmunityDespiteNotBeingPreventable;
		}
		set
		{
			canBeAddedToImmunityDespiteNotBeingPreventable = value;
		}
	}

	public float AuraDamageIncreasedPercentPerStackPerEnergy
	{
		get
		{
			return auraDamageIncreasedPercentPerStackPerEnergy;
		}
		set
		{
			auraDamageIncreasedPercentPerStackPerEnergy = value;
		}
	}

	public float AuraDamageIncreasedPercentPerStackPerEnergy2
	{
		get
		{
			return auraDamageIncreasedPercentPerStackPerEnergy2;
		}
		set
		{
			auraDamageIncreasedPercentPerStackPerEnergy2 = value;
		}
	}

	public int HealDonePercentPerStackPerEnergy
	{
		get
		{
			return healDonePercentPerStackPerEnergy;
		}
		set
		{
			healDonePercentPerStackPerEnergy = value;
		}
	}

	public float AuraDamageIncreasedPercentPerStackPerEnergy3
	{
		get
		{
			return auraDamageIncreasedPercentPerStackPerEnergy3;
		}
		set
		{
			auraDamageIncreasedPercentPerStackPerEnergy3 = value;
		}
	}

	public float HealSidesWhenConsumedPerCharge
	{
		get
		{
			return healSidesWhenConsumedPerCharge;
		}
		set
		{
			healSidesWhenConsumedPerCharge = value;
		}
	}

	public Enums.DamageType IncreasedDamageReceivedType2
	{
		get
		{
			return increasedDamageReceivedType2;
		}
		set
		{
			increasedDamageReceivedType2 = value;
		}
	}

	public int IncreasedDirectDamageChargesMultiplierNeededForOne2
	{
		get
		{
			return increasedDirectDamageChargesMultiplierNeededForOne2;
		}
		set
		{
			increasedDirectDamageChargesMultiplierNeededForOne2 = value;
		}
	}

	public int IncreasedDirectDamageReceivedPerTurn2
	{
		get
		{
			return increasedDirectDamageReceivedPerTurn2;
		}
		set
		{
			increasedDirectDamageReceivedPerTurn2 = value;
		}
	}

	public float IncreasedDirectDamageReceivedPerStack2
	{
		get
		{
			return increasedDirectDamageReceivedPerStack2;
		}
		set
		{
			increasedDirectDamageReceivedPerStack2 = value;
		}
	}

	public int IncreasedPercentDamageReceivedPerTurn2
	{
		get
		{
			return increasedPercentDamageReceivedPerTurn2;
		}
		set
		{
			increasedPercentDamageReceivedPerTurn2 = value;
		}
	}

	public int IncreasedPercentDamageReceivedPerStack2
	{
		get
		{
			return increasedPercentDamageReceivedPerStack2;
		}
		set
		{
			increasedPercentDamageReceivedPerStack2 = value;
		}
	}

	public float AuraDamageIncreasedPercentPerStackPerEnergy4
	{
		get
		{
			return auraDamageIncreasedPercentPerStackPerEnergy4;
		}
		set
		{
			auraDamageIncreasedPercentPerStackPerEnergy4 = value;
		}
	}

	public int DoubleDamageIfCursesLessThan
	{
		get
		{
			return doubleDamageIfCursesLessThan;
		}
		set
		{
			doubleDamageIfCursesLessThan = value;
		}
	}

	public AuraCurseData AuraDamageChargesBasedOnACCharges
	{
		get
		{
			return auraDamageChargesBasedOnACCharges;
		}
		set
		{
			auraDamageChargesBasedOnACCharges = value;
		}
	}

	public AuraCurseData ConsumedDamageChargesBasedOnACCharges
	{
		get
		{
			return consumedDamageChargesBasedOnACCharges;
		}
		set
		{
			consumedDamageChargesBasedOnACCharges = value;
		}
	}

	public AuraCurseData ConsumeDamageChargesIfACApplied
	{
		get
		{
			return consumeDamageChargesIfACApplied;
		}
		set
		{
			consumeDamageChargesIfACApplied = value;
		}
	}

	public AudioClip SoundRework
	{
		get
		{
			return soundRework;
		}
		set
		{
			soundRework = value;
		}
	}

	public List<AuraCurseChargesBonus> ACBonusData
	{
		get
		{
			return acBonusData;
		}
		set
		{
			acBonusData = value;
		}
	}

	public bool GrantBlockToTeamForAmountOfDamageBlocked
	{
		get
		{
			return grantBlockToTeamForAmountOfDamageBlocked;
		}
		set
		{
			grantBlockToTeamForAmountOfDamageBlocked = value;
		}
	}

	public int ChargesPreReqForGrantBlockToTeamForAmountOfDamageBlocked
	{
		get
		{
			return chargesPreReqForGrantBlockToTeamForAmountOfDamageBlocked;
		}
		set
		{
			chargesPreReqForGrantBlockToTeamForAmountOfDamageBlocked = value;
		}
	}

	public int HealTotalOnExplode
	{
		get
		{
			return healTotalOnExplode;
		}
		set
		{
			healTotalOnExplode = value;
		}
	}

	public float HealPerChargeOnExplode
	{
		get
		{
			return healPerChargeOnExplode;
		}
		set
		{
			healPerChargeOnExplode = value;
		}
	}

	public Enums.AuraCurseExplodeHealTarget HealTargetOnExplode
	{
		get
		{
			return healTargetOnExplode;
		}
		set
		{
			healTargetOnExplode = value;
		}
	}

	public AuraCurseData ACOnExplode
	{
		get
		{
			return acOnExplode;
		}
		set
		{
			acOnExplode = value;
		}
	}

	public int ACTotalChargesOnExplode
	{
		get
		{
			return acTotalChargesOnExplode;
		}
		set
		{
			acTotalChargesOnExplode = value;
		}
	}

	public int ACChargesPerStackChargeOnExplode
	{
		get
		{
			return acChargesPerStackChargeOnExplodeOnExplode;
		}
		set
		{
			acChargesPerStackChargeOnExplodeOnExplode = value;
		}
	}

	public bool SkipEndTurnRemovalIfNoBegin => skipEndTurnRemovalIfNoBegin;

	public void Init()
	{
		if (string.IsNullOrEmpty(id))
		{
			id = Regex.Replace(acName, "\\s+", "").ToLower();
		}
	}

	public AuraCurseData DeepClone()
	{
		AuraCurseData auraCurseData = UnityEngine.Object.Instantiate(this);
		if (auraDamageChargesBasedOnACCharges != null)
		{
			auraCurseData.auraDamageChargesBasedOnACCharges = UnityEngine.Object.Instantiate(auraDamageChargesBasedOnACCharges);
		}
		if (preventedAuraCurse != null)
		{
			auraCurseData.preventedAuraCurse = UnityEngine.Object.Instantiate(preventedAuraCurse);
		}
		if (consumedDamageChargesBasedOnACCharges != null)
		{
			auraCurseData.consumedDamageChargesBasedOnACCharges = UnityEngine.Object.Instantiate(consumedDamageChargesBasedOnACCharges);
		}
		if ((bool)removeAuraCurse)
		{
			auraCurseData.removeAuraCurse = UnityEngine.Object.Instantiate(removeAuraCurse);
		}
		if ((bool)removeAuraCurse2)
		{
			auraCurseData.removeAuraCurse2 = UnityEngine.Object.Instantiate(removeAuraCurse2);
		}
		if (gainAuraCurseConsumption != null)
		{
			auraCurseData.gainAuraCurseConsumption = UnityEngine.Object.Instantiate(gainAuraCurseConsumption);
		}
		if (gainAuraCurseConsumption2 != null)
		{
			auraCurseData.gainAuraCurseConsumption2 = UnityEngine.Object.Instantiate(gainAuraCurseConsumption2);
		}
		if (gainChargesFromThisAuraCurse != null)
		{
			auraCurseData.gainChargesFromThisAuraCurse = UnityEngine.Object.Instantiate(gainChargesFromThisAuraCurse);
		}
		if (gainChargesFromThisAuraCurse2 != null)
		{
			auraCurseData.gainChargesFromThisAuraCurse2 = UnityEngine.Object.Instantiate(gainChargesFromThisAuraCurse2);
		}
		return auraCurseData;
	}

	public int GetMaxCharges()
	{
		int result = maxCharges;
		if (MadnessManager.Instance.IsMadnessTraitActive("restrictedpower") || AtOManager.Instance.IsChallengeTraitActive("restrictedpower"))
		{
			result = maxMadnessCharges;
		}
		return result;
	}

	public AudioClip GetSound(bool useLegacy = false)
	{
		if (!GameManager.Instance.ConfigUseLegacySounds && soundRework != null && !useLegacy)
		{
			return soundRework;
		}
		return sound;
	}
}
