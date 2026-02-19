using UnityEngine;

[CreateAssetMenu(fileName = "New Cardback Data", menuName = "Cardback Data", order = 64)]
public class CardbackData : ScriptableObject
{
	[SerializeField]
	private string cardbackId;

	[SerializeField]
	private string cardbackName;

	[SerializeField]
	private SubClassData cardbackSubclass;

	[SerializeField]
	private bool locked;

	[SerializeField]
	private bool showIfLocked;

	[Header("Base cardback for characters")]
	[SerializeField]
	private bool baseCardback;

	[Header("Order in charpopup")]
	[SerializeField]
	private int cardbackOrder = 1000;

	[Header("Rank Requeriment")]
	[SerializeField]
	private int rankLevel;

	[Header("DLC Requeriment")]
	[SerializeField]
	private string sku;

	[Header("Steam Stat Requeriment")]
	[SerializeField]
	private string steamStat;

	[Header("Adventure Level Requeriment")]
	[SerializeField]
	private int adventureLevel;

	[Header("Obelisk Level Requeriment")]
	[SerializeField]
	private int obeliskLevel;

	[Header("Singularity Level Requeriment")]
	[SerializeField]
	private int singularityLevel;

	[Header("PDX Account Required")]
	[SerializeField]
	private bool pdxAccountRequired;

	[Header("Sprites")]
	[SerializeField]
	private Sprite cardbackSprite;

	[Header("Text Id")]
	[SerializeField]
	private string cardbackTextId;

	public string CardbackId
	{
		get
		{
			return cardbackId;
		}
		set
		{
			cardbackId = value;
		}
	}

	public string CardbackName
	{
		get
		{
			return cardbackName;
		}
		set
		{
			cardbackName = value;
		}
	}

	public SubClassData CardbackSubclass
	{
		get
		{
			return cardbackSubclass;
		}
		set
		{
			cardbackSubclass = value;
		}
	}

	public bool BaseCardback
	{
		get
		{
			return baseCardback;
		}
		set
		{
			baseCardback = value;
		}
	}

	public int CardbackOrder
	{
		get
		{
			return cardbackOrder;
		}
		set
		{
			cardbackOrder = value;
		}
	}

	public int RankLevel
	{
		get
		{
			return rankLevel;
		}
		set
		{
			rankLevel = value;
		}
	}

	public Sprite CardbackSprite
	{
		get
		{
			return cardbackSprite;
		}
		set
		{
			cardbackSprite = value;
		}
	}

	public bool ShowIfLocked
	{
		get
		{
			return showIfLocked;
		}
		set
		{
			showIfLocked = value;
		}
	}

	public bool Locked
	{
		get
		{
			return locked;
		}
		set
		{
			locked = value;
		}
	}

	public int AdventureLevel
	{
		get
		{
			return adventureLevel;
		}
		set
		{
			adventureLevel = value;
		}
	}

	public int ObeliskLevel
	{
		get
		{
			return obeliskLevel;
		}
		set
		{
			obeliskLevel = value;
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

	public string SteamStat
	{
		get
		{
			return steamStat;
		}
		set
		{
			steamStat = value;
		}
	}

	public bool PdxAccountRequired
	{
		get
		{
			return pdxAccountRequired;
		}
		set
		{
			pdxAccountRequired = value;
		}
	}

	public int SingularityLevel
	{
		get
		{
			return singularityLevel;
		}
		set
		{
			singularityLevel = value;
		}
	}

	public string CardbackTextId
	{
		get
		{
			return cardbackTextId;
		}
		set
		{
			cardbackTextId = value;
		}
	}
}
