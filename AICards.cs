using System;
using UnityEngine;

[Serializable]
public class AICards
{
	[SerializeField]
	private CardData card;

	[SerializeField]
	private int unitsInDeck;

	[SerializeField]
	private int addCardRound;

	[SerializeField]
	private int _startsAtObeliskMadnessLevel;

	[SerializeField]
	private int _startsAtSingularityMadnessLevel;

	[Header("Order of cast (priority: less is first) ")]
	[SerializeField]
	private int priority;

	[SerializeField]
	private float percentToCast;

	[Header("Cast card only if")]
	[SerializeField]
	private Enums.OnlyCastIf onlyCastIf;

	[SerializeField]
	private float valueCastIf;

	[SerializeField]
	private string _specialSecondTargetID;

	[SerializeField]
	private Enums.OnlyCastIf _secondOnlyCastIf;

	[SerializeField]
	private float _secondValueCastIf;

	[SerializeField]
	private AuraCurseData auracurseCastIf;

	[Header("If you can cast, choose among the possible targets")]
	[SerializeField]
	private Enums.TargetCast targetCast;

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

	public int StartsAtObeliskMadnessLevel => _startsAtObeliskMadnessLevel;

	public int StartsAtSingularityMadnessLevel => _startsAtSingularityMadnessLevel;

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

	public int AddCardRound
	{
		get
		{
			return addCardRound;
		}
		set
		{
			addCardRound = value;
		}
	}

	public int Priority
	{
		get
		{
			return priority;
		}
		set
		{
			priority = value;
		}
	}

	public float PercentToCast
	{
		get
		{
			return percentToCast;
		}
		set
		{
			percentToCast = value;
		}
	}

	public Enums.OnlyCastIf OnlyCastIf
	{
		get
		{
			return onlyCastIf;
		}
		set
		{
			onlyCastIf = value;
		}
	}

	public float ValueCastIf
	{
		get
		{
			return valueCastIf;
		}
		set
		{
			valueCastIf = value;
		}
	}

	public string SpecialSecondTargetID => _specialSecondTargetID;

	public Enums.OnlyCastIf SecondOnlyCastIf
	{
		get
		{
			return _secondOnlyCastIf;
		}
		set
		{
			_secondOnlyCastIf = value;
		}
	}

	public float SecondValueCastIf
	{
		get
		{
			return _secondValueCastIf;
		}
		set
		{
			_secondValueCastIf = value;
		}
	}

	public AuraCurseData AuracurseCastIf
	{
		get
		{
			return auracurseCastIf;
		}
		set
		{
			auracurseCastIf = value;
		}
	}

	public Enums.TargetCast TargetCast
	{
		get
		{
			return targetCast;
		}
		set
		{
			targetCast = value;
		}
	}
}
