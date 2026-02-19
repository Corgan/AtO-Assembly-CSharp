using UnityEngine;

[CreateAssetMenu(fileName = "Challenge Trait", menuName = "Challenge Trait", order = 64)]
public class ChallengeTrait : ScriptableObject
{
	[SerializeField]
	private string id;

	[SerializeField]
	private new string name;

	[SerializeField]
	private Sprite icon;

	[SerializeField]
	private bool isMadnessTrait;

	[SerializeField]
	private int order;

	[SerializeField]
	private bool isSingularityTrait;

	[SerializeField]
	private int orderSingularity;

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

	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	public int Order
	{
		get
		{
			return order;
		}
		set
		{
			order = value;
		}
	}

	public bool IsMadnessTrait
	{
		get
		{
			return isMadnessTrait;
		}
		set
		{
			isMadnessTrait = value;
		}
	}

	public Sprite Icon
	{
		get
		{
			return icon;
		}
		set
		{
			icon = value;
		}
	}

	public bool IsSingularityTrait
	{
		get
		{
			return isSingularityTrait;
		}
		set
		{
			isSingularityTrait = value;
		}
	}

	public int OrderSingularity
	{
		get
		{
			return orderSingularity;
		}
		set
		{
			orderSingularity = value;
		}
	}
}
