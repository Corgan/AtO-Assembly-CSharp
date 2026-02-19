using UnityEngine;

[CreateAssetMenu(fileName = "New Thermometer", menuName = "Thermometer Data", order = 69)]
public class ThermometerData : ScriptableObject
{
	[SerializeField]
	private string thermometerId;

	[SerializeField]
	private Color thermometerColor;

	[SerializeField]
	private float goldBonus;

	[SerializeField]
	private float expBonus;

	[SerializeField]
	private int cardBonus;

	[SerializeField]
	private bool cardReward;

	[SerializeField]
	private int expertiseBonus;

	[SerializeField]
	private int uiGold;

	[SerializeField]
	private int uiExp;

	[SerializeField]
	private int uiCard;

	public string ThermometerId
	{
		get
		{
			return thermometerId;
		}
		set
		{
			thermometerId = value;
		}
	}

	public float GoldBonus
	{
		get
		{
			return goldBonus;
		}
		set
		{
			goldBonus = value;
		}
	}

	public float ExpBonus
	{
		get
		{
			return expBonus;
		}
		set
		{
			expBonus = value;
		}
	}

	public int CardBonus
	{
		get
		{
			return cardBonus;
		}
		set
		{
			cardBonus = value;
		}
	}

	public int ExpertiseBonus
	{
		get
		{
			return expertiseBonus;
		}
		set
		{
			expertiseBonus = value;
		}
	}

	public Color ThermometerColor
	{
		get
		{
			return thermometerColor;
		}
		set
		{
			thermometerColor = value;
		}
	}

	public bool CardReward
	{
		get
		{
			return cardReward;
		}
		set
		{
			cardReward = value;
		}
	}

	public int UiGold
	{
		get
		{
			return uiGold;
		}
		set
		{
			uiGold = value;
		}
	}

	public int UiExp
	{
		get
		{
			return uiExp;
		}
		set
		{
			uiExp = value;
		}
	}

	public int UiCard
	{
		get
		{
			return uiCard;
		}
		set
		{
			uiCard = value;
		}
	}
}
