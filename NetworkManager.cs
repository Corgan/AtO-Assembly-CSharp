using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ExitGames.Client.Photon;
using Paradox;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Steamworks;
using Steamworks.Data;
using UnityEngine;
using WebSocketSharp;

public class NetworkManager : MonoBehaviourPunCallbacks
{
	[SerializeField]
	public Dictionary<string, bool> PlayerReady;

	public Dictionary<string, Dictionary<string, bool>> PlayerStatusReady;

	[SerializeField]
	public Dictionary<string, bool> PlayerManualReady;

	[SerializeField]
	public Dictionary<string, bool> PlayerDivinationReady;

	private new PhotonView photonView;

	private string gameVersion = "1";

	private bool onRoom;

	public Player[] PlayerList;

	public string[] PlayerPositionList;

	public string[] PlayerHeroPositionOwner;

	public string netAuxValue = "";

	public string Owner0 = "";

	public string Owner1 = "";

	public string Owner2 = "";

	public string Owner3 = "";

	public List<string> PlayerMuteList;

	public Dictionary<string, string> PlayerNickRealDict;

	public Dictionary<string, string> PlayerVersionDict;

	public Dictionary<string, string> PlayerPlatformDict;

	public Dictionary<string, string> PlayerPDXUserDict;

	public Dictionary<string, List<string>> PlayerSkuList;

	public Dictionary<string, string> PlayerPing;

	private string realName = "";

	public string regionSelected = "";

	public Dictionary<string, bool> WaitingSyncro = new Dictionary<string, bool>();

	private PhotonLagSimulationGui lagSimulator;

	private PhotonStatsGui statsGui;

	public string WantToJoinRoomName = "";

	public List<string> waitingCalls = new List<string>();

	private int rrc;

	public int networkDisconnectAlert;

	private AuthTicket hAuthTicket;

	private Coroutine connTimeoutCo;

	public static NetworkManager Instance { get; private set; }

	private void Update()
	{
		if (Globals.Instance.ShowDebug && PhotonNetwork.IsConnected && Time.frameCount % 24 == 0 && rrc != PhotonNetwork.ResentReliableCommands)
		{
			rrc = PhotonNetwork.ResentReliableCommands;
			Debug.LogError("PhotonNetwork.ResentReliableCommands=>" + rrc);
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
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		photonView = PhotonView.Get(this);
		PhotonNetwork.LogLevel = PunLogLevel.ErrorsOnly;
		PlayerReady = new Dictionary<string, bool>();
		PlayerStatusReady = new Dictionary<string, Dictionary<string, bool>>();
		PlayerManualReady = new Dictionary<string, bool>();
		PlayerDivinationReady = new Dictionary<string, bool>();
		PlayerNickRealDict = new Dictionary<string, string>();
		PlayerVersionDict = new Dictionary<string, string>();
		PlayerPlatformDict = new Dictionary<string, string>();
		PlayerPDXUserDict = new Dictionary<string, string>();
		PlayerSkuList = new Dictionary<string, List<string>>();
		PlayerPing = new Dictionary<string, string>();
		lagSimulator = GetComponent<PhotonLagSimulationGui>();
		ShowLagSimulator(state: false);
		statsGui = GetComponent<PhotonStatsGui>();
		GameManager.Instance.SceneLoaded();
	}

	private void Start()
	{
		PhotonNetwork.SendRate = 20;
		PhotonNetwork.SerializationRate = 10;
		PhotonNetwork.UseRpcMonoBehaviourCache = true;
	}

	public bool IsConnected()
	{
		return PhotonNetwork.IsConnected;
	}

	public bool IsOnRoom()
	{
		return onRoom;
	}

	public string GetPing()
	{
		if (PhotonNetwork.IsConnected)
		{
			return PhotonNetwork.GetPing().ToString();
		}
		return "";
	}

	public void SendPing(string ping)
	{
		photonView.RPC("SendPingCo", RpcTarget.All, GetPlayerNick(), ping, (byte)GetMyPosition());
	}

	[PunRPC]
	public void SendPingCo(string _nick, string _ping, byte _slot)
	{
		if (PlayerPing.ContainsKey(_nick))
		{
			PlayerPing[_nick] = _ping;
		}
		else
		{
			PlayerPing.Add(_nick, _ping);
		}
		AlertManager.Instance.UpdatePlayer(_slot);
	}

	public void StartStopQueue(bool state)
	{
		if (GameManager.Instance.IsMultiplayer() && PhotonNetwork.IsConnected)
		{
			PhotonNetwork.IsMessageQueueRunning = state;
		}
	}

	public void ClearSyncro()
	{
		if (WaitingSyncro != null)
		{
			WaitingSyncro.Clear();
		}
		else
		{
			WaitingSyncro = new Dictionary<string, bool>();
		}
	}

	public void ClearPlayerStatus()
	{
		PlayerStatusReady = new Dictionary<string, Dictionary<string, bool>>();
	}

	public bool IsSyncroClean()
	{
		if (WaitingSyncro.Count == 0)
		{
			return true;
		}
		foreach (KeyValuePair<string, bool> item in WaitingSyncro)
		{
			if (item.Value)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("*WAITING SYNCRO* " + item.Key);
				}
				return false;
			}
		}
		return true;
	}

	public void ShowLagSimulator(bool state)
	{
		lagSimulator.Visible = state;
	}

	public void ShowLagSimulatorTrigger()
	{
		lagSimulator.Visible = !lagSimulator.Visible;
	}

	public void JoinLobby()
	{
		PhotonNetwork.JoinLobby(TypedLobby.Default);
	}

	public void AddPlayerSkuList(string _nick, List<string> _list)
	{
		if (PlayerSkuList == null)
		{
			PlayerSkuList = new Dictionary<string, List<string>>();
		}
		_nick = GetPlayerNickReal(_nick);
		if (PlayerSkuList.ContainsKey(_nick))
		{
			PlayerSkuList[_nick] = _list;
		}
		else
		{
			PlayerSkuList.Add(_nick, _list);
		}
	}

	public bool PlayerHaveSku(string _nick, string _sku)
	{
		if (PlayerSkuList != null && PlayerSkuList.ContainsKey(_nick) && PlayerSkuList[_nick] != null && PlayerSkuList[_nick].Contains(_sku))
		{
			return true;
		}
		return false;
	}

	public bool AllPlayersHaveSku(string _sku)
	{
		if (PlayerSkuList == null)
		{
			return false;
		}
		foreach (KeyValuePair<string, List<string>> playerSku in PlayerSkuList)
		{
			if (playerSku.Value == null)
			{
				return false;
			}
			if (!playerSku.Value.Contains(_sku))
			{
				if (GameManager.Instance.GetDeveloperMode())
				{
					Debug.LogError(playerSku.Key + " doesn't have sku " + _sku);
				}
				return false;
			}
		}
		return true;
	}

	public bool AnyPlayersHaveSku(string _sku)
	{
		if (PlayerSkuList == null)
		{
			return false;
		}
		foreach (KeyValuePair<string, List<string>> playerSku in PlayerSkuList)
		{
			if (playerSku.Value != null && playerSku.Value.Contains(_sku))
			{
				return true;
			}
		}
		return false;
	}

	public bool AllPlayersHaveSkuList()
	{
		if (PlayerSkuList.Count == GetNumPlayers())
		{
			return true;
		}
		return false;
	}

	public void Connect(string region = "")
	{
		if (PhotonNetwork.IsConnected)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("already connected");
			}
			ConnectedToMaster();
			return;
		}
		PlayerNickRealDict.Clear();
		PlayerVersionDict.Clear();
		PlayerPlatformDict.Clear();
		PlayerSkuList.Clear();
		PlayerPing.Clear();
		if (SteamManager.Instance.steamName != "")
		{
			string steamName = SteamManager.Instance.steamName;
			steamName = steamName.Replace(" ", "").Replace("_", "");
			realName = steamName;
		}
		else
		{
			realName = PlayerManager.Instance.GetPlayerName();
		}
		PhotonNetwork.NickName = realName;
		int num = 0;
		while (PlayerNickRealDict.ContainsKey(PhotonNetwork.NickName) && num < 20)
		{
			PhotonNetwork.NickName = realName + "_" + Functions.RandomString(4f);
			num++;
		}
		if (!PlayerNickRealDict.ContainsKey(PhotonNetwork.NickName))
		{
			PlayerNickRealDict.Add(PhotonNetwork.NickName, realName);
		}
		if (!PlayerVersionDict.ContainsKey(PhotonNetwork.NickName))
		{
			PlayerVersionDict.Add(PhotonNetwork.NickName, GameManager.Instance.gameVersion);
		}
		if (!PlayerPlatformDict.ContainsKey(PhotonNetwork.NickName))
		{
			PlayerPlatformDict.Add(PhotonNetwork.NickName, Globals.Instance.ConfigPlatform.ToString());
		}
		PhotonNetwork.GameVersion = gameVersion;
		PhotonNetwork.NetworkingClient.ServerPortOverrides = PhotonPortDefinition.AlternativeUdpPorts;
		PhotonNetwork.QuickResends = 3;
		PhotonNetwork.MaxResendsBeforeDisconnect = 8;
		PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 60000;
		PhotonNetwork.NetworkingClient.LoadBalancingPeer.MaximumTransferUnit = 520;
		PhotonNetwork.KeepAliveInBackground = 60000f;
		PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = region;
		regionSelected = region;
		string steamAuthTicket = GetSteamAuthTicket(out hAuthTicket);
		PhotonNetwork.AuthValues = new AuthenticationValues();
		PhotonNetwork.AuthValues.UserId = SteamManager.Instance.steamId.ToString();
		if (!GameManager.Instance.DisableSteamAuthorizationForPhoton)
		{
			PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Steam;
		}
		PhotonNetwork.AuthValues.AddAuthParameter("ticket", steamAuthTicket);
		PhotonNetwork.ConnectUsingSettings();
		connTimeoutCo = StartCoroutine(ConnectionTimeout());
	}

	private IEnumerator ConnectionTimeout()
	{
		yield return Globals.Instance.WaitForSeconds(15f);
		if (LobbyManager.Instance.regionsDisconnect != null && !LobbyManager.Instance.regionsDisconnect.gameObject.activeSelf)
		{
			Disconnect();
		}
	}

	public void Disconnect()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("Disconnect", "net");
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("Disconnect");
		}
		PhotonNetwork.Disconnect();
		regionSelected = "";
	}

	public void ChatSend(string _text, bool showAlertIfClosed, string _playerNick)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(_text);
		photonView.RPC("GotChat", RpcTarget.Others, bytes, showAlertIfClosed, _playerNick);
	}

	[PunRPC]
	public void GotChat(byte[] _text, bool showAlertIfClosed, string _playerNick)
	{
		if (_playerNick.IsNullOrEmpty() || !Instance.IsPlayerMutedByNick(_playerNick))
		{
			string text = Encoding.UTF8.GetString(_text);
			ChatManager.Instance.ChatText(text, showAlertIfClosed);
		}
		else
		{
			Debug.Log("Blocked communication because player is muted");
		}
	}

	public void CreateRoom(string roomName, string roomPlayers, string roomPassword = "", string lfm = "")
	{
		if (onRoom)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(roomName);
		stringBuilder.Append("<br><size=20><color=#A0A0A0>");
		if (AtOManager.Instance.GetGameId() != "")
		{
			stringBuilder.Append(" <sprite name=disk> ");
		}
		string value = "";
		if (GameManager.Instance.GameType == Enums.GameType.Adventure)
		{
			stringBuilder.Append(Texts.Instance.GetText("modeAdventure"));
			if (AtOManager.Instance.GetGameId() != "")
			{
				stringBuilder.Append(" <color=#666>|</color> ");
				value = string.Format(Texts.Instance.GetText("madnessNumber"), AtOManager.Instance.GetMadnessDifficulty().ToString());
			}
		}
		else if (GameManager.Instance.GameType == Enums.GameType.Challenge)
		{
			stringBuilder.Append(Texts.Instance.GetText("modeObelisk"));
			if (AtOManager.Instance.GetGameId() != "")
			{
				stringBuilder.Append(" <color=#666>|</color> ");
				value = string.Format(Texts.Instance.GetText("madnessNumber"), AtOManager.Instance.GetObeliskMadness().ToString());
			}
		}
		else if (GameManager.Instance.GameType == Enums.GameType.WeeklyChallenge)
		{
			stringBuilder.Append(Texts.Instance.GetText("modeWeekly"));
			stringBuilder.Append(" <color=#666>|</color> ");
			value = ((!(AtOManager.Instance.GetGameId() != "")) ? AtOManager.Instance.GetWeeklyName(Functions.GetCurrentWeeklyWeek()) : AtOManager.Instance.GetWeeklyName(AtOManager.Instance.GetWeekly()));
		}
		else if (GameManager.Instance.GameType == Enums.GameType.Singularity)
		{
			stringBuilder.Append(Texts.Instance.GetText("singularity"));
			if (AtOManager.Instance.GetGameId() != "")
			{
				stringBuilder.Append(" <color=#666>|</color> ");
				value = "";
			}
		}
		stringBuilder.Append(value);
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable.Add("pwd", roomPassword);
		hashtable.Add("creator", GetPlayerNickReal(PhotonNetwork.NickName));
		hashtable.Add("description", stringBuilder.ToString());
		hashtable.Add("status", "lobby");
		hashtable.Add("version", GameManager.Instance.gameVersion.ToString());
		hashtable.Add("lfm", lfm);
		hashtable.Add("platform", Globals.Instance.ConfigPlatform.ToString());
		hashtable.Add("crossplay", GameManager.Instance.ConfigCrossPlayEnabled ? "1" : "0");
		roomName = Functions.RandomStringSafe(6f).ToUpper();
		string[] customRoomPropertiesForLobby = new string[8] { "pwd", "creator", "description", "status", "version", "lfm", "platform", "crossplay" };
		PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions
		{
			PlayerTtl = 1000,
			EmptyRoomTtl = 1000,
			MaxPlayers = byte.Parse(roomPlayers),
			CustomRoomProperties = hashtable,
			CustomRoomPropertiesForLobby = customRoomPropertiesForLobby,
			PublishUserId = true,
			IsOpen = true,
			IsVisible = true
		}, TypedLobby.Default);
		PlayerHeroPositionOwner = new string[4];
	}

	public void JoinRoom(string roomName)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("JoinRoom " + roomName);
		}
		PhotonNetwork.JoinRoom(roomName);
	}

	public void JoinRoomByPreloadedSteam()
	{
		if (WantToJoinRoomName != "")
		{
			JoinRoom(WantToJoinRoomName);
			WantToJoinRoomName = "";
		}
	}

	public void ExitRoom()
	{
		Debug.Log("ExitRoom");
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("Exitroom");
		}
		CloseRoom();
		if (PhotonNetwork.IsConnected && PhotonNetwork.CurrentRoom != null)
		{
			PhotonNetwork.LeaveRoom();
		}
	}

	public void LoadScene(string scene, bool showMask = true)
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			GlobalLog.Instance.Log("PhotonNetwork", "Trying to Load a level but we are not the master Client");
		}
		int num = 0;
		if (GameManager.Instance.IsObeliskChallenge())
		{
			num = ((!GameManager.Instance.IsWeeklyChallenge()) ? 1 : 2);
		}
		else if (GameManager.Instance.IsSingularity())
		{
			num = 3;
		}
		photonView.RPC("NET_LoadScene", RpcTarget.All, scene, showMask, num);
	}

	[PunRPC]
	public void NET_LoadScene(string scene, bool showMask, int gameType)
	{
		UnityEngine.Object.Destroy(GameObject.Find("CardAmplifyOutside"));
		GameManager.Instance.SetMaskLoading();
		if (scene == "HeroSelection")
		{
			switch (gameType)
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
		}
		SceneStatic.LoadByName(scene);
	}

	public string GetRoomDescription()
	{
		return PhotonNetwork.NetworkingClient.CurrentRoom.CustomProperties["description"].ToString();
	}

	public string GetRoomPassword()
	{
		string text = PhotonNetwork.NetworkingClient.CurrentRoom.CustomProperties["pwd"].ToString();
		if (text != "")
		{
			text = new SimplerAES().Decrypt(text);
		}
		return text;
	}

	public string GetRoomName()
	{
		if (PhotonNetwork.NetworkingClient.CurrentRoom != null)
		{
			return PhotonNetwork.NetworkingClient.CurrentRoom.Name;
		}
		return "";
	}

	public int GetNumPlayers()
	{
		int num = 0;
		if (PlayerPositionList != null)
		{
			for (int i = 0; i < PlayerPositionList.Length; i++)
			{
				if (PlayerPositionList[i] != null && PlayerPositionList[i] != "")
				{
					num++;
				}
			}
		}
		return num;
	}

	public int GetMaxPlayers()
	{
		return PhotonNetwork.NetworkingClient.CurrentRoom.MaxPlayers;
	}

	public Player GetPlayer()
	{
		return PhotonNetwork.LocalPlayer;
	}

	public bool VersionMatch()
	{
		for (int i = 0; i < PlayerPositionList.Length; i++)
		{
			if (PlayerPositionList[i] != null)
			{
				if (PlayerVersionDict == null || !PlayerVersionDict.ContainsKey(PlayerPositionList[i]))
				{
					return false;
				}
				if (!Functions.CompatibleVersion(PlayerVersionDict[PlayerPositionList[i]], GameManager.Instance.gameVersion))
				{
					return false;
				}
			}
		}
		return true;
	}

	public string GetPlatformString(int slot)
	{
		if (slot > -1 && PlayerPositionList != null && slot < PlayerPositionList.Length && !PlayerPositionList[slot].IsNullOrEmpty())
		{
			return PlayerPlatformDict[PlayerPositionList[slot]];
		}
		return "";
	}

	public Sprite GetSlotPlatformImage(int slot)
	{
		bool flag = false;
		if (PlayerPlatformDict == null || !PlayerPlatformDict.ContainsKey(PlayerPositionList[slot]))
		{
			flag = true;
		}
		else if (PlayerPlatformDict[PlayerPositionList[slot]] == Globals.Instance.ConfigPlatform.ToString())
		{
			flag = true;
		}
		if (flag)
		{
			return GetPlatformImage(Globals.Instance.ConfigPlatform.ToString());
		}
		return GetPlatformImage();
	}

	private Sprite GetPlatformImage(string platform = "")
	{
		if (platform == Enums.Platform.PC.ToString())
		{
			return Globals.Instance.pdxLogo;
		}
		if (platform == Enums.Platform.Switch.ToString())
		{
			return Globals.Instance.switchLogo;
		}
		if (platform == Enums.Platform.PS5.ToString())
		{
			return Globals.Instance.psLogo;
		}
		if (platform == Enums.Platform.XBOX.ToString())
		{
			return Globals.Instance.xboxLogo;
		}
		return Globals.Instance.pdxLogo;
	}

	public string GetMyNickReal()
	{
		return realName;
	}

	public string GetPlayerNickReal(string nick)
	{
		if (nick != "" && nick != null && PlayerNickRealDict != null && PlayerNickRealDict.ContainsKey(nick))
		{
			return PlayerNickRealDict[nick];
		}
		return nick;
	}

	public string GetPlayerVersion(string nick)
	{
		if (nick != "" && nick != null && PlayerVersionDict != null && PlayerVersionDict.ContainsKey(nick))
		{
			return PlayerVersionDict[nick];
		}
		return nick;
	}

	public string GetPlayerNick()
	{
		if (PhotonNetwork.IsConnected)
		{
			return GetPlayer().NickName;
		}
		return Globals.Instance.DefaultNickName;
	}

	public string[] PlayerPositionListArray()
	{
		return PlayerPositionList;
	}

	public void NormalizePlayerPositionList()
	{
		int num = 0;
		for (int i = 0; i < PlayerPositionList.Length; i++)
		{
			if (PlayerPositionList[i] != null && PlayerPositionList[i] != "")
			{
				num++;
			}
		}
		if (num >= PlayerPositionList.Length)
		{
			return;
		}
		string[] array = new string[num];
		int num2 = 0;
		for (int j = 0; j < PlayerPositionList.Length; j++)
		{
			if (PlayerPositionList[j] != null && PlayerPositionList[j] != "")
			{
				array[num2] = PlayerPositionList[j];
				num2++;
			}
		}
		PlayerPositionList = new string[num];
		for (int k = 0; k < PlayerPositionList.Length; k++)
		{
			PlayerPositionList[k] = array[k];
		}
		string text = JsonHelper.ToJson(PlayerPositionList);
		photonView.RPC("NET_SharePlayerPositionList", RpcTarget.Others, text);
	}

	public string GetPlayerNickPosition(int position)
	{
		if (PlayerPositionList != null && position > -1 && position < PlayerPositionList.Length)
		{
			return PlayerPositionList[position];
		}
		return "";
	}

	public int GetPlayerListPosition(string nick)
	{
		int result = -1;
		if (PlayerPositionList != null)
		{
			for (int i = 0; i < PlayerPositionList.Length; i++)
			{
				if (PlayerPositionList[i] == nick)
				{
					result = i;
					break;
				}
			}
		}
		return result;
	}

	public int GetMyPosition()
	{
		return GetPlayerListPosition(GetPlayerNick());
	}

	public bool PlayerIsMaster(string _nick)
	{
		return GetPlayerListPosition(_nick) == 0;
	}

	public bool IsMaster()
	{
		if (!PhotonNetwork.IsConnected)
		{
			return false;
		}
		return GetPlayer().IsMasterClient;
	}

	public void OpenRoomDoors()
	{
		PhotonNetwork.CurrentRoom.IsOpen = true;
	}

	public void CloseRoomDoors()
	{
		PhotonNetwork.CurrentRoom.IsOpen = false;
	}

	public void CloseRoom(bool disconnect = false)
	{
		if (PhotonNetwork.IsConnected)
		{
			if (PhotonNetwork.IsMasterClient)
			{
				_ = (bool)LobbyManager.Instance;
			}
			if (disconnect)
			{
				StartCoroutine(CoDisconnect());
			}
		}
	}

	private IEnumerator CoDisconnect()
	{
		yield return Globals.Instance.WaitForSeconds(0.1f);
		PhotonNetwork.Disconnect();
	}

	public void SetRoomStatus(string newStatus)
	{
	}

	public string GetColorFromNick(string nick)
	{
		return ColorFromPosition(GetPlayerListPosition(nick));
	}

	public string ColorFromPosition(int position)
	{
		return position switch
		{
			0 => Globals.Instance.ClassColor["warrior"], 
			1 => Globals.Instance.ClassColor["mage"], 
			2 => Globals.Instance.ClassColor["scout"], 
			_ => Globals.Instance.ClassColor["magicknight"], 
		};
	}

	private void CreateStatus(string matchStatus)
	{
		Functions.DebugLogGD("[NET] CreateStatus " + matchStatus);
		if (PlayerStatusReady == null)
		{
			Functions.DebugLogGD("[NET] Null");
			PlayerStatusReady = new Dictionary<string, Dictionary<string, bool>>();
		}
		if (PlayerStatusReady.ContainsKey(matchStatus))
		{
			Functions.DebugLogGD("[NET] Contains");
			if (PlayerStatusReady[matchStatus] == null)
			{
				PlayerStatusReady[matchStatus] = new Dictionary<string, bool>();
			}
			else
			{
				PlayerStatusReady[matchStatus].Clear();
			}
		}
		else
		{
			Functions.DebugLogGD("[NET] Not contains");
			PlayerStatusReady.Add(matchStatus, new Dictionary<string, bool>());
		}
		string playerNick = GetPlayerNick();
		Player[] playerList = PlayerList;
		foreach (Player player in playerList)
		{
			PlayerStatusReady[matchStatus].Add(player.NickName, player.NickName == playerNick);
		}
	}

	public void ClearAllPlayerReadyStatus(string matchStatus)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("Clearing all players status ->" + matchStatus);
		}
		if (PlayerStatusReady == null || !PlayerStatusReady.ContainsKey(matchStatus))
		{
			CreateStatus(matchStatus);
		}
		else
		{
			PlayerStatusReady[matchStatus].Clear();
		}
	}

	public void SetStatusReady(string matchStatus)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("SetPlayerStatusReady " + matchStatus);
		}
		photonView.RPC("NET_SetPlayerStatusReady", RpcTarget.MasterClient, true, matchStatus, (byte)GetPlayerListPosition(GetPlayerNick()));
	}

	[PunRPC]
	private void NET_SetPlayerStatusReady(bool status, string matchStatus, byte _position)
	{
		string playerNickPosition = GetPlayerNickPosition(_position);
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD(playerNickPosition + " was about to set ready for " + matchStatus, "net");
		}
		if (PlayerStatusReady == null || !PlayerStatusReady.ContainsKey(matchStatus) || PlayerStatusReady[matchStatus] == null)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("dictionary is not ready", "net");
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD(playerNickPosition + " != " + GetPlayerNick(), "net");
			}
			if (playerNickPosition != GetPlayerNick())
			{
				string text = playerNickPosition + "%" + matchStatus;
				if (!waitingCalls.Contains(text))
				{
					if (Globals.Instance.ShowDebug)
					{
						Debug.LogWarning("SAVED FOR LATER " + text);
					}
					waitingCalls.Add(text);
				}
				else if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("waitingCalls already contains" + text, "net");
				}
			}
			else if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD(playerNickPosition + " == " + GetPlayerNick(), "net");
			}
		}
		else if (PlayerStatusReady.ContainsKey(matchStatus))
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("PlayerStatusReady contains " + matchStatus, "net");
			}
			if (PlayerStatusReady[matchStatus] == null)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("status " + matchStatus + " was NULL", "net");
				}
				PlayerStatusReady[matchStatus] = new Dictionary<string, bool>();
			}
			if (PlayerStatusReady[matchStatus].ContainsKey(playerNickPosition))
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("status " + matchStatus + " modified for " + playerNickPosition + " -> " + status, "net");
				}
				PlayerStatusReady[matchStatus][playerNickPosition] = status;
			}
			else
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("status " + matchStatus + " created for " + playerNickPosition + " -> " + status, "net");
				}
				PlayerStatusReady[matchStatus].Add(playerNickPosition, status);
			}
		}
		else if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("PlayerStatusReady not containskey " + matchStatus, "net");
		}
	}

	public bool AllPlayersReady(string matchStatus)
	{
		if (PlayerStatusReady == null || !PlayerStatusReady.ContainsKey(matchStatus) || PlayerStatusReady[matchStatus] == null)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("PlayerStatusReady Not Exist. Create matchStatus -> " + matchStatus, "net");
			}
			CreateStatus(matchStatus);
		}
		for (int num = waitingCalls.Count - 1; num >= 0; num--)
		{
			string[] array = waitingCalls[num].Split('%');
			string text = "";
			for (int i = 0; i < array.Length - 1; i++)
			{
				text += array[i];
				if (i < array.Length - 2)
				{
					text += "%";
				}
			}
			if (array[^1] == matchStatus)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[AllPlayerSReady] Insert " + waitingCalls[num], "net");
				}
				if (PlayerStatusReady[matchStatus].ContainsKey(text))
				{
					PlayerStatusReady[matchStatus][text] = true;
				}
				else
				{
					PlayerStatusReady[matchStatus].Add(text, value: true);
				}
			}
		}
		if (PlayerStatusReady[matchStatus].Count < PlayerList.Length - 1)
		{
			return false;
		}
		string playerNick = GetPlayerNick();
		foreach (KeyValuePair<string, bool> item in PlayerStatusReady[matchStatus])
		{
			if (!item.Value && item.Key != playerNick)
			{
				return false;
			}
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("Clear PlayerStatusReady for " + matchStatus, "net");
		}
		PlayerStatusReady[matchStatus].Clear();
		ClearWaitingCalls(matchStatus);
		return true;
	}

	public void ClearWaitingCalls(string _matchStatus = "")
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("Clear Waiting Calls ->  " + _matchStatus, "net");
		}
		if (_matchStatus == "")
		{
			waitingCalls.Clear();
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("%");
		stringBuilder.Append(_matchStatus);
		for (int num = waitingCalls.Count - 1; num >= 0; num--)
		{
			if (waitingCalls[num].Contains(stringBuilder.ToString()))
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Clear Waiting Calls (REMOVED) ->  " + waitingCalls[num], "net");
				}
				waitingCalls.RemoveAt(num);
			}
		}
		stringBuilder = null;
	}

	public void PlayersNetworkContinue(string key, string auxValue = "")
	{
		photonView.RPC("NET_NetworkContinue", RpcTarget.Others, key, auxValue);
	}

	[PunRPC]
	public void NET_NetworkContinue(string key, string auxValue)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("Received continue sync signal from master -> " + key, "net");
		}
		netAuxValue = auxValue;
		SetWaitingSyncro(key, status: false);
	}

	public void SetWaitingSyncro(string key, bool status)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("SetWaitingSyncro " + key + " -> " + status);
		}
		if (WaitingSyncro == null)
		{
			WaitingSyncro = new Dictionary<string, bool>();
		}
		if (WaitingSyncro.ContainsKey(key))
		{
			WaitingSyncro[key] = status;
		}
		else
		{
			WaitingSyncro.Add(key, status);
		}
	}

	public void ClearAllPlayerManualReady()
	{
		PlayerManualReady.Clear();
		if (PlayerList != null)
		{
			Player[] playerList = PlayerList;
			foreach (Player player in playerList)
			{
				PlayerManualReady.Add(player.NickName, value: false);
			}
		}
	}

	public void SetManualReady(bool status)
	{
		photonView.RPC("NET_SetPlayerManualReady", RpcTarget.MasterClient, status, GetPlayerNick());
	}

	[PunRPC]
	private void NET_SetPlayerManualReady(bool status, string nickname)
	{
		PlayerManualReady[nickname] = status;
		DoReadyStatus();
		string[] array = new string[PlayerManualReady.Count];
		PlayerManualReady.Keys.CopyTo(array, 0);
		bool[] array2 = new bool[PlayerManualReady.Count];
		PlayerManualReady.Values.CopyTo(array2, 0);
		string text = JsonHelper.ToJson(array);
		string text2 = JsonHelper.ToJson(array2);
		photonView.RPC("NET_SharePlayerManualReady", RpcTarget.Others, text, text2);
	}

	[PunRPC]
	private void NET_SharePlayerManualReady(string _keys, string _values)
	{
		PlayerManualReady = new Dictionary<string, bool>();
		string[] array = JsonHelper.FromJson<string>(_keys);
		bool[] array2 = JsonHelper.FromJson<bool>(_values);
		for (int i = 0; i < array.Length; i++)
		{
			PlayerManualReady.Add(array[i], array2[i]);
		}
		DoReadyStatus();
	}

	public bool IsPlayerReady(string _nick)
	{
		foreach (KeyValuePair<string, bool> item in PlayerManualReady)
		{
			if (item.Value && _nick == item.Key)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsMasterReady()
	{
		foreach (KeyValuePair<string, bool> item in PlayerManualReady)
		{
			if (item.Value && PlayerIsMaster(item.Key))
			{
				return true;
			}
		}
		return false;
	}

	private void DoReadyStatus()
	{
		int num = 0;
		foreach (KeyValuePair<string, bool> item in PlayerManualReady)
		{
			if (item.Value)
			{
				num++;
			}
		}
		if (HeroSelectionManager.Instance != null)
		{
			HeroSelectionManager.Instance.SetPlayersReady();
		}
		else if (TownManager.Instance != null)
		{
			TownManager.Instance.SetWaitingPlayersText(GetWaitingPlayersString(num, PlayerManualReady.Count));
		}
		else if (ChallengeSelectionManager.Instance != null)
		{
			ChallengeSelectionManager.Instance.SetWaitingPlayersText(GetWaitingPlayersString(num, PlayerManualReady.Count));
		}
		else if (CardCraftManager.Instance != null)
		{
			CardCraftManager.Instance.SetWaitingPlayersText(GetWaitingPlayersString(num, PlayerManualReady.Count));
		}
		else if (EventManager.Instance != null)
		{
			EventManager.Instance.SetWaitingPlayersText(GetWaitingPlayersString(num, PlayerManualReady.Count));
		}
	}

	public string GetWaitingPlayersString(int ready, int total)
	{
		if (ready > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(");
			stringBuilder.Append(ready);
			stringBuilder.Append("/");
			stringBuilder.Append(total);
			stringBuilder.Append(")");
			string value = stringBuilder.ToString();
			stringBuilder.Clear();
			stringBuilder.Append(Texts.Instance.GetText("playersReady"));
			stringBuilder.Append(" ");
			stringBuilder.Append(value);
			return stringBuilder.ToString();
		}
		return "";
	}

	public bool AllPlayersManualReady()
	{
		foreach (KeyValuePair<string, bool> item in PlayerManualReady)
		{
			if (!item.Value)
			{
				return false;
			}
		}
		return true;
	}

	public void ClearAllPlayerDivinationReady()
	{
		PlayerDivinationReady.Clear();
		Player[] playerList = PlayerList;
		foreach (Player player in playerList)
		{
			PlayerDivinationReady.Add(player.NickName, value: false);
		}
	}

	public void SetPlayerDivinationReady(string nickname)
	{
		PlayerDivinationReady[nickname] = true;
		bool flag = true;
		foreach (KeyValuePair<string, bool> item in PlayerDivinationReady)
		{
			if (!item.Value)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			AtOManager.Instance.LaunchRewards(isFromDivination: true);
			return;
		}
		string[] array = new string[PlayerDivinationReady.Count];
		PlayerDivinationReady.Keys.CopyTo(array, 0);
		bool[] array2 = new bool[PlayerDivinationReady.Count];
		PlayerDivinationReady.Values.CopyTo(array2, 0);
		string text = JsonHelper.ToJson(array);
		string text2 = JsonHelper.ToJson(array2);
		photonView.RPC("NET_SharePlayerDivinationReady", RpcTarget.Others, text, text2);
		AtOManager.Instance.UpdateDivinationStatus();
	}

	[PunRPC]
	private void NET_SharePlayerDivinationReady(string _keys, string _values)
	{
		PlayerDivinationReady = new Dictionary<string, bool>();
		string[] array = JsonHelper.FromJson<string>(_keys);
		bool[] array2 = JsonHelper.FromJson<bool>(_values);
		for (int i = 0; i < array.Length; i++)
		{
			PlayerDivinationReady.Add(array[i], array2[i]);
		}
		AtOManager.Instance.UpdateDivinationStatus();
	}

	public override void OnLeftRoom()
	{
		Debug.Log("OnLeftRoom");
		onRoom = false;
		if (ChatManager.Instance != null)
		{
			ChatManager.Instance.DisableChat();
		}
		if (LobbyManager.Instance != null && PhotonNetwork.IsConnected)
		{
			LobbyManager.Instance.ShowJoin();
		}
	}

	public override void OnCreatedRoom()
	{
		onRoom = true;
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("OnCreatedRoom() called by PUN.");
		}
		CreatePlayerPositions(GetMaxPlayers());
	}

	public override void OnJoinedRoom()
	{
		onRoom = true;
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("OnJoinedRoom() called by PUN. Now this client is in a room.");
		}
		ChatManager.Instance.EnableChat();
		ChatManager.Instance.WelcomeMsg(GetRoomName());
		LobbyManager.Instance.ShowRoom();
		LobbyManager.Instance.RemoveAllRooms();
		photonView.RPC("SetPlayerPosition", RpcTarget.MasterClient, realName, GetPlayerNick(), GameManager.Instance.gameVersion, Globals.Instance.ConfigPlatform, Startup.userId);
	}

	private void OnJoinRoomFailed(object[] codeAndMsg)
	{
		Debug.LogErrorFormat("Room join failed with error code {0} and error message {1}", codeAndMsg[0], codeAndMsg[1]);
	}

	public void CreatePlayerPositions(int numPlayers)
	{
		PlayerPositionList = new string[numPlayers];
	}

	[PunRPC]
	public void SetPlayerPosition(string playerNickReal, string playerNick, string gameVersion = "0.0.0", Enums.Platform platform = Enums.Platform.PC, string pdxUserId = "")
	{
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("[SetPlayerPosition] playerNickReal -> " + playerNickReal + " // playerNick -> " + playerNick + " // gameVersion -> " + gameVersion + " // platform -> " + platform.ToString() + " // pdxUserId -> " + pdxUserId);
		}
		bool flag = false;
		for (int i = 0; i < PlayerPositionList.Length; i++)
		{
			if (PlayerPositionList[i] == playerNick)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			for (int j = 0; j < PlayerPositionList.Length; j++)
			{
				if (PlayerPositionList[j] == "" || PlayerPositionList[j] == null)
				{
					PlayerPositionList[j] = playerNick;
					break;
				}
			}
		}
		if (!PlayerNickRealDict.ContainsKey(playerNick))
		{
			PlayerNickRealDict.Add(playerNick, playerNickReal);
		}
		if (!PlayerVersionDict.ContainsKey(playerNick))
		{
			PlayerVersionDict.Add(playerNick, gameVersion);
		}
		if (!PlayerPlatformDict.ContainsKey(playerNick))
		{
			PlayerPlatformDict.Add(playerNick, platform.ToString());
		}
		if (!PlayerPDXUserDict.ContainsKey(playerNick))
		{
			PlayerPDXUserDict.Add(playerNick, pdxUserId);
		}
		string[] array = new string[PlayerNickRealDict.Count];
		PlayerNickRealDict.Keys.CopyTo(array, 0);
		string[] array2 = new string[PlayerNickRealDict.Count];
		PlayerNickRealDict.Values.CopyTo(array2, 0);
		string text = JsonHelper.ToJson(array);
		string text2 = JsonHelper.ToJson(array2);
		string[] array3 = new string[PlayerVersionDict.Count];
		PlayerVersionDict.Values.CopyTo(array3, 0);
		string text3 = JsonHelper.ToJson(array3);
		string[] array4 = new string[PlayerPlatformDict.Count];
		PlayerPlatformDict.Values.CopyTo(array4, 0);
		string text4 = JsonHelper.ToJson(array4);
		string[] array5 = new string[PlayerPDXUserDict.Count];
		PlayerPDXUserDict.Values.CopyTo(array5, 0);
		string text5 = JsonHelper.ToJson(array5);
		photonView.RPC("NET_SharePlayerNickReal", RpcTarget.Others, text, text2, text3, text4, text5);
		string text6 = JsonHelper.ToJson(PlayerPositionList);
		photonView.RPC("NET_SharePlayerPositionList", RpcTarget.Others, text6);
		if (playerNick != GetPlayerNick())
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<color=");
			stringBuilder.Append(GetColorFromNick(playerNick));
			stringBuilder.Append(">");
			stringBuilder.Append(GetPlayerNickReal(playerNick));
			stringBuilder.Append("</color>");
			ChatSend(Texts.Instance.GetText("chatPlayerJoined").Replace("%p", stringBuilder.ToString()), showAlertIfClosed: false, "");
		}
		SetLobbyPlayersData();
	}

	public bool IsPlayerPositionZero(string playerNick)
	{
		if (PlayerPositionList != null && PlayerPositionList[0] != null && PlayerPositionList[0] == playerNick)
		{
			return true;
		}
		return false;
	}

	public void RemovePlayerPosition(string playerNick)
	{
		Debug.Log("RemovePlayerPosition " + playerNick);
		if (PlayerPositionList != null)
		{
			for (int i = 0; i < PlayerPositionList.Length; i++)
			{
				if (PlayerPositionList[i] != null && PlayerPositionList[i] == playerNick)
				{
					PlayerPositionList[i] = "";
					break;
				}
			}
			foreach (KeyValuePair<string, string> item in PlayerNickRealDict)
			{
				if (item.Key == playerNick)
				{
					PlayerNickRealDict.Remove(item.Key);
					break;
				}
			}
			foreach (KeyValuePair<string, string> item2 in PlayerVersionDict)
			{
				if (item2.Key == playerNick)
				{
					PlayerVersionDict.Remove(item2.Key);
					break;
				}
			}
			string text = JsonHelper.ToJson(PlayerPositionList);
			photonView.RPC("NET_SharePlayerPositionList", RpcTarget.Others, text);
		}
		if (LobbyManager.Instance != null)
		{
			SetLobbyPlayersData();
		}
	}

	[PunRPC]
	public void NET_SharePlayerPositionList(string _PlayerPositionList)
	{
		PlayerPositionList = JsonHelper.FromJson<string>(_PlayerPositionList);
		SetLobbyPlayersData();
	}

	[PunRPC]
	public void NET_SharePlayerNickReal(string _keys, string _values, string _versions, string _platforms, string _pdxs)
	{
		string[] array = JsonHelper.FromJson<string>(_keys);
		string[] array2 = JsonHelper.FromJson<string>(_values);
		string[] array3 = JsonHelper.FromJson<string>(_versions);
		string[] array4 = JsonHelper.FromJson<string>(_platforms);
		string[] array5 = JsonHelper.FromJson<string>(_pdxs);
		PlayerNickRealDict = new Dictionary<string, string>();
		PlayerVersionDict = new Dictionary<string, string>();
		PlayerPlatformDict = new Dictionary<string, string>();
		PlayerPDXUserDict = new Dictionary<string, string>();
		for (int i = 0; i < array.Length; i++)
		{
			PlayerNickRealDict.Add(array[i], array2[i]);
			PlayerVersionDict.Add(array[i], array3[i]);
			PlayerPlatformDict.Add(array[i], array4[i]);
			PlayerPDXUserDict.Add(array[i], array5[i]);
		}
	}

	public void KickPlayer(int position, bool sendKickMsg = true)
	{
		int num = 0;
		Player[] playerList = PhotonNetwork.PlayerList;
		foreach (Player player in playerList)
		{
			if (num == position)
			{
				string text = player.NickName.ToLower().Split('_')[0];
				if (!(text == "dreamsitegames") && !(text == "rhin"))
				{
					if (sendKickMsg)
					{
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.Append("<color=");
						stringBuilder.Append(GetColorFromNick(player.NickName));
						stringBuilder.Append(">");
						stringBuilder.Append(GetPlayerNickReal(player.NickName));
						stringBuilder.Append("</color>");
						ChatSend(Texts.Instance.GetText("chatPlayerKicked").Replace("%p", stringBuilder.ToString()), showAlertIfClosed: false, "");
						photonView.RPC("BeenKicked", RpcTarget.Others, player.NickName);
					}
					RemovePlayerPosition(player.NickName);
				}
				break;
			}
			num++;
		}
	}

	[PunRPC]
	private void BeenKicked(string nick)
	{
		if (GetPlayerNick() == nick)
		{
			if (PhotonNetwork.IsConnected && PhotonNetwork.CurrentRoom != null)
			{
				PhotonNetwork.LeaveRoom();
			}
			AlertManager.buttonClickDelegate = BeenKickedAction;
			AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("masterKickedYou"));
		}
	}

	private void BeenKickedAction()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(BeenKickedAction));
	}

	public void AssignHeroPlayerPositionOwner(int id, string nickName)
	{
		if (PlayerHeroPositionOwner == null)
		{
			PlayerHeroPositionOwner = new string[4];
		}
		PlayerHeroPositionOwner[id] = nickName;
	}

	public void SetLobbyPlayersData()
	{
		LobbyManager.Instance.SetLobbyPlayersData(PhotonNetwork.PlayerList);
		AlertManager.Instance.SetPlayers();
		ChatManager.Instance.UpdatePlayersButton();
	}

	public override void OnPlayerEnteredRoom(Player other)
	{
		Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);
		if (PhotonNetwork.IsMasterClient)
		{
			if (PlayerReady.ContainsKey(other.NickName))
			{
				PlayerReady[other.NickName] = false;
			}
			else
			{
				PlayerReady.Add(other.NickName, value: false);
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD(other.NickName + " added to PlayerReady array");
			}
		}
	}

	public override void OnPlayerLeftRoom(Player other)
	{
		Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName);
		string sceneName = SceneStatic.GetSceneName();
		if (sceneName == "FinishRun" || (sceneName == "IntroNewGame" && AtOManager.Instance.GetTownTier() == 3))
		{
			return;
		}
		if (IsPlayerPositionZero(other.NickName))
		{
			Debug.Log("leaveRoom");
			PhotonNetwork.LeaveRoom();
			AtOManager.Instance.ClearGame();
			if (!LobbyManager.Instance)
			{
				networkDisconnectAlert = 1;
				if (SaveManager.PrefsHasKey("coopRoomId"))
				{
					SaveManager.PrefsRemoveKey("coopRoomId");
				}
				SceneStatic.LoadByName("Lobby");
			}
			return;
		}
		if (PhotonNetwork.IsMasterClient && (bool)LobbyManager.Instance)
		{
			RemovePlayerPosition(other.NickName);
		}
		if (!(LobbyManager.Instance == null) || (!(AtOManager.Instance.GetGameId() != "") && !(sceneName == "HeroSelection")))
		{
			return;
		}
		bool flag = GameManager.Instance.IsLoadingGame();
		AtOManager.Instance.ClearGame();
		RemovePlayerPosition(other.NickName);
		networkDisconnectAlert = 2;
		if (PhotonNetwork.IsMasterClient)
		{
			if ((sceneName == "HeroSelection" || sceneName == "ChallengeSelection") && !flag)
			{
				SceneStatic.LoadByName("Lobby");
			}
			else
			{
				AtOManager.Instance.LoadGame(AtOManager.Instance.GetSaveSlot());
			}
		}
		else
		{
			SceneStatic.LoadByName("Lobby");
		}
	}

	public override void OnJoinedLobby()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("OnJoinedLobby");
		}
		if (connTimeoutCo != null)
		{
			StopCoroutine(connTimeoutCo);
		}
		LobbyManager.Instance.JustConnectedToPhoton();
	}

	public override void OnRoomListUpdate(List<RoomInfo> _roomList)
	{
		foreach (RoomInfo _room in _roomList)
		{
			LobbyManager.Instance.RoomReceived(_room);
		}
		LobbyManager.Instance.RemoveOldRooms();
	}

	public override void OnConnectedToMaster()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("OnConnectedToMaster() was called by PUN");
		}
		ConnectedToMaster();
	}

	private void ConnectedToMaster()
	{
		LobbyManager.Instance.SetStatus(reset: true, Texts.Instance.GetText("statusConnected"));
		LobbyManager.Instance.RemoveAllRooms();
		JoinLobby();
		CancelAuthTicket(hAuthTicket);
	}

	public void ClearPreviousInfo()
	{
		PlayerList = null;
		PlayerPositionList = null;
		PlayerHeroPositionOwner = null;
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.LogFormat("OnDisconnected() was called by PUN with reason {0}", cause);
		onRoom = false;
		if (cause == DisconnectCause.CustomAuthenticationFailed)
		{
			regionSelected = "";
			LobbyManager.Instance.InitLobby();
		}
		else if ((bool)LobbyManager.Instance)
		{
			LobbyManager.Instance.InitLobby();
		}
		else if (!MainMenuManager.Instance)
		{
			SceneStatic.LoadByName("MainMenu");
		}
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
		}
	}

	public override void OnCustomAuthenticationFailed(string errorMessage)
	{
		Debug.Log("Authentication Failed: " + errorMessage);
		CancelAuthTicket(hAuthTicket);
		AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("steamErrorConnect"));
	}

	public override void OnCustomAuthenticationResponse(Dictionary<string, object> data)
	{
		Debug.Log("Authentication Response");
		foreach (KeyValuePair<string, object> datum in data)
		{
			Debug.Log(datum.Key + "=>" + datum.Value);
		}
	}

	public string GetSteamAuthTicket(out AuthTicket authTicket)
	{
		NetIdentity identity = SteamClient.SteamId;
		authTicket = SteamUser.GetAuthSessionTicket(identity);
		AuthTicket authTicket2 = authTicket;
		if (authTicket2 != null && authTicket2.Data != null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < authTicket.Data.Length; i++)
			{
				stringBuilder.AppendFormat("{0:x2}", authTicket.Data[i]);
			}
			return stringBuilder.ToString();
		}
		return "";
	}

	private void CancelAuthTicket(AuthTicket ticket)
	{
		ticket?.Cancel();
	}

	public string GetPlatformNick(int _playerSlot)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Instance.GetPlatformString(_playerSlot));
		stringBuilder.Append('_');
		stringBuilder.Append(Instance.GetPlayerNickReal(PlayerList[_playerSlot].NickName));
		return stringBuilder.ToString();
	}

	public void DoMute(int _playerSlot)
	{
		string platformNick = GetPlatformNick(_playerSlot);
		if (!PlayerMuteList.Contains(platformNick))
		{
			PlayerMuteList.Add(platformNick);
		}
		SaveManager.SaveMutedPlayers();
	}

	public void DoUnmute(int _playerSlot)
	{
		string platformNick = GetPlatformNick(_playerSlot);
		if (PlayerMuteList.Contains(platformNick))
		{
			PlayerMuteList.Remove(platformNick);
		}
		SaveManager.SaveMutedPlayers();
	}

	public bool IsPlayerMutedByNick(string _nick)
	{
		int playerListPosition = GetPlayerListPosition(_nick);
		string platformNick = GetPlatformNick(playerListPosition);
		return PlayerMuteList.Contains(platformNick);
	}

	public bool IsPlayerMutedBySlot(int _playerSlot)
	{
		string platformNick = GetPlatformNick(_playerSlot);
		return PlayerMuteList.Contains(platformNick);
	}
}
