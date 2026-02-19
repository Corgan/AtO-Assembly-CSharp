using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CardPack", menuName = "New CardPack", order = 64)]
public class PackData : ScriptableObject
{
	[SerializeField]
	private string packId;

	[Header("Name and class")]
	[SerializeField]
	private string packName;

	[SerializeField]
	private SubClassData requiredClass;

	[SerializeField]
	private Enums.CardClass packClass;

	[Header("Pack Cards")]
	[SerializeField]
	private CardData card0;

	[SerializeField]
	private CardData card1;

	[SerializeField]
	private CardData card2;

	[SerializeField]
	private CardData card3;

	[SerializeField]
	private CardData card4;

	[SerializeField]
	private CardData card5;

	[Header("Special Cards")]
	[SerializeField]
	private CardData cardSpecial0;

	[SerializeField]
	private CardData cardSpecial1;

	[Header("Perks")]
	[SerializeField]
	private List<PerkData> perkList;

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

	public string PackName
	{
		get
		{
			return packName;
		}
		set
		{
			packName = value;
		}
	}

	public SubClassData RequiredClass
	{
		get
		{
			return requiredClass;
		}
		set
		{
			requiredClass = value;
		}
	}

	public Enums.CardClass PackClass
	{
		get
		{
			return packClass;
		}
		set
		{
			packClass = value;
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

	public CardData Card4
	{
		get
		{
			return card4;
		}
		set
		{
			card4 = value;
		}
	}

	public CardData Card5
	{
		get
		{
			return card5;
		}
		set
		{
			card5 = value;
		}
	}

	public CardData CardSpecial0
	{
		get
		{
			return cardSpecial0;
		}
		set
		{
			cardSpecial0 = value;
		}
	}

	public CardData CardSpecial1
	{
		get
		{
			return cardSpecial1;
		}
		set
		{
			cardSpecial1 = value;
		}
	}

	public List<PerkData> PerkList
	{
		get
		{
			return perkList;
		}
		set
		{
			perkList = value;
		}
	}
}
