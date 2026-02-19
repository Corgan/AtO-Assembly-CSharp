using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Zone", menuName = "Zone Data", order = 59)]
public class ZoneData : ScriptableObject
{
	[SerializeField]
	private string zoneId;

	[SerializeField]
	private string zoneName;

	[SerializeField]
	private bool obeliskLow;

	[SerializeField]
	private bool obeliskHigh;

	[SerializeField]
	private bool obeliskFinal;

	[Header("DLC Requeriment")]
	[SerializeField]
	private string sku;

	[Header("Team Management")]
	[SerializeField]
	private bool changeTeamOnEntrance;

	[SerializeField]
	private List<SubClassData> newTeam;

	[SerializeField]
	private bool restoreTeamOnExit;

	[Header("Experience")]
	[SerializeField]
	private bool disableExperienceOnThisZone;

	[Header("Madness")]
	[SerializeField]
	private bool disableMadnessOnThisZone;

	[Header("CombatTool")]
	[SerializeField]
	public Sprite CombatBackground;

	public string ZoneId
	{
		get
		{
			return zoneId;
		}
		set
		{
			zoneId = value;
		}
	}

	public string ZoneName
	{
		get
		{
			return zoneName;
		}
		set
		{
			zoneName = value;
		}
	}

	public bool ObeliskLow
	{
		get
		{
			return obeliskLow;
		}
		set
		{
			obeliskLow = value;
		}
	}

	public bool ObeliskHigh
	{
		get
		{
			return obeliskHigh;
		}
		set
		{
			obeliskHigh = value;
		}
	}

	public bool ObeliskFinal
	{
		get
		{
			return obeliskFinal;
		}
		set
		{
			obeliskFinal = value;
		}
	}

	public bool ChangeTeamOnEntrance
	{
		get
		{
			return changeTeamOnEntrance;
		}
		set
		{
			changeTeamOnEntrance = value;
		}
	}

	public List<SubClassData> NewTeam
	{
		get
		{
			return newTeam;
		}
		set
		{
			newTeam = value;
		}
	}

	public bool RestoreTeamOnExit
	{
		get
		{
			return restoreTeamOnExit;
		}
		set
		{
			restoreTeamOnExit = value;
		}
	}

	public bool DisableExperienceOnThisZone
	{
		get
		{
			return disableExperienceOnThisZone;
		}
		set
		{
			disableExperienceOnThisZone = value;
		}
	}

	public bool DisableMadnessOnThisZone
	{
		get
		{
			return disableMadnessOnThisZone;
		}
		set
		{
			disableMadnessOnThisZone = value;
		}
	}

	public string Sku
	{
		get
		{
			return sku;
		}
		set
		{
			sku = value;
		}
	}
}
