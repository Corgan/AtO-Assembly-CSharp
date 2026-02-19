using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
	private string version = "";

	private string steamUserId;

	private string steamUserIdOriginal = "";

	private string gameId;

	private string gameUniqueId;

	private string gameDate;

	private string gameNick = "";

	private int weekly;

	private Enums.GameMode gameMode;

	private Enums.GameType gameType;

	private int ngPlus;

	private string madnessCorruptors;

	private int obeliskMadness;

	private int singularityMadness;

	private int playerDust;

	private int playerGold;

	private string teamAtO;

	private string teamAtOBackup;

	private int divinationsNumber;

	private bool inTown;

	private int townTier;

	private string townZoneId;

	private int combatExpertise;

	private int combatTotal;

	private int bossesKilled;

	private List<string> bossesKilledName;

	private int monstersKilled;

	private string currentCombatData;

	private string fromEventCombatData;

	private string fromEventEventData;

	private string currentMapNode;

	private Dictionary<string, string> gameNodeAssigned;

	private Dictionary<string, List<string>> heroPerks;

	private List<string> mapVisitedNodes;

	private List<string> mapVisitedNodesAction;

	private List<string> playerRequeriments;

	private Dictionary<string, int> mpPlayersGold;

	private Dictionary<string, int> mpPlayersDust;

	private int totalGoldGained;

	private int totalDustGained;

	private List<string> unlockedCards;

	private List<Dictionary<string, int>> craftedCards;

	private Dictionary<string, List<string>> boughtItems;

	private Dictionary<string, int> boughtItemInShopByWho;

	private Dictionary<string, int> upgradedCardsInAltarByWho;

	private Dictionary<string, string> playerNickRealDict;

	private string owner0 = "";

	private string owner1 = "";

	private string owner2 = "";

	private string owner3 = "";

	private string owner0Original = "";

	private string owner1Original = "";

	private string owner2Original = "";

	private string owner3Original = "";

	private List<int> lootCharacterOrder;

	private List<int> conflictCharacterOrder;

	private Dictionary<string, string> townRerollList;

	private float playedTime;

	private int[,] combatStats;

	private string playerPositionList;

	private int gameHandicap;

	private bool corruptionAccepted;

	private int corruptionType;

	private string corruptionId;

	private string corruptionIdCard;

	private string corruptionRewardCard;

	private int corruptionRewardChar;

	private int corruptionCommonCompleted;

	private int corruptionUncommonCompleted;

	private int corruptionRareCompleted;

	private int corruptionEpicCompleted;

	private int townTutorialStep = -1;

	private string sandboxMods = "";

	public string GameDate
	{
		get
		{
			return gameDate;
		}
		set
		{
			gameDate = value;
		}
	}

	public string CurrentMapNode
	{
		get
		{
			return currentMapNode;
		}
		set
		{
			currentMapNode = value;
		}
	}

	public string TeamAtO
	{
		get
		{
			return teamAtO;
		}
		set
		{
			teamAtO = value;
		}
	}

	public string PlayerPositionList
	{
		get
		{
			return playerPositionList;
		}
		set
		{
			playerPositionList = value;
		}
	}

	public Enums.GameMode GameMode
	{
		get
		{
			return gameMode;
		}
		set
		{
			gameMode = value;
		}
	}

	public Dictionary<string, string> PlayerNickRealDict
	{
		get
		{
			return playerNickRealDict;
		}
		set
		{
			playerNickRealDict = value;
		}
	}

	public int NgPlus
	{
		get
		{
			return ngPlus;
		}
		set
		{
			ngPlus = value;
		}
	}

	public string MadnessCorruptors
	{
		get
		{
			return madnessCorruptors;
		}
		set
		{
			madnessCorruptors = value;
		}
	}

	public string SteamUserId
	{
		get
		{
			return steamUserId;
		}
		set
		{
			steamUserId = value;
		}
	}

	public string Owner0
	{
		get
		{
			return owner0;
		}
		set
		{
			owner0 = value;
		}
	}

	public string Owner1
	{
		get
		{
			return owner1;
		}
		set
		{
			owner1 = value;
		}
	}

	public string Owner2
	{
		get
		{
			return owner2;
		}
		set
		{
			owner2 = value;
		}
	}

	public string Owner3
	{
		get
		{
			return owner3;
		}
		set
		{
			owner3 = value;
		}
	}

	public string Version
	{
		get
		{
			return version;
		}
		set
		{
			version = value;
		}
	}

	public int Weekly
	{
		get
		{
			return weekly;
		}
		set
		{
			weekly = value;
		}
	}

	public int ObeliskMadness
	{
		get
		{
			return obeliskMadness;
		}
		set
		{
			obeliskMadness = value;
		}
	}

	public Enums.GameType GameType
	{
		get
		{
			return gameType;
		}
		set
		{
			gameType = value;
		}
	}

	public int TownTier
	{
		get
		{
			return townTier;
		}
		set
		{
			townTier = value;
		}
	}

	public string GameUniqueId
	{
		get
		{
			return gameUniqueId;
		}
		set
		{
			gameUniqueId = value;
		}
	}

	public void FillData(bool newGame)
	{
		version = GameManager.Instance.gameVersion;
		if (GameManager.Instance.GetDeveloperMode())
		{
			steamUserId = null;
		}
		else
		{
			steamUserId = SteamManager.Instance.steamId.ToString();
		}
		if (steamUserIdOriginal == "")
		{
			steamUserIdOriginal = steamUserId;
		}
		gameId = AtOManager.Instance.GetGameId();
		gameUniqueId = AtOManager.Instance.GetGameUniqueId();
		gameDate = DateTime.Now.ToString();
		if (gameNick == "")
		{
			gameNick = PlayerManager.Instance.GetPlayerName();
		}
		gameMode = GameManager.Instance.GameMode;
		gameType = GameManager.Instance.GameType;
		ngPlus = AtOManager.Instance.GetNgPlus(_fromSave: true);
		madnessCorruptors = AtOManager.Instance.GetMadnessCorruptors();
		obeliskMadness = AtOManager.Instance.GetObeliskMadness();
		singularityMadness = AtOManager.Instance.GetSingularityMadness();
		weekly = AtOManager.Instance.GetWeekly();
		playerDust = AtOManager.Instance.GetPlayerDust();
		playerGold = AtOManager.Instance.GetPlayerGold();
		AtOManager.Instance.DistributeGoldDustBetweenHeroes();
		Hero[] team = AtOManager.Instance.GetTeam();
		teamAtO = JsonHelper.ToJson(team);
		Hero[] teamBackup = AtOManager.Instance.GetTeamBackup();
		teamAtOBackup = JsonHelper.ToJson(teamBackup);
		divinationsNumber = AtOManager.Instance.divinationsNumber;
		inTown = AtOManager.Instance.CharInTown();
		townTier = AtOManager.Instance.GetTownTier();
		townZoneId = AtOManager.Instance.GetTownZoneId();
		combatExpertise = AtOManager.Instance.combatExpertise;
		combatTotal = AtOManager.Instance.combatTotal;
		bossesKilled = AtOManager.Instance.bossesKilled;
		bossesKilledName = AtOManager.Instance.bossesKilledName;
		monstersKilled = AtOManager.Instance.monstersKilled;
		CombatData combatData = AtOManager.Instance.GetCurrentCombatData();
		if (combatData != null)
		{
			currentCombatData = combatData.CombatId;
		}
		else
		{
			currentCombatData = "";
		}
		if (AtOManager.Instance.fromEventCombatData != null)
		{
			fromEventCombatData = AtOManager.Instance.fromEventCombatData.CombatId;
		}
		else
		{
			fromEventCombatData = "";
		}
		if (AtOManager.Instance.fromEventEventData != null)
		{
			fromEventEventData = AtOManager.Instance.fromEventEventData.EventId;
		}
		else
		{
			fromEventEventData = "";
		}
		currentMapNode = AtOManager.Instance.currentMapNode;
		mapVisitedNodes = AtOManager.Instance.mapVisitedNodes;
		mapVisitedNodesAction = AtOManager.Instance.mapVisitedNodesAction;
		gameNodeAssigned = AtOManager.Instance.gameNodeAssigned;
		playerRequeriments = AtOManager.Instance.GetPlayerRequeriments();
		mpPlayersGold = AtOManager.Instance.GetMpPlayersGold();
		mpPlayersDust = AtOManager.Instance.GetMpPlayersDust();
		totalGoldGained = AtOManager.Instance.totalGoldGained;
		totalDustGained = AtOManager.Instance.totalDustGained;
		unlockedCards = AtOManager.Instance.unlockedCards;
		craftedCards = AtOManager.Instance.craftedCards;
		boughtItems = AtOManager.Instance.boughtItems;
		heroPerks = AtOManager.Instance.heroPerks;
		boughtItemInShopByWho = AtOManager.Instance.boughtItemInShopByWho;
		upgradedCardsInAltarByWho = AtOManager.Instance.upgradedCardsInAltarByWho;
		lootCharacterOrder = AtOManager.Instance.lootCharacterOrder;
		conflictCharacterOrder = AtOManager.Instance.conflictCharacterOrder;
		combatStats = AtOManager.Instance.combatStats;
		townRerollList = AtOManager.Instance.townRerollList;
		if (!GameManager.Instance.IsMultiplayer() && townRerollList != null)
		{
			using Dictionary<string, string>.Enumerator enumerator = townRerollList.GetEnumerator();
			if (enumerator.MoveNext())
			{
				KeyValuePair<string, string> current = enumerator.Current;
				AtOManager.Instance.shopItemReroll = current.Value;
			}
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			playerNickRealDict = NetworkManager.Instance.PlayerNickRealDict;
			if (team[0] != null && team[0].HeroData != null && playerNickRealDict.ContainsKey(team[0].Owner))
			{
				owner0 = playerNickRealDict[team[0].Owner];
			}
			if (team[1] != null && team[1].HeroData != null && playerNickRealDict.ContainsKey(team[1].Owner))
			{
				owner1 = playerNickRealDict[team[1].Owner];
			}
			if (team[2] != null && team[2].HeroData != null && playerNickRealDict.ContainsKey(team[2].Owner))
			{
				owner2 = playerNickRealDict[team[2].Owner];
			}
			if (team[3] != null && team[3].HeroData != null && playerNickRealDict.ContainsKey(team[3].Owner))
			{
				owner3 = playerNickRealDict[team[3].Owner];
			}
			if (owner0Original == "" && owner0 != "")
			{
				owner0Original = owner0;
			}
			if (owner1Original == "" && owner1 != "")
			{
				owner1Original = owner0;
			}
			if (owner2Original == "" && owner2 != "")
			{
				owner2Original = owner0;
			}
			if (owner3Original == "" && owner3 != "")
			{
				owner3Original = owner0;
			}
		}
		playedTime = AtOManager.Instance.playedTime;
		corruptionAccepted = AtOManager.Instance.corruptionAccepted;
		corruptionType = AtOManager.Instance.corruptionType;
		corruptionId = AtOManager.Instance.corruptionId;
		corruptionIdCard = AtOManager.Instance.corruptionIdCard;
		corruptionRewardCard = AtOManager.Instance.corruptionRewardCard;
		corruptionRewardChar = AtOManager.Instance.corruptionRewardChar;
		corruptionCommonCompleted = AtOManager.Instance.corruptionCommonCompleted;
		corruptionUncommonCompleted = AtOManager.Instance.corruptionUncommonCompleted;
		corruptionRareCompleted = AtOManager.Instance.corruptionRareCompleted;
		corruptionEpicCompleted = AtOManager.Instance.corruptionEpicCompleted;
		townTutorialStep = AtOManager.Instance.TownTutorialStep;
		gameHandicap = AtOManager.Instance.gameHandicap;
		sandboxMods = AtOManager.Instance.GetSandboxMods();
		AtOManager.Instance.saveLoadStatus = false;
		Functions.DebugLogGD("SAVING GAME", "save");
	}

	public void LoadData(bool comingFromReloadCombat = false)
	{
		AtOManager.Instance.ClearGame();
		AtOManager.Instance.SetGameId(gameId);
		AtOManager.Instance.SetGameUniqueId(gameUniqueId);
		GameManager.Instance.GameMode = gameMode;
		GameManager.Instance.GameType = gameType;
		GameManager.Instance.SetGameStatus(Enums.GameStatus.LoadGame);
		AtOManager.Instance.SetNgPlus(ngPlus);
		AtOManager.Instance.SetMadnessCorruptors(madnessCorruptors);
		AtOManager.Instance.SetObeliskMadness(obeliskMadness);
		AtOManager.Instance.SetSingularityMadness(singularityMadness);
		AtOManager.Instance.SetWeekly(weekly);
		AtOManager.Instance.SetPlayerDust(playerDust);
		AtOManager.Instance.SetPlayerGold(playerGold);
		Hero[] team = JsonHelper.FromJson<Hero>(teamAtO);
		AtOManager.Instance.AssignTeamFromSaveGame(team);
		if (teamAtOBackup != "" && teamAtOBackup != null)
		{
			Hero[] team2 = JsonHelper.FromJson<Hero>(teamAtOBackup);
			AtOManager.Instance.AssignTeamBackupFromSaveGame(team2);
		}
		AtOManager.Instance.divinationsNumber = divinationsNumber;
		AtOManager.Instance.SetCharInTown(inTown);
		AtOManager.Instance.SetTownTier(townTier);
		AtOManager.Instance.SetTownZoneId(townZoneId);
		AtOManager.Instance.combatExpertise = combatExpertise;
		AtOManager.Instance.combatTotal = combatTotal;
		AtOManager.Instance.bossesKilled = bossesKilled;
		AtOManager.Instance.bossesKilledName = bossesKilledName;
		AtOManager.Instance.monstersKilled = monstersKilled;
		AtOManager.Instance.currentMapNode = currentMapNode;
		AtOManager.Instance.mapVisitedNodes = mapVisitedNodes;
		AtOManager.Instance.mapVisitedNodesAction = mapVisitedNodesAction;
		AtOManager.Instance.gameNodeAssigned = gameNodeAssigned;
		AtOManager.Instance.SetPlayerRequeriments(playerRequeriments);
		AtOManager.Instance.townRerollList = townRerollList;
		if (currentCombatData != "")
		{
			AtOManager.Instance.SetCombatData(Globals.Instance.GetCombatData(currentCombatData));
		}
		if (fromEventCombatData != "")
		{
			AtOManager.Instance.fromEventCombatData = Globals.Instance.GetCombatData(fromEventCombatData);
			if (AtOManager.Instance.GetCurrentCombatData() == null)
			{
				AtOManager.Instance.SetCombatData(Globals.Instance.GetCombatData(fromEventCombatData));
			}
		}
		if (fromEventEventData != "")
		{
			AtOManager.Instance.fromEventEventData = Globals.Instance.GetEventData(fromEventEventData);
		}
		AtOManager.Instance.heroPerks = heroPerks;
		AtOManager.Instance.SetMpPlayersGold(mpPlayersGold);
		AtOManager.Instance.SetMpPlayersDust(mpPlayersDust);
		AtOManager.Instance.totalGoldGained = totalGoldGained;
		AtOManager.Instance.totalDustGained = totalDustGained;
		AtOManager.Instance.unlockedCards = unlockedCards;
		bool flag = false;
		if (craftedCards.GetType().IsGenericType && craftedCards.GetType().GetGenericTypeDefinition() == typeof(List<>))
		{
			flag = true;
		}
		if (flag)
		{
			AtOManager.Instance.craftedCards = craftedCards;
		}
		AtOManager.Instance.boughtItems = boughtItems;
		AtOManager.Instance.boughtItemInShopByWho = boughtItemInShopByWho;
		AtOManager.Instance.upgradedCardsInAltarByWho = upgradedCardsInAltarByWho;
		AtOManager.Instance.lootCharacterOrder = lootCharacterOrder;
		AtOManager.Instance.conflictCharacterOrder = conflictCharacterOrder;
		AtOManager.Instance.gameHandicap = gameHandicap;
		AtOManager.Instance.combatStats = combatStats;
		AtOManager.Instance.playedTime = playedTime;
		AtOManager.Instance.corruptionAccepted = corruptionAccepted;
		AtOManager.Instance.corruptionType = corruptionType;
		AtOManager.Instance.corruptionId = corruptionId;
		AtOManager.Instance.corruptionIdCard = corruptionIdCard;
		AtOManager.Instance.corruptionRewardCard = corruptionRewardCard;
		AtOManager.Instance.corruptionRewardChar = corruptionRewardChar;
		AtOManager.Instance.corruptionCommonCompleted = corruptionCommonCompleted;
		AtOManager.Instance.corruptionUncommonCompleted = corruptionUncommonCompleted;
		AtOManager.Instance.corruptionRareCompleted = corruptionRareCompleted;
		AtOManager.Instance.corruptionEpicCompleted = corruptionEpicCompleted;
		AtOManager.Instance.TownTutorialStep = townTutorialStep;
		NetworkManager.Instance.Owner0 = owner0;
		NetworkManager.Instance.Owner1 = owner1;
		NetworkManager.Instance.Owner2 = owner2;
		NetworkManager.Instance.Owner3 = owner3;
		AtOManager.Instance.SetSandboxMods(sandboxMods);
		AtOManager.Instance.saveLoadStatus = false;
		AtOManager.Instance.DoLoadGame(comingFromReloadCombat);
	}
}
