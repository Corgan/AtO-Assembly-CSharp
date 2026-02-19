using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class Reply : MonoBehaviour
{
	public Transform replyContainer;

	public Transform eventTrackContainer;

	public GameObject eventTrackPrefab;

	public SpriteRenderer replyImage;

	public TMP_Text replyText;

	public Transform probDice;

	public PopupText probPopup;

	public Transform replyChar0;

	public Transform replyChar1;

	public Transform replyChar2;

	public Transform replyChar3;

	public Transform replyButtonBlocked;

	public SpriteRenderer replyBg;

	public SpriteRenderer replyBgButton;

	private SpriteRenderer replyChar0Spr;

	private SpriteRenderer replyChar1Spr;

	private SpriteRenderer replyChar2Spr;

	private SpriteRenderer replyChar3Spr;

	public Transform replyRoll;

	public TMP_Text replyRollText;

	public Transform replyRollBox;

	public TMP_Text replyBoxText;

	public TMP_Text dlcText;

	private Color colorButton;

	private Color colorText;

	private Color colorHover;

	private Color colorOff;

	private Animator anim;

	private CardItem CI;

	private EventReplyData eventReplyData;

	private int optionIndex;

	private bool selected;

	private bool thereIsRoll;

	private bool blocked;

	private bool showGoldMsg;

	private int modRollMadness;

	private void Awake()
	{
		colorText = replyText.color;
		colorButton = replyBg.color;
		colorOff = new Color(colorButton.r, colorButton.g, colorButton.b, 0.2f);
		colorHover = new Color(colorButton.r, colorButton.g, colorButton.b, 0.7f);
		replyBgButton.color = colorOff;
		replyChar0Spr = replyChar0.GetComponent<SpriteRenderer>();
		replyChar1Spr = replyChar1.GetComponent<SpriteRenderer>();
		replyChar2Spr = replyChar2.GetComponent<SpriteRenderer>();
		replyChar3Spr = replyChar3.GetComponent<SpriteRenderer>();
		CI = GetComponent<CardItem>();
	}

	public int GetOptionIndex()
	{
		return optionIndex;
	}

	public void DisableOption()
	{
		blocked = true;
	}

	public void Block(bool _showRedLayer = true, bool _showGoldShardMessage = true)
	{
		blocked = true;
		if (_showGoldShardMessage)
		{
			showGoldMsg = _showRedLayer;
		}
		if (_showRedLayer)
		{
			replyButtonBlocked.gameObject.SetActive(value: true);
		}
	}

	public void SetImage(Sprite image)
	{
		replyImage.sprite = image;
	}

	public EventReplyData GetEventReplyData()
	{
		return eventReplyData;
	}

	public void Init(string _eventId, int _replyIndex, int _replyOrder)
	{
		anim = GetComponent<Animator>();
		optionIndex = _replyIndex;
		EventData eventData = Globals.Instance.GetEventData(_eventId);
		eventReplyData = eventData.Replys[_replyIndex];
		if (AtOManager.Instance.GetNgPlus() >= 4 || AtOManager.Instance.IsChallengeTraitActive("unlucky"))
		{
			modRollMadness = 1;
		}
		else if (AtOManager.Instance.IsChallengeTraitActive("lucky"))
		{
			modRollMadness = -1;
		}
		bool flag = false;
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		string text = "";
		probDice.gameObject.SetActive(value: false);
		if (eventReplyData.ReplyActionText != Enums.EventAction.None)
		{
			string text2 = "";
			if (eventReplyData.ReplyActionText == Enums.EventAction.CharacterName)
			{
				if (eventReplyData.RequiredClass != null)
				{
					SubClassData subClassData = Globals.Instance.GetSubClassData(eventReplyData.RequiredClass.Id);
					replyImage.sprite = subClassData.SpriteBorderSmall;
					replyText.margin = new Vector4(0.4f, 0.05f, 0f, 0.05f);
					flag = true;
				}
			}
			else
			{
				text2 = Enum.GetName(typeof(Enums.EventAction), eventReplyData.ReplyActionText) + "Action";
				text2 = Texts.Instance.GetText(text2);
			}
			if (text2 != "")
			{
				stringBuilder.Append("<color=#333>[");
				stringBuilder.Append(text2);
				if (eventReplyData.ReplyActionText == Enums.EventAction.Rest)
				{
					stringBuilder.Append(" <size=-.4><color=#11490E>+");
					stringBuilder.Append(eventReplyData.SsRewardHealthPercent.ToString());
					if (Functions.SpaceBeforePercentSign())
					{
						stringBuilder.Append(" ");
					}
					stringBuilder.Append("% <sprite name=heart></color></size>");
				}
				stringBuilder.Append("]</color> ");
			}
		}
		StringBuilder stringBuilder3 = new StringBuilder();
		stringBuilder3.Append(eventData.EventId);
		stringBuilder3.Append("_rp");
		stringBuilder3.Append(eventReplyData.IndexForAnswerTranslation);
		text = Texts.Instance.GetText(stringBuilder3.ToString(), "events");
		stringBuilder3 = null;
		if (text != "")
		{
			stringBuilder.Append(text);
			if (eventReplyData.RequirementSku != "" && !SteamManager.Instance.PlayerHaveDLC(eventReplyData.RequirementSku))
			{
				dlcText.gameObject.SetActive(value: true);
				dlcText.text = string.Format(Texts.Instance.GetText("requiredDLC").Replace("#FFF", "#46291A"), SteamManager.Instance.GetDLCName(eventReplyData.RequirementSku));
			}
		}
		else
		{
			stringBuilder.Append(eventReplyData.ReplyText);
		}
		if (flag)
		{
			stringBuilder.Insert(0, '"');
			stringBuilder.Append('"');
		}
		replyText.text = stringBuilder.ToString();
		replyText.text = ParseTextToApplyNumericModifications(replyText.text);
		PopupText component = replyRoll.GetComponent<PopupText>();
		if (eventReplyData.SsRoll)
		{
			replyRoll.gameObject.SetActive(value: true);
			replyRollText.text = "";
			string text3 = Enum.GetName(typeof(Enums.RollMode), eventReplyData.SsRollMode);
			string text4 = Enum.GetName(typeof(Enums.RollTarget), eventReplyData.SsRollTarget);
			stringBuilder.Clear();
			stringBuilder.Append("<size=6><sprite name=cards></size><voffset=.7>");
			if (eventReplyData.SsRollTarget != Enums.RollTarget.Character)
			{
				stringBuilder.Append(Texts.Instance.GetText(text4));
				stringBuilder.Append("  ");
			}
			stringBuilder.Append("<color=#CCC>[");
			StringBuilder stringBuilder4 = new StringBuilder();
			if (eventReplyData.SsRollCard != Enums.CardType.None)
			{
				stringBuilder4.Append("<color=#FFAA00><size=+.2>");
				stringBuilder4.Append(Texts.Instance.GetText(Enum.GetName(typeof(Enums.CardType), eventReplyData.SsRollCard)));
				stringBuilder4.Append("</size></color>");
				stringBuilder.Append(string.Format(Texts.Instance.GetText("rollCard"), stringBuilder4.ToString()));
				stringBuilder2.Append(EventManager.Instance.GetProbabilityType(eventReplyData.SsRollCard, eventReplyData.RequiredClass.Id));
				if (Functions.SpaceBeforePercentSign())
				{
					stringBuilder.Append(" ");
				}
				stringBuilder2.Append("%");
			}
			else if (text3 == "HigherOrEqual" || text3 == "LowerOrEqual")
			{
				stringBuilder4.Append("<color=#FFAA00><b><size=+.2>");
				int ssRollNumber = eventReplyData.SsRollNumber;
				ssRollNumber = ((!(text3 == "HigherOrEqual")) ? (ssRollNumber - modRollMadness) : (ssRollNumber + modRollMadness));
				stringBuilder4.Append(ssRollNumber);
				stringBuilder4.Append("</size></b></color>");
				stringBuilder.Append(string.Format(Texts.Instance.GetText(text3), stringBuilder4.ToString()));
				if (text4 == "Single")
				{
					Hero[] heroes = EventManager.Instance.Heroes;
					if (heroes[0] != null && heroes[0].HeroData != null)
					{
						stringBuilder2.Append("<size=+6><color=#AAA>");
						stringBuilder2.Append(heroes[0].SourceName);
						stringBuilder2.Append("</color></size> ");
						stringBuilder2.Append(EventManager.Instance.GetProbabilitySingle(ssRollNumber, text3 == "HigherOrEqual", 0));
						if (Functions.SpaceBeforePercentSign())
						{
							stringBuilder2.Append(" ");
						}
						stringBuilder2.Append("%");
						stringBuilder2.Append("<br>");
					}
					if (heroes[1] != null && heroes[1].HeroData != null)
					{
						stringBuilder2.Append("<size=+6><color=#AAA>");
						stringBuilder2.Append(heroes[1].SourceName);
						stringBuilder2.Append("</color></size> ");
						stringBuilder2.Append(EventManager.Instance.GetProbabilitySingle(ssRollNumber, text3 == "HigherOrEqual", 1));
						if (Functions.SpaceBeforePercentSign())
						{
							stringBuilder2.Append(" ");
						}
						stringBuilder2.Append("%");
						stringBuilder2.Append("<br>");
					}
					if (heroes[2] != null && heroes[2].HeroData != null)
					{
						stringBuilder2.Append("<size=+6><color=#AAA>");
						stringBuilder2.Append(heroes[2].SourceName);
						stringBuilder2.Append("</color></size> ");
						stringBuilder2.Append(EventManager.Instance.GetProbabilitySingle(ssRollNumber, text3 == "HigherOrEqual", 2));
						if (Functions.SpaceBeforePercentSign())
						{
							stringBuilder2.Append(" ");
						}
						stringBuilder2.Append("%");
						stringBuilder2.Append("<br>");
					}
					if (heroes[3] != null && heroes[3].HeroData != null)
					{
						stringBuilder2.Append("<size=+6><color=#AAA>");
						stringBuilder2.Append(heroes[3].SourceName);
						stringBuilder2.Append("</color></size> ");
						stringBuilder2.Append(EventManager.Instance.GetProbabilitySingle(ssRollNumber, text3 == "HigherOrEqual", 3));
						if (Functions.SpaceBeforePercentSign())
						{
							stringBuilder2.Append(" ");
						}
						stringBuilder2.Append("%");
					}
				}
				else if (text4 == "Character")
				{
					Hero[] heroes2 = EventManager.Instance.Heroes;
					int num = 0;
					for (int i = 0; i < heroes2.Length; i++)
					{
						if (heroes2[i].SubclassName.ToLower().Replace(" ", "") == eventReplyData.RequiredClass.SubClassName.ToLower().Replace(" ", ""))
						{
							num = i;
							break;
						}
					}
					stringBuilder2.Append("<size=+6><color=#AAA>");
					stringBuilder2.Append(heroes2[num].SourceName);
					stringBuilder2.Append("</color></size> ");
					stringBuilder2.Append(EventManager.Instance.GetProbabilitySingle(ssRollNumber, text3 == "HigherOrEqual", num));
					if (Functions.SpaceBeforePercentSign())
					{
						stringBuilder2.Append(" ");
					}
					stringBuilder2.Append("%");
				}
				else
				{
					stringBuilder2.Append(EventManager.Instance.GetProbability(ssRollNumber, text3 == "HigherOrEqual"));
					if (Functions.SpaceBeforePercentSign())
					{
						stringBuilder2.Append(" ");
					}
					stringBuilder2.Append("%");
				}
			}
			else
			{
				stringBuilder.Append(Texts.Instance.GetText(text3));
			}
			stringBuilder.Append("] ");
			if (stringBuilder2.Length > 0)
			{
				stringBuilder2.Insert(0, "<br><color=#FFAA00><size=+10>");
				probPopup.text = "<size=+3>" + string.Format(Texts.Instance.GetText("replyProbability"), stringBuilder2.ToString());
				probDice.gameObject.SetActive(value: true);
			}
			replyRollText.text = stringBuilder.ToString();
			switch (text4)
			{
			case "Single":
				component.SetId("singleDesc");
				break;
			case "Group":
				component.SetId("groupDesc");
				break;
			case "Competition":
				component.SetId("competitionDesc");
				break;
			}
			thereIsRoll = true;
		}
		else
		{
			replyRoll.gameObject.SetActive(value: false);
			UnityEngine.Object.Destroy(component);
		}
		ShowRequirementTracks();
		replyRollBox.gameObject.SetActive(value: false);
		if (thereIsRoll && !GameManager.Instance.TutorialWatched("eventRolls"))
		{
			GameManager.Instance.ShowTutorialPopup("eventRolls", replyRollText.transform.position, Vector3.zero);
		}
	}

	private string ParseTextToApplyNumericModifications(string input)
	{
		if ((eventReplyData.SsGoldReward > 0 || eventReplyData.SscGoldReward > 0 || eventReplyData.FlGoldReward > 0 || eventReplyData.FlcGoldReward > 0) && eventReplyData.GoldCost == 0 && eventReplyData.DustCost == 0)
		{
			float modifier = 1f;
			if (MadnessManager.Instance.IsMadnessTraitActive("poverty") || AtOManager.Instance.IsChallengeTraitActive("poverty"))
			{
				if (GameManager.Instance.IsObeliskChallenge())
				{
					modifier = 0.3f;
				}
				else
				{
					modifier = 0.5f;
				}
			}
			float result;
			return new Regex("\\b\\d+(?:[,.]\\d+)?g\\b").Replace(input, (Match m) => float.TryParse(m.Value.Replace("g", "").Replace(".", "").Replace(",", ""), out result) ? ((result * modifier).ToString("#,##0") + "g") : m.Value);
		}
		return input;
	}

	private IEnumerator ShowAnim(int _replyOrder)
	{
		if (thereIsRoll && !GameManager.Instance.TutorialWatched("eventRolls"))
		{
			GameManager.Instance.ShowTutorialPopup("eventRolls", replyRollText.transform.position, Vector3.zero);
		}
		yield return null;
	}

	private void ShowRequirementTracks()
	{
		if (!(eventReplyData.SsNodeTravel != null))
		{
			return;
		}
		List<string> playerRequeriments = AtOManager.Instance.GetPlayerRequeriments();
		for (int i = 0; i < playerRequeriments.Count; i++)
		{
			EventRequirementData requirementData = Globals.Instance.GetRequirementData(playerRequeriments[i]);
			if (requirementData.RequirementTrack && requirementData.CanShowRequeriment(Enums.Zone.None, AtOManager.Instance.GetMapZone(eventReplyData.SsNodeTravel.NodeId)))
			{
				GameObject obj = UnityEngine.Object.Instantiate(eventTrackPrefab, Vector3.zero, Quaternion.identity, eventTrackContainer);
				obj.transform.localPosition = new Vector3(0f, 0f, -3f);
				obj.transform.GetComponent<EventTrack>().SetTrack(playerRequeriments[i]);
			}
		}
	}

	public void SelectedByMultiplayer(string nick)
	{
		if (!replyChar0.gameObject.activeSelf)
		{
			replyChar0.gameObject.SetActive(value: true);
			replyChar0Spr.color = Functions.HexToColor(NetworkManager.Instance.GetColorFromNick(nick));
		}
		else if (!replyChar1.gameObject.activeSelf)
		{
			replyChar1.gameObject.SetActive(value: true);
			replyChar1Spr.color = Functions.HexToColor(NetworkManager.Instance.GetColorFromNick(nick));
		}
		else if (!replyChar2.gameObject.activeSelf)
		{
			replyChar2.gameObject.SetActive(value: true);
			replyChar2Spr.color = Functions.HexToColor(NetworkManager.Instance.GetColorFromNick(nick));
		}
		else if (!replyChar3.gameObject.activeSelf)
		{
			replyChar3.gameObject.SetActive(value: true);
			replyChar3Spr.color = Functions.HexToColor(NetworkManager.Instance.GetColorFromNick(nick));
		}
	}

	public void SetRollBox(string rollText)
	{
		if (eventReplyData.SsRollCard == Enums.CardType.None)
		{
			replyBoxText.text = rollText;
			replyRollBox.gameObject.SetActive(value: true);
		}
		else
		{
			HideRollBox();
		}
	}

	public void HideRollBox()
	{
		replyRollBox.gameObject.SetActive(value: false);
	}

	private void ShowSelectedDesign()
	{
		TMP_Text tMP_Text = replyText;
		Color color = (replyImage.color = new Color(1f, 1f, 1f, 1f));
		tMP_Text.color = color;
		replyBgButton.color = colorHover;
	}

	private void OnMouseEnter()
	{
		if (AlertManager.Instance.IsActive() || GameManager.Instance.IsTutorialActive() || SettingsManager.Instance.IsActive() || DamageMeterManager.Instance.IsActive() || (MapManager.Instance.characterWindow.gameObject.activeSelf && MapManager.Instance.characterWindow.IsActive()))
		{
			return;
		}
		if (blocked)
		{
			if (showGoldMsg)
			{
				PopupManager.Instance.SetText(Texts.Instance.GetText("notEnoughGold"), fast: true);
			}
		}
		else
		{
			GameManager.Instance.SetCursorHover();
			ShowSelectedDesign();
		}
		if (eventReplyData.ReplyShowCard != null && !blocked)
		{
			CI.cardoutsideverticallist = true;
			CI.CreateAmplifyOutsideCard(Globals.Instance.GetCardData(eventReplyData.ReplyShowCard.Id, instantiate: false), GetComponent<BoxCollider2D>());
		}
		else if (eventReplyData.SsCorruptItemSlot != Enums.ItemSlot.None && !blocked)
		{
			SubClassData subClassData = Globals.Instance.GetSubClassData(eventReplyData.RequiredClass.Id);
			if (!(subClassData != null))
			{
				return;
			}
			Hero[] heroes = EventManager.Instance.Heroes;
			int num = -1;
			for (int i = 0; i < heroes.Length; i++)
			{
				if (heroes[i] != null && heroes[i].HeroData != null && heroes[i].HeroData.HeroSubClass.Id == subClassData.Id)
				{
					num = i;
					break;
				}
			}
			if (num > -1)
			{
				CardData cardData = null;
				if (eventReplyData.SsCorruptItemSlot == Enums.ItemSlot.Weapon && heroes[num].Weapon != "")
				{
					cardData = Functions.GetCardDataFromCardData(Globals.Instance.GetCardData(heroes[num].Weapon, instantiate: false), "");
				}
				else if (eventReplyData.SsCorruptItemSlot == Enums.ItemSlot.Armor && heroes[num].Armor != "")
				{
					cardData = Functions.GetCardDataFromCardData(Globals.Instance.GetCardData(heroes[num].Armor, instantiate: false), "");
				}
				else if (eventReplyData.SsCorruptItemSlot == Enums.ItemSlot.Jewelry && heroes[num].Jewelry != "")
				{
					cardData = Functions.GetCardDataFromCardData(Globals.Instance.GetCardData(heroes[num].Jewelry, instantiate: false), "");
				}
				else if (eventReplyData.SsCorruptItemSlot == Enums.ItemSlot.Accesory && heroes[num].Accesory != "")
				{
					cardData = Functions.GetCardDataFromCardData(Globals.Instance.GetCardData(heroes[num].Accesory, instantiate: false), "");
				}
				else if (eventReplyData.SsCorruptItemSlot == Enums.ItemSlot.Pet && heroes[num].Pet != "")
				{
					cardData = Functions.GetCardDataFromCardData(Globals.Instance.GetCardData(heroes[num].Pet, instantiate: false), "");
				}
				if (cardData != null && cardData.UpgradesToRare != null)
				{
					CI.cardoutsideverticallist = true;
					CI.CreateAmplifyOutsideCard(Globals.Instance.GetCardData(cardData.UpgradesToRare.Id, instantiate: false), GetComponent<BoxCollider2D>(), heroes[num]);
				}
			}
		}
		else
		{
			if (eventReplyData.SsRemoveItemSlot == Enums.ItemSlot.None || blocked)
			{
				return;
			}
			SubClassData subClassData2 = Globals.Instance.GetSubClassData(eventReplyData.RequiredClass.Id);
			if (!(subClassData2 != null))
			{
				return;
			}
			Hero[] heroes2 = EventManager.Instance.Heroes;
			int num2 = -1;
			for (int j = 0; j < heroes2.Length; j++)
			{
				if (heroes2[j] != null && heroes2[j].HeroData != null && heroes2[j].HeroData.HeroSubClass.Id == subClassData2.Id)
				{
					num2 = j;
					break;
				}
			}
			if (num2 > -1)
			{
				CardData cardData2 = null;
				if (eventReplyData.SsRemoveItemSlot == Enums.ItemSlot.Weapon && heroes2[num2].Weapon != "")
				{
					cardData2 = Globals.Instance.GetCardData(heroes2[num2].Weapon, instantiate: false);
				}
				else if (eventReplyData.SsRemoveItemSlot == Enums.ItemSlot.Armor && heroes2[num2].Armor != "")
				{
					cardData2 = Globals.Instance.GetCardData(heroes2[num2].Armor, instantiate: false);
				}
				else if (eventReplyData.SsRemoveItemSlot == Enums.ItemSlot.Jewelry && heroes2[num2].Jewelry != "")
				{
					cardData2 = Globals.Instance.GetCardData(heroes2[num2].Jewelry, instantiate: false);
				}
				else if (eventReplyData.SsRemoveItemSlot == Enums.ItemSlot.Accesory && heroes2[num2].Accesory != "")
				{
					cardData2 = Globals.Instance.GetCardData(heroes2[num2].Accesory, instantiate: false);
				}
				else if (eventReplyData.SsRemoveItemSlot == Enums.ItemSlot.Pet && heroes2[num2].Pet != "")
				{
					cardData2 = Globals.Instance.GetCardData(heroes2[num2].Pet, instantiate: false);
				}
				if (cardData2 != null)
				{
					CI.cardoutsideverticallist = true;
					CI.CreateAmplifyOutsideCard(Globals.Instance.GetCardData(cardData2.Id, instantiate: false), GetComponent<BoxCollider2D>(), heroes2[num2]);
				}
			}
		}
	}

	private void OnMouseExit()
	{
		GameManager.Instance.SetCursorPlain();
		GameManager.Instance.CleanTempContainer();
		PopupManager.Instance.ClosePopup();
		if (!selected)
		{
			replyText.color = colorText;
			replyBgButton.color = colorOff;
		}
	}

	public void OnMouseUp()
	{
		if (!AlertManager.Instance.IsActive() && !GameManager.Instance.IsTutorialActive() && !SettingsManager.Instance.IsActive() && !DamageMeterManager.Instance.IsActive() && (!MapManager.Instance.characterWindow.gameObject.activeSelf || !MapManager.Instance.characterWindow.IsActive()) && !blocked && (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster() || !AtOManager.Instance.followingTheLeader) && Functions.ClickedThisTransform(base.transform) && EventManager.Instance.optionSelected == -1)
		{
			SelectThisOption();
		}
	}

	public void SelectThisOption()
	{
		EventManager.Instance.SelectOption(optionIndex);
		selected = true;
		ShowSelectedDesign();
		GameManager.Instance.CleanTempContainer();
	}
}
