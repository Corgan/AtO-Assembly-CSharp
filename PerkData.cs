using UnityEngine;

[CreateAssetMenu(fileName = "New Perk", menuName = "Perk Data", order = 57)]
public class PerkData : ScriptableObject
{
	[SerializeField]
	private string id = "";

	[Header("Attributes")]
	[SerializeField]
	private Enums.CardClass cardClass;

	[SerializeField]
	private bool mainPerk;

	[SerializeField]
	private bool obeliskPerk;

	[SerializeField]
	private int level;

	[SerializeField]
	private int row;

	[SerializeField]
	private Sprite icon;

	[SerializeField]
	private string iconTextValue;

	[SerializeField]
	private string customDescription;

	[Header("Currency")]
	[SerializeField]
	private int additionalCurrency;

	[SerializeField]
	private int additionalShards;

	[Header("Stat modification")]
	[SerializeField]
	private int maxHealth;

	[Header("Energy begin combat")]
	[SerializeField]
	private int energyBegin;

	[Header("Speed combat")]
	[SerializeField]
	private int speedQuantity;

	[Header("Damage value bonus")]
	[SerializeField]
	private Enums.DamageType damageFlatBonus;

	[SerializeField]
	private int damageFlatBonusValue;

	[Header("AuraCurse bonus")]
	[SerializeField]
	private AuraCurseData auracurseBonus;

	[SerializeField]
	private int auracurseBonusValue;

	[Header("Resist modification")]
	[SerializeField]
	private Enums.DamageType resistModified;

	[SerializeField]
	private int resistModifiedValue;

	[Header("Heal bonus")]
	[SerializeField]
	private int healQuantity;

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

	public int Level
	{
		get
		{
			return level;
		}
		set
		{
			level = value;
		}
	}

	public int Row
	{
		get
		{
			return row;
		}
		set
		{
			row = value;
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

	public int EnergyBegin
	{
		get
		{
			return energyBegin;
		}
		set
		{
			energyBegin = value;
		}
	}

	public AuraCurseData AuracurseBonus
	{
		get
		{
			return auracurseBonus;
		}
		set
		{
			auracurseBonus = value;
		}
	}

	public int AuracurseBonusValue
	{
		get
		{
			return auracurseBonusValue;
		}
		set
		{
			auracurseBonusValue = value;
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

	public int ResistModifiedValue
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

	public int SpeedQuantity
	{
		get
		{
			return speedQuantity;
		}
		set
		{
			speedQuantity = value;
		}
	}

	public Sprite Icon
	{
		get
		{
			return icon;
		}
		set
		{
			icon = value;
		}
	}

	public string IconTextValue
	{
		get
		{
			return iconTextValue;
		}
		set
		{
			iconTextValue = value;
		}
	}

	public int AdditionalCurrency
	{
		get
		{
			return additionalCurrency;
		}
		set
		{
			additionalCurrency = value;
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

	public bool ObeliskPerk
	{
		get
		{
			return obeliskPerk;
		}
		set
		{
			obeliskPerk = value;
		}
	}

	public bool MainPerk
	{
		get
		{
			return mainPerk;
		}
		set
		{
			mainPerk = value;
		}
	}

	public int AdditionalShards
	{
		get
		{
			return additionalShards;
		}
		set
		{
			additionalShards = value;
		}
	}

	public string CustomDescription
	{
		get
		{
			return customDescription;
		}
		set
		{
			customDescription = value;
		}
	}

	public void Init()
	{
		id = id.ToLower();
	}
}
