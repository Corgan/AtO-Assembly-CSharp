using System;
using UnityEngine;

[Serializable]
public class LootItem
{
	[SerializeField]
	private CardData lootCard;

	[SerializeField]
	private float lootPercent;

	[SerializeField]
	private Enums.CardType lootType;

	[SerializeField]
	private Enums.CardRarity lootRarity;

	[SerializeField]
	private string lootMisc;

	public CardData LootCard
	{
		get
		{
			return lootCard;
		}
		set
		{
			lootCard = value;
		}
	}

	public float LootPercent
	{
		get
		{
			return lootPercent;
		}
		set
		{
			lootPercent = value;
		}
	}

	public Enums.CardType LootType
	{
		get
		{
			return lootType;
		}
		set
		{
			lootType = value;
		}
	}

	public Enums.CardRarity LootRarity
	{
		get
		{
			return lootRarity;
		}
		set
		{
			lootRarity = value;
		}
	}

	public string LootMisc
	{
		get
		{
			return lootMisc;
		}
		set
		{
			lootMisc = value;
		}
	}
}
