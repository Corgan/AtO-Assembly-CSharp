// Decompiled with JetBrains decompiler
// Type: EventsParser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

#nullable disable
public static class EventsParser
{
  private static SortedList subclassList = new SortedList();
  private static SortedDictionary<string, EventData> eventsData = new SortedDictionary<string, EventData>((IComparer<string>) new StringComparerWithZeroPadding());
  private static SortedDictionary<string, List<string>> eventsByZone = new SortedDictionary<string, List<string>>((IComparer<string>) new StringComparerWithZeroPadding());
  private static Dictionary<string, List<string>> eventsByCharacter = new Dictionary<string, List<string>>();
  private static Dictionary<string, List<EventsParser.evCharacter>> evcDictionary = new Dictionary<string, List<EventsParser.evCharacter>>();

  private static void CreateHTMLFile()
  {
    string path = Application.dataPath + "/TextsFromGame/events.html";
    string str1 = "\r\n            <!DOCTYPE html>\r\n            <html>\r\n            <head>\r\n                <title>Event Parse</title>\r\n\r\n                <style>\r\n\r\n                    body {\r\n                        font-family: Arial;\r\n                        font-size:15px;\r\n                    }\r\n\r\n                    #header {\r\n                        position: fixed;\r\n                        top: 0;\r\n                        width: 100%;\r\n                        color: #333;\r\n                        font-weight: bold;\r\n                        padding: 0.6em;\r\n                        text-align: center;\r\n                        background: #CCC;\r\n                        border-bottom: 0.1em solid #AAA;\r\n                    }\r\n\r\n                    #header label {\r\n                        margin-left: 0.5em;\r\n                    }\r\n\r\n                    #header select {\r\n                        font-size: 1.1em;\r\n                    }\r\n\r\n                    #content {\r\n                        margin-top: 50px;\r\n                        padding: 20px;\r\n                    }\r\n\r\n                    .hidden {\r\n                        display: none;\r\n                    }\r\n\r\n                    .event {\r\n                        margin: .6em 0;\r\n                        padding: .4em .8em .8em;\r\n                        border: 1px solid #CCC;\r\n                        background: #EEE;\r\n                    }\r\n\r\n                    .event h3 {\r\n                        margin: .2em 0 .7em;\r\n                        font-size: 1.1em;\r\n                    }\r\n\r\n                    .event h3 span {\r\n                        font-size: 0.8em;\r\n                        font-style: italic;\r\n                    }\r\n\r\n                    .eventAction {\r\n                        font-style: italic;\r\n                        font-weight: bold;\r\n                    }\r\n\r\n                    .reply {\r\n                        margin: 0.8em;\r\n                        padding: 1em 0.8em;\r\n                        background: #ddd;\r\n                        border-bottom: 1px solid #ccc;\r\n                    }\r\n\r\n                    .reply label {\r\n                        font-size:.8em;\r\n                        font-weight:bold;\r\n                    }\r\n\r\n                    .replyResult {\r\n                        font-size: .9em;\r\n                        padding: 1em;\r\n                        background: #d0d0d0;\r\n                        margin: 0.6em;\r\n                    }\r\n\r\n                    .replyResult>span{ \r\n                        font-size: .9em;\r\n                        font-weight: bold;\r\n                        text-shadow: 1px 1px 2px #aaa;\r\n                    }\r\n\r\n                    .replyReward {\r\n                        margin-top: 1em;\r\n                        background: #ddd;\r\n                        font-size: .9em;\r\n                    }\r\n\r\n                    .replyReward>span {\r\n                        font-weight: bold;\r\n                        display: inline-block;\r\n                        margin: 0.7em 0.2em 0.7em 1em;\r\n                        line-height: 0.7em;\r\n                    }\r\n\r\n                    .results {\r\n                        margin-top: 1em;\r\n                    }\r\n\r\n                    a.showLink {\r\n                        font-weight: bold;\r\n                        font-size: .8em;\r\n                        color: #5528c3;\r\n                        text-decoration: underline;\r\n                        display: inline-block;\r\n                        margin: 0 0 0 0.6em;\r\n                        padding: 0.3em 0.4em;\r\n                    }\r\n\r\n                    a.showLink:hover {\r\n                        background: #5528c3;\r\n                        color:#FFF;\r\n                    }\r\n\r\n                    .eventClasses {\r\n                        float:right;\r\n                        margin-bottom: 4px;\r\n                    }\r\n\r\n                    .eventClasses span{\r\n                        display: inline-block;\r\n                        margin: .2em .1em;\r\n                        font-size: .8em;\r\n                        padding: .4em .3em;\r\n                        font-weight: bold;\r\n                    }\r\n\r\n                    .roll {\r\n                        font-size: .7em;\r\n                        font-weight: bold;\r\n                        border: 1px solid #aaa;\r\n                        padding: 0.3em 0.7em;\r\n                        border-radius: 1em;\r\n                        display: inline-block;\r\n                        margin: 0 0 0 0.6em;\r\n                        background: #f0f0f0;\r\n                    }\r\n\r\n                    .magnus, .grukli, .heiner, .bree, .yogger {\r\n                        background:#F3404E;\r\n                        border-bottom: 2px solid #9d1b25;\r\n                        color: #FFF;\r\n                    }\r\n\r\n                    .evelyn, .wilbur, .cornelius, .zek, .amelia, .sigrun {\r\n                        background:#3298FF;\r\n                        border-bottom: 2px solid #237cd5;\r\n                        color: #FFF;\r\n                    }\r\n\r\n                    .andrin, .sylvie, .gustav, .thuls, .nenukil, .bernard {\r\n                        background:#34FF46;\r\n                        border-bottom: 2px solid #02930f;\r\n                        color: #333;\r\n                    }\r\n\r\n                    .reginald, .malukah, .ottis, .nezglekt, .tulah {\r\n                        background:#BBBBBB;\r\n                        border-bottom: 2px solid #666;\r\n                        color: #333;\r\n                    }\r\n\r\n                    .navalea, .laia {\r\n                        background:#D07FFF;\r\n                        border-bottom: 2px solid #b247ef;\r\n                        color: #FFF;\r\n                    }\r\n\r\n                    .allCharacters {\r\n                        background: #edc15e;\r\n                        border-bottom: 2px solid #c19a43;\r\n                        color: #333;\r\n                    }\r\n\r\n                    .replySuccess {\r\n                        background-color:#b4cdb4;\r\n                    }\r\n\r\n                    .replyFail {\r\n                        background-color:#cfbcbf;\r\n                    }\r\n\r\n\r\n\r\n                </style>\r\n\r\n\r\n                <script>\r\n\r\n                    var events;\r\n                    var zones;\r\n                    var replies;\r\n                    var results;\r\n\r\n                    var linkResults;\r\n                    var linkReplies;\r\n\r\n                    function toggleReplies(link) {\r\n                        var answersDiv = link.nextElementSibling;\r\n\r\n                        if (answersDiv.classList.contains('hidden')) {\r\n                            answersDiv.classList.remove('hidden');\r\n                            link.innerHTML = 'Hide replies';\r\n                        } else {\r\n                            answersDiv.classList.add('hidden');\r\n                            link.innerHTML = 'Show replies';\r\n                        }\r\n                    }\r\n\r\n                    function toggleResults(link) {\r\n                        var resultsDiv = link.nextElementSibling;\r\n\r\n                        if (resultsDiv.classList.contains('hidden')) {\r\n                            resultsDiv.classList.remove('hidden');\r\n                            link.innerHTML = 'Hide results';\r\n                        } else {\r\n                            resultsDiv.classList.add('hidden');\r\n                            link.innerHTML = 'Show results';\r\n                        }\r\n                    }\r\n\r\n\r\n                    function filterContent() {\r\n\r\n                        var zona = document.getElementById('zonaSelect').value;\r\n                        var personaje = document.getElementById('personajeSelect').value;\r\n\r\n                        var classToShow = '';\r\n\r\n                        if (zona != 'all') {\r\n\r\n                            classToShow += 'zona_'+zona;\r\n\r\n                            for (var i = 0; i < zones.length; i++) {\r\n                                zones[i].classList.add('hidden');\r\n                            }\r\n\r\n                            var zoneHiddenElements = document.getElementsByClassName(classToShow);\r\n\r\n                            for (var i = 0; i < zoneHiddenElements.length; i++) {\r\n                                zoneHiddenElements[i].classList.remove('hidden');\r\n                            }\r\n\r\n                        }\r\n                        else {\r\n                            for (var i = 0; i < zones.length; i++) {\r\n                                zones[i].classList.remove('hidden');\r\n                            }\r\n\r\n                        }\r\n\r\n\r\n\r\n                        if (personaje != 'all') {\r\n                            classToShow += ' personaje_'+personaje;\r\n                        }\r\n\r\n\r\n                        if (classToShow == '') {\r\n                            for (var i = 0; i < events.length; i++) {\r\n                                events[i].classList.remove('hidden');\r\n                            }\r\n\r\n                        }\r\n                        else \r\n                        {\r\n                            for (var i = 0; i < events.length; i++) {\r\n                                events[i].classList.add('hidden');\r\n                            }\r\n\r\n                            var hiddenElements = document.getElementsByClassName(classToShow);\r\n\r\n                            for (var i = 0; i < hiddenElements.length; i++) {\r\n                                hiddenElements[i].classList.remove('hidden');\r\n                            }\r\n\r\n                        }\r\n\r\n                    }\r\n\r\n                    function showAll(state) {\r\n                        if (state) {\r\n                            for (var i = 0; i < replies.length; i++) {\r\n                                replies[i].classList.remove('hidden');\r\n                            }\r\n                            for (var i = 0; i < results.length; i++) {\r\n                                results[i].classList.remove('hidden');\r\n                            }\r\n                            for (var i = 0; i < linkReplies.length; i++) {\r\n                                linkReplies[i].innerHTML = 'Hide replies';\r\n                            }\r\n                            for (var i = 0; i < linkResults.length; i++) {\r\n                                linkResults[i].innerHTML = 'Hide results';\r\n                            }\r\n                        }\r\n                        else \r\n                        {\r\n                            for (var i = 0; i < replies.length; i++) {\r\n                                replies[i].classList.add('hidden');\r\n                            }\r\n                            for (var i = 0; i < results.length; i++) {\r\n                                results[i].classList.add('hidden');\r\n                            }\r\n                            for (var i = 0; i < linkReplies.length; i++) {\r\n                                linkReplies[i].innerHTML = 'Show replies';\r\n                            }\r\n                            for (var i = 0; i < linkResults.length; i++) {\r\n                                linkResults[i].innerHTML = 'Show results';\r\n                            }\r\n                        }\r\n                    }\r\n\r\n\r\n                    function showReplies() {\r\n                        for (var i = 0; i < replies.length; i++) {\r\n                            replies[i].classList.remove('hidden');\r\n                        }\r\n                        for (var i = 0; i < linkReplies.length; i++) {\r\n                            linkReplies[i].innerHTML = 'Hide replies';\r\n                        }\r\n                    }\r\n\r\n                    function Ready() {\r\n                        zones = document.getElementsByClassName('zoneH2');\r\n                        events = document.getElementsByClassName('event');\r\n                        replies = document.getElementsByClassName('replies');\r\n                        results = document.getElementsByClassName('results');\r\n                        linkReplies = document.getElementsByClassName('linkReplies');\r\n                        linkResults = document.getElementsByClassName('linkResults');\r\n                    }\r\n\r\n                </script>\r\n\r\n            </head>\r\n            <body>\r\n\r\n                <div id='header'>\r\n                <label for='zonaSelect'>Zone:</label>\r\n                <select id='zonaSelect' onchange='filterContent()'>\r\n                    <option value='all'>All</option>";
    foreach (KeyValuePair<string, List<string>> keyValuePair in EventsParser.eventsByZone)
    {
      if (keyValuePair.Key != "")
        str1 = str1 + "<option value='" + keyValuePair.Key.ToLower() + "'>" + keyValuePair.Key + "</option>";
    }
    string str2 = str1 + "\r\n                </select>\r\n\r\n                <label for='personajeSelect'>Character:</label>\r\n                <select id='personajeSelect' onchange='filterContent()'>\r\n                    <option value='all'>All</option>";
    for (int index = 0; index < EventsParser.subclassList.Count; ++index)
      str2 = str2 + "<option value='" + EventsParser.subclassList.GetByIndex(index).ToString().ToLower() + "'>" + EventsParser.subclassList.GetByIndex(index).ToString() + "</option>";
    string str3 = str2 + "\r\n                </select>\r\n\r\n                <a href='javascript:void(0);' class='showLink linkHeader' onclick='showReplies()'>Show replies</a>\r\n                <a href='javascript:void(0);' class='showLink linkHeader' onclick='showAll(true)'>Show everything</a>\r\n                <a href='javascript:void(0);' class='showLink linkHeader' onclick='showAll(false)'>Hide everything</a>\r\n\r\n            </div>\r\n\r\n\r\n            <div id='content'>\r\n";
    StringBuilder stringBuilder1 = new StringBuilder();
    foreach (KeyValuePair<string, List<string>> keyValuePair in EventsParser.eventsByZone)
    {
      if (!(keyValuePair.Key == ""))
      {
        stringBuilder1.Append("<div id='zone_");
        stringBuilder1.Append(keyValuePair.Key.ToLower());
        stringBuilder1.Append("'>");
        stringBuilder1.Append("<h2 class='zoneH2 zona_" + keyValuePair.Key.ToLower() + "'>");
        stringBuilder1.Append(keyValuePair.Key);
        stringBuilder1.Append("</h2>");
        for (int index1 = 0; index1 < keyValuePair.Value.Count; ++index1)
        {
          EventData eventData = EventsParser.eventsData[keyValuePair.Value[index1]];
          List<string> stringList = new List<string>();
          for (int index2 = 0; index2 < eventData.Replys.Length; ++index2)
          {
            SubClassData requiredClass = eventData.Replys[index2].RequiredClass;
            if ((UnityEngine.Object) requiredClass != (UnityEngine.Object) null)
              stringList.Add(requiredClass.CharacterName);
          }
          stringBuilder1.Append("<div class='event");
          stringBuilder1.Append(" zona_" + keyValuePair.Key.ToLower());
          for (int index3 = 0; index3 < stringList.Count; ++index3)
            stringBuilder1.Append(" personaje_" + stringList[index3].ToLower());
          stringBuilder1.Append("' id='event_");
          stringBuilder1.Append(eventData.EventId);
          stringBuilder1.Append("'>");
          stringBuilder1.Append("<div class='eventClasses'>");
          for (int index4 = 0; index4 < stringList.Count; ++index4)
          {
            stringBuilder1.Append("<span class='");
            stringBuilder1.Append(stringList[index4].ToLower());
            stringBuilder1.Append("'>");
            stringBuilder1.Append(stringList[index4]);
            stringBuilder1.Append("</span>");
          }
          stringBuilder1.Append("</div>");
          stringBuilder1.Append("<h3>");
          stringBuilder1.Append(Texts.Instance.GetText(eventData.EventId + "_nm", "events"));
          if (Texts.Instance.GetText(eventData.EventId + "_nm", "events") == "")
            stringBuilder1.Append("[missing] " + eventData.EventName);
          stringBuilder1.Append(" <span>(");
          stringBuilder1.Append(eventData.EventId);
          stringBuilder1.Append(")</span>");
          stringBuilder1.Append("</h3>");
          stringBuilder1.Append(Texts.Instance.GetText(eventData.EventId + "_dsc", "events"));
          if (Texts.Instance.GetText(eventData.EventId + "_dsc", "events") == "")
            stringBuilder1.Append("[missing] " + eventData.Description);
          stringBuilder1.Append("<br><br>");
          stringBuilder1.Append("<span class='eventAction'>");
          stringBuilder1.Append(Texts.Instance.GetText(eventData.EventId + "_dsca", "events"));
          if (Texts.Instance.GetText(eventData.EventId + "_dsca", "events") == "")
            stringBuilder1.Append("[missing] " + eventData.DescriptionAction);
          stringBuilder1.Append("</span>");
          stringBuilder1.Append(" <a href='javascript:void(0);' class='showLink linkReplies' onclick='toggleReplies(this)'>Show replies</a>");
          stringBuilder1.Append("<div class='replies hidden'>");
          for (int index5 = 0; index5 < eventData.Replys.Length; ++index5)
          {
            stringBuilder1.Append("<div class='reply'>");
            if ((UnityEngine.Object) eventData.Replys[index5].RequiredClass != (UnityEngine.Object) null)
            {
              stringBuilder1.Append("<div class='eventClasses'>");
              stringBuilder1.Append("<span class='");
              stringBuilder1.Append(eventData.Replys[index5].RequiredClass.CharacterName.ToLower());
              stringBuilder1.Append("'>");
              stringBuilder1.Append(eventData.Replys[index5].RequiredClass.CharacterName);
              stringBuilder1.Append("</span>");
              stringBuilder1.Append("</div>");
            }
            else if (eventData.Replys[index5].RepeatForAllCharacters)
            {
              stringBuilder1.Append("<div class='eventClasses'>");
              stringBuilder1.Append("<span class='allCharacters'>");
              stringBuilder1.Append("All characters");
              stringBuilder1.Append("</span>");
              stringBuilder1.Append("</div>");
            }
            string _id = eventData.EventId + "_rp" + index5.ToString();
            if (eventData.Replys[index5].ReplyActionText != Enums.EventAction.None)
              stringBuilder1.Append("<label>[" + Enum.GetName(typeof (Enums.EventAction), (object) eventData.Replys[index5].ReplyActionText) + "]</label> ");
            stringBuilder1.Append(Texts.Instance.GetText(_id, "events"));
            if (Texts.Instance.GetText(_id, "events") == "")
              stringBuilder1.Append("[missing] " + eventData.Replys[index5].ReplyText);
            if (eventData.Replys[index5].SsRoll)
            {
              stringBuilder1.Append("<span class='roll'>Roll ");
              stringBuilder1.Append(Enum.GetName(typeof (Enums.RollTarget), (object) eventData.Replys[index5].SsRollTarget));
              stringBuilder1.Append(" | ");
              if (eventData.Replys[index5].SsRollCard != Enums.CardType.None)
              {
                stringBuilder1.Append("Card ");
                stringBuilder1.Append(Enum.GetName(typeof (Enums.CardType), (object) eventData.Replys[index5].SsRollCard));
              }
              else
              {
                stringBuilder1.Append(Enum.GetName(typeof (Enums.RollMode), (object) eventData.Replys[index5].SsRollMode));
                stringBuilder1.Append(" ");
                stringBuilder1.Append(eventData.Replys[index5].SsRollNumber);
              }
              stringBuilder1.Append("</span>");
            }
            if ((UnityEngine.Object) eventData.Replys[index5].Requirement != (UnityEngine.Object) null || (bool) (UnityEngine.Object) eventData.Replys[index5].RequirementItem)
            {
              stringBuilder1.Append("<span class='roll'>Requeriment:");
              if ((UnityEngine.Object) eventData.Replys[index5].Requirement != (UnityEngine.Object) null)
              {
                stringBuilder1.Append(" ");
                stringBuilder1.Append(eventData.Replys[index5].Requirement.RequirementId);
              }
              if ((UnityEngine.Object) eventData.Replys[index5].RequirementItem != (UnityEngine.Object) null)
              {
                stringBuilder1.Append(" Item ");
                stringBuilder1.Append(eventData.Replys[index5].RequirementItem.Id);
              }
              stringBuilder1.Append("</span>");
            }
            if (eventData.Replys[index5].SsRemoveItemSlot != Enums.ItemSlot.None)
            {
              stringBuilder1.Append("<span class='roll'>RemoveItemSlot: ");
              stringBuilder1.Append(Enum.GetName(typeof (Enums.ItemSlot), (object) eventData.Replys[index5].SsRemoveItemSlot));
              stringBuilder1.Append("</span>");
            }
            if (eventData.Replys[index5].SsCorruptItemSlot != Enums.ItemSlot.None)
            {
              stringBuilder1.Append("<span class='roll'>CorruptItemSlot: ");
              stringBuilder1.Append(Enum.GetName(typeof (Enums.ItemSlot), (object) eventData.Replys[index5].SsCorruptItemSlot));
              stringBuilder1.Append("</span>");
            }
            stringBuilder1.Append("<a href='javascript:void(0);' class='showLink linkResults' onclick='toggleResults(this)'>Show results</a>");
            stringBuilder1.Append("<div class='results hidden'>");
            string text1 = Texts.Instance.GetText(_id + "_s", "events");
            int num;
            if (text1 != "")
            {
              stringBuilder1.Append("<div class='replyResult replySuccess'>");
              stringBuilder1.Append("<span>");
              stringBuilder1.Append("[Success] ");
              stringBuilder1.Append("</span>");
              stringBuilder1.Append(text1);
              stringBuilder1.Append("<div class='replyReward'>");
              if (eventData.Replys[index5].SsGoldReward != 0)
              {
                StringBuilder stringBuilder2 = stringBuilder1;
                num = eventData.Replys[index5].SsGoldReward;
                string str4 = "<span>Gold: </span>" + num.ToString() + " ";
                stringBuilder2.Append(str4);
              }
              if (eventData.Replys[index5].SsDustReward != 0)
              {
                StringBuilder stringBuilder3 = stringBuilder1;
                num = eventData.Replys[index5].SsDustReward;
                string str5 = "<span>Dust: </span>" + num.ToString() + " ";
                stringBuilder3.Append(str5);
              }
              if (eventData.Replys[index5].SsDustReward != 0)
              {
                StringBuilder stringBuilder4 = stringBuilder1;
                num = eventData.Replys[index5].SsSupplyReward;
                string str6 = "<span>Supply: </span>" + num.ToString() + " ";
                stringBuilder4.Append(str6);
              }
              if (eventData.Replys[index5].SsExperienceReward != 0)
              {
                StringBuilder stringBuilder5 = stringBuilder1;
                num = eventData.Replys[index5].SsExperienceReward;
                string str7 = "<span>Experience: </span>" + num.ToString() + " ";
                stringBuilder5.Append(str7);
              }
              if ((UnityEngine.Object) eventData.Replys[index5].SsRequirementUnlock != (UnityEngine.Object) null)
                stringBuilder1.Append("<span>Unlock: </span>" + eventData.Replys[index5].SsRequirementUnlock.RequirementId + " ");
              if ((UnityEngine.Object) eventData.Replys[index5].SsRequirementLock != (UnityEngine.Object) null)
                stringBuilder1.Append("<span>Lock: </span>" + eventData.Replys[index5].SsRequirementLock.RequirementId + " ");
              if ((UnityEngine.Object) eventData.Replys[index5].SsAddCard1 != (UnityEngine.Object) null)
                stringBuilder1.Append("<span>AddCard: </span>" + eventData.Replys[index5].SsAddCard1.Id + " ");
              stringBuilder1.Append("</div>");
              stringBuilder1.Append("</div>");
            }
            string text2 = Texts.Instance.GetText(_id + "_sc", "events");
            if (text2 != "")
            {
              stringBuilder1.Append("<div class='replyResult replySuccess'>");
              stringBuilder1.Append("<span>");
              stringBuilder1.Append("[Critical success] ");
              stringBuilder1.Append("</span>");
              stringBuilder1.Append(text2);
              stringBuilder1.Append("<div class='replyReward'>");
              if (eventData.Replys[index5].SscGoldReward != 0)
              {
                StringBuilder stringBuilder6 = stringBuilder1;
                num = eventData.Replys[index5].SscGoldReward;
                string str8 = "<span>Gold: </span>" + num.ToString() + " ";
                stringBuilder6.Append(str8);
              }
              if (eventData.Replys[index5].SscDustReward != 0)
              {
                StringBuilder stringBuilder7 = stringBuilder1;
                num = eventData.Replys[index5].SscDustReward;
                string str9 = "<span>Dust: </span>" + num.ToString() + " ";
                stringBuilder7.Append(str9);
              }
              if (eventData.Replys[index5].SscDustReward != 0)
              {
                StringBuilder stringBuilder8 = stringBuilder1;
                num = eventData.Replys[index5].SscSupplyReward;
                string str10 = "<span>Supply: </span>" + num.ToString() + " ";
                stringBuilder8.Append(str10);
              }
              if (eventData.Replys[index5].SscExperienceReward != 0)
              {
                StringBuilder stringBuilder9 = stringBuilder1;
                num = eventData.Replys[index5].SscExperienceReward;
                string str11 = "<span>Experience: </span>" + num.ToString() + " ";
                stringBuilder9.Append(str11);
              }
              if ((UnityEngine.Object) eventData.Replys[index5].SscRequirementUnlock != (UnityEngine.Object) null)
                stringBuilder1.Append("<span>Unlock: </span>" + eventData.Replys[index5].SscRequirementUnlock.RequirementId + " ");
              if ((UnityEngine.Object) eventData.Replys[index5].SscRequirementLock != (UnityEngine.Object) null)
                stringBuilder1.Append("<span>Lock: </span>" + eventData.Replys[index5].SscRequirementLock.RequirementId + " ");
              if ((UnityEngine.Object) eventData.Replys[index5].SscAddCard1 != (UnityEngine.Object) null)
                stringBuilder1.Append("<span>AddCard: </span>" + eventData.Replys[index5].SscAddCard1.Id + " ");
              stringBuilder1.Append("</div>");
              stringBuilder1.Append("</div>");
            }
            string text3 = Texts.Instance.GetText(_id + "_f", "events");
            if (text3 != "")
            {
              stringBuilder1.Append("<div class='replyResult replyFail'>");
              stringBuilder1.Append("<span>");
              stringBuilder1.Append("[Failure] ");
              stringBuilder1.Append("</span>");
              stringBuilder1.Append(text3);
              stringBuilder1.Append("<div class='replyReward'>");
              if (eventData.Replys[index5].FlGoldReward != 0)
              {
                StringBuilder stringBuilder10 = stringBuilder1;
                num = eventData.Replys[index5].FlGoldReward;
                string str12 = "<span>Gold: </span>" + num.ToString() + " ";
                stringBuilder10.Append(str12);
              }
              if (eventData.Replys[index5].FlDustReward != 0)
              {
                StringBuilder stringBuilder11 = stringBuilder1;
                num = eventData.Replys[index5].FlDustReward;
                string str13 = "<span>Dust: </span>" + num.ToString() + " ";
                stringBuilder11.Append(str13);
              }
              if (eventData.Replys[index5].FlDustReward != 0)
              {
                StringBuilder stringBuilder12 = stringBuilder1;
                num = eventData.Replys[index5].FlSupplyReward;
                string str14 = "<span>Supply: </span>" + num.ToString() + " ";
                stringBuilder12.Append(str14);
              }
              if (eventData.Replys[index5].FlExperienceReward != 0)
              {
                StringBuilder stringBuilder13 = stringBuilder1;
                num = eventData.Replys[index5].FlExperienceReward;
                string str15 = "<span>Experience: </span>" + num.ToString() + " ";
                stringBuilder13.Append(str15);
              }
              if ((UnityEngine.Object) eventData.Replys[index5].FlRequirementUnlock != (UnityEngine.Object) null)
                stringBuilder1.Append("<span>Unlock: </span>" + eventData.Replys[index5].FlRequirementUnlock.RequirementId + " ");
              if ((UnityEngine.Object) eventData.Replys[index5].FlRequirementLock != (UnityEngine.Object) null)
                stringBuilder1.Append("<span>Lock: </span>" + eventData.Replys[index5].FlRequirementLock.RequirementId + " ");
              if ((UnityEngine.Object) eventData.Replys[index5].FlAddCard1 != (UnityEngine.Object) null)
                stringBuilder1.Append("<span>AddCard: </span>" + eventData.Replys[index5].FlAddCard1.Id + " ");
              stringBuilder1.Append("</div>");
              stringBuilder1.Append("</div>");
            }
            string text4 = Texts.Instance.GetText(_id + "_fc", "events");
            if (text4 != "")
            {
              stringBuilder1.Append("<div class='replyResult replyFail'>");
              stringBuilder1.Append("<span>");
              stringBuilder1.Append("[Critical failure] ");
              stringBuilder1.Append("</span>");
              stringBuilder1.Append(text4);
              stringBuilder1.Append("<div class='replyReward'>");
              if (eventData.Replys[index5].FlcGoldReward != 0)
              {
                StringBuilder stringBuilder14 = stringBuilder1;
                num = eventData.Replys[index5].FlcGoldReward;
                string str16 = "<span>Gold: </span>" + num.ToString() + " ";
                stringBuilder14.Append(str16);
              }
              if (eventData.Replys[index5].FlcDustReward != 0)
              {
                StringBuilder stringBuilder15 = stringBuilder1;
                num = eventData.Replys[index5].FlcDustReward;
                string str17 = "<span>Dust: </span>" + num.ToString() + " ";
                stringBuilder15.Append(str17);
              }
              if (eventData.Replys[index5].FlcDustReward != 0)
              {
                StringBuilder stringBuilder16 = stringBuilder1;
                num = eventData.Replys[index5].FlcSupplyReward;
                string str18 = "<span>Supply: </span>" + num.ToString() + " ";
                stringBuilder16.Append(str18);
              }
              if (eventData.Replys[index5].FlcExperienceReward != 0)
              {
                StringBuilder stringBuilder17 = stringBuilder1;
                num = eventData.Replys[index5].FlcExperienceReward;
                string str19 = "<span>Experience: </span>" + num.ToString() + " ";
                stringBuilder17.Append(str19);
              }
              if ((UnityEngine.Object) eventData.Replys[index5].FlcRequirementUnlock != (UnityEngine.Object) null)
                stringBuilder1.Append("<span>Unlock: </span>" + eventData.Replys[index5].FlcRequirementUnlock.RequirementId + " ");
              if ((UnityEngine.Object) eventData.Replys[index5].FlcRequirementLock != (UnityEngine.Object) null)
                stringBuilder1.Append("<span>Lock: </span>" + eventData.Replys[index5].FlcRequirementLock.RequirementId + " ");
              if ((UnityEngine.Object) eventData.Replys[index5].FlcAddCard1 != (UnityEngine.Object) null)
                stringBuilder1.Append("<span>AddCard: </span>" + eventData.Replys[index5].FlcAddCard1.Id + " ");
              stringBuilder1.Append("</div>");
              stringBuilder1.Append("</div>");
            }
            stringBuilder1.Append("</div>");
            stringBuilder1.Append("</div>");
          }
          stringBuilder1.Append("</div>");
          stringBuilder1.Append("</div>");
        }
        stringBuilder1.Append("</div>");
      }
    }
    string str20 = "\r\n                </div>\r\n\r\n            <script>Ready();</script>\r\n            </body>\r\n            </html>\r\n        ";
    string str21 = stringBuilder1.ToString();
    string contents = str3 + str21 + str20;
    try
    {
      File.WriteAllText(path, contents);
    }
    catch (Exception ex)
    {
    }
  }

  public static void ParseAll()
  {
    Debug.Log((object) "--------------\nEventsParser\n--------------");
    SubClassData[] subClassDataArray = Resources.LoadAll<SubClassData>("SubClass");
    EventsParser.subclassList = new SortedList();
    for (int index = 0; index < subClassDataArray.Length; ++index)
    {
      if (subClassDataArray[index].MainCharacter)
        EventsParser.subclassList.Add((object) subClassDataArray[index].CharacterName, (object) subClassDataArray[index].CharacterName);
    }
    foreach (EventData eventData in Resources.LoadAll<EventData>("World/Events"))
    {
      if (!EventsParser.eventsData.ContainsKey(eventData.EventId))
        EventsParser.eventsData.Add(eventData.EventId, eventData);
    }
    foreach (KeyValuePair<string, EventData> keyValuePair in EventsParser.eventsData)
    {
      string eventZone = EventsParser.GetEventZone(keyValuePair.Value.EventId);
      if (!EventsParser.eventsByZone.ContainsKey(eventZone))
        EventsParser.eventsByZone.Add(eventZone, new List<string>());
      string eventId = keyValuePair.Value.EventId;
      if (!EventsParser.eventsByZone[eventZone].Contains(eventId))
        EventsParser.eventsByZone[eventZone].Add(eventId);
    }
    EventsParser.CreateHTMLFile();
  }

  private static string GetEventZone(string id)
  {
    if (id.StartsWith("e_aquar"))
      return "Aquarfall";
    if (id.StartsWith("e_forge"))
      return "BlackForge";
    if (id.StartsWith("e_challenge"))
      return "Challenge";
    if (id.StartsWith("e_dread"))
      return "Dreadnought";
    if (id.StartsWith("e_faen"))
      return "Faeborg";
    if (id.StartsWith("e_sewers"))
      return "FrozenSewers";
    if (id.StartsWith("e_pyr"))
      return "Pyramid";
    if (id.StartsWith("e_sahti"))
      return "Sahti";
    if (id.StartsWith("e_secta"))
      return "Sectarium";
    if (id.StartsWith("e_sen"))
      return "Senenthia";
    if (id.StartsWith("e_lair"))
      return "SpiderLair";
    if (id.StartsWith("e_ulmin"))
      return "Ulminin";
    if (id.StartsWith("e_upri"))
      return "Uprising";
    if (id.StartsWith("e_velka"))
      return "Velkarath";
    if (id.StartsWith("e_voidhigh"))
      return "VoidHigh";
    if (id.StartsWith("e_voidlow"))
      return "VoidLow";
    if (id.StartsWith("e_wolf"))
      return "WolfWars";
    if (id.StartsWith("e_sunken"))
      return "SunkenTemple";
    return id.StartsWith("e_dream") ? "Dreams" : "";
  }

  private class evCharacter
  {
    public string eventId;
    public string characterName;
    public string characterText;
    public string answer_s;
    public string answer_f;
    public string answer_sc;
    public string answer_fc;
  }
}
