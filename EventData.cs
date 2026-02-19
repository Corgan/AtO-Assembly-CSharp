using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Event Data", order = 62)]
public class EventData : ScriptableObject
{
	[SerializeField]
	private string eventId;

	[SerializeField]
	private string eventUniqueId;

	[SerializeField]
	private string eventName;

	[SerializeField]
	private EventRequirementData requirement;

	[SerializeField]
	private SubClassData requiredClass;

	[TextArea]
	[SerializeField]
	private string description;

	[TextArea]
	[SerializeField]
	private string descriptionAction;

	[SerializeField]
	private Sprite eventSpriteMap;

	[SerializeField]
	private Sprite eventSpriteDecor;

	[SerializeField]
	private Enums.MapIconShader eventIconShader;

	[SerializeField]
	private Sprite eventSpriteBook;

	[SerializeField]
	private bool historyMode;

	[Header("Event Tier")]
	[SerializeField]
	private Enums.CombatTier eventTier;

	[Header("Reply List")]
	[SerializeField]
	private int replyRandom;

	[SerializeField]
	private EventReplyData[] replys;

	public string EventId
	{
		get
		{
			return eventId;
		}
		set
		{
			eventId = value;
		}
	}

	public string EventName
	{
		get
		{
			return eventName;
		}
		set
		{
			eventName = value;
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

	public string DescriptionAction
	{
		get
		{
			return descriptionAction;
		}
		set
		{
			descriptionAction = value;
		}
	}

	public Sprite EventSpriteMap
	{
		get
		{
			return eventSpriteMap;
		}
		set
		{
			eventSpriteMap = value;
		}
	}

	public Sprite EventSpriteBook
	{
		get
		{
			return eventSpriteBook;
		}
		set
		{
			eventSpriteBook = value;
		}
	}

	public bool HistoryMode
	{
		get
		{
			return historyMode;
		}
		set
		{
			historyMode = value;
		}
	}

	public EventReplyData[] Replys
	{
		get
		{
			return replys;
		}
		set
		{
			replys = value;
		}
	}

	public EventRequirementData Requirement
	{
		get
		{
			return requirement;
		}
		set
		{
			requirement = value;
		}
	}

	public SubClassData RequiredClass
	{
		get
		{
			return requiredClass;
		}
		set
		{
			requiredClass = value;
		}
	}

	public Enums.MapIconShader EventIconShader
	{
		get
		{
			return eventIconShader;
		}
		set
		{
			eventIconShader = value;
		}
	}

	public Enums.CombatTier EventTier
	{
		get
		{
			return eventTier;
		}
		set
		{
			eventTier = value;
		}
	}

	public Sprite EventSpriteDecor
	{
		get
		{
			return eventSpriteDecor;
		}
		set
		{
			eventSpriteDecor = value;
		}
	}

	public string EventUniqueId
	{
		get
		{
			return eventUniqueId;
		}
		set
		{
			eventUniqueId = value;
		}
	}

	public int ReplyRandom
	{
		get
		{
			return replyRandom;
		}
		set
		{
			replyRandom = value;
		}
	}

	public void Init()
	{
		List<EventReplyData> list = new List<EventReplyData>();
		List<string> list2 = new List<string>();
		for (int i = 0; i < replys.Length; i++)
		{
			EventReplyData eventReplyData = replys[i];
			if (eventReplyData != null && eventReplyData.RequiredClass != null)
			{
				list2.Add(eventReplyData.RequiredClass.Id);
			}
		}
		bool flag = false;
		for (int j = 0; j < replys.Length; j++)
		{
			EventReplyData eventReplyData2 = replys[j];
			if (eventReplyData2 == null)
			{
				continue;
			}
			if (eventReplyData2.RepeatForAllCharacters || eventReplyData2.RepeatForAllWarriors || eventReplyData2.RepeatForAllScouts || eventReplyData2.RepeatForAllMages || eventReplyData2.RepeatForAllHealers)
			{
				foreach (KeyValuePair<string, SubClassData> item in Globals.Instance.SubClass)
				{
					if (!(item.Value != null) || !item.Value.MainCharacter || (list2.Count > 0 && list2.Contains(item.Value.Id)))
					{
						continue;
					}
					flag = false;
					if (eventReplyData2.RepeatForAllCharacters)
					{
						flag = true;
					}
					else
					{
						if (eventReplyData2.RepeatForAllWarriors && (item.Value.HeroClass == Enums.HeroClass.Warrior || item.Value.HeroClassSecondary == Enums.HeroClass.Warrior))
						{
							flag = true;
						}
						if (eventReplyData2.RepeatForAllScouts && (item.Value.HeroClass == Enums.HeroClass.Scout || item.Value.HeroClassSecondary == Enums.HeroClass.Scout))
						{
							flag = true;
						}
						if (eventReplyData2.RepeatForAllMages && (item.Value.HeroClass == Enums.HeroClass.Mage || item.Value.HeroClassSecondary == Enums.HeroClass.Mage))
						{
							flag = true;
						}
						if (eventReplyData2.RepeatForAllHealers && (item.Value.HeroClass == Enums.HeroClass.Healer || item.Value.HeroClassSecondary == Enums.HeroClass.Healer))
						{
							flag = true;
						}
					}
					if (flag)
					{
						EventReplyData eventReplyData3 = eventReplyData2.ShallowCopy();
						eventReplyData3.RequiredClass = item.Value;
						eventReplyData3.IndexForAnswerTranslation = j;
						list.Add(eventReplyData3);
					}
				}
			}
			else
			{
				eventReplyData2.IndexForAnswerTranslation = j;
				list.Add(eventReplyData2);
			}
		}
		replys = list.ToArray();
	}
}
