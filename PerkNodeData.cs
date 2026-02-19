using UnityEngine;

[CreateAssetMenu(fileName = "New Perk Node", menuName = "Perk Node Data", order = 57)]
public class PerkNodeData : ScriptableObject
{
	[SerializeField]
	private string id = "";

	[SerializeField]
	private int type;

	[SerializeField]
	private Sprite sprite;

	[SerializeField]
	private int column;

	[SerializeField]
	private int row;

	[SerializeField]
	private bool lockedInTown;

	[SerializeField]
	private bool notStack;

	[SerializeField]
	private Enums.PerkCost cost;

	[SerializeField]
	private PerkData perk;

	[SerializeField]
	private PerkNodeData perkRequired;

	[SerializeField]
	private PerkNodeData[] perksConnected;

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

	public int Type
	{
		get
		{
			return type;
		}
		set
		{
			type = value;
		}
	}

	public int Column
	{
		get
		{
			return column;
		}
		set
		{
			column = value;
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

	public PerkNodeData PerkRequired
	{
		get
		{
			return perkRequired;
		}
		set
		{
			perkRequired = value;
		}
	}

	public PerkData Perk
	{
		get
		{
			return perk;
		}
		set
		{
			perk = value;
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

	public Enums.PerkCost Cost
	{
		get
		{
			return cost;
		}
		set
		{
			cost = value;
		}
	}

	public PerkNodeData[] PerksConnected
	{
		get
		{
			return perksConnected;
		}
		set
		{
			perksConnected = value;
		}
	}

	public bool LockedInTown
	{
		get
		{
			return lockedInTown;
		}
		set
		{
			lockedInTown = value;
		}
	}

	public bool NotStack
	{
		get
		{
			return notStack;
		}
		set
		{
			notStack = value;
		}
	}
}
