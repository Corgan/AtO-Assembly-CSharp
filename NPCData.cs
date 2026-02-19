using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC", menuName = "NPC Data", order = 54)]
public class NPCData : ScriptableObject
{
	[SerializeField]
	private string npcName = "";

	[SerializeField]
	private string scriptableObjectName = "";

	[SerializeField]
	private string id;

	[SerializeField]
	private string description;

	[SerializeField]
	private bool isBoss;

	[SerializeField]
	private bool isNamed;

	[SerializeField]
	public bool OnlyKillBossWhenHpZero;

	[SerializeField]
	private NPCData baseMonster;

	[SerializeField]
	private bool bigModel;

	[SerializeField]
	private bool finishCombatOnDead;

	[SerializeField]
	private bool female;

	[Header("Tier and upgrade")]
	[SerializeField]
	private Enums.CombatTier tierMob;

	[SerializeField]
	private NPCData upgradedMob;

	[Header("NG+ Id")]
	[SerializeField]
	private NPCData ngPlusMob;

	[Header("Hell/Hard Id")]
	[SerializeField]
	private NPCData hellModeMob;

	[Header("Obelisk Challenge")]
	[SerializeField]
	private int difficulty;

	[SerializeField]
	private Enums.CardTargetPosition preferredPosition;

	[Header("Game objects")]
	[SerializeField]
	private GameObject gameObjectAnimated;

	[SerializeField]
	private Sprite sprite;

	[SerializeField]
	private Sprite spriteSpeed;

	[SerializeField]
	private Sprite spritePortrait;

	[SerializeField]
	private float posBottom;

	[SerializeField]
	private float fluffOffsetX;

	[SerializeField]
	private float fluffOffsetY;

	[Header("Alternative Game objects")]
	[SerializeField]
	private GameObject gameObjectAnimatedAlternate;

	[SerializeField]
	private Sprite spriteSpeedAlternate;

	[SerializeField]
	private Sprite spritePortraitAlternate;

	[Header("Sound")]
	[SerializeField]
	private AudioClip hitSound;

	[Header("Sound (new)")]
	[SerializeField]
	public List<AudioClip> hitSoundRework;

	[Header("Stats")]
	[SerializeField]
	private int hp;

	[SerializeField]
	private int energy;

	[SerializeField]
	private int energyTurn;

	[SerializeField]
	private int speed;

	[Header("Resits")]
	[SerializeField]
	private int resistSlashing;

	[SerializeField]
	private int resistBlunt;

	[SerializeField]
	private int resistPiercing;

	[SerializeField]
	private int resistFire;

	[SerializeField]
	private int resistCold;

	[SerializeField]
	private int resistLightning;

	[SerializeField]
	private int resistMind;

	[SerializeField]
	private int resistHoly;

	[SerializeField]
	private int resistShadow;

	[Header("Immunities")]
	[SerializeField]
	private List<string> auracurseImmune = new List<string>();

	[Header("Cards")]
	[SerializeField]
	private int cardsInHand;

	[SerializeField]
	private AICards[] aiCards;

	[Header("Rewards")]
	[SerializeField]
	private int experienceReward;

	[SerializeField]
	private int goldReward;

	[SerializeField]
	private TierRewardData tierReward;

	public string NPCName
	{
		get
		{
			return npcName;
		}
		set
		{
			npcName = value;
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

	public bool IsBoss
	{
		get
		{
			return isBoss;
		}
		set
		{
			isBoss = value;
		}
	}

	public bool BigModel
	{
		get
		{
			return bigModel;
		}
		set
		{
			bigModel = value;
		}
	}

	public bool FinishCombatOnDead
	{
		get
		{
			return finishCombatOnDead;
		}
		set
		{
			finishCombatOnDead = value;
		}
	}

	public int Difficulty
	{
		get
		{
			return difficulty;
		}
		set
		{
			difficulty = value;
		}
	}

	public Enums.CardTargetPosition PreferredPosition
	{
		get
		{
			return preferredPosition;
		}
		set
		{
			preferredPosition = value;
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

	public GameObject GameObjectAnimatedAlternate
	{
		get
		{
			return gameObjectAnimatedAlternate;
		}
		set
		{
			gameObjectAnimatedAlternate = value;
		}
	}

	public Sprite Sprite => sprite;

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

	public Sprite SpriteSpeedAlternate => spriteSpeedAlternate;

	public float PosBottom
	{
		get
		{
			return posBottom;
		}
		set
		{
			posBottom = value;
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

	public AICards[] AICards
	{
		get
		{
			return aiCards;
		}
		set
		{
			aiCards = value;
		}
	}

	public int CardsInHand
	{
		get
		{
			return cardsInHand;
		}
		set
		{
			cardsInHand = value;
		}
	}

	public AudioClip HitSound
	{
		get
		{
			return hitSound;
		}
		set
		{
			hitSound = value;
		}
	}

	public int ExperienceReward
	{
		get
		{
			return experienceReward;
		}
		set
		{
			experienceReward = value;
		}
	}

	public int GoldReward
	{
		get
		{
			return goldReward;
		}
		set
		{
			goldReward = value;
		}
	}

	public TierRewardData TierReward
	{
		get
		{
			return tierReward;
		}
		set
		{
			tierReward = value;
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

	public Sprite SpritePortraitAlternate
	{
		get
		{
			return spritePortraitAlternate;
		}
		set
		{
			spritePortraitAlternate = value;
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

	public float FluffOffsetX
	{
		get
		{
			return fluffOffsetX;
		}
		set
		{
			fluffOffsetX = value;
		}
	}

	public float FluffOffsetY
	{
		get
		{
			return fluffOffsetY;
		}
		set
		{
			fluffOffsetY = value;
		}
	}

	public Enums.CombatTier TierMob
	{
		get
		{
			return tierMob;
		}
		set
		{
			tierMob = value;
		}
	}

	public NPCData UpgradedMob
	{
		get
		{
			return upgradedMob;
		}
		set
		{
			upgradedMob = value;
		}
	}

	public NPCData NgPlusMob
	{
		get
		{
			return ngPlusMob;
		}
		set
		{
			ngPlusMob = value;
		}
	}

	public bool IsNamed
	{
		get
		{
			return isNamed;
		}
		set
		{
			isNamed = value;
		}
	}

	public NPCData HellModeMob
	{
		get
		{
			return hellModeMob;
		}
		set
		{
			hellModeMob = value;
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

	public bool Female
	{
		get
		{
			return female;
		}
		set
		{
			female = value;
		}
	}

	public NPCData BaseMonster
	{
		get
		{
			return baseMonster;
		}
		set
		{
			baseMonster = value;
		}
	}

	public AudioClip GetHitSound(int _soundIndex = 0)
	{
		if (GameManager.Instance.ConfigUseLegacySoundsSheepOwl && hitSound != null && (hitSound.name == "sheephit" || hitSound.name == "owl_hoot_1" || hitSound.name == "owl_hoot_2"))
		{
			return hitSound;
		}
		if (!GameManager.Instance.ConfigUseLegacySounds && hitSoundRework != null && hitSoundRework.Count > 0)
		{
			int num = _soundIndex % hitSoundRework.Count;
			if (num < hitSoundRework.Count && hitSoundRework[num] != null)
			{
				return hitSoundRework[num];
			}
		}
		return hitSound;
	}
}
