using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cards;
using CustomAbilities;
using NPCs;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using WebSocketSharp;

public class MatchManager : MonoBehaviour
{
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

	private struct DamageReflectedPayload
	{
		public Hero theCasterHero;

		public NPC theCasterNPC;

		public int dmgTotal;

		public int blocked;

		public DamageReflectedPayload(Hero theCasterHero, NPC theCasterNPC, int dmgTotal, int blocked)
		{
			this.theCasterHero = theCasterHero;
			this.theCasterNPC = theCasterNPC;
			this.dmgTotal = dmgTotal;
			this.blocked = blocked;
		}

		public static DamageReflectedPayload operator +(DamageReflectedPayload left, DamageReflectedPayload right)
		{
			return new DamageReflectedPayload
			{
				theCasterHero = left.theCasterHero,
				theCasterNPC = left.theCasterNPC,
				dmgTotal = left.dmgTotal + right.dmgTotal,
				blocked = left.blocked + right.blocked
			};
		}
	}

	private struct HealAttackerPayload
	{
		public Hero theCasterHero;

		public NPC theCasterNPC;

		public HealAttackerPayload(Hero theCasterHero, NPC theCasterNPC)
		{
			this.theCasterHero = theCasterHero;
			this.theCasterNPC = theCasterNPC;
		}
	}

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
	private List<CharacterForOrder> CharOrder;

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

	private Vector3 DeckPileOutOfScreenPositionVector = new Vector3(6f, 0f, 0f);

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

	public BossNPC BossNpc;

	public GameObject phantomArmorPrefab;

	public GameObject phantomArmorVfxPrefab;

	public List<ModelVisualsUpdater> modelVisualsUpdaters;

	public List<string> ItemTraitsActivatedSinceLastCardCast;

	private MindSpikeAbility mindSpikeAbility;

	private Aura auraCurse;

	private readonly Dictionary<string, Dictionary<Character, bool>> auraStates = new Dictionary<string, Dictionary<Character, bool>>(StringComparer.OrdinalIgnoreCase);

	private readonly TraitEnum[] alwaysActiveTraits = new TraitEnum[3]
	{
		TraitEnum.PurifyingResonance,
		TraitEnum.DarkMercy,
		TraitEnum.DarkOverflow
	};

	public Transform VignetteSprite;

	public static Action<NPCData, HeroData, int> OnCharacterKilled;

	public static Action<Character, int, int> OnCharacterDamaged;

	private bool doItemCoScheduledThisFrame;

	private Dictionary<string, int> itemsActivatedThisFrame = new Dictionary<string, int>();

	public MindSpikeAbility MindSpikeAbility => mindSpikeAbility;

	public static MatchManager Instance { get; private set; }

	public bool CardDrag
	{
		get
		{
			return cardDrag;
		}
		set
		{
			cardDrag = value;
		}
	}

	public CardData CardActive => cardActive;

	public CardItem CardItemActive
	{
		get
		{
			return cardItemActive;
		}
		set
		{
			cardItemActive = value;
		}
	}

	public Transform CardActiveT
	{
		get
		{
			return cardActiveT;
		}
		set
		{
			cardActiveT = value;
		}
	}

	public string GameStatus
	{
		get
		{
			return gameStatus;
		}
		set
		{
			gameStatus = value;
		}
	}

	public UICombatDeath DeathScreen => deathScreen;

	public UIEnergySelector EnergySelector => energySelector;

	public UIDeckCards DeckCardsWindow
	{
		get
		{
			return deckCardsWindow;
		}
		set
		{
			deckCardsWindow = value;
		}
	}

	public int CardsWaitingForReset
	{
		get
		{
			return cardsWaitingForReset;
		}
		set
		{
			cardsWaitingForReset = value;
		}
	}

	public bool IsBeginTournPhase
	{
		get
		{
			return isBeginTournPhase;
		}
		set
		{
			isBeginTournPhase = value;
		}
	}

	public bool MatchIsOver
	{
		get
		{
			return matchIsOver;
		}
		set
		{
			matchIsOver = value;
		}
	}

	public bool WaitingForCardEnergyAssignment
	{
		get
		{
			return waitingForCardEnergyAssignment;
		}
		set
		{
			waitingForCardEnergyAssignment = value;
		}
	}

	public string CurrentGameCodeForReload
	{
		get
		{
			return currentGameCodeForReload;
		}
		set
		{
			currentGameCodeForReload = value;
		}
	}

	public bool CombatLoading
	{
		get
		{
			return combatLoading;
		}
		set
		{
			combatLoading = value;
		}
	}

	public Dictionary<string, LogEntry> LogDictionary
	{
		get
		{
			return logDictionary;
		}
		set
		{
			logDictionary = value;
		}
	}

	public int PreCastNum
	{
		get
		{
			return preCastNum;
		}
		set
		{
			preCastNum = value;
		}
	}

	public List<string> EventList => eventList;

	public UIDiscardSelector DiscardSelector
	{
		get
		{
			return discardSelector;
		}
		set
		{
			discardSelector = value;
		}
	}

	public bool KeyClickedCard
	{
		get
		{
			return keyClickedCard;
		}
		set
		{
			keyClickedCard = value;
		}
	}

	public bool WaitingForDiscardAssignment
	{
		get
		{
			return waitingForDiscardAssignment;
		}
		set
		{
			waitingForDiscardAssignment = value;
		}
	}

	public bool WaitingForAddcardAssignment
	{
		get
		{
			return waitingForAddcardAssignment;
		}
		set
		{
			waitingForAddcardAssignment = value;
		}
	}

	public bool WaitingForLookDiscardWindow
	{
		get
		{
			return waitingForLookDiscardWindow;
		}
		set
		{
			waitingForLookDiscardWindow = value;
		}
	}

	public int HitSoundIndex
	{
		get
		{
			return hitSoundIndex;
		}
		set
		{
			hitSoundIndex = value;
		}
	}

	private void Awake()
	{
		if (GameManager.Instance == null)
		{
			SceneStatic.LoadByName("TeamManagement");
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
		botEndTurnAnim = botEndTurn.GetComponent<Animator>();
		deckCounterTM = deckCounter.GetChild(0).GetComponent<TMP_Text>();
		discardCounterTM = discardCounter.GetChild(0).GetComponent<TMP_Text>();
		newCardsCounterTM = newCardsCounter.GetChild(0).GetComponent<TMP_Text>();
		sceneCamera.gameObject.SetActive(value: false);
		photonView = PhotonView.Get(this);
		BossNpc = null;
		AlertManager.Instance.HideAlert();
		ShowMaskFull();
		NetworkManager.Instance.StartStopQueue(state: true);
		modelVisualsUpdaters = new List<ModelVisualsUpdater>();
		mindSpikeAbility = new MindSpikeAbility();
	}

	private void Update()
	{
		sinceLastUpdate += Time.deltaTime;
		if (sinceLastUpdate < updateTimeout)
		{
			return;
		}
		sinceLastUpdate = 0f;
		combatCounter++;
		if (MatchIsOver)
		{
			return;
		}
		if (combatCounter % 2 == 0)
		{
			if (!amplifiedTransformShow && amplifiedTransform.childCount > 0)
			{
				foreach (Transform item in amplifiedTransform)
				{
					UnityEngine.Object.Destroy(item.gameObject);
				}
			}
			amplifiedTransformShow = false;
		}
		if (heroActive <= -1 || (combatCounter % 2 != 0 && GameManager.Instance.configGameSpeed != Enums.ConfigSpeed.Ultrafast))
		{
			return;
		}
		if (heroTurn && IsYourTurn() && !waitingTutorial)
		{
			if (gameStatus != "CastCard" && cardsWaitingForReset == 0 && eventList.Count == 0 && !isBeginTournPhase && castingCardBlocked.Count == 0 && !waitingItemTrait && generatedCardTimes == 0)
			{
				ResetFailCount();
				if (autoEndCount < limitAutoEndCount)
				{
					getStatusString = GenerateSyncCodeForCheckingAction();
					if (getStatusString == lastStatusString)
					{
						autoEndCount++;
					}
					else
					{
						ResetAutoEndCount();
					}
					lastStatusString = GenerateSyncCodeForCheckingAction();
				}
			}
			else
			{
				ResetAutoEndCount();
				ShowHandMask(state: true);
				if (!WaitingForActionScreen() && !waitingDeathScreen && !waitingKill)
				{
					getStatusString = GenerateStatusString();
					if (getStatusString != lastStatusString)
					{
						ResetFailCount();
					}
					else
					{
						if (failCount > 0 && failCount % 50 == 0 && Globals.Instance.ShowDebug)
						{
							Debug.LogError("[" + failCount + "] " + cardsWaitingForReset + " && " + eventList.Count + " && " + isBeginTournPhase + " && " + castingCardBlocked.Count + " && " + waitingItemTrait + " && " + generatedCardTimes);
						}
						failCount++;
						if (failCount == 300)
						{
							if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
							{
								photonView.RPC("NET_MasterReloadCombat", RpcTarget.MasterClient, "from update() " + failCount);
							}
							else
							{
								ReloadCombat("from update() " + failCount);
							}
						}
					}
					lastStatusString = getStatusString;
				}
				else
				{
					ResetFailCount();
				}
			}
		}
		else
		{
			ResetAutoEndCount();
		}
		if (autoEndCount >= limitAutoEndCount)
		{
			return;
		}
		if (autoEndCount > 1)
		{
			if (IsYourTurn())
			{
				if (eventList.Count == 0 && cardsWaitingForReset == 0)
				{
					if (!GameManager.Instance.IsMultiplayer())
					{
						if (autoEndCount == limitAutoEndCount - 1)
						{
							if (GameManager.Instance.ConfigAutoEnd && !CanPlayACardRightNow())
							{
								ShowHandMask(state: true);
								botEndTurn.gameObject.SetActive(value: false);
								ShowEnergyCounter(state: false);
								EndTurn();
							}
							else
							{
								autoEndCount = limitAutoEndCount;
							}
						}
						else if (autoEndCount >= limitAutoEndCountStep1)
						{
							if (autoEndCount == limitAutoEndCountStep1)
							{
								ShowHandMask(state: false);
								SetGameBusy(state: false);
								RedrawCardsBorder();
								canKeyboardCast = true;
							}
							if (!botEndTurn.gameObject.activeSelf)
							{
								botEndTurn.gameObject.SetActive(value: true);
								ShowEnergyCounter(state: true);
								botEndTurnAnim.enabled = false;
							}
						}
						else if (autoEndCount < limitAutoEndCountStep1)
						{
							canKeyboardCast = false;
						}
						else
						{
							autoEndCount = limitAutoEndCount;
						}
					}
					else if (!NetworkManager.Instance.IsSyncroClean())
					{
						autoEndCount = 0;
					}
					else if (autoEndCount == limitAutoEndCount - 1)
					{
						if (GameManager.Instance.ConfigAutoEnd && !CanPlayACardRightNow())
						{
							ShowHandMask(state: true);
							botEndTurn.gameObject.SetActive(value: false);
							ShowEnergyCounter(state: false);
							EndTurn();
						}
					}
					else if (autoEndCount >= limitAutoEndCountStep1)
					{
						if (autoEndCount == limitAutoEndCountStep1)
						{
							ShowHandMask(state: false);
							SetGameBusy(state: false);
							RedrawCardsBorder();
							canKeyboardCast = true;
						}
						if (!botEndTurn.gameObject.activeSelf)
						{
							botEndTurn.gameObject.SetActive(value: true);
							ShowEnergyCounter(state: true);
							botEndTurnAnim.enabled = false;
						}
					}
					else if (autoEndCount < limitAutoEndCountStep1)
					{
						canKeyboardCast = false;
					}
					else
					{
						autoEndCount = limitAutoEndCount;
					}
				}
				else
				{
					ShowHandMask(state: true);
				}
			}
			else
			{
				if (botEndTurn.gameObject.activeSelf)
				{
					botEndTurn.gameObject.SetActive(value: false);
					ShowEnergyCounter(state: false);
				}
				RedrawCardsBorder();
				autoEndCount = limitAutoEndCount;
			}
		}
		else if (combatCounter % 10 == 0)
		{
			if (IsYourTurn())
			{
				ShowHandMask(state: true);
			}
			else
			{
				ShowHandMask(state: false);
			}
			if (!deathScreen.IsActive() && !addcardSelector.IsActive() && !deckCardsWindow.IsActive())
			{
				RedrawCardsBorder();
			}
		}
	}

	private void Start()
	{
		StopCoroutines();
		if (GameManager.Instance.IsMultiplayer())
		{
			NetworkManager.Instance.ClearSyncro();
			NetworkManager.Instance.ClearPlayerStatus();
			NetworkManager.Instance.ClearWaitingCalls();
		}
		botEndTurnAnim.enabled = false;
		botEndTurn.gameObject.SetActive(value: false);
		ShowEnergyCounter(state: false);
		combatLoading = true;
		gotHeroDeck = false;
		gotNPCDeck = false;
		gotDictionary = false;
		ShowTraitInfo(state: false, clearText: true);
		AtOManager.Instance.ClearCacheGlobalACModification();
		AtOManager.Instance.ResetCombatScarab();
		InitializeVars();
		StartCoroutine(LoadTurnData());
	}

	private void ClearEventList()
	{
		eventList.Clear();
	}

	private void ResetAutoEndCount()
	{
		autoEndCount = 0;
		combatCounter = 0;
	}

	private void ResetFailCount()
	{
		failCount = 0;
	}

	public void SetWaitingKill(bool _state)
	{
		waitingKill = _state;
	}

	private void StopCoroutines()
	{
		if (!MatchIsOver)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("*.*.*. STOP COROUTINES .*.*.*", "trace");
			}
			StopAllCoroutines();
			ClearItemQueue();
		}
	}

	private string GenerateStatusString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(cardsWaitingForReset);
		stringBuilder.Append("//");
		stringBuilder.Append(eventList.Count);
		stringBuilder.Append("//");
		stringBuilder.Append(isBeginTournPhase);
		stringBuilder.Append("//");
		stringBuilder.Append(castingCardBlocked.Count);
		stringBuilder.Append("//");
		stringBuilder.Append(waitingItemTrait);
		stringBuilder.Append("//");
		stringBuilder.Append(generatedCardTimes);
		stringBuilder.Append("//");
		stringBuilder.Append(gameStatus);
		return stringBuilder.ToString();
	}

	[PunRPC]
	private void NET_SetLoadTurn(string turnData, string turnCombatDictionaryKeys, string turnCombatDictionaryValues, string turnHeroItems, string turnCombatStatsEffects, string turnCombatStatsCurrent, string turnHeroLife, string itemExecutionInCombat, string currentSpecialCardsUsedInMatch, string npcParams, string turnDataOverflowData)
	{
		turnData += turnDataOverflowData;
		if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster() && turnData == "")
		{
			NetworkManager.Instance.SetStatusReady("loadturndatasyncro");
		}
		else
		{
			SetLoadTurn(turnData, turnCombatDictionaryKeys, turnCombatDictionaryValues, turnHeroItems, turnCombatStatsEffects, turnCombatStatsCurrent, turnHeroLife, itemExecutionInCombat, currentSpecialCardsUsedInMatch, npcParams);
		}
	}

	public void RunRpc(string rpcName, int arg0)
	{
		photonView.RPC(rpcName, RpcTarget.Others, arg0);
	}

	public void SetLoadTurn(string turnData, string turnCombatDictionaryKeys, string turnCombatDictionaryValues, string turnHeroItems, string turnCombatStatsEffects, string turnCombatStatsCurrent, string turnHeroLife, string itemExecutionInCombat, string currentSpecialCardsUsedInMatch, string npcParams)
	{
		AtOManager.Instance.combatGameCode = turnData;
		SetCardDictionaryKeysValues(turnCombatDictionaryKeys, turnCombatDictionaryValues);
		SetHeroItemsFromTurnSave(turnHeroItems);
		AtOManager.Instance.InitCombatStatsCurrent();
		SetCombatStatsForTurnSave(turnCombatStatsEffects);
		SetCombatStatsCurrentForTurnSave(turnCombatStatsCurrent);
		SetHeroLifeArrForTurnSave(turnHeroLife);
		SetItemExecutionInCombatForTurnSave(itemExecutionInCombat);
		SetCurrentSpecialCardsUsedInMatchForTurnSave(currentSpecialCardsUsedInMatch);
		SetNPCParamsFromTurnSave(npcParams);
		if (GameManager.Instance.IsMultiplayer())
		{
			if (NetworkManager.Instance.IsMaster())
			{
				turnDataForMP = new string[10];
				turnDataForMP[0] = turnData;
				turnDataForMP[1] = turnCombatDictionaryKeys;
				turnDataForMP[2] = turnCombatDictionaryValues;
				turnDataForMP[3] = turnHeroItems;
				turnDataForMP[4] = turnCombatStatsEffects;
				turnDataForMP[5] = turnCombatStatsCurrent;
				turnDataForMP[6] = turnHeroLife;
				turnDataForMP[7] = itemExecutionInCombat;
				turnDataForMP[8] = currentSpecialCardsUsedInMatch;
				turnDataForMP[9] = npcParams;
			}
			else
			{
				NetworkManager.Instance.SetStatusReady("loadturndatasyncro");
			}
		}
		turnLoadedBySave = true;
	}

	private IEnumerator LoadTurnData()
	{
		if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
		{
			NetworkManager.Instance.SetWaitingSyncro("loadturndatasyncro", status: true);
		}
		if (currentGameCode == "" && (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster()))
		{
			AtOManager.Instance.LoadGameTurn();
			int exhaust = 0;
			while (!turnLoadedBySave && exhaust < 20)
			{
				yield return Globals.Instance.WaitForSeconds(0.01f);
				exhaust++;
			}
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			if (NetworkManager.Instance.IsMaster())
			{
				if (turnLoadedBySave)
				{
					if (turnDataForMP != null && turnDataForMP.Length == 10)
					{
						int num = turnDataForMP[0].Length / 2;
						string text = turnDataForMP[0].Substring(num);
						turnDataForMP[0] = turnDataForMP[0].Substring(0, num);
						photonView.RPC("NET_SetLoadTurn", RpcTarget.Others, turnDataForMP[0], turnDataForMP[1], turnDataForMP[2], turnDataForMP[3], turnDataForMP[4], turnDataForMP[5], turnDataForMP[6], turnDataForMP[7], turnDataForMP[8], turnDataForMP[9], text);
						turnDataForMP[0] += text;
					}
					else
					{
						photonView.RPC("NET_SetLoadTurn", RpcTarget.Others, "", "", "", "", "", "", "", "", "", "", "");
					}
				}
				else
				{
					photonView.RPC("NET_SetLoadTurn", RpcTarget.Others, "", "", "", "", "", "", "", "", "", "", "");
				}
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("**************************", "net");
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("WaitingSyncro loadturndatasyncro", "net");
			}
			yield return Globals.Instance.WaitForSeconds(0.1f);
			if (NetworkManager.Instance.IsMaster())
			{
				while (!NetworkManager.Instance.AllPlayersReady("loadturndatasyncro"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked loadturndatasyncro", "net");
				}
				NetworkManager.Instance.PlayersNetworkContinue("loadturndatasyncro");
			}
			else
			{
				while (NetworkManager.Instance.WaitingSyncro["loadturndatasyncro"])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("loadturndatasyncro, we can continue!", "net");
				}
			}
		}
		if (AtOManager.Instance.corruptionAccepted)
		{
			corruptionCard = Globals.Instance.GetCardData(AtOManager.Instance.corruptionIdCard, instantiate: false);
			iconCorruption.transform.gameObject.SetActive(value: true);
			iconCorruption.ShowIconCorruption(corruptionCard);
			iconCorruption.transform.gameObject.SetActive(value: false);
		}
		ProgressManager.Instance.HideAll();
		if (AtOManager.Instance.currentMapNode == "tutorial_1")
		{
			tutorialCombat = true;
		}
		FinishLoadTurnData();
	}

	private void FinishLoadTurnData()
	{
		StartCoroutine(NewGame());
	}

	public void Resize()
	{
		characterWindow.Resize();
	}

	private void GameObjectFuncs()
	{
		helpCharacterTransform.gameObject.SetActive(value: false);
		cardGos = new Dictionary<string, GameObject>();
		gameObjectsParentFolder = worldTransform;
		foreach (Transform item in goTransform)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		gameObjectsParentFolder = goTransform;
		HideExhaust();
		GO_NewTurn = UnityEngine.Object.Instantiate(newTurnPrefab, Vector3.zero, Quaternion.identity, gameObjectsParentFolder);
		GO_NewTurn.name = "NewTurn";
		newTurnScript = GO_NewTurn.transform.GetComponent<NewTurn>();
		GO_Initiative = new GameObject();
		GO_Initiative.name = "Initiative";
		GO_Initiative.transform.parent = gameObjectsParentFolder;
		GO_Heroes = new GameObject();
		GO_Heroes.name = "Heroes";
		GO_Heroes.transform.parent = gameObjectsParentFolder;
		GO_NPCs = new GameObject();
		GO_NPCs.name = "NPCs";
		GO_NPCs.transform.parent = gameObjectsParentFolder;
		Transform obj = GO_Heroes.transform;
		Vector3 position = (GO_NPCs.transform.position = new Vector3(0f, -0.6f, 5f));
		obj.position = position;
		GO_Hand = new GameObject();
		GO_Hand.name = "Hand";
		GO_Hand.transform.parent = gameObjectsParentFolder;
		GO_Hand.transform.position = new Vector3(0f, handPosY, 0f);
		GO_Hand.transform.localScale = new Vector3(0.95f, 0.95f, 1f);
		HandMask.parent = GO_Hand.transform;
		if (GO_DecksObject != null)
		{
			deckCounter.transform.SetParent(gameObjectsParentFolder, worldPositionStays: true);
			discardCounter.transform.SetParent(gameObjectsParentFolder, worldPositionStays: true);
			foreach (Transform item2 in GO_DecksObject.transform)
			{
				UnityEngine.Object.Destroy(item2.gameObject);
			}
		}
		GO_DecksObject = new GameObject();
		GO_DecksObject.name = "Decks";
		GO_DecksObject.transform.parent = gameObjectsParentFolder;
		GO_DecksObject.transform.localScale = new Vector3(0.95f, 0.95f, 1f);
		GameObject gameObject = UnityEngine.Object.Instantiate(energySelectorPrefab, Vector3.zero, Quaternion.identity, gameObjectsParentFolder);
		gameObject.name = "GO_EnergySelector";
		energySelector = gameObject.GetComponent<UIEnergySelector>();
		GameObject gameObject2 = UnityEngine.Object.Instantiate(discardSelectorPrefab, Vector3.zero, Quaternion.identity, gameObjectsParentFolder);
		gameObject2.name = "GO_DiscardSelector";
		discardSelector = gameObject2.GetComponent<UIDiscardSelector>();
		GameObject gameObject3 = UnityEngine.Object.Instantiate(addcardSelectorPrefab, Vector3.zero, Quaternion.identity, gameObjectsParentFolder);
		gameObject3.name = "GO_AddcardSelector";
		addcardSelector = gameObject3.GetComponent<UIAddcardSelector>();
		GameObject gameObject4 = UnityEngine.Object.Instantiate(deckCardsPrefab, Vector3.zero, Quaternion.identity, gameObjectsParentFolder);
		gameObject4.name = "GO_DeckCards";
		deckCardsWindow = gameObject4.GetComponent<UIDeckCards>();
		GameObject gameObject5 = UnityEngine.Object.Instantiate(deathScreenPrefab, Vector3.zero, Quaternion.identity, gameObjectsParentFolder);
		deathScreen = gameObject5.GetComponent<UICombatDeath>();
		GO_DiscardPile = new GameObject();
		GO_DiscardPile.name = "DiscardPile";
		GO_DiscardPile.transform.parent = GO_DecksObject.transform;
		GO_DeckPile = new GameObject();
		GO_DeckPile.name = "DeckPile";
		GO_DeckPile.transform.parent = GO_DecksObject.transform;
		GO_DeckPileCards = new GameObject[4];
		GO_DeckPileCardsT = new Transform[GO_DeckPileCards.Length];
		for (int i = 0; i < GO_DeckPileCards.Length; i++)
		{
			GameObject gameObject6 = UnityEngine.Object.Instantiate(deckPileCardPrefab, Vector3.zero, Quaternion.identity, GO_DeckPile.transform);
			gameObject6.GetComponent<BoxCollider2D>().enabled = false;
			GO_DeckPileCardsT[i] = gameObject6.transform;
			if (i > 0)
			{
				gameObject6.transform.localPosition = new Vector3(0.04f * (float)(i - 1), 0.04f * (float)(i - 1), 0f);
				GO_DeckPileCards[i - 1] = gameObject6;
			}
			else
			{
				gameObject6.transform.localPosition = Vector3.zero;
				gameObject6.transform.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.3f);
			}
			gameObject6.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
			gameObject6.GetComponent<Renderer>().sortingOrder = -10000 + i;
		}
		GameObject gameObject7 = UnityEngine.Object.Instantiate(deckPileParticlePrefab, Vector3.zero, Quaternion.identity, GO_DeckPile.transform);
		deckPileParticle = gameObject7.GetComponent<ParticleSystem>();
		DeckPilePosition = new Vector3(-8.5f, -4f, 0f);
		GO_DecksObject.transform.localPosition = DeckPilePosition;
		deckCounter.transform.localPosition = DeckPilePosition + new Vector3(-0.6f, -0.3f, 0f);
		discardCounter.transform.localPosition = DeckPilePosition + new Vector3(2.4f, -0.3f, 0f);
		deckCounter.transform.SetParent(GO_DecksObject.transform, worldPositionStays: true);
		discardCounter.transform.SetParent(GO_DecksObject.transform, worldPositionStays: true);
		DiscardPilePosition = DeckPilePosition + new Vector3(1.8f, 0f, 0f);
		GameObject gameObject8 = UnityEngine.Object.Instantiate(deckPileCardPrefab, Vector3.zero, Quaternion.identity, gameObjectsParentFolder);
		gameObject8.name = "discardpile";
		gameObject8.transform.localScale = new Vector3(0.8f, 0.9f, 1f);
		gameObject8.transform.localPosition = new Vector3(DiscardPilePosition.x, DiscardPilePosition.y, -1f);
		gameObject8.transform.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
		gameObject7 = UnityEngine.Object.Instantiate(deckPileParticlePrefab, gameObject8.transform.localPosition, Quaternion.identity, GO_DeckPile.transform);
		discardPileParticle = gameObject7.GetComponent<ParticleSystem>();
		discardPileParticle.GetComponent<Renderer>().sortingLayerName = "Discards";
		GO_DeckPile.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
		GO_DiscardPile.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
		GO_DecksObject.transform.localPosition = DeckPilePosition - DeckPileOutOfScreenPositionVector;
	}

	private void InitializeVars()
	{
		if (TomeManager.Instance.IsActive())
		{
			TomeManager.Instance.ShowTome(_status: false);
		}
		if (CardScreenManager.Instance.IsActive())
		{
			CardScreenManager.Instance.ShowCardScreen(_state: false);
		}
		if (console.IsActive())
		{
			console.Show(_state: false);
		}
		if (DamageMeterManager.Instance.IsActive())
		{
			DamageMeterManager.Instance.Hide();
		}
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("Initialize Variables");
		}
		reloadingGame = false;
		heroIndexWaitingForAddDiscard = -1;
		HeroDeck = new List<string>[4];
		HeroDeckDiscard = new List<string>[4];
		HeroDeckVanish = new List<string>[4];
		HeroHand = new List<string>[4];
		NPCDeck = new List<string>[4];
		NPCDeckDiscard = new List<string>[4];
		NPCHand = new List<string>[4];
		cardDictionary = new Dictionary<string, CardData>();
		castedCards = new List<string>();
		castedCards.Add("");
		CICardDiscard = new List<CardItem>();
		CICardAddcard = new List<CardItem>();
		npcCardsCasted = new Dictionary<string, List<string>>();
		canInstaCastDict = new Dictionary<string, bool>();
		if (!turnLoadedBySave)
		{
			AtOManager.Instance.InitCombatStatsCurrent();
		}
		for (int i = 0; i < 4; i++)
		{
			HeroDeck[i] = new List<string>();
		}
		for (int j = 0; j < 4; j++)
		{
			NPCDeck[j] = new List<string>();
		}
		itemTimeout = new float[10];
		for (int k = 0; k < itemTimeout.Length; k++)
		{
			itemTimeout[k] = 0f;
		}
	}

	private IEnumerator NewGame()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[NewGame] AtOManager.Instance.combatGameCode -> " + AtOManager.Instance.combatGameCode, "synccode");
		}
		if (AtOManager.Instance.combatGameCode == "")
		{
			combatStatsAux = new int[AtOManager.Instance.combatStats.GetLength(0), AtOManager.Instance.combatStats.GetLength(1)];
			combatStatsCurrentAux = new int[AtOManager.Instance.combatStatsCurrent.GetLength(0), AtOManager.Instance.combatStatsCurrent.GetLength(1)];
			combatStatsDict = new Dictionary<string, Dictionary<string, List<string>>>();
			logDictionary = new Dictionary<string, LogEntry>();
			logDictionaryAux = new Dictionary<string, LogEntry>();
			StoreCombatStats();
		}
		else if (combatStatsDict == null)
		{
			combatStatsAux = new int[AtOManager.Instance.combatStats.GetLength(0), AtOManager.Instance.combatStats.GetLength(1)];
			combatStatsCurrentAux = new int[AtOManager.Instance.combatStatsCurrent.GetLength(0), AtOManager.Instance.combatStatsCurrent.GetLength(1)];
			combatStatsDict = new Dictionary<string, Dictionary<string, List<string>>>();
			StoreCombatStats();
		}
		logDictionary = new Dictionary<string, LogEntry>();
		foreach (KeyValuePair<string, LogEntry> item in logDictionaryAux)
		{
			logDictionary.Add(item.Key, item.Value);
		}
		logDictionaryAux = new Dictionary<string, LogEntry>();
		GameObjectFuncs();
		HandMask.GetComponent<BoxCollider2D>().enabled = true;
		watchingTutorialText.gameObject.SetActive(value: false);
		GetCombatData();
		GenerateRandomStringBatch();
		if (GameManager.Instance.IsMultiplayer())
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("**************************", "net");
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("WaitingSyncro newgame", "net");
			}
			yield return Globals.Instance.WaitForSeconds(0.1f);
			if (NetworkManager.Instance.IsMaster())
			{
				GenerateSyncCodeDict();
				while (!NetworkManager.Instance.AllPlayersReady("newgame"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked newgame", "net");
				}
				NetworkManager.Instance.PlayersNetworkContinue("newgame");
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("newgame", status: true);
				NetworkManager.Instance.SetStatusReady("newgame");
				while (NetworkManager.Instance.WaitingSyncro["newgame"])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("newgame, we can continue!", "net");
				}
			}
		}
		if (!GameManager.Instance.IsMultiplayer() || (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster()))
		{
			DoBackground();
			GetPlayerTeam();
			GetNPCTeam();
			SetRandomIndex(UnityEngine.Random.Range(200, 500));
			if (GameManager.Instance.IsMultiplayer())
			{
				string text = "";
				if (combatData != null)
				{
					text = combatData.CombatId;
				}
				photonView.RPC("NET_SetTeams", RpcTarget.Others, text, randomIndex);
			}
			ContinueNewGame();
		}
		yield return null;
	}

	private void GetCombatData()
	{
		AtOManager.Instance.CinematicId = "";
		combatData = AtOManager.Instance.GetCurrentCombatData();
		if (combatData == null)
		{
			combatData = AtOManager.Instance.fromEventCombatData;
			AtOManager.Instance.fromEventCombatData = null;
		}
		else if (combatData.CinematicData != null)
		{
			AtOManager.Instance.CinematicId = combatData.CinematicData.CinematicId;
		}
	}

	private void ContinueNewGame()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("ContinueNewGame");
		}
		GenerateHeroes();
		GenerateNPCs();
		RepositionCharacters();
		SortCharacterSprites();
		StartCoroutine(ContinueNewGameCo());
	}

	private IEnumerator ContinueNewGameCo()
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("**************************", "net");
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("WaitingSyncro ContinueNewGameCo", "net");
			}
			if (NetworkManager.Instance.IsMaster())
			{
				while (!NetworkManager.Instance.AllPlayersReady("ContinueNewGameCo"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked ContinueNewGameCo", "net");
				}
				NetworkManager.Instance.PlayersNetworkContinue("ContinueNewGameCo");
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("ContinueNewGameCo", status: true);
				NetworkManager.Instance.SetStatusReady("ContinueNewGameCo");
				while (NetworkManager.Instance.WaitingSyncro["ContinueNewGameCo"])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("ContinueNewGameCo, we can continue!", "net");
				}
			}
		}
		if (AtOManager.Instance.combatGameCode != "")
		{
			CheckForCombatCode();
		}
		else if (!GameManager.Instance.IsMultiplayer() || (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster()))
		{
			GenerateDecks();
			GenerateDecksNPCs();
			NET_ShareDecks(ContinueNewGame: true);
		}
	}

	public void DoSahtiRustBackground(bool showPittAlive, bool justKilled = false)
	{
		if (backgroundSahtiAlive == null)
		{
			backgroundSahtiAlive = backgroundPrefab.transform.Find("Alive");
		}
		if (backgroundSahtiDead == null)
		{
			backgroundSahtiDead = backgroundPrefab.transform.Find("Dead");
		}
		if (showPittAlive)
		{
			backgroundSahtiAlive.gameObject.SetActive(value: true);
			backgroundSahtiDead.gameObject.SetActive(value: false);
			return;
		}
		backgroundSahtiAlive.gameObject.SetActive(value: false);
		backgroundSahtiDead.gameObject.SetActive(value: true);
		if (justKilled)
		{
			StartCoroutine(DoSahtiRustBackgroundJustKilled(backgroundSahtiDead));
		}
	}

	private IEnumerator DoSahtiRustBackgroundJustKilled(Transform deadT)
	{
		SpriteRenderer pittSR = deadT.Find("cavern_deadpitt").GetComponent<SpriteRenderer>();
		pittSR.color = new Color(1f, 1f, 1f, 0f);
		float index = 0f;
		Color hideColor = new Color(1f, 1f, 1f, 0f);
		while (index < 1f)
		{
			yield return Globals.Instance.WaitForSeconds(0.02f);
			index = (hideColor.a = index + 0.1f);
			pittSR.color = hideColor;
		}
	}

	private void DoBackground()
	{
		GameManager.Instance.PlayAudio(AudioManager.Instance.soundCombatBegins);
		AudioClip audioClip = null;
		if (combatData != null)
		{
			backgroundActive = Enum.GetName(typeof(Enums.CombatBackground), combatData.CombatBackground);
			audioClip = combatData.CombatMusic;
		}
		else
		{
			backgroundActive = "Senenthia_Dia";
		}
		for (int i = 0; i < backgroundPrefabs.Count; i++)
		{
			if (!(backgroundPrefabs[i] != null) || !(backgroundActive != "") || !(backgroundPrefabs[i].name.ToLower() == backgroundActive.ToLower()))
			{
				continue;
			}
			bool flag = false;
			for (int j = 0; j < backgroundTransform.childCount; j++)
			{
				if (backgroundTransform.GetChild(j).gameObject.name == backgroundPrefabs[i].name)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				continue;
			}
			backgroundPrefab = UnityEngine.Object.Instantiate(backgroundPrefabs[i], new Vector3(0f, 0f, 0f), Quaternion.identity, backgroundTransform);
			backgroundPrefab.name = backgroundPrefabs[i].name;
			backgroundPrefab.transform.localScale = new Vector3(0.545f, 0.545f, 1f);
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			if (GameManager.Instance.IsWeeklyChallenge())
			{
				ChallengeData weeklyData = Globals.Instance.GetWeeklyData(AtOManager.Instance.GetWeekly());
				if (weeklyData?.IdSteam == "challengeSpooky")
				{
					flag2 = true;
				}
				else if (weeklyData?.IdSteam == "challengeChristmas")
				{
					flag3 = true;
				}
				else if (weeklyData?.IdSteam == "challengeLunar")
				{
					flag4 = true;
				}
			}
			Transform transform = backgroundPrefab.transform.Find("halloween");
			if (transform != null)
			{
				if (flag2 || flag3)
				{
					if (!transform.gameObject.activeSelf)
					{
						transform.gameObject.SetActive(value: true);
					}
					if (flag3)
					{
						Transform transform2 = null;
						int childCount = transform.childCount;
						for (int k = 0; k < childCount; k++)
						{
							Transform child = transform.GetChild(k);
							if (!child.gameObject.activeSelf)
							{
								continue;
							}
							if ((bool)child.GetComponent<SpriteRenderer>())
							{
								transform2 = child;
							}
							else if (child.GetChild(0) != null && (bool)child.GetChild(0).GetComponent<SpriteRenderer>())
							{
								transform2 = child.GetChild(0);
							}
							if (transform2 != null && transform2.GetComponent<SpriteRenderer>() != null && transform2.GetComponent<SpriteRenderer>().sprite != null)
							{
								string text = "";
								if (flag3)
								{
									text = transform2.GetComponent<SpriteRenderer>().sprite.name.Replace("halloween", "christmas").Replace("_front", "");
								}
								else if (flag4)
								{
									text = transform2.GetComponent<SpriteRenderer>().sprite.name.Replace("halloween", "lunar").Replace("_front", "");
								}
								if (text != "")
								{
									for (int l = 0; l < decorationsT.Count; l++)
									{
										if (decorationsT[l].name == text)
										{
											GameObject gameObject = UnityEngine.Object.Instantiate(decorationsT[l], child.transform.position, Quaternion.identity, transform);
											gameObject.transform.localScale = child.transform.localScale;
											if ((bool)gameObject.GetComponent<SpriteRenderer>())
											{
												gameObject.GetComponent<SpriteRenderer>().sortingOrder = transform2.GetComponent<SpriteRenderer>().sortingOrder;
												gameObject.GetComponent<SpriteRenderer>().sortingLayerName = transform2.GetComponent<SpriteRenderer>().sortingLayerName;
											}
											break;
										}
									}
								}
							}
							child.gameObject.SetActive(value: false);
						}
					}
				}
				else if (transform.gameObject.activeSelf)
				{
					transform.gameObject.SetActive(value: false);
				}
			}
			Transform transform3 = backgroundPrefab.transform.Find("Lunar");
			if (!(transform3 != null))
			{
				continue;
			}
			if (flag4)
			{
				if (!transform3.gameObject.activeSelf)
				{
					transform3.gameObject.SetActive(value: true);
				}
			}
			else if (transform3.gameObject.activeSelf)
			{
				transform3.gameObject.SetActive(value: false);
			}
		}
		if (combatData != null && combatData.IsRift)
		{
			UnityEngine.Object.Instantiate(riftPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, backgroundTransform);
		}
		if (audioClip != null)
		{
			AudioManager.Instance.DoBSOAudioClip(audioClip);
		}
		if (backgroundActive == "Senenthia_Dia")
		{
			AudioManager.Instance.DoAmbience("forest_ambience_day");
		}
		else if (backgroundActive == "Senenthia_Tarde" || backgroundActive == "Senenthia_LobosTarde" || backgroundActive == "Senenthia_Bosque" || backgroundActive == "Faeborg_Bosque" || backgroundActive == "Faeborg_BosqueCastor")
		{
			AudioManager.Instance.DoAmbience("forest_ambience_sunset");
		}
		else if (backgroundActive == "Senenthia_LobosNoche" || backgroundActive == "Senenthia_BosqueEntrada" || backgroundActive == "Senenthia_BosqueBoss")
		{
			AudioManager.Instance.DoAmbience("forest_ambience_night");
		}
		else if (backgroundActive == "Sectarium" || backgroundActive == "Spider_Lair" || backgroundActive == "Sewers")
		{
			AudioManager.Instance.DoAmbience("cave_ambience");
		}
	}

	[PunRPC]
	private void NET_SetTeams(string _combatDataId, int _randomIndex)
	{
		if (_combatDataId != "")
		{
			combatData = Globals.Instance.GetCombatData(_combatDataId);
			DoBackground();
		}
		GetPlayerTeam();
		GetNPCTeam();
		SetRandomIndex(_randomIndex);
		ContinueNewGame();
	}

	public bool IsYourTurnForAddDiscard()
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			if (heroIndexWaitingForAddDiscard > -1 && TeamHero[heroIndexWaitingForAddDiscard].Owner == NetworkManager.Instance.GetPlayerNick())
			{
				return true;
			}
			return false;
		}
		return true;
	}

	public bool IsYourTurn()
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			if (heroActive > -1 && TeamHero[heroActive].Owner == NetworkManager.Instance.GetPlayerNick())
			{
				return true;
			}
			return false;
		}
		return true;
	}

	public void SetOwnerForTeamHero(int index, string owner)
	{
		if (TeamHero[index] != null)
		{
			TeamHero[index].Owner = owner;
		}
	}

	private void GetPlayerTeam()
	{
		List<Hero> list = new List<Hero>();
		TeamHero = AtOManager.Instance.GetTeam();
		for (int i = 0; i < TeamHero.Length; i++)
		{
			if (TeamHero[i] != null && !(TeamHero[i].HeroData == null) && TeamHero[i].Id != null)
			{
				list.Add(TeamHero[i]);
			}
		}
		TeamHero = new Hero[list.Count];
		for (int j = 0; j < list.Count; j++)
		{
			TeamHero[j] = list[j];
		}
	}

	private void GetNPCTeam()
	{
		string[] teamNPC = AtOManager.Instance.GetTeamNPC();
		_ = new NPC[4];
		TeamNPC = new NPC[4];
		for (int i = 0; i < teamNPC.Length; i++)
		{
			TeamNPC[i] = null;
			if (teamNPC[i] != null && teamNPC[i] != "" && (!(combatData != null) || ((!GameManager.Instance.IsGameAdventure() || AtOManager.Instance.GetMadnessDifficulty() != 0) && (!GameManager.Instance.IsSingularity() || AtOManager.Instance.GetSingularityMadness() != 0)) || combatData.NpcRemoveInMadness0Index != i || AtOManager.Instance.GetActNumberForText() >= 3))
			{
				NPC nPC = new NPC();
				nPC.NpcData = Globals.Instance.GetNPC(teamNPC[i]);
				nPC.InitData();
				nPC.Position = i;
				TeamNPC[i] = nPC;
			}
		}
		if (GameManager.Instance.IsMultiplayer() || !(combatData != null) || !combatData.RandomizeNpcPosition)
		{
			return;
		}
		System.Random random = new System.Random();
		for (int num = TeamNPC.Length - 1; num > 0; num--)
		{
			int num2 = random.Next(num + 1);
			num2 %= TeamNPC.Length - 1;
			NPC nPC2 = TeamNPC[num2];
			TeamNPC[num2] = TeamNPC[num];
			if (TeamNPC[num] != null)
			{
				TeamNPC[num2].Position = TeamNPC[num].Position;
				TeamNPC[num2].NPCIndex = TeamNPC[num].NPCIndex;
			}
			TeamNPC[num] = nPC2;
			if (nPC2 != null)
			{
				TeamNPC[num].Position = nPC2.Position;
				TeamNPC[num].NPCIndex = nPC2.NPCIndex;
			}
		}
	}

	private void UpdateBossNpc()
	{
		if (BossNpc != null)
		{
			return;
		}
		NPC[] teamNPC = TeamNPC;
		foreach (NPC nPC in teamNPC)
		{
			if (nPC != null && nPC.NPCIsBoss())
			{
				if (nPC.NpcData.Id.StartsWith("franky"))
				{
					BossNpc = new Frankenstein(nPC);
				}
				else if (nPC.NpcData.Id.StartsWith("s_queen"))
				{
					BossNpc = new SirenQueen(nPC);
				}
				else if (nPC.NpcData.Id.StartsWith("count"))
				{
					BossNpc = new Dracula(nPC);
				}
				else if (nPC.NpcData.Id.StartsWith("pa_d"))
				{
					BossNpc = new PhantomArmor(nPC);
				}
			}
			if (BossNpc != null && BossNpc is Dracula && nPC.Id.StartsWith("count"))
			{
				((Dracula)BossNpc).DoDraculaTransformation(nPC, instantTransition: true);
			}
		}
		if (combatData != null && BossNpc == null)
		{
			if (combatData.NpcToSummonOnNpcKilled != null && combatData.NpcToSummonOnNpcKilled.Id.StartsWith("s_queen"))
			{
				if (combatData.NpcToSummonOnNpcKilled != null)
				{
					BossNpc = new SirenQueen(null, combatData.NpcToSummonOnNpcKilled.Id);
				}
				else
				{
					BossNpc = new SirenQueen();
				}
			}
		}
		else if (AtOManager.Instance.SirenQueenBattle)
		{
			BossNpc = new SirenQueen();
		}
		if (!(BossNpc is SirenQueen))
		{
			return;
		}
		teamNPC = GetTeamNPC();
		foreach (NPC nPC2 in teamNPC)
		{
			if (nPC2 != null && nPC2.Alive && nPC2.Id.StartsWith("s_queen"))
			{
				((SirenQueen)BossNpc).hasAppeared = true;
			}
		}
	}

	private void OnDestroy()
	{
		if (BossNpc != null)
		{
			BossNpc.Dispose();
		}
	}

	private void GenerateHeroes()
	{
		int num = 0;
		Hero[] array = new Hero[4];
		for (int i = 0; i < TeamHero.Length; i++)
		{
			if (TeamHero[i] == null || (tutorialCombat && (i == 1 || i == 2)))
			{
				continue;
			}
			Hero hero = TeamHero[i];
			if (AtOManager.Instance.combatGameCode == "" || teamHeroItemsFromTurnSave != null)
			{
				heroLifeArr[i] = hero.HpCurrent;
				List<string> list = new List<string>();
				list.Add(hero.Weapon);
				list.Add(hero.Armor);
				list.Add(hero.Jewelry);
				list.Add(hero.Accesory);
				list.Add(hero.Pet);
				if (!heroBeginItems.ContainsKey(i))
				{
					heroBeginItems.Add(i, list);
				}
				else
				{
					heroBeginItems[i] = list;
				}
			}
			if (AtOManager.Instance.combatGameCode != "")
			{
				if (teamHeroItemsFromTurnSave != null)
				{
					if (teamHeroItemsFromTurnSave.Count > i * 5 + 4)
					{
						hero.Weapon = teamHeroItemsFromTurnSave[i * 5];
						hero.Armor = teamHeroItemsFromTurnSave[i * 5 + 1];
						hero.Jewelry = teamHeroItemsFromTurnSave[i * 5 + 2];
						hero.Accesory = teamHeroItemsFromTurnSave[i * 5 + 3];
						hero.Pet = teamHeroItemsFromTurnSave[i * 5 + 4];
					}
				}
				else if (currentRound == 0 && heroBeginItems != null && heroBeginItems.ContainsKey(i) && heroBeginItems[i] != null)
				{
					List<string> list2 = heroBeginItems[i];
					hero.Weapon = list2[0];
					hero.Armor = list2[1];
					hero.Jewelry = list2[2];
					hero.Accesory = list2[3];
					hero.Pet = list2[4];
				}
				else if (currentRound > 0 && heroDestroyedItemsInThisTurn.ContainsKey(i))
				{
					if (heroDestroyedItemsInThisTurn[i].ContainsKey("weapon"))
					{
						hero.Weapon = heroDestroyedItemsInThisTurn[i]["weapon"];
					}
					if (heroDestroyedItemsInThisTurn[i].ContainsKey("armor"))
					{
						hero.Armor = heroDestroyedItemsInThisTurn[i]["armor"];
					}
					if (heroDestroyedItemsInThisTurn[i].ContainsKey("jewelry"))
					{
						hero.Jewelry = heroDestroyedItemsInThisTurn[i]["jewelry"];
					}
					if (heroDestroyedItemsInThisTurn[i].ContainsKey("accesory"))
					{
						hero.Accesory = heroDestroyedItemsInThisTurn[i]["accesory"];
					}
					if (heroDestroyedItemsInThisTurn[i].ContainsKey("pet"))
					{
						hero.Pet = heroDestroyedItemsInThisTurn[i]["pet"];
					}
				}
			}
			if (hero.HpCurrent <= 0)
			{
				hero.HpCurrent = 1;
			}
			hero.Alive = true;
			hero.InternalId = GetRandomString();
			hero.Id = hero.HeroData.HeroSubClass.Id + "_" + hero.InternalId;
			hero.Position = num;
			GameObject gameObject = UnityEngine.Object.Instantiate(heroPrefab, Vector3.zero, Quaternion.identity, GO_Heroes.transform);
			gameObject.name = hero.Id;
			targetTransformDict.Add(hero.Id, gameObject.transform);
			hero.ResetDataForNewCombat(currentGameCode == "");
			hero.SetHeroIndex(i);
			hero.HeroItem = gameObject.GetComponent<HeroItem>();
			hero.HeroItem.HeroData = hero.HeroData;
			hero.HeroItem.Init(hero);
			hero.HeroItem.SetPosition(instant: true);
			if (AtOManager.Instance.CharacterHavePerk(hero.SubclassName, "mainperkmark1a") && !hero.AuracurseImmune.Contains("mark"))
			{
				hero.AuracurseImmune.Add("mark");
			}
			if (AtOManager.Instance.CharacterHavePerk(hero.SubclassName, "mainperkinspire0c") && !hero.AuracurseImmune.Contains("stress"))
			{
				hero.AuracurseImmune.Add("stress");
			}
			HeroHand[i] = new List<string>();
			HeroDeckDiscard[i] = new List<string>();
			HeroDeckVanish[i] = new List<string>();
			array[i] = hero;
			num++;
			CardData pet = hero.GetPet();
			if (pet != null)
			{
				CreatePet(pet, gameObject, hero, null);
			}
		}
		TeamHero = new Hero[4];
		for (int j = 0; j < 4; j++)
		{
			TeamHero[j] = array[j];
		}
		teamHeroItemsFromTurnSave = null;
	}

	public void RemovePetEnchantment(GameObject charGO, string _enchantmentName = "")
	{
		string n = "thePetEnchantment" + _enchantmentName;
		Transform transform = charGO.transform.Find(n);
		if (transform != null)
		{
			UnityEngine.Object.Destroy(transform.gameObject);
		}
	}

	public void CreatePet(CardData cardPet, GameObject charGO, Hero _hero, NPC _npc, bool _fromEnchant = false, string _enchantName = "")
	{
		string n = "thePet";
		if (_fromEnchant)
		{
			n = "thePetEnchantment" + _enchantName;
		}
		Transform transform = charGO.transform.Find(n);
		if (transform != null)
		{
			if (_fromEnchant)
			{
				return;
			}
			UnityEngine.Object.Destroy(transform.gameObject);
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(cardPet.PetModel, Vector3.zero, Quaternion.identity, charGO.transform);
		gameObject.name = n;
		if (cardPet.PetModel != null && cardPet.PetModel.name.ToLower() == "scrappy")
		{
			ScrappyRobotResolver.ShowHideRobotLayers(gameObject.transform, cardPet.Id);
		}
		gameObject.GetComponent<BoxCollider2D>().enabled = false;
		if (_hero != null && !_fromEnchant)
		{
			_hero.HeroItem.animPet = gameObject.GetComponent<Animator>();
		}
		else if (_npc != null)
		{
			_npc.NPCItem.animPet = gameObject.GetComponent<Animator>();
		}
		gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * cardPet.PetSize.x, gameObject.transform.localScale.y * cardPet.PetSize.y, gameObject.transform.localScale.z);
		if (cardPet.PetInvert && _hero != null)
		{
			gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * -1f, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
		}
		gameObject.transform.localPosition = cardPet.PetOffset;
		NPCItem nPCItem = gameObject.AddComponent<NPCItem>();
		nPCItem.SetOriginalLocalPosition(gameObject.transform.localPosition);
		nPCItem.GetSpritesFromAnimated(gameObject);
		if (!cardPet.PetFront)
		{
			nPCItem.DeleteShadow(gameObject);
		}
		if (_hero != null)
		{
			if (!_fromEnchant)
			{
				_hero.HeroItem.PetItem = nPCItem;
				_hero.HeroItem.PetItemFront = cardPet.PetFront;
			}
			else
			{
				_hero.HeroItem.PetItemEnchantment = nPCItem;
				_hero.HeroItem.PetItemEnchantmentFront = cardPet.PetFront;
			}
			_hero.HeroItem.DrawOrderSprites(goToFront: true, _hero.Position * 2);
		}
		else if (_npc != null)
		{
			if (!_fromEnchant)
			{
				_npc.NPCItem.PetItem = nPCItem;
				_npc.NPCItem.PetItemFront = cardPet.PetFront;
			}
			else
			{
				_npc.NPCItem.PetItemEnchantment = nPCItem;
				_npc.NPCItem.PetItemEnchantmentFront = cardPet.PetFront;
			}
			_npc.NPCItem.DrawOrderSprites(goToFront: true, _npc.Position * 2);
		}
	}

	private void GenerateNPCs()
	{
		int num = 0;
		for (int i = 0; i < TeamNPC.Length; i++)
		{
			NPC nPC;
			if (TeamNPC[i] == null || TeamNPC[i].NpcData == null)
			{
				nPC = new NPC();
				nPC.NpcData = Globals.Instance.GetNPC("sheep");
				nPC.Alive = false;
				nPC.HpCurrent = 0;
			}
			else
			{
				nPC = TeamNPC[i];
				nPC.NpcData = Globals.Instance.GetNPC(nPC.GameName);
				if (nPC.NpcData != null && AtOManager.Instance.PlayerHasRequirement(Globals.Instance.GetRequirementData("_tier2")) && nPC.NpcData.UpgradedMob != null)
				{
					nPC.NpcData = nPC.NpcData.UpgradedMob;
				}
				if (nPC.NpcData != null && (AtOManager.Instance.GetNgPlus() > 0 || (!nPC.NpcData.IsNamed && !nPC.NpcData.IsBoss && AtOManager.Instance.IsChallengeTraitActive("toughermonsters")) || (nPC.NpcData.IsNamed && !nPC.NpcData.IsBoss && AtOManager.Instance.IsChallengeTraitActive("toughermonsters")) || (nPC.NpcData.IsBoss && AtOManager.Instance.IsChallengeTraitActive("hardcorebosses"))) && nPC.NpcData.NgPlusMob != null)
				{
					nPC.NpcData = nPC.NpcData.NgPlusMob;
				}
				if ((MadnessManager.Instance.IsMadnessTraitActive("despair") || AtOManager.Instance.IsChallengeTraitActive("despair")) && nPC.NpcData.HellModeMob != null)
				{
					nPC.NpcData = nPC.NpcData.HellModeMob;
				}
				num++;
			}
			nPC.SetNPCIndex(i);
			nPC.Position = i;
			if (nPC.NpcData != null && nPC.Alive)
			{
				nPC.InternalId = GetRandomString();
				nPC.InitData();
				GameObject gameObject = UnityEngine.Object.Instantiate(npcPrefab, Vector3.zero, Quaternion.identity, GO_NPCs.transform);
				gameObject.name = nPC.Id;
				targetTransformDict.Add(gameObject.name, gameObject.transform);
				nPC.GO = gameObject;
				nPC.NPCItem = gameObject.GetComponent<NPCItem>();
				nPC.NPCItem.NpcData = nPC.NpcData;
				nPC.NPCItem.Init(nPC);
				nPC.NPCItem.SetPosition(instant: true);
				NPCHand[i] = new List<string>();
				NPCDeckDiscard[i] = new List<string>();
			}
		}
		if (currentRound == 0 && AtOManager.Instance.Sandbox_lessNPCs != 0)
		{
			SortedDictionary<int, int> sortedDictionary = new SortedDictionary<int, int>();
			for (int j = 0; j < TeamNPC.Length; j++)
			{
				if (TeamNPC[j] != null && TeamNPC[j].NpcData != null && TeamNPC[j].Alive && !TeamNPC[j].NpcData.IsNamed && !TeamNPC[j].NpcData.IsBoss)
				{
					sortedDictionary.Add(TeamNPC[j].GetHp() * 10000 + j, j);
				}
			}
			int num2 = AtOManager.Instance.Sandbox_lessNPCs;
			if (num2 >= num)
			{
				num2 = num - 1;
			}
			if (num2 > sortedDictionary.Count)
			{
				num2 = sortedDictionary.Count;
			}
			for (int k = 0; k < num2; k++)
			{
				NPC nPC2 = TeamNPC[sortedDictionary.ElementAt(k).Value];
				if (nPC2 != null)
				{
					if (CanDestroyIllusion(nPC2))
					{
						DestroyIllusion(nPC2);
					}
					nPC2.DestroyCharacter();
					nPC2.Alive = false;
					nPC2.HpCurrent = 0;
					nPC2.NpcData = null;
					TeamNPC[sortedDictionary.ElementAt(k).Value] = null;
				}
			}
		}
		if (!(backgroundActive == "Sahti_Rust"))
		{
			return;
		}
		bool flag = false;
		for (int l = 0; l < TeamNPC.Length; l++)
		{
			if (TeamNPC[l] != null && TeamNPC[l].Id.StartsWith("pitt") && TeamNPC[l].Alive)
			{
				flag = true;
				DoSahtiRustBackground(showPittAlive: true);
				break;
			}
		}
		if (!flag)
		{
			DoSahtiRustBackground(showPittAlive: false);
		}
	}

	public int GetNPCAvailablePosition(int slotsNeeded = 1)
	{
		if (slotsNeeded < 1 || slotsNeeded > TeamNPC.Length)
		{
			return -1;
		}
		for (int i = 0; i < TeamNPC.Length && i + slotsNeeded <= TeamNPC.Length; i++)
		{
			if ((TeamNPC[i] != null && TeamNPC[i].Alive) || (i > 0 && TeamNPC[i - 1] != null && TeamNPC[i - 1].NpcData != null && TeamNPC[i - 1].Alive && TeamNPC[i - 1].NpcData.BigModel))
			{
				continue;
			}
			bool flag = true;
			for (int j = 0; j < slotsNeeded; j++)
			{
				if (TeamNPC[i + j] != null && TeamNPC[i + j].Alive)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return i;
			}
		}
		return -1;
	}

	public void RemoveFromTransformDict(string _key)
	{
		if (targetTransformDict != null && targetTransformDict.ContainsKey(_key))
		{
			targetTransformDict.Remove(_key);
		}
	}

	private Character CreateIllusion(Character originalCharacter, string effectTarget, CardData card, int createdCharacterPositionInTeam)
	{
		bool flag = originalCharacter is NPC;
		if (!flag)
		{
			throw new InvalidOperationException("Illusion for Hero is not supported");
		}
		CharacterItem characterItem = (flag ? ((CharacterItem)originalCharacter.NPCItem) : ((CharacterItem)originalCharacter.HeroItem));
		List<Aura> auraList = originalCharacter.AuraList;
		GameObject gameObject = UnityEngine.Object.Instantiate(npcPrefab, Vector3.zero, Quaternion.identity, GO_NPCs.transform);
		Character character = CreateCharacterWithUniqueId(originalCharacter, flag, createdCharacterPositionInTeam);
		gameObject.name = character.Id;
		if (targetTransformDict != null)
		{
			targetTransformDict[gameObject.name] = gameObject.transform;
		}
		character.Position = createdCharacterPositionInTeam;
		character.RoundMoved = currentRound + 1;
		character.GO = gameObject;
		if (flag)
		{
			character.SetNPCIndex(createdCharacterPositionInTeam);
			character.NPCItem = gameObject.GetComponent<NPCItem>();
			character.NPCItem.NpcData = character.NpcData;
			character.NPCItem.Init((NPC)character);
			character.NPCItem.DrawOrderSprites(goToFront: false, createdCharacterPositionInTeam * 2);
			NPCHand[createdCharacterPositionInTeam] = new List<string>();
			NPCDeckDiscard[createdCharacterPositionInTeam] = new List<string>(NPCDeckDiscard[originalCharacter.NPCIndex]);
			NPCDeck[createdCharacterPositionInTeam] = new List<string>();
			NPCDeck[createdCharacterPositionInTeam].AddRange(NPCHand[originalCharacter.NPCIndex].Where((string text) => text != null));
			NPCDeck[createdCharacterPositionInTeam].AddRange(NPCDeck[originalCharacter.NPCIndex]);
		}
		else
		{
			character.SetHeroIndex(createdCharacterPositionInTeam);
			character.HeroItem = gameObject.GetComponent<HeroItem>();
			character.HeroItem.HeroData = character.HeroData;
			character.HeroItem.Init((Hero)character);
			character.HeroItem.DrawOrderSprites(goToFront: false, createdCharacterPositionInTeam * 2);
			HeroHand[createdCharacterPositionInTeam] = new List<string>();
			HeroDeckDiscard[createdCharacterPositionInTeam] = new List<string>(HeroDeckDiscard[originalCharacter.HeroIndex]);
			HeroDeck[createdCharacterPositionInTeam].AddRange(HeroHand[originalCharacter.HeroIndex].Where((string text) => text != null));
			HeroDeck[createdCharacterPositionInTeam].AddRange(HeroDeck[originalCharacter.HeroIndex]);
		}
		SetInitiatives();
		ReDrawInitiatives();
		(flag ? ((CharacterItem)((NPC)character).NPCItem) : ((CharacterItem)((Hero)character).HeroItem)).SetPosition(instant: true);
		Character[] source;
		if (!flag)
		{
			Character[] teamHero = TeamHero;
			source = teamHero;
		}
		else
		{
			Character[] teamHero = TeamNPC;
			source = teamHero;
		}
		string corruption = source.FirstOrDefault((Character m) => m != null && !(m.Corruption == ""))?.Corruption ?? "";
		character.Corruption = corruption;
		character.AuraList = auraList.Select((Aura a) => a.DeepClone()).ToList();
		character.HpCurrent = originalCharacter.HpCurrent;
		character.UpdateAuraCurseFunctions();
		if (flag)
		{
			string internalId = originalCharacter.InternalId;
			if (!npcCardsCasted.TryGetValue(internalId, out var value))
			{
				value = new List<string>();
			}
			npcCardsCasted[character.InternalId] = new List<string>(value);
		}
		if (card != null)
		{
			if (!string.IsNullOrEmpty(effectTarget))
			{
				EffectsManager.Instance.PlayEffect(card, isCaster: false, isHero: false, characterItem.CharImageT);
			}
			if (card.SummonAura != null && card.SummonAuraCharges > 0)
			{
				character.SetAura(null, card.SummonAura, card.SummonAuraCharges);
			}
			if (card.SummonAura2 != null && card.SummonAuraCharges2 > 0)
			{
				character.SetAura(null, card.SummonAura2, card.SummonAuraCharges2);
			}
			if (card.SummonAura3 != null && card.SummonAuraCharges3 > 0)
			{
				character.SetAura(null, card.SummonAura3, card.SummonAuraCharges3);
			}
		}
		character.IsIllusion = true;
		originalCharacter.IllusionCharacter = character;
		if (flag)
		{
			character.NpcData.FinishCombatOnDead = false;
		}
		return character;
	}

	private Character CreateCharacterWithUniqueId(Character original, bool isNPC, int newPositionInTeam)
	{
		Character character = (isNPC ? ((Character)new NPC
		{
			NpcData = original.NpcData
		}) : ((Character)new Hero
		{
			HeroData = original.HeroData
		}));
		Character[] array;
		if (!isNPC)
		{
			Character[] teamHero = TeamHero;
			array = teamHero;
		}
		else
		{
			Character[] teamHero = TeamNPC;
			array = teamHero;
		}
		Character[] array2 = array;
		character.InitData();
		character.Position = newPositionInTeam;
		array2[newPositionInTeam] = character;
		string randomString = GetRandomString();
		while (targetTransformDict.ContainsKey(randomString))
		{
			randomString = GetRandomString();
		}
		character.InternalId = randomString;
		bool flag;
		do
		{
			flag = false;
			for (int i = 0; i < array2.Length; i++)
			{
				if (i != newPositionInTeam && array2[i] != null && array2[i].InternalId == character.InternalId)
				{
					flag = true;
					character.InternalId += newPositionInTeam;
					break;
				}
			}
		}
		while (flag);
		character.InitData();
		GameManager.Instance.PlayAudio(Globals.Instance.GetAuraCurseData("spark").GetSound(useLegacy: true));
		return character;
	}

	public void CreateNPC(NPCData _npcData, string effectTarget = "", int _position = -1, bool generateFromReload = false, string internalId = "", CardData _cardActive = null, string _casterInternalId = "", float delay = 0f, bool replaceCharacter = false, bool isSummon = false)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("CreateNPC -> " + _npcData?.ToString() + " ID->" + internalId + " INPOS->" + _position, "trace");
		}
		int num = -1;
		bool flag = false;
		if (!generateFromReload && _cardActive != null && (_cardActive.Metamorph || _cardActive.Evolve))
		{
			replaceCharacter = true;
			if (_cardActive.Evolve)
			{
				flag = true;
			}
		}
		if (!replaceCharacter)
		{
			num = GetNPCAvailablePosition();
			if (_position > -1)
			{
				num = _position;
			}
		}
		else if (_casterInternalId != "")
		{
			for (int i = 0; i < TeamNPC.Length; i++)
			{
				if (TeamNPC[i] != null && TeamNPC[i].InternalId == _casterInternalId)
				{
					num = i;
					break;
				}
			}
		}
		else
		{
			replaceCharacter = false;
		}
		if (num <= -1)
		{
			return;
		}
		float num2 = 1f;
		List<Aura> auraList = null;
		if (replaceCharacter)
		{
			if (flag)
			{
				auraList = new List<Aura>(TeamNPC[num].AuraList);
				num2 = (float)TeamNPC[num].HpCurrent / (float)TeamNPC[num].Hp;
			}
			TeamNPC[num].DestroyCharacter();
		}
		NPC nPC = new NPC();
		nPC.NpcData = _npcData;
		nPC.InitData();
		nPC.Position = num;
		TeamNPC[num] = nPC;
		if (internalId == "")
		{
			string randomString = GetRandomString();
			while (targetTransformDict.ContainsKey(randomString))
			{
				randomString = GetRandomString();
			}
			nPC.InternalId = randomString;
		}
		else
		{
			nPC.InternalId = internalId;
		}
		bool flag2 = true;
		while (flag2)
		{
			flag2 = false;
			for (int j = 0; j < TeamNPC.Length; j++)
			{
				if (j != num && TeamNPC[j] != null && TeamNPC[j].InternalId == nPC.InternalId)
				{
					flag2 = true;
					nPC.InternalId += num;
					break;
				}
			}
		}
		nPC.InitData();
		GameObject gameObject = UnityEngine.Object.Instantiate(npcPrefab, Vector3.zero, Quaternion.identity, GO_NPCs.transform);
		gameObject.name = nPC.Id;
		nPC.SetNPCIndex(num);
		nPC.Position = num;
		nPC.RoundMoved = currentRound;
		nPC.GO = gameObject;
		if (targetTransformDict != null && targetTransformDict.ContainsKey(gameObject.name))
		{
			targetTransformDict.Remove(gameObject.name);
		}
		targetTransformDict.Add(gameObject.name, nPC.GO.transform);
		nPC.NPCItem = gameObject.GetComponent<NPCItem>();
		nPC.NPCItem.NpcData = _npcData;
		nPC.NPCItem.Init(nPC);
		nPC.IsSummon = isSummon;
		nPC.NPCItem.SetPosition(instant: true);
		nPC.NPCItem.DrawOrderSprites(goToFront: false, num * 2);
		NPCHand[num] = new List<string>();
		NPCDeckDiscard[num] = new List<string>();
		if (!generateFromReload)
		{
			nPC.NPCItem.InstantFadeOutCharacter();
			Globals.Instance.StartCoroutine(nPC.NPCItem.FadeInCharacter(delay));
			GenerateDecksNPCs(num);
		}
		SetInitiatives();
		ReDrawInitiatives();
		RepositionCharacters();
		if (_cardActive != null)
		{
			if (effectTarget != "")
			{
				EffectsManager.Instance.PlayEffect(_cardActive, isCaster: false, isHero: false, nPC.NPCItem.CharImageT);
			}
			if (_cardActive.SummonAura != null && _cardActive.SummonAuraCharges > 0)
			{
				nPC.SetAura(null, _cardActive.SummonAura, _cardActive.SummonAuraCharges);
			}
			if (_cardActive.SummonAura2 != null && _cardActive.SummonAuraCharges2 > 0)
			{
				nPC.SetAura(null, _cardActive.SummonAura2, _cardActive.SummonAuraCharges2);
			}
			if (_cardActive.SummonAura3 != null && _cardActive.SummonAuraCharges3 > 0)
			{
				nPC.SetAura(null, _cardActive.SummonAura3, _cardActive.SummonAuraCharges3);
			}
		}
		if (!generateFromReload)
		{
			string corruption = "";
			for (int k = 0; k < 4; k++)
			{
				if (TeamNPC[k] != null && TeamNPC[k].NpcData != null && TeamNPC[k].Corruption != "")
				{
					corruption = TeamNPC[k].Corruption;
					break;
				}
			}
			nPC.Corruption = corruption;
		}
		if (flag)
		{
			nPC.AuraList = auraList;
			nPC.HpCurrent = Functions.FuncRoundToInt((float)nPC.Hp * num2);
			nPC.UpdateAuraCurseFunctions();
		}
	}

	public void GenerateDecks()
	{
		string text = "";
		List<string>[] array = new List<string>[4];
		for (int i = 0; i < TeamHero.Length; i++)
		{
			if (TeamHero[i] == null || TeamHero[i].HeroData == null)
			{
				continue;
			}
			array[i] = new List<string>();
			List<string> list = TeamHero[i].Cards;
			if (tutorialCombat)
			{
				switch (i)
				{
				case 0:
					list = new List<string>();
					list.Add("fastStrike");
					list.Add("defend");
					list.Add("rend");
					list.Add("intercept");
					list.Add("intercept");
					break;
				case 3:
					list = new List<string>();
					list.Add("heal");
					list.Add("heal");
					list.Add("heal");
					list.Add("flash");
					list.Add("foresight");
					break;
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				if (!(Globals.Instance.GetCardData(list[j], instantiate: false) == null))
				{
					text = CreateCardInDictionary(list[j]);
					array[i].Add(text);
				}
			}
		}
		for (int k = 0; k < TeamHero.Length; k++)
		{
			if (TeamHero[k] == null || TeamHero[k].HeroData == null)
			{
				continue;
			}
			List<string> list2 = array[k].ShuffleList();
			HeroDeck[k] = list2;
			if (currentRound != 0)
			{
				continue;
			}
			List<string> list3 = new List<string>();
			List<string> list4 = new List<string>();
			for (int num = HeroDeck[k].Count - 1; num >= 0; num--)
			{
				CardData cardData = GetCardData(HeroDeck[k][num]);
				if (cardData.Innate)
				{
					list3.Add(HeroDeck[k][num]);
					HeroDeck[k].RemoveAt(num);
				}
				else if (cardData.Lazy)
				{
					list4.Add(HeroDeck[k][num]);
					HeroDeck[k].RemoveAt(num);
				}
				cardData = null;
			}
			if (list3.Count > 0)
			{
				list3 = list3.ShuffleList();
				list3.AddRange(HeroDeck[k]);
				HeroDeck[k] = new List<string>();
				HeroDeck[k].Clear();
				for (int l = 0; l < list3.Count; l++)
				{
					HeroDeck[k].Add(list3[l]);
				}
			}
			if (list4.Count > 0)
			{
				list4 = list4.ShuffleList();
				for (int m = 0; m < list4.Count; m++)
				{
					HeroDeck[k].Add(list4[m]);
				}
			}
			list3 = null;
			list4 = null;
		}
	}

	public void GenerateDecksNPCs(int _npcIndex = -1)
	{
		string text = "";
		List<string>[] array = new List<string>[4];
		for (int i = 0; i < TeamNPC.Length; i++)
		{
			if ((_npcIndex > -1 && i != _npcIndex) || TeamNPC[i] == null || TeamNPC[i].NpcData == null)
			{
				continue;
			}
			array[i] = new List<string>();
			for (int j = 0; j < TeamNPC[i].Cards.Count; j++)
			{
				bool flag = true;
				for (int k = 0; k < TeamNPC[i].NpcData.AICards.Length; k++)
				{
					try
					{
						if (TeamNPC[i].NpcData.AICards[k].Card.Id == TeamNPC[i].Cards[j] && TeamNPC[i].NpcData.AICards[k].AddCardRound >= currentRound + 1)
						{
							flag = false;
						}
					}
					catch (Exception)
					{
						throw;
					}
				}
				if (flag && !(Globals.Instance.GetCardData(TeamNPC[i].Cards[j], instantiate: false) == null))
				{
					text = CreateCardInDictionary(TeamNPC[i].Cards[j]);
					array[i].Add(text);
				}
			}
		}
		for (int l = 0; l < TeamNPC.Length; l++)
		{
			if ((_npcIndex > -1 && l != _npcIndex) || TeamNPC[l] == null || TeamNPC[l].NpcData == null)
			{
				continue;
			}
			List<string> list = array[l].ShuffleList();
			NPCDeck[l] = list;
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			for (int num = NPCDeck[l].Count - 1; num >= 0; num--)
			{
				CardData cardData = GetCardData(NPCDeck[l][num]);
				if (cardData.Innate)
				{
					list2.Add(NPCDeck[l][num]);
					NPCDeck[l].RemoveAt(num);
				}
				else if (cardData.Lazy)
				{
					list3.Add(NPCDeck[l][num]);
					NPCDeck[l].RemoveAt(num);
				}
				cardData = null;
			}
			if (list2.Count > 0)
			{
				list2 = list2.ShuffleList();
				list2.AddRange(NPCDeck[l]);
				NPCDeck[l] = new List<string>();
				NPCDeck[l].Clear();
				for (int m = 0; m < list2.Count; m++)
				{
					NPCDeck[l].Add(list2[m]);
				}
			}
			if (list3.Count > 0)
			{
				list3 = list3.ShuffleList();
				for (int n = 0; n < list3.Count; n++)
				{
					NPCDeck[l].Add(list3[n]);
				}
			}
			list2 = null;
			list3 = null;
		}
	}

	private void CheckForCombatCode()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("CheckForCombatCode", "net");
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD(AtOManager.Instance.combatGameCode, "net");
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD(AtOManager.Instance.combatGameCode, "synccode");
		}
		eventList.Add("CheckForCombatCode");
		if (AtOManager.Instance.combatGameCode != "")
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("+++++++++++ RESTORE COMBAT FROM +++++++++++++++", "net");
			}
			AtOManager.Instance.combatCardDictionary = null;
			FixCodeSyncFromMasterTOTAL(randomIndex, AtOManager.Instance.combatGameCode);
		}
		eventList.Remove("CheckForCombatCode");
	}

	public string GetCardDictionaryKeys()
	{
		string[] array = new string[cardDictionary.Count];
		cardDictionary.Keys.CopyTo(array, 0);
		return Functions.CompressString(JsonHelper.ToJson(array));
	}

	public string GetCardDictionaryValues()
	{
		CardDataForShare[] array = new CardDataForShare[cardDictionary.Count];
		int num = 0;
		foreach (KeyValuePair<string, CardData> item in cardDictionary)
		{
			CardDataForShare cardDataForShare = new CardDataForShare();
			cardDataForShare.vanish = item.Value.Vanish;
			cardDataForShare.energyReductionPermanent = item.Value.EnergyReductionPermanent;
			cardDataForShare.energyReductionTemporal = item.Value.EnergyReductionTemporal;
			cardDataForShare.energyReductionToZeroPermanent = item.Value.EnergyReductionToZeroPermanent;
			cardDataForShare.energyReductionToZeroTemporal = item.Value.EnergyReductionToZeroTemporal;
			array[num] = cardDataForShare;
			num++;
		}
		return Functions.CompressString(JsonHelper.ToJson(array));
	}

	public void SetCardDictionaryKeysValues(string _keys, string _values)
	{
		if (_keys == "")
		{
			return;
		}
		NET_SaveCardDictionary(_keys, _values);
		AtOManager.Instance.combatCardDictionary = new Dictionary<string, CardData>();
		foreach (KeyValuePair<string, CardData> item in cardDictionary)
		{
			AtOManager.Instance.combatCardDictionary.Add(item.Key, item.Value);
		}
	}

	public string GetHeroItemsForTurnSave()
	{
		List<string> list = new List<string>();
		for (int i = 0; i < 4; i++)
		{
			if (TeamHero[i] != null && !(TeamHero[i].HeroData == null))
			{
				list.Add(TeamHero[i].Weapon);
				list.Add(TeamHero[i].Armor);
				list.Add(TeamHero[i].Jewelry);
				list.Add(TeamHero[i].Accesory);
				list.Add(TeamHero[i].Pet);
			}
		}
		return Functions.CompressString(JsonHelper.ToJson(list.ToArray()));
	}

	public void SetHeroItemsFromTurnSave(string _heroItems)
	{
		if (!(_heroItems == ""))
		{
			_heroItems = Functions.DecompressString(_heroItems);
			teamHeroItemsFromTurnSave = JsonHelper.FromJson<string>(_heroItems).ToList();
		}
	}

	public string GetNPCParamsForTurnSave()
	{
		if (!TeamNPC.Any((NPC npc) => npc != null && (IsIllusion(npc) || npc.IsSummon)))
		{
			return string.Empty;
		}
		List<NPCState> list = new List<NPCState>();
		NPC[] teamNPC = TeamNPC;
		foreach (NPC nPC in teamNPC)
		{
			if (!(nPC?.NpcData == null) && (IsIllusion(nPC) || nPC.IllusionCharacter != null))
			{
				Character illusionCharacter = nPC.IllusionCharacter;
				list.Add(new NPCState
				{
					Id = nPC.NpcData.Id + "_" + nPC.InternalId,
					IsIllusion = nPC.IsIllusion,
					IsIllusionExposed = nPC.IsIllusionExposed,
					IllusionCharacterId = ((illusionCharacter == null) ? "" : (illusionCharacter.NpcData.Id + "_" + illusionCharacter.InternalId)),
					IsSummon = nPC.IsSummon
				});
			}
		}
		if (list.Count != 0)
		{
			return Functions.CompressString(JsonHelper.ToJson(list.ToArray()));
		}
		return string.Empty;
	}

	public void SetNPCParamsFromTurnSave(string _npcParams)
	{
		if (!string.IsNullOrEmpty(_npcParams))
		{
			_npcParams = Functions.DecompressString(_npcParams);
			NPCState[] array = JsonHelper.FromJson<NPCState>(_npcParams);
			if (array != null)
			{
				npcParamsFromTurnSave = array.ToList();
			}
		}
	}

	private void NET_ShareDecks(bool ContinueNewGame = false)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[BEGIN] NET_ShareDecks", "net");
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			if (currentRound == 0)
			{
				string[] array = new string[cardDictionary.Count];
				cardDictionary.Keys.CopyTo(array, 0);
				string text = JsonHelper.ToJson(array);
				CardDataForShare[] array2 = new CardDataForShare[cardDictionary.Count];
				int num = 0;
				foreach (KeyValuePair<string, CardData> item in cardDictionary)
				{
					CardDataForShare cardDataForShare = new CardDataForShare();
					cardDataForShare.vanish = item.Value.Vanish;
					cardDataForShare.energyReductionPermanent = item.Value.EnergyReductionPermanent;
					cardDataForShare.energyReductionTemporal = item.Value.EnergyReductionTemporal;
					cardDataForShare.energyReductionToZeroPermanent = item.Value.EnergyReductionToZeroPermanent;
					cardDataForShare.energyReductionToZeroTemporal = item.Value.EnergyReductionToZeroTemporal;
					array2[num] = cardDataForShare;
					num++;
				}
				string text2 = JsonHelper.ToJson(array2);
				photonView.RPC("NET_SaveCardDictionary", RpcTarget.Others, Functions.CompressString(text), Functions.CompressString(text2));
			}
			else
			{
				photonView.RPC("NET_SaveCardDictionary", RpcTarget.Others, "", "");
			}
			gotDictionary = true;
			string[] array3 = new string[8];
			_ = new string[4];
			for (int i = 0; i < 4; i++)
			{
				array3[i] = "";
				if (HeroDeck[i] != null)
				{
					string text3 = JsonHelper.ToJson(HeroDeck[i].ToArray());
					array3[i] = Functions.CompressString(text3);
				}
			}
			photonView.RPC("NET_SaveCardDeck", RpcTarget.Others, "Hero", array3[0], array3[1], array3[2], array3[3]);
			array3 = new string[4];
			_ = new string[4];
			for (int j = 0; j < 4; j++)
			{
				array3[j] = "";
				if (NPCDeck[j] != null)
				{
					string text4 = JsonHelper.ToJson(NPCDeck[j].ToArray());
					array3[j] = Functions.CompressString(text4);
				}
			}
			photonView.RPC("NET_SaveCardDeck", RpcTarget.Others, "NPC", array3[0], array3[1], array3[2], array3[3]);
			if (ContinueNewGame)
			{
				gotHeroDeck = true;
				gotNPCDeck = true;
				photonView.RPC("NET_BeginMatch", RpcTarget.All, randomIndex);
			}
		}
		else if (ContinueNewGame)
		{
			StartCoroutine(BeginMatch());
		}
	}

	[PunRPC]
	private void NET_BeginMatch(int _randomIndex)
	{
		SetRandomIndex(_randomIndex);
		StartCoroutine(BeginMatch());
	}

	[PunRPC]
	private void NET_SaveCardDictionary(string _keys, string _values)
	{
		if (_keys != "" && _values != "")
		{
			cardDictionary.Clear();
			string[] array = JsonHelper.FromJson<string>(Functions.DecompressString(_keys));
			CardDataForShare[] array2 = JsonHelper.FromJson<CardDataForShare>(Functions.DecompressString(_values));
			for (int i = 0; i < array.Length; i++)
			{
				CardData cardData = null;
				if (!cardDictionary.ContainsKey(array[i]))
				{
					cardData = Globals.Instance.GetCardData(array[i].Split(char.Parse("_"))[0]);
					cardDictionary.Add(array[i], cardData);
				}
				cardDictionary[array[i]].InitClone(array[i]);
				if (cardData == null)
				{
					cardData = UnityEngine.Object.Instantiate(cardDictionary[array[i]]);
				}
				CardDataForShare cardDataForShare = array2[i];
				cardData.Vanish = cardDataForShare.vanish;
				cardData.EnergyReductionPermanent = cardDataForShare.energyReductionPermanent;
				cardData.EnergyReductionTemporal = cardDataForShare.energyReductionTemporal;
				cardData.EnergyReductionToZeroPermanent = cardDataForShare.energyReductionToZeroPermanent;
				cardData.EnergyReductionToZeroTemporal = cardDataForShare.energyReductionToZeroTemporal;
				CardData cardData2 = cardData;
				string internalId = (cardData.Id = array[i]);
				cardData2.InternalId = internalId;
			}
		}
		gotDictionary = true;
	}

	[PunRPC]
	private void NET_SaveCardDeck(string _type, string _arr0, string _arr1, string _arr2, string _arr3)
	{
		List<string> list = new List<string>();
		if (_arr0 != "")
		{
			list.AddRange(JsonHelper.FromJson<string>(Functions.DecompressString(_arr0)));
		}
		List<string> list2 = new List<string>();
		if (_arr1 != "")
		{
			list2.AddRange(JsonHelper.FromJson<string>(Functions.DecompressString(_arr1)));
		}
		List<string> list3 = new List<string>();
		if (_arr2 != "")
		{
			list3.AddRange(JsonHelper.FromJson<string>(Functions.DecompressString(_arr2)));
		}
		List<string> list4 = new List<string>();
		if (_arr3 != "")
		{
			list4.AddRange(JsonHelper.FromJson<string>(Functions.DecompressString(_arr3)));
		}
		if (_type == "Hero")
		{
			HeroDeck[0] = list;
			HeroDeck[1] = list2;
			HeroDeck[2] = list3;
			HeroDeck[3] = list4;
			gotHeroDeck = true;
		}
		else
		{
			NPCDeck[0] = list;
			NPCDeck[1] = list2;
			NPCDeck[2] = list3;
			NPCDeck[3] = list4;
			gotNPCDeck = true;
		}
	}

	private void SaveCardDataIntoDictionary(string _id)
	{
		CardData cardData = cardDictionary[_id];
		if (cardData != null)
		{
			string text = JsonUtility.ToJson(cardData);
			photonView.RPC("NET_SaveCardDataIntoDictionary", RpcTarget.Others, text);
		}
	}

	[PunRPC]
	private void NET_SaveCardDataIntoDictionary(string _id, string _theCard)
	{
		CardData cardData = new CardData();
		JsonUtility.FromJsonOverwrite(_theCard, cardData);
		cardDictionary[_id] = cardData;
	}

	public void ResetCastCardNum()
	{
		if (preCastNum != -1)
		{
			int num = preCastNum;
			preCastNum = -1;
			num = ((num != 0) ? (num - 1) : 9);
			if (num > -1 && num < cardItemTable.Count && cardItemTable[num] != null)
			{
				cardItemTable[num].OnMouseExit();
			}
			ShowCombatKeyboardByConfig();
		}
	}

	public void CastCardNum(int num)
	{
		if (heroActive == -1 || gameBusy || isBeginTournPhase || autoEndCount < 10 || characterWindow.IsActive())
		{
			return;
		}
		bool flag = false;
		Transform transform = null;
		if (!controllerClickedCard)
		{
			keyClickedCard = true;
		}
		if (preCastNum > -1)
		{
			transform = GetTargetByNum(num);
			num = preCastNum;
		}
		if (num > cardItemTable.Count)
		{
			return;
		}
		CardItem cardItem = cardItemTable[num - 1];
		if (cardItem == null || cardItem.CardData == null || !IsThereAnyTargetForCard(cardItem.CardData) || !cardItem.CardData.Playable)
		{
			ResetCastCardNum();
		}
		else
		{
			if (cardItem.GetEnergyCost() > GetHeroEnergy())
			{
				return;
			}
			CardData cardData = cardItem.CardData;
			if (!TeamHero[heroActive].CanPlayCard(cardData))
			{
				ResetCastCardNum();
				return;
			}
			if (CanInstaCast(cardData))
			{
				flag = true;
			}
			else if (transform != null)
			{
				flag = CheckTarget(transform, cardData);
				if (!flag)
				{
					ResetCastCardNum();
					return;
				}
			}
			else
			{
				preCastNum = num;
				ShowCombatKeyboard(_state: true);
				int num2 = preCastNum;
				num2 = ((num2 != 0) ? (num2 - 1) : 9);
				if (num2 > -1 && num2 < cardItemTable.Count && cardItemTable[num2] != null)
				{
					for (int i = 0; i < cardItemTable.Count; i++)
					{
						if (i != num2)
						{
							cardItemTable[i].RestoreCard();
						}
					}
					cardItemTable[num2].OnMouseEnter();
				}
			}
			if (flag)
			{
				CardDrag = false;
				CardActiveT = null;
				canKeyboardCast = false;
				ResetAutoEndCount();
				ResetCastCardNum();
				ShowCombatKeyboard(_state: false);
				cardActive = cardItem.CardData;
				targetTransform = null;
				targetTransform = transform;
				StartCoroutine(CastCard(cardItem));
			}
		}
	}

	private void CastCardPropagation(CardItem theCardItem = null, bool automatic = false, CardData card = null, int energy = -1, int tablePosition = -1)
	{
		if (card == null && theCardItem != null)
		{
			card = theCardItem.GetCardData();
		}
		if (card != null && (card.AutoplayDraw || card.AutoplayEndTurn))
		{
			return;
		}
		string text = "";
		if (targetTransform != null)
		{
			text = targetTransform.gameObject.name;
		}
		GetRandomString();
		int num = randomIndex;
		SetRandomIndex(num);
		int num2 = 11;
		if (theCardItem != null)
		{
			for (int i = 0; i < cardItemTable.Count; i++)
			{
				if (cardItemTable[i].gameObject.name == theCardItem.gameObject.name)
				{
					num2 = i;
					break;
				}
			}
		}
		photonView.RPC("NET_CastCardPropagation", RpcTarget.Others, num, (short)num2, automatic, (short)energy, (short)tablePosition, text);
	}

	private bool IsCountCombat()
	{
		NPC[] teamNPC = GetTeamNPC();
		foreach (NPC nPC in teamNPC)
		{
			if (nPC != null && nPC.Id.StartsWith("count"))
			{
				return true;
			}
		}
		return false;
	}

	[PunRPC]
	private void NET_CastCardPropagation(int _randomIndex, short __theCardItemIndex, bool _automatic, short __energy, short __tablePosition, string _targetTransform)
	{
		SetDamagePreview(theCasterIsHero: false);
		SetOverDeck(state: false);
		int num = __theCardItemIndex;
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[NET_CastCardPropagation] " + num, "net");
		}
		CardItem cardItem = ((num >= 11) ? null : ((cardItemTable == null || !(cardItemTable[num] != null)) ? null : cardItemTable[num]));
		SetRandomIndex(_randomIndex);
		bool automatic = _automatic;
		CardData cardData = new CardData();
		cardData = ((!(cardItem != null)) ? null : cardItem.GetCardData());
		if (_targetTransform == "")
		{
			targetTransform = null;
		}
		else
		{
			targetTransform = targetTransformDict[_targetTransform];
		}
		if (cardData != null)
		{
			SetCardActive(cardData);
		}
		else if (cardItem != null && cardItem.CardData != null)
		{
			SetCardActive(cardItem.CardData);
		}
		StartCoroutine(CastCard(cardItem, automatic, cardData, __energy, __tablePosition));
	}

	private IEnumerator BeginMatch()
	{
		GameManager.Instance.DisableCardCast = false;
		UpdateBossNpc();
		GameManager.Instance.SceneLoaded();
		string key = AtOManager.Instance.GetTownZoneId().ToLower();
		if (GameManager.Instance.IsMultiplayer() && Globals.Instance.ZoneDataSource.ContainsKey(key) && !Globals.Instance.ZoneDataSource[key].ChangeTeamOnEntrance)
		{
			emoteManager.gameObject.SetActive(value: true);
			emoteManager.Init();
		}
		else
		{
			emoteManager.gameObject.SetActive(value: false);
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			while (!gotHeroDeck || !gotNPCDeck || !gotDictionary)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(gotHeroDeck + "||" + gotNPCDeck + "||" + gotDictionary);
				}
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("**************************", "net");
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("WaitingSyncro readyForMatch", "net");
			}
			if (NetworkManager.Instance.IsMaster())
			{
				while (!NetworkManager.Instance.AllPlayersReady("readyForMatch"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked readyForMatch", "net");
				}
				NetworkManager.Instance.PlayersNetworkContinue("readyForMatch");
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("readyForMatch", status: true);
				NetworkManager.Instance.SetStatusReady("readyForMatch");
				while (NetworkManager.Instance.WaitingSyncro["readyForMatch"])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("readyForMatch, we can continue!", "net");
				}
			}
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("BeginMatch combatGameCode =>" + AtOManager.Instance.combatGameCode);
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("Current Round => " + currentRound);
		}
		if (AtOManager.Instance.combatGameCode != "")
		{
			int eventExaust = 0;
			while (eventList.Count > 0)
			{
				if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
				{
					eventListDbg = "";
					for (int i = 0; i < eventList.Count; i++)
					{
						eventListDbg = eventListDbg + eventList[i] + " ";
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("[BeginMatch] Waiting For Eventlist to clean");
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD(eventListDbg);
					}
				}
				eventExaust++;
				if (eventExaust > 200)
				{
					ClearEventList();
					break;
				}
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
		}
		else
		{
			backingDictionary = true;
			BackupCardDictionary();
			while (backingDictionary)
			{
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
		}
		for (int j = 0; j < 4; j++)
		{
			if (TeamHero[j] != null)
			{
				TeamHero[j].Corruption = "";
			}
		}
		if (corruptionCard != null)
		{
			string id = corruptionCard.Id;
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("CORRUPTIONCARD ->" + corruptionCard.Id);
			}
			corruptionCardId = CreateCardInDictionary(id);
			corruptionItem = GetCardData(corruptionCardId).Item;
			if (!corruptionItem.OnlyAddItemToNPCs)
			{
				for (int k = 0; k < 4; k++)
				{
					if (TeamHero[k] != null)
					{
						TeamHero[k].Corruption = id;
					}
				}
			}
			if (currentRound == 0)
			{
				for (int l = 0; l < 4; l++)
				{
					if (TeamNPC[l] != null && TeamNPC[l].NpcData != null)
					{
						TeamNPC[l].Corruption = id;
						if (corruptionItem.MaxHealth > 0)
						{
							TeamNPC[l].Hp += corruptionItem.MaxHealth;
							TeamNPC[l].HpCurrent += corruptionItem.MaxHealth;
							TeamNPC[l].SetHP();
						}
					}
				}
			}
		}
		yield return Globals.Instance.WaitForSeconds(0.1f);
		SetInitiatives();
		if (GameManager.Instance.IsMultiplayer())
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("**************************", "net");
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("WaitingSyncro beginmatch", "net");
			}
			yield return Globals.Instance.WaitForSeconds(0.1f);
			if (NetworkManager.Instance.IsMaster())
			{
				while (!NetworkManager.Instance.AllPlayersReady("beginmatch"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked beginmatch", "net");
				}
				NetworkManager.Instance.PlayersNetworkContinue("beginmatch");
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("beginmatch", status: true);
				NetworkManager.Instance.SetStatusReady("beginmatch");
				while (NetworkManager.Instance.WaitingSyncro["beginmatch"])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("beginmatch, we can continue!", "net");
				}
			}
		}
		CleanTempTransform();
		if (combatData != null)
		{
			InitThermoPieces(combatData);
			if (AtOManager.Instance.combatGameCode == "")
			{
				SetCombatDataEffects(combatData);
				yield return Globals.Instance.WaitForSeconds(0.4f);
			}
			yield return Globals.Instance.WaitForSeconds(0.1f);
		}
		if (MadnessManager.Instance.IsMadnessTraitActive("randomcombats") || GameManager.Instance.IsObeliskChallenge() || AtOManager.Instance.IsChallengeTraitActive("randomcombats"))
		{
			for (int m = 0; m < TeamNPC.Length; m++)
			{
				NPC nPC = TeamNPC[m];
				if (nPC == null || !nPC.Alive)
				{
					continue;
				}
				if (nPC.NpcData.IsNamed)
				{
					if (!GameManager.Instance.IsObeliskChallenge() || (Globals.Instance.GetNodeData(AtOManager.Instance.currentMapNode).NodeCombatTier != Enums.CombatTier.T8 && Globals.Instance.GetNodeData(AtOManager.Instance.currentMapNode).NodeCombatTier != Enums.CombatTier.T9))
					{
						if (immunityListForNamedSaved.Count == 0)
						{
							string auraCurseImmune = Functions.GetAuraCurseImmune(nPC.NpcData);
							bool flag = true;
							if (auraCurseImmune != "")
							{
								if (auraCurseImmune == "bleed" && AtOManager.Instance.TeamHavePerk("mainperkbleed2c"))
								{
									flag = false;
								}
								else if (auraCurseImmune == "sight" && AtOManager.Instance.TeamHavePerk("mainperksight1c"))
								{
									flag = false;
								}
								else if (nPC.AuracurseImmune.Contains(auraCurseImmune))
								{
									flag = false;
								}
								if (flag)
								{
									nPC.AuracurseImmune.Add(auraCurseImmune);
									immunityListForNamedSaved.Add(auraCurseImmune);
								}
							}
						}
						else
						{
							for (int n = 0; n < immunityListForNamedSaved.Count; n++)
							{
								if (!nPC.AuracurseImmune.Contains(immunityListForNamedSaved[n]))
								{
									nPC.AuracurseImmune.Add(immunityListForNamedSaved[n]);
								}
							}
						}
					}
					if (!GameManager.Instance.IsObeliskChallenge() && AtOManager.Instance.IsZoneAffectedByMadness() && currentRound == 0 && AtOManager.Instance.combatGameCode == "")
					{
						if (AtOManager.Instance.GetTownTier() == 0)
						{
							nPC.Hp += 20;
							nPC.HpCurrent += 20;
							nPC.SetHP();
						}
						else if (AtOManager.Instance.GetTownTier() == 1)
						{
							nPC.Hp += 40;
							nPC.HpCurrent += 40;
							nPC.SetHP();
						}
						else if (AtOManager.Instance.GetTownTier() == 2)
						{
							nPC.Hp += 60;
							nPC.HpCurrent += 60;
							nPC.SetHP();
						}
						else if (AtOManager.Instance.GetTownTier() == 3)
						{
							nPC.Hp += 80;
							nPC.HpCurrent += 80;
							nPC.SetHP();
						}
					}
				}
				if (AtOManager.Instance.NodeIsObeliskFinal() && currentRound == 0)
				{
					if (AtOManager.Instance.GetObeliskMadness() > 8 && AtOManager.Instance.IsZoneAffectedByMadness())
					{
						nPC.Hp -= Functions.FuncRoundToInt((float)nPC.Hp * 0.15f);
						nPC.HpCurrent -= Functions.FuncRoundToInt((float)nPC.HpCurrent * 0.15f);
					}
					else
					{
						nPC.Hp -= Functions.FuncRoundToInt((float)nPC.Hp * 0.2f);
						nPC.HpCurrent -= Functions.FuncRoundToInt((float)nPC.HpCurrent * 0.2f);
					}
					nPC.SetHP();
				}
			}
		}
		synchronizing.gameObject.SetActive(value: false);
		ShowMask(state: false);
		yield return Globals.Instance.WaitForSeconds(0.35f);
		MaskWindow.gameObject.SetActive(value: false);
		if (AtOManager.Instance.combatGameCode == "" || (!roundBegan && currentRound == 0))
		{
			OnLoadCombatFinished?.Invoke();
			for (int num = 0; num < TeamHero.Length; num++)
			{
				if (TeamHero[num] != null && !(TeamHero[num].HeroData == null))
				{
					TeamHero[num].SetEvent(Enums.EventActivation.PreBeginCombat);
				}
			}
			for (int num2 = 0; num2 < TeamNPC.Length; num2++)
			{
				if (TeamNPC[num2] != null && !(TeamNPC[num2].NpcData == null))
				{
					TeamNPC[num2].SetEvent(Enums.EventActivation.PreBeginCombat);
				}
			}
			yield return Globals.Instance.WaitForSeconds(0.1f);
			int eventExaust = 0;
			while (eventList.Count > 0)
			{
				if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
				{
					eventListDbg = "";
					for (int num3 = 0; num3 < eventList.Count; num3++)
					{
						eventListDbg = eventListDbg + eventList[num3] + " || ";
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("[PreBeginCombat] Waiting For Eventlist to clean");
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD(eventListDbg);
					}
				}
				eventExaust++;
				if (eventExaust > 300)
				{
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("[PreBeginCombat] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
					}
					ClearEventList();
					break;
				}
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("BEGIN COMBAT EVENT");
			}
			for (int num4 = 0; num4 < TeamHero.Length; num4++)
			{
				if (TeamHero[num4] != null && !(TeamHero[num4].HeroData == null))
				{
					TeamHero[num4].SetEvent(Enums.EventActivation.BeginCombat);
					yield return Globals.Instance.WaitForSeconds(0.1f);
				}
			}
			for (int num4 = 0; num4 < TeamNPC.Length; num4++)
			{
				if (TeamNPC[num4] != null && !(TeamNPC[num4].NpcData == null))
				{
					TeamNPC[num4].SetEvent(Enums.EventActivation.BeginCombat);
					yield return Globals.Instance.WaitForSeconds(0.1f);
				}
			}
			eventExaust = 0;
			while (eventList.Count > 0)
			{
				if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
				{
					eventListDbg = "";
					for (int num5 = 0; num5 < eventList.Count; num5++)
					{
						eventListDbg = eventListDbg + eventList[num5] + " || ";
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("[BeginCombat] Waiting For Eventlist to clean");
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD(eventListDbg);
					}
				}
				eventExaust++;
				if (eventExaust > 300)
				{
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("[BeginCombat] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
					}
					ClearEventList();
					break;
				}
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
		}
		SetInitiatives();
		if (AtOManager.Instance.corruptionAccepted)
		{
			iconCorruption.transform.gameObject.SetActive(value: true);
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("**************************", "net");
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("WaitingSyncro abouttobegin", "net");
			}
			yield return Globals.Instance.WaitForSeconds(0.1f);
			if (NetworkManager.Instance.IsMaster())
			{
				while (!NetworkManager.Instance.AllPlayersReady("abouttobegin"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked abouttobegin", "net");
				}
				NetworkManager.Instance.PlayersNetworkContinue("abouttobegin");
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("abouttobegin", status: true);
				NetworkManager.Instance.SetStatusReady("abouttobegin");
				while (NetworkManager.Instance.WaitingSyncro["abouttobegin"])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("abouttobegin, we can continue!", "net");
				}
			}
		}
		if (GameManager.Instance.CheatMode && GameManager.Instance.UseImmortal)
		{
			int num6 = 99999;
			Hero[] teamHero = TeamHero;
			foreach (Hero obj in teamHero)
			{
				obj.Hp = num6;
				obj.HpCurrent = num6;
				obj.SetHP();
			}
		}
		combatLoading = false;
		NextTurnFunction();
		yield return null;
	}

	private bool TryGetNPCsWithMasterReweaverAbility(CardData data, Character casterCharacter, out List<NPC> npcsWithAbility)
	{
		npcsWithAbility = new List<NPC>();
		if (casterCharacter == null || !casterCharacter.Alive)
		{
			return false;
		}
		if (!(casterCharacter is Hero) || casterCharacter.HeroData == null)
		{
			return false;
		}
		if (!IsFightCard(data) || data.Corrupted)
		{
			return false;
		}
		NPC[] teamNPC = TeamNPC;
		foreach (NPC nPC in teamNPC)
		{
			if (nPC == null || !nPC.Alive)
			{
				continue;
			}
			string[] masterReweaverKeys = GameUtils.MasterReweaverKeys;
			foreach (string id in masterReweaverKeys)
			{
				if (nPC.HasEnchantment(id))
				{
					npcsWithAbility.Add(nPC);
					break;
				}
			}
		}
		return npcsWithAbility.Count > 0;
	}

	private void SetEventDirect(string theEvent, bool automatic = true, bool add = false)
	{
		if (automatic)
		{
			if (!eventList.Contains(theEvent))
			{
				Functions.DebugLogGD("^^^^^^^^^ SetEventDirect [ON] " + theEvent + " ^^^^^^^^^^^", "trace");
				eventList.Add(theEvent);
			}
			else
			{
				Functions.DebugLogGD("^^^^^^^^^ SetEventDirect [OFF] " + theEvent + " ^^^^^^^^^^^", "trace");
				eventList.Remove(theEvent);
			}
		}
		else if (add)
		{
			if (!eventList.Contains(theEvent))
			{
				Functions.DebugLogGD("^^^^^^^^^ SetEventDirect [ON] " + theEvent + " ^^^^^^^^^^^", "trace");
				eventList.Add(theEvent);
			}
		}
		else if (eventList.Contains(theEvent))
		{
			Functions.DebugLogGD("^^^^^^^^^ SetEventDirect [OFF] " + theEvent + " ^^^^^^^^^^^", "trace");
			eventList.Remove(theEvent);
		}
	}

	public void SetEventCo(Character theChar, Enums.EventActivation theEvent, Character target = null, int auxInt = 0, string auxString = "", Character caster = null)
	{
		StartCoroutine(SetEventCoAction(theChar, theEvent, target, auxInt, auxString, caster));
	}

	private IEnumerator SetEventCoAction(Character theChar, Enums.EventActivation theEvent, Character target = null, int auxInt = 0, string auxString = "", Character caster = null)
	{
		while (waitingDeathScreen)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[SETEVENTACTION] waitingDeathScreen");
			}
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
		if (theChar == null || !theChar.Alive)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[SETEVENTACTION] break by death");
			}
			ClearEventList();
			yield break;
		}
		string eventCode = new StringBuilder().Append(theChar.Id).Append(auxInt).Append(auxString)
			.Append(theEvent.ToString())
			.ToString();
		if (!eventList.Contains(eventCode))
		{
			eventList.Add(eventCode);
		}
		if (theEvent != Enums.EventActivation.BeginTurn || !theChar.HasEffectSkipsTurn())
		{
			if (theChar.IsHero && theChar.ActivateTrait(theEvent, target, auxInt, auxString))
			{
				yield return Globals.Instance.WaitForSeconds(0.2f);
				while (generatedCardTimes > 0)
				{
					yield return Globals.Instance.WaitForSeconds(0.5f);
					generatedCardTimes--;
				}
			}
			else if (target != null && target.Alive)
			{
				TraitEnum[] array = alwaysActiveTraits;
				foreach (TraitEnum trait in array)
				{
					if (theEvent == GetActivationEvent(trait))
					{
						GetHeroForTrait(trait, theChar, target, caster)?.ActivateTrait(theEvent, target, auxInt, auxString);
					}
				}
			}
			if (generatedCardTimes < 0)
			{
				generatedCardTimes = 0;
			}
			theChar.ActivateItem(theEvent, target, auxInt, auxString);
			if (waitingItemTrait)
			{
				while (waitingItemTrait)
				{
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("[" + theEvent.ToString() + "] waitingItemTrait " + waitingItemTrait);
					}
					waitingItemTrait = false;
					yield return Globals.Instance.WaitForSeconds(0.1f);
				}
			}
			generatedCardTimes = 0;
		}
		theChar.FinishSetEvent(theEvent, target, auxInt, auxString);
		yield return Globals.Instance.WaitForSeconds(0.01f);
		if (eventList.Contains(eventCode))
		{
			eventList.Remove(eventCode);
		}
		foreach (ModelVisualsUpdater modelVisualsUpdater in modelVisualsUpdaters)
		{
			modelVisualsUpdater.UpdateVisuals();
		}
	}

	private Hero GetHeroForTrait(TraitEnum trait, Character theChar, Character target, Character caster)
	{
		Hero[] teamHero = GetTeamHero();
		switch (trait)
		{
		case TraitEnum.PurifyingResonance:
			return teamHero.FirstOrDefault((Hero h) => h.HasTrait(trait) || h.HasEnchantment(trait.ToString()));
		case TraitEnum.DarkMercy:
			if (!theChar.IsHero)
			{
				return teamHero.FirstOrDefault((Hero h) => h.HasTrait(trait) || h.HasEnchantment(trait.ToString()));
			}
			return null;
		case TraitEnum.DarkOverflow:
			if (caster is Hero { Alive: not false } hero && hero.HasTrait(TraitEnum.DarkOverflow))
			{
				return hero;
			}
			return teamHero.FirstOrDefault((Hero h) => h.HasTrait(TraitEnum.DarkOverflow) && AtOManager.Instance.CharacterHavePerk(h, "mainperkdark2d"));
		default:
			return null;
		}
	}

	private Enums.EventActivation GetActivationEvent(TraitEnum trait)
	{
		return Globals.Instance.GetTraitData(trait.ToString().ToLower())?.Activation ?? Enums.EventActivation.None;
	}

	private void SetCombatDataEffects(CombatData _combatData)
	{
		if (_combatData.HealHeroes)
		{
			for (int i = 0; i < TeamHero.Length; i++)
			{
				if (TeamHero[i] != null && !(TeamHero[i].HeroData == null))
				{
					TeamHero[i].HealToMax();
				}
			}
		}
		for (int j = 0; j < _combatData.CombatEffect.Length; j++)
		{
			CombatEffect combatEffect = _combatData.CombatEffect[j];
			AuraCurseData auraCurseData = combatEffect.AuraCurse;
			if (auraCurseData == null)
			{
				continue;
			}
			bool flag = combatEffect.AuraCurseTarget == Enums.CombatUnit.All;
			bool flag2 = combatEffect.AuraCurseTarget == Enums.CombatUnit.Heroes;
			bool flag3 = combatEffect.AuraCurseTarget == Enums.CombatUnit.Monsters;
			for (int k = 0; k < TeamHero.Length; k++)
			{
				if (TeamHero[k] != null && !(TeamHero[k].HeroData == null))
				{
					bool flag4 = false;
					if (flag || flag2)
					{
						flag4 = true;
					}
					else if (k == 0 && combatEffect.AuraCurseTarget == Enums.CombatUnit.Hero0)
					{
						flag4 = true;
					}
					else if (k == 1 && combatEffect.AuraCurseTarget == Enums.CombatUnit.Hero1)
					{
						flag4 = true;
					}
					else if (k == 2 && combatEffect.AuraCurseTarget == Enums.CombatUnit.Hero2)
					{
						flag4 = true;
					}
					else if (k == 3 && combatEffect.AuraCurseTarget == Enums.CombatUnit.Hero3)
					{
						flag4 = true;
					}
					if (flag4)
					{
						TeamHero[k].SetAuraTrait(null, auraCurseData.Id, combatEffect.AuraCurseCharges);
					}
				}
			}
			for (int l = 0; l < TeamNPC.Length; l++)
			{
				if (TeamNPC[l] != null && !(TeamNPC[l].NpcData == null))
				{
					bool flag4 = false;
					if (flag || flag3)
					{
						flag4 = true;
					}
					else if (l == 0 && combatEffect.AuraCurseTarget == Enums.CombatUnit.Monster0)
					{
						flag4 = true;
					}
					else if (l == 1 && combatEffect.AuraCurseTarget == Enums.CombatUnit.Monster1)
					{
						flag4 = true;
					}
					else if (l == 2 && combatEffect.AuraCurseTarget == Enums.CombatUnit.Monster2)
					{
						flag4 = true;
					}
					else if (l == 3 && combatEffect.AuraCurseTarget == Enums.CombatUnit.Monster3)
					{
						flag4 = true;
					}
					if (flag4)
					{
						TeamNPC[l].SetAuraTrait(null, auraCurseData.Id, combatEffect.AuraCurseCharges);
					}
				}
			}
		}
	}

	public void CleanTempTransform()
	{
		foreach (Transform item in tempTransform)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
	}

	public void CleanTempVanishedTransform()
	{
		foreach (Transform item in tempVanishedTransform)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
	}

	[PunRPC]
	private void NET_Endturn()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("NET_EndTurn", "trace");
		}
		EndTurn();
	}

	public void EndTurn(bool forceIt = false)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[ENDTURN]", "trace");
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[ENDTURN] GameStatus->" + gameStatus, "trace");
		}
		Debug.Log("EndTurn");
		ShowHandMask(state: true);
		if (botEndTurn.gameObject.activeSelf)
		{
			botEndTurn.gameObject.SetActive(value: false);
		}
		if (cursorArrow.gameObject.activeSelf)
		{
			cursorArrow.gameObject.SetActive(value: false);
		}
		heroTurn = false;
		if (gameStatus == "EndTurn" && !forceIt)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[ENDTURN] ** exit because gameStatus == EndTurn", "trace");
			}
			return;
		}
		gameStatus = "EndTurn";
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD(gameStatus, "gamestatus");
		}
		ShowEnergyCounter(state: false);
		ClearCanInstaCastDict();
		ResetCastCardNum();
		ResetCharactersPing();
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[ENDTURN] Call EndTurnWait", "trace");
		}
		if (GameManager.Instance.IsMultiplayer() && IsYourTurn())
		{
			photonView.RPC("NET_Endturn", RpcTarget.Others);
		}
		StartCoroutine(EndTurnWait());
	}

	private IEnumerator EndTurnWait()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[ENDTURNWAIT]", "trace");
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[ENDTURNWAIT] GameStatus->" + gameStatus, "trace");
		}
		string codeGenCheck = GenerateSyncCodeForCheckingAction();
		yield return Globals.Instance.WaitForSeconds(0.1f);
		int _logEntryIndex = logDictionary.Count;
		CreateLogEntry(_initial: true, "endeffects:" + _logEntryIndex, "", theHero, theNPC, null, null, currentRound, Enums.EventActivation.EndTurn);
		if (theHero != null && theHero.Alive)
		{
			theHero.SetEvent(Enums.EventActivation.EndTurn);
		}
		if (characterKilled)
		{
			yield return Globals.Instance.WaitForSeconds(0.1f);
		}
		int eventExaust = 0;
		while (eventList.Count > 0)
		{
			if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
			{
				eventListDbg = "";
				for (int i = 0; i < eventList.Count; i++)
				{
					eventListDbg = eventListDbg + eventList[i] + " || ";
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[ENDTURNWAIT] Waiting For Eventlist to clean", "trace");
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(eventListDbg, "trace");
				}
			}
			eventExaust++;
			if (eventExaust > 400)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[ENDTURNWAIT] Beak by eventexaust", "trace");
				}
				ClearEventList();
				generatedCardTimes = 0;
			}
			yield return Globals.Instance.WaitForSeconds(0.02f);
		}
		yield return Globals.Instance.WaitForSeconds(0.1f);
		string codeGen = GenerateSyncCodeForCheckingAction();
		int exaustCodeGen = 0;
		while (codeGen != codeGenCheck)
		{
			codeGen = codeGenCheck;
			yield return Globals.Instance.WaitForSeconds(0.02f);
			codeGenCheck = GenerateSyncCodeForCheckingAction();
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("** CHECK " + (codeGen == codeGenCheck) + " **", "trace");
			}
			exaustCodeGen++;
			if (exaustCodeGen > 300)
			{
				codeGen = codeGenCheck;
			}
		}
		if (MatchIsOver)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[ENDTURNWAIT] Broken by finish game", "trace");
			}
			yield break;
		}
		gameStatus = "EndTurn";
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD(gameStatus, "gamestatus");
		}
		CleanTempTransform();
		CleanTempVanishedTransform();
		cardActive = null;
		cardGos.Clear();
		cardIteration.Clear();
		ShowTraitInfo(state: false, clearText: true);
		if (theHero != null)
		{
			for (int num = cardItemTable.Count - 1; num >= 0; num--)
			{
				if (cardItemTable[num] != null)
				{
					cardItemTable[num].DrawBorder("");
					cardItemTable[num].active = false;
					cardItemTable[num].DisableCollider();
				}
			}
			SetGameBusy(state: false);
			StartCoroutine(MoveItemsOut(state: true));
			theHero.RoundMoved = currentRound;
			if (theHero.HeroItem != null)
			{
				theHero.HeroItem.ActivateMark(state: false);
			}
			eventExaust = 0;
			while (CountHeroHand() > 0)
			{
				CardItem cardItem = cardItemTable[0];
				if (cardItem != null)
				{
					cardItem.DiscardCard(discardedFromHand: true, Enums.CardPlace.Discard, CountHeroDiscard() + 1);
					eventExaust = 0;
				}
				if (GameManager.Instance.IsMultiplayer())
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
				{
					yield return Globals.Instance.WaitForSeconds(0.02f);
				}
				else
				{
					yield return null;
				}
				eventExaust++;
				if (eventExaust > 250)
				{
					HeroHand[heroActive].Clear();
				}
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[ENDTURNCO] MoveDecksOut", "trace");
			}
			MovingDeckPile = true;
			HideExhaust();
			if (heroActive > -1)
			{
				for (int j = 0; j < HeroDeck[heroActive].Count; j++)
				{
					if (cardDictionary.ContainsKey(HeroDeck[heroActive][j]) && cardDictionary[HeroDeck[heroActive][j]] != null)
					{
						cardDictionary[HeroDeck[heroActive][j]].ResetExhaust();
					}
				}
				for (int k = 0; k < HeroDeckDiscard[heroActive].Count; k++)
				{
					if (cardDictionary.ContainsKey(HeroDeckDiscard[heroActive][k]) && cardDictionary[HeroDeckDiscard[heroActive][k]] != null)
					{
						cardDictionary[HeroDeckDiscard[heroActive][k]].ResetExhaust();
					}
				}
				for (int l = 0; l < HeroDeckVanish[heroActive].Count; l++)
				{
					if (cardDictionary.ContainsKey(HeroDeckVanish[heroActive][l]) && cardDictionary[HeroDeckVanish[heroActive][l]] != null)
					{
						cardDictionary[HeroDeckVanish[heroActive][l]].ResetExhaust();
					}
				}
			}
			StartCoroutine(MoveDecksOut(state: true));
			while (MovingDeckPile)
			{
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[ENDTURNCO] MoveDecksOut END", "trace");
			}
			waitExecution = true;
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[ENDTURNCO] ActionsCharacterEndTurn", "trace");
			}
			ActionsCharacterEndTurn();
			eventExaust = 0;
			while (waitExecution)
			{
				yield return Globals.Instance.WaitForSeconds(0.01f);
				eventExaust++;
				if (eventExaust > 300)
				{
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("[ENDTURNCO] Waitexecution EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
					}
					waitExecution = false;
				}
			}
			if (characterKilled)
			{
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
			eventExaust = 0;
			while (eventList.Count > 0)
			{
				if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
				{
					eventListDbg = "";
					for (int m = 0; m < eventList.Count; m++)
					{
						eventListDbg = eventListDbg + eventList[m] + " || ";
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("[ActionsCharacterEndTurn] Waiting For Eventlist to clean", "trace");
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD(eventListDbg);
					}
				}
				eventExaust++;
				if (eventExaust > 200)
				{
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("[ActionsCharacterEndTurn] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
					}
					ClearEventList();
					break;
				}
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[ENDTURNCO] END ActionsCharacterEndTurn");
			}
		}
		else if (theNPC != null)
		{
			if (theNPC != null && theNPC.Alive && theNPC.NPCItem != null)
			{
				theNPC.NPCItem.MoveToCenterBack();
			}
			if (theNPC.RoundMoved != currentRound)
			{
				theNPC.RoundMoved = currentRound;
				if (theNPC.NPCItem != null)
				{
					theNPC.NPCItem.ActivateMark(state: false);
				}
				theNPC.DiscardHand();
				waitExecution = true;
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[ENDTURNCO] ActionsCharacterEndTurn", "trace");
				}
				ActionsCharacterEndTurn();
				eventExaust = 0;
				while (waitExecution)
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
					eventExaust++;
					if (eventExaust > 400)
					{
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("[ENDTURNCO] Waitexecution EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
						}
						waitExecution = false;
					}
				}
				yield return Globals.Instance.WaitForSeconds(0.1f);
				eventExaust = 0;
				while (eventList.Count > 0)
				{
					if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
					{
						eventListDbg = "";
						for (int n = 0; n < eventList.Count; n++)
						{
							eventListDbg = eventListDbg + eventList[n] + " || ";
						}
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("[ActionsCharacterEndTurn] Waiting For Eventlist to clean");
						}
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD(eventListDbg);
						}
					}
					eventExaust++;
					if (eventExaust > 300)
					{
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("[ActionsCharacterEndTurn] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
						}
						ClearEventList();
						break;
					}
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[ENDTURNCO] END ActionsCharacterEndTurn");
				}
			}
		}
		heroActive = -1;
		npcActive = -1;
		theHeroPreAutomatic = null;
		theNPCPreAutomatic = null;
		HideExhaust();
		SetGlobalOutlines(state: false);
		SetDamagePreview(theCasterIsHero: false);
		SetOverDeck(state: false);
		CreateLogEntry(_initial: false, "endeffects:" + _logEntryIndex, "", theHero, theNPC, null, null, currentRound, Enums.EventActivation.EndTurn);
		logDictionaryAux = new Dictionary<string, LogEntry>();
		foreach (KeyValuePair<string, LogEntry> item in logDictionary)
		{
			logDictionaryAux.Add(item.Key, item.Value);
		}
		NextTurnFunction();
	}

	public bool IsGameBusy()
	{
		return gameBusy;
	}

	public void SetGameBusy(bool state)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("********** SETGAMEBUSY (" + state + ") *********", "gamebusy");
		}
		gameBusy = state;
	}

	public int GameRound()
	{
		return currentRound;
	}

	internal void NextTurnFunction()
	{
		StartCoroutine(NextTurn());
		if (GameManager.Instance.CheatMode && GameManager.Instance.WinMatchOnStart)
		{
			FinishCombat();
		}
	}

	private IEnumerator NextTurn()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[NEXTTURN]");
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[NEXTTURN] GameStatus->" + gameStatus);
		}
		if (gameStatus == "NextTurn" && Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[NEXTTURN] *** EXIT because GameStatus == NextTurn");
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[NEXTTURN] ASSIGN GameStatus = NextTurn");
		}
		gameStatus = "NextTurn";
		cursorArrow.StopDraw();
		PopupManager.Instance.ClosePopup();
		if (characterKilled)
		{
			yield return Globals.Instance.WaitForSeconds(0.3f);
		}
		for (int i = 0; i < TeamNPC.Length; i++)
		{
			if (TeamNPC[i] != null && TeamNPC[i].Alive && TeamNPC[i].NPCItem == null)
			{
				TeamNPC[i] = null;
			}
		}
		while (waitingDeathScreen)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[NEXTTURN] waitingDeathScreen");
			}
			yield return Globals.Instance.WaitForSeconds(0.1f);
		}
		while (waitingKill)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[NEXTTURN] waitingKill");
			}
			yield return Globals.Instance.WaitForSeconds(0.1f);
		}
		if (CheckMatchIsOver())
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[NEXTTURN] FINISHCOMBAT");
			}
			FinishCombat();
			yield break;
		}
		CleanTempTransform();
		SortCharacterSprites();
		int _queueIndex = 0;
		while (ctQueue.Count > 0)
		{
			yield return Globals.Instance.WaitForSeconds(0.1f);
			if (_queueIndex > 50)
			{
				ctQueue.Clear();
			}
		}
		castingCardListMP.Clear();
		int eventExaust = 0;
		while (eventList.Count > 0)
		{
			if (GameManager.Instance.GetDeveloperMode() && eventExaust % 1 == 0)
			{
				eventListDbg = "";
				for (int j = 0; j < eventList.Count; j++)
				{
					eventListDbg = eventListDbg + eventList[j] + " || ";
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[NEXTTURN] Waiting For Eventlist to clean");
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(eventListDbg);
				}
			}
			eventExaust++;
			if (eventExaust > 300)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[NEXTTURN] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
				}
				ClearEventList();
				break;
			}
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
		characterKilled = false;
		NextTurnAfterClean();
	}

	private void NextTurnAfterClean()
	{
		if (!beforeSyncCodeLocked)
		{
			beforeSyncCodeLocked = true;
			StopCoroutines();
			if (GameManager.Instance.IsMultiplayer())
			{
				StartCoroutine("NextTurnSyncro");
				return;
			}
			SetMasterSyncCode();
			currentGameCodeForReload = currentGameCode;
			StartCoroutine("NextTurnContinue");
		}
	}

	private IEnumerator NextTurnSyncro()
	{
		yield return Globals.Instance.WaitForSeconds(0.01f);
		if (currentRound == 0)
		{
			yield return Globals.Instance.WaitForSeconds(0.1f);
		}
		if (NetworkManager.Instance.IsMaster())
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("+++ Checking +++", "synccode");
			}
			SetMasterSyncCode(md5: true);
			int syncExhaust = 0;
			while (CheckSyncCodeDict() == 0)
			{
				if (GameManager.Instance.GetDeveloperMode() && syncExhaust % 50 == 0 && Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Waiting For CheckSyncCodeDict in MD5", "synccode");
				}
				syncExhaust++;
				yield return Globals.Instance.WaitForSeconds(0.01f);
				if (syncExhaust > 500)
				{
					ReloadCombat("CheckSyncCodeDict exhausted time");
					yield break;
				}
			}
			string theCode = Functions.Md5Sum(GenerateSyncCode());
			string theCode2 = "";
			if (AllDesyncEqual(theCode))
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("MD5 codes OK continue", "synccode");
				}
			}
			else
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("MD5 codes were different", "synccode");
				}
				GenerateSyncCodeDict();
				yield return Globals.Instance.WaitForSeconds(0.01f);
				SetMasterSyncCode();
				yield return Globals.Instance.WaitForSeconds(0.01f);
				RequestSyncCode();
				if (GameManager.Instance.GetDeveloperMode())
				{
					Debug.LogError("There was an error with the sync codes");
					Debug.Log(currentGameCode);
				}
				syncExhaust = 0;
				while (CheckSyncCodeDict() == 0)
				{
					if (GameManager.Instance.GetDeveloperMode() && syncExhaust % 50 == 0 && Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("Waiting For CheckSyncCodeDict plain", "synccode");
					}
					syncExhaust++;
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				switch (DesyncFixable())
				{
				case 0:
					theCode2 = GenerateSyncCode();
					if (GameManager.Instance.GetDeveloperMode())
					{
						Debug.Log("Fixable OK");
						Debug.Log("Code -> " + theCode2);
					}
					break;
				case 2:
				{
					string[] array = new string[cardDictionary.Count];
					cardDictionary.Keys.CopyTo(array, 0);
					string text = JsonHelper.ToJson(array);
					CardDataForShare[] array2 = new CardDataForShare[cardDictionary.Count];
					int num = 0;
					foreach (KeyValuePair<string, CardData> item in cardDictionary)
					{
						CardDataForShare cardDataForShare = new CardDataForShare();
						cardDataForShare.vanish = item.Value.Vanish;
						cardDataForShare.energyReductionPermanent = item.Value.EnergyReductionPermanent;
						cardDataForShare.energyReductionTemporal = item.Value.EnergyReductionTemporal;
						cardDataForShare.energyReductionToZeroPermanent = item.Value.EnergyReductionToZeroPermanent;
						cardDataForShare.energyReductionToZeroTemporal = item.Value.EnergyReductionToZeroTemporal;
						array2[num] = cardDataForShare;
						num++;
					}
					string text2 = JsonHelper.ToJson(array2);
					Debug.LogError("[CheckSyncCode] dictionary redone");
					photonView.RPC("NET_SaveCardDictionary", RpcTarget.Others, Functions.CompressString(text), Functions.CompressString(text2));
					theCode2 = currentGameCode;
					yield return Globals.Instance.WaitForSeconds(1f);
					break;
				}
				default:
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("NOT Fixable - ReloadCombat", "synccode");
					}
					ReloadCombat("[CheckSyncCode] NOT Fixable - ReloadCombat");
					yield break;
				}
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("Game ready, Everybody checked FixingSyncCode", "synccode");
			}
			SetRandomIndex(randomIndex);
			if (theCode2 == "")
			{
				photonView.RPC("NET_FixCodeSyncFromMaster", RpcTarget.Others, randomIndex, "");
			}
			else
			{
				photonView.RPC("NET_FixCodeSyncFromMaster", RpcTarget.Others, randomIndex, Functions.CompressString(theCode2));
			}
			if (coroutineSyncFixSyncCode != null)
			{
				StopCoroutine(coroutineSyncFixSyncCode);
			}
			coroutineSyncFixSyncCode = StartCoroutine(ReloadCombatCo("FixingSyncCode"));
			while (!NetworkManager.Instance.AllPlayersReady("FixingSyncCode"))
			{
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			if (coroutineSyncFixSyncCode != null)
			{
				StopCoroutine(coroutineSyncFixSyncCode);
			}
			NetworkManager.Instance.PlayersNetworkContinue("FixingSyncCode");
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("FixingSyncCode Received continue", "net");
			}
		}
		else
		{
			NetworkManager.Instance.SetWaitingSyncro("FixingSyncCode", status: true);
			SendSyncCode(md5: true);
			while (NetworkManager.Instance.WaitingSyncro["FixingSyncCode"])
			{
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("FixingSyncCode, we can continue!", "net");
			}
		}
		currentGameCodeForReload = currentGameCode;
		NextTurnContinuePlain();
	}

	private void NextTurnContinuePlain()
	{
		StartCoroutine(NextTurnContinue());
	}

	private IEnumerator NextTurnContinue()
	{
		AtOManager.Instance.SaveGameTurn();
		backingDictionary = true;
		BackupCardDictionary();
		while (backingDictionary)
		{
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
		if (logDictionary.Count > 0)
		{
			CreateLogEntry(_initial: true, "", "", theHero, theNPC, null, null, currentRound, Enums.EventActivation.EndTurn);
		}
		CombatTextIterations = 0;
		generatedCardTimes = 0;
		ResetFailCount();
		if (theHero != null && theHero.Alive)
		{
			CleanPrePostDamageDictionary(theHero.Id);
			if (theHero.HeroItem != null)
			{
				theHero.HeroItem.CalculateDamagePrePostForThisCharacter();
			}
		}
		if (theNPC != null && theNPC.Alive)
		{
			CleanPrePostDamageDictionary(theNPC.Id);
			if (theNPC.NPCItem != null)
			{
				theNPC.NPCItem.CalculateDamagePrePostForThisCharacter();
			}
		}
		theHero = null;
		theNPC = null;
		bool flag = true;
		for (int i = 0; i < CharOrder.Count; i++)
		{
			if (CharOrder[i].hero != null)
			{
				if (CharOrder[i].hero.RoundMoved < currentRound && CharOrder[i].hero.Alive)
				{
					flag = false;
					break;
				}
			}
			else if (CharOrder[i].npc.RoundMoved < currentRound && CharOrder[i].npc.Alive)
			{
				flag = false;
				break;
			}
		}
		int eventExaust;
		if (flag)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("CURRENT ROUND -> " + currentRound, "trace");
			}
			if (currentRound > 0)
			{
				for (int j = 0; j < TeamHero.Length; j++)
				{
					if (TeamHero[j] == null || TeamHero[j].HeroData == null || !TeamHero[j].Alive)
					{
						continue;
					}
					waitExecution = true;
					TeamHero[j].EndRound();
					eventExaust = 0;
					while (waitExecution)
					{
						yield return Globals.Instance.WaitForSeconds(0.01f);
						eventExaust++;
						if (eventExaust > 400)
						{
							if (Globals.Instance.ShowDebug)
							{
								Functions.DebugLogGD("[ENDTURNCO] Waitexecution EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
							}
							waitExecution = false;
						}
					}
				}
				for (int j = 0; j < TeamNPC.Length; j++)
				{
					if (TeamNPC[j] == null || TeamNPC[j].NpcData == null || !TeamNPC[j].Alive)
					{
						continue;
					}
					waitExecution = true;
					TeamNPC[j].EndRound();
					eventExaust = 0;
					while (waitExecution)
					{
						yield return Globals.Instance.WaitForSeconds(0.01f);
						eventExaust++;
						if (eventExaust > 400)
						{
							if (Globals.Instance.ShowDebug)
							{
								Functions.DebugLogGD("[ENDTURNCO] Waitexecution EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
							}
							waitExecution = false;
						}
					}
				}
			}
			if (MatchIsOver)
			{
				yield break;
			}
			if (currentRound == 0)
			{
				if (corruptionItem != null && corruptionItem.Activation == Enums.EventActivation.CorruptionCombatStart)
				{
					CardData cardData = GetCardData(corruptionCardId);
					cardData.EnergyCost = 0;
					cardData.Vanish = true;
					GenerateNewCard(1, corruptionCardId, createCard: false, Enums.CardPlace.Vanish);
					for (int k = 0; k < 4; k++)
					{
						if (TeamNPC[k] != null && TeamNPC[k].Alive)
						{
							TeamNPC[k].DoItem(Enums.EventActivation.CorruptionCombatStart, cardData, corruptionItem.Id, null, 0, "", 0, null);
							break;
						}
					}
					yield return Globals.Instance.WaitForSeconds(1.5f);
					SetGameBusy(state: false);
				}
			}
			else
			{
				bool flag2 = false;
				if (scarabSpawned == "" && combatData != null && Globals.Instance.GetNodeData(AtOManager.Instance.currentMapNode) != null)
				{
					switch (Globals.Instance.GetNodeData(AtOManager.Instance.currentMapNode).NodeCombatTier)
					{
					case Enums.CombatTier.T2:
						if (GameManager.Instance.IsObeliskChallenge())
						{
							flag2 = true;
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
						}
						break;
					case Enums.CombatTier.T7:
						if (GameManager.Instance.IsObeliskChallenge())
						{
							flag2 = true;
						}
						break;
					}
				}
				if (flag2)
				{
					int j = GetNPCAvailablePosition();
					if (j > -1)
					{
						int randomIntRange = GetRandomIntRange(0, 100);
						int num = 7;
						if (AtOManager.Instance.TeamHaveItem("scarabyrare"))
						{
							num += 40;
						}
						else if (AtOManager.Instance.TeamHaveItem("scaraby"))
						{
							num += 30;
						}
						if (randomIntRange < num)
						{
							bool flag3 = false;
							int scarabType = 0;
							while (!flag3)
							{
								scarabType = GetRandomIntRange(0, 4);
								flag3 = ((!AtOManager.Instance.TeamHaveItem("scarabyrare") || scarabType < 3) ? true : false);
							}
							string text = "";
							if (!GameManager.Instance.IsObeliskChallenge())
							{
								if (AtOManager.Instance.GetTownTier() == 2)
								{
									text = ((AtOManager.Instance.GetNgPlus() <= 0) ? "_b" : "_plus_b");
								}
								else if (AtOManager.Instance.GetNgPlus() > 0)
								{
									text = "_plus";
								}
							}
							else if (AtOManager.Instance.GetObeliskMadness() > 8)
							{
								text = ((combatData.CombatTier != Enums.CombatTier.T6 && combatData.CombatTier != Enums.CombatTier.T7) ? "_plus" : "_plus_b");
							}
							else if (combatData.CombatTier == Enums.CombatTier.T6 || combatData.CombatTier == Enums.CombatTier.T7)
							{
								text = "_b";
							}
							switch (scarabType)
							{
							case 0:
								scarabSpawned = "crystalscarab";
								break;
							case 1:
								scarabSpawned = "goldenscarab";
								break;
							case 2:
								scarabSpawned = "jadescarab";
								break;
							default:
								scarabSpawned = "scourgescarab";
								break;
							}
							NPCData nPC = Globals.Instance.GetNPC(scarabSpawned + text);
							if (nPC == null)
							{
								Debug.LogError("scarabData Null for scarab => " + scarabSpawned + text);
							}
							else
							{
								CreateNPC(nPC, "", j);
								yield return Globals.Instance.WaitForSeconds(0.5f);
								if (scarabType != 3)
								{
									TeamNPC[j].SetAura(null, Globals.Instance.GetAuraCurseData("luckyscarab"), 1);
								}
								TeamNPC[j].SetAura(null, Globals.Instance.GetAuraCurseData("sight"), 3);
								switch (scarabType)
								{
								case 1:
									if (AtOManager.Instance.GetNgPlus() == 0)
									{
										if (AtOManager.Instance.GetTownTier() <= 1)
										{
											TeamNPC[j].SetAura(null, Globals.Instance.GetAuraCurseData("zeal"), 5);
										}
										else
										{
											TeamNPC[j].SetAura(null, Globals.Instance.GetAuraCurseData("zeal"), 7);
										}
									}
									else if (AtOManager.Instance.GetTownTier() <= 1)
									{
										TeamNPC[j].SetAura(null, Globals.Instance.GetAuraCurseData("zeal"), 6);
									}
									else
									{
										TeamNPC[j].SetAura(null, Globals.Instance.GetAuraCurseData("zeal"), 7);
									}
									break;
								case 2:
									if (AtOManager.Instance.GetNgPlus() == 0)
									{
										if (AtOManager.Instance.GetTownTier() <= 1)
										{
											TeamNPC[j].SetAura(null, Globals.Instance.GetAuraCurseData("evasion"), 5);
											TeamNPC[j].SetAura(null, Globals.Instance.GetAuraCurseData("buffer"), 5);
										}
										else
										{
											TeamNPC[j].SetAura(null, Globals.Instance.GetAuraCurseData("evasion"), 7);
											TeamNPC[j].SetAura(null, Globals.Instance.GetAuraCurseData("buffer"), 7);
										}
									}
									else if (AtOManager.Instance.GetTownTier() <= 1)
									{
										TeamNPC[j].SetAura(null, Globals.Instance.GetAuraCurseData("evasion"), 6);
										TeamNPC[j].SetAura(null, Globals.Instance.GetAuraCurseData("buffer"), 6);
									}
									else
									{
										TeamNPC[j].SetAura(null, Globals.Instance.GetAuraCurseData("evasion"), 7);
										TeamNPC[j].SetAura(null, Globals.Instance.GetAuraCurseData("buffer"), 7);
									}
									break;
								}
								GameManager.Instance.PlayLibraryAudio("glitter", 0.25f);
								yield return Globals.Instance.WaitForSeconds(0.5f);
							}
						}
					}
				}
			}
			currentRound++;
			CreateLogEntry(_initial: true, "round:" + currentRound, "", null, null, null, null, currentRound, Enums.EventActivation.CorruptionBeginRound);
			activatedTraitsRound.Clear();
			if (combatData != null && (MadnessManager.Instance.IsMadnessTraitActive("impedingdoom") || AtOManager.Instance.IsChallengeTraitActive("impedingdoom")))
			{
				ThermometerTierData thermometerTierData = combatData.ThermometerTierData;
				if (thermometerTierData != null)
				{
					for (int l = 0; l < thermometerTierData.RoundThermometer.Length; l++)
					{
						if (thermometerTierData.RoundThermometer[l] == null || currentRound < thermometerTierData.RoundThermometer[l].Round)
						{
							continue;
						}
						if (thermometerTierData.RoundThermometer[l].Round == currentRound)
						{
							ThermometerData thermometerData = thermometerTierData.RoundThermometer[l].ThermometerData;
							if (Globals.Instance.ShowDebug)
							{
								Functions.DebugLogGD(thermometerData.ThermometerId + "<------------");
							}
							if (!(thermometerData != null) || !(thermometerData.ThermometerId.ToLower() == "underwhelming"))
							{
								break;
							}
							for (int m = 0; m < TeamHero.Length; m++)
							{
								if (TeamHero[m] != null && TeamHero[m].HeroData != null)
								{
									if (GameManager.Instance.IsObeliskChallenge())
									{
										TeamHero[m].SetAura(null, Globals.Instance.GetAuraCurseData("doom"), 3);
									}
									else
									{
										TeamHero[m].SetAura(null, Globals.Instance.GetAuraCurseData("doom"), 2);
									}
								}
							}
							break;
						}
						if (thermometerTierData.RoundThermometer[l].Round > currentRound)
						{
							break;
						}
					}
				}
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[CORRUPTIONBEGINROUND]");
			}
			if (corruptionItem != null && corruptionItem.Activation == Enums.EventActivation.CorruptionBeginRound)
			{
				CreateLogEntry(_initial: true, corruptionItem.Id + currentRound, corruptionItem.Id, null, null, null, null, currentRound, Enums.EventActivation.CorruptionBeginRound);
				if (GameManager.Instance.IsMultiplayer())
				{
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("**************************", "net");
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("WaitingSyncro CorruptionBeginRoundPre", "net");
					}
					if (NetworkManager.Instance.IsMaster())
					{
						if (coroutineSyncBeginRound != null)
						{
							StopCoroutine(coroutineSyncBeginRound);
						}
						coroutineSyncBeginRound = StartCoroutine(ReloadCombatCo("CorruptionBeginRoundPre"));
						while (!NetworkManager.Instance.AllPlayersReady("CorruptionBeginRoundPre"))
						{
							yield return Globals.Instance.WaitForSeconds(0.01f);
						}
						if (coroutineSyncBeginRound != null)
						{
							StopCoroutine(coroutineSyncBeginRound);
						}
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("Game ready, Everybody checked CorruptionBeginRoundPre", "net");
						}
						SetRandomIndex(randomIndex);
						NetworkManager.Instance.PlayersNetworkContinue("CorruptionBeginRoundPre", randomIndex.ToString());
					}
					else
					{
						NetworkManager.Instance.SetWaitingSyncro("CorruptionBeginRoundPre", status: true);
						NetworkManager.Instance.SetStatusReady("CorruptionBeginRoundPre");
						while (NetworkManager.Instance.WaitingSyncro["CorruptionBeginRoundPre"])
						{
							yield return Globals.Instance.WaitForSeconds(0.01f);
						}
						if (NetworkManager.Instance.netAuxValue != "")
						{
							SetRandomIndex(int.Parse(NetworkManager.Instance.netAuxValue));
						}
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("CorruptionBeginRoundPre, we can continue!", "net");
						}
					}
				}
				Enums.ItemTarget itemTarget = corruptionItem.ItemTarget;
				if (itemTarget == Enums.ItemTarget.AllHero || itemTarget == Enums.ItemTarget.RandomHero || itemTarget == Enums.ItemTarget.Self)
				{
					itemTarget = corruptionItem.ItemTarget;
					if (itemTarget == Enums.ItemTarget.AllHero || itemTarget == Enums.ItemTarget.Self)
					{
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("corr0");
						}
						for (int n = 0; n < 4; n++)
						{
							if (TeamHero[n] != null && TeamHero[n].Alive)
							{
								TeamHero[n].SetEvent(Enums.EventActivation.CorruptionBeginRound);
							}
						}
					}
					else
					{
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("corr1");
						}
						bool flag4 = false;
						while (!flag4)
						{
							int randomIntRange2 = GetRandomIntRange(0, 4);
							if (TeamHero[randomIntRange2] != null && TeamHero[randomIntRange2].Alive)
							{
								TeamHero[randomIntRange2].SetEvent(Enums.EventActivation.CorruptionBeginRound);
								flag4 = true;
							}
						}
					}
				}
				else
				{
					itemTarget = corruptionItem.ItemTarget;
					if (itemTarget == Enums.ItemTarget.AllEnemy || itemTarget == Enums.ItemTarget.RandomEnemy || itemTarget == Enums.ItemTarget.SelfEnemy)
					{
						itemTarget = corruptionItem.ItemTarget;
						if (itemTarget == Enums.ItemTarget.AllEnemy || itemTarget == Enums.ItemTarget.SelfEnemy)
						{
							if (Globals.Instance.ShowDebug)
							{
								Functions.DebugLogGD("corr2");
							}
							for (int num2 = 0; num2 < 4; num2++)
							{
								if (TeamNPC[num2] != null && TeamNPC[num2].Alive)
								{
									TeamNPC[num2].SetEvent(Enums.EventActivation.CorruptionBeginRound);
								}
							}
						}
						else
						{
							if (Globals.Instance.ShowDebug)
							{
								Functions.DebugLogGD("corr3");
							}
							bool flag5 = false;
							while (!flag5)
							{
								int randomIntRange3 = GetRandomIntRange(0, 4);
								if (TeamNPC[randomIntRange3] != null && TeamNPC[randomIntRange3].Alive)
								{
									TeamNPC[randomIntRange3].SetEvent(Enums.EventActivation.CorruptionBeginRound);
									flag5 = true;
								}
							}
						}
					}
				}
				yield return Globals.Instance.WaitForSeconds(0.2f);
				int j = 0;
				while (generatedCardTimes > 0 && j < 200)
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
					j++;
				}
				yield return Globals.Instance.WaitForSeconds(0.1f);
				yield return Globals.Instance.WaitForSeconds(0.1f);
				int scarabType = 0;
				while (eventList.Count > 0)
				{
					if (GameManager.Instance.GetDeveloperMode() && scarabType % 50 == 0)
					{
						eventListDbg = "";
						for (int num3 = 0; num3 < eventList.Count; num3++)
						{
							eventListDbg = eventListDbg + eventList[num3] + " || ";
						}
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("[CorruptionWAIT] Waiting For Eventlist to clean");
						}
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD(eventListDbg);
						}
					}
					scarabType++;
					if (scarabType > 300)
					{
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("[CorruptionWAIT] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
						}
						ClearEventList();
						break;
					}
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (GameManager.Instance.IsMultiplayer())
				{
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("**************************", "net");
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("WaitingSyncro CorruptionBeginRound", "net");
					}
					if (NetworkManager.Instance.IsMaster())
					{
						if (coroutineSyncBeginRound != null)
						{
							StopCoroutine(coroutineSyncBeginRound);
						}
						coroutineSyncBeginRound = StartCoroutine(ReloadCombatCo("CorruptionBeginRound"));
						while (!NetworkManager.Instance.AllPlayersReady("CorruptionBeginRound"))
						{
							yield return Globals.Instance.WaitForSeconds(0.01f);
						}
						if (coroutineSyncBeginRound != null)
						{
							StopCoroutine(coroutineSyncBeginRound);
						}
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("Game ready, Everybody checked CorruptionBeginRound", "net");
						}
						SetRandomIndex(randomIndex);
						NetworkManager.Instance.PlayersNetworkContinue("CorruptionBeginRound", randomIndex.ToString());
					}
					else
					{
						NetworkManager.Instance.SetWaitingSyncro("CorruptionBeginRound", status: true);
						NetworkManager.Instance.SetStatusReady("CorruptionBeginRound");
						while (NetworkManager.Instance.WaitingSyncro["CorruptionBeginRound"])
						{
							yield return Globals.Instance.WaitForSeconds(0.01f);
						}
						if (NetworkManager.Instance.netAuxValue != "")
						{
							SetRandomIndex(int.Parse(NetworkManager.Instance.netAuxValue));
						}
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("CorruptionBeginRound, we can continue!", "net");
						}
					}
					yield return Globals.Instance.WaitForSeconds(0.1f);
				}
				CreateLogEntry(_initial: false, corruptionItem.Id + currentRound, corruptionItem.Id, null, null, null, null, currentRound, Enums.EventActivation.CorruptionBeginRound);
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[CORRUPTIONBEGINROUND] END", "trace");
			}
			ClearItemExecuteForThisTurn();
			ItemExecuteForThisCombatDuplicate();
			for (int scarabType = 0; scarabType < TeamHero.Length; scarabType++)
			{
				if (TeamHero[scarabType] == null || TeamHero[scarabType].HeroData == null || !TeamHero[scarabType].Alive)
				{
					continue;
				}
				waitExecution = true;
				TeamHero[scarabType].BeginRound();
				eventExaust = 0;
				while (waitExecution)
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
					eventExaust++;
					if (eventExaust > 400)
					{
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("[BeginRound] Waitexecution EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
						}
						waitExecution = false;
					}
				}
				if (TeamHero[scarabType].Alive)
				{
					TeamHero[scarabType].SetEvent(Enums.EventActivation.BeginRound);
				}
			}
			for (int scarabType = 0; scarabType < TeamNPC.Length; scarabType++)
			{
				if (TeamNPC[scarabType] == null || TeamNPC[scarabType].NpcData == null || !TeamNPC[scarabType].Alive)
				{
					continue;
				}
				waitExecution = true;
				TeamNPC[scarabType].BeginRound();
				eventExaust = 0;
				while (waitExecution)
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
					eventExaust++;
					if (eventExaust > 400)
					{
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("[BeginRound] Waitexecution EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
						}
						waitExecution = false;
					}
				}
				if (TeamNPC[scarabType].Alive)
				{
					TeamNPC[scarabType].SetEvent(Enums.EventActivation.BeginRound);
				}
			}
			yield return Globals.Instance.WaitForSeconds(0.1f);
			if (GameManager.Instance.IsMultiplayer())
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("**************************", "net");
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("WaitingSyncro beginround", "net");
				}
				if (NetworkManager.Instance.IsMaster())
				{
					if (coroutineSyncBeginRound != null)
					{
						StopCoroutine(coroutineSyncBeginRound);
					}
					coroutineSyncBeginRound = StartCoroutine(ReloadCombatCo("beginround"));
					while (!NetworkManager.Instance.AllPlayersReady("beginround"))
					{
						yield return Globals.Instance.WaitForSeconds(0.01f);
					}
					if (coroutineSyncBeginRound != null)
					{
						StopCoroutine(coroutineSyncBeginRound);
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("Game ready, Everybody checked beginround", "net");
					}
					SetRandomIndex(randomIndex);
					NetworkManager.Instance.PlayersNetworkContinue("beginround");
				}
				else
				{
					NetworkManager.Instance.SetWaitingSyncro("beginround", status: true);
					NetworkManager.Instance.SetStatusReady("beginround");
					while (NetworkManager.Instance.WaitingSyncro["beginround"])
					{
						yield return Globals.Instance.WaitForSeconds(0.01f);
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("beginround, we can continue!", "net");
					}
				}
			}
		}
		eventExaust = 0;
		while (eventList.Count > 0)
		{
			if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
			{
				eventListDbg = "";
				for (int num4 = 0; num4 < eventList.Count; num4++)
				{
					eventListDbg = eventListDbg + eventList[num4] + " || ";
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[BeginTurn] Waiting For Eventlist to clean");
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(eventListDbg);
				}
			}
			eventExaust++;
			if (eventExaust > 300)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[BeginTurn] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
				}
				ClearEventList();
				break;
			}
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
		int _queueIndex = 0;
		while (ctQueue.Count > 0)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[ItemQueue] Waiting queue -> " + ctQueue.Count + " // " + _queueIndex);
			}
			yield return Globals.Instance.WaitForSeconds(0.1f);
			if (_queueIndex > 50)
			{
				ctQueue.Clear();
			}
		}
		NextTurnContinue2();
	}

	private void NextTurnContinue2()
	{
		StopCoroutines();
		ClearEventList();
		StoreCombatStats();
		StartCoroutine(HideComic());
		StartCoroutine(NextTurnContinue3());
	}

	private IEnumerator NextTurnContinue3()
	{
		yield return Globals.Instance.WaitForSeconds(0.1f);
		if (MatchIsOver)
		{
			yield break;
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			if (NetworkManager.Instance.IsMaster())
			{
				GenerateSyncCodeDict();
			}
			NetworkManager.Instance.ClearSyncro();
		}
		beforeSyncCodeLocked = false;
		castingCardBlocked.Clear();
		activatedTraits.Clear();
		heroDestroyedItemsInThisTurn.Clear();
		ReDrawInitiatives();
		CleanTempTransform();
		yield return Globals.Instance.WaitForSeconds(0.1f);
		int eventExaust = 0;
		while (eventList.Count > 0)
		{
			if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
			{
				eventListDbg = "";
				for (int i = 0; i < eventList.Count; i++)
				{
					eventListDbg = eventListDbg + eventList[i] + " || ";
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[BeginTurn] Waiting For Eventlist to clean");
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(eventListDbg);
				}
			}
			eventExaust++;
			if (eventExaust > 300)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[BeginTurn] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
				}
				ClearEventList();
				break;
			}
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
		gameStatus = "BeginTurn";
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD(gameStatus, "gamestatus");
		}
		waitExecution = true;
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[NEXTTURNCONTINUE] SetActiveCharacter");
		}
		SetActiveCharacter();
		CreateLogEntry(_initial: true, "status:" + logDictionary.Count, "", theHero, theNPC, null, null, currentRound, Enums.EventActivation.EndTurn);
		CreateLogEntry(_initial: true, "", "", theHero, theNPC, null, null, currentRound, Enums.EventActivation.BeginTurn);
		if (theHero != null)
		{
			CreateLogEntry(_initial: true, "", "", theHero, theNPC, null, null, currentRound, Enums.EventActivation.BeginTurnAboutToDealCards, theHero.GetDrawCardsTurnForDisplayInDeck());
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			yield return Globals.Instance.WaitForSeconds(0.1f);
		}
		else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
		{
			yield return Globals.Instance.WaitForSeconds(0.3f);
		}
		else
		{
			yield return Globals.Instance.WaitForSeconds(0.1f);
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[NEXTTURNCONTINUE] END SetActiveCharacter", "trace");
		}
		waitExecution = true;
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[NEXTTURNCONTINUE] ActionsCharacterBeginTurn", "trace");
		}
		isBeginTournPhase = true;
		int _logEntryIndex = logDictionary.Count;
		CreateLogEntry(_initial: true, "begineffects:" + _logEntryIndex, "", theHero, theNPC, null, null, currentRound, Enums.EventActivation.BeginTurn);
		ActionsCharacterBeginTurn();
		eventExaust = 0;
		while (waitExecution)
		{
			yield return Globals.Instance.WaitForSeconds(0.01f);
			eventExaust++;
			if (eventExaust > 400)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[ActionsCharacterBeginTurn] Waitexecution EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
				}
				waitExecution = false;
			}
		}
		waitExecution = true;
		eventExaust = 0;
		ActionsCharacterBeginTurnPerks();
		while (waitExecution)
		{
			yield return Globals.Instance.WaitForSeconds(0.01f);
			eventExaust++;
			if (eventExaust > 400)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[ActionsCharacterBeginTurn] Waitexecution EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
				}
				waitExecution = false;
			}
		}
		eventExaust = 0;
		while (eventList.Count > 0 && ctQueue.Count > 0)
		{
			if (GameManager.Instance.GetDeveloperMode() && eventExaust % 5 == 0)
			{
				eventListDbg = "";
				for (int j = 0; j < eventList.Count; j++)
				{
					eventListDbg = eventListDbg + eventList[j] + " || ";
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[ActionsCharacterBeginTurn] Waiting For Eventlist to clean");
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(eventListDbg);
				}
			}
			eventExaust++;
			if (eventExaust > 60)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[ActionsCharacterBeginTurn] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
				}
				ClearEventList();
				ctQueue.Clear();
				break;
			}
			yield return Globals.Instance.WaitForSeconds(0.1f);
			Debug.Log("[ActionsCharacterBeginTurn] LOOP " + eventList.Count + "//" + ctQueue.Count);
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[NEXTTURNCONTINUE] END ActionsCharacterBeginTurn");
		}
		turnIndex++;
		if (!GameManager.Instance.IsMultiplayer() && turnIndex > 1 && heroActive > -1 && !GameManager.Instance.TutorialWatched("combatSpeed"))
		{
			waitingTutorial = true;
			yield return Globals.Instance.WaitForSeconds(0.4f);
			GameManager.Instance.ShowTutorialPopup("combatSpeed", CharOrder[0].initiativePortrait.transform.Find("Speed").transform.position, Vector3.zero);
			characterWindow.Hide();
			while (waitingTutorial)
			{
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			SetWatchingTutorial(state: false);
		}
		eventExaust = 0;
		while (waitExecution)
		{
			yield return Globals.Instance.WaitForSeconds(0.01f);
			eventExaust++;
			if (eventExaust > 400)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[CombatBeforeCharMoves] Waitexecution EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
				}
				waitExecution = false;
			}
		}
		yield return Globals.Instance.WaitForSeconds(0.1f);
		eventExaust = 0;
		while (eventList.Count > 0)
		{
			if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
			{
				eventListDbg = "";
				for (int k = 0; k < eventList.Count; k++)
				{
					eventListDbg = eventListDbg + eventList[k] + " || ";
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[CombatBeforeCharMoves] Waiting For Eventlist to clean");
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(eventListDbg);
				}
			}
			eventExaust++;
			if (eventExaust > 300)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[CombatBeforeCharMoves] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
				}
				ClearEventList();
				break;
			}
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
		CreateLogEntry(_initial: false, "begineffects:" + _logEntryIndex, "", theHero, theNPC, null, null, currentRound, Enums.EventActivation.BeginTurn);
		SetGameBusy(state: false);
		activationItemsAtBeginTurnList = new List<string>();
		if (heroActive != -1)
		{
			if (theHero == null || !theHero.Alive)
			{
				yield break;
			}
			if (GameManager.Instance.IsMultiplayer())
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("**************************", "net");
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("WaitingSyncro beginturn", "net");
				}
				yield return Globals.Instance.WaitForSeconds(0.1f);
				if (NetworkManager.Instance.IsMaster())
				{
					if (coroutineSync != null)
					{
						StopCoroutine(coroutineSync);
					}
					coroutineSync = StartCoroutine(ReloadCombatCo("beginturn"));
					while (!NetworkManager.Instance.AllPlayersReady("beginturn"))
					{
						yield return Globals.Instance.WaitForSeconds(0.01f);
					}
					if (coroutineSync != null)
					{
						StopCoroutine(coroutineSync);
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("Game ready, Everybody checked beginturn", "net");
					}
					SetRandomIndex(randomIndex);
					NetworkManager.Instance.PlayersNetworkContinue("beginturn", randomIndex.ToString());
				}
				else
				{
					NetworkManager.Instance.SetWaitingSyncro("beginturn", status: true);
					NetworkManager.Instance.SetStatusReady("beginturn");
					while (NetworkManager.Instance.WaitingSyncro["beginturn"])
					{
						yield return Globals.Instance.WaitForSeconds(0.01f);
					}
					if (NetworkManager.Instance.netAuxValue != "")
					{
						SetRandomIndex(int.Parse(NetworkManager.Instance.netAuxValue));
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("beginturn, we can continue!", "net");
					}
				}
			}
			ResetItemTimeout();
			if (!theHero.HasEffectSkipsTurn())
			{
				StartCoroutine(MoveItemsOut(state: false));
			}
			yield return Globals.Instance.WaitForSeconds(0.2f);
			CleanTempTransform();
			theHero.SetEvent(Enums.EventActivation.BeginTurn);
			if (!theHero.HasEffectSkipsTurn() && theHero.HaveItemToActivate(Enums.EventActivation.BeginTurn, _checkForItems: true))
			{
				ResetAutoEndCount();
				bool exit = false;
				int validTimes = 0;
				eventExaust = 0;
				while (!exit)
				{
					yield return Globals.Instance.WaitForSeconds(0.2f);
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("[BeginTurn] Waiting exit.... " + gameStatus + "|" + NumChildsInTemporal() + "|" + waitingKill + "|" + waitingDeathScreen + "|" + castingCardListMP.Count + "|" + validTimes + "|" + eventExaust);
					}
					if (gameStatus != "CastCardEnd" || NumChildsInTemporal() > 0 || waitingKill || waitingDeathScreen || castingCardListMP.Count > 0)
					{
						validTimes = 0;
						if (waitingKill || waitingDeathScreen)
						{
							eventExaust = 0;
						}
						eventExaust++;
						if (eventExaust % 4 == 0 && Globals.Instance.ShowDebug)
						{
							Debug.Log(eventExaust + "||" + gameStatus + "||" + NumChildsInTemporal() + "||" + waitingKill + "||" + waitingDeathScreen);
						}
						if (eventExaust > 40)
						{
							if (Globals.Instance.ShowDebug)
							{
								Functions.DebugLogGD("begin turn items activation break by exhaust");
							}
							break;
						}
					}
					else
					{
						eventExaust = 0;
						validTimes++;
						if (!GameManager.Instance.IsMultiplayer())
						{
							if (validTimes > 3)
							{
								if (Globals.Instance.ShowDebug)
								{
									Functions.DebugLogGD("validTimes > 3");
								}
								exit = true;
							}
						}
						else if (validTimes > 3)
						{
							if (Globals.Instance.ShowDebug)
							{
								Functions.DebugLogGD("validTimes > 3");
							}
							exit = true;
						}
					}
					if (gameStatus == "FinishCombat")
					{
						yield break;
					}
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[BeginTurn] EXIT");
				}
			}
			while (waitingDeathScreen)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[BeginTurn] waitingDeathScreen");
				}
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
			while (waitingKill)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[BeginTurn] Waitingforkill");
				}
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
			if (!theHero.Alive)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[BeginTurn] HEro is dead, wait for resurrection item");
				}
				yield return Globals.Instance.WaitForSeconds(0.3f);
				if (!theHero.Alive)
				{
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("[BeginTurn] break because Hero died");
					}
					yield break;
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[BeginTurn] Hero have resurrected, continue");
				}
			}
			if (MatchIsOver)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[BeginTurn] Broken by finish game");
				}
				yield break;
			}
			ReDrawInitiatives();
			combatTarget.Refresh();
			SortCharacterSprites(toFront: true, heroActive);
			cursorArrow.gameObject.SetActive(value: true);
			eventExaust = 0;
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[BeginTurn] gamestatus->" + gameStatus, "trace");
			}
			while (gameStatus != "BeginTurn" && gameStatus != "CastCardEnd")
			{
				eventExaust++;
				if (eventExaust > 300)
				{
					break;
				}
				if (eventExaust % 10 == 0 && Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[BeginTurn] Waiting loop gamestatus->" + gameStatus, "trace");
				}
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			CleanTempTransform();
			if (!theHero.HasEffectSkipsTurn() && theHero.GetDrawCardsTurn() > 0)
			{
				while (eventList.Count > 0)
				{
					yield return Globals.Instance.WaitForSeconds(0.05f);
				}
				while (waitingKill)
				{
					yield return Globals.Instance.WaitForSeconds(0.05f);
				}
				while (waitingDeathScreen)
				{
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("DealNewCard -  waitingDeathScreen", "trace");
					}
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (!theHero.Alive)
				{
					EndTurn();
					yield break;
				}
				gameStatus = "BeginTurnHero";
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(gameStatus, "gamestatus");
				}
				MovingDeckPile = true;
				DrawDeckPile(CountHeroDeck() + 1);
				StartCoroutine(MoveDecksOut(state: false));
				while (MovingDeckPile)
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				SetCardsWaitingForReset(1);
				ShowHandMask(state: true);
				StartCoroutine(DealCards());
				while (cardsWaitingForReset > 0)
				{
					yield return Globals.Instance.WaitForSeconds(0.1f);
				}
				eventExaust = 0;
				while (eventList.Count > 0)
				{
					if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
					{
						eventListDbg = "";
						for (int l = 0; l < eventList.Count; l++)
						{
							eventListDbg = eventListDbg + eventList[l] + " || ";
						}
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("[DealCards] Waiting For Eventlist to clean", "general");
						}
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD(eventListDbg, "general");
						}
					}
					eventExaust++;
					if (eventExaust > 300)
					{
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("[DealCards] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
						}
						ClearEventList();
						break;
					}
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				heroTurn = true;
				yield break;
			}
			if (theHero.GetDrawCardsTurn() <= 0)
			{
				newTurnScript.CantDraw(theHero.SourceName);
			}
			else
			{
				newTurnScript.PassTurn(theHero.SourceName);
			}
			yield return Globals.Instance.WaitForSeconds(1.2f);
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[HERO SKIP TURN]", "trace");
			}
			if (GameManager.Instance.IsMultiplayer())
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("**************************", "net");
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("WaitingSyncro endturnbyskip", "net");
				}
				yield return Globals.Instance.WaitForSeconds(0.1f);
				if (NetworkManager.Instance.IsMaster())
				{
					if (coroutineSync != null)
					{
						StopCoroutine(coroutineSync);
					}
					coroutineSync = StartCoroutine(ReloadCombatCo("endturnbyskip"));
					while (!NetworkManager.Instance.AllPlayersReady("endturnbyskip"))
					{
						yield return Globals.Instance.WaitForSeconds(0.01f);
					}
					if (coroutineSync != null)
					{
						StopCoroutine(coroutineSync);
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("Game ready, Everybody checked endturnbyskip", "net");
					}
					NetworkManager.Instance.PlayersNetworkContinue("endturnbyskip");
				}
				else
				{
					NetworkManager.Instance.SetWaitingSyncro("endturnbyskip", status: true);
					NetworkManager.Instance.SetStatusReady("endturnbyskip");
					while (NetworkManager.Instance.WaitingSyncro["endturnbyskip"])
					{
						yield return Globals.Instance.WaitForSeconds(0.01f);
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("endturnbyskip, we can continue!", "net");
					}
				}
				if (IsYourTurn())
				{
					EndTurn();
				}
			}
			else
			{
				EndTurn();
			}
		}
		else
		{
			if (theNPC == null || !theNPC.Alive)
			{
				yield break;
			}
			theNPC.SetEvent(Enums.EventActivation.BeginTurn);
			theNPC.SetEvent(Enums.EventActivation.EnemyBeginTurnDelay);
			yield return Globals.Instance.WaitForSeconds(0.1f);
			eventExaust = 0;
			bool exit = false;
			while (!exit)
			{
				exit = true;
				while (eventList.Count > 0)
				{
					exit = false;
					if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
					{
						eventListDbg = "";
						for (int m = 0; m < eventList.Count; m++)
						{
							eventListDbg = eventListDbg + eventList[m] + " || ";
						}
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("[NextTurnContinueAfterStopCoroutines] Waiting For Eventlist to clean");
						}
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD(eventListDbg);
						}
					}
					eventExaust++;
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[NextTurnContinueAfterStopCoroutines] cleanExecution -> " + exit);
				}
				if (waitingKill)
				{
					exit = false;
					while (waitingKill)
					{
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("[NextTurnContinueAfterStopCoroutines] Waitingforkill");
						}
						yield return Globals.Instance.WaitForSeconds(0.1f);
					}
				}
				if (!exit)
				{
					yield return Globals.Instance.WaitForSeconds(0.5f);
					if (eventList.Count == 0)
					{
						exit = true;
					}
					else
					{
						eventExaust = 0;
					}
				}
			}
			ReDrawInitiatives();
			SortCharacterSprites(toFront: true, -1, npcActive);
			isBeginTournPhase = false;
			if (!theNPC.HasEffectSkipsTurn())
			{
				CastNPC();
				yield break;
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[NPC SKIP TURN]", "trace");
			}
			if (GameManager.Instance.IsMultiplayer())
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("**************************", "net");
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("WaitingSyncro endturnbyskipnpc", "net");
				}
				yield return Globals.Instance.WaitForSeconds(0.1f);
				if (NetworkManager.Instance.IsMaster())
				{
					if (coroutineSync != null)
					{
						StopCoroutine(coroutineSync);
					}
					coroutineSync = StartCoroutine(ReloadCombatCo("endturnbyskipnpc"));
					while (!NetworkManager.Instance.AllPlayersReady("endturnbyskipnpc"))
					{
						yield return Globals.Instance.WaitForSeconds(0.01f);
					}
					if (coroutineSync != null)
					{
						StopCoroutine(coroutineSync);
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("Game ready, Everybody checked endturnbyskipnpc", "net");
					}
					NetworkManager.Instance.PlayersNetworkContinue("endturnbyskipnpc");
				}
				else
				{
					NetworkManager.Instance.SetWaitingSyncro("endturnbyskipnpc", status: true);
					NetworkManager.Instance.SetStatusReady("endturnbyskipnpc");
					while (NetworkManager.Instance.WaitingSyncro["endturnbyskipnpc"])
					{
						yield return Globals.Instance.WaitForSeconds(0.01f);
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("endturnbyskipnpc, we can continue!", "net");
					}
				}
				EndTurn();
			}
			else
			{
				EndTurn();
			}
		}
	}

	private void BeginTurnHero()
	{
		gameStatus = "HeroTurn";
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD(gameStatus, "gamestatus");
		}
		castingCardBlocked.Clear();
		ClearEventList();
		StartCoroutine(BeginTurnHeroCo());
	}

	private IEnumerator BeginTurnHeroCo()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("BeginTurnHeroCo");
		}
		for (int num = cardItemTable.Count - 1; num >= 0; num--)
		{
			if (num <= cardItemTable.Count - 1 && cardItemTable[num] != null)
			{
				cardItemTable[num].discard = false;
				cardItemTable[num].active = true;
			}
		}
		gameStatus = "";
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD(gameStatus, "gamestatus");
		}
		SetGameBusy(state: false);
		RepositionCards();
		if (GameManager.Instance.IsMultiplayer())
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("**************************", "net");
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("WaitingSyncro beginturnheroco", "net");
			}
			yield return Globals.Instance.WaitForSeconds(0.1f);
			if (NetworkManager.Instance.IsMaster())
			{
				if (coroutineSyncBeginTurnHero != null)
				{
					StopCoroutine(coroutineSyncBeginTurnHero);
				}
				coroutineSyncBeginTurnHero = StartCoroutine(ReloadCombatCo("beginturnheroco"));
				while (!NetworkManager.Instance.AllPlayersReady("beginturnheroco"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (coroutineSyncBeginTurnHero != null)
				{
					StopCoroutine(coroutineSyncBeginTurnHero);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked beginturnheroco", "net");
				}
				NetworkManager.Instance.PlayersNetworkContinue("beginturnheroco");
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("beginturnheroco", status: true);
				NetworkManager.Instance.SetStatusReady("beginturnheroco");
				while (NetworkManager.Instance.WaitingSyncro["beginturnheroco"])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("beginturnheroco, we can continue!", "net");
				}
			}
		}
		if (!GameManager.Instance.IsMultiplayer() && (!GameManager.Instance.TutorialWatched("firstTurnEnergy") || !GameManager.Instance.TutorialWatched("cardTarget")))
		{
			waitingTutorial = true;
			CardItem cardTutorial = cardItemTable.Last();
			for (int num2 = cardItemTable.Count() - 1; num2 >= 0; num2--)
			{
				if (cardItemTable[num2].CardData.Damage > 0)
				{
					cardTutorial = cardItemTable[num2];
					break;
				}
			}
			_ = cardTutorial.transform.localPosition;
			_ = cardTutorial.transform.rotation;
			cardTutorial.DisableCollider();
			yield return Globals.Instance.WaitForSeconds(0.3f);
			cardTutorial.SetDestinationScaleRotation(-cardTutorial.transform.parent.transform.position, 1.4f, Quaternion.Euler(0f, 0f, 0f));
			yield return Globals.Instance.WaitForSeconds(0.6f);
			if (!GameManager.Instance.TutorialWatched("firstTurnEnergy"))
			{
				GameManager.Instance.ShowTutorialPopup("firstTurnEnergy", theHero.HeroItem.transform.Find("Energy/Energy_Text").transform.position, cardTutorial.transform.Find("CardGO/Energy").transform.position);
				characterWindow.Hide();
				while (waitingTutorial)
				{
					yield return Globals.Instance.WaitForSeconds(0.1f);
				}
			}
			if (!GameManager.Instance.TutorialWatched("cardTarget"))
			{
				waitingTutorial = true;
				yield return Globals.Instance.WaitForSeconds(0.2f);
				GameManager.Instance.ShowTutorialPopup("cardTarget", cardTutorial.transform.Find("CardGO/CardTargetText").transform.position, Vector3.zero);
				characterWindow.Hide();
				while (waitingTutorial)
				{
					yield return Globals.Instance.WaitForSeconds(0.1f);
				}
			}
			yield return Globals.Instance.WaitForSeconds(0.2f);
			if (cardTutorial != null)
			{
				cardTutorial.EnableCollider();
				RepositionCards();
			}
			if (GameManager.Instance.IsMultiplayer())
			{
				SetWatchingTutorial(state: false);
			}
			yield return Globals.Instance.WaitForSeconds(0.4f);
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("BeginTurnCardsDealt", "general");
		}
		yield return Globals.Instance.WaitForSeconds(0.02f);
		theHero.SetEvent(Enums.EventActivation.BeginTurnCardsDealt);
		if (theHero.HaveItemToActivate(Enums.EventActivation.BeginTurnCardsDealt))
		{
			yield return Globals.Instance.WaitForSeconds(0.1f);
		}
		isBeginTournPhase = false;
	}

	private void SetWatchingTutorial(bool state)
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			photonView.RPC("NET_WatchingTutorial", RpcTarget.MasterClient, NetworkManager.Instance.GetPlayerNick(), state);
		}
	}

	[PunRPC]
	private void NET_WatchingTutorial(string nick, bool state)
	{
		if (state)
		{
			if (!playersWatchingTutorial.Contains(nick))
			{
				playersWatchingTutorial.Add(nick);
			}
		}
		else if (playersWatchingTutorial.Contains(nick))
		{
			playersWatchingTutorial.Remove(nick);
		}
		if (playersWatchingTutorial.Count == 0)
		{
			photonView.RPC("NET_FinishedTutorial", RpcTarget.All, false);
		}
		else
		{
			photonView.RPC("NET_FinishedTutorial", RpcTarget.All, true);
		}
	}

	[PunRPC]
	private void NET_FinishedTutorial(bool state)
	{
		multiplayerWatchingTutorial = state;
	}

	private void InitThermoPieces(CombatData _combatData)
	{
		for (int i = 0; i < roundPieces.Length; i++)
		{
			roundPieces[i].SetThermometerTierData(_combatData.ThermometerTierData);
		}
		if (_combatData.ThermometerTierData == null)
		{
			roundThermoSprite.transform.GetComponent<PolygonCollider2D>().enabled = false;
		}
	}

	private void SetRoundText()
	{
		roundTransform.gameObject.SetActive(value: true);
		if (currentRound > 0)
		{
			roundTM.text = string.Format(Texts.Instance.GetText("roundNumber"), currentRound).ToUpper();
		}
		else
		{
			roundTM.text = "";
		}
		if (currentRound > 0)
		{
			initiativeRoundGO.transform.GetChild(0).transform.GetComponent<TMP_Text>().text = string.Format(Texts.Instance.GetText("roundNumber"), currentRound + 1).ToUpper();
		}
		else
		{
			initiativeRoundGO.transform.GetChild(0).transform.GetComponent<TMP_Text>().text = "";
		}
		for (int i = 0; i < roundPieces.Length; i++)
		{
			roundPieces[i].Init(currentRound);
		}
		if (combatData != null && combatData.ThermometerTierData != null)
		{
			roundThermoSprite.sprite = roundThermoSprites[2];
		}
		else
		{
			roundThermoSprite.sprite = roundThermoSpriteNull;
		}
		if (roundTM.text != "")
		{
			if (!roundTransform.gameObject.activeSelf)
			{
				roundTransform.gameObject.SetActive(value: true);
			}
		}
		else
		{
			roundTransform.gameObject.SetActive(value: false);
		}
	}

	public void AdjustRoundForThermoDisplay(int piece, int roundForDisplay, Color colorForDisplay)
	{
		if (roundForDisplay == 0)
		{
			roundTM.text = string.Format(Texts.Instance.GetText("roundNumber"), currentRound).ToUpper();
		}
		else
		{
			roundTM.text = string.Format(Texts.Instance.GetText("roundNumber"), roundForDisplay).ToUpper();
		}
		roundTM.color = colorForDisplay;
		roundThermoSprite.sprite = roundThermoSprites[piece];
	}

	public void SetWaitExecution(bool status)
	{
		waitExecution = status;
	}

	private void ActionsCharacterBeginTurn()
	{
		if (theHero != null)
		{
			theHero.BeginTurn();
		}
		else if (theNPC != null)
		{
			theNPC.BeginTurn();
		}
		else
		{
			waitExecution = false;
		}
	}

	private void ActionsCharacterBeginTurnPerks()
	{
		if (theHero != null)
		{
			theHero.BeginTurnPerks();
		}
		else if (theNPC != null)
		{
			theNPC.BeginTurnPerks();
		}
		else
		{
			waitExecution = false;
		}
	}

	private void ActionsCharacterEndTurn()
	{
		if (theHero != null && theHero.Alive && theHero.HeroItem != null)
		{
			theHero.EndTurn();
		}
		else if (theNPC != null && theNPC.Alive && theNPC.NPCItem != null)
		{
			theNPC.EndTurn();
		}
		else
		{
			waitExecution = false;
		}
	}

	private void ActionsCharacterBeginRound()
	{
		for (int i = 0; i < TeamHero.Length; i++)
		{
			if (TeamHero[i] != null && !(TeamHero[i].HeroData == null) && TeamHero[i].Alive)
			{
				TeamHero[i].BeginRound();
			}
		}
		for (int j = 0; j < TeamNPC.Length; j++)
		{
			if (TeamNPC[j] != null && !(TeamNPC[j].NpcData == null) && TeamNPC[j].Alive)
			{
				TeamNPC[j].BeginRound();
			}
		}
	}

	private void ActionsCharacterEndRound()
	{
		for (int i = 0; i < TeamHero.Length; i++)
		{
			if (TeamHero[i] != null && !(TeamHero[i].HeroData == null) && TeamHero[i].Alive)
			{
				TeamHero[i].EndRound();
			}
		}
		for (int j = 0; j < TeamNPC.Length; j++)
		{
			if (TeamNPC[j] != null && !(TeamNPC[j].NpcData == null) && TeamNPC[j].Alive)
			{
				TeamNPC[j].EndRound();
			}
		}
	}

	public void SetInitiatives()
	{
		if (CharOrder != null)
		{
			for (int num = CharOrder.Count - 1; num >= 0; num--)
			{
				if (CharOrder[num] != null && CharOrder[num].initiativePortrait != null)
				{
					UnityEngine.Object.Destroy(CharOrder[num].initiativePortrait.gameObject);
				}
				CharOrder.RemoveAt(num);
			}
		}
		CharOrder = new List<CharacterForOrder>();
		for (int i = 0; i < TeamHero.Length; i++)
		{
			if (TeamHero[i] != null && !(TeamHero[i].HeroData == null))
			{
				Hero hero = TeamHero[i];
				if (hero.Alive)
				{
					CharacterForOrder characterForOrder = new CharacterForOrder();
					characterForOrder.index = i;
					characterForOrder.id = hero.Id;
					characterForOrder.hero = hero;
					characterForOrder.npc = null;
					characterForOrder.speed = hero.GetSpeed();
					characterForOrder.heroItem = hero.HeroItem;
					characterForOrder.speedForOrder = (float)(AuxiliarSumForInitiative(characterForOrder) + characterForOrder.speed[0]) + 0.5f + 0.01f * (float)(10 - i);
					CharOrder.Add(characterForOrder);
				}
			}
		}
		for (int j = 0; j < 4; j++)
		{
			if (TeamNPC[j] != null && !(TeamNPC[j].NpcData == null))
			{
				NPC nPC = TeamNPC[j];
				if (nPC.Alive)
				{
					CharacterForOrder characterForOrder2 = new CharacterForOrder();
					characterForOrder2.index = j;
					characterForOrder2.id = nPC.Id;
					characterForOrder2.hero = null;
					characterForOrder2.npc = nPC;
					characterForOrder2.speed = nPC.GetSpeed();
					characterForOrder2.npcItem = nPC.NPCItem;
					characterForOrder2.speedForOrder = (float)(AuxiliarSumForInitiative(characterForOrder2) + characterForOrder2.speed[0]) + 0.01f * (float)(10 - j);
					CharOrder.Add(characterForOrder2);
				}
			}
		}
		CharOrder = CharOrder.OrderByDescending((CharacterForOrder w) => w.speedForOrder).ToList();
		DrawInitiatives();
	}

	private void ReDoInitiatives(object objKilled)
	{
		for (int num = CharOrder.Count - 1; num >= 0; num--)
		{
			if (CharOrder[num].hero == objKilled || CharOrder[num].npc == objKilled)
			{
				UnityEngine.Object.Destroy(CharOrder[num].initiativePortrait.gameObject);
				CharOrder.RemoveAt(num);
			}
		}
		ReDrawInitiatives();
	}

	private void DrawInitiatives()
	{
		initiativeRoundGO = UnityEngine.Object.Instantiate(initiativeRoundPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, GO_Initiative.transform);
		for (int i = 0; i < CharOrder.Count; i++)
		{
			InitiativePortrait component = UnityEngine.Object.Instantiate(initiativePrefab, new Vector3(0f, 0f, 1f), Quaternion.identity, GO_Initiative.transform).GetComponent<InitiativePortrait>();
			CharOrder[i].initiativePortrait = component;
			if (CharOrder[i].hero != null)
			{
				component.SetHero(CharOrder[i].hero, CharOrder[i].heroItem);
				component.SetSpeed(CharOrder[i].speed);
			}
			else if (CharOrder[i].npc != null)
			{
				component.SetNPC(CharOrder[i].npc.NpcData, CharOrder[i].npcItem);
				component.SetSpeed(CharOrder[i].speed);
			}
			component.Init(i);
		}
		DrawInitiativesWidth();
	}

	public void ReDrawInitiatives()
	{
		if (CharOrder == null)
		{
			return;
		}
		if (initiativeRoundGO == null)
		{
			initiativeRoundGO = UnityEngine.Object.Instantiate(initiativeRoundPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, GO_Initiative.transform);
		}
		initiativeRoundGO.SetActive(value: false);
		SetRoundText();
		for (int i = 0; i < CharOrder.Count; i++)
		{
			CharacterForOrder characterForOrder = CharOrder[i];
			if (characterForOrder.hero != null)
			{
				characterForOrder.speed = characterForOrder.hero.GetSpeed();
				characterForOrder.speedForOrder = (float)(AuxiliarSumForInitiative(characterForOrder) + characterForOrder.speed[0]) + 0.5f + 0.01f * (float)(20 - characterForOrder.hero.Position);
			}
			else
			{
				characterForOrder.speed = characterForOrder.npc.GetSpeed();
				characterForOrder.speedForOrder = (float)(AuxiliarSumForInitiative(characterForOrder) + characterForOrder.speed[0]) + 0.01f * (float)(20 - characterForOrder.npc.Position);
			}
			characterForOrder.initiativePortrait.SetSpeed(characterForOrder.speed);
		}
		CharOrder = CharOrder.OrderByDescending((CharacterForOrder w) => w.speedForOrder).ToList();
		bool flag = false;
		for (int num = 0; num < CharOrder.Count; num++)
		{
			CharacterForOrder characterForOrder2 = CharOrder[num];
			if (!flag && ((characterForOrder2.hero != null && characterForOrder2.hero.RoundMoved >= currentRound) || (characterForOrder2.npc != null && characterForOrder2.npc.RoundMoved >= currentRound)))
			{
				initiativeRoundGO.transform.parent = characterForOrder2.initiativePortrait.transform;
				initiativeRoundGO.transform.localPosition = new Vector3(-0.48f, 0f, 0f);
				flag = true;
			}
			characterForOrder2.initiativePortrait.RedoPos(num, flag);
			if (num == 0)
			{
				characterForOrder2.initiativePortrait.SetPlaying(state: true);
			}
			else
			{
				characterForOrder2.initiativePortrait.SetPlaying(state: false);
			}
		}
		if (flag && currentRound > 0)
		{
			initiativeRoundGO.SetActive(value: true);
		}
		DrawInitiativesWidth();
	}

	private void DrawInitiativesWidth()
	{
		float num = 0.48f * (float)CharOrder.Count + 0.24f * (float)(CharOrder.Count - 1);
		float y = 4.8f;
		if (initiativeRoundGO.activeSelf)
		{
			num += 0.35999998f;
		}
		float x = -1f * (num * 0.5f) + 0.24f;
		GO_Initiative.transform.position = new Vector3(x, y, 0f);
	}

	private int AuxiliarSumForInitiative(CharacterForOrder theChar)
	{
		int result = 0;
		if (theChar.hero != null)
		{
			if (theChar.hero.RoundMoved == currentRound)
			{
				result = -100000;
			}
			else if (theChar.index == heroActive)
			{
				result = 100000;
			}
		}
		else if (theChar.npc.RoundMoved == currentRound)
		{
			result = -100000;
		}
		else if (theChar.index == npcActive)
		{
			result = 100000;
		}
		return result;
	}

	public void PortraitHighlight(bool state, CharacterItem characterItem)
	{
		if (CharOrder == null)
		{
			return;
		}
		for (int i = 0; i < CharOrder.Count; i++)
		{
			if (CharOrder[i].initiativePortrait.heroItem == characterItem)
			{
				CharOrder[i].initiativePortrait.SetActive(state);
			}
			if (CharOrder[i].initiativePortrait.npcItem == characterItem)
			{
				CharOrder[i].initiativePortrait.SetActive(state);
			}
		}
	}

	private void SetActiveCharacter()
	{
		CharacterForOrder characterForOrder = CharOrder[0];
		if (characterForOrder.hero != null)
		{
			npcActive = -1;
			theNPC = null;
			heroActive = characterForOrder.index;
			theHero = TeamHero[heroActive];
			if (theHero != null && theHero.Alive)
			{
				theHero.HeroItem.ActivateMark(state: true);
				if (HeroHand[heroActive] == null)
				{
					HeroHand[heroActive] = new List<string>();
				}
				else
				{
					HeroHand[heroActive].Clear();
				}
				if (cardItemTable == null)
				{
					cardItemTable = new List<CardItem>();
				}
				else
				{
					cardItemTable.Clear();
				}
				newTurnScript.SetTurn(theHero.SourceName);
				if (GameManager.Instance.IsMultiplayer() && IsYourTurn())
				{
					GameManager.Instance.PlayAudio(AudioManager.Instance.soundCombatIsYourTurn);
				}
				ShowTraitInfo(state: true);
				SetTraitInfoText();
			}
		}
		else
		{
			heroActive = -1;
			theHero = null;
			npcActive = characterForOrder.index;
			theNPC = TeamNPC[npcActive];
			if (theNPC != null && theNPC.Alive)
			{
				theNPC.NPCItem.ActivateMark(state: true);
				newTurnScript.SetTurn(theNPC.SourceName);
			}
		}
	}

	private int GetIndexForChar(Hero _hero)
	{
		for (int i = 0; i < CharOrder.Count; i++)
		{
			if (CharOrder[i].hero == _hero)
			{
				return CharOrder[i].index;
			}
		}
		return -1;
	}

	public int GetIndexInCharOrder(Hero _hero)
	{
		for (int i = 0; i < CharOrder.Count; i++)
		{
			if (CharOrder[i].hero == _hero)
			{
				return i;
			}
		}
		return -1;
	}

	public void SetTarget(Transform transform)
	{
		if (npcActive > -1)
		{
			targetTransform = transform;
		}
		if (!GameManager.Instance.IsMultiplayer() || IsYourTurn())
		{
			if (transform == null)
			{
				targetTransform = null;
			}
			else if (transform.GetComponent<CharacterGOItem>() != null)
			{
				targetTransform = transform.parent.transform;
			}
			else
			{
				targetTransform = transform;
			}
		}
	}

	public Transform GetTarget()
	{
		return targetTransform;
	}

	[PunRPC]
	private void NET_SetTargetTransform(string _targetTransform)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("NET_SetTargetTransform", "net");
		}
		if (_targetTransform == "")
		{
			targetTransform = null;
			return;
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD(_targetTransform + " --> " + targetTransform);
		}
		targetTransform = GameObject.Find(_targetTransform).transform;
	}

	public void DrawArrowNet(int tablePosition, Vector3 source, Vector3 target, bool isHero, byte characterIndex)
	{
		ArrowTarget = target;
		if (arrowMPCo != null)
		{
			StopCoroutine(arrowMPCo);
		}
		if (cardItemTable != null && tablePosition <= cardItemTable.Count && tablePosition >= 0 && !(cardItemTable[tablePosition] == null))
		{
			photonView.RPC("NET_DrawArrowNet", RpcTarget.Others, (short)tablePosition, isHero, characterIndex);
		}
	}

	[PunRPC]
	private void NET_DrawArrowNet(short _tablePosition, bool _isHero, byte _characterIndex)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("NET_DrawArrowNet", "net");
		}
		if (cardItemTable == null || _tablePosition > cardItemTable.Count || _tablePosition < 0 || cardItemTable[_tablePosition] == null)
		{
			return;
		}
		cardItemTable[_tablePosition].fOnMouseDownCardData();
		if (coroutineDrawArrow != null)
		{
			StopCoroutine(coroutineDrawArrow);
		}
		if (cardItemTable == null || _tablePosition > cardItemTable.Count || !(cardItemTable[_tablePosition] != null))
		{
			return;
		}
		Vector3 ori = new Vector3(cardItemTable[_tablePosition].transform.position.x, cardItemTable[_tablePosition].transform.position.y, 0f);
		Vector3 zero = Vector3.zero;
		if (_isHero)
		{
			if (TeamHero[_characterIndex] == null || !(TeamHero[_characterIndex].HeroItem != null) || !(TeamHero[_characterIndex].HeroItem.transform != null))
			{
				return;
			}
			zero = TeamHero[_characterIndex].HeroItem.transform.position + new Vector3(0f, TeamHero[_characterIndex].HeroItem.transform.GetComponent<BoxCollider2D>().size.y * 0.8f, 0f);
		}
		else
		{
			if (TeamNPC[_characterIndex] == null || !(TeamNPC[_characterIndex].NPCItem != null) || !(TeamNPC[_characterIndex].NPCItem.transform != null))
			{
				return;
			}
			zero = TeamNPC[_characterIndex].NPCItem.transform.position + new Vector3(0f, TeamNPC[_characterIndex].NPCItem.transform.GetComponent<BoxCollider2D>().size.y * 0.7f, 0f);
		}
		coroutineDrawArrow = StartCoroutine(cardItemTable[_tablePosition].DrawArrowRemote(ori, zero));
	}

	public void StopArrowNet(int tablePosition)
	{
		if (arrowMPCo != null)
		{
			StopCoroutine(arrowMPCo);
		}
		arrowMPCo = StartCoroutine(StopArrowNetCo());
	}

	private IEnumerator StopArrowNetCo()
	{
		yield return Globals.Instance.WaitForSeconds(0.3f);
		photonView.RPC("NET_StopArrowNet", RpcTarget.Others);
	}

	[PunRPC]
	private void NET_StopArrowNet()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("NET_StopArrowNet", "net");
		}
		if (coroutineDrawArrow != null)
		{
			StopCoroutine(coroutineDrawArrow);
		}
		cursorArrow.StopDraw();
	}

	public Transform GetTargetByNum(int num)
	{
		if (num < 0)
		{
			return null;
		}
		SortedDictionary<string, Transform> sortedDictionary = new SortedDictionary<string, Transform>();
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < TeamHero.Length; i++)
		{
			if (TeamHero[i] != null && TeamHero[i].Alive)
			{
				sortedDictionary.Add("H" + (3 - TeamHero[i].Position), TeamHero[i].HeroItem.transform);
			}
		}
		for (int j = 0; j < TeamNPC.Length; j++)
		{
			if (TeamNPC[j] != null && TeamNPC[j].Alive)
			{
				sortedDictionary.Add("M" + TeamNPC[j].Position, TeamNPC[j].NPCItem.transform);
			}
		}
		list = sortedDictionary.Values.ToList();
		if (num <= list.Count)
		{
			return list[num - 1];
		}
		return null;
	}

	public bool CheckTarget(Transform transform = null, CardData cardToCheck = null, bool casterIsHero = true)
	{
		if (transform == null)
		{
			transform = targetTransform;
		}
		if (transform == null)
		{
			return false;
		}
		if (cardToCheck == null)
		{
			cardToCheck = cardActive;
		}
		if (cardToCheck == null)
		{
			return false;
		}
		if (cardToCheck.TempAttackSelf)
		{
			return true;
		}
		if (cardToCheck.EffectRequired != "")
		{
			if (cardToCheck.EffectRequired != "stealth")
			{
				if (cardToCheck.EffectRequired == "stanzai")
				{
					if (theHero != null && !theHero.HasEffect("stanzai") && !theHero.HasEffect("stanzaii") && !theHero.HasEffect("stanzaiii"))
					{
						return false;
					}
				}
				else if (cardToCheck.EffectRequired == "stanzaii")
				{
					if (theHero != null && !theHero.HasEffect("stanzaii") && !theHero.HasEffect("stanzaiii"))
					{
						return false;
					}
				}
				else
				{
					if (theHero != null && !theHero.HasEffect(cardToCheck.EffectRequired))
					{
						return false;
					}
					if (theNPC != null && !theNPC.HasEffect(cardToCheck.EffectRequired))
					{
						return false;
					}
				}
			}
			else
			{
				if (theHero != null && !theHero.HasEffect("stealth") && !theHero.HasEffect("Stealthbonus"))
				{
					return false;
				}
				if (theNPC != null && !theNPC.HasEffect("stealth") && !theNPC.HasEffect("Stealthbonus"))
				{
					return false;
				}
			}
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		Hero heroById = GetHeroById(transform.name);
		NPC nPCById = GetNPCById(transform.name);
		if (cardToCheck.TargetType != Enums.CardTargetType.Global && ((casterIsHero && cardToCheck.EffectRepeatTarget != Enums.EffectRepeatTarget.Random && cardToCheck.TargetPosition != Enums.CardTargetPosition.Random) || !casterIsHero))
		{
			if (casterIsHero && nPCById != null && nPCById.HasEffect("stealth"))
			{
				return false;
			}
			if (!casterIsHero && heroById != null && heroById.HasEffect("stealth"))
			{
				return false;
			}
		}
		if (cardToCheck.TargetType != Enums.CardTargetType.Global)
		{
			Enums.CardTargetSide targetSide = cardToCheck.TargetSide;
			if (targetSide == Enums.CardTargetSide.Enemy || targetSide == Enums.CardTargetSide.Anyone)
			{
				bool flag6 = false;
				bool flag7 = false;
				List<Hero> list = new List<Hero>();
				List<NPC> list2 = new List<NPC>();
				if (heroById != null && !casterIsHero)
				{
					for (int i = 0; i < TeamHero.Length; i++)
					{
						if (TeamHero[i] != null)
						{
							Hero hero = TeamHero[i];
							if (hero != null && hero.Alive && hero.HasEffect("taunt") && !hero.HasEffect("stealth"))
							{
								list.Add(hero);
								flag6 = true;
							}
						}
					}
					if (flag6 && !list.Contains(heroById))
					{
						return false;
					}
				}
				else if (nPCById != null && casterIsHero)
				{
					for (int j = 0; j < TeamNPC.Length; j++)
					{
						if (TeamNPC[j] != null)
						{
							NPC nPC = TeamNPC[j];
							if (nPC != null && nPC.Alive && nPC.HasEffect("taunt") && !nPC.HasEffect("stealth"))
							{
								list2.Add(nPC);
								flag7 = true;
							}
						}
					}
					if (flag7 && !list2.Contains(nPCById))
					{
						return false;
					}
				}
			}
		}
		if (heroById != null)
		{
			flag = true;
			if (PositionIsFront(isHero: true, heroById.Position))
			{
				flag3 = true;
			}
			if (PositionIsBack(heroById))
			{
				flag4 = true;
			}
			if (theHero != null && transform.name == theHero.Id)
			{
				flag5 = true;
			}
		}
		else if (nPCById != null)
		{
			flag2 = true;
			if (PositionIsFront(isHero: false, nPCById.Position))
			{
				flag3 = true;
			}
			if (PositionIsBack(nPCById))
			{
				flag4 = true;
			}
			if (theNPC != null && transform.name == theNPC.Id)
			{
				flag5 = true;
			}
		}
		if (flag || flag2)
		{
			if (cardToCheck.TargetPosition == Enums.CardTargetPosition.Front && !flag3)
			{
				return false;
			}
			if (cardToCheck.TargetPosition == Enums.CardTargetPosition.Back && !flag4)
			{
				return false;
			}
			if (cardToCheck.TargetSide == Enums.CardTargetSide.Friend)
			{
				if (flag && casterIsHero)
				{
					return true;
				}
				if (flag2 && !casterIsHero)
				{
					return true;
				}
				return false;
			}
			if (cardToCheck.TargetSide == Enums.CardTargetSide.Enemy)
			{
				if (flag && casterIsHero)
				{
					return false;
				}
				if (flag2 && !casterIsHero)
				{
					return false;
				}
				return true;
			}
			if (cardToCheck.TargetSide == Enums.CardTargetSide.Self)
			{
				if (flag5)
				{
					return true;
				}
				return false;
			}
			if (cardToCheck.TargetSide == Enums.CardTargetSide.FriendNotSelf)
			{
				if (flag && casterIsHero && !flag5)
				{
					return true;
				}
				if (flag2 && !casterIsHero && !flag5)
				{
					return true;
				}
				return false;
			}
			if (cardToCheck.TargetSide == Enums.CardTargetSide.Anyone)
			{
				return true;
			}
		}
		return false;
	}

	public bool HaveDeckEffect(CardData _cardActive)
	{
		if (_cardActive != null && (_cardActive.DiscardCard != 0 || _cardActive.LookCards != 0))
		{
			return true;
		}
		return false;
	}

	public bool CanInstaCast(CardData _cardActive)
	{
		if (_cardActive == null)
		{
			return false;
		}
		if (canInstaCastDict.ContainsKey(_cardActive.InternalId))
		{
			return canInstaCastDict[_cardActive.InternalId];
		}
		if (_cardActive.TargetType == Enums.CardTargetType.Global)
		{
			canInstaCastDict[_cardActive.InternalId] = true;
			return true;
		}
		if (_cardActive.TargetSide == Enums.CardTargetSide.Self)
		{
			canInstaCastDict[_cardActive.InternalId] = true;
			return true;
		}
		if (_cardActive.TargetPosition == Enums.CardTargetPosition.Random)
		{
			canInstaCastDict[_cardActive.InternalId] = true;
			return true;
		}
		if (_cardActive.TargetSide == Enums.CardTargetSide.Enemy)
		{
			bool flag = true;
			if (heroActive > -1)
			{
				for (int i = 0; i < TeamNPC.Length; i++)
				{
					if (TeamNPC[i] != null && !(TeamNPC[i].NpcData == null) && TeamNPC[i].Alive && !TeamNPC[i].HasEffect("stealth"))
					{
						flag = false;
					}
				}
			}
			else
			{
				for (int j = 0; j < TeamHero.Length; j++)
				{
					if (TeamHero[j] != null && !(TeamHero[j].HeroData == null) && TeamHero[j].Alive && !TeamHero[j].HasEffect("stealth"))
					{
						flag = false;
					}
				}
			}
			if (flag)
			{
				canInstaCastDict[_cardActive.InternalId] = false;
				return false;
			}
		}
		if (_cardActive.TargetPosition != Enums.CardTargetPosition.Anywhere)
		{
			canInstaCastDict[_cardActive.InternalId] = true;
			return true;
		}
		if (_cardActive.TargetSide == Enums.CardTargetSide.Enemy)
		{
			int num = 0;
			if (heroActive > -1)
			{
				for (int k = 0; k < TeamNPC.Length; k++)
				{
					if (TeamNPC[k] != null && !(TeamNPC[k].NpcData == null) && TeamNPC[k].Alive && TeamNPC[k].HasEffect("taunt") && !TeamNPC[k].HasEffect("stealth"))
					{
						num++;
					}
				}
			}
			else
			{
				for (int l = 0; l < TeamHero.Length; l++)
				{
					if (TeamHero[l] != null && !(TeamHero[l].HeroData == null) && TeamHero[l].Alive && TeamHero[l].HasEffect("taunt") && !TeamHero[l].HasEffect("stealth"))
					{
						num++;
					}
				}
			}
			if (num > 0)
			{
				if (num > 1)
				{
					canInstaCastDict[_cardActive.InternalId] = false;
					return false;
				}
				if (num == 1)
				{
					canInstaCastDict[_cardActive.InternalId] = true;
					return true;
				}
			}
		}
		int num2 = 0;
		int num3 = 0;
		for (int m = 0; m < TeamHero.Length; m++)
		{
			if (TeamHero[m] != null && !(TeamHero[m].HeroData == null) && TeamHero[m].Alive)
			{
				if (CheckTarget(TeamHero[m].HeroItem.transform, _cardActive))
				{
					num2++;
				}
				else
				{
					num3++;
				}
			}
		}
		for (int n = 0; n < TeamNPC.Length; n++)
		{
			if (TeamNPC[n] != null && !(TeamNPC[n].NpcData == null) && TeamNPC[n].Alive && TeamNPC[n].NPCItem != null)
			{
				if (CheckTarget(TeamNPC[n].NPCItem.transform, _cardActive))
				{
					num2++;
				}
				else
				{
					num3++;
				}
			}
		}
		if (num2 == 1)
		{
			canInstaCastDict[_cardActive.InternalId] = true;
			return true;
		}
		canInstaCastDict[_cardActive.InternalId] = false;
		return false;
	}

	public void ClearCanInstaCastDict()
	{
		canInstaCastDict.Clear();
	}

	public List<Transform> GetInstaCastTransformList(CardData _cardActive, bool casterIsHero = true)
	{
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < TeamHero.Length; i++)
		{
			if (TeamHero[i] != null && !(TeamHero[i].HeroData == null) && TeamHero[i].HeroItem != null && TeamHero[i].Alive && CheckTarget(TeamHero[i].HeroItem.transform, _cardActive, casterIsHero))
			{
				list.Add(TeamHero[i].HeroItem.transform);
			}
		}
		for (int j = 0; j < TeamNPC.Length; j++)
		{
			if (TeamNPC[j] != null && !(TeamNPC[j].NpcData == null) && TeamNPC[j].Alive && TeamNPC[j].NPCItem != null && CheckTarget(TeamNPC[j].NPCItem.transform, _cardActive, casterIsHero))
			{
				list.Add(TeamNPC[j].NPCItem.transform);
			}
		}
		return list;
	}

	public bool IsThereAnyTargetForCard(CardData _cardActive)
	{
		if (_cardActive.TargetSide == Enums.CardTargetSide.Friend || _cardActive.TargetSide == Enums.CardTargetSide.Self || _cardActive.TargetSide == Enums.CardTargetSide.FriendNotSelf || _cardActive.TargetSide == Enums.CardTargetSide.Anyone)
		{
			for (int i = 0; i < TeamHero.Length; i++)
			{
				if (TeamHero[i] != null && !(TeamHero[i].HeroData == null) && TeamHero[i].Alive && TeamHero[i].HeroItem != null && CheckTarget(TeamHero[i].HeroItem.transform, _cardActive))
				{
					return true;
				}
			}
		}
		if (_cardActive.TargetSide == Enums.CardTargetSide.Enemy || _cardActive.TargetSide == Enums.CardTargetSide.Anyone)
		{
			for (int j = 0; j < TeamNPC.Length; j++)
			{
				if (TeamNPC[j] != null && !(TeamNPC[j].NpcData == null) && TeamNPC[j].Alive && TeamNPC[j].NPCItem != null && CheckTarget(TeamNPC[j].NPCItem.transform, _cardActive))
				{
					return true;
				}
			}
		}
		return false;
	}

	public int GetHeroFromId(string _id)
	{
		for (int i = 0; i < 4; i++)
		{
			if (TeamHero[i] != null && TeamHero[i].Id == _id)
			{
				return i;
			}
		}
		return -1;
	}

	public bool CheckEnergyForCast()
	{
		if (GetHeroEnergy() >= theHero.GetCardFinalCost(cardActive))
		{
			return true;
		}
		return false;
	}

	public int GetHeroEnergy()
	{
		if (theHero == null)
		{
			return -1;
		}
		return theHero.GetEnergy();
	}

	public void SetEnergyCounter(int energy, int energyMod = 0)
	{
		energyCounterTM.text = energy.ToString();
		if (energy > 0)
		{
			energyCounterAnim.enabled = true;
			return;
		}
		energyCounterAnim.enabled = false;
		energyCounterTM.color = new Color(1f, 1f, 1f, 0.4f);
		energyCounterBg.color = new Color(0.4f, 0.4f, 0.4f);
	}

	private void ShowEnergyCounter(bool state)
	{
		energyCounterTM.transform.parent.gameObject.SetActive(state);
	}

	public void ShowEnergyCounterParticles()
	{
		energyCounterParticle.Play();
	}

	public void AssignEnergyAction(int energy)
	{
		energyAssigned = energy;
		theHero.ModifyEnergy(-energy);
		energyJustWastedByHero += energyAssigned;
		if (GameManager.Instance.IsMultiplayer() && IsYourTurn())
		{
			photonView.RPC("NET_AssignEnergyAction", RpcTarget.Others, energy);
		}
		OnCardCastByHeroBegins?.Invoke(TeamHero[heroActive], cardActive);
		StartCoroutine(AssignEnergyActionCo());
	}

	private IEnumerator AssignEnergyActionCo()
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("**************************", "net");
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("WaitingSyncro assignenergyactionco", "net");
			}
			yield return Globals.Instance.WaitForSeconds(0.1f);
			if (NetworkManager.Instance.IsMaster())
			{
				if (coroutineSyngAssignEnergy != null)
				{
					StopCoroutine(coroutineSyngAssignEnergy);
				}
				coroutineSyngAssignEnergy = StartCoroutine(ReloadCombatCo("assignenergyactionco"));
				while (!NetworkManager.Instance.AllPlayersReady("assignenergyactionco"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (coroutineSyngAssignEnergy != null)
				{
					StopCoroutine(coroutineSyngAssignEnergy);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked assignenergyactionco", "net");
				}
				NetworkManager.Instance.PlayersNetworkContinue("assignenergyactionco");
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("assignenergyactionco", status: true);
				NetworkManager.Instance.SetStatusReady("assignenergyactionco");
				while (NetworkManager.Instance.WaitingSyncro["assignenergyactionco"])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("assignenergyactionco, we can continue!", "net");
				}
			}
		}
		energySelector.TurnOff();
		waitingForCardEnergyAssignment = false;
		EventSystem.current.SetSelectedGameObject(null);
		yield return null;
	}

	[PunRPC]
	private void NET_AssignEnergyAction(int energy)
	{
		AssignEnergyAction(energy);
	}

	public void AssignEnergyMultiplayer(int energy)
	{
		photonView.RPC("NET_AssignEnergyMultiplayer", RpcTarget.Others, energy);
	}

	[PunRPC]
	private void NET_AssignEnergyMultiplayer(int energy)
	{
		energySelector.AssignEnergyFromOutside(energy);
	}

	[PunRPC]
	private void NET_ShareArrLookDiscard(string _arrToAddCard, int _randomIndex)
	{
		SetRandomIndex(_randomIndex);
		StartCoroutine(NET_ShareArrLookDiscardCo(_arrToAddCard, _randomIndex));
	}

	private IEnumerator NET_ShareArrLookDiscardCo(string _arrToAddCard, int _randomIndex)
	{
		List<string> list = JsonHelper.FromJson<string>(_arrToAddCard).ToList();
		CICardAddcard.Clear();
		CardItem item = new CardItem();
		for (int i = 0; i < list.Count; i++)
		{
			string text = list[i];
			if (cardGos.ContainsKey(text))
			{
				item = cardGos[text].GetComponent<CardItem>();
			}
			else
			{
				GameObject gameObject = GameObject.Find(text);
				if (gameObject != null)
				{
					item = gameObject.transform.GetComponent<CardItem>();
				}
				else
				{
					foreach (Transform item2 in deckCardsWindow.cardContainer)
					{
						if ((bool)item2.GetComponent<CardItem>() && item2.GetComponent<CardItem>().CardData.InternalId == text)
						{
							item = item2.GetComponent<CardItem>();
							break;
						}
					}
				}
			}
			CICardAddcard.Add(item);
		}
		deckCardsWindow.TurnOff();
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("**************************", "net");
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("WaitingSyncro NET_SALD_" + _randomIndex);
		}
		yield return Globals.Instance.WaitForSeconds(0.1f);
		if (NetworkManager.Instance.IsMaster())
		{
			if (coroutineSyncLookDiscard != null)
			{
				StopCoroutine(coroutineSyncLookDiscard);
			}
			coroutineSyncLookDiscard = StartCoroutine(ReloadCombatCo("NET_SALD_" + _randomIndex));
			while (!NetworkManager.Instance.AllPlayersReady("NET_SALD_" + _randomIndex))
			{
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			if (coroutineSyncLookDiscard != null)
			{
				StopCoroutine(coroutineSyncLookDiscard);
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("Game ready, Everybody checked NET_SALD_" + _randomIndex, "net");
			}
			NetworkManager.Instance.PlayersNetworkContinue("NET_SALD_" + _randomIndex);
			yield return Globals.Instance.WaitForSeconds(0.2f);
		}
		else
		{
			NetworkManager.Instance.SetWaitingSyncro("NET_SALD_" + _randomIndex, status: true);
			NetworkManager.Instance.SetStatusReady("NET_SALD_" + _randomIndex);
			while (NetworkManager.Instance.WaitingSyncro["NET_SALD_" + _randomIndex])
			{
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("NET_SALD_" + _randomIndex + ", we can continue!", "net");
			}
		}
		waitingForLookDiscardWindow = false;
		waitingForAddcardAssignment = false;
	}

	public void AssignLookDiscardAction()
	{
		if (!GameManager.Instance.IsMultiplayer())
		{
			waitingForLookDiscardWindow = false;
			waitingForAddcardAssignment = false;
			deckCardsWindow.TurnOff();
		}
		else
		{
			List<string> list = new List<string>();
			for (int i = 0; i < CICardAddcard.Count; i++)
			{
				if (CICardAddcard[i] != null && CICardAddcard[i].CardData != null)
				{
					list.Add(CICardAddcard[i].CardData.InternalId);
				}
			}
			string text = JsonHelper.ToJson(list.ToArray());
			photonView.RPC("NET_ShareArrLookDiscard", RpcTarget.All, text, randomIndex);
		}
		EventSystem.current.SetSelectedGameObject(null);
	}

	public void AmplifyCard(int tablePosition)
	{
		if (amplifyCardCo != null)
		{
			StopCoroutine(amplifyCardCo);
		}
		amplifyCardCo = StartCoroutine(amplifyCardCoroutine(tablePosition));
		if (preCastNum != -1)
		{
			return;
		}
		for (int i = 0; i < cardItemTable.Count; i++)
		{
			if (i != tablePosition)
			{
				AmplifyCardOut(i, fromnet: false);
			}
		}
	}

	private IEnumerator amplifyCardCoroutine(int tablePosition)
	{
		yield return Globals.Instance.WaitForSeconds(0.05f);
		photonView.RPC("NET_AmplifyCard", RpcTarget.Others, (short)tablePosition);
	}

	[PunRPC]
	public void NET_AmplifyCard(short _tablePosition)
	{
		SetDamagePreview(theCasterIsHero: false);
		SetOverDeck(state: false);
		RepositionCards();
		if (cardItemTable != null && cardItemTable.Count != 0 && _tablePosition < cardItemTable.Count)
		{
			cardItemTable[_tablePosition].fOnMouseEnter();
		}
	}

	public void AmplifyCardOut(int tablePosition, bool fromnet = true)
	{
		if (fromnet)
		{
			if (amplifyCardCo != null)
			{
				StopCoroutine(amplifyCardCo);
			}
			amplifyCardCo = StartCoroutine(amplifyCardOutCoroutine(tablePosition));
		}
		else if (cardItemTable != null && cardItemTable.Count != 0 && tablePosition <= cardItemTable.Count - 1)
		{
			cardItemTable[tablePosition].fOnMouseExit();
		}
	}

	private IEnumerator amplifyCardOutCoroutine(int tablePosition)
	{
		yield return Globals.Instance.WaitForSeconds(0.05f);
		photonView.RPC("NET_AmplifyCardOut", RpcTarget.Others, (short)tablePosition);
	}

	[PunRPC]
	public void NET_AmplifyCardOut(short _tablePosition)
	{
		SetDamagePreview(theCasterIsHero: false);
		SetOverDeck(state: false);
		if (cardItemTable != null && cardItemTable.Count != 0 && _tablePosition <= cardItemTable.Count - 1)
		{
			cardItemTable[_tablePosition].fOnMouseExit();
		}
	}

	public void NoEnergy()
	{
		theHero.HeroItem.ScrollCombatText(Texts.Instance.GetText("noEnergy"), Enums.CombatScrollEffectType.Energy);
	}

	private void CastNPC()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("PreCastNPC " + theNPC.Id, "trace");
		}
		if (IsGameBusy() || theNPC == null)
		{
			return;
		}
		if (coroutineCastNPC != null)
		{
			StopCoroutine(coroutineCastNPC);
		}
		if (!(theNPC.NPCItem == null))
		{
			if (CardsInNPCHand(theNPC.NPCIndex) > 0)
			{
				coroutineCastNPC = StartCoroutine(CastNPCCo());
			}
			else
			{
				EndTurn();
			}
		}
	}

	private IEnumerator CastNPCCo()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("CastNPCCo", "trace");
		}
		int eventExaust = 0;
		yield return Globals.Instance.WaitForSeconds(0.1f);
		while (eventList.Count > 0)
		{
			if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
			{
				eventListDbg = "";
				for (int i = 0; i < eventList.Count; i++)
				{
					eventListDbg = eventListDbg + eventList[i] + " || ";
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[CastNPCCo] Waiting For Eventlist to clean", "general");
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(eventListDbg, "general");
				}
			}
			eventExaust++;
			if (eventExaust > 300)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[CastNPCCo] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "general");
				}
				ClearEventList();
				break;
			}
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
		yield return Globals.Instance.WaitForSeconds(0.1f);
		if (GameManager.Instance.IsMultiplayer())
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("**************************", "net");
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("WaitingSyncro castnpco", "net");
			}
			if (NetworkManager.Instance.IsMaster())
			{
				if (coroutineSyncCastNPC != null)
				{
					StopCoroutine(coroutineSyncCastNPC);
				}
				coroutineSyncCastNPC = StartCoroutine(ReloadCombatCo("castnpco"));
				while (!NetworkManager.Instance.AllPlayersReady("castnpco"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (coroutineSyncCastNPC != null)
				{
					StopCoroutine(coroutineSyncCastNPC);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked castnpco", "net");
				}
				NetworkManager.Instance.PlayersNetworkContinue("castnpco");
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("castnpco", status: true);
				NetworkManager.Instance.SetStatusReady("castnpco");
				while (NetworkManager.Instance.WaitingSyncro["castnpco"])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("castnpco, we can continue!", "net");
				}
			}
		}
		if (!GameManager.Instance.IsMultiplayer() && !GameManager.Instance.TutorialWatched("castNPC"))
		{
			waitingTutorial = true;
			yield return Globals.Instance.WaitForSeconds(0.1f);
			GameManager.Instance.ShowTutorialPopup("castNPC", theNPC.NPCItem.transform.Find("Cards").transform.position, Vector3.zero);
			characterWindow.Hide();
			while (waitingTutorial)
			{
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
			if (GameManager.Instance.IsMultiplayer())
			{
				SetWatchingTutorial(state: false);
			}
		}
		eventExaust = 0;
		waitExecution = true;
		while (waitExecution && theNPC != null && theNPC.NPCItem != null && theNPC.NPCItem.IsCombatScrollEffectActive())
		{
			yield return Globals.Instance.WaitForSeconds(0.01f);
			eventExaust++;
			if (eventExaust > 400)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[PREwait] Waitexecution EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
				}
				waitExecution = false;
			}
		}
		while (waitingDeathScreen)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[castNPC] waitingDeathScreen", "general");
			}
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
		eventExaust = 0;
		while (waitingKill)
		{
			yield return Globals.Instance.WaitForSeconds(0.01f);
			eventExaust++;
			if (eventExaust > 400)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[castNPC] waitingKill EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
				}
				waitExecution = false;
			}
		}
		if (theNPC != null && theNPC.Alive)
		{
			float castDelay = 0f;
			if (theNPC.Id.StartsWith("count") && BossNpc != null && BossNpc is Dracula)
			{
				while (GameManager.Instance.DisableCardCast)
				{
					yield return new WaitForSeconds(0.1f);
				}
			}
			bool casted = AI.DoAI(theNPC, TeamHero, TeamNPC, ref castDelay);
			if (castDelay > 0f && !casted && BossNpc != null && BossNpc is PhantomArmor)
			{
				PhantomArmor pa = BossNpc as PhantomArmor;
				pa.TriggerSpecialEffect();
				yield return Globals.Instance.WaitForSeconds(castDelay);
				casted = AI.DoAI(theNPC, TeamHero, TeamNPC, ref castDelay);
				yield return Globals.Instance.WaitForSeconds(2f * castDelay);
				pa.SpecialEffectFinish();
			}
			if (!casted && !CheckMatchIsOver())
			{
				EndTurn();
			}
		}
		yield return null;
	}

	public void NPCDiscard(int npcIndex, int cardPosition, bool casted)
	{
		if (npcIndex >= NPCHand.Length || NPCHand[npcIndex] == null || cardPosition >= NPCHand[npcIndex].Count || NPCHand[npcIndex][cardPosition] == null)
		{
			return;
		}
		string text = NPCHand[npcIndex].ElementAt(cardPosition);
		bool flag = true;
		if (casted && GetCardData(text).Vanish)
		{
			flag = false;
		}
		if (flag)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("NPCDiscard " + npcIndex, "trace");
			}
			if (NPCDeckDiscard[npcIndex] == null)
			{
				NPCDeckDiscard[npcIndex] = new List<string>();
			}
			NPCDeckDiscard[npcIndex].Add(text);
		}
		NPCHand[npcIndex][cardPosition] = null;
	}

	public int CardsInNPCHand(int npcIndex)
	{
		int num = 0;
		if (npcIndex < NPCHand.Length && NPCHand[npcIndex] != null)
		{
			for (int i = 0; i < NPCHand[npcIndex].Count; i++)
			{
				if (i < NPCHand[npcIndex].Count && NPCHand[npcIndex][i] != null)
				{
					num++;
				}
			}
		}
		return num;
	}

	public void CastAutomatic(CardData theCardData, Transform from, Transform to)
	{
		theHeroPreAutomatic = theHero;
		theNPCPreAutomatic = theNPC;
		string npcInternalId = "";
		if (from.GetComponent<HeroItem>() != null)
		{
			theHero = from.GetComponent<HeroItem>().Hero;
			theNPC = null;
			npcInternalId = theHero.InternalId;
		}
		else if (from.GetComponent<NPCItem>() != null)
		{
			theNPC = from.GetComponent<NPCItem>().NPC;
			theHero = null;
			npcInternalId = theNPC.InternalId;
		}
		SetTarget(to);
		if (theCardData.Id.Contains('_'))
		{
			NPCCastCardList(theCardData.Id);
		}
		else
		{
			NPCCastCardList(theCardData.InternalId, npcInternalId);
		}
		StartCoroutine(CastCard(null, _automatic: true, theCardData, 0));
	}

	private void CreateConsoleKey()
	{
		consoleKey = GetRandomString();
		console.SetKey(consoleKey);
	}

	public IEnumerator JustCastedCo()
	{
		justCasted = true;
		yield return Globals.Instance.WaitForSeconds(0.3f);
		justCasted = false;
	}

	public IEnumerator CastCard(CardItem theCardItem = null, bool _automatic = false, CardData _card = null, int _energy = -1, int _posInTable = -1, bool _propagate = true, Hero casterHeroParam = null)
	{
		if (!GameManager.Instance.IsMultiplayer())
		{
			RaiseCardCastBegin(theCardItem);
		}
		ClearCardsBorder();
		if (gameStatus == "EndTurn")
		{
			yield break;
		}
		ResetFailCount();
		preCastNum = -1;
		GlobalVanishCardsNum = 0;
		handCardsBeforeCast = CountHeroHand();
		deckCardsBeforeCast = CountHeroDeck();
		discardCardsBeforeCast = CountHeroDiscard();
		vanishCardsBeforeCast = CountHeroVanish();
		ClearCanInstaCastDict();
		string comingId = "";
		if (_card != null)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("CastCard '" + _card.Id + "'", "general");
			}
			comingId = _card.Id;
		}
		else if (theCardItem != null)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("CastCard '" + theCardItem.CardData.Id + "'", "general");
			}
			comingId = theCardItem.CardData.Id;
			theCardItem.RemoveEmotes();
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[CASTCARD] SetGameBusy TRUE GameStatus CastCard " + comingId, "general");
		}
		SetGameBusy(state: true);
		gameStatus = "CastCard";
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD(gameStatus, "gamestatus");
		}
		SetEventDirect("CastCardEvent" + comingId, automatic: false, add: true);
		ShowHandMask(state: true);
		ShowCombatKeyboard(_state: false);
		if (GameManager.Instance.IsMultiplayer())
		{
			cursorArrow.StopDraw();
			RaiseCardCastBegin(theCardItem);
			if (IsYourTurn())
			{
				if (!_automatic && _propagate)
				{
					CastCardPropagation(theCardItem, _automatic, _card, _energy, theCardItem.TablePosition);
				}
			}
			else if (theCardItem == null && _posInTable != -1)
			{
				bool flag = false;
				if (_posInTable >= cardItemTable.Count)
				{
					flag = true;
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("**** errorWithCard (0) **", "error");
					}
				}
				else if (cardItemTable[_posInTable] == null)
				{
					flag = true;
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("**** errorWithCard (1) **", "error");
					}
				}
				if (!flag)
				{
					theCardItem = cardItemTable[_posInTable];
					if (theCardItem.CardData.Id != comingId)
					{
						if (theCardItem.CardData.Id.Split('_')[0] == comingId.Split('_')[0])
						{
							Debug.LogError("**** errorWithCard BUT continue " + theCardItem.CardData.Id + "!=" + comingId + "**");
							theCardItem.CardData.Id = comingId;
						}
						else
						{
							flag = true;
							Debug.LogError("**** errorWithCard " + theCardItem.CardData.Id + "!=" + comingId + "**");
						}
					}
				}
				if (flag && NetworkManager.Instance.IsMaster())
				{
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("**** errorWithCard -> master Break by Desync **", "error");
					}
					yield return Globals.Instance.WaitForSeconds(0.1f);
					ReloadCombat("**** errorWithCard -> master Break by Desync **");
					yield break;
				}
			}
		}
		CardData _cardActive;
		if (theCardItem != null)
		{
			_cardActive = theCardItem.CardData;
		}
		else
		{
			_cardActive = _card;
		}
		if (!_automatic && GameManager.Instance.IsMultiplayer() && !castingCardListMP.Contains(_cardActive.Id))
		{
			castingCardListMP.Add(_cardActive.Id);
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("CastCardProcess Debug 1", "trace");
		}
		if (_cardActive != null && TeamHero != null && heroActive > -1 && heroActive < TeamHero.Length && TeamHero[heroActive] != null)
		{
			List<NPC> npcsWithAbility;
			bool flag2 = TryGetNPCsWithMasterReweaverAbility(_cardActive, TeamHero[heroActive], out npcsWithAbility);
			CardItem cardItem = cardItemTable.FirstOrDefault((CardItem cardItem9) => cardItem9.CardData.Id == _cardActive.Id);
			if (!_cardActive.TempAttackSelf && flag2)
			{
				ApplyCorruptedEcho(_cardActive, cardItem, npcsWithAbility);
			}
			else if (_cardActive.TempAttackSelf && !flag2)
			{
				ReturnCardDataToOriginalState(_cardActive, cardItem);
			}
			TeamHero[heroActive].SetCastedCard(_cardActive);
		}
		if (_automatic)
		{
			if (_cardActive.CardName.ToLower() == "crystallize")
			{
				scarabSuccess = "1";
			}
			if (_cardActive.Fluff != "" && (float)(DateTime.Now.Millisecond % 100) < _cardActive.FluffPercent)
			{
				DoComic(theNPC, _cardActive.Fluff);
			}
			GameManager.Instance.PlayLibraryAudio("castnpccard");
			if (GameManager.Instance.IsMultiplayer())
			{
				string _cardForSync = _cardActive.InternalId;
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("**************************", "net");
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("WaitingSyncro castnpccard_" + _cardForSync, "net");
				}
				if (NetworkManager.Instance.IsMaster())
				{
					if (coroutineSyncCastCardNPC != null)
					{
						StopCoroutine(coroutineSyncCastCardNPC);
					}
					coroutineSyncCastCardNPC = StartCoroutine(ReloadCombatCo("castnpccard_" + _cardForSync));
					while (!NetworkManager.Instance.AllPlayersReady("castnpccard_" + _cardForSync))
					{
						yield return Globals.Instance.WaitForSeconds(0.01f);
					}
					if (coroutineSyncCastCardNPC != null)
					{
						StopCoroutine(coroutineSyncCastCardNPC);
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("Game ready, Everybody checked castnpccard_" + _cardForSync, "net");
					}
					SetRandomIndex(randomIndex);
					NetworkManager.Instance.PlayersNetworkContinue("castnpccard_" + _cardForSync, randomIndex.ToString());
				}
				else
				{
					NetworkManager.Instance.SetWaitingSyncro("castnpccard_" + _cardForSync, status: true);
					NetworkManager.Instance.SetStatusReady("castnpccard_" + _cardForSync);
					while (NetworkManager.Instance.WaitingSyncro["castnpccard_" + _cardForSync])
					{
						yield return Globals.Instance.WaitForSeconds(0.01f);
					}
					if (NetworkManager.Instance.netAuxValue != "")
					{
						SetRandomIndex(int.Parse(NetworkManager.Instance.netAuxValue));
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("castnpccard_" + _cardForSync + ", we can continue!", "net");
					}
				}
			}
			else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
			{
				yield return Globals.Instance.WaitForSeconds(0.8f);
			}
			else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Ultrafast)
			{
				yield return null;
			}
			else
			{
				yield return Globals.Instance.WaitForSeconds(0.28f);
			}
			if (!GameManager.Instance.IsMultiplayer() && theNPC != null && theNPC.NPCItem != null)
			{
				CardData cardData = _cardActive;
				if ((object)cardData != null && cardData.SpecialCardEnum == SpecialCardEnum.NightmareImage && !GameManager.Instance.TutorialWatched("illusionAbility"))
				{
					CardItem cardTutorial = theNPC.GetCurrentCardItem();
					yield return Globals.Instance.WaitForSeconds(0.3f);
					waitingTutorial = true;
					yield return Globals.Instance.WaitForSeconds(0.2f);
					GameObject gameObject = cardTutorial.GetComponentsInChildren<Transform>(includeInactive: true).First((Transform t) => t.name == "TitleTextTab").gameObject;
					GameManager.Instance.ShowTutorialPopup("illusionAbility", gameObject.transform.position, Vector3.zero);
					characterWindow.Hide();
					while (waitingTutorial)
					{
						yield return Globals.Instance.WaitForSeconds(0.1f);
					}
				}
			}
			if (_cardActive.MoveToCenter)
			{
				if (_cardActive.AddCard == 0 && !IsPhantomArmorSpecialCard(_cardActive.Id))
				{
					yield return Globals.Instance.WaitForSeconds(0.15f);
					theNPC.NPCItem.CharacterAttackAnim();
				}
			}
			else if (theNPC != null && theNPC.NPCItem != null && !IsPhantomArmorSpecialCard(_cardActive.Id))
			{
				theNPC.NPCItem.CharacterCastAnim();
				yield return Globals.Instance.WaitForSeconds(0.2f);
			}
		}
		if (castingCardBlocked.ContainsKey(_cardActive.InternalId))
		{
			castingCardBlocked[_cardActive.InternalId] = true;
		}
		else
		{
			castingCardBlocked.Add(_cardActive.InternalId, value: true);
		}
		string _uniqueCastId = Functions.RandomString(6f);
		castCardDamageDoneTotal = 0f;
		energyAssigned = _energy;
		Hero theCasterHero = theHero;
		if (casterHeroParam != null)
		{
			theCasterHero = casterHeroParam;
		}
		NPC theCasterNPC = theNPC;
		Character theCasterCharacter = null;
		bool theCasterIsHero = false;
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("CastCardProcess Debug 2", "trace");
		}
		if (targetTransform == null)
		{
			List<Transform> instaCastTransformList = GetInstaCastTransformList(_cardActive);
			if (instaCastTransformList.Count == 0)
			{
				yield break;
			}
			targetTransform = instaCastTransformList[0];
			if (_cardActive.TargetPosition == Enums.CardTargetPosition.Random)
			{
				targetTransform = instaCastTransformList[GetRandomIntRange(0, instaCastTransformList.Count)];
			}
		}
		if (theCasterHero != null)
		{
			theCasterIsHero = true;
			theCasterCharacter = theCasterHero;
			_ = theHero.GameName;
			_ = theHero.Id;
			if (_cardActive != null)
			{
				if (!_cardActive.AutoplayDraw && !_cardActive.AutoplayEndTurn)
				{
					energyJustWastedByHero = theHero.GetCardFinalCost(_cardActive);
				}
			}
			else
			{
				energyJustWastedByHero = 0;
			}
			theHero.ModifyEnergy(-theHero.GetCardFinalCost(_cardActive));
			if (_cardActive.TargetSide == Enums.CardTargetSide.Self && theHero.HeroItem != null)
			{
				targetTransform = theHero.HeroItem.transform;
			}
			theHero.SetEvent(Enums.EventActivation.CastCard, null, _cardActive.GetCardFinalCost(), _cardActive.Id);
			if (theCasterHero.HeroData != null && theCasterHero.HeroData.HeroSubClass.ActionSound != null)
			{
				GameManager.Instance.PlayAudio(theCasterHero.HeroData.HeroSubClass.ActionSound, 0.25f);
			}
		}
		else if (theCasterNPC != null)
		{
			theCasterCharacter = theCasterNPC;
			_ = theNPC.GameName;
			_ = theNPC.Id;
			if (_cardActive.TargetSide == Enums.CardTargetSide.Self && theNPC.NPCItem != null)
			{
				targetTransform = theNPC.NPCItem.transform;
			}
			theNPC.ModifyEnergy(-theNPC.GetCardFinalCost(_cardActive));
			theNPC.SetEvent(Enums.EventActivation.CastCard, null, _cardActive.GetCardFinalCost(), _cardActive.Id);
			if (targetTransform != null)
			{
				theHero = GetHeroById(targetTransform.name);
			}
		}
		if (waitingItemTrait)
		{
			while (waitingItemTrait)
			{
				waitingItemTrait = false;
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("CastCardProcess Debug 2.5", "trace");
		}
		AudioClip soundRelease = _cardActive.GetSoundRelease(theCasterHero, theCasterNPC);
		if (soundRelease != null)
		{
			GameManager.Instance.PlayAudio(soundRelease, 0.25f);
		}
		if (theCardItem != null && theCardItem.CardData.EffectPreAction.Trim() != "")
		{
			if (theCasterNPC != null && theCasterNPC.NPCItem != null)
			{
				EffectsManager.Instance.PlayEffectAC(theCardItem.CardData.EffectPreAction, isHero: true, theCasterNPC.NPCItem.CharImageT, flip: false);
				if (!theCardItem.CardData.IsPetCast)
				{
					theCasterNPC.NPCItem.CharacterCastAnim();
				}
			}
			else if (theCasterHero != null && theCasterHero.HeroItem != null)
			{
				EffectsManager.Instance.PlayEffectAC(theCardItem.CardData.EffectPreAction, isHero: true, theCasterHero.HeroItem.CharImageT, flip: false);
				if (!theCardItem.CardData.IsPetCast)
				{
					theCasterHero.HeroItem.CharacterCastAnim();
				}
			}
			if (theCardItem.CardData.EffectPostCastDelay > 0f)
			{
				theCardItem.PreDiscardCard();
				float num = theCardItem.CardData.EffectPostCastDelay;
				if (GameManager.Instance.IsMultiplayer() || GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Fast || GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Ultrafast)
				{
					num *= 0.7f;
				}
				if (num < 0.1f)
				{
					num = 0.1f;
				}
				yield return Globals.Instance.WaitForSeconds(num);
			}
		}
		string theCardItemPostDiscard = "";
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("CastCardProcess Debug 3", "trace");
		}
		if (!_automatic)
		{
			if (theCardItem != null && theCardItem.CardData != null && theCardItem.CardData.Vanish)
			{
				theCardItem.cardVanish = true;
				DiscardCard(theCardItem.TablePosition, Enums.CardPlace.Vanish);
			}
			else if (_cardActive.DrawCard != 0 || _cardActive.AddCard != 0)
			{
				theCardItemPostDiscard = theCardItem.InternalId;
				DiscardCard(theCardItem.TablePosition, Enums.CardPlace.Discard, moveToDiscard: false);
			}
			else
			{
				DiscardCard(theCardItem.TablePosition);
			}
			if (energyAssigned == -1 && (_cardActive.DamageEnergyBonus > 0 || _cardActive.HealEnergyBonus > 0 || _cardActive.EffectRepeatEnergyBonus > 0 || (_cardActive.AcEnergyBonus != null && _cardActive.AcEnergyBonusQuantity > 0)))
			{
				if (theHero != null)
				{
					if (theHero.GetEnergy() > 0)
					{
						theCardItem.AmplifySetEnergy();
						energySelector.TurnOn(theHero.GetEnergy(), _cardActive.EffectRepeatMaxBonus);
						DrawDeckPileLayer("Default");
						energyAssigned = 0;
						if (GameManager.Instance.IsMultiplayer())
						{
							if (NetworkManager.Instance.IsMaster())
							{
								if (coroutineSyncWaitingAction != null)
								{
									StopCoroutine(coroutineSyncWaitingAction);
								}
								coroutineSyncWaitingAction = StartCoroutine(ReloadCombatCo("waitingAction" + _cardActive.Id));
								while (!NetworkManager.Instance.AllPlayersReady("waitingAction" + _cardActive.Id))
								{
									yield return Globals.Instance.WaitForSeconds(0.01f);
								}
								if (coroutineSyncWaitingAction != null)
								{
									StopCoroutine(coroutineSyncWaitingAction);
								}
								if (Globals.Instance.ShowDebug)
								{
									Functions.DebugLogGD("Game ready, Everybody checked waitingAction" + _cardActive.Id, "net");
								}
								NetworkManager.Instance.PlayersNetworkContinue("waitingAction" + _cardActive.Id);
								yield return Globals.Instance.WaitForSeconds(0.01f);
							}
							else
							{
								NetworkManager.Instance.SetWaitingSyncro("waitingAction" + _cardActive.Id, status: true);
								NetworkManager.Instance.SetStatusReady("waitingAction" + _cardActive.Id);
								while (NetworkManager.Instance.WaitingSyncro["waitingAction" + _cardActive.Id])
								{
									yield return Globals.Instance.WaitForSeconds(0.01f);
								}
								if (Globals.Instance.ShowDebug)
								{
									Functions.DebugLogGD("waitingAction, we can continue!", "net");
								}
							}
						}
						waitingForCardEnergyAssignment = true;
						while (waitingForCardEnergyAssignment)
						{
							yield return Globals.Instance.WaitForSeconds(0.1f);
						}
					}
					theCardItem.discard = true;
					yield return Globals.Instance.WaitForSeconds(0.1f);
					theCardItem.PreDiscardCard();
					yield return Globals.Instance.WaitForSeconds(0.2f);
				}
				else
				{
					energyAssigned = 1;
					theCardItem.DiscardCard(discardedFromHand: true);
				}
			}
			else if ((IsBeginTournPhase && _cardActive.CardClass == Enums.CardClass.Special) || theNPC != null)
			{
				theCardItem.DiscardCard(discardedFromHand: false);
			}
			else if (_cardActive.DrawCard != 0 || _cardActive.DiscardCard != 0 || _cardActive.AddCard != 0 || _cardActive.LookCards != 0 || _cardActive.EffectRepeat > 1)
			{
				if (!theCardItem.CardIsPrediscarding())
				{
					theCardItem.PreDiscardCard();
					yield return Globals.Instance.WaitForSeconds(0.2f);
				}
			}
			else
			{
				theCardItem.DiscardCard(discardedFromHand: false);
			}
		}
		if (!isBeginTournPhase && !_automatic && GameManager.Instance.IsMultiplayer())
		{
			yield return Globals.Instance.WaitForSeconds(0.01f);
			string _cardForSync = _cardActive.InternalId;
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("**************************", "net");
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("WaitingSyncro castcard_" + _cardForSync, "net");
			}
			if (NetworkManager.Instance.IsMaster())
			{
				if (coroutineSyncCastCard != null)
				{
					StopCoroutine(coroutineSyncCastCard);
				}
				coroutineSyncCastCard = StartCoroutine(ReloadCombatCo("castcard_" + _cardForSync));
				while (!NetworkManager.Instance.AllPlayersReady("castcard_" + _cardForSync))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (coroutineSyncCastCard != null)
				{
					StopCoroutine(coroutineSyncCastCard);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked castcard_" + _cardForSync, "net");
				}
				SetRandomIndex(randomIndex);
				randomForIteration = randomIndex;
				NetworkManager.Instance.PlayersNetworkContinue("castcard_" + _cardForSync, randomIndex.ToString());
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("castcard_" + _cardForSync, status: true);
				NetworkManager.Instance.SetStatusReady("castcard_" + _cardForSync);
				while (NetworkManager.Instance.WaitingSyncro["castcard_" + _cardForSync])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (NetworkManager.Instance.netAuxValue != "")
				{
					SetRandomIndex(int.Parse(NetworkManager.Instance.netAuxValue));
					randomForIteration = randomIndex;
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("castcard_" + _cardForSync + ", we can continue!", "net");
				}
			}
		}
		if (_cardActive.DrawCard != 0 || _cardActive.DiscardCard != 0 || _cardActive.AddCard != 0 || _cardActive.LookCards != 0)
		{
			SetEventDirect("CastCardEvent" + comingId, automatic: false);
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("CastCardProcess Debug 4", "trace");
		}
		ResetControllerPositions();
		ResetControllerClickedCard();
		CardItem theCardItemPre = theCardItem;
		CardData _cardActivePre = _cardActive;
		Hero theCasterHeroPre = theCasterHero;
		Hero theHeroPre = theHero;
		NPC theCasterNPCPre = theCasterNPC;
		NPC theNPCPre = theNPC;
		Character theCasterCharacterPre = theCasterCharacter;
		Transform targetTransformPre = targetTransform;
		bool theCasterIsHeroPre = theCasterIsHero;
		string theCardItemPostDiscardPre = theCardItemPostDiscard;
		int _drawLoopsTotal = 1;
		bool _checkCardManipulationBeforeDraw = false;
		if (_cardActive.DrawCardSpecialValueGlobal)
		{
			_checkCardManipulationBeforeDraw = true;
			_drawLoopsTotal = 2;
		}
		for (int _drawLoopCurrent = 0; _drawLoopCurrent < _drawLoopsTotal; _drawLoopCurrent++)
		{
			int cardsNum;
			if (_drawLoopsTotal == 1 || _drawLoopCurrent == 1)
			{
				cardsNum = _cardActive.DrawCard;
				if (_checkCardManipulationBeforeDraw)
				{
					cardsNum = Functions.FuncRoundToInt(GetCardSpecialValue(_cardActive, 0, theHero, null, null, null, _IsPreview: false));
					if (_cardActive.EnergyRechargeSpecialValueGlobal && _cardActive.SpecialValueGlobal == Enums.CardSpecialValue.DiscardedCards && theHero != null && theHero.Alive)
					{
						theHero.ModifyEnergy(cardsNum, showScrollCombatText: true);
					}
				}
				if (cardsNum != 0)
				{
					gameStatus = "DrawingCards";
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD(gameStatus, "gamestatus");
					}
					int num2 = 10 - CountHeroHand();
					if (cardsNum == -1 || cardsNum > num2)
					{
						cardsNum = num2;
					}
					int num3 = CountHeroDeck() + CountHeroDiscard();
					if (cardsNum > num3)
					{
						cardsNum = num3;
					}
					for (int i = 0; i < cardsNum; i++)
					{
						gameStatus = "DrawingCards";
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("DrawingCards " + _cardActivePre.Id, "gamestatus");
						}
						if (castingCardBlocked.ContainsKey(_cardActivePre.InternalId))
						{
							castingCardBlocked[_cardActivePre.InternalId] = true;
						}
						else
						{
							castingCardBlocked.Add(_cardActivePre.InternalId, value: true);
						}
						NewCard(1, Enums.CardFrom.Deck, _cardActivePre.InternalId);
						int indexExtremeBlock = 0;
						int indexGameBusy = 0;
						while (castingCardBlocked.ContainsKey(_cardActivePre.InternalId) && castingCardBlocked[_cardActivePre.InternalId])
						{
							yield return Globals.Instance.WaitForSeconds(0.01f);
							if (GameManager.Instance.GetDeveloperMode() && indexExtremeBlock % 100 == 0)
							{
								if (Globals.Instance.ShowDebug)
								{
									Functions.DebugLogGD("[DRAWINGCARD] indexExtremeBlock" + indexExtremeBlock, "trace");
								}
								if (Globals.Instance.ShowDebug)
								{
									Functions.DebugLogGD("[DRAWINGCARD] indexGameBusy" + indexGameBusy, "trace");
								}
							}
							indexExtremeBlock++;
							if (!gameBusy)
							{
								indexGameBusy++;
								if (indexGameBusy > 200 && CountHeroDeck() + CountHeroDiscard() == 0)
								{
									if (Globals.Instance.ShowDebug)
									{
										Functions.DebugLogGD("[DRAWINGCARD] EXIT by indexGameBusy", "trace");
									}
									break;
								}
							}
							else
							{
								indexGameBusy = 0;
							}
							if (indexExtremeBlock > 700)
							{
								if (Globals.Instance.ShowDebug)
								{
									Functions.DebugLogGD("[DRAWINGCARD] EXIT by indexExtremeBlock", "trace");
								}
								break;
							}
						}
						if (theHero != null && !theHero.Alive)
						{
							yield break;
						}
						if (10 - CountHeroHand() == 0)
						{
							break;
						}
					}
					yield return Globals.Instance.WaitForSeconds(0.15f);
					gameStatus = "";
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD(gameStatus, "gamestatus");
					}
				}
				_cardActive = _cardActivePre;
				theCardItem = theCardItemPre;
				theCasterHero = theCasterHeroPre;
				theHero = theHeroPre;
				theCasterNPC = theCasterNPCPre;
				theNPC = theNPCPre;
				theCasterCharacter = theCasterCharacterPre;
				targetTransform = targetTransformPre;
				theCasterIsHero = theCasterIsHeroPre;
				theCardItemPostDiscard = theCardItemPostDiscardPre;
			}
			bool flag3 = true;
			if (heroActive > -1 && (TeamHero[heroActive] == null || !TeamHero[heroActive].Alive))
			{
				flag3 = false;
			}
			heroIndexWaitingForAddDiscard = heroActive;
			if (!(heroActive > -1 && flag3))
			{
				continue;
			}
			theCardItemPre = theCardItem;
			_cardActivePre = _cardActive;
			theCasterHeroPre = theCasterHero;
			theHeroPre = theHero;
			theCasterNPCPre = theCasterNPC;
			theNPCPre = theNPC;
			theCasterCharacterPre = theCasterCharacter;
			targetTransformPre = targetTransform;
			theCasterIsHeroPre = theCasterIsHero;
			theCardItemPostDiscardPre = theCardItemPostDiscard;
			if ((_drawLoopsTotal == 1 || _drawLoopCurrent == 1) && _cardActive.AddCard != 0)
			{
				cardsNum = _cardActive.AddCard;
				if (_cardActive.AddCardPlace == Enums.CardPlace.Hand && CountHeroHand() + cardsNum > 10)
				{
					cardsNum = 10 - CountHeroHand();
				}
				if (cardsNum > 0)
				{
					if (_cardActive.AddCardId != "")
					{
						Hero hero = null;
						if (targetTransform != null)
						{
							hero = GetHeroById(targetTransform.name);
						}
						bool flag4 = false;
						if (_cardActive.TargetSide == Enums.CardTargetSide.Friend)
						{
							if (hero == null && theCasterHero != null)
							{
								hero = theCasterHero;
							}
							for (int num4 = 0; num4 < 4; num4++)
							{
								if (TeamHero[num4] != null && TeamHero[num4].HeroItem != null && TeamHero[num4].Alive && (_cardActive.TargetType == Enums.CardTargetType.Global || (hero != null && hero.Id == TeamHero[num4].Id)))
								{
									CardData cardForModify = GetCardForModify(_cardActive, theCasterHero, TeamHero[num4]);
									GenerateNewCard(cardsNum, _cardActive.AddCardId, createCard: true, _cardActive.AddCardPlace, cardForModify, null, num4);
									flag4 = true;
								}
							}
						}
						else
						{
							CardData cardForModify2 = GetCardForModify(_cardActive, theCasterHero, hero);
							GenerateNewCard(cardsNum, _cardActive.AddCardId, createCard: true, _cardActive.AddCardPlace, cardForModify2);
							flag4 = true;
						}
						if (flag4)
						{
							yield return Globals.Instance.WaitForSeconds(0.1f);
							while (gameBusy)
							{
								yield return Globals.Instance.WaitForSeconds(0.01f);
							}
						}
					}
					else
					{
						gameStatus = "AddingCards";
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD(gameStatus, "gamestatus");
						}
						List<Enums.CardType> list = new List<Enums.CardType>();
						if (_cardActive.AddCardType != Enums.CardType.None && _cardActive.AddCardType != Enums.CardType.Enchantment && _cardActive.AddCardType != Enums.CardType.Boon && _cardActive.AddCardType != Enums.CardType.Injury)
						{
							if (!_cardActive.AddCardOnlyCheckAuxTypes)
							{
								list.Add(_cardActive.AddCardType);
							}
							for (int num5 = 0; num5 < _cardActive.AddCardTypeAux.Length; num5++)
							{
								if (_cardActive.AddCardTypeAux[num5] != Enums.CardType.None)
								{
									list.Add(_cardActive.AddCardTypeAux[num5]);
								}
							}
						}
						foreach (CardData.CardToGainTypeBasedOnHeroClass item10 in _cardActive.AddCardTypeBasedOnHeroClass)
						{
							if (item10.heroClass != theHero.HeroData.HeroSubClass.HeroClass && item10.heroClass != theHero.HeroData.HeroSubClass.HeroClassSecondary && item10.heroClass != theHero.HeroData.HeroSubClass.HeroClassThird)
							{
								continue;
							}
							foreach (Enums.CardType cardType in item10.cardTypes)
							{
								if (!list.Contains(cardType))
								{
									list.Add(cardType);
								}
							}
						}
						if (_cardActivePre.AddCardListBasedOnHeroClass.Count > 0 && theHero != null && theHero.IsHero)
						{
							List<CardData> list2 = new List<CardData>();
							foreach (CardData.CardToGainListBasedOnHeroClass item11 in _cardActivePre.AddCardListBasedOnHeroClass)
							{
								if (item11.heroClass != theHero.HeroData.HeroSubClass.HeroClass && item11.heroClass != theHero.HeroData.HeroSubClass.HeroClassSecondary && item11.heroClass != theHero.HeroData.HeroSubClass.HeroClassThird)
								{
									continue;
								}
								foreach (CardData cards in item11.cardsList)
								{
									if (cards.Id == "vanisheddiscover")
									{
										if (Instance.GetHeroVanishPile(theHero.HeroIndex).Count < cards.AddCard)
										{
											continue;
										}
										int num6 = 0;
										foreach (string item12 in Instance.GetHeroVanishPile(theHero.HeroIndex))
										{
											CardData cardData2 = Globals.Instance.GetCardData(item12);
											if (cardData2 != null && !cardData2.Vanish)
											{
												num6++;
											}
										}
										if (num6 < cards.AddCard)
										{
											continue;
										}
									}
									if (!list2.Contains(cards))
									{
										list2.Add(cards);
									}
								}
							}
							if (list2.Count > 0)
							{
								_cardActivePre.AddCardList = list2.ToArray();
								_cardActivePre.AddCardChoose = list2.Count;
								_cardActivePre.SetDescriptionNew(forceDescription: true);
							}
						}
						if (_cardActivePre.AddCardFromVanishPile && theHero != null && theHero.IsHero)
						{
							List<string> heroVanishPile = Instance.GetHeroVanishPile(theHero.HeroIndex);
							if (heroVanishPile.Count >= _cardActivePre.AddCard)
							{
								List<CardData> list3 = new List<CardData>();
								foreach (string item13 in heroVanishPile)
								{
									string id = item13.Split('_')[0];
									CardData cardData3 = Globals.Instance.GetCardData(id);
									if (cardData3 != null && !list3.Contains(cardData3) && !cardData3.Vanish && list3.Count() < _cardActivePre.AddCardChoose)
									{
										list3.Add(cardData3);
									}
								}
								if (list3.Count > 0)
								{
									_cardActivePre.AddCardList = list3.ToArray();
									_cardActivePre.AddCardChoose = list3.Count;
									_cardActivePre.SetDescriptionNew(forceDescription: true);
								}
							}
						}
						List<int> ValidCardsInDeck = new List<int>();
						List<string> ValidCardsStringInDeck = new List<string>();
						new List<string>();
						int num7 = 0;
						if (_cardActive.AddCardFrom == Enums.CardFrom.Deck)
						{
							num7 = CountHeroDeck();
						}
						else if (_cardActive.AddCardFrom == Enums.CardFrom.Discard)
						{
							num7 = CountHeroDiscard();
						}
						else if (_cardActive.AddCardFrom == Enums.CardFrom.Hand)
						{
							num7 = CountHeroHand();
						}
						else if (_cardActive.AddCardFrom == Enums.CardFrom.Vanish)
						{
							num7 = CountHeroVanish();
						}
						if (_cardActive.AddCardFrom != Enums.CardFrom.Game)
						{
							for (int num8 = 0; num8 < num7; num8++)
							{
								if (list.Count > 0)
								{
									CardData cardData4 = null;
									if (_cardActive.AddCardFrom == Enums.CardFrom.Deck)
									{
										cardData4 = GetCardData(HeroDeck[heroActive][num8]);
									}
									else if (_cardActive.AddCardFrom == Enums.CardFrom.Discard)
									{
										if (_cardActive.InternalId == HeroDeckDiscard[heroActive][num8])
										{
											continue;
										}
										cardData4 = GetCardData(HeroDeckDiscard[heroActive][num8]);
									}
									else if (_cardActive.AddCardFrom == Enums.CardFrom.Hand)
									{
										if (_cardActive.InternalId == HeroHand[heroActive][num8])
										{
											continue;
										}
										cardData4 = GetCardData(HeroHand[heroActive][num8]);
									}
									else if (_cardActive.AddCardFrom == Enums.CardFrom.Vanish)
									{
										if (_cardActive.InternalId == HeroDeckVanish[heroActive][num8])
										{
											continue;
										}
										cardData4 = GetCardData(HeroDeckVanish[heroActive][num8]);
										if (!cardData4.Playable)
										{
											continue;
										}
									}
									List<Enums.CardType> cardTypes = cardData4.GetCardTypes();
									bool flag5 = false;
									for (int num9 = 0; num9 < list.Count; num9++)
									{
										if (cardTypes.Contains(list[num9]))
										{
											flag5 = true;
											break;
										}
									}
									if (flag5)
									{
										ValidCardsInDeck.Add(num8);
									}
								}
								else if ((_cardActive.AddCardFrom != Enums.CardFrom.Discard || !(_cardActive.InternalId == HeroDeckDiscard[heroActive][num8])) && (_cardActive.AddCardFrom != Enums.CardFrom.Hand || !(_cardActive.InternalId == HeroHand[heroActive][num8])) && (_cardActive.AddCardFrom != Enums.CardFrom.Vanish || (!(_cardActive.InternalId == HeroDeckVanish[heroActive][num8]) && GetCardData(HeroDeckVanish[heroActive][num8]).Playable)))
								{
									ValidCardsInDeck.Add(num8);
								}
							}
						}
						else if (list.Count > 0)
						{
							List<string> list4 = new List<string>();
							string text = Enum.GetName(typeof(Enums.HeroClass), theHero.HeroData.HeroClass);
							string text2 = Enum.GetName(typeof(Enums.HeroClass), theHero.HeroData.HeroSubClass.HeroClassSecondary);
							string text3 = Enum.GetName(typeof(Enums.HeroClass), theHero.HeroData.HeroSubClass.HeroClassThird);
							_ = list.Count;
							bool flag6 = _cardActive.AddCardTypeBasedOnHeroClass.Count > 0;
							for (int num10 = 0; num10 < list.Count; num10++)
							{
								string text4 = Enum.GetName(typeof(Enums.CardType), list[num10]);
								string key = text + "_" + text4;
								string key2 = text2 + "_" + text4;
								string key3 = text3 + "_" + text4;
								int count;
								if (Globals.Instance.CardListByClassType.ContainsKey(key))
								{
									count = Globals.Instance.CardListByClassType[key].Count;
									for (int num11 = 0; num11 < count; num11++)
									{
										string text5 = Globals.Instance.CardListByClassType[key][num11];
										if (!list4.Contains(text5) && (!flag6 || list.Contains(GetCardData(text5).CardType)))
										{
											list4.Add(text5);
										}
									}
								}
								if (Globals.Instance.CardListByClassType.ContainsKey(key2))
								{
									count = Globals.Instance.CardListByClassType[key2].Count;
									for (int num12 = 0; num12 < count; num12++)
									{
										string text5 = Globals.Instance.CardListByClassType[key2][num12];
										if (!list4.Contains(text5) && (!flag6 || list.Contains(GetCardData(text5).CardType)))
										{
											list4.Add(text5);
										}
									}
								}
								if (!Globals.Instance.CardListByClassType.ContainsKey(key3))
								{
									continue;
								}
								count = Globals.Instance.CardListByClassType[key3].Count;
								for (int num13 = 0; num13 < count; num13++)
								{
									string text5 = Globals.Instance.CardListByClassType[key3][num13];
									if (!list4.Contains(text5) && (!flag6 || list.Contains(GetCardData(text5).CardType)))
									{
										list4.Add(text5);
									}
								}
							}
							if (list4.Count > 0)
							{
								if (_cardActive.AddCardChoose > 0)
								{
									List<string> UsedCardsId = new List<string>();
									Dictionary<Enums.CardType, int> dictionary = new Dictionary<Enums.CardType, int>();
									int num14 = Mathf.CeilToInt((float)_cardActive.AddCardChoose / (float)list.Count);
									bool flag7 = _cardActive.AddCardTypeBasedOnHeroClass.Count > 0;
									foreach (Enums.CardType item14 in list)
									{
										dictionary.Add(item14, 0);
									}
									for (int num15 = 0; num15 < _cardActive.AddCardChoose; num15++)
									{
										bool flag8 = false;
										while (!flag8)
										{
											string key4 = list4[GetRandomIntRange(0, list4.Count)];
											CardData cardData5 = Globals.Instance.Cards[key4];
											if (!(cardData5 != null) || (flag7 && dictionary.ContainsKey(cardData5.CardType) && dictionary[cardData5.CardType] == num14))
											{
												continue;
											}
											string cardByRarity = Functions.GetCardByRarity(GetRandomIntRange(0, 100), cardData5);
											if (list4.Contains(cardByRarity) && !UsedCardsId.Contains(cardByRarity) && Globals.Instance.Cards[cardByRarity] != null)
											{
												string item = CreateCardInDictionary(Globals.Instance.Cards[cardByRarity].Id);
												ValidCardsStringInDeck.Add(item);
												flag8 = true;
												if (flag7)
												{
													dictionary[cardData5.CardType]++;
												}
												UsedCardsId.Add(cardByRarity);
											}
										}
									}
								}
								else
								{
									List<string> UsedCardsId = new List<string>();
									for (int num16 = 0; num16 < cardsNum; num16++)
									{
										bool flag9 = false;
										while (!flag9)
										{
											string key5 = list4[GetRandomIntRange(0, list4.Count)];
											CardData cardData6 = Globals.Instance.Cards[key5];
											if (cardData6 != null)
											{
												string cardByRarity2 = Functions.GetCardByRarity(GetRandomIntRange(0, 100), cardData6);
												if (list4.Contains(cardByRarity2) && !UsedCardsId.Contains(cardByRarity2) && Globals.Instance.Cards[cardByRarity2] != null)
												{
													string item2 = CreateCardInDictionary(Globals.Instance.Cards[cardByRarity2].Id);
													ValidCardsStringInDeck.Add(item2);
													flag9 = true;
													UsedCardsId.Add(cardByRarity2);
												}
											}
										}
									}
								}
							}
						}
						else if (_cardActive.AddCardList != null && _cardActive.AddCardList.Length != 0)
						{
							for (int num17 = 0; num17 < _cardActive.AddCardList.Length; num17++)
							{
								string item3 = CreateCardInDictionary(_cardActive.AddCardList[num17].Id);
								ValidCardsStringInDeck.Add(item3);
							}
						}
						else
						{
							List<string> keyList = new List<string>();
							if (_cardActive.AddCardType == Enums.CardType.Boon)
							{
								int count2 = Globals.Instance.CardListByType[Enums.CardType.Boon].Count;
								for (int num18 = 0; num18 < count2; num18++)
								{
									string text6 = Globals.Instance.CardListByType[Enums.CardType.Boon][num18];
									if (text6 != "success" && !keyList.Contains(text6))
									{
										keyList.Add(text6);
									}
								}
							}
							else if (_cardActive.AddCardType == Enums.CardType.Injury)
							{
								int count3 = Globals.Instance.CardListByType[Enums.CardType.Injury].Count;
								for (int num19 = 0; num19 < count3; num19++)
								{
									string item4 = Globals.Instance.CardListByType[Enums.CardType.Injury][num19];
									if (!keyList.Contains(item4))
									{
										keyList.Add(item4);
									}
								}
							}
							else
							{
								string value = Enum.GetName(typeof(Enums.HeroClass), theHero.HeroData.HeroClass);
								Enums.CardClass key6 = (Enums.CardClass)Enum.Parse(typeof(Enums.CardClass), value);
								int count4 = Globals.Instance.CardListByClass[key6].Count;
								for (int num20 = 0; num20 < count4; num20++)
								{
									string item5 = Globals.Instance.CardListByClass[key6][num20];
									if (!keyList.Contains(item5))
									{
										keyList.Add(item5);
									}
								}
								if (theHero.HeroData.HeroSubClass.HeroClassSecondary != Enums.HeroClass.None)
								{
									value = Enum.GetName(typeof(Enums.HeroClass), theHero.HeroData.HeroSubClass.HeroClassSecondary);
									key6 = (Enums.CardClass)Enum.Parse(typeof(Enums.CardClass), value);
									count4 = Globals.Instance.CardListByClass[key6].Count;
									for (int num21 = 0; num21 < count4; num21++)
									{
										string item6 = Globals.Instance.CardListByClass[key6][num21];
										if (!keyList.Contains(item6))
										{
											keyList.Add(item6);
										}
									}
								}
							}
							if (keyList.Count > 0)
							{
								if (_cardActive.AddCardChoose > 0)
								{
									List<string> UsedCardsId = new List<string>();
									for (int indexGameBusy = 0; indexGameBusy < _cardActive.AddCardChoose; indexGameBusy++)
									{
										bool valid = false;
										while (!valid)
										{
											int randomIntRange = GetRandomIntRange(0, keyList.Count);
											string key7 = keyList[randomIntRange];
											if (Globals.Instance.CardListByClass[Enums.CardClass.Monster].Contains(Globals.Instance.Cards[key7].Id) || Globals.Instance.CardListByType[Enums.CardType.Injury].Contains(Globals.Instance.Cards[key7].Id))
											{
												continue;
											}
											CardData cardData7 = Globals.Instance.Cards[key7];
											if (cardData7 != null && Functions.CardIsPercentRarity(GetRandomIntRange(0, 100), cardData7))
											{
												string cardByRarity3 = Functions.GetCardByRarity(GetRandomIntRange(0, 100), cardData7);
												if (!UsedCardsId.Contains(cardByRarity3) && Globals.Instance.Cards[cardByRarity3] != null)
												{
													string item7 = CreateCardInDictionary(Globals.Instance.Cards[cardByRarity3].Id);
													ValidCardsStringInDeck.Add(item7);
													valid = true;
													UsedCardsId.Add(cardByRarity3);
													yield return null;
												}
											}
										}
									}
								}
								else
								{
									List<string> UsedCardsId = new List<string>();
									for (int num22 = 0; num22 < cardsNum; num22++)
									{
										bool flag10 = false;
										int num23 = 0;
										while (!flag10 && num23 < 1000)
										{
											num23++;
											string key8 = keyList[GetRandomIntRange(0, keyList.Count)];
											if (!Globals.Instance.Cards.ContainsKey(key8) || !(Globals.Instance.Cards[key8] != null) || Globals.Instance.CardListByClass[Enums.CardClass.Monster].Contains(Globals.Instance.Cards[key8].Id))
											{
												continue;
											}
											CardData cardData8 = Globals.Instance.Cards[key8];
											if (cardData8 != null && Functions.CardIsPercentRarity(GetRandomIntRange(0, 100), cardData8))
											{
												string cardByRarity4 = Functions.GetCardByRarity(GetRandomIntRange(0, 100), cardData8);
												if (cardByRarity4 != "" && !UsedCardsId.Contains(cardByRarity4) && Globals.Instance.Cards.ContainsKey(cardByRarity4) && Globals.Instance.Cards[cardByRarity4] != null)
												{
													string item8 = CreateCardInDictionary(Globals.Instance.Cards[cardByRarity4].Id);
													ValidCardsStringInDeck.Add(item8);
													flag10 = true;
													UsedCardsId.Add(cardByRarity4);
												}
											}
										}
									}
								}
							}
						}
						if (ValidCardsInDeck.Count > 0 || (_cardActive.AddCardFrom == Enums.CardFrom.Game && ValidCardsStringInDeck.Count > 0))
						{
							List<string> keyList = new List<string>();
							int indexGameBusy = ((_cardActive.AddCardChoose > 0) ? _cardActive.AddCardChoose : ((_cardActive.AddCardChoose != -1) ? cardsNum : ValidCardsInDeck.Count));
							if (ValidCardsStringInDeck.Count > 0)
							{
								for (int num24 = 0; num24 < ValidCardsStringInDeck.Count; num24++)
								{
									keyList.Add(ValidCardsStringInDeck[num24]);
								}
							}
							else
							{
								if (ValidCardsInDeck.Count > indexGameBusy)
								{
									ValidCardsInDeck = ValidCardsInDeck.ShuffleList();
									int num25 = ValidCardsInDeck.Count - indexGameBusy;
									for (int num26 = 0; num26 < num25; num26++)
									{
										ValidCardsInDeck.RemoveAt(0);
									}
								}
								else
								{
									indexGameBusy = ValidCardsInDeck.Count;
								}
								for (int num27 = 0; num27 < ValidCardsInDeck.Count; num27++)
								{
									if (_cardActive.AddCardFrom == Enums.CardFrom.Deck)
									{
										keyList.Add(HeroDeck[heroActive][ValidCardsInDeck[num27]]);
										continue;
									}
									if (_cardActive.AddCardFrom == Enums.CardFrom.Discard)
									{
										keyList.Add(HeroDeckDiscard[heroActive][ValidCardsInDeck[num27]]);
										continue;
									}
									if (_cardActive.AddCardFrom == Enums.CardFrom.Hand)
									{
										keyList.Add(HeroHand[heroActive][ValidCardsInDeck[num27]]);
										continue;
									}
									if (_cardActive.AddCardFrom == Enums.CardFrom.Vanish)
									{
										keyList.Add(HeroDeckVanish[heroActive][ValidCardsInDeck[num27]]);
										continue;
									}
									string key9 = Globals.Instance.Cards.ElementAt(ValidCardsInDeck[num27]).Key;
									string item9 = CreateCardInDictionary(Globals.Instance.Cards[key9].Id);
									keyList.Add(item9);
								}
							}
							if (_cardActive.AddCardChoose != 0)
							{
								if (cardsNum > indexGameBusy)
								{
									GlobalAddcardCardsNum = indexGameBusy;
								}
								else
								{
									GlobalAddcardCardsNum = cardsNum;
								}
								new List<GameObject>();
								CICardAddcard = new List<CardItem>();
								if (_cardActive.AddCardFrom == Enums.CardFrom.Hand)
								{
									deckCardsWindow.TurnOn(3, 1, indexGameBusy);
								}
								else
								{
									deckCardsWindow.TurnOn(3, _cardActive.AddCard, indexGameBusy);
								}
								GO_List = new List<GameObject>();
								cardGos.Clear();
								for (int indexExtremeBlock = 0; indexExtremeBlock < indexGameBusy; indexExtremeBlock++)
								{
									GameObject gameObject2 = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, deckCardsWindow.cardContainer);
									GO_List.Add(gameObject2);
									CardItem cardTutorial = gameObject2.GetComponent<CardItem>();
									gameObject2.name = "TMP_" + indexExtremeBlock;
									cardGos.Add(gameObject2.name, gameObject2);
									cardTutorial.SetCard(keyList[indexExtremeBlock], deckScale: false, theHero);
									AddCardModificationsForCardForShow(_cardActive, cardTutorial.CardData);
									cardTutorial.DrawEnergyCost(ActualCost: false);
									yield return Globals.Instance.WaitForSeconds(0.01f);
									cardTutorial.cardforaddcard = true;
									cardTutorial.AmplifyForSelection(indexExtremeBlock, indexGameBusy);
									cardTutorial.SetDestination(cardTutorial.GetDestination() - new Vector3(2.5f, -4.5f, 0f));
									cardTutorial.DisableTrail();
									cardTutorial.active = true;
									cardTutorial.HideRarityParticles();
									cardTutorial.HideCardIconParticles();
									if (IsYourTurnForAddDiscard())
									{
										cardTutorial.ShowKeyNum(_state: true, (indexExtremeBlock + 1).ToString());
									}
									yield return null;
								}
								if (GameManager.Instance.IsMultiplayer())
								{
									if (NetworkManager.Instance.IsMaster())
									{
										if (coroutineSyncWaitingAction != null)
										{
											StopCoroutine(coroutineSyncWaitingAction);
										}
										coroutineSyncWaitingAction = StartCoroutine(ReloadCombatCo("waitingAction" + _cardActive.Id));
										while (!NetworkManager.Instance.AllPlayersReady("waitingAction" + _cardActive.Id))
										{
											yield return Globals.Instance.WaitForSeconds(0.01f);
										}
										if (coroutineSyncWaitingAction != null)
										{
											StopCoroutine(coroutineSyncWaitingAction);
										}
										if (Globals.Instance.ShowDebug)
										{
											Functions.DebugLogGD("Game ready, Everybody checked waitingAction" + _cardActive.Id, "net");
										}
										NetworkManager.Instance.PlayersNetworkContinue("waitingAction" + _cardActive.Id);
										yield return Globals.Instance.WaitForSeconds(0.01f);
									}
									else
									{
										NetworkManager.Instance.SetWaitingSyncro("waitingAction" + _cardActive.Id, status: true);
										NetworkManager.Instance.SetStatusReady("waitingAction" + _cardActive.Id);
										while (NetworkManager.Instance.WaitingSyncro["waitingAction" + _cardActive.Id])
										{
											yield return Globals.Instance.WaitForSeconds(0.01f);
										}
										if (Globals.Instance.ShowDebug)
										{
											Functions.DebugLogGD("waitingAction, we can continue!", "net");
										}
									}
								}
								waitingForAddcardAssignment = true;
								while (waitingForAddcardAssignment)
								{
									yield return Globals.Instance.WaitForSeconds(0.1f);
								}
								DrawDeckScreenDestroy();
								deckCardsWindow.TurnOff();
								ValidCardsInDeck = new List<int>();
								ValidCardsStringInDeck = new List<string>();
								for (int indexExtremeBlock = 0; indexExtremeBlock < CICardAddcard.Count; indexExtremeBlock++)
								{
									if (!(CICardAddcard[indexExtremeBlock] != null) || !(CICardAddcard[indexExtremeBlock].CardData != null))
									{
										continue;
									}
									string internalId = CICardAddcard[indexExtremeBlock].CardData.InternalId;
									if (_cardActive.AddCardFrom == Enums.CardFrom.Deck)
									{
										for (int num28 = 0; num28 < CountHeroDeck(); num28++)
										{
											if (internalId == HeroDeck[heroActive][num28])
											{
												ValidCardsInDeck.Add(num28);
												break;
											}
										}
									}
									else if (_cardActive.AddCardFrom == Enums.CardFrom.Hand)
									{
										string id2 = CICardAddcard[indexExtremeBlock].CardData.Id;
										if (_cardActive.TargetSide == Enums.CardTargetSide.Friend)
										{
											Hero hero2 = null;
											if (targetTransform != null)
											{
												hero2 = GetHeroById(targetTransform.name);
											}
											if (hero2 != null)
											{
												for (int num29 = 0; num29 < 4; num29++)
												{
													if (TeamHero[num29] != null && TeamHero[num29].Alive && hero2.Id == TeamHero[num29].Id)
													{
														CardData cardForModify3 = GetCardForModify(_cardActive, theCasterHero, TeamHero[num29]);
														GenerateNewCard(_cardActive.AddCard, id2, createCard: true, _cardActive.AddCardPlace, cardForModify3, CICardAddcard[indexExtremeBlock].CardData, num29);
														break;
													}
												}
											}
										}
										else
										{
											CardData cardForModify4 = GetCardForModify(_cardActive, theCasterHero, null);
											GenerateNewCard(_cardActive.AddCard, id2, createCard: true, _cardActive.AddCardPlace, cardForModify4, CICardAddcard[indexExtremeBlock].CardData);
										}
										yield return Globals.Instance.WaitForSeconds(0.1f);
										while (gameBusy)
										{
											yield return Globals.Instance.WaitForSeconds(0.1f);
										}
									}
									else if (_cardActive.AddCardFrom == Enums.CardFrom.Discard)
									{
										for (int num30 = 0; num30 < CountHeroDiscard(); num30++)
										{
											if (internalId == HeroDeckDiscard[heroActive][num30])
											{
												ValidCardsInDeck.Add(num30);
												break;
											}
										}
									}
									else if (_cardActive.AddCardFrom == Enums.CardFrom.Vanish)
									{
										for (int num31 = 0; num31 < CountHeroVanish(); num31++)
										{
											if (internalId == HeroDeckVanish[heroActive][num31])
											{
												ValidCardsInDeck.Add(num31);
												break;
											}
										}
									}
									else
									{
										ValidCardsStringInDeck.Add(internalId);
									}
								}
							}
							ValidCardsInDeck.Sort();
							List<string> list5 = new List<string>();
							for (int num32 = ValidCardsInDeck.Count - 1; num32 >= 0; num32--)
							{
								if (_cardActive.AddCardFrom == Enums.CardFrom.Deck)
								{
									CardData cardData9 = GetCardData(HeroDeck[heroActive][ValidCardsInDeck[num32]]);
									AddCardModificationsForCard(_cardActive, cardData9);
									list5.Add(HeroDeck[heroActive][ValidCardsInDeck[num32]]);
									HeroDeck[heroActive].RemoveAt(ValidCardsInDeck[num32]);
									WriteDeckCounter();
								}
								else if (_cardActive.AddCardFrom == Enums.CardFrom.Discard)
								{
									CardData cardData9 = GetCardData(HeroDeckDiscard[heroActive][ValidCardsInDeck[num32]]);
									AddCardModificationsForCard(_cardActive, cardData9);
									GetHeroHeroActive().traitClass.UpdateCostIf3Runes(heroActive, _cardActive);
									RemoveCardFromDiscardPile(HeroDeckDiscard[heroActive][ValidCardsInDeck[num32]]);
									list5.Add(HeroDeckDiscard[heroActive][ValidCardsInDeck[num32]]);
									HeroDeckDiscard[heroActive].RemoveAt(ValidCardsInDeck[num32]);
									RedoDiscardPile();
								}
								else if (_cardActive.AddCardFrom == Enums.CardFrom.Vanish)
								{
									CardData cardData9 = GetCardData(HeroDeckVanish[heroActive][ValidCardsInDeck[num32]]);
									AddCardModificationsForCard(_cardActive, cardData9);
									list5.Add(HeroDeckVanish[heroActive][ValidCardsInDeck[num32]]);
									HeroDeckVanish[heroActive].RemoveAt(ValidCardsInDeck[num32]);
								}
							}
							if (_cardActive.AddCardFrom != Enums.CardFrom.Game)
							{
								if (_cardActive.AddCardFrom != Enums.CardFrom.Hand)
								{
									if (_cardActive.AddCardPlace == Enums.CardPlace.Hand)
									{
										for (int num33 = list5.Count - 1; num33 >= 0; num33--)
										{
											HeroDeck[heroActive].Insert(0, list5[num33]);
										}
										for (int num34 = 0; num34 < list5.Count; num34++)
										{
											if (num34 < 10 - CountHeroHand())
											{
												CreateLogEntry(_initial: true, "toHand:" + logDictionary.Count, list5[num34], TeamHero[heroActive], null, null, null, currentRound);
											}
										}
										NewCard(ValidCardsInDeck.Count, _cardActive.AddCardFrom);
										while (cardsWaitingForReset > 0)
										{
											yield return Globals.Instance.WaitForSeconds(0.1f);
										}
									}
									else if (_cardActive.AddCardPlace == Enums.CardPlace.Discard)
									{
										for (int num35 = list5.Count - 1; num35 >= 0; num35--)
										{
											MoveCardTo(1, list5[num35], Enums.CardPlace.Discard);
											CreateLogEntry(_initial: true, "toDiscard:" + logDictionary.Count, list5[num35], TeamHero[heroActive], null, null, null, currentRound);
										}
									}
									else if (_cardActive.AddCardPlace == Enums.CardPlace.BottomDeck || _cardActive.AddCardPlace == Enums.CardPlace.TopDeck || _cardActive.AddCardPlace == Enums.CardPlace.RandomDeck)
									{
										for (int num36 = list5.Count - 1; num36 >= 0; num36--)
										{
											MoveCardTo(1, list5[num36], _cardActive.AddCardPlace);
											if (_cardActive.AddCardPlace == Enums.CardPlace.BottomDeck)
											{
												CreateLogEntry(_initial: true, "toBottomDeck:" + logDictionary.Count, list5[num36], TeamHero[heroActive], null, null, null, currentRound);
											}
											else if (_cardActive.AddCardPlace == Enums.CardPlace.TopDeck)
											{
												CreateLogEntry(_initial: true, "toTopDeck:" + logDictionary.Count, list5[num36], TeamHero[heroActive], null, null, null, currentRound);
											}
											else if (_cardActive.AddCardPlace == Enums.CardPlace.RandomDeck)
											{
												CreateLogEntry(_initial: true, "toDeck:" + logDictionary.Count, list5[num36], TeamHero[heroActive], null, null, null, currentRound);
											}
										}
									}
									yield return Globals.Instance.WaitForSeconds(0.5f);
								}
							}
							else if (ValidCardsStringInDeck.Count > 0)
							{
								int indexExtremeBlock = -1;
								Hero _targetHero = null;
								if (targetTransform != null)
								{
									_targetHero = GetHeroById(targetTransform.name);
								}
								if (_cardActive.TargetSide == Enums.CardTargetSide.Friend)
								{
									if (_targetHero == null && theCasterHero != null)
									{
										_targetHero = theCasterHero;
									}
									if (_targetHero != null)
									{
										for (int num37 = 0; num37 < 4; num37++)
										{
											if (TeamHero[num37] != null && TeamHero[num37].HeroItem != null && TeamHero[num37].Alive && _targetHero.Id == TeamHero[num37].Id)
											{
												indexExtremeBlock = num37;
												break;
											}
										}
									}
								}
								if (_cardActive.AddCardChoose != 0)
								{
									for (int i = 0; i < ValidCardsStringInDeck.Count; i++)
									{
										CardData cardForModify5 = GetCardForModify(_cardActive, theCasterHero, _targetHero);
										GenerateNewCard(1, ValidCardsStringInDeck[i], createCard: false, _cardActive.AddCardPlace, cardForModify5, null, indexExtremeBlock);
										yield return Globals.Instance.WaitForSeconds(0.1f);
										while (gameBusy)
										{
											yield return Globals.Instance.WaitForSeconds(0.01f);
										}
									}
								}
								else
								{
									for (int i = 0; i < ValidCardsStringInDeck.Count; i++)
									{
										CardData cardForModify6 = GetCardForModify(_cardActive, theCasterHero, _targetHero);
										GenerateNewCard(1, ValidCardsStringInDeck[i], createCard: false, _cardActive.AddCardPlace, cardForModify6, null, indexExtremeBlock);
										yield return Globals.Instance.WaitForSeconds(0.1f);
										while (gameBusy)
										{
											yield return Globals.Instance.WaitForSeconds(0.01f);
										}
									}
								}
							}
						}
					}
				}
				gameStatus = "";
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(gameStatus, "gamestatus");
				}
			}
			if ((_drawLoopsTotal == 1 || _drawLoopCurrent == 0) && (_cardActive.DiscardCard != 0 || _cardActive.SpecialValueGlobal == Enums.CardSpecialValue.DiscardedCards || _cardActive.SpecialValueGlobal == Enums.CardSpecialValue.VanishedCards))
			{
				if (_cardActive.DrawCard != 0)
				{
					yield return Globals.Instance.WaitForSeconds(0.05f);
					RepositionCards();
				}
				gameStatus = "DiscardingCards";
				GlobalDiscardCardsNum = _cardActive.DiscardCard;
				if (GlobalDiscardCardsNum == -1)
				{
					GlobalDiscardCardsNum = 10;
				}
				yield return Globals.Instance.WaitForSeconds(0.05f);
				List<Enums.CardType> list6 = new List<Enums.CardType>();
				if (_cardActive.DiscardCardType != Enums.CardType.None)
				{
					list6.Add(_cardActive.DiscardCardType);
					for (int num38 = 0; num38 < _cardActive.DiscardCardTypeAux.Length; num38++)
					{
						if (_cardActive.DiscardCardTypeAux[num38] != Enums.CardType.None)
						{
							list6.Add(_cardActive.DiscardCardTypeAux[num38]);
						}
					}
				}
				List<string> list7 = new List<string>();
				for (int num39 = 0; num39 < cardItemTable.Count; num39++)
				{
					CardItem cardItem2 = cardItemTable[num39];
					if (cardItem2.InternalId == theCardItem.InternalId)
					{
						continue;
					}
					if (list6.Count > 0)
					{
						List<Enums.CardType> cardTypes2 = cardItem2.CardData.GetCardTypes();
						for (int num40 = 0; num40 < cardTypes2.Count; num40++)
						{
							if (list6.Contains(cardTypes2[num40]))
							{
								list7.Add(cardItem2.InternalId);
								break;
							}
						}
					}
					else
					{
						list7.Add(cardItem2.InternalId);
					}
				}
				bool flag11 = _cardActive.DiscardCardAutomatic;
				if (GlobalDiscardCardsNum >= list7.Count)
				{
					GlobalDiscardCardsNum = list7.Count;
					flag11 = true;
				}
				discardNumDecidedByThePlayer = false;
				if (_cardActive.SpecialValueGlobal == Enums.CardSpecialValue.DiscardedCards || _cardActive.SpecialValueGlobal == Enums.CardSpecialValue.VanishedCards)
				{
					flag11 = false;
					GlobalDiscardCardsNum = 10;
					discardNumDecidedByThePlayer = true;
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("--->" + GlobalDiscardCardsNum);
				}
				if (GlobalDiscardCardsNum > 0)
				{
					if (flag11)
					{
						List<CardItem> CIAutomatic = new List<CardItem>();
						if (GlobalDiscardCardsNum != list7.Count)
						{
							for (int num41 = 0; num41 < GlobalDiscardCardsNum; num41++)
							{
								int randomIntRange2 = GetRandomIntRange(0, list7.Count);
								for (int num42 = 0; num42 < cardItemTable.Count; num42++)
								{
									CardItem cardItem3 = cardItemTable[num42];
									if (cardItem3.InternalId == list7[randomIntRange2])
									{
										CIAutomatic.Add(cardItem3);
										list7.RemoveAt(randomIntRange2);
										break;
									}
								}
							}
						}
						else
						{
							for (int num43 = 0; num43 < GlobalDiscardCardsNum; num43++)
							{
								for (int num44 = 0; num44 < cardItemTable.Count; num44++)
								{
									CardItem cardItem4 = cardItemTable[num44];
									if (cardItem4.InternalId == list7[num43])
									{
										CIAutomatic.Add(cardItem4);
										break;
									}
								}
							}
						}
						cardsNum = CIAutomatic.Count;
						if (cardsNum > 0)
						{
							GameManager.Instance.PlayLibraryAudio("castnpccard");
							for (int num45 = 0; num45 < cardsNum; num45++)
							{
								CIAutomatic[num45].CenterToDiscard();
							}
							yield return Globals.Instance.WaitForSeconds(0.75f);
							for (int indexGameBusy = 0; indexGameBusy < cardsNum; indexGameBusy++)
							{
								CIAutomatic[indexGameBusy].DiscardCard(discardedFromHand: true, _cardActive.DiscardCardPlace);
								RepositionCards();
								CreateLogEntry(_initial: true, "toDiscard:" + logDictionary.Count, CIAutomatic[indexGameBusy].CardData.InternalId, TeamHero[heroActive], null, null, null, currentRound);
								yield return Globals.Instance.WaitForSeconds(0.1f);
							}
							yield return Globals.Instance.WaitForSeconds(0.25f);
						}
					}
					else
					{
						CICardDiscard = new List<CardItem>();
						discardSelector.TurnOn(_cardActive.DiscardCardPlace, discardNumDecidedByThePlayer);
						Transform cardContainer = discardSelector.cardContainer;
						for (int num46 = 0; num46 < list7.Count; num46++)
						{
							for (int num47 = 0; num47 < cardItemTable.Count; num47++)
							{
								CardItem cardItem5 = cardItemTable[num47];
								if (cardItem5.InternalId == list7[num46])
								{
									cardItem5.cardfordiscard = true;
									cardItem5.transform.parent = cardContainer;
									cardItem5.AmplifyForSelection(num46, list7.Count);
									if (IsYourTurnForAddDiscard())
									{
										cardItem5.ShowKeyNum(_state: true, (num46 + 1).ToString());
									}
									break;
								}
							}
						}
						if (GameManager.Instance.IsMultiplayer())
						{
							if (NetworkManager.Instance.IsMaster())
							{
								if (coroutineSyncWaitingAction != null)
								{
									StopCoroutine(coroutineSyncWaitingAction);
								}
								coroutineSyncWaitingAction = StartCoroutine(ReloadCombatCo("waitingAction" + _cardActive.Id));
								while (!NetworkManager.Instance.AllPlayersReady("waitingAction" + _cardActive.Id))
								{
									yield return Globals.Instance.WaitForSeconds(0.01f);
								}
								if (coroutineSyncWaitingAction != null)
								{
									StopCoroutine(coroutineSyncWaitingAction);
								}
								if (Globals.Instance.ShowDebug)
								{
									Functions.DebugLogGD("Game ready, Everybody checked waitingAction" + _cardActive.Id, "net");
								}
								NetworkManager.Instance.PlayersNetworkContinue("waitingAction" + _cardActive.Id);
								yield return Globals.Instance.WaitForSeconds(0.01f);
							}
							else
							{
								NetworkManager.Instance.SetWaitingSyncro("waitingAction" + _cardActive.Id, status: true);
								NetworkManager.Instance.SetStatusReady("waitingAction" + _cardActive.Id);
								while (NetworkManager.Instance.WaitingSyncro["waitingAction" + _cardActive.Id])
								{
									yield return Globals.Instance.WaitForSeconds(0.01f);
								}
								if (Globals.Instance.ShowDebug)
								{
									Functions.DebugLogGD("waitingAction, we can continue!", "net");
								}
							}
						}
						waitingForDiscardAssignment = true;
						while (waitingForDiscardAssignment)
						{
							yield return Globals.Instance.WaitForSeconds(0.1f);
						}
						GlobalDiscardCardsNum = 0;
						for (int num48 = cardItemTable.Count - 1; num48 >= 0; num48--)
						{
							CardItem cardItem6 = cardItemTable[num48];
							if (CICardDiscard.Contains(cardItem6))
							{
								cardItem6.cardfordiscard = false;
								cardItem6.DiscardCard(discardedFromHand: true, _cardActive.DiscardCardPlace);
								GlobalDiscardCardsNum++;
								CreateLogEntry(_initial: true, "toDiscard:" + logDictionary.Count, cardItem6.CardData.InternalId, TeamHero[heroActive], null, null, null, currentRound);
							}
						}
						yield return Globals.Instance.WaitForSeconds(0.01f);
						RepositionCards();
						discardSelector.TurnOff();
						for (int num49 = 0; num49 < cardItemTable.Count; num49++)
						{
							CardItem cardItem7 = cardItemTable[num49];
							if (cardItem7.InternalId != theCardItem.InternalId)
							{
								cardItem7.cardfordiscard = false;
								cardItem7.transform.parent = GO_Hand.transform;
								cardItem7.RestoreCard();
							}
						}
						CICardDiscard.Clear();
						if (GameManager.Instance.IsMultiplayer())
						{
							yield return Globals.Instance.WaitForSeconds(0.2f);
						}
						else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
						{
							yield return Globals.Instance.WaitForSeconds(0.3f);
						}
						else
						{
							yield return Globals.Instance.WaitForSeconds(0.1f);
						}
					}
				}
				gameStatus = "";
			}
			if ((_drawLoopsTotal != 1 && _drawLoopCurrent != 1) || _cardActive.LookCards == 0)
			{
				continue;
			}
			gameStatus = "LookingCards";
			if (targetTransform == null)
			{
				List<Transform> instaCastTransformList2 = GetInstaCastTransformList(_cardActive);
				if (instaCastTransformList2.Count == 0)
				{
					yield break;
				}
				targetTransform = instaCastTransformList2[0];
			}
			cardsNum = (heroIndexWaitingForAddDiscard = GetIndexForChar(targetTransform.GetComponent<HeroItem>().Hero));
			if (cardsNum > -1 && HeroDeck[cardsNum].Count > 0)
			{
				if (_cardActive.LookCardsDiscardUpTo > 0)
				{
					GlobalDiscardCardsNum = _cardActive.LookCardsDiscardUpTo;
				}
				else if (_cardActive.LookCardsVanishUpTo > 0)
				{
					GlobalDiscardCardsNum = _cardActive.LookCardsVanishUpTo;
				}
				else
				{
					GlobalDiscardCardsNum = _cardActive.LookCards;
				}
				List<string> UsedCardsId = new List<string>();
				int num50 = _cardActive.LookCards;
				if (num50 == -1)
				{
					for (int num51 = 0; num51 < HeroDeck[cardsNum].Count; num51++)
					{
						UsedCardsId.Add(HeroDeck[cardsNum][num51]);
					}
					UsedCardsId.Sort();
				}
				else
				{
					if (num50 > HeroDeck[cardsNum].Count)
					{
						num50 = HeroDeck[cardsNum].Count;
					}
					for (int num52 = 0; num52 < num50; num52++)
					{
						UsedCardsId.Add(HeroDeck[cardsNum][num52]);
					}
				}
				int indexGameBusy = UsedCardsId.Count;
				deckCardsWindow.TurnOn(2, GlobalDiscardCardsNum, indexGameBusy, _cardActive.LookCardsVanishUpTo > 0);
				List<CardItem> CIAutomatic = new List<CardItem>();
				CICardDiscard = new List<CardItem>();
				cardGos.Clear();
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(indexGameBusy.ToString());
				}
				for (int indexExtremeBlock = 0; indexExtremeBlock < indexGameBusy; indexExtremeBlock++)
				{
					GameObject gameObject3 = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, deckCardsWindow.cardContainer);
					CardItem cardTutorial = gameObject3.GetComponent<CardItem>();
					gameObject3.name = "TMP_" + indexExtremeBlock;
					cardGos.Add(gameObject3.name, gameObject3);
					CIAutomatic.Add(cardTutorial);
					cardTutorial.SetCard(UsedCardsId[indexExtremeBlock], deckScale: false, TeamHero[cardsNum]);
					yield return Globals.Instance.WaitForSeconds(0.01f);
					cardTutorial.cardfordiscard = true;
					cardTutorial.AmplifyForSelection(indexExtremeBlock, indexGameBusy);
					cardTutorial.SetDestination(cardTutorial.GetDestination() - new Vector3(2.5f, -4.5f, 0f));
					cardTutorial.DisableTrail();
					cardTutorial.active = true;
					cardTutorial.HideRarityParticles();
					cardTutorial.HideCardIconParticles();
					if (IsYourTurnForAddDiscard())
					{
						cardTutorial.ShowKeyNum(_state: true, (indexExtremeBlock + 1).ToString());
					}
					yield return null;
				}
				if (GameManager.Instance.IsMultiplayer())
				{
					if (NetworkManager.Instance.IsMaster())
					{
						if (coroutineSyncWaitingAction != null)
						{
							StopCoroutine(coroutineSyncWaitingAction);
						}
						coroutineSyncWaitingAction = StartCoroutine(ReloadCombatCo("waitingAction" + _cardActive.Id));
						while (!NetworkManager.Instance.AllPlayersReady("waitingAction" + _cardActive.Id))
						{
							yield return Globals.Instance.WaitForSeconds(0.01f);
						}
						if (coroutineSyncWaitingAction != null)
						{
							StopCoroutine(coroutineSyncWaitingAction);
						}
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("Game ready, Everybody checked waitingAction" + _cardActive.Id, "net");
						}
						NetworkManager.Instance.PlayersNetworkContinue("waitingAction" + _cardActive.Id);
						yield return Globals.Instance.WaitForSeconds(0.01f);
					}
					else
					{
						NetworkManager.Instance.SetWaitingSyncro("waitingAction" + _cardActive.Id, status: true);
						NetworkManager.Instance.SetStatusReady("waitingAction" + _cardActive.Id);
						while (NetworkManager.Instance.WaitingSyncro["waitingAction" + _cardActive.Id])
						{
							yield return Globals.Instance.WaitForSeconds(0.01f);
						}
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("waitingAction, we can continue!", "net");
						}
					}
				}
				waitingForLookDiscardWindow = true;
				while (waitingForLookDiscardWindow)
				{
					yield return Globals.Instance.WaitForSeconds(0.1f);
				}
				if (CICardDiscard.Count > 0)
				{
					for (int num53 = 0; num53 < CICardDiscard.Count; num53++)
					{
						CardItem cardItem8 = CICardDiscard[num53];
						cardItem8.cardfordiscard = false;
						HeroDeck[cardsNum].Remove(cardItem8.CardData.InternalId);
						if (_cardActive.LookCardsVanishUpTo > 0)
						{
							HeroDeckVanish[cardsNum].Add(cardItem8.CardData.InternalId);
							GlobalVanishCardsNum++;
						}
						else
						{
							HeroDeckDiscard[cardsNum].Add(cardItem8.CardData.InternalId);
						}
						if (_cardActive.LookCardsVanishUpTo > 0)
						{
							CreateLogEntry(_initial: true, "toVanish:" + logDictionary.Count, cardItem8.CardData.InternalId, TeamHero[cardsNum], null, null, null, currentRound);
						}
						else if (_cardActive.DiscardCardPlace == Enums.CardPlace.Discard)
						{
							CreateLogEntry(_initial: true, "toDiscard:" + logDictionary.Count, cardItem8.CardData.InternalId, TeamHero[cardsNum], null, null, null, currentRound);
						}
						else
						{
							CreateLogEntry(_initial: true, "toTopDeck:" + logDictionary.Count, cardItem8.CardData.InternalId, TeamHero[cardsNum], null, null, null, currentRound);
						}
						if (heroActive == cardsNum)
						{
							cardItem8.DiscardCard(discardedFromHand: false, _cardActive.DiscardCardPlace);
						}
						else
						{
							cardItem8.DiscardCard(discardedFromHand: false, Enums.CardPlace.Vanish);
						}
						CIAutomatic.Remove(cardItem8);
					}
					DrawDeckPile(CountHeroDeck() + 1);
					DrawDiscardPileCardNumeral();
				}
				for (int num54 = 0; num54 < CIAutomatic.Count; num54++)
				{
					if (heroActive == cardsNum)
					{
						CIAutomatic[num54].DiscardCard(discardedFromHand: false, Enums.CardPlace.TopDeck);
					}
					else
					{
						CIAutomatic[num54].DiscardCard(discardedFromHand: false, Enums.CardPlace.Vanish);
					}
				}
				heroIndexWaitingForAddDiscard = -1;
				deckCardsWindow.TurnOff();
				yield return Globals.Instance.WaitForSeconds(0.5f);
			}
			gameStatus = "";
		}
		_cardActive = _cardActivePre;
		theCardItem = theCardItemPre;
		theCasterHero = theCasterHeroPre;
		theHero = theHeroPre;
		theCasterNPC = theCasterNPCPre;
		theNPC = theNPCPre;
		theCasterCharacter = theCasterCharacterPre;
		targetTransform = targetTransformPre;
		theCasterIsHero = theCasterIsHeroPre;
		theCardItemPostDiscard = theCardItemPostDiscardPre;
		heroIndexWaitingForAddDiscard = -1;
		if (_cardActive.SummonUnit != null)
		{
			int _drawLoopCurrent = _cardActive.SummonUnitNum;
			if (_drawLoopCurrent == 0)
			{
				_drawLoopCurrent = 3;
			}
			for (int cardsNum = 0; cardsNum < _drawLoopCurrent; cardsNum++)
			{
				if (theCasterNPC != null)
				{
					CreateNPC(_cardActive.SummonUnit, _cardActive.EffectTarget, -1, generateFromReload: false, "", _cardActive, theCasterNPC.InternalId, 0f, replaceCharacter: false, isSummon: true);
				}
				else
				{
					CreateNPC(_cardActive.SummonUnit, _cardActive.EffectTarget, -1, generateFromReload: false, "", _cardActive, "", 0f, replaceCharacter: false, isSummon: true);
				}
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
			if (_cardActive != null && _cardActive.Metamorph)
			{
				yield return Globals.Instance.WaitForSeconds(0.25f);
				SetEventDirect("CastCardEvent" + comingId, automatic: false);
				CreateLogCastCard(_status: false, _cardActive, _uniqueCastId, theHero, theNPC, null, null);
				EndTurn();
				yield break;
			}
		}
		if (_cardActive.DrawCard != 0 || _cardActive.DiscardCard != 0 || _cardActive.AddCard != 0 || _cardActive.LookCards != 0)
		{
			SetEventDirect("CastCardEvent" + comingId, automatic: false, add: true);
		}
		if (theCardItemPostDiscard != "")
		{
			DiscardItem(theCardItemPostDiscard);
		}
		if (cardIteration.ContainsKey(_cardActive.InternalId))
		{
			cardIteration[_cardActive.InternalId] = 0;
		}
		else
		{
			cardIteration.Add(_cardActive.InternalId, 0);
		}
		if (theCasterNPC != null)
		{
			theHero = null;
		}
		if (!_cardActive.AutoplayDraw && !_cardActive.AutoplayEndTurn && (_cardActive.CardClass != Enums.CardClass.Special || (_cardActive.CardClass == Enums.CardClass.Special && _cardActive.Playable)))
		{
			if (theCasterHero != null)
			{
				for (int num55 = theHero.AuraList.Count - 1; num55 >= 0; num55--)
				{
					if (theHero != null && theHero.AuraList[num55] != null && theHero.AuraList[num55].ACData != null && theHero.AuraList[num55].ACData.Id == "stealth")
					{
						if ((_cardActive.HasCardType(Enums.CardType.Skill) || _cardActive.HasCardType(Enums.CardType.Enchantment)) && theHero.HaveTrait("veilofshadows") && (activatedTraits == null || !activatedTraits.ContainsKey("veilofshadows") || activatedTraits["veilofshadows"] < 2))
						{
							theHero.DoTraitFunction("veilofshadows");
							break;
						}
						theHero.SetAura(theCasterCharacter, Globals.Instance.GetAuraCurseData("stealthbonus"), theHero.AuraList[num55].AuraCharges, fromTrait: false, _cardActive.CardClass);
						theHero.HealAuraCurse(Globals.Instance.GetAuraCurseData("stealth"));
						break;
					}
				}
			}
			else if (theCasterNPC != null)
			{
				for (int num56 = theNPC.AuraList.Count - 1; num56 >= 0; num56--)
				{
					if (theNPC != null && theNPC.AuraList[num56] != null && theNPC.AuraList[num56].ACData != null && theNPC.AuraList[num56].ACData.Id == "stealth")
					{
						theNPC.SetAura(theCasterCharacter, Globals.Instance.GetAuraCurseData("stealthbonus"), theNPC.AuraList[num56].AuraCharges, fromTrait: false, _cardActive.CardClass);
						theNPC.HealAuraCurse(Globals.Instance.GetAuraCurseData("stealth"));
						break;
					}
				}
			}
		}
		Hero hero3 = null;
		NPC targetNPC = null;
		if (targetTransform != null && targetTransform.GetComponent<HeroItem>() != null)
		{
			hero3 = targetTransform.GetComponent<HeroItem>().Hero;
		}
		if (targetTransform != null && targetTransform.GetComponent<NPCItem>() != null)
		{
			targetNPC = targetTransform.GetComponent<NPCItem>().NPC;
		}
		int _cardSpecialValueGlobal = Functions.FuncRoundToInt(GetCardSpecialValue(_cardActive, 0, theHero, theNPC, hero3, targetNPC, _IsPreview: false));
		cardItemActive = theCardItem;
		if (_cardActive.TargetType == Enums.CardTargetType.Global)
		{
			int _drawLoopCurrent = 0;
			int cardsNum = 0;
			List<Hero> targetsHero = new List<Hero>();
			for (int num57 = 0; num57 < TeamHero.Length; num57++)
			{
				if (TeamHero[num57] != null && TeamHero[num57].Alive && TeamHero[num57].HeroItem != null && CheckTarget(TeamHero[num57].HeroItem.transform, _cardActive, theCasterIsHero))
				{
					targetsHero.Add(TeamHero[num57]);
					_drawLoopCurrent++;
				}
			}
			List<NPC> targetsNPC = new List<NPC>();
			for (int num58 = 0; num58 < TeamNPC.Length; num58++)
			{
				if (TeamNPC[num58] != null && TeamNPC[num58].Alive && TeamNPC[num58].NPCItem != null && CheckTarget(TeamNPC[num58].NPCItem.transform, _cardActive, theCasterIsHero))
				{
					targetsNPC.Add(TeamNPC[num58]);
					cardsNum++;
				}
			}
			int _cardIterationTotal = _drawLoopCurrent + cardsNum;
			for (int indexGameBusy = 0; indexGameBusy < _drawLoopCurrent; indexGameBusy++)
			{
				if (targetsHero != null && indexGameBusy < targetsHero.Count && targetsHero[indexGameBusy] != null && targetsHero[indexGameBusy].Alive && targetsHero[indexGameBusy].HeroItem != null)
				{
					targetTransform = targetsHero[indexGameBusy].HeroItem.transform;
					StartCoroutine(CastCardAction(_cardActive, targetTransform, theCardItem, _uniqueCastId, _automatic, _card, _cardIterationTotal, _cardSpecialValueGlobal, casterHeroParam));
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
			}
			for (int indexGameBusy = 0; indexGameBusy < cardsNum; indexGameBusy++)
			{
				if (targetsNPC != null && indexGameBusy < targetsNPC.Count && targetsNPC[indexGameBusy] != null && targetsNPC[indexGameBusy].Alive && targetsNPC[indexGameBusy].NPCItem != null)
				{
					targetTransform = targetsNPC[indexGameBusy].NPCItem.transform;
					StartCoroutine(CastCardAction(_cardActive, targetTransform, theCardItem, _uniqueCastId, _automatic, _card, _cardIterationTotal, _cardSpecialValueGlobal, casterHeroParam));
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
			}
			yield return null;
		}
		else if (_cardActive.TargetPosition == Enums.CardTargetPosition.Random)
		{
			List<Transform> instaCastTransformList3 = GetInstaCastTransformList(_cardActive, theCasterIsHero);
			int num59 = hero3?.HeroIndex ?? GetRandomIntRange(0, instaCastTransformList3.Count);
			if (hero3 != null)
			{
				StartCoroutine(CastCardAction(_cardActive, hero3.HeroItem.transform, theCardItem, _uniqueCastId, _automatic, _card, 1, _cardSpecialValueGlobal, casterHeroParam));
			}
			else if (num59 < instaCastTransformList3.Count)
			{
				StartCoroutine(CastCardAction(_cardActive, instaCastTransformList3[num59], theCardItem, _uniqueCastId, _automatic, _card, 1, _cardSpecialValueGlobal, casterHeroParam));
			}
			yield return null;
		}
		else
		{
			if (_cardActive.TargetPosition == Enums.CardTargetPosition.Front || _cardActive.TargetPosition == Enums.CardTargetPosition.Back)
			{
				if (theHero != null)
				{
					for (int num60 = 0; num60 < TeamNPC.Length; num60++)
					{
						if (TeamNPC[num60] != null && !(TeamNPC[num60].NpcData == null) && TeamNPC[num60].Alive && CheckTarget(TeamNPC[num60].NPCItem.transform, _cardActive))
						{
							targetTransform = TeamNPC[num60].NPCItem.transform;
							break;
						}
					}
				}
				else
				{
					_ = theNPC;
				}
			}
			StartCoroutine(CastCardAction(_cardActive, targetTransform, theCardItem, _uniqueCastId, _automatic, _card, 1, _cardSpecialValueGlobal, casterHeroParam));
			yield return null;
		}
		NPC[] teamNPC = TeamNPC;
		foreach (NPC currentNPC in teamNPC)
		{
			if (TryGetTransferPainTarget(_cardActive, currentNPC, out var targetNPC2))
			{
				ApplyTransferPain(currentNPC, targetNPC2);
			}
		}
		if (CanUpdateMindSpikeProgress(_cardActive))
		{
			mindSpikeAbility.IncreaseSpecialCardCount();
		}
		Character character = ((theCasterNPC != null) ? ((Character)theCasterNPC) : ((Character)theCasterHero));
		if (TryGetNPCsWithMasterReweaverAbility(_cardActive, character, out var npcsWithAbility2))
		{
			foreach (NPC item15 in npcsWithAbility2)
			{
				UpdateMasterReweaverProgress(character, item15);
			}
		}
		if (_cardActive.Id.StartsWith("reverie", StringComparison.OrdinalIgnoreCase))
		{
			Hero[] array = TeamHero.Where((Hero hero5) => hero5 != null && hero5.Alive && !hero5.HasThreeEnchantments() && ((hero5.GetHpPercent() > 50f && !hero5.HasEnchantment("darkspike")) || (hero5.GetHpPercent() <= 50f && !hero5.HasEnchantment("dreamcocoon")))).ToArray();
			if (array.Length != 0)
			{
				ApplyReveriesEffect(array, _cardActive.CardUpgraded == Enums.CardUpgraded.Rare);
			}
		}
		Hero hero4 = theHero;
		if (hero4 != null && hero4.Alive)
		{
			Enums.CardTargetSide targetSide = _cardActive.TargetSide;
			if (targetSide != Enums.CardTargetSide.Self && targetSide != Enums.CardTargetSide.Friend && targetTransform != null && IsFightCard(_cardActive) && theHero.HasEnchantment("darkspike"))
			{
				NPC nPCById = GetNPCById(targetTransform.name);
				if (nPCById != null && nPCById.Alive)
				{
					theHero.ActivateItem(Enums.EventActivation.Manual, nPCById, 0, "", Enums.ActivationManual.DarkSpike);
				}
			}
		}
		if (character.HasTrait(TraitEnum.DarkOverflow))
		{
			string[] curseIds = new string[2] { "sight", "dark" };
			if (curseIds.Contains(_cardActive.Curse?.Id, StringComparer.OrdinalIgnoreCase) && _cardActive.CurseCharges > 0)
			{
				_cardActive.CurseCharges += GetIncrement(_cardActive.Curse.Id);
			}
			if (curseIds.Contains(_cardActive.Curse2?.Id, StringComparer.OrdinalIgnoreCase) && _cardActive.CurseCharges2 > 0)
			{
				_cardActive.CurseCharges2 += GetIncrement(_cardActive.Curse2.Id);
			}
			if (curseIds.Contains(_cardActive.Curse3?.Id, StringComparer.OrdinalIgnoreCase) && _cardActive.CurseCharges3 > 0)
			{
				_cardActive.CurseCharges3 += GetIncrement(_cardActive.Curse3.Id);
			}
			foreach (CardData.CurseDebuffs item16 in _cardActive.Curses.Where((CardData.CurseDebuffs d) => curseIds.Contains(d.curse?.Id, StringComparer.OrdinalIgnoreCase)))
			{
				if (item16.curseCharges > 0)
				{
					item16.curseCharges += GetIncrement(item16.curse?.Id);
				}
			}
		}
		Character characterActive = GetCharacterActive();
		if (characterActive != null && characterActive.Alive && characterActive.IsHero && CardIsNotInjuryOrBoon(cardActive))
		{
			OnCardCastedByHero?.Invoke(theHero, cardActive);
		}
		targetTransform = null;
		yield return null;
		static int GetIncrement(string curseId)
		{
			if (!curseId.Equals("sight", StringComparison.OrdinalIgnoreCase))
			{
				return 1;
			}
			return 2;
		}
	}

	private void ApplyReveriesEffect(Hero[] heroesWithoutEnchant, bool isRare)
	{
		string text = (isRare ? "darkspikerare" : "darkspike");
		string text2 = (isRare ? "dreamcocoonrare" : "dreamcocoon");
		int randomIntRange = GetRandomIntRange(0, heroesWithoutEnchant.Length);
		Hero obj = heroesWithoutEnchant[randomIntRange];
		obj.AssignEnchantment((obj.GetHpPercent() > 50f) ? text : text2);
		obj.HeroItem.ShowEnchantments();
	}

	public void RaiseCardCastBegin(CardItem theCardItem)
	{
		if (theCardItem != null && heroActive >= 0 && heroActive < TeamHero.Length && TeamHero[heroActive] != null && CardIsNotInjuryOrBoon(theCardItem.CardData))
		{
			OnCardCastByHeroBegins?.Invoke(TeamHero[heroActive], theCardItem.CardData);
		}
	}

	private bool CardHasOCEffect(CardData cardData)
	{
		if (cardData.DamageEnergyBonus <= 0 && cardData.HealEnergyBonus <= 0 && cardData.EffectRepeatEnergyBonus <= 0)
		{
			if (cardData.AcEnergyBonus != null)
			{
				return cardData.AcEnergyBonusQuantity > 0;
			}
			return false;
		}
		return true;
	}

	private bool CardIsNotInjuryOrBoon(CardData cardData)
	{
		if (cardData.CardClass != Enums.CardClass.Boon && cardData.CardClass != Enums.CardClass.Injury)
		{
			return true;
		}
		return false;
	}

	public IEnumerator CastCardAction(CardData _cardActive, Transform targetTransformCast, CardItem theCardItem, string _uniqueCastId, bool _automatic, CardData _card, int _cardIterationTotal, int _cardSpecialValueGlobal, Hero casterHeroParam = null)
	{
		bool youCanCastEffect = false;
		bool isYourFirstTarget = false;
		if (!castedCards.Contains(_uniqueCastId))
		{
			castedCards.Add(_uniqueCastId);
			youCanCastEffect = true;
			isYourFirstTarget = true;
		}
		gameStatus = "CastingCardAction";
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD(gameStatus, "gamestatus");
		}
		if (waitingForCardEnergyAssignment)
		{
			yield break;
		}
		if (!_automatic)
		{
			_cardActive = theCardItem.CardData;
			theCardItem.discard = true;
		}
		else if (_cardActive == null && _card != null)
		{
			_cardActive = _card;
		}
		cardActive = _cardActive;
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("CastCardAction " + _cardActive.Id, "general");
		}
		Hero theCasterHero = theHero;
		if (casterHeroParam != null)
		{
			theCasterHero = casterHeroParam;
		}
		NPC theCasterNPC = theNPC;
		Character theCasterCharacter = null;
		bool theCasterIsHero = false;
		if (theCasterHero != null)
		{
			theCasterIsHero = true;
			theCasterCharacter = theCasterHero;
			_ = theCasterHero.GameName;
			_ = theCasterHero.Id;
			if (!_cardActive.AutoplayDraw && !_cardActive.AutoplayEndTurn && youCanCastEffect && _cardActive.EffectPreAction == "" && !_cardActive.IsPetCast && !_cardActive.IsPetAttack)
			{
				if (_cardActive.MoveToCenter)
				{
					theCasterHero.HeroItem.CharacterAttackAnim();
				}
				else
				{
					theCasterHero.HeroItem.CharacterCastAnim();
				}
			}
		}
		else if (theCasterNPC != null)
		{
			theCasterCharacter = theCasterNPC;
			_ = theNPC.GameName;
			_ = theNPC.Id;
		}
		theCasterCharacter.SetCastedCard(cardActive);
		if (theCasterHero != null && theCasterHero.HeroItem != null)
		{
			if (_cardActive.IsPetCast)
			{
				theCasterHero.HeroItem.PetCastAnim("cast");
			}
			else if (_cardActive.IsPetAttack)
			{
				theCasterHero.HeroItem.PetCastAnim("attack");
			}
		}
		else if (theNPC != null && theNPC.NPCItem != null)
		{
			if (_cardActive.IsPetCast)
			{
				theNPC.NPCItem.PetCastAnim("cast");
			}
			else if (_cardActive.IsPetAttack)
			{
				theNPC.NPCItem.PetCastAnim("attack");
			}
		}
		Hero _hero = null;
		NPC _npc = null;
		HeroItem _heroItem = null;
		NPCItem _npcItem = null;
		if (targetTransformCast == null || (!targetTransformCast.GetComponent<HeroItem>() && !targetTransformCast.GetComponent<NPCItem>()))
		{
			List<Transform> instaCastTransformList = GetInstaCastTransformList(_cardActive, theCasterIsHero);
			if (instaCastTransformList.Count == 1)
			{
				targetTransformCast = instaCastTransformList[0];
			}
			else if (theCasterHero != null)
			{
				if (theCasterHero.HeroItem != null)
				{
					targetTransformCast = theCasterHero.HeroItem.transform;
				}
			}
			else if (theNPC.NPCItem != null)
			{
				targetTransformCast = theNPC.NPCItem.transform;
			}
		}
		if (targetTransformCast != null)
		{
			_heroItem = targetTransformCast.GetComponent<HeroItem>();
			_npcItem = targetTransformCast.GetComponent<NPCItem>();
		}
		bool isHero = false;
		bool isNPC = false;
		if (_heroItem != null)
		{
			isHero = true;
			if (targetTransformCast != null)
			{
				_hero = GetHeroById(targetTransformCast.name);
			}
		}
		else
		{
			isNPC = true;
			if (targetTransformCast != null)
			{
				_npc = GetNPCById(targetTransformCast.name);
			}
		}
		CreateLogCastCard(_status: true, _cardActive, _uniqueCastId, theCasterHero, theNPC, _hero, _npc);
		int repeatCast = _cardActive.EffectRepeat;
		if (repeatCast < 1)
		{
			CardData cardData = _cardActive;
			int num = 1;
			cardData.EffectRepeat = 1;
			repeatCast = num;
		}
		if (_cardActive.EffectRepeatEnergyBonus > 0 && energyAssigned > 0)
		{
			repeatCast *= energyAssigned + 1;
		}
		List<string> targetsUsed = new List<string>();
		List<string> targetElegible = new List<string>();
		float effectRepeatPercent = 100f;
		bool doRedrawInitiatives = false;
		if (targetTransformCast == null)
		{
			repeatCast = 0;
		}
		if (repeatCast > 1 && BossNpc != null)
		{
			BossNpc.OnRepeatCardCast();
		}
		for (int iteration = 0; iteration < repeatCast; iteration++)
		{
			castCardDamageDoneIteration = 0f;
			if ((theCasterHero != null && theCasterHero.HeroItem == null) || (theCasterNPC != null && theCasterNPC.NPCItem == null))
			{
				break;
			}
			string lastId = "";
			castCardDamageDone = 0f;
			if (_cardActive.PetActivation == Enums.ActivePets.CurrentTarget && theCasterHero != null && theCasterHero.Alive)
			{
				Character character = ((_npc != null) ? ((Character)_npc) : ((Character)_hero));
				character.ActivateItem(Enums.EventActivation.None, null, 0, character.Pet, Enums.ActivationManual.None, forceActivate: true);
				AtOManager.Instance.RaisePetActivationEvent(theCasterHero, character, character.Pet, 0, fromPlayerCard: true);
			}
			if (_cardActive.PetBonusDamageType != Enums.DamageType.None)
			{
				Character character2 = ((_npc != null) ? ((Character)_npc) : ((Character)_hero));
				if (character2.IsHero)
				{
					((Hero)character2).PetBonusDamageType = _cardActive.PetBonusDamageType;
					((Hero)character2).PetBonusDamageAmount = _cardActive.PetBonusDamageAmount;
				}
			}
			if (iteration == 0 && Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("RAND 1 => " + randomForIteration, "randomindex");
			}
			if (iteration > 0)
			{
				if (CheckMatchIsOver())
				{
					FinishCombat();
					yield break;
				}
				randomForIteration += 5;
				if (randomForIteration >= randomStringArr.Count)
				{
					randomForIteration = 0;
				}
				SetRandomIndex(randomForIteration);
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("INCREASE RANDOM FOR ITERATION => " + randomForIteration, "randomindex");
				}
				if (_cardActive.EffectRepeatModificator != 0)
				{
					effectRepeatPercent += effectRepeatPercent * (float)_cardActive.EffectRepeatModificator * 0.01f;
				}
				if (_cardActive.EffectRepeatTarget != Enums.EffectRepeatTarget.Same)
				{
					bool flag = false;
					string text = "";
					if (_cardActive.EffectRepeatTarget == Enums.EffectRepeatTarget.Random)
					{
						if (isHero)
						{
							int num2 = 0;
							while (!flag)
							{
								int randomIntRange = GetRandomIntRange(0, TeamHero.Length);
								if (TeamHero[randomIntRange] != null && TeamHero[randomIntRange].Alive && CheckTarget(TeamHero[randomIntRange].HeroItem.transform, _cardActive, theCasterIsHero))
								{
									text = TeamHero[randomIntRange].Id;
									flag = true;
								}
								num2++;
								if (num2 > 1000)
								{
									text = "";
									flag = true;
								}
							}
						}
						else
						{
							int num3 = 0;
							while (!flag)
							{
								int randomIntRange = GetRandomIntRange(0, TeamNPC.Length);
								if (TeamNPC[randomIntRange] != null && TeamNPC[randomIntRange].Alive && TeamNPC[randomIntRange].NPCItem != null && CheckTarget(TeamNPC[randomIntRange].NPCItem.transform, _cardActive, theCasterIsHero))
								{
									text = TeamNPC[randomIntRange].Id;
									flag = true;
								}
								num3++;
								if (num3 > 1000)
								{
									text = "";
									flag = true;
								}
							}
						}
					}
					else if (_cardActive.EffectRepeatTarget == Enums.EffectRepeatTarget.Chain)
					{
						lastId = targetsUsed.Last();
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("LastId->" + lastId);
						}
						targetElegible.Clear();
						if (isHero)
						{
							int randomIntRange = GetHeroById(lastId).Position;
							int num4 = randomIntRange - 1;
							int num5 = (GetHeroById(lastId).Alive ? (randomIntRange + 1) : randomIntRange);
							for (int i = 0; i < TeamHero.Length; i++)
							{
								if (TeamHero[i] != null && !(TeamHero[i].HeroData == null) && TeamHero[i].Alive && TeamHero[i].Id != lastId && (TeamHero[i].Position == num4 || TeamHero[i].Position == num5))
								{
									targetElegible.Add(TeamHero[i].Id);
								}
							}
						}
						else
						{
							NPC nPCById = GetNPCById(lastId);
							if (nPCById != null)
							{
								int randomIntRange = nPCById.Position;
								int num4 = randomIntRange - 1;
								int num5 = randomIntRange + 1;
								if (!nPCById.Alive)
								{
									num5 = randomIntRange;
								}
								for (int j = 0; j < TeamNPC.Length; j++)
								{
									if (TeamNPC[j] != null && !(TeamNPC[j].NpcData == null) && TeamNPC[j].Alive && TeamNPC[j].Id != lastId && (TeamNPC[j].Position == num4 || TeamNPC[j].Position == num5))
									{
										targetElegible.Add(TeamNPC[j].Id);
									}
								}
							}
						}
						if (targetElegible.Count > 0)
						{
							text = targetElegible[GetRandomIntRange(0, targetElegible.Count)];
						}
					}
					else if (_cardActive.EffectRepeatTarget == Enums.EffectRepeatTarget.NoRepeat)
					{
						lastId = targetsUsed.Last();
						targetElegible.Clear();
						if (isHero)
						{
							for (int k = 0; k < TeamHero.Length; k++)
							{
								if (TeamHero[k] != null && !(TeamHero[k].HeroData == null) && TeamHero[k].Alive && TeamHero[k].Id != lastId && _hero.Id != TeamHero[k].Id)
								{
									targetElegible.Add(TeamHero[k].Id);
								}
							}
						}
						else
						{
							for (int l = 0; l < TeamNPC.Length; l++)
							{
								if (TeamNPC[l] != null && !(TeamNPC[l].NpcData == null) && TeamNPC[l].Alive && TeamNPC[l].Id != lastId && _npc.Id != TeamNPC[l].Id)
								{
									targetElegible.Add(TeamNPC[l].Id);
								}
							}
						}
						if (targetElegible.Count > 0)
						{
							text = targetElegible[GetRandomIntRange(0, targetElegible.Count)];
						}
					}
					else if (_cardActive.EffectRepeatTarget == Enums.EffectRepeatTarget.NeverRepeat)
					{
						targetElegible.Clear();
						if (isHero)
						{
							for (int m = 0; m < TeamHero.Length; m++)
							{
								if (TeamHero[m] != null && !(TeamHero[m].HeroData == null) && TeamHero[m].Alive && !targetsUsed.Contains(TeamHero[m].Id))
								{
									targetElegible.Add(TeamHero[m].Id);
								}
							}
						}
						else
						{
							for (int n = 0; n < TeamNPC.Length; n++)
							{
								if (TeamNPC[n] != null && !(TeamNPC[n].NpcData == null) && TeamNPC[n].Alive && !targetsUsed.Contains(TeamNPC[n].Id))
								{
									targetElegible.Add(TeamNPC[n].Id);
								}
							}
						}
						if (targetElegible.Count > 0)
						{
							text = targetElegible[GetRandomIntRange(0, targetElegible.Count)];
						}
					}
					if (text != "")
					{
						if (isHero)
						{
							_hero = GetHeroById(text);
							targetTransformCast = _hero.HeroItem.transform;
						}
						else
						{
							_npc = GetNPCById(text);
							targetTransformCast = _npc.NPCItem.transform;
						}
						targetsUsed.Add(text);
					}
					else
					{
						targetTransformCast = null;
						repeatCast = 0;
					}
					if (targetTransformCast == null)
					{
						targetTransform = null;
					}
				}
				if (_npc != null)
				{
					_npcItem = _npc.NPCItem;
					if (!_npc.Alive || (BossNpc != null && _npc == BossNpc.npc && !BossNpc.IsValidTarget()))
					{
						break;
					}
				}
				if (_hero != null)
				{
					_heroItem = _hero.HeroItem;
				}
			}
			else
			{
				if (_hero != null)
				{
					targetsUsed.Add(_hero.Id);
				}
				if (_npc != null)
				{
					targetsUsed.Add(_npc.Id);
				}
			}
			if (theCasterNPC != null && _cardActive.AddCardId != "" && _hero != null && _hero.Alive)
			{
				CardData cardForModify = GetCardForModify(_cardActive, theCasterNPC, _hero);
				GenerateNewCard(_cardActive.AddCard, _cardActive.AddCardId, createCard: true, _cardActive.AddCardPlace, cardForModify, null, _hero.HeroIndex, isHero: true, 0, _cardActive.CopyConfig);
				yield return Globals.Instance.WaitForSeconds(1f);
				int exahustGenerating = 0;
				while (generatedCardTimes > 0 && exahustGenerating < 10)
				{
					yield return Globals.Instance.WaitForSeconds(0.1f);
					exahustGenerating++;
				}
			}
			if (targetTransformCast != null)
			{
				bool castedEffect = false;
				if (iteration == 0 && _cardActive.MoveToCenter)
				{
					if (youCanCastEffect && cardIteration.ContainsKey(_cardActive.InternalId) && cardIteration[_cardActive.InternalId] == 0)
					{
						if (theCasterHero != null && theCasterHero.HeroItem != null && !_cardActive.TempAttackSelf)
						{
							theCasterHero.HeroItem.MoveToCenter();
						}
						else if (theCasterNPC != null && theCasterNPC.NPCItem != null && !IsPhantomArmorSpecialCard(_cardActive.Id))
						{
							theCasterNPC.NPCItem.MoveToCenter();
						}
					}
					if (theCasterHero != null && theCasterHero.HeroItem != null)
					{
						while (theCasterHero.HeroItem.CharIsMoving())
						{
							yield return null;
						}
					}
					else if (theCasterNPC != null && theCasterNPC.NPCItem != null)
					{
						while (theCasterNPC.NPCItem.CharIsMoving())
						{
							yield return null;
						}
					}
					if (_automatic && _cardActive.AddCard > 0 && _cardActive.MoveToCenter && theCasterNPC != null && theCasterNPC.NPCItem != null)
					{
						theCasterNPC.NPCItem.CharacterAttackAnim();
					}
				}
				if (_cardActive != null && _cardActive.EffectCaster.Trim() != "" && (iteration == 0 || _cardActive.EffectCasterRepeat) && youCanCastEffect)
				{
					if (theCasterNPC != null && theCasterNPC.NPCItem != null)
					{
						EffectsManager.Instance.PlayEffect(_cardActive, isCaster: true, isHero: false, theCasterNPC.NPCItem.CharImageT);
					}
					else if (theCasterHero != null && theCasterHero.HeroItem != null)
					{
						EffectsManager.Instance.PlayEffect(_cardActive, isCaster: true, isHero: true, theCasterHero.HeroItem.CharImageT);
					}
				}
				int exahustGenerating = Functions.FuncRoundToInt(GetCardSpecialValue(_cardActive, 1, theCasterHero, theCasterNPC, _hero, _npc, _IsPreview: false));
				int cardSpecialValue2 = Functions.FuncRoundToInt(GetCardSpecialValue(_cardActive, 2, theCasterHero, theCasterNPC, _hero, _npc, _IsPreview: false));
				if (_cardActive.ItemEnchantment != null)
				{
					string id = _cardActive.ItemEnchantment.Id;
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("ENCHANTMENT ->" + id);
					}
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
				else if (_cardActive.Playable && _cardActive.PetModel != null && _cardActive.PetTemporal)
				{
					StartCoroutine(CreateCardCastPet(_cardActive, theCasterHero.HeroItem.gameObject, theCasterHero, null));
					yield return Globals.Instance.WaitForSeconds(0.75f);
				}
				if (_cardActive.TransferCurses > 0)
				{
					List<string> list = new List<string>();
					List<int> list2 = new List<int>();
					int num6 = 0;
					if (theCasterCharacter != null)
					{
						for (int num7 = 0; num7 < theCasterCharacter.AuraList.Count; num7++)
						{
							if (num6 >= _cardActive.TransferCurses)
							{
								break;
							}
							if (theCasterCharacter.AuraList[num7] != null && theCasterCharacter.AuraList[num7].ACData != null && !theCasterCharacter.AuraList[num7].ACData.IsAura && theCasterCharacter.AuraList[num7].ACData.Removable && theCasterCharacter.AuraList[num7].GetCharges() > 0)
							{
								list.Add(theCasterCharacter.AuraList[num7].ACData.Id);
								list2.Add(theCasterCharacter.AuraList[num7].GetCharges());
								num6++;
							}
						}
					}
					if (num6 > 0)
					{
						theCasterCharacter.HealCursesName(list);
						for (int num8 = 0; num8 < list.Count; num8++)
						{
							if (_hero != null && _hero.Alive)
							{
								if (list[num8] == "sight")
								{
									list[num8] = "sighthero";
								}
								_hero.SetAura(theCasterCharacter, Globals.Instance.GetAuraCurseData(list[num8]), list2[num8], fromTrait: false, _cardActive.CardClass);
							}
							else if (_npc != null && _npc.Alive)
							{
								if (list[num8] == "sighthero")
								{
									list[num8] = "sight";
								}
								_npc.SetAura(theCasterCharacter, Globals.Instance.GetAuraCurseData(list[num8]), list2[num8], fromTrait: false, _cardActive.CardClass);
							}
						}
					}
					yield return Globals.Instance.WaitForSeconds(0.1f);
				}
				if (_cardActive.StealAuras > 0)
				{
					List<string> list3 = new List<string>();
					List<int> list4 = new List<int>();
					int num9 = 0;
					Character character3 = null;
					if (_hero != null && _hero.Alive)
					{
						character3 = _hero;
					}
					else if (_npc != null && _npc.Alive)
					{
						character3 = _npc;
					}
					if (character3 != null)
					{
						for (int num10 = 0; num10 < character3.AuraList.Count; num10++)
						{
							if (num9 >= _cardActive.StealAuras)
							{
								break;
							}
							if (character3.AuraList[num10] != null && character3.AuraList[num10].ACData != null && character3.AuraList[num10].ACData.IsAura && character3.AuraList[num10].ACData.Removable && character3.AuraList[num10].GetCharges() > 0 && !(character3.AuraList[num10].ACData.Id == "invulnerableunremovable"))
							{
								list3.Add(character3.AuraList[num10].ACData.Id);
								list4.Add(character3.AuraList[num10].GetCharges());
								num9++;
							}
						}
					}
					if (num9 > 0)
					{
						character3.HealCursesName(list3);
						for (int num11 = 0; num11 < list3.Count; num11++)
						{
							if (theCasterCharacter != null && theCasterCharacter.Alive)
							{
								theCasterCharacter.SetAura(character3, Globals.Instance.GetAuraCurseData(list3[num11]), list4[num11], fromTrait: false, _cardActive.CardClass);
							}
						}
					}
					yield return Globals.Instance.WaitForSeconds(0.1f);
				}
				if (_cardActive.AddVanishToDeck)
				{
					if (_hero != null)
					{
						foreach (string item in HeroDeck[_hero.HeroIndex])
						{
							GetCardData(item).Vanish = true;
						}
					}
					else if (_npc != null)
					{
						foreach (string item2 in NPCDeck[_npc.NPCIndex])
						{
							GetCardData(item2).Vanish = true;
						}
					}
				}
				if (_cardActive.HealCurses != 0)
				{
					if (_hero != null && _hero.Alive)
					{
						_hero.HealCurses(_cardActive.HealCurses);
					}
					else if (_npc != null && _npc.Alive)
					{
						_npc.HealCurses(_cardActive.HealCurses);
					}
					if (!doRedrawInitiatives)
					{
						doRedrawInitiatives = true;
					}
				}
				if (isYourFirstTarget && _cardActive.HealAuraCurseSelf != null && _cardActive.HealAuraCurseSelf.Id != "spellsword")
				{
					theCasterCharacter.HealAuraCurse(_cardActive.HealAuraCurseSelf);
				}
				List<string> list5 = new List<string>();
				if (_cardActive.HealAuraCurseName != null)
				{
					list5.Add(_cardActive.HealAuraCurseName.Id);
				}
				if (_cardActive.HealAuraCurseName2 != null)
				{
					list5.Add(_cardActive.HealAuraCurseName2.Id);
				}
				if (_cardActive.HealAuraCurseName3 != null)
				{
					list5.Add(_cardActive.HealAuraCurseName3.Id);
				}
				if (_cardActive.HealAuraCurseName4 != null)
				{
					list5.Add(_cardActive.HealAuraCurseName4.Id);
				}
				if (list5.Count > 0)
				{
					if (_hero != null && _hero.Alive)
					{
						_hero.HealCursesName(list5);
					}
					else if (_npc != null && _npc.Alive)
					{
						_npc.HealCursesName(list5);
					}
					if (!doRedrawInitiatives)
					{
						doRedrawInitiatives = true;
					}
				}
				if (_cardActive.IncreaseAuras > 0 || _cardActive.IncreaseCurses > 0 || _cardActive.ReduceAuras > 0 || _cardActive.ReduceCurses > 0)
				{
					Character character4 = null;
					if (_hero != null && _hero.Alive)
					{
						character4 = _hero;
					}
					else if (_npc != null && _npc.Alive)
					{
						character4 = _npc;
					}
					if (character4 != null)
					{
						for (int num12 = 0; num12 < 4; num12++)
						{
							if ((num12 == 0 && _cardActive.IncreaseAuras <= 0) || (num12 == 1 && _cardActive.IncreaseCurses <= 0) || (num12 == 2 && _cardActive.ReduceAuras <= 0) || (num12 == 3 && _cardActive.ReduceCurses <= 0))
							{
								continue;
							}
							List<string> list6 = new List<string>();
							List<int> list7 = new List<int>();
							for (int num13 = 0; num13 < character4.AuraList.Count; num13++)
							{
								if (character4.AuraList[num13] != null && character4.AuraList[num13].ACData != null && character4.AuraList[num13].GetCharges() > 0 && !(character4.AuraList[num13].ACData.Id == "furnace") && !(character4.AuraList[num13].ACData.Id == "spellsword") && !character4.AuraList[num13].ACData.Id.StartsWith("rune"))
								{
									bool flag2 = false;
									if ((num12 == 0 || num12 == 2) && character4.AuraList[num13].ACData.IsAura)
									{
										flag2 = true;
									}
									else if ((num12 == 1 || num12 == 3) && !character4.AuraList[num13].ACData.IsAura)
									{
										flag2 = true;
									}
									if (flag2)
									{
										list6.Add(character4.AuraList[num13].ACData.Id);
										list7.Add(character4.AuraList[num13].GetCharges());
									}
								}
							}
							if (list6.Count <= 0)
							{
								continue;
							}
							for (int num14 = 0; num14 < list6.Count; num14++)
							{
								int num15 = num12 switch
								{
									0 => Functions.FuncRoundToInt((float)list7[num14] * (float)_cardActive.IncreaseAuras / 100f), 
									1 => Functions.FuncRoundToInt((float)list7[num14] * (float)_cardActive.IncreaseCurses / 100f), 
									2 => list7[num14] - Functions.FuncRoundToInt((float)list7[num14] * (float)_cardActive.ReduceAuras / 100f), 
									_ => list7[num14] - Functions.FuncRoundToInt((float)list7[num14] * (float)_cardActive.ReduceCurses / 100f), 
								};
								switch (num12)
								{
								case 0:
								case 1:
								{
									AuraCurseData auraCurseData = AtOManager.Instance.GlobalAuraCurseModificationByTraitsAndItems("set", list6[num14], theCasterCharacter, character4);
									if (auraCurseData != null)
									{
										int maxCharges = auraCurseData.GetMaxCharges();
										if (maxCharges > -1 && list7[num14] + num15 > maxCharges)
										{
											num15 = maxCharges - list7[num14];
										}
										character4.SetAura(theCasterCharacter, auraCurseData, num15, fromTrait: false, _cardActive.CardClass, useCharacterMods: false, canBePreventable: false);
									}
									break;
								}
								case 2:
								case 3:
									if (num15 <= 0)
									{
										num15 = 1;
									}
									character4.ModifyAuraCurseCharges(list6[num14], num15);
									character4.UpdateAuraCurseFunctions();
									break;
								}
							}
						}
					}
				}
				if (_cardActive.DispelAuras != 0)
				{
					if (_hero != null && _hero.Alive)
					{
						_hero.DispelAuras(_cardActive.DispelAuras);
					}
					else if (_npc != null && _npc.Alive)
					{
						_npc.DispelAuras(_cardActive.DispelAuras);
					}
					if (!doRedrawInitiatives)
					{
						doRedrawInitiatives = true;
					}
				}
				Dictionary<Character, int> charactersDamagedEvents = new Dictionary<Character, int>();
				Dictionary<Character, int> damageWithHitEvents = new Dictionary<Character, int>();
				Dictionary<Character, DamageReflectedPayload> damageReflectedActions = new Dictionary<Character, DamageReflectedPayload>();
				Dictionary<Character, HealAttackerPayload> healAttackerActions = new Dictionary<Character, HealAttackerPayload>();
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
					if (_npcItem != null)
					{
						ctNPC[_npcItem] = CRT;
					}
					if (_heroItem != null)
					{
						ctHero[_heroItem] = CRT;
					}
					int directAttackIteration = 0;
					bool damagedEventFired = false;
					bool blockedEventFired = false;
					bool fullEvadedDmg = false;
					bool fullEvadedDmgSide1 = false;
					bool fullEvadedDmgSide2 = false;
					bool ignoreBlock = false;
					for (int num16 = 1; num16 <= 8; num16++)
					{
						if (num16 == 1 || num16 == 3 || num16 == 4)
						{
							damagedEventFired = false;
						}
						if ((fullEvadedDmg && num16 == 2) || (fullEvadedDmgSide1 && num16 == 5) || (fullEvadedDmgSide2 && num16 == 6))
						{
							continue;
						}
						int dmg = 0;
						Enums.DamageType dmgType = Enums.DamageType.None;
						switch (num16)
						{
						case 1:
							dmg = _cardActive.Damage;
							if (_cardActive.DamageEnergyBonus > 0 && energyAssigned > 0)
							{
								dmg += _cardActive.DamageEnergyBonus * energyAssigned;
							}
							ignoreBlock = _cardActive.GetIgnoreBlock();
							if (dmg <= 0)
							{
								continue;
							}
							dmgType = _cardActive.DamageType;
							break;
						case 2:
							if (_cardActive.DamageType == _cardActive.DamageType2)
							{
								continue;
							}
							dmg = _cardActive.Damage2;
							ignoreBlock = _cardActive.GetIgnoreBlock(1);
							if (dmg <= 0)
							{
								continue;
							}
							dmgType = _cardActive.DamageType2;
							break;
						case 3:
						case 4:
						case 5:
						case 6:
						{
							CRT = new CastResolutionForCombatText();
							if (num16 == 3 || num16 == 4)
							{
								dmg = _cardActive.DamageSides;
								ignoreBlock = _cardActive.GetIgnoreBlock();
								if (dmg <= 0)
								{
									continue;
								}
								dmgType = (CRT.damageType = _cardActive.DamageType);
							}
							else
							{
								dmg = _cardActive.DamageSides2;
								ignoreBlock = _cardActive.GetIgnoreBlock(1);
								if (dmg <= 0)
								{
									continue;
								}
								dmgType = _cardActive.DamageType2;
							}
							int num17 = ((!isHero) ? _npc.Position : _hero.Position);
							int num18;
							if (num16 == 3 || num16 == 5)
							{
								if (num17 == 0)
								{
									continue;
								}
								num18 = num17 - 1;
							}
							else
							{
								num18 = num17 + 1;
							}
							bool flag3 = false;
							if (isHero)
							{
								for (int num19 = 0; num19 < TeamHero.Length; num19++)
								{
									if (TeamHero[num19] != null && TeamHero[num19].Alive && TeamHero[num19].Position == num18)
									{
										_hero = TeamHero[num19];
										_heroItem = _hero.HeroItem;
										flag3 = true;
										ctHero[_heroItem] = CRT;
										break;
									}
								}
							}
							else
							{
								for (int num20 = 0; num20 < TeamNPC.Length; num20++)
								{
									if (TeamNPC[num20] != null && TeamNPC[num20].Alive && TeamNPC[num20].Position == num18)
									{
										_npc = TeamNPC[num20];
										_npcItem = _npc.NPCItem;
										flag3 = true;
										ctNPC[_npcItem] = CRT;
										break;
									}
								}
							}
							if (!flag3)
							{
								continue;
							}
							break;
						}
						case 7:
						case 8:
							CRT = new CastResolutionForCombatText();
							if (!youCanCastEffect)
							{
								continue;
							}
							if (num16 == 7)
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
							if (dmg <= 0)
							{
								continue;
							}
							if (theCasterIsHero)
							{
								isNPC = false;
								_hero = theCasterHero;
								_heroItem = _hero.HeroItem;
								ctHero[_heroItem] = CRT;
							}
							else
							{
								isNPC = true;
								_npc = theCasterNPC;
								_npcItem = _npc.NPCItem;
								ctNPC[_npcItem] = CRT;
							}
							break;
						}
						if (num16 == 1 && ((_npc != null && _npc.Alive) || (_hero != null && _hero.Alive)))
						{
							castedEffect = true;
							AudioClip soundHit = _cardActive.GetSoundHit();
							if (soundHit != null)
							{
								float delay = 0f;
								if (_cardActive.SoundHitReworkDelay > 0f && !GameManager.Instance.ConfigUseLegacySounds)
								{
									delay = _cardActive.SoundHitReworkDelay;
								}
								if (theHero != null || _cardActive.MoveToCenter)
								{
									GameManager.Instance.PlayAudio(soundHit, 0.25f, delay);
								}
								else
								{
									GameManager.Instance.PlayAudio(soundHit, 0.75f, delay);
								}
							}
							if (_cardActive.EffectPreAction == "" && _cardActive.EffectPostCastDelay > 0f && (iteration == 0 || _cardActive.EffectCasterRepeat))
							{
								float num21 = _cardActive.EffectPostCastDelay;
								if (GameManager.Instance.IsMultiplayer() || GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Fast || GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Ultrafast)
								{
									num21 = _cardActive.EffectPostCastDelay * 0.6f;
								}
								if (num21 < 0.1f)
								{
									num21 = 0.1f;
								}
								yield return Globals.Instance.WaitForSeconds(num21);
							}
							if (_cardActive.EffectTrail != "" && (iteration == 0 || _cardActive.EffectTrailRepeat))
							{
								Transform transform = null;
								Transform to = null;
								int num22 = 0;
								int num23 = 0;
								bool flag4 = true;
								if (theCasterNPC != null && theCasterNPC.NPCItem != null)
								{
									if (iteration > 0 && (_cardActive.EffectRepeatTarget == Enums.EffectRepeatTarget.Chain || _cardActive.EffectRepeatTarget == Enums.EffectRepeatTarget.NoRepeat))
									{
										Hero heroById = GetHeroById(lastId);
										if (heroById != null && heroById.HeroItem != null)
										{
											transform = heroById.HeroItem.CharImageT;
											num22 = heroById.Position;
										}
										else
										{
											flag4 = false;
										}
									}
									else
									{
										transform = theCasterNPC.NPCItem.CharImageT;
										num22 = theCasterNPC.Position;
									}
								}
								else if (theCasterHero != null && theCasterHero.HeroItem != null)
								{
									if (iteration > 0 && (_cardActive.EffectRepeatTarget == Enums.EffectRepeatTarget.Chain || _cardActive.EffectRepeatTarget == Enums.EffectRepeatTarget.NoRepeat))
									{
										NPC nPCById2 = GetNPCById(lastId);
										if (nPCById2 != null && nPCById2.NPCItem != null)
										{
											transform = nPCById2.NPCItem.CharImageT;
											num22 = nPCById2.Position;
										}
										else
										{
											flag4 = false;
										}
									}
									else
									{
										transform = theCasterHero.HeroItem.CharImageT;
										num22 = theCasterHero.Position;
									}
								}
								if (flag4)
								{
									if (_npc != null && _npcItem != null)
									{
										to = _npcItem.CharImageT;
										num23 = _npc.Position;
									}
									else if (_hero != null && _heroItem != null)
									{
										to = _heroItem.CharImageT;
										num23 = _hero.Position;
									}
									waitingTrail = true;
									EffectsManager.Instance.PlayEffectTrail(_cardActive, theCasterIsHero, transform, to, num22 + num23 + 1);
									while (waitingTrail)
									{
										yield return Globals.Instance.WaitForSeconds(0.01f);
									}
								}
							}
							if (_cardActive.EffectTarget != "")
							{
								if (_npc != null && _npcItem != null && _cardActive.SummonUnit == null)
								{
									EffectsManager.Instance.PlayEffect(_cardActive, isCaster: false, theCasterIsHero, _npcItem.CharImageT);
								}
								else if (_hero != null && _heroItem != null)
								{
									EffectsManager.Instance.PlayEffect(_cardActive, isCaster: false, theCasterIsHero, _heroItem.CharImageT);
								}
							}
							if (_cardActive.EffectPostTargetDelay > 0f)
							{
								if (GameManager.Instance.IsMultiplayer())
								{
									yield return Globals.Instance.WaitForSeconds(_cardActive.EffectPostTargetDelay * 0.3f);
								}
								else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
								{
									yield return Globals.Instance.WaitForSeconds(_cardActive.EffectPostTargetDelay);
								}
								else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Ultrafast)
								{
									yield return null;
								}
								else
								{
									yield return Globals.Instance.WaitForSeconds(_cardActive.EffectPostTargetDelay * 0.2f);
								}
							}
						}
						if (num16 == 1 || num16 == 3 || num16 == 4 || num16 == 7)
						{
							if (_cardActive.DamageSpecialValueGlobal)
							{
								dmg = _cardSpecialValueGlobal;
							}
							else if (_cardActive.DamageSpecialValue1)
							{
								dmg = exahustGenerating;
							}
							else if (_cardActive.DamageSpecialValue2)
							{
								dmg = cardSpecialValue2;
							}
						}
						if (num16 == 1 && dmgType == _cardActive.DamageType2)
						{
							bool flag5 = false;
							if (_cardActive.Damage2SpecialValueGlobal)
							{
								dmg += _cardSpecialValueGlobal;
								flag5 = true;
							}
							else if (_cardActive.Damage2SpecialValue1)
							{
								dmg += exahustGenerating;
								flag5 = true;
							}
							else if (_cardActive.Damage2SpecialValue2)
							{
								dmg += cardSpecialValue2;
								flag5 = true;
							}
							if (!flag5)
							{
								dmg += _cardActive.Damage2;
							}
						}
						if (num16 == 2 || num16 == 5 || num16 == 6 || num16 == 8)
						{
							if (_cardActive.Damage2SpecialValueGlobal)
							{
								dmg = _cardSpecialValueGlobal;
							}
							else if (_cardActive.Damage2SpecialValue1)
							{
								dmg = exahustGenerating;
							}
							else if (_cardActive.Damage2SpecialValue2)
							{
								dmg = cardSpecialValue2;
							}
						}
						int num24 = 0;
						int dmgTotal = 0;
						if (theCasterHero != null)
						{
							num24 = theCasterHero.DamageWithCharacterBonus(dmg, dmgType, _cardActive.CardClass, energyJustWastedByHero);
							num24 += TeamBonusHotline.GetDamageBonusFromTeamHeroesForPets(_cardActive, theCasterHero, num24, dmgType, _cardActive.CardClass, energyJustWastedByHero);
						}
						else if (theCasterNPC != null)
						{
							num24 = theCasterNPC.DamageWithCharacterBonus(dmg, dmgType, _cardActive.CardClass, energyJustWastedByHero);
						}
						if (effectRepeatPercent != 100f)
						{
							num24 = Functions.FuncRoundToInt((float)num24 * effectRepeatPercent * 0.01f);
						}
						int blocked = 0;
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("directAttackIteration->" + directAttackIteration, "general");
						}
						if (isNPC)
						{
							if (_npc.IsInvulnerable())
							{
								CRT.invulnerable = true;
							}
							else
							{
								int num25 = _npc.IncreasedCursedDamagePerStack(dmgType);
								num24 += num25;
								if (num24 <= 0)
								{
									num24 = 0;
									CRT.mitigated = true;
								}
								int num26 = _npc.EffectCharges("evasion");
								bool flag6 = _npc.GetAuraCharges("block") > 0;
								if ((num16 == 1 || num16 == 3 || num16 == 4 || num16 == 7) && num26 > 0)
								{
									_npc.ConsumeEffectCharges("evasion", 1);
									CRT.evaded = true;
									switch (num16)
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
									theCasterCharacter.SetEvent(Enums.EventActivation.Evaded, _npc);
								}
								else
								{
									if (num16 == 1 || num16 == 3 || num16 == 4)
									{
										_npc.SetEvent(Enums.EventActivation.Hitted, theCasterCharacter);
										if (directAttackIteration == 0)
										{
											theCasterCharacter.SetEvent(Enums.EventActivation.Hit);
										}
										if (_npc.HasEffect("sanctify") && theCasterHero != null && AtOManager.Instance.CharacterHavePerk(theCasterHero.SubclassName, "mainperksanctify2c"))
										{
											_npc.IndirectDamage(Enums.DamageType.Holy, Functions.FuncRoundToInt((float)_npc.GetAuraCharges("sanctify") * 0.25f), theCasterCharacter);
										}
										if (_npc.HasEffect("sanctify") && AtOManager.Instance.IsChallengeTraitActive("holypenance"))
										{
											_npc.IndirectDamage(Enums.DamageType.Holy, Functions.FuncRoundToInt((float)_npc.GetAuraCharges("sanctify") * 0.25f), theCasterCharacter);
										}
									}
									if (!ignoreBlock)
									{
										blocked = num24;
										num24 = _npc.ModifyBlock(num24);
										blocked -= num24;
									}
									CRT.blocked = blocked;
									if (num24 == 0 && flag6)
									{
										if (blocked > 0)
										{
											CRT.fullblocked = true;
											if (directAttackIteration == 0)
											{
												_npc.SetEvent(Enums.EventActivation.Block, theCasterCharacter, blocked);
												theCasterCharacter.SetEvent(Enums.EventActivation.Blocked, _npc, blocked);
											}
										}
									}
									else
									{
										CRT.fullblocked = false;
										if (!_npc.IsImmune(dmgType))
										{
											int num27 = -1 * _npc.BonusResists(dmgType);
											dmgTotal = Functions.FuncRoundToInt((float)num24 + (float)num24 * (float)num27 * 0.01f);
											dmgTotal = IncreaseDamage(_npc, theCasterCharacter, dmgTotal);
											if (num16 == 2 || num16 == 8)
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
										{
											CRT.immune = true;
										}
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
							int num25 = _hero.IncreasedCursedDamagePerStack(dmgType);
							num24 += num25;
							if (num24 <= 0)
							{
								num24 = 0;
								CRT.mitigated = true;
							}
							int num28 = _hero.EffectCharges("evasion");
							bool flag7 = _hero.GetAuraCharges("block") > 0;
							if ((num16 == 1 || num16 == 3 || num16 == 4 || num16 == 7) && num28 > 0)
							{
								_hero.ConsumeEffectCharges("evasion", 1);
								CRT.evaded = true;
								switch (num16)
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
								theCasterCharacter.SetEvent(Enums.EventActivation.Evaded, _hero);
							}
							else
							{
								if (num16 == 1 || num16 == 3 || num16 == 4)
								{
									_hero.SetEvent(Enums.EventActivation.Hitted, theCasterCharacter);
									if (directAttackIteration == 0)
									{
										theCasterCharacter.SetEvent(Enums.EventActivation.Hit);
									}
									if (_hero.HasEffect("sanctify") && AtOManager.Instance.IsChallengeTraitActive("holypenance"))
									{
										_hero.IndirectDamage(Enums.DamageType.Holy, Functions.FuncRoundToInt((float)_hero.GetAuraCharges("sanctify") * 0.25f), theCasterCharacter);
									}
								}
								if (!ignoreBlock)
								{
									blocked = num24;
									num24 = _hero.ModifyBlock(num24);
									blocked -= num24;
								}
								CRT.blocked = blocked;
								if (num24 == 0 && flag7)
								{
									if (blocked > 0)
									{
										CRT.fullblocked = true;
										if (directAttackIteration == 0)
										{
											_hero.SetEvent(Enums.EventActivation.Block, theCasterCharacter, blocked);
											theCasterCharacter.SetEvent(Enums.EventActivation.Blocked, _hero, blocked);
										}
									}
								}
								else if (!_hero.IsImmune(dmgType))
								{
									int num27 = -1 * _hero.BonusResists(dmgType);
									dmgTotal = Functions.FuncRoundToInt((float)num24 + (float)num24 * (float)num27 * 0.01f);
									dmgTotal = IncreaseDamage(_hero, theCasterCharacter, dmgTotal);
									if (num16 == 2 || num16 == 8)
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
								{
									CRT.immune = true;
								}
							}
						}
						if (dmgTotal < 0)
						{
							dmgTotal = 0;
						}
						new Dictionary<Enums.DamageType, int>();
						if (isNPC)
						{
							if (dmgTotal > _npc.GetHp())
							{
								castCardDamageDone = _npc.GetHp();
							}
							else
							{
								castCardDamageDone = dmgTotal;
							}
							castCardDamageDoneTotal += castCardDamageDone;
							castCardDamageDoneIteration += dmgTotal;
							_npc.ModifyHp(-dmgTotal, _includeInStats: true, _refreshHP: true, theCasterCharacter);
							if (IsPrimaryDirectDamage(_npc, _npcBeforeDmg, num16, dmgTotal))
							{
								_npc.TryRemoveAuraCurse("sleep");
							}
							if (dmgTotal > 0 && (num16 == 1 || num16 == 2 || num16 == 3 || num16 == 4))
							{
								if (num16 != 2 || (num16 == 2 && !damagedEventFired))
								{
									AddValueToDictionary(damageWithHitEvents, _npc, dmgTotal);
								}
								if (!damagedEventFired)
								{
									AddValueToDictionary(charactersDamagedEvents, _npc, dmgTotal);
									damagedEventFired = true;
								}
								else if (num16 == 2)
								{
									AddValueToDictionary(charactersDamagedEvents, _npc, dmgTotal);
								}
							}
							if (dmgTotal == 0 && blocked > 0 && (num16 == 3 || num16 == 4 || num16 == 5 || num16 == 6) && !blockedEventFired)
							{
								_npc.SetEvent(Enums.EventActivation.Block, theCasterCharacter, blocked);
								blockedEventFired = true;
							}
							if ((!fullEvadedDmg && num16 == 1) || (!fullEvadedDmgSide1 && num16 == 2) || (!fullEvadedDmgSide1 && num16 == 3) || (!fullEvadedDmgSide2 && num16 == 4))
							{
								AddValueToDictionary(damageReflectedActions, _npc, new DamageReflectedPayload(theCasterHero, theCasterNPC, dmgTotal, blocked));
								yield return Globals.Instance.WaitForSeconds(0.05f);
								AddValueToDictionary(healAttackerActions, _npc, new HealAttackerPayload(theCasterHero, theCasterNPC));
							}
						}
						else
						{
							if (dmgTotal > _hero.GetHp())
							{
								castCardDamageDone = _hero.GetHp();
							}
							else
							{
								castCardDamageDone = dmgTotal;
							}
							castCardDamageDoneTotal += castCardDamageDone;
							castCardDamageDoneIteration += dmgTotal;
							_hero.ModifyHp(-dmgTotal, _includeInStats: true, _refreshHP: true, theCasterCharacter);
							if (IsPrimaryDirectDamage(_hero, _heroBeforeDmg, num16, dmgTotal))
							{
								_hero.TryRemoveAuraCurse("sleep");
							}
							if (dmgTotal > 0 && (num16 == 1 || num16 == 2 || num16 == 3 || num16 == 4))
							{
								if (num16 != 2 || (num16 == 2 && !damagedEventFired))
								{
									AddValueToDictionary(damageWithHitEvents, _hero, dmgTotal);
								}
								if (!damagedEventFired)
								{
									AddValueToDictionary(charactersDamagedEvents, _hero, dmgTotal);
									damagedEventFired = true;
								}
								else if (num16 == 2)
								{
									AddValueToDictionary(charactersDamagedEvents, _hero, dmgTotal);
								}
							}
							if (dmgTotal == 0 && blocked > 0 && (num16 == 3 || num16 == 4 || num16 == 5 || num16 == 6) && !blockedEventFired)
							{
								_hero.SetEvent(Enums.EventActivation.Block, theCasterCharacter, blocked);
								blockedEventFired = true;
							}
							if ((!fullEvadedDmg && num16 == 1) || (!fullEvadedDmgSide1 && num16 == 2) || (!fullEvadedDmgSide1 && num16 == 3) || (!fullEvadedDmgSide2 && num16 == 4))
							{
								AddValueToDictionary(damageReflectedActions, _hero, new DamageReflectedPayload(theCasterHero, theCasterNPC, dmgTotal, blocked));
								yield return Globals.Instance.WaitForSeconds(0.05f);
								_hero.GrantBlockToTeam(theCasterHero, theCasterNPC, dmgTotal, blocked);
								AddValueToDictionary(healAttackerActions, _hero, new HealAttackerPayload(theCasterHero, theCasterNPC));
							}
						}
						if (theCasterIsHero)
						{
							_ = theCasterHero.GameName;
						}
						else
						{
							_ = theCasterNPC.GameName;
						}
						if (isNPC)
						{
							_ = _npc.GameName;
						}
						else
						{
							_ = _hero.GameName;
						}
						_hero = _heroBeforeDmg;
						_heroItem = _heroItemBeforeDmg;
						_npc = _npcBeforeDmg;
						_npcItem = _npcItemBeforeDmg;
						isHero = _isHero;
						isNPC = _isNPC;
						directAttackIteration++;
					}
					foreach (KeyValuePair<NPCItem, CastResolutionForCombatText> item3 in ctNPC)
					{
						item3.Key.ScrollCombatTextDamageNew(item3.Value);
					}
					foreach (KeyValuePair<HeroItem, CastResolutionForCombatText> item4 in ctHero)
					{
						item4.Key.ScrollCombatTextDamageNew(item4.Value);
					}
					yield return Globals.Instance.WaitForSeconds(0.05f);
				}
				foreach (KeyValuePair<Character, int> item5 in charactersDamagedEvents)
				{
					item5.Key.SetEvent(Enums.EventActivation.Damaged, theCasterCharacter, item5.Value);
				}
				foreach (KeyValuePair<Character, int> item6 in damageWithHitEvents)
				{
					theCasterCharacter.SetEvent(Enums.EventActivation.Damage, item6.Key, item6.Value, cardActive.Id);
				}
				foreach (KeyValuePair<Character, DamageReflectedPayload> item7 in damageReflectedActions)
				{
					item7.Key.DamageReflected(item7.Value.theCasterHero, item7.Value.theCasterNPC, item7.Value.dmgTotal, item7.Value.blocked);
				}
				foreach (KeyValuePair<Character, HealAttackerPayload> item8 in healAttackerActions)
				{
					item8.Key.HealAttacker(item8.Value.theCasterHero, item8.Value.theCasterNPC);
				}
				if (castCardDamageDoneIteration > 0f)
				{
					if (castCardDamageDoneIteration > 49f)
					{
						PlayerManager.Instance.AchievementUnlock("MISC_GIANT");
					}
					if (castCardDamageDoneIteration > 149f)
					{
						PlayerManager.Instance.AchievementUnlock("MISC_COLOSSUS");
					}
					if (castCardDamageDoneIteration > 399f)
					{
						PlayerManager.Instance.AchievementUnlock("MISC_BEHEMOTH");
					}
				}
				if (isYourFirstTarget && _cardActive.HealAuraCurseSelf != null && _cardActive.HealAuraCurseSelf.Id == "spellsword")
				{
					theCasterCharacter.HealAuraCurse(_cardActive.HealAuraCurseSelf);
				}
				if (!castedEffect && ((_npc != null && _npc.Alive) || (_hero != null && _hero.Alive)))
				{
					AudioClip soundHit2 = _cardActive.GetSoundHit();
					if (soundHit2 != null)
					{
						float delay2 = 0f;
						if (_cardActive.SoundHitReworkDelay > 0f && !GameManager.Instance.ConfigUseLegacySounds)
						{
							delay2 = _cardActive.SoundHitReworkDelay;
						}
						GameManager.Instance.PlayAudio(soundHit2, 0.25f, delay2);
					}
					if (_cardActive.EffectPreAction == "" && _cardActive.EffectPostCastDelay > 0f && (iteration == 0 || _cardActive.EffectCasterRepeat))
					{
						yield return Globals.Instance.WaitForSeconds(_cardActive.EffectPostCastDelay);
					}
					if (_cardActive.EffectTrail != "")
					{
						Transform transform2 = null;
						Transform to2 = null;
						int num29 = 0;
						int num30 = 0;
						if (theCasterNPC != null && theCasterNPC.NPCItem != null)
						{
							transform2 = theCasterNPC.NPCItem.CharImageT;
							num29 = theCasterNPC.Position;
						}
						else if (theCasterHero != null && theCasterHero.HeroItem != null)
						{
							transform2 = theCasterHero.HeroItem.CharImageT;
							num29 = theCasterHero.Position;
						}
						if (_npc != null && _npcItem != null)
						{
							to2 = _npcItem.CharImageT;
							num30 = _npc.Position;
						}
						else if (_hero != null && _heroItem != null)
						{
							to2 = _heroItem.CharImageT;
							num30 = _hero.Position;
						}
						waitingTrail = true;
						EffectsManager.Instance.PlayEffectTrail(_cardActive, theCasterIsHero, transform2, to2, num29 + num30 + 1);
						while (waitingTrail)
						{
							yield return Globals.Instance.WaitForSeconds(0.01f);
						}
					}
					if (_cardActive.EffectTarget != "")
					{
						if (_npc != null && _npcItem != null && _cardActive.SummonUnit == null)
						{
							EffectsManager.Instance.PlayEffect(_cardActive, isCaster: false, theCasterIsHero, _npcItem.CharImageT);
						}
						else if (_hero != null && _heroItem != null)
						{
							EffectsManager.Instance.PlayEffect(_cardActive, isCaster: false, theCasterIsHero, _heroItem.CharImageT);
						}
					}
					if (_cardActive.EffectPostTargetDelay > 0f)
					{
						yield return Globals.Instance.WaitForSeconds(_cardActive.EffectPostTargetDelay);
					}
				}
				if (_cardActive.SelfHealthLoss > 0)
				{
					int damage = _cardActive.SelfHealthLoss;
					if (_cardActive.SelfHealthLossSpecialGlobal)
					{
						damage = _cardSpecialValueGlobal;
					}
					else if (_cardActive.SelfHealthLossSpecialValue1)
					{
						damage = exahustGenerating;
					}
					else if (_cardActive.SelfHealthLossSpecialValue2)
					{
						damage = cardSpecialValue2;
					}
					if (theCasterHero != null && theCasterHero.Alive)
					{
						theCasterHero.IndirectDamage(Enums.DamageType.None, damage, theCasterHero);
						yield return Globals.Instance.WaitForSeconds(0.01f);
					}
					else if (theCasterNPC != null && theCasterNPC.Alive)
					{
						theCasterNPC.IndirectDamage(Enums.DamageType.None, damage, theCasterNPC);
						yield return Globals.Instance.WaitForSeconds(0.01f);
					}
				}
				yield return Globals.Instance.WaitForSeconds(0.01f);
				if ((theCasterHero != null && theCasterHero.Alive) || (theCasterNPC != null && theCasterNPC.Alive))
				{
					int directAttackIteration = 0;
					if (_cardActive.Aura != null || _cardActive.Aura2 != null || _cardActive.Aura3 != null || (_cardActive.AcEnergyBonus != null && _cardActive.AcEnergyBonus.IsAura) || (_cardActive.Auras != null && _cardActive.Auras.Length != 0))
					{
						Character targetCharacter = ((theCasterHero != null) ? ((Character)_npc) : ((Character)_hero));
						bool ignoreBlock = false;
						bool fullEvadedDmgSide2 = false;
						bool fullEvadedDmgSide1 = false;
						bool fullEvadedDmg = false;
						bool blockedEventFired = _cardActive.Auras != null && _cardActive.Auras.Length != 0;
						int dmgTotal = (blockedEventFired ? (_cardActive.Auras.Length + 5) : 5);
						for (int dmg = 0; dmg < dmgTotal; dmg++)
						{
							bool damagedEventFired = false;
							AuraCurseData auraCurseData2 = null;
							if (dmg == 0 && !_cardActive.ChooseOneOfAvailableAuras && _cardActive.Aura != null)
							{
								auraCurseData2 = _cardActive.Aura;
								directAttackIteration = ResolveAuraCurseCharges(auraCurseData2, _cardActive.AuraCharges, _cardActive.AuraChargesSpecialValueGlobal, _cardActive.AuraChargesSpecialValue1, _cardActive.AuraChargesSpecialValue2, targetCharacter, _cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2, null, _cardActive, theCasterCharacter);
								if (_cardActive.AuraCharges2 > 0 && _cardActive.Aura == _cardActive.Aura2)
								{
									directAttackIteration += ResolveAuraCurseCharges(auraCurseData2, 0, _cardActive.AuraCharges2SpecialValueGlobal, _cardActive.AuraCharges2SpecialValue1, _cardActive.AuraCharges2SpecialValue2, targetCharacter, _cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2, _cardActive.AuraCharges2, _cardActive, theCasterCharacter);
									fullEvadedDmgSide1 = true;
								}
							}
							else if (dmg == 1 && !_cardActive.ChooseOneOfAvailableAuras && _cardActive.Aura2 != null)
							{
								if (fullEvadedDmgSide1)
								{
									continue;
								}
								auraCurseData2 = _cardActive.Aura2;
								directAttackIteration = ResolveAuraCurseCharges(auraCurseData2, _cardActive.AuraCharges2, _cardActive.AuraCharges2SpecialValueGlobal, _cardActive.AuraCharges2SpecialValue1, _cardActive.AuraCharges2SpecialValue2, targetCharacter, _cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2, null, _cardActive, theCasterCharacter);
								if (_cardActive.AuraCharges3 > 0 && _cardActive.Aura2 == _cardActive.Aura3)
								{
									directAttackIteration += ResolveAuraCurseCharges(auraCurseData2, 0, _cardActive.AuraCharges3SpecialValueGlobal, _cardActive.AuraCharges3SpecialValue1, _cardActive.AuraCharges3SpecialValue2, targetCharacter, _cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2, _cardActive.AuraCharges3, _cardActive, theCasterCharacter);
									fullEvadedDmg = true;
								}
							}
							else if (dmg == 2 && !_cardActive.ChooseOneOfAvailableAuras && _cardActive.Aura3 != null)
							{
								if (fullEvadedDmg)
								{
									continue;
								}
								auraCurseData2 = _cardActive.Aura3;
								directAttackIteration = ResolveAuraCurseCharges(auraCurseData2, _cardActive.AuraCharges3, _cardActive.AuraCharges3SpecialValueGlobal, _cardActive.AuraCharges3SpecialValue1, _cardActive.AuraCharges3SpecialValue2, targetCharacter, _cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2, null, _cardActive, theCasterCharacter);
							}
							else if (blockedEventFired && dmg >= 3 && dmg <= dmgTotal - 3)
							{
								CardData.AuraBuffs auraBuffs = _cardActive.Auras[dmg - 3];
								if (auraBuffs != null && auraBuffs.aura != null)
								{
									auraCurseData2 = auraBuffs.aura;
									directAttackIteration = ResolveAuraCurseCharges(auraCurseData2, auraBuffs.auraCharges, auraBuffs.auraChargesSpecialValueGlobal, auraBuffs.auraChargesSpecialValue1, auraBuffs.auraChargesSpecialValue2, targetCharacter, _cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2, null, _cardActive, theCasterCharacter);
								}
							}
							else if (_cardActive.ChooseOneOfAvailableAuras)
							{
								damagedEventFired = true;
								List<AuraCurseData> list8 = new List<AuraCurseData>();
								List<int> list9 = new List<int>();
								if ((bool)_cardActive.Aura)
								{
									list8.Add(_cardActive.Aura);
									list9.Add(_cardActive.AuraCharges);
								}
								if ((bool)_cardActive.Aura2)
								{
									list8.Add(_cardActive.Aura2);
									list9.Add(_cardActive.AuraCharges2);
								}
								if ((bool)_cardActive.Aura3)
								{
									list8.Add(_cardActive.Aura3);
									list9.Add(_cardActive.AuraCharges3);
								}
								if (_cardActive.Auras != null && _cardActive.Auras.Length != 0)
								{
									list8.AddRange(from x in _cardActive.Auras
										where x.aura != null
										select x.aura);
									list9.AddRange(from x in _cardActive.Auras
										where x.aura != null
										select x.auraCharges);
								}
								int randomIntRange2 = GetRandomIntRange(0, list8.Count, "misc");
								auraCurseData2 = list8[randomIntRange2];
								directAttackIteration = list9[randomIntRange2];
							}
							if (energyAssigned > 0 && _cardActive.AcEnergyBonus != null && _cardActive.AcEnergyBonus.IsAura)
							{
								if (dmg == dmgTotal - 2 && !ignoreBlock)
								{
									auraCurseData2 = _cardActive.AcEnergyBonus;
									directAttackIteration = _cardActive.AcEnergyBonusQuantity * energyAssigned;
									ignoreBlock = true;
								}
								else if (!ignoreBlock && auraCurseData2 != null && auraCurseData2.Id == _cardActive.AcEnergyBonus.Id)
								{
									directAttackIteration += _cardActive.AcEnergyBonusQuantity * energyAssigned;
									ignoreBlock = true;
								}
							}
							if (energyAssigned > 0 && _cardActive.AcEnergyBonus2 != null && _cardActive.AcEnergyBonus2.IsAura)
							{
								if (dmg == dmgTotal - 1 && !fullEvadedDmgSide2)
								{
									auraCurseData2 = _cardActive.AcEnergyBonus2;
									directAttackIteration = _cardActive.AcEnergyBonus2Quantity * energyAssigned;
									fullEvadedDmgSide2 = true;
								}
								else if (!fullEvadedDmgSide2 && auraCurseData2 != null && auraCurseData2.Id == _cardActive.AcEnergyBonus2.Id)
								{
									directAttackIteration += _cardActive.AcEnergyBonus2Quantity * energyAssigned;
									fullEvadedDmgSide2 = true;
								}
							}
							if (auraCurseData2 != null)
							{
								if (isHero)
								{
									if (_heroItem != null)
									{
										_hero.SetAura(theCasterCharacter, auraCurseData2, directAttackIteration, fromTrait: false, _cardActive.CardClass);
									}
									yield return Globals.Instance.WaitForSeconds(0.01f);
								}
								else
								{
									if (_npcItem != null)
									{
										_npc.SetAura(theCasterCharacter, auraCurseData2, directAttackIteration, fromTrait: false, _cardActive.CardClass);
									}
									yield return Globals.Instance.WaitForSeconds(0.01f);
								}
							}
							yield return Globals.Instance.WaitForSeconds(0.01f);
							doRedrawInitiatives = true;
							if (damagedEventFired)
							{
								break;
							}
						}
					}
					directAttackIteration = 0;
					if (isYourFirstTarget && (_cardActive.AuraSelf != null || _cardActive.AuraSelf2 != null || _cardActive.AuraSelf3 != null || (_cardActive.Auras != null && _cardActive.Auras.Length != 0)))
					{
						Character targetCharacter = ((theCasterHero != null) ? ((Character)_npc) : ((Character)_hero));
						int dmgTotal = ((_cardActive.Auras != null && _cardActive.Auras.Length != 0) ? (_cardActive.Auras.Length + 3) : 3);
						for (int dmg = 0; dmg < dmgTotal; dmg++)
						{
							AuraCurseData auraCurseData2 = null;
							if (dmg == 0 && _cardActive.AuraSelf != null)
							{
								auraCurseData2 = _cardActive.AuraSelf;
								directAttackIteration = ResolveAuraCurseCharges(auraCurseData2, _cardActive.AuraCharges, _cardActive.AuraChargesSpecialValueGlobal, _cardActive.AuraChargesSpecialValue1, _cardActive.AuraChargesSpecialValue2, targetCharacter, _cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2, null, _cardActive, theCasterCharacter);
							}
							else if (dmg == 1 && _cardActive.AuraSelf2 != null)
							{
								auraCurseData2 = _cardActive.AuraSelf2;
								directAttackIteration = ResolveAuraCurseCharges(auraCurseData2, _cardActive.AuraCharges2, _cardActive.AuraCharges2SpecialValueGlobal, _cardActive.AuraCharges2SpecialValue1, _cardActive.AuraCharges2SpecialValue2, targetCharacter, _cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2, null, _cardActive, theCasterCharacter);
							}
							else if (dmg == 2 && _cardActive.AuraSelf3 != null)
							{
								auraCurseData2 = _cardActive.AuraSelf3;
								directAttackIteration = ResolveAuraCurseCharges(auraCurseData2, _cardActive.AuraCharges3, _cardActive.AuraCharges3SpecialValueGlobal, _cardActive.AuraCharges3SpecialValue1, _cardActive.AuraCharges3SpecialValue2, targetCharacter, _cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2, null, _cardActive, theCasterCharacter);
							}
							else if (dmg > 2)
							{
								CardData.AuraBuffs auraBuffs2 = _cardActive.Auras[dmg - 3];
								if (auraBuffs2 != null && auraBuffs2.auraSelf != null)
								{
									auraCurseData2 = auraBuffs2.auraSelf;
									directAttackIteration = ResolveAuraCurseCharges(auraCurseData2, auraBuffs2.auraCharges, auraBuffs2.auraChargesSpecialValueGlobal, auraBuffs2.auraChargesSpecialValue1, auraBuffs2.auraChargesSpecialValue2, targetCharacter, _cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2, null, _cardActive, theCasterCharacter);
								}
							}
							if (auraCurseData2 != null)
							{
								if (theCasterHero != null && theCasterHero.Alive)
								{
									theCasterHero.SetAura(theCasterCharacter, auraCurseData2, directAttackIteration, fromTrait: false, _cardActive.CardClass);
									yield return Globals.Instance.WaitForSeconds(0.01f);
								}
								else if (theCasterNPC != null && theCasterNPC.Alive)
								{
									theCasterNPC.SetAura(theCasterCharacter, auraCurseData2, directAttackIteration, fromTrait: false, _cardActive.CardClass);
									yield return Globals.Instance.WaitForSeconds(0.01f);
								}
								if (!doRedrawInitiatives)
								{
									doRedrawInitiatives = true;
								}
								yield return Globals.Instance.WaitForSeconds(0.01f);
							}
						}
					}
					directAttackIteration = 0;
					int num16 = 0;
					if (_cardActive.ConvertAllDebuffsIntoCurse)
					{
						GameUtils.ConvertAllDebuffsIntoSelectedCurse(_cardActive, (_hero != null) ? ((Character)_hero) : ((Character)_npc));
					}
					if (_cardActive.Curse != null || _cardActive.Curse2 != null || _cardActive.Curse3 != null || (_cardActive.AcEnergyBonus != null && !_cardActive.AcEnergyBonus.IsAura) || (_cardActive.Curses != null && _cardActive.Curses.Length != 0))
					{
						Character targetCharacter = ((theCasterHero != null) ? ((Character)_npc) : ((Character)_hero));
						bool blockedEventFired = false;
						bool fullEvadedDmg = false;
						bool fullEvadedDmgSide1 = _cardActive.Curses != null && _cardActive.Curses.Length != 0;
						int dmgTotal = (fullEvadedDmgSide1 ? (_cardActive.Curses.Length + 5) : 5);
						for (int dmg = 0; dmg < dmgTotal; dmg++)
						{
							AuraCurseData auraCurseData3 = null;
							if (dmg == 0 && _cardActive.Curse != null)
							{
								auraCurseData3 = _cardActive.Curse;
								directAttackIteration = ResolveAuraCurseCharges(auraCurseData3, _cardActive.CurseCharges, _cardActive.CurseChargesSpecialValueGlobal, _cardActive.CurseChargesSpecialValue1, _cardActive.CurseChargesSpecialValue2, targetCharacter, _cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2, null, _cardActive, theCasterCharacter);
								if (_cardActive.Curse?.Id == _cardActive.Curse2?.Id)
								{
									directAttackIteration += ResolveAuraCurseCharges(_cardActive.Curse2, _cardActive.CurseCharges2, _cardActive.CurseCharges2SpecialValueGlobal, _cardActive.CurseCharges2SpecialValue1, _cardActive.CurseCharges2SpecialValue2, targetCharacter, _cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2);
								}
								num16 = _cardActive.CurseChargesSides;
							}
							else if (dmg == 1 && _cardActive.Curse2 != null)
							{
								if (_cardActive.Curse?.Id != _cardActive.Curse2?.Id)
								{
									auraCurseData3 = _cardActive.Curse2;
									directAttackIteration = ResolveAuraCurseCharges(auraCurseData3, _cardActive.CurseCharges2, _cardActive.CurseCharges2SpecialValueGlobal, _cardActive.CurseCharges2SpecialValue1, _cardActive.CurseCharges2SpecialValue2, targetCharacter, _cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2, null, _cardActive, theCasterCharacter);
								}
							}
							else if (dmg == 2 && _cardActive.Curse3 != null)
							{
								auraCurseData3 = _cardActive.Curse3;
								directAttackIteration = ResolveAuraCurseCharges(auraCurseData3, _cardActive.CurseCharges3, _cardActive.CurseCharges3SpecialValueGlobal, _cardActive.CurseCharges3SpecialValue1, _cardActive.CurseCharges3SpecialValue2, targetCharacter, _cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2, null, _cardActive, theCasterCharacter);
							}
							else if (fullEvadedDmgSide1 && dmg >= 3 && dmg <= dmgTotal - 3)
							{
								CardData.CurseDebuffs curseDebuffs = _cardActive.Curses[dmg - 3];
								if (curseDebuffs != null && curseDebuffs.curse != null)
								{
									auraCurseData3 = curseDebuffs.curse;
									directAttackIteration = ResolveAuraCurseCharges(auraCurseData3, curseDebuffs.curseCharges, curseDebuffs.curseChargesSpecialValueGlobal, curseDebuffs.curseChargesSpecialValue1, curseDebuffs.curseChargesSpecialValue2, targetCharacter, _cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2, null, _cardActive, theCasterCharacter);
									num16 = curseDebuffs.curseChargesSides;
								}
							}
							if (energyAssigned > 0 && _cardActive.AcEnergyBonus != null && !_cardActive.AcEnergyBonus.IsAura)
							{
								if (dmg == dmgTotal - 2 && !blockedEventFired)
								{
									auraCurseData3 = _cardActive.AcEnergyBonus;
									directAttackIteration = _cardActive.AcEnergyBonusQuantity * energyAssigned;
									blockedEventFired = true;
								}
								else if (!blockedEventFired && auraCurseData3 != null && auraCurseData3.Id == _cardActive.AcEnergyBonus.Id)
								{
									directAttackIteration += _cardActive.AcEnergyBonusQuantity * energyAssigned;
									blockedEventFired = true;
								}
							}
							if (energyAssigned > 0 && _cardActive.AcEnergyBonus2 != null && !_cardActive.AcEnergyBonus2.IsAura)
							{
								if (dmg == dmgTotal - 1 && !fullEvadedDmg)
								{
									auraCurseData3 = _cardActive.AcEnergyBonus2;
									directAttackIteration = _cardActive.AcEnergyBonus2Quantity * energyAssigned;
									fullEvadedDmg = true;
								}
								else if (!fullEvadedDmg && auraCurseData3 != null && auraCurseData3.Id == _cardActive.AcEnergyBonus2.Id)
								{
									directAttackIteration += _cardActive.AcEnergyBonus2Quantity * energyAssigned;
									fullEvadedDmg = true;
								}
							}
							if (auraCurseData3 != null)
							{
								if (isHero)
								{
									if (_hero != null && _hero.Alive && (!_hero.IsInvulnerable() || !(auraCurseData3.Id.ToLower() != "doom")))
									{
										_hero.SetAura(theCasterCharacter, auraCurseData3, directAttackIteration, fromTrait: false, _cardActive.CardClass);
										foreach (Hero heroSide in GetHeroSides(_hero.Position))
										{
											heroSide.SetAura(theCasterCharacter, auraCurseData3, num16, fromTrait: false, _cardActive.CardClass);
										}
										if (castCardDamageDoneTotal <= 0f)
										{
											_hero.HeroItem.CharacterHitted();
										}
										yield return Globals.Instance.WaitForSeconds(0.01f);
									}
								}
								else if (_npc != null && _npc.Alive && !_npc.IsInvulnerable())
								{
									_npc.SetAura(theCasterCharacter, auraCurseData3, directAttackIteration, fromTrait: false, _cardActive.CardClass);
									foreach (NPC nPCSide in GetNPCSides(_npc.Position))
									{
										nPCSide.SetAura(theCasterCharacter, auraCurseData3, num16, fromTrait: false, _cardActive.CardClass);
									}
									if (castCardDamageDoneTotal <= 0f)
									{
										_npc.NPCItem.CharacterHitted();
									}
									yield return Globals.Instance.WaitForSeconds(0.01f);
								}
							}
							if (!doRedrawInitiatives)
							{
								doRedrawInitiatives = true;
							}
							yield return Globals.Instance.WaitForSeconds(0.01f);
						}
					}
					directAttackIteration = 0;
					if (isYourFirstTarget && (_cardActive.CurseSelf != null || _cardActive.CurseSelf2 != null || _cardActive.CurseSelf3 != null || (_cardActive.Curses != null && _cardActive.Curses.Length != 0)))
					{
						Character targetCharacter = ((theCasterHero != null) ? ((Character)_npc) : ((Character)_hero));
						bool fullEvadedDmgSide1 = _cardActive.Curses != null && _cardActive.Curses.Length != 0;
						int dmgTotal = (fullEvadedDmgSide1 ? (_cardActive.Curses.Length + 3) : 3);
						for (int dmg = 0; dmg < dmgTotal; dmg++)
						{
							AuraCurseData auraCurseData3 = null;
							if (dmg == 0 && _cardActive.CurseSelf != null)
							{
								auraCurseData3 = _cardActive.CurseSelf;
								directAttackIteration = ResolveAuraCurseCharges(auraCurseData3, _cardActive.CurseCharges, _cardActive.CurseChargesSpecialValueGlobal, _cardActive.CurseChargesSpecialValue1, _cardActive.CurseChargesSpecialValue2, targetCharacter, _cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2, null, _cardActive, theCasterCharacter);
							}
							else if (dmg == 1 && _cardActive.CurseSelf2 != null)
							{
								auraCurseData3 = _cardActive.CurseSelf2;
								directAttackIteration = ResolveAuraCurseCharges(auraCurseData3, _cardActive.CurseCharges2, _cardActive.CurseCharges2SpecialValueGlobal, _cardActive.CurseCharges2SpecialValue1, _cardActive.CurseCharges2SpecialValue2, targetCharacter, _cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2, null, _cardActive, theCasterCharacter);
							}
							else if (dmg == 2 && _cardActive.CurseSelf3 != null)
							{
								auraCurseData3 = _cardActive.CurseSelf3;
								directAttackIteration = ResolveAuraCurseCharges(auraCurseData3, _cardActive.CurseCharges3, _cardActive.CurseCharges3SpecialValueGlobal, _cardActive.CurseCharges3SpecialValue1, _cardActive.CurseCharges3SpecialValue2, targetCharacter, _cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2, null, _cardActive, theCasterCharacter);
							}
							else if (fullEvadedDmgSide1 && dmg >= 3 && dmg <= dmgTotal - 3)
							{
								CardData.CurseDebuffs curseDebuffs2 = _cardActive.Curses[dmg - 3];
								if (curseDebuffs2 != null && curseDebuffs2.curseSelf != null)
								{
									auraCurseData3 = curseDebuffs2.curseSelf;
									directAttackIteration = ResolveAuraCurseCharges(auraCurseData3, curseDebuffs2.curseCharges, curseDebuffs2.curseChargesSpecialValueGlobal, curseDebuffs2.curseChargesSpecialValue1, curseDebuffs2.curseChargesSpecialValue2, targetCharacter, _cardSpecialValueGlobal, exahustGenerating, cardSpecialValue2, null, _cardActive, theCasterCharacter);
								}
							}
							if (auraCurseData3 != null)
							{
								if (theHero != null)
								{
									if (!theHero.IsInvulnerable())
									{
										theHero.SetAura(theCasterCharacter, auraCurseData3, directAttackIteration, fromTrait: false, _cardActive.CardClass);
										if (theCasterCharacter != theHero)
										{
											theCasterCharacter.SetEvent(Enums.EventActivation.AuraCurseSet, theHero, directAttackIteration, auraCurseData3.ACName);
										}
										if (castCardDamageDoneTotal <= 0f)
										{
											theHero.HeroItem.CharacterHitted();
										}
										yield return Globals.Instance.WaitForSeconds(0.01f);
									}
								}
								else if (!theNPC.IsInvulnerable())
								{
									theNPC.SetAura(theCasterCharacter, auraCurseData3, directAttackIteration, fromTrait: false, _cardActive.CardClass);
									if (theCasterCharacter != theNPC)
									{
										theCasterCharacter.SetEvent(Enums.EventActivation.AuraCurseSet, theNPC, directAttackIteration, auraCurseData3.ACName);
									}
									if (castCardDamageDoneTotal <= 0f)
									{
										theNPC.NPCItem.CharacterHitted();
									}
									yield return Globals.Instance.WaitForSeconds(0.01f);
								}
							}
							if (!doRedrawInitiatives)
							{
								doRedrawInitiatives = true;
							}
							yield return Globals.Instance.WaitForSeconds(0.01f);
						}
					}
					int blocked = 0;
					if (_cardActive.HealEnergyBonus > 0 && energyAssigned > 0)
					{
						blocked += _cardActive.HealEnergyBonus * energyAssigned;
					}
					if (_cardActive.Heal > 0)
					{
						CastResolutionForCombatText castResolutionForCombatText = new CastResolutionForCombatText();
						int num31 = _cardActive.Heal + blocked;
						if (_cardActive.HealSpecialValueGlobal)
						{
							num31 += _cardSpecialValueGlobal;
						}
						else if (_cardActive.HealSpecialValue1)
						{
							num31 += exahustGenerating;
						}
						else if (_cardActive.HealSpecialValue2)
						{
							num31 += cardSpecialValue2;
						}
						if (theCasterHero != null)
						{
							num31 = theCasterHero.HealWithCharacterBonus(num31, _cardActive.CardClass);
						}
						else if (theCasterNPC != null)
						{
							num31 = theCasterNPC.HealWithCharacterBonus(num31, _cardActive.CardClass);
						}
						if (effectRepeatPercent != 100f)
						{
							num31 = Functions.FuncRoundToInt((float)num31 * effectRepeatPercent * 0.01f);
						}
						if (_hero != null && _hero.Alive)
						{
							num31 = _hero.HealReceivedFinal(num31);
							if (_hero.GetHpLeftForMax() < num31)
							{
								num31 = _hero.GetHpLeftForMax();
							}
							if (_hero.HpCurrent + num31 >= _hero.GetMaxHP())
							{
								_hero.SetEvent(Enums.EventActivation.Overhealed);
							}
							if (num31 > 0)
							{
								_hero.SetEvent(Enums.EventActivation.Healed);
								theCasterCharacter.SetEvent(Enums.EventActivation.Heal, _hero);
								_hero.ModifyHp(num31);
								castResolutionForCombatText.heal = num31;
								if (_heroItem != null)
								{
									_heroItem.ScrollCombatTextDamageNew(castResolutionForCombatText);
								}
							}
						}
						else if (_npc != null && _npc.Alive)
						{
							num31 = _npc.HealReceivedFinal(num31);
							if (_npc.GetHpLeftForMax() < num31)
							{
								num31 = _npc.GetHpLeftForMax();
							}
							if (_npc.HpCurrent + num31 >= _npc.GetMaxHP())
							{
								_npc.SetEvent(Enums.EventActivation.Overhealed);
							}
							if (num31 > 0)
							{
								_npc.SetEvent(Enums.EventActivation.Healed);
								theCasterCharacter.SetEvent(Enums.EventActivation.Heal, _npc);
								_npc.ModifyHp(num31);
								castResolutionForCombatText.heal = num31;
								if (_npcItem != null)
								{
									_npcItem.ScrollCombatTextDamageNew(castResolutionForCombatText);
								}
							}
						}
						yield return null;
					}
					if (_cardActive.HealSelf != 0 || _cardActive.HealSelfPerDamageDonePercent != 0f)
					{
						yield return Globals.Instance.WaitForSeconds(0.1f);
						CastResolutionForCombatText castResolutionForCombatText2 = new CastResolutionForCombatText();
						int num32 = 0;
						if (_cardActive.HealSelf > 0)
						{
							num32 = _cardActive.HealSelf;
						}
						if (_cardActive.HealSelfPerDamageDonePercent != 0f)
						{
							num32 += Functions.FuncRoundToInt(castCardDamageDone * _cardActive.HealSelfPerDamageDonePercent * 0.01f);
						}
						if (_cardActive.HealSelfSpecialValueGlobal)
						{
							num32 = _cardSpecialValueGlobal;
						}
						else if (_cardActive.HealSelfSpecialValue1)
						{
							num32 = exahustGenerating;
						}
						else if (_cardActive.HealSelfSpecialValue2)
						{
							num32 = cardSpecialValue2;
						}
						if (num32 > 0)
						{
							num32 += blocked;
							if (theCasterHero != null)
							{
								theCasterHero.SetEvent(Enums.EventActivation.Heal, theCasterHero);
								theCasterHero.SetEvent(Enums.EventActivation.Healed);
								num32 = theCasterHero.HealWithCharacterBonus(num32, _cardActive.CardClass);
								num32 = theCasterHero.HealReceivedFinal(num32);
								if (theCasterHero.HpCurrent + num32 >= theCasterHero.GetMaxHP())
								{
									theCasterHero.SetEvent(Enums.EventActivation.Overhealed);
								}
								theCasterHero.ModifyHp(num32);
								castResolutionForCombatText2.heal = num32;
								if (theCasterHero.HeroItem != null)
								{
									theCasterHero.HeroItem.ScrollCombatTextDamageNew(castResolutionForCombatText2);
								}
							}
							else if (theCasterNPC != null)
							{
								theCasterNPC.SetEvent(Enums.EventActivation.Heal);
								theCasterNPC.SetEvent(Enums.EventActivation.Healed);
								num32 = theCasterNPC.HealWithCharacterBonus(num32, _cardActive.CardClass);
								num32 = theCasterNPC.HealReceivedFinal(num32);
								if (theCasterNPC.HpCurrent + num32 >= theCasterNPC.GetMaxHP())
								{
									theCasterNPC.SetEvent(Enums.EventActivation.Overhealed);
								}
								theCasterNPC.ModifyHp(num32);
								castResolutionForCombatText2.heal = num32;
								if (theCasterNPC.NPCItem != null)
								{
									theCasterNPC.NPCItem.ScrollCombatTextDamageNew(castResolutionForCombatText2);
								}
							}
						}
						yield return null;
					}
					if (_cardActive.HealSides > 0)
					{
						CastResolutionForCombatText castResolutionForCombatText3 = new CastResolutionForCombatText();
						int num33 = _cardActive.HealSides + blocked;
						if (theCasterHero != null)
						{
							num33 = theCasterHero.HealWithCharacterBonus(num33, _cardActive.CardClass);
						}
						else if (theCasterNPC != null)
						{
							num33 = theCasterNPC.HealWithCharacterBonus(num33, _cardActive.CardClass);
						}
						if (_hero != null && _hero.Alive)
						{
							List<Hero> heroSides = GetHeroSides(_hero.Position);
							for (int num34 = 0; num34 < heroSides.Count; num34++)
							{
								num33 = heroSides[num34].HealReceivedFinal(num33);
								heroSides[num34].ModifyHp(num33);
								castResolutionForCombatText3.heal = num33;
								heroSides[num34].HeroItem.ScrollCombatTextDamageNew(castResolutionForCombatText3);
							}
						}
						else if (_npc != null && _npc.Alive)
						{
							List<NPC> nPCSides = GetNPCSides(_npc.Position);
							for (int num35 = 0; num35 < nPCSides.Count; num35++)
							{
								num33 = nPCSides[num35].HealReceivedFinal(num33);
								nPCSides[num35].ModifyHp(num33);
								castResolutionForCombatText3.heal = num33;
								nPCSides[num35].NPCItem.ScrollCombatTextDamageNew(castResolutionForCombatText3);
							}
						}
						yield return null;
					}
					if (_cardActive.KillPet && _hero != null && _hero.Alive && _hero.Pet != "" && _hero.Pet != "harleyrare" && _hero.Pet != "templelurkerpetrare" && _hero.Pet != "mentalscavengerspetrare")
					{
						DestroyedItemInThisTurn(_hero.HeroIndex, _hero.Pet);
						_hero.Pet = "tombstone";
						CreatePet(Globals.Instance.GetCardData("tombstone", instantiate: false), _hero.HeroItem.gameObject, _hero, null);
						_hero.RefreshItems(5);
						RefreshItems();
					}
					if (_hero != null && _hero.Alive)
					{
						if (_cardActive.EnergyRecharge != 0)
						{
							_hero.ModifyEnergy(_cardActive.EnergyRecharge, showScrollCombatText: true);
						}
						else if (_cardActive.EnergyRechargeSpecialValueGlobal && _cardSpecialValueGlobal != 0)
						{
							_hero.ModifyEnergy(_cardSpecialValueGlobal, showScrollCombatText: true);
						}
					}
				}
				if ((_hero != null && !_hero.Alive) || (theCasterHero != null && !theCasterHero.Alive) || (_npc != null && !_npc.Alive) || (theCasterNPC != null && !theCasterNPC.Alive))
				{
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("[COMBAT LOOP] Waiting Hero/Monster Kill");
					}
					yield return Globals.Instance.WaitForSeconds(0.3f);
					while (waitingKill)
					{
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("[COMBAT LOOP] Waiting Hero/Monster Kill");
						}
						yield return Globals.Instance.WaitForSeconds(0.1f);
					}
				}
				if (iteration < repeatCast - 1)
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
					while (waitingKill)
					{
						yield return Globals.Instance.WaitForSeconds(0.05f);
					}
					if (GameManager.Instance.IsMultiplayer() && !_automatic)
					{
						string codeGen = GenerateSyncCodeForCheckingAction();
						int blocked = 0;
						while (blocked < 3)
						{
							yield return Globals.Instance.WaitForSeconds(0.01f);
							string text2 = GenerateSyncCodeForCheckingAction();
							if (codeGen == text2)
							{
								blocked++;
								continue;
							}
							codeGen = text2;
							blocked = 0;
						}
					}
					while (waitingKill)
					{
						yield return Globals.Instance.WaitForSeconds(0.05f);
					}
					if ((theCasterNPC != null && !theCasterNPC.Alive) || (theCasterHero != null && !theCasterHero.Alive))
					{
						iteration = 100;
					}
					else
					{
						if (youCanCastEffect && targetTransformCast != null && _cardActive.EffectPreAction == "")
						{
							if (_cardActive.EffectRepeatDelay > 0f)
							{
								if (GameManager.Instance.IsMultiplayer())
								{
									yield return Globals.Instance.WaitForSeconds(_cardActive.EffectRepeatDelay * 0.3f);
								}
								else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
								{
									yield return Globals.Instance.WaitForSeconds(_cardActive.EffectRepeatDelay);
								}
								else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Ultrafast)
								{
									yield return null;
								}
								else
								{
									yield return Globals.Instance.WaitForSeconds(_cardActive.EffectRepeatDelay * 0.2f);
								}
							}
							if (_automatic)
							{
								if (theNPC != null && theNPC.NPCItem != null)
								{
									if (_cardActive.MoveToCenter && !IsPhantomArmorSpecialCard(_cardActive.Id))
									{
										theNPC.NPCItem.CharacterAttackAnim();
									}
									else if (_cardActive.EffectCasterRepeat && !_cardActive.IsPetCast && !IsPhantomArmorSpecialCard(_cardActive.Id))
									{
										theNPC.NPCItem.CharacterCastAnim();
									}
								}
							}
							else if (theHero != null && theHero.HeroItem != null)
							{
								if (_cardActive.MoveToCenter)
								{
									theHero.HeroItem.CharacterAttackAnim();
								}
								else if (_cardActive.EffectCasterRepeat && !_cardActive.IsPetCast)
								{
									theHero.HeroItem.CharacterCastAnim();
								}
							}
						}
						if (GameManager.Instance.IsMultiplayer())
						{
							yield return Globals.Instance.WaitForSeconds(0.15f);
						}
						else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
						{
							yield return Globals.Instance.WaitForSeconds(0.15f);
						}
						else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Ultrafast)
						{
							yield return Globals.Instance.WaitForSeconds(0.01f);
						}
						else
						{
							yield return Globals.Instance.WaitForSeconds(0.1f);
						}
					}
				}
				while (waitingKill)
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
			}
			if (matchIsOver)
			{
				yield break;
			}
			int _indexQueue = 0;
			while (ctQueue.Count > 0)
			{
				if (combatCounter % 200 == 0 && GameManager.Instance.GetDeveloperMode())
				{
					Debug.Log("Queue loop " + ctQueue.Count + "//" + eventList.Count);
				}
				yield return Globals.Instance.WaitForSeconds(0.01f);
				_indexQueue++;
				if (_indexQueue > 25)
				{
					break;
				}
			}
		}
		if (repeatCast > 1 && BossNpc != null)
		{
			BossNpc.OnRepeatCardIterationsFinished();
		}
		if (_cardActive.SelfKillHiddenSeconds > 0f)
		{
			yield return Globals.Instance.WaitForSeconds(_cardActive.SelfKillHiddenSeconds);
			if (theNPC != null && theNPC.NPCItem != null)
			{
				theNPC.KillCharacterFromOutside();
			}
			else if (theHero != null && theHero.HeroItem != null)
			{
				theHero.KillCharacterFromOutside();
			}
			yield return Globals.Instance.WaitForSeconds(0.1f);
		}
		if (repeatCast == 0)
		{
			while (waitingKill)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[COMBAT LOOP] Waiting Hero/Monster Kill at end  of cast for NO REPEAT CARDS");
				}
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
		}
		if (cardIteration.ContainsKey(_cardActive.InternalId))
		{
			cardIteration[_cardActive.InternalId]++;
			if (cardIteration[_cardActive.InternalId] < _cardIterationTotal)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[cardIteration] EXIT HERE BECAUSE " + cardIteration[_cardActive.InternalId] + "<" + _cardIterationTotal, "trace");
				}
				yield break;
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[cardIteration] CONTINUE " + cardIteration[_cardActive.InternalId] + ">=" + _cardIterationTotal, "trace");
			}
			if (theCasterHero != null && theCasterHero.Alive && energyJustWastedByHero > 5 && AtOManager.Instance.CharacterHavePerk(theCasterHero.SubclassName, "mainperkenergy2a"))
			{
				theCasterHero.ModifyEnergy(1, showScrollCombatText: true);
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("***** [cardIteration] castFinalIteration ** " + _cardActive.Id + " (" + cardIteration[_cardActive.InternalId] + ") ******", "trace");
			}
			if (_cardActive.MoveToCenter)
			{
				if (theCasterIsHero)
				{
					if (theCasterHero != null && theCasterHero.Alive && theCasterHero.HeroItem != null)
					{
						yield return Globals.Instance.WaitForSeconds(0.2f);
						theCasterHero.HeroItem.MoveToCenterBack();
					}
				}
				else if (theCasterNPC != null && theCasterNPC.Alive && theCasterNPC.NPCItem != null)
				{
					yield return Globals.Instance.WaitForSeconds(0.2f);
					theCasterNPC.NPCItem.MoveToCenterBack();
				}
			}
			if (doRedrawInitiatives)
			{
				ReDrawInitiatives();
			}
			if (MatchIsOver)
			{
				yield break;
			}
			if (GameManager.Instance.IsMultiplayer() && !_automatic)
			{
				bool castedEffect = false;
				while (!castedEffect)
				{
					if (castingCardListMP.Count <= 1 || castingCardListMP[castingCardListMP.Count - 1] == _cardActive.Id)
					{
						castedEffect = true;
					}
					if (!castedEffect)
					{
						yield return Globals.Instance.WaitForSeconds(0.01f);
					}
				}
				if (!_cardActive.AutoplayDraw)
				{
					if (!NetworkManager.Instance.IsMaster())
					{
						NetworkManager.Instance.SetWaitingSyncro("prefinishcast" + _cardActive.Id, status: true);
						NetworkManager.Instance.SetWaitingSyncro("finishcast" + _cardActive.Id, status: true);
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("**************************", "net");
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("WaitingSyncro prefinishcast" + _cardActive.Id, "net");
					}
					if (castingCardListMP.Count <= 1 && !IsBeginTournPhase)
					{
						string lastId = GenerateSyncCodeForCheckingAction();
						int iteration = 0;
						int _indexQueue = 20;
						int cardSpecialValue2 = 0;
						while (iteration < _indexQueue)
						{
							while (waitingKill)
							{
								yield return Globals.Instance.WaitForSeconds(0.01f);
							}
							string text3 = GenerateSyncCodeForCheckingAction();
							if (lastId == text3)
							{
								cardSpecialValue2 = 0;
								iteration++;
							}
							else
							{
								lastId = text3;
								iteration = 0;
								cardSpecialValue2++;
							}
							if (cardSpecialValue2 > 200)
							{
								break;
							}
							yield return Globals.Instance.WaitForSeconds(0.01f);
						}
					}
					if (NetworkManager.Instance.IsMaster())
					{
						if (coroutineSyncPreFinishCast != null)
						{
							StopCoroutine(coroutineSyncPreFinishCast);
						}
						coroutineSyncPreFinishCast = StartCoroutine(ReloadCombatCo("prefinishcast" + _cardActive.Id));
						while (!NetworkManager.Instance.AllPlayersReady("prefinishcast" + _cardActive.Id))
						{
							yield return Globals.Instance.WaitForSeconds(0.01f);
						}
						if (coroutineSyncPreFinishCast != null)
						{
							StopCoroutine(coroutineSyncPreFinishCast);
						}
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("Game ready, Everybody checked prefinishcast" + _cardActive.Id, "net");
						}
						NetworkManager.Instance.PlayersNetworkContinue("prefinishcast" + _cardActive.Id);
						yield return Globals.Instance.WaitForSeconds(0.01f);
					}
					else
					{
						NetworkManager.Instance.SetStatusReady("prefinishcast" + _cardActive.Id);
						while (NetworkManager.Instance.WaitingSyncro["prefinishcast" + _cardActive.Id])
						{
							yield return Globals.Instance.WaitForSeconds(0.01f);
						}
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("prefinishcast, we can continue!", "net");
						}
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("**************************", "net");
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("WaitingSyncro finishcast" + _cardActive.Id, "net");
					}
					if (NetworkManager.Instance.IsMaster())
					{
						SetRandomIndex(randomIndex);
						string text4 = GenerateSyncCode();
						photonView.RPC("NET_FinishCastCodeSyncFromMaster", RpcTarget.Others, randomIndex, Functions.CompressString(text4), _cardActive.Id);
						if (coroutineSyncFixSyncCode != null)
						{
							StopCoroutine(coroutineSyncFixSyncCode);
						}
						coroutineSyncFixSyncCode = StartCoroutine(ReloadCombatCo("FixingSyncCode finishcast" + _cardActive.Id));
						while (!NetworkManager.Instance.AllPlayersReady("finishcast" + _cardActive.Id))
						{
							yield return Globals.Instance.WaitForSeconds(0.01f);
						}
						if (coroutineSyncFixSyncCode != null)
						{
							StopCoroutine(coroutineSyncFixSyncCode);
						}
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("Game ready, Everybody checked finishcast" + _cardActive.Id, "net");
						}
						NetworkManager.Instance.PlayersNetworkContinue("finishcast" + _cardActive.Id);
					}
					else
					{
						while (NetworkManager.Instance.WaitingSyncro["finishcast" + _cardActive.Id])
						{
							yield return Globals.Instance.WaitForSeconds(0.01f);
						}
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("finishcast" + _cardActive.Id + ", we can continue!", "net");
						}
					}
				}
				if (castingCardListMP.Contains(_cardActive.Id))
				{
					castingCardListMP.Remove(_cardActive.Id);
				}
			}
			SetGameBusy(state: false);
			gameStatus = "FinishingCardAction";
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD(gameStatus, "gamestatus");
			}
			if (castingCardBlocked.ContainsKey(_cardActive.InternalId))
			{
				castingCardBlocked.Remove(_cardActive.InternalId);
			}
			CreateLogCastCard(_status: false, _cardActive, _uniqueCastId, theHero, theNPC, _hero, _npc);
			if (theNPC != null || _automatic)
			{
				yield return Globals.Instance.WaitForSeconds(0.01f);
				SetEventDirect("CastCardEvent" + _cardActive.Id, automatic: false);
				StartCoroutine(CastCardEndAutomatic(_cardActive, _uniqueCastId, _hero, _npc));
			}
			else
			{
				if (theCardItem != null)
				{
					theCardItem.DiscardCard(discardedFromHand: false);
				}
				if (_cardActive.EndTurn)
				{
					yield return Globals.Instance.WaitForSeconds(0.5f);
				}
				SetEventDirect("CastCardEvent" + _cardActive.Id, automatic: false);
				StartCoroutine(CastCardEnd(_cardActive, _uniqueCastId, _hero, _npc));
			}
			Instance.RefreshStatusEffects();
			yield return null;
		}
		else if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[cardIteration] EXIT HERE BECAUSE CARDITERATION DOES NOT CONTAIN " + _cardActive.InternalId, "trace");
		}
	}

	private CardData GetCardForModify(CardData cardData, Character caster, Character target)
	{
		CardIdProvider addCardForModify = cardData.AddCardForModify;
		CardData result = null;
		if (addCardForModify != null)
		{
			string cardID = addCardForModify.GetCardID(cardData, caster, target);
			if (!string.IsNullOrEmpty(cardID))
			{
				result = Globals.Instance.GetCardData(cardID, instantiate: false);
			}
		}
		return result;
	}

	public int IncreaseDamage(Character targetCharacter, Character casterCharacter, int dmgTotal)
	{
		CardData cardData = cardActive;
		if ((object)cardData != null && cardData.SpecialCardEnum == SpecialCardEnum.MindSpike)
		{
			dmgTotal += (int)cardActive.SpecialValueModifierGlobal * mindSpikeAbility.CurrentSpecialCardsUsedInMatch;
		}
		float damageMultiplier = GetDamageMultiplier(casterCharacter, targetCharacter);
		return (int)((float)dmgTotal * damageMultiplier);
	}

	private float GetDamageMultiplier(Character caster, Character target)
	{
		float result = 1f;
		if (target == null || !target.Alive)
		{
			return result;
		}
		bool flag = caster is Hero && caster.HasTrait(TraitEnum.RevealingPresence);
		if (flag && IsIllusion(target))
		{
			result = (IsExposedIllusion(target) ? 2f : 1.5f);
		}
		else if (flag && target.IsSummon)
		{
			result = 1.5f;
		}
		else if (IsExposedIllusion(target))
		{
			result = 1.5f;
		}
		return result;
	}

	private bool IsExposedIllusion(Character character)
	{
		if (character != null && character.Alive && character.IsIllusion)
		{
			return character.IsIllusionExposed;
		}
		return false;
	}

	private bool IsIllusion(Character character)
	{
		if (character != null && character.Alive)
		{
			return character.IsIllusion;
		}
		return false;
	}

	private IEnumerator ApplyNightmareImageCo(CardData data, Character character)
	{
		int originalCharacterPositionInTeam = GetCharacterPositionInTeam(character);
		int illusionCharacterPositionInTeam = GetNPCAvailablePosition();
		bool isNPC = character is NPC;
		CharacterItem originalCharacterItem = (isNPC ? ((CharacterItem)character.NPCItem) : ((CharacterItem)character.HeroItem));
		Vector3 originalCharacterPosition = originalCharacterItem.CalculateLocalPosition(originalCharacterPositionInTeam);
		Vector3 illusionCharacterPosition = originalCharacterItem.CalculateLocalPosition(illusionCharacterPositionInTeam);
		Vector3 targetPosition = (originalCharacterPosition + illusionCharacterPosition) / 2f;
		originalCharacterItem.MoveToPosition(originalCharacterItem.transform, targetPosition, returnBack: true, playSmokeEffect: false, 0.3f);
		yield return new WaitWhile(originalCharacterItem.CharIsMoving);
		bool swapPositions = Functions.Random(0, 2, GetRandomString()) == 0;
		if (swapPositions)
		{
			int num = illusionCharacterPositionInTeam;
			int num2 = originalCharacterPositionInTeam;
			originalCharacterPositionInTeam = num;
			illusionCharacterPositionInTeam = num2;
			if (isNPC)
			{
				TeamNPC[originalCharacterPositionInTeam] = TeamNPC[character.NPCIndex];
				NPCHand[originalCharacterPositionInTeam] = NPCHand[character.NPCIndex];
				NPCDeckDiscard[originalCharacterPositionInTeam] = NPCDeckDiscard[character.NPCIndex];
				NPCDeck[originalCharacterPositionInTeam] = NPCDeck[character.NPCIndex];
				character.SetNPCIndex(originalCharacterPositionInTeam);
			}
			else
			{
				TeamHero[originalCharacterPositionInTeam] = TeamHero[character.HeroIndex];
				HeroHand[originalCharacterPositionInTeam] = HeroHand[character.HeroIndex];
				HeroDeckDiscard[originalCharacterPositionInTeam] = HeroDeckDiscard[character.HeroIndex];
				HeroDeck[originalCharacterPositionInTeam] = HeroDeck[character.HeroIndex];
				character.SetNPCIndex(originalCharacterPositionInTeam);
			}
			character.Position = originalCharacterPositionInTeam;
		}
		Character character2 = CreateIllusion(character, data.EffectTarget, data, illusionCharacterPositionInTeam);
		CharacterItem illusionCharacterItem = (isNPC ? ((CharacterItem)character2.NPCItem) : ((CharacterItem)character2.HeroItem));
		illusionCharacterItem.transform.localPosition = new Vector3(originalCharacterItem.transform.localPosition.x, originalCharacterItem.transform.localPosition.y, originalCharacterItem.transform.localPosition.z);
		yield return Globals.Instance.WaitForSeconds(0.3f);
		originalCharacterItem.MoveToPosition(originalCharacterItem.transform, swapPositions ? illusionCharacterPosition : originalCharacterPosition, returnBack: true, playSmokeEffect: false, 0.3f);
		illusionCharacterItem.MoveToPosition(illusionCharacterItem.transform, swapPositions ? originalCharacterPosition : illusionCharacterPosition, returnBack: true, playSmokeEffect: false, 0.3f);
		yield return new WaitWhile(() => originalCharacterItem.CharIsMoving() || illusionCharacterItem.CharIsMoving());
		RepositionCharacters();
	}

	private bool CanApplyNightmareImage(CardData card, Character casterCharacter)
	{
		if (card.SpecialCardEnum != SpecialCardEnum.NightmareImage || casterCharacter == null || !casterCharacter.Alive)
		{
			return false;
		}
		int num = EnchantmentExecutedTimes(casterCharacter.Id, card.ItemEnchantment?.Id);
		ItemData itemEnchantment = card.ItemEnchantment;
		if ((object)itemEnchantment == null || itemEnchantment.DestroyAfterUses != num)
		{
			return false;
		}
		return true;
	}

	private void UpdateMasterReweaverProgress(Character casterCharacter, NPC npcWithAbility)
	{
		npcWithAbility.SetEvent(Enums.EventActivation.CastFightCard, casterCharacter);
	}

	private bool IsFightCard(CardData activeCard)
	{
		if (activeCard.Damage <= 0 && activeCard.Damage2 <= 0 && activeCard.DamageSelf <= 0)
		{
			return activeCard.DamageSelf2 > 0;
		}
		return true;
	}

	private void ApplyCorruptedEcho(CardData data, CardItem cardItem, List<NPC> npcsWithAbility)
	{
		int num = 0;
		int num2 = 0;
		foreach (NPC item in npcsWithAbility)
		{
			num = Globals.Instance.GetCardData("masterreweaver").ItemEnchantment.DestroyAfterUses;
			num2 = EnchantmentExecutedTimes(item.Id, "masterreweaver");
		}
		if (num != 0 && num2 < num)
		{
			data.TempAttackSelf = true;
			data.TargetPosition = Enums.CardTargetPosition.Anywhere;
			data.TargetSide = Enums.CardTargetSide.Self;
			data.TargetType = Enums.CardTargetType.Single;
			data.EffectCastCenter = false;
			data.SetTarget();
			if ((bool)cardItem)
			{
				cardItem.TargetTextTM = data.Target;
			}
		}
	}

	private void ReturnCardDataToOriginalState(CardData data, CardItem cardItem)
	{
		CardData cardData = Globals.Instance.GetCardData(data.Id, instantiate: false);
		data.TempAttackSelf = false;
		data.TargetPosition = cardData.TargetPosition;
		data.TargetSide = cardData.TargetSide;
		data.TargetType = cardData.TargetType;
		data.EffectCastCenter = cardData.EffectCastCenter;
		data.SetTarget();
		if ((bool)cardItem)
		{
			cardItem.TargetTextTM = data.Target;
		}
	}

	private int GetCharacterPositionInTeam(Character character)
	{
		int result = 0;
		Character[] array;
		if (!(character is NPC))
		{
			Character[] teamHero = TeamHero;
			array = teamHero;
		}
		else
		{
			Character[] teamHero = TeamNPC;
			array = teamHero;
		}
		Character[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			if (character == array2[i])
			{
				result = i;
			}
		}
		return result;
	}

	private bool TryGetTransferPainTarget(CardData cardActive, NPC currentNPC, out NPC targetNPC)
	{
		targetNPC = null;
		if (cardActive.SpecialCardEnum != SpecialCardEnum.TransferPain || currentNPC == null || !currentNPC.Alive)
		{
			return false;
		}
		NPC[] teamNPC = TeamNPC;
		foreach (NPC nPC in teamNPC)
		{
			if (nPC == null || !nPC.Alive || nPC.IsIllusion)
			{
				continue;
			}
			AICards[] aICards = currentNPC.NpcData.AICards;
			foreach (AICards aICards2 in aICards)
			{
				if (nPC.GameName.StartsWith(aICards2.SpecialSecondTargetID, StringComparison.OrdinalIgnoreCase) && nPC.GetHp() > currentNPC.GetHp())
				{
					targetNPC = nPC;
					return true;
				}
			}
		}
		return false;
	}

	private bool CanUpdateMindSpikeProgress(CardData data)
	{
		return data.SpecialCardEnum == mindSpikeAbility.CollectedSpecialCard;
	}

	private void ApplyTransferPain(NPC currentNPC, NPC targetNPC)
	{
		int hp = currentNPC.GetHp();
		int maxHP = currentNPC.GetMaxHP();
		int hp2 = targetNPC.GetHp();
		int maxHP2 = targetNPC.GetMaxHP();
		currentNPC.HpCurrent = Mathf.Min(hp2, maxHP);
		targetNPC.HpCurrent = Mathf.Min(hp, maxHP2);
		currentNPC.SetHP();
		targetNPC.SetHP();
	}

	private IEnumerator CastCardEnd(CardData _cardActive, string _uniqueCastId, Hero _hero, NPC _npc)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[CastCardEnd] END " + _cardActive.InternalId + " // " + _uniqueCastId, "general");
		}
		while (waitingDeathScreen)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[CastCardEnd] waitingDeathScreen", "trace");
			}
			yield return Globals.Instance.WaitForSeconds(0.1f);
		}
		if (theHero == null && theNPC != null)
		{
			SetGameBusy(state: false);
			gameStatus = "CastCardEnd";
			yield break;
		}
		if (theHero != null && theHero.HeroData != null)
		{
			theHero.HealAuraCurse(Globals.Instance.GetAuraCurseData("stealthbonus"));
			if (theHero.HeroData.HeroSubClass.Id == "queen" && theHero.HaveTrait("timeparadox") && _cardActive.Id.Contains("eldritchdischarge") && !Instance.ItemExecuteForThisCombat(theHero.HeroData.HeroSubClass.Id, "timeparadox", 1, ""))
			{
				string text = CreateCardInDictionary(_cardActive.Id);
				CardData cardData = GetCardData(text);
				cardData.EnergyReductionToZeroPermanent = true;
				ModifyCardInDictionary(text, cardData);
				GenerateNewCard(1, text, createCard: false, Enums.CardPlace.RandomDeck, null, null, theHero.HeroIndex);
				SetTraitInfoText();
				theHero.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Time Paradox") + Functions.TextChargesLeft(ItemExecutedInThisCombat(theHero.SubclassName, "timeparadox"), 1), Enums.CombatScrollEffectType.Trait);
			}
			if (theHero.HeroData.HeroSubClass.Id == "engineer" && _cardActive != null && _cardActive.GetCardTypes().Contains(Enums.CardType.Ranged_Attack))
			{
				bool flag = false;
				if (theHero.HaveTrait("doublebarrel"))
				{
					theHero.SetAura(theHero, Globals.Instance.GetAuraCurseData("fury"), 2);
					theHero.SetAura(theHero, Globals.Instance.GetAuraCurseData("fast"), 1);
					if (ItemExecuteForThisTurn(theHero.HeroData.HeroSubClass.Id, "loadedgun", 1, ""))
					{
						flag = true;
					}
					theHero.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_Double Barrel"), Enums.CombatScrollEffectType.Trait);
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					theHero.SetAura(null, Globals.Instance.GetAuraCurseData("reloading"), 1);
				}
			}
		}
		if (theHero == null || _cardActive.CardType == Enums.CardType.Corruption)
		{
			yield break;
		}
		keyClickedCard = false;
		combatTarget.Refresh();
		WriteNewCardsCounter();
		RedrawCardsDescriptionPrecalculated();
		DrawDeckPileLayer("Cards");
		SetGlobalOutlines(state: false);
		SetGameBusy(state: false);
		gameStatus = "PreCastCardEnd";
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD(gameStatus, "gamestatus");
		}
		yield return Globals.Instance.WaitForSeconds(0.01f);
		if (castingCardBlocked.ContainsKey(_cardActive.InternalId))
		{
			castingCardBlocked.Remove(_cardActive.InternalId);
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[CastCardEnd] **** CASTINGCARDBLOCKED REMOVE " + _cardActive.InternalId);
			}
		}
		if (_cardActive.EndTurn)
		{
			EndTurn();
			yield break;
		}
		if (CheckMatchIsOver())
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[CastCardEnd] FINISH CAST FINISH COMBAT");
			}
			FinishCombat();
			yield break;
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[CastCardEnd] FINISH CAST " + _cardActive.Id + " (" + cardsWaitingForReset + ")");
		}
		int indexExhaust = 0;
		if (!_cardActive.AutoplayDraw)
		{
			while (cardsWaitingForReset > 0)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[CastCardEnd] CardsWaitingForReset " + cardsWaitingForReset, "trace");
				}
				yield return Globals.Instance.WaitForSeconds(0.1f);
				indexExhaust++;
				if (indexExhaust > 100)
				{
					SetCardsWaitingForReset(0);
				}
			}
		}
		if (TeamHero != null && heroActive > -1 && heroActive < TeamHero.Length && TeamHero[heroActive] != null)
		{
			TeamHero[heroActive].SetCastedCard(_cardActive);
			if (_cardActive != null && !_cardActive.AutoplayDraw && !_cardActive.AutoplayEndTurn)
			{
				gameStatus = "PreFinishCast";
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(gameStatus, "gamestatus");
				}
				if (!_cardActive.IsPetAttack && !_cardActive.IsPetCast && (TeamHero[heroActive].HaveItemToActivate(Enums.EventActivation.PreFinishCast, _checkForItems: false, _checkForCorruption: true) || TeamHero[heroActive].HaveTraitToActivate(Enums.EventActivation.PreFinishCast)))
				{
					TeamHero[heroActive].SetEvent(Enums.EventActivation.PreFinishCast);
					yield return Globals.Instance.WaitForSeconds(0.01f);
					while (generatedCardTimes > 0)
					{
						yield return Globals.Instance.WaitForSeconds(0.7f);
					}
					while (waitingKill)
					{
						yield return Globals.Instance.WaitForSeconds(0.01f);
					}
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				ResetAutoEndCount();
				if (TeamHero[heroActive].HaveItemToActivate(Enums.EventActivation.FinishCast, _checkForItems: false, _checkForCorruption: true) || TeamHero[heroActive].HaveTraitToActivate(Enums.EventActivation.FinishCast))
				{
					Mathf.Clamp(energyAssigned, 0, int.MaxValue);
					TeamHero[heroActive].SetEvent(Enums.EventActivation.FinishCast, null, energyJustWastedByHero, cardActive.Id);
					yield return Globals.Instance.WaitForSeconds(0.01f);
					while (waitingKill)
					{
						yield return Globals.Instance.WaitForSeconds(0.01f);
					}
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				ResetAutoEndCount();
				if (!_cardActive.IsPetAttack && !_cardActive.IsPetCast)
				{
					if (heroActive > -1 && heroActive < TeamHero.Length && (TeamHero[heroActive].HaveItemToActivate(Enums.EventActivation.FinishFinishCast, _checkForItems: false, _checkForCorruption: true) || TeamHero[heroActive].HaveTraitToActivate(Enums.EventActivation.FinishFinishCast)))
					{
						TeamHero[heroActive].SetEvent(Enums.EventActivation.FinishFinishCast);
						yield return Globals.Instance.WaitForSeconds(0.01f);
						while (generatedCardTimes > 0)
						{
							yield return Globals.Instance.WaitForSeconds(0.7f);
						}
						while (waitingKill)
						{
							yield return Globals.Instance.WaitForSeconds(0.01f);
						}
						yield return Globals.Instance.WaitForSeconds(0.01f);
					}
					if (heroActive > -1 && heroActive < TeamHero.Length && (TeamHero[heroActive].HaveItemToActivate(Enums.EventActivation.FinishFinishCastCorruption, _checkForItems: false, _checkForCorruption: true) || TeamHero[heroActive].HaveTraitToActivate(Enums.EventActivation.FinishFinishCastCorruption)))
					{
						TeamHero[heroActive].SetEvent(Enums.EventActivation.FinishFinishCastCorruption);
						yield return Globals.Instance.WaitForSeconds(0.01f);
						while (generatedCardTimes > 0)
						{
							yield return Globals.Instance.WaitForSeconds(0.7f);
						}
						while (waitingKill)
						{
							yield return Globals.Instance.WaitForSeconds(0.01f);
						}
						yield return Globals.Instance.WaitForSeconds(0.01f);
					}
				}
			}
			ResetAutoEndCount();
		}
		while (eventList.Count > 0 || generatedCardTimes > 0)
		{
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
		ResetAutoEndCount();
		gameStatus = "CastCardEnd";
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD(gameStatus, "gamestatus");
		}
		CreateLogEntry(_initial: true, "finishCast:" + logDictionary.Count, "", null, null, null, null, currentRound);
	}

	private IEnumerator CastCardEndAutomatic(CardData _cardActive, string _uniqueCastId, Hero _hero, NPC _npc)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[CastCardEndAutomatic] " + _cardActive.Id + " // " + _uniqueCastId, "trace");
		}
		if (theNPC != null && theNPC.Alive && theNPC.NPCItem != null && npcActive > -1)
		{
			theNPC.HealAuraCurse(Globals.Instance.GetAuraCurseData("stealthbonus"));
			GameManager.Instance.PlayLibraryAudio("castnpccard");
			theNPC.CastCardNPCEnd();
			SetGameBusy(state: false);
			gameStatus = "CastCardEnd";
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD(gameStatus, "gamestatus");
			}
			waitingItemTrait = true;
			if (TeamNPC != null && npcActive < TeamNPC.Length)
			{
				TeamNPC[npcActive].SetEvent(Enums.EventActivation.FinishCast);
			}
			while (waitingItemTrait)
			{
				waitingItemTrait = false;
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[CastCardEndAutomatic] NPC waitingItemTrait", "trace");
				}
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
			if (_cardActive.EndTurn)
			{
				EndTurn();
				yield break;
			}
			if (CheckMatchIsOver())
			{
				FinishCombat();
				yield break;
			}
			if (theHeroPreAutomatic != null)
			{
				theHero = theHeroPreAutomatic;
			}
			if (theNPCPreAutomatic != null)
			{
				theNPC = theNPCPreAutomatic;
			}
			if (isBeginTournPhase)
			{
				yield break;
			}
			if (theNPC.NPCIsBoss() && BossNpc != null && BossNpc is PhantomArmor)
			{
				PhantomArmor pa = BossNpc as PhantomArmor;
				while (!pa.IsSpecialEffectFinished())
				{
					yield return Globals.Instance.WaitForSeconds(0.1f);
				}
			}
			CastNPC();
		}
		else
		{
			SetGameBusy(state: false);
		}
	}

	public void ShowTraitInfo(bool state, bool clearText = false)
	{
		if (clearText)
		{
			traitInfoText.text = "";
		}
		if (state && heroActive == -1)
		{
			state = false;
		}
		if (traitInfo.gameObject.activeSelf != state)
		{
			traitInfo.gameObject.SetActive(state);
		}
	}

	public void SetTraitInfoText()
	{
		if (theHero == null)
		{
			return;
		}
		TraitData traitData = null;
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		if (theHero.Traits != null)
		{
			for (int i = 0; i < theHero.Traits.Length; i++)
			{
				if (theHero.Traits[i] == null)
				{
					continue;
				}
				traitData = Globals.Instance.GetTraitData(theHero.Traits[i]);
				if (!(traitData != null))
				{
					continue;
				}
				if (traitData.Id == "ragnarok" && !theHero.HaveTrait("spellsinger"))
				{
					traitData = Globals.Instance.GetTraitData("spellsinger");
				}
				else if (traitData.HideTimesPerTurnText)
				{
					continue;
				}
				int value = 0;
				if (traitData.TimesPerTurn > 0 || traitData.TimesPerRound > 0)
				{
					stringBuilder.Append(traitData.TraitName);
					stringBuilder.Append("  ");
					if (traitData.TimesPerTurn > 0)
					{
						value = traitData.TimesPerTurn;
						if (traitData.Id == "warriorduality" && theHero.HaveTrait("beaconoflight"))
						{
							value++;
						}
						else if (traitData.Id == "magicduality" && theHero.HaveTrait("unholyblight"))
						{
							value++;
						}
						else if (traitData.Id == "healerduality" && theHero.HaveTrait("philosophersstone"))
						{
							value++;
						}
						else if (traitData.Id == "scoutduality" && theHero.HaveTrait("valhalla"))
						{
							value++;
						}
						else if (traitData.Id == "spellsinger" && theHero.HaveTrait("spellsinger") && theHero.HaveTrait("ragnarok"))
						{
							value++;
						}
					}
					else
					{
						value = traitData.TimesPerRound;
					}
					if (activatedTraits.ContainsKey(traitData.Id))
					{
						stringBuilder.Append(activatedTraits[traitData.Id]);
					}
					else if (activatedTraitsRound.ContainsKey(traitData.Id))
					{
						stringBuilder.Append(activatedTraitsRound[traitData.Id]);
					}
					else
					{
						stringBuilder.Append("0");
					}
					stringBuilder.Append("/");
					stringBuilder.Append(value);
					stringBuilder.Append("<br>");
					flag = true;
				}
				else if (traitData.Id == "timeloop" || traitData.Id == "timeparadox")
				{
					stringBuilder.Append(traitData.TraitName);
					stringBuilder.Append("  ");
					if (traitData.Id == "timeloop")
					{
						value = ((!theHero.HaveTrait("TimeParadox")) ? 1 : 2);
					}
					else if (traitData.Id == "timeparadox")
					{
						value = 1;
					}
					StringBuilder stringBuilder2 = new StringBuilder();
					stringBuilder2.Append(theHero.SubclassName);
					stringBuilder2.Append("_");
					stringBuilder2.Append(traitData.Id);
					if (itemExecutedInCombat.ContainsKey(stringBuilder2.ToString()))
					{
						stringBuilder.Append(itemExecutedInCombat[stringBuilder2.ToString()]);
					}
					else
					{
						stringBuilder.Append("0");
					}
					stringBuilder.Append("/");
					stringBuilder.Append(value);
					stringBuilder.Append("<br>");
					flag = true;
				}
			}
		}
		if (flag)
		{
			StringBuilder stringBuilder3 = new StringBuilder();
			stringBuilder3.Append("<size=-.2><size=-.4><sprite name=experience></size><color=#F0A169FF>");
			stringBuilder3.Append(Texts.Instance.GetText("activatedTraits"));
			stringBuilder3.Append("</color></size><br>");
			stringBuilder.Insert(0, stringBuilder3.ToString());
			traitInfoText.text = stringBuilder.ToString();
		}
	}

	private void AddCardModificationsForCard(CardData _cardActive, CardData _cardTarget, CopyConfig copyConfig = null)
	{
		if (_cardTarget != null && _cardActive != null && Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("AddCardModification to " + _cardTarget.Id + " from " + _cardActive.Id, "general");
		}
		if (_cardActive.AddCardReducedCost != 0)
		{
			if (_cardActive.AddCardReducedCost == -1)
			{
				if (!_cardActive.AddCardCostTurn)
				{
					_cardTarget.EnergyReductionToZeroPermanent = true;
				}
				else
				{
					_cardTarget.EnergyReductionToZeroTemporal = true;
				}
			}
			else if (!_cardActive.AddCardCostTurn)
			{
				_cardTarget.EnergyReductionPermanent += _cardActive.AddCardReducedCost;
			}
			else
			{
				_cardTarget.EnergyReductionTemporal += _cardActive.AddCardReducedCost;
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("EnergyCost " + _cardTarget.EnergyCost, "general");
			}
		}
		if (_cardActive.AddCardVanish)
		{
			_cardTarget.Vanish = true;
		}
		if (copyConfig != null)
		{
			CopyCardDataFromAnotherCard(_cardActive, _cardTarget, copyConfig);
		}
	}

	public void CopyCardDataFromAnotherCard(CardData _cardActive, CardData _cardTarget, CopyConfig copyConfig)
	{
		if (!(_cardTarget == null) && !(_cardActive == null) && copyConfig != null)
		{
			if (copyConfig.CopyNameFromOriginal)
			{
				_cardTarget.CardName = _cardActive.CardName;
			}
			if (copyConfig.CopyCardUpgradedFromOriginal)
			{
				_cardTarget.UpgradedFrom = _cardActive.UpgradedFrom;
				_cardTarget.UpgradesTo1 = _cardActive.UpgradesTo1;
				_cardTarget.UpgradesTo2 = _cardActive.UpgradesTo2;
				_cardTarget.UpgradesToRare = _cardActive.UpgradesToRare;
				_cardTarget.CardUpgraded = _cardActive.CardUpgraded;
			}
			if (copyConfig.CopyImageFromOriginal)
			{
				_cardTarget.Sprite = _cardActive.Sprite;
			}
			if (copyConfig.CopyCardTypeFromOriginal)
			{
				_cardTarget.CardType = _cardActive.CardType;
			}
		}
	}

	private void AddCardModificationsForCardForShow(CardData _cardActive, CardData _cardTarget)
	{
		if (theHero != null)
		{
			_cardTarget.EnergyCostForShow = theHero.GetCardFinalCost(_cardTarget);
		}
		else if (theNPC != null)
		{
			_cardTarget.EnergyCostForShow = theNPC.GetCardFinalCost(_cardTarget);
		}
		if (_cardActive.AddCardReducedCost == 0)
		{
			return;
		}
		if (_cardActive.AddCardReducedCost == -1)
		{
			_cardTarget.EnergyCostForShow = 0;
			return;
		}
		_cardTarget.EnergyCostForShow -= _cardActive.AddCardReducedCost;
		if (_cardTarget.EnergyCostForShow < 0)
		{
			_cardTarget.EnergyCostForShow = 0;
		}
	}

	public void ItemTraitActivated(bool state = true)
	{
		waitingItemTrait = state;
	}

	public void SortCharacterSprites(bool toFront = false, int heroIndex = -1, int npcIndex = -1)
	{
		for (int i = 0; i < TeamHero.Length; i++)
		{
			if ((!toFront || heroIndex == i) && TeamHero[i] != null && !(TeamHero[i].HeroData == null) && TeamHero[i].Alive && TeamHero[i].HeroItem != null)
			{
				TeamHero[i].HeroItem.DrawOrderSprites(toFront, TeamHero[i].Position * 2);
			}
		}
		for (int j = 0; j < TeamNPC.Length; j++)
		{
			if ((!toFront || npcIndex == j) && TeamNPC[j] != null && !(TeamNPC[j].NpcData == null) && TeamNPC[j].Alive && TeamNPC[j].NPCItem != null)
			{
				TeamNPC[j].NPCItem.DrawOrderSprites(toFront, TeamNPC[j].Position * 2);
			}
		}
	}

	public Hero[] GetTeamHero()
	{
		return TeamHero;
	}

	public NPC[] GetTeamNPC()
	{
		return TeamNPC;
	}

	public NPC GetNPCCharacter(int _index)
	{
		if (_index >= 0 || _index <= 3)
		{
			return TeamNPC[_index];
		}
		return null;
	}

	public NPC GetNPCById(string theId)
	{
		for (int i = 0; i < TeamNPC.Length; i++)
		{
			if (TeamNPC[i] != null && !(TeamNPC[i].NpcData == null) && TeamNPC[i].Id == theId)
			{
				return TeamNPC[i];
			}
		}
		return null;
	}

	public Hero GetHeroById(string theId)
	{
		for (int i = 0; i < TeamHero.Length; i++)
		{
			if (TeamHero[i] != null && TeamHero[i].Id == theId)
			{
				return TeamHero[i];
			}
		}
		return null;
	}

	public Character GetCharacterById(string theId)
	{
		Character heroById = GetHeroById(theId);
		if (heroById != null)
		{
			return heroById;
		}
		return GetNPCById(theId);
	}

	public bool PositionIsFront(bool isHero, int position)
	{
		if (isHero)
		{
			bool flag = false;
			List<Hero> list = new List<Hero>();
			for (int i = 0; i < TeamHero.Length; i++)
			{
				if (TeamHero[i] != null && !(TeamHero[i].HeroData == null))
				{
					Hero hero = TeamHero[i];
					if (hero.Alive && hero.HasEffect("taunt") && !hero.HasEffect("stealth"))
					{
						list.Add(hero);
						flag = true;
					}
				}
			}
			if (flag)
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].Alive)
					{
						if (list[j].Position == position)
						{
							return true;
						}
						return false;
					}
				}
			}
			else
			{
				for (int k = 0; k < TeamHero.Length; k++)
				{
					if (TeamHero[k] != null && !(TeamHero[k].HeroData == null) && TeamHero[k].Alive && !TeamHero[k].HasEffect("stealth"))
					{
						if (TeamHero[k].Position == position)
						{
							return true;
						}
						return false;
					}
				}
			}
		}
		else
		{
			bool flag2 = false;
			List<NPC> list2 = new List<NPC>();
			for (int l = 0; l < TeamNPC.Length; l++)
			{
				if (TeamNPC[l] != null && !(TeamNPC[l].NpcData == null))
				{
					NPC nPC = TeamNPC[l];
					if (nPC.Alive && nPC.HasEffect("taunt") && !nPC.HasEffect("stealth"))
					{
						list2.Add(nPC);
						flag2 = true;
					}
				}
			}
			if (flag2)
			{
				for (int m = 0; m < list2.Count; m++)
				{
					if (list2[m].Alive)
					{
						if (list2[m].Position == position)
						{
							return true;
						}
						return false;
					}
				}
			}
			else
			{
				for (int n = 0; n < TeamNPC.Length; n++)
				{
					if (TeamNPC[n] != null && !(TeamNPC[n].NpcData == null) && TeamNPC[n].Alive && !TeamNPC[n].HasEffect("stealth"))
					{
						if (TeamNPC[n].Position == position)
						{
							return true;
						}
						return false;
					}
				}
			}
		}
		return false;
	}

	public bool PositionIsBack(Character character)
	{
		if (character == null || !character.Alive)
		{
			return false;
		}
		if (character.HasEffect("stealth"))
		{
			return false;
		}
		int position = character.Position;
		if (character.IsHero)
		{
			bool flag = false;
			List<Hero> list = new List<Hero>();
			for (int i = 0; i < TeamHero.Length; i++)
			{
				if (TeamHero[i] != null && !(TeamHero[i].HeroData == null))
				{
					Hero hero = TeamHero[i];
					if (hero.Alive && hero.HasEffect("taunt"))
					{
						list.Add(hero);
						flag = true;
					}
				}
			}
			if (flag)
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].Alive && list[j].Position > position && !list[j].HasEffect("stealth"))
					{
						return false;
					}
				}
			}
			else
			{
				for (int k = 0; k < TeamHero.Length; k++)
				{
					if (TeamHero[k] != null && !(TeamHero[k].HeroData == null) && TeamHero[k].Alive && TeamHero[k].Position > position && !TeamHero[k].HasEffect("stealth"))
					{
						return false;
					}
				}
			}
		}
		else
		{
			bool flag2 = false;
			List<NPC> list2 = new List<NPC>();
			for (int l = 0; l < TeamNPC.Length; l++)
			{
				if (TeamNPC[l] != null && !(TeamNPC[l].NpcData == null))
				{
					NPC nPC = TeamNPC[l];
					if (nPC.Alive && nPC.HasEffect("taunt"))
					{
						list2.Add(nPC);
						flag2 = true;
					}
				}
			}
			if (flag2)
			{
				for (int m = 0; m < list2.Count; m++)
				{
					if (list2[m].Alive && list2[m].Position > position && !list2[m].HasEffect("stealth"))
					{
						return false;
					}
				}
			}
			else
			{
				for (int n = 0; n < TeamNPC.Length; n++)
				{
					if (TeamNPC[n] != null && !(TeamNPC[n].NpcData == null) && TeamNPC[n].Alive && TeamNPC[n].Position > position && !TeamNPC[n].HasEffect("stealth"))
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public bool PositionIsMiddle(Character character)
	{
		if (character == null || !character.Alive)
		{
			return false;
		}
		bool flag = false;
		bool flag2 = false;
		if (character.IsHero)
		{
			flag = PositionIsFront(isHero: true, character.Position);
			flag2 = PositionIsBack(character);
			if (NumHeroesAlive() > 2 && (flag || flag2))
			{
				bool flag3 = true;
				for (int i = 0; i < 4; i++)
				{
					if (TeamHero[i] != null && TeamHero[i].HeroData != null && TeamHero[i].Alive && character.Id != TeamHero[i].Id && !PositionIsFront(isHero: true, TeamHero[i].Position) && !PositionIsBack(TeamHero[i]) && !TeamHero[i].HasEffect("Stealth"))
					{
						flag3 = false;
						break;
					}
				}
				if (flag3)
				{
					return true;
				}
				return false;
			}
			return true;
		}
		flag = PositionIsFront(isHero: false, character.Position);
		flag2 = PositionIsBack(character);
		if (NumNPCsAlive() > 2 && (flag || flag2))
		{
			return false;
		}
		return true;
	}

	public void SetGlobalOutlines(bool state, CardData cardOver = null)
	{
		for (int i = 0; i < TeamHero.Length; i++)
		{
			if (TeamHero[i] != null && !(TeamHero[i].HeroData == null) && TeamHero[i].Alive)
			{
				HeroItem heroItem = TeamHero[i].HeroItem;
				if (heroItem == null)
				{
					return;
				}
				if (!state)
				{
					heroItem.OutlineHide();
				}
				else if (CheckTarget(heroItem.transform, cardOver))
				{
					heroItem.OutlineGreen();
				}
				else
				{
					heroItem.OutlineRed();
				}
			}
		}
		for (int j = 0; j < TeamNPC.Length; j++)
		{
			if (TeamNPC[j] == null || TeamNPC[j].NpcData == null || !TeamNPC[j].Alive)
			{
				continue;
			}
			NPCItem nPCItem = TeamNPC[j].NPCItem;
			if (nPCItem != null)
			{
				if (!state)
				{
					nPCItem.OutlineHide();
				}
				else if (CheckTarget(nPCItem.transform, cardOver))
				{
					nPCItem.OutlineGreen();
				}
				else
				{
					nPCItem.OutlineRed();
				}
			}
		}
	}

	public int CountHeroHand(int hero = -1)
	{
		if (HeroHand == null)
		{
			return 0;
		}
		if (hero == -1)
		{
			hero = heroActive;
		}
		if (hero == -1)
		{
			return 0;
		}
		return HeroHand[hero].Count;
	}

	public int CountNPCHand(int _npc = -1)
	{
		if (NPCHand == null)
		{
			return 0;
		}
		if (_npc == -1)
		{
			_npc = npcActive;
		}
		if (_npc == -1)
		{
			return 0;
		}
		if (NPCHand[_npc] == null)
		{
			NPCHand[_npc] = new List<string>();
		}
		return NPCHand[_npc].Count;
	}

	public int GetHeroHandEnergyTotalValue(int _index)
	{
		int num = 0;
		if (_index == heroActive)
		{
			for (int i = 0; i < HeroHand[_index].Count; i++)
			{
				num += GetCardFromTableByIndex(HeroHand[_index][i]).GetEnergyCost();
			}
		}
		return num;
	}

	public List<string> GetHeroDeck(int _index)
	{
		return HeroDeck[_index];
	}

	public List<string> GetHeroDiscard(int _index)
	{
		return HeroDeckDiscard[_index];
	}

	public List<string> GetHeroHand(int _index)
	{
		return HeroHand[_index];
	}

	public List<string> GetHeroVanish(int _index)
	{
		return HeroDeckVanish[_index];
	}

	public int GetCardIndexInTableById(string internalId)
	{
		for (int i = 0; i < cardItemTable.Count; i++)
		{
			if (cardItemTable[i].InternalId == internalId)
			{
				return i;
			}
		}
		return -1;
	}

	public CardItem GetCardFromTableByIndex(string internalId)
	{
		for (int i = 0; i < cardItemTable.Count; i++)
		{
			if (cardItemTable[i].InternalId == internalId)
			{
				return cardItemTable[i];
			}
		}
		return null;
	}

	public void UpdateHandCards()
	{
		for (int i = 0; i < cardItemTable.Count; i++)
		{
			cardItemTable[i].DrawEnergyCost();
		}
	}

	public int CountHeroDeck(int hero = -1)
	{
		if (HeroDeck == null)
		{
			return 0;
		}
		if (hero == -1)
		{
			hero = heroActive;
		}
		if (hero == -1)
		{
			return 0;
		}
		return HeroDeck[hero].Count;
	}

	public int CountNPCActiveDeck()
	{
		return CountNPCDeck(npcActive);
	}

	public int CountNPCDeck(int npc = -1)
	{
		if (NPCDeck == null)
		{
			return 0;
		}
		if (npc == -1)
		{
			npc = npcActive;
		}
		if (npc == -1)
		{
			return 0;
		}
		if (NPCDeck[npc] != null)
		{
			return NPCDeck[npc].Count;
		}
		return 0;
	}

	public int CountHeroDiscard(int hero = -1)
	{
		if (hero == -1)
		{
			hero = heroActive;
		}
		if (hero > -1 && HeroDeckDiscard[hero] != null)
		{
			return HeroDeckDiscard[hero].Count;
		}
		return 0;
	}

	public int CountHeroVanish(int hero = -1)
	{
		if (hero == -1)
		{
			hero = heroActive;
		}
		if (hero > -1 && HeroDeckVanish[hero] != null)
		{
			return HeroDeckVanish[hero].Count;
		}
		return 0;
	}

	public int CountNPCDiscard(int npc = -1)
	{
		if (npc == -1)
		{
			npc = npcActive;
		}
		if (npc > -1 && NPCDeckDiscard[npc] != null)
		{
			return NPCDeckDiscard[npc].Count;
		}
		return 0;
	}

	private void RemoveCardFromDiscardPile(string id)
	{
		GameObject gameObject = GameObject.Find("/World/Decks/DiscardPile/" + id);
		if (gameObject != null)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	public void DeckParticlesShow(int type, bool state)
	{
		if (type == 0)
		{
			_ = deckPileParticle.main;
		}
		else
		{
			_ = discardPileParticle.main;
		}
		if (state)
		{
			if (type == 0 && CountHeroDeck() > 0)
			{
				deckPileParticle.Play();
			}
			else if (type == 1 && CountHeroDiscard() > 0)
			{
				discardPileParticle.Play();
			}
		}
		else if (type == 0 && CountHeroDeck() > 0)
		{
			deckPileParticle.Stop();
		}
		else if (type == 1 && CountHeroDiscard() > 0)
		{
			discardPileParticle.Stop();
		}
	}

	public CardData GetCardData(string id, bool createInDict = true)
	{
		if (id != "")
		{
			id = id.ToLower();
			if (createInDict)
			{
				if (cardDictionary == null)
				{
					cardDictionary = new Dictionary<string, CardData>();
				}
				if (!cardDictionary.ContainsKey(id))
				{
					CreateCardInDictionary(id, "", _overwriteId: true);
				}
			}
			if (cardDictionary != null && cardDictionary.ContainsKey(id))
			{
				return cardDictionary[id];
			}
		}
		return null;
	}

	public void SelectedCardPosition(int cardPosition, bool state)
	{
		if (cardPosition > -1)
		{
			int num = CountHeroHand();
			for (int i = 0; i < num; i++)
			{
				cardItemTable[i].AdjustPositionBecauseHover(cardPosition, state, num);
			}
		}
	}

	public void SetCardHover(int cardPosition, bool state)
	{
		if (gameStatus != "" && gameStatus != "CastCardEnd")
		{
			return;
		}
		if (state)
		{
			CardData cData = cardItemTable[cardPosition].CardData;
			CardItem cardItem = cardItemTable.FirstOrDefault((CardItem i) => i.CardData.Id == cData.Id);
			List<NPC> npcsWithAbility;
			bool flag = TryGetNPCsWithMasterReweaverAbility(cData, TeamHero[heroActive], out npcsWithAbility);
			if (!cData.TempAttackSelf && flag)
			{
				ApplyCorruptedEcho(cData, cardItem, npcsWithAbility);
			}
			else if (cData.TempAttackSelf && !flag)
			{
				ReturnCardDataToOriginalState(cData, cardItem);
			}
			cardActive = cData;
			SetDamagePreview(theCasterIsHero: true, cData, cardPosition);
			SetOverDeck(state: true);
			if (cardHoverIndex != cardPosition)
			{
				if (cardHoverIndex > -1)
				{
					SelectedCardPosition(cardHoverIndex, state: false);
				}
				SelectedCardPosition(cardPosition, state: true);
				cardHoverIndex = cardPosition;
			}
		}
		else
		{
			SelectedCardPosition(cardPosition, state: false);
			ResetCardHoverIndex();
		}
	}

	public void ResetCardHoverIndex()
	{
		cardHoverIndex = -1;
	}

	public void PreSelectCard()
	{
		deckCardsWindowPosY = deckCardsWindow.GetScrolled();
	}

	[PunRPC]
	private void NET_ShareArrDiscardCard(string _arrToDiscardCard, int _randomIndex)
	{
		SetRandomIndex(_randomIndex);
		StartCoroutine(NET_ShareArrDiscardCardCo(_arrToDiscardCard, _randomIndex));
	}

	private IEnumerator NET_ShareArrDiscardCardCo(string _arrToDiscardCard, int _randomIndex)
	{
		List<string> list = JsonHelper.FromJson<string>(_arrToDiscardCard).ToList();
		CICardDiscard.Clear();
		for (int i = 0; i < list.Count; i++)
		{
			CardItem item = new CardItem();
			string text = list[i];
			if (cardGos.ContainsKey(text))
			{
				item = cardGos[text].GetComponent<CardItem>();
			}
			else
			{
				foreach (Transform item2 in discardSelector.cardContainer)
				{
					if ((bool)item2.GetComponent<CardItem>() && item2.GetComponent<CardItem>().CardData.InternalId == text)
					{
						item = item2.GetComponent<CardItem>();
						break;
					}
				}
			}
			CICardDiscard.Add(item);
		}
		discardSelector.TurnOff();
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("**************************", "net");
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("WaitingSyncro NET_SADC_" + _randomIndex, "net");
		}
		yield return Globals.Instance.WaitForSeconds(0.1f);
		if (NetworkManager.Instance.IsMaster())
		{
			if (coroutineSyncDiscard != null)
			{
				StopCoroutine(coroutineSyncDiscard);
			}
			coroutineSyncDiscard = StartCoroutine(ReloadCombatCo("NET_SADC_" + _randomIndex));
			while (!NetworkManager.Instance.AllPlayersReady("NET_SADC_" + _randomIndex))
			{
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			if (coroutineSyncDiscard != null)
			{
				StopCoroutine(coroutineSyncDiscard);
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("Game ready, Everybody checked NET_SADC_" + _randomIndex, "net");
			}
			NetworkManager.Instance.PlayersNetworkContinue("NET_SADC_" + _randomIndex);
			yield return Globals.Instance.WaitForSeconds(0.2f);
		}
		else
		{
			NetworkManager.Instance.SetWaitingSyncro("NET_SADC_" + _randomIndex, status: true);
			NetworkManager.Instance.SetStatusReady("NET_SADC_" + _randomIndex);
			while (NetworkManager.Instance.WaitingSyncro["NET_SADC_" + _randomIndex])
			{
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("NET_SADC_" + _randomIndex + ", we can continue!", "net");
			}
		}
		waitingForDiscardAssignment = false;
	}

	public void AssignDiscardAction()
	{
		if (!GameManager.Instance.IsMultiplayer())
		{
			waitingForDiscardAssignment = false;
		}
		else
		{
			List<string> list = new List<string>();
			for (int i = 0; i < CICardDiscard.Count; i++)
			{
				if (CICardDiscard[i] != null && CICardDiscard[i].CardData != null)
				{
					list.Add(CICardDiscard[i].CardData.InternalId);
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD(CICardDiscard[i].CardData.InternalId);
					}
				}
			}
			string text = JsonHelper.ToJson(list.ToArray());
			photonView.RPC("NET_ShareArrDiscardCard", RpcTarget.All, text, randomIndex);
		}
		EventSystem.current.SetSelectedGameObject(null);
	}

	public void SelectCardToDiscard(CardItem CI)
	{
		if (!(cardActive == null) && !(CI == null) && (waitingForDiscardAssignment || waitingForLookDiscardWindow))
		{
			bool iconSkull = false;
			if (cardActive.DiscardCardPlace == Enums.CardPlace.Discard || cardActive.DiscardCardPlace == Enums.CardPlace.Vanish)
			{
				iconSkull = true;
			}
			if (!CICardDiscard.Contains(CI))
			{
				CICardDiscard.Add(CI);
				CI.EnableDisableDiscardAction(state: true, iconSkull);
			}
			else
			{
				CICardDiscard.Remove(CI);
				CI.EnableDisableDiscardAction(state: false, iconSkull);
			}
			if (GameManager.Instance.IsMultiplayer() && IsYourTurnForAddDiscard())
			{
				photonView.RPC("NET_SelectCardToDiscard", RpcTarget.Others, CI.transform.gameObject.name);
			}
			if (CICardDiscard.Count > GlobalDiscardCardsNum)
			{
				CICardDiscard[0].EnableDisableDiscardAction(state: false, iconSkull);
				CICardDiscard.RemoveAt(0);
			}
			discardSelector.TextInstructions();
		}
	}

	[PunRPC]
	private void NET_SelectCardToDiscard(string goName)
	{
		CardItem cardItem = null;
		if (cardGos.ContainsKey(goName))
		{
			cardItem = cardGos[goName].GetComponent<CardItem>();
		}
		else
		{
			GameObject gameObject = GameObject.Find(goName);
			if (gameObject != null)
			{
				cardItem = gameObject.transform.GetComponent<CardItem>();
			}
		}
		if (cardItem != null)
		{
			SelectCardToDiscard(cardItem);
		}
	}

	[PunRPC]
	private void NET_ShareArrAddCard(string _arrToAddCard, int _randomIndex)
	{
		SetRandomIndex(_randomIndex);
		StartCoroutine(NET_ShareArrAddCardCo(_arrToAddCard, _randomIndex));
	}

	private IEnumerator NET_ShareArrAddCardCo(string _arrToAddCard, int _randomIndex)
	{
		List<string> list = JsonHelper.FromJson<string>(_arrToAddCard).ToList();
		CICardAddcard.Clear();
		for (int i = 0; i < list.Count; i++)
		{
			string key = list[i];
			CardItem item = ((!cardGos.ContainsKey(key)) ? GameObject.Find(key).transform.GetComponent<CardItem>() : cardGos[key].GetComponent<CardItem>());
			CICardAddcard.Add(item);
		}
		addcardSelector.TurnOff();
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("**************************", "net");
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("WaitingSyncro NET_SAAC_" + _randomIndex, "net");
		}
		yield return Globals.Instance.WaitForSeconds(0.1f);
		if (NetworkManager.Instance.IsMaster())
		{
			if (coroutineSyncAddcard != null)
			{
				StopCoroutine(coroutineSyncAddcard);
			}
			coroutineSyncAddcard = StartCoroutine(ReloadCombatCo("NET_SAAC_" + _randomIndex));
			while (!NetworkManager.Instance.AllPlayersReady("NET_SAAC_" + _randomIndex))
			{
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			if (coroutineSyncAddcard != null)
			{
				StopCoroutine(coroutineSyncAddcard);
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("Game ready, Everybody checked NET_SAAC_" + _randomIndex, "net");
			}
			NetworkManager.Instance.PlayersNetworkContinue("NET_SAAC_" + _randomIndex);
			yield return Globals.Instance.WaitForSeconds(0.2f);
		}
		else
		{
			NetworkManager.Instance.SetWaitingSyncro("NET_SAAC_" + _randomIndex, status: true);
			NetworkManager.Instance.SetStatusReady("NET_SAAC_" + _randomIndex);
			while (NetworkManager.Instance.WaitingSyncro["NET_SAAC_" + _randomIndex])
			{
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("NET_SAAC_" + _randomIndex + ", we can continue!", "net");
			}
		}
		waitingForAddcardAssignment = false;
	}

	public void AssignAddcardAction()
	{
		if (!GameManager.Instance.IsMultiplayer())
		{
			waitingForAddcardAssignment = false;
		}
		else
		{
			List<string> list = new List<string>();
			for (int i = 0; i < CICardAddcard.Count; i++)
			{
				if (CICardAddcard[i] != null && CICardAddcard[i].CardData != null)
				{
					list.Add(CICardAddcard[i].CardData.InternalId);
				}
			}
			string text = JsonHelper.ToJson(list.ToArray());
			photonView.RPC("NET_ShareArrAddCard", RpcTarget.All, text, randomIndex);
		}
		EventSystem.current.SetSelectedGameObject(null);
	}

	public void SelectCardToAddcard(CardItem CI)
	{
		if (!(CI == null) && waitingForAddcardAssignment)
		{
			if (!CICardAddcard.Contains(CI))
			{
				CICardAddcard.Add(CI);
				CI.EnableDisableAddcardAction(state: true);
			}
			else
			{
				CICardAddcard.Remove(CI);
				CI.EnableDisableAddcardAction(state: false);
			}
			if (GameManager.Instance.IsMultiplayer() && IsYourTurn())
			{
				photonView.RPC("NET_SelectCardToAddcard", RpcTarget.Others, CI.transform.gameObject.name);
			}
			if (CICardAddcard.Count > GlobalAddcardCardsNum)
			{
				CICardAddcard[0].EnableDisableAddcardAction(state: false);
				CICardAddcard.RemoveAt(0);
			}
			addcardSelector.TextInstructions();
		}
	}

	[PunRPC]
	private void NET_SelectCardToAddcard(string goName)
	{
		CardItem cI = ((!cardGos.ContainsKey(goName)) ? GameObject.Find(goName).transform.GetComponent<CardItem>() : cardGos[goName].GetComponent<CardItem>());
		SelectCardToAddcard(cI);
	}

	public int CardsLeftForDiscard()
	{
		return GlobalDiscardCardsNum - CICardDiscard.Count();
	}

	public int CardsLeftForAddcard()
	{
		return GlobalAddcardCardsNum - CICardAddcard.Count();
	}

	public void WarningMultiplayerIfNotActive()
	{
		if (GameManager.Instance.IsMultiplayer() && !IsYourTurn() && IsYourTurnForAddDiscard())
		{
			GameManager.Instance.PlayLibraryAudio("yourturn3", 0.5f);
		}
	}

	public string CreateCardInDictionary(string _id, string _auxString = "", bool _overwriteId = false)
	{
		string text = _id.Split('_')[0];
		CardData cardData = Globals.Instance.GetCardData(text);
		if (cardData == null)
		{
			return "";
		}
		if (_overwriteId && !cardDictionary.ContainsKey(_id))
		{
			Debug.Log("ADD CARD TO DICT -> " + _id);
			cardDictionary.Add(_id, cardData);
			cardDictionary[_id].InitClone(_id);
			return _id;
		}
		int num = 0;
		bool flag = false;
		string text2 = "";
		StringBuilder stringBuilder = new StringBuilder();
		while (!flag)
		{
			stringBuilder.Clear();
			stringBuilder.Append(text.ToLower());
			stringBuilder.Append("_");
			stringBuilder.Append(_auxString);
			stringBuilder.Append(num);
			text2 = stringBuilder.ToString();
			if (cardDictionary.ContainsKey(text2))
			{
				num++;
				continue;
			}
			cardDictionary.Add(text2, cardData);
			flag = true;
		}
		cardDictionary[text2].InitClone(text2);
		return text2;
	}

	public void ModifyCardInDictionary(string id, CardData cardData, string overrideId = "")
	{
		id = id.ToLower();
		if (!cardDictionary.ContainsKey(id))
		{
			return;
		}
		cardDictionary[id] = UnityEngine.Object.Instantiate(cardData);
		CardData cardData2 = cardDictionary[id];
		string internalId = (cardDictionary[id].Id = id);
		cardData2.InternalId = internalId;
		if (cardItemTable != null)
		{
			for (int i = 0; i < cardItemTable.Count; i++)
			{
				if (cardItemTable[i] != null && cardItemTable[i].InternalId == id)
				{
					cardItemTable[i].DrawEnergyCost();
				}
			}
		}
		if (!string.IsNullOrEmpty(overrideId))
		{
			cardDictionary[id].Id = overrideId;
			CardData cardData3 = cardDictionary[id];
			internalId = (cardDictionary[id].Id = overrideId);
			cardData3.InternalId = internalId;
			if (cardDictionary.TryGetValue(id, out var value))
			{
				cardDictionary[overrideId] = value;
				cardDictionary.Remove(id);
			}
		}
	}

	public CardData DuplicateCardData(CardData _cardDestine, CardData _cardSource)
	{
		if (_cardSource != null && _cardDestine != null)
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
		if (cardDictionary != null && cardDictionary.ContainsKey(id))
		{
			cardDictionary.Remove(id);
		}
	}

	private string CardNamesForSyncCode()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(CardDictionaryKeys());
		for (int i = 0; i < 4; i++)
		{
			if (HeroDeck[i] != null)
			{
				for (int j = 0; j < HeroDeck[i].Count; j++)
				{
					stringBuilder.Append(HeroDeck[i][j]);
				}
			}
			stringBuilder.Append("*0*");
			if (HeroDeckDiscard[i] != null)
			{
				for (int k = 0; k < HeroDeckDiscard[i].Count; k++)
				{
					stringBuilder.Append(HeroDeckDiscard[i][k]);
				}
			}
			stringBuilder.Append("*1*");
			if (HeroDeckVanish[i] != null)
			{
				for (int l = 0; l < HeroDeckVanish[i].Count; l++)
				{
					stringBuilder.Append(HeroDeckVanish[i][l]);
				}
			}
			stringBuilder.Append("*2*");
		}
		return stringBuilder.ToString();
	}

	private string CardDictionaryKeys()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<string, CardData> item in cardDictionary)
		{
			stringBuilder.Append(item.Key);
			stringBuilder.Append("@");
		}
		return stringBuilder.ToString();
	}

	private void BackupCardDictionary()
	{
		if (cardDictionaryBackup != null)
		{
			foreach (KeyValuePair<string, CardData> item in cardDictionaryBackup)
			{
				UnityEngine.Object.Destroy(item.Value);
			}
			cardDictionaryBackup.Clear();
		}
		else
		{
			cardDictionaryBackup = new Dictionary<string, CardData>();
		}
		foreach (KeyValuePair<string, CardData> item2 in cardDictionary)
		{
			cardDictionaryBackup.Add(item2.Key, UnityEngine.Object.Instantiate(item2.Value));
		}
		backingDictionary = false;
	}

	private void RestoreCardDictionary()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[RestoreCardDictionary] begin", "general");
		}
		if (cardDictionaryBackup != null && cardDictionaryBackup.Count > 0)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[RestoreCardDictionary] copied", "general");
			}
			cardDictionary = new Dictionary<string, CardData>();
			foreach (KeyValuePair<string, CardData> item in cardDictionaryBackup)
			{
				cardDictionary.Add(item.Key, UnityEngine.Object.Instantiate(item.Value));
			}
		}
		backingDictionary = false;
	}

	public void CardCreatorAction(int numCards, string theCard, bool createCard, Enums.CardPlace where, bool fromNet)
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			if (NetworkManager.Instance.IsMaster())
			{
				photonView.RPC("NET_CardCreatorAction", RpcTarget.Others, numCards, theCard, createCard, where, true);
			}
			else if (!fromNet)
			{
				return;
			}
		}
		GenerateNewCard(numCards, theCard, createCard, where);
	}

	[PunRPC]
	private void NET_CardCreatorAction(int numCards, string theCard, bool createCard, Enums.CardPlace where, bool fromNet)
	{
		CardCreatorAction(numCards, theCard, createCard, where, fromNet);
	}

	public void GenerateNewCard(int numCards, string theCard, bool createCard, Enums.CardPlace where, CardData cardDataForModification = null, CardData copyDataFromThisCard = null, int heroIndex = -1, bool isHero = true, int indexForBatch = 0, CopyConfig copyConfig = null)
	{
		if (MatchIsOver)
		{
			return;
		}
		if (where == Enums.CardPlace.Hand)
		{
			int num = 10 - CountHeroHand();
			if (num < numCards)
			{
				numCards = num;
			}
			if (numCards <= 0)
			{
				return;
			}
		}
		SetGameBusy(state: true);
		StartCoroutine(GenerateNewCardCo(numCards, theCard, createCard, where, cardDataForModification, copyDataFromThisCard, heroIndex, isHero, indexForBatch, copyConfig));
	}

	private IEnumerator GenerateNewCardCo(int numCards, string _theCard, bool createCard, Enums.CardPlace where, CardData cardDataForModification, CardData copyDataFromThisCard, int heroIndex, bool isHero, int indexForBatch, CopyConfig copyConfig = null)
	{
		List<string> theCards = new List<string>();
		for (int i = 0; i < numCards; i++)
		{
			string text = ((!createCard) ? _theCard : CreateCardInDictionary(_theCard));
			if (copyDataFromThisCard != null)
			{
				ModifyCardInDictionary(text, copyDataFromThisCard);
			}
			if (cardDataForModification != null)
			{
				CardData cardData = GetCardData(text);
				if (cardData.SpecialCardEnum == SpecialCardEnum.NightmareEchoTemplate)
				{
					string text2 = cardData.Id.Split('_')[0] + "-" + cardDataForModification.Id;
					cardData = GetCardData(text2);
					text = text2;
				}
				AddCardModificationsForCard(cardDataForModification, cardData, copyConfig);
			}
			theCards.Add(text);
		}
		if (heroActive == -1 && npcActive == -1)
		{
			yield return Globals.Instance.WaitForSeconds((float)indexForBatch * 0.1f);
		}
		else
		{
			yield return null;
		}
		generatedCardTimes++;
		if (theCards.Count > 0)
		{
			SetEventDirect("GenerateNewCard" + theCards[0], automatic: false, add: true);
			int _heroActive = heroIndex;
			if (_heroActive == -1)
			{
				_heroActive = heroActive;
			}
			List<CardItem> CIG = new List<CardItem>();
			int tempTransformChildCount = tempTransform.childCount;
			for (int j = 0; j < theCards.Count; j++)
			{
				string id = theCards[j];
				if (heroIndex > -1)
				{
					Hero hero = GetTeamHero()[heroIndex];
					if (hero.PetBonusDamageType != Enums.DamageType.None)
					{
						CardData cardData2 = Instance.GetCardData(id);
						if (ApplyHeroModsToPetCard(cardData2, hero))
						{
							cardData2.SetDescriptionNew(forceDescription: true);
						}
					}
				}
				tempTransformChildCount++;
				GameObject gameObject = ((theNPC == null) ? UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, new Vector3(-0.35f, 0.35f, 0f) + new Vector3((float)tempTransformChildCount * 0.25f, (float)tempTransformChildCount * -0.15f, 0f), Quaternion.identity, tempTransform) : UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, new Vector3(-0.35f, 0.35f, 0f) + new Vector3((float)tempTransformChildCount * 0.25f, (float)tempTransformChildCount * -0.15f, 0f), Quaternion.identity, tempTransform));
				gameObject.name = id;
				CardItem component = gameObject.GetComponent<CardItem>();
				if (theHero != null)
				{
					component.SetCard(id, deckScale: false, theHero);
				}
				else if (theNPC != null)
				{
					component.SetCard(id, deckScale: false, null, theNPC);
				}
				else
				{
					component.SetCard(id, deckScale: false);
				}
				component.TopLayeringOrder("Default", 20000 + 60 * tempTransformChildCount + 100 * indexForBatch);
				component.DisableCollider();
				CIG.Add(component);
				if (theNPC != null)
				{
					component.SetDestinationScaleRotation(new Vector3(-0.35f, 0.35f, 0f) + new Vector3((float)tempTransformChildCount * 0.25f, (float)tempTransformChildCount * -0.15f, 0f), 1.4f, Quaternion.Euler(0f, 0f, 0f));
				}
				else
				{
					component.SetDestinationScaleRotation(new Vector3(-0.35f, 0.35f, 0f) + new Vector3((float)tempTransformChildCount * 0.25f, (float)tempTransformChildCount * -0.15f, 0f), 1.4f, Quaternion.Euler(0f, 0f, 0f));
				}
				component.discard = true;
				if (where != Enums.CardPlace.Hand)
				{
					component.HideRarityParticles();
					component.HideCardIconParticles();
				}
				yield return Globals.Instance.WaitForSeconds(0.15f);
			}
			if (CIG != null && CIG.Count == 1 && CIG[0] != null && CIG[0].CardData.CardType == Enums.CardType.Corruption)
			{
				yield return Globals.Instance.WaitForSeconds(0.5f);
				GameManager.Instance.GenerateParticleTrail(2, CIG[0].transform.position, iconCorruption.transform.position);
				UnityEngine.Object.Destroy(CIG[0].gameObject);
				yield return Globals.Instance.WaitForSeconds(0.1f);
				iconCorruption.transform.gameObject.SetActive(value: true);
				generatedCardTimes--;
				if (generatedCardTimes < 0)
				{
					generatedCardTimes = 0;
				}
				SetEventDirect("GenerateNewCard" + theCards[0], automatic: false);
				yield break;
			}
			yield return Globals.Instance.WaitForSeconds(0.01f);
			for (int j = 0; j < CIG.Count; j++)
			{
				switch (where)
				{
				case Enums.CardPlace.Discard:
					HeroDeckDiscard[_heroActive].Add(CIG[j].InternalId);
					if (theNPC != null && isHero)
					{
						yield return Globals.Instance.WaitForSeconds(0.2f);
						if (CIG[j] != null)
						{
							CIG[j].destroyAtLocation = true;
							GameManager.Instance.GenerateParticleTrail(2, CIG[j].transform.position, TeamHero[_heroActive].HeroItem.transform.position + new Vector3(0f, 2f, 0f));
							UnityEngine.Object.Destroy(CIG[j].gameObject);
						}
					}
					else
					{
						CIG[j].DiscardCard(discardedFromHand: false);
					}
					DrawDiscardPileCardNumeral();
					CreateLogEntry(_initial: true, "toDiscard:" + logDictionary.Count, CIG[j].InternalId, TeamHero[_heroActive], null, null, null, currentRound);
					continue;
				case Enums.CardPlace.Hand:
					HeroHand[_heroActive].Add(CIG[j].name);
					CIG[j].transform.parent = GO_Hand.transform;
					CIG[j].active = true;
					CIG[j].discard = false;
					CIG[j].SetCard(CIG[j].name, deckScale: false, theHero);
					cardItemTable.Add(CIG[j]);
					RepositionCards();
					CreateLogEntry(_initial: true, "toHand:" + logDictionary.Count, CIG[j].InternalId, TeamHero[_heroActive], null, null, null, currentRound);
					continue;
				case Enums.CardPlace.Cast:
					yield return Globals.Instance.WaitForSeconds(0.15f);
					if (CIG[j] != null && IsThereAnyTargetForCard(CIG[j].CardData))
					{
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("Set in hand card ->" + CIG[j].name + " | " + heroIndex + " | " + theHero);
						}
						if (theHero != null)
						{
							if (CIG[j] != null && CIG[j].CardData.CardType != Enums.CardType.Corruption)
							{
								HeroHand[_heroActive].Add(CIG[j].name);
							}
							if (CIG[j] != null)
							{
								CIG[j].SetCard(CIG[j].name, deckScale: false, theHero);
							}
							if (CIG[j] != null && CIG[j].CardData.CardType != Enums.CardType.Corruption)
							{
								cardItemTable.Add(CIG[j]);
								RepositionCards();
							}
						}
						else if (CIG[j] != null)
						{
							CIG[j].SetCard(CIG[j].name, deckScale: false, null, theNPC);
						}
						if (CIG[j] != null)
						{
							CIG[j].DrawBorder("");
							CIG[j].SetDestinationScaleRotation(CIG[j].transform.localPosition, 1.4f, Quaternion.Euler(0f, 0f, 0f));
						}
						if (CIG[j] != null)
						{
							if (theHero != null)
							{
								Hero casterHeroParam = ((heroIndex > -1) ? GetTeamHero()[heroIndex] : null);
								StartCoroutine(CastCard(CIG[j], _automatic: false, null, -1, -1, _propagate: false, casterHeroParam));
							}
							else if (theNPC != null)
							{
								StartCoroutine(CastCard(CIG[j], _automatic: false, null, -1, -1, _propagate: false));
							}
						}
					}
					else
					{
						if (!(CIG[j] != null))
						{
							continue;
						}
						CIG[j].DiscardCard(discardedFromHand: false, Enums.CardPlace.Vanish);
						if (gameStatus == "BeginTurn")
						{
							gameStatus = "CastCardEnd";
							if (Globals.Instance.ShowDebug)
							{
								Functions.DebugLogGD(gameStatus, "gamestatus");
							}
						}
					}
					continue;
				}
				string internalId = CIG[j].InternalId;
				switch (where)
				{
				case Enums.CardPlace.TopDeck:
					if (isHero)
					{
						if (_heroActive > -1)
						{
							HeroDeck[_heroActive].Insert(0, internalId);
							CreateLogEntry(_initial: true, "toTopDeck:" + logDictionary.Count, internalId, TeamHero[_heroActive], null, null, null, currentRound);
						}
						else
						{
							HeroDeck[theHero.HeroIndex].Insert(0, internalId);
							CreateLogEntry(_initial: true, "toTopDeck:" + logDictionary.Count, internalId, TeamHero[theHero.HeroIndex], null, null, null, currentRound);
						}
					}
					else
					{
						NPCDeck[heroIndex].Insert(0, internalId);
						CreateLogEntry(_initial: true, "toTopDeck:" + logDictionary.Count, internalId, null, TeamNPC[heroIndex], null, null, currentRound);
					}
					break;
				case Enums.CardPlace.BottomDeck:
					if (_heroActive > -1)
					{
						HeroDeck[_heroActive].Insert(HeroDeck[_heroActive].Count, internalId);
						CreateLogEntry(_initial: true, "toBottomDeck:" + logDictionary.Count, internalId, TeamHero[_heroActive], null, null, null, currentRound);
					}
					else
					{
						HeroDeck[theHero.HeroIndex].Insert(HeroDeck[theHero.HeroIndex].Count, internalId);
						CreateLogEntry(_initial: true, "toBottomDeck:" + logDictionary.Count, internalId, TeamHero[theHero.HeroIndex], null, null, null, currentRound);
					}
					break;
				default:
					if (_heroActive > -1)
					{
						int randomIntRange = GetRandomIntRange(0, HeroDeck[_heroActive].Count, "deck");
						HeroDeck[_heroActive].Insert(randomIntRange, internalId);
						CreateLogEntry(_initial: true, "toRandomDeck:" + logDictionary.Count, internalId, TeamHero[_heroActive], null, null, null, currentRound);
					}
					else
					{
						int randomIntRange2 = GetRandomIntRange(0, HeroDeck[theHero.HeroIndex].Count, "deck");
						HeroDeck[theHero.HeroIndex].Insert(randomIntRange2, internalId);
						CreateLogEntry(_initial: true, "toRandomDeck:" + logDictionary.Count, internalId, TeamHero[theHero.HeroIndex], null, null, null, currentRound);
					}
					break;
				case Enums.CardPlace.Vanish:
					break;
				}
				yield return Globals.Instance.WaitForSeconds(0.15f);
				if (isHero && _heroActive == heroActive)
				{
					DrawDeckPile(CountHeroDeck(_heroActive) + 1);
				}
				if (where != Enums.CardPlace.RandomDeck)
				{
					CIG[j].CardData.Visible = true;
				}
				CIG[j].discard = true;
				if (isHero)
				{
					if (cardActive != null && cardActive.TargetSide == Enums.CardTargetSide.Friend)
					{
						yield return Globals.Instance.WaitForSeconds(0.02f);
						if (CIG[j] != null)
						{
							GameManager.Instance.GenerateParticleTrail(2, CIG[j].transform.position, TeamHero[_heroActive].HeroItem.transform.position + new Vector3(0f, 2f, 0f));
							UnityEngine.Object.Destroy(CIG[j].gameObject);
						}
					}
					else if (_heroActive > -1)
					{
						yield return Globals.Instance.WaitForSeconds(0.02f);
						if (CIG[j] != null)
						{
							GameManager.Instance.GenerateParticleTrail(2, CIG[j].transform.position, TeamHero[_heroActive].HeroItem.transform.position + new Vector3(0f, 2f, 0f));
							UnityEngine.Object.Destroy(CIG[j].gameObject);
						}
					}
					else if (theHero != null)
					{
						yield return Globals.Instance.WaitForSeconds(0.02f);
						if (CIG[j] != null)
						{
							GameManager.Instance.GenerateParticleTrail(2, CIG[j].transform.position, GO_DeckPile.transform.position);
							UnityEngine.Object.Destroy(CIG[j].gameObject);
						}
					}
				}
				else
				{
					yield return Globals.Instance.WaitForSeconds(0.02f);
					if (CIG[j] != null)
					{
						CIG[j].destroyAtLocation = true;
						GameManager.Instance.GenerateParticleTrail(2, CIG[j].transform.position, TeamNPC[_heroActive].NPCItem.transform.position + new Vector3(0f, 2f, 0f));
						UnityEngine.Object.Destroy(CIG[j].gameObject);
					}
				}
			}
		}
		yield return null;
		if (theHero != null)
		{
			SetGameBusy(state: false);
		}
		generatedCardTimes--;
		if (generatedCardTimes < 0)
		{
			generatedCardTimes = 0;
		}
		SetEventDirect("GenerateNewCard" + theCards[0], automatic: false);
		foreach (string item in theCards)
		{
			theHero?.SetEvent(Enums.EventActivation.DrawCard, null, 0, item);
		}
	}

	private void MoveCardTo(int numCards, string theCard, Enums.CardPlace cardPlace)
	{
		StartCoroutine(MoveCardToCo(numCards, theCard, cardPlace));
	}

	private IEnumerator MoveCardToCo(int numCards, string theCard, Enums.CardPlace cardPlace)
	{
		List<CardItem> CIG = new List<CardItem>();
		for (int i = 0; i < numCards; i++)
		{
			GameObject obj = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero + new Vector3((float)i * 0.1f, (float)i * -0.1f, 0f), Quaternion.identity, GO_DeckPile.transform);
			CardItem component = obj.GetComponent<CardItem>();
			obj.name = theCard;
			component.SetCard(theCard, deckScale: false, theHero);
			component.DefaultElementsLayeringOrder(50 * i);
			CIG.Add(component);
			yield return Globals.Instance.WaitForSeconds(0.1f);
		}
		yield return Globals.Instance.WaitForSeconds(0.2f);
		for (int i = CIG.Count - 1; i >= 0; i--)
		{
			if (cardPlace == Enums.CardPlace.Discard)
			{
				HeroDeckDiscard[heroActive].Insert(0, theCard);
				DrawDiscardPileCardNumeral();
				CIG[i].DiscardCard(discardedFromHand: false);
			}
			else
			{
				switch (cardPlace)
				{
				case Enums.CardPlace.TopDeck:
					HeroDeck[heroActive].Insert(0, theCard);
					break;
				case Enums.CardPlace.BottomDeck:
					HeroDeck[heroActive].Insert(HeroDeck[heroActive].Count, theCard);
					break;
				default:
					HeroDeck[heroActive].Insert(GetRandomIntRange(0, HeroDeck[heroActive].Count, "deck"), theCard);
					break;
				}
				DrawDeckPile(CountHeroDeck() + 1);
				if (cardPlace != Enums.CardPlace.RandomDeck)
				{
					CIG[i].CardData.Visible = true;
				}
				CIG[i].discard = true;
				CIG[i].MoveCardToDeckPile();
			}
			yield return Globals.Instance.WaitForSeconds(0.5f);
		}
		theHero.SetEvent(Enums.EventActivation.CardMoved);
	}

	private IEnumerator DealCards()
	{
		if (MatchIsOver)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("MOVEDECKSOUT Broken by finish game", "trace");
			}
			yield break;
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("DealCards", "trace");
		}
		int eventExaust = 0;
		while (eventList.Count > 0)
		{
			if (GameManager.Instance.GetDeveloperMode() && eventExaust % 50 == 0)
			{
				eventListDbg = "";
				for (int i = 0; i < eventList.Count; i++)
				{
					eventListDbg = eventListDbg + eventList[i] + " || ";
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[DealCards] Waiting For Eventlist to clean", "trace");
				}
			}
			eventExaust++;
			if (eventExaust > 300)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[DealCards] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "trace");
				}
				ClearEventList();
				break;
			}
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
		yield return Globals.Instance.WaitForSeconds(0.05f);
		if (GameManager.Instance.IsMultiplayer())
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("**************************", "net");
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("WaitingSyncro dealcards", "net");
			}
			if (NetworkManager.Instance.IsMaster())
			{
				if (coroutineSyncDealCards != null)
				{
					StopCoroutine(coroutineSyncDealCards);
				}
				coroutineSyncDealCards = StartCoroutine(ReloadCombatCo("dealcards"));
				while (!NetworkManager.Instance.AllPlayersReady("dealcards"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (coroutineSyncDealCards != null)
				{
					StopCoroutine(coroutineSyncDealCards);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked dealcards", "net");
				}
				SetRandomIndex(randomIndex);
				NetworkManager.Instance.PlayersNetworkContinue("dealcards", randomIndex.ToString());
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("dealcards", status: true);
				NetworkManager.Instance.SetStatusReady("dealcards");
				while (NetworkManager.Instance.WaitingSyncro["dealcards"])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (NetworkManager.Instance.netAuxValue != "")
				{
					SetRandomIndex(int.Parse(NetworkManager.Instance.netAuxValue));
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("dealcards, we can continue!", "net");
				}
			}
		}
		if (MatchIsOver)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("MOVEDECKSOUT Broken by finish game", "trace");
			}
		}
		else
		{
			CreateLogEntry(_initial: true, "dealcards" + logDictionary.Count, "", null, null, null, null, currentRound);
			NewCard(theHero.GetDrawCardsTurn() - 1, Enums.CardFrom.Deck);
			theHero.SetEvent(Enums.EventActivation.DrawCard);
			theHero.SetEvent(Enums.EventActivation.AfterDealCards);
			ResetCardHoverIndex();
		}
	}

	private IEnumerator DealCardsContinue()
	{
		if (MatchIsOver)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("MOVEDECKSOUT Broken by finish game", "trace");
			}
		}
		else
		{
			NewCard(theHero.GetDrawCardsTurn(), Enums.CardFrom.Deck);
			ResetCardHoverIndex();
			yield return null;
		}
	}

	[PunRPC]
	private void NET_DealCardsContinue()
	{
		StartCoroutine(DealCardsContinue());
	}

	[PunRPC]
	private void NET_DealCardsContinueSync(int _randomIndex)
	{
		SetRandomIndex(_randomIndex);
		NetworkManager.Instance.SetStatusReady("dealcards");
	}

	public void NewCard(int numCards, Enums.CardFrom fromPlace, string comingFromCardId = "")
	{
		if (theHero == null)
		{
			return;
		}
		int num = CountHeroDiscard() + CountHeroDeck();
		if (num == 0)
		{
			SetCardsWaitingForReset(0);
			isBeginTournPhase = false;
			return;
		}
		if (numCards > num)
		{
			numCards = num;
		}
		cardsWaitingForReset += numCards;
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[NEW CARD] cardsWaitingForReset " + cardsWaitingForReset, "trace");
		}
		if (cardsWaitingForReset > 0)
		{
			StartCoroutine(DealNewCard(fromPlace, comingFromCardId));
		}
		else if (castingCardBlocked.ContainsKey(comingFromCardId))
		{
			Debug.Log("**** CASTINGCARDBLOCKED REMOVE " + comingFromCardId);
			castingCardBlocked[comingFromCardId] = false;
		}
	}

	private IEnumerator DealNewCard(Enums.CardFrom fromPlace, string comingFromCardId = "")
	{
		if (!isBeginTournPhase)
		{
			string codeGen = GenerateSyncCodeForCheckingAction() + GenerateStatusString();
			yield return Globals.Instance.WaitForSeconds(0.05f);
			while (waitingKill)
			{
				yield return Globals.Instance.WaitForSeconds(0.05f);
			}
			string text = GenerateSyncCodeForCheckingAction() + GenerateStatusString();
			int exaustCodeGen = 0;
			while (codeGen != text)
			{
				codeGen = text;
				yield return Globals.Instance.WaitForSeconds(0.05f);
				text = GenerateSyncCodeForCheckingAction() + GenerateStatusString();
				exaustCodeGen++;
				if (exaustCodeGen > 50)
				{
					codeGen = text;
				}
			}
		}
		ResetAutoEndCount();
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("DealNewCard ___ " + comingFromCardId, "general");
		}
		if (cardsWaitingForReset > 0)
		{
			int num = CountHeroHand();
			int num2 = 10;
			if (num + 1 > num2)
			{
				SetCardsWaitingForReset(0);
				if (isBeginTournPhase)
				{
					BeginTurnHero();
					yield break;
				}
			}
			else if (CountHeroDeck() == 0)
			{
				if (CountHeroDiscard() > 0)
				{
					if (GameManager.Instance.IsMultiplayer())
					{
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("**************************", "net");
						}
						if (Globals.Instance.ShowDebug)
						{
							Functions.DebugLogGD("WaitingSyncro shuffle", "net");
						}
						yield return Globals.Instance.WaitForSeconds(0.1f);
						if (NetworkManager.Instance.IsMaster())
						{
							if (coroutineSyncShuffle != null)
							{
								StopCoroutine(coroutineSyncShuffle);
							}
							coroutineSyncShuffle = StartCoroutine(ReloadCombatCo("shuffle"));
							while (!NetworkManager.Instance.AllPlayersReady("shuffle"))
							{
								yield return Globals.Instance.WaitForSeconds(0.01f);
							}
							if (coroutineSyncShuffle != null)
							{
								StopCoroutine(coroutineSyncShuffle);
							}
							if (Globals.Instance.ShowDebug)
							{
								Functions.DebugLogGD("Game ready, Everybody checked shuffle", "net");
							}
							SetRandomIndex(randomShuffleIndex, "shuffle");
							NetworkManager.Instance.PlayersNetworkContinue("shuffle", randomShuffleIndex.ToString());
						}
						else
						{
							NetworkManager.Instance.SetWaitingSyncro("shuffle", status: true);
							NetworkManager.Instance.SetStatusReady("shuffle");
							while (NetworkManager.Instance.WaitingSyncro["shuffle"])
							{
								yield return Globals.Instance.WaitForSeconds(0.01f);
							}
							if (NetworkManager.Instance.netAuxValue != "")
							{
								SetRandomIndex(int.Parse(NetworkManager.Instance.netAuxValue), "shuffle");
							}
							if (Globals.Instance.ShowDebug)
							{
								Functions.DebugLogGD("shuffle, we can continue!", "net");
							}
						}
					}
					yield return StartCoroutine(ResetDeck());
					yield return Globals.Instance.WaitForSeconds(0.1f);
					while (eventList.Contains("ResetDeck"))
					{
						yield return Globals.Instance.WaitForSeconds(0.01f);
					}
					GiveExhaust();
					yield return StartCoroutine(DealNewCard(fromPlace, comingFromCardId));
					yield break;
				}
				SetCardsWaitingForReset(0);
				if (isBeginTournPhase)
				{
					BeginTurnHero();
					yield break;
				}
			}
			else
			{
				if (isBeginTournPhase)
				{
					if (GameManager.Instance.IsMultiplayer())
					{
						yield return Globals.Instance.WaitForSeconds(0.07f);
					}
					else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
					{
						yield return Globals.Instance.WaitForSeconds(0.1f);
					}
					else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Ultrafast)
					{
						yield return Globals.Instance.WaitForSeconds(0.02f);
					}
					else
					{
						yield return Globals.Instance.WaitForSeconds(0.05f);
					}
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("CardsWaitingForRset PRE " + cardsWaitingForReset, "trace");
				}
				GameManager.Instance.PlayLibraryAudio("dealcard");
				GetCardFromDeckToHand();
				ResetAutoEndCount();
				StartCoroutine(CreateCard(CountHeroHand() - 1, fromPlace));
				if (!isBeginTournPhase)
				{
					if (GameManager.Instance.IsMultiplayer())
					{
						yield return Globals.Instance.WaitForSeconds(0.1f);
					}
					else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
					{
						yield return Globals.Instance.WaitForSeconds(0.1f);
					}
					else
					{
						yield return Globals.Instance.WaitForSeconds(0.07f);
					}
				}
				if (cardItemTable[cardItemTable.Count - 1] == null)
				{
					yield break;
				}
				CardData _cardData = cardItemTable[cardItemTable.Count - 1].CardData;
				if (_cardData != null && _cardData.AutoplayDraw)
				{
					string codeGen = gameStatus;
					CardItem CI = cardItemTable[cardItemTable.Count - 1];
					CI.SetDestinationScaleRotation(Vector3.zero - CI.transform.parent.transform.localPosition, 1.4f, Quaternion.Euler(0f, 0f, 0f));
					CI.DrawBorder("blue");
					CI.active = true;
					if (_cardData.CardClass == Enums.CardClass.Injury)
					{
						yield return Globals.Instance.WaitForSeconds(0.4f);
					}
					else
					{
						yield return Globals.Instance.WaitForSeconds(0.3f);
					}
					StartCoroutine(CastCard(CI));
					yield return Globals.Instance.WaitForSeconds(0.1f);
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("Autoplay " + _cardData.InternalId, "trace");
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD(castingCardBlocked.ContainsKey(_cardData.InternalId).ToString(), "trace");
					}
					if (castingCardBlocked.ContainsKey(_cardData.InternalId))
					{
						int exaustCodeGen = 0;
						while (castingCardBlocked.ContainsKey(_cardData.InternalId) && castingCardBlocked[_cardData.InternalId])
						{
							while (waitingDeathScreen)
							{
								if (Globals.Instance.ShowDebug)
								{
									Functions.DebugLogGD("DealNewCard -  waitingDeathScreen", "trace");
								}
								yield return Globals.Instance.WaitForSeconds(0.01f);
							}
							bool exit = false;
							if (theHero != null && !theHero.Alive)
							{
								yield return Globals.Instance.WaitForSeconds(1f);
								if (theHero != null && !theHero.Alive)
								{
									exit = true;
								}
							}
							else if (theNPC != null && !theNPC.Alive)
							{
								yield return Globals.Instance.WaitForSeconds(0.1f);
								if (theNPC != null && !theNPC.Alive)
								{
									exit = true;
								}
							}
							if (exit)
							{
								if (Globals.Instance.ShowDebug)
								{
									Functions.DebugLogGD("DealNewCard - Character died", "trace");
								}
								if (theHero != null && !theHero.Alive && IsBeginTournPhase)
								{
									yield return Globals.Instance.WaitForSeconds(0.5f);
									if (theHero != null && !theHero.Alive && IsBeginTournPhase)
									{
										if (Globals.Instance.ShowDebug)
										{
											Functions.DebugLogGD("DealNewCard - Goto endturn", "trace");
										}
										SetCardsWaitingForReset(0);
										if (comingFromCardId != "")
										{
											Debug.Log("**** CASTINGCARDBLOCKED REMOVE " + comingFromCardId);
											castingCardBlocked[comingFromCardId] = false;
										}
										EndTurn();
										yield break;
									}
								}
							}
							yield return Globals.Instance.WaitForSeconds(0.01f);
							exaustCodeGen++;
							if (exaustCodeGen > 10000)
							{
								Debug.Log("Index extreme exhausted");
								Debug.Log("**** CASTINGCARDBLOCKED REMOVE " + comingFromCardId);
								castingCardBlocked[_cardData.InternalId] = false;
							}
							if (GameManager.Instance.GetDeveloperMode() && exaustCodeGen % 100 == 0)
							{
								Debug.Log("[DEALNEWCARD] indexExtremeBlock" + exaustCodeGen + " <-- " + _cardData.InternalId);
							}
						}
					}
					yield return Globals.Instance.WaitForSeconds(0.1f);
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("End autodraw cast card " + _cardData.InternalId, "general");
					}
					gameStatus = codeGen;
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("GameStatus->" + gameStatus, "general");
					}
					if (comingFromCardId == "")
					{
						yield return Globals.Instance.WaitForSeconds(0.3f);
					}
				}
				else if (gameStatus == "AddingCards" || gameStatus == "DrawingCards")
				{
					if (cardActive != null && cardActive.Id.ToLower().StartsWith("repetitiontraining"))
					{
						theHero?.SetEvent(Enums.EventActivation.DrawCard, null, 0, _cardData.Id);
					}
					yield return Globals.Instance.WaitForSeconds(0.2f);
				}
				RepositionCards();
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("CardsWaitingForRset POST " + cardsWaitingForReset, "general");
				}
				cardsWaitingForReset--;
				if (cardsWaitingForReset < 0)
				{
					SetCardsWaitingForReset(0);
				}
				ResetAutoEndCount();
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("comingFromCardId" + comingFromCardId, "general");
				}
				if (comingFromCardId == "")
				{
					if (cardsWaitingForReset > 0)
					{
						if (theHero != null && theHero.Alive)
						{
							StartCoroutine(DealNewCard(Enums.CardFrom.Deck));
							yield break;
						}
						yield return Globals.Instance.WaitForSeconds(1f);
						if (theHero != null && theHero.Alive)
						{
							StartCoroutine(DealNewCard(Enums.CardFrom.Deck));
						}
						yield break;
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("finish new card Gamestatus->" + gameStatus, "general");
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("isBeginTournPhase->" + isBeginTournPhase, "general");
					}
					if (isBeginTournPhase)
					{
						int exaustCodeGen = 0;
						while (eventList.Count > 0)
						{
							if (GameManager.Instance.GetDeveloperMode() && exaustCodeGen % 50 == 0)
							{
								eventListDbg = "Waiting begin turn because of eventlist ";
								for (int i = 0; i < eventList.Count; i++)
								{
									eventListDbg = eventListDbg + eventList[i] + " || ";
								}
							}
							exaustCodeGen++;
							if (exaustCodeGen > 150)
							{
								Debug.Log("[Waiting begin turn] EXHAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
								ClearEventList();
								break;
							}
							yield return Globals.Instance.WaitForSeconds(0.01f);
						}
						BeginTurnHero();
						yield break;
					}
				}
			}
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("FINISH DEAL CARD ->" + comingFromCardId, "trace");
		}
		if (comingFromCardId != "")
		{
			castingCardBlocked[comingFromCardId] = false;
		}
		yield return null;
	}

	private IEnumerator CreateCard(int i, Enums.CardFrom fromPlace)
	{
		if (heroActive > -1)
		{
			string id = HeroHand[heroActive][i];
			GameObject gameObject = ((fromPlace != Enums.CardFrom.Deck) ? UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, DiscardPilePosition, Quaternion.identity, GO_Hand.transform) : UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, DeckPilePosition, Quaternion.identity, GO_Hand.transform));
			CardItem component = gameObject.GetComponent<CardItem>();
			gameObject.name = id;
			gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, -179f);
			cardItemTable.Add(component);
			component.SetTablePositionValues(i, CountHeroHand());
			if (fromPlace == Enums.CardFrom.Deck || fromPlace == Enums.CardFrom.Discard)
			{
				component.SetCard(id, deckScale: true, theHero, null, GetFromGlobal: false, _generated: true);
			}
			else
			{
				component.SetCard(id, deckScale: true, theHero);
			}
			component.DisableCollider();
			if (gameStatus == "AddingCards" || gameStatus == "DrawingCards")
			{
				component.SetDestinationScaleRotation(Vector3.zero - component.transform.parent.transform.localPosition, 1.4f, Quaternion.Euler(0f, 0f, 0f));
				component.active = true;
				yield return null;
			}
		}
	}

	private IEnumerator ResetDeck(bool ShuffleAction = true)
	{
		SetEventDirect("ResetDeck");
		TeamHero[heroActive].SetEvent(Enums.EventActivation.ResetDeck);
		if (HeroDeckDiscard[heroActive] != null)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < CountHeroDiscard(); i++)
			{
				list.Add(HeroDeckDiscard[heroActive][i]);
			}
			if (list.Count > 0)
			{
				if (ShuffleAction)
				{
					HeroDeck[heroActive] = list.ShuffleList();
				}
				HeroDeckDiscard[heroActive].Clear();
				if (GameManager.Instance.IsMultiplayer())
				{
					yield return Globals.Instance.WaitForSeconds(0.3f);
				}
				else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
				{
					yield return Globals.Instance.WaitForSeconds(0.5f);
				}
				else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Ultrafast)
				{
					yield return Globals.Instance.WaitForSeconds(0.1f);
				}
				else
				{
					yield return Globals.Instance.WaitForSeconds(0.3f);
				}
				if (shuffleSound != null)
				{
					GameManager.Instance.PlayAudio(shuffleSound);
				}
				WriteDiscardCounter("0");
				int index = 0;
				for (int i2 = GO_DiscardPile.transform.childCount - 1; i2 >= 0; i2--)
				{
					Transform child = GO_DiscardPile.transform.GetChild(i2);
					DrawDeckPile(index + 1);
					if (child.localPosition.z >= 0f)
					{
						if (GameManager.Instance.IsMultiplayer())
						{
							child.transform.localPosition -= new Vector3(0.5f, 0f, 1f);
							yield return Globals.Instance.WaitForSeconds(0.02f);
						}
						else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
						{
							child.transform.localPosition -= new Vector3(0.5f, 0f, 1f);
							yield return Globals.Instance.WaitForSeconds(0.02f);
						}
						else
						{
							yield return null;
						}
					}
					child.gameObject.SetActive(value: false);
				}
				DestroyDiscardPileChilds();
				SetEventDirect("ResetDeck", automatic: false);
				yield break;
			}
			SetCardsWaitingForReset(0);
		}
		SetEventDirect("ResetDeck", automatic: false);
	}

	public void DestroyDiscardPileChilds()
	{
		for (int num = GO_DiscardPile.transform.childCount - 1; num >= 0; num--)
		{
			UnityEngine.Object.Destroy(GO_DiscardPile.transform.GetChild(num).gameObject);
		}
	}

	public int NumChildsInTemporal()
	{
		return tempTransform.childCount;
	}

	private void GetCardFromDeckToHand()
	{
		int num = CountHeroDeck();
		DrawDeckPile(num);
		if (num > 0)
		{
			string text = "";
			text = HeroDeck[heroActive][0];
			HeroDeck[heroActive].RemoveAt(0);
			HeroHand[heroActive].Add(text);
		}
	}

	public void GetCardFromDeckToHandNPC(int npcIndex)
	{
		string text = "";
		if (CountNPCDeck(npcIndex) == 0)
		{
			NPCDeck[npcIndex] = new List<string>(NPCDeckDiscard[npcIndex]);
			NPCDeckDiscard[npcIndex].Clear();
			NPCDeck[npcIndex] = NPCDeck[npcIndex].ShuffleList();
		}
		if (NPCDeck[npcIndex].Count > 0)
		{
			text = NPCDeck[npcIndex][0];
			NPCDeck[npcIndex].RemoveAt(0);
			if (NPCHand[npcIndex] == null)
			{
				NPCHand[npcIndex] = new List<string>();
			}
			NPCHand[npcIndex].Add(text);
		}
	}

	public void ResetDeckHandNPC(int npcIndex)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("ResetDeckHandNPC " + npcIndex, "trace");
		}
		NPCHand[npcIndex].Clear();
	}

	public void AddCardToNPCDeck(int npcIndex, string idCard, string npcIinternalId)
	{
		string text = idCard + "_" + GetRandomString();
		text = text.ToLower();
		if (!cardDictionary.ContainsKey(text))
		{
			CardData cardData = Globals.Instance.GetCardData(idCard, instantiate: false);
			cardDictionary.Add(text, cardData);
		}
		cardDictionary[text].InitClone(idCard);
		int num = 0;
		for (int i = 0; i < NPCDeck[npcIndex].Count; i++)
		{
			if (NPCDeck[npcIndex] == null)
			{
				break;
			}
			if (NPCDeck[npcIndex][i] == null)
			{
				break;
			}
			if (!Globals.Instance.GetCardData(NPCDeck[npcIndex][i].Split('_')[0], instantiate: false).Corrupted)
			{
				break;
			}
			num++;
		}
		NPCDeck[npcIndex].Insert(num, text);
	}

	public void RefreshStatusEffects()
	{
		for (int i = 0; i < TeamHero.Length; i++)
		{
			if (TeamHero[i] != null && TeamHero[i].Alive)
			{
				TeamHero[i].UpdateAuraCurseFunctions();
			}
		}
		for (int j = 0; j < TeamNPC.Length; j++)
		{
			if (TeamNPC[j] != null && TeamNPC[j].Alive)
			{
				TeamNPC[j].UpdateAuraCurseFunctions();
			}
		}
	}

	public void RefreshItems()
	{
		iconWeapon.ShowIcon("weapon");
		iconArmor.ShowIcon("armor");
		iconJewelry.ShowIcon("jewelry");
		iconAccesory.ShowIcon("accesory");
		iconPet.ShowIcon("pet");
		if (theHero != null && theHero.HeroItem != null)
		{
			theHero.HeroItem.ShowEnchantments();
			theHero.UpdateAuraCurseFunctions();
		}
	}

	public void RemoveCorruptionItemFromNPC(int _npcIndex)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("RemoveCorruptionItemFromNPC " + _npcIndex);
		}
		TeamNPC[_npcIndex].Corruption = "";
	}

	private void ClearItemExecuteForThisTurn()
	{
		itemExecutedInTurn.Clear();
	}

	private void ClearItemExecuteForThisTurnSpecific(string _key)
	{
		if (itemExecutedInTurn.ContainsKey(_key))
		{
			itemExecutedInTurn.Remove(_key);
		}
	}

	public bool CanExecuteItemInThisTurn(string charId, string itemId, int timesPerTurn)
	{
		string key = charId + "_" + itemId;
		if (itemExecutedInTurn.ContainsKey(key) && itemExecutedInTurn[key] >= timesPerTurn)
		{
			return false;
		}
		return true;
	}

	public bool CanExecuteItemInThisCombat(string charId, string itemId, int timesPerCombat)
	{
		string key = charId + "_" + itemId;
		if (itemExecutedInCombat.ContainsKey(key) && itemExecutedInCombat[key] >= timesPerCombat)
		{
			return false;
		}
		return true;
	}

	public bool ItemExecuteForThisTurn(string charId, string itemId, int timesPerTurn, string itemType)
	{
		int num = 1;
		string key = charId + "_" + itemId;
		if (itemExecutedInTurn.ContainsKey(key))
		{
			if (itemExecutedInTurn[key] >= timesPerTurn)
			{
				return true;
			}
			num = itemExecutedInTurn[key] + 1;
			itemExecutedInTurn[key] = num;
		}
		else
		{
			itemExecutedInTurn.Add(key, num);
		}
		switch (itemType)
		{
		case "weapon":
			iconWeapon.SetTimesExecuted(num);
			break;
		case "armor":
			iconArmor.SetTimesExecuted(num);
			break;
		case "jewelry":
			iconJewelry.SetTimesExecuted(num);
			break;
		case "accesory":
			iconAccesory.SetTimesExecuted(num);
			break;
		}
		return false;
	}

	private void ItemExecuteForThisCombatDuplicate()
	{
		itemExecutedInCombatTurnSave = new Dictionary<string, int>();
		foreach (KeyValuePair<string, int> item in itemExecutedInCombat)
		{
			itemExecutedInCombatTurnSave.Add(item.Key, item.Value);
		}
	}

	private void ItemExecuteForThisCombatReload()
	{
		itemExecutedInCombat = new Dictionary<string, int>();
		foreach (KeyValuePair<string, int> item in itemExecutedInCombatTurnSave)
		{
			itemExecutedInCombat.Add(item.Key, item.Value);
		}
	}

	public bool ItemExecuteForThisCombat(string charId, string itemId, int timesPerCombat, string itemType)
	{
		int num = 1;
		string key = charId + "_" + itemId;
		if (itemExecutedInCombat.ContainsKey(key))
		{
			if (itemExecutedInCombat[key] >= timesPerCombat)
			{
				return true;
			}
			num = itemExecutedInCombat[key] + 1;
			itemExecutedInCombat[key] = num;
		}
		else
		{
			itemExecutedInCombat.Add(key, num);
		}
		switch (itemType)
		{
		case "weapon":
			iconWeapon.SetTimesExecuted(num);
			break;
		case "armor":
			iconArmor.SetTimesExecuted(num);
			break;
		case "jewelry":
			iconJewelry.SetTimesExecuted(num);
			break;
		case "accesory":
			iconAccesory.SetTimesExecuted(num);
			break;
		}
		return false;
	}

	public int ItemExecutedInThisCombat(string charId, string itemId)
	{
		int result = 0;
		string key = charId + "_" + itemId;
		if (itemExecutedInCombat.ContainsKey(key))
		{
			result = itemExecutedInCombat[key];
		}
		return result;
	}

	public void EnchantmentExecute(string charId, string itemId)
	{
		int value = 1;
		string key = charId + "_" + itemId;
		if (enchantmentExecutedTotal.ContainsKey(key))
		{
			value = enchantmentExecutedTotal[key] + 1;
			enchantmentExecutedTotal[key] = value;
		}
		else
		{
			enchantmentExecutedTotal.Add(key, value);
		}
	}

	public int EnchantmentExecutedTimes(string charId, string itemId)
	{
		string key = charId + "_" + itemId;
		if (enchantmentExecutedTotal.ContainsKey(key))
		{
			return enchantmentExecutedTotal[key];
		}
		return 0;
	}

	public void CleanEnchantmentExecutedTimes(string charId, string itemId)
	{
		string key = charId + "_" + itemId;
		if (enchantmentExecutedTotal.ContainsKey(key))
		{
			enchantmentExecutedTotal.Remove(key);
		}
	}

	public void DoItemEventDelay()
	{
		StartCoroutine(DoItemEventDelayCo());
	}

	public IEnumerator DoItemEventDelayCo()
	{
		string waitEvent = "DoItemEventDelay_+" + Time.frameCount;
		eventList.Add(waitEvent);
		yield return Globals.Instance.WaitForSeconds(0.7f);
		eventList.Remove(waitEvent);
	}

	public void ItemActivationDisplay(string itemType)
	{
		switch (itemType)
		{
		case "weapon":
			iconWeapon.SetActivated();
			break;
		case "armor":
			iconArmor.SetActivated();
			break;
		case "jewelry":
			iconJewelry.SetActivated();
			break;
		case "accesory":
			iconAccesory.SetActivated();
			break;
		}
	}

	private IEnumerator MoveItemsOut(bool state)
	{
		if (!state)
		{
			iconWeapon.MoveIn("weapon", 0f, theHero);
			iconArmor.MoveIn("armor", 0.1f, theHero);
			iconJewelry.MoveIn("jewelry", 0.2f, theHero);
			iconAccesory.MoveIn("accesory", 0.3f, theHero);
			iconPet.MoveIn("pet", 0.4f, theHero);
		}
		else
		{
			iconPet.MoveOut(0f);
			iconAccesory.MoveOut(0.05f);
			iconJewelry.MoveOut(0.1f);
			iconArmor.MoveOut(0.15f);
			iconWeapon.MoveOut(0.2f);
		}
		yield return null;
	}

	private IEnumerator MoveDecksOut(bool state)
	{
		MovingDeckPile = true;
		if (MatchIsOver)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[MOVEDECKSOUT] Broken by finish game");
			}
			yield break;
		}
		while (waitingKill)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[MOVEDECKSOUT] Waitingforkill");
			}
			yield return Globals.Instance.WaitForSeconds(0.1f);
		}
		if (!theHero.Alive && !state)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[MOVEDECKSOUT] Broken by hero dead");
			}
			yield break;
		}
		Vector3 vectorDestination;
		if (state)
		{
			for (int num = GO_DiscardPile.transform.childCount - 1; num >= 0; num--)
			{
				CardItem component = GO_DiscardPile.transform.GetChild(num).GetComponent<CardItem>();
				component.GetComponent<BoxCollider2D>().enabled = false;
				component.DisableTrail();
			}
			vectorDestination = DeckPilePosition - DeckPileOutOfScreenPositionVector;
		}
		else
		{
			if (!GO_DecksObject.gameObject.activeSelf)
			{
				GO_DecksObject.gameObject.SetActive(value: true);
			}
			vectorDestination = DeckPilePosition;
			RepaintDeckPile();
			RedoDiscardPile();
			WriteNewCardsCounter();
		}
		if (!state)
		{
			foreach (Transform item in GO_Hand.transform)
			{
				if (item != null && item.gameObject.name != "HandMask")
				{
					UnityEngine.Object.Destroy(item.gameObject);
				}
			}
		}
		if (!state && gameStatus == "BeginTurnHero")
		{
			ResetItemTimeout();
			theHero.SetEvent(Enums.EventActivation.BeginTurnAboutToDealCards);
			int eventExaustBeginH = 0;
			if (theHero.HaveAboutToDealCardsItemNum() > 0)
			{
				Debug.Log("****** enter ***** " + gameStatus);
				yield return Globals.Instance.WaitForSeconds(0.2f * (float)theHero.HaveAboutToDealCardsItemNum());
				while (generatedCardTimes > 0)
				{
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("*********** " + generatedCardTimes + "||" + gameStatus + "||" + NumChildsInTemporal(), "item");
					}
					yield return Globals.Instance.WaitForSeconds(0.7f);
					eventExaustBeginH++;
					if (eventExaustBeginH > 10)
					{
						generatedCardTimes = 0;
					}
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("******exit ***** " + gameStatus, "item");
				}
			}
			yield return Globals.Instance.WaitForSeconds(0.1f);
			eventExaustBeginH = 0;
			while (eventList.Count > 0)
			{
				if (GameManager.Instance.GetDeveloperMode() && eventExaustBeginH % 50 == 0)
				{
					eventListDbg = "";
					for (int i = 0; i < eventList.Count; i++)
					{
						eventListDbg = eventListDbg + eventList[i] + " || ";
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD(eventListDbg);
					}
				}
				eventExaustBeginH++;
				if (eventExaustBeginH > 200)
				{
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("[MoveDecksOut IN] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
					}
					ClearEventList();
					break;
				}
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[MOVING DECK PILE MOVEMENT]", "trace");
		}
		float movementSpeed = 0.9f;
		while (Vector3.Distance(GO_DecksObject.transform.localPosition, vectorDestination) > 0.25f)
		{
			GO_DecksObject.transform.localPosition = Vector3.Lerp(GO_DecksObject.transform.localPosition, vectorDestination, movementSpeed);
			yield return null;
		}
		GO_DecksObject.transform.localPosition = vectorDestination;
		if (state)
		{
			if (GO_DecksObject.gameObject.activeSelf)
			{
				GO_DecksObject.gameObject.SetActive(value: false);
			}
			DestroyDiscardPileChilds();
		}
		if (!state)
		{
			yield return Globals.Instance.WaitForSeconds(0.1f);
			int eventExaustBeginH = 0;
			while (eventList.Count > 0)
			{
				if (GameManager.Instance.GetDeveloperMode() && eventExaustBeginH % 50 == 0)
				{
					eventListDbg = "";
					for (int j = 0; j < eventList.Count; j++)
					{
						eventListDbg = eventListDbg + eventList[j] + " || ";
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("[MOVING DECK PILE END] Waiting For Eventlist to clean");
					}
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD(eventListDbg);
					}
				}
				eventExaustBeginH++;
				if (eventExaustBeginH > 500)
				{
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("[MOVING DECK PILE END] EXAUSTED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
					}
					ClearEventList();
					break;
				}
				yield return Globals.Instance.WaitForSeconds(0.01f);
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("=>" + eventExaustBeginH);
				}
			}
			if (eventExaustBeginH > 5)
			{
				if (GameManager.Instance.IsMultiplayer())
				{
					yield return Globals.Instance.WaitForSeconds(0.3f);
				}
				else
				{
					yield return Globals.Instance.WaitForSeconds(0.15f);
				}
			}
		}
		MovingDeckPile = false;
		Cursor.visible = true;
	}

	public string CardFromNPCHand(int index, int position)
	{
		if (NPCHand != null && index < NPCHand.Length && NPCHand[index] != null && position < NPCHand[index].Count && NPCHand[index][position] != null)
		{
			return NPCHand[index][position];
		}
		return "";
	}

	public void RemoveCardsFromNPCHand(int index, int position)
	{
		for (int num = NPCHand[index].Count - 1; num >= position; num--)
		{
			NPCHand[index].RemoveAt(num);
		}
	}

	public void MoveCards(Enums.CardPlace from, Enums.CardPlace to, Enums.CardClass cardClass = Enums.CardClass.None, int energyReductionPermanent = 0)
	{
		bool flag = false;
		int num = 0;
		if (from == Enums.CardPlace.Vanish)
		{
			for (int num2 = HeroDeckVanish[heroActive].Count - 1; num2 >= 0; num2--)
			{
				CardData cardData = GetCardData(HeroDeckVanish[heroActive][num2]);
				flag = false;
				if (cardClass == Enums.CardClass.None)
				{
					flag = true;
				}
				else if (cardClass == cardData.CardClass)
				{
					flag = true;
				}
				if (flag)
				{
					if (energyReductionPermanent != 0)
					{
						cardData.EnergyReductionPermanent += energyReductionPermanent;
					}
					if (to == Enums.CardPlace.RandomDeck)
					{
						num = GetRandomIntRange(0, HeroDeck[heroActive].Count);
						HeroDeck[heroActive].Insert(num, HeroDeckVanish[heroActive][num2]);
					}
					HeroDeckVanish[heroActive].RemoveAt(num2);
				}
			}
		}
		if (to == Enums.CardPlace.RandomDeck || to == Enums.CardPlace.BottomDeck || to == Enums.CardPlace.TopDeck)
		{
			DrawDeckPile(CountHeroDeck() + 1);
		}
	}

	private void WriteDiscardCounter(string text = "")
	{
		if (text == "")
		{
			text = CountHeroDiscard().ToString();
		}
		if (text == "0")
		{
			text = "";
			if (discardCounter.gameObject.activeSelf)
			{
				discardCounter.gameObject.SetActive(value: false);
			}
		}
		else if (!discardCounter.gameObject.activeSelf)
		{
			discardCounter.gameObject.SetActive(value: true);
		}
		discardCounterTM.text = text;
		combatTarget.RefreshCards();
	}

	private void WriteNewCardsCounter()
	{
		if (theHero != null)
		{
			newCardsCounterTM.text = "+" + theHero.GetDrawCardsTurnForDisplayInDeck();
		}
	}

	private void WriteDeckCounter(string text = "")
	{
		if (text == "")
		{
			text = CountHeroDeck().ToString();
		}
		deckCounterTM.text = text;
		combatTarget.RefreshCards();
	}

	private void DrawDeckPile(int numCards)
	{
		int num = numCards - 1;
		WriteDeckCounter(num.ToString());
		int num2 = ((num > 0) ? ((num < 3) ? 1 : ((num >= 7) ? 3 : 2)) : 0);
		if (num2 == deckPileVisualState)
		{
			return;
		}
		for (int i = 1; i <= num2; i++)
		{
			GO_DeckPileCards[i - 1].transform.gameObject.SetActive(value: true);
			if (i < num2)
			{
				GO_DeckPileCards[i - 1].GetComponent<BoxCollider2D>().enabled = false;
			}
			else
			{
				GO_DeckPileCards[i - 1].GetComponent<BoxCollider2D>().enabled = true;
			}
		}
		for (int j = num2 + 1; j <= 3; j++)
		{
			GO_DeckPileCards[j - 1].transform.gameObject.SetActive(value: false);
		}
		deckPileVisualState = num2;
	}

	private void SetCardbacks()
	{
		if (theHero == null)
		{
			return;
		}
		string cardbackUsed = theHero.CardbackUsed;
		if (!(cardbackUsed != ""))
		{
			return;
		}
		CardbackData cardbackData = Globals.Instance.GetCardbackData(cardbackUsed);
		if (cardbackData == null)
		{
			cardbackData = Globals.Instance.GetCardbackData(Globals.Instance.GetCardbackBaseIdBySubclass(theHero.HeroData.HeroSubClass.Id));
			if (cardbackData == null)
			{
				cardbackData = Globals.Instance.GetCardbackData("defaultCardback");
			}
		}
		Sprite cardbackSprite = cardbackData.CardbackSprite;
		if (cardbackSprite != null)
		{
			for (int i = 0; i < GO_DeckPileCardsT.Length; i++)
			{
				GO_DeckPileCardsT[i].GetComponent<SpriteRenderer>().sprite = cardbackSprite;
			}
		}
	}

	private void DrawDeckPileLayer(string theLayer)
	{
		for (int i = 0; i < GO_DeckPileCardsT.Length; i++)
		{
			GO_DeckPileCardsT[i].GetComponent<SpriteRenderer>().sortingLayerName = theLayer;
		}
		SetCardbacks();
	}

	private void RepaintDeckPile()
	{
		Sprite sprite = GameManager.Instance.cardBackSprites[(int)theHero.HeroData.HeroClass];
		for (int i = 0; i < GO_DeckPileCardsT.Length; i++)
		{
			GO_DeckPileCardsT[i].GetComponent<SpriteRenderer>().sprite = sprite;
		}
		SetCardbacks();
	}

	public void DrawDeckScreen(int heroTarget = -1, int type = 0, int quantity = -100)
	{
		if (coroutineDrawDeck != null)
		{
			StopCoroutine(coroutineDrawDeck);
		}
		coroutineDrawDeck = StartCoroutine(DrawDeckScreenCo(heroTarget, type, quantity));
	}

	private IEnumerator DrawDeckScreenCo(int heroTarget, int type, int quantity)
	{
		GO_List = new List<GameObject>();
		deckCardsWindow.TurnOn(type);
		List<string> CardList = new List<string>();
		if (heroTarget == -1)
		{
			heroTarget = heroActive;
		}
		if (type == 0)
		{
			if (quantity == -100)
			{
				for (int i = 0; i < HeroDeck[heroTarget].Count; i++)
				{
					CardList.Add(HeroDeck[heroTarget][i]);
				}
				CardList.Sort();
			}
			else
			{
				if (quantity > HeroDeck[heroTarget].Count)
				{
					quantity = HeroDeck[heroTarget].Count;
				}
				for (int j = 0; j < quantity; j++)
				{
					CardList.Add(HeroDeck[heroTarget][j]);
				}
			}
		}
		else
		{
			CardList = HeroDeckDiscard[heroTarget];
		}
		int numCards = CardList.Count;
		cardGos.Clear();
		for (int k = 0; k < numCards; k++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, deckCardsWindow.cardContainer);
			GO_List.Add(gameObject);
			CardItem CI = gameObject.GetComponent<CardItem>();
			gameObject.name = "TMP_" + k;
			cardGos.Add(gameObject.name, gameObject);
			CI.SetCard(CardList[k], deckScale: false, theHero);
			yield return Globals.Instance.WaitForSeconds(0.01f);
			CI.AmplifyForSelection(k, numCards);
			CI.SetDestination(CI.GetDestination() - new Vector3(2.5f, -4.5f, 0f));
			CI.DisableTrail();
			CI.HideRarityParticles();
			CI.HideCardIconParticles();
			CI.cardfordisplay = true;
			CI.discard = true;
			if (IsYourTurnForAddDiscard())
			{
				CI.ShowKeyNum(_state: true, (k + 1).ToString());
			}
			yield return null;
		}
	}

	public void DrawDeckScreenDestroy()
	{
		if (coroutineDrawDeck != null)
		{
			StopCoroutine(coroutineDrawDeck);
		}
		if (GO_List != null)
		{
			for (int num = GO_List.Count - 1; num >= 0; num--)
			{
				UnityEngine.Object.Destroy(GO_List[num]);
			}
		}
		for (int i = 0; i < deckCardsWindow.cardContainer.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(deckCardsWindow.cardContainer.transform.GetChild(i).gameObject);
		}
		deckCardsWindow.TurnOff();
	}

	private void RepositionCards()
	{
		RedrawPositionInTable();
		int num = CountHeroHand();
		float num2 = -0.69f;
		for (int i = 0; i < num; i++)
		{
			num2 = ((i >= 1) ? ((i >= 2) ? ((i >= 3) ? ((i >= 4) ? ((i >= 5) ? ((i >= 6) ? ((i >= 8) ? ((i >= 9) ? (num2 + -0.14f) : (num2 + -0.1f)) : (num2 + 0.12f)) : (num2 + 0.4f)) : (num2 + 0.7f)) : (num2 + 0.62f)) : (num2 + 0.78f)) : (num2 + 0.78f)) : (num2 + 0.58f));
		}
		handTransformPosition = new Vector3(num2, GO_Hand.transform.position.y, 0f);
		GO_Hand.transform.position = handTransformPosition;
		HandMask.localPosition = new Vector3(0f - GO_Hand.transform.position.x, 0f, HandMask.localPosition.z);
	}

	public void RedrawPositionInTable()
	{
		int count = cardItemTable.Count;
		for (int i = 0; i < count; i++)
		{
			cardItemTable[i].SetTablePositionValues(i, count);
			cardItemTable[i].PositionCardInTable();
		}
	}

	public void ClearCardsBorder()
	{
		if (heroActive <= -1)
		{
			return;
		}
		int count = cardItemTable.Count;
		if (count != 0)
		{
			for (int i = 0; i < count; i++)
			{
				cardItemTable[i].HideEnergyBorder();
			}
		}
	}

	public void RedrawCardsBorder()
	{
		if (heroActive <= -1 || WaitingForActionScreen() || cardsWaitingForReset > 0 || gameBusy || eventList.Count > 0)
		{
			return;
		}
		int count = cardItemTable.Count;
		if (count == 0)
		{
			return;
		}
		bool flag = false;
		if (!HandMask.gameObject.activeSelf)
		{
			flag = true;
		}
		for (int i = 0; i < count; i++)
		{
			if (flag)
			{
				cardItemTable[i].DrawEnergyBorder();
			}
			else
			{
				cardItemTable[i].HideEnergyBorder();
			}
		}
		if (flag)
		{
			ShowCombatKeyboardByConfig();
		}
	}

	public bool CanPlayACardRightNow()
	{
		if (heroActive > -1)
		{
			int count = cardItemTable.Count;
			for (int i = 0; i < count; i++)
			{
				if (cardItemTable[i].IsPlayableRightNow())
				{
					return true;
				}
			}
		}
		return false;
	}

	public void RedrawCardsDescriptionPrecalculated()
	{
		if (heroActive <= -1)
		{
			return;
		}
		for (int i = 0; i < CountHeroHand(); i++)
		{
			if (cardItemTable[i] != null)
			{
				cardItemTable[i].RedrawDescriptionPrecalculated(theHero);
			}
		}
	}

	public void RedrawCardsDamageType()
	{
		if (heroActive <= -1)
		{
			return;
		}
		for (int i = 0; i < CountHeroHand(); i++)
		{
			if (cardItemTable[i] != null)
			{
				cardItemTable[i].RedrawCardsDamageType(theHero);
			}
		}
	}

	public void DiscardItem(string card, Enums.CardPlace whereToDiscard = Enums.CardPlace.Discard)
	{
		switch (whereToDiscard)
		{
		case Enums.CardPlace.Discard:
			HeroDeckDiscard[heroActive].Add(card);
			DrawDiscardPileCardNumeral();
			break;
		case Enums.CardPlace.TopDeck:
			HeroDeck[heroActive].Insert(0, card);
			DrawDeckPile(CountHeroDeck() + 1);
			break;
		case Enums.CardPlace.BottomDeck:
			HeroDeck[heroActive].Add(card);
			DrawDeckPile(CountHeroDeck() + 1);
			break;
		case Enums.CardPlace.RandomDeck:
			HeroDeck[heroActive].Insert(GetRandomIntRange(0, HeroDeck[heroActive].Count, "deck"), card);
			DrawDeckPile(CountHeroDeck() + 1);
			break;
		case Enums.CardPlace.Vanish:
			HeroDeckVanish[heroActive].Add(card);
			break;
		}
	}

	public void DiscardCard(int cardPosition, Enums.CardPlace whereToDiscard = Enums.CardPlace.Discard, bool moveToDiscard = true)
	{
		if (heroActive == -1)
		{
			return;
		}
		if (cardPosition > -1 && cardPosition < HeroHand[heroActive].Count && HeroHand[heroActive][cardPosition] != null)
		{
			if (discardSound != null)
			{
				GameManager.Instance.PlayAudio(discardSound);
			}
			CardData cardData = GetCardData(HeroHand[heroActive][cardPosition]);
			if (cardData != null)
			{
				cardData.EnergyReductionTemporal = 0;
				cardData.EnergyReductionToZeroTemporal = false;
				if (moveToDiscard)
				{
					switch (whereToDiscard)
					{
					case Enums.CardPlace.Discard:
						HeroDeckDiscard[heroActive].Add(HeroHand[heroActive][cardPosition]);
						DrawDiscardPileCardNumeral();
						break;
					case Enums.CardPlace.TopDeck:
						HeroDeck[heroActive].Insert(0, HeroHand[heroActive][cardPosition]);
						DrawDeckPile(CountHeroDeck() + 1);
						break;
					case Enums.CardPlace.BottomDeck:
						HeroDeck[heroActive].Add(HeroHand[heroActive][cardPosition]);
						DrawDeckPile(CountHeroDeck() + 1);
						break;
					case Enums.CardPlace.RandomDeck:
						HeroDeck[heroActive].Insert(GetRandomIntRange(0, HeroDeck[heroActive].Count, "deck"), HeroHand[heroActive][cardPosition]);
						DrawDeckPile(CountHeroDeck() + 1);
						break;
					case Enums.CardPlace.Vanish:
						HeroDeckVanish[heroActive].Add(HeroHand[heroActive][cardPosition]);
						GlobalVanishCardsNum++;
						break;
					}
				}
			}
			HeroHand[heroActive].RemoveAt(cardPosition);
			if (cardItemTable.ElementAt(cardPosition) != null)
			{
				cardItemTable.RemoveAt(cardPosition);
			}
			if (gameStatus != "DiscardingCards")
			{
				RepositionCards();
			}
		}
		else
		{
			if (cardItemTable.ElementAt(cardPosition) != null)
			{
				cardItemTable.RemoveAt(cardPosition);
			}
			if (gameStatus != "DiscardingCards")
			{
				RepositionCards();
			}
		}
	}

	public void DeActivateDiscards()
	{
		for (int i = 0; i < GO_DiscardPile.transform.childCount - 1; i++)
		{
			GO_DiscardPile.transform.GetChild(i).GetComponent<CardItem>().enabled = false;
		}
	}

	public void SetOverDeck(bool state)
	{
		if (state && (!state || !HaveDeckEffect(CardActive)))
		{
			return;
		}
		for (int i = 0; i < TeamHero.Length; i++)
		{
			if (TeamHero[i] != null && !(TeamHero[i].HeroItem == null) && TeamHero[i].Alive)
			{
				if (state)
				{
					TeamHero[i].HeroItem.ShowOverCards();
				}
				else
				{
					TeamHero[i].HeroItem.HideOverCards();
				}
			}
		}
	}

	[PunRPC]
	private void NET_SetDamagePreview(bool theCasterIsHero, int tablePosition)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("NET_SetDamagePreview", "net");
		}
		if (cardItemTable != null)
		{
			CardData cardData = null;
			if (tablePosition > -1 && tablePosition < cardItemTable.Count && cardItemTable[tablePosition] != null)
			{
				cardData = (cardActive = cardItemTable[tablePosition].CardData);
			}
			SetDamagePreview(theCasterIsHero, cardData);
		}
	}

	public void SetDamagePreview(bool theCasterIsHero, CardData cardData = null, int tablePosition = -1)
	{
		bool flag = true;
		bool flag2 = false;
		if (cardData == null)
		{
			flag = false;
		}
		else if (cardData.Damage == 0 && cardData.SelfHealthLoss == 0 && cardData.Heal == 0 && cardData.DamageSelf == 0 && cardData.HealCurses == 0 && cardData.DispelAuras == 0 && cardData.StealAuras == 0 && cardData.HealAuraCurseSelf == null && cardData.HealAuraCurseName == null && cardData.HealAuraCurseName2 == null && cardData.HealAuraCurseName3 == null && cardData.HealAuraCurseName4 == null && cardData.Curse == null && cardData.Aura == null)
		{
			flag = false;
		}
		int energyCost = 0;
		CardItem cardItem = null;
		if (tablePosition > -1 && tablePosition < cardItemTable.Count)
		{
			energyCost = cardItemTable[tablePosition].GetEnergyCost();
		}
		if (cardItem != null)
		{
			energyCost = cardItem.GetEnergyCost();
		}
		if (!flag)
		{
			for (int i = 0; i < TeamNPC.Length; i++)
			{
				if (TeamNPC[i] != null && !(TeamNPC[i].NpcData == null) && TeamNPC[i].Alive && TeamNPC[i].NPCItem != null)
				{
					TeamNPC[i].NPCItem.SetDamagePreview(state: false);
				}
			}
			for (int j = 0; j < TeamHero.Length; j++)
			{
				if (TeamHero[j] != null && !(TeamHero[j].HeroData == null) && TeamHero[j].Alive && TeamHero[j].HeroItem != null)
				{
					TeamHero[j].HeroItem.SetDamagePreview(state: false);
				}
			}
			return;
		}
		Character character;
		if (theCasterIsHero)
		{
			character = TeamHero[heroActive];
			if (cardActive != null)
			{
				if (cardActive.SelfHealthLoss > 0)
				{
					if (character.HeroItem != null)
					{
						int dmg = cardActive.SelfHealthLoss;
						if (cardActive.SelfHealthLossSpecialValue1)
						{
							dmg = Functions.FuncRoundToInt(GetCardSpecialValue(cardActive, 1, character, null, character, null, _IsPreview: false));
						}
						else if (cardActive.SelfHealthLossSpecialValue2)
						{
							dmg = Functions.FuncRoundToInt(GetCardSpecialValue(cardActive, 2, character, null, character, null, _IsPreview: false));
						}
						character.HeroItem.SetDamagePreview(state: true, dmg, "heart");
						flag2 = true;
					}
				}
				else if (cardActive.DamageSelf > 0)
				{
					int num = cardActive.DamageSelf;
					Enums.DamageType damageType = cardActive.DamageType;
					if (num > 0)
					{
						if (cardActive.DamageSpecialValueGlobal)
						{
							num = Functions.FuncRoundToInt(GetCardSpecialValue(cardData, 0, character, null, character, null, _IsPreview: true));
						}
						if (cardActive.DamageSpecialValue1)
						{
							num = Functions.FuncRoundToInt(GetCardSpecialValue(cardData, 1, character, null, character, null, _IsPreview: true));
						}
						if (cardActive.DamageSpecialValue2)
						{
							num = Functions.FuncRoundToInt(GetCardSpecialValue(cardData, 2, character, null, character, null, _IsPreview: true));
						}
					}
					int num2 = 0;
					int num3 = 0;
					int num4 = 0;
					int num5 = 0;
					num3 = character.DamageWithCharacterBonus(num, damageType, cardData.CardClass, energyCost);
					int num6 = character.IncreasedCursedDamagePerStack(damageType);
					num3 += num6;
					if (num3 <= 0)
					{
						num3 = 0;
					}
					if (!character.IsImmune(damageType))
					{
						if (!cardActive.GetIgnoreBlock())
						{
							int num7 = character.GetBlock() - num5;
							if (num7 > 0)
							{
								if (num7 >= num3)
								{
									num5 += num3;
									num3 = 0;
								}
								else
								{
									num5 += num7;
									num3 -= num7;
								}
							}
						}
						num2 = -1 * character.BonusResists(damageType);
						num4 = Functions.FuncRoundToInt((float)num3 + (float)num3 * (float)num2 * 0.01f);
					}
					else
					{
						num4 = 0;
					}
					string text = Enum.GetName(typeof(Enums.DamageType), damageType).ToLower();
					if (text == "none")
					{
						text = "heart";
					}
					if ((num4 > 0 || num5 > 0) && character.HeroItem != null)
					{
						character.HeroItem.SetDamagePreview(state: true, num4, text, 0, "", 0, num5);
						flag2 = true;
					}
				}
				if (cardData.HealAuraCurseSelf != null && character.HeroItem != null)
				{
					character.HeroItem.SetDamagePreview(state: true, 0, "", 0, "", 0, 0, cardActive);
					flag2 = true;
				}
			}
		}
		else
		{
			character = TeamNPC[npcActive];
		}
		List<Transform> instaCastTransformList = GetInstaCastTransformList(cardActive);
		for (int k = 0; k < instaCastTransformList.Count; k++)
		{
			Character character2 = null;
			CharacterItem characterItem = null;
			if (instaCastTransformList[k].GetComponent<NPCItem>() != null && instaCastTransformList[k].GetComponent<NPCItem>().NPC != null)
			{
				characterItem = instaCastTransformList[k].GetComponent<NPCItem>();
				if (!(characterItem != null))
				{
					continue;
				}
				character2 = characterItem.NPC;
			}
			else
			{
				if (!(instaCastTransformList[k].GetComponent<HeroItem>() != null) || instaCastTransformList[k].GetComponent<HeroItem>().Hero == null)
				{
					continue;
				}
				characterItem = instaCastTransformList[k].GetComponent<HeroItem>();
				if (!(characterItem != null))
				{
					continue;
				}
				character2 = characterItem.Hero;
			}
			if (TeamHero[heroActive].HeroItem == characterItem && flag2)
			{
				continue;
			}
			int[] array = new int[2] { 99999999, 0 };
			string[] array2 = new string[2];
			int num8 = 0;
			int num9 = 0;
			Character character3 = (character.IsHero ? character : null);
			Character character4 = (character.IsHero ? null : character);
			Character character5 = (character2.IsHero ? character2 : null);
			Character targetNPC = (character2.IsHero ? null : character2);
			for (int l = 0; l < 2; l++)
			{
				int num10 = 0;
				Enums.DamageType damageType2 = Enums.DamageType.None;
				if (l == 0)
				{
					num10 = cardData.Damage;
					damageType2 = cardData.DamageType;
					if (num10 > 0)
					{
						if (cardActive.DamageSpecialValueGlobal)
						{
							num10 = Functions.FuncRoundToInt(GetCardSpecialValue(cardData, 0, character3, character4, character5, targetNPC, _IsPreview: true));
						}
						if (cardActive.DamageSpecialValue1)
						{
							num10 = Functions.FuncRoundToInt(GetCardSpecialValue(cardData, 1, character3, character4, character5, targetNPC, _IsPreview: true));
						}
						if (cardActive.DamageSpecialValue2)
						{
							num10 = Functions.FuncRoundToInt(GetCardSpecialValue(cardData, 2, character3, character4, character5, targetNPC, _IsPreview: true));
						}
					}
					if (damageType2 == cardData.DamageType2)
					{
						int num11 = 0;
						bool flag3 = false;
						if (cardActive.Damage2SpecialValueGlobal)
						{
							num11 = Functions.FuncRoundToInt(GetCardSpecialValue(cardData, 0, character3, character4, character5, targetNPC, _IsPreview: true));
							flag3 = true;
						}
						else if (cardActive.Damage2SpecialValue1)
						{
							num11 = Functions.FuncRoundToInt(GetCardSpecialValue(cardData, 1, character3, character4, character5, targetNPC, _IsPreview: true));
							flag3 = true;
						}
						else if (cardActive.Damage2SpecialValue2)
						{
							num11 = Functions.FuncRoundToInt(GetCardSpecialValue(cardData, 2, character3, character4, character5, targetNPC, _IsPreview: true));
							flag3 = true;
						}
						if (!flag3)
						{
							num11 = cardData.Damage2;
						}
						num10 += num11;
					}
				}
				else
				{
					if (cardData.DamageType == cardData.DamageType2)
					{
						continue;
					}
					num10 = cardData.Damage2;
					damageType2 = cardData.DamageType2;
					if (num10 > 0)
					{
						if (cardActive.Damage2SpecialValueGlobal)
						{
							num10 = Functions.FuncRoundToInt(GetCardSpecialValue(cardData, 0, character3, character4, character5, targetNPC, _IsPreview: true));
						}
						if (cardActive.Damage2SpecialValue1)
						{
							num10 = Functions.FuncRoundToInt(GetCardSpecialValue(cardData, 1, character3, character4, character5, targetNPC, _IsPreview: true));
						}
						if (cardActive.Damage2SpecialValue2)
						{
							num10 = Functions.FuncRoundToInt(GetCardSpecialValue(cardData, 2, character3, character4, character5, targetNPC, _IsPreview: true));
						}
					}
				}
				if (num10 <= 0 && (num10 != 0 || damageType2 == Enums.DamageType.None || cardData.DamageSelf != 0))
				{
					continue;
				}
				int num12 = 0;
				int num13 = 0;
				int num14 = 0;
				num13 = ((!theCasterIsHero) ? character.DamageWithCharacterBonus(num10, damageType2, cardData.CardClass) : character.DamageWithCharacterBonus(num10, damageType2, cardData.CardClass, energyCost));
				int num15 = character2.IncreasedCursedDamagePerStack(damageType2, cardData);
				num13 += num15;
				if (num13 <= 0)
				{
					num13 = 0;
				}
				if (!character2.IsImmune(damageType2))
				{
					if (!cardData.GetIgnoreBlock() && !cardData.GetIgnoreBlockBecausePurge())
					{
						int num16 = character2.GetBlock() - num9;
						if (num16 > 0)
						{
							if (num16 >= num13)
							{
								num9 += num13;
								num13 = 0;
							}
							else
							{
								num9 += num16;
								num13 -= num16;
							}
						}
					}
					num12 = -1 * character2.BonusResists(damageType2, "", countChargesConsumedPre: false, countChargesConsumedPost: false, cardData);
					num14 = Functions.FuncRoundToInt((float)num13 + (float)num13 * (float)num12 * 0.01f);
					num14 = IncreaseDamage(character2, character3 ?? character4, num14);
				}
				else
				{
					num14 = 0;
				}
				string text2 = Enum.GetName(typeof(Enums.DamageType), damageType2).ToLower();
				if (text2 == "none")
				{
					text2 = "heart";
				}
				array[l] = num14;
				array2[l] = text2;
			}
			character5?.Pet.IsNullOrEmpty();
			int num17 = cardActive.Heal;
			if (num17 > 0)
			{
				if (cardActive.HealSpecialValueGlobal)
				{
					num17 = Functions.FuncRoundToInt(GetCardSpecialValue(cardData, 0, character, character, null, null, _IsPreview: true));
				}
				if (cardActive.HealSpecialValue1)
				{
					num17 = Functions.FuncRoundToInt(GetCardSpecialValue(cardData, 1, character, null, character2, null, _IsPreview: true));
				}
				if (cardActive.HealSpecialValue2)
				{
					num17 = Functions.FuncRoundToInt(GetCardSpecialValue(cardData, 2, character, null, character2, null, _IsPreview: true));
				}
				num17 = character.HealWithCharacterBonus(num17, cardData.CardClass);
				num8 = character2.HealReceivedFinal(num17, isIndirect: false, cardData);
			}
			if (array[0] != 99999999 || array[1] > 0 || num8 > 0 || num9 > 0 || cardData.Aura != null || cardData.Curse != null || cardData.HealCurses > 0 || cardData.DispelAuras > 0 || cardData.StealAuras > 0 || cardData.HealAuraCurseSelf != null || cardData.HealAuraCurseName != null || cardData.HealAuraCurseName2 != null || cardData.HealAuraCurseName3 != null || cardData.HealAuraCurseName4 != null)
			{
				if (array[0] == 99999999)
				{
					array[0] = 0;
				}
				characterItem.SetDamagePreview(state: true, array[0], array2[0], array[1], array2[1], num8, num9, cardData);
			}
		}
	}

	private void DrawDiscardPileCardNumeral()
	{
		WriteDiscardCounter(CountHeroDiscard().ToString());
	}

	public Vector3 GetDeckPilePosition()
	{
		if (GO_DeckPile != null)
		{
			return GO_DeckPile.transform.localPosition;
		}
		return Vector3.zero;
	}

	public Transform GetDeckPileTransform()
	{
		if (GO_DeckPile != null)
		{
			return GO_DeckPile.transform;
		}
		return null;
	}

	public Vector3 GetDiscardPilePosition()
	{
		if (GO_DeckPile != null)
		{
			return GO_DeckPile.transform.localPosition + new Vector3(1.5f, 0f, 0f);
		}
		return Vector3.zero;
	}

	public Transform GetDiscardPileTransform()
	{
		if (GO_DiscardPile != null)
		{
			return GO_DiscardPile.transform;
		}
		return null;
	}

	public Transform GetWorldTransform()
	{
		return worldTransform;
	}

	private void RedoDiscardPileOLD()
	{
		Debug.LogError("REDODISCARDPILE!!!!");
		WriteDeckCounter();
		WriteDiscardCounter();
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("RedoDiscardPile", "general");
		}
		for (int num = GO_DiscardPile.transform.childCount - 1; num >= 0; num--)
		{
			UnityEngine.Object.Destroy(GO_DiscardPile.transform.GetChild(num).gameObject);
		}
		_ = GetDiscardPilePosition() + GO_DecksObject.transform.position;
		for (int i = 0; i < CountHeroDiscard(); i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, GO_DiscardPile.transform);
			gameObject.transform.localPosition = GetDiscardPilePosition();
			CardItem component = gameObject.GetComponent<CardItem>();
			string id = (gameObject.name = HeroDeckDiscard[heroActive][i]);
			component.SetCard(id, deckScale: true, theHero);
			if (i < CountHeroDiscard() - 1)
			{
				component.ShowDiscardImage();
			}
			component.SetDiscardSortingOrder(i);
			component.DisableCollider();
			component.active = false;
			component.enabled = false;
			component.HideRarityParticles();
			component.HideCardIconParticles();
			gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			if (i < CountHeroDiscard() - 1)
			{
				gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, -100f);
			}
		}
	}

	private void RedoDiscardPile()
	{
		WriteDeckCounter();
		WriteDiscardCounter();
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("RedoDiscardPile", "general");
		}
		_ = GetDiscardPilePosition() + GO_DecksObject.transform.position;
		DestroyDiscardPileChilds();
		for (int i = 0; i < CountHeroDiscard(); i++)
		{
			if (i >= CountHeroDiscard() - 1)
			{
				GameObject obj = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, GO_DiscardPile.transform);
				obj.transform.localPosition = GetDiscardPilePosition();
				CardItem component = obj.GetComponent<CardItem>();
				string id = (obj.name = HeroDeckDiscard[heroActive][i]);
				component.SetCard(id, deckScale: true, theHero);
				if (i < CountHeroDiscard() - 1)
				{
					component.ShowDiscardImage();
				}
				component.SetDiscardSortingOrder(i);
				component.DisableCollider();
				component.active = false;
				component.enabled = false;
				component.HideRarityParticles();
				component.HideCardIconParticles();
				obj.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			}
		}
	}

	public void RedoDiscardPileDepth()
	{
		int childCount = GO_DiscardPile.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			if (i < childCount - 2)
			{
				Transform child = GO_DiscardPile.transform.GetChild(i);
				child.localPosition = new Vector3(child.localPosition.x, child.localPosition.y, -100f);
			}
		}
	}

	public int GetNPCActive()
	{
		return npcActive;
	}

	public int GetHeroActive()
	{
		return heroActive;
	}

	public Hero GetHeroHeroActive()
	{
		return theHero;
	}

	public Hero GetHero(int _order)
	{
		if (_order > -1 && _order < 4 && TeamHero[_order] != null)
		{
			return TeamHero[_order];
		}
		return null;
	}

	public Character GetCharacterActive()
	{
		if (heroActive > -1 && TeamHero[heroActive] != null && TeamHero[heroActive].Alive)
		{
			return TeamHero[heroActive];
		}
		if (npcActive > -1 && TeamNPC[npcActive] != null && TeamNPC[npcActive].Alive)
		{
			return TeamNPC[npcActive];
		}
		return null;
	}

	public List<Hero> GetHeroSides(int position)
	{
		List<Hero> list = new List<Hero>();
		for (int i = 0; i < TeamHero.Length; i++)
		{
			if (TeamHero[i] != null && !(TeamHero[i].HeroData == null) && TeamHero[i].Alive && (TeamHero[i].Position == position - 1 || TeamHero[i].Position == position + 1))
			{
				list.Add(TeamHero[i]);
			}
		}
		return list;
	}

	public List<NPC> GetNPCSides(int position)
	{
		List<NPC> list = new List<NPC>();
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("get npc sides pos->" + position);
		}
		NPC nPC = null;
		NPC nPC2 = null;
		int num = -1;
		int num2 = 5;
		for (int i = 0; i < 4; i++)
		{
			if (TeamNPC[i] != null && TeamNPC[i].Alive && TeamNPC[i].Position < position && TeamNPC[i].Position > num)
			{
				nPC = TeamNPC[i];
				num = TeamNPC[i].Position;
			}
		}
		for (int j = 0; j < 4; j++)
		{
			if (TeamNPC[j] != null && TeamNPC[j].Alive && TeamNPC[j].Position > position && TeamNPC[j].Position < num2)
			{
				nPC2 = TeamNPC[j];
				num2 = TeamNPC[j].Position;
			}
		}
		if (nPC != null)
		{
			list.Add(nPC);
		}
		if (nPC2 != null)
		{
			list.Add(nPC2);
		}
		return list;
	}

	public void NPCCastCardList(string id, string npcInternalId = "")
	{
		if (npcInternalId == "")
		{
			npcInternalId = theNPC.InternalId;
		}
		if (!(npcInternalId == ""))
		{
			if (!npcCardsCasted.ContainsKey(npcInternalId))
			{
				npcCardsCasted[npcInternalId] = new List<string>();
			}
			StringBuilder stringBuilder = new StringBuilder(id);
			while (npcCardsCasted[npcInternalId].Contains(stringBuilder.ToString()))
			{
				stringBuilder.Append("0");
			}
			npcCardsCasted[npcInternalId].Add(stringBuilder.ToString());
		}
	}

	public List<string> GetNPCCardsCastedList(string id)
	{
		if (npcCardsCasted.ContainsKey(id))
		{
			return npcCardsCasted[id];
		}
		return new List<string>();
	}

	public int NPCCastCardTimes(string id)
	{
		if (!npcCardsCasted.ContainsKey(theNPC.InternalId))
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < npcCardsCasted[theNPC.InternalId].Count - 1; i++)
		{
			if (npcCardsCasted[theNPC.InternalId][i] == id)
			{
				num++;
			}
		}
		return num;
	}

	public void ConsumeAuraCurse(string whenToConsume, Character character, string auraToConsume = "")
	{
		Functions.DebugLogGD("[CONSUMEAURACURSE] " + character?.ToString() + " " + whenToConsume + " " + auraToConsume, "trace");
		if (character != null)
		{
			string key = character.Id + whenToConsume + auraToConsume;
			if (character.HeroItem != null || character.NPCItem != null)
			{
				if (dictCoroutineConsume.ContainsKey(key))
				{
					if (dictCoroutineConsume[key] != null)
					{
						StopCoroutine(dictCoroutineConsume[key]);
					}
					dictCoroutineConsume.Remove(key);
				}
				Coroutine value = StartCoroutine(ConsumeAuraCurseCo(whenToConsume, character, auraToConsume));
				dictCoroutineConsume.Add(key, value);
			}
			else
			{
				waitExecution = false;
			}
		}
		else
		{
			Debug.LogError("[CONSUMEAURACURSE] Break because character is null");
		}
	}

	private IEnumerator ConsumeAuraCurseCo(string whenToConsume, Character character, string auraToConsume = "")
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[CONSUMEAURACURSE] when " + whenToConsume + " aura " + auraToConsume, "trace");
		}
		if (character == null)
		{
			Debug.LogError("[CONSUMEAURACURSE] Break because character is NULL");
			waitExecution = false;
			yield break;
		}
		int count = character.AuraList.Count;
		bool isAlive = true;
		int totalconsumed = 0;
		if (count > 0)
		{
			List<Aura> AuraToIncludeList = new List<Aura>();
			bool consumeBlock = true;
			for (int i = 0; i < count; i++)
			{
				if (i < character.AuraList.Count && character.AuraList[i] != null && character.AuraList[i].ACData != null && character.AuraList[i].ACData.NoRemoveBlockAtTurnEnd)
				{
					consumeBlock = false;
				}
			}
			List<string> auraConsumed = new List<string>();
			int totalCharAura = character.AuraList.Count;
			bool value3 = default(bool);
			for (int j = 0; j < totalCharAura; j++)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("consume " + j + ". " + character.AuraList[j].ACData.Id, "trace");
				}
				if (j >= character.AuraList.Count || character.AuraList[j] == null || character.AuraList[j].ACData == null || auraConsumed.Contains(character.AuraList[j].ACData.Id) || character.AuraList[j].AuraCharges <= 0)
				{
					continue;
				}
				auraConsumed.Add(character.AuraList[j].ACData.Id);
				if (!character.Alive)
				{
					waitExecution = false;
					yield break;
				}
				bool flag = false;
				AuraCurseData AC = AtOManager.Instance.GlobalAuraCurseModificationByTraitsAndItems("consume", character.AuraList[j].ACData.Id, character, null);
				if (AC == null)
				{
					AC = character.AuraList[j].ACData;
				}
				switch (whenToConsume)
				{
				case "Now":
					if (AC.Id == auraToConsume.ToLower())
					{
						flag = true;
					}
					break;
				case "BeginRound":
					if (AC.ConsumedAtRoundBegin)
					{
						flag = true;
					}
					break;
				case "BeginTurn":
					if (AC.ConsumedAtTurnBegin)
					{
						flag = true;
					}
					if (AC.SkipEndTurnRemovalIfNoBegin)
					{
						if (!auraStates.TryGetValue(AC.Id, out var value))
						{
							value = new Dictionary<Character, bool>();
							auraStates[AC.Id] = value;
						}
						value[character] = true;
					}
					break;
				case "EndTurn":
					if (AC.ConsumedAtTurn)
					{
						flag = !AC.SkipEndTurnRemovalIfNoBegin || (auraStates.TryGetValue(AC.Id, out var value2) && value2.TryGetValue(character, out value3) && value3);
					}
					break;
				case "EndRound":
					if (AC.ConsumedAtRound)
					{
						flag = true;
						if (AC.Id == "block" && !consumeBlock)
						{
							flag = false;
						}
					}
					break;
				}
				if (!flag)
				{
					continue;
				}
				if ((AC.ProduceDamageWhenConsumed || AC.ProduceHealWhenConsumed) && !(whenToConsume == "EndRound") && !(whenToConsume == "BeginRound"))
				{
					if (GameManager.Instance.IsMultiplayer())
					{
						yield return Globals.Instance.WaitForSeconds(0.35f);
					}
					else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Slow)
					{
						yield return Globals.Instance.WaitForSeconds(0.4f);
					}
					else if (GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Ultrafast)
					{
						yield return Globals.Instance.WaitForSeconds(0.1f);
					}
					else
					{
						yield return Globals.Instance.WaitForSeconds(0.3f);
					}
				}
				totalconsumed++;
				int actCharges = 0;
				if (character.AuraList[j] != null)
				{
					actCharges = character.AuraList[j].AuraCharges;
				}
				if (AC.EffectTick != "" && (AC.ProduceDamageWhenConsumed || AC.ProduceHealWhenConsumed))
				{
					if (character.NPCItem != null)
					{
						EffectsManager.Instance.PlayEffectAC(AC.EffectTick, isHero: false, character.NPCItem.CharImageT, flip: false);
					}
					else if (character.HeroItem != null)
					{
						EffectsManager.Instance.PlayEffectAC(AC.EffectTick, isHero: true, character.HeroItem.CharImageT, flip: false);
					}
				}
				if (AC.ProduceDamageWhenConsumed)
				{
					int num = 0;
					num += AC.DamageWhenConsumed;
					int num2 = actCharges;
					if (AC.ConsumedDamageChargesBasedOnACCharges != null)
					{
						num2 = character.GetAuraCharges(AC.ConsumedDamageChargesBasedOnACCharges.Id);
					}
					if (AC.ConsumeDamageChargesIfACApplied != null && character.GetAuraCharges(AC.ConsumeDamageChargesIfACApplied.Id) <= 0)
					{
						num2 = 0;
					}
					num += Functions.FuncRoundToInt(AC.DamageWhenConsumedPerCharge * (float)num2);
					if (!character.IsHero && AC.Id == "scourge")
					{
						if (AtOManager.Instance.TeamHavePerk("mainperkscourge0b"))
						{
							num += character.GetAuraCharges("burn");
						}
						if (AtOManager.Instance.TeamHavePerk("mainperkscourge0c"))
						{
							num += character.GetAuraCharges("insane");
						}
					}
					if (AC.DamageSidesWhenConsumed > 0 || AC.DamageSidesWhenConsumedPerCharge > 0)
					{
						int num3 = 0;
						num3 += AC.DamageSidesWhenConsumed;
						num3 += AC.DamageSidesWhenConsumedPerCharge * actCharges;
						AudioClip sound = null;
						if (AC.GetSound() != null)
						{
							sound = AC.GetSound();
						}
						if (character.HeroItem != null)
						{
							List<Hero> heroSides = GetHeroSides(character.Position);
							for (int k = 0; k < heroSides.Count; k++)
							{
								if (heroSides[k] != null)
								{
									if (AC.DoubleDamageIfCursesLessThan > 0 && heroSides[k].GetAuraCurseTotal(_auras: false, _curses: true) < AC.DoubleDamageIfCursesLessThan)
									{
										num3 *= 2;
									}
									heroSides[k].IndirectDamage(AC.DamageTypeWhenConsumed, num3, character, sound, AC.Id);
									if (AC.EffectTickSides != "" && heroSides[k].HeroItem != null)
									{
										EffectsManager.Instance.PlayEffectAC(AC.EffectTickSides, isHero: true, heroSides[k].HeroItem.CharImageT, flip: false, 0.2f);
									}
								}
							}
						}
						else if (character.NPCItem != null)
						{
							List<NPC> nPCSides = GetNPCSides(character.Position);
							for (int l = 0; l < nPCSides.Count; l++)
							{
								if (nPCSides[l] != null)
								{
									if (AC.DoubleDamageIfCursesLessThan > 0 && nPCSides[l].GetAuraCurseTotal(_auras: false, _curses: true) < AC.DoubleDamageIfCursesLessThan)
									{
										num3 *= 2;
									}
									nPCSides[l].IndirectDamage(AC.DamageTypeWhenConsumed, num3, character, sound, AC.Id);
									if (AC.EffectTickSides != "" && nPCSides[l].NPCItem != null)
									{
										EffectsManager.Instance.PlayEffectAC(AC.EffectTickSides, isHero: false, nPCSides[l].NPCItem.CharImageT, flip: false, 0.2f);
									}
								}
							}
						}
					}
					if (num > 0)
					{
						if (AC.DoubleDamageIfCursesLessThan > 0 && character.GetAuraCurseTotal(_auras: false, _curses: true) < AC.DoubleDamageIfCursesLessThan)
						{
							num *= 2;
						}
						AudioClip sound2 = null;
						if (AC.GetSound() != null)
						{
							sound2 = AC.GetSound();
						}
						if (AtOManager.Instance.TeamHavePerk("mainperkdark2d") && AC.Id == "dark")
						{
							character.SetEvent(Enums.EventActivation.CurseExploded, character, num, AC.Id);
						}
						character.IndirectDamage(AC.DamageTypeWhenConsumed, num, character, sound2, AC.Id);
						yield return Globals.Instance.WaitForSeconds(0.1f);
						if (character.HpCurrent <= 0 || !character.Alive)
						{
							waitExecution = false;
							yield break;
						}
					}
				}
				if (AC.ProduceHealWhenConsumed)
				{
					int num4 = 0;
					num4 += AC.HealWhenConsumed;
					num4 += Functions.FuncRoundToInt((float)actCharges * AC.HealWhenConsumedPerCharge);
					if (num4 > 0 && character.GetHpLeftForMax() > 0)
					{
						AudioClip sound3 = null;
						if (AC.GetSound() != null)
						{
							sound3 = AC.GetSound();
						}
						character.IndirectHeal(num4, sound3, AC.Id);
					}
					if (AC.HealSidesWhenConsumed != 0 || AC.HealSidesWhenConsumedPerCharge != 0f)
					{
						num4 = 0;
						num4 += AC.HealSidesWhenConsumed;
						num4 += Functions.FuncRoundToInt((float)actCharges * AC.HealSidesWhenConsumedPerCharge);
						if (num4 > 0)
						{
							if (character.HeroItem != null)
							{
								List<Hero> heroSides2 = GetHeroSides(character.Position);
								for (int m = 0; m < heroSides2.Count; m++)
								{
									if (heroSides2[m].GetHpLeftForMax() > 0)
									{
										AudioClip sound4 = null;
										if (AC.GetSound() != null)
										{
											sound4 = AC.GetSound();
										}
										heroSides2[m].IndirectHeal(num4, sound4, AC.Id);
									}
								}
							}
							else if (character.NPCItem != null)
							{
								List<NPC> nPCSides2 = GetNPCSides(character.Position);
								for (int n = 0; n < nPCSides2.Count; n++)
								{
									if (nPCSides2[n].GetHpLeftForMax() > 0)
									{
										AudioClip sound5 = null;
										if (AC.GetSound() != null)
										{
											sound5 = AC.GetSound();
										}
										nPCSides2[n].IndirectHeal(num4, sound5, AC.Id);
									}
								}
							}
						}
					}
				}
				if (AC.GainAuraCurseConsumption != null)
				{
					int num5 = actCharges * AC.GainAuraCurseConsumptionPerCharge;
					if (AC.GainChargesFromThisAuraCurse != null)
					{
						num5 += character.EffectCharges(AC.GainChargesFromThisAuraCurse.Id);
						if (AC.GainChargesFromThisAuraCurse.Id == "fury" && character.HaveTrait("bloody"))
						{
							num5 = Functions.FuncRoundToInt((float)num5 * 0.5f);
						}
					}
					if (num5 > 0)
					{
						Aura aura = new Aura();
						aura.SetAura(AtOManager.Instance.GlobalAuraCurseModificationByTraitsAndItems("set", AC.GainAuraCurseConsumption.Id, null, character), num5);
						AuraToIncludeList.Add(aura);
					}
				}
				if (AC.GainAuraCurseConsumption2 != null)
				{
					int num6 = actCharges * AC.GainAuraCurseConsumptionPerCharge2;
					if (AC.GainChargesFromThisAuraCurse2 != null)
					{
						num6 += character.EffectCharges(AC.GainChargesFromThisAuraCurse2.Id);
						if (AC.GainChargesFromThisAuraCurse2.Id == "fury" && character.HaveTrait("bloody"))
						{
							num6 = Functions.FuncRoundToInt((float)num6 * 0.5f);
						}
					}
					if (num6 > 0)
					{
						Aura aura = new Aura();
						aura.SetAura(AtOManager.Instance.GlobalAuraCurseModificationByTraitsAndItems("set", AC.GainAuraCurseConsumption2.Id, null, character), num6);
						AuraToIncludeList.Add(aura);
					}
				}
				if (j < character.AuraList.Count && character.AuraList[j] != null && AC.Id == character.AuraList[j].ACData.Id)
				{
					if (AC.ConsumeAll)
					{
						character.AuraList[j].AuraCharges = 0;
						ConsumeStatusForCombatStats(character.Id, auraToConsume, -1);
					}
					else if (AC.AuraConsumed > 0)
					{
						character.AuraList[j].AuraCharges -= AC.AuraConsumed;
						ConsumeStatusForCombatStats(character.Id, AC.Id, AC.AuraConsumed);
					}
				}
				if (!(AC.Id == "scourge") || character.GetAuraCharges("dark") <= 0)
				{
					continue;
				}
				float num7 = 0.5f;
				if (character.HeroItem != null)
				{
					List<Hero> heroSides3 = GetHeroSides(character.Position);
					for (int num8 = 0; num8 < heroSides3.Count; num8++)
					{
						heroSides3[num8].SetAura(null, Globals.Instance.GetAuraCurseData("dark"), Functions.FuncRoundToInt((float)character.GetAuraCharges("dark") * num7));
					}
				}
				else
				{
					if (AtOManager.Instance.TeamHaveTrait("unholyblight"))
					{
						num7 = 1f;
					}
					List<NPC> nPCSides3 = GetNPCSides(character.Position);
					for (int num9 = 0; num9 < nPCSides3.Count; num9++)
					{
						nPCSides3[num9].SetAura(null, Globals.Instance.GetAuraCurseData("dark"), Functions.FuncRoundToInt((float)character.GetAuraCharges("dark") * num7));
					}
				}
				if (AC.GetSound() != null)
				{
					GameManager.Instance.PlayAudio(AC.GetSound(), 0.5f);
				}
			}
			if (isAlive)
			{
				for (int num10 = character.AuraList.Count - 1; num10 >= 0; num10--)
				{
					if (character.AuraList[num10] != null && character.AuraList[num10].ACData != null && character.AuraList[num10].AuraCharges <= 0)
					{
						if (character.AuraList[num10].ACData.DieWhenConsumedAll)
						{
							character.ModifyHp(-character.GetHp());
							break;
						}
						character.AuraList.RemoveAt(num10);
					}
				}
			}
			if (AuraToIncludeList.Count > 0)
			{
				for (int num11 = 0; num11 < AuraToIncludeList.Count; num11++)
				{
					if (AuraToIncludeList[num11] != null)
					{
						character.SetAura(character, AuraToIncludeList[num11].ACData, AuraToIncludeList[num11].AuraCharges);
					}
				}
			}
		}
		yield return Globals.Instance.WaitForSeconds(0.02f);
		character.UpdateAuraCurseFunctions();
		waitExecution = false;
	}

	private float GetCardSpecialValue(CardData _cardData, int _index, Character _casterHero, Character _casterNPC, Character _targetHero, Character _targetNPC, bool _IsPreview)
	{
		float num = 0f;
		Character character = null;
		Character character2 = null;
		character = ((_casterHero == null) ? _casterNPC : _casterHero);
		character2 = ((_targetHero == null) ? _targetNPC : _targetHero);
		if ((character == null || character2 == null) && _index != 0)
		{
			return 0f;
		}
		string aCName = "";
		Enums.CardSpecialValue cardSpecialValue;
		switch (_index)
		{
		case 0:
			cardSpecialValue = _cardData.SpecialValueGlobal;
			if (_cardData.SpecialAuraCurseNameGlobal != null)
			{
				aCName = _cardData.SpecialAuraCurseNameGlobal.Id;
			}
			break;
		case 1:
			cardSpecialValue = _cardData.SpecialValue1;
			if (_cardData.SpecialAuraCurseName1 != null)
			{
				aCName = _cardData.SpecialAuraCurseName1.Id;
			}
			break;
		default:
			cardSpecialValue = _cardData.SpecialValue2;
			if (_cardData.SpecialAuraCurseName2 != null)
			{
				aCName = _cardData.SpecialAuraCurseName2.Id;
			}
			break;
		}
		switch (cardSpecialValue)
		{
		case Enums.CardSpecialValue.AuraCurseYours:
			if (character == null)
			{
				return 0f;
			}
			num = character.GetAuraCharges(aCName);
			break;
		case Enums.CardSpecialValue.AuraCurseTarget:
			if (character2 == null)
			{
				return 0f;
			}
			num = character2.GetAuraCharges(aCName);
			break;
		case Enums.CardSpecialValue.CardsHand:
			num = ((!_IsPreview) ? ((float)handCardsBeforeCast) : ((float)CountHeroHand()));
			break;
		case Enums.CardSpecialValue.CardsDeck:
			num = ((!_IsPreview) ? ((float)deckCardsBeforeCast) : ((float)CountHeroDeck()));
			break;
		case Enums.CardSpecialValue.CardsDiscard:
			num = ((!_IsPreview) ? ((float)discardCardsBeforeCast) : ((float)CountHeroDiscard()));
			break;
		case Enums.CardSpecialValue.CardsVanish:
			num = ((!_IsPreview) ? ((float)vanishCardsBeforeCast) : ((float)CountHeroVanish()));
			break;
		case Enums.CardSpecialValue.CardsDeckTarget:
			num = ((_casterHero != _targetHero) ? ((float)CountHeroDeck(_targetHero.HeroIndex)) : ((!_IsPreview) ? ((float)deckCardsBeforeCast) : ((float)CountHeroDeck())));
			break;
		case Enums.CardSpecialValue.CardsDiscardTarget:
			num = ((_casterHero != _targetHero) ? ((float)CountHeroDiscard(_targetHero.HeroIndex)) : ((!_IsPreview) ? ((float)discardCardsBeforeCast) : ((float)CountHeroDiscard())));
			break;
		case Enums.CardSpecialValue.CardsVanishTarget:
			num = ((_casterHero != _targetHero) ? ((float)CountHeroVanish(_targetHero.HeroIndex)) : ((!_IsPreview) ? ((float)vanishCardsBeforeCast) : ((float)CountHeroVanish())));
			break;
		case Enums.CardSpecialValue.HealthYours:
			num = character.GetHp();
			break;
		case Enums.CardSpecialValue.HealthTarget:
			num = character2.GetHp();
			break;
		case Enums.CardSpecialValue.SpeedYours:
			num = character.GetSpeed()[0];
			break;
		case Enums.CardSpecialValue.SpeedTarget:
			num = character2.GetSpeed()[0];
			break;
		case Enums.CardSpecialValue.SpeedDifference:
			num = Mathf.Abs(character.GetSpeed()[0] - character2.GetSpeed()[0]);
			break;
		case Enums.CardSpecialValue.MissingHealthYours:
			num = character.GetHpLeftForMax();
			break;
		case Enums.CardSpecialValue.MissingHealthTarget:
			num = character2.GetHpLeftForMax();
			break;
		case Enums.CardSpecialValue.DiscardedCards:
			num = GlobalDiscardCardsNum;
			break;
		case Enums.CardSpecialValue.VanishedCards:
			num = GlobalVanishCardsNum;
			break;
		}
		return _index switch
		{
			0 => num * _cardData.SpecialValueModifierGlobal * 0.01f, 
			1 => num * _cardData.SpecialValueModifier1 * 0.01f, 
			_ => num * _cardData.SpecialValueModifier2 * 0.01f, 
		};
	}

	public void ShowDeathScreen(Hero _hero)
	{
		waitingDeathScreen = true;
		if (TomeManager.Instance.IsActive())
		{
			TomeManager.Instance.ShowTome(_status: false);
		}
		if (CardScreenManager.Instance.IsActive())
		{
			CardScreenManager.Instance.ShowCardScreen(_state: false);
		}
		if (console.IsActive())
		{
			console.Show(_state: false);
		}
		characterWindow.Hide();
		HideCharacterWindow();
		PlayerManager.Instance.CardUnlock("Deathsdoor");
		if (!deathScreen.IsActive())
		{
			deathScreen.SetCharacter(_hero);
			deathScreen.SetCard("DeathsDoor");
			if (!GameManager.Instance.IsMultiplayer() || _hero.Owner == NetworkManager.Instance.GetPlayerNick())
			{
				deathScreen.TurnOn();
			}
			else
			{
				ShowHandMask(state: false);
				deathScreen.TurnOn(showButton: false);
			}
		}
		if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
		{
			if (coroutineDeathScreen != null)
			{
				StopCoroutine(coroutineDeathScreen);
			}
			coroutineDeathScreen = StartCoroutine(DeathScreenOffCo());
		}
	}

	private IEnumerator DeathScreenOffCo()
	{
		yield return Globals.Instance.WaitForSeconds(30f);
		DeathScreenOff();
	}

	public void DeathScreenOff()
	{
		if (coroutineDeathScreen != null)
		{
			StopCoroutine(coroutineDeathScreen);
		}
		if (waitingDeathScreen)
		{
			if (GameManager.Instance.IsMultiplayer())
			{
				photonView.RPC("NET_HideDeathScreen", RpcTarget.Others);
			}
			deathScreen.TurnOff();
		}
	}

	[PunRPC]
	private void NET_HideDeathScreen()
	{
		deathScreen.TurnOff();
	}

	public void KillHero(Hero _hero)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[KillHero]");
		}
		if (_hero == null || !(_hero.HeroItem != null))
		{
			return;
		}
		characterKilled = true;
		_hero.DestroyCharacter();
		if (NumHeroesAlive() > 0)
		{
			ReDoInitiatives(_hero);
			RepositionCharacters();
			if (theHero == _hero)
			{
				waitExecution = false;
				if (IsYourTurn())
				{
					EndTurn();
				}
			}
		}
		else
		{
			FinishCombat();
		}
	}

	public void KillNPC(NPC _npc)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[KillNPCCo]");
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[NPCS live->" + NumNPCsAlive() + "]");
		}
		if (_npc != null && _npc.NPCItem != null && _npc.NpcData != null)
		{
			characterKilled = true;
			if (CanDestroyIllusion(_npc))
			{
				DestroyIllusion(_npc);
			}
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
			{
				if (cardActive != null && cardActive.Id.Split('_')[0].ToLower() == "escapegold")
				{
					scarabSuccess = "0";
				}
				else
				{
					scarabSuccess = "1";
				}
			}
			else if (_npc.NpcData.Id == "jadescarab" || _npc.NpcData.Id == "jadescarab_b" || _npc.NpcData.Id == "jadescarab_plus" || _npc.NpcData.Id == "jadescarab_plus_b")
			{
				if (cardActive != null && cardActive.Id.Split('_')[0].ToLower() == "escapejade")
				{
					scarabSuccess = "0";
				}
				else
				{
					scarabSuccess = "1";
				}
			}
			else if ((_npc.NpcData.Id == "crystalscarab" || _npc.NpcData.Id == "crystalscarab_b" || _npc.NpcData.Id == "crystalscarab_plus" || _npc.NpcData.Id == "crystalscarab_plus_b") && cardActive != null && cardActive.Id.Split('_')[0].ToLower() == "escapecrystal")
			{
				scarabSuccess = "0";
			}
			if (NumNPCsAlive() > 0)
			{
				ReDoInitiatives(_npc);
				RepositionCharacters();
				if (_npc.NpcData.FinishCombatOnDead)
				{
					for (int i = 0; i < TeamNPC.Length; i++)
					{
						if (TeamNPC[i] != null && !(TeamNPC[i].NpcData == null) && TeamNPC[i].Alive)
						{
							TeamNPC[i].DestroyCharacter();
						}
					}
					FinishCombat();
				}
				else if (theNPC == _npc)
				{
					waitExecution = false;
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("-------------------------------> " + gameStatus, "trace");
					}
					if (gameStatus != "EndTurn" && gameStatus != "NextTurn")
					{
						EndTurn();
					}
				}
			}
			else if (!AnyNPCAlive())
			{
				FinishCombat();
			}
		}
		else
		{
			Debug.LogError("KILL NPC error because there's no NPC");
		}
	}

	private void DestroyIllusion(Character currentCharacter)
	{
		if (currentCharacter.IllusionCharacter is Hero hero)
		{
			KillHero(hero);
		}
		else if (currentCharacter.IllusionCharacter is NPC npc)
		{
			KillNPC(npc);
		}
		currentCharacter.IllusionCharacter.Alive = false;
		currentCharacter.IllusionCharacter.HpCurrent = 0;
		currentCharacter.IllusionCharacter.NpcData = null;
		currentCharacter.IllusionCharacter = null;
	}

	private static bool CanDestroyIllusion(Character currentCharacter)
	{
		if (currentCharacter.IllusionCharacter == null || !currentCharacter.IllusionCharacter.Alive)
		{
			return false;
		}
		return true;
	}

	public void RepositionCharacters()
	{
		int num = 0;
		for (int i = 0; i < TeamHero.Length; i++)
		{
			if (TeamHero[i] != null && !(TeamHero[i].HeroData == null) && TeamHero[i].Alive)
			{
				TeamHero[i].Position = num;
				num++;
			}
		}
		num = 0;
		for (int j = 0; j < TeamNPC.Length; j++)
		{
			if (TeamNPC[j] != null && !(TeamNPC[j].NpcData == null) && TeamNPC[j].Alive)
			{
				TeamNPC[j].Position = num;
				num++;
			}
		}
	}

	public Vector3 CharPosition(bool isHero, int position, int totalchars)
	{
		float num = 1.6f;
		float y = 0f;
		float z = 1f - (float)position * 0.1f;
		float num2 = 1.7f;
		float x = ((!isHero) ? (num + (float)position * num2) : (0f - num - (float)position * num2));
		return new Vector3(x, y, z);
	}

	public bool AnyHeroAlive()
	{
		for (int i = 0; i < TeamHero.Length; i++)
		{
			if (TeamHero[i] != null && !(TeamHero[i].HeroData == null) && TeamHero[i].Alive)
			{
				if (TeamHero[i].HpCurrent > 0)
				{
					return true;
				}
				TeamHero[i].Alive = false;
				TeamHero[i].UpdateAuraCurseFunctions();
			}
		}
		return false;
	}

	private int NumHeroesAlive()
	{
		int num = 0;
		for (int i = 0; i < TeamHero.Length; i++)
		{
			if (TeamHero[i] != null && !(TeamHero[i].HeroData == null) && TeamHero[i].Alive)
			{
				num++;
			}
		}
		return num;
	}

	public bool AnyNPCAlive()
	{
		for (int i = 0; i < TeamNPC.Length; i++)
		{
			if (TeamNPC[i] != null && !(TeamNPC[i].NpcData == null) && TeamNPC[i].Alive)
			{
				if (TeamNPC[i].HpCurrent > 0)
				{
					return true;
				}
				TeamNPC[i].Alive = false;
				TeamNPC[i].UpdateAuraCurseFunctions();
			}
		}
		if (BossNpc != null && BossNpc.npc != null && BossNpc.npc.HpCurrent > 0)
		{
			return true;
		}
		return false;
	}

	public int NumNPCsAlive()
	{
		int num = 0;
		for (int i = 0; i < TeamNPC.Length; i++)
		{
			if (TeamNPC[i] != null && !(TeamNPC[i].NpcData == null) && TeamNPC[i].Alive)
			{
				num++;
			}
		}
		return num;
	}

	public bool AnyNPCIsParalyzed()
	{
		NPC[] teamNPC = GetTeamNPC();
		foreach (NPC nPC in teamNPC)
		{
			if (nPC != null && nPC.Alive && nPC.IsParalyzed())
			{
				return true;
			}
		}
		return false;
	}

	public bool CheckMatchIsOver()
	{
		if (!AnyHeroAlive() || !AnyNPCAlive())
		{
			return true;
		}
		return false;
	}

	public int GetCurrentRound()
	{
		return currentRound;
	}

	public void FinishCombat()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("----- FINISH COMBAT -- " + MatchIsOver + " ---");
		}
		if (!MatchIsOver)
		{
			MatchIsOver = true;
			RedrawCardsBorder();
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("FinishCombat");
			}
			AtOManager.Instance.combatCardDictionary = null;
			AtOManager.Instance.combatGameCode = "";
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("GameBusy TRUE", "gamebusy");
			}
			SetGameBusy(state: true);
			waitingKill = false;
			gameStatus = "FinishCombat";
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD(gameStatus, "gamestatus");
			}
			StartCoroutine(FinishCombatCo());
			AtOManager.Instance.SirenQueenBattle = false;
		}
		mindSpikeAbility.Reset();
	}

	private IEnumerator GoToMainMenu()
	{
		yield return Globals.Instance.WaitForSeconds(3f);
		AlertManager.Instance.HideAlert();
		OptionsManager.Instance.DoExit();
	}

	private IEnumerator FinishCombatCo()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("----- FINISH COMBAT Co -- " + MatchIsOver + " ---");
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("matchIsOver OK");
		}
		if (resignCombat)
		{
			newTurnScript.FinishCombat(won: false);
		}
		else
		{
			yield return Globals.Instance.WaitForSeconds(0.1f);
			while (waitingDeathScreen)
			{
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
			if (CheckForCrack())
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(Texts.Instance.GetText("crackedVersion"));
				AlertManager.Instance.AlertConfirm(stringBuilder.ToString());
				AlertManager.Instance.ShowDoorIcon();
				StartCoroutine(GoToMainMenu());
				yield break;
			}
			if (!AnyHeroAlive())
			{
				if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
				{
					if (GameManager.Instance.ConfigRestartCombatOption)
					{
						characterWindow.Hide();
						HideCharacterWindow();
						AlertManager.Instance.AlertConfirmDouble(Texts.Instance.GetText("combatWantToRetry"));
						AlertManager.buttonClickDelegate = WantToRetry;
						AlertManager.Instance.ShowReloadIcon();
						AlertManager.Instance.SetRestartPosition();
						ShowMask(state: true);
					}
					else if (GameManager.Instance.IsMultiplayer())
					{
						photonView.RPC("NET_FinishCombat2", RpcTarget.All);
					}
					else
					{
						NET_FinishCombat2();
					}
				}
				else
				{
					AlertManager.Instance.AlertConfirmDouble(Texts.Instance.GetText("combatWantToRetry") + "<size=-6><br><color=#F1D2A9>" + Texts.Instance.GetText("combatWantToRetryClient"), "", "", showButtons: false);
					AlertManager.Instance.SetRestartPosition();
					ShowMask(state: true);
				}
				yield break;
			}
		}
		StartCoroutine(FinishCombat2());
	}

	private bool CheckForCrack()
	{
		return false;
	}

	[PunRPC]
	public void NET_FinishCombat2()
	{
		AlertManager.Instance.HideAlert();
		StartCoroutine(FinishCombat2());
	}

	private IEnumerator FinishCombat2()
	{
		if (!resignCombat)
		{
			newTurnScript.FinishCombat(AnyHeroAlive());
			AtOManager.Instance.combatScarab = scarabSpawned + "%" + scarabSuccess;
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("**************************", "net");
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("WaitingSyncro finishcombat", "net");
			}
			yield return Globals.Instance.WaitForSeconds(0.1f);
			if (NetworkManager.Instance.IsMaster())
			{
				if (coroutineSyncFinishCombat != null)
				{
					StopCoroutine(coroutineSyncFinishCombat);
				}
				coroutineSyncFinishCombat = StartCoroutine(ReloadCombatCo("finishcombat"));
				while (!NetworkManager.Instance.AllPlayersReady("finishcombat"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (coroutineSyncFinishCombat != null)
				{
					StopCoroutine(coroutineSyncFinishCombat);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked finishcombat", "net");
				}
				NetworkManager.Instance.PlayersNetworkContinue("finishcombat");
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("finishcombat", status: true);
				NetworkManager.Instance.SetStatusReady("finishcombat");
				while (NetworkManager.Instance.WaitingSyncro["finishcombat"])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("finishcombat, we can continue!", "net");
				}
			}
		}
		AlertManager.Instance.HideAlert();
		if (!resignCombat && AnyHeroAlive())
		{
			for (int i = 0; i < TeamHero.Length; i++)
			{
				if (TeamHero[i] != null && TeamHero[i].HeroData != null && TeamHero[i].Alive)
				{
					TeamHero[i].SetEvent(Enums.EventActivation.BeginCombatEnd);
				}
			}
			GameManager.Instance.PlayLibraryAudio("stinger_win_game");
		}
		else
		{
			GameManager.Instance.PlayLibraryAudio("stinger_lose_game");
		}
		AudioManager.Instance.StopAmbience();
		AudioManager.Instance.StopBSO();
	}

	private void WantToRetry()
	{
		bool confirmAnswer = AlertManager.Instance.GetConfirmAnswer();
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(WantToRetry));
		if (!confirmAnswer)
		{
			if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
			{
				photonView.RPC("NET_FinishCombat2", RpcTarget.All);
			}
			else
			{
				NET_FinishCombat2();
			}
		}
		else
		{
			MatchIsOver = false;
			ReloadCombatFull();
		}
	}

	public void BackToFinishGame()
	{
		if (!resignCombat && combatData != null && AnyHeroAlive())
		{
			ThermometerTierData thermometerTierData = combatData.ThermometerTierData;
			ThermometerData thermometerData = null;
			bool flag = false;
			if (thermometerTierData != null)
			{
				for (int i = 0; i < thermometerTierData.RoundThermometer.Length; i++)
				{
					if (currentRound >= thermometerTierData.RoundThermometer[i].Round)
					{
						thermometerData = thermometerTierData.RoundThermometer[i].ThermometerData;
						if (thermometerTierData.RoundThermometer[i].Round > currentRound)
						{
							break;
						}
					}
				}
				flag = true;
				AtOManager.Instance.SetCombatExpertise(thermometerData);
				if (MadnessManager.Instance.IsMadnessTraitActive("equalizer") && thermometerTierData != null && thermometerTierData.RoundThermometer[1] != null && thermometerData != null)
				{
					ThermometerData thermometerData2 = UnityEngine.Object.Instantiate(thermometerTierData.RoundThermometer[1].ThermometerData);
					thermometerData2.ThermometerId = thermometerData.ThermometerId;
					thermometerData2.ThermometerColor = thermometerData.ThermometerColor;
					thermometerData = thermometerData2;
				}
			}
			if (!flag)
			{
				AtOManager.Instance.SetCombatExpertise(thermometerData);
			}
			AtOManager.Instance.SetCombatThermometerData(thermometerData);
		}
		StopCoroutines();
		AtOManager.Instance.FinishCombat(resignCombat);
	}

	public void ResignCombat()
	{
		if (!resignCombat)
		{
			if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
			{
				AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("combatOnlyMasterCanResign"));
				AlertManager.buttonClickDelegate = null;
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Texts.Instance.GetText("combatWantToResign"));
			stringBuilder.Append("<br><size=-4><color=#AAAAAA>");
			stringBuilder.Append(Texts.Instance.GetText("combatWantToResignEnd"));
			stringBuilder.Append("</size></color>");
			AlertManager.buttonClickDelegate = ResignCombatAction;
			AlertManager.Instance.AlertConfirmDouble(stringBuilder.ToString());
			AlertManager.Instance.ShowResignIcon();
		}
	}

	private void ResignCombatAction()
	{
		bool confirmAnswer = AlertManager.Instance.GetConfirmAnswer();
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(ResignCombatAction));
		if (confirmAnswer)
		{
			if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
			{
				photonView.RPC("NET_ResignCombatActionExecute", RpcTarget.Others);
			}
			StartCoroutine(ResignCombatActionExecute());
		}
	}

	[PunRPC]
	public void NET_ResignCombatActionExecute()
	{
		StartCoroutine(ResignCombatActionExecute());
	}

	private IEnumerator ResignCombatActionExecute()
	{
		AtOManager.Instance.combatCardDictionary = null;
		AtOManager.Instance.combatGameCode = "";
		resignCombat = true;
		if (GameManager.Instance.IsMultiplayer())
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("**************************", "net");
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("WaitingSyncro resign", "net");
			}
			yield return Globals.Instance.WaitForSeconds(0.1f);
			if (NetworkManager.Instance.IsMaster())
			{
				if (coroutineSyncResign != null)
				{
					StopCoroutine(coroutineSyncResign);
				}
				coroutineSyncResign = StartCoroutine(ReloadCombatCo("resign"));
				while (!NetworkManager.Instance.AllPlayersReady("resign"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (coroutineSyncResign != null)
				{
					StopCoroutine(coroutineSyncResign);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked resign", "net");
				}
				NetworkManager.Instance.PlayersNetworkContinue("resign");
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("resign", status: true);
				NetworkManager.Instance.SetStatusReady("resign");
				while (NetworkManager.Instance.WaitingSyncro["resign"])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("resign, we can continue!", "net");
				}
			}
		}
		for (int i = 0; i < TeamHero.Length; i++)
		{
			if (TeamHero[i] != null && !(TeamHero[i].HeroData == null))
			{
				TeamHero[i].Alive = false;
			}
		}
		FinishCombat();
	}

	private void SetCardsWaitingForReset(int _num)
	{
		cardsWaitingForReset = _num;
	}

	private void ShowHandMask(bool state)
	{
		if (HandMask.gameObject.activeSelf != state)
		{
			HandMask.gameObject.SetActive(state);
		}
	}

	private void ShowMaskFull()
	{
		RawImage component = MaskWindow.GetChild(0).transform.GetComponent<RawImage>();
		MaskWindow.gameObject.SetActive(value: true);
		component.color = new Color(0f, 0f, 0f, 1f);
	}

	private void HideMaskFull()
	{
		StartCoroutine(HideMaskFullCo());
	}

	private IEnumerator HideMaskFullCo()
	{
		RawImage imageBg = MaskWindow.GetChild(0).transform.GetComponent<RawImage>();
		float index = imageBg.color.a;
		while (index > 0.8f)
		{
			imageBg.color = new Color(0f, 0f, 0f, index);
			index -= 0.025f;
			yield return null;
		}
	}

	public void ShowMaskFromUIScreen(bool state)
	{
		RawImage component = MaskWindow.GetChild(0).transform.GetComponent<RawImage>();
		if (state)
		{
			component.color = new Color(0f, 0f, 0f, 0.8f);
			MaskWindow.GetComponent<BoxCollider2D>().enabled = true;
		}
		else
		{
			component.color = new Color(0f, 0f, 0f, 0.15f);
			MaskWindow.GetComponent<BoxCollider2D>().enabled = false;
		}
	}

	public void ShowMask(bool state, bool hardMask = true)
	{
		if (!lockHideMask)
		{
			if (coroutineMask != null)
			{
				StopCoroutine(coroutineMask);
			}
			coroutineMask = StartCoroutine(ShowMaskCo(state, hardMask));
		}
	}

	private IEnumerator ShowMaskCo(bool state, bool hardMask)
	{
		float maxAlplha = 0.8f;
		if (!hardMask)
		{
			maxAlplha = 0.25f;
		}
		RawImage imageBg = MaskWindow.GetChild(0).transform.GetComponent<RawImage>();
		float index = imageBg.color.a;
		if (!state)
		{
			yield return Globals.Instance.WaitForSeconds(0.1f);
			while (index > 0f)
			{
				imageBg.color = new Color(0f, 0f, 0f, index);
				index -= 0.05f;
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			imageBg.color = new Color(0f, 0f, 0f, 0f);
			MaskWindow.gameObject.SetActive(value: false);
		}
		else
		{
			MaskWindow.gameObject.SetActive(value: true);
			while (index < maxAlplha)
			{
				imageBg.color = new Color(0f, 0f, 0f, index);
				index += 0.05f;
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			imageBg.color = new Color(0f, 0f, 0f, maxAlplha);
		}
	}

	public void SetAuraIterator(Character target, Character theCaster, AuraCurseData _acData, int charges, bool fromTrait = false)
	{
		StartCoroutine(SetAuraIteratorCo(target, theCaster, _acData, charges, fromTrait));
	}

	private IEnumerator SetAuraIteratorCo(Character target, Character theCaster, AuraCurseData _acData, int charges, bool fromTrait = false)
	{
		yield return Globals.Instance.WaitForSeconds(0.01f);
		if (target != null && target.Alive)
		{
			target.SetAura(theCaster, _acData, charges, fromTrait);
		}
	}

	private void GiveExhaust()
	{
		if (!isBeginTournPhase && (AtOManager.Instance.IsChallengeTraitActive("exhaustion") || AtOManager.Instance.GetNgPlus() > 1 || AtOManager.Instance.GetSingularityMadness() > 3) && heroActive > -1 && TeamHero[heroActive] != null && TeamHero[heroActive].Alive)
		{
			TeamHero[heroActive].SetAura(null, Globals.Instance.GetAuraCurseData("Exhaust"), 1);
			DoExhaust();
		}
	}

	public void DoExhaust()
	{
		if (AtOManager.Instance.IsChallengeTraitActive("exhaustion") || AtOManager.Instance.GetNgPlus() > 1 || AtOManager.Instance.GetSingularityMadness() > 3)
		{
			if (heroActive > -1 && TeamHero[heroActive] != null && TeamHero[heroActive].Alive && TeamHero[heroActive].HasEffect("Exhaust"))
			{
				ShowExhaust();
				exhaustNumber.text = TeamHero[heroActive].GetAuraCharges("Exhaust").ToString();
			}
			else
			{
				HideExhaust();
			}
		}
	}

	public void ShowExhaust()
	{
		if (!exhaustT.gameObject.activeSelf)
		{
			exhaustT.gameObject.SetActive(value: true);
		}
	}

	public void HideExhaust()
	{
		if (exhaustT.gameObject.activeSelf)
		{
			exhaustT.gameObject.SetActive(value: false);
		}
	}

	private void ResetItemTimeout()
	{
		for (int i = 0; i < itemTimeout.Length; i++)
		{
			itemTimeout[i] = 0f;
		}
	}

	public void DoItem(Character caller, Enums.EventActivation theEvent, CardData cardData, string item, Character target, int auxInt, string auxString, int timesActivated = 0, bool forceActivate = false)
	{
		ResetAutoEndCount();
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("match do item " + theEvent.ToString() + " -> " + item, "item");
		}
		bool flag = false;
		if (cardData != null && cardData.Item != null && (cardData.Item.DrawCards > 0 || cardData.Item.CardNum > 0 || cardData.Item.CardsReduced > 0))
		{
			flag = true;
		}
		if (cardData != null && cardData.ItemEnchantment != null && (cardData.ItemEnchantment.DrawCards > 0 || cardData.ItemEnchantment.CardNum > 0 || cardData.ItemEnchantment.CardsReduced > 0))
		{
			flag = true;
		}
		if (theEvent == Enums.EventActivation.CorruptionBeginRound || theEvent == Enums.EventActivation.CorruptionCombatStart)
		{
			flag = false;
		}
		if (!flag || (ctQueue.Count == 0 && eventList.Count == 0))
		{
			StartCoroutine(DoItemCo(caller, theEvent, cardData, item, target, auxInt, auxString, timesActivated, forceActivate));
			return;
		}
		bool flag2 = false;
		if (eventList.Count > 0)
		{
			for (int i = 0; i < eventList.Count; i++)
			{
				if (eventList[i].StartsWith("item->"))
				{
					flag2 = true;
				}
			}
		}
		if (flag2)
		{
			ctQueue.Enqueue(delegate
			{
				StartCoroutine(DoItemCo(caller, theEvent, cardData, item, target, auxInt, auxString, timesActivated));
			});
			if (checkQueueCo != null)
			{
				StopCoroutine(checkQueueCo);
			}
			checkQueueCo = StartCoroutine(CheckDeQueue());
		}
		else
		{
			StartCoroutine(DoItemCo(caller, theEvent, cardData, item, target, auxInt, auxString, timesActivated, forceActivate));
		}
	}

	private void StartDoItemSafely(Character caller, Enums.EventActivation theEvent, CardData cardData, string item, Character target, int auxInt, string auxString, int timesActivated, bool forceActivate)
	{
		if (!doItemCoScheduledThisFrame)
		{
			doItemCoScheduledThisFrame = true;
			StartCoroutine(DoItemDelayed(caller, theEvent, cardData, item, target, auxInt, auxString, timesActivated, forceActivate));
		}
	}

	private IEnumerator DoItemDelayed(Character caller, Enums.EventActivation theEvent, CardData cardData, string item, Character target, int auxInt, string auxString, int timesActivated, bool forceActivate)
	{
		yield return null;
		doItemCoScheduledThisFrame = false;
		yield return StartCoroutine(DoItemCo(caller, theEvent, cardData, item, target, auxInt, auxString, timesActivated, forceActivate));
	}

	private void ClearItemQueue()
	{
		for (int num = eventList.Count - 1; num >= 0; num--)
		{
			if (eventList[num].StartsWith("item->"))
			{
				eventList.RemoveAt(num);
			}
		}
		ctQueue.Clear();
	}

	private IEnumerator CheckDeQueue()
	{
		int cleanList = 0;
		int exhaustIndex = 0;
		while (ctQueue.Count > 0)
		{
			while (waitingDeathScreen)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[CheckDeQueue] waitingDeathScreen");
				}
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			while (waitingKill)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[CheckDeQueue] waitingKill");
				}
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			yield return Globals.Instance.WaitForSeconds(0.01f);
			if (eventList.Count == 0 && ctQueue.Count > 0)
			{
				if (cleanList >= 1)
				{
					ctQueue.Dequeue()();
					cleanList = 0;
				}
				else
				{
					cleanList++;
				}
				exhaustIndex = 0;
				continue;
			}
			cleanList = 0;
			if (eventList.Count == 1 && eventList[0].StartsWith("CastCardEvent"))
			{
				exhaustIndex++;
				if (exhaustIndex >= 100)
				{
					Debug.LogError("DeQUEUE B");
					ctQueue.Dequeue()();
					cleanList = 0;
				}
			}
			else
			{
				exhaustIndex = 0;
			}
		}
	}

	private NPC GetRandomNPC()
	{
		NPC[] teamNPC = Instance.GetTeamNPC();
		List<int> list = new List<int>();
		for (int i = 0; i < teamNPC.Length; i++)
		{
			if (teamNPC[i] != null && teamNPC[i].Alive)
			{
				list.Add(i);
			}
		}
		if (list.Count > 0)
		{
			bool flag = false;
			int num = Instance.GetRandomIntRange(0, list.Count, "item");
			int num2 = 0;
			while (!flag)
			{
				if (teamNPC[list[num]] != null && teamNPC[list[num]].Alive)
				{
					return teamNPC[list[num]];
				}
				num++;
				num %= teamNPC.Length;
				num2++;
				if (num2 > teamNPC.Length)
				{
					return null;
				}
			}
		}
		return null;
	}

	private IEnumerator DoItemCo(Character caller, Enums.EventActivation theEvent, CardData cardData, string item, Character target, int auxInt, string auxString, int timesActivated = 0, bool forceActivate = false)
	{
		SetEventDirect("item->" + item, automatic: false, add: true);
		if (!itemsActivatedThisFrame.ContainsKey(item))
		{
			itemsActivatedThisFrame[item] = 0;
		}
		itemsActivatedThisFrame[item]++;
		int delayFrames = itemsActivatedThisFrame[item];
		yield return null;
		itemsActivatedThisFrame.Remove(item);
		for (int i = 0; i < delayFrames; i++)
		{
			yield return null;
		}
		CardData _castedCard = caller.CardCasted;
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[DoItemCo] doitemco -> " + theEvent.ToString() + " -> " + item + " " + timesActivated + " " + cardData, "item");
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[DoItemCo] Delay for item " + 0.1f * (float)timesActivated, "item");
		}
		yield return Globals.Instance.WaitForSeconds(0.2f * (float)timesActivated);
		while (eventList.Contains("ResetDeck"))
		{
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
		if (caller != null && !caller.IsHero && theEvent == Enums.EventActivation.BeginTurn)
		{
			if (!activationItemsAtBeginTurnList.Contains(item))
			{
				activationItemsAtBeginTurnList.Add(item);
			}
			while (true)
			{
				List<string> list = activationItemsAtBeginTurnList;
				if (list != null && list.Count > 0 && activationItemsAtBeginTurnList[0] != item)
				{
					yield return Globals.Instance.WaitForSeconds(0.1f);
					if (caller == null || !caller.Alive)
					{
						activationItemsAtBeginTurnList.Clear();
						break;
					}
					continue;
				}
				break;
			}
			while (waitingKill)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[DoItem] waitingKill");
				}
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
			if (activationItemsAtBeginTurnList != null && activationItemsAtBeginTurnList.Count > 0 && activationItemsAtBeginTurnList[0] == item)
			{
				if (activationItemsAtBeginTurnList.Count > 0)
				{
					yield return Globals.Instance.WaitForSeconds(0.2f);
				}
				activationItemsAtBeginTurnList.RemoveAt(0);
			}
		}
		if ((bool)cardData?.ItemEnchantment && cardData.ItemEnchantment.HealBasedOnAuraCurse > 0 && cardData.ItemEnchantment.AuraCurseSetted != null)
		{
			caller.ModifyHp(cardData.ItemEnchantment.HealBasedOnAuraCurse * caller.GetAuraCharges(cardData.ItemEnchantment.AuraCurseSetted.Id));
		}
		int exhaustEvent = 0;
		if (theEvent == Enums.EventActivation.FinishCast)
		{
			for (; exhaustEvent < 700; exhaustEvent++)
			{
				if (cardsWaitingForReset <= 0)
				{
					break;
				}
				if (exhaustEvent > 0)
				{
					_ = exhaustEvent % 100;
				}
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			for (exhaustEvent = 0; exhaustEvent < 700; exhaustEvent++)
			{
				if (NumChildsInTemporal() <= 0)
				{
					break;
				}
				if (exhaustEvent > 0 && exhaustEvent % 100 == 0)
				{
					Debug.Log("[DoItemCo] ************************ YYY " + NumChildsInTemporal());
				}
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[DoItemCo] doitemco -> " + theEvent.ToString() + " -> " + item + " " + timesActivated + " " + cardData, "item");
		}
		if (!IsBeginTournPhase || theEvent != Enums.EventActivation.Killed)
		{
			while (waitingKill)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[DoItemCo] waitingKill", "general");
				}
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
			while (waitingDeathScreen)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[DoItemCo] waitingDeathScreen", "general");
				}
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
			while (cardsWaitingForReset > 0)
			{
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
		}
		exhaustEvent = 0;
		if (IsBeginTournPhase && theEvent != Enums.EventActivation.Killed)
		{
			while (NumChildsInTemporal() > 0 && exhaustEvent < 20)
			{
				yield return Globals.Instance.WaitForSeconds(0.1f);
				exhaustEvent++;
			}
		}
		if (!caller.Alive && theEvent != Enums.EventActivation.Killed)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[DoItemCo] break because Hero died", "general");
			}
			SetEventDirect("item->" + item, automatic: false);
			yield break;
		}
		if (MatchIsOver && theEvent != Enums.EventActivation.BeginCombatEnd)
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[DoItemCo] Broken by finish game", "general");
			}
			SetEventDirect("item->" + item, automatic: false);
			yield break;
		}
		string key = "";
		if ((object)cardData != null && cardData.CardType == Enums.CardType.Corruption && theEvent != Enums.EventActivation.CorruptionBeginRound)
		{
			key = logDictionary.Count.ToString();
			CreateLogEntry(_initial: true, key, item, theHero, theNPC, null, null, currentRound, Enums.EventActivation.CorruptionBeginRound);
		}
		caller.DoItem(theEvent, cardData, item, target, auxInt, auxString, timesActivated, _castedCard, forceActivate);
		if ((object)cardData != null && cardData.CardType == Enums.CardType.Corruption && theEvent != Enums.EventActivation.CorruptionBeginRound)
		{
			CreateLogEntry(_initial: false, key, item, theHero, theNPC, null, null, currentRound, Enums.EventActivation.CorruptionBeginRound);
		}
		if ((object)cardData != null && cardData.ItemEnchantment?.Activation == Enums.EventActivation.EndRound && CanApplyNightmareImage(cardData, caller))
		{
			yield return ApplyNightmareImageCo(cardData, caller);
		}
		ResetAutoEndCount();
		SetEventDirect("item->" + item, automatic: false);
	}

	public bool CardRandomRarityOk(int rarity, CardData cardData)
	{
		if ((rarity < 80 && cardData.CardUpgraded == Enums.CardUpgraded.No) || (rarity >= 80 && rarity < 90 && cardData.CardUpgraded == Enums.CardUpgraded.A) || (rarity >= 90 && cardData.CardUpgraded == Enums.CardUpgraded.B))
		{
			return true;
		}
		return false;
	}

	public void DoComic(Character _character, string _text, float duration = 3f)
	{
		if (comicGO != null || _character == null)
		{
			return;
		}
		bool flag = true;
		Vector3 zero = Vector3.zero;
		Transform transform = null;
		if (_character.HeroItem != null)
		{
			zero = new Vector3(_character.HeroData.HeroSubClass.FluffOffsetX, _character.HeroData.HeroSubClass.FluffOffsetY, 0f);
			transform = _character.HeroItem.animatedTransform;
		}
		else
		{
			if (!(_character.NPCItem != null))
			{
				return;
			}
			flag = false;
			zero = new Vector3(_character.NpcData.FluffOffsetX, _character.NpcData.FluffOffsetY, 0f);
			transform = _character.NPCItem.animatedTransform;
		}
		comicGO = UnityEngine.Object.Instantiate(comicPrefab, Vector3.zero, Quaternion.identity, transform);
		if (transform.localScale.x < 0f)
		{
			comicGO.transform.localScale = new Vector3(-1f, comicGO.transform.localScale.y, comicGO.transform.localScale.z);
		}
		else
		{
			comicGO.transform.localScale = new Vector3(1f, comicGO.transform.localScale.y, comicGO.transform.localScale.z);
		}
		comicGO.transform.localPosition = Vector3.zero + zero;
		if (!flag)
		{
			comicGO.transform.GetChild(1).GetComponent<SpriteRenderer>().flipX = true;
		}
		comicGO.transform.GetChild(0).GetComponent<TMP_Text>().text = _text;
		StartCoroutine(HideComic(duration));
	}

	private IEnumerator HideComic(float duration = 0f)
	{
		if (comicGO != null)
		{
			yield return Globals.Instance.WaitForSeconds(duration);
			if (comicGO != null)
			{
				if (duration > 0f)
				{
					comicGO.transform.GetComponent<Animator>().SetTrigger("hide");
					yield return Globals.Instance.WaitForSeconds(1.3f);
				}
				UnityEngine.Object.Destroy(comicGO);
				comicGO = null;
			}
		}
		yield return null;
	}

	public void GenerateRandomStringBatch(int total = -1)
	{
		if (resultSeed == -1)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(AtOManager.Instance.currentMapNode);
			stringBuilder.Append(AtOManager.Instance.GetGameId());
			if (combatData != null)
			{
				stringBuilder.Append(combatData.CombatId);
			}
			resultSeed = stringBuilder.ToString().GetDeterministicHashCode();
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("GenerateRandomStringBatch =>" + resultSeed, "randomindex");
		}
		UnityEngine.Random.InitState(resultSeed);
		if (total == -1)
		{
			total = randomStringArrLength;
		}
		randomStringArr.Clear();
		for (int i = 0; i < total; i++)
		{
			randomStringArr.Add(Functions.RandomString(6f));
		}
		randomStringItemsArr.Clear();
		for (int j = 0; j < total; j++)
		{
			randomStringItemsArr.Add(Functions.RandomString(6f));
		}
		randomStringTraitsArr.Clear();
		for (int k = 0; k < total; k++)
		{
			randomStringTraitsArr.Add(Functions.RandomString(6f));
		}
		randomStringDeckArr.Clear();
		for (int l = 0; l < total; l++)
		{
			randomStringDeckArr.Add(Functions.RandomString(6f));
		}
		randomStringShuffleArr.Clear();
		for (int m = 0; m < total; m++)
		{
			randomStringShuffleArr.Add(Functions.RandomString(6f));
		}
		randomStringMiscArr.Clear();
		for (int n = 0; n < total; n++)
		{
			randomStringMiscArr.Add(Functions.RandomString(6f));
		}
	}

	private void SetRandomIndex(int _value, string _type = "")
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("SetRandomIndex -> " + _value, "randomindex");
		}
		if (_type == "")
		{
			randomIndex = _value;
			randomItemsIndex = _value;
			randomTraitsIndex = _value;
			randomDeckIndex = _value;
			randomShuffleIndex = _value;
			randomMiscIndex = _value;
		}
		else if (_type == "shuffle")
		{
			randomShuffleIndex = _value;
		}
	}

	public void GenerateRandomIndex(bool share = true)
	{
		Debug.Log("GenerateRandomIndex " + share);
		UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
		SetRandomIndex(UnityEngine.Random.Range(100, 500));
		if (share)
		{
			ShareRandomIndex();
		}
	}

	private void ShareRandomIndex()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("ShareRandomIndex -> " + randomIndex, "randomindex");
		}
		photonView.RPC("NET_SendRandomSeed", RpcTarget.Others, randomIndex);
	}

	[PunRPC]
	private void NET_SendRandomSeed(int _index)
	{
		SetRandomIndex(_index);
	}

	private void SendRandomStringBatch()
	{
		string text = JsonHelper.ToJson(randomStringArr.ToArray());
		photonView.RPC("NET_SetRandomStringBatch", RpcTarget.Others, Functions.CompressString(text));
	}

	[PunRPC]
	private void NET_SetRandomStringBatch(string _arr)
	{
		string json = Functions.DecompressString(_arr);
		randomStringArr = JsonHelper.FromJson<string>(json).ToList();
	}

	public string GetRandomString(string type = "default")
	{
		string text = "";
		switch (type)
		{
		case "default":
			if (randomIndex >= randomStringArr.Count)
			{
				randomIndex = 0;
			}
			text = randomStringArr[randomIndex];
			randomIndex++;
			break;
		case "item":
			if (randomItemsIndex >= randomStringItemsArr.Count)
			{
				randomItemsIndex = 0;
			}
			text = randomStringItemsArr[randomItemsIndex];
			randomItemsIndex++;
			break;
		case "trait":
			if (randomTraitsIndex >= randomStringTraitsArr.Count)
			{
				randomTraitsIndex = 0;
			}
			text = randomStringTraitsArr[randomTraitsIndex];
			randomTraitsIndex++;
			break;
		case "deck":
			if (randomDeckIndex >= randomStringDeckArr.Count)
			{
				randomDeckIndex = 0;
			}
			text = randomStringDeckArr[randomDeckIndex];
			randomDeckIndex++;
			break;
		case "shuffle":
			if (randomShuffleIndex >= randomStringShuffleArr.Count)
			{
				randomShuffleIndex = 0;
			}
			text = randomStringShuffleArr[randomShuffleIndex];
			randomShuffleIndex++;
			break;
		case "misc":
			if (randomMiscIndex >= randomStringMiscArr.Count)
			{
				randomMiscIndex = 0;
			}
			text = randomStringMiscArr[randomMiscIndex];
			randomMiscIndex++;
			break;
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("GRS " + type + "->" + text, "randomindex");
		}
		return text;
	}

	public int GetRandomIntRange(int min, int max, string type = "default", string seed = "")
	{
		if (seed == "")
		{
			seed = GetRandomString(type);
		}
		return Functions.Random(min, max, seed);
	}

	public int GetRandomIntRangeOLD(int min, int max, string type = "default", string randomStr = "")
	{
		if (min == max)
		{
			return min;
		}
		string text = randomStr;
		if (text == "")
		{
			text = GetRandomString(type);
		}
		long num = 0L;
		if (min == 0 && max == 2 && (type == "item" || type == "deck"))
		{
			num = text[0];
		}
		else
		{
			int num2 = 0;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] == ' ')
				{
					num2++;
				}
			}
			long[] sumArr = new long[num2 + 1];
			num = Functions.ASCIIWordSum(text, sumArr);
		}
		return min + Mathf.FloorToInt(num % (max - min));
	}

	public void ShowCharacterWindow(string type = "", bool isHero = true, int characterIndex = -1)
	{
		if (isHero)
		{
			sideCharacters.Show();
			sideCharacters.ShowActiveStatus(characterIndex);
			characterWindow.Show(type, characterIndex);
		}
		else
		{
			characterWindow.Show(type, characterIndex, isHero: false);
		}
	}

	public void HideCharacterWindow()
	{
		sideCharacters.Hide();
	}

	private void GenerateSyncCodeDict()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("GenerateSyncCodeDict", "general");
		}
		syncCodeDict.Clear();
		Player[] playerList = NetworkManager.Instance.PlayerList;
		foreach (Player player in playerList)
		{
			syncCodeDict.Add(player.NickName, "");
		}
	}

	private int CheckSyncCodeDict()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("CheckSyncCodeDict", "general");
		}
		foreach (KeyValuePair<string, string> item in syncCodeDict)
		{
			if (item.Value == "")
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("CheckSyncCodeDict => 0", "general");
				}
				return 0;
			}
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("CheckSyncCodeDict => 1", "general");
		}
		return 1;
	}

	private void SetMasterSyncCode(bool md5 = false)
	{
		currentGameCode = GenerateSyncCode(fixDeck: true);
		if (GameManager.Instance.IsMultiplayer())
		{
			string text = GenerateSyncCode();
			if (md5)
			{
				text = Functions.Md5Sum(text);
			}
			InsertSyncCode(NetworkManager.Instance.GetPlayerNick(), text);
		}
	}

	private void RequestSyncCode()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("RequestSyncCode");
		}
		photonView.RPC("NET_RequestSyncCode", RpcTarget.Others);
	}

	[PunRPC]
	private void NET_RequestSyncCode()
	{
		SendSyncCode();
	}

	private void SendSyncCode(bool md5 = false)
	{
		string text = GenerateSyncCode();
		if (md5)
		{
			text = Functions.Md5Sum(text);
		}
		string text2 = Functions.CompressString(text);
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("SendSyncCode " + text2);
		}
		photonView.RPC("NET_SendSyncCode", RpcTarget.MasterClient, NetworkManager.Instance.GetPlayerNick(), text2);
	}

	[PunRPC]
	private void NET_SendSyncCode(string _nick, string _code)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("SYNC CODE received from " + _nick, "synccode");
		}
		string code = Functions.DecompressString(_code);
		InsertSyncCode(_nick, code);
	}

	private void InsertSyncCode(string _nick, string _code)
	{
		if (!syncCodeDict.ContainsKey(_nick))
		{
			syncCodeDict.Add(_nick, _code);
		}
		else
		{
			syncCodeDict[_nick] = _code;
		}
	}

	public void ALL_BreakByDesync()
	{
		if (!MatchIsOver && (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster()))
		{
			AlertManager.Instance.AlertConfirmDouble(Texts.Instance.GetText("reloadForced"));
			AlertManager.Instance.ShowReloadIcon();
			AlertManager.buttonClickDelegate = ReloadCombatFull;
		}
	}

	private void ReloadCombatFull()
	{
		if (MatchIsOver)
		{
			return;
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("---------> ReloadCombatFull <---------", "general");
		}
		bool confirmAnswer = AlertManager.Instance.GetConfirmAnswer();
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(ReloadCombatFull));
		if (confirmAnswer && !reloadingGame)
		{
			if (coroutineSync != null)
			{
				StopCoroutine(coroutineSync);
			}
			StopCoroutines();
			AtOManager.Instance.DeleteSaveGameTurn();
			ReloadCombatFullAction();
		}
	}

	private void ReloadCombatFullAction()
	{
		for (int i = 0; i < 4; i++)
		{
			if (TeamHero[i] != null)
			{
				TeamHero[i].HpCurrent = heroLifeArr[i];
				TeamHero[i].Alive = true;
				if (i < heroBeginItems.Count && heroBeginItems[i] != null)
				{
					List<string> list = heroBeginItems[i];
					TeamHero[i].Weapon = list[0];
					TeamHero[i].Armor = list[1];
					TeamHero[i].Jewelry = list[2];
					TeamHero[i].Accesory = list[3];
					TeamHero[i].Pet = list[4];
				}
			}
		}
		AtOManager.Instance.combatCardDictionary = null;
		AtOManager.Instance.combatGameCode = "";
		currentGameCode = "";
		heroDestroyedItemsInThisTurn.Clear();
		heroBeginItems.Clear();
		heroLifeArr = null;
		if (GameManager.Instance.IsMultiplayer())
		{
			AtOManager.Instance.DoLoadGameFromMP(comingFromReloadCombat: true);
		}
		else
		{
			AtOManager.Instance.LoadGame(-1, comingFromReloadCombat: true);
		}
	}

	private IEnumerator ReloadCombatCo(string from)
	{
		yield return Globals.Instance.WaitForSeconds(12f);
		ReloadCombat(from);
	}

	private void ReloadCombat(string from = "")
	{
		if (reloadingGame)
		{
			return;
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("---------> RELOAD COMBAT " + from + " <----------", "reload");
		}
		reloadingGame = true;
		characterWindow.Hide();
		StopCoroutines();
		AlertManager.Instance.Abort();
		ShowMaskFull();
		synchronizing.gameObject.SetActive(value: true);
		NetworkManager.Instance.ClearSyncro();
		ClearEventList();
		currentGameCode = currentGameCodeForReload;
		if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
		{
			photonView.RPC("NET_ReloadCombat", RpcTarget.Others, Functions.CompressString(currentGameCode), from);
			NetworkManager.Instance.ClearPlayerStatus();
			NetworkManager.Instance.ClearWaitingCalls();
		}
		gameStatus = "";
		cardActive = null;
		beforeSyncCodeLocked = false;
		ResetAutoEndCount();
		heroTurn = false;
		waitingDeathScreen = false;
		waitingKill = false;
		waitingForAddcardAssignment = false;
		waitingForCardEnergyAssignment = false;
		waitingForDiscardAssignment = false;
		waitingForLookDiscardWindow = false;
		matchIsOver = false;
		characterKilled = false;
		heroActive = -1;
		npcActive = -1;
		preCastNum = -1;
		scarabSpawned = "";
		scarabSuccess = "";
		theHeroPreAutomatic = null;
		theNPCPreAutomatic = null;
		turnLoadedBySave = false;
		activatedTraitsRound.Clear();
		SetCardsWaitingForReset(0);
		GlobalDiscardCardsNum = 0;
		GlobalVanishCardsNum = 0;
		targetTransformDict = new Dictionary<string, Transform>();
		castingCardBlocked = new Dictionary<string, bool>();
		deckPileVisualState = 0;
		deathScreen.TurnOff();
		CleanTempTransform();
		PopupManager.Instance.ClosePopup();
		ClearItemExecuteForThisTurn();
		ItemExecuteForThisCombatReload();
		AtOManager.Instance.combatCardDictionary = new Dictionary<string, CardData>();
		StartCoroutine(MoveItemsOut(state: true));
		foreach (Transform item in GO_Hand.transform)
		{
			if (item != null && item.gameObject.name != "HandMask")
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}
		foreach (Transform item2 in GO_Heroes.transform)
		{
			UnityEngine.Object.Destroy(item2.gameObject);
		}
		foreach (Transform item3 in GO_NPCs.transform)
		{
			UnityEngine.Object.Destroy(item3.gameObject);
		}
		foreach (Transform item4 in combattextTransform)
		{
			UnityEngine.Object.Destroy(item4.gameObject);
		}
		foreach (KeyValuePair<string, CardData> item5 in cardDictionary)
		{
			AtOManager.Instance.combatCardDictionary.Add(item5.Key, item5.Value);
		}
		AtOManager.Instance.combatGameCode = currentGameCode;
		Start();
	}

	public bool WaitingForActionScreen()
	{
		if (waitingForAddcardAssignment || waitingForCardEnergyAssignment || waitingForDiscardAssignment || waitingForLookDiscardWindow)
		{
			return true;
		}
		return false;
	}

	[PunRPC]
	private void NET_MasterReloadCombat(string _theProblem)
	{
		ReloadCombat(_theProblem);
	}

	[PunRPC]
	private void NET_ReloadCombat(string _theCode, string _from)
	{
		StopCoroutines();
		currentGameCode = Functions.DecompressString(_theCode);
		currentGameCodeForReload = currentGameCode;
		ReloadCombat(_from);
	}

	public int DesyncFixable()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[DESYNC FIXABLE??]");
		}
		string text = GenerateSyncCode();
		string text2 = "";
		foreach (KeyValuePair<string, string> item in syncCodeDict)
		{
			text2 = "";
			if (item.Value == "")
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[DESYNC from " + item.Key + "] Empty code", "synccode");
				}
				return 1;
			}
			if (!(item.Value != text))
			{
				continue;
			}
			text2 = item.Value;
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[SOURCE] " + text, "synccode");
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[TARGET] " + text2, "synccode");
			}
			string[] array = text.Split('$');
			string[] array2 = text2.Split('$');
			if (array.Length != array2.Length)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("[NOT FIXABLE] Length mismatch", "synccode");
				}
				return 1;
			}
			for (int i = 0; i < array.Length; i++)
			{
				string[] array3 = array[i].Split('|');
				string[] array4 = array2[i].Split('|');
				if (array3.Length != array4.Length)
				{
					if (Globals.Instance.ShowDebug)
					{
						Functions.DebugLogGD("[NOT FIXABLE] Subcode Length mismatch", "synccode");
					}
					return 1;
				}
			}
			if (array.Length > 10 && array2.Length > 10 && array[10] != array2[10])
			{
				Debug.LogError("[CheckSyncCode] Dictionary code mismatch");
				return 2;
			}
		}
		return 0;
	}

	public bool AllDesyncEqual(string theCode)
	{
		foreach (KeyValuePair<string, string> item in syncCodeDict)
		{
			if (item.Value == "")
			{
				return false;
			}
			if (item.Value != theCode)
			{
				return false;
			}
		}
		return true;
	}

	[PunRPC]
	private void NET_FinishCastCodeSyncFromMaster(int _randomIndex, string _codeFromMaster, string _cardId)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("NET_FinishCastCodeSyncFromMaster " + _codeFromMaster + " // " + _cardId, "synccode");
		}
		FixCodeSyncFromMaster(_randomIndex, _codeFromMaster, _sendStatusReady: false, _sendFinishCastReady: true, _cardId);
	}

	[PunRPC]
	private void NET_FixCodeSyncFromMaster(int _randomIndex, string _codeFromMaster)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("NET_FixCodeSyncFromMaster " + _codeFromMaster, "synccode");
		}
		FixCodeSyncFromMaster(_randomIndex, _codeFromMaster, _sendStatusReady: true, _sendFinishCastReady: false);
	}

	private void FixCodeSyncFromMaster(int _randomIndex, string _codeFromMaster, bool _sendStatusReady, bool _sendFinishCastReady, string _cardId = "")
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("FixCodeSyncFromMaster", "synccode");
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD(_codeFromMaster, "synccode");
		}
		SetRandomIndex(_randomIndex);
		if (_sendFinishCastReady && castingCardListMP.Count > 1)
		{
			NetworkManager.Instance.SetStatusReady("finishcast" + _cardId);
			return;
		}
		if (checkSyncCodeInCoop && _codeFromMaster != "")
		{
			try
			{
				_codeFromMaster = Functions.DecompressString(_codeFromMaster);
			}
			catch
			{
			}
			string text = GenerateSyncCode();
			if (GameManager.Instance.GetDeveloperMode())
			{
				Debug.Log("FIX CODE FROM MASTER -> " + _codeFromMaster);
				Debug.Log("MY FIX FALSE -> " + text);
			}
			bool flag = false;
			if (_codeFromMaster != text)
			{
				if (GameManager.Instance.GetDeveloperMode())
				{
					Debug.LogError("DIFFERENT CODES!!");
				}
				flag = true;
			}
			if (flag)
			{
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD(_codeFromMaster, "synccode");
				}
				string[] array = _codeFromMaster.Split('$');
				for (int i = 0; i < 8; i++)
				{
					string[] array2 = array[i].Split('|');
					Character character = null;
					character = ((i >= 4) ? ((Character)TeamNPC[i - 4]) : ((Character)TeamHero[i]));
					if (character == null || !character.Alive)
					{
						continue;
					}
					string[] array3 = array2[1].Split('_');
					character.HpCurrent = int.Parse(array3[0]);
					if (array3.Length > 1 && array3[1] != null)
					{
						character.Hp = int.Parse(array3[1]);
					}
					else
					{
						character.Hp = character.HpCurrent;
					}
					character.AuraList = new List<Aura>();
					if (array2.Length >= 4)
					{
						string[] array4 = array2[3].Split(':');
						if (array4.Length != 0)
						{
							for (int j = 0; j < array4.Length; j++)
							{
								Aura aura = new Aura();
								string[] array5 = array4[j].Split('_');
								if (array5.Length == 2)
								{
									aura.SetAura(AtOManager.Instance.GlobalAuraCurseModificationByTraitsAndItems("set", Globals.Instance.GetAuraCurseFromIndex(int.Parse(array5[0])).ToLower(), null, character), int.Parse(array5[1]));
									character.AuraList.Add(aura);
								}
							}
							character.UpdateAuraCurseFunctions();
						}
					}
					if (i < 4)
					{
						character.EnergyCurrent = int.Parse(array2[4]);
						character.HeroItem.DrawEnergy();
						if (array2[5] != "")
						{
							string[] array6 = array2[5].Split('%');
							HeroDeckDiscard[i] = new List<string>();
							for (int k = 0; k < array6.Length; k++)
							{
								if (array6[k] != "")
								{
									HeroDeckDiscard[i].Add(array6[k]);
								}
							}
							string[] array7 = array2[6].Split('%');
							HeroDeck[i] = new List<string>();
							for (int l = 0; l < array7.Length; l++)
							{
								if (array7[l] != "")
								{
									HeroDeck[i].Add(array7[l]);
								}
							}
							string[] array8 = array2[7].Split('%');
							HeroDeckVanish[i] = new List<string>();
							for (int m = 0; m < array8.Length; m++)
							{
								if (array8[m] != "")
								{
									HeroDeckVanish[i].Add(array8[m]);
								}
							}
						}
						string[] array9 = array2[10].Split(':');
						bool flag2 = false;
						if (array9 != null)
						{
							if (array9[0] != null && array9[0] != "")
							{
								character.AssignEnchantmentManual(array9[0], 0);
							}
							if (array9[1] != null && array9[1] != "")
							{
								character.AssignEnchantmentManual(array9[1], 1);
							}
							if (array9[2] != null && array9[2] != "")
							{
								character.AssignEnchantmentManual(array9[2], 2);
							}
							flag2 = true;
						}
						if (flag2)
						{
							character.HeroItem.ShowEnchantments();
						}
					}
					character.RoundMoved = int.Parse(array2[8]);
				}
				currentRound = int.Parse(array[8]);
				string[] array10 = array[9].Split('&');
				enchantmentExecutedTotal.Clear();
				if (array10 != null)
				{
					for (int n = 0; n < array10.Length; n++)
					{
						string[] array11 = array10[n].Split('%');
						if (array11 != null && array11.Length == 2 && array11[0] != null && array11[1] != null && int.Parse(array11[1]) > 0)
						{
							enchantmentExecutedTotal.Add(array11[0], int.Parse(array11[1]));
						}
					}
				}
			}
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("FixCodeSyncFromMaster ends", "synccode");
		}
		if (_sendStatusReady)
		{
			NetworkManager.Instance.SetStatusReady("FixingSyncCode");
		}
		if (_sendFinishCastReady)
		{
			NetworkManager.Instance.SetStatusReady("finishcast" + _cardId);
		}
	}

	public void RecalculateBossNpcRefs()
	{
		RecalculateDraculaReferences();
		RecalculateSirenQueenReferences();
		RecalculateFrankyReferences();
		RecalculatePhantomArmorReferences();
	}

	private void RecalculateDraculaReferences()
	{
		if (BossNpc != null && BossNpc is Dracula && BossNpc.npc.NPCItem == null)
		{
			NPC nPC = (from x in GetTeamNPC()
				where x != null && x.Alive && x.Id.StartsWith("count")
				select x).FirstOrDefault();
			if (nPC != null)
			{
				BossNpc.npc = nPC;
			}
		}
	}

	private void RecalculateFrankyReferences()
	{
		if (BossNpc != null && BossNpc is Frankenstein && BossNpc.npc.NPCItem == null)
		{
			NPC nPC = (from x in GetTeamNPC()
				where x != null && x.Alive && x.Id.StartsWith("franky")
				select x).FirstOrDefault();
			if (nPC != null)
			{
				BossNpc.npc = nPC;
			}
		}
	}

	private void RecalculatePhantomArmorReferences()
	{
		if (BossNpc != null && BossNpc is PhantomArmor && BossNpc.npc.NPCItem == null)
		{
			NPC nPC = (from x in GetTeamNPC()
				where x != null && x.Alive && x.Id.StartsWith("pa_d")
				select x).FirstOrDefault();
			if (nPC != null)
			{
				BossNpc.npc = nPC;
				((PhantomArmor)BossNpc).Initialize();
			}
		}
	}

	private void RecalculateSirenQueenReferences()
	{
		if (BossNpc == null || !(BossNpc is SirenQueen) || !(BossNpc.npc.NPCItem == null))
		{
			return;
		}
		NPC nPC = (from x in GetTeamNPC()
			where x != null && x.Alive && x.Id.StartsWith("s_queen")
			select x).FirstOrDefault();
		if (nPC != null)
		{
			BossNpc.npc = nPC;
			((SirenQueen)BossNpc).isEscaped = false;
			return;
		}
		SirenQueen sirenQueen = (SirenQueen)BossNpc;
		if (sirenQueen.hasAppeared)
		{
			sirenQueen.isEscaped = false;
			sirenQueen.isSpawning = false;
			sirenQueen.hasAppeared = false;
		}
	}

	private void FixCodeSyncFromMasterTOTAL(int _randomIndex, string codeFromMaster)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("FixCodeSyncFromMaster", "synccode");
		}
		foreach (Transform item in GO_NPCs.transform)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		StartCoroutine(FixTOTALCo(_randomIndex, codeFromMaster));
	}

	private IEnumerator FixTOTALCo(int _randomIndex, string codeFromMaster)
	{
		eventList.Add("FixingTotalCo");
		string[] codeArr = codeFromMaster.Split('$');
		SetRandomIndex(int.Parse(codeArr[11]));
		currentRound = int.Parse(codeArr[8]);
		for (int i = 0; i < 8; i++)
		{
			string[] array = codeArr[i].Split('|');
			if (array.Length >= 9 && int.Parse(array[8]) == currentRound)
			{
				roundBegan = true;
			}
		}
		string[] array2 = codeArr[9].Split('&');
		enchantmentExecutedTotal.Clear();
		for (int j = 0; j < array2.Length; j++)
		{
			string[] array3 = array2[j].Split('%');
			if (array3 != null && array3.Length == 2 && array3[0] != null && array3[1] != null && int.Parse(array3[1]) > 0)
			{
				enchantmentExecutedTotal.Add(array3[0], int.Parse(array3[1]));
			}
		}
		for (int k = 0; k < 8; k++)
		{
			string[] aux = codeArr[k].Split('|');
			Character theChar = ((k >= 4) ? ((Character)TeamNPC[k - 4]) : ((Character)TeamHero[k]));
			if (theChar == null && (aux.Length < 7 || !(aux[8] != "")))
			{
				continue;
			}
			if (aux.Length == 1)
			{
				theChar.Alive = false;
				theChar.HpCurrent = 0;
				if (k < 4)
				{
					UnityEngine.Object.Destroy(GO_Heroes.transform.GetChild(k).gameObject);
				}
				else if (backgroundActive == "Sahti_Rust" && theChar.Id.StartsWith("pitt"))
				{
					DoSahtiRustBackground(showPittAlive: false);
				}
				continue;
			}
			if (k >= 4)
			{
				CreateNPC(Globals.Instance.GetNPC(aux[9]), "", k - 4, generateFromReload: true, aux[10]);
				yield return Globals.Instance.WaitForSeconds(0.2f);
				theChar = TeamNPC[k - 4];
				theChar.Corruption = aux[11];
			}
			string[] array4 = aux[1].Split('_');
			theChar.HpCurrent = int.Parse(array4[0]);
			if (array4.Length > 1 && array4[1] != null)
			{
				theChar.Hp = int.Parse(array4[1]);
			}
			else
			{
				theChar.Hp = theChar.HpCurrent;
			}
			theChar.AuraList = new List<Aura>();
			if (aux.Length >= 4)
			{
				string[] array5 = aux[3].Split(':');
				for (int l = 0; l < array5.Length; l++)
				{
					Aura aura = new Aura();
					string[] array6 = array5[l].Split('_');
					if (array6.Length == 2)
					{
						aura.SetAura(AtOManager.Instance.GlobalAuraCurseModificationByTraitsAndItems("set", Globals.Instance.GetAuraCurseFromIndex(int.Parse(array6[0])).ToLower(), null, theChar), int.Parse(array6[1]));
						theChar.AuraList.Add(aura);
					}
				}
				theChar.UpdateAuraCurseFunctions();
			}
			if (k < 4)
			{
				theChar.EnergyCurrent = int.Parse(aux[4]);
				theChar.HeroItem.DrawEnergy();
				string[] array7 = aux[5].Split('%');
				HeroDeckDiscard[k] = new List<string>();
				for (int m = 0; m < array7.Length; m++)
				{
					if (array7[m] != "")
					{
						HeroDeckDiscard[k].Add(array7[m]);
					}
				}
				string[] array8 = aux[6].Split('%');
				HeroDeck[k] = new List<string>();
				for (int n = 0; n < array8.Length; n++)
				{
					if (array8[n] != "")
					{
						HeroDeck[k].Add(array8[n]);
					}
				}
				string[] array9 = aux[7].Split('%');
				HeroDeckVanish[k] = new List<string>();
				for (int num = 0; num < array9.Length; num++)
				{
					if (array9[num] != "")
					{
						HeroDeckVanish[k].Add(array9[num]);
					}
				}
				string[] array10 = aux[10].Split(':');
				bool flag = false;
				if (array10 != null)
				{
					if (array10[0] != null && array10[0] != "")
					{
						theChar.AssignEnchantmentManual(array10[0], 0);
					}
					if (array10[1] != null && array10[1] != "")
					{
						theChar.AssignEnchantmentManual(array10[1], 1);
					}
					if (array10[2] != null && array10[2] != "")
					{
						theChar.AssignEnchantmentManual(array10[2], 2);
					}
					flag = true;
				}
				if (flag)
				{
					theChar.HeroItem.ShowEnchantments();
				}
			}
			else
			{
				string[] array11 = aux[4].Split('%');
				if (NPCDeck[k - 4] != null)
				{
					NPCDeck[k - 4].Clear();
					for (int num2 = 0; num2 < array11.Length; num2++)
					{
						if (array11[num2] != "")
						{
							NPCDeck[k - 4].Add(array11[num2]);
						}
					}
				}
				string[] array12 = aux[5].Split('%');
				if (NPCDeckDiscard[k - 4] != null)
				{
					NPCDeckDiscard[k - 4].Clear();
					for (int num3 = 0; num3 < array12.Length; num3++)
					{
						if (array12[num3] != "")
						{
							NPCDeckDiscard[k - 4].Add(array12[num3]);
						}
					}
				}
				string[] array13 = aux[6].Split('%');
				if (NPCHand != null && NPCHand[k - 4] == null)
				{
					NPCHand[k - 4] = new List<string>();
				}
				if (NPCHand != null && NPCHand[k - 4] != null)
				{
					NPCHand[k - 4].Clear();
					for (int num4 = 0; num4 < array13.Length; num4++)
					{
						if (array13[num4] != "")
						{
							NPCHand[k - 4].Add(array13[num4]);
						}
					}
					if (NPCHand[k - 4].Count > 0)
					{
						yield return Globals.Instance.WaitForSeconds(0.1f);
						theChar.CreateOverDeck(getCardFromDeck: false);
					}
				}
				string[] array14 = aux[7].Split('%');
				if (npcCardsCasted == null)
				{
					npcCardsCasted = new Dictionary<string, List<string>>();
				}
				if (!npcCardsCasted.ContainsKey(aux[10]))
				{
					npcCardsCasted.Add(aux[10], new List<string>());
				}
				for (int num5 = 0; num5 < array14.Length; num5++)
				{
					if (array14[num5] != "")
					{
						npcCardsCasted[aux[10]].Add(array14[num5]);
					}
				}
				string[] array15 = aux[12].Split(':');
				bool flag2 = false;
				if (array15 != null)
				{
					if (array15[0] != null && array15[0] != "")
					{
						theChar.AssignEnchantmentManual(array15[0], 0);
					}
					if (array15[1] != null && array15[1] != "")
					{
						theChar.AssignEnchantmentManual(array15[1], 1);
					}
					if (array15[2] != null && array15[2] != "")
					{
						theChar.AssignEnchantmentManual(array15[2], 2);
					}
					flag2 = true;
				}
				if (flag2)
				{
					theChar.NPCItem.ShowEnchantments();
				}
			}
			theChar.RoundMoved = int.Parse(aux[8]);
		}
		if (codeArr.Length > 12)
		{
			string[] array16 = codeArr[12].Split('%');
			scarabSpawned = array16[0];
			scarabSuccess = array16[1];
		}
		eventList.Remove("FixingTotalCo");
		gotHeroDeck = true;
		gotNPCDeck = true;
		gotDictionary = true;
		RestoreCombatStats();
		backingDictionary = true;
		RestoreCardDictionary();
		List<NPCState> list = npcParamsFromTurnSave;
		if (list != null && list.Count > 0)
		{
			ApplyNPCState();
		}
		while (backingDictionary)
		{
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			Instance.RecalculateBossNpcRefs();
		}
		if (!GameManager.Instance.IsMultiplayer())
		{
			StartCoroutine(BeginMatch());
		}
		else if (NetworkManager.Instance.IsMaster())
		{
			NET_ShareDecks(ContinueNewGame: true);
		}
	}

	private void ApplyNPCState()
	{
		Dictionary<string, NPC> dictionary = TeamNPC.Where((NPC n) => n != null).ToDictionary((NPC n) => n.InternalId);
		foreach (NPCState state in npcParamsFromTurnSave)
		{
			NPC nPC = dictionary.Values.FirstOrDefault((NPC n) => n.Id.StartsWith(state.Id, StringComparison.CurrentCultureIgnoreCase));
			if (nPC != null)
			{
				nPC.IsIllusion = state.IsIllusion;
				nPC.IsIllusionExposed = state.IsIllusionExposed;
				nPC.IllusionCharacter = (string.IsNullOrEmpty(state.IllusionCharacterId) ? null : dictionary.Values.FirstOrDefault((NPC n) => n.Id.Contains(state.IllusionCharacterId, StringComparison.InvariantCultureIgnoreCase)));
			}
		}
	}

	private void StoreCombatStats()
	{
		for (int i = 0; i < AtOManager.Instance.combatStats.GetLength(0); i++)
		{
			for (int j = 0; j < AtOManager.Instance.combatStats.GetLength(1); j++)
			{
				combatStatsAux[i, j] = AtOManager.Instance.combatStats[i, j];
			}
		}
		for (int k = 0; k < AtOManager.Instance.combatStatsCurrent.GetLength(0); k++)
		{
			for (int l = 0; l < AtOManager.Instance.combatStatsCurrent.GetLength(1); l++)
			{
				combatStatsCurrentAux[k, l] = AtOManager.Instance.combatStatsCurrent[k, l];
			}
		}
		combatStatsDictAux = new Dictionary<string, Dictionary<string, List<string>>>(combatStatsDict);
	}

	private void RestoreCombatStats()
	{
		if (combatStatsDictAux == null)
		{
			return;
		}
		if (combatStatsAux != null)
		{
			for (int i = 0; i < combatStatsAux.GetLength(0); i++)
			{
				for (int j = 0; j < combatStatsAux.GetLength(1); j++)
				{
					AtOManager.Instance.combatStats[i, j] = combatStatsAux[i, j];
				}
			}
		}
		if (combatStatsCurrentAux != null)
		{
			for (int k = 0; k < combatStatsCurrentAux.GetLength(0); k++)
			{
				for (int l = 0; l < combatStatsCurrentAux.GetLength(1); l++)
				{
					AtOManager.Instance.combatStatsCurrent[k, l] = combatStatsCurrentAux[k, l];
				}
			}
		}
		combatStatsDict = new Dictionary<string, Dictionary<string, List<string>>>(combatStatsDictAux);
		combatStatsAux = new int[AtOManager.Instance.combatStats.GetLength(0), AtOManager.Instance.combatStats.GetLength(1)];
		combatStatsCurrentAux = new int[AtOManager.Instance.combatStatsCurrent.GetLength(0), AtOManager.Instance.combatStatsCurrent.GetLength(1)];
	}

	public string GetCombatStatsCurrentForTurnSave()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < AtOManager.Instance.combatStatsCurrent.GetLength(0); i++)
		{
			for (int j = 0; j < AtOManager.Instance.combatStatsCurrent.GetLength(1); j++)
			{
				stringBuilder.Append(AtOManager.Instance.combatStatsCurrent[i, j]);
				stringBuilder.Append("_");
			}
			stringBuilder.Append("|");
		}
		return Functions.CompressString(stringBuilder.ToString());
	}

	public void SetCombatStatsCurrentForTurnSave(string _stats)
	{
		if (_stats == "")
		{
			return;
		}
		_stats = Functions.DecompressString(_stats);
		string[] array = _stats.Split('|');
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split('_');
			for (int j = 0; j < array2.Length; j++)
			{
				if (array2[j] != "")
				{
					AtOManager.Instance.combatStatsCurrent[i, j] = int.Parse(array2[j]);
				}
			}
		}
	}

	public string GetCombatStatsForTurnSave()
	{
		StringBuilder stringBuilder = new StringBuilder();
		string text = "";
		foreach (KeyValuePair<string, Dictionary<string, List<string>>> item in combatStatsDict)
		{
			foreach (KeyValuePair<string, List<string>> item2 in item.Value)
			{
				stringBuilder.Append(item.Key);
				stringBuilder.Append("&");
				stringBuilder.Append(item2.Key);
				stringBuilder.Append("|");
				int num = 1;
				string text2 = "";
				for (int i = 0; i < item2.Value.Count; i++)
				{
					if (text2 != item2.Value[i])
					{
						if (text2 != "")
						{
							stringBuilder.Append("*");
							stringBuilder.Append(num);
							stringBuilder.Append("+");
							num = 1;
						}
						stringBuilder.Append(item2.Value[i]);
						text2 = item2.Value[i];
					}
					else
					{
						num++;
					}
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
		{
			return;
		}
		_stats = Functions.DecompressString(_stats);
		combatStatsDict = new Dictionary<string, Dictionary<string, List<string>>>();
		combatStatsAux = new int[AtOManager.Instance.combatStats.GetLength(0), AtOManager.Instance.combatStats.GetLength(1)];
		combatStatsCurrentAux = new int[AtOManager.Instance.combatStatsCurrent.GetLength(0), AtOManager.Instance.combatStatsCurrent.GetLength(1)];
		string[] array = _stats.Split('/');
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split('&');
			if (array2.Length != 2)
			{
				continue;
			}
			if (!combatStatsDict.ContainsKey(array2[0]))
			{
				combatStatsDict.Add(array2[0], new Dictionary<string, List<string>>());
			}
			string[] array3 = array2[1].Split('|');
			if (array3.Length != 2)
			{
				continue;
			}
			if (!combatStatsDict[array2[0]].ContainsKey(array3[0]))
			{
				combatStatsDict[array2[0]].Add(array3[0], new List<string>());
			}
			string[] array4 = array3[1].Split('+');
			for (int j = 0; j < array4.Length; j++)
			{
				string[] array5 = array4[j].Split('*');
				for (int k = 0; k < int.Parse(array5[1]); k++)
				{
					combatStatsDict[array2[0]][array3[0]].Add(array5[0]);
				}
			}
		}
	}

	public string GetHeroLifeArrForTurnSave()
	{
		return JsonHelper.ToJson(heroLifeArr);
	}

	public void SetHeroLifeArrForTurnSave(string _life)
	{
		if (!(_life == ""))
		{
			heroLifeArr = JsonHelper.FromJson<int>(_life);
		}
	}

	public string GetItemExecutionInCombatForTurnSave()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<string, int> item in itemExecutedInCombat)
		{
			stringBuilder.Append(item.Key);
			stringBuilder.Append(":");
			stringBuilder.Append(item.Value);
			stringBuilder.Append("|");
		}
		return Functions.CompressString(stringBuilder.ToString());
	}

	public void SetItemExecutionInCombatForTurnSave(string _itemExecution)
	{
		if (_itemExecution == "" || _itemExecution == null)
		{
			return;
		}
		_itemExecution = Functions.DecompressString(_itemExecution);
		if (_itemExecution == "")
		{
			return;
		}
		itemExecutedInCombat.Clear();
		string[] array = _itemExecution.Split('|');
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(':');
			if (array2.Length == 2)
			{
				itemExecutedInCombat.Add(array2[0], int.Parse(array2[1]));
			}
		}
		ItemExecuteForThisCombatDuplicate();
	}

	public void SetCurrentSpecialCardsUsedInMatchForTurnSave(string currentSpecialCardsUsedInMatch)
	{
		int result;
		bool flag = int.TryParse(currentSpecialCardsUsedInMatch, out result);
		MindSpikeAbility.CurrentSpecialCardsUsedInMatch = (flag ? result : 0);
	}

	public void ConsumeStatusForCombatStats(string _target, string _status, int _charges)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("ConsumeStatusForCombatStats " + _target + " " + _status + " " + _charges, "trace");
		}
		_status = _status.ToLower();
		if (!combatStatsDict.ContainsKey(_target) || !combatStatsDict[_target].ContainsKey(_status))
		{
			return;
		}
		if (_charges == -1)
		{
			combatStatsDict[_target].Remove(_status);
			return;
		}
		if (_charges >= combatStatsDict[_target][_status].Count)
		{
			combatStatsDict[_target].Remove(_status);
		}
		for (int i = 0; i < _charges; i++)
		{
			if (combatStatsDict[_target].ContainsKey(_status) && combatStatsDict[_target][_status] != null && combatStatsDict[_target][_status].Count > 0)
			{
				combatStatsDict[_target][_status].RemoveAt(0);
			}
		}
	}

	public void StoreStatusForCombatStats(string _target, string _status, string _caster, int _charges)
	{
		_status = _status.ToLower();
		if (!combatStatsDict.ContainsKey(_target))
		{
			combatStatsDict.Add(_target, new Dictionary<string, List<string>>());
		}
		if (!combatStatsDict[_target].ContainsKey(_status))
		{
			combatStatsDict[_target].Add(_status, new List<string>());
		}
		for (int i = 0; i < _charges; i++)
		{
			combatStatsDict[_target][_status].Add(_caster);
		}
	}

	public void DamageStatusFromCombatStats(string _target, string _status, int _damage)
	{
		_status = _status.ToLower();
		if (!combatStatsDict.ContainsKey(_target) || !combatStatsDict[_target].ContainsKey(_status))
		{
			return;
		}
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		int count = combatStatsDict[_target][_status].Count;
		for (int i = 0; i < count; i++)
		{
			string key = combatStatsDict[_target][_status][i];
			if (dictionary.ContainsKey(key))
			{
				dictionary[key]++;
			}
			else
			{
				dictionary.Add(key, 1);
			}
		}
		Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
		int num = 0;
		foreach (KeyValuePair<string, int> item in dictionary)
		{
			if (item.Key.Split('_').Length == 0)
			{
				continue;
			}
			int heroFromId = GetHeroFromId(item.Key);
			if (heroFromId > -1)
			{
				float num2 = (float)item.Value / (float)count;
				int num3 = Functions.FuncRoundToInt((float)_damage * num2);
				if (num + num3 > _damage)
				{
					num3 = _damage - num;
				}
				dictionary2.Add(heroFromId, num3);
				num += num3;
			}
		}
		bool flag = true;
		foreach (KeyValuePair<int, int> item2 in dictionary2)
		{
			int num4 = 0;
			num4 = ((!flag || num >= _damage) ? item2.Value : (item2.Value + (_damage - num)));
			if (_status != "regeneration" && _status != "sanctify")
			{
				AtOManager.Instance.combatStats[item2.Key, 0] += num4;
				AtOManager.Instance.combatStatsCurrent[item2.Key, 0] += num4;
			}
			else
			{
				AtOManager.Instance.combatStats[item2.Key, 3] += num4;
				AtOManager.Instance.combatStatsCurrent[item2.Key, 3] += num4;
			}
			flag = false;
			if (AtOManager.Instance.combatStats.GetLength(1) > 10)
			{
				switch (_status)
				{
				case "bleed":
					AtOManager.Instance.combatStats[item2.Key, 6] += num4;
					AtOManager.Instance.combatStatsCurrent[item2.Key, 6] += num4;
					break;
				case "burn":
					AtOManager.Instance.combatStats[item2.Key, 7] += num4;
					AtOManager.Instance.combatStatsCurrent[item2.Key, 7] += num4;
					break;
				case "dark":
					AtOManager.Instance.combatStats[item2.Key, 8] += num4;
					AtOManager.Instance.combatStatsCurrent[item2.Key, 8] += num4;
					break;
				case "poison":
					AtOManager.Instance.combatStats[item2.Key, 9] += num4;
					AtOManager.Instance.combatStatsCurrent[item2.Key, 9] += num4;
					break;
				case "spark":
					AtOManager.Instance.combatStats[item2.Key, 10] += num4;
					AtOManager.Instance.combatStatsCurrent[item2.Key, 10] += num4;
					break;
				case "thorns":
					AtOManager.Instance.combatStats[item2.Key, 11] += num4;
					AtOManager.Instance.combatStatsCurrent[item2.Key, 11] += num4;
					break;
				}
			}
		}
	}

	public string GenerateSyncCodeForCheckingAction()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(eventList.Count);
		stringBuilder.Append(randomIndex);
		stringBuilder.Append(randomItemsIndex);
		stringBuilder.Append(randomTraitsIndex);
		stringBuilder.Append(randomDeckIndex);
		stringBuilder.Append(ctQueue.Count);
		for (int i = 0; i < TeamHero.Length; i++)
		{
			if (TeamHero[i] == null || TeamHero[i].HeroData == null || !TeamHero[i].Alive)
			{
				continue;
			}
			stringBuilder.Append(TeamHero[i].HpCurrent);
			stringBuilder.Append(TeamHero[i].Hp);
			if (TeamHero[i].AuraList != null)
			{
				int count = TeamHero[i].AuraList.Count;
				stringBuilder.Append(count);
				for (int j = 0; j < count; j++)
				{
					if (TeamHero[i].AuraList[j] != null && TeamHero[i].AuraList[j].ACData != null)
					{
						stringBuilder.Append(TeamHero[i].AuraList[j].ACData.Id);
						stringBuilder.Append(TeamHero[i].AuraList[j].AuraCharges);
					}
				}
			}
			stringBuilder.Append(TeamHero[i].EnergyCurrent);
			stringBuilder.Append(TeamHero[i].Enchantment);
			stringBuilder.Append(TeamHero[i].Enchantment2);
			stringBuilder.Append(TeamHero[i].Enchantment3);
		}
		for (int k = 0; k < TeamNPC.Length; k++)
		{
			if (TeamNPC[k] == null || !TeamNPC[k].Alive)
			{
				continue;
			}
			stringBuilder.Append(TeamNPC[k].HpCurrent);
			stringBuilder.Append(TeamNPC[k].Hp);
			if (TeamNPC[k].AuraList == null)
			{
				continue;
			}
			int count2 = TeamNPC[k].AuraList.Count;
			stringBuilder.Append(count2);
			for (int l = 0; l < count2; l++)
			{
				if (TeamNPC[k].AuraList[l] != null && TeamNPC[k].AuraList[l].ACData != null)
				{
					stringBuilder.Append(TeamNPC[k].AuraList[l].ACData.Id);
					stringBuilder.Append(TeamNPC[k].AuraList[l].AuraCharges);
				}
			}
		}
		foreach (KeyValuePair<string, int> item in enchantmentExecutedTotal)
		{
			stringBuilder.Append(item.Key);
			stringBuilder.Append(item.Value);
		}
		return stringBuilder.ToString();
	}

	public string GenerateSyncCode(bool fixDeck = false)
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("Generate code " + fixDeck, "synccode");
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (TeamHero == null)
		{
			Debug.LogError("GenerateSyncCode ERROR: TeamHero array is NULL.");
			return "";
		}
		for (int i = 0; i < TeamHero.Length; i++)
		{
			stringBuilder.Append("H_");
			stringBuilder.Append(i);
			Hero hero = TeamHero[i];
			if (hero == null)
			{
				Debug.LogError($"GenerateSyncCode WARNING: TeamHero[{i}] is NULL.");
				stringBuilder.Append("$");
				continue;
			}
			if (hero.HeroData == null)
			{
				Debug.LogError($"GenerateSyncCode WARNING: TeamHero[{i}].HeroData is NULL.");
				stringBuilder.Append("$");
				continue;
			}
			if (!hero.Alive)
			{
				stringBuilder.Append("$");
				continue;
			}
			stringBuilder.Append("|");
			stringBuilder.Append(hero.HpCurrent);
			stringBuilder.Append("_");
			stringBuilder.Append(hero.Hp);
			stringBuilder.Append("|");
			if (HeroDeck == null)
			{
				Debug.LogError("GenerateSyncCode ERROR: HeroDeck is NULL.");
			}
			if (HeroDeckDiscard == null)
			{
				Debug.LogError("GenerateSyncCode ERROR: HeroDeckDiscard is NULL.");
			}
			if (HeroHand == null)
			{
				Debug.LogError("GenerateSyncCode ERROR: HeroHand is NULL.");
			}
			if (HeroDeckVanish == null)
			{
				Debug.LogError("GenerateSyncCode ERROR: HeroDeckVanish is NULL.");
			}
			stringBuilder.Append((HeroDeck != null && HeroDeck[i] != null) ? HeroDeck[i].Count : 0);
			stringBuilder.Append("_");
			stringBuilder.Append((HeroDeckDiscard != null && HeroDeckDiscard[i] != null) ? HeroDeckDiscard[i].Count : 0);
			stringBuilder.Append("_");
			stringBuilder.Append((HeroHand != null && HeroHand[i] != null) ? HeroHand[i].Count : 0);
			stringBuilder.Append("|");
			if (hero.AuraList == null)
			{
				Debug.LogError($"GenerateSyncCode ERROR: TeamHero[{i}].AuraList is NULL.");
				stringBuilder.Append("|");
			}
			else
			{
				foreach (Aura aura in hero.AuraList)
				{
					if (aura == null)
					{
						Debug.LogError($"GenerateSyncCode WARNING: Null aura found in TeamHero[{i}].AuraList.");
						continue;
					}
					if (aura.ACData == null)
					{
						Debug.LogError($"GenerateSyncCode WARNING: Aura with null ACData in TeamHero[{i}].AuraList.");
						continue;
					}
					stringBuilder.Append(Globals.Instance.GetAuraCurseIndex(aura.ACData.Id.ToLower()));
					stringBuilder.Append("_");
					stringBuilder.Append(aura.AuraCharges);
					stringBuilder.Append(":");
				}
				stringBuilder.Append("|");
			}
			stringBuilder.Append(hero.EnergyCurrent);
			stringBuilder.Append("|");
			if (fixDeck)
			{
				if (HeroDeckDiscard != null && HeroDeckDiscard[i] != null)
				{
					foreach (string item in HeroDeckDiscard[i])
					{
						stringBuilder.Append(item + "%");
					}
				}
				else
				{
					Debug.LogError($"GenerateSyncCode WARNING: HeroDeckDiscard[{i}] is NULL when fixDeck=true.");
				}
			}
			stringBuilder.Append("|");
			if (fixDeck)
			{
				if (HeroDeck != null && HeroDeck[i] != null)
				{
					foreach (string item2 in HeroDeck[i])
					{
						stringBuilder.Append(item2 + "%");
					}
				}
				else
				{
					Debug.LogError($"GenerateSyncCode WARNING: HeroDeck[{i}] is NULL when fixDeck=true.");
				}
			}
			stringBuilder.Append("|");
			if (fixDeck)
			{
				if (HeroDeckVanish != null && HeroDeckVanish[i] != null)
				{
					foreach (string item3 in HeroDeckVanish[i])
					{
						stringBuilder.Append(item3 + "%");
					}
				}
				else
				{
					Debug.LogError($"GenerateSyncCode WARNING: HeroDeckVanish[{i}] is NULL when fixDeck=true.");
				}
			}
			stringBuilder.Append("|");
			stringBuilder.Append(hero.RoundMoved);
			stringBuilder.Append("|");
			if (hero.HeroItem != null)
			{
				stringBuilder.Append(hero.HeroItem.gameObject.name);
			}
			else
			{
				stringBuilder.Append("");
			}
			stringBuilder.Append("|");
			stringBuilder.Append(hero.Enchantment);
			stringBuilder.Append(":");
			stringBuilder.Append(hero.Enchantment2);
			stringBuilder.Append(":");
			stringBuilder.Append(hero.Enchantment3);
			stringBuilder.Append("$");
		}
		if (TeamNPC == null)
		{
			Debug.LogError("GenerateSyncCode ERROR: TeamNPC array is NULL.");
			return "";
		}
		for (int j = 0; j < TeamNPC.Length; j++)
		{
			stringBuilder.Append("N_");
			stringBuilder.Append(j);
			NPC nPC = TeamNPC[j];
			if (nPC == null)
			{
				Debug.LogWarning($"GenerateSyncCode WARNING: TeamNPC[{j}] is NULL.");
				stringBuilder.Append("$");
				continue;
			}
			if (!nPC.Alive)
			{
				stringBuilder.Append("$");
				continue;
			}
			stringBuilder.Append("|");
			stringBuilder.Append(nPC.HpCurrent);
			stringBuilder.Append("_");
			stringBuilder.Append(nPC.Hp);
			stringBuilder.Append("|0|");
			if (nPC.AuraList == null)
			{
				Debug.LogError($"GenerateSyncCode ERROR: NPC[{j}].AuraList is NULL.");
				stringBuilder.Append("|");
			}
			else
			{
				foreach (Aura aura2 in nPC.AuraList)
				{
					if (aura2 == null)
					{
						Debug.LogError($"GenerateSyncCode WARNING: Null aura in NPC[{j}].AuraList.");
						continue;
					}
					if (aura2.ACData == null)
					{
						Debug.LogError($"GenerateSyncCode WARNING: Aura with null ACData in NPC[{j}].AuraList.");
						continue;
					}
					stringBuilder.Append(Globals.Instance.GetAuraCurseIndex(aura2.ACData.Id.ToLower()));
					stringBuilder.Append("_");
					stringBuilder.Append(aura2.AuraCharges);
					stringBuilder.Append(":");
				}
				stringBuilder.Append("|");
			}
			if (fixDeck)
			{
				if (NPCDeck == null || NPCDeck[j] == null)
				{
					Debug.LogError($"GenerateSyncCode WARNING: NPCDeck[{j}] is NULL when fixDeck=true.");
				}
				else
				{
					foreach (string item4 in NPCDeck[j])
					{
						stringBuilder.Append(item4 + "%");
					}
				}
			}
			stringBuilder.Append("|");
			if (fixDeck)
			{
				if (NPCDeckDiscard != null && NPCDeckDiscard[j] != null)
				{
					foreach (string item5 in NPCDeckDiscard[j])
					{
						stringBuilder.Append(item5 + "%");
					}
				}
				else
				{
					Debug.LogError($"GenerateSyncCode WARNING: NPCDeckDiscard[{j}] is NULL when fixDeck=true.");
				}
			}
			stringBuilder.Append("|");
			if (fixDeck)
			{
				if (NPCHand != null && NPCHand[j] != null)
				{
					foreach (string item6 in NPCHand[j])
					{
						stringBuilder.Append(item6 + "%");
					}
				}
				else
				{
					Debug.LogError($"GenerateSyncCode WARNING: NPCHand[{j}] is NULL when fixDeck=true.");
				}
			}
			stringBuilder.Append("|");
			if (fixDeck)
			{
				if (npcCardsCasted != null && npcCardsCasted.ContainsKey(nPC.InternalId) && npcCardsCasted[nPC.InternalId] != null)
				{
					foreach (string item7 in npcCardsCasted[nPC.InternalId])
					{
						stringBuilder.Append(item7 + "%");
					}
				}
				else
				{
					Debug.LogWarning("GenerateSyncCode WARNING: npcCardsCasted missing or NULL for npc.InternalId = " + nPC.InternalId);
				}
			}
			stringBuilder.Append("|");
			stringBuilder.Append(nPC.RoundMoved);
			stringBuilder.Append("|");
			if (nPC.NpcData == null)
			{
				Debug.LogWarning($"GenerateSyncCode ERROR: TeamNPC[{j}].NpcData is NULL.");
			}
			else
			{
				stringBuilder.Append(nPC.NpcData.Id);
			}
			stringBuilder.Append("|");
			stringBuilder.Append(nPC.InternalId);
			stringBuilder.Append("|");
			stringBuilder.Append(nPC.Corruption);
			stringBuilder.Append("|");
			stringBuilder.Append(nPC.Enchantment);
			stringBuilder.Append(":");
			stringBuilder.Append(nPC.Enchantment2);
			stringBuilder.Append(":");
			stringBuilder.Append(nPC.Enchantment3);
			stringBuilder.Append("$");
		}
		stringBuilder.Append(currentRound);
		stringBuilder.Append("$");
		if (enchantmentExecutedTotal == null)
		{
			Debug.LogError("GenerateSyncCode ERROR: enchantmentExecutedTotal dictionary is NULL.");
			stringBuilder.Append("$");
		}
		else
		{
			foreach (KeyValuePair<string, int> item8 in enchantmentExecutedTotal)
			{
				stringBuilder.Append(item8.Key);
				stringBuilder.Append("%");
				stringBuilder.Append(item8.Value);
				stringBuilder.Append("&");
			}
			stringBuilder.Append("$");
		}
		try
		{
			stringBuilder.Append(CardNamesForSyncCode());
		}
		catch (Exception ex)
		{
			Debug.LogError("GenerateSyncCode ERROR: CardNamesForSyncCode() failed: " + ex);
		}
		stringBuilder.Append("$");
		stringBuilder.Append(randomIndex);
		stringBuilder.Append("$");
		stringBuilder.Append(scarabSpawned);
		stringBuilder.Append("%");
		stringBuilder.Append(scarabSuccess);
		stringBuilder.Append("%");
		if (BossNpc != null && BossNpc is SirenQueen)
		{
			SirenQueen sirenQueen = (SirenQueen)BossNpc;
			if (sirenQueen.isEscaped)
			{
				sirenQueen.queenStateData.Save(sirenQueen.npc, sirenQueen.npc.HpCurrent, sirenQueen.npc.AuraList, sirenQueen.npc.Enchantment, sirenQueen.npc.Enchantment2, sirenQueen.npc.Enchantment3);
				sirenQueen.loadDataOnNextSpawn = true;
			}
		}
		return stringBuilder.ToString();
	}

	public void DestroyedItemInThisTurn(int _charIndex, string _cardId)
	{
		if (!heroDestroyedItemsInThisTurn.ContainsKey(_charIndex))
		{
			heroDestroyedItemsInThisTurn.Add(_charIndex, new Dictionary<string, string>());
		}
		CardData cardData = Globals.Instance.GetCardData(_cardId, instantiate: false);
		string key = Enum.GetName(typeof(Enums.CardType), cardData.CardType).ToLower();
		if (!heroDestroyedItemsInThisTurn[_charIndex].ContainsKey(key))
		{
			heroDestroyedItemsInThisTurn[_charIndex].Add(key, cardData.Item.Id);
		}
	}

	public void CleanPrePostDamageDictionary(string _id)
	{
		if (prePostDamageDictionary.ContainsKey(_id))
		{
			prePostDamageDictionary.Remove(_id);
		}
	}

	public void SetCardActive(CardData _cardActive)
	{
		cardActive = _cardActive;
	}

	public void ShowCombatKeyboardByConfig()
	{
		if (GameManager.Instance.ConfigKeyboardShortcuts)
		{
			ShowCombatKeyboard(_state: true);
		}
		else
		{
			ShowCombatKeyboard(_state: false);
		}
	}

	public void ShowCombatKeyboard(bool _state)
	{
		if (_state && !IsYourTurn())
		{
			_state = false;
		}
		if (_state)
		{
			if (preCastNum > -1)
			{
				int num = 1;
				for (int num2 = 3; num2 >= 0; num2--)
				{
					if (TeamHero[num2] != null && TeamHero[num2].Alive && TeamHero[num2].HeroItem != null)
					{
						Transform targetByNum = GetTargetByNum(num);
						if (targetByNum != null && cardItemTable[preCastNum - 1] != null)
						{
							if (CheckTarget(targetByNum, cardItemTable[preCastNum - 1].CardData))
							{
								TeamHero[num2].HeroItem.ShowKeyNum(_state: true, num.ToString());
							}
							else
							{
								TeamHero[num2].HeroItem.ShowKeyNum(_state: true, num.ToString(), _disabled: true);
							}
						}
						num++;
					}
				}
				for (int i = 0; i < 4; i++)
				{
					if (TeamNPC[i] == null || !TeamNPC[i].Alive || !(TeamNPC[i].NPCItem != null))
					{
						continue;
					}
					Transform targetByNum = GetTargetByNum(num);
					if (targetByNum != null && cardItemTable[preCastNum - 1] != null)
					{
						if (CheckTarget(targetByNum, cardItemTable[preCastNum - 1].CardData))
						{
							TeamNPC[i].NPCItem.ShowKeyNum(_state: true, num.ToString());
						}
						else
						{
							TeamNPC[i].NPCItem.ShowKeyNum(_state: true, num.ToString(), _disabled: true);
						}
					}
					num++;
				}
				if (cardItemTable == null)
				{
					return;
				}
				for (int j = 0; j < cardItemTable.Count; j++)
				{
					if (cardItemTable[j] != null)
					{
						cardItemTable[j].ShowKeyNum(_state: false);
					}
				}
				return;
			}
			int num3 = 1;
			if (cardItemTable != null)
			{
				for (int k = 0; k < cardItemTable.Count; k++)
				{
					if (cardItemTable[k] != null)
					{
						if (cardItemTable[k].IsPlayable())
						{
							cardItemTable[k].ShowKeyNum(_state: true, num3.ToString());
						}
						else
						{
							cardItemTable[k].ShowKeyNum(_state: true, num3.ToString(), _disabled: true);
						}
						num3++;
					}
				}
			}
			for (int l = 0; l < 4; l++)
			{
				if (TeamHero[l] != null && TeamHero[l].Alive && TeamHero[l].HeroItem != null)
				{
					TeamHero[l].HeroItem.ShowKeyNum(_state: false);
				}
			}
			for (int m = 0; m < 4; m++)
			{
				if (TeamNPC[m] != null && TeamNPC[m].Alive && TeamNPC[m].NPCItem != null)
				{
					TeamNPC[m].NPCItem.ShowKeyNum(_state: false);
				}
			}
			return;
		}
		if (cardItemTable != null)
		{
			for (int n = 0; n < cardItemTable.Count; n++)
			{
				if (cardItemTable[n] != null)
				{
					cardItemTable[n].ShowKeyNum(_state: false);
				}
			}
		}
		for (int num4 = 0; num4 < 4; num4++)
		{
			if (TeamHero[num4] != null && TeamHero[num4].Alive && TeamHero[num4].HeroItem != null)
			{
				TeamHero[num4].HeroItem.ShowKeyNum(_state: false);
			}
		}
		for (int num5 = 0; num5 < 4; num5++)
		{
			if (TeamNPC[num5] != null && TeamNPC[num5].Alive && TeamNPC[num5].NPCItem != null)
			{
				TeamNPC[num5].NPCItem.ShowKeyNum(_state: false);
			}
		}
	}

	public void KeyboardNum(int _num)
	{
		if (MatchIsOver || (GameManager.Instance.GetDeveloperMode() && !GameManager.Instance.ConfigKeyboardShortcuts) || !heroTurn || (!IsYourTurn() && !IsYourTurnForAddDiscard()) || deathScreen == null || addcardSelector == null || deckCardsWindow == null)
		{
			return;
		}
		if (deckCardsWindow.IsActive())
		{
			int num = _num;
			num = ((num != 0) ? (num - 1) : 9);
			int num2 = 0;
			{
				foreach (Transform item in deckCardsWindow.cardContainer)
				{
					if ((bool)item.GetComponent<CardItem>() && num2 == num)
					{
						item.GetComponent<CardItem>().OnMouseUp();
						break;
					}
					num2++;
				}
				return;
			}
		}
		if (discardSelector.IsActive())
		{
			int num3 = _num;
			num3 = ((num3 != 0) ? (num3 - 1) : 9);
			int num4 = 0;
			{
				foreach (Transform item2 in discardSelector.cardContainer)
				{
					if ((bool)item2.GetComponent<CardItem>() && num4 == num3)
					{
						item2.GetComponent<CardItem>().OnMouseUp();
						break;
					}
					num4++;
				}
				return;
			}
		}
		if (energySelector.IsActive())
		{
			energySelector.AssignEnergyFromOutside(_num);
		}
		else if ((EventSystem.current.currentSelectedGameObject == null || !Functions.TransformIsVisible(EventSystem.current.currentSelectedGameObject.transform)) && canKeyboardCast && !gameBusy && !deathScreen.IsActive() && !addcardSelector.IsActive() && !deckCardsWindow.IsActive())
		{
			if (_num == 0)
			{
				_num = 10;
			}
			if (_num > 0 && _num < 11)
			{
				CastCardNum(_num);
			}
		}
	}

	public void KeyboardEnter()
	{
		if (MatchIsOver || (!GameManager.Instance.GetDeveloperMode() && !GameManager.Instance.ConfigKeyboardShortcuts))
		{
			return;
		}
		if (deckCardsWindow.IsActive())
		{
			if (IsYourTurnForAddDiscard())
			{
				deckCardsWindow.Action();
			}
		}
		else if (discardSelector.IsActive())
		{
			if (IsYourTurnForAddDiscard())
			{
				discardSelector.Action();
			}
		}
		else if (energySelector.IsActive())
		{
			if (IsYourTurn())
			{
				energySelector.AssignEnergyAction();
			}
		}
		else if (deathScreen.IsActive())
		{
			if (Functions.TransformIsVisible(deathScreen.button))
			{
				deathScreen.TurnOffFromButton();
			}
		}
		else if (GameManager.Instance.GetDeveloperMode())
		{
			KeyboardDraw();
		}
	}

	public void KeyboardSpace()
	{
		if (!MatchIsOver && (!GameManager.Instance.GetDeveloperMode() || GameManager.Instance.ConfigKeyboardShortcuts) && (EventSystem.current.currentSelectedGameObject == null || !Functions.TransformIsVisible(EventSystem.current.currentSelectedGameObject.transform)) && !(deathScreen == null) && !(addcardSelector == null) && !(deckCardsWindow == null) && canKeyboardCast && heroTurn && IsYourTurn() && !gameBusy && !deathScreen.IsActive() && !addcardSelector.IsActive() && !deckCardsWindow.IsActive() && heroActive > -1 && botEndTurn.gameObject.activeSelf)
		{
			EndTurn();
		}
	}

	public void KeyboardEmote(int _number)
	{
		if (!MatchIsOver && (EventSystem.current.currentSelectedGameObject == null || !Functions.TransformIsVisible(EventSystem.current.currentSelectedGameObject.transform)) && GameManager.Instance.IsMultiplayer() && !(deathScreen == null) && !(addcardSelector == null) && !(deckCardsWindow == null))
		{
			SetCharactersPing(_number);
		}
	}

	public void KeyboardEnergy()
	{
		if (!MatchIsOver && theHero != null)
		{
			theHero.ModifyEnergy(10);
		}
	}

	public void KeyboardReloadCombat()
	{
		ReloadCombat();
	}

	public void KeyboardDraw()
	{
		if ((EventSystem.current.currentSelectedGameObject == null || !Functions.TransformIsVisible(EventSystem.current.currentSelectedGameObject.transform)) && !(deathScreen == null) && !(addcardSelector == null) && !(deckCardsWindow == null) && heroActive > -1)
		{
			NewCard(1, Enums.CardFrom.Deck);
		}
	}

	public void KeyboardDbg()
	{
		Debug.Log("\ncardsWaitingForReset=>" + cardsWaitingForReset + "\neventList.Count=>" + eventList.Count + "\nisBeginTournPhase=>" + isBeginTournPhase + "\ncastingCardBlocked=>" + castingCardBlocked.Count + "\nwaitingItemTrait=>" + waitingItemTrait + "\ncanKeyboardCast=>" + canKeyboardCast + "\ngameBusy=>" + gameBusy + "\ngeneratedCardTimes=>" + generatedCardTimes + "\ngameStatus=>" + gameStatus + "\nwaitingKill=>" + waitingKill + "\nHandMask.gameObject.activeSelf=>" + HandMask.gameObject.activeSelf);
		if (castingCardBlocked.Count > 0)
		{
			foreach (KeyValuePair<string, bool> item in castingCardBlocked)
			{
				Debug.Log(item.Key);
			}
		}
		if (eventList.Count > 0)
		{
			string text = "";
			for (int i = 0; i < eventList.Count; i++)
			{
				text = text + eventList[i] + " ";
			}
			Debug.Log(text);
		}
	}

	public void ShowCharactersPing(int _action)
	{
		for (int i = 0; i < TeamHero.Length; i++)
		{
			if (TeamHero[i] != null && TeamHero[i].HeroItem != null && TeamHero[i].Alive)
			{
				TeamHero[i].HeroItem.ShowCharacterPing(_action);
			}
		}
		for (int j = 0; j < TeamNPC.Length; j++)
		{
			if (TeamNPC[j] != null && TeamNPC[j].NPCItem != null && TeamNPC[j].Alive)
			{
				TeamNPC[j].NPCItem.ShowCharacterPing(_action);
			}
		}
	}

	public void HideCharactersPing()
	{
		for (int i = 0; i < TeamHero.Length; i++)
		{
			if (TeamHero[i] != null && TeamHero[i].HeroItem != null && TeamHero[i].Alive)
			{
				TeamHero[i].HeroItem.HideCharacterPing();
			}
		}
		for (int j = 0; j < TeamNPC.Length; j++)
		{
			if (TeamNPC[j] != null && TeamNPC[j].NPCItem != null && TeamNPC[j].Alive)
			{
				TeamNPC[j].NPCItem.HideCharacterPing();
			}
		}
	}

	public void SetCharactersPing(int _action)
	{
		if (!waitingDeathScreen && !WaitingForActionScreen() && !emoteManager.IsBlocked() && emoteManager.gameObject.activeSelf)
		{
			emoteManager.HideEmotes();
			if (emoteManager.EmoteNeedsTarget(_action))
			{
				ShowCharactersPing(_action);
			}
			else if (emoteManager.heroActive > -1 && emoteManager.heroActive < 4 && TeamHero[emoteManager.heroActive] != null)
			{
				EmoteTarget(TeamHero[emoteManager.heroActive].Id, _action);
			}
		}
	}

	public void ResetCharactersPing()
	{
		HideCharactersPing();
	}

	[PunRPC]
	private void NET_EmoteTarget(string _id, byte _action, int _heroIndex)
	{
		if (TeamHero != null && TeamHero[_heroIndex] != null)
		{
			string owner = TeamHero[_heroIndex].Owner;
			if (NetworkManager.Instance.IsPlayerMutedByNick(owner))
			{
				Debug.Log("Blocked communication because player is muted");
			}
			else
			{
				EmoteTarget(_id, _action, _heroIndex, _fromNet: true);
			}
		}
	}

	public void EmoteTarget(string _id, int _action, int _heroIndex = -1, bool _fromNet = false)
	{
		if (!_fromNet)
		{
			_heroIndex = emoteManager.heroActive;
		}
		if (!_fromNet && GameManager.Instance.IsMultiplayer())
		{
			photonView.RPC("NET_EmoteTarget", RpcTarget.Others, _id, (byte)_action, _heroIndex);
		}
		Transform transform = null;
		CharacterItem characterItem = null;
		for (int i = 0; i < TeamHero.Length; i++)
		{
			if (TeamHero[i] != null && TeamHero[i].Alive && TeamHero[i].HeroItem != null && TeamHero[i].Id == _id)
			{
				transform = TeamHero[i].HeroItem.transform;
				characterItem = TeamHero[i].HeroItem;
				break;
			}
		}
		if (transform == null)
		{
			for (int j = 0; j < TeamNPC.Length; j++)
			{
				if (TeamNPC[j] != null && TeamNPC[j].Alive && TeamNPC[j].NPCItem != null && TeamNPC[j].Id == _id)
				{
					transform = TeamNPC[j].NPCItem.transform;
					characterItem = TeamNPC[j].NPCItem;
					break;
				}
			}
		}
		if (transform != null && characterItem != null)
		{
			GameObject obj = UnityEngine.Object.Instantiate(emoteTargetPrefab, Vector3.zero, Quaternion.identity);
			obj.transform.position = characterItem.emoteCharacterPing.transform.position;
			obj.GetComponent<EmoteTarget>().SetIcons(_heroIndex, _action);
			GameManager.Instance.PlayLibraryAudio("Pop3", 2.9f);
			if (!_fromNet)
			{
				emoteManager.SetBlocked(_state: true);
			}
		}
		if (!_fromNet)
		{
			ResetCharactersPing();
		}
	}

	public void SendEmoteCard(int tablePosition)
	{
		if (heroActive > -1)
		{
			photonView.RPC("NET_SendEmoteCard", RpcTarget.Others, (byte)tablePosition, (byte)emoteManager.heroActive);
			ResetCharactersPing();
			DoEmoteCard((byte)tablePosition, (byte)emoteManager.heroActive);
		}
	}

	[PunRPC]
	public void NET_SendEmoteCard(byte _tablePosition, byte _heroIndex)
	{
		if (TeamHero != null && TeamHero[_heroIndex] != null)
		{
			string owner = TeamHero[_heroIndex].Owner;
			if (NetworkManager.Instance.IsPlayerMutedByNick(owner))
			{
				Debug.Log("Blocked communication because player is muted");
			}
			else
			{
				DoEmoteCard(_tablePosition, _heroIndex);
			}
		}
	}

	private void DoEmoteCard(byte _tablePosition, byte _heroIndex)
	{
		if (cardItemTable == null)
		{
			return;
		}
		if (cardItemTable.Count > _tablePosition && cardItemTable[_tablePosition] != null && cardItemTable[_tablePosition].HaveEmoteIcon(_heroIndex))
		{
			cardItemTable[_tablePosition].RemoveEmoteIcon(_heroIndex);
			return;
		}
		for (int i = 0; i < cardItemTable.Count; i++)
		{
			if (i != _tablePosition && cardItemTable[i] != null)
			{
				cardItemTable[i].RemoveEmoteIcon(_heroIndex);
			}
		}
		if (cardItemTable.Count > _tablePosition && cardItemTable[_tablePosition] != null)
		{
			cardItemTable[_tablePosition].ShowEmoteIcon(_heroIndex);
			if (IsYourTurn())
			{
				GameManager.Instance.PlayLibraryAudio("Pop6", 2.9f);
			}
		}
	}

	public void CreateLogCardModification(string _cardId, Hero _theHero)
	{
		CreateLogEntry(_initial: false, "cardModification:" + logDictionary.Count, _cardId, _theHero, null, null, null, currentRound);
	}

	public void CreateLogEntry(bool _initial, string _key, string _cardId, Hero _theHero, NPC _theNPC, Hero _theHeroTarget, NPC _theNPCTarget, int _round = 0, Enums.EventActivation _event = Enums.EventActivation.None, int _auxInt = -1, string _auxString = "")
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
		{
			_key = _event.ToString() + logEntry.logHeroName + logEntry.logNPCName + logEntry.logRound + logDictionary.Count;
		}
		if (!logDictionary.ContainsKey(_key))
		{
			new LogResult();
			logDictionary.Add(_key, logEntry);
		}
		CreateLogResult(_initial, _key);
	}

	private void CreateLogCastCard(bool _status, CardData _cardData, string _uniqueId, Hero _theHero, NPC _theNPC, Hero _hero, NPC _npc)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(_cardData.InternalId);
		stringBuilder.Append("_");
		stringBuilder.Append(_uniqueId);
		if (_cardData.TargetType == Enums.CardTargetType.Global || _cardData.TargetSide == Enums.CardTargetSide.Self)
		{
			CreateLogEntry(_status, stringBuilder.ToString(), _cardData.Id, _theHero, theNPC, null, null, currentRound);
		}
		else
		{
			CreateLogEntry(_status, stringBuilder.ToString(), _cardData.Id, _theHero, theNPC, _hero, _npc, currentRound);
		}
	}

	private void CreateLogResult(bool _initial, string _key)
	{
		bool flag = false;
		for (int i = 0; i < 8; i++)
		{
			Character character = ((i >= 4) ? ((Character)TeamNPC[i - 4]) : ((Character)TeamHero[i]));
			if (character == null)
			{
				continue;
			}
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
				if (i < 4)
				{
					for (int j = 0; j < HeroHand[i].Count; j++)
					{
						CardData cardData = GetCardData(HeroHand[i][j], createInDict: false);
						stringBuilder.Append(character.GetCardFinalCost(cardData));
					}
				}
				for (int k = 0; k < character.AuraList.Count; k++)
				{
					AuraCurseData aCData = character.AuraList[k].ACData;
					if (!(aCData != null))
					{
						continue;
					}
					int auraCharges = character.AuraList[k].AuraCharges;
					if (auraCharges > 0)
					{
						if (!dictionary.ContainsKey(aCData.Id))
						{
							dictionary.Add(aCData.Id, auraCharges);
						}
						else
						{
							dictionary[aCData.Id] = auraCharges;
						}
						stringBuilder.Append(aCData.Id);
						stringBuilder.Append(auraCharges);
					}
				}
				if (logDictionary[_key].logActivation == Enums.EventActivation.TraitActivation)
				{
					foreach (KeyValuePair<string, int> activatedTrait in activatedTraits)
					{
						stringBuilder.Append(activatedTrait.Key);
						stringBuilder.Append(activatedTrait.Value);
					}
				}
				logResult.logResultMd5 = stringBuilder.ToString();
				logResult.logResultDict = dictionary;
				if (logDictionary.ContainsKey(_key))
				{
					if (logDictionary[_key].logResult == null)
					{
						logDictionary[_key].logResult = new Dictionary<string, LogResult>();
					}
					if (!logDictionary[_key].logResult.ContainsKey(character.Id))
					{
						logDictionary[_key].logResult.Add(character.Id, logResult);
					}
				}
			}
			else
			{
				if (!logDictionary.ContainsKey(_key) || logDictionary[_key].logResult == null || !logDictionary[_key].logResult.ContainsKey(character.Id))
				{
					continue;
				}
				LogResult logResult = logDictionary[_key].logResult[character.Id];
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.Append(character.GetHp());
				if (i < 4)
				{
					for (int l = 0; l < HeroHand[i].Count; l++)
					{
						CardData cardData2 = GetCardData(HeroHand[i][l], createInDict: false);
						stringBuilder2.Append(character.GetCardFinalCost(cardData2));
					}
				}
				for (int m = 0; m < character.AuraList.Count; m++)
				{
					AuraCurseData aCData2 = character.AuraList[m].ACData;
					if (aCData2 != null)
					{
						int auraCharges2 = character.AuraList[m].AuraCharges;
						if (auraCharges2 > 0)
						{
							stringBuilder2.Append(aCData2.Id);
							stringBuilder2.Append(auraCharges2);
						}
					}
				}
				if (logDictionary[_key].logActivation == Enums.EventActivation.TraitActivation)
				{
					foreach (KeyValuePair<string, int> activatedTrait2 in activatedTraits)
					{
						stringBuilder2.Append(activatedTrait2.Key);
						stringBuilder2.Append(activatedTrait2.Value);
					}
				}
				if (stringBuilder2.ToString() == logResult.logResultMd5)
				{
					logResult.logResultDict = new Dictionary<string, int>();
					continue;
				}
				flag = true;
				Dictionary<string, int> dictionary2 = logResult.logResultDict.ToDictionary((KeyValuePair<string, int> entry) => entry.Key, (KeyValuePair<string, int> entry) => entry.Value);
				if (!dictionary2.ContainsKey("hp"))
				{
					dictionary2.Add("hp", 0);
				}
				dictionary2["hpCurrent"] = character.GetHp();
				dictionary2["hp"] = character.GetHp() - dictionary2["hp"];
				if (dictionary2["hp"] == 0)
				{
					dictionary2.Remove("hp");
				}
				for (int num = 0; num < character.AuraList.Count; num++)
				{
					AuraCurseData aCData3 = character.AuraList[num].ACData;
					if (aCData3 != null)
					{
						int auraCharges3 = character.AuraList[num].AuraCharges;
						if (dictionary2.ContainsKey(aCData3.Id))
						{
							dictionary2[aCData3.Id] = auraCharges3 - dictionary2[aCData3.Id];
						}
						else
						{
							dictionary2.Add(aCData3.Id, auraCharges3);
						}
						if (dictionary2[aCData3.Id] == 0)
						{
							dictionary2.Remove(aCData3.Id);
						}
					}
				}
				foreach (KeyValuePair<string, int> item in logResult.logResultDict)
				{
					if (item.Key != "hp" && item.Key != "hpCurrent" && !character.HasEffect(item.Key) && dictionary2.ContainsKey(item.Key))
					{
						dictionary2[item.Key] = -dictionary2[item.Key];
					}
				}
				logResult.logResultDict = dictionary2;
				logDictionary[_key].logFinished = true;
			}
		}
		if (!_initial && !flag && logDictionary.ContainsKey(_key) && logDictionary[_key].logActivation == Enums.EventActivation.TraitActivation)
		{
			logDictionary.Remove(_key);
		}
		if (console != null)
		{
			console.DoLog();
		}
	}

	public void ShowLog()
	{
		console.Show(!console.gameObject.activeSelf);
	}

	public void ConsoleLgDbg()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<string, LogEntry> item in logDictionary)
		{
			stringBuilder.Append("**** ");
			stringBuilder.Append(item.Key);
			stringBuilder.Append(" ****\n");
			stringBuilder.Append("CardId -> ");
			stringBuilder.Append(item.Value.logCardId);
			stringBuilder.Append("\n");
			stringBuilder.Append("FromHero -> ");
			stringBuilder.Append(item.Value.logHero);
			stringBuilder.Append("\n");
			stringBuilder.Append("FromNPC -> ");
			stringBuilder.Append(item.Value.logNPC);
			stringBuilder.Append("\n");
			stringBuilder.Append("ToHero -> ");
			stringBuilder.Append(item.Value.logHeroTarget);
			stringBuilder.Append("\n");
			stringBuilder.Append("ToNPC -> ");
			stringBuilder.Append(item.Value.logNPCTarget);
			stringBuilder.Append("\n");
			stringBuilder.Append("Round -> ");
			stringBuilder.Append(item.Value.logRound);
			stringBuilder.Append("\n");
			stringBuilder.Append("Effects");
			foreach (KeyValuePair<string, LogResult> item2 in item.Value.logResult)
			{
				stringBuilder.Append("\n");
				stringBuilder.Append(item2.Key);
				stringBuilder.Append("=>");
				foreach (KeyValuePair<string, int> item3 in item2.Value.logResultDict)
				{
					stringBuilder.Append(item3.Key);
					stringBuilder.Append("=>");
					stringBuilder.Append(item3.Value);
					stringBuilder.Append(",");
				}
			}
			stringBuilder.Append("\n");
			stringBuilder.Append("[Log]");
			stringBuilder.Append("\n");
		}
		Debug.Log(stringBuilder.ToString());
	}

	public void DoStepSound()
	{
		if (combatData != null)
		{
			combatData.DoStepSound();
		}
	}

	private IEnumerator CreateCardCastPet(CardData cardPet, GameObject charGO, Hero _hero, NPC _npc)
	{
		CreatePet(cardPet, charGO, _hero, _npc, _fromEnchant: true, cardPet.InternalId);
		Transform _pet = charGO.transform.Find("thePetEnchantment" + cardPet.InternalId);
		if (!(_pet != null))
		{
			yield break;
		}
		eventList.Add("CreateCardCastPet");
		NPCItem _petNPCItem = _pet.GetComponent<NPCItem>();
		Animator _petAnimator = _pet.GetComponent<Animator>();
		_petNPCItem.InstantFadeOutCharacter();
		StartCoroutine(_petNPCItem.FadeInCharacter());
		yield return Globals.Instance.WaitForSeconds(0.5f);
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
				yield return Globals.Instance.WaitForSeconds(1f);
				_petNPCItem.MoveToCenterBack();
			}
		}
		if (cardPet.PetTemporalFadeOutDelay > 0f)
		{
			yield return Globals.Instance.WaitForSeconds(cardPet.PetTemporalFadeOutDelay);
		}
		StartCoroutine(_petNPCItem.FadeOutCharacter());
		yield return Globals.Instance.WaitForSeconds(0.4f);
		UnityEngine.Object.Destroy(_pet.gameObject);
		eventList.Remove("CreateCardCastPet");
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false, bool shoulderLeft = false, bool shoulderRight = false, int absoluteIndex = -1)
	{
		if (energySelector == null || (CardDrag && (goingDown || goingUp)))
		{
			return;
		}
		if (energySelector.IsActive() && IsYourTurn())
		{
			if (goingLeft)
			{
				warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(energySelector.buttonLess.transform.position);
				energySelector.AssignEnergyLess();
			}
			else if (goingRight)
			{
				warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(energySelector.buttonMore.transform.position);
				energySelector.AssignEnergyMore();
			}
			else
			{
				warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(energySelector.buttonAccept.transform.position);
			}
			Mouse.current.WarpCursorPosition(warpPosition);
			return;
		}
		if (deathScreen.IsActive() && Functions.TransformIsVisible(deathScreen.button))
		{
			warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(deathScreen.button.transform.position);
			Mouse.current.WarpCursorPosition(warpPosition);
			return;
		}
		if (console.IsActive() && Functions.TransformIsVisible(consoleCloseButton))
		{
			warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(consoleCloseButton.position);
			Mouse.current.WarpCursorPosition(warpPosition);
			return;
		}
		controllerList.Clear();
		if (deckCardsWindow.IsActive() || discardSelector.IsActive())
		{
			if (deckCardsWindow.IsActive())
			{
				if (deckCardsWindow.elements.gameObject.activeSelf)
				{
					foreach (Transform item in deckCardsWindow.cardContainer)
					{
						if ((bool)item.GetComponent<CardItem>())
						{
							controllerList.Add(item);
						}
					}
					if (Functions.TransformIsVisible(deckCardsWindow.buttonDiscard.GetChild(0)))
					{
						controllerList.Add(deckCardsWindow.buttonDiscard.GetChild(0));
					}
					controllerList.Add(deckCardsWindow.buttonHide.GetChild(0));
					controllerCurrentIndex = Functions.GetListClosestIndexToMousePosition(controllerList);
					controllerCurrentIndex = Functions.GetClosestIndexBasedOnDirection(controllerList, controllerCurrentIndex, goingUp, goingRight, goingDown, goingLeft);
					if (controllerCurrentIndex >= 0 && controllerCurrentIndex < controllerList.Count - 2)
					{
						Canvas.ForceUpdateCanvases();
						Vector3 zero = Vector3.zero;
						zero.x = deckCardsWindow.cardContainerRT.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.x;
						if (deckCardsWindow.cardContainer.childCount <= 10)
						{
							zero.y = -4.5f;
						}
						else
						{
							zero.y = -4.5f + 1.65f * Mathf.Floor((float)deckCardsWindow.cardContainer.childCount / 5f) + 3.1f * Mathf.Floor((float)controllerCurrentIndex / 5f);
						}
						deckCardsWindow.cardContainerRT.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = zero;
					}
					warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(controllerList[controllerCurrentIndex].position);
					Mouse.current.WarpCursorPosition(warpPosition);
					return;
				}
				controllerList.Add(deckCardsWindow.buttonHide.GetChild(0));
				for (int num = TeamHero.Length - 1; num >= 0; num--)
				{
					if (TeamHero[num] != null && TeamHero[num].Alive && TeamHero[num].HeroItem != null)
					{
						controllerList.Add(TeamHero[num].HeroItem.characterTransform);
					}
				}
				for (int num2 = TeamNPC.Length - 1; num2 >= 0; num2--)
				{
					if (TeamNPC[num2] != null && TeamNPC[num2].Alive && TeamNPC[num2].NPCItem != null)
					{
						controllerList.Add(TeamNPC[num2].NPCItem.characterTransform);
					}
				}
				controllerCurrentIndex = Functions.GetListClosestIndexToMousePosition(controllerList);
				controllerCurrentIndex = Functions.GetClosestIndexBasedOnDirection(controllerList, controllerCurrentIndex, goingUp, goingRight, goingDown, goingLeft);
				warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(controllerList[controllerCurrentIndex].position);
				Mouse.current.WarpCursorPosition(warpPosition);
			}
			else
			{
				if (!discardSelector.IsActive())
				{
					return;
				}
				if (discardSelector.elements.gameObject.activeSelf)
				{
					foreach (Transform item2 in discardSelector.cardContainer)
					{
						if ((bool)item2.GetComponent<CardItem>())
						{
							controllerList.Add(item2);
						}
					}
					if (Functions.TransformIsVisible(discardSelector.button.GetChild(0)))
					{
						controllerList.Add(discardSelector.button.GetChild(0));
					}
					controllerList.Add(discardSelector.buttonHide.GetChild(0));
					controllerCurrentIndex = Functions.GetListClosestIndexToMousePosition(controllerList);
					controllerCurrentIndex = Functions.GetClosestIndexBasedOnDirection(controllerList, controllerCurrentIndex, goingUp, goingRight, goingDown, goingLeft);
					warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(controllerList[controllerCurrentIndex].position);
					Mouse.current.WarpCursorPosition(warpPosition);
					return;
				}
				controllerList.Add(discardSelector.buttonHide.GetChild(0));
				for (int num3 = TeamHero.Length - 1; num3 >= 0; num3--)
				{
					if (TeamHero[num3] != null && TeamHero[num3].Alive && TeamHero[num3].HeroItem != null)
					{
						controllerList.Add(TeamHero[num3].HeroItem.characterTransform);
					}
				}
				for (int num4 = TeamNPC.Length - 1; num4 >= 0; num4--)
				{
					if (TeamNPC[num4] != null && TeamNPC[num4].Alive && TeamNPC[num4].NPCItem != null)
					{
						controllerList.Add(TeamNPC[num4].NPCItem.characterTransform);
					}
				}
				controllerCurrentIndex = Functions.GetListClosestIndexToMousePosition(controllerList);
				controllerCurrentIndex = Functions.GetClosestIndexBasedOnDirection(controllerList, controllerCurrentIndex, goingUp, goingRight, goingDown, goingLeft);
				warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(controllerList[controllerCurrentIndex].position);
				Mouse.current.WarpCursorPosition(warpPosition);
			}
			return;
		}
		int num5 = -1;
		int num6 = -1;
		int num7 = -1;
		if (!CardDrag)
		{
			if (heroTurn)
			{
				if (CountHeroDeck() > 0)
				{
					controllerList.Add(GO_DecksObject.transform);
				}
				if (CountHeroDiscard() > 0)
				{
					controllerList.Add(GO_DiscardPile.transform.GetChild(0).transform);
				}
			}
			if (cardItemTable != null)
			{
				for (int i = 0; i < cardItemTable.Count; i++)
				{
					controllerList.Add(cardItemTable[i].transform);
				}
			}
			if (heroTurn)
			{
				if (IsYourTurn())
				{
					num5 = controllerList.Count;
					controllerList.Add(botEndTurn);
				}
				if (iconWeapon.GetComponent<BoxCollider2D>().enabled)
				{
					controllerList.Add(iconWeapon.transform);
				}
				if (iconArmor.GetComponent<BoxCollider2D>().enabled)
				{
					controllerList.Add(iconArmor.transform);
				}
				if (iconJewelry.GetComponent<BoxCollider2D>().enabled)
				{
					controllerList.Add(iconJewelry.transform);
				}
				if (iconAccesory.GetComponent<BoxCollider2D>().enabled)
				{
					controllerList.Add(iconAccesory.transform);
				}
				if (iconPet.GetComponent<BoxCollider2D>().enabled)
				{
					controllerList.Add(iconPet.transform);
				}
			}
		}
		num6 = controllerList.Count;
		for (int num8 = TeamHero.Length - 1; num8 >= 0; num8--)
		{
			if (TeamHero[num8] != null && TeamHero[num8].Alive && TeamHero[num8].HeroItem != null)
			{
				controllerList.Add(TeamHero[num8].HeroItem.characterTransform);
			}
		}
		for (int j = 0; j < TeamNPC.Length; j++)
		{
			if (TeamNPC[j] != null && TeamNPC[j].Alive && TeamNPC[j].NPCItem != null)
			{
				num7 = controllerList.Count;
				controllerList.Add(TeamNPC[j].NPCItem.characterTransform);
			}
		}
		if (!CardDrag)
		{
			for (int k = 0; k < TeamHero.Length; k++)
			{
				if (TeamHero[k] == null || !TeamHero[k].Alive || !(TeamHero[k].HeroItem != null))
				{
					continue;
				}
				if (Functions.TransformIsVisible(TeamHero[k].HeroItem.emoteCharacterPing.transform))
				{
					controllerList.Add(TeamHero[k].HeroItem.emoteCharacterPing.transform);
				}
				if (!Functions.TransformIsVisible(TeamHero[k].HeroItem.iconEnchantment.transform))
				{
					continue;
				}
				controllerList.Add(TeamHero[k].HeroItem.iconEnchantment.transform);
				if (Functions.TransformIsVisible(TeamHero[k].HeroItem.iconEnchantment2.transform))
				{
					controllerList.Add(TeamHero[k].HeroItem.iconEnchantment2.transform);
					if (Functions.TransformIsVisible(TeamHero[k].HeroItem.iconEnchantment3.transform))
					{
						controllerList.Add(TeamHero[k].HeroItem.iconEnchantment3.transform);
					}
				}
			}
			for (int l = 0; l < TeamNPC.Length; l++)
			{
				if (TeamNPC[l] == null || !TeamNPC[l].Alive || !(TeamNPC[l].NPCItem != null))
				{
					continue;
				}
				foreach (Transform item3 in TeamNPC[l].NPCItem.cardsGOT)
				{
					if ((bool)item3.GetComponent<CardItem>() && item3.GetComponent<CardItem>().IsRevealed())
					{
						controllerList.Add(item3);
					}
				}
				if (Functions.TransformIsVisible(TeamNPC[l].NPCItem.emoteCharacterPing.transform))
				{
					controllerList.Add(TeamNPC[l].NPCItem.emoteCharacterPing.transform);
				}
				if (!Functions.TransformIsVisible(TeamNPC[l].NPCItem.iconEnchantment.transform))
				{
					continue;
				}
				controllerList.Add(TeamNPC[l].NPCItem.iconEnchantment.transform);
				if (Functions.TransformIsVisible(TeamNPC[l].NPCItem.iconEnchantment2.transform))
				{
					controllerList.Add(TeamNPC[l].NPCItem.iconEnchantment2.transform);
					if (Functions.TransformIsVisible(TeamNPC[l].NPCItem.iconEnchantment3.transform))
					{
						controllerList.Add(TeamNPC[l].NPCItem.iconEnchantment3.transform);
					}
				}
			}
			if (Functions.TransformIsVisible(iconCorruption.transform))
			{
				controllerList.Add(iconCorruption.transform);
			}
			foreach (Transform item4 in GO_Initiative.transform)
			{
				if (Functions.TransformIsVisible(item4))
				{
					controllerList.Add(item4);
				}
			}
			for (int m = 0; m < emotesTransform.Count; m++)
			{
				if (Functions.TransformIsVisible(emotesTransform[m]))
				{
					controllerList.Add(emotesTransform[m]);
				}
			}
		}
		if (absoluteIndex > -1)
		{
			controllerCurrentIndex = absoluteIndex;
		}
		else
		{
			controllerCurrentIndex = Functions.GetListClosestIndexToMousePosition(controllerList);
		}
		if (controllerCurrentIndex == -1 || controllerList.Count == 0)
		{
			return;
		}
		if (num5 > -1 && controllerCurrentIndex <= num5 && (goingRight || goingLeft || goingDown))
		{
			if (goingRight)
			{
				if (controllerCurrentIndex == num5)
				{
					return;
				}
				controllerCurrentIndex++;
			}
			else if (goingLeft)
			{
				controllerCurrentIndex--;
				if (controllerCurrentIndex < 0)
				{
					controllerCurrentIndex = 0;
				}
			}
			else if (goingDown)
			{
				if (controllerCurrentIndex != num5)
				{
					return;
				}
				controllerCurrentIndex++;
			}
		}
		else if (controllerCurrentIndex >= num6 && controllerCurrentIndex <= num7 && (goingRight || goingLeft))
		{
			if (goingRight)
			{
				if (controllerCurrentIndex == num7)
				{
					return;
				}
				controllerCurrentIndex++;
			}
			else
			{
				if (controllerCurrentIndex == num6)
				{
					return;
				}
				controllerCurrentIndex--;
			}
		}
		else
		{
			controllerCurrentIndex = Functions.GetClosestIndexBasedOnDirection(controllerList, controllerCurrentIndex, goingUp, goingRight, goingDown, goingLeft);
		}
		if (controllerCurrentIndex == -1 || !(controllerList[controllerCurrentIndex] != null))
		{
			return;
		}
		if ((bool)controllerList[controllerCurrentIndex].parent.GetComponent<HeroItem>() || (bool)controllerList[controllerCurrentIndex].parent.GetComponent<NPCItem>())
		{
			controllerOffsetVector = Vector3.zero;
			controllerOffsetVector.y = -0.5f;
			warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(controllerList[controllerCurrentIndex].position + controllerOffsetVector);
		}
		else if (controllerList[controllerCurrentIndex].GetComponent<CardItem>() != null && controllerList[controllerCurrentIndex].GetComponent<CardItem>().enabled && controllerList[controllerCurrentIndex].parent.transform == GO_Hand.transform)
		{
			controllerOffsetVector = Vector3.zero;
			controllerOffsetVector.y = 1.25f;
			if (goingRight)
			{
				controllerOffsetVector.x = -0.6f;
			}
			else if (goingLeft)
			{
				controllerOffsetVector.x = 0.5f;
			}
			warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(controllerList[controllerCurrentIndex].position + controllerOffsetVector);
		}
		else
		{
			warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(controllerList[controllerCurrentIndex].position);
		}
		Mouse.current.WarpCursorPosition(warpPosition);
	}

	public void ControllerMoveShoulder(bool _isRight = false)
	{
	}

	public void SetControllerCardClicked()
	{
		controllerClickedCardIndex = controllerCurrentIndex;
		controllerClickedCard = true;
		MoveControllerToHeroes();
	}

	public void MoveControllerToHeroes()
	{
		bool flag = false;
		int num = -1;
		int absoluteIndex = 0;
		if (cardItemActive != null && cardItemActive.CardData != null)
		{
			for (int i = 0; i < TeamHero.Length; i++)
			{
				if (TeamHero[i] != null && TeamHero[i].Alive)
				{
					num++;
					if (CheckTarget(TeamHero[i].HeroItem.transform, cardItemActive.CardData))
					{
						absoluteIndex = NumHeroesAlive() - 1 - num;
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				for (int j = 0; j < TeamNPC.Length; j++)
				{
					if (TeamNPC[j] != null && TeamNPC[j].Alive)
					{
						num++;
						if (CheckTarget(TeamNPC[j].NPCItem.transform, cardItemActive.CardData))
						{
							absoluteIndex = num;
							break;
						}
					}
				}
			}
		}
		ControllerMovement(goingUp: false, goingRight: false, goingDown: false, goingLeft: false, shoulderLeft: false, shoulderRight: false, absoluteIndex);
	}

	public void ResetController()
	{
		if (IsYourTurn() && controllerClickedCard)
		{
			ResetControllerPositions();
			ResetControllerClickedCard();
		}
	}

	private int ResolveAuraCurseCharges(AuraCurseData auraCurse, int baseCharges, bool useGlobal, bool useSpecialValue1, bool useSpecialValue2, Character targetCharacter, int cardSpecialValueGlobal, int cardSpecialValue1, int cardSpecialValue2, int? additionalAuraCharges = null, CardData cardData = null, Character caster = null)
	{
		if (targetCharacter != null && targetCharacter.Alive && auraCurse != null && !auraCurse.IsAura && auraCurse.Id.Equals("sleep", StringComparison.OrdinalIgnoreCase))
		{
			return CalculateSleepCharges(targetCharacter);
		}
		int baseCharges2 = baseCharges;
		if (useGlobal)
		{
			baseCharges2 = cardSpecialValueGlobal;
		}
		else if (useSpecialValue1)
		{
			baseCharges2 = cardSpecialValue1;
		}
		else if (useSpecialValue2)
		{
			baseCharges2 = cardSpecialValue2;
		}
		else if (additionalAuraCharges.HasValue && auraCurse != null && auraCurse.IsAura)
		{
			baseCharges2 = additionalAuraCharges.GetValueOrDefault();
		}
		return GetEffectiveCharges(auraCurse, baseCharges2, targetCharacter) + TeamBonusHotline.GetAuraBonusFromTeamHeroesForPets(caster, cardData, auraCurse.Id);
	}

	private void ResetControllerPositions(bool forceIt = false)
	{
		if (IsYourTurn() && (forceIt || controllerClickedCard))
		{
			StartCoroutine(ResetControllerPositionsAction());
		}
	}

	private IEnumerator ResetControllerPositionsAction()
	{
		yield return Globals.Instance.WaitForSeconds(0.15f);
		int num = 0;
		if (heroTurn)
		{
			if (CountHeroDeck() > 0)
			{
				num++;
			}
			if (CountHeroDiscard() > 0)
			{
				num++;
			}
		}
		ControllerMovement(goingUp: false, goingRight: false, goingDown: false, goingLeft: false, shoulderLeft: false, shoulderRight: false, num);
	}

	private void ResetControllerClickedCard()
	{
		if (IsYourTurn() && controllerClickedCard)
		{
			controllerClickedCard = false;
			controllerClickedCardIndex = -1;
		}
	}

	public void ControllerStopDrag()
	{
		cardItemActive.DoReturnCardToDeckFromDrag();
	}

	public void ControllerExecute()
	{
		cardItemActive.OnMouseUp();
	}

	private int CalculateSleepCharges(Character target)
	{
		if (target == null || !target.Alive)
		{
			return 0;
		}
		if (target.GetCurseList().Contains("sleep"))
		{
			return 0;
		}
		if (!(target.GetHpPercent() > 50f))
		{
			return 2;
		}
		return 1;
	}

	private int GetEffectiveCharges(AuraCurseData curse, int baseCharges, Character character)
	{
		if (character == null || !character.Alive || curse == null)
		{
			return baseCharges;
		}
		int max = ((curse.MaxCharges < 0) ? int.MaxValue : curse.MaxCharges);
		int chargeMultiplier = GetChargeMultiplier(curse, character);
		return Mathf.Clamp(baseCharges * chargeMultiplier, 0, max);
	}

	private int GetChargeMultiplier(AuraCurseData auraCurse, Character character)
	{
		int result = 1;
		if (!auraCurse.IsAura && character.HasEffect("sleep"))
		{
			result = 2;
		}
		return result;
	}

	public void SwapNPCDeck(int index1, int index2)
	{
		List<string>[] nPCDeck = NPCDeck;
		List<string>[] nPCDeck2 = NPCDeck;
		List<string> list = NPCDeck[index2];
		List<string> list2 = NPCDeck[index1];
		nPCDeck[index1] = list;
		nPCDeck2[index2] = list2;
	}

	public void SwapNPCDeckDiscard(int index1, int index2)
	{
		List<string>[] nPCDeckDiscard = NPCDeckDiscard;
		List<string>[] nPCDeckDiscard2 = NPCDeckDiscard;
		List<string> list = NPCDeckDiscard[index2];
		List<string> list2 = NPCDeckDiscard[index1];
		nPCDeckDiscard[index1] = list;
		nPCDeckDiscard2[index2] = list2;
	}

	public void SwapNPCHand(int index1, int index2)
	{
		List<string>[] nPCHand = NPCHand;
		List<string>[] nPCHand2 = NPCHand;
		List<string> list = NPCHand[index2];
		List<string> list2 = NPCHand[index1];
		nPCHand[index1] = list;
		nPCHand2[index2] = list2;
	}

	public List<string> GetNPCDeck(int index)
	{
		return NPCDeck[index];
	}

	public List<string> GetNPCHand(int index)
	{
		return NPCHand[index];
	}

	public List<string> GetNPCDiscardDeck(int index)
	{
		return NPCDeckDiscard[index];
	}

	public void SetNPCDeck(int index, List<string> newDeck)
	{
		NPCDeck[index] = newDeck;
	}

	public void SetNPCHand(int index, List<string> newHand)
	{
		NPCHand[index] = newHand;
	}

	public void SetNPCDiscardDeck(int index, List<string> newDiscardPile)
	{
		NPCDeckDiscard[index] = newDiscardPile;
	}

	public void NPCDiscardPileClear(int index)
	{
		NPCDeckDiscard[index].Clear();
	}

	public void SwapCharacterOrder(string charId1, string charId2, int index1, int index2)
	{
		int num = CharOrder.FindIndex((CharacterForOrder x) => x.id == charId1);
		int num2 = CharOrder.FindIndex((CharacterForOrder x) => x.id == charId2);
		if (num >= 0 && num2 >= 0)
		{
			CharacterForOrder characterForOrder = CharOrder[num];
			CharacterForOrder characterForOrder2 = CharOrder[num2];
			int index3 = CharOrder[num2].index;
			int index4 = CharOrder[num].index;
			characterForOrder.index = index3;
			characterForOrder2.index = index4;
			List<CharacterForOrder> charOrder = CharOrder;
			index4 = num;
			List<CharacterForOrder> charOrder2 = CharOrder;
			index3 = num2;
			characterForOrder = CharOrder[num2];
			CharacterForOrder characterForOrder3 = CharOrder[num];
			CharacterForOrder characterForOrder4 = (charOrder[index4] = characterForOrder);
			characterForOrder4 = (charOrder2[index3] = characterForOrder3);
		}
		else
		{
			Debug.LogError("Character not found in CharOrder array of match manager.");
		}
	}

	public void AddVanishToDeck(int index, bool isHero)
	{
		if (isHero)
		{
			foreach (string item in HeroDeck[index])
			{
				GetCardData(item).Vanish = true;
			}
			{
				foreach (string item2 in HeroHand[index])
				{
					GetCardData(item2).Vanish = true;
					CardItem[] componentsInChildren = GO_Hand.GetComponentsInChildren<CardItem>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].SetVanishIcon(state: true);
					}
				}
				return;
			}
		}
		foreach (string item3 in NPCDeck[index])
		{
			GetCardData(item3).Vanish = true;
		}
		foreach (string item4 in NPCHand[index])
		{
			GetCardData(item4).Vanish = true;
		}
	}

	public bool IsPhantomArmorSpecialCard(string cardId)
	{
		if (BossNpc != null && BossNpc is PhantomArmor)
		{
			return (BossNpc as PhantomArmor).IsSpecialCard(cardId);
		}
		return false;
	}

	public Transform GetNPCParent()
	{
		return GO_NPCs.transform;
	}

	public List<string> GetHeroVanishPile(int heroIndex)
	{
		return HeroDeckVanish[heroIndex];
	}

	public bool ApplyHeroModsToPetCard(CardData cardData, Hero theHero)
	{
		if (cardData == null)
		{
			return false;
		}
		if ((cardData.IsPetAttack || cardData.IsPetCast) && cardData.Id.ToLower().Split("_")[0] == Globals.Instance.GetCardData(theHero.Pet)?.Item?.CardToGain?.Id.ToLower())
		{
			if (cardData.DamageType == Enums.DamageType.None)
			{
				cardData.DamageType = theHero.PetBonusDamageType;
				cardData.Damage = theHero.PetBonusDamageAmount;
			}
			else if (cardData.DamageType == theHero.PetBonusDamageType)
			{
				cardData.Damage += theHero.PetBonusDamageAmount;
			}
			else if (cardData.DamageType2 == Enums.DamageType.None)
			{
				cardData.DamageType2 = theHero.PetBonusDamageType;
				cardData.Damage2 = theHero.PetBonusDamageAmount;
			}
			else if (cardData.DamageType2 == theHero.PetBonusDamageType)
			{
				cardData.Damage2 += theHero.PetBonusDamageAmount;
			}
			return true;
		}
		return false;
	}

	private void AddValueToDictionary<T>(Dictionary<T, int> dict, T key, int value)
	{
		if (dict.ContainsKey(key))
		{
			dict[key] += value;
		}
		else
		{
			dict.Add(key, value);
		}
	}

	private void AddValueToDictionary<T>(Dictionary<T, DamageReflectedPayload> dict, T key, DamageReflectedPayload value)
	{
		if (dict.ContainsKey(key))
		{
			dict[key] += value;
		}
		else
		{
			dict.Add(key, value);
		}
	}

	private void AddValueToDictionary<T>(Dictionary<T, HealAttackerPayload> dict, T key, HealAttackerPayload value)
	{
		if (dict.ContainsKey(key))
		{
			dict[key] = value;
		}
		else
		{
			dict.Add(key, value);
		}
	}

	private static bool IsPrimaryDirectDamage(Character currentTarget, Character originalTarget, int damageIteration, int dmgTotal)
	{
		bool flag = currentTarget == originalTarget;
		bool flag2 = damageIteration == 1 || damageIteration == 2;
		return dmgTotal > 0 && flag && flag2;
	}
}
