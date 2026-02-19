using UnityEngine;

[CreateAssetMenu(fileName = "New Event Requirement", menuName = "Event Requirement Data", order = 63)]
public class EventRequirementData : ScriptableObject
{
	[SerializeField]
	private string requirementId;

	[SerializeField]
	private string requirementName;

	[TextArea]
	[SerializeField]
	private string description;

	[SerializeField]
	private bool assignToPlayerAtBegin;

	[SerializeField]
	private bool requirementTrack;

	[SerializeField]
	private Enums.Zone requirementZoneFinishTrack;

	[SerializeField]
	private Enums.Zone requirementZoneFinishTrackAlternateFinalAct = Enums.Zone.None;

	[SerializeField]
	private bool itemTrack;

	[SerializeField]
	private Sprite itemSprite;

	[SerializeField]
	private Sprite trackSprite;

	[SerializeField]
	private CardData trackCard;

	public string RequirementName
	{
		get
		{
			return requirementName;
		}
		set
		{
			requirementName = value;
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

	public string RequirementId
	{
		get
		{
			return requirementId;
		}
		set
		{
			requirementId = value;
		}
	}

	public bool RequirementTrack
	{
		get
		{
			return requirementTrack;
		}
		set
		{
			requirementTrack = value;
		}
	}

	public bool ItemTrack
	{
		get
		{
			return itemTrack;
		}
		set
		{
			itemTrack = value;
		}
	}

	public Sprite ItemSprite
	{
		get
		{
			return itemSprite;
		}
		set
		{
			itemSprite = value;
		}
	}

	public bool AssignToPlayerAtBegin
	{
		get
		{
			return assignToPlayerAtBegin;
		}
		set
		{
			assignToPlayerAtBegin = value;
		}
	}

	public Sprite TrackSprite
	{
		get
		{
			return trackSprite;
		}
		set
		{
			trackSprite = value;
		}
	}

	public CardData TrackCard
	{
		get
		{
			return trackCard;
		}
		set
		{
			trackCard = value;
		}
	}

	public Enums.Zone RequirementZoneFinishTrack
	{
		get
		{
			return requirementZoneFinishTrack;
		}
		set
		{
			requirementZoneFinishTrack = value;
		}
	}

	public bool CanShowRequeriment(Enums.Zone fromZone, Enums.Zone toZone)
	{
		if (requirementZoneFinishTrackAlternateFinalAct != Enums.Zone.None)
		{
			Enums.Zone zone = fromZone;
			if (zone == Enums.Zone.None)
			{
				zone = toZone;
			}
			if (zone == Enums.Zone.WitchWoods || zone == Enums.Zone.CastleCourtyard || zone == Enums.Zone.CastleSpire)
			{
				switch (requirementZoneFinishTrackAlternateFinalAct)
				{
				case Enums.Zone.WitchWoods:
					if (zone == Enums.Zone.CastleCourtyard || zone == Enums.Zone.CastleSpire)
					{
						return false;
					}
					break;
				case Enums.Zone.CastleCourtyard:
					if (zone == Enums.Zone.CastleSpire)
					{
						return false;
					}
					break;
				case Enums.Zone.CastleSpire:
					return true;
				}
			}
		}
		if (fromZone == Enums.Zone.SpiderLair)
		{
			fromZone = Enums.Zone.Aquarfall;
		}
		if (toZone == Enums.Zone.SpiderLair)
		{
			toZone = Enums.Zone.Aquarfall;
		}
		if (fromZone == Enums.Zone.Sectarium)
		{
			fromZone = Enums.Zone.Senenthia;
		}
		if (toZone == Enums.Zone.Sectarium)
		{
			toZone = Enums.Zone.Senenthia;
		}
		if (fromZone == Enums.Zone.FrozenSewers)
		{
			fromZone = Enums.Zone.Faeborg;
		}
		if (toZone == Enums.Zone.FrozenSewers)
		{
			toZone = Enums.Zone.Faeborg;
		}
		if (fromZone == Enums.Zone.BlackForge)
		{
			fromZone = Enums.Zone.Velkarath;
		}
		if (toZone == Enums.Zone.BlackForge)
		{
			toZone = Enums.Zone.Velkarath;
		}
		if (requirementZoneFinishTrack == Enums.Zone.Senenthia)
		{
			if (fromZone == Enums.Zone.Senenthia)
			{
				return true;
			}
			return false;
		}
		if (requirementZoneFinishTrack == Enums.Zone.VoidLow)
		{
			if (fromZone == Enums.Zone.VoidHigh || toZone == Enums.Zone.VoidHigh)
			{
				return false;
			}
			return true;
		}
		if (requirementZoneFinishTrack == Enums.Zone.VoidHigh)
		{
			return true;
		}
		if (requirementZoneFinishTrack != Enums.Zone.None)
		{
			if (AtOManager.Instance.GetActNumberForText() == 4)
			{
				return false;
			}
			if (AtOManager.Instance.GetActNumberForText() == 3)
			{
				if (toZone != Enums.Zone.None)
				{
					return false;
				}
				if (requirementZoneFinishTrack == fromZone)
				{
					return true;
				}
				return false;
			}
			if (AtOManager.Instance.GetActNumberForText() == 2)
			{
				if (requirementZoneFinishTrack != toZone && toZone != Enums.Zone.None)
				{
					return false;
				}
				return true;
			}
			if (requirementZoneFinishTrack != toZone && toZone != Enums.Zone.None)
			{
				return false;
			}
			return true;
		}
		return true;
	}
}
