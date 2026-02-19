using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Paradox;
using Photon.Pun;
using UnityEngine;
using WebSocketSharp;

public class AtOManager : MonoBehaviour
{
	private string gameId = "";

	private string gameUniqueId = "";

	public Dictionary<string, string> gameNodeAssigned;

	public List<string> mapVisitedNodes;

	public List<string> mapVisitedNodesAction;

	public Dictionary<string, List<string>> shopList;

	private Dictionary<string, int> mpPlayersGold;

	private Dictionary<string, int> mpPlayersDust;

	public int totalGoldGained;

	public int totalDustGained;

	public List<int> lootCharacterOrder;

	public List<int> conflictCharacterOrder;

	public bool followingTheLeader;

	private bool charInTown;

	private int townTier;

	private string townZoneId = "";

	private int ngPlus;

	private string madnessCorruptors = "";

	private int obeliskMadness;

	private int singularityMadness;

	public int combatTotal;

	public int combatExpertise;

	public int bossesKilled;

	public List<string> bossesKilledName;

	public int monstersKilled;

	private int totalScoreTMP;

	public int mapVisitedNodesTMP;

	private int experienceGainedTMP;

	private int totalDeathsTMP;

	private int combatExpertiseTMP;

	private int bossesKilledTMP;

	private int corruptionCommonCompletedTMP;

	private int corruptionUncommonCompletedTMP;

	private int corruptionRareCompletedTMP;

	private int corruptionEpicCompletedTMP;

	public string currentMapNode = "";

	public bool comingFromCombatDoRewards;

	public List<string> upgradedCardsList;

	private CombatData currentCombatData;

	[SerializeField]
	private int playerDust;

	[SerializeField]
	private int playerGold;

	[SerializeField]
	internal Hero[] teamAtO;

	[SerializeField]
	private Hero[] teamAtOBackup;

	[SerializeField]
	private string[] teamNPCAtO;

	[SerializeField]
	private List<string> playerRequeriments;

	private PhotonView photonView;

	public TierRewardData townDivinationTier;

	public string townDivinationCreator;

	public int townDivinationCost;

	public int divinationsNumber;

	private TierRewardData eventRewardTier;

	public int currentRewardTier;

	public string shopItemReroll = "";

	public Dictionary<string, string> townRerollList = new Dictionary<string, string>();

	private string lootListId;

	public CombatData fromEventCombatData;

	public EventData fromEventEventData;

	public NodeData fromEventDestinationNode;

	public SubClassData characterUnlockData;

	public SkinData skinUnlockData;

	public ThermometerData combatThermometerData;

	public GameObject cardcratPrefab;

	private GameObject cardcraftGO;

	private CardCraftManager cardCraftManager;

	public List<Dictionary<string, int>> craftedCards;

	public Dictionary<string, List<string>> boughtItems;

	public Dictionary<string, int> boughtItemInShopByWho;

	public Dictionary<string, int> upgradedCardsInAltarByWho;

	public List<string> unlockedCards;

	public int[,] combatStats;

	public int[,] combatStatsCurrent;

	public bool advancedCraft;

	public bool affordableCraft;

	public List<string> craftFilterAura = new List<string>();

	public List<string> craftFilterCurse = new List<string>();

	public List<string> craftFilterDT = new List<string>();

	public bool saveLoadStatus;

	private int saveSlot = -1;

	public bool combatResigned;

	public int gameHandicap;

	public float playedTime;

	public Dictionary<string, List<string>> heroPerks = new Dictionary<string, List<string>>();

	public bool dreadfulPerformance;

	public bool corruptionAccepted;

	public int corruptionType = -1;

	public string corruptionId = "";

	public string corruptionIdCard = "";

	public string corruptionRewardCard = "";

	public int corruptionRewardChar = -1;

	public int corruptionCommonCompleted;

	public int corruptionUncommonCompleted;

	public int corruptionRareCompleted;

	public int corruptionEpicCompleted;

	private int townTutorialStep = -1;

	private bool adventureCompleted;

	private int weekly;

	private List<string> ChallengeTraits;

	private List<string> invalidTraitsForTougherMonsters;

	public string combatGameCode = "";

	public Dictionary<string, CardData> combatCardDictionary;

	public string combatScarab = "";

	public string obeliskLow = "";

	public string obeliskHigh = "";

	public string obeliskFinal = "";

	private List<string> mapNodeObeliskBoss = new List<string>();

	public CardPlayerPackData cardPlayerPackData;

	public CardPlayerPairsPackData cardPlayerPairsPackData;

	public Dictionary<string, Dictionary<string, string>> skinsInTheGame;

	public string CinematicId = "";

	private Dictionary<string, AuraCurseData> cacheGlobalACModification = new Dictionary<string, AuraCurseData>();

	private bool useCache = true;

	public string weeklyForcedId = "";

	private DateTime currentDate;

	private DateTime startDate;

	public bool SirenQueenBattle;

	public bool doubleFuryEffect;

	public bool doublePowerfulEffect;

	public bool IsCombatTool;

	private int sandbox_startingEnergy;

	private int sandbox_startingSpeed;

	private int sandbox_additionalGold;

	private int sandbox_additionalShards;

	private int sandbox_cardCraftPrice;

	private int sandbox_cardUpgradePrice;

	private int sandbox_cardTransformPrice;

	private int sandbox_cardRemovePrice;

	private int sandbox_itemsPrice;

	private int sandbox_petsPrice;

	private bool sandbox_craftUnlocked;

	private bool sandbox_allRarities;

	private bool sandbox_unlimitedAvailableCards;

	private bool sandbox_freeRerolls;

	private bool sandbox_unlimitedRerolls;

	private int sandbox_divinationsPrice;

	private bool sandbox_noMinimumDecksize;

	private bool sandbox_alwaysPassEventRoll;

	private int sandbox_additionalMonsterHP;

	private int sandbox_additionalMonsterDamage;

	private int sandbox_totalHeroes;

	private int sandbox_lessNPCs;

	private bool sandbox_doubleChampions;

	public static AtOManager Instance { get; private set; }

	public int TownTutorialStep
	{
		get
		{
			return townTutorialStep;
		}
		set
		{
			townTutorialStep = value;
		}
	}

	public int Sandbox_startingEnergy
	{
		get
		{
			return sandbox_startingEnergy;
		}
		set
		{
			sandbox_startingEnergy = value;
		}
	}

	public int Sandbox_startingSpeed
	{
		get
		{
			return sandbox_startingSpeed;
		}
		set
		{
			sandbox_startingSpeed = value;
		}
	}

	public int Sandbox_additionalGold
	{
		get
		{
			return sandbox_additionalGold;
		}
		set
		{
			sandbox_additionalGold = value;
		}
	}

	public int Sandbox_additionalShards
	{
		get
		{
			return sandbox_additionalShards;
		}
		set
		{
			sandbox_additionalShards = value;
		}
	}

	public int Sandbox_cardCraftPrice
	{
		get
		{
			return sandbox_cardCraftPrice;
		}
		set
		{
			sandbox_cardCraftPrice = value;
		}
	}

	public int Sandbox_cardUpgradePrice
	{
		get
		{
			return sandbox_cardUpgradePrice;
		}
		set
		{
			sandbox_cardUpgradePrice = value;
		}
	}

	public int Sandbox_cardTransformPrice
	{
		get
		{
			return sandbox_cardTransformPrice;
		}
		set
		{
			sandbox_cardTransformPrice = value;
		}
	}

	public int Sandbox_cardRemovePrice
	{
		get
		{
			return sandbox_cardRemovePrice;
		}
		set
		{
			sandbox_cardRemovePrice = value;
		}
	}

	public int Sandbox_itemsPrice
	{
		get
		{
			return sandbox_itemsPrice;
		}
		set
		{
			sandbox_itemsPrice = value;
		}
	}

	public int Sandbox_petsPrice
	{
		get
		{
			return sandbox_petsPrice;
		}
		set
		{
			sandbox_petsPrice = value;
		}
	}

	public bool Sandbox_allRarities
	{
		get
		{
			return sandbox_allRarities;
		}
		set
		{
			sandbox_allRarities = value;
		}
	}

	public bool Sandbox_unlimitedAvailableCards
	{
		get
		{
			return sandbox_unlimitedAvailableCards;
		}
		set
		{
			sandbox_unlimitedAvailableCards = value;
		}
	}

	public bool Sandbox_freeRerolls
	{
		get
		{
			return sandbox_freeRerolls;
		}
		set
		{
			sandbox_freeRerolls = value;
		}
	}

	public bool Sandbox_unlimitedRerolls
	{
		get
		{
			return sandbox_unlimitedRerolls;
		}
		set
		{
			sandbox_unlimitedRerolls = value;
		}
	}

	public int Sandbox_divinationsPrice
	{
		get
		{
			return sandbox_divinationsPrice;
		}
		set
		{
			sandbox_divinationsPrice = value;
		}
	}

	public bool Sandbox_noMinimumDecksize
	{
		get
		{
			return sandbox_noMinimumDecksize;
		}
		set
		{
			sandbox_noMinimumDecksize = value;
		}
	}

	public bool Sandbox_alwaysPassEventRoll
	{
		get
		{
			return sandbox_alwaysPassEventRoll;
		}
		set
		{
			sandbox_alwaysPassEventRoll = value;
		}
	}

	public int Sandbox_additionalMonsterHP
	{
		get
		{
			return sandbox_additionalMonsterHP;
		}
		set
		{
			sandbox_additionalMonsterHP = value;
		}
	}

	public int Sandbox_totalHeroes
	{
		get
		{
			return sandbox_totalHeroes;
		}
		set
		{
			sandbox_totalHeroes = value;
		}
	}

	public int Sandbox_lessNPCs
	{
		get
		{
			if ((currentCombatData != null && currentCombatData.NPCList != null && currentCombatData.NPCList.Any((NPCData npc) => npc != null && npc.Id == "count")) || IsCombatTool)
			{
				return 0;
			}
			return sandbox_lessNPCs;
		}
		set
		{
			sandbox_lessNPCs = value;
		}
	}

	public bool Sandbox_doubleChampions
	{
		get
		{
			return sandbox_doubleChampions;
		}
		set
		{
			sandbox_doubleChampions = value;
		}
	}

	public int Sandbox_additionalMonsterDamage
	{
		get
		{
			return sandbox_additionalMonsterDamage;
		}
		set
		{
			sandbox_additionalMonsterDamage = value;
		}
	}

	public bool Sandbox_craftUnlocked
	{
		get
		{
			return sandbox_craftUnlocked;
		}
		set
		{
			sandbox_craftUnlocked = value;
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
			UnityEngine.Object.Destroy(base.gameObject);
		}
		photonView = PhotonView.Get(this);
	}

	private void Start()
	{
		invalidTraitsForTougherMonsters = new List<string>();
		invalidTraitsForTougherMonsters.Add("fastmonsters");
		invalidTraitsForTougherMonsters.Add("vigorousmonsters");
		invalidTraitsForTougherMonsters.Add("resistantmonsters");
		InvokeRepeating("PlayedCounter", 0f, 1f);
	}

	private void PlayedCounter()
	{
		if (gameId != "" && !HeroSelectionManager.Instance && !FinishRunManager.Instance)
		{
			currentDate = DateTime.Now;
			playedTime += (float)currentDate.Subtract(startDate).TotalSeconds;
			startDate = currentDate;
		}
		else
		{
			startDate = DateTime.Now;
		}
	}

	public void GiveControlDbg()
	{
		for (byte b = 0; b < 4; b++)
		{
			GiveControlToPlayer(b, 0);
		}
	}

	[PunRPC]
	private void NET_GiveControlToPlayer(byte _hero, byte _player)
	{
		GiveControlToPlayer(_hero, _player, _fromNet: true);
	}

	public void GiveControlToPlayer(byte _hero, byte _player, bool _fromNet = false)
	{
		if (teamAtO[_hero] != null)
		{
			string playerNickPosition = NetworkManager.Instance.GetPlayerNickPosition(_player);
			if (playerNickPosition != "")
			{
				teamAtO[_hero].Owner = playerNickPosition;
				if (MatchManager.Instance != null)
				{
					MatchManager.Instance.SetOwnerForTeamHero(_hero, playerNickPosition);
				}
			}
		}
		if (GameManager.Instance.IsMultiplayer() && !_fromNet)
		{
			photonView.RPC("NET_GiveControlToPlayer", RpcTarget.Others, _hero, _player);
		}
	}

	public void ClearCorruption()
	{
		corruptionAccepted = false;
		corruptionType = -1;
		corruptionId = "";
		corruptionIdCard = "";
		corruptionRewardCard = "";
		corruptionRewardChar = -1;
	}

	public string GetSandboxMods()
	{
		return JsonConvert.SerializeObject(new Dictionary<string, int>
		{
			{
				"sb_isEnabled",
				Convert.ToInt32(SandboxManager.Instance.IsEnabled())
			},
			{ "sb_startingEnergy", sandbox_startingEnergy },
			{ "sb_startingSpeed", sandbox_startingSpeed },
			{ "sb_additionalGold", sandbox_additionalGold },
			{ "sb_additionalShards", sandbox_additionalShards },
			{ "sb_cardCraftPrice", sandbox_cardCraftPrice },
			{ "sb_cardUpgradePrice", sandbox_cardUpgradePrice },
			{ "sb_cardTransformPrice", sandbox_cardTransformPrice },
			{ "sb_cardRemovePrice", sandbox_cardRemovePrice },
			{ "sb_itemsPrice", sandbox_itemsPrice },
			{ "sb_petsPrice", sandbox_petsPrice },
			{ "sb_divinationsPrice", sandbox_divinationsPrice },
			{
				"sb_craftUnlocked",
				Convert.ToInt32(sandbox_craftUnlocked)
			},
			{
				"sb_allRarities",
				Convert.ToInt32(sandbox_allRarities)
			},
			{
				"sb_unlimitedAvailableCards",
				Convert.ToInt32(sandbox_unlimitedAvailableCards)
			},
			{
				"sb_freeRerolls",
				Convert.ToInt32(sandbox_freeRerolls)
			},
			{
				"sb_unlimitedRerolls",
				Convert.ToInt32(sandbox_unlimitedRerolls)
			},
			{
				"sb_noMinimumDecksize",
				Convert.ToInt32(sandbox_noMinimumDecksize)
			},
			{
				"sb_alwaysPassEventRoll",
				Convert.ToInt32(sandbox_alwaysPassEventRoll)
			},
			{ "sb_additionalMonsterHP", sandbox_additionalMonsterHP },
			{ "sb_additionalMonsterDamage", sandbox_additionalMonsterDamage },
			{ "sb_totalHeroes", sandbox_totalHeroes },
			{ "sb_lessNPCs", sandbox_lessNPCs },
			{
				"sb_doubleChampions",
				Convert.ToInt32(sandbox_doubleChampions)
			}
		});
	}

	public void SetSandboxMods(string json)
	{
		if (json != null && json != "")
		{
			Dictionary<string, int> dictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
			if (dictionary["sb_isEnabled"] == 1)
			{
				SandboxManager.Instance.EnableSandbox();
			}
			else
			{
				SandboxManager.Instance.DisableSandbox();
			}
			sandbox_startingEnergy = (dictionary.ContainsKey("sb_startingEnergy") ? dictionary["sb_startingEnergy"] : 0);
			sandbox_startingSpeed = (dictionary.ContainsKey("sb_startingSpeed") ? dictionary["sb_startingSpeed"] : 0);
			sandbox_additionalGold = (dictionary.ContainsKey("sb_additionalGold") ? dictionary["sb_additionalGold"] : 0);
			sandbox_additionalShards = (dictionary.ContainsKey("sb_additionalShards") ? dictionary["sb_additionalShards"] : 0);
			sandbox_cardCraftPrice = (dictionary.ContainsKey("sb_cardCraftPrice") ? dictionary["sb_cardCraftPrice"] : 0);
			sandbox_cardUpgradePrice = (dictionary.ContainsKey("sb_cardUpgradePrice") ? dictionary["sb_cardUpgradePrice"] : 0);
			sandbox_cardTransformPrice = (dictionary.ContainsKey("sb_cardTransformPrice") ? dictionary["sb_cardTransformPrice"] : 0);
			sandbox_cardRemovePrice = (dictionary.ContainsKey("sb_cardRemovePrice") ? dictionary["sb_cardRemovePrice"] : 0);
			sandbox_itemsPrice = (dictionary.ContainsKey("sb_itemsPrice") ? dictionary["sb_itemsPrice"] : 0);
			sandbox_petsPrice = (dictionary.ContainsKey("sb_petsPrice") ? dictionary["sb_petsPrice"] : 0);
			sandbox_divinationsPrice = (dictionary.ContainsKey("sb_divinationsPrice") ? dictionary["sb_divinationsPrice"] : 0);
			sandbox_craftUnlocked = dictionary.ContainsKey("sb_craftUnlocked") && Convert.ToBoolean(dictionary["sb_craftUnlocked"]);
			sandbox_allRarities = dictionary.ContainsKey("sb_allRarities") && Convert.ToBoolean(dictionary["sb_allRarities"]);
			sandbox_unlimitedAvailableCards = dictionary.ContainsKey("sb_unlimitedAvailableCards") && Convert.ToBoolean(dictionary["sb_unlimitedAvailableCards"]);
			sandbox_freeRerolls = dictionary.ContainsKey("sb_freeRerolls") && Convert.ToBoolean(dictionary["sb_freeRerolls"]);
			sandbox_unlimitedRerolls = dictionary.ContainsKey("sb_unlimitedRerolls") && Convert.ToBoolean(dictionary["sb_unlimitedRerolls"]);
			sandbox_noMinimumDecksize = dictionary.ContainsKey("sb_noMinimumDecksize") && Convert.ToBoolean(dictionary["sb_noMinimumDecksize"]);
			sandbox_alwaysPassEventRoll = dictionary.ContainsKey("sb_alwaysPassEventRoll") && Convert.ToBoolean(dictionary["sb_alwaysPassEventRoll"]);
			sandbox_additionalMonsterHP = (dictionary.ContainsKey("sb_additionalMonsterHP") ? dictionary["sb_additionalMonsterHP"] : 0);
			sandbox_additionalMonsterDamage = (dictionary.ContainsKey("sb_additionalMonsterDamage") ? dictionary["sb_additionalMonsterDamage"] : 0);
			sandbox_totalHeroes = (dictionary.ContainsKey("sb_totalHeroes") ? dictionary["sb_totalHeroes"] : 0);
			sandbox_lessNPCs = (dictionary.ContainsKey("sb_lessNPCs") ? dictionary["sb_lessNPCs"] : 0);
			sandbox_doubleChampions = dictionary.ContainsKey("sb_doubleChampions") && Convert.ToBoolean(dictionary["sb_doubleChampions"]);
		}
	}

	private void ShareSandbox()
	{
		photonView.RPC("NET_ShareSandbox", RpcTarget.Others, GetSandboxMods());
		SandboxManager.Instance.LoadValuesFromAtOManager();
	}

	[PunRPC]
	private void NET_ShareSandbox(string _sandboxJson)
	{
		SetSandboxMods(_sandboxJson);
		SandboxManager.Instance.LoadValuesFromAtOManager();
	}

	public void ClearSandbox()
	{
		sandbox_startingEnergy = 0;
		sandbox_startingSpeed = 0;
		sandbox_additionalGold = 0;
		sandbox_additionalShards = 0;
		sandbox_cardCraftPrice = 0;
		sandbox_cardUpgradePrice = 0;
		sandbox_cardTransformPrice = 0;
		sandbox_cardRemovePrice = 0;
		sandbox_itemsPrice = 0;
		sandbox_petsPrice = 0;
		sandbox_divinationsPrice = 0;
		sandbox_craftUnlocked = false;
		sandbox_allRarities = false;
		sandbox_unlimitedAvailableCards = false;
		sandbox_freeRerolls = false;
		sandbox_unlimitedRerolls = false;
		sandbox_noMinimumDecksize = false;
		sandbox_alwaysPassEventRoll = false;
		sandbox_totalHeroes = 0;
		sandbox_lessNPCs = 0;
		sandbox_additionalMonsterHP = 0;
		sandbox_additionalMonsterDamage = 0;
		sandbox_doubleChampions = false;
	}

	public void InitCombatStats()
	{
		combatStats = new int[4, 12];
	}

	public void InitCombatStatsCurrent()
	{
		if (combatStats == null)
		{
			InitCombatStats();
		}
		combatStatsCurrent = new int[combatStats.GetLength(0), combatStats.GetLength(1)];
	}

	public void ClearGame()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[ATO] ClearGame", "trace");
		}
		PlayerManager.Instance.RecreateSkins();
		playedTime = 0f;
		teamAtO = null;
		combatGameCode = "";
		weeklyForcedId = "";
		CleanGameId();
		ClearCurrentCombatData();
		charInTown = false;
		townTier = 0;
		currentMapNode = "";
		mapVisitedNodes = new List<string>();
		mapVisitedNodesAction = new List<string>();
		comingFromCombatDoRewards = false;
		mpPlayersGold = new Dictionary<string, int>();
		mpPlayersDust = new Dictionary<string, int>();
		totalGoldGained = 0;
		totalDustGained = 0;
		townTutorialStep = -1;
		ngPlus = 0;
		madnessCorruptors = "";
		obeliskMadness = 0;
		singularityMadness = 0;
		ChallengeTraits = new List<string>();
		weekly = 0;
		characterUnlockData = null;
		skinUnlockData = null;
		adventureCompleted = false;
		upgradedCardsList = new List<string>();
		shopList = new Dictionary<string, List<string>>();
		boughtItems = new Dictionary<string, List<string>>();
		boughtItemInShopByWho = new Dictionary<string, int>();
		upgradedCardsInAltarByWho = new Dictionary<string, int>();
		lootCharacterOrder = new List<int>();
		conflictCharacterOrder = new List<int>();
		gameNodeAssigned = new Dictionary<string, string>();
		playerRequeriments = new List<string>();
		divinationsNumber = 0;
		eventRewardTier = null;
		townDivinationTier = null;
		currentRewardTier = 0;
		craftedCards = new List<Dictionary<string, int>>();
		for (int i = 0; i < 4; i++)
		{
			craftedCards.Add(new Dictionary<string, int>());
		}
		playerDust = 0;
		playerGold = 0;
		fromEventCombatData = null;
		fromEventEventData = null;
		unlockedCards = new List<string>();
		InitCombatStats();
		totalScoreTMP = 0;
		combatExpertise = 0;
		combatTotal = 0;
		bossesKilled = 0;
		bossesKilledName = new List<string>();
		monstersKilled = 0;
		combatResigned = false;
		PlayerUIManager.Instance.ClearBag();
		corruptionCommonCompleted = 0;
		corruptionUncommonCompleted = 0;
		corruptionRareCompleted = 0;
		corruptionEpicCompleted = 0;
		ClearCorruption();
		ClearReroll();
		ClearSandbox();
		ClearTemporalNodeScore();
		GameManager.Instance.SetGameStatus(Enums.GameStatus.NewGame);
		obeliskLow = "";
		obeliskHigh = "";
		obeliskFinal = "";
		mapNodeObeliskBoss.Clear();
		heroPerks = new Dictionary<string, List<string>>();
		NetworkManager.Instance.ClearSyncro();
		NetworkManager.Instance.ClearPlayerStatus();
		NetworkManager.Instance.ClearWaitingCalls();
	}

	public void RemoveSave()
	{
		if (saveSlot != -1)
		{
			SaveManager.DeleteGame(saveSlot);
		}
	}

	public void SetSaveSlot(int slot)
	{
		saveSlot = slot;
	}

	public int GetSaveSlot()
	{
		return saveSlot;
	}

	public void CleanSaveSlot()
	{
		saveSlot = -1;
	}

	public void SaveCauseTreasure()
	{
		photonView.RPC("NET_SaveCauseTresure", RpcTarget.MasterClient);
	}

	[PunRPC]
	public void NET_SaveCauseTresure()
	{
		SaveGame();
	}

	public void SaveGameTurn()
	{
		if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
		{
			return;
		}
		if ((bool)MatchManager.Instance)
		{
			NPC[] teamNPC = MatchManager.Instance.GetTeamNPC();
			foreach (NPC nPC in teamNPC)
			{
				if (nPC != null && !nPC.Alive && nPC.NpcData != null && nPC.NpcData.FinishCombatOnDead)
				{
					return;
				}
			}
		}
		if (saveSlot > -1)
		{
			SaveManager.SaveGameTurn(saveSlot);
		}
	}

	public void LoadGameTurn()
	{
		SaveManager.LoadGameTurn(saveSlot);
	}

	public void DeleteSaveGameTurn()
	{
		if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
		{
			SaveManager.DeleteSaveGameTurn(saveSlot);
		}
	}

	public void SaveGame(int slot = -1, bool backUp = false)
	{
		if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
		{
			saveLoadStatus = false;
			return;
		}
		saveLoadStatus = true;
		if (slot == -1)
		{
			slot = saveSlot;
		}
		if (slot != -1)
		{
			SaveManager.SaveGame(slot, backUp);
		}
		else
		{
			saveLoadStatus = false;
		}
	}

	public void LoadGame(int slot = -1, bool comingFromReloadCombat = false)
	{
		saveLoadStatus = true;
		if (slot == -1)
		{
			slot = saveSlot;
		}
		if (slot != -1)
		{
			SaveManager.LoadGame(slot, comingFromReloadCombat);
			return;
		}
		saveLoadStatus = false;
		if (GameManager.Instance.GetDeveloperMode() || IsCombatTool)
		{
			SceneStatic.LoadByName("Combat");
		}
	}

	public void UseManyResources()
	{
		GivePlayer(0, 99999);
		GivePlayer(1, 99999);
		GivePlayer(2, 500);
		int playerRankProgress = Globals.Instance.PerkLevel[Globals.Instance.PerkLevel.Count - 1];
		PlayerManager.Instance.SetPlayerRankProgress(playerRankProgress);
	}

	public void UnlockAllHeroes()
	{
		PlayerManager.Instance.UnlockHeroes();
	}

	public void MovePetFromAccesoryItem()
	{
		if (teamAtO == null)
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			if (teamAtO[i] == null)
			{
				continue;
			}
			string accesory = teamAtO[i].Accesory;
			if (accesory != "")
			{
				CardData cardData = Globals.Instance.GetCardData(accesory, instantiate: false);
				if (cardData != null && cardData.CardType == Enums.CardType.Pet)
				{
					teamAtO[i].Pet = accesory;
					teamAtO[i].Accesory = "";
				}
			}
		}
	}

	public bool CheckLoadGameUserHaveAllContent()
	{
		return true;
	}

	public void DoLoadGame(bool comingFromReloadCombat = false)
	{
		if (!GameManager.Instance.IsTutorialGame())
		{
			townTutorialStep = -1;
		}
		MovePetFromAccesoryItem();
		if (!GameManager.Instance.IsMultiplayer())
		{
			if (!CheckLoadGameUserHaveAllContent())
			{
				ClearGame();
				return;
			}
			RedoSkins();
			if (charInTown)
			{
				SceneStatic.LoadByName("Town");
			}
			else
			{
				SceneStatic.LoadByName("Map");
			}
			if (!comingFromReloadCombat)
			{
				StartCoroutine(SendStartTelemetryCo(isLoading: true));
			}
		}
		else
		{
			SceneStatic.LoadByName("Lobby");
		}
	}

	public void DoLoadGameFromMP(bool comingFromReloadCombat = false)
	{
		MovePetFromAccesoryItem();
		StartCoroutine(ShareGameMP(comingFromReloadCombat));
	}

	private void ShareNGPlus()
	{
		int num = ngPlus;
		string text = madnessCorruptors;
		photonView.RPC("NET_ShareNGPlus", RpcTarget.Others, num, text);
	}

	[PunRPC]
	private void NET_ShareNGPlus(int _ngPlus, string _madnessCorruptors)
	{
		ngPlus = _ngPlus;
		madnessCorruptors = _madnessCorruptors;
	}

	private void ShareSingularityMadness()
	{
		photonView.RPC("NET_ShareSingularityMadnesss", RpcTarget.Others, singularityMadness);
	}

	[PunRPC]
	private void NET_ShareSingularityMadnesss(int _singularityMadness)
	{
		SetSingularityMadness(_singularityMadness);
	}

	private void ShareObeliskMadnessAndMaps()
	{
		photonView.RPC("NET_ShareObeliskMadness", RpcTarget.Others, obeliskMadness, obeliskLow, obeliskHigh, obeliskFinal);
	}

	[PunRPC]
	private void NET_ShareObeliskMadness(int _obeliskMadness, string _oLow, string _oHigh, string _oFinal)
	{
		SetObeliskMadness(_obeliskMadness);
		obeliskLow = _oLow;
		obeliskHigh = _oHigh;
		obeliskFinal = _oFinal;
	}

	public IEnumerator ShareGameMP(bool comingFromReloadCombat = false)
	{
		combatGameCode = "";
		string text = gameId;
		string text2 = gameUniqueId;
		string text3 = currentMapNode;
		int num = ngPlus;
		string text4 = madnessCorruptors;
		int num2 = obeliskMadness;
		int num3 = singularityMadness;
		string text5 = "";
		if (fromEventCombatData != null)
		{
			text5 = fromEventCombatData.CombatId;
		}
		string text6 = "";
		if (fromEventEventData != null)
		{
			text6 = fromEventEventData.EventId;
		}
		string text7 = JsonHelper.ToJson(mapVisitedNodes.ToArray());
		string text8 = "";
		if (mapVisitedNodesAction != null)
		{
			text8 = JsonHelper.ToJson(mapVisitedNodesAction.ToArray());
		}
		string text9 = JsonHelper.ToJson(playerRequeriments.ToArray());
		string text10 = JsonHelper.ToJson(lootCharacterOrder.ToArray());
		string text11 = JsonHelper.ToJson(conflictCharacterOrder.ToArray());
		string text12 = "";
		string text13 = "";
		string text14 = "";
		string text15 = "";
		string text16 = "";
		for (int i = 0; i < combatStats.GetLength(0); i++)
		{
			for (int j = 0; j < combatStats.GetLength(1); j++)
			{
				text16 = text16 + combatStats[i, j] + "_";
			}
			text16 += "/";
		}
		if (boughtItemInShopByWho != null)
		{
			string[] array = new string[boughtItemInShopByWho.Count];
			boughtItemInShopByWho.Keys.CopyTo(array, 0);
			int[] array2 = new int[boughtItemInShopByWho.Count];
			boughtItemInShopByWho.Values.CopyTo(array2, 0);
			text12 = JsonHelper.ToJson(array);
			text13 = JsonHelper.ToJson(array2);
		}
		if (upgradedCardsInAltarByWho != null)
		{
			string[] array3 = new string[upgradedCardsInAltarByWho.Count];
			upgradedCardsInAltarByWho.Keys.CopyTo(array3, 0);
			int[] array4 = new int[upgradedCardsInAltarByWho.Count];
			upgradedCardsInAltarByWho.Values.CopyTo(array4, 0);
			text14 = JsonHelper.ToJson(array3);
			text15 = JsonHelper.ToJson(array4);
		}
		int num4 = townTier;
		int num5 = combatExpertise;
		int num6 = bossesKilled;
		int num7 = monstersKilled;
		string text17 = JsonHelper.ToJson(bossesKilledName.ToArray());
		bool flag = corruptionAccepted;
		int num8 = corruptionType;
		string text18 = corruptionId;
		string text19 = corruptionIdCard;
		string text20 = corruptionRewardCard;
		int num9 = corruptionRewardChar;
		_ = corruptionCommonCompleted;
		if (corruptionCommonCompleted < 0)
		{
			corruptionCommonCompleted = 0;
		}
		_ = corruptionUncommonCompleted;
		if (corruptionUncommonCompleted < 0)
		{
			corruptionUncommonCompleted = 0;
		}
		_ = corruptionRareCompleted;
		if (corruptionRareCompleted < 0)
		{
			corruptionRareCompleted = 0;
		}
		_ = corruptionEpicCompleted;
		if (corruptionEpicCompleted < 0)
		{
			corruptionEpicCompleted = 0;
		}
		string text21 = corruptionCommonCompleted + "|" + corruptionUncommonCompleted + "|" + corruptionRareCompleted + "|" + corruptionEpicCompleted;
		string text22 = "";
		string text23 = "";
		if (craftedCards != null && craftedCards != null)
		{
			for (int k = 0; k < 4; k++)
			{
				if (craftedCards[k] == null || craftedCards[k].Count == 0)
				{
					text22 += "_";
					text23 += "_";
					continue;
				}
				string[] array5 = new string[craftedCards[k].Count];
				craftedCards[k].Keys.CopyTo(array5, 0);
				int[] array6 = new int[craftedCards[k].Count];
				craftedCards[k].Values.CopyTo(array6, 0);
				text22 = text22 + JsonHelper.ToJson(array5) + "_";
				text23 = text23 + JsonHelper.ToJson(array6) + "_";
			}
		}
		float num10 = playedTime;
		int num11 = 0;
		if (GameManager.Instance.IsObeliskChallenge())
		{
			num11 = ((!GameManager.Instance.IsWeeklyChallenge()) ? 1 : 2);
		}
		else if (GameManager.Instance.IsSingularity())
		{
			num11 = 3;
		}
		int num12 = weekly;
		JsonHelper.ToJson(craftedCards.ToArray());
		string text24 = "";
		string text25 = "";
		if (townRerollList != null && townRerollList.Count > 0)
		{
			string[] array7 = new string[townRerollList.Count];
			townRerollList.Keys.CopyTo(array7, 0);
			string[] array8 = new string[townRerollList.Count];
			townRerollList.Values.CopyTo(array8, 0);
			text24 = JsonHelper.ToJson(array7);
			text25 = JsonHelper.ToJson(array8);
		}
		NetworkManager.Instance.ClearAllPlayerManualReady();
		if (!comingFromReloadCombat)
		{
			StartCoroutine(SendStartTelemetryCo(isLoading: true));
		}
		photonView.RPC("NET_ShareGameMP", RpcTarget.Others, text, text2, num4, text3, num, text4, num2, num3, text5, text6, text7, text8, text9, text12, text13, text14, text15, text10, text11, text16, num5, num6, text17, num7, flag, num8, text18, text19, text20, num9, text22, text23, text21, num10, num11, num12, text24, text25, comingFromReloadCombat);
		while (!NetworkManager.Instance.AllPlayersReady("sharegamemp"))
		{
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
		GetTotalPlayersGold();
		GetTotalPlayersDust();
		int numPlayers = NetworkManager.Instance.GetNumPlayers();
		mpPlayersGold = new Dictionary<string, int>();
		mpPlayersDust = new Dictionary<string, int>();
		for (int l = 0; l < numPlayers; l++)
		{
			float num13 = 0f;
			float num14 = 0f;
			string playerNickPosition = NetworkManager.Instance.GetPlayerNickPosition(l);
			for (int m = 0; m < 4; m++)
			{
				if (NetworkManager.Instance.PlayerHeroPositionOwner[m] == playerNickPosition)
				{
					num13 += teamAtO[m].HeroGold;
					num14 += teamAtO[m].HeroDust;
				}
			}
			GivePlayer(0, Mathf.CeilToInt(num13), playerNickPosition, "", anim: false);
			GivePlayer(1, Mathf.CeilToInt(num14), playerNickPosition, "", anim: false);
		}
		if ((bool)MatchManager.Instance)
		{
			StartCoroutine(ShareTeam("Combat", setOwners: true));
		}
		else if (charInTown)
		{
			StartCoroutine(ShareTeam("Town", setOwners: true));
		}
		else
		{
			StartCoroutine(ShareTeam("Map", setOwners: true));
		}
	}

	[PunRPC]
	private void NET_ShareGameMP(string _gameId, string _gameUniqueId, int _townTier, string _currentMapNode, int _ngPlus, string _madnessCorruptors, int _obeliskMadness, int _singularityMadness, string _fromEventCombatDataId, string _fromEventEventDataId, string _mapVisitedNodes, string _mapVisitedNodesAction, string _playerRequeriments, string _boughtKeys, string _boughtValues, string _upgradedKeys, string _upgradedValues, string _lootCharacterOrder, string _conflictCharacterOrder, string _combatStats, int _combatExpertise, int _bossesKilled, string _bossesKilledName, int _monstersKilled, bool _corruptionAccepted, int _corruptionType, string _corruptionId, string _corruptionIdCard, string _corruptionRewardCard, int _corruptionRewardChar, string _craftedKeys, string _craftedValues, string _corruptionsCompleted, float _playedTime, int _gameType, int _weekly, string _rerollKeys, string _rerollValues, bool comingFromReloadCombat)
	{
		combatGameCode = "";
		corruptionAccepted = _corruptionAccepted;
		corruptionType = _corruptionType;
		corruptionId = _corruptionId;
		corruptionIdCard = _corruptionIdCard;
		corruptionRewardCard = _corruptionRewardCard;
		corruptionRewardChar = _corruptionRewardChar;
		string[] array = _corruptionsCompleted.Split('|');
		if (array[0] != null)
		{
			corruptionCommonCompleted = int.Parse(array[0]);
		}
		if (array[1] != null)
		{
			corruptionUncommonCompleted = int.Parse(array[1]);
		}
		if (array[2] != null)
		{
			corruptionRareCompleted = int.Parse(array[2]);
		}
		if (array[3] != null)
		{
			corruptionEpicCompleted = int.Parse(array[3]);
		}
		ngPlus = _ngPlus;
		madnessCorruptors = _madnessCorruptors;
		SetObeliskMadness(_obeliskMadness);
		SetSingularityMadness(_singularityMadness);
		gameId = _gameId;
		gameUniqueId = _gameUniqueId;
		townTier = _townTier;
		combatExpertise = _combatExpertise;
		bossesKilled = _bossesKilled;
		monstersKilled = _monstersKilled;
		SetTownTier(townTier);
		currentMapNode = _currentMapNode;
		if (Globals.Instance.GetNodeData(_currentMapNode) != null)
		{
			SetTownZoneId(Globals.Instance.GetNodeData(_currentMapNode).NodeZone.ZoneId);
		}
		fromEventCombatData = Globals.Instance.GetCombatData(_fromEventCombatDataId);
		fromEventEventData = Globals.Instance.GetEventData(_fromEventEventDataId);
		mapVisitedNodes = JsonHelper.FromJson<string>(_mapVisitedNodes).ToList();
		if (_mapVisitedNodesAction != null && _mapVisitedNodesAction != "")
		{
			mapVisitedNodesAction = JsonHelper.FromJson<string>(_mapVisitedNodesAction).ToList();
		}
		else
		{
			mapVisitedNodesAction = new List<string>();
		}
		playerRequeriments = JsonHelper.FromJson<string>(_playerRequeriments).ToList();
		bossesKilledName = JsonHelper.FromJson<string>(_bossesKilledName).ToList();
		string[] array2 = _combatStats.Split('/');
		string[] array3 = array2[0].Split('_');
		combatStats = new int[array2.Length - 1, array3.Length - 1];
		string[] array4 = _combatStats.Split('/');
		for (int i = 0; i < combatStats.GetLength(0); i++)
		{
			string[] array5 = array4[i].Split('_');
			for (int j = 0; j < combatStats.GetLength(1); j++)
			{
				if (j < array5.Length && array5[j] != null)
				{
					combatStats[i, j] = ((!string.IsNullOrEmpty(array5[j])) ? int.Parse(array5[j]) : 0);
				}
				else
				{
					combatStats[i, j] = 0;
				}
			}
		}
		if (_boughtKeys != "")
		{
			string[] array6 = JsonHelper.FromJson<string>(_boughtKeys);
			int[] array7 = JsonHelper.FromJson<int>(_boughtValues);
			boughtItemInShopByWho = new Dictionary<string, int>();
			for (int k = 0; k < array6.Length; k++)
			{
				boughtItemInShopByWho.Add(array6[k], array7[k]);
			}
		}
		if (_upgradedKeys != "")
		{
			string[] array8 = JsonHelper.FromJson<string>(_upgradedKeys);
			int[] array9 = JsonHelper.FromJson<int>(_upgradedValues);
			upgradedCardsInAltarByWho = new Dictionary<string, int>();
			for (int l = 0; l < array8.Length; l++)
			{
				upgradedCardsInAltarByWho.Add(array8[l], array9[l]);
			}
		}
		lootCharacterOrder = JsonHelper.FromJson<int>(_lootCharacterOrder).ToList();
		conflictCharacterOrder = JsonHelper.FromJson<int>(_conflictCharacterOrder).ToList();
		if (_craftedKeys != "")
		{
			string[] array10 = _craftedKeys.Split('_');
			string[] array11 = _craftedValues.Split('_');
			craftedCards = new List<Dictionary<string, int>>();
			for (int m = 0; m < 4; m++)
			{
				if (array10[m].Length > 0)
				{
					string[] array12 = JsonHelper.FromJson<string>(array10[m]);
					if (array12.Length != 0)
					{
						int[] array13 = JsonHelper.FromJson<int>(array11[m]);
						Dictionary<string, int> dictionary = new Dictionary<string, int>();
						for (int n = 0; n < array12.Length; n++)
						{
							dictionary.Add(array12[n], array13[n]);
						}
						craftedCards.Add(dictionary);
					}
					else
					{
						craftedCards.Add(new Dictionary<string, int>());
					}
				}
				else
				{
					craftedCards.Add(new Dictionary<string, int>());
				}
			}
		}
		playedTime = _playedTime;
		switch (_gameType)
		{
		case 0:
			GameManager.Instance.GameType = Enums.GameType.Adventure;
			break;
		case 1:
			GameManager.Instance.GameType = Enums.GameType.Challenge;
			break;
		case 2:
			GameManager.Instance.GameType = Enums.GameType.WeeklyChallenge;
			break;
		case 3:
			GameManager.Instance.GameType = Enums.GameType.Singularity;
			break;
		}
		SetWeekly(_weekly);
		if (_rerollKeys != "")
		{
			string[] array14 = JsonHelper.FromJson<string>(_rerollKeys);
			string[] array15 = JsonHelper.FromJson<string>(_rerollValues);
			townRerollList = new Dictionary<string, string>();
			string playerNick = NetworkManager.Instance.GetPlayerNick();
			for (int num = 0; num < array14.Length; num++)
			{
				townRerollList.Add(array14[num], array15[num]);
				if (array14[num] == playerNick)
				{
					string[] array16 = array15[num].Split("_");
					if (array16.Length != 0)
					{
						shopItemReroll = array16[^1];
					}
				}
			}
		}
		NetworkManager.Instance.SetStatusReady("sharegamemp");
		if (!comingFromReloadCombat)
		{
			StartCoroutine(SendStartTelemetryCo(isLoading: true));
		}
	}

	public void SwapCharacter(SubClassData subClassData, int position)
	{
		if (position < 0 || position >= 4)
		{
			return;
		}
		Hero hero = teamAtO[position];
		if (hero != null)
		{
			string owner = hero.Owner;
			int experience = hero.Experience;
			Hero hero2 = GameManager.Instance.CreateHero(subClassData.SubClassName);
			AssignZeroTrait(hero2);
			hero2.Owner = owner;
			teamAtO[position] = hero2;
			HeroCards[] cardsOnReplaceCharacter = subClassData.CardsOnReplaceCharacter;
			if (cardsOnReplaceCharacter != null && cardsOnReplaceCharacter.Length != 0)
			{
				ReplaceHeroCards(subClassData, position);
			}
			string perksOnReplace = subClassData.PerksOnReplace;
			if (perksOnReplace != null && perksOnReplace.Length > 0)
			{
				ReplaceHeroPerks(subClassData, position);
			}
			if (subClassData.UseXpFromOriginal)
			{
				hero2.Experience = experience;
			}
		}
	}

	private void ReplaceHeroPerks(SubClassData subClassData, int heroIndex)
	{
		List<string> list = Functions.ParseCompressedCodeList(subClassData.PerksOnReplace);
		if (list == null)
		{
			return;
		}
		foreach (string item in list)
		{
			AddPerkToHero(heroIndex, item);
		}
	}

	private void ReplaceHeroCards(SubClassData subClassData, int heroIndex)
	{
		RemoveAllCardsInDeck(heroIndex);
		HeroCards[] cardsOnReplaceCharacter = subClassData.CardsOnReplaceCharacter;
		foreach (HeroCards heroCards in cardsOnReplaceCharacter)
		{
			if (GameManager.Instance.IsMultiplayer())
			{
				AddCardToHeroMP(heroIndex, heroCards.Card.Id, heroCards.UnitsInDeck);
			}
			else
			{
				AddCardToHero(heroIndex, heroCards.Card.Id, heroCards.UnitsInDeck);
			}
		}
	}

	public void TravelBetweenZones(ZoneData _fromZone, ZoneData _toZone)
	{
		if (_toZone.ChangeTeamOnEntrance)
		{
			if (_toZone.NewTeam != null && _toZone.NewTeam.Count == 4)
			{
				CreateTeamBackup();
				Array.Copy(teamAtO, teamAtOBackup, 4);
				string[] array = new string[4];
				for (int i = 0; i < 4; i++)
				{
					array[i] = _toZone.NewTeam[i].SubClassName;
				}
				SetTeamFromArray(array);
				for (int j = 0; j < 4; j++)
				{
					if (teamAtOBackup[j] != null && teamAtO[j] != null)
					{
						teamAtO[j].Owner = teamAtOBackup[j].Owner;
					}
				}
				if (GameManager.Instance.IsMultiplayer() && SandboxManager.Instance.IsEnabled())
				{
					if (sandbox_totalHeroes == 2)
					{
						teamAtO[2].Owner = teamAtOBackup[0].Owner;
						teamAtO[3].Owner = teamAtOBackup[1].Owner;
					}
					else if (sandbox_totalHeroes == 3)
					{
						teamAtO[3].Owner = teamAtOBackup[0].Owner;
					}
				}
			}
		}
		else if (_fromZone.RestoreTeamOnExit && teamAtOBackup != null)
		{
			for (int k = 0; k < 4; k++)
			{
				if (teamAtOBackup[k] != null && teamAtO[k] != null)
				{
					teamAtOBackup[k].Owner = teamAtO[k].Owner;
				}
			}
			CreateTeam();
			Array.Copy(teamAtOBackup, teamAtO, 4);
			teamAtOBackup = new Hero[4];
		}
		SetTownZoneId(_toZone.ZoneId);
		SceneStatic.LoadByName("IntroNewGame");
	}

	public void SetTownZoneId(string _townZoneId)
	{
		townZoneId = _townZoneId;
	}

	public string GetTownZoneId()
	{
		return townZoneId;
	}

	public void SetCharInTown(bool _state)
	{
		charInTown = _state;
		TownRerollRestore();
	}

	public bool CharInTown()
	{
		return charInTown;
	}

	public void SetTownTier(int _townTier)
	{
		townTier = _townTier;
		gameHandicap = townTier;
		if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
		{
			photonView.RPC("NET_SetTownTier", RpcTarget.Others, townTier);
		}
	}

	[PunRPC]
	private void NET_SetTownTier(int _townTier)
	{
		SetTownTier(_townTier);
	}

	public int GetActNumberForText(string _currentMapNode = "")
	{
		int num = 0;
		if (_currentMapNode == "")
		{
			_currentMapNode = currentMapNode;
		}
		if (GameManager.Instance.IsObeliskChallenge())
		{
			num = (NodeIsObeliskLow() ? 1 : ((!NodeIsObeliskFinal()) ? 2 : 3));
		}
		else if (_currentMapNode != "")
		{
			switch (_currentMapNode)
			{
			case "sen_0":
			case "tutorial_0":
			case "tutorial_1":
			case "tutorial_2":
				return 1;
			case "dream_0":
				return townTier + 1;
			case "sunken_0":
				return 2;
			}
			num = townTier + 1;
			if (Globals.Instance.GetNodeData(_currentMapNode) != null)
			{
				string[] array = Globals.Instance.GetNodeData(_currentMapNode).NodeId.Split('_');
				if (array.Length == 2 && array[1] == "0")
				{
					num++;
					if (num > 4)
					{
						num = 4;
					}
					return num;
				}
			}
		}
		return num;
	}

	public int GetTownTier()
	{
		return townTier;
	}

	public void UpgradeTownTier()
	{
		int num = 0;
		if (Globals.Instance.GetRequirementData("_tier4") != null && PlayerHasRequirement(Globals.Instance.GetRequirementData("_tier4")))
		{
			num = 4;
		}
		else if (Globals.Instance.GetRequirementData("_tier3") != null && PlayerHasRequirement(Globals.Instance.GetRequirementData("_tier3")))
		{
			num = 3;
		}
		else if (Globals.Instance.GetRequirementData("_tier2") != null && PlayerHasRequirement(Globals.Instance.GetRequirementData("_tier2")))
		{
			num = 2;
		}
		else if (Globals.Instance.GetRequirementData("_tier1") != null && PlayerHasRequirement(Globals.Instance.GetRequirementData("_tier1")))
		{
			num = 1;
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("UpgradeTownTier " + num);
		}
		SetTownTier(num);
	}

	private void InitGame()
	{
		combatResigned = false;
		int num = 300;
		int num2 = 300;
		if (GameManager.Instance.IsObeliskChallenge())
		{
			num = (num2 = 200);
			if (IsChallengeTraitActive("wealthyheroes"))
			{
				num = (num2 = 1200);
			}
		}
		else if (GameManager.Instance.IsGameAdventure() && ngPlus > 0)
		{
			if (ngPlus == 3)
			{
				num = (num2 = 3200);
			}
			else if (ngPlus == 4)
			{
				num = (num2 = 2800);
			}
			else if (ngPlus == 5)
			{
				num = (num2 = 2400);
			}
			else if (ngPlus == 6)
			{
				num = (num2 = 2000);
			}
			else if (ngPlus == 7)
			{
				num = (num2 = 1600);
			}
			else if (ngPlus == 8)
			{
				num = (num2 = 1200);
			}
			else if (ngPlus == 9)
			{
				num = (num2 = 1000);
			}
		}
		for (int i = 0; i < 4; i++)
		{
			if (teamAtO[i] != null)
			{
				num += PlayerManager.Instance.GetPerkCurrency(teamAtO[i].SubclassName);
				num2 += PlayerManager.Instance.GetPerkShards(teamAtO[i].SubclassName);
			}
		}
		num += sandbox_additionalGold;
		num2 += sandbox_additionalShards;
		GivePlayer(0, num, "", "", anim: false);
		GivePlayer(1, num2, "", "", anim: false);
	}

	private void InitGameMP()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("InitGameMP gameid->" + gameId);
		}
		NetworkManager.Instance.PlayerPositionListArray();
		for (int i = 0; i < NetworkManager.Instance.GetNumPlayers(); i++)
		{
			string playerNickPosition = NetworkManager.Instance.GetPlayerNickPosition(i);
			int num = 0;
			int num2 = 0;
			for (int j = 0; j < 4; j++)
			{
				if (!(NetworkManager.Instance.PlayerHeroPositionOwner[j] == playerNickPosition))
				{
					continue;
				}
				int num3 = 75;
				int num4 = 75;
				if (GameManager.Instance.IsObeliskChallenge())
				{
					num3 = (num4 = 50);
					if (IsChallengeTraitActive("wealthyheroes"))
					{
						num3 = (num4 = 300);
					}
				}
				else if (GameManager.Instance.IsGameAdventure() && ngPlus > 0)
				{
					if (ngPlus == 3)
					{
						num3 = (num4 = 800);
					}
					else if (ngPlus == 4)
					{
						num3 = (num4 = 700);
					}
					else if (ngPlus == 5)
					{
						num3 = (num4 = 600);
					}
					else if (ngPlus == 6)
					{
						num3 = (num4 = 500);
					}
					else if (ngPlus == 7)
					{
						num3 = (num4 = 400);
					}
					else if (ngPlus == 8)
					{
						num3 = (num4 = 300);
					}
					else if (ngPlus == 9)
					{
						num3 = (num4 = 250);
					}
				}
				num += num3;
				num2 += num4;
				if (teamAtO[j] != null && teamAtO[j].HeroData != null)
				{
					num += PlayerManager.Instance.GetPerkCurrency(teamAtO[j].SubclassName);
					num2 += PlayerManager.Instance.GetPerkShards(teamAtO[j].SubclassName);
				}
			}
			num += sandbox_additionalGold;
			num2 += sandbox_additionalShards;
			GivePlayer(0, num, playerNickPosition, "", anim: false);
			GivePlayer(1, num2, playerNickPosition, "", anim: false);
		}
	}

	public void SetGameId(string _gameId = "")
	{
		if (_gameId == "")
		{
			if (GameManager.Instance.IsWeeklyChallenge())
			{
				gameId = GetWeeklySeed();
			}
			else
			{
				gameId = Functions.RandomStringSafe(7f).ToUpper();
			}
			if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
			{
				photonView.RPC("NET_SetGameId", RpcTarget.Others, gameId);
			}
		}
		else
		{
			gameId = _gameId.ToUpper();
		}
	}

	[PunRPC]
	private void NET_SetGameId(string _gameId)
	{
		SetGameId(_gameId);
	}

	public void SetGameUniqueId(string _gameUniqueId = "")
	{
		if (_gameUniqueId == "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			string value = DateTime.Now.ToString("ddMMyyHHmm");
			stringBuilder.Append(gameId);
			stringBuilder.Append("_");
			stringBuilder.Append(value);
			stringBuilder.Append("_");
			stringBuilder.Append(Functions.RandomStringSafe(6f).ToUpper());
			gameUniqueId = stringBuilder.ToString();
			stringBuilder = null;
		}
		else
		{
			gameUniqueId = _gameUniqueId;
		}
		if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
		{
			photonView.RPC("NET_SetGameUniqueId", RpcTarget.Others, gameUniqueId);
		}
	}

	[PunRPC]
	private void NET_SetGameUniqueId(string _gameUniqueId)
	{
		SetGameUniqueId(_gameUniqueId);
	}

	public void SetWeeklyRandomState()
	{
		UnityEngine.Random.InitState(("WeeklyChallengeWeek" + Functions.GetCurrentWeeklyWeek()).GetDeterministicHashCode());
	}

	private string GetWeeklySeed()
	{
		SetWeeklyRandomState();
		return Functions.RandomStringSafe(7f).ToUpper();
	}

	public void CleanGameId()
	{
		gameId = "";
		gameUniqueId = "";
	}

	public string GetGameId()
	{
		return gameId;
	}

	public string GetGameUniqueId()
	{
		return gameUniqueId;
	}

	public void SaveCraftedCard(int index, string cardId)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("SaveCraftedCard ->" + cardId);
		}
		if (craftedCards == null || craftedCards.Count == 0)
		{
			craftedCards = new List<Dictionary<string, int>>();
			for (int i = 0; i < 4; i++)
			{
				craftedCards.Add(new Dictionary<string, int>());
			}
		}
		if (craftedCards[index] == null)
		{
			craftedCards[index] = new Dictionary<string, int>();
		}
		if (craftedCards[index].ContainsKey(cardId))
		{
			craftedCards[index][cardId]++;
		}
		else
		{
			craftedCards[index].Add(cardId, 1);
		}
		if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
		{
			photonView.RPC("NET_SaveCraftedCard", RpcTarget.MasterClient, index, cardId);
		}
		if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
		{
			SaveGame();
		}
	}

	[PunRPC]
	public void NET_SaveCraftedCard(int index, string cardId)
	{
		SaveCraftedCard(index, cardId);
	}

	public int HowManyCrafted(int index, string cardId)
	{
		if (craftedCards == null || index >= craftedCards.Count || craftedCards[index] == null)
		{
			return 0;
		}
		if (craftedCards[index].ContainsKey(cardId))
		{
			return craftedCards[index][cardId];
		}
		return 0;
	}

	public void SideBarRefresh()
	{
		if ((bool)MapManager.Instance)
		{
			MapManager.Instance.sideCharacters.Refresh();
		}
		else if ((bool)TownManager.Instance)
		{
			TownManager.Instance.sideCharacters.Refresh();
		}
	}

	public void ReDrawLevel()
	{
		if ((bool)MapManager.Instance)
		{
			MapManager.Instance.characterWindow.ReDrawLevel();
		}
		else if ((bool)TownManager.Instance)
		{
			TownManager.Instance.characterWindow.ReDrawLevel();
		}
	}

	public void SideBarRefreshCards(int heroIndex = 0)
	{
		if ((bool)MapManager.Instance)
		{
			MapManager.Instance.sideCharacters.RefreshCards(heroIndex);
		}
		else if ((bool)TownManager.Instance)
		{
			TownManager.Instance.sideCharacters.RefreshCards(heroIndex);
		}
	}

	public void SideBarCharacterClicked(int characterIndex)
	{
		if ((bool)CardCraftManager.Instance)
		{
			if (CardCraftManager.Instance.craftType == 3)
			{
				return;
			}
			if (!(MapManager.Instance != null) || !MapManager.Instance.characterWindow.IsActive())
			{
				CardCraftManager.Instance.SelectCharacter(characterIndex);
			}
		}
		if ((bool)MapManager.Instance)
		{
			MapManager.Instance.sideCharacters.SetActive(characterIndex);
		}
		else if ((bool)TownManager.Instance)
		{
			TownManager.Instance.sideCharacters.SetActive(characterIndex);
		}
		else if ((bool)MatchManager.Instance)
		{
			MatchManager.Instance.sideCharacters.SetActive(characterIndex);
		}
		else if ((bool)ChallengeSelectionManager.Instance)
		{
			ChallengeSelectionManager.Instance.sideCharacters.SetActive(characterIndex);
		}
	}

	public int GetPlayerGold()
	{
		return playerGold;
	}

	public void SetPlayerGold(int _playerGold)
	{
		playerGold = _playerGold;
		RefreshQuantities(0, anim: false);
	}

	public int GetPlayerDust()
	{
		return playerDust;
	}

	public void SetPlayerDust(int _playerDust)
	{
		playerDust = _playerDust;
		RefreshQuantities(1, anim: false);
	}

	public Dictionary<string, int> GetMpPlayersGold()
	{
		return mpPlayersGold;
	}

	public void SetMpPlayersGold(Dictionary<string, int> _mpPlayersGold)
	{
		mpPlayersGold = _mpPlayersGold;
	}

	public Dictionary<string, int> GetMpPlayersDust()
	{
		return mpPlayersDust;
	}

	public void SetMpPlayersDust(Dictionary<string, int> _mpPlayersDust)
	{
		mpPlayersDust = _mpPlayersDust;
	}

	public void RefreshQuantities(int type, bool anim = true)
	{
		switch (type)
		{
		case 0:
			PlayerUIManager.Instance.SetGold(anim);
			break;
		case 1:
			PlayerUIManager.Instance.SetDust(anim);
			break;
		}
		RefreshScreens();
	}

	private void RefreshScreens()
	{
		if (CardCraftManager.Instance != null)
		{
			CardCraftManager.Instance.RefreshCardPrices();
		}
	}

	public bool IsFirstGame()
	{
		if (GameManager.Instance.CheatMode && GameManager.Instance.SkipTutorial)
		{
			return false;
		}
		if (IsCombatTool)
		{
			return false;
		}
		if (GameManager.Instance.IsThisAProfile() && GameManager.Instance.TutorialWatched("firstTurnEnergy"))
		{
			return false;
		}
		if (PlayerManager.Instance.MonstersKilled < 1 && PlayerManager.Instance.GetHighestCharacterRank() == 0)
		{
			return true;
		}
		return false;
	}

	public void AskGivePlayerToPlayer(int type, int quantity, string to, string from)
	{
		photonView.RPC("NET_MASTERGivePlayer", RpcTarget.MasterClient, type, quantity, to, from, true, true);
	}

	public void AskForGold(string nick, int quantity)
	{
		photonView.RPC("NET_MASTERGivePlayer", RpcTarget.MasterClient, 0, quantity, nick, "", true, true);
	}

	public void AskForDust(string nick, int quantity)
	{
		photonView.RPC("NET_MASTERGivePlayer", RpcTarget.MasterClient, 1, quantity, nick, "", true, true);
	}

	[PunRPC]
	public void NET_MASTERGivePlayer(int type, int quantity, string to, string from, bool anim = true, bool save = false)
	{
		GivePlayer(type, quantity, to, from, anim, save);
	}

	public void GivePlayer(int type, int quantity, string to = "", string from = "", bool anim = true, bool save = false)
	{
		if (mpPlayersGold == null)
		{
			return;
		}
		if (type == 0 && quantity > 0)
		{
			totalGoldGained += quantity;
		}
		else if (type == 1 && quantity > 0)
		{
			totalDustGained += quantity;
		}
		if (!GameManager.Instance.IsMultiplayer())
		{
			if (type == 0)
			{
				playerGold += quantity;
				if (playerGold < 0)
				{
					playerGold = 0;
				}
				mpPlayersGold[NetworkManager.Instance.GetPlayerNick()] = playerGold;
				RefreshQuantities(0, anim);
			}
			if (type == 1)
			{
				playerDust += quantity;
				if (playerDust < 0)
				{
					playerDust = 0;
				}
				mpPlayersDust[NetworkManager.Instance.GetPlayerNick()] = playerDust;
				RefreshQuantities(1, anim);
			}
			if (type == 2)
			{
				PlayerManager.Instance.GainSupply(quantity);
			}
		}
		else
		{
			if (!NetworkManager.Instance.IsMaster())
			{
				return;
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("GIVE " + type + " " + quantity + " TO " + to);
			}
			switch (type)
			{
			case 0:
				if (from != "")
				{
					if (!mpPlayersGold.ContainsKey(from))
					{
						mpPlayersGold.Add(from, 0);
						return;
					}
					if (mpPlayersGold[from] < quantity)
					{
						quantity = mpPlayersGold[from];
					}
					mpPlayersGold[from] -= quantity;
					if (mpPlayersGold[from] < 0)
					{
						mpPlayersGold[from] = 0;
					}
				}
				if (to == "")
				{
					to = NetworkManager.Instance.GetPlayerNick();
				}
				if (mpPlayersGold.ContainsKey(to))
				{
					mpPlayersGold[to] += quantity;
				}
				else
				{
					mpPlayersGold.Add(to, quantity);
				}
				if (mpPlayersGold[to] < 0)
				{
					mpPlayersGold[to] = 0;
				}
				ShareQuantities(0, anim);
				if (mpPlayersGold != null && mpPlayersGold.ContainsKey(from) && Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Gold from ->" + from + " = " + mpPlayersGold[from]);
				}
				break;
			case 1:
				if (from != "")
				{
					if (!mpPlayersDust.ContainsKey(from))
					{
						mpPlayersDust.Add(from, 0);
						return;
					}
					if (mpPlayersDust[from] < quantity)
					{
						quantity = mpPlayersDust[from];
					}
					mpPlayersDust[from] -= quantity;
					if (mpPlayersDust[from] < 0)
					{
						mpPlayersDust[from] = 0;
					}
				}
				if (to == "")
				{
					to = NetworkManager.Instance.GetPlayerNick();
				}
				if (mpPlayersDust.ContainsKey(to))
				{
					mpPlayersDust[to] += quantity;
				}
				else
				{
					mpPlayersDust.Add(to, quantity);
				}
				if (mpPlayersDust[to] < 0)
				{
					mpPlayersDust[to] = 0;
				}
				ShareQuantities(1, anim);
				break;
			case 2:
				photonView.RPC("NET_GainSupply", RpcTarget.All, to, quantity);
				break;
			}
			if (save)
			{
				SaveGame();
			}
		}
	}

	[PunRPC]
	private void NET_GainSupply(string nickname, int quantity)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD(nickname + "== " + NetworkManager.Instance.GetPlayerNick());
		}
		if (nickname == NetworkManager.Instance.GetPlayerNick())
		{
			PlayerManager.Instance.GainSupply(quantity);
		}
	}

	public void SellSupply(int _quantity)
	{
		_ = PlayerManager.Instance.SupplyActual;
		if (_quantity > 0 && _quantity <= PlayerManager.Instance.SupplyActual)
		{
			if (!GameManager.Instance.IsMultiplayer())
			{
				GivePlayer(0, _quantity * 100);
				GivePlayer(1, _quantity * 100);
			}
			else
			{
				AskForGold(NetworkManager.Instance.GetPlayerNick(), _quantity * 100);
				AskForDust(NetworkManager.Instance.GetPlayerNick(), _quantity * 100);
			}
			PlayerManager.Instance.SpendSupply(_quantity);
			SaveManager.SavePlayerData();
		}
	}

	public void DistributeGoldDustBetweenHeroes()
	{
		for (int i = 0; i < NetworkManager.Instance.GetNumPlayers(); i++)
		{
			string playerNickPosition = NetworkManager.Instance.GetPlayerNickPosition(i);
			int num = 0;
			for (int j = 0; j < 4; j++)
			{
				if (NetworkManager.Instance.PlayerHeroPositionOwner[j] == playerNickPosition)
				{
					num++;
				}
			}
			float heroGold = 0f;
			float heroDust = 0f;
			if (mpPlayersGold.ContainsKey(playerNickPosition))
			{
				heroGold = (float)mpPlayersGold[playerNickPosition] / (float)num;
			}
			if (mpPlayersDust.ContainsKey(playerNickPosition))
			{
				heroDust = (float)mpPlayersDust[playerNickPosition] / (float)num;
			}
			for (int k = 0; k < 4; k++)
			{
				if (NetworkManager.Instance.PlayerHeroPositionOwner[k] == playerNickPosition)
				{
					teamAtO[k].HeroGold = heroGold;
					teamAtO[k].HeroDust = heroDust;
				}
			}
		}
	}

	private void ShareQuantities(int type, bool anim = true)
	{
		string[] array;
		int[] array2;
		if (type == 0)
		{
			array = new string[mpPlayersGold.Count];
			mpPlayersGold.Keys.CopyTo(array, 0);
			array2 = new int[mpPlayersGold.Count];
			mpPlayersGold.Values.CopyTo(array2, 0);
		}
		else
		{
			array = new string[mpPlayersDust.Count];
			mpPlayersDust.Keys.CopyTo(array, 0);
			array2 = new int[mpPlayersDust.Count];
			mpPlayersDust.Values.CopyTo(array2, 0);
		}
		string text = JsonHelper.ToJson(array);
		string text2 = JsonHelper.ToJson(array2);
		photonView.RPC("NET_ShareGoldDustDict", RpcTarget.All, type, text, text2, anim, totalGoldGained, totalDustGained);
	}

	[PunRPC]
	private void NET_ShareGoldDustDict(int type, string _keys, string _values, bool anim, int _totalGoldGained, int _totalDustGained)
	{
		string[] array = JsonHelper.FromJson<string>(_keys);
		int[] array2 = JsonHelper.FromJson<int>(_values);
		string playerNick = NetworkManager.Instance.GetPlayerNick();
		switch (type)
		{
		case 0:
		{
			mpPlayersGold = new Dictionary<string, int>();
			for (int j = 0; j < array.Length; j++)
			{
				mpPlayersGold.Add(array[j], array2[j]);
				if (array[j] == playerNick)
				{
					playerGold = array2[j];
				}
			}
			break;
		}
		case 1:
		{
			mpPlayersDust = new Dictionary<string, int>();
			for (int i = 0; i < array.Length; i++)
			{
				mpPlayersDust.Add(array[i], array2[i]);
				if (array[i] == playerNick)
				{
					playerDust = array2[i];
				}
			}
			break;
		}
		}
		totalGoldGained = _totalGoldGained;
		totalDustGained = _totalDustGained;
		RefreshQuantities(type, anim);
	}

	public void PayGold(int goldCost, bool paySingle = true, bool isReroll = false)
	{
		if (!GameManager.Instance.IsMultiplayer())
		{
			GivePlayer(0, -goldCost);
		}
		else if (paySingle)
		{
			if (NetworkManager.Instance.IsMaster())
			{
				if (!isReroll)
				{
					GivePlayer(0, -goldCost);
				}
				else
				{
					GivePlayer(0, -goldCost, "", "", anim: true, save: true);
				}
				ShareQuantities(0);
			}
			else if (!isReroll)
			{
				photonView.RPC("NET_MASTERGivePlayer", RpcTarget.MasterClient, 0, -goldCost, NetworkManager.Instance.GetPlayerNick(), "", true, false);
			}
			else
			{
				photonView.RPC("NET_MASTERGivePlayer", RpcTarget.MasterClient, 0, -goldCost, NetworkManager.Instance.GetPlayerNick(), "", true, true);
			}
		}
		else
		{
			if (!NetworkManager.Instance.IsMaster())
			{
				return;
			}
			int num = 0;
			int num2 = goldCost / 4;
			for (int i = 0; i < 4; i++)
			{
				if (GetHero(i) != null && GetHero(i).HeroData != null)
				{
					int num3 = num2;
					if (num3 > mpPlayersGold[GetHero(i).Owner])
					{
						num3 = mpPlayersGold[GetHero(i).Owner];
					}
					mpPlayersGold[GetHero(i).Owner] -= num3;
					num += num3;
				}
			}
			int num4 = 0;
			while (num < goldCost)
			{
				if (mpPlayersGold[NetworkManager.Instance.PlayerPositionList[num4]] > 0)
				{
					mpPlayersGold[NetworkManager.Instance.PlayerPositionList[num4]]--;
					num++;
				}
				num4++;
				if (num4 >= mpPlayersGold.Count)
				{
					num4 = 0;
				}
			}
			ShareQuantities(0);
		}
	}

	public int GetTotalPlayersGold()
	{
		int num = 0;
		foreach (KeyValuePair<string, int> item in mpPlayersGold)
		{
			num += item.Value;
		}
		return num;
	}

	public int GetTotalPlayersDust()
	{
		int num = 0;
		foreach (KeyValuePair<string, int> item in mpPlayersDust)
		{
			num += item.Value;
		}
		return num;
	}

	public void PayDust(int dustCost, bool paySingle = true)
	{
		if (!GameManager.Instance.IsMultiplayer())
		{
			GivePlayer(1, -dustCost);
		}
		else if (paySingle)
		{
			if (NetworkManager.Instance.IsMaster())
			{
				GivePlayer(1, -dustCost);
				ShareQuantities(1);
				return;
			}
			photonView.RPC("NET_MASTERGivePlayer", RpcTarget.MasterClient, 1, -dustCost, NetworkManager.Instance.GetPlayerNick(), "", true, false);
		}
		else
		{
			if (!NetworkManager.Instance.IsMaster())
			{
				return;
			}
			int num = 0;
			int num2 = num / 4;
			for (int i = 0; i < 4; i++)
			{
				int num3 = num2;
				if (num3 > mpPlayersDust[GetHero(i).Owner])
				{
					num3 = mpPlayersDust[GetHero(i).Owner];
				}
				mpPlayersDust[GetHero(i).Owner] -= num3;
				num += num3;
			}
			int num4 = 0;
			while (num < dustCost)
			{
				if (mpPlayersDust[NetworkManager.Instance.PlayerPositionList[num4]] > 0)
				{
					mpPlayersDust[NetworkManager.Instance.PlayerPositionList[num4]]--;
					num++;
				}
				num4++;
				if (num4 >= mpPlayersDust.Count)
				{
					num4 = 0;
				}
			}
			ShareQuantities(1);
		}
	}

	private void CreateTeam()
	{
		teamAtO = new Hero[4];
	}

	private void CreateTeamBackup()
	{
		teamAtOBackup = new Hero[4];
	}

	private void CreateTeamNPC()
	{
		teamNPCAtO = new string[4];
	}

	public void ClearTeamNPC()
	{
		teamNPCAtO = null;
	}

	public Hero[] GetTeam()
	{
		return teamAtO;
	}

	public Hero[] GetTeamBackup()
	{
		return teamAtOBackup;
	}

	public string[] GetTeamNPC()
	{
		return teamNPCAtO;
	}

	public Hero GetHero(int index)
	{
		if (index >= 0 && index < 5 && teamAtO != null && teamAtO.Length != 0)
		{
			return teamAtO[index];
		}
		return null;
	}

	public int GetTeamTotalHp()
	{
		int num = 0;
		for (int i = 0; i < teamAtO.Length; i++)
		{
			num += teamAtO[i].GetHp();
		}
		return num;
	}

	public int GetTeamTotalExperience()
	{
		int num = 0;
		for (int i = 0; i < teamAtO.Length; i++)
		{
			num += teamAtO[i].Experience;
		}
		return num;
	}

	public void SetTeamFromArray(string[] _team)
	{
		CreateTeam();
		for (int i = 0; i < _team.Length; i++)
		{
			if (_team[i] == null || !(_team[i] != ""))
			{
				continue;
			}
			teamAtO[i] = GameManager.Instance.CreateHero(_team[i].ToLower());
			if (teamAtO[i] != null && !(teamAtO[i].HeroData == null) && HeroSelectionManager.Instance != null)
			{
				teamAtO[i].PerkRank = HeroSelectionManager.Instance.heroSelectionDictionary[teamAtO[i].SubclassName].rankTMHidden;
				teamAtO[i].SkinUsed = HeroSelectionManager.Instance.playerHeroSkinsDict[teamAtO[i].SubclassName];
				teamAtO[i].CardbackUsed = HeroSelectionManager.Instance.playerHeroCardbackDict[teamAtO[i].SubclassName];
				SetSkinIntoSubclassData(teamAtO[i].SubclassName, teamAtO[i].SkinUsed);
				teamAtO[i].RedoSubclassFromSkin();
				string subclassName = teamAtO[i].SubclassName;
				if (heroPerks.ContainsKey(subclassName))
				{
					teamAtO[i].PerkList = heroPerks[subclassName];
				}
				if (teamAtO[i].Energy < 0)
				{
					teamAtO[i].Energy = 0;
				}
				if (teamAtO[i].EnergyCurrent < 0)
				{
					teamAtO[i].EnergyCurrent = 0;
				}
			}
		}
	}

	public void SetTeamFromTeamHero(Hero[] _team)
	{
		CreateTeam();
		for (int i = 0; i < _team.Length; i++)
		{
			teamAtO[i] = _team[i];
		}
	}

	public bool TeamHaveTrait(string _id)
	{
		_id = _id.ToLower();
		for (int i = 0; i < teamAtO.Length; i++)
		{
			if (teamAtO[i].Traits == null)
			{
				continue;
			}
			for (int j = 0; j < teamAtO[i].Traits.Length; j++)
			{
				if (teamAtO[i].Traits[j] == _id)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool AliveTeamHaveTrait(string _id)
	{
		_id = _id.ToLower();
		for (int i = 0; i < teamAtO.Length; i++)
		{
			if (!teamAtO[i].Alive || teamAtO[i].Traits == null)
			{
				continue;
			}
			for (int j = 0; j < teamAtO[i].Traits.Length; j++)
			{
				if (teamAtO[i].Traits[j] == _id)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool CharacterHaveTrait(string _subclassId, string _id)
	{
		_id = _id.ToLower();
		for (int i = 0; i < teamAtO.Length; i++)
		{
			if (!(teamAtO[i].SubclassName == _subclassId))
			{
				continue;
			}
			for (int j = 0; j < teamAtO[i].Traits.Length; j++)
			{
				if (teamAtO[i].Traits[j] == _id)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool CharacterHavePerk(string _subclassId, string _id)
	{
		_id = _id.ToLower();
		for (int i = 0; i < teamAtO.Length; i++)
		{
			if (!(teamAtO[i].SubclassName == _subclassId) || teamAtO[i].PerkList == null)
			{
				continue;
			}
			for (int j = 0; j < teamAtO[i].PerkList.Count; j++)
			{
				if (teamAtO[i].PerkList[j] == _id)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool TeamHavePerk(string perkId)
	{
		return HavePerk(null, perkId);
	}

	public bool CharacterHavePerk(Character character, string perkId)
	{
		return HavePerk(character, perkId);
	}

	private bool HavePerk(Character onlyThisCharacter, string perkId)
	{
		if (teamAtO == null || string.IsNullOrWhiteSpace(perkId))
		{
			return false;
		}
		Hero[] array = teamAtO;
		foreach (Hero hero in array)
		{
			if (hero == null || (onlyThisCharacter != null && hero != onlyThisCharacter))
			{
				continue;
			}
			List<string> perkList = hero.PerkList;
			if (perkList == null)
			{
				continue;
			}
			foreach (string item in perkList)
			{
				if (string.Equals(item, perkId, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool CharacterHaveItem(string _subclassId, string _id)
	{
		_id = _id.ToLower();
		for (int i = 0; i < teamAtO.Length; i++)
		{
			if (teamAtO[i].SubclassName == _subclassId && teamAtO[i].HaveItem(_id))
			{
				return true;
			}
		}
		return false;
	}

	public bool TeamHaveItem(string _id, int _itemSlot = -1, bool _checkRareToo = false)
	{
		_id = _id.ToLower();
		for (int i = 0; i < teamAtO.Length; i++)
		{
			if (teamAtO[i] != null && teamAtO[i].HaveItem(_id, _itemSlot, _checkRareToo))
			{
				return true;
			}
		}
		return false;
	}

	public AuraCurseData GlobalAuraCurseModifyDamage(AuraCurseData _AC, Enums.DamageType _DT, int _value, int _perStack, int _percent)
	{
		if (_AC.AuraDamageType == _DT)
		{
			_AC.AuraDamageIncreasedTotal += _value;
			_AC.AuraDamageIncreasedPerStack += _perStack;
			_AC.AuraDamageIncreasedPercent += _percent;
		}
		else if (_AC.AuraDamageType2 == _DT)
		{
			_AC.AuraDamageIncreasedTotal2 += _value;
			_AC.AuraDamageIncreasedPerStack2 += _perStack;
			_AC.AuraDamageIncreasedPercent2 += _percent;
		}
		else if (_AC.AuraDamageType3 == _DT)
		{
			_AC.AuraDamageIncreasedTotal3 += _value;
			_AC.AuraDamageIncreasedPerStack3 += _perStack;
			_AC.AuraDamageIncreasedPercent3 += _percent;
		}
		else if (_AC.AuraDamageType4 == _DT)
		{
			_AC.AuraDamageIncreasedTotal4 += _value;
			_AC.AuraDamageIncreasedPerStack4 += _perStack;
			_AC.AuraDamageIncreasedPercent4 += _percent;
		}
		else if (_AC.AuraDamageType == Enums.DamageType.None)
		{
			_AC.AuraDamageType = _DT;
			_AC.AuraDamageIncreasedTotal += _value;
			_AC.AuraDamageIncreasedPerStack += _perStack;
			_AC.AuraDamageIncreasedPercent += _percent;
		}
		else if (_AC.AuraDamageType2 == Enums.DamageType.None)
		{
			_AC.AuraDamageType2 = _DT;
			_AC.AuraDamageIncreasedTotal2 += _value;
			_AC.AuraDamageIncreasedPerStack2 += _perStack;
			_AC.AuraDamageIncreasedPercent2 += _percent;
		}
		else if (_AC.AuraDamageType3 == Enums.DamageType.None)
		{
			_AC.AuraDamageType3 = _DT;
			_AC.AuraDamageIncreasedTotal3 += _value;
			_AC.AuraDamageIncreasedPerStack3 += _perStack;
			_AC.AuraDamageIncreasedPercent3 += _percent;
		}
		else if (_AC.AuraDamageType4 == Enums.DamageType.None)
		{
			_AC.AuraDamageType4 = _DT;
			_AC.AuraDamageIncreasedTotal4 += _value;
			_AC.AuraDamageIncreasedPerStack4 += _perStack;
			_AC.AuraDamageIncreasedPercent4 += _percent;
		}
		for (int i = 0; i < _AC.AuraDamageConditionalBonuses.Length; i++)
		{
			if (_AC.AuraDamageConditionalBonuses[i].AuraDamageType == Enums.DamageType.None || _AC.AuraDamageConditionalBonuses[i].AuraDamageType == _DT)
			{
				_AC.AuraDamageConditionalBonuses[i].AuraDamageType = _DT;
				_AC.AuraDamageConditionalBonuses[i].AuraDamageIncreasedTotal += _value;
				_AC.AuraDamageConditionalBonuses[i].AuraDamageIncreasedPerStack += _perStack;
				_AC.AuraDamageConditionalBonuses[i].AuraDamageIncreasedPercent += _percent;
			}
		}
		return _AC;
	}

	public AuraCurseData GlobalAuraCurseModifyResist(AuraCurseData _AC, Enums.DamageType _DT, int _value, float _valuePerStack)
	{
		if (_AC.ResistModified == _DT)
		{
			_AC.ResistModifiedValue = _value;
			_AC.ResistModifiedPercentagePerStack += _valuePerStack;
		}
		else if (_AC.ResistModified2 == _DT)
		{
			_AC.ResistModifiedValue2 = _value;
			_AC.ResistModifiedPercentagePerStack2 += _valuePerStack;
		}
		else if (_AC.ResistModified3 == _DT)
		{
			_AC.ResistModifiedValue3 = _value;
			_AC.ResistModifiedPercentagePerStack3 += _valuePerStack;
		}
		else if (_AC.ResistModified == Enums.DamageType.None)
		{
			_AC.ResistModified = _DT;
			_AC.ResistModifiedValue = _value;
			_AC.ResistModifiedPercentagePerStack += _valuePerStack;
		}
		else if (_AC.ResistModified2 == Enums.DamageType.None)
		{
			_AC.ResistModified2 = _DT;
			_AC.ResistModifiedValue2 = _value;
			_AC.ResistModifiedPercentagePerStack2 += _valuePerStack;
		}
		else if (_AC.ResistModified3 == Enums.DamageType.None)
		{
			_AC.ResistModified3 = _DT;
			_AC.ResistModifiedValue3 = _value;
			_AC.ResistModifiedPercentagePerStack3 += _valuePerStack;
		}
		return _AC;
	}

	public void ClearCacheGlobalACModification()
	{
		cacheGlobalACModification.Clear();
	}

	public AuraCurseData GlobalAuraCurseModificationByTraitsAndItems(string _type, string _acId, Character _characterCaster, Character _characterTarget, bool forPopup = false)
	{
		if (_characterCaster == null && _characterTarget == null)
		{
			return null;
		}
		string key = "";
		useCache = false;
		if (useCache)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(_type);
			stringBuilder.Append("|");
			stringBuilder.Append(_acId);
			stringBuilder.Append("|");
			if (_characterCaster != null)
			{
				stringBuilder.Append(_characterCaster.Id);
			}
			stringBuilder.Append("|");
			if (_characterTarget != null)
			{
				stringBuilder.Append(_characterTarget.Id);
			}
			key = stringBuilder.ToString();
			stringBuilder = null;
			if (cacheGlobalACModification.ContainsKey(key))
			{
				return cacheGlobalACModification[key];
			}
		}
		AuraCurseData AC = UnityEngine.Object.Instantiate(Globals.Instance.GetAuraCurseData(_acId));
		if (AC == null)
		{
			return null;
		}
		bool flag = false;
		bool flag2 = false;
		if (_characterCaster != null && _characterCaster.IsHero)
		{
			flag = _characterCaster.IsHero;
		}
		if (_characterTarget != null && _characterTarget.IsHero)
		{
			flag2 = true;
		}
		switch (_acId)
		{
		case "bleed":
			if (_type == "set")
			{
				if (flag2)
				{
					if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkfury1c"))
					{
						AC.Preventable = false;
						AC.DamageWhenConsumedPerCharge = 1.5f;
					}
					if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkbleed2b"))
					{
						AC.Removable = false;
						AC.ConsumedAtTurnBegin = false;
						AC.ConsumedAtTurn = true;
					}
				}
				else if (TeamHavePerk("mainperkbleed2c"))
				{
					AC.Preventable = false;
					AC.ConsumedAtTurnBegin = false;
					AC.ConsumedAtTurn = true;
				}
				if (IsChallengeTraitActive("hemorrhage"))
				{
					AC.Preventable = false;
				}
			}
			else if (_type == "consume")
			{
				if (!flag && TeamHavePerk("mainperkbleed2c"))
				{
					AC.ConsumedAtTurnBegin = false;
					AC.ConsumedAtTurn = true;
				}
				if (_characterCaster != null && CharacterHavePerk(_characterCaster.SubclassName, "mainperkbleed2b"))
				{
					AC.ConsumedAtTurnBegin = false;
					AC.ConsumedAtTurn = true;
				}
				if (_characterCaster != null && CharacterHavePerk(_characterCaster.SubclassName, "mainperkfury1c"))
				{
					AC.DamageWhenConsumedPerCharge = 1.5f;
				}
				if (IsChallengeTraitActive("hemorrhage"))
				{
					AC.ConsumedAtTurnBegin = false;
					AC.ConsumedAtTurn = true;
				}
			}
			break;
		case "bless":
			if (_type == "set")
			{
				if (flag2)
				{
					if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkbless1a"))
					{
						AC.AuraDamageIncreasedPerStack = 1.5f;
						AC.HealReceivedPerStack = 0;
					}
					if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkbless1b"))
					{
						AC.HealDonePercentPerStack = 1;
					}
					if (TeamHavePerk("mainperkbless1c"))
					{
						AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Holy, 0, 0.5f);
						AC.ConsumedAtTurn = false;
						AC.AuraConsumed = 0;
					}
				}
			}
			else if (_type == "consume" && flag && TeamHavePerk("mainperkbless1c"))
			{
				AC.ConsumedAtTurn = false;
				AC.AuraConsumed = 0;
			}
			break;
		case "burn":
			if (_type == "set")
			{
				if (flag2)
				{
					if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkburn2b"))
					{
						AC.ResistModifiedPercentagePerStack = -0.3f;
						AC.CharacterStatModified = Enums.CharacterStat.Speed;
						AC.CharacterStatChargesMultiplierNeededForOne = 8;
						AC.CharacterStatModifiedValuePerStack = 1f;
					}
				}
				else
				{
					if (_characterTarget != null && TeamHavePerk("mainperkburn2c"))
					{
						AC.ResistModifiedPercentagePerStack = -0.3f;
						AC.ResistModified2 = Enums.DamageType.Cold;
						AC.ResistModifiedPercentagePerStack2 = -0.3f;
					}
					if (TeamHavePerk("mainperkburn2d"))
					{
						AC.DoubleDamageIfCursesLessThan = 3;
					}
				}
				if (IsChallengeTraitActive("extremeburning"))
				{
					AC.DoubleDamageIfCursesLessThan = 3;
				}
				if (IsChallengeTraitActive("frostfire"))
				{
					AC.ResistModifiedPercentagePerStack = -0.3f;
					AC.ResistModified2 = Enums.DamageType.Cold;
					AC.ResistModifiedPercentagePerStack2 = -0.3f;
				}
				if (flag2 && _characterTarget != null && CharacterHaveTrait(_characterTarget.SubclassName, "righteousflame"))
				{
					AC.ProduceDamageWhenConsumed = false;
					AC.DamageWhenConsumedPerCharge = 0f;
					AC.ProduceHealWhenConsumed = true;
					AC.HealWhenConsumedPerCharge = 0.3f;
				}
			}
			if (!(_type == "consume"))
			{
				break;
			}
			if (!flag)
			{
				if (TeamHavePerk("mainperkburn2c"))
				{
					AC.DamageTypeWhenConsumed = Enums.DamageType.Cold;
				}
				if (TeamHavePerk("mainperkburn2d"))
				{
					AC.DoubleDamageIfCursesLessThan = 3;
				}
			}
			if (IsChallengeTraitActive("extremeburning"))
			{
				AC.DoubleDamageIfCursesLessThan = 3;
			}
			if (flag && _characterCaster != null && CharacterHaveTrait(_characterCaster.SubclassName, "righteousflame"))
			{
				AC.ProduceDamageWhenConsumed = false;
				AC.DamageWhenConsumedPerCharge = 0f;
				AC.ProduceHealWhenConsumed = true;
				AC.HealWhenConsumedPerCharge = 0.3f;
			}
			break;
		case "chill":
			if (!(_type == "set"))
			{
				break;
			}
			if (!flag2)
			{
				if (TeamHavePerk("mainperkChill2b"))
				{
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Blunt, 0, -0.3f);
				}
				if (TeamHavePerk("mainperkChill2c"))
				{
					AC.CharacterStatChargesMultiplierNeededForOne = 4;
				}
			}
			else if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkChill2d"))
			{
				AC.CharacterStatChargesMultiplierNeededForOne = 8;
			}
			if (IsChallengeTraitActive("intensecold"))
			{
				AC.CharacterStatChargesMultiplierNeededForOne = 4;
			}
			break;
		case "courage":
			if (_type == "set" && flag2)
			{
				if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkCourage0"))
				{
					AC.Removable = false;
				}
				if (TeamHavePerk("mainperkCourage1b"))
				{
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Holy, 35, 0f);
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Shadow, 35, 0f);
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Mind, 35, 0f);
				}
				if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkCourage1c"))
				{
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Holy, 0, 7f);
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Shadow, 0, 7f);
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Mind, 0, 7f);
					AC.GainCharges = true;
					AuraCurseData auraCurseData3 = AC;
					int maxCharges = (AC.MaxMadnessCharges = 8);
					auraCurseData3.MaxCharges = maxCharges;
				}
			}
			UpdateResistanceModifiersBasedOnInfuse(_type, _characterCaster, _characterTarget, ref AC);
			break;
		case "crack":
		{
			int num13 = 0;
			if (_characterCaster != null && _type == "consume")
			{
				num13 = _characterCaster.GetAuraCharges("rust");
			}
			else if (_characterTarget != null)
			{
				num13 = _characterTarget.GetAuraCharges("rust");
			}
			if (_type == "set")
			{
				if (num13 > 0)
				{
					AC.IncreasedDirectDamageReceivedPerStack *= 1.5f;
				}
				if (num13 > 0 && !flag2 && TeamHavePerk("mainperkrust0b"))
				{
					AC.IncreasedDirectDamageReceivedPerStack /= 1.5f;
					AC.IncreasedDirectDamageReceivedPerStack *= 1f + (float)num13 * 0.2f;
				}
			}
			break;
		}
		case "dark":
			if (_type == "set")
			{
				if (!flag2)
				{
					if (TeamHaveItem("thedarkone", 0, _checkRareToo: true))
					{
						AC.ExplodeAtStacks = Globals.Instance.GetItemData("thedarkone").AuracurseCustomModValue1;
					}
					else if (TeamHaveItem("blackdeck", 0, _checkRareToo: true))
					{
						AC.ExplodeAtStacks = Globals.Instance.GetItemData("blackdeck").AuracurseCustomModValue1;
					}
					else if (TeamHaveItem("cupofdeath", 0, _checkRareToo: true))
					{
						AC.ExplodeAtStacks = Globals.Instance.GetItemData("cupofdeath").AuracurseCustomModValue1;
					}
					if (TeamHaveTrait("putrefaction"))
					{
						AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Shadow, 0, -1.5f);
					}
					if (TeamHavePerk("mainperkdark2c") || TeamHaveTrait("absolutedarkness"))
					{
						if (TeamHavePerk("mainperkdark2c") && TeamHaveTrait("absolutedarkness"))
						{
							AC.DamageWhenConsumedPerCharge += AC.DamageWhenConsumedPerCharge * 0.7f;
						}
						else
						{
							AC.DamageWhenConsumedPerCharge += AC.DamageWhenConsumedPerCharge * 0.35f;
						}
					}
				}
				else if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkdark2b"))
				{
					AC.ExplodeAtStacks = 38;
				}
			}
			else
			{
				if (!(_type == "consume"))
				{
					break;
				}
				if (!flag)
				{
					if (TeamHavePerk("mainperkdark2c") || TeamHaveTrait("absolutedarkness"))
					{
						if (TeamHavePerk("mainperkdark2c") && TeamHaveTrait("absolutedarkness"))
						{
							AC.DamageWhenConsumedPerCharge += AC.DamageWhenConsumedPerCharge * 0.7f;
						}
						else
						{
							AC.DamageWhenConsumedPerCharge += AC.DamageWhenConsumedPerCharge * 0.35f;
						}
					}
					if (TeamHavePerk("mainperkdark2d"))
					{
						AC.ProduceDamageWhenConsumed = true;
					}
				}
				if (IsChallengeTraitActive("darkestnight"))
				{
					AC.ProduceDamageWhenConsumed = true;
				}
			}
			break;
		case "leech":
			if (_type == "set" && !flag2)
			{
				if (TeamHaveItem("bloodlettersfang", 0))
				{
					AC.ExplodeAtStacks = Globals.Instance.GetItemData("bloodlettersfang").AuracurseCustomModValue1;
				}
				else if (TeamHaveItem("bloodlettersfangrare", 0))
				{
					AC.ExplodeAtStacks = Globals.Instance.GetItemData("bloodlettersfang").AuracurseCustomModValue1;
				}
				if (TeamHavePerk("mainperkleech0a"))
				{
					AC.ResistModified = Enums.DamageType.All;
					AC.ResistModifiedPercentagePerStack = -1.5f;
					AC.HealPerChargeOnExplode = 0f;
				}
				if (_characterCaster != null && TeamHavePerk("mainperkleech0c"))
				{
					AC.ACOnExplode = Globals.Instance.GetAuraCurseData("chill");
					AC.ACChargesPerStackChargeOnExplode = 1;
				}
				if (TeamHavePerk("mainperkleech0b") && 18 < AC.ExplodeAtStacks)
				{
					AC.ExplodeAtStacks = 18;
				}
			}
			break;
		case "decay":
			if (_type == "set" && !flag2)
			{
				if (TeamHavePerk("mainperkdecay1b"))
				{
					AC.HealReceivedPercent = -75;
					AC.Removable = false;
				}
				if (TeamHavePerk("mainperkdecay1c"))
				{
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Shadow, 0, -8f);
				}
			}
			break;
		case "fast":
		{
			if (_type == "set")
			{
				if (flag2 && _characterTarget != null)
				{
					if (CharacterHaveItem(_characterTarget.SubclassName, "rocketboots") || CharacterHaveItem(_characterTarget.SubclassName, "rocketbootsa") || CharacterHaveItem(_characterTarget.SubclassName, "rocketbootsb") || CharacterHaveItem(_characterTarget.SubclassName, "rocketbootsrare") || CharacterHaveItem(_characterTarget.SubclassName, "turboboots") || CharacterHaveItem(_characterTarget.SubclassName, "turbobootsrare"))
					{
						AC.GainCharges = true;
						AC.ConsumeAll = true;
					}
					if (CharacterHaveTrait(_characterTarget.SubclassName, "greasedgears"))
					{
						AC.AuraDamageType = Enums.DamageType.All;
						AC.AuraDamageIncreasedPerStack = 1f;
					}
				}
			}
			else if (_type == "consume" && flag && _characterCaster != null && (CharacterHaveItem(_characterCaster.SubclassName, "rocketboots") || CharacterHaveItem(_characterCaster.SubclassName, "rocketbootsa") || CharacterHaveItem(_characterCaster.SubclassName, "rocketbootsb") || CharacterHaveItem(_characterCaster.SubclassName, "rocketbootsrare") || CharacterHaveItem(_characterCaster.SubclassName, "turboboots") || CharacterHaveItem(_characterCaster.SubclassName, "turbobootsrare")))
			{
				AC.ConsumeAll = true;
			}
			int num12 = 0;
			if (_characterCaster != null && _type == "consume")
			{
				num12 = _characterCaster.GetAuraCharges("rust");
			}
			else if (_characterTarget != null)
			{
				num12 = _characterTarget.GetAuraCharges("rust");
			}
			if (num12 > 0)
			{
				AC.CharacterStatModifiedValuePerStack = Functions.FuncRoundToInt(AC.CharacterStatModifiedValuePerStack * 0.5f);
			}
			break;
		}
		case "fortify":
			if (_type == "set")
			{
				if (flag2)
				{
					if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkfortify1a"))
					{
						AC.AuraDamageType = Enums.DamageType.Blunt;
						AC.AuraDamageIncreasedPerStack = 1f;
						AC.AuraDamageType2 = Enums.DamageType.Fire;
						AC.AuraDamageIncreasedPerStack2 = 1f;
						AC.GainCharges = true;
						AuraCurseData auraCurseData8 = AC;
						int maxCharges = (AC.MaxMadnessCharges = 50);
						auraCurseData8.MaxCharges = maxCharges;
					}
					if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkfortify1b"))
					{
						AC.Removable = false;
					}
				}
			}
			else if (_type == "consume" && flag && TeamHavePerk("mainperkfortify1c"))
			{
				AC.ConsumeAll = true;
			}
			break;
		case "fury":
			AC.AuraDamageIncreasedPercentPerStack = 3f;
			if (_type == "set")
			{
				if (flag2)
				{
					if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkfury1b"))
					{
						AC.AuraDamageIncreasedPercentPerStack = 1f;
						AC.GainAuraCurseConsumption = null;
					}
					if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkfury1c"))
					{
						AC.AuraDamageIncreasedPercentPerStack = 5f;
					}
					if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkbleed2b"))
					{
						AuraCurseData auraCurseData2 = AC;
						int maxCharges = (AC.MaxMadnessCharges = 25);
						auraCurseData2.MaxCharges = maxCharges;
					}
				}
				if (IsChallengeTraitActive("containedfury"))
				{
					AC.AuraDamageIncreasedPercentPerStack = 2f;
					AC.GainAuraCurseConsumption = null;
				}
			}
			else if (_type == "consume")
			{
				if (flag && _characterCaster != null && CharacterHavePerk(_characterCaster.SubclassName, "mainperkfury1b"))
				{
					AC.GainAuraCurseConsumption = null;
				}
				if (IsChallengeTraitActive("containedfury"))
				{
					AC.GainAuraCurseConsumption = null;
				}
			}
			if (doubleFuryEffect && flag2)
			{
				AC.AuraDamageIncreasedPercentPerStack *= 2f;
			}
			break;
		case "insane":
			if (_type == "set")
			{
				if (flag2)
				{
					if (TeamHavePerk("mainperkinsane2c"))
					{
						AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Mind, 0, 0.5f);
						AC.AuraDamageIncreasedPercentPerStack = -0.3f;
					}
				}
				else if (TeamHavePerk("mainperkinsane2b"))
				{
					AC.CharacterStatModified = Enums.CharacterStat.Hp;
					AC.CharacterStatModifiedValuePerStack = -2f;
				}
			}
			else if (!(_type == "consume"))
			{
			}
			break;
		case "inspire":
			if (_type == "set" && flag2)
			{
				if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkInspire0b"))
				{
					AuraCurseData auraCurseData7 = AC;
					int maxCharges = (AC.MaxMadnessCharges = 1);
					auraCurseData7.MaxCharges = maxCharges;
					AC.CardsDrawPerStack = 2;
				}
				if (TeamHaveItem("goldenlaurel", 3, _checkRareToo: true))
				{
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.All, 0, Globals.Instance.GetItemData("goldenlaurel").AuracurseCustomModValue1);
					AC.HealReceivedPercentPerStack = Globals.Instance.GetItemData("goldenlaurel").AuracurseCustomModValue2;
				}
			}
			break;
		case "insulate":
			if (_type == "set" && flag2)
			{
				if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkInsulate0"))
				{
					AC.Removable = false;
				}
				if (TeamHavePerk("mainperkInsulate1b"))
				{
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Fire, 35, 0f);
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Cold, 35, 0f);
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Blunt, 35, 0f);
				}
				if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkInsulate1c"))
				{
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Fire, 0, 7f);
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Cold, 0, 7f);
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Lightning, 0, 7f);
					AC.GainCharges = true;
					AuraCurseData auraCurseData4 = AC;
					int maxCharges = (AC.MaxMadnessCharges = 8);
					auraCurseData4.MaxCharges = maxCharges;
				}
			}
			UpdateResistanceModifiersBasedOnInfuse(_type, _characterCaster, _characterTarget, ref AC);
			break;
		case "mark":
			if (_type == "set")
			{
				if (!flag2)
				{
					if (TeamHavePerk("mainperkmark1b"))
					{
						AC.ConsumedAtTurn = false;
						AC.AuraConsumed = 0;
					}
					if (TeamHavePerk("mainperkmark1c"))
					{
						AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Slashing, 0, -0.3f);
					}
					if (Instance.AliveTeamHaveTrait("glareofdoom"))
					{
						AC.ChargesMultiplierDescription = 2;
						AC.IncreasedDirectDamageReceivedPerStack = 2f;
					}
				}
			}
			else if (_type == "consume" && !flag && TeamHavePerk("mainperkmark1b"))
			{
				AC.ConsumedAtTurn = false;
				AC.AuraConsumed = 0;
			}
			break;
		case "poison":
		{
			if (_type == "set")
			{
				if (!flag2)
				{
					if (TeamHavePerk("mainperkpoison2b"))
					{
						AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Shadow, 0, -0.3f);
					}
					if (TeamHavePerk("mainperkpoison2c"))
					{
						AC.ConsumeAll = true;
						AC.DamageWhenConsumedPerCharge = 1.5f;
					}
				}
				if (IsChallengeTraitActive("lethalpoison"))
				{
					AC.ConsumeAll = true;
					AC.DamageWhenConsumedPerCharge = 1.5f;
				}
			}
			else if (_type == "consume")
			{
				if (!flag && TeamHavePerk("mainperkpoison2c"))
				{
					AC.ConsumeAll = true;
					AC.DamageWhenConsumedPerCharge = 1.5f;
				}
				if (IsChallengeTraitActive("lethalpoison"))
				{
					AC.ConsumeAll = true;
					AC.DamageWhenConsumedPerCharge = 1.5f;
				}
			}
			int num2 = 0;
			if (_characterCaster != null && _type == "consume")
			{
				num2 = _characterCaster.GetAuraCharges("rust");
			}
			else if (_characterTarget != null)
			{
				num2 = _characterTarget.GetAuraCharges("rust");
			}
			if (num2 > 0)
			{
				AC.DamageWhenConsumedPerCharge *= 1.5f;
				if (((_characterTarget != null && !flag2) || (_characterTarget == null && !flag)) && TeamHavePerk("mainperkrust0b"))
				{
					AC.DamageWhenConsumedPerCharge /= 1.5f;
					AC.DamageWhenConsumedPerCharge *= 1f + (float)num2 * 0.2f;
				}
				if (_type == "set" && !flag2 && TeamHavePerk("mainperkpoison2b"))
				{
					AC.ResistModifiedPercentagePerStack = -0.45f;
				}
			}
			break;
		}
		case "powerful":
			AC.HealDonePercentPerStack = 5;
			AC.AuraDamageIncreasedPercentPerStack = 5f;
			if (_type == "set")
			{
				if (flag2)
				{
					if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkpowerful1b"))
					{
						AC.MaxCharges += 2;
						AC.MaxMadnessCharges += 2;
						AC.AuraConsumed = 1;
					}
					if (TeamHaveTrait("valhalla"))
					{
						AC.MaxCharges += 8;
						AC.MaxMadnessCharges += 8;
					}
					if (TeamHaveItem("powercoilrare", 2))
					{
						AC.MaxCharges += Globals.Instance.GetItemData("powercoilrare").AuracurseCustomModValue1;
						AC.MaxMadnessCharges += Globals.Instance.GetItemData("powercoilrare").AuracurseCustomModValue1;
					}
					else if (TeamHaveItem("powercoil", 2))
					{
						AC.MaxCharges += Globals.Instance.GetItemData("powercoil").AuracurseCustomModValue1;
						AC.MaxMadnessCharges += Globals.Instance.GetItemData("powercoil").AuracurseCustomModValue1;
					}
					if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkpowerful1c"))
					{
						AC.AuraDamageIncreasedPercentPerStack = 8f;
						AC.HealDonePercentPerStack = 8;
						AC.MaxCharges -= 2;
						AC.MaxMadnessCharges -= 2;
						AC.ConsumeAll = true;
					}
				}
			}
			else if (_type == "consume" && flag)
			{
				if (_characterCaster != null && CharacterHavePerk(_characterCaster.SubclassName, "mainperkpowerful1b"))
				{
					AC.AuraConsumed = 1;
				}
				if (_characterCaster != null && CharacterHavePerk(_characterCaster.SubclassName, "mainperkpowerful1c"))
				{
					AC.ConsumeAll = true;
				}
			}
			if (flag2 && doublePowerfulEffect)
			{
				AC.HealDonePercentPerStack *= 2;
				AC.AuraDamageIncreasedPercentPerStack *= 2f;
			}
			break;
		case "regeneration":
			if (_type == "set")
			{
				if (flag2)
				{
					if (TeamHavePerk("mainperkregeneration1b"))
					{
						AC.HealReceivedPercentPerStack = 1;
					}
					if (TeamHavePerk("mainperkregeneration1c"))
					{
						AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Shadow, 0, 0.5f);
						AC.AuraConsumed = 0;
					}
				}
			}
			else if (_type == "consume" && flag)
			{
				if (_characterCaster != null && CharacterHavePerk(_characterCaster.SubclassName, "mainperkregeneration1a"))
				{
					AC.HealSidesWhenConsumed = AC.HealWhenConsumed;
					AC.HealSidesWhenConsumedPerCharge = AC.HealWhenConsumedPerCharge;
				}
				if (TeamHavePerk("mainperkregeneration1c"))
				{
					AC.AuraConsumed = 0;
				}
			}
			break;
		case "reinforce":
			if (_type == "set" && flag2)
			{
				if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkReinforce0"))
				{
					AC.Removable = false;
				}
				if (TeamHavePerk("mainperkReinforce1b"))
				{
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Slashing, 35, 0f);
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Piercing, 35, 0f);
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Blunt, 35, 0f);
				}
				if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkReinforce1c"))
				{
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Slashing, 0, 7f);
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Piercing, 0, 7f);
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Blunt, 0, 7f);
					AC.GainCharges = true;
					AuraCurseData auraCurseData6 = AC;
					int maxCharges = (AC.MaxMadnessCharges = 8);
					auraCurseData6.MaxCharges = maxCharges;
				}
			}
			UpdateResistanceModifiersBasedOnInfuse(_type, _characterCaster, _characterTarget, ref AC);
			break;
		case "rust":
			if (_type == "set" && _characterTarget != null && !flag2)
			{
				if (TeamHavePerk("mainperkrust0b"))
				{
					AC.GainCharges = true;
					AC.MaxCharges = 12;
					AC.MaxMadnessCharges = 12;
				}
				if (TeamHavePerk("mainperkrust0c"))
				{
					AC.PreventedAuraCurse = null;
					AC.PreventedAuraCurseStackPerStack = 0;
					AC.ChargesMultiplierDescription = 0;
				}
			}
			break;
		case "sanctify":
			if (_type == "set" && !flag2 && TeamHavePerk("mainperkSanctify2b"))
			{
				AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Holy, 0, -0.5f);
			}
			break;
		case "scourge":
			if (_type == "set" && !flag2)
			{
				if (TeamHaveTrait("auraofdespair"))
				{
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.All, 0, -5f);
				}
				if (TeamHaveTrait("unholyblight"))
				{
					AC.GainCharges = true;
				}
			}
			break;
		case "shackle":
			if (_type == "set")
			{
				if (!flag2 && TeamHaveTrait("webweaver"))
				{
					AC = GlobalAuraCurseModifyDamage(AC, Enums.DamageType.All, 0, 0, -30);
					AC.ConsumedAtTurnBegin = false;
					AC.ConsumedAtTurn = true;
				}
			}
			else if (_type == "consume" && !flag && TeamHaveTrait("webweaver"))
			{
				AC.ConsumedAtTurnBegin = false;
				AC.ConsumedAtTurn = true;
				AC.GainAuraCurseConsumption = Globals.Instance.GetAuraCurseData("slow");
				AC.GainAuraCurseConsumptionPerCharge = 2;
			}
			break;
		case "sharp":
		{
			if (_type == "set")
			{
				if (flag2)
				{
					if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkSharp1a"))
					{
						AC.AuraDamageIncreasedPerStack = 1.5f;
						AC.AuraDamageType2 = Enums.DamageType.None;
						AC.AuraDamageIncreasedPerStack2 = 0f;
					}
					if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkSharp1b"))
					{
						AC.AuraDamageType = Enums.DamageType.Piercing;
						AC.AuraDamageIncreasedPerStack = 1.5f;
						AC.AuraDamageType2 = Enums.DamageType.None;
						AC.AuraDamageIncreasedPerStack2 = 0f;
					}
					if (TeamHavePerk("mainperkSharp1c"))
					{
						AC.ConsumedAtTurn = false;
						AC.AuraConsumed = 0;
						AC.Removable = false;
					}
					if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkSharp1d"))
					{
						AC = GlobalAuraCurseModifyDamage(AC, Enums.DamageType.Shadow, 0, 1, 0);
					}
					if (TeamHaveTrait("shrilltone"))
					{
						AC = GlobalAuraCurseModifyDamage(AC, Enums.DamageType.Mind, 0, 1, 0);
					}
				}
			}
			else if (_type == "consume" && flag && TeamHavePerk("mainperkSharp1c"))
			{
				AC.ConsumedAtTurn = false;
				AC.AuraConsumed = 0;
			}
			int num10 = 0;
			if (_characterCaster != null && _type == "consume")
			{
				num10 = _characterCaster.GetAuraCharges("rust");
			}
			else if (_characterTarget != null)
			{
				num10 = _characterTarget.GetAuraCharges("rust");
			}
			if (num10 > 0)
			{
				AC.AuraDamageIncreasedPerStack *= 0.5f;
				AC.AuraDamageIncreasedPerStack2 *= 0.5f;
			}
			break;
		}
		case "sight":
			if (_type == "set")
			{
				if (!flag2)
				{
					if (TeamHavePerk("mainperksight1b"))
					{
						AC.Removable = false;
					}
					if (TeamHavePerk("mainperksight1c"))
					{
						AC.Preventable = false;
						AC.ConsumeAll = true;
					}
					if (TeamHaveTrait("keensight"))
					{
						AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Piercing, 0, -0.3f);
					}
				}
			}
			else if (_type == "consume" && !flag && TeamHavePerk("mainperksight1c"))
			{
				AC.ConsumeAll = true;
			}
			break;
		case "slow":
		{
			int num9 = 0;
			if (_characterCaster != null && _type == "consume")
			{
				num9 = _characterCaster.GetAuraCharges("rust");
			}
			else if (_characterTarget != null)
			{
				num9 = _characterTarget.GetAuraCharges("rust");
			}
			if (num9 > 0)
			{
				if (!flag2 && TeamHavePerk("mainperkrust0b"))
				{
					AC.CharacterStatModifiedValuePerStack *= 1f + (float)num9 * 0.2f;
				}
				else
				{
					AC.CharacterStatModifiedValuePerStack = Functions.FuncRoundToInt(AC.CharacterStatModifiedValuePerStack * 1.5f);
				}
			}
			break;
		}
		case "spark":
			if (_type == "set")
			{
				if (!flag && TeamHavePerk("mainperkspark1b"))
				{
					AC.DamageWhenConsumedPerCharge = 1f;
				}
			}
			else if (_type == "consume" && !flag && TeamHaveTrait("voltaicarc"))
			{
				AC.DamageWhenConsumedPerCharge = 1f;
			}
			break;
		case "spellsword":
			if (_type == "set" && flag2 && _characterTarget != null)
			{
				if (CharacterHaveTrait(_characterTarget.SubclassName, "unlimitedblades"))
				{
					AuraCurseData auraCurseData5 = AC;
					int maxCharges = (AC.MaxMadnessCharges = 12);
					auraCurseData5.MaxCharges = maxCharges;
				}
				if (CharacterHaveItem(_characterTarget.SubclassName, "eldritchswordrare"))
				{
					AC.MaxCharges += Globals.Instance.GetItemData("eldritchswordrare").AuracurseCustomModValue1;
					AC.MaxMadnessCharges += Globals.Instance.GetItemData("eldritchswordrare").AuracurseCustomModValue1;
				}
				if (CharacterHaveTrait(_characterTarget.SubclassName, "frostswords"))
				{
					AC.AuraDamageIncreasedPerStack = 1f;
				}
			}
			break;
		case "infuse":
			AC.CustomAuxValue = 50;
			AC.ChargesAuxNeedForOne2 = 1;
			if (!(_type == "set") || !flag2)
			{
				break;
			}
			if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkinfuse0b"))
			{
				for (int i = 0; i < AC.AuraDamageConditionalBonuses.Length; i++)
				{
					AC.AuraDamageConditionalBonuses[i].AuraDamageIncreasedPerStack = 0f;
				}
				AC.CustomAuxValue = 75;
				AC.ChargesAuxNeedForOne2 = 0;
			}
			if (_characterTarget != null)
			{
				if (_characterCaster != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkinfuse0c") && !forPopup)
				{
					_characterTarget.SetAuraTrait(_characterCaster, "buffer", 1);
				}
				if (TeamHaveItem("veiledbondsrare"))
				{
					AC.MaxCharges += Globals.Instance.GetItemData("veiledbondsrare").AuracurseCustomModValue1;
					AC.MaxMadnessCharges += Globals.Instance.GetItemData("veiledbondsrare").AuracurseCustomModValue1;
				}
				else if (TeamHaveItem("veiledbonds"))
				{
					AC.MaxCharges += Globals.Instance.GetItemData("veiledbonds").AuracurseCustomModValue1;
					AC.MaxMadnessCharges += Globals.Instance.GetItemData("veiledbonds").AuracurseCustomModValue1;
				}
			}
			break;
		case "stanzai":
			if (_type == "set")
			{
				if (flag2 && _characterTarget != null && TeamHavePerk("mainperkstanza0a"))
				{
					AC.AuraDamageType = Enums.DamageType.All;
				}
			}
			else if (_type == "consume" && flag && _characterCaster != null && CharacterHavePerk(_characterCaster.SubclassName, "mainperkstanza0b"))
			{
				AC.GainAuraCurseConsumption2 = Globals.Instance.GetAuraCurseData("inspire");
				AC.GainAuraCurseConsumptionPerCharge2 = 1;
			}
			break;
		case "stanzaii":
			if (_type == "set" && flag2 && _characterTarget != null && TeamHavePerk("mainperkstanza0a"))
			{
				AC.AuraDamageType = Enums.DamageType.All;
			}
			break;
		case "stanzaiii":
			if (_type == "set")
			{
				if (flag2 && _characterTarget != null && TeamHavePerk("mainperkstanza0a"))
				{
					AC.AuraDamageType = Enums.DamageType.All;
				}
			}
			else if (_type == "consume" && flag && TeamHaveTrait("choir"))
			{
				AC.ConsumedAtTurn = false;
				AC.AuraConsumed = 0;
			}
			break;
		case "stealth":
			if (_type == "set")
			{
				if (flag2)
				{
					if (TeamHavePerk("mainperkstealth1a"))
					{
						AC.AuraDamageIncreasedPercentPerStack = 25f;
						AC.HealDonePercentPerStack = 25;
					}
					if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkstealth1b"))
					{
						AC.ConsumedAtTurnBegin = false;
						AC.AuraConsumed = 0;
					}
					if (TeamHavePerk("mainperkstealth1c"))
					{
						AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.All, 0, 5f);
					}
				}
			}
			else if (_type == "consume" && flag && _characterCaster != null && CharacterHavePerk(_characterCaster.SubclassName, "mainperkstealth1b"))
			{
				AC.ConsumedAtTurnBegin = false;
				AC.AuraConsumed = 0;
			}
			break;
		case "stealthbonus":
			if (_type == "set" && flag2 && TeamHavePerk("mainperkstealth1a"))
			{
				AC.AuraDamageIncreasedPercentPerStack = 25f;
				AC.HealDonePercentPerStack = 25;
			}
			break;
		case "taunt":
			if (_type == "set" && flag2)
			{
				if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkTaunt1b"))
				{
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Slashing, 0, 10f);
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Piercing, 0, 10f);
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Blunt, 0, 10f);
				}
				if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkTaunt1c"))
				{
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Fire, 0, 10f);
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Cold, 0, 10f);
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Lightning, 0, 10f);
				}
				if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkTaunt1d"))
				{
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Mind, 0, 10f);
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Holy, 0, 10f);
					AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Shadow, 0, 10f);
				}
			}
			break;
		case "thorns":
			if (!(_type == "set"))
			{
				break;
			}
			if (flag2)
			{
				if (TeamHavePerk("mainperkthorns1a"))
				{
					AC.DamageReflectedConsumeCharges = 0;
				}
				if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkthorns1b"))
				{
					AC.DamageReflectedType = Enums.DamageType.Holy;
				}
			}
			if (IsChallengeTraitActive("sacredthorns"))
			{
				AC.DamageReflectedType = Enums.DamageType.Holy;
			}
			break;
		case "vitality":
			if (_type == "set")
			{
				if (flag2)
				{
					if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkvitality1a"))
					{
						AC.CharacterStatModifiedValuePerStack = 8f;
					}
					if (TeamHavePerk("mainperkvitality1c"))
					{
						AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Mind, 0, 0.5f);
						AC.ConsumedAtTurnBegin = false;
						AC.AuraConsumed = 0;
					}
				}
			}
			else if (_type == "consume" && flag && TeamHavePerk("mainperkvitality1c"))
			{
				AC.ConsumedAtTurnBegin = false;
				AC.AuraConsumed = 0;
			}
			break;
		case "vulnerable":
			if (_type == "set")
			{
				if (!flag2)
				{
					if (MadnessManager.Instance.IsMadnessTraitActive("resistantmonsters"))
					{
						AuraCurseData auraCurseData = AC;
						int maxCharges = (AC.MaxMadnessCharges = 6);
						auraCurseData.MaxCharges = maxCharges;
					}
					if (TeamHavePerk("mainperkVulnerable0b"))
					{
						AC.MaxCharges++;
						AC.MaxMadnessCharges++;
						AC.AuraConsumed = 1;
					}
					if (TeamHaveTrait("alkahest"))
					{
						AC.MaxCharges += 8;
						AC.MaxMadnessCharges += 8;
					}
					if (TeamHaveItem("nullifierrare", 3))
					{
						AC.MaxCharges += Globals.Instance.GetItemData("nullifierrare").AuracurseCustomModValue1;
						AC.MaxMadnessCharges += Globals.Instance.GetItemData("nullifierrare").AuracurseCustomModValue1;
					}
					else if (TeamHaveItem("nullifier", 3))
					{
						AC.MaxCharges += Globals.Instance.GetItemData("nullifier").AuracurseCustomModValue1;
						AC.MaxMadnessCharges += Globals.Instance.GetItemData("nullifier").AuracurseCustomModValue1;
					}
					if (TeamHavePerk("mainperkVulnerable0c"))
					{
						AC.ResistModified = Enums.DamageType.None;
						AC.ResistModifiedValue = 0f;
						AC.ResistModifiedPercentagePerStack = 0f;
						AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Slashing, 0, -8f);
						AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Piercing, 0, -8f);
						AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Blunt, 0, -8f);
					}
				}
			}
			else if (_type == "consume" && !flag && TeamHavePerk("mainperkVulnerable0b"))
			{
				AC.AuraConsumed = 1;
				AC.ConsumedAtTurnBegin = false;
				AC.ConsumedAtTurn = true;
			}
			break;
		case "wet":
		{
			if (_type == "set")
			{
				if (!flag2)
				{
					if (TeamHavePerk("mainperkwet1a"))
					{
						AC.IncreasedDamageReceivedType2 = Enums.DamageType.Cold;
						AC.IncreasedDirectDamageReceivedPerStack2 = 1f;
					}
					if (TeamHavePerk("mainperkwet1b"))
					{
						AC = GlobalAuraCurseModifyResist(AC, Enums.DamageType.Lightning, 0, -1f);
						AC.ConsumedAtTurn = false;
						AC.AuraConsumed = 0;
					}
				}
				else if (TeamHavePerk("mainperkwet1c"))
				{
					AC.PreventedAuraCurseStackPerStack = 3;
					AC.ConsumedAtTurn = false;
					AC.AuraConsumed = 0;
				}
				if (IsChallengeTraitActive("icydeluge"))
				{
					AC.IncreasedDamageReceivedType2 = Enums.DamageType.Cold;
					AC.IncreasedDirectDamageReceivedPerStack2 = 1f;
				}
			}
			else if (_type == "consume")
			{
				if (!flag)
				{
					if (TeamHavePerk("mainperkwet1b"))
					{
						AC.ConsumedAtTurn = false;
						AC.AuraConsumed = 0;
					}
				}
				else if (TeamHavePerk("mainperkwet1c"))
				{
					AC.ConsumedAtTurn = false;
					AC.AuraConsumed = 0;
				}
			}
			int num7 = 0;
			if (_characterCaster != null && _type == "consume")
			{
				num7 = _characterCaster.GetAuraCharges("rust");
			}
			else if (_characterTarget != null)
			{
				num7 = _characterTarget.GetAuraCharges("rust");
			}
			if (num7 > 0)
			{
				AC.IncreasedDirectDamageReceivedPerStack *= 0.5f;
				AC.IncreasedDirectDamageReceivedPerStack2 *= 0.5f;
				if (TeamHavePerk("mainperkwet1b"))
				{
					AC.ResistModifiedPercentagePerStack = -0.5f;
				}
			}
			break;
		}
		case "zeal":
			if (_type == "set")
			{
				if (flag2)
				{
					if (_characterTarget != null && CharacterHaveTrait(_characterTarget.SubclassName, "zealotry"))
					{
						AC.AuraDamageIncreasedPercentPerStack = 1.5f;
						AC.ConsumeAll = false;
						AC.AuraConsumed = 2;
					}
					if (_characterTarget != null && CharacterHaveTrait(_characterTarget.SubclassName, "righteousflame"))
					{
						AC.AuraDamageType2 = Enums.DamageType.All;
						AC.AuraDamageIncreasedPercentPerStack2 = 7f;
					}
					if (_characterTarget != null && CharacterHavePerk(_characterTarget.SubclassName, "mainperkzeal0c"))
					{
						AC.ConsumeAll = false;
						AC.GainCharges = true;
						AC.MaxCharges = 10;
						AC.MaxMadnessCharges = 10;
						AC.ResistModifiedPercentagePerStack = 5f;
						AC.AuraDamageType = Enums.DamageType.None;
						AC.AuraDamageChargesBasedOnACCharges = null;
						AC.AuraDamageIncreasedPercentPerStack = 0f;
						AC.ChargesMultiplierDescription = 12;
					}
				}
			}
			else if (_type == "consume" && flag)
			{
				if (_characterCaster != null && CharacterHaveTrait(_characterCaster.SubclassName, "zealotry"))
				{
					AC.ConsumeAll = false;
					AC.AuraConsumed = 2;
				}
				if (_characterCaster != null && TeamHavePerk("mainperkzeal0b"))
				{
					AC.ConsumeAll = false;
					AC.AuraConsumed = 1;
				}
				if (_characterCaster != null && CharacterHavePerk(_characterCaster.SubclassName, "mainperkzeal0c"))
				{
					AC.ConsumeAll = false;
					AC.AuraConsumed = 1;
				}
			}
			break;
		}
		if (useCache)
		{
			if (cacheGlobalACModification.ContainsKey(key))
			{
				cacheGlobalACModification[key] = AC;
			}
			else
			{
				cacheGlobalACModification.Add(key, AC);
			}
		}
		return AC;
	}

	private void UpdateResistanceModifiersBasedOnInfuse(string _type, Character _characterCaster, Character _characterTarget, ref AuraCurseData AC)
	{
		int num = 0;
		if (_characterCaster != null && _type == "consume")
		{
			num = _characterCaster.GetAuraCharges("infuse");
		}
		else if (_characterTarget != null)
		{
			num = _characterTarget.GetAuraCharges("infuse");
		}
		if (num > 0)
		{
			float num2 = 1.5f;
			if (_characterCaster != null && CharacterHavePerk(_characterCaster.SubclassName, "mainperkinfuse0b"))
			{
				num2 = 1.75f;
			}
			AC.ResistModifiedValue *= num2;
			AC.ResistModifiedValue2 *= num2;
			AC.ResistModifiedValue3 *= num2;
			AC.ResistModifiedPercentagePerStack *= num2;
			AC.ResistModifiedPercentagePerStack2 *= num2;
			AC.ResistModifiedPercentagePerStack3 *= num2;
		}
	}

	public bool ValidCardback(CardbackData cbd, int rankProgressN)
	{
		bool result = false;
		if (cbd.Sku != "" && SteamManager.Instance.PlayerHaveDLC(cbd.Sku))
		{
			result = true;
		}
		else if (cbd.SteamStat != "" && SteamManager.Instance.GetStatInt(cbd.SteamStat) == 1)
		{
			result = true;
		}
		else if (cbd.RankLevel > 0)
		{
			if (cbd.RankLevel <= rankProgressN)
			{
				result = true;
			}
		}
		else if (cbd.AdventureLevel > 0)
		{
			if (cbd.AdventureLevel == 1 && PlayerManager.Instance.NgLevel > 0)
			{
				result = true;
			}
			else if (cbd.AdventureLevel <= PlayerManager.Instance.MaxAdventureMadnessLevel)
			{
				result = true;
			}
		}
		else if (cbd.ObeliskLevel > 0)
		{
			if (cbd.ObeliskLevel == 1 && PlayerManager.Instance.ObeliskMadnessLevel > 0)
			{
				result = true;
			}
			else if (cbd.ObeliskLevel < PlayerManager.Instance.ObeliskMadnessLevel)
			{
				result = true;
			}
		}
		else if (cbd.SingularityLevel <= 0)
		{
			result = !cbd.Locked || (PlayerManager.Instance.IsCardbackUnlocked(cbd.CardbackId) ? true : false);
		}
		else if (cbd.SingularityLevel == 1 && PlayerManager.Instance.SingularityMadnessLevel > 0)
		{
			result = true;
		}
		else if (cbd.SingularityLevel < PlayerManager.Instance.SingularityMadnessLevel)
		{
			result = true;
		}
		if (cbd.PdxAccountRequired && !Startup.isLoggedIn)
		{
			result = false;
		}
		return result;
	}

	public void RedoSkins()
	{
		for (int i = 0; i < teamAtO.Length; i++)
		{
			if (teamAtO[i] != null)
			{
				if (teamAtO[i].SkinUsed != null && teamAtO[i].SkinUsed != "")
				{
					SetSkinIntoSubclassData(teamAtO[i].SubclassName, teamAtO[i].SkinUsed);
				}
				else
				{
					SetSkinIntoSubclassData(teamAtO[i].SubclassName, Globals.Instance.GetSkinBaseIdBySubclass(teamAtO[i].SubclassName));
				}
				teamAtO[i].RedoSubclassFromSkin();
			}
		}
	}

	public void AssignTeamFromSaveGame(Hero[] _team)
	{
		CreateTeam();
		for (int i = 0; i < _team.Length; i++)
		{
			teamAtO[i] = GameManager.Instance.AssignDataToHero(_team[i]);
		}
	}

	public void AssignTeamBackupFromSaveGame(Hero[] _team)
	{
		CreateTeamBackup();
		if (_team != null && _team.Length == 4)
		{
			for (int i = 0; i < _team.Length; i++)
			{
				teamAtOBackup[i] = GameManager.Instance.AssignDataToHero(_team[i]);
			}
		}
	}

	public void SetTeam(Hero[] _team)
	{
		CreateTeam();
		for (int i = 0; i < _team.Length; i++)
		{
			if (_team[i] != null)
			{
				teamAtO[i] = _team[i];
			}
		}
	}

	public void SetTeamNPC(string[] _team)
	{
		teamNPCAtO = new string[4];
		for (int i = 0; i < _team.Length; i++)
		{
			if (_team[i] != null)
			{
				teamNPCAtO[i] = _team[i];
			}
		}
	}

	public void SetTeamSingle(Hero _hero, int position)
	{
		if (teamAtO == null || teamAtO.Length == 0)
		{
			CreateTeam();
		}
		teamAtO[position] = _hero;
	}

	public void SetTeamNPCSingle(string _npc, int position)
	{
		if (teamNPCAtO == null || teamNPCAtO.Length == 0)
		{
			CreateTeamNPC();
		}
		teamNPCAtO[position] = _npc;
	}

	public void SetTeamNPCFromCombatData(CombatData _combatData)
	{
		bool flag = false;
		if (MadnessManager.Instance.IsMadnessTraitActive("randomcombats") || GameManager.Instance.IsObeliskChallenge() || Instance.IsChallengeTraitActive("randomcombats"))
		{
			flag = true;
		}
		bool flag2 = false;
		NodeData nodeData = Globals.Instance.GetNodeData(currentMapNode);
		if (nodeData != null && nodeData.NodeCombatTier != Enums.CombatTier.T0)
		{
			flag2 = true;
		}
		if (nodeData != null && nodeData.DisableRandom)
		{
			flag2 = false;
		}
		if (flag2 && (_combatData.CombatId == "eaqua_37a" || _combatData.CombatId == "eaqua_37b" || _combatData.NeverRandomizeEnemies))
		{
			flag2 = false;
		}
		if (flag && flag2 && _combatData != null)
		{
			int deterministicHashCode = (currentMapNode + GetGameId() + _combatData.CombatId).GetDeterministicHashCode();
			NPCData[] randomCombat = Functions.GetRandomCombat(nodeData.NodeCombatTier, deterministicHashCode, currentMapNode);
			for (int i = 0; i < 4; i++)
			{
				if (i < randomCombat.Length && randomCombat[i] != null)
				{
					SetTeamNPCSingle(randomCombat[i].Id, i);
				}
				else
				{
					SetTeamNPCSingle("", i);
				}
			}
			return;
		}
		for (int j = 0; j < 4; j++)
		{
			if (j < _combatData.NPCList.Length && _combatData.NPCList[j] != null)
			{
				SetTeamNPCSingle(_combatData.NPCList[j].Id, j);
			}
			else
			{
				SetTeamNPCSingle("", j);
			}
		}
	}

	public void SetObeliskNodes()
	{
		obeliskLow = "";
		obeliskHigh = "";
		obeliskFinal = "";
		List<string> list = new List<string>(Globals.Instance.ZoneDataSource.Keys);
		for (int num = list.Count - 1; num >= 0; num--)
		{
			if (list[num] == "pyramid" || list[num] == "ulminin")
			{
				list.RemoveAt(num);
			}
		}
		UnityEngine.Random.InitState(GetGameId().GetDeterministicHashCode());
		bool flag = false;
		int num2 = -1;
		while (!flag)
		{
			int num3 = UnityEngine.Random.Range(0, list.Count);
			string key = list[num3];
			if (obeliskLow == "")
			{
				if (Globals.Instance.ZoneDataSource[key].ObeliskLow)
				{
					obeliskLow = Globals.Instance.ZoneDataSource[key].ZoneId.ToLower();
					num2 = num3;
				}
			}
			else if (obeliskHigh == "")
			{
				if (num3 == num2)
				{
					continue;
				}
				if (Globals.Instance.ZoneDataSource[key].ObeliskHigh)
				{
					obeliskHigh = Globals.Instance.ZoneDataSource[key].ZoneId.ToLower();
				}
			}
			else if (obeliskFinal == "" && Globals.Instance.ZoneDataSource[key].ObeliskFinal)
			{
				obeliskFinal = Globals.Instance.ZoneDataSource[key].ZoneId.ToLower();
			}
			if (obeliskLow != "" && obeliskHigh != "" && obeliskFinal != "")
			{
				flag = true;
			}
		}
		if (MapManager.Instance != null)
		{
			MapManager.Instance.IncludeMapPrefab(obeliskLow + "_0");
			MapManager.Instance.IncludeMapPrefab(obeliskHigh + "_0");
			MapManager.Instance.IncludeMapPrefab(obeliskFinal + "_0");
			MapManager.Instance.IncludeObeliskBgs();
		}
	}

	public void GenerateObeliskMap()
	{
		List<string> list = new List<string>();
		int deterministicHashCode = GetGameId().GetDeterministicHashCode();
		UnityEngine.Random.InitState(deterministicHashCode);
		SetObeliskBosses();
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		List<string> list4 = new List<string>();
		List<string> list5 = new List<string>();
		List<EventData> list6 = new List<EventData>();
		foreach (KeyValuePair<string, EventData> @event in Globals.Instance.Events)
		{
			if (@event.Value.EventTier == Enums.CombatTier.T1)
			{
				list2.Add(@event.Value.EventId);
			}
			else if (@event.Value.EventTier == Enums.CombatTier.T2)
			{
				list3.Add(@event.Value.EventId);
			}
			else if (@event.Value.EventTier == Enums.CombatTier.T3)
			{
				list4.Add(@event.Value.EventId);
			}
			else if (@event.Value.EventTier == Enums.CombatTier.T4)
			{
				list5.Add(@event.Value.EventId);
			}
			else if (@event.Value.EventTier == Enums.CombatTier.T5)
			{
				list6.Add(@event.Value);
			}
		}
		UnityEngine.Random.InitState(deterministicHashCode);
		list2 = list2.ShuffleList();
		list3 = list3.ShuffleList();
		list4 = list4.ShuffleList();
		list5 = list5.ShuffleList();
		List<NodeData> list7 = new List<NodeData>();
		List<NodeData> list8 = new List<NodeData>();
		List<NodeData> list9 = new List<NodeData>();
		foreach (KeyValuePair<string, string> item in gameNodeAssigned)
		{
			NodeData nodeData = Globals.Instance.GetNodeData(item.Key);
			if (!(nodeData == null) && nodeData.NodeEvent != null && (nodeData.NodeZone.ZoneId.ToLower() == obeliskLow || nodeData.NodeZone.ZoneId.ToLower() == obeliskHigh || nodeData.NodeZone.ZoneId.ToLower() == obeliskFinal))
			{
				list7.Add(nodeData);
			}
		}
		list7 = list7.ShuffleList();
		for (int i = 0; i < list7.Count; i++)
		{
			if (!(list7[i].NodeZone.ZoneId.ToLower() == obeliskHigh))
			{
				list9.Add(list7[i]);
			}
		}
		int num = 0;
		while (num < list7.Count)
		{
			NodeData nodeData2 = list7[num];
			Enums.CombatTier nodeEventTier = nodeData2.NodeEventTier;
			EventData eventData = null;
			switch (nodeEventTier)
			{
			case Enums.CombatTier.T1:
				if (list2.Count > 0)
				{
					eventData = Globals.Instance.GetEventData(list2[0]);
					list2.RemoveAt(0);
				}
				break;
			case Enums.CombatTier.T2:
				if (list3.Count > 0)
				{
					eventData = Globals.Instance.GetEventData(list3[0]);
					list3.RemoveAt(0);
				}
				else if (list2.Count > 0)
				{
					eventData = Globals.Instance.GetEventData(list2[0]);
					list2.RemoveAt(0);
				}
				break;
			case Enums.CombatTier.T3:
				if (list5.Count > 0)
				{
					eventData = Globals.Instance.GetEventData(list5[0]);
					list5.RemoveAt(0);
				}
				else if (list4.Count > 0)
				{
					eventData = Globals.Instance.GetEventData(list4[0]);
					list4.RemoveAt(0);
				}
				break;
			}
			if (eventData != null)
			{
				if (eventData.EventUniqueId == "" || !list.Contains(eventData.EventUniqueId))
				{
					gameNodeAssigned[nodeData2.NodeId] = "event:" + eventData.EventId;
					if ((eventData.EventTier == Enums.CombatTier.T3 || eventData.EventTier == Enums.CombatTier.T4) && (mapVisitedNodes == null || !mapVisitedNodes.Contains(nodeData2.NodeId)))
					{
						list8.Add(nodeData2);
					}
					if (eventData.EventUniqueId != "")
					{
						list.Add(eventData.EventUniqueId);
					}
					num++;
				}
				else if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Unique Id already used: " + eventData.EventUniqueId, "ocmap");
				}
			}
			else
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Event data is null", "ocmap");
				}
				num++;
			}
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("------- requirements -------", "ocmap");
		}
		int index = 0;
		for (int j = 0; j < playerRequeriments.Count; j++)
		{
			for (int k = 0; k < list6.Count; k++)
			{
				if (list6[k] != null && list6[k].Requirement != null && list6[k].Requirement.RequirementId == playerRequeriments[j] && list8 != null && list8.Count > 0)
				{
					gameNodeAssigned[list8[list8.Count - 1].NodeId] = "event:" + list6[k].EventId;
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("Node replaced " + list8[index].NodeId + " -> event:" + list6[k].EventId, "ocmap");
					}
					list8.RemoveAt(list8.Count - 1);
				}
			}
		}
		for (int l = 0; l < mapVisitedNodesAction.Count; l++)
		{
			string[] array = mapVisitedNodesAction[l].Split('|');
			if (array != null && array[0] != null && array[1] != null && Globals.Instance.GetEventData(array[1]) != null)
			{
				gameNodeAssigned[array[0]] = "event:" + array[1];
			}
		}
	}

	public void SetObeliskBosses()
	{
		mapNodeObeliskBoss.Clear();
		int num = 1;
		if (IsChallengeTraitActive("morechampions"))
		{
			num = 2;
		}
		List<string> list = new List<string>();
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < 11; j++)
			{
				Enums.CombatTier combatTier = Enums.CombatTier.T0;
				switch (j)
				{
				case 0:
					combatTier = Enums.CombatTier.T1;
					break;
				case 1:
					combatTier = Enums.CombatTier.T2;
					break;
				case 2:
					combatTier = Enums.CombatTier.T3;
					break;
				case 3:
					combatTier = Enums.CombatTier.T4;
					break;
				case 4:
					combatTier = Enums.CombatTier.T5;
					break;
				case 5:
					combatTier = Enums.CombatTier.T6;
					break;
				case 6:
					combatTier = Enums.CombatTier.T7;
					break;
				case 7:
					combatTier = Enums.CombatTier.T8;
					break;
				case 8:
					combatTier = Enums.CombatTier.T9;
					break;
				case 9:
					combatTier = Enums.CombatTier.T11;
					break;
				case 10:
					combatTier = Enums.CombatTier.T12;
					break;
				}
				if (combatTier == Enums.CombatTier.T0)
				{
					continue;
				}
				List<string> list2 = new List<string>();
				foreach (KeyValuePair<string, string> item2 in gameNodeAssigned)
				{
					NodeData nodeData = Globals.Instance.GetNodeData(item2.Key);
					if (nodeData.NodeCombatTier == combatTier && (nodeData.NodeZone.ZoneId.ToLower() == obeliskLow || nodeData.NodeZone.ZoneId.ToLower() == obeliskHigh || nodeData.NodeZone.ZoneId.ToLower() == obeliskFinal) && !list.Contains(item2.Key))
					{
						list2.Add(item2.Key);
					}
				}
				if (list2.Count > 0)
				{
					string item = list2[UnityEngine.Random.Range(0, list2.Count)];
					if (!list.Contains(item))
					{
						list.Add(item);
					}
					switch (combatTier)
					{
					case Enums.CombatTier.T8:
						mapNodeObeliskBoss.Add(item);
						break;
					case Enums.CombatTier.T9:
						mapNodeObeliskBoss.Add(item);
						break;
					default:
						mapNodeObeliskBoss.Add(item);
						break;
					}
				}
			}
		}
	}

	public bool NodeHaveBossRare(string _nodeId)
	{
		if (mapNodeObeliskBoss.Contains(_nodeId))
		{
			return true;
		}
		return false;
	}

	public void ReplaceCardInDeck(int heroIndex, int cardIndex, string cardId)
	{
		if (cardIndex < teamAtO[heroIndex].Cards.Count)
		{
			teamAtO[heroIndex].Cards[cardIndex] = cardId;
			if (GameManager.Instance.IsMultiplayer())
			{
				photonView.RPC("NET_ReplaceCardInDeck", RpcTarget.Others, heroIndex, cardIndex, cardId);
			}
		}
	}

	[PunRPC]
	private void NET_ReplaceCardInDeck(int heroIndex, int cardIndex, string cardId)
	{
		if (cardIndex < teamAtO[heroIndex].Cards.Count)
		{
			teamAtO[heroIndex].Cards[cardIndex] = cardId;
		}
		SideBarRefreshCards(heroIndex);
	}

	public void RemoveCardInDeck(int heroIndex, int cardIndex)
	{
		if (cardIndex < teamAtO[heroIndex].Cards.Count)
		{
			teamAtO[heroIndex].Cards.RemoveAt(cardIndex);
			if (GameManager.Instance.IsMultiplayer())
			{
				photonView.RPC("NET_RemoveCardInDeck", RpcTarget.Others, heroIndex, cardIndex);
			}
		}
	}

	public void RemoveAllCardsInDeck(int heroIndex)
	{
		for (int num = teamAtO[heroIndex].Cards.Count - 1; num >= 0; num--)
		{
			RemoveCardInDeck(heroIndex, num);
		}
	}

	[PunRPC]
	private void NET_RemoveCardInDeck(int heroIndex, int cardIndex)
	{
		if (cardIndex < teamAtO[heroIndex].Cards.Count)
		{
			teamAtO[heroIndex].Cards.RemoveAt(cardIndex);
		}
		SideBarRefreshCards(heroIndex);
	}

	public IEnumerator ShareTeam(string sceneToLoad = "", bool setOwners = false)
	{
		if (!NetworkManager.Instance.IsMaster())
		{
			yield break;
		}
		for (int i = 0; i < 4; i++)
		{
			if (teamAtO[i] != null && !(teamAtO[i].HeroData == null))
			{
				if (NetworkManager.Instance.PlayerHeroPositionOwner[i] != "" && NetworkManager.Instance.PlayerHeroPositionOwner[i] != null)
				{
					teamAtO[i].AssignOwner(NetworkManager.Instance.PlayerHeroPositionOwner[i]);
				}
				if (teamAtO[i].HpCurrent <= 0)
				{
					teamAtO[i].HpCurrent = 1;
				}
				if (teamAtOBackup != null && i < teamAtOBackup.Length && teamAtOBackup[i] != null && teamAtOBackup[i].HeroData != null && NetworkManager.Instance.PlayerHeroPositionOwner[i] != "" && NetworkManager.Instance.PlayerHeroPositionOwner[i] != null)
				{
					teamAtOBackup[i].AssignOwner(NetworkManager.Instance.PlayerHeroPositionOwner[i]);
				}
				if (heroPerks != null && heroPerks.ContainsKey(teamAtO[i].SubclassName))
				{
					teamAtO[i].PerkList = heroPerks[teamAtO[i].SubclassName];
				}
			}
		}
		string text = JsonHelper.ToJson(teamAtO);
		string text2 = JsonHelper.ToJson(teamNPCAtO);
		string text3 = JsonHelper.ToJson(teamAtOBackup);
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("shareTeam MP", "net");
		}
		RedoSkins();
		NetworkManager.Instance.ClearAllPlayerManualReady();
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("NET_SetTeam CALL", "net");
		}
		_ = corruptionCommonCompleted;
		if (corruptionCommonCompleted < 0)
		{
			corruptionCommonCompleted = 0;
		}
		_ = corruptionUncommonCompleted;
		if (corruptionUncommonCompleted < 0)
		{
			corruptionUncommonCompleted = 0;
		}
		_ = corruptionRareCompleted;
		if (corruptionRareCompleted < 0)
		{
			corruptionRareCompleted = 0;
		}
		_ = corruptionEpicCompleted;
		if (corruptionEpicCompleted < 0)
		{
			corruptionEpicCompleted = 0;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(corruptionCommonCompleted.ToString());
		stringBuilder.Append("|");
		stringBuilder.Append(corruptionUncommonCompleted.ToString());
		stringBuilder.Append("|");
		stringBuilder.Append(corruptionRareCompleted.ToString());
		stringBuilder.Append("|");
		stringBuilder.Append(corruptionEpicCompleted.ToString());
		photonView.RPC("NET_SetTeam", RpcTarget.Others, gameId, currentMapNode, Functions.CompressString(text), Functions.CompressString(text2), Functions.CompressString(text3), stringBuilder.ToString());
		while (!NetworkManager.Instance.AllPlayersReady("shareteam"))
		{
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
		if (sceneToLoad != "")
		{
			NetworkManager.Instance.LoadScene(sceneToLoad);
		}
		else if (MapManager.Instance != null)
		{
			MapManager.Instance.sideCharacters.Refresh();
		}
	}

	[PunRPC]
	public void NET_SetTeam(string _gameId, string _currentMapNode, string _TeamHero, string _TeamNPC, string _TeamHeroBackup, string _CorruptionCompleted)
	{
		gameId = _gameId;
		currentMapNode = _currentMapNode;
		Hero[] array = JsonHelper.FromJson<Hero>(Functions.DecompressString(_TeamHero));
		string[] array2 = JsonHelper.FromJson<string>(Functions.DecompressString(_TeamNPC));
		Hero[] array3 = JsonHelper.FromJson<Hero>(Functions.DecompressString(_TeamHeroBackup));
		string[] array4 = _CorruptionCompleted.Split('|');
		corruptionCommonCompleted = int.Parse(array4[0]);
		corruptionUncommonCompleted = int.Parse(array4[1]);
		corruptionRareCompleted = int.Parse(array4[2]);
		corruptionEpicCompleted = int.Parse(array4[3]);
		CreateTeam();
		CreateTeamBackup();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == null)
			{
				continue;
			}
			teamAtO[i] = GameManager.Instance.AssignDataToHero(array[i]);
			if (teamAtO[i] != null && teamAtO[i].HeroData != null)
			{
				if (heroPerks == null)
				{
					heroPerks = new Dictionary<string, List<string>>();
				}
				string subclassName = teamAtO[i].SubclassName;
				if (heroPerks.ContainsKey(subclassName))
				{
					heroPerks[subclassName] = teamAtO[i].PerkList;
				}
				else
				{
					heroPerks.Add(subclassName, teamAtO[i].PerkList);
				}
			}
			if (teamAtOBackup == null)
			{
				CreateTeamBackup();
			}
			if (array3 != null && i < array3.Length)
			{
				teamAtOBackup[i] = GameManager.Instance.AssignDataToHero(array3[i]);
			}
		}
		RedoSkins();
		teamNPCAtO = new string[array2.Length];
		for (int j = 0; j < array2.Length; j++)
		{
			teamNPCAtO[j] = array2[j];
		}
		NetworkManager.Instance.SetStatusReady("shareteam");
		if (MapManager.Instance != null)
		{
			MapManager.Instance.sideCharacters.Refresh();
		}
	}

	public void SetCraftReaminingUses(int heroIndex, int uses, bool fromCoop = false)
	{
		if (teamAtO == null || teamAtO.Length == 0 || heroIndex > teamAtO.Length || heroIndex < 0 || teamAtO[heroIndex] == null)
		{
			return;
		}
		if ((bool)CardCraftManager.Instance && CardCraftManager.Instance.craftType == 0 && GameManager.Instance.IsSingularity() && Instance.CharInTown())
		{
			int num = Instance.HowManyUpgradedCardsInAltar(heroIndex);
			uses -= num;
			if (uses < 0)
			{
				uses = 0;
			}
		}
		teamAtO[heroIndex].CraftRemainingUses = uses;
		if (GameManager.Instance.IsMultiplayer() && !fromCoop)
		{
			photonView.RPC("NET_SetCraftReaminingUses", RpcTarget.Others, heroIndex, uses);
		}
	}

	[PunRPC]
	private void NET_SetCraftReaminingUses(int heroIndex, int uses)
	{
		SetCraftReaminingUses(heroIndex, uses, fromCoop: true);
	}

	public int GetCraftReaminingUses(int heroIndex)
	{
		if (heroIndex > -1 && heroIndex < teamAtO.Length)
		{
			return teamAtO[heroIndex].CraftRemainingUses;
		}
		return 0;
	}

	public void SubstractCraftReaminingUses(int heroIndex, int quantity = 1, bool fromCoop = false)
	{
		if (teamAtO[heroIndex].CraftRemainingUses > 0)
		{
			teamAtO[heroIndex].CraftRemainingUses -= quantity;
			if (GameManager.Instance.IsMultiplayer() && !fromCoop)
			{
				photonView.RPC("NET_SubstractCraftReaminingUses", RpcTarget.Others, heroIndex, quantity);
			}
		}
	}

	[PunRPC]
	private void NET_SubstractCraftReaminingUses(int heroIndex, int quantity)
	{
		SubstractCraftReaminingUses(heroIndex, quantity, fromCoop: true);
	}

	public void HeroCraftCrafted(int heroIndex, bool fromCoop = false)
	{
		teamAtO[heroIndex].CardsCrafted++;
		if (!fromCoop)
		{
			PlayerManager.Instance.CardCrafted();
		}
		if (GameManager.Instance.IsMultiplayer() && !fromCoop)
		{
			photonView.RPC("NET_HeroCraftCrafted", RpcTarget.Others, heroIndex);
		}
	}

	[PunRPC]
	private void NET_HeroCraftCrafted(int heroIndex)
	{
		HeroCraftCrafted(heroIndex, fromCoop: true);
	}

	public void HeroCraftUpgraded(int heroIndex, bool fromCoop = false)
	{
		teamAtO[heroIndex].CardsUpgraded++;
		if (!fromCoop)
		{
			PlayerManager.Instance.CardUpgraded();
		}
		if (GameManager.Instance.IsMultiplayer() && !fromCoop)
		{
			photonView.RPC("NET_HeroCraftUpgraded", RpcTarget.Others, heroIndex);
		}
	}

	[PunRPC]
	private void NET_HeroCraftUpgraded(int heroIndex)
	{
		HeroCraftUpgraded(heroIndex, fromCoop: true);
	}

	public void HeroCraftRemoved(int heroIndex, bool fromCoop = false)
	{
		if (!CharInTown() || GetTownTier() != 0)
		{
			teamAtO[heroIndex].CardsRemoved++;
			if (GameManager.Instance.IsMultiplayer() && !fromCoop)
			{
				photonView.RPC("NET_HeroCraftRemoved", RpcTarget.Others, heroIndex);
			}
		}
	}

	[PunRPC]
	private void NET_HeroCraftRemoved(int heroIndex)
	{
		HeroCraftRemoved(heroIndex, fromCoop: true);
	}

	public int GetHeroCraftRemovedTimes(int heroIndex)
	{
		return teamAtO[heroIndex].CardsRemoved;
	}

	public void HeroLevelUp(int heroIndex, string traitId)
	{
		Hero hero = teamAtO[heroIndex];
		if (hero.AssignTrait(traitId))
		{
			TraitData traitData = Globals.Instance.GetTraitData(traitId);
			if (traitData != null && traitData.TraitCard != null)
			{
				int num = PlayerManager.Instance.GetCharacterTier("", "trait", hero.PerkRank);
				if (GameManager.Instance.IsObeliskChallenge())
				{
					if (GameManager.Instance.IsWeeklyChallenge())
					{
						num = 2;
					}
					else if (obeliskMadness >= 5)
					{
						num = ((obeliskMadness < 8) ? 1 : 2);
					}
				}
				string text = traitData.TraitCard.Id;
				switch (num)
				{
				case 1:
					text = Globals.Instance.GetCardData(text, instantiate: false).UpgradesTo1;
					break;
				case 2:
					text = Globals.Instance.GetCardData(text, instantiate: false).UpgradesTo2;
					break;
				}
				AddCardToHero(heroIndex, text);
			}
			if (traitData.TraitCardForAllHeroes != null)
			{
				string id = traitData.TraitCardForAllHeroes.Id;
				for (int i = 0; i < 4; i++)
				{
					AddCardToHero(i, id);
				}
			}
			hero.LevelUp();
		}
		GameManager.Instance.PlayLibraryAudio("ui_level_up");
		SideBarRefresh();
		ReDrawLevel();
	}

	public void HeroLevelUpMP(int _heroIndex, string _traitId)
	{
		photonView.RPC("NET_HeroLevelUpMP", RpcTarget.MasterClient, _heroIndex, _traitId);
	}

	[PunRPC]
	private void NET_HeroLevelUpMP(int _heroIndex, string _traitId)
	{
		HeroLevelUp(_heroIndex, _traitId);
		photonView.RPC("NET_SetHeroLevelUp", RpcTarget.Others, _heroIndex, _traitId);
	}

	[PunRPC]
	public void NET_SetHeroLevelUp(int _heroIndex, string _traitId)
	{
		HeroLevelUp(_heroIndex, _traitId);
	}

	public void ModifyHeroLife(int _heroIndex = -1, int _flat = 0, float _percent = 0f)
	{
		for (int i = 0; i < 4; i++)
		{
			if (_heroIndex == -1 || _heroIndex == i)
			{
				if (_flat != 0)
				{
					teamAtO[i].HpCurrent += _flat;
				}
				if (_percent != 0f)
				{
					teamAtO[i].HpCurrent += Functions.FuncRoundToInt((float)teamAtO[i].Hp * _percent / 100f);
				}
				if (teamAtO[i].HpCurrent > teamAtO[i].Hp)
				{
					teamAtO[i].HpCurrent = teamAtO[i].Hp;
				}
				else if (teamAtO[i].HpCurrent <= 0)
				{
					teamAtO[i].HpCurrent = 1;
				}
			}
		}
	}

	public void UpgradeRandomCardToHero(int _heroIndex, int _quantity = 1)
	{
		for (int i = 0; i < _quantity; i++)
		{
			bool flag = false;
			int num = 0;
			int num2 = 0;
			while (!flag)
			{
				num2 = UnityEngine.Random.Range(0, teamAtO[_heroIndex].Cards.Count);
				CardData cardData = Globals.Instance.GetCardData(teamAtO[_heroIndex].Cards[num2]);
				if (cardData != null && cardData.CardClass != Enums.CardClass.Injury && cardData.CardClass != Enums.CardClass.Boon && cardData.CardClass != Enums.CardClass.Item && cardData.CardUpgraded == Enums.CardUpgraded.No)
				{
					string item = "";
					if (UnityEngine.Random.Range(0, 2) == 0 && cardData.UpgradesTo1 != "")
					{
						teamAtO[_heroIndex].Cards[num2] = cardData.UpgradesTo1;
						item = cardData.UpgradesTo1;
					}
					else if (cardData.UpgradesTo2 != "")
					{
						teamAtO[_heroIndex].Cards[num2] = cardData.UpgradesTo2;
						item = cardData.UpgradesTo2;
					}
					flag = true;
					upgradedCardsList.Add(item);
				}
				if (flag)
				{
					continue;
				}
				num++;
				if (num > 1000)
				{
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("Exhasusted!!!");
					}
					return;
				}
			}
		}
		if (MapManager.Instance != null)
		{
			MapManager.Instance.sideCharacters.ShowUpgrade(_heroIndex);
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			string text = JsonHelper.ToJson(upgradedCardsList.ToArray());
			string text2 = JsonHelper.ToJson(teamAtO[_heroIndex].Cards.ToArray());
			photonView.RPC("NET_SetTeamHeroCards", RpcTarget.Others, _heroIndex, text2, text);
		}
	}

	public void AddCardToHero(int _heroIndex, string _cardName, int count = 1)
	{
		for (int i = 0; i < count; i++)
		{
			CardData cardData = Globals.Instance.GetCardData(_cardName, instantiate: false);
			if (cardData != null && cardData.CardClass == Enums.CardClass.Injury && ngPlus > 0 && IsZoneAffectedByMadness())
			{
				if (ngPlus >= 3 && ngPlus <= 5 && cardData.UpgradesTo1 != "")
				{
					_cardName = cardData.UpgradesTo1;
				}
				else if (ngPlus >= 6 && cardData.UpgradesTo2 != "")
				{
					_cardName = cardData.UpgradesTo2;
				}
			}
			if (cardData != null && cardData.CardType == Enums.CardType.Pet)
			{
				teamAtO[_heroIndex].Pet = _cardName;
			}
			else
			{
				if (GameManager.Instance.IsSingularity())
				{
					CardData cardDataFromCardData = Functions.GetCardDataFromCardData(cardData, "");
					for (int j = 0; j < teamAtO[_heroIndex].Cards.Count; j++)
					{
						if (Functions.GetCardDataFromCardData(Globals.Instance.GetCardData(teamAtO[_heroIndex].Cards[j], instantiate: false), "").Id == cardDataFromCardData.Id)
						{
							teamAtO[_heroIndex].Cards.RemoveAt(j);
							break;
						}
					}
				}
				teamAtO[_heroIndex].Cards.Add(_cardName);
			}
			if (cardData != null)
			{
				if (cardData.CardClass == Enums.CardClass.Boon && IsChallengeTraitActive("doubleboons"))
				{
					teamAtO[_heroIndex].Cards.Add(_cardName);
				}
				if (cardData.CardClass == Enums.CardClass.Injury && IsChallengeTraitActive("doubleinjuries"))
				{
					teamAtO[_heroIndex].Cards.Add(_cardName);
				}
			}
			if (cardData.CardClass == Enums.CardClass.Boon)
			{
				PlayerManager.Instance.CardUnlock(_cardName, save: true);
			}
		}
	}

	public void AddDeckToHeroMP(int _heroIndex, List<string> _cardList)
	{
		string text = JsonHelper.ToJson(_cardList.ToArray());
		photonView.RPC("NET_AddDeckToHeroMP", RpcTarget.Others, _heroIndex, text);
	}

	[PunRPC]
	private void NET_AddDeckToHeroMP(int _heroIndex, string _cardList)
	{
		List<string> list = new List<string>();
		list.AddRange(JsonHelper.FromJson<string>(_cardList));
		teamAtO[_heroIndex].Cards = list;
		SideBarRefreshCards(_heroIndex);
	}

	public void AddCardToHeroMP(int _heroIndex, string _cardName, int count = 1)
	{
		for (int i = 0; i < count; i++)
		{
			photonView.RPC("NET_AddCardToHeroMP", RpcTarget.MasterClient, _heroIndex, _cardName);
		}
	}

	[PunRPC]
	private void NET_AddCardToHeroMP(int _heroIndex, string _cardName)
	{
		teamAtO[_heroIndex].Cards.Add(_cardName);
		string text = JsonHelper.ToJson(teamAtO[_heroIndex].Cards.ToArray());
		photonView.RPC("NET_SetTeamHeroCards", RpcTarget.Others, _heroIndex, text, "");
		SideBarRefreshCards(_heroIndex);
	}

	[PunRPC]
	public void NET_SetTeamHeroCards(int _heroIndex, string _heroCards, string _upgradedCards)
	{
		List<string> list = new List<string>();
		list.AddRange(JsonHelper.FromJson<string>(_heroCards));
		teamAtO[_heroIndex].Cards = list;
		List<string> list2 = new List<string>();
		if (_upgradedCards != "")
		{
			list2.AddRange(JsonHelper.FromJson<string>(_upgradedCards));
		}
		upgradedCardsList = list2;
		SideBarRefreshCards(_heroIndex);
	}

	private void ClearNodeInfo()
	{
		ClearCurrentCombatData();
		ResetEventRewardTier();
	}

	public void SetPositionText(string position = "")
	{
		string text = "";
		text = ((!(position == "")) ? position : Globals.Instance.GetNodeData(currentMapNode).NodeName);
		if (text == "" && GameManager.Instance.IsObeliskChallenge())
		{
			text = ((!GameManager.Instance.IsWeeklyChallenge()) ? Texts.Instance.GetText("modeObelisk") : Texts.Instance.GetText("modeWeekly"));
		}
		SteamManager.Instance.SetRichPresence("location", text);
		SteamManager.Instance.SetRichPresence("steam_display", "#Status_Location");
	}

	public Enums.Zone GetMapZone(string _nodeName)
	{
		NodeData nodeData = Globals.Instance.GetNodeData(_nodeName);
		if (nodeData != null && nodeData.NodeZone != null)
		{
			string text = nodeData.NodeZone.ZoneId.ToLower();
			string[] names = Enum.GetNames(typeof(Enums.Zone));
			for (int i = 0; i < names.Length; i++)
			{
				if (names[i].ToLower() == text)
				{
					return (Enums.Zone)Enum.Parse(typeof(Enums.Zone), names[i]);
				}
			}
		}
		return Enums.Zone.None;
	}

	public bool IsZoneAffectedByMadness()
	{
		if (currentMapNode == "")
		{
			return true;
		}
		if (Globals.Instance.GetNodeData(currentMapNode) != null)
		{
			return !Globals.Instance.GetNodeData(currentMapNode).NodeZone.DisableMadnessOnThisZone;
		}
		return false;
	}

	public bool SetCurrentNode(string _nodeName, string _nodeNameUnlock = "", string _nodeObeliskIcon = "")
	{
		if (_nodeNameUnlock == "")
		{
			_nodeNameUnlock = _nodeName;
		}
		PlayerManager.Instance.NodeUnlock(_nodeNameUnlock);
		currentMapNode = _nodeName;
		SetTownZoneId(Globals.Instance.GetNodeData(currentMapNode).NodeZone.ZoneId);
		if (GameManager.Instance.IsWeeklyChallenge() && NodeIsObeliskFinal() && currentMapNode.Contains("_0"))
		{
			ChallengeData weeklyData = Globals.Instance.GetWeeklyData(weekly);
			if (weeklyData != null && weeklyData.IdSteam != "")
			{
				SteamManager.Instance.SetStatInt(weeklyData.IdSteam, 1);
			}
			weeklyData = Globals.Instance.GetWeeklyData(weekly, _getSecondary: true);
			if (weeklyData != null && weeklyData.IdSteam != "")
			{
				SteamManager.Instance.SetStatInt(weeklyData.IdSteam, 1);
			}
		}
		SetPositionText();
		if (GameManager.Instance.IsMultiplayer())
		{
			if (NetworkManager.Instance.IsMaster())
			{
				photonView.RPC("NET_SetCurrentNode", RpcTarget.Others, currentMapNode, _nodeNameUnlock, _nodeObeliskIcon);
			}
			else
			{
				StartCoroutine(EndCurrentNodeMP());
			}
		}
		ClearNodeInfo();
		if (!mapVisitedNodes.Contains(currentMapNode))
		{
			mapVisitedNodes.Add(currentMapNode);
			string text = currentMapNode + "|" + _nodeNameUnlock;
			text = ((!GameManager.Instance.IsObeliskChallenge() || !(_nodeObeliskIcon != "")) ? (text + "|") : (text + "|" + _nodeObeliskIcon));
			if (mapVisitedNodesAction == null)
			{
				mapVisitedNodesAction = new List<string>();
			}
			if (!mapVisitedNodesAction.Contains(text))
			{
				mapVisitedNodesAction.Add(text);
			}
			return true;
		}
		return false;
	}

	[PunRPC]
	private void NET_SetCurrentNode(string _nodeName, string _nodeNameUnlock, string _nodeObeliskIcon)
	{
		SetCurrentNode(_nodeName, _nodeNameUnlock, _nodeObeliskIcon);
	}

	private IEnumerator EndCurrentNodeMP()
	{
		NetworkManager.Instance.SetWaitingSyncro("waitingSetCurrentNode", status: true);
		NetworkManager.Instance.SetStatusReady("waitingSetCurrentNode");
		while (NetworkManager.Instance.WaitingSyncro.ContainsKey("waitingSetCurrentNode") && NetworkManager.Instance.WaitingSyncro["waitingSetCurrentNode"])
		{
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("waitingSetCurrentNode, we can continue!");
		}
	}

	public void GoToTown(string _nodeName)
	{
		currentMapNode = _nodeName;
		ClearReroll();
		if (!mapVisitedNodes.Contains(currentMapNode))
		{
			mapVisitedNodes.Add(currentMapNode);
			Debug.Log("Add to mapVisitedNodes " + currentMapNode);
			string text = currentMapNode + "|Town";
			if (!mapVisitedNodesAction.Contains(text))
			{
				Debug.Log("Add to mapVisitedNodesAction " + text);
				mapVisitedNodesAction.Add(text);
			}
		}
		UpgradeTownTier();
		if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
		{
			photonView.RPC("NET_GoToTown", RpcTarget.Others, currentMapNode);
		}
		SceneStatic.LoadByName("Town");
	}

	[PunRPC]
	private void NET_GoToTown(string _nodeName)
	{
		GoToTown(_nodeName);
	}

	public CombatData GetCurrentCombatData()
	{
		return currentCombatData;
	}

	public void ClearCurrentCombatData()
	{
		currentCombatData = null;
	}

	private void ReAssignNodeByRequeriment(string _requerimentId)
	{
		if ((GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster()) || MapManager.Instance == null)
		{
			return;
		}
		Dictionary<string, Node> mapNodeDict = MapManager.Instance.GetMapNodeDict();
		if (mapNodeDict == null)
		{
			return;
		}
		foreach (KeyValuePair<string, Node> item in mapNodeDict)
		{
			if (gameNodeAssigned.ContainsKey(item.Key) && mapVisitedNodes.Contains(item.Key))
			{
				continue;
			}
			Node value = item.Value;
			for (int i = 0; i < value.nodeData.NodeEvent.Length; i++)
			{
				if (value.nodeData.NodeEvent[i].Requirement != null && value.nodeData.NodeEvent[i].Requirement.RequirementId == _requerimentId && gameNodeAssigned.ContainsKey(value.nodeData.NodeId))
				{
					gameNodeAssigned.Remove(value.nodeData.NodeId);
					AssignSingleGameNode(value);
				}
			}
		}
	}

	public void AssignSingleGameNode(Node _node)
	{
		if (_node.nodeData.ExistsSku != "" && !SteamManager.Instance.PlayerHaveDLC(_node.nodeData.ExistsSku))
		{
			gameNodeAssigned.Remove(_node.nodeData.NodeId);
			return;
		}
		UnityEngine.Random.InitState((_node.nodeData.NodeId + GetGameId() + "AssignSingleGameNode").GetDeterministicHashCode());
		if (UnityEngine.Random.Range(0, 100) >= _node.nodeData.ExistsPercent)
		{
			gameNodeAssigned.Remove(_node.nodeData.NodeId);
			return;
		}
		bool flag = true;
		bool flag2 = true;
		if (_node.nodeData.NodeEvent != null && _node.nodeData.NodeEvent.Length != 0 && _node.nodeData.NodeCombat != null && _node.nodeData.NodeCombat.Length != 0)
		{
			if (UnityEngine.Random.Range(0, 100) < _node.nodeData.CombatPercent)
			{
				flag = false;
			}
			else
			{
				flag2 = false;
			}
		}
		if (_node.nodeData.GoToTown)
		{
			if (gameNodeAssigned.ContainsKey(_node.nodeData.NodeId))
			{
				gameNodeAssigned[_node.nodeData.NodeId] = "town:town";
			}
			else
			{
				gameNodeAssigned.Add(_node.nodeData.NodeId, "town:town");
			}
		}
		else if (_node.nodeData.TravelDestination)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("TravelDestination **** " + _node.nodeData.NodeId, "map");
			}
			if (gameNodeAssigned.ContainsKey(_node.nodeData.NodeId))
			{
				gameNodeAssigned[_node.nodeData.NodeId] = "destination:destination";
			}
			else
			{
				gameNodeAssigned.Add(_node.nodeData.NodeId, "destination:destination");
			}
		}
		else if (flag && _node.nodeData.NodeEvent != null && _node.nodeData.NodeEvent.Length != 0)
		{
			int num = 0;
			string text = "";
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			for (int i = 0; i < _node.nodeData.NodeEvent.Length; i++)
			{
				if (!(_node.nodeData.NodeEvent[i] != null))
				{
					continue;
				}
				for (int j = 0; j < _node.nodeData.NodeEvent[i].Replys.Length; j++)
				{
					EventReplyData eventReplyData = _node.nodeData.NodeEvent[i].Replys[j];
					if (eventReplyData.ChooseReplacementHero)
					{
						Hero[] team = GetTeam();
						if (j < team.Length)
						{
							SubClassData subClassData = Globals.Instance.GetSubClassData(team[j].SubclassName);
							eventReplyData.RequiredClass = subClassData;
						}
					}
				}
				bool flag3 = true;
				if (_node.nodeData.NodeEvent[i].Requirement != null && !PlayerHasRequirement(_node.nodeData.NodeEvent[i].Requirement))
				{
					flag3 = false;
				}
				if (_node.nodeData.NodeEvent[i].RequiredClass != null && !PlayerHasRequirementClass(_node.nodeData.NodeEvent[i].RequiredClass.Id))
				{
					flag3 = false;
				}
				if (flag3)
				{
					int value = 10000;
					if (i < _node.nodeData.NodeEventPriority.Length)
					{
						value = _node.nodeData.NodeEventPriority[i];
					}
					dictionary.Add(_node.nodeData.NodeEvent[i].EventId, value);
				}
			}
			if (dictionary.Count == 0)
			{
				return;
			}
			dictionary = dictionary.OrderBy((KeyValuePair<string, int> x) => x.Value).ToDictionary((KeyValuePair<string, int> x) => x.Key, (KeyValuePair<string, int> x) => x.Value);
			int num2 = 1;
			for (int value = dictionary.ElementAt(0).Value; num2 < dictionary.Count && dictionary.ElementAt(num2).Value == value; num2++)
			{
			}
			if (num2 == 1)
			{
				text = dictionary.ElementAt(0).Key;
			}
			else
			{
				if (_node.nodeData.NodeEventPercent != null && _node.nodeData.NodeEvent.Length == _node.nodeData.NodeEventPercent.Length)
				{
					Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
					int num3 = 0;
					for (int num4 = 0; num4 < num2; num4++)
					{
						int num5 = 0;
						while (num4 < _node.nodeData.NodeEvent.Length)
						{
							if (_node.nodeData.NodeEvent[num5].EventId == dictionary.ElementAt(num3).Key)
							{
								dictionary2.Add(_node.nodeData.NodeEvent[num5].EventId, _node.nodeData.NodeEventPercent[num5]);
								num3++;
								break;
							}
							num5++;
						}
					}
					int num6 = UnityEngine.Random.Range(0, 100);
					int num7 = 0;
					foreach (KeyValuePair<string, int> item in dictionary2)
					{
						num7 += item.Value;
						if (num6 < num7)
						{
							text = item.Key;
							break;
						}
					}
				}
				if (text == "")
				{
					num = UnityEngine.Random.Range(0, num2);
					text = dictionary.ElementAt(num).Key;
				}
			}
			if (gameNodeAssigned.ContainsKey(_node.nodeData.NodeId))
			{
				gameNodeAssigned[_node.nodeData.NodeId] = "event:" + text;
			}
			else
			{
				gameNodeAssigned.Add(_node.nodeData.NodeId, "event:" + text);
			}
		}
		else if (flag2 && _node.nodeData.NodeCombat != null && _node.nodeData.NodeCombat.Length != 0)
		{
			string text2 = "";
			if (GameManager.Instance.IsWeeklyChallenge() && (_node.nodeData.NodeId == "of1_10" || _node.nodeData.NodeId == "of2_10"))
			{
				ChallengeData weeklyData = Globals.Instance.GetWeeklyData(weekly);
				if (weeklyData != null && weeklyData.BossCombat != null)
				{
					text2 = weeklyData.BossCombat.CombatId;
				}
			}
			if (text2 == "")
			{
				int num8 = 0;
				if (_node.nodeData.NodeId == "of1_10" || _node.nodeData.NodeId == "of2_10")
				{
					UnityEngine.Random.State state = UnityEngine.Random.state;
					UnityEngine.Random.InitState((_node.nodeData.NodeId + GetGameId() + "finalBoss").GetDeterministicHashCode());
					num8 = UnityEngine.Random.Range(0, _node.nodeData.NodeCombat.Length);
					UnityEngine.Random.state = state;
				}
				text2 = _node.nodeData.NodeCombat[num8].CombatId;
			}
			if (gameNodeAssigned.ContainsKey(_node.nodeData.NodeId))
			{
				gameNodeAssigned[_node.nodeData.NodeId] = "combat:" + text2;
			}
			else
			{
				gameNodeAssigned.Add(_node.nodeData.NodeId, "combat:" + text2);
			}
		}
		else if (gameNodeAssigned.ContainsKey(_node.nodeData.NodeId))
		{
			gameNodeAssigned[_node.nodeData.NodeId] = "";
		}
		else
		{
			gameNodeAssigned.Add(_node.nodeData.NodeId, "");
		}
	}

	public TierRewardData GetTeamNPCReward()
	{
		int num = 0;
		for (int i = 0; i < teamNPCAtO.Length; i++)
		{
			if (teamNPCAtO[i] != null && teamNPCAtO[i] != "")
			{
				NPCData nPCData = Globals.Instance.GetNPC(teamNPCAtO[i]);
				if (nPCData != null && PlayerHasRequirement(Globals.Instance.GetRequirementData("_tier2")) && nPCData.UpgradedMob != null)
				{
					nPCData = nPCData.UpgradedMob;
				}
				if (nPCData != null && ngPlus > 0 && nPCData.NgPlusMob != null)
				{
					nPCData = nPCData.NgPlusMob;
				}
				if ((MadnessManager.Instance.IsMadnessTraitActive("despair") || Instance.IsChallengeTraitActive("despair")) && nPCData.HellModeMob != null)
				{
					nPCData = nPCData.HellModeMob;
				}
				if (nPCData != null && nPCData.TierReward != null && nPCData.TierReward.TierNum > num)
				{
					num = nPCData.TierReward.TierNum;
				}
			}
		}
		return Globals.Instance.GetTierRewardData(num);
	}

	public void ResetCombatScarab()
	{
		combatScarab = "";
	}

	public void FinishCardRewards(string[] arrRewards)
	{
		for (int i = 0; i < 4; i++)
		{
			if (teamAtO[i] == null || teamAtO[i].HeroData == null)
			{
				continue;
			}
			if (arrRewards[i] != "" && arrRewards[i] != "dust")
			{
				AddCardToHero(i, arrRewards[i]);
			}
			else if (arrRewards[i] == "dust")
			{
				int num = Globals.Instance.GetTierRewardData(currentRewardTier).Dust;
				if (GameManager.Instance.IsObeliskChallenge() && Globals.Instance.ZoneDataSource[GetTownZoneId().ToLower()].ObeliskLow)
				{
					num *= 2;
				}
				if (MadnessManager.Instance.IsMadnessTraitActive("poverty") || IsChallengeTraitActive("poverty"))
				{
					num = (GameManager.Instance.IsObeliskChallenge() ? (num - Functions.FuncRoundToInt((float)num * 0.3f)) : (num - Functions.FuncRoundToInt((float)num * 0.5f)));
				}
				if (IsChallengeTraitActive("prosperity"))
				{
					num += Functions.FuncRoundToInt((float)num * 0.5f);
				}
				GivePlayer(1, num, teamAtO[i].Owner);
			}
		}
		if (GameManager.Instance.IsObeliskChallenge() && mapNodeObeliskBoss.Contains(currentMapNode))
		{
			NodeData nodeData = Globals.Instance.GetNodeData(currentMapNode);
			if (nodeData.NodeCombatTier == Enums.CombatTier.T8)
			{
				DoLoot("challenge_boss_low");
			}
			else if (nodeData.NodeCombatTier == Enums.CombatTier.T9)
			{
				DoLoot("challenge_boss_high");
			}
			else if (townZoneId.ToLower() == obeliskLow)
			{
				DoLoot("challenge_chest_low");
			}
			else if (townZoneId.ToLower() == obeliskHigh)
			{
				DoLoot("challenge_chest_high");
			}
			else
			{
				DoLoot("challenge_chest_final");
			}
		}
		else if (GameManager.Instance.IsMultiplayer())
		{
			if (townDivinationTier != null)
			{
				StartCoroutine(ShareTeam("Town"));
			}
			else
			{
				StartCoroutine(ShareTeam("Map"));
			}
		}
		else if (townDivinationTier != null)
		{
			SceneStatic.LoadByName("Town");
		}
		else
		{
			SceneStatic.LoadByName("Map");
		}
	}

	public void FinishObeliskDraft()
	{
		Dictionary<int, List<string>> dictionary = new Dictionary<int, List<string>>();
		for (int i = 0; i < 4; i++)
		{
			List<string> cards = teamAtO[i].Cards;
			dictionary.Add(i, cards);
		}
		string[] array = new string[4];
		string[] array2 = new string[4];
		string[] array3 = new string[4];
		for (int j = 0; j < 4; j++)
		{
			if (teamAtO[j] != null && !(teamAtO[j].HeroData == null))
			{
				array[j] = teamAtO[j].HeroData.HeroSubClass.SubClassName;
				array2[j] = teamAtO[j].SkinUsed;
				array3[j] = teamAtO[j].CardbackUsed;
			}
		}
		SetTeamFromArray(array);
		for (int k = 0; k < 4; k++)
		{
			if (teamAtO[k] != null && !(teamAtO[k].HeroData == null))
			{
				teamAtO[k].SkinUsed = array2[k];
				teamAtO[k].CardbackUsed = array3[k];
				if (heroPerks.ContainsKey(array[k]))
				{
					teamAtO[k].PerkList = heroPerks[array[k]];
				}
				teamAtO[k].Cards = dictionary[k];
				AssignZeroTrait(teamAtO[k]);
			}
		}
		string text = "challenge_start";
		if (GameManager.Instance.IsWeeklyChallenge())
		{
			ChallengeData weeklyData = Globals.Instance.GetWeeklyData(Functions.GetCurrentWeeklyWeek());
			if (weeklyData != null && weeklyData.Loot != null && weeklyData.Loot.Id != "")
			{
				text = weeklyData.Loot.Id;
			}
		}
		DoLoot(text);
	}

	private void AssignZeroTrait(Hero hero)
	{
		if (hero != null && !(hero.HeroData == null) && !(hero.HeroData.HeroSubClass == null) && !(hero.HeroData.HeroSubClass.Trait0 == null))
		{
			hero.AssignTrait(hero.HeroData.HeroSubClass.Trait0.Id);
		}
	}

	public void SetObeliskLootReroll()
	{
		shopItemReroll = currentMapNode + gameId;
	}

	public void DoTownReroll()
	{
		shopItemReroll = Functions.RandomString(6f);
		SetTownReroll(NetworkManager.Instance.GetPlayerNick(), shopItemReroll);
	}

	public void SetTownReroll(string _nick, string _code, bool _sendCoop = true)
	{
		if (townRerollList == null)
		{
			townRerollList = new Dictionary<string, string>();
		}
		string playerNickReal = NetworkManager.Instance.GetPlayerNickReal(_nick);
		if (!townRerollList.ContainsKey(playerNickReal))
		{
			townRerollList.Add(playerNickReal, _code);
		}
		else
		{
			Dictionary<string, string> dictionary = townRerollList;
			string key = playerNickReal;
			dictionary[key] = dictionary[key] + "_" + _code;
		}
		if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
		{
			SaveGame();
		}
		if (_sendCoop && GameManager.Instance.IsMultiplayer())
		{
			photonView.RPC("NET_SetTownReroll", RpcTarget.Others, _nick, _code);
		}
	}

	[PunRPC]
	private void NET_SetTownReroll(string _nick, string _code)
	{
		SetTownReroll(_nick, _code, _sendCoop: false);
	}

	public bool IsTownRerollLimited()
	{
		if (sandbox_unlimitedRerolls)
		{
			return false;
		}
		if (!GameManager.Instance.IsObeliskChallenge() && GetNgPlus() > 3)
		{
			return true;
		}
		return false;
	}

	public bool IsTownRerollAvailable()
	{
		if (IsTownRerollLimited() && HowManyTownRerolls() > 0)
		{
			return false;
		}
		return true;
	}

	public int HowManyTownRerolls()
	{
		if (townRerollList == null)
		{
			townRerollList = new Dictionary<string, string>();
			return 0;
		}
		if (!GameManager.Instance.IsMultiplayer())
		{
			if (townRerollList.Count > 0)
			{
				return townRerollList.First().Value.Split("_").Length;
			}
		}
		else
		{
			string playerNickReal = NetworkManager.Instance.GetPlayerNickReal(NetworkManager.Instance.GetPlayerNick());
			if (townRerollList.ContainsKey(playerNickReal))
			{
				return townRerollList[playerNickReal].Split("_").Length;
			}
		}
		return 0;
	}

	private void TownRerollRestore()
	{
		if (townRerollList != null && townRerollList.ContainsKey(NetworkManager.Instance.GetMyNickReal()))
		{
			string[] array = townRerollList[NetworkManager.Instance.GetMyNickReal()].Split('_');
			if (array.Length != 0 && array[^1] != null)
			{
				shopItemReroll = array[^1];
			}
		}
	}

	public int GetExperienceFromCombat()
	{
		int num = 0;
		if (teamNPCAtO != null)
		{
			for (int i = 0; i < teamNPCAtO.Length; i++)
			{
				if (teamNPCAtO[i] != null && teamNPCAtO[i] != "")
				{
					NPCData nPCData = Globals.Instance.GetNPC(teamNPCAtO[i]);
					if (nPCData != null && PlayerHasRequirement(Globals.Instance.GetRequirementData("_tier2")) && nPCData.UpgradedMob != null)
					{
						nPCData = nPCData.UpgradedMob;
					}
					if (nPCData != null && ngPlus > 0 && nPCData.NgPlusMob != null)
					{
						nPCData = nPCData.NgPlusMob;
					}
					if ((MadnessManager.Instance.IsMadnessTraitActive("despair") || Instance.IsChallengeTraitActive("despair")) && nPCData.HellModeMob != null)
					{
						nPCData = nPCData.HellModeMob;
					}
					if (nPCData != null && nPCData.ExperienceReward > 0)
					{
						num += nPCData.ExperienceReward;
					}
				}
			}
		}
		return num;
	}

	public int GetGoldFromCombat()
	{
		int num = 0;
		if (teamNPCAtO != null)
		{
			for (int i = 0; i < teamNPCAtO.Length; i++)
			{
				if (teamNPCAtO[i] != null && teamNPCAtO[i] != "")
				{
					NPCData nPCData = Globals.Instance.GetNPC(teamNPCAtO[i]);
					if (nPCData != null && PlayerHasRequirement(Globals.Instance.GetRequirementData("_tier2")) && nPCData.UpgradedMob != null)
					{
						nPCData = nPCData.UpgradedMob;
					}
					if (nPCData != null && ngPlus > 0 && nPCData.NgPlusMob != null)
					{
						nPCData = nPCData.NgPlusMob;
					}
					if ((MadnessManager.Instance.IsMadnessTraitActive("despair") || Instance.IsChallengeTraitActive("despair")) && nPCData.HellModeMob != null)
					{
						nPCData = nPCData.HellModeMob;
					}
					if (nPCData != null && nPCData.GoldReward > 0)
					{
						num += nPCData.GoldReward;
					}
				}
			}
		}
		return num;
	}

	public void ClearCombatThermometerData()
	{
		combatThermometerData = null;
	}

	public void SetCombatExpertise(ThermometerData _thermometerData)
	{
		if (currentMapNode == "tutorial_0" || currentMapNode == "tutorial_1" || currentMapNode == "tutorial_2")
		{
			return;
		}
		if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
		{
			if (_thermometerData != null)
			{
				combatExpertise += _thermometerData.ExpertiseBonus;
			}
			else
			{
				combatExpertise = combatExpertise;
			}
			if (GameManager.Instance.IsMultiplayer())
			{
				photonView.RPC("NET_SetCombatExpertise", RpcTarget.Others, combatExpertise);
			}
		}
		combatTotal++;
	}

	[PunRPC]
	private void NET_SetCombatExpertise(int _combatExpertise)
	{
		combatExpertise = _combatExpertise;
	}

	public void SetCombatThermometerData(ThermometerData _thermometerData)
	{
		combatThermometerData = _thermometerData;
	}

	public ThermometerData GetCombatThermometerData()
	{
		return combatThermometerData;
	}

	public List<string> GetPlayerRequeriments()
	{
		return playerRequeriments;
	}

	public void SetPlayerRequeriments(List<string> _playerRequeriments)
	{
		playerRequeriments = _playerRequeriments;
	}

	private void AssignGlobalEventRequirements()
	{
		foreach (KeyValuePair<string, EventRequirementData> requirement in Globals.Instance.Requirements)
		{
			if (requirement.Value.AssignToPlayerAtBegin)
			{
				AddPlayerRequirement(requirement.Value, share: false);
			}
		}
		if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_6_4") && (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster()))
		{
			AddPlayerRequirement(Globals.Instance.GetRequirementData("caravan"), share: false);
		}
		if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
		{
			string[] array = playerRequeriments.ToArray();
			PhotonView obj = photonView;
			object[] parameters = array;
			obj.RPC("NET_AssignGlobalEventRequirements", RpcTarget.Others, parameters);
		}
	}

	[PunRPC]
	private void NET_AssignGlobalEventRequirements(string[] _requeriments)
	{
		playerRequeriments = _requeriments.ToList();
	}

	public void AddPlayerRequirementOthers(string _requirementId)
	{
		photonView.RPC("NET_AddPlayerRequirement", RpcTarget.Others, _requirementId);
	}

	[PunRPC]
	private void NET_AddPlayerRequirement(string _requirementId)
	{
		EventRequirementData requirementData = Globals.Instance.GetRequirementData(_requirementId);
		if (!playerRequeriments.Contains(requirementData.RequirementId))
		{
			playerRequeriments.Add(requirementData.RequirementId);
		}
	}

	public void AddPlayerRequirement(EventRequirementData requirement, bool share = true)
	{
		if (requirement.RequirementId == "_demo" && (!GameManager.Instance.IsDemo() || GameManager.Instance.GetDeveloperMode()))
		{
			return;
		}
		bool flag = false;
		if (!GameManager.Instance.IsObeliskChallenge() && (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster()))
		{
			flag = true;
		}
		if (!playerRequeriments.Contains(requirement.RequirementId))
		{
			playerRequeriments.Add(requirement.RequirementId);
			if (flag)
			{
				ReAssignNodeByRequeriment(requirement.RequirementId);
			}
		}
	}

	public void RemovePlayerRequirement(EventRequirementData requirement, string requirementId = "")
	{
		if (requirementId != "")
		{
			requirement = Globals.Instance.GetRequirementData(requirementId);
			if (requirement == null)
			{
				return;
			}
		}
		bool flag = false;
		if (!GameManager.Instance.IsObeliskChallenge() && (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster()))
		{
			flag = true;
		}
		if (playerRequeriments.Contains(requirement.RequirementId))
		{
			playerRequeriments.Remove(requirement.RequirementId);
			if (flag)
			{
				ReAssignNodeByRequeriment(requirement.RequirementId);
			}
			Debug.Log("-- Player removed requeriment [" + requirement.RequirementId + "] --");
		}
	}

	public void ShowPlayerRequirements()
	{
		for (int i = 0; i < playerRequeriments.Count; i++)
		{
			Debug.Log(i + ".- " + playerRequeriments[i]);
		}
	}

	public bool PlayerHasRequirement(EventRequirementData requirement, string requirementId = "")
	{
		if (requirement == null && requirementId == "")
		{
			return true;
		}
		if (requirement != null && playerRequeriments.Contains(requirement.RequirementId))
		{
			return true;
		}
		if (requirementId != "" && playerRequeriments.Contains(requirementId))
		{
			return true;
		}
		return false;
	}

	public bool PlayerHasRequirementClass(string _classId)
	{
		for (int i = 0; i < teamAtO.Length; i++)
		{
			if (teamAtO[i] != null && teamAtO[i].SubclassName == _classId.ToLower())
			{
				return true;
			}
		}
		return false;
	}

	public int PlayerHasRequirementItem(CardData requirementItem, SubClassData requiredSCD = null)
	{
		for (int i = 0; i < teamAtO.Length; i++)
		{
			if (teamAtO[i] != null && teamAtO[i].HasItemCardData(requirementItem) && (requiredSCD == null || teamAtO[i].SubclassName == requiredSCD.Id.ToLower()))
			{
				return i;
			}
		}
		return -1;
	}

	public bool PlayerHasRequirementCard(CardData requirementCard, SubClassData requiredSCD = null, bool checkUpgrades = true)
	{
		if (requirementCard != null)
		{
			for (int i = 0; i < teamAtO.Length; i++)
			{
				if (teamAtO[i] != null && teamAtO[i].HeroData != null && teamAtO[i].HasCardDataString(requirementCard.Id, checkUpgrades) && (requiredSCD == null || teamAtO[i].SubclassName == requiredSCD.Id.ToLower()))
				{
					return true;
				}
			}
		}
		return false;
	}

	public TierRewardData GetEventRewardTier()
	{
		return eventRewardTier;
	}

	public void SetEventRewardTier(TierRewardData tierNum)
	{
		eventRewardTier = tierNum;
	}

	public void ResetEventRewardTier()
	{
		eventRewardTier = null;
	}

	public bool SubClassDataHaveAnythingInSlot(Enums.ItemSlot _slot, SubClassData requiredSCD)
	{
		if (requiredSCD == null)
		{
			return false;
		}
		int num = -1;
		for (int i = 0; i < teamAtO.Length; i++)
		{
			if (teamAtO[i] != null && teamAtO[i].HeroData != null && teamAtO[i].SubclassName == requiredSCD.Id.ToLower())
			{
				num = i;
				break;
			}
		}
		if (num != -1 && _slot != Enums.ItemSlot.None)
		{
			if (_slot == Enums.ItemSlot.Weapon && teamAtO[num].Weapon != "")
			{
				return true;
			}
			if (_slot == Enums.ItemSlot.Armor && teamAtO[num].Armor != "")
			{
				return true;
			}
			if (_slot == Enums.ItemSlot.Jewelry && teamAtO[num].Jewelry != "")
			{
				return true;
			}
			if (_slot == Enums.ItemSlot.Accesory && teamAtO[num].Accesory != "")
			{
				return true;
			}
			if (_slot == Enums.ItemSlot.Pet && teamAtO[num].Pet != "")
			{
				Globals instance = Globals.Instance;
				if ((object)instance != null && instance.GetCardData(teamAtO[num].Pet)?.CardUpgraded == Enums.CardUpgraded.No)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void CorruptItemSlot(int _heroIndex, string _slot)
	{
		CardData cardData = null;
		if (_slot == "weapon" && teamAtO[_heroIndex].Weapon != "")
		{
			cardData = Functions.GetCardDataFromCardData(Globals.Instance.GetCardData(teamAtO[_heroIndex].Weapon, instantiate: false), "");
		}
		else if (_slot == "armor" && teamAtO[_heroIndex].Armor != "")
		{
			cardData = Functions.GetCardDataFromCardData(Globals.Instance.GetCardData(teamAtO[_heroIndex].Armor, instantiate: false), "");
		}
		else if (_slot == "jewelry" && teamAtO[_heroIndex].Jewelry != "")
		{
			cardData = Functions.GetCardDataFromCardData(Globals.Instance.GetCardData(teamAtO[_heroIndex].Jewelry, instantiate: false), "");
		}
		else if (_slot == "accesory" && teamAtO[_heroIndex].Accesory != "")
		{
			cardData = Functions.GetCardDataFromCardData(Globals.Instance.GetCardData(teamAtO[_heroIndex].Accesory, instantiate: false), "");
		}
		else if (_slot == "pet" && teamAtO[_heroIndex].Pet != "")
		{
			cardData = Functions.GetCardDataFromCardData(Globals.Instance.GetCardData(teamAtO[_heroIndex].Pet, instantiate: false), "");
		}
		if (cardData != null && cardData.UpgradesToRare != null)
		{
			if (!GameManager.Instance.IsMultiplayer())
			{
				AddItemToHero(_heroIndex, cardData.UpgradesToRare.Id);
			}
			else
			{
				AddItemToHeroMP(_heroIndex, cardData.UpgradesToRare.Id);
			}
		}
	}

	[PunRPC]
	public void AddItemToHero(int _heroIndex, string itemToAddName, string _itemListId = "")
	{
		CardData cardData = Globals.Instance.GetCardData(itemToAddName, instantiate: false);
		string cardId = itemToAddName;
		bool flag = false;
		int num = -1;
		string text = "";
		if (cardData != null)
		{
			if (cardData.CardType == Enums.CardType.Weapon)
			{
				num = 0;
				text = teamAtO[_heroIndex].Weapon;
			}
			else if (cardData.CardType == Enums.CardType.Armor)
			{
				num = 1;
				text = teamAtO[_heroIndex].Armor;
			}
			else if (cardData.CardType == Enums.CardType.Jewelry)
			{
				num = 2;
				text = teamAtO[_heroIndex].Jewelry;
			}
			else if (cardData.CardType == Enums.CardType.Accesory)
			{
				num = 3;
				text = teamAtO[_heroIndex].Accesory;
			}
			else if (cardData.CardType == Enums.CardType.Pet)
			{
				num = 4;
				text = teamAtO[_heroIndex].Pet;
			}
			else if (cardData.CardType == Enums.CardType.Enchantment)
			{
				teamAtO[_heroIndex].AssignEnchantment(itemToAddName);
			}
			else
			{
				teamAtO[_heroIndex].Corruption = "";
			}
			if (num > -1 && (!(text != "") || !(cardData.UpgradesToRare == null) || !(cardData.Id == text)) && (!(text != "") || !(cardData.UpgradesToRare != null) || !(cardData.UpgradesToRare.Id == text)))
			{
				if (cardData.Id == text)
				{
					if (cardData.UpgradesToRare != null)
					{
						itemToAddName = Functions.GetCardDataFromCardData(cardData, "RARE").Id;
					}
				}
				else if (text != "")
				{
					CardData cardData2 = Globals.Instance.GetCardData(text, instantiate: false);
					if (cardData2 != null && cardData2.BaseCard != "" && cardData.BaseCard != "" && cardData2.BaseCard.ToLower() == cardData.BaseCard.ToLower() && cardData2.UpgradesToRare != null)
					{
						cardData2 = Functions.GetCardDataFromCardData(cardData2, "RARE");
						if (cardData2 != null)
						{
							itemToAddName = cardData2.Id;
						}
					}
				}
				switch (num)
				{
				case 0:
					RemoveItemFromHero(_isHero: true, _heroIndex, "weapon");
					teamAtO[_heroIndex].Weapon = itemToAddName;
					break;
				case 1:
					RemoveItemFromHero(_isHero: true, _heroIndex, "armor");
					teamAtO[_heroIndex].Armor = itemToAddName;
					break;
				case 2:
					RemoveItemFromHero(_isHero: true, _heroIndex, "jewelry");
					teamAtO[_heroIndex].Jewelry = itemToAddName;
					break;
				case 3:
					RemoveItemFromHero(_isHero: true, _heroIndex, "accesory");
					teamAtO[_heroIndex].Accesory = itemToAddName;
					break;
				case 4:
					RemoveItemFromHero(_isHero: true, _heroIndex, "pet");
					teamAtO[_heroIndex].Pet = itemToAddName;
					break;
				}
				cardData = Globals.Instance.GetCardData(itemToAddName, instantiate: false);
				flag = true;
			}
			teamAtO[_heroIndex].ResetItemDataBySlotCache();
			teamAtO[_heroIndex].ClearCaches();
		}
		if ((!TownManager.Instance || cardData.CardType != Enums.CardType.Pet) && _itemListId != "TeamManagement")
		{
			PlayerManager.Instance.CardUnlock(itemToAddName);
		}
		if (flag)
		{
			if (cardData != null && cardData.Item != null)
			{
				ItemData item = cardData.Item;
				if (item.MaxHealth != 0)
				{
					teamAtO[_heroIndex].ModifyMaxHP(item.MaxHealth);
				}
				else
				{
					SideBarRefresh();
				}
			}
			teamAtO[_heroIndex].SetEvent(Enums.EventActivation.CharacterAssign, null, 0, itemToAddName);
			teamAtO[_heroIndex].ResetItemDataBySlotCache();
			teamAtO[_heroIndex].ClearCaches();
		}
		ClearCacheGlobalACModification();
		if (_itemListId != "" && _itemListId != "TeamManagement")
		{
			SaveBoughtItem(_heroIndex, _itemListId, cardId);
		}
		if (_itemListId != "TeamManagement" && (bool)TownManager.Instance && (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster()))
		{
			SaveGame();
		}
	}

	public void AddItemToHeroMP(int _heroIndex, string _itemName, string _itemListId = "")
	{
		photonView.RPC("AddItemToHero", RpcTarget.All, _heroIndex, _itemName, _itemListId);
	}

	public void RemoveItemFromHeroFromEvent(bool _isHero, int _heroIndex, string _slot, string _id = "")
	{
		if (!GameManager.Instance.IsMultiplayer())
		{
			RemoveItemFromHero(_isHero, _heroIndex, _slot, _id);
		}
		else
		{
			RemoveItemFromHeroMP(_isHero, _heroIndex, _slot, _id);
		}
	}

	public void RemoveItemFromHeroMP(bool _isHero, int _heroIndex, string _slot, string _id = "")
	{
		photonView.RPC("RemoveItemFromHero", RpcTarget.All, _isHero, _heroIndex, _slot, _id);
	}

	[PunRPC]
	public void RemoveItemFromHero(bool _isHero, int _heroIndex, string _slot, string _id = "")
	{
		NPC[] array = null;
		Character character = null;
		if (!_isHero)
		{
			if (!MatchManager.Instance)
			{
				return;
			}
			array = MatchManager.Instance.GetTeamNPC();
			if (array == null || _heroIndex >= array.Length)
			{
				return;
			}
			character = array[_heroIndex];
		}
		else
		{
			character = teamAtO[_heroIndex];
		}
		string text = _id.ToLower();
		if (text != "")
		{
			if (character.Weapon == text)
			{
				character.Weapon = "";
			}
			if (character.Armor == text)
			{
				character.Armor = "";
			}
			if (character.Jewelry == text)
			{
				character.Jewelry = "";
			}
			if (character.Accesory == text)
			{
				character.Accesory = "";
			}
			if (character.Pet == text)
			{
				character.Pet = "";
			}
			if (character.Enchantment == text)
			{
				if (_isHero)
				{
					if (character.HeroItem != null)
					{
						MatchManager.Instance.RemovePetEnchantment(character.HeroItem.gameObject, text);
					}
				}
				else if (character.NPCItem != null)
				{
					MatchManager.Instance.RemovePetEnchantment(character.NPCItem.gameObject, text);
				}
				character.Enchantment = "";
				character.ReorganizeEnchantments();
			}
			if (character.Enchantment2 == text)
			{
				if (_isHero)
				{
					if (character.HeroItem != null)
					{
						MatchManager.Instance.RemovePetEnchantment(character.HeroItem.gameObject, text);
					}
				}
				else if (character.NPCItem != null)
				{
					MatchManager.Instance.RemovePetEnchantment(character.NPCItem.gameObject, text);
				}
				character.Enchantment2 = "";
				character.ReorganizeEnchantments();
			}
			if (character.Enchantment3 == text)
			{
				if (_isHero)
				{
					if (character.HeroItem != null)
					{
						MatchManager.Instance.RemovePetEnchantment(character.HeroItem.gameObject, text);
					}
				}
				else if (character.NPCItem != null)
				{
					MatchManager.Instance.RemovePetEnchantment(character.NPCItem.gameObject, text);
				}
				character.Enchantment3 = "";
				character.ReorganizeEnchantments();
			}
		}
		else
		{
			switch (_slot)
			{
			case "weapon":
				if (character.Weapon != "")
				{
					text = character.Weapon;
					character.Weapon = "";
				}
				break;
			case "armor":
				if (character.Armor != "")
				{
					text = character.Armor;
					character.Armor = "";
				}
				break;
			case "jewelry":
				if (character.Jewelry != "")
				{
					text = character.Jewelry;
					character.Jewelry = "";
				}
				break;
			case "accesory":
				if (character.Accesory != "")
				{
					text = character.Accesory;
					character.Accesory = "";
				}
				break;
			case "pet":
				if (character.Pet != "")
				{
					text = character.Pet;
					character.Pet = "";
				}
				break;
			case "enchantment":
				if (character.Enchantment != "")
				{
					text = character.Enchantment;
					character.Enchantment = "";
				}
				break;
			case "enchantment2":
				if (character.Enchantment2 != "")
				{
					text = character.Enchantment2;
					character.Enchantment2 = "";
				}
				break;
			case "enchantment3":
				if (character.Enchantment3 != "")
				{
					text = character.Enchantment3;
					character.Enchantment3 = "";
				}
				break;
			default:
				if (character.Corruption != "")
				{
					text = character.Corruption;
					character.Corruption = "";
				}
				break;
			}
		}
		if (text != "")
		{
			ItemData itemData = Globals.Instance.GetItemData(text);
			if (itemData != null && itemData.MaxHealth != 0)
			{
				character.DecreaseMaxHP(itemData.MaxHealth);
			}
		}
		character.ResetItemDataBySlotCache();
		character.ClearCaches();
		ClearCacheGlobalACModification();
	}

	public void AddPerkToHeroGlobal(int _heroIndex, string _perkId)
	{
		AddPerkToHero(_heroIndex, _perkId);
		if (GameManager.Instance.IsMultiplayer())
		{
			photonView.RPC("NET_AddPerkToHero", RpcTarget.Others, _heroIndex, _perkId);
		}
	}

	public void AddPerkToHeroGlobalList(int _heroIndex, List<string> _perkIdList)
	{
		AddPerkToHeroList(_heroIndex, _perkIdList);
		if (GameManager.Instance.IsMultiplayer())
		{
			string text = JsonHelper.ToJson(_perkIdList.ToArray());
			photonView.RPC("NET_AddPerkToHeroGlobalList", RpcTarget.Others, _heroIndex, text);
		}
	}

	[PunRPC]
	private void NET_AddPerkToHeroGlobalList(int _heroIndex, string _perksJson)
	{
		List<string> perkIdList = JsonHelper.FromJson<string>(_perksJson).ToList();
		AddPerkToHeroList(_heroIndex, perkIdList);
	}

	private void AddPerkToHeroList(int _heroIndex, List<string> _perkIdList)
	{
		string subclassName = teamAtO[_heroIndex].SubclassName;
		if (heroPerks.ContainsKey(subclassName))
		{
			heroPerks[subclassName] = new List<string>();
		}
		if (CharInTown() && GetTownTier() == 0)
		{
			teamAtO[_heroIndex].InitHP();
		}
		for (int i = 0; i < _perkIdList.Count; i++)
		{
			AddPerkToHero(_heroIndex, _perkIdList[i]);
		}
		if (CharInTown() && GetTownTier() == 0)
		{
			teamAtO[_heroIndex].ModifyMaxHP(teamAtO[_heroIndex].GetItemsMaxHPModifier());
			SideBarRefresh();
		}
	}

	[PunRPC]
	private void NET_AddPerkToHero(int _heroIndex, string _perkId)
	{
		AddPerkToHero(_heroIndex, _perkId);
	}

	public void AddPerkToHero(int _heroIndex, string _perkId, bool _initHealth = true)
	{
		_perkId = _perkId.ToLower();
		PerkData perkData = Globals.Instance.GetPerkData(_perkId);
		if (perkData != null)
		{
			string subclassName = teamAtO[_heroIndex].SubclassName;
			if (!heroPerks.ContainsKey(subclassName))
			{
				heroPerks.Add(subclassName, new List<string>());
			}
			List<string> list = heroPerks[subclassName];
			if (list == null)
			{
				list = new List<string>();
			}
			if (!list.Contains(perkData.Id))
			{
				list.Add(perkData.Id);
			}
			heroPerks[subclassName] = list;
			teamAtO[_heroIndex].PerkList = heroPerks[subclassName];
			if (_initHealth && perkData.MaxHealth != 0)
			{
				teamAtO[_heroIndex].ModifyMaxHP(perkData.MaxHealth);
			}
			teamAtO[_heroIndex].InitEnergy();
			teamAtO[_heroIndex].InitSpeed();
			teamAtO[_heroIndex].InitResist();
		}
	}

	public void ClearReroll()
	{
		shopItemReroll = "";
		if (townRerollList != null)
		{
			townRerollList.Clear();
		}
		else
		{
			townRerollList = new Dictionary<string, string>();
		}
	}

	public void RemoveItemList(bool keepPets)
	{
		shopList = null;
		boughtItems = null;
		if (boughtItemInShopByWho == null || boughtItemInShopByWho.Count <= 0)
		{
			return;
		}
		if (keepPets)
		{
			List<string> list = new List<string>();
			foreach (string key in boughtItemInShopByWho.Keys)
			{
				if (!key.StartsWith("petShop"))
				{
					list.Add(key);
				}
			}
			{
				foreach (string item in list)
				{
					boughtItemInShopByWho.Remove(item);
				}
				return;
			}
		}
		boughtItemInShopByWho.Clear();
	}

	public List<string> GetItemList(string itemListId)
	{
		string text = itemListId + "_" + gameId;
		if (GameManager.Instance.IsObeliskChallenge())
		{
			SetObeliskLootReroll();
		}
		if (currentMapNode == "dream_1")
		{
			text += "d";
		}
		if (shopItemReroll != "")
		{
			text = text + "_" + shopItemReroll;
		}
		if (shopList == null)
		{
			shopList = new Dictionary<string, List<string>>();
		}
		if (shopList.ContainsKey(text))
		{
			return shopList[text];
		}
		List<string> lootItems = Loot.GetLootItems(itemListId, shopItemReroll);
		shopList.Add(text, lootItems);
		return lootItems;
	}

	public void DoLeachExplosion(Character target)
	{
		if (target.IsHero)
		{
			return;
		}
		Hero[] array = teamAtO;
		foreach (Hero hero in array)
		{
			if (hero != null && hero.Alive)
			{
				hero.SetEvent(Enums.EventActivation.OnLeechExplosion, target);
			}
		}
	}

	public void RaisePetActivationEvent(Character caster, Character target, string petName, int auxInt, bool fromPlayerCard = false)
	{
		if (petName.IsNullOrEmpty())
		{
			return;
		}
		Hero[] array = teamAtO;
		foreach (Hero hero in array)
		{
			if (hero != null && hero.Alive)
			{
				if (fromPlayerCard)
				{
					hero.SetEvent(Enums.EventActivation.PetActivatedByCard, target, auxInt, petName, caster);
				}
				hero.SetEvent(Enums.EventActivation.PetActivatedAuto, target, auxInt, petName, caster);
			}
		}
	}

	public void SaveBoughtItem(int _heroIndex, string _shopId, string _cardId)
	{
		if (boughtItems == null)
		{
			boughtItems = new Dictionary<string, List<string>>();
		}
		string text = _shopId + "_" + mapVisitedNodes.Count();
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("SaveBoughtItem => " + text);
		}
		if (!boughtItems.ContainsKey(text))
		{
			boughtItems.Add(text, new List<string>());
		}
		if (boughtItems[text].Contains(_cardId))
		{
			return;
		}
		boughtItems[text].Add(_cardId);
		string key = _shopId + _cardId;
		if (boughtItemInShopByWho == null)
		{
			boughtItemInShopByWho = new Dictionary<string, int>();
		}
		if (!boughtItemInShopByWho.ContainsKey(key))
		{
			boughtItemInShopByWho.Add(key, _heroIndex);
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			if (NetworkManager.Instance.IsMaster())
			{
				photonView.RPC("NET_SaveBoughtItem", RpcTarget.Others, _heroIndex, _shopId, _cardId);
			}
			else
			{
				photonView.RPC("NET_SaveBoughtItem", RpcTarget.MasterClient, _heroIndex, _shopId, _cardId);
			}
		}
		if (CardCraftManager.Instance != null && CardCraftManager.Instance.craftType == 4)
		{
			CardCraftManager.Instance.ShowItemsForBuy(CardCraftManager.Instance.CurrentItemsPageNum, _cardId);
		}
	}

	[PunRPC]
	private void NET_SaveBoughtItem(int _heroIndex, string _shopId, string _cardId)
	{
		SaveBoughtItem(_heroIndex, _shopId, _cardId);
	}

	public bool ItemBoughtOnThisShop(string _shopId, string _cardId)
	{
		if (boughtItems == null)
		{
			boughtItems = new Dictionary<string, List<string>>();
		}
		string key = _shopId + "_" + mapVisitedNodes.Count();
		if (!boughtItems.ContainsKey(key))
		{
			boughtItems.Add(key, new List<string>());
		}
		if (boughtItems[key].Contains(_cardId))
		{
			return true;
		}
		return false;
	}

	public int WhoBoughtOnThisShop(string _shopId, string _cardId)
	{
		if (boughtItems == null)
		{
			boughtItems = new Dictionary<string, List<string>>();
		}
		string key = _shopId + "_" + mapVisitedNodes.Count();
		if (!boughtItems.ContainsKey(key))
		{
			boughtItems.Add(key, new List<string>());
		}
		if (boughtItems[key].Contains(_cardId))
		{
			string key2 = _shopId + _cardId;
			if (boughtItemInShopByWho == null)
			{
				boughtItemInShopByWho = new Dictionary<string, int>();
			}
			if (boughtItemInShopByWho.ContainsKey(key2))
			{
				return boughtItemInShopByWho[key2];
			}
		}
		return -1;
	}

	public void SaveUpgradedCardInAltar(int heroIndex, bool fromCoop = false)
	{
		if (upgradedCardsInAltarByWho == null)
		{
			upgradedCardsInAltarByWho = new Dictionary<string, int>();
		}
		string key = $"{heroIndex}{townZoneId}{townTier}{currentMapNode}";
		if (!upgradedCardsInAltarByWho.ContainsKey(key))
		{
			upgradedCardsInAltarByWho.Add(key, 1);
		}
		else
		{
			upgradedCardsInAltarByWho[key]++;
		}
		if (GameManager.Instance.IsMultiplayer() && !fromCoop)
		{
			photonView.RPC("NET_SaveUpgradedCardInAltar", RpcTarget.Others, heroIndex);
		}
	}

	[PunRPC]
	private void NET_SaveUpgradedCardInAltar(int heroIndex)
	{
		SaveUpgradedCardInAltar(heroIndex, fromCoop: true);
	}

	public int HowManyUpgradedCardsInAltar(int heroIndex)
	{
		if (upgradedCardsInAltarByWho == null)
		{
			return 0;
		}
		string key = $"{heroIndex}{townZoneId}{townTier}{currentMapNode}";
		if (upgradedCardsInAltarByWho.ContainsKey(key))
		{
			return upgradedCardsInAltarByWho[key];
		}
		return 0;
	}

	public void DoLoot(string _lootListId)
	{
		lootListId = _lootListId;
		if (GameManager.Instance.IsMultiplayer())
		{
			StartCoroutine(ShareTeam("Loot"));
		}
		else
		{
			SceneStatic.LoadByName("Loot");
		}
	}

	[PunRPC]
	private void NET_ShareLootId(string _lootListId)
	{
		lootListId = _lootListId;
		NetworkManager.Instance.SetStatusReady("dolootco");
	}

	public void SetLootListId(string _lootListId)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("SetLootListId ->" + _lootListId);
		}
		lootListId = _lootListId;
	}

	public string GetLootListId()
	{
		if (lootListId != "" && lootListId != null)
		{
			return lootListId;
		}
		return "";
	}

	public void DoCardPlayerGame(CardPlayerPackData _packData)
	{
		cardPlayerPackData = _packData;
		if (GameManager.Instance.IsMultiplayer())
		{
			StartCoroutine(ShareTeam("CardPlayer"));
		}
		else
		{
			SceneStatic.LoadByName("CardPlayer");
		}
	}

	public void DoCardPlayerPairsGame(CardPlayerPairsPackData _packData)
	{
		cardPlayerPairsPackData = _packData;
		if (GameManager.Instance.IsMultiplayer())
		{
			StartCoroutine(ShareTeam("CardPlayerPairs"));
		}
		else
		{
			SceneStatic.LoadByName("CardPlayerPairs");
		}
	}

	public void FinishCardPlayer()
	{
		cardPlayerPackData = null;
		cardPlayerPairsPackData = null;
		if (GameManager.Instance.IsMultiplayer())
		{
			StartCoroutine(ShareTeam("Map"));
		}
		else
		{
			SceneStatic.LoadByName("Map");
		}
	}

	public void FinishLoot()
	{
		lootListId = "";
		if (GameManager.Instance.IsMultiplayer())
		{
			StartCoroutine(ShareTeam("Map"));
		}
		else
		{
			SceneStatic.LoadByName("Map");
		}
	}

	public List<int> GetLootCharacterOrder()
	{
		lootCharacterOrder = new List<int>();
		lootCharacterOrder.Add(0);
		lootCharacterOrder.Add(1);
		lootCharacterOrder.Add(2);
		lootCharacterOrder.Add(3);
		lootCharacterOrder.Shuffle();
		return lootCharacterOrder;
	}

	public TierRewardData GetTownDivinationTier()
	{
		return townDivinationTier;
	}

	public void SetTownDivinationTier(int tierNum, string nickCreator = "", int divinationCost = 0)
	{
		if (townDivinationTier == null)
		{
			townDivinationTier = Globals.Instance.GetTierRewardData(tierNum);
			townDivinationCreator = nickCreator;
			townDivinationCost = divinationCost;
			if (GameManager.Instance.IsMultiplayer())
			{
				photonView.RPC("NET_SetTownDivination", RpcTarget.MasterClient, tierNum, nickCreator, divinationCost);
			}
		}
	}

	[PunRPC]
	private void NET_SetTownDivination(int tierNum, string nickCreator, int divinationCost)
	{
		if (!(townDivinationTier != null) || !(townDivinationCreator != nickCreator))
		{
			townDivinationTier = Globals.Instance.GetTierRewardData(tierNum);
			townDivinationCreator = nickCreator;
			townDivinationCost = divinationCost;
			NetworkManager.Instance.ClearAllPlayerDivinationReady();
			photonView.RPC("NET_InviteToTownDivination", RpcTarget.All, tierNum, nickCreator);
		}
	}

	[PunRPC]
	private void NET_InviteToTownDivination(int tierNum, string nickCreator)
	{
		townDivinationTier = Globals.Instance.GetTierRewardData(tierNum);
		townDivinationCreator = nickCreator;
		TownManager.Instance.ShowJoinDivination();
	}

	public void JoinCardDivination()
	{
		TownManager.Instance.DisableReady();
		TownManager.Instance.characterWindow.Hide(goToDivination: true);
		TownManager.Instance.ShowJoinDivination(state: false);
		DoCardDivination();
		photonView.RPC("NET_JoinTownDivination", RpcTarget.MasterClient, NetworkManager.Instance.GetPlayerNick());
	}

	[PunRPC]
	private void NET_JoinTownDivination(string nickPlayer)
	{
		NetworkManager.Instance.SetPlayerDivinationReady(nickPlayer);
	}

	public void UpdateDivinationStatus()
	{
		if (cardCraftManager != null)
		{
			cardCraftManager.RefreshDivinationWaiting();
		}
	}

	public void ResetTownDivination()
	{
		townDivinationTier = null;
		townDivinationCreator = "";
		townDivinationCost = 0;
	}

	public void SetCombatData(CombatData _combatData)
	{
		SetTeamNPCFromCombatData(_combatData);
		currentCombatData = _combatData;
	}

	public void LaunchCombat(CombatData _combatData = null)
	{
		if (_combatData != null)
		{
			SetCombatData(_combatData);
			if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
			{
				photonView.RPC("NET_SetCombatData", RpcTarget.Others, _combatData.CombatId, corruptionAccepted, corruptionId);
			}
		}
		if (fromEventCombatData == null)
		{
			fromEventCombatData = _combatData;
			SaveGame();
		}
		if (!Instance.IsCombatTool)
		{
			MapManager.Instance.CleanFromEvent();
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			StartCoroutine(ShareTeam("Combat"));
		}
		else
		{
			SceneStatic.LoadByName("Combat");
		}
	}

	[PunRPC]
	public void NET_SetCombatData(string _combatId, bool _corruptionAccepted, string _corruptionId)
	{
		currentCombatData = Globals.Instance.GetCombatData(_combatId);
		corruptionAccepted = _corruptionAccepted;
		corruptionId = _corruptionId;
	}

	public void LaunchRewards(bool isFromDivination = false)
	{
		if (IsCombatTool)
		{
			SceneStatic.LoadByName("HeroSelection");
			Instance.ClearGame();
		}
		else if (!GameManager.Instance.IsMultiplayer())
		{
			SceneStatic.LoadByName("Rewards");
		}
		else if (NetworkManager.Instance.IsMaster())
		{
			if (isFromDivination)
			{
				GivePlayer(0, -townDivinationCost, townDivinationCreator);
			}
			StartCoroutine(ShareTeam("Rewards"));
		}
	}

	public void RelaunchRewards()
	{
		if (!GameManager.Instance.IsMultiplayer())
		{
			SceneStatic.LoadByName("Rewards");
		}
		else
		{
			StartCoroutine(ShareTeam("Rewards"));
		}
	}

	public void FinishRun()
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			StartCoroutine(ShareTeam("FinishRun"));
		}
		else
		{
			SceneStatic.LoadByName("FinishRun");
		}
	}

	public void FinishObeliskMap()
	{
		if (GameManager.Instance.IsObeliskChallenge())
		{
			ZoneData zoneData = Globals.Instance.ZoneDataSource[GetTownZoneId().ToLower()];
			if (zoneData != null && zoneData.ObeliskLow)
			{
				MapManager.Instance.TravelToThisNode(MapManager.Instance.GetNodeFromId(obeliskHigh + "_0"));
				UpgradeTownTier();
			}
		}
	}

	public void SetNgPlus(int _ngPlus)
	{
		ngPlus = _ngPlus;
	}

	public int GetNgPlus(bool _fromSave = false)
	{
		if (!IsZoneAffectedByMadness() && !_fromSave)
		{
			return 0;
		}
		return ngPlus;
	}

	public void SetObeliskMadness(int _value)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("*** SET OBELISK MADNESS **** " + _value, "trace");
		}
		obeliskMadness = _value;
		if (GameManager.Instance.IsObeliskChallenge() && !GameManager.Instance.IsWeeklyChallenge())
		{
			SetObeliskMadnessTraits(obeliskMadness);
		}
	}

	public int GetObeliskMadness()
	{
		return obeliskMadness;
	}

	public void SetSingularityMadness(int _value)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("*** SET SINGULARITY MADNESS **** " + _value, "trace");
		}
		singularityMadness = _value;
		if (GameManager.Instance.IsSingularity())
		{
			SetSingularityMadnessTraits(singularityMadness);
		}
	}

	public int GetSingularityMadness()
	{
		return singularityMadness;
	}

	public void SetMadnessCorruptors(string corruptors)
	{
		madnessCorruptors = corruptors;
	}

	public string GetMadnessCorruptors()
	{
		return madnessCorruptors;
	}

	public int GetMadnessDifficulty()
	{
		return MadnessManager.Instance.CalculateMadnessTotal(ngPlus, madnessCorruptors);
	}

	public void SetAdventureCompleted(bool state)
	{
		adventureCompleted = state;
	}

	public bool IsAdventureCompleted()
	{
		return adventureCompleted;
	}

	public void FinishGame()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("** FinishGame **", "trace");
		}
		SetAdventureCompleted(state: true);
		PlayerManager.Instance.AchievementUnlock("MISC_HERO");
		if (GameManager.Instance.IsGameAdventure())
		{
			if (ngPlus > 0)
			{
				PlayerManager.Instance.AchievementUnlock("MISC_LEGENDARYHERO");
			}
			PlayerManager.Instance.IncreaseNg();
			if (ngPlus == 0)
			{
				SceneStatic.LoadByName("IntroNewGame");
			}
			else
			{
				SceneStatic.LoadByName("FinishRun");
			}
		}
		else if (GameManager.Instance.IsSingularity())
		{
			if (singularityMadness > 0)
			{
				PlayerManager.Instance.AchievementUnlock("MISC_LEGENDARYHERO");
			}
			PlayerManager.Instance.IncreaseSingularityMadness();
			if (singularityMadness == 0)
			{
				SceneStatic.LoadByName("IntroNewGame");
			}
			else
			{
				SceneStatic.LoadByName("FinishRun");
			}
		}
	}

	public void FinishObeliskChallenge()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("//FinishObelisk", "trace");
		}
		SetAdventureCompleted(state: true);
		if (!GameManager.Instance.IsWeeklyChallenge())
		{
			PlayerManager.Instance.IncreaseObeliskMadness();
		}
		FinishRun();
	}

	public void FinishCombat(bool resigned)
	{
		if (gameId == "" && GameManager.Instance.GetDeveloperMode())
		{
			SceneStatic.LoadByName("TeamManagement");
			return;
		}
		if (IsCombatTool)
		{
			SceneStatic.LoadByName("HeroSelection");
			Instance.ClearGame();
			return;
		}
		combatResigned = resigned;
		if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
		{
			return;
		}
		bool flag = true;
		if (!combatResigned)
		{
			for (int i = 0; i < 4; i++)
			{
				if (teamAtO[i] != null && teamAtO[i].HeroData != null && teamAtO[i].Alive)
				{
					flag = false;
					break;
				}
			}
		}
		if (!flag)
		{
			for (int j = 0; j < 4; j++)
			{
				if (teamAtO[j] == null || teamAtO[j].HeroData == null)
				{
					continue;
				}
				teamAtO[j].ResetDataForNewCombat();
				if (!teamAtO[j].Alive)
				{
					teamAtO[j].Alive = true;
					teamAtO[j].HpCurrent = Functions.FuncRoundToInt(teamAtO[j].Hp * 70 / 100);
					teamAtO[j].TotalDeaths++;
					if (teamAtO[j].Cards != null)
					{
						AddCardToHero(j, "DeathsDoor");
					}
				}
			}
			PopupManager.Instance.ClosePopup();
			if (currentMapNode == "tutorial_1")
			{
				if (!combatResigned)
				{
					LaunchMap();
				}
				else
				{
					FinishRun();
				}
			}
			else if (CinematicId != "")
			{
				LaunchCinematic();
			}
			else if (currentMapNode == "of1_10" || currentMapNode == "of2_10")
			{
				SetCombatCorruptionForScore();
				ClearCorruption();
				LaunchMap();
			}
			else if (combatThermometerData != null)
			{
				if (combatThermometerData.CardReward)
				{
					LaunchRewards();
				}
				else
				{
					LaunchMap();
				}
			}
			else
			{
				LaunchMap();
			}
		}
		else
		{
			FinishRun();
		}
	}

	public void DoCardUpgrade(int discount = 0, int maxQuantity = -1)
	{
		if (maxQuantity < 1 && GameManager.Instance.IsSingularity())
		{
			maxQuantity = 1;
		}
		else if (maxQuantity == 0)
		{
			maxQuantity = -1;
		}
		StartCoroutine(DoCraftHealerCo(0, discount, maxQuantity));
	}

	public void DoCardHealer(int discount = 0, int maxQuantity = -1)
	{
		if (maxQuantity == 0)
		{
			maxQuantity = -1;
		}
		StartCoroutine(DoCraftHealerCo(1, discount, maxQuantity));
	}

	public void DoCardCorruption(int discount = 0, int maxQuantity = -1)
	{
		if (maxQuantity == 0)
		{
			maxQuantity = -1;
		}
		StartCoroutine(DoCraftHealerCo(6, discount, maxQuantity));
	}

	public void DoItemCorrupt(int discount, int maxQuantity = -1)
	{
		if (maxQuantity == 0)
		{
			maxQuantity = -1;
		}
		StartCoroutine(DoCraftHealerCo(7, discount, maxQuantity));
	}

	public void DoCardCraft(int discount = 0, int maxQuantity = -1, Enums.CardRarity maxCraftRarity = Enums.CardRarity.Common)
	{
		if (maxQuantity == 0)
		{
			maxQuantity = -1;
		}
		StartCoroutine(DoCraftHealerCo(2, discount, maxQuantity, "", maxCraftRarity));
	}

	public void DoCardDivination(int discount = 0, int maxQuantity = -1)
	{
		if (maxQuantity == 0)
		{
			maxQuantity = -1;
		}
		StartCoroutine(DoCraftHealerCo(3, discount, maxQuantity));
	}

	public void DoItemShop(string shopListId, int discount = 0)
	{
		StartCoroutine(DoCraftHealerCo(4, discount, -1, shopListId));
	}

	public void DoChallengeShop()
	{
		StartCoroutine(DoCraftHealerCo(5));
	}

	public string GetTownItemListId()
	{
		string text = "towntier" + GetTownTier();
		if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_5_6"))
		{
			text += "_b";
		}
		else if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_5_3"))
		{
			text += "_a";
		}
		return text;
	}

	public void GenerateTownItemList()
	{
		string townItemListId = GetTownItemListId();
		GetItemList(townItemListId);
	}

	private IEnumerator DoCraftHealerCo(int type = 0, int discount = 0, int maxQuantity = -1, string itemListId = "", Enums.CardRarity maxCraftRarity = Enums.CardRarity.Common)
	{
		if (cardCraftManager != null)
		{
			CloseCardCraft();
			yield return Globals.Instance.WaitForSeconds(0.1f);
		}
		if ((bool)MapManager.Instance)
		{
			MapManager.Instance.ShowMask(state: false);
		}
		cardcraftGO = UnityEngine.Object.Instantiate(cardcratPrefab, new Vector3(0f, 0f, -2f), Quaternion.identity);
		cardCraftManager = cardcraftGO.GetComponent<CardCraftManager>();
		cardCraftManager.SetDiscount(discount);
		if (type != 3)
		{
			if (itemListId == "" && TownManager.Instance != null)
			{
				itemListId = GetTownItemListId();
			}
			cardCraftManager.SetItemList(itemListId);
		}
		cardCraftManager.ShowCardCraft(type);
		if (type != 3)
		{
			cardCraftManager.SetMaxQuantity(maxQuantity);
			cardCraftManager.SetMaxCraftRarity(maxCraftRarity);
		}
		yield return null;
	}

	public void CloseCardCraft()
	{
		UnityEngine.Object.Destroy(cardcraftGO);
		cardcraftGO = null;
		cardCraftManager = null;
		if ((bool)MapManager.Instance)
		{
			AudioManager.Instance.DoBSO("Map");
			MapManager.Instance.CloseEventFromEvent();
			MapManager.Instance.sideCharacters.ResetCharacters();
			MapManager.Instance.sideCharacters.InCharacterScreen(state: false);
			SaveManager.SavePlayerData();
		}
		else if ((bool)TownManager.Instance)
		{
			TownManager.Instance.sideCharacters.ResetCharacters();
			TownManager.Instance.ShowButtons(state: true);
		}
		PopupManager.Instance.ClosePopup();
		GameManager.Instance.CleanTempContainer();
	}

	public List<int> GetConflictResolutionOrder()
	{
		if (conflictCharacterOrder == null)
		{
			conflictCharacterOrder = new List<int>();
		}
		if (conflictCharacterOrder.Count == 0)
		{
			conflictCharacterOrder.Add(0);
			conflictCharacterOrder.Add(1);
			conflictCharacterOrder.Add(2);
			conflictCharacterOrder.Add(3);
			conflictCharacterOrder.Shuffle();
		}
		else
		{
			int item = conflictCharacterOrder[0];
			conflictCharacterOrder.RemoveAt(0);
			conflictCharacterOrder.Add(item);
		}
		return conflictCharacterOrder;
	}

	public void BossKilled(string bossName)
	{
		if (bossesKilledName == null)
		{
			bossesKilledName = new List<string>();
		}
		if (!bossesKilledName.Contains(bossName))
		{
			bossesKilledName.Add(bossName);
		}
		bossesKilled++;
	}

	public bool IsBossKilled(string bossName)
	{
		for (int i = 0; i < bossesKilledName.Count; i++)
		{
			Debug.Log("->" + bossesKilledName[i]);
		}
		if (bossesKilledName == null)
		{
			return false;
		}
		if (bossesKilledName.Contains(bossName))
		{
			return true;
		}
		string value = bossName.ToLower().Replace(" ", "");
		for (int j = 0; j < bossesKilledName.Count; j++)
		{
			if (bossesKilledName[j].Contains(value))
			{
				return true;
			}
		}
		return false;
	}

	public void MonsterKilled()
	{
		monstersKilled++;
	}

	public void SetHeroTraitsCombatTool()
	{
		for (int i = 0; i < 4; i++)
		{
			if (teamAtO[i] != null && teamAtO[i].HeroData != null && teamAtO[i].HeroData.HeroSubClass != null && teamAtO[i].HeroData.HeroSubClass.Trait0 != null)
			{
				teamAtO[i].AssignTrait(teamAtO[i].HeroData.HeroSubClass.Trait0.Id);
			}
		}
	}

	public void BeginAdventure()
	{
		Debug.Log("AtO Begin Adventure");
		if (teamAtO == null || teamAtO.Length == 0)
		{
			CreateTeam();
		}
		if (GameManager.Instance.IsWeeklyChallenge())
		{
			SandboxManager.Instance.DisableSandbox();
		}
		if (!SandboxManager.Instance.IsEnabled())
		{
			ClearSandbox();
		}
		else
		{
			SandboxManager.Instance.SaveValuesToAtOManager();
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			ShareSandbox();
		}
		if (GameManager.Instance.IsObeliskChallenge())
		{
			ngPlus = 0;
			madnessCorruptors = "";
			singularityMadness = 0;
		}
		else if (GameManager.Instance.IsSingularity())
		{
			ngPlus = 0;
			madnessCorruptors = "";
			obeliskMadness = 0;
		}
		else
		{
			obeliskMadness = 0;
			singularityMadness = 0;
			if (GameManager.Instance.GameStatus != Enums.GameStatus.LoadGame && !GameManager.Instance.IsMultiplayer() && GameManager.Instance.IsTutorialGame())
			{
				townTutorialStep = 0;
			}
		}
		for (int i = 0; i < 4; i++)
		{
			if (teamAtO[i] == null || !(teamAtO[i].HeroData != null) || !(teamAtO[i].HeroData.HeroSubClass != null))
			{
				continue;
			}
			for (int j = 0; j < teamAtO[i].HeroData.HeroSubClass.Traits.Count; j++)
			{
				if (teamAtO[i].HeroData.HeroSubClass.Traits[j] != null)
				{
					teamAtO[i].AssignTrait(teamAtO[i].HeroData.HeroSubClass.Traits[j].Id);
				}
			}
			if (!GameManager.Instance.IsObeliskChallenge())
			{
				teamAtO[i].ReassignInitialItem();
			}
		}
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			AssignGlobalEventRequirements();
			if (GameManager.Instance.IsGameAdventure())
			{
				if (ngPlus >= 7)
				{
					SetGameId();
				}
				if (GameManager.Instance.IsMultiplayer())
				{
					ShareNGPlus();
				}
			}
			else if (GameManager.Instance.IsSingularity() && GameManager.Instance.IsMultiplayer())
			{
				ShareSingularityMadness();
			}
		}
		else
		{
			if (!GameManager.Instance.IsWeeklyChallenge() && obeliskMadness >= 8)
			{
				SetGameId();
			}
			SetObeliskNodes();
			if (!GameManager.Instance.IsWeeklyChallenge() && GameManager.Instance.IsMultiplayer())
			{
				ShareObeliskMadnessAndMaps();
			}
		}
		SetGameUniqueId();
		for (int k = 0; k < 4; k++)
		{
			if (teamAtO[k] != null && teamAtO[k].HeroData != null && teamAtO[k].HeroData.HeroSubClass != null && teamAtO[k].HeroData.HeroSubClass.Trait0 != null)
			{
				teamAtO[k].AssignTrait(teamAtO[k].HeroData.HeroSubClass.Trait0.Id);
			}
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			InitGameMP();
		}
		else
		{
			InitGame();
		}
		SetTownTier(0);
		if (GameManager.Instance.IsObeliskChallenge())
		{
			SetCurrentNode(obeliskLow + "_0");
		}
		else if (!GameManager.Instance.IsMultiplayer() && IsFirstGame() && GameManager.Instance.IsGameAdventure())
		{
			SetCurrentNode("tutorial_0");
		}
		else
		{
			SetCurrentNode("sen_0");
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			if (GameManager.Instance.IsObeliskChallenge())
			{
				StartCoroutine(ShareTeam("ChallengeSelection", setOwners: true));
			}
			else
			{
				StartCoroutine(ShareTeam("IntroNewGame", setOwners: true));
			}
		}
		else if (!GameManager.Instance.IsMultiplayer())
		{
			if (GameManager.Instance.IsObeliskChallenge())
			{
				SceneStatic.LoadByName("ChallengeSelection");
			}
			else
			{
				SceneStatic.LoadByName("IntroNewGame");
			}
		}
		StartCoroutine(SendStartTelemetryCo(isLoading: false));
		if (GameManager.Instance.IsMultiplayer())
		{
			photonView.RPC("NET_SendStartTelemetryCo", RpcTarget.Others);
		}
	}

	[PunRPC]
	private void NET_SendStartTelemetryCo()
	{
		StartCoroutine(SendStartTelemetryCo(isLoading: false));
	}

	private IEnumerator SendStartTelemetryCo(bool isLoading)
	{
		yield return Globals.Instance.WaitForSeconds(0.5f);
		Telemetry.SendPlaysessionStart(isLoading);
	}

	public void LaunchCinematic()
	{
		if (!GameManager.Instance.IsMultiplayer())
		{
			SceneStatic.LoadByName("Cinematic");
		}
		else if (NetworkManager.Instance.IsMaster())
		{
			StartCoroutine(ShareTeam("Cinematic"));
		}
	}

	public void LaunchMap()
	{
		if (MapManager.Instance == null)
		{
			if (!GameManager.Instance.IsMultiplayer())
			{
				SceneStatic.LoadByName("Map");
			}
			else if (NetworkManager.Instance.IsMaster())
			{
				StartCoroutine(ShareTeam("Map"));
			}
		}
	}

	public void SetPlayerPerks(Dictionary<string, List<string>> _playerHeroPerks, string[] teamString)
	{
		heroPerks = new Dictionary<string, List<string>>();
		if (!GameManager.Instance.IsMultiplayer())
		{
			for (int i = 0; i < 4; i++)
			{
				string text = teamString[i].ToLower();
				if (text != "")
				{
					List<string> value = PlayerManager.Instance.GetHeroPerks(text, forceFromPlayerManager: true);
					heroPerks.Add(text, value);
				}
			}
			return;
		}
		for (int j = 0; j < 4; j++)
		{
			string text2 = teamString[j].ToLower();
			if (text2 != "")
			{
				string key = (NetworkManager.Instance.PlayerHeroPositionOwner[j] + "_" + text2).ToLower();
				if (_playerHeroPerks.ContainsKey(key))
				{
					heroPerks.Add(text2, _playerHeroPerks[key]);
				}
			}
		}
	}

	public void SetPlayerPerksChallenge(Dictionary<int, List<int>> heroPerksDict)
	{
		heroPerks = new Dictionary<string, List<string>>();
		for (int i = 0; i < 4; i++)
		{
			if (teamAtO[i] == null || teamAtO[i].HeroData == null)
			{
				continue;
			}
			string subclassName = teamAtO[i].SubclassName;
			Enums.CardClass cardClass = (Enums.CardClass)Enum.Parse(typeof(Enums.CardClass), Enum.GetName(typeof(Enums.HeroClass), teamAtO[i].HeroData.HeroClass));
			if (teamAtO[i].HeroData.HeroSubClass.Id == Globals.Instance.GetSubClassData("engineer").Id)
			{
				cardClass = Enums.CardClass.Warrior;
			}
			List<PerkData> perkDataClass = Globals.Instance.GetPerkDataClass(cardClass);
			SortedDictionary<int, PerkData> sortedDictionary = new SortedDictionary<int, PerkData>();
			for (int j = 0; j < perkDataClass.Count; j++)
			{
				if (perkDataClass[j].ObeliskPerk && !sortedDictionary.ContainsKey(perkDataClass[j].Level))
				{
					sortedDictionary.Add(perkDataClass[j].Level, perkDataClass[j]);
				}
			}
			List<string> list = new List<string>();
			foreach (KeyValuePair<int, PerkData> item in sortedDictionary)
			{
				if (heroPerksDict[i].Contains(item.Key - 1))
				{
					list.Add(item.Value.Id);
				}
			}
			heroPerks.Add(subclassName, list);
		}
	}

	public void ClearTemporalNodeScore()
	{
		totalScoreTMP = 0;
		mapVisitedNodesTMP = 0;
		experienceGainedTMP = 0;
		totalDeathsTMP = 0;
		combatExpertiseTMP = 0;
		bossesKilledTMP = 0;
		corruptionCommonCompletedTMP = 0;
		corruptionUncommonCompletedTMP = 0;
		corruptionRareCompletedTMP = 0;
		corruptionEpicCompletedTMP = 0;
	}

	public void SetCombatCorruptionForScore()
	{
		if (!corruptionAccepted)
		{
			return;
		}
		CardData cardData = Globals.Instance.GetCardData(corruptionIdCard, instantiate: false);
		if (cardData != null)
		{
			if (cardData.CardRarity == Enums.CardRarity.Common)
			{
				corruptionCommonCompleted++;
			}
			else if (cardData.CardRarity == Enums.CardRarity.Uncommon)
			{
				corruptionUncommonCompleted++;
			}
			else if (cardData.CardRarity == Enums.CardRarity.Rare)
			{
				corruptionRareCompleted++;
			}
			else
			{
				corruptionEpicCompleted++;
			}
		}
	}

	public void NodeScore()
	{
		if (teamAtO == null)
		{
			return;
		}
		bool flag = mapVisitedNodesTMP == 0;
		int num = 0;
		for (int i = 0; i < mapVisitedNodes.Count; i++)
		{
			if (Globals.Instance.GetNodeData(mapVisitedNodes[i]) != null && Globals.Instance.GetNodeData(mapVisitedNodes[i]).NodeZone != null && !Globals.Instance.GetNodeData(mapVisitedNodes[i]).NodeZone.DisableExperienceOnThisZone)
			{
				num++;
			}
		}
		int num2 = num - mapVisitedNodesTMP;
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			if (num < 2)
			{
				mapVisitedNodesTMP = 0;
				num2 = 0;
			}
			else
			{
				if (mapVisitedNodesTMP == 0)
				{
					num2 -= 2;
				}
				mapVisitedNodesTMP = num;
			}
		}
		else if (num < 1)
		{
			mapVisitedNodesTMP = 0;
			num2 = 0;
		}
		else
		{
			if (mapVisitedNodesTMP == 0)
			{
				num2--;
			}
			mapVisitedNodesTMP = num;
		}
		int num3 = num2 * 36;
		int num4 = combatExpertise - combatExpertiseTMP;
		combatExpertiseTMP = combatExpertise;
		int num5 = num4;
		if (num5 < 0)
		{
			num5 = 0;
		}
		int num6 = num5 * 13;
		int num7 = 0;
		int num8 = 0;
		if (teamAtO != null)
		{
			for (int j = 0; j < teamAtO.Length; j++)
			{
				if (teamAtO[j] != null && !(teamAtO[j].HeroData == null))
				{
					num7 += teamAtO[j].Experience;
					num8 += teamAtO[j].TotalDeaths;
				}
			}
		}
		int num9 = num7 - experienceGainedTMP;
		if (Globals.Instance.GetNodeData(currentMapNode) != null && Globals.Instance.GetNodeData(currentMapNode).NodeZone != null && !Globals.Instance.GetNodeData(currentMapNode).NodeZone.DisableExperienceOnThisZone)
		{
			experienceGainedTMP = num7;
		}
		int num10 = Mathf.Max(0, Functions.FuncRoundToInt((float)num9 * 0.5f));
		int num11 = num8 - totalDeathsTMP;
		totalDeathsTMP = num8;
		int num12 = -num11 * 100;
		int num13 = bossesKilled - bossesKilledTMP;
		bossesKilledTMP = bossesKilled;
		int num14 = num13 * 80;
		int num15 = corruptionCommonCompleted - corruptionCommonCompletedTMP;
		corruptionCommonCompletedTMP = corruptionCommonCompleted;
		int num16 = corruptionUncommonCompleted - corruptionUncommonCompletedTMP;
		corruptionUncommonCompletedTMP = corruptionUncommonCompleted;
		int num17 = corruptionRareCompleted - corruptionRareCompletedTMP;
		corruptionRareCompletedTMP = corruptionRareCompleted;
		int num18 = corruptionEpicCompleted - corruptionEpicCompletedTMP;
		corruptionEpicCompletedTMP = corruptionEpicCompleted;
		int num19 = num15 * 40 + num16 * 80 + num17 * 130 + num18 * 200;
		int num20 = num3 + num6 + num12 + num10 + num14 + num19;
		if (num20 < 0)
		{
			num20 = 0;
		}
		totalScoreTMP += num20;
		if (flag || num20 <= 0)
		{
			return;
		}
		int num21 = num20;
		if (GameManager.Instance.IsObeliskChallenge())
		{
			num21 = Functions.FuncRoundToInt((float)num20 * 0.5f);
		}
		for (int k = 0; k < 4; k++)
		{
			if (teamAtO[k] != null)
			{
				PlayerManager.Instance.ModifyProgress(teamAtO[k].SubclassName, num21, k);
			}
		}
		PlayerManager.Instance.ModifyPlayerRankProgress(num21);
	}

	public int CalculateScore(bool _calculateMadnessMultiplier = true, int _auxValue = 0)
	{
		if (teamAtO == null)
		{
			return 0;
		}
		int num = totalScoreTMP;
		if (_auxValue > 0)
		{
			num += _auxValue;
		}
		if (_calculateMadnessMultiplier)
		{
			int level = 0;
			if (!GameManager.Instance.IsObeliskChallenge())
			{
				level = GetMadnessDifficulty();
			}
			else if (!GameManager.Instance.IsWeeklyChallenge())
			{
				level = GetObeliskMadness();
			}
			int madnessScoreMultiplier = Functions.GetMadnessScoreMultiplier(level, GameManager.Instance.IsGameAdventure());
			num += Functions.FuncRoundToInt(num * madnessScoreMultiplier / 100);
		}
		if (num < 0)
		{
			num = 0;
		}
		return num;
	}

	public void SetWeekly(int _weekly)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("SetWeekly(" + _weekly + ")", "trace");
		}
		weekly = _weekly;
		if (weekly <= 0)
		{
			return;
		}
		ChallengeTraits = new List<string>();
		ChallengeTraits.Add("energizedheroeslow");
		ChallengeData weeklyData = Globals.Instance.GetWeeklyData(_weekly);
		if (!(weeklyData != null) || weeklyData.Traits == null)
		{
			return;
		}
		for (int i = 0; i < weeklyData.Traits.Count; i++)
		{
			string item = weeklyData.Traits[i].Id.ToLower();
			if (!ChallengeTraits.Contains(item))
			{
				ChallengeTraits.Add(item);
			}
		}
	}

	public int GetWeekly()
	{
		return weekly;
	}

	public string GetWeeklyName(int _week)
	{
		ChallengeData weeklyData = Globals.Instance.GetWeeklyData(_week);
		string text = "";
		if (weeklyData != null && weeklyData.IdSteam != "" && Texts.Instance.GetText(weeklyData.IdSteam) != "")
		{
			return Texts.Instance.GetText(weeklyData.IdSteam);
		}
		return string.Format(Texts.Instance.GetText("weekNumber"), _week);
	}

	public void SetObeliskMadnessTraits(int _madnessLevel)
	{
		ChallengeTraits = new List<string>();
		foreach (KeyValuePair<string, ChallengeTrait> item in Globals.Instance.ChallengeTraitsSource)
		{
			if (item.Value.IsMadnessTrait && item.Value.Order <= _madnessLevel)
			{
				ChallengeTraits.Add(item.Value.Id.ToLower());
			}
		}
		if (_madnessLevel >= 0)
		{
			ChallengeTraits.Add("energizedheroeslow");
		}
		if (_madnessLevel >= 8)
		{
			ChallengeTraits.Add("energizedheroes");
		}
	}

	public void SetSingularityMadnessTraits(int _madnessLevel)
	{
		ChallengeTraits = new List<string>();
		foreach (KeyValuePair<string, ChallengeTrait> item in Globals.Instance.ChallengeTraitsSource)
		{
			if (item.Value.IsSingularityTrait && item.Value.OrderSingularity <= _madnessLevel)
			{
				ChallengeTraits.Add(item.Value.Id.ToLower());
			}
		}
	}

	public Character GetHeroWithTrait(string trait)
	{
		if (TeamHaveTrait(trait))
		{
			Hero result = MatchManager.Instance.GetTeamHero()[MatchManager.Instance.GetHeroActive()];
			Hero[] teamHero = MatchManager.Instance.GetTeamHero();
			foreach (Hero hero in teamHero)
			{
				if (Instance.CharacterHaveTrait(hero.SubclassName, trait))
				{
					result = hero;
					break;
				}
			}
			return result;
		}
		return null;
	}

	public void SetSkinIntoSubclassData(string _subclass, string _skinId)
	{
		SubClassData subClassData = Globals.Instance.GetSubClassData(_subclass);
		if (subClassData != null)
		{
			SkinData skinData = Globals.Instance.GetSkinData(_skinId);
			if (skinData != null)
			{
				subClassData.GameObjectAnimated = skinData.SkinGo;
				subClassData.SpriteBorder = skinData.SpriteSiluetaGrande;
				subClassData.SpriteBorderSmall = skinData.SpriteSilueta;
				subClassData.SpriteSpeed = skinData.SpritePortrait;
				subClassData.SpritePortrait = skinData.SpritePortraitGrande;
			}
		}
	}

	public bool NodeIsObeliskLow()
	{
		if (GameManager.Instance.IsObeliskChallenge())
		{
			NodeData nodeData = Globals.Instance.GetNodeData(currentMapNode);
			if (nodeData != null && nodeData.NodeZone != null && nodeData.NodeZone.ZoneId.ToLower() == obeliskLow)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public bool NodeIsObeliskFinal()
	{
		if (GameManager.Instance.IsObeliskChallenge())
		{
			NodeData nodeData = Globals.Instance.GetNodeData(currentMapNode);
			if (nodeData != null && nodeData.NodeZone != null && nodeData.NodeZone.ZoneId.ToLower() == obeliskFinal)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public bool IsChallengeTraitActive(string _trait)
	{
		if ((GameManager.Instance.IsObeliskChallenge() || GameManager.Instance.IsSingularity()) && ChallengeTraits != null && ChallengeTraits.Contains(_trait.ToLower()))
		{
			if (ChallengeTraits.Contains("toughermonsters") && invalidTraitsForTougherMonsters.Contains(_trait.ToLower()))
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public int ModifyQuantityObeliskTraits(int type, int quantity)
	{
		if (type == 0)
		{
			if (MadnessManager.Instance.IsMadnessTraitActive("poverty") || IsChallengeTraitActive("poverty"))
			{
				quantity = (GameManager.Instance.IsObeliskChallenge() ? (quantity - Functions.FuncRoundToInt((float)quantity * 0.3f)) : (quantity - Functions.FuncRoundToInt((float)quantity * 0.5f)));
				if (quantity < 0)
				{
					quantity = 0;
				}
			}
			if (IsChallengeTraitActive("prosperity"))
			{
				quantity += Functions.FuncRoundToInt((float)quantity * 0.5f);
			}
		}
		else
		{
			if (MadnessManager.Instance.IsMadnessTraitActive("poverty") || IsChallengeTraitActive("poverty"))
			{
				quantity = (GameManager.Instance.IsObeliskChallenge() ? (quantity - Functions.FuncRoundToInt((float)quantity * 0.3f)) : (quantity - Functions.FuncRoundToInt((float)quantity * 0.5f)));
				if (quantity < 0)
				{
					quantity = 0;
				}
			}
			if (IsChallengeTraitActive("prosperity"))
			{
				quantity += Functions.FuncRoundToInt((float)quantity * 0.5f);
			}
		}
		return quantity;
	}

	public void IncreaseTownTutorialStep()
	{
		townTutorialStep++;
	}

	public void GoSandboxFromMadness()
	{
		if (MadnessManager.Instance.IsActive())
		{
			MadnessManager.Instance.ShowMadness();
		}
		if (!SandboxManager.Instance.IsActive())
		{
			SandboxManager.Instance.ShowSandbox();
		}
	}

	public void GoMadnessFromSandbox()
	{
		if (SandboxManager.Instance.IsActive())
		{
			SandboxManager.Instance.CloseSandbox();
		}
		if (!MadnessManager.Instance.IsActive())
		{
			MadnessManager.Instance.ShowMadness();
		}
	}
}
