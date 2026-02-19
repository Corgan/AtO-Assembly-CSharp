using UnityEngine;

[CreateAssetMenu(fileName = "New ThermometerTier", menuName = "ThermometerTier Data", order = 69)]
public class ThermometerTierData : ScriptableObject
{
	[SerializeField]
	private string thermometerTierId;

	[SerializeField]
	private RoundThermometer[] roundThermometer;

	public string ThermometerTierId
	{
		get
		{
			return thermometerTierId;
		}
		set
		{
			thermometerTierId = value;
		}
	}

	public RoundThermometer[] RoundThermometer
	{
		get
		{
			return roundThermometer;
		}
		set
		{
			roundThermometer = value;
		}
	}
}
