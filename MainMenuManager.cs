// Decompiled with JetBrains decompiler
// Type: MainMenuManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using Paradox;
using PDX.SDK.Contracts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using WebSocketSharp;

#nullable disable
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
  public Transform DLCpopup;
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
  private Vector3 vectorPositionGameMode = new Vector3(-8.2f, 0.0f, 0.0f);
  private List<Transform> controllerList = new List<Transform>();
  public int controllerHorizontalIndex = -1;
  private Vector2 warpPosition = Vector2.zero;

  public static MainMenuManager Instance { get; private set; }

  private void Awake()
  {
    if ((UnityEngine.Object) GameManager.Instance == (UnityEngine.Object) null)
    {
      SceneStatic.LoadByName("MainMenu");
    }
    else
    {
      if ((UnityEngine.Object) MainMenuManager.Instance == (UnityEngine.Object) null)
        MainMenuManager.Instance = this;
      else if ((UnityEngine.Object) MainMenuManager.Instance != (UnityEngine.Object) this)
        UnityEngine.Object.Destroy((UnityEngine.Object) this);
      this.sceneCamera.gameObject.SetActive(false);
      NetworkManager.Instance.StartStopQueue(true);
    }
  }

  private void Update()
  {
    if (!this.setWeekly || Time.frameCount % 24 != 0)
      return;
    this.SetWeeklyLeft();
  }

  private void Start() => this.StartAsync();

  private async void StartAsync()
  {
    this.HideGameModeSelection();
    this.DLCPopupShow(false);
    await Startup.Start();
    GiveManager.Instance.ShowGive(false);
    ChatManager.Instance.DisableChat();
    SteamManager.Instance.SetRichPresence("steam_display", "#Status_MainMenu");
    AtOManager.Instance.ClearGame();
    AtOManager.Instance.CleanSaveSlot();
    PlayerManager.Instance.ClearAdventurePerks();
    NetworkManager.Instance.ClearPreviousInfo();
    PerkTree.Instance.Hide();
    CardScreenManager.Instance.ShowCardScreen(false);
    DamageMeterManager.Instance.Hide();
    GameManager.Instance.GameMode = global::Enums.GameMode.SinglePlayer;
    this.PositionMenuItems();
    this.LoadCredits();
    this.LoadProfiles();
    this.ShowHideDLCInformation(false);
    this.ShowDLCButtons();
    this.ShowSaveGame(false);
    this.CreatePDXDropdowns();
    this.ResetPDXScreens();
    if (Startup.isLoggedIn)
      this.ShowPDXLogged();
    else
      this.ShowPDXPreLogin();
    GameManager.Instance.SetGameVersionText();
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("v.");
    stringBuilder.Append(GameManager.Instance.gameVersion);
    this.version.text = stringBuilder.ToString();
    AudioManager.Instance.StopAmbience();
    AudioManager.Instance.DoBSO("Game");
    if (GameManager.Instance.mainMenuGoToMultiplayer)
    {
      GameManager.Instance.mainMenuGoToMultiplayer = false;
      this.Multiplayer();
    }
    else if (NetworkManager.Instance.IsConnected())
      NetworkManager.Instance.Disconnect();
    if (GameManager.Instance.FirstLoad)
    {
      GameManager.Instance.FirstLoad = false;
      if (GameManager.Instance.PDXCliContinue)
      {
        int slotFromContinue = SaveManager.GetSlotFromContinue();
        GameData gameData = SaveManager.GetGameData(slotFromContinue);
        if (gameData == null)
          SaveManager.RemoveGameContinue();
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
        AtOManager.Instance.UseManyResources();
      if (GameManager.Instance.UnlockAllHeroes)
        AtOManager.Instance.UnlockAllHeroes();
      if (GameManager.Instance.UnlockMadness)
        PlayerManager.Instance.SingularityMadnessLevel = 10;
    }
    GameManager.Instance.SceneLoaded();
  }

  private void LoadCredits()
  {
    string text = Texts.Instance.GetText("creditsList");
    if (text == "")
    {
      TextAsset textAsset = (TextAsset) UnityEngine.Resources.Load("Credits/creditsText");
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
    this.creditText.text = text;
  }

  public bool IsDLCPopupActive() => this.DLCpopup.gameObject.activeSelf;

  private void DLCPopupShow(bool state)
  {
    if (this.DLCpopup.gameObject.activeSelf == state)
      return;
    this.DLCpopup.gameObject.SetActive(state);
  }

  public void DLCButtonHoverOn()
  {
    GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonHover, 0.1f);
  }

  public void ShowHideDLCInformation(bool state)
  {
    this.DLCInformationT.gameObject.SetActive(!state);
    this.DLCT.gameObject.SetActive(state);
  }

  private void ShowDLCButtons()
  {
    Color color = new Color(0.3f, 0.3f, 0.3f, 1f);
    if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[0]))
    {
      this.DLChalloween.colors = this.DLChalloween.colors with
      {
        normalColor = color
      };
      this.DLChalloweenLineOk.gameObject.SetActive(false);
      this.DLChalloweenLineKo.gameObject.SetActive(true);
    }
    else
    {
      this.DLChalloweenLineOk.gameObject.SetActive(true);
      this.DLChalloweenLineKo.gameObject.SetActive(false);
    }
    if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[1]))
    {
      this.DLCwolfwars.colors = this.DLCwolfwars.colors with
      {
        normalColor = color
      };
      this.DLCwolfwarsLineOk.gameObject.SetActive(false);
      this.DLCwolfwarsLineKo.gameObject.SetActive(true);
    }
    else
    {
      this.DLCwolfwarsLineOk.gameObject.SetActive(true);
      this.DLCwolfwarsLineKo.gameObject.SetActive(false);
    }
    if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[2]))
    {
      this.DLCulminin.colors = this.DLCulminin.colors with
      {
        normalColor = color
      };
      this.DLCulmininLineOk.gameObject.SetActive(false);
      this.DLCulmininLineKo.gameObject.SetActive(true);
    }
    else
    {
      this.DLCulmininLineOk.gameObject.SetActive(true);
      this.DLCulmininLineKo.gameObject.SetActive(false);
    }
    if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[3]))
    {
      this.DLCqueen.colors = this.DLCqueen.colors with
      {
        normalColor = color
      };
      this.DLCqueenLineOk.gameObject.SetActive(false);
      this.DLCqueenLineKo.gameObject.SetActive(true);
    }
    else
    {
      this.DLCqueenLineOk.gameObject.SetActive(true);
      this.DLCqueenLineKo.gameObject.SetActive(false);
    }
    if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[4]))
    {
      this.DLCuprising.colors = this.DLCuprising.colors with
      {
        normalColor = color
      };
      this.DLCuprisingLineOk.gameObject.SetActive(false);
      this.DLCuprisingLineKo.gameObject.SetActive(true);
    }
    else
    {
      this.DLCuprisingLineOk.gameObject.SetActive(true);
      this.DLCuprisingLineKo.gameObject.SetActive(false);
    }
    if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[5]))
    {
      this.DLCnenukil.colors = this.DLCnenukil.colors with
      {
        normalColor = color
      };
      this.DLCnenukilLineOk.gameObject.SetActive(false);
      this.DLCnenukilLineKo.gameObject.SetActive(true);
    }
    else
    {
      this.DLCnenukilLineOk.gameObject.SetActive(true);
      this.DLCnenukilLineKo.gameObject.SetActive(false);
    }
    if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[6]))
    {
      this.DLCsahti.colors = this.DLCsahti.colors with
      {
        normalColor = color
      };
      this.DLCsahtiLineOk.gameObject.SetActive(false);
      this.DLCsahtiLineKo.gameObject.SetActive(true);
    }
    else
    {
      this.DLCsahtiLineOk.gameObject.SetActive(true);
      this.DLCsahtiLineKo.gameObject.SetActive(false);
    }
    if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[7]))
    {
      this.DLCsigrun.colors = this.DLCsigrun.colors with
      {
        normalColor = color
      };
      this.DLCsigrunLineOk.gameObject.SetActive(false);
      this.DLCsigrunLineKo.gameObject.SetActive(true);
    }
    else
    {
      this.DLCsigrunLineOk.gameObject.SetActive(true);
      this.DLCsigrunLineKo.gameObject.SetActive(false);
    }
    if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[8]))
    {
      this.DLCbernard.colors = this.DLCbernard.colors with
      {
        normalColor = color
      };
      this.DLCbernardLineOk.gameObject.SetActive(false);
      this.DLCbernardLineKo.gameObject.SetActive(true);
    }
    else
    {
      this.DLCbernardLineOk.gameObject.SetActive(true);
      this.DLCbernardLineKo.gameObject.SetActive(false);
    }
    if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[9]))
    {
      this.DLCtemple.colors = this.DLCtemple.colors with
      {
        normalColor = color
      };
      this.DLCtempleLineOk.gameObject.SetActive(false);
      this.DLCtempleLineKo.gameObject.SetActive(true);
    }
    else
    {
      this.DLCtempleLineOk.gameObject.SetActive(true);
      this.DLCtempleLineKo.gameObject.SetActive(false);
    }
    if (!SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[10]))
    {
      this.DLCspider.colors = this.DLCspider.colors with
      {
        normalColor = color
      };
      this.DLCspiderLineOk.gameObject.SetActive(false);
      this.DLCspiderLineKo.gameObject.SetActive(true);
    }
    else
    {
      this.DLCspiderLineOk.gameObject.SetActive(true);
      this.DLCspiderLineKo.gameObject.SetActive(false);
    }
  }

  public void ShowDLCPopup(int _index)
  {
    this.DLCPopupShow(true);
    this.activePopup = _index;
    this.DLCpopupImageSkins.gameObject.SetActive(false);
    this.DLCpopupImageWW.gameObject.SetActive(false);
    this.DLCpopupImageUlminin.gameObject.SetActive(false);
    this.DLCpopupImageAmelia.gameObject.SetActive(false);
    this.DLCpopupImageObsidian.gameObject.SetActive(false);
    this.DLCpopupImageNenukil.gameObject.SetActive(false);
    this.DLCpopupImageSahti.gameObject.SetActive(false);
    this.DLCpopupImageSigrun.gameObject.SetActive(false);
    this.DLCpopupImageBernard.gameObject.SetActive(false);
    this.DLCpopupImageTemple.gameObject.SetActive(false);
    this.DLCpopupImageSpider.gameObject.SetActive(false);
    this.DLCpopupTitle.text = SteamManager.Instance.GetDLCName(Globals.Instance.SkuAvailable[this.activePopup]);
    if (this.activePopup == 0)
    {
      this.DLCpopupDescription.text = Texts.Instance.GetText("howToDLCSkin");
      this.DLCpopupImageSkins.gameObject.SetActive(true);
    }
    else if (this.activePopup == 1)
    {
      this.DLCpopupDescription.text = Texts.Instance.GetText("howToDLCWW");
      this.DLCpopupImageWW.gameObject.SetActive(true);
    }
    else if (this.activePopup == 2)
    {
      this.DLCpopupDescription.text = Texts.Instance.GetText("howToDLCUlminin");
      this.DLCpopupImageUlminin.gameObject.SetActive(true);
    }
    else if (this.activePopup == 3)
    {
      this.DLCpopupDescription.text = Texts.Instance.GetText("howToDLCCharacter");
      TMP_Text cpopupDescription = this.DLCpopupDescription;
      cpopupDescription.text = cpopupDescription.text + "<br><br>" + Texts.Instance.GetText("howToDLCPet");
      this.DLCpopupImageAmelia.gameObject.SetActive(true);
    }
    else if (this.activePopup == 4)
    {
      this.DLCpopupDescription.text = Texts.Instance.GetText("howToDLCObsidian");
      this.DLCpopupImageObsidian.gameObject.SetActive(true);
    }
    else if (this.activePopup == 5)
    {
      this.DLCpopupDescription.text = Texts.Instance.GetText("howToDLCCharacter");
      TMP_Text cpopupDescription = this.DLCpopupDescription;
      cpopupDescription.text = cpopupDescription.text + "<br><br>" + Texts.Instance.GetText("howToDLCPet");
      this.DLCpopupImageNenukil.gameObject.SetActive(true);
    }
    else if (this.activePopup == 6)
    {
      this.DLCpopupDescription.text = Texts.Instance.GetText("howToDLCSahti");
      this.DLCpopupImageSahti.gameObject.SetActive(true);
    }
    else if (this.activePopup == 7)
    {
      this.DLCpopupDescription.text = Texts.Instance.GetText("howToDLCCharacter");
      TMP_Text cpopupDescription = this.DLCpopupDescription;
      cpopupDescription.text = cpopupDescription.text + "<br><br>" + Texts.Instance.GetText("howToDLCPet");
      this.DLCpopupImageSigrun.gameObject.SetActive(true);
    }
    else if (this.activePopup == 8)
    {
      this.DLCpopupDescription.text = Texts.Instance.GetText("howToDLCCharacter");
      TMP_Text cpopupDescription = this.DLCpopupDescription;
      cpopupDescription.text = cpopupDescription.text + "<br><br>" + Texts.Instance.GetText("howToDLCPet");
      this.DLCpopupImageBernard.gameObject.SetActive(true);
    }
    else if (this.activePopup == 9)
    {
      this.DLCpopupDescription.text = Texts.Instance.GetText("howToDLCTemple");
      this.DLCpopupImageTemple.gameObject.SetActive(true);
    }
    else
    {
      if (this.activePopup != 10)
        return;
      this.DLCpopupDescription.text = Texts.Instance.GetText("howToDLCCharacter");
      TMP_Text cpopupDescription = this.DLCpopupDescription;
      cpopupDescription.text = cpopupDescription.text + "<br><br>" + Texts.Instance.GetText("howToDLCPet");
      this.DLCpopupImageSpider.gameObject.SetActive(true);
    }
  }

  public void HideDLCPopup() => this.DLCPopupShow(false);

  public void ButtonDLCPopupLink()
  {
    Application.OpenURL("https://store.steampowered.com/app/" + Globals.Instance.SkuAvailable[this.activePopup]);
  }

  public void LoadProfiles() => this.profiles = SaveManager.GetProfileNames();

  private void SetMenuCurrentProfile()
  {
    this.profileMenuText.text = string.Format(Texts.Instance.GetText("profileMenu"), (object) this.profiles[GameManager.Instance.ProfileId]);
  }

  private void HideGameModeSelection()
  {
    if (this.weeklyReward.gameObject.activeSelf)
      this.weeklyReward.gameObject.SetActive(false);
    this.SelectGameMode(-1);
    this.gameModeSelectionMode.gameObject.SetActive(false);
    this.gameModeSelectionChoose.gameObject.SetActive(false);
    this.gameModeSelectionDescription.gameObject.SetActive(false);
    this.gameModeSelectionDescriptionLarge.gameObject.SetActive(false);
    this.gameModeSelectionT.gameObject.SetActive(false);
    this.gameModeSelection0.gameObject.SetActive(false);
    this.gameModeSelection1.gameObject.SetActive(false);
    this.gameModeSelection2.gameObject.SetActive(false);
    this.gameModeSelection3.gameObject.SetActive(false);
    this.joinT.gameObject.SetActive(false);
    this.exitT.gameObject.SetActive(false);
  }

  private void MenuControllerHoverOff()
  {
    for (int index = 0; index < this.menuController0.Count; ++index)
    {
      if ((bool) (UnityEngine.Object) this.menuController0[index].GetComponent<MenuButton>())
        this.menuController0[index].GetComponent<MenuButton>().HoverOff();
    }
    this.exitT.GetChild(0).GetComponent<MenuButton>().HoverOff();
  }

  private void ShowGameModeSelection(int _type)
  {
    GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
    this.MenuControllerHoverOff();
    this.challengeLocked = PlayerManager.Instance.GetHighestCharacterRank() >= 3 ? (this.weeklyLocked = false) : (this.weeklyLocked = true);
    this.singularityLocked = !GameManager.Instance.CheatMode && GameManager.Instance.UnlockMadness || PlayerManager.Instance.GetHighestCharacterRank() < 3;
    this.LoadSaveGames();
    this.ShowMask(true, 0.8f);
    this.profilesT.gameObject.SetActive(false);
    this.menuT.gameObject.SetActive(false);
    this.bannerObject.gameObject.SetActive(false);
    this.ShowHideDLCInformation(false);
    this.HideGameModeSelection();
    this.gameModeSelection0.GetChild(0).GetComponent<BotonMenuGameMode>().TurnOffState();
    this.gameModeSelection1.GetChild(0).GetComponent<BotonMenuGameMode>().TurnOffState();
    this.gameModeSelection2.GetChild(0).GetComponent<BotonMenuGameMode>().TurnOffState();
    this.gameModeSelection3.GetChild(0).GetComponent<BotonMenuGameMode>().TurnOffState();
    switch (_type)
    {
      case 0:
        this.gameModeType.GetComponent<TMP_Text>().text = Texts.Instance.GetText("mainMenuSaveSingle");
        this.gameModeSelectionMode.GetComponent<TMP_Text>().text = Texts.Instance.GetText("mainMenuSaveSingle");
        break;
      case 1:
        this.gameModeType.GetComponent<TMP_Text>().text = Texts.Instance.GetText("mainMenuSaveMulti");
        this.gameModeSelectionMode.GetComponent<TMP_Text>().text = Texts.Instance.GetText("mainMenuSaveMulti");
        this.joinT.gameObject.SetActive(true);
        break;
    }
    this.exitT.gameObject.SetActive(true);
    this.gameModeType.gameObject.SetActive(true);
    this.gameModeChoose.gameObject.SetActive(true);
    if (this.gameModeSelectionCoroutine != null)
      this.StopCoroutine(this.gameModeSelectionCoroutine);
    this.gameModeSelectionCoroutine = this.StartCoroutine(this.ShowGameModeSelectionCo());
  }

  private void SelectGameMode(int _index)
  {
    if (this.gameModeSelectionCoroutine != null)
      this.StopCoroutine(this.gameModeSelectionCoroutine);
    this.gameModeWeekly.text = "";
    this.setWeekly = false;
    this.gameModeType.gameObject.SetActive(false);
    this.gameModeChoose.gameObject.SetActive(false);
    this.gameModeSelectionMode.gameObject.SetActive(true);
    this.gameModeSelectionChoose.gameObject.SetActive(true);
    this.gameModeSelection0.gameObject.SetActive(true);
    this.gameModeSelection1.gameObject.SetActive(true);
    this.gameModeSelection2.gameObject.SetActive(true);
    this.gameModeSelection3.gameObject.SetActive(true);
    if (_index == 0 || _index == 1)
    {
      GameManager.Instance.GameType = global::Enums.GameType.Adventure;
      this.gameModeSelection0.localPosition = this.vectorPositionGameMode;
      this.gameModeSelection0.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
      this.StartCoroutine(this.BotonMenuGameModeStateOn(0));
      this.gameModeSelectionChoose.GetComponent<TMP_Text>().text = Texts.Instance.GetText("modeAdventure");
      this.gameModeSelectionDescriptionLarge.gameObject.SetActive(true);
      this.gameModeSelectionDescriptionLarge.GetComponent<TMP_Text>().text = Texts.Instance.GetText("mainMenuAdventureDescription");
    }
    else if (_index > -1)
    {
      this.gameModeSelection0.gameObject.SetActive(false);
    }
    else
    {
      this.gameModeSelection0.localPosition = new Vector3(-10.5f, 0.0f, 0.0f);
      this.gameModeSelection0.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
    }
    if (_index == 2 || _index == 3)
    {
      GameManager.Instance.GameType = global::Enums.GameType.Challenge;
      this.gameModeSelection1.localPosition = this.vectorPositionGameMode;
      this.gameModeSelection1.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
      this.StartCoroutine(this.BotonMenuGameModeStateOn(1));
      this.gameModeSelectionChoose.GetComponent<TMP_Text>().text = Texts.Instance.GetText("modeObelisk");
      this.gameModeSelectionDescriptionLarge.gameObject.SetActive(true);
      this.gameModeSelectionDescriptionLarge.GetComponent<TMP_Text>().text = Texts.Instance.GetText("mainMenuObeliskDescription");
    }
    else if (_index > -1)
    {
      this.gameModeSelection1.gameObject.SetActive(false);
    }
    else
    {
      this.gameModeSelection1.localPosition = new Vector3(-3.5f, 0.0f, 0.0f);
      this.gameModeSelection1.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
    }
    if (_index == 4 || _index == 5)
    {
      GameManager.Instance.GameType = global::Enums.GameType.WeeklyChallenge;
      this.gameModeSelection2.localPosition = this.vectorPositionGameMode;
      this.gameModeSelection2.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
      this.StartCoroutine(this.BotonMenuGameModeStateOn(2));
      this.gameModeSelectionChoose.GetComponent<TMP_Text>().text = Texts.Instance.GetText("modeWeekly");
      this.gameModeSelectionDescription.gameObject.SetActive(true);
      this.gameModeSelectionDescription.GetComponent<TMP_Text>().text = Texts.Instance.GetText("mainMenuWeeklyDescription");
      this.SetWeeklyLeft();
      this.setWeekly = true;
      ChallengeData weeklyData1 = Globals.Instance.GetWeeklyData(Functions.GetCurrentWeeklyWeek());
      if ((UnityEngine.Object) weeklyData1 != (UnityEngine.Object) null)
      {
        CardbackData cardbackData1 = weeklyData1.GetCardbackData();
        if ((UnityEngine.Object) cardbackData1 != (UnityEngine.Object) null)
        {
          this.weeklyReward.gameObject.SetActive(true);
          this.weeklyRewardCardback.sprite = cardbackData1.CardbackSprite;
        }
        ChallengeData weeklyData2 = Globals.Instance.GetWeeklyData(Functions.GetCurrentWeeklyWeek(), true);
        this.weeklyRewardCardbackSecondary.gameObject.SetActive(false);
        if ((UnityEngine.Object) weeklyData2 != (UnityEngine.Object) null)
        {
          CardbackData cardbackData2 = weeklyData2.GetCardbackData();
          if ((UnityEngine.Object) cardbackData2 != (UnityEngine.Object) null)
          {
            this.weeklyRewardCardbackSecondary.sprite = cardbackData2.CardbackSprite;
            this.weeklyRewardCardbackSecondary.gameObject.SetActive(true);
          }
        }
      }
    }
    else if (_index > -1)
    {
      this.gameModeSelection2.gameObject.SetActive(false);
    }
    else
    {
      this.gameModeSelection2.localPosition = new Vector3(3.5f, 0.0f, 0.0f);
      this.gameModeSelection2.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
    }
    if (_index == 6 || _index == 7)
    {
      GameManager.Instance.GameType = global::Enums.GameType.Singularity;
      this.gameModeSelection3.localPosition = this.vectorPositionGameMode;
      this.gameModeSelection3.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
      this.StartCoroutine(this.BotonMenuGameModeStateOn(3));
      this.gameModeSelectionChoose.GetComponent<TMP_Text>().text = Texts.Instance.GetText("singularity");
      this.gameModeSelectionDescriptionLarge.gameObject.SetActive(true);
      this.gameModeSelectionDescriptionLarge.GetComponent<TMP_Text>().text = Texts.Instance.GetText("mainMenuSingularityDescription");
    }
    else if (_index > -1)
    {
      this.gameModeSelection3.gameObject.SetActive(false);
    }
    else
    {
      this.gameModeSelection3.localPosition = new Vector3(10.5f, 0.0f, 0.0f);
      this.gameModeSelection3.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
    }
  }

  private IEnumerator BotonMenuGameModeStateOn(int _option)
  {
    yield return (object) null;
    switch (_option)
    {
      case 0:
        this.gameModeSelection0.GetChild(0).GetComponent<BotonMenuGameMode>().TurnOnState();
        break;
      case 1:
        this.gameModeSelection1.GetChild(0).GetComponent<BotonMenuGameMode>().TurnOnState();
        break;
      case 2:
        this.gameModeSelection2.GetChild(0).GetComponent<BotonMenuGameMode>().TurnOnState();
        break;
      case 3:
        this.gameModeSelection3.GetChild(0).GetComponent<BotonMenuGameMode>().TurnOnState();
        break;
    }
  }

  private IEnumerator ShowGameModeSelectionCo()
  {
    this.gameModeSelectionT.gameObject.SetActive(true);
    if (this.challengeLocked)
      this.gameModeObeliskChains.gameObject.SetActive(true);
    else
      this.gameModeObeliskChains.gameObject.SetActive(false);
    if (this.weeklyLocked)
      this.gameModeWeeklyChains.gameObject.SetActive(true);
    else
      this.gameModeWeeklyChains.gameObject.SetActive(false);
    if (this.singularityLocked)
      this.gameModeSingularityChains.gameObject.SetActive(true);
    else
      this.gameModeSingularityChains.gameObject.SetActive(false);
    this.gameModeSelection0.gameObject.SetActive(true);
    yield return (object) Globals.Instance.WaitForSeconds(0.1f);
    this.gameModeSelection1.gameObject.SetActive(true);
    yield return (object) Globals.Instance.WaitForSeconds(0.1f);
    this.gameModeSelection2.gameObject.SetActive(true);
    yield return (object) Globals.Instance.WaitForSeconds(0.1f);
    this.gameModeSelection3.gameObject.SetActive(true);
  }

  public void GoToSteamPage()
  {
    Application.OpenURL("https://store.steampowered.com/app/1385380/Across_the_Obelisk/");
  }

  public void GoToLeaderboard()
  {
    Application.OpenURL("https://steamcommunity.com/stats/1421400/leaderboards/6109803");
  }

  public void GoToDiscord() => GameManager.Instance.Discord();

  private void LoadSaveGames() => this.saveGames = SaveManager.SaveGamesList();

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
    for (int index = 0; index < 3; ++index)
    {
      if ((UnityEngine.Object) this.menuSaveButtons[index] != (UnityEngine.Object) null)
      {
        this.menuSaveButtons[index].HoverOff();
        this.menuSaveButtons[index].SetSlot(num + index);
        if (this.saveGames[num + index] != null)
        {
          this.menuSaveButtons[index].SetActive(true);
          this.menuSaveButtons[index].SetGameData(this.saveGames[num + index]);
        }
        else
          this.menuSaveButtons[index].SetActive(false);
      }
    }
  }

  public void Resize()
  {
    this.PositionMenuItems();
    if ((double) Globals.Instance.scale < 1.0)
      this.relatedScaler.matchWidthOrHeight = 1f;
    else
      this.relatedScaler.matchWidthOrHeight = 0.0f;
  }

  private void PositionMenuItems()
  {
    float num1 = 50f * Globals.Instance.multiplierY * (float) (1920.0 * (double) Screen.height / (1080.0 * (double) Screen.width));
    float num2 = (float) ((double) num1 * 2.0 - 610.0);
    int num3 = 0;
    for (int index = this.menuOps.Length - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) this.menuOps[index] != (UnityEngine.Object) null)
        this.menuOps[index].transform.localPosition = new Vector3(this.menuOps[index].transform.localPosition.x, num2 + (float) num3 * num1);
      ++num3;
    }
    int num4 = 0;
    float num5 = this.menuOps[this.menuOps.Length - 3].transform.localPosition.y - num1;
    this.profileDelete.transform.localPosition = new Vector3(this.profileDelete.transform.localPosition.x, num5 - 2f * num1);
    for (int index = this.profileOps.Length - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) this.profileOps[index] != (UnityEngine.Object) null)
        this.profileOps[index].transform.localPosition = new Vector3(this.profileOps[index].transform.localPosition.x, num5 + (float) num4 * num1);
      ++num4;
    }
  }

  private void ClearButtonsState()
  {
    for (int index = 0; index < this.menuOps.Length; ++index)
    {
      if ((UnityEngine.Object) this.menuOps[index] != (UnityEngine.Object) null)
      {
        Button component = this.menuOps[index].GetComponent<Button>();
        component.interactable = false;
        component.interactable = true;
      }
    }
  }

  public bool IsSaveMenuActive() => this.saveT.gameObject.activeSelf;

  public bool IsGameModesActive() => this.gameModeSelectionT.gameObject.activeSelf;

  public void ShowCredits()
  {
    GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
    this.MenuControllerHoverOff();
    this.ShowMask(true, 0.8f);
    this.profilesT.gameObject.SetActive(false);
    this.menuT.gameObject.SetActive(false);
    this.bannerObject.gameObject.SetActive(false);
    this.credits.gameObject.SetActive(true);
    this.exitT.gameObject.SetActive(true);
    this.ClearButtonsState();
    if (this.creditsCo != null)
      this.StopCoroutine(this.creditsCo);
    this.creditsCo = this.StartCoroutine(this.CreditAutoMovement());
  }

  public void HideCredits()
  {
    if (this.credits.gameObject.activeSelf)
    {
      this.credits.gameObject.SetActive(false);
      this.exitT.gameObject.SetActive(false);
    }
    if (this.creditsCo == null)
      return;
    this.StopCoroutine(this.creditsCo);
  }

  private IEnumerator CreditAutoMovement()
  {
    this.creditScrollRect.verticalNormalizedPosition = 1f;
    yield return (object) null;
    float creditsContentHeight = this.creditScrollRect.content.rect.height;
    while (true)
    {
      if (!Mouse.current.leftButton.isPressed && (double) this.creditScrollRect.verticalNormalizedPosition > 0.0)
        this.creditScrollRect.verticalNormalizedPosition = Mathf.Clamp01(this.creditScrollRect.verticalNormalizedPosition - this.creditScrollSpeed / creditsContentHeight * Time.deltaTime);
      yield return (object) null;
    }
  }

  private void MoveCredits(bool goDown)
  {
    if (goDown)
      this.creditScrollRect.verticalNormalizedPosition -= 0.01f;
    else
      this.creditScrollRect.verticalNormalizedPosition += 0.01f;
  }

  public void ShowSaveGame(bool status, int section = -1)
  {
    if (this.challengeLocked && section == 2 || this.weeklyLocked && section == 4 || this.singularityLocked && section == 6)
      return;
    this.ShowMask(status, 0.8f);
    this.profilesT.gameObject.SetActive(false);
    this.saveT.gameObject.SetActive(status);
    this.menuT.gameObject.SetActive(!status);
    this.HideCredits();
    this.bannerObject.gameObject.SetActive(!status);
    if (status)
    {
      this.saveSlots.gameObject.SetActive(true);
      if (!this.singlePlayer)
        ++section;
      this.PositionSaveItems(section);
      this.SelectGameMode(section);
    }
    else
      this.SetMenuCurrentProfile();
  }

  public void NewGame()
  {
    GameManager.Instance.GameType = global::Enums.GameType.Adventure;
    this.singlePlayer = true;
    this.ShowGameModeSelection(0);
  }

  public void LoadGame()
  {
    GameManager.Instance.GameType = global::Enums.GameType.Adventure;
    this.ShowSaveGame(true, 0);
    this.ClearButtonsState();
  }

  public void ExitSaveGame(bool _forceIt = false)
  {
    if (!_forceIt && AlertManager.Instance.IsActive())
      return;
    GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
    if (NetworkManager.Instance.IsConnected())
      NetworkManager.Instance.Disconnect();
    this.setWeekly = false;
    this.HideGameModeSelection();
    this.LoadSaveGames();
    this.ShowSaveGame(false);
  }

  public void ObeliskChallenge()
  {
    GameManager.Instance.GameType = global::Enums.GameType.Challenge;
    this.ShowSaveGame(true, 2);
    this.ClearButtonsState();
    this.challengeTitle.text = Texts.Instance.GetText("menuChallenge");
  }

  public void ObeliskChallengeSP()
  {
    this.ShowSaveGame(true, 2);
    this.ClearButtonsState();
  }

  public void ObeliskChallengeMP()
  {
    this.ShowSaveGame(true, 3);
    this.ClearButtonsState();
  }

  public void WeeklyChallenge()
  {
    GameManager.Instance.GameType = global::Enums.GameType.WeeklyChallenge;
    this.ShowSaveGame(true, 2);
    this.ClearButtonsState();
    this.SetWeeklyLeft();
    this.setWeekly = true;
  }

  public void Multiplayer()
  {
    GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
    this.ShowGameModeSelection(1);
    this.singlePlayer = false;
    GameManager.Instance.GameType = global::Enums.GameType.Adventure;
  }

  public void JoinMultiplayer()
  {
    GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
    GameManager.Instance.GameType = global::Enums.GameType.Adventure;
    SceneStatic.LoadByName("Lobby");
  }

  public void TomeOfKnowledge()
  {
    GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
    SceneStatic.LoadByName(nameof (TomeOfKnowledge));
    this.ClearButtonsState();
  }

  public void Statistics() => this.ClearButtonsState();

  public void Settings()
  {
    GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
    SettingsManager.Instance.ShowSettings(true);
    this.ClearButtonsState();
  }

  public void Credits()
  {
    this.ShowCredits();
    this.ClearButtonsState();
  }

  public void QuitGame()
  {
    GameManager.Instance.QuitGame();
    this.ClearButtonsState();
  }

  public void ShowProfiles()
  {
    this.LoadProfiles();
    this.profilesT.gameObject.SetActive(true);
    this.menuT.gameObject.SetActive(false);
    this.exitT.gameObject.SetActive(true);
    string str = "";
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = 0; index < this.profileOps.Length; ++index)
    {
      stringBuilder.Clear();
      if (index == GameManager.Instance.ProfileId)
      {
        stringBuilder.Append("<size=+3><b><sprite name=experience><color=orange>");
        str = this.profiles[index];
      }
      if (this.profiles[index] == "")
      {
        stringBuilder.Append("<size=-4><color=#B0B0B0>");
        stringBuilder.Append(Texts.Instance.GetText("profileCreate"));
      }
      else
        stringBuilder.Append(this.profiles[index]);
      this.profileOpsText[index].text = stringBuilder.ToString();
    }
    if (GameManager.Instance.ProfileId == 0 || str == "")
    {
      if (!this.profileDelete.gameObject.activeSelf)
        return;
      this.profileDelete.gameObject.SetActive(false);
    }
    else
    {
      stringBuilder.Clear();
      stringBuilder.Append("<color=#B0B0B0>");
      stringBuilder.Append(str);
      stringBuilder.Append("</color>");
      this.profileDeleteText.text = string.Format(Texts.Instance.GetText("deleteProfile"), (object) stringBuilder.ToString());
      if (this.profileDelete.gameObject.activeSelf)
        return;
      this.profileDelete.gameObject.SetActive(true);
    }
  }

  public void DeleteProfile()
  {
    if (GameManager.Instance.ProfileId <= 0 || !(this.profiles[GameManager.Instance.ProfileId] != ""))
      return;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(string.Format(Texts.Instance.GetText("doYouWantToDeleteProfile"), (object) this.profiles[GameManager.Instance.ProfileId]));
    stringBuilder.Append("<br><size=-4><color=#AAAAAA>");
    stringBuilder.Append(Texts.Instance.GetText("wantToRemoveSavePermanent"));
    stringBuilder.Append("</color></size>");
    AlertManager.buttonClickDelegate = new AlertManager.OnButtonClickDelegate(this.DeleteProfileAction);
    AlertManager.Instance.AlertConfirmDouble(stringBuilder.ToString());
  }

  private void DeleteProfileAction()
  {
    AlertManager.buttonClickDelegate -= new AlertManager.OnButtonClickDelegate(this.DeleteProfileAction);
    if (!AlertManager.Instance.GetConfirmAnswer())
      return;
    SaveManager.DeleteProfileFolder(GameManager.Instance.ProfileId);
    this.UseProfile(0);
  }

  public void UseProfile(int _slot)
  {
    if (_slot == GameManager.Instance.ProfileId)
    {
      this.ExitSaveGame();
    }
    else
    {
      SaveManager.RemoveGameContinue();
      if (this.profiles[_slot] == "")
      {
        this.CreateProfile(_slot);
      }
      else
      {
        GameManager.Instance.UseProfileFile(_slot);
        GameManager.Instance.ReloadProfile();
        this.ExitSaveGame(true);
      }
    }
  }

  private void CreateProfile(int _slot)
  {
    if (_slot == 0)
      return;
    this.profileCreateSlot = _slot;
    AlertManager.buttonClickDelegate = new AlertManager.OnButtonClickDelegate(this.CreateProfileAction);
    AlertManager.Instance.AlertInput(Texts.Instance.GetText("inputConfigSaveName"), Texts.Instance.GetText("accept").ToUpper());
  }

  public void CreateProfileAction()
  {
    AlertManager.buttonClickDelegate -= new AlertManager.OnButtonClickDelegate(this.CreateProfileAction);
    string _name = Functions.OnlyAscii(AlertManager.Instance.GetInputValue()).ToUpper().Trim();
    if (_name == "")
      return;
    SaveManager.CreateProfileFolder(this.profileCreateSlot, _name);
  }

  private void ShowMask(bool state, float alpha = 1f)
  {
    if (this.maskCoroutine != null)
      this.StopCoroutine(this.maskCoroutine);
    if (state)
      this.maskCoroutine = this.StartCoroutine(this.ShowMaskCo(alpha));
    else
      this.maskCoroutine = this.StartCoroutine(this.HideMaskCo());
  }

  private IEnumerator ShowMaskCo(float alpha = 1f)
  {
    float index = this.maskImage.color.a;
    while ((double) index < (double) alpha)
    {
      this.maskImage.color = new Color(0.0f, 0.0f, 0.0f, index);
      index += 0.025f;
      yield return (object) null;
    }
    this.maskImage.color = new Color(0.0f, 0.0f, 0.0f, alpha);
  }

  private IEnumerator HideMaskCo()
  {
    float index = this.maskImage.color.a;
    while ((double) index > 0.0)
    {
      this.maskImage.color = new Color(0.0f, 0.0f, 0.0f, index);
      index -= 0.025f;
      yield return (object) null;
    }
  }

  private void SetWeeklyLeft()
  {
    TimeSpan timeSpan = Functions.TimeSpanLeftWeekly();
    string str = string.Format("{0:D2}h. {1:D2}m. {2:D2}s.", (object) (int) timeSpan.TotalHours, (object) timeSpan.Minutes, (object) timeSpan.Seconds);
    this.SBWeekly.Clear();
    this.SBWeekly.Append("<b><size=+2>");
    if (this.weeklyName.IsNullOrEmpty())
      this.weeklyName = AtOManager.Instance.GetWeeklyName(Functions.GetCurrentWeeklyWeek());
    this.SBWeekly.Append(this.weeklyName);
    this.SBWeekly.Append("</size></b>\n");
    this.SBWeekly.Append(str);
    this.gameModeWeekly.text = this.SBWeekly.ToString();
  }

  public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false)
  {
    bool flag1 = false;
    this.controllerList.Clear();
    if (Functions.TransformIsVisible(this.credits))
    {
      if (goingUp)
        this.MoveCredits(false);
      else if (goingDown)
      {
        this.MoveCredits(true);
      }
      else
      {
        this.warpPosition = (Vector2) this.exitT.GetChild(0).position;
        Mouse.current.WarpCursorPosition(this.warpPosition);
      }
    }
    else
    {
      if (this.DLCpopup.gameObject.activeSelf)
      {
        this.controllerList.Add(this.DLCLinkButton);
        this.controllerList.Add(this.DLCExitButton);
      }
      else if (!this.paradoxDocumentPopup.gameObject.activeSelf)
      {
        if (this.IsSaveMenuActive() || this.IsGameModesActive())
        {
          for (int index = 0; index < this.menuController1.Count; ++index)
            this.controllerList.Add(this.menuController1[index]);
          if (this.IsSaveMenuActive())
          {
            for (int index = 0; index < this.menuControllerSaveGames.Count; ++index)
              this.controllerList.Add(this.menuControllerSaveGames[index]);
          }
          else if (this.IsGameModesActive())
          {
            for (int index = 0; index < this.menuControllerModeSelection.Count; ++index)
              this.controllerList.Add(this.menuControllerModeSelection[index]);
          }
        }
        else if (this.profilesT.gameObject.activeSelf)
        {
          for (int index = 0; index < this.menuControllerProfiler.Count; ++index)
            this.controllerList.Add(this.menuControllerProfiler[index]);
        }
        else
        {
          flag1 = true;
          for (int index = 0; index < this.menuController0.Count; ++index)
            this.controllerList.Add(this.menuController0[index]);
          if (this.DLCT.gameObject.activeSelf)
          {
            foreach (Transform transform in this.DLCT.transform)
              this.controllerList.Add(transform);
          }
        }
        for (int index = 0; index < this.menuControllerPDX.Count; ++index)
          this.controllerList.Add(this.menuControllerPDX[index]);
        for (int index = this.controllerList.Count - 1; index >= 0; --index)
        {
          if (!Functions.TransformIsVisible(this.controllerList[index]))
            this.controllerList.RemoveAt(index);
        }
      }
      else
        this.controllerList.Add(this.paradoxDocumentCloseButton);
      this.controllerHorizontalIndex = Functions.GetListClosestIndexToMousePosition(this.controllerList, true);
      bool flag2 = false;
      if (flag1 && (goingDown && this.controllerHorizontalIndex == 5 || goingUp && this.controllerHorizontalIndex == 7))
      {
        this.controllerHorizontalIndex = 6;
        flag2 = true;
      }
      if (!flag2)
        this.controllerHorizontalIndex = Functions.GetClosestIndexBasedOnDirection(this.controllerList, this.controllerHorizontalIndex, goingUp, goingRight, goingDown, goingLeft, true);
      if (!((UnityEngine.Object) this.controllerList[this.controllerHorizontalIndex] != (UnityEngine.Object) null))
        return;
      this.warpPosition = (Vector2) this.controllerList[this.controllerHorizontalIndex].position;
      Mouse.current.WarpCursorPosition(this.warpPosition);
    }
  }

  public void CreatePDXDropdowns()
  {
    this.paradoxDropdownRegion.options.Clear();
    List<string> options = new List<string>()
    {
      Texts.Instance.GetText("Region")
    };
    this.regionDictionary = new SortedDictionary<string, string>();
    foreach (string name in Enum.GetNames(typeof (Country)))
    {
      if (name != "Undefined")
      {
        if (Globals.Instance.IsoToEnglishCountryNames.ContainsKey(name))
          this.regionDictionary.Add(Globals.Instance.IsoToEnglishCountryNames[name], name);
        else
          this.regionDictionary.Add(name, name);
      }
    }
    foreach (KeyValuePair<string, string> region in this.regionDictionary)
      options.Add(region.Key);
    this.paradoxDropdownRegion.AddOptions(options);
    this.paradoxDropdownDay.options.Clear();
    options.Clear();
    options.Add(Texts.Instance.GetText("Day"));
    for (int index = 0; index < 31; ++index)
      options.Add((index + 1).ToString());
    this.paradoxDropdownDay.AddOptions(options);
    this.paradoxDropdownMonth.options.Clear();
    options.Clear();
    options.Add(Texts.Instance.GetText("Month"));
    for (int index = 0; index < 12; ++index)
      options.Add(Texts.Instance.GetText("month" + (index + 1).ToString()));
    this.paradoxDropdownMonth.AddOptions(options);
    this.paradoxDropdownYear.options.Clear();
    options.Clear();
    options.Add(Texts.Instance.GetText("Year"));
    DateTime now = DateTime.Now;
    int year = now.Year;
    while (true)
    {
      int num1 = year;
      now = DateTime.Now;
      int num2 = now.Year - 100;
      if (num1 >= num2)
      {
        options.Add(year.ToString());
        --year;
      }
      else
        break;
    }
    this.paradoxDropdownYear.AddOptions(options);
  }

  public void ResetPDXScreens()
  {
    if (this.paradoxLoginPopup.gameObject.activeSelf)
      this.paradoxLoginPopup.gameObject.SetActive(false);
    if (this.paradoxLoginPrePopup.gameObject.activeSelf)
      this.paradoxLoginPrePopup.gameObject.SetActive(false);
    if (this.paradoxLoginFieldsPopup.gameObject.activeSelf)
      this.paradoxLoginFieldsPopup.gameObject.SetActive(false);
    if (this.paradoxLoggedPopup.gameObject.activeSelf)
      this.paradoxLoggedPopup.gameObject.SetActive(false);
    if (this.paradoxCreatePopup.gameObject.activeSelf)
      this.paradoxCreatePopup.gameObject.SetActive(false);
    this.ShowPDXMask(false);
  }

  public void ShowPDXMask(bool state)
  {
    if (this.paradoxMask.gameObject.activeSelf == state)
      return;
    this.paradoxMask.gameObject.SetActive(state);
  }

  public void PDXPreLoginButton() => this.ShowPDXLogin();

  public void ShowPDXPreLogin()
  {
    this.ResetPDXScreens();
    this.paradoxLoginPopup.GetComponent<RectTransform>().sizeDelta = this.preLoginBackgroundSize;
    if (!this.paradoxLoginPopup.gameObject.activeSelf)
      this.paradoxLoginPopup.gameObject.SetActive(true);
    if (this.paradoxLoginPrePopup.gameObject.activeSelf)
      return;
    this.paradoxLoginPrePopup.gameObject.SetActive(true);
  }

  public void ShowPDXLogin()
  {
    this.ResetPDXScreens();
    this.paradoxLoginPopup.GetComponent<RectTransform>().sizeDelta = this.loginBackgroundSize;
    if (!this.paradoxLoginPopup.gameObject.activeSelf)
      this.paradoxLoginPopup.gameObject.SetActive(true);
    if (this.paradoxLoginFieldsPopup.gameObject.activeSelf)
      return;
    this.paradoxLoginFieldsPopup.gameObject.SetActive(true);
  }

  public void ShowPDXLogged()
  {
    if (GameManager.Instance.GetDeveloperMode())
      Debug.Log((object) nameof (ShowPDXLogged));
    this.ResetPDXScreens();
    if (!this.paradoxLoggedPopup.gameObject.activeSelf)
      this.paradoxLoggedPopup.gameObject.SetActive(true);
    this.SetLoggedUsername();
  }

  private void SetLoggedUsername()
  {
    if (!string.IsNullOrEmpty(Startup.userNamespaceName))
      this.paradoxLoggedUser.text = Startup.userNamespaceName;
    else if (!string.IsNullOrEmpty(Startup.userSocialName))
      this.paradoxLoggedUser.text = Startup.userSocialName;
    else
      this.paradoxLoggedUser.text = Functions.HideEmail(Account.Email);
  }

  public void ShowPDXCreate()
  {
    this.ResetPDXScreens();
    if (this.paradoxCreatePopup.gameObject.activeSelf)
      return;
    this.paradoxCreatePopup.gameObject.SetActive(true);
  }

  public void ShowPDXDocumentFromForm(int document) => Startup.ShowDocumentFromForm(document);

  public void ShowPDXDocument() => this.StartCoroutine(this.ShowPDXDocumentCo());

  public IEnumerator ShowPDXDocumentCo()
  {
    if (!this.paradoxDocumentPopup.gameObject.activeSelf)
      this.paradoxDocumentPopup.gameObject.SetActive(true);
    yield return (object) Globals.Instance.WaitForSeconds(0.01f);
    this.paradoxDocumentScrollRect.verticalNormalizedPosition = 1f;
  }

  public void ClosePDXDocument()
  {
    EventSystem.current.SetSelectedGameObject((GameObject) null);
    if (Startup.waitingForLoginDocuments)
      Startup.ShowDocumentFromStartup();
    else
      this.paradoxDocumentPopup.gameObject.SetActive(false);
  }

  public void PDXLogin()
  {
    this.ShowPDXMask(true);
    string _email = Functions.Sanitize(this.paradoxLoginUser.text, false);
    string _password = Functions.Sanitize(this.paradoxLoginPassword.text, false);
    if (_email != "" && _password != "")
    {
      Account.SetEmailAndPasswordCredential(_email, _password);
      Account.LoginWithEmailAndPassword();
    }
    else
      this.ShowPDXError("pdxErrorLoginFields");
  }

  public void LogoutPDX()
  {
    this.ShowPDXMask(true);
    AlertManager.buttonClickDelegate = new AlertManager.OnButtonClickDelegate(this.LogoutPDXAction);
    AlertManager.Instance.AlertConfirmDouble(Texts.Instance.GetText("pdxLogoutConfirm"));
  }

  private void LogoutPDXAction()
  {
    int num = AlertManager.Instance.GetConfirmAnswer() ? 1 : 0;
    AlertManager.buttonClickDelegate -= new AlertManager.OnButtonClickDelegate(this.LogoutPDXAction);
    if (num == 0)
    {
      this.ShowPDXMask(false);
    }
    else
    {
      Account.Logout();
      Startup.userId = "";
    }
  }

  public void LinkSteamPDX()
  {
    AlertManager.buttonClickDelegate = new AlertManager.OnButtonClickDelegate(this.LinkSteamPDXAction);
    AlertManager.Instance.AlertConfirmDouble(Texts.Instance.GetText("pdxSteamLinkConfirm"));
  }

  private void LinkSteamPDXAction()
  {
    AlertManager.buttonClickDelegate -= new AlertManager.OnButtonClickDelegate(this.LinkSteamPDXAction);
  }

  public void CreatePDXAccount()
  {
    bool _marketing = false;
    string str = Functions.Sanitize(this.paradoxCreateEmail.text, false);
    string _password = Functions.Sanitize(this.paradoxCreatePassword.text, false);
    int num = this.paradoxDropdownRegion.value;
    int day = this.paradoxDropdownDay.value;
    int month = this.paradoxDropdownDay.value;
    int year = this.paradoxDropdownDay.value;
    if (this.paradoxCreateOffers.isOn)
      _marketing = true;
    string error = "";
    if (!Functions.IsValidEmail(str))
      error = "pdxErrorEmail";
    else if (_password.Length < 6 || _password.Length > 128)
      error = "pdxErrorPassword";
    else if (num == 0)
      error = "pdxErrorRegion";
    else if (day == 0 || month == 0 || year == 0)
      error = "pdxErrorBirth";
    else if (!this.IsDateDifferenceGreaterThan16Years(new DateTime(year, month, day)))
      error = "pdx16years";
    if (error == "")
    {
      string text = this.paradoxDropdownRegion.options[this.paradoxDropdownRegion.value].text;
      string _country = "";
      if (this.regionDictionary.ContainsKey(text))
        _country = this.regionDictionary[text];
      string _date = this.paradoxDropdownYear.options[this.paradoxDropdownYear.value].text + "-" + this.paradoxDropdownMonth.value.ToString("D2") + "-" + this.paradoxDropdownDay.value.ToString("D2");
      Account.SetEmailAndPasswordCredential(str, _password);
      Account.SetCountry(_country);
      Account.SetBirthdate(_date);
      Account.SetLang();
      Account.SetMarketingPermission(_marketing);
      Account.CreateParadoxAccount();
    }
    else
      this.ShowPDXError(error);
  }

  private bool IsDateDifferenceGreaterThan16Years(DateTime birthDate)
  {
    DateTime now = DateTime.Now;
    int num = Mathf.Abs(now.Year - birthDate.Year);
    if (now.Month < birthDate.Month || now.Month == birthDate.Month && now.Day < birthDate.Day)
      --num;
    return num > 16;
  }

  public void ShowPDXError(string error)
  {
    this.ShowPDXMask(false);
    AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate) null;
    AlertManager.Instance.AlertConfirm(Texts.Instance.GetText(error));
  }
}
