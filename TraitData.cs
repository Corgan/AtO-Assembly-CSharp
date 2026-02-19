using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trait", menuName = "Trait Data", order = 58)]
public class TraitData : ScriptableObject
{
	[SerializeField]
	private string traitName;

	[SerializeField]
	private string id;

	[TextArea]
	[SerializeField]
	private string description;

	[SerializeField]
	private Enums.EventActivation activation;

	[SerializeField]
	private bool activateOnRuneTypeAdded;

	[SerializeField]
	private bool tryActivateOnEveryEvent;

	[SerializeField]
	private int timesPerTurn;

	[SerializeField]
	private int timesPerRound;

	[Header("Card reward")]
	[SerializeField]
	private CardData traitCard;

	[Header("Card reward for all heroes")]
	[SerializeField]
	private CardData traitCardForAllHeroes;

	[Header("Character stat modification")]
	[SerializeField]
	private Enums.CharacterStat characterStatModified;

	[SerializeField]
	private int characterStatModifiedValue;

	[Header("Resist modification")]
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

	[Header("AuraCurse immunity")]
	[SerializeField]
	private string auracurseImmune1 = "";

	[SerializeField]
	private string auracurseImmune2 = "";

	[SerializeField]
	private string auracurseImmune3 = "";

	[SerializeField]
	private int maxBleedDamagePerTurn = -1;

	[Header("AuraCurse bonus")]
	[SerializeField]
	private AuraCurseData auracurseBonus1;

	[SerializeField]
	private int auracurseBonusValue1;

	[SerializeField]
	private AuraCurseData auracurseBonus2;

	[SerializeField]
	private int auracurseBonusValue2;

	[SerializeField]
	private AuraCurseData auracurseBonus3;

	[SerializeField]
	private int auracurseBonusValue3;

	[Header("Heal bonus")]
	[SerializeField]
	private int healFlatBonus;

	[SerializeField]
	private float healPercentBonus;

	[SerializeField]
	private int healReceivedFlatBonus;

	[SerializeField]
	private float healReceivedPercentBonus;

	[Header("Damage value bonus")]
	[SerializeField]
	private Enums.DamageType damageBonusFlat;

	[SerializeField]
	private int damageBonusFlatValue;

	[SerializeField]
	private Enums.DamageType damageBonusFlat2;

	[SerializeField]
	private int damageBonusFlatValue2;

	[SerializeField]
	private Enums.DamageType damageBonusFlat3;

	[SerializeField]
	private int damageBonusFlatValue3;

	[SerializeField]
	private Enums.DamageType damageBonusPercent;

	[SerializeField]
	private float damageBonusPercentValue;

	[SerializeField]
	private Enums.DamageType damageBonusPercent2;

	[SerializeField]
	private float damageBonusPercentValue2;

	[SerializeField]
	private Enums.DamageType damageBonusPercent3;

	[SerializeField]
	private float damageBonusPercentValue3;

	[Header("Keynotes")]
	[SerializeField]
	private List<KeyNotesData> keyNotes;

	public string TraitName
	{
		get
		{
			return traitName;
		}
		set
		{
			traitName = value;
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

	public string AuracurseImmune1
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

	public string AuracurseImmune2
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

	public string AuracurseImmune3
	{
		get
		{
			return auracurseImmune3;
		}
		set
		{
			auracurseImmune3 = value;
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

	public Enums.DamageType DamageBonusFlat
	{
		get
		{
			return damageBonusFlat;
		}
		set
		{
			damageBonusFlat = value;
		}
	}

	public int DamageBonusFlatValue
	{
		get
		{
			return damageBonusFlatValue;
		}
		set
		{
			damageBonusFlatValue = value;
		}
	}

	public Enums.DamageType DamageBonusFlat2
	{
		get
		{
			return damageBonusFlat2;
		}
		set
		{
			damageBonusFlat2 = value;
		}
	}

	public int DamageBonusFlatValue2
	{
		get
		{
			return damageBonusFlatValue2;
		}
		set
		{
			damageBonusFlatValue2 = value;
		}
	}

	public Enums.DamageType DamageBonusFlat3
	{
		get
		{
			return damageBonusFlat3;
		}
		set
		{
			damageBonusFlat3 = value;
		}
	}

	public int DamageBonusFlatValue3
	{
		get
		{
			return damageBonusFlatValue3;
		}
		set
		{
			damageBonusFlatValue3 = value;
		}
	}

	public Enums.DamageType DamageBonusPercent
	{
		get
		{
			return damageBonusPercent;
		}
		set
		{
			damageBonusPercent = value;
		}
	}

	public float DamageBonusPercentValue
	{
		get
		{
			return damageBonusPercentValue;
		}
		set
		{
			damageBonusPercentValue = value;
		}
	}

	public Enums.DamageType DamageBonusPercent2
	{
		get
		{
			return damageBonusPercent2;
		}
		set
		{
			damageBonusPercent2 = value;
		}
	}

	public float DamageBonusPercentValue2
	{
		get
		{
			return damageBonusPercentValue2;
		}
		set
		{
			damageBonusPercentValue2 = value;
		}
	}

	public Enums.DamageType DamageBonusPercent3
	{
		get
		{
			return damageBonusPercent3;
		}
		set
		{
			damageBonusPercent3 = value;
		}
	}

	public float DamageBonusPercentValue3
	{
		get
		{
			return damageBonusPercentValue3;
		}
		set
		{
			damageBonusPercentValue3 = value;
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

	public AuraCurseData AuracurseBonus3
	{
		get
		{
			return auracurseBonus3;
		}
		set
		{
			auracurseBonus3 = value;
		}
	}

	public int AuracurseBonusValue3
	{
		get
		{
			return auracurseBonusValue3;
		}
		set
		{
			auracurseBonusValue3 = value;
		}
	}

	public CardData TraitCard
	{
		get
		{
			return traitCard;
		}
		set
		{
			traitCard = value;
		}
	}

	public CardData TraitCardForAllHeroes
	{
		get
		{
			return traitCardForAllHeroes;
		}
		set
		{
			traitCardForAllHeroes = value;
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

	public int TimesPerRound
	{
		get
		{
			return timesPerRound;
		}
		set
		{
			timesPerRound = value;
		}
	}

	public bool ActivateOnRuneTypeAdded
	{
		get
		{
			return activateOnRuneTypeAdded;
		}
		set
		{
			activateOnRuneTypeAdded = value;
		}
	}

	public bool TryActivateOnEveryEvent
	{
		get
		{
			return tryActivateOnEveryEvent;
		}
		set
		{
			tryActivateOnEveryEvent = value;
		}
	}

	public int MaxBleedDamagePerTurn
	{
		get
		{
			return maxBleedDamagePerTurn;
		}
		set
		{
			maxBleedDamagePerTurn = value;
		}
	}

	public void Init()
	{
		id = Regex.Replace(traitName, "[\\s']+", "").ToLower();
	}

	public void SetNameAndDescription()
	{
		string text = Texts.Instance.GetText(id, "Traits");
		if (text != "")
		{
			traitName = text;
		}
		string text2 = Texts.Instance.GetText(id + "_description", "Traits");
		if (text2 != "")
		{
			description = text2;
		}
	}
}
