using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
	public Transform viewportTransform;

	public Transform logTM;

	public TMP_Text headerTM;

	private List<string> consoleText = new List<string>();

	private Dictionary<string, string> consoleDict;

	private string key = "";

	public CardItem consoleCardItem;

	public Transform[] characterTransform;

	public Transform characterContainerTransform;

	public Transform cardContainerTransform;

	public Transform cardTransform;

	public TMP_Text characterContainerTitle;

	public Transform characterContainerMsgNone;

	private Image[] characterSprite;

	private Image[] characterSpriteSkull;

	private Image[] characterSpriteBg;

	private TMP_Text[] characterText;

	private Color32 colorBgHero;

	private Color32 colorBgNPC;

	private StringBuilder SBCardEvent = new StringBuilder();

	private string oldHeroCardEvent = "";

	private string oldEntryTo = "";

	private Coroutine DoLogCoroutine;

	private StringBuilder SBconsole = new StringBuilder();

	private HashSet<string> addedLogEntries = new HashSet<string>();

	private void Awake()
	{
		consoleDict = new Dictionary<string, string>();
		characterSprite = new Image[characterTransform.Length];
		characterSpriteSkull = new Image[characterTransform.Length];
		characterSpriteBg = new Image[characterTransform.Length];
		characterText = new TMP_Text[characterTransform.Length];
		for (int i = 0; i < characterTransform.Length; i++)
		{
			characterSpriteBg[i] = characterTransform[i].GetChild(0).GetComponent<Image>();
			characterSprite[i] = characterTransform[i].GetChild(1).GetComponent<Image>();
			characterText[i] = characterTransform[i].GetChild(2).GetComponent<TMP_Text>();
			characterSpriteSkull[i] = characterTransform[i].GetChild(3).GetComponent<Image>();
		}
		colorBgHero = new Color(0.1f, 0.21f, 0.32f, 0.7f);
		colorBgNPC = new Color(0.27f, 0.08f, 0.08f, 0.7f);
	}

	private void Start()
	{
		consoleCardItem.DisableTrail();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<size=40>");
		stringBuilder.Append(Texts.Instance.GetText("menuLog"));
		stringBuilder.Append("</size>");
		headerTM.text = stringBuilder.ToString();
	}

	public bool IsActive()
	{
		return base.gameObject.activeSelf;
	}

	public void Show(bool _state)
	{
		if (base.gameObject.activeSelf != _state)
		{
			base.gameObject.SetActive(_state);
		}
		if (_state)
		{
			DoLog();
		}
	}

	public void DoLog()
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		ShowHideCardTransform(_state: false);
		ShowHideCharacterTransform(_state: false);
		HideCharactersLog(0);
		List<string> list = new List<string>();
		StringBuilder stringBuilder = new StringBuilder();
		string text = "";
		foreach (KeyValuePair<string, LogEntry> item in MatchManager.Instance.LogDictionary)
		{
			if (!addedLogEntries.Add(item.Key))
			{
				continue;
			}
			if ((item.Value.logCardId == "" || item.Value.logActivation == Enums.EventActivation.TraitActivation || item.Value.logActivation == Enums.EventActivation.ItemActivation) && SBCardEvent.Length > 0 && !item.Key.StartsWith("toHand:") && !item.Key.StartsWith("toTopDeck:") && !item.Key.StartsWith("toBottomDeck:") && !item.Key.StartsWith("toRandomDeck:") && !item.Key.StartsWith("toDiscard:") && !item.Key.StartsWith("toVanish:"))
			{
				SBCardEvent.Append("</size></margin>");
				SBCardEvent.Append("<br>");
				list.Add(SBCardEvent.ToString());
				SBCardEvent.Clear();
				oldHeroCardEvent = "";
				oldEntryTo = "";
			}
			stringBuilder.Clear();
			if (item.Value.logActivation != Enums.EventActivation.None)
			{
				string arg = "";
				if (item.Value.logHeroName != "")
				{
					arg = item.Value.logHeroName;
				}
				else if (item.Value.logNPCName != "")
				{
					arg = item.Value.logNPCName;
				}
				if (item.Value.logActivation == Enums.EventActivation.BeginTurn)
				{
					if (item.Key.StartsWith("begineffects:"))
					{
						stringBuilder.Append(TimeStamp(item.Value.logDateTime));
						stringBuilder.Append(string.Format(Texts.Instance.GetText("consoleBeginTurn"), arg, item.Key));
						stringBuilder.Append("<br>");
					}
				}
				else if (item.Value.logActivation == Enums.EventActivation.EndTurn)
				{
					if (item.Key.StartsWith("status:"))
					{
						stringBuilder.Append("<line-height=30%><br><align=center><size=-5><sprite name=sep></size><space=80><sprite name=damage> <link=");
						stringBuilder.Append(item.Key);
						stringBuilder.Append("><color=#A1D8E5><u>");
						stringBuilder.Append(Texts.Instance.GetText("consoleCombatStatus"));
						stringBuilder.Append("</u></color></link>\u00a0 <sprite name=damage><size=-5><space=75><sprite name=sep></size></align><br></line-height>");
						stringBuilder.Append("<br>");
					}
					else if (item.Key.StartsWith("endeffects:"))
					{
						stringBuilder.Append(TimeStamp(item.Value.logDateTime));
						stringBuilder.Append(string.Format(Texts.Instance.GetText("consoleEndTurn"), arg, item.Key));
						stringBuilder.Append("<br>");
					}
				}
				else if (item.Value.logActivation == Enums.EventActivation.Killed)
				{
					stringBuilder.Append("<margin=50><size=-4>");
					stringBuilder.Append("<color=");
					stringBuilder.Append(Globals.Instance.ClassColor["injury"]);
					stringBuilder.Append(">");
					stringBuilder.Append(string.Format(Texts.Instance.GetText("characterDies"), arg));
					stringBuilder.Append("</color>");
					stringBuilder.Append("</size>");
					stringBuilder.Append("</margin>");
					stringBuilder.Append("<br>");
				}
				else if (item.Value.logActivation == Enums.EventActivation.Resurrect)
				{
					stringBuilder.Append(TimeStamp(item.Value.logDateTime));
					stringBuilder.Append("<color=");
					stringBuilder.Append(Globals.Instance.ClassColor["boon"]);
					stringBuilder.Append(">");
					stringBuilder.Append(string.Format(Texts.Instance.GetText("characterResurrects"), arg));
					stringBuilder.Append("</color>");
					stringBuilder.Append("<br>");
				}
				else if (item.Value.logActivation == Enums.EventActivation.BeginTurnAboutToDealCards)
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					foreach (KeyValuePair<string, LogResult> item2 in item.Value.logResult)
					{
						if ((item.Value.logHero != null && item.Value.logHero.Id != item2.Key) || item2.Value.logResultDict.Count <= 0)
						{
							continue;
						}
						foreach (KeyValuePair<string, int> item3 in item2.Value.logResultDict)
						{
							if (item3.Key == "inspire" || item3.Key == "stress")
							{
								stringBuilder2.Append("\u00a0 ");
								stringBuilder2.Append("<sprite name=");
								stringBuilder2.Append(item3.Key);
								stringBuilder2.Append(">");
								stringBuilder2.Append(item3.Value);
							}
						}
					}
					stringBuilder.Append(TimeStamp(item.Value.logDateTime));
					stringBuilder.Append(string.Format(Texts.Instance.GetText("consoleDraw"), item.Value.logAuxInt, stringBuilder2.ToString()));
					stringBuilder.Append("<br>");
				}
			}
			if (item.Value.logCardId != "")
			{
				if (item.Key.StartsWith("toHand:") || item.Key.StartsWith("toTopDeck:") || item.Key.StartsWith("toBottomDeck:") || item.Key.StartsWith("toRandomDeck:") || item.Key.StartsWith("toDiscard:") || item.Key.StartsWith("toVanish:"))
				{
					string text2 = item.Key;
					if (MatchManager.Instance.LogDictionary.ContainsKey(text2))
					{
						LogEntry logEntry = MatchManager.Instance.LogDictionary[text2];
						text = DoLogCard(text2, logEntry, logEntry.logCardId);
						string arg2 = ((item.Value.logHero != null) ? item.Value.logHero.SourceName : ((item.Value.logNPC != null) ? item.Value.logNPC.SourceName : ""));
						string text3 = ((item.Value.logHero != null) ? item.Value.logHero.InternalId : ((item.Value.logNPC != null) ? item.Value.logNPC.InternalId : ""));
						if (SBCardEvent.Length > 0 && oldEntryTo != "" && oldEntryTo != item.Key.Substring(0, 6))
						{
							oldEntryTo = "";
						}
						if (SBCardEvent.Length > 0 && oldHeroCardEvent == text3 && oldEntryTo != "")
						{
							SBCardEvent.Append(", ");
						}
						else
						{
							if (SBCardEvent.Length == 0)
							{
								SBCardEvent.Append("<margin=50><size=-4>");
							}
							else
							{
								SBCardEvent.Append("<br>");
							}
							if (item.Key.StartsWith("toHand:"))
							{
								SBCardEvent.Append(string.Format(Texts.Instance.GetText("consoleCardToHand"), arg2));
								SBCardEvent.Append("\u00a0 ");
							}
							else if (item.Key.StartsWith("toTopDeck:"))
							{
								SBCardEvent.Append(string.Format(Texts.Instance.GetText("consoleCardToTopDeck"), arg2));
								SBCardEvent.Append("\u00a0 ");
							}
							else if (item.Key.StartsWith("toBottomDeck:"))
							{
								SBCardEvent.Append(string.Format(Texts.Instance.GetText("consoleCardToBottomDeck"), arg2));
								SBCardEvent.Append("\u00a0 ");
							}
							else if (item.Key.StartsWith("toRandomDeck:"))
							{
								SBCardEvent.Append(string.Format(Texts.Instance.GetText("consoleCardToRandomDeck"), arg2));
								SBCardEvent.Append("\u00a0 ");
							}
							else if (item.Key.StartsWith("toDiscard:"))
							{
								SBCardEvent.Append(string.Format(Texts.Instance.GetText("consoleCardToDiscard"), arg2));
								SBCardEvent.Append("\u00a0 ");
							}
							else if (item.Key.StartsWith("toVanish:"))
							{
								SBCardEvent.Append(string.Format(Texts.Instance.GetText("consoleCardToVanish"), arg2));
								SBCardEvent.Append("\u00a0 ");
							}
							else if (item.Key.StartsWith("cardModification:"))
							{
								SBCardEvent.Append(string.Format(Texts.Instance.GetText("consoleCardModification"), arg2));
								SBCardEvent.Append("\u00a0 ");
							}
						}
						oldHeroCardEvent = text3;
						oldEntryTo = item.Key.Substring(0, 6);
						SBCardEvent.Append(text);
					}
				}
				else
				{
					if (SBCardEvent.Length > 0)
					{
						SBCardEvent.Append("</size></margin>");
						SBCardEvent.Append("<br>");
						list.Add(SBCardEvent.ToString());
						SBCardEvent.Clear();
						oldHeroCardEvent = "";
						oldEntryTo = "";
					}
					string text4 = item.Key;
					if (MatchManager.Instance.LogDictionary.ContainsKey(text4))
					{
						LogEntry logEntry2 = MatchManager.Instance.LogDictionary[text4];
						if (item.Key.StartsWith("cardModification:"))
						{
							text = DoLogCard(text4, logEntry2, logEntry2.logCardId);
							if (text != "")
							{
								stringBuilder.Append("<margin=50><size=-4>");
								stringBuilder.Append(string.Format(Texts.Instance.GetText("consoleCardModification"), item.Value.logHero.SourceName));
								stringBuilder.Append("\u00a0 ");
								stringBuilder.Append(text);
								stringBuilder.Append("</size></margin>");
								stringBuilder.Append("<br>");
							}
						}
						else
						{
							text = DoLogCard(text4, logEntry2);
							if (text != "")
							{
								stringBuilder.Append(text);
							}
						}
					}
				}
			}
			if (item.Key.StartsWith("round:"))
			{
				stringBuilder.Append(TimeStamp(item.Value.logDateTime));
				stringBuilder.Append("<color=orange>");
				stringBuilder.Append(string.Format(Texts.Instance.GetText("roundNumber"), item.Key.Replace("round:", "")));
				stringBuilder.Append("</color>");
				stringBuilder.Append("<line-height=20><br><br></line-height>");
			}
			if (stringBuilder.Length > 0)
			{
				list.Add(stringBuilder.ToString());
			}
		}
		if (SBCardEvent.Length > 0)
		{
			SBCardEvent.Append("</size></margin>");
			SBCardEvent.Append("<br>");
			list.Add(SBCardEvent.ToString());
			SBCardEvent.Clear();
			oldHeroCardEvent = "";
			oldEntryTo = "";
		}
		StringBuilder stringBuilder3 = new StringBuilder();
		if (list.Count > 0)
		{
			for (int num = list.Count - 1; num >= 0; num--)
			{
				stringBuilder3.Append(list[num]);
			}
		}
		TMP_Text component = Object.Instantiate(logTM, viewportTransform).GetComponent<TMP_Text>();
		component.transform.SetSiblingIndex(0);
		component.SetText(stringBuilder3);
	}

	public void ShowCard(string _key, string _title)
	{
		bool flag = false;
		bool flag2 = true;
		bool flag3 = true;
		if (_key.StartsWith("crd:"))
		{
			_key = _key.Replace("crd:", "");
		}
		if (_key.StartsWith("plain:"))
		{
			_key = _key.Replace("plain:", "");
		}
		if (_key.StartsWith("log:"))
		{
			_key = _key.Replace("log:", "");
		}
		if (_key.StartsWith("logTrait:"))
		{
			_key = _key.Replace("logTrait:", "");
			if (_key.ToLower().Contains("reaping") || _key.ToLower().Contains("replenishing"))
			{
				flag3 = false;
			}
		}
		if (_key.StartsWith("cardModification:") || _key.StartsWith("toHand:") || _key.StartsWith("toTopDeck:") || _key.StartsWith("toBottomDeck:") || _key.StartsWith("toRandomDeck:") || _key.StartsWith("toDiscard:") || _key.StartsWith("toVanish:"))
		{
			flag3 = false;
		}
		if (_key.StartsWith("status:"))
		{
			flag = true;
			flag2 = false;
		}
		if (_key.StartsWith("begineffects:") || _key.StartsWith("endeffects:"))
		{
			flag2 = false;
		}
		if (!MatchManager.Instance.LogDictionary.ContainsKey(_key))
		{
			return;
		}
		LogEntry logEntry = MatchManager.Instance.LogDictionary[_key];
		if (flag2)
		{
			if (logEntry.logActivation == Enums.EventActivation.TraitActivation)
			{
				flag2 = false;
			}
			else if (logEntry.logActivation != Enums.EventActivation.CorruptionBeginRound)
			{
				consoleCardItem.SetCard(logEntry.logCardId, deckScale: true, logEntry.logHero, logEntry.logNPC);
			}
			else
			{
				consoleCardItem.SetCard(logEntry.logCardId, deckScale: true, null, null, GetFromGlobal: true);
			}
			consoleCardItem.TopLayeringOrder("UI", 30200);
			consoleCardItem.SetLocalScale(new Vector3(1.2f, 1.2f, 1f));
			consoleCardItem.HideKeyNotes();
		}
		int num = 0;
		if (flag3)
		{
			foreach (KeyValuePair<string, LogResult> item in logEntry.logResult)
			{
				if (item.Value.logResultDict.Count <= 0 || (item.Value.logResultDict.ContainsKey("hp") && item.Value.logResultDict["hp"] == 0))
				{
					continue;
				}
				ShowCharactersLog(num);
				Character character = null;
				StringBuilder stringBuilder = new StringBuilder();
				character = MatchManager.Instance.GetHeroById(item.Key);
				characterSprite[num].sprite = item.Value.logResultSprite;
				characterSpriteSkull[num].gameObject.SetActive(value: false);
				stringBuilder.Append("<size=20>");
				if (character != null)
				{
					stringBuilder.Append(character.SourceName);
					characterSpriteBg[num].color = colorBgHero;
				}
				else
				{
					character = MatchManager.Instance.GetNPCById(item.Key);
					if (character != null)
					{
						stringBuilder.Append(character.SourceName);
						characterSpriteBg[num].color = colorBgNPC;
					}
				}
				stringBuilder.Append("</size><line-height=14><br></line-height><br>");
				bool flag4 = false;
				bool flag5 = false;
				StringBuilder stringBuilder2 = new StringBuilder();
				foreach (KeyValuePair<string, int> item2 in item.Value.logResultDict)
				{
					if (item2.Key == "hp")
					{
						stringBuilder2.Append("<nobr>");
						stringBuilder2.Append("<sprite name=heart>");
						stringBuilder2.Append("<space=3><size=-3>");
						if (!flag)
						{
							if (item2.Value > 0)
							{
								stringBuilder2.Append("<color=");
								stringBuilder2.Append(Globals.Instance.ClassColor["scout"]);
								stringBuilder2.Append(">");
								stringBuilder2.Append("+");
							}
							else
							{
								stringBuilder2.Append("<color=");
								stringBuilder2.Append(Globals.Instance.ClassColor["warrior"]);
								stringBuilder2.Append(">");
							}
							stringBuilder2.Append(item2.Value);
							stringBuilder2.Append("</color>");
						}
						else
						{
							stringBuilder2.Append(item2.Value);
						}
						stringBuilder2.Append("</size>");
						if (!flag)
						{
							stringBuilder2.Append(" <size=-5><color=#AAA>(");
							stringBuilder2.Append(item.Value.logResultDict["hpCurrent"]);
							stringBuilder2.Append(")</color></size>");
							if (item.Value.logResultDict["hpCurrent"] <= 0)
							{
								characterSpriteSkull[num].gameObject.SetActive(value: true);
							}
						}
						stringBuilder2.Append("</nobr>");
						flag4 = true;
					}
					else if (!(item2.Key == "hpCurrent") && !(item2.Key == "stealthbonus"))
					{
						if (flag4 || flag5)
						{
							stringBuilder2.Append("<space=5><size=15><color=#666><voffset=4>|</voffset></color></size><space=8>");
						}
						stringBuilder2.Append("<nobr><sprite name=");
						stringBuilder2.Append(item2.Key);
						stringBuilder2.Append(">");
						stringBuilder2.Append("<space=-3><size=-3><color=#AAA> ");
						if (item2.Value > 0 && !flag)
						{
							stringBuilder2.Append("+");
						}
						stringBuilder2.Append(item2.Value);
						stringBuilder2.Append("</color></size>");
						stringBuilder2.Append("</nobr>");
						flag5 = true;
					}
				}
				if (stringBuilder2.Length > 0)
				{
					stringBuilder.Append(stringBuilder2.ToString());
					characterText[num].text = stringBuilder.ToString();
					num++;
				}
			}
			HideCharactersLog(num);
		}
		if (flag2)
		{
			ShowHideCardTransform(_state: true);
		}
		else
		{
			ShowHideCardTransform(_state: false);
		}
		ShowHideCharacterTransform(flag3);
		if (num > 0)
		{
			characterContainerMsgNone.gameObject.SetActive(value: false);
		}
		else
		{
			characterContainerMsgNone.gameObject.SetActive(value: true);
		}
		characterContainerTitle.text = _title;
	}

	private void ShowCharactersLog(int _target)
	{
		if (!characterTransform[_target].gameObject.activeSelf)
		{
			characterTransform[_target].gameObject.SetActive(value: true);
		}
	}

	private void HideCharactersLog(int _base)
	{
		for (int i = _base; i < 8; i++)
		{
			if (characterTransform[i].gameObject.activeSelf)
			{
				characterTransform[i].gameObject.SetActive(value: false);
			}
		}
	}

	private void ShowHideCardTransform(bool _state)
	{
		if (cardTransform.gameObject.activeSelf != _state)
		{
			cardTransform.gameObject.SetActive(_state);
		}
		if (cardContainerTransform.gameObject.activeSelf != _state)
		{
			cardContainerTransform.gameObject.SetActive(_state);
		}
	}

	private void ShowHideCharacterTransform(bool _state)
	{
		if (characterContainerTransform.gameObject.activeSelf != _state)
		{
			characterContainerTransform.gameObject.SetActive(_state);
		}
	}

	public void HideLogCard()
	{
		PopupManager.Instance.ClosePopup();
		ShowHideCardTransform(_state: false);
		ShowHideCharacterTransform(_state: false);
	}

	public string DoLogCard(string _key, LogEntry _logEntry, string _cardId = "")
	{
		StringBuilder stringBuilder = new StringBuilder();
		string text = "";
		if (_logEntry.logActivation == Enums.EventActivation.CorruptionBeginRound)
		{
			stringBuilder.Append("<margin=50>");
			stringBuilder.Append("<size=-4>");
			stringBuilder.Append("<color=");
			stringBuilder.Append(Globals.Instance.ClassColor["magicknight"]);
			stringBuilder.Append(">");
			stringBuilder.Append(Texts.Instance.GetText("consoleCorruptionActivated"));
			stringBuilder.Append("</color>  <sprite name=cards><link=log:{1}><u>{2}</u></link>");
			stringBuilder.Append("</size>");
			stringBuilder.Append("</margin>");
			stringBuilder.Append("<br>");
			if (_logEntry.logHero == null && _logEntry.logNPC == null)
			{
				stringBuilder.Append("<line-height=20%><br></line-height>");
			}
		}
		else if (_logEntry.logActivation == Enums.EventActivation.ItemActivation)
		{
			if (_logEntry.logHero != null || _logEntry.logNPC != null)
			{
				stringBuilder.Append("<line-height=20%><br></line-height>");
			}
			stringBuilder.Append("<margin=50>");
			stringBuilder.Append("<size=-4>");
			stringBuilder.Append("<color=#A29279>");
			stringBuilder.Append(Texts.Instance.GetText("consoleItemActivated"));
			stringBuilder.Append("</color>  <sprite name=cards><link=log:{1}><u>{2}</u></link>");
			stringBuilder.Append("</size>");
			stringBuilder.Append("</margin>");
			stringBuilder.Append("<br>");
		}
		else if (_logEntry.logActivation == Enums.EventActivation.TraitActivation)
		{
			if (_logEntry.logHero != null || _logEntry.logNPC != null)
			{
				stringBuilder.Append("<line-height=20%><br></line-height>");
			}
			stringBuilder.Append("<margin=50>");
			stringBuilder.Append("<size=-4>");
			stringBuilder.Append("<color=#A29279>");
			stringBuilder.Append(Texts.Instance.GetText("consoleTraitActivated"));
			stringBuilder.Append("</color>  <sprite name=experience><link=logTrait:{1}><u>{2}</u></link>");
			stringBuilder.Append("</size>");
			stringBuilder.Append("</margin>");
			stringBuilder.Append("<br>");
		}
		else if (_cardId != "")
		{
			stringBuilder.Append("<sprite name=cards><link=log:{1}><u>{2}</u></link>");
		}
		else
		{
			stringBuilder.Append(TimeStamp(_logEntry.logDateTime));
			stringBuilder.Append(Texts.Instance.GetText("consoleCharacterCasted"));
			stringBuilder.Append(" <sprite name=cards><link=log:{1}><u>{2}</u></link>");
			stringBuilder.Append("<br>");
		}
		text = stringBuilder.ToString();
		stringBuilder.Clear();
		string arg = "";
		if (_logEntry.logHero != null)
		{
			arg = _logEntry.logHero.SourceName;
		}
		else if (_logEntry.logNPC != null)
		{
			arg = _logEntry.logNPC.SourceName;
		}
		StringBuilder stringBuilder2 = new StringBuilder();
		if (_logEntry.logActivation == Enums.EventActivation.TraitActivation)
		{
			TraitData traitData = Globals.Instance.GetTraitData(_logEntry.logCardId);
			if (traitData != null)
			{
				stringBuilder2.Append(traitData.TraitName);
			}
		}
		else if (_logEntry.logCardId != "")
		{
			CardData cardData = Globals.Instance.GetCardData(_logEntry.logCardId, instantiate: false);
			if (cardData != null)
			{
				stringBuilder2.Append("<color=#");
				if (cardData.CardUpgraded == Enums.CardUpgraded.Rare)
				{
					stringBuilder2.Append(ColorUtility.ToHtmlStringRGBA(Globals.Instance.ColorColor["purple"]));
				}
				else if (cardData.CardUpgraded == Enums.CardUpgraded.A)
				{
					stringBuilder2.Append(ColorUtility.ToHtmlStringRGBA(Globals.Instance.ColorColor["blueCardTitle"]));
				}
				else if (cardData.CardUpgraded == Enums.CardUpgraded.B)
				{
					stringBuilder2.Append(ColorUtility.ToHtmlStringRGBA(Globals.Instance.ColorColor["goldCardTitle"]));
				}
				else
				{
					stringBuilder2.Append(ColorUtility.ToHtmlStringRGBA(Globals.Instance.ColorColor["white"]));
				}
				stringBuilder2.Append(">");
				stringBuilder2.Append(cardData.CardName);
				stringBuilder2.Append("</color>");
			}
		}
		return string.Format(text, arg, _key, stringBuilder2);
	}

	public string GetText(string key)
	{
		if (consoleDict == null)
		{
			return "";
		}
		if (!consoleDict.TryGetValue(key, out var _))
		{
			return "";
		}
		return consoleDict[key];
	}

	public void SetKey(string _key)
	{
		key = _key;
	}

	public void LogNew(LogItem logItem)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (logItem.LogType == Enums.LogType.Text)
		{
			stringBuilder.Append(logItem.text);
		}
		else
		{
			string value = "#8D3901";
			if (GetText(key).LastIndexOf(logItem.CasterName + "]</color> <color=#FFCC00>" + logItem.SkillName) == -1)
			{
				if (logItem.ClassName != "")
				{
					value = Globals.Instance.ClassColor[logItem.ClassName];
				}
				stringBuilder.Append("<line-height=125%><size=24>");
				stringBuilder.Append("<color=");
				stringBuilder.Append(value);
				stringBuilder.Append(">[");
				stringBuilder.Append(logItem.CasterName);
				stringBuilder.Append("]</color> <color=#FFCC00>");
				stringBuilder.Append(logItem.SkillName);
				stringBuilder.Append("</color></size><br>");
			}
			if (logItem.LogType == Enums.LogType.Damage)
			{
				stringBuilder.Append("<line-height=65%><size=24><b>");
				if (logItem.Invulnerable || logItem.Evaded)
				{
					stringBuilder.Append("0");
				}
				else
				{
					stringBuilder.Append(logItem.DamageFinal.ToString());
				}
				stringBuilder.Append(" ");
				if (logItem.DamageType != Enums.DamageType.None)
				{
					stringBuilder.Append(logItem.DamageType);
				}
				else
				{
					stringBuilder.Append("damage");
				}
				stringBuilder.Append("</b><color=#AAA> to ");
				stringBuilder.Append(logItem.TargetName);
				stringBuilder.Append("</color></size><br><color=#999>____________</color></line-height>\n");
				string text = "<color=#FFCC00>" + logItem.DamagePre + " ";
				text = ((logItem.DamageType == Enums.DamageType.None) ? (text + "damage") : (text + logItem.DamageType));
				text += "</color> ";
				if (logItem.Invulnerable)
				{
					stringBuilder.Append(text);
					stringBuilder.Append("<color=#00E6FF>*Invulnerable*</color><br>");
				}
				else if (logItem.Evaded)
				{
					stringBuilder.Append(text);
					stringBuilder.Append("<color=#00E6FF>*Evaded*</color><br>");
				}
				else if (logItem.DamageBlocked > 0 && logItem.DamageBlocked == logItem.DamagePre)
				{
					stringBuilder.Append(text);
					stringBuilder.Append("<color=#00E6FF>*Blocked*</color><br>");
				}
				else if (logItem.Immune)
				{
					stringBuilder.Append(text);
					stringBuilder.Append("<color=#00E6FF>*Immune*</color>");
				}
				else
				{
					stringBuilder.Append("<pos=16>");
					stringBuilder.Append(text);
					if (logItem.DamageAuraCurse > 0)
					{
						stringBuilder.Append(" <color=#FBF>*defended ");
						if (logItem.DamageAuraCurse > 0)
						{
							stringBuilder.Append("-");
						}
						else
						{
							stringBuilder.Append("-");
						}
						stringBuilder.Append(Mathf.Abs(logItem.DamageAuraCurse));
						stringBuilder.Append("*</color>");
						stringBuilder.Append(" => ");
						stringBuilder.Append(logItem.DamagePostAuraCurse);
						stringBuilder.Append("Hp");
						stringBuilder.Append("\n");
						stringBuilder.Append("<pos=16>");
						stringBuilder.Append(" <color=#F3404E>");
						stringBuilder.Append(logItem.DamagePostAuraCurse);
						stringBuilder.Append("Hp");
						stringBuilder.Append("</color>");
					}
					if (logItem.DamageBlocked > 0)
					{
						stringBuilder.Append(" <color=#00E6FF>*blocked ");
						stringBuilder.Append(logItem.DamageBlocked.ToString());
						stringBuilder.Append("*</color>");
						stringBuilder.Append(" => ");
						stringBuilder.Append((logItem.DamagePre - logItem.DamageBlocked).ToString());
						stringBuilder.Append(" Hp");
						stringBuilder.Append("\n");
						stringBuilder.Append("<pos=16>");
						stringBuilder.Append(" <color=#F3404E>");
						stringBuilder.Append((logItem.DamagePre - logItem.DamageBlocked).ToString());
						stringBuilder.Append(" Hp");
						stringBuilder.Append("</color>");
					}
					stringBuilder.Append(" <color=#FF9B34>");
					stringBuilder.Append(logItem.DamageResist.ToString());
					stringBuilder.Append("% resist</color>");
					stringBuilder.Append(" => ");
					stringBuilder.Append("<color=white>");
					stringBuilder.Append(logItem.DamageFinal);
					stringBuilder.Append(" Hp</color>");
				}
			}
		}
		if (!consoleDict.TryGetValue(key, out var _))
		{
			consoleDict.Add(key, stringBuilder.ToString());
		}
		else
		{
			consoleDict[key] = consoleDict[key] + "\n\n" + stringBuilder.ToString();
		}
	}

	private string TimeStamp(string _date)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<size=-4><voffset=1><color=#666>[");
		stringBuilder.Append(_date);
		stringBuilder.Append("]</color></voffset></size> ");
		return stringBuilder.ToString();
	}
}
