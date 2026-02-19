using UnityEngine;

[CreateAssetMenu(fileName = "New CardPlayerPack", menuName = "New CardPlayerPack", order = 64)]
public class CardPlayerPackData : ScriptableObject
{
	[SerializeField]
	private string packId;

	[Header("Pack Cards")]
	[SerializeField]
	private CardData card0;

	[SerializeField]
	private bool card0RandomBoon;

	[SerializeField]
	private bool card0RandomInjury;

	[SerializeField]
	private CardData card1;

	[SerializeField]
	private bool card1RandomBoon;

	[SerializeField]
	private bool card1RandomInjury;

	[SerializeField]
	private CardData card2;

	[SerializeField]
	private bool card2RandomBoon;

	[SerializeField]
	private bool card2RandomInjury;

	[SerializeField]
	private CardData card3;

	[SerializeField]
	private bool card3RandomBoon;

	[SerializeField]
	private bool card3RandomInjury;

	[Header("Difficulty +- X (Base = 10)")]
	[SerializeField]
	private int modSpeed;

	[SerializeField]
	private int modIterations;

	public string PackId
	{
		get
		{
			return packId;
		}
		set
		{
			packId = value;
		}
	}

	public CardData Card0
	{
		get
		{
			return card0;
		}
		set
		{
			card0 = value;
		}
	}

	public bool Card0RandomBoon
	{
		get
		{
			return card0RandomBoon;
		}
		set
		{
			card0RandomBoon = value;
		}
	}

	public bool Card0RandomInjury
	{
		get
		{
			return card0RandomInjury;
		}
		set
		{
			card0RandomInjury = value;
		}
	}

	public CardData Card1
	{
		get
		{
			return card1;
		}
		set
		{
			card1 = value;
		}
	}

	public bool Card1RandomBoon
	{
		get
		{
			return card1RandomBoon;
		}
		set
		{
			card1RandomBoon = value;
		}
	}

	public bool Card1RandomInjury
	{
		get
		{
			return card1RandomInjury;
		}
		set
		{
			card1RandomInjury = value;
		}
	}

	public CardData Card2
	{
		get
		{
			return card2;
		}
		set
		{
			card2 = value;
		}
	}

	public bool Card2RandomBoon
	{
		get
		{
			return card2RandomBoon;
		}
		set
		{
			card2RandomBoon = value;
		}
	}

	public bool Card2RandomInjury
	{
		get
		{
			return card2RandomInjury;
		}
		set
		{
			card2RandomInjury = value;
		}
	}

	public CardData Card3
	{
		get
		{
			return card3;
		}
		set
		{
			card3 = value;
		}
	}

	public bool Card3RandomBoon
	{
		get
		{
			return card3RandomBoon;
		}
		set
		{
			card3RandomBoon = value;
		}
	}

	public bool Card3RandomInjury
	{
		get
		{
			return card3RandomInjury;
		}
		set
		{
			card3RandomInjury = value;
		}
	}

	public int ModSpeed
	{
		get
		{
			return modSpeed;
		}
		set
		{
			modSpeed = value;
		}
	}

	public int ModIterations
	{
		get
		{
			return modIterations;
		}
		set
		{
			modIterations = value;
		}
	}
}
