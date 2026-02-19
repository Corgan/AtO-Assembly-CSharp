using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Corruption CardPack", menuName = "New Corruption CardPack", order = 65)]
public class CorruptionPackData : ScriptableObject
{
	[Header("Name and class")]
	[SerializeField]
	private string packName;

	[SerializeField]
	private Enums.CardClass packClass;

	[SerializeField]
	private int packTier;

	[SerializeField]
	private List<CardData> lowPack;

	[SerializeField]
	private List<CardData> highPack;

	public string PackName
	{
		get
		{
			return packName;
		}
		set
		{
			packName = value;
		}
	}

	public Enums.CardClass PackClass
	{
		get
		{
			return packClass;
		}
		set
		{
			packClass = value;
		}
	}

	public int PackTier
	{
		get
		{
			return packTier;
		}
		set
		{
			packTier = value;
		}
	}

	public List<CardData> LowPack
	{
		get
		{
			return lowPack;
		}
		set
		{
			lowPack = value;
		}
	}

	public List<CardData> HighPack
	{
		get
		{
			return highPack;
		}
		set
		{
			highPack = value;
		}
	}
}
