using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New SubClass", menuName = "SubClass Data", order = 53)]
public class SubClassData : ScriptableObject
{
	[SerializeField]
	private string subClassName;

	private string id;

	[SerializeField]
	private string characterName;

	private string sourceCharacterName;

	[SerializeField]
	private bool mainCharacter;

	[SerializeField]
	private bool initialUnlock;

	[TextArea]
	[SerializeField]
	private string characterDescription;

	[TextArea]
	[SerializeField]
	private string characterDescriptionStrength;

	[SerializeField]
	private Sprite sprite;

	[SerializeField]
	private GameObject gameObjectAnimated;

	[SerializeField]
	private Sprite spriteBorder;

	[SerializeField]
	private Sprite spriteBorderSmall;

	[SerializeField]
	private Sprite spriteBorderLocked;

	[SerializeField]
	private Sprite spriteSpeed;

	[SerializeField]
	private Sprite spritePortrait;

	[Header("Class")]
	[SerializeField]
	private Enums.HeroClass heroClass;

	[SerializeField]
	private Enums.HeroClass heroClassSecondary;

	[SerializeField]
	private Enums.HeroClass heroClassThird;

	[Header("Sound")]
	[SerializeField]
	private AudioClip actionSound;

	[SerializeField]
	private AudioClip hitSound;

	[Header("Sound (new)")]
	[SerializeField]
	private List<AudioClip> hitSoundRework;

	[Header("Misc")]
	[SerializeField]
	private float fluffOffsetX;

	[SerializeField]
	private float fluffOffsetY;

	[SerializeField]
	private bool female;

	[Header("DLC Requeriment")]
	[SerializeField]
	private string sku;

	[Header("Sticker")]
	[SerializeField]
	private Sprite stickerBase;

	[SerializeField]
	private Sprite stickerLove;

	[SerializeField]
	private Sprite stickerSurprise;

	[SerializeField]
	private Sprite stickerAngry;

	[SerializeField]
	private Sprite stickerIndiferent;

	[SerializeField]
	private float stickerOffsetX;

	[Header("Ingame Parameters")]
	[SerializeField]
	private int orderInList;

	[SerializeField]
	private bool blocked = true;

	[Header("Main Stats")]
	[SerializeField]
	private int speed;

	[SerializeField]
	private int hp;

	[SerializeField]
	private int energy;

	[SerializeField]
	private int energyTurn;

	[Header("Resists")]
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

	[Header("Item")]
	[SerializeField]
	private CardData item;

	[Header("LevelUp")]
	[SerializeField]
	private int[] maxHp;

	[Header("Cards")]
	[SerializeField]
	private HeroCards[] cards;

	[Header("Traits")]
	[SerializeField]
	private List<TraitData> traits;

	[Header("Trait List")]
	[SerializeField]
	private TraitData trait0;

	[Space(10f)]
	[SerializeField]
	private TraitData trait1A;

	[SerializeField]
	private CardData trait1ACard;

	[SerializeField]
	private TraitData trait1B;

	[SerializeField]
	private CardData trait1BCard;

	[Space(10f)]
	[SerializeField]
	private TraitData trait2A;

	[SerializeField]
	private TraitData trait2B;

	[Space(10f)]
	[SerializeField]
	private TraitData trait3A;

	[SerializeField]
	private CardData trait3ACard;

	[SerializeField]
	private TraitData trait3B;

	[SerializeField]
	private CardData trait3BCard;

	[Space(10f)]
	[SerializeField]
	private TraitData trait4A;

	[SerializeField]
	private TraitData trait4B;

	[Header("Challenge packs")]
	[SerializeField]
	private PackData challengePack0;

	[SerializeField]
	private PackData challengePack1;

	[SerializeField]
	private PackData challengePack2;

	[SerializeField]
	private PackData challengePack3;

	[SerializeField]
	private PackData challengePack4;

	[SerializeField]
	private PackData challengePack5;

	[SerializeField]
	private PackData challengePack6;

	[Header("Cards Singularity")]
	[SerializeField]
	private CardData[] cardsSingularity;

	public string SubClassName
	{
		get
		{
			return subClassName;
		}
		set
		{
			subClassName = value;
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

	public string CharacterName
	{
		get
		{
			return characterName;
		}
		set
		{
			characterName = value;
		}
	}

	public string CharacterDescription
	{
		get
		{
			return characterDescription;
		}
		set
		{
			characterDescription = value;
		}
	}

	public string CharacterDescriptionStrength
	{
		get
		{
			return characterDescriptionStrength;
		}
		set
		{
			characterDescriptionStrength = value;
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

	public Enums.HeroClass HeroClass
	{
		get
		{
			return heroClass;
		}
		set
		{
			heroClass = value;
		}
	}

	public int OrderInList
	{
		get
		{
			return orderInList;
		}
		set
		{
			orderInList = value;
		}
	}

	public bool Blocked
	{
		get
		{
			return blocked;
		}
		set
		{
			blocked = value;
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

	public HeroCards[] Cards
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

	public CardData[] CardsSingularity
	{
		get
		{
			return cardsSingularity;
		}
		set
		{
			cardsSingularity = value;
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

	public List<TraitData> Traits
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

	public TraitData Trait0
	{
		get
		{
			return trait0;
		}
		set
		{
			trait0 = value;
		}
	}

	public TraitData Trait1A
	{
		get
		{
			return trait1A;
		}
		set
		{
			trait1A = value;
		}
	}

	public TraitData Trait1B
	{
		get
		{
			return trait1B;
		}
		set
		{
			trait1B = value;
		}
	}

	public TraitData Trait2A
	{
		get
		{
			return trait2A;
		}
		set
		{
			trait2A = value;
		}
	}

	public TraitData Trait2B
	{
		get
		{
			return trait2B;
		}
		set
		{
			trait2B = value;
		}
	}

	public TraitData Trait3A
	{
		get
		{
			return trait3A;
		}
		set
		{
			trait3A = value;
		}
	}

	public TraitData Trait3B
	{
		get
		{
			return trait3B;
		}
		set
		{
			trait3B = value;
		}
	}

	public TraitData Trait4A
	{
		get
		{
			return trait4A;
		}
		set
		{
			trait4A = value;
		}
	}

	public TraitData Trait4B
	{
		get
		{
			return trait4B;
		}
		set
		{
			trait4B = value;
		}
	}

	public Sprite SpriteBorder
	{
		get
		{
			return spriteBorder;
		}
		set
		{
			spriteBorder = value;
		}
	}

	public Sprite SpriteBorderSmall
	{
		get
		{
			return spriteBorderSmall;
		}
		set
		{
			spriteBorderSmall = value;
		}
	}

	public int[] MaxHp
	{
		get
		{
			return maxHp;
		}
		set
		{
			maxHp = value;
		}
	}

	public CardData Item
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

	public Sprite SpriteBorderLocked
	{
		get
		{
			return spriteBorderLocked;
		}
		set
		{
			spriteBorderLocked = value;
		}
	}

	public CardData Trait1ACard
	{
		get
		{
			return trait1ACard;
		}
		set
		{
			trait1ACard = value;
		}
	}

	public CardData Trait1BCard
	{
		get
		{
			return trait1BCard;
		}
		set
		{
			trait1BCard = value;
		}
	}

	public CardData Trait3ACard
	{
		get
		{
			return trait3ACard;
		}
		set
		{
			trait3ACard = value;
		}
	}

	public CardData Trait3BCard
	{
		get
		{
			return trait3BCard;
		}
		set
		{
			trait3BCard = value;
		}
	}

	public AudioClip ActionSound
	{
		get
		{
			return actionSound;
		}
		set
		{
			actionSound = value;
		}
	}

	public Sprite StickerLove
	{
		get
		{
			return stickerLove;
		}
		set
		{
			stickerLove = value;
		}
	}

	public Sprite StickerSurprise
	{
		get
		{
			return stickerSurprise;
		}
		set
		{
			stickerSurprise = value;
		}
	}

	public Sprite StickerAngry
	{
		get
		{
			return stickerAngry;
		}
		set
		{
			stickerAngry = value;
		}
	}

	public Sprite StickerIndiferent
	{
		get
		{
			return stickerIndiferent;
		}
		set
		{
			stickerIndiferent = value;
		}
	}

	public Sprite StickerBase
	{
		get
		{
			return stickerBase;
		}
		set
		{
			stickerBase = value;
		}
	}

	public float StickerOffsetX
	{
		get
		{
			return stickerOffsetX;
		}
		set
		{
			stickerOffsetX = value;
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

	public PackData ChallengePack0
	{
		get
		{
			return challengePack0;
		}
		set
		{
			challengePack0 = value;
		}
	}

	public PackData ChallengePack1
	{
		get
		{
			return challengePack1;
		}
		set
		{
			challengePack1 = value;
		}
	}

	public PackData ChallengePack2
	{
		get
		{
			return challengePack2;
		}
		set
		{
			challengePack2 = value;
		}
	}

	public PackData ChallengePack3
	{
		get
		{
			return challengePack3;
		}
		set
		{
			challengePack3 = value;
		}
	}

	public PackData ChallengePack4
	{
		get
		{
			return challengePack4;
		}
		set
		{
			challengePack4 = value;
		}
	}

	public PackData ChallengePack5
	{
		get
		{
			return challengePack5;
		}
		set
		{
			challengePack5 = value;
		}
	}

	public PackData ChallengePack6
	{
		get
		{
			return challengePack6;
		}
		set
		{
			challengePack6 = value;
		}
	}

	public bool MainCharacter
	{
		get
		{
			return mainCharacter;
		}
		set
		{
			mainCharacter = value;
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

	public Enums.HeroClass HeroClassSecondary
	{
		get
		{
			return heroClassSecondary;
		}
		set
		{
			heroClassSecondary = value;
		}
	}

	public Enums.HeroClass HeroClassThird
	{
		get
		{
			return heroClassThird;
		}
		set
		{
			heroClassThird = value;
		}
	}

	public bool InitialUnlock
	{
		get
		{
			return initialUnlock;
		}
		set
		{
			initialUnlock = value;
		}
	}

	public string SourceCharacterName
	{
		get
		{
			return sourceCharacterName;
		}
		set
		{
			sourceCharacterName = value;
		}
	}

	private void Awake()
	{
		if (!string.IsNullOrEmpty(subClassName))
		{
			Init();
		}
	}

	public void Init()
	{
		id = Regex.Replace(subClassName, "\\s+", "").ToLower();
		sourceCharacterName = characterName;
	}

	public bool IsMultiClass()
	{
		return heroClassSecondary != Enums.HeroClass.None;
	}

	public Sprite GetEmoteBase()
	{
		return stickerBase;
	}

	public Sprite GetEmote(int _action)
	{
		return _action switch
		{
			0 => stickerLove, 
			1 => stickerSurprise, 
			4 => stickerIndiferent, 
			5 => stickerAngry, 
			_ => null, 
		};
	}

	public int GetTraitLevel(string traitName)
	{
		traitName = traitName.ToLower();
		if (trait0.Id == traitName)
		{
			return 0;
		}
		if (trait1A.Id == traitName)
		{
			return 1;
		}
		if (trait1B.Id == traitName)
		{
			return 1;
		}
		if (trait2A.Id == traitName)
		{
			return 2;
		}
		if (trait2B.Id == traitName)
		{
			return 2;
		}
		if (trait3A.Id == traitName)
		{
			return 3;
		}
		if (trait3B.Id == traitName)
		{
			return 3;
		}
		if (trait4A.Id == traitName)
		{
			return 4;
		}
		if (trait4B.Id == traitName)
		{
			return 4;
		}
		return -1;
	}

	public AudioClip GetHitSound(int _soundIndex = 0)
	{
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

	public List<string> GetCardsId()
	{
		List<string> list = new List<string>();
		for (int i = 0; i < cards.Length; i++)
		{
			if (!list.Contains(cards[i].Card.Id))
			{
				list.Add(cards[i].Card.Id);
			}
		}
		return list;
	}
}
