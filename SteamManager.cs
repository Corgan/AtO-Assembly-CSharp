using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

public class SteamManager : MonoBehaviour
{
	public bool steamLoaded;

	public bool steamConnected = true;

	public string steamName = "";

	public SteamId steamId = 0uL;

	private uint releaseAppId = 1385380u;

	public Lobby lobby;

	private List<string> achievementsUnlocked = new List<string>();

	public LeaderboardEntry[] scoreboardGlobal;

	public LeaderboardEntry[] scoreboardFriends;

	public LeaderboardEntry[] scoreboardSingle;

	public Dictionary<string, string> dlcInfo;

	public bool gettingScoreboards;

	public List<string> shameList;

	public List<string> weeklyScoreboards;

	public static SteamManager Instance { get; private set; }

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
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		shameList = new List<string>();
		shameList.Add("76561197967061331");
		shameList.Add("76561198090333522");
		shameList.Add("76561198010763033");
		shameList.Add("76561198973152554");
		shameList.Add("76561198071221108");
		shameList.Add("76561198161693534");
		shameList.Add("76561198014409560");
		shameList.Add("76561198360113370");
		shameList.Add("76561198104667997");
		shameList.Add("76561198166576962");
		shameList.Add("76561198110796654");
		shameList.Add("76561198099219329");
		shameList.Add("76561198005796435");
		shameList.Add("76561198308284145");
		shameList.Add("76561198364083963");
		shameList.Add("76561198051322233");
		shameList.Add("76561199010874940");
		shameList.Add("76561198151550085");
		shameList.Add("76561198091595297");
		shameList.Add("76561198045144129");
		shameList.Add("76561198103653826");
		shameList.Add("76561198968892849");
		shameList.Add("76561198127761770");
		shameList.Add("76561198351803536");
		shameList.Add("76561198069814663");
		shameList.Add("76561198215385943");
		shameList.Add("76561198394562597");
		shameList.Add("76561198002690061");
		shameList.Add("76561198298394060");
		shameList.Add("76561198035495634");
		shameList.Add("76561198015014615");
		shameList.Add("76561198258642840");
		shameList.Add("76561198039423148");
		shameList.Add("76561198046686325");
		shameList.Add("76561198100345602");
		shameList.Add("76561199098349852");
		shameList.Add("76561198050815767");
		shameList.Add("76561198065259678");
		shameList.Add("76561198314042772");
		shameList.Add("76561198158188514");
		shameList.Add("76561198006577550");
		shameList.Add("76561198059708390");
		shameList.Add("76561198015294057");
		shameList.Add("76561198004205610");
		shameList.Add("76561198054470753");
		shameList.Add("76561198846477761");
		shameList.Add("76561198346627042");
		shameList.Add("76561198881899421");
		shameList.Add("76561198986226022");
		shameList.Add("76561198363545598");
		shameList.Add("76561199256732247");
		shameList.Add("76561198179268647");
		shameList.Add("76561198080679919");
		shameList.Add("76561198060522910");
		shameList.Add("76561198046652127");
		shameList.Add("76561198034940625");
		shameList.Add("76561198009205578");
		shameList.Add("76561197980643859");
		shameList.Add("76561198151550085");
		shameList.Add("76561198846477761");
		shameList.Add("76561198290102386");
		shameList.Add("76561198170429125");
		shameList.Add("76561198247692284");
		shameList.Add("76561198101912322");
		shameList.Add("76561198307388683");
		shameList.Add("76561198797186891");
		shameList.Add("76561198070643116");
		shameList.Add("76561198080367358");
		shameList.Add("76561198071602793");
		shameList.Add("76561199199873267");
		shameList.Add("76561198841015318");
		shameList.Add("76561198288441233");
		shameList.Add("76561198350975460");
		shameList.Add("76561198065708345");
		shameList.Add("76561198254155353");
		shameList.Add("76561199205050622");
		shameList.Add("76561198374766185");
		shameList.Add("76561198016895913");
		shameList.Add("76561198171587526");
		shameList.Add("76561198140422317");
		shameList.Add("76561198046571676");
		shameList.Add("76561197974637386");
		shameList.Add("76561198004538781");
		shameList.Add("76561198198851386");
		shameList.Add("76561198005638497");
		shameList.Add("76561198023548551");
		shameList.Add("76561198177945821");
		shameList.Add("76561199168522784");
		shameList.Add("76561198164878710");
		shameList.Add("76561198006755583");
		shameList.Add("76561199217708664");
		shameList.Add("76561198799221793");
		shameList.Add("76561198011821425");
		shameList.Add("76561198397021079");
		shameList.Add("76561198000980157");
		shameList.Add("76561198154927630");
		shameList.Add("76561198214203984");
		shameList.Add("76561198274612857");
		shameList.Add("76561198313485330");
		shameList.Add("76561199025382063");
		shameList.Add("76561198340738336");
		shameList.Add("76561198293218125");
		shameList.Add("76561198314825649");
		shameList.Add("76561197999985728");
		shameList.Add("76561198031450406");
		shameList.Add("76561198079013335");
		shameList.Add("76561198131655603");
		shameList.Add("76561198050338192");
		shameList.Add("76561198402790968");
		shameList.Add("76561198091899280");
		shameList.Add("76561198225341324");
		shameList.Add("76561198140064824");
		shameList.Add("76561198296474483");
		shameList.Add("76561198363974009");
		shameList.Add("76561198402790968");
		shameList.Add("76561197995670501");
		shameList.Add("76561198040759667");
		shameList.Add("76561198294003247");
		shameList.Add("76561198256108408");
		shameList.Add("76561198119337267");
		shameList.Add("76561198124951939");
		shameList.Add("76561198384354335");
		shameList.Add("76561198282949213");
		shameList.Add("76561199348762256");
		shameList.Add("76561198850068192");
		shameList.Add("76561198348456538");
		shameList.Add("76561198423590954");
		shameList.Add("76561198863792752");
		shameList.Add("76561198070802868");
		shameList.Add("76561198829816933");
		shameList.Add("76561199362839599");
		shameList.Add("76561197986029843");
		shameList.Add("76561199362839599");
		shameList.Add("76561198824056936");
		shameList.Add("76561198254975029");
		shameList.Add("76561198065096276");
		shameList.Add("76561197997194195");
		shameList.Add("76561199074073309");
		shameList.Add("76561198031312496");
		shameList.Add("76561198102228964");
		shameList.Add("76561198324161664");
		shameList.Add("76561198288397625");
		shameList.Add("76561198124937642");
		shameList.Add("76561198048763052");
		shameList.Add("76561199234795844");
		shameList.Add("76561198119823284");
		shameList.Add("76561198424109209");
		shameList.Add("76561198086161708");
		shameList.Add("76561198070161930");
		shameList.Add("76561198197183203");
		shameList.Add("76561198124334507");
		shameList.Add("76561198985445897");
		shameList.Add("76561198103477347");
		shameList.Add("76561197992613000");
		shameList.Add("76561198048621985");
		shameList.Add("76561198424017417");
		shameList.Add("76561198264284174");
		shameList.Add("76561198126097202");
		shameList.Add("76561198263680470");
	}

	private void OnDisable()
	{
		SteamClient.Shutdown();
	}

	public void DoSteam()
	{
		if (GameManager.Instance.UseTestSteamID)
		{
			releaseAppId = 480u;
		}
		uint num = releaseAppId;
		try
		{
			if (SteamClient.RestartAppIfNecessary((AppId)num))
			{
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException ex)
		{
			Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + ex, this);
			steamConnected = false;
			Application.Quit();
			return;
		}
		try
		{
			SteamClient.Init(num);
		}
		catch (Exception message)
		{
			Debug.Log("[STEAM] Error connecting to Steam");
			Debug.Log(message);
			steamConnected = false;
		}
		if (!steamConnected)
		{
			return;
		}
		if (SteamApps.IsSubscribedToApp(releaseAppId))
		{
			GameManager.Instance.SetDemo(state: false);
		}
		steamName = SteamClient.Name;
		steamId = SteamClient.SteamId;
		if (steamId.ToString() == "76561198229850604" || steamId.ToString() == "76561198018931074" || steamId.ToString() == "76561198019918417" || steamId.ToString() == "76561198856292125" || steamId.ToString() == "76561199225796884" || steamId.ToString() == "76561198965756754" || steamId.ToString() == "76561198036268174" || steamId.ToString() == "76561198180023935" || steamId.ToString() == "76561198019330206" || steamId.ToString() == "76561198049739831" || steamId.ToString() == "76561197995379359" || steamId.ToString() == "76561199738699144" || steamId.ToString() == "76561198175071742")
		{
			GameManager.Instance.SetDeveloperMode(state: true);
		}
		GetDLCInformation();
		SteamFriends.OnGameRichPresenceJoinRequested += OnGameRichPresenceJoinRequested;
		SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
		SteamMatchmaking.OnLobbyMemberJoined += OnLobbyMemberJoined;
		SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequested;
		SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
		SteamMatchmaking.OnLobbyInvite += OnLobbyInvite;
		SteamFriends.OnChatMessage += OnChatMessage;
		SteamApps.OnNewLaunchParameters += OnNewLaunchParameters;
		SteamApps.GetLaunchParam("+connect_lobby");
		int num2 = -1;
		string text = "";
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		for (int i = 0; i < commandLineArgs.Length; i++)
		{
			if (i == num2)
			{
				text = commandLineArgs[i];
			}
			else if (commandLineArgs[i] == "+connect_lobby")
			{
				num2 = i + i;
			}
		}
		if (text != "")
		{
			SteamId lobbyId = ulong.Parse(text);
			try
			{
				SteamMatchmaking.JoinLobbyAsync(lobbyId);
				return;
			}
			catch (Exception message2)
			{
				Debug.Log("[STEAM] Error connecting to Steam");
				Debug.Log(message2);
				steamLoaded = false;
				return;
			}
		}
		steamLoaded = true;
	}

	public int GetStatInt(string _name)
	{
		return SteamUserStats.GetStatInt(_name);
	}

	public void SetStatInt(string _name, int _value)
	{
		SteamUserStats.SetStat(_name, _value);
	}

	public DateTime GetSteamTime()
	{
		return SteamUtils.SteamServerTime;
	}

	public bool PlayerHaveDLC(string _sku)
	{
		if (GameManager.Instance.GetDeveloperMode() || GameManager.Instance.CheatMode)
		{
			return true;
		}
		uint num = uint.Parse(_sku);
		if (SteamApps.IsSubscribedToApp(num) && LauncherEnabledDLC(num))
		{
			return true;
		}
		return false;
	}

	private bool LauncherEnabledDLC(uint _appId)
	{
		string item = "";
		switch (_appId)
		{
		case 2666340u:
			item = "amelia_the_queen";
			break;
		case 2168960u:
			item = "spooky_nights_in_senenthia";
			break;
		case 2511580u:
			item = "sands_of_ulminin";
			break;
		case 2325780u:
			item = "wolf_wars";
			break;
		case 2879690u:
			item = "the_obsidian_uprising";
			break;
		case 2879680u:
			item = "nenukil_the_engineer";
			break;
		case 3875470u:
			item = "necropolis";
			break;
		case 4013420u:
			item = "asian_skins";
			break;
		}
		if (GameManager.Instance.DisabledDLCs.Contains(item))
		{
			return false;
		}
		return true;
	}

	public void GetDLCInformation()
	{
		dlcInfo = new Dictionary<string, string>();
		foreach (DlcInformation item in SteamApps.DlcInformation())
		{
			dlcInfo.Add(item.AppId.ToString(), item.Name);
		}
	}

	public string GetDLCName(string _sku)
	{
		if (dlcInfo != null && dlcInfo.ContainsKey(_sku))
		{
			return dlcInfo[_sku];
		}
		return "";
	}

	public void AchievementUnlock(string id)
	{
		if (steamConnected && !achievementsUnlocked.Contains(id))
		{
			new Achievement(id).Trigger();
			achievementsUnlocked.Add(id);
		}
	}

	private void OnInvitedToGame(Friend _friendId, string connect)
	{
		Debug.Log("OnInvitedToGame");
		Debug.Log(_friendId);
		Debug.Log(connect);
	}

	private void OnGameLobbyJoinRequested(Lobby _lobby, SteamId _friendId)
	{
		Debug.Log("OnGameLobbyJoinRequested");
		Debug.Log(_lobby.Id);
		Debug.Log(_friendId);
		SteamMatchmaking.JoinLobbyAsync(_lobby.Id);
	}

	private void OnNewLaunchParameters()
	{
		Debug.Log("OnNewLaunchParameters");
		string launchParam = SteamApps.GetLaunchParam("+connect_lobby");
		Debug.Log("[Steam] launchParam -> " + launchParam);
	}

	private void OnChatMessage(Friend _friendId, string _string0, string _string1)
	{
		Debug.Log("OnChatMessage");
		Debug.Log(_friendId);
		Debug.Log(_string0);
		Debug.Log(_string1);
	}

	private void OnGameRichPresenceJoinRequested(Friend _friendId, string _action)
	{
		Debug.Log("OnGameRichPresenceJoinRequested");
		Debug.Log(_friendId);
		Debug.Log(_action);
	}

	private void OnLobbyCreated(Result result, Lobby _lobby)
	{
		lobby = _lobby;
		lobby.SetPublic();
		lobby.SetJoinable(b: true);
		lobby.SetData("RoomName", NetworkManager.Instance.GetRoomName());
		Debug.Log("[Lobby] OnLobbyCreated " + lobby.Id.ToString());
		SteamFriends.OpenGameInviteOverlay(lobby.Id);
	}

	private void OnLobbyMemberJoined(Lobby _lobby, Friend _friendId)
	{
	}

	private void OnLobbyEntered(Lobby _lobby)
	{
		Debug.Log("[Lobby] OnLobbyEntered");
		if (!_lobby.IsOwnedBy(steamId))
		{
			string data = _lobby.GetData("RoomName");
			Debug.Log("Steam wants to join room -> " + data);
			NetworkManager.Instance.WantToJoinRoomName = data;
			steamLoaded = true;
		}
	}

	private void OnLobbyInvite(Friend _friendId, Lobby _lobby)
	{
		Debug.Log("[Lobby] OnLobbyInvite");
		Debug.Log(_friendId);
	}

	public async Task GetLeaderboards(int _index, int _subindex = 1)
	{
		gettingScoreboards = true;
		scoreboardGlobal = null;
		scoreboardFriends = null;
		scoreboardSingle = null;
		Leaderboard? leaderboard = null;
		switch (_index)
		{
		case 0:
			leaderboard = await SteamUserStats.FindLeaderboardAsync("RankingAct4");
			break;
		case 1:
			leaderboard = await SteamUserStats.FindLeaderboardAsync("RankingAct4Coop");
			break;
		case 2:
			leaderboard = await SteamUserStats.FindLeaderboardAsync("Challenge");
			break;
		case 3:
			leaderboard = await SteamUserStats.FindLeaderboardAsync("ChallengeCoop");
			break;
		case 4:
			leaderboard = await SteamUserStats.FindLeaderboardAsync("Weekly" + _subindex);
			break;
		case 5:
			leaderboard = await SteamUserStats.FindLeaderboardAsync("Weekly" + _subindex + "Coop");
			break;
		case 6:
			leaderboard = await SteamUserStats.FindLeaderboardAsync("Singularity");
			break;
		case 7:
			leaderboard = await SteamUserStats.FindLeaderboardAsync("SingularityCoop");
			break;
		}
		if (!leaderboard.HasValue)
		{
			Debug.Log("Couldn't Get Leaderboard!");
		}
		else
		{
			scoreboardGlobal = await leaderboard.Value.GetScoresAsync(450);
			scoreboardFriends = await leaderboard.Value.GetScoresFromFriendsAsync();
			scoreboardSingle = await leaderboard.Value.GetScoresAroundUserAsync(0, 0);
			if (!TomeManager.Instance.IsActive())
			{
				return;
			}
			bool flag = false;
			if (shameList.Contains(steamId.ToString()))
			{
				flag = true;
			}
			if (scoreboardGlobal != null)
			{
				int num = 0;
				LeaderboardEntry[] array = scoreboardGlobal;
				for (int i = 0; i < array.Length; i++)
				{
					LeaderboardEntry leaderboardEntry = array[i];
					Convert.ToInt32(leaderboardEntry.Details[0] + leaderboardEntry.Score * 101);
					if (!flag && shameList.Contains(leaderboardEntry.User.Id.ToString()))
					{
						scoreboardGlobal[num].User.Id = 0uL;
					}
					num++;
				}
			}
		}
		gettingScoreboards = false;
	}

	public async Task Leaderboards(int delay = 100)
	{
		Leaderboard? leaderboard = await SteamUserStats.FindLeaderboardAsync("Ranking");
		if (!leaderboard.HasValue)
		{
			Debug.Log("Couldn't Get Leaderboard!");
		}
		else
		{
			await leaderboard.Value.GetScoresAsync(400);
		}
	}

	public async Task SetScoreLeaderboard(int score, bool singleplayer = true)
	{
		int gameId32 = Functions.StringToAsciiInt32(AtOManager.Instance.GetGameId());
		int details = Convert.ToInt32(gameId32 + score * 101);
		if (singleplayer)
		{
			Leaderboard? leaderboard = await SteamUserStats.FindLeaderboardAsync("RankingAct4");
			if (leaderboard.HasValue)
			{
				await leaderboard.Value.SubmitScoreAsync(score, new int[2] { gameId32, details });
			}
			else
			{
				Debug.Log("Couldn't Get Leaderboard!");
			}
		}
		else
		{
			Leaderboard? leaderboard2 = await SteamUserStats.FindLeaderboardAsync("RankingAct4Coop");
			if (leaderboard2.HasValue)
			{
				await leaderboard2.Value.SubmitScoreAsync(score, new int[2] { gameId32, details });
			}
			else
			{
				Debug.Log("Couldn't Get Leaderboard!");
			}
		}
	}

	public async void SetScore(int score, bool singleplayer = true)
	{
		if (score > 0)
		{
			await SetScoreLeaderboard(score, singleplayer);
		}
	}

	public async void SetObeliskScore(int score, bool singleplayer = true)
	{
		if (score > 0)
		{
			await SetObeliskScoreLeaderboard(score, singleplayer);
		}
	}

	public async void SetSingularityScore(int score, bool singleplayer = true)
	{
		if (score > 0)
		{
			await SetSingularityScoreLeaderboard(score, singleplayer);
		}
	}

	public async Task SetObeliskScoreLeaderboard(int score, bool singleplayer = true)
	{
		int gameId32 = Functions.StringToAsciiInt32(AtOManager.Instance.GetGameId());
		int details = Convert.ToInt32(gameId32 + score * 101);
		if (singleplayer)
		{
			Leaderboard? leaderboard = await SteamUserStats.FindLeaderboardAsync("Challenge");
			if (leaderboard.HasValue)
			{
				await leaderboard.Value.SubmitScoreAsync(score, new int[2] { gameId32, details });
			}
			else
			{
				Debug.Log("Couldn't Get Leaderboard!");
			}
		}
		else
		{
			Leaderboard? leaderboard2 = await SteamUserStats.FindLeaderboardAsync("ChallengeCoop");
			if (leaderboard2.HasValue)
			{
				await leaderboard2.Value.SubmitScoreAsync(score, new int[2] { gameId32, details });
			}
			else
			{
				Debug.Log("Couldn't Get Leaderboard!");
			}
		}
	}

	public async Task SetSingularityScoreLeaderboard(int score, bool singleplayer = true)
	{
		int gameId32 = Functions.StringToAsciiInt32(AtOManager.Instance.GetGameId());
		int details = Convert.ToInt32(gameId32 + score * 101);
		if (singleplayer)
		{
			Leaderboard? leaderboard = await SteamUserStats.FindLeaderboardAsync("Singularity");
			if (leaderboard.HasValue)
			{
				await leaderboard.Value.SubmitScoreAsync(score, new int[2] { gameId32, details });
			}
			else
			{
				Debug.Log("Couldn't Get Leaderboard!");
			}
		}
		else
		{
			Leaderboard? leaderboard2 = await SteamUserStats.FindLeaderboardAsync("SingularityCoop");
			if (leaderboard2.HasValue)
			{
				await leaderboard2.Value.SubmitScoreAsync(score, new int[2] { gameId32, details });
			}
			else
			{
				Debug.Log("Couldn't Get Leaderboard!");
			}
		}
	}

	public async void SetWeeklyScore(int score, int week, string nick, string nickgroup, bool singleplayer = true)
	{
		if (score > 0)
		{
			await SetWeeklyScoreLeaderboard(score, week, nick, nickgroup, singleplayer);
		}
	}

	public async Task SetWeeklyScoreLeaderboard(int score, int week, string nick, string nickgroup, bool singleplayer = true)
	{
		int gameId32 = Functions.StringToAsciiInt32(AtOManager.Instance.GetGameId());
		int details = Convert.ToInt32(gameId32 + score * 101);
		Debug.Log(details);
		if (singleplayer)
		{
			Leaderboard? leaderboard = await SteamUserStats.FindOrCreateLeaderboardAsync("Weekly" + week, LeaderboardSort.Descending, LeaderboardDisplay.Numeric);
			if (leaderboard.HasValue)
			{
				await leaderboard.Value.SubmitScoreAsync(score, new int[2] { gameId32, details });
			}
			else
			{
				Debug.Log("Couldn't Get Leaderboard!");
			}
		}
		else
		{
			Leaderboard? leaderboard2 = await SteamUserStats.FindOrCreateLeaderboardAsync("Weekly" + week + "Coop", LeaderboardSort.Descending, LeaderboardDisplay.Numeric);
			if (leaderboard2.HasValue)
			{
				await leaderboard2.Value.SubmitScoreAsync(score, new int[2] { gameId32, details });
			}
			else
			{
				Debug.Log("Couldn't Get Leaderboard!");
			}
		}
	}

	public void SetRichPresence(string key, string value)
	{
		if (steamConnected)
		{
			SteamFriends.SetRichPresence(key, value);
		}
	}

	public void InviteSteam()
	{
		SteamMatchmaking.CreateLobbyAsync(4);
	}

	public Dictionary<string, string> GetFriends(bool onlyOnline = false)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (Friend friend in SteamFriends.GetFriends())
		{
			if (!onlyOnline || friend.IsOnline)
			{
				string key = friend.Name;
				SteamId id = friend.Id;
				dictionary.Add(key, id.ToString());
				string text = friend.Name;
				id = friend.Id;
				Debug.Log(text + "->" + id.ToString());
			}
		}
		return dictionary;
	}
}
