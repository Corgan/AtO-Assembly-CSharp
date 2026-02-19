using System;
using UnityEngine;

[Serializable]
public class Card
{
	[SerializeField]
	private CardData cardData;

	[SerializeField]
	private string id;

	public CardData CardData
	{
		get
		{
			return cardData;
		}
		set
		{
			cardData = value;
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

	public void InitData()
	{
		if (cardData != null)
		{
			id = cardData.Id;
		}
	}
}
