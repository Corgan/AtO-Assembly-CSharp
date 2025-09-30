// Decompiled with JetBrains decompiler
// Type: MatchManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using Cards;
using CustomAbilities;
using NPCs;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

#nullable disable
public class MatchManager : MonoBehaviour
{
  public Action OnLoadCombatFinished;
  public Action<Hero, CardData> OnCardCastByHeroBegins;
  public Action<Hero, CardData> OnCardCastedByHero;
  private bool checkSyncCodeInCoop = true;
  private int combatCounter;
  private Dictionary<string, string> syncCodeDict = new Dictionary<string, string>();
  private bool waitingItemTrait;
  public Dictionary<string, int> activatedTraits = new Dictionary<string, int>();
  public Dictionary<string, int> activatedTraitsRound = new Dictionary<string, int>();
  private bool beforeSyncCodeLocked;
  private string currentGameCode = "";
  private string currentGameCodeForReload = "";
  private string[] turnDataForMP;
  private List<string> eventList = new List<string>();
  private string eventListDbg = "";
  private bool heroTurn;
  public Transform sceneCamera;
  public float[] itemTimeout;
  private bool roundBegan;
  private bool gotHeroDeck;
  private bool gotNPCDeck;
  private bool gotDictionary;
  private Dictionary<string, int> cardIteration = new Dictionary<string, int>();
  private int randomForIteration;
  private int generatedCardTimes;
  [SerializeField]
  private Hero[] TeamHero;
  [SerializeField]
  private NPC[] TeamNPC;
  [SerializeField]
  private List<string>[] HeroDeck;
  [SerializeField]
  private List<string>[] HeroDeckDiscard;
  [SerializeField]
  private List<string>[] HeroDeckVanish;
  [SerializeField]
  private List<string>[] HeroHand;
  [SerializeField]
  private List<string>[] NPCDeck;
  [SerializeField]
  private List<string>[] NPCDeckDiscard;
  [SerializeField]
  private List<string>[] NPCHand;
  [SerializeField]
  private int currentRound;
  [SerializeField]
  private List<MatchManager.CharacterForOrder> CharOrder;
  [SerializeField]
  public List<SkillForLog> skillLogHeroList = new List<SkillForLog>();
  [SerializeField]
  public List<SkillForLog> skillLogNPCList = new List<SkillForLog>();
  private int handCardsBeforeCast;
  private int deckCardsBeforeCast;
  private int discardCardsBeforeCast;
  private int vanishCardsBeforeCast;
  private Dictionary<string, Transform> targetTransformDict = new Dictionary<string, Transform>();
  private Dictionary<string, bool> castingCardBlocked = new Dictionary<string, bool>();
  private int randomStringArrLength = 2000;
  private int randomIndex;
  private int randomTraitsIndex;
  private int randomItemsIndex;
  private int randomDeckIndex;
  private int randomShuffleIndex;
  private int randomMiscIndex;
  [SerializeField]
  private List<string> randomStringArr = new List<string>();
  private List<string> randomStringTraitsArr = new List<string>();
  private List<string> randomStringItemsArr = new List<string>();
  private List<string> randomStringDeckArr = new List<string>();
  private List<string> randomStringShuffleArr = new List<string>();
  private List<string> randomStringMiscArr = new List<string>();
  private bool winOnBattleFast;
  private bool combatLoading;
  private bool reloadingGame;
  public bool justCasted;
  public int heroIndexWaitingForAddDiscard = -1;
  private int[] heroLifeArr = new int[4];
  private bool turnLoadedBySave;
  private Dictionary<int, List<string>> heroBeginItems = new Dictionary<int, List<string>>();
  private Dictionary<int, Dictionary<string, string>> heroDestroyedItemsInThisTurn = new Dictionary<int, Dictionary<string, string>>();
  private List<string> teamHeroItemsFromTurnSave;
  private List<NPCState> npcParamsFromTurnSave;
  public Dictionary<string, List<string>> prePostDamageDictionary = new Dictionary<string, List<string>>();
  public int energyJustWastedByHero;
  public SideCharacters sideCharacters;
  public CharacterWindowUI characterWindow;
  private bool resignCombat;
  public Transform MaskWindow;
  public bool lockHideMask;
  public CombatTarget combatTarget;
  public Transform traitInfo;
  public TMP_Text traitInfoText;
  public Transform LoadingT;
  public Transform synchronizing;
  public Transform worldTransform;
  public Transform goTransform;
  public Transform combattextTransform;
  public List<GameObject> backgroundPrefabs;
  private GameObject backgroundPrefab;
  public Transform backgroundTransform;
  private string backgroundActive;
  private Transform backgroundSahtiAlive;
  private Transform backgroundSahtiDead;
  public GameObject riftPrefab;
  public Transform exhaustT;
  public TMP_Text exhaustNumber;
  public GameObject comicPrefab;
  private GameObject comicGO;
  public GameObject heroPrefab;
  public GameObject npcPrefab;
  public GameObject deckPileCardPrefab;
  public GameObject deckPileParticlePrefab;
  private ParticleSystem deckPileParticle;
  private ParticleSystem discardPileParticle;
  private Vector3 DeckPilePosition;
  private Vector3 DeckPileOutOfScreenPositionVector = new Vector3(6f, 0.0f, 0.0f);
  private Vector3 DiscardPilePosition;
  private int deckPileVisualState;
  private bool MovingDeckPile;
  public Transform HandMask;
  public Transform deckCounter;
  private TMP_Text deckCounterTM;
  public Transform discardCounter;
  private TMP_Text discardCounterTM;
  public Transform newCardsCounter;
  private TMP_Text newCardsCounterTM;
  public PopupSheet popupSheet;
  public Animator energyCounterAnim;
  public TMP_Text energyCounterTM;
  public SpriteRenderer energyCounterBg;
  public ParticleSystem energyCounterParticle;
  public GameObject newTurnPrefab;
  private NewTurn newTurnScript;
  public Transform roundTransform;
  public ThermometerPiece[] roundPieces;
  public Sprite[] roundThermoSprites;
  public SpriteRenderer roundThermoSprite;
  public Sprite roundThermoSpriteNull;
  public TMP_Text roundTM;
  public Dictionary<string, int> itemExecutedInTurn = new Dictionary<string, int>();
  public Dictionary<string, int> itemExecutedInCombat = new Dictionary<string, int>();
  public Dictionary<string, int> itemExecutedInCombatTurnSave = new Dictionary<string, int>();
  public Dictionary<string, int> enchantmentExecutedTotal = new Dictionary<string, int>();
  public ItemCombatIcon iconWeapon;
  public ItemCombatIcon iconArmor;
  public ItemCombatIcon iconAccesory;
  public ItemCombatIcon iconJewelry;
  public ItemCombatIcon iconPet;
  public ItemCombatIcon iconCorruption;
  public GameObject initiativePrefab;
  public GameObject initiativeRoundPrefab;
  private GameObject initiativeRoundGO;
  public GameObject skillLogPrefab;
  public GameObject skillLogBackgroundPrefab;
  public GameObject skillLogCard;
  public Transform tempTransform;
  public Transform tempVanishedTransform;
  public Transform amplifiedTransform;
  public bool amplifiedTransformShow;
  public Transform helpCharacterTransform;
  public TMP_Text helpRight;
  public AudioClip shuffleSound;
  public AudioClip discardSound;
  public AudioClip npcCardSound;
  private int hitSoundIndex;
  private bool cardDrag;
  private int cardHoverIndex;
  private CardData cardActive;
  private CardItem cardItemActive;
  private Transform cardActiveT;
  private int turnIndex;
  private int preCastNum = -1;
  private List<CardItem> cardItemTable;
  private Dictionary<string, bool> canInstaCastDict;
  private Dictionary<string, GameObject> cardGos = new Dictionary<string, GameObject>();
  private CombatData combatData;
  [SerializeField]
  private Transform targetTransform;
  private float handPosY = -4.2f;
  private int heroActive = -1;
  private int npcActive = -1;
  private List<string> activationItemsAtBeginTurnList;
  private bool isBeginTournPhase;
  private Hero theHero;
  private NPC theNPC;
  private Hero theHeroPreAutomatic;
  private NPC theNPCPreAutomatic;
  private Transform gameObjectsParentFolder;
  private GameObject GO_NewTurn;
  private GameObject GO_Initiative;
  private GameObject GO_Heroes;
  private GameObject GO_NPCs;
  private GameObject GO_Hand;
  private GameObject GO_DecksObject;
  private GameObject GO_DiscardPile;
  private GameObject GO_DeckPile;
  private GameObject[] GO_DeckPileCards;
  private Transform[] GO_DeckPileCardsT;
  public GameObject energySelectorPrefab;
  public GameObject discardSelectorPrefab;
  public GameObject addcardSelectorPrefab;
  public GameObject deckCardsPrefab;
  public GameObject deathScreenPrefab;
  public CursorArrow cursorArrow;
  private UIEnergySelector energySelector;
  private UIDiscardSelector discardSelector;
  private UIAddcardSelector addcardSelector;
  private UIDeckCards deckCardsWindow;
  private UICombatDeath deathScreen;
  private float deckCardsWindowPosY;
  private List<GameObject> GO_List;
  private Vector3 handTransformPosition;
  public bool waitingTrail;
  private bool waitingKill;
  public bool waitingDeathScreen;
  public bool waitingTutorial;
  private bool characterKilled;
  private string gameStatus = "";
  private bool waitExecution;
  private bool gameBusy;
  private bool waitingForCardEnergyAssignment;
  private int energyAssigned;
  private bool matchIsOver;
  private int autoEndCount;
  private int limitAutoEndCount = 15;
  private int limitAutoEndCountStep1 = 8;
  private int failCount;
  private int cardsWaitingForReset;
  private string getStatusString = "";
  private string lastStatusString = "";
  public float castCardDamageDone;
  public float castCardDamageDoneTotal;
  public float castCardDamageDoneIteration;
  public Transform botEndTurn;
  private Animator botEndTurnAnim;
  public List<string> castedCards;
  private List<CardItem> CICardDiscard;
  private int GlobalDiscardCardsNum;
  private int GlobalVanishCardsNum;
  private bool waitingForDiscardAssignment;
  private bool discardNumDecidedByThePlayer;
  private List<CardItem> CICardAddcard;
  private int GlobalAddcardCardsNum;
  private bool waitingForAddcardAssignment;
  private bool waitingForLookDiscardWindow;
  private Coroutine coroutineSync;
  private Coroutine coroutineSyncFinishCombat;
  private Coroutine coroutineSyncPreFinishCast;
  private Coroutine coroutineSyncFixSyncCode;
  private Coroutine coroutineSyncBeginRound;
  private Coroutine coroutineSyncBeginTurnHero;
  private Coroutine coroutineSyncCastNPC;
  private Coroutine coroutineSyncCastCard;
  private Coroutine coroutineSyncCastCardNPC;
  private Coroutine coroutineSyncDealCards;
  private Coroutine coroutineSyncShuffle;
  private Coroutine coroutineSyncResign;
  private Coroutine coroutineSyngAssignEnergy;
  private Coroutine coroutineSyncLookDiscard;
  private Coroutine coroutineSyncDiscard;
  private Coroutine coroutineSyncAddcard;
  private Coroutine coroutineSyncWaitingAction;
  private Coroutine coroutineDeathScreen;
  private Coroutine coroutineCastNPC;
  private Coroutine coroutineDrawArrow;
  private Coroutine coroutineMask;
  private Coroutine coroutineDrawDeck;
  private Coroutine arrowMPCo;
  private Coroutine amplifyCardCo;
  private Vector3 ArrowTarget = Vector3.zero;
  public Transform CardCreator;
  public Console console;
  public Transform consoleCloseButton;
  private string consoleKey = "";
  public int CombatTextIterations;
  private Dictionary<string, CardData> cardDictionary;
  private Dictionary<string, CardData> cardDictionaryBackup;
  private bool backingDictionary;
  private Dictionary<string, List<string>> npcCardsCasted;
  private PhotonView photonView;
  private List<string> playersWatchingTutorial = new List<string>();
  private bool multiplayerWatchingTutorial;
  public Transform watchingTutorialText;
  public CardData corruptionCard;
  public ItemData corruptionItem;
  public string corruptionCardId;
  private bool tutorialCombat;
  private Dictionary<string, Coroutine> dictCoroutineConsume = new Dictionary<string, Coroutine>();
  private bool canKeyboardCast;
  private List<string> immunityListForNamedSaved = new List<string>();
  private int resultSeed = -1;
  public int[,] combatStatsAux;
  public int[,] combatStatsCurrentAux;
  public Dictionary<string, Dictionary<string, List<string>>> combatStatsDict;
  public Dictionary<string, Dictionary<string, List<string>>> combatStatsDictAux;
  public EmoteManager emoteManager;
  public GameObject emoteTargetPrefab;
  public List<Transform> emotesTransform;
  private Dictionary<string, LogEntry> logDictionary;
  private Dictionary<string, LogEntry> logDictionaryAux = new Dictionary<string, LogEntry>();
  public int controllerCurrentIndex;
  public int controllerCurrentShoulder = -1;
  public Transform controllerArrow;
  public bool controllerClickedCard;
  public int controllerClickedCardIndex = -1;
  private bool keyClickedCard;
  private Vector3 controllerOffsetVector;
  private List<Transform> controllerList = new List<Transform>();
  private Vector2 warpPosition;
  private Queue<Action> ctQueue = new Queue<Action>();
  private Coroutine checkQueueCo;
  private List<string> castingCardListMP = new List<string>();
  private string scarabSpawned = "";
  private string scarabSuccess = "0";
  private float sinceLastUpdate;
  private float updateTimeout = 0.015f;
  public List<GameObject> decorationsT;
  private BossNPC bossNpc;
  public GameObject phantomArmorPrefab;
  public GameObject phantomArmorVfxPrefab;
  private MindSpikeAbility mindSpikeAbility;
  private Aura auraCurse;
  private readonly Dictionary<string, Dictionary<Character, bool>> auraStates = new Dictionary<string, Dictionary<Character, bool>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  public static Action<NPCData, HeroData, int> OnCharacterKilled;
  public static Action<Character, int> OnCharacterDamaged;

  public MindSpikeAbility MindSpikeAbility => this.mindSpikeAbility;

  public static MatchManager Instance { get; private set; }

  private void Awake()
  {
    if ((UnityEngine.Object) GameManager.Instance == (UnityEngine.Object) null)
    {
      SceneStatic.LoadByName("TeamManagement");
    }
    else
    {
      if ((UnityEngine.Object) MatchManager.Instance == (UnityEngine.Object) null)
        MatchManager.Instance = this;
      else if ((UnityEngine.Object) MatchManager.Instance != (UnityEngine.Object) this)
        UnityEngine.Object.Destroy((UnityEngine.Object) this);
      this.botEndTurnAnim = this.botEndTurn.GetComponent<Animator>();
      this.deckCounterTM = this.deckCounter.GetChild(0).GetComponent<TMP_Text>();
      this.discardCounterTM = this.discardCounter.GetChild(0).GetComponent<TMP_Text>();
      this.newCardsCounterTM = this.newCardsCounter.GetChild(0).GetComponent<TMP_Text>();
      this.sceneCamera.gameObject.SetActive(false);
      this.photonView = PhotonView.Get((Component) this);
      this.bossNpc = (BossNPC) null;
      AlertManager.Instance.HideAlert();
      this.ShowMaskFull();
      NetworkManager.Instance.StartStopQueue(true);
      this.mindSpikeAbility = new MindSpikeAbility();
    }
  }

  private void Update()
  {
    this.sinceLastUpdate += Time.deltaTime;
    if ((double) this.sinceLastUpdate < (double) this.updateTimeout)
      return;
    this.sinceLastUpdate = 0.0f;
    ++this.combatCounter;
    if (this.MatchIsOver)
      return;
    if (this.combatCounter % 2 == 0)
    {
      if (!this.amplifiedTransformShow && this.amplifiedTransform.childCount > 0)
      {
        foreach (Component component in this.amplifiedTransform)
          UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
      }
      this.amplifiedTransformShow = false;
    }
    if (this.heroActive <= -1 || this.combatCounter % 2 != 0 && GameManager.Instance.configGameSpeed != Enums.ConfigSpeed.Ultrafast)
      return;
    if (this.heroTurn && this.IsYourTurn() && !this.waitingTutorial)
    {
      if (this.gameStatus != "CastCard" && this.cardsWaitingForReset == 0 && this.eventList.Count == 0 && !this.isBeginTournPhase && this.castingCardBlocked.Count == 0 && !this.waitingItemTrait && this.generatedCardTimes == 0)
      {
        this.ResetFailCount();
        if (this.autoEndCount < this.limitAutoEndCount)
        {
          this.getStatusString = this.GenerateSyncCodeForCheckingAction();
          if (this.getStatusString == this.lastStatusString)
            ++this.autoEndCount;
          else
            this.ResetAutoEndCount();
          this.lastStatusString = this.GenerateSyncCodeForCheckingAction();
        }
      }
      else
      {
        this.ResetAutoEndCount();
        this.ShowHandMask(true);
        if (!this.WaitingForActionScreen() && !this.waitingDeathScreen && !this.waitingKill)
        {
          this.getStatusString = this.GenerateStatusString();
          if (this.getStatusString != this.lastStatusString)
          {
            this.ResetFailCount();
          }
          else
          {
            if (this.failCount > 0 && this.failCount % 50 == 0 && Globals.Instance.ShowDebug)
            {
              string[] strArray = new string[14];
              strArray[0] = "[";
              strArray[1] = this.failCount.ToString();
              strArray[2] = "] ";
              strArray[3] = this.cardsWaitingForReset.ToString();
              strArray[4] = " && ";
              int count = this.eventList.Count;
              strArray[5] = count.ToString();
              strArray[6] = " && ";
              strArray[7] = this.isBeginTournPhase.ToString();
              strArray[8] = " && ";
              count = this.castingCardBlocked.Count;
              strArray[9] = count.ToString();
              strArray[10] = " && ";
              strArray[11] = this.waitingItemTrait.ToString();
              strArray[12] = " && ";
              strArray[13] = this.generatedCardTimes.ToString();
              Debug.LogError((object) string.Concat(strArray));
            }
            ++this.failCount;
            if (this.failCount == 300)
            {
              if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
                this.photonView.RPC("NET_MasterReloadCombat", RpcTarget.MasterClient, (object) ("from update() " + this.failCount.ToString()));
              else
                this.ReloadCombat("from update() " + this.failCount.ToString());
            }
          }
          this.lastStatusString = this.getStatusString;
        }
        else
          this.ResetFailCount();
      }
    }
    else
      this.ResetAutoEndCount();
    if (this.autoEndCount >= this.limitAutoEndCount)
      return;
    if (this.autoEndCount > 1)
    {
      if (this.IsYourTurn())
      {
        if (this.eventList.Count == 0 && this.cardsWaitingForReset == 0)
        {
          if (!GameManager.Instance.IsMultiplayer())
          {
            if (this.autoEndCount == this.limitAutoEndCount - 1)
            {
              if (GameManager.Instance.ConfigAutoEnd && !this.CanPlayACardRightNow())
              {
                this.ShowHandMask(true);
                this.botEndTurn.gameObject.SetActive(false);
                this.ShowEnergyCounter(false);
                this.EndTurn();
              }
              else
                this.autoEndCount = this.limitAutoEndCount;
            }
            else if (this.autoEndCount >= this.limitAutoEndCountStep1)
            {
              if (this.autoEndCount == this.limitAutoEndCountStep1)
              {
                this.ShowHandMask(false);
                this.SetGameBusy(false);
                this.RedrawCardsBorder();
                this.canKeyboardCast = true;
              }
              if (this.botEndTurn.gameObject.activeSelf)
                return;
              this.botEndTurn.gameObject.SetActive(true);
              this.ShowEnergyCounter(true);
              this.botEndTurnAnim.enabled = false;
            }
            else if (this.autoEndCount < this.limitAutoEndCountStep1)
              this.canKeyboardCast = false;
            else
              this.autoEndCount = this.limitAutoEndCount;
          }
          else if (!NetworkManager.Instance.IsSyncroClean())
            this.autoEndCount = 0;
          else if (this.autoEndCount == this.limitAutoEndCount - 1)
          {
            if (!GameManager.Instance.ConfigAutoEnd || this.CanPlayACardRightNow())
              return;
            this.ShowHandMask(true);
            this.botEndTurn.gameObject.SetActive(false);
            this.ShowEnergyCounter(false);
            this.EndTurn();
          }
          else if (this.autoEndCount >= this.limitAutoEndCountStep1)
          {
            if (this.autoEndCount == this.limitAutoEndCountStep1)
            {
              this.ShowHandMask(false);
              this.SetGameBusy(false);
              this.RedrawCardsBorder();
              this.canKeyboardCast = true;
            }
            if (this.botEndTurn.gameObject.activeSelf)
              return;
            this.botEndTurn.gameObject.SetActive(true);
            this.ShowEnergyCounter(true);
            this.botEndTurnAnim.enabled = false;
          }
          else if (this.autoEndCount < this.limitAutoEndCountStep1)
            this.canKeyboardCast = false;
          else
            this.autoEndCount = this.limitAutoEndCount;
        }
        else
          this.ShowHandMask(true);
      }
      else
      {
        if (this.botEndTurn.gameObject.activeSelf)
        {
          this.botEndTurn.gameObject.SetActive(false);
          this.ShowEnergyCounter(false);
        }
        this.RedrawCardsBorder();
        this.autoEndCount = this.limitAutoEndCount;
      }
    }
    else
    {
      if (this.combatCounter % 10 != 0)
        return;
      if (this.IsYourTurn())
        this.ShowHandMask(true);
      else
        this.ShowHandMask(false);
      if (this.deathScreen.IsActive() || this.addcardSelector.IsActive() || this.deckCardsWindow.IsActive())
        return;
      this.RedrawCardsBorder();
    }
  }

  private void Start()
  {
    this.StopCoroutines();
    if (GameManager.Instance.IsMultiplayer())
    {
      NetworkManager.Instance.ClearSyncro();
      NetworkManager.Instance.ClearPlayerStatus();
      NetworkManager.Instance.ClearWaitingCalls();
    }
    this.botEndTurnAnim.enabled = false;
    this.botEndTurn.gameObject.SetActive(false);
    this.ShowEnergyCounter(false);
    this.combatLoading = true;
    this.gotHeroDeck = false;
    this.gotNPCDeck = false;
    this.gotDictionary = false;
    this.ShowTraitInfo(false, true);
    AtOManager.Instance.ClearCacheGlobalACModification();
    AtOManager.Instance.ResetCombatScarab();
    this.InitializeVars();
    this.StartCoroutine(this.LoadTurnData());
  }

  private void ClearEventList() => this.eventList.Clear();

  private void ResetAutoEndCount()
  {
    this.autoEndCount = 0;
    this.combatCounter = 0;
  }

  private void ResetFailCount() => this.failCount = 0;

  public void SetWaitingKill(bool _state) => this.waitingKill = _state;

  private void StopCoroutines()
  {
    if (this.MatchIsOver)
      return;
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("*.*.*. STOP COROUTINES .*.*.*", "trace");
    this.StopAllCoroutines();
    this.ClearItemQueue();
  }

  private string GenerateStatusString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this.cardsWaitingForReset);
    stringBuilder.Append("//");
    stringBuilder.Append(this.eventList.Count);
    stringBuilder.Append("//");
    stringBuilder.Append(this.isBeginTournPhase);
    stringBuilder.Append("//");
    stringBuilder.Append(this.castingCardBlocked.Count);
    stringBuilder.Append("//");
    stringBuilder.Append(this.waitingItemTrait);
    stringBuilder.Append("//");
    stringBuilder.Append(this.generatedCardTimes);
    stringBuilder.Append("//");
    stringBuilder.Append(this.gameStatus);
    return stringBuilder.ToString();
  }

  [PunRPC]
  private void NET_SetLoadTurn(
    string turnData,
    string turnCombatDictionaryKeys,
    string turnCombatDictionaryValues,
    string turnHeroItems,
    string turnCombatStatsEffects,
    string turnCombatStatsCurrent,
    string turnHeroLife,
    string itemExecutionInCombat,
    string currentSpecialCardsUsedInMatch,
    string npcParams)
  {
    if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster() && turnData == "")
      NetworkManager.Instance.SetStatusReady("loadturndatasyncro");
    else
      this.SetLoadTurn(turnData, turnCombatDictionaryKeys, turnCombatDictionaryValues, turnHeroItems, turnCombatStatsEffects, turnCombatStatsCurrent, turnHeroLife, itemExecutionInCombat, currentSpecialCardsUsedInMatch, npcParams);
  }

  public void SetLoadTurn(
    string turnData,
    string turnCombatDictionaryKeys,
    string turnCombatDictionaryValues,
    string turnHeroItems,
    string turnCombatStatsEffects,
    string turnCombatStatsCurrent,
    string turnHeroLife,
    string itemExecutionInCombat,
    string currentSpecialCardsUsedInMatch,
    string npcParams)
  {
    AtOManager.Instance.combatGameCode = turnData;
    this.SetCardDictionaryKeysValues(turnCombatDictionaryKeys, turnCombatDictionaryValues);
    this.SetHeroItemsFromTurnSave(turnHeroItems);
    AtOManager.Instance.InitCombatStatsCurrent();
    this.SetCombatStatsForTurnSave(turnCombatStatsEffects);
    this.SetCombatStatsCurrentForTurnSave(turnCombatStatsCurrent);
    this.SetHeroLifeArrForTurnSave(turnHeroLife);
    this.SetItemExecutionInCombatForTurnSave(itemExecutionInCombat);
    this.SetCurrentSpecialCardsUsedInMatchForTurnSave(currentSpecialCardsUsedInMatch);
    this.SetNPCParamsFromTurnSave(npcParams);
    if (GameManager.Instance.IsMultiplayer())
    {
      if (NetworkManager.Instance.IsMaster())
      {
        this.turnDataForMP = new string[10];
        this.turnDataForMP[0] = turnData;
        this.turnDataForMP[1] = turnCombatDictionaryKeys;
        this.turnDataForMP[2] = turnCombatDictionaryValues;
        this.turnDataForMP[3] = turnHeroItems;
        this.turnDataForMP[4] = turnCombatStatsEffects;
        this.turnDataForMP[5] = turnCombatStatsCurrent;
        this.turnDataForMP[6] = turnHeroLife;
        this.turnDataForMP[7] = itemExecutionInCombat;
        this.turnDataForMP[8] = currentSpecialCardsUsedInMatch;
        this.turnDataForMP[9] = npcParams;
      }
      else
        NetworkManager.Instance.SetStatusReady("loadturndatasyncro");
    }
    this.turnLoadedBySave = true;
  }

  private IEnumerator LoadTurnData()
  {
    if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
      NetworkManager.Instance.SetWaitingSyncro("loadturndatasyncro", true);
    if (this.currentGameCode == "" && (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster()))
    {
      AtOManager.Instance.LoadGameTurn();
      for (int exhaust = 0; !this.turnLoadedBySave && exhaust < 20; ++exhaust)
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
    }
    if (GameManager.Instance.IsMultiplayer())
    {
      if (NetworkManager.Instance.IsMaster())
      {
        if (this.turnLoadedBySave)
        {
          if (this.turnDataForMP != null && this.turnDataForMP.Length == 10)
            this.photonView.RPC("NET_SetLoadTurn", RpcTarget.Others, (object) this.turnDataForMP[0], (object) this.turnDataForMP[1], (object) this.turnDataForMP[2], (object) this.turnDataForMP[3], (object) this.turnDataForMP[4], (object) this.turnDataForMP[5], (object) this.turnDataForMP[6], (object) this.turnDataForMP[7], (object) this.turnDataForMP[8], (object) this.turnDataForMP[9]);
          else
            this.photonView.RPC("NET_SetLoadTurn", RpcTarget.Others, (object) "", (object) "", (object) "", (object) "", (object) "", (object) "", (object) "", (object) "", (object) "", (object) "");
        }
        else
          this.photonView.RPC("NET_SetLoadTurn", RpcTarget.Others, (object) "", (object) "", (object) "", (object) "", (object) "", (object) "", (object) "", (object) "", (object) "", (object) "");
      }
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("**************************", "net");
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("WaitingSyncro loadturndatasyncro", "net");
      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      if (NetworkManager.Instance.IsMaster())
      {
        while (!NetworkManager.Instance.AllPlayersReady("loadturndatasyncro"))
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("Game ready, Everybody checked loadturndatasyncro", "net");
        NetworkManager.Instance.PlayersNetworkContinue("loadturndatasyncro");
      }
      else
      {
        while (NetworkManager.Instance.WaitingSyncro["loadturndatasyncro"])
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("loadturndatasyncro, we can continue!", "net");
      }
    }
    if (AtOManager.Instance.corruptionAccepted)
    {
      this.corruptionCard = Globals.Instance.GetCardData(AtOManager.Instance.corruptionIdCard, false);
      this.iconCorruption.transform.gameObject.SetActive(true);
      this.iconCorruption.ShowIconCorruption(this.corruptionCard);
      this.iconCorruption.transform.gameObject.SetActive(false);
    }
    ProgressManager.Instance.HideAll();
    if (AtOManager.Instance.currentMapNode == "tutorial_1")
      this.tutorialCombat = true;
    this.FinishLoadTurnData();
  }

  private void FinishLoadTurnData() => this.StartCoroutine(this.NewGame());

  public void Resize() => this.characterWindow.Resize();

  private void GameObjectFuncs()
  {
    this.helpCharacterTransform.gameObject.SetActive(false);
    this.cardGos = new Dictionary<string, GameObject>();
    this.gameObjectsParentFolder = this.worldTransform;
    foreach (Component component in this.goTransform)
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
    this.gameObjectsParentFolder = this.goTransform;
    this.HideExhaust();
    this.GO_NewTurn = UnityEngine.Object.Instantiate<GameObject>(this.newTurnPrefab, Vector3.zero, Quaternion.identity, this.gameObjectsParentFolder);
    this.GO_NewTurn.name = "NewTurn";
    this.newTurnScript = this.GO_NewTurn.transform.GetComponent<NewTurn>();
    this.GO_Initiative = new GameObject();
    this.GO_Initiative.name = "Initiative";
    this.GO_Initiative.transform.parent = this.gameObjectsParentFolder;
    this.GO_Heroes = new GameObject();
    this.GO_Heroes.name = "Heroes";
    this.GO_Heroes.transform.parent = this.gameObjectsParentFolder;
    this.GO_NPCs = new GameObject();
    this.GO_NPCs.name = "NPCs";
    this.GO_NPCs.transform.parent = this.gameObjectsParentFolder;
    Transform transform1 = this.GO_Heroes.transform;
    Transform transform2 = this.GO_NPCs.transform;
    Vector3 vector3_1 = new Vector3(0.0f, -0.6f, 5f);
    Vector3 vector3_2 = vector3_1;
    transform2.position = vector3_2;
    Vector3 vector3_3 = vector3_1;
    transform1.position = vector3_3;
    this.GO_Hand = new GameObject();
    this.GO_Hand.name = "Hand";
    this.GO_Hand.transform.parent = this.gameObjectsParentFolder;
    this.GO_Hand.transform.position = new Vector3(0.0f, this.handPosY, 0.0f);
    this.GO_Hand.transform.localScale = new Vector3(0.95f, 0.95f, 1f);
    this.HandMask.parent = this.GO_Hand.transform;
    if ((UnityEngine.Object) this.GO_DecksObject != (UnityEngine.Object) null)
    {
      this.deckCounter.transform.SetParent(this.gameObjectsParentFolder, true);
      this.discardCounter.transform.SetParent(this.gameObjectsParentFolder, true);
      foreach (Component component in this.GO_DecksObject.transform)
        UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
    }
    this.GO_DecksObject = new GameObject();
    this.GO_DecksObject.name = "Decks";
    this.GO_DecksObject.transform.parent = this.gameObjectsParentFolder;
    this.GO_DecksObject.transform.localScale = new Vector3(0.95f, 0.95f, 1f);
    GameObject gameObject1 = UnityEngine.Object.Instantiate<GameObject>(this.energySelectorPrefab, Vector3.zero, Quaternion.identity, this.gameObjectsParentFolder);
    gameObject1.name = "GO_EnergySelector";
    this.energySelector = gameObject1.GetComponent<UIEnergySelector>();
    GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.discardSelectorPrefab, Vector3.zero, Quaternion.identity, this.gameObjectsParentFolder);
    gameObject2.name = "GO_DiscardSelector";
    this.discardSelector = gameObject2.GetComponent<UIDiscardSelector>();
    GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.addcardSelectorPrefab, Vector3.zero, Quaternion.identity, this.gameObjectsParentFolder);
    gameObject3.name = "GO_AddcardSelector";
    this.addcardSelector = gameObject3.GetComponent<UIAddcardSelector>();
    GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(this.deckCardsPrefab, Vector3.zero, Quaternion.identity, this.gameObjectsParentFolder);
    gameObject4.name = "GO_DeckCards";
    this.deckCardsWindow = gameObject4.GetComponent<UIDeckCards>();
    this.deathScreen = UnityEngine.Object.Instantiate<GameObject>(this.deathScreenPrefab, Vector3.zero, Quaternion.identity, this.gameObjectsParentFolder).GetComponent<UICombatDeath>();
    this.GO_DiscardPile = new GameObject();
    this.GO_DiscardPile.name = "DiscardPile";
    this.GO_DiscardPile.transform.parent = this.GO_DecksObject.transform;
    this.GO_DeckPile = new GameObject();
    this.GO_DeckPile.name = "DeckPile";
    this.GO_DeckPile.transform.parent = this.GO_DecksObject.transform;
    this.GO_DeckPileCards = new GameObject[4];
    this.GO_DeckPileCardsT = new Transform[this.GO_DeckPileCards.Length];
    for (int index = 0; index < this.GO_DeckPileCards.Length; ++index)
    {
      GameObject gameObject5 = UnityEngine.Object.Instantiate<GameObject>(this.deckPileCardPrefab, Vector3.zero, Quaternion.identity, this.GO_DeckPile.transform);
      gameObject5.GetComponent<BoxCollider2D>().enabled = false;
      this.GO_DeckPileCardsT[index] = gameObject5.transform;
      if (index > 0)
      {
        gameObject5.transform.localPosition = new Vector3(0.04f * (float) (index - 1), 0.04f * (float) (index - 1), 0.0f);
        this.GO_DeckPileCards[index - 1] = gameObject5;
      }
      else
      {
        gameObject5.transform.localPosition = Vector3.zero;
        gameObject5.transform.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.3f);
      }
      gameObject5.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
      gameObject5.GetComponent<Renderer>().sortingOrder = index - 10000;
    }
    this.deckPileParticle = UnityEngine.Object.Instantiate<GameObject>(this.deckPileParticlePrefab, Vector3.zero, Quaternion.identity, this.GO_DeckPile.transform).GetComponent<ParticleSystem>();
    this.DeckPilePosition = new Vector3(-8.5f, -4f, 0.0f);
    this.GO_DecksObject.transform.localPosition = this.DeckPilePosition;
    this.deckCounter.transform.localPosition = this.DeckPilePosition + new Vector3(-0.6f, -0.3f, 0.0f);
    this.discardCounter.transform.localPosition = this.DeckPilePosition + new Vector3(2.4f, -0.3f, 0.0f);
    this.deckCounter.transform.SetParent(this.GO_DecksObject.transform, true);
    this.discardCounter.transform.SetParent(this.GO_DecksObject.transform, true);
    this.DiscardPilePosition = this.DeckPilePosition + new Vector3(1.8f, 0.0f, 0.0f);
    GameObject gameObject6 = UnityEngine.Object.Instantiate<GameObject>(this.deckPileCardPrefab, Vector3.zero, Quaternion.identity, this.gameObjectsParentFolder);
    gameObject6.name = "discardpile";
    gameObject6.transform.localScale = new Vector3(0.8f, 0.9f, 1f);
    gameObject6.transform.localPosition = new Vector3(this.DiscardPilePosition.x, this.DiscardPilePosition.y, -1f);
    gameObject6.transform.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    this.discardPileParticle = UnityEngine.Object.Instantiate<GameObject>(this.deckPileParticlePrefab, gameObject6.transform.localPosition, Quaternion.identity, this.GO_DeckPile.transform).GetComponent<ParticleSystem>();
    this.discardPileParticle.GetComponent<Renderer>().sortingLayerName = "Discards";
    this.GO_DeckPile.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
    this.GO_DiscardPile.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
    this.GO_DecksObject.transform.localPosition = this.DeckPilePosition - this.DeckPileOutOfScreenPositionVector;
  }

  private void InitializeVars()
  {
    if (TomeManager.Instance.IsActive())
      TomeManager.Instance.ShowTome(false);
    if (CardScreenManager.Instance.IsActive())
      CardScreenManager.Instance.ShowCardScreen(false);
    if (this.console.IsActive())
      this.console.Show(false);
    if (DamageMeterManager.Instance.IsActive())
      DamageMeterManager.Instance.Hide();
    if (GameManager.Instance.GetDeveloperMode())
      Debug.Log((object) "Initialize Variables");
    this.reloadingGame = false;
    this.heroIndexWaitingForAddDiscard = -1;
    this.HeroDeck = new List<string>[4];
    this.HeroDeckDiscard = new List<string>[4];
    this.HeroDeckVanish = new List<string>[4];
    this.HeroHand = new List<string>[4];
    this.NPCDeck = new List<string>[4];
    this.NPCDeckDiscard = new List<string>[4];
    this.NPCHand = new List<string>[4];
    this.cardDictionary = new Dictionary<string, CardData>();
    this.castedCards = new List<string>();
    this.castedCards.Add("");
    this.CICardDiscard = new List<CardItem>();
    this.CICardAddcard = new List<CardItem>();
    this.npcCardsCasted = new Dictionary<string, List<string>>();
    this.canInstaCastDict = new Dictionary<string, bool>();
    if (!this.turnLoadedBySave)
      AtOManager.Instance.InitCombatStatsCurrent();
    for (int index = 0; index < 4; ++index)
      this.HeroDeck[index] = new List<string>();
    for (int index = 0; index < 4; ++index)
      this.NPCDeck[index] = new List<string>();
    this.itemTimeout = new float[10];
    for (int index = 0; index < this.itemTimeout.Length; ++index)
      this.itemTimeout[index] = 0.0f;
  }

  private IEnumerator NewGame()
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("[NewGame] AtOManager.Instance.combatGameCode -> " + AtOManager.Instance.combatGameCode, "synccode");
    if (AtOManager.Instance.combatGameCode == "")
    {
      this.combatStatsAux = new int[AtOManager.Instance.combatStats.GetLength(0), AtOManager.Instance.combatStats.GetLength(1)];
      this.combatStatsCurrentAux = new int[AtOManager.Instance.combatStatsCurrent.GetLength(0), AtOManager.Instance.combatStatsCurrent.GetLength(1)];
      this.combatStatsDict = new Dictionary<string, Dictionary<string, List<string>>>();
      this.logDictionary = new Dictionary<string, LogEntry>();
      this.logDictionaryAux = new Dictionary<string, LogEntry>();
      this.StoreCombatStats();
    }
    else if (this.combatStatsDict == null)
    {
      this.combatStatsAux = new int[AtOManager.Instance.combatStats.GetLength(0), AtOManager.Instance.combatStats.GetLength(1)];
      this.combatStatsCurrentAux = new int[AtOManager.Instance.combatStatsCurrent.GetLength(0), AtOManager.Instance.combatStatsCurrent.GetLength(1)];
      this.combatStatsDict = new Dictionary<string, Dictionary<string, List<string>>>();
      this.StoreCombatStats();
    }
    this.logDictionary = new Dictionary<string, LogEntry>();
    foreach (KeyValuePair<string, LogEntry> keyValuePair in this.logDictionaryAux)
      this.logDictionary.Add(keyValuePair.Key, keyValuePair.Value);
    this.logDictionaryAux = new Dictionary<string, LogEntry>();
    this.GameObjectFuncs();
    this.HandMask.GetComponent<BoxCollider2D>().enabled = true;
    this.watchingTutorialText.gameObject.SetActive(false);
    this.GetCombatData();
    this.GenerateRandomStringBatch();
    if (GameManager.Instance.IsMultiplayer())
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("**************************", "net");
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("WaitingSyncro newgame", "net");
      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      if (NetworkManager.Instance.IsMaster())
      {
        this.GenerateSyncCodeDict();
        while (!NetworkManager.Instance.AllPlayersReady("newgame"))
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("Game ready, Everybody checked newgame", "net");
        NetworkManager.Instance.PlayersNetworkContinue("newgame");
      }
      else
      {
        NetworkManager.Instance.SetWaitingSyncro("newgame", true);
        NetworkManager.Instance.SetStatusReady("newgame");
        while (NetworkManager.Instance.WaitingSyncro["newgame"])
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("newgame, we can continue!", "net");
      }
    }
    if (!GameManager.Instance.IsMultiplayer() || GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
    {
      this.DoBackground();
      this.GetPlayerTeam();
      this.GetNPCTeam();
      this.SetRandomIndex(UnityEngine.Random.Range(200, 500));
      if (GameManager.Instance.IsMultiplayer())
      {
        string str = "";
        if ((UnityEngine.Object) this.combatData != (UnityEngine.Object) null)
          str = this.combatData.CombatId;
        this.photonView.RPC("NET_SetTeams", RpcTarget.Others, (object) str, (object) this.randomIndex);
      }
      this.ContinueNewGame();
    }
    yield return (object) null;
  }

  private void GetCombatData()
  {
    AtOManager.Instance.CinematicId = "";
    this.combatData = AtOManager.Instance.GetCurrentCombatData();
    if ((UnityEngine.Object) this.combatData == (UnityEngine.Object) null)
    {
      this.combatData = AtOManager.Instance.fromEventCombatData;
      AtOManager.Instance.fromEventCombatData = (CombatData) null;
    }
    else
    {
      if (!((UnityEngine.Object) this.combatData.CinematicData != (UnityEngine.Object) null))
        return;
      AtOManager.Instance.CinematicId = this.combatData.CinematicData.CinematicId;
    }
  }

  private void ContinueNewGame()
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD(nameof (ContinueNewGame));
    this.GenerateHeroes();
    this.GenerateNPCs();
    this.RepositionCharacters();
    this.SortCharacterSprites();
    this.StartCoroutine(this.ContinueNewGameCo());
  }

  private IEnumerator ContinueNewGameCo()
  {
    if (GameManager.Instance.IsMultiplayer())
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("**************************", "net");
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("WaitingSyncro ContinueNewGameCo", "net");
      if (NetworkManager.Instance.IsMaster())
      {
        while (!NetworkManager.Instance.AllPlayersReady(nameof (ContinueNewGameCo)))
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("Game ready, Everybody checked ContinueNewGameCo", "net");
        NetworkManager.Instance.PlayersNetworkContinue(nameof (ContinueNewGameCo));
      }
      else
      {
        NetworkManager.Instance.SetWaitingSyncro(nameof (ContinueNewGameCo), true);
        NetworkManager.Instance.SetStatusReady(nameof (ContinueNewGameCo));
        while (NetworkManager.Instance.WaitingSyncro[nameof (ContinueNewGameCo)])
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("ContinueNewGameCo, we can continue!", "net");
      }
    }
    if (AtOManager.Instance.combatGameCode != "")
      this.CheckForCombatCode();
    else if (!GameManager.Instance.IsMultiplayer() || GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
    {
      this.GenerateDecks();
      this.GenerateDecksNPCs();
      this.NET_ShareDecks(true);
    }
  }

  public void DoSahtiRustBackground(bool showPittAlive, bool justKilled = false)
  {
    if ((UnityEngine.Object) this.backgroundSahtiAlive == (UnityEngine.Object) null)
      this.backgroundSahtiAlive = this.backgroundPrefab.transform.Find("Alive");
    if ((UnityEngine.Object) this.backgroundSahtiDead == (UnityEngine.Object) null)
      this.backgroundSahtiDead = this.backgroundPrefab.transform.Find("Dead");
    if (showPittAlive)
    {
      this.backgroundSahtiAlive.gameObject.SetActive(true);
      this.backgroundSahtiDead.gameObject.SetActive(false);
    }
    else
    {
      this.backgroundSahtiAlive.gameObject.SetActive(false);
      this.backgroundSahtiDead.gameObject.SetActive(true);
      if (!justKilled)
        return;
      this.StartCoroutine(this.DoSahtiRustBackgroundJustKilled(this.backgroundSahtiDead));
    }
  }

  private IEnumerator DoSahtiRustBackgroundJustKilled(Transform deadT)
  {
    SpriteRenderer pittSR = deadT.Find("cavern_deadpitt").GetComponent<SpriteRenderer>();
    pittSR.color = new Color(1f, 1f, 1f, 0.0f);
    float index = 0.0f;
    Color hideColor = new Color(1f, 1f, 1f, 0.0f);
    while ((double) index < 1.0)
    {
      yield return (object) Globals.Instance.WaitForSeconds(0.02f);
      index += 0.1f;
      hideColor.a = index;
      pittSR.color = hideColor;
    }
  }

  private void DoBackground()
  {
    GameManager.Instance.PlayAudio(AudioManager.Instance.soundCombatBegins);
    AudioClip audioClip = (AudioClip) null;
    if ((UnityEngine.Object) this.combatData != (UnityEngine.Object) null)
    {
      this.backgroundActive = Enum.GetName(typeof (Enums.CombatBackground), (object) this.combatData.CombatBackground);
      audioClip = this.combatData.CombatMusic;
    }
    else
      this.backgroundActive = "Senenthia_Dia";
    for (int index1 = 0; index1 < this.backgroundPrefabs.Count; ++index1)
    {
      if ((UnityEngine.Object) this.backgroundPrefabs[index1] != (UnityEngine.Object) null && this.backgroundActive != "" && this.backgroundPrefabs[index1].name.ToLower() == this.backgroundActive.ToLower())
      {
        bool flag1 = false;
        for (int index2 = 0; index2 < this.backgroundTransform.childCount; ++index2)
        {
          if (this.backgroundTransform.GetChild(index2).gameObject.name == this.backgroundPrefabs[index1].name)
          {
            flag1 = true;
            break;
          }
        }
        if (!flag1)
        {
          this.backgroundPrefab = UnityEngine.Object.Instantiate<GameObject>(this.backgroundPrefabs[index1], new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, this.backgroundTransform);
          this.backgroundPrefab.name = this.backgroundPrefabs[index1].name;
          this.backgroundPrefab.transform.localScale = new Vector3(0.545f, 0.545f, 1f);
          bool flag2 = false;
          bool flag3 = false;
          bool flag4 = false;
          if (GameManager.Instance.IsWeeklyChallenge())
          {
            ChallengeData weeklyData = Globals.Instance.GetWeeklyData(AtOManager.Instance.GetWeekly());
            if (weeklyData?.IdSteam == "challengeSpooky")
              flag2 = true;
            else if (weeklyData?.IdSteam == "challengeChristmas")
              flag3 = true;
            else if (weeklyData?.IdSteam == "challengeLunar")
              flag4 = true;
          }
          Transform parent = this.backgroundPrefab.transform.Find("halloween");
          if ((UnityEngine.Object) parent != (UnityEngine.Object) null)
          {
            if (flag2 | flag3)
            {
              if (!parent.gameObject.activeSelf)
                parent.gameObject.SetActive(true);
              if (flag3)
              {
                Transform transform = (Transform) null;
                int childCount = parent.childCount;
                for (int index3 = 0; index3 < childCount; ++index3)
                {
                  Transform child = parent.GetChild(index3);
                  if (child.gameObject.activeSelf)
                  {
                    if ((bool) (UnityEngine.Object) child.GetComponent<SpriteRenderer>())
                      transform = child;
                    else if ((UnityEngine.Object) child.GetChild(0) != (UnityEngine.Object) null && (bool) (UnityEngine.Object) child.GetChild(0).GetComponent<SpriteRenderer>())
                      transform = child.GetChild(0);
                    if ((UnityEngine.Object) transform != (UnityEngine.Object) null && (UnityEngine.Object) transform.GetComponent<SpriteRenderer>() != (UnityEngine.Object) null && (UnityEngine.Object) transform.GetComponent<SpriteRenderer>().sprite != (UnityEngine.Object) null)
                    {
                      string str = "";
                      if (flag3)
                        str = transform.GetComponent<SpriteRenderer>().sprite.name.Replace("halloween", "christmas").Replace("_front", "");
                      else if (flag4)
                        str = transform.GetComponent<SpriteRenderer>().sprite.name.Replace("halloween", "lunar").Replace("_front", "");
                      if (str != "")
                      {
                        for (int index4 = 0; index4 < this.decorationsT.Count; ++index4)
                        {
                          if (this.decorationsT[index4].name == str)
                          {
                            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.decorationsT[index4], child.transform.position, Quaternion.identity, parent);
                            gameObject.transform.localScale = child.transform.localScale;
                            if ((bool) (UnityEngine.Object) gameObject.GetComponent<SpriteRenderer>())
                            {
                              gameObject.GetComponent<SpriteRenderer>().sortingOrder = transform.GetComponent<SpriteRenderer>().sortingOrder;
                              gameObject.GetComponent<SpriteRenderer>().sortingLayerName = transform.GetComponent<SpriteRenderer>().sortingLayerName;
                              break;
                            }
                            break;
                          }
                        }
                      }
                    }
                    child.gameObject.SetActive(false);
                  }
                }
              }
            }
            else if (parent.gameObject.activeSelf)
              parent.gameObject.SetActive(false);
          }
          Transform transform1 = this.backgroundPrefab.transform.Find("Lunar");
          if ((UnityEngine.Object) transform1 != (UnityEngine.Object) null)
          {
            if (flag4)
            {
              if (!transform1.gameObject.activeSelf)
                transform1.gameObject.SetActive(true);
            }
            else if (transform1.gameObject.activeSelf)
              transform1.gameObject.SetActive(false);
          }
        }
      }
    }
    if ((UnityEngine.Object) this.combatData != (UnityEngine.Object) null && this.combatData.IsRift)
      UnityEngine.Object.Instantiate<GameObject>(this.riftPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, this.backgroundTransform);
    if ((UnityEngine.Object) audioClip != (UnityEngine.Object) null)
      AudioManager.Instance.DoBSOAudioClip(audioClip);
    if (this.backgroundActive == "Senenthia_Dia")
      AudioManager.Instance.DoAmbience("forest_ambience_day");
    else if (this.backgroundActive == "Senenthia_Tarde" || this.backgroundActive == "Senenthia_LobosTarde" || this.backgroundActive == "Senenthia_Bosque" || this.backgroundActive == "Faeborg_Bosque" || this.backgroundActive == "Faeborg_BosqueCastor")
      AudioManager.Instance.DoAmbience("forest_ambience_sunset");
    else if (this.backgroundActive == "Senenthia_LobosNoche" || this.backgroundActive == "Senenthia_BosqueEntrada" || this.backgroundActive == "Senenthia_BosqueBoss")
    {
      AudioManager.Instance.DoAmbience("forest_ambience_night");
    }
    else
    {
      if (!(this.backgroundActive == "Sectarium") && !(this.backgroundActive == "Spider_Lair") && !(this.backgroundActive == "Sewers"))
        return;
      AudioManager.Instance.DoAmbience("cave_ambience");
    }
  }

  [PunRPC]
  private void NET_SetTeams(string _combatDataId, int _randomIndex)
  {
    if (_combatDataId != "")
    {
      this.combatData = Globals.Instance.GetCombatData(_combatDataId);
      this.DoBackground();
    }
    this.GetPlayerTeam();
    this.GetNPCTeam();
    this.SetRandomIndex(_randomIndex);
    this.ContinueNewGame();
  }

  public bool IsYourTurnForAddDiscard()
  {
    return !GameManager.Instance.IsMultiplayer() || this.heroIndexWaitingForAddDiscard > -1 && this.TeamHero[this.heroIndexWaitingForAddDiscard].Owner == NetworkManager.Instance.GetPlayerNick();
  }

  public bool IsYourTurn()
  {
    return !GameManager.Instance.IsMultiplayer() || this.heroActive > -1 && this.TeamHero[this.heroActive].Owner == NetworkManager.Instance.GetPlayerNick();
  }

  public void SetOwnerForTeamHero(int index, string owner)
  {
    if (this.TeamHero[index] == null)
      return;
    this.TeamHero[index].Owner = owner;
  }

  private void GetPlayerTeam()
  {
    List<Hero> heroList = new List<Hero>();
    this.TeamHero = AtOManager.Instance.GetTeam();
    for (int index = 0; index < this.TeamHero.Length; ++index)
    {
      if (this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroData == (UnityEngine.Object) null) && this.TeamHero[index].Id != null)
        heroList.Add(this.TeamHero[index]);
    }
    this.TeamHero = new Hero[heroList.Count];
    for (int index = 0; index < heroList.Count; ++index)
      this.TeamHero[index] = heroList[index];
  }

  private void GetNPCTeam()
  {
    string[] teamNpc = AtOManager.Instance.GetTeamNPC();
    NPC[] npcArray = new NPC[4];
    this.TeamNPC = new NPC[4];
    for (int index = 0; index < teamNpc.Length; ++index)
    {
      this.TeamNPC[index] = (NPC) null;
      if (teamNpc[index] != null && teamNpc[index] != "" && (!((UnityEngine.Object) this.combatData != (UnityEngine.Object) null) || (!GameManager.Instance.IsGameAdventure() || AtOManager.Instance.GetMadnessDifficulty() != 0) && (!GameManager.Instance.IsSingularity() || AtOManager.Instance.GetSingularityMadness() != 0) || this.combatData.NpcRemoveInMadness0Index != index || AtOManager.Instance.GetActNumberForText() >= 3))
      {
        NPC npc = new NPC();
        npc.NpcData = Globals.Instance.GetNPC(teamNpc[index]);
        npc.InitData();
        npc.Position = index;
        this.TeamNPC[index] = npc;
      }
    }
    if (!((UnityEngine.Object) this.combatData != (UnityEngine.Object) null) || !this.combatData.RandomizeNpcPosition)
      return;
    System.Random random = new System.Random();
    for (int index1 = this.TeamNPC.Length - 1; index1 > 0; --index1)
    {
      int index2 = random.Next(index1 + 1) % (this.TeamNPC.Length - 1);
      NPC npc = this.TeamNPC[index2];
      this.TeamNPC[index2] = this.TeamNPC[index1];
      if (this.TeamNPC[index1] != null)
      {
        this.TeamNPC[index2].Position = this.TeamNPC[index1].Position;
        this.TeamNPC[index2].NPCIndex = this.TeamNPC[index1].NPCIndex;
      }
      this.TeamNPC[index1] = npc;
      if (npc != null)
      {
        this.TeamNPC[index1].Position = npc.Position;
        this.TeamNPC[index1].NPCIndex = npc.NPCIndex;
      }
    }
  }

  private void UpdateBossNpc()
  {
    if (this.bossNpc != null)
      return;
    foreach (NPC npc in this.TeamNPC)
    {
      if (npc != null && npc.NPCIsBoss())
      {
        if (npc.NpcData.Id == "franky")
          this.bossNpc = (BossNPC) new Frankenstein(npc);
        else if (npc.NpcData.Id == "s_queen")
          this.bossNpc = (BossNPC) new SirenQueen();
        else if (npc.NpcData.Id == "count")
          this.bossNpc = (BossNPC) new Dracula(npc);
        else if (npc.NpcData.Id == "pa_d")
          this.bossNpc = (BossNPC) new PhantomArmor(npc);
      }
    }
    if ((UnityEngine.Object) this.combatData != (UnityEngine.Object) null)
    {
      if (!((UnityEngine.Object) this.combatData.NpcToSummonOnNpcKilled != (UnityEngine.Object) null) || !(this.combatData.NpcToSummonOnNpcKilled.Id == "s_queen"))
        return;
      this.bossNpc = (BossNPC) new SirenQueen();
    }
    else
    {
      if (!AtOManager.Instance.SirenQueenBattle)
        return;
      this.bossNpc = (BossNPC) new SirenQueen();
    }
  }

  private void OnDestroy()
  {
    if (this.bossNpc == null)
      return;
    this.bossNpc.Dispose();
  }

  private void GenerateHeroes()
  {
    int num = 0;
    Hero[] heroArray = new Hero[4];
    for (int index = 0; index < this.TeamHero.Length; ++index)
    {
      if (this.TeamHero[index] != null && (!this.tutorialCombat || index != 1 && index != 2))
      {
        Hero _hero = this.TeamHero[index];
        if (AtOManager.Instance.combatGameCode == "" || this.teamHeroItemsFromTurnSave != null)
        {
          this.heroLifeArr[index] = _hero.HpCurrent;
          List<string> stringList = new List<string>();
          stringList.Add(_hero.Weapon);
          stringList.Add(_hero.Armor);
          stringList.Add(_hero.Jewelry);
          stringList.Add(_hero.Accesory);
          stringList.Add(_hero.Pet);
          if (!this.heroBeginItems.ContainsKey(index))
            this.heroBeginItems.Add(index, stringList);
          else
            this.heroBeginItems[index] = stringList;
        }
        if (AtOManager.Instance.combatGameCode != "")
        {
          if (this.teamHeroItemsFromTurnSave != null)
          {
            if (this.teamHeroItemsFromTurnSave.Count > index * 5 + 4)
            {
              _hero.Weapon = this.teamHeroItemsFromTurnSave[index * 5];
              _hero.Armor = this.teamHeroItemsFromTurnSave[index * 5 + 1];
              _hero.Jewelry = this.teamHeroItemsFromTurnSave[index * 5 + 2];
              _hero.Accesory = this.teamHeroItemsFromTurnSave[index * 5 + 3];
              _hero.Pet = this.teamHeroItemsFromTurnSave[index * 5 + 4];
            }
          }
          else if (this.currentRound == 0 && this.heroBeginItems != null && this.heroBeginItems.ContainsKey(index) && this.heroBeginItems[index] != null)
          {
            List<string> heroBeginItem = this.heroBeginItems[index];
            _hero.Weapon = heroBeginItem[0];
            _hero.Armor = heroBeginItem[1];
            _hero.Jewelry = heroBeginItem[2];
            _hero.Accesory = heroBeginItem[3];
            _hero.Pet = heroBeginItem[4];
          }
          else if (this.currentRound > 0 && this.heroDestroyedItemsInThisTurn.ContainsKey(index))
          {
            if (this.heroDestroyedItemsInThisTurn[index].ContainsKey("weapon"))
              _hero.Weapon = this.heroDestroyedItemsInThisTurn[index]["weapon"];
            if (this.heroDestroyedItemsInThisTurn[index].ContainsKey("armor"))
              _hero.Armor = this.heroDestroyedItemsInThisTurn[index]["armor"];
            if (this.heroDestroyedItemsInThisTurn[index].ContainsKey("jewelry"))
              _hero.Jewelry = this.heroDestroyedItemsInThisTurn[index]["jewelry"];
            if (this.heroDestroyedItemsInThisTurn[index].ContainsKey("accesory"))
              _hero.Accesory = this.heroDestroyedItemsInThisTurn[index]["accesory"];
            if (this.heroDestroyedItemsInThisTurn[index].ContainsKey("pet"))
              _hero.Pet = this.heroDestroyedItemsInThisTurn[index]["pet"];
          }
        }
        if (_hero.HpCurrent <= 0)
          _hero.HpCurrent = 1;
        _hero.Alive = true;
        _hero.InternalId = this.GetRandomString();
        _hero.Id = _hero.HeroData.HeroSubClass.Id + "_" + _hero.InternalId;
        _hero.Position = num;
        GameObject charGO = UnityEngine.Object.Instantiate<GameObject>(this.heroPrefab, Vector3.zero, Quaternion.identity, this.GO_Heroes.transform);
        charGO.name = _hero.Id;
        this.targetTransformDict.Add(_hero.Id, charGO.transform);
        _hero.ResetDataForNewCombat(this.currentGameCode == "");
        _hero.SetHeroIndex(index);
        _hero.HeroItem = charGO.GetComponent<HeroItem>();
        _hero.HeroItem.HeroData = _hero.HeroData;
        _hero.HeroItem.Init(_hero);
        _hero.HeroItem.SetPosition(true);
        if (AtOManager.Instance.CharacterHavePerk(_hero.SubclassName, "mainperkmark1a") && !_hero.AuracurseImmune.Contains("mark"))
          _hero.AuracurseImmune.Add("mark");
        if (AtOManager.Instance.CharacterHavePerk(_hero.SubclassName, "mainperkinspire0c") && !_hero.AuracurseImmune.Contains("stress"))
          _hero.AuracurseImmune.Add("stress");
        this.HeroHand[index] = new List<string>();
        this.HeroDeckDiscard[index] = new List<string>();
        this.HeroDeckVanish[index] = new List<string>();
        heroArray[index] = _hero;
        ++num;
        CardData pet = _hero.GetPet();
        if ((UnityEngine.Object) pet != (UnityEngine.Object) null)
          this.CreatePet(pet, charGO, _hero, (NPC) null);
      }
    }
    this.TeamHero = new Hero[4];
    for (int index = 0; index < 4; ++index)
      this.TeamHero[index] = heroArray[index];
    this.teamHeroItemsFromTurnSave = (List<string>) null;
  }

  public void RemovePetEnchantment(GameObject charGO, string _enchantmentName = "")
  {
    string n = "thePetEnchantment" + _enchantmentName;
    Transform transform = charGO.transform.Find(n);
    if (!((UnityEngine.Object) transform != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) transform.gameObject);
  }

  public void CreatePet(
    CardData cardPet,
    GameObject charGO,
    Hero _hero,
    NPC _npc,
    bool _fromEnchant = false,
    string _enchantName = "")
  {
    string n = "thePet";
    if (_fromEnchant)
      n = "thePetEnchantment" + _enchantName;
    Transform transform = charGO.transform.Find(n);
    if ((UnityEngine.Object) transform != (UnityEngine.Object) null)
    {
      if (_fromEnchant)
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) transform.gameObject);
    }
    GameObject GO = UnityEngine.Object.Instantiate<GameObject>(cardPet.PetModel, Vector3.zero, Quaternion.identity, charGO.transform);
    GO.name = n;
    if ((UnityEngine.Object) cardPet.PetModel != (UnityEngine.Object) null && cardPet.PetModel.name.ToLower() == "scrappy")
      ScrappyRobotResolver.ShowHideRobotLayers(GO.transform, cardPet.Id);
    GO.GetComponent<BoxCollider2D>().enabled = false;
    if (_hero != null && !_fromEnchant)
      _hero.HeroItem.animPet = GO.GetComponent<Animator>();
    else if (_npc != null)
      _npc.NPCItem.animPet = GO.GetComponent<Animator>();
    GO.transform.localScale = new Vector3(GO.transform.localScale.x * cardPet.PetSize.x, GO.transform.localScale.y * cardPet.PetSize.y, GO.transform.localScale.z);
    if (cardPet.PetInvert && _hero != null)
      GO.transform.localScale = new Vector3(GO.transform.localScale.x * -1f, GO.transform.localScale.y, GO.transform.localScale.z);
    GO.transform.localPosition = (Vector3) cardPet.PetOffset;
    NPCItem npcItem = GO.AddComponent<NPCItem>();
    npcItem.SetOriginalLocalPosition(GO.transform.localPosition);
    npcItem.GetSpritesFromAnimated(GO);
    if (!cardPet.PetFront)
      npcItem.DeleteShadow(GO);
    if (_hero != null)
    {
      if (!_fromEnchant)
      {
        _hero.HeroItem.PetItem = npcItem;
        _hero.HeroItem.PetItemFront = cardPet.PetFront;
      }
      else
      {
        _hero.HeroItem.PetItemEnchantment = npcItem;
        _hero.HeroItem.PetItemEnchantmentFront = cardPet.PetFront;
      }
      _hero.HeroItem.DrawOrderSprites(true, _hero.Position * 2);
    }
    else
    {
      if (_npc == null)
        return;
      if (!_fromEnchant)
      {
        _npc.NPCItem.PetItem = npcItem;
        _npc.NPCItem.PetItemFront = cardPet.PetFront;
      }
      else
      {
        _npc.NPCItem.PetItemEnchantment = npcItem;
        _npc.NPCItem.PetItemEnchantmentFront = cardPet.PetFront;
      }
      _npc.NPCItem.DrawOrderSprites(true, _npc.Position * 2);
    }
  }

  private void GenerateNPCs()
  {
    int num1 = 0;
    for (int index = 0; index < this.TeamNPC.Length; ++index)
    {
      NPC _npc;
      if (this.TeamNPC[index] == null || (UnityEngine.Object) this.TeamNPC[index].NpcData == (UnityEngine.Object) null)
      {
        _npc = new NPC();
        _npc.NpcData = Globals.Instance.GetNPC("sheep");
        _npc.Alive = false;
        _npc.HpCurrent = 0;
      }
      else
      {
        _npc = this.TeamNPC[index];
        _npc.NpcData = Globals.Instance.GetNPC(_npc.GameName);
        if ((UnityEngine.Object) _npc.NpcData != (UnityEngine.Object) null && AtOManager.Instance.PlayerHasRequirement(Globals.Instance.GetRequirementData("_tier2")) && (UnityEngine.Object) _npc.NpcData.UpgradedMob != (UnityEngine.Object) null)
          _npc.NpcData = _npc.NpcData.UpgradedMob;
        if ((UnityEngine.Object) _npc.NpcData != (UnityEngine.Object) null && (AtOManager.Instance.GetNgPlus() > 0 || !_npc.NpcData.IsNamed && !_npc.NpcData.IsBoss && AtOManager.Instance.IsChallengeTraitActive("toughermonsters") || _npc.NpcData.IsNamed && !_npc.NpcData.IsBoss && AtOManager.Instance.IsChallengeTraitActive("toughermonsters") || _npc.NpcData.IsBoss && AtOManager.Instance.IsChallengeTraitActive("hardcorebosses")) && (UnityEngine.Object) _npc.NpcData.NgPlusMob != (UnityEngine.Object) null)
          _npc.NpcData = _npc.NpcData.NgPlusMob;
        if ((MadnessManager.Instance.IsMadnessTraitActive("despair") || AtOManager.Instance.IsChallengeTraitActive("despair")) && (UnityEngine.Object) _npc.NpcData.HellModeMob != (UnityEngine.Object) null)
          _npc.NpcData = _npc.NpcData.HellModeMob;
        ++num1;
      }
      _npc.SetNPCIndex(index);
      _npc.Position = index;
      if ((UnityEngine.Object) _npc.NpcData != (UnityEngine.Object) null && _npc.Alive)
      {
        _npc.InternalId = this.GetRandomString();
        _npc.InitData();
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.npcPrefab, Vector3.zero, Quaternion.identity, this.GO_NPCs.transform);
        gameObject.name = _npc.Id;
        this.targetTransformDict.Add(gameObject.name, gameObject.transform);
        _npc.GO = gameObject;
        _npc.NPCItem = gameObject.GetComponent<NPCItem>();
        _npc.NPCItem.NpcData = _npc.NpcData;
        _npc.NPCItem.Init(_npc);
        _npc.NPCItem.SetPosition(true);
        this.NPCHand[index] = new List<string>();
        this.NPCDeckDiscard[index] = new List<string>();
      }
    }
    if (this.currentRound == 0 && AtOManager.Instance.Sandbox_lessNPCs != 0)
    {
      SortedDictionary<int, int> source = new SortedDictionary<int, int>();
      for (int index = 0; index < this.TeamNPC.Length; ++index)
      {
        if (this.TeamNPC[index] != null && (UnityEngine.Object) this.TeamNPC[index].NpcData != (UnityEngine.Object) null && this.TeamNPC[index].Alive && !this.TeamNPC[index].NpcData.IsNamed && !this.TeamNPC[index].NpcData.IsBoss)
          source.Add(this.TeamNPC[index].GetHp() * 10000 + index, index);
      }
      int num2 = AtOManager.Instance.Sandbox_lessNPCs;
      if (num2 >= num1)
        num2 = num1 - 1;
      if (num2 > source.Count)
        num2 = source.Count;
      for (int index1 = 0; index1 < num2; ++index1)
      {
        NPC[] teamNpc1 = this.TeamNPC;
        KeyValuePair<int, int> keyValuePair = source.ElementAt<KeyValuePair<int, int>>(index1);
        int index2 = keyValuePair.Value;
        NPC currentCharacter = teamNpc1[index2];
        if (currentCharacter != null)
        {
          if (MatchManager.CanDestroyIllusion((Character) currentCharacter))
            this.DestroyIllusion((Character) currentCharacter);
          currentCharacter.DestroyCharacter();
          currentCharacter.Alive = false;
          currentCharacter.HpCurrent = 0;
          currentCharacter.NpcData = (NPCData) null;
          NPC[] teamNpc2 = this.TeamNPC;
          keyValuePair = source.ElementAt<KeyValuePair<int, int>>(index1);
          int index3 = keyValuePair.Value;
          teamNpc2[index3] = (NPC) null;
        }
      }
    }
    if (!(this.backgroundActive == "Sahti_Rust"))
      return;
    bool flag = false;
    for (int index = 0; index < this.TeamNPC.Length; ++index)
    {
      if (this.TeamNPC[index] != null && this.TeamNPC[index].Id.StartsWith("pitt") && this.TeamNPC[index].Alive)
      {
        flag = true;
        this.DoSahtiRustBackground(true);
        break;
      }
    }
    if (flag)
      return;
    this.DoSahtiRustBackground(false);
  }

  public int GetNPCAvailablePosition(int slotsNeeded = 1)
  {
    if (slotsNeeded < 1 || slotsNeeded > this.TeamNPC.Length)
      return -1;
    for (int availablePosition = 0; availablePosition < this.TeamNPC.Length && availablePosition + slotsNeeded <= this.TeamNPC.Length; ++availablePosition)
    {
      if ((this.TeamNPC[availablePosition] == null || !this.TeamNPC[availablePosition].Alive) && (availablePosition <= 0 || this.TeamNPC[availablePosition - 1] == null || !((UnityEngine.Object) this.TeamNPC[availablePosition - 1].NpcData != (UnityEngine.Object) null) || !this.TeamNPC[availablePosition - 1].Alive || !this.TeamNPC[availablePosition - 1].NpcData.BigModel))
      {
        bool flag = true;
        for (int index = 0; index < slotsNeeded; ++index)
        {
          if (this.TeamNPC[availablePosition + index] != null && this.TeamNPC[availablePosition + index].Alive)
          {
            flag = false;
            break;
          }
        }
        if (flag)
          return availablePosition;
      }
    }
    return -1;
  }

  public void RemoveFromTransformDict(string _key)
  {
    if (this.targetTransformDict == null || !this.targetTransformDict.ContainsKey(_key))
      return;
    this.targetTransformDict.Remove(_key);
  }

  private Character CreateIllusion(
    Character originalCharacter,
    string effectTarget,
    CardData card1,
    int createdCharacterPositionInTeam)
  {
    bool isNPC = originalCharacter is NPC;
    if (!isNPC)
      throw new InvalidOperationException("Illusion for Hero is not supported");
    CharacterItem characterItem = isNPC ? (CharacterItem) originalCharacter.NPCItem : (CharacterItem) originalCharacter.HeroItem;
    List<Aura> auraList = originalCharacter.AuraList;
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.npcPrefab, Vector3.zero, Quaternion.identity, this.GO_NPCs.transform);
    Character characterWithUniqueId = this.CreateCharacterWithUniqueId(originalCharacter, isNPC, createdCharacterPositionInTeam);
    gameObject.name = characterWithUniqueId.Id;
    if (this.targetTransformDict != null)
      this.targetTransformDict[gameObject.name] = gameObject.transform;
    characterWithUniqueId.Position = createdCharacterPositionInTeam;
    characterWithUniqueId.RoundMoved = this.currentRound + 1;
    characterWithUniqueId.GO = gameObject;
    if (isNPC)
    {
      characterWithUniqueId.SetNPCIndex(createdCharacterPositionInTeam);
      characterWithUniqueId.NPCItem = gameObject.GetComponent<NPCItem>();
      characterWithUniqueId.NPCItem.NpcData = characterWithUniqueId.NpcData;
      characterWithUniqueId.NPCItem.Init((NPC) characterWithUniqueId);
      characterWithUniqueId.NPCItem.DrawOrderSprites(false, createdCharacterPositionInTeam * 2);
      this.NPCHand[createdCharacterPositionInTeam] = new List<string>();
      this.NPCDeckDiscard[createdCharacterPositionInTeam] = new List<string>((IEnumerable<string>) this.NPCDeckDiscard[originalCharacter.NPCIndex]);
      this.NPCDeck[createdCharacterPositionInTeam] = new List<string>();
      this.NPCDeck[createdCharacterPositionInTeam].AddRange(this.NPCHand[originalCharacter.NPCIndex].Where<string>((Func<string, bool>) (card2 => card2 != null)));
      this.NPCDeck[createdCharacterPositionInTeam].AddRange((IEnumerable<string>) this.NPCDeck[originalCharacter.NPCIndex]);
    }
    else
    {
      characterWithUniqueId.SetHeroIndex(createdCharacterPositionInTeam);
      characterWithUniqueId.HeroItem = gameObject.GetComponent<HeroItem>();
      characterWithUniqueId.HeroItem.HeroData = characterWithUniqueId.HeroData;
      characterWithUniqueId.HeroItem.Init((Hero) characterWithUniqueId);
      characterWithUniqueId.HeroItem.DrawOrderSprites(false, createdCharacterPositionInTeam * 2);
      this.HeroHand[createdCharacterPositionInTeam] = new List<string>();
      this.HeroDeckDiscard[createdCharacterPositionInTeam] = new List<string>((IEnumerable<string>) this.HeroDeckDiscard[originalCharacter.HeroIndex]);
      this.HeroDeck[createdCharacterPositionInTeam].AddRange(this.HeroHand[originalCharacter.HeroIndex].Where<string>((Func<string, bool>) (card3 => card3 != null)));
      this.HeroDeck[createdCharacterPositionInTeam].AddRange((IEnumerable<string>) this.HeroDeck[originalCharacter.HeroIndex]);
    }
    this.SetInitiatives();
    this.ReDrawInitiatives();
    (isNPC ? (CharacterItem) characterWithUniqueId.NPCItem : (CharacterItem) characterWithUniqueId.HeroItem).SetPosition(true);
    string str = (isNPC ? (IEnumerable<Character>) this.TeamNPC : (IEnumerable<Character>) this.TeamHero).FirstOrDefault<Character>((Func<Character, bool>) (m => m != null && !(m.Corruption == "")))?.Corruption ?? "";
    characterWithUniqueId.Corruption = str;
    characterWithUniqueId.AuraList = auraList.Select<Aura, Aura>((Func<Aura, Aura>) (a => a.DeepClone())).ToList<Aura>();
    characterWithUniqueId.HpCurrent = originalCharacter.HpCurrent;
    characterWithUniqueId.UpdateAuraCurseFunctions();
    if (isNPC)
    {
      List<string> collection;
      if (!this.npcCardsCasted.TryGetValue(originalCharacter.InternalId, out collection))
        collection = new List<string>();
      this.npcCardsCasted[characterWithUniqueId.InternalId] = new List<string>((IEnumerable<string>) collection);
    }
    if ((UnityEngine.Object) card1 != (UnityEngine.Object) null)
    {
      if (!string.IsNullOrEmpty(effectTarget))
        EffectsManager.Instance.PlayEffect(card1, false, false, characterItem.CharImageT);
      if ((UnityEngine.Object) card1.SummonAura != (UnityEngine.Object) null && card1.SummonAuraCharges > 0)
        characterWithUniqueId.SetAura((Character) null, card1.SummonAura, card1.SummonAuraCharges);
      if ((UnityEngine.Object) card1.SummonAura2 != (UnityEngine.Object) null && card1.SummonAuraCharges2 > 0)
        characterWithUniqueId.SetAura((Character) null, card1.SummonAura2, card1.SummonAuraCharges2);
      if ((UnityEngine.Object) card1.SummonAura3 != (UnityEngine.Object) null && card1.SummonAuraCharges3 > 0)
        characterWithUniqueId.SetAura((Character) null, card1.SummonAura3, card1.SummonAuraCharges3);
    }
    characterWithUniqueId.IsIllusion = true;
    originalCharacter.IllusionCharacter = characterWithUniqueId;
    if (isNPC)
      characterWithUniqueId.NpcData.FinishCombatOnDead = false;
    return characterWithUniqueId;
  }

  private Character CreateCharacterWithUniqueId(
    Character original,
    bool isNPC,
    int newPositionInTeam)
  {
    Character character;
    if (!isNPC)
    {
      Hero hero = new Hero();
      hero.HeroData = original.HeroData;
      character = (Character) hero;
    }
    else
    {
      character = (Character) new NPC();
      character.NpcData = original.NpcData;
    }
    Character characterWithUniqueId = character;
    Character[] characterArray = isNPC ? (Character[]) this.TeamNPC : (Character[]) this.TeamHero;
    characterWithUniqueId.InitData();
    characterWithUniqueId.Position = newPositionInTeam;
    characterArray[newPositionInTeam] = characterWithUniqueId;
    string randomString = this.GetRandomString();
    while (this.targetTransformDict.ContainsKey(randomString))
      randomString = this.GetRandomString();
    characterWithUniqueId.InternalId = randomString;
    bool flag;
    do
    {
      flag = false;
      for (int index = 0; index < characterArray.Length; ++index)
      {
        if (index != newPositionInTeam && characterArray[index] != null && characterArray[index].InternalId == characterWithUniqueId.InternalId)
        {
          flag = true;
          characterWithUniqueId.InternalId += newPositionInTeam.ToString();
          break;
        }
      }
    }
    while (flag);
    characterWithUniqueId.InitData();
    GameManager.Instance.PlayAudio(Globals.Instance.GetAuraCurseData("spark").GetSound(true));
    return characterWithUniqueId;
  }

  public void CreateNPC(
    NPCData _npcData,
    string effectTarget = "",
    int _position = -1,
    bool generateFromReload = false,
    string internalId = "",
    CardData _cardActive = null,
    string _casterInternalId = "",
    float delay = 0.0f,
    bool replaceCharacter = false,
    bool isSummon = false)
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("CreateNPC -> " + _npcData?.ToString() + " ID->" + internalId + " INPOS->" + _position.ToString(), "trace");
    int index1 = -1;
    bool flag1 = false;
    if (!generateFromReload && (UnityEngine.Object) _cardActive != (UnityEngine.Object) null && (_cardActive.Metamorph || _cardActive.Evolve))
    {
      replaceCharacter = true;
      if (_cardActive.Evolve)
        flag1 = true;
    }
    if (!replaceCharacter)
    {
      index1 = this.GetNPCAvailablePosition();
      if (_position > -1)
        index1 = _position;
    }
    else if (_casterInternalId != "")
    {
      for (int index2 = 0; index2 < this.TeamNPC.Length; ++index2)
      {
        if (this.TeamNPC[index2] != null && this.TeamNPC[index2].InternalId == _casterInternalId)
        {
          index1 = index2;
          break;
        }
      }
    }
    else
      replaceCharacter = false;
    if (index1 <= -1)
      return;
    float num = 1f;
    List<Aura> auraList = (List<Aura>) null;
    if (replaceCharacter)
    {
      if (flag1)
      {
        auraList = new List<Aura>((IEnumerable<Aura>) this.TeamNPC[index1].AuraList);
        num = (float) this.TeamNPC[index1].HpCurrent / (float) this.TeamNPC[index1].Hp;
      }
      this.TeamNPC[index1].DestroyCharacter();
    }
    NPC _npc = new NPC();
    _npc.NpcData = _npcData;
    _npc.InitData();
    _npc.Position = index1;
    this.TeamNPC[index1] = _npc;
    if (internalId == "")
    {
      string randomString = this.GetRandomString();
      while (this.targetTransformDict.ContainsKey(randomString))
        randomString = this.GetRandomString();
      _npc.InternalId = randomString;
    }
    else
      _npc.InternalId = internalId;
    bool flag2 = true;
    while (flag2)
    {
      flag2 = false;
      for (int index3 = 0; index3 < this.TeamNPC.Length; ++index3)
      {
        if (index3 != index1 && this.TeamNPC[index3] != null && this.TeamNPC[index3].InternalId == _npc.InternalId)
        {
          flag2 = true;
          _npc.InternalId += index1.ToString();
          break;
        }
      }
    }
    _npc.InitData();
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.npcPrefab, Vector3.zero, Quaternion.identity, this.GO_NPCs.transform);
    gameObject.name = _npc.Id;
    _npc.SetNPCIndex(index1);
    _npc.Position = index1;
    _npc.RoundMoved = this.currentRound;
    _npc.GO = gameObject;
    if (this.targetTransformDict != null && this.targetTransformDict.ContainsKey(gameObject.name))
      this.targetTransformDict.Remove(gameObject.name);
    this.targetTransformDict.Add(gameObject.name, _npc.GO.transform);
    _npc.NPCItem = gameObject.GetComponent<NPCItem>();
    _npc.NPCItem.NpcData = _npcData;
    _npc.NPCItem.Init(_npc);
    _npc.IsSummon = isSummon;
    _npc.NPCItem.SetPosition(true);
    _npc.NPCItem.DrawOrderSprites(false, index1 * 2);
    this.NPCHand[index1] = new List<string>();
    this.NPCDeckDiscard[index1] = new List<string>();
    if (!generateFromReload)
    {
      _npc.NPCItem.InstantFadeOutCharacter();
      Globals.Instance.StartCoroutine(_npc.NPCItem.FadeInCharacter(delay));
      this.GenerateDecksNPCs(index1);
    }
    this.SetInitiatives();
    this.ReDrawInitiatives();
    this.RepositionCharacters();
    if ((UnityEngine.Object) _cardActive != (UnityEngine.Object) null)
    {
      if (effectTarget != "")
        EffectsManager.Instance.PlayEffect(_cardActive, false, false, _npc.NPCItem.CharImageT);
      if ((UnityEngine.Object) _cardActive.SummonAura != (UnityEngine.Object) null && _cardActive.SummonAuraCharges > 0)
        _npc.SetAura((Character) null, _cardActive.SummonAura, _cardActive.SummonAuraCharges);
      if ((UnityEngine.Object) _cardActive.SummonAura2 != (UnityEngine.Object) null && _cardActive.SummonAuraCharges2 > 0)
        _npc.SetAura((Character) null, _cardActive.SummonAura2, _cardActive.SummonAuraCharges2);
      if ((UnityEngine.Object) _cardActive.SummonAura3 != (UnityEngine.Object) null && _cardActive.SummonAuraCharges3 > 0)
        _npc.SetAura((Character) null, _cardActive.SummonAura3, _cardActive.SummonAuraCharges3);
    }
    if (!generateFromReload)
    {
      string str = "";
      for (int index4 = 0; index4 < 4; ++index4)
      {
        if (this.TeamNPC[index4] != null && (UnityEngine.Object) this.TeamNPC[index4].NpcData != (UnityEngine.Object) null && this.TeamNPC[index4].Corruption != "")
        {
          str = this.TeamNPC[index4].Corruption;
          break;
        }
      }
      _npc.Corruption = str;
    }
    if (!flag1)
      return;
    _npc.AuraList = auraList;
    _npc.HpCurrent = Functions.FuncRoundToInt((float) _npc.Hp * num);
    _npc.UpdateAuraCurseFunctions();
  }

  public void GenerateDecks()
  {
    List<string>[] stringListArray = new List<string>[4];
    for (int index1 = 0; index1 < this.TeamHero.Length; ++index1)
    {
      if (this.TeamHero[index1] != null && !((UnityEngine.Object) this.TeamHero[index1].HeroData == (UnityEngine.Object) null))
      {
        stringListArray[index1] = new List<string>();
        List<string> stringList = this.TeamHero[index1].Cards;
        if (this.tutorialCombat)
        {
          switch (index1)
          {
            case 0:
              stringList = new List<string>();
              stringList.Add("fastStrike");
              stringList.Add("defend");
              stringList.Add("rend");
              stringList.Add("intercept");
              stringList.Add("intercept");
              break;
            case 3:
              stringList = new List<string>();
              stringList.Add("heal");
              stringList.Add("heal");
              stringList.Add("heal");
              stringList.Add("flash");
              stringList.Add("foresight");
              break;
          }
        }
        for (int index2 = 0; index2 < stringList.Count; ++index2)
        {
          if (!((UnityEngine.Object) Globals.Instance.GetCardData(stringList[index2], false) == (UnityEngine.Object) null))
          {
            string cardInDictionary = this.CreateCardInDictionary(stringList[index2]);
            stringListArray[index1].Add(cardInDictionary);
          }
        }
      }
    }
    for (int index3 = 0; index3 < this.TeamHero.Length; ++index3)
    {
      if (this.TeamHero[index3] != null && !((UnityEngine.Object) this.TeamHero[index3].HeroData == (UnityEngine.Object) null))
      {
        List<string> stringList1 = stringListArray[index3].ShuffleList<string>();
        this.HeroDeck[index3] = stringList1;
        if (this.currentRound == 0)
        {
          List<string> ts1 = new List<string>();
          List<string> ts2 = new List<string>();
          for (int index4 = this.HeroDeck[index3].Count - 1; index4 >= 0; --index4)
          {
            CardData cardData = this.GetCardData(this.HeroDeck[index3][index4]);
            if (cardData.Innate)
            {
              ts1.Add(this.HeroDeck[index3][index4]);
              this.HeroDeck[index3].RemoveAt(index4);
            }
            else if (cardData.Lazy)
            {
              ts2.Add(this.HeroDeck[index3][index4]);
              this.HeroDeck[index3].RemoveAt(index4);
            }
          }
          if (ts1.Count > 0)
          {
            List<string> stringList2 = ts1.ShuffleList<string>();
            stringList2.AddRange((IEnumerable<string>) this.HeroDeck[index3]);
            this.HeroDeck[index3] = new List<string>();
            this.HeroDeck[index3].Clear();
            for (int index5 = 0; index5 < stringList2.Count; ++index5)
              this.HeroDeck[index3].Add(stringList2[index5]);
          }
          if (ts2.Count > 0)
          {
            List<string> stringList3 = ts2.ShuffleList<string>();
            for (int index6 = 0; index6 < stringList3.Count; ++index6)
              this.HeroDeck[index3].Add(stringList3[index6]);
          }
        }
      }
    }
  }

  public void GenerateDecksNPCs(int _npcIndex = -1)
  {
    List<string>[] stringListArray = new List<string>[4];
    for (int index1 = 0; index1 < this.TeamNPC.Length; ++index1)
    {
      if ((_npcIndex <= -1 || index1 == _npcIndex) && this.TeamNPC[index1] != null && !((UnityEngine.Object) this.TeamNPC[index1].NpcData == (UnityEngine.Object) null))
      {
        stringListArray[index1] = new List<string>();
        for (int index2 = 0; index2 < this.TeamNPC[index1].Cards.Count; ++index2)
        {
          bool flag = true;
          for (int index3 = 0; index3 < this.TeamNPC[index1].NpcData.AICards.Length; ++index3)
          {
            if (this.TeamNPC[index1].NpcData.AICards[index3].Card.Id == this.TeamNPC[index1].Cards[index2] && this.TeamNPC[index1].NpcData.AICards[index3].AddCardRound >= this.currentRound + 1)
              flag = false;
          }
          if (flag && !((UnityEngine.Object) Globals.Instance.GetCardData(this.TeamNPC[index1].Cards[index2], false) == (UnityEngine.Object) null))
          {
            string cardInDictionary = this.CreateCardInDictionary(this.TeamNPC[index1].Cards[index2]);
            stringListArray[index1].Add(cardInDictionary);
          }
        }
      }
    }
    for (int index4 = 0; index4 < this.TeamNPC.Length; ++index4)
    {
      if ((_npcIndex <= -1 || index4 == _npcIndex) && this.TeamNPC[index4] != null && !((UnityEngine.Object) this.TeamNPC[index4].NpcData == (UnityEngine.Object) null))
      {
        List<string> stringList1 = stringListArray[index4].ShuffleList<string>();
        this.NPCDeck[index4] = stringList1;
        List<string> ts1 = new List<string>();
        List<string> ts2 = new List<string>();
        for (int index5 = this.NPCDeck[index4].Count - 1; index5 >= 0; --index5)
        {
          CardData cardData = this.GetCardData(this.NPCDeck[index4][index5]);
          if (cardData.Innate)
          {
            ts1.Add(this.NPCDeck[index4][index5]);
            this.NPCDeck[index4].RemoveAt(index5);
          }
          else if (cardData.Lazy)
          {
            ts2.Add(this.NPCDeck[index4][index5]);
            this.NPCDeck[index4].RemoveAt(index5);
          }
        }
        if (ts1.Count > 0)
        {
          List<string> stringList2 = ts1.ShuffleList<string>();
          stringList2.AddRange((IEnumerable<string>) this.NPCDeck[index4]);
          this.NPCDeck[index4] = new List<string>();
          this.NPCDeck[index4].Clear();
          for (int index6 = 0; index6 < stringList2.Count; ++index6)
            this.NPCDeck[index4].Add(stringList2[index6]);
        }
        if (ts2.Count > 0)
        {
          List<string> stringList3 = ts2.ShuffleList<string>();
          for (int index7 = 0; index7 < stringList3.Count; ++index7)
            this.NPCDeck[index4].Add(stringList3[index7]);
        }
      }
    }
  }

  private void CheckForCombatCode()
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD(nameof (CheckForCombatCode), "net");
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD(AtOManager.Instance.combatGameCode, "net");
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD(AtOManager.Instance.combatGameCode, "synccode");
    this.eventList.Add(nameof (CheckForCombatCode));
    if (AtOManager.Instance.combatGameCode != "")
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("+++++++++++ RESTORE COMBAT FROM +++++++++++++++", "net");
      AtOManager.Instance.combatCardDictionary = (Dictionary<string, CardData>) null;
      this.FixCodeSyncFromMasterTOTAL(this.randomIndex, AtOManager.Instance.combatGameCode);
    }
    this.eventList.Remove(nameof (CheckForCombatCode));
  }

  public string GetCardDictionaryKeys()
  {
    string[] array = new string[this.cardDictionary.Count];
    this.cardDictionary.Keys.CopyTo(array, 0);
    return Functions.CompressString(JsonHelper.ToJson<string>(array));
  }

  public string GetCardDictionaryValues()
  {
    MatchManager.CardDataForShare[] array = new MatchManager.CardDataForShare[this.cardDictionary.Count];
    int index = 0;
    foreach (KeyValuePair<string, CardData> card in this.cardDictionary)
    {
      array[index] = new MatchManager.CardDataForShare()
      {
        vanish = card.Value.Vanish,
        energyReductionPermanent = card.Value.EnergyReductionPermanent,
        energyReductionTemporal = card.Value.EnergyReductionTemporal,
        energyReductionToZeroPermanent = card.Value.EnergyReductionToZeroPermanent,
        energyReductionToZeroTemporal = card.Value.EnergyReductionToZeroTemporal
      };
      ++index;
    }
    return Functions.CompressString(JsonHelper.ToJson<MatchManager.CardDataForShare>(array));
  }

  public void SetCardDictionaryKeysValues(string _keys, string _values)
  {
    if (_keys == "")
      return;
    this.NET_SaveCardDictionary(_keys, _values);
    AtOManager.Instance.combatCardDictionary = new Dictionary<string, CardData>();
    foreach (KeyValuePair<string, CardData> card in this.cardDictionary)
      AtOManager.Instance.combatCardDictionary.Add(card.Key, card.Value);
  }

  public string GetHeroItemsForTurnSave()
  {
    List<string> stringList = new List<string>();
    for (int index = 0; index < 4; ++index)
    {
      if (this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroData == (UnityEngine.Object) null))
      {
        stringList.Add(this.TeamHero[index].Weapon);
        stringList.Add(this.TeamHero[index].Armor);
        stringList.Add(this.TeamHero[index].Jewelry);
        stringList.Add(this.TeamHero[index].Accesory);
        stringList.Add(this.TeamHero[index].Pet);
      }
    }
    return Functions.CompressString(JsonHelper.ToJson<string>(stringList.ToArray()));
  }

  public void SetHeroItemsFromTurnSave(string _heroItems)
  {
    if (_heroItems == "")
      return;
    _heroItems = Functions.DecompressString(_heroItems);
    this.teamHeroItemsFromTurnSave = ((IEnumerable<string>) JsonHelper.FromJson<string>(_heroItems)).ToList<string>();
  }

  public string GetNPCParamsForTurnSave()
  {
    if (!((IEnumerable<NPC>) this.TeamNPC).Any<NPC>((Func<NPC, bool>) (npc => this.IsIllusion((Character) npc) || npc.IsSummon)))
      return string.Empty;
    List<NPCState> npcStateList = new List<NPCState>();
    foreach (NPC npc in this.TeamNPC)
    {
      if (!((UnityEngine.Object) npc?.NpcData == (UnityEngine.Object) null) && (this.IsIllusion((Character) npc) || npc.IllusionCharacter != null))
      {
        Character illusionCharacter = npc.IllusionCharacter;
        npcStateList.Add(new NPCState()
        {
          Id = npc.NpcData.Id + "_" + npc.InternalId,
          IsIllusion = npc.IsIllusion,
          IsIllusionExposed = npc.IsIllusionExposed,
          IllusionCharacterId = illusionCharacter == null ? "" : illusionCharacter.NpcData.Id + "_" + illusionCharacter.InternalId
        });
      }
    }
    return npcStateList.Count != 0 ? Functions.CompressString(JsonHelper.ToJson<NPCState>(npcStateList.ToArray())) : string.Empty;
  }

  public void SetNPCParamsFromTurnSave(string _npcParams)
  {
    if (string.IsNullOrEmpty(_npcParams))
      return;
    _npcParams = Functions.DecompressString(_npcParams);
    NPCState[] source = JsonHelper.FromJson<NPCState>(_npcParams);
    if (source == null)
      return;
    this.npcParamsFromTurnSave = ((IEnumerable<NPCState>) source).ToList<NPCState>();
  }

  private void NET_ShareDecks(bool ContinueNewGame = false)
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("[BEGIN] NET_ShareDecks", "net");
    if (GameManager.Instance.IsMultiplayer())
    {
      if (this.currentRound == 0)
      {
        string[] array1 = new string[this.cardDictionary.Count];
        this.cardDictionary.Keys.CopyTo(array1, 0);
        string json1 = JsonHelper.ToJson<string>(array1);
        MatchManager.CardDataForShare[] array2 = new MatchManager.CardDataForShare[this.cardDictionary.Count];
        int index = 0;
        foreach (KeyValuePair<string, CardData> card in this.cardDictionary)
        {
          array2[index] = new MatchManager.CardDataForShare()
          {
            vanish = card.Value.Vanish,
            energyReductionPermanent = card.Value.EnergyReductionPermanent,
            energyReductionTemporal = card.Value.EnergyReductionTemporal,
            energyReductionToZeroPermanent = card.Value.EnergyReductionToZeroPermanent,
            energyReductionToZeroTemporal = card.Value.EnergyReductionToZeroTemporal
          };
          ++index;
        }
        string json2 = JsonHelper.ToJson<MatchManager.CardDataForShare>(array2);
        this.photonView.RPC("NET_SaveCardDictionary", RpcTarget.Others, (object) Functions.CompressString(json1), (object) Functions.CompressString(json2));
      }
      else
        this.photonView.RPC("NET_SaveCardDictionary", RpcTarget.Others, (object) "", (object) "");
      this.gotDictionary = true;
      string[] strArray1 = new string[8];
      string[] strArray2 = new string[4];
      for (int index = 0; index < 4; ++index)
      {
        strArray1[index] = "";
        if (this.HeroDeck[index] != null)
        {
          string json = JsonHelper.ToJson<string>(this.HeroDeck[index].ToArray());
          strArray1[index] = Functions.CompressString(json);
        }
      }
      this.photonView.RPC("NET_SaveCardDeck", RpcTarget.Others, (object) "Hero", (object) strArray1[0], (object) strArray1[1], (object) strArray1[2], (object) strArray1[3]);
      string[] strArray3 = new string[4];
      string[] strArray4 = new string[4];
      for (int index = 0; index < 4; ++index)
      {
        strArray3[index] = "";
        if (this.NPCDeck[index] != null)
        {
          string json = JsonHelper.ToJson<string>(this.NPCDeck[index].ToArray());
          strArray3[index] = Functions.CompressString(json);
        }
      }
      this.photonView.RPC("NET_SaveCardDeck", RpcTarget.Others, (object) "NPC", (object) strArray3[0], (object) strArray3[1], (object) strArray3[2], (object) strArray3[3]);
      if (!ContinueNewGame)
        return;
      this.gotHeroDeck = true;
      this.gotNPCDeck = true;
      this.photonView.RPC("NET_BeginMatch", RpcTarget.All, (object) this.randomIndex);
    }
    else
    {
      if (!ContinueNewGame)
        return;
      this.StartCoroutine(this.BeginMatch());
    }
  }

  [PunRPC]
  private void NET_BeginMatch(int _randomIndex)
  {
    this.SetRandomIndex(_randomIndex);
    this.StartCoroutine(this.BeginMatch());
  }

  [PunRPC]
  private void NET_SaveCardDictionary(string _keys, string _values)
  {
    if (_keys != "" && _values != "")
    {
      this.cardDictionary.Clear();
      string[] strArray = JsonHelper.FromJson<string>(Functions.DecompressString(_keys));
      MatchManager.CardDataForShare[] cardDataForShareArray = JsonHelper.FromJson<MatchManager.CardDataForShare>(Functions.DecompressString(_values));
      for (int index = 0; index < strArray.Length; ++index)
      {
        CardData cardData = (CardData) null;
        if (!this.cardDictionary.ContainsKey(strArray[index]))
        {
          cardData = Globals.Instance.GetCardData(strArray[index].Split(char.Parse("_"), StringSplitOptions.None)[0]);
          this.cardDictionary.Add(strArray[index], cardData);
        }
        this.cardDictionary[strArray[index]].InitClone(strArray[index]);
        if ((UnityEngine.Object) cardData == (UnityEngine.Object) null)
          cardData = UnityEngine.Object.Instantiate<CardData>(this.cardDictionary[strArray[index]]);
        MatchManager.CardDataForShare cardDataForShare = cardDataForShareArray[index];
        cardData.Vanish = cardDataForShare.vanish;
        cardData.EnergyReductionPermanent = cardDataForShare.energyReductionPermanent;
        cardData.EnergyReductionTemporal = cardDataForShare.energyReductionTemporal;
        cardData.EnergyReductionToZeroPermanent = cardDataForShare.energyReductionToZeroPermanent;
        cardData.EnergyReductionToZeroTemporal = cardDataForShare.energyReductionToZeroTemporal;
        cardData.InternalId = cardData.Id = strArray[index];
      }
    }
    this.gotDictionary = true;
  }

  [PunRPC]
  private void NET_SaveCardDeck(
    string _type,
    string _arr0,
    string _arr1,
    string _arr2,
    string _arr3)
  {
    List<string> stringList1 = new List<string>();
    if (_arr0 != "")
      stringList1.AddRange((IEnumerable<string>) JsonHelper.FromJson<string>(Functions.DecompressString(_arr0)));
    List<string> stringList2 = new List<string>();
    if (_arr1 != "")
      stringList2.AddRange((IEnumerable<string>) JsonHelper.FromJson<string>(Functions.DecompressString(_arr1)));
    List<string> stringList3 = new List<string>();
    if (_arr2 != "")
      stringList3.AddRange((IEnumerable<string>) JsonHelper.FromJson<string>(Functions.DecompressString(_arr2)));
    List<string> stringList4 = new List<string>();
    if (_arr3 != "")
      stringList4.AddRange((IEnumerable<string>) JsonHelper.FromJson<string>(Functions.DecompressString(_arr3)));
    if (_type == "Hero")
    {
      this.HeroDeck[0] = stringList1;
      this.HeroDeck[1] = stringList2;
      this.HeroDeck[2] = stringList3;
      this.HeroDeck[3] = stringList4;
      this.gotHeroDeck = true;
    }
    else
    {
      this.NPCDeck[0] = stringList1;
      this.NPCDeck[1] = stringList2;
      this.NPCDeck[2] = stringList3;
      this.NPCDeck[3] = stringList4;
      this.gotNPCDeck = true;
    }
  }

  private void SaveCardDataIntoDictionary(string _id)
  {
    CardData card = this.cardDictionary[_id];
    if (!((UnityEngine.Object) card != (UnityEngine.Object) null))
      return;
    this.photonView.RPC("NET_SaveCardDataIntoDictionary", RpcTarget.Others, (object) JsonUtility.ToJson((object) card));
  }

  [PunRPC]
  private void NET_SaveCardDataIntoDictionary(string _id, string _theCard)
  {
    CardData objectToOverwrite = new CardData();
    JsonUtility.FromJsonOverwrite(_theCard, (object) objectToOverwrite);
    this.cardDictionary[_id] = objectToOverwrite;
  }

  public void ResetCastCardNum()
  {
    if (this.preCastNum == -1)
      return;
    int preCastNum = this.preCastNum;
    this.preCastNum = -1;
    int index = preCastNum != 0 ? preCastNum - 1 : 9;
    if (index > -1 && index < this.cardItemTable.Count && (UnityEngine.Object) this.cardItemTable[index] != (UnityEngine.Object) null)
      this.cardItemTable[index].OnMouseExit();
    this.ShowCombatKeyboardByConfig();
  }

  public void CastCardNum(int num)
  {
    if (this.heroActive == -1 || this.gameBusy || this.isBeginTournPhase || this.autoEndCount < 10 || this.characterWindow.IsActive())
      return;
    bool flag = false;
    Transform transform = (Transform) null;
    if (!this.controllerClickedCard)
      this.keyClickedCard = true;
    if (this.preCastNum > -1)
    {
      transform = this.GetTargetByNum(num);
      num = this.preCastNum;
    }
    if (num > this.cardItemTable.Count)
      return;
    CardItem theCardItem = this.cardItemTable[num - 1];
    if ((UnityEngine.Object) theCardItem == (UnityEngine.Object) null || (UnityEngine.Object) theCardItem.CardData == (UnityEngine.Object) null || !this.IsThereAnyTargetForCard(theCardItem.CardData) || !theCardItem.CardData.Playable)
    {
      this.ResetCastCardNum();
    }
    else
    {
      if (theCardItem.GetEnergyCost() > this.GetHeroEnergy())
        return;
      CardData cardData = theCardItem.CardData;
      if (!this.TeamHero[this.heroActive].CanPlayCard(cardData))
      {
        this.ResetCastCardNum();
      }
      else
      {
        if (this.CanInstaCast(cardData))
          flag = true;
        else if ((UnityEngine.Object) transform != (UnityEngine.Object) null)
        {
          flag = this.CheckTarget(transform, cardData);
          if (!flag)
          {
            this.ResetCastCardNum();
            return;
          }
        }
        else
        {
          this.preCastNum = num;
          this.ShowCombatKeyboard(true);
          int preCastNum = this.preCastNum;
          int index1 = preCastNum != 0 ? preCastNum - 1 : 9;
          if (index1 > -1 && index1 < this.cardItemTable.Count && (UnityEngine.Object) this.cardItemTable[index1] != (UnityEngine.Object) null)
          {
            for (int index2 = 0; index2 < this.cardItemTable.Count; ++index2)
            {
              if (index2 != index1)
                this.cardItemTable[index2].RestoreCard();
            }
            this.cardItemTable[index1].OnMouseEnter();
          }
        }
        if (!flag)
          return;
        this.CardDrag = false;
        this.CardActiveT = (Transform) null;
        this.canKeyboardCast = false;
        this.ResetAutoEndCount();
        this.ResetCastCardNum();
        this.ShowCombatKeyboard(false);
        this.cardActive = theCardItem.CardData;
        this.targetTransform = transform;
        this.StartCoroutine(this.CastCard(theCardItem));
      }
    }
  }

  private void CastCardPropagation(
    CardItem theCardItem = null,
    bool automatic = false,
    CardData card = null,
    int energy = -1,
    int tablePosition = -1)
  {
    if ((UnityEngine.Object) card == (UnityEngine.Object) null && (UnityEngine.Object) theCardItem != (UnityEngine.Object) null)
      card = theCardItem.GetCardData();
    if ((UnityEngine.Object) card != (UnityEngine.Object) null && (card.AutoplayDraw || card.AutoplayEndTurn))
      return;
    string str = "";
    if ((UnityEngine.Object) this.targetTransform != (UnityEngine.Object) null)
      str = this.targetTransform.gameObject.name;
    this.GetRandomString();
    int randomIndex = this.randomIndex;
    this.SetRandomIndex(randomIndex);
    int num = 11;
    if ((UnityEngine.Object) theCardItem != (UnityEngine.Object) null)
    {
      for (int index = 0; index < this.cardItemTable.Count; ++index)
      {
        if (this.cardItemTable[index].gameObject.name == theCardItem.gameObject.name)
        {
          num = index;
          break;
        }
      }
    }
    this.photonView.RPC("NET_CastCardPropagation", RpcTarget.Others, (object) randomIndex, (object) (short) num, (object) automatic, (object) (short) energy, (object) (short) tablePosition, (object) str);
  }

  [PunRPC]
  private void NET_CastCardPropagation(
    int _randomIndex,
    short __theCardItemIndex,
    bool _automatic,
    short __energy,
    short __tablePosition,
    string _targetTransform)
  {
    this.SetDamagePreview(false);
    this.SetOverDeck(false);
    int index = (int) __theCardItemIndex;
    int num1 = (int) __energy;
    int num2 = (int) __tablePosition;
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("[NET_CastCardPropagation] " + index.ToString(), "net");
    CardItem theCardItem = index >= 11 ? (CardItem) null : (this.cardItemTable == null || !((UnityEngine.Object) this.cardItemTable[index] != (UnityEngine.Object) null) ? (CardItem) null : this.cardItemTable[index]);
    this.SetRandomIndex(_randomIndex);
    bool _automatic1 = _automatic;
    CardData cardData1 = new CardData();
    CardData cardData2 = !((UnityEngine.Object) theCardItem != (UnityEngine.Object) null) ? (CardData) null : theCardItem.GetCardData();
    int _energy = num1;
    int _posInTable = num2;
    this.targetTransform = !(_targetTransform == "") ? this.targetTransformDict[_targetTransform] : (Transform) null;
    if ((UnityEngine.Object) cardData2 != (UnityEngine.Object) null)
      this.SetCardActive(cardData2);
    else if ((UnityEngine.Object) theCardItem != (UnityEngine.Object) null && (UnityEngine.Object) theCardItem.CardData != (UnityEngine.Object) null)
      this.SetCardActive(theCardItem.CardData);
    this.StartCoroutine(this.CastCard(theCardItem, _automatic1, cardData2, _energy, _posInTable));
  }

  private IEnumerator BeginMatch()
  {
    this.UpdateBossNpc();
    GameManager.Instance.SceneLoaded();
    Action loadCombatFinished = this.OnLoadCombatFinished;
    if (loadCombatFinished != null)
      loadCombatFinished();
    string lower = AtOManager.Instance.GetTownZoneId().ToLower();
    if (GameManager.Instance.IsMultiplayer() && Globals.Instance.ZoneDataSource.ContainsKey(lower) && !Globals.Instance.ZoneDataSource[lower].ChangeTeamOnEntrance)
    {
      this.emoteManager.gameObject.SetActive(true);
      this.emoteManager.Init();
    }
    else
      this.emoteManager.gameObject.SetActive(false);
    if (GameManager.Instance.IsMultiplayer())
    {
      while (!this.gotHeroDeck || !this.gotNPCDeck || !this.gotDictionary)
      {
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD(this.gotHeroDeck.ToString() + "||" + this.gotNPCDeck.ToString() + "||" + this.gotDictionary.ToString());
        yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      }
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("**************************", "net");
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("WaitingSyncro readyForMatch", "net");
      if (NetworkManager.Instance.IsMaster())
      {
        while (!NetworkManager.Instance.AllPlayersReady("readyForMatch"))
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("Game ready, Everybody checked readyForMatch", "net");
        NetworkManager.Instance.PlayersNetworkContinue("readyForMatch");
      }
      else
      {
        NetworkManager.Instance.SetWaitingSyncro("readyForMatch", true);
        NetworkManager.Instance.SetStatusReady("readyForMatch");
        while (NetworkManager.Instance.WaitingSyncro["readyForMatch"])
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("readyForMatch, we can continue!", "net");
      }
    }
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("BeginMatch combatGameCode =>" + AtOManager.Instance.combatGameCode);
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("Current Round => " + this.currentRound.ToString());
    int eventExaust;
    if (AtOManager.Instance.combatGameCode != "")
    {
      eventExaust = 0;
      while (this.eventList.Count > 0)
      {
        if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
        {
          this.eventListDbg = "";
          for (int index = 0; index < this.eventList.Count; ++index)
            this.eventListDbg = this.eventListDbg + this.eventList[index] + " ";
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[BeginMatch] Waiting For Eventlist to clean");
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD(this.eventListDbg);
        }
        ++eventExaust;
        if (eventExaust > 200)
        {
          this.ClearEventList();
          break;
        }
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      }
    }
    else
    {
      this.backingDictionary = true;
      this.BackupCardDictionary();
      while (this.backingDictionary)
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
    }
    for (int index = 0; index < 4; ++index)
    {
      if (this.TeamHero[index] != null)
        this.TeamHero[index].Corruption = "";
    }
    if ((UnityEngine.Object) this.corruptionCard != (UnityEngine.Object) null)
    {
      string id = this.corruptionCard.Id;
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("CORRUPTIONCARD ->" + this.corruptionCard.Id);
      this.corruptionCardId = this.CreateCardInDictionary(id);
      this.corruptionItem = this.GetCardData(this.corruptionCardId).Item;
      if (!this.corruptionItem.OnlyAddItemToNPCs)
      {
        for (int index = 0; index < 4; ++index)
        {
          if (this.TeamHero[index] != null)
            this.TeamHero[index].Corruption = id;
        }
      }
      if (this.currentRound == 0)
      {
        for (int index = 0; index < 4; ++index)
        {
          if (this.TeamNPC[index] != null && (UnityEngine.Object) this.TeamNPC[index].NpcData != (UnityEngine.Object) null)
          {
            this.TeamNPC[index].Corruption = id;
            if (this.corruptionItem.MaxHealth > 0)
            {
              this.TeamNPC[index].Hp += this.corruptionItem.MaxHealth;
              this.TeamNPC[index].HpCurrent += this.corruptionItem.MaxHealth;
              this.TeamNPC[index].SetHP();
            }
          }
        }
      }
    }
    yield return (object) Globals.Instance.WaitForSeconds(0.1f);
    this.SetInitiatives();
    if (GameManager.Instance.IsMultiplayer())
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("**************************", "net");
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("WaitingSyncro beginmatch", "net");
      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      if (NetworkManager.Instance.IsMaster())
      {
        while (!NetworkManager.Instance.AllPlayersReady("beginmatch"))
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("Game ready, Everybody checked beginmatch", "net");
        NetworkManager.Instance.PlayersNetworkContinue("beginmatch");
      }
      else
      {
        NetworkManager.Instance.SetWaitingSyncro("beginmatch", true);
        NetworkManager.Instance.SetStatusReady("beginmatch");
        while (NetworkManager.Instance.WaitingSyncro["beginmatch"])
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("beginmatch, we can continue!", "net");
      }
    }
    this.CleanTempTransform();
    if ((UnityEngine.Object) this.combatData != (UnityEngine.Object) null)
    {
      this.InitThermoPieces(this.combatData);
      if (AtOManager.Instance.combatGameCode == "")
      {
        this.SetCombatDataEffects(this.combatData);
        yield return (object) Globals.Instance.WaitForSeconds(0.4f);
      }
      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
    }
    if (MadnessManager.Instance.IsMadnessTraitActive("randomcombats") || GameManager.Instance.IsObeliskChallenge() || AtOManager.Instance.IsChallengeTraitActive("randomcombats"))
    {
      for (int index1 = 0; index1 < this.TeamNPC.Length; ++index1)
      {
        NPC npc = this.TeamNPC[index1];
        if (npc != null && npc.Alive)
        {
          if (npc.NpcData.IsNamed)
          {
            if (!GameManager.Instance.IsObeliskChallenge() || Globals.Instance.GetNodeData(AtOManager.Instance.currentMapNode).NodeCombatTier != Enums.CombatTier.T8 && Globals.Instance.GetNodeData(AtOManager.Instance.currentMapNode).NodeCombatTier != Enums.CombatTier.T9)
            {
              if (this.immunityListForNamedSaved.Count == 0)
              {
                string auraCurseImmune = Functions.GetAuraCurseImmune(npc.NpcData);
                bool flag = true;
                if (auraCurseImmune != "")
                {
                  if (auraCurseImmune == "bleed" && AtOManager.Instance.TeamHavePerk("mainperkbleed2c"))
                    flag = false;
                  else if (auraCurseImmune == "sight" && AtOManager.Instance.TeamHavePerk("mainperksight1c"))
                    flag = false;
                  else if (npc.AuracurseImmune.Contains(auraCurseImmune))
                    flag = false;
                  if (flag)
                  {
                    npc.AuracurseImmune.Add(auraCurseImmune);
                    this.immunityListForNamedSaved.Add(auraCurseImmune);
                  }
                }
              }
              else
              {
                for (int index2 = 0; index2 < this.immunityListForNamedSaved.Count; ++index2)
                {
                  if (!npc.AuracurseImmune.Contains(this.immunityListForNamedSaved[index2]))
                    npc.AuracurseImmune.Add(this.immunityListForNamedSaved[index2]);
                }
              }
            }
            if (!GameManager.Instance.IsObeliskChallenge() && AtOManager.Instance.IsZoneAffectedByMadness() && this.currentRound == 0)
            {
              if (AtOManager.Instance.GetTownTier() == 0)
              {
                npc.Hp += 20;
                npc.HpCurrent += 20;
                npc.SetHP();
              }
              else if (AtOManager.Instance.GetTownTier() == 1)
              {
                npc.Hp += 40;
                npc.HpCurrent += 40;
                npc.SetHP();
              }
              else if (AtOManager.Instance.GetTownTier() == 2)
              {
                npc.Hp += 60;
                npc.HpCurrent += 60;
                npc.SetHP();
              }
              else if (AtOManager.Instance.GetTownTier() == 3)
              {
                npc.Hp += 80;
                npc.HpCurrent += 80;
                npc.SetHP();
              }
            }
          }
          if (AtOManager.Instance.NodeIsObeliskFinal() && this.currentRound == 0)
          {
            if (AtOManager.Instance.GetObeliskMadness() > 8 && AtOManager.Instance.IsZoneAffectedByMadness())
            {
              npc.Hp -= Functions.FuncRoundToInt((float) npc.Hp * 0.15f);
              npc.HpCurrent -= Functions.FuncRoundToInt((float) npc.HpCurrent * 0.15f);
            }
            else
            {
              npc.Hp -= Functions.FuncRoundToInt((float) npc.Hp * 0.2f);
              npc.HpCurrent -= Functions.FuncRoundToInt((float) npc.HpCurrent * 0.2f);
            }
            npc.SetHP();
          }
        }
      }
    }
    this.synchronizing.gameObject.SetActive(false);
    this.ShowMask(false);
    yield return (object) Globals.Instance.WaitForSeconds(0.35f);
    this.MaskWindow.gameObject.SetActive(false);
    if (AtOManager.Instance.combatGameCode == "" || !this.roundBegan && this.currentRound == 0)
    {
      for (int index = 0; index < this.TeamHero.Length; ++index)
      {
        if (this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroData == (UnityEngine.Object) null))
          this.TeamHero[index].SetEvent(Enums.EventActivation.PreBeginCombat);
      }
      for (int index = 0; index < this.TeamNPC.Length; ++index)
      {
        if (this.TeamNPC[index] != null && !((UnityEngine.Object) this.TeamNPC[index].NpcData == (UnityEngine.Object) null))
          this.TeamNPC[index].SetEvent(Enums.EventActivation.PreBeginCombat);
      }
      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      eventExaust = 0;
      while (this.eventList.Count > 0)
      {
        if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
        {
          this.eventListDbg = "";
          for (int index = 0; index < this.eventList.Count; ++index)
            this.eventListDbg = this.eventListDbg + this.eventList[index] + " || ";
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[PreBeginCombat] Waiting For Eventlist to clean");
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD(this.eventListDbg);
        }
        ++eventExaust;
        if (eventExaust > 300)
        {
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[PreBeginCombat] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
          this.ClearEventList();
          break;
        }
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      }
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("BEGIN COMBAT EVENT");
      int i;
      for (i = 0; i < this.TeamHero.Length; ++i)
      {
        if (this.TeamHero[i] != null && !((UnityEngine.Object) this.TeamHero[i].HeroData == (UnityEngine.Object) null))
        {
          this.TeamHero[i].SetEvent(Enums.EventActivation.BeginCombat);
          yield return (object) Globals.Instance.WaitForSeconds(0.1f);
        }
      }
      for (i = 0; i < this.TeamNPC.Length; ++i)
      {
        if (this.TeamNPC[i] != null && !((UnityEngine.Object) this.TeamNPC[i].NpcData == (UnityEngine.Object) null))
        {
          this.TeamNPC[i].SetEvent(Enums.EventActivation.BeginCombat);
          yield return (object) Globals.Instance.WaitForSeconds(0.1f);
        }
      }
      eventExaust = 0;
      while (this.eventList.Count > 0)
      {
        if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
        {
          this.eventListDbg = "";
          for (int index = 0; index < this.eventList.Count; ++index)
            this.eventListDbg = this.eventListDbg + this.eventList[index] + " || ";
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[BeginCombat] Waiting For Eventlist to clean");
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD(this.eventListDbg);
        }
        ++eventExaust;
        if (eventExaust > 300)
        {
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[BeginCombat] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
          this.ClearEventList();
          break;
        }
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      }
    }
    this.SetInitiatives();
    if (AtOManager.Instance.corruptionAccepted)
      this.iconCorruption.transform.gameObject.SetActive(true);
    if (GameManager.Instance.IsMultiplayer())
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("**************************", "net");
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("WaitingSyncro abouttobegin", "net");
      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      if (NetworkManager.Instance.IsMaster())
      {
        while (!NetworkManager.Instance.AllPlayersReady("abouttobegin"))
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("Game ready, Everybody checked abouttobegin", "net");
        NetworkManager.Instance.PlayersNetworkContinue("abouttobegin");
      }
      else
      {
        NetworkManager.Instance.SetWaitingSyncro("abouttobegin", true);
        NetworkManager.Instance.SetStatusReady("abouttobegin");
        while (NetworkManager.Instance.WaitingSyncro["abouttobegin"])
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("abouttobegin, we can continue!", "net");
      }
    }
    if (GameManager.Instance.CheatMode && GameManager.Instance.UseImmortal)
    {
      int num = 99999;
      foreach (Hero hero in this.TeamHero)
      {
        hero.Hp = num;
        hero.HpCurrent = num;
        hero.SetHP();
      }
    }
    this.combatLoading = false;
    this.NextTurnFunction();
    yield return (object) null;
  }

  private bool TryGetNPCsWithMasterReweaverAbility(
    CardData data,
    Character casterCharacter,
    out List<NPC> npcsWithAbility)
  {
    npcsWithAbility = new List<NPC>();
    if (casterCharacter == null || !casterCharacter.Alive || !(casterCharacter is Hero) || (UnityEngine.Object) casterCharacter.HeroData == (UnityEngine.Object) null || !this.IsFightCard(data) || data.Corrupted)
      return false;
    foreach (NPC npc in this.TeamNPC)
    {
      if (npc != null && npc.Alive)
      {
        foreach (string masterReweaverKey in CardUtils.MasterReweaverKeys)
        {
          if (npc.HasEnchantment(masterReweaverKey))
          {
            npcsWithAbility.Add(npc);
            break;
          }
        }
      }
    }
    return npcsWithAbility.Count > 0;
  }

  private void SetEventDirect(string theEvent, bool automatic = true, bool add = false)
  {
    if (automatic)
    {
      if (!this.eventList.Contains(theEvent))
      {
        Functions.DebugLogGD("^^^^^^^^^ SetEventDirect [ON] " + theEvent + " ^^^^^^^^^^^", "trace");
        this.eventList.Add(theEvent);
      }
      else
      {
        Functions.DebugLogGD("^^^^^^^^^ SetEventDirect [OFF] " + theEvent + " ^^^^^^^^^^^", "trace");
        this.eventList.Remove(theEvent);
      }
    }
    else if (add)
    {
      if (this.eventList.Contains(theEvent))
        return;
      Functions.DebugLogGD("^^^^^^^^^ SetEventDirect [ON] " + theEvent + " ^^^^^^^^^^^", "trace");
      this.eventList.Add(theEvent);
    }
    else
    {
      if (!this.eventList.Contains(theEvent))
        return;
      Functions.DebugLogGD("^^^^^^^^^ SetEventDirect [OFF] " + theEvent + " ^^^^^^^^^^^", "trace");
      this.eventList.Remove(theEvent);
    }
  }

  public void SetEventCo(
    Character theChar,
    Enums.EventActivation theEvent,
    Character target = null,
    int auxInt = 0,
    string auxString = "",
    Character caster = null)
  {
    this.StartCoroutine(this.SetEventCoAction(theChar, theEvent, target, auxInt, auxString, caster));
  }

  private IEnumerator SetEventCoAction(
    Character theChar,
    Enums.EventActivation theEvent,
    Character target = null,
    int auxInt = 0,
    string auxString = "",
    Character caster = null)
  {
    while (this.waitingDeathScreen)
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("[SETEVENTACTION] waitingDeathScreen");
      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
    }
    if (theChar == null || !theChar.Alive)
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("[SETEVENTACTION] break by death");
      this.ClearEventList();
    }
    else
    {
      string eventCode = new StringBuilder().Append(theChar.Id).Append(auxInt).Append(auxString).Append(theEvent.ToString()).ToString();
      if (!this.eventList.Contains(eventCode))
        this.eventList.Add(eventCode);
      if (theEvent != Enums.EventActivation.BeginTurn || !theChar.HasEffectSkipsTurn())
      {
        if (theChar.IsHero && theChar.ActivateTrait(theEvent, target, auxInt, auxString))
        {
          yield return (object) Globals.Instance.WaitForSeconds(0.2f);
          for (; this.generatedCardTimes > 0; --this.generatedCardTimes)
            yield return (object) Globals.Instance.WaitForSeconds(0.5f);
        }
        else
        {
          Character character = target;
          if ((character != null ? (character.IsHero ? 1 : 0) : 0) != 0 && theEvent == Enums.EventActivation.AuraCurseSet)
            target.ActivateTrait(theEvent, target, auxInt, auxString);
        }
        if (this.generatedCardTimes < 0)
          this.generatedCardTimes = 0;
        theChar.ActivateItem(theEvent, target, auxInt, auxString);
        if (this.waitingItemTrait)
        {
          while (this.waitingItemTrait)
          {
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("[" + theEvent.ToString() + "] waitingItemTrait " + this.waitingItemTrait.ToString());
            this.waitingItemTrait = false;
            yield return (object) Globals.Instance.WaitForSeconds(0.1f);
          }
        }
        this.generatedCardTimes = 0;
      }
      theChar.FinishSetEvent(theEvent, target, auxInt, auxString);
      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      if (this.eventList.Contains(eventCode))
        this.eventList.Remove(eventCode);
    }
  }

  private void SetCombatDataEffects(CombatData _combatData)
  {
    if (_combatData.HealHeroes)
    {
      for (int index = 0; index < this.TeamHero.Length; ++index)
      {
        if (this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroData == (UnityEngine.Object) null))
          this.TeamHero[index].HealToMax();
      }
    }
    for (int index1 = 0; index1 < _combatData.CombatEffect.Length; ++index1)
    {
      CombatEffect combatEffect = _combatData.CombatEffect[index1];
      AuraCurseData auraCurse = combatEffect.AuraCurse;
      if (!((UnityEngine.Object) auraCurse == (UnityEngine.Object) null))
      {
        bool flag1 = combatEffect.AuraCurseTarget == Enums.CombatUnit.All;
        bool flag2 = combatEffect.AuraCurseTarget == Enums.CombatUnit.Heroes;
        bool flag3 = combatEffect.AuraCurseTarget == Enums.CombatUnit.Monsters;
        for (int index2 = 0; index2 < this.TeamHero.Length; ++index2)
        {
          if (this.TeamHero[index2] != null && !((UnityEngine.Object) this.TeamHero[index2].HeroData == (UnityEngine.Object) null))
          {
            bool flag4 = false;
            if (flag1 | flag2)
              flag4 = true;
            else if (index2 == 0 && combatEffect.AuraCurseTarget == Enums.CombatUnit.Hero0)
              flag4 = true;
            else if (index2 == 1 && combatEffect.AuraCurseTarget == Enums.CombatUnit.Hero1)
              flag4 = true;
            else if (index2 == 2 && combatEffect.AuraCurseTarget == Enums.CombatUnit.Hero2)
              flag4 = true;
            else if (index2 == 3 && combatEffect.AuraCurseTarget == Enums.CombatUnit.Hero3)
              flag4 = true;
            if (flag4)
              this.TeamHero[index2].SetAuraTrait((Character) null, auraCurse.Id, combatEffect.AuraCurseCharges);
          }
        }
        for (int index3 = 0; index3 < this.TeamNPC.Length; ++index3)
        {
          if (this.TeamNPC[index3] != null && !((UnityEngine.Object) this.TeamNPC[index3].NpcData == (UnityEngine.Object) null))
          {
            bool flag5 = false;
            if (flag1 | flag3)
              flag5 = true;
            else if (index3 == 0 && combatEffect.AuraCurseTarget == Enums.CombatUnit.Monster0)
              flag5 = true;
            else if (index3 == 1 && combatEffect.AuraCurseTarget == Enums.CombatUnit.Monster1)
              flag5 = true;
            else if (index3 == 2 && combatEffect.AuraCurseTarget == Enums.CombatUnit.Monster2)
              flag5 = true;
            else if (index3 == 3 && combatEffect.AuraCurseTarget == Enums.CombatUnit.Monster3)
              flag5 = true;
            if (flag5)
              this.TeamNPC[index3].SetAuraTrait((Character) null, auraCurse.Id, combatEffect.AuraCurseCharges);
          }
        }
      }
    }
  }

  public void CleanTempTransform()
  {
    foreach (Component component in this.tempTransform)
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
  }

  public void CleanTempVanishedTransform()
  {
    foreach (Component component in this.tempVanishedTransform)
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
  }

  [PunRPC]
  private void NET_Endturn()
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("NET_EndTurn", "trace");
    this.EndTurn();
  }

  public void EndTurn(bool forceIt = false)
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("[ENDTURN]", "trace");
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("[ENDTURN] GameStatus->" + this.gameStatus, "trace");
    Debug.Log((object) nameof (EndTurn));
    this.ShowHandMask(true);
    if (this.botEndTurn.gameObject.activeSelf)
      this.botEndTurn.gameObject.SetActive(false);
    if (this.cursorArrow.gameObject.activeSelf)
      this.cursorArrow.gameObject.SetActive(false);
    this.heroTurn = false;
    if (this.gameStatus == nameof (EndTurn) && !forceIt)
    {
      if (!Globals.Instance.ShowDebug)
        return;
      Functions.DebugLogGD("[ENDTURN] ** exit because gameStatus == EndTurn", "trace");
    }
    else
    {
      this.gameStatus = nameof (EndTurn);
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD(this.gameStatus, "gamestatus");
      this.ShowEnergyCounter(false);
      this.ClearCanInstaCastDict();
      this.ResetCastCardNum();
      this.ResetCharactersPing();
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("[ENDTURN] Call EndTurnWait", "trace");
      if (GameManager.Instance.IsMultiplayer() && this.IsYourTurn())
        this.photonView.RPC("NET_Endturn", RpcTarget.Others);
      this.StartCoroutine(this.EndTurnWait());
    }
  }

  private IEnumerator EndTurnWait()
  {
    MatchManager matchManager = this;
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("[ENDTURNWAIT]", "trace");
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("[ENDTURNWAIT] GameStatus->" + matchManager.gameStatus, "trace");
    string codeGenCheck = matchManager.GenerateSyncCodeForCheckingAction();
    yield return (object) Globals.Instance.WaitForSeconds(0.1f);
    int _logEntryIndex = matchManager.logDictionary.Count;
    matchManager.CreateLogEntry(true, "endeffects:" + _logEntryIndex.ToString(), "", matchManager.theHero, matchManager.theNPC, (Hero) null, (NPC) null, matchManager.currentRound, Enums.EventActivation.EndTurn);
    if (matchManager.theHero != null && matchManager.theHero.Alive)
      matchManager.theHero.SetEvent(Enums.EventActivation.EndTurn);
    if (matchManager.characterKilled)
      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
    int eventExaust = 0;
    while (matchManager.eventList.Count > 0)
    {
      if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
      {
        matchManager.eventListDbg = "";
        for (int index = 0; index < matchManager.eventList.Count; ++index)
          matchManager.eventListDbg = matchManager.eventListDbg + matchManager.eventList[index] + " || ";
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[ENDTURNWAIT] Waiting For Eventlist to clean", "trace");
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD(matchManager.eventListDbg, "trace");
      }
      ++eventExaust;
      if (eventExaust > 400)
      {
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[ENDTURNWAIT] Beak by eventexaust", "trace");
        matchManager.ClearEventList();
        matchManager.generatedCardTimes = 0;
      }
      yield return (object) Globals.Instance.WaitForSeconds(0.02f);
    }
    yield return (object) Globals.Instance.WaitForSeconds(0.1f);
    string codeGen = matchManager.GenerateSyncCodeForCheckingAction();
    int exaustCodeGen = 0;
    while (codeGen != codeGenCheck)
    {
      codeGen = codeGenCheck;
      yield return (object) Globals.Instance.WaitForSeconds(0.02f);
      codeGenCheck = matchManager.GenerateSyncCodeForCheckingAction();
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("** CHECK " + (codeGen == codeGenCheck).ToString() + " **", "trace");
      ++exaustCodeGen;
      if (exaustCodeGen > 300)
        codeGen = codeGenCheck;
    }
    if (matchManager.MatchIsOver)
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("[ENDTURNWAIT] Broken by finish game", "trace");
    }
    else
    {
      matchManager.gameStatus = "EndTurn";
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD(matchManager.gameStatus, "gamestatus");
      matchManager.CleanTempTransform();
      matchManager.CleanTempVanishedTransform();
      matchManager.cardActive = (CardData) null;
      matchManager.cardGos.Clear();
      matchManager.cardIteration.Clear();
      matchManager.ShowTraitInfo(false, true);
      if (matchManager.theHero != null)
      {
        for (int index = matchManager.cardItemTable.Count - 1; index >= 0; --index)
        {
          if ((UnityEngine.Object) matchManager.cardItemTable[index] != (UnityEngine.Object) null)
          {
            matchManager.cardItemTable[index].DrawBorder("");
            matchManager.cardItemTable[index].active = false;
            matchManager.cardItemTable[index].DisableCollider();
          }
        }
        matchManager.SetGameBusy(false);
        matchManager.StartCoroutine(matchManager.MoveItemsOut(true));
        matchManager.theHero.RoundMoved = matchManager.currentRound;
        if ((UnityEngine.Object) matchManager.theHero.HeroItem != (UnityEngine.Object) null)
          matchManager.theHero.HeroItem.ActivateMark(false);
        eventExaust = 0;
        while (matchManager.CountHeroHand() > 0)
        {
          CardItem cardItem = matchManager.cardItemTable[0];
          if ((UnityEngine.Object) cardItem != (UnityEngine.Object) null)
          {
            cardItem.DiscardCard(true, auxIndex: matchManager.CountHeroDiscard() + 1);
            eventExaust = 0;
          }
          if (GameManager.Instance.IsMultiplayer())
            yield return (object) Globals.Instance.WaitForSeconds(0.01f);
          else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
            yield return (object) Globals.Instance.WaitForSeconds(0.02f);
          else
            yield return (object) null;
          ++eventExaust;
          if (eventExaust > 250)
            matchManager.HeroHand[matchManager.heroActive].Clear();
        }
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[ENDTURNCO] MoveDecksOut", "trace");
        matchManager.MovingDeckPile = true;
        matchManager.HideExhaust();
        if (matchManager.heroActive > -1)
        {
          for (int index = 0; index < matchManager.HeroDeck[matchManager.heroActive].Count; ++index)
          {
            if (matchManager.cardDictionary.ContainsKey(matchManager.HeroDeck[matchManager.heroActive][index]) && (UnityEngine.Object) matchManager.cardDictionary[matchManager.HeroDeck[matchManager.heroActive][index]] != (UnityEngine.Object) null)
              matchManager.cardDictionary[matchManager.HeroDeck[matchManager.heroActive][index]].ResetExhaust();
          }
          for (int index = 0; index < matchManager.HeroDeckDiscard[matchManager.heroActive].Count; ++index)
          {
            if (matchManager.cardDictionary.ContainsKey(matchManager.HeroDeckDiscard[matchManager.heroActive][index]) && (UnityEngine.Object) matchManager.cardDictionary[matchManager.HeroDeckDiscard[matchManager.heroActive][index]] != (UnityEngine.Object) null)
              matchManager.cardDictionary[matchManager.HeroDeckDiscard[matchManager.heroActive][index]].ResetExhaust();
          }
          for (int index = 0; index < matchManager.HeroDeckVanish[matchManager.heroActive].Count; ++index)
          {
            if (matchManager.cardDictionary.ContainsKey(matchManager.HeroDeckVanish[matchManager.heroActive][index]) && (UnityEngine.Object) matchManager.cardDictionary[matchManager.HeroDeckVanish[matchManager.heroActive][index]] != (UnityEngine.Object) null)
              matchManager.cardDictionary[matchManager.HeroDeckVanish[matchManager.heroActive][index]].ResetExhaust();
          }
        }
        matchManager.StartCoroutine(matchManager.MoveDecksOut(true));
        while (matchManager.MovingDeckPile)
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[ENDTURNCO] MoveDecksOut END", "trace");
        matchManager.waitExecution = true;
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[ENDTURNCO] ActionsCharacterEndTurn", "trace");
        matchManager.ActionsCharacterEndTurn();
        eventExaust = 0;
        while (matchManager.waitExecution)
        {
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
          ++eventExaust;
          if (eventExaust > 300)
          {
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("[ENDTURNCO] Waitexecution EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
            matchManager.waitExecution = false;
          }
        }
        if (matchManager.characterKilled)
          yield return (object) Globals.Instance.WaitForSeconds(0.1f);
        eventExaust = 0;
        while (matchManager.eventList.Count > 0)
        {
          if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
          {
            matchManager.eventListDbg = "";
            for (int index = 0; index < matchManager.eventList.Count; ++index)
              matchManager.eventListDbg = matchManager.eventListDbg + matchManager.eventList[index] + " || ";
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("[ActionsCharacterEndTurn] Waiting For Eventlist to clean", "trace");
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD(matchManager.eventListDbg);
          }
          ++eventExaust;
          if (eventExaust > 200)
          {
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("[ActionsCharacterEndTurn] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
            matchManager.ClearEventList();
            break;
          }
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        }
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[ENDTURNCO] END ActionsCharacterEndTurn");
      }
      else if (matchManager.theNPC != null)
      {
        if (matchManager.theNPC != null && matchManager.theNPC.Alive && (UnityEngine.Object) matchManager.theNPC.NPCItem != (UnityEngine.Object) null)
          matchManager.theNPC.NPCItem.MoveToCenterBack();
        if (matchManager.theNPC.RoundMoved != matchManager.currentRound)
        {
          matchManager.theNPC.RoundMoved = matchManager.currentRound;
          if ((UnityEngine.Object) matchManager.theNPC.NPCItem != (UnityEngine.Object) null)
            matchManager.theNPC.NPCItem.ActivateMark(false);
          matchManager.theNPC.DiscardHand();
          matchManager.waitExecution = true;
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[ENDTURNCO] ActionsCharacterEndTurn", "trace");
          matchManager.ActionsCharacterEndTurn();
          eventExaust = 0;
          while (matchManager.waitExecution)
          {
            yield return (object) Globals.Instance.WaitForSeconds(0.01f);
            ++eventExaust;
            if (eventExaust > 400)
            {
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("[ENDTURNCO] Waitexecution EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
              matchManager.waitExecution = false;
            }
          }
          yield return (object) Globals.Instance.WaitForSeconds(0.1f);
          eventExaust = 0;
          while (matchManager.eventList.Count > 0)
          {
            if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
            {
              matchManager.eventListDbg = "";
              for (int index = 0; index < matchManager.eventList.Count; ++index)
                matchManager.eventListDbg = matchManager.eventListDbg + matchManager.eventList[index] + " || ";
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("[ActionsCharacterEndTurn] Waiting For Eventlist to clean");
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD(matchManager.eventListDbg);
            }
            ++eventExaust;
            if (eventExaust > 300)
            {
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("[ActionsCharacterEndTurn] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
              matchManager.ClearEventList();
              break;
            }
            yield return (object) Globals.Instance.WaitForSeconds(0.01f);
          }
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[ENDTURNCO] END ActionsCharacterEndTurn");
        }
      }
      matchManager.heroActive = -1;
      matchManager.npcActive = -1;
      matchManager.theHeroPreAutomatic = (Hero) null;
      matchManager.theNPCPreAutomatic = (NPC) null;
      matchManager.HideExhaust();
      matchManager.SetGlobalOutlines(false);
      matchManager.SetDamagePreview(false);
      matchManager.SetOverDeck(false);
      matchManager.CreateLogEntry(false, "endeffects:" + _logEntryIndex.ToString(), "", matchManager.theHero, matchManager.theNPC, (Hero) null, (NPC) null, matchManager.currentRound, Enums.EventActivation.EndTurn);
      matchManager.logDictionaryAux = new Dictionary<string, LogEntry>();
      foreach (KeyValuePair<string, LogEntry> log in matchManager.logDictionary)
        matchManager.logDictionaryAux.Add(log.Key, log.Value);
      matchManager.NextTurnFunction();
    }
  }

  public bool IsGameBusy() => this.gameBusy;

  private void SetGameBusy(bool state)
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("********** SETGAMEBUSY (" + state.ToString() + ") *********", "gamebusy");
    this.gameBusy = state;
  }

  public int GameRound() => this.currentRound;

  internal void NextTurnFunction()
  {
    this.StartCoroutine(this.NextTurn());
    if (!GameManager.Instance.CheatMode || !GameManager.Instance.WinMatchOnStart)
      return;
    this.FinishCombat();
  }

  private IEnumerator NextTurn()
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("[NEXTTURN]");
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("[NEXTTURN] GameStatus->" + this.gameStatus);
    if (this.gameStatus == nameof (NextTurn) && Globals.Instance.ShowDebug)
      Functions.DebugLogGD("[NEXTTURN] *** EXIT because GameStatus == NextTurn");
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("[NEXTTURN] ASSIGN GameStatus = NextTurn");
    this.gameStatus = nameof (NextTurn);
    this.cursorArrow.StopDraw();
    PopupManager.Instance.ClosePopup();
    if (this.characterKilled)
      yield return (object) Globals.Instance.WaitForSeconds(0.3f);
    for (int index = 0; index < this.TeamNPC.Length; ++index)
    {
      if (this.TeamNPC[index] != null && this.TeamNPC[index].Alive && (UnityEngine.Object) this.TeamNPC[index].NPCItem == (UnityEngine.Object) null)
        this.TeamNPC[index] = (NPC) null;
    }
    while (this.waitingDeathScreen)
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("[NEXTTURN] waitingDeathScreen");
      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
    }
    while (this.waitingKill)
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("[NEXTTURN] waitingKill");
      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
    }
    if (this.CheckMatchIsOver())
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("[NEXTTURN] FINISHCOMBAT");
      this.FinishCombat();
    }
    else
    {
      this.CleanTempTransform();
      this.SortCharacterSprites();
      int _queueIndex = 0;
      while (this.ctQueue.Count > 0)
      {
        yield return (object) Globals.Instance.WaitForSeconds(0.1f);
        if (_queueIndex > 50)
          this.ctQueue.Clear();
      }
      this.castingCardListMP.Clear();
      int eventExaust = 0;
      while (this.eventList.Count > 0)
      {
        if (GameManager.Instance.GetDeveloperMode() && eventExaust % 1 == 0)
        {
          this.eventListDbg = "";
          for (int index = 0; index < this.eventList.Count; ++index)
            this.eventListDbg = this.eventListDbg + this.eventList[index] + " || ";
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[NEXTTURN] Waiting For Eventlist to clean");
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD(this.eventListDbg);
        }
        ++eventExaust;
        if (eventExaust > 300)
        {
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[NEXTTURN] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
          this.ClearEventList();
          break;
        }
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      }
      this.characterKilled = false;
      this.NextTurnAfterClean();
    }
  }

  private void NextTurnAfterClean()
  {
    if (this.beforeSyncCodeLocked)
      return;
    this.beforeSyncCodeLocked = true;
    this.StopCoroutines();
    if (GameManager.Instance.IsMultiplayer())
    {
      this.StartCoroutine("NextTurnSyncro");
    }
    else
    {
      this.SetMasterSyncCode();
      this.currentGameCodeForReload = this.currentGameCode;
      this.StartCoroutine("NextTurnContinue");
    }
  }

  private IEnumerator NextTurnSyncro()
  {
    MatchManager matchManager = this;
    yield return (object) Globals.Instance.WaitForSeconds(0.01f);
    if (matchManager.currentRound == 0)
      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
    if (NetworkManager.Instance.IsMaster())
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("+++ Checking +++", "synccode");
      matchManager.SetMasterSyncCode(true);
      int syncExhaust = 0;
      while (matchManager.CheckSyncCodeDict() == 0)
      {
        if (GameManager.Instance.GetDeveloperMode() && syncExhaust % 50 == 0 && Globals.Instance.ShowDebug)
          Functions.DebugLogGD("Waiting For CheckSyncCodeDict in MD5", "synccode");
        ++syncExhaust;
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (syncExhaust > 500)
        {
          matchManager.ReloadCombat("CheckSyncCodeDict exhausted time");
          yield break;
        }
      }
      string theCode1 = Functions.Md5Sum(matchManager.GenerateSyncCode());
      string theCode = "";
      if (matchManager.AllDesyncEqual(theCode1))
      {
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("MD5 codes OK continue", "synccode");
      }
      else
      {
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("MD5 codes were different", "synccode");
        matchManager.GenerateSyncCodeDict();
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        matchManager.SetMasterSyncCode();
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        matchManager.RequestSyncCode();
        if (GameManager.Instance.GetDeveloperMode())
        {
          Debug.LogError((object) "There was an error with the sync codes");
          Debug.Log((object) matchManager.currentGameCode);
        }
        syncExhaust = 0;
        while (matchManager.CheckSyncCodeDict() == 0)
        {
          if (GameManager.Instance.GetDeveloperMode() && syncExhaust % 50 == 0 && Globals.Instance.ShowDebug)
            Functions.DebugLogGD("Waiting For CheckSyncCodeDict plain", "synccode");
          ++syncExhaust;
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        }
        switch (matchManager.DesyncFixable())
        {
          case 0:
            theCode = matchManager.GenerateSyncCode();
            if (GameManager.Instance.GetDeveloperMode())
            {
              Debug.Log((object) "Fixable OK");
              Debug.Log((object) ("Code -> " + theCode));
              break;
            }
            break;
          case 2:
            string[] array1 = new string[matchManager.cardDictionary.Count];
            matchManager.cardDictionary.Keys.CopyTo(array1, 0);
            string json1 = JsonHelper.ToJson<string>(array1);
            MatchManager.CardDataForShare[] array2 = new MatchManager.CardDataForShare[matchManager.cardDictionary.Count];
            int index = 0;
            foreach (KeyValuePair<string, CardData> card in matchManager.cardDictionary)
            {
              array2[index] = new MatchManager.CardDataForShare()
              {
                vanish = card.Value.Vanish,
                energyReductionPermanent = card.Value.EnergyReductionPermanent,
                energyReductionTemporal = card.Value.EnergyReductionTemporal,
                energyReductionToZeroPermanent = card.Value.EnergyReductionToZeroPermanent,
                energyReductionToZeroTemporal = card.Value.EnergyReductionToZeroTemporal
              };
              ++index;
            }
            string json2 = JsonHelper.ToJson<MatchManager.CardDataForShare>(array2);
            Debug.LogError((object) "[CheckSyncCode] dictionary redone");
            matchManager.photonView.RPC("NET_SaveCardDictionary", RpcTarget.Others, (object) Functions.CompressString(json1), (object) Functions.CompressString(json2));
            theCode = matchManager.currentGameCode;
            yield return (object) Globals.Instance.WaitForSeconds(1f);
            break;
          default:
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("NOT Fixable - ReloadCombat", "synccode");
            matchManager.ReloadCombat("[CheckSyncCode] NOT Fixable - ReloadCombat");
            yield break;
        }
      }
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("Game ready, Everybody checked FixingSyncCode", "synccode");
      matchManager.SetRandomIndex(matchManager.randomIndex);
      if (theCode == "")
        matchManager.photonView.RPC("NET_FixCodeSyncFromMaster", RpcTarget.Others, (object) matchManager.randomIndex, (object) "");
      else
        matchManager.photonView.RPC("NET_FixCodeSyncFromMaster", RpcTarget.Others, (object) matchManager.randomIndex, (object) Functions.CompressString(theCode));
      if (matchManager.coroutineSyncFixSyncCode != null)
        matchManager.StopCoroutine(matchManager.coroutineSyncFixSyncCode);
      matchManager.coroutineSyncFixSyncCode = matchManager.StartCoroutine(matchManager.ReloadCombatCo("FixingSyncCode"));
      while (!NetworkManager.Instance.AllPlayersReady("FixingSyncCode"))
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      if (matchManager.coroutineSyncFixSyncCode != null)
        matchManager.StopCoroutine(matchManager.coroutineSyncFixSyncCode);
      NetworkManager.Instance.PlayersNetworkContinue("FixingSyncCode");
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("FixingSyncCode Received continue", "net");
      theCode = (string) null;
    }
    else
    {
      NetworkManager.Instance.SetWaitingSyncro("FixingSyncCode", true);
      matchManager.SendSyncCode(true);
      while (NetworkManager.Instance.WaitingSyncro["FixingSyncCode"])
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("FixingSyncCode, we can continue!", "net");
    }
    matchManager.currentGameCodeForReload = matchManager.currentGameCode;
    matchManager.NextTurnContinuePlain();
  }

  private void NextTurnContinuePlain() => this.StartCoroutine(this.NextTurnContinue());

  private IEnumerator NextTurnContinue()
  {
    MatchManager matchManager = this;
    AtOManager.Instance.SaveGameTurn();
    matchManager.backingDictionary = true;
    matchManager.BackupCardDictionary();
    while (matchManager.backingDictionary)
      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
    if (matchManager.logDictionary.Count > 0)
      matchManager.CreateLogEntry(true, "", "", matchManager.theHero, matchManager.theNPC, (Hero) null, (NPC) null, matchManager.currentRound, Enums.EventActivation.EndTurn);
    int eventExaust = 0;
    matchManager.CombatTextIterations = 0;
    matchManager.generatedCardTimes = 0;
    matchManager.ResetFailCount();
    if (matchManager.theHero != null && matchManager.theHero.Alive)
    {
      matchManager.CleanPrePostDamageDictionary(matchManager.theHero.Id);
      if ((UnityEngine.Object) matchManager.theHero.HeroItem != (UnityEngine.Object) null)
        matchManager.theHero.HeroItem.CalculateDamagePrePostForThisCharacter();
    }
    if (matchManager.theNPC != null && matchManager.theNPC.Alive)
    {
      matchManager.CleanPrePostDamageDictionary(matchManager.theNPC.Id);
      if ((UnityEngine.Object) matchManager.theNPC.NPCItem != (UnityEngine.Object) null)
        matchManager.theNPC.NPCItem.CalculateDamagePrePostForThisCharacter();
    }
    matchManager.theHero = (Hero) null;
    matchManager.theNPC = (NPC) null;
    bool flag1 = true;
    for (int index = 0; index < matchManager.CharOrder.Count; ++index)
    {
      if (matchManager.CharOrder[index].hero != null)
      {
        if (matchManager.CharOrder[index].hero.RoundMoved < matchManager.currentRound && matchManager.CharOrder[index].hero.Alive)
        {
          flag1 = false;
          break;
        }
      }
      else if (matchManager.CharOrder[index].npc.RoundMoved < matchManager.currentRound && matchManager.CharOrder[index].npc.Alive)
      {
        flag1 = false;
        break;
      }
    }
    if (flag1)
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("CURRENT ROUND -> " + matchManager.currentRound.ToString(), "trace");
      int i;
      if (matchManager.currentRound > 0)
      {
        for (i = 0; i < matchManager.TeamHero.Length; ++i)
        {
          if (matchManager.TeamHero[i] != null && !((UnityEngine.Object) matchManager.TeamHero[i].HeroData == (UnityEngine.Object) null) && matchManager.TeamHero[i].Alive)
          {
            matchManager.waitExecution = true;
            matchManager.TeamHero[i].EndRound();
            eventExaust = 0;
            while (matchManager.waitExecution)
            {
              yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              ++eventExaust;
              if (eventExaust > 400)
              {
                if (Globals.Instance.ShowDebug)
                  Functions.DebugLogGD("[ENDTURNCO] Waitexecution EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
                matchManager.waitExecution = false;
              }
            }
          }
        }
        for (i = 0; i < matchManager.TeamNPC.Length; ++i)
        {
          if (matchManager.TeamNPC[i] != null && !((UnityEngine.Object) matchManager.TeamNPC[i].NpcData == (UnityEngine.Object) null) && matchManager.TeamNPC[i].Alive)
          {
            matchManager.waitExecution = true;
            matchManager.TeamNPC[i].EndRound();
            eventExaust = 0;
            while (matchManager.waitExecution)
            {
              yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              ++eventExaust;
              if (eventExaust > 400)
              {
                if (Globals.Instance.ShowDebug)
                  Functions.DebugLogGD("[ENDTURNCO] Waitexecution EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
                matchManager.waitExecution = false;
              }
            }
          }
        }
      }
      if (matchManager.MatchIsOver)
      {
        yield break;
      }
      else
      {
        int scarabType;
        if (matchManager.currentRound == 0)
        {
          if ((UnityEngine.Object) matchManager.corruptionItem != (UnityEngine.Object) null && matchManager.corruptionItem.Activation == Enums.EventActivation.CorruptionCombatStart)
          {
            CardData cardData = matchManager.GetCardData(matchManager.corruptionCardId);
            cardData.EnergyCost = 0;
            cardData.Vanish = true;
            matchManager.GenerateNewCard(1, matchManager.corruptionCardId, false, Enums.CardPlace.Vanish);
            for (int index = 0; index < 4; ++index)
            {
              if (matchManager.TeamNPC[index] != null && matchManager.TeamNPC[index].Alive)
              {
                matchManager.TeamNPC[index].DoItem(Enums.EventActivation.CorruptionCombatStart, cardData, matchManager.corruptionItem.Id, (Character) null, 0, "", 0, (CardData) null);
                break;
              }
            }
            yield return (object) Globals.Instance.WaitForSeconds(1.5f);
            matchManager.SetGameBusy(false);
          }
        }
        else
        {
          bool flag2 = false;
          if (matchManager.scarabSpawned == "" && (UnityEngine.Object) matchManager.combatData != (UnityEngine.Object) null && (UnityEngine.Object) Globals.Instance.GetNodeData(AtOManager.Instance.currentMapNode) != (UnityEngine.Object) null)
          {
            switch (Globals.Instance.GetNodeData(AtOManager.Instance.currentMapNode).NodeCombatTier)
            {
              case Enums.CombatTier.T2:
                if (GameManager.Instance.IsObeliskChallenge())
                {
                  flag2 = true;
                  break;
                }
                break;
              case Enums.CombatTier.T3:
                flag2 = true;
                break;
              case Enums.CombatTier.T4:
                flag2 = true;
                break;
              case Enums.CombatTier.T5:
                flag2 = true;
                break;
              case Enums.CombatTier.T6:
                if (GameManager.Instance.IsObeliskChallenge())
                {
                  flag2 = true;
                  break;
                }
                break;
              case Enums.CombatTier.T7:
                if (GameManager.Instance.IsObeliskChallenge())
                {
                  flag2 = true;
                  break;
                }
                break;
            }
          }
          if (flag2)
          {
            i = matchManager.GetNPCAvailablePosition();
            if (i > -1)
            {
              int randomIntRange = matchManager.GetRandomIntRange(0, 100);
              int num1 = 7;
              if (AtOManager.Instance.TeamHaveItem("scarabyrare"))
                num1 += 40;
              else if (AtOManager.Instance.TeamHaveItem("scaraby"))
                num1 += 30;
              int num2 = num1;
              if (randomIntRange < num2)
              {
                bool flag3 = false;
                scarabType = 0;
                for (; !flag3; flag3 = !AtOManager.Instance.TeamHaveItem("scarabyrare") || scarabType < 3)
                  scarabType = matchManager.GetRandomIntRange(0, 4);
                string str = "";
                if (!GameManager.Instance.IsObeliskChallenge())
                {
                  if (AtOManager.Instance.GetTownTier() == 2)
                    str = AtOManager.Instance.GetNgPlus() <= 0 ? "_b" : "_plus_b";
                  else if (AtOManager.Instance.GetNgPlus() > 0)
                    str = "_plus";
                }
                else if (AtOManager.Instance.GetObeliskMadness() > 8)
                  str = matchManager.combatData.CombatTier == Enums.CombatTier.T6 || matchManager.combatData.CombatTier == Enums.CombatTier.T7 ? "_plus_b" : "_plus";
                else if (matchManager.combatData.CombatTier == Enums.CombatTier.T6 || matchManager.combatData.CombatTier == Enums.CombatTier.T7)
                  str = "_b";
                matchManager.scarabSpawned = scarabType != 0 ? (scarabType != 1 ? (scarabType != 2 ? "scourgescarab" : "jadescarab") : "goldenscarab") : "crystalscarab";
                NPCData npc = Globals.Instance.GetNPC(matchManager.scarabSpawned + str);
                if ((UnityEngine.Object) npc == (UnityEngine.Object) null)
                {
                  Debug.LogError((object) ("scarabData Null for scarab => " + matchManager.scarabSpawned + str));
                }
                else
                {
                  matchManager.CreateNPC(npc, _position: i);
                  yield return (object) Globals.Instance.WaitForSeconds(0.5f);
                  if (scarabType != 3)
                    matchManager.TeamNPC[i].SetAura((Character) null, Globals.Instance.GetAuraCurseData("luckyscarab"), 1);
                  matchManager.TeamNPC[i].SetAura((Character) null, Globals.Instance.GetAuraCurseData("sight"), 3);
                  if (scarabType == 1)
                  {
                    if (AtOManager.Instance.GetNgPlus() == 0)
                    {
                      if (AtOManager.Instance.GetTownTier() <= 1)
                        matchManager.TeamNPC[i].SetAura((Character) null, Globals.Instance.GetAuraCurseData("zeal"), 5);
                      else
                        matchManager.TeamNPC[i].SetAura((Character) null, Globals.Instance.GetAuraCurseData("zeal"), 7);
                    }
                    else if (AtOManager.Instance.GetTownTier() <= 1)
                      matchManager.TeamNPC[i].SetAura((Character) null, Globals.Instance.GetAuraCurseData("zeal"), 6);
                    else
                      matchManager.TeamNPC[i].SetAura((Character) null, Globals.Instance.GetAuraCurseData("zeal"), 7);
                  }
                  else if (scarabType == 2)
                  {
                    if (AtOManager.Instance.GetNgPlus() == 0)
                    {
                      if (AtOManager.Instance.GetTownTier() <= 1)
                      {
                        matchManager.TeamNPC[i].SetAura((Character) null, Globals.Instance.GetAuraCurseData("evasion"), 5);
                        matchManager.TeamNPC[i].SetAura((Character) null, Globals.Instance.GetAuraCurseData("buffer"), 5);
                      }
                      else
                      {
                        matchManager.TeamNPC[i].SetAura((Character) null, Globals.Instance.GetAuraCurseData("evasion"), 7);
                        matchManager.TeamNPC[i].SetAura((Character) null, Globals.Instance.GetAuraCurseData("buffer"), 7);
                      }
                    }
                    else if (AtOManager.Instance.GetTownTier() <= 1)
                    {
                      matchManager.TeamNPC[i].SetAura((Character) null, Globals.Instance.GetAuraCurseData("evasion"), 6);
                      matchManager.TeamNPC[i].SetAura((Character) null, Globals.Instance.GetAuraCurseData("buffer"), 6);
                    }
                    else
                    {
                      matchManager.TeamNPC[i].SetAura((Character) null, Globals.Instance.GetAuraCurseData("evasion"), 7);
                      matchManager.TeamNPC[i].SetAura((Character) null, Globals.Instance.GetAuraCurseData("buffer"), 7);
                    }
                  }
                  GameManager.Instance.PlayLibraryAudio("glitter", 0.25f);
                  yield return (object) Globals.Instance.WaitForSeconds(0.5f);
                }
              }
            }
          }
        }
        ++matchManager.currentRound;
        matchManager.CreateLogEntry(true, "round:" + matchManager.currentRound.ToString(), "", (Hero) null, (NPC) null, (Hero) null, (NPC) null, matchManager.currentRound, Enums.EventActivation.CorruptionBeginRound);
        matchManager.activatedTraitsRound.Clear();
        if ((UnityEngine.Object) matchManager.combatData != (UnityEngine.Object) null && (MadnessManager.Instance.IsMadnessTraitActive("impedingdoom") || AtOManager.Instance.IsChallengeTraitActive("impedingdoom")))
        {
          ThermometerTierData thermometerTierData = matchManager.combatData.ThermometerTierData;
          if ((UnityEngine.Object) thermometerTierData != (UnityEngine.Object) null)
          {
            for (int index1 = 0; index1 < thermometerTierData.RoundThermometer.Length; ++index1)
            {
              if (thermometerTierData.RoundThermometer[index1] != null && matchManager.currentRound >= thermometerTierData.RoundThermometer[index1].Round)
              {
                if (thermometerTierData.RoundThermometer[index1].Round == matchManager.currentRound)
                {
                  ThermometerData thermometerData = thermometerTierData.RoundThermometer[index1].ThermometerData;
                  if (Globals.Instance.ShowDebug)
                    Functions.DebugLogGD(thermometerData.ThermometerId + "<------------");
                  if ((UnityEngine.Object) thermometerData != (UnityEngine.Object) null && thermometerData.ThermometerId.ToLower() == "underwhelming")
                  {
                    for (int index2 = 0; index2 < matchManager.TeamHero.Length; ++index2)
                    {
                      if (matchManager.TeamHero[index2] != null && (UnityEngine.Object) matchManager.TeamHero[index2].HeroData != (UnityEngine.Object) null)
                      {
                        if (GameManager.Instance.IsObeliskChallenge())
                          matchManager.TeamHero[index2].SetAura((Character) null, Globals.Instance.GetAuraCurseData("doom"), 3);
                        else
                          matchManager.TeamHero[index2].SetAura((Character) null, Globals.Instance.GetAuraCurseData("doom"), 2);
                      }
                    }
                    break;
                  }
                  break;
                }
                if (thermometerTierData.RoundThermometer[index1].Round > matchManager.currentRound)
                  break;
              }
            }
          }
        }
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[CORRUPTIONBEGINROUND]");
        if ((UnityEngine.Object) matchManager.corruptionItem != (UnityEngine.Object) null && matchManager.corruptionItem.Activation == Enums.EventActivation.CorruptionBeginRound)
        {
          matchManager.CreateLogEntry(true, matchManager.corruptionItem.Id + matchManager.currentRound.ToString(), matchManager.corruptionItem.Id, (Hero) null, (NPC) null, (Hero) null, (NPC) null, matchManager.currentRound, Enums.EventActivation.CorruptionBeginRound);
          if (GameManager.Instance.IsMultiplayer())
          {
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("**************************", "net");
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("WaitingSyncro CorruptionBeginRoundPre", "net");
            if (NetworkManager.Instance.IsMaster())
            {
              if (matchManager.coroutineSyncBeginRound != null)
                matchManager.StopCoroutine(matchManager.coroutineSyncBeginRound);
              matchManager.coroutineSyncBeginRound = matchManager.StartCoroutine(matchManager.ReloadCombatCo("CorruptionBeginRoundPre"));
              while (!NetworkManager.Instance.AllPlayersReady("CorruptionBeginRoundPre"))
                yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              if (matchManager.coroutineSyncBeginRound != null)
                matchManager.StopCoroutine(matchManager.coroutineSyncBeginRound);
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("Game ready, Everybody checked CorruptionBeginRoundPre", "net");
              matchManager.SetRandomIndex(matchManager.randomIndex);
              NetworkManager.Instance.PlayersNetworkContinue("CorruptionBeginRoundPre", matchManager.randomIndex.ToString());
            }
            else
            {
              NetworkManager.Instance.SetWaitingSyncro("CorruptionBeginRoundPre", true);
              NetworkManager.Instance.SetStatusReady("CorruptionBeginRoundPre");
              while (NetworkManager.Instance.WaitingSyncro["CorruptionBeginRoundPre"])
                yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              if (NetworkManager.Instance.netAuxValue != "")
                matchManager.SetRandomIndex(int.Parse(NetworkManager.Instance.netAuxValue));
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("CorruptionBeginRoundPre, we can continue!", "net");
            }
          }
          switch (matchManager.corruptionItem.ItemTarget)
          {
            case Enums.ItemTarget.Self:
            case Enums.ItemTarget.RandomHero:
            case Enums.ItemTarget.AllHero:
              switch (matchManager.corruptionItem.ItemTarget)
              {
                case Enums.ItemTarget.Self:
                case Enums.ItemTarget.AllHero:
                  if (Globals.Instance.ShowDebug)
                    Functions.DebugLogGD("corr0");
                  for (int index = 0; index < 4; ++index)
                  {
                    if (matchManager.TeamHero[index] != null && matchManager.TeamHero[index].Alive)
                      matchManager.TeamHero[index].SetEvent(Enums.EventActivation.CorruptionBeginRound);
                  }
                  break;
                default:
                  if (Globals.Instance.ShowDebug)
                    Functions.DebugLogGD("corr1");
                  bool flag4 = false;
                  while (!flag4)
                  {
                    int randomIntRange = matchManager.GetRandomIntRange(0, 4);
                    if (matchManager.TeamHero[randomIntRange] != null && matchManager.TeamHero[randomIntRange].Alive)
                    {
                      matchManager.TeamHero[randomIntRange].SetEvent(Enums.EventActivation.CorruptionBeginRound);
                      flag4 = true;
                    }
                  }
                  break;
              }
              break;
            default:
              switch (matchManager.corruptionItem.ItemTarget)
              {
                case Enums.ItemTarget.RandomEnemy:
                case Enums.ItemTarget.AllEnemy:
                case Enums.ItemTarget.SelfEnemy:
                  switch (matchManager.corruptionItem.ItemTarget)
                  {
                    case Enums.ItemTarget.AllEnemy:
                    case Enums.ItemTarget.SelfEnemy:
                      if (Globals.Instance.ShowDebug)
                        Functions.DebugLogGD("corr2");
                      for (int index = 0; index < 4; ++index)
                      {
                        if (matchManager.TeamNPC[index] != null && matchManager.TeamNPC[index].Alive)
                          matchManager.TeamNPC[index].SetEvent(Enums.EventActivation.CorruptionBeginRound);
                      }
                      break;
                    default:
                      if (Globals.Instance.ShowDebug)
                        Functions.DebugLogGD("corr3");
                      bool flag5 = false;
                      while (!flag5)
                      {
                        int randomIntRange = matchManager.GetRandomIntRange(0, 4);
                        if (matchManager.TeamNPC[randomIntRange] != null && matchManager.TeamNPC[randomIntRange].Alive)
                        {
                          matchManager.TeamNPC[randomIntRange].SetEvent(Enums.EventActivation.CorruptionBeginRound);
                          flag5 = true;
                        }
                      }
                      break;
                  }
                  break;
              }
              break;
          }
          yield return (object) Globals.Instance.WaitForSeconds(0.2f);
          for (i = 0; matchManager.generatedCardTimes > 0 && i < 200; ++i)
            yield return (object) Globals.Instance.WaitForSeconds(0.01f);
          yield return (object) Globals.Instance.WaitForSeconds(0.1f);
          yield return (object) Globals.Instance.WaitForSeconds(0.1f);
          scarabType = 0;
          while (matchManager.eventList.Count > 0)
          {
            if (GameManager.Instance.GetDeveloperMode() && scarabType % 50 == 0)
            {
              matchManager.eventListDbg = "";
              for (int index = 0; index < matchManager.eventList.Count; ++index)
                matchManager.eventListDbg = matchManager.eventListDbg + matchManager.eventList[index] + " || ";
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("[CorruptionWAIT] Waiting For Eventlist to clean");
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD(matchManager.eventListDbg);
            }
            ++scarabType;
            if (scarabType > 300)
            {
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("[CorruptionWAIT] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
              matchManager.ClearEventList();
              break;
            }
            yield return (object) Globals.Instance.WaitForSeconds(0.01f);
          }
          if (GameManager.Instance.IsMultiplayer())
          {
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("**************************", "net");
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("WaitingSyncro CorruptionBeginRound", "net");
            if (NetworkManager.Instance.IsMaster())
            {
              if (matchManager.coroutineSyncBeginRound != null)
                matchManager.StopCoroutine(matchManager.coroutineSyncBeginRound);
              matchManager.coroutineSyncBeginRound = matchManager.StartCoroutine(matchManager.ReloadCombatCo("CorruptionBeginRound"));
              while (!NetworkManager.Instance.AllPlayersReady("CorruptionBeginRound"))
                yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              if (matchManager.coroutineSyncBeginRound != null)
                matchManager.StopCoroutine(matchManager.coroutineSyncBeginRound);
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("Game ready, Everybody checked CorruptionBeginRound", "net");
              matchManager.SetRandomIndex(matchManager.randomIndex);
              NetworkManager.Instance.PlayersNetworkContinue("CorruptionBeginRound", matchManager.randomIndex.ToString());
            }
            else
            {
              NetworkManager.Instance.SetWaitingSyncro("CorruptionBeginRound", true);
              NetworkManager.Instance.SetStatusReady("CorruptionBeginRound");
              while (NetworkManager.Instance.WaitingSyncro["CorruptionBeginRound"])
                yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              if (NetworkManager.Instance.netAuxValue != "")
                matchManager.SetRandomIndex(int.Parse(NetworkManager.Instance.netAuxValue));
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("CorruptionBeginRound, we can continue!", "net");
            }
            yield return (object) Globals.Instance.WaitForSeconds(0.1f);
          }
          matchManager.CreateLogEntry(false, matchManager.corruptionItem.Id + matchManager.currentRound.ToString(), matchManager.corruptionItem.Id, (Hero) null, (NPC) null, (Hero) null, (NPC) null, matchManager.currentRound, Enums.EventActivation.CorruptionBeginRound);
        }
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[CORRUPTIONBEGINROUND] END", "trace");
        matchManager.ClearItemExecuteForThisTurn();
        matchManager.ItemExecuteForThisCombatDuplicate();
        for (scarabType = 0; scarabType < matchManager.TeamHero.Length; ++scarabType)
        {
          if (matchManager.TeamHero[scarabType] != null && !((UnityEngine.Object) matchManager.TeamHero[scarabType].HeroData == (UnityEngine.Object) null) && matchManager.TeamHero[scarabType].Alive)
          {
            matchManager.waitExecution = true;
            matchManager.TeamHero[scarabType].BeginRound();
            eventExaust = 0;
            while (matchManager.waitExecution)
            {
              yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              ++eventExaust;
              if (eventExaust > 400)
              {
                if (Globals.Instance.ShowDebug)
                  Functions.DebugLogGD("[BeginRound] Waitexecution EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
                matchManager.waitExecution = false;
              }
            }
            if (matchManager.TeamHero[scarabType].Alive)
              matchManager.TeamHero[scarabType].SetEvent(Enums.EventActivation.BeginRound);
          }
        }
        for (scarabType = 0; scarabType < matchManager.TeamNPC.Length; ++scarabType)
        {
          if (matchManager.TeamNPC[scarabType] != null && !((UnityEngine.Object) matchManager.TeamNPC[scarabType].NpcData == (UnityEngine.Object) null) && matchManager.TeamNPC[scarabType].Alive)
          {
            matchManager.waitExecution = true;
            matchManager.TeamNPC[scarabType].BeginRound();
            eventExaust = 0;
            while (matchManager.waitExecution)
            {
              yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              ++eventExaust;
              if (eventExaust > 400)
              {
                if (Globals.Instance.ShowDebug)
                  Functions.DebugLogGD("[BeginRound] Waitexecution EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
                matchManager.waitExecution = false;
              }
            }
            if (matchManager.TeamNPC[scarabType].Alive)
              matchManager.TeamNPC[scarabType].SetEvent(Enums.EventActivation.BeginRound);
          }
        }
        yield return (object) Globals.Instance.WaitForSeconds(0.1f);
        if (GameManager.Instance.IsMultiplayer())
        {
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("**************************", "net");
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("WaitingSyncro beginround", "net");
          if (NetworkManager.Instance.IsMaster())
          {
            if (matchManager.coroutineSyncBeginRound != null)
              matchManager.StopCoroutine(matchManager.coroutineSyncBeginRound);
            matchManager.coroutineSyncBeginRound = matchManager.StartCoroutine(matchManager.ReloadCombatCo("beginround"));
            while (!NetworkManager.Instance.AllPlayersReady("beginround"))
              yield return (object) Globals.Instance.WaitForSeconds(0.01f);
            if (matchManager.coroutineSyncBeginRound != null)
              matchManager.StopCoroutine(matchManager.coroutineSyncBeginRound);
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("Game ready, Everybody checked beginround", "net");
            matchManager.SetRandomIndex(matchManager.randomIndex);
            NetworkManager.Instance.PlayersNetworkContinue("beginround");
          }
          else
          {
            NetworkManager.Instance.SetWaitingSyncro("beginround", true);
            NetworkManager.Instance.SetStatusReady("beginround");
            while (NetworkManager.Instance.WaitingSyncro["beginround"])
              yield return (object) Globals.Instance.WaitForSeconds(0.01f);
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("beginround, we can continue!", "net");
          }
        }
      }
    }
    eventExaust = 0;
    while (matchManager.eventList.Count > 0)
    {
      if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
      {
        matchManager.eventListDbg = "";
        for (int index = 0; index < matchManager.eventList.Count; ++index)
          matchManager.eventListDbg = matchManager.eventListDbg + matchManager.eventList[index] + " || ";
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[BeginTurn] Waiting For Eventlist to clean");
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD(matchManager.eventListDbg);
      }
      ++eventExaust;
      if (eventExaust > 300)
      {
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[BeginTurn] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        matchManager.ClearEventList();
        break;
      }
      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
    }
    int _queueIndex = 0;
    while (matchManager.ctQueue.Count > 0)
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("[ItemQueue] Waiting queue -> " + matchManager.ctQueue.Count.ToString() + " // " + _queueIndex.ToString());
      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      if (_queueIndex > 50)
        matchManager.ctQueue.Clear();
    }
    matchManager.NextTurnContinue2();
  }

  private void NextTurnContinue2()
  {
    this.StopCoroutines();
    this.ClearEventList();
    this.StoreCombatStats();
    this.StartCoroutine(this.HideComic());
    this.StartCoroutine(this.NextTurnContinue3());
  }

  private IEnumerator NextTurnContinue3()
  {
    MatchManager matchManager = this;
    yield return (object) Globals.Instance.WaitForSeconds(0.1f);
    if (!matchManager.MatchIsOver)
    {
      if (GameManager.Instance.IsMultiplayer())
      {
        if (NetworkManager.Instance.IsMaster())
          matchManager.GenerateSyncCodeDict();
        NetworkManager.Instance.ClearSyncro();
      }
      matchManager.beforeSyncCodeLocked = false;
      matchManager.castingCardBlocked.Clear();
      matchManager.activatedTraits.Clear();
      matchManager.heroDestroyedItemsInThisTurn.Clear();
      matchManager.ReDrawInitiatives();
      matchManager.CleanTempTransform();
      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      int eventExaust = 0;
      while (matchManager.eventList.Count > 0)
      {
        if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
        {
          matchManager.eventListDbg = "";
          for (int index = 0; index < matchManager.eventList.Count; ++index)
            matchManager.eventListDbg = matchManager.eventListDbg + matchManager.eventList[index] + " || ";
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[BeginTurn] Waiting For Eventlist to clean");
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD(matchManager.eventListDbg);
        }
        ++eventExaust;
        if (eventExaust > 300)
        {
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[BeginTurn] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
          matchManager.ClearEventList();
          break;
        }
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      }
      matchManager.gameStatus = "BeginTurn";
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD(matchManager.gameStatus, "gamestatus");
      matchManager.waitExecution = true;
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("[NEXTTURNCONTINUE] SetActiveCharacter");
      matchManager.SetActiveCharacter();
      matchManager.CreateLogEntry(true, "status:" + matchManager.logDictionary.Count.ToString(), "", matchManager.theHero, matchManager.theNPC, (Hero) null, (NPC) null, matchManager.currentRound, Enums.EventActivation.EndTurn);
      matchManager.CreateLogEntry(true, "", "", matchManager.theHero, matchManager.theNPC, (Hero) null, (NPC) null, matchManager.currentRound, Enums.EventActivation.BeginTurn);
      if (matchManager.theHero != null)
        matchManager.CreateLogEntry(true, "", "", matchManager.theHero, matchManager.theNPC, (Hero) null, (NPC) null, matchManager.currentRound, Enums.EventActivation.BeginTurnAboutToDealCards, matchManager.theHero.GetDrawCardsTurnForDisplayInDeck());
      if (GameManager.Instance.IsMultiplayer())
        yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
        yield return (object) Globals.Instance.WaitForSeconds(0.3f);
      else
        yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("[NEXTTURNCONTINUE] END SetActiveCharacter", "trace");
      matchManager.waitExecution = true;
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("[NEXTTURNCONTINUE] ActionsCharacterBeginTurn", "trace");
      matchManager.isBeginTournPhase = true;
      int _logEntryIndex = matchManager.logDictionary.Count;
      matchManager.CreateLogEntry(true, "begineffects:" + _logEntryIndex.ToString(), "", matchManager.theHero, matchManager.theNPC, (Hero) null, (NPC) null, matchManager.currentRound, Enums.EventActivation.BeginTurn);
      matchManager.ActionsCharacterBeginTurn();
      eventExaust = 0;
      while (matchManager.waitExecution)
      {
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        ++eventExaust;
        if (eventExaust > 400)
        {
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[ActionsCharacterBeginTurn] Waitexecution EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
          matchManager.waitExecution = false;
        }
      }
      matchManager.waitExecution = true;
      eventExaust = 0;
      matchManager.ActionsCharacterBeginTurnPerks();
      while (matchManager.waitExecution)
      {
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        ++eventExaust;
        if (eventExaust > 400)
        {
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[ActionsCharacterBeginTurn] Waitexecution EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
          matchManager.waitExecution = false;
        }
      }
      eventExaust = 0;
      while (matchManager.eventList.Count > 0 && matchManager.ctQueue.Count > 0)
      {
        if (GameManager.Instance.GetDeveloperMode() && eventExaust % 5 == 0)
        {
          matchManager.eventListDbg = "";
          for (int index = 0; index < matchManager.eventList.Count; ++index)
            matchManager.eventListDbg = matchManager.eventListDbg + matchManager.eventList[index] + " || ";
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[ActionsCharacterBeginTurn] Waiting For Eventlist to clean");
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD(matchManager.eventListDbg);
        }
        ++eventExaust;
        if (eventExaust > 60)
        {
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[ActionsCharacterBeginTurn] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
          matchManager.ClearEventList();
          matchManager.ctQueue.Clear();
          break;
        }
        yield return (object) Globals.Instance.WaitForSeconds(0.1f);
        Debug.Log((object) ("[ActionsCharacterBeginTurn] LOOP " + matchManager.eventList.Count.ToString() + "//" + matchManager.ctQueue.Count.ToString()));
      }
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("[NEXTTURNCONTINUE] END ActionsCharacterBeginTurn");
      ++matchManager.turnIndex;
      if (!GameManager.Instance.IsMultiplayer() && matchManager.turnIndex > 1 && matchManager.heroActive > -1 && !GameManager.Instance.TutorialWatched("combatSpeed"))
      {
        matchManager.waitingTutorial = true;
        yield return (object) Globals.Instance.WaitForSeconds(0.4f);
        GameManager.Instance.ShowTutorialPopup("combatSpeed", matchManager.CharOrder[0].initiativePortrait.transform.Find("Speed").transform.position, Vector3.zero);
        matchManager.characterWindow.Hide();
        while (matchManager.waitingTutorial)
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        matchManager.SetWatchingTutorial(false);
      }
      eventExaust = 0;
      while (matchManager.waitExecution)
      {
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        ++eventExaust;
        if (eventExaust > 400)
        {
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[CombatBeforeCharMoves] Waitexecution EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
          matchManager.waitExecution = false;
        }
      }
      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      eventExaust = 0;
      while (matchManager.eventList.Count > 0)
      {
        if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
        {
          matchManager.eventListDbg = "";
          for (int index = 0; index < matchManager.eventList.Count; ++index)
            matchManager.eventListDbg = matchManager.eventListDbg + matchManager.eventList[index] + " || ";
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[CombatBeforeCharMoves] Waiting For Eventlist to clean");
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD(matchManager.eventListDbg);
        }
        ++eventExaust;
        if (eventExaust > 300)
        {
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[CombatBeforeCharMoves] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
          matchManager.ClearEventList();
          break;
        }
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      }
      matchManager.CreateLogEntry(false, "begineffects:" + _logEntryIndex.ToString(), "", matchManager.theHero, matchManager.theNPC, (Hero) null, (NPC) null, matchManager.currentRound, Enums.EventActivation.BeginTurn);
      matchManager.SetGameBusy(false);
      matchManager.activationItemsAtBeginTurnList = new List<string>();
      bool exit;
      if (matchManager.heroActive != -1)
      {
        if (matchManager.theHero != null && matchManager.theHero.Alive)
        {
          if (GameManager.Instance.IsMultiplayer())
          {
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("**************************", "net");
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("WaitingSyncro beginturn", "net");
            yield return (object) Globals.Instance.WaitForSeconds(0.1f);
            if (NetworkManager.Instance.IsMaster())
            {
              if (matchManager.coroutineSync != null)
                matchManager.StopCoroutine(matchManager.coroutineSync);
              matchManager.coroutineSync = matchManager.StartCoroutine(matchManager.ReloadCombatCo("beginturn"));
              while (!NetworkManager.Instance.AllPlayersReady("beginturn"))
                yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              if (matchManager.coroutineSync != null)
                matchManager.StopCoroutine(matchManager.coroutineSync);
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("Game ready, Everybody checked beginturn", "net");
              matchManager.SetRandomIndex(matchManager.randomIndex);
              NetworkManager.Instance.PlayersNetworkContinue("beginturn", matchManager.randomIndex.ToString());
            }
            else
            {
              NetworkManager.Instance.SetWaitingSyncro("beginturn", true);
              NetworkManager.Instance.SetStatusReady("beginturn");
              while (NetworkManager.Instance.WaitingSyncro["beginturn"])
                yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              if (NetworkManager.Instance.netAuxValue != "")
                matchManager.SetRandomIndex(int.Parse(NetworkManager.Instance.netAuxValue));
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("beginturn, we can continue!", "net");
            }
          }
          matchManager.ResetItemTimeout();
          if (!matchManager.theHero.HasEffectSkipsTurn())
            matchManager.StartCoroutine(matchManager.MoveItemsOut(false));
          yield return (object) Globals.Instance.WaitForSeconds(0.2f);
          matchManager.CleanTempTransform();
          matchManager.theHero.SetEvent(Enums.EventActivation.BeginTurn);
          int num;
          if (!matchManager.theHero.HasEffectSkipsTurn() && matchManager.theHero.HaveItemToActivate(Enums.EventActivation.BeginTurn, true))
          {
            matchManager.ResetAutoEndCount();
            exit = false;
            int validTimes = 0;
            eventExaust = 0;
            while (!exit)
            {
              yield return (object) Globals.Instance.WaitForSeconds(0.2f);
              if (Globals.Instance.ShowDebug)
              {
                string[] strArray = new string[14];
                strArray[0] = "[BeginTurn] Waiting exit.... ";
                strArray[1] = matchManager.gameStatus;
                strArray[2] = "|";
                num = matchManager.NumChildsInTemporal();
                strArray[3] = num.ToString();
                strArray[4] = "|";
                strArray[5] = matchManager.waitingKill.ToString();
                strArray[6] = "|";
                strArray[7] = matchManager.waitingDeathScreen.ToString();
                strArray[8] = "|";
                num = matchManager.castingCardListMP.Count;
                strArray[9] = num.ToString();
                strArray[10] = "|";
                strArray[11] = validTimes.ToString();
                strArray[12] = "|";
                strArray[13] = eventExaust.ToString();
                Functions.DebugLogGD(string.Concat(strArray));
              }
              if (matchManager.gameStatus != "CastCardEnd" || matchManager.NumChildsInTemporal() > 0 || matchManager.waitingKill || matchManager.waitingDeathScreen || matchManager.castingCardListMP.Count > 0)
              {
                validTimes = 0;
                if (matchManager.waitingKill || matchManager.waitingDeathScreen)
                  eventExaust = 0;
                num = eventExaust++;
                if (eventExaust % 4 == 0 && Globals.Instance.ShowDebug)
                {
                  string[] strArray = new string[9];
                  strArray[0] = eventExaust.ToString();
                  strArray[1] = "||";
                  strArray[2] = matchManager.gameStatus;
                  strArray[3] = "||";
                  num = matchManager.NumChildsInTemporal();
                  strArray[4] = num.ToString();
                  strArray[5] = "||";
                  strArray[6] = matchManager.waitingKill.ToString();
                  strArray[7] = "||";
                  strArray[8] = matchManager.waitingDeathScreen.ToString();
                  Debug.Log((object) string.Concat(strArray));
                }
                if (eventExaust > 40)
                {
                  if (Globals.Instance.ShowDebug)
                  {
                    Functions.DebugLogGD("begin turn items activation break by exhaust");
                    break;
                  }
                  break;
                }
              }
              else
              {
                eventExaust = 0;
                num = validTimes++;
                if (!GameManager.Instance.IsMultiplayer())
                {
                  if (validTimes > 3)
                  {
                    if (Globals.Instance.ShowDebug)
                      Functions.DebugLogGD("validTimes > 3");
                    exit = true;
                  }
                }
                else if (validTimes > 3)
                {
                  if (Globals.Instance.ShowDebug)
                    Functions.DebugLogGD("validTimes > 3");
                  exit = true;
                }
              }
              if (matchManager.gameStatus == "FinishCombat")
                yield break;
            }
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("[BeginTurn] EXIT");
          }
          while (matchManager.waitingDeathScreen)
          {
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("[BeginTurn] waitingDeathScreen");
            yield return (object) Globals.Instance.WaitForSeconds(0.1f);
          }
          while (matchManager.waitingKill)
          {
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("[BeginTurn] Waitingforkill");
            yield return (object) Globals.Instance.WaitForSeconds(0.1f);
          }
          if (!matchManager.theHero.Alive)
          {
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("[BeginTurn] HEro is dead, wait for resurrection item");
            yield return (object) Globals.Instance.WaitForSeconds(0.3f);
            if (!matchManager.theHero.Alive)
            {
              if (!Globals.Instance.ShowDebug)
              {
                yield break;
              }
              else
              {
                Functions.DebugLogGD("[BeginTurn] break because Hero died");
                yield break;
              }
            }
            else if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("[BeginTurn] Hero have resurrected, continue");
          }
          if (matchManager.MatchIsOver)
          {
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("[BeginTurn] Broken by finish game");
          }
          else
          {
            matchManager.ReDrawInitiatives();
            matchManager.combatTarget.Refresh();
            matchManager.SortCharacterSprites(true, matchManager.heroActive);
            matchManager.cursorArrow.gameObject.SetActive(true);
            eventExaust = 0;
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("[BeginTurn] gamestatus->" + matchManager.gameStatus, "trace");
            while (matchManager.gameStatus != "BeginTurn" && matchManager.gameStatus != "CastCardEnd")
            {
              num = eventExaust++;
              if (eventExaust <= 300)
              {
                if (eventExaust % 10 == 0 && Globals.Instance.ShowDebug)
                  Functions.DebugLogGD("[BeginTurn] Waiting loop gamestatus->" + matchManager.gameStatus, "trace");
                yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              }
              else
                break;
            }
            matchManager.CleanTempTransform();
            if (!matchManager.theHero.HasEffectSkipsTurn() && matchManager.theHero.GetDrawCardsTurn() > 0)
            {
              while (matchManager.eventList.Count > 0)
                yield return (object) Globals.Instance.WaitForSeconds(0.05f);
              while (matchManager.waitingKill)
                yield return (object) Globals.Instance.WaitForSeconds(0.05f);
              while (matchManager.waitingDeathScreen)
              {
                if (Globals.Instance.ShowDebug)
                  Functions.DebugLogGD("DealNewCard -  waitingDeathScreen", "trace");
                yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              }
              if (!matchManager.theHero.Alive)
              {
                matchManager.EndTurn();
              }
              else
              {
                matchManager.gameStatus = "BeginTurnHero";
                if (Globals.Instance.ShowDebug)
                  Functions.DebugLogGD(matchManager.gameStatus, "gamestatus");
                matchManager.MovingDeckPile = true;
                matchManager.DrawDeckPile(matchManager.CountHeroDeck() + 1);
                matchManager.StartCoroutine(matchManager.MoveDecksOut(false));
                while (matchManager.MovingDeckPile)
                  yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                matchManager.SetCardsWaitingForReset(1);
                matchManager.ShowHandMask(true);
                matchManager.StartCoroutine(matchManager.DealCards());
                while (matchManager.cardsWaitingForReset > 0)
                  yield return (object) Globals.Instance.WaitForSeconds(0.1f);
                eventExaust = 0;
                while (matchManager.eventList.Count > 0)
                {
                  if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
                  {
                    matchManager.eventListDbg = "";
                    for (int index = 0; index < matchManager.eventList.Count; ++index)
                      matchManager.eventListDbg = matchManager.eventListDbg + matchManager.eventList[index] + " || ";
                    if (Globals.Instance.ShowDebug)
                      Functions.DebugLogGD("[DealCards] Waiting For Eventlist to clean", "general");
                    if (Globals.Instance.ShowDebug)
                      Functions.DebugLogGD(matchManager.eventListDbg, "general");
                  }
                  num = eventExaust++;
                  if (eventExaust > 300)
                  {
                    if (Globals.Instance.ShowDebug)
                      Functions.DebugLogGD("[DealCards] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    matchManager.ClearEventList();
                    break;
                  }
                  yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                }
                matchManager.heroTurn = true;
              }
            }
            else
            {
              if (matchManager.theHero.GetDrawCardsTurn() <= 0)
                matchManager.newTurnScript.CantDraw(matchManager.theHero.SourceName);
              else
                matchManager.newTurnScript.PassTurn(matchManager.theHero.SourceName);
              yield return (object) Globals.Instance.WaitForSeconds(1.2f);
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("[HERO SKIP TURN]", "trace");
              if (GameManager.Instance.IsMultiplayer())
              {
                if (Globals.Instance.ShowDebug)
                  Functions.DebugLogGD("**************************", "net");
                if (Globals.Instance.ShowDebug)
                  Functions.DebugLogGD("WaitingSyncro endturnbyskip", "net");
                yield return (object) Globals.Instance.WaitForSeconds(0.1f);
                if (NetworkManager.Instance.IsMaster())
                {
                  if (matchManager.coroutineSync != null)
                    matchManager.StopCoroutine(matchManager.coroutineSync);
                  matchManager.coroutineSync = matchManager.StartCoroutine(matchManager.ReloadCombatCo("endturnbyskip"));
                  while (!NetworkManager.Instance.AllPlayersReady("endturnbyskip"))
                    yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                  if (matchManager.coroutineSync != null)
                    matchManager.StopCoroutine(matchManager.coroutineSync);
                  if (Globals.Instance.ShowDebug)
                    Functions.DebugLogGD("Game ready, Everybody checked endturnbyskip", "net");
                  NetworkManager.Instance.PlayersNetworkContinue("endturnbyskip");
                }
                else
                {
                  NetworkManager.Instance.SetWaitingSyncro("endturnbyskip", true);
                  NetworkManager.Instance.SetStatusReady("endturnbyskip");
                  while (NetworkManager.Instance.WaitingSyncro["endturnbyskip"])
                    yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                  if (Globals.Instance.ShowDebug)
                    Functions.DebugLogGD("endturnbyskip, we can continue!", "net");
                }
                if (matchManager.IsYourTurn())
                  matchManager.EndTurn();
              }
              else
                matchManager.EndTurn();
            }
          }
        }
      }
      else if (matchManager.theNPC != null && matchManager.theNPC.Alive)
      {
        matchManager.theNPC.SetEvent(Enums.EventActivation.BeginTurn);
        yield return (object) Globals.Instance.WaitForSeconds(0.1f);
        eventExaust = 0;
        exit = false;
        while (!exit)
        {
          exit = true;
          while (matchManager.eventList.Count > 0)
          {
            exit = false;
            if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
            {
              matchManager.eventListDbg = "";
              for (int index = 0; index < matchManager.eventList.Count; ++index)
                matchManager.eventListDbg = matchManager.eventListDbg + matchManager.eventList[index] + " || ";
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("[NextTurnContinueAfterStopCoroutines] Waiting For Eventlist to clean");
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD(matchManager.eventListDbg);
            }
            ++eventExaust;
            yield return (object) Globals.Instance.WaitForSeconds(0.01f);
          }
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[NextTurnContinueAfterStopCoroutines] cleanExecution -> " + exit.ToString());
          if (matchManager.waitingKill)
          {
            exit = false;
            while (matchManager.waitingKill)
            {
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("[NextTurnContinueAfterStopCoroutines] Waitingforkill");
              yield return (object) Globals.Instance.WaitForSeconds(0.1f);
            }
          }
          if (!exit)
          {
            yield return (object) Globals.Instance.WaitForSeconds(0.5f);
            if (matchManager.eventList.Count == 0)
              exit = true;
            else
              eventExaust = 0;
          }
        }
        matchManager.ReDrawInitiatives();
        matchManager.SortCharacterSprites(true, npcIndex: matchManager.npcActive);
        matchManager.isBeginTournPhase = false;
        if (!matchManager.theNPC.HasEffectSkipsTurn())
        {
          matchManager.CastNPC();
        }
        else
        {
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[NPC SKIP TURN]", "trace");
          if (GameManager.Instance.IsMultiplayer())
          {
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("**************************", "net");
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("WaitingSyncro endturnbyskipnpc", "net");
            yield return (object) Globals.Instance.WaitForSeconds(0.1f);
            if (NetworkManager.Instance.IsMaster())
            {
              if (matchManager.coroutineSync != null)
                matchManager.StopCoroutine(matchManager.coroutineSync);
              matchManager.coroutineSync = matchManager.StartCoroutine(matchManager.ReloadCombatCo("endturnbyskipnpc"));
              while (!NetworkManager.Instance.AllPlayersReady("endturnbyskipnpc"))
                yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              if (matchManager.coroutineSync != null)
                matchManager.StopCoroutine(matchManager.coroutineSync);
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("Game ready, Everybody checked endturnbyskipnpc", "net");
              NetworkManager.Instance.PlayersNetworkContinue("endturnbyskipnpc");
            }
            else
            {
              NetworkManager.Instance.SetWaitingSyncro("endturnbyskipnpc", true);
              NetworkManager.Instance.SetStatusReady("endturnbyskipnpc");
              while (NetworkManager.Instance.WaitingSyncro["endturnbyskipnpc"])
                yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("endturnbyskipnpc, we can continue!", "net");
            }
            matchManager.EndTurn();
          }
          else
            matchManager.EndTurn();
        }
      }
    }
  }

  private void BeginTurnHero()
  {
    this.gameStatus = "HeroTurn";
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD(this.gameStatus, "gamestatus");
    this.castingCardBlocked.Clear();
    this.ClearEventList();
    this.StartCoroutine(this.BeginTurnHeroCo());
  }

  private IEnumerator BeginTurnHeroCo()
  {
    MatchManager matchManager = this;
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD(nameof (BeginTurnHeroCo));
    for (int index = matchManager.cardItemTable.Count - 1; index >= 0; --index)
    {
      if (index <= matchManager.cardItemTable.Count - 1 && (UnityEngine.Object) matchManager.cardItemTable[index] != (UnityEngine.Object) null)
      {
        matchManager.cardItemTable[index].discard = false;
        matchManager.cardItemTable[index].active = true;
      }
    }
    matchManager.gameStatus = "";
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD(matchManager.gameStatus, "gamestatus");
    matchManager.SetGameBusy(false);
    matchManager.RepositionCards();
    if (GameManager.Instance.IsMultiplayer())
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("**************************", "net");
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("WaitingSyncro beginturnheroco", "net");
      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      if (NetworkManager.Instance.IsMaster())
      {
        if (matchManager.coroutineSyncBeginTurnHero != null)
          matchManager.StopCoroutine(matchManager.coroutineSyncBeginTurnHero);
        matchManager.coroutineSyncBeginTurnHero = matchManager.StartCoroutine(matchManager.ReloadCombatCo("beginturnheroco"));
        while (!NetworkManager.Instance.AllPlayersReady("beginturnheroco"))
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (matchManager.coroutineSyncBeginTurnHero != null)
          matchManager.StopCoroutine(matchManager.coroutineSyncBeginTurnHero);
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("Game ready, Everybody checked beginturnheroco", "net");
        NetworkManager.Instance.PlayersNetworkContinue("beginturnheroco");
      }
      else
      {
        NetworkManager.Instance.SetWaitingSyncro("beginturnheroco", true);
        NetworkManager.Instance.SetStatusReady("beginturnheroco");
        while (NetworkManager.Instance.WaitingSyncro["beginturnheroco"])
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("beginturnheroco, we can continue!", "net");
      }
    }
    if (!GameManager.Instance.IsMultiplayer() && (!GameManager.Instance.TutorialWatched("firstTurnEnergy") || !GameManager.Instance.TutorialWatched("cardTarget")))
    {
      matchManager.waitingTutorial = true;
      CardItem cardTutorial = (CardItem) null;
      cardTutorial = matchManager.cardItemTable.Last<CardItem>();
      for (int index = matchManager.cardItemTable.Count<CardItem>() - 1; index >= 0; --index)
      {
        if (matchManager.cardItemTable[index].CardData.Damage > 0)
        {
          cardTutorial = matchManager.cardItemTable[index];
          break;
        }
      }
      Vector3 localPosition = cardTutorial.transform.localPosition;
      Quaternion rotation = cardTutorial.transform.rotation;
      cardTutorial.DisableCollider();
      yield return (object) Globals.Instance.WaitForSeconds(0.3f);
      cardTutorial.SetDestinationScaleRotation(-cardTutorial.transform.parent.transform.position, 1.4f, Quaternion.Euler(0.0f, 0.0f, 0.0f));
      yield return (object) Globals.Instance.WaitForSeconds(0.6f);
      if (!GameManager.Instance.TutorialWatched("firstTurnEnergy"))
      {
        GameManager.Instance.ShowTutorialPopup("firstTurnEnergy", matchManager.theHero.HeroItem.transform.Find("Energy/Energy_Text").transform.position, cardTutorial.transform.Find("CardGO/Energy").transform.position);
        matchManager.characterWindow.Hide();
        while (matchManager.waitingTutorial)
          yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      }
      if (!GameManager.Instance.TutorialWatched("cardTarget"))
      {
        matchManager.waitingTutorial = true;
        yield return (object) Globals.Instance.WaitForSeconds(0.2f);
        GameManager.Instance.ShowTutorialPopup("cardTarget", cardTutorial.transform.Find("CardGO/CardTargetText").transform.position, Vector3.zero);
        matchManager.characterWindow.Hide();
        while (matchManager.waitingTutorial)
          yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      }
      yield return (object) Globals.Instance.WaitForSeconds(0.2f);
      if ((UnityEngine.Object) cardTutorial != (UnityEngine.Object) null)
      {
        cardTutorial.EnableCollider();
        matchManager.RepositionCards();
      }
      if (GameManager.Instance.IsMultiplayer())
        matchManager.SetWatchingTutorial(false);
      yield return (object) Globals.Instance.WaitForSeconds(0.4f);
      cardTutorial = (CardItem) null;
    }
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("BeginTurnCardsDealt", "general");
    yield return (object) Globals.Instance.WaitForSeconds(0.02f);
    matchManager.theHero.SetEvent(Enums.EventActivation.BeginTurnCardsDealt);
    if (matchManager.theHero.HaveItemToActivate(Enums.EventActivation.BeginTurnCardsDealt))
      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
    matchManager.isBeginTournPhase = false;
  }

  private void SetWatchingTutorial(bool state)
  {
    if (!GameManager.Instance.IsMultiplayer())
      return;
    this.photonView.RPC("NET_WatchingTutorial", RpcTarget.MasterClient, (object) NetworkManager.Instance.GetPlayerNick(), (object) state);
  }

  [PunRPC]
  private void NET_WatchingTutorial(string nick, bool state)
  {
    if (state)
    {
      if (!this.playersWatchingTutorial.Contains(nick))
        this.playersWatchingTutorial.Add(nick);
    }
    else if (this.playersWatchingTutorial.Contains(nick))
      this.playersWatchingTutorial.Remove(nick);
    if (this.playersWatchingTutorial.Count == 0)
      this.photonView.RPC("NET_FinishedTutorial", RpcTarget.All, (object) false);
    else
      this.photonView.RPC("NET_FinishedTutorial", RpcTarget.All, (object) true);
  }

  [PunRPC]
  private void NET_FinishedTutorial(bool state) => this.multiplayerWatchingTutorial = state;

  private void InitThermoPieces(CombatData _combatData)
  {
    for (int index = 0; index < this.roundPieces.Length; ++index)
      this.roundPieces[index].SetThermometerTierData(_combatData.ThermometerTierData);
    if (!((UnityEngine.Object) _combatData.ThermometerTierData == (UnityEngine.Object) null))
      return;
    this.roundThermoSprite.transform.GetComponent<PolygonCollider2D>().enabled = false;
  }

  private void SetRoundText()
  {
    this.roundTransform.gameObject.SetActive(true);
    this.roundTM.text = this.currentRound <= 0 ? "" : string.Format(Texts.Instance.GetText("roundNumber"), (object) this.currentRound).ToUpper();
    if (this.currentRound > 0)
      this.initiativeRoundGO.transform.GetChild(0).transform.GetComponent<TMP_Text>().text = string.Format(Texts.Instance.GetText("roundNumber"), (object) (this.currentRound + 1)).ToUpper();
    else
      this.initiativeRoundGO.transform.GetChild(0).transform.GetComponent<TMP_Text>().text = "";
    for (int index = 0; index < this.roundPieces.Length; ++index)
      this.roundPieces[index].Init(this.currentRound);
    this.roundThermoSprite.sprite = !((UnityEngine.Object) this.combatData != (UnityEngine.Object) null) || !((UnityEngine.Object) this.combatData.ThermometerTierData != (UnityEngine.Object) null) ? this.roundThermoSpriteNull : this.roundThermoSprites[2];
    if (this.roundTM.text != "")
    {
      if (this.roundTransform.gameObject.activeSelf)
        return;
      this.roundTransform.gameObject.SetActive(true);
    }
    else
      this.roundTransform.gameObject.SetActive(false);
  }

  public void AdjustRoundForThermoDisplay(int piece, int roundForDisplay, Color colorForDisplay)
  {
    this.roundTM.text = roundForDisplay != 0 ? string.Format(Texts.Instance.GetText("roundNumber"), (object) roundForDisplay).ToUpper() : string.Format(Texts.Instance.GetText("roundNumber"), (object) this.currentRound).ToUpper();
    this.roundTM.color = colorForDisplay;
    this.roundThermoSprite.sprite = this.roundThermoSprites[piece];
  }

  public void SetWaitExecution(bool status) => this.waitExecution = status;

  private void ActionsCharacterBeginTurn()
  {
    if (this.theHero != null)
      this.theHero.BeginTurn();
    else if (this.theNPC != null)
      this.theNPC.BeginTurn();
    else
      this.waitExecution = false;
  }

  private void ActionsCharacterBeginTurnPerks()
  {
    if (this.theHero != null)
      this.theHero.BeginTurnPerks();
    else if (this.theNPC != null)
      this.theNPC.BeginTurnPerks();
    else
      this.waitExecution = false;
  }

  private void ActionsCharacterEndTurn()
  {
    if (this.theHero != null && this.theHero.Alive && (UnityEngine.Object) this.theHero.HeroItem != (UnityEngine.Object) null)
      this.theHero.EndTurn();
    else if (this.theNPC != null && this.theNPC.Alive && (UnityEngine.Object) this.theNPC.NPCItem != (UnityEngine.Object) null)
      this.theNPC.EndTurn();
    else
      this.waitExecution = false;
  }

  private void ActionsCharacterBeginRound()
  {
    for (int index = 0; index < this.TeamHero.Length; ++index)
    {
      if (this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroData == (UnityEngine.Object) null) && this.TeamHero[index].Alive)
        this.TeamHero[index].BeginRound();
    }
    for (int index = 0; index < this.TeamNPC.Length; ++index)
    {
      if (this.TeamNPC[index] != null && !((UnityEngine.Object) this.TeamNPC[index].NpcData == (UnityEngine.Object) null) && this.TeamNPC[index].Alive)
        this.TeamNPC[index].BeginRound();
    }
  }

  private void ActionsCharacterEndRound()
  {
    for (int index = 0; index < this.TeamHero.Length; ++index)
    {
      if (this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroData == (UnityEngine.Object) null) && this.TeamHero[index].Alive)
        this.TeamHero[index].EndRound();
    }
    for (int index = 0; index < this.TeamNPC.Length; ++index)
    {
      if (this.TeamNPC[index] != null && !((UnityEngine.Object) this.TeamNPC[index].NpcData == (UnityEngine.Object) null) && this.TeamNPC[index].Alive)
        this.TeamNPC[index].EndRound();
    }
  }

  public void SetInitiatives()
  {
    if (this.CharOrder != null)
    {
      for (int index = this.CharOrder.Count - 1; index >= 0; --index)
      {
        if (this.CharOrder[index] != null && (UnityEngine.Object) this.CharOrder[index].initiativePortrait != (UnityEngine.Object) null)
          UnityEngine.Object.Destroy((UnityEngine.Object) this.CharOrder[index].initiativePortrait.gameObject);
        this.CharOrder.RemoveAt(index);
      }
    }
    this.CharOrder = new List<MatchManager.CharacterForOrder>();
    for (int index = 0; index < this.TeamHero.Length; ++index)
    {
      if (this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroData == (UnityEngine.Object) null))
      {
        Hero hero = this.TeamHero[index];
        if (hero.Alive)
        {
          MatchManager.CharacterForOrder theChar = new MatchManager.CharacterForOrder()
          {
            index = index,
            id = hero.Id,
            hero = hero,
            npc = (NPC) null,
            speed = hero.GetSpeed(),
            heroItem = hero.HeroItem
          };
          theChar.speedForOrder = (float) ((double) (this.AuxiliarSumForInitiative(theChar) + theChar.speed[0]) + 0.5 + 0.0099999997764825821 * (double) (10 - index));
          this.CharOrder.Add(theChar);
        }
      }
    }
    for (int index = 0; index < 4; ++index)
    {
      if (this.TeamNPC[index] != null && !((UnityEngine.Object) this.TeamNPC[index].NpcData == (UnityEngine.Object) null))
      {
        NPC npc = this.TeamNPC[index];
        if (npc.Alive)
        {
          MatchManager.CharacterForOrder theChar = new MatchManager.CharacterForOrder()
          {
            index = index,
            id = npc.Id,
            hero = (Hero) null,
            npc = npc,
            speed = npc.GetSpeed(),
            npcItem = npc.NPCItem
          };
          theChar.speedForOrder = (float) (this.AuxiliarSumForInitiative(theChar) + theChar.speed[0]) + 0.01f * (float) (10 - index);
          this.CharOrder.Add(theChar);
        }
      }
    }
    this.CharOrder = this.CharOrder.OrderByDescending<MatchManager.CharacterForOrder, float>((Func<MatchManager.CharacterForOrder, float>) (w => w.speedForOrder)).ToList<MatchManager.CharacterForOrder>();
    this.DrawInitiatives();
  }

  private void ReDoInitiatives(object objKilled)
  {
    for (int index = this.CharOrder.Count - 1; index >= 0; --index)
    {
      if (this.CharOrder[index].hero == objKilled || this.CharOrder[index].npc == objKilled)
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.CharOrder[index].initiativePortrait.gameObject);
        this.CharOrder.RemoveAt(index);
      }
    }
    this.ReDrawInitiatives();
  }

  private void DrawInitiatives()
  {
    this.initiativeRoundGO = UnityEngine.Object.Instantiate<GameObject>(this.initiativeRoundPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, this.GO_Initiative.transform);
    for (int index = 0; index < this.CharOrder.Count; ++index)
    {
      InitiativePortrait component = UnityEngine.Object.Instantiate<GameObject>(this.initiativePrefab, new Vector3(0.0f, 0.0f, 1f), Quaternion.identity, this.GO_Initiative.transform).GetComponent<InitiativePortrait>();
      this.CharOrder[index].initiativePortrait = component;
      if (this.CharOrder[index].hero != null)
      {
        component.SetHero(this.CharOrder[index].hero, this.CharOrder[index].heroItem);
        component.SetSpeed(this.CharOrder[index].speed);
      }
      else if (this.CharOrder[index].npc != null)
      {
        component.SetNPC(this.CharOrder[index].npc.NpcData, this.CharOrder[index].npcItem);
        component.SetSpeed(this.CharOrder[index].speed);
      }
      component.Init(index);
    }
    this.DrawInitiativesWidth();
  }

  public void ReDrawInitiatives()
  {
    if (this.CharOrder == null)
      return;
    if ((UnityEngine.Object) this.initiativeRoundGO == (UnityEngine.Object) null)
      this.initiativeRoundGO = UnityEngine.Object.Instantiate<GameObject>(this.initiativeRoundPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, this.GO_Initiative.transform);
    this.initiativeRoundGO.SetActive(false);
    this.SetRoundText();
    for (int index = 0; index < this.CharOrder.Count; ++index)
    {
      MatchManager.CharacterForOrder theChar = this.CharOrder[index];
      if (theChar.hero != null)
      {
        theChar.speed = theChar.hero.GetSpeed();
        theChar.speedForOrder = (float) ((double) (this.AuxiliarSumForInitiative(theChar) + theChar.speed[0]) + 0.5 + 0.0099999997764825821 * (double) (20 - theChar.hero.Position));
      }
      else
      {
        theChar.speed = theChar.npc.GetSpeed();
        theChar.speedForOrder = (float) (this.AuxiliarSumForInitiative(theChar) + theChar.speed[0]) + 0.01f * (float) (20 - theChar.npc.Position);
      }
      theChar.initiativePortrait.SetSpeed(theChar.speed);
    }
    this.CharOrder = this.CharOrder.OrderByDescending<MatchManager.CharacterForOrder, float>((Func<MatchManager.CharacterForOrder, float>) (w => w.speedForOrder)).ToList<MatchManager.CharacterForOrder>();
    bool adjust = false;
    for (int index = 0; index < this.CharOrder.Count; ++index)
    {
      MatchManager.CharacterForOrder characterForOrder = this.CharOrder[index];
      if (!adjust && (characterForOrder.hero != null && characterForOrder.hero.RoundMoved >= this.currentRound || characterForOrder.npc != null && characterForOrder.npc.RoundMoved >= this.currentRound))
      {
        this.initiativeRoundGO.transform.parent = characterForOrder.initiativePortrait.transform;
        this.initiativeRoundGO.transform.localPosition = new Vector3(-0.48f, 0.0f, 0.0f);
        adjust = true;
      }
      characterForOrder.initiativePortrait.RedoPos(index, adjust);
      if (index == 0)
        characterForOrder.initiativePortrait.SetPlaying(true);
      else
        characterForOrder.initiativePortrait.SetPlaying(false);
    }
    if (adjust && this.currentRound > 0)
      this.initiativeRoundGO.SetActive(true);
    this.DrawInitiativesWidth();
  }

  private void DrawInitiativesWidth()
  {
    float num = (float) (0.47999998927116394 * (double) this.CharOrder.Count + 0.23999999463558197 * (double) (this.CharOrder.Count - 1));
    float y = 4.8f;
    if (this.initiativeRoundGO.activeSelf)
      num += 0.359999985f;
    this.GO_Initiative.transform.position = new Vector3((float) (-1.0 * ((double) num * 0.5) + 0.23999999463558197), y, 0.0f);
  }

  private int AuxiliarSumForInitiative(MatchManager.CharacterForOrder theChar)
  {
    int num = 0;
    if (theChar.hero != null)
    {
      if (theChar.hero.RoundMoved == this.currentRound)
        num = -100000;
      else if (theChar.index == this.heroActive)
        num = 100000;
    }
    else if (theChar.npc.RoundMoved == this.currentRound)
      num = -100000;
    else if (theChar.index == this.npcActive)
      num = 100000;
    return num;
  }

  public void PortraitHighlight(bool state, CharacterItem characterItem)
  {
    if (this.CharOrder == null)
      return;
    for (int index = 0; index < this.CharOrder.Count; ++index)
    {
      if ((UnityEngine.Object) this.CharOrder[index].initiativePortrait.heroItem == (UnityEngine.Object) characterItem)
        this.CharOrder[index].initiativePortrait.SetActive(state);
      if ((UnityEngine.Object) this.CharOrder[index].initiativePortrait.npcItem == (UnityEngine.Object) characterItem)
        this.CharOrder[index].initiativePortrait.SetActive(state);
    }
  }

  private void SetActiveCharacter()
  {
    MatchManager.CharacterForOrder characterForOrder = this.CharOrder[0];
    if (characterForOrder.hero != null)
    {
      this.npcActive = -1;
      this.theNPC = (NPC) null;
      this.heroActive = characterForOrder.index;
      this.theHero = this.TeamHero[this.heroActive];
      if (this.theHero == null || !this.theHero.Alive)
        return;
      this.theHero.HeroItem.ActivateMark(true);
      if (this.HeroHand[this.heroActive] == null)
        this.HeroHand[this.heroActive] = new List<string>();
      else
        this.HeroHand[this.heroActive].Clear();
      if (this.cardItemTable == null)
        this.cardItemTable = new List<CardItem>();
      else
        this.cardItemTable.Clear();
      this.newTurnScript.SetTurn(this.theHero.SourceName);
      if (GameManager.Instance.IsMultiplayer() && this.IsYourTurn())
        GameManager.Instance.PlayAudio(AudioManager.Instance.soundCombatIsYourTurn);
      this.ShowTraitInfo(true);
      this.SetTraitInfoText();
    }
    else
    {
      this.heroActive = -1;
      this.theHero = (Hero) null;
      this.npcActive = characterForOrder.index;
      this.theNPC = this.TeamNPC[this.npcActive];
      if (this.theNPC == null || !this.theNPC.Alive)
        return;
      this.theNPC.NPCItem.ActivateMark(true);
      this.newTurnScript.SetTurn(this.theNPC.SourceName);
    }
  }

  private int GetIndexForChar(Hero _hero)
  {
    for (int index = 0; index < this.CharOrder.Count; ++index)
    {
      if (this.CharOrder[index].hero == _hero)
        return this.CharOrder[index].index;
    }
    return -1;
  }

  public void SetTarget(Transform transform)
  {
    if (this.npcActive > -1)
      this.targetTransform = transform;
    if (GameManager.Instance.IsMultiplayer() && !this.IsYourTurn())
      return;
    if ((UnityEngine.Object) transform == (UnityEngine.Object) null)
      this.targetTransform = (Transform) null;
    else if ((UnityEngine.Object) transform.GetComponent<CharacterGOItem>() != (UnityEngine.Object) null)
      this.targetTransform = transform.parent.transform;
    else
      this.targetTransform = transform;
  }

  public Transform GetTarget() => this.targetTransform;

  [PunRPC]
  private void NET_SetTargetTransform(string _targetTransform)
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD(nameof (NET_SetTargetTransform), "net");
    if (_targetTransform == "")
    {
      this.targetTransform = (Transform) null;
    }
    else
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD(_targetTransform + " --> " + this.targetTransform?.ToString());
      this.targetTransform = this.targetTransformDict[_targetTransform];
    }
  }

  public void DrawArrowNet(
    int tablePosition,
    Vector3 source,
    Vector3 target,
    bool isHero,
    byte characterIndex)
  {
    this.ArrowTarget = target;
    if (this.arrowMPCo != null)
      this.StopCoroutine(this.arrowMPCo);
    if (this.cardItemTable == null || tablePosition > this.cardItemTable.Count || tablePosition < 0 || (UnityEngine.Object) this.cardItemTable[tablePosition] == (UnityEngine.Object) null)
      return;
    this.photonView.RPC("NET_DrawArrowNet", RpcTarget.Others, (object) (short) tablePosition, (object) isHero, (object) characterIndex);
  }

  [PunRPC]
  private void NET_DrawArrowNet(short _tablePosition, bool _isHero, byte _characterIndex)
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD(nameof (NET_DrawArrowNet), "net");
    int index1 = (int) _tablePosition;
    int index2 = (int) _characterIndex;
    if (this.cardItemTable == null || index1 > this.cardItemTable.Count || index1 < 0 || (UnityEngine.Object) this.cardItemTable[index1] == (UnityEngine.Object) null)
      return;
    this.cardItemTable[index1].fOnMouseDownCardData();
    if (this.coroutineDrawArrow != null)
      this.StopCoroutine(this.coroutineDrawArrow);
    if (this.cardItemTable == null || index1 > this.cardItemTable.Count || !((UnityEngine.Object) this.cardItemTable[index1] != (UnityEngine.Object) null))
      return;
    Vector3 ori = new Vector3(this.cardItemTable[index1].transform.position.x, this.cardItemTable[index1].transform.position.y, 0.0f);
    Vector3 zero = Vector3.zero;
    Vector3 dest;
    if (_isHero)
    {
      if (this.TeamHero[index2] == null || !((UnityEngine.Object) this.TeamHero[index2].HeroItem != (UnityEngine.Object) null) || !((UnityEngine.Object) this.TeamHero[index2].HeroItem.transform != (UnityEngine.Object) null))
        return;
      dest = this.TeamHero[index2].HeroItem.transform.position + new Vector3(0.0f, this.TeamHero[index2].HeroItem.transform.GetComponent<BoxCollider2D>().size.y * 0.8f, 0.0f);
    }
    else
    {
      if (this.TeamNPC[index2] == null || !((UnityEngine.Object) this.TeamNPC[index2].NPCItem != (UnityEngine.Object) null) || !((UnityEngine.Object) this.TeamNPC[index2].NPCItem.transform != (UnityEngine.Object) null))
        return;
      dest = this.TeamNPC[index2].NPCItem.transform.position + new Vector3(0.0f, this.TeamNPC[index2].NPCItem.transform.GetComponent<BoxCollider2D>().size.y * 0.7f, 0.0f);
    }
    this.coroutineDrawArrow = this.StartCoroutine(this.cardItemTable[index1].DrawArrowRemote(ori, dest));
  }

  public void StopArrowNet(int tablePosition)
  {
    if (this.arrowMPCo != null)
      this.StopCoroutine(this.arrowMPCo);
    this.arrowMPCo = this.StartCoroutine(this.StopArrowNetCo());
  }

  private IEnumerator StopArrowNetCo()
  {
    yield return (object) Globals.Instance.WaitForSeconds(0.3f);
    this.photonView.RPC("NET_StopArrowNet", RpcTarget.Others);
  }

  [PunRPC]
  private void NET_StopArrowNet()
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD(nameof (NET_StopArrowNet), "net");
    if (this.coroutineDrawArrow != null)
      this.StopCoroutine(this.coroutineDrawArrow);
    this.cursorArrow.StopDraw();
  }

  public Transform GetTargetByNum(int num)
  {
    if (num < 0)
      return (Transform) null;
    SortedDictionary<string, Transform> sortedDictionary1 = new SortedDictionary<string, Transform>();
    List<Transform> transformList = new List<Transform>();
    int num1;
    for (int index = 0; index < this.TeamHero.Length; ++index)
    {
      if (this.TeamHero[index] != null && this.TeamHero[index].Alive)
      {
        SortedDictionary<string, Transform> sortedDictionary2 = sortedDictionary1;
        num1 = 3 - this.TeamHero[index].Position;
        string key = "H" + num1.ToString();
        Transform transform = this.TeamHero[index].HeroItem.transform;
        sortedDictionary2.Add(key, transform);
      }
    }
    for (int index = 0; index < this.TeamNPC.Length; ++index)
    {
      if (this.TeamNPC[index] != null && this.TeamNPC[index].Alive)
      {
        SortedDictionary<string, Transform> sortedDictionary3 = sortedDictionary1;
        num1 = this.TeamNPC[index].Position;
        string key = "M" + num1.ToString();
        Transform transform = this.TeamNPC[index].NPCItem.transform;
        sortedDictionary3.Add(key, transform);
      }
    }
    List<Transform> list = sortedDictionary1.Values.ToList<Transform>();
    return num <= list.Count ? list[num - 1] : (Transform) null;
  }

  public bool CheckTarget(Transform transform = null, CardData cardToCheck = null, bool casterIsHero = true)
  {
    if ((UnityEngine.Object) transform == (UnityEngine.Object) null)
      transform = this.targetTransform;
    if ((UnityEngine.Object) transform == (UnityEngine.Object) null)
      return false;
    if ((UnityEngine.Object) cardToCheck == (UnityEngine.Object) null)
      cardToCheck = this.cardActive;
    if ((UnityEngine.Object) cardToCheck == (UnityEngine.Object) null)
      return false;
    if (cardToCheck.TempAttackSelf)
      return true;
    if (cardToCheck.EffectRequired != "")
    {
      if (cardToCheck.EffectRequired != "stealth")
      {
        if (cardToCheck.EffectRequired == "stanzai")
        {
          if (this.theHero != null && !this.theHero.HasEffect("stanzai") && !this.theHero.HasEffect("stanzaii") && !this.theHero.HasEffect("stanzaiii"))
            return false;
        }
        else if (cardToCheck.EffectRequired == "stanzaii")
        {
          if (this.theHero != null && !this.theHero.HasEffect("stanzaii") && !this.theHero.HasEffect("stanzaiii"))
            return false;
        }
        else if (this.theHero != null && !this.theHero.HasEffect(cardToCheck.EffectRequired) || this.theNPC != null && !this.theNPC.HasEffect(cardToCheck.EffectRequired))
          return false;
      }
      else if (this.theHero != null && !this.theHero.HasEffect("stealth") && !this.theHero.HasEffect("Stealthbonus") || this.theNPC != null && !this.theNPC.HasEffect("stealth") && !this.theNPC.HasEffect("Stealthbonus"))
        return false;
    }
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = false;
    bool flag4 = false;
    bool flag5 = false;
    Hero heroById = this.GetHeroById(transform.name);
    NPC npcById = this.GetNPCById(transform.name);
    if (cardToCheck.TargetType != Enums.CardTargetType.Global && (casterIsHero && cardToCheck.EffectRepeatTarget != Enums.EffectRepeatTarget.Random && cardToCheck.TargetPosition != Enums.CardTargetPosition.Random || !casterIsHero) && (casterIsHero && npcById != null && npcById.HasEffect("stealth") || !casterIsHero && heroById != null && heroById.HasEffect("stealth")))
      return false;
    if (cardToCheck.TargetType != Enums.CardTargetType.Global)
    {
      switch (cardToCheck.TargetSide)
      {
        case Enums.CardTargetSide.Enemy:
        case Enums.CardTargetSide.Anyone:
          bool flag6 = false;
          bool flag7 = false;
          List<Hero> heroList = new List<Hero>();
          List<NPC> npcList = new List<NPC>();
          if (heroById != null && !casterIsHero)
          {
            for (int index = 0; index < this.TeamHero.Length; ++index)
            {
              if (this.TeamHero[index] != null)
              {
                Hero hero = this.TeamHero[index];
                if (hero != null && hero.Alive && hero.HasEffect("taunt") && !hero.HasEffect("stealth"))
                {
                  heroList.Add(hero);
                  flag6 = true;
                }
              }
            }
            if (flag6 && !heroList.Contains(heroById))
              return false;
            break;
          }
          if (npcById != null & casterIsHero)
          {
            for (int index = 0; index < this.TeamNPC.Length; ++index)
            {
              if (this.TeamNPC[index] != null)
              {
                NPC npc = this.TeamNPC[index];
                if (npc != null && npc.Alive && npc.HasEffect("taunt") && !npc.HasEffect("stealth"))
                {
                  npcList.Add(npc);
                  flag7 = true;
                }
              }
            }
            if (flag7 && !npcList.Contains(npcById))
              return false;
            break;
          }
          break;
      }
    }
    if (heroById != null)
    {
      flag1 = true;
      if (this.PositionIsFront(true, heroById.Position))
        flag3 = true;
      if (this.PositionIsBack((Character) heroById))
        flag4 = true;
      if (this.theHero != null && transform.name == this.theHero.Id)
        flag5 = true;
    }
    else if (npcById != null)
    {
      flag2 = true;
      if (this.PositionIsFront(false, npcById.Position))
        flag3 = true;
      if (this.PositionIsBack((Character) npcById))
        flag4 = true;
      if (this.theNPC != null && transform.name == this.theNPC.Id)
        flag5 = true;
    }
    if (flag1 | flag2 && (cardToCheck.TargetPosition != Enums.CardTargetPosition.Front || flag3) && (cardToCheck.TargetPosition != Enums.CardTargetPosition.Back || flag4))
    {
      if (cardToCheck.TargetSide == Enums.CardTargetSide.Friend)
        return flag1 & casterIsHero || flag2 && !casterIsHero;
      if (cardToCheck.TargetSide == Enums.CardTargetSide.Enemy)
        return !(flag1 & casterIsHero) && (!flag2 || casterIsHero);
      if (cardToCheck.TargetSide == Enums.CardTargetSide.Self)
        return flag5;
      if (cardToCheck.TargetSide == Enums.CardTargetSide.FriendNotSelf)
        return flag1 & casterIsHero && !flag5 || flag2 && !casterIsHero && !flag5;
      if (cardToCheck.TargetSide == Enums.CardTargetSide.Anyone)
        return true;
    }
    return false;
  }

  public bool HaveDeckEffect(CardData _cardActive)
  {
    return (UnityEngine.Object) _cardActive != (UnityEngine.Object) null && (_cardActive.DiscardCard != 0 || _cardActive.LookCards != 0);
  }

  public bool CanInstaCast(CardData _cardActive)
  {
    if ((UnityEngine.Object) _cardActive == (UnityEngine.Object) null)
      return false;
    if (this.canInstaCastDict.ContainsKey(_cardActive.InternalId))
      return this.canInstaCastDict[_cardActive.InternalId];
    if (_cardActive.TargetType == Enums.CardTargetType.Global)
    {
      this.canInstaCastDict[_cardActive.InternalId] = true;
      return true;
    }
    if (_cardActive.TargetSide == Enums.CardTargetSide.Self)
    {
      this.canInstaCastDict[_cardActive.InternalId] = true;
      return true;
    }
    if (_cardActive.TargetPosition == Enums.CardTargetPosition.Random)
    {
      this.canInstaCastDict[_cardActive.InternalId] = true;
      return true;
    }
    if (_cardActive.TargetSide == Enums.CardTargetSide.Enemy)
    {
      bool flag = true;
      if (this.heroActive > -1)
      {
        for (int index = 0; index < this.TeamNPC.Length; ++index)
        {
          if (this.TeamNPC[index] != null && !((UnityEngine.Object) this.TeamNPC[index].NpcData == (UnityEngine.Object) null) && this.TeamNPC[index].Alive && !this.TeamNPC[index].HasEffect("stealth"))
            flag = false;
        }
      }
      else
      {
        for (int index = 0; index < this.TeamHero.Length; ++index)
        {
          if (this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroData == (UnityEngine.Object) null) && this.TeamHero[index].Alive && !this.TeamHero[index].HasEffect("stealth"))
            flag = false;
        }
      }
      if (flag)
      {
        this.canInstaCastDict[_cardActive.InternalId] = false;
        return false;
      }
    }
    if (_cardActive.TargetPosition != Enums.CardTargetPosition.Anywhere)
    {
      this.canInstaCastDict[_cardActive.InternalId] = true;
      return true;
    }
    if (_cardActive.TargetSide == Enums.CardTargetSide.Enemy)
    {
      int num = 0;
      if (this.heroActive > -1)
      {
        for (int index = 0; index < this.TeamNPC.Length; ++index)
        {
          if (this.TeamNPC[index] != null && !((UnityEngine.Object) this.TeamNPC[index].NpcData == (UnityEngine.Object) null) && this.TeamNPC[index].Alive && this.TeamNPC[index].HasEffect("taunt") && !this.TeamNPC[index].HasEffect("stealth"))
            ++num;
        }
      }
      else
      {
        for (int index = 0; index < this.TeamHero.Length; ++index)
        {
          if (this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroData == (UnityEngine.Object) null) && this.TeamHero[index].Alive && this.TeamHero[index].HasEffect("taunt") && !this.TeamHero[index].HasEffect("stealth"))
            ++num;
        }
      }
      if (num > 0)
      {
        if (num > 1)
        {
          this.canInstaCastDict[_cardActive.InternalId] = false;
          return false;
        }
        if (num == 1)
        {
          this.canInstaCastDict[_cardActive.InternalId] = true;
          return true;
        }
      }
    }
    int num1 = 0;
    int num2 = 0;
    for (int index = 0; index < this.TeamHero.Length; ++index)
    {
      if (this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroData == (UnityEngine.Object) null) && this.TeamHero[index].Alive)
      {
        if (this.CheckTarget(this.TeamHero[index].HeroItem.transform, _cardActive))
          ++num1;
        else
          ++num2;
      }
    }
    for (int index = 0; index < this.TeamNPC.Length; ++index)
    {
      if (this.TeamNPC[index] != null && !((UnityEngine.Object) this.TeamNPC[index].NpcData == (UnityEngine.Object) null) && this.TeamNPC[index].Alive && (UnityEngine.Object) this.TeamNPC[index].NPCItem != (UnityEngine.Object) null)
      {
        if (this.CheckTarget(this.TeamNPC[index].NPCItem.transform, _cardActive))
          ++num1;
        else
          ++num2;
      }
    }
    if (num1 == 1)
    {
      this.canInstaCastDict[_cardActive.InternalId] = true;
      return true;
    }
    this.canInstaCastDict[_cardActive.InternalId] = false;
    return false;
  }

  public void ClearCanInstaCastDict() => this.canInstaCastDict.Clear();

  public List<Transform> GetInstaCastTransformList(CardData _cardActive, bool casterIsHero = true)
  {
    List<Transform> castTransformList = new List<Transform>();
    for (int index = 0; index < this.TeamHero.Length; ++index)
    {
      if (this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroData == (UnityEngine.Object) null) && (UnityEngine.Object) this.TeamHero[index].HeroItem != (UnityEngine.Object) null && this.TeamHero[index].Alive && this.CheckTarget(this.TeamHero[index].HeroItem.transform, _cardActive, casterIsHero))
        castTransformList.Add(this.TeamHero[index].HeroItem.transform);
    }
    for (int index = 0; index < this.TeamNPC.Length; ++index)
    {
      if (this.TeamNPC[index] != null && !((UnityEngine.Object) this.TeamNPC[index].NpcData == (UnityEngine.Object) null) && this.TeamNPC[index].Alive && (UnityEngine.Object) this.TeamNPC[index].NPCItem != (UnityEngine.Object) null && this.CheckTarget(this.TeamNPC[index].NPCItem.transform, _cardActive, casterIsHero))
        castTransformList.Add(this.TeamNPC[index].NPCItem.transform);
    }
    return castTransformList;
  }

  public bool IsThereAnyTargetForCard(CardData _cardActive)
  {
    if (_cardActive.TargetSide == Enums.CardTargetSide.Friend || _cardActive.TargetSide == Enums.CardTargetSide.Self || _cardActive.TargetSide == Enums.CardTargetSide.FriendNotSelf || _cardActive.TargetSide == Enums.CardTargetSide.Anyone)
    {
      for (int index = 0; index < this.TeamHero.Length; ++index)
      {
        if (this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroData == (UnityEngine.Object) null) && this.TeamHero[index].Alive && (UnityEngine.Object) this.TeamHero[index].HeroItem != (UnityEngine.Object) null && this.CheckTarget(this.TeamHero[index].HeroItem.transform, _cardActive))
          return true;
      }
    }
    if (_cardActive.TargetSide == Enums.CardTargetSide.Enemy || _cardActive.TargetSide == Enums.CardTargetSide.Anyone)
    {
      for (int index = 0; index < this.TeamNPC.Length; ++index)
      {
        if (this.TeamNPC[index] != null && !((UnityEngine.Object) this.TeamNPC[index].NpcData == (UnityEngine.Object) null) && this.TeamNPC[index].Alive && (UnityEngine.Object) this.TeamNPC[index].NPCItem != (UnityEngine.Object) null && this.CheckTarget(this.TeamNPC[index].NPCItem.transform, _cardActive))
          return true;
      }
    }
    return false;
  }

  public int GetHeroFromId(string _id)
  {
    for (int heroFromId = 0; heroFromId < 4; ++heroFromId)
    {
      if (this.TeamHero[heroFromId] != null && this.TeamHero[heroFromId].Id == _id)
        return heroFromId;
    }
    return -1;
  }

  public bool CheckEnergyForCast()
  {
    return this.GetHeroEnergy() >= this.theHero.GetCardFinalCost(this.cardActive);
  }

  public int GetHeroEnergy() => this.theHero.GetEnergy();

  public void SetEnergyCounter(int energy, int energyMod = 0)
  {
    this.energyCounterTM.text = energy.ToString();
    if (energy > 0)
    {
      this.energyCounterAnim.enabled = true;
    }
    else
    {
      this.energyCounterAnim.enabled = false;
      this.energyCounterTM.color = new Color(1f, 1f, 1f, 0.4f);
      this.energyCounterBg.color = new Color(0.4f, 0.4f, 0.4f);
    }
  }

  private void ShowEnergyCounter(bool state)
  {
    this.energyCounterTM.transform.parent.gameObject.SetActive(state);
  }

  public void ShowEnergyCounterParticles() => this.energyCounterParticle.Play();

  public void AssignEnergyAction(int energy)
  {
    this.energyAssigned = energy;
    this.theHero.ModifyEnergy(-energy);
    this.energyJustWastedByHero += this.energyAssigned;
    if (GameManager.Instance.IsMultiplayer() && this.IsYourTurn())
      this.photonView.RPC("NET_AssignEnergyAction", RpcTarget.Others, (object) energy);
    this.StartCoroutine(this.AssignEnergyActionCo());
  }

  private IEnumerator AssignEnergyActionCo()
  {
    MatchManager matchManager = this;
    if (GameManager.Instance.IsMultiplayer())
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("**************************", "net");
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("WaitingSyncro assignenergyactionco", "net");
      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      if (NetworkManager.Instance.IsMaster())
      {
        if (matchManager.coroutineSyngAssignEnergy != null)
          matchManager.StopCoroutine(matchManager.coroutineSyngAssignEnergy);
        matchManager.coroutineSyngAssignEnergy = matchManager.StartCoroutine(matchManager.ReloadCombatCo("assignenergyactionco"));
        while (!NetworkManager.Instance.AllPlayersReady("assignenergyactionco"))
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (matchManager.coroutineSyngAssignEnergy != null)
          matchManager.StopCoroutine(matchManager.coroutineSyngAssignEnergy);
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("Game ready, Everybody checked assignenergyactionco", "net");
        NetworkManager.Instance.PlayersNetworkContinue("assignenergyactionco");
      }
      else
      {
        NetworkManager.Instance.SetWaitingSyncro("assignenergyactionco", true);
        NetworkManager.Instance.SetStatusReady("assignenergyactionco");
        while (NetworkManager.Instance.WaitingSyncro["assignenergyactionco"])
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("assignenergyactionco, we can continue!", "net");
      }
    }
    matchManager.energySelector.TurnOff();
    matchManager.waitingForCardEnergyAssignment = false;
    EventSystem.current.SetSelectedGameObject((GameObject) null);
    yield return (object) null;
  }

  [PunRPC]
  private void NET_AssignEnergyAction(int energy) => this.AssignEnergyAction(energy);

  public void AssignEnergyMultiplayer(int energy)
  {
    this.photonView.RPC("NET_AssignEnergyMultiplayer", RpcTarget.Others, (object) energy);
  }

  [PunRPC]
  private void NET_AssignEnergyMultiplayer(int energy)
  {
    this.energySelector.AssignEnergyFromOutside(energy);
  }

  [PunRPC]
  private void NET_ShareArrLookDiscard(string _arrToAddCard, int _randomIndex)
  {
    this.SetRandomIndex(_randomIndex);
    this.StartCoroutine(this.NET_ShareArrLookDiscardCo(_arrToAddCard, _randomIndex));
  }

  private IEnumerator NET_ShareArrLookDiscardCo(string _arrToAddCard, int _randomIndex)
  {
    MatchManager matchManager = this;
    List<string> list = ((IEnumerable<string>) JsonHelper.FromJson<string>(_arrToAddCard)).ToList<string>();
    matchManager.CICardAddcard.Clear();
    CardItem cardItem = new CardItem();
    for (int index = 0; index < list.Count; ++index)
    {
      string str = list[index];
      if (matchManager.cardGos.ContainsKey(str))
      {
        cardItem = matchManager.cardGos[str].GetComponent<CardItem>();
      }
      else
      {
        GameObject gameObject = GameObject.Find(str);
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        {
          cardItem = gameObject.transform.GetComponent<CardItem>();
        }
        else
        {
          foreach (Transform transform in matchManager.deckCardsWindow.cardContainer)
          {
            if ((bool) (UnityEngine.Object) transform.GetComponent<CardItem>() && transform.GetComponent<CardItem>().CardData.InternalId == str)
            {
              cardItem = transform.GetComponent<CardItem>();
              break;
            }
          }
        }
      }
      matchManager.CICardAddcard.Add(cardItem);
    }
    matchManager.deckCardsWindow.TurnOff();
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("**************************", "net");
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("WaitingSyncro NET_SALD_" + _randomIndex.ToString());
    yield return (object) Globals.Instance.WaitForSeconds(0.1f);
    if (NetworkManager.Instance.IsMaster())
    {
      if (matchManager.coroutineSyncLookDiscard != null)
        matchManager.StopCoroutine(matchManager.coroutineSyncLookDiscard);
      matchManager.coroutineSyncLookDiscard = matchManager.StartCoroutine(matchManager.ReloadCombatCo("NET_SALD_" + _randomIndex.ToString()));
      while (!NetworkManager.Instance.AllPlayersReady("NET_SALD_" + _randomIndex.ToString()))
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      if (matchManager.coroutineSyncLookDiscard != null)
        matchManager.StopCoroutine(matchManager.coroutineSyncLookDiscard);
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("Game ready, Everybody checked NET_SALD_" + _randomIndex.ToString(), "net");
      NetworkManager.Instance.PlayersNetworkContinue("NET_SALD_" + _randomIndex.ToString());
      yield return (object) Globals.Instance.WaitForSeconds(0.2f);
    }
    else
    {
      NetworkManager.Instance.SetWaitingSyncro("NET_SALD_" + _randomIndex.ToString(), true);
      NetworkManager.Instance.SetStatusReady("NET_SALD_" + _randomIndex.ToString());
      while (NetworkManager.Instance.WaitingSyncro["NET_SALD_" + _randomIndex.ToString()])
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("NET_SALD_" + _randomIndex.ToString() + ", we can continue!", "net");
    }
    matchManager.waitingForLookDiscardWindow = false;
    matchManager.waitingForAddcardAssignment = false;
  }

  public void AssignLookDiscardAction()
  {
    if (!GameManager.Instance.IsMultiplayer())
    {
      this.waitingForLookDiscardWindow = false;
      this.waitingForAddcardAssignment = false;
      this.deckCardsWindow.TurnOff();
    }
    else
    {
      List<string> stringList = new List<string>();
      for (int index = 0; index < this.CICardAddcard.Count; ++index)
      {
        if ((UnityEngine.Object) this.CICardAddcard[index] != (UnityEngine.Object) null && (UnityEngine.Object) this.CICardAddcard[index].CardData != (UnityEngine.Object) null)
          stringList.Add(this.CICardAddcard[index].CardData.InternalId);
      }
      this.photonView.RPC("NET_ShareArrLookDiscard", RpcTarget.All, (object) JsonHelper.ToJson<string>(stringList.ToArray()), (object) this.randomIndex);
    }
    EventSystem.current.SetSelectedGameObject((GameObject) null);
  }

  public void AmplifyCard(int tablePosition)
  {
    if (this.amplifyCardCo != null)
      this.StopCoroutine(this.amplifyCardCo);
    this.amplifyCardCo = this.StartCoroutine(this.amplifyCardCoroutine(tablePosition));
    if (this.preCastNum != -1)
      return;
    for (int tablePosition1 = 0; tablePosition1 < this.cardItemTable.Count; ++tablePosition1)
    {
      if (tablePosition1 != tablePosition)
        this.AmplifyCardOut(tablePosition1, false);
    }
  }

  private IEnumerator amplifyCardCoroutine(int tablePosition)
  {
    yield return (object) Globals.Instance.WaitForSeconds(0.05f);
    this.photonView.RPC("NET_AmplifyCard", RpcTarget.Others, (object) (short) tablePosition);
  }

  [PunRPC]
  public void NET_AmplifyCard(short _tablePosition)
  {
    int index = (int) _tablePosition;
    this.SetDamagePreview(false);
    this.SetOverDeck(false);
    this.RepositionCards();
    if (this.cardItemTable == null || this.cardItemTable.Count == 0 || index >= this.cardItemTable.Count)
      return;
    this.cardItemTable[index].fOnMouseEnter();
  }

  public void AmplifyCardOut(int tablePosition, bool fromnet = true)
  {
    if (fromnet)
    {
      if (this.amplifyCardCo != null)
        this.StopCoroutine(this.amplifyCardCo);
      this.amplifyCardCo = this.StartCoroutine(this.amplifyCardOutCoroutine(tablePosition));
    }
    else
    {
      if (this.cardItemTable == null || this.cardItemTable.Count == 0 || tablePosition > this.cardItemTable.Count - 1)
        return;
      this.cardItemTable[tablePosition].fOnMouseExit();
    }
  }

  private IEnumerator amplifyCardOutCoroutine(int tablePosition)
  {
    yield return (object) Globals.Instance.WaitForSeconds(0.05f);
    this.photonView.RPC("NET_AmplifyCardOut", RpcTarget.Others, (object) (short) tablePosition);
  }

  [PunRPC]
  public void NET_AmplifyCardOut(short _tablePosition)
  {
    this.SetDamagePreview(false);
    this.SetOverDeck(false);
    int index = (int) _tablePosition;
    if (this.cardItemTable == null || this.cardItemTable.Count == 0 || index > this.cardItemTable.Count - 1)
      return;
    this.cardItemTable[index].fOnMouseExit();
  }

  public void NoEnergy()
  {
    this.theHero.HeroItem.ScrollCombatText(Texts.Instance.GetText("noEnergy"), Enums.CombatScrollEffectType.Energy);
  }

  private void CastNPC()
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("PreCastNPC " + this.theNPC.Id, "trace");
    if (this.IsGameBusy() || this.theNPC == null)
      return;
    if (this.coroutineCastNPC != null)
      this.StopCoroutine(this.coroutineCastNPC);
    if ((UnityEngine.Object) this.theNPC.NPCItem == (UnityEngine.Object) null)
      return;
    if (this.CardsInNPCHand(this.theNPC.NPCIndex) > 0)
      this.coroutineCastNPC = this.StartCoroutine(this.CastNPCCo());
    else
      this.EndTurn();
  }

  private IEnumerator CastNPCCo()
  {
    MatchManager matchManager = this;
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD(nameof (CastNPCCo), "trace");
    int eventExaust = 0;
    yield return (object) Globals.Instance.WaitForSeconds(0.1f);
    while (matchManager.eventList.Count > 0)
    {
      if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
      {
        matchManager.eventListDbg = "";
        for (int index = 0; index < matchManager.eventList.Count; ++index)
          matchManager.eventListDbg = matchManager.eventListDbg + matchManager.eventList[index] + " || ";
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[CastNPCCo] Waiting For Eventlist to clean", "general");
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD(matchManager.eventListDbg, "general");
      }
      ++eventExaust;
      if (eventExaust > 300)
      {
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[CastNPCCo] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "general");
        matchManager.ClearEventList();
        break;
      }
      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
    }
    yield return (object) Globals.Instance.WaitForSeconds(0.1f);
    if (GameManager.Instance.IsMultiplayer())
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("**************************", "net");
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("WaitingSyncro castnpco", "net");
      if (NetworkManager.Instance.IsMaster())
      {
        if (matchManager.coroutineSyncCastNPC != null)
          matchManager.StopCoroutine(matchManager.coroutineSyncCastNPC);
        matchManager.coroutineSyncCastNPC = matchManager.StartCoroutine(matchManager.ReloadCombatCo("castnpco"));
        while (!NetworkManager.Instance.AllPlayersReady("castnpco"))
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (matchManager.coroutineSyncCastNPC != null)
          matchManager.StopCoroutine(matchManager.coroutineSyncCastNPC);
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("Game ready, Everybody checked castnpco", "net");
        NetworkManager.Instance.PlayersNetworkContinue("castnpco");
      }
      else
      {
        NetworkManager.Instance.SetWaitingSyncro("castnpco", true);
        NetworkManager.Instance.SetStatusReady("castnpco");
        while (NetworkManager.Instance.WaitingSyncro["castnpco"])
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("castnpco, we can continue!", "net");
      }
    }
    if (!GameManager.Instance.IsMultiplayer() && !GameManager.Instance.TutorialWatched("castNPC"))
    {
      matchManager.waitingTutorial = true;
      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      GameManager.Instance.ShowTutorialPopup("castNPC", matchManager.theNPC.NPCItem.transform.Find("Cards").transform.position, Vector3.zero);
      matchManager.characterWindow.Hide();
      while (matchManager.waitingTutorial)
        yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      if (GameManager.Instance.IsMultiplayer())
        matchManager.SetWatchingTutorial(false);
    }
    eventExaust = 0;
    matchManager.waitExecution = true;
    while (matchManager.waitExecution && matchManager.theNPC != null && (UnityEngine.Object) matchManager.theNPC.NPCItem != (UnityEngine.Object) null && matchManager.theNPC.NPCItem.IsCombatScrollEffectActive())
    {
      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      ++eventExaust;
      if (eventExaust > 400)
      {
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[PREwait] Waitexecution EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
        matchManager.waitExecution = false;
      }
    }
    while (matchManager.waitingDeathScreen)
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("[castNPC] waitingDeathScreen", "general");
      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
    }
    eventExaust = 0;
    while (matchManager.waitingKill)
    {
      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      ++eventExaust;
      if (eventExaust > 400)
      {
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[castNPC] waitingKill EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
        matchManager.waitExecution = false;
      }
    }
    if (matchManager.theNPC != null && matchManager.theNPC.Alive)
    {
      float castDelay = 0.0f;
      bool casted = AI.DoAI(matchManager.theNPC, matchManager.TeamHero, matchManager.TeamNPC, ref castDelay);
      if ((double) castDelay > 0.0 && !casted && matchManager.bossNpc != null && matchManager.bossNpc is PhantomArmor)
      {
        PhantomArmor pa = matchManager.bossNpc as PhantomArmor;
        pa.TriggerSpecialEffect();
        yield return (object) Globals.Instance.WaitForSeconds(castDelay);
        casted = AI.DoAI(matchManager.theNPC, matchManager.TeamHero, matchManager.TeamNPC, ref castDelay);
        yield return (object) Globals.Instance.WaitForSeconds(2f * castDelay);
        pa.SpecialEffectFinish();
        pa = (PhantomArmor) null;
      }
      if (!casted && !matchManager.CheckMatchIsOver())
        matchManager.EndTurn();
    }
    yield return (object) null;
  }

  public void NPCDiscard(int npcIndex, int cardPosition, bool casted)
  {
    if (npcIndex >= this.NPCHand.Length || this.NPCHand[npcIndex] == null || cardPosition >= this.NPCHand[npcIndex].Count || this.NPCHand[npcIndex][cardPosition] == null)
      return;
    string id = this.NPCHand[npcIndex].ElementAt<string>(cardPosition);
    bool flag = true;
    if (casted && this.GetCardData(id).Vanish)
      flag = false;
    if (flag)
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("NPCDiscard " + npcIndex.ToString(), "trace");
      if (this.NPCDeckDiscard[npcIndex] == null)
        this.NPCDeckDiscard[npcIndex] = new List<string>();
      this.NPCDeckDiscard[npcIndex].Add(id);
    }
    this.NPCHand[npcIndex][cardPosition] = (string) null;
  }

  public int CardsInNPCHand(int npcIndex)
  {
    int num = 0;
    if (npcIndex < this.NPCHand.Length && this.NPCHand[npcIndex] != null)
    {
      for (int index = 0; index < this.NPCHand[npcIndex].Count; ++index)
      {
        if (index < this.NPCHand[npcIndex].Count && this.NPCHand[npcIndex][index] != null)
          ++num;
      }
    }
    return num;
  }

  public void CastAutomatic(CardData theCardData, Transform from, Transform to)
  {
    this.theHeroPreAutomatic = this.theHero;
    this.theNPCPreAutomatic = this.theNPC;
    string npcInternalId = "";
    if ((UnityEngine.Object) from.GetComponent<HeroItem>() != (UnityEngine.Object) null)
    {
      this.theHero = from.GetComponent<HeroItem>().Hero;
      this.theNPC = (NPC) null;
      npcInternalId = this.theHero.InternalId;
    }
    else if ((UnityEngine.Object) from.GetComponent<NPCItem>() != (UnityEngine.Object) null)
    {
      this.theNPC = from.GetComponent<NPCItem>().NPC;
      this.theHero = (Hero) null;
      npcInternalId = this.theNPC.InternalId;
    }
    this.SetTarget(to);
    if (theCardData.Id.Contains('_'))
      this.NPCCastCardList(theCardData.Id);
    else
      this.NPCCastCardList(theCardData.InternalId, npcInternalId);
    this.StartCoroutine(this.CastCard(_automatic: true, _card: theCardData, _energy: 0));
  }

  private void CreateConsoleKey()
  {
    this.consoleKey = this.GetRandomString();
    this.console.SetKey(this.consoleKey);
  }

  public IEnumerator JustCastedCo()
  {
    this.justCasted = true;
    yield return (object) Globals.Instance.WaitForSeconds(0.3f);
    this.justCasted = false;
  }

  public IEnumerator CastCard(
    CardItem theCardItem = null,
    bool _automatic = false,
    CardData _card = null,
    int _energy = -1,
    int _posInTable = -1,
    bool _propagate = true)
  {
    MatchManager matchManager1 = this;
    MatchManager.Instance.RaiseCardCastBegin(theCardItem);
    matchManager1.ClearCardsBorder();
    if (!(matchManager1.gameStatus == "EndTurn"))
    {
      matchManager1.ResetFailCount();
      matchManager1.preCastNum = -1;
      matchManager1.GlobalVanishCardsNum = 0;
      matchManager1.handCardsBeforeCast = matchManager1.CountHeroHand();
      matchManager1.deckCardsBeforeCast = matchManager1.CountHeroDeck();
      matchManager1.discardCardsBeforeCast = matchManager1.CountHeroDiscard();
      matchManager1.vanishCardsBeforeCast = matchManager1.CountHeroVanish();
      matchManager1.ClearCanInstaCastDict();
      string comingId = "";
      if ((UnityEngine.Object) _card != (UnityEngine.Object) null)
      {
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("CastCard '" + _card.Id + "'", "general");
        comingId = _card.Id;
      }
      else if ((UnityEngine.Object) theCardItem != (UnityEngine.Object) null)
      {
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("CastCard '" + theCardItem.CardData.Id + "'", "general");
        comingId = theCardItem.CardData.Id;
        theCardItem.RemoveEmotes();
      }
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("[CASTCARD] SetGameBusy TRUE GameStatus CastCard " + comingId, "general");
      matchManager1.SetGameBusy(true);
      matchManager1.gameStatus = nameof (CastCard);
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD(matchManager1.gameStatus, "gamestatus");
      matchManager1.SetEventDirect("CastCardEvent" + comingId, false, true);
      matchManager1.ShowHandMask(true);
      matchManager1.ShowCombatKeyboard(false);
      if (GameManager.Instance.IsMultiplayer())
      {
        matchManager1.cursorArrow.StopDraw();
        if (matchManager1.IsYourTurn())
        {
          if (!_automatic & _propagate)
            matchManager1.CastCardPropagation(theCardItem, _automatic, _card, _energy, theCardItem.TablePosition);
        }
        else if ((UnityEngine.Object) theCardItem == (UnityEngine.Object) null && _posInTable != -1)
        {
          bool flag = false;
          if (_posInTable >= matchManager1.cardItemTable.Count)
          {
            flag = true;
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("**** errorWithCard (0) **", "error");
          }
          else if ((UnityEngine.Object) matchManager1.cardItemTable[_posInTable] == (UnityEngine.Object) null)
          {
            flag = true;
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("**** errorWithCard (1) **", "error");
          }
          if (!flag)
          {
            theCardItem = matchManager1.cardItemTable[_posInTable];
            if (theCardItem.CardData.Id != comingId)
            {
              if (theCardItem.CardData.Id.Split('_', StringSplitOptions.None)[0] == comingId.Split('_', StringSplitOptions.None)[0])
              {
                Debug.LogError((object) ("**** errorWithCard BUT continue " + theCardItem.CardData.Id + "!=" + comingId + "**"));
                theCardItem.CardData.Id = comingId;
              }
              else
              {
                flag = true;
                Debug.LogError((object) ("**** errorWithCard " + theCardItem.CardData.Id + "!=" + comingId + "**"));
              }
            }
          }
          if (flag && NetworkManager.Instance.IsMaster())
          {
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("**** errorWithCard -> master Break by Desync **", "error");
            yield return (object) Globals.Instance.WaitForSeconds(0.1f);
            matchManager1.ReloadCombat("**** errorWithCard -> master Break by Desync **");
            yield break;
          }
        }
      }
      CardData _cardActive = !((UnityEngine.Object) theCardItem != (UnityEngine.Object) null) ? _card : theCardItem.CardData;
      if (!_automatic && GameManager.Instance.IsMultiplayer() && !matchManager1.castingCardListMP.Contains(_cardActive.Id))
        matchManager1.castingCardListMP.Add(_cardActive.Id);
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("CastCardProcess Debug 1", "trace");
      if ((UnityEngine.Object) _cardActive != (UnityEngine.Object) null && matchManager1.TeamHero != null && matchManager1.heroActive > -1 && matchManager1.heroActive < matchManager1.TeamHero.Length && matchManager1.TeamHero[matchManager1.heroActive] != null)
      {
        List<NPC> npcsWithAbility;
        bool masterReweaverAbility = matchManager1.TryGetNPCsWithMasterReweaverAbility(_cardActive, (Character) matchManager1.TeamHero[matchManager1.heroActive], out npcsWithAbility);
        CardItem cardItem = matchManager1.cardItemTable.FirstOrDefault<CardItem>((Func<CardItem, bool>) (i => i.CardData.Id == _cardActive.Id));
        if (!_cardActive.TempAttackSelf & masterReweaverAbility)
          matchManager1.ApplyCorruptedEcho(_cardActive, cardItem, npcsWithAbility);
        else if (_cardActive.TempAttackSelf && !masterReweaverAbility)
          matchManager1.ReturnCardDataToOriginalState(_cardActive, cardItem);
        matchManager1.TeamHero[matchManager1.heroActive].SetCastedCard(_cardActive);
      }
      string _cardForSync;
      CardItem cardTutorial;
      if (_automatic)
      {
        if (_cardActive.CardName.ToLower() == "crystallize")
          matchManager1.scarabSuccess = "1";
        if (_cardActive.Fluff != "" && (double) (DateTime.Now.Millisecond % 100) < (double) _cardActive.FluffPercent)
          matchManager1.DoComic((Character) matchManager1.theNPC, _cardActive.Fluff);
        GameManager.Instance.PlayLibraryAudio("castnpccard");
        if (GameManager.Instance.IsMultiplayer())
        {
          _cardForSync = _cardActive.InternalId;
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("**************************", "net");
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("WaitingSyncro castnpccard_" + _cardForSync, "net");
          if (NetworkManager.Instance.IsMaster())
          {
            if (matchManager1.coroutineSyncCastCardNPC != null)
              matchManager1.StopCoroutine(matchManager1.coroutineSyncCastCardNPC);
            matchManager1.coroutineSyncCastCardNPC = matchManager1.StartCoroutine(matchManager1.ReloadCombatCo("castnpccard_" + _cardForSync));
            while (!NetworkManager.Instance.AllPlayersReady("castnpccard_" + _cardForSync))
              yield return (object) Globals.Instance.WaitForSeconds(0.01f);
            if (matchManager1.coroutineSyncCastCardNPC != null)
              matchManager1.StopCoroutine(matchManager1.coroutineSyncCastCardNPC);
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("Game ready, Everybody checked castnpccard_" + _cardForSync, "net");
            matchManager1.SetRandomIndex(matchManager1.randomIndex);
            NetworkManager.Instance.PlayersNetworkContinue("castnpccard_" + _cardForSync, matchManager1.randomIndex.ToString());
          }
          else
          {
            NetworkManager.Instance.SetWaitingSyncro("castnpccard_" + _cardForSync, true);
            NetworkManager.Instance.SetStatusReady("castnpccard_" + _cardForSync);
            while (NetworkManager.Instance.WaitingSyncro["castnpccard_" + _cardForSync])
              yield return (object) Globals.Instance.WaitForSeconds(0.01f);
            if (NetworkManager.Instance.netAuxValue != "")
              matchManager1.SetRandomIndex(int.Parse(NetworkManager.Instance.netAuxValue));
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("castnpccard_" + _cardForSync + ", we can continue!", "net");
          }
          _cardForSync = (string) null;
        }
        else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
          yield return (object) Globals.Instance.WaitForSeconds(0.8f);
        else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Ultrafast)
          yield return (object) null;
        else
          yield return (object) Globals.Instance.WaitForSeconds(0.28f);
        if (!GameManager.Instance.IsMultiplayer() && matchManager1.theNPC != null && (UnityEngine.Object) matchManager1.theNPC.NPCItem != (UnityEngine.Object) null)
        {
          CardData cardData = _cardActive;
          if ((cardData != null ? (cardData.SpecialCardEnum == SpecialCardEnum.NightmareImage ? 1 : 0) : 0) != 0 && !GameManager.Instance.TutorialWatched("illusionAbility"))
          {
            cardTutorial = matchManager1.theNPC.GetCurrentCardItem();
            yield return (object) Globals.Instance.WaitForSeconds(0.3f);
            matchManager1.waitingTutorial = true;
            yield return (object) Globals.Instance.WaitForSeconds(0.2f);
            GameManager.Instance.ShowTutorialPopup("illusionAbility", cardTutorial.transform.Find("CardGO/TitleText").transform.position, Vector3.zero);
            matchManager1.characterWindow.Hide();
            while (matchManager1.waitingTutorial)
              yield return (object) Globals.Instance.WaitForSeconds(0.1f);
            cardTutorial = (CardItem) null;
          }
        }
        if (_cardActive.MoveToCenter)
        {
          if (_cardActive.AddCard == 0 && !matchManager1.IsPhantomArmorSpecialCard(_cardActive.Id))
          {
            yield return (object) Globals.Instance.WaitForSeconds(0.15f);
            matchManager1.theNPC.NPCItem.CharacterAttackAnim();
          }
        }
        else if (matchManager1.theNPC != null && (UnityEngine.Object) matchManager1.theNPC.NPCItem != (UnityEngine.Object) null && !matchManager1.IsPhantomArmorSpecialCard(_cardActive.Id))
        {
          matchManager1.theNPC.NPCItem.CharacterCastAnim();
          yield return (object) Globals.Instance.WaitForSeconds(0.2f);
        }
      }
      if (matchManager1.castingCardBlocked.ContainsKey(_cardActive.InternalId))
        matchManager1.castingCardBlocked[_cardActive.InternalId] = true;
      else
        matchManager1.castingCardBlocked.Add(_cardActive.InternalId, true);
      string _uniqueCastId = Functions.RandomString(6f);
      matchManager1.castCardDamageDoneTotal = 0.0f;
      matchManager1.energyAssigned = _energy;
      Hero theCasterHero = matchManager1.theHero;
      NPC theCasterNPC = matchManager1.theNPC;
      Character theCasterCharacter = (Character) null;
      bool theCasterIsHero = false;
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("CastCardProcess Debug 2", "trace");
      if ((UnityEngine.Object) matchManager1.targetTransform == (UnityEngine.Object) null)
      {
        List<Transform> castTransformList = matchManager1.GetInstaCastTransformList(_cardActive);
        if (castTransformList.Count == 0)
          yield break;
        else
          matchManager1.targetTransform = castTransformList[0];
      }
      if (theCasterHero != null)
      {
        theCasterIsHero = true;
        theCasterCharacter = (Character) theCasterHero;
        string gameName = matchManager1.theHero.GameName;
        string id = matchManager1.theHero.Id;
        if ((UnityEngine.Object) _cardActive != (UnityEngine.Object) null)
        {
          if (!_cardActive.AutoplayDraw && !_cardActive.AutoplayEndTurn)
            matchManager1.energyJustWastedByHero = matchManager1.theHero.GetCardFinalCost(_cardActive);
        }
        else
          matchManager1.energyJustWastedByHero = 0;
        matchManager1.theHero.ModifyEnergy(-matchManager1.theHero.GetCardFinalCost(_cardActive));
        if (_cardActive.TargetSide == Enums.CardTargetSide.Self && (UnityEngine.Object) matchManager1.theHero.HeroItem != (UnityEngine.Object) null)
          matchManager1.targetTransform = matchManager1.theHero.HeroItem.transform;
        matchManager1.theHero.SetEvent(Enums.EventActivation.CastCard, auxInt: _cardActive.GetCardFinalCost(), auxString: _cardActive.Id);
        if ((UnityEngine.Object) theCasterHero.HeroData != (UnityEngine.Object) null && (UnityEngine.Object) theCasterHero.HeroData.HeroSubClass.ActionSound != (UnityEngine.Object) null)
          GameManager.Instance.PlayAudio(theCasterHero.HeroData.HeroSubClass.ActionSound, 0.25f);
      }
      else if (theCasterNPC != null)
      {
        theCasterCharacter = (Character) theCasterNPC;
        string gameName = matchManager1.theNPC.GameName;
        string id = matchManager1.theNPC.Id;
        if (_cardActive.TargetSide == Enums.CardTargetSide.Self && (UnityEngine.Object) matchManager1.theNPC.NPCItem != (UnityEngine.Object) null)
          matchManager1.targetTransform = matchManager1.theNPC.NPCItem.transform;
        matchManager1.theNPC.ModifyEnergy(-matchManager1.theNPC.GetCardFinalCost(_cardActive));
        if ((UnityEngine.Object) matchManager1.targetTransform != (UnityEngine.Object) null)
          matchManager1.theHero = matchManager1.GetHeroById(matchManager1.targetTransform.name);
      }
      if (matchManager1.waitingItemTrait)
      {
        while (matchManager1.waitingItemTrait)
        {
          matchManager1.waitingItemTrait = false;
          yield return (object) Globals.Instance.WaitForSeconds(0.1f);
        }
      }
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("CastCardProcess Debug 2.5", "trace");
      AudioClip soundRelease = _cardActive.GetSoundRelease(theCasterHero, theCasterNPC);
      if ((UnityEngine.Object) soundRelease != (UnityEngine.Object) null)
        GameManager.Instance.PlayAudio(soundRelease, 0.25f);
      if ((UnityEngine.Object) theCardItem != (UnityEngine.Object) null && theCardItem.CardData.EffectPreAction.Trim() != "")
      {
        if (theCasterNPC != null && (UnityEngine.Object) theCasterNPC.NPCItem != (UnityEngine.Object) null)
        {
          EffectsManager.Instance.PlayEffectAC(theCardItem.CardData.EffectPreAction, true, theCasterNPC.NPCItem.CharImageT, false);
          if (!theCardItem.CardData.IsPetCast)
            theCasterNPC.NPCItem.CharacterCastAnim();
        }
        else if (theCasterHero != null && (UnityEngine.Object) theCasterHero.HeroItem != (UnityEngine.Object) null)
        {
          EffectsManager.Instance.PlayEffectAC(theCardItem.CardData.EffectPreAction, true, theCasterHero.HeroItem.CharImageT, false);
          if (!theCardItem.CardData.IsPetCast)
            theCasterHero.HeroItem.CharacterCastAnim();
        }
        if ((double) theCardItem.CardData.EffectPostCastDelay > 0.0)
        {
          theCardItem.PreDiscardCard();
          float _time = theCardItem.CardData.EffectPostCastDelay;
          if (GameManager.Instance.IsMultiplayer() || GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Fast || GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Ultrafast)
            _time *= 0.7f;
          if ((double) _time < 0.10000000149011612)
            _time = 0.1f;
          yield return (object) Globals.Instance.WaitForSeconds(_time);
        }
      }
      string theCardItemPostDiscard = "";
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("CastCardProcess Debug 3", "trace");
      if (!_automatic)
      {
        if ((UnityEngine.Object) theCardItem != (UnityEngine.Object) null && (UnityEngine.Object) theCardItem.CardData != (UnityEngine.Object) null && theCardItem.CardData.Vanish)
        {
          theCardItem.cardVanish = true;
          matchManager1.DiscardCard(theCardItem.TablePosition, Enums.CardPlace.Vanish);
        }
        else if (_cardActive.DrawCard != 0 || _cardActive.AddCard != 0)
        {
          theCardItemPostDiscard = theCardItem.InternalId;
          matchManager1.DiscardCard(theCardItem.TablePosition, moveToDiscard: false);
        }
        else
          matchManager1.DiscardCard(theCardItem.TablePosition);
        if (matchManager1.energyAssigned == -1 && (_cardActive.DamageEnergyBonus > 0 || _cardActive.HealEnergyBonus > 0 || _cardActive.EffectRepeatEnergyBonus > 0 || (UnityEngine.Object) _cardActive.AcEnergyBonus != (UnityEngine.Object) null && _cardActive.AcEnergyBonusQuantity > 0))
        {
          if (matchManager1.theHero != null)
          {
            if (matchManager1.theHero.GetEnergy() > 0)
            {
              theCardItem.AmplifySetEnergy();
              matchManager1.energySelector.TurnOn(matchManager1.theHero.GetEnergy(), _cardActive.EffectRepeatMaxBonus);
              matchManager1.DrawDeckPileLayer("Default");
              matchManager1.energyAssigned = 0;
              if (GameManager.Instance.IsMultiplayer())
              {
                if (NetworkManager.Instance.IsMaster())
                {
                  if (matchManager1.coroutineSyncWaitingAction != null)
                    matchManager1.StopCoroutine(matchManager1.coroutineSyncWaitingAction);
                  matchManager1.coroutineSyncWaitingAction = matchManager1.StartCoroutine(matchManager1.ReloadCombatCo("waitingAction" + _cardActive.Id));
                  while (!NetworkManager.Instance.AllPlayersReady("waitingAction" + _cardActive.Id))
                    yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                  if (matchManager1.coroutineSyncWaitingAction != null)
                    matchManager1.StopCoroutine(matchManager1.coroutineSyncWaitingAction);
                  if (Globals.Instance.ShowDebug)
                    Functions.DebugLogGD("Game ready, Everybody checked waitingAction" + _cardActive.Id, "net");
                  NetworkManager.Instance.PlayersNetworkContinue("waitingAction" + _cardActive.Id);
                  yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                }
                else
                {
                  NetworkManager.Instance.SetWaitingSyncro("waitingAction" + _cardActive.Id, true);
                  NetworkManager.Instance.SetStatusReady("waitingAction" + _cardActive.Id);
                  while (NetworkManager.Instance.WaitingSyncro["waitingAction" + _cardActive.Id])
                    yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                  if (Globals.Instance.ShowDebug)
                    Functions.DebugLogGD("waitingAction, we can continue!", "net");
                }
              }
              matchManager1.waitingForCardEnergyAssignment = true;
              while (matchManager1.waitingForCardEnergyAssignment)
                yield return (object) Globals.Instance.WaitForSeconds(0.1f);
            }
            theCardItem.discard = true;
            yield return (object) Globals.Instance.WaitForSeconds(0.1f);
            theCardItem.PreDiscardCard();
            yield return (object) Globals.Instance.WaitForSeconds(0.2f);
          }
          else
          {
            matchManager1.energyAssigned = 1;
            theCardItem.DiscardCard(true);
          }
        }
        else if (matchManager1.IsBeginTournPhase && _cardActive.CardClass == Enums.CardClass.Special || matchManager1.theNPC != null)
          theCardItem.DiscardCard(false);
        else if (_cardActive.DrawCard != 0 || _cardActive.DiscardCard != 0 || _cardActive.AddCard != 0 || _cardActive.LookCards != 0 || _cardActive.EffectRepeat > 1)
        {
          if (!theCardItem.CardIsPrediscarding())
          {
            theCardItem.PreDiscardCard();
            yield return (object) Globals.Instance.WaitForSeconds(0.2f);
          }
        }
        else
          theCardItem.DiscardCard(false);
      }
      if (!matchManager1.isBeginTournPhase && !_automatic && GameManager.Instance.IsMultiplayer())
      {
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        _cardForSync = _cardActive.InternalId;
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("**************************", "net");
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("WaitingSyncro castcard_" + _cardForSync, "net");
        if (NetworkManager.Instance.IsMaster())
        {
          if (matchManager1.coroutineSyncCastCard != null)
            matchManager1.StopCoroutine(matchManager1.coroutineSyncCastCard);
          matchManager1.coroutineSyncCastCard = matchManager1.StartCoroutine(matchManager1.ReloadCombatCo("castcard_" + _cardForSync));
          while (!NetworkManager.Instance.AllPlayersReady("castcard_" + _cardForSync))
            yield return (object) Globals.Instance.WaitForSeconds(0.01f);
          if (matchManager1.coroutineSyncCastCard != null)
            matchManager1.StopCoroutine(matchManager1.coroutineSyncCastCard);
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("Game ready, Everybody checked castcard_" + _cardForSync, "net");
          matchManager1.SetRandomIndex(matchManager1.randomIndex);
          matchManager1.randomForIteration = matchManager1.randomIndex;
          NetworkManager.Instance.PlayersNetworkContinue("castcard_" + _cardForSync, matchManager1.randomIndex.ToString());
          yield return (object) Globals.Instance.WaitForSeconds(0.1f);
        }
        else
        {
          NetworkManager.Instance.SetWaitingSyncro("castcard_" + _cardForSync, true);
          NetworkManager.Instance.SetStatusReady("castcard_" + _cardForSync);
          while (NetworkManager.Instance.WaitingSyncro["castcard_" + _cardForSync])
            yield return (object) Globals.Instance.WaitForSeconds(0.01f);
          if (NetworkManager.Instance.netAuxValue != "")
          {
            matchManager1.SetRandomIndex(int.Parse(NetworkManager.Instance.netAuxValue));
            matchManager1.randomForIteration = matchManager1.randomIndex;
          }
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("castcard_" + _cardForSync + ", we can continue!", "net");
        }
        _cardForSync = (string) null;
      }
      if (_cardActive.DrawCard != 0 || _cardActive.DiscardCard != 0 || _cardActive.AddCard != 0 || _cardActive.LookCards != 0)
        matchManager1.SetEventDirect("CastCardEvent" + comingId, false);
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("CastCardProcess Debug 4", "trace");
      matchManager1.ResetControllerPositions();
      matchManager1.ResetControllerClickedCard();
      CardItem theCardItemPre = theCardItem;
      CardData _cardActivePre = _cardActive;
      Hero theCasterHeroPre = theCasterHero;
      Hero theHeroPre = matchManager1.theHero;
      NPC theCasterNPCPre = theCasterNPC;
      NPC theNPCPre = matchManager1.theNPC;
      Character theCasterCharacterPre = theCasterCharacter;
      Transform targetTransformPre = matchManager1.targetTransform;
      bool theCasterIsHeroPre = theCasterIsHero;
      string theCardItemPostDiscardPre = theCardItemPostDiscard;
      int _drawLoopsTotal = 1;
      bool _checkCardManipulationBeforeDraw = false;
      if (_cardActive.DrawCardSpecialValueGlobal)
      {
        _checkCardManipulationBeforeDraw = true;
        _drawLoopsTotal = 2;
      }
      int _drawLoopCurrent;
      int cardsNum;
      int indexGameBusy;
      for (_drawLoopCurrent = 0; _drawLoopCurrent < _drawLoopsTotal; ++_drawLoopCurrent)
      {
        int indexExtremeBlock;
        int i;
        if (_drawLoopsTotal == 1 || _drawLoopCurrent == 1)
        {
          cardsNum = _cardActive.DrawCard;
          if (_checkCardManipulationBeforeDraw)
            cardsNum = Functions.FuncRoundToInt(matchManager1.GetCardSpecialValue(_cardActive, 0, (Character) matchManager1.theHero, (Character) null, (Character) null, (Character) null, false));
          if (cardsNum != 0)
          {
            matchManager1.gameStatus = "DrawingCards";
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD(matchManager1.gameStatus, "gamestatus");
            int num1 = 10 - matchManager1.CountHeroHand();
            if (cardsNum == -1 || cardsNum > num1)
              cardsNum = num1;
            int num2 = matchManager1.CountHeroDeck() + matchManager1.CountHeroDiscard();
            if (cardsNum > num2)
              cardsNum = num2;
            indexExtremeBlock = 0;
            indexGameBusy = 0;
            for (i = 0; i < cardsNum; ++i)
            {
              matchManager1.gameStatus = "DrawingCards";
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("DrawingCards " + _cardActivePre.Id, "gamestatus");
              if (matchManager1.castingCardBlocked.ContainsKey(_cardActivePre.InternalId))
                matchManager1.castingCardBlocked[_cardActivePre.InternalId] = true;
              else
                matchManager1.castingCardBlocked.Add(_cardActivePre.InternalId, true);
              matchManager1.NewCard(1, Enums.CardFrom.Deck, _cardActivePre.InternalId);
              indexExtremeBlock = 0;
              indexGameBusy = 0;
              while (matchManager1.castingCardBlocked.ContainsKey(_cardActivePre.InternalId) && matchManager1.castingCardBlocked[_cardActivePre.InternalId])
              {
                yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                if (GameManager.Instance.GetDeveloperMode() && indexExtremeBlock % 100 == 0)
                {
                  if (Globals.Instance.ShowDebug)
                    Functions.DebugLogGD("[DRAWINGCARD] indexExtremeBlock" + indexExtremeBlock.ToString(), "trace");
                  if (Globals.Instance.ShowDebug)
                    Functions.DebugLogGD("[DRAWINGCARD] indexGameBusy" + indexGameBusy.ToString(), "trace");
                }
                ++indexExtremeBlock;
                if (!matchManager1.gameBusy)
                {
                  ++indexGameBusy;
                  if (indexGameBusy > 200 && matchManager1.CountHeroDeck() + matchManager1.CountHeroDiscard() == 0)
                  {
                    if (Globals.Instance.ShowDebug)
                    {
                      Functions.DebugLogGD("[DRAWINGCARD] EXIT by indexGameBusy", "trace");
                      break;
                    }
                    break;
                  }
                }
                else
                  indexGameBusy = 0;
                if (indexExtremeBlock > 700)
                {
                  if (Globals.Instance.ShowDebug)
                  {
                    Functions.DebugLogGD("[DRAWINGCARD] EXIT by indexExtremeBlock", "trace");
                    break;
                  }
                  break;
                }
              }
              if (matchManager1.theHero != null && !matchManager1.theHero.Alive)
                yield break;
              else if (10 - matchManager1.CountHeroHand() == 0)
                break;
            }
            yield return (object) Globals.Instance.WaitForSeconds(0.15f);
            matchManager1.gameStatus = "";
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD(matchManager1.gameStatus, "gamestatus");
          }
          _cardActive = _cardActivePre;
          theCardItem = theCardItemPre;
          theCasterHero = theCasterHeroPre;
          matchManager1.theHero = theHeroPre;
          theCasterNPC = theCasterNPCPre;
          matchManager1.theNPC = theNPCPre;
          theCasterCharacter = theCasterCharacterPre;
          matchManager1.targetTransform = targetTransformPre;
          theCasterIsHero = theCasterIsHeroPre;
          theCardItemPostDiscard = theCardItemPostDiscardPre;
        }
        bool flag1 = true;
        if (matchManager1.heroActive > -1 && (matchManager1.TeamHero[matchManager1.heroActive] == null || !matchManager1.TeamHero[matchManager1.heroActive].Alive))
          flag1 = false;
        matchManager1.heroIndexWaitingForAddDiscard = matchManager1.heroActive;
        if (matchManager1.heroActive > -1 & flag1)
        {
          theCardItemPre = theCardItem;
          _cardActivePre = _cardActive;
          theCasterHeroPre = theCasterHero;
          theHeroPre = matchManager1.theHero;
          theCasterNPCPre = theCasterNPC;
          theNPCPre = matchManager1.theNPC;
          theCasterCharacterPre = theCasterCharacter;
          targetTransformPre = matchManager1.targetTransform;
          theCasterIsHeroPre = theCasterIsHero;
          theCardItemPostDiscardPre = theCardItemPostDiscard;
          List<string> UsedCardsId;
          if ((_drawLoopsTotal == 1 || _drawLoopCurrent == 1) && _cardActive.AddCard != 0)
          {
            cardsNum = _cardActive.AddCard;
            if (_cardActive.AddCardPlace == Enums.CardPlace.Hand && matchManager1.CountHeroHand() + cardsNum > 10)
              cardsNum = 10 - matchManager1.CountHeroHand();
            if (cardsNum > 0)
            {
              if (_cardActive.AddCardId != "")
              {
                Hero target = (Hero) null;
                if ((UnityEngine.Object) matchManager1.targetTransform != (UnityEngine.Object) null)
                  target = matchManager1.GetHeroById(matchManager1.targetTransform.name);
                bool flag2 = false;
                if (_cardActive.TargetSide == Enums.CardTargetSide.Friend)
                {
                  if (target == null && theCasterHero != null)
                    target = theCasterHero;
                  for (int heroIndex = 0; heroIndex < 4; ++heroIndex)
                  {
                    if (matchManager1.TeamHero[heroIndex] != null && (UnityEngine.Object) matchManager1.TeamHero[heroIndex].HeroItem != (UnityEngine.Object) null && matchManager1.TeamHero[heroIndex].Alive && (_cardActive.TargetType == Enums.CardTargetType.Global || target != null && target.Id == matchManager1.TeamHero[heroIndex].Id))
                    {
                      CardData cardForModify = matchManager1.GetCardForModify(_cardActive, (Character) theCasterHero, (Character) matchManager1.TeamHero[heroIndex]);
                      matchManager1.GenerateNewCard(cardsNum, _cardActive.AddCardId, true, _cardActive.AddCardPlace, cardForModify, heroIndex: heroIndex);
                      flag2 = true;
                    }
                  }
                }
                else
                {
                  CardData cardForModify = matchManager1.GetCardForModify(_cardActive, (Character) theCasterHero, (Character) target);
                  matchManager1.GenerateNewCard(cardsNum, _cardActive.AddCardId, true, _cardActive.AddCardPlace, cardForModify);
                  flag2 = true;
                }
                if (flag2)
                {
                  yield return (object) Globals.Instance.WaitForSeconds(0.1f);
                  while (matchManager1.gameBusy)
                    yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                }
              }
              else
              {
                matchManager1.gameStatus = "AddingCards";
                if (Globals.Instance.ShowDebug)
                  Functions.DebugLogGD(matchManager1.gameStatus, "gamestatus");
                List<Enums.CardType> cardTypeList = new List<Enums.CardType>();
                if (_cardActive.AddCardType != Enums.CardType.None && _cardActive.AddCardType != Enums.CardType.Enchantment && _cardActive.AddCardType != Enums.CardType.Boon && _cardActive.AddCardType != Enums.CardType.Injury)
                {
                  if (!_cardActive.AddCardOnlyCheckAuxTypes)
                    cardTypeList.Add(_cardActive.AddCardType);
                  for (int index = 0; index < _cardActive.AddCardTypeAux.Length; ++index)
                  {
                    if (_cardActive.AddCardTypeAux[index] != Enums.CardType.None)
                      cardTypeList.Add(_cardActive.AddCardTypeAux[index]);
                  }
                }
                foreach (CardData.CardToGainTypeBasedOnHeroClass basedOnHeroClass in _cardActive.AddCardTypeBasedOnHeroClass)
                {
                  if (basedOnHeroClass.heroClass == matchManager1.theHero.HeroData.HeroSubClass.HeroClass || basedOnHeroClass.heroClass == matchManager1.theHero.HeroData.HeroSubClass.HeroClassSecondary || basedOnHeroClass.heroClass == matchManager1.theHero.HeroData.HeroSubClass.HeroClassThird)
                  {
                    foreach (Enums.CardType cardType in basedOnHeroClass.cardTypes)
                    {
                      if (!cardTypeList.Contains(cardType))
                        cardTypeList.Add(cardType);
                    }
                  }
                }
                List<int> ValidCardsInDeck = new List<int>();
                List<string> ValidCardsStringInDeck = new List<string>();
                UsedCardsId = new List<string>();
                int num3 = 0;
                if (_cardActive.AddCardFrom == Enums.CardFrom.Deck)
                  num3 = matchManager1.CountHeroDeck();
                else if (_cardActive.AddCardFrom == Enums.CardFrom.Discard)
                  num3 = matchManager1.CountHeroDiscard();
                else if (_cardActive.AddCardFrom == Enums.CardFrom.Hand)
                  num3 = matchManager1.CountHeroHand();
                else if (_cardActive.AddCardFrom == Enums.CardFrom.Vanish)
                  num3 = matchManager1.CountHeroVanish();
                List<string> keyList;
                if (_cardActive.AddCardFrom != Enums.CardFrom.Game)
                {
                  for (int index1 = 0; index1 < num3; ++index1)
                  {
                    if (cardTypeList.Count > 0)
                    {
                      CardData cardData = (CardData) null;
                      if (_cardActive.AddCardFrom == Enums.CardFrom.Deck)
                        cardData = matchManager1.GetCardData(matchManager1.HeroDeck[matchManager1.heroActive][index1]);
                      else if (_cardActive.AddCardFrom == Enums.CardFrom.Discard)
                      {
                        if (!(_cardActive.InternalId == matchManager1.HeroDeckDiscard[matchManager1.heroActive][index1]))
                          cardData = matchManager1.GetCardData(matchManager1.HeroDeckDiscard[matchManager1.heroActive][index1]);
                        else
                          continue;
                      }
                      else if (_cardActive.AddCardFrom == Enums.CardFrom.Hand)
                      {
                        if (!(_cardActive.InternalId == matchManager1.HeroHand[matchManager1.heroActive][index1]))
                          cardData = matchManager1.GetCardData(matchManager1.HeroHand[matchManager1.heroActive][index1]);
                        else
                          continue;
                      }
                      else if (_cardActive.AddCardFrom == Enums.CardFrom.Vanish)
                      {
                        if (!(_cardActive.InternalId == matchManager1.HeroDeckVanish[matchManager1.heroActive][index1]))
                        {
                          cardData = matchManager1.GetCardData(matchManager1.HeroDeckVanish[matchManager1.heroActive][index1]);
                          if (!cardData.Playable)
                            continue;
                        }
                        else
                          continue;
                      }
                      List<Enums.CardType> cardTypes = cardData.GetCardTypes();
                      bool flag3 = false;
                      for (int index2 = 0; index2 < cardTypeList.Count; ++index2)
                      {
                        if (cardTypes.Contains(cardTypeList[index2]))
                        {
                          flag3 = true;
                          break;
                        }
                      }
                      if (flag3)
                        ValidCardsInDeck.Add(index1);
                    }
                    else if ((_cardActive.AddCardFrom != Enums.CardFrom.Discard || !(_cardActive.InternalId == matchManager1.HeroDeckDiscard[matchManager1.heroActive][index1])) && (_cardActive.AddCardFrom != Enums.CardFrom.Hand || !(_cardActive.InternalId == matchManager1.HeroHand[matchManager1.heroActive][index1])) && (_cardActive.AddCardFrom != Enums.CardFrom.Vanish || !(_cardActive.InternalId == matchManager1.HeroDeckVanish[matchManager1.heroActive][index1]) && matchManager1.GetCardData(matchManager1.HeroDeckVanish[matchManager1.heroActive][index1]).Playable))
                      ValidCardsInDeck.Add(index1);
                  }
                }
                else if (cardTypeList.Count > 0)
                {
                  List<string> stringList = new List<string>();
                  string name1 = Enum.GetName(typeof (Enums.HeroClass), (object) matchManager1.theHero.HeroData.HeroClass);
                  string name2 = Enum.GetName(typeof (Enums.HeroClass), (object) matchManager1.theHero.HeroData.HeroSubClass.HeroClassSecondary);
                  string name3 = Enum.GetName(typeof (Enums.HeroClass), (object) matchManager1.theHero.HeroData.HeroSubClass.HeroClassThird);
                  int count1 = cardTypeList.Count;
                  bool flag4 = _cardActive.AddCardTypeBasedOnHeroClass.Count > 0;
                  for (int index3 = 0; index3 < cardTypeList.Count; ++index3)
                  {
                    string name4 = Enum.GetName(typeof (Enums.CardType), (object) cardTypeList[index3]);
                    string key1 = name1 + "_" + name4;
                    string key2 = name2 + "_" + name4;
                    string key3 = name3 + "_" + name4;
                    if (Globals.Instance.CardListByClassType.ContainsKey(key1))
                    {
                      int count2 = Globals.Instance.CardListByClassType[key1].Count;
                      for (int index4 = 0; index4 < count2; ++index4)
                      {
                        string id = Globals.Instance.CardListByClassType[key1][index4];
                        if (!stringList.Contains(id) && (!flag4 || cardTypeList.Contains(matchManager1.GetCardData(id).CardType)))
                          stringList.Add(id);
                      }
                    }
                    if (Globals.Instance.CardListByClassType.ContainsKey(key2))
                    {
                      int count3 = Globals.Instance.CardListByClassType[key2].Count;
                      for (int index5 = 0; index5 < count3; ++index5)
                      {
                        string id = Globals.Instance.CardListByClassType[key2][index5];
                        if (!stringList.Contains(id) && (!flag4 || cardTypeList.Contains(matchManager1.GetCardData(id).CardType)))
                          stringList.Add(id);
                      }
                    }
                    if (Globals.Instance.CardListByClassType.ContainsKey(key3))
                    {
                      int count4 = Globals.Instance.CardListByClassType[key3].Count;
                      for (int index6 = 0; index6 < count4; ++index6)
                      {
                        string id = Globals.Instance.CardListByClassType[key3][index6];
                        if (!stringList.Contains(id) && (!flag4 || cardTypeList.Contains(matchManager1.GetCardData(id).CardType)))
                          stringList.Add(id);
                      }
                    }
                  }
                  if (stringList.Count > 0)
                  {
                    if (_cardActive.AddCardChoose > 0)
                    {
                      UsedCardsId = new List<string>();
                      Dictionary<Enums.CardType, int> dictionary = new Dictionary<Enums.CardType, int>();
                      int num4 = Mathf.CeilToInt((float) _cardActive.AddCardChoose / (float) cardTypeList.Count);
                      bool flag5 = _cardActive.AddCardTypeBasedOnHeroClass.Count > 0;
                      foreach (Enums.CardType key in cardTypeList)
                        dictionary.Add(key, 0);
                      for (int index = 0; index < _cardActive.AddCardChoose; ++index)
                      {
                        bool flag6 = false;
                        while (!flag6)
                        {
                          CardData card = Globals.Instance.Cards[stringList[matchManager1.GetRandomIntRange(0, stringList.Count)]];
                          if ((UnityEngine.Object) card != (UnityEngine.Object) null && (!flag5 || !dictionary.ContainsKey(card.CardType) || dictionary[card.CardType] != num4))
                          {
                            string cardByRarity = Functions.GetCardByRarity(matchManager1.GetRandomIntRange(0, 100), card);
                            if (stringList.Contains(cardByRarity) && !UsedCardsId.Contains(cardByRarity) && (UnityEngine.Object) Globals.Instance.Cards[cardByRarity] != (UnityEngine.Object) null)
                            {
                              ValidCardsStringInDeck.Add(matchManager1.CreateCardInDictionary(Globals.Instance.Cards[cardByRarity].Id));
                              flag6 = true;
                              if (flag5)
                                dictionary[card.CardType]++;
                              UsedCardsId.Add(cardByRarity);
                            }
                          }
                        }
                      }
                    }
                    else
                    {
                      UsedCardsId = new List<string>();
                      for (int index = 0; index < cardsNum; ++index)
                      {
                        bool flag7 = false;
                        while (!flag7)
                        {
                          CardData card = Globals.Instance.Cards[stringList[matchManager1.GetRandomIntRange(0, stringList.Count)]];
                          if ((UnityEngine.Object) card != (UnityEngine.Object) null)
                          {
                            string cardByRarity = Functions.GetCardByRarity(matchManager1.GetRandomIntRange(0, 100), card);
                            if (stringList.Contains(cardByRarity) && !UsedCardsId.Contains(cardByRarity) && (UnityEngine.Object) Globals.Instance.Cards[cardByRarity] != (UnityEngine.Object) null)
                            {
                              ValidCardsStringInDeck.Add(matchManager1.CreateCardInDictionary(Globals.Instance.Cards[cardByRarity].Id));
                              flag7 = true;
                              UsedCardsId.Add(cardByRarity);
                            }
                          }
                        }
                      }
                    }
                  }
                }
                else if (_cardActive.AddCardList != null && _cardActive.AddCardList.Length != 0)
                {
                  for (int index = 0; index < _cardActive.AddCardList.Length; ++index)
                    ValidCardsStringInDeck.Add(matchManager1.CreateCardInDictionary(_cardActive.AddCardList[index].Id));
                }
                else
                {
                  keyList = new List<string>();
                  if (_cardActive.AddCardType == Enums.CardType.Boon)
                  {
                    int count = Globals.Instance.CardListByType[Enums.CardType.Boon].Count;
                    for (int index = 0; index < count; ++index)
                    {
                      string str = Globals.Instance.CardListByType[Enums.CardType.Boon][index];
                      if (str != "success" && !keyList.Contains(str))
                        keyList.Add(str);
                    }
                  }
                  else if (_cardActive.AddCardType == Enums.CardType.Injury)
                  {
                    int count = Globals.Instance.CardListByType[Enums.CardType.Injury].Count;
                    for (int index = 0; index < count; ++index)
                    {
                      string str = Globals.Instance.CardListByType[Enums.CardType.Injury][index];
                      if (!keyList.Contains(str))
                        keyList.Add(str);
                    }
                  }
                  else
                  {
                    Enums.CardClass key4 = (Enums.CardClass) Enum.Parse(typeof (Enums.CardClass), Enum.GetName(typeof (Enums.HeroClass), (object) matchManager1.theHero.HeroData.HeroClass));
                    int count5 = Globals.Instance.CardListByClass[key4].Count;
                    for (int index = 0; index < count5; ++index)
                    {
                      string str = Globals.Instance.CardListByClass[key4][index];
                      if (!keyList.Contains(str))
                        keyList.Add(str);
                    }
                    if (matchManager1.theHero.HeroData.HeroSubClass.HeroClassSecondary != Enums.HeroClass.None)
                    {
                      Enums.CardClass key5 = (Enums.CardClass) Enum.Parse(typeof (Enums.CardClass), Enum.GetName(typeof (Enums.HeroClass), (object) matchManager1.theHero.HeroData.HeroSubClass.HeroClassSecondary));
                      int count6 = Globals.Instance.CardListByClass[key5].Count;
                      for (int index = 0; index < count6; ++index)
                      {
                        string str = Globals.Instance.CardListByClass[key5][index];
                        if (!keyList.Contains(str))
                          keyList.Add(str);
                      }
                    }
                  }
                  if (keyList.Count > 0)
                  {
                    if (_cardActive.AddCardChoose > 0)
                    {
                      UsedCardsId = new List<string>();
                      for (indexGameBusy = 0; indexGameBusy < _cardActive.AddCardChoose; ++indexGameBusy)
                      {
                        bool valid = false;
                        while (!valid)
                        {
                          string key = keyList[matchManager1.GetRandomIntRange(0, keyList.Count)];
                          if (!Globals.Instance.CardListByClass[Enums.CardClass.Monster].Contains(Globals.Instance.Cards[key].Id) && !Globals.Instance.CardListByType[Enums.CardType.Injury].Contains(Globals.Instance.Cards[key].Id))
                          {
                            CardData card = Globals.Instance.Cards[key];
                            if ((UnityEngine.Object) card != (UnityEngine.Object) null && Functions.CardIsPercentRarity(matchManager1.GetRandomIntRange(0, 100), card))
                            {
                              string cardByRarity = Functions.GetCardByRarity(matchManager1.GetRandomIntRange(0, 100), card);
                              if (!UsedCardsId.Contains(cardByRarity) && (UnityEngine.Object) Globals.Instance.Cards[cardByRarity] != (UnityEngine.Object) null)
                              {
                                ValidCardsStringInDeck.Add(matchManager1.CreateCardInDictionary(Globals.Instance.Cards[cardByRarity].Id));
                                valid = true;
                                UsedCardsId.Add(cardByRarity);
                                yield return (object) null;
                              }
                            }
                          }
                        }
                      }
                    }
                    else
                    {
                      UsedCardsId = new List<string>();
                      for (int index = 0; index < cardsNum; ++index)
                      {
                        bool flag8 = false;
                        int num5 = 0;
                        while (!flag8 && num5 < 1000)
                        {
                          ++num5;
                          string key = keyList[matchManager1.GetRandomIntRange(0, keyList.Count)];
                          if (Globals.Instance.Cards.ContainsKey(key) && (UnityEngine.Object) Globals.Instance.Cards[key] != (UnityEngine.Object) null && !Globals.Instance.CardListByClass[Enums.CardClass.Monster].Contains(Globals.Instance.Cards[key].Id))
                          {
                            CardData card = Globals.Instance.Cards[key];
                            if ((UnityEngine.Object) card != (UnityEngine.Object) null && Functions.CardIsPercentRarity(matchManager1.GetRandomIntRange(0, 100), card))
                            {
                              string cardByRarity = Functions.GetCardByRarity(matchManager1.GetRandomIntRange(0, 100), card);
                              if (cardByRarity != "" && !UsedCardsId.Contains(cardByRarity) && Globals.Instance.Cards.ContainsKey(cardByRarity) && (UnityEngine.Object) Globals.Instance.Cards[cardByRarity] != (UnityEngine.Object) null)
                              {
                                ValidCardsStringInDeck.Add(matchManager1.CreateCardInDictionary(Globals.Instance.Cards[cardByRarity].Id));
                                flag8 = true;
                                UsedCardsId.Add(cardByRarity);
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                  keyList = (List<string>) null;
                }
                if (ValidCardsInDeck.Count > 0 || _cardActive.AddCardFrom == Enums.CardFrom.Game && ValidCardsStringInDeck.Count > 0)
                {
                  keyList = new List<string>();
                  indexGameBusy = 0;
                  indexGameBusy = _cardActive.AddCardChoose <= 0 ? (_cardActive.AddCardChoose != -1 ? cardsNum : ValidCardsInDeck.Count) : _cardActive.AddCardChoose;
                  if (ValidCardsStringInDeck.Count > 0)
                  {
                    for (int index = 0; index < ValidCardsStringInDeck.Count; ++index)
                      keyList.Add(ValidCardsStringInDeck[index]);
                  }
                  else
                  {
                    if (ValidCardsInDeck.Count > indexGameBusy)
                    {
                      ValidCardsInDeck = ValidCardsInDeck.ShuffleList<int>();
                      int num6 = ValidCardsInDeck.Count - indexGameBusy;
                      for (int index = 0; index < num6; ++index)
                        ValidCardsInDeck.RemoveAt(0);
                    }
                    else
                      indexGameBusy = ValidCardsInDeck.Count;
                    for (int index = 0; index < ValidCardsInDeck.Count; ++index)
                    {
                      if (_cardActive.AddCardFrom == Enums.CardFrom.Deck)
                        keyList.Add(matchManager1.HeroDeck[matchManager1.heroActive][ValidCardsInDeck[index]]);
                      else if (_cardActive.AddCardFrom == Enums.CardFrom.Discard)
                        keyList.Add(matchManager1.HeroDeckDiscard[matchManager1.heroActive][ValidCardsInDeck[index]]);
                      else if (_cardActive.AddCardFrom == Enums.CardFrom.Hand)
                        keyList.Add(matchManager1.HeroHand[matchManager1.heroActive][ValidCardsInDeck[index]]);
                      else if (_cardActive.AddCardFrom == Enums.CardFrom.Vanish)
                      {
                        keyList.Add(matchManager1.HeroDeckVanish[matchManager1.heroActive][ValidCardsInDeck[index]]);
                      }
                      else
                      {
                        string key = Globals.Instance.Cards.ElementAt<KeyValuePair<string, CardData>>(ValidCardsInDeck[index]).Key;
                        keyList.Add(matchManager1.CreateCardInDictionary(Globals.Instance.Cards[key].Id));
                      }
                    }
                  }
                  if (_cardActive.AddCardChoose != 0)
                  {
                    matchManager1.GlobalAddcardCardsNum = cardsNum <= indexGameBusy ? cardsNum : indexGameBusy;
                    List<GameObject> gameObjectList = new List<GameObject>();
                    matchManager1.CICardAddcard = new List<CardItem>();
                    if (_cardActive.AddCardFrom == Enums.CardFrom.Hand)
                      matchManager1.deckCardsWindow.TurnOn(3, 1, indexGameBusy);
                    else
                      matchManager1.deckCardsWindow.TurnOn(3, _cardActive.AddCard, indexGameBusy);
                    matchManager1.GO_List = new List<GameObject>();
                    matchManager1.cardGos.Clear();
                    for (indexExtremeBlock = 0; indexExtremeBlock < indexGameBusy; ++indexExtremeBlock)
                    {
                      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, matchManager1.deckCardsWindow.cardContainer);
                      matchManager1.GO_List.Add(gameObject);
                      cardTutorial = gameObject.GetComponent<CardItem>();
                      gameObject.name = "TMP_" + indexExtremeBlock.ToString();
                      matchManager1.cardGos.Add(gameObject.name, gameObject);
                      cardTutorial.SetCard(keyList[indexExtremeBlock], false, matchManager1.theHero);
                      matchManager1.AddCardModificationsForCardForShow(_cardActive, cardTutorial.CardData);
                      cardTutorial.DrawEnergyCost(false);
                      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                      cardTutorial.cardforaddcard = true;
                      cardTutorial.AmplifyForSelection(indexExtremeBlock, indexGameBusy);
                      cardTutorial.SetDestination(cardTutorial.GetDestination() - new Vector3(2.5f, -4.5f, 0.0f));
                      cardTutorial.DisableTrail();
                      cardTutorial.active = true;
                      cardTutorial.HideRarityParticles();
                      cardTutorial.HideCardIconParticles();
                      if (matchManager1.IsYourTurnForAddDiscard())
                        cardTutorial.ShowKeyNum(true, (indexExtremeBlock + 1).ToString());
                      yield return (object) null;
                      cardTutorial = (CardItem) null;
                    }
                    if (GameManager.Instance.IsMultiplayer())
                    {
                      if (NetworkManager.Instance.IsMaster())
                      {
                        if (matchManager1.coroutineSyncWaitingAction != null)
                          matchManager1.StopCoroutine(matchManager1.coroutineSyncWaitingAction);
                        matchManager1.coroutineSyncWaitingAction = matchManager1.StartCoroutine(matchManager1.ReloadCombatCo("waitingAction" + _cardActive.Id));
                        while (!NetworkManager.Instance.AllPlayersReady("waitingAction" + _cardActive.Id))
                          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                        if (matchManager1.coroutineSyncWaitingAction != null)
                          matchManager1.StopCoroutine(matchManager1.coroutineSyncWaitingAction);
                        if (Globals.Instance.ShowDebug)
                          Functions.DebugLogGD("Game ready, Everybody checked waitingAction" + _cardActive.Id, "net");
                        NetworkManager.Instance.PlayersNetworkContinue("waitingAction" + _cardActive.Id);
                        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                      }
                      else
                      {
                        NetworkManager.Instance.SetWaitingSyncro("waitingAction" + _cardActive.Id, true);
                        NetworkManager.Instance.SetStatusReady("waitingAction" + _cardActive.Id);
                        while (NetworkManager.Instance.WaitingSyncro["waitingAction" + _cardActive.Id])
                          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                        if (Globals.Instance.ShowDebug)
                          Functions.DebugLogGD("waitingAction, we can continue!", "net");
                      }
                    }
                    matchManager1.waitingForAddcardAssignment = true;
                    while (matchManager1.waitingForAddcardAssignment)
                      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
                    matchManager1.DrawDeckScreenDestroy();
                    matchManager1.deckCardsWindow.TurnOff();
                    ValidCardsInDeck = new List<int>();
                    ValidCardsStringInDeck = new List<string>();
                    for (indexExtremeBlock = 0; indexExtremeBlock < matchManager1.CICardAddcard.Count; ++indexExtremeBlock)
                    {
                      if ((UnityEngine.Object) matchManager1.CICardAddcard[indexExtremeBlock] != (UnityEngine.Object) null && (UnityEngine.Object) matchManager1.CICardAddcard[indexExtremeBlock].CardData != (UnityEngine.Object) null)
                      {
                        string internalId = matchManager1.CICardAddcard[indexExtremeBlock].CardData.InternalId;
                        if (_cardActive.AddCardFrom == Enums.CardFrom.Deck)
                        {
                          for (int index = 0; index < matchManager1.CountHeroDeck(); ++index)
                          {
                            if (internalId == matchManager1.HeroDeck[matchManager1.heroActive][index])
                            {
                              ValidCardsInDeck.Add(index);
                              break;
                            }
                          }
                        }
                        else if (_cardActive.AddCardFrom == Enums.CardFrom.Hand)
                        {
                          string id = matchManager1.CICardAddcard[indexExtremeBlock].CardData.Id;
                          if (_cardActive.TargetSide == Enums.CardTargetSide.Friend)
                          {
                            Hero hero = (Hero) null;
                            if ((UnityEngine.Object) matchManager1.targetTransform != (UnityEngine.Object) null)
                              hero = matchManager1.GetHeroById(matchManager1.targetTransform.name);
                            if (hero != null)
                            {
                              for (int heroIndex = 0; heroIndex < 4; ++heroIndex)
                              {
                                if (matchManager1.TeamHero[heroIndex] != null && matchManager1.TeamHero[heroIndex].Alive && hero.Id == matchManager1.TeamHero[heroIndex].Id)
                                {
                                  CardData cardForModify = matchManager1.GetCardForModify(_cardActive, (Character) theCasterHero, (Character) matchManager1.TeamHero[heroIndex]);
                                  matchManager1.GenerateNewCard(_cardActive.AddCard, id, true, _cardActive.AddCardPlace, cardForModify, matchManager1.CICardAddcard[indexExtremeBlock].CardData, heroIndex);
                                  break;
                                }
                              }
                            }
                          }
                          else
                          {
                            CardData cardForModify = matchManager1.GetCardForModify(_cardActive, (Character) theCasterHero, (Character) null);
                            matchManager1.GenerateNewCard(_cardActive.AddCard, id, true, _cardActive.AddCardPlace, cardForModify, matchManager1.CICardAddcard[indexExtremeBlock].CardData);
                          }
                          yield return (object) Globals.Instance.WaitForSeconds(0.1f);
                          while (matchManager1.gameBusy)
                            yield return (object) Globals.Instance.WaitForSeconds(0.1f);
                        }
                        else if (_cardActive.AddCardFrom == Enums.CardFrom.Discard)
                        {
                          for (int index = 0; index < matchManager1.CountHeroDiscard(); ++index)
                          {
                            if (internalId == matchManager1.HeroDeckDiscard[matchManager1.heroActive][index])
                            {
                              ValidCardsInDeck.Add(index);
                              break;
                            }
                          }
                        }
                        else if (_cardActive.AddCardFrom == Enums.CardFrom.Vanish)
                        {
                          for (int index = 0; index < matchManager1.CountHeroVanish(); ++index)
                          {
                            if (internalId == matchManager1.HeroDeckVanish[matchManager1.heroActive][index])
                            {
                              ValidCardsInDeck.Add(index);
                              break;
                            }
                          }
                        }
                        else
                          ValidCardsStringInDeck.Add(internalId);
                      }
                    }
                  }
                  ValidCardsInDeck.Sort();
                  List<string> stringList = new List<string>();
                  for (int index = ValidCardsInDeck.Count - 1; index >= 0; --index)
                  {
                    if (_cardActive.AddCardFrom == Enums.CardFrom.Deck)
                    {
                      CardData cardData = matchManager1.GetCardData(matchManager1.HeroDeck[matchManager1.heroActive][ValidCardsInDeck[index]]);
                      matchManager1.AddCardModificationsForCard(_cardActive, cardData);
                      stringList.Add(matchManager1.HeroDeck[matchManager1.heroActive][ValidCardsInDeck[index]]);
                      matchManager1.HeroDeck[matchManager1.heroActive].RemoveAt(ValidCardsInDeck[index]);
                      matchManager1.WriteDeckCounter();
                    }
                    else if (_cardActive.AddCardFrom == Enums.CardFrom.Discard)
                    {
                      CardData cardData = matchManager1.GetCardData(matchManager1.HeroDeckDiscard[matchManager1.heroActive][ValidCardsInDeck[index]]);
                      matchManager1.AddCardModificationsForCard(_cardActive, cardData);
                      matchManager1.RemoveCardFromDiscardPile(matchManager1.HeroDeckDiscard[matchManager1.heroActive][ValidCardsInDeck[index]]);
                      stringList.Add(matchManager1.HeroDeckDiscard[matchManager1.heroActive][ValidCardsInDeck[index]]);
                      matchManager1.HeroDeckDiscard[matchManager1.heroActive].RemoveAt(ValidCardsInDeck[index]);
                      matchManager1.RedoDiscardPile();
                    }
                    else if (_cardActive.AddCardFrom == Enums.CardFrom.Vanish)
                    {
                      CardData cardData = matchManager1.GetCardData(matchManager1.HeroDeckVanish[matchManager1.heroActive][ValidCardsInDeck[index]]);
                      matchManager1.AddCardModificationsForCard(_cardActive, cardData);
                      stringList.Add(matchManager1.HeroDeckVanish[matchManager1.heroActive][ValidCardsInDeck[index]]);
                      matchManager1.HeroDeckVanish[matchManager1.heroActive].RemoveAt(ValidCardsInDeck[index]);
                    }
                  }
                  if (_cardActive.AddCardFrom != Enums.CardFrom.Game)
                  {
                    if (_cardActive.AddCardFrom != Enums.CardFrom.Hand)
                    {
                      if (_cardActive.AddCardPlace == Enums.CardPlace.Hand)
                      {
                        for (int index = stringList.Count - 1; index >= 0; --index)
                          matchManager1.HeroDeck[matchManager1.heroActive].Insert(0, stringList[index]);
                        for (int index = 0; index < stringList.Count; ++index)
                        {
                          if (index < 10 - matchManager1.CountHeroHand())
                            matchManager1.CreateLogEntry(true, "toHand:" + matchManager1.logDictionary.Count.ToString(), stringList[index], matchManager1.TeamHero[matchManager1.heroActive], (NPC) null, (Hero) null, (NPC) null, matchManager1.currentRound);
                        }
                        matchManager1.NewCard(ValidCardsInDeck.Count, _cardActive.AddCardFrom);
                        while (matchManager1.cardsWaitingForReset > 0)
                          yield return (object) Globals.Instance.WaitForSeconds(0.1f);
                      }
                      else if (_cardActive.AddCardPlace == Enums.CardPlace.Discard)
                      {
                        for (int index = stringList.Count - 1; index >= 0; --index)
                        {
                          matchManager1.MoveCardTo(1, stringList[index], Enums.CardPlace.Discard);
                          matchManager1.CreateLogEntry(true, "toDiscard:" + matchManager1.logDictionary.Count.ToString(), stringList[index], matchManager1.TeamHero[matchManager1.heroActive], (NPC) null, (Hero) null, (NPC) null, matchManager1.currentRound);
                        }
                      }
                      else if (_cardActive.AddCardPlace == Enums.CardPlace.BottomDeck || _cardActive.AddCardPlace == Enums.CardPlace.TopDeck || _cardActive.AddCardPlace == Enums.CardPlace.RandomDeck)
                      {
                        for (int index = stringList.Count - 1; index >= 0; --index)
                        {
                          matchManager1.MoveCardTo(1, stringList[index], _cardActive.AddCardPlace);
                          int count;
                          if (_cardActive.AddCardPlace == Enums.CardPlace.BottomDeck)
                          {
                            MatchManager matchManager2 = matchManager1;
                            count = matchManager1.logDictionary.Count;
                            string _key = "toBottomDeck:" + count.ToString();
                            string _cardId = stringList[index];
                            Hero _theHero = matchManager1.TeamHero[matchManager1.heroActive];
                            int currentRound = matchManager1.currentRound;
                            matchManager2.CreateLogEntry(true, _key, _cardId, _theHero, (NPC) null, (Hero) null, (NPC) null, currentRound);
                          }
                          else if (_cardActive.AddCardPlace == Enums.CardPlace.TopDeck)
                          {
                            MatchManager matchManager3 = matchManager1;
                            count = matchManager1.logDictionary.Count;
                            string _key = "toTopDeck:" + count.ToString();
                            string _cardId = stringList[index];
                            Hero _theHero = matchManager1.TeamHero[matchManager1.heroActive];
                            int currentRound = matchManager1.currentRound;
                            matchManager3.CreateLogEntry(true, _key, _cardId, _theHero, (NPC) null, (Hero) null, (NPC) null, currentRound);
                          }
                          else if (_cardActive.AddCardPlace == Enums.CardPlace.RandomDeck)
                          {
                            MatchManager matchManager4 = matchManager1;
                            count = matchManager1.logDictionary.Count;
                            string _key = "toDeck:" + count.ToString();
                            string _cardId = stringList[index];
                            Hero _theHero = matchManager1.TeamHero[matchManager1.heroActive];
                            int currentRound = matchManager1.currentRound;
                            matchManager4.CreateLogEntry(true, _key, _cardId, _theHero, (NPC) null, (Hero) null, (NPC) null, currentRound);
                          }
                        }
                      }
                      yield return (object) Globals.Instance.WaitForSeconds(0.5f);
                    }
                  }
                  else if (ValidCardsStringInDeck.Count > 0)
                  {
                    indexExtremeBlock = -1;
                    Hero _targetHero = (Hero) null;
                    if ((UnityEngine.Object) matchManager1.targetTransform != (UnityEngine.Object) null)
                      _targetHero = matchManager1.GetHeroById(matchManager1.targetTransform.name);
                    if (_cardActive.TargetSide == Enums.CardTargetSide.Friend)
                    {
                      if (_targetHero == null && theCasterHero != null)
                        _targetHero = theCasterHero;
                      if (_targetHero != null)
                      {
                        for (int index = 0; index < 4; ++index)
                        {
                          if (matchManager1.TeamHero[index] != null && (UnityEngine.Object) matchManager1.TeamHero[index].HeroItem != (UnityEngine.Object) null && matchManager1.TeamHero[index].Alive && _targetHero.Id == matchManager1.TeamHero[index].Id)
                          {
                            indexExtremeBlock = index;
                            break;
                          }
                        }
                      }
                    }
                    if (_cardActive.AddCardChoose != 0)
                    {
                      for (i = 0; i < ValidCardsStringInDeck.Count; ++i)
                      {
                        CardData cardForModify = matchManager1.GetCardForModify(_cardActive, (Character) theCasterHero, (Character) _targetHero);
                        matchManager1.GenerateNewCard(1, ValidCardsStringInDeck[i], false, _cardActive.AddCardPlace, cardForModify, heroIndex: indexExtremeBlock);
                        yield return (object) Globals.Instance.WaitForSeconds(0.1f);
                        while (matchManager1.gameBusy)
                          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                      }
                    }
                    else
                    {
                      for (i = 0; i < ValidCardsStringInDeck.Count; ++i)
                      {
                        CardData cardForModify = matchManager1.GetCardForModify(_cardActive, (Character) theCasterHero, (Character) _targetHero);
                        matchManager1.GenerateNewCard(1, ValidCardsStringInDeck[i], false, _cardActive.AddCardPlace, cardForModify, heroIndex: indexExtremeBlock);
                        yield return (object) Globals.Instance.WaitForSeconds(0.1f);
                        while (matchManager1.gameBusy)
                          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                      }
                    }
                    _targetHero = (Hero) null;
                  }
                  keyList = (List<string>) null;
                }
                ValidCardsInDeck = (List<int>) null;
                ValidCardsStringInDeck = (List<string>) null;
                UsedCardsId = (List<string>) null;
              }
            }
            matchManager1.gameStatus = "";
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD(matchManager1.gameStatus, "gamestatus");
          }
          List<CardItem> CIAutomatic;
          if ((_drawLoopsTotal == 1 || _drawLoopCurrent == 0) && (_cardActive.DiscardCard != 0 || _cardActive.SpecialValueGlobal == Enums.CardSpecialValue.DiscardedCards || _cardActive.SpecialValueGlobal == Enums.CardSpecialValue.VanishedCards))
          {
            if (_cardActive.DrawCard != 0)
            {
              yield return (object) Globals.Instance.WaitForSeconds(0.05f);
              matchManager1.RepositionCards();
            }
            matchManager1.gameStatus = "DiscardingCards";
            matchManager1.GlobalDiscardCardsNum = _cardActive.DiscardCard;
            if (matchManager1.GlobalDiscardCardsNum == -1)
              matchManager1.GlobalDiscardCardsNum = 10;
            yield return (object) Globals.Instance.WaitForSeconds(0.05f);
            List<Enums.CardType> cardTypeList = new List<Enums.CardType>();
            if (_cardActive.DiscardCardType != Enums.CardType.None)
            {
              cardTypeList.Add(_cardActive.DiscardCardType);
              for (int index = 0; index < _cardActive.DiscardCardTypeAux.Length; ++index)
              {
                if (_cardActive.DiscardCardTypeAux[index] != Enums.CardType.None)
                  cardTypeList.Add(_cardActive.DiscardCardTypeAux[index]);
              }
            }
            List<string> stringList = new List<string>();
            for (int index7 = 0; index7 < matchManager1.cardItemTable.Count; ++index7)
            {
              CardItem cardItem = matchManager1.cardItemTable[index7];
              if (!(cardItem.InternalId == theCardItem.InternalId))
              {
                if (cardTypeList.Count > 0)
                {
                  List<Enums.CardType> cardTypes = cardItem.CardData.GetCardTypes();
                  for (int index8 = 0; index8 < cardTypes.Count; ++index8)
                  {
                    if (cardTypeList.Contains(cardTypes[index8]))
                    {
                      stringList.Add(cardItem.InternalId);
                      break;
                    }
                  }
                }
                else
                  stringList.Add(cardItem.InternalId);
              }
            }
            bool flag9 = _cardActive.DiscardCardAutomatic;
            if (matchManager1.GlobalDiscardCardsNum >= stringList.Count)
            {
              matchManager1.GlobalDiscardCardsNum = stringList.Count;
              flag9 = true;
            }
            matchManager1.discardNumDecidedByThePlayer = false;
            if (_cardActive.SpecialValueGlobal == Enums.CardSpecialValue.DiscardedCards || _cardActive.SpecialValueGlobal == Enums.CardSpecialValue.VanishedCards)
            {
              flag9 = false;
              matchManager1.GlobalDiscardCardsNum = 10;
              matchManager1.discardNumDecidedByThePlayer = true;
            }
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("--->" + matchManager1.GlobalDiscardCardsNum.ToString());
            if (matchManager1.GlobalDiscardCardsNum > 0)
            {
              if (flag9)
              {
                CIAutomatic = new List<CardItem>();
                if (matchManager1.GlobalDiscardCardsNum != stringList.Count)
                {
                  for (int index9 = 0; index9 < matchManager1.GlobalDiscardCardsNum; ++index9)
                  {
                    int randomIntRange = matchManager1.GetRandomIntRange(0, stringList.Count);
                    for (int index10 = 0; index10 < matchManager1.cardItemTable.Count; ++index10)
                    {
                      CardItem cardItem = matchManager1.cardItemTable[index10];
                      if (cardItem.InternalId == stringList[randomIntRange])
                      {
                        CIAutomatic.Add(cardItem);
                        stringList.RemoveAt(randomIntRange);
                        break;
                      }
                    }
                  }
                }
                else
                {
                  for (int index11 = 0; index11 < matchManager1.GlobalDiscardCardsNum; ++index11)
                  {
                    for (int index12 = 0; index12 < matchManager1.cardItemTable.Count; ++index12)
                    {
                      CardItem cardItem = matchManager1.cardItemTable[index12];
                      if (cardItem.InternalId == stringList[index11])
                      {
                        CIAutomatic.Add(cardItem);
                        break;
                      }
                    }
                  }
                }
                cardsNum = CIAutomatic.Count;
                if (cardsNum > 0)
                {
                  GameManager.Instance.PlayLibraryAudio("castnpccard");
                  for (int index = 0; index < cardsNum; ++index)
                    CIAutomatic[index].CenterToDiscard();
                  yield return (object) Globals.Instance.WaitForSeconds(0.75f);
                  for (indexGameBusy = 0; indexGameBusy < cardsNum; ++indexGameBusy)
                  {
                    CIAutomatic[indexGameBusy].DiscardCard(true, _cardActive.DiscardCardPlace);
                    matchManager1.RepositionCards();
                    matchManager1.CreateLogEntry(true, "toDiscard:" + matchManager1.logDictionary.Count.ToString(), CIAutomatic[indexGameBusy].CardData.InternalId, matchManager1.TeamHero[matchManager1.heroActive], (NPC) null, (Hero) null, (NPC) null, matchManager1.currentRound);
                    yield return (object) Globals.Instance.WaitForSeconds(0.1f);
                  }
                  yield return (object) Globals.Instance.WaitForSeconds(0.25f);
                }
                CIAutomatic = (List<CardItem>) null;
              }
              else
              {
                matchManager1.CICardDiscard = new List<CardItem>();
                matchManager1.discardSelector.TurnOn(_cardActive.DiscardCardPlace, matchManager1.discardNumDecidedByThePlayer);
                Transform cardContainer = matchManager1.discardSelector.cardContainer;
                for (int index13 = 0; index13 < stringList.Count; ++index13)
                {
                  for (int index14 = 0; index14 < matchManager1.cardItemTable.Count; ++index14)
                  {
                    CardItem cardItem = matchManager1.cardItemTable[index14];
                    if (cardItem.InternalId == stringList[index13])
                    {
                      cardItem.cardfordiscard = true;
                      cardItem.transform.parent = cardContainer;
                      cardItem.AmplifyForSelection(index13, stringList.Count);
                      if (matchManager1.IsYourTurnForAddDiscard())
                      {
                        cardItem.ShowKeyNum(true, (index13 + 1).ToString());
                        break;
                      }
                      break;
                    }
                  }
                }
                if (GameManager.Instance.IsMultiplayer())
                {
                  if (NetworkManager.Instance.IsMaster())
                  {
                    if (matchManager1.coroutineSyncWaitingAction != null)
                      matchManager1.StopCoroutine(matchManager1.coroutineSyncWaitingAction);
                    matchManager1.coroutineSyncWaitingAction = matchManager1.StartCoroutine(matchManager1.ReloadCombatCo("waitingAction" + _cardActive.Id));
                    while (!NetworkManager.Instance.AllPlayersReady("waitingAction" + _cardActive.Id))
                      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                    if (matchManager1.coroutineSyncWaitingAction != null)
                      matchManager1.StopCoroutine(matchManager1.coroutineSyncWaitingAction);
                    if (Globals.Instance.ShowDebug)
                      Functions.DebugLogGD("Game ready, Everybody checked waitingAction" + _cardActive.Id, "net");
                    NetworkManager.Instance.PlayersNetworkContinue("waitingAction" + _cardActive.Id);
                    yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                  }
                  else
                  {
                    NetworkManager.Instance.SetWaitingSyncro("waitingAction" + _cardActive.Id, true);
                    NetworkManager.Instance.SetStatusReady("waitingAction" + _cardActive.Id);
                    while (NetworkManager.Instance.WaitingSyncro["waitingAction" + _cardActive.Id])
                      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                    if (Globals.Instance.ShowDebug)
                      Functions.DebugLogGD("waitingAction, we can continue!", "net");
                  }
                }
                matchManager1.waitingForDiscardAssignment = true;
                while (matchManager1.waitingForDiscardAssignment)
                  yield return (object) Globals.Instance.WaitForSeconds(0.1f);
                matchManager1.GlobalDiscardCardsNum = 0;
                for (int index = matchManager1.cardItemTable.Count - 1; index >= 0; --index)
                {
                  CardItem cardItem = matchManager1.cardItemTable[index];
                  if (matchManager1.CICardDiscard.Contains(cardItem))
                  {
                    cardItem.cardfordiscard = false;
                    cardItem.DiscardCard(true, _cardActive.DiscardCardPlace);
                    ++matchManager1.GlobalDiscardCardsNum;
                    matchManager1.CreateLogEntry(true, "toDiscard:" + matchManager1.logDictionary.Count.ToString(), cardItem.CardData.InternalId, matchManager1.TeamHero[matchManager1.heroActive], (NPC) null, (Hero) null, (NPC) null, matchManager1.currentRound);
                  }
                }
                yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                matchManager1.RepositionCards();
                matchManager1.discardSelector.TurnOff();
                for (int index = 0; index < matchManager1.cardItemTable.Count; ++index)
                {
                  CardItem cardItem = matchManager1.cardItemTable[index];
                  if (cardItem.InternalId != theCardItem.InternalId)
                  {
                    cardItem.cardfordiscard = false;
                    cardItem.transform.parent = matchManager1.GO_Hand.transform;
                    cardItem.RestoreCard();
                  }
                }
                matchManager1.CICardDiscard.Clear();
                if (GameManager.Instance.IsMultiplayer())
                  yield return (object) Globals.Instance.WaitForSeconds(0.2f);
                else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
                  yield return (object) Globals.Instance.WaitForSeconds(0.3f);
                else
                  yield return (object) Globals.Instance.WaitForSeconds(0.1f);
              }
            }
            matchManager1.gameStatus = "";
          }
          if ((_drawLoopsTotal == 1 || _drawLoopCurrent == 1) && _cardActive.LookCards != 0)
          {
            matchManager1.gameStatus = "LookingCards";
            if ((UnityEngine.Object) matchManager1.targetTransform == (UnityEngine.Object) null)
            {
              List<Transform> castTransformList = matchManager1.GetInstaCastTransformList(_cardActive);
              if (castTransformList.Count == 0)
                yield break;
              else
                matchManager1.targetTransform = castTransformList[0];
            }
            cardsNum = matchManager1.GetIndexForChar(matchManager1.targetTransform.GetComponent<HeroItem>().Hero);
            matchManager1.heroIndexWaitingForAddDiscard = cardsNum;
            if (cardsNum > -1 && matchManager1.HeroDeck[cardsNum].Count > 0)
            {
              matchManager1.GlobalDiscardCardsNum = _cardActive.LookCardsDiscardUpTo <= 0 ? (_cardActive.LookCardsVanishUpTo <= 0 ? _cardActive.LookCards : _cardActive.LookCardsVanishUpTo) : _cardActive.LookCardsDiscardUpTo;
              UsedCardsId = new List<string>();
              int num = _cardActive.LookCards;
              if (num == -1)
              {
                for (int index = 0; index < matchManager1.HeroDeck[cardsNum].Count; ++index)
                  UsedCardsId.Add(matchManager1.HeroDeck[cardsNum][index]);
                UsedCardsId.Sort();
              }
              else
              {
                if (num > matchManager1.HeroDeck[cardsNum].Count)
                  num = matchManager1.HeroDeck[cardsNum].Count;
                for (int index = 0; index < num; ++index)
                  UsedCardsId.Add(matchManager1.HeroDeck[cardsNum][index]);
              }
              indexGameBusy = UsedCardsId.Count;
              matchManager1.deckCardsWindow.TurnOn(2, matchManager1.GlobalDiscardCardsNum, indexGameBusy, _cardActive.LookCardsVanishUpTo > 0);
              CIAutomatic = new List<CardItem>();
              matchManager1.CICardDiscard = new List<CardItem>();
              matchManager1.cardGos.Clear();
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD(indexGameBusy.ToString());
              for (indexExtremeBlock = 0; indexExtremeBlock < indexGameBusy; ++indexExtremeBlock)
              {
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, matchManager1.deckCardsWindow.cardContainer);
                cardTutorial = gameObject.GetComponent<CardItem>();
                gameObject.name = "TMP_" + indexExtremeBlock.ToString();
                matchManager1.cardGos.Add(gameObject.name, gameObject);
                CIAutomatic.Add(cardTutorial);
                cardTutorial.SetCard(UsedCardsId[indexExtremeBlock], false, matchManager1.TeamHero[cardsNum]);
                yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                cardTutorial.cardfordiscard = true;
                cardTutorial.AmplifyForSelection(indexExtremeBlock, indexGameBusy);
                cardTutorial.SetDestination(cardTutorial.GetDestination() - new Vector3(2.5f, -4.5f, 0.0f));
                cardTutorial.DisableTrail();
                cardTutorial.active = true;
                cardTutorial.HideRarityParticles();
                cardTutorial.HideCardIconParticles();
                if (matchManager1.IsYourTurnForAddDiscard())
                  cardTutorial.ShowKeyNum(true, (indexExtremeBlock + 1).ToString());
                yield return (object) null;
                cardTutorial = (CardItem) null;
              }
              if (GameManager.Instance.IsMultiplayer())
              {
                if (NetworkManager.Instance.IsMaster())
                {
                  if (matchManager1.coroutineSyncWaitingAction != null)
                    matchManager1.StopCoroutine(matchManager1.coroutineSyncWaitingAction);
                  matchManager1.coroutineSyncWaitingAction = matchManager1.StartCoroutine(matchManager1.ReloadCombatCo("waitingAction" + _cardActive.Id));
                  while (!NetworkManager.Instance.AllPlayersReady("waitingAction" + _cardActive.Id))
                    yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                  if (matchManager1.coroutineSyncWaitingAction != null)
                    matchManager1.StopCoroutine(matchManager1.coroutineSyncWaitingAction);
                  if (Globals.Instance.ShowDebug)
                    Functions.DebugLogGD("Game ready, Everybody checked waitingAction" + _cardActive.Id, "net");
                  NetworkManager.Instance.PlayersNetworkContinue("waitingAction" + _cardActive.Id);
                  yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                }
                else
                {
                  NetworkManager.Instance.SetWaitingSyncro("waitingAction" + _cardActive.Id, true);
                  NetworkManager.Instance.SetStatusReady("waitingAction" + _cardActive.Id);
                  while (NetworkManager.Instance.WaitingSyncro["waitingAction" + _cardActive.Id])
                    yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                  if (Globals.Instance.ShowDebug)
                    Functions.DebugLogGD("waitingAction, we can continue!", "net");
                }
              }
              matchManager1.waitingForLookDiscardWindow = true;
              while (matchManager1.waitingForLookDiscardWindow)
                yield return (object) Globals.Instance.WaitForSeconds(0.1f);
              if (matchManager1.CICardDiscard.Count > 0)
              {
                for (int index = 0; index < matchManager1.CICardDiscard.Count; ++index)
                {
                  CardItem cardItem = matchManager1.CICardDiscard[index];
                  cardItem.cardfordiscard = false;
                  matchManager1.HeroDeck[cardsNum].Remove(cardItem.CardData.InternalId);
                  if (_cardActive.LookCardsVanishUpTo > 0)
                  {
                    matchManager1.HeroDeckVanish[cardsNum].Add(cardItem.CardData.InternalId);
                    ++matchManager1.GlobalVanishCardsNum;
                  }
                  else
                    matchManager1.HeroDeckDiscard[cardsNum].Add(cardItem.CardData.InternalId);
                  int count;
                  if (_cardActive.LookCardsVanishUpTo > 0)
                  {
                    MatchManager matchManager5 = matchManager1;
                    count = matchManager1.logDictionary.Count;
                    string _key = "toVanish:" + count.ToString();
                    string internalId = cardItem.CardData.InternalId;
                    Hero _theHero = matchManager1.TeamHero[cardsNum];
                    int currentRound = matchManager1.currentRound;
                    matchManager5.CreateLogEntry(true, _key, internalId, _theHero, (NPC) null, (Hero) null, (NPC) null, currentRound);
                  }
                  else if (_cardActive.DiscardCardPlace == Enums.CardPlace.Discard)
                  {
                    MatchManager matchManager6 = matchManager1;
                    count = matchManager1.logDictionary.Count;
                    string _key = "toDiscard:" + count.ToString();
                    string internalId = cardItem.CardData.InternalId;
                    Hero _theHero = matchManager1.TeamHero[cardsNum];
                    int currentRound = matchManager1.currentRound;
                    matchManager6.CreateLogEntry(true, _key, internalId, _theHero, (NPC) null, (Hero) null, (NPC) null, currentRound);
                  }
                  else
                  {
                    MatchManager matchManager7 = matchManager1;
                    count = matchManager1.logDictionary.Count;
                    string _key = "toTopDeck:" + count.ToString();
                    string internalId = cardItem.CardData.InternalId;
                    Hero _theHero = matchManager1.TeamHero[cardsNum];
                    int currentRound = matchManager1.currentRound;
                    matchManager7.CreateLogEntry(true, _key, internalId, _theHero, (NPC) null, (Hero) null, (NPC) null, currentRound);
                  }
                  if (matchManager1.heroActive == cardsNum)
                    cardItem.DiscardCard(false, _cardActive.DiscardCardPlace);
                  else
                    cardItem.DiscardCard(false, Enums.CardPlace.Vanish);
                  CIAutomatic.Remove(cardItem);
                }
                matchManager1.DrawDeckPile(matchManager1.CountHeroDeck() + 1);
                matchManager1.DrawDiscardPileCardNumeral();
              }
              for (int index = 0; index < CIAutomatic.Count; ++index)
              {
                if (matchManager1.heroActive == cardsNum)
                  CIAutomatic[index].DiscardCard(false, Enums.CardPlace.TopDeck);
                else
                  CIAutomatic[index].DiscardCard(false, Enums.CardPlace.Vanish);
              }
              matchManager1.heroIndexWaitingForAddDiscard = -1;
              matchManager1.deckCardsWindow.TurnOff();
              yield return (object) Globals.Instance.WaitForSeconds(0.5f);
              UsedCardsId = (List<string>) null;
              CIAutomatic = (List<CardItem>) null;
            }
            matchManager1.gameStatus = "";
          }
        }
      }
      _cardActive = _cardActivePre;
      theCardItem = theCardItemPre;
      theCasterHero = theCasterHeroPre;
      matchManager1.theHero = theHeroPre;
      theCasterNPC = theCasterNPCPre;
      matchManager1.theNPC = theNPCPre;
      theCasterCharacter = theCasterCharacterPre;
      matchManager1.targetTransform = targetTransformPre;
      theCasterIsHero = theCasterIsHeroPre;
      theCardItemPostDiscard = theCardItemPostDiscardPre;
      matchManager1.heroIndexWaitingForAddDiscard = -1;
      if ((UnityEngine.Object) _cardActive.SummonUnit != (UnityEngine.Object) null)
      {
        _drawLoopCurrent = _cardActive.SummonUnitNum;
        if (_drawLoopCurrent == 0)
          _drawLoopCurrent = 3;
        for (cardsNum = 0; cardsNum < _drawLoopCurrent; ++cardsNum)
        {
          if (theCasterNPC != null)
            matchManager1.CreateNPC(_cardActive.SummonUnit, _cardActive.EffectTarget, _cardActive: _cardActive, _casterInternalId: theCasterNPC.InternalId, isSummon: true);
          else
            matchManager1.CreateNPC(_cardActive.SummonUnit, _cardActive.EffectTarget, _cardActive: _cardActive, isSummon: true);
          yield return (object) Globals.Instance.WaitForSeconds(0.1f);
        }
        if ((UnityEngine.Object) _cardActive != (UnityEngine.Object) null && _cardActive.Metamorph)
        {
          yield return (object) Globals.Instance.WaitForSeconds(0.25f);
          matchManager1.SetEventDirect("CastCardEvent" + comingId, false);
          matchManager1.CreateLogCastCard(false, _cardActive, _uniqueCastId, matchManager1.theHero, matchManager1.theNPC, (Hero) null, (NPC) null);
          matchManager1.EndTurn();
          yield break;
        }
      }
      if (_cardActive.DrawCard != 0 || _cardActive.DiscardCard != 0 || _cardActive.AddCard != 0 || _cardActive.LookCards != 0)
        matchManager1.SetEventDirect("CastCardEvent" + comingId, false, true);
      if (theCardItemPostDiscard != "")
        matchManager1.DiscardItem(theCardItemPostDiscard);
      if (matchManager1.cardIteration.ContainsKey(_cardActive.InternalId))
        matchManager1.cardIteration[_cardActive.InternalId] = 0;
      else
        matchManager1.cardIteration.Add(_cardActive.InternalId, 0);
      if (theCasterNPC != null)
        matchManager1.theHero = (Hero) null;
      if (!_cardActive.AutoplayDraw && !_cardActive.AutoplayEndTurn && (_cardActive.CardClass != Enums.CardClass.Special || _cardActive.CardClass == Enums.CardClass.Special && _cardActive.Playable))
      {
        if (theCasterHero != null)
        {
          for (int index = matchManager1.theHero.AuraList.Count - 1; index >= 0; --index)
          {
            if (matchManager1.theHero != null && matchManager1.theHero.AuraList[index] != null && (UnityEngine.Object) matchManager1.theHero.AuraList[index].ACData != (UnityEngine.Object) null && matchManager1.theHero.AuraList[index].ACData.Id == "stealth")
            {
              if ((_cardActive.HasCardType(Enums.CardType.Skill) || _cardActive.HasCardType(Enums.CardType.Enchantment)) && matchManager1.theHero.HaveTrait("veilofshadows") && (matchManager1.activatedTraits == null || !matchManager1.activatedTraits.ContainsKey("veilofshadows") || matchManager1.activatedTraits["veilofshadows"] < 2))
              {
                matchManager1.theHero.DoTraitFunction("veilofshadows");
                break;
              }
              matchManager1.theHero.SetAura(theCasterCharacter, Globals.Instance.GetAuraCurseData("stealthbonus"), matchManager1.theHero.AuraList[index].AuraCharges, CC: _cardActive.CardClass);
              matchManager1.theHero.HealAuraCurse(Globals.Instance.GetAuraCurseData("stealth"));
              break;
            }
          }
        }
        else if (theCasterNPC != null)
        {
          for (int index = matchManager1.theNPC.AuraList.Count - 1; index >= 0; --index)
          {
            if (matchManager1.theNPC != null && matchManager1.theNPC.AuraList[index] != null && (UnityEngine.Object) matchManager1.theNPC.AuraList[index].ACData != (UnityEngine.Object) null && matchManager1.theNPC.AuraList[index].ACData.Id == "stealth")
            {
              matchManager1.theNPC.SetAura(theCasterCharacter, Globals.Instance.GetAuraCurseData("stealthbonus"), matchManager1.theNPC.AuraList[index].AuraCharges, CC: _cardActive.CardClass);
              matchManager1.theNPC.HealAuraCurse(Globals.Instance.GetAuraCurseData("stealth"));
              break;
            }
          }
        }
      }
      Hero _targetHero1 = (Hero) null;
      NPC _targetNPC = (NPC) null;
      if ((UnityEngine.Object) matchManager1.targetTransform != (UnityEngine.Object) null && (UnityEngine.Object) matchManager1.targetTransform.GetComponent<HeroItem>() != (UnityEngine.Object) null)
        _targetHero1 = matchManager1.targetTransform.GetComponent<HeroItem>().Hero;
      if ((UnityEngine.Object) matchManager1.targetTransform != (UnityEngine.Object) null && (UnityEngine.Object) matchManager1.targetTransform.GetComponent<NPCItem>() != (UnityEngine.Object) null)
        _targetNPC = matchManager1.targetTransform.GetComponent<NPCItem>().NPC;
      int _cardSpecialValueGlobal = Functions.FuncRoundToInt(matchManager1.GetCardSpecialValue(_cardActive, 0, (Character) matchManager1.theHero, (Character) matchManager1.theNPC, (Character) _targetHero1, (Character) _targetNPC, false));
      matchManager1.cardItemActive = theCardItem;
      int _cardIterationTotal = 1;
      if (_cardActive.TargetType == Enums.CardTargetType.Global)
      {
        _drawLoopCurrent = 0;
        cardsNum = 0;
        List<Hero> targetsHero = new List<Hero>();
        for (int index = 0; index < matchManager1.TeamHero.Length; ++index)
        {
          if (matchManager1.TeamHero[index] != null && matchManager1.TeamHero[index].Alive && (UnityEngine.Object) matchManager1.TeamHero[index].HeroItem != (UnityEngine.Object) null && matchManager1.CheckTarget(matchManager1.TeamHero[index].HeroItem.transform, _cardActive, theCasterIsHero))
          {
            targetsHero.Add(matchManager1.TeamHero[index]);
            ++_drawLoopCurrent;
          }
        }
        List<NPC> targetsNPC = new List<NPC>();
        for (int index = 0; index < matchManager1.TeamNPC.Length; ++index)
        {
          if (matchManager1.TeamNPC[index] != null && matchManager1.TeamNPC[index].Alive && (UnityEngine.Object) matchManager1.TeamNPC[index].NPCItem != (UnityEngine.Object) null && matchManager1.CheckTarget(matchManager1.TeamNPC[index].NPCItem.transform, _cardActive, theCasterIsHero))
          {
            targetsNPC.Add(matchManager1.TeamNPC[index]);
            ++cardsNum;
          }
        }
        _cardIterationTotal = _drawLoopCurrent + cardsNum;
        for (indexGameBusy = 0; indexGameBusy < _drawLoopCurrent; ++indexGameBusy)
        {
          if (targetsHero != null && indexGameBusy < targetsHero.Count && targetsHero[indexGameBusy] != null && targetsHero[indexGameBusy].Alive && (UnityEngine.Object) targetsHero[indexGameBusy].HeroItem != (UnityEngine.Object) null)
          {
            matchManager1.targetTransform = targetsHero[indexGameBusy].HeroItem.transform;
            matchManager1.StartCoroutine(matchManager1.CastCardAction(_cardActive, matchManager1.targetTransform, theCardItem, _uniqueCastId, _automatic, _card, _cardIterationTotal, _cardSpecialValueGlobal));
            yield return (object) Globals.Instance.WaitForSeconds(0.01f);
          }
        }
        for (indexGameBusy = 0; indexGameBusy < cardsNum; ++indexGameBusy)
        {
          if (targetsNPC != null && indexGameBusy < targetsNPC.Count && targetsNPC[indexGameBusy] != null && targetsNPC[indexGameBusy].Alive && (UnityEngine.Object) targetsNPC[indexGameBusy].NPCItem != (UnityEngine.Object) null)
          {
            matchManager1.targetTransform = targetsNPC[indexGameBusy].NPCItem.transform;
            matchManager1.StartCoroutine(matchManager1.CastCardAction(_cardActive, matchManager1.targetTransform, theCardItem, _uniqueCastId, _automatic, _card, _cardIterationTotal, _cardSpecialValueGlobal));
            yield return (object) Globals.Instance.WaitForSeconds(0.01f);
          }
        }
        yield return (object) null;
        targetsHero = (List<Hero>) null;
        targetsNPC = (List<NPC>) null;
      }
      else if (_cardActive.TargetPosition == Enums.CardTargetPosition.Random)
      {
        List<Transform> castTransformList = matchManager1.GetInstaCastTransformList(_cardActive, theCasterIsHero);
        int randomIntRange = matchManager1.GetRandomIntRange(0, castTransformList.Count);
        if (randomIntRange < castTransformList.Count)
          matchManager1.StartCoroutine(matchManager1.CastCardAction(_cardActive, castTransformList[randomIntRange], theCardItem, _uniqueCastId, _automatic, _card, 1, _cardSpecialValueGlobal));
        yield return (object) null;
      }
      else
      {
        if (_cardActive.TargetPosition == Enums.CardTargetPosition.Front || _cardActive.TargetPosition == Enums.CardTargetPosition.Back)
        {
          if (matchManager1.theHero != null)
          {
            for (int index = 0; index < matchManager1.TeamNPC.Length; ++index)
            {
              if (matchManager1.TeamNPC[index] != null && !((UnityEngine.Object) matchManager1.TeamNPC[index].NpcData == (UnityEngine.Object) null) && matchManager1.TeamNPC[index].Alive && matchManager1.CheckTarget(matchManager1.TeamNPC[index].NPCItem.transform, _cardActive))
              {
                matchManager1.targetTransform = matchManager1.TeamNPC[index].NPCItem.transform;
                break;
              }
            }
          }
          else
          {
            NPC theNpc = matchManager1.theNPC;
          }
        }
        matchManager1.StartCoroutine(matchManager1.CastCardAction(_cardActive, matchManager1.targetTransform, theCardItem, _uniqueCastId, _automatic, _card, 1, _cardSpecialValueGlobal));
        yield return (object) null;
      }
      foreach (NPC currentNPC in matchManager1.TeamNPC)
      {
        NPC targetNPC;
        if (matchManager1.TryGetTransferPainTarget(_cardActive, currentNPC, out targetNPC))
          matchManager1.ApplyTransferPain(currentNPC, targetNPC);
      }
      if (matchManager1.CanUpdateMindSpikeProgress(_cardActive))
        matchManager1.mindSpikeAbility.IncreaseSpecialCardCount();
      Character casterCharacter = theCasterNPC != null ? (Character) theCasterNPC : (Character) theCasterHero;
      List<NPC> npcsWithAbility1;
      if (matchManager1.TryGetNPCsWithMasterReweaverAbility(_cardActive, casterCharacter, out npcsWithAbility1))
      {
        foreach (NPC npcWithAbility in npcsWithAbility1)
          matchManager1.UpdateMasterReweaverProgress(casterCharacter, npcWithAbility);
      }
      if (matchManager1.GetCharacterActive().IsHero)
      {
        Action<Hero, CardData> cardCastedByHero = matchManager1.OnCardCastedByHero;
        if (cardCastedByHero != null)
          cardCastedByHero(matchManager1.theHero, matchManager1.cardActive);
      }
      yield return (object) null;
    }
  }

  public void RaiseCardCastBegin(CardItem theCardItem)
  {
    if (!((UnityEngine.Object) theCardItem != (UnityEngine.Object) null) || this.heroActive < 0 || this.heroActive >= this.TeamHero.Length || this.TeamHero[this.heroActive] == null)
      return;
    Action<Hero, CardData> castByHeroBegins = this.OnCardCastByHeroBegins;
    if (castByHeroBegins == null)
      return;
    castByHeroBegins(this.TeamHero[this.heroActive], theCardItem.CardData);
  }

  public IEnumerator CastCardAction(
    CardData _cardActive,
    Transform targetTransformCast,
    CardItem theCardItem,
    string _uniqueCastId,
    bool _automatic,
    CardData _card,
    int _cardIterationTotal,
    int _cardSpecialValueGlobal)
  {
    MatchManager matchManager = this;
    bool youCanCastEffect = false;
    bool isYourFirstTarget = false;
    if (!matchManager.castedCards.Contains(_uniqueCastId))
    {
      matchManager.castedCards.Add(_uniqueCastId);
      youCanCastEffect = true;
      isYourFirstTarget = true;
    }
    matchManager.gameStatus = "CastingCardAction";
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD(matchManager.gameStatus, "gamestatus");
    if (!matchManager.waitingForCardEnergyAssignment)
    {
      if (!_automatic)
      {
        _cardActive = theCardItem.CardData;
        theCardItem.discard = true;
      }
      else if ((UnityEngine.Object) _cardActive == (UnityEngine.Object) null && (UnityEngine.Object) _card != (UnityEngine.Object) null)
        _cardActive = _card;
      matchManager.cardActive = _cardActive;
      int cardSpecialValueGlobal = _cardSpecialValueGlobal;
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("CastCardAction " + _cardActive.Id, "general");
      Hero theCasterHero = matchManager.theHero;
      NPC theCasterNPC = matchManager.theNPC;
      Character theCasterCharacter = (Character) null;
      string theCasterName = "";
      string theCasterId = "";
      bool theCasterIsHero = false;
      if (theCasterHero != null)
      {
        theCasterIsHero = true;
        theCasterCharacter = (Character) theCasterHero;
        theCasterName = matchManager.theHero.GameName;
        theCasterId = matchManager.theHero.Id;
        if (!_cardActive.AutoplayDraw && !_cardActive.AutoplayEndTurn && youCanCastEffect && _cardActive.EffectPreAction == "" && !_cardActive.IsPetCast && !_cardActive.IsPetAttack)
        {
          if (_cardActive.MoveToCenter)
            matchManager.theHero.HeroItem.CharacterAttackAnim();
          else
            matchManager.theHero.HeroItem.CharacterCastAnim();
        }
      }
      else if (theCasterNPC != null)
      {
        theCasterCharacter = (Character) theCasterNPC;
        theCasterName = matchManager.theNPC.GameName;
        theCasterId = matchManager.theNPC.Id;
      }
      if (matchManager.theHero != null && (UnityEngine.Object) matchManager.theHero.HeroItem != (UnityEngine.Object) null)
      {
        if (_cardActive.IsPetCast)
          matchManager.theHero.HeroItem.PetCastAnim("cast");
        else if (_cardActive.IsPetAttack)
          matchManager.theHero.HeroItem.PetCastAnim("attack");
      }
      else if (matchManager.theNPC != null && (UnityEngine.Object) matchManager.theNPC.NPCItem != (UnityEngine.Object) null)
      {
        if (_cardActive.IsPetCast)
          matchManager.theNPC.NPCItem.PetCastAnim("cast");
        else if (_cardActive.IsPetAttack)
          matchManager.theNPC.NPCItem.PetCastAnim("attack");
      }
      Hero _hero = (Hero) null;
      NPC _npc = (NPC) null;
      HeroItem _heroItem = (HeroItem) null;
      NPCItem _npcItem = (NPCItem) null;
      if ((UnityEngine.Object) targetTransformCast == (UnityEngine.Object) null || !(bool) (UnityEngine.Object) targetTransformCast.GetComponent<HeroItem>() && !(bool) (UnityEngine.Object) targetTransformCast.GetComponent<NPCItem>())
      {
        List<Transform> castTransformList = matchManager.GetInstaCastTransformList(_cardActive, theCasterIsHero);
        if (castTransformList.Count == 1)
          targetTransformCast = castTransformList[0];
        else if (theCasterHero != null)
        {
          if ((UnityEngine.Object) matchManager.theHero.HeroItem != (UnityEngine.Object) null)
            targetTransformCast = matchManager.theHero.HeroItem.transform;
        }
        else if ((UnityEngine.Object) matchManager.theNPC.NPCItem != (UnityEngine.Object) null)
          targetTransformCast = matchManager.theNPC.NPCItem.transform;
      }
      if ((UnityEngine.Object) targetTransformCast != (UnityEngine.Object) null)
      {
        _heroItem = targetTransformCast.GetComponent<HeroItem>();
        _npcItem = targetTransformCast.GetComponent<NPCItem>();
      }
      bool isHero = false;
      bool isNPC = false;
      if ((UnityEngine.Object) _heroItem != (UnityEngine.Object) null)
      {
        isHero = true;
        if ((UnityEngine.Object) targetTransformCast != (UnityEngine.Object) null)
          _hero = matchManager.GetHeroById(targetTransformCast.name);
      }
      else
      {
        isNPC = true;
        if ((UnityEngine.Object) targetTransformCast != (UnityEngine.Object) null)
          _npc = matchManager.GetNPCById(targetTransformCast.name);
      }
      matchManager.CreateLogCastCard(true, _cardActive, _uniqueCastId, matchManager.theHero, matchManager.theNPC, _hero, _npc);
      int repeatCast = _cardActive.EffectRepeat;
      if (repeatCast < 1)
        repeatCast = _cardActive.EffectRepeat = 1;
      if (_cardActive.EffectRepeatEnergyBonus > 0 && matchManager.energyAssigned > 0)
        repeatCast *= matchManager.energyAssigned + 1;
      List<string> targetsUsed = new List<string>();
      List<string> targetElegible = new List<string>();
      float effectRepeatPercent = 100f;
      bool doRedrawInitiatives = false;
      if ((UnityEngine.Object) targetTransformCast == (UnityEngine.Object) null)
        repeatCast = 0;
      int iteration;
      string lastId;
      bool castedEffect;
      int _indexQueue;
      int cardSpecialValue2;
      for (iteration = 0; iteration < repeatCast; ++iteration)
      {
        matchManager.castCardDamageDoneIteration = 0.0f;
        if ((theCasterHero == null || !((UnityEngine.Object) theCasterHero.HeroItem == (UnityEngine.Object) null)) && (theCasterNPC == null || !((UnityEngine.Object) theCasterNPC.NPCItem == (UnityEngine.Object) null)))
        {
          lastId = "";
          matchManager.castCardDamageDone = 0.0f;
          if (iteration == 0 && Globals.Instance.ShowDebug)
            Functions.DebugLogGD("RAND 1 => " + matchManager.randomForIteration.ToString(), "randomindex");
          if (iteration > 0)
          {
            if (matchManager.CheckMatchIsOver())
            {
              matchManager.FinishCombat();
              yield break;
            }
            else
            {
              matchManager.randomForIteration += 5;
              if (matchManager.randomForIteration >= matchManager.randomStringArr.Count)
                matchManager.randomForIteration = 0;
              matchManager.SetRandomIndex(matchManager.randomForIteration);
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("INCREASE RANDOM FOR ITERATION => " + matchManager.randomForIteration.ToString(), "randomindex");
              if (_cardActive.EffectRepeatModificator != 0)
                effectRepeatPercent += (float) ((double) effectRepeatPercent * (double) _cardActive.EffectRepeatModificator * 0.0099999997764825821);
              if (_cardActive.EffectRepeatTarget != Enums.EffectRepeatTarget.Same)
              {
                bool flag = false;
                string theId = "";
                if (_cardActive.EffectRepeatTarget == Enums.EffectRepeatTarget.Random)
                {
                  if (isHero)
                  {
                    int num = 0;
                    while (!flag)
                    {
                      int randomIntRange = matchManager.GetRandomIntRange(0, matchManager.TeamHero.Length);
                      if (matchManager.TeamHero[randomIntRange] != null && matchManager.TeamHero[randomIntRange].Alive && matchManager.CheckTarget(matchManager.TeamHero[randomIntRange].HeroItem.transform, _cardActive, theCasterIsHero))
                      {
                        theId = matchManager.TeamHero[randomIntRange].Id;
                        flag = true;
                      }
                      ++num;
                      if (num > 1000)
                      {
                        theId = "";
                        flag = true;
                      }
                    }
                  }
                  else
                  {
                    int num = 0;
                    while (!flag)
                    {
                      int randomIntRange = matchManager.GetRandomIntRange(0, matchManager.TeamNPC.Length);
                      if (matchManager.TeamNPC[randomIntRange] != null && matchManager.TeamNPC[randomIntRange].Alive && (UnityEngine.Object) matchManager.TeamNPC[randomIntRange].NPCItem != (UnityEngine.Object) null && matchManager.CheckTarget(matchManager.TeamNPC[randomIntRange].NPCItem.transform, _cardActive, theCasterIsHero))
                      {
                        theId = matchManager.TeamNPC[randomIntRange].Id;
                        flag = true;
                      }
                      ++num;
                      if (num > 1000)
                      {
                        theId = "";
                        flag = true;
                      }
                    }
                  }
                }
                else if (_cardActive.EffectRepeatTarget == Enums.EffectRepeatTarget.Chain)
                {
                  lastId = targetsUsed.Last<string>();
                  if (Globals.Instance.ShowDebug)
                    Functions.DebugLogGD("LastId->" + lastId);
                  targetElegible.Clear();
                  if (isHero)
                  {
                    int position = matchManager.GetHeroById(lastId).Position;
                    int num1 = position - 1;
                    int num2 = matchManager.GetHeroById(lastId).Alive ? position + 1 : position;
                    for (int index = 0; index < matchManager.TeamHero.Length; ++index)
                    {
                      if (matchManager.TeamHero[index] != null && !((UnityEngine.Object) matchManager.TeamHero[index].HeroData == (UnityEngine.Object) null) && matchManager.TeamHero[index].Alive && matchManager.TeamHero[index].Id != lastId && (matchManager.TeamHero[index].Position == num1 || matchManager.TeamHero[index].Position == num2))
                        targetElegible.Add(matchManager.TeamHero[index].Id);
                    }
                  }
                  else
                  {
                    NPC npcById = matchManager.GetNPCById(lastId);
                    if (npcById != null)
                    {
                      int position = npcById.Position;
                      int num3 = position - 1;
                      int num4 = position + 1;
                      if (!npcById.Alive)
                        num4 = position;
                      for (int index = 0; index < matchManager.TeamNPC.Length; ++index)
                      {
                        if (matchManager.TeamNPC[index] != null && !((UnityEngine.Object) matchManager.TeamNPC[index].NpcData == (UnityEngine.Object) null) && matchManager.TeamNPC[index].Alive && matchManager.TeamNPC[index].Id != lastId && (matchManager.TeamNPC[index].Position == num3 || matchManager.TeamNPC[index].Position == num4))
                          targetElegible.Add(matchManager.TeamNPC[index].Id);
                      }
                    }
                  }
                  if (targetElegible.Count > 0)
                    theId = targetElegible[matchManager.GetRandomIntRange(0, targetElegible.Count)];
                }
                else if (_cardActive.EffectRepeatTarget == Enums.EffectRepeatTarget.NoRepeat)
                {
                  lastId = targetsUsed.Last<string>();
                  targetElegible.Clear();
                  if (isHero)
                  {
                    for (int index = 0; index < matchManager.TeamHero.Length; ++index)
                    {
                      if (matchManager.TeamHero[index] != null && !((UnityEngine.Object) matchManager.TeamHero[index].HeroData == (UnityEngine.Object) null) && matchManager.TeamHero[index].Alive && matchManager.TeamHero[index].Id != lastId && _hero.Id != matchManager.TeamHero[index].Id)
                        targetElegible.Add(matchManager.TeamHero[index].Id);
                    }
                  }
                  else
                  {
                    for (int index = 0; index < matchManager.TeamNPC.Length; ++index)
                    {
                      if (matchManager.TeamNPC[index] != null && !((UnityEngine.Object) matchManager.TeamNPC[index].NpcData == (UnityEngine.Object) null) && matchManager.TeamNPC[index].Alive && matchManager.TeamNPC[index].Id != lastId && _npc.Id != matchManager.TeamNPC[index].Id)
                        targetElegible.Add(matchManager.TeamNPC[index].Id);
                    }
                  }
                  if (targetElegible.Count > 0)
                    theId = targetElegible[matchManager.GetRandomIntRange(0, targetElegible.Count)];
                }
                else if (_cardActive.EffectRepeatTarget == Enums.EffectRepeatTarget.NeverRepeat)
                {
                  targetElegible.Clear();
                  if (isHero)
                  {
                    for (int index = 0; index < matchManager.TeamHero.Length; ++index)
                    {
                      if (matchManager.TeamHero[index] != null && !((UnityEngine.Object) matchManager.TeamHero[index].HeroData == (UnityEngine.Object) null) && matchManager.TeamHero[index].Alive && !targetsUsed.Contains(matchManager.TeamHero[index].Id))
                        targetElegible.Add(matchManager.TeamHero[index].Id);
                    }
                  }
                  else
                  {
                    for (int index = 0; index < matchManager.TeamNPC.Length; ++index)
                    {
                      if (matchManager.TeamNPC[index] != null && !((UnityEngine.Object) matchManager.TeamNPC[index].NpcData == (UnityEngine.Object) null) && matchManager.TeamNPC[index].Alive && !targetsUsed.Contains(matchManager.TeamNPC[index].Id))
                        targetElegible.Add(matchManager.TeamNPC[index].Id);
                    }
                  }
                  if (targetElegible.Count > 0)
                    theId = targetElegible[matchManager.GetRandomIntRange(0, targetElegible.Count)];
                }
                if (theId != "")
                {
                  if (isHero)
                  {
                    _hero = matchManager.GetHeroById(theId);
                    targetTransformCast = _hero.HeroItem.transform;
                  }
                  else
                  {
                    _npc = matchManager.GetNPCById(theId);
                    targetTransformCast = _npc.NPCItem.transform;
                  }
                  targetsUsed.Add(theId);
                }
                else
                {
                  targetTransformCast = (Transform) null;
                  repeatCast = 0;
                }
                if ((UnityEngine.Object) targetTransformCast == (UnityEngine.Object) null)
                  matchManager.targetTransform = (Transform) null;
              }
              if (_npc != null)
              {
                _npcItem = _npc.NPCItem;
                if (!_npc.Alive || matchManager.bossNpc != null && !matchManager.bossNpc.IsValidTarget())
                  break;
              }
              if (_hero != null)
                _heroItem = _hero.HeroItem;
            }
          }
          else
          {
            if (_hero != null)
              targetsUsed.Add(_hero.Id);
            if (_npc != null)
              targetsUsed.Add(_npc.Id);
          }
          int exahustGenerating;
          if (theCasterNPC != null && _cardActive.AddCardId != "" && _hero != null && _hero.Alive)
          {
            CardData cardForModify = matchManager.GetCardForModify(_cardActive, (Character) theCasterNPC, (Character) _hero);
            matchManager.GenerateNewCard(_cardActive.AddCard, _cardActive.AddCardId, true, _cardActive.AddCardPlace, cardForModify, heroIndex: _hero.HeroIndex, copyConfig: _cardActive.CopyConfig);
            yield return (object) Globals.Instance.WaitForSeconds(1f);
            for (exahustGenerating = 0; matchManager.generatedCardTimes > 0 && exahustGenerating < 10; ++exahustGenerating)
              yield return (object) Globals.Instance.WaitForSeconds(0.1f);
          }
          castedEffect = false;
          if ((UnityEngine.Object) targetTransformCast != (UnityEngine.Object) null)
          {
            castedEffect = false;
            if (iteration == 0 && _cardActive.MoveToCenter)
            {
              if (youCanCastEffect && matchManager.cardIteration.ContainsKey(_cardActive.InternalId) && matchManager.cardIteration[_cardActive.InternalId] == 0)
              {
                if (theCasterHero != null && (UnityEngine.Object) theCasterHero.HeroItem != (UnityEngine.Object) null && !_cardActive.TempAttackSelf)
                  theCasterHero.HeroItem.MoveToCenter();
                else if (theCasterNPC != null && (UnityEngine.Object) theCasterNPC.NPCItem != (UnityEngine.Object) null && !matchManager.IsPhantomArmorSpecialCard(_cardActive.Id))
                  theCasterNPC.NPCItem.MoveToCenter();
              }
              if (theCasterHero != null && (UnityEngine.Object) theCasterHero.HeroItem != (UnityEngine.Object) null)
              {
                while (theCasterHero.HeroItem.CharIsMoving())
                  yield return (object) null;
              }
              else if (theCasterNPC != null && (UnityEngine.Object) theCasterNPC.NPCItem != (UnityEngine.Object) null)
              {
                while (theCasterNPC.NPCItem.CharIsMoving())
                  yield return (object) null;
              }
              if (_automatic && _cardActive.AddCard > 0 && _cardActive.MoveToCenter && theCasterNPC != null && (UnityEngine.Object) theCasterNPC.NPCItem != (UnityEngine.Object) null)
                theCasterNPC.NPCItem.CharacterAttackAnim();
            }
            if ((UnityEngine.Object) _cardActive != (UnityEngine.Object) null && _cardActive.EffectCaster.Trim() != "" && (iteration == 0 || _cardActive.EffectCasterRepeat) && youCanCastEffect)
            {
              if (theCasterNPC != null && (UnityEngine.Object) theCasterNPC.NPCItem != (UnityEngine.Object) null)
                EffectsManager.Instance.PlayEffect(_cardActive, true, false, theCasterNPC.NPCItem.CharImageT);
              else if (theCasterHero != null && (UnityEngine.Object) theCasterHero.HeroItem != (UnityEngine.Object) null)
                EffectsManager.Instance.PlayEffect(_cardActive, true, true, theCasterHero.HeroItem.CharImageT);
            }
            exahustGenerating = Functions.FuncRoundToInt(matchManager.GetCardSpecialValue(_cardActive, 1, (Character) theCasterHero, (Character) theCasterNPC, (Character) _hero, (Character) _npc, false));
            cardSpecialValue2 = Functions.FuncRoundToInt(matchManager.GetCardSpecialValue(_cardActive, 2, (Character) theCasterHero, (Character) theCasterNPC, (Character) _hero, (Character) _npc, false));
            if ((UnityEngine.Object) _cardActive.ItemEnchantment != (UnityEngine.Object) null)
            {
              string id = _cardActive.ItemEnchantment.Id;
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("ENCHANTMENT ->" + id);
              if (_hero != null)
              {
                _hero.AssignEnchantment(id);
                _hero.HeroItem.ShowEnchantments();
              }
              else
              {
                _npc.AssignEnchantment(id);
                _npc.NPCItem.ShowEnchantments();
              }
            }
            else if (_cardActive.Playable && (UnityEngine.Object) _cardActive.PetModel != (UnityEngine.Object) null && _cardActive.PetTemporal)
            {
              matchManager.StartCoroutine(matchManager.CreateCardCastPet(_cardActive, theCasterHero.HeroItem.gameObject, theCasterHero, (NPC) null));
              yield return (object) Globals.Instance.WaitForSeconds(0.75f);
            }
            if (_cardActive.TransferCurses > 0)
            {
              List<string> curseList = new List<string>();
              List<int> intList = new List<int>();
              int num = 0;
              if (theCasterCharacter != null)
              {
                for (int index = 0; index < theCasterCharacter.AuraList.Count && num < _cardActive.TransferCurses; ++index)
                {
                  if (theCasterCharacter.AuraList[index] != null && (UnityEngine.Object) theCasterCharacter.AuraList[index].ACData != (UnityEngine.Object) null && !theCasterCharacter.AuraList[index].ACData.IsAura && theCasterCharacter.AuraList[index].ACData.Removable && theCasterCharacter.AuraList[index].GetCharges() > 0)
                  {
                    curseList.Add(theCasterCharacter.AuraList[index].ACData.Id);
                    intList.Add(theCasterCharacter.AuraList[index].GetCharges());
                    ++num;
                  }
                }
              }
              if (num > 0)
              {
                theCasterCharacter.HealCursesName(curseList);
                for (int index = 0; index < curseList.Count; ++index)
                {
                  if (_hero != null && _hero.Alive)
                    _hero.SetAura(theCasterCharacter, Globals.Instance.GetAuraCurseData(curseList[index]), intList[index], CC: _cardActive.CardClass);
                  else if (_npc != null && _npc.Alive)
                    _npc.SetAura(theCasterCharacter, Globals.Instance.GetAuraCurseData(curseList[index]), intList[index], CC: _cardActive.CardClass);
                }
              }
              yield return (object) Globals.Instance.WaitForSeconds(0.1f);
            }
            if (_cardActive.StealAuras > 0)
            {
              List<string> curseList = new List<string>();
              List<int> intList = new List<int>();
              int num = 0;
              Character theCaster = (Character) null;
              if (_hero != null && _hero.Alive)
                theCaster = (Character) _hero;
              else if (_npc != null && _npc.Alive)
                theCaster = (Character) _npc;
              if (theCaster != null)
              {
                for (int index = 0; index < theCaster.AuraList.Count && num < _cardActive.StealAuras; ++index)
                {
                  if (theCaster.AuraList[index] != null && (UnityEngine.Object) theCaster.AuraList[index].ACData != (UnityEngine.Object) null && theCaster.AuraList[index].ACData.IsAura && theCaster.AuraList[index].ACData.Removable && theCaster.AuraList[index].GetCharges() > 0)
                  {
                    curseList.Add(theCaster.AuraList[index].ACData.Id);
                    intList.Add(theCaster.AuraList[index].GetCharges());
                    ++num;
                  }
                }
              }
              if (num > 0)
              {
                theCaster.HealCursesName(curseList);
                for (int index = 0; index < curseList.Count; ++index)
                {
                  if (theCasterCharacter != null && theCasterCharacter.Alive)
                    theCasterCharacter.SetAura(theCaster, Globals.Instance.GetAuraCurseData(curseList[index]), intList[index], CC: _cardActive.CardClass);
                }
              }
              yield return (object) Globals.Instance.WaitForSeconds(0.1f);
            }
            if (_cardActive.AddVanishToDeck)
            {
              if (_hero != null)
              {
                foreach (string id in matchManager.HeroDeck[_hero.HeroIndex])
                  matchManager.GetCardData(id).Vanish = true;
              }
              else if (_npc != null)
              {
                foreach (string id in matchManager.NPCDeck[_npc.NPCIndex])
                  matchManager.GetCardData(id).Vanish = true;
              }
            }
            if (_cardActive.HealCurses != 0)
            {
              if (_hero != null && _hero.Alive)
                _hero.HealCurses(_cardActive.HealCurses);
              else if (_npc != null && _npc.Alive)
                _npc.HealCurses(_cardActive.HealCurses);
              if (!doRedrawInitiatives)
                doRedrawInitiatives = true;
            }
            if (isYourFirstTarget && (UnityEngine.Object) _cardActive.HealAuraCurseSelf != (UnityEngine.Object) null && _cardActive.HealAuraCurseSelf.Id != "spellsword")
              theCasterCharacter.HealAuraCurse(_cardActive.HealAuraCurseSelf);
            List<string> curseList1 = new List<string>();
            if ((UnityEngine.Object) _cardActive.HealAuraCurseName != (UnityEngine.Object) null)
              curseList1.Add(_cardActive.HealAuraCurseName.Id);
            if ((UnityEngine.Object) _cardActive.HealAuraCurseName2 != (UnityEngine.Object) null)
              curseList1.Add(_cardActive.HealAuraCurseName2.Id);
            if ((UnityEngine.Object) _cardActive.HealAuraCurseName3 != (UnityEngine.Object) null)
              curseList1.Add(_cardActive.HealAuraCurseName3.Id);
            if ((UnityEngine.Object) _cardActive.HealAuraCurseName4 != (UnityEngine.Object) null)
              curseList1.Add(_cardActive.HealAuraCurseName4.Id);
            if (curseList1.Count > 0)
            {
              if (_hero != null && _hero.Alive)
                _hero.HealCursesName(curseList1);
              else if (_npc != null && _npc.Alive)
                _npc.HealCursesName(curseList1);
              if (!doRedrawInitiatives)
                doRedrawInitiatives = true;
            }
            if (_cardActive.IncreaseAuras > 0 || _cardActive.IncreaseCurses > 0 || _cardActive.ReduceAuras > 0 || _cardActive.ReduceCurses > 0)
            {
              Character _characterTarget = (Character) null;
              if (_hero != null && _hero.Alive)
                _characterTarget = (Character) _hero;
              else if (_npc != null && _npc.Alive)
                _characterTarget = (Character) _npc;
              if (_characterTarget != null)
              {
                for (int index1 = 0; index1 < 4; ++index1)
                {
                  if ((index1 != 0 || _cardActive.IncreaseAuras > 0) && (index1 != 1 || _cardActive.IncreaseCurses > 0) && (index1 != 2 || _cardActive.ReduceAuras > 0) && (index1 != 3 || _cardActive.ReduceCurses > 0))
                  {
                    List<string> stringList = new List<string>();
                    List<int> intList = new List<int>();
                    for (int index2 = 0; index2 < _characterTarget.AuraList.Count; ++index2)
                    {
                      if (_characterTarget.AuraList[index2] != null && (UnityEngine.Object) _characterTarget.AuraList[index2].ACData != (UnityEngine.Object) null && _characterTarget.AuraList[index2].GetCharges() > 0 && !(_characterTarget.AuraList[index2].ACData.Id == "furnace") && !(_characterTarget.AuraList[index2].ACData.Id == "spellsword"))
                      {
                        bool flag = false;
                        if ((index1 == 0 || index1 == 2) && _characterTarget.AuraList[index2].ACData.IsAura)
                          flag = true;
                        else if ((index1 == 1 || index1 == 3) && !_characterTarget.AuraList[index2].ACData.IsAura)
                          flag = true;
                        if (flag)
                        {
                          stringList.Add(_characterTarget.AuraList[index2].ACData.Id);
                          intList.Add(_characterTarget.AuraList[index2].GetCharges());
                        }
                      }
                    }
                    if (stringList.Count > 0)
                    {
                      for (int index3 = 0; index3 < stringList.Count; ++index3)
                      {
                        int num;
                        switch (index1)
                        {
                          case 0:
                            num = Functions.FuncRoundToInt((float) ((double) intList[index3] * (double) _cardActive.IncreaseAuras / 100.0));
                            break;
                          case 1:
                            num = Functions.FuncRoundToInt((float) ((double) intList[index3] * (double) _cardActive.IncreaseCurses / 100.0));
                            break;
                          case 2:
                            num = intList[index3] - Functions.FuncRoundToInt((float) ((double) intList[index3] * (double) _cardActive.ReduceAuras / 100.0));
                            break;
                          default:
                            num = intList[index3] - Functions.FuncRoundToInt((float) ((double) intList[index3] * (double) _cardActive.ReduceCurses / 100.0));
                            break;
                        }
                        switch (index1)
                        {
                          case 0:
                          case 1:
                            AuraCurseData _acData = AtOManager.Instance.GlobalAuraCurseModificationByTraitsAndItems("set", stringList[index3], theCasterCharacter, _characterTarget);
                            if ((UnityEngine.Object) _acData != (UnityEngine.Object) null)
                            {
                              int maxCharges = _acData.GetMaxCharges();
                              if (maxCharges > -1 && intList[index3] + num > maxCharges)
                                num = maxCharges - intList[index3];
                              _characterTarget.SetAura(theCasterCharacter, _acData, num, CC: _cardActive.CardClass, useCharacterMods: false, canBePreventable: false);
                              break;
                            }
                            break;
                          case 2:
                          case 3:
                            if (num <= 0)
                              num = 1;
                            _characterTarget.ModifyAuraCurseCharges(stringList[index3], num);
                            _characterTarget.UpdateAuraCurseFunctions();
                            break;
                        }
                      }
                    }
                  }
                }
              }
            }
            if (_cardActive.DispelAuras != 0)
            {
              if (_hero != null && _hero.Alive)
                _hero.DispelAuras(_cardActive.DispelAuras);
              else if (_npc != null && _npc.Alive)
                _npc.DispelAuras(_cardActive.DispelAuras);
              if (!doRedrawInitiatives)
                doRedrawInitiatives = true;
            }
            int directAttackIteration;
            bool damagedEventFired;
            bool blockedEventFired;
            bool fullEvadedDmg;
            bool fullEvadedDmgSide1;
            bool fullEvadedDmgSide2;
            bool ignoreBlock;
            int i;
            int dmg;
            int dmgTotal;
            int blocked;
            if (_cardActive.Damage > 0 || _cardActive.Damage2 > 0 || _cardActive.DamageSides > 0 || _cardActive.DamageSides2 > 0 || _cardActive.DamageSelf > 0 || _cardActive.DamageSelf2 > 0)
            {
              bool _isHero = isHero;
              bool _isNPC = isNPC;
              Hero _heroBeforeDmg = _hero;
              HeroItem _heroItemBeforeDmg = _heroItem;
              NPC _npcBeforeDmg = _npc;
              NPCItem _npcItemBeforeDmg = _npcItem;
              Dictionary<HeroItem, CastResolutionForCombatText> ctHero = new Dictionary<HeroItem, CastResolutionForCombatText>();
              Dictionary<NPCItem, CastResolutionForCombatText> ctNPC = new Dictionary<NPCItem, CastResolutionForCombatText>();
              CastResolutionForCombatText CRT = new CastResolutionForCombatText();
              if ((UnityEngine.Object) _npcItem != (UnityEngine.Object) null)
                ctNPC[_npcItem] = CRT;
              if ((UnityEngine.Object) _heroItem != (UnityEngine.Object) null)
                ctHero[_heroItem] = CRT;
              directAttackIteration = 0;
              damagedEventFired = false;
              blockedEventFired = false;
              fullEvadedDmg = false;
              fullEvadedDmgSide1 = false;
              fullEvadedDmgSide2 = false;
              ignoreBlock = false;
              for (i = 1; i <= 8; ++i)
              {
                if (i == 1 || i == 3 || i == 4)
                  damagedEventFired = false;
                if ((!fullEvadedDmg || i != 2) && (!fullEvadedDmgSide1 || i != 5) && (!fullEvadedDmgSide2 || i != 6))
                {
                  dmg = 0;
                  Enums.DamageType dmgType = Enums.DamageType.None;
                  switch (i)
                  {
                    case 1:
                      dmg = _cardActive.Damage;
                      if (_cardActive.DamageEnergyBonus > 0 && matchManager.energyAssigned > 0)
                        dmg += _cardActive.DamageEnergyBonus * matchManager.energyAssigned;
                      ignoreBlock = _cardActive.GetIgnoreBlock();
                      if (dmg > 0)
                      {
                        dmgType = _cardActive.DamageType;
                        break;
                      }
                      continue;
                    case 2:
                      if (_cardActive.DamageType != _cardActive.DamageType2)
                      {
                        dmg = _cardActive.Damage2;
                        ignoreBlock = _cardActive.GetIgnoreBlock(1);
                        if (dmg > 0)
                        {
                          dmgType = _cardActive.DamageType2;
                          break;
                        }
                        continue;
                      }
                      continue;
                    default:
                      if (i == 3 || i == 4 || i == 5 || i == 6)
                      {
                        CRT = new CastResolutionForCombatText();
                        if (i == 3 || i == 4)
                        {
                          dmg = _cardActive.DamageSides;
                          ignoreBlock = _cardActive.GetIgnoreBlock();
                          if (dmg > 0)
                          {
                            dmgType = _cardActive.DamageType;
                            CRT.damageType = dmgType;
                          }
                          else
                            continue;
                        }
                        else
                        {
                          dmg = _cardActive.DamageSides2;
                          ignoreBlock = _cardActive.GetIgnoreBlock(1);
                          if (dmg > 0)
                            dmgType = _cardActive.DamageType2;
                          else
                            continue;
                        }
                        int num5 = !isHero ? _npc.Position : _hero.Position;
                        int num6;
                        if (i == 3 || i == 5)
                        {
                          if (num5 != 0)
                            num6 = num5 - 1;
                          else
                            continue;
                        }
                        else
                          num6 = num5 + 1;
                        bool flag = false;
                        if (isHero)
                        {
                          for (int index = 0; index < matchManager.TeamHero.Length; ++index)
                          {
                            if (matchManager.TeamHero[index] != null && matchManager.TeamHero[index].Alive && matchManager.TeamHero[index].Position == num6)
                            {
                              _hero = matchManager.TeamHero[index];
                              _heroItem = _hero.HeroItem;
                              flag = true;
                              ctHero[_heroItem] = CRT;
                              break;
                            }
                          }
                        }
                        else
                        {
                          for (int index = 0; index < matchManager.TeamNPC.Length; ++index)
                          {
                            if (matchManager.TeamNPC[index] != null && matchManager.TeamNPC[index].Alive && matchManager.TeamNPC[index].Position == num6)
                            {
                              _npc = matchManager.TeamNPC[index];
                              _npcItem = _npc.NPCItem;
                              flag = true;
                              ctNPC[_npcItem] = CRT;
                              break;
                            }
                          }
                        }
                        if (flag)
                          break;
                        continue;
                      }
                      if (i == 7 || i == 8)
                      {
                        CRT = new CastResolutionForCombatText();
                        if (youCanCastEffect)
                        {
                          if (i == 7)
                          {
                            ignoreBlock = _cardActive.GetIgnoreBlock();
                            dmg = _cardActive.DamageSelf;
                            dmgType = _cardActive.DamageType;
                          }
                          else
                          {
                            ignoreBlock = _cardActive.GetIgnoreBlock(1);
                            dmg = _cardActive.DamageSelf2;
                            dmgType = _cardActive.DamageType2;
                          }
                          if (dmg > 0)
                          {
                            if (theCasterIsHero)
                            {
                              isHero = true;
                              isNPC = false;
                              _hero = theCasterHero;
                              _heroItem = _hero.HeroItem;
                              ctHero[_heroItem] = CRT;
                              break;
                            }
                            isNPC = true;
                            isHero = false;
                            _npc = theCasterNPC;
                            _npcItem = _npc.NPCItem;
                            ctNPC[_npcItem] = CRT;
                            break;
                          }
                          continue;
                        }
                        continue;
                      }
                      break;
                  }
                  if (i == 1 && (_npc != null && _npc.Alive || _hero != null && _hero.Alive))
                  {
                    castedEffect = true;
                    AudioClip soundHit = _cardActive.GetSoundHit();
                    if ((UnityEngine.Object) soundHit != (UnityEngine.Object) null)
                    {
                      float delay = 0.0f;
                      if ((double) _cardActive.SoundHitReworkDelay > 0.0 && !GameManager.Instance.ConfigUseLegacySounds)
                        delay = _cardActive.SoundHitReworkDelay;
                      if (matchManager.theHero != null || _cardActive.MoveToCenter)
                        GameManager.Instance.PlayAudio(soundHit, 0.25f, delay);
                      else
                        GameManager.Instance.PlayAudio(soundHit, 0.75f, delay);
                    }
                    if (_cardActive.EffectPreAction == "" && (double) _cardActive.EffectPostCastDelay > 0.0 && (iteration == 0 || _cardActive.EffectCasterRepeat))
                    {
                      float _time = _cardActive.EffectPostCastDelay;
                      if (GameManager.Instance.IsMultiplayer() || GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Fast || GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Ultrafast)
                        _time = _cardActive.EffectPostCastDelay * 0.6f;
                      if ((double) _time < 0.10000000149011612)
                        _time = 0.1f;
                      yield return (object) Globals.Instance.WaitForSeconds(_time);
                    }
                    if (_cardActive.EffectTrail != "" && (iteration == 0 || _cardActive.EffectTrailRepeat))
                    {
                      Transform from = (Transform) null;
                      Transform to = (Transform) null;
                      int num7 = 0;
                      int num8 = 0;
                      bool flag = true;
                      if (theCasterNPC != null && (UnityEngine.Object) theCasterNPC.NPCItem != (UnityEngine.Object) null)
                      {
                        if (iteration > 0 && (_cardActive.EffectRepeatTarget == Enums.EffectRepeatTarget.Chain || _cardActive.EffectRepeatTarget == Enums.EffectRepeatTarget.NoRepeat))
                        {
                          Hero heroById = matchManager.GetHeroById(lastId);
                          if (heroById != null && (UnityEngine.Object) heroById.HeroItem != (UnityEngine.Object) null)
                          {
                            from = heroById.HeroItem.CharImageT;
                            num7 = heroById.Position;
                          }
                          else
                            flag = false;
                        }
                        else
                        {
                          from = theCasterNPC.NPCItem.CharImageT;
                          num7 = theCasterNPC.Position;
                        }
                      }
                      else if (theCasterHero != null && (UnityEngine.Object) theCasterHero.HeroItem != (UnityEngine.Object) null)
                      {
                        if (iteration > 0 && (_cardActive.EffectRepeatTarget == Enums.EffectRepeatTarget.Chain || _cardActive.EffectRepeatTarget == Enums.EffectRepeatTarget.NoRepeat))
                        {
                          NPC npcById = matchManager.GetNPCById(lastId);
                          if (npcById != null && (UnityEngine.Object) npcById.NPCItem != (UnityEngine.Object) null)
                          {
                            from = npcById.NPCItem.CharImageT;
                            num7 = npcById.Position;
                          }
                          else
                            flag = false;
                        }
                        else
                        {
                          from = theCasterHero.HeroItem.CharImageT;
                          num7 = theCasterHero.Position;
                        }
                      }
                      if (flag)
                      {
                        if (_npc != null && (UnityEngine.Object) _npcItem != (UnityEngine.Object) null)
                        {
                          to = _npcItem.CharImageT;
                          num8 = _npc.Position;
                        }
                        else if (_hero != null && (UnityEngine.Object) _heroItem != (UnityEngine.Object) null)
                        {
                          to = _heroItem.CharImageT;
                          num8 = _hero.Position;
                        }
                        matchManager.waitingTrail = true;
                        EffectsManager.Instance.PlayEffectTrail(_cardActive, theCasterIsHero, from, to, num7 + num8 + 1);
                        while (matchManager.waitingTrail)
                          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                      }
                    }
                    if (_cardActive.EffectTarget != "")
                    {
                      if (_npc != null && (UnityEngine.Object) _npcItem != (UnityEngine.Object) null && (UnityEngine.Object) _cardActive.SummonUnit == (UnityEngine.Object) null)
                        EffectsManager.Instance.PlayEffect(_cardActive, false, theCasterIsHero, _npcItem.CharImageT);
                      else if (_hero != null && (UnityEngine.Object) _heroItem != (UnityEngine.Object) null)
                        EffectsManager.Instance.PlayEffect(_cardActive, false, theCasterIsHero, _heroItem.CharImageT);
                    }
                    if ((double) _cardActive.EffectPostTargetDelay > 0.0)
                    {
                      if (GameManager.Instance.IsMultiplayer())
                        yield return (object) Globals.Instance.WaitForSeconds(_cardActive.EffectPostTargetDelay * 0.3f);
                      else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
                        yield return (object) Globals.Instance.WaitForSeconds(_cardActive.EffectPostTargetDelay);
                      else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Ultrafast)
                        yield return (object) null;
                      else
                        yield return (object) Globals.Instance.WaitForSeconds(_cardActive.EffectPostTargetDelay * 0.2f);
                    }
                  }
                  if (i == 1 || i == 3 || i == 4 || i == 7)
                  {
                    if (_cardActive.DamageSpecialValueGlobal)
                      dmg = cardSpecialValueGlobal;
                    else if (_cardActive.DamageSpecialValue1)
                      dmg = exahustGenerating;
                    else if (_cardActive.DamageSpecialValue2)
                      dmg = cardSpecialValue2;
                  }
                  if (i == 1 && dmgType == _cardActive.DamageType2)
                  {
                    bool flag = false;
                    if (_cardActive.Damage2SpecialValueGlobal)
                    {
                      dmg += cardSpecialValueGlobal;
                      flag = true;
                    }
                    else if (_cardActive.Damage2SpecialValue1)
                    {
                      dmg += exahustGenerating;
                      flag = true;
                    }
                    else if (_cardActive.Damage2SpecialValue2)
                    {
                      dmg += cardSpecialValue2;
                      flag = true;
                    }
                    if (!flag)
                      dmg += _cardActive.Damage2;
                  }
                  if (i == 2 || i == 5 || i == 6 || i == 8)
                  {
                    if (_cardActive.Damage2SpecialValueGlobal)
                      dmg = cardSpecialValueGlobal;
                    else if (_cardActive.Damage2SpecialValue1)
                      dmg = exahustGenerating;
                    else if (_cardActive.Damage2SpecialValue2)
                      dmg = cardSpecialValue2;
                  }
                  int num9 = 0;
                  dmgTotal = 0;
                  if (theCasterHero != null)
                    num9 = theCasterHero.DamageWithCharacterBonus(dmg, dmgType, _cardActive.CardClass, matchManager.energyJustWastedByHero);
                  else if (theCasterNPC != null)
                    num9 = theCasterNPC.DamageWithCharacterBonus(dmg, dmgType, _cardActive.CardClass, matchManager.energyJustWastedByHero);
                  if ((double) effectRepeatPercent != 100.0)
                    num9 = Functions.FuncRoundToInt((float) ((double) num9 * (double) effectRepeatPercent * 0.0099999997764825821));
                  blocked = 0;
                  if (Globals.Instance.ShowDebug)
                    Functions.DebugLogGD("directAttackIteration->" + directAttackIteration.ToString(), "general");
                  int num10;
                  if (isNPC)
                  {
                    if (_npc.IsInvulnerable())
                    {
                      CRT.invulnerable = true;
                    }
                    else
                    {
                      int num11 = _npc.IncreasedCursedDamagePerStack(dmgType);
                      int _dmg = num9 + num11;
                      if (_dmg <= 0)
                      {
                        _dmg = 0;
                        CRT.mitigated = true;
                      }
                      int num12 = _npc.EffectCharges("evasion");
                      bool flag = _npc.GetAuraCharges("block") > 0;
                      if ((i == 1 || i == 3 || i == 4 || i == 7) && num12 > 0)
                      {
                        _npc.ConsumeEffectCharges("evasion", 1);
                        num10 = 0;
                        CRT.evaded = true;
                        switch (i)
                        {
                          case 1:
                            fullEvadedDmg = true;
                            break;
                          case 3:
                            fullEvadedDmgSide1 = true;
                            break;
                          case 4:
                            fullEvadedDmgSide2 = true;
                            break;
                        }
                        _npc.SetEvent(Enums.EventActivation.Evade, theCasterCharacter);
                        theCasterCharacter.SetEvent(Enums.EventActivation.Evaded, (Character) _npc);
                      }
                      else
                      {
                        if (i == 1 || i == 3 || i == 4)
                        {
                          _npc.SetEvent(Enums.EventActivation.Hitted, theCasterCharacter);
                          if (directAttackIteration == 0)
                            theCasterCharacter.SetEvent(Enums.EventActivation.Hit);
                          if (_npc.HasEffect("sanctify") && theCasterHero != null && AtOManager.Instance.CharacterHavePerk(theCasterHero.SubclassName, "mainperksanctify2c"))
                            _npc.IndirectDamage(Enums.DamageType.Holy, Functions.FuncRoundToInt((float) _npc.GetAuraCharges("sanctify") * 0.25f));
                          if (_npc.HasEffect("sanctify") && AtOManager.Instance.IsChallengeTraitActive("holypenance"))
                            _npc.IndirectDamage(Enums.DamageType.Holy, Functions.FuncRoundToInt((float) _npc.GetAuraCharges("sanctify") * 0.25f));
                        }
                        if (!ignoreBlock)
                        {
                          blocked = _dmg;
                          _dmg = _npc.ModifyBlock(_dmg);
                          blocked -= _dmg;
                        }
                        CRT.blocked = blocked;
                        if (_npc.GetAuraCharges("block") == 0 && blocked > 0)
                          _npc.SetEvent(Enums.EventActivation.BlockReachedZero);
                        if (_dmg == 0 & flag)
                        {
                          if (blocked > 0)
                          {
                            CRT.fullblocked = true;
                            if (directAttackIteration == 0)
                            {
                              _npc.SetEvent(Enums.EventActivation.Block, theCasterCharacter, blocked);
                              theCasterCharacter.SetEvent(Enums.EventActivation.Blocked, (Character) _npc, blocked);
                            }
                          }
                        }
                        else
                        {
                          CRT.fullblocked = false;
                          if (!_npc.IsImmune(dmgType))
                          {
                            int num13 = -1 * _npc.BonusResists(dmgType);
                            dmgTotal = Functions.FuncRoundToInt((float) _dmg + (float) ((double) _dmg * (double) num13 * 0.0099999997764825821));
                            dmgTotal = matchManager.IncreaseDamage((Character) _npc, dmgTotal);
                            if (i == 2 || i == 8)
                            {
                              CRT.damage2 = dmgTotal;
                              CRT.damageType2 = dmgType;
                            }
                            else
                            {
                              CRT.damage = dmgTotal;
                              CRT.damageType = dmgType;
                            }
                          }
                          else
                            CRT.immune = true;
                        }
                      }
                    }
                  }
                  else if (_hero.IsInvulnerable())
                  {
                    CRT.invulnerable = true;
                  }
                  else
                  {
                    int num14 = _hero.IncreasedCursedDamagePerStack(dmgType);
                    int _dmg = num9 + num14;
                    if (_dmg <= 0)
                    {
                      _dmg = 0;
                      CRT.mitigated = true;
                    }
                    int num15 = _hero.EffectCharges("evasion");
                    bool flag = _hero.GetAuraCharges("block") > 0;
                    if (_hero.GetAuraCharges("block") == 0 && blocked > 0)
                      _hero.SetEvent(Enums.EventActivation.BlockReachedZero);
                    if ((i == 1 || i == 3 || i == 4 || i == 7) && num15 > 0)
                    {
                      _hero.ConsumeEffectCharges("evasion", 1);
                      num10 = 0;
                      CRT.evaded = true;
                      switch (i)
                      {
                        case 1:
                          fullEvadedDmg = true;
                          break;
                        case 3:
                          fullEvadedDmgSide1 = true;
                          break;
                        case 4:
                          fullEvadedDmgSide2 = true;
                          break;
                      }
                      _hero.SetEvent(Enums.EventActivation.Evade, theCasterCharacter);
                      theCasterCharacter.SetEvent(Enums.EventActivation.Evaded, (Character) _hero);
                    }
                    else
                    {
                      if (i == 1 || i == 3 || i == 4)
                      {
                        _hero.SetEvent(Enums.EventActivation.Hitted, theCasterCharacter);
                        if (directAttackIteration == 0)
                          theCasterCharacter.SetEvent(Enums.EventActivation.Hit);
                        if (_hero.HasEffect("sanctify") && AtOManager.Instance.IsChallengeTraitActive("holypenance"))
                          _hero.IndirectDamage(Enums.DamageType.Holy, Functions.FuncRoundToInt((float) _hero.GetAuraCharges("sanctify") * 0.25f));
                      }
                      if (!ignoreBlock)
                      {
                        blocked = _dmg;
                        _dmg = _hero.ModifyBlock(_dmg);
                        blocked -= _dmg;
                      }
                      CRT.blocked = blocked;
                      if (_dmg == 0 & flag)
                      {
                        if (blocked > 0)
                        {
                          CRT.fullblocked = true;
                          if (directAttackIteration == 0)
                          {
                            _hero.SetEvent(Enums.EventActivation.Block, theCasterCharacter, blocked);
                            theCasterCharacter.SetEvent(Enums.EventActivation.Blocked, (Character) _hero, blocked);
                          }
                        }
                      }
                      else if (!_hero.IsImmune(dmgType))
                      {
                        int num16 = -1 * _hero.BonusResists(dmgType);
                        dmgTotal = Functions.FuncRoundToInt((float) _dmg + (float) ((double) _dmg * (double) num16 * 0.0099999997764825821));
                        dmgTotal = matchManager.IncreaseDamage((Character) _hero, dmgTotal);
                        if (i == 2 || i == 8)
                        {
                          CRT.damage2 = dmgTotal;
                          CRT.damageType2 = dmgType;
                        }
                        else
                        {
                          CRT.damage = dmgTotal;
                          CRT.damageType = dmgType;
                        }
                      }
                      else
                        CRT.immune = true;
                    }
                  }
                  if (dmgTotal < 0)
                    dmgTotal = 0;
                  Dictionary<Enums.DamageType, int> dictionary = new Dictionary<Enums.DamageType, int>();
                  if (isNPC)
                  {
                    matchManager.castCardDamageDone = dmgTotal <= _npc.GetHp() ? (float) dmgTotal : (float) _npc.GetHp();
                    matchManager.castCardDamageDoneTotal += matchManager.castCardDamageDone;
                    matchManager.castCardDamageDoneIteration += (float) dmgTotal;
                    _npc.ModifyHp(-dmgTotal);
                    if (dmgTotal > 0 && (i == 1 || i == 2 || i == 3 || i == 4))
                    {
                      if (i != 2 || i == 2 && !damagedEventFired)
                        theCasterCharacter.SetEvent(Enums.EventActivation.Damage, (Character) _npc, dmgTotal);
                      if (!damagedEventFired)
                      {
                        _npc.SetEvent(Enums.EventActivation.Damaged, theCasterCharacter, dmgTotal);
                        damagedEventFired = true;
                      }
                      else if (i == 2)
                        _npc.SetEvent(Enums.EventActivation.DamagedSecondary, theCasterCharacter, dmgTotal);
                    }
                    if (dmgTotal == 0 && blocked > 0 && (i == 3 || i == 4 || i == 5 || i == 6) && !blockedEventFired)
                    {
                      _npc.SetEvent(Enums.EventActivation.Block, theCasterCharacter, blocked);
                      blockedEventFired = true;
                    }
                    if (!fullEvadedDmg && i == 1 || !fullEvadedDmgSide1 && i == 2 || !fullEvadedDmgSide1 && i == 3 || !fullEvadedDmgSide2 && i == 4)
                    {
                      _npc.DamageReflected(theCasterHero, theCasterNPC, dmgTotal, blocked);
                      yield return (object) Globals.Instance.WaitForSeconds(0.05f);
                      _npc.HealAttacker(theCasterHero, theCasterNPC);
                    }
                  }
                  else
                  {
                    matchManager.castCardDamageDone = dmgTotal <= _hero.GetHp() ? (float) dmgTotal : (float) _hero.GetHp();
                    matchManager.castCardDamageDoneTotal += matchManager.castCardDamageDone;
                    matchManager.castCardDamageDoneIteration += (float) dmgTotal;
                    _hero.ModifyHp(-dmgTotal);
                    if (dmgTotal > 0 && (i == 1 || i == 2 || i == 3 || i == 4))
                    {
                      if (i != 2 || i == 2 && !damagedEventFired)
                        theCasterCharacter.SetEvent(Enums.EventActivation.Damage, (Character) _hero, dmgTotal);
                      if (!damagedEventFired)
                      {
                        _hero.SetEvent(Enums.EventActivation.Damaged, theCasterCharacter, dmgTotal);
                        damagedEventFired = true;
                      }
                      else if (i == 2)
                        _hero.SetEvent(Enums.EventActivation.DamagedSecondary, theCasterCharacter, dmgTotal);
                    }
                    if (dmgTotal == 0 && blocked > 0 && (i == 3 || i == 4 || i == 5 || i == 6) && !blockedEventFired)
                    {
                      _hero.SetEvent(Enums.EventActivation.Block, theCasterCharacter, blocked);
                      blockedEventFired = true;
                    }
                    if (!fullEvadedDmg && i == 1 || !fullEvadedDmgSide1 && i == 2 || !fullEvadedDmgSide1 && i == 3 || !fullEvadedDmgSide2 && i == 4)
                    {
                      _hero.DamageReflected(theCasterHero, theCasterNPC, dmgTotal, blocked);
                      yield return (object) Globals.Instance.WaitForSeconds(0.05f);
                      _hero.GrantBlockToTeam(theCasterHero, theCasterNPC, dmgTotal, blocked);
                      _hero.HealAttacker(theCasterHero, theCasterNPC);
                    }
                  }
                  if (theCasterIsHero)
                  {
                    string gameName1 = theCasterHero.GameName;
                  }
                  else
                  {
                    string gameName2 = theCasterNPC.GameName;
                  }
                  if (isNPC)
                  {
                    string gameName3 = _npc.GameName;
                  }
                  else
                  {
                    string gameName4 = _hero.GameName;
                  }
                  _hero = _heroBeforeDmg;
                  _heroItem = _heroItemBeforeDmg;
                  _npc = _npcBeforeDmg;
                  _npcItem = _npcItemBeforeDmg;
                  isHero = _isHero;
                  isNPC = _isNPC;
                  ++directAttackIteration;
                }
              }
              foreach (KeyValuePair<NPCItem, CastResolutionForCombatText> keyValuePair in ctNPC)
                keyValuePair.Key.ScrollCombatTextDamageNew(keyValuePair.Value);
              foreach (KeyValuePair<HeroItem, CastResolutionForCombatText> keyValuePair in ctHero)
                keyValuePair.Key.ScrollCombatTextDamageNew(keyValuePair.Value);
              yield return (object) Globals.Instance.WaitForSeconds(0.05f);
              _heroBeforeDmg = (Hero) null;
              _heroItemBeforeDmg = (HeroItem) null;
              _npcBeforeDmg = (NPC) null;
              _npcItemBeforeDmg = (NPCItem) null;
              ctHero = (Dictionary<HeroItem, CastResolutionForCombatText>) null;
              ctNPC = (Dictionary<NPCItem, CastResolutionForCombatText>) null;
              CRT = (CastResolutionForCombatText) null;
            }
            if ((double) matchManager.castCardDamageDoneIteration > 0.0)
            {
              if ((double) matchManager.castCardDamageDoneIteration > 49.0)
                PlayerManager.Instance.AchievementUnlock("MISC_GIANT");
              if ((double) matchManager.castCardDamageDoneIteration > 149.0)
                PlayerManager.Instance.AchievementUnlock("MISC_COLOSSUS");
              if ((double) matchManager.castCardDamageDoneIteration > 399.0)
                PlayerManager.Instance.AchievementUnlock("MISC_BEHEMOTH");
            }
            if (isYourFirstTarget && (UnityEngine.Object) _cardActive.HealAuraCurseSelf != (UnityEngine.Object) null && _cardActive.HealAuraCurseSelf.Id == "spellsword")
              theCasterCharacter.HealAuraCurse(_cardActive.HealAuraCurseSelf);
            if (!castedEffect && (_npc != null && _npc.Alive || _hero != null && _hero.Alive))
            {
              AudioClip soundHit = _cardActive.GetSoundHit();
              if ((UnityEngine.Object) soundHit != (UnityEngine.Object) null)
              {
                float delay = 0.0f;
                if ((double) _cardActive.SoundHitReworkDelay > 0.0 && !GameManager.Instance.ConfigUseLegacySounds)
                  delay = _cardActive.SoundHitReworkDelay;
                GameManager.Instance.PlayAudio(soundHit, 0.25f, delay);
              }
              if (_cardActive.EffectPreAction == "" && (double) _cardActive.EffectPostCastDelay > 0.0 && (iteration == 0 || _cardActive.EffectCasterRepeat))
                yield return (object) Globals.Instance.WaitForSeconds(_cardActive.EffectPostCastDelay);
              if (_cardActive.EffectTrail != "")
              {
                Transform from = (Transform) null;
                Transform to = (Transform) null;
                int num17 = 0;
                int num18 = 0;
                if (theCasterNPC != null && (UnityEngine.Object) theCasterNPC.NPCItem != (UnityEngine.Object) null)
                {
                  from = theCasterNPC.NPCItem.CharImageT;
                  num17 = theCasterNPC.Position;
                }
                else if (theCasterHero != null && (UnityEngine.Object) theCasterHero.HeroItem != (UnityEngine.Object) null)
                {
                  from = theCasterHero.HeroItem.CharImageT;
                  num17 = theCasterHero.Position;
                }
                if (_npc != null && (UnityEngine.Object) _npcItem != (UnityEngine.Object) null)
                {
                  to = _npcItem.CharImageT;
                  num18 = _npc.Position;
                }
                else if (_hero != null && (UnityEngine.Object) _heroItem != (UnityEngine.Object) null)
                {
                  to = _heroItem.CharImageT;
                  num18 = _hero.Position;
                }
                matchManager.waitingTrail = true;
                EffectsManager.Instance.PlayEffectTrail(_cardActive, theCasterIsHero, from, to, num17 + num18 + 1);
                while (matchManager.waitingTrail)
                  yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              }
              if (_cardActive.EffectTarget != "")
              {
                if (_npc != null && (UnityEngine.Object) _npcItem != (UnityEngine.Object) null && (UnityEngine.Object) _cardActive.SummonUnit == (UnityEngine.Object) null)
                  EffectsManager.Instance.PlayEffect(_cardActive, false, theCasterIsHero, _npcItem.CharImageT);
                else if (_hero != null && (UnityEngine.Object) _heroItem != (UnityEngine.Object) null)
                  EffectsManager.Instance.PlayEffect(_cardActive, false, theCasterIsHero, _heroItem.CharImageT);
              }
              if ((double) _cardActive.EffectPostTargetDelay > 0.0)
                yield return (object) Globals.Instance.WaitForSeconds(_cardActive.EffectPostTargetDelay);
            }
            if (_cardActive.SelfHealthLoss > 0)
            {
              int damage = _cardActive.SelfHealthLoss;
              if (_cardActive.SelfHealthLossSpecialGlobal)
                damage = cardSpecialValueGlobal;
              else if (_cardActive.SelfHealthLossSpecialValue1)
                damage = exahustGenerating;
              else if (_cardActive.SelfHealthLossSpecialValue2)
                damage = cardSpecialValue2;
              if (theCasterHero != null && theCasterHero.Alive)
              {
                theCasterHero.IndirectDamage(Enums.DamageType.None, damage, sourceCharacterName: theCasterName, sourceCharacterId: theCasterId);
                yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              }
              else if (theCasterNPC != null && theCasterNPC.Alive)
              {
                theCasterNPC.IndirectDamage(Enums.DamageType.None, damage, sourceCharacterName: theCasterName, sourceCharacterId: theCasterId);
                yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              }
            }
            yield return (object) Globals.Instance.WaitForSeconds(0.01f);
            if (theCasterHero != null && theCasterHero.Alive || theCasterNPC != null && theCasterNPC.Alive)
            {
              AuraCurseData auraCurseData1 = (AuraCurseData) null;
              directAttackIteration = 0;
              Character targetCharacter;
              if ((UnityEngine.Object) _cardActive.Aura != (UnityEngine.Object) null || (UnityEngine.Object) _cardActive.Aura2 != (UnityEngine.Object) null || (UnityEngine.Object) _cardActive.Aura3 != (UnityEngine.Object) null || (UnityEngine.Object) _cardActive.AcEnergyBonus != (UnityEngine.Object) null && _cardActive.AcEnergyBonus.IsAura || _cardActive.Auras != null && _cardActive.Auras.Length != 0)
              {
                targetCharacter = theCasterHero != null ? (Character) _npc : (Character) _hero;
                ignoreBlock = false;
                fullEvadedDmgSide2 = false;
                fullEvadedDmgSide1 = false;
                fullEvadedDmg = false;
                blockedEventFired = _cardActive.Auras != null && _cardActive.Auras.Length != 0;
                dmgTotal = blockedEventFired ? _cardActive.Auras.Length + 5 : 5;
                for (dmg = 0; dmg < dmgTotal; ++dmg)
                {
                  damagedEventFired = false;
                  AuraCurseData auraCurseData2 = (AuraCurseData) null;
                  if (dmg == 0 && !_cardActive.ChooseOneOfAvailableAuras && (UnityEngine.Object) _cardActive.Aura != (UnityEngine.Object) null)
                  {
                    auraCurseData2 = _cardActive.Aura;
                    directAttackIteration = matchManager.ResolveAuraCurseCharges(auraCurseData2, _cardActive.AuraCharges, _cardActive.AuraChargesSpecialValueGlobal, _cardActive.AuraChargesSpecialValue1, _cardActive.AuraChargesSpecialValue2, targetCharacter, cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2);
                    if (_cardActive.AuraCharges2 > 0 && (UnityEngine.Object) _cardActive.Aura == (UnityEngine.Object) _cardActive.Aura2)
                    {
                      directAttackIteration += matchManager.ResolveAuraCurseCharges(auraCurseData2, 0, _cardActive.AuraCharges2SpecialValueGlobal, _cardActive.AuraCharges2SpecialValue1, _cardActive.AuraCharges2SpecialValue2, targetCharacter, cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2, new int?(_cardActive.AuraCharges2));
                      fullEvadedDmgSide1 = true;
                    }
                  }
                  else if (dmg == 1 && !_cardActive.ChooseOneOfAvailableAuras && (UnityEngine.Object) _cardActive.Aura2 != (UnityEngine.Object) null)
                  {
                    if (!fullEvadedDmgSide1)
                    {
                      auraCurseData2 = _cardActive.Aura2;
                      directAttackIteration = matchManager.ResolveAuraCurseCharges(auraCurseData2, _cardActive.AuraCharges2, _cardActive.AuraCharges2SpecialValueGlobal, _cardActive.AuraCharges2SpecialValue1, _cardActive.AuraCharges2SpecialValue2, targetCharacter, cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2);
                      if (_cardActive.AuraCharges3 > 0 && (UnityEngine.Object) _cardActive.Aura2 == (UnityEngine.Object) _cardActive.Aura3)
                      {
                        directAttackIteration += matchManager.ResolveAuraCurseCharges(auraCurseData2, 0, _cardActive.AuraCharges3SpecialValueGlobal, _cardActive.AuraCharges3SpecialValue1, _cardActive.AuraCharges3SpecialValue2, targetCharacter, cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2, new int?(_cardActive.AuraCharges3));
                        fullEvadedDmg = true;
                      }
                    }
                    else
                      continue;
                  }
                  else if (dmg == 2 && !_cardActive.ChooseOneOfAvailableAuras && (UnityEngine.Object) _cardActive.Aura3 != (UnityEngine.Object) null)
                  {
                    if (!fullEvadedDmg)
                    {
                      auraCurseData2 = _cardActive.Aura3;
                      directAttackIteration = matchManager.ResolveAuraCurseCharges(auraCurseData2, _cardActive.AuraCharges3, _cardActive.AuraCharges3SpecialValueGlobal, _cardActive.AuraCharges3SpecialValue1, _cardActive.AuraCharges3SpecialValue2, targetCharacter, cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2);
                    }
                    else
                      continue;
                  }
                  else if (blockedEventFired && dmg >= 3 && dmg <= dmgTotal - 3)
                  {
                    CardData.AuraBuffs aura = _cardActive.Auras[dmg - 3];
                    if (aura != null && (UnityEngine.Object) aura.aura != (UnityEngine.Object) null)
                    {
                      auraCurseData2 = aura.aura;
                      directAttackIteration = matchManager.ResolveAuraCurseCharges(auraCurseData2, aura.auraCharges, aura.auraChargesSpecialValueGlobal, aura.auraChargesSpecialValue1, aura.auraChargesSpecialValue2, targetCharacter, cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2);
                    }
                  }
                  else if (_cardActive.ChooseOneOfAvailableAuras)
                  {
                    damagedEventFired = true;
                    List<AuraCurseData> auraCurseDataList = new List<AuraCurseData>();
                    List<int> intList = new List<int>();
                    if ((bool) (UnityEngine.Object) _cardActive.Aura)
                    {
                      auraCurseDataList.Add(_cardActive.Aura);
                      intList.Add(_cardActive.AuraCharges);
                    }
                    if ((bool) (UnityEngine.Object) _cardActive.Aura2)
                    {
                      auraCurseDataList.Add(_cardActive.Aura2);
                      intList.Add(_cardActive.AuraCharges2);
                    }
                    if ((bool) (UnityEngine.Object) _cardActive.Aura3)
                    {
                      auraCurseDataList.Add(_cardActive.Aura3);
                      intList.Add(_cardActive.AuraCharges3);
                    }
                    if (_cardActive.Auras != null && _cardActive.Auras.Length != 0)
                    {
                      auraCurseDataList.AddRange(((IEnumerable<CardData.AuraBuffs>) _cardActive.Auras).Where<CardData.AuraBuffs>((Func<CardData.AuraBuffs, bool>) (x => (UnityEngine.Object) x.aura != (UnityEngine.Object) null)).Select<CardData.AuraBuffs, AuraCurseData>((Func<CardData.AuraBuffs, AuraCurseData>) (x => x.aura)));
                      intList.AddRange(((IEnumerable<CardData.AuraBuffs>) _cardActive.Auras).Where<CardData.AuraBuffs>((Func<CardData.AuraBuffs, bool>) (x => (UnityEngine.Object) x.aura != (UnityEngine.Object) null)).Select<CardData.AuraBuffs, int>((Func<CardData.AuraBuffs, int>) (x => x.auraCharges)));
                    }
                    int randomIntRange = matchManager.GetRandomIntRange(0, auraCurseDataList.Count, "misc");
                    auraCurseData2 = auraCurseDataList[randomIntRange];
                    directAttackIteration = intList[randomIntRange];
                  }
                  if (matchManager.energyAssigned > 0 && (UnityEngine.Object) _cardActive.AcEnergyBonus != (UnityEngine.Object) null && _cardActive.AcEnergyBonus.IsAura)
                  {
                    if (dmg == dmgTotal - 2 && !ignoreBlock)
                    {
                      auraCurseData2 = _cardActive.AcEnergyBonus;
                      directAttackIteration = _cardActive.AcEnergyBonusQuantity * matchManager.energyAssigned;
                      ignoreBlock = true;
                    }
                    else if (!ignoreBlock && (UnityEngine.Object) auraCurseData2 != (UnityEngine.Object) null && auraCurseData2.Id == _cardActive.AcEnergyBonus.Id)
                    {
                      directAttackIteration += _cardActive.AcEnergyBonusQuantity * matchManager.energyAssigned;
                      ignoreBlock = true;
                    }
                  }
                  if (matchManager.energyAssigned > 0 && (UnityEngine.Object) _cardActive.AcEnergyBonus2 != (UnityEngine.Object) null && _cardActive.AcEnergyBonus2.IsAura)
                  {
                    if (dmg == dmgTotal - 1 && !fullEvadedDmgSide2)
                    {
                      auraCurseData2 = _cardActive.AcEnergyBonus2;
                      directAttackIteration = _cardActive.AcEnergyBonus2Quantity * matchManager.energyAssigned;
                      fullEvadedDmgSide2 = true;
                    }
                    else if (!fullEvadedDmgSide2 && (UnityEngine.Object) auraCurseData2 != (UnityEngine.Object) null && auraCurseData2.Id == _cardActive.AcEnergyBonus2.Id)
                    {
                      directAttackIteration += _cardActive.AcEnergyBonus2Quantity * matchManager.energyAssigned;
                      fullEvadedDmgSide2 = true;
                    }
                  }
                  if ((UnityEngine.Object) auraCurseData2 != (UnityEngine.Object) null)
                  {
                    if (isHero)
                    {
                      if ((UnityEngine.Object) _heroItem != (UnityEngine.Object) null)
                        _hero.SetAura(theCasterCharacter, auraCurseData2, directAttackIteration, CC: _cardActive.CardClass);
                      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                    }
                    else
                    {
                      if ((UnityEngine.Object) _npcItem != (UnityEngine.Object) null)
                        _npc.SetAura(theCasterCharacter, auraCurseData2, directAttackIteration, CC: _cardActive.CardClass);
                      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                    }
                  }
                  yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                  doRedrawInitiatives = true;
                  if (damagedEventFired)
                    break;
                }
                targetCharacter = (Character) null;
              }
              auraCurseData1 = (AuraCurseData) null;
              directAttackIteration = 0;
              if (isYourFirstTarget && ((UnityEngine.Object) _cardActive.AuraSelf != (UnityEngine.Object) null || (UnityEngine.Object) _cardActive.AuraSelf2 != (UnityEngine.Object) null || (UnityEngine.Object) _cardActive.AuraSelf3 != (UnityEngine.Object) null || _cardActive.Auras != null && _cardActive.Auras.Length != 0))
              {
                targetCharacter = theCasterHero != null ? (Character) _npc : (Character) _hero;
                dmgTotal = _cardActive.Auras != null && _cardActive.Auras.Length != 0 ? _cardActive.Auras.Length + 3 : 3;
                for (dmg = 0; dmg < dmgTotal; ++dmg)
                {
                  AuraCurseData auraCurseData3 = (AuraCurseData) null;
                  if (dmg == 0 && (UnityEngine.Object) _cardActive.AuraSelf != (UnityEngine.Object) null)
                  {
                    auraCurseData3 = _cardActive.AuraSelf;
                    directAttackIteration = matchManager.ResolveAuraCurseCharges(auraCurseData3, _cardActive.AuraCharges, _cardActive.AuraChargesSpecialValueGlobal, _cardActive.AuraChargesSpecialValue1, _cardActive.AuraChargesSpecialValue2, targetCharacter, cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2);
                  }
                  else if (dmg == 1 && (UnityEngine.Object) _cardActive.AuraSelf2 != (UnityEngine.Object) null)
                  {
                    auraCurseData3 = _cardActive.AuraSelf2;
                    directAttackIteration = matchManager.ResolveAuraCurseCharges(auraCurseData3, _cardActive.AuraCharges2, _cardActive.AuraCharges2SpecialValueGlobal, _cardActive.AuraCharges2SpecialValue1, _cardActive.AuraCharges2SpecialValue2, targetCharacter, cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2);
                  }
                  else if (dmg == 2 && (UnityEngine.Object) _cardActive.AuraSelf3 != (UnityEngine.Object) null)
                  {
                    auraCurseData3 = _cardActive.AuraSelf3;
                    directAttackIteration = matchManager.ResolveAuraCurseCharges(auraCurseData3, _cardActive.AuraCharges3, _cardActive.AuraCharges3SpecialValueGlobal, _cardActive.AuraCharges3SpecialValue1, _cardActive.AuraCharges3SpecialValue2, targetCharacter, cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2);
                  }
                  else if (dmg > 2)
                  {
                    CardData.AuraBuffs aura = _cardActive.Auras[dmg - 3];
                    if (aura != null && (UnityEngine.Object) aura.auraSelf != (UnityEngine.Object) null)
                    {
                      auraCurseData3 = aura.auraSelf;
                      directAttackIteration = matchManager.ResolveAuraCurseCharges(auraCurseData3, aura.auraCharges, aura.auraChargesSpecialValueGlobal, aura.auraChargesSpecialValue1, aura.auraChargesSpecialValue2, targetCharacter, cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2);
                    }
                  }
                  if ((UnityEngine.Object) auraCurseData3 != (UnityEngine.Object) null)
                  {
                    if (theCasterHero != null && theCasterHero.Alive)
                    {
                      theCasterHero.SetAura(theCasterCharacter, auraCurseData3, directAttackIteration, CC: _cardActive.CardClass);
                      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                    }
                    else if (theCasterNPC != null && theCasterNPC.Alive)
                    {
                      theCasterNPC.SetAura(theCasterCharacter, auraCurseData3, directAttackIteration, CC: _cardActive.CardClass);
                      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                    }
                    if (!doRedrawInitiatives)
                      doRedrawInitiatives = true;
                    yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                  }
                }
                targetCharacter = (Character) null;
              }
              AuraCurseData auraCurseData4 = (AuraCurseData) null;
              directAttackIteration = 0;
              i = 0;
              if ((UnityEngine.Object) _cardActive.Curse != (UnityEngine.Object) null || (UnityEngine.Object) _cardActive.Curse2 != (UnityEngine.Object) null || (UnityEngine.Object) _cardActive.Curse3 != (UnityEngine.Object) null || (UnityEngine.Object) _cardActive.AcEnergyBonus != (UnityEngine.Object) null && !_cardActive.AcEnergyBonus.IsAura || _cardActive.Curses != null && _cardActive.Curses.Length != 0)
              {
                targetCharacter = theCasterHero != null ? (Character) _npc : (Character) _hero;
                blockedEventFired = false;
                fullEvadedDmg = false;
                fullEvadedDmgSide1 = _cardActive.Curses != null && _cardActive.Curses.Length != 0;
                dmgTotal = fullEvadedDmgSide1 ? _cardActive.Curses.Length + 5 : 5;
                for (dmg = 0; dmg < dmgTotal; ++dmg)
                {
                  AuraCurseData auraCurseData5 = (AuraCurseData) null;
                  if (dmg == 0 && (UnityEngine.Object) _cardActive.Curse != (UnityEngine.Object) null)
                  {
                    auraCurseData5 = _cardActive.Curse;
                    directAttackIteration = matchManager.ResolveAuraCurseCharges(auraCurseData5, _cardActive.CurseCharges, _cardActive.CurseChargesSpecialValueGlobal, _cardActive.CurseChargesSpecialValue1, _cardActive.CurseChargesSpecialValue2, targetCharacter, cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2);
                    i = _cardActive.CurseChargesSides;
                  }
                  else if (dmg == 1 && (UnityEngine.Object) _cardActive.Curse2 != (UnityEngine.Object) null)
                  {
                    auraCurseData5 = _cardActive.Curse2;
                    directAttackIteration = matchManager.ResolveAuraCurseCharges(auraCurseData5, _cardActive.CurseCharges2, _cardActive.CurseCharges2SpecialValueGlobal, _cardActive.CurseCharges2SpecialValue1, _cardActive.CurseCharges2SpecialValue2, targetCharacter, cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2);
                  }
                  else if (dmg == 2 && (UnityEngine.Object) _cardActive.Curse3 != (UnityEngine.Object) null)
                  {
                    auraCurseData5 = _cardActive.Curse3;
                    directAttackIteration = matchManager.ResolveAuraCurseCharges(auraCurseData5, _cardActive.CurseCharges3, _cardActive.CurseCharges3SpecialValueGlobal, _cardActive.CurseCharges3SpecialValue1, _cardActive.CurseCharges3SpecialValue2, targetCharacter, cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2);
                  }
                  else if (fullEvadedDmgSide1 && dmg >= 3 && dmg <= dmgTotal - 3)
                  {
                    CardData.CurseDebuffs curse = _cardActive.Curses[dmg - 3];
                    if (curse != null && (UnityEngine.Object) curse.curse != (UnityEngine.Object) null)
                    {
                      auraCurseData5 = curse.curse;
                      directAttackIteration = matchManager.ResolveAuraCurseCharges(auraCurseData5, curse.curseCharges, curse.curseChargesSpecialValueGlobal, curse.curseChargesSpecialValue1, curse.curseChargesSpecialValue2, targetCharacter, cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2);
                      i = curse.curseChargesSides;
                    }
                  }
                  if (matchManager.energyAssigned > 0 && (UnityEngine.Object) _cardActive.AcEnergyBonus != (UnityEngine.Object) null && !_cardActive.AcEnergyBonus.IsAura)
                  {
                    if (dmg == dmgTotal - 2 && !blockedEventFired)
                    {
                      auraCurseData5 = _cardActive.AcEnergyBonus;
                      directAttackIteration = _cardActive.AcEnergyBonusQuantity * matchManager.energyAssigned;
                      blockedEventFired = true;
                    }
                    else if (!blockedEventFired && (UnityEngine.Object) auraCurseData5 != (UnityEngine.Object) null && auraCurseData5.Id == _cardActive.AcEnergyBonus.Id)
                    {
                      directAttackIteration += _cardActive.AcEnergyBonusQuantity * matchManager.energyAssigned;
                      blockedEventFired = true;
                    }
                  }
                  if (matchManager.energyAssigned > 0 && (UnityEngine.Object) _cardActive.AcEnergyBonus2 != (UnityEngine.Object) null && !_cardActive.AcEnergyBonus2.IsAura)
                  {
                    if (dmg == dmgTotal - 1 && !fullEvadedDmg)
                    {
                      auraCurseData5 = _cardActive.AcEnergyBonus2;
                      directAttackIteration = _cardActive.AcEnergyBonus2Quantity * matchManager.energyAssigned;
                      fullEvadedDmg = true;
                    }
                    else if (!fullEvadedDmg && (UnityEngine.Object) auraCurseData5 != (UnityEngine.Object) null && auraCurseData5.Id == _cardActive.AcEnergyBonus2.Id)
                    {
                      directAttackIteration += _cardActive.AcEnergyBonus2Quantity * matchManager.energyAssigned;
                      fullEvadedDmg = true;
                    }
                  }
                  if ((UnityEngine.Object) auraCurseData5 != (UnityEngine.Object) null)
                  {
                    if (isHero)
                    {
                      if (_hero != null && _hero.Alive && (!_hero.IsInvulnerable() || !(auraCurseData5.Id.ToLower() != "doom")))
                      {
                        _hero.SetAura(theCasterCharacter, auraCurseData5, directAttackIteration, CC: _cardActive.CardClass);
                        foreach (Character heroSide in matchManager.GetHeroSides(_hero.Position))
                          heroSide.SetAura(theCasterCharacter, auraCurseData5, i, CC: _cardActive.CardClass);
                        if ((double) matchManager.castCardDamageDoneTotal <= 0.0)
                          _hero.HeroItem.CharacterHitted();
                        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                      }
                    }
                    else if (_npc != null && _npc.Alive && !_npc.IsInvulnerable())
                    {
                      _npc.SetAura(theCasterCharacter, auraCurseData5, directAttackIteration, CC: _cardActive.CardClass);
                      foreach (Character npcSide in matchManager.GetNPCSides(_npc.Position))
                        npcSide.SetAura(theCasterCharacter, auraCurseData5, i, CC: _cardActive.CardClass);
                      if ((double) matchManager.castCardDamageDoneTotal <= 0.0)
                        _npc.NPCItem.CharacterHitted();
                      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                    }
                  }
                  if (!doRedrawInitiatives)
                    doRedrawInitiatives = true;
                  yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                }
                targetCharacter = (Character) null;
              }
              auraCurseData4 = (AuraCurseData) null;
              directAttackIteration = 0;
              if (isYourFirstTarget && ((UnityEngine.Object) _cardActive.CurseSelf != (UnityEngine.Object) null || (UnityEngine.Object) _cardActive.CurseSelf2 != (UnityEngine.Object) null || (UnityEngine.Object) _cardActive.CurseSelf3 != (UnityEngine.Object) null || _cardActive.Curses != null && _cardActive.Curses.Length != 0))
              {
                targetCharacter = theCasterHero != null ? (Character) _npc : (Character) _hero;
                fullEvadedDmgSide1 = _cardActive.Curses != null && _cardActive.Curses.Length != 0;
                dmgTotal = fullEvadedDmgSide1 ? _cardActive.Curses.Length + 3 : 3;
                for (dmg = 0; dmg < dmgTotal; ++dmg)
                {
                  AuraCurseData auraCurseData6 = (AuraCurseData) null;
                  if (dmg == 0 && (UnityEngine.Object) _cardActive.CurseSelf != (UnityEngine.Object) null)
                  {
                    auraCurseData6 = _cardActive.CurseSelf;
                    directAttackIteration = matchManager.ResolveAuraCurseCharges(auraCurseData6, _cardActive.CurseCharges, _cardActive.CurseChargesSpecialValueGlobal, _cardActive.CurseChargesSpecialValue1, _cardActive.CurseChargesSpecialValue2, targetCharacter, cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2);
                  }
                  else if (dmg == 1 && (UnityEngine.Object) _cardActive.CurseSelf2 != (UnityEngine.Object) null)
                  {
                    auraCurseData6 = _cardActive.CurseSelf2;
                    directAttackIteration = matchManager.ResolveAuraCurseCharges(auraCurseData6, _cardActive.CurseCharges2, _cardActive.CurseCharges2SpecialValueGlobal, _cardActive.CurseCharges2SpecialValue1, _cardActive.CurseCharges2SpecialValue2, targetCharacter, cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2);
                  }
                  else if (dmg == 2 && (UnityEngine.Object) _cardActive.CurseSelf3 != (UnityEngine.Object) null)
                  {
                    auraCurseData6 = _cardActive.CurseSelf3;
                    directAttackIteration = matchManager.ResolveAuraCurseCharges(auraCurseData6, _cardActive.CurseCharges3, _cardActive.CurseCharges3SpecialValueGlobal, _cardActive.CurseCharges3SpecialValue1, _cardActive.CurseCharges3SpecialValue2, targetCharacter, cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2);
                  }
                  else if (fullEvadedDmgSide1 && dmg >= 3 && dmg <= dmgTotal - 3)
                  {
                    CardData.CurseDebuffs curse = _cardActive.Curses[dmg - 3];
                    if (curse != null && (UnityEngine.Object) curse.curseSelf != (UnityEngine.Object) null)
                    {
                      auraCurseData6 = curse.curseSelf;
                      directAttackIteration = matchManager.ResolveAuraCurseCharges(auraCurseData6, curse.curseCharges, curse.curseChargesSpecialValueGlobal, curse.curseChargesSpecialValue1, curse.curseChargesSpecialValue2, targetCharacter, cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2);
                    }
                  }
                  if ((UnityEngine.Object) auraCurseData6 != (UnityEngine.Object) null)
                  {
                    if (matchManager.theHero != null)
                    {
                      if (!matchManager.theHero.IsInvulnerable())
                      {
                        matchManager.theHero.SetAura(theCasterCharacter, auraCurseData6, directAttackIteration, CC: _cardActive.CardClass);
                        if (theCasterCharacter != matchManager.theHero)
                          theCasterCharacter.SetEvent(Enums.EventActivation.AuraCurseSet, (Character) matchManager.theHero, directAttackIteration, auraCurseData6.ACName);
                        if ((double) matchManager.castCardDamageDoneTotal <= 0.0)
                          matchManager.theHero.HeroItem.CharacterHitted();
                        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                      }
                    }
                    else if (!matchManager.theNPC.IsInvulnerable())
                    {
                      matchManager.theNPC.SetAura(theCasterCharacter, auraCurseData6, directAttackIteration, CC: _cardActive.CardClass);
                      if (theCasterCharacter != matchManager.theNPC)
                        theCasterCharacter.SetEvent(Enums.EventActivation.AuraCurseSet, (Character) matchManager.theNPC, directAttackIteration, auraCurseData6.ACName);
                      if ((double) matchManager.castCardDamageDoneTotal <= 0.0)
                        matchManager.theNPC.NPCItem.CharacterHitted();
                      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                    }
                  }
                  if (!doRedrawInitiatives)
                    doRedrawInitiatives = true;
                  yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                }
                targetCharacter = (Character) null;
              }
              blocked = 0;
              if (_cardActive.HealEnergyBonus > 0 && matchManager.energyAssigned > 0)
                blocked += _cardActive.HealEnergyBonus * matchManager.energyAssigned;
              if (_cardActive.Heal > 0)
              {
                CastResolutionForCombatText _cast = new CastResolutionForCombatText();
                int heal = _cardActive.Heal + blocked;
                if (_cardActive.HealSpecialValueGlobal)
                  heal += cardSpecialValueGlobal;
                else if (_cardActive.HealSpecialValue1)
                  heal += exahustGenerating;
                else if (_cardActive.HealSpecialValue2)
                  heal += cardSpecialValue2;
                if (theCasterHero != null)
                  heal = theCasterHero.HealWithCharacterBonus(heal, _cardActive.CardClass);
                else if (theCasterNPC != null)
                  heal = theCasterNPC.HealWithCharacterBonus(heal, _cardActive.CardClass);
                if ((double) effectRepeatPercent != 100.0)
                  heal = Functions.FuncRoundToInt((float) ((double) heal * (double) effectRepeatPercent * 0.0099999997764825821));
                if (_hero != null && _hero.Alive)
                {
                  int _hp = _hero.HealReceivedFinal(heal);
                  if (_hero.GetHpLeftForMax() < _hp)
                    _hp = _hero.GetHpLeftForMax();
                  if (_hp > 0)
                  {
                    _hero.SetEvent(Enums.EventActivation.Healed);
                    theCasterCharacter.SetEvent(Enums.EventActivation.Heal, (Character) _hero);
                    _hero.ModifyHp(_hp);
                    _cast.heal = _hp;
                    if ((UnityEngine.Object) _heroItem != (UnityEngine.Object) null)
                      _heroItem.ScrollCombatTextDamageNew(_cast);
                  }
                }
                else if (_npc != null && _npc.Alive)
                {
                  int _hp = _npc.HealReceivedFinal(heal);
                  if (_npc.GetHpLeftForMax() < _hp)
                    _hp = _npc.GetHpLeftForMax();
                  if (_hp > 0)
                  {
                    _npc.SetEvent(Enums.EventActivation.Healed);
                    theCasterCharacter.SetEvent(Enums.EventActivation.Heal, (Character) _npc);
                    _npc.ModifyHp(_hp);
                    _cast.heal = _hp;
                    if ((UnityEngine.Object) _npcItem != (UnityEngine.Object) null)
                      _npcItem.ScrollCombatTextDamageNew(_cast);
                  }
                }
                yield return (object) null;
              }
              if (_cardActive.HealSelf != 0 || (double) _cardActive.HealSelfPerDamageDonePercent != 0.0)
              {
                yield return (object) Globals.Instance.WaitForSeconds(0.1f);
                CastResolutionForCombatText _cast = new CastResolutionForCombatText();
                int num = 0;
                if (_cardActive.HealSelf > 0)
                  num = _cardActive.HealSelf;
                if ((double) _cardActive.HealSelfPerDamageDonePercent != 0.0)
                  num += Functions.FuncRoundToInt((float) ((double) matchManager.castCardDamageDone * (double) _cardActive.HealSelfPerDamageDonePercent * 0.0099999997764825821));
                if (_cardActive.HealSelfSpecialValueGlobal)
                  num = cardSpecialValueGlobal;
                else if (_cardActive.HealSelfSpecialValue1)
                  num = exahustGenerating;
                else if (_cardActive.HealSelfSpecialValue2)
                  num = cardSpecialValue2;
                if (num > 0)
                {
                  int heal = num + blocked;
                  if (theCasterHero != null)
                  {
                    theCasterHero.SetEvent(Enums.EventActivation.Heal, (Character) theCasterHero);
                    theCasterHero.SetEvent(Enums.EventActivation.Healed);
                    int _hp = theCasterHero.HealReceivedFinal(theCasterHero.HealWithCharacterBonus(heal, _cardActive.CardClass));
                    theCasterHero.ModifyHp(_hp);
                    _cast.heal = _hp;
                    if ((UnityEngine.Object) theCasterHero.HeroItem != (UnityEngine.Object) null)
                      theCasterHero.HeroItem.ScrollCombatTextDamageNew(_cast);
                  }
                  else if (theCasterNPC != null)
                  {
                    theCasterNPC.SetEvent(Enums.EventActivation.Heal);
                    theCasterNPC.SetEvent(Enums.EventActivation.Healed);
                    int _hp = theCasterNPC.HealReceivedFinal(theCasterNPC.HealWithCharacterBonus(heal, _cardActive.CardClass));
                    theCasterNPC.ModifyHp(_hp);
                    _cast.heal = _hp;
                    if ((UnityEngine.Object) theCasterNPC.NPCItem != (UnityEngine.Object) null)
                      theCasterNPC.NPCItem.ScrollCombatTextDamageNew(_cast);
                  }
                }
                yield return (object) null;
              }
              if (_cardActive.HealSides > 0)
              {
                CastResolutionForCombatText _cast = new CastResolutionForCombatText();
                int num = _cardActive.HealSides + blocked;
                if (theCasterHero != null)
                  num = theCasterHero.HealWithCharacterBonus(num, _cardActive.CardClass);
                else if (theCasterNPC != null)
                  num = theCasterNPC.HealWithCharacterBonus(num, _cardActive.CardClass);
                if (_hero != null && _hero.Alive)
                {
                  List<Hero> heroSides = matchManager.GetHeroSides(_hero.Position);
                  for (int index = 0; index < heroSides.Count; ++index)
                  {
                    num = heroSides[index].HealReceivedFinal(num);
                    heroSides[index].ModifyHp(num);
                    _cast.heal = num;
                    heroSides[index].HeroItem.ScrollCombatTextDamageNew(_cast);
                  }
                }
                else if (_npc != null && _npc.Alive)
                {
                  List<NPC> npcSides = matchManager.GetNPCSides(_npc.Position);
                  for (int index = 0; index < npcSides.Count; ++index)
                  {
                    num = npcSides[index].HealReceivedFinal(num);
                    npcSides[index].ModifyHp(num);
                    _cast.heal = num;
                    npcSides[index].NPCItem.ScrollCombatTextDamageNew(_cast);
                  }
                }
                yield return (object) null;
              }
              if (_cardActive.KillPet && _hero != null && _hero.Alive && _hero.Pet != "" && _hero.Pet != "harleyrare" && _hero.Pet != "templelurkerpetrare" && _hero.Pet != "mentalscavengerspetrare")
              {
                matchManager.DestroyedItemInThisTurn(_hero.HeroIndex, _hero.Pet);
                _hero.Pet = "tombstone";
                matchManager.CreatePet(Globals.Instance.GetCardData("tombstone", false), _hero.HeroItem.gameObject, _hero, (NPC) null);
                _hero.RefreshItems(5);
                matchManager.RefreshItems();
              }
              if (_hero != null && _hero.Alive)
              {
                if (_cardActive.EnergyRecharge != 0)
                  _hero.ModifyEnergy(_cardActive.EnergyRecharge, true);
                else if (_cardActive.EnergyRechargeSpecialValueGlobal && cardSpecialValueGlobal != 0)
                  _hero.ModifyEnergy(cardSpecialValueGlobal, true);
              }
            }
            if (_hero != null && !_hero.Alive || theCasterHero != null && !theCasterHero.Alive || _npc != null && !_npc.Alive || theCasterNPC != null && !theCasterNPC.Alive)
            {
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("[COMBAT LOOP] Waiting Hero/Monster Kill");
              yield return (object) Globals.Instance.WaitForSeconds(0.3f);
              while (matchManager.waitingKill)
              {
                if (Globals.Instance.ShowDebug)
                  Functions.DebugLogGD("[COMBAT LOOP] Waiting Hero/Monster Kill");
                yield return (object) Globals.Instance.WaitForSeconds(0.1f);
              }
            }
            if (iteration < repeatCast - 1)
            {
              yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              while (matchManager.waitingKill)
                yield return (object) Globals.Instance.WaitForSeconds(0.05f);
              if (GameManager.Instance.IsMultiplayer() && !_automatic)
              {
                string codeGen = matchManager.GenerateSyncCodeForCheckingAction();
                blocked = 0;
                while (blocked < 3)
                {
                  yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                  string forCheckingAction = matchManager.GenerateSyncCodeForCheckingAction();
                  if (codeGen == forCheckingAction)
                  {
                    ++blocked;
                  }
                  else
                  {
                    codeGen = forCheckingAction;
                    blocked = 0;
                  }
                }
                codeGen = (string) null;
              }
              while (matchManager.waitingKill)
                yield return (object) Globals.Instance.WaitForSeconds(0.05f);
              if (theCasterNPC != null && !theCasterNPC.Alive || theCasterHero != null && !theCasterHero.Alive)
              {
                iteration = 100;
              }
              else
              {
                if (youCanCastEffect && (UnityEngine.Object) targetTransformCast != (UnityEngine.Object) null && _cardActive.EffectPreAction == "")
                {
                  if ((double) _cardActive.EffectRepeatDelay > 0.0)
                  {
                    if (GameManager.Instance.IsMultiplayer())
                      yield return (object) Globals.Instance.WaitForSeconds(_cardActive.EffectRepeatDelay * 0.3f);
                    else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
                      yield return (object) Globals.Instance.WaitForSeconds(_cardActive.EffectRepeatDelay);
                    else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Ultrafast)
                      yield return (object) null;
                    else
                      yield return (object) Globals.Instance.WaitForSeconds(_cardActive.EffectRepeatDelay * 0.2f);
                  }
                  if (_automatic)
                  {
                    if (matchManager.theNPC != null && (UnityEngine.Object) matchManager.theNPC.NPCItem != (UnityEngine.Object) null)
                    {
                      if (_cardActive.MoveToCenter && !matchManager.IsPhantomArmorSpecialCard(_cardActive.Id))
                        matchManager.theNPC.NPCItem.CharacterAttackAnim();
                      else if (_cardActive.EffectCasterRepeat && !_cardActive.IsPetCast && !matchManager.IsPhantomArmorSpecialCard(_cardActive.Id))
                        matchManager.theNPC.NPCItem.CharacterCastAnim();
                    }
                  }
                  else if (matchManager.theHero != null && (UnityEngine.Object) matchManager.theHero.HeroItem != (UnityEngine.Object) null)
                  {
                    if (_cardActive.MoveToCenter)
                      matchManager.theHero.HeroItem.CharacterAttackAnim();
                    else if (_cardActive.EffectCasterRepeat && !_cardActive.IsPetCast)
                      matchManager.theHero.HeroItem.CharacterCastAnim();
                  }
                }
                if (GameManager.Instance.IsMultiplayer())
                  yield return (object) Globals.Instance.WaitForSeconds(0.15f);
                else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
                  yield return (object) Globals.Instance.WaitForSeconds(0.15f);
                else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Ultrafast)
                  yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                else
                  yield return (object) Globals.Instance.WaitForSeconds(0.1f);
              }
            }
            while (matchManager.waitingKill)
              yield return (object) Globals.Instance.WaitForSeconds(0.01f);
          }
          if (matchManager.matchIsOver)
          {
            yield break;
          }
          else
          {
            _indexQueue = 0;
            while (matchManager.ctQueue.Count > 0)
            {
              if (matchManager.combatCounter % 200 == 0 && GameManager.Instance.GetDeveloperMode())
              {
                int count = matchManager.ctQueue.Count;
                string str1 = count.ToString();
                count = matchManager.eventList.Count;
                string str2 = count.ToString();
                Debug.Log((object) ("Queue loop " + str1 + "//" + str2));
              }
              yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              ++_indexQueue;
              if (_indexQueue > 25)
                break;
            }
            lastId = (string) null;
          }
        }
        else
          break;
      }
      if ((double) _cardActive.SelfKillHiddenSeconds > 0.0)
      {
        yield return (object) Globals.Instance.WaitForSeconds(_cardActive.SelfKillHiddenSeconds);
        if (matchManager.theNPC != null && (UnityEngine.Object) matchManager.theNPC.NPCItem != (UnityEngine.Object) null)
          matchManager.theNPC.KillCharacterFromOutside();
        else if (matchManager.theHero != null && (UnityEngine.Object) matchManager.theHero.HeroItem != (UnityEngine.Object) null)
          matchManager.theHero.KillCharacterFromOutside();
        yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      }
      if (repeatCast == 0)
      {
        while (matchManager.waitingKill)
        {
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[COMBAT LOOP] Waiting Hero/Monster Kill at end  of cast for NO REPEAT CARDS");
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        }
      }
      if (matchManager.cardIteration.ContainsKey(_cardActive.InternalId))
      {
        matchManager.cardIteration[_cardActive.InternalId]++;
        if (matchManager.cardIteration[_cardActive.InternalId] < _cardIterationTotal)
        {
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[cardIteration] EXIT HERE BECAUSE " + matchManager.cardIteration[_cardActive.InternalId].ToString() + "<" + _cardIterationTotal.ToString(), "trace");
        }
        else
        {
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[cardIteration] CONTINUE " + matchManager.cardIteration[_cardActive.InternalId].ToString() + ">=" + _cardIterationTotal.ToString(), "trace");
          if (theCasterHero != null && theCasterHero.Alive && matchManager.energyJustWastedByHero > 5 && AtOManager.Instance.CharacterHavePerk(theCasterHero.SubclassName, "mainperkenergy2a"))
            theCasterHero.ModifyEnergy(1, true);
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("***** [cardIteration] castFinalIteration ** " + _cardActive.Id + " (" + matchManager.cardIteration[_cardActive.InternalId].ToString() + ") ******", "trace");
          if (_cardActive.MoveToCenter)
          {
            if (theCasterIsHero)
            {
              if (theCasterHero != null && theCasterHero.Alive && (UnityEngine.Object) theCasterHero.HeroItem != (UnityEngine.Object) null)
              {
                yield return (object) Globals.Instance.WaitForSeconds(0.2f);
                theCasterHero.HeroItem.MoveToCenterBack();
              }
            }
            else if (theCasterNPC != null && theCasterNPC.Alive && (UnityEngine.Object) theCasterNPC.NPCItem != (UnityEngine.Object) null)
            {
              yield return (object) Globals.Instance.WaitForSeconds(0.2f);
              theCasterNPC.NPCItem.MoveToCenterBack();
            }
          }
          if (doRedrawInitiatives)
            matchManager.ReDrawInitiatives();
          if (!matchManager.MatchIsOver)
          {
            if (GameManager.Instance.IsMultiplayer() && !_automatic)
            {
              castedEffect = false;
              while (!castedEffect)
              {
                if (matchManager.castingCardListMP.Count <= 1 || matchManager.castingCardListMP[matchManager.castingCardListMP.Count - 1] == _cardActive.Id)
                  castedEffect = true;
                if (!castedEffect)
                  yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              }
              if (!_cardActive.AutoplayDraw)
              {
                if (!NetworkManager.Instance.IsMaster())
                {
                  NetworkManager.Instance.SetWaitingSyncro("prefinishcast" + _cardActive.Id, true);
                  NetworkManager.Instance.SetWaitingSyncro("finishcast" + _cardActive.Id, true);
                }
                if (Globals.Instance.ShowDebug)
                  Functions.DebugLogGD("**************************", "net");
                if (Globals.Instance.ShowDebug)
                  Functions.DebugLogGD("WaitingSyncro prefinishcast" + _cardActive.Id, "net");
                if (matchManager.castingCardListMP.Count <= 1 && !matchManager.IsBeginTournPhase)
                {
                  lastId = matchManager.GenerateSyncCodeForCheckingAction();
                  iteration = 0;
                  _indexQueue = 20;
                  cardSpecialValue2 = 0;
                  while (iteration < _indexQueue)
                  {
                    while (matchManager.waitingKill)
                      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                    string forCheckingAction = matchManager.GenerateSyncCodeForCheckingAction();
                    if (lastId == forCheckingAction)
                    {
                      cardSpecialValue2 = 0;
                      ++iteration;
                    }
                    else
                    {
                      lastId = forCheckingAction;
                      iteration = 0;
                      ++cardSpecialValue2;
                    }
                    if (cardSpecialValue2 <= 200)
                      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                    else
                      break;
                  }
                  lastId = (string) null;
                }
                if (NetworkManager.Instance.IsMaster())
                {
                  if (matchManager.coroutineSyncPreFinishCast != null)
                    matchManager.StopCoroutine(matchManager.coroutineSyncPreFinishCast);
                  matchManager.coroutineSyncPreFinishCast = matchManager.StartCoroutine(matchManager.ReloadCombatCo("prefinishcast" + _cardActive.Id));
                  while (!NetworkManager.Instance.AllPlayersReady("prefinishcast" + _cardActive.Id))
                    yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                  if (matchManager.coroutineSyncPreFinishCast != null)
                    matchManager.StopCoroutine(matchManager.coroutineSyncPreFinishCast);
                  if (Globals.Instance.ShowDebug)
                    Functions.DebugLogGD("Game ready, Everybody checked prefinishcast" + _cardActive.Id, "net");
                  NetworkManager.Instance.PlayersNetworkContinue("prefinishcast" + _cardActive.Id);
                  yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                }
                else
                {
                  NetworkManager.Instance.SetStatusReady("prefinishcast" + _cardActive.Id);
                  while (NetworkManager.Instance.WaitingSyncro["prefinishcast" + _cardActive.Id])
                    yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                  if (Globals.Instance.ShowDebug)
                    Functions.DebugLogGD("prefinishcast, we can continue!", "net");
                }
                if (Globals.Instance.ShowDebug)
                  Functions.DebugLogGD("**************************", "net");
                if (Globals.Instance.ShowDebug)
                  Functions.DebugLogGD("WaitingSyncro finishcast" + _cardActive.Id, "net");
                if (NetworkManager.Instance.IsMaster())
                {
                  matchManager.SetRandomIndex(matchManager.randomIndex);
                  string syncCode = matchManager.GenerateSyncCode();
                  matchManager.photonView.RPC("NET_FinishCastCodeSyncFromMaster", RpcTarget.Others, (object) matchManager.randomIndex, (object) Functions.CompressString(syncCode), (object) _cardActive.Id);
                  if (matchManager.coroutineSyncFixSyncCode != null)
                    matchManager.StopCoroutine(matchManager.coroutineSyncFixSyncCode);
                  matchManager.coroutineSyncFixSyncCode = matchManager.StartCoroutine(matchManager.ReloadCombatCo("FixingSyncCode finishcast" + _cardActive.Id));
                  while (!NetworkManager.Instance.AllPlayersReady("finishcast" + _cardActive.Id))
                    yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                  if (matchManager.coroutineSyncFixSyncCode != null)
                    matchManager.StopCoroutine(matchManager.coroutineSyncFixSyncCode);
                  if (Globals.Instance.ShowDebug)
                    Functions.DebugLogGD("Game ready, Everybody checked finishcast" + _cardActive.Id, "net");
                  NetworkManager.Instance.PlayersNetworkContinue("finishcast" + _cardActive.Id);
                }
                else
                {
                  while (NetworkManager.Instance.WaitingSyncro["finishcast" + _cardActive.Id])
                    yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                  if (Globals.Instance.ShowDebug)
                    Functions.DebugLogGD("finishcast" + _cardActive.Id + ", we can continue!", "net");
                }
              }
              if (matchManager.castingCardListMP.Contains(_cardActive.Id))
                matchManager.castingCardListMP.Remove(_cardActive.Id);
            }
            matchManager.SetGameBusy(false);
            matchManager.gameStatus = "FinishingCardAction";
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD(matchManager.gameStatus, "gamestatus");
            if (matchManager.castingCardBlocked.ContainsKey(_cardActive.InternalId))
              matchManager.castingCardBlocked.Remove(_cardActive.InternalId);
            matchManager.CreateLogCastCard(false, _cardActive, _uniqueCastId, matchManager.theHero, matchManager.theNPC, _hero, _npc);
            if (matchManager.theNPC != null | _automatic)
            {
              yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              matchManager.SetEventDirect("CastCardEvent" + _cardActive.Id, false);
              matchManager.StartCoroutine(matchManager.CastCardEndAutomatic(_cardActive, _uniqueCastId, _hero, _npc));
            }
            else
            {
              if ((UnityEngine.Object) theCardItem != (UnityEngine.Object) null)
                theCardItem.DiscardCard(false);
              if (_cardActive.EndTurn)
                yield return (object) Globals.Instance.WaitForSeconds(0.5f);
              matchManager.SetEventDirect("CastCardEvent" + _cardActive.Id, false);
              matchManager.StartCoroutine(matchManager.CastCardEnd(_cardActive, _uniqueCastId, _hero, _npc));
            }
            MatchManager.Instance.RefreshStatusEffects();
            yield return (object) null;
          }
        }
      }
      else if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("[cardIteration] EXIT HERE BECAUSE CARDITERATION DOES NOT CONTAIN " + _cardActive.InternalId, "trace");
    }
  }

  private CardData GetCardForModify(CardData cardData, Character caster, Character target)
  {
    CardIdProvider addCardForModify = cardData.AddCardForModify;
    CardData cardForModify = (CardData) null;
    if ((UnityEngine.Object) addCardForModify != (UnityEngine.Object) null)
    {
      string cardId = addCardForModify.GetCardID(cardData, caster, target);
      if (!string.IsNullOrEmpty(cardId))
        cardForModify = Globals.Instance.GetCardData(cardId, false);
    }
    return cardForModify;
  }

  public int IncreaseDamage(Character character, int dmgTotal)
  {
    CardData cardActive = this.cardActive;
    if ((cardActive != null ? (cardActive.SpecialCardEnum == SpecialCardEnum.MindSpike ? 1 : 0) : 0) != 0)
      dmgTotal += (int) this.cardActive.SpecialValueModifierGlobal * this.mindSpikeAbility.CurrentSpecialCardsUsedInMatch;
    float num = 1f;
    return (int) ((double) dmgTotal * (double) num);
  }

  private bool IsExposedIllusion(Character character)
  {
    return character != null && character.Alive && character.IsIllusion && character.IsIllusionExposed;
  }

  private bool IsIllusion(Character character)
  {
    return character != null && character.Alive && character.IsIllusion;
  }

  private IEnumerator ApplyNightmareImageCo(CardData data, Character character)
  {
    int originalCharacterPositionInTeam = this.GetCharacterPositionInTeam(character);
    int illusionCharacterPositionInTeam = this.GetNPCAvailablePosition();
    bool isNPC = character is NPC;
    CharacterItem originalCharacterItem = isNPC ? (CharacterItem) character.NPCItem : (CharacterItem) character.HeroItem;
    Vector3 originalCharacterPosition = originalCharacterItem.CalculateLocalPosition(originalCharacterPositionInTeam);
    Vector3 illusionCharacterPosition = originalCharacterItem.CalculateLocalPosition(illusionCharacterPositionInTeam);
    originalCharacterItem.MoveToPosition(originalCharacterItem.transform, (originalCharacterPosition + illusionCharacterPosition) / 2f, true, false, 0.3f);
    yield return (object) new WaitWhile(new Func<bool>(originalCharacterItem.CharIsMoving));
    bool swapPositions = MapManager.Instance.GetRandomIntRange(0, 2) == 0;
    if (swapPositions)
    {
      int num1 = illusionCharacterPositionInTeam;
      int num2 = originalCharacterPositionInTeam;
      originalCharacterPositionInTeam = num1;
      illusionCharacterPositionInTeam = num2;
      if (isNPC)
      {
        this.TeamNPC[originalCharacterPositionInTeam] = this.TeamNPC[character.NPCIndex];
        this.NPCHand[originalCharacterPositionInTeam] = this.NPCHand[character.NPCIndex];
        this.NPCDeckDiscard[originalCharacterPositionInTeam] = this.NPCDeckDiscard[character.NPCIndex];
        this.NPCDeck[originalCharacterPositionInTeam] = this.NPCDeck[character.NPCIndex];
        character.SetNPCIndex(originalCharacterPositionInTeam);
      }
      else
      {
        this.TeamHero[originalCharacterPositionInTeam] = this.TeamHero[character.HeroIndex];
        this.HeroHand[originalCharacterPositionInTeam] = this.HeroHand[character.HeroIndex];
        this.HeroDeckDiscard[originalCharacterPositionInTeam] = this.HeroDeckDiscard[character.HeroIndex];
        this.HeroDeck[originalCharacterPositionInTeam] = this.HeroDeck[character.HeroIndex];
        character.SetNPCIndex(originalCharacterPositionInTeam);
      }
      character.Position = originalCharacterPositionInTeam;
    }
    Character illusion = this.CreateIllusion(character, data.EffectTarget, data, illusionCharacterPositionInTeam);
    CharacterItem illusionCharacterItem = isNPC ? (CharacterItem) illusion.NPCItem : (CharacterItem) illusion.HeroItem;
    illusionCharacterItem.transform.localPosition = new Vector3(originalCharacterItem.transform.localPosition.x, originalCharacterItem.transform.localPosition.y, originalCharacterItem.transform.localPosition.z);
    yield return (object) Globals.Instance.WaitForSeconds(0.3f);
    originalCharacterItem.MoveToPosition(originalCharacterItem.transform, swapPositions ? illusionCharacterPosition : originalCharacterPosition, true, false, 0.3f);
    illusionCharacterItem.MoveToPosition(illusionCharacterItem.transform, swapPositions ? originalCharacterPosition : illusionCharacterPosition, true, false, 0.3f);
    yield return (object) new WaitWhile((Func<bool>) (() => originalCharacterItem.CharIsMoving() || illusionCharacterItem.CharIsMoving()));
    this.RepositionCharacters();
  }

  private bool CanApplyNightmareImage(CardData card, Character casterCharacter)
  {
    if (card.SpecialCardEnum != SpecialCardEnum.NightmareImage || casterCharacter == null || !casterCharacter.Alive)
      return false;
    int num = this.EnchantmentExecutedTimes(casterCharacter.Id, card.ItemEnchantment?.Id);
    ItemData itemEnchantment = card.ItemEnchantment;
    return (itemEnchantment != null ? (itemEnchantment.DestroyAfterUses != num ? 1 : 0) : 1) == 0;
  }

  private void UpdateMasterReweaverProgress(Character casterCharacter, NPC npcWithAbility)
  {
    npcWithAbility.SetEvent(Enums.EventActivation.CastFightCard, casterCharacter);
  }

  private bool IsFightCard(CardData activeCard)
  {
    return activeCard.Damage > 0 || activeCard.Damage2 > 0 || activeCard.DamageSelf > 0 || activeCard.DamageSelf2 > 0;
  }

  private void ApplyCorruptedEcho(CardData data, CardItem cardItem, List<NPC> npcsWithAbility)
  {
    int num1 = 0;
    int num2 = 0;
    foreach (NPC npc in npcsWithAbility)
    {
      num1 = Globals.Instance.GetCardData("masterreweaver").ItemEnchantment.DestroyAfterUses;
      num2 = this.EnchantmentExecutedTimes(npc.Id, "masterreweaver");
    }
    if (num1 == 0 || num2 >= num1)
      return;
    data.TempAttackSelf = true;
    data.TargetPosition = Enums.CardTargetPosition.Anywhere;
    data.TargetSide = Enums.CardTargetSide.Self;
    data.TargetType = Enums.CardTargetType.Single;
    data.EffectCastCenter = false;
    data.SetTarget();
    if (!(bool) (UnityEngine.Object) cardItem)
      return;
    cardItem.TargetTextTM = data.Target;
  }

  private void ReturnCardDataToOriginalState(CardData data, CardItem cardItem)
  {
    CardData cardData = Globals.Instance.GetCardData(data.Id, false);
    data.TempAttackSelf = false;
    data.TargetPosition = cardData.TargetPosition;
    data.TargetSide = cardData.TargetSide;
    data.TargetType = cardData.TargetType;
    data.EffectCastCenter = cardData.EffectCastCenter;
    data.SetTarget();
    if (!(bool) (UnityEngine.Object) cardItem)
      return;
    cardItem.TargetTextTM = data.Target;
  }

  private int GetCharacterPositionInTeam(Character character)
  {
    int characterPositionInTeam = 0;
    Character[] characterArray = character is NPC ? (Character[]) this.TeamNPC : (Character[]) this.TeamHero;
    for (int index = 0; index < characterArray.Length; ++index)
    {
      if (character == characterArray[index])
        characterPositionInTeam = index;
    }
    return characterPositionInTeam;
  }

  private bool TryGetTransferPainTarget(CardData cardActive, NPC currentNPC, out NPC targetNPC)
  {
    targetNPC = (NPC) null;
    if (cardActive.SpecialCardEnum != SpecialCardEnum.TransferPain || currentNPC == null || !currentNPC.Alive)
      return false;
    foreach (NPC npc in this.TeamNPC)
    {
      if (npc != null && npc.Alive && !npc.IsIllusion)
      {
        foreach (AICards aiCard in currentNPC.NpcData.AICards)
        {
          if (npc.GameName.StartsWith(aiCard.SpecialSecondTargetID, StringComparison.OrdinalIgnoreCase) && npc.GetHp() > currentNPC.GetHp())
          {
            targetNPC = npc;
            return true;
          }
        }
      }
    }
    return false;
  }

  private bool CanUpdateMindSpikeProgress(CardData data)
  {
    return data.SpecialCardEnum == this.mindSpikeAbility.CollectedSpecialCard;
  }

  private void ApplyTransferPain(NPC currentNPC, NPC targetNPC)
  {
    int hp1 = currentNPC.GetHp();
    int maxHp1 = currentNPC.GetMaxHP();
    int hp2 = targetNPC.GetHp();
    int maxHp2 = targetNPC.GetMaxHP();
    currentNPC.HpCurrent = Mathf.Min(hp2, maxHp1);
    targetNPC.HpCurrent = Mathf.Min(hp1, maxHp2);
    currentNPC.SetHP();
    targetNPC.SetHP();
  }

  private IEnumerator CastCardEnd(
    CardData _cardActive,
    string _uniqueCastId,
    Hero _hero,
    NPC _npc)
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("[CastCardEnd] END " + _cardActive.InternalId + " // " + _uniqueCastId, "general");
    while (this.waitingDeathScreen)
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("[CastCardEnd] waitingDeathScreen", "trace");
      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
    }
    if (this.theHero == null && this.theNPC != null)
    {
      this.SetGameBusy(false);
      this.gameStatus = nameof (CastCardEnd);
    }
    else
    {
      if (this.theHero != null && (UnityEngine.Object) this.theHero.HeroData != (UnityEngine.Object) null)
      {
        this.theHero.HealAuraCurse(Globals.Instance.GetAuraCurseData("stealthbonus"));
        if (this.theHero.HeroData.HeroSubClass.Id == "queen" && this.theHero.HaveTrait("timeparadox") && _cardActive.Id.Contains("eldritchdischarge") && !MatchManager.Instance.ItemExecuteForThisCombat(this.theHero.HeroData.HeroSubClass.Id, "timeparadox", 1, ""))
        {
          string cardInDictionary = this.CreateCardInDictionary(_cardActive.Id);
          CardData cardData = this.GetCardData(cardInDictionary);
          cardData.EnergyReductionToZeroPermanent = true;
          this.ModifyCardInDictionary(cardInDictionary, cardData);
          this.GenerateNewCard(1, cardInDictionary, false, Enums.CardPlace.RandomDeck, heroIndex: this.theHero.HeroIndex);
          this.SetTraitInfoText();
          this.theHero.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Time Paradox") + Functions.TextChargesLeft(this.ItemExecutedInThisCombat(this.theHero.SubclassName, "timeparadox"), 1), Enums.CombatScrollEffectType.Trait);
        }
        if (this.theHero.HeroData.HeroSubClass.Id == "engineer" && (UnityEngine.Object) _cardActive != (UnityEngine.Object) null && _cardActive.GetCardTypes().Contains(Enums.CardType.Ranged_Attack))
        {
          bool flag = false;
          if (this.theHero.HaveTrait("doublebarrel"))
          {
            this.theHero.SetAura((Character) this.theHero, Globals.Instance.GetAuraCurseData("fury"), 2);
            this.theHero.SetAura((Character) this.theHero, Globals.Instance.GetAuraCurseData("fast"), 1);
            if (this.ItemExecuteForThisTurn(this.theHero.HeroData.HeroSubClass.Id, "loadedgun", 1, ""))
              flag = true;
            this.theHero.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Double Barrel"), Enums.CombatScrollEffectType.Trait);
          }
          else
            flag = true;
          if (flag)
            this.theHero.SetAura((Character) null, Globals.Instance.GetAuraCurseData("reloading"), 1);
        }
      }
      if (this.theHero != null && _cardActive.CardType != Enums.CardType.Corruption)
      {
        this.keyClickedCard = false;
        this.combatTarget.Refresh();
        this.WriteNewCardsCounter();
        this.RedrawCardsDescriptionPrecalculated();
        this.DrawDeckPileLayer("Cards");
        this.SetGlobalOutlines(false);
        this.SetGameBusy(false);
        this.gameStatus = "PreCastCardEnd";
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD(this.gameStatus, "gamestatus");
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (this.castingCardBlocked.ContainsKey(_cardActive.InternalId))
        {
          this.castingCardBlocked.Remove(_cardActive.InternalId);
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[CastCardEnd] **** CASTINGCARDBLOCKED REMOVE " + _cardActive.InternalId);
        }
        if (_cardActive.EndTurn)
          this.EndTurn();
        else if (this.CheckMatchIsOver())
        {
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[CastCardEnd] FINISH CAST FINISH COMBAT");
          this.FinishCombat();
        }
        else
        {
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[CastCardEnd] FINISH CAST " + _cardActive.Id + " (" + this.cardsWaitingForReset.ToString() + ")");
          int indexExhaust = 0;
          if (!_cardActive.AutoplayDraw)
          {
            while (this.cardsWaitingForReset > 0)
            {
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("[CastCardEnd] CardsWaitingForReset " + this.cardsWaitingForReset.ToString(), "trace");
              yield return (object) Globals.Instance.WaitForSeconds(0.1f);
              ++indexExhaust;
              if (indexExhaust > 100)
                this.SetCardsWaitingForReset(0);
            }
          }
          if (this.TeamHero != null && this.heroActive > -1 && this.heroActive < this.TeamHero.Length && this.TeamHero[this.heroActive] != null)
          {
            this.TeamHero[this.heroActive].SetCastedCard(_cardActive);
            if ((UnityEngine.Object) _cardActive != (UnityEngine.Object) null && !_cardActive.AutoplayDraw && !_cardActive.AutoplayEndTurn)
            {
              this.gameStatus = "PreFinishCast";
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD(this.gameStatus, "gamestatus");
              if (!_cardActive.IsPetAttack && !_cardActive.IsPetCast && (this.TeamHero[this.heroActive].HaveItemToActivate(Enums.EventActivation.PreFinishCast, _checkForCorruption: true) || this.TeamHero[this.heroActive].HaveTraitToActivate(Enums.EventActivation.PreFinishCast)))
              {
                this.TeamHero[this.heroActive].SetEvent(Enums.EventActivation.PreFinishCast);
                yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                while (this.generatedCardTimes > 0)
                  yield return (object) Globals.Instance.WaitForSeconds(0.7f);
                while (this.waitingKill)
                  yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              }
              this.ResetAutoEndCount();
              if (this.TeamHero[this.heroActive].HaveItemToActivate(Enums.EventActivation.FinishCast, _checkForCorruption: true) || this.TeamHero[this.heroActive].HaveTraitToActivate(Enums.EventActivation.FinishCast))
              {
                this.TeamHero[this.heroActive].SetEvent(Enums.EventActivation.FinishCast);
                yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                while (this.waitingKill)
                  yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              }
              this.ResetAutoEndCount();
              if (!_cardActive.IsPetAttack && !_cardActive.IsPetCast)
              {
                if (this.heroActive > -1 && this.heroActive < this.TeamHero.Length && (this.TeamHero[this.heroActive].HaveItemToActivate(Enums.EventActivation.FinishFinishCast, _checkForCorruption: true) || this.TeamHero[this.heroActive].HaveTraitToActivate(Enums.EventActivation.FinishFinishCast)))
                {
                  this.TeamHero[this.heroActive].SetEvent(Enums.EventActivation.FinishFinishCast);
                  yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                  while (this.generatedCardTimes > 0)
                    yield return (object) Globals.Instance.WaitForSeconds(0.7f);
                  while (this.waitingKill)
                    yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                  yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                }
                if (this.heroActive > -1 && this.heroActive < this.TeamHero.Length && (this.TeamHero[this.heroActive].HaveItemToActivate(Enums.EventActivation.FinishFinishCastCorruption, _checkForCorruption: true) || this.TeamHero[this.heroActive].HaveTraitToActivate(Enums.EventActivation.FinishFinishCastCorruption)))
                {
                  this.TeamHero[this.heroActive].SetEvent(Enums.EventActivation.FinishFinishCastCorruption);
                  yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                  while (this.generatedCardTimes > 0)
                    yield return (object) Globals.Instance.WaitForSeconds(0.7f);
                  while (this.waitingKill)
                    yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                  yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                }
              }
            }
            this.ResetAutoEndCount();
          }
          while (this.eventList.Count > 0 || this.generatedCardTimes > 0)
            yield return (object) Globals.Instance.WaitForSeconds(0.01f);
          this.ResetAutoEndCount();
          this.gameStatus = nameof (CastCardEnd);
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD(this.gameStatus, "gamestatus");
          this.CreateLogEntry(true, "finishCast:" + this.logDictionary.Count.ToString(), "", (Hero) null, (NPC) null, (Hero) null, (NPC) null, this.currentRound);
        }
      }
    }
  }

  private IEnumerator CastCardEndAutomatic(
    CardData _cardActive,
    string _uniqueCastId,
    Hero _hero,
    NPC _npc)
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("[CastCardEndAutomatic] " + _cardActive.Id + " // " + _uniqueCastId, "trace");
    if (this.theNPC != null && this.theNPC.Alive && (UnityEngine.Object) this.theNPC.NPCItem != (UnityEngine.Object) null && this.npcActive > -1)
    {
      this.theNPC.HealAuraCurse(Globals.Instance.GetAuraCurseData("stealthbonus"));
      GameManager.Instance.PlayLibraryAudio("castnpccard");
      this.theNPC.CastCardNPCEnd();
      this.SetGameBusy(false);
      this.gameStatus = "CastCardEnd";
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD(this.gameStatus, "gamestatus");
      this.waitingItemTrait = true;
      if (this.TeamNPC != null && this.npcActive < this.TeamNPC.Length)
        this.TeamNPC[this.npcActive].SetEvent(Enums.EventActivation.FinishCast);
      while (this.waitingItemTrait)
      {
        this.waitingItemTrait = false;
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[CastCardEndAutomatic] NPC waitingItemTrait", "trace");
        yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      }
      if (_cardActive.EndTurn)
        this.EndTurn();
      else if (this.CheckMatchIsOver())
      {
        this.FinishCombat();
      }
      else
      {
        if (this.theHeroPreAutomatic != null)
          this.theHero = this.theHeroPreAutomatic;
        if (this.theNPCPreAutomatic != null)
          this.theNPC = this.theNPCPreAutomatic;
        if (!this.isBeginTournPhase)
        {
          if (this.theNPC.NPCIsBoss() && this.bossNpc != null && this.bossNpc is PhantomArmor)
          {
            PhantomArmor pa = this.bossNpc as PhantomArmor;
            while (!pa.IsSpecialEffectFinished())
              yield return (object) Globals.Instance.WaitForSeconds(0.1f);
            pa = (PhantomArmor) null;
          }
          this.CastNPC();
        }
      }
    }
    else
      this.SetGameBusy(false);
  }

  public void ShowTraitInfo(bool state, bool clearText = false)
  {
    if (clearText)
      this.traitInfoText.text = "";
    if (state && this.heroActive == -1)
      state = false;
    if (this.traitInfo.gameObject.activeSelf == state)
      return;
    this.traitInfo.gameObject.SetActive(state);
  }

  public void SetTraitInfoText()
  {
    if (this.theHero == null)
      return;
    StringBuilder stringBuilder1 = new StringBuilder();
    bool flag = false;
    if (this.theHero.Traits != null)
    {
      for (int index = 0; index < this.theHero.Traits.Length; ++index)
      {
        if (this.theHero.Traits[index] != null)
        {
          TraitData traitData = Globals.Instance.GetTraitData(this.theHero.Traits[index]);
          if ((UnityEngine.Object) traitData != (UnityEngine.Object) null)
          {
            if (traitData.Id == "ragnarok" && !this.theHero.HaveTrait("spellsinger"))
              traitData = Globals.Instance.GetTraitData("spellsinger");
            int num1 = 0;
            if (traitData.TimesPerTurn > 0 || traitData.TimesPerRound > 0)
            {
              stringBuilder1.Append(traitData.TraitName);
              stringBuilder1.Append("  ");
              int num2;
              if (traitData.TimesPerTurn > 0)
              {
                num2 = traitData.TimesPerTurn;
                if (traitData.Id == "warriorduality" && this.theHero.HaveTrait("beaconoflight"))
                  ++num2;
                else if (traitData.Id == "magicduality" && this.theHero.HaveTrait("unholyblight"))
                  ++num2;
                else if (traitData.Id == "healerduality" && this.theHero.HaveTrait("philosophersstone"))
                  ++num2;
                else if (traitData.Id == "scoutduality" && this.theHero.HaveTrait("valhalla"))
                  ++num2;
                else if (traitData.Id == "spellsinger" && this.theHero.HaveTrait("spellsinger") && this.theHero.HaveTrait("ragnarok"))
                  ++num2;
              }
              else
                num2 = traitData.TimesPerRound;
              if (this.activatedTraits.ContainsKey(traitData.Id))
                stringBuilder1.Append(this.activatedTraits[traitData.Id]);
              else if (this.activatedTraitsRound.ContainsKey(traitData.Id))
                stringBuilder1.Append(this.activatedTraitsRound[traitData.Id]);
              else
                stringBuilder1.Append("0");
              stringBuilder1.Append("/");
              stringBuilder1.Append(num2);
              stringBuilder1.Append("<br>");
              flag = true;
            }
            else if (traitData.Id == "timeloop" || traitData.Id == "timeparadox")
            {
              stringBuilder1.Append(traitData.TraitName);
              stringBuilder1.Append("  ");
              if (traitData.Id == "timeloop")
                num1 = !this.theHero.HaveTrait("TimeParadox") ? 1 : 2;
              else if (traitData.Id == "timeparadox")
                num1 = 1;
              StringBuilder stringBuilder2 = new StringBuilder();
              stringBuilder2.Append(this.theHero.SubclassName);
              stringBuilder2.Append("_");
              stringBuilder2.Append(traitData.Id);
              if (this.itemExecutedInCombat.ContainsKey(stringBuilder2.ToString()))
                stringBuilder1.Append(this.itemExecutedInCombat[stringBuilder2.ToString()]);
              else
                stringBuilder1.Append("0");
              stringBuilder1.Append("/");
              stringBuilder1.Append(num1);
              stringBuilder1.Append("<br>");
              flag = true;
            }
          }
        }
      }
    }
    if (!flag)
      return;
    StringBuilder stringBuilder3 = new StringBuilder();
    stringBuilder3.Append("<size=-.2><size=-.4><sprite name=experience></size><color=#F0A169FF>");
    stringBuilder3.Append(Texts.Instance.GetText("activatedTraits"));
    stringBuilder3.Append("</color></size><br>");
    stringBuilder1.Insert(0, stringBuilder3.ToString());
    this.traitInfoText.text = stringBuilder1.ToString();
  }

  private void AddCardModificationsForCard(
    CardData _cardActive,
    CardData _cardTarget,
    CopyConfig copyConfig = null)
  {
    if ((UnityEngine.Object) _cardTarget != (UnityEngine.Object) null && (UnityEngine.Object) _cardActive != (UnityEngine.Object) null && Globals.Instance.ShowDebug)
      Functions.DebugLogGD("AddCardModification to " + _cardTarget.Id + " from " + _cardActive.Id, "general");
    if (_cardActive.AddCardReducedCost != 0)
    {
      if (_cardActive.AddCardReducedCost == -1)
      {
        if (!_cardActive.AddCardCostTurn)
          _cardTarget.EnergyReductionToZeroPermanent = true;
        else
          _cardTarget.EnergyReductionToZeroTemporal = true;
      }
      else if (!_cardActive.AddCardCostTurn)
        _cardTarget.EnergyReductionPermanent += _cardActive.AddCardReducedCost;
      else
        _cardTarget.EnergyReductionTemporal += _cardActive.AddCardReducedCost;
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("EnergyCost " + _cardTarget.EnergyCost.ToString(), "general");
    }
    if (_cardActive.AddCardVanish)
      _cardTarget.Vanish = true;
    if (copyConfig == null)
      return;
    this.CopyCardDataFromAnotherCard(_cardActive, _cardTarget, copyConfig);
  }

  public void CopyCardDataFromAnotherCard(
    CardData _cardActive,
    CardData _cardTarget,
    CopyConfig copyConfig)
  {
    if ((UnityEngine.Object) _cardTarget == (UnityEngine.Object) null || (UnityEngine.Object) _cardActive == (UnityEngine.Object) null || copyConfig == null)
      return;
    if (copyConfig.CopyNameFromOriginal)
      _cardTarget.CardName = _cardActive.CardName;
    if (copyConfig.CopyCardUpgradedFromOriginal)
    {
      _cardTarget.UpgradedFrom = _cardActive.UpgradedFrom;
      _cardTarget.UpgradesTo1 = _cardActive.UpgradesTo1;
      _cardTarget.UpgradesTo2 = _cardActive.UpgradesTo2;
      _cardTarget.UpgradesToRare = _cardActive.UpgradesToRare;
      _cardTarget.CardUpgraded = _cardActive.CardUpgraded;
    }
    if (copyConfig.CopyImageFromOriginal)
      _cardTarget.Sprite = _cardActive.Sprite;
    if (!copyConfig.CopyCardTypeFromOriginal)
      return;
    _cardTarget.CardType = _cardActive.CardType;
  }

  private void AddCardModificationsForCardForShow(CardData _cardActive, CardData _cardTarget)
  {
    if (this.theHero != null)
      _cardTarget.EnergyCostForShow = this.theHero.GetCardFinalCost(_cardTarget);
    else if (this.theNPC != null)
      _cardTarget.EnergyCostForShow = this.theNPC.GetCardFinalCost(_cardTarget);
    if (_cardActive.AddCardReducedCost == 0)
      return;
    if (_cardActive.AddCardReducedCost == -1)
    {
      _cardTarget.EnergyCostForShow = 0;
    }
    else
    {
      _cardTarget.EnergyCostForShow -= _cardActive.AddCardReducedCost;
      if (_cardTarget.EnergyCostForShow >= 0)
        return;
      _cardTarget.EnergyCostForShow = 0;
    }
  }

  public void ItemTraitActivated(bool state = true) => this.waitingItemTrait = state;

  public void SortCharacterSprites(bool toFront = false, int heroIndex = -1, int npcIndex = -1)
  {
    for (int index = 0; index < this.TeamHero.Length; ++index)
    {
      if ((!toFront || heroIndex == index) && this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroData == (UnityEngine.Object) null) && this.TeamHero[index].Alive && (UnityEngine.Object) this.TeamHero[index].HeroItem != (UnityEngine.Object) null)
        this.TeamHero[index].HeroItem.DrawOrderSprites(toFront, this.TeamHero[index].Position * 2);
    }
    for (int index = 0; index < this.TeamNPC.Length; ++index)
    {
      if ((!toFront || npcIndex == index) && this.TeamNPC[index] != null && !((UnityEngine.Object) this.TeamNPC[index].NpcData == (UnityEngine.Object) null) && this.TeamNPC[index].Alive && (UnityEngine.Object) this.TeamNPC[index].NPCItem != (UnityEngine.Object) null)
        this.TeamNPC[index].NPCItem.DrawOrderSprites(toFront, this.TeamNPC[index].Position * 2);
    }
  }

  public Hero[] GetTeamHero() => this.TeamHero;

  public NPC[] GetTeamNPC() => this.TeamNPC;

  public NPC GetNPCCharacter(int _index)
  {
    return _index >= 0 || _index <= 3 ? this.TeamNPC[_index] : (NPC) null;
  }

  public NPC GetNPCById(string theId)
  {
    for (int index = 0; index < this.TeamNPC.Length; ++index)
    {
      if (this.TeamNPC[index] != null && !((UnityEngine.Object) this.TeamNPC[index].NpcData == (UnityEngine.Object) null) && this.TeamNPC[index].Id == theId)
        return this.TeamNPC[index];
    }
    return (NPC) null;
  }

  public Hero GetHeroById(string theId)
  {
    for (int index = 0; index < this.TeamHero.Length; ++index)
    {
      if (this.TeamHero[index] != null && this.TeamHero[index].Id == theId)
        return this.TeamHero[index];
    }
    return (Hero) null;
  }

  public Character GetCharacterById(string theId)
  {
    return (Character) this.GetHeroById(theId) ?? (Character) this.GetNPCById(theId);
  }

  public bool PositionIsFront(bool isHero, int position)
  {
    if (isHero)
    {
      bool flag = false;
      List<Hero> heroList = new List<Hero>();
      for (int index = 0; index < this.TeamHero.Length; ++index)
      {
        if (this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroData == (UnityEngine.Object) null))
        {
          Hero hero = this.TeamHero[index];
          if (hero.Alive && hero.HasEffect("taunt") && !hero.HasEffect("stealth"))
          {
            heroList.Add(hero);
            flag = true;
          }
        }
      }
      if (flag)
      {
        for (int index = 0; index < heroList.Count; ++index)
        {
          if (heroList[index].Alive)
            return heroList[index].Position == position;
        }
      }
      else
      {
        for (int index = 0; index < this.TeamHero.Length; ++index)
        {
          if (this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroData == (UnityEngine.Object) null) && this.TeamHero[index].Alive && !this.TeamHero[index].HasEffect("stealth"))
            return this.TeamHero[index].Position == position;
        }
      }
    }
    else
    {
      bool flag = false;
      List<NPC> npcList = new List<NPC>();
      for (int index = 0; index < this.TeamNPC.Length; ++index)
      {
        if (this.TeamNPC[index] != null && !((UnityEngine.Object) this.TeamNPC[index].NpcData == (UnityEngine.Object) null))
        {
          NPC npc = this.TeamNPC[index];
          if (npc.Alive && npc.HasEffect("taunt") && !npc.HasEffect("stealth"))
          {
            npcList.Add(npc);
            flag = true;
          }
        }
      }
      if (flag)
      {
        for (int index = 0; index < npcList.Count; ++index)
        {
          if (npcList[index].Alive)
            return npcList[index].Position == position;
        }
      }
      else
      {
        for (int index = 0; index < this.TeamNPC.Length; ++index)
        {
          if (this.TeamNPC[index] != null && !((UnityEngine.Object) this.TeamNPC[index].NpcData == (UnityEngine.Object) null) && this.TeamNPC[index].Alive && !this.TeamNPC[index].HasEffect("stealth"))
            return this.TeamNPC[index].Position == position;
        }
      }
    }
    return false;
  }

  public bool PositionIsBack(Character character)
  {
    if (character == null || !character.Alive || character.HasEffect("stealth"))
      return false;
    int position = character.Position;
    if (character.IsHero)
    {
      bool flag = false;
      List<Hero> heroList = new List<Hero>();
      for (int index = 0; index < this.TeamHero.Length; ++index)
      {
        if (this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroData == (UnityEngine.Object) null))
        {
          Hero hero = this.TeamHero[index];
          if (hero.Alive && hero.HasEffect("taunt"))
          {
            heroList.Add(hero);
            flag = true;
          }
        }
      }
      if (flag)
      {
        for (int index = 0; index < heroList.Count; ++index)
        {
          if (heroList[index].Alive && heroList[index].Position > position && !heroList[index].HasEffect("stealth"))
            return false;
        }
      }
      else
      {
        for (int index = 0; index < this.TeamHero.Length; ++index)
        {
          if (this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroData == (UnityEngine.Object) null) && this.TeamHero[index].Alive && this.TeamHero[index].Position > position && !this.TeamHero[index].HasEffect("stealth"))
            return false;
        }
      }
    }
    else
    {
      bool flag = false;
      List<NPC> npcList = new List<NPC>();
      for (int index = 0; index < this.TeamNPC.Length; ++index)
      {
        if (this.TeamNPC[index] != null && !((UnityEngine.Object) this.TeamNPC[index].NpcData == (UnityEngine.Object) null))
        {
          NPC npc = this.TeamNPC[index];
          if (npc.Alive && npc.HasEffect("taunt"))
          {
            npcList.Add(npc);
            flag = true;
          }
        }
      }
      if (flag)
      {
        for (int index = 0; index < npcList.Count; ++index)
        {
          if (npcList[index].Alive && npcList[index].Position > position && !npcList[index].HasEffect("stealth"))
            return false;
        }
      }
      else
      {
        for (int index = 0; index < this.TeamNPC.Length; ++index)
        {
          if (this.TeamNPC[index] != null && !((UnityEngine.Object) this.TeamNPC[index].NpcData == (UnityEngine.Object) null) && this.TeamNPC[index].Alive && this.TeamNPC[index].Position > position && !this.TeamNPC[index].HasEffect("stealth"))
            return false;
        }
      }
    }
    return true;
  }

  public bool PositionIsMiddle(Character character)
  {
    if (character == null || !character.Alive)
      return false;
    if (character.IsHero)
    {
      bool flag1 = this.PositionIsFront(true, character.Position);
      bool flag2 = this.PositionIsBack(character);
      if (this.NumHeroesAlive() <= 2 || !(flag1 | flag2))
        return true;
      bool flag3 = true;
      for (int index = 0; index < 4; ++index)
      {
        if (this.TeamHero[index] != null && (UnityEngine.Object) this.TeamHero[index].HeroData != (UnityEngine.Object) null && this.TeamHero[index].Alive && character.Id != this.TeamHero[index].Id && !this.PositionIsFront(true, this.TeamHero[index].Position) && !this.PositionIsBack((Character) this.TeamHero[index]) && !this.TeamHero[index].HasEffect("Stealth"))
        {
          flag3 = false;
          break;
        }
      }
      return flag3;
    }
    bool flag4 = this.PositionIsFront(false, character.Position);
    bool flag5 = this.PositionIsBack(character);
    return this.NumNPCsAlive() <= 2 || !(flag4 | flag5);
  }

  public void SetGlobalOutlines(bool state, CardData cardOver = null)
  {
    for (int index = 0; index < this.TeamHero.Length; ++index)
    {
      if (this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroData == (UnityEngine.Object) null) && this.TeamHero[index].Alive)
      {
        HeroItem heroItem = this.TeamHero[index].HeroItem;
        if ((UnityEngine.Object) heroItem == (UnityEngine.Object) null)
          return;
        if (!state)
          heroItem.OutlineHide();
        else if (this.CheckTarget(heroItem.transform, cardOver))
          heroItem.OutlineGreen();
        else
          heroItem.OutlineRed();
      }
    }
    for (int index = 0; index < this.TeamNPC.Length; ++index)
    {
      if (this.TeamNPC[index] != null && !((UnityEngine.Object) this.TeamNPC[index].NpcData == (UnityEngine.Object) null) && this.TeamNPC[index].Alive)
      {
        NPCItem npcItem = this.TeamNPC[index].NPCItem;
        if ((UnityEngine.Object) npcItem != (UnityEngine.Object) null)
        {
          if (!state)
            npcItem.OutlineHide();
          else if (this.CheckTarget(npcItem.transform, cardOver))
            npcItem.OutlineGreen();
          else
            npcItem.OutlineRed();
        }
      }
    }
  }

  public int CountHeroHand(int hero = -1)
  {
    if (this.HeroHand == null)
      return 0;
    if (hero == -1)
      hero = this.heroActive;
    return hero == -1 ? 0 : this.HeroHand[hero].Count;
  }

  public int CountNPCHand(int _npc = -1)
  {
    if (this.NPCHand == null)
      return 0;
    if (_npc == -1)
      _npc = this.npcActive;
    if (_npc == -1)
      return 0;
    if (this.NPCHand[_npc] == null)
      this.NPCHand[_npc] = new List<string>();
    return this.NPCHand[_npc].Count;
  }

  public int GetHeroHandEnergyTotalValue(int _index)
  {
    int energyTotalValue = 0;
    if (_index == this.heroActive)
    {
      for (int index = 0; index < this.HeroHand[_index].Count; ++index)
        energyTotalValue += this.GetCardFromTableByIndex(this.HeroHand[_index][index]).GetEnergyCost();
    }
    return energyTotalValue;
  }

  public List<string> GetHeroDeck(int _index) => this.HeroDeck[_index];

  public List<string> GetHeroDiscard(int _index) => this.HeroDeckDiscard[_index];

  public List<string> GetHeroHand(int _index) => this.HeroHand[_index];

  public List<string> GetHeroVanish(int _index) => this.HeroDeckVanish[_index];

  public int GetCardIndexInTableById(string internalId)
  {
    for (int index = 0; index < this.cardItemTable.Count; ++index)
    {
      if (this.cardItemTable[index].InternalId == internalId)
        return index;
    }
    return -1;
  }

  public CardItem GetCardFromTableByIndex(string internalId)
  {
    for (int index = 0; index < this.cardItemTable.Count; ++index)
    {
      if (this.cardItemTable[index].InternalId == internalId)
        return this.cardItemTable[index];
    }
    return (CardItem) null;
  }

  public void UpdateHandCards()
  {
    for (int index = 0; index < this.cardItemTable.Count; ++index)
      this.cardItemTable[index].DrawEnergyCost();
  }

  public int CountHeroDeck(int hero = -1)
  {
    if (this.HeroDeck == null)
      return 0;
    if (hero == -1)
      hero = this.heroActive;
    return hero == -1 ? 0 : this.HeroDeck[hero].Count;
  }

  public int CountNPCActiveDeck() => this.CountNPCDeck(this.npcActive);

  public int CountNPCDeck(int npc = -1)
  {
    if (this.NPCDeck == null)
      return 0;
    if (npc == -1)
      npc = this.npcActive;
    return npc == -1 || this.NPCDeck[npc] == null ? 0 : this.NPCDeck[npc].Count;
  }

  public int CountHeroDiscard(int hero = -1)
  {
    if (hero == -1)
      hero = this.heroActive;
    return hero > -1 && this.HeroDeckDiscard[hero] != null ? this.HeroDeckDiscard[hero].Count : 0;
  }

  public int CountHeroVanish(int hero = -1)
  {
    if (hero == -1)
      hero = this.heroActive;
    return hero > -1 && this.HeroDeckVanish[hero] != null ? this.HeroDeckVanish[hero].Count : 0;
  }

  public int CountNPCDiscard(int npc = -1)
  {
    if (npc == -1)
      npc = this.npcActive;
    return npc > -1 && this.NPCDeckDiscard[npc] != null ? this.NPCDeckDiscard[npc].Count : 0;
  }

  private void RemoveCardFromDiscardPile(string id)
  {
    GameObject gameObject = GameObject.Find("/World/Decks/DiscardPile/" + id);
    if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) gameObject);
  }

  public void DeckParticlesShow(int type, bool state)
  {
    if (type == 0)
    {
      ParticleSystem.MainModule main1 = this.deckPileParticle.main;
    }
    else
    {
      ParticleSystem.MainModule main2 = this.discardPileParticle.main;
    }
    if (state)
    {
      if (type == 0 && this.CountHeroDeck() > 0)
      {
        this.deckPileParticle.Play();
      }
      else
      {
        if (type != 1 || this.CountHeroDiscard() <= 0)
          return;
        this.discardPileParticle.Play();
      }
    }
    else if (type == 0 && this.CountHeroDeck() > 0)
    {
      this.deckPileParticle.Stop();
    }
    else
    {
      if (type != 1 || this.CountHeroDiscard() <= 0)
        return;
      this.discardPileParticle.Stop();
    }
  }

  public CardData GetCardData(string id, bool createInDict = true)
  {
    if (id != "")
    {
      id = id.ToLower();
      if (createInDict)
      {
        if (this.cardDictionary == null)
          this.cardDictionary = new Dictionary<string, CardData>();
        if (!this.cardDictionary.ContainsKey(id))
          this.CreateCardInDictionary(id, _overwriteId: true);
      }
      if (this.cardDictionary != null && this.cardDictionary.ContainsKey(id))
        return this.cardDictionary[id];
    }
    return (CardData) null;
  }

  public void SelectedCardPosition(int cardPosition, bool state)
  {
    if (cardPosition <= -1)
      return;
    int totalCards = this.CountHeroHand();
    for (int index = 0; index < totalCards; ++index)
      this.cardItemTable[index].AdjustPositionBecauseHover(cardPosition, state, totalCards);
  }

  public void SetCardHover(int cardPosition, bool state)
  {
    if (this.gameStatus != "" && this.gameStatus != "CastCardEnd")
      return;
    if (state)
    {
      CardData cData = this.cardItemTable[cardPosition].CardData;
      CardItem cardItem = this.cardItemTable.FirstOrDefault<CardItem>((Func<CardItem, bool>) (i => i.CardData.Id == cData.Id));
      List<NPC> npcsWithAbility;
      bool masterReweaverAbility = this.TryGetNPCsWithMasterReweaverAbility(cData, (Character) this.TeamHero[this.heroActive], out npcsWithAbility);
      if (!cData.TempAttackSelf & masterReweaverAbility)
        this.ApplyCorruptedEcho(cData, cardItem, npcsWithAbility);
      else if (cData.TempAttackSelf && !masterReweaverAbility)
        this.ReturnCardDataToOriginalState(cData, cardItem);
      this.cardActive = cData;
      this.SetDamagePreview(true, cData, cardPosition);
      this.SetOverDeck(true);
      if (this.cardHoverIndex == cardPosition)
        return;
      if (this.cardHoverIndex > -1)
        this.SelectedCardPosition(this.cardHoverIndex, false);
      this.SelectedCardPosition(cardPosition, true);
      this.cardHoverIndex = cardPosition;
    }
    else
    {
      this.SelectedCardPosition(cardPosition, false);
      this.ResetCardHoverIndex();
    }
  }

  public void ResetCardHoverIndex() => this.cardHoverIndex = -1;

  public void PreSelectCard() => this.deckCardsWindowPosY = this.deckCardsWindow.GetScrolled();

  [PunRPC]
  private void NET_ShareArrDiscardCard(string _arrToDiscardCard, int _randomIndex)
  {
    this.SetRandomIndex(_randomIndex);
    this.StartCoroutine(this.NET_ShareArrDiscardCardCo(_arrToDiscardCard, _randomIndex));
  }

  private IEnumerator NET_ShareArrDiscardCardCo(string _arrToDiscardCard, int _randomIndex)
  {
    MatchManager matchManager = this;
    List<string> list = ((IEnumerable<string>) JsonHelper.FromJson<string>(_arrToDiscardCard)).ToList<string>();
    matchManager.CICardDiscard.Clear();
    for (int index = 0; index < list.Count; ++index)
    {
      CardItem cardItem = new CardItem();
      string key = list[index];
      if (matchManager.cardGos.ContainsKey(key))
      {
        cardItem = matchManager.cardGos[key].GetComponent<CardItem>();
      }
      else
      {
        foreach (Transform transform in matchManager.discardSelector.cardContainer)
        {
          if ((bool) (UnityEngine.Object) transform.GetComponent<CardItem>() && transform.GetComponent<CardItem>().CardData.InternalId == key)
          {
            cardItem = transform.GetComponent<CardItem>();
            break;
          }
        }
      }
      matchManager.CICardDiscard.Add(cardItem);
    }
    matchManager.discardSelector.TurnOff();
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("**************************", "net");
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("WaitingSyncro NET_SADC_" + _randomIndex.ToString(), "net");
    yield return (object) Globals.Instance.WaitForSeconds(0.1f);
    if (NetworkManager.Instance.IsMaster())
    {
      if (matchManager.coroutineSyncDiscard != null)
        matchManager.StopCoroutine(matchManager.coroutineSyncDiscard);
      matchManager.coroutineSyncDiscard = matchManager.StartCoroutine(matchManager.ReloadCombatCo("NET_SADC_" + _randomIndex.ToString()));
      while (!NetworkManager.Instance.AllPlayersReady("NET_SADC_" + _randomIndex.ToString()))
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      if (matchManager.coroutineSyncDiscard != null)
        matchManager.StopCoroutine(matchManager.coroutineSyncDiscard);
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("Game ready, Everybody checked NET_SADC_" + _randomIndex.ToString(), "net");
      NetworkManager.Instance.PlayersNetworkContinue("NET_SADC_" + _randomIndex.ToString());
      yield return (object) Globals.Instance.WaitForSeconds(0.2f);
    }
    else
    {
      NetworkManager.Instance.SetWaitingSyncro("NET_SADC_" + _randomIndex.ToString(), true);
      NetworkManager.Instance.SetStatusReady("NET_SADC_" + _randomIndex.ToString());
      while (NetworkManager.Instance.WaitingSyncro["NET_SADC_" + _randomIndex.ToString()])
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("NET_SADC_" + _randomIndex.ToString() + ", we can continue!", "net");
    }
    matchManager.waitingForDiscardAssignment = false;
  }

  public void AssignDiscardAction()
  {
    if (!GameManager.Instance.IsMultiplayer())
    {
      this.waitingForDiscardAssignment = false;
    }
    else
    {
      List<string> stringList = new List<string>();
      for (int index = 0; index < this.CICardDiscard.Count; ++index)
      {
        if ((UnityEngine.Object) this.CICardDiscard[index] != (UnityEngine.Object) null && (UnityEngine.Object) this.CICardDiscard[index].CardData != (UnityEngine.Object) null)
        {
          stringList.Add(this.CICardDiscard[index].CardData.InternalId);
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD(this.CICardDiscard[index].CardData.InternalId);
        }
      }
      this.photonView.RPC("NET_ShareArrDiscardCard", RpcTarget.All, (object) JsonHelper.ToJson<string>(stringList.ToArray()), (object) this.randomIndex);
    }
    EventSystem.current.SetSelectedGameObject((GameObject) null);
  }

  public void SelectCardToDiscard(CardItem CI)
  {
    if ((UnityEngine.Object) this.cardActive == (UnityEngine.Object) null || (UnityEngine.Object) CI == (UnityEngine.Object) null || !this.waitingForDiscardAssignment && !this.waitingForLookDiscardWindow)
      return;
    bool iconSkull = false;
    if (this.cardActive.DiscardCardPlace == Enums.CardPlace.Discard || this.cardActive.DiscardCardPlace == Enums.CardPlace.Vanish)
      iconSkull = true;
    if (!this.CICardDiscard.Contains(CI))
    {
      this.CICardDiscard.Add(CI);
      CI.EnableDisableDiscardAction(true, iconSkull);
    }
    else
    {
      this.CICardDiscard.Remove(CI);
      CI.EnableDisableDiscardAction(false, iconSkull);
    }
    if (GameManager.Instance.IsMultiplayer() && this.IsYourTurnForAddDiscard())
      this.photonView.RPC("NET_SelectCardToDiscard", RpcTarget.Others, (object) CI.transform.gameObject.name);
    if (this.CICardDiscard.Count > this.GlobalDiscardCardsNum)
    {
      this.CICardDiscard[0].EnableDisableDiscardAction(false, iconSkull);
      this.CICardDiscard.RemoveAt(0);
    }
    this.discardSelector.TextInstructions();
  }

  [PunRPC]
  private void NET_SelectCardToDiscard(string goName)
  {
    CardItem CI = (CardItem) null;
    if (this.cardGos.ContainsKey(goName))
    {
      CI = this.cardGos[goName].GetComponent<CardItem>();
    }
    else
    {
      GameObject gameObject = GameObject.Find(goName);
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        CI = gameObject.transform.GetComponent<CardItem>();
    }
    if (!((UnityEngine.Object) CI != (UnityEngine.Object) null))
      return;
    this.SelectCardToDiscard(CI);
  }

  [PunRPC]
  private void NET_ShareArrAddCard(string _arrToAddCard, int _randomIndex)
  {
    this.SetRandomIndex(_randomIndex);
    this.StartCoroutine(this.NET_ShareArrAddCardCo(_arrToAddCard, _randomIndex));
  }

  private IEnumerator NET_ShareArrAddCardCo(string _arrToAddCard, int _randomIndex)
  {
    MatchManager matchManager = this;
    List<string> list = ((IEnumerable<string>) JsonHelper.FromJson<string>(_arrToAddCard)).ToList<string>();
    matchManager.CICardAddcard.Clear();
    for (int index = 0; index < list.Count; ++index)
    {
      string str = list[index];
      CardItem cardItem = !matchManager.cardGos.ContainsKey(str) ? GameObject.Find(str).transform.GetComponent<CardItem>() : matchManager.cardGos[str].GetComponent<CardItem>();
      matchManager.CICardAddcard.Add(cardItem);
    }
    matchManager.addcardSelector.TurnOff();
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("**************************", "net");
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("WaitingSyncro NET_SAAC_" + _randomIndex.ToString(), "net");
    yield return (object) Globals.Instance.WaitForSeconds(0.1f);
    if (NetworkManager.Instance.IsMaster())
    {
      if (matchManager.coroutineSyncAddcard != null)
        matchManager.StopCoroutine(matchManager.coroutineSyncAddcard);
      matchManager.coroutineSyncAddcard = matchManager.StartCoroutine(matchManager.ReloadCombatCo("NET_SAAC_" + _randomIndex.ToString()));
      while (!NetworkManager.Instance.AllPlayersReady("NET_SAAC_" + _randomIndex.ToString()))
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      if (matchManager.coroutineSyncAddcard != null)
        matchManager.StopCoroutine(matchManager.coroutineSyncAddcard);
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("Game ready, Everybody checked NET_SAAC_" + _randomIndex.ToString(), "net");
      NetworkManager.Instance.PlayersNetworkContinue("NET_SAAC_" + _randomIndex.ToString());
      yield return (object) Globals.Instance.WaitForSeconds(0.2f);
    }
    else
    {
      NetworkManager.Instance.SetWaitingSyncro("NET_SAAC_" + _randomIndex.ToString(), true);
      NetworkManager.Instance.SetStatusReady("NET_SAAC_" + _randomIndex.ToString());
      while (NetworkManager.Instance.WaitingSyncro["NET_SAAC_" + _randomIndex.ToString()])
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("NET_SAAC_" + _randomIndex.ToString() + ", we can continue!", "net");
    }
    matchManager.waitingForAddcardAssignment = false;
  }

  public void AssignAddcardAction()
  {
    if (!GameManager.Instance.IsMultiplayer())
    {
      this.waitingForAddcardAssignment = false;
    }
    else
    {
      List<string> stringList = new List<string>();
      for (int index = 0; index < this.CICardAddcard.Count; ++index)
      {
        if ((UnityEngine.Object) this.CICardAddcard[index] != (UnityEngine.Object) null && (UnityEngine.Object) this.CICardAddcard[index].CardData != (UnityEngine.Object) null)
          stringList.Add(this.CICardAddcard[index].CardData.InternalId);
      }
      this.photonView.RPC("NET_ShareArrAddCard", RpcTarget.All, (object) JsonHelper.ToJson<string>(stringList.ToArray()), (object) this.randomIndex);
    }
    EventSystem.current.SetSelectedGameObject((GameObject) null);
  }

  public void SelectCardToAddcard(CardItem CI)
  {
    if ((UnityEngine.Object) CI == (UnityEngine.Object) null || !this.waitingForAddcardAssignment)
      return;
    if (!this.CICardAddcard.Contains(CI))
    {
      this.CICardAddcard.Add(CI);
      CI.EnableDisableAddcardAction(true);
    }
    else
    {
      this.CICardAddcard.Remove(CI);
      CI.EnableDisableAddcardAction(false);
    }
    if (GameManager.Instance.IsMultiplayer() && this.IsYourTurn())
      this.photonView.RPC("NET_SelectCardToAddcard", RpcTarget.Others, (object) CI.transform.gameObject.name);
    if (this.CICardAddcard.Count > this.GlobalAddcardCardsNum)
    {
      this.CICardAddcard[0].EnableDisableAddcardAction(false);
      this.CICardAddcard.RemoveAt(0);
    }
    this.addcardSelector.TextInstructions();
  }

  [PunRPC]
  private void NET_SelectCardToAddcard(string goName)
  {
    this.SelectCardToAddcard(!this.cardGos.ContainsKey(goName) ? GameObject.Find(goName).transform.GetComponent<CardItem>() : this.cardGos[goName].GetComponent<CardItem>());
  }

  public int CardsLeftForDiscard()
  {
    return this.GlobalDiscardCardsNum - this.CICardDiscard.Count<CardItem>();
  }

  public int CardsLeftForAddcard()
  {
    return this.GlobalAddcardCardsNum - this.CICardAddcard.Count<CardItem>();
  }

  public void WarningMultiplayerIfNotActive()
  {
    if (!GameManager.Instance.IsMultiplayer() || this.IsYourTurn() || !this.IsYourTurnForAddDiscard())
      return;
    GameManager.Instance.PlayLibraryAudio("yourturn3", 0.5f);
  }

  public string CreateCardInDictionary(string _id, string _auxString = "", bool _overwriteId = false)
  {
    string cardInDictionary1 = _id;
    string id = _id.Split('_', StringSplitOptions.None)[0];
    CardData cardData = Globals.Instance.GetCardData(id);
    if ((UnityEngine.Object) cardData == (UnityEngine.Object) null)
      return "";
    if (_overwriteId && !this.cardDictionary.ContainsKey(_id))
    {
      Debug.Log((object) ("ADD CARD TO DICT -> " + cardInDictionary1));
      this.cardDictionary.Add(cardInDictionary1, cardData);
      this.cardDictionary[cardInDictionary1].InitClone(cardInDictionary1);
      return cardInDictionary1;
    }
    int num = 0;
    bool flag = false;
    string cardInDictionary2 = "";
    StringBuilder stringBuilder = new StringBuilder();
    while (!flag)
    {
      stringBuilder.Clear();
      stringBuilder.Append(id.ToLower());
      stringBuilder.Append("_");
      stringBuilder.Append(_auxString);
      stringBuilder.Append(num);
      cardInDictionary2 = stringBuilder.ToString();
      if (this.cardDictionary.ContainsKey(cardInDictionary2))
      {
        ++num;
      }
      else
      {
        this.cardDictionary.Add(cardInDictionary2, cardData);
        flag = true;
      }
    }
    this.cardDictionary[cardInDictionary2].InitClone(cardInDictionary2);
    return cardInDictionary2;
  }

  public void ModifyCardInDictionary(string id, CardData cardData, string overrideId = "")
  {
    id = id.ToLower();
    if (!this.cardDictionary.ContainsKey(id))
      return;
    this.cardDictionary[id] = UnityEngine.Object.Instantiate<CardData>(cardData);
    this.cardDictionary[id].InternalId = this.cardDictionary[id].Id = id;
    if (this.cardItemTable != null)
    {
      for (int index = 0; index < this.cardItemTable.Count; ++index)
      {
        if ((UnityEngine.Object) this.cardItemTable[index] != (UnityEngine.Object) null && this.cardItemTable[index].InternalId == id)
          this.cardItemTable[index].DrawEnergyCost();
      }
    }
    if (string.IsNullOrEmpty(overrideId))
      return;
    this.cardDictionary[id].Id = overrideId;
    this.cardDictionary[id].InternalId = this.cardDictionary[id].Id = overrideId;
    CardData cardData1;
    if (!this.cardDictionary.TryGetValue(id, out cardData1))
      return;
    this.cardDictionary[overrideId] = cardData1;
    this.cardDictionary.Remove(id);
  }

  public CardData DuplicateCardData(CardData _cardDestine, CardData _cardSource)
  {
    if ((UnityEngine.Object) _cardSource != (UnityEngine.Object) null && (UnityEngine.Object) _cardDestine != (UnityEngine.Object) null)
    {
      _cardDestine.Vanish = _cardSource.Vanish;
      _cardDestine.EnergyReductionPermanent = _cardSource.EnergyReductionPermanent;
      _cardDestine.EnergyReductionTemporal = _cardSource.EnergyReductionTemporal;
      _cardDestine.EnergyReductionToZeroPermanent = _cardSource.EnergyReductionToZeroPermanent;
      _cardDestine.EnergyReductionToZeroTemporal = _cardSource.EnergyReductionToZeroTemporal;
    }
    return _cardDestine;
  }

  private void RemoveCardFromDictionary(string id)
  {
    id = id.ToLower();
    if (this.cardDictionary == null || !this.cardDictionary.ContainsKey(id))
      return;
    this.cardDictionary.Remove(id);
  }

  private string CardNamesForSyncCode()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this.CardDictionaryKeys());
    for (int index1 = 0; index1 < 4; ++index1)
    {
      if (this.HeroDeck[index1] != null)
      {
        for (int index2 = 0; index2 < this.HeroDeck[index1].Count; ++index2)
          stringBuilder.Append(this.HeroDeck[index1][index2]);
      }
      stringBuilder.Append("*0*");
      if (this.HeroDeckDiscard[index1] != null)
      {
        for (int index3 = 0; index3 < this.HeroDeckDiscard[index1].Count; ++index3)
          stringBuilder.Append(this.HeroDeckDiscard[index1][index3]);
      }
      stringBuilder.Append("*1*");
      if (this.HeroDeckVanish[index1] != null)
      {
        for (int index4 = 0; index4 < this.HeroDeckVanish[index1].Count; ++index4)
          stringBuilder.Append(this.HeroDeckVanish[index1][index4]);
      }
      stringBuilder.Append("*2*");
    }
    return stringBuilder.ToString();
  }

  private string CardDictionaryKeys()
  {
    StringBuilder stringBuilder = new StringBuilder();
    foreach (KeyValuePair<string, CardData> card in this.cardDictionary)
    {
      stringBuilder.Append(card.Key);
      stringBuilder.Append("@");
    }
    return stringBuilder.ToString();
  }

  private void BackupCardDictionary()
  {
    if (this.cardDictionaryBackup != null)
    {
      foreach (KeyValuePair<string, CardData> keyValuePair in this.cardDictionaryBackup)
        UnityEngine.Object.Destroy((UnityEngine.Object) keyValuePair.Value);
      this.cardDictionaryBackup.Clear();
    }
    else
      this.cardDictionaryBackup = new Dictionary<string, CardData>();
    foreach (KeyValuePair<string, CardData> card in this.cardDictionary)
      this.cardDictionaryBackup.Add(card.Key, UnityEngine.Object.Instantiate<CardData>(card.Value));
    this.backingDictionary = false;
  }

  private void RestoreCardDictionary()
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("[RestoreCardDictionary] begin", "general");
    if (this.cardDictionaryBackup != null && this.cardDictionaryBackup.Count > 0)
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("[RestoreCardDictionary] copied", "general");
      this.cardDictionary = new Dictionary<string, CardData>();
      foreach (KeyValuePair<string, CardData> keyValuePair in this.cardDictionaryBackup)
        this.cardDictionary.Add(keyValuePair.Key, UnityEngine.Object.Instantiate<CardData>(keyValuePair.Value));
    }
    this.backingDictionary = false;
  }

  public void CardCreatorAction(
    int numCards,
    string theCard,
    bool createCard,
    Enums.CardPlace where,
    bool fromNet)
  {
    if (GameManager.Instance.IsMultiplayer())
    {
      if (NetworkManager.Instance.IsMaster())
        this.photonView.RPC("NET_CardCreatorAction", RpcTarget.Others, (object) numCards, (object) theCard, (object) createCard, (object) where, (object) true);
      else if (!fromNet)
        return;
    }
    this.GenerateNewCard(numCards, theCard, createCard, where);
  }

  [PunRPC]
  private void NET_CardCreatorAction(
    int numCards,
    string theCard,
    bool createCard,
    Enums.CardPlace where,
    bool fromNet)
  {
    this.CardCreatorAction(numCards, theCard, createCard, where, fromNet);
  }

  public void GenerateNewCard(
    int numCards,
    string theCard,
    bool createCard,
    Enums.CardPlace where,
    CardData cardDataForModification = null,
    CardData copyDataFromThisCard = null,
    int heroIndex = -1,
    bool isHero = true,
    int indexForBatch = 0,
    CopyConfig copyConfig = null)
  {
    if (this.MatchIsOver)
      return;
    if (where == Enums.CardPlace.Hand)
    {
      int num = 10 - this.CountHeroHand();
      if (num < numCards)
        numCards = num;
      if (numCards <= 0)
        return;
    }
    this.SetGameBusy(true);
    this.StartCoroutine(this.GenerateNewCardCo(numCards, theCard, createCard, where, cardDataForModification, copyDataFromThisCard, heroIndex, isHero, indexForBatch, copyConfig));
  }

  private IEnumerator GenerateNewCardCo(
    int numCards,
    string _theCard,
    bool createCard,
    Enums.CardPlace where,
    CardData cardDataForModification,
    CardData copyDataFromThisCard,
    int heroIndex,
    bool isHero,
    int indexForBatch,
    CopyConfig copyConfig = null)
  {
    MatchManager matchManager1 = this;
    List<string> theCards = new List<string>();
    for (int index = 0; index < numCards; ++index)
    {
      string id1 = !createCard ? _theCard : matchManager1.CreateCardInDictionary(_theCard);
      if ((UnityEngine.Object) copyDataFromThisCard != (UnityEngine.Object) null)
        matchManager1.ModifyCardInDictionary(id1, copyDataFromThisCard);
      if ((UnityEngine.Object) cardDataForModification != (UnityEngine.Object) null)
      {
        CardData cardData = matchManager1.GetCardData(id1);
        if (cardData.SpecialCardEnum == SpecialCardEnum.NightmareEchoTemplate)
        {
          string id2 = cardData.Id.Split('_', StringSplitOptions.None)[0] + "-" + cardDataForModification.Id;
          cardData = matchManager1.GetCardData(id2);
          id1 = id2;
        }
        matchManager1.AddCardModificationsForCard(cardDataForModification, cardData, copyConfig);
      }
      theCards.Add(id1);
    }
    if (matchManager1.heroActive == -1 && matchManager1.npcActive == -1)
      yield return (object) Globals.Instance.WaitForSeconds((float) indexForBatch * 0.1f);
    else
      yield return (object) null;
    ++matchManager1.generatedCardTimes;
    if (theCards.Count > 0)
    {
      matchManager1.SetEventDirect("GenerateNewCard" + theCards[0], false, true);
      int _heroActive = heroIndex;
      if (_heroActive == -1)
        _heroActive = matchManager1.heroActive;
      List<CardItem> CIG = new List<CardItem>();
      int tempTransformChildCount = matchManager1.tempTransform.childCount;
      int i;
      for (i = 0; i < theCards.Count; ++i)
      {
        string id = theCards[i];
        ++tempTransformChildCount;
        GameObject gameObject = matchManager1.theNPC == null ? UnityEngine.Object.Instantiate<GameObject>(GameManager.Instance.CardPrefab, new Vector3(-0.35f, 0.35f, 0.0f) + new Vector3((float) tempTransformChildCount * 0.25f, (float) tempTransformChildCount * -0.15f, 0.0f), Quaternion.identity, matchManager1.tempTransform) : UnityEngine.Object.Instantiate<GameObject>(GameManager.Instance.CardPrefab, new Vector3(-0.35f, 0.35f, 0.0f) + new Vector3((float) tempTransformChildCount * 0.25f, (float) tempTransformChildCount * -0.15f, 0.0f), Quaternion.identity, matchManager1.tempTransform);
        gameObject.name = id;
        CardItem component = gameObject.GetComponent<CardItem>();
        if (matchManager1.theHero != null)
          component.SetCard(id, false, matchManager1.theHero);
        else if (matchManager1.theNPC != null)
          component.SetCard(id, false, _theNPC: matchManager1.theNPC);
        else
          component.SetCard(id, false);
        component.TopLayeringOrder("Default", 20000 + 60 * tempTransformChildCount + 100 * indexForBatch);
        component.DisableCollider();
        CIG.Add(component);
        if (matchManager1.theNPC != null)
          component.SetDestinationScaleRotation(new Vector3(-0.35f, 0.35f, 0.0f) + new Vector3((float) tempTransformChildCount * 0.25f, (float) tempTransformChildCount * -0.15f, 0.0f), 1.4f, Quaternion.Euler(0.0f, 0.0f, 0.0f));
        else
          component.SetDestinationScaleRotation(new Vector3(-0.35f, 0.35f, 0.0f) + new Vector3((float) tempTransformChildCount * 0.25f, (float) tempTransformChildCount * -0.15f, 0.0f), 1.4f, Quaternion.Euler(0.0f, 0.0f, 0.0f));
        component.discard = true;
        if (where != Enums.CardPlace.Hand)
        {
          component.HideRarityParticles();
          component.HideCardIconParticles();
        }
        yield return (object) Globals.Instance.WaitForSeconds(0.15f);
      }
      if (CIG != null && CIG.Count == 1 && (UnityEngine.Object) CIG[0] != (UnityEngine.Object) null && CIG[0].CardData.CardType == Enums.CardType.Corruption)
      {
        yield return (object) Globals.Instance.WaitForSeconds(0.5f);
        GameManager.Instance.GenerateParticleTrail(2, CIG[0].transform.position, matchManager1.iconCorruption.transform.position);
        UnityEngine.Object.Destroy((UnityEngine.Object) CIG[0].gameObject);
        yield return (object) Globals.Instance.WaitForSeconds(0.1f);
        matchManager1.iconCorruption.transform.gameObject.SetActive(true);
        --matchManager1.generatedCardTimes;
        if (matchManager1.generatedCardTimes < 0)
          matchManager1.generatedCardTimes = 0;
        matchManager1.SetEventDirect("GenerateNewCard" + theCards[0], false);
        yield break;
      }
      else
      {
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        int num;
        for (i = 0; i < CIG.Count; num = i++)
        {
          switch (where)
          {
            case Enums.CardPlace.Discard:
              matchManager1.HeroDeckDiscard[_heroActive].Add(CIG[i].InternalId);
              if (matchManager1.theNPC != null & isHero)
              {
                yield return (object) Globals.Instance.WaitForSeconds(0.2f);
                if ((UnityEngine.Object) CIG[i] != (UnityEngine.Object) null)
                {
                  CIG[i].destroyAtLocation = true;
                  GameManager.Instance.GenerateParticleTrail(2, CIG[i].transform.position, matchManager1.TeamHero[_heroActive].HeroItem.transform.position + new Vector3(0.0f, 2f, 0.0f));
                  UnityEngine.Object.Destroy((UnityEngine.Object) CIG[i].gameObject);
                }
              }
              else
                CIG[i].DiscardCard(false);
              matchManager1.DrawDiscardPileCardNumeral();
              MatchManager matchManager2 = matchManager1;
              num = matchManager1.logDictionary.Count;
              string _key1 = "toDiscard:" + num.ToString();
              string internalId1 = CIG[i].InternalId;
              Hero _theHero1 = matchManager1.TeamHero[_heroActive];
              int currentRound1 = matchManager1.currentRound;
              matchManager2.CreateLogEntry(true, _key1, internalId1, _theHero1, (NPC) null, (Hero) null, (NPC) null, currentRound1);
              break;
            case Enums.CardPlace.Hand:
              matchManager1.HeroHand[_heroActive].Add(CIG[i].name);
              CIG[i].transform.parent = matchManager1.GO_Hand.transform;
              CIG[i].active = true;
              CIG[i].discard = false;
              CIG[i].SetCard(CIG[i].name, false, matchManager1.theHero);
              matchManager1.cardItemTable.Add(CIG[i]);
              matchManager1.RepositionCards();
              MatchManager matchManager3 = matchManager1;
              num = matchManager1.logDictionary.Count;
              string _key2 = "toHand:" + num.ToString();
              string internalId2 = CIG[i].InternalId;
              Hero _theHero2 = matchManager1.TeamHero[_heroActive];
              int currentRound2 = matchManager1.currentRound;
              matchManager3.CreateLogEntry(true, _key2, internalId2, _theHero2, (NPC) null, (Hero) null, (NPC) null, currentRound2);
              break;
            case Enums.CardPlace.Cast:
              yield return (object) Globals.Instance.WaitForSeconds(0.15f);
              if ((UnityEngine.Object) CIG[i] != (UnityEngine.Object) null && matchManager1.IsThereAnyTargetForCard(CIG[i].CardData))
              {
                if (Globals.Instance.ShowDebug)
                  Functions.DebugLogGD("Set in hand card ->" + CIG[i].name + " | " + heroIndex.ToString() + " | " + matchManager1.theHero?.ToString());
                if (matchManager1.theHero != null)
                {
                  if ((UnityEngine.Object) CIG[i] != (UnityEngine.Object) null && CIG[i].CardData.CardType != Enums.CardType.Corruption)
                    matchManager1.HeroHand[_heroActive].Add(CIG[i].name);
                  if ((UnityEngine.Object) CIG[i] != (UnityEngine.Object) null)
                    CIG[i].SetCard(CIG[i].name, false, matchManager1.theHero);
                  if ((UnityEngine.Object) CIG[i] != (UnityEngine.Object) null && CIG[i].CardData.CardType != Enums.CardType.Corruption)
                  {
                    matchManager1.cardItemTable.Add(CIG[i]);
                    matchManager1.RepositionCards();
                  }
                }
                else if ((UnityEngine.Object) CIG[i] != (UnityEngine.Object) null)
                  CIG[i].SetCard(CIG[i].name, false, _theNPC: matchManager1.theNPC);
                if ((UnityEngine.Object) CIG[i] != (UnityEngine.Object) null)
                {
                  CIG[i].DrawBorder("");
                  CIG[i].SetDestinationScaleRotation(CIG[i].transform.localPosition, 1.4f, Quaternion.Euler(0.0f, 0.0f, 0.0f));
                }
                if ((UnityEngine.Object) CIG[i] != (UnityEngine.Object) null)
                {
                  if (matchManager1.theHero != null)
                  {
                    matchManager1.StartCoroutine(matchManager1.CastCard(CIG[i], _propagate: false));
                    break;
                  }
                  if (matchManager1.theNPC != null)
                  {
                    matchManager1.StartCoroutine(matchManager1.CastCard(CIG[i], _propagate: false));
                    break;
                  }
                  break;
                }
                break;
              }
              if ((UnityEngine.Object) CIG[i] != (UnityEngine.Object) null)
              {
                CIG[i].DiscardCard(false, Enums.CardPlace.Vanish);
                if (matchManager1.gameStatus == "BeginTurn")
                {
                  matchManager1.gameStatus = "CastCardEnd";
                  if (Globals.Instance.ShowDebug)
                  {
                    Functions.DebugLogGD(matchManager1.gameStatus, "gamestatus");
                    break;
                  }
                  break;
                }
                break;
              }
              break;
            default:
              string internalId3 = CIG[i].InternalId;
              if (where == Enums.CardPlace.TopDeck)
              {
                if (isHero)
                {
                  if (_heroActive > -1)
                  {
                    matchManager1.HeroDeck[_heroActive].Insert(0, internalId3);
                    MatchManager matchManager4 = matchManager1;
                    num = matchManager1.logDictionary.Count;
                    string _key3 = "toTopDeck:" + num.ToString();
                    string _cardId = internalId3;
                    Hero _theHero3 = matchManager1.TeamHero[_heroActive];
                    int currentRound3 = matchManager1.currentRound;
                    matchManager4.CreateLogEntry(true, _key3, _cardId, _theHero3, (NPC) null, (Hero) null, (NPC) null, currentRound3);
                  }
                  else
                  {
                    matchManager1.HeroDeck[matchManager1.theHero.HeroIndex].Insert(0, internalId3);
                    MatchManager matchManager5 = matchManager1;
                    num = matchManager1.logDictionary.Count;
                    string _key4 = "toTopDeck:" + num.ToString();
                    string _cardId = internalId3;
                    Hero _theHero4 = matchManager1.TeamHero[matchManager1.theHero.HeroIndex];
                    int currentRound4 = matchManager1.currentRound;
                    matchManager5.CreateLogEntry(true, _key4, _cardId, _theHero4, (NPC) null, (Hero) null, (NPC) null, currentRound4);
                  }
                }
                else
                {
                  matchManager1.NPCDeck[heroIndex].Insert(0, internalId3);
                  MatchManager matchManager6 = matchManager1;
                  num = matchManager1.logDictionary.Count;
                  string _key5 = "toTopDeck:" + num.ToString();
                  string _cardId = internalId3;
                  NPC _theNPC = matchManager1.TeamNPC[heroIndex];
                  int currentRound5 = matchManager1.currentRound;
                  matchManager6.CreateLogEntry(true, _key5, _cardId, (Hero) null, _theNPC, (Hero) null, (NPC) null, currentRound5);
                }
              }
              else if (where == Enums.CardPlace.BottomDeck)
              {
                if (_heroActive > -1)
                {
                  matchManager1.HeroDeck[_heroActive].Insert(matchManager1.HeroDeck[_heroActive].Count, internalId3);
                  MatchManager matchManager7 = matchManager1;
                  num = matchManager1.logDictionary.Count;
                  string _key6 = "toBottomDeck:" + num.ToString();
                  string _cardId = internalId3;
                  Hero _theHero5 = matchManager1.TeamHero[_heroActive];
                  int currentRound6 = matchManager1.currentRound;
                  matchManager7.CreateLogEntry(true, _key6, _cardId, _theHero5, (NPC) null, (Hero) null, (NPC) null, currentRound6);
                }
                else
                {
                  matchManager1.HeroDeck[matchManager1.theHero.HeroIndex].Insert(matchManager1.HeroDeck[matchManager1.theHero.HeroIndex].Count, internalId3);
                  MatchManager matchManager8 = matchManager1;
                  num = matchManager1.logDictionary.Count;
                  string _key7 = "toBottomDeck:" + num.ToString();
                  string _cardId = internalId3;
                  Hero _theHero6 = matchManager1.TeamHero[matchManager1.theHero.HeroIndex];
                  int currentRound7 = matchManager1.currentRound;
                  matchManager8.CreateLogEntry(true, _key7, _cardId, _theHero6, (NPC) null, (Hero) null, (NPC) null, currentRound7);
                }
              }
              else if (where != Enums.CardPlace.Vanish)
              {
                if (_heroActive > -1)
                {
                  int randomIntRange = matchManager1.GetRandomIntRange(0, matchManager1.HeroDeck[_heroActive].Count, "deck");
                  matchManager1.HeroDeck[_heroActive].Insert(randomIntRange, internalId3);
                  MatchManager matchManager9 = matchManager1;
                  num = matchManager1.logDictionary.Count;
                  string _key8 = "toRandomDeck:" + num.ToString();
                  string _cardId = internalId3;
                  Hero _theHero7 = matchManager1.TeamHero[_heroActive];
                  int currentRound8 = matchManager1.currentRound;
                  matchManager9.CreateLogEntry(true, _key8, _cardId, _theHero7, (NPC) null, (Hero) null, (NPC) null, currentRound8);
                }
                else
                {
                  int randomIntRange = matchManager1.GetRandomIntRange(0, matchManager1.HeroDeck[matchManager1.theHero.HeroIndex].Count, "deck");
                  matchManager1.HeroDeck[matchManager1.theHero.HeroIndex].Insert(randomIntRange, internalId3);
                  MatchManager matchManager10 = matchManager1;
                  num = matchManager1.logDictionary.Count;
                  string _key9 = "toRandomDeck:" + num.ToString();
                  string _cardId = internalId3;
                  Hero _theHero8 = matchManager1.TeamHero[matchManager1.theHero.HeroIndex];
                  int currentRound9 = matchManager1.currentRound;
                  matchManager10.CreateLogEntry(true, _key9, _cardId, _theHero8, (NPC) null, (Hero) null, (NPC) null, currentRound9);
                }
              }
              yield return (object) Globals.Instance.WaitForSeconds(0.15f);
              if (isHero && _heroActive == matchManager1.heroActive)
                matchManager1.DrawDeckPile(matchManager1.CountHeroDeck(_heroActive) + 1);
              if (where != Enums.CardPlace.RandomDeck)
                CIG[i].CardData.Visible = true;
              CIG[i].discard = true;
              if (isHero)
              {
                if ((UnityEngine.Object) matchManager1.cardActive != (UnityEngine.Object) null && matchManager1.cardActive.TargetSide == Enums.CardTargetSide.Friend)
                {
                  yield return (object) Globals.Instance.WaitForSeconds(0.02f);
                  if ((UnityEngine.Object) CIG[i] != (UnityEngine.Object) null)
                  {
                    GameManager.Instance.GenerateParticleTrail(2, CIG[i].transform.position, matchManager1.TeamHero[_heroActive].HeroItem.transform.position + new Vector3(0.0f, 2f, 0.0f));
                    UnityEngine.Object.Destroy((UnityEngine.Object) CIG[i].gameObject);
                    break;
                  }
                  break;
                }
                if (_heroActive > -1)
                {
                  yield return (object) Globals.Instance.WaitForSeconds(0.02f);
                  if ((UnityEngine.Object) CIG[i] != (UnityEngine.Object) null)
                  {
                    GameManager.Instance.GenerateParticleTrail(2, CIG[i].transform.position, matchManager1.TeamHero[_heroActive].HeroItem.transform.position + new Vector3(0.0f, 2f, 0.0f));
                    UnityEngine.Object.Destroy((UnityEngine.Object) CIG[i].gameObject);
                    break;
                  }
                  break;
                }
                if (matchManager1.theHero != null)
                {
                  yield return (object) Globals.Instance.WaitForSeconds(0.02f);
                  if ((UnityEngine.Object) CIG[i] != (UnityEngine.Object) null)
                  {
                    GameManager.Instance.GenerateParticleTrail(2, CIG[i].transform.position, matchManager1.GO_DeckPile.transform.position);
                    UnityEngine.Object.Destroy((UnityEngine.Object) CIG[i].gameObject);
                    break;
                  }
                  break;
                }
                break;
              }
              yield return (object) Globals.Instance.WaitForSeconds(0.02f);
              if ((UnityEngine.Object) CIG[i] != (UnityEngine.Object) null)
              {
                CIG[i].destroyAtLocation = true;
                GameManager.Instance.GenerateParticleTrail(2, CIG[i].transform.position, matchManager1.TeamNPC[_heroActive].NPCItem.transform.position + new Vector3(0.0f, 2f, 0.0f));
                UnityEngine.Object.Destroy((UnityEngine.Object) CIG[i].gameObject);
                break;
              }
              break;
          }
        }
        CIG = (List<CardItem>) null;
      }
    }
    yield return (object) null;
    if (matchManager1.theHero != null)
      matchManager1.SetGameBusy(false);
    --matchManager1.generatedCardTimes;
    if (matchManager1.generatedCardTimes < 0)
      matchManager1.generatedCardTimes = 0;
    matchManager1.SetEventDirect("GenerateNewCard" + theCards[0], false);
    foreach (string auxString in theCards)
      matchManager1.theHero?.SetEvent(Enums.EventActivation.DrawCard, auxString: auxString);
  }

  private void MoveCardTo(int numCards, string theCard, Enums.CardPlace cardPlace)
  {
    this.StartCoroutine(this.MoveCardToCo(numCards, theCard, cardPlace));
  }

  private IEnumerator MoveCardToCo(int numCards, string theCard, Enums.CardPlace cardPlace)
  {
    List<CardItem> CIG = new List<CardItem>();
    int i;
    for (i = 0; i < numCards; ++i)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GameManager.Instance.CardPrefab, Vector3.zero + new Vector3((float) i * 0.1f, (float) i * -0.1f, 0.0f), Quaternion.identity, this.GO_DeckPile.transform);
      CardItem component = gameObject.GetComponent<CardItem>();
      gameObject.name = theCard;
      component.SetCard(theCard, false, this.theHero);
      component.DefaultElementsLayeringOrder(50 * i);
      CIG.Add(component);
      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
    }
    yield return (object) Globals.Instance.WaitForSeconds(0.2f);
    for (i = CIG.Count - 1; i >= 0; --i)
    {
      switch (cardPlace)
      {
        case Enums.CardPlace.Discard:
          this.HeroDeckDiscard[this.heroActive].Insert(0, theCard);
          this.DrawDiscardPileCardNumeral();
          CIG[i].DiscardCard(false);
          goto label_13;
        case Enums.CardPlace.TopDeck:
          this.HeroDeck[this.heroActive].Insert(0, theCard);
          break;
        case Enums.CardPlace.BottomDeck:
          this.HeroDeck[this.heroActive].Insert(this.HeroDeck[this.heroActive].Count, theCard);
          break;
        default:
          this.HeroDeck[this.heroActive].Insert(this.GetRandomIntRange(0, this.HeroDeck[this.heroActive].Count, "deck"), theCard);
          break;
      }
      this.DrawDeckPile(this.CountHeroDeck() + 1);
      if (cardPlace != Enums.CardPlace.RandomDeck)
        CIG[i].CardData.Visible = true;
      CIG[i].discard = true;
      CIG[i].MoveCardToDeckPile();
label_13:
      yield return (object) Globals.Instance.WaitForSeconds(0.5f);
    }
  }

  private IEnumerator DealCards()
  {
    MatchManager matchManager = this;
    if (matchManager.MatchIsOver)
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("MOVEDECKSOUT Broken by finish game", "trace");
    }
    else
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD(nameof (DealCards), "trace");
      int eventExaust = 0;
      while (matchManager.eventList.Count > 0)
      {
        if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
        {
          matchManager.eventListDbg = "";
          for (int index = 0; index < matchManager.eventList.Count; ++index)
            matchManager.eventListDbg = matchManager.eventListDbg + matchManager.eventList[index] + " || ";
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[DealCards] Waiting For Eventlist to clean", "trace");
        }
        ++eventExaust;
        if (eventExaust > 300)
        {
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[DealCards] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
          matchManager.ClearEventList();
          break;
        }
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      }
      yield return (object) Globals.Instance.WaitForSeconds(0.05f);
      if (GameManager.Instance.IsMultiplayer())
      {
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("**************************", "net");
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("WaitingSyncro dealcards", "net");
        if (NetworkManager.Instance.IsMaster())
        {
          if (matchManager.coroutineSyncDealCards != null)
            matchManager.StopCoroutine(matchManager.coroutineSyncDealCards);
          matchManager.coroutineSyncDealCards = matchManager.StartCoroutine(matchManager.ReloadCombatCo("dealcards"));
          while (!NetworkManager.Instance.AllPlayersReady("dealcards"))
            yield return (object) Globals.Instance.WaitForSeconds(0.01f);
          if (matchManager.coroutineSyncDealCards != null)
            matchManager.StopCoroutine(matchManager.coroutineSyncDealCards);
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("Game ready, Everybody checked dealcards", "net");
          matchManager.SetRandomIndex(matchManager.randomIndex);
          NetworkManager.Instance.PlayersNetworkContinue("dealcards", matchManager.randomIndex.ToString());
        }
        else
        {
          NetworkManager.Instance.SetWaitingSyncro("dealcards", true);
          NetworkManager.Instance.SetStatusReady("dealcards");
          while (NetworkManager.Instance.WaitingSyncro["dealcards"])
            yield return (object) Globals.Instance.WaitForSeconds(0.01f);
          if (NetworkManager.Instance.netAuxValue != "")
            matchManager.SetRandomIndex(int.Parse(NetworkManager.Instance.netAuxValue));
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("dealcards, we can continue!", "net");
        }
      }
      if (matchManager.MatchIsOver)
      {
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("MOVEDECKSOUT Broken by finish game", "trace");
      }
      else
      {
        matchManager.CreateLogEntry(true, "dealcards" + matchManager.logDictionary.Count.ToString(), "", (Hero) null, (NPC) null, (Hero) null, (NPC) null, matchManager.currentRound);
        matchManager.NewCard(matchManager.theHero.GetDrawCardsTurn() - 1, Enums.CardFrom.Deck);
        matchManager.theHero.SetEvent(Enums.EventActivation.DrawCard);
        matchManager.theHero.SetEvent(Enums.EventActivation.AfterDealCards);
        matchManager.ResetCardHoverIndex();
      }
    }
  }

  private IEnumerator DealCardsContinue()
  {
    if (this.MatchIsOver)
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("MOVEDECKSOUT Broken by finish game", "trace");
    }
    else
    {
      this.NewCard(this.theHero.GetDrawCardsTurn(), Enums.CardFrom.Deck);
      this.ResetCardHoverIndex();
      yield return (object) null;
    }
  }

  [PunRPC]
  private void NET_DealCardsContinue() => this.StartCoroutine(this.DealCardsContinue());

  [PunRPC]
  private void NET_DealCardsContinueSync(int _randomIndex)
  {
    this.SetRandomIndex(_randomIndex);
    NetworkManager.Instance.SetStatusReady("dealcards");
  }

  public void NewCard(int numCards, Enums.CardFrom fromPlace, string comingFromCardId = "")
  {
    if (this.theHero == null)
      return;
    int num = this.CountHeroDiscard() + this.CountHeroDeck();
    if (num == 0)
    {
      this.SetCardsWaitingForReset(0);
      this.isBeginTournPhase = false;
    }
    else
    {
      if (numCards > num)
        numCards = num;
      this.cardsWaitingForReset += numCards;
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("[NEW CARD] cardsWaitingForReset " + this.cardsWaitingForReset.ToString(), "trace");
      if (this.cardsWaitingForReset > 0)
      {
        this.StartCoroutine(this.DealNewCard(fromPlace, comingFromCardId));
      }
      else
      {
        if (!this.castingCardBlocked.ContainsKey(comingFromCardId))
          return;
        Debug.Log((object) ("**** CASTINGCARDBLOCKED REMOVE " + comingFromCardId));
        this.castingCardBlocked[comingFromCardId] = false;
      }
    }
  }

  private IEnumerator DealNewCard(Enums.CardFrom fromPlace, string comingFromCardId = "")
  {
    MatchManager matchManager = this;
    string codeGen;
    int exaustCodeGen;
    if (!matchManager.isBeginTournPhase)
    {
      codeGen = matchManager.GenerateSyncCodeForCheckingAction() + matchManager.GenerateStatusString();
      yield return (object) Globals.Instance.WaitForSeconds(0.05f);
      while (matchManager.waitingKill)
        yield return (object) Globals.Instance.WaitForSeconds(0.05f);
      string str = matchManager.GenerateSyncCodeForCheckingAction() + matchManager.GenerateStatusString();
      exaustCodeGen = 0;
      while (codeGen != str)
      {
        codeGen = str;
        yield return (object) Globals.Instance.WaitForSeconds(0.05f);
        str = matchManager.GenerateSyncCodeForCheckingAction() + matchManager.GenerateStatusString();
        ++exaustCodeGen;
        if (exaustCodeGen > 50)
          codeGen = str;
      }
      codeGen = (string) null;
    }
    matchManager.ResetAutoEndCount();
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("DealNewCard ___ " + comingFromCardId, "general");
    CardData _cardData = (CardData) null;
    if (matchManager.cardsWaitingForReset > 0)
    {
      if (matchManager.CountHeroHand() + 1 > 10)
      {
        matchManager.SetCardsWaitingForReset(0);
        if (matchManager.isBeginTournPhase)
        {
          matchManager.BeginTurnHero();
          yield break;
        }
      }
      else if (matchManager.CountHeroDeck() == 0)
      {
        if (matchManager.CountHeroDiscard() > 0)
        {
          if (GameManager.Instance.IsMultiplayer())
          {
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("**************************", "net");
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("WaitingSyncro shuffle", "net");
            yield return (object) Globals.Instance.WaitForSeconds(0.1f);
            if (NetworkManager.Instance.IsMaster())
            {
              if (matchManager.coroutineSyncShuffle != null)
                matchManager.StopCoroutine(matchManager.coroutineSyncShuffle);
              matchManager.coroutineSyncShuffle = matchManager.StartCoroutine(matchManager.ReloadCombatCo("shuffle"));
              while (!NetworkManager.Instance.AllPlayersReady("shuffle"))
                yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              if (matchManager.coroutineSyncShuffle != null)
                matchManager.StopCoroutine(matchManager.coroutineSyncShuffle);
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("Game ready, Everybody checked shuffle", "net");
              matchManager.SetRandomIndex(matchManager.randomShuffleIndex, "shuffle");
              NetworkManager.Instance.PlayersNetworkContinue("shuffle", matchManager.randomShuffleIndex.ToString());
            }
            else
            {
              NetworkManager.Instance.SetWaitingSyncro("shuffle", true);
              NetworkManager.Instance.SetStatusReady("shuffle");
              while (NetworkManager.Instance.WaitingSyncro["shuffle"])
                yield return (object) Globals.Instance.WaitForSeconds(0.01f);
              if (NetworkManager.Instance.netAuxValue != "")
                matchManager.SetRandomIndex(int.Parse(NetworkManager.Instance.netAuxValue), "shuffle");
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("shuffle, we can continue!", "net");
            }
          }
          yield return (object) matchManager.StartCoroutine(matchManager.ResetDeck());
          yield return (object) Globals.Instance.WaitForSeconds(0.1f);
          while (matchManager.eventList.Contains("ResetDeck"))
            yield return (object) Globals.Instance.WaitForSeconds(0.01f);
          matchManager.GiveExhaust();
          yield return (object) matchManager.StartCoroutine(matchManager.DealNewCard(fromPlace, comingFromCardId));
          yield break;
        }
        else
        {
          matchManager.SetCardsWaitingForReset(0);
          if (matchManager.isBeginTournPhase)
          {
            matchManager.BeginTurnHero();
            yield break;
          }
        }
      }
      else
      {
        if (matchManager.isBeginTournPhase)
        {
          if (GameManager.Instance.IsMultiplayer())
            yield return (object) Globals.Instance.WaitForSeconds(0.07f);
          else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
            yield return (object) Globals.Instance.WaitForSeconds(0.1f);
          else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Ultrafast)
            yield return (object) Globals.Instance.WaitForSeconds(0.02f);
          else
            yield return (object) Globals.Instance.WaitForSeconds(0.05f);
        }
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("CardsWaitingForRset PRE " + matchManager.cardsWaitingForReset.ToString(), "trace");
        GameManager.Instance.PlayLibraryAudio("dealcard");
        matchManager.GetCardFromDeckToHand();
        matchManager.ResetAutoEndCount();
        matchManager.StartCoroutine(matchManager.CreateCard(matchManager.CountHeroHand() - 1, fromPlace));
        if (!matchManager.isBeginTournPhase)
        {
          if (GameManager.Instance.IsMultiplayer())
            yield return (object) Globals.Instance.WaitForSeconds(0.1f);
          else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
            yield return (object) Globals.Instance.WaitForSeconds(0.1f);
          else
            yield return (object) Globals.Instance.WaitForSeconds(0.07f);
        }
        if ((UnityEngine.Object) matchManager.cardItemTable[matchManager.cardItemTable.Count - 1] == (UnityEngine.Object) null)
        {
          yield break;
        }
        else
        {
          _cardData = matchManager.cardItemTable[matchManager.cardItemTable.Count - 1].CardData;
          if ((UnityEngine.Object) _cardData != (UnityEngine.Object) null && _cardData.AutoplayDraw)
          {
            codeGen = matchManager.gameStatus;
            CardItem CI = matchManager.cardItemTable[matchManager.cardItemTable.Count - 1];
            CI.SetDestinationScaleRotation(Vector3.zero - CI.transform.parent.transform.localPosition, 1.4f, Quaternion.Euler(0.0f, 0.0f, 0.0f));
            CI.DrawBorder("blue");
            CI.active = true;
            if (_cardData.CardClass == Enums.CardClass.Injury)
              yield return (object) Globals.Instance.WaitForSeconds(0.4f);
            else
              yield return (object) Globals.Instance.WaitForSeconds(0.3f);
            matchManager.StartCoroutine(matchManager.CastCard(CI));
            yield return (object) Globals.Instance.WaitForSeconds(0.1f);
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("Autoplay " + _cardData.InternalId, "trace");
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD(matchManager.castingCardBlocked.ContainsKey(_cardData.InternalId).ToString(), "trace");
            if (matchManager.castingCardBlocked.ContainsKey(_cardData.InternalId))
            {
              exaustCodeGen = 0;
              while (matchManager.castingCardBlocked.ContainsKey(_cardData.InternalId) && matchManager.castingCardBlocked[_cardData.InternalId])
              {
                while (matchManager.waitingDeathScreen)
                {
                  if (Globals.Instance.ShowDebug)
                    Functions.DebugLogGD("DealNewCard -  waitingDeathScreen", "trace");
                  yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                }
                bool exit = false;
                if (matchManager.theHero != null && !matchManager.theHero.Alive)
                {
                  yield return (object) Globals.Instance.WaitForSeconds(1f);
                  if (matchManager.theHero != null && !matchManager.theHero.Alive)
                    exit = true;
                }
                else if (matchManager.theNPC != null && !matchManager.theNPC.Alive)
                {
                  yield return (object) Globals.Instance.WaitForSeconds(0.1f);
                  if (matchManager.theNPC != null && !matchManager.theNPC.Alive)
                    exit = true;
                }
                if (exit)
                {
                  if (Globals.Instance.ShowDebug)
                    Functions.DebugLogGD("DealNewCard - Character died", "trace");
                  if (matchManager.theHero != null && !matchManager.theHero.Alive && matchManager.IsBeginTournPhase)
                  {
                    yield return (object) Globals.Instance.WaitForSeconds(0.5f);
                    if (matchManager.theHero != null && !matchManager.theHero.Alive && matchManager.IsBeginTournPhase)
                    {
                      if (Globals.Instance.ShowDebug)
                        Functions.DebugLogGD("DealNewCard - Goto endturn", "trace");
                      matchManager.SetCardsWaitingForReset(0);
                      if (comingFromCardId != "")
                      {
                        Debug.Log((object) ("**** CASTINGCARDBLOCKED REMOVE " + comingFromCardId));
                        matchManager.castingCardBlocked[comingFromCardId] = false;
                      }
                      matchManager.EndTurn();
                      yield break;
                    }
                  }
                }
                yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                ++exaustCodeGen;
                if (exaustCodeGen > 10000)
                {
                  Debug.Log((object) "Index extreme exhausted");
                  Debug.Log((object) ("**** CASTINGCARDBLOCKED REMOVE " + comingFromCardId));
                  matchManager.castingCardBlocked[_cardData.InternalId] = false;
                }
                if (GameManager.Instance.GetDeveloperMode() && exaustCodeGen % 100 == 0)
                  Debug.Log((object) ("[DEALNEWCARD] indexExtremeBlock" + exaustCodeGen.ToString() + " <-- " + _cardData.InternalId));
              }
            }
            yield return (object) Globals.Instance.WaitForSeconds(0.1f);
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("End autodraw cast card " + _cardData.InternalId, "general");
            matchManager.gameStatus = codeGen;
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("GameStatus->" + matchManager.gameStatus, "general");
            if (comingFromCardId == "")
              yield return (object) Globals.Instance.WaitForSeconds(0.3f);
            codeGen = (string) null;
            CI = (CardItem) null;
          }
          else if (matchManager.gameStatus == "AddingCards" || matchManager.gameStatus == "DrawingCards")
            yield return (object) Globals.Instance.WaitForSeconds(0.2f);
          matchManager.RepositionCards();
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("CardsWaitingForRset POST " + matchManager.cardsWaitingForReset.ToString(), "general");
          --matchManager.cardsWaitingForReset;
          if (matchManager.cardsWaitingForReset < 0)
            matchManager.SetCardsWaitingForReset(0);
          matchManager.ResetAutoEndCount();
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD(nameof (comingFromCardId) + comingFromCardId, "general");
          if (comingFromCardId == "")
          {
            if (matchManager.cardsWaitingForReset > 0)
            {
              if (matchManager.theHero != null && matchManager.theHero.Alive)
              {
                matchManager.StartCoroutine(matchManager.DealNewCard(Enums.CardFrom.Deck));
                yield break;
              }
              else
              {
                yield return (object) Globals.Instance.WaitForSeconds(1f);
                if (matchManager.theHero == null || !matchManager.theHero.Alive)
                {
                  yield break;
                }
                else
                {
                  matchManager.StartCoroutine(matchManager.DealNewCard(Enums.CardFrom.Deck));
                  yield break;
                }
              }
            }
            else
            {
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("finish new card Gamestatus->" + matchManager.gameStatus, "general");
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("isBeginTournPhase->" + matchManager.isBeginTournPhase.ToString(), "general");
              if (matchManager.isBeginTournPhase)
              {
                exaustCodeGen = 0;
                while (matchManager.eventList.Count > 0)
                {
                  if (GameManager.Instance.GetDeveloperMode() && exaustCodeGen % 50 == 0)
                  {
                    matchManager.eventListDbg = "Waiting begin turn because of eventlist ";
                    for (int index = 0; index < matchManager.eventList.Count; ++index)
                      matchManager.eventListDbg = matchManager.eventListDbg + matchManager.eventList[index] + " || ";
                  }
                  ++exaustCodeGen;
                  if (exaustCodeGen > 150)
                  {
                    Debug.Log((object) "[Waiting begin turn] EXHAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    matchManager.ClearEventList();
                    break;
                  }
                  yield return (object) Globals.Instance.WaitForSeconds(0.01f);
                }
                matchManager.BeginTurnHero();
                yield break;
              }
            }
          }
        }
      }
    }
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("FINISH DEAL CARD ->" + comingFromCardId, "trace");
    if (comingFromCardId != "")
      matchManager.castingCardBlocked[comingFromCardId] = false;
    yield return (object) null;
  }

  private IEnumerator CreateCard(int i, Enums.CardFrom fromPlace)
  {
    if (this.heroActive > -1)
    {
      string id = this.HeroHand[this.heroActive][i];
      GameObject gameObject = fromPlace != Enums.CardFrom.Deck ? UnityEngine.Object.Instantiate<GameObject>(GameManager.Instance.CardPrefab, this.DiscardPilePosition, Quaternion.identity, this.GO_Hand.transform) : UnityEngine.Object.Instantiate<GameObject>(GameManager.Instance.CardPrefab, this.DeckPilePosition, Quaternion.identity, this.GO_Hand.transform);
      CardItem component = gameObject.GetComponent<CardItem>();
      gameObject.name = id;
      gameObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -179f);
      this.cardItemTable.Add(component);
      component.SetTablePositionValues(i, this.CountHeroHand());
      if (fromPlace == Enums.CardFrom.Deck || fromPlace == Enums.CardFrom.Discard)
        component.SetCard(id, _theHero: this.theHero, _generated: true);
      else
        component.SetCard(id, _theHero: this.theHero);
      component.DisableCollider();
      if (this.gameStatus == "AddingCards" || this.gameStatus == "DrawingCards")
      {
        component.SetDestinationScaleRotation(Vector3.zero - component.transform.parent.transform.localPosition, 1.4f, Quaternion.Euler(0.0f, 0.0f, 0.0f));
        component.active = true;
        yield return (object) null;
      }
    }
  }

  private IEnumerator ResetDeck(bool ShuffleAction = true)
  {
    this.SetEventDirect(nameof (ResetDeck));
    if (this.HeroDeckDiscard[this.heroActive] != null)
    {
      List<string> ts = new List<string>();
      for (int index = 0; index < this.CountHeroDiscard(); ++index)
        ts.Add(this.HeroDeckDiscard[this.heroActive][index]);
      if (ts.Count > 0)
      {
        if (ShuffleAction)
          this.HeroDeck[this.heroActive] = ts.ShuffleList<string>();
        this.HeroDeckDiscard[this.heroActive].Clear();
        if (GameManager.Instance.IsMultiplayer())
          yield return (object) Globals.Instance.WaitForSeconds(0.3f);
        else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
          yield return (object) Globals.Instance.WaitForSeconds(0.5f);
        else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Ultrafast)
          yield return (object) Globals.Instance.WaitForSeconds(0.1f);
        else
          yield return (object) Globals.Instance.WaitForSeconds(0.3f);
        if ((UnityEngine.Object) this.shuffleSound != (UnityEngine.Object) null)
          GameManager.Instance.PlayAudio(this.shuffleSound);
        this.WriteDiscardCounter("0");
        int index = 0;
        for (int i = this.GO_DiscardPile.transform.childCount - 1; i >= 0; --i)
        {
          Transform child = this.GO_DiscardPile.transform.GetChild(i);
          this.DrawDeckPile(index + 1);
          if ((double) child.localPosition.z >= 0.0)
          {
            if (GameManager.Instance.IsMultiplayer())
            {
              child.transform.localPosition -= new Vector3(0.5f, 0.0f, 1f);
              yield return (object) Globals.Instance.WaitForSeconds(0.02f);
            }
            else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
            {
              child.transform.localPosition -= new Vector3(0.5f, 0.0f, 1f);
              yield return (object) Globals.Instance.WaitForSeconds(0.02f);
            }
            else
              yield return (object) null;
          }
          child.gameObject.SetActive(false);
          child = (Transform) null;
        }
        this.DestroyDiscardPileChilds();
        this.SetEventDirect(nameof (ResetDeck), false);
        yield break;
      }
      else
        this.SetCardsWaitingForReset(0);
    }
    this.SetEventDirect(nameof (ResetDeck), false);
  }

  public void DestroyDiscardPileChilds()
  {
    for (int index = this.GO_DiscardPile.transform.childCount - 1; index >= 0; --index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.GO_DiscardPile.transform.GetChild(index).gameObject);
  }

  public int NumChildsInTemporal() => this.tempTransform.childCount;

  private void GetCardFromDeckToHand()
  {
    int numCards = this.CountHeroDeck();
    this.DrawDeckPile(numCards);
    if (numCards <= 0)
      return;
    string str = this.HeroDeck[this.heroActive][0];
    this.HeroDeck[this.heroActive].RemoveAt(0);
    this.HeroHand[this.heroActive].Add(str);
  }

  public void GetCardFromDeckToHandNPC(int npcIndex)
  {
    if (this.CountNPCDeck(npcIndex) == 0)
    {
      this.NPCDeck[npcIndex] = new List<string>((IEnumerable<string>) this.NPCDeckDiscard[npcIndex]);
      this.NPCDeckDiscard[npcIndex].Clear();
      this.NPCDeck[npcIndex] = this.NPCDeck[npcIndex].ShuffleList<string>();
    }
    if (this.NPCDeck[npcIndex].Count <= 0)
      return;
    string str = this.NPCDeck[npcIndex][0];
    this.NPCDeck[npcIndex].RemoveAt(0);
    if (this.NPCHand[npcIndex] == null)
      this.NPCHand[npcIndex] = new List<string>();
    this.NPCHand[npcIndex].Add(str);
  }

  public void ResetDeckHandNPC(int npcIndex)
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("ResetDeckHandNPC " + npcIndex.ToString(), "trace");
    this.NPCHand[npcIndex].Clear();
  }

  public void AddCardToNPCDeck(int npcIndex, string idCard, string npcIinternalId)
  {
    string lower = (idCard + "_" + this.GetRandomString()).ToLower();
    if (!this.cardDictionary.ContainsKey(lower))
    {
      CardData cardData = Globals.Instance.GetCardData(idCard, false);
      this.cardDictionary.Add(lower, cardData);
    }
    this.cardDictionary[lower].InitClone(idCard);
    int index1 = 0;
    for (int index2 = 0; index2 < this.NPCDeck[npcIndex].Count && this.NPCDeck[npcIndex] != null && this.NPCDeck[npcIndex][index2] != null && Globals.Instance.GetCardData(this.NPCDeck[npcIndex][index2].Split('_', StringSplitOptions.None)[0], false).Corrupted; ++index2)
      ++index1;
    this.NPCDeck[npcIndex].Insert(index1, lower);
  }

  public void RefreshStatusEffects()
  {
    for (int index = 0; index < this.TeamHero.Length; ++index)
    {
      if (this.TeamHero[index] != null && this.TeamHero[index].Alive)
        this.TeamHero[index].UpdateAuraCurseFunctions();
    }
    for (int index = 0; index < this.TeamNPC.Length; ++index)
    {
      if (this.TeamNPC[index] != null && this.TeamNPC[index].Alive)
        this.TeamNPC[index].UpdateAuraCurseFunctions();
    }
  }

  public void RefreshItems()
  {
    this.iconWeapon.ShowIcon("weapon");
    this.iconArmor.ShowIcon("armor");
    this.iconJewelry.ShowIcon("jewelry");
    this.iconAccesory.ShowIcon("accesory");
    this.iconPet.ShowIcon("pet");
    if (this.theHero == null || !((UnityEngine.Object) this.theHero.HeroItem != (UnityEngine.Object) null))
      return;
    this.theHero.HeroItem.ShowEnchantments();
    this.theHero.UpdateAuraCurseFunctions();
  }

  public void RemoveCorruptionItemFromNPC(int _npcIndex)
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("RemoveCorruptionItemFromNPC " + _npcIndex.ToString());
    this.TeamNPC[_npcIndex].Corruption = "";
  }

  private void ClearItemExecuteForThisTurn() => this.itemExecutedInTurn.Clear();

  private void ClearItemExecuteForThisTurnSpecific(string _key)
  {
    if (!this.itemExecutedInTurn.ContainsKey(_key))
      return;
    this.itemExecutedInTurn.Remove(_key);
  }

  public bool CanExecuteItemInThisTurn(string charId, string itemId, int timesPerTurn)
  {
    string key = charId + "_" + itemId;
    return !this.itemExecutedInTurn.ContainsKey(key) || this.itemExecutedInTurn[key] < timesPerTurn;
  }

  public bool CanExecuteItemInThisCombat(string charId, string itemId, int timesPerCombat)
  {
    string key = charId + "_" + itemId;
    return !this.itemExecutedInCombat.ContainsKey(key) || this.itemExecutedInCombat[key] < timesPerCombat;
  }

  public bool ItemExecuteForThisTurn(
    string charId,
    string itemId,
    int timesPerTurn,
    string itemType)
  {
    int times = 1;
    string key = charId + "_" + itemId;
    if (this.itemExecutedInTurn.ContainsKey(key))
    {
      if (this.itemExecutedInTurn[key] >= timesPerTurn)
        return true;
      times = ++this.itemExecutedInTurn[key];
    }
    else
      this.itemExecutedInTurn.Add(key, times);
    switch (itemType)
    {
      case "weapon":
        this.iconWeapon.SetTimesExecuted(times);
        break;
      case "armor":
        this.iconArmor.SetTimesExecuted(times);
        break;
      case "jewelry":
        this.iconJewelry.SetTimesExecuted(times);
        break;
      case "accesory":
        this.iconAccesory.SetTimesExecuted(times);
        break;
    }
    return false;
  }

  private void ItemExecuteForThisCombatDuplicate()
  {
    this.itemExecutedInCombatTurnSave = new Dictionary<string, int>();
    foreach (KeyValuePair<string, int> keyValuePair in this.itemExecutedInCombat)
      this.itemExecutedInCombatTurnSave.Add(keyValuePair.Key, keyValuePair.Value);
  }

  private void ItemExecuteForThisCombatReload()
  {
    this.itemExecutedInCombat = new Dictionary<string, int>();
    foreach (KeyValuePair<string, int> keyValuePair in this.itemExecutedInCombatTurnSave)
      this.itemExecutedInCombat.Add(keyValuePair.Key, keyValuePair.Value);
  }

  public bool ItemExecuteForThisCombat(
    string charId,
    string itemId,
    int timesPerCombat,
    string itemType)
  {
    int times = 1;
    string key = charId + "_" + itemId;
    if (this.itemExecutedInCombat.ContainsKey(key))
    {
      if (this.itemExecutedInCombat[key] >= timesPerCombat)
        return true;
      times = ++this.itemExecutedInCombat[key];
    }
    else
      this.itemExecutedInCombat.Add(key, times);
    switch (itemType)
    {
      case "weapon":
        this.iconWeapon.SetTimesExecuted(times);
        break;
      case "armor":
        this.iconArmor.SetTimesExecuted(times);
        break;
      case "jewelry":
        this.iconJewelry.SetTimesExecuted(times);
        break;
      case "accesory":
        this.iconAccesory.SetTimesExecuted(times);
        break;
    }
    return false;
  }

  public int ItemExecutedInThisCombat(string charId, string itemId)
  {
    int num = 0;
    string key = charId + "_" + itemId;
    if (this.itemExecutedInCombat.ContainsKey(key))
      num = this.itemExecutedInCombat[key];
    return num;
  }

  public void EnchantmentExecute(string charId, string itemId)
  {
    int num = 1;
    string key = charId + "_" + itemId;
    if (this.enchantmentExecutedTotal.ContainsKey(key))
      ++this.enchantmentExecutedTotal[key];
    else
      this.enchantmentExecutedTotal.Add(key, num);
  }

  public int EnchantmentExecutedTimes(string charId, string itemId)
  {
    string key = charId + "_" + itemId;
    return this.enchantmentExecutedTotal.ContainsKey(key) ? this.enchantmentExecutedTotal[key] : 0;
  }

  public void CleanEnchantmentExecutedTimes(string charId, string itemId)
  {
    string key = charId + "_" + itemId;
    if (!this.enchantmentExecutedTotal.ContainsKey(key))
      return;
    this.enchantmentExecutedTotal.Remove(key);
  }

  public void DoItemEventDelay() => this.StartCoroutine(this.DoItemEventDelayCo());

  public IEnumerator DoItemEventDelayCo()
  {
    string waitEvent = "DoItemEventDelay_+" + Time.frameCount.ToString();
    this.eventList.Add(waitEvent);
    yield return (object) Globals.Instance.WaitForSeconds(0.7f);
    this.eventList.Remove(waitEvent);
  }

  public void ItemActivationDisplay(string itemType)
  {
    switch (itemType)
    {
      case "weapon":
        this.iconWeapon.SetActivated();
        break;
      case "armor":
        this.iconArmor.SetActivated();
        break;
      case "jewelry":
        this.iconJewelry.SetActivated();
        break;
      case "accesory":
        this.iconAccesory.SetActivated();
        break;
    }
  }

  private IEnumerator MoveItemsOut(bool state)
  {
    if (!state)
    {
      this.iconWeapon.MoveIn("weapon", 0.0f, this.theHero);
      this.iconArmor.MoveIn("armor", 0.1f, this.theHero);
      this.iconJewelry.MoveIn("jewelry", 0.2f, this.theHero);
      this.iconAccesory.MoveIn("accesory", 0.3f, this.theHero);
      this.iconPet.MoveIn("pet", 0.4f, this.theHero);
    }
    else
    {
      this.iconPet.MoveOut(0.0f);
      this.iconAccesory.MoveOut(0.05f);
      this.iconJewelry.MoveOut(0.1f);
      this.iconArmor.MoveOut(0.15f);
      this.iconWeapon.MoveOut(0.2f);
    }
    yield return (object) null;
  }

  private IEnumerator MoveDecksOut(bool state)
  {
    this.MovingDeckPile = true;
    if (this.MatchIsOver)
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("[MOVEDECKSOUT] Broken by finish game");
    }
    else
    {
      while (this.waitingKill)
      {
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[MOVEDECKSOUT] Waitingforkill");
        yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      }
      if (!this.theHero.Alive && !state)
      {
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[MOVEDECKSOUT] Broken by hero dead");
      }
      else
      {
        Vector3 vectorDestination;
        if (state)
        {
          for (int index = this.GO_DiscardPile.transform.childCount - 1; index >= 0; --index)
          {
            CardItem component = this.GO_DiscardPile.transform.GetChild(index).GetComponent<CardItem>();
            component.GetComponent<BoxCollider2D>().enabled = false;
            component.DisableTrail();
          }
          vectorDestination = this.DeckPilePosition - this.DeckPileOutOfScreenPositionVector;
        }
        else
        {
          if (!this.GO_DecksObject.gameObject.activeSelf)
            this.GO_DecksObject.gameObject.SetActive(true);
          vectorDestination = this.DeckPilePosition;
          this.RepaintDeckPile();
          this.RedoDiscardPile();
          this.WriteNewCardsCounter();
        }
        if (!state)
        {
          foreach (Transform transform in this.GO_Hand.transform)
          {
            if ((UnityEngine.Object) transform != (UnityEngine.Object) null && transform.gameObject.name != "HandMask")
              UnityEngine.Object.Destroy((UnityEngine.Object) transform.gameObject);
          }
        }
        int eventExaustBeginH;
        if (!state && this.gameStatus == "BeginTurnHero")
        {
          this.ResetItemTimeout();
          this.theHero.SetEvent(Enums.EventActivation.BeginTurnAboutToDealCards);
          eventExaustBeginH = 0;
          if (this.theHero.HaveAboutToDealCardsItemNum() > 0)
          {
            Debug.Log((object) ("****** enter ***** " + this.gameStatus));
            yield return (object) Globals.Instance.WaitForSeconds(0.2f * (float) this.theHero.HaveAboutToDealCardsItemNum());
            while (this.generatedCardTimes > 0)
            {
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("*********** " + this.generatedCardTimes.ToString() + "||" + this.gameStatus + "||" + this.NumChildsInTemporal().ToString(), "item");
              yield return (object) Globals.Instance.WaitForSeconds(0.7f);
              ++eventExaustBeginH;
              if (eventExaustBeginH > 10)
                this.generatedCardTimes = 0;
            }
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("******exit ***** " + this.gameStatus, "item");
          }
          yield return (object) Globals.Instance.WaitForSeconds(0.1f);
          eventExaustBeginH = 0;
          while (this.eventList.Count > 0)
          {
            if (GameManager.Instance.GetDeveloperMode() && eventExaustBeginH % 50 == 0)
            {
              this.eventListDbg = "";
              for (int index = 0; index < this.eventList.Count; ++index)
                this.eventListDbg = this.eventListDbg + this.eventList[index] + " || ";
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD(this.eventListDbg);
            }
            ++eventExaustBeginH;
            if (eventExaustBeginH > 200)
            {
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("[MoveDecksOut IN] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
              this.ClearEventList();
              break;
            }
            yield return (object) Globals.Instance.WaitForSeconds(0.01f);
          }
        }
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[MOVING DECK PILE MOVEMENT]", "trace");
        float movementSpeed = 0.9f;
        while ((double) Vector3.Distance(this.GO_DecksObject.transform.localPosition, vectorDestination) > 0.25)
        {
          this.GO_DecksObject.transform.localPosition = Vector3.Lerp(this.GO_DecksObject.transform.localPosition, vectorDestination, movementSpeed);
          yield return (object) null;
        }
        this.GO_DecksObject.transform.localPosition = vectorDestination;
        if (state)
        {
          if (this.GO_DecksObject.gameObject.activeSelf)
            this.GO_DecksObject.gameObject.SetActive(false);
          this.DestroyDiscardPileChilds();
        }
        if (!state)
        {
          yield return (object) Globals.Instance.WaitForSeconds(0.1f);
          eventExaustBeginH = 0;
          while (this.eventList.Count > 0)
          {
            if (GameManager.Instance.GetDeveloperMode() && eventExaustBeginH % 50 == 0)
            {
              this.eventListDbg = "";
              for (int index = 0; index < this.eventList.Count; ++index)
                this.eventListDbg = this.eventListDbg + this.eventList[index] + " || ";
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("[MOVING DECK PILE END] Waiting For Eventlist to clean");
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD(this.eventListDbg);
            }
            ++eventExaustBeginH;
            if (eventExaustBeginH > 500)
            {
              if (Globals.Instance.ShowDebug)
                Functions.DebugLogGD("[MOVING DECK PILE END] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
              this.ClearEventList();
              break;
            }
            yield return (object) Globals.Instance.WaitForSeconds(0.01f);
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("=>" + eventExaustBeginH.ToString());
          }
          if (eventExaustBeginH > 5)
          {
            if (GameManager.Instance.IsMultiplayer())
              yield return (object) Globals.Instance.WaitForSeconds(0.3f);
            else
              yield return (object) Globals.Instance.WaitForSeconds(0.15f);
          }
        }
        this.MovingDeckPile = false;
        Cursor.visible = true;
      }
    }
  }

  public string CardFromNPCHand(int index, int position)
  {
    return this.NPCHand != null && index < this.NPCHand.Length && this.NPCHand[index] != null && position < this.NPCHand[index].Count && this.NPCHand[index][position] != null ? this.NPCHand[index][position] : "";
  }

  public void RemoveCardsFromNPCHand(int index, int position)
  {
    for (int index1 = this.NPCHand[index].Count - 1; index1 >= position; --index1)
      this.NPCHand[index].RemoveAt(index1);
  }

  public void MoveCards(
    Enums.CardPlace from,
    Enums.CardPlace to,
    Enums.CardClass cardClass = Enums.CardClass.None,
    int energyReductionPermanent = 0)
  {
    if (from == Enums.CardPlace.Vanish)
    {
      for (int index = this.HeroDeckVanish[this.heroActive].Count - 1; index >= 0; --index)
      {
        CardData cardData = this.GetCardData(this.HeroDeckVanish[this.heroActive][index]);
        bool flag = false;
        if (cardClass == Enums.CardClass.None)
          flag = true;
        else if (cardClass == cardData.CardClass)
          flag = true;
        if (flag)
        {
          if (energyReductionPermanent != 0)
            cardData.EnergyReductionPermanent += energyReductionPermanent;
          if (to == Enums.CardPlace.RandomDeck)
            this.HeroDeck[this.heroActive].Insert(this.GetRandomIntRange(0, this.HeroDeck[this.heroActive].Count), this.HeroDeckVanish[this.heroActive][index]);
          this.HeroDeckVanish[this.heroActive].RemoveAt(index);
        }
      }
    }
    if (to != Enums.CardPlace.RandomDeck && to != Enums.CardPlace.BottomDeck && to != Enums.CardPlace.TopDeck)
      return;
    this.DrawDeckPile(this.CountHeroDeck() + 1);
  }

  private void WriteDiscardCounter(string text = "")
  {
    if (text == "")
      text = this.CountHeroDiscard().ToString();
    if (text == "0")
    {
      text = "";
      if (this.discardCounter.gameObject.activeSelf)
        this.discardCounter.gameObject.SetActive(false);
    }
    else if (!this.discardCounter.gameObject.activeSelf)
      this.discardCounter.gameObject.SetActive(true);
    this.discardCounterTM.text = text;
    this.combatTarget.RefreshCards();
  }

  private void WriteNewCardsCounter()
  {
    if (this.theHero == null)
      return;
    this.newCardsCounterTM.text = "+" + this.theHero.GetDrawCardsTurnForDisplayInDeck().ToString();
  }

  private void WriteDeckCounter(string text = "")
  {
    if (text == "")
      text = this.CountHeroDeck().ToString();
    this.deckCounterTM.text = text;
    this.combatTarget.RefreshCards();
  }

  private void DrawDeckPile(int numCards)
  {
    int num1 = numCards - 1;
    this.WriteDeckCounter(num1.ToString());
    int num2 = num1 > 0 ? (num1 >= 3 ? (num1 >= 7 ? 3 : 2) : 1) : 0;
    if (num2 == this.deckPileVisualState)
      return;
    for (int index = 1; index <= num2; ++index)
    {
      this.GO_DeckPileCards[index - 1].transform.gameObject.SetActive(true);
      if (index < num2)
        this.GO_DeckPileCards[index - 1].GetComponent<BoxCollider2D>().enabled = false;
      else
        this.GO_DeckPileCards[index - 1].GetComponent<BoxCollider2D>().enabled = true;
    }
    for (int index = num2 + 1; index <= 3; ++index)
      this.GO_DeckPileCards[index - 1].transform.gameObject.SetActive(false);
    this.deckPileVisualState = num2;
  }

  private void SetCardbacks()
  {
    if (this.theHero == null)
      return;
    string cardbackUsed = this.theHero.CardbackUsed;
    if (!(cardbackUsed != ""))
      return;
    CardbackData cardbackData = Globals.Instance.GetCardbackData(cardbackUsed);
    if ((UnityEngine.Object) cardbackData == (UnityEngine.Object) null)
    {
      cardbackData = Globals.Instance.GetCardbackData(Globals.Instance.GetCardbackBaseIdBySubclass(this.theHero.HeroData.HeroSubClass.Id));
      if ((UnityEngine.Object) cardbackData == (UnityEngine.Object) null)
        cardbackData = Globals.Instance.GetCardbackData("defaultCardback");
    }
    Sprite cardbackSprite = cardbackData.CardbackSprite;
    if (!((UnityEngine.Object) cardbackSprite != (UnityEngine.Object) null))
      return;
    for (int index = 0; index < this.GO_DeckPileCardsT.Length; ++index)
      this.GO_DeckPileCardsT[index].GetComponent<SpriteRenderer>().sprite = cardbackSprite;
  }

  private void DrawDeckPileLayer(string theLayer)
  {
    for (int index = 0; index < this.GO_DeckPileCardsT.Length; ++index)
      this.GO_DeckPileCardsT[index].GetComponent<SpriteRenderer>().sortingLayerName = theLayer;
    this.SetCardbacks();
  }

  private void RepaintDeckPile()
  {
    Sprite cardBackSprite = GameManager.Instance.cardBackSprites[(int) this.theHero.HeroData.HeroClass];
    for (int index = 0; index < this.GO_DeckPileCardsT.Length; ++index)
      this.GO_DeckPileCardsT[index].GetComponent<SpriteRenderer>().sprite = cardBackSprite;
    this.SetCardbacks();
  }

  public void DrawDeckScreen(int heroTarget = -1, int type = 0, int quantity = -100)
  {
    if (this.coroutineDrawDeck != null)
      this.StopCoroutine(this.coroutineDrawDeck);
    this.coroutineDrawDeck = this.StartCoroutine(this.DrawDeckScreenCo(heroTarget, type, quantity));
  }

  private IEnumerator DrawDeckScreenCo(int heroTarget, int type, int quantity)
  {
    this.GO_List = new List<GameObject>();
    this.deckCardsWindow.TurnOn(type);
    List<string> CardList = new List<string>();
    if (heroTarget == -1)
      heroTarget = this.heroActive;
    if (type == 0)
    {
      if (quantity == -100)
      {
        for (int index = 0; index < this.HeroDeck[heroTarget].Count; ++index)
          CardList.Add(this.HeroDeck[heroTarget][index]);
        CardList.Sort();
      }
      else
      {
        if (quantity > this.HeroDeck[heroTarget].Count)
          quantity = this.HeroDeck[heroTarget].Count;
        for (int index = 0; index < quantity; ++index)
          CardList.Add(this.HeroDeck[heroTarget][index]);
      }
    }
    else
      CardList = this.HeroDeckDiscard[heroTarget];
    int numCards = CardList.Count;
    this.cardGos.Clear();
    for (int i = 0; i < numCards; ++i)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, this.deckCardsWindow.cardContainer);
      this.GO_List.Add(gameObject);
      CardItem CI = gameObject.GetComponent<CardItem>();
      gameObject.name = "TMP_" + i.ToString();
      this.cardGos.Add(gameObject.name, gameObject);
      CI.SetCard(CardList[i], false, this.theHero);
      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      CI.AmplifyForSelection(i, numCards);
      CI.SetDestination(CI.GetDestination() - new Vector3(2.5f, -4.5f, 0.0f));
      CI.DisableTrail();
      CI.HideRarityParticles();
      CI.HideCardIconParticles();
      CI.cardfordisplay = true;
      CI.discard = true;
      if (this.IsYourTurnForAddDiscard())
        CI.ShowKeyNum(true, (i + 1).ToString());
      yield return (object) null;
      CI = (CardItem) null;
    }
  }

  public void DrawDeckScreenDestroy()
  {
    if (this.coroutineDrawDeck != null)
      this.StopCoroutine(this.coroutineDrawDeck);
    if (this.GO_List != null)
    {
      for (int index = this.GO_List.Count - 1; index >= 0; --index)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.GO_List[index]);
    }
    for (int index = 0; index < this.deckCardsWindow.cardContainer.transform.childCount; ++index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.deckCardsWindow.cardContainer.transform.GetChild(index).gameObject);
    this.deckCardsWindow.TurnOff();
  }

  private void RepositionCards()
  {
    this.RedrawPositionInTable();
    int num = this.CountHeroHand();
    float x = -0.69f;
    for (int index = 0; index < num; ++index)
    {
      if (index < 1)
        x += 0.58f;
      else if (index < 2)
        x += 0.78f;
      else if (index < 3)
        x += 0.78f;
      else if (index < 4)
        x += 0.62f;
      else if (index < 5)
        x += 0.7f;
      else if (index < 6)
        x += 0.4f;
      else if (index < 8)
        x += 0.12f;
      else if (index < 9)
        x += -0.1f;
      else
        x += -0.14f;
    }
    this.handTransformPosition = new Vector3(x, this.GO_Hand.transform.position.y, 0.0f);
    this.GO_Hand.transform.position = this.handTransformPosition;
    this.HandMask.localPosition = new Vector3(-this.GO_Hand.transform.position.x, 0.0f, this.HandMask.localPosition.z);
  }

  public void RedrawPositionInTable()
  {
    int count = this.cardItemTable.Count;
    for (int index = 0; index < count; ++index)
    {
      this.cardItemTable[index].SetTablePositionValues(index, count);
      this.cardItemTable[index].PositionCardInTable();
    }
  }

  public void ClearCardsBorder()
  {
    if (this.heroActive <= -1)
      return;
    int count = this.cardItemTable.Count;
    if (count == 0)
      return;
    for (int index = 0; index < count; ++index)
      this.cardItemTable[index].HideEnergyBorder();
  }

  public void RedrawCardsBorder()
  {
    if (this.heroActive <= -1 || this.WaitingForActionScreen() || this.cardsWaitingForReset > 0 || this.gameBusy || this.eventList.Count > 0)
      return;
    int count = this.cardItemTable.Count;
    if (count == 0)
      return;
    bool flag = false;
    if (!this.HandMask.gameObject.activeSelf)
      flag = true;
    for (int index = 0; index < count; ++index)
    {
      if (flag)
        this.cardItemTable[index].DrawEnergyBorder();
      else
        this.cardItemTable[index].HideEnergyBorder();
    }
    if (!flag)
      return;
    this.ShowCombatKeyboardByConfig();
  }

  public bool CanPlayACardRightNow()
  {
    if (this.heroActive > -1)
    {
      int count = this.cardItemTable.Count;
      for (int index = 0; index < count; ++index)
      {
        if (this.cardItemTable[index].IsPlayableRightNow())
          return true;
      }
    }
    return false;
  }

  public void RedrawCardsDescriptionPrecalculated()
  {
    if (this.heroActive <= -1)
      return;
    for (int index = 0; index < this.CountHeroHand(); ++index)
    {
      if ((UnityEngine.Object) this.cardItemTable[index] != (UnityEngine.Object) null)
        this.cardItemTable[index].RedrawDescriptionPrecalculated(this.theHero);
    }
  }

  public void RedrawCardsDamageType()
  {
    if (this.heroActive <= -1)
      return;
    for (int index = 0; index < this.CountHeroHand(); ++index)
    {
      if ((UnityEngine.Object) this.cardItemTable[index] != (UnityEngine.Object) null)
        this.cardItemTable[index].RedrawCardsDamageType(this.theHero);
    }
  }

  public void DiscardItem(string card, Enums.CardPlace whereToDiscard = Enums.CardPlace.Discard)
  {
    switch (whereToDiscard)
    {
      case Enums.CardPlace.Discard:
        this.HeroDeckDiscard[this.heroActive].Add(card);
        this.DrawDiscardPileCardNumeral();
        break;
      case Enums.CardPlace.TopDeck:
        this.HeroDeck[this.heroActive].Insert(0, card);
        this.DrawDeckPile(this.CountHeroDeck() + 1);
        break;
      case Enums.CardPlace.BottomDeck:
        this.HeroDeck[this.heroActive].Add(card);
        this.DrawDeckPile(this.CountHeroDeck() + 1);
        break;
      case Enums.CardPlace.RandomDeck:
        this.HeroDeck[this.heroActive].Insert(this.GetRandomIntRange(0, this.HeroDeck[this.heroActive].Count, "deck"), card);
        this.DrawDeckPile(this.CountHeroDeck() + 1);
        break;
      case Enums.CardPlace.Vanish:
        this.HeroDeckVanish[this.heroActive].Add(card);
        break;
    }
  }

  public void DiscardCard(int cardPosition, Enums.CardPlace whereToDiscard = Enums.CardPlace.Discard, bool moveToDiscard = true)
  {
    if (this.heroActive == -1 || cardPosition <= -1 || cardPosition >= this.HeroHand[this.heroActive].Count || this.HeroHand[this.heroActive][cardPosition] == null)
      return;
    if ((UnityEngine.Object) this.discardSound != (UnityEngine.Object) null)
      GameManager.Instance.PlayAudio(this.discardSound);
    CardData cardData = this.GetCardData(this.HeroHand[this.heroActive][cardPosition]);
    if ((UnityEngine.Object) cardData != (UnityEngine.Object) null)
    {
      cardData.EnergyReductionTemporal = 0;
      cardData.EnergyReductionToZeroTemporal = false;
      if (moveToDiscard)
      {
        switch (whereToDiscard)
        {
          case Enums.CardPlace.Discard:
            this.HeroDeckDiscard[this.heroActive].Add(this.HeroHand[this.heroActive][cardPosition]);
            this.DrawDiscardPileCardNumeral();
            break;
          case Enums.CardPlace.TopDeck:
            this.HeroDeck[this.heroActive].Insert(0, this.HeroHand[this.heroActive][cardPosition]);
            this.DrawDeckPile(this.CountHeroDeck() + 1);
            break;
          case Enums.CardPlace.BottomDeck:
            this.HeroDeck[this.heroActive].Add(this.HeroHand[this.heroActive][cardPosition]);
            this.DrawDeckPile(this.CountHeroDeck() + 1);
            break;
          case Enums.CardPlace.RandomDeck:
            this.HeroDeck[this.heroActive].Insert(this.GetRandomIntRange(0, this.HeroDeck[this.heroActive].Count, "deck"), this.HeroHand[this.heroActive][cardPosition]);
            this.DrawDeckPile(this.CountHeroDeck() + 1);
            break;
          case Enums.CardPlace.Vanish:
            this.HeroDeckVanish[this.heroActive].Add(this.HeroHand[this.heroActive][cardPosition]);
            ++this.GlobalVanishCardsNum;
            break;
        }
      }
    }
    this.HeroHand[this.heroActive].RemoveAt(cardPosition);
    if ((UnityEngine.Object) this.cardItemTable.ElementAt<CardItem>(cardPosition) != (UnityEngine.Object) null)
      this.cardItemTable.RemoveAt(cardPosition);
    if (!(this.gameStatus != "DiscardingCards"))
      return;
    this.RepositionCards();
  }

  public void DeActivateDiscards()
  {
    for (int index = 0; index < this.GO_DiscardPile.transform.childCount - 1; ++index)
      this.GO_DiscardPile.transform.GetChild(index).GetComponent<CardItem>().enabled = false;
  }

  public void SetOverDeck(bool state)
  {
    if (state && (!state || !this.HaveDeckEffect(this.CardActive)))
      return;
    for (int index = 0; index < this.TeamHero.Length; ++index)
    {
      if (this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroItem == (UnityEngine.Object) null) && this.TeamHero[index].Alive)
      {
        if (state)
          this.TeamHero[index].HeroItem.ShowOverCards();
        else
          this.TeamHero[index].HeroItem.HideOverCards();
      }
    }
  }

  [PunRPC]
  private void NET_SetDamagePreview(bool theCasterIsHero, int tablePosition)
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD(nameof (NET_SetDamagePreview), "net");
    if (this.cardItemTable == null)
      return;
    CardData cardData = (CardData) null;
    if (tablePosition > -1 && tablePosition < this.cardItemTable.Count && (UnityEngine.Object) this.cardItemTable[tablePosition] != (UnityEngine.Object) null)
    {
      cardData = this.cardItemTable[tablePosition].CardData;
      this.cardActive = cardData;
    }
    this.SetDamagePreview(theCasterIsHero, cardData);
  }

  public void SetDamagePreview(bool theCasterIsHero, CardData cardData = null, int tablePosition = -1)
  {
    bool flag1 = true;
    bool flag2 = false;
    if ((UnityEngine.Object) cardData == (UnityEngine.Object) null)
      flag1 = false;
    else if (cardData.Damage == 0 && cardData.SelfHealthLoss == 0 && cardData.Heal == 0 && cardData.DamageSelf == 0 && cardData.HealCurses == 0 && cardData.DispelAuras == 0 && cardData.StealAuras == 0 && (UnityEngine.Object) cardData.HealAuraCurseSelf == (UnityEngine.Object) null && (UnityEngine.Object) cardData.HealAuraCurseName == (UnityEngine.Object) null && (UnityEngine.Object) cardData.HealAuraCurseName2 == (UnityEngine.Object) null && (UnityEngine.Object) cardData.HealAuraCurseName3 == (UnityEngine.Object) null && (UnityEngine.Object) cardData.HealAuraCurseName4 == (UnityEngine.Object) null && (UnityEngine.Object) cardData.Curse == (UnityEngine.Object) null && (UnityEngine.Object) cardData.Aura == (UnityEngine.Object) null)
      flag1 = false;
    int energyCost = 0;
    CardItem cardItem = (CardItem) null;
    if (tablePosition > -1 && tablePosition < this.cardItemTable.Count)
      energyCost = this.cardItemTable[tablePosition].GetEnergyCost();
    if ((UnityEngine.Object) cardItem != (UnityEngine.Object) null)
      energyCost = cardItem.GetEnergyCost();
    if (!flag1)
    {
      for (int index = 0; index < this.TeamNPC.Length; ++index)
      {
        if (this.TeamNPC[index] != null && !((UnityEngine.Object) this.TeamNPC[index].NpcData == (UnityEngine.Object) null) && this.TeamNPC[index].Alive && (UnityEngine.Object) this.TeamNPC[index].NPCItem != (UnityEngine.Object) null)
          this.TeamNPC[index].NPCItem.SetDamagePreview(false);
      }
      for (int index = 0; index < this.TeamHero.Length; ++index)
      {
        if (this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroData == (UnityEngine.Object) null) && this.TeamHero[index].Alive && (UnityEngine.Object) this.TeamHero[index].HeroItem != (UnityEngine.Object) null)
          this.TeamHero[index].HeroItem.SetDamagePreview(false);
      }
    }
    else
    {
      Character character1;
      if (theCasterIsHero)
      {
        character1 = (Character) this.TeamHero[this.heroActive];
        if ((UnityEngine.Object) this.cardActive != (UnityEngine.Object) null)
        {
          if (this.cardActive.SelfHealthLoss > 0)
          {
            if ((UnityEngine.Object) character1.HeroItem != (UnityEngine.Object) null)
            {
              int selfHealthLoss = this.cardActive.SelfHealthLoss;
              if (this.cardActive.SelfHealthLossSpecialValue1)
                selfHealthLoss = Functions.FuncRoundToInt(this.GetCardSpecialValue(this.cardActive, 1, character1, (Character) null, character1, (Character) null, false));
              else if (this.cardActive.SelfHealthLossSpecialValue2)
                selfHealthLoss = Functions.FuncRoundToInt(this.GetCardSpecialValue(this.cardActive, 2, character1, (Character) null, character1, (Character) null, false));
              character1.HeroItem.SetDamagePreview(true, selfHealthLoss, "heart");
              flag2 = true;
            }
          }
          else if (this.cardActive.DamageSelf > 0)
          {
            int damageSelf = this.cardActive.DamageSelf;
            Enums.DamageType damageType = this.cardActive.DamageType;
            if (damageSelf > 0)
            {
              if (this.cardActive.DamageSpecialValueGlobal)
                damageSelf = Functions.FuncRoundToInt(this.GetCardSpecialValue(cardData, 0, character1, (Character) null, character1, (Character) null, true));
              if (this.cardActive.DamageSpecialValue1)
                damageSelf = Functions.FuncRoundToInt(this.GetCardSpecialValue(cardData, 1, character1, (Character) null, character1, (Character) null, true));
              if (this.cardActive.DamageSpecialValue2)
                damageSelf = Functions.FuncRoundToInt(this.GetCardSpecialValue(cardData, 2, character1, (Character) null, character1, (Character) null, true));
            }
            int blocked = 0;
            int num1 = character1.DamageWithCharacterBonus(damageSelf, damageType, cardData.CardClass, energyCost) + character1.IncreasedCursedDamagePerStack(damageType);
            if (num1 <= 0)
              num1 = 0;
            int dmg;
            if (!character1.IsImmune(damageType))
            {
              if (!this.cardActive.GetIgnoreBlock())
              {
                int num2 = character1.GetBlock() - blocked;
                if (num2 > 0)
                {
                  if (num2 >= num1)
                  {
                    blocked += num1;
                    num1 = 0;
                  }
                  else
                  {
                    blocked += num2;
                    num1 -= num2;
                  }
                }
              }
              int num3 = -1 * character1.BonusResists(damageType);
              dmg = Functions.FuncRoundToInt((float) num1 + (float) ((double) num1 * (double) num3 * 0.0099999997764825821));
            }
            else
              dmg = 0;
            string dmgType = Enum.GetName(typeof (Enums.DamageType), (object) damageType).ToLower();
            if (dmgType == "none")
              dmgType = "heart";
            if ((dmg > 0 || blocked > 0) && (UnityEngine.Object) character1.HeroItem != (UnityEngine.Object) null)
            {
              character1.HeroItem.SetDamagePreview(true, dmg, dmgType, blocked: blocked);
              flag2 = true;
            }
          }
          if ((UnityEngine.Object) cardData.HealAuraCurseSelf != (UnityEngine.Object) null && (UnityEngine.Object) character1.HeroItem != (UnityEngine.Object) null)
          {
            character1.HeroItem.SetDamagePreview(true, _cardData: this.cardActive);
            flag2 = true;
          }
        }
      }
      else
        character1 = (Character) this.TeamNPC[this.npcActive];
      List<Transform> castTransformList = this.GetInstaCastTransformList(this.cardActive);
      for (int index1 = 0; index1 < castTransformList.Count; ++index1)
      {
        CharacterItem component;
        Character character2;
        if ((UnityEngine.Object) castTransformList[index1].GetComponent<NPCItem>() != (UnityEngine.Object) null && castTransformList[index1].GetComponent<NPCItem>().NPC != null)
        {
          component = (CharacterItem) castTransformList[index1].GetComponent<NPCItem>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            character2 = (Character) component.NPC;
          else
            continue;
        }
        else if ((UnityEngine.Object) castTransformList[index1].GetComponent<HeroItem>() != (UnityEngine.Object) null && castTransformList[index1].GetComponent<HeroItem>().Hero != null)
        {
          component = (CharacterItem) castTransformList[index1].GetComponent<HeroItem>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            character2 = (Character) component.Hero;
          else
            continue;
        }
        else
          continue;
        if (!((UnityEngine.Object) this.TeamHero[this.heroActive].HeroItem == (UnityEngine.Object) component & flag2))
        {
          int[] numArray = new int[2]{ 99999999, 0 };
          string[] strArray = new string[2];
          int heal1 = 0;
          int blocked = 0;
          Character _casterHero = character1.IsHero ? character1 : (Character) null;
          Character _casterNPC = character1.IsHero ? (Character) null : character1;
          Character _targetHero = character2.IsHero ? character2 : (Character) null;
          Character _targetNPC = character2.IsHero ? (Character) null : character2;
          for (int index2 = 0; index2 < 2; ++index2)
          {
            int num4;
            Enums.DamageType damageType;
            if (index2 == 0)
            {
              num4 = cardData.Damage;
              damageType = cardData.DamageType;
              if (num4 > 0)
              {
                if (this.cardActive.DamageSpecialValueGlobal)
                  num4 = Functions.FuncRoundToInt(this.GetCardSpecialValue(cardData, 0, _casterHero, _casterNPC, _targetHero, _targetNPC, true));
                if (this.cardActive.DamageSpecialValue1)
                  num4 = Functions.FuncRoundToInt(this.GetCardSpecialValue(cardData, 1, _casterHero, _casterNPC, _targetHero, _targetNPC, true));
                if (this.cardActive.DamageSpecialValue2)
                  num4 = Functions.FuncRoundToInt(this.GetCardSpecialValue(cardData, 2, _casterHero, _casterNPC, _targetHero, _targetNPC, true));
              }
              if (damageType == cardData.DamageType2)
              {
                int num5 = 0;
                bool flag3 = false;
                if (this.cardActive.Damage2SpecialValueGlobal)
                {
                  num5 = Functions.FuncRoundToInt(this.GetCardSpecialValue(cardData, 0, _casterHero, _casterNPC, _targetHero, _targetNPC, true));
                  flag3 = true;
                }
                else if (this.cardActive.Damage2SpecialValue1)
                {
                  num5 = Functions.FuncRoundToInt(this.GetCardSpecialValue(cardData, 1, _casterHero, _casterNPC, _targetHero, _targetNPC, true));
                  flag3 = true;
                }
                else if (this.cardActive.Damage2SpecialValue2)
                {
                  num5 = Functions.FuncRoundToInt(this.GetCardSpecialValue(cardData, 2, _casterHero, _casterNPC, _targetHero, _targetNPC, true));
                  flag3 = true;
                }
                if (!flag3)
                  num5 = cardData.Damage2;
                num4 += num5;
              }
            }
            else if (cardData.DamageType != cardData.DamageType2)
            {
              num4 = cardData.Damage2;
              damageType = cardData.DamageType2;
              if (num4 > 0)
              {
                if (this.cardActive.Damage2SpecialValueGlobal)
                  num4 = Functions.FuncRoundToInt(this.GetCardSpecialValue(cardData, 0, _casterHero, _casterNPC, _targetHero, _targetNPC, true));
                if (this.cardActive.Damage2SpecialValue1)
                  num4 = Functions.FuncRoundToInt(this.GetCardSpecialValue(cardData, 1, _casterHero, _casterNPC, _targetHero, _targetNPC, true));
                if (this.cardActive.Damage2SpecialValue2)
                  num4 = Functions.FuncRoundToInt(this.GetCardSpecialValue(cardData, 2, _casterHero, _casterNPC, _targetHero, _targetNPC, true));
              }
            }
            else
              continue;
            if (num4 > 0 || num4 == 0 && damageType != Enums.DamageType.None && cardData.DamageSelf == 0)
            {
              int num6 = (!theCasterIsHero ? character1.DamageWithCharacterBonus(num4, damageType, cardData.CardClass) : character1.DamageWithCharacterBonus(num4, damageType, cardData.CardClass, energyCost)) + character2.IncreasedCursedDamagePerStack(damageType, cardData);
              if (num6 <= 0)
                num6 = 0;
              int num7;
              if (!character2.IsImmune(damageType))
              {
                if (!cardData.GetIgnoreBlock() && !cardData.GetIgnoreBlockBecausePurge())
                {
                  int num8 = character2.GetBlock() - blocked;
                  if (num8 > 0)
                  {
                    if (num8 >= num6)
                    {
                      blocked += num6;
                      num6 = 0;
                    }
                    else
                    {
                      blocked += num8;
                      num6 -= num8;
                    }
                  }
                }
                int num9 = -1 * character2.BonusResists(damageType, cardAux: cardData);
                int dmgTotal = Functions.FuncRoundToInt((float) num6 + (float) ((double) num6 * (double) num9 * 0.0099999997764825821));
                num7 = this.IncreaseDamage(character2, dmgTotal);
              }
              else
                num7 = 0;
              string str = Enum.GetName(typeof (Enums.DamageType), (object) damageType).ToLower();
              if (str == "none")
                str = "heart";
              numArray[index2] = num7;
              strArray[index2] = str;
            }
          }
          int heal2 = this.cardActive.Heal;
          if (heal2 > 0)
          {
            if (this.cardActive.HealSpecialValueGlobal)
              heal2 = Functions.FuncRoundToInt(this.GetCardSpecialValue(cardData, 0, character1, character1, (Character) null, (Character) null, true));
            if (this.cardActive.HealSpecialValue1)
              heal2 = Functions.FuncRoundToInt(this.GetCardSpecialValue(cardData, 1, character1, (Character) null, character2, (Character) null, true));
            if (this.cardActive.HealSpecialValue2)
              heal2 = Functions.FuncRoundToInt(this.GetCardSpecialValue(cardData, 2, character1, (Character) null, character2, (Character) null, true));
            int heal3 = character1.HealWithCharacterBonus(heal2, cardData.CardClass);
            heal1 = character2.HealReceivedFinal(heal3, cardAux: cardData);
          }
          if (numArray[0] != 99999999 || numArray[1] > 0 || heal1 > 0 || blocked > 0 || (UnityEngine.Object) cardData.Aura != (UnityEngine.Object) null || (UnityEngine.Object) cardData.Curse != (UnityEngine.Object) null || cardData.HealCurses > 0 || cardData.DispelAuras > 0 || cardData.StealAuras > 0 || (UnityEngine.Object) cardData.HealAuraCurseSelf != (UnityEngine.Object) null || (UnityEngine.Object) cardData.HealAuraCurseName != (UnityEngine.Object) null || (UnityEngine.Object) cardData.HealAuraCurseName2 != (UnityEngine.Object) null || (UnityEngine.Object) cardData.HealAuraCurseName3 != (UnityEngine.Object) null || (UnityEngine.Object) cardData.HealAuraCurseName4 != (UnityEngine.Object) null)
          {
            if (numArray[0] == 99999999)
              numArray[0] = 0;
            component.SetDamagePreview(true, numArray[0], strArray[0], numArray[1], strArray[1], heal1, blocked, cardData);
          }
        }
      }
    }
  }

  private void DrawDiscardPileCardNumeral()
  {
    this.WriteDiscardCounter(this.CountHeroDiscard().ToString());
  }

  public Vector3 GetDeckPilePosition()
  {
    return (UnityEngine.Object) this.GO_DeckPile != (UnityEngine.Object) null ? this.GO_DeckPile.transform.localPosition : Vector3.zero;
  }

  public Transform GetDeckPileTransform()
  {
    return (UnityEngine.Object) this.GO_DeckPile != (UnityEngine.Object) null ? this.GO_DeckPile.transform : (Transform) null;
  }

  public Vector3 GetDiscardPilePosition()
  {
    return (UnityEngine.Object) this.GO_DeckPile != (UnityEngine.Object) null ? this.GO_DeckPile.transform.localPosition + new Vector3(1.5f, 0.0f, 0.0f) : Vector3.zero;
  }

  public Transform GetDiscardPileTransform()
  {
    return (UnityEngine.Object) this.GO_DiscardPile != (UnityEngine.Object) null ? this.GO_DiscardPile.transform : (Transform) null;
  }

  public Transform GetWorldTransform() => this.worldTransform;

  private void RedoDiscardPileOLD()
  {
    Debug.LogError((object) "REDODISCARDPILE!!!!");
    this.WriteDeckCounter();
    this.WriteDiscardCounter();
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("RedoDiscardPile", "general");
    for (int index = this.GO_DiscardPile.transform.childCount - 1; index >= 0; --index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.GO_DiscardPile.transform.GetChild(index).gameObject);
    Vector3 vector3 = this.GetDiscardPilePosition() + this.GO_DecksObject.transform.position;
    for (int index = 0; index < this.CountHeroDiscard(); ++index)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, this.GO_DiscardPile.transform);
      gameObject.transform.localPosition = this.GetDiscardPilePosition();
      CardItem component = gameObject.GetComponent<CardItem>();
      string id = this.HeroDeckDiscard[this.heroActive][index];
      gameObject.name = id;
      component.SetCard(id, _theHero: this.theHero);
      if (index < this.CountHeroDiscard() - 1)
        component.ShowDiscardImage();
      component.SetDiscardSortingOrder(index);
      component.DisableCollider();
      component.active = false;
      component.enabled = false;
      component.HideRarityParticles();
      component.HideCardIconParticles();
      gameObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
      if (index < this.CountHeroDiscard() - 1)
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, -100f);
    }
  }

  private void RedoDiscardPile()
  {
    this.WriteDeckCounter();
    this.WriteDiscardCounter();
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD(nameof (RedoDiscardPile), "general");
    Vector3 vector3 = this.GetDiscardPilePosition() + this.GO_DecksObject.transform.position;
    this.DestroyDiscardPileChilds();
    for (int index = 0; index < this.CountHeroDiscard(); ++index)
    {
      if (index >= this.CountHeroDiscard() - 1)
      {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, this.GO_DiscardPile.transform);
        gameObject.transform.localPosition = this.GetDiscardPilePosition();
        CardItem component = gameObject.GetComponent<CardItem>();
        string id = this.HeroDeckDiscard[this.heroActive][index];
        gameObject.name = id;
        component.SetCard(id, _theHero: this.theHero);
        if (index < this.CountHeroDiscard() - 1)
          component.ShowDiscardImage();
        component.SetDiscardSortingOrder(index);
        component.DisableCollider();
        component.active = false;
        component.enabled = false;
        component.HideRarityParticles();
        component.HideCardIconParticles();
        gameObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
      }
    }
  }

  public void RedoDiscardPileDepth()
  {
    int childCount = this.GO_DiscardPile.transform.childCount;
    for (int index = 0; index < childCount; ++index)
    {
      if (index < childCount - 2)
      {
        Transform child = this.GO_DiscardPile.transform.GetChild(index);
        child.localPosition = new Vector3(child.localPosition.x, child.localPosition.y, -100f);
      }
    }
  }

  public int GetNPCActive() => this.npcActive;

  public int GetHeroActive() => this.heroActive;

  public Hero GetHeroHeroActive() => this.theHero;

  public Hero GetHero(int _order)
  {
    return _order > -1 && _order < 4 && this.TeamHero[_order] != null ? this.TeamHero[_order] : (Hero) null;
  }

  public Character GetCharacterActive()
  {
    if (this.heroActive > -1 && this.TeamHero[this.heroActive] != null && this.TeamHero[this.heroActive].Alive)
      return (Character) this.TeamHero[this.heroActive];
    return this.npcActive > -1 && this.TeamNPC[this.npcActive] != null && this.TeamNPC[this.npcActive].Alive ? (Character) this.TeamNPC[this.npcActive] : (Character) null;
  }

  public List<Hero> GetHeroSides(int position)
  {
    List<Hero> heroSides = new List<Hero>();
    for (int index = 0; index < this.TeamHero.Length; ++index)
    {
      if (this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroData == (UnityEngine.Object) null) && this.TeamHero[index].Alive && (this.TeamHero[index].Position == position - 1 || this.TeamHero[index].Position == position + 1))
        heroSides.Add(this.TeamHero[index]);
    }
    return heroSides;
  }

  public List<NPC> GetNPCSides(int position)
  {
    List<NPC> npcSides = new List<NPC>();
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("get npc sides pos->" + position.ToString());
    NPC npc1 = (NPC) null;
    NPC npc2 = (NPC) null;
    int num1 = -1;
    int num2 = 5;
    for (int index = 0; index < 4; ++index)
    {
      if (this.TeamNPC[index] != null && this.TeamNPC[index].Alive && this.TeamNPC[index].Position < position && this.TeamNPC[index].Position > num1)
      {
        npc1 = this.TeamNPC[index];
        num1 = this.TeamNPC[index].Position;
      }
    }
    for (int index = 0; index < 4; ++index)
    {
      if (this.TeamNPC[index] != null && this.TeamNPC[index].Alive && this.TeamNPC[index].Position > position && this.TeamNPC[index].Position < num2)
      {
        npc2 = this.TeamNPC[index];
        num2 = this.TeamNPC[index].Position;
      }
    }
    if (npc1 != null)
      npcSides.Add(npc1);
    if (npc2 != null)
      npcSides.Add(npc2);
    return npcSides;
  }

  public void NPCCastCardList(string id, string npcInternalId = "")
  {
    if (npcInternalId == "")
      npcInternalId = this.theNPC.InternalId;
    if (npcInternalId == "")
      return;
    if (!this.npcCardsCasted.ContainsKey(npcInternalId))
      this.npcCardsCasted[npcInternalId] = new List<string>();
    StringBuilder stringBuilder = new StringBuilder(id);
    while (this.npcCardsCasted[npcInternalId].Contains(stringBuilder.ToString()))
      stringBuilder.Append("0");
    this.npcCardsCasted[npcInternalId].Add(stringBuilder.ToString());
  }

  public List<string> GetNPCCardsCastedList(string id)
  {
    return this.npcCardsCasted.ContainsKey(id) ? this.npcCardsCasted[id] : new List<string>();
  }

  public int NPCCastCardTimes(string id)
  {
    if (!this.npcCardsCasted.ContainsKey(this.theNPC.InternalId))
      return 0;
    int num = 0;
    for (int index = 0; index < this.npcCardsCasted[this.theNPC.InternalId].Count - 1; ++index)
    {
      if (this.npcCardsCasted[this.theNPC.InternalId][index] == id)
        ++num;
    }
    return num;
  }

  public void ConsumeAuraCurse(string whenToConsume, Character character, string auraToConsume = "")
  {
    Functions.DebugLogGD("[CONSUMEAURACURSE] " + character?.ToString() + " " + whenToConsume + " " + auraToConsume, "trace");
    if (character != null)
    {
      string key = character.Id + whenToConsume + auraToConsume;
      if ((UnityEngine.Object) character.HeroItem != (UnityEngine.Object) null || (UnityEngine.Object) character.NPCItem != (UnityEngine.Object) null)
      {
        if (this.dictCoroutineConsume.ContainsKey(key))
        {
          if (this.dictCoroutineConsume[key] != null)
            this.StopCoroutine(this.dictCoroutineConsume[key]);
          this.dictCoroutineConsume.Remove(key);
        }
        Coroutine coroutine = this.StartCoroutine(this.ConsumeAuraCurseCo(whenToConsume, character, auraToConsume));
        this.dictCoroutineConsume.Add(key, coroutine);
      }
      else
        this.waitExecution = false;
    }
    else
      Debug.LogError((object) "[CONSUMEAURACURSE] Break because character is null");
  }

  private IEnumerator ConsumeAuraCurseCo(
    string whenToConsume,
    Character character,
    string auraToConsume = "")
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("[CONSUMEAURACURSE] when " + whenToConsume + " aura " + auraToConsume, "trace");
    if (character == null)
    {
      Debug.LogError((object) "[CONSUMEAURACURSE] Break because character is NULL");
      this.waitExecution = false;
    }
    else
    {
      int count = character.AuraList.Count;
      bool isAlive = true;
      int totalconsumed = 0;
      if (count > 0)
      {
        List<Aura> AuraToIncludeList = new List<Aura>();
        bool consumeBlock = true;
        for (int index = 0; index < count; ++index)
        {
          if (index < character.AuraList.Count && character.AuraList[index] != null && (UnityEngine.Object) character.AuraList[index].ACData != (UnityEngine.Object) null && character.AuraList[index].ACData.NoRemoveBlockAtTurnEnd)
            consumeBlock = false;
        }
        List<string> auraConsumed = new List<string>();
        int totalCharAura = character.AuraList.Count;
        for (int i = 0; i < totalCharAura; ++i)
        {
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("consume " + i.ToString() + ". " + character.AuraList[i].ACData.Id, "trace");
          if (i < character.AuraList.Count && character.AuraList[i] != null && !((UnityEngine.Object) character.AuraList[i].ACData == (UnityEngine.Object) null) && !auraConsumed.Contains(character.AuraList[i].ACData.Id) && character.AuraList[i].AuraCharges > 0)
          {
            auraConsumed.Add(character.AuraList[i].ACData.Id);
            if (!character.Alive)
            {
              this.waitExecution = false;
              yield break;
            }
            else
            {
              bool flag1 = false;
              AuraCurseData AC = AtOManager.Instance.GlobalAuraCurseModificationByTraitsAndItems("consume", character.AuraList[i].ACData.Id, character, (Character) null);
              if ((UnityEngine.Object) AC == (UnityEngine.Object) null)
                AC = character.AuraList[i].ACData;
              switch (whenToConsume)
              {
                case "Now":
                  if (AC.Id == auraToConsume.ToLower())
                  {
                    flag1 = true;
                    break;
                  }
                  break;
                case "BeginRound":
                  if (AC.ConsumedAtRoundBegin)
                  {
                    flag1 = true;
                    break;
                  }
                  break;
                case "BeginTurn":
                  if (AC.ConsumedAtTurnBegin)
                    flag1 = true;
                  if (AC.IgnoreUseInEndTurnIfMissingOnBegin)
                  {
                    Dictionary<Character, bool> dictionary;
                    if (!this.auraStates.TryGetValue(AC.Id, out dictionary))
                    {
                      dictionary = new Dictionary<Character, bool>();
                      this.auraStates[AC.Id] = dictionary;
                    }
                    dictionary[character] = true;
                    break;
                  }
                  break;
                case "EndTurn":
                  if (AC.ConsumedAtTurn)
                  {
                    Dictionary<Character, bool> dictionary;
                    bool flag2;
                    flag1 = !AC.IgnoreUseInEndTurnIfMissingOnBegin || ((!this.auraStates.TryGetValue(AC.Id, out dictionary) ? 0 : (dictionary.TryGetValue(character, out flag2) ? 1 : 0)) & (flag2 ? 1 : 0)) != 0;
                    break;
                  }
                  break;
                default:
                  if (whenToConsume == "EndRound" && AC.ConsumedAtRound)
                  {
                    flag1 = true;
                    if (AC.Id == "block" && !consumeBlock)
                    {
                      flag1 = false;
                      break;
                    }
                    break;
                  }
                  break;
              }
              if (flag1)
              {
                if ((AC.ProduceDamageWhenConsumed || AC.ProduceHealWhenConsumed) && !(whenToConsume == "EndRound") && !(whenToConsume == "BeginRound"))
                {
                  if (GameManager.Instance.IsMultiplayer())
                    yield return (object) Globals.Instance.WaitForSeconds(0.35f);
                  else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
                    yield return (object) Globals.Instance.WaitForSeconds(0.4f);
                  else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Ultrafast)
                    yield return (object) Globals.Instance.WaitForSeconds(0.1f);
                  else
                    yield return (object) Globals.Instance.WaitForSeconds(0.3f);
                }
                ++totalconsumed;
                int actCharges = 0;
                if (character.AuraList[i] != null)
                  actCharges = character.AuraList[i].AuraCharges;
                if (AC.EffectTick != "" && (AC.ProduceDamageWhenConsumed || AC.ProduceHealWhenConsumed))
                {
                  if ((UnityEngine.Object) character.NPCItem != (UnityEngine.Object) null)
                    EffectsManager.Instance.PlayEffectAC(AC.EffectTick, false, character.NPCItem.CharImageT, false);
                  else if ((UnityEngine.Object) character.HeroItem != (UnityEngine.Object) null)
                    EffectsManager.Instance.PlayEffectAC(AC.EffectTick, true, character.HeroItem.CharImageT, false);
                }
                if (AC.ProduceDamageWhenConsumed)
                {
                  int num1 = 0 + AC.DamageWhenConsumed;
                  int num2 = actCharges;
                  if ((UnityEngine.Object) AC.ConsumedDamageChargesBasedOnACCharges != (UnityEngine.Object) null)
                    num2 = character.GetAuraCharges(AC.ConsumedDamageChargesBasedOnACCharges.Id);
                  if ((UnityEngine.Object) AC.ConsumeDamageChargesIfACApplied != (UnityEngine.Object) null && character.GetAuraCharges(AC.ConsumeDamageChargesIfACApplied.Id) <= 0)
                    num2 = 0;
                  int damage1 = num1 + Functions.FuncRoundToInt(AC.DamageWhenConsumedPerCharge * (float) num2);
                  if (AC.DamageSidesWhenConsumed > 0 || AC.DamageSidesWhenConsumedPerCharge > 0)
                  {
                    int damage2 = 0 + AC.DamageSidesWhenConsumed + AC.DamageSidesWhenConsumedPerCharge * actCharges;
                    AudioClip sound = (AudioClip) null;
                    if ((UnityEngine.Object) AC.GetSound() != (UnityEngine.Object) null)
                      sound = AC.GetSound();
                    if ((UnityEngine.Object) character.HeroItem != (UnityEngine.Object) null)
                    {
                      List<Hero> heroSides = this.GetHeroSides(character.Position);
                      for (int index = 0; index < heroSides.Count; ++index)
                      {
                        if (heroSides[index] != null)
                        {
                          if (AC.DoubleDamageIfCursesLessThan > 0 && heroSides[index].GetAuraCurseTotal(false, true) < AC.DoubleDamageIfCursesLessThan)
                            damage2 *= 2;
                          heroSides[index].IndirectDamage(AC.DamageTypeWhenConsumed, damage2, sound, AC.Id, character.GameName, character.Id);
                          if (AC.EffectTickSides != "" && (UnityEngine.Object) heroSides[index].HeroItem != (UnityEngine.Object) null)
                            EffectsManager.Instance.PlayEffectAC(AC.EffectTickSides, true, heroSides[index].HeroItem.CharImageT, false, 0.2f);
                        }
                      }
                    }
                    else if ((UnityEngine.Object) character.NPCItem != (UnityEngine.Object) null)
                    {
                      List<NPC> npcSides = this.GetNPCSides(character.Position);
                      for (int index = 0; index < npcSides.Count; ++index)
                      {
                        if (npcSides[index] != null)
                        {
                          if (AC.DoubleDamageIfCursesLessThan > 0 && npcSides[index].GetAuraCurseTotal(false, true) < AC.DoubleDamageIfCursesLessThan)
                            damage2 *= 2;
                          npcSides[index].IndirectDamage(AC.DamageTypeWhenConsumed, damage2, sound, AC.Id, character.GameName, character.Id);
                          if (AC.EffectTickSides != "" && (UnityEngine.Object) npcSides[index].NPCItem != (UnityEngine.Object) null)
                            EffectsManager.Instance.PlayEffectAC(AC.EffectTickSides, false, npcSides[index].NPCItem.CharImageT, false, 0.2f);
                        }
                      }
                    }
                  }
                  if (damage1 > 0)
                  {
                    if (AC.DoubleDamageIfCursesLessThan > 0 && character.GetAuraCurseTotal(false, true) < AC.DoubleDamageIfCursesLessThan)
                      damage1 *= 2;
                    AudioClip sound = (AudioClip) null;
                    if ((UnityEngine.Object) AC.GetSound() != (UnityEngine.Object) null)
                      sound = AC.GetSound();
                    character.IndirectDamage(AC.DamageTypeWhenConsumed, damage1, sound, AC.Id, character.GameName, character.Id);
                    yield return (object) Globals.Instance.WaitForSeconds(0.1f);
                    if (character.HpCurrent <= 0 || !character.Alive)
                    {
                      this.waitExecution = false;
                      yield break;
                    }
                  }
                }
                if (AC.ProduceHealWhenConsumed)
                {
                  int heal1 = 0 + AC.HealWhenConsumed + Functions.FuncRoundToInt((float) actCharges * AC.HealWhenConsumedPerCharge);
                  if (heal1 > 0 && character.GetHpLeftForMax() > 0)
                  {
                    AudioClip sound = (AudioClip) null;
                    if ((UnityEngine.Object) AC.GetSound() != (UnityEngine.Object) null)
                      sound = AC.GetSound();
                    character.IndirectHeal(heal1, sound, AC.Id);
                  }
                  if (AC.HealSidesWhenConsumed != 0 || (double) AC.HealSidesWhenConsumedPerCharge != 0.0)
                  {
                    int heal2 = 0 + AC.HealSidesWhenConsumed + Functions.FuncRoundToInt((float) actCharges * AC.HealSidesWhenConsumedPerCharge);
                    if (heal2 > 0)
                    {
                      if ((UnityEngine.Object) character.HeroItem != (UnityEngine.Object) null)
                      {
                        List<Hero> heroSides = this.GetHeroSides(character.Position);
                        for (int index = 0; index < heroSides.Count; ++index)
                        {
                          if (heroSides[index].GetHpLeftForMax() > 0)
                          {
                            AudioClip sound = (AudioClip) null;
                            if ((UnityEngine.Object) AC.GetSound() != (UnityEngine.Object) null)
                              sound = AC.GetSound();
                            heroSides[index].IndirectHeal(heal2, sound, AC.Id);
                          }
                        }
                      }
                      else if ((UnityEngine.Object) character.NPCItem != (UnityEngine.Object) null)
                      {
                        List<NPC> npcSides = this.GetNPCSides(character.Position);
                        for (int index = 0; index < npcSides.Count; ++index)
                        {
                          if (npcSides[index].GetHpLeftForMax() > 0)
                          {
                            AudioClip sound = (AudioClip) null;
                            if ((UnityEngine.Object) AC.GetSound() != (UnityEngine.Object) null)
                              sound = AC.GetSound();
                            npcSides[index].IndirectHeal(heal2, sound, AC.Id);
                          }
                        }
                      }
                    }
                  }
                }
                if ((UnityEngine.Object) AC.GainAuraCurseConsumption != (UnityEngine.Object) null)
                {
                  int _auraCharges = actCharges * AC.GainAuraCurseConsumptionPerCharge;
                  if ((UnityEngine.Object) AC.GainChargesFromThisAuraCurse != (UnityEngine.Object) null)
                  {
                    _auraCharges += character.EffectCharges(AC.GainChargesFromThisAuraCurse.Id);
                    if (AC.GainChargesFromThisAuraCurse.Id == "fury" && character.HaveTrait("bloody"))
                      _auraCharges = Functions.FuncRoundToInt((float) _auraCharges * 0.5f);
                  }
                  if (_auraCharges > 0)
                  {
                    Aura aura = new Aura();
                    aura.SetAura(AtOManager.Instance.GlobalAuraCurseModificationByTraitsAndItems("set", AC.GainAuraCurseConsumption.Id, (Character) null, character), _auraCharges);
                    AuraToIncludeList.Add(aura);
                  }
                }
                if ((UnityEngine.Object) AC.GainAuraCurseConsumption2 != (UnityEngine.Object) null)
                {
                  int _auraCharges = actCharges * AC.GainAuraCurseConsumptionPerCharge2;
                  if ((UnityEngine.Object) AC.GainChargesFromThisAuraCurse2 != (UnityEngine.Object) null)
                  {
                    _auraCharges += character.EffectCharges(AC.GainChargesFromThisAuraCurse2.Id);
                    if (AC.GainChargesFromThisAuraCurse2.Id == "fury" && character.HaveTrait("bloody"))
                      _auraCharges = Functions.FuncRoundToInt((float) _auraCharges * 0.5f);
                  }
                  if (_auraCharges > 0)
                  {
                    Aura aura = new Aura();
                    aura.SetAura(AtOManager.Instance.GlobalAuraCurseModificationByTraitsAndItems("set", AC.GainAuraCurseConsumption2.Id, (Character) null, character), _auraCharges);
                    AuraToIncludeList.Add(aura);
                  }
                }
                if (i < character.AuraList.Count && character.AuraList[i] != null && AC.Id == character.AuraList[i].ACData.Id)
                {
                  if (AC.ConsumeAll)
                  {
                    character.AuraList[i].AuraCharges = 0;
                    this.ConsumeStatusForCombatStats(character.Id, auraToConsume, -1);
                  }
                  else if (AC.AuraConsumed > 0)
                  {
                    character.AuraList[i].AuraCharges -= AC.AuraConsumed;
                    this.ConsumeStatusForCombatStats(character.Id, AC.Id, AC.AuraConsumed);
                  }
                }
                if (AC.Id == "scourge" && character.GetAuraCharges("dark") > 0)
                {
                  float num = 0.5f;
                  if ((UnityEngine.Object) character.HeroItem != (UnityEngine.Object) null)
                  {
                    List<Hero> heroSides = this.GetHeroSides(character.Position);
                    for (int index = 0; index < heroSides.Count; ++index)
                      heroSides[index].SetAura((Character) null, Globals.Instance.GetAuraCurseData("dark"), Functions.FuncRoundToInt((float) character.GetAuraCharges("dark") * num));
                  }
                  else
                  {
                    if (AtOManager.Instance.TeamHaveTrait("unholyblight"))
                      num = 1f;
                    List<NPC> npcSides = this.GetNPCSides(character.Position);
                    for (int index = 0; index < npcSides.Count; ++index)
                      npcSides[index].SetAura((Character) null, Globals.Instance.GetAuraCurseData("dark"), Functions.FuncRoundToInt((float) character.GetAuraCharges("dark") * num));
                  }
                  if ((UnityEngine.Object) AC.GetSound() != (UnityEngine.Object) null)
                    GameManager.Instance.PlayAudio(AC.GetSound(), 0.5f);
                }
              }
              AC = (AuraCurseData) null;
            }
          }
        }
        if (isAlive)
        {
          for (int index = character.AuraList.Count - 1; index >= 0; --index)
          {
            if (character.AuraList[index] != null && (UnityEngine.Object) character.AuraList[index].ACData != (UnityEngine.Object) null && character.AuraList[index].AuraCharges <= 0)
            {
              if (character.AuraList[index].ACData.DieWhenConsumedAll)
              {
                character.ModifyHp(-character.GetHp());
                break;
              }
              character.AuraList.RemoveAt(index);
            }
          }
        }
        if (AuraToIncludeList.Count > 0)
        {
          for (int index = 0; index < AuraToIncludeList.Count; ++index)
          {
            if (AuraToIncludeList[index] != null)
              character.SetAura(character, AuraToIncludeList[index].ACData, AuraToIncludeList[index].AuraCharges);
          }
        }
        AuraToIncludeList = (List<Aura>) null;
        auraConsumed = (List<string>) null;
      }
      yield return (object) Globals.Instance.WaitForSeconds(0.02f);
      character.UpdateAuraCurseFunctions();
      this.waitExecution = false;
    }
  }

  private float GetCardSpecialValue(
    CardData _cardData,
    int _index,
    Character _casterHero,
    Character _casterNPC,
    Character _targetHero,
    Character _targetNPC,
    bool _IsPreview)
  {
    float num = 0.0f;
    Character character1 = _casterHero == null ? _casterNPC : _casterHero;
    Character character2 = _targetHero == null ? _targetNPC : _targetHero;
    if ((character1 == null || character2 == null) && _index != 0)
      return 0.0f;
    string ACName = "";
    Enums.CardSpecialValue cardSpecialValue1;
    switch (_index)
    {
      case 0:
        cardSpecialValue1 = _cardData.SpecialValueGlobal;
        if ((UnityEngine.Object) _cardData.SpecialAuraCurseNameGlobal != (UnityEngine.Object) null)
        {
          ACName = _cardData.SpecialAuraCurseNameGlobal.Id;
          break;
        }
        break;
      case 1:
        cardSpecialValue1 = _cardData.SpecialValue1;
        if ((UnityEngine.Object) _cardData.SpecialAuraCurseName1 != (UnityEngine.Object) null)
        {
          ACName = _cardData.SpecialAuraCurseName1.Id;
          break;
        }
        break;
      default:
        cardSpecialValue1 = _cardData.SpecialValue2;
        if ((UnityEngine.Object) _cardData.SpecialAuraCurseName2 != (UnityEngine.Object) null)
        {
          ACName = _cardData.SpecialAuraCurseName2.Id;
          break;
        }
        break;
    }
    switch (cardSpecialValue1)
    {
      case Enums.CardSpecialValue.AuraCurseYours:
        if (character1 == null)
          return 0.0f;
        num = (float) character1.GetAuraCharges(ACName);
        break;
      case Enums.CardSpecialValue.AuraCurseTarget:
        if (character2 == null)
          return 0.0f;
        num = (float) character2.GetAuraCharges(ACName);
        break;
      case Enums.CardSpecialValue.CardsHand:
        num = !_IsPreview ? (float) this.handCardsBeforeCast : (float) this.CountHeroHand();
        break;
      case Enums.CardSpecialValue.CardsDeck:
        num = !_IsPreview ? (float) this.deckCardsBeforeCast : (float) this.CountHeroDeck();
        break;
      case Enums.CardSpecialValue.CardsDiscard:
        num = !_IsPreview ? (float) this.discardCardsBeforeCast : (float) this.CountHeroDiscard();
        break;
      case Enums.CardSpecialValue.HealthYours:
        num = (float) character1.GetHp();
        break;
      case Enums.CardSpecialValue.HealthTarget:
        num = (float) character2.GetHp();
        break;
      case Enums.CardSpecialValue.CardsVanish:
        num = !_IsPreview ? (float) this.vanishCardsBeforeCast : (float) this.CountHeroVanish();
        break;
      case Enums.CardSpecialValue.CardsDeckTarget:
        num = _casterHero != _targetHero ? (float) this.CountHeroDeck(_targetHero.HeroIndex) : (!_IsPreview ? (float) this.deckCardsBeforeCast : (float) this.CountHeroDeck());
        break;
      case Enums.CardSpecialValue.CardsDiscardTarget:
        num = _casterHero != _targetHero ? (float) this.CountHeroDiscard(_targetHero.HeroIndex) : (!_IsPreview ? (float) this.discardCardsBeforeCast : (float) this.CountHeroDiscard());
        break;
      case Enums.CardSpecialValue.CardsVanishTarget:
        num = _casterHero != _targetHero ? (float) this.CountHeroVanish(_targetHero.HeroIndex) : (!_IsPreview ? (float) this.vanishCardsBeforeCast : (float) this.CountHeroVanish());
        break;
      case Enums.CardSpecialValue.SpeedYours:
        num = (float) character1.GetSpeed()[0];
        break;
      case Enums.CardSpecialValue.SpeedTarget:
        num = (float) character2.GetSpeed()[0];
        break;
      case Enums.CardSpecialValue.SpeedDifference:
        num = (float) Mathf.Abs(character1.GetSpeed()[0] - character2.GetSpeed()[0]);
        break;
      case Enums.CardSpecialValue.MissingHealthYours:
        num = (float) character1.GetHpLeftForMax();
        break;
      case Enums.CardSpecialValue.MissingHealthTarget:
        num = (float) character2.GetHpLeftForMax();
        break;
      case Enums.CardSpecialValue.DiscardedCards:
        num = (float) this.GlobalDiscardCardsNum;
        break;
      case Enums.CardSpecialValue.VanishedCards:
        num = (float) this.GlobalVanishCardsNum;
        break;
    }
    float cardSpecialValue2;
    switch (_index)
    {
      case 0:
        cardSpecialValue2 = (float) ((double) num * (double) _cardData.SpecialValueModifierGlobal * 0.0099999997764825821);
        break;
      case 1:
        cardSpecialValue2 = (float) ((double) num * (double) _cardData.SpecialValueModifier1 * 0.0099999997764825821);
        break;
      default:
        cardSpecialValue2 = (float) ((double) num * (double) _cardData.SpecialValueModifier2 * 0.0099999997764825821);
        break;
    }
    return cardSpecialValue2;
  }

  public void ShowDeathScreen(Hero _hero)
  {
    this.waitingDeathScreen = true;
    if (TomeManager.Instance.IsActive())
      TomeManager.Instance.ShowTome(false);
    if (CardScreenManager.Instance.IsActive())
      CardScreenManager.Instance.ShowCardScreen(false);
    if (this.console.IsActive())
      this.console.Show(false);
    this.characterWindow.Hide();
    this.HideCharacterWindow();
    PlayerManager.Instance.CardUnlock("Deathsdoor");
    if (!this.deathScreen.IsActive())
    {
      this.deathScreen.SetCharacter(_hero);
      this.deathScreen.SetCard("DeathsDoor");
      if (!GameManager.Instance.IsMultiplayer() || _hero.Owner == NetworkManager.Instance.GetPlayerNick())
      {
        this.deathScreen.TurnOn();
      }
      else
      {
        this.ShowHandMask(false);
        this.deathScreen.TurnOn(false);
      }
    }
    if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
      return;
    if (this.coroutineDeathScreen != null)
      this.StopCoroutine(this.coroutineDeathScreen);
    this.coroutineDeathScreen = this.StartCoroutine(this.DeathScreenOffCo());
  }

  private IEnumerator DeathScreenOffCo()
  {
    yield return (object) Globals.Instance.WaitForSeconds(30f);
    this.DeathScreenOff();
  }

  public void DeathScreenOff()
  {
    if (this.coroutineDeathScreen != null)
      this.StopCoroutine(this.coroutineDeathScreen);
    if (!this.waitingDeathScreen)
      return;
    if (GameManager.Instance.IsMultiplayer())
      this.photonView.RPC("NET_HideDeathScreen", RpcTarget.Others);
    this.deathScreen.TurnOff();
  }

  [PunRPC]
  private void NET_HideDeathScreen() => this.deathScreen.TurnOff();

  public void KillHero(Hero _hero)
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("[KillHero]");
    if (_hero == null || !((UnityEngine.Object) _hero.HeroItem != (UnityEngine.Object) null))
      return;
    this.characterKilled = true;
    _hero.DestroyCharacter();
    if (this.NumHeroesAlive() > 0)
    {
      this.ReDoInitiatives((object) _hero);
      this.RepositionCharacters();
      if (this.theHero != _hero)
        return;
      this.waitExecution = false;
      if (!this.IsYourTurn())
        return;
      this.EndTurn();
    }
    else
      this.FinishCombat();
  }

  public void KillNPC(NPC _npc)
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("[KillNPCCo]");
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("[NPCS live->" + this.NumNPCsAlive().ToString() + "]");
    if (_npc != null && (UnityEngine.Object) _npc.NPCItem != (UnityEngine.Object) null && (UnityEngine.Object) _npc.NpcData != (UnityEngine.Object) null)
    {
      this.characterKilled = true;
      if (MatchManager.CanDestroyIllusion((Character) _npc))
        this.DestroyIllusion((Character) _npc);
      _npc.DestroyCharacter();
      if (_npc.NpcData.IsBoss)
      {
        AtOManager.Instance.BossKilled(_npc.GameName);
        PlayerManager.Instance.BossKilled(_npc.GameName, _npc.NpcData.NPCName, _npc.ScriptableObjectName);
      }
      else
      {
        AtOManager.Instance.MonsterKilled();
        PlayerManager.Instance.MonsterKilled();
      }
      if (_npc.NpcData.Id == "goldenscarab" || _npc.NpcData.Id == "goldenscarab_b" || _npc.NpcData.Id == "goldenscarab_plus" || _npc.NpcData.Id == "goldenscarab_plus_b")
        this.scarabSuccess = !((UnityEngine.Object) this.cardActive != (UnityEngine.Object) null) || !(this.cardActive.Id.Split('_', StringSplitOptions.None)[0].ToLower() == "escapegold") ? "1" : "0";
      else if (_npc.NpcData.Id == "jadescarab" || _npc.NpcData.Id == "jadescarab_b" || _npc.NpcData.Id == "jadescarab_plus" || _npc.NpcData.Id == "jadescarab_plus_b")
        this.scarabSuccess = !((UnityEngine.Object) this.cardActive != (UnityEngine.Object) null) || !(this.cardActive.Id.Split('_', StringSplitOptions.None)[0].ToLower() == "escapejade") ? "1" : "0";
      else if ((_npc.NpcData.Id == "crystalscarab" || _npc.NpcData.Id == "crystalscarab_b" || _npc.NpcData.Id == "crystalscarab_plus" || _npc.NpcData.Id == "crystalscarab_plus_b") && (UnityEngine.Object) this.cardActive != (UnityEngine.Object) null && this.cardActive.Id.Split('_', StringSplitOptions.None)[0].ToLower() == "escapecrystal")
        this.scarabSuccess = "0";
      if (this.NumNPCsAlive() > 0)
      {
        this.ReDoInitiatives((object) _npc);
        this.RepositionCharacters();
        if (_npc.NpcData.FinishCombatOnDead)
        {
          for (int index = 0; index < this.TeamNPC.Length; ++index)
          {
            if (this.TeamNPC[index] != null && !((UnityEngine.Object) this.TeamNPC[index].NpcData == (UnityEngine.Object) null) && this.TeamNPC[index].Alive)
              this.TeamNPC[index].DestroyCharacter();
          }
          this.FinishCombat();
        }
        else
        {
          if (this.theNPC != _npc)
            return;
          this.waitExecution = false;
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("-------------------------------> " + this.gameStatus, "trace");
          if (!(this.gameStatus != "EndTurn") || !(this.gameStatus != "NextTurn"))
            return;
          this.EndTurn();
        }
      }
      else
      {
        if (this.AnyNPCAlive())
          return;
        this.FinishCombat();
      }
    }
    else
      Debug.LogError((object) "KILL NPC error because there's no NPC");
  }

  private void DestroyIllusion(Character currentCharacter)
  {
    if (currentCharacter.IllusionCharacter is Hero illusionCharacter2)
      this.KillHero(illusionCharacter2);
    else if (currentCharacter.IllusionCharacter is NPC illusionCharacter1)
      this.KillNPC(illusionCharacter1);
    currentCharacter.IllusionCharacter.Alive = false;
    currentCharacter.IllusionCharacter.HpCurrent = 0;
    currentCharacter.IllusionCharacter.NpcData = (NPCData) null;
    currentCharacter.IllusionCharacter = (Character) null;
  }

  private static bool CanDestroyIllusion(Character currentCharacter)
  {
    return currentCharacter.IllusionCharacter != null && currentCharacter.IllusionCharacter.Alive;
  }

  public void RepositionCharacters()
  {
    int num1 = 0;
    for (int index = 0; index < this.TeamHero.Length; ++index)
    {
      if (this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroData == (UnityEngine.Object) null) && this.TeamHero[index].Alive)
      {
        this.TeamHero[index].Position = num1;
        ++num1;
      }
    }
    int num2 = 0;
    for (int index = 0; index < this.TeamNPC.Length; ++index)
    {
      if (this.TeamNPC[index] != null && !((UnityEngine.Object) this.TeamNPC[index].NpcData == (UnityEngine.Object) null) && this.TeamNPC[index].Alive)
      {
        this.TeamNPC[index].Position = num2;
        ++num2;
      }
    }
  }

  public Vector3 CharPosition(bool isHero, int position, int totalchars)
  {
    float num1 = 1.6f;
    float y = 0.0f;
    float z = (float) (1.0 - (double) position * 0.10000000149011612);
    float num2 = 1.7f;
    return new Vector3(!isHero ? num1 + (float) position * num2 : (float) (-(double) num1 - (double) position * (double) num2), y, z);
  }

  public bool AnyHeroAlive()
  {
    for (int index = 0; index < this.TeamHero.Length; ++index)
    {
      if (this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroData == (UnityEngine.Object) null) && this.TeamHero[index].Alive)
      {
        if (this.TeamHero[index].HpCurrent > 0)
          return true;
        this.TeamHero[index].Alive = false;
        this.TeamHero[index].UpdateAuraCurseFunctions();
      }
    }
    return false;
  }

  private int NumHeroesAlive()
  {
    int num = 0;
    for (int index = 0; index < this.TeamHero.Length; ++index)
    {
      if (this.TeamHero[index] != null && !((UnityEngine.Object) this.TeamHero[index].HeroData == (UnityEngine.Object) null) && this.TeamHero[index].Alive)
        ++num;
    }
    return num;
  }

  public bool AnyNPCAlive()
  {
    for (int index = 0; index < this.TeamNPC.Length; ++index)
    {
      if (this.TeamNPC[index] != null && !((UnityEngine.Object) this.TeamNPC[index].NpcData == (UnityEngine.Object) null) && this.TeamNPC[index].Alive)
      {
        if (this.TeamNPC[index].HpCurrent > 0)
          return true;
        this.TeamNPC[index].Alive = false;
        this.TeamNPC[index].UpdateAuraCurseFunctions();
      }
    }
    return this.bossNpc != null && this.bossNpc.npc != null && this.bossNpc.npc.HpCurrent > 0;
  }

  public int NumNPCsAlive()
  {
    int num = 0;
    for (int index = 0; index < this.TeamNPC.Length; ++index)
    {
      if (this.TeamNPC[index] != null && !((UnityEngine.Object) this.TeamNPC[index].NpcData == (UnityEngine.Object) null) && this.TeamNPC[index].Alive)
        ++num;
    }
    return num;
  }

  public bool CheckMatchIsOver() => !this.AnyHeroAlive() || !this.AnyNPCAlive();

  public int GetCurrentRound() => this.currentRound;

  public void FinishCombat()
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("----- FINISH COMBAT -- " + this.MatchIsOver.ToString() + " ---");
    if (!this.MatchIsOver)
    {
      this.MatchIsOver = true;
      this.RedrawCardsBorder();
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD(nameof (FinishCombat));
      AtOManager.Instance.combatCardDictionary = (Dictionary<string, CardData>) null;
      AtOManager.Instance.combatGameCode = "";
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("GameBusy TRUE", "gamebusy");
      this.SetGameBusy(true);
      this.waitingKill = false;
      this.gameStatus = nameof (FinishCombat);
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD(this.gameStatus, "gamestatus");
      this.StartCoroutine(this.FinishCombatCo());
      AtOManager.Instance.SirenQueenBattle = false;
    }
    this.mindSpikeAbility.Reset();
  }

  private IEnumerator GoToMainMenu()
  {
    yield return (object) Globals.Instance.WaitForSeconds(3f);
    AlertManager.Instance.HideAlert();
    OptionsManager.Instance.DoExit();
  }

  private IEnumerator FinishCombatCo()
  {
    MatchManager matchManager = this;
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("----- FINISH COMBAT Co -- " + matchManager.MatchIsOver.ToString() + " ---");
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("matchIsOver OK");
    if (matchManager.resignCombat)
    {
      matchManager.newTurnScript.FinishCombat(false);
    }
    else
    {
      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      while (matchManager.waitingDeathScreen)
        yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      if (matchManager.CheckForCrack())
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(Texts.Instance.GetText("crackedVersion"));
        AlertManager.Instance.AlertConfirm(stringBuilder.ToString());
        AlertManager.Instance.ShowDoorIcon();
        matchManager.StartCoroutine(matchManager.GoToMainMenu());
        yield break;
      }
      else if (!matchManager.AnyHeroAlive())
      {
        if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
        {
          if (GameManager.Instance.ConfigRestartCombatOption)
          {
            matchManager.characterWindow.Hide();
            matchManager.HideCharacterWindow();
            AlertManager.Instance.AlertConfirmDouble(Texts.Instance.GetText("combatWantToRetry"));
            AlertManager.buttonClickDelegate = new AlertManager.OnButtonClickDelegate(matchManager.WantToRetry);
            AlertManager.Instance.ShowReloadIcon();
            AlertManager.Instance.SetRestartPosition();
            matchManager.ShowMask(true);
            yield break;
          }
          else if (GameManager.Instance.IsMultiplayer())
          {
            matchManager.photonView.RPC("NET_FinishCombat2", RpcTarget.All);
            yield break;
          }
          else
          {
            matchManager.NET_FinishCombat2();
            yield break;
          }
        }
        else
        {
          AlertManager.Instance.AlertConfirmDouble(Texts.Instance.GetText("combatWantToRetry") + "<size=-6><br><color=#F1D2A9>" + Texts.Instance.GetText("combatWantToRetryClient"), showButtons: false);
          AlertManager.Instance.SetRestartPosition();
          matchManager.ShowMask(true);
          yield break;
        }
      }
    }
    matchManager.StartCoroutine(matchManager.FinishCombat2());
  }

  private bool CheckForCrack() => false;

  [PunRPC]
  public void NET_FinishCombat2()
  {
    AlertManager.Instance.HideAlert();
    this.StartCoroutine(this.FinishCombat2());
  }

  private IEnumerator FinishCombat2()
  {
    MatchManager matchManager = this;
    if (!matchManager.resignCombat)
    {
      matchManager.newTurnScript.FinishCombat(matchManager.AnyHeroAlive());
      AtOManager.Instance.combatScarab = matchManager.scarabSpawned + "%" + matchManager.scarabSuccess;
    }
    if (GameManager.Instance.IsMultiplayer())
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("**************************", "net");
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("WaitingSyncro finishcombat", "net");
      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      if (NetworkManager.Instance.IsMaster())
      {
        if (matchManager.coroutineSyncFinishCombat != null)
          matchManager.StopCoroutine(matchManager.coroutineSyncFinishCombat);
        matchManager.coroutineSyncFinishCombat = matchManager.StartCoroutine(matchManager.ReloadCombatCo("finishcombat"));
        while (!NetworkManager.Instance.AllPlayersReady("finishcombat"))
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (matchManager.coroutineSyncFinishCombat != null)
          matchManager.StopCoroutine(matchManager.coroutineSyncFinishCombat);
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("Game ready, Everybody checked finishcombat", "net");
        NetworkManager.Instance.PlayersNetworkContinue("finishcombat");
      }
      else
      {
        NetworkManager.Instance.SetWaitingSyncro("finishcombat", true);
        NetworkManager.Instance.SetStatusReady("finishcombat");
        while (NetworkManager.Instance.WaitingSyncro["finishcombat"])
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("finishcombat, we can continue!", "net");
      }
    }
    AlertManager.Instance.HideAlert();
    if (!matchManager.resignCombat && matchManager.AnyHeroAlive())
    {
      for (int index = 0; index < matchManager.TeamHero.Length; ++index)
      {
        if (matchManager.TeamHero[index] != null && (UnityEngine.Object) matchManager.TeamHero[index].HeroData != (UnityEngine.Object) null && matchManager.TeamHero[index].Alive)
          matchManager.TeamHero[index].SetEvent(Enums.EventActivation.BeginCombatEnd);
      }
      GameManager.Instance.PlayLibraryAudio("stinger_win_game");
    }
    else
      GameManager.Instance.PlayLibraryAudio("stinger_lose_game");
    AudioManager.Instance.StopAmbience();
    AudioManager.Instance.StopBSO();
  }

  private void WantToRetry()
  {
    int num = AlertManager.Instance.GetConfirmAnswer() ? 1 : 0;
    AlertManager.buttonClickDelegate -= new AlertManager.OnButtonClickDelegate(this.WantToRetry);
    if (num == 0)
    {
      if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
        this.photonView.RPC("NET_FinishCombat2", RpcTarget.All);
      else
        this.NET_FinishCombat2();
    }
    else
    {
      this.MatchIsOver = false;
      this.ReloadCombatFull();
    }
  }

  public void BackToFinishGame()
  {
    if (!this.resignCombat && (UnityEngine.Object) this.combatData != (UnityEngine.Object) null && this.AnyHeroAlive())
    {
      ThermometerTierData thermometerTierData = this.combatData.ThermometerTierData;
      ThermometerData _thermometerData = (ThermometerData) null;
      bool flag = false;
      if ((UnityEngine.Object) thermometerTierData != (UnityEngine.Object) null)
      {
        for (int index = 0; index < thermometerTierData.RoundThermometer.Length; ++index)
        {
          if (this.currentRound >= thermometerTierData.RoundThermometer[index].Round)
          {
            _thermometerData = thermometerTierData.RoundThermometer[index].ThermometerData;
            if (thermometerTierData.RoundThermometer[index].Round > this.currentRound)
              break;
          }
        }
        flag = true;
        AtOManager.Instance.SetCombatExpertise(_thermometerData);
        if (MadnessManager.Instance.IsMadnessTraitActive("equalizer") && (UnityEngine.Object) thermometerTierData != (UnityEngine.Object) null && thermometerTierData.RoundThermometer[1] != null && (UnityEngine.Object) _thermometerData != (UnityEngine.Object) null)
        {
          ThermometerData thermometerData = UnityEngine.Object.Instantiate<ThermometerData>(thermometerTierData.RoundThermometer[1].ThermometerData);
          thermometerData.ThermometerId = _thermometerData.ThermometerId;
          thermometerData.ThermometerColor = _thermometerData.ThermometerColor;
          _thermometerData = thermometerData;
        }
      }
      if (!flag)
        AtOManager.Instance.SetCombatExpertise(_thermometerData);
      AtOManager.Instance.SetCombatThermometerData(_thermometerData);
    }
    this.StopCoroutines();
    AtOManager.Instance.FinishCombat(this.resignCombat);
  }

  public void ResignCombat()
  {
    if (this.resignCombat)
      return;
    if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
    {
      AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("combatOnlyMasterCanResign"));
      AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate) null;
    }
    else
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(Texts.Instance.GetText("combatWantToResign"));
      stringBuilder.Append("<br><size=-4><color=#AAAAAA>");
      stringBuilder.Append(Texts.Instance.GetText("combatWantToResignEnd"));
      stringBuilder.Append("</size></color>");
      AlertManager.buttonClickDelegate = new AlertManager.OnButtonClickDelegate(this.ResignCombatAction);
      AlertManager.Instance.AlertConfirmDouble(stringBuilder.ToString());
      AlertManager.Instance.ShowResignIcon();
    }
  }

  private void ResignCombatAction()
  {
    int num = AlertManager.Instance.GetConfirmAnswer() ? 1 : 0;
    AlertManager.buttonClickDelegate -= new AlertManager.OnButtonClickDelegate(this.ResignCombatAction);
    if (num == 0)
      return;
    if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
      this.photonView.RPC("NET_ResignCombatActionExecute", RpcTarget.Others);
    this.StartCoroutine(this.ResignCombatActionExecute());
  }

  [PunRPC]
  public void NET_ResignCombatActionExecute()
  {
    this.StartCoroutine(this.ResignCombatActionExecute());
  }

  private IEnumerator ResignCombatActionExecute()
  {
    MatchManager matchManager = this;
    AtOManager.Instance.combatCardDictionary = (Dictionary<string, CardData>) null;
    AtOManager.Instance.combatGameCode = "";
    matchManager.resignCombat = true;
    if (GameManager.Instance.IsMultiplayer())
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("**************************", "net");
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("WaitingSyncro resign", "net");
      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      if (NetworkManager.Instance.IsMaster())
      {
        if (matchManager.coroutineSyncResign != null)
          matchManager.StopCoroutine(matchManager.coroutineSyncResign);
        matchManager.coroutineSyncResign = matchManager.StartCoroutine(matchManager.ReloadCombatCo("resign"));
        while (!NetworkManager.Instance.AllPlayersReady("resign"))
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (matchManager.coroutineSyncResign != null)
          matchManager.StopCoroutine(matchManager.coroutineSyncResign);
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("Game ready, Everybody checked resign", "net");
        NetworkManager.Instance.PlayersNetworkContinue("resign");
      }
      else
      {
        NetworkManager.Instance.SetWaitingSyncro("resign", true);
        NetworkManager.Instance.SetStatusReady("resign");
        while (NetworkManager.Instance.WaitingSyncro["resign"])
          yield return (object) Globals.Instance.WaitForSeconds(0.01f);
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("resign, we can continue!", "net");
      }
    }
    for (int index = 0; index < matchManager.TeamHero.Length; ++index)
    {
      if (matchManager.TeamHero[index] != null && !((UnityEngine.Object) matchManager.TeamHero[index].HeroData == (UnityEngine.Object) null))
        matchManager.TeamHero[index].Alive = false;
    }
    matchManager.FinishCombat();
  }

  private void SetCardsWaitingForReset(int _num) => this.cardsWaitingForReset = _num;

  private void ShowHandMask(bool state)
  {
    if (this.HandMask.gameObject.activeSelf == state)
      return;
    this.HandMask.gameObject.SetActive(state);
  }

  private void ShowMaskFull()
  {
    RawImage component = this.MaskWindow.GetChild(0).transform.GetComponent<RawImage>();
    this.MaskWindow.gameObject.SetActive(true);
    Color color = new Color(0.0f, 0.0f, 0.0f, 1f);
    component.color = color;
  }

  private void HideMaskFull() => this.StartCoroutine(this.HideMaskFullCo());

  private IEnumerator HideMaskFullCo()
  {
    RawImage imageBg = this.MaskWindow.GetChild(0).transform.GetComponent<RawImage>();
    float index = imageBg.color.a;
    while ((double) index > 0.800000011920929)
    {
      imageBg.color = new Color(0.0f, 0.0f, 0.0f, index);
      index -= 0.025f;
      yield return (object) null;
    }
  }

  public void ShowMaskFromUIScreen(bool state)
  {
    RawImage component = this.MaskWindow.GetChild(0).transform.GetComponent<RawImage>();
    if (state)
    {
      component.color = new Color(0.0f, 0.0f, 0.0f, 0.8f);
      this.MaskWindow.GetComponent<BoxCollider2D>().enabled = true;
    }
    else
    {
      component.color = new Color(0.0f, 0.0f, 0.0f, 0.15f);
      this.MaskWindow.GetComponent<BoxCollider2D>().enabled = false;
    }
  }

  public void ShowMask(bool state, bool hardMask = true)
  {
    if (this.lockHideMask)
      return;
    if (this.coroutineMask != null)
      this.StopCoroutine(this.coroutineMask);
    this.coroutineMask = this.StartCoroutine(this.ShowMaskCo(state, hardMask));
  }

  private IEnumerator ShowMaskCo(bool state, bool hardMask)
  {
    float maxAlplha = 0.8f;
    if (!hardMask)
      maxAlplha = 0.25f;
    RawImage imageBg = this.MaskWindow.GetChild(0).transform.GetComponent<RawImage>();
    float index = imageBg.color.a;
    if (!state)
    {
      yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      while ((double) index > 0.0)
      {
        imageBg.color = new Color(0.0f, 0.0f, 0.0f, index);
        index -= 0.05f;
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      }
      imageBg.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
      this.MaskWindow.gameObject.SetActive(false);
    }
    else
    {
      this.MaskWindow.gameObject.SetActive(true);
      while ((double) index < (double) maxAlplha)
      {
        imageBg.color = new Color(0.0f, 0.0f, 0.0f, index);
        index += 0.05f;
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      }
      imageBg.color = new Color(0.0f, 0.0f, 0.0f, maxAlplha);
    }
  }

  public void SetAuraIterator(
    Character target,
    Character theCaster,
    AuraCurseData _acData,
    int charges,
    bool fromTrait = false)
  {
    this.StartCoroutine(this.SetAuraIteratorCo(target, theCaster, _acData, charges, fromTrait));
  }

  private IEnumerator SetAuraIteratorCo(
    Character target,
    Character theCaster,
    AuraCurseData _acData,
    int charges,
    bool fromTrait = false)
  {
    yield return (object) Globals.Instance.WaitForSeconds(0.01f);
    if (target != null && target.Alive)
      target.SetAura(theCaster, _acData, charges, fromTrait);
  }

  private void GiveExhaust()
  {
    if (this.isBeginTournPhase || !AtOManager.Instance.IsChallengeTraitActive("exhaustion") && AtOManager.Instance.GetNgPlus() <= 1 && AtOManager.Instance.GetSingularityMadness() <= 3 || this.heroActive <= -1 || this.TeamHero[this.heroActive] == null || !this.TeamHero[this.heroActive].Alive)
      return;
    this.TeamHero[this.heroActive].SetAura((Character) null, Globals.Instance.GetAuraCurseData("Exhaust"), 1);
    this.DoExhaust();
  }

  public void DoExhaust()
  {
    if (!AtOManager.Instance.IsChallengeTraitActive("exhaustion") && AtOManager.Instance.GetNgPlus() <= 1 && AtOManager.Instance.GetSingularityMadness() <= 3)
      return;
    if (this.heroActive > -1 && this.TeamHero[this.heroActive] != null && this.TeamHero[this.heroActive].Alive && this.TeamHero[this.heroActive].HasEffect("Exhaust"))
    {
      this.ShowExhaust();
      this.exhaustNumber.text = this.TeamHero[this.heroActive].GetAuraCharges("Exhaust").ToString();
    }
    else
      this.HideExhaust();
  }

  public void ShowExhaust()
  {
    if (this.exhaustT.gameObject.activeSelf)
      return;
    this.exhaustT.gameObject.SetActive(true);
  }

  public void HideExhaust()
  {
    if (!this.exhaustT.gameObject.activeSelf)
      return;
    this.exhaustT.gameObject.SetActive(false);
  }

  private void ResetItemTimeout()
  {
    for (int index = 0; index < this.itemTimeout.Length; ++index)
      this.itemTimeout[index] = 0.0f;
  }

  public void DoItem(
    Character caller,
    Enums.EventActivation theEvent,
    CardData cardData,
    string item,
    Character target,
    int auxInt,
    string auxString,
    int timesActivated = 0)
  {
    this.ResetAutoEndCount();
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("match do item " + theEvent.ToString() + " -> " + item, nameof (item));
    bool flag1 = false;
    if ((UnityEngine.Object) cardData != (UnityEngine.Object) null && (UnityEngine.Object) cardData.Item != (UnityEngine.Object) null && (cardData.Item.DrawCards > 0 || cardData.Item.CardNum > 0 || cardData.Item.CardsReduced > 0))
      flag1 = true;
    if ((UnityEngine.Object) cardData != (UnityEngine.Object) null && (UnityEngine.Object) cardData.ItemEnchantment != (UnityEngine.Object) null && (cardData.ItemEnchantment.DrawCards > 0 || cardData.ItemEnchantment.CardNum > 0 || cardData.ItemEnchantment.CardsReduced > 0))
      flag1 = true;
    if (theEvent == Enums.EventActivation.CorruptionBeginRound || theEvent == Enums.EventActivation.CorruptionCombatStart)
      flag1 = false;
    if (!flag1 || this.ctQueue.Count == 0 && this.eventList.Count == 0)
    {
      this.StartCoroutine(this.DoItemCo(caller, theEvent, cardData, item, target, auxInt, auxString, timesActivated));
    }
    else
    {
      bool flag2 = false;
      if (this.eventList.Count > 0)
      {
        for (int index = 0; index < this.eventList.Count; ++index)
        {
          if (this.eventList[index].StartsWith("item->"))
            flag2 = true;
        }
      }
      if (flag2)
      {
        this.ctQueue.Enqueue((Action) (() => this.StartCoroutine(this.DoItemCo(caller, theEvent, cardData, item, target, auxInt, auxString, timesActivated))));
        if (this.checkQueueCo != null)
          this.StopCoroutine(this.checkQueueCo);
        this.checkQueueCo = this.StartCoroutine(this.CheckDeQueue());
      }
      else
        this.StartCoroutine(this.DoItemCo(caller, theEvent, cardData, item, target, auxInt, auxString, timesActivated));
    }
  }

  private void ClearItemQueue()
  {
    for (int index = this.eventList.Count - 1; index >= 0; --index)
    {
      if (this.eventList[index].StartsWith("item->"))
        this.eventList.RemoveAt(index);
    }
    this.ctQueue.Clear();
  }

  private IEnumerator CheckDeQueue()
  {
    int cleanList = 0;
    int exhaustIndex = 0;
    while (this.ctQueue.Count > 0)
    {
      while (this.waitingDeathScreen)
      {
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[CheckDeQueue] waitingDeathScreen");
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      }
      while (this.waitingKill)
      {
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[CheckDeQueue] waitingKill");
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      }
      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      if (this.eventList.Count == 0 && this.ctQueue.Count > 0)
      {
        if (cleanList >= 1)
        {
          this.ctQueue.Dequeue()();
          cleanList = 0;
        }
        else
          ++cleanList;
        exhaustIndex = 0;
      }
      else
      {
        cleanList = 0;
        if (this.eventList.Count == 1 && this.eventList[0].StartsWith("CastCardEvent"))
        {
          ++exhaustIndex;
          if (exhaustIndex >= 100)
          {
            Debug.LogError((object) "DeQUEUE B");
            this.ctQueue.Dequeue()();
            cleanList = 0;
          }
        }
        else
          exhaustIndex = 0;
      }
    }
  }

  private NPC GetRandomNPC()
  {
    NPC[] teamNpc = MatchManager.Instance.GetTeamNPC();
    List<int> intList = new List<int>();
    for (int index = 0; index < teamNpc.Length; ++index)
    {
      if (teamNpc[index] != null && teamNpc[index].Alive)
        intList.Add(index);
    }
    if (intList.Count > 0)
    {
      bool flag = false;
      int index = MatchManager.Instance.GetRandomIntRange(0, intList.Count, "item");
      int num = 0;
      while (!flag)
      {
        if (teamNpc[intList[index]] != null && teamNpc[intList[index]].Alive)
          return teamNpc[intList[index]];
        index = (index + 1) % teamNpc.Length;
        ++num;
        if (num > teamNpc.Length)
          return (NPC) null;
      }
    }
    return (NPC) null;
  }

  private IEnumerator DoItemCo(
    Character caller,
    Enums.EventActivation theEvent,
    CardData cardData,
    string item,
    Character target,
    int auxInt,
    string auxString,
    int timesActivated = 0)
  {
    this.SetEventDirect("item->" + item, false, true);
    CardData itemData = cardData;
    CardData _castedCard = caller.CardCasted;
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("[DoItemCo] doitemco -> " + theEvent.ToString() + " -> " + item + " " + timesActivated.ToString() + " " + itemData?.ToString(), nameof (item));
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("[DoItemCo] Delay for item " + (0.1f * (float) timesActivated).ToString(), nameof (item));
    yield return (object) Globals.Instance.WaitForSeconds(0.2f * (float) timesActivated);
    while (this.eventList.Contains("ResetDeck"))
      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
    if (caller != null && !caller.IsHero && theEvent == Enums.EventActivation.BeginTurn)
    {
      if (!this.activationItemsAtBeginTurnList.Contains(item))
        this.activationItemsAtBeginTurnList.Add(item);
      do
      {
        List<string> itemsAtBeginTurnList = this.activationItemsAtBeginTurnList;
        if (itemsAtBeginTurnList != null && itemsAtBeginTurnList.Count > 0 && this.activationItemsAtBeginTurnList[0] != item)
          yield return (object) Globals.Instance.WaitForSeconds(0.1f);
        else
          goto label_16;
      }
      while (caller != null && caller.Alive);
      this.activationItemsAtBeginTurnList.Clear();
label_16:
      while (this.waitingKill)
      {
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[DoItem] waitingKill");
        yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      }
      if (this.activationItemsAtBeginTurnList != null && this.activationItemsAtBeginTurnList.Count > 0 && this.activationItemsAtBeginTurnList[0] == item)
      {
        if (this.activationItemsAtBeginTurnList.Count > 0)
          yield return (object) Globals.Instance.WaitForSeconds(0.2f);
        this.activationItemsAtBeginTurnList.RemoveAt(0);
      }
    }
    if ((bool) (UnityEngine.Object) itemData.ItemEnchantment && itemData.ItemEnchantment.HealBasedOnAuraCurse > 0 && (UnityEngine.Object) itemData.ItemEnchantment.AuraCurseSetted != (UnityEngine.Object) null)
      caller.ModifyHp(itemData.ItemEnchantment.HealBasedOnAuraCurse * caller.GetAuraCharges(itemData.ItemEnchantment.AuraCurseSetted.Id));
    int exhaustEvent = 0;
    if (theEvent == Enums.EventActivation.FinishCast)
    {
      for (; exhaustEvent < 700 && this.cardsWaitingForReset > 0; ++exhaustEvent)
      {
        if (exhaustEvent > 0 && exhaustEvent % 100 == 0)
          Debug.Log((object) ("[DoItemCo] ************************ XXX " + this.cardsWaitingForReset.ToString()));
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      }
      for (exhaustEvent = 0; exhaustEvent < 700 && this.NumChildsInTemporal() > 0; ++exhaustEvent)
      {
        if (exhaustEvent > 0 && exhaustEvent % 100 == 0)
          Debug.Log((object) ("[DoItemCo] ************************ YYY " + this.NumChildsInTemporal().ToString()));
        yield return (object) Globals.Instance.WaitForSeconds(0.01f);
      }
    }
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("[DoItemCo] doitemco -> " + theEvent.ToString() + " -> " + item + " " + timesActivated.ToString() + " " + itemData?.ToString(), nameof (item));
    if (!this.IsBeginTournPhase || theEvent != Enums.EventActivation.Killed)
    {
      while (this.waitingKill)
      {
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[DoItemCo] waitingKill", "general");
        yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      }
      while (this.waitingDeathScreen)
      {
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[DoItemCo] waitingDeathScreen", "general");
        yield return (object) Globals.Instance.WaitForSeconds(0.1f);
      }
      while (this.cardsWaitingForReset > 0)
        yield return (object) Globals.Instance.WaitForSeconds(0.1f);
    }
    exhaustEvent = 0;
    if (this.IsBeginTournPhase && theEvent != Enums.EventActivation.Killed)
    {
      for (; this.NumChildsInTemporal() > 0 && exhaustEvent < 20; ++exhaustEvent)
        yield return (object) Globals.Instance.WaitForSeconds(0.1f);
    }
    if (!caller.Alive && theEvent != Enums.EventActivation.Killed)
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("[DoItemCo] break because Hero died", "general");
      this.SetEventDirect("item->" + item, false);
    }
    else if (this.MatchIsOver && theEvent != Enums.EventActivation.BeginCombatEnd)
    {
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("[DoItemCo] Broken by finish game", "general");
      this.SetEventDirect("item->" + item, false);
    }
    else
    {
      string _key = "";
      if (cardData.CardType == Enums.CardType.Corruption && theEvent != Enums.EventActivation.CorruptionBeginRound)
      {
        _key = this.logDictionary.Count.ToString();
        this.CreateLogEntry(true, _key, item, this.theHero, this.theNPC, (Hero) null, (NPC) null, this.currentRound, Enums.EventActivation.CorruptionBeginRound);
      }
      caller.DoItem(theEvent, itemData, item, target, auxInt, auxString, timesActivated, _castedCard);
      if (cardData.CardType == Enums.CardType.Corruption && theEvent != Enums.EventActivation.CorruptionBeginRound)
        this.CreateLogEntry(false, _key, item, this.theHero, this.theNPC, (Hero) null, (NPC) null, this.currentRound, Enums.EventActivation.CorruptionBeginRound);
      ItemData itemEnchantment = cardData.ItemEnchantment;
      if ((itemEnchantment != null ? (itemEnchantment.Activation == Enums.EventActivation.EndRound ? 1 : 0) : 0) != 0 && this.CanApplyNightmareImage(cardData, caller))
        yield return (object) this.ApplyNightmareImageCo(cardData, caller);
      this.ResetAutoEndCount();
      this.SetEventDirect("item->" + item, false);
    }
  }

  public bool CardRandomRarityOk(int rarity, CardData cardData)
  {
    return rarity < 80 && cardData.CardUpgraded == Enums.CardUpgraded.No || rarity >= 80 && rarity < 90 && cardData.CardUpgraded == Enums.CardUpgraded.A || rarity >= 90 && cardData.CardUpgraded == Enums.CardUpgraded.B;
  }

  public void DoComic(Character _character, string _text, float duration = 3f)
  {
    if ((UnityEngine.Object) this.comicGO != (UnityEngine.Object) null || _character == null)
      return;
    bool flag = true;
    Vector3 vector3 = Vector3.zero;
    Transform animatedTransform;
    if ((UnityEngine.Object) _character.HeroItem != (UnityEngine.Object) null)
    {
      vector3 = new Vector3(_character.HeroData.HeroSubClass.FluffOffsetX, _character.HeroData.HeroSubClass.FluffOffsetY, 0.0f);
      animatedTransform = _character.HeroItem.animatedTransform;
    }
    else
    {
      if (!((UnityEngine.Object) _character.NPCItem != (UnityEngine.Object) null))
        return;
      flag = false;
      vector3 = new Vector3(_character.NpcData.FluffOffsetX, _character.NpcData.FluffOffsetY, 0.0f);
      animatedTransform = _character.NPCItem.animatedTransform;
    }
    this.comicGO = UnityEngine.Object.Instantiate<GameObject>(this.comicPrefab, Vector3.zero, Quaternion.identity, animatedTransform);
    this.comicGO.transform.localScale = (double) animatedTransform.localScale.x >= 0.0 ? new Vector3(1f, this.comicGO.transform.localScale.y, this.comicGO.transform.localScale.z) : new Vector3(-1f, this.comicGO.transform.localScale.y, this.comicGO.transform.localScale.z);
    this.comicGO.transform.localPosition = Vector3.zero + vector3;
    if (!flag)
      this.comicGO.transform.GetChild(1).GetComponent<SpriteRenderer>().flipX = true;
    this.comicGO.transform.GetChild(0).GetComponent<TMP_Text>().text = _text;
    this.StartCoroutine(this.HideComic(duration));
  }

  private IEnumerator HideComic(float duration = 0.0f)
  {
    if ((UnityEngine.Object) this.comicGO != (UnityEngine.Object) null)
    {
      yield return (object) Globals.Instance.WaitForSeconds(duration);
      if ((UnityEngine.Object) this.comicGO != (UnityEngine.Object) null)
      {
        if ((double) duration > 0.0)
        {
          this.comicGO.transform.GetComponent<Animator>().SetTrigger("hide");
          yield return (object) Globals.Instance.WaitForSeconds(1.3f);
        }
        UnityEngine.Object.Destroy((UnityEngine.Object) this.comicGO);
        this.comicGO = (GameObject) null;
      }
    }
    yield return (object) null;
  }

  public void GenerateRandomStringBatch(int total = -1)
  {
    if (this.resultSeed == -1)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(AtOManager.Instance.currentMapNode);
      stringBuilder.Append(AtOManager.Instance.GetGameId());
      if ((UnityEngine.Object) this.combatData != (UnityEngine.Object) null)
        stringBuilder.Append(this.combatData.CombatId);
      this.resultSeed = stringBuilder.ToString().GetDeterministicHashCode();
    }
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("GenerateRandomStringBatch =>" + this.resultSeed.ToString(), "randomindex");
    UnityEngine.Random.InitState(this.resultSeed);
    if (total == -1)
      total = this.randomStringArrLength;
    this.randomStringArr.Clear();
    for (int index = 0; index < total; ++index)
      this.randomStringArr.Add(Functions.RandomString(6f));
    this.randomStringItemsArr.Clear();
    for (int index = 0; index < total; ++index)
      this.randomStringItemsArr.Add(Functions.RandomString(6f));
    this.randomStringTraitsArr.Clear();
    for (int index = 0; index < total; ++index)
      this.randomStringTraitsArr.Add(Functions.RandomString(6f));
    this.randomStringDeckArr.Clear();
    for (int index = 0; index < total; ++index)
      this.randomStringDeckArr.Add(Functions.RandomString(6f));
    this.randomStringShuffleArr.Clear();
    for (int index = 0; index < total; ++index)
      this.randomStringShuffleArr.Add(Functions.RandomString(6f));
    this.randomStringMiscArr.Clear();
    for (int index = 0; index < total; ++index)
      this.randomStringMiscArr.Add(Functions.RandomString(6f));
  }

  private void SetRandomIndex(int _value, string _type = "")
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("SetRandomIndex -> " + _value.ToString(), "randomindex");
    switch (_type)
    {
      case "":
        this.randomIndex = _value;
        this.randomItemsIndex = _value;
        this.randomTraitsIndex = _value;
        this.randomDeckIndex = _value;
        this.randomShuffleIndex = _value;
        this.randomMiscIndex = _value;
        break;
      case "shuffle":
        this.randomShuffleIndex = _value;
        break;
    }
  }

  public void GenerateRandomIndex(bool share = true)
  {
    Debug.Log((object) ("GenerateRandomIndex " + share.ToString()));
    UnityEngine.Random.InitState((int) DateTime.Now.Ticks);
    this.SetRandomIndex(UnityEngine.Random.Range(100, 500));
    if (!share)
      return;
    this.ShareRandomIndex();
  }

  private void ShareRandomIndex()
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("ShareRandomIndex -> " + this.randomIndex.ToString(), "randomindex");
    this.photonView.RPC("NET_SendRandomSeed", RpcTarget.Others, (object) this.randomIndex);
  }

  [PunRPC]
  private void NET_SendRandomSeed(int _index) => this.SetRandomIndex(_index);

  private void SendRandomStringBatch()
  {
    this.photonView.RPC("NET_SetRandomStringBatch", RpcTarget.Others, (object) Functions.CompressString(JsonHelper.ToJson<string>(this.randomStringArr.ToArray())));
  }

  [PunRPC]
  private void NET_SetRandomStringBatch(string _arr)
  {
    this.randomStringArr = ((IEnumerable<string>) JsonHelper.FromJson<string>(Functions.DecompressString(_arr))).ToList<string>();
  }

  public string GetRandomString(string type = "default")
  {
    string randomString = "";
    switch (type)
    {
      case "default":
        if (this.randomIndex >= this.randomStringArr.Count)
          this.randomIndex = 0;
        randomString = this.randomStringArr[this.randomIndex];
        ++this.randomIndex;
        break;
      case "item":
        if (this.randomItemsIndex >= this.randomStringItemsArr.Count)
          this.randomItemsIndex = 0;
        randomString = this.randomStringItemsArr[this.randomItemsIndex];
        ++this.randomItemsIndex;
        break;
      case "trait":
        if (this.randomTraitsIndex >= this.randomStringTraitsArr.Count)
          this.randomTraitsIndex = 0;
        randomString = this.randomStringTraitsArr[this.randomTraitsIndex];
        ++this.randomTraitsIndex;
        break;
      case "deck":
        if (this.randomDeckIndex >= this.randomStringDeckArr.Count)
          this.randomDeckIndex = 0;
        randomString = this.randomStringDeckArr[this.randomDeckIndex];
        ++this.randomDeckIndex;
        break;
      case "shuffle":
        if (this.randomShuffleIndex >= this.randomStringShuffleArr.Count)
          this.randomShuffleIndex = 0;
        randomString = this.randomStringShuffleArr[this.randomShuffleIndex];
        ++this.randomShuffleIndex;
        break;
      case "misc":
        if (this.randomMiscIndex >= this.randomStringMiscArr.Count)
          this.randomMiscIndex = 0;
        randomString = this.randomStringMiscArr[this.randomMiscIndex];
        ++this.randomMiscIndex;
        break;
    }
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("GRS " + type + "->" + randomString, "randomindex");
    return randomString;
  }

  public int GetRandomIntRange(int min, int max, string type = "default", string seed = "")
  {
    if (seed == "")
      seed = this.GetRandomString(type);
    return Functions.Random(min, max, seed);
  }

  public int GetRandomIntRangeOLD(int min, int max, string type = "default", string randomStr = "")
  {
    if (min == max)
      return min;
    string str = randomStr;
    if (str == "")
      str = this.GetRandomString(type);
    long num1;
    if (min == 0 && max == 2 && (type == "item" || type == "deck"))
    {
      num1 = (long) str[0];
    }
    else
    {
      int num2 = 0;
      for (int index = 0; index < str.Length; ++index)
      {
        if (str[index] == ' ')
          ++num2;
      }
      long[] sumArr = new long[num2 + 1];
      num1 = Functions.ASCIIWordSum(str, sumArr);
    }
    return min + Mathf.FloorToInt((float) (num1 % (long) (max - min)));
  }

  public void ShowCharacterWindow(string type = "", bool isHero = true, int characterIndex = -1)
  {
    if (isHero)
    {
      this.sideCharacters.Show();
      this.sideCharacters.ShowActiveStatus(characterIndex);
      this.characterWindow.Show(type, characterIndex);
    }
    else
      this.characterWindow.Show(type, characterIndex, false);
  }

  public void HideCharacterWindow() => this.sideCharacters.Hide();

  private void GenerateSyncCodeDict()
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD(nameof (GenerateSyncCodeDict), "general");
    this.syncCodeDict.Clear();
    foreach (Player player in NetworkManager.Instance.PlayerList)
      this.syncCodeDict.Add(player.NickName, "");
  }

  private int CheckSyncCodeDict()
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD(nameof (CheckSyncCodeDict), "general");
    foreach (KeyValuePair<string, string> keyValuePair in this.syncCodeDict)
    {
      if (keyValuePair.Value == "")
      {
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("CheckSyncCodeDict => 0", "general");
        return 0;
      }
    }
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("CheckSyncCodeDict => 1", "general");
    return 1;
  }

  private void SetMasterSyncCode(bool md5 = false)
  {
    this.currentGameCode = this.GenerateSyncCode(true);
    if (!GameManager.Instance.IsMultiplayer())
      return;
    string str = this.GenerateSyncCode();
    if (md5)
      str = Functions.Md5Sum(str);
    this.InsertSyncCode(NetworkManager.Instance.GetPlayerNick(), str);
  }

  private void RequestSyncCode()
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD(nameof (RequestSyncCode));
    this.photonView.RPC("NET_RequestSyncCode", RpcTarget.Others);
  }

  [PunRPC]
  private void NET_RequestSyncCode() => this.SendSyncCode();

  private void SendSyncCode(bool md5 = false)
  {
    string str1 = this.GenerateSyncCode();
    if (md5)
      str1 = Functions.Md5Sum(str1);
    string str2 = Functions.CompressString(str1);
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("SendSyncCode " + str2);
    this.photonView.RPC("NET_SendSyncCode", RpcTarget.MasterClient, (object) NetworkManager.Instance.GetPlayerNick(), (object) str2);
  }

  [PunRPC]
  private void NET_SendSyncCode(string _nick, string _code)
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("SYNC CODE received from " + _nick, "synccode");
    string _code1 = Functions.DecompressString(_code);
    this.InsertSyncCode(_nick, _code1);
  }

  private void InsertSyncCode(string _nick, string _code)
  {
    if (!this.syncCodeDict.ContainsKey(_nick))
      this.syncCodeDict.Add(_nick, _code);
    else
      this.syncCodeDict[_nick] = _code;
  }

  public void ALL_BreakByDesync()
  {
    if (this.MatchIsOver || GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
      return;
    AlertManager.Instance.AlertConfirmDouble(Texts.Instance.GetText("reloadForced"));
    AlertManager.Instance.ShowReloadIcon();
    AlertManager.buttonClickDelegate = new AlertManager.OnButtonClickDelegate(this.ReloadCombatFull);
  }

  private void ReloadCombatFull()
  {
    if (this.MatchIsOver)
      return;
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("---------> ReloadCombatFull <---------", "general");
    int num = AlertManager.Instance.GetConfirmAnswer() ? 1 : 0;
    AlertManager.buttonClickDelegate -= new AlertManager.OnButtonClickDelegate(this.ReloadCombatFull);
    if (num == 0 || this.reloadingGame)
      return;
    if (this.coroutineSync != null)
      this.StopCoroutine(this.coroutineSync);
    this.StopCoroutines();
    AtOManager.Instance.DeleteSaveGameTurn();
    this.ReloadCombatFullAction();
  }

  private void ReloadCombatFullAction()
  {
    for (int key = 0; key < 4; ++key)
    {
      if (this.TeamHero[key] != null)
      {
        this.TeamHero[key].HealToMax();
        this.TeamHero[key].Alive = true;
        if (key < this.heroBeginItems.Count && this.heroBeginItems[key] != null)
        {
          List<string> heroBeginItem = this.heroBeginItems[key];
          this.TeamHero[key].Weapon = heroBeginItem[0];
          this.TeamHero[key].Armor = heroBeginItem[1];
          this.TeamHero[key].Jewelry = heroBeginItem[2];
          this.TeamHero[key].Accesory = heroBeginItem[3];
          this.TeamHero[key].Pet = heroBeginItem[4];
        }
      }
    }
    AtOManager.Instance.combatCardDictionary = (Dictionary<string, CardData>) null;
    AtOManager.Instance.combatGameCode = "";
    this.currentGameCode = "";
    this.heroDestroyedItemsInThisTurn.Clear();
    this.heroBeginItems.Clear();
    this.heroLifeArr = (int[]) null;
    if (GameManager.Instance.IsMultiplayer())
      AtOManager.Instance.DoLoadGameFromMP(true);
    else
      AtOManager.Instance.LoadGame(comingFromReloadCombat: true);
  }

  private IEnumerator ReloadCombatCo(string from)
  {
    yield return (object) Globals.Instance.WaitForSeconds(12f);
    this.ReloadCombat(from);
  }

  private void ReloadCombat(string from = "")
  {
    if (this.reloadingGame)
      return;
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("---------> RELOAD COMBAT " + from + " <----------", "reload");
    this.reloadingGame = true;
    this.characterWindow.Hide();
    this.StopCoroutines();
    AlertManager.Instance.Abort();
    this.ShowMaskFull();
    this.synchronizing.gameObject.SetActive(true);
    NetworkManager.Instance.ClearSyncro();
    this.ClearEventList();
    this.currentGameCode = this.currentGameCodeForReload;
    if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
    {
      this.photonView.RPC("NET_ReloadCombat", RpcTarget.Others, (object) Functions.CompressString(this.currentGameCode), (object) from);
      NetworkManager.Instance.ClearPlayerStatus();
      NetworkManager.Instance.ClearWaitingCalls();
    }
    this.gameStatus = "";
    this.cardActive = (CardData) null;
    this.beforeSyncCodeLocked = false;
    this.ResetAutoEndCount();
    this.heroTurn = false;
    this.waitingDeathScreen = false;
    this.waitingKill = false;
    this.waitingForAddcardAssignment = false;
    this.waitingForCardEnergyAssignment = false;
    this.waitingForDiscardAssignment = false;
    this.waitingForLookDiscardWindow = false;
    this.matchIsOver = false;
    this.characterKilled = false;
    this.heroActive = -1;
    this.npcActive = -1;
    this.preCastNum = -1;
    this.scarabSpawned = "";
    this.scarabSuccess = "";
    this.theHeroPreAutomatic = (Hero) null;
    this.theNPCPreAutomatic = (NPC) null;
    this.turnLoadedBySave = false;
    this.activatedTraitsRound.Clear();
    this.SetCardsWaitingForReset(0);
    this.GlobalDiscardCardsNum = 0;
    this.GlobalVanishCardsNum = 0;
    this.targetTransformDict = new Dictionary<string, Transform>();
    this.castingCardBlocked = new Dictionary<string, bool>();
    this.deckPileVisualState = 0;
    this.deathScreen.TurnOff();
    this.CleanTempTransform();
    PopupManager.Instance.ClosePopup();
    this.ClearItemExecuteForThisTurn();
    this.ItemExecuteForThisCombatReload();
    AtOManager.Instance.combatCardDictionary = new Dictionary<string, CardData>();
    this.StartCoroutine(this.MoveItemsOut(true));
    foreach (Transform transform in this.GO_Hand.transform)
    {
      if ((UnityEngine.Object) transform != (UnityEngine.Object) null && transform.gameObject.name != "HandMask")
        UnityEngine.Object.Destroy((UnityEngine.Object) transform.gameObject);
    }
    foreach (Component component in this.GO_Heroes.transform)
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
    foreach (Component component in this.GO_NPCs.transform)
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
    foreach (Component component in this.combattextTransform)
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
    foreach (KeyValuePair<string, CardData> card in this.cardDictionary)
      AtOManager.Instance.combatCardDictionary.Add(card.Key, card.Value);
    AtOManager.Instance.combatGameCode = this.currentGameCode;
    this.Start();
  }

  public bool WaitingForActionScreen()
  {
    return this.waitingForAddcardAssignment || this.waitingForCardEnergyAssignment || this.waitingForDiscardAssignment || this.waitingForLookDiscardWindow;
  }

  [PunRPC]
  private void NET_MasterReloadCombat(string _theProblem) => this.ReloadCombat(_theProblem);

  [PunRPC]
  private void NET_ReloadCombat(string _theCode, string _from)
  {
    this.StopCoroutines();
    this.currentGameCode = Functions.DecompressString(_theCode);
    this.currentGameCodeForReload = this.currentGameCode;
    this.ReloadCombat(_from);
  }

  public int DesyncFixable()
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("[DESYNC FIXABLE??]");
    string syncCode = this.GenerateSyncCode();
    string str1 = "";
    foreach (KeyValuePair<string, string> keyValuePair in this.syncCodeDict)
    {
      str1 = "";
      if (keyValuePair.Value == "")
      {
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[DESYNC from " + keyValuePair.Key + "] Empty code", "synccode");
        return 1;
      }
      if (keyValuePair.Value != syncCode)
      {
        string str2 = keyValuePair.Value;
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[SOURCE] " + syncCode, "synccode");
        if (Globals.Instance.ShowDebug)
          Functions.DebugLogGD("[TARGET] " + str2, "synccode");
        string[] strArray1 = syncCode.Split('$', StringSplitOptions.None);
        string[] strArray2 = str2.Split('$', StringSplitOptions.None);
        if (strArray1.Length != strArray2.Length)
        {
          if (Globals.Instance.ShowDebug)
            Functions.DebugLogGD("[NOT FIXABLE] Length mismatch", "synccode");
          return 1;
        }
        for (int index = 0; index < strArray1.Length; ++index)
        {
          if (strArray1[index].Split('|', StringSplitOptions.None).Length != strArray2[index].Split('|', StringSplitOptions.None).Length)
          {
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD("[NOT FIXABLE] Subcode Length mismatch", "synccode");
            return 1;
          }
        }
        if (strArray1.Length > 10 && strArray2.Length > 10 && strArray1[10] != strArray2[10])
        {
          Debug.LogError((object) "[CheckSyncCode] Dictionary code mismatch");
          return 2;
        }
      }
    }
    return 0;
  }

  public bool AllDesyncEqual(string theCode)
  {
    foreach (KeyValuePair<string, string> keyValuePair in this.syncCodeDict)
    {
      if (keyValuePair.Value == "" || keyValuePair.Value != theCode)
        return false;
    }
    return true;
  }

  [PunRPC]
  private void NET_FinishCastCodeSyncFromMaster(
    int _randomIndex,
    string _codeFromMaster,
    string _cardId)
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("NET_FinishCastCodeSyncFromMaster " + _codeFromMaster + " // " + _cardId, "synccode");
    this.FixCodeSyncFromMaster(_randomIndex, _codeFromMaster, false, true, _cardId);
  }

  [PunRPC]
  private void NET_FixCodeSyncFromMaster(int _randomIndex, string _codeFromMaster)
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("NET_FixCodeSyncFromMaster " + _codeFromMaster, "synccode");
    this.FixCodeSyncFromMaster(_randomIndex, _codeFromMaster, true, false);
  }

  private void FixCodeSyncFromMaster(
    int _randomIndex,
    string _codeFromMaster,
    bool _sendStatusReady,
    bool _sendFinishCastReady,
    string _cardId = "")
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD(nameof (FixCodeSyncFromMaster), "synccode");
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD(_codeFromMaster, "synccode");
    this.SetRandomIndex(_randomIndex);
    if (_sendFinishCastReady && this.castingCardListMP.Count > 1)
    {
      NetworkManager.Instance.SetStatusReady("finishcast" + _cardId);
    }
    else
    {
      if (this.checkSyncCodeInCoop)
      {
        if (_codeFromMaster != "")
        {
          try
          {
            _codeFromMaster = Functions.DecompressString(_codeFromMaster);
          }
          catch
          {
          }
          string syncCode = this.GenerateSyncCode();
          if (GameManager.Instance.GetDeveloperMode())
          {
            Debug.Log((object) ("FIX CODE FROM MASTER -> " + _codeFromMaster));
            Debug.Log((object) ("MY FIX FALSE -> " + syncCode));
          }
          bool flag1 = false;
          if (_codeFromMaster != syncCode)
          {
            if (GameManager.Instance.GetDeveloperMode())
              Debug.LogError((object) "DIFFERENT CODES!!");
            flag1 = true;
          }
          if (flag1)
          {
            if (Globals.Instance.ShowDebug)
              Functions.DebugLogGD(_codeFromMaster, "synccode");
            string[] strArray1 = _codeFromMaster.Split('$', StringSplitOptions.None);
            for (int index1 = 0; index1 < 8; ++index1)
            {
              string[] strArray2 = strArray1[index1].Split('|', StringSplitOptions.None);
              Character _characterTarget = index1 >= 4 ? (Character) this.TeamNPC[index1 - 4] : (Character) this.TeamHero[index1];
              if (_characterTarget != null && _characterTarget.Alive)
              {
                string[] strArray3 = strArray2[1].Split('_', StringSplitOptions.None);
                _characterTarget.HpCurrent = int.Parse(strArray3[0]);
                _characterTarget.Hp = strArray3.Length <= 1 || strArray3[1] == null ? _characterTarget.HpCurrent : int.Parse(strArray3[1]);
                _characterTarget.AuraList = new List<Aura>();
                if (strArray2.Length >= 4)
                {
                  string[] strArray4 = strArray2[3].Split(':', StringSplitOptions.None);
                  if (strArray4.Length != 0)
                  {
                    for (int index2 = 0; index2 < strArray4.Length; ++index2)
                    {
                      Aura aura = new Aura();
                      string[] strArray5 = strArray4[index2].Split('_', StringSplitOptions.None);
                      if (strArray5.Length == 2)
                      {
                        aura.SetAura(AtOManager.Instance.GlobalAuraCurseModificationByTraitsAndItems("set", Globals.Instance.GetAuraCurseFromIndex(int.Parse(strArray5[0])).ToLower(), (Character) null, _characterTarget), int.Parse(strArray5[1]));
                        _characterTarget.AuraList.Add(aura);
                      }
                    }
                    _characterTarget.UpdateAuraCurseFunctions();
                  }
                }
                if (index1 < 4)
                {
                  _characterTarget.EnergyCurrent = int.Parse(strArray2[4]);
                  _characterTarget.HeroItem.DrawEnergy();
                  if (strArray2[5] != "")
                  {
                    string[] strArray6 = strArray2[5].Split('%', StringSplitOptions.None);
                    this.HeroDeckDiscard[index1] = new List<string>();
                    for (int index3 = 0; index3 < strArray6.Length; ++index3)
                    {
                      if (strArray6[index3] != "")
                        this.HeroDeckDiscard[index1].Add(strArray6[index3]);
                    }
                    string[] strArray7 = strArray2[6].Split('%', StringSplitOptions.None);
                    this.HeroDeck[index1] = new List<string>();
                    for (int index4 = 0; index4 < strArray7.Length; ++index4)
                    {
                      if (strArray7[index4] != "")
                        this.HeroDeck[index1].Add(strArray7[index4]);
                    }
                    string[] strArray8 = strArray2[7].Split('%', StringSplitOptions.None);
                    this.HeroDeckVanish[index1] = new List<string>();
                    for (int index5 = 0; index5 < strArray8.Length; ++index5)
                    {
                      if (strArray8[index5] != "")
                        this.HeroDeckVanish[index1].Add(strArray8[index5]);
                    }
                  }
                  string[] strArray9 = strArray2[10].Split(':', StringSplitOptions.None);
                  bool flag2 = false;
                  if (strArray9 != null)
                  {
                    if (strArray9[0] != null && strArray9[0] != "")
                      _characterTarget.AssignEnchantmentManual(strArray9[0], 0);
                    if (strArray9[1] != null && strArray9[1] != "")
                      _characterTarget.AssignEnchantmentManual(strArray9[1], 1);
                    if (strArray9[2] != null && strArray9[2] != "")
                      _characterTarget.AssignEnchantmentManual(strArray9[2], 2);
                    flag2 = true;
                  }
                  if (flag2)
                    _characterTarget.HeroItem.ShowEnchantments();
                }
                _characterTarget.RoundMoved = int.Parse(strArray2[8]);
              }
            }
            this.currentRound = int.Parse(strArray1[8]);
            string[] strArray10 = strArray1[9].Split('&', StringSplitOptions.None);
            this.enchantmentExecutedTotal.Clear();
            if (strArray10 != null)
            {
              for (int index = 0; index < strArray10.Length; ++index)
              {
                string[] strArray11 = strArray10[index].Split('%', StringSplitOptions.None);
                if (strArray11 != null && strArray11.Length == 2 && strArray11[0] != null && strArray11[1] != null && int.Parse(strArray11[1]) > 0)
                  this.enchantmentExecutedTotal.Add(strArray11[0], int.Parse(strArray11[1]));
              }
            }
          }
        }
      }
      if (Globals.Instance.ShowDebug)
        Functions.DebugLogGD("FixCodeSyncFromMaster ends", "synccode");
      if (_sendStatusReady)
        NetworkManager.Instance.SetStatusReady("FixingSyncCode");
      if (!_sendFinishCastReady)
        return;
      NetworkManager.Instance.SetStatusReady("finishcast" + _cardId);
    }
  }

  private void FixCodeSyncFromMasterTOTAL(int _randomIndex, string codeFromMaster)
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("FixCodeSyncFromMaster", "synccode");
    foreach (Component component in this.GO_NPCs.transform)
      UnityEngine.Object.Destroy((UnityEngine.Object) component.gameObject);
    this.StartCoroutine(this.FixTOTALCo(_randomIndex, codeFromMaster));
  }

  private IEnumerator FixTOTALCo(int _randomIndex, string codeFromMaster)
  {
    MatchManager matchManager = this;
    matchManager.eventList.Add("FixingTotalCo");
    string[] codeArr = codeFromMaster.Split('$', StringSplitOptions.None);
    matchManager.SetRandomIndex(int.Parse(codeArr[11]));
    matchManager.currentRound = int.Parse(codeArr[8]);
    for (int index = 0; index < 8; ++index)
    {
      string[] strArray = codeArr[index].Split('|', StringSplitOptions.None);
      if (strArray.Length >= 9 && int.Parse(strArray[8]) == matchManager.currentRound)
        matchManager.roundBegan = true;
    }
    string[] strArray1 = codeArr[9].Split('&', StringSplitOptions.None);
    matchManager.enchantmentExecutedTotal.Clear();
    for (int index = 0; index < strArray1.Length; ++index)
    {
      string[] strArray2 = strArray1[index].Split('%', StringSplitOptions.None);
      if (strArray2 != null && strArray2.Length == 2 && strArray2[0] != null && strArray2[1] != null && int.Parse(strArray2[1]) > 0)
        matchManager.enchantmentExecutedTotal.Add(strArray2[0], int.Parse(strArray2[1]));
    }
    for (int i = 0; i < 8; ++i)
    {
      string[] aux = codeArr[i].Split('|', StringSplitOptions.None);
      Character theChar = (Character) null;
      theChar = i >= 4 ? (Character) matchManager.TeamNPC[i - 4] : (Character) matchManager.TeamHero[i];
      if (theChar != null || aux.Length >= 7 && aux[8] != "")
      {
        if (aux.Length == 1)
        {
          theChar.Alive = false;
          theChar.HpCurrent = 0;
          if (i < 4)
            UnityEngine.Object.Destroy((UnityEngine.Object) matchManager.GO_Heroes.transform.GetChild(i).gameObject);
          else if (matchManager.backgroundActive == "Sahti_Rust" && theChar.Id.StartsWith("pitt"))
            matchManager.DoSahtiRustBackground(false);
        }
        else
        {
          if (i >= 4)
          {
            matchManager.CreateNPC(Globals.Instance.GetNPC(aux[9]), _position: i - 4, generateFromReload: true, internalId: aux[10]);
            yield return (object) Globals.Instance.WaitForSeconds(0.2f);
            theChar = (Character) matchManager.TeamNPC[i - 4];
            theChar.Corruption = aux[11];
          }
          string[] strArray3 = aux[1].Split('_', StringSplitOptions.None);
          theChar.HpCurrent = int.Parse(strArray3[0]);
          theChar.Hp = strArray3.Length <= 1 || strArray3[1] == null ? theChar.HpCurrent : int.Parse(strArray3[1]);
          theChar.AuraList = new List<Aura>();
          if (aux.Length >= 4)
          {
            foreach (string str in aux[3].Split(':', StringSplitOptions.None))
            {
              Aura aura = new Aura();
              string[] strArray4 = str.Split('_', StringSplitOptions.None);
              if (strArray4.Length == 2)
              {
                aura.SetAura(AtOManager.Instance.GlobalAuraCurseModificationByTraitsAndItems("set", Globals.Instance.GetAuraCurseFromIndex(int.Parse(strArray4[0])).ToLower(), (Character) null, theChar), int.Parse(strArray4[1]));
                theChar.AuraList.Add(aura);
              }
            }
            theChar.UpdateAuraCurseFunctions();
          }
          if (i < 4)
          {
            theChar.EnergyCurrent = int.Parse(aux[4]);
            theChar.HeroItem.DrawEnergy();
            string[] strArray5 = aux[5].Split('%', StringSplitOptions.None);
            matchManager.HeroDeckDiscard[i] = new List<string>();
            for (int index = 0; index < strArray5.Length; ++index)
            {
              if (strArray5[index] != "")
                matchManager.HeroDeckDiscard[i].Add(strArray5[index]);
            }
            string[] strArray6 = aux[6].Split('%', StringSplitOptions.None);
            matchManager.HeroDeck[i] = new List<string>();
            for (int index = 0; index < strArray6.Length; ++index)
            {
              if (strArray6[index] != "")
                matchManager.HeroDeck[i].Add(strArray6[index]);
            }
            string[] strArray7 = aux[7].Split('%', StringSplitOptions.None);
            matchManager.HeroDeckVanish[i] = new List<string>();
            for (int index = 0; index < strArray7.Length; ++index)
            {
              if (strArray7[index] != "")
                matchManager.HeroDeckVanish[i].Add(strArray7[index]);
            }
            string[] strArray8 = aux[10].Split(':', StringSplitOptions.None);
            bool flag = false;
            if (strArray8 != null)
            {
              if (strArray8[0] != null && strArray8[0] != "")
                theChar.AssignEnchantmentManual(strArray8[0], 0);
              if (strArray8[1] != null && strArray8[1] != "")
                theChar.AssignEnchantmentManual(strArray8[1], 1);
              if (strArray8[2] != null && strArray8[2] != "")
                theChar.AssignEnchantmentManual(strArray8[2], 2);
              flag = true;
            }
            if (flag)
              theChar.HeroItem.ShowEnchantments();
          }
          else
          {
            string[] strArray9 = aux[4].Split('%', StringSplitOptions.None);
            if (matchManager.NPCDeck[i - 4] != null)
            {
              matchManager.NPCDeck[i - 4].Clear();
              for (int index = 0; index < strArray9.Length; ++index)
              {
                if (strArray9[index] != "")
                  matchManager.NPCDeck[i - 4].Add(strArray9[index]);
              }
            }
            string[] strArray10 = aux[5].Split('%', StringSplitOptions.None);
            if (matchManager.NPCDeckDiscard[i - 4] != null)
            {
              matchManager.NPCDeckDiscard[i - 4].Clear();
              for (int index = 0; index < strArray10.Length; ++index)
              {
                if (strArray10[index] != "")
                  matchManager.NPCDeckDiscard[i - 4].Add(strArray10[index]);
              }
            }
            string[] strArray11 = aux[6].Split('%', StringSplitOptions.None);
            if (matchManager.NPCHand != null && matchManager.NPCHand[i - 4] == null)
              matchManager.NPCHand[i - 4] = new List<string>();
            if (matchManager.NPCHand != null && matchManager.NPCHand[i - 4] != null)
            {
              matchManager.NPCHand[i - 4].Clear();
              for (int index = 0; index < strArray11.Length; ++index)
              {
                if (strArray11[index] != "")
                  matchManager.NPCHand[i - 4].Add(strArray11[index]);
              }
              if (matchManager.NPCHand[i - 4].Count > 0)
              {
                yield return (object) Globals.Instance.WaitForSeconds(0.1f);
                theChar.CreateOverDeck(false);
              }
            }
            string[] strArray12 = aux[7].Split('%', StringSplitOptions.None);
            if (matchManager.npcCardsCasted == null)
              matchManager.npcCardsCasted = new Dictionary<string, List<string>>();
            if (!matchManager.npcCardsCasted.ContainsKey(aux[10]))
              matchManager.npcCardsCasted.Add(aux[10], new List<string>());
            for (int index = 0; index < strArray12.Length; ++index)
            {
              if (strArray12[index] != "")
                matchManager.npcCardsCasted[aux[10]].Add(strArray12[index]);
            }
            string[] strArray13 = aux[12].Split(':', StringSplitOptions.None);
            bool flag = false;
            if (strArray13 != null)
            {
              if (strArray13[0] != null && strArray13[0] != "")
                theChar.AssignEnchantmentManual(strArray13[0], 0);
              if (strArray13[1] != null && strArray13[1] != "")
                theChar.AssignEnchantmentManual(strArray13[1], 1);
              if (strArray13[2] != null && strArray13[2] != "")
                theChar.AssignEnchantmentManual(strArray13[2], 2);
              flag = true;
            }
            if (flag)
              theChar.NPCItem.ShowEnchantments();
          }
          theChar.RoundMoved = int.Parse(aux[8]);
        }
      }
      aux = (string[]) null;
      theChar = (Character) null;
    }
    if (codeArr.Length > 12)
    {
      string[] strArray14 = codeArr[12].Split('%', StringSplitOptions.None);
      matchManager.scarabSpawned = strArray14[0];
      matchManager.scarabSuccess = strArray14[1];
    }
    matchManager.eventList.Remove("FixingTotalCo");
    matchManager.gotHeroDeck = true;
    matchManager.gotNPCDeck = true;
    matchManager.gotDictionary = true;
    matchManager.RestoreCombatStats();
    matchManager.backingDictionary = true;
    matchManager.RestoreCardDictionary();
    List<NPCState> paramsFromTurnSave = matchManager.npcParamsFromTurnSave;
    // ISSUE: explicit non-virtual call
    if ((paramsFromTurnSave != null ? (__nonvirtual (paramsFromTurnSave.Count) > 0 ? 1 : 0) : 0) != 0)
      matchManager.ApplyNPCState();
    while (matchManager.backingDictionary)
      yield return (object) Globals.Instance.WaitForSeconds(0.01f);
    if (!GameManager.Instance.IsMultiplayer())
      matchManager.StartCoroutine(matchManager.BeginMatch());
    else if (NetworkManager.Instance.IsMaster())
      matchManager.NET_ShareDecks(true);
  }

  private void ApplyNPCState()
  {
    Dictionary<string, NPC> dictionary = ((IEnumerable<NPC>) this.TeamNPC).Where<NPC>((Func<NPC, bool>) (n => n != null)).ToDictionary<NPC, string>((Func<NPC, string>) (n => n.InternalId));
    foreach (NPCState npcState in this.npcParamsFromTurnSave)
    {
      NPCState state = npcState;
      NPC npc = dictionary.Values.FirstOrDefault<NPC>((Func<NPC, bool>) (n => n.Id.StartsWith(state.Id, StringComparison.CurrentCultureIgnoreCase)));
      if (npc != null)
      {
        npc.IsIllusion = state.IsIllusion;
        npc.IsIllusionExposed = state.IsIllusionExposed;
        npc.IllusionCharacter = string.IsNullOrEmpty(state.IllusionCharacterId) ? (Character) null : (Character) dictionary.Values.FirstOrDefault<NPC>((Func<NPC, bool>) (n => n.Id.Contains(state.IllusionCharacterId, StringComparison.InvariantCultureIgnoreCase)));
      }
    }
  }

  private void StoreCombatStats()
  {
    for (int index1 = 0; index1 < AtOManager.Instance.combatStats.GetLength(0); ++index1)
    {
      for (int index2 = 0; index2 < AtOManager.Instance.combatStats.GetLength(1); ++index2)
        this.combatStatsAux[index1, index2] = AtOManager.Instance.combatStats[index1, index2];
    }
    for (int index3 = 0; index3 < AtOManager.Instance.combatStatsCurrent.GetLength(0); ++index3)
    {
      for (int index4 = 0; index4 < AtOManager.Instance.combatStatsCurrent.GetLength(1); ++index4)
        this.combatStatsCurrentAux[index3, index4] = AtOManager.Instance.combatStatsCurrent[index3, index4];
    }
    this.combatStatsDictAux = new Dictionary<string, Dictionary<string, List<string>>>((IDictionary<string, Dictionary<string, List<string>>>) this.combatStatsDict);
  }

  private void RestoreCombatStats()
  {
    if (this.combatStatsDictAux == null)
      return;
    if (this.combatStatsAux != null)
    {
      for (int index1 = 0; index1 < this.combatStatsAux.GetLength(0); ++index1)
      {
        for (int index2 = 0; index2 < this.combatStatsAux.GetLength(1); ++index2)
          AtOManager.Instance.combatStats[index1, index2] = this.combatStatsAux[index1, index2];
      }
    }
    if (this.combatStatsCurrentAux != null)
    {
      for (int index3 = 0; index3 < this.combatStatsCurrentAux.GetLength(0); ++index3)
      {
        for (int index4 = 0; index4 < this.combatStatsCurrentAux.GetLength(1); ++index4)
          AtOManager.Instance.combatStatsCurrent[index3, index4] = this.combatStatsCurrentAux[index3, index4];
      }
    }
    this.combatStatsDict = new Dictionary<string, Dictionary<string, List<string>>>((IDictionary<string, Dictionary<string, List<string>>>) this.combatStatsDictAux);
    this.combatStatsAux = new int[AtOManager.Instance.combatStats.GetLength(0), AtOManager.Instance.combatStats.GetLength(1)];
    this.combatStatsCurrentAux = new int[AtOManager.Instance.combatStatsCurrent.GetLength(0), AtOManager.Instance.combatStatsCurrent.GetLength(1)];
  }

  public string GetCombatStatsCurrentForTurnSave()
  {
    StringBuilder stringBuilder = new StringBuilder();
    for (int index1 = 0; index1 < AtOManager.Instance.combatStatsCurrent.GetLength(0); ++index1)
    {
      for (int index2 = 0; index2 < AtOManager.Instance.combatStatsCurrent.GetLength(1); ++index2)
      {
        stringBuilder.Append(AtOManager.Instance.combatStatsCurrent[index1, index2]);
        stringBuilder.Append("_");
      }
      stringBuilder.Append("|");
    }
    return Functions.CompressString(stringBuilder.ToString());
  }

  public void SetCombatStatsCurrentForTurnSave(string _stats)
  {
    if (_stats == "")
      return;
    _stats = Functions.DecompressString(_stats);
    string[] strArray1 = _stats.Split('|', StringSplitOptions.None);
    for (int index1 = 0; index1 < strArray1.Length; ++index1)
    {
      string[] strArray2 = strArray1[index1].Split('_', StringSplitOptions.None);
      for (int index2 = 0; index2 < strArray2.Length; ++index2)
      {
        if (strArray2[index2] != "")
          AtOManager.Instance.combatStatsCurrent[index1, index2] = int.Parse(strArray2[index2]);
      }
    }
  }

  public string GetCombatStatsForTurnSave()
  {
    StringBuilder stringBuilder = new StringBuilder();
    string text = "";
    foreach (KeyValuePair<string, Dictionary<string, List<string>>> keyValuePair1 in this.combatStatsDict)
    {
      foreach (KeyValuePair<string, List<string>> keyValuePair2 in keyValuePair1.Value)
      {
        stringBuilder.Append(keyValuePair1.Key);
        stringBuilder.Append("&");
        stringBuilder.Append(keyValuePair2.Key);
        stringBuilder.Append("|");
        int num = 1;
        string str = "";
        for (int index = 0; index < keyValuePair2.Value.Count; ++index)
        {
          if (str != keyValuePair2.Value[index])
          {
            if (str != "")
            {
              stringBuilder.Append("*");
              stringBuilder.Append(num);
              stringBuilder.Append("+");
              num = 1;
            }
            stringBuilder.Append(keyValuePair2.Value[index]);
            str = keyValuePair2.Value[index];
          }
          else
            ++num;
        }
        stringBuilder.Append("*");
        stringBuilder.Append(num);
        stringBuilder.Append("/");
        text += stringBuilder.ToString();
        stringBuilder.Clear();
      }
    }
    return Functions.CompressString(text);
  }

  public void SetCombatStatsForTurnSave(string _stats)
  {
    if (_stats == "")
      return;
    _stats = Functions.DecompressString(_stats);
    this.combatStatsDict = new Dictionary<string, Dictionary<string, List<string>>>();
    this.combatStatsAux = new int[AtOManager.Instance.combatStats.GetLength(0), AtOManager.Instance.combatStats.GetLength(1)];
    this.combatStatsCurrentAux = new int[AtOManager.Instance.combatStatsCurrent.GetLength(0), AtOManager.Instance.combatStatsCurrent.GetLength(1)];
    foreach (string str1 in _stats.Split('/', StringSplitOptions.None))
    {
      string[] strArray1 = str1.Split('&', StringSplitOptions.None);
      if (strArray1.Length == 2)
      {
        if (!this.combatStatsDict.ContainsKey(strArray1[0]))
          this.combatStatsDict.Add(strArray1[0], new Dictionary<string, List<string>>());
        string[] strArray2 = strArray1[1].Split('|', StringSplitOptions.None);
        if (strArray2.Length == 2)
        {
          if (!this.combatStatsDict[strArray1[0]].ContainsKey(strArray2[0]))
            this.combatStatsDict[strArray1[0]].Add(strArray2[0], new List<string>());
          foreach (string str2 in strArray2[1].Split('+', StringSplitOptions.None))
          {
            string[] strArray3 = str2.Split('*', StringSplitOptions.None);
            for (int index = 0; index < int.Parse(strArray3[1]); ++index)
              this.combatStatsDict[strArray1[0]][strArray2[0]].Add(strArray3[0]);
          }
        }
      }
    }
  }

  public string GetHeroLifeArrForTurnSave() => JsonHelper.ToJson<int>(this.heroLifeArr);

  public void SetHeroLifeArrForTurnSave(string _life)
  {
    if (_life == "")
      return;
    this.heroLifeArr = JsonHelper.FromJson<int>(_life);
  }

  public string GetItemExecutionInCombatForTurnSave()
  {
    StringBuilder stringBuilder = new StringBuilder();
    foreach (KeyValuePair<string, int> keyValuePair in this.itemExecutedInCombat)
    {
      stringBuilder.Append(keyValuePair.Key);
      stringBuilder.Append(":");
      stringBuilder.Append(keyValuePair.Value);
      stringBuilder.Append("|");
    }
    return Functions.CompressString(stringBuilder.ToString());
  }

  public void SetItemExecutionInCombatForTurnSave(string _itemExecution)
  {
    switch (_itemExecution)
    {
      case "":
        break;
      case null:
        break;
      default:
        _itemExecution = Functions.DecompressString(_itemExecution);
        if (_itemExecution == "")
          break;
        this.itemExecutedInCombat.Clear();
        foreach (string str in _itemExecution.Split('|', StringSplitOptions.None))
        {
          string[] strArray = str.Split(':', StringSplitOptions.None);
          if (strArray.Length == 2)
            this.itemExecutedInCombat.Add(strArray[0], int.Parse(strArray[1]));
        }
        this.ItemExecuteForThisCombatDuplicate();
        break;
    }
  }

  public void SetCurrentSpecialCardsUsedInMatchForTurnSave(string currentSpecialCardsUsedInMatch)
  {
    int result;
    this.MindSpikeAbility.CurrentSpecialCardsUsedInMatch = int.TryParse(currentSpecialCardsUsedInMatch, out result) ? result : 0;
  }

  public void ConsumeStatusForCombatStats(string _target, string _status, int _charges)
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("ConsumeStatusForCombatStats " + _target + " " + _status + " " + _charges.ToString(), "trace");
    _status = _status.ToLower();
    if (!this.combatStatsDict.ContainsKey(_target) || !this.combatStatsDict[_target].ContainsKey(_status))
      return;
    if (_charges == -1)
    {
      this.combatStatsDict[_target].Remove(_status);
    }
    else
    {
      if (_charges >= this.combatStatsDict[_target][_status].Count)
        this.combatStatsDict[_target].Remove(_status);
      for (int index = 0; index < _charges; ++index)
      {
        if (this.combatStatsDict[_target].ContainsKey(_status) && this.combatStatsDict[_target][_status] != null && this.combatStatsDict[_target][_status].Count > 0)
          this.combatStatsDict[_target][_status].RemoveAt(0);
      }
    }
  }

  public void StoreStatusForCombatStats(
    string _target,
    string _status,
    string _caster,
    int _charges)
  {
    _status = _status.ToLower();
    if (!this.combatStatsDict.ContainsKey(_target))
      this.combatStatsDict.Add(_target, new Dictionary<string, List<string>>());
    if (!this.combatStatsDict[_target].ContainsKey(_status))
      this.combatStatsDict[_target].Add(_status, new List<string>());
    for (int index = 0; index < _charges; ++index)
      this.combatStatsDict[_target][_status].Add(_caster);
  }

  public void DamageStatusFromCombatStats(string _target, string _status, int _damage)
  {
    _status = _status.ToLower();
    if (!this.combatStatsDict.ContainsKey(_target) || !this.combatStatsDict[_target].ContainsKey(_status))
      return;
    Dictionary<string, int> dictionary1 = new Dictionary<string, int>();
    int count = this.combatStatsDict[_target][_status].Count;
    for (int index = 0; index < count; ++index)
    {
      string key = this.combatStatsDict[_target][_status][index];
      if (dictionary1.ContainsKey(key))
        dictionary1[key]++;
      else
        dictionary1.Add(key, 1);
    }
    Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
    int num1 = 0;
    foreach (KeyValuePair<string, int> keyValuePair in dictionary1)
    {
      if (keyValuePair.Key.Split('_', StringSplitOptions.None).Length != 0)
      {
        int heroFromId = this.GetHeroFromId(keyValuePair.Key);
        if (heroFromId > -1)
        {
          float num2 = (float) keyValuePair.Value / (float) count;
          int num3 = Functions.FuncRoundToInt((float) _damage * num2);
          if (num1 + num3 > _damage)
            num3 = _damage - num1;
          dictionary2.Add(heroFromId, num3);
          num1 += num3;
        }
      }
    }
    bool flag = true;
    foreach (KeyValuePair<int, int> keyValuePair in dictionary2)
    {
      int num4 = !flag || num1 >= _damage ? keyValuePair.Value : keyValuePair.Value + (_damage - num1);
      if (_status != "regeneration" && _status != "sanctify")
      {
        AtOManager.Instance.combatStats[keyValuePair.Key, 0] += num4;
        AtOManager.Instance.combatStatsCurrent[keyValuePair.Key, 0] += num4;
      }
      else
      {
        AtOManager.Instance.combatStats[keyValuePair.Key, 3] += num4;
        AtOManager.Instance.combatStatsCurrent[keyValuePair.Key, 3] += num4;
      }
      flag = false;
      if (AtOManager.Instance.combatStats.GetLength(1) > 10)
      {
        switch (_status)
        {
          case "bleed":
            AtOManager.Instance.combatStats[keyValuePair.Key, 6] += num4;
            AtOManager.Instance.combatStatsCurrent[keyValuePair.Key, 6] += num4;
            continue;
          case "burn":
            AtOManager.Instance.combatStats[keyValuePair.Key, 7] += num4;
            AtOManager.Instance.combatStatsCurrent[keyValuePair.Key, 7] += num4;
            continue;
          case "dark":
            AtOManager.Instance.combatStats[keyValuePair.Key, 8] += num4;
            AtOManager.Instance.combatStatsCurrent[keyValuePair.Key, 8] += num4;
            continue;
          case "poison":
            AtOManager.Instance.combatStats[keyValuePair.Key, 9] += num4;
            AtOManager.Instance.combatStatsCurrent[keyValuePair.Key, 9] += num4;
            continue;
          case "spark":
            AtOManager.Instance.combatStats[keyValuePair.Key, 10] += num4;
            AtOManager.Instance.combatStatsCurrent[keyValuePair.Key, 10] += num4;
            continue;
          case "thorns":
            AtOManager.Instance.combatStats[keyValuePair.Key, 11] += num4;
            AtOManager.Instance.combatStatsCurrent[keyValuePair.Key, 11] += num4;
            continue;
          default:
            continue;
        }
      }
    }
  }

  public string GenerateSyncCodeForCheckingAction()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this.eventList.Count);
    stringBuilder.Append(this.randomIndex);
    stringBuilder.Append(this.randomItemsIndex);
    stringBuilder.Append(this.randomTraitsIndex);
    stringBuilder.Append(this.randomDeckIndex);
    stringBuilder.Append(this.ctQueue.Count);
    for (int index1 = 0; index1 < this.TeamHero.Length; ++index1)
    {
      if (this.TeamHero[index1] != null && !((UnityEngine.Object) this.TeamHero[index1].HeroData == (UnityEngine.Object) null) && this.TeamHero[index1].Alive)
      {
        stringBuilder.Append(this.TeamHero[index1].HpCurrent);
        stringBuilder.Append(this.TeamHero[index1].Hp);
        if (this.TeamHero[index1].AuraList != null)
        {
          int count = this.TeamHero[index1].AuraList.Count;
          stringBuilder.Append(count);
          for (int index2 = 0; index2 < count; ++index2)
          {
            if (this.TeamHero[index1].AuraList[index2] != null && (UnityEngine.Object) this.TeamHero[index1].AuraList[index2].ACData != (UnityEngine.Object) null)
            {
              stringBuilder.Append(this.TeamHero[index1].AuraList[index2].ACData.Id);
              stringBuilder.Append(this.TeamHero[index1].AuraList[index2].AuraCharges);
            }
          }
        }
        stringBuilder.Append(this.TeamHero[index1].EnergyCurrent);
        stringBuilder.Append(this.TeamHero[index1].Enchantment);
        stringBuilder.Append(this.TeamHero[index1].Enchantment2);
        stringBuilder.Append(this.TeamHero[index1].Enchantment3);
      }
    }
    for (int index3 = 0; index3 < this.TeamNPC.Length; ++index3)
    {
      if (this.TeamNPC[index3] != null && this.TeamNPC[index3].Alive)
      {
        stringBuilder.Append(this.TeamNPC[index3].HpCurrent);
        stringBuilder.Append(this.TeamNPC[index3].Hp);
        if (this.TeamNPC[index3].AuraList != null)
        {
          int count = this.TeamNPC[index3].AuraList.Count;
          stringBuilder.Append(count);
          for (int index4 = 0; index4 < count; ++index4)
          {
            if (this.TeamNPC[index3].AuraList[index4] != null && (UnityEngine.Object) this.TeamNPC[index3].AuraList[index4].ACData != (UnityEngine.Object) null)
            {
              stringBuilder.Append(this.TeamNPC[index3].AuraList[index4].ACData.Id);
              stringBuilder.Append(this.TeamNPC[index3].AuraList[index4].AuraCharges);
            }
          }
        }
      }
    }
    foreach (KeyValuePair<string, int> keyValuePair in this.enchantmentExecutedTotal)
    {
      stringBuilder.Append(keyValuePair.Key);
      stringBuilder.Append(keyValuePair.Value);
    }
    return stringBuilder.ToString();
  }

  public string GenerateSyncCode(bool fixDeck = false)
  {
    if (Globals.Instance.ShowDebug)
      Functions.DebugLogGD("Generate code " + fixDeck.ToString(), "synccode");
    StringBuilder stringBuilder = new StringBuilder();
    for (int index1 = 0; index1 < this.TeamHero.Length; ++index1)
    {
      stringBuilder.Append("H_");
      stringBuilder.Append(index1);
      if (this.TeamHero[index1] == null || (UnityEngine.Object) this.TeamHero[index1].HeroData == (UnityEngine.Object) null || !this.TeamHero[index1].Alive)
      {
        stringBuilder.Append("$");
      }
      else
      {
        stringBuilder.Append("|");
        stringBuilder.Append(this.TeamHero[index1].HpCurrent);
        stringBuilder.Append("_");
        stringBuilder.Append(this.TeamHero[index1].Hp);
        stringBuilder.Append("|");
        if (this.HeroDeck != null && this.HeroDeck[index1] != null)
          stringBuilder.Append(this.HeroDeck[index1].Count);
        else
          stringBuilder.Append("0");
        stringBuilder.Append("_");
        if (this.HeroDeckDiscard != null && this.HeroDeckDiscard[index1] != null)
          stringBuilder.Append(this.HeroDeckDiscard[index1].Count);
        else
          stringBuilder.Append("0");
        stringBuilder.Append("_");
        if (this.HeroHand != null && this.HeroHand[index1] != null)
          stringBuilder.Append(this.HeroHand[index1].Count);
        else
          stringBuilder.Append("0");
        stringBuilder.Append("|");
        for (int index2 = 0; index2 < this.TeamHero[index1].AuraList.Count; ++index2)
        {
          if (this.TeamHero[index1].AuraList[index2] != null && (UnityEngine.Object) this.TeamHero[index1].AuraList[index2].ACData != (UnityEngine.Object) null)
          {
            stringBuilder.Append(Globals.Instance.GetAuraCurseIndex(this.TeamHero[index1].AuraList[index2].ACData.Id.ToLower()));
            stringBuilder.Append("_");
            stringBuilder.Append(this.TeamHero[index1].AuraList[index2].AuraCharges);
            stringBuilder.Append(":");
          }
        }
        stringBuilder.Append("|");
        stringBuilder.Append(this.TeamHero[index1].EnergyCurrent);
        stringBuilder.Append("|");
        if (fixDeck)
        {
          for (int index3 = 0; index3 < this.HeroDeckDiscard[index1].Count; ++index3)
          {
            stringBuilder.Append(this.HeroDeckDiscard[index1][index3]);
            stringBuilder.Append("%");
          }
        }
        stringBuilder.Append("|");
        if (fixDeck)
        {
          for (int index4 = 0; index4 < this.HeroDeck[index1].Count; ++index4)
          {
            stringBuilder.Append(this.HeroDeck[index1][index4]);
            stringBuilder.Append("%");
          }
        }
        stringBuilder.Append("|");
        if (fixDeck)
        {
          for (int index5 = 0; index5 < this.HeroDeckVanish[index1].Count; ++index5)
          {
            stringBuilder.Append(this.HeroDeckVanish[index1][index5]);
            stringBuilder.Append("%");
          }
        }
        stringBuilder.Append("|");
        stringBuilder.Append(this.TeamHero[index1].RoundMoved);
        stringBuilder.Append("|");
        if ((UnityEngine.Object) this.TeamHero[index1].HeroItem != (UnityEngine.Object) null)
          stringBuilder.Append(this.TeamHero[index1].HeroItem.gameObject.name);
        else
          stringBuilder.Append("");
        stringBuilder.Append("|");
        stringBuilder.Append(this.TeamHero[index1].Enchantment);
        stringBuilder.Append(":");
        stringBuilder.Append(this.TeamHero[index1].Enchantment2);
        stringBuilder.Append(":");
        stringBuilder.Append(this.TeamHero[index1].Enchantment3);
        stringBuilder.Append("$");
      }
    }
    for (int index6 = 0; index6 < this.TeamNPC.Length; ++index6)
    {
      stringBuilder.Append("N_");
      stringBuilder.Append(index6);
      if (this.TeamNPC[index6] == null || !this.TeamNPC[index6].Alive)
      {
        stringBuilder.Append("$");
      }
      else
      {
        stringBuilder.Append("|");
        stringBuilder.Append(this.TeamNPC[index6].HpCurrent);
        stringBuilder.Append("_");
        stringBuilder.Append(this.TeamNPC[index6].Hp);
        stringBuilder.Append("|0|");
        for (int index7 = 0; index7 < this.TeamNPC[index6].AuraList.Count; ++index7)
        {
          if (this.TeamNPC[index6].AuraList[index7] != null && (UnityEngine.Object) this.TeamNPC[index6].AuraList[index7].ACData != (UnityEngine.Object) null)
          {
            stringBuilder.Append(Globals.Instance.GetAuraCurseIndex(this.TeamNPC[index6].AuraList[index7].ACData.Id.ToLower()));
            stringBuilder.Append("_");
            stringBuilder.Append(this.TeamNPC[index6].AuraList[index7].AuraCharges);
            stringBuilder.Append(":");
          }
        }
        stringBuilder.Append("|");
        if (fixDeck)
        {
          for (int index8 = 0; index8 < this.NPCDeck[index6].Count; ++index8)
          {
            stringBuilder.Append(this.NPCDeck[index6][index8]);
            stringBuilder.Append("%");
          }
        }
        stringBuilder.Append("|");
        if (fixDeck && this.NPCDeckDiscard != null && this.NPCDeckDiscard[index6] != null)
        {
          for (int index9 = 0; index9 < this.NPCDeckDiscard[index6].Count; ++index9)
          {
            stringBuilder.Append(this.NPCDeckDiscard[index6][index9]);
            stringBuilder.Append("%");
          }
        }
        stringBuilder.Append("|");
        if (fixDeck && this.NPCHand != null && this.NPCHand[index6] != null)
        {
          for (int index10 = 0; index10 < this.NPCHand[index6].Count; ++index10)
          {
            stringBuilder.Append(this.NPCHand[index6][index10]);
            stringBuilder.Append("%");
          }
        }
        stringBuilder.Append("|");
        if (fixDeck && this.npcCardsCasted != null && this.npcCardsCasted.ContainsKey(this.TeamNPC[index6].InternalId) && this.npcCardsCasted[this.TeamNPC[index6].InternalId] != null)
        {
          for (int index11 = 0; index11 < this.npcCardsCasted[this.TeamNPC[index6].InternalId].Count; ++index11)
          {
            stringBuilder.Append(this.npcCardsCasted[this.TeamNPC[index6].InternalId][index11]);
            stringBuilder.Append("%");
          }
        }
        stringBuilder.Append("|");
        stringBuilder.Append(this.TeamNPC[index6].RoundMoved);
        stringBuilder.Append("|");
        stringBuilder.Append(this.TeamNPC[index6].NpcData.Id);
        stringBuilder.Append("|");
        stringBuilder.Append(this.TeamNPC[index6].InternalId);
        stringBuilder.Append("|");
        stringBuilder.Append(this.TeamNPC[index6].Corruption);
        stringBuilder.Append("|");
        stringBuilder.Append(this.TeamNPC[index6].Enchantment);
        stringBuilder.Append(":");
        stringBuilder.Append(this.TeamNPC[index6].Enchantment2);
        stringBuilder.Append(":");
        stringBuilder.Append(this.TeamNPC[index6].Enchantment3);
        stringBuilder.Append("$");
      }
    }
    stringBuilder.Append(this.currentRound);
    stringBuilder.Append("$");
    foreach (KeyValuePair<string, int> keyValuePair in this.enchantmentExecutedTotal)
    {
      stringBuilder.Append(keyValuePair.Key);
      stringBuilder.Append("%");
      stringBuilder.Append(keyValuePair.Value);
      stringBuilder.Append("&");
    }
    stringBuilder.Append("$");
    stringBuilder.Append(this.CardNamesForSyncCode());
    stringBuilder.Append("$");
    stringBuilder.Append(this.randomIndex);
    stringBuilder.Append("$");
    stringBuilder.Append(this.scarabSpawned);
    stringBuilder.Append("%");
    stringBuilder.Append(this.scarabSuccess);
    stringBuilder.Append("%");
    return stringBuilder.ToString();
  }

  public void DestroyedItemInThisTurn(int _charIndex, string _cardId)
  {
    if (!this.heroDestroyedItemsInThisTurn.ContainsKey(_charIndex))
      this.heroDestroyedItemsInThisTurn.Add(_charIndex, new Dictionary<string, string>());
    CardData cardData = Globals.Instance.GetCardData(_cardId, false);
    string lower = Enum.GetName(typeof (Enums.CardType), (object) cardData.CardType).ToLower();
    if (this.heroDestroyedItemsInThisTurn[_charIndex].ContainsKey(lower))
      return;
    this.heroDestroyedItemsInThisTurn[_charIndex].Add(lower, cardData.Item.Id);
  }

  public void CleanPrePostDamageDictionary(string _id)
  {
    if (!this.prePostDamageDictionary.ContainsKey(_id))
      return;
    this.prePostDamageDictionary.Remove(_id);
  }

  public void SetCardActive(CardData _cardActive) => this.cardActive = _cardActive;

  public void ShowCombatKeyboardByConfig()
  {
    if (GameManager.Instance.ConfigKeyboardShortcuts)
      this.ShowCombatKeyboard(true);
    else
      this.ShowCombatKeyboard(false);
  }

  public void ShowCombatKeyboard(bool _state)
  {
    if (_state && !this.IsYourTurn())
      _state = false;
    if (_state)
    {
      if (this.preCastNum > -1)
      {
        int num = 1;
        for (int index = 3; index >= 0; --index)
        {
          if (this.TeamHero[index] != null && this.TeamHero[index].Alive && (UnityEngine.Object) this.TeamHero[index].HeroItem != (UnityEngine.Object) null)
          {
            Transform targetByNum = this.GetTargetByNum(num);
            if ((UnityEngine.Object) targetByNum != (UnityEngine.Object) null && (UnityEngine.Object) this.cardItemTable[this.preCastNum - 1] != (UnityEngine.Object) null)
            {
              if (this.CheckTarget(targetByNum, this.cardItemTable[this.preCastNum - 1].CardData))
                this.TeamHero[index].HeroItem.ShowKeyNum(true, num.ToString());
              else
                this.TeamHero[index].HeroItem.ShowKeyNum(true, num.ToString(), true);
            }
            ++num;
          }
        }
        for (int index = 0; index < 4; ++index)
        {
          if (this.TeamNPC[index] != null && this.TeamNPC[index].Alive && (UnityEngine.Object) this.TeamNPC[index].NPCItem != (UnityEngine.Object) null)
          {
            Transform targetByNum = this.GetTargetByNum(num);
            if ((UnityEngine.Object) targetByNum != (UnityEngine.Object) null && (UnityEngine.Object) this.cardItemTable[this.preCastNum - 1] != (UnityEngine.Object) null)
            {
              if (this.CheckTarget(targetByNum, this.cardItemTable[this.preCastNum - 1].CardData))
                this.TeamNPC[index].NPCItem.ShowKeyNum(true, num.ToString());
              else
                this.TeamNPC[index].NPCItem.ShowKeyNum(true, num.ToString(), true);
            }
            ++num;
          }
        }
        if (this.cardItemTable == null)
          return;
        for (int index = 0; index < this.cardItemTable.Count; ++index)
        {
          if ((UnityEngine.Object) this.cardItemTable[index] != (UnityEngine.Object) null)
            this.cardItemTable[index].ShowKeyNum(false);
        }
      }
      else
      {
        int num = 1;
        if (this.cardItemTable != null)
        {
          for (int index = 0; index < this.cardItemTable.Count; ++index)
          {
            if ((UnityEngine.Object) this.cardItemTable[index] != (UnityEngine.Object) null)
            {
              if (this.cardItemTable[index].IsPlayable())
                this.cardItemTable[index].ShowKeyNum(true, num.ToString());
              else
                this.cardItemTable[index].ShowKeyNum(true, num.ToString(), true);
              ++num;
            }
          }
        }
        for (int index = 0; index < 4; ++index)
        {
          if (this.TeamHero[index] != null && this.TeamHero[index].Alive && (UnityEngine.Object) this.TeamHero[index].HeroItem != (UnityEngine.Object) null)
            this.TeamHero[index].HeroItem.ShowKeyNum(false);
        }
        for (int index = 0; index < 4; ++index)
        {
          if (this.TeamNPC[index] != null && this.TeamNPC[index].Alive && (UnityEngine.Object) this.TeamNPC[index].NPCItem != (UnityEngine.Object) null)
            this.TeamNPC[index].NPCItem.ShowKeyNum(false);
        }
      }
    }
    else
    {
      if (this.cardItemTable != null)
      {
        for (int index = 0; index < this.cardItemTable.Count; ++index)
        {
          if ((UnityEngine.Object) this.cardItemTable[index] != (UnityEngine.Object) null)
            this.cardItemTable[index].ShowKeyNum(false);
        }
      }
      for (int index = 0; index < 4; ++index)
      {
        if (this.TeamHero[index] != null && this.TeamHero[index].Alive && (UnityEngine.Object) this.TeamHero[index].HeroItem != (UnityEngine.Object) null)
          this.TeamHero[index].HeroItem.ShowKeyNum(false);
      }
      for (int index = 0; index < 4; ++index)
      {
        if (this.TeamNPC[index] != null && this.TeamNPC[index].Alive && (UnityEngine.Object) this.TeamNPC[index].NPCItem != (UnityEngine.Object) null)
          this.TeamNPC[index].NPCItem.ShowKeyNum(false);
      }
    }
  }

  public void KeyboardNum(int _num)
  {
    if (this.MatchIsOver || GameManager.Instance.GetDeveloperMode() && !GameManager.Instance.ConfigKeyboardShortcuts || !this.heroTurn || !this.IsYourTurn() && !this.IsYourTurnForAddDiscard() || (UnityEngine.Object) this.deathScreen == (UnityEngine.Object) null || (UnityEngine.Object) this.addcardSelector == (UnityEngine.Object) null || (UnityEngine.Object) this.deckCardsWindow == (UnityEngine.Object) null)
      return;
    if (this.deckCardsWindow.IsActive())
    {
      int num1 = _num;
      int num2 = num1 != 0 ? num1 - 1 : 9;
      int num3 = 0;
      foreach (Transform transform in this.deckCardsWindow.cardContainer)
      {
        if ((bool) (UnityEngine.Object) transform.GetComponent<CardItem>() && num3 == num2)
        {
          transform.GetComponent<CardItem>().OnMouseUp();
          break;
        }
        ++num3;
      }
    }
    else if (this.discardSelector.IsActive())
    {
      int num4 = _num;
      int num5 = num4 != 0 ? num4 - 1 : 9;
      int num6 = 0;
      foreach (Transform transform in this.discardSelector.cardContainer)
      {
        if ((bool) (UnityEngine.Object) transform.GetComponent<CardItem>() && num6 == num5)
        {
          transform.GetComponent<CardItem>().OnMouseUp();
          break;
        }
        ++num6;
      }
    }
    else if (this.energySelector.IsActive())
    {
      this.energySelector.AssignEnergyFromOutside(_num);
    }
    else
    {
      if (!((UnityEngine.Object) EventSystem.current.currentSelectedGameObject == (UnityEngine.Object) null) && Functions.TransformIsVisible(EventSystem.current.currentSelectedGameObject.transform) || !this.canKeyboardCast || this.gameBusy || this.deathScreen.IsActive() || this.addcardSelector.IsActive() || this.deckCardsWindow.IsActive())
        return;
      if (_num == 0)
        _num = 10;
      if (_num <= 0 || _num >= 11)
        return;
      this.CastCardNum(_num);
    }
  }

  public void KeyboardEnter()
  {
    if (this.MatchIsOver || !GameManager.Instance.GetDeveloperMode() && !GameManager.Instance.ConfigKeyboardShortcuts)
      return;
    if (this.deckCardsWindow.IsActive())
    {
      if (!this.IsYourTurnForAddDiscard())
        return;
      this.deckCardsWindow.Action();
    }
    else if (this.discardSelector.IsActive())
    {
      if (!this.IsYourTurnForAddDiscard())
        return;
      this.discardSelector.Action();
    }
    else if (this.energySelector.IsActive())
    {
      if (!this.IsYourTurn())
        return;
      this.energySelector.AssignEnergyAction();
    }
    else if (this.deathScreen.IsActive())
    {
      if (!Functions.TransformIsVisible(this.deathScreen.button))
        return;
      this.deathScreen.TurnOffFromButton();
    }
    else
    {
      if (!GameManager.Instance.GetDeveloperMode())
        return;
      this.KeyboardDraw();
    }
  }

  public void KeyboardSpace()
  {
    if (this.MatchIsOver || GameManager.Instance.GetDeveloperMode() && !GameManager.Instance.ConfigKeyboardShortcuts || !((UnityEngine.Object) EventSystem.current.currentSelectedGameObject == (UnityEngine.Object) null) && Functions.TransformIsVisible(EventSystem.current.currentSelectedGameObject.transform) || (UnityEngine.Object) this.deathScreen == (UnityEngine.Object) null || (UnityEngine.Object) this.addcardSelector == (UnityEngine.Object) null || (UnityEngine.Object) this.deckCardsWindow == (UnityEngine.Object) null || !this.canKeyboardCast || !this.heroTurn || !this.IsYourTurn() || this.gameBusy || this.deathScreen.IsActive() || this.addcardSelector.IsActive() || this.deckCardsWindow.IsActive() || this.heroActive <= -1 || !this.botEndTurn.gameObject.activeSelf)
      return;
    this.EndTurn();
  }

  public void KeyboardEmote(int _number)
  {
    if (this.MatchIsOver || !((UnityEngine.Object) EventSystem.current.currentSelectedGameObject == (UnityEngine.Object) null) && Functions.TransformIsVisible(EventSystem.current.currentSelectedGameObject.transform) || !GameManager.Instance.IsMultiplayer() || (UnityEngine.Object) this.deathScreen == (UnityEngine.Object) null || (UnityEngine.Object) this.addcardSelector == (UnityEngine.Object) null || (UnityEngine.Object) this.deckCardsWindow == (UnityEngine.Object) null)
      return;
    this.SetCharactersPing(_number);
  }

  public void KeyboardEnergy()
  {
    if (this.MatchIsOver || this.theHero == null)
      return;
    this.theHero.ModifyEnergy(10);
  }

  public void KeyboardReloadCombat() => this.ReloadCombat();

  public void KeyboardDraw()
  {
    if (!((UnityEngine.Object) EventSystem.current.currentSelectedGameObject == (UnityEngine.Object) null) && Functions.TransformIsVisible(EventSystem.current.currentSelectedGameObject.transform) || (UnityEngine.Object) this.deathScreen == (UnityEngine.Object) null || (UnityEngine.Object) this.addcardSelector == (UnityEngine.Object) null || (UnityEngine.Object) this.deckCardsWindow == (UnityEngine.Object) null || this.heroActive <= -1)
      return;
    this.NewCard(1, Enums.CardFrom.Deck);
  }

  public void KeyboardDbg()
  {
    Debug.Log((object) ("\ncardsWaitingForReset=>" + this.cardsWaitingForReset.ToString() + "\neventList.Count=>" + this.eventList.Count.ToString() + "\nisBeginTournPhase=>" + this.isBeginTournPhase.ToString() + "\ncastingCardBlocked=>" + this.castingCardBlocked.Count.ToString() + "\nwaitingItemTrait=>" + this.waitingItemTrait.ToString() + "\ncanKeyboardCast=>" + this.canKeyboardCast.ToString() + "\ngameBusy=>" + this.gameBusy.ToString() + "\ngeneratedCardTimes=>" + this.generatedCardTimes.ToString() + "\ngameStatus=>" + this.gameStatus + "\nwaitingKill=>" + this.waitingKill.ToString() + "\nHandMask.gameObject.activeSelf=>" + this.HandMask.gameObject.activeSelf.ToString()));
    if (this.castingCardBlocked.Count > 0)
    {
      foreach (KeyValuePair<string, bool> keyValuePair in this.castingCardBlocked)
        Debug.Log((object) keyValuePair.Key);
    }
    if (this.eventList.Count <= 0)
      return;
    string message = "";
    for (int index = 0; index < this.eventList.Count; ++index)
      message = message + this.eventList[index] + " ";
    Debug.Log((object) message);
  }

  public void ShowCharactersPing(int _action)
  {
    for (int index = 0; index < this.TeamHero.Length; ++index)
    {
      if (this.TeamHero[index] != null && (UnityEngine.Object) this.TeamHero[index].HeroItem != (UnityEngine.Object) null && this.TeamHero[index].Alive)
        this.TeamHero[index].HeroItem.ShowCharacterPing(_action);
    }
    for (int index = 0; index < this.TeamNPC.Length; ++index)
    {
      if (this.TeamNPC[index] != null && (UnityEngine.Object) this.TeamNPC[index].NPCItem != (UnityEngine.Object) null && this.TeamNPC[index].Alive)
        this.TeamNPC[index].NPCItem.ShowCharacterPing(_action);
    }
  }

  public void HideCharactersPing()
  {
    for (int index = 0; index < this.TeamHero.Length; ++index)
    {
      if (this.TeamHero[index] != null && (UnityEngine.Object) this.TeamHero[index].HeroItem != (UnityEngine.Object) null && this.TeamHero[index].Alive)
        this.TeamHero[index].HeroItem.HideCharacterPing();
    }
    for (int index = 0; index < this.TeamNPC.Length; ++index)
    {
      if (this.TeamNPC[index] != null && (UnityEngine.Object) this.TeamNPC[index].NPCItem != (UnityEngine.Object) null && this.TeamNPC[index].Alive)
        this.TeamNPC[index].NPCItem.HideCharacterPing();
    }
  }

  public void SetCharactersPing(int _action)
  {
    if (this.waitingDeathScreen || this.WaitingForActionScreen() || this.emoteManager.IsBlocked() || !this.emoteManager.gameObject.activeSelf)
      return;
    this.emoteManager.HideEmotes();
    if (this.emoteManager.EmoteNeedsTarget(_action))
    {
      this.ShowCharactersPing(_action);
    }
    else
    {
      if (this.emoteManager.heroActive <= -1 || this.emoteManager.heroActive >= 4 || this.TeamHero[this.emoteManager.heroActive] == null)
        return;
      this.EmoteTarget(this.TeamHero[this.emoteManager.heroActive].Id, _action);
    }
  }

  public void ResetCharactersPing() => this.HideCharactersPing();

  [PunRPC]
  private void NET_EmoteTarget(string _id, byte _action, int _heroIndex)
  {
    if (this.TeamHero == null || this.TeamHero[_heroIndex] == null)
      return;
    if (NetworkManager.Instance.IsPlayerMutedByNick(this.TeamHero[_heroIndex].Owner))
      Debug.Log((object) "Blocked communication because player is muted");
    else
      this.EmoteTarget(_id, (int) _action, _heroIndex, true);
  }

  public void EmoteTarget(string _id, int _action, int _heroIndex = -1, bool _fromNet = false)
  {
    if (!_fromNet)
      _heroIndex = this.emoteManager.heroActive;
    if (!_fromNet && GameManager.Instance.IsMultiplayer())
      this.photonView.RPC("NET_EmoteTarget", RpcTarget.Others, (object) _id, (object) (byte) _action, (object) _heroIndex);
    Transform transform = (Transform) null;
    CharacterItem characterItem = (CharacterItem) null;
    for (int index = 0; index < this.TeamHero.Length; ++index)
    {
      if (this.TeamHero[index] != null && this.TeamHero[index].Alive && (UnityEngine.Object) this.TeamHero[index].HeroItem != (UnityEngine.Object) null && this.TeamHero[index].Id == _id)
      {
        transform = this.TeamHero[index].HeroItem.transform;
        characterItem = (CharacterItem) this.TeamHero[index].HeroItem;
        break;
      }
    }
    if ((UnityEngine.Object) transform == (UnityEngine.Object) null)
    {
      for (int index = 0; index < this.TeamNPC.Length; ++index)
      {
        if (this.TeamNPC[index] != null && this.TeamNPC[index].Alive && (UnityEngine.Object) this.TeamNPC[index].NPCItem != (UnityEngine.Object) null && this.TeamNPC[index].Id == _id)
        {
          transform = this.TeamNPC[index].NPCItem.transform;
          characterItem = (CharacterItem) this.TeamNPC[index].NPCItem;
          break;
        }
      }
    }
    if ((UnityEngine.Object) transform != (UnityEngine.Object) null && (UnityEngine.Object) characterItem != (UnityEngine.Object) null)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.emoteTargetPrefab, Vector3.zero, Quaternion.identity);
      gameObject.transform.position = characterItem.emoteCharacterPing.transform.position;
      gameObject.GetComponent<global::EmoteTarget>().SetIcons(_heroIndex, _action);
      GameManager.Instance.PlayLibraryAudio("Pop3", 2.9f);
      if (!_fromNet)
        this.emoteManager.SetBlocked(true);
    }
    if (_fromNet)
      return;
    this.ResetCharactersPing();
  }

  public void SendEmoteCard(int tablePosition)
  {
    if (this.heroActive <= -1)
      return;
    this.photonView.RPC("NET_SendEmoteCard", RpcTarget.Others, (object) (byte) tablePosition, (object) (byte) this.emoteManager.heroActive);
    this.ResetCharactersPing();
    this.DoEmoteCard((byte) tablePosition, (byte) this.emoteManager.heroActive);
  }

  [PunRPC]
  public void NET_SendEmoteCard(byte _tablePosition, byte _heroIndex)
  {
    if (this.TeamHero == null || this.TeamHero[(int) _heroIndex] == null)
      return;
    if (NetworkManager.Instance.IsPlayerMutedByNick(this.TeamHero[(int) _heroIndex].Owner))
      Debug.Log((object) "Blocked communication because player is muted");
    else
      this.DoEmoteCard(_tablePosition, _heroIndex);
  }

  private void DoEmoteCard(byte _tablePosition, byte _heroIndex)
  {
    int index1 = (int) _tablePosition;
    if (this.cardItemTable == null)
      return;
    if (this.cardItemTable.Count > index1 && (UnityEngine.Object) this.cardItemTable[index1] != (UnityEngine.Object) null && this.cardItemTable[index1].HaveEmoteIcon(_heroIndex))
    {
      this.cardItemTable[index1].RemoveEmoteIcon(_heroIndex);
    }
    else
    {
      for (int index2 = 0; index2 < this.cardItemTable.Count; ++index2)
      {
        if (index2 != (int) _tablePosition && (UnityEngine.Object) this.cardItemTable[index2] != (UnityEngine.Object) null)
          this.cardItemTable[index2].RemoveEmoteIcon(_heroIndex);
      }
      if (this.cardItemTable.Count <= index1 || !((UnityEngine.Object) this.cardItemTable[index1] != (UnityEngine.Object) null))
        return;
      this.cardItemTable[index1].ShowEmoteIcon(_heroIndex);
      if (!this.IsYourTurn())
        return;
      GameManager.Instance.PlayLibraryAudio("Pop6", 2.9f);
    }
  }

  public void CreateLogCardModification(string _cardId, Hero _theHero)
  {
    this.CreateLogEntry(false, "cardModification:" + this.logDictionary.Count.ToString(), _cardId, _theHero, (NPC) null, (Hero) null, (NPC) null, this.currentRound);
  }

  public void CreateLogEntry(
    bool _initial,
    string _key,
    string _cardId,
    Hero _theHero,
    NPC _theNPC,
    Hero _theHeroTarget,
    NPC _theNPCTarget,
    int _round = 0,
    Enums.EventActivation _event = Enums.EventActivation.None,
    int _auxInt = -1,
    string _auxString = "")
  {
    LogEntry logEntry = new LogEntry();
    logEntry.logCardId = _cardId;
    if (_theHero != null)
    {
      logEntry.logHero = _theHero;
      logEntry.logHeroName = _theHero.SourceName;
    }
    if (_theNPC != null)
    {
      logEntry.logNPC = _theNPC;
      logEntry.logNPCName = _theNPC.SourceName;
    }
    if (_theHeroTarget != null)
    {
      logEntry.logHeroTarget = _theHeroTarget;
      logEntry.logHeroTargetName = _theHeroTarget.SourceName;
    }
    if (_theNPCTarget != null)
    {
      logEntry.logNPCTarget = _theNPCTarget;
      logEntry.logNPCTargetName = _theNPCTarget.SourceName;
    }
    logEntry.logActivation = _event;
    logEntry.logRound = _round;
    logEntry.logDateTime = DateTime.Now.ToString("HH:mm");
    logEntry.logAuxInt = _auxInt;
    logEntry.logAuxString = _auxString;
    if (_key == "" && _event != Enums.EventActivation.None)
      _key = _event.ToString() + logEntry.logHeroName + logEntry.logNPCName + logEntry.logRound.ToString() + this.logDictionary.Count.ToString();
    if (!this.logDictionary.ContainsKey(_key))
    {
      LogResult logResult = new LogResult();
      this.logDictionary.Add(_key, logEntry);
    }
    this.CreateLogResult(_initial, _key);
  }

  private void CreateLogCastCard(
    bool _status,
    CardData _cardData,
    string _uniqueId,
    Hero _theHero,
    NPC _theNPC,
    Hero _hero,
    NPC _npc)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(_cardData.InternalId);
    stringBuilder.Append("_");
    stringBuilder.Append(_uniqueId);
    if (_cardData.TargetType == Enums.CardTargetType.Global || _cardData.TargetSide == Enums.CardTargetSide.Self)
      this.CreateLogEntry(_status, stringBuilder.ToString(), _cardData.Id, this.theHero, this.theNPC, (Hero) null, (NPC) null, this.currentRound);
    else
      this.CreateLogEntry(_status, stringBuilder.ToString(), _cardData.Id, this.theHero, this.theNPC, _hero, _npc, this.currentRound);
  }

  private void CreateLogResult(bool _initial, string _key)
  {
    bool flag = false;
    for (int index1 = 0; index1 < 8; ++index1)
    {
      Character character = index1 >= 4 ? (Character) this.TeamNPC[index1 - 4] : (Character) this.TeamHero[index1];
      if (character != null)
      {
        if (_initial)
        {
          LogResult logResult = new LogResult();
          logResult.logResultSprite = character.SpriteSpeed;
          logResult.logResultName = character.SourceName;
          Dictionary<string, int> dictionary = new Dictionary<string, int>();
          dictionary.Add("hp", character.GetHp());
          dictionary.Add("hpCurrent", character.GetHp());
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append(character.GetHp());
          if (index1 < 4)
          {
            for (int index2 = 0; index2 < this.HeroHand[index1].Count; ++index2)
            {
              CardData cardData = this.GetCardData(this.HeroHand[index1][index2], false);
              stringBuilder.Append(character.GetCardFinalCost(cardData));
            }
          }
          for (int index3 = 0; index3 < character.AuraList.Count; ++index3)
          {
            AuraCurseData acData = character.AuraList[index3].ACData;
            if ((UnityEngine.Object) acData != (UnityEngine.Object) null)
            {
              int auraCharges = character.AuraList[index3].AuraCharges;
              if (auraCharges > 0)
              {
                if (!dictionary.ContainsKey(acData.Id))
                  dictionary.Add(acData.Id, auraCharges);
                else
                  dictionary[acData.Id] = auraCharges;
                stringBuilder.Append(acData.Id);
                stringBuilder.Append(auraCharges);
              }
            }
          }
          if (this.logDictionary[_key].logActivation == Enums.EventActivation.TraitActivation)
          {
            foreach (KeyValuePair<string, int> activatedTrait in this.activatedTraits)
            {
              stringBuilder.Append(activatedTrait.Key);
              stringBuilder.Append(activatedTrait.Value);
            }
          }
          logResult.logResultMd5 = stringBuilder.ToString();
          logResult.logResultDict = dictionary;
          if (this.logDictionary.ContainsKey(_key))
          {
            if (this.logDictionary[_key].logResult == null)
              this.logDictionary[_key].logResult = new Dictionary<string, LogResult>();
            if (!this.logDictionary[_key].logResult.ContainsKey(character.Id))
              this.logDictionary[_key].logResult.Add(character.Id, logResult);
          }
        }
        else if (this.logDictionary.ContainsKey(_key) && this.logDictionary[_key].logResult != null && this.logDictionary[_key].logResult.ContainsKey(character.Id))
        {
          LogResult logResult = this.logDictionary[_key].logResult[character.Id];
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append(character.GetHp());
          if (index1 < 4)
          {
            for (int index4 = 0; index4 < this.HeroHand[index1].Count; ++index4)
            {
              CardData cardData = this.GetCardData(this.HeroHand[index1][index4], false);
              stringBuilder.Append(character.GetCardFinalCost(cardData));
            }
          }
          for (int index5 = 0; index5 < character.AuraList.Count; ++index5)
          {
            AuraCurseData acData = character.AuraList[index5].ACData;
            if ((UnityEngine.Object) acData != (UnityEngine.Object) null)
            {
              int auraCharges = character.AuraList[index5].AuraCharges;
              if (auraCharges > 0)
              {
                stringBuilder.Append(acData.Id);
                stringBuilder.Append(auraCharges);
              }
            }
          }
          if (this.logDictionary[_key].logActivation == Enums.EventActivation.TraitActivation)
          {
            foreach (KeyValuePair<string, int> activatedTrait in this.activatedTraits)
            {
              stringBuilder.Append(activatedTrait.Key);
              stringBuilder.Append(activatedTrait.Value);
            }
          }
          if (stringBuilder.ToString() == logResult.logResultMd5)
          {
            logResult.logResultDict = new Dictionary<string, int>();
          }
          else
          {
            flag = true;
            Dictionary<string, int> dictionary = logResult.logResultDict.ToDictionary<KeyValuePair<string, int>, string, int>((Func<KeyValuePair<string, int>, string>) (entry => entry.Key), (Func<KeyValuePair<string, int>, int>) (entry => entry.Value));
            if (!dictionary.ContainsKey("hp"))
              dictionary.Add("hp", 0);
            dictionary["hpCurrent"] = character.GetHp();
            dictionary["hp"] = character.GetHp() - dictionary["hp"];
            if (dictionary["hp"] == 0)
              dictionary.Remove("hp");
            for (int index6 = 0; index6 < character.AuraList.Count; ++index6)
            {
              AuraCurseData acData = character.AuraList[index6].ACData;
              if ((UnityEngine.Object) acData != (UnityEngine.Object) null)
              {
                int auraCharges = character.AuraList[index6].AuraCharges;
                if (dictionary.ContainsKey(acData.Id))
                  dictionary[acData.Id] = auraCharges - dictionary[acData.Id];
                else
                  dictionary.Add(acData.Id, auraCharges);
                if (dictionary[acData.Id] == 0)
                  dictionary.Remove(acData.Id);
              }
            }
            foreach (KeyValuePair<string, int> keyValuePair in logResult.logResultDict)
            {
              if (keyValuePair.Key != "hp" && keyValuePair.Key != "hpCurrent" && !character.HasEffect(keyValuePair.Key) && dictionary.ContainsKey(keyValuePair.Key))
                dictionary[keyValuePair.Key] = -dictionary[keyValuePair.Key];
            }
            logResult.logResultDict = dictionary;
            this.logDictionary[_key].logFinished = true;
          }
        }
      }
    }
    if (!_initial && !flag && this.logDictionary.ContainsKey(_key) && this.logDictionary[_key].logActivation == Enums.EventActivation.TraitActivation)
      this.logDictionary.Remove(_key);
    if (!((UnityEngine.Object) this.console != (UnityEngine.Object) null))
      return;
    this.console.DoLog();
  }

  public void ShowLog() => this.console.Show(!this.console.gameObject.activeSelf);

  public void ConsoleLgDbg()
  {
    StringBuilder stringBuilder = new StringBuilder();
    foreach (KeyValuePair<string, LogEntry> log in this.logDictionary)
    {
      stringBuilder.Append("**** ");
      stringBuilder.Append(log.Key);
      stringBuilder.Append(" ****\n");
      stringBuilder.Append("CardId -> ");
      stringBuilder.Append(log.Value.logCardId);
      stringBuilder.Append("\n");
      stringBuilder.Append("FromHero -> ");
      stringBuilder.Append((object) log.Value.logHero);
      stringBuilder.Append("\n");
      stringBuilder.Append("FromNPC -> ");
      stringBuilder.Append((object) log.Value.logNPC);
      stringBuilder.Append("\n");
      stringBuilder.Append("ToHero -> ");
      stringBuilder.Append((object) log.Value.logHeroTarget);
      stringBuilder.Append("\n");
      stringBuilder.Append("ToNPC -> ");
      stringBuilder.Append((object) log.Value.logNPCTarget);
      stringBuilder.Append("\n");
      stringBuilder.Append("Round -> ");
      stringBuilder.Append(log.Value.logRound);
      stringBuilder.Append("\n");
      stringBuilder.Append("Effects");
      foreach (KeyValuePair<string, LogResult> keyValuePair1 in log.Value.logResult)
      {
        stringBuilder.Append("\n");
        stringBuilder.Append(keyValuePair1.Key);
        stringBuilder.Append("=>");
        foreach (KeyValuePair<string, int> keyValuePair2 in keyValuePair1.Value.logResultDict)
        {
          stringBuilder.Append(keyValuePair2.Key);
          stringBuilder.Append("=>");
          stringBuilder.Append(keyValuePair2.Value);
          stringBuilder.Append(",");
        }
      }
      stringBuilder.Append("\n");
      stringBuilder.Append("[Log]");
      stringBuilder.Append("\n");
    }
    Debug.Log((object) stringBuilder.ToString());
  }

  public void DoStepSound()
  {
    if (!((UnityEngine.Object) this.combatData != (UnityEngine.Object) null))
      return;
    this.combatData.DoStepSound();
  }

  private IEnumerator CreateCardCastPet(CardData cardPet, GameObject charGO, Hero _hero, NPC _npc)
  {
    MatchManager matchManager = this;
    matchManager.CreatePet(cardPet, charGO, _hero, _npc, true, cardPet.InternalId);
    Transform _pet = charGO.transform.Find("thePetEnchantment" + cardPet.InternalId);
    if ((UnityEngine.Object) _pet != (UnityEngine.Object) null)
    {
      matchManager.eventList.Add(nameof (CreateCardCastPet));
      NPCItem _petNPCItem = _pet.GetComponent<NPCItem>();
      Animator _petAnimator = _pet.GetComponent<Animator>();
      _petNPCItem.InstantFadeOutCharacter();
      matchManager.StartCoroutine(_petNPCItem.FadeInCharacter());
      yield return (object) Globals.Instance.WaitForSeconds(0.5f);
      if (cardPet.PetTemporalAttack)
      {
        _petAnimator.ResetTrigger("attack");
        _petAnimator.SetTrigger("attack");
      }
      else if (cardPet.PetTemporalCast)
      {
        _petAnimator.ResetTrigger("attack");
        _petAnimator.SetTrigger("attack");
      }
      if (cardPet.PetTemporalMoveToCenter)
      {
        _petNPCItem.MoveToCenter();
        if (cardPet.PetTemporalMoveToBack)
        {
          yield return (object) Globals.Instance.WaitForSeconds(1f);
          _petNPCItem.MoveToCenterBack();
        }
      }
      if ((double) cardPet.PetTemporalFadeOutDelay > 0.0)
        yield return (object) Globals.Instance.WaitForSeconds(cardPet.PetTemporalFadeOutDelay);
      matchManager.StartCoroutine(_petNPCItem.FadeOutCharacter());
      yield return (object) Globals.Instance.WaitForSeconds(0.4f);
      UnityEngine.Object.Destroy((UnityEngine.Object) _pet.gameObject);
      matchManager.eventList.Remove(nameof (CreateCardCastPet));
      _petNPCItem = (NPCItem) null;
      _petAnimator = (Animator) null;
    }
  }

  public void ControllerMovement(
    bool goingUp = false,
    bool goingRight = false,
    bool goingDown = false,
    bool goingLeft = false,
    bool shoulderLeft = false,
    bool shoulderRight = false,
    int absoluteIndex = -1)
  {
    if ((UnityEngine.Object) this.energySelector == (UnityEngine.Object) null || this.CardDrag && goingDown | goingUp)
      return;
    if (this.energySelector.IsActive() && this.IsYourTurn())
    {
      if (goingLeft)
      {
        this.warpPosition = (Vector2) GameManager.Instance.cameraMain.WorldToScreenPoint(this.energySelector.buttonLess.transform.position);
        this.energySelector.AssignEnergyLess();
      }
      else if (goingRight)
      {
        this.warpPosition = (Vector2) GameManager.Instance.cameraMain.WorldToScreenPoint(this.energySelector.buttonMore.transform.position);
        this.energySelector.AssignEnergyMore();
      }
      else
        this.warpPosition = (Vector2) GameManager.Instance.cameraMain.WorldToScreenPoint(this.energySelector.buttonAccept.transform.position);
      Mouse.current.WarpCursorPosition(this.warpPosition);
    }
    else if (this.deathScreen.IsActive() && Functions.TransformIsVisible(this.deathScreen.button))
    {
      this.warpPosition = (Vector2) GameManager.Instance.cameraMain.WorldToScreenPoint(this.deathScreen.button.transform.position);
      Mouse.current.WarpCursorPosition(this.warpPosition);
    }
    else if (this.console.IsActive() && Functions.TransformIsVisible(this.consoleCloseButton))
    {
      this.warpPosition = (Vector2) GameManager.Instance.cameraMain.WorldToScreenPoint(this.consoleCloseButton.position);
      Mouse.current.WarpCursorPosition(this.warpPosition);
    }
    else
    {
      this.controllerList.Clear();
      if (this.deckCardsWindow.IsActive() || this.discardSelector.IsActive())
      {
        if (this.deckCardsWindow.IsActive())
        {
          if (this.deckCardsWindow.elements.gameObject.activeSelf)
          {
            foreach (Transform transform in this.deckCardsWindow.cardContainer)
            {
              if ((bool) (UnityEngine.Object) transform.GetComponent<CardItem>())
                this.controllerList.Add(transform);
            }
            if (Functions.TransformIsVisible(this.deckCardsWindow.buttonDiscard.GetChild(0)))
              this.controllerList.Add(this.deckCardsWindow.buttonDiscard.GetChild(0));
            this.controllerList.Add(this.deckCardsWindow.buttonHide.GetChild(0));
            this.controllerCurrentIndex = Functions.GetListClosestIndexToMousePosition(this.controllerList);
            this.controllerCurrentIndex = Functions.GetClosestIndexBasedOnDirection(this.controllerList, this.controllerCurrentIndex, goingUp, goingRight, goingDown, goingLeft);
            if (this.controllerCurrentIndex >= 0 && this.controllerCurrentIndex < this.controllerList.Count - 2)
            {
              Canvas.ForceUpdateCanvases();
              Vector3 zero = Vector3.zero with
              {
                x = this.deckCardsWindow.cardContainerRT.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.x,
                y = this.deckCardsWindow.cardContainer.childCount > 10 ? (float) (1.6499999761581421 * (double) Mathf.Floor((float) this.deckCardsWindow.cardContainer.childCount / 5f) - 4.5 + 3.0999999046325684 * (double) Mathf.Floor((float) this.controllerCurrentIndex / 5f)) : -4.5f
              };
              this.deckCardsWindow.cardContainerRT.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = (Vector2) zero;
            }
            this.warpPosition = (Vector2) GameManager.Instance.cameraMain.WorldToScreenPoint(this.controllerList[this.controllerCurrentIndex].position);
            Mouse.current.WarpCursorPosition(this.warpPosition);
          }
          else
          {
            this.controllerList.Add(this.deckCardsWindow.buttonHide.GetChild(0));
            for (int index = this.TeamHero.Length - 1; index >= 0; --index)
            {
              if (this.TeamHero[index] != null && this.TeamHero[index].Alive && (UnityEngine.Object) this.TeamHero[index].HeroItem != (UnityEngine.Object) null)
                this.controllerList.Add(this.TeamHero[index].HeroItem.characterTransform);
            }
            for (int index = this.TeamNPC.Length - 1; index >= 0; --index)
            {
              if (this.TeamNPC[index] != null && this.TeamNPC[index].Alive && (UnityEngine.Object) this.TeamNPC[index].NPCItem != (UnityEngine.Object) null)
                this.controllerList.Add(this.TeamNPC[index].NPCItem.characterTransform);
            }
            this.controllerCurrentIndex = Functions.GetListClosestIndexToMousePosition(this.controllerList);
            this.controllerCurrentIndex = Functions.GetClosestIndexBasedOnDirection(this.controllerList, this.controllerCurrentIndex, goingUp, goingRight, goingDown, goingLeft);
            this.warpPosition = (Vector2) GameManager.Instance.cameraMain.WorldToScreenPoint(this.controllerList[this.controllerCurrentIndex].position);
            Mouse.current.WarpCursorPosition(this.warpPosition);
          }
        }
        else
        {
          if (!this.discardSelector.IsActive())
            return;
          if (this.discardSelector.elements.gameObject.activeSelf)
          {
            foreach (Transform transform in this.discardSelector.cardContainer)
            {
              if ((bool) (UnityEngine.Object) transform.GetComponent<CardItem>())
                this.controllerList.Add(transform);
            }
            if (Functions.TransformIsVisible(this.discardSelector.button.GetChild(0)))
              this.controllerList.Add(this.discardSelector.button.GetChild(0));
            this.controllerList.Add(this.discardSelector.buttonHide.GetChild(0));
            this.controllerCurrentIndex = Functions.GetListClosestIndexToMousePosition(this.controllerList);
            this.controllerCurrentIndex = Functions.GetClosestIndexBasedOnDirection(this.controllerList, this.controllerCurrentIndex, goingUp, goingRight, goingDown, goingLeft);
            this.warpPosition = (Vector2) GameManager.Instance.cameraMain.WorldToScreenPoint(this.controllerList[this.controllerCurrentIndex].position);
            Mouse.current.WarpCursorPosition(this.warpPosition);
          }
          else
          {
            this.controllerList.Add(this.discardSelector.buttonHide.GetChild(0));
            for (int index = this.TeamHero.Length - 1; index >= 0; --index)
            {
              if (this.TeamHero[index] != null && this.TeamHero[index].Alive && (UnityEngine.Object) this.TeamHero[index].HeroItem != (UnityEngine.Object) null)
                this.controllerList.Add(this.TeamHero[index].HeroItem.characterTransform);
            }
            for (int index = this.TeamNPC.Length - 1; index >= 0; --index)
            {
              if (this.TeamNPC[index] != null && this.TeamNPC[index].Alive && (UnityEngine.Object) this.TeamNPC[index].NPCItem != (UnityEngine.Object) null)
                this.controllerList.Add(this.TeamNPC[index].NPCItem.characterTransform);
            }
            this.controllerCurrentIndex = Functions.GetListClosestIndexToMousePosition(this.controllerList);
            this.controllerCurrentIndex = Functions.GetClosestIndexBasedOnDirection(this.controllerList, this.controllerCurrentIndex, goingUp, goingRight, goingDown, goingLeft);
            this.warpPosition = (Vector2) GameManager.Instance.cameraMain.WorldToScreenPoint(this.controllerList[this.controllerCurrentIndex].position);
            Mouse.current.WarpCursorPosition(this.warpPosition);
          }
        }
      }
      else
      {
        int num1 = -1;
        int num2 = -1;
        if (!this.CardDrag)
        {
          if (this.heroTurn)
          {
            if (this.CountHeroDeck() > 0)
              this.controllerList.Add(this.GO_DecksObject.transform);
            if (this.CountHeroDiscard() > 0)
              this.controllerList.Add(this.GO_DiscardPile.transform.GetChild(0).transform);
          }
          if (this.cardItemTable != null)
          {
            for (int index = 0; index < this.cardItemTable.Count; ++index)
              this.controllerList.Add(this.cardItemTable[index].transform);
          }
          if (this.heroTurn)
          {
            if (this.IsYourTurn())
            {
              num1 = this.controllerList.Count;
              this.controllerList.Add(this.botEndTurn);
            }
            if (this.iconWeapon.GetComponent<BoxCollider2D>().enabled)
              this.controllerList.Add(this.iconWeapon.transform);
            if (this.iconArmor.GetComponent<BoxCollider2D>().enabled)
              this.controllerList.Add(this.iconArmor.transform);
            if (this.iconJewelry.GetComponent<BoxCollider2D>().enabled)
              this.controllerList.Add(this.iconJewelry.transform);
            if (this.iconAccesory.GetComponent<BoxCollider2D>().enabled)
              this.controllerList.Add(this.iconAccesory.transform);
            if (this.iconPet.GetComponent<BoxCollider2D>().enabled)
              this.controllerList.Add(this.iconPet.transform);
          }
        }
        int count = this.controllerList.Count;
        for (int index = this.TeamHero.Length - 1; index >= 0; --index)
        {
          if (this.TeamHero[index] != null && this.TeamHero[index].Alive && (UnityEngine.Object) this.TeamHero[index].HeroItem != (UnityEngine.Object) null)
            this.controllerList.Add(this.TeamHero[index].HeroItem.characterTransform);
        }
        for (int index = 0; index < this.TeamNPC.Length; ++index)
        {
          if (this.TeamNPC[index] != null && this.TeamNPC[index].Alive && (UnityEngine.Object) this.TeamNPC[index].NPCItem != (UnityEngine.Object) null)
          {
            num2 = this.controllerList.Count;
            this.controllerList.Add(this.TeamNPC[index].NPCItem.characterTransform);
          }
        }
        if (!this.CardDrag)
        {
          for (int index = 0; index < this.TeamHero.Length; ++index)
          {
            if (this.TeamHero[index] != null && this.TeamHero[index].Alive && (UnityEngine.Object) this.TeamHero[index].HeroItem != (UnityEngine.Object) null)
            {
              if (Functions.TransformIsVisible(this.TeamHero[index].HeroItem.emoteCharacterPing.transform))
                this.controllerList.Add(this.TeamHero[index].HeroItem.emoteCharacterPing.transform);
              if (Functions.TransformIsVisible(this.TeamHero[index].HeroItem.iconEnchantment.transform))
              {
                this.controllerList.Add(this.TeamHero[index].HeroItem.iconEnchantment.transform);
                if (Functions.TransformIsVisible(this.TeamHero[index].HeroItem.iconEnchantment2.transform))
                {
                  this.controllerList.Add(this.TeamHero[index].HeroItem.iconEnchantment2.transform);
                  if (Functions.TransformIsVisible(this.TeamHero[index].HeroItem.iconEnchantment3.transform))
                    this.controllerList.Add(this.TeamHero[index].HeroItem.iconEnchantment3.transform);
                }
              }
            }
          }
          for (int index = 0; index < this.TeamNPC.Length; ++index)
          {
            if (this.TeamNPC[index] != null && this.TeamNPC[index].Alive && (UnityEngine.Object) this.TeamNPC[index].NPCItem != (UnityEngine.Object) null)
            {
              foreach (Transform transform in this.TeamNPC[index].NPCItem.cardsGOT)
              {
                if ((bool) (UnityEngine.Object) transform.GetComponent<CardItem>() && transform.GetComponent<CardItem>().IsRevealed())
                  this.controllerList.Add(transform);
              }
              if (Functions.TransformIsVisible(this.TeamNPC[index].NPCItem.emoteCharacterPing.transform))
                this.controllerList.Add(this.TeamNPC[index].NPCItem.emoteCharacterPing.transform);
              if (Functions.TransformIsVisible(this.TeamNPC[index].NPCItem.iconEnchantment.transform))
              {
                this.controllerList.Add(this.TeamNPC[index].NPCItem.iconEnchantment.transform);
                if (Functions.TransformIsVisible(this.TeamNPC[index].NPCItem.iconEnchantment2.transform))
                {
                  this.controllerList.Add(this.TeamNPC[index].NPCItem.iconEnchantment2.transform);
                  if (Functions.TransformIsVisible(this.TeamNPC[index].NPCItem.iconEnchantment3.transform))
                    this.controllerList.Add(this.TeamNPC[index].NPCItem.iconEnchantment3.transform);
                }
              }
            }
          }
          if (Functions.TransformIsVisible(this.iconCorruption.transform))
            this.controllerList.Add(this.iconCorruption.transform);
          foreach (Transform transform in this.GO_Initiative.transform)
          {
            if (Functions.TransformIsVisible(transform))
              this.controllerList.Add(transform);
          }
          for (int index = 0; index < this.emotesTransform.Count; ++index)
          {
            if (Functions.TransformIsVisible(this.emotesTransform[index]))
              this.controllerList.Add(this.emotesTransform[index]);
          }
        }
        this.controllerCurrentIndex = absoluteIndex <= -1 ? Functions.GetListClosestIndexToMousePosition(this.controllerList) : absoluteIndex;
        if (this.controllerCurrentIndex == -1 || this.controllerList.Count == 0)
          return;
        if (num1 > -1 && this.controllerCurrentIndex <= num1 && goingRight | goingLeft | goingDown)
        {
          if (goingRight)
          {
            if (this.controllerCurrentIndex == num1)
              return;
            ++this.controllerCurrentIndex;
          }
          else if (goingLeft)
          {
            --this.controllerCurrentIndex;
            if (this.controllerCurrentIndex < 0)
              this.controllerCurrentIndex = 0;
          }
          else if (goingDown)
          {
            if (this.controllerCurrentIndex != num1)
              return;
            ++this.controllerCurrentIndex;
          }
        }
        else if (this.controllerCurrentIndex >= count && this.controllerCurrentIndex <= num2 && goingRight | goingLeft)
        {
          if (goingRight)
          {
            if (this.controllerCurrentIndex == num2)
              return;
            ++this.controllerCurrentIndex;
          }
          else
          {
            if (this.controllerCurrentIndex == count)
              return;
            --this.controllerCurrentIndex;
          }
        }
        else
          this.controllerCurrentIndex = Functions.GetClosestIndexBasedOnDirection(this.controllerList, this.controllerCurrentIndex, goingUp, goingRight, goingDown, goingLeft);
        if (this.controllerCurrentIndex == -1 || !((UnityEngine.Object) this.controllerList[this.controllerCurrentIndex] != (UnityEngine.Object) null))
          return;
        if ((bool) (UnityEngine.Object) this.controllerList[this.controllerCurrentIndex].parent.GetComponent<HeroItem>() || (bool) (UnityEngine.Object) this.controllerList[this.controllerCurrentIndex].parent.GetComponent<NPCItem>())
        {
          this.controllerOffsetVector = Vector3.zero;
          this.controllerOffsetVector.y = -0.5f;
          this.warpPosition = (Vector2) GameManager.Instance.cameraMain.WorldToScreenPoint(this.controllerList[this.controllerCurrentIndex].position + this.controllerOffsetVector);
        }
        else if ((UnityEngine.Object) this.controllerList[this.controllerCurrentIndex].GetComponent<CardItem>() != (UnityEngine.Object) null && this.controllerList[this.controllerCurrentIndex].GetComponent<CardItem>().enabled && (UnityEngine.Object) this.controllerList[this.controllerCurrentIndex].parent.transform == (UnityEngine.Object) this.GO_Hand.transform)
        {
          this.controllerOffsetVector = Vector3.zero;
          this.controllerOffsetVector.y = 1.25f;
          if (goingRight)
            this.controllerOffsetVector.x = -0.6f;
          else if (goingLeft)
            this.controllerOffsetVector.x = 0.5f;
          this.warpPosition = (Vector2) GameManager.Instance.cameraMain.WorldToScreenPoint(this.controllerList[this.controllerCurrentIndex].position + this.controllerOffsetVector);
        }
        else
          this.warpPosition = (Vector2) GameManager.Instance.cameraMain.WorldToScreenPoint(this.controllerList[this.controllerCurrentIndex].position);
        Mouse.current.WarpCursorPosition(this.warpPosition);
      }
    }
  }

  public void ControllerMoveShoulder(bool _isRight = false)
  {
  }

  public void SetControllerCardClicked()
  {
    this.controllerClickedCardIndex = this.controllerCurrentIndex;
    this.controllerClickedCard = true;
    this.MoveControllerToHeroes();
  }

  public void MoveControllerToHeroes()
  {
    bool flag = false;
    int num = -1;
    int absoluteIndex = 0;
    if ((UnityEngine.Object) this.cardItemActive != (UnityEngine.Object) null && (UnityEngine.Object) this.cardItemActive.CardData != (UnityEngine.Object) null)
    {
      for (int index = 0; index < this.TeamHero.Length; ++index)
      {
        if (this.TeamHero[index] != null && this.TeamHero[index].Alive)
        {
          ++num;
          if (this.CheckTarget(this.TeamHero[index].HeroItem.transform, this.cardItemActive.CardData))
          {
            absoluteIndex = this.NumHeroesAlive() - 1 - num;
            flag = true;
            break;
          }
        }
      }
      if (!flag)
      {
        for (int index = 0; index < this.TeamNPC.Length; ++index)
        {
          if (this.TeamNPC[index] != null && this.TeamNPC[index].Alive)
          {
            ++num;
            if (this.CheckTarget(this.TeamNPC[index].NPCItem.transform, this.cardItemActive.CardData))
            {
              absoluteIndex = num;
              break;
            }
          }
        }
      }
    }
    this.ControllerMovement(absoluteIndex: absoluteIndex);
  }

  public void ResetController()
  {
    if (!this.IsYourTurn() || !this.controllerClickedCard)
      return;
    this.ResetControllerPositions();
    this.ResetControllerClickedCard();
  }

  private int ResolveAuraCurseCharges(
    AuraCurseData auraCurse,
    int baseCharges,
    bool useGlobal,
    bool useSpecialValue1,
    bool useSpecialValue2,
    Character targetCharacter,
    int cardSpecialValueGlobal,
    int cardSpecialValue1,
    int cardSpecialValue2,
    int? additionalAuraCharges = null)
  {
    if (targetCharacter != null && targetCharacter.Alive && !auraCurse.IsAura && targetCharacter.IsSleep())
      return this.CalculateSleepCharges(targetCharacter);
    int baseCharges1 = baseCharges;
    if (useGlobal)
      baseCharges1 = cardSpecialValueGlobal;
    else if (useSpecialValue1)
      baseCharges1 = cardSpecialValue1;
    else if (useSpecialValue2)
      baseCharges1 = cardSpecialValue2;
    else if (additionalAuraCharges.HasValue && auraCurse.IsAura)
      baseCharges1 = additionalAuraCharges.GetValueOrDefault();
    return this.GetEffectiveCharges(auraCurse, baseCharges1, targetCharacter);
  }

  private void ResetControllerPositions(bool forceIt = false)
  {
    if (!this.IsYourTurn() || !forceIt && !this.controllerClickedCard)
      return;
    this.StartCoroutine(this.ResetControllerPositionsAction());
  }

  private IEnumerator ResetControllerPositionsAction()
  {
    yield return (object) Globals.Instance.WaitForSeconds(0.15f);
    int absoluteIndex = 0;
    if (this.heroTurn)
    {
      if (this.CountHeroDeck() > 0)
        ++absoluteIndex;
      if (this.CountHeroDiscard() > 0)
        ++absoluteIndex;
    }
    this.ControllerMovement(absoluteIndex: absoluteIndex);
  }

  private void ResetControllerClickedCard()
  {
    if (!this.IsYourTurn() || !this.controllerClickedCard)
      return;
    this.controllerClickedCard = false;
    this.controllerClickedCardIndex = -1;
  }

  public void ControllerStopDrag() => this.cardItemActive.DoReturnCardToDeckFromDrag();

  public void ControllerExecute() => this.cardItemActive.OnMouseUp();

  private int CalculateSleepCharges(Character target)
  {
    if (target == null || !target.Alive || target.GetCurseList().Contains("sleep"))
      return 0;
    return (double) target.GetHpPercent() <= 50.0 ? 2 : 1;
  }

  private int GetEffectiveCharges(AuraCurseData curse, int baseCharges, Character character)
  {
    if (character == null || !character.Alive)
      return baseCharges;
    int max = curse.MaxCharges < 0 ? int.MaxValue : curse.MaxCharges;
    int chargeMultiplier = this.GetChargeMultiplier(curse, character);
    return Mathf.Clamp(baseCharges * chargeMultiplier, 0, max);
  }

  private int GetChargeMultiplier(AuraCurseData curse, Character character)
  {
    int chargeMultiplier = 1;
    if (character == null || !character.Alive || curse.IsAura || !character.GetCurseList().Contains("sleep"))
      return chargeMultiplier;
    chargeMultiplier *= 2;
    return chargeMultiplier;
  }

  public bool CardDrag
  {
    get => this.cardDrag;
    set => this.cardDrag = value;
  }

  public CardData CardActive => this.cardActive;

  public CardItem CardItemActive
  {
    get => this.cardItemActive;
    set => this.cardItemActive = value;
  }

  public Transform CardActiveT
  {
    get => this.cardActiveT;
    set => this.cardActiveT = value;
  }

  public string GameStatus
  {
    get => this.gameStatus;
    set => this.gameStatus = value;
  }

  public UICombatDeath DeathScreen => this.deathScreen;

  public UIEnergySelector EnergySelector => this.energySelector;

  public UIDeckCards DeckCardsWindow
  {
    get => this.deckCardsWindow;
    set => this.deckCardsWindow = value;
  }

  public int CardsWaitingForReset
  {
    get => this.cardsWaitingForReset;
    set => this.cardsWaitingForReset = value;
  }

  public bool IsBeginTournPhase
  {
    get => this.isBeginTournPhase;
    set => this.isBeginTournPhase = value;
  }

  public bool MatchIsOver
  {
    get => this.matchIsOver;
    set => this.matchIsOver = value;
  }

  public bool WaitingForCardEnergyAssignment
  {
    get => this.waitingForCardEnergyAssignment;
    set => this.waitingForCardEnergyAssignment = value;
  }

  public string CurrentGameCodeForReload
  {
    get => this.currentGameCodeForReload;
    set => this.currentGameCodeForReload = value;
  }

  public bool CombatLoading
  {
    get => this.combatLoading;
    set => this.combatLoading = value;
  }

  public Dictionary<string, LogEntry> LogDictionary
  {
    get => this.logDictionary;
    set => this.logDictionary = value;
  }

  public int PreCastNum
  {
    get => this.preCastNum;
    set => this.preCastNum = value;
  }

  public List<string> EventList => this.eventList;

  public UIDiscardSelector DiscardSelector
  {
    get => this.discardSelector;
    set => this.discardSelector = value;
  }

  public bool KeyClickedCard
  {
    get => this.keyClickedCard;
    set => this.keyClickedCard = value;
  }

  public bool WaitingForDiscardAssignment
  {
    get => this.waitingForDiscardAssignment;
    set => this.waitingForDiscardAssignment = value;
  }

  public bool WaitingForAddcardAssignment
  {
    get => this.waitingForAddcardAssignment;
    set => this.waitingForAddcardAssignment = value;
  }

  public bool WaitingForLookDiscardWindow
  {
    get => this.waitingForLookDiscardWindow;
    set => this.waitingForLookDiscardWindow = value;
  }

  public int HitSoundIndex
  {
    get => this.hitSoundIndex;
    set => this.hitSoundIndex = value;
  }

  public void SwapNPCDeck(int index1, int index2)
  {
    List<string>[] npcDeck1 = this.NPCDeck;
    int index3 = index1;
    List<string>[] npcDeck2 = this.NPCDeck;
    int num = index2;
    List<string> stringList1 = this.NPCDeck[index2];
    List<string> stringList2 = this.NPCDeck[index1];
    npcDeck1[index3] = stringList1;
    int index4 = num;
    List<string> stringList3 = stringList2;
    npcDeck2[index4] = stringList3;
  }

  public void SwapNPCDeckDiscard(int index1, int index2)
  {
    List<string>[] npcDeckDiscard1 = this.NPCDeckDiscard;
    int index3 = index1;
    List<string>[] npcDeckDiscard2 = this.NPCDeckDiscard;
    int num = index2;
    List<string> stringList1 = this.NPCDeckDiscard[index2];
    List<string> stringList2 = this.NPCDeckDiscard[index1];
    npcDeckDiscard1[index3] = stringList1;
    int index4 = num;
    List<string> stringList3 = stringList2;
    npcDeckDiscard2[index4] = stringList3;
  }

  public void SwapNPCHand(int index1, int index2)
  {
    List<string>[] npcHand1 = this.NPCHand;
    int index3 = index1;
    List<string>[] npcHand2 = this.NPCHand;
    int num = index2;
    List<string> stringList1 = this.NPCHand[index2];
    List<string> stringList2 = this.NPCHand[index1];
    npcHand1[index3] = stringList1;
    int index4 = num;
    List<string> stringList3 = stringList2;
    npcHand2[index4] = stringList3;
  }

  public List<string> GetNPCDeck(int index) => this.NPCDeck[index];

  public List<string> GetNPCHand(int index) => this.NPCHand[index];

  public void SetNPCDeck(int index, List<string> newDeck) => this.NPCDeck[index] = newDeck;

  public void SetNPCHand(int index, List<string> newHand) => this.NPCHand[index] = newHand;

  public void SwapCharacterOrder(string charId1, string charId2, int index1, int index2)
  {
    int index3 = this.CharOrder.FindIndex((Predicate<MatchManager.CharacterForOrder>) (x => x.id == charId1));
    int index4 = this.CharOrder.FindIndex((Predicate<MatchManager.CharacterForOrder>) (x => x.id == charId2));
    if (index3 >= 0 && index4 >= 0)
    {
      MatchManager.CharacterForOrder characterForOrder1 = this.CharOrder[index3];
      MatchManager.CharacterForOrder characterForOrder2 = this.CharOrder[index4];
      int index5 = this.CharOrder[index4].index;
      int index6 = this.CharOrder[index3].index;
      characterForOrder1.index = index5;
      int num1 = index6;
      characterForOrder2.index = num1;
      List<MatchManager.CharacterForOrder> charOrder1 = this.CharOrder;
      int num2 = index3;
      List<MatchManager.CharacterForOrder> charOrder2 = this.CharOrder;
      int index7 = index4;
      MatchManager.CharacterForOrder characterForOrder3 = this.CharOrder[index4];
      MatchManager.CharacterForOrder characterForOrder4 = this.CharOrder[index3];
      int index8 = num2;
      MatchManager.CharacterForOrder characterForOrder5;
      MatchManager.CharacterForOrder characterForOrder6 = characterForOrder5 = characterForOrder3;
      charOrder1[index8] = characterForOrder5;
      charOrder2[index7] = characterForOrder6 = characterForOrder4;
    }
    else
      Debug.LogError((object) "Character not found in CharOrder array of match manager.");
  }

  public void AddVanishToDeck(int index, bool isHero)
  {
    Debug.Log((object) ("XXX Hand: " + this.HeroDeck?.ToString()));
    Debug.Log((object) ("XXX Deck: " + this.HeroHand?.ToString()));
    if (isHero)
    {
      foreach (string id in this.HeroDeck[index])
        this.GetCardData(id).Vanish = true;
      foreach (string id in this.HeroHand[index])
      {
        this.GetCardData(id).Vanish = true;
        foreach (CardItem componentsInChild in this.GO_Hand.GetComponentsInChildren<CardItem>())
          componentsInChild.SetVanishIcon(true);
      }
    }
    else
    {
      foreach (string id in this.NPCDeck[index])
        this.GetCardData(id).Vanish = true;
      foreach (string id in this.NPCHand[index])
        this.GetCardData(id).Vanish = true;
    }
  }

  public bool IsPhantomArmorSpecialCard(string cardId)
  {
    return this.bossNpc != null && this.bossNpc is PhantomArmor && (this.bossNpc as PhantomArmor).IsSpecialCard(cardId);
  }

  public Transform GetNPCParent() => this.GO_NPCs.transform;

  public class CharacterForOrder
  {
    public int index;
    public string id;
    public int[] speed;
    public float speedForOrder;
    public Hero hero;
    public HeroItem heroItem;
    public NPC npc;
    public NPCItem npcItem;
    public InitiativePortrait initiativePortrait;
  }

  [Serializable]
  public class CardDataForShare
  {
    public int energyCost;
    public bool energyCostChangePermanent;
    public bool vanish;
    public int energyReductionPermanent;
    public int energyReductionTemporal;
    public bool energyReductionToZeroPermanent;
    public bool energyReductionToZeroTemporal;
    public string originalId;
  }
}
