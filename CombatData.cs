using UnityEngine;

[CreateAssetMenu(fileName = "New Combat", menuName = "Combat Data", order = 61)]
public class CombatData : ScriptableObject
{
	[SerializeField]
	private string combatId;

	[TextArea]
	[SerializeField]
	private string description;

	[SerializeField]
	private Enums.CombatBackground combatBackground;

	[SerializeField]
	private AudioClip combatMusic;

	[SerializeField]
	private Enums.CombatStepSound stepSound;

	public bool NeverRandomizeEnemies;

	[SerializeField]
	private NPCData[] npcList;

	[SerializeField]
	private NPCData npcToSummonOnNpcKilled;

	[SerializeField]
	private bool randomizeNpcPosition;

	[SerializeField]
	private int npcRemoveInMadness0Index = -1;

	[Header("Thermometer")]
	[SerializeField]
	private ThermometerTierData thermometerTierData;

	[Header("Combat Effects")]
	[SerializeField]
	private CombatEffect[] combatEffect;

	[SerializeField]
	private bool healHeroes;

	[Header("Combat Tier")]
	[SerializeField]
	private Enums.CombatTier combatTier;

	[Header("Launch event at the end of combat (w/ requisite)")]
	[SerializeField]
	private EventData eventData;

	[SerializeField]
	private EventRequirementData eventRequirementData;

	[Header("Cinematic after the combat")]
	[SerializeField]
	private CinematicData cinematicData;

	[Header("Is this a node with a Rift?")]
	[SerializeField]
	private bool isRift;

	public NPCData[] NPCList
	{
		get
		{
			return npcList;
		}
		set
		{
			npcList = value;
		}
	}

	public string CombatId
	{
		get
		{
			return combatId;
		}
		set
		{
			combatId = value;
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

	public Enums.CombatBackground CombatBackground
	{
		get
		{
			return combatBackground;
		}
		set
		{
			combatBackground = value;
		}
	}

	public CombatEffect[] CombatEffect
	{
		get
		{
			return combatEffect;
		}
		set
		{
			combatEffect = value;
		}
	}

	public Enums.CombatTier CombatTier
	{
		get
		{
			return combatTier;
		}
		set
		{
			combatTier = value;
		}
	}

	public EventData EventData
	{
		get
		{
			return eventData;
		}
		set
		{
			eventData = value;
		}
	}

	public EventRequirementData EventRequirementData
	{
		get
		{
			return eventRequirementData;
		}
		set
		{
			eventRequirementData = value;
		}
	}

	public ThermometerTierData ThermometerTierData
	{
		get
		{
			return thermometerTierData;
		}
		set
		{
			thermometerTierData = value;
		}
	}

	public AudioClip CombatMusic
	{
		get
		{
			return combatMusic;
		}
		set
		{
			combatMusic = value;
		}
	}

	public CinematicData CinematicData
	{
		get
		{
			return cinematicData;
		}
		set
		{
			cinematicData = value;
		}
	}

	public bool HealHeroes
	{
		get
		{
			return healHeroes;
		}
		set
		{
			healHeroes = value;
		}
	}

	public int NpcRemoveInMadness0Index
	{
		get
		{
			return npcRemoveInMadness0Index;
		}
		set
		{
			npcRemoveInMadness0Index = value;
		}
	}

	public bool IsRift
	{
		get
		{
			return isRift;
		}
		set
		{
			isRift = value;
		}
	}

	public bool RandomizeNpcPosition
	{
		get
		{
			return randomizeNpcPosition;
		}
		set
		{
			randomizeNpcPosition = value;
		}
	}

	public NPCData NpcToSummonOnNpcKilled
	{
		get
		{
			NPCData nPCData = npcToSummonOnNpcKilled;
			if (nPCData == null)
			{
				return null;
			}
			if (npcToSummonOnNpcKilled != null && AtOManager.Instance.PlayerHasRequirement(Globals.Instance.GetRequirementData("_tier2")) && npcToSummonOnNpcKilled.UpgradedMob != null)
			{
				nPCData = npcToSummonOnNpcKilled.UpgradedMob;
			}
			if (npcToSummonOnNpcKilled != null && (AtOManager.Instance.GetNgPlus() > 0 || (!npcToSummonOnNpcKilled.IsNamed && !npcToSummonOnNpcKilled.IsBoss && AtOManager.Instance.IsChallengeTraitActive("toughermonsters")) || (npcToSummonOnNpcKilled.IsNamed && !npcToSummonOnNpcKilled.IsBoss && AtOManager.Instance.IsChallengeTraitActive("toughermonsters")) || (npcToSummonOnNpcKilled.IsBoss && AtOManager.Instance.IsChallengeTraitActive("hardcorebosses"))) && npcToSummonOnNpcKilled.NgPlusMob != null)
			{
				nPCData = npcToSummonOnNpcKilled.NgPlusMob;
			}
			if ((MadnessManager.Instance.IsMadnessTraitActive("despair") || AtOManager.Instance.IsChallengeTraitActive("despair")) && nPCData.HellModeMob != null)
			{
				nPCData = nPCData.HellModeMob;
			}
			return nPCData;
		}
		set
		{
			npcToSummonOnNpcKilled = value;
		}
	}

	public void DoStepSound()
	{
		if (stepSound == Enums.CombatStepSound.Walk_Stone)
		{
			GameManager.Instance.PlayAudio(AudioManager.Instance.walkStone);
		}
		else if (stepSound == Enums.CombatStepSound.Walk_Grass)
		{
			GameManager.Instance.PlayAudio(AudioManager.Instance.walkGrass);
		}
		else if (stepSound == Enums.CombatStepSound.Walk_Water)
		{
			GameManager.Instance.PlayAudio(AudioManager.Instance.walkWater);
		}
	}
}
