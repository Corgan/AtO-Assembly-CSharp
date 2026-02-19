using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class EventsParser
{
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

	private static SortedList subclassList = new SortedList();

	private static SortedDictionary<string, EventData> eventsData = new SortedDictionary<string, EventData>(new StringComparerWithZeroPadding());

	private static SortedDictionary<string, List<string>> eventsByZone = new SortedDictionary<string, List<string>>(new StringComparerWithZeroPadding());

	private static Dictionary<string, List<string>> eventsByCharacter = new Dictionary<string, List<string>>();

	private static Dictionary<string, List<evCharacter>> evcDictionary = new Dictionary<string, List<evCharacter>>();

	private static void CreateHTMLFile()
	{
		string path = Application.dataPath + "/TextsFromGame/events.html";
		string text = "\n            <!DOCTYPE html>\n            <html>\n            <head>\n                <title>Event Parse</title>\n\n                <style>\n\n                    body {\n                        font-family: Arial;\n                        font-size:15px;\n                    }\n\n                    #header {\n                        position: fixed;\n                        top: 0;\n                        width: 100%;\n                        color: #333;\n                        font-weight: bold;\n                        padding: 0.6em;\n                        text-align: center;\n                        background: #CCC;\n                        border-bottom: 0.1em solid #AAA;\n                    }\n\n                    #header label {\n                        margin-left: 0.5em;\n                    }\n\n                    #header select {\n                        font-size: 1.1em;\n                    }\n\n                    #content {\n                        margin-top: 50px;\n                        padding: 20px;\n                    }\n\n                    .hidden {\n                        display: none;\n                    }\n\n                    .event {\n                        margin: .6em 0;\n                        padding: .4em .8em .8em;\n                        border: 1px solid #CCC;\n                        background: #EEE;\n                    }\n\n                    .event h3 {\n                        margin: .2em 0 .7em;\n                        font-size: 1.1em;\n                    }\n\n                    .event h3 span {\n                        font-size: 0.8em;\n                        font-style: italic;\n                    }\n\n                    .eventAction {\n                        font-style: italic;\n                        font-weight: bold;\n                    }\n\n                    .reply {\n                        margin: 0.8em;\n                        padding: 1em 0.8em;\n                        background: #ddd;\n                        border-bottom: 1px solid #ccc;\n                    }\n\n                    .reply label {\n                        font-size:.8em;\n                        font-weight:bold;\n                    }\n\n                    .replyResult {\n                        font-size: .9em;\n                        padding: 1em;\n                        background: #d0d0d0;\n                        margin: 0.6em;\n                    }\n\n                    .replyResult>span{ \n                        font-size: .9em;\n                        font-weight: bold;\n                        text-shadow: 1px 1px 2px #aaa;\n                    }\n\n                    .replyReward {\n                        margin-top: 1em;\n                        background: #ddd;\n                        font-size: .9em;\n                    }\n\n                    .replyReward>span {\n                        font-weight: bold;\n                        display: inline-block;\n                        margin: 0.7em 0.2em 0.7em 1em;\n                        line-height: 0.7em;\n                    }\n\n                    .results {\n                        margin-top: 1em;\n                    }\n\n                    a.showLink {\n                        font-weight: bold;\n                        font-size: .8em;\n                        color: #5528c3;\n                        text-decoration: underline;\n                        display: inline-block;\n                        margin: 0 0 0 0.6em;\n                        padding: 0.3em 0.4em;\n                    }\n\n                    a.showLink:hover {\n                        background: #5528c3;\n                        color:#FFF;\n                    }\n\n                    .eventClasses {\n                        float:right;\n                        margin-bottom: 4px;\n                    }\n\n                    .eventClasses span{\n                        display: inline-block;\n                        margin: .2em .1em;\n                        font-size: .8em;\n                        padding: .4em .3em;\n                        font-weight: bold;\n                    }\n\n                    .roll {\n                        font-size: .7em;\n                        font-weight: bold;\n                        border: 1px solid #aaa;\n                        padding: 0.3em 0.7em;\n                        border-radius: 1em;\n                        display: inline-block;\n                        margin: 0 0 0 0.6em;\n                        background: #f0f0f0;\n                    }\n\n                    .magnus, .grukli, .heiner, .bree, .yogger {\n                        background:#F3404E;\n                        border-bottom: 2px solid #9d1b25;\n                        color: #FFF;\n                    }\n\n                    .evelyn, .wilbur, .cornelius, .zek, .amelia, .sigrun {\n                        background:#3298FF;\n                        border-bottom: 2px solid #237cd5;\n                        color: #FFF;\n                    }\n\n                    .andrin, .sylvie, .gustav, .thuls, .nenukil, .bernard {\n                        background:#34FF46;\n                        border-bottom: 2px solid #02930f;\n                        color: #333;\n                    }\n\n                    .reginald, .malukah, .ottis, .nezglekt, .tulah {\n                        background:#BBBBBB;\n                        border-bottom: 2px solid #666;\n                        color: #333;\n                    }\n\n                    .navalea, .laia {\n                        background:#D07FFF;\n                        border-bottom: 2px solid #b247ef;\n                        color: #FFF;\n                    }\n\n                    .allCharacters {\n                        background: #edc15e;\n                        border-bottom: 2px solid #c19a43;\n                        color: #333;\n                    }\n\n                    .replySuccess {\n                        background-color:#b4cdb4;\n                    }\n\n                    .replyFail {\n                        background-color:#cfbcbf;\n                    }\n\n\n\n                </style>\n\n\n                <script>\n\n                    var events;\n                    var zones;\n                    var replies;\n                    var results;\n\n                    var linkResults;\n                    var linkReplies;\n\n                    function toggleReplies(link) {\n                        var answersDiv = link.nextElementSibling;\n\n                        if (answersDiv.classList.contains('hidden')) {\n                            answersDiv.classList.remove('hidden');\n                            link.innerHTML = 'Hide replies';\n                        } else {\n                            answersDiv.classList.add('hidden');\n                            link.innerHTML = 'Show replies';\n                        }\n                    }\n\n                    function toggleResults(link) {\n                        var resultsDiv = link.nextElementSibling;\n\n                        if (resultsDiv.classList.contains('hidden')) {\n                            resultsDiv.classList.remove('hidden');\n                            link.innerHTML = 'Hide results';\n                        } else {\n                            resultsDiv.classList.add('hidden');\n                            link.innerHTML = 'Show results';\n                        }\n                    }\n\n\n                    function filterContent() {\n\n                        var zona = document.getElementById('zonaSelect').value;\n                        var personaje = document.getElementById('personajeSelect').value;\n\n                        var classToShow = '';\n\n                        if (zona != 'all') {\n\n                            classToShow += 'zona_'+zona;\n\n                            for (var i = 0; i < zones.length; i++) {\n                                zones[i].classList.add('hidden');\n                            }\n\n                            var zoneHiddenElements = document.getElementsByClassName(classToShow);\n\n                            for (var i = 0; i < zoneHiddenElements.length; i++) {\n                                zoneHiddenElements[i].classList.remove('hidden');\n                            }\n\n                        }\n                        else {\n                            for (var i = 0; i < zones.length; i++) {\n                                zones[i].classList.remove('hidden');\n                            }\n\n                        }\n\n\n\n                        if (personaje != 'all') {\n                            classToShow += ' personaje_'+personaje;\n                        }\n\n\n                        if (classToShow == '') {\n                            for (var i = 0; i < events.length; i++) {\n                                events[i].classList.remove('hidden');\n                            }\n\n                        }\n                        else \n                        {\n                            for (var i = 0; i < events.length; i++) {\n                                events[i].classList.add('hidden');\n                            }\n\n                            var hiddenElements = document.getElementsByClassName(classToShow);\n\n                            for (var i = 0; i < hiddenElements.length; i++) {\n                                hiddenElements[i].classList.remove('hidden');\n                            }\n\n                        }\n\n                    }\n\n                    function showAll(state) {\n                        if (state) {\n                            for (var i = 0; i < replies.length; i++) {\n                                replies[i].classList.remove('hidden');\n                            }\n                            for (var i = 0; i < results.length; i++) {\n                                results[i].classList.remove('hidden');\n                            }\n                            for (var i = 0; i < linkReplies.length; i++) {\n                                linkReplies[i].innerHTML = 'Hide replies';\n                            }\n                            for (var i = 0; i < linkResults.length; i++) {\n                                linkResults[i].innerHTML = 'Hide results';\n                            }\n                        }\n                        else \n                        {\n                            for (var i = 0; i < replies.length; i++) {\n                                replies[i].classList.add('hidden');\n                            }\n                            for (var i = 0; i < results.length; i++) {\n                                results[i].classList.add('hidden');\n                            }\n                            for (var i = 0; i < linkReplies.length; i++) {\n                                linkReplies[i].innerHTML = 'Show replies';\n                            }\n                            for (var i = 0; i < linkResults.length; i++) {\n                                linkResults[i].innerHTML = 'Show results';\n                            }\n                        }\n                    }\n\n\n                    function showReplies() {\n                        for (var i = 0; i < replies.length; i++) {\n                            replies[i].classList.remove('hidden');\n                        }\n                        for (var i = 0; i < linkReplies.length; i++) {\n                            linkReplies[i].innerHTML = 'Hide replies';\n                        }\n                    }\n\n                    function Ready() {\n                        zones = document.getElementsByClassName('zoneH2');\n                        events = document.getElementsByClassName('event');\n                        replies = document.getElementsByClassName('replies');\n                        results = document.getElementsByClassName('results');\n                        linkReplies = document.getElementsByClassName('linkReplies');\n                        linkResults = document.getElementsByClassName('linkResults');\n                    }\n\n                </script>\n\n            </head>\n            <body>\n\n                <div id='header'>\n                <label for='zonaSelect'>Zone:</label>\n                <select id='zonaSelect' onchange='filterContent()'>\n                    <option value='all'>All</option>";
		foreach (KeyValuePair<string, List<string>> item in eventsByZone)
		{
			if (item.Key != "")
			{
				text = text + "<option value='" + item.Key.ToLower() + "'>" + item.Key + "</option>";
			}
		}
		text += "\n                </select>\n\n                <label for='personajeSelect'>Character:</label>\n                <select id='personajeSelect' onchange='filterContent()'>\n                    <option value='all'>All</option>";
		for (int i = 0; i < subclassList.Count; i++)
		{
			text = text + "<option value='" + subclassList.GetByIndex(i).ToString().ToLower() + "'>" + subclassList.GetByIndex(i).ToString() + "</option>";
		}
		text += "\n                </select>\n\n                <a href='javascript:void(0);' class='showLink linkHeader' onclick='showReplies()'>Show replies</a>\n                <a href='javascript:void(0);' class='showLink linkHeader' onclick='showAll(true)'>Show everything</a>\n                <a href='javascript:void(0);' class='showLink linkHeader' onclick='showAll(false)'>Hide everything</a>\n\n            </div>\n\n\n            <div id='content'>\n";
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<string, List<string>> item2 in eventsByZone)
		{
			if (item2.Key == "")
			{
				continue;
			}
			stringBuilder.Append("<div id='zone_");
			stringBuilder.Append(item2.Key.ToLower());
			stringBuilder.Append("'>");
			stringBuilder.Append("<h2 class='zoneH2 zona_" + item2.Key.ToLower() + "'>");
			stringBuilder.Append(item2.Key);
			stringBuilder.Append("</h2>");
			for (int j = 0; j < item2.Value.Count; j++)
			{
				EventData eventData = eventsData[item2.Value[j]];
				List<string> list = new List<string>();
				for (int k = 0; k < eventData.Replys.Length; k++)
				{
					SubClassData requiredClass = eventData.Replys[k].RequiredClass;
					if (requiredClass != null)
					{
						list.Add(requiredClass.CharacterName);
					}
				}
				stringBuilder.Append("<div class='event");
				stringBuilder.Append(" zona_" + item2.Key.ToLower());
				for (int l = 0; l < list.Count; l++)
				{
					stringBuilder.Append(" personaje_" + list[l].ToLower());
				}
				stringBuilder.Append("' id='event_");
				stringBuilder.Append(eventData.EventId);
				stringBuilder.Append("'>");
				stringBuilder.Append("<div class='eventClasses'>");
				for (int m = 0; m < list.Count; m++)
				{
					stringBuilder.Append("<span class='");
					stringBuilder.Append(list[m].ToLower());
					stringBuilder.Append("'>");
					stringBuilder.Append(list[m]);
					stringBuilder.Append("</span>");
				}
				stringBuilder.Append("</div>");
				stringBuilder.Append("<h3>");
				stringBuilder.Append(Texts.Instance.GetText(eventData.EventId + "_nm", "events"));
				if (Texts.Instance.GetText(eventData.EventId + "_nm", "events") == "")
				{
					stringBuilder.Append("[missing] " + eventData.EventName);
				}
				stringBuilder.Append(" <span>(");
				stringBuilder.Append(eventData.EventId);
				stringBuilder.Append(")</span>");
				stringBuilder.Append("</h3>");
				stringBuilder.Append(Texts.Instance.GetText(eventData.EventId + "_dsc", "events"));
				if (Texts.Instance.GetText(eventData.EventId + "_dsc", "events") == "")
				{
					stringBuilder.Append("[missing] " + eventData.Description);
				}
				stringBuilder.Append("<br><br>");
				stringBuilder.Append("<span class='eventAction'>");
				stringBuilder.Append(Texts.Instance.GetText(eventData.EventId + "_dsca", "events"));
				if (Texts.Instance.GetText(eventData.EventId + "_dsca", "events") == "")
				{
					stringBuilder.Append("[missing] " + eventData.DescriptionAction);
				}
				stringBuilder.Append("</span>");
				stringBuilder.Append(" <a href='javascript:void(0);' class='showLink linkReplies' onclick='toggleReplies(this)'>Show replies</a>");
				stringBuilder.Append("<div class='replies hidden'>");
				string text2 = "";
				string text3 = "";
				string text4 = "";
				for (int n = 0; n < eventData.Replys.Length; n++)
				{
					stringBuilder.Append("<div class='reply'>");
					if (eventData.Replys[n].RequiredClass != null)
					{
						stringBuilder.Append("<div class='eventClasses'>");
						stringBuilder.Append("<span class='");
						stringBuilder.Append(eventData.Replys[n].RequiredClass.CharacterName.ToLower());
						stringBuilder.Append("'>");
						stringBuilder.Append(eventData.Replys[n].RequiredClass.CharacterName);
						stringBuilder.Append("</span>");
						stringBuilder.Append("</div>");
					}
					else if (eventData.Replys[n].RepeatForAllCharacters)
					{
						stringBuilder.Append("<div class='eventClasses'>");
						stringBuilder.Append("<span class='allCharacters'>");
						stringBuilder.Append("All characters");
						stringBuilder.Append("</span>");
						stringBuilder.Append("</div>");
					}
					text2 = eventData.EventId + "_rp" + n;
					if (eventData.Replys[n].ReplyActionText != Enums.EventAction.None)
					{
						stringBuilder.Append("<label>[" + Enum.GetName(typeof(Enums.EventAction), eventData.Replys[n].ReplyActionText) + "]</label> ");
					}
					stringBuilder.Append(Texts.Instance.GetText(text2, "events"));
					if (Texts.Instance.GetText(text2, "events") == "")
					{
						stringBuilder.Append("[missing] " + eventData.Replys[n].ReplyText);
					}
					if (eventData.Replys[n].SsRoll)
					{
						stringBuilder.Append("<span class='roll'>Roll ");
						stringBuilder.Append(Enum.GetName(typeof(Enums.RollTarget), eventData.Replys[n].SsRollTarget));
						stringBuilder.Append(" | ");
						if (eventData.Replys[n].SsRollCard != Enums.CardType.None)
						{
							stringBuilder.Append("Card ");
							stringBuilder.Append(Enum.GetName(typeof(Enums.CardType), eventData.Replys[n].SsRollCard));
						}
						else
						{
							stringBuilder.Append(Enum.GetName(typeof(Enums.RollMode), eventData.Replys[n].SsRollMode));
							stringBuilder.Append(" ");
							stringBuilder.Append(eventData.Replys[n].SsRollNumber);
						}
						stringBuilder.Append("</span>");
					}
					if (eventData.Replys[n].Requirement != null || (bool)eventData.Replys[n].RequirementItem)
					{
						stringBuilder.Append("<span class='roll'>Requeriment:");
						if (eventData.Replys[n].Requirement != null)
						{
							stringBuilder.Append(" ");
							stringBuilder.Append(eventData.Replys[n].Requirement.RequirementId);
						}
						if (eventData.Replys[n].RequirementItem != null)
						{
							stringBuilder.Append(" Item ");
							stringBuilder.Append(eventData.Replys[n].RequirementItem.Id);
						}
						stringBuilder.Append("</span>");
					}
					if (eventData.Replys[n].SsRemoveItemSlot != Enums.ItemSlot.None)
					{
						stringBuilder.Append("<span class='roll'>RemoveItemSlot: ");
						stringBuilder.Append(Enum.GetName(typeof(Enums.ItemSlot), eventData.Replys[n].SsRemoveItemSlot));
						stringBuilder.Append("</span>");
					}
					if (eventData.Replys[n].SsCorruptItemSlot != Enums.ItemSlot.None)
					{
						stringBuilder.Append("<span class='roll'>CorruptItemSlot: ");
						stringBuilder.Append(Enum.GetName(typeof(Enums.ItemSlot), eventData.Replys[n].SsCorruptItemSlot));
						stringBuilder.Append("</span>");
					}
					stringBuilder.Append("<a href='javascript:void(0);' class='showLink linkResults' onclick='toggleResults(this)'>Show results</a>");
					stringBuilder.Append("<div class='results hidden'>");
					text3 = text2 + "_s";
					text4 = Texts.Instance.GetText(text3, "events");
					if (text4 != "")
					{
						stringBuilder.Append("<div class='replyResult replySuccess'>");
						stringBuilder.Append("<span>");
						stringBuilder.Append("[Success] ");
						stringBuilder.Append("</span>");
						stringBuilder.Append(text4);
						stringBuilder.Append("<div class='replyReward'>");
						if (eventData.Replys[n].SsGoldReward != 0)
						{
							stringBuilder.Append("<span>Gold: </span>" + eventData.Replys[n].SsGoldReward + " ");
						}
						if (eventData.Replys[n].SsDustReward != 0)
						{
							stringBuilder.Append("<span>Dust: </span>" + eventData.Replys[n].SsDustReward + " ");
						}
						if (eventData.Replys[n].SsDustReward != 0)
						{
							stringBuilder.Append("<span>Supply: </span>" + eventData.Replys[n].SsSupplyReward + " ");
						}
						if (eventData.Replys[n].SsExperienceReward != 0)
						{
							stringBuilder.Append("<span>Experience: </span>" + eventData.Replys[n].SsExperienceReward + " ");
						}
						if (eventData.Replys[n].SsRequirementUnlock != null)
						{
							stringBuilder.Append("<span>Unlock: </span>" + eventData.Replys[n].SsRequirementUnlock.RequirementId + " ");
						}
						if (eventData.Replys[n].SsRequirementLock != null)
						{
							stringBuilder.Append("<span>Lock: </span>" + eventData.Replys[n].SsRequirementLock.RequirementId + " ");
						}
						if (eventData.Replys[n].SsAddCard1 != null)
						{
							stringBuilder.Append("<span>AddCard: </span>" + eventData.Replys[n].SsAddCard1.Id + " ");
						}
						stringBuilder.Append("</div>");
						stringBuilder.Append("</div>");
					}
					text3 = text2 + "_sc";
					text4 = Texts.Instance.GetText(text3, "events");
					if (text4 != "")
					{
						stringBuilder.Append("<div class='replyResult replySuccess'>");
						stringBuilder.Append("<span>");
						stringBuilder.Append("[Critical success] ");
						stringBuilder.Append("</span>");
						stringBuilder.Append(text4);
						stringBuilder.Append("<div class='replyReward'>");
						if (eventData.Replys[n].SscGoldReward != 0)
						{
							stringBuilder.Append("<span>Gold: </span>" + eventData.Replys[n].SscGoldReward + " ");
						}
						if (eventData.Replys[n].SscDustReward != 0)
						{
							stringBuilder.Append("<span>Dust: </span>" + eventData.Replys[n].SscDustReward + " ");
						}
						if (eventData.Replys[n].SscDustReward != 0)
						{
							stringBuilder.Append("<span>Supply: </span>" + eventData.Replys[n].SscSupplyReward + " ");
						}
						if (eventData.Replys[n].SscExperienceReward != 0)
						{
							stringBuilder.Append("<span>Experience: </span>" + eventData.Replys[n].SscExperienceReward + " ");
						}
						if (eventData.Replys[n].SscRequirementUnlock != null)
						{
							stringBuilder.Append("<span>Unlock: </span>" + eventData.Replys[n].SscRequirementUnlock.RequirementId + " ");
						}
						if (eventData.Replys[n].SscRequirementLock != null)
						{
							stringBuilder.Append("<span>Lock: </span>" + eventData.Replys[n].SscRequirementLock.RequirementId + " ");
						}
						if (eventData.Replys[n].SscAddCard1 != null)
						{
							stringBuilder.Append("<span>AddCard: </span>" + eventData.Replys[n].SscAddCard1.Id + " ");
						}
						stringBuilder.Append("</div>");
						stringBuilder.Append("</div>");
					}
					text3 = text2 + "_f";
					text4 = Texts.Instance.GetText(text3, "events");
					if (text4 != "")
					{
						stringBuilder.Append("<div class='replyResult replyFail'>");
						stringBuilder.Append("<span>");
						stringBuilder.Append("[Failure] ");
						stringBuilder.Append("</span>");
						stringBuilder.Append(text4);
						stringBuilder.Append("<div class='replyReward'>");
						if (eventData.Replys[n].FlGoldReward != 0)
						{
							stringBuilder.Append("<span>Gold: </span>" + eventData.Replys[n].FlGoldReward + " ");
						}
						if (eventData.Replys[n].FlDustReward != 0)
						{
							stringBuilder.Append("<span>Dust: </span>" + eventData.Replys[n].FlDustReward + " ");
						}
						if (eventData.Replys[n].FlDustReward != 0)
						{
							stringBuilder.Append("<span>Supply: </span>" + eventData.Replys[n].FlSupplyReward + " ");
						}
						if (eventData.Replys[n].FlExperienceReward != 0)
						{
							stringBuilder.Append("<span>Experience: </span>" + eventData.Replys[n].FlExperienceReward + " ");
						}
						if (eventData.Replys[n].FlRequirementUnlock != null)
						{
							stringBuilder.Append("<span>Unlock: </span>" + eventData.Replys[n].FlRequirementUnlock.RequirementId + " ");
						}
						if (eventData.Replys[n].FlRequirementLock != null)
						{
							stringBuilder.Append("<span>Lock: </span>" + eventData.Replys[n].FlRequirementLock.RequirementId + " ");
						}
						if (eventData.Replys[n].FlAddCard1 != null)
						{
							stringBuilder.Append("<span>AddCard: </span>" + eventData.Replys[n].FlAddCard1.Id + " ");
						}
						stringBuilder.Append("</div>");
						stringBuilder.Append("</div>");
					}
					text3 = text2 + "_fc";
					text4 = Texts.Instance.GetText(text3, "events");
					if (text4 != "")
					{
						stringBuilder.Append("<div class='replyResult replyFail'>");
						stringBuilder.Append("<span>");
						stringBuilder.Append("[Critical failure] ");
						stringBuilder.Append("</span>");
						stringBuilder.Append(text4);
						stringBuilder.Append("<div class='replyReward'>");
						if (eventData.Replys[n].FlcGoldReward != 0)
						{
							stringBuilder.Append("<span>Gold: </span>" + eventData.Replys[n].FlcGoldReward + " ");
						}
						if (eventData.Replys[n].FlcDustReward != 0)
						{
							stringBuilder.Append("<span>Dust: </span>" + eventData.Replys[n].FlcDustReward + " ");
						}
						if (eventData.Replys[n].FlcDustReward != 0)
						{
							stringBuilder.Append("<span>Supply: </span>" + eventData.Replys[n].FlcSupplyReward + " ");
						}
						if (eventData.Replys[n].FlcExperienceReward != 0)
						{
							stringBuilder.Append("<span>Experience: </span>" + eventData.Replys[n].FlcExperienceReward + " ");
						}
						if (eventData.Replys[n].FlcRequirementUnlock != null)
						{
							stringBuilder.Append("<span>Unlock: </span>" + eventData.Replys[n].FlcRequirementUnlock.RequirementId + " ");
						}
						if (eventData.Replys[n].FlcRequirementLock != null)
						{
							stringBuilder.Append("<span>Lock: </span>" + eventData.Replys[n].FlcRequirementLock.RequirementId + " ");
						}
						if (eventData.Replys[n].FlcAddCard1 != null)
						{
							stringBuilder.Append("<span>AddCard: </span>" + eventData.Replys[n].FlcAddCard1.Id + " ");
						}
						stringBuilder.Append("</div>");
						stringBuilder.Append("</div>");
					}
					stringBuilder.Append("</div>");
					stringBuilder.Append("</div>");
				}
				stringBuilder.Append("</div>");
				stringBuilder.Append("</div>");
			}
			stringBuilder.Append("</div>");
		}
		string text5 = "\n                </div>\n\n            <script>Ready();</script>\n            </body>\n            </html>\n        ";
		string text6 = stringBuilder.ToString();
		string contents = text + text6 + text5;
		try
		{
			File.WriteAllText(path, contents);
		}
		catch (Exception)
		{
		}
	}

	public static void ParseAll()
	{
		Debug.Log("--------------\nEventsParser\n--------------");
		SubClassData[] array = Resources.LoadAll<SubClassData>("SubClass");
		subclassList = new SortedList();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].MainCharacter)
			{
				subclassList.Add(array[i].CharacterName, array[i].CharacterName);
			}
		}
		EventData[] array2 = Resources.LoadAll<EventData>("World/Events");
		foreach (EventData eventData in array2)
		{
			if (!eventsData.ContainsKey(eventData.EventId))
			{
				eventsData.Add(eventData.EventId, eventData);
			}
		}
		foreach (KeyValuePair<string, EventData> eventsDatum in eventsData)
		{
			string eventZone = GetEventZone(eventsDatum.Value.EventId);
			if (!eventsByZone.ContainsKey(eventZone))
			{
				eventsByZone.Add(eventZone, new List<string>());
			}
			string eventId = eventsDatum.Value.EventId;
			if (!eventsByZone[eventZone].Contains(eventId))
			{
				eventsByZone[eventZone].Add(eventId);
			}
		}
		CreateHTMLFile();
	}

	private static string GetEventZone(string id)
	{
		if (id.StartsWith("e_aquar"))
		{
			return "Aquarfall";
		}
		if (id.StartsWith("e_forge"))
		{
			return "BlackForge";
		}
		if (id.StartsWith("e_challenge"))
		{
			return "Challenge";
		}
		if (id.StartsWith("e_dread"))
		{
			return "Dreadnought";
		}
		if (id.StartsWith("e_faen"))
		{
			return "Faeborg";
		}
		if (id.StartsWith("e_sewers"))
		{
			return "FrozenSewers";
		}
		if (id.StartsWith("e_pyr"))
		{
			return "Pyramid";
		}
		if (id.StartsWith("e_sahti"))
		{
			return "Sahti";
		}
		if (id.StartsWith("e_secta"))
		{
			return "Sectarium";
		}
		if (id.StartsWith("e_sen"))
		{
			return "Senenthia";
		}
		if (id.StartsWith("e_lair"))
		{
			return "SpiderLair";
		}
		if (id.StartsWith("e_ulmin"))
		{
			return "Ulminin";
		}
		if (id.StartsWith("e_upri"))
		{
			return "Uprising";
		}
		if (id.StartsWith("e_velka"))
		{
			return "Velkarath";
		}
		if (id.StartsWith("e_voidhigh"))
		{
			return "VoidHigh";
		}
		if (id.StartsWith("e_voidlow"))
		{
			return "VoidLow";
		}
		if (id.StartsWith("e_wolf"))
		{
			return "WolfWars";
		}
		if (id.StartsWith("e_sunken"))
		{
			return "SunkenTemple";
		}
		if (id.StartsWith("e_dream"))
		{
			return "Dreams";
		}
		return "";
	}
}
