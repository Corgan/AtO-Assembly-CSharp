using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using WebSocketSharp;

public class EventManager : MonoBehaviour
{
	public TMP_Text title;

	public TMP_Text description;

	public TMP_Text descriptionRolls;

	public TMP_Text result;

	public TMP_Text notMeeted;

	public TMP_Text resultOK;

	public TMP_Text resultOKc;

	public TMP_Text resultKO;

	public TMP_Text resultKOc;

	public SpriteRenderer image;

	public Transform replysT;

	public GameObject replyPrefab;

	public Transform continueButton;

	public Transform cardKO;

	public Transform cardKOCards;

	public TMP_Text cardKOText;

	public Transform cardOK;

	public Transform cardOKCards;

	public TMP_Text cardOKText;

	private GameObject[] replysGOs;

	private Reply[] replys;

	private int numReplys;

	private int replysInvalid;

	public Transform[] characterT = new Transform[4];

	private Transform[] characterTcards = new Transform[4];

	private SpriteRenderer[] characterSPR = new SpriteRenderer[4];

	private TMP_Text[] characterTnum = new TMP_Text[4];

	private TMP_Text[] charTresultOK = new TMP_Text[4];

	private TMP_Text[] charTresultKO = new TMP_Text[4];

	private EventData currentEvent;

	private EventReplyData replySelected;

	[SerializeField]
	private float topReply = 3.3f;

	[SerializeField]
	private float distanceReply = 1.2f;

	public int optionSelected = -1;

	private Hero[] heroes = new Hero[4];

	private Hero[] heroesSource = new Hero[4];

	private bool[] charRoll;

	private int[] charRollIterations;

	private int[] charRollResult;

	private CardData[] charRollType;

	private bool criticalSuccess;

	private bool criticalFail;

	private bool[] charWinner = new bool[4];

	private bool groupWinner;

	private int cardOrder;

	private bool isGroup;

	private CombatData followUpCombatData;

	private EventData followUpEventData;

	private int followUpDiscount;

	private int followUpMaxQuantity;

	private Enums.CardRarity followUpMaxCraftRarity;

	private bool followUpUpgrade;

	private bool followUpHealer;

	private bool followUpCraft;

	private bool followUpCorruption;

	private bool followUpItemCorruption;

	private bool followUpCardPlayerGame;

	private CardPlayerPackData followUpCardPlayerGamePack;

	private bool followUpCardPlayerPairsGame;

	private CardPlayerPairsPackData followUpCardPlayerPairsGamePack;

	private string followUpShopListId;

	private string followUpLootListId;

	private NodeData destinationNode;

	private List<string> cardCharacters;

	public Dictionary<string, int> MultiplayerPlayerSelection = new Dictionary<string, int>();

	private List<int> probability;

	private bool statusReady;

	public Transform waitingMsg;

	public TMP_Text waitingMsgText;

	private Coroutine manualReadyCo;

	private PhotonView photonView;

	private int modRollMadness;

	private int dustQuantity;

	private int goldQuantity;

	private bool[] characterPassRoll = new bool[4];

	public int controllerHorizontalIndex = -1;

	private Vector2 warpPosition = Vector2.zero;

	private List<Transform> _controllerList = new List<Transform>();

	public List<GameObject> DisableWhenConflictResolution;

	public static EventManager Instance { get; private set; }

	public Hero[] Heroes
	{
		get
		{
			return heroes;
		}
		set
		{
			heroes = value;
		}
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		photonView = MapManager.Instance.GetPhotonView();
		for (int i = 0; i < 4; i++)
		{
			characterSPR[i] = characterT[i].GetComponent<SpriteRenderer>();
			characterTcards[i] = characterT[i].GetChild(0).transform;
			characterTnum[i] = characterT[i].GetChild(1).transform.GetComponent<TMP_Text>();
			charTresultKO[i] = characterT[i].GetChild(2).transform.GetComponent<TMP_Text>();
			charTresultOK[i] = characterT[i].GetChild(3).transform.GetComponent<TMP_Text>();
			characterPassRoll[i] = false;
		}
		waitingMsg.gameObject.SetActive(value: false);
	}

	private void Update()
	{
		if (GameManager.Instance.GetDeveloperMode() && Input.GetKeyDown(KeyCode.F1))
		{
			MapManager.Instance.ShowHideEvent();
		}
	}

	private void Start()
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			statusReady = false;
			continueButton.GetComponent<BotonGeneric>().SetBackgroundColor(Functions.HexToColor(Globals.Instance.ClassColor["warrior"]));
			if (NetworkManager.Instance.IsMaster())
			{
				NetworkManager.Instance.ClearAllPlayerManualReady();
			}
		}
		continueButton.gameObject.SetActive(value: false);
	}

	public void SetListGameObjectsState(bool state)
	{
		foreach (GameObject item in DisableWhenConflictResolution)
		{
			if (item != null)
			{
				item.SetActive(state);
			}
		}
	}

	public void Show()
	{
		AudioManager.Instance.DoBSO("Event");
		base.gameObject.SetActive(value: true);
		GameManager.Instance.CleanTempContainer();
		PopupManager.Instance.ClosePopup();
		MapManager.Instance.HidePopup();
		MapManager.Instance.sideCharacters.InCharacterScreen(state: true);
		MapManager.Instance.sideCharacters.ResetCharacters();
		AlertManager.Instance.HideAlert();
		SettingsManager.Instance.ShowSettings(_state: false);
		DamageMeterManager.Instance.Hide();
		MapManager.Instance.characterWindow.HideAllWindows();
	}

	public void CloseEvent()
	{
		Functions.DebugLogGD("EVENT CloseEvent", "trace");
		foreach (Transform item in replysT)
		{
			Object.Destroy(item.gameObject);
		}
		MapManager.Instance.sideCharacters.InCharacterScreen(state: false);
		PopupManager.Instance.ClosePopup();
		AudioManager.Instance.DoBSO("Map");
		SaveManager.SavePlayerData();
		MapManager.Instance.CloseEventFromEvent(destinationNode, followUpCombatData, followUpEventData, followUpUpgrade, followUpDiscount, followUpMaxQuantity, followUpHealer, followUpCraft, followUpShopListId, followUpLootListId, followUpMaxCraftRarity, followUpCardPlayerGame, followUpCardPlayerGamePack, followUpCardPlayerPairsGame, followUpCardPlayerPairsGamePack, followUpCorruption, followUpItemCorruption);
	}

	public void SetEvent(EventData _eventData)
	{
		destinationNode = null;
		followUpCombatData = null;
		followUpEventData = null;
		followUpDiscount = 0;
		followUpMaxQuantity = -1;
		followUpMaxCraftRarity = Enums.CardRarity.Common;
		followUpUpgrade = false;
		followUpHealer = false;
		followUpCraft = false;
		followUpCardPlayerGame = false;
		followUpCardPlayerGamePack = null;
		followUpShopListId = "";
		followUpLootListId = "";
		currentEvent = Globals.Instance.GetEventData(_eventData.EventId);
		optionSelected = -1;
		cardOrder = 1000;
		if (AtOManager.Instance.GetNgPlus() >= 4 || AtOManager.Instance.IsChallengeTraitActive("unlucky"))
		{
			modRollMadness = 1;
		}
		else if (AtOManager.Instance.IsChallengeTraitActive("lucky"))
		{
			modRollMadness = -1;
		}
		if (!(currentEvent == null))
		{
			cardCharacters = new List<string>();
			string text = Texts.Instance.GetText(_eventData.EventId + "_nm", "events");
			if (text != "")
			{
				title.text = text;
			}
			else
			{
				Debug.LogError(_eventData.EventId + " <EventName> missing translation");
				title.text = currentEvent.EventName;
			}
			StringBuilder stringBuilder = new StringBuilder();
			text = Texts.Instance.GetText(_eventData.EventId + "_dsc", "events");
			if (text != "")
			{
				stringBuilder.Append(text);
			}
			else
			{
				Debug.LogError(_eventData.EventId + " <Description> missing translation");
				stringBuilder.Append(currentEvent.Description);
			}
			stringBuilder.Append("\n\n<color=#333>");
			text = Texts.Instance.GetText(_eventData.EventId + "_dsca", "events");
			if (text != "")
			{
				stringBuilder.Append(text);
			}
			else
			{
				stringBuilder.Append(currentEvent.DescriptionAction);
			}
			description.text = stringBuilder.ToString();
			stringBuilder.Clear();
			stringBuilder.Append("<color=#FFF>");
			stringBuilder.Append(Texts.Instance.GetText("eventRolls"));
			stringBuilder.Append("</color><br><br>");
			stringBuilder.Append("<u>");
			stringBuilder.Append(Texts.Instance.GetText("single"));
			stringBuilder.Append("</u><br>");
			stringBuilder.Append(Texts.Instance.GetText("singleDesc"));
			stringBuilder.Append("<br><br>");
			stringBuilder.Append("<u>");
			stringBuilder.Append(Texts.Instance.GetText("competition"));
			stringBuilder.Append("</u><br>");
			stringBuilder.Append(Texts.Instance.GetText("competitionDesc"));
			stringBuilder.Append("<br><br>");
			stringBuilder.Append("<u>");
			stringBuilder.Append(Texts.Instance.GetText("group"));
			stringBuilder.Append("</u><br>");
			stringBuilder.Append(Texts.Instance.GetText("groupDesc"));
			descriptionRolls.text = stringBuilder.ToString();
			if (currentEvent.EventSpriteBook != null)
			{
				image.sprite = currentEvent.EventSpriteBook;
			}
			numReplys = currentEvent.Replys.Length;
			replysGOs = new GameObject[numReplys];
			replys = new Reply[numReplys];
			heroes = AtOManager.Instance.GetTeam();
			for (int i = 0; i < 4; i++)
			{
				charWinner[i] = true;
			}
			Show();
			SetProbability();
		}
	}

	private void EndProbability()
	{
		StartCoroutine(SetReplys());
	}

	private void SetProbability()
	{
		bool flag = false;
		for (int i = 0; i < numReplys; i++)
		{
			if (currentEvent.Replys[i].SsRoll)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			EndProbability();
			return;
		}
		if (probability == null)
		{
			probability = new List<int>();
		}
		else
		{
			probability.Clear();
		}
		string text = "";
		Dictionary<int, List<string>> dictionary = new Dictionary<int, List<string>>();
		for (int j = 0; j < 4; j++)
		{
			dictionary[j] = new List<string>();
			if (heroes[j] == null || !(heroes[j].HeroData != null) || heroes[j].Cards == null)
			{
				continue;
			}
			for (int k = 0; k < heroes[j].Cards.Count; k++)
			{
				text = heroes[j].Cards[k].ToLower();
				if (!Globals.Instance.CardListByClass[Enums.CardClass.Boon].Contains(text) && !Globals.Instance.CardListByClass[Enums.CardClass.Injury].Contains(text))
				{
					dictionary[j].Add(text);
				}
			}
		}
		int num = 50;
		int[] array = new int[4];
		for (int l = 0; l < 4; l++)
		{
			array[l] = dictionary[l].Count;
			if (array[l] > num)
			{
				array[l] = num;
			}
		}
		int num2 = array[0];
		int num3 = array[1];
		int num4 = array[2];
		int num5 = array[3];
		if (num2 > dictionary[0].Count)
		{
			num2 = dictionary[0].Count;
		}
		if (num3 > dictionary[1].Count)
		{
			num3 = dictionary[1].Count;
		}
		if (num4 > dictionary[2].Count)
		{
			num4 = dictionary[2].Count;
		}
		if (num5 > dictionary[3].Count)
		{
			num5 = dictionary[3].Count;
		}
		for (int m = 0; m < num2; m++)
		{
			text = dictionary[0][m];
			int num6 = (Globals.Instance.CardEnergyCost.ContainsKey(text) ? Globals.Instance.CardEnergyCost[text] : 0);
			if (num3 > 0)
			{
				for (int n = 0; n < num3; n++)
				{
					text = dictionary[1][n];
					int num7 = ((!Globals.Instance.CardEnergyCost.ContainsKey(text)) ? num6 : (num6 + Globals.Instance.CardEnergyCost[text]));
					if (num4 > 0)
					{
						for (int num8 = 0; num8 < num4; num8++)
						{
							text = dictionary[2][num8];
							int num9 = ((!Globals.Instance.CardEnergyCost.ContainsKey(text)) ? num7 : (num7 + Globals.Instance.CardEnergyCost[text]));
							if (num5 > 0)
							{
								for (int num10 = 0; num10 < num5; num10++)
								{
									text = dictionary[3][num10];
									int item = ((!Globals.Instance.CardEnergyCost.ContainsKey(text)) ? num9 : (num9 + Globals.Instance.CardEnergyCost[text]));
									probability.Add(item);
								}
							}
							else
							{
								probability.Add(num9);
							}
						}
					}
					else
					{
						probability.Add(num7);
					}
				}
			}
			else
			{
				probability.Add(num6);
			}
		}
		EndProbability();
	}

	public int GetProbability(int result, bool higherOrEqual)
	{
		int num = 0;
		for (int i = 0; i < probability.Count; i++)
		{
			if (result == probability[i])
			{
				num++;
			}
			else if (probability[i] > result && higherOrEqual)
			{
				num++;
			}
			else if (probability[i] < result && !higherOrEqual)
			{
				num++;
			}
		}
		if (AtOManager.Instance.currentMapNode == "tutorial_2")
		{
			return 100;
		}
		return ProbabilityResult(num, probability.Count);
	}

	private int ProbabilityResult(int _times, int _total)
	{
		int num = Mathf.CeilToInt((float)_times / (float)_total * 100f);
		if (num > 100)
		{
			num = 100;
		}
		else if (num < 0)
		{
			num = 0;
		}
		return num;
	}

	public int GetProbabilityType(Enums.CardType cType, string cClassId)
	{
		int num = 0;
		int num2 = -1;
		for (int i = 0; i < heroes.Length; i++)
		{
			if (heroes[i].HeroData.HeroSubClass.Id == cClassId)
			{
				num2 = i;
				break;
			}
		}
		if (heroes[num2].GetItemToPassEventRoll() != "")
		{
			return 100;
		}
		int num3 = 0;
		for (int j = 0; j < heroes[num2].Cards.Count; j++)
		{
			string item = heroes[num2].Cards[j].ToLower();
			if (!Globals.Instance.CardListByClass[Enums.CardClass.Boon].Contains(item) && !Globals.Instance.CardListByClass[Enums.CardClass.Injury].Contains(item))
			{
				if (Globals.Instance.CardListByType[cType].Contains(item))
				{
					num++;
				}
				num3++;
			}
		}
		return ProbabilityResult(num, num3);
	}

	public int GetProbabilitySingle(int result, bool higherOrEqual, int heroId)
	{
		if (heroes[heroId].GetItemToPassEventRoll() != "")
		{
			return 100;
		}
		string text = "";
		int num = 0;
		int num2 = 0;
		if (heroes[heroId].Cards != null)
		{
			for (int i = 0; i < heroes[heroId].Cards.Count; i++)
			{
				text = heroes[heroId].Cards[i].ToLower();
				if (!Globals.Instance.CardListByClass[Enums.CardClass.Boon].Contains(text) && !Globals.Instance.CardListByClass[Enums.CardClass.Injury].Contains(text))
				{
					if (!Globals.Instance.CardEnergyCost.ContainsKey(text))
					{
						Globals.Instance.CardEnergyCost.Add(text, Globals.Instance.GetCardData(text, instantiate: false).EnergyCost);
					}
					int num3 = Globals.Instance.CardEnergyCost[text];
					if (result == num3)
					{
						num++;
					}
					else if (num3 > result && higherOrEqual)
					{
						num++;
					}
					else if (num3 < result && !higherOrEqual)
					{
						num++;
					}
					num2++;
				}
			}
		}
		return ProbabilityResult(num, num2);
	}

	private IEnumerator SetReplys()
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			yield return Globals.Instance.WaitForSeconds(0.1f);
			if (NetworkManager.Instance.IsMaster())
			{
				while (!NetworkManager.Instance.AllPlayersReady("eventreply"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				Functions.DebugLogGD("Game ready, Everybody checked eventreply");
				NetworkManager.Instance.PlayersNetworkContinue("eventreply");
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("eventreply", status: true);
				NetworkManager.Instance.SetStatusReady("eventreply");
				while (NetworkManager.Instance.WaitingSyncro["eventreply"])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				Functions.DebugLogGD("eventreply, we can continue!");
			}
		}
		GameManager.Instance.PlayLibraryAudio("ui_book_page");
		GameManager.Instance.PlayLibraryAudio("ui_book_write");
		int num = 0;
		replysInvalid = 0;
		int totalPlayersGold = AtOManager.Instance.GetTotalPlayersGold();
		int totalPlayersDust = AtOManager.Instance.GetTotalPlayersDust();
		Hero[] team = AtOManager.Instance.GetTeam();
		for (int i = 0; i < numReplys; i++)
		{
			if (optionSelected > -1)
			{
				break;
			}
			EventReplyData eventReplyData = currentEvent.Replys[i];
			if (i < team.Length && eventReplyData.ChooseReplacementHero)
			{
				currentEvent.Replys[i].RequiredClass = Globals.Instance.GetSubClassData(team[i].SubclassName);
			}
			bool flag = true;
			if (!GameManager.Instance.IsMultiplayer() && eventReplyData.RequirementMultiplayer)
			{
				flag = false;
			}
			if (flag && eventReplyData.RequiredClass != null && !AtOManager.Instance.PlayerHasRequirementClass(eventReplyData.RequiredClass.Id))
			{
				flag = false;
			}
			if (flag && eventReplyData.Requirement != null && !AtOManager.Instance.PlayerHasRequirement(eventReplyData.Requirement))
			{
				flag = false;
			}
			if (flag)
			{
				if (eventReplyData.RequirementBlocked != null && AtOManager.Instance.PlayerHasRequirement(eventReplyData.RequirementBlocked))
				{
					flag = false;
				}
				if (eventReplyData.RequiredClassForBlocked != null && AtOManager.Instance.PlayerHasRequirementClass(eventReplyData.RequiredClassForBlocked.Id))
				{
					flag = false;
				}
			}
			if (flag && eventReplyData.RequirementItems != null && eventReplyData.RequirementItems.Count > 0)
			{
				foreach (CardData requirementItem in eventReplyData.RequirementItems)
				{
					if (!(requirementItem == null))
					{
						if (AtOManager.Instance.PlayerHasRequirementItem(requirementItem, eventReplyData.RequiredClass) != -1)
						{
							flag = true;
							break;
						}
						flag = false;
					}
				}
			}
			if (flag && eventReplyData.RequirementItem != null && AtOManager.Instance.PlayerHasRequirementItem(eventReplyData.RequirementItem, eventReplyData.RequiredClass) == -1)
			{
				flag = false;
			}
			if (flag)
			{
				if (eventReplyData.SsRemoveItemSlot != Enums.ItemSlot.None && !AtOManager.Instance.SubClassDataHaveAnythingInSlot(eventReplyData.SsRemoveItemSlot, eventReplyData.RequiredClass))
				{
					flag = false;
				}
				if (flag && eventReplyData.SsCorruptItemSlot != Enums.ItemSlot.None && !AtOManager.Instance.SubClassDataHaveAnythingInSlot(eventReplyData.SsCorruptItemSlot, eventReplyData.RequiredClass))
				{
					flag = false;
				}
			}
			if (flag && eventReplyData.RequirementCard != null && eventReplyData.RequirementCard.Count > 0)
			{
				flag = false;
				for (int j = 0; j < eventReplyData.RequirementCard.Count; j++)
				{
					if (eventReplyData.RequirementCard[j] != null && AtOManager.Instance.PlayerHasRequirementCard(eventReplyData.RequirementCard[j], eventReplyData.RequiredClass))
					{
						flag = true;
						break;
					}
				}
			}
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			if (flag)
			{
				if (eventReplyData.RequirementSku != "")
				{
					if (!GameManager.Instance.IsMultiplayer())
					{
						if (!SteamManager.Instance.PlayerHaveDLC(eventReplyData.RequirementSku))
						{
							flag4 = true;
						}
					}
					else if (!NetworkManager.Instance.AnyPlayersHaveSku(eventReplyData.RequirementSku))
					{
						flag4 = true;
					}
				}
				if (eventReplyData.GoldCost > 0 && totalPlayersGold < eventReplyData.GoldCost)
				{
					flag2 = true;
				}
				if (eventReplyData.DustCost > 0 && totalPlayersDust < eventReplyData.DustCost)
				{
					flag3 = true;
				}
			}
			if (flag)
			{
				GameObject gameObject = Object.Instantiate(replyPrefab, replysT);
				gameObject.name = "reply_" + i;
				replysGOs[i] = gameObject;
				replys[i] = gameObject.transform.GetComponent<Reply>();
				if (eventReplyData.Requirement != null && eventReplyData.Requirement.ItemSprite != null)
				{
					replys[i].SetImage(eventReplyData.Requirement.ItemSprite);
				}
				if (flag4)
				{
					replys[i].Block(_showRedLayer: true, _showGoldShardMessage: false);
				}
				else if (flag2 || flag3)
				{
					replys[i].Block();
				}
				num++;
			}
			else
			{
				replysInvalid++;
				replysGOs[i] = null;
				replys[i] = null;
			}
		}
		if (currentEvent.ReplyRandom > 0)
		{
			while (TotalValidGoInReplys() > currentEvent.ReplyRandom)
			{
				int randomIntRange = MapManager.Instance.GetRandomIntRange(0, replysGOs.Length);
				if (replysGOs[randomIntRange] != null)
				{
					Object.Destroy(replysGOs[randomIntRange]);
					replysGOs[randomIntRange] = null;
				}
			}
			replysInvalid = replysGOs.Length - TotalValidGoInReplys();
		}
		num = 0;
		int replyIndexForAutomaticSelection = -1;
		for (int k = 0; k < replysGOs.Length; k++)
		{
			if (replysGOs[k] != null)
			{
				replysGOs[k].transform.localPosition = new Vector3(0f, topReply - distanceReply * (float)num, -1f);
				replys[k].Init(currentEvent.EventId, k, num);
				num++;
				replyIndexForAutomaticSelection = k;
			}
		}
		notMeeted.gameObject.SetActive(value: false);
		if (replysInvalid > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Texts.Instance.GetText("eventMissingOptions"));
			stringBuilder.Append(" (");
			stringBuilder.Append(replysInvalid);
			stringBuilder.Append(")");
			notMeeted.text = stringBuilder.ToString();
			notMeeted.gameObject.SetActive(value: true);
		}
		if (!GameManager.Instance.IsMultiplayer() && num == 1 && AtOManager.Instance.currentMapNode != "tutorial_2" && currentEvent != null && currentEvent.EventId != "e_challenge_finish" && !notMeeted.gameObject.activeSelf)
		{
			yield return new WaitForSeconds(0.5f);
			replys[replyIndexForAutomaticSelection].SelectThisOption();
		}
	}

	private int TotalValidGoInReplys()
	{
		int num = 0;
		for (int i = 0; i < replysGOs.Length; i++)
		{
			if (replysGOs[i] != null)
			{
				num++;
			}
		}
		return num;
	}

	public void SelectOption(int _index)
	{
		if (optionSelected > -1)
		{
			return;
		}
		optionSelected = _index;
		for (int i = 0; i < replys.Length; i++)
		{
			if (replys[i] != null)
			{
				replys[i].DisableOption();
			}
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			photonView.RPC("NET_Event_OptionSelected", RpcTarget.All, NetworkManager.Instance.GetPlayerNick(), _index);
		}
		else
		{
			StartCoroutine(SelectOptionCo());
		}
	}

	public void NET_OptionSelected(string _playerNick, int _option)
	{
		SelectOptionMP(_playerNick, _option);
		GameManager.Instance.PlayLibraryAudio("ui_eventoptionselection");
		if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster() && AtOManager.Instance.followingTheLeader && NetworkManager.Instance.PlayerIsMaster(_playerNick))
		{
			StartCoroutine(AutoSelectClient(_option));
		}
	}

	private IEnumerator AutoSelectClient(int _option)
	{
		if (_option < 0)
		{
			AtOManager.Instance.followingTheLeader = false;
			yield break;
		}
		while (replys == null || _option > replys.Length || replys[_option] == null)
		{
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
		replys[_option].SelectThisOption();
	}

	private void SelectOptionMP(string _playerNick, int _option)
	{
		if (!MultiplayerPlayerSelection.ContainsKey(_playerNick))
		{
			MultiplayerPlayerSelection.Add(_playerNick, _option);
		}
		for (int i = 0; i < replys.Length; i++)
		{
			if (replys[i] != null && replys[i].GetOptionIndex() == _option)
			{
				replys[i].SelectedByMultiplayer(_playerNick);
			}
		}
		if (NetworkManager.Instance.IsMaster() && MultiplayerPlayerSelection.Count == NetworkManager.Instance.GetNumPlayers())
		{
			SelectMultiplayerAnswer();
		}
	}

	private void SelectMultiplayerAnswer()
	{
		int value = MultiplayerPlayerSelection.ElementAt(0).Value;
		bool flag = false;
		for (int i = 1; i < MultiplayerPlayerSelection.Count; i++)
		{
			if (MultiplayerPlayerSelection.ElementAt(i).Value != value)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			int value2 = MultiplayerPlayerSelection.ElementAt(0).Value;
			photonView.RPC("NET_Event_SelectAnswer", RpcTarget.All, value2);
		}
		else
		{
			photonView.RPC("NET_DoConflict", RpcTarget.All);
		}
	}

	public void NET_SelectAnswer(int _answerId)
	{
		optionSelected = _answerId;
		StartCoroutine(SelectOptionCo());
	}

	private IEnumerator SelectOptionCo()
	{
		PopupManager.Instance.ClosePopup();
		GameObject selectedGO = null;
		for (int i = 0; i < replys.Length; i++)
		{
			if (replys[i] != null)
			{
				if (replys[i].GetOptionIndex() != optionSelected)
				{
					replysGOs[i].gameObject.SetActive(value: false);
				}
				else
				{
					selectedGO = replysGOs[i];
				}
			}
		}
		float speed = (topReply - selectedGO.transform.localPosition.y) * 0.2f;
		yield return Globals.Instance.WaitForSeconds(0.05f);
		while (selectedGO.transform.localPosition.y < topReply)
		{
			selectedGO.transform.localPosition += new Vector3(0f, speed, 0f);
			if (speed > 0.1f)
			{
				speed *= 0.8f;
			}
			yield return null;
		}
		selectedGO.transform.localPosition = new Vector3(selectedGO.transform.localPosition.x, topReply, selectedGO.transform.localPosition.z);
		SelectOptionResult();
	}

	private void SelectOptionResult()
	{
		replySelected = currentEvent.Replys[optionSelected];
		if (replySelected.GoldCost > 0)
		{
			AtOManager.Instance.PayGold(replySelected.GoldCost, paySingle: false);
		}
		if (replySelected.DustCost > 0)
		{
			AtOManager.Instance.PayDust(replySelected.DustCost, paySingle: false);
		}
		if (replySelected.SsRoll)
		{
			StartCoroutine(ShowCharactersRoll());
			return;
		}
		groupWinner = true;
		FinalResolution();
	}

	private IEnumerator ShowCharactersRoll()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (replySelected.SsRollMode == Enums.RollMode.HigherOrEqual)
		{
			stringBuilder.Append(">=");
			stringBuilder.Append(replySelected.SsRollNumber + modRollMadness);
		}
		else if (replySelected.SsRollMode == Enums.RollMode.LowerOrEqual)
		{
			stringBuilder.Append("<=");
			stringBuilder.Append(replySelected.SsRollNumber - modRollMadness);
		}
		else if (replySelected.SsRollMode == Enums.RollMode.Highest)
		{
			stringBuilder.Append(">>");
		}
		else if (replySelected.SsRollMode == Enums.RollMode.Lowest)
		{
			stringBuilder.Append("<<");
		}
		else
		{
			stringBuilder.Append("~");
			stringBuilder.Append(replySelected.SsRollNumber);
		}
		replys[optionSelected].SetRollBox(stringBuilder.ToString());
		charRoll = new bool[4];
		charRollIterations = new int[4];
		charRollResult = new int[4];
		charRollType = new CardData[4];
		for (int i = 0; i < 4; i++)
		{
			if (heroes[i] != null && !(heroes[i].HeroData == null))
			{
				bool flag = true;
				charRollIterations[i] = -1;
				characterSPR[i].sprite = heroes[i].SpritePortrait;
				if (replySelected.SsRollTarget == Enums.RollTarget.Character && replySelected.RequiredClass.SubClassName != heroes[i].HeroData.HeroSubClass.SubClassName)
				{
					flag = false;
				}
				charRoll[i] = flag;
				if (!flag)
				{
					TurnOffCharacter(i);
				}
				characterT[i].gameObject.SetActive(value: true);
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
		}
		if ((bool)MapManager.Instance)
		{
			int deterministicHashCode = (AtOManager.Instance.GetGameId() + "_" + AtOManager.Instance.currentMapNode + "_" + AtOManager.Instance.GetTeamTotalHp() + "_" + AtOManager.Instance.GetTeamTotalExperience()).GetDeterministicHashCode();
			MapManager.Instance.GenerateRandomStringBatch(100, deterministicHashCode);
		}
		StartCoroutine(RollCards());
	}

	private IEnumerator RollCards()
	{
		int rolls = 0;
		for (int i = 0; i < 4; i++)
		{
			if (charRoll[i])
			{
				yield return Globals.Instance.WaitForSeconds(0.25f);
				DoCard(i);
				rolls++;
			}
		}
		yield return Globals.Instance.WaitForSeconds(2.3f + (float)rolls * 0.1f);
		StartCoroutine(EventResult());
	}

	private void DoCard(int heroIndex)
	{
		if (heroes[heroIndex] == null || heroes[heroIndex].Cards == null)
		{
			return;
		}
		charRollIterations[heroIndex]++;
		if (charRollIterations[heroIndex] > 3)
		{
			charRollIterations[heroIndex] = 3;
		}
		charRollResult[heroIndex] = -1;
		charRollType[heroIndex] = null;
		List<string> list = new List<string>();
		for (int i = 0; i < heroes[heroIndex].Cards.Count; i++)
		{
			list.Add(heroes[heroIndex].Cards[i]);
		}
		bool flag = false;
		string id = "";
		string itemToPassEventRoll = heroes[heroIndex].GetItemToPassEventRoll();
		if (itemToPassEventRoll != "" && (replySelected.SsRollTarget == Enums.RollTarget.Character || replySelected.SsRollTarget == Enums.RollTarget.Single))
		{
			id = itemToPassEventRoll;
			characterPassRoll[heroIndex] = true;
		}
		else
		{
			while (!flag)
			{
				id = list[MapManager.Instance.GetRandomIntRange(0, list.Count)];
				CardData cardData = Globals.Instance.GetCardData(id, instantiate: false);
				if (cardData.CardClass != Enums.CardClass.Injury && cardData.CardClass != Enums.CardClass.Boon)
				{
					flag = true;
				}
			}
		}
		GameObject gameObject = Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, characterTcards[heroIndex]);
		CardItem component = gameObject.GetComponent<CardItem>();
		component.gameObject.name = "EventRollCard";
		component.SetCard(id, deckScale: false);
		component.SetCardback(heroes[heroIndex]);
		gameObject.transform.localPosition = new Vector3(0f, -0.75f - 0.1f * (float)charRollIterations[heroIndex], 0f);
		component.DoReward(fromReward: false, fromEvent: true, fromLoot: false, selectable: false);
		component.SetDestinationLocalScale(0.9f);
		component.TopLayeringOrder("Book", cardOrder);
		cardOrder += 50;
		component.DisableCollider();
		charRollResult[heroIndex] = Globals.Instance.GetCardData(id, instantiate: false).EnergyCost;
		charRollType[heroIndex] = Globals.Instance.GetCardData(id, instantiate: false);
		if (replySelected.SsRollCard == Enums.CardType.None)
		{
			StartCoroutine(ShowNumRoll(heroIndex));
		}
	}

	private void ClearNumRoll(int heroIndex)
	{
		characterTnum[heroIndex].text = "";
	}

	private IEnumerator ShowNumRoll(int heroIndex)
	{
		if (!charRoll[heroIndex])
		{
			characterTnum[heroIndex].text = "";
			yield break;
		}
		yield return Globals.Instance.WaitForSeconds(2.2f);
		characterTcards[heroIndex].GetChild(0).GetComponent<CardItem>().active = true;
		characterTnum[heroIndex].text = charRollResult[heroIndex].ToString();
	}

	private IEnumerator EventResult()
	{
		bool flag = false;
		int num = 0;
		while (!flag)
		{
			yield return Globals.Instance.WaitForSeconds(0.1f);
			num = 0;
			flag = true;
			for (int i = 0; i < 4; i++)
			{
				if (charRoll[i] && charRollResult[i] == -1)
				{
					flag = false;
					break;
				}
				if (charRoll[i])
				{
					num += charRollResult[i];
				}
			}
		}
		isGroup = true;
		bool success = false;
		int ssRollNumber = replySelected.SsRollNumber;
		int ssRollNumberCritical = replySelected.SsRollNumberCritical;
		int ssRollNumberCriticalFail = replySelected.SsRollNumberCriticalFail;
		if (replySelected.SsRollTarget == Enums.RollTarget.Group)
		{
			success = false;
			if (replySelected.SsRollMode == Enums.RollMode.HigherOrEqual)
			{
				if (num >= ssRollNumber + modRollMadness)
				{
					success = true;
				}
				if (ssRollNumberCritical > -1 && num >= ssRollNumberCritical + modRollMadness)
				{
					criticalSuccess = true;
				}
				else if (ssRollNumberCritical > -1 && num <= ssRollNumberCriticalFail + modRollMadness)
				{
					criticalFail = true;
				}
			}
			else if (replySelected.SsRollMode == Enums.RollMode.LowerOrEqual)
			{
				if (num <= ssRollNumber - modRollMadness)
				{
					success = true;
				}
				if (ssRollNumberCritical > -1 && num <= ssRollNumberCritical - modRollMadness)
				{
					criticalSuccess = true;
				}
				else if (ssRollNumberCritical > -1 && num >= ssRollNumberCriticalFail - modRollMadness)
				{
					criticalFail = true;
				}
			}
			for (int j = 0; j < 4; j++)
			{
				charWinner[j] = success;
			}
			groupWinner = success;
			StartCoroutine(ShowResultTitle(success));
		}
		else if (replySelected.SsRollTarget == Enums.RollTarget.Character)
		{
			for (int k = 0; k < 4; k++)
			{
				if (!charRoll[k])
				{
					continue;
				}
				success = false;
				if (characterPassRoll[k])
				{
					success = true;
					if (replySelected.SsRollMode == Enums.RollMode.HigherOrEqual)
					{
						charRollResult[k] = ssRollNumber + modRollMadness;
					}
					else if (replySelected.SsRollMode == Enums.RollMode.LowerOrEqual)
					{
						charRollResult[k] = ssRollNumber - modRollMadness;
					}
				}
				else if (replySelected.SsRollCard != Enums.CardType.None)
				{
					if (charRollType[k].HasCardType(replySelected.SsRollCard))
					{
						success = true;
					}
				}
				else if (replySelected.SsRollMode == Enums.RollMode.HigherOrEqual)
				{
					if (charRollResult[k] >= ssRollNumber + modRollMadness)
					{
						success = true;
					}
				}
				else if (replySelected.SsRollMode == Enums.RollMode.LowerOrEqual && charRollResult[k] <= ssRollNumber - modRollMadness)
				{
					success = true;
				}
				groupWinner = success;
			}
			for (int l = 0; l < 4; l++)
			{
				charWinner[l] = groupWinner;
			}
			StartCoroutine(ShowResultTitle(success));
		}
		else if (replySelected.SsRollTarget == Enums.RollTarget.Single)
		{
			isGroup = false;
			for (int m = 0; m < 4; m++)
			{
				if (characterPassRoll[m])
				{
					success = true;
					if (replySelected.SsRollMode == Enums.RollMode.HigherOrEqual)
					{
						charRollResult[m] = ssRollNumber + modRollMadness;
					}
					else if (replySelected.SsRollMode == Enums.RollMode.LowerOrEqual)
					{
						charRollResult[m] = ssRollNumber - modRollMadness;
					}
				}
				else
				{
					success = false;
					if (replySelected.SsRollMode == Enums.RollMode.HigherOrEqual)
					{
						if (charRollResult[m] >= ssRollNumber + modRollMadness)
						{
							success = true;
						}
					}
					else if (replySelected.SsRollMode == Enums.RollMode.LowerOrEqual && charRollResult[m] <= ssRollNumber - modRollMadness)
					{
						success = true;
					}
				}
				charWinner[m] = success;
				StartCoroutine(ShowResultTitle(success, m));
			}
		}
		else if (replySelected.SsRollTarget == Enums.RollTarget.Competition)
		{
			isGroup = false;
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			for (int n = 0; n < 4; n++)
			{
				charWinner[n] = false;
				if (charRoll[n])
				{
					dictionary.Add(n, charRollResult[n]);
				}
			}
			dictionary = dictionary.OrderBy((KeyValuePair<int, int> x) => x.Value).ToDictionary((KeyValuePair<int, int> x) => x.Key, (KeyValuePair<int, int> x) => x.Value);
			int num2 = -1;
			int num3 = -1;
			int num4 = dictionary.Count - 1;
			if (num4 == 0)
			{
				num3 = dictionary.ElementAt(0).Key;
			}
			else if (replySelected.SsRollMode == Enums.RollMode.Highest)
			{
				if (dictionary.ElementAt(num4).Value == dictionary.ElementAt(num4 - 1).Value)
				{
					num2 = dictionary.ElementAt(num4).Value;
				}
				else
				{
					num3 = dictionary.ElementAt(num4).Key;
				}
			}
			else if (replySelected.SsRollMode == Enums.RollMode.Lowest)
			{
				if (dictionary.ElementAt(0).Value == dictionary.ElementAt(1).Value)
				{
					num2 = dictionary.ElementAt(0).Value;
				}
				else
				{
					num3 = dictionary.ElementAt(0).Key;
				}
			}
			if (num2 > -1)
			{
				for (int num5 = 0; num5 < dictionary.Count; num5++)
				{
					int key = dictionary.ElementAt(num5).Key;
					if (charRoll[key] && dictionary.ElementAt(num5).Value == num2)
					{
						charRoll[key] = true;
					}
					else
					{
						charRoll[key] = false;
						StartCoroutine(ShowResultTitle(success: false, key));
						TurnOffCharacter(key);
					}
					ClearNumRoll(key);
				}
				StartCoroutine(RollCards());
				yield break;
			}
			StartCoroutine(ShowResultTitle(success: true, num3));
			charWinner[num3] = true;
			for (int num6 = 0; num6 < dictionary.Count; num6++)
			{
				int key2 = dictionary.ElementAt(num6).Key;
				if (key2 != num3)
				{
					if (charRoll[key2])
					{
						StartCoroutine(ShowResultTitle(success: false, key2));
					}
					TurnOffCharacter(key2);
				}
			}
		}
		FinalResolution();
	}

	private void FinalResolution()
	{
		GameManager.Instance.PlayLibraryAudio("ui_book_write");
		bool flag = false;
		bool flag2 = false;
		if (replySelected.SsRoll)
		{
			if (isGroup)
			{
				if (groupWinner)
				{
					flag = true;
				}
				else
				{
					flag2 = true;
				}
			}
			else
			{
				for (int i = 0; i < 4; i++)
				{
					if (charWinner[i])
					{
						flag = true;
					}
					else
					{
						flag2 = true;
					}
				}
			}
		}
		else
		{
			flag = true;
			isGroup = true;
		}
		replys[optionSelected].HideRollBox();
		if (AtOManager.Instance.Sandbox_alwaysPassEventRoll)
		{
			for (int j = 0; j < 4; j++)
			{
				charWinner[j] = true;
			}
			criticalFail = false;
			flag = true;
			flag2 = false;
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		for (int k = 0; k < 4; k++)
		{
			if (heroes[k] == null || heroes[k].HeroData == null)
			{
				continue;
			}
			if ((replySelected.ReplyActionText != Enums.EventAction.CharacterName || replySelected.RequiredClass.SubClassName == heroes[k].HeroData.HeroSubClass.SubClassName) && charWinner[k])
			{
				if (!criticalSuccess)
				{
					if (replySelected.SsAddCard1 != null)
					{
						AtOManager.Instance.AddCardToHero(k, replySelected.SsAddCard1.Id);
						cardCharacters.Add(AtOManager.Instance.GetHero(k).SourceName);
					}
					if (replySelected.SsAddCard2 != null)
					{
						AtOManager.Instance.AddCardToHero(k, replySelected.SsAddCard2.Id);
					}
					if (replySelected.SsAddCard3 != null)
					{
						AtOManager.Instance.AddCardToHero(k, replySelected.SsAddCard3.Id);
					}
				}
				else
				{
					if (replySelected.SscAddCard1 != null)
					{
						AtOManager.Instance.AddCardToHero(k, replySelected.SscAddCard1.Id);
						cardCharacters.Add(AtOManager.Instance.GetHero(k).SourceName);
					}
					if (replySelected.SscAddCard2 != null)
					{
						AtOManager.Instance.AddCardToHero(k, replySelected.SscAddCard2.Id);
					}
					if (replySelected.SscAddCard3 != null)
					{
						AtOManager.Instance.AddCardToHero(k, replySelected.SscAddCard3.Id);
					}
				}
				if (replySelected.SsPerkData != null)
				{
					AtOManager.Instance.AddPerkToHero(k, replySelected.SsPerkData.Id);
				}
				if (replySelected.SsPerkData1 != null)
				{
					AtOManager.Instance.AddPerkToHero(k, replySelected.SsPerkData1.Id);
				}
			}
			if ((replySelected.ReplyActionText != Enums.EventAction.CharacterName || replySelected.RequiredClass.SubClassName == heroes[k].HeroData.HeroSubClass.SubClassName) && !charWinner[k])
			{
				if (!criticalFail)
				{
					if (replySelected.FlAddCard1 != null)
					{
						AtOManager.Instance.AddCardToHero(k, replySelected.FlAddCard1.Id);
						cardCharacters.Add(AtOManager.Instance.GetHero(k).SourceName);
					}
					if (replySelected.FlAddCard2 != null)
					{
						AtOManager.Instance.AddCardToHero(k, replySelected.FlAddCard2.Id);
					}
					if (replySelected.FlAddCard3 != null)
					{
						AtOManager.Instance.AddCardToHero(k, replySelected.FlAddCard3.Id);
					}
				}
				else
				{
					if (replySelected.FlcAddCard1 != null)
					{
						AtOManager.Instance.AddCardToHero(k, replySelected.FlcAddCard1.Id);
						cardCharacters.Add(AtOManager.Instance.GetHero(k).SourceName);
					}
					if (replySelected.FlcAddCard2 != null)
					{
						AtOManager.Instance.AddCardToHero(k, replySelected.FlcAddCard2.Id);
					}
					if (replySelected.FlcAddCard3 != null)
					{
						AtOManager.Instance.AddCardToHero(k, replySelected.FlcAddCard3.Id);
					}
				}
			}
			if (charWinner[k])
			{
				if (!GameManager.Instance.IsMultiplayer() || heroes[k].Owner == NetworkManager.Instance.GetPlayerNick())
				{
					num++;
					if (!criticalSuccess)
					{
						if (replySelected.SsUpgradeRandomCard)
						{
							AtOManager.Instance.UpgradeRandomCardToHero(k, replySelected.SsMaxQuantity);
						}
						if ((bool)replySelected.SsAddItem && (replySelected.RequiredClass == null || replySelected.RequiredClass.SubClassName == heroes[k].HeroData.HeroSubClass.SubClassName))
						{
							string id = replySelected.SsAddItem.Id;
							cardCharacters.Add(AtOManager.Instance.GetHero(k).SourceName);
							StartCoroutine(GenerateRewardCard(ok: true, id));
							if (!GameManager.Instance.IsMultiplayer())
							{
								AtOManager.Instance.AddItemToHero(k, id);
							}
							else
							{
								AtOManager.Instance.AddItemToHeroMP(k, id);
							}
						}
						if (replySelected.SsRemoveItemSlot != Enums.ItemSlot.None && (replySelected.RequiredClass == null || replySelected.RequiredClass.SubClassName == heroes[k].HeroData.HeroSubClass.SubClassName))
						{
							List<string> list = new List<string>();
							if (replySelected.SsRemoveItemSlot == Enums.ItemSlot.AllWithoutPet)
							{
								list.Add("weapon");
								list.Add("armor");
								list.Add("jewelry");
								list.Add("accesory");
							}
							else if (replySelected.SsRemoveItemSlot == Enums.ItemSlot.AllIncludedPet)
							{
								list.Add("weapon");
								list.Add("armor");
								list.Add("jewelry");
								list.Add("accesory");
								list.Add("pet");
							}
							else
							{
								list.Add(replySelected.SsRemoveItemSlot.ToString().ToLower());
							}
							for (int l = 0; l < list.Count; l++)
							{
								AtOManager.Instance.RemoveItemFromHeroFromEvent(_isHero: true, k, list[l]);
							}
						}
						if (replySelected.SsCorruptItemSlot != Enums.ItemSlot.None && (replySelected.RequiredClass == null || replySelected.RequiredClass.SubClassName == heroes[k].HeroData.HeroSubClass.SubClassName))
						{
							List<string> list2 = new List<string>();
							if (replySelected.SsCorruptItemSlot == Enums.ItemSlot.AllWithoutPet)
							{
								list2.Add("weapon");
								list2.Add("armor");
								list2.Add("jewelry");
								list2.Add("accesory");
							}
							else if (replySelected.SsCorruptItemSlot == Enums.ItemSlot.AllIncludedPet)
							{
								list2.Add("weapon");
								list2.Add("armor");
								list2.Add("jewelry");
								list2.Add("accesory");
								list2.Add("pet");
							}
							else
							{
								list2.Add(replySelected.SsCorruptItemSlot.ToString().ToLower());
							}
							for (int m = 0; m < list2.Count; m++)
							{
								AtOManager.Instance.CorruptItemSlot(k, list2[m]);
							}
							if (list2.Count == 1 && list2[0] == "pet")
							{
								StartCoroutine(GenerateRewardCard(ok: true, AtOManager.Instance.GetHero(k).Pet, AtOManager.Instance.GetHero(k).SubclassName));
							}
						}
					}
					else
					{
						if (replySelected.SscUpgradeRandomCard)
						{
							AtOManager.Instance.UpgradeRandomCardToHero(k, replySelected.SscMaxQuantity);
						}
						if ((bool)replySelected.SscAddItem && (replySelected.RequiredClass == null || replySelected.RequiredClass.SubClassName == heroes[k].HeroData.HeroSubClass.SubClassName))
						{
							string id2 = replySelected.SscAddItem.Id;
							cardCharacters.Add(AtOManager.Instance.GetHero(k).SourceName);
							StartCoroutine(GenerateRewardCard(ok: true, id2));
							if (!GameManager.Instance.IsMultiplayer())
							{
								AtOManager.Instance.AddItemToHero(k, id2);
							}
							else
							{
								AtOManager.Instance.AddItemToHeroMP(k, id2);
							}
						}
						if (replySelected.SscRemoveItemSlot != Enums.ItemSlot.None && (replySelected.RequiredClass == null || replySelected.RequiredClass.SubClassName == heroes[k].HeroData.HeroSubClass.SubClassName))
						{
							List<string> list3 = new List<string>();
							if (replySelected.SscRemoveItemSlot == Enums.ItemSlot.AllWithoutPet)
							{
								list3.Add("weapon");
								list3.Add("armor");
								list3.Add("jewelry");
								list3.Add("accesory");
							}
							else if (replySelected.SscRemoveItemSlot == Enums.ItemSlot.AllIncludedPet)
							{
								list3.Add("weapon");
								list3.Add("armor");
								list3.Add("jewelry");
								list3.Add("accesory");
								list3.Add("pet");
							}
							else
							{
								list3.Add(replySelected.SscRemoveItemSlot.ToString().ToLower());
							}
							for (int n = 0; n < list3.Count; n++)
							{
								AtOManager.Instance.RemoveItemFromHeroFromEvent(_isHero: true, k, list3[n]);
							}
						}
						if (replySelected.SscCorruptItemSlot != Enums.ItemSlot.None && (replySelected.RequiredClass == null || replySelected.RequiredClass.SubClassName == heroes[k].HeroData.HeroSubClass.SubClassName))
						{
							List<string> list4 = new List<string>();
							if (replySelected.SscCorruptItemSlot == Enums.ItemSlot.AllWithoutPet)
							{
								list4.Add("weapon");
								list4.Add("armor");
								list4.Add("jewelry");
								list4.Add("accesory");
							}
							else if (replySelected.SscCorruptItemSlot == Enums.ItemSlot.AllIncludedPet)
							{
								list4.Add("weapon");
								list4.Add("armor");
								list4.Add("jewelry");
								list4.Add("accesory");
								list4.Add("pet");
							}
							else
							{
								list4.Add(replySelected.SscCorruptItemSlot.ToString().ToLower());
							}
							for (int num5 = 0; num5 < list4.Count; num5++)
							{
								AtOManager.Instance.CorruptItemSlot(k, list4[num5]);
							}
						}
					}
				}
				if (!criticalSuccess)
				{
					AtOManager.Instance.ModifyHeroLife(k, replySelected.SsRewardHealthFlat, replySelected.SsRewardHealthPercent);
					if (replySelected.SsExperienceReward != 0)
					{
						int num6 = heroes[k].CalculateRewardForCharacter(replySelected.SsExperienceReward);
						num3 += num6;
						heroes[k].GrantExperience(num6);
					}
					if (replySelected.SsGoldReward != 0)
					{
						goldQuantity = replySelected.SsGoldReward;
						if (MadnessManager.Instance.IsMadnessTraitActive("poverty") || AtOManager.Instance.IsChallengeTraitActive("poverty"))
						{
							if (!GameManager.Instance.IsObeliskChallenge())
							{
								goldQuantity -= Functions.FuncRoundToInt((float)goldQuantity * 0.5f);
							}
							else
							{
								goldQuantity -= Functions.FuncRoundToInt((float)goldQuantity * 0.3f);
							}
							if (goldQuantity < 0)
							{
								goldQuantity = 0;
							}
						}
						if (AtOManager.Instance.IsChallengeTraitActive("prosperity"))
						{
							goldQuantity += Functions.FuncRoundToInt((float)goldQuantity * 0.5f);
						}
						AtOManager.Instance.GivePlayer(0, goldQuantity, heroes[k].Owner);
						if (goldQuantity > 0 && (!GameManager.Instance.IsMultiplayer() || heroes[k].Owner == NetworkManager.Instance.GetPlayerNick()))
						{
							PlayerManager.Instance.GoldGainedSum(goldQuantity, save: false);
						}
					}
					if (replySelected.SsDustReward == 0)
					{
						continue;
					}
					dustQuantity = replySelected.SsDustReward;
					if (MadnessManager.Instance.IsMadnessTraitActive("poverty") || AtOManager.Instance.IsChallengeTraitActive("poverty"))
					{
						if (!GameManager.Instance.IsObeliskChallenge())
						{
							dustQuantity -= Functions.FuncRoundToInt((float)dustQuantity * 0.5f);
						}
						else
						{
							dustQuantity -= Functions.FuncRoundToInt((float)dustQuantity * 0.3f);
						}
						if (dustQuantity < 0)
						{
							dustQuantity = 0;
						}
					}
					if (AtOManager.Instance.IsChallengeTraitActive("prosperity"))
					{
						dustQuantity += Functions.FuncRoundToInt((float)dustQuantity * 0.5f);
					}
					AtOManager.Instance.GivePlayer(1, dustQuantity, heroes[k].Owner);
					if (dustQuantity > 0 && (!GameManager.Instance.IsMultiplayer() || heroes[k].Owner == NetworkManager.Instance.GetPlayerNick()))
					{
						PlayerManager.Instance.DustGainedSum(dustQuantity, save: false);
					}
					continue;
				}
				AtOManager.Instance.ModifyHeroLife(k, replySelected.SscRewardHealthFlat, replySelected.SscRewardHealthPercent);
				if (replySelected.SscExperienceReward != 0)
				{
					int num7 = heroes[k].CalculateRewardForCharacter(replySelected.SscExperienceReward);
					num3 += num7;
					heroes[k].GrantExperience(num7);
				}
				if (replySelected.SscGoldReward != 0)
				{
					goldQuantity = replySelected.SscGoldReward;
					if (MadnessManager.Instance.IsMadnessTraitActive("poverty") || AtOManager.Instance.IsChallengeTraitActive("poverty"))
					{
						if (!GameManager.Instance.IsObeliskChallenge())
						{
							goldQuantity -= Functions.FuncRoundToInt((float)goldQuantity * 0.5f);
						}
						else
						{
							goldQuantity -= Functions.FuncRoundToInt((float)goldQuantity * 0.3f);
						}
						if (goldQuantity < 0)
						{
							goldQuantity = 0;
						}
					}
					if (AtOManager.Instance.IsChallengeTraitActive("prosperity"))
					{
						goldQuantity += Functions.FuncRoundToInt((float)goldQuantity * 0.5f);
					}
					AtOManager.Instance.GivePlayer(0, goldQuantity, heroes[k].Owner);
					if (goldQuantity > 0 && (!GameManager.Instance.IsMultiplayer() || heroes[k].Owner == NetworkManager.Instance.GetPlayerNick()))
					{
						PlayerManager.Instance.GoldGainedSum(goldQuantity, save: false);
					}
				}
				if (replySelected.SscDustReward == 0)
				{
					continue;
				}
				dustQuantity = replySelected.SscDustReward;
				if (MadnessManager.Instance.IsMadnessTraitActive("poverty") || AtOManager.Instance.IsChallengeTraitActive("poverty"))
				{
					if (!GameManager.Instance.IsObeliskChallenge())
					{
						dustQuantity -= Functions.FuncRoundToInt((float)dustQuantity * 0.5f);
					}
					else
					{
						dustQuantity -= Functions.FuncRoundToInt((float)dustQuantity * 0.3f);
					}
					if (dustQuantity < 0)
					{
						dustQuantity = 0;
					}
				}
				if (AtOManager.Instance.IsChallengeTraitActive("prosperity"))
				{
					dustQuantity += Functions.FuncRoundToInt((float)dustQuantity * 0.5f);
				}
				AtOManager.Instance.GivePlayer(1, dustQuantity, heroes[k].Owner);
				if (dustQuantity > 0 && (!GameManager.Instance.IsMultiplayer() || heroes[k].Owner == NetworkManager.Instance.GetPlayerNick()))
				{
					PlayerManager.Instance.DustGainedSum(dustQuantity, save: false);
				}
				continue;
			}
			if (!criticalFail)
			{
				if (!GameManager.Instance.IsMultiplayer() || heroes[k].Owner == NetworkManager.Instance.GetPlayerNick())
				{
					num2++;
					if (replySelected.FlUpgradeRandomCard)
					{
						AtOManager.Instance.UpgradeRandomCardToHero(k, replySelected.FlMaxQuantity);
					}
					if ((bool)replySelected.FlAddItem && (replySelected.RequiredClass == null || replySelected.RequiredClass.SubClassName == heroes[k].HeroData.HeroSubClass.SubClassName))
					{
						string id3 = replySelected.FlAddItem.Id;
						cardCharacters.Add(AtOManager.Instance.GetHero(k).SourceName);
						StartCoroutine(GenerateRewardCard(ok: false, id3));
						if (!GameManager.Instance.IsMultiplayer())
						{
							AtOManager.Instance.AddItemToHero(k, id3);
						}
						else
						{
							AtOManager.Instance.AddItemToHeroMP(k, id3);
						}
					}
					if (replySelected.FlRemoveItemSlot != Enums.ItemSlot.None && (replySelected.RequiredClass == null || replySelected.RequiredClass.SubClassName == heroes[k].HeroData.HeroSubClass.SubClassName))
					{
						List<string> list5 = new List<string>();
						if (replySelected.FlRemoveItemSlot == Enums.ItemSlot.AllWithoutPet)
						{
							list5.Add("weapon");
							list5.Add("armor");
							list5.Add("jewelry");
							list5.Add("accesory");
						}
						else if (replySelected.FlRemoveItemSlot == Enums.ItemSlot.AllIncludedPet)
						{
							list5.Add("weapon");
							list5.Add("armor");
							list5.Add("jewelry");
							list5.Add("accesory");
							list5.Add("pet");
						}
						else
						{
							list5.Add(replySelected.FlRemoveItemSlot.ToString().ToLower());
						}
						for (int num8 = 0; num8 < list5.Count; num8++)
						{
							AtOManager.Instance.RemoveItemFromHeroFromEvent(_isHero: true, k, list5[num8]);
						}
					}
					if (replySelected.FlCorruptItemSlot != Enums.ItemSlot.None && (replySelected.RequiredClass == null || replySelected.RequiredClass.SubClassName == heroes[k].HeroData.HeroSubClass.SubClassName))
					{
						List<string> list6 = new List<string>();
						if (replySelected.FlCorruptItemSlot == Enums.ItemSlot.AllWithoutPet)
						{
							list6.Add("weapon");
							list6.Add("armor");
							list6.Add("jewelry");
							list6.Add("accesory");
						}
						else if (replySelected.FlCorruptItemSlot == Enums.ItemSlot.AllIncludedPet)
						{
							list6.Add("weapon");
							list6.Add("armor");
							list6.Add("jewelry");
							list6.Add("accesory");
							list6.Add("pet");
						}
						else
						{
							list6.Add(replySelected.FlCorruptItemSlot.ToString().ToLower());
						}
						for (int num9 = 0; num9 < list6.Count; num9++)
						{
							AtOManager.Instance.CorruptItemSlot(k, list6[num9]);
						}
					}
				}
				AtOManager.Instance.ModifyHeroLife(k, replySelected.FlRewardHealthFlat, replySelected.FlRewardHealthPercent);
				if (replySelected.FlExperienceReward != 0)
				{
					int num10 = heroes[k].CalculateRewardForCharacter(replySelected.FlExperienceReward);
					num4 += num10;
					heroes[k].GrantExperience(num10);
				}
				if (replySelected.FlGoldReward != 0)
				{
					goldQuantity = replySelected.FlGoldReward;
					if (MadnessManager.Instance.IsMadnessTraitActive("poverty") || AtOManager.Instance.IsChallengeTraitActive("poverty"))
					{
						if (!GameManager.Instance.IsObeliskChallenge())
						{
							goldQuantity -= Functions.FuncRoundToInt((float)goldQuantity * 0.5f);
						}
						else
						{
							goldQuantity -= Functions.FuncRoundToInt((float)goldQuantity * 0.3f);
						}
						if (goldQuantity < 0)
						{
							goldQuantity = 0;
						}
					}
					if (AtOManager.Instance.IsChallengeTraitActive("prosperity"))
					{
						goldQuantity += Functions.FuncRoundToInt((float)goldQuantity * 0.5f);
					}
					AtOManager.Instance.GivePlayer(0, goldQuantity, heroes[k].Owner);
					if (goldQuantity > 0 && (!GameManager.Instance.IsMultiplayer() || heroes[k].Owner == NetworkManager.Instance.GetPlayerNick()))
					{
						PlayerManager.Instance.GoldGainedSum(goldQuantity, save: false);
					}
				}
				if (replySelected.FlDustReward == 0)
				{
					continue;
				}
				dustQuantity = replySelected.FlDustReward;
				if (MadnessManager.Instance.IsMadnessTraitActive("poverty") || AtOManager.Instance.IsChallengeTraitActive("poverty"))
				{
					if (!GameManager.Instance.IsObeliskChallenge())
					{
						dustQuantity -= Functions.FuncRoundToInt((float)dustQuantity * 0.5f);
					}
					else
					{
						dustQuantity -= Functions.FuncRoundToInt((float)dustQuantity * 0.3f);
					}
					if (dustQuantity < 0)
					{
						dustQuantity = 0;
					}
				}
				if (AtOManager.Instance.IsChallengeTraitActive("prosperity"))
				{
					dustQuantity += Functions.FuncRoundToInt((float)dustQuantity * 0.5f);
				}
				AtOManager.Instance.GivePlayer(1, dustQuantity, heroes[k].Owner);
				if (dustQuantity > 0 && (!GameManager.Instance.IsMultiplayer() || heroes[k].Owner == NetworkManager.Instance.GetPlayerNick()))
				{
					PlayerManager.Instance.DustGainedSum(dustQuantity, save: false);
				}
				continue;
			}
			if (!GameManager.Instance.IsMultiplayer() || heroes[k].Owner == NetworkManager.Instance.GetPlayerNick())
			{
				num2++;
				if (replySelected.FlcUpgradeRandomCard)
				{
					AtOManager.Instance.UpgradeRandomCardToHero(k, replySelected.FlcMaxQuantity);
				}
				if ((bool)replySelected.FlcAddItem && (replySelected.RequiredClass == null || replySelected.RequiredClass.SubClassName == heroes[k].HeroData.HeroSubClass.SubClassName))
				{
					string id4 = replySelected.FlcAddItem.Id;
					cardCharacters.Add(AtOManager.Instance.GetHero(k).SourceName);
					StartCoroutine(GenerateRewardCard(ok: false, id4));
					if (!GameManager.Instance.IsMultiplayer())
					{
						AtOManager.Instance.AddItemToHero(k, id4);
					}
					else
					{
						AtOManager.Instance.AddItemToHeroMP(k, id4);
					}
				}
				if (replySelected.FlcRemoveItemSlot != Enums.ItemSlot.None && (replySelected.RequiredClass == null || replySelected.RequiredClass.SubClassName == heroes[k].HeroData.HeroSubClass.SubClassName))
				{
					List<string> list7 = new List<string>();
					if (replySelected.FlcRemoveItemSlot == Enums.ItemSlot.AllWithoutPet)
					{
						list7.Add("weapon");
						list7.Add("armor");
						list7.Add("jewelry");
						list7.Add("accesory");
					}
					else if (replySelected.FlcRemoveItemSlot == Enums.ItemSlot.AllIncludedPet)
					{
						list7.Add("weapon");
						list7.Add("armor");
						list7.Add("jewelry");
						list7.Add("accesory");
						list7.Add("pet");
					}
					else
					{
						list7.Add(replySelected.FlcRemoveItemSlot.ToString().ToLower());
					}
					for (int num11 = 0; num11 < list7.Count; num11++)
					{
						AtOManager.Instance.RemoveItemFromHeroFromEvent(_isHero: true, k, list7[num11]);
					}
				}
				if (replySelected.FlcCorruptItemSlot != Enums.ItemSlot.None && (replySelected.RequiredClass == null || replySelected.RequiredClass.SubClassName == heroes[k].HeroData.HeroSubClass.SubClassName))
				{
					List<string> list8 = new List<string>();
					if (replySelected.FlcCorruptItemSlot == Enums.ItemSlot.AllWithoutPet)
					{
						list8.Add("weapon");
						list8.Add("armor");
						list8.Add("jewelry");
						list8.Add("accesory");
					}
					else if (replySelected.FlcCorruptItemSlot == Enums.ItemSlot.AllIncludedPet)
					{
						list8.Add("weapon");
						list8.Add("armor");
						list8.Add("jewelry");
						list8.Add("accesory");
						list8.Add("pet");
					}
					else
					{
						list8.Add(replySelected.FlcCorruptItemSlot.ToString().ToLower());
					}
					for (int num12 = 0; num12 < list8.Count; num12++)
					{
						AtOManager.Instance.CorruptItemSlot(k, list8[num12]);
					}
				}
			}
			AtOManager.Instance.ModifyHeroLife(k, replySelected.FlcRewardHealthFlat, replySelected.FlcRewardHealthPercent);
			if (replySelected.FlcExperienceReward != 0)
			{
				int num13 = heroes[k].CalculateRewardForCharacter(replySelected.FlcExperienceReward);
				num4 += num13;
				heroes[k].GrantExperience(num13);
			}
			if (replySelected.FlcGoldReward != 0)
			{
				goldQuantity = replySelected.FlcGoldReward;
				if (MadnessManager.Instance.IsMadnessTraitActive("poverty") || AtOManager.Instance.IsChallengeTraitActive("poverty"))
				{
					if (!GameManager.Instance.IsObeliskChallenge())
					{
						goldQuantity -= Functions.FuncRoundToInt((float)goldQuantity * 0.5f);
					}
					else
					{
						goldQuantity -= Functions.FuncRoundToInt((float)goldQuantity * 0.3f);
					}
					if (goldQuantity < 0)
					{
						goldQuantity = 0;
					}
				}
				if (AtOManager.Instance.IsChallengeTraitActive("prosperity"))
				{
					goldQuantity += Functions.FuncRoundToInt((float)goldQuantity * 0.5f);
				}
				AtOManager.Instance.GivePlayer(0, goldQuantity, heroes[k].Owner);
				if (goldQuantity > 0 && (!GameManager.Instance.IsMultiplayer() || heroes[k].Owner == NetworkManager.Instance.GetPlayerNick()))
				{
					PlayerManager.Instance.GoldGainedSum(goldQuantity, save: false);
				}
			}
			if (replySelected.FlcDustReward == 0)
			{
				continue;
			}
			dustQuantity = replySelected.FlcDustReward;
			if (MadnessManager.Instance.IsMadnessTraitActive("poverty") || AtOManager.Instance.IsChallengeTraitActive("poverty"))
			{
				if (!GameManager.Instance.IsObeliskChallenge())
				{
					dustQuantity -= Functions.FuncRoundToInt((float)dustQuantity * 0.5f);
				}
				else
				{
					dustQuantity -= Functions.FuncRoundToInt((float)dustQuantity * 0.3f);
				}
				if (dustQuantity < 0)
				{
					dustQuantity = 0;
				}
			}
			if (AtOManager.Instance.IsChallengeTraitActive("prosperity"))
			{
				dustQuantity += Functions.FuncRoundToInt((float)dustQuantity * 0.5f);
			}
			AtOManager.Instance.GivePlayer(1, dustQuantity, heroes[k].Owner);
			if (dustQuantity > 0 && (!GameManager.Instance.IsMultiplayer() || heroes[k].Owner == NetworkManager.Instance.GetPlayerNick()))
			{
				PlayerManager.Instance.DustGainedSum(dustQuantity, save: false);
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		string text = "";
		if (flag)
		{
			if (!criticalSuccess)
			{
				destinationNode = replySelected.SsNodeTravel;
				followUpCombatData = replySelected.SsCombat;
				followUpEventData = replySelected.SsEvent;
				if (!isGroup)
				{
					stringBuilder.Append("<color=#098C20><u>");
					stringBuilder.Append(Texts.Instance.GetText("success"));
					stringBuilder.Append("</u></color>\n\n");
				}
				text = Texts.Instance.GetText(currentEvent.EventId + "_rp" + replySelected.IndexForAnswerTranslation + "_s", "events");
				if (text != "")
				{
					stringBuilder.Append(text);
				}
				else
				{
					Debug.LogError(currentEvent.EventId + " <" + optionSelected + "-> success> missing translation");
					stringBuilder.Append(replySelected.SsRewardText);
				}
				bool flag3 = false;
				if (replySelected.SsGoldReward != 0 || replySelected.SsDustReward != 0 || num3 > 0 || replySelected.SsSupplyReward != 0)
				{
					stringBuilder.Append("\n\n<color=#202020>");
					stringBuilder.Append(Texts.Instance.GetText("eventYouGet"));
					stringBuilder.Append(" ");
					flag3 = true;
				}
				if (replySelected.SsGoldReward != 0)
				{
					stringBuilder.Append(" <sprite name=gold>");
					stringBuilder.Append(goldQuantity * num);
					stringBuilder.Append("  ");
				}
				if (replySelected.SsDustReward != 0)
				{
					stringBuilder.Append(" <sprite name=dust>");
					stringBuilder.Append(dustQuantity * num);
					stringBuilder.Append("  ");
				}
				if (num3 > 0)
				{
					stringBuilder.Append(" <sprite name=experience>");
					stringBuilder.Append(num3);
					stringBuilder.Append("  ");
				}
				if (replySelected.SsSupplyReward != 0)
				{
					stringBuilder.Append(" <sprite name=supply>");
					stringBuilder.Append(replySelected.SsSupplyReward);
					stringBuilder.Append("  ");
					PlayerManager.Instance.GainSupply(replySelected.SsSupplyReward);
				}
				if (replySelected.SsPerkData != null)
				{
					stringBuilder.Append("<sprite name=");
					stringBuilder.Append(replySelected.SsPerkData.Icon.name);
					stringBuilder.Append("><space=-.1>");
					stringBuilder.Append(replySelected.SsPerkData.IconTextValue);
					stringBuilder.Append("  ");
				}
				if (replySelected.SsPerkData1 != null)
				{
					stringBuilder.Append("<sprite name=");
					stringBuilder.Append(replySelected.SsPerkData1.Icon.name);
					stringBuilder.Append("><space=-.1>");
					stringBuilder.Append(replySelected.SsPerkData1.IconTextValue);
					stringBuilder.Append("  ");
				}
				if (flag3)
				{
					stringBuilder.Append("</color>");
				}
				if (replySelected.SsRequirementUnlock != null)
				{
					AtOManager.Instance.AddPlayerRequirement(replySelected.SsRequirementUnlock);
				}
				if (replySelected.SsRequirementUnlock2 != null)
				{
					AtOManager.Instance.AddPlayerRequirement(replySelected.SsRequirementUnlock2);
				}
				if (replySelected.SsAddCard1 != null)
				{
					StartCoroutine(GenerateRewardCard(ok: true, replySelected.SsAddCard1.Id));
				}
				if (replySelected.SsRewardTier != null)
				{
					AtOManager.Instance.SetEventRewardTier(replySelected.SsRewardTier);
				}
				if ((bool)replySelected.SsRequirementLock2)
				{
					AtOManager.Instance.RemovePlayerRequirement(replySelected.SsRequirementLock2);
				}
				if ((bool)replySelected.SsRequirementLock)
				{
					AtOManager.Instance.RemovePlayerRequirement(replySelected.SsRequirementLock);
				}
				if (replySelected.SsUpgradeUI)
				{
					followUpUpgrade = true;
					followUpDiscount = replySelected.SsDiscount;
					followUpMaxQuantity = replySelected.SsMaxQuantity;
				}
				if (replySelected.SsHealerUI)
				{
					followUpHealer = true;
					followUpDiscount = replySelected.SsDiscount;
					followUpMaxQuantity = replySelected.SsMaxQuantity;
				}
				if (replySelected.SsHealerUI)
				{
					followUpHealer = true;
					followUpDiscount = replySelected.SsDiscount;
					followUpMaxQuantity = replySelected.SsMaxQuantity;
				}
				if (replySelected.SsCraftUI)
				{
					followUpCraft = true;
					followUpDiscount = replySelected.SsDiscount;
					followUpMaxQuantity = replySelected.SsMaxQuantity;
					followUpMaxCraftRarity = replySelected.SsCraftUIMaxType;
				}
				if (replySelected.SsCorruptionUI)
				{
					followUpCorruption = true;
					followUpDiscount = replySelected.SsDiscount;
					followUpMaxQuantity = replySelected.SsMaxQuantity;
				}
				if (replySelected.SsItemCorruptionUI)
				{
					followUpItemCorruption = true;
					followUpDiscount = replySelected.SsDiscount;
					followUpMaxQuantity = replySelected.SsMaxQuantity;
				}
				if (replySelected.SsShopList != null)
				{
					followUpShopListId = replySelected.SsShopList.Id;
					followUpDiscount = replySelected.SsDiscount;
				}
				if (replySelected.SsLootList != null)
				{
					followUpLootListId = replySelected.SsLootList.Id;
				}
				if (replySelected.SsCardPlayerGame)
				{
					followUpCardPlayerGame = true;
					followUpCardPlayerGamePack = replySelected.SsCardPlayerGamePackData;
				}
				if (replySelected.SsCardPlayerPairsGame)
				{
					followUpCardPlayerPairsGame = true;
					followUpCardPlayerPairsGamePack = replySelected.SsCardPlayerPairsGamePackData;
				}
				if ((bool)replySelected.SsUnlockClass)
				{
					AtOManager.Instance.characterUnlockData = replySelected.SsUnlockClass;
				}
				if ((bool)replySelected.SsUnlockSkin)
				{
					AtOManager.Instance.skinUnlockData = replySelected.SsUnlockSkin;
				}
				if (replySelected.SsUnlockSteamAchievement != "")
				{
					PlayerManager.Instance.AchievementUnlock(replySelected.SsUnlockSteamAchievement);
				}
				if (replySelected.SsFinishEarlyAccess)
				{
					AtOManager.Instance.FinishGame();
					return;
				}
				if (replySelected.SsFinishGame)
				{
					AtOManager.Instance.FinishRun();
					return;
				}
				if (replySelected.SsFinishObeliskMap)
				{
					ZoneData zoneData = Globals.Instance.ZoneDataSource[AtOManager.Instance.GetTownZoneId().ToLower()];
					if (zoneData != null && zoneData.ObeliskLow)
					{
						destinationNode = MapManager.Instance.GetNodeFromId(AtOManager.Instance.obeliskHigh + "_0").nodeData;
						AtOManager.Instance.UpgradeTownTier();
					}
					else
					{
						if (!(zoneData != null) || !zoneData.ObeliskHigh)
						{
							AtOManager.Instance.FinishObeliskChallenge();
							return;
						}
						destinationNode = MapManager.Instance.GetNodeFromId(AtOManager.Instance.obeliskFinal + "_0").nodeData;
						AtOManager.Instance.UpgradeTownTier();
					}
				}
				if (replySelected.SsCharacterReplacement != null && replySelected.SsCharacterReplacementPosition > -1 && replySelected.SsCharacterReplacementPosition < 4)
				{
					AtOManager.Instance.SwapCharacter(replySelected.SsCharacterReplacement, replySelected.SsCharacterReplacementPosition);
				}
			}
			else
			{
				if (replySelected.SscNodeTravel != null)
				{
					destinationNode = replySelected.SscNodeTravel;
				}
				else if (replySelected.SsNodeTravel != null)
				{
					destinationNode = replySelected.SscNodeTravel;
				}
				followUpCombatData = replySelected.SscCombat;
				followUpEventData = replySelected.SscEvent;
				if (!isGroup)
				{
					stringBuilder.Append("<color=#098C20><u>");
					stringBuilder.Append(Texts.Instance.GetText("success"));
					stringBuilder.Append("</u></color>\n\n");
				}
				text = Texts.Instance.GetText(currentEvent.EventId + "_rp" + replySelected.IndexForAnswerTranslation + "_sc", "events");
				if (text != "")
				{
					stringBuilder.Append(text);
				}
				else
				{
					Debug.LogError(currentEvent.EventId + " <" + optionSelected + "-> successCrit> missing translation");
					stringBuilder.Append(replySelected.SscRewardText);
				}
				bool flag4 = false;
				if (replySelected.SscGoldReward != 0 || replySelected.SscDustReward != 0 || num3 > 0 || replySelected.SscSupplyReward != 0)
				{
					stringBuilder.Append("\n\n<color=#202020>");
					stringBuilder.Append(Texts.Instance.GetText("eventYouGet"));
					stringBuilder.Append(" ");
					flag4 = true;
				}
				if (replySelected.SscGoldReward != 0)
				{
					stringBuilder.Append(" <sprite name=gold>");
					stringBuilder.Append(goldQuantity * num);
					stringBuilder.Append("  ");
				}
				if (replySelected.SscDustReward != 0)
				{
					stringBuilder.Append(" <sprite name=dust>");
					stringBuilder.Append(dustQuantity * num);
					stringBuilder.Append("  ");
				}
				if (num3 > 0)
				{
					stringBuilder.Append(" <sprite name=experience>");
					stringBuilder.Append(num3);
					stringBuilder.Append("  ");
				}
				if (replySelected.SscSupplyReward != 0)
				{
					stringBuilder.Append(" <sprite name=supply>");
					stringBuilder.Append(replySelected.SscSupplyReward);
					stringBuilder.Append("  ");
					PlayerManager.Instance.GainSupply(replySelected.SscSupplyReward);
				}
				if (flag4)
				{
					stringBuilder.Append("</color>");
				}
				if (replySelected.SscRequirementUnlock != null)
				{
					AtOManager.Instance.AddPlayerRequirement(replySelected.SscRequirementUnlock);
				}
				if (replySelected.SscRequirementUnlock2 != null)
				{
					AtOManager.Instance.AddPlayerRequirement(replySelected.SscRequirementUnlock2);
				}
				if (replySelected.SscAddCard1 != null)
				{
					StartCoroutine(GenerateRewardCard(ok: true, replySelected.SscAddCard1.Id));
				}
				if (replySelected.SscRewardTier != null)
				{
					AtOManager.Instance.SetEventRewardTier(replySelected.SscRewardTier);
				}
				if ((bool)replySelected.SscRequirementLock)
				{
					AtOManager.Instance.RemovePlayerRequirement(replySelected.SscRequirementLock);
				}
				if (replySelected.SscUpgradeUI)
				{
					followUpUpgrade = true;
					followUpDiscount = replySelected.SscDiscount;
					followUpMaxQuantity = replySelected.SscMaxQuantity;
				}
				if (replySelected.SscHealerUI)
				{
					followUpHealer = true;
					followUpDiscount = replySelected.SscDiscount;
					followUpMaxQuantity = replySelected.SscMaxQuantity;
				}
				if (replySelected.SscHealerUI)
				{
					followUpHealer = true;
					followUpDiscount = replySelected.SscDiscount;
					followUpMaxQuantity = replySelected.SscMaxQuantity;
				}
				if (replySelected.SscCraftUI)
				{
					followUpCraft = true;
					followUpDiscount = replySelected.SscDiscount;
					followUpMaxQuantity = replySelected.SscMaxQuantity;
					followUpMaxCraftRarity = replySelected.SscCraftUIMaxType;
				}
				if (replySelected.SscCorruptionUI)
				{
					followUpCorruption = true;
					followUpDiscount = replySelected.SscDiscount;
					followUpMaxQuantity = replySelected.SscMaxQuantity;
				}
				if (replySelected.SscItemCorruptionUI)
				{
					followUpItemCorruption = true;
					followUpDiscount = replySelected.SscDiscount;
					followUpMaxQuantity = replySelected.SscMaxQuantity;
				}
				if (replySelected.SscShopList != null)
				{
					followUpShopListId = replySelected.SscShopList.Id;
					followUpDiscount = replySelected.SscDiscount;
				}
				if (replySelected.SscLootList != null)
				{
					followUpLootListId = replySelected.SscLootList.Id;
				}
				if (replySelected.SscCardPlayerGame)
				{
					followUpCardPlayerGame = true;
					followUpCardPlayerGamePack = replySelected.SscCardPlayerGamePackData;
				}
				if (replySelected.SscCardPlayerPairsGame)
				{
					followUpCardPlayerPairsGame = true;
					followUpCardPlayerPairsGamePack = replySelected.SscCardPlayerPairsGamePackData;
				}
				if ((bool)replySelected.SscUnlockClass)
				{
					AtOManager.Instance.characterUnlockData = replySelected.SscUnlockClass;
				}
				if (replySelected.SscUnlockSteamAchievement != "")
				{
					PlayerManager.Instance.AchievementUnlock(replySelected.SscUnlockSteamAchievement);
				}
				if (replySelected.SscFinishEarlyAccess)
				{
					AtOManager.Instance.FinishGame();
					return;
				}
				if (replySelected.SscFinishGame)
				{
					AtOManager.Instance.FinishRun();
					return;
				}
			}
		}
		if (flag2)
		{
			if (!criticalFail)
			{
				destinationNode = replySelected.FlNodeTravel;
				followUpCombatData = replySelected.FlCombat;
				followUpEventData = replySelected.FlEvent;
				if (flag)
				{
					stringBuilder.Append("\n\n\n");
				}
				if (!isGroup)
				{
					stringBuilder.Append("<color=#980B06><u>");
					stringBuilder.Append(Texts.Instance.GetText("fail"));
					stringBuilder.Append("</u></color>\n\n");
				}
				text = Texts.Instance.GetText(currentEvent.EventId + "_rp" + replySelected.IndexForAnswerTranslation + "_f", "events");
				if (text != "")
				{
					stringBuilder.Append(text);
				}
				else
				{
					Debug.LogError(currentEvent.EventId + " <" + optionSelected + "-> fail> missing translation");
					stringBuilder.Append(replySelected.FlRewardText);
				}
				if (replySelected.FlGoldReward != 0 || replySelected.FlDustReward != 0 || num4 > 0 || replySelected.FlSupplyReward != 0)
				{
					stringBuilder.Append("\n\n<color=#202020>");
					stringBuilder.Append(Texts.Instance.GetText("eventYouGet"));
					stringBuilder.Append(" ");
				}
				if (replySelected.FlGoldReward != 0)
				{
					stringBuilder.Append(" <sprite name=gold>");
					stringBuilder.Append(goldQuantity * num2);
					stringBuilder.Append("  ");
				}
				if (replySelected.FlDustReward != 0)
				{
					stringBuilder.Append(" <sprite name=dust>");
					stringBuilder.Append(dustQuantity * num2);
					stringBuilder.Append("  ");
				}
				if (num4 > 0)
				{
					stringBuilder.Append(" <sprite name=experience>");
					stringBuilder.Append(num4);
					stringBuilder.Append("  ");
				}
				if (replySelected.FlSupplyReward != 0)
				{
					stringBuilder.Append(" <sprite name=supply>");
					stringBuilder.Append(replySelected.FlSupplyReward);
					stringBuilder.Append("  ");
					PlayerManager.Instance.GainSupply(replySelected.FlSupplyReward);
				}
				stringBuilder.Append("\n");
				if (replySelected.FlRequirementUnlock != null)
				{
					AtOManager.Instance.AddPlayerRequirement(replySelected.FlRequirementUnlock);
				}
				if (replySelected.FlRequirementUnlock2 != null)
				{
					AtOManager.Instance.AddPlayerRequirement(replySelected.FlRequirementUnlock2);
				}
				if (replySelected.FlAddCard1 != null)
				{
					StartCoroutine(GenerateRewardCard(ok: false, replySelected.FlAddCard1.Id));
				}
				if (replySelected.FlRewardTier != null)
				{
					AtOManager.Instance.SetEventRewardTier(replySelected.FlRewardTier);
				}
				if ((bool)replySelected.FlRequirementLock)
				{
					AtOManager.Instance.RemovePlayerRequirement(replySelected.FlRequirementLock);
				}
				if (replySelected.FlUpgradeUI)
				{
					followUpUpgrade = true;
					followUpDiscount = replySelected.FlDiscount;
					followUpMaxQuantity = replySelected.FlMaxQuantity;
				}
				if (replySelected.FlHealerUI)
				{
					followUpHealer = true;
					followUpDiscount = replySelected.FlDiscount;
					followUpMaxQuantity = replySelected.FlMaxQuantity;
				}
				if (replySelected.FlCraftUI)
				{
					followUpCraft = true;
					followUpDiscount = replySelected.FlDiscount;
					followUpMaxQuantity = replySelected.FlMaxQuantity;
					followUpMaxCraftRarity = replySelected.FlCraftUIMaxType;
				}
				if (replySelected.FlCorruptionUI)
				{
					followUpCorruption = true;
					followUpDiscount = replySelected.FlDiscount;
					followUpMaxQuantity = replySelected.FlMaxQuantity;
				}
				if (replySelected.FlItemCorruptionUI)
				{
					followUpItemCorruption = true;
					followUpDiscount = replySelected.FlDiscount;
					followUpMaxQuantity = replySelected.FlMaxQuantity;
				}
				if (replySelected.FlShopList != null)
				{
					followUpShopListId = replySelected.FlShopList.Id;
					followUpDiscount = replySelected.FlDiscount;
				}
				if (replySelected.FlLootList != null)
				{
					followUpLootListId = replySelected.FlLootList.Id;
				}
				if (replySelected.FlCardPlayerGame)
				{
					followUpCardPlayerGame = true;
					followUpCardPlayerGamePack = replySelected.FlCardPlayerGamePackData;
				}
				if (replySelected.FlCardPlayerPairsGame)
				{
					followUpCardPlayerPairsGame = true;
					followUpCardPlayerPairsGamePack = replySelected.FlCardPlayerPairsGamePackData;
				}
				if ((bool)replySelected.FlUnlockClass)
				{
					AtOManager.Instance.characterUnlockData = replySelected.FlUnlockClass;
				}
				if (replySelected.FlUnlockSteamAchievement != "")
				{
					PlayerManager.Instance.AchievementUnlock(replySelected.FlUnlockSteamAchievement);
				}
			}
			else
			{
				if (replySelected.FlcNodeTravel != null)
				{
					destinationNode = replySelected.FlcNodeTravel;
				}
				else if (replySelected.FlNodeTravel != null)
				{
					destinationNode = replySelected.FlNodeTravel;
				}
				followUpCombatData = replySelected.FlcCombat;
				followUpEventData = replySelected.FlcEvent;
				if (flag)
				{
					stringBuilder.Append("\n\n\n");
				}
				if (!isGroup)
				{
					stringBuilder.Append("<color=#980B06><u>");
					stringBuilder.Append(Texts.Instance.GetText("failCritical"));
					stringBuilder.Append("</u></color>\n\n");
				}
				text = Texts.Instance.GetText(currentEvent.EventId + "_rp" + replySelected.IndexForAnswerTranslation + "_fc", "events");
				if (text != "")
				{
					stringBuilder.Append(text);
				}
				else
				{
					Debug.LogError(currentEvent.EventId + " <" + optionSelected + "-> failCrit> missing translation");
					stringBuilder.Append(replySelected.FlcRewardText);
				}
				if (replySelected.FlcGoldReward != 0 || replySelected.FlcDustReward != 0 || num4 > 0 || replySelected.FlcSupplyReward != 0)
				{
					stringBuilder.Append("\n\n<color=#202020>");
					stringBuilder.Append(Texts.Instance.GetText("eventYouGet"));
					stringBuilder.Append(" ");
				}
				if (replySelected.FlcGoldReward != 0)
				{
					stringBuilder.Append(" <sprite name=gold>");
					stringBuilder.Append(goldQuantity * num2);
					stringBuilder.Append("  ");
				}
				if (replySelected.FlcDustReward != 0)
				{
					stringBuilder.Append(" <sprite name=dust>");
					stringBuilder.Append(dustQuantity * num2);
					stringBuilder.Append("  ");
				}
				if (num4 > 0)
				{
					stringBuilder.Append(" <sprite name=experience>");
					stringBuilder.Append(num4);
					stringBuilder.Append("  ");
				}
				if (replySelected.FlcSupplyReward != 0)
				{
					stringBuilder.Append(" <sprite name=supply>");
					stringBuilder.Append(replySelected.FlcSupplyReward);
					stringBuilder.Append("  ");
					PlayerManager.Instance.GainSupply(replySelected.FlcSupplyReward);
				}
				stringBuilder.Append("\n");
				if (replySelected.FlcRequirementUnlock != null)
				{
					AtOManager.Instance.AddPlayerRequirement(replySelected.FlcRequirementUnlock);
				}
				if (replySelected.FlcRequirementUnlock2 != null)
				{
					AtOManager.Instance.AddPlayerRequirement(replySelected.FlcRequirementUnlock2);
				}
				if (replySelected.FlcAddCard1 != null)
				{
					StartCoroutine(GenerateRewardCard(ok: false, replySelected.FlcAddCard1.Id));
				}
				if (replySelected.FlcRewardTier != null)
				{
					AtOManager.Instance.SetEventRewardTier(replySelected.FlcRewardTier);
				}
				if ((bool)replySelected.FlcRequirementLock)
				{
					AtOManager.Instance.RemovePlayerRequirement(replySelected.FlcRequirementLock);
				}
				if (replySelected.FlcUpgradeUI)
				{
					followUpUpgrade = true;
					followUpDiscount = replySelected.FlcDiscount;
					followUpMaxQuantity = replySelected.FlcMaxQuantity;
				}
				if (replySelected.FlcHealerUI)
				{
					followUpHealer = true;
					followUpDiscount = replySelected.FlcDiscount;
					followUpMaxQuantity = replySelected.FlcMaxQuantity;
				}
				if (replySelected.FlcCraftUI)
				{
					followUpCraft = true;
					followUpDiscount = replySelected.FlcDiscount;
					followUpMaxQuantity = replySelected.FlcMaxQuantity;
					followUpMaxCraftRarity = replySelected.FlcCraftUIMaxType;
				}
				if (replySelected.FlcCorruptionUI)
				{
					followUpCorruption = true;
					followUpDiscount = replySelected.FlcDiscount;
					followUpMaxQuantity = replySelected.FlcMaxQuantity;
				}
				if (replySelected.FlcItemCorruptionUI)
				{
					followUpItemCorruption = true;
					followUpDiscount = replySelected.FlcDiscount;
					followUpMaxQuantity = replySelected.FlcMaxQuantity;
				}
				if (replySelected.FlcShopList != null)
				{
					followUpShopListId = replySelected.FlcShopList.Id;
					followUpDiscount = replySelected.FlcDiscount;
				}
				if (replySelected.FlcLootList != null)
				{
					followUpLootListId = replySelected.FlcLootList.Id;
				}
				if (replySelected.FlcCardPlayerGame)
				{
					followUpCardPlayerGame = true;
					followUpCardPlayerGamePack = replySelected.FlcCardPlayerGamePackData;
				}
				if (replySelected.FlcCardPlayerPairsGame)
				{
					followUpCardPlayerPairsGame = true;
					followUpCardPlayerPairsGamePack = replySelected.FlcCardPlayerPairsGamePackData;
				}
				if ((bool)replySelected.FlcUnlockClass)
				{
					AtOManager.Instance.characterUnlockData = replySelected.FlcUnlockClass;
				}
				if (replySelected.FlcUnlockSteamAchievement != "")
				{
					PlayerManager.Instance.AchievementUnlock(replySelected.FlcUnlockSteamAchievement);
				}
			}
		}
		if (!replySelected.SsRoll)
		{
			RectTransform component = result.GetComponent<RectTransform>();
			component.localPosition = new Vector2(component.localPosition.x, 1.2f);
			result.fontSizeMin = 2.4f;
			result.transform.position = new Vector3(result.transform.position.x, 1.4f, result.transform.position.z);
		}
		stringBuilder.Replace("(", "<color=#333><size=-.2><voffset=.2>(");
		stringBuilder.Replace(")", ")</voffset></size></color>");
		result.text = stringBuilder.ToString();
		result.gameObject.SetActive(value: true);
		if (MapManager.Instance != null)
		{
			MapManager.Instance.sideCharacters.Refresh();
		}
		continueButton.gameObject.SetActive(value: true);
	}

	private IEnumerator GenerateRewardCard(bool ok, string cardName, string subclassName = "")
	{
		yield return Globals.Instance.WaitForSeconds(0.2f);
		CardData cardData = Globals.Instance.GetCardData(cardName);
		if (cardData != null && cardData.CardClass == Enums.CardClass.Injury)
		{
			int ngPlus = AtOManager.Instance.GetNgPlus();
			if (ngPlus > 0)
			{
				if (ngPlus >= 3 && ngPlus <= 4 && cardData.UpgradesTo1 != "")
				{
					cardName = cardData.UpgradesTo1;
					cardData = Globals.Instance.GetCardData(cardName);
				}
				else if (ngPlus >= 5 && cardData.UpgradesTo2 != "")
				{
					cardName = cardData.UpgradesTo2;
					cardData = Globals.Instance.GetCardData(cardName);
				}
			}
		}
		if (cardData == null)
		{
			yield break;
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < cardCharacters.Count; i++)
		{
			if (i > 0)
			{
				stringBuilder.Append(", ");
			}
			stringBuilder.Append(cardCharacters[i]);
		}
		if (cardCharacters.Count == 0 && !subclassName.IsNullOrEmpty())
		{
			stringBuilder.Append(Texts.Instance.GetText(subclassName.ToLower().Replace(" ", "") + "_name", "class"));
		}
		stringBuilder.Append("\n");
		string text;
		if (cardCharacters.Count > 1)
		{
			text = Texts.Instance.GetText("charsReceives");
			text = text.Replace("<chars>", stringBuilder.ToString());
		}
		else
		{
			text = Texts.Instance.GetText("charReceive");
			text = text.Replace("<char>", stringBuilder.ToString());
		}
		GameObject gameObject;
		if (cardData.CardClass == Enums.CardClass.Injury)
		{
			cardKO.gameObject.SetActive(value: true);
			cardKOText.text = text;
			gameObject = Object.Instantiate(GameManager.Instance.CardPrefab, new Vector3(-2.5f, 0f, 0f), Quaternion.identity, cardKOCards);
		}
		else
		{
			cardOK.gameObject.SetActive(value: true);
			cardOKText.text = text;
			gameObject = Object.Instantiate(GameManager.Instance.CardPrefab, new Vector3(-2.5f, 0f, 0f), Quaternion.identity, cardOKCards);
		}
		CardItem component = gameObject.GetComponent<CardItem>();
		component.SetCard(cardName, deckScale: false);
		gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
		component.SetDestinationScaleRotation(new Vector3(3.3f, 1.1f, -1f), 1.4f, Quaternion.Euler(0f, 0f, 0f));
		component.cardrevealed = true;
		component.TopLayeringOrder("Book", 20000);
		component.CreateColliderAdjusted();
		if (cardData.CardType != Enums.CardType.Pet || cardData.CardRarity != Enums.CardRarity.Rare)
		{
			PlayerManager.Instance.CardUnlock(cardName, save: true, component);
		}
	}

	private IEnumerator ShowResultTitle(bool success, int heroIndex = -1)
	{
		if (AtOManager.Instance.Sandbox_alwaysPassEventRoll)
		{
			success = true;
		}
		TMP_Text textObj;
		if (success)
		{
			if (heroIndex == -1)
			{
				textObj = resultOK;
				if (criticalSuccess)
				{
					textObj = resultOKc;
				}
			}
			else
			{
				textObj = charTresultOK[heroIndex];
			}
		}
		else if (heroIndex == -1)
		{
			textObj = resultKO;
			if (criticalFail)
			{
				textObj = resultKOc;
			}
		}
		else
		{
			textObj = charTresultKO[heroIndex];
		}
		Color colorOri = textObj.color;
		textObj.gameObject.SetActive(value: true);
		float alpha = colorOri.a;
		while (alpha < 1f)
		{
			alpha += 0.1f;
			textObj.color = new Color(colorOri.r, colorOri.g, colorOri.b, alpha);
			yield return Globals.Instance.WaitForSeconds(0.1f);
		}
	}

	private void TurnOffCharacter(int heroIndex)
	{
		characterSPR[heroIndex].color = new Color(0.3f, 0.3f, 0.3f, 1f);
	}

	public void SetWaitingPlayersText(string msg)
	{
		if (msg != "")
		{
			waitingMsg.gameObject.SetActive(value: true);
			waitingMsgText.text = msg;
		}
		else
		{
			waitingMsg.gameObject.SetActive(value: false);
		}
		if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster() || !AtOManager.Instance.followingTheLeader)
		{
			return;
		}
		if (NetworkManager.Instance.IsMasterReady())
		{
			if (!statusReady)
			{
				Ready(forceIt: true);
			}
		}
		else if (statusReady)
		{
			Ready(forceIt: true);
		}
	}

	public void Ready(bool forceIt = false)
	{
		if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster() && AtOManager.Instance.followingTheLeader && !forceIt)
		{
			return;
		}
		if (!GameManager.Instance.IsMultiplayer() || TownManager.Instance != null)
		{
			CloseEvent();
			return;
		}
		if (manualReadyCo != null)
		{
			StopCoroutine(manualReadyCo);
		}
		statusReady = !statusReady;
		NetworkManager.Instance.SetManualReady(statusReady);
		if (statusReady)
		{
			continueButton.GetComponent<BotonGeneric>().SetBackgroundColor(Functions.HexToColor(Globals.Instance.ClassColor["scout"]));
			continueButton.GetComponent<BotonGeneric>().SetText(Texts.Instance.GetText("waitingForPlayers"));
			if (NetworkManager.Instance.IsMaster())
			{
				manualReadyCo = StartCoroutine(CheckForAllManualReady());
			}
		}
		else
		{
			continueButton.GetComponent<BotonGeneric>().SetBackgroundColor(Functions.HexToColor(Globals.Instance.ClassColor["warrior"]));
			continueButton.GetComponent<BotonGeneric>().SetText(Texts.Instance.GetText("continueAction"));
		}
	}

	private IEnumerator CheckForAllManualReady()
	{
		bool check = true;
		while (check)
		{
			if (!NetworkManager.Instance.AllPlayersManualReady())
			{
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
			else
			{
				check = false;
			}
		}
		photonView.RPC("NET_CloseEvent", RpcTarget.Others);
		CloseEvent();
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false, int absolutePosition = -1)
	{
		_controllerList.Clear();
		if (Functions.TransformIsVisible(continueButton))
		{
			_controllerList.Add(continueButton);
		}
		else if (replysGOs.Length != 0)
		{
			for (int i = 0; i < replysGOs.Length; i++)
			{
				if (replysGOs[i] != null)
				{
					_controllerList.Add(replysGOs[i].transform);
					if (Functions.TransformIsVisible(replys[i].probDice))
					{
						_controllerList.Add(replys[i].probDice);
					}
				}
			}
		}
		_controllerList.Add(MapManager.Instance.eventShowHideButton);
		for (int j = 0; j < 4; j++)
		{
			if (Functions.TransformIsVisible(MapManager.Instance.sideCharacters.charArray[j].transform))
			{
				_controllerList.Add(MapManager.Instance.sideCharacters.charArray[j].transform.GetChild(0).transform);
			}
		}
		if (Functions.TransformIsVisible(PlayerUIManager.Instance.giveGold))
		{
			_controllerList.Add(PlayerUIManager.Instance.giveGold);
		}
		controllerHorizontalIndex = Functions.GetListClosestIndexToMousePosition(_controllerList);
		controllerHorizontalIndex = Functions.GetClosestIndexBasedOnDirection(_controllerList, controllerHorizontalIndex, goingUp, goingRight, goingDown, goingLeft);
		if (_controllerList[controllerHorizontalIndex] != null)
		{
			warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerList[controllerHorizontalIndex].position);
			Mouse.current.WarpCursorPosition(warpPosition);
		}
	}
}
