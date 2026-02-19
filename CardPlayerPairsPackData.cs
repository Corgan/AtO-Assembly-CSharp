using UnityEngine;

[CreateAssetMenu(fileName = "New CardPlayerPairsPack", menuName = "New CardPlayerPairsPack", order = 64)]
public class CardPlayerPairsPackData : ScriptableObject
{
	[SerializeField]
	private string packId;

	[Header("Pack Cards (will instance two cards for each)")]
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
}
