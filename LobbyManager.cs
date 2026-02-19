using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Paradox;
using Photon.Pun;
using Photon.Realtime;
using ProfanityFilter;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
	public TMP_Dropdown dropRegion;

	public TMP_Text statusTM;

	public Transform buttonLaunch;

	public Transform buttonSteam;

	public Transform regions;

	public Transform regionsDisconnect;

	public Toggle UICrossPlay;

	public Transform lockCrossBackground;

	public Transform lockCrossPlay;

	public GameObject RoomPrefab;

	public Transform CreateRoomT;

	public Transform JoinRoomT;

	public Transform RoomT;

	public Transform GridTransform;

	public TMP_InputField UICreateName;

	public TMP_Dropdown UICreatePlayers;

	public TMP_InputField UICreatePwd;

	public Toggle UITogglePwd;

	public Toggle UIToggleLfm;

	public TMP_Text roomTitle;

	public TMP_Text roomWaiting;

	public TMP_Text[] roomSlots;

	public Image[] roomSlotsImage;

	public Transform[] roomSlotsKick;

	public Image[] roomSlotsIcon;

	private string roomPassword = "";

	private string roomName = "";

	private bool automaticJoin;

	private List<RoomList> _roomListButtons = new List<RoomList>();

	public Transform[] buttonsController;

	public int controllerHorizontalIndex = -1;

	private Vector2 warpPosition = Vector2.zero;

	private List<Transform> _controllerList = new List<Transform>();

	public static LobbyManager Instance { get; private set; }

	private List<RoomList> RoomListButtons => _roomListButtons;

	private void Awake()
	{
		if (GameManager.Instance == null)
		{
			SceneStatic.LoadByName("Lobby");
			return;
		}
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			UnityEngine.Object.Destroy(this);
		}
		GameManager.Instance.SetCamera();
		NetworkManager.Instance.StartStopQueue(state: true);
		GameManager.Instance.SceneLoaded();
		automaticJoin = true;
		ShowJoin();
	}

	private void Start()
	{
		if (PhotonNetwork.IsConnected && NetworkManager.Instance.GetRoomName() != "")
		{
			ShowRoom();
			regions.gameObject.SetActive(value: false);
			ShowConnectedStatus();
			if (PhotonNetwork.IsMasterClient)
			{
				NetworkManager.Instance.OpenRoomDoors();
			}
			if (NetworkManager.Instance.networkDisconnectAlert == 1)
			{
				AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("abortMultiplayerMaster"));
				AlertManager.buttonClickDelegate = LobbyAlertFinishAction;
			}
			else if (NetworkManager.Instance.networkDisconnectAlert == 2)
			{
				AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("abortMultiplayerPlayer"));
				AlertManager.buttonClickDelegate = LobbyAlertFinishAction;
			}
		}
		else
		{
			InitLobby();
		}
		NetworkManager.Instance.networkDisconnectAlert = 0;
		AudioManager.Instance.DoBSO("Game");
		AudioManager.Instance.StopAmbience();
	}

	private void LobbyAlertFinishAction()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(LobbyAlertFinishAction));
	}

	public void InitLobby()
	{
		GameManager.Instance.GameMode = Enums.GameMode.Multiplayer;
		if (RoomT != null && RoomT.gameObject.activeSelf)
		{
			RoomT.gameObject.SetActive(value: false);
		}
		if (CreateRoomT != null && CreateRoomT.gameObject.activeSelf)
		{
			CreateRoomT.gameObject.SetActive(value: false);
		}
		if (JoinRoomT != null && JoinRoomT.gameObject.activeSelf)
		{
			JoinRoomT.gameObject.SetActive(value: false);
		}
		if (regionsDisconnect != null && regionsDisconnect.gameObject.activeSelf)
		{
			regionsDisconnect.gameObject.SetActive(value: false);
		}
		SetStatus(reset: true);
		if (NetworkManager.Instance.regionSelected != "")
		{
			SetRegion(NetworkManager.Instance.regionSelected);
		}
		else
		{
			regions.gameObject.SetActive(value: true);
			if (!Startup.isLoggedIn)
			{
				UICrossPlay.isOn = false;
				UICrossPlay.interactable = false;
				lockCrossPlay.gameObject.SetActive(value: true);
				lockCrossBackground.gameObject.SetActive(value: false);
			}
			else
			{
				UICrossPlay.interactable = true;
				UICrossPlay.isOn = GameManager.Instance.ConfigCrossPlayEnabled;
				lockCrossPlay.gameObject.SetActive(value: false);
				lockCrossBackground.gameObject.SetActive(value: true);
			}
			if (!SaveManager.PrefsHasKey("networkRegion"))
			{
				automaticJoin = false;
				return;
			}
			int value = SaveManager.LoadPrefsInt("networkRegion");
			dropRegion.value = value;
			if (automaticJoin && !PhotonNetwork.IsConnected)
			{
				StartCoroutine(SelectRegionCo());
			}
		}
		automaticJoin = false;
	}

	public void DisconnectRegion(bool _fromButton = false)
	{
		NetworkManager.Instance.Disconnect();
		if (_fromButton && SaveManager.PrefsHasKey("networkRegion"))
		{
			SaveManager.PrefsRemoveKey("networkRegion");
		}
	}

	private IEnumerator SelectRegionCo()
	{
		yield return Globals.Instance.WaitForSeconds(0.1f);
		SelectRegion();
	}

	public void SelectRegion()
	{
		string text = "";
		text = dropRegion.value switch
		{
			0 => "asia", 
			1 => "au", 
			2 => "cae", 
			3 => "eu", 
			4 => "in", 
			5 => "jp", 
			6 => "ru", 
			7 => "rue", 
			8 => "za", 
			9 => "sa", 
			10 => "kr", 
			11 => "us", 
			12 => "usw", 
			_ => "", 
		};
		if (text != "")
		{
			SetRegion(text);
		}
	}

	public void SetRegion(string regionName)
	{
		if (regions != null && regions.gameObject.activeSelf)
		{
			regions.gameObject.SetActive(value: false);
		}
		SaveManager.SaveIntoPrefsInt("networkRegion", dropRegion.value);
		NetworkManager.Instance.Connect(regionName);
		SetStatus(reset: true, Texts.Instance.GetText("statusConnecting"));
	}

	private void GetPlayerName()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(GetPlayerName));
		AlertManager.Instance.GetInputValue();
	}

	private void ConnectToPhoton()
	{
		Debug.Log("ConnectToPhoton");
		NetworkManager.Instance.Connect();
	}

	public void JustConnectedToPhoton()
	{
		if (NetworkManager.Instance.WantToJoinRoomName != "")
		{
			NetworkManager.Instance.JoinRoomByPreloadedSteam();
		}
		else if (AtOManager.Instance.GetSaveSlot() > -1)
		{
			ShowCreate();
		}
		else
		{
			ShowJoin();
		}
		ShowConnectedStatus();
		if (regionsDisconnect != null)
		{
			regionsDisconnect.gameObject.SetActive(value: true);
		}
	}

	private void ShowReconnectAlert()
	{
		AlertManager.buttonClickDelegate = AutomaticJoinId;
		AlertManager.Instance.AlertConfirmDouble(Texts.Instance.GetText("joinOldGame"), Texts.Instance.GetText("accept").ToUpper(), Texts.Instance.GetText("cancel").ToUpper());
	}

	public void AutomaticJoinId()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(AutomaticJoinId));
		if (AlertManager.Instance.GetConfirmAnswer())
		{
			string text = SaveManager.LoadPrefsString("coopRoomId");
			NetworkManager.Instance.JoinRoom(text);
		}
		SaveManager.PrefsRemoveKey("coopRoomId");
	}

	private void ShowConnectedStatus()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Texts.Instance.GetText("region"));
		stringBuilder.Append(" [");
		stringBuilder.Append(NetworkManager.Instance.regionSelected.ToUpper());
		stringBuilder.Append("]<br>");
		stringBuilder.Append(Texts.Instance.GetText("crossplayToggle"));
		stringBuilder.Append(" [");
		if (Startup.isLoggedIn && GameManager.Instance.ConfigCrossPlayEnabled)
		{
			stringBuilder.Append(Texts.Instance.GetText("enabled"));
		}
		else
		{
			stringBuilder.Append(Texts.Instance.GetText("disabled"));
		}
		stringBuilder.Append("]");
		SetStatus(reset: true, stringBuilder.ToString());
	}

	public void ShowCreate()
	{
		Functions.DebugLogGD("ShowCreate", "trace");
		if (CreateRoomT != null)
		{
			CreateRoomT.gameObject.SetActive(value: true);
			JoinRoomT.gameObject.SetActive(value: false);
			if (SaveManager.PrefsHasKey("LobbyRoomName"))
			{
				UICreateName.text = SaveManager.LoadPrefsString("LobbyRoomName");
			}
		}
	}

	public void ShowJoin()
	{
		Functions.DebugLogGD("ShowJoin", "trace");
		if (RoomT != null && RoomT.gameObject != null && PhotonNetwork.IsConnected)
		{
			RoomT.gameObject.SetActive(value: false);
			CreateRoomT.gameObject.SetActive(value: false);
			JoinRoomT.gameObject.SetActive(value: true);
		}
	}

	public void JoinRoomById()
	{
		AlertManager.buttonClickDelegate = GetRoomId;
		AlertManager.Instance.AlertInput(Texts.Instance.GetText("inputIDRoom"), Texts.Instance.GetText("accept").ToUpper());
	}

	public void GetRoomId()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(GetRoomId));
		if (AlertManager.Instance.GetInputValue() == null)
		{
			return;
		}
		string text = AlertManager.Instance.GetInputValue().ToUpper();
		if (!(text.Trim() != ""))
		{
			return;
		}
		for (int i = 0; i < RoomListButtons.Count; i++)
		{
			if (text == RoomListButtons[i].RoomName)
			{
				NetworkManager.Instance.JoinRoom(text);
				return;
			}
		}
		StartCoroutine(RoomByIdErrorAlert());
	}

	private IEnumerator RoomByIdErrorAlert()
	{
		yield return Globals.Instance.WaitForSeconds(0.1f);
		AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("roomDoesNotExist"));
	}

	public void JoinRoom(string _roomName, string _roomPassword)
	{
		Functions.DebugLogGD("joinRoom " + _roomName + " // " + _roomPassword, "trace");
		roomPassword = _roomPassword;
		roomName = _roomName;
		if (_roomPassword != "")
		{
			AlertManager.buttonClickDelegate = GetRoomPassword;
			AlertManager.Instance.AlertInput(Texts.Instance.GetText("inputPasswordRoom"), Texts.Instance.GetText("accept").ToUpper());
		}
		else
		{
			NetworkManager.Instance.JoinRoom(roomName);
			roomPassword = "";
			roomName = "";
		}
	}

	public void GetRoomPassword()
	{
		string unencrypted = AlertManager.Instance.GetInputValue().ToUpper();
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(GetRoomPassword));
		if (new SimplerAES().Encrypt(unencrypted) == roomPassword)
		{
			NetworkManager.Instance.JoinRoom(roomName);
			roomPassword = "";
			roomName = "";
		}
	}

	public void ShowRoom()
	{
		CreateRoomT.gameObject.SetActive(value: false);
		JoinRoomT.gameObject.SetActive(value: false);
		for (int i = 0; i < roomSlots.Length; i++)
		{
			roomSlots[i].gameObject.SetActive(value: false);
			roomSlotsImage[i].gameObject.SetActive(value: false);
			roomSlotsIcon[i].gameObject.SetActive(value: false);
		}
		RoomT.gameObject.SetActive(value: true);
		buttonLaunch.gameObject.SetActive(value: false);
		buttonSteam.gameObject.SetActive(value: false);
		StringBuilder stringBuilder = new StringBuilder();
		string text = NetworkManager.Instance.GetRoomPassword();
		stringBuilder.Append(NetworkManager.Instance.GetRoomDescription());
		stringBuilder.Append("<br><size=30>");
		stringBuilder.Append("<color=#BBB>");
		stringBuilder.Append(Texts.Instance.GetText("roomId"));
		stringBuilder.Append(":</color> ");
		stringBuilder.Append(NetworkManager.Instance.GetRoomName());
		if (text != "")
		{
			stringBuilder.Append("<br>");
			stringBuilder.Append("<color=#BBB>");
			stringBuilder.Append(Texts.Instance.GetText("password"));
			stringBuilder.Append(":</color> ");
			stringBuilder.Append(text);
		}
		stringBuilder.Append("</size>");
		roomTitle.text = stringBuilder.ToString();
	}

	public void InviteSteam()
	{
		if (!TomeManager.Instance.IsActive())
		{
			SteamManager.Instance.InviteSteam();
		}
	}

	public void GoToDiscord()
	{
		GameManager.Instance.Discord();
	}

	public void ExitRoom()
	{
		AlertManager.buttonClickDelegate = ExitRoomAction;
		AlertManager.Instance.AlertConfirmDouble(Texts.Instance.GetText("exitRoomConfirm"), Texts.Instance.GetText("accept").ToUpper(), Texts.Instance.GetText("cancel").ToUpper());
	}

	public void ExitRoomAction()
	{
		bool confirmAnswer = AlertManager.Instance.GetConfirmAnswer();
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(ExitRoomAction));
		if (confirmAnswer)
		{
			ExitRoomInstant();
		}
	}

	public void ExitRoomInstant()
	{
		NetworkManager.Instance.ExitRoom();
		AtOManager.Instance.CleanSaveSlot();
		ShowJoin();
	}

	public void SetLobbyPlayersData(Player[] _PlayerList)
	{
		NetworkManager.Instance.PlayerList = _PlayerList;
		DrawLobbyNames();
	}

	public void KickPlayer(int position)
	{
		NetworkManager.Instance.KickPlayer(position);
	}

	private void DrawLobbyNames()
	{
		int num = 0;
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		Player[] playerList = NetworkManager.Instance.PlayerList;
		foreach (Player player in playerList)
		{
			stringBuilder.Clear();
			stringBuilder.Append("<color=#FFF>");
			if (player.NickName == NetworkManager.Instance.GetPlayerNick())
			{
				stringBuilder.Append("<u>");
				stringBuilder.Append(NetworkManager.Instance.GetPlayerNickReal(player.NickName));
				stringBuilder.Append("</u>");
			}
			else
			{
				stringBuilder.Append(NetworkManager.Instance.GetPlayerNickReal(player.NickName));
			}
			stringBuilder.Append("</color>");
			stringBuilder.Append("<br><size=24>");
			string playerVersion = NetworkManager.Instance.GetPlayerVersion(player.NickName);
			if (playerVersion == GameManager.Instance.gameVersion)
			{
				stringBuilder.Append("<color=#A0A0A0>");
			}
			else if (Functions.CompatibleVersion(playerVersion, GameManager.Instance.gameVersion))
			{
				stringBuilder.Append("<color=#FC0>");
			}
			else
			{
				stringBuilder.Append("<color=#D73232>");
			}
			stringBuilder.Append(NetworkManager.Instance.GetPlayerVersion(player.NickName));
			stringBuilder.Append("</size></color>");
			if (player.IsMasterClient)
			{
				stringBuilder.Append("  <color=#DE70BF><size=26>");
				stringBuilder.Append(Texts.Instance.GetText("master"));
				stringBuilder.Append("</size></color>");
			}
			if (roomSlots[num] != null)
			{
				roomSlots[num].gameObject.SetActive(value: true);
				roomSlots[num].text = stringBuilder.ToString();
			}
			if (roomSlotsImage[num] != null)
			{
				roomSlotsImage[num].gameObject.SetActive(value: true);
				roomSlotsImage[num].color = Functions.HexToColor(NetworkManager.Instance.GetColorFromNick(player.NickName));
			}
			if (roomSlotsIcon[num] != null)
			{
				roomSlotsIcon[num].gameObject.SetActive(value: true);
				roomSlotsIcon[num].sprite = NetworkManager.Instance.GetSlotPlatformImage(num);
			}
			if (num > 0)
			{
				if (PhotonNetwork.IsMasterClient)
				{
					roomSlotsKick[num].gameObject.SetActive(value: true);
				}
				else
				{
					roomSlotsKick[num].gameObject.SetActive(value: false);
				}
			}
			if (Globals.Instance.ConfigPlatform.ToString() != NetworkManager.Instance.GetPlatformString(num))
			{
				flag = false;
			}
			num++;
		}
		int num2 = num;
		bool flag2 = true;
		if (num < PhotonNetwork.CurrentRoom.MaxPlayers)
		{
			flag2 = false;
			for (int j = num; j < PhotonNetwork.CurrentRoom.MaxPlayers; j++)
			{
				stringBuilder.Clear();
				stringBuilder.Append("<color=#999>");
				stringBuilder.Append(Texts.Instance.GetText("openSlot"));
				stringBuilder.Append("</color>");
				roomSlots[j].gameObject.SetActive(value: true);
				roomSlots[j].text = stringBuilder.ToString();
				roomSlotsImage[j].gameObject.SetActive(value: false);
				roomSlotsIcon[j].gameObject.SetActive(value: false);
				roomSlotsKick[j].gameObject.SetActive(value: false);
				num++;
			}
		}
		for (int k = num; k < roomSlots.Length; k++)
		{
			roomSlots[k].gameObject.SetActive(value: false);
			roomSlotsImage[k].gameObject.SetActive(value: false);
			roomSlotsIcon[k].gameObject.SetActive(value: false);
		}
		if (!flag2)
		{
			roomWaiting.text = Texts.Instance.GetText("waitingPlayers");
		}
		else
		{
			roomWaiting.text = Texts.Instance.GetText("roomFull");
		}
		if (PhotonNetwork.IsMasterClient)
		{
			if (num2 > 1)
			{
				buttonLaunch.gameObject.SetActive(value: true);
			}
			else
			{
				buttonLaunch.gameObject.SetActive(value: false);
			}
			if (SteamManager.Instance.steamConnected)
			{
				buttonSteam.gameObject.SetActive(value: true);
			}
			else
			{
				buttonSteam.gameObject.SetActive(value: false);
			}
		}
		else if (!flag && (!Startup.isLoggedIn || !GameManager.Instance.ConfigCrossPlayEnabled))
		{
			AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("crossPlayDisabled"));
			ExitRoomInstant();
		}
	}

	public void SetCrossPlayEnabled(bool _enabled)
	{
		SettingsManager.Instance.SetCrossPlayEnabled(_enabled);
	}

	private bool IsRoomAvailableForMultiPlatform(RoomInfo room)
	{
		object obj = room.CustomProperties["crossplay"];
		if (obj != null && obj.ToString() == "1")
		{
			return GameManager.Instance.ConfigCrossPlayEnabled;
		}
		string text = "PC";
		if (room.CustomProperties["platform"] != null)
		{
			text = room.CustomProperties["platform"].ToString();
		}
		if (Globals.Instance.ConfigPlatform.ToString() == text)
		{
			return true;
		}
		return false;
	}

	public void RoomReceived(RoomInfo room)
	{
		if (!IsRoomAvailableForMultiPlatform(room))
		{
			return;
		}
		int num = RoomListButtons.FindIndex((RoomList x) => x.RoomName == room.Name);
		if (num == -1 && room.IsVisible && room.PlayerCount < room.MaxPlayers)
		{
			GameObject obj = UnityEngine.Object.Instantiate(RoomPrefab);
			obj.transform.SetParent(GridTransform, worldPositionStays: false);
			RoomList component = obj.GetComponent<RoomList>();
			RoomListButtons.Add(component);
			num = RoomListButtons.Count - 1;
		}
		if (num == -1)
		{
			return;
		}
		RoomList roomList = RoomListButtons[num];
		roomList.SetRoomName(room.Name);
		roomList.SetRoomPlayers(room.PlayerCount, room.MaxPlayers);
		string text = "";
		if (SaveManager.PrefsHasKey("coopRoomId"))
		{
			text = SaveManager.LoadPrefsString("coopRoomId");
		}
		if (room.IsOpen && room.PlayerCount > 0 && room.PlayerCount < room.MaxPlayers)
		{
			roomList.SetRoomCreator(room.CustomProperties["creator"].ToString());
			roomList.SetRoomPassword(room.CustomProperties["pwd"].ToString());
			roomList.SetRoomDescription(room.CustomProperties["description"].ToString());
			if (room.CustomProperties.ContainsKey("lfm"))
			{
				roomList.SetLfm(room.CustomProperties["lfm"].ToString());
			}
			else
			{
				roomList.SetLfm("");
			}
			if (room.CustomProperties.ContainsKey("version"))
			{
				roomList.SetRoomVersion(room.CustomProperties["version"].ToString(), room.CustomProperties["platform"]?.ToString());
			}
			if (room.CustomProperties.ContainsKey("crossplay"))
			{
				roomList.SetRoomCrossplayPlatform(room.CustomProperties["crossplay"].ToString(), room.CustomProperties["platform"].ToString());
			}
			if (text != "" && text == room.Name)
			{
				ShowReconnectAlert();
			}
		}
		else if (roomList != null)
		{
			GameObject obj2 = roomList.gameObject;
			RoomListButtons.Remove(roomList);
			UnityEngine.Object.Destroy(obj2);
		}
	}

	public void RemoveAllRooms()
	{
		if (GridTransform != null)
		{
			foreach (Transform item in GridTransform)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}
		RoomListButtons.Clear();
	}

	[PunRPC]
	public void RemoveOldRooms()
	{
	}

	public void GoBack()
	{
		if (GameManager.Instance.GameStatus == Enums.GameStatus.LoadGame)
		{
			AtOManager.Instance.CleanSaveSlot();
		}
		ShowJoin();
	}

	public void CreateMultiplayerGame()
	{
		GameManager.Instance.mainMenuGoToMultiplayer = true;
		SceneStatic.LoadByName("MainMenu");
	}

	public void CreateRoom()
	{
		string text = UICreateName.text.ToString().Trim();
		string roomPlayers = UICreatePlayers.options[UICreatePlayers.value].text.ToString();
		string text2 = UICreatePwd.text.ToString().ToUpper().Trim();
		string text3 = "0";
		text2 = ((!UITogglePwd.isOn || !(text2 != "")) ? "" : new SimplerAES().Encrypt(text2));
		text3 = ((!UIToggleLfm.isOn) ? "" : "1");
		if (text != "")
		{
			text = new global::ProfanityFilter.ProfanityFilter().CensorString(text);
			SaveManager.SaveIntoPrefsString("LobbyRoomName", text);
			NetworkManager.Instance.CreateRoom(text, roomPlayers, text2, text3);
		}
	}

	public void SetStatus(bool reset = false, string text = "")
	{
		if (reset)
		{
			statusTM.text = text;
			return;
		}
		TMP_Text tMP_Text = statusTM;
		tMP_Text.text = tMP_Text.text + text + "\n";
	}

	public void LaunchGame()
	{
		LaunchGameCheckVersion();
	}

	private void LaunchGameCheckVersion()
	{
		if (NetworkManager.Instance.VersionMatch())
		{
			NetworkManager.Instance.CloseRoomDoors();
			NetworkManager.Instance.NormalizePlayerPositionList();
			NetworkManager.Instance.LoadScene("HeroSelection");
		}
		else
		{
			AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("versionMismatch"));
		}
	}

	public void ShowPlayerStatus()
	{
		SetStatus(reset: false, "Player status");
		List<string> list = new List<string>(NetworkManager.Instance.PlayerStatusReady["lobbymanager"].Keys);
		for (int i = 0; i < list.Count; i++)
		{
			SetStatus(reset: false, list[i] + ": " + NetworkManager.Instance.PlayerStatusReady["lobbymanager"][list[i]]);
		}
	}

	public void SetReady()
	{
		NetworkManager.Instance.SetStatusReady("lobbymanager");
	}

	public void AllUnready()
	{
		if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
		{
			NetworkManager.Instance.ClearAllPlayerReadyStatus("lobbymanager");
		}
		ShowPlayerStatus();
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false)
	{
		_controllerList.Clear();
		if (Functions.TransformIsVisible(regionsDisconnect))
		{
			_controllerList.Add(regionsDisconnect);
		}
		if (Functions.TransformIsVisible(regions))
		{
			_controllerList.Add(regions.GetChild(1));
			_controllerList.Add(regions.GetChild(2));
			_controllerList.Add(regions.GetChild(3));
		}
		for (int i = 0; i < buttonsController.Length; i++)
		{
			if (Functions.TransformIsVisible(buttonsController[i]))
			{
				_controllerList.Add(buttonsController[i]);
			}
		}
		if (Functions.TransformIsVisible(GridTransform))
		{
			foreach (Transform item in GridTransform)
			{
				_controllerList.Add(item.GetChild(5));
			}
		}
		if (_controllerList != null && _controllerList.Count != 0)
		{
			controllerHorizontalIndex = Functions.GetListClosestIndexToMousePosition(_controllerList);
			controllerHorizontalIndex = Functions.GetClosestIndexBasedOnDirection(_controllerList, controllerHorizontalIndex, goingUp, goingRight, goingDown, goingLeft);
			if (_controllerList[controllerHorizontalIndex] != null)
			{
				warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerList[controllerHorizontalIndex].position);
				Mouse.current.WarpCursorPosition(warpPosition);
			}
		}
	}
}
