using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Paradox;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zone;

public class GameManager : MonoBehaviour
{
	[Serializable]
	public class DLCData
	{
		public List<ParadoxAppId> disabledDLC;
	}

	[Serializable]
	public class ParadoxAppId
	{
		public string paradoxAppId;
	}

	public Texture2D cursorTexture;

	public Texture2D cursorTextureHover;

	public Transform globalBlack;

	public GameObject CardPrefab;

	public GameObject trailGoldPrefab;

	public GameObject trailCardPrefab;

	public GameObject trailDustPrefab;

	public GameObject popTutorialPrefab;

	public GameObject keyboardPrefab;

	public GameObject alertPrefab;

	private GameObject popTutorialGO;

	public Transform TempContainer;

	public Transform TutorialContainer;

	public AudioSource Audio;

	[SerializeField]
	private Dictionary<string, Hero> gameHeroes;

	public Sprite[] cardSprites;

	public Sprite[] cardBackSprites;

	public Sprite[] cardEnergySprites;

	public Sprite[] cardBorderSprites;

	private Dictionary<AudioClip, float> AudioPlayed = new Dictionary<AudioClip, float>();

	public float TimeSyncro;

	public Enums.GameMode GameMode;

	public Enums.GameStatus GameStatus;

	public Enums.GameType GameType;

	public CameraManager cameraManager;

	public Camera cameraMain;

	private Coroutine coroutineMask;

	public Transform MaskWindow;

	public RawImage maskWindowImageBg;

	public Transform maskWindowLoadingText;

	public Transform ChatWindow;

	public TMP_Text gameVersionT;

	public string gameVersion = "0.0.0";

	public Enums.ConfigSpeed configGameSpeed;

	private bool configAutoEnd;

	private bool configShowEffects = true;

	private bool configExtendedDescriptions = true;

	private bool configACBackgrounds = true;

	private bool configRestartCombatOption = true;

	private bool configScreenShakeOption = true;

	private bool configBackgroundMute = true;

	private bool configVsync;

	private bool configKeyboardShortcuts;

	private bool configUseLegacySounds;

	private bool configUseLegacySoundsSheepOwl;

	private bool configCrossPlayEnabled = true;

	[SerializeField]
	private bool developerMode;

	[SerializeField]
	private bool cheatMode;

	[SerializeField]
	private bool winMatchOnStart;

	[SerializeField]
	private bool skipTutorial;

	[SerializeField]
	private MapType startFromMap;

	[SerializeField]
	private bool useImmortal;

	[SerializeField]
	private bool useManyResources;

	[SerializeField]
	private bool unlockAllHeroes;

	[SerializeField]
	private bool unlockMadness;

	[SerializeField]
	private bool disableSave;

	[SerializeField]
	private bool disableSteamAuthorizationForPhoton;

	[SerializeField]
	private bool useTestSteamID;

	private bool isDemo;

	private bool showedDemoMsg;

	private StringBuilder SBversion;

	public bool mainMenuGoToMultiplayer;

	public bool gameIsOnFocus = true;

	private Coroutine steamCo;

	public List<string> communityRewards = new List<string>();

	public string communityRewardsExpire = "";

	public DateTime storedDateTime;

	private Coroutine clockCoroutine;

	public double clockSeconds;

	public bool PrefsLoaded;

	private string profileFolder = "";

	private int profileId;

	public ConsoleToGUI consoleGUI;

	private string pDXCliToken = "";

	private string pDXCliUserId = "";

	private bool pDXCliContinue;

	private bool firstLoad = true;

	private List<string> disabledDLCs;

	public bool DisableCardCast;

	public static GameManager Instance { get; private set; }

	public bool CheatMode
	{
		get
		{
			return cheatMode;
		}
		set
		{
			cheatMode = value;
		}
	}

	public bool WinMatchOnStart
	{
		get
		{
			return winMatchOnStart;
		}
		set
		{
			winMatchOnStart = value;
		}
	}

	public bool SkipTutorial
	{
		get
		{
			return skipTutorial;
		}
		set
		{
			skipTutorial = value;
		}
	}

	public MapType StartFromMap
	{
		get
		{
			return startFromMap;
		}
		set
		{
			startFromMap = value;
		}
	}

	public bool IsSaveDisabled
	{
		get
		{
			return disableSave;
		}
		set
		{
			disableSave = value;
		}
	}

	public bool UseTestSteamID
	{
		get
		{
			return useTestSteamID;
		}
		set
		{
			useTestSteamID = value;
		}
	}

	public bool UseImmortal
	{
		get
		{
			return useImmortal;
		}
		set
		{
			useImmortal = value;
		}
	}

	public bool UseManyResources
	{
		get
		{
			return useManyResources;
		}
		set
		{
			useManyResources = value;
		}
	}

	public bool UnlockAllHeroes
	{
		get
		{
			return unlockAllHeroes;
		}
		set
		{
			unlockAllHeroes = value;
		}
	}

	public bool UnlockMadness
	{
		get
		{
			return unlockMadness;
		}
		set
		{
			unlockMadness = value;
		}
	}

	public bool DisableSteamAuthorizationForPhoton
	{
		get
		{
			return disableSteamAuthorizationForPhoton;
		}
		set
		{
			disableSteamAuthorizationForPhoton = value;
		}
	}

	public Dictionary<string, Hero> GameHeroes
	{
		get
		{
			return gameHeroes;
		}
		set
		{
			gameHeroes = value;
		}
	}

	public bool ConfigAutoEnd
	{
		get
		{
			return configAutoEnd;
		}
		set
		{
			configAutoEnd = value;
		}
	}

	public bool ConfigShowEffects
	{
		get
		{
			return configShowEffects;
		}
		set
		{
			configShowEffects = value;
		}
	}

	public bool ConfigExtendedDescriptions
	{
		get
		{
			return configExtendedDescriptions;
		}
		set
		{
			configExtendedDescriptions = value;
		}
	}

	public bool ConfigACBackgrounds
	{
		get
		{
			return configACBackgrounds;
		}
		set
		{
			configACBackgrounds = value;
		}
	}

	public bool ConfigRestartCombatOption
	{
		get
		{
			return configRestartCombatOption;
		}
		set
		{
			configRestartCombatOption = value;
		}
	}

	public bool ConfigScreenShakeOption
	{
		get
		{
			return configScreenShakeOption;
		}
		set
		{
			configScreenShakeOption = value;
		}
	}

	public bool ConfigBackgroundMute
	{
		get
		{
			return configBackgroundMute;
		}
		set
		{
			configBackgroundMute = value;
		}
	}

	public bool ConfigVsync
	{
		get
		{
			return configVsync;
		}
		set
		{
			configVsync = value;
		}
	}

	public string ProfileFolder
	{
		get
		{
			return profileFolder;
		}
		set
		{
			profileFolder = value;
		}
	}

	public int ProfileId
	{
		get
		{
			return profileId;
		}
		set
		{
			profileId = value;
		}
	}

	public bool ConfigKeyboardShortcuts
	{
		get
		{
			return configKeyboardShortcuts;
		}
		set
		{
			configKeyboardShortcuts = value;
		}
	}

	public string PDXCliToken
	{
		get
		{
			return pDXCliToken;
		}
		set
		{
			pDXCliToken = value;
		}
	}

	public string PDXCliUserId
	{
		get
		{
			return pDXCliUserId;
		}
		set
		{
			pDXCliUserId = value;
		}
	}

	public bool PDXCliContinue
	{
		get
		{
			return pDXCliContinue;
		}
		set
		{
			pDXCliContinue = value;
		}
	}

	public bool FirstLoad
	{
		get
		{
			return firstLoad;
		}
		set
		{
			firstLoad = value;
		}
	}

	public List<string> DisabledDLCs
	{
		get
		{
			return disabledDLCs;
		}
		set
		{
			disabledDLCs = value;
		}
	}

	public bool ConfigUseLegacySounds
	{
		get
		{
			return configUseLegacySounds;
		}
		set
		{
			configUseLegacySounds = value;
		}
	}

	public bool ConfigUseLegacySoundsSheepOwl
	{
		get
		{
			return configUseLegacySoundsSheepOwl;
		}
		set
		{
			configUseLegacySoundsSheepOwl = value;
		}
	}

	public bool ConfigCrossPlayEnabled
	{
		get
		{
			return configCrossPlayEnabled;
		}
		set
		{
			configCrossPlayEnabled = value;
		}
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		gameIsOnFocus = hasFocus;
		AudioManager.Instance.StartStopBSO(gameIsOnFocus);
		AudioManager.Instance.StartStopAmbience(gameIsOnFocus);
	}

	private void OnApplicationQuit()
	{
		Startup.Shutdown();
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
		globalBlack.gameObject.SetActive(value: true);
		CultureInfo.DefaultThreadCurrentUICulture = (CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US", useUserOverride: false));
		disableSteamAuthorizationForPhoton = false;
	}

	private void Start()
	{
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		if (commandLineArgs != null)
		{
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				if (commandLineArgs[i] == "--pdx-launcher-session-token")
				{
					pDXCliToken = commandLineArgs[i + 1];
				}
				else if (commandLineArgs[i] == "--paradox-account-userid")
				{
					pDXCliUserId = commandLineArgs[i + 1];
				}
				else if (commandLineArgs[i] == "--continuelastsave")
				{
					pDXCliContinue = true;
				}
			}
		}
		string text = ((TextAsset)Resources.Load("runtime-version")).text;
		gameVersion = text.Trim();
		GameType = Enums.GameType.Adventure;
		configGameSpeed = Enums.ConfigSpeed.Slow;
		configAutoEnd = false;
		configShowEffects = true;
		configExtendedDescriptions = true;
		SettingsManager.Instance.LoadPrefs();
		PrefsLoaded = true;
		SetCamera();
		Resize();
		UnityEngine.Object.Instantiate(keyboardPrefab, Vector3.zero, Quaternion.identity);
		UnityEngine.Object.Instantiate(alertPrefab, Vector3.zero, Quaternion.identity);
		Texts.Instance.LoadTranslation();
		Globals.Instance.CreateGameContent();
		GenerateHeroes();
		steamCo = StartCoroutine(WaitForSteam());
	}

	private void SteamNotConnected()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(SteamNotConnected));
		Application.Quit();
	}

	private IEnumerator WaitForSteam()
	{
		yield return Globals.Instance.WaitForSeconds(0.2f);
		SteamManager.Instance.DoSteam();
		int exhaustSteam = 0;
		while ((!SteamManager.Instance.steamLoaded || (ulong)SteamManager.Instance.steamId == 0L) && exhaustSteam < 30)
		{
			yield return Globals.Instance.WaitForSeconds(0.1f);
			exhaustSteam++;
		}
		if (exhaustSteam >= 30)
		{
			AlertManager.buttonClickDelegate = SteamNotConnected;
			AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("steamErrorConnect"));
			yield break;
		}
		InitProfileId();
		TomeManager.Instance.InitTome();
		PerkTree.Instance.InitPerkTree();
		MadnessManager.Instance.InitMadness();
		SandboxManager.Instance.InitSandbox();
		PlayerUIManager.Instance.Hide();
		OptionsManager.Instance.Hide();
		GiveManager.Instance.ShowGive(state: false);
		SettingsManager.Instance.ShowSettings(_state: false);
		PlayerManager.Instance.PreBeginGame();
		LoadPlayerData();
		PlayerManager.Instance.UnlockInitialHeroes();
	}

	public void LoadPlayerData()
	{
		PlayerData playerData = SaveManager.LoadPlayerData();
		if (playerData == null)
		{
			playerData = SaveManager.LoadPlayerData(fromBackup: true);
		}
		if (playerData != null)
		{
			SaveManager.RestorePlayerData(playerData);
			if (PlayerManager.Instance.UnlockedCards.Count != 33 && PlayerManager.Instance.UnlockedNodes.Count != 0 && !AtOManager.Instance.IsFirstGame())
			{
				SaveManager.SavePlayerDataBackup();
			}
		}
		BeginGame();
	}

	private void BeginGame()
	{
		SBversion = new StringBuilder();
		SBversion.Append("AtO #");
		SBversion.Append(gameVersion);
		InvokeRepeating("SetGameVersionText", 0f, 2f);
		PlayerManager.Instance.BeginGame();
		SaveManager.SavePlayerData();
		StartTimer();
		if (NetworkManager.Instance.WantToJoinRoomName != "")
		{
			ChangeScene("Lobby");
		}
		else if (SceneStatic.CrossScene != null && SceneStatic.CrossScene != "")
		{
			ChangeScene(SceneStatic.CrossScene);
			SceneStatic.CrossScene = "";
		}
		else if (!MainMenuManager.Instance)
		{
			ChangeScene("Logos");
		}
	}

	private void InitProfileId()
	{
		if (!SaveManager.PrefsHasKey("profileId"))
		{
			UseProfileFile(0);
		}
		else
		{
			profileId = SaveManager.LoadPrefsInt("profileId");
			if (profileId < 0 || profileId > 4)
			{
				profileId = 0;
			}
		}
		SetProfileFolder();
	}

	public bool IsThisAProfile()
	{
		return profileId != 0;
	}

	public void UseProfileFile(int _profile)
	{
		SaveManager.SaveIntoPrefsInt("profileId", _profile);
		SaveManager.SavePrefs();
		profileId = _profile;
	}

	public void SetProfileFolder()
	{
		if (profileId == 0)
		{
			profileFolder = "";
		}
		else
		{
			profileFolder = "profile" + profileId + "/";
		}
		if (!SaveManager.ExistsProfileFolder(profileId))
		{
			UseProfileFile(0);
			SetProfileFolder();
		}
	}

	public void ReloadProfile()
	{
		SetProfileFolder();
		PlayerManager.Instance.InitPlayerData();
		PlayerManager.Instance.UnlockInitialHeroes();
		PlayerManager.Instance.PreBeginGame();
		PlayerManager.Instance.BeginGame();
		LoadPlayerData();
	}

	public void SaveGameDeveloper()
	{
		if ((IsMultiplayer() && !NetworkManager.Instance.IsMaster()) || (!TownManager.Instance && !MapManager.Instance))
		{
			return;
		}
		if (IsWeeklyChallenge())
		{
			if (!IsMultiplayer())
			{
				SaveManager.SaveGame(24);
				SaveManager.SaveGame(25);
				SaveManager.SaveGame(26);
			}
			else
			{
				SaveManager.SaveGame(30);
				SaveManager.SaveGame(31);
				SaveManager.SaveGame(32);
			}
		}
		else if (IsObeliskChallenge())
		{
			if (!IsMultiplayer())
			{
				SaveManager.SaveGame(12);
				SaveManager.SaveGame(13);
				SaveManager.SaveGame(14);
			}
			else
			{
				SaveManager.SaveGame(18);
				SaveManager.SaveGame(19);
				SaveManager.SaveGame(20);
			}
		}
		else if (IsSingularity())
		{
			if (!IsMultiplayer())
			{
				SaveManager.SaveGame(36);
				SaveManager.SaveGame(37);
				SaveManager.SaveGame(38);
			}
			else
			{
				SaveManager.SaveGame(42);
				SaveManager.SaveGame(43);
				SaveManager.SaveGame(44);
			}
		}
		else if (!IsMultiplayer())
		{
			SaveManager.SaveGame(0);
			SaveManager.SaveGame(1);
			SaveManager.SaveGame(2);
		}
		else
		{
			SaveManager.SaveGame(6);
			SaveManager.SaveGame(7);
			SaveManager.SaveGame(8);
		}
	}

	public void SetVsync()
	{
		if (configVsync)
		{
			QualitySettings.vSyncCount = 1;
			float num = (float)Screen.currentResolution.refreshRateRatio.value;
			if (num > 0f)
			{
				Application.targetFrameRate = Functions.FuncRoundToInt(num);
			}
			else
			{
				Application.targetFrameRate = 60;
			}
		}
		else
		{
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = Globals.Instance.NormalFPS;
		}
	}

	private void AlphaExpired()
	{
		QuitGame(instant: true);
	}

	public void AbortGameSave()
	{
		Debug.Log("Abort Game Save");
		AtOManager.Instance.CleanGameId();
		AlertManager.buttonClickDelegate = AbortGameSaveAction;
		AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("abortSave"));
	}

	private void AbortGameSaveAction()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(AbortGameSaveAction));
		NetworkManager.Instance.Disconnect();
		StartCoroutine(CoMainMenu());
	}

	private IEnumerator CoMainMenu()
	{
		yield return Globals.Instance.WaitForSeconds(0.1f);
		SceneStatic.LoadByName("MainMenu");
	}

	public void Discord()
	{
		Application.OpenURL("https://discord.gg/VuQKR2yVxC");
	}

	public void AbortGame()
	{
		StartCoroutine(AbortGameCo());
	}

	private IEnumerator AbortGameCo()
	{
		yield return Globals.Instance.WaitForSeconds(0.5f);
		Debug.Log("Abort Game");
		if (!AtOManager.Instance.IsAdventureCompleted() && !(SceneStatic.GetSceneName() == "FinishRun") && NetworkManager.Instance.IsConnected())
		{
			AtOManager.Instance.CleanGameId();
			AlertManager.buttonClickDelegate = AbortGameAction;
			AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("abortMultiplayer"));
		}
	}

	private void AbortGameAction()
	{
		Debug.Log("Abort Game Action");
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(AbortGameAction));
		NetworkManager.Instance.Disconnect();
		StartCoroutine(CoMainMenu());
	}

	private void SteamConfirm()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(SteamConfirm));
		QuitGame(instant: true);
	}

	public void SetDeveloperMode(bool state)
	{
		developerMode = state;
	}

	public void DebugShow()
	{
		Globals.Instance.ShowDebug = !Globals.Instance.ShowDebug;
	}

	public bool GetDeveloperMode()
	{
		return developerMode;
	}

	public void SetDemo(bool state)
	{
		isDemo = state;
	}

	public bool IsDemo()
	{
		return isDemo;
	}

	public void SetCursorPlain()
	{
		Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
	}

	public void SetCursorHover()
	{
		Cursor.SetCursor(cursorTextureHover, Vector2.zero, CursorMode.Auto);
	}

	public void SetGameVersionText()
	{
		if (!(gameVersionT != null) || SBversion == null)
		{
			return;
		}
		if (NetworkManager.Instance.IsConnected())
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(SBversion.ToString());
			string ping = NetworkManager.Instance.GetPing();
			if (ping != "")
			{
				stringBuilder.Append(" | ");
				stringBuilder.Append(Texts.Instance.GetText("ping"));
				stringBuilder.Append(": ");
				stringBuilder.Append(ping);
				stringBuilder.Append("ms");
				if (NetworkManager.Instance.IsOnRoom())
				{
					NetworkManager.Instance.SendPing(ping);
				}
			}
			gameVersionT.text = stringBuilder.ToString();
		}
		else
		{
			gameVersionT.text = SBversion.ToString();
		}
	}

	public bool TutorialWatched(string popType)
	{
		if (CheatMode && Instance.SkipTutorial)
		{
			return true;
		}
		if (PlayerManager.Instance.TutorialWatched != null && PlayerManager.Instance.TutorialWatched.Contains(popType))
		{
			return true;
		}
		return false;
	}

	public bool IsTutorialGame()
	{
		if (!Instance.IsGameAdventure())
		{
			return false;
		}
		if (AtOManager.Instance.GetGameId().ToLower() != "tuto".ToLower())
		{
			return false;
		}
		Hero[] team = AtOManager.Instance.GetTeam();
		List<string> list = new List<string>();
		list.Add("magnus");
		list.Add("andrin");
		list.Add("evelyn");
		list.Add("reginald");
		if (team != null && team.Length == 4)
		{
			for (int i = 0; i < team.Length; i++)
			{
				if (team[i] == null || team[i].HeroData == null)
				{
					return false;
				}
				if (!list.Contains(team[i].SourceName.ToLower()))
				{
					return false;
				}
			}
		}
		return true;
	}

	public void ShowTutorialPopup(string popType, Vector3 position, Vector3 position2)
	{
		List<string> obj = new List<string> { "characterPerks", "cardsReward", "townReward" };
		if (PlayerManager.Instance.TutorialWatched == null)
		{
			PlayerManager.Instance.TutorialWatched = new List<string>();
		}
		if (obj.Contains(popType) && AtOManager.Instance.IsCombatTool)
		{
			return;
		}
		if (PlayerManager.Instance.TutorialWatched.Contains(popType))
		{
			switch (popType)
			{
			case "townItemCraft":
			case "townItemUpgrade":
			case "townItemLoot":
				break;
			default:
				TutorialCombatContinue();
				return;
			}
		}
		PlayerManager.Instance.TutorialWatched.Add(popType);
		SaveManager.SavePlayerData();
		if (popTutorialGO != null)
		{
			HideTutorialPopup();
		}
		popTutorialGO = UnityEngine.Object.Instantiate(popTutorialPrefab, new Vector3(0f, 0f, -7f), Quaternion.identity, TutorialContainer);
		popTutorialGO.GetComponent<PopTutorialManager>().Show(popType, position, position2);
	}

	private void TutorialCombatContinue()
	{
		if (MatchManager.Instance != null)
		{
			MatchManager.Instance.waitingTutorial = false;
		}
	}

	public void HideTutorialPopup()
	{
		TutorialCombatContinue();
		UnityEngine.Object.Destroy(popTutorialGO);
	}

	public bool IsTutorialActive()
	{
		return popTutorialGO != null;
	}

	public void EscapeFunction(bool activateExit = true)
	{
		CardBackSelectionPanel cardBackSelectionPanel = null;
		if (HeroSelectionManager.Instance != null && HeroSelectionManager.Instance.CardBacksPopUp != null)
		{
			cardBackSelectionPanel = HeroSelectionManager.Instance.CardBacksPopUp.GetComponent<CardBackSelectionPanel>();
		}
		if (!PrefsLoaded || IsMaskActive() || AtOManager.Instance.saveLoadStatus)
		{
			return;
		}
		string sceneName = SceneStatic.GetSceneName();
		if (sceneName == "FinishRun")
		{
			return;
		}
		if (KeyboardManager.Instance.IsActive())
		{
			KeyboardManager.Instance.ShowKeyboard(state: false);
			return;
		}
		if (popTutorialGO != null && popTutorialGO.gameObject.activeSelf)
		{
			HideTutorialPopup();
			return;
		}
		if (AlertManager.Instance.IsActive())
		{
			AlertManager.Instance.CloseAlert();
			return;
		}
		if (SettingsManager.Instance.IsActive())
		{
			SettingsManager.Instance.ShowSettings(_state: false);
			return;
		}
		if (CardScreenManager.Instance.IsActive())
		{
			CardScreenManager.Instance.ShowCardScreen(_state: false);
			return;
		}
		if (DamageMeterManager.Instance.IsActive())
		{
			DamageMeterManager.Instance.Hide();
			return;
		}
		if (TomeManager.Instance.IsActive())
		{
			if (Functions.TransformIsVisible(TomeManager.Instance.runDetails))
			{
				TomeManager.Instance.RunDetailClose();
			}
			else if (Functions.TransformIsVisible(TomeManager.Instance.glossarySection))
			{
				TomeManager.Instance.SetPage(0);
			}
			else
			{
				TomeManager.Instance.ShowTome(_status: false);
			}
			return;
		}
		if (MadnessManager.Instance.IsActive())
		{
			MadnessManager.Instance.ShowMadness();
			return;
		}
		if (SandboxManager.Instance.IsActive())
		{
			SandboxManager.Instance.CloseSandbox();
			return;
		}
		if (GiveManager.Instance.IsActive())
		{
			GiveManager.Instance.ShowGive(state: false);
			return;
		}
		if (PerkTree.Instance.IsActive())
		{
			PerkTree.Instance.Hide();
			return;
		}
		if (cardBackSelectionPanel != null && cardBackSelectionPanel.gameObject.activeSelf)
		{
			cardBackSelectionPanel.Close();
			return;
		}
		if (sceneName != "ChallengeSelection" && sceneName != "Map" && CardCraftManager.Instance != null && !CardCraftManager.Instance.IsWaitingDivination())
		{
			if (Functions.TransformIsVisible(CardCraftManager.Instance.filterWindow))
			{
				CardCraftManager.Instance.ShowFilter(state: false);
			}
			else if (Functions.TransformIsVisible(CardCraftManager.Instance.cardCraftSave.transform))
			{
				CardCraftManager.Instance.ShowSaveLoad();
			}
			else
			{
				CardCraftManager.Instance.ExitCardCraft();
			}
			return;
		}
		if (sceneName == "Combat")
		{
			if (MatchManager.Instance.console.IsActive())
			{
				MatchManager.Instance.ShowLog();
			}
			else if (MatchManager.Instance.characterWindow.IsActive())
			{
				MatchManager.Instance.characterWindow.Hide();
			}
			else if (MatchManager.Instance.PreCastNum != -1)
			{
				MatchManager.Instance.ResetCastCardNum();
			}
			else if (MatchManager.Instance.CardDrag)
			{
				MatchManager.Instance.ControllerStopDrag();
			}
			return;
		}
		if (sceneName == "Town" && TownManager.Instance.characterWindow.IsActive())
		{
			TownManager.Instance.characterWindow.Hide();
			return;
		}
		if (sceneName == "Town" && TownManager.Instance.townUpgradeWindow.IsActive())
		{
			TownManager.Instance.ShowTownUpgrades(state: false);
			return;
		}
		if (sceneName == "Map" && MapManager.Instance.characterWindow.IsActive())
		{
			MapManager.Instance.characterWindow.Hide();
			return;
		}
		switch (sceneName)
		{
		case "HeroSelection":
			if (HeroSelectionManager.Instance.controllerCurrentHS != null)
			{
				HeroSelectionManager.Instance.controllerCurrentHS.ResetClickedController();
			}
			else if (HeroSelectionManager.Instance.charPopup != null && HeroSelectionManager.Instance.charPopup.IsOpened())
			{
				HeroSelectionManager.Instance.charPopup.Close();
			}
			break;
		case "MainMenu":
			if (MainMenuManager.Instance.IsDLCPopupActive())
			{
				MainMenuManager.Instance.HideDLCPopup();
			}
			else if (MainMenuManager.Instance.IsGameModesActive() || MainMenuManager.Instance.credits.gameObject.activeSelf || MainMenuManager.Instance.profilesT.gameObject.activeSelf)
			{
				MainMenuManager.Instance.ExitSaveGame();
			}
			else
			{
				QuitGame();
			}
			break;
		case "IntroNewGame":
			IntroNewGameManager.Instance.SkipIntro();
			break;
		case "Rewards":
			if (RewardsManager.Instance.characterWindowUI.IsActive())
			{
				RewardsManager.Instance.characterWindowUI.Hide();
			}
			else
			{
				OptionsManager.Instance.CantExitBecauseRewards();
			}
			break;
		case "Loot":
			if (LootManager.Instance.characterWindowUI.IsActive())
			{
				LootManager.Instance.characterWindowUI.Hide();
			}
			else
			{
				OptionsManager.Instance.CantExitBecauseRewards();
			}
			break;
		case "Cinematic":
			CinematicManager.Instance.SkipCinematic();
			break;
		case "Logos":
			LogosManager.Instance.Escape();
			break;
		default:
			if (activateExit)
			{
				OptionsManager.Instance.Exit();
			}
			break;
		}
	}

	public void QuitGame(bool instant = false)
	{
		if (!instant)
		{
			AlertManager.buttonClickDelegate = QuitGameAction;
			AlertManager.Instance.AlertConfirmDouble(Texts.Instance.GetText("wantToQuitGame"), Texts.Instance.GetText("accept").ToUpper(), Texts.Instance.GetText("cancel").ToUpper());
			AlertManager.Instance.ShowDoorIcon();
		}
		else
		{
			Application.Quit();
		}
	}

	private void QuitGameAction()
	{
		if (AlertManager.Instance.GetConfirmAnswer())
		{
			Application.Quit();
		}
		else
		{
			AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(QuitGameAction));
		}
	}

	public void Resize()
	{
		Globals.Instance.sizeW = Camera.main.orthographicSize * 2f * Camera.main.aspect;
		Globals.Instance.sizeH = Camera.main.orthographicSize * 2f;
		Globals.Instance.multiplierX = 0.9999999f;
		Globals.Instance.multiplierY = 1f;
		Globals.Instance.scale = 1920f * (float)Screen.height / (1080f * (float)Screen.width);
		Globals.Instance.scaleV = new Vector3(Globals.Instance.scale, Globals.Instance.scale, 1f);
		if (MainMenuManager.Instance != null)
		{
			MainMenuManager.Instance.Resize();
		}
		else if (HeroSelectionManager.Instance != null)
		{
			HeroSelectionManager.Instance.Resize();
		}
		else if (MapManager.Instance != null)
		{
			MapManager.Instance.Resize();
		}
		else if (ChallengeSelectionManager.Instance != null)
		{
			ChallengeSelectionManager.Instance.Resize();
		}
		else if (TownManager.Instance != null)
		{
			TownManager.Instance.Resize();
		}
		else if (MatchManager.Instance != null)
		{
			MatchManager.Instance.Resize();
		}
		if (OptionsManager.Instance != null)
		{
			OptionsManager.Instance.Resize();
		}
		if (PlayerUIManager.Instance != null)
		{
			PlayerUIManager.Instance.Resize();
		}
		if (CardCraftManager.Instance != null)
		{
			CardCraftManager.Instance.Resize();
		}
		if (SettingsManager.Instance != null)
		{
			SettingsManager.Instance.Resize();
		}
		if (CardScreenManager.Instance != null)
		{
			CardScreenManager.Instance.Resize();
		}
		if (PopupManager.Instance != null)
		{
			PopupManager.Instance.Resize();
		}
	}

	private void GenerateHeroes()
	{
		if (GetDeveloperMode())
		{
			Debug.Log("GenerateHeroes");
		}
		gameHeroes = new Dictionary<string, Hero>();
		Hero value = CreateHero("mercenary");
		gameHeroes.Add("mercenary", value);
		value = CreateHero("sentinel");
		gameHeroes.Add("sentinel", value);
		value = CreateHero("berserker");
		gameHeroes.Add("berserker", value);
		value = CreateHero("warden");
		gameHeroes.Add("warden", value);
		value = CreateHero("cleric");
		gameHeroes.Add("cleric", value);
		value = CreateHero("priest");
		gameHeroes.Add("priest", value);
		value = CreateHero("voodoowitch");
		gameHeroes.Add("voodoowitch", value);
		value = CreateHero("prophet");
		gameHeroes.Add("prophet", value);
		value = CreateHero("elementalist");
		gameHeroes.Add("elementalist", value);
		value = CreateHero("pyromancer");
		gameHeroes.Add("pyromancer", value);
		value = CreateHero("warlock");
		gameHeroes.Add("warlock", value);
		value = CreateHero("loremaster");
		gameHeroes.Add("loremaster", value);
		value = CreateHero("ranger");
		gameHeroes.Add("ranger", value);
		value = CreateHero("assassin");
		gameHeroes.Add("assassin", value);
		value = CreateHero("archer");
		gameHeroes.Add("archer", value);
		value = CreateHero("minstrel");
		gameHeroes.Add("minstrel", value);
		value = CreateHero("bandit");
		gameHeroes.Add("bandit", value);
		value = CreateHero("fallen");
		gameHeroes.Add("fallen", value);
		value = CreateHero("paladin");
		gameHeroes.Add("paladin", value);
		value = CreateHero("queen");
		gameHeroes.Add("queen", value);
		value = CreateHero("engineer");
		gameHeroes.Add("engineer", value);
		value = CreateHero("valkyrie");
		gameHeroes.Add("valkyrie", value);
		value = CreateHero("alchemist");
		gameHeroes.Add("alchemist", value);
		value = CreateHero("deathknight");
		if (value != null)
		{
			gameHeroes.Add("deathknight", value);
		}
		value = CreateHero("bloodmage");
		if (value != null)
		{
			gameHeroes.Add("bloodmage", value);
		}
	}

	public bool IsObeliskChallenge()
	{
		bool result = false;
		if (GameType == Enums.GameType.Challenge || GameType == Enums.GameType.WeeklyChallenge)
		{
			result = true;
		}
		return result;
	}

	public bool IsWeeklyChallenge()
	{
		return GameType == Enums.GameType.WeeklyChallenge;
	}

	public bool IsGameAdventure()
	{
		return GameType == Enums.GameType.Adventure;
	}

	public bool IsMultiplayer()
	{
		return GameMode == Enums.GameMode.Multiplayer;
	}

	public bool IsLoadingGame()
	{
		return GameStatus == Enums.GameStatus.LoadGame;
	}

	public bool IsSingularity()
	{
		return GameType == Enums.GameType.Singularity;
	}

	public void SetGameStatus(Enums.GameStatus status)
	{
		GameStatus = status;
	}

	public void LoadCombat()
	{
		SceneManager.LoadScene("Combat");
	}

	public void BackToGame()
	{
		SceneManager.LoadScene("Game");
	}

	public void SetCamera()
	{
		Camera[] allCameras = Camera.allCameras;
		for (int i = 0; i < allCameras.Length; i++)
		{
			if (allCameras[i].gameObject.tag != "MainCamera")
			{
				allCameras[i].gameObject.SetActive(value: false);
				continue;
			}
			cameraManager = allCameras[i].GetComponent<CameraManager>();
			cameraMain = allCameras[i];
		}
	}

	public void PlayLibraryAudio(string name, float timePassed = 0f)
	{
		if (gameIsOnFocus || !configBackgroundMute)
		{
			if (timePassed == 0f)
			{
				timePassed = 0.1f;
			}
			if (AudioManager.Instance.audioLibraryNew != null && AudioManager.Instance.audioLibraryNew.ContainsKey(name) && !configUseLegacySounds)
			{
				PlayAudio(AudioManager.Instance.audioLibraryNew[name], timePassed);
			}
			else if (AudioManager.Instance.audioLibrary.ContainsKey(name))
			{
				PlayAudio(AudioManager.Instance.audioLibrary[name], timePassed);
			}
		}
	}

	public void PlayAudio(AudioClip sound, float timePassed = 0f, float delay = 0f)
	{
		if ((!gameIsOnFocus && configBackgroundMute) || sound == null)
		{
			return;
		}
		bool flag = true;
		if (timePassed > 0f && AudioPlayed.TryGetValue(sound, out var _) && Time.time < AudioPlayed[sound] + timePassed)
		{
			flag = false;
		}
		if (flag)
		{
			if (delay == 0f)
			{
				Audio.PlayOneShot(sound, 1f);
				AudioPlayed[sound] = Time.time;
			}
			else
			{
				StartCoroutine(PlayAudioDelayed(sound, delay));
			}
		}
	}

	private IEnumerator PlayAudioDelayed(AudioClip sound, float delay)
	{
		yield return Globals.Instance.WaitForSeconds(delay);
		Audio.PlayOneShot(sound, 1f);
		AudioPlayed[sound] = Time.time;
	}

	public void SceneLoaded()
	{
		string sceneName = SceneStatic.GetSceneName();
		if (Startup.PDXContext == null && sceneName != "Game" && sceneName != "Logos")
		{
			Instance.QuitGame(instant: true);
		}
		else if (!(OptionsManager.Instance == null))
		{
			if (PopupManager.Instance != null)
			{
				PopupManager.Instance.ClosePopup();
			}
			CleanTempContainer();
			bool flag = true;
			bool flag2 = true;
			bool state = false;
			switch (sceneName)
			{
			case "Logos":
				flag = false;
				flag2 = false;
				break;
			case "Cinematic":
				flag = false;
				flag2 = false;
				break;
			case "TomeOfKnowledge":
				flag2 = false;
				flag = false;
				break;
			case "TeamManagement":
				flag2 = false;
				break;
			case "Lobby":
				flag2 = false;
				break;
			case "MainMenu":
				flag = false;
				flag2 = false;
				break;
			case "IntroNewGame":
				flag = false;
				flag2 = false;
				break;
			case "HeroSelection":
				flag2 = false;
				break;
			case "Combat":
				flag2 = false;
				break;
			case "FinishRun":
				flag2 = false;
				break;
			case "TrailerEnd":
				flag2 = false;
				break;
			case "TrailerPoster":
				flag2 = false;
				break;
			case "ChallengeSelection":
				flag2 = false;
				break;
			case "Map":
				state = true;
				break;
			case "Town":
				state = true;
				break;
			}
			if (flag2)
			{
				PlayerUIManager.Instance.Show();
			}
			else
			{
				PlayerUIManager.Instance.Hide();
			}
			if (flag)
			{
				OptionsManager.Instance.ShowScore(state);
				OptionsManager.Instance.SetMadness();
				OptionsManager.Instance.Show();
			}
			else
			{
				OptionsManager.Instance.Hide();
			}
			gameVersionT.transform.parent.gameObject.SetActive(value: true);
			Resize();
			ShowMask(state: false);
		}
	}

	public void SetMaskLoading()
	{
		if (MaskWindow.gameObject != null)
		{
			if (!MaskWindow.gameObject.activeSelf)
			{
				MaskWindow.gameObject.SetActive(value: true);
			}
			maskWindowImageBg.color = new Color(0f, 0f, 0f, 1f);
			maskWindowLoadingText.gameObject.SetActive(value: true);
		}
	}

	public void ChangeScene(string scene)
	{
		SetMaskLoading();
		HideTutorialPopup();
		if (TomeManager.Instance.IsActive())
		{
			TomeManager.Instance.ShowTome(_status: false);
		}
		if (MadnessManager.Instance.IsActive())
		{
			MadnessManager.Instance.ShowMadness();
		}
		if (SandboxManager.Instance.IsActive())
		{
			SandboxManager.Instance.CloseSandbox();
		}
		if (PerkTree.Instance.IsActive())
		{
			PerkTree.Instance.HideAction(checkSubclass: false);
		}
		Resources.UnloadUnusedAssets();
		GC.Collect();
		Globals.Instance.CleanInstantiatedCardData();
		NetworkManager.Instance.StartStopQueue(state: false);
		SceneManager.LoadScene(scene);
	}

	public void CleanTempContainer()
	{
		foreach (Transform item in TempContainer)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
	}

	public void ShowMask(bool state, string sceneToLoad = "")
	{
		if (coroutineMask != null)
		{
			StopCoroutine(coroutineMask);
		}
		coroutineMask = StartCoroutine(ShowMaskCo(state, sceneToLoad));
	}

	private IEnumerator ShowMaskCo(bool state, string sceneToLoad)
	{
		float maxAlplha = 1f;
		if (MaskWindow == null || MaskWindow.GetChild(0) == null || MaskWindow.GetChild(0).transform == null)
		{
			yield break;
		}
		float index = maskWindowImageBg.color.a;
		if (!state)
		{
			maskWindowLoadingText.gameObject.SetActive(value: false);
			while (index > 0f)
			{
				maskWindowImageBg.color = new Color(0f, 0f, 0f, index);
				index -= 0.05f;
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			maskWindowImageBg.color = new Color(0f, 0f, 0f, 0f);
			MaskWindow.gameObject.SetActive(value: false);
		}
		else
		{
			MaskWindow.gameObject.SetActive(value: true);
			while (index < maxAlplha)
			{
				maskWindowImageBg.color = new Color(0f, 0f, 0f, index);
				index += 0.2f;
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			maskWindowImageBg.color = new Color(0f, 0f, 0f, maxAlplha);
			maskWindowLoadingText.gameObject.SetActive(value: true);
		}
	}

	public bool IsMaskActive()
	{
		return MaskWindow.gameObject.activeSelf;
	}

	public Hero CreateHero(string subClass)
	{
		SubClassData subClassData = Globals.Instance.GetSubClassData(subClass);
		if (!subClassData)
		{
			return null;
		}
		string id = subClassData.HeroClass.ToString().ToLower();
		HeroData heroData = UnityEngine.Object.Instantiate(Globals.Instance.GetHeroData(id));
		Hero hero = new Hero();
		heroData.HeroSubClass = subClassData;
		hero.HeroData = heroData;
		hero.InitData();
		hero.GameName = subClassData.SubClassName;
		return hero;
	}

	public Hero AssignDataToHero(Hero hh)
	{
		if (hh == null || hh.SubclassName == "")
		{
			return null;
		}
		SubClassData subClassData = Globals.Instance.GetSubClassData(hh.SubclassName);
		if (subClassData == null)
		{
			return null;
		}
		HeroData heroData = UnityEngine.Object.Instantiate(Globals.Instance.GetHeroData(hh.ClassName));
		heroData.HeroSubClass = subClassData;
		hh.HeroData = heroData;
		hh.InitGeneralData();
		return hh;
	}

	public void GenerateParticleTrail(int type, Vector3 from, Vector3 to)
	{
		StartCoroutine(GenerateParticleTrailCo(type, from, to));
	}

	private IEnumerator GenerateParticleTrailCo(int type, Vector3 from, Vector3 to)
	{
		GameObject trail = type switch
		{
			0 => UnityEngine.Object.Instantiate(trailGoldPrefab, from, Quaternion.identity), 
			2 => UnityEngine.Object.Instantiate(trailCardPrefab, from, Quaternion.identity), 
			_ => UnityEngine.Object.Instantiate(trailDustPrefab, from, Quaternion.identity), 
		};
		if ((bool)MatchManager.Instance)
		{
			ParticleSystemRenderer component = trail.GetComponent<ParticleSystemRenderer>();
			if (component != null)
			{
				component.sortingLayerName = "Book";
			}
		}
		Vector3[] array = new Vector3[3];
		float x = ((!(from.x > to.x)) ? (from.x + Mathf.Abs(from.x - to.x) * UnityEngine.Random.Range(0.35f, 0.65f)) : (from.x - Mathf.Abs(to.x - from.x) * UnityEngine.Random.Range(0.35f, 0.65f)));
		float num = ((UnityEngine.Random.Range(0, 2) != 1) ? (from.y - Mathf.Abs(to.y - from.y + 2f) * UnityEngine.Random.Range(0.35f, 0.65f)) : (from.y + Mathf.Abs(to.y - from.y + 2f) * UnityEngine.Random.Range(0.35f, 0.65f)));
		if (num > Globals.Instance.sizeH * 0.5f || num < (0f - Globals.Instance.sizeH) * 0.5f)
		{
			num *= -1f;
		}
		if (num > Globals.Instance.sizeH * 0.5f || num < (0f - Globals.Instance.sizeH) * 0.5f)
		{
			num *= 0.5f;
		}
		Vector3 vector = new Vector3(x, num, 0f);
		array[0] = from;
		array[1] = vector;
		array[2] = to;
		Vector3[] gotPoints = LineSmoother.SmoothLine(array, 0.6f);
		float speed = 50f;
		PlayLibraryAudio("ui_swoosh");
		for (int i = 0; i < gotPoints.Length - 1; i++)
		{
			if (trail == null)
			{
				break;
			}
			trail.transform.position = gotPoints[i];
			Vector3 destination = gotPoints[i + 1];
			while (trail != null && Vector3.Distance(trail.transform.position, destination) > 0.2f)
			{
				float maxDistanceDelta = speed * Time.deltaTime;
				trail.transform.position = Vector3.MoveTowards(trail.transform.position, destination, maxDistanceDelta);
				yield return null;
			}
		}
		yield return Globals.Instance.WaitForSeconds(0.3f);
		UnityEngine.Object.Destroy(trail);
	}

	public void ShowedDemoMsg()
	{
		showedDemoMsg = true;
	}

	public bool ShowDemoMsgState()
	{
		return showedDemoMsg;
	}

	private void StartTimer()
	{
		storedDateTime = SteamManager.Instance.GetSteamTime().AddHours(2.0);
		if (clockCoroutine == null)
		{
			clockCoroutine = StartCoroutine(clockCounter());
		}
	}

	public DateTime GetTime()
	{
		_ = storedDateTime;
		return storedDateTime.AddSeconds(clockSeconds);
	}

	private IEnumerator clockCounter()
	{
		while (true)
		{
			yield return Globals.Instance.WaitForSeconds(1f);
			clockSeconds += 1.0;
		}
	}

	public void GetDisabledDLCs()
	{
		disabledDLCs = new List<string>();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Application.persistentDataPath);
		stringBuilder.Append("/");
		stringBuilder.Append("content_load.json");
		if (!File.Exists(stringBuilder.ToString()))
		{
			return;
		}
		foreach (ParadoxAppId item in JsonUtility.FromJson<DLCData>(File.ReadAllText(stringBuilder.ToString())).disabledDLC)
		{
			disabledDLCs.Add(item.paradoxAppId);
		}
	}
}
