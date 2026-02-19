using UnityEngine;

[CreateAssetMenu(fileName = "New Cinematic", menuName = "Cinematic Data", order = 62)]
public class CinematicData : ScriptableObject
{
	[SerializeField]
	private string cinematicId;

	[SerializeField]
	private GameObject cinematicGo;

	[SerializeField]
	private AudioClip cinematicBSO;

	[SerializeField]
	private EventData cinematicEvent;

	[SerializeField]
	private CombatData cinematicCombat;

	[SerializeField]
	private bool cinematicEndAdventure;

	public string CinematicId
	{
		get
		{
			return cinematicId;
		}
		set
		{
			cinematicId = value;
		}
	}

	public GameObject CinematicGo
	{
		get
		{
			return cinematicGo;
		}
		set
		{
			cinematicGo = value;
		}
	}

	public EventData CinematicEvent
	{
		get
		{
			return cinematicEvent;
		}
		set
		{
			cinematicEvent = value;
		}
	}

	public CombatData CinematicCombat
	{
		get
		{
			return cinematicCombat;
		}
		set
		{
			cinematicCombat = value;
		}
	}

	public bool CinematicEndAdventure
	{
		get
		{
			return cinematicEndAdventure;
		}
		set
		{
			cinematicEndAdventure = value;
		}
	}

	public AudioClip CinematicBSO
	{
		get
		{
			return cinematicBSO;
		}
		set
		{
			cinematicBSO = value;
		}
	}
}
