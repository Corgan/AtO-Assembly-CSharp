using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using WebSocketSharp;

[CreateAssetMenu(fileName = "New ChallengeData", menuName = "New ChallengeData", order = 64)]
public class ChallengeData : ScriptableObject
{
	[SerializeField]
	private string id;

	[SerializeField]
	private string idSteam;

	[SerializeField]
	private int week;

	[SerializeField]
	private string seed;

	[Header("Weekly dates")]
	[Tooltip("dd-mm-yyyy")]
	[SerializeField]
	private string fromDay;

	[Tooltip("hh:mm")]
	[SerializeField]
	private string fromHour;

	[Tooltip("dd-mm-yyyy")]
	[SerializeField]
	private string toDay;

	[Tooltip("hh:mm")]
	[SerializeField]
	private string toHour;

	[Header("Heroes")]
	[SerializeField]
	private SubClassData hero1;

	[SerializeField]
	private SubClassData hero2;

	[SerializeField]
	private SubClassData hero3;

	[SerializeField]
	private SubClassData hero4;

	[Header("Boss")]
	[SerializeField]
	private NPCData boss1;

	[SerializeField]
	private NPCData boss2;

	[SerializeField]
	private CombatData bossCombat;

	[Header("Loot")]
	[SerializeField]
	private LootData loot;

	[Header("Traits")]
	[SerializeField]
	private List<ChallengeTrait> traits;

	[Header("Corruptions")]
	[SerializeField]
	private List<CardData> corruptionList;

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

	public string IdSteam
	{
		get
		{
			return idSteam;
		}
		set
		{
			idSteam = value;
		}
	}

	public int Week
	{
		get
		{
			return week;
		}
		set
		{
			week = value;
		}
	}

	public string Seed
	{
		get
		{
			return seed;
		}
		set
		{
			seed = value;
		}
	}

	public SubClassData Hero1
	{
		get
		{
			return hero1;
		}
		set
		{
			hero1 = value;
		}
	}

	public SubClassData Hero2
	{
		get
		{
			return hero2;
		}
		set
		{
			hero2 = value;
		}
	}

	public SubClassData Hero3
	{
		get
		{
			return hero3;
		}
		set
		{
			hero3 = value;
		}
	}

	public SubClassData Hero4
	{
		get
		{
			return hero4;
		}
		set
		{
			hero4 = value;
		}
	}

	public List<ChallengeTrait> Traits
	{
		get
		{
			return traits;
		}
		set
		{
			traits = value;
		}
	}

	public LootData Loot
	{
		get
		{
			return loot;
		}
		set
		{
			loot = value;
		}
	}

	public List<CardData> CorruptionList
	{
		get
		{
			return corruptionList;
		}
		set
		{
			corruptionList = value;
		}
	}

	public NPCData Boss1
	{
		get
		{
			return boss1;
		}
		set
		{
			boss1 = value;
		}
	}

	public NPCData Boss2
	{
		get
		{
			return boss2;
		}
		set
		{
			boss2 = value;
		}
	}

	public CombatData BossCombat
	{
		get
		{
			return bossCombat;
		}
		set
		{
			bossCombat = value;
		}
	}

	public DateTime GetDateFrom()
	{
		if (fromDay == "" || fromHour == "")
		{
			return DateTime.Now.AddYears(1);
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(fromDay);
		stringBuilder.Append(" ");
		stringBuilder.Append(fromHour);
		return DateTime.ParseExact(stringBuilder.ToString(), "dd-MM-yyyy HH:mm", null);
	}

	public DateTime GetDateTo()
	{
		if (toDay == "" || toHour == "")
		{
			return DateTime.Now.AddYears(-1);
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(toDay);
		stringBuilder.Append(" ");
		stringBuilder.Append(toHour);
		return DateTime.ParseExact(stringBuilder.ToString(), "dd-MM-yyyy HH:mm", null);
	}

	public CardbackData GetCardbackData()
	{
		CardbackData result = null;
		if (idSteam != "")
		{
			foreach (KeyValuePair<string, CardbackData> item in Globals.Instance.CardbackDataSource)
			{
				if (item.Value.SteamStat == idSteam)
				{
					result = item.Value;
					break;
				}
			}
		}
		return result;
	}

	public bool IsDateFixed()
	{
		if (!fromDay.IsNullOrEmpty() && !fromHour.IsNullOrEmpty() && !toHour.IsNullOrEmpty() && !toDay.IsNullOrEmpty())
		{
			return true;
		}
		return false;
	}
}
