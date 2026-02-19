using UnityEngine;

[CreateAssetMenu(fileName = "New MapNode", menuName = "MapNode Data", order = 60)]
public class NodeData : ScriptableObject
{
	[SerializeField]
	private string nodeId;

	[SerializeField]
	private string nodeName;

	private string sourceNodeName;

	[SerializeField]
	private ZoneData nodeZone;

	[TextArea]
	[SerializeField]
	private string description;

	[Header("Node background")]
	[SerializeField]
	private Sprite nodeBackgroundImg;

	[Header("Percent to appear in map")]
	[SerializeField]
	private int existsPercent = 100;

	[SerializeField]
	private string existsSku = "";

	[Header("It's a destination for a zone/portal travel")]
	[SerializeField]
	private bool travelDestination;

	[Header("Node that opens Town")]
	[SerializeField]
	private bool goToTown;

	[Header("Combat/Event percent")]
	[SerializeField]
	private int combatPercent;

	[SerializeField]
	private int eventPercent;

	[Header("Combat")]
	[SerializeField]
	private CombatData[] nodeCombat;

	[SerializeField]
	private Enums.CombatTier nodeCombatTier;

	[Header("Event and priority (lowest == highest priority)")]
	[SerializeField]
	private EventData[] nodeEvent;

	[SerializeField]
	private Enums.CombatTier nodeEventTier;

	[SerializeField]
	private int[] nodeEventPriority;

	[SerializeField]
	private int[] nodeEventPercent;

	[Header("Connections")]
	[SerializeField]
	private NodeData[] nodesConnected;

	[SerializeField]
	private NodesConnectedRequirement[] nodesConnectedRequirement;

	[Header("Node Requirements")]
	[SerializeField]
	private EventRequirementData nodeRequirement;

	[Header("Show the node even if the requirement is not fulfilled")]
	[SerializeField]
	private bool visibleIfNotRequirement;

	[Header("Node misc")]
	[SerializeField]
	private bool disableCorruption;

	[SerializeField]
	private bool disableRandom;

	[Header("Node Ground")]
	[SerializeField]
	private Enums.NodeGround nodeGround;

	public string NodeId
	{
		get
		{
			return nodeId;
		}
		set
		{
			nodeId = value;
		}
	}

	public string NodeName
	{
		get
		{
			return nodeName;
		}
		set
		{
			nodeName = value;
		}
	}

	public string Description
	{
		get
		{
			return description;
		}
		set
		{
			description = value;
		}
	}

	public NodeData[] NodesConnected
	{
		get
		{
			return nodesConnected;
		}
		set
		{
			nodesConnected = value;
		}
	}

	public CombatData[] NodeCombat
	{
		get
		{
			return nodeCombat;
		}
		set
		{
			nodeCombat = value;
		}
	}

	public EventData[] NodeEvent
	{
		get
		{
			return nodeEvent;
		}
		set
		{
			nodeEvent = value;
		}
	}

	public int[] NodeEventPriority
	{
		get
		{
			return nodeEventPriority;
		}
		set
		{
			nodeEventPriority = value;
		}
	}

	public EventRequirementData NodeRequirement
	{
		get
		{
			return nodeRequirement;
		}
		set
		{
			nodeRequirement = value;
		}
	}

	public bool VisibleIfNotRequirement
	{
		get
		{
			return visibleIfNotRequirement;
		}
		set
		{
			visibleIfNotRequirement = value;
		}
	}

	public NodesConnectedRequirement[] NodesConnectedRequirement
	{
		get
		{
			return nodesConnectedRequirement;
		}
		set
		{
			nodesConnectedRequirement = value;
		}
	}

	public ZoneData NodeZone
	{
		get
		{
			return nodeZone;
		}
		set
		{
			nodeZone = value;
		}
	}

	public bool GoToTown
	{
		get
		{
			return goToTown;
		}
		set
		{
			goToTown = value;
		}
	}

	public int CombatPercent
	{
		get
		{
			return combatPercent;
		}
		set
		{
			combatPercent = value;
		}
	}

	public int EventPercent
	{
		get
		{
			return eventPercent;
		}
		set
		{
			eventPercent = value;
		}
	}

	public int ExistsPercent
	{
		get
		{
			return existsPercent;
		}
		set
		{
			existsPercent = value;
		}
	}

	public int[] NodeEventPercent
	{
		get
		{
			return nodeEventPercent;
		}
		set
		{
			nodeEventPercent = value;
		}
	}

	public bool TravelDestination
	{
		get
		{
			return travelDestination;
		}
		set
		{
			travelDestination = value;
		}
	}

	public Enums.CombatTier NodeCombatTier
	{
		get
		{
			return nodeCombatTier;
		}
		set
		{
			nodeCombatTier = value;
		}
	}

	public Enums.CombatTier NodeEventTier
	{
		get
		{
			return nodeEventTier;
		}
		set
		{
			nodeEventTier = value;
		}
	}

	public Sprite NodeBackgroundImg
	{
		get
		{
			return nodeBackgroundImg;
		}
		set
		{
			nodeBackgroundImg = value;
		}
	}

	public bool DisableCorruption
	{
		get
		{
			return disableCorruption;
		}
		set
		{
			disableCorruption = value;
		}
	}

	public bool DisableRandom
	{
		get
		{
			return disableRandom;
		}
		set
		{
			disableRandom = value;
		}
	}

	public Enums.NodeGround NodeGround
	{
		get
		{
			return nodeGround;
		}
		set
		{
			nodeGround = value;
		}
	}

	public string ExistsSku
	{
		get
		{
			return existsSku;
		}
		set
		{
			existsSku = value;
		}
	}

	public string SourceNodeName
	{
		get
		{
			return sourceNodeName;
		}
		set
		{
			sourceNodeName = value;
		}
	}
}
