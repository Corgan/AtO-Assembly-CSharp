using System;
using UnityEngine;

[Serializable]
public class HeroCards
{
	[SerializeField]
	private CardData card;

	[SerializeField]
	private int unitsInDeck;

	public CardData Card
	{
		get
		{
			return card;
		}
		set
		{
			card = value;
		}
	}

	public int UnitsInDeck
	{
		get
		{
			return unitsInDeck;
		}
		set
		{
			unitsInDeck = value;
		}
	}
}
