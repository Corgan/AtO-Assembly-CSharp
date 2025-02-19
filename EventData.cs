// Decompiled with JetBrains decompiler
// Type: EventData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
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

  public void Init()
  {
    List<EventReplyData> eventReplyDataList = new List<EventReplyData>();
    List<string> stringList = new List<string>();
    for (int index = 0; index < this.replys.Length; ++index)
    {
      EventReplyData reply = this.replys[index];
      if (reply != null && (Object) reply.RequiredClass != (Object) null)
        stringList.Add(reply.RequiredClass.Id);
    }
    for (int index = 0; index < this.replys.Length; ++index)
    {
      EventReplyData reply = this.replys[index];
      if (reply != null)
      {
        if (reply.RepeatForAllCharacters || reply.RepeatForAllWarriors || reply.RepeatForAllScouts || reply.RepeatForAllMages || reply.RepeatForAllHealers)
        {
          foreach (KeyValuePair<string, SubClassData> keyValuePair in Globals.Instance.SubClass)
          {
            if ((Object) keyValuePair.Value != (Object) null && keyValuePair.Value.MainCharacter && (stringList.Count <= 0 || !stringList.Contains(keyValuePair.Value.Id)))
            {
              bool flag = false;
              if (reply.RepeatForAllCharacters)
              {
                flag = true;
              }
              else
              {
                if (reply.RepeatForAllWarriors && (keyValuePair.Value.HeroClass == Enums.HeroClass.Warrior || keyValuePair.Value.HeroClassSecondary == Enums.HeroClass.Warrior))
                  flag = true;
                if (reply.RepeatForAllScouts && (keyValuePair.Value.HeroClass == Enums.HeroClass.Scout || keyValuePair.Value.HeroClassSecondary == Enums.HeroClass.Scout))
                  flag = true;
                if (reply.RepeatForAllMages && (keyValuePair.Value.HeroClass == Enums.HeroClass.Mage || keyValuePair.Value.HeroClassSecondary == Enums.HeroClass.Mage))
                  flag = true;
                if (reply.RepeatForAllHealers && (keyValuePair.Value.HeroClass == Enums.HeroClass.Healer || keyValuePair.Value.HeroClassSecondary == Enums.HeroClass.Healer))
                  flag = true;
              }
              if (flag)
              {
                EventReplyData eventReplyData = reply.ShallowCopy();
                eventReplyData.RequiredClass = keyValuePair.Value;
                eventReplyData.IndexForAnswerTranslation = index;
                eventReplyDataList.Add(eventReplyData);
              }
            }
          }
        }
        else
        {
          reply.IndexForAnswerTranslation = index;
          eventReplyDataList.Add(reply);
        }
      }
    }
    this.replys = eventReplyDataList.ToArray();
  }

  public string EventId
  {
    get => this.eventId;
    set => this.eventId = value;
  }

  public string EventName
  {
    get => this.eventName;
    set => this.eventName = value;
  }

  public string Description
  {
    get => this.description;
    set => this.description = value;
  }

  public string DescriptionAction
  {
    get => this.descriptionAction;
    set => this.descriptionAction = value;
  }

  public Sprite EventSpriteMap
  {
    get => this.eventSpriteMap;
    set => this.eventSpriteMap = value;
  }

  public Sprite EventSpriteBook
  {
    get => this.eventSpriteBook;
    set => this.eventSpriteBook = value;
  }

  public bool HistoryMode
  {
    get => this.historyMode;
    set => this.historyMode = value;
  }

  public EventReplyData[] Replys
  {
    get => this.replys;
    set => this.replys = value;
  }

  public EventRequirementData Requirement
  {
    get => this.requirement;
    set => this.requirement = value;
  }

  public SubClassData RequiredClass
  {
    get => this.requiredClass;
    set => this.requiredClass = value;
  }

  public Enums.MapIconShader EventIconShader
  {
    get => this.eventIconShader;
    set => this.eventIconShader = value;
  }

  public Enums.CombatTier EventTier
  {
    get => this.eventTier;
    set => this.eventTier = value;
  }

  public Sprite EventSpriteDecor
  {
    get => this.eventSpriteDecor;
    set => this.eventSpriteDecor = value;
  }

  public string EventUniqueId
  {
    get => this.eventUniqueId;
    set => this.eventUniqueId = value;
  }

  public int ReplyRandom
  {
    get => this.replyRandom;
    set => this.replyRandom = value;
  }
}
