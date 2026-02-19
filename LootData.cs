using UnityEngine;

[CreateAssetMenu(fileName = "New Loot Table", menuName = "Loot Table Data", order = 57)]
public class LootData : ScriptableObject
{
	[SerializeField]
	private string id;

	[SerializeField]
	private bool allowDropOnlyItems;

	[SerializeField]
	private int numItems;

	[SerializeField]
	private int goldQuantity;

	[SerializeField]
	private LootItem[] lootItemTable;

	[Header("Upgrade Percents")]
	[SerializeField]
	private float defaultPercentUncommon;

	[SerializeField]
	private float defaultPercentRare;

	[SerializeField]
	private float defaultPercentEpic;

	[SerializeField]
	private float defaultPercentMythic;

	[Header("Shady Deal")]
	[SerializeField]
	private GameObject shadyModel;

	[SerializeField]
	private float shadyScaleX = 1f;

	[SerializeField]
	private float shadyScaleY = 1f;

	[SerializeField]
	private float shadyOffsetX;

	[SerializeField]
	private float shadyOffsetY;

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

	public int NumItems
	{
		get
		{
			return numItems;
		}
		set
		{
			numItems = value;
		}
	}

	public LootItem[] LootItemTable
	{
		get
		{
			return lootItemTable;
		}
		set
		{
			lootItemTable = value;
		}
	}

	public float DefaultPercentUncommon
	{
		get
		{
			return defaultPercentUncommon;
		}
		set
		{
			defaultPercentUncommon = value;
		}
	}

	public float DefaultPercentRare
	{
		get
		{
			return defaultPercentRare;
		}
		set
		{
			defaultPercentRare = value;
		}
	}

	public float DefaultPercentEpic
	{
		get
		{
			return defaultPercentEpic;
		}
		set
		{
			defaultPercentEpic = value;
		}
	}

	public float DefaultPercentMythic
	{
		get
		{
			return defaultPercentMythic;
		}
		set
		{
			defaultPercentMythic = value;
		}
	}

	public int GoldQuantity
	{
		get
		{
			return goldQuantity;
		}
		set
		{
			goldQuantity = value;
		}
	}

	public GameObject ShadyModel
	{
		get
		{
			return shadyModel;
		}
		set
		{
			shadyModel = value;
		}
	}

	public float ShadyScaleX
	{
		get
		{
			return shadyScaleX;
		}
		set
		{
			shadyScaleX = value;
		}
	}

	public float ShadyScaleY
	{
		get
		{
			return shadyScaleY;
		}
		set
		{
			shadyScaleY = value;
		}
	}

	public float ShadyOffsetX
	{
		get
		{
			return shadyOffsetX;
		}
		set
		{
			shadyOffsetX = value;
		}
	}

	public float ShadyOffsetY
	{
		get
		{
			return shadyOffsetY;
		}
		set
		{
			shadyOffsetY = value;
		}
	}

	public bool AllowDropOnlyItems
	{
		get
		{
			return allowDropOnlyItems;
		}
		set
		{
			allowDropOnlyItems = value;
		}
	}
}
