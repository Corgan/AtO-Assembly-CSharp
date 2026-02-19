using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Paradox;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
	private PhotonView photonView;

	public Transform sceneCamera;

	public CorruptionManager corruption;

	public Transform worldTransform;

	public Transform trackList;

	public PopupNode popupNode;

	public Transform ItemCreator;

	public Transform maskObject;

	public Transform maskMP;

	public Image maskImage;

	public Node nodeActive;

	private List<string> roadTemp = new List<string>();

	private ZoneData zoneActive;

	private Dictionary<string, GameObject> zoneGOs;

	private Dictionary<string, Node> mapNode;

	private Dictionary<string, bool> canTravelDict;

	private List<string> mapNodes;

	private List<string> availableNodes;

	private bool gettingMapNodes;

	public GameObject sourceTmpRoad;

	public Transform tmpRoads;

	public Material materialTmpRoad;

	private Vector2 vectorLineTexture = new Vector2(0.6f, 0.6f);

	private Dictionary<string, Transform> roads;

	private string roadActive = "";

	public SideCharacters sideCharacters;

	public CharacterWindowUI characterWindow;

	public GameObject unlockCharacterPrefab;

	private GameObject unlockGO;

	public GameObject eventTrackPrefab;

	public GameObject eventPrefab;

	public Transform eventShowHideButton;

	public GameObject conflictPrefab;

	private ConflictManager conflict;

	private GameObject conflictGO;

	public MapLegend mapLegend;

	private GameObject eventGO;

	private EventManager eventManager;

	private Coroutine showPopupCo;

	private Coroutine maskCoroutine;

	private List<string> randomStringArr = new List<string>();

	private int randomIndex;

	public TMP_Text tipText;

	public Dictionary<string, string> playerSelectedNodesDict = new Dictionary<string, string>();

	private bool corruptionSetted;

	public bool selectedNode;

	public Transform giveWindow;

	public List<GameObject> mapList;

	private Coroutine followingCo;

	public ChallengeBossBanners challengeBossBanners;

	private List<Transform> availableNodesTransform;

	private List<Transform> instantTravelNodesTransform;

	private int controllerCurrentOption = -1;

	private int controllerShoulderCurrentOption = -1;

	private List<Transform> controllerList = new List<Transform>();

	private Vector2 warpPosition;

	public static MapManager Instance { get; private set; }

	public ConflictManager Conflict
	{
		get
		{
			return conflict;
		}
		set
		{
			conflict = value;
		}
	}

	public PhotonView GetPhotonView()
	{
		return photonView;
	}

	private void Awake()
	{
		if (GameManager.Instance == null)
		{
			SceneStatic.LoadByName("Map");
			return;
		}
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("Awake Map Manager", "general");
		}
		photonView = PhotonView.Get(this);
		sceneCamera.gameObject.SetActive(value: false);
		if (GameManager.Instance.IsObeliskChallenge())
		{
			AtOManager.Instance.SetObeliskNodes();
		}
		GetMapNodes(useDelay: true);
		corruption.gameObject.SetActive(value: false);
		NetworkManager.Instance.StartStopQueue(state: true);
	}

	private void Start()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("Begin Map", "general");
		}
		AtOManager.Instance.ClearReroll();
		AtOManager.Instance.ResetCombatScarab();
		eventShowHideButton.gameObject.SetActive(value: false);
		ShowMask(state: true);
		StartCoroutine(StartCo());
	}

	private IEnumerator StartCo()
	{
		yield return Globals.Instance.WaitForSeconds(0.1f);
		if (GameManager.Instance.IsMultiplayer())
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("**************************");
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("WaitingSyncro startmap", "net");
			}
			yield return Globals.Instance.WaitForSeconds(0.1f);
			if (NetworkManager.Instance.IsMaster())
			{
				while (!NetworkManager.Instance.AllPlayersReady("startmap"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked startmap", "net");
				}
				NetworkManager.Instance.PlayersNetworkContinue("startmap");
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("startmap", status: true);
				NetworkManager.Instance.SetStatusReady("startmap");
				while (NetworkManager.Instance.WaitingSyncro["startmap"])
				{
					yield return Globals.Instance.WaitForSeconds(0.1f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("startmap, we can continue!", "net");
				}
			}
		}
		int exhaustGettingMapNodes = 0;
		controllerCurrentOption = -1;
		while (gettingMapNodes && exhaustGettingMapNodes < 100)
		{
			yield return Globals.Instance.WaitForSeconds(0.01f);
			exhaustGettingMapNodes++;
		}
		if (exhaustGettingMapNodes > 100)
		{
			Debug.LogError("ERROR getting map nodes");
		}
		if (CurrentNode() == null || CurrentNode() == "")
		{
			AtOManager.Instance.BeginAdventure();
			yield break;
		}
		zoneActive = mapNode[CurrentNode()].nodeData.NodeZone;
		zoneGOs[zoneActive.ZoneId].SetActive(value: true);
		if (GameManager.Instance.IsObeliskChallenge())
		{
			if (zoneActive.ObeliskHigh)
			{
				zoneGOs["ChallengeHighMap"].SetActive(value: true);
			}
			else if (zoneActive.ObeliskFinal)
			{
				zoneGOs["ChallengeFinalMap"].SetActive(value: true);
			}
			else
			{
				zoneGOs["ChallengeCommonMap"].SetActive(value: true);
			}
		}
		AtOManager.Instance.SetCharInTown(_state: false);
		AtOManager.Instance.SetTownZoneId(zoneActive.ZoneId);
		GetRoads();
		AtOManager.Instance.ClearCombatThermometerData();
		BeginMap();
		Resize();
	}

	public void BeginMap()
	{
		AtOManager.Instance.NodeScore();
		selectedNode = false;
		if (GameManager.Instance.IsMultiplayer())
		{
			playerSelectedNodesDict.Clear();
		}
		StartCoroutine(BeginMapCo());
	}

	private IEnumerator BeginMapCo()
	{
		canTravelDict = new Dictionary<string, bool>();
		if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
		{
			AssignGameNodes();
			AtOManager.Instance.saveLoadStatus = true;
			AtOManager.Instance.SaveGame(-1, backUp: true);
			yield return Globals.Instance.WaitForSeconds(0.01f);
			while (AtOManager.Instance.saveLoadStatus)
			{
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
		}
		BeginMapContinue();
		yield return null;
	}

	private void BeginMapContinue()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("BeginMapContinue", "general");
		}
		AtOManager.Instance.ClearTeamNPC();
		ShowRequirementTracks();
		HideAllArrows();
		AtOManager.Instance.SetPositionText();
		sideCharacters.EnableAll();
		if (!GameManager.Instance.IsMultiplayer() || (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster()))
		{
			GenerateRandomStringBatch(100);
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			ClearMultiplayerSelectedNodes();
			StartCoroutine(WaitMultiplayer());
		}
		else if (AtOManager.Instance.fromEventDestinationNode != null)
		{
			string nodeId = AtOManager.Instance.fromEventDestinationNode.NodeId;
			AtOManager.Instance.fromEventDestinationNode = null;
			TravelToThisNode(GetNodeFromId(nodeId));
		}
		else if (AtOManager.Instance.fromEventCombatData != null)
		{
			DoCombat(AtOManager.Instance.fromEventCombatData);
		}
		else if (AtOManager.Instance.fromEventEventData != null)
		{
			DoEvent(AtOManager.Instance.fromEventEventData);
		}
		else
		{
			DrawMap();
			AssignGameNodes();
			BeginMapContinueEnd();
		}
	}

	private void DoMapTips()
	{
		tipText.gameObject.SetActive(value: false);
		List<string> tipsList = Texts.Instance.TipsList;
		UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
		int index = UnityEngine.Random.Range(0, tipsList.Count);
		string text = tipsList[index];
		if (text.Trim() != "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<line-height=100%>");
			stringBuilder.Append(text.Trim());
			tipText.text = Regex.Replace(stringBuilder.ToString(), "\\<icon=(\\w+)\\>", " <size=+1.2><voffset=-.5><sprite name=$1><voffset=0></size>");
		}
		tipText.gameObject.SetActive(value: true);
	}

	private void BeginMapContinueEnd()
	{
		if (GameManager.Instance.IsObeliskChallenge())
		{
			challengeBossBanners.SetBosses();
		}
		DoMapTips();
		PlayerUIManager.Instance.SetItems();
		ProgressManager.Instance.CheckProgress();
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("comingFromCombatDoRewards->" + AtOManager.Instance.comingFromCombatDoRewards, "map");
		}
		if (AtOManager.Instance.comingFromCombatDoRewards)
		{
			StartCoroutine(DoCorruptionReward());
		}
		else if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
		{
			ShowMask(state: false);
		}
		controllerList.Clear();
		if (Functions.TransformIsVisible(PlayerUIManager.Instance.giveGold))
		{
			controllerList.Add(PlayerUIManager.Instance.giveGold);
		}
		for (int i = 0; i < sideCharacters.transform.childCount; i++)
		{
			if (Functions.TransformIsVisible(sideCharacters.transform.GetChild(i).transform))
			{
				controllerList.Add(sideCharacters.transform.GetChild(i).transform);
			}
		}
		foreach (Transform track in trackList)
		{
			controllerList.Add(track);
		}
		if (instantTravelNodesTransform != null)
		{
			controllerList.Add(mapNode[CurrentNode()].transform);
			for (int j = 0; j < instantTravelNodesTransform.Count; j++)
			{
				if (instantTravelNodesTransform[j] != mapNode[CurrentNode()].transform)
				{
					controllerList.Add(instantTravelNodesTransform[j]);
				}
			}
		}
		AudioManager.Instance.DoBSO("Map");
	}

	private IEnumerator DoCorruptionReward()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("Corruption Accepted->" + AtOManager.Instance.corruptionAccepted, "map");
		}
		if (AtOManager.Instance.corruptionAccepted)
		{
			if (GameManager.Instance.IsMultiplayer())
			{
				yield return Globals.Instance.WaitForSeconds(0.1f);
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("**************************");
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("WaitingSyncro DoCorruptionReward", "net");
				}
				if (NetworkManager.Instance.IsMaster())
				{
					while (!NetworkManager.Instance.AllPlayersReady("DoCorruptionReward"))
					{
						yield return Globals.Instance.WaitForSeconds(0.01f);
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("Game ready, Everybody checked DoCorruptionReward", "net");
					}
					NetworkManager.Instance.PlayersNetworkContinue("DoCorruptionReward");
				}
				else
				{
					NetworkManager.Instance.SetWaitingSyncro("DoCorruptionReward", status: true);
					NetworkManager.Instance.SetStatusReady("DoCorruptionReward");
					while (NetworkManager.Instance.WaitingSyncro["DoCorruptionReward"])
					{
						yield return Globals.Instance.WaitForSeconds(0.1f);
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("DoCorruptionReward, we can continue!", "net");
					}
				}
			}
			PlayerManager.Instance.CorruptionCompleted();
			string corruptionId = AtOManager.Instance.corruptionId;
			CardData cDataCorruption = Globals.Instance.GetCardData(AtOManager.Instance.corruptionIdCard);
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[CorruptionRewardId] " + corruptionId);
			}
			switch (corruptionId)
			{
			case "goldshards0":
			{
				if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
				{
					break;
				}
				Character[] team = AtOManager.Instance.GetTeam();
				Character[] array = team;
				if (cDataCorruption.CardRarity == Enums.CardRarity.Common)
				{
					for (int i = 0; i < 4; i++)
					{
						if (array[i] != null && array[i].HeroData != null)
						{
							int quantity = 80;
							quantity = AtOManager.Instance.ModifyQuantityObeliskTraits(0, quantity);
							AtOManager.Instance.GivePlayer(0, quantity, array[i].Owner);
							quantity = 80;
							quantity = AtOManager.Instance.ModifyQuantityObeliskTraits(1, quantity);
							AtOManager.Instance.GivePlayer(1, quantity, array[i].Owner);
						}
					}
					break;
				}
				for (int j = 0; j < 4; j++)
				{
					if (array[j] != null && array[j].HeroData != null)
					{
						int quantity = 130;
						quantity = AtOManager.Instance.ModifyQuantityObeliskTraits(0, quantity);
						AtOManager.Instance.GivePlayer(0, quantity, array[j].Owner);
						quantity = 130;
						quantity = AtOManager.Instance.ModifyQuantityObeliskTraits(1, quantity);
						AtOManager.Instance.GivePlayer(1, quantity, array[j].Owner);
					}
				}
				break;
			}
			case "freecardremove":
				sideCharacters.InCharacterScreen(state: true);
				yield return Globals.Instance.WaitForSeconds(0.2f);
				AtOManager.Instance.DoCardHealer(100, 1);
				break;
			case "rareshop":
			{
				sideCharacters.InCharacterScreen(state: true);
				string shopListId = "Rareshop_Tier" + AtOManager.Instance.GetTownTier();
				AtOManager.Instance.RemoveItemList(keepPets: true);
				yield return Globals.Instance.WaitForSeconds(0.2f);
				if (cDataCorruption.CardRarity == Enums.CardRarity.Common)
				{
					AtOManager.Instance.DoItemShop(shopListId);
				}
				else
				{
					AtOManager.Instance.DoItemShop(shopListId, 30);
				}
				break;
			}
			case "altarupgrade":
				sideCharacters.InCharacterScreen(state: true);
				yield return Globals.Instance.WaitForSeconds(0.2f);
				AtOManager.Instance.DoCardUpgrade(0, 0);
				break;
			case "randomcardupgrade":
			{
				if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
				{
					break;
				}
				Character[] team = AtOManager.Instance.GetTeam();
				Character[] array = team;
				for (int num = 0; num < 4; num++)
				{
					if (array[num] != null && array[num].HeroData != null)
					{
						AtOManager.Instance.UpgradeRandomCardToHero(num);
					}
				}
				if (AtOManager.Instance.upgradedCardsList != null && AtOManager.Instance.upgradedCardsList.Count > 0)
				{
					if (GameManager.Instance.IsMultiplayer())
					{
						string text = JsonHelper.ToJson(AtOManager.Instance.upgradedCardsList.ToArray());
						photonView.RPC("NET_ShareUpgradedCards", RpcTarget.Others, text);
					}
					characterWindow.ShowUpgradedCards(AtOManager.Instance.upgradedCardsList);
				}
				break;
			}
			case "heal20":
				if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
				{
					AtOManager.Instance.ModifyHeroLife(-1, 0, 30f);
					if (GameManager.Instance.IsMultiplayer())
					{
						StartCoroutine(AtOManager.Instance.ShareTeam());
					}
				}
				break;
			case "herocard":
				if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
				{
					if (!GameManager.Instance.IsMultiplayer())
					{
						AtOManager.Instance.AddCardToHero(AtOManager.Instance.corruptionRewardChar, AtOManager.Instance.corruptionRewardCard);
					}
					else
					{
						AtOManager.Instance.AddCardToHeroMP(AtOManager.Instance.corruptionRewardChar, AtOManager.Instance.corruptionRewardCard);
					}
				}
				break;
			case "goldshards1":
			{
				if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
				{
					break;
				}
				Character[] team = AtOManager.Instance.GetTeam();
				Character[] array = team;
				if (cDataCorruption.CardRarity == Enums.CardRarity.Rare)
				{
					for (int k = 0; k < 4; k++)
					{
						if (array[k] != null && array[k].HeroData != null)
						{
							int quantity = 180;
							quantity = AtOManager.Instance.ModifyQuantityObeliskTraits(0, quantity);
							AtOManager.Instance.GivePlayer(0, quantity, array[k].Owner);
							quantity = 180;
							quantity = AtOManager.Instance.ModifyQuantityObeliskTraits(1, quantity);
							AtOManager.Instance.GivePlayer(1, quantity, array[k].Owner);
						}
					}
					if (!GameManager.Instance.IsMultiplayer())
					{
						AtOManager.Instance.GivePlayer(2, 1, array[0].Owner);
						break;
					}
					for (int l = 0; l < NetworkManager.Instance.GetNumPlayers(); l++)
					{
						string playerNickPosition = NetworkManager.Instance.GetPlayerNickPosition(l);
						AtOManager.Instance.GivePlayer(2, 1, playerNickPosition);
					}
					break;
				}
				for (int m = 0; m < 4; m++)
				{
					if (array[m] != null && array[m].HeroData != null)
					{
						int quantity = 250;
						quantity = AtOManager.Instance.ModifyQuantityObeliskTraits(0, quantity);
						AtOManager.Instance.GivePlayer(0, quantity, array[m].Owner);
						quantity = 250;
						quantity = AtOManager.Instance.ModifyQuantityObeliskTraits(1, quantity);
						AtOManager.Instance.GivePlayer(1, quantity, array[m].Owner);
					}
				}
				if (!GameManager.Instance.IsMultiplayer())
				{
					AtOManager.Instance.GivePlayer(2, 2, array[0].Owner);
					break;
				}
				for (int n = 0; n < NetworkManager.Instance.GetNumPlayers(); n++)
				{
					string playerNickPosition2 = NetworkManager.Instance.GetPlayerNickPosition(n);
					AtOManager.Instance.GivePlayer(2, 2, playerNickPosition2);
				}
				break;
			}
			case "freecardupgrade":
				sideCharacters.InCharacterScreen(state: true);
				yield return Globals.Instance.WaitForSeconds(0.2f);
				AtOManager.Instance.DoCardUpgrade(100, 1);
				break;
			case "freecardremove2":
				sideCharacters.InCharacterScreen(state: true);
				yield return Globals.Instance.WaitForSeconds(0.2f);
				AtOManager.Instance.DoCardHealer(100, 2);
				break;
			case "exoticshop":
			{
				sideCharacters.InCharacterScreen(state: true);
				string shopListId = "Exoticshop_Tier" + AtOManager.Instance.GetTownTier();
				AtOManager.Instance.RemoveItemList(keepPets: true);
				yield return Globals.Instance.WaitForSeconds(0.2f);
				if (cDataCorruption.CardRarity == Enums.CardRarity.Rare)
				{
					AtOManager.Instance.DoItemShop(shopListId);
				}
				else
				{
					AtOManager.Instance.DoItemShop(shopListId, 30);
				}
				break;
			}
			}
			if (!GameManager.Instance.IsMultiplayer())
			{
				sideCharacters.Refresh();
			}
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("end corruption check in map", "map");
		}
		AtOManager.Instance.comingFromCombatDoRewards = false;
		AtOManager.Instance.ClearCorruption();
		AtOManager.Instance.SaveGame();
		GameManager.Instance.SceneLoaded();
		ShowMask(state: false);
	}

	[PunRPC]
	public void NET_ShareUpgradedCards(string upgradedCards)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("NET_ShareUpgradedCards " + upgradedCards);
		}
		List<string> list = new List<string>();
		list.AddRange(JsonHelper.FromJson<string>(upgradedCards));
		characterWindow.ShowUpgradedCards(list);
	}

	private IEnumerator WaitMultiplayer()
	{
		string key = CurrentNode();
		yield return Globals.Instance.WaitForSeconds(0.1f);
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("**************************", "net");
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("WaitingSyncro mapmanager" + key, "net");
		}
		if (NetworkManager.Instance.IsMaster())
		{
			while (!NetworkManager.Instance.AllPlayersReady("mapmanager" + key))
			{
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("Game ready, Everybody checked mapmanager" + key, "net");
			}
			NetworkManager.Instance.PlayersNetworkContinue("mapmanager" + key);
			if (AtOManager.Instance.fromEventDestinationNode != null)
			{
				string nodeId = AtOManager.Instance.fromEventDestinationNode.NodeId;
				AtOManager.Instance.fromEventDestinationNode = null;
				TravelToThisNode(GetNodeFromId(nodeId));
				yield break;
			}
			if (AtOManager.Instance.fromEventCombatData != null)
			{
				DoCombat(AtOManager.Instance.fromEventCombatData);
				yield break;
			}
			if (AtOManager.Instance.fromEventEventData != null)
			{
				DoEvent(AtOManager.Instance.fromEventEventData);
				yield break;
			}
			DrawMap();
			ShareAssignGameNodes();
		}
		else
		{
			NetworkManager.Instance.SetWaitingSyncro("mapmanager" + key, status: true);
			NetworkManager.Instance.SetStatusReady("mapmanager" + key);
			while (NetworkManager.Instance.WaitingSyncro["mapmanager" + key])
			{
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("mapmanager" + key + ", we can continue!", "net");
			}
			if (AtOManager.Instance.fromEventDestinationNode != null)
			{
				string nodeId2 = AtOManager.Instance.fromEventDestinationNode.NodeId;
				AtOManager.Instance.fromEventDestinationNode = null;
				TravelToThisNode(GetNodeFromId(nodeId2));
				ShowMask(state: false);
				yield break;
			}
		}
		BeginMapContinueEnd();
	}

	public void CleanFromEvent()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[MAPMANAGER] CleanFromEvent", "map");
		}
		AtOManager.Instance.fromEventCombatData = null;
		AtOManager.Instance.fromEventEventData = null;
	}

	private void DrawMap()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("DrawMap", "map");
		}
		ShowWorld();
		availableNodes = new List<string>();
		instantTravelNodesTransform = new List<Transform>();
		availableNodesTransform = new List<Transform>();
		GetAvailableNodes(mapNode[CurrentNode()].nodeData);
		DrawNodes();
		if (!SetPositionInCurrentNode())
		{
			return;
		}
		ShowTMPRoads();
		if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
		{
			CombatData currentCombatData = AtOManager.Instance.GetCurrentCombatData();
			if (currentCombatData != null)
			{
				if (currentCombatData.EventData != null && (currentCombatData.EventRequirementData == null || AtOManager.Instance.PlayerHasRequirement(currentCombatData.EventRequirementData)))
				{
					DoEvent(currentCombatData.EventData);
				}
				AtOManager.Instance.ClearCurrentCombatData();
			}
		}
		GameManager.Instance.SceneLoaded();
		if (AtOManager.Instance.characterUnlockData != null)
		{
			CharacterUnlock();
		}
		if (AtOManager.Instance.skinUnlockData != null)
		{
			SkinUnlock();
		}
	}

	private void ShowRequirementTracks()
	{
		foreach (Transform track in trackList)
		{
			UnityEngine.Object.Destroy(track.gameObject);
		}
		int num = 0;
		List<string> playerRequeriments = AtOManager.Instance.GetPlayerRequeriments();
		for (int i = 0; i < playerRequeriments.Count; i++)
		{
			EventRequirementData requirementData = Globals.Instance.GetRequirementData(playerRequeriments[i]);
			if (requirementData != null && requirementData.RequirementTrack && requirementData.CanShowRequeriment(AtOManager.Instance.GetMapZone(AtOManager.Instance.currentMapNode), Enums.Zone.None))
			{
				GameObject obj = UnityEngine.Object.Instantiate(eventTrackPrefab, Vector3.zero, Quaternion.identity, trackList);
				obj.transform.localPosition = new Vector3(0f, -0.55f * (float)num, 0f);
				obj.transform.GetComponent<EventTrack>().SetTrack(playerRequeriments[i]);
				num++;
			}
		}
	}

	public void Resize()
	{
		sideCharacters.Resize();
		mapLegend.Resize();
		characterWindow.Resize();
		trackList.transform.localPosition = new Vector3((0f - Globals.Instance.sizeW) * 0.5f + 2f * Globals.Instance.multiplierX, (0f - Globals.Instance.sizeH) * 0.5f + 3.75f * Globals.Instance.multiplierY, trackList.transform.localPosition.z);
	}

	public void ShowCharacterWindow(string type = "", int heroIndex = -1)
	{
		HidePopup();
		characterWindow.Show(type, heroIndex);
	}

	public void ShowDeck(int auxInt)
	{
		characterWindow.Show("deck", auxInt);
	}

	public void HideCharacterWindow()
	{
	}

	private void GetRoads()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("GetRoads", "map");
		}
		roads = new Dictionary<string, Transform>();
		foreach (Transform item in worldTransform)
		{
			if (item.gameObject.name != AtOManager.Instance.GetTownZoneId())
			{
				continue;
			}
			foreach (Transform item2 in item)
			{
				if (!(item2.gameObject.name == "Roads"))
				{
					continue;
				}
				foreach (Transform item3 in item2)
				{
					string key = item3.gameObject.name.ToLower().Trim();
					roads.Add(key, item3);
				}
			}
		}
	}

	public Dictionary<string, Node> GetMapNodeDict()
	{
		return mapNode;
	}

	private void GetMapNodes(bool useDelay = false)
	{
		StartCoroutine(GetMapNodesCo(useDelay));
	}

	private IEnumerator GetMapNodesCo(bool useDelay)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("** GetMapNodesCo **", "trace");
		}
		gettingMapNodes = true;
		string text = CurrentNode();
		if (text == "")
		{
			yield break;
		}
		IncludeMapPrefab(text);
		mapNode = new Dictionary<string, Node>();
		mapNodes = new List<string>();
		zoneGOs = new Dictionary<string, GameObject>();
		foreach (Transform item2 in worldTransform)
		{
			GameObject gameObject = item2.gameObject;
			if (!zoneGOs.ContainsKey(gameObject.name))
			{
				zoneGOs.Add(gameObject.name, gameObject);
			}
			gameObject.SetActive(value: false);
			string text2 = gameObject.name.ToLower();
			if (!GameManager.Instance.IsObeliskChallenge())
			{
				switch (text2)
				{
				case "challengecommonmap":
				case "challengehighmap":
				case "challengefinalmap":
					continue;
				}
				if (text2.Substring(0, 2) == "ol" || text2.Substring(0, 2) == "oh" || text2.Substring(0, 2) == "of")
				{
					continue;
				}
			}
			foreach (Transform item3 in item2)
			{
				if (item3.gameObject.name == "Nodes")
				{
					item3.transform.localPosition = new Vector3(item3.transform.localPosition.x, item3.transform.localPosition.y, -2f);
					foreach (Transform item4 in item3)
					{
						Node component = item4.GetComponent<Node>();
						component.InitNode();
						string item = component.nodeData.NodeId.ToLower();
						mapNodes.Add(item);
						if (AtOManager.Instance.mapVisitedNodes.Contains(item))
						{
							component.SetVisited();
						}
						mapNode.Add(item4.gameObject.name.ToLower(), component);
					}
					if (useDelay)
					{
						yield return Globals.Instance.WaitForSeconds(0.001f);
					}
				}
				if (useDelay)
				{
					yield return Globals.Instance.WaitForSeconds(0.001f);
				}
			}
			if (useDelay)
			{
				yield return Globals.Instance.WaitForSeconds(0.001f);
			}
		}
		gettingMapNodes = false;
	}

	public void IncludeObeliskBgs()
	{
		for (int i = 0; i < mapList.Count; i++)
		{
			if (mapList[i].name == "ChallengeCommonMap")
			{
				UnityEngine.Object.Instantiate(mapList[i], new Vector3(0f, 0f, 0f), Quaternion.identity, worldTransform).name = "ChallengeCommonMap";
			}
			else if (mapList[i].name == "ChallengeHighMap")
			{
				UnityEngine.Object.Instantiate(mapList[i], new Vector3(0f, 0f, 0f), Quaternion.identity, worldTransform).name = "ChallengeHighMap";
			}
			else if (mapList[i].name == "ChallengeFinalMap")
			{
				UnityEngine.Object.Instantiate(mapList[i], new Vector3(0f, 0f, 0f), Quaternion.identity, worldTransform).name = "ChallengeFinalMap";
			}
		}
	}

	public bool IncludeMapPrefab(string nodeId)
	{
		if (nodeId == "")
		{
			return false;
		}
		string zoneId = Globals.Instance.GetNodeData(nodeId).NodeZone.ZoneId;
		bool result = false;
		for (int i = 0; i < mapList.Count; i++)
		{
			if (!(mapList[i].name.ToLower() == zoneId.ToLower()))
			{
				continue;
			}
			bool flag = false;
			for (int j = 0; j < worldTransform.childCount; j++)
			{
				if (worldTransform.GetChild(j).gameObject.name == mapList[i].name)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				UnityEngine.Object.Instantiate(mapList[i], new Vector3(0f, 0f, 0f), Quaternion.identity, worldTransform).name = mapList[i].name;
				result = true;
			}
		}
		return result;
	}

	public Node GetNodeFromId(string nodeId)
	{
		if (IncludeMapPrefab(nodeId))
		{
			GetMapNodes();
		}
		if (mapNode.ContainsKey(nodeId))
		{
			return mapNode[nodeId];
		}
		return null;
	}

	private void DrawNodes()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("Draw Nodes", "map");
		}
		if (GameManager.Instance.IsObeliskChallenge())
		{
			AtOManager.Instance.GenerateObeliskMap();
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("+++ Draw Nodes +++ (" + mapNodes.Count + ")", "map");
		}
		for (int i = 0; i < mapNodes.Count; i++)
		{
			Node node = mapNode[mapNodes[i]];
			Functions.DebugLogGD("->" + node.nodeData.NodeId + " " + AtOManager.Instance.gameNodeAssigned.ContainsKey(node.nodeData.NodeId), "map");
			if (AtOManager.Instance.currentMapNode == "tutorial_0" || AtOManager.Instance.currentMapNode == "tutorial_1" || AtOManager.Instance.currentMapNode == "tutorial_2")
			{
				if (node.nodeData.NodeId != "tutorial_0" && node.nodeData.NodeId != "tutorial_1" && node.nodeData.NodeId != "tutorial_2" && node.nodeData.NodeId != "sen_41")
				{
					node.gameObject.SetActive(value: false);
					continue;
				}
			}
			else if (node.nodeData.NodeId == "tutorial_0" || node.nodeData.NodeId == "tutorial_1" || node.nodeData.NodeId == "tutorial_2")
			{
				node.gameObject.SetActive(value: false);
				continue;
			}
			if (!AtOManager.Instance.gameNodeAssigned.ContainsKey(node.nodeData.NodeId) || (node.nodeData.NodeRequirement != null && !AtOManager.Instance.PlayerHasRequirement(node.nodeData.NodeRequirement) && !node.nodeData.VisibleIfNotRequirement))
			{
				Functions.DebugLogGD("Set hidden -> " + node.nodeData.NodeId, "map");
				node.SetHidden();
				continue;
			}
			if (!availableNodes.Contains(mapNodes[i]) && mapNodes[i] != CurrentNode())
			{
				node.SetBlocked();
			}
			else
			{
				node.SetPlain();
			}
			if (!node.gameObject.activeSelf)
			{
				node.gameObject.SetActive(value: true);
			}
			availableNodesTransform.Add(node.transform);
		}
		if (GameManager.Instance.IsObeliskChallenge())
		{
			foreach (Transform item in worldTransform)
			{
				if (!item.gameObject.activeSelf)
				{
					continue;
				}
				int num = 0;
				int num2 = 0;
				foreach (Transform item2 in item)
				{
					if (!(item2.gameObject.name == "Nodes"))
					{
						continue;
					}
					foreach (Transform item3 in item2)
					{
						num++;
						if (!item3.gameObject.activeSelf)
						{
							num2++;
							continue;
						}
						break;
					}
				}
				if (num > 0 && num == num2)
				{
					Debug.LogError("No nodes to show, reassign nodes");
					AtOManager.Instance.gameNodeAssigned.Clear();
					Start();
					return;
				}
			}
		}
		for (int j = 0; j < AtOManager.Instance.mapVisitedNodes.Count; j++)
		{
			if (AtOManager.Instance.mapVisitedNodes[j] != "" && mapNode.ContainsKey(AtOManager.Instance.mapVisitedNodes[j]))
			{
				mapNode[AtOManager.Instance.mapVisitedNodes[j]].SetVisited();
			}
		}
	}

	private void AssignGameNodes()
	{
		if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
		{
			return;
		}
		bool flag = true;
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			flag = true;
		}
		else if (AtOManager.Instance.gameNodeAssigned.Count > 0)
		{
			flag = false;
		}
		string gameId = AtOManager.Instance.GetGameId();
		UnityEngine.Random.InitState(gameId.GetDeterministicHashCode());
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("AssignGameNodes -> " + gameId, "map");
		}
		for (int i = 0; i < mapNode.Count; i++)
		{
			Node value = mapNode.ElementAt(i).Value;
			if (value == null)
			{
				Debug.LogError("Node not found at element " + i);
				continue;
			}
			if (AtOManager.Instance.gameNodeAssigned.ContainsKey(value.nodeData.NodeId) && AtOManager.Instance.mapVisitedNodes.Contains(value.nodeData.NodeId))
			{
				value.AssignBackground();
				continue;
			}
			if (flag)
			{
				AtOManager.Instance.AssignSingleGameNode(value);
			}
			value.AssignNode();
		}
		if (GameManager.Instance.IsObeliskChallenge())
		{
			AtOManager.Instance.SetObeliskBosses();
		}
	}

	private void ShareAssignGameNodes()
	{
		AssignGameNodesNode();
		string[] array = new string[AtOManager.Instance.gameNodeAssigned.Count];
		AtOManager.Instance.gameNodeAssigned.Keys.CopyTo(array, 0);
		string[] array2 = new string[AtOManager.Instance.gameNodeAssigned.Count];
		AtOManager.Instance.gameNodeAssigned.Values.CopyTo(array2, 0);
		string value = JsonHelper.ToJson(array);
		string value2 = JsonHelper.ToJson(array2);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(value);
		stringBuilder.Append("|");
		stringBuilder.Append(value2);
		photonView.RPC("NET_AssignGameNodes", RpcTarget.Others, Functions.CompressString(stringBuilder.ToString()));
	}

	[PunRPC]
	private void NET_AssignGameNodes(string _compressed)
	{
		string[] array = Functions.DecompressString(_compressed).Split('|');
		string[] array2 = JsonHelper.FromJson<string>(array[0]);
		string[] array3 = JsonHelper.FromJson<string>(array[1]);
		AtOManager.Instance.gameNodeAssigned = new Dictionary<string, string>();
		for (int i = 0; i < array2.Length; i++)
		{
			AtOManager.Instance.gameNodeAssigned.Add(array2[i], array3[i]);
		}
		DrawMap();
		AssignGameNodesNode();
		ShowMask(state: false);
	}

	private void AssignGameNodesNode()
	{
		for (int i = 0; i < mapNode.Count; i++)
		{
			Node value = mapNode.ElementAt(i).Value;
			if (availableNodes == null)
			{
				availableNodes = new List<string>();
			}
			value.AssignNode();
		}
	}

	private void GetAvailableNodes(NodeData _nodeData)
	{
		for (int i = 0; i < _nodeData.NodesConnected.Length; i++)
		{
			if (_nodeData.NodesConnected[i].NodeId != _nodeData.NodeId && CanTravelToThisNode(mapNode[_nodeData.NodesConnected[i].NodeId], mapNode[_nodeData.NodeId]))
			{
				availableNodes.Add(_nodeData.NodesConnected[i].NodeId);
				GetAvailableNodes(_nodeData.NodesConnected[i]);
			}
		}
	}

	private string CurrentNode()
	{
		return AtOManager.Instance.currentMapNode;
	}

	private bool SetPositionInCurrentNode()
	{
		string text = CurrentNode();
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("SetPositionInCurrentNode " + text, "map");
		}
		Node node = (nodeActive = mapNode[text]);
		node.SetActive();
		zoneActive = node.nodeData.NodeZone;
		if (GameManager.Instance.IsObeliskChallenge())
		{
			DrawAllArrows();
		}
		DrawArrowActive(iconHover: true);
		if (text == "aqua_16")
		{
			StartCoroutine(ChangeZoneBecauseStuck(text));
		}
		else if (text == "spider_8" && AtOManager.Instance.PlayerHasRequirement(Globals.Instance.GetRequirementData("spidercaveout")) && !NodeExists(Globals.Instance.GetNodeData("spider_9")))
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("Exit", "map");
			}
			StartCoroutine(ChangeZoneBecauseStuck(text));
		}
		else if (text == "secta_5" && AtOManager.Instance.PlayerHasRequirement(Globals.Instance.GetRequirementData("belphyordead")) && !NodeExists(Globals.Instance.GetNodeData("secta_8")))
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("Exit", "map");
			}
			StartCoroutine(ChangeZoneBecauseStuck(text));
		}
		else
		{
			switch (text)
			{
			case "sen_34":
			case "velka_33":
			case "aqua_36":
			case "ulmin_40":
			case "faen_39":
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Exit", "map");
				}
				StartCoroutine(ChangeZoneBecauseStuck(text));
				break;
			case "voidhigh_13":
				if (AtOManager.Instance.bossesKilledName != null && AtOManager.Instance.bossesKilledName.Any((string s) => s.StartsWith("lordhanshek", StringComparison.OrdinalIgnoreCase)))
				{
					if (!AtOManager.Instance.bossesKilledName.Any((string s) => s.StartsWith("archonnihr", StringComparison.OrdinalIgnoreCase)))
					{
						AtOManager.Instance.SetCombatData(Globals.Instance.GetCombatData("evoidhigh_13b"));
						DoCombat(AtOManager.Instance.GetCurrentCombatData());
						return false;
					}
					AtOManager.Instance.FinishGame();
					return false;
				}
				break;
			}
		}
		return true;
	}

	private IEnumerator ChangeZoneBecauseStuck(string nodeName)
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("**************************", "net");
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("WaitingSyncro changeZoneBecauseStuck" + nodeName, "net");
			}
			if (NetworkManager.Instance.IsMaster())
			{
				while (!NetworkManager.Instance.AllPlayersReady("changeZoneBecauseStuck" + nodeName))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked changeZoneBecauseStuck" + nodeName, "net");
				}
				NetworkManager.Instance.PlayersNetworkContinue("changeZoneBecauseStuck" + nodeName);
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("changeZoneBecauseStuck" + nodeName, status: true);
				NetworkManager.Instance.SetStatusReady("changeZoneBecauseStuck" + nodeName);
				while (NetworkManager.Instance.WaitingSyncro["changeZoneBecauseStuck" + nodeName])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("changeZoneBecauseStuck" + nodeName + ", we can continue!", "net");
				}
			}
		}
		if (Globals.Instance.ShowDebug)
		{
			Debug.Log("nodeName");
		}
		switch (nodeName)
		{
		case "spider_8":
			TravelToThisNode(GetNodeFromId("aqua_32"));
			yield break;
		case "secta_5":
			TravelToThisNode(GetNodeFromId("sen_12"));
			yield break;
		case "aqua_16":
			TravelToThisNode(GetNodeFromId("spider_1"));
			yield break;
		}
		if (AtOManager.Instance.mapVisitedNodes.Contains(nodeName))
		{
			AtOManager.Instance.mapVisitedNodes.Remove(nodeName);
		}
		TravelToThisNode(GetNodeFromId(nodeName));
	}

	private void DrawArrowActive(bool iconHover = false)
	{
		if (nodeActive == null)
		{
			return;
		}
		NodeData nodeData = nodeActive.nodeData;
		for (int i = 0; i < nodeData.NodesConnected.Length; i++)
		{
			Node node = mapNode[nodeData.NodesConnected[i].NodeId];
			if (CanTravelToThisNode(node))
			{
				DrawArrow(nodeActive, node);
				if (iconHover)
				{
					node.SetAvailable();
				}
				instantTravelNodesTransform.Add(node.transform);
			}
		}
		instantTravelNodesTransform.Add(mapNode[CurrentNode()].transform);
	}

	private void DrawAllArrows()
	{
		foreach (Transform tmpRoad in tmpRoads)
		{
			UnityEngine.Object.Destroy(tmpRoad.gameObject);
		}
		foreach (KeyValuePair<string, Node> item in mapNode)
		{
			Node value = item.Value;
			if (value.availableT.gameObject.activeSelf || value.plainT.gameObject.activeSelf)
			{
				DrawArrowsTempObelisk(value);
			}
		}
	}

	public void DrawArrow(Node _nodeSource, Node _nodeDestination, bool highlight = false, bool temp = false, bool challenge = false)
	{
		string text = _nodeSource.nodeData.NodeId + "-" + _nodeDestination.nodeData.NodeId;
		if (roads.ContainsKey(text))
		{
			Transform transform = null;
			transform = ((!challenge) ? roads[text] : UnityEngine.Object.Instantiate(roads[text], tmpRoads).transform);
			transform.gameObject.SetActive(value: true);
			LineRenderer component = transform.GetComponent<LineRenderer>();
			if (challenge)
			{
				component.materials = new Material[1];
				component.material = materialTmpRoad;
				component.widthMultiplier = 0.06f;
				component.textureMode = LineTextureMode.Static;
				component.textureScale = vectorLineTexture;
				Color startColor = (component.endColor = new Color(1f, 1f, 1f, 0.65f));
				component.startColor = startColor;
			}
			else if (highlight)
			{
				component.startColor = Globals.Instance.MapArrow;
				component.endColor = Globals.Instance.MapArrowHighlight;
			}
			else if (temp)
			{
				if (GameManager.Instance.IsObeliskChallenge())
				{
					component.startColor = Globals.Instance.MapArrowTempChallenge;
					component.endColor = Globals.Instance.MapArrowTempChallenge;
				}
				else
				{
					component.startColor = Globals.Instance.MapArrowTemp;
					component.endColor = Globals.Instance.MapArrowTemp;
				}
				roadTemp.Add(text);
			}
			else
			{
				component.startColor = Globals.Instance.MapArrow;
				component.endColor = Globals.Instance.MapArrow;
			}
		}
		roadActive = text;
	}

	public void DrawArrowsTemp(Node _nodeSource)
	{
		foreach (KeyValuePair<string, Node> item in mapNode)
		{
			Node value = item.Value;
			for (int i = 0; i < value.nodeData.NodesConnected.Length; i++)
			{
				if (value.nodeData.NodesConnected[i].NodeId == _nodeSource.nodeData.NodeId && mapNode[value.nodeData.NodeId].gameObject.activeSelf && CanTravelToThisNode(_nodeSource, value))
				{
					DrawArrow(value, _nodeSource, highlight: false, temp: true);
				}
			}
		}
		for (int j = 0; j < _nodeSource.nodeData.NodesConnected.Length; j++)
		{
			Node node = mapNode[_nodeSource.nodeData.NodesConnected[j].NodeId];
			if (CanTravelToThisNode(node, _nodeSource))
			{
				DrawArrow(_nodeSource, node, highlight: false, temp: true);
			}
		}
	}

	public void DrawArrowsTempObelisk(Node _nodeSource)
	{
		foreach (KeyValuePair<string, Node> item in mapNode)
		{
			Node value = item.Value;
			for (int i = 0; i < value.nodeData.NodesConnected.Length; i++)
			{
				if (value.nodeData.NodesConnected[i].NodeId == _nodeSource.nodeData.NodeId && mapNode[value.nodeData.NodeId].gameObject.activeSelf && !(AtOManager.Instance.currentMapNode == value.nodeData.NodeId) && !value.blockedT.gameObject.activeSelf && CanTravelToThisNode(_nodeSource, value))
				{
					DrawArrow(value, _nodeSource, highlight: false, temp: false, challenge: true);
				}
			}
		}
	}

	public void HideArrowsTemp(Node _nodeSource)
	{
		for (int i = 0; i < roadTemp.Count; i++)
		{
			roads[roadTemp[i]].gameObject.SetActive(value: false);
		}
		roadTemp.Clear();
		DrawArrowActive();
	}

	private void HideAllArrows()
	{
		for (int i = 0; i < roads.Count; i++)
		{
			roads.ElementAt(i).Value.gameObject.SetActive(value: false);
		}
	}

	public void HideArrow()
	{
		if (roads.ContainsKey(roadActive))
		{
			roads[roadActive].gameObject.SetActive(value: false);
		}
		roadActive = "";
	}

	public bool NodeExists(NodeData _nodeData)
	{
		if (_nodeData == null || !AtOManager.Instance.gameNodeAssigned.ContainsKey(_nodeData.NodeId))
		{
			return false;
		}
		return true;
	}

	public bool NodeVisible(NodeData _nodeData)
	{
		if (_nodeData.NodeRequirement != null && !AtOManager.Instance.PlayerHasRequirement(_nodeData.NodeRequirement))
		{
			return false;
		}
		return true;
	}

	public bool CanTravelToThisNode(Node _node, Node _nodeSource = null)
	{
		if (_nodeSource == null)
		{
			_nodeSource = nodeActive;
		}
		if (_nodeSource == null)
		{
			return false;
		}
		string key = _nodeSource.nodeData.NodeId + "_" + _node.nodeData.NodeId;
		if (canTravelDict.ContainsKey(key))
		{
			return canTravelDict[key];
		}
		if (!AtOManager.Instance.gameNodeAssigned.ContainsKey(_node.nodeData.NodeId) || !AtOManager.Instance.gameNodeAssigned.ContainsKey(_nodeSource.nodeData.NodeId) || (_node.nodeData.NodeRequirement != null && !AtOManager.Instance.PlayerHasRequirement(_node.nodeData.NodeRequirement)))
		{
			canTravelDict.Add(key, value: false);
			return false;
		}
		NodeData[] nodesConnected = _nodeSource.nodeData.NodesConnected;
		for (int i = 0; i < nodesConnected.Length; i++)
		{
			if (!(nodesConnected[i].NodeId == _node.nodeData.NodeId))
			{
				continue;
			}
			if (_nodeSource.nodeData.NodesConnectedRequirement != null)
			{
				for (int j = 0; j < _nodeSource.nodeData.NodesConnectedRequirement.Length; j++)
				{
					if (_nodeSource.nodeData.NodesConnectedRequirement[j].NodeData.NodeId == _node.nodeData.NodeId && ((_nodeSource.nodeData.NodesConnectedRequirement[j].ConectionRequeriment != null && !AtOManager.Instance.PlayerHasRequirement(_nodeSource.nodeData.NodesConnectedRequirement[j].ConectionRequeriment)) || (_nodeSource.nodeData.NodesConnectedRequirement[j].ConectionIfNotNode != null && NodeExists(_nodeSource.nodeData.NodesConnectedRequirement[j].ConectionIfNotNode) && NodeVisible(_nodeSource.nodeData.NodesConnectedRequirement[j].ConectionIfNotNode))))
					{
						canTravelDict.Add(key, value: false);
						return false;
					}
				}
			}
			canTravelDict.Add(key, value: true);
			return true;
		}
		canTravelDict.Add(key, value: false);
		return false;
	}

	public void ChangeZone(string nodeDest)
	{
		ZoneData nodeZone = GetNodeFromId(nodeDest).nodeData.NodeZone;
		if (!(nodeZone == null) && !(zoneActive == null))
		{
			if (nodeZone.ZoneId != zoneActive.ZoneId)
			{
				zoneGOs[zoneActive.ZoneId].SetActive(value: false);
				AtOManager.Instance.TravelBetweenZones(zoneActive, nodeZone);
			}
			else
			{
				BeginMap();
			}
		}
	}

	public void PlayerSelectedNode(Node _node, bool _fromFollowTheLeader = false)
	{
		if ((GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster() && AtOManager.Instance.followingTheLeader && !_fromFollowTheLeader) || selectedNode)
		{
			return;
		}
		selectedNode = true;
		if (!GameManager.Instance.IsMultiplayer())
		{
			TravelToThisNode(_node);
			return;
		}
		if (followingCo != null)
		{
			StopCoroutine(followingCo);
		}
		followingCo = StartCoroutine(FollowCoroutine(NetworkManager.Instance.GetPlayerNick(), _node.nodeData.NodeId));
	}

	private IEnumerator FollowCoroutine(string _nick, string _nodeId)
	{
		yield return Globals.Instance.WaitForSeconds(0.1f);
		photonView.RPC("NET_PlayerSelectedNode", RpcTarget.MasterClient, _nick, _nodeId);
	}

	[PunRPC]
	private void NET_PlayerSelectedNode(string _nick, string _nodeId)
	{
		if (playerSelectedNodesDict.ContainsKey(_nick))
		{
			return;
		}
		playerSelectedNodesDict.Add(_nick, _nodeId);
		string[] array = new string[playerSelectedNodesDict.Count];
		playerSelectedNodesDict.Keys.CopyTo(array, 0);
		string[] array2 = new string[playerSelectedNodesDict.Count];
		playerSelectedNodesDict.Values.CopyTo(array2, 0);
		string text = JsonHelper.ToJson(array);
		string text2 = JsonHelper.ToJson(array2);
		photonView.RPC("NET_SharePlayerSelectedNode", RpcTarget.All, text, text2);
		if (playerSelectedNodesDict.Count != NetworkManager.Instance.GetNumPlayers())
		{
			return;
		}
		bool flag = true;
		string text3 = "";
		foreach (KeyValuePair<string, string> item in playerSelectedNodesDict)
		{
			if (text3 == "")
			{
				text3 = item.Value;
			}
			else if (item.Value != text3)
			{
				flag = false;
				break;
			}
		}
		if (!flag)
		{
			photonView.RPC("NET_DoConflict", RpcTarget.All);
		}
		else
		{
			StartCoroutine(TravelToThisNodeCo(text3));
		}
	}

	[PunRPC]
	private void NET_SharePlayerSelectedNode(string _keys, string _values)
	{
		playerSelectedNodesDict.Clear();
		string[] array = JsonHelper.FromJson<string>(_keys);
		string[] array2 = JsonHelper.FromJson<string>(_values);
		ClearMultiplayerSelectedNodes();
		for (int i = 0; i < array.Length; i++)
		{
			playerSelectedNodesDict.Add(array[i], array2[i]);
			GetNodeFromId(array2[i]).ShowSelectedNode(array[i]);
			if (!selectedNode && GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster() && AtOManager.Instance.followingTheLeader && NetworkManager.Instance.PlayerIsMaster(array[i]))
			{
				PlayerSelectedNode(GetNodeFromId(array2[i]), _fromFollowTheLeader: true);
				return;
			}
		}
		GameManager.Instance.PlayLibraryAudio("ui_mapnodeselection");
	}

	private void ClearMultiplayerSelectedNodes()
	{
		for (int i = 0; i < mapNodes.Count; i++)
		{
			mapNode[mapNodes[i]].ClearSelectedNode();
		}
	}

	private IEnumerator TravelToThisNodeCo(string _nodeId)
	{
		if (followingCo != null)
		{
			StopCoroutine(followingCo);
		}
		yield return Globals.Instance.WaitForSeconds(0.5f);
		TravelToThisNode(GetNodeFromId(_nodeId));
	}

	public void TravelToThisNode(Node _node)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("TravelToThisNode " + _node, "map");
		}
		StartCoroutine(TravelToThisNodeCorruption(_node));
	}

	private IEnumerator TravelToThisNodeCorruption(Node _node)
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			playerSelectedNodesDict.Clear();
		}
		string nodeId = _node.nodeData.NodeId;
		bool flag = false;
		if (AtOManager.Instance.GetGameId() != "tuto".ToUpper() && nodeId != "tutorial_1" && nodeId != "sen_1" && nodeId != "sen_2" && nodeId != "sen_3" && nodeId != "aqua_27")
		{
			flag = true;
		}
		if (_node.nodeData.DisableCorruption)
		{
			flag = false;
		}
		if (flag && _node.action == "combat")
		{
			CombatData combatData = Globals.Instance.GetCombatData(_node.actionId);
			if (combatData.EventData == null || (combatData.EventRequirementData != null && !AtOManager.Instance.PlayerHasRequirement(combatData.EventRequirementData)))
			{
				corruptionSetted = false;
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("corruptionType->" + AtOManager.Instance.corruptionType, "map");
				}
				corruption.InitCorruption(_node);
				while (!corruptionSetted)
				{
					yield return Globals.Instance.WaitForSeconds(0.1f);
				}
			}
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("Travel To This Node " + nodeId, "map");
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD(AtOManager.Instance.currentMapNode + "!=" + nodeId, "map");
		}
		_node.SetActive();
		string nodeObeliskIcon = "";
		if (GameManager.Instance.IsObeliskChallenge())
		{
			foreach (Transform item in _node.transform)
			{
				if (item.gameObject.name == "nodeIcon" && item.GetComponent<SpriteRenderer>() != null && item.GetComponent<SpriteRenderer>().sprite != null)
				{
					nodeObeliskIcon = item.GetComponent<SpriteRenderer>().sprite.name;
				}
			}
		}
		bool executeActionInNode = AtOManager.Instance.SetCurrentNode(nodeId, _node.actionId, nodeObeliskIcon);
		if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("**************************", "net");
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("WaitingSyncro waitingSetCurrentNode", "net");
			}
			while (!NetworkManager.Instance.AllPlayersReady("waitingSetCurrentNode"))
			{
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("Game ready, Everybody checked", "net");
			}
			NetworkManager.Instance.PlayersNetworkContinue("waitingSetCurrentNode");
			yield return Globals.Instance.WaitForSeconds(0.1f);
		}
		if (_node.nodeData.GoToTown)
		{
			AtOManager.Instance.GoToTown(nodeId);
			yield break;
		}
		if (executeActionInNode)
		{
			if (_node.action == "combat")
			{
				DoCombat(Globals.Instance.GetCombatData(_node.actionId));
				yield break;
			}
			if (_node.action == "event")
			{
				DoEvent(Globals.Instance.GetEventData(_node.actionId));
				yield break;
			}
		}
		ChangeZone(nodeId);
	}

	private void DoCombat(CombatData _combatData)
	{
		if (!GameManager.Instance.IsMultiplayer() || (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster()))
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[MAPMANAGER] DoCombat");
			}
			HidePopup();
			AtOManager.Instance.LaunchCombat(_combatData);
		}
	}

	public void ShowPopup(Node _node)
	{
		if (!(CardCraftManager.Instance != null) && !(EventManager.Instance != null))
		{
			if (showPopupCo != null)
			{
				StopCoroutine(showPopupCo);
			}
			if (!IsMaskActive())
			{
				showPopupCo = StartCoroutine(ShowPopupCo(_node));
			}
		}
	}

	private IEnumerator ShowPopupCo(Node _node)
	{
		yield return Globals.Instance.WaitForSeconds(0.05f);
		popupNode.Show(_node);
	}

	public void HidePopup()
	{
		if (showPopupCo != null)
		{
			StopCoroutine(showPopupCo);
		}
		popupNode.Hide();
	}

	private void HideWorld()
	{
		worldTransform.gameObject.SetActive(value: false);
	}

	private void ShowWorld()
	{
		worldTransform.gameObject.SetActive(value: true);
	}

	public void ShowHideEvent()
	{
		if (!worldTransform.gameObject.activeSelf)
		{
			worldTransform.gameObject.SetActive(value: true);
			eventShowHideButton.GetComponent<BotonGeneric>().SetText(Texts.Instance.GetText("show"));
			eventGO.transform.localPosition = new Vector3(eventGO.transform.localPosition.x, eventGO.transform.localPosition.y, -100f);
		}
		else
		{
			worldTransform.gameObject.SetActive(value: false);
			eventShowHideButton.GetComponent<BotonGeneric>().SetText(Texts.Instance.GetText("hide"));
			eventGO.transform.localPosition = new Vector3(eventGO.transform.localPosition.x, eventGO.transform.localPosition.y, -1f);
		}
	}

	private void ShowTMPRoads()
	{
		tmpRoads.gameObject.SetActive(value: true);
	}

	private void HideTMPRoads()
	{
		tmpRoads.gameObject.SetActive(value: false);
	}

	public void HighlightNode(string _nodeDestinationId, bool _status)
	{
		mapNode[_nodeDestinationId].HighlightNode(_status);
	}

	public void HighlightNodeMP(Node _nodeDestination, bool _status)
	{
		photonView.RPC("NET_HightlightNode", RpcTarget.Others, _nodeDestination.nodeData.NodeId, _status);
	}

	[PunRPC]
	private void NET_HightlightNode(string _nodeDestination, bool _status)
	{
		mapNode[_nodeDestination].HighlightNode(_status);
	}

	private void DoEvent(EventData _eventData)
	{
		if (!GameManager.Instance.IsMultiplayer() || (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster()))
		{
			if (GameManager.Instance.IsMultiplayer())
			{
				photonView.RPC("NET_DoEvent", RpcTarget.Others, _eventData.EventId);
			}
			LaunchEvent(_eventData);
			CleanFromEvent();
		}
	}

	[PunRPC]
	public void NET_DoEvent(string eventId)
	{
		if (followingCo != null)
		{
			Debug.Log("STOP FollowCoruitne");
			StopCoroutine(followingCo);
		}
		EventData eventData = Globals.Instance.GetEventData(eventId);
		LaunchEvent(eventData);
	}

	private void LaunchEvent(EventData _eventData)
	{
		GameManager.Instance.SceneLoaded();
		ShowMask(state: false);
		if (eventGO != null)
		{
			UnityEngine.Object.Destroy(eventGO);
		}
		StartCoroutine(DoEventCo(_eventData));
	}

	private IEnumerator DoEventCo(EventData _eventData)
	{
		yield return Globals.Instance.WaitForSeconds(0.02f);
		eventGO = UnityEngine.Object.Instantiate(eventPrefab, new Vector3(0f, 0f, -1f), Quaternion.identity);
		eventGO.transform.localScale = new Vector3(1f, 1f, 1f);
		eventManager = eventGO.GetComponent<EventManager>();
		eventManager.SetEvent(_eventData);
		HidePopup();
		HideWorld();
		HideTMPRoads();
		eventShowHideButton.gameObject.SetActive(value: true);
		eventShowHideButton.GetComponent<BotonGeneric>().SetText(Texts.Instance.GetText("hide"));
		yield return null;
	}

	public void EventReady()
	{
		EventManager.Instance.Ready();
	}

	[PunRPC]
	private void NET_CloseEvent()
	{
		EventManager.Instance.CloseEvent();
	}

	public void CloseEventFromEvent(NodeData _destinationNode = null, CombatData _combatData = null, EventData _eventData = null, bool _upgrade = false, int _discount = 0, int _maxQuantity = -1, bool _healer = false, bool _craft = false, string _shopListId = "", string _lootListId = "", Enums.CardRarity _maxCraftRarity = Enums.CardRarity.Common, bool _cardPlayerGame = false, CardPlayerPackData _cardPlayerGamePack = null, bool _cardPlayerPairsGame = false, CardPlayerPairsPackData _cardPlayerPairsGamePack = null, bool _corruption = false, bool _itemCorruption = false)
	{
		StartCoroutine(CloseEventFromEventCo(_destinationNode, _combatData, _eventData, _upgrade, _discount, _maxQuantity, _healer, _craft, _shopListId, _lootListId, _maxCraftRarity, _cardPlayerGame, _cardPlayerGamePack, _cardPlayerPairsGame, _cardPlayerPairsGamePack, _corruption, _itemCorruption));
	}

	private IEnumerator CloseEventFromEventCo(NodeData _destinationNode = null, CombatData _combatData = null, EventData _eventData = null, bool _upgrade = false, int _discount = 0, int _maxQuantity = -1, bool _healer = false, bool _craft = false, string _shopListId = "", string _lootListId = "", Enums.CardRarity _maxCraftRarity = Enums.CardRarity.Common, bool _cardPlayerGame = false, CardPlayerPackData _cardPlayerGamePack = null, bool _cardPlayerPairsGame = false, CardPlayerPairsPackData _cardPlayerPairsGamePack = null, bool _corruption = false, bool _itemCorruption = false)
	{
		UnityEngine.Object.Destroy(eventGO);
		eventGO = null;
		eventManager = null;
		eventShowHideButton.gameObject.SetActive(value: false);
		if (GameManager.Instance.IsMultiplayer())
		{
			string key = CurrentNode();
			ShowMask(state: true);
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("**************************");
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("WaitingSyncro closevent" + key, "net");
			}
			yield return Globals.Instance.WaitForSeconds(0.1f);
			if (NetworkManager.Instance.IsMaster())
			{
				while (!NetworkManager.Instance.AllPlayersReady("closevent" + key))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked closevent" + key, "net");
				}
				NetworkManager.Instance.PlayersNetworkContinue("closevent" + key);
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("closevent" + key, status: true);
				NetworkManager.Instance.SetStatusReady("closevent" + key);
				while (NetworkManager.Instance.WaitingSyncro["closevent" + key])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("closevent" + key + ", we can continue!", "net");
				}
			}
		}
		AtOManager.Instance.fromEventCombatData = _combatData;
		AtOManager.Instance.fromEventEventData = _eventData;
		if (AtOManager.Instance.GetEventRewardTier() != null)
		{
			AtOManager.Instance.LaunchRewards();
		}
		else if (_upgrade)
		{
			AtOManager.Instance.DoCardUpgrade(_discount, _maxQuantity);
		}
		else if (_healer)
		{
			AtOManager.Instance.DoCardHealer(_discount, _maxQuantity);
		}
		else if (_craft)
		{
			AtOManager.Instance.DoCardCraft(_discount, _maxQuantity, _maxCraftRarity);
		}
		else if (_corruption)
		{
			AtOManager.Instance.DoCardCorruption(_discount, _maxQuantity);
		}
		else if (_itemCorruption)
		{
			AtOManager.Instance.DoItemCorrupt(_discount, _maxQuantity);
		}
		else if (_shopListId != "")
		{
			AtOManager.Instance.DoItemShop(_shopListId, _discount);
		}
		else if (_lootListId != "")
		{
			AtOManager.Instance.fromEventDestinationNode = _destinationNode;
			StartCoroutine(DoLoot(_lootListId));
		}
		else if (_cardPlayerGame)
		{
			AtOManager.Instance.DoCardPlayerGame(_cardPlayerGamePack);
		}
		else if (_cardPlayerPairsGame)
		{
			AtOManager.Instance.DoCardPlayerPairsGame(_cardPlayerPairsGamePack);
		}
		else if (_destinationNode != null)
		{
			TravelToThisNode(GetNodeFromId(_destinationNode.NodeId));
		}
		else
		{
			BeginMap();
		}
	}

	private IEnumerator DoLoot(string _lootListId)
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			ShowMask(state: true);
			yield return Globals.Instance.WaitForSeconds(0.1f);
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("**************************");
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("WaitingSyncro mapdoloot", "net");
			}
			if (NetworkManager.Instance.IsMaster())
			{
				while (!NetworkManager.Instance.AllPlayersReady("mapdoloot"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked mapdoloot", "net");
				}
				NetworkManager.Instance.PlayersNetworkContinue("mapdoloot");
				AtOManager.Instance.DoLoot(_lootListId);
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("mapdoloot", status: true);
				NetworkManager.Instance.SetStatusReady("mapdoloot");
				while (NetworkManager.Instance.WaitingSyncro["mapdoloot"])
				{
					yield return Globals.Instance.WaitForSeconds(0.1f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("mapdoloot, we can continue!", "net");
				}
			}
		}
		else
		{
			AtOManager.Instance.DoLoot(_lootListId);
		}
	}

	[PunRPC]
	public void NET_CloseCardCraft()
	{
		CardCraftManager.Instance.ExitCardCraft();
	}

	[PunRPC]
	private void NET_Event_OptionSelected(string _playerNick, int _option)
	{
		eventManager.NET_OptionSelected(_playerNick, _option);
	}

	[PunRPC]
	public void NET_Event_SelectAnswer(int _answerId)
	{
		eventManager.NET_SelectAnswer(_answerId);
	}

	public void GenerateRandomStringBatch(int total, int _seed = -1)
	{
		randomStringArr.Clear();
		int num = (int)DateTime.Now.Ticks;
		if (_seed != -1)
		{
			num = _seed;
		}
		UnityEngine.Random.InitState(num);
		for (int i = 0; i < total; i++)
		{
			randomStringArr.Add(Functions.RandomString(6f));
		}
		if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
		{
			photonView.RPC("NET_SetRandomSeed", RpcTarget.Others, (short)total, num);
		}
	}

	private void SendRandomStringBatch()
	{
		string text = JsonHelper.ToJson(randomStringArr.ToArray());
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		Debug.Log("randomArr=>" + text);
		Debug.Log(bytes);
		Debug.Log("compress=>" + Functions.CompressString(text));
		photonView.RPC("NET_SetRandomStringBatch", RpcTarget.Others, bytes, randomIndex);
	}

	[PunRPC]
	private void NET_SetRandomStringBatch(byte[] arrByte, int index)
	{
		string json = Encoding.UTF8.GetString(arrByte);
		randomStringArr = JsonHelper.FromJson<string>(json).ToList();
		randomIndex = index;
	}

	[PunRPC]
	private void NET_SetRandomSeed(short _total, int _seed)
	{
		GenerateRandomStringBatch(_total, _seed);
	}

	public void SetRandomIndexRandom()
	{
		randomIndex = UnityEngine.Random.Range(0, randomStringArr.Count);
	}

	public string GetRandomString()
	{
		string result = randomStringArr[randomIndex];
		randomIndex++;
		if (randomIndex >= randomStringArr.Count)
		{
			randomIndex = 0;
		}
		return result;
	}

	public int GetRandomIntRange(int min, int max)
	{
		return Functions.Random(min, max, GetRandomString());
	}

	public int GetRandomIntRangeOLD(int min, int max)
	{
		if (min == max)
		{
			return min;
		}
		string randomString = GetRandomString();
		int num = 0;
		for (int i = 0; i < randomString.Length; i++)
		{
			if (randomString[i] == ' ')
			{
				num++;
			}
		}
		long[] sumArr = new long[num + 1];
		long num2 = Functions.ASCIIWordSum(randomString, sumArr);
		return min + Mathf.FloorToInt(num2 % (max - min));
	}

	public void ShowCharacterStats()
	{
		sideCharacters.Show();
		int num = 0;
		if ((bool)CardCraftManager.Instance)
		{
			num = CardCraftManager.Instance.heroIndex;
		}
		sideCharacters.ShowActiveStatus(num);
		characterWindow.Show("stats", num);
	}

	public void CorruptionBox()
	{
		if (!corruptionSetted)
		{
			corruption.BoxClicked();
		}
	}

	public void CorruptionContinue()
	{
		if (corruption.CorruptionOk())
		{
			corruptionSetted = true;
			corruption.HideButton();
		}
	}

	public void CorruptionShowHide()
	{
		corruption.ShowHide();
	}

	public bool IsCorruptionOver()
	{
		return corruption.IsActive();
	}

	public void CorruptionSelectReward(string type)
	{
		if (!corruptionSetted)
		{
			corruption.ChooseReward(type);
		}
	}

	[PunRPC]
	public void NET_ShareCorruption(string _corruptionRewardId, string _corruptionRewardIdB, string _corruptionIdCard, int _corruptionRewardChar, string _corruptionRewardCard, string _nodeSelectedAssignedId, string _nodeSelectedDataId)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("NET_ShareCorruption");
		}
		corruption.DrawCorruptionFromNet(_corruptionRewardId, _corruptionRewardIdB, _corruptionIdCard, _corruptionRewardChar, _corruptionRewardCard, _nodeSelectedAssignedId, _nodeSelectedDataId);
	}

	[PunRPC]
	public void NET_ChooseRewardCorruption(short choosed)
	{
		string choosed2 = "";
		switch (choosed)
		{
		case 1:
			choosed2 = "A";
			break;
		case 2:
			choosed2 = "B";
			break;
		}
		corruption.ChooseReward(choosed2);
	}

	[PunRPC]
	public void NET_BoxClicked(bool status)
	{
		corruption.BoxClicked(setStatus: true, status);
	}

	public void ConflictSelection(int option)
	{
		conflict.SelectOption(option);
	}

	public bool IsConflictOver()
	{
		return conflict != null;
	}

	[PunRPC]
	public void NET_DoConflict()
	{
		if (followingCo != null)
		{
			Debug.Log("STOP FollowCoruitne");
			StopCoroutine(followingCo);
		}
		StartCoroutine(InitConflictResolutionCo());
	}

	private IEnumerator InitConflictResolutionCo()
	{
		yield return Globals.Instance.WaitForSeconds(0.15f);
		InitConflictResolution();
	}

	private void InitConflictResolution()
	{
		conflictGO = UnityEngine.Object.Instantiate(conflictPrefab, new Vector3(0f, 0f, -2f), Quaternion.identity);
		conflict = conflictGO.GetComponent<ConflictManager>();
		conflict.Show();
	}

	[PunRPC]
	public void NET_ShareConflictOrder(int _playerChoosing)
	{
		StartCoroutine(NET_ShareConflictOrderCo(_playerChoosing));
	}

	private IEnumerator NET_ShareConflictOrderCo(int _playerChoosing)
	{
		while (conflict == null)
		{
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
		conflict.playerChoosing = _playerChoosing;
		StartCoroutine(conflict.NET_ShareConflictOrderCo());
	}

	[PunRPC]
	public void NET_SelectConflictOptionFromSlave(int _option)
	{
		conflict.SelectOptionFromOutside(_option);
	}

	[PunRPC]
	public void NET_SelectConflictOption(int _option)
	{
		conflict.SelectOptionFromOutside(_option);
	}

	public void ResultConflict(string _playerWin)
	{
		StartCoroutine(ResultConflictCo(_playerWin));
	}

	private IEnumerator ResultConflictCo(string _playerWin)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("ResultConflict " + _playerWin);
		}
		yield return Globals.Instance.WaitForSeconds(2.5f);
		UnityEngine.Object.Destroy(conflictGO);
		conflict = null;
		if (NetworkManager.Instance.IsMaster())
		{
			yield return Globals.Instance.WaitForSeconds(0.8f);
			ResultConflictAction(_playerWin);
		}
	}

	private void ResultConflictAction(string _playerWin)
	{
		if (EventManager.Instance != null)
		{
			int num = 0;
			if (EventManager.Instance.MultiplayerPlayerSelection != null && EventManager.Instance.MultiplayerPlayerSelection.ContainsKey(_playerWin))
			{
				num = EventManager.Instance.MultiplayerPlayerSelection[_playerWin];
			}
			photonView.RPC("NET_Event_SelectAnswer", RpcTarget.All, num);
		}
		else if (playerSelectedNodesDict.Count > 0)
		{
			TravelToThisNode(GetNodeFromId(playerSelectedNodesDict[_playerWin]));
		}
	}

	private void SkinUnlock(bool admin = false)
	{
		if (AtOManager.Instance.skinUnlockData != null)
		{
			string steamStat = AtOManager.Instance.skinUnlockData.SteamStat;
			if (!(steamStat == "") && SteamManager.Instance.GetStatInt(steamStat) != 1)
			{
				SteamManager.Instance.SetStatInt(steamStat, 1);
				unlockGO = UnityEngine.Object.Instantiate(unlockCharacterPrefab, new Vector3(0f, 0f, -7f), Quaternion.identity);
				unlockGO.GetComponent<CharacterUnlock>().ShowUnlock(AtOManager.Instance.skinUnlockData.SkinSubclass, AtOManager.Instance.skinUnlockData);
				Telemetry.SendUnlock("skin", AtOManager.Instance.skinUnlockData.SkinId);
				AtOManager.Instance.skinUnlockData = null;
			}
		}
	}

	private void CharacterUnlock()
	{
		if (AtOManager.Instance.characterUnlockData != null)
		{
			if (!PlayerManager.Instance.IsHeroUnlocked(AtOManager.Instance.characterUnlockData.Id))
			{
				unlockGO = UnityEngine.Object.Instantiate(unlockCharacterPrefab, new Vector3(0f, 0f, -7f), Quaternion.identity);
				unlockGO.GetComponent<CharacterUnlock>().ShowUnlock(AtOManager.Instance.characterUnlockData);
				PlayerManager.Instance.HeroUnlock(AtOManager.Instance.characterUnlockData.Id);
			}
			AtOManager.Instance.characterUnlockData = null;
		}
	}

	public bool IsCharacterUnlock()
	{
		if (unlockGO == null)
		{
			return false;
		}
		return unlockGO.gameObject.activeSelf;
	}

	public void CharacterUnlockClose()
	{
		UnityEngine.Object.Destroy(unlockGO);
	}

	public void ShowMask(bool state, float alpha = 1f)
	{
		if (maskCoroutine != null)
		{
			StopCoroutine(maskCoroutine);
		}
		if (state)
		{
			maskCoroutine = StartCoroutine(ShowMaskCo(alpha));
		}
		else
		{
			StartCoroutine(HideMaskCo());
		}
	}

	private IEnumerator ShowMaskCo(float alpha = 1f)
	{
		maskImage.color = new Color(0f, 0f, 0f, alpha);
		maskObject.gameObject.SetActive(value: true);
		maskMP.gameObject.SetActive(value: false);
		HidePopup();
		if (GameManager.Instance.IsMultiplayer())
		{
			yield return Globals.Instance.WaitForSeconds(0.2f);
			maskMP.gameObject.SetActive(value: true);
		}
	}

	private IEnumerator HideMaskCo()
	{
		maskMP.gameObject.SetActive(value: false);
		float index = maskImage.color.a;
		yield return Globals.Instance.WaitForSeconds(0.1f);
		while (index > 0f)
		{
			maskImage.color = new Color(0f, 0f, 0f, index);
			index -= 0.2f;
			yield return null;
		}
		HidePopup();
		maskObject.gameObject.SetActive(value: false);
	}

	public bool IsMaskActive()
	{
		if (maskObject.gameObject.activeSelf || maskMP.gameObject.activeSelf)
		{
			return true;
		}
		return false;
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false, bool shoulderLeft = false, bool shoulderRight = false)
	{
		if (corruption.IsActive())
		{
			corruption.ControllerMovement(goingUp, goingRight, goingDown, goingLeft, shoulderLeft, shoulderRight);
			return;
		}
		controllerShoulderCurrentOption = -1;
		if (controllerList.Count <= 0)
		{
			return;
		}
		if (controllerCurrentOption == -1)
		{
			for (int i = 0; i < controllerList.Count; i++)
			{
				if (controllerList[i] == mapNode[CurrentNode()].transform)
				{
					controllerCurrentOption = i;
					break;
				}
			}
		}
		else if (goingLeft || goingUp)
		{
			controllerCurrentOption--;
			if (controllerCurrentOption < 0)
			{
				controllerCurrentOption = 0;
			}
		}
		else
		{
			controllerCurrentOption++;
			if (controllerCurrentOption > controllerList.Count - 1)
			{
				controllerCurrentOption = controllerList.Count - 1;
			}
		}
		if (controllerList[controllerCurrentOption] != null && Functions.TransformIsVisible(controllerList[controllerCurrentOption]))
		{
			warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(controllerList[controllerCurrentOption].position);
			Mouse.current.WarpCursorPosition(warpPosition);
		}
	}

	public void ControllerMoveBlock(bool _isRight)
	{
		if (characterWindow.IsActive())
		{
			return;
		}
		if (corruption.IsActive())
		{
			corruption.ControllerMoveBlock(_isRight);
			return;
		}
		if (controllerCurrentOption > -1)
		{
			string text = instantTravelNodesTransform[controllerCurrentOption].gameObject.name;
			for (int i = 0; i < availableNodes.Count; i++)
			{
				if (text == availableNodesTransform[i].gameObject.name)
				{
					controllerShoulderCurrentOption = i;
					break;
				}
			}
		}
		controllerCurrentOption = -1;
		if (availableNodesTransform == null || availableNodesTransform.Count <= 0)
		{
			return;
		}
		if (!_isRight)
		{
			controllerShoulderCurrentOption--;
			if (controllerShoulderCurrentOption < 0)
			{
				controllerShoulderCurrentOption = availableNodesTransform.Count - 1;
			}
		}
		else
		{
			controllerShoulderCurrentOption++;
			if (controllerShoulderCurrentOption > availableNodesTransform.Count - 1)
			{
				controllerShoulderCurrentOption = 0;
			}
		}
		Transform transform = availableNodesTransform[controllerShoulderCurrentOption];
		if (transform != null && Functions.TransformIsVisible(transform))
		{
			Vector2 position = GameManager.Instance.cameraMain.WorldToScreenPoint(transform.position);
			Mouse.current.WarpCursorPosition(position);
		}
	}
}
