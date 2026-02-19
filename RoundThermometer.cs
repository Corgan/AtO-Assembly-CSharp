using System;
using UnityEngine;

[Serializable]
public class RoundThermometer
{
	[SerializeField]
	private int round;

	[SerializeField]
	private ThermometerData thermometerData;

	public int Round
	{
		get
		{
			return round;
		}
		set
		{
			round = value;
		}
	}

	public ThermometerData ThermometerData
	{
		get
		{
			return thermometerData;
		}
		set
		{
			thermometerData = value;
		}
	}
}
