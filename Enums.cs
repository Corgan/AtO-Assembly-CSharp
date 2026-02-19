using System.Collections.Generic;

public class Enums
{
	public enum Platform
	{
		PC,
		Switch,
		PS5,
		XBOX
	}

	public enum GameMode
	{
		SinglePlayer,
		Multiplayer
	}

	public enum GameStatus
	{
		NewGame,
		LoadGame
	}

	public enum GameType
	{
		Adventure,
		Challenge,
		WeeklyChallenge,
		Singularity
	}

	public enum ConfigSpeed
	{
		Slow,
		Fast,
		Ultrafast
	}

	public enum ItemTarget
	{
		None = -1,
		Self,
		RandomHero,
		RandomEnemy,
		AllHero,
		AllEnemy,
		CurrentTarget,
		SelfEnemy,
		HighestFlatHpHero,
		LowestFlatHpHero,
		HighestFlatHpEnemy,
		LowestFlatHpEnemy,
		Random,
		Custom,
		FrontEnemy
	}

	public enum ItemSlot
	{
		None,
		Weapon,
		Armor,
		Jewelry,
		Accesory,
		Pet,
		AllWithoutPet,
		AllIncludedPet
	}

	public enum CardClass
	{
		Warrior,
		Mage,
		Healer,
		Scout,
		MagicKnight,
		Monster,
		Injury,
		Boon,
		Item,
		Special,
		Enchantment,
		None
	}

	public enum CardRarity
	{
		Common,
		Uncommon,
		Rare,
		Epic,
		Mythic
	}

	public enum CardType
	{
		None,
		Melee_Attack,
		Ranged_Attack,
		Magic_Attack,
		Defense,
		Fire_Spell,
		Cold_Spell,
		Lightning_Spell,
		Mind_Spell,
		Shadow_Spell,
		Holy_Spell,
		Curse_Spell,
		Healing_Spell,
		Book,
		Small_Weapon,
		Song,
		Skill,
		Power,
		Injury,
		Attack,
		Spell,
		Boon,
		Weapon,
		Armor,
		Jewelry,
		Accesory,
		Pet,
		Corruption,
		Enchantment,
		Food,
		Flask,
		Petrare
	}

	public enum CardUpgraded
	{
		No,
		A,
		B,
		Rare
	}

	public enum CardTargetType
	{
		Single,
		Global,
		SingleSided
	}

	public enum CardTargetSide
	{
		Enemy,
		Friend,
		Anyone,
		Self,
		FriendNotSelf
	}

	public enum CardTargetPosition
	{
		Anywhere,
		Front,
		Random,
		Back,
		Middle,
		Slowest,
		Fastest,
		LeastHP,
		MostHP
	}

	public enum CardPlace
	{
		Discard,
		TopDeck,
		BottomDeck,
		RandomDeck,
		Vanish,
		Hand,
		Cast
	}

	public enum CardFrom
	{
		Deck,
		Discard,
		Game,
		Hand,
		Vanish
	}

	public enum CardSpecialValue
	{
		None,
		AuraCurseYours,
		AuraCurseTarget,
		CardsHand,
		CardsDeck,
		CardsDiscard,
		HealthYours,
		HealthTarget,
		CardsVanish,
		CardsDeckTarget,
		CardsDiscardTarget,
		CardsVanishTarget,
		SpeedYours,
		SpeedTarget,
		SpeedDifference,
		MissingHealthYours,
		MissingHealthTarget,
		DiscardedCards,
		VanishedCards
	}

	public enum DamageType
	{
		None,
		Slashing,
		Blunt,
		Piercing,
		Fire,
		Cold,
		Lightning,
		Mind,
		Holy,
		Shadow,
		All
	}

	public enum EffectRepeatTarget
	{
		Same,
		Random,
		Chain,
		NoRepeat,
		NeverRepeat
	}

	public enum CharacterStat
	{
		None,
		Hp,
		Speed,
		Energy,
		EnergyTurn
	}

	public enum HeroClass
	{
		Warrior,
		Mage,
		Healer,
		Scout,
		MagicKnight,
		Monster,
		None
	}

	public enum CombatScrollEffectType
	{
		Damage,
		Heal,
		Aura,
		Curse,
		Energy,
		Text,
		Block,
		Trait,
		Weapon,
		Armor,
		Jewelry,
		Accesory,
		Corruption
	}

	public enum TargetCast
	{
		Random,
		Front,
		Middle,
		Back,
		AnyButFront,
		AnyButBack,
		LessHealthPercent,
		LessHealthFlat,
		MoreHealthPercent,
		MoreHealthFlat,
		LessHealthAbsolute,
		MoreHealthAbsolute,
		LessInitiative,
		MoreInitiative
	}

	public enum OnlyCastIf
	{
		Always,
		TargetLifeLessThanPercent,
		TargetHasNotAuraCurse,
		TargetLifeHigherThanPercent,
		TargetHasAuraCurse,
		TargetHasAnyAura,
		TargetHasAnyCurse,
		TeamNpcAllAlive,
		LifeInMainTargetHigherThanInSpecialTarget,
		LifeInMainTargetLessThanInSpecialTarget,
		TargetNotIllusion
	}

	public enum LogType
	{
		Text,
		Damage,
		Heal,
		Aura,
		Curse
	}

	public enum EffectTrailAngle
	{
		Horizontal,
		Parabolic,
		Diagonal,
		Vertical
	}

	public enum EventActivation
	{
		None = 0,
		BeginCombat = 1,
		BeginRound = 2,
		BeginTurn = 3,
		EndTurn = 4,
		EndRound = 5,
		DrawCard = 6,
		CastCard = 7,
		Hit = 8,
		Hitted = 9,
		Block = 10,
		Blocked = 11,
		Evade = 12,
		Evaded = 13,
		Heal = 14,
		Healed = 15,
		FinishCast = 16,
		BeginCombatEnd = 17,
		CharacterAssign = 18,
		Damage = 19,
		Damaged = 20,
		PreBeginCombat = 21,
		BeginTurnCardsDealt = 22,
		AuraCurseSet = 23,
		BeginTurnAboutToDealCards = 24,
		Killed = 25,
		DestroyItem = 26,
		CorruptionCombatStart = 27,
		CorruptionBeginRound = 28,
		DamagedSecondary = 29,
		FinishFinishCast = 30,
		PreFinishCast = 31,
		Resurrect = 32,
		ItemActivation = 33,
		TraitActivation = 34,
		CharacterKilled = 35,
		FinishFinishCastCorruption = 36,
		DamagedDueToACConsumption = 37,
		BlockReachedZero = 38,
		AfterDealCards = 39,
		CastFightCard = 40,
		OnLeechExplosion = 41,
		AuraCurseRemoved = 42,
		EnemyBeginTurnDelay = 43,
		ResetDeck = 44,
		PetActivatedByCard = 45,
		PetActivatedAuto = 46,
		CardMoved = 47,
		CurseExploded = 55,
		Manual = 56,
		CurseRemoved = 57,
		Overhealed = 58
	}

	public enum ActivationManual
	{
		None = 0,
		DarkSpike = 2,
		PurifyingResonance = 3
	}

	public enum Zone
	{
		Senenthia,
		Velkarath,
		Aquarfall,
		Faeborg,
		VoidLow,
		VoidHigh,
		Sectarium,
		SpiderLair,
		FrozenSewers,
		BlackForge,
		None,
		WolfWars,
		Ulminin,
		Pyramid,
		Sahti,
		Dreadnought,
		WitchWoods,
		CastleCourtyard,
		CastleSpire,
		Dreams
	}

	public enum NodeGround
	{
		None,
		HeavyRain,
		ExtremeHeat,
		FreezingCold,
		HolyGround,
		Graveyard,
		PoisonousAir,
		StaticElectricity,
		EerieAtmosphere,
		DampCavern,
		BloodMist,
		Infestation
	}

	public enum CombatBackground
	{
		Senenthia_Plains,
		Senenthia_Forest,
		Senenthia_Night,
		Sectarium,
		Velkarath_Stone,
		Velkarath_Lava0,
		Velkarath_Lavavolcan,
		Velkarath_Lava2,
		Spider_Lair,
		Aquarfall_Main,
		Aquarfall_Water,
		Aquarfall_Boss,
		Senenthia_Deepforest,
		Void_Regular,
		Void_Stairs,
		Void_Obelisk,
		Void_StairsClose,
		Void_StairsClose2,
		Void_RegularUP,
		Void_ObeliskClose,
		Void_ObeliskClose2,
		Senenthia_Dia,
		Senenthia_Tarde,
		Senenthia_LobosTarde,
		Senenthia_LobosNoche,
		Senenthia_BosqueEntrada,
		Senenthia_BosqueBoss,
		Senenthia_Bosque,
		Velkarath_Lava1,
		Velkarath_Arena,
		Velkarath_Cave,
		Void_Boss,
		Void_BossFinal,
		Void_Obelisk_Lila,
		Void_ObeliskClose2_Lila,
		Void_Regular_Rosa,
		Void_Stairs_Rosa,
		Void_StairsClose_Rosa,
		Void_StairsClose2_Rosa,
		Void_Twins,
		Faeborg_Armeria,
		Faeborg_Bosque,
		Faeborg_BosqueCastor,
		Faeborg_Boss,
		Faeborg_Ciudad,
		Faeborg_Costa,
		Faeborg_Rabbit,
		Forge_Boss,
		Forge_Mid,
		Forge_Pared,
		Sewers,
		Wolfwars_Boss,
		Wolfwars_Bree,
		Wolfwars_Forest,
		Wolfwars_Talado,
		Wolfwars_Torre,
		Ulminin_Arriba,
		Ulminin_Arriba2,
		Ulminin_Centro,
		Ulminin_Centro2,
		Ulminin_Desierto,
		Ulminin_Desierto2,
		Ulminin_DesiertoGusano,
		Ulminin_Oasis,
		Ulminin_Oasis2,
		Ulminin_Primero,
		Ulminin_Catacombs,
		Ulminin_CatacombsBoss,
		Ulminin_Pyramid,
		Ulminin_PyramidBoss,
		Uprising_LowEnergy,
		Uprising_LowLava,
		Uprising_Mid,
		Uprising_MidElevator,
		Uprising_Top,
		Dread_Halfman,
		Dread_Ship,
		Sahti_Cavern,
		Sahti_Kraken,
		Sahti_Rust,
		Sahti_Ship,
		Sahti_ShipCloudy,
		Sahti_ShipCloudyStorm,
		Sahti_ShipStorm,
		Sunken_BossBlood,
		Sunken_BossHoly,
		Sunken_BossThunder,
		Sunken_Firstfloor,
		Sunken_Firstflooraltar,
		Sunken_Secondfloor,
		Sunken_Secondflooraltar,
		Castle_Interior,
		Outskirts_Graveyard,
		Castle_Interior_Night,
		Courtyard_Day,
		Courtyard_Night,
		Graveyard_Day,
		Forest_Day,
		Forest_Night,
		Archon_Throne_Room,
		Siren_Queen_Throne_Room,
		Dracula_Throne_Room,
		Franky_Lab,
		Senenthia_Dia_Nightmare,
		Senenthia_BosqueEntrada_Nightmare
	}

	public enum CombatStepSound
	{
		Walk_Stone,
		Walk_Grass,
		Walk_Water,
		None
	}

	public enum CombatTier
	{
		T0,
		T1,
		T2,
		T3,
		T4,
		T5,
		T6,
		T7,
		T8,
		T9,
		T10,
		T11,
		T12,
		T13
	}

	public enum CombatUnit
	{
		All,
		Heroes,
		Monsters,
		Hero0,
		Hero1,
		Hero2,
		Hero3,
		Monster0,
		Monster1,
		Monster2,
		Monster3
	}

	public enum RollMode
	{
		HigherOrEqual,
		LowerOrEqual,
		Highest,
		Lowest,
		Closest
	}

	public enum RollTarget
	{
		Single,
		Competition,
		Group,
		Character
	}

	public enum EventAction
	{
		None,
		CharacterName,
		Combat,
		Jump,
		Run,
		Bribe,
		Threaten,
		Persuade,
		Stealth,
		Steal,
		SneakAway,
		Leave,
		Loot,
		Profane,
		Forage,
		Decline,
		Accept,
		Extorsion,
		Ambush,
		Race,
		Rest,
		Bury,
		Dig,
		Break,
		Bet,
		Play,
		Fishing,
		Pay,
		Bargain,
		Continue,
		Explore,
		Enter,
		Pretend,
		Capture,
		Throw,
		Look,
		Examine,
		Repair,
		Rebuild,
		Heal,
		Climb,
		Evade,
		Use,
		Ask,
		Answer,
		Open,
		Buy,
		Murder,
		Burn,
		Kill,
		Give,
		Talk,
		Sing,
		Read,
		Help,
		Prepare,
		Rush,
		Study
	}

	public enum MapIconShader
	{
		None,
		Green,
		Blue,
		Orange,
		Purple,
		Red,
		Black
	}

	public enum PerkCost
	{
		PerkCostBase = 1,
		PerkCostAdvanced = 3
	}

	public enum SpecialValueModifierName
	{
		RuneCharges,
		DamageDealt,
		AuracurseSetCharges,
		AuraCurseYours
	}

	public enum RefectedDamageModifierType
	{
		DamagePerAuraCharge,
		DamagePerDamageReceived,
		DamagePerDamageBlocked
	}

	public enum AuraCurseExplodeHealTarget
	{
		None,
		Caster,
		CasterTeam,
		Target,
		TargetTeam
	}

	public enum ActivePets
	{
		None,
		AllTeam,
		Self,
		CurrentTarget
	}

	public static HashSet<CardType> SpellCardTypes = new HashSet<CardType>
	{
		CardType.Spell,
		CardType.Cold_Spell,
		CardType.Curse_Spell,
		CardType.Fire_Spell,
		CardType.Healing_Spell,
		CardType.Holy_Spell,
		CardType.Lightning_Spell,
		CardType.Mind_Spell,
		CardType.Shadow_Spell
	};
}
