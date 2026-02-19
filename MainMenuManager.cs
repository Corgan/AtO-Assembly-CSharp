using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using PDX.SDK.Contracts.Enums;
using Paradox;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using WebSocketSharp;

public class MainMenuManager : MonoBehaviour
{
	public Transform sceneCamera;

	public CanvasScaler relatedScaler;

	public Transform gameModeType;

	public Transform gameModeChoose;

	public Transform gameModeSelectionMode;

	public Transform gameModeSelectionChoose;

	public Transform gameModeSelectionDescription;

	public Transform gameModeSelectionDescriptionLarge;

	public Transform gameModeObeliskChains;

	public Transform gameModeWeeklyChains;

	public Transform gameModeSingularityChains;

	private bool challengeLocked;

	private bool weeklyLocked;

	private bool singularityLocked;

	public Transform credits;

	public Transform gameModeSelectionT;

	public Transform gameModeSelection0;

	public Transform gameModeSelection1;

	public Transform gameModeSelection2;

	public Transform gameModeSelection3;

	public TMP_Text gameModeWeekly;

	private string weeklyName;

	public Transform joinT;

	public Transform exitT;

	public Transform logo;

	public Transform[] menuOps;

	public Transform[] profileOps;

	public TMP_Text[] profileOpsText;

	public Transform profileDelete;

	public TMP_Text profileDeleteText;

	public MenuSaveButton[] menuSaveButtons;

	public Transform fadeBorders;

	public Transform menuT;

	public Transform socialT;

	public Transform saveT;

	public Transform profilesT;

	public Transform saveSlots;

	public Transform saveSingleT;

	public Transform saveMultiT;

	public Transform saveChallengeT;

	public Transform saveChallengeTSingle;

	public Transform saveChallengeTMulti;

	public TMP_Text saveTitle;

	public TMP_Text saveDescription;

	public TMP_Text challengeTitle;

	public TMP_Text challengeDescription;

	public Button challengeSPButton;

	public Button challengeMPButton;

	public TMP_Text version;

	public Transform bannerObject;

	public Transform exitSaveGameButton;

	private GameData[] saveGames;

	private Coroutine maskCoroutine;

	public SpriteRenderer maskImage;

	private bool setWeekly;

	private StringBuilder SBWeekly = new StringBuilder();

	private bool singlePlayer = true;

	public List<Transform> menuController0;

	public List<Transform> menuController1;

	public List<Transform> menuControllerModeSelection;

	public List<Transform> menuControllerSaveGames;

	public List<Transform> menuControllerProfiler;

	public List<Transform> menuControllerPDX;

	public TMP_Text creditText;

	public ScrollRect creditScrollRect;

	private Coroutine creditsCo;

	private float creditScrollSpeed = 90f;

	public Transform weeklyReward;

	public SpriteRenderer weeklyRewardCardback;

	public SpriteRenderer weeklyRewardCardbackSecondary;

	public TMP_Text profileMenuText;

	private string[] profiles;

	private int profileCreateSlot;

	private Coroutine gameModeSelectionCoroutine;

	public Transform DLCInformationT;

	public Transform DLCT;

	public Button DLChalloween;

	public Transform DLChalloweenLineOk;

	public Transform DLChalloweenLineKo;

	public Button DLCwolfwars;

	public Transform DLCwolfwarsLineOk;

	public Transform DLCwolfwarsLineKo;

	public Button DLCulminin;

	public Transform DLCulmininLineOk;

	public Transform DLCulmininLineKo;

	public Button DLCqueen;

	public Transform DLCqueenLineOk;

	public Transform DLCqueenLineKo;

	public Button DLCuprising;

	public Transform DLCuprisingLineOk;

	public Transform DLCuprisingLineKo;

	public Button DLCnenukil;

	public Transform DLCnenukilLineOk;

	public Transform DLCnenukilLineKo;

	public Button DLCsahti;

	public Transform DLCsahtiLineOk;

	public Transform DLCsahtiLineKo;

	public Button DLCsigrun;

	public Transform DLCsigrunLineOk;

	public Transform DLCsigrunLineKo;

	public Button DLCbernard;

	public Transform DLCbernardLineOk;

	public Transform DLCbernardLineKo;

	public Button DLCtemple;

	public Transform DLCtempleLineOk;

	public Transform DLCtempleLineKo;

	public Button DLCspider;

	public Transform DLCspiderLineOk;

	public Transform DLCspiderLineKo;

	public Button DLCAsianSkins;

	public Transform DLCAsianSkinsLineOk;

	public Transform DLCAsianSkinsLineKo;

	public Button DLCNordicSkins;

	public Transform DLCNordicSkinsLineOk;

	public Transform DLCNordicSkinsLineKo;

	public Button DLCNecropolis;

	public Transform DLCNecropolisOk;

	public Transform DLCNecropolisKo;

	public Transform DLCpopup;

	public Transform DLCpopupImageNecropolis;

	public Transform DLCpopupImageWW;

	public Transform DLCpopupImageUlminin;

	public Transform DLCpopupImageObsidian;

	public Transform DLCpopupImageSkins;

	public Transform DLCpopupImageAmelia;

	public Transform DLCpopupImageNenukil;

	public Transform DLCpopupImageSahti;

	public Transform DLCpopupImageSigrun;

	public Transform DLCpopupImageBernard;

	public Transform DLCpopupImageTemple;

	public Transform DLCpopupImageSpider;

	public Transform DLCpopupImageNordicSkins;

	public Transform DLCpopupImageAsianSkins;

	public Transform DLCExitButton;

	public Transform DLCLinkButton;

	public TMP_Text DLCpopupTitle;

	public TMP_Text DLCpopupDescription;

	private int activePopup;

	public Transform paradoxT;

	public Transform paradoxMask;

	public Transform paradoxLoginPopup;

	public Transform paradoxLoginPrePopup;

	public Transform paradoxLoginFieldsPopup;

	public Transform paradoxLoggedPopup;

	public Transform paradoxCreatePopup;

	public Transform paradoxDocumentPopup;

	public TMP_InputField paradoxLoginUser;

	public TMP_InputField paradoxLoginPassword;

	public TMP_Text paradoxLoggedUser;

	public TMP_Text paradoxDocumentText;

	public ScrollRect paradoxDocumentScrollRect;

	public Transform paradoxDocumentCloseButton;

	public TMP_InputField paradoxCreateEmail;

	public TMP_InputField paradoxCreatePassword;

	public TMP_Dropdown paradoxDropdownRegion;

	public TMP_Dropdown paradoxDropdownDay;

	public TMP_Dropdown paradoxDropdownMonth;

	public TMP_Dropdown paradoxDropdownYear;

	public Toggle paradoxCreateOffers;

	private SortedDictionary<string, string> regionDictionary;

	private Vector2 preLoginBackgroundSize = new Vector2(440f, 260f);

	private Vector2 loginBackgroundSize = new Vector2(440f, 380f);

	private Vector3 vectorPositionGameMode = new Vector3(-8.2f, 0f, 0f);

	private List<Transform> controllerList = new List<Transform>();

	public int controllerHorizontalIndex = -1;

	private Vector2 warpPosition = Vector2.zero;

	public static MainMenuManager Instance { get; private set; }

	private void Awake()
	{
		if (GameManager.Instance == null)
		{
			SceneStatic.LoadByName("MainMenu");
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
		sceneCamera.gameObject.SetActive(value: false);
		NetworkManager.Instance.StartStopQueue(state: true);
	}

	private void Update()
	{
		if (setWeekly && Time.frameCount % 24 == 0)
		{
			SetWeeklyLeft();
		}
	}

	private void Start()
	{
		StartAsync();
	}

	private async void StartAsync()
	{
		AtOManager.Instance.IsCombatTool = false;
		HideGameModeSelection();
		DLCPopupShow(state: false);
		await Startup.Start();
		GiveManager.Instance.ShowGive(state: false);
		ChatManager.Instance.DisableChat();
		SteamManager.Instance.SetRichPresence("steam_display", "#Status_MainMenu");
		AtOManager.Instance.ClearGame();
		AtOManager.Instance.CleanSaveSlot();
		PlayerManager.Instance.ClearAdventurePerks();
		NetworkManager.Instance.ClearPreviousInfo();
		PerkTree.Instance.Hide();
		CardScreenManager.Instance.ShowCardScreen(_state: false);
		DamageMeterManager.Instance.Hide();
		GameManager.Instance.GameMode = Enums.GameMode.SinglePlayer;
		PositionMenuItems();
		LoadCredits();
		LoadProfiles();
		ShowHideDLCInformation(state: false);
		ShowDLCButtons();
		ShowSaveGame(status: false);
		CreatePDXDropdowns();
		ResetPDXScreens();
		if (Startup.isLoggedIn)
		{
			ShowPDXLogged();
		}
		else
		{
			ShowPDXPreLogin();
		}
		GameManager.Instance.SetGameVersionText();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("v.");
		stringBuilder.Append(GameManager.Instance.gameVersion);
		version.text = stringBuilder.ToString();
		AudioManager.Instance.StopAmbience();
		AudioManager.Instance.DoBSO("Game");
		if (GameManager.Instance.mainMenuGoToMultiplayer)
		{
			GameManager.Instance.mainMenuGoToMultiplayer = false;
			Multiplayer();
		}
		else if (NetworkManager.Instance.IsConnected())
		{
			NetworkManager.Instance.Disconnect();
		}
		if (GameManager.Instance.FirstLoad)
		{
			GameManager.Instance.FirstLoad = false;
			if (GameManager.Instance.PDXCliContinue)
			{
				int slotFromContinue = SaveManager.GetSlotFromContinue();
				GameData gameData = SaveManager.GetGameData(slotFromContinue);
				if (gameData == null)
				{
					SaveManager.RemoveGameContinue();
				}
				else if (Functions.CheckIfSavegameIsCompatible(gameData) == "")
				{
					AtOManager.Instance.SetSaveSlot(slotFromContinue);
					AtOManager.Instance.LoadGame(slotFromContinue);
					return;
				}
			}
		}
		if (GameManager.Instance.CheatMode)
		{
			if (GameManager.Instance.UseManyResources)
			{
				AtOManager.Instance.UseManyResources();
			}
			if (GameManager.Instance.UnlockAllExceptHeroes)
			{
				PlayerManager.Instance.UnlockAllExceptHeroes();
			}
			if (GameManager.Instance.UnlockHeroes)
			{
				PlayerManager.Instance.UnlockHeroes();
			}
		}
		GameManager.Instance.SceneLoaded();
	}

	private void LoadCredits()
	{
		string text = Texts.Instance.GetText("creditsList");
		if (text == "")
		{
			TextAsset textAsset = (TextAsset)Resources.Load("Credits/creditsText");
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(textAsset.text);
			stringBuilder.Replace("<company>", "<br><br><size=+15><color=#ECAC32>");
			stringBuilder.Replace("</company>", "</color></size>");
			stringBuilder.Replace("<header>", "<br><size=+8><color=#FFCE6D>");
			stringBuilder.Replace("</header>", "</color></size>");
			stringBuilder.Replace("<position>", "<br><size=-5><color=#999>");
			stringBuilder.Replace("</position>", "</color></size>");
			text = stringBuilder.ToString();
			Texts.Instance.SetText("creditsList", text);
		}
		creditText.text = text;
	}

	public bool IsDLCPopupActive()
	{
		return DLCpopup.gameObject.activeSelf;
	}

	private void DLCPopupShow(bool state)
	{
		if (DLCpopup.gameObject.activeSelf != state)
		{
			DLCpopup.gameObject.SetActive(state);
		}
	}

	public void DLCButtonHoverOn()
	{
		GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonHover, 0.1f);
	}

	public void ShowHideDLCInformation(bool state)
	{
		DLCInformationT.gameObject.SetActive(!state);
		DLCT.gameObject.SetActive(state);
	}

	private void ShowDLCButtons()
	{
		Color normalColor = new Color(0.3f, 0.3f, 0.3f, 1f);
		if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[0]))
		{
			ColorBlock colors = DLChalloween.colors;
			colors.normalColor = normalColor;
			DLChalloween.colors = colors;
			DLChalloweenLineOk.gameObject.SetActive(value: false);
			DLChalloweenLineKo.gameObject.SetActive(value: true);
		}
		else
		{
			DLChalloweenLineOk.gameObject.SetActive(value: true);
			DLChalloweenLineKo.gameObject.SetActive(value: false);
		}
		if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[1]))
		{
			ColorBlock colors = DLCwolfwars.colors;
			colors.normalColor = normalColor;
			DLCwolfwars.colors = colors;
			DLCwolfwarsLineOk.gameObject.SetActive(value: false);
			DLCwolfwarsLineKo.gameObject.SetActive(value: true);
		}
		else
		{
			DLCwolfwarsLineOk.gameObject.SetActive(value: true);
			DLCwolfwarsLineKo.gameObject.SetActive(value: false);
		}
		if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[2]))
		{
			ColorBlock colors = DLCulminin.colors;
			colors.normalColor = normalColor;
			DLCulminin.colors = colors;
			DLCulmininLineOk.gameObject.SetActive(value: false);
			DLCulmininLineKo.gameObject.SetActive(value: true);
		}
		else
		{
			DLCulmininLineOk.gameObject.SetActive(value: true);
			DLCulmininLineKo.gameObject.SetActive(value: false);
		}
		if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[3]))
		{
			ColorBlock colors = DLCqueen.colors;
			colors.normalColor = normalColor;
			DLCqueen.colors = colors;
			DLCqueenLineOk.gameObject.SetActive(value: false);
			DLCqueenLineKo.gameObject.SetActive(value: true);
		}
		else
		{
			DLCqueenLineOk.gameObject.SetActive(value: true);
			DLCqueenLineKo.gameObject.SetActive(value: false);
		}
		if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[4]))
		{
			ColorBlock colors = DLCuprising.colors;
			colors.normalColor = normalColor;
			DLCuprising.colors = colors;
			DLCuprisingLineOk.gameObject.SetActive(value: false);
			DLCuprisingLineKo.gameObject.SetActive(value: true);
		}
		else
		{
			DLCuprisingLineOk.gameObject.SetActive(value: true);
			DLCuprisingLineKo.gameObject.SetActive(value: false);
		}
		if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[5]))
		{
			ColorBlock colors = DLCnenukil.colors;
			colors.normalColor = normalColor;
			DLCnenukil.colors = colors;
			DLCnenukilLineOk.gameObject.SetActive(value: false);
			DLCnenukilLineKo.gameObject.SetActive(value: true);
		}
		else
		{
			DLCnenukilLineOk.gameObject.SetActive(value: true);
			DLCnenukilLineKo.gameObject.SetActive(value: false);
		}
		if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[6]))
		{
			ColorBlock colors = DLCsahti.colors;
			colors.normalColor = normalColor;
			DLCsahti.colors = colors;
			DLCsahtiLineOk.gameObject.SetActive(value: false);
			DLCsahtiLineKo.gameObject.SetActive(value: true);
		}
		else
		{
			DLCsahtiLineOk.gameObject.SetActive(value: true);
			DLCsahtiLineKo.gameObject.SetActive(value: false);
		}
		if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[7]))
		{
			ColorBlock colors = DLCsigrun.colors;
			colors.normalColor = normalColor;
			DLCsigrun.colors = colors;
			DLCsigrunLineOk.gameObject.SetActive(value: false);
			DLCsigrunLineKo.gameObject.SetActive(value: true);
		}
		else
		{
			DLCsigrunLineOk.gameObject.SetActive(value: true);
			DLCsigrunLineKo.gameObject.SetActive(value: false);
		}
		if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[8]))
		{
			ColorBlock colors = DLCbernard.colors;
			colors.normalColor = normalColor;
			DLCbernard.colors = colors;
			DLCbernardLineOk.gameObject.SetActive(value: false);
			DLCbernardLineKo.gameObject.SetActive(value: true);
		}
		else
		{
			DLCbernardLineOk.gameObject.SetActive(value: true);
			DLCbernardLineKo.gameObject.SetActive(value: false);
		}
		if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[9]))
		{
			ColorBlock colors = DLCtemple.colors;
			colors.normalColor = normalColor;
			DLCtemple.colors = colors;
			DLCtempleLineOk.gameObject.SetActive(value: false);
			DLCtempleLineKo.gameObject.SetActive(value: true);
		}
		else
		{
			DLCtempleLineOk.gameObject.SetActive(value: true);
			DLCtempleLineKo.gameObject.SetActive(value: false);
		}
		if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[10]))
		{
			ColorBlock colors = DLCspider.colors;
			colors.normalColor = normalColor;
			DLCspider.colors = colors;
			DLCspiderLineOk.gameObject.SetActive(value: false);
			DLCspiderLineKo.gameObject.SetActive(value: true);
		}
		else
		{
			DLCspiderLineOk.gameObject.SetActive(value: true);
			DLCspiderLineKo.gameObject.SetActive(value: false);
		}
		if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[11]))
		{
			ColorBlock colors = DLCAsianSkins.colors;
			colors.normalColor = normalColor;
			DLCAsianSkins.colors = colors;
			DLCAsianSkinsLineOk.gameObject.SetActive(value: false);
			DLCAsianSkinsLineKo.gameObject.SetActive(value: true);
		}
		else
		{
			DLCAsianSkinsLineOk.gameObject.SetActive(value: true);
			DLCAsianSkinsLineKo.gameObject.SetActive(value: false);
		}
		if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[12]))
		{
			ColorBlock colors = DLCNecropolis.colors;
			colors.normalColor = normalColor;
			DLCNecropolis.colors = colors;
			DLCNecropolisOk.gameObject.SetActive(value: false);
			DLCNecropolisKo.gameObject.SetActive(value: true);
		}
		else
		{
			DLCNecropolisOk.gameObject.SetActive(value: true);
			DLCNecropolisKo.gameObject.SetActive(value: false);
		}
	}

	public void ShowDLCPopup(int _index)
	{
		DLCPopupShow(state: true);
		activePopup = _index;
		DLCpopupImageSkins.gameObject.SetActive(value: false);
		DLCpopupImageWW.gameObject.SetActive(value: false);
		DLCpopupImageUlminin.gameObject.SetActive(value: false);
		DLCpopupImageAmelia.gameObject.SetActive(value: false);
		DLCpopupImageObsidian.gameObject.SetActive(value: false);
		DLCpopupImageNenukil.gameObject.SetActive(value: false);
		DLCpopupImageSahti.gameObject.SetActive(value: false);
		DLCpopupImageSigrun.gameObject.SetActive(value: false);
		DLCpopupImageBernard.gameObject.SetActive(value: false);
		DLCpopupImageTemple.gameObject.SetActive(value: false);
		DLCpopupImageSpider.gameObject.SetActive(value: false);
		DLCpopupImageNordicSkins.gameObject.SetActive(value: false);
		DLCpopupImageAsianSkins.gameObject.SetActive(value: false);
		DLCpopupImageNecropolis.gameObject.SetActive(value: false);
		try
		{
			DLCpopupTitle.text = SteamManager.Instance.GetDLCName(Globals.Instance.SkuAvailable[activePopup]);
		}
		catch (Exception)
		{
			Debug.LogError("DLC sku not found");
		}
		if (activePopup == 0)
		{
			DLCpopupDescription.text = Texts.Instance.GetText("howToDLCSkin");
			DLCpopupImageSkins.gameObject.SetActive(value: true);
		}
		else if (activePopup == 1)
		{
			DLCpopupDescription.text = Texts.Instance.GetText("howToDLCWW");
			DLCpopupImageWW.gameObject.SetActive(value: true);
		}
		else if (activePopup == 2)
		{
			DLCpopupDescription.text = Texts.Instance.GetText("howToDLCUlminin");
			DLCpopupImageUlminin.gameObject.SetActive(value: true);
		}
		else if (activePopup == 3)
		{
			DLCpopupDescription.text = Texts.Instance.GetText("howToDLCCharacter");
			TMP_Text dLCpopupDescription = DLCpopupDescription;
			dLCpopupDescription.text = dLCpopupDescription.text + "<br><br>" + Texts.Instance.GetText("howToDLCPet");
			DLCpopupImageAmelia.gameObject.SetActive(value: true);
		}
		else if (activePopup == 4)
		{
			DLCpopupDescription.text = Texts.Instance.GetText("howToDLCObsidian");
			DLCpopupImageObsidian.gameObject.SetActive(value: true);
		}
		else if (activePopup == 5)
		{
			DLCpopupDescription.text = Texts.Instance.GetText("howToDLCCharacter");
			TMP_Text dLCpopupDescription2 = DLCpopupDescription;
			dLCpopupDescription2.text = dLCpopupDescription2.text + "<br><br>" + Texts.Instance.GetText("howToDLCPet");
			DLCpopupImageNenukil.gameObject.SetActive(value: true);
		}
		else if (activePopup == 6)
		{
			DLCpopupDescription.text = Texts.Instance.GetText("howToDLCSahti");
			DLCpopupImageSahti.gameObject.SetActive(value: true);
		}
		else if (activePopup == 7)
		{
			DLCpopupDescription.text = Texts.Instance.GetText("howToDLCCharacter");
			TMP_Text dLCpopupDescription3 = DLCpopupDescription;
			dLCpopupDescription3.text = dLCpopupDescription3.text + "<br><br>" + Texts.Instance.GetText("howToDLCPet");
			DLCpopupImageSigrun.gameObject.SetActive(value: true);
		}
		else if (activePopup == 8)
		{
			DLCpopupDescription.text = Texts.Instance.GetText("howToDLCCharacter");
			TMP_Text dLCpopupDescription4 = DLCpopupDescription;
			dLCpopupDescription4.text = dLCpopupDescription4.text + "<br><br>" + Texts.Instance.GetText("howToDLCPet");
			DLCpopupImageBernard.gameObject.SetActive(value: true);
		}
		else if (activePopup == 9)
		{
			DLCpopupDescription.text = Texts.Instance.GetText("howToDLCTemple");
			DLCpopupImageTemple.gameObject.SetActive(value: true);
		}
		else if (activePopup == 10)
		{
			DLCpopupDescription.text = Texts.Instance.GetText("howToDLCCharacter");
			TMP_Text dLCpopupDescription5 = DLCpopupDescription;
			dLCpopupDescription5.text = dLCpopupDescription5.text + "<br><br>" + Texts.Instance.GetText("howToDLCPet");
			DLCpopupImageSpider.gameObject.SetActive(value: true);
		}
		else if (activePopup == 11)
		{
			DLCpopupDescription.text = Texts.Instance.GetText("howToDLCSkin");
			DLCpopupImageAsianSkins.gameObject.SetActive(value: true);
		}
		else if (activePopup == 12)
		{
			DLCpopupDescription.text = Texts.Instance.GetText("howToDLCNecro");
			DLCpopupImageNecropolis.gameObject.SetActive(value: true);
		}
		else if (activePopup == 13)
		{
			DLCpopupDescription.text = Texts.Instance.GetText("howToDLCSkin");
			DLCpopupImageNordicSkins.gameObject.SetActive(value: true);
		}
	}

	public void HideDLCPopup()
	{
		DLCPopupShow(state: false);
	}

	public void ButtonDLCPopupLink()
	{
		Application.OpenURL("https://store.steampowered.com/app/" + Globals.Instance.SkuAvailable[activePopup]);
	}

	public void LoadProfiles()
	{
		profiles = SaveManager.GetProfileNames();
	}

	private void SetMenuCurrentProfile()
	{
		profileMenuText.text = string.Format(Texts.Instance.GetText("profileMenu"), profiles[GameManager.Instance.ProfileId]);
	}

	private void HideGameModeSelection()
	{
		if (weeklyReward.gameObject.activeSelf)
		{
			weeklyReward.gameObject.SetActive(value: false);
		}
		SelectGameMode(-1);
		gameModeSelectionMode.gameObject.SetActive(value: false);
		gameModeSelectionChoose.gameObject.SetActive(value: false);
		gameModeSelectionDescription.gameObject.SetActive(value: false);
		gameModeSelectionDescriptionLarge.gameObject.SetActive(value: false);
		gameModeSelectionT.gameObject.SetActive(value: false);
		gameModeSelection0.gameObject.SetActive(value: false);
		gameModeSelection1.gameObject.SetActive(value: false);
		gameModeSelection2.gameObject.SetActive(value: false);
		gameModeSelection3.gameObject.SetActive(value: false);
		joinT.gameObject.SetActive(value: false);
		exitT.gameObject.SetActive(value: false);
	}

	private void MenuControllerHoverOff()
	{
		for (int i = 0; i < menuController0.Count; i++)
		{
			if ((bool)menuController0[i].GetComponent<MenuButton>())
			{
				menuController0[i].GetComponent<MenuButton>().HoverOff();
			}
		}
		exitT.GetChild(0).GetComponent<MenuButton>().HoverOff();
	}

	private void ShowGameModeSelection(int _type)
	{
		GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
		MenuControllerHoverOff();
		if (PlayerManager.Instance.GetHighestCharacterRank() < 3)
		{
			challengeLocked = (weeklyLocked = true);
		}
		else
		{
			challengeLocked = (weeklyLocked = false);
		}
		if ((!GameManager.Instance.CheatMode && GameManager.Instance.UnlockAllExceptHeroes) || PlayerManager.Instance.GetHighestCharacterRank() < 3)
		{
			singularityLocked = true;
		}
		else
		{
			singularityLocked = false;
		}
		LoadSaveGames();
		ShowMask(state: true, 0.8f);
		profilesT.gameObject.SetActive(value: false);
		menuT.gameObject.SetActive(value: false);
		bannerObject.gameObject.SetActive(value: false);
		ShowHideDLCInformation(state: false);
		HideGameModeSelection();
		gameModeSelection0.GetChild(0).GetComponent<BotonMenuGameMode>().TurnOffState();
		gameModeSelection1.GetChild(0).GetComponent<BotonMenuGameMode>().TurnOffState();
		gameModeSelection2.GetChild(0).GetComponent<BotonMenuGameMode>().TurnOffState();
		gameModeSelection3.GetChild(0).GetComponent<BotonMenuGameMode>().TurnOffState();
		switch (_type)
		{
		case 0:
			gameModeType.GetComponent<TMP_Text>().text = Texts.Instance.GetText("mainMenuSaveSingle");
			gameModeSelectionMode.GetComponent<TMP_Text>().text = Texts.Instance.GetText("mainMenuSaveSingle");
			break;
		case 1:
			gameModeType.GetComponent<TMP_Text>().text = Texts.Instance.GetText("mainMenuSaveMulti");
			gameModeSelectionMode.GetComponent<TMP_Text>().text = Texts.Instance.GetText("mainMenuSaveMulti");
			joinT.gameObject.SetActive(value: true);
			break;
		}
		exitT.gameObject.SetActive(value: true);
		gameModeType.gameObject.SetActive(value: true);
		gameModeChoose.gameObject.SetActive(value: true);
		if (gameModeSelectionCoroutine != null)
		{
			StopCoroutine(gameModeSelectionCoroutine);
		}
		gameModeSelectionCoroutine = StartCoroutine(ShowGameModeSelectionCo());
	}

	private void SelectGameMode(int _index)
	{
		if (gameModeSelectionCoroutine != null)
		{
			StopCoroutine(gameModeSelectionCoroutine);
		}
		gameModeWeekly.text = "";
		setWeekly = false;
		gameModeType.gameObject.SetActive(value: false);
		gameModeChoose.gameObject.SetActive(value: false);
		gameModeSelectionMode.gameObject.SetActive(value: true);
		gameModeSelectionChoose.gameObject.SetActive(value: true);
		gameModeSelection0.gameObject.SetActive(value: true);
		gameModeSelection1.gameObject.SetActive(value: true);
		gameModeSelection2.gameObject.SetActive(value: true);
		gameModeSelection3.gameObject.SetActive(value: true);
		if (_index == 0 || _index == 1)
		{
			GameManager.Instance.GameType = Enums.GameType.Adventure;
			gameModeSelection0.localPosition = vectorPositionGameMode;
			gameModeSelection0.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
			StartCoroutine(BotonMenuGameModeStateOn(0));
			gameModeSelectionChoose.GetComponent<TMP_Text>().text = Texts.Instance.GetText("modeAdventure");
			gameModeSelectionDescriptionLarge.gameObject.SetActive(value: true);
			gameModeSelectionDescriptionLarge.GetComponent<TMP_Text>().text = Texts.Instance.GetText("mainMenuAdventureDescription");
		}
		else if (_index > -1)
		{
			gameModeSelection0.gameObject.SetActive(value: false);
		}
		else
		{
			gameModeSelection0.localPosition = new Vector3(-10.5f, 0f, 0f);
			gameModeSelection0.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
		}
		if (_index == 2 || _index == 3)
		{
			GameManager.Instance.GameType = Enums.GameType.Challenge;
			gameModeSelection1.localPosition = vectorPositionGameMode;
			gameModeSelection1.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
			StartCoroutine(BotonMenuGameModeStateOn(1));
			gameModeSelectionChoose.GetComponent<TMP_Text>().text = Texts.Instance.GetText("modeObelisk");
			gameModeSelectionDescriptionLarge.gameObject.SetActive(value: true);
			gameModeSelectionDescriptionLarge.GetComponent<TMP_Text>().text = Texts.Instance.GetText("mainMenuObeliskDescription");
		}
		else if (_index > -1)
		{
			gameModeSelection1.gameObject.SetActive(value: false);
		}
		else
		{
			gameModeSelection1.localPosition = new Vector3(-3.5f, 0f, 0f);
			gameModeSelection1.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
		}
		if (_index == 4 || _index == 5)
		{
			GameManager.Instance.GameType = Enums.GameType.WeeklyChallenge;
			gameModeSelection2.localPosition = vectorPositionGameMode;
			gameModeSelection2.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
			StartCoroutine(BotonMenuGameModeStateOn(2));
			gameModeSelectionChoose.GetComponent<TMP_Text>().text = Texts.Instance.GetText("modeWeekly");
			gameModeSelectionDescription.gameObject.SetActive(value: true);
			gameModeSelectionDescription.GetComponent<TMP_Text>().text = Texts.Instance.GetText("mainMenuWeeklyDescription");
			SetWeeklyLeft();
			setWeekly = true;
			ChallengeData weeklyData = Globals.Instance.GetWeeklyData(Functions.GetCurrentWeeklyWeek());
			if (weeklyData != null)
			{
				CardbackData cardbackData = weeklyData.GetCardbackData();
				if (cardbackData != null)
				{
					weeklyReward.gameObject.SetActive(value: true);
					weeklyRewardCardback.sprite = cardbackData.CardbackSprite;
				}
				weeklyData = Globals.Instance.GetWeeklyData(Functions.GetCurrentWeeklyWeek(), _getSecondary: true);
				weeklyRewardCardbackSecondary.gameObject.SetActive(value: false);
				if (weeklyData != null)
				{
					cardbackData = weeklyData.GetCardbackData();
					if (cardbackData != null)
					{
						weeklyRewardCardbackSecondary.sprite = cardbackData.CardbackSprite;
						weeklyRewardCardbackSecondary.gameObject.SetActive(value: true);
					}
				}
			}
		}
		else if (_index > -1)
		{
			gameModeSelection2.gameObject.SetActive(value: false);
		}
		else
		{
			gameModeSelection2.localPosition = new Vector3(3.5f, 0f, 0f);
			gameModeSelection2.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
		}
		if (_index == 6 || _index == 7)
		{
			GameManager.Instance.GameType = Enums.GameType.Singularity;
			gameModeSelection3.localPosition = vectorPositionGameMode;
			gameModeSelection3.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
			StartCoroutine(BotonMenuGameModeStateOn(3));
			gameModeSelectionChoose.GetComponent<TMP_Text>().text = Texts.Instance.GetText("singularity");
			gameModeSelectionDescriptionLarge.gameObject.SetActive(value: true);
			gameModeSelectionDescriptionLarge.GetComponent<TMP_Text>().text = Texts.Instance.GetText("mainMenuSingularityDescription");
		}
		else if (_index > -1)
		{
			gameModeSelection3.gameObject.SetActive(value: false);
		}
		else
		{
			gameModeSelection3.localPosition = new Vector3(10.5f, 0f, 0f);
			gameModeSelection3.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
		}
	}

	private IEnumerator BotonMenuGameModeStateOn(int _option)
	{
		yield return null;
		switch (_option)
		{
		case 0:
			gameModeSelection0.GetChild(0).GetComponent<BotonMenuGameMode>().TurnOnState();
			break;
		case 1:
			gameModeSelection1.GetChild(0).GetComponent<BotonMenuGameMode>().TurnOnState();
			break;
		case 2:
			gameModeSelection2.GetChild(0).GetComponent<BotonMenuGameMode>().TurnOnState();
			break;
		case 3:
			gameModeSelection3.GetChild(0).GetComponent<BotonMenuGameMode>().TurnOnState();
			break;
		}
	}

	private IEnumerator ShowGameModeSelectionCo()
	{
		gameModeSelectionT.gameObject.SetActive(value: true);
		if (challengeLocked)
		{
			gameModeObeliskChains.gameObject.SetActive(value: true);
		}
		else
		{
			gameModeObeliskChains.gameObject.SetActive(value: false);
		}
		if (weeklyLocked)
		{
			gameModeWeeklyChains.gameObject.SetActive(value: true);
		}
		else
		{
			gameModeWeeklyChains.gameObject.SetActive(value: false);
		}
		if (singularityLocked)
		{
			gameModeSingularityChains.gameObject.SetActive(value: true);
		}
		else
		{
			gameModeSingularityChains.gameObject.SetActive(value: false);
		}
		gameModeSelection0.gameObject.SetActive(value: true);
		yield return Globals.Instance.WaitForSeconds(0.1f);
		gameModeSelection1.gameObject.SetActive(value: true);
		yield return Globals.Instance.WaitForSeconds(0.1f);
		gameModeSelection2.gameObject.SetActive(value: true);
		yield return Globals.Instance.WaitForSeconds(0.1f);
		gameModeSelection3.gameObject.SetActive(value: true);
	}

	public void GoToSteamPage()
	{
		Application.OpenURL("https://store.steampowered.com/app/1385380/Across_the_Obelisk/");
	}

	public void GoToLeaderboard()
	{
		Application.OpenURL("https://steamcommunity.com/stats/1421400/leaderboards/6109803");
	}

	public void GoToDiscord()
	{
		GameManager.Instance.Discord();
	}

	private void LoadSaveGames()
	{
		saveGames = SaveManager.SaveGamesList();
	}

	private void PositionSaveItems(int section)
	{
		int num = 0;
		switch (section)
		{
		case 0:
			num = 0;
			break;
		case 1:
			num = 6;
			break;
		case 2:
			num = 12;
			break;
		case 3:
			num = 18;
			break;
		case 4:
			num = 24;
			break;
		case 5:
			num = 30;
			break;
		case 6:
			num = 36;
			break;
		case 7:
			num = 42;
			break;
		}
		for (int i = 0; i < 3; i++)
		{
			if (menuSaveButtons[i] != null)
			{
				menuSaveButtons[i].HoverOff();
				menuSaveButtons[i].SetSlot(num + i);
				if (saveGames[num + i] != null)
				{
					menuSaveButtons[i].SetActive(_state: true);
					menuSaveButtons[i].SetGameData(saveGames[num + i]);
				}
				else
				{
					menuSaveButtons[i].SetActive(_state: false);
				}
			}
		}
	}

	public void Resize()
	{
		PositionMenuItems();
		if (Globals.Instance.scale < 1f)
		{
			relatedScaler.matchWidthOrHeight = 1f;
		}
		else
		{
			relatedScaler.matchWidthOrHeight = 0f;
		}
	}

	private void PositionMenuItems()
	{
		float num = 1920f * (float)Screen.height / (1080f * (float)Screen.width);
		float num2 = 50f * Globals.Instance.multiplierY * num;
		float num3 = num2 * 2f - 610f;
		int num4 = 0;
		for (int num5 = menuOps.Length - 1; num5 >= 0; num5--)
		{
			if (menuOps[num5] != null)
			{
				menuOps[num5].transform.localPosition = new Vector3(menuOps[num5].transform.localPosition.x, num3 + (float)num4 * num2);
			}
			num4++;
		}
		num4 = 0;
		num3 = menuOps[menuOps.Length - 3].transform.localPosition.y - num2;
		profileDelete.transform.localPosition = new Vector3(profileDelete.transform.localPosition.x, num3 - 2f * num2);
		for (int num6 = profileOps.Length - 1; num6 >= 0; num6--)
		{
			if (profileOps[num6] != null)
			{
				profileOps[num6].transform.localPosition = new Vector3(profileOps[num6].transform.localPosition.x, num3 + (float)num4 * num2);
			}
			num4++;
		}
	}

	private void ClearButtonsState()
	{
		for (int i = 0; i < menuOps.Length; i++)
		{
			if (menuOps[i] != null)
			{
				Button component = menuOps[i].GetComponent<Button>();
				component.interactable = false;
				component.interactable = true;
			}
		}
	}

	public bool IsSaveMenuActive()
	{
		return saveT.gameObject.activeSelf;
	}

	public bool IsGameModesActive()
	{
		return gameModeSelectionT.gameObject.activeSelf;
	}

	public void ShowCredits()
	{
		GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
		MenuControllerHoverOff();
		ShowMask(state: true, 0.8f);
		profilesT.gameObject.SetActive(value: false);
		menuT.gameObject.SetActive(value: false);
		bannerObject.gameObject.SetActive(value: false);
		credits.gameObject.SetActive(value: true);
		exitT.gameObject.SetActive(value: true);
		ClearButtonsState();
		if (creditsCo != null)
		{
			StopCoroutine(creditsCo);
		}
		creditsCo = StartCoroutine(CreditAutoMovement());
	}

	public void HideCredits()
	{
		if (credits.gameObject.activeSelf)
		{
			credits.gameObject.SetActive(value: false);
			exitT.gameObject.SetActive(value: false);
		}
		if (creditsCo != null)
		{
			StopCoroutine(creditsCo);
		}
	}

	private IEnumerator CreditAutoMovement()
	{
		creditScrollRect.verticalNormalizedPosition = 1f;
		yield return null;
		float creditsContentHeight = creditScrollRect.content.rect.height;
		while (true)
		{
			if (!Mouse.current.leftButton.isPressed && creditScrollRect.verticalNormalizedPosition > 0f)
			{
				creditScrollRect.verticalNormalizedPosition = Mathf.Clamp01(creditScrollRect.verticalNormalizedPosition - creditScrollSpeed / creditsContentHeight * Time.deltaTime);
			}
			yield return null;
		}
	}

	private void MoveCredits(bool goDown)
	{
		if (goDown)
		{
			creditScrollRect.verticalNormalizedPosition -= 0.01f;
		}
		else
		{
			creditScrollRect.verticalNormalizedPosition += 0.01f;
		}
	}

	public void ShowSaveGame(bool status, int section = -1)
	{
		if ((challengeLocked && section == 2) || (weeklyLocked && section == 4) || (singularityLocked && section == 6))
		{
			return;
		}
		ShowMask(status, 0.8f);
		profilesT.gameObject.SetActive(value: false);
		saveT.gameObject.SetActive(status);
		menuT.gameObject.SetActive(!status);
		HideCredits();
		bannerObject.gameObject.SetActive(!status);
		if (status)
		{
			saveSlots.gameObject.SetActive(value: true);
			if (!singlePlayer)
			{
				section++;
			}
			PositionSaveItems(section);
			SelectGameMode(section);
		}
		else
		{
			SetMenuCurrentProfile();
		}
	}

	public void NewGame()
	{
		GameManager.Instance.GameType = Enums.GameType.Adventure;
		singlePlayer = true;
		ShowGameModeSelection(0);
	}

	public void LoadGame()
	{
		GameManager.Instance.GameType = Enums.GameType.Adventure;
		ShowSaveGame(status: true, 0);
		ClearButtonsState();
	}

	public void ExitSaveGame(bool _forceIt = false)
	{
		if (_forceIt || !AlertManager.Instance.IsActive())
		{
			GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
			if (NetworkManager.Instance.IsConnected())
			{
				NetworkManager.Instance.Disconnect();
			}
			setWeekly = false;
			HideGameModeSelection();
			LoadSaveGames();
			ShowSaveGame(status: false);
		}
	}

	public void ObeliskChallenge()
	{
		GameManager.Instance.GameType = Enums.GameType.Challenge;
		ShowSaveGame(status: true, 2);
		ClearButtonsState();
		challengeTitle.text = Texts.Instance.GetText("menuChallenge");
	}

	public void ObeliskChallengeSP()
	{
		ShowSaveGame(status: true, 2);
		ClearButtonsState();
	}

	public void ObeliskChallengeMP()
	{
		ShowSaveGame(status: true, 3);
		ClearButtonsState();
	}

	public void WeeklyChallenge()
	{
		GameManager.Instance.GameType = Enums.GameType.WeeklyChallenge;
		ShowSaveGame(status: true, 2);
		ClearButtonsState();
		SetWeeklyLeft();
		setWeekly = true;
	}

	public void Multiplayer()
	{
		GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
		ShowGameModeSelection(1);
		singlePlayer = false;
		GameManager.Instance.GameType = Enums.GameType.Adventure;
	}

	public void JoinMultiplayer()
	{
		GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
		GameManager.Instance.GameType = Enums.GameType.Adventure;
		SceneStatic.LoadByName("Lobby");
	}

	public void LoadTeamManagement()
	{
		SceneStatic.LoadByName("TeamManagement");
	}

	public void TomeOfKnowledge()
	{
		GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
		SceneStatic.LoadByName("TomeOfKnowledge");
		ClearButtonsState();
	}

	public void Statistics()
	{
		ClearButtonsState();
	}

	public void Settings()
	{
		GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
		SettingsManager.Instance.ShowSettings(_state: true);
		ClearButtonsState();
	}

	public void Credits()
	{
		ShowCredits();
		ClearButtonsState();
	}

	public void QuitGame()
	{
		GameManager.Instance.QuitGame();
		ClearButtonsState();
	}

	public void ShowProfiles()
	{
		LoadProfiles();
		profilesT.gameObject.SetActive(value: true);
		menuT.gameObject.SetActive(value: false);
		exitT.gameObject.SetActive(value: true);
		string text = "";
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < profileOps.Length; i++)
		{
			stringBuilder.Clear();
			if (i == GameManager.Instance.ProfileId)
			{
				stringBuilder.Append("<size=+3><b><sprite name=experience><color=orange>");
				text = profiles[i];
			}
			if (profiles[i] == "")
			{
				stringBuilder.Append("<size=-4><color=#B0B0B0>");
				stringBuilder.Append(Texts.Instance.GetText("profileCreate"));
			}
			else
			{
				stringBuilder.Append(profiles[i]);
			}
			profileOpsText[i].text = stringBuilder.ToString();
		}
		if (GameManager.Instance.ProfileId == 0 || text == "")
		{
			if (profileDelete.gameObject.activeSelf)
			{
				profileDelete.gameObject.SetActive(value: false);
			}
			return;
		}
		stringBuilder.Clear();
		stringBuilder.Append("<color=#B0B0B0>");
		stringBuilder.Append(text);
		stringBuilder.Append("</color>");
		profileDeleteText.text = string.Format(Texts.Instance.GetText("deleteProfile"), stringBuilder.ToString());
		if (!profileDelete.gameObject.activeSelf)
		{
			profileDelete.gameObject.SetActive(value: true);
		}
	}

	public void DeleteProfile()
	{
		if (GameManager.Instance.ProfileId > 0 && profiles[GameManager.Instance.ProfileId] != "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format(Texts.Instance.GetText("doYouWantToDeleteProfile"), profiles[GameManager.Instance.ProfileId]));
			stringBuilder.Append("<br><size=-4><color=#AAAAAA>");
			stringBuilder.Append(Texts.Instance.GetText("wantToRemoveSavePermanent"));
			stringBuilder.Append("</color></size>");
			AlertManager.buttonClickDelegate = DeleteProfileAction;
			AlertManager.Instance.AlertConfirmDouble(stringBuilder.ToString());
		}
	}

	private void DeleteProfileAction()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(DeleteProfileAction));
		if (AlertManager.Instance.GetConfirmAnswer())
		{
			SaveManager.DeleteProfileFolder(GameManager.Instance.ProfileId);
			UseProfile(0);
		}
	}

	public void UseProfile(int _slot)
	{
		if (_slot == GameManager.Instance.ProfileId)
		{
			ExitSaveGame();
			return;
		}
		SaveManager.RemoveGameContinue();
		if (profiles[_slot] == "")
		{
			CreateProfile(_slot);
			return;
		}
		GameManager.Instance.UseProfileFile(_slot);
		GameManager.Instance.ReloadProfile();
		ExitSaveGame(_forceIt: true);
	}

	private void CreateProfile(int _slot)
	{
		if (_slot != 0)
		{
			profileCreateSlot = _slot;
			AlertManager.buttonClickDelegate = CreateProfileAction;
			AlertManager.Instance.AlertInput(Texts.Instance.GetText("inputConfigSaveName"), Texts.Instance.GetText("accept").ToUpper());
		}
	}

	public void CreateProfileAction()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(CreateProfileAction));
		string text = Functions.OnlyAscii(AlertManager.Instance.GetInputValue()).ToUpper().Trim();
		if (!(text == ""))
		{
			SaveManager.CreateProfileFolder(profileCreateSlot, text);
		}
	}

	private void ShowMask(bool state, float alpha = 1f)
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
			maskCoroutine = StartCoroutine(HideMaskCo());
		}
	}

	private IEnumerator ShowMaskCo(float alpha = 1f)
	{
		float index = maskImage.color.a;
		while (index < alpha)
		{
			maskImage.color = new Color(0f, 0f, 0f, index);
			index += 0.025f;
			yield return null;
		}
		maskImage.color = new Color(0f, 0f, 0f, alpha);
	}

	private IEnumerator HideMaskCo()
	{
		float index = maskImage.color.a;
		while (index > 0f)
		{
			maskImage.color = new Color(0f, 0f, 0f, index);
			index -= 0.025f;
			yield return null;
		}
	}

	private void SetWeeklyLeft()
	{
		TimeSpan timeSpan = Functions.TimeSpanLeftWeekly();
		string value = $"{(int)timeSpan.TotalHours:D2}h. {timeSpan.Minutes:D2}m. {timeSpan.Seconds:D2}s.";
		SBWeekly.Clear();
		SBWeekly.Append("<b><size=+2>");
		if (weeklyName.IsNullOrEmpty())
		{
			weeklyName = AtOManager.Instance.GetWeeklyName(Functions.GetCurrentWeeklyWeek());
		}
		SBWeekly.Append(weeklyName);
		SBWeekly.Append("</size></b>\n");
		SBWeekly.Append(value);
		gameModeWeekly.text = SBWeekly.ToString();
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false)
	{
		bool flag = false;
		controllerList.Clear();
		if (Functions.TransformIsVisible(credits))
		{
			if (goingUp)
			{
				MoveCredits(goDown: false);
				return;
			}
			if (goingDown)
			{
				MoveCredits(goDown: true);
				return;
			}
			warpPosition = exitT.GetChild(0).position;
			Mouse.current.WarpCursorPosition(warpPosition);
			return;
		}
		if (DLCpopup.gameObject.activeSelf)
		{
			controllerList.Add(DLCLinkButton);
			controllerList.Add(DLCExitButton);
		}
		else if (!paradoxDocumentPopup.gameObject.activeSelf)
		{
			if (IsSaveMenuActive() || IsGameModesActive())
			{
				for (int i = 0; i < menuController1.Count; i++)
				{
					controllerList.Add(menuController1[i]);
				}
				if (IsSaveMenuActive())
				{
					for (int j = 0; j < menuControllerSaveGames.Count; j++)
					{
						controllerList.Add(menuControllerSaveGames[j]);
					}
				}
				else if (IsGameModesActive())
				{
					for (int k = 0; k < menuControllerModeSelection.Count; k++)
					{
						controllerList.Add(menuControllerModeSelection[k]);
					}
				}
			}
			else if (profilesT.gameObject.activeSelf)
			{
				for (int l = 0; l < menuControllerProfiler.Count; l++)
				{
					controllerList.Add(menuControllerProfiler[l]);
				}
			}
			else
			{
				flag = true;
				for (int m = 0; m < menuController0.Count; m++)
				{
					controllerList.Add(menuController0[m]);
				}
				if (DLCT.gameObject.activeSelf)
				{
					foreach (Transform item in DLCT.transform)
					{
						controllerList.Add(item);
					}
				}
			}
			for (int n = 0; n < menuControllerPDX.Count; n++)
			{
				controllerList.Add(menuControllerPDX[n]);
			}
			for (int num = controllerList.Count - 1; num >= 0; num--)
			{
				if (!Functions.TransformIsVisible(controllerList[num]))
				{
					controllerList.RemoveAt(num);
				}
			}
		}
		else
		{
			controllerList.Add(paradoxDocumentCloseButton);
		}
		controllerHorizontalIndex = Functions.GetListClosestIndexToMousePosition(controllerList, checkUiItems: true);
		bool flag2 = false;
		if (flag && ((goingDown && controllerHorizontalIndex == 5) || (goingUp && controllerHorizontalIndex == 7)))
		{
			controllerHorizontalIndex = 6;
			flag2 = true;
		}
		if (!flag2)
		{
			controllerHorizontalIndex = Functions.GetClosestIndexBasedOnDirection(controllerList, controllerHorizontalIndex, goingUp, goingRight, goingDown, goingLeft, checkUiItems: true);
		}
		if (controllerList[controllerHorizontalIndex] != null)
		{
			warpPosition = controllerList[controllerHorizontalIndex].position;
			Mouse.current.WarpCursorPosition(warpPosition);
		}
	}

	public void CreatePDXDropdowns()
	{
		paradoxDropdownRegion.options.Clear();
		List<string> list = new List<string> { Texts.Instance.GetText("Region") };
		regionDictionary = new SortedDictionary<string, string>();
		string[] names = Enum.GetNames(typeof(Country));
		foreach (string text in names)
		{
			if (text != "Undefined")
			{
				if (Globals.Instance.IsoToEnglishCountryNames.ContainsKey(text))
				{
					regionDictionary.Add(Globals.Instance.IsoToEnglishCountryNames[text], text);
				}
				else
				{
					regionDictionary.Add(text, text);
				}
			}
		}
		foreach (KeyValuePair<string, string> item in regionDictionary)
		{
			list.Add(item.Key);
		}
		paradoxDropdownRegion.AddOptions(list);
		paradoxDropdownDay.options.Clear();
		list.Clear();
		list.Add(Texts.Instance.GetText("Day"));
		for (int j = 0; j < 31; j++)
		{
			list.Add((j + 1).ToString());
		}
		paradoxDropdownDay.AddOptions(list);
		paradoxDropdownMonth.options.Clear();
		list.Clear();
		list.Add(Texts.Instance.GetText("Month"));
		for (int k = 0; k < 12; k++)
		{
			list.Add(Texts.Instance.GetText("month" + (k + 1)));
		}
		paradoxDropdownMonth.AddOptions(list);
		paradoxDropdownYear.options.Clear();
		list.Clear();
		list.Add(Texts.Instance.GetText("Year"));
		for (int num = DateTime.Now.Year; num >= DateTime.Now.Year - 100; num--)
		{
			list.Add(num.ToString());
		}
		paradoxDropdownYear.AddOptions(list);
	}

	public void ResetPDXScreens()
	{
		if (paradoxLoginPopup.gameObject.activeSelf)
		{
			paradoxLoginPopup.gameObject.SetActive(value: false);
		}
		if (paradoxLoginPrePopup.gameObject.activeSelf)
		{
			paradoxLoginPrePopup.gameObject.SetActive(value: false);
		}
		if (paradoxLoginFieldsPopup.gameObject.activeSelf)
		{
			paradoxLoginFieldsPopup.gameObject.SetActive(value: false);
		}
		if (paradoxLoggedPopup.gameObject.activeSelf)
		{
			paradoxLoggedPopup.gameObject.SetActive(value: false);
		}
		if (paradoxCreatePopup.gameObject.activeSelf)
		{
			paradoxCreatePopup.gameObject.SetActive(value: false);
		}
		ShowPDXMask(state: false);
	}

	public void ShowPDXMask(bool state)
	{
		if (paradoxMask.gameObject.activeSelf != state)
		{
			paradoxMask.gameObject.SetActive(state);
		}
	}

	public void PDXPreLoginButton()
	{
		ShowPDXLogin();
	}

	public void ShowPDXPreLogin()
	{
		ResetPDXScreens();
		paradoxLoginPopup.GetComponent<RectTransform>().sizeDelta = preLoginBackgroundSize;
		if (!paradoxLoginPopup.gameObject.activeSelf)
		{
			paradoxLoginPopup.gameObject.SetActive(value: true);
		}
		if (!paradoxLoginPrePopup.gameObject.activeSelf)
		{
			paradoxLoginPrePopup.gameObject.SetActive(value: true);
		}
	}

	public void ShowPDXLogin()
	{
		ResetPDXScreens();
		paradoxLoginPopup.GetComponent<RectTransform>().sizeDelta = loginBackgroundSize;
		if (!paradoxLoginPopup.gameObject.activeSelf)
		{
			paradoxLoginPopup.gameObject.SetActive(value: true);
		}
		if (!paradoxLoginFieldsPopup.gameObject.activeSelf)
		{
			paradoxLoginFieldsPopup.gameObject.SetActive(value: true);
		}
	}

	public void ShowPDXLogged()
	{
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("ShowPDXLogged");
		}
		ResetPDXScreens();
		if (!paradoxLoggedPopup.gameObject.activeSelf)
		{
			paradoxLoggedPopup.gameObject.SetActive(value: true);
		}
		SetLoggedUsername();
	}

	private void SetLoggedUsername()
	{
		if (!string.IsNullOrEmpty(Startup.userNamespaceName))
		{
			paradoxLoggedUser.text = Startup.userNamespaceName;
		}
		else if (!string.IsNullOrEmpty(Startup.userSocialName))
		{
			paradoxLoggedUser.text = Startup.userSocialName;
		}
		else
		{
			paradoxLoggedUser.text = Functions.HideEmail(Account.Email);
		}
	}

	public void ShowPDXCreate()
	{
		ResetPDXScreens();
		if (!paradoxCreatePopup.gameObject.activeSelf)
		{
			paradoxCreatePopup.gameObject.SetActive(value: true);
		}
	}

	public void ShowPDXDocumentFromForm(int document)
	{
		Startup.ShowDocumentFromForm(document);
	}

	public void ShowPDXDocument()
	{
		StartCoroutine(ShowPDXDocumentCo());
	}

	public IEnumerator ShowPDXDocumentCo()
	{
		if (!paradoxDocumentPopup.gameObject.activeSelf)
		{
			paradoxDocumentPopup.gameObject.SetActive(value: true);
		}
		yield return Globals.Instance.WaitForSeconds(0.01f);
		paradoxDocumentScrollRect.verticalNormalizedPosition = 1f;
	}

	public void ClosePDXDocument()
	{
		EventSystem.current.SetSelectedGameObject(null);
		if (Startup.waitingForLoginDocuments)
		{
			Startup.ShowDocumentFromStartup();
		}
		else
		{
			paradoxDocumentPopup.gameObject.SetActive(value: false);
		}
	}

	public void PDXLogin()
	{
		ShowPDXMask(state: true);
		string text = Functions.Sanitize(paradoxLoginUser.text, doLowerCase: false);
		string text2 = Functions.Sanitize(paradoxLoginPassword.text, doLowerCase: false);
		if (text != "" && text2 != "")
		{
			Account.SetEmailAndPasswordCredential(text, text2);
			Account.LoginWithEmailAndPassword();
		}
		else
		{
			ShowPDXError("pdxErrorLoginFields");
		}
	}

	public void LogoutPDX()
	{
		ShowPDXMask(state: true);
		AlertManager.buttonClickDelegate = LogoutPDXAction;
		AlertManager.Instance.AlertConfirmDouble(Texts.Instance.GetText("pdxLogoutConfirm"));
	}

	private void LogoutPDXAction()
	{
		bool confirmAnswer = AlertManager.Instance.GetConfirmAnswer();
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(LogoutPDXAction));
		if (!confirmAnswer)
		{
			ShowPDXMask(state: false);
			return;
		}
		Account.Logout();
		Startup.userId = "";
	}

	public void LinkSteamPDX()
	{
		AlertManager.buttonClickDelegate = LinkSteamPDXAction;
		AlertManager.Instance.AlertConfirmDouble(Texts.Instance.GetText("pdxSteamLinkConfirm"));
	}

	private void LinkSteamPDXAction()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(LinkSteamPDXAction));
	}

	public void CreatePDXAccount()
	{
		bool marketingPermission = false;
		string email = Functions.Sanitize(paradoxCreateEmail.text, doLowerCase: false);
		string text = Functions.Sanitize(paradoxCreatePassword.text, doLowerCase: false);
		int value = paradoxDropdownRegion.value;
		int value2 = paradoxDropdownDay.value;
		int value3 = paradoxDropdownDay.value;
		int value4 = paradoxDropdownDay.value;
		if (paradoxCreateOffers.isOn)
		{
			marketingPermission = true;
		}
		string text2 = "";
		if (!Functions.IsValidEmail(email))
		{
			text2 = "pdxErrorEmail";
		}
		else if (text.Length < 6 || text.Length > 128)
		{
			text2 = "pdxErrorPassword";
		}
		else if (value == 0)
		{
			text2 = "pdxErrorRegion";
		}
		else if (value2 == 0 || value3 == 0 || value4 == 0)
		{
			text2 = "pdxErrorBirth";
		}
		else if (!IsDateDifferenceGreaterThan16Years(new DateTime(value4, value3, value2)))
		{
			text2 = "pdx16years";
		}
		if (text2 == "")
		{
			string text3 = paradoxDropdownRegion.options[paradoxDropdownRegion.value].text;
			string country = "";
			if (regionDictionary.ContainsKey(text3))
			{
				country = regionDictionary[text3];
			}
			string birthdate = paradoxDropdownYear.options[paradoxDropdownYear.value].text + "-" + paradoxDropdownMonth.value.ToString("D2") + "-" + paradoxDropdownDay.value.ToString("D2");
			Account.SetEmailAndPasswordCredential(email, text);
			Account.SetCountry(country);
			Account.SetBirthdate(birthdate);
			Account.SetLang();
			Account.SetMarketingPermission(marketingPermission);
			Account.CreateParadoxAccount();
		}
		else
		{
			ShowPDXError(text2);
		}
	}

	private bool IsDateDifferenceGreaterThan16Years(DateTime birthDate)
	{
		DateTime now = DateTime.Now;
		int num = Mathf.Abs(now.Year - birthDate.Year);
		if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
		{
			num--;
		}
		return num > 16;
	}

	public void ShowPDXError(string error)
	{
		ShowPDXMask(state: false);
		AlertManager.buttonClickDelegate = null;
		AlertManager.Instance.AlertConfirm(Texts.Instance.GetText(error));
	}
}
