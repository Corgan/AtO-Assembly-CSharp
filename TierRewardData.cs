using UnityEngine;

[CreateAssetMenu(fileName = "Tier Reward", menuName = "Tier Reward Data", order = 61)]
public class TierRewardData : ScriptableObject
{
	[SerializeField]
	private int tierNum;

	[SerializeField]
	private int common;

	[SerializeField]
	private int uncommon;

	[SerializeField]
	private int rare;

	[SerializeField]
	private int epic;

	[SerializeField]
	private int mythic;

	[SerializeField]
	private int dust;

	public int TierNum
	{
		get
		{
			return tierNum;
		}
		set
		{
			tierNum = value;
		}
	}

	public int Common
	{
		get
		{
			return common;
		}
		set
		{
			common = value;
		}
	}

	public int Uncommon
	{
		get
		{
			return uncommon;
		}
		set
		{
			uncommon = value;
		}
	}

	public int Rare
	{
		get
		{
			return rare;
		}
		set
		{
			rare = value;
		}
	}

	public int Epic
	{
		get
		{
			return epic;
		}
		set
		{
			epic = value;
		}
	}

	public int Mythic
	{
		get
		{
			return mythic;
		}
		set
		{
			mythic = value;
		}
	}

	public int Dust
	{
		get
		{
			return dust;
		}
		set
		{
			dust = value;
		}
	}
}
