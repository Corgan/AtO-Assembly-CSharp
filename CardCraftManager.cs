using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using WebSocketSharp;

public class CardCraftManager : MonoBehaviour
{
	public Transform deckUI;

	public Transform itemCorruptionUI;

	public DeckEnergy deckEnergy;

	public Transform canvasSearchT;

	public TMP_InputField searchInput;

	public TMP_InputField searchInputArmory;

	public GameObject SearchArmoryCanvas;

	public TMP_Text searchInputPlaceholder;

	public TextMeshProUGUI searchInputPlaceholderGUI;

	public TMP_Text searchInputText;

	public Transform canvasSearchCloseT;

	public Transform backgroundCraft;

	public Transform backgroundTransmute;

	public Transform backgroundRemove;

	public Transform backgroundDivination;

	public Transform backgroundCorruption;

	public Transform backgroundItems;

	public Transform backgroundChallenge;

	public Transform exitCraftButton;

	public GameObject cardVerticalPrefab;

	public GameObject cardCraftBuyButton;

	public GameObject cardCraftItemBuyButton;

	public GameObject cardCraftPageButton;

	public GameObject cardDivinationButton;

	public Transform cardCraftElements;

	public Transform cardCraftSave;

	public GameObject cardCraftItem;

	public RectTransform cardListContainerRectTransform;

	public Transform cardListContainer;

	public TMP_Text cardChallengeGlobalTitle;

	public TMP_Text cardChallengeGlobalIntro;

	public Transform[] cardChallengeContainer;

	public Transform[] cardChallengeButton;

	public TitleMovement[] cardChallengeTitle;

	public Transform[] cardChallengeSelected;

	public TMP_Text cardChallengeRound;

	public BotonGeneric rerollChallenge;

	public Transform challengeReadyBlock;

	public Transform readyChallenge;

	public Transform waitingMsgChallenge;

	public TMP_Text waitingMsgTextChallenge;

	public TMP_Text cardChallengeBonus;

	public PerkChallengeItem[] perkChallengeItems;

	public Transform challengePerks;

	public Transform cardUpgradeContainer;

	public Transform cardRemoveContainer;

	public Transform cardCraftContainer;

	public Transform cardItemContainer;

	public Transform cardCraftPageContainer;

	public Transform cardCraftEnergySelectorContainer;

	public Transform cardCraftRaritySelectorContainer;

	public Transform itemsCraftPageContainer;

	public Transform divinationButtonContainer;

	public Transform divinationWaitingContainer;

	public Transform cardCorruptionContainer;

	public Animator removeAnim;

	public Animator transformAnim;

	public Animator corruptAnim;

	public Transform tmpContainer;

	public TMP_Text divinationWaitingMsg;

	public TMP_Text cardsOwner;

	public Transform rerollButton;

	public Transform rerollButtonLock;

	public Transform rerollButtonWarning;

	public Transform petShopButton;

	public Transform itemShopButton;

	public Transform maxPetNumber;

	public Transform upgradeAText;

	public Transform upgradeBText;

	public TMP_Text oldcostAText;

	public TMP_Text oldcostBText;

	public Transform transformAText;

	public Transform transformBText;

	public Transform transformRemoveText;

	public TMP_Text oldcostRemoveText;

	public Transform usesLeftT;

	public TMP_Text usesLeftText;

	public Transform minCardsT;

	public TMP_Text minCardsText;

	public TMP_Text discountText;

	public ItemCombatIcon iconWeapon;

	public ItemCombatIcon iconArmor;

	public ItemCombatIcon iconAccesory;

	public ItemCombatIcon iconJewelry;

	public ItemCombatIcon iconPet;

	public TMP_Text itemsOwner;

	public BotonRollover iconDeck;

	public TMP_Text iconDeckText;

	public SpriteRenderer deckBgBorder;

	public Transform cardSingle0;

	public Transform cardSingle1;

	public Transform cardSingle2;

	public Transform arrowTR;

	public Transform arrowTL;

	public Transform arrowRL;

	public Transform arrowLR;

	public Transform buttonL;

	public Transform buttonR;

	public Transform buttonRemove;

	public Transform buttonCorruption;

	public List<Transform> corruptionArrows;

	public List<BotonGeneric> corruptionButtons;

	public Transform corruptionCharacterStats;

	public List<TMP_Text> corruptionTexts;

	public TMP_Text corruptionPercent;

	public TMP_Text corruptionPercentRoll;

	public Transform corruptionPercentRollSuccess;

	public Transform corruptionPercentRollFail;

	private int[] corruptionTry = new int[4];

	private int corruptionValue;

	public BotonAdvancedCraft buttonAffordableCraft;

	public BotonAdvancedCraft buttonAdvancedCraft;

	public BotonAdvancedCraft buttonFilterCraft;

	public Transform filterWindow;

	public ItemCorruptionIcon[] ItemCorruptionIcons = new ItemCorruptionIcon[4];

	public TMP_Text CorruptionTitle;

	public TMP_Text CorruptionDescription;

	private string itemListId;

	private bool isPetShop;

	private GameObject[] deckGOs;

	private Hero currentHero;

	public int heroIndex;

	private Coroutine blockedCoroutine;

	private int costA;

	private int costB;

	private int costRemove;

	private int costCorruption;

	private int costDust;

	private CardData cardData;

	private string cardActiveName = "";

	private CardVertical cardActive;

	private CardVertical[] cardVerticalDeck;

	private Vector3 posPlain = new Vector3(0f, 1.92f, -1f);

	private Vector3 posUpgradeA = new Vector3(-1.84f, -1.69f, -1f);

	private Vector3 posUpgradeB = new Vector3(1.84f, -1.69f, -1f);

	private Vector3 posRemove = new Vector3(0.03f, 0.63f, -1f);

	private CardItem CIPlain;

	private CardItem CIA;

	private CardItem CIB;

	private CardItem CIRemove;

	private CardItem CICorruption;

	private BotonGeneric BG_Left;

	private BotonGeneric BG_Right;

	private BotonGeneric BG_Remove;

	private BotonGeneric BG_Corruption;

	public bool blocked;

	private int discount;

	private int remainingUses = -1;

	private Coroutine craftCoroutine;

	private int currentItemsPageNum;

	private int currentCraftPageNum;

	private int maxCraftPageNum;

	private bool currentCraftAllRarities;

	private bool currentCraftAllCosts;

	private Dictionary<Enums.CardRarity, bool> currentCraftRarity;

	private int currentCraftCost;

	private CardCraftSelectorEnergy selectorEnergy;

	private Enums.CardRarity maxCraftRarity;

	private int craftTierZone;

	private Dictionary<int, CardCraftItem> craftCardItemDict = new Dictionary<int, CardCraftItem>();

	public List<string> craftFilterAura = new List<string>();

	public List<string> craftFilterCurse = new List<string>();

	public List<string> craftFilterDT = new List<string>();

	private List<BotonFilter> craftBotonFilters = new List<BotonFilter>();

	public GameObject portraitItem;

	public int craftType;

	private int savingSlot = -1;

	public DeckSlot[] deckSlot;

	public Transform loadDeckContainer;

	public Transform loadDeckCardContainer;

	public SpriteRenderer loadDeckHeroSprite;

	public TMP_Text deckCraftPrice;

	public TMP_Text loadDeckHeroName;

	public TMP_Text containerDeckName;

	public BotonGeneric botCraftingDeck;

	public BotonGeneric botSaveLoad;

	private int deckCraftingCostGold;

	private int deckCraftingCostDust;

	private Dictionary<string, int> craftTimes;

	public bool deckAvailableForSaveLoad = true;

	private string searchTerm = "";

	private Coroutine searchCo;

	private bool statusReady;

	public Transform waitingMsg;

	public TMP_Text waitingMsgText;

	private Coroutine manualReadyCo;

	private PhotonView photonView;

	public SpriteRenderer[] cardbackSprites;

	private Coroutine assignSpecial;

	private Coroutine assignCard;

	private int controllerBlock = -1;

	private int controllerVerticalIndex = -1;

	public int controllerHorizontalIndex = -1;

	private Vector2 warpPosition = Vector2.zero;

	private Vector3 auxVector = Vector3.zero;

	private List<Transform> _controllerList = new List<Transform>();

	private List<Transform> _controllerVerticalList = new List<Transform>();

	private bool controllerIsOnVerticalList;

	public Transform shadyDeal;

	public Transform shadyDealModelContainer;

	public TMP_Text shadyDealLeft;

	public Transform shadyDealButton;

	public TMP_Text shadyDealResult;

	private bool shadyDealInitiated;

	private int shadyDealGold;

	private int shadyDealDust;

	private int shadyRemaining;

	private GameObject shadyDealModel;

	public TMP_Text CorruptionTextTranslatable;

	public TMP_Text CorruptionDescriptionTranslatable;

	public TMP_Text itemCorruptionTextTitle;

	public static CardCraftManager Instance { get; private set; }

	public int CurrentItemsPageNum
	{
		get
		{
			return currentItemsPageNum;
		}
		set
		{
			currentItemsPageNum = value;
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
		BG_Right = buttonR.GetComponent<BotonGeneric>();
		BG_Left = buttonL.GetComponent<BotonGeneric>();
		BG_Remove = buttonRemove.GetComponent<BotonGeneric>();
		BG_Corruption = buttonCorruption.GetComponent<BotonGeneric>();
		currentCraftRarity = new Dictionary<Enums.CardRarity, bool>();
		currentCraftRarity.Add(Enums.CardRarity.Common, value: false);
		currentCraftRarity.Add(Enums.CardRarity.Uncommon, value: false);
		currentCraftRarity.Add(Enums.CardRarity.Rare, value: false);
		currentCraftRarity.Add(Enums.CardRarity.Epic, value: false);
		currentCraftRarity.Add(Enums.CardRarity.Mythic, value: false);
		if (MapManager.Instance != null)
		{
			photonView = MapManager.Instance.GetPhotonView();
		}
	}

	private void Start()
	{
		Resize();
	}

	public void DoMouseScroll(Vector2 vectorScroll)
	{
		if (!cardCraftElements.gameObject.activeSelf || maxCraftPageNum <= 1)
		{
			return;
		}
		if (vectorScroll.y > 0f)
		{
			RaycastHit2D raycastHit2D = Physics2D.Raycast(GameManager.Instance.cameraMain.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (!(raycastHit2D.collider != null) || !(raycastHit2D.transform != null) || !(raycastHit2D.transform.name.Split('_')[0] == "deckcard"))
			{
				DoPrevPage();
			}
		}
		else if (vectorScroll.y < 0f)
		{
			RaycastHit2D raycastHit2D2 = Physics2D.Raycast(GameManager.Instance.cameraMain.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (!(raycastHit2D2.collider != null) || !(raycastHit2D2.transform != null) || !(raycastHit2D2.transform.name.Split('_')[0] == "deckcard"))
			{
				DoNextPage();
			}
		}
	}

	public void AffordableCraft(bool change = false)
	{
		if (change)
		{
			AtOManager.Instance.affordableCraft = !AtOManager.Instance.affordableCraft;
			ShowListCardsForCraft(1, reset: true);
		}
		buttonAffordableCraft.SetState(AtOManager.Instance.affordableCraft);
		buttonAffordableCraft.GetComponent<BotonGeneric>().ShowDisableMask(!AtOManager.Instance.affordableCraft);
	}

	public void AdvancedCraft(bool change = false)
	{
		maxCraftPageNum = 1;
		if (change)
		{
			AtOManager.Instance.advancedCraft = !AtOManager.Instance.advancedCraft;
			ShowListCardsForCraft(1, reset: true);
		}
		buttonAdvancedCraft.SetState(AtOManager.Instance.advancedCraft);
		buttonAdvancedCraft.GetComponent<BotonGeneric>().ShowDisableMask(!AtOManager.Instance.advancedCraft);
	}

	public void ExitCardCraftAlert()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(ExitCardCraftAlert));
		AtOManager.Instance.CloseCardCraft();
	}

	public void ExitCardCraft()
	{
		if (!blocked)
		{
			AtOManager.Instance.CloseCardCraft();
		}
	}

	public void Resize()
	{
		exitCraftButton.transform.localPosition = new Vector3((0f - Globals.Instance.sizeW) * 0.5f + 1f * Globals.Instance.multiplierX, (0f - Globals.Instance.sizeH) * 0.5f + 3.72f * Globals.Instance.multiplierY, exitCraftButton.transform.localPosition.z);
	}

	private void DoNextPage()
	{
		if (craftType != 2 && craftType != 4)
		{
			return;
		}
		if (craftType == 2)
		{
			if (currentCraftPageNum == maxCraftPageNum)
			{
				ChangePage(1);
			}
			else
			{
				ChangePage(currentCraftPageNum + 1);
			}
		}
		else if (currentItemsPageNum == maxCraftPageNum)
		{
			ChangePage(1);
		}
		else
		{
			ChangePage(currentItemsPageNum + 1);
		}
	}

	private void DoPrevPage()
	{
		if (craftType != 2 && craftType != 4)
		{
			return;
		}
		if (craftType == 2)
		{
			if (currentCraftPageNum == 1)
			{
				ChangePage(maxCraftPageNum);
			}
			else
			{
				ChangePage(currentCraftPageNum - 1);
			}
		}
		else if (currentItemsPageNum == 1)
		{
			ChangePage(maxCraftPageNum);
		}
		else
		{
			ChangePage(currentItemsPageNum - 1);
		}
	}

	public async void ShowCardCraft(int type = 0)
	{
		waitingMsg.gameObject.SetActive(value: false);
		if (GameManager.Instance.IsMultiplayer())
		{
			if (!TownManager.Instance)
			{
				statusReady = false;
				exitCraftButton.GetComponent<BotonGeneric>().SetBackgroundColor(Functions.HexToColor(Globals.Instance.ClassColor["warrior"]));
				if (NetworkManager.Instance.IsMaster())
				{
					NetworkManager.Instance.ClearAllPlayerManualReady();
				}
			}
			else
			{
				TownManager.Instance.DisableReady();
			}
		}
		base.transform.localPosition = new Vector3(0f, 0.2f, base.transform.localPosition.z);
		craftType = type;
		if ((bool)MapManager.Instance)
		{
			AudioManager.Instance.DoBSO("Craft");
		}
		SetMaxQuantity();
		if ((bool)MapManager.Instance)
		{
			MapManager.Instance.HidePopup();
			if (MapManager.Instance.characterWindow.IsActive())
			{
				MapManager.Instance.characterWindow.Hide();
			}
			if (craftType != 3)
			{
				MapManager.Instance.sideCharacters.EnableOwnedCharacters();
				heroIndex = MapManager.Instance.sideCharacters.GetFirstEnabledCharacter();
				MapManager.Instance.sideCharacters.SetActive(heroIndex);
			}
			MapManager.Instance.sideCharacters.ShowLevelUpCharacters();
			MapManager.Instance.sideCharacters.InCharacterScreen(state: true);
		}
		else if ((bool)TownManager.Instance)
		{
			if (craftType != 3)
			{
				TownManager.Instance.sideCharacters.EnableOwnedCharacters();
				if (TownManager.Instance.LastUsedCharacter != -1)
				{
					heroIndex = TownManager.Instance.LastUsedCharacter;
				}
				else
				{
					heroIndex = TownManager.Instance.sideCharacters.GetFirstEnabledCharacter();
				}
				TownManager.Instance.sideCharacters.SetActive(heroIndex);
			}
			TownManager.Instance.sideCharacters.ShowLevelUpCharacters();
			TownManager.Instance.ShowButtons(state: false);
		}
		craftCardItemDict = new Dictionary<int, CardCraftItem>();
		currentHero = AtOManager.Instance.GetHero(heroIndex);
		SetCardbacks(heroIndex);
		if (craftType == 0)
		{
			backgroundTransmute.gameObject.SetActive(value: true);
			backgroundRemove.gameObject.SetActive(value: false);
			backgroundCraft.gameObject.SetActive(value: false);
			backgroundDivination.gameObject.SetActive(value: false);
			backgroundItems.gameObject.SetActive(value: false);
			backgroundChallenge.gameObject.SetActive(value: false);
			backgroundCorruption.gameObject.SetActive(value: false);
			minCardsT.gameObject.SetActive(value: false);
			StartCoroutine(ShowCardsSingle());
			if (AtOManager.Instance.TownTutorialStep == 1)
			{
				Hero hero = null;
				for (int i = 0; i < 4; i++)
				{
					hero = AtOManager.Instance.GetHero(i);
					if (hero != null)
					{
						_ = hero.SourceName == "Magnus";
					}
				}
				if (hero == null)
				{
					AtOManager.Instance.IncreaseTownTutorialStep();
				}
				else
				{
					GameManager.Instance.ShowTutorialPopup("townItemUpgrade", Vector3.zero, Vector3.zero);
				}
			}
		}
		else if (craftType == 1)
		{
			backgroundRemove.gameObject.SetActive(value: true);
			backgroundTransmute.gameObject.SetActive(value: false);
			backgroundCraft.gameObject.SetActive(value: false);
			backgroundDivination.gameObject.SetActive(value: false);
			backgroundItems.gameObject.SetActive(value: false);
			backgroundChallenge.gameObject.SetActive(value: false);
			backgroundCorruption.gameObject.SetActive(value: false);
			if (!AtOManager.Instance.Sandbox_noMinimumDecksize)
			{
				minCardsT.gameObject.SetActive(value: true);
				minCardsText.text = "* " + Texts.Instance.GetText("minDeckCards").Replace("<n>", 15.ToString());
			}
			else
			{
				minCardsT.gameObject.SetActive(value: false);
			}
		}
		else if (craftType == 2)
		{
			ShowSearch(state: true);
			backgroundCraft.gameObject.SetActive(value: true);
			cardCraftElements.gameObject.SetActive(value: true);
			cardCraftSave.gameObject.SetActive(value: false);
			backgroundTransmute.gameObject.SetActive(value: false);
			backgroundRemove.gameObject.SetActive(value: false);
			backgroundDivination.gameObject.SetActive(value: false);
			backgroundItems.gameObject.SetActive(value: false);
			backgroundChallenge.gameObject.SetActive(value: false);
			backgroundCorruption.gameObject.SetActive(value: false);
			minCardsT.gameObject.SetActive(value: false);
			canvasSearchCloseT.gameObject.SetActive(value: false);
			filterWindow.gameObject.SetActive(value: false);
			searchInputPlaceholder.text = "<voffset=-4><size=+10><sprite name=cards></size></voffset>" + Texts.Instance.GetText("searchCards");
			currentCraftAllCosts = true;
			currentCraftAllRarities = true;
			currentCraftPageNum = 1;
			maxCraftPageNum = 1;
			craftTierZone = AtOManager.Instance.GetTownTier();
			bool flag = true;
			if ((bool)TownManager.Instance)
			{
				flag = false;
			}
			if (flag)
			{
				AtOManager.Instance.advancedCraft = false;
				AtOManager.Instance.affordableCraft = false;
				ResetFilter();
			}
			else
			{
				AdvancedCraft();
				AffordableCraft();
				SetFilterButtonState();
				ShowListCardsForCraft(1);
			}
			if ((bool)MapManager.Instance)
			{
				botSaveLoad.gameObject.SetActive(value: false);
				buttonAdvancedCraft.gameObject.SetActive(value: false);
			}
			else if ((bool)TownManager.Instance)
			{
				if (AtOManager.Instance.GetTownTier() == 0 && (!GameManager.Instance.IsMultiplayer() || (AtOManager.Instance.GetHero(heroIndex) != null && AtOManager.Instance.GetHero(heroIndex).HeroData != null && AtOManager.Instance.GetHero(heroIndex).Owner == NetworkManager.Instance.GetPlayerNick())))
				{
					botSaveLoad.gameObject.SetActive(value: true);
				}
				else
				{
					botSaveLoad.gameObject.SetActive(value: false);
				}
				buttonAdvancedCraft.gameObject.SetActive(value: true);
				if (AtOManager.Instance.TownTutorialStep == 0)
				{
					Hero hero2 = null;
					for (int j = 0; j < 4; j++)
					{
						hero2 = AtOManager.Instance.GetHero(j);
						if (hero2 != null)
						{
							_ = hero2.SourceName == "Evelyn";
						}
					}
					if (hero2 == null || hero2.Cards.Contains("fireball"))
					{
						AtOManager.Instance.IncreaseTownTutorialStep();
					}
					else
					{
						GameManager.Instance.ShowTutorialPopup("townItemCraft", Vector3.zero, Vector3.zero);
					}
				}
			}
			if (GameManager.Instance.IsSingularity())
			{
				buttonAdvancedCraft.gameObject.SetActive(value: false);
				buttonAffordableCraft.gameObject.SetActive(value: false);
				if (AtOManager.Instance.advancedCraft)
				{
					AtOManager.Instance.advancedCraft = false;
					AtOManager.Instance.affordableCraft = false;
					ResetFilter();
				}
			}
		}
		else if (craftType == 3)
		{
			backgroundDivination.gameObject.SetActive(value: true);
			backgroundTransmute.gameObject.SetActive(value: false);
			backgroundRemove.gameObject.SetActive(value: false);
			backgroundCraft.gameObject.SetActive(value: false);
			backgroundItems.gameObject.SetActive(value: false);
			backgroundChallenge.gameObject.SetActive(value: false);
			backgroundCorruption.gameObject.SetActive(value: false);
			minCardsT.gameObject.SetActive(value: false);
			deckUI.gameObject.SetActive(value: false);
			deckEnergy.gameObject.SetActive(value: false);
			craftTierZone = AtOManager.Instance.GetTownTier();
			SetDivinationButtons();
			if (AtOManager.Instance.GetTownDivinationTier() != null)
			{
				SetDivinationWaiting();
			}
		}
		else if (craftType == 4)
		{
			ClearItemListContainer();
			backgroundItems.gameObject.SetActive(value: true);
			if (!AtOManager.Instance.IsCombatTool)
			{
				SearchArmoryCanvas.gameObject.SetActive(value: false);
			}
			backgroundTransmute.gameObject.SetActive(value: false);
			backgroundRemove.gameObject.SetActive(value: false);
			backgroundCraft.gameObject.SetActive(value: false);
			backgroundDivination.gameObject.SetActive(value: false);
			backgroundChallenge.gameObject.SetActive(value: false);
			backgroundCorruption.gameObject.SetActive(value: false);
			minCardsT.gameObject.SetActive(value: false);
			deckUI.gameObject.SetActive(value: false);
			deckEnergy.gameObject.SetActive(value: false);
			craftTierZone = AtOManager.Instance.GetTownTier();
			currentItemsPageNum = 1;
			shadyDealInitiated = false;
			ShowItemsForBuy(currentItemsPageNum);
			ShowCharacterItems();
			if (!isPetShop && AtOManager.Instance.TownTutorialStep == 2 && AtOManager.Instance.CharInTown())
			{
				bool flag2 = false;
				for (int k = 0; k < craftCardItemDict.Count; k++)
				{
					if (!(craftCardItemDict[k].cardId == "spyglass") || !craftCardItemDict[k].Available)
					{
						continue;
					}
					GameManager.Instance.ShowTutorialPopup("townItemLoot", craftCardItemDict[k].buttonItem.position, Vector3.zero);
					for (int l = 0; l < craftCardItemDict.Count; l++)
					{
						if (l != k)
						{
							craftCardItemDict[l].EnableButton(_state: false);
						}
					}
					flag2 = true;
					break;
				}
				if (!flag2)
				{
					AtOManager.Instance.IncreaseTownTutorialStep();
				}
			}
		}
		else if (craftType == 5)
		{
			backgroundChallenge.gameObject.SetActive(value: true);
			backgroundTransmute.gameObject.SetActive(value: false);
			backgroundRemove.gameObject.SetActive(value: false);
			backgroundCraft.gameObject.SetActive(value: false);
			backgroundDivination.gameObject.SetActive(value: false);
			backgroundItems.gameObject.SetActive(value: false);
			backgroundCorruption.gameObject.SetActive(value: false);
			minCardsT.gameObject.SetActive(value: false);
			exitCraftButton.gameObject.SetActive(value: false);
			heroIndex = -1;
			SelectCharacter(0);
		}
		else if (craftType == 6)
		{
			corruptAnim.SetTrigger("hide");
			corruptionTry = new int[4];
			for (int m = 0; m < 4; m++)
			{
				corruptionTry[m] = 0;
			}
			backgroundCorruption.gameObject.SetActive(value: true);
			backgroundRemove.gameObject.SetActive(value: false);
			backgroundTransmute.gameObject.SetActive(value: false);
			backgroundCraft.gameObject.SetActive(value: false);
			backgroundDivination.gameObject.SetActive(value: false);
			backgroundItems.gameObject.SetActive(value: false);
			backgroundChallenge.gameObject.SetActive(value: false);
			minCardsT.gameObject.SetActive(value: false);
			UnityEngine.Random.InitState(string.Concat(AtOManager.Instance.currentMapNode + AtOManager.Instance.GetGameId() + NetworkManager.Instance.GetPlayerNick(), MapManager.Instance.GetRandomString()).GetDeterministicHashCode());
			InitCorruption();
		}
		else if (craftType == 7)
		{
			corruptAnim.SetTrigger("hide");
			corruptionTry = new int[4];
			for (int n = 0; n < 4; n++)
			{
				corruptionTry[n] = 0;
			}
			backgroundCorruption.gameObject.SetActive(value: true);
			backgroundRemove.gameObject.SetActive(value: false);
			backgroundTransmute.gameObject.SetActive(value: false);
			backgroundCraft.gameObject.SetActive(value: false);
			backgroundDivination.gameObject.SetActive(value: false);
			backgroundItems.gameObject.SetActive(value: false);
			backgroundChallenge.gameObject.SetActive(value: false);
			minCardsT.gameObject.SetActive(value: false);
			UnityEngine.Random.InitState(string.Concat(AtOManager.Instance.currentMapNode + AtOManager.Instance.GetGameId() + NetworkManager.Instance.GetPlayerNick(), DateTime.UtcNow.Second.ToString()).GetDeterministicHashCode());
			deckUI.gameObject.SetActive(value: false);
			itemCorruptionUI.gameObject.SetActive(value: true);
			ShowCharacterCorruptionItems();
			InitCorruption();
			await SetText(CorruptionTextTranslatable, Texts.Instance.GetText("itemCorruptionTitle"));
			await SetText(CorruptionDescriptionTranslatable, Texts.Instance.GetText("itemCorruptionDesc"));
		}
		if (craftType < 3 || craftType == 6)
		{
			CreateDeck(heroIndex);
		}
	}

	public void ShowSearchFocus()
	{
		StartCoroutine(ShowSearchFocusCo());
	}

	private async Task SetText(TMP_Text textField, string content)
	{
		await Task.Delay(20);
		textField.SetText(content);
	}

	private IEnumerator ShowSearchFocusCo()
	{
		yield return Globals.Instance.WaitForSeconds(0.1f);
		if (searchInput.text == "")
		{
			searchInputPlaceholder.GetComponent<TextMeshProUGUI>().enabled = false;
		}
	}

	public void ShowSearch(bool state)
	{
		canvasSearchT.gameObject.SetActive(state);
	}

	public void ResetSearch()
	{
		searchInput.text = "";
	}

	public void ResetSearchArmory()
	{
		searchInputArmory.text = "";
	}

	public void Search(string _term)
	{
		if (searchCo != null)
		{
			StopCoroutine(searchCo);
		}
		searchCo = StartCoroutine(SearchCoroutine(_term));
	}

	private IEnumerator SearchCoroutine(string _term)
	{
		if (_term != "")
		{
			yield return Globals.Instance.WaitForSeconds(0.35f);
			searchTerm = _term.Trim();
			canvasSearchCloseT.gameObject.SetActive(value: true);
		}
		else
		{
			searchTerm = "";
			canvasSearchCloseT.gameObject.SetActive(value: false);
		}
		if (craftType == 2)
		{
			ShowListCardsForCraft(1, reset: true);
		}
		else if (craftType == 4)
		{
			ShowItemsForBuy();
		}
	}

	private IEnumerator ShowCardsSingle()
	{
		yield return Globals.Instance.WaitForSeconds(0.05f);
		cardSingle0.gameObject.SetActive(value: true);
		yield return Globals.Instance.WaitForSeconds(0.1f);
		cardSingle1.gameObject.SetActive(value: true);
		yield return Globals.Instance.WaitForSeconds(0.1f);
		cardSingle2.gameObject.SetActive(value: true);
	}

	private void SetCardbacks(int _heroIndex)
	{
		Hero hero = AtOManager.Instance.GetHero(_heroIndex);
		if (hero == null || hero.HeroData == null)
		{
			return;
		}
		string cardbackUsed = hero.CardbackUsed;
		if (!(cardbackUsed != ""))
		{
			return;
		}
		CardbackData cardbackData = Globals.Instance.GetCardbackData(cardbackUsed);
		if (cardbackData == null)
		{
			cardbackData = Globals.Instance.GetCardbackData(Globals.Instance.GetCardbackBaseIdBySubclass(hero.HeroData.HeroSubClass.Id));
			if (cardbackData == null)
			{
				cardbackData = Globals.Instance.GetCardbackData("defaultCardback");
			}
		}
		Sprite cardbackSprite = cardbackData.CardbackSprite;
		if (!(cardbackSprite != null))
		{
			return;
		}
		for (int i = 0; i < cardbackSprites.Length; i++)
		{
			if (cardbackSprites[i] != null)
			{
				cardbackSprites[i].sprite = cardbackSprite;
			}
		}
	}

	public void SelectCharacter(int characterIndex)
	{
		if (characterIndex == heroIndex)
		{
			return;
		}
		ClearTmpContainer();
		if (craftCoroutine != null)
		{
			StopCoroutine(craftCoroutine);
			craftCoroutine = null;
		}
		heroIndex = characterIndex;
		SetCardbacks(heroIndex);
		if (craftType != 3)
		{
			CreateDeck(characterIndex);
		}
		controllerVerticalIndex = -1;
		if (craftType == 0)
		{
			TMP_Text tMP_Text = oldcostAText;
			string text = (oldcostBText.text = "");
			tMP_Text.text = text;
			transformAnim.SetTrigger("hide");
		}
		else if (craftType == 1)
		{
			oldcostRemoveText.text = "";
			removeAnim.SetTrigger("hide");
		}
		else if (craftType == 2)
		{
			if (cardCraftElements.gameObject.activeSelf)
			{
				currentCraftPageNum = 1;
				maxCraftPageNum = 1;
				ShowListCardsForCraft(1);
			}
			else if (!GameManager.Instance.IsMultiplayer() || AtOManager.Instance.GetHero(characterIndex).Owner == NetworkManager.Instance.GetPlayerNick())
			{
				LoadDecks();
			}
			else
			{
				ShowSaveLoad();
			}
		}
		else if (craftType == 4)
		{
			ShowCharacterItems();
			if (TownManager.Instance != null && TownManager.Instance.characterWindow.IsActive())
			{
				TownManager.Instance.ShowDeck(characterIndex);
			}
			else if (MapManager.Instance != null && MapManager.Instance.characterWindow.IsActive())
			{
				MapManager.Instance.ShowDeck(characterIndex);
			}
			if (AtOManager.Instance.CharInTown() && AtOManager.Instance.TownTutorialStep == 2)
			{
				return;
			}
			ShowItemsForBuy(currentItemsPageNum);
		}
		else if (craftType == 6)
		{
			InitCorruption();
			corruptAnim.SetTrigger("hide");
		}
		else if (craftType == 7)
		{
			InitCorruption();
			ShowCharacterCorruptionItems();
			if (TownManager.Instance != null && TownManager.Instance.characterWindow.IsActive())
			{
				TownManager.Instance.ShowDeck(characterIndex);
			}
			else if (MapManager.Instance != null && MapManager.Instance.characterWindow.IsActive())
			{
				MapManager.Instance.ShowDeck(characterIndex);
			}
			if (AtOManager.Instance.CharInTown() && AtOManager.Instance.TownTutorialStep == 2)
			{
				return;
			}
		}
		if (!(TownManager.Instance != null))
		{
			return;
		}
		if (!GameManager.Instance.IsMultiplayer() || AtOManager.Instance.GetHero(characterIndex).Owner == NetworkManager.Instance.GetPlayerNick())
		{
			TownManager.Instance.LastUsedCharacter = characterIndex;
		}
		if (craftType == 2)
		{
			if (AtOManager.Instance.GetTownTier() == 0 && (!GameManager.Instance.IsMultiplayer() || (AtOManager.Instance.GetHero(heroIndex) != null && AtOManager.Instance.GetHero(heroIndex).Owner == NetworkManager.Instance.GetPlayerNick())))
			{
				botSaveLoad.gameObject.SetActive(value: true);
			}
			else
			{
				botSaveLoad.gameObject.SetActive(value: false);
			}
		}
	}

	public void SetDiscount(int _discount)
	{
		discount = _discount;
		if (discount != 0)
		{
			discountText.gameObject.SetActive(value: true);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<size=+1>");
			stringBuilder.Append(Mathf.Abs(discount));
			stringBuilder.Append("%</size><br>");
			if (discount > 0)
			{
				discountText.color = Globals.Instance.ColorColor["greenCard"];
				discountText.text = string.Format(Texts.Instance.GetText("shopDiscount"), stringBuilder.ToString());
			}
			else
			{
				discountText.color = Globals.Instance.ColorColor["redCard"];
				discountText.text = string.Format(Texts.Instance.GetText("shopIncrease"), stringBuilder.ToString());
			}
		}
		else
		{
			discountText.gameObject.SetActive(value: false);
		}
	}

	public void SetMaxCraftRarity(Enums.CardRarity _maxCraftRarity)
	{
		maxCraftRarity = _maxCraftRarity;
	}

	public void SetMaxQuantity(int _maxQuantity = -1)
	{
		for (int i = 0; i < 4; i++)
		{
			AtOManager.Instance.SetCraftReaminingUses(i, _maxQuantity);
		}
		ShowRemainingUses();
	}

	private void ShowRemainingUses()
	{
		int num = AtOManager.Instance.GetCraftReaminingUses(heroIndex);
		if (craftType == 6 || craftType == 7)
		{
			num = 3 - corruptionTry[heroIndex];
		}
		if (num > 0)
		{
			usesLeftT.gameObject.SetActive(value: true);
			usesLeftText.text = Texts.Instance.GetText("reaminingUses") + ": <color=#FFFFFF>" + num;
			buttonAffordableCraft.gameObject.SetActive(value: false);
		}
		else if (num == 0)
		{
			usesLeftT.gameObject.SetActive(value: true);
			usesLeftText.text = Texts.Instance.GetText("noMoreUses");
			buttonAffordableCraft.gameObject.SetActive(value: false);
		}
		else
		{
			usesLeftT.gameObject.SetActive(value: false);
		}
		if (craftType != 2)
		{
			usesLeftT.localPosition = new Vector3(0.45f, 4f, 0f);
		}
		else
		{
			usesLeftT.localPosition = new Vector3(4.02f, -4.89f, 0f);
		}
		remainingUses = num;
	}

	public bool CanBuy(string type, string cardId = "")
	{
		if (remainingUses == 0)
		{
			return false;
		}
		if (GameManager.Instance.IsMultiplayer() && AtOManager.Instance.GetHero(heroIndex) != null && AtOManager.Instance.GetHero(heroIndex).Owner != NetworkManager.Instance.GetPlayerNick())
		{
			return false;
		}
		cardId = cardId.Trim();
		int playerDust = AtOManager.Instance.GetPlayerDust();
		int playerGold = AtOManager.Instance.GetPlayerGold();
		bool result = false;
		if (type == "A" && costA <= playerDust)
		{
			result = true;
		}
		else if (type == "B" && costB <= playerDust)
		{
			result = true;
		}
		else if (type == "Remove" && costRemove <= playerGold)
		{
			result = true;
		}
		else
		{
			switch (type)
			{
			case "Craft":
				if (SetPrice("Craft", "", cardId, craftTierZone) <= playerDust)
				{
					result = true;
				}
				break;
			case "Item":
			{
				CardData cardData = Globals.Instance.GetCardData(cardId, instantiate: false);
				string rarity = Enum.GetName(typeof(Enums.CardRarity), cardData.CardRarity);
				if (SetPrice("Item", rarity, cardId, craftTierZone) <= playerGold)
				{
					result = true;
				}
				break;
			}
			case "Reroll":
				if (!AtOManager.Instance.IsTownRerollAvailable())
				{
					return false;
				}
				if (Globals.Instance.GetCostReroll() <= playerGold)
				{
					result = true;
				}
				break;
			case "Corruption":
				if (SetPrice("Corruption", "") <= playerDust)
				{
					result = true;
				}
				break;
			}
		}
		return result;
	}

	public void RemoveCard()
	{
		if (CanBuy("Remove"))
		{
			craftCoroutine = StartCoroutine(BuyRemoveCo());
		}
	}

	private IEnumerator BuyRemoveCo()
	{
		if (!blocked)
		{
			GameManager.Instance.PlayLibraryAudio("ui_anvil");
			SetBlocked(_status: true);
			buttonRemove.gameObject.SetActive(value: false);
			cardActiveName = "";
			ShowElements("");
			CIRemove.SetDestinationScaleRotation(CIRemove.transform.localPosition, 0f, Quaternion.Euler(0f, 0f, 180f));
			CIRemove.Vanish();
			CIRemove.cardrevealed = true;
			int cardIndex = int.Parse(CIRemove.transform.gameObject.name.Split('_')[2]);
			AtOManager.Instance.PayGold(costRemove);
			AtOManager.Instance.RemoveCardInDeck(heroIndex, cardIndex);
			AtOManager.Instance.SubstractCraftReaminingUses(heroIndex);
			ShowRemainingUses();
			removeAnim.SetTrigger("hide");
			oldcostRemoveText.text = "";
			if (costRemove > 0)
			{
				AtOManager.Instance.HeroCraftRemoved(heroIndex);
			}
			AtOManager.Instance.SideBarRefreshCards(heroIndex);
			SetControllerIntoVerticalList();
			yield return Globals.Instance.WaitForSeconds(0.5f);
			if (CIRemove != null)
			{
				UnityEngine.Object.Destroy(CIRemove.gameObject);
			}
			CreateDeck(heroIndex);
			SetBlocked(_status: false);
		}
	}

	public void BuyUpgrade(string type)
	{
		if (CanBuy(type))
		{
			craftCoroutine = StartCoroutine(BuyUpgradeCo(type));
		}
	}

	private IEnumerator BuyUpgradeCo(string type)
	{
		if (!blocked)
		{
			SetBlocked(_status: true);
			cardActiveName = "";
			ShowElements("");
			CIPlain.SetDestinationScaleRotation(CIPlain.transform.localPosition, 0f, Quaternion.Euler(0f, 0f, 180f));
			CIPlain.Vanish();
			CIPlain.cardrevealed = true;
			CardItem CIselected = null;
			CardItem cardItem = null;
			if (type == "A")
			{
				CIselected = CIA;
				cardItem = CIB;
				costDust = costA;
			}
			else if (type == "B")
			{
				CIselected = CIB;
				cardItem = CIA;
				costDust = costB;
			}
			int cardIndex = int.Parse(CIselected.transform.gameObject.name.Split('_')[2]);
			CardData cardDataAux = Functions.GetCardDataFromCardData(cardData, type);
			AtOManager.Instance.PayDust(costDust);
			AtOManager.Instance.ReplaceCardInDeck(heroIndex, cardIndex, cardDataAux.Id);
			AtOManager.Instance.SubstractCraftReaminingUses(heroIndex);
			AtOManager.Instance.SaveUpgradedCardInAltar(heroIndex);
			if (CIPlain != null && CIPlain.CardData != null && CIPlain.CardData.Id == "faststrike" && AtOManager.Instance.TownTutorialStep == 1 && AtOManager.Instance.CharInTown())
			{
				AtOManager.Instance.IncreaseTownTutorialStep();
				AlertManager.buttonClickDelegate = ExitCardCraftAlert;
				AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("tutorialTownUpgradeDone"));
			}
			deckEnergy.WriteEnergy(heroIndex, 0);
			ShowRemainingUses();
			SetControllerIntoVerticalList();
			transformAnim.SetTrigger("hide");
			TMP_Text tMP_Text = oldcostAText;
			string text = (oldcostBText.text = "");
			tMP_Text.text = text;
			if (costDust > 0)
			{
				AtOManager.Instance.HeroCraftUpgraded(heroIndex);
			}
			cardItem.SetDestinationScaleRotation(cardItem.transform.localPosition, 0f, Quaternion.Euler(0f, 0f, 180f));
			cardItem.Vanish();
			cardItem.cardrevealed = true;
			bool showParticles = true;
			if (deckGOs[cardIndex].transform.position.y < -3.2f || deckGOs[cardIndex].transform.position.y > 3.7f)
			{
				showParticles = false;
			}
			if (showParticles)
			{
				yield return Globals.Instance.WaitForSeconds(0.2f);
				CIselected.cardrevealed = true;
				CIselected.EnableTrail();
				CIselected.TopLayeringOrder("UI", 20000);
				CIselected.PlayDissolveParticle();
				SetBlocked(_status: false);
				CIselected.SetDestinationScaleRotation(deckGOs[cardIndex].transform.position + new Vector3(-1.1f, -0.6f, 0f), 0f, Quaternion.Euler(0f, 0f, 180f));
				yield return Globals.Instance.WaitForSeconds(0.4f);
			}
			else
			{
				CIselected.SetDestinationScaleRotation(CIselected.transform.localPosition, 0f, Quaternion.Euler(0f, 0f, 180f));
				CIselected.Vanish();
				CIselected.cardrevealed = true;
				yield return Globals.Instance.WaitForSeconds(0.2f);
			}
			cardVerticalDeck[cardIndex].ReplaceWithCard(cardDataAux, type, showParticles);
			SetBlocked(_status: false);
		}
	}

	public void BuyCraft(string cardId)
	{
		if ((!blocked && CanBuy("Craft", cardId)) || AtOManager.Instance.IsCombatTool)
		{
			SetBlocked(_status: true);
			craftCoroutine = StartCoroutine(BuyCraftCo(cardId));
		}
	}

	private IEnumerator BuyCraftCo(string cardId)
	{
		cardId = cardId.Trim();
		GameManager.Instance.PlayLibraryAudio("ui_anvil");
		int costDust = SetPrice("Craft", "", cardId, craftTierZone);
		AtOManager.Instance.PayDust(costDust);
		AtOManager.Instance.SubstractCraftReaminingUses(heroIndex);
		ShowRemainingUses();
		if (!GameManager.Instance.IsMultiplayer())
		{
			AtOManager.Instance.AddCardToHero(heroIndex, cardId);
			AtOManager.Instance.SideBarRefreshCards(heroIndex);
			if (cardId == "fireball" && AtOManager.Instance.TownTutorialStep == 0 && AtOManager.Instance.CharInTown())
			{
				AtOManager.Instance.IncreaseTownTutorialStep();
				AlertManager.buttonClickDelegate = ExitCardCraftAlert;
				AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("tutorialTownCraftDone"));
			}
		}
		else
		{
			int actualCards = AtOManager.Instance.GetHero(heroIndex).Cards.Count;
			AtOManager.Instance.AddCardToHeroMP(heroIndex, cardId);
			while (AtOManager.Instance.GetHero(heroIndex).Cards.Count == actualCards)
			{
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
		}
		string cardId2 = cardId;
		CardData cardData = Globals.Instance.GetCardData(cardId, instantiate: false);
		if (cardData.CardUpgraded != Enums.CardUpgraded.No && cardData.UpgradedFrom != "")
		{
			cardId2 = cardData.UpgradedFrom.ToLower();
		}
		AtOManager.Instance.SaveCraftedCard(heroIndex, cardId2);
		ShowListCardsForCraft(currentCraftPageNum);
		if (costDust > 0)
		{
			AtOManager.Instance.HeroCraftCrafted(heroIndex);
		}
		yield return Globals.Instance.WaitForSeconds(0.05f);
		GameObject gameObject = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, GameManager.Instance.cameraMain.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0f, 2.5f, 0f), Quaternion.identity, tmpContainer);
		CardItem CI = gameObject.GetComponent<CardItem>();
		CI.SetCard(cardId, deckScale: false, currentHero);
		CI.TopLayeringOrder("UI", -500);
		CI.DrawEnergyCost();
		_ = CI.CardData;
		CI.cardrevealed = true;
		CI.EnableTrail();
		CI.TopLayeringOrder("UI", 20000);
		CI.SetDestinationLocalScale(1.6f);
		yield return Globals.Instance.WaitForSeconds(0.3f);
		CreateDeck(heroIndex);
		yield return Globals.Instance.WaitForSeconds(0.2f);
		CI.SetDestinationLocalScale(0f);
		yield return Globals.Instance.WaitForSeconds(0.1f);
		UnityEngine.Object.Destroy(CI.gameObject);
		Vector3 to = ((!TownManager.Instance) ? MapManager.Instance.sideCharacters.CharacterIconPosition(heroIndex) : TownManager.Instance.sideCharacters.CharacterIconPosition(heroIndex));
		GameManager.Instance.GenerateParticleTrail(0, Vector3.zero, to);
		yield return Globals.Instance.WaitForSeconds(0.7f);
		SetBlocked(_status: false);
	}

	private IEnumerator BuyItemCo(string cardId)
	{
		int goldCost = SetPrice("Item", "", cardId, craftTierZone);
		AtOManager.Instance.PayGold(goldCost);
		PlayerManager.Instance.PurchasedItem();
		if (!GameManager.Instance.IsMultiplayer())
		{
			AtOManager.Instance.AddItemToHero(heroIndex, cardId, itemListId);
			if (cardId == "spyglass" && AtOManager.Instance.TownTutorialStep == 2 && AtOManager.Instance.CharInTown())
			{
				AtOManager.Instance.IncreaseTownTutorialStep();
				AlertManager.buttonClickDelegate = ExitCardCraftAlert;
				AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("tutorialTownLootDone"));
			}
		}
		else
		{
			AtOManager.Instance.AddItemToHeroMP(heroIndex, cardId, itemListId);
		}
		yield return Globals.Instance.WaitForSeconds(0.05f);
		GameManager.Instance.PlayLibraryAudio("ui_equip_item");
		GameObject gameObject = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, GameManager.Instance.cameraMain.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0f, 2.5f, 0f), Quaternion.identity, tmpContainer);
		CardItem CI = gameObject.GetComponent<CardItem>();
		CI.SetCard(cardId, deckScale: false, currentHero);
		CI.TopLayeringOrder("UI", -500);
		CI.DrawEnergyCost();
		CardData cardData = CI.CardData;
		CI.cardrevealed = true;
		CI.EnableTrail();
		CI.TopLayeringOrder("UI", 25000);
		CI.SetDestinationLocalScale(1.6f);
		yield return Globals.Instance.WaitForSeconds(0.5f);
		CI.SetDestinationLocalScale(0f);
		yield return Globals.Instance.WaitForSeconds(0.1f);
		UnityEngine.Object.Destroy(CI.gameObject);
		string text = Enum.GetName(typeof(Enums.CardType), cardData.CardType);
		Vector3 position = base.transform.Find("backgroundItems/CharacterItemBlock/CharacterItems/" + text).transform.position;
		GameManager.Instance.GenerateParticleTrail(0, Vector3.zero, position);
		yield return Globals.Instance.WaitForSeconds(0.2f);
		ShowCharacterItems();
		SetBlocked(_status: false);
	}

	private void SetBlocked(bool _status)
	{
		blocked = _status;
		if (blockedCoroutine != null)
		{
			StopCoroutine(blockedCoroutine);
		}
		if (blocked)
		{
			blockedCoroutine = StartCoroutine(BlockedCo());
		}
	}

	private IEnumerator BlockedCo()
	{
		yield return Globals.Instance.WaitForSeconds(5f);
		blocked = false;
	}

	public void BuyItem(string cardId)
	{
		if (blocked)
		{
			return;
		}
		SetBlocked(_status: true);
		if (CanBuy("Item", cardId))
		{
			if (cardId == "spyglass" && AtOManager.Instance.TownTutorialStep == 2 && AtOManager.Instance.CharInTown() && currentHero.SourceName != "Andrin")
			{
				AlertManager.Instance.MustSelect("Andrin");
				SetBlocked(_status: false);
			}
			else
			{
				SetBlocked(_status: true);
				craftCoroutine = StartCoroutine(BuyItemCo(cardId));
			}
		}
		else
		{
			SetBlocked(_status: false);
		}
	}

	public string ButtonText(int cost)
	{
		if (cost != 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (craftType == 0)
			{
				stringBuilder.Append("<sprite name=dust> ");
			}
			else if (craftType == 1)
			{
				stringBuilder.Append("<sprite name=gold> ");
			}
			else if (craftType == 2)
			{
				stringBuilder.Append("<sprite name=dust> ");
			}
			else if (craftType == 4)
			{
				stringBuilder.Append("<sprite name=gold> ");
			}
			else if (craftType == 6 || craftType == 7)
			{
				stringBuilder.Append("<sprite name=dust> ");
			}
			stringBuilder.Append(cost);
			return stringBuilder.ToString();
		}
		return Texts.Instance.GetText("freeCost");
	}

	public void ShowElements(string direction, string cardId = "")
	{
		if (craftType == 0)
		{
			if (direction == "R")
			{
				arrowLR.gameObject.SetActive(value: true);
				buttonR.gameObject.SetActive(value: true);
				transformBText.gameObject.SetActive(value: true);
				BG_Right.SetText(ButtonText(costB));
			}
			else
			{
				arrowLR.gameObject.SetActive(value: false);
				buttonR.gameObject.SetActive(value: false);
				transformBText.gameObject.SetActive(value: false);
			}
			if (direction == "L")
			{
				arrowRL.gameObject.SetActive(value: true);
				buttonL.gameObject.SetActive(value: true);
				transformAText.gameObject.SetActive(value: true);
				BG_Left.SetText(ButtonText(costA));
			}
			else
			{
				arrowRL.gameObject.SetActive(value: false);
				buttonL.gameObject.SetActive(value: false);
				transformAText.gameObject.SetActive(value: false);
			}
			if (direction == "T")
			{
				arrowTR.gameObject.SetActive(value: true);
				arrowTL.gameObject.SetActive(value: true);
				buttonL.gameObject.SetActive(value: true);
				buttonR.gameObject.SetActive(value: true);
				upgradeAText.gameObject.SetActive(value: true);
				upgradeBText.gameObject.SetActive(value: true);
				BG_Left.SetText(ButtonText(costA));
				BG_Right.SetText(ButtonText(costB));
			}
			else
			{
				arrowTR.gameObject.SetActive(value: false);
				arrowTL.gameObject.SetActive(value: false);
				upgradeAText.gameObject.SetActive(value: false);
				upgradeBText.gameObject.SetActive(value: false);
			}
			if (!CanBuy("A"))
			{
				BG_Left.Disable();
			}
			else
			{
				BG_Left.Enable();
			}
			if (!CanBuy("B"))
			{
				BG_Right.Disable();
			}
			else
			{
				BG_Right.Enable();
			}
			if (AtOManager.Instance.TownTutorialStep == 1 && CIPlain != null && CIPlain.CardData != null && CIPlain.CardData.Id != "faststrike")
			{
				BG_Left.Disable();
				BG_Right.Disable();
			}
		}
		else
		{
			if (craftType != 1)
			{
				return;
			}
			if (direction == "")
			{
				buttonRemove.gameObject.SetActive(value: false);
				transformRemoveText.gameObject.SetActive(value: false);
				return;
			}
			buttonRemove.gameObject.SetActive(value: true);
			transformRemoveText.gameObject.SetActive(value: true);
			bool flag = true;
			bool flag2 = false;
			CardData cardData = Globals.Instance.GetCardData(cardId, instantiate: false);
			if (cardData.CardClass == Enums.CardClass.Injury && AtOManager.Instance.GetNgPlus() >= 9 && AtOManager.Instance.CharInTown())
			{
				flag2 = true;
			}
			else if (cardData.CardClass == Enums.CardClass.Injury || cardData.CardClass == Enums.CardClass.Boon)
			{
				if (currentHero.GetTotalCardsInDeck() <= 15)
				{
					flag = false;
				}
			}
			else if (currentHero.GetTotalCardsInDeck(excludeInjuriesAndBoons: true) <= 15)
			{
				flag = false;
			}
			if (!flag2 && (flag || AtOManager.Instance.Sandbox_noMinimumDecksize))
			{
				if (CanBuy("Remove"))
				{
					BG_Remove.Enable();
				}
				else
				{
					BG_Remove.Disable();
				}
			}
			else
			{
				BG_Remove.Disable();
			}
		}
	}

	private int SetPrice(string function, string rarity, string cardName = "", int zoneTier = 0, bool useShopDiscount = true)
	{
		int num = 0;
		if (isPetShop)
		{
			if (cardName != "")
			{
				rarity = Enum.GetName(typeof(Enums.CardRarity), Globals.Instance.GetCardData(cardName, instantiate: false).CardRarity);
			}
			if (rarity.ToLower() == "common")
			{
				num = 72;
			}
			else if (rarity.ToLower() == "uncommon")
			{
				num = 156;
			}
			else if (rarity.ToLower() == "rare")
			{
				num = 348;
			}
			else if (rarity.ToLower() == "epic")
			{
				num = 744;
			}
			else if (rarity.ToLower() == "mythic")
			{
				num = 1200;
			}
		}
		else
		{
			switch (function)
			{
			case "Remove":
				if (zoneTier == 0 && GameManager.Instance.IsSingularity())
				{
					num = 0;
					break;
				}
				num = Globals.Instance.GetRemoveCost(cardData.CardType, cardData.CardRarity);
				if (cardData.CardType == Enums.CardType.Injury)
				{
					if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_3_4"))
					{
						num -= Functions.FuncRoundToInt((float)num * 0.3f);
					}
					else if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_3_2"))
					{
						num -= Functions.FuncRoundToInt((float)num * 0.15f);
					}
					break;
				}
				if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_3_5"))
				{
					num -= Functions.FuncRoundToInt((float)num * 0.5f);
				}
				else if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_3_3"))
				{
					num -= Functions.FuncRoundToInt((float)num * 0.25f);
				}
				if (!AtOManager.Instance.CharInTown())
				{
					break;
				}
				if (AtOManager.Instance.GetTownTier() == 1)
				{
					if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_3_6"))
					{
						num = 0;
					}
				}
				else if (AtOManager.Instance.GetTownTier() == 0 && PlayerManager.Instance.PlayerHaveSupply("townUpgrade_3_1"))
				{
					num = 0;
				}
				break;
			case "Upgrade":
			case "Transform":
				num = Globals.Instance.GetUpgradeCost(function, rarity);
				if (function == "Upgrade")
				{
					if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_2_6"))
					{
						num -= Functions.FuncRoundToInt((float)num * 0.5f);
					}
					else if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_2_4"))
					{
						num -= Functions.FuncRoundToInt((float)num * 0.3f);
					}
					else if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_2_2"))
					{
						num -= Functions.FuncRoundToInt((float)num * 0.15f);
					}
				}
				if (function == "Transform")
				{
					if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_2_5"))
					{
						num = 0;
					}
					else if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_2_3"))
					{
						num -= Functions.FuncRoundToInt((float)num * 0.5f);
					}
					else if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_2_1"))
					{
						num -= Functions.FuncRoundToInt((float)num * 0.25f);
					}
				}
				break;
			case "Craft":
			{
				float discountCraft = 0f;
				if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_1_5"))
				{
					discountCraft = 0.3f;
				}
				else if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_1_2"))
				{
					discountCraft = 0.15f;
				}
				float discountUpgrade = 0f;
				if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_2_6"))
				{
					discountUpgrade = 0.5f;
				}
				else if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_2_4"))
				{
					discountUpgrade = 0.3f;
				}
				else if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_2_2"))
				{
					discountUpgrade = 0.15f;
				}
				num = Globals.Instance.GetCraftCost(cardName, discountCraft, discountUpgrade, zoneTier);
				break;
			}
			case "Item":
				num = Globals.Instance.GetItemCost(cardName);
				if (AtOManager.Instance.CharInTown())
				{
					if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_5_5"))
					{
						num -= Functions.FuncRoundToInt((float)num * 0.3f);
					}
					else if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_5_2"))
					{
						num -= Functions.FuncRoundToInt((float)num * 0.15f);
					}
				}
				break;
			case "Corruption":
				num = 300;
				break;
			}
		}
		if (num > 0)
		{
			int num2 = 0;
			for (int i = 0; i < 4; i++)
			{
				if (AtOManager.Instance.GetHero(i) != null)
				{
					num2 += AtOManager.Instance.GetHero(i).GetItemDiscountModification();
				}
			}
			int num3 = num2;
			if (useShopDiscount)
			{
				num3 += discount;
			}
			if (num3 != 0)
			{
				num -= Functions.FuncRoundToInt(num * num3 / 100);
				if (num < 0)
				{
					num = 0;
				}
			}
		}
		if (function == "Corruption" && IsCorruptionEnabled(0))
		{
			num += 400;
		}
		if (function == "Upgrade" && AtOManager.Instance.Sandbox_cardUpgradePrice != 0)
		{
			num += Functions.FuncRoundToInt((float)num * (float)AtOManager.Instance.Sandbox_cardUpgradePrice * 0.01f);
		}
		else if (function == "Transform" && AtOManager.Instance.Sandbox_cardTransformPrice != 0)
		{
			num += Functions.FuncRoundToInt((float)num * (float)AtOManager.Instance.Sandbox_cardTransformPrice * 0.01f);
		}
		else if (function == "Remove" && AtOManager.Instance.Sandbox_cardRemovePrice != 0)
		{
			num += Functions.FuncRoundToInt((float)num * (float)AtOManager.Instance.Sandbox_cardRemovePrice * 0.01f);
		}
		else if (function == "Item" && AtOManager.Instance.Sandbox_itemsPrice != 0)
		{
			num += Functions.FuncRoundToInt((float)num * (float)AtOManager.Instance.Sandbox_itemsPrice * 0.01f);
		}
		else if (isPetShop && AtOManager.Instance.Sandbox_petsPrice != 0)
		{
			num += Functions.FuncRoundToInt((float)num * (float)AtOManager.Instance.Sandbox_petsPrice * 0.01f);
		}
		return num;
	}

	public void RefreshCardPrices()
	{
		if (craftType == 0 || craftType == 1)
		{
			if (!blocked && cardActiveName != "")
			{
				SelectCard(cardActiveName);
			}
		}
		else if (craftType == 2)
		{
			RefreshCraftButtonsPrices();
			if (!cardCraftSave.gameObject.activeSelf && AtOManager.Instance.affordableCraft)
			{
				ShowListCardsForCraft(currentCraftPageNum);
			}
		}
		else if (craftType == 3)
		{
			RefreshDivinationButtonsPrices();
		}
		else if (craftType == 4)
		{
			RefreshCraftButtonsPrices();
			SetShadyDealLeftUses();
		}
		else if (craftType == 6 || craftType == 7)
		{
			RefreshCraftButtonsPrices();
		}
	}

	public void SelectCard(string cardName)
	{
		if (remainingUses == 0)
		{
			return;
		}
		cardActiveName = cardName;
		RemoveUpgradeCards();
		RemoveRemoveCards();
		RemoveCorruptionCards();
		string[] array = cardName.Split('_');
		string text = array[0];
		string cardIndex = array[1];
		this.cardData = Globals.Instance.GetCardData(text, instantiate: false);
		CreateCard(text, cardIndex, isYours: true);
		int num = 0;
		if (craftType == 0)
		{
			TMP_Text tMP_Text = oldcostAText;
			string text2 = (oldcostBText.text = "");
			tMP_Text.text = text2;
			if (this.cardData.CardUpgraded == Enums.CardUpgraded.No)
			{
				CreateCard(this.cardData.UpgradesTo1, cardIndex, isYours: false);
				CreateCard(this.cardData.UpgradesTo2, cardIndex, isYours: false);
				CardData cardData = Globals.Instance.GetCardData(this.cardData.UpgradesTo1, instantiate: false);
				string rarity = Enum.GetName(typeof(Enums.CardRarity), cardData.CardRarity);
				costA = SetPrice("Upgrade", rarity);
				num = SetPrice("Upgrade", rarity, "", 0, useShopDiscount: false);
				if (num != costA)
				{
					oldcostAText.text = string.Format(Texts.Instance.GetText("oldCost"), num.ToString());
				}
				cardData = Globals.Instance.GetCardData(this.cardData.UpgradesTo2, instantiate: false);
				rarity = Enum.GetName(typeof(Enums.CardRarity), cardData.CardRarity);
				costB = SetPrice("Upgrade", rarity);
				num = SetPrice("Upgrade", rarity, "", 0, useShopDiscount: false);
				if (num != costB)
				{
					oldcostBText.text = string.Format(Texts.Instance.GetText("oldCost"), num.ToString());
				}
				ShowElements("T");
			}
			else if (this.cardData.CardUpgraded == Enums.CardUpgraded.A)
			{
				string text4 = text.Remove(text.Length - 1, 1) + "B";
				CreateCard(this.cardData.UpgradedFrom, cardIndex, isYours: false);
				CreateCard(text4, cardIndex, isYours: false);
				costA = 1000000;
				CardData cardData = Globals.Instance.GetCardData(text4, instantiate: false);
				string rarity = Enum.GetName(typeof(Enums.CardRarity), cardData.CardRarity);
				costB = SetPrice("Transform", rarity);
				num = SetPrice("Transform", rarity, "", 0, useShopDiscount: false);
				if (num != costB)
				{
					oldcostBText.text = string.Format(Texts.Instance.GetText("oldCost"), num.ToString());
				}
				ShowElements("R");
			}
			else if (this.cardData.CardUpgraded == Enums.CardUpgraded.B)
			{
				string text4 = text.Remove(text.Length - 1, 1) + "A";
				CreateCard(this.cardData.UpgradedFrom, cardIndex, isYours: false);
				CreateCard(text4, cardIndex, isYours: false);
				CardData cardData = Globals.Instance.GetCardData(text4, instantiate: false);
				string rarity = Enum.GetName(typeof(Enums.CardRarity), cardData.CardRarity);
				costA = SetPrice("Transform", rarity);
				num = SetPrice("Transform", rarity, "", 0, useShopDiscount: false);
				if (num != costA)
				{
					oldcostAText.text = string.Format(Texts.Instance.GetText("oldCost"), num.ToString());
				}
				costB = 1000000;
				ShowElements("L");
			}
		}
		else if (craftType == 1)
		{
			oldcostRemoveText.text = "";
			costRemove = SetPrice("Remove", "");
			BG_Remove.SetText(ButtonText(costRemove));
			num = SetPrice("Remove", "", "", 0, useShopDiscount: false);
			if (num != costRemove)
			{
				oldcostRemoveText.text = string.Format(Texts.Instance.GetText("oldCost"), num.ToString());
			}
			ShowElements("Remove", text);
		}
		else if ((craftType == 6 || craftType == 7) && (craftType != 7 || this.cardData.CardUpgraded != Enums.CardUpgraded.Rare))
		{
			oldcostRemoveText.text = "";
			costCorruption = SetPrice("Corruption", "");
			BG_Corruption.SetText(ButtonText(costCorruption));
			num = SetPrice("Corruption", "", "", 0, useShopDiscount: false);
			if (num != costCorruption)
			{
				oldcostRemoveText.text = string.Format(Texts.Instance.GetText("oldCost"), num.ToString());
			}
			ActivateButtonCorruption();
		}
	}

	public void SetActiveCard(CardVertical cv)
	{
		cardActive = cv;
	}

	public void ClearActiveCard()
	{
		if (cardActive != null)
		{
			cardActive.ClearActive();
		}
		cardActive = null;
	}

	private void RemoveUpgradeCards()
	{
		foreach (Transform item in cardUpgradeContainer)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		CIPlain = null;
		CIA = null;
		CIB = null;
	}

	private void RemoveRemoveCards()
	{
		foreach (Transform item in cardRemoveContainer)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		CIRemove = null;
	}

	private void RemoveCraftCards()
	{
		foreach (Transform item in cardCraftContainer)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		CIRemove = null;
	}

	private void RemoveCorruptionCards()
	{
		foreach (Transform item in cardCorruptionContainer)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		CICorruption = null;
		corruptionPercentRoll.text = "";
		corruptionPercentRollSuccess.gameObject.SetActive(value: false);
		corruptionPercentRollFail.gameObject.SetActive(value: false);
	}

	public void CreateCard(string cardId, string cardIndex, bool isYours)
	{
		Transform parent = null;
		if (craftType == 0)
		{
			parent = cardUpgradeContainer;
		}
		else if (craftType == 1)
		{
			parent = cardRemoveContainer;
		}
		else if (craftType == 6 || craftType == 7)
		{
			parent = cardCorruptionContainer;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, parent);
		CardItem component = gameObject.GetComponent<CardItem>();
		if (craftType == 6 || craftType == 7)
		{
			CardData cardDataFromCardData = Functions.GetCardDataFromCardData(Globals.Instance.GetCardData(cardId, instantiate: false), "RARE");
			if (!(cardDataFromCardData != null))
			{
				RemoveCorruptionCards();
				DeactivateCorruptions();
				return;
			}
			component.SetCard(cardDataFromCardData.Id, deckScale: false, currentHero);
		}
		else
		{
			component.SetCard(cardId, deckScale: false, currentHero);
		}
		component.TopLayeringOrder("UI", -500);
		component.DrawEnergyCost();
		CardData cardData = component.CardData;
		if (craftType == 0)
		{
			component.SetLocalScale(new Vector3(1.2f, 1.2f, 1f));
			if (cardData.CardUpgraded == Enums.CardUpgraded.No)
			{
				gameObject.name = "TMP_Plain_" + cardIndex;
				component.SetLocalPosition(posPlain);
				CIPlain = component;
			}
			else if (cardData.CardUpgraded == Enums.CardUpgraded.A)
			{
				gameObject.name = "TMP_A_" + cardIndex;
				component.SetLocalPosition(posUpgradeA);
				component.ShowDifferences(Globals.Instance.GetCardData(cardData.UpgradedFrom, instantiate: false));
				CIA = component;
			}
			else if (cardData.CardUpgraded == Enums.CardUpgraded.B)
			{
				gameObject.name = "TMP_B_" + cardIndex;
				component.SetLocalPosition(posUpgradeB);
				component.ShowDifferences(Globals.Instance.GetCardData(cardData.UpgradedFrom, instantiate: false));
				CIB = component;
			}
			transformAnim.SetTrigger("fade");
		}
		else if (craftType == 1)
		{
			gameObject.name = "TMP_Plain_" + cardIndex;
			CIRemove = component;
			component.SetLocalPosition(posRemove);
			removeAnim.SetTrigger("fade");
		}
		else if (craftType == 6 || craftType == 7)
		{
			gameObject.name = "TMP_Plain_" + cardIndex;
			CICorruption = component;
			component.SetLocalPosition(posRemove);
			corruptAnim.SetTrigger("fade");
		}
		component.cardforaddcard = true;
		component.cardoutsidereward = true;
		component.CreateColliderAdjusted();
		component.DisableTrail();
	}

	private void ShowCharacterItems()
	{
		if (currentHero == null || currentHero.HeroData == null)
		{
			return;
		}
		iconWeapon.ShowIconExternal("weapon", currentHero);
		iconArmor.ShowIconExternal("armor", currentHero);
		iconJewelry.ShowIconExternal("jewelry", currentHero);
		iconAccesory.ShowIconExternal("accesory", currentHero);
		iconPet.ShowIconExternal("pet", currentHero);
		itemsOwner.text = string.Format(Texts.Instance.GetText("heroEquipment"), currentHero.SourceName);
		if (craftType == 7)
		{
			itemCorruptionTextTitle.text = string.Format(Texts.Instance.GetText("heroEquipment"), currentHero.SourceName);
		}
		for (int i = 0; i < 4; i++)
		{
			Hero hero = AtOManager.Instance.GetHero(i);
			if (hero.Id == currentHero.Id)
			{
				iconDeck.auxInt = i;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(Texts.Instance.GetText("deck"));
				stringBuilder.Append("\n<color=#bbb><size=-.5>");
				stringBuilder.Append(string.Format(Texts.Instance.GetText("cardsNum"), hero.Cards.Count));
				iconDeckText.text = stringBuilder.ToString();
				break;
			}
		}
	}

	private void ShowCharacterCorruptionItems()
	{
		if (currentHero == null || currentHero.HeroData == null)
		{
			return;
		}
		RemoveCorruptionCards();
		AssignCorruptionItemSlots();
		itemsOwner.text = string.Format(Texts.Instance.GetText("heroEquipment"), currentHero.SourceName);
		if (craftType == 7)
		{
			itemCorruptionTextTitle.text = string.Format(Texts.Instance.GetText("heroEquipment"), currentHero.SourceName);
		}
		for (int i = 0; i < 4; i++)
		{
			Hero hero = AtOManager.Instance.GetHero(i);
			if (hero.Id == currentHero.Id)
			{
				iconDeck.auxInt = i;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(Texts.Instance.GetText("deck"));
				stringBuilder.Append("\n<color=#bbb><size=-.5>");
				stringBuilder.Append(string.Format(Texts.Instance.GetText("cardsNum"), hero.Cards.Count));
				iconDeckText.text = stringBuilder.ToString();
				break;
			}
		}
	}

	private void AssignCorruptionItemSlots()
	{
		string[] array = new string[4] { currentHero.Weapon, currentHero.Armor, currentHero.Jewelry, currentHero.Accesory };
		for (int i = 0; i < ItemCorruptionIcons.Length; i++)
		{
			AssignSpriteToCorruptionIcons(ItemCorruptionIcons[i], array[i]);
			AssignInternalIdToCorruptionIcons(ItemCorruptionIcons[i], array[i], i);
			if (array[i].ToLower().Contains("rare"))
			{
				ItemCorruptionIcons[i].AllowInput(state: true);
				ItemCorruptionIcons[i].ShowRareParticles();
			}
			else
			{
				bool state = !array[i].IsNullOrEmpty();
				ItemCorruptionIcons[i].AllowInput(state);
				ItemCorruptionIcons[i].ShowRareParticles(state: false);
			}
		}
	}

	private void AssignSpriteToCorruptionIcons(ItemCorruptionIcon iconItem, string type)
	{
		Sprite sprite = null;
		if ((bool)iconItem && !type.IsNullOrEmpty())
		{
			sprite = Globals.Instance.GetCardData(type).Sprite;
		}
		iconItem.SetSprite(sprite);
	}

	private void AssignInternalIdToCorruptionIcons(ItemCorruptionIcon iconItem, string itemId, int index)
	{
		CardData cardData = Globals.Instance.GetCardData(itemId);
		if ((bool)iconItem && (bool)cardData && !itemId.IsNullOrEmpty())
		{
			iconItem.SetInternalId(itemId + "_" + index);
		}
	}

	public void HoverItem(bool state, Enums.CardType cardType)
	{
		switch (cardType)
		{
		case Enums.CardType.Weapon:
			if (iconWeapon != null)
			{
				iconWeapon.DoHover(state);
			}
			break;
		case Enums.CardType.Armor:
			if (iconArmor != null)
			{
				iconArmor.DoHover(state);
			}
			break;
		case Enums.CardType.Jewelry:
			if (iconJewelry != null)
			{
				iconJewelry.DoHover(state);
			}
			break;
		case Enums.CardType.Accesory:
			if (iconAccesory != null)
			{
				iconAccesory.DoHover(state);
			}
			break;
		case Enums.CardType.Pet:
			if (iconPet != null)
			{
				iconPet.DoHover(state);
			}
			break;
		}
	}

	private void ClearItemListContainer()
	{
		foreach (Transform item in cardItemContainer)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
	}

	public void SetItemList(string _itemListId)
	{
		if (_itemListId != "")
		{
			itemListId = _itemListId.ToLower();
		}
		else if (isPetShop)
		{
			itemListId = "petShop";
		}
		else
		{
			itemListId = AtOManager.Instance.GetTownItemListId();
		}
	}

	public void DoPetShop()
	{
		craftCardItemDict = new Dictionary<int, CardCraftItem>();
		ClearItemListContainer();
		isPetShop = true;
		SetItemList("");
		ShowItemsForBuy();
	}

	public void DoItemShop()
	{
		craftCardItemDict = new Dictionary<int, CardCraftItem>();
		ClearItemListContainer();
		isPetShop = false;
		SetItemList("");
		ShowItemsForBuy();
	}

	public void RerollItems()
	{
		if (CanBuy("Reroll"))
		{
			AtOManager.Instance.PayGold(Globals.Instance.GetCostReroll(), paySingle: true, isReroll: true);
			AtOManager.Instance.DoTownReroll();
			DoItemShop();
		}
	}

	public void ShowItemsForBuy(int pageNum = 1, string itemBought = "")
	{
		SetBlocked(_status: false);
		List<string> itemList = new List<string>();
		bool flag = true;
		bool flag2 = false;
		maxPetNumber.gameObject.SetActive(value: false);
		if ((bool)TownManager.Instance)
		{
			if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_6_2") || GameManager.Instance.IsSingularity() || AtOManager.Instance.IsCombatTool)
			{
				petShopButton.gameObject.SetActive(value: true);
				itemShopButton.gameObject.SetActive(value: true);
			}
			else
			{
				petShopButton.gameObject.SetActive(value: false);
				itemShopButton.gameObject.SetActive(value: false);
			}
		}
		else
		{
			petShopButton.gameObject.SetActive(value: false);
			itemShopButton.gameObject.SetActive(value: false);
		}
		rerollButton.gameObject.SetActive(value: false);
		rerollButtonLock.gameObject.SetActive(value: false);
		rerollButtonWarning.gameObject.SetActive(value: false);
		if (!isPetShop)
		{
			if (AtOManager.Instance.CharInTown() && PlayerManager.Instance.PlayerHaveSupply("townUpgrade_5_1"))
			{
				if (AtOManager.Instance.IsCombatTool)
				{
					rerollButton.gameObject.SetActive(value: false);
				}
				else
				{
					rerollButton.gameObject.SetActive(value: true);
				}
				int costReroll = Globals.Instance.GetCostReroll();
				StringBuilder stringBuilder = new StringBuilder();
				if (Globals.Instance.CurrentLang != "de" && Globals.Instance.CurrentLang != "ko")
				{
					stringBuilder.Append("<br>");
				}
				stringBuilder.Append("<size=-.3><sprite name=gold> ");
				stringBuilder.Append(costReroll);
				if (Globals.Instance.CurrentLang == "de" || Globals.Instance.CurrentLang == "ko")
				{
					stringBuilder.Append("<br>");
				}
				rerollButton.GetComponent<BotonGeneric>().SetText(string.Format(Texts.Instance.GetText("rerollButton"), stringBuilder.ToString()));
				if (!GameManager.Instance.IsMultiplayer())
				{
					rerollButton.GetComponent<BotonGeneric>().SetPopupId("rerollButtonCostSP");
				}
				else
				{
					rerollButton.GetComponent<BotonGeneric>().SetPopupId("rerollButtonCostCoop");
				}
				if (AtOManager.Instance.IsTownRerollLimited())
				{
					rerollButtonWarning.gameObject.SetActive(value: true);
				}
				if (AtOManager.Instance.GetPlayerGold() < costReroll || !AtOManager.Instance.IsTownRerollAvailable())
				{
					rerollButton.GetComponent<BotonGeneric>().Disable();
					if (!AtOManager.Instance.IsTownRerollAvailable())
					{
						rerollButtonLock.gameObject.SetActive(value: true);
						rerollButtonWarning.gameObject.SetActive(value: false);
					}
				}
				else
				{
					rerollButton.GetComponent<BotonGeneric>().Enable();
				}
			}
			if (itemShopButton.gameObject.activeSelf)
			{
				itemShopButton.GetComponent<BotonGeneric>().Disable();
				itemShopButton.GetComponent<BotonGeneric>().ShowBackground(state: true);
				petShopButton.GetComponent<BotonGeneric>().Enable();
				petShopButton.GetComponent<BotonGeneric>().ShowBackground(state: false);
			}
			if (AtOManager.Instance.IsCombatTool)
			{
				LoadAllItemsEquipment();
			}
			else
			{
				itemList = AtOManager.Instance.GetItemList(itemListId);
			}
			LootData lootData = Globals.Instance.GetLootData(itemListId);
			if (!AtOManager.Instance.CharInTown() && lootData.ShadyModel != null)
			{
				flag2 = true;
				if (!shadyDeal.gameObject.activeSelf)
				{
					shadyDeal.gameObject.SetActive(value: true);
				}
				if (shadyDealModel != null)
				{
					UnityEngine.Object.Destroy(shadyDealModel);
				}
				shadyDealModel = UnityEngine.Object.Instantiate(lootData.ShadyModel, new Vector3(0f, 0f, 0f), Quaternion.identity, shadyDealModelContainer);
				shadyDealModel.GetComponent<BoxCollider2D>().enabled = false;
				shadyDealModel.GetComponent<CharacterGOItem>().enabled = false;
				shadyDealModel.transform.localScale = new Vector3(lootData.ShadyScaleX, lootData.ShadyScaleY, 1f);
				shadyDealModel.transform.localPosition = new Vector3(lootData.ShadyOffsetX, lootData.ShadyOffsetY, 0f);
				int childCount = shadyDealModel.transform.childCount;
				for (int i = 0; i < childCount; i++)
				{
					SpriteRenderer component = shadyDealModel.transform.GetChild(i).GetComponent<SpriteRenderer>();
					if (component != null)
					{
						component.sortingOrder = 200 - i;
						component.sortingLayerName = "UI";
					}
				}
			}
			else if (shadyDeal.gameObject.activeSelf)
			{
				shadyDeal.gameObject.SetActive(value: false);
			}
		}
		else
		{
			if (petShopButton.gameObject.activeSelf)
			{
				petShopButton.GetComponent<BotonGeneric>().Disable();
				petShopButton.GetComponent<BotonGeneric>().ShowBackground(state: true);
				itemShopButton.GetComponent<BotonGeneric>().Enable();
				itemShopButton.GetComponent<BotonGeneric>().ShowBackground(state: false);
			}
			if (AtOManager.Instance.IsCombatTool)
			{
				LoadAllItemsPets();
			}
			else
			{
				itemList = Globals.Instance.CardItemByType[Enums.CardType.Pet];
			}
		}
		if (itemList == null)
		{
			pageNum = currentItemsPageNum;
			return;
		}
		if (AtOManager.Instance.IsCombatTool && searchTerm != "")
		{
			if (isPetShop)
			{
				itemList = (from item in LoadAllItemsPets()
					where Globals.Instance.IsInSearch(searchTerm, item)
					select item).ToList();
			}
			else
			{
				itemList = (from item in LoadAllItemsEquipment()
					where Globals.Instance.IsInSearch(searchTerm, item)
					select item).ToList();
			}
		}
		else
		{
			itemList = itemList.Where((string item) => Globals.Instance.IsInSearch(searchTerm, item)).ToList();
		}
		itemList.Sort();
		int num = 0;
		float num2 = 4f;
		float num3 = num2 * 2f;
		int num4 = Mathf.CeilToInt((float)itemList.Count / num3);
		if (pageNum < 1 || pageNum > num4)
		{
			pageNum = currentItemsPageNum;
			if (!GameManager.Instance.GetDeveloperMode())
			{
				return;
			}
		}
		currentItemsPageNum = pageNum;
		ClearCraftPages();
		int num5 = 0;
		for (int num6 = 0; num6 < itemList.Count; num6++)
		{
			if ((float)num5 >= (float)(pageNum - 1) * num3 && (float)num5 < (float)pageNum * num3)
			{
				CardCraftItem cardCraftItem;
				CardData cardData;
				if (!craftCardItemDict.ContainsKey(num))
				{
					GameObject obj = UnityEngine.Object.Instantiate(this.cardCraftItem, new Vector3(0f, 0f, -3f), Quaternion.identity, cardItemContainer);
					cardCraftItem = obj.transform.GetComponent<CardCraftItem>();
					obj.name = itemList[num6];
					craftCardItemDict.Add(num, cardCraftItem);
					int num7 = Mathf.FloorToInt((float)num / num2);
					float x = cardItemContainer.transform.localPosition.x - 1.2f + (float)num % num2 * 2.5f;
					float y = cardItemContainer.transform.localPosition.y + 3.05f - 4.2f * (float)num7;
					cardCraftItem.SetPosition(new Vector3(x, y, 0f));
					cardCraftItem.SetIndex(num);
					cardCraftItem.SetHero(currentHero);
					cardCraftItem.SetGenericCard(item: true);
					cardData = Globals.Instance.GetCardData(itemList[num6], instantiate: false);
					string rarity = Enum.GetName(typeof(Enums.CardRarity), cardData.CardRarity);
					int cost = SetPrice("Item", rarity, itemList[num6], craftTierZone);
					cardCraftItem.SetButtonTextItem(ButtonText(cost));
					cardCraftItem.SetCard(itemList[num6], itemListId, currentHero);
				}
				else
				{
					cardCraftItem = craftCardItemDict[num];
					cardCraftItem.gameObject.name = itemList[num6];
					cardCraftItem.gameObject.SetActive(value: true);
					cardData = Globals.Instance.GetCardData(itemList[num6], instantiate: false);
					string rarity2 = Enum.GetName(typeof(Enums.CardRarity), cardData.CardRarity);
					int cost2 = SetPrice("Item", rarity2, itemList[num6], craftTierZone);
					cardCraftItem.SetButtonTextItem(ButtonText(cost2));
					cardCraftItem.SetCard(itemList[num6], itemListId, currentHero);
				}
				bool flag3 = false;
				if (cardCraftItem != null)
				{
					string key = itemListId + cardCraftItem.cardId;
					if (AtOManager.Instance.boughtItemInShopByWho != null && AtOManager.Instance.boughtItemInShopByWho.ContainsKey(key))
					{
						if (!AtOManager.Instance.IsCombatTool)
						{
							ShowPortraitItemBought(AtOManager.Instance.boughtItemInShopByWho[key], "", cardCraftItem);
							cardCraftItem.EnableButton(_state: false);
							cardCraftItem.ShowDisable(_state: true);
							cardCraftItem.ShowSold(_state: true);
						}
						flag3 = true;
					}
					else
					{
						Transform transform = cardCraftItem.transform.GetChild(1).transform.GetChild(0).transform.Find("itemBuyer");
						if (transform != null)
						{
							UnityEngine.Object.Destroy(transform.gameObject);
						}
					}
				}
				if (!isPetShop)
				{
					if (!PlayerManager.Instance.IsCardUnlocked(itemList[num6]))
					{
						PlayerManager.Instance.CardUnlock(itemList[num6]);
						if (cardCraftItem != null)
						{
							cardCraftItem.GetCI().ShowUnlocked(showEffects: false);
						}
					}
				}
				else if (cardCraftItem != null)
				{
					if (!flag3)
					{
						cardCraftItem.ShowSold(_state: false);
					}
					if (!flag || !PlayerManager.Instance.IsCardUnlocked(itemList[num6]))
					{
						cardCraftItem.ShowDisable(_state: true);
						cardCraftItem.EnableButton(_state: false);
					}
					else if (!flag3)
					{
						cardCraftItem.ShowSold(_state: false);
						if (Instance.CanBuy("Item", cardCraftItem.cardId))
						{
							cardCraftItem.EnableButton(_state: true);
							cardCraftItem.ShowDisable(_state: false);
						}
						else
						{
							cardCraftItem.EnableButton(_state: false);
							cardCraftItem.ShowDisable(_state: true);
						}
					}
				}
				if (cardCraftItem != null && cardCraftItem.enabled)
				{
					if (cardData != null && cardData.CardType == Enums.CardType.Pet && cardData.Sku != "" && !SteamManager.Instance.PlayerHaveDLC(cardData.Sku))
					{
						cardCraftItem.EnableButton(_state: false);
						cardCraftItem.ShowDisable(_state: true);
						string text = string.Format(Texts.Instance.GetText("requiredDLC"), SteamManager.Instance.GetDLCName(cardData.Sku));
						cardCraftItem.ShowLock(_state: true, text);
					}
					else if (cardData != null && AtOManager.Instance.IsCombatTool && cardData.Sku != "" && !SteamManager.Instance.PlayerHaveDLC(cardData.Sku))
					{
						cardCraftItem.EnableButton(_state: false);
						cardCraftItem.ShowDisable(_state: true);
						string text2 = string.Format(Texts.Instance.GetText("requiredDLC"), SteamManager.Instance.GetDLCName(cardData.Sku));
						cardCraftItem.ShowLock(_state: true, text2);
					}
					else
					{
						cardCraftItem.ShowLock(_state: false);
						if (AtOManager.Instance.IsCombatTool)
						{
							cardCraftItem.EnableButton(_state: true);
							cardCraftItem.ShowDisable(_state: false);
						}
					}
				}
				num++;
			}
			num5++;
		}
		if (num4 > 1 || AtOManager.Instance.IsCombatTool)
		{
			if ((float)num < num2 * 2f)
			{
				for (int num8 = num; (float)num8 < num2 * 2f; num8++)
				{
					if (craftCardItemDict.ContainsKey(num8))
					{
						craftCardItemDict[num8].transform.gameObject.SetActive(value: false);
					}
				}
			}
			CreateCraftPages(pageNum, num4);
		}
		if (flag2 && !shadyDealInitiated)
		{
			GetShadyDealRemaining();
			SetShadyDealLeftUses();
			GetShadyDealDustGold(itemList);
			SetShadyDealDustGoldValues();
			shadyDealInitiated = true;
		}
		void CheckItemsToRemove()
		{
			string[] removeItems = new string[5] { "BrokenItem", "Tombstone", "BurnedItem", "UselessItem", "DeformedItem" };
			itemList.RemoveAll((string item) => removeItems.Contains(item));
		}
		List<string> LoadAllItemsEquipment()
		{
			itemList.Clear();
			CardData[] collection = Resources.LoadAll<CardData>("Cards/Items");
			CardData[] array = Resources.LoadAll<CardData>("Cards/Items/Corruption");
			List<CardData> list = new List<CardData>(collection);
			CardData[] array2 = array;
			foreach (CardData item in array2)
			{
				list.Remove(item);
			}
			itemList.Clear();
			foreach (CardData item2 in list)
			{
				if (item2.CardType != Enums.CardType.Pet && item2.CardType != Enums.CardType.Food && item2.ShowInTome && (!(item2.Item == null) || !(item2.ItemEnchantment == null)))
				{
					itemList.Add(item2.name);
				}
			}
			CheckItemsToRemove();
			itemList.Sort();
			return itemList;
		}
		List<string> LoadAllItemsPets()
		{
			itemList.Clear();
			CardData[] array = Resources.LoadAll<CardData>("Cards/Items/Pets");
			foreach (CardData cardData2 in array)
			{
				if (cardData2.CardType != Enums.CardType.Enchantment)
				{
					itemList.Add(cardData2.name);
				}
			}
			CheckItemsToRemove();
			itemList.Sort();
			return itemList;
		}
	}

	public void ShowPortraitItemBought(int _heroIndex, string _itemId = "", CardCraftItem _CCI = null)
	{
		CardCraftItem cardCraftItem = _CCI;
		if (cardCraftItem == null)
		{
			cardCraftItem = craftCardItemDict[0];
		}
		if (cardCraftItem != null)
		{
			Transform transform = cardCraftItem.transform.GetChild(1).transform.GetChild(0).transform;
			Transform transform2 = transform.Find("itemBuyer");
			if (transform2 != null)
			{
				transform2.transform.GetComponent<SpriteRenderer>().sprite = AtOManager.Instance.GetHero(_heroIndex).HeroData.HeroSubClass.SpriteSpeed;
				return;
			}
			SpriteRenderer component = transform.Find("Disable").GetComponent<SpriteRenderer>();
			GameObject obj = UnityEngine.Object.Instantiate(portraitItem, Vector3.zero, Quaternion.identity, transform);
			obj.name = "itemBuyer";
			obj.transform.gameObject.SetActive(value: true);
			SpriteRenderer component2 = obj.transform.GetComponent<SpriteRenderer>();
			component2.sprite = AtOManager.Instance.GetHero(_heroIndex).HeroData.HeroSubClass.SpriteSpeed;
			component2.sortingOrder = component.sortingOrder + 1;
			component2.sortingLayerName = component.sortingLayerName;
			obj.transform.localPosition = new Vector3(0.65f, 0.75f, 0f);
		}
	}

	public bool HavePortraitItemBought(CardCraftItem _CCI)
	{
		Transform transform = _CCI.transform.GetChild(1).transform.GetChild(0).transform.Find("itemBuyer");
		if (transform != null && transform.gameObject.activeSelf)
		{
			return true;
		}
		return false;
	}

	private void RefreshCraftButtonsPrices()
	{
		for (int i = 0; i < craftCardItemDict.Count; i++)
		{
			CardCraftItem cardCraftItem = craftCardItemDict[i];
			if (craftType == 2)
			{
				cardCraftItem.SetAvailability();
			}
			else if (craftType < 3)
			{
				cardCraftItem.EnableButton(CanBuy("Craft", cardCraftItem.cardId));
			}
			else if (craftType == 4 && !cardCraftItem.lockIcon.gameObject.activeSelf)
			{
				cardCraftItem.SetAvailability();
			}
		}
		if (craftType == 6)
		{
			ActivateButtonCorruption();
		}
		if (rerollButton.gameObject.activeSelf)
		{
			if (AtOManager.Instance.GetPlayerGold() < Globals.Instance.GetCostReroll())
			{
				rerollButton.GetComponent<BotonGeneric>().Disable();
			}
			else
			{
				rerollButton.GetComponent<BotonGeneric>().Enable();
			}
		}
	}

	public void ClearCraftPages()
	{
		Transform transform = cardCraftPageContainer;
		if (craftType == 4)
		{
			transform = itemsCraftPageContainer;
		}
		foreach (Transform item in transform)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
	}

	public void ClearCardListContainer()
	{
		foreach (Transform item in cardListContainer)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
	}

	public void ClearTmpContainer()
	{
		foreach (Transform item in tmpContainer)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
	}

	private void SetDivinationButtons()
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		int num = 0;
		int playerGold = AtOManager.Instance.GetPlayerGold();
		for (int i = 0; i < 5; i++)
		{
			if (AtOManager.Instance.GetTownTier() == 0)
			{
				if (i > 2 || (i == 1 && !PlayerManager.Instance.PlayerHaveSupply("townUpgrade_4_2")) || (i == 2 && !PlayerManager.Instance.PlayerHaveSupply("townUpgrade_4_6")))
				{
					continue;
				}
			}
			else if (AtOManager.Instance.GetTownTier() == 1)
			{
				if (i > 2 || (i == 2 && !PlayerManager.Instance.PlayerHaveSupply("townUpgrade_4_4")))
				{
					continue;
				}
			}
			else if (AtOManager.Instance.GetTownTier() == 2)
			{
				if (i == 0 || i == 4)
				{
					continue;
				}
			}
			else if (AtOManager.Instance.GetTownTier() >= 2 && (i == 0 || i == 1))
			{
				continue;
			}
			stringBuilder2.Clear();
			stringBuilder2.Append(i);
			int divinationCost = Globals.Instance.GetDivinationCost(stringBuilder2.ToString());
			if (divinationCost > -1)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(cardDivinationButton, new Vector3(-2f + 4f * (float)num, 0f, 0f), Quaternion.identity, divinationButtonContainer);
				gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 0f, -1f);
				gameObject.name = "CraftDivination";
				BotonGeneric component = gameObject.transform.GetComponent<BotonGeneric>();
				component.auxString = divinationCost.ToString();
				component.auxInt = i;
				switch (i)
				{
				case 0:
					component.color = new Color(0.6f, 0.6f, 0.6f, 1f);
					break;
				case 1:
					component.color = Globals.Instance.RarityColor["uncommon"];
					break;
				case 2:
					component.color = Globals.Instance.RarityColor["rare"];
					break;
				case 3:
					component.color = Globals.Instance.RarityColor["epic"];
					break;
				case 4:
					component.color = Globals.Instance.RarityColor["mythic"];
					break;
				}
				component.SetColor();
				stringBuilder.Clear();
				stringBuilder.Append("<size=4.5><sprite name=gold></size><size=5.5><color=#FFAA00> ");
				if (divinationCost > 0)
				{
					stringBuilder.Append(divinationCost.ToString());
				}
				else
				{
					stringBuilder.Append(Texts.Instance.GetText("freeCost"));
				}
				stringBuilder.Append("</color></size><br>");
				stringBuilder.Append(Texts.Instance.GetText("divinationT" + i));
				component.SetText(stringBuilder.ToString());
				if (playerGold < divinationCost)
				{
					component.Disable();
				}
				num++;
			}
		}
		divinationButtonContainer.localPosition = new Vector3(-0.5f - (float)((num - 2) * 2), -2.6f, divinationButtonContainer.localPosition.z);
	}

	private void RefreshDivinationButtonsPrices()
	{
		int playerGold = AtOManager.Instance.GetPlayerGold();
		foreach (Transform item in divinationButtonContainer)
		{
			BotonGeneric component = item.transform.GetComponent<BotonGeneric>();
			if (playerGold < int.Parse(component.auxString))
			{
				component.Disable();
			}
			else
			{
				component.Enable();
			}
		}
	}

	public void BuyDivination(int divinationType)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(divinationType);
		int divinationCost = Globals.Instance.GetDivinationCost(stringBuilder.ToString());
		int playerGold = AtOManager.Instance.GetPlayerGold();
		int divinationTier = Globals.Instance.GetDivinationTier(divinationType);
		if (playerGold >= divinationCost)
		{
			if (!GameManager.Instance.IsMultiplayer())
			{
				AtOManager.Instance.PayGold(divinationCost);
				AtOManager.Instance.SetTownDivinationTier(divinationTier);
				AtOManager.Instance.LaunchRewards();
			}
			else if (AtOManager.Instance.GetTownDivinationTier() != null)
			{
				AlertManager.buttonClickDelegate = SetDivinationWaitState;
				AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("divinationRoundExists"));
			}
			else
			{
				AtOManager.Instance.SetTownDivinationTier(divinationTier, NetworkManager.Instance.GetPlayerNick(), divinationCost);
				SetDivinationWaiting();
			}
		}
		stringBuilder = null;
	}

	private void SetDivinationWaitState()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(SetDivinationWaitState));
		SetDivinationWaiting();
	}

	private void SetDivinationWaiting()
	{
		divinationButtonContainer.gameObject.SetActive(value: false);
		divinationWaitingContainer.gameObject.SetActive(value: true);
		RefreshDivinationWaiting();
	}

	public bool IsWaitingDivination()
	{
		return divinationWaitingContainer.gameObject.activeSelf;
	}

	public void RefreshDivinationWaiting()
	{
		if (!divinationWaitingContainer.gameObject.activeSelf)
		{
			return;
		}
		int num = 0;
		foreach (KeyValuePair<string, bool> item in NetworkManager.Instance.PlayerDivinationReady)
		{
			if (item.Value)
			{
				num++;
			}
		}
		if (num == NetworkManager.Instance.PlayerDivinationReady.Count)
		{
			divinationWaitingMsg.text = Texts.Instance.GetText("divinationRoundLaunch");
		}
		else
		{
			divinationWaitingMsg.text = NetworkManager.Instance.GetWaitingPlayersString(num, NetworkManager.Instance.PlayerDivinationReady.Count);
		}
	}

	public void CraftSelectorEnergy(CardCraftSelectorEnergy CCSE, string energy)
	{
		int num = ((!(energy == "4+")) ? int.Parse(energy) : 4);
		if (selectorEnergy != null)
		{
			selectorEnergy.SetEnable(state: false);
		}
		if (!currentCraftAllCosts && currentCraftCost == num)
		{
			currentCraftAllCosts = true;
			currentCraftCost = 0;
		}
		else
		{
			currentCraftAllCosts = false;
			currentCraftCost = num;
			selectorEnergy = CCSE;
			CCSE.SetEnable(state: true);
		}
		maxCraftPageNum = 1;
		ChangePage(1);
	}

	public void CraftSelectorRarity(CardCraftSelectorRarity CCSR, Enums.CardRarity rarity)
	{
		currentCraftRarity[rarity] = !currentCraftRarity[rarity];
		if (!currentCraftRarity[rarity])
		{
			CCSR.SetEnable(state: false);
		}
		currentCraftAllRarities = true;
		foreach (KeyValuePair<Enums.CardRarity, bool> item in currentCraftRarity)
		{
			if (item.Value)
			{
				currentCraftAllRarities = false;
				break;
			}
		}
		maxCraftPageNum = 1;
		ChangePage(1);
	}

	private void CreateCraftPages(int page, int total)
	{
		ClearCraftPages();
		maxCraftPageNum = total;
		int num = 6;
		int num2 = 1;
		int num3 = 0;
		if (total <= 1)
		{
			return;
		}
		for (int i = 0; i < total; i++)
		{
			bool flag = false;
			Transform parent = cardCraftPageContainer;
			if (craftType == 4)
			{
				parent = itemsCraftPageContainer;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(cardCraftPageButton, Vector3.zero, Quaternion.identity, parent);
			gameObject.transform.localPosition = new Vector3(0.625f * (float)num3, 0f, 0f);
			gameObject.name = "CraftPage";
			BotonGeneric component = gameObject.transform.GetComponent<BotonGeneric>();
			component.transform.localScale = new Vector3(1f, 1f, 1f);
			if (total < num)
			{
				component.SetText((i + 1).ToString() ?? "");
				component.auxInt = i + 1;
				if (page - 1 == i)
				{
					component.ShowDisableMask(state: false);
					component.Disable();
					component.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
				}
				flag = true;
			}
			else if (i == num2 && i != page && page > num2 + num2 + 1)
			{
				component.SetText("...");
				component.Disable();
				component.ShowBackgroundDisable(state: false);
				component.ShowBackground(state: false);
				flag = true;
			}
			else if (i == total - num2 - 1 && i != page && page < total - num2 - num2)
			{
				component.SetText("...");
				component.Disable();
				component.ShowBackgroundDisable(state: false);
				component.ShowBackground(state: false);
				flag = true;
			}
			else if ((i <= num2 || i >= page - num2 - 1 || i >= total - Functions.FuncRoundToInt((float)num * 0.5f) - num2) && (i <= page + num2 - 1 || i >= total - num2 || i <= Functions.FuncRoundToInt((float)num * 0.5f) + 1))
			{
				component.SetText((i + 1).ToString() ?? "");
				component.auxInt = i + 1;
				if (page - 1 == i)
				{
					component.ShowDisableMask(state: false);
					component.Disable();
					component.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
				}
				flag = true;
			}
			if (flag)
			{
				num3++;
			}
			else
			{
				gameObject.SetActive(value: false);
			}
		}
	}

	public void ChangePage(int page)
	{
		if (craftType == 2)
		{
			ShowListCardsForCraft(page);
		}
		else if (craftType == 4)
		{
			ShowItemsForBuy(page);
		}
	}

	public void CraftCard(string cardId)
	{
		SetPrice("Craft", "", cardId, craftTierZone);
	}

	public void CreateDeck(int _heroIndex, bool fast = false)
	{
		heroIndex = _heroIndex;
		currentHero = AtOManager.Instance.GetHero(_heroIndex);
		if (currentHero == null || currentHero.HeroData == null)
		{
			return;
		}
		int count = currentHero.Cards.Count;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Texts.Instance.GetText("heroCards").Replace("<hero>", currentHero.SourceName));
		stringBuilder.Append("  <size=-.2><color=#CCC>(");
		stringBuilder.Append(count);
		if (craftType == 1 && !AtOManager.Instance.Sandbox_noMinimumDecksize)
		{
			stringBuilder.Append(")*</color>");
		}
		else
		{
			stringBuilder.Append(")</color>");
		}
		cardsOwner.text = stringBuilder.ToString();
		stringBuilder = null;
		if (craftType == 4)
		{
			RedrawGridLayout();
			return;
		}
		ShowRemainingUses();
		ShowElements("");
		cardActiveName = "";
		ClearCardListContainer();
		RemoveUpgradeCards();
		RemoveRemoveCards();
		SetBlocked(_status: false);
		deckBgBorder.color = Functions.HexToColor(Globals.Instance.ClassColor[Enum.GetName(typeof(Enums.HeroClass), currentHero.HeroData.HeroClass)]);
		deckGOs = new GameObject[count];
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		SortedList sortedList = new SortedList();
		for (int i = 0; i < count; i++)
		{
			sortedList.Add(Globals.Instance.GetCardData(currentHero.Cards[i], instantiate: false).CardName + currentHero.Cards[i] + i, currentHero.Cards[i] + "_" + i);
		}
		for (int j = 0; j < count; j++)
		{
			CardData cardData = Globals.Instance.GetCardData(sortedList.GetByIndex(j).ToString().Split('_')[0], instantiate: false);
			int num = 0;
			dictionary.Add(value: (cardData.CardClass != Enums.CardClass.Injury) ? ((cardData.CardClass != Enums.CardClass.Boon) ? cardData.EnergyCost : (-2)) : (-1), key: cardData.Id + "_" + sortedList.GetByIndex(j).ToString().Split('_')[1]);
		}
		dictionary = dictionary.OrderBy((KeyValuePair<string, int> x) => x.Value).ToDictionary((KeyValuePair<string, int> x) => x.Key, (KeyValuePair<string, int> x) => x.Value);
		cardVerticalDeck = new CardVertical[dictionary.Count];
		for (int num2 = 0; num2 < dictionary.Count; num2++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(cardVerticalPrefab, new Vector3(0f, 0f, -3f), Quaternion.identity, cardListContainer);
			string key = dictionary.ElementAt(num2).Key;
			int num3 = int.Parse(key.Split('_')[1]);
			gameObject.name = "deckcard_" + num3;
			deckGOs[num3] = gameObject;
			cardVerticalDeck[num3] = gameObject.GetComponent<CardVertical>();
			cardVerticalDeck[num3].SetCard(key, craftType, currentHero);
			if (craftType == 6 || craftType == 0)
			{
				CheckForCorruptableCards(cardVerticalDeck[num3]);
			}
		}
		RedrawGridLayout();
		deckEnergy.WriteEnergy(_heroIndex, 0);
	}

	private void RedrawGridLayout()
	{
		cardListContainer.GetComponent<GridLayoutGroup>().enabled = false;
		cardListContainer.GetComponent<GridLayoutGroup>().enabled = true;
	}

	public bool CanCraftThisCard(CardData cData)
	{
		if (AtOManager.Instance.IsCombatTool)
		{
			return true;
		}
		if (cData.CardUpgraded == Enums.CardUpgraded.Rare)
		{
			return false;
		}
		cData = Functions.GetCardDataFromCardData(cData, "");
		if ((bool)MapManager.Instance && GameManager.Instance.IsObeliskChallenge())
		{
			if (maxCraftRarity == Enums.CardRarity.Mythic)
			{
				return true;
			}
			if (maxCraftRarity == Enums.CardRarity.Epic && cData.CardRarity != Enums.CardRarity.Mythic)
			{
				return true;
			}
			if (maxCraftRarity == Enums.CardRarity.Rare && cData.CardRarity != Enums.CardRarity.Mythic && cData.CardRarity != Enums.CardRarity.Epic)
			{
				return true;
			}
			if (maxCraftRarity == Enums.CardRarity.Uncommon && cData.CardRarity != Enums.CardRarity.Mythic && cData.CardRarity != Enums.CardRarity.Epic && cData.CardRarity != Enums.CardRarity.Rare)
			{
				return true;
			}
			if (maxCraftRarity == Enums.CardRarity.Common && cData.CardRarity == Enums.CardRarity.Common)
			{
				return true;
			}
			return false;
		}
		if (AtOManager.Instance.Sandbox_allRarities)
		{
			return true;
		}
		if (cData.CardRarity == Enums.CardRarity.Mythic)
		{
			return false;
		}
		if (AtOManager.Instance.GetTownTier() == 0)
		{
			if (cData.CardRarity == Enums.CardRarity.Rare && (!PlayerManager.Instance.PlayerHaveSupply("townUpgrade_1_4") || AtOManager.Instance.GetNgPlus() >= 8))
			{
				return false;
			}
			if (cData.CardRarity == Enums.CardRarity.Epic || cData.CardRarity == Enums.CardRarity.Mythic)
			{
				return false;
			}
		}
		else if (AtOManager.Instance.GetTownTier() == 1 && cData.CardRarity == Enums.CardRarity.Epic && (!PlayerManager.Instance.PlayerHaveSupply("townUpgrade_1_6") || AtOManager.Instance.GetNgPlus() >= 8))
		{
			return false;
		}
		return true;
	}

	public void ShowListCardsForCraft(int pageNum, bool reset = false)
	{
		if (heroIndex == -1 || AtOManager.Instance.GetHero(heroIndex) == null || AtOManager.Instance.GetHero(heroIndex).HeroData == null)
		{
			return;
		}
		SetBlocked(_status: false);
		PopupManager.Instance.ClosePopup();
		if (pageNum < 1)
		{
			return;
		}
		if (reset)
		{
			maxCraftPageNum = 1;
		}
		if (pageNum > maxCraftPageNum)
		{
			return;
		}
		currentCraftPageNum = pageNum;
		Enums.CardClass result = Enums.CardClass.None;
		Enum.TryParse<Enums.CardClass>(Enum.GetName(typeof(Enums.HeroClass), AtOManager.Instance.GetHero(heroIndex).HeroData.HeroClass), out result);
		if (result == Enums.CardClass.None)
		{
			return;
		}
		Enums.CardClass result2 = Enums.CardClass.None;
		Enum.TryParse<Enums.CardClass>(Enum.GetName(typeof(Enums.HeroClass), AtOManager.Instance.GetHero(heroIndex).HeroData.HeroSubClass.HeroClassSecondary), out result2);
		Enums.CardClass result3 = Enums.CardClass.None;
		Enum.TryParse<Enums.CardClass>(Enum.GetName(typeof(Enums.HeroClass), AtOManager.Instance.GetHero(heroIndex).HeroData.HeroSubClass.HeroClassThird), out result3);
		List<string> list = new List<string>();
		int num = 0;
		if (AtOManager.Instance.advancedCraft)
		{
			num = Globals.Instance.CardListByClass[result].Count;
			for (int i = 0; i < num; i++)
			{
				list.Add(Globals.Instance.CardListByClass[result][i]);
			}
			list.Sort();
			if (result2 != Enums.CardClass.None)
			{
				num = Globals.Instance.CardListByClass[result2].Count;
				for (int j = 0; j < num; j++)
				{
					list.Add(Globals.Instance.CardListByClass[result2][j]);
				}
				list.Sort();
			}
			if (result3 != Enums.CardClass.None)
			{
				num = Globals.Instance.CardListByClass[result3].Count;
				for (int k = 0; k < num; k++)
				{
					list.Add(Globals.Instance.CardListByClass[result3][k]);
				}
				list.Sort();
			}
		}
		else
		{
			num = Globals.Instance.CardListNotUpgradedByClass[result].Count;
			for (int l = 0; l < num; l++)
			{
				list.Add(Globals.Instance.CardListNotUpgradedByClass[result][l]);
			}
			list.Sort();
			if (result2 != Enums.CardClass.None)
			{
				num = Globals.Instance.CardListNotUpgradedByClass[result2].Count;
				for (int m = 0; m < num; m++)
				{
					list.Add(Globals.Instance.CardListNotUpgradedByClass[result2][m]);
				}
				list.Sort();
			}
			if (result3 != Enums.CardClass.None)
			{
				num = Globals.Instance.CardListNotUpgradedByClass[result3].Count;
				for (int n = 0; n < num; n++)
				{
					list.Add(Globals.Instance.CardListNotUpgradedByClass[result3][n]);
				}
				list.Sort();
			}
		}
		Transform parent = cardCraftContainer;
		float num2 = 5f;
		int num3 = 0;
		float num4 = num2 * 2f;
		int num5 = 0;
		int playerDust = AtOManager.Instance.GetPlayerDust();
		for (int num6 = 0; num6 < list.Count; num6++)
		{
			string text = list[num6];
			CardData cardData = Globals.Instance.GetCardData(text, instantiate: false);
			if (cardData.CardUpgraded != Enums.CardUpgraded.No)
			{
				text = cardData.UpgradedFrom.Trim();
			}
			if ((!PlayerManager.Instance.IsCardUnlocked(text) && !GameManager.Instance.IsObeliskChallenge()) || !CanCraftThisCard(cardData))
			{
				continue;
			}
			if (!currentCraftAllRarities || !currentCraftAllCosts)
			{
				if (!currentCraftAllRarities && !currentCraftRarity[cardData.CardRarity])
				{
					continue;
				}
				if (!currentCraftAllCosts)
				{
					if (currentCraftCost < 4)
					{
						if (cardData.EnergyCost != currentCraftCost)
						{
							continue;
						}
					}
					else if (cardData.EnergyCost < currentCraftCost)
					{
						continue;
					}
				}
			}
			bool flag = true;
			if (flag && AtOManager.Instance.craftFilterDT.Count > 0)
			{
				foreach (string item in AtOManager.Instance.craftFilterDT)
				{
					flag = false;
					string text2 = item;
					if (text2 != "heal" && text2 != "energy" && text2 != "draw" && text2 != "discard")
					{
						if (text2 == "slash")
						{
							text2 = "slashing";
						}
						if (cardData.DamageType != Enums.DamageType.None && Enum.GetName(typeof(Enums.DamageType), cardData.DamageType).ToLower() == text2)
						{
							flag = true;
						}
						else if (cardData.DamageType2 != Enums.DamageType.None && Enum.GetName(typeof(Enums.DamageType), cardData.DamageType2).ToLower() == text2)
						{
							flag = true;
						}
					}
					else
					{
						switch (text2)
						{
						case "heal":
							if (cardData.Heal > 0 || cardData.HealSides > 0 || cardData.HealSelf > 0)
							{
								flag = true;
							}
							break;
						case "energy":
							if (cardData.EnergyRecharge > 0)
							{
								flag = true;
							}
							break;
						case "draw":
							if (cardData.DrawCard > 0)
							{
								flag = true;
							}
							break;
						case "discard":
							if (cardData.DiscardCard > 0)
							{
								flag = true;
							}
							break;
						}
					}
					if (!flag)
					{
						break;
					}
				}
			}
			if (flag && AtOManager.Instance.craftFilterAura.Count > 0)
			{
				foreach (string item2 in AtOManager.Instance.craftFilterAura)
				{
					flag = false;
					string _aura = "";
					if (item2 == "stanza")
					{
						_aura = "stanzai";
					}
					else
					{
						_aura = item2;
					}
					if (cardData.Aura != null && cardData.Aura.Id == _aura)
					{
						flag = true;
					}
					else if (cardData.Aura2 != null && cardData.Aura2.Id == _aura)
					{
						flag = true;
					}
					else if (cardData.Aura3 != null && cardData.Aura3.Id == _aura)
					{
						flag = true;
					}
					else if (cardData.AuraSelf != null && cardData.AuraSelf.Id == _aura)
					{
						flag = true;
					}
					else if (cardData.AuraSelf2 != null && cardData.AuraSelf2.Id == _aura)
					{
						flag = true;
					}
					else if (cardData.AuraSelf3 != null && cardData.AuraSelf3.Id == _aura)
					{
						flag = true;
					}
					else if (cardData.Auras != null && cardData.Auras.Length != 0)
					{
						if (cardData.Auras.Any((CardData.AuraBuffs x) => (x.aura != null && x.aura.Id == _aura) || (x.auraSelf != null && x.auraSelf.Id == _aura)))
						{
							flag = true;
						}
					}
					else if (cardData.SpecialAuraCurseNameGlobal != null && cardData.SpecialAuraCurseNameGlobal.Id == _aura)
					{
						flag = true;
					}
					else if (cardData.SpecialAuraCurseName1 != null && cardData.SpecialAuraCurseName1.Id == _aura)
					{
						flag = true;
					}
					else if (cardData.SpecialAuraCurseName2 != null && cardData.SpecialAuraCurseName2.Id == _aura)
					{
						flag = true;
					}
					else if (cardData.HealAuraCurseSelf != null && cardData.HealAuraCurseSelf.Id == _aura)
					{
						flag = true;
					}
					else if (cardData.HealAuraCurseName != null && cardData.HealAuraCurseName.Id == _aura)
					{
						flag = true;
					}
					else if (cardData.HealAuraCurseName2 != null && cardData.HealAuraCurseName2.Id == _aura)
					{
						flag = true;
					}
					else if (cardData.HealAuraCurseName3 != null && cardData.HealAuraCurseName3.Id == _aura)
					{
						flag = true;
					}
					else if (cardData.HealAuraCurseName4 != null && cardData.HealAuraCurseName4.Id == _aura)
					{
						flag = true;
					}
					if (!flag)
					{
						break;
					}
				}
			}
			if (flag && AtOManager.Instance.craftFilterCurse.Count > 0)
			{
				foreach (string curse in AtOManager.Instance.craftFilterCurse)
				{
					flag = false;
					if (cardData.Curse != null && cardData.Curse.Id == curse)
					{
						flag = true;
					}
					else if (cardData.Curse2 != null && cardData.Curse2.Id == curse)
					{
						flag = true;
					}
					else if (cardData.Curse3 != null && cardData.Curse3.Id == curse)
					{
						flag = true;
					}
					else if (cardData.CurseSelf != null && cardData.CurseSelf.Id == curse)
					{
						flag = true;
					}
					else if (cardData.CurseSelf2 != null && cardData.CurseSelf2.Id == curse)
					{
						flag = true;
					}
					else if (cardData.CurseSelf3 != null && cardData.CurseSelf3.Id == curse)
					{
						flag = true;
					}
					else if (cardData.Curses != null && cardData.Curses.Length != 0)
					{
						if (cardData.Curses.Any((CardData.CurseDebuffs x) => (x.curse != null && x.curse.Id == curse) || (x.curseSelf != null && x.curseSelf.Id == curse)))
						{
							flag = true;
						}
					}
					else if (cardData.SpecialAuraCurseNameGlobal != null && cardData.SpecialAuraCurseNameGlobal.Id == curse)
					{
						flag = true;
					}
					else if (cardData.SpecialAuraCurseName1 != null && cardData.SpecialAuraCurseName1.Id == curse)
					{
						flag = true;
					}
					else if (cardData.SpecialAuraCurseName2 != null && cardData.SpecialAuraCurseName2.Id == curse)
					{
						flag = true;
					}
					else if (cardData.HealAuraCurseSelf != null && cardData.HealAuraCurseSelf.Id == curse)
					{
						flag = true;
					}
					else if (cardData.HealAuraCurseName != null && cardData.HealAuraCurseName.Id == curse)
					{
						flag = true;
					}
					else if (cardData.HealAuraCurseName2 != null && cardData.HealAuraCurseName2.Id == curse)
					{
						flag = true;
					}
					else if (cardData.HealAuraCurseName3 != null && cardData.HealAuraCurseName3.Id == curse)
					{
						flag = true;
					}
					else if (cardData.HealAuraCurseName4 != null && cardData.HealAuraCurseName4.Id == curse)
					{
						flag = true;
					}
					if (!flag)
					{
						break;
					}
				}
			}
			if (!flag || (searchTerm != "" && !Globals.Instance.IsInSearch(searchTerm, cardData.Id)))
			{
				continue;
			}
			int num7 = ((!AtOManager.Instance.IsCombatTool) ? SetPrice("Craft", "", list[num6], craftTierZone) : 0);
			if (num7 == -1)
			{
				continue;
			}
			if (AtOManager.Instance.affordableCraft)
			{
				if (num7 > playerDust)
				{
					continue;
				}
				int[] cardAvailability = GetCardAvailability(list[num6], "");
				int num8 = cardAvailability[0];
				int num9 = cardAvailability[1];
				if (num8 >= num9)
				{
					continue;
				}
			}
			if ((float)num3 >= (float)(pageNum - 1) * num4 && (float)num3 < (float)pageNum * num4)
			{
				if (!craftCardItemDict.ContainsKey(num5))
				{
					CardCraftItem component = UnityEngine.Object.Instantiate(cardCraftItem, new Vector3(0f, 0f, -3f), Quaternion.identity, parent).transform.GetComponent<CardCraftItem>();
					craftCardItemDict.Add(num5, component);
					float num10 = 0f;
					float num11 = 0f;
					int num12 = Mathf.FloorToInt((float)num5 / num2);
					num10 = -1.8f + (float)num5 % num2 * 2.25f;
					num11 = 1.8f - 3.7f * (float)num12;
					component.SetPosition(new Vector3(num10, num11, 0f));
					component.SetIndex(num5);
					component.SetHero(currentHero);
					component.SetGenericCard();
					component.SetButtonText(ButtonText(num7));
					component.SetCard(list[num6], "", currentHero);
				}
				else
				{
					CardCraftItem obj = craftCardItemDict[num5];
					obj.SetButtonText(ButtonText(num7));
					obj.SetCard(list[num6], "", currentHero);
					obj.transform.gameObject.SetActive(value: true);
				}
				num5++;
			}
			num3++;
		}
		if ((float)num5 < num2 * 2f)
		{
			for (int num13 = num5; (float)num13 < num2 * 2f; num13++)
			{
				if (craftCardItemDict.ContainsKey(num13))
				{
					craftCardItemDict[num13].transform.gameObject.SetActive(value: false);
				}
			}
		}
		CreateCraftPages(pageNum, Mathf.CeilToInt((float)num3 / num4));
		if (AtOManager.Instance.TownTutorialStep != 0)
		{
			return;
		}
		foreach (KeyValuePair<int, CardCraftItem> item3 in craftCardItemDict)
		{
			if (item3.Value.cardId != "fireball")
			{
				item3.Value.EnableButton(_state: false);
			}
			else
			{
				item3.Value.EnableButton(_state: true);
			}
		}
	}

	public void SelectFilter(string type, string name, bool state)
	{
		switch (type)
		{
		case "dt":
			if (state)
			{
				if (!craftFilterDT.Contains(name))
				{
					craftFilterDT.Add(name);
				}
			}
			else
			{
				craftFilterDT.Remove(name);
			}
			break;
		case "aura":
			if (state)
			{
				if (!craftFilterAura.Contains(name))
				{
					craftFilterAura.Add(name);
				}
			}
			else
			{
				craftFilterAura.Remove(name);
			}
			break;
		case "curse":
			if (state)
			{
				if (!craftFilterCurse.Contains(name))
				{
					craftFilterCurse.Add(name);
				}
			}
			else
			{
				craftFilterCurse.Remove(name);
			}
			break;
		}
	}

	public void ShowFilter(bool state)
	{
		PopupManager.Instance.ClosePopup();
		filterWindow.gameObject.SetActive(state);
		if (state)
		{
			controllerVerticalIndex = -1;
			ShowSearch(state: false);
			craftFilterDT = new List<string>();
			for (int i = 0; i < AtOManager.Instance.craftFilterDT.Count; i++)
			{
				craftFilterDT.Add(AtOManager.Instance.craftFilterDT[i]);
			}
			craftFilterAura = new List<string>();
			for (int j = 0; j < AtOManager.Instance.craftFilterAura.Count; j++)
			{
				craftFilterAura.Add(AtOManager.Instance.craftFilterAura[j]);
			}
			craftFilterCurse = new List<string>();
			for (int k = 0; k < AtOManager.Instance.craftFilterCurse.Count; k++)
			{
				craftFilterCurse.Add(AtOManager.Instance.craftFilterCurse[k]);
			}
			if (craftBotonFilters.Count == 0)
			{
				foreach (Transform item in filterWindow.GetChild(2))
				{
					if ((bool)item.GetComponent<BotonFilter>())
					{
						craftBotonFilters.Add(item.GetComponent<BotonFilter>());
					}
				}
				foreach (Transform item2 in filterWindow.GetChild(3))
				{
					if ((bool)item2.GetComponent<BotonFilter>())
					{
						craftBotonFilters.Add(item2.GetComponent<BotonFilter>());
					}
				}
				foreach (Transform item3 in filterWindow.GetChild(4))
				{
					if ((bool)item3.GetComponent<BotonFilter>())
					{
						craftBotonFilters.Add(item3.GetComponent<BotonFilter>());
					}
				}
				foreach (Transform item4 in filterWindow.GetChild(5))
				{
					if ((bool)item4.GetComponent<BotonFilter>())
					{
						craftBotonFilters.Add(item4.GetComponent<BotonFilter>());
					}
				}
			}
			foreach (Transform item5 in filterWindow.GetChild(2))
			{
				if ((bool)item5.GetComponent<BotonFilter>())
				{
					if (craftFilterDT.Contains(item5.GetComponent<BotonFilter>().id))
					{
						item5.GetComponent<BotonFilter>().select(state: true);
					}
					else
					{
						item5.GetComponent<BotonFilter>().select(state: false);
					}
				}
			}
			foreach (Transform item6 in filterWindow.GetChild(3))
			{
				if ((bool)item6.GetComponent<BotonFilter>())
				{
					if (craftFilterDT.Contains(item6.GetComponent<BotonFilter>().id))
					{
						item6.GetComponent<BotonFilter>().select(state: true);
					}
					else
					{
						item6.GetComponent<BotonFilter>().select(state: false);
					}
				}
			}
			foreach (Transform item7 in filterWindow.GetChild(4))
			{
				if ((bool)item7.GetComponent<BotonFilter>())
				{
					if (craftFilterAura.Contains(item7.GetComponent<BotonFilter>().id))
					{
						item7.GetComponent<BotonFilter>().select(state: true);
					}
					else
					{
						item7.GetComponent<BotonFilter>().select(state: false);
					}
				}
			}
			{
				foreach (Transform item8 in filterWindow.GetChild(5))
				{
					if ((bool)item8.GetComponent<BotonFilter>())
					{
						if (craftFilterCurse.Contains(item8.GetComponent<BotonFilter>().id))
						{
							item8.GetComponent<BotonFilter>().select(state: true);
						}
						else
						{
							item8.GetComponent<BotonFilter>().select(state: false);
						}
					}
				}
				return;
			}
		}
		ShowSearch(state: true);
		SetFilterButtonState();
	}

	private void SetFilterButtonState()
	{
		if (AtOManager.Instance.craftFilterDT.Count > 0 || AtOManager.Instance.craftFilterAura.Count > 0 || AtOManager.Instance.craftFilterCurse.Count > 0)
		{
			buttonFilterCraft.SetState(state: true);
			buttonFilterCraft.GetComponent<BotonGeneric>().ShowDisableMask(state: false);
		}
		else
		{
			buttonFilterCraft.SetState(state: false);
			buttonFilterCraft.GetComponent<BotonGeneric>().ShowDisableMask(state: true);
		}
	}

	public void ResetFilter()
	{
		craftFilterAura.Clear();
		craftFilterCurse.Clear();
		craftFilterDT.Clear();
		foreach (BotonFilter craftBotonFilter in craftBotonFilters)
		{
			craftBotonFilter.select(state: false);
		}
		ApplyFilter();
	}

	public void ApplyFilter()
	{
		AtOManager.Instance.craftFilterDT = craftFilterDT;
		AtOManager.Instance.craftFilterAura = craftFilterAura;
		AtOManager.Instance.craftFilterCurse = craftFilterCurse;
		ShowListCardsForCraft(1, reset: true);
		ShowFilter(state: false);
	}

	public void AssignChallengeTitle(int block, string title)
	{
		cardChallengeTitle[block].SetText(title);
		ShowChallengeTitle(state: true, block);
	}

	public void AssignChallengeRoundCards(int round, int maxRound)
	{
		cardChallengeGlobalTitle.text = Texts.Instance.GetText("challengeSelection");
		cardChallengeGlobalIntro.text = Texts.Instance.GetText("challengeSelectionIntro");
		string format = "<color=#FFF>{0}</color> <size=+.2>[<color=#FC0>{1}</color> / {2}]";
		cardChallengeRound.text = string.Format(format, Texts.Instance.GetText("challengeRound"), round, maxRound);
	}

	public void AssignChallengeRoundPerks(int selected, int maxPerks)
	{
		cardChallengeGlobalTitle.text = Texts.Instance.GetText("challengeSelectionPerk");
		cardChallengeGlobalIntro.text = Texts.Instance.GetText("challengeSelectionPerkIntro");
		string format = "<color=#FFF>{0}</color><br><size=+2>[<color=#FC0>{1}</color> / {2}]";
		cardChallengeRound.text = string.Format(format, Texts.Instance.GetText("challengeRoundPerks"), selected, maxPerks);
	}

	public void AssignChallengeCard(int hero, int block, int row, string theCard, bool selectedPack = false)
	{
		if (heroIndex == hero)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(cardVerticalPrefab, new Vector3(0f, 0f, -3f), Quaternion.identity, cardChallengeContainer[block]);
			gameObject.gameObject.SetActive(value: false);
			if (assignSpecial != null)
			{
				StopCoroutine(assignSpecial);
			}
			assignCard = StartCoroutine(ShowChallengeCardCo(gameObject, row, theCard, block, selectedPack));
		}
	}

	private IEnumerator ShowChallengeCardCo(GameObject card, int row, string theCard, int block, bool selectedPack)
	{
		yield return Globals.Instance.WaitForSeconds(0.15f + (float)(row + 1) * 0.05f);
		GameManager.Instance.PlayLibraryAudio("dealcard", 0.08f);
		if (!(card != null))
		{
			yield break;
		}
		CardVertical component = card.GetComponent<CardVertical>();
		component.SetCard(theCard, 0, currentHero);
		if (!card.gameObject.activeSelf)
		{
			card.gameObject.SetActive(value: true);
		}
		CardData cardData = Globals.Instance.GetCardData(theCard, instantiate: false);
		if (cardData.CardUpgraded == Enums.CardUpgraded.A)
		{
			component.PlayParticle("A");
		}
		else if (cardData.CardUpgraded == Enums.CardUpgraded.B)
		{
			component.PlayParticle("B");
		}
		if (row == ChallengeSelectionManager.Instance.CardsForPack - 1)
		{
			if (selectedPack)
			{
				ShowChallengePackSelected(_state: true, block);
				ShowChallengeButtons(state: false, block);
			}
			else if (!GameManager.Instance.IsMultiplayer() || AtOManager.Instance.GetHero(ChallengeSelectionManager.Instance.currentHeroIndex).Owner == NetworkManager.Instance.GetPlayerNick())
			{
				ShowChallengeButtons(state: true, block);
			}
			else
			{
				ShowChallengeButtons(state: false, block);
			}
		}
	}

	public void AssignChallengeCardSpecial(string theCards, bool showButtons)
	{
		if (assignCard != null)
		{
			StopCoroutine(assignCard);
		}
		if (assignSpecial != null)
		{
			StopCoroutine(assignSpecial);
		}
		assignSpecial = StartCoroutine(ShowChallengeCardSpecialCo(theCards, showButtons));
	}

	private IEnumerator ShowChallengeCardSpecialCo(string theCards, bool showButtons)
	{
		string[] theCardsArr = theCards.Split('_');
		for (int i = 0; i < 3; i++)
		{
			string text = theCardsArr[i];
			if (text != null)
			{
				GameObject obj = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, new Vector3(-0.7f + 3.8f * (float)i, 0.35f, -5f), Quaternion.identity, tmpContainer);
				CardItem component = obj.GetComponent<CardItem>();
				obj.name = "card_" + i;
				component.SetCard(text);
				component.DoReward(fromReward: false, fromEvent: true, fromLoot: false, selectable: true, 0.3f);
				component.SetDestinationLocalScale(1.2f);
				component.TopLayeringOrder("UI");
				component.cardmakebig = true;
				component.CreateColliderAdjusted();
				component.cardmakebigSize = 1.2f;
				component.cardmakebigSizeMax = 1.3f;
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
		}
		if (showButtons)
		{
			yield return Globals.Instance.WaitForSeconds(0.35f);
			for (int j = 0; j < 3; j++)
			{
				ShowChallengeButtons(state: true, j);
			}
		}
	}

	public void CleanChallengeBlocks(int _theBlock = -1)
	{
		for (int i = 0; i < cardChallengeContainer.Length; i++)
		{
			if (_theBlock != -1 && i != _theBlock)
			{
				continue;
			}
			foreach (Transform item in cardChallengeContainer[i])
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
			ShowChallengePackSelected(_state: false, i);
		}
		if (_theBlock == -1)
		{
			ShowChallengeButtons(state: false);
			ShowChallengeTitles(state: false);
			ClearTmpContainer();
		}
	}

	public void ShowChallengeTitle(bool state, int _theBlock = -1)
	{
		if (cardChallengeTitle[_theBlock].gameObject.activeSelf != state)
		{
			cardChallengeTitle[_theBlock].gameObject.SetActive(state);
		}
		if (!state)
		{
			return;
		}
		if (ChallengeSelectionManager.Instance.GetCurrentRound() < 3)
		{
			cardChallengeTitle[_theBlock].transform.localPosition = new Vector3(-8.25f + (float)(_theBlock % 4) * 3.8f, -1.3f, cardChallengeTitle[_theBlock].transform.localPosition.z);
			if (_theBlock != 0 && _theBlock != 1)
			{
				return;
			}
			{
				foreach (Transform item in cardChallengeTitle[_theBlock].transform)
				{
					if (item != null && item.name == "Recommended")
					{
						item.gameObject.SetActive(value: true);
						break;
					}
				}
				return;
			}
		}
		cardChallengeTitle[_theBlock].transform.localPosition = new Vector3(-7.35f + (float)_theBlock * 4.75f, -1.65f, cardChallengeTitle[_theBlock].transform.localPosition.z);
		if (_theBlock != 0 && _theBlock != 1)
		{
			return;
		}
		foreach (Transform item2 in cardChallengeTitle[_theBlock].transform)
		{
			if (item2 != null && item2.name == "Recommended")
			{
				item2.gameObject.SetActive(value: false);
				break;
			}
		}
	}

	public void ShowChallengeTitles(bool state)
	{
		for (int i = 0; i < cardChallengeTitle.Length; i++)
		{
			if (cardChallengeTitle[i].gameObject.activeSelf != state)
			{
				cardChallengeTitle[i].gameObject.SetActive(state);
			}
		}
	}

	public void ShowChallengePackSelected(bool _state, int _block = -1)
	{
		if (cardChallengeSelected[_block].gameObject.activeSelf != _state)
		{
			cardChallengeSelected[_block].gameObject.SetActive(_state);
		}
		if (!_state)
		{
			return;
		}
		foreach (Transform item in cardChallengeContainer[_block])
		{
			item.GetComponent<CardVertical>().SetBgColor("#FF8500");
		}
	}

	public void ReassignChallengeButtons()
	{
		if ((bool)ChallengeSelectionManager.Instance)
		{
			for (int i = 0; i < cardChallengeButton.Length; i++)
			{
				cardChallengeButton[i].GetComponent<BotonGeneric>().auxInt = ChallengeSelectionManager.Instance.currentHeroIndex;
			}
		}
	}

	public void ShowChallengeButtons(bool state, int block = -1)
	{
		if (!ChallengeSelectionManager.Instance)
		{
			return;
		}
		for (int i = 0; i < cardChallengeButton.Length; i++)
		{
			if (block != -1 && block != i)
			{
				continue;
			}
			if (cardChallengeButton[i].gameObject.activeSelf != state)
			{
				if (state)
				{
					cardChallengeButton[i].gameObject.SetActive(value: true);
				}
				else
				{
					cardChallengeButton[i].gameObject.SetActive(value: false);
				}
			}
			if (state)
			{
				if (ChallengeSelectionManager.Instance.GetCurrentRound() < 3)
				{
					cardChallengeButton[i].localPosition = new Vector3(-8.1f + (float)(i % 4) * 3.8f, -3.3f, cardChallengeButton[i].localPosition.z);
				}
				else
				{
					cardChallengeButton[i].localPosition = new Vector3(-7.2f + (float)i * 4.75f, -6.25f, cardChallengeButton[i].localPosition.z);
				}
			}
		}
		if (state)
		{
			for (int j = 0; j < cardChallengeButton.Length; j++)
			{
				cardChallengeButton[j].GetComponent<BotonGeneric>().auxInt = ChallengeSelectionManager.Instance.currentHeroIndex;
			}
		}
	}

	public void ShowChallengeReroll(bool state)
	{
		if (state)
		{
			rerollChallenge.Enable();
		}
		else
		{
			rerollChallenge.Disable();
		}
	}

	public void ActivateChallengeReroll(bool state)
	{
		rerollChallenge.EnabledButton(state);
	}

	public void ShowChallengeRerollFully(bool state)
	{
		rerollChallenge.gameObject.SetActive(state);
	}

	public void ShowChallengePerks(bool state)
	{
		if (challengePerks.gameObject.activeSelf != state)
		{
			challengePerks.gameObject.SetActive(state);
		}
		if (state)
		{
			if (assignCard != null)
			{
				StopCoroutine(assignCard);
			}
			if (assignSpecial != null)
			{
				StopCoroutine(assignSpecial);
			}
		}
	}

	public void EnableChallengeReadyButton(bool state)
	{
		if (state)
		{
			readyChallenge.GetComponent<BotonGeneric>().Enable();
		}
		else
		{
			readyChallenge.GetComponent<BotonGeneric>().Disable();
		}
	}

	public void ChallengeReadySetButton(bool state)
	{
		if (state)
		{
			readyChallenge.GetComponent<BotonGeneric>().SetColorAbsolute(Functions.HexToColor("#15A42E"));
			if (GameManager.Instance.IsMultiplayer())
			{
				readyChallenge.GetComponent<BotonGeneric>().SetText(Texts.Instance.GetText("waitingForPlayers"));
			}
			else
			{
				readyChallenge.GetComponent<BotonGeneric>().SetText(Texts.Instance.GetText("ready"));
			}
		}
		else
		{
			readyChallenge.GetComponent<BotonGeneric>().SetColorAbsolute(Functions.HexToColor(Globals.Instance.ClassColor["warrior"]));
			readyChallenge.GetComponent<BotonGeneric>().SetText(Texts.Instance.GetText("ready"));
		}
	}

	public void SetWaitingPlayerTextChallenge(string msg)
	{
		if (msg != "")
		{
			waitingMsgChallenge.gameObject.SetActive(value: true);
			waitingMsgTextChallenge.text = msg;
		}
		else
		{
			waitingMsgChallenge.gameObject.SetActive(value: false);
		}
	}

	public void ShowSaveLoad()
	{
		if (!cardCraftSave.gameObject.activeSelf)
		{
			cardCraftElements.gameObject.SetActive(value: false);
			cardCraftSave.gameObject.SetActive(value: true);
			deckCraftPrice.text = "";
			botSaveLoad.SetText(Texts.Instance.GetText("saveLoadReturn"));
			botSaveLoad.SetPopupText("");
			LoadDecks();
		}
		else
		{
			cardCraftElements.gameObject.SetActive(value: true);
			cardCraftSave.gameObject.SetActive(value: false);
			botSaveLoad.SetText(Texts.Instance.GetText("saveLoad"));
			botSaveLoad.SetPopupText(Texts.Instance.GetText("saveLoadDes"));
			CheckForLoadSaveCorrect(checkLockStatus: false);
			ShowListCardsForCraft(1, reset: true);
		}
	}

	public void SaveDeck(int slot)
	{
		savingSlot = slot;
		AlertManager.buttonClickDelegate = SaveDeckAction;
		AlertManager.Instance.AlertInput(Texts.Instance.GetText("inputSaveName"), Texts.Instance.GetText("accept").ToUpper());
	}

	public void SaveDeckAction()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(SaveDeckAction));
		string text = Functions.OnlyAscii(AlertManager.Instance.GetInputValue()).Trim();
		if (text == "")
		{
			return;
		}
		string value = Enum.GetName(typeof(Enums.HeroClass), AtOManager.Instance.GetHero(heroIndex).HeroData.HeroClass);
		_ = (Enums.CardClass)Enum.Parse(typeof(Enums.CardClass), value);
		Hero hero = AtOManager.Instance.GetHero(heroIndex);
		List<string> list = new List<string>();
		List<string> cards = hero.Cards;
		for (int i = 0; i < cards.Count; i++)
		{
			CardData cardData = Globals.Instance.GetCardData(cards[i], instantiate: false);
			if (cardData.CardClass != Enums.CardClass.Injury && cardData.CardClass != Enums.CardClass.Boon && cardData.CardUpgraded != Enums.CardUpgraded.Rare)
			{
				list.Add(cards[i]);
			}
		}
		if (GameManager.Instance.IsSingularity())
		{
			for (int j = 0; j < list.Count; j++)
			{
				CardData cardData2 = Globals.Instance.GetCardData(list[j], instantiate: false);
				if (GameManager.Instance.IsSingularity() && cardData2.CardUpgraded != Enums.CardUpgraded.No && !cardData2.Starter)
				{
					list[j] = cardData2.BaseCard.ToLower();
				}
			}
		}
		string sourceName = AtOManager.Instance.GetHero(heroIndex).SourceName;
		if (!PlayerManager.Instance.PlayerSavedDeck.DeckTitle.ContainsKey(hero.SourceName))
		{
			PlayerManager.Instance.PlayerSavedDeck.DeckTitle.Add(sourceName, new string[20]);
		}
		if (!PlayerManager.Instance.PlayerSavedDeck.DeckCards.ContainsKey(hero.SourceName))
		{
			PlayerManager.Instance.PlayerSavedDeck.DeckCards.Add(sourceName, new List<string>[20]);
		}
		PlayerManager.Instance.PlayerSavedDeck.DeckTitle[sourceName][savingSlot] = text;
		PlayerManager.Instance.PlayerSavedDeck.DeckCards[sourceName][savingSlot] = list;
		SaveManager.SavePlayerDeck();
		LoadDecks(refreshDecks: false);
	}

	private void LoadDecks(bool refreshDecks = true)
	{
		if (refreshDecks)
		{
			SaveManager.LoadPlayerDeck();
		}
		loadDeckContainer.gameObject.SetActive(value: false);
		CheckForLoadSaveCorrect();
		Hero hero = AtOManager.Instance.GetHero(heroIndex);
		string sourceName = hero.SourceName;
		Enum.GetName(typeof(Enums.HeroClass), hero.HeroData.HeroClass);
		loadDeckHeroSprite.sprite = hero.HeroData.HeroSubClass.SpriteBorder;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<size=+.5><color=");
		stringBuilder.Append(Globals.Instance.ClassColor[Enum.GetName(typeof(Enums.HeroClass), hero.HeroData.HeroClass)]);
		stringBuilder.Append(">");
		stringBuilder.Append(sourceName);
		stringBuilder.Append("</size></color><br>");
		stringBuilder.Append(Texts.Instance.GetText("savedDecks"));
		loadDeckHeroName.text = stringBuilder.ToString();
		for (int i = 0; i < deckSlot.Length; i++)
		{
			if (deckSlot[i] != null)
			{
				stringBuilder.Clear();
				if (PlayerManager.Instance.PlayerSavedDeck.DeckTitle.ContainsKey(sourceName) && PlayerManager.Instance.PlayerSavedDeck.DeckTitle[sourceName][i] != null && PlayerManager.Instance.PlayerSavedDeck.DeckTitle[sourceName][i] != "")
				{
					stringBuilder.Append(PlayerManager.Instance.PlayerSavedDeck.DeckTitle[sourceName][i]);
					deckSlot[i].SetActive(stringBuilder.ToString(), PlayerManager.Instance.PlayerSavedDeck.DeckCards[sourceName][i].Count.ToString());
				}
				else
				{
					deckSlot[i].SetEmpty(deckAvailableForSaveLoad);
				}
			}
		}
		stringBuilder = null;
	}

	private void CheckForLoadSaveCorrect(bool checkLockStatus = true)
	{
		deckAvailableForSaveLoad = true;
		for (int i = 0; i < cardVerticalDeck.Length; i++)
		{
			if ((checkLockStatus && cardVerticalDeck[i].cardData.CardClass == Enums.CardClass.Injury) || cardVerticalDeck[i].cardData.CardClass == Enums.CardClass.Boon || cardVerticalDeck[i].cardData.CardUpgraded == Enums.CardUpgraded.Rare || (cardVerticalDeck[i].cardData.CardClass == Enums.CardClass.Special && !cardVerticalDeck[i].cardData.Starter))
			{
				cardVerticalDeck[i].ShowLock(state: true);
				deckAvailableForSaveLoad = false;
			}
			else
			{
				cardVerticalDeck[i].ShowLock(state: false);
			}
		}
	}

	public void LoadDeck(int slot)
	{
		deckCraftingCostDust = 0;
		deckCraftingCostGold = 0;
		craftTimes = new Dictionary<string, int>();
		List<string> list = new List<string>();
		savingSlot = slot;
		string sourceName = AtOManager.Instance.GetHero(heroIndex).SourceName;
		Enum.GetName(typeof(Enums.HeroClass), AtOManager.Instance.GetHero(heroIndex).HeroData.HeroClass);
		list = PlayerManager.Instance.PlayerSavedDeck.DeckCards[sourceName][slot];
		if (list == null)
		{
			return;
		}
		int characterTier = PlayerManager.Instance.GetCharacterTier("", "card", AtOManager.Instance.GetHero(heroIndex).PerkRank);
		if (characterTier > 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				CardData cardData = Globals.Instance.GetCardData(list[i], instantiate: false);
				if (cardData.Starter)
				{
					switch (characterTier)
					{
					case 1:
						list[i] = Functions.GetCardDataFromCardData(cardData, "A").Id.ToLower();
						break;
					case 2:
						list[i] = Functions.GetCardDataFromCardData(cardData, "B").Id.ToLower();
						break;
					}
				}
			}
		}
		foreach (Transform item in loadDeckCardContainer)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		containerDeckName.text = PlayerManager.Instance.PlayerSavedDeck.DeckTitle[sourceName][slot].ToUpper();
		loadDeckContainer.gameObject.SetActive(value: true);
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		SortedList sortedList = new SortedList();
		for (int j = 0; j < list.Count; j++)
		{
			sortedList.Add(Globals.Instance.GetCardData(list[j], instantiate: false).CardName + list[j] + j, list[j] + "_" + j);
		}
		for (int k = 0; k < list.Count; k++)
		{
			CardData cardData2 = Globals.Instance.GetCardData(sortedList.GetByIndex(k).ToString().Split('_')[0], instantiate: false);
			int num = 0;
			dictionary.Add(value: (cardData2.CardClass != Enums.CardClass.Injury) ? ((cardData2.CardClass != Enums.CardClass.Boon) ? cardData2.EnergyCost : (-2)) : (-1), key: cardData2.Id + "_" + sortedList.GetByIndex(k).ToString().Split('_')[1]);
		}
		dictionary = dictionary.OrderBy((KeyValuePair<string, int> x) => x.Value).ToDictionary((KeyValuePair<string, int> x) => x.Key, (KeyValuePair<string, int> x) => x.Value);
		cardVerticalDeck = new CardVertical[dictionary.Count];
		for (int num2 = 0; num2 < dictionary.Count; num2++)
		{
			CardData cardData3 = Globals.Instance.GetCardData(dictionary.ElementAt(num2).Key.Split('_')[0], instantiate: false);
			if (craftType != 0 || (cardData3.CardClass != Enums.CardClass.Injury && cardData3.CardClass != Enums.CardClass.Boon))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(cardVerticalPrefab, new Vector3(0f, 0f, -3f), Quaternion.identity, loadDeckCardContainer);
				string key = dictionary.ElementAt(num2).Key;
				int num3 = int.Parse(key.Split('_')[1]);
				gameObject.name = key.Split('_')[0];
				cardVerticalDeck[num3] = gameObject.GetComponent<CardVertical>();
				cardVerticalDeck[num3].SetCard(key, craftType, currentHero);
			}
		}
		loadDeckCardContainer.GetComponent<GridLayoutGroup>().enabled = false;
		loadDeckCardContainer.GetComponent<GridLayoutGroup>().enabled = true;
		SetPrice(list);
	}

	private void SetPrice(List<string> _targetDeck)
	{
		bool flag = true;
		int townTier = AtOManager.Instance.GetTownTier();
		int num = 0;
		int num2 = 0;
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		List<string> list4 = new List<string>();
		for (int i = 0; i < currentHero.Cards.Count; i++)
		{
			list.Add(currentHero.Cards[i].ToLower());
		}
		for (int j = 0; j < _targetDeck.Count; j++)
		{
			list2.Add(_targetDeck[j]);
		}
		if (list2.Count < 15 && (!SandboxManager.Instance.IsEnabled() || !AtOManager.Instance.Sandbox_noMinimumDecksize))
		{
			flag = false;
		}
		if (flag)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			for (int num3 = list2.Count - 1; num3 >= 0; num3--)
			{
				for (int k = 0; k < list.Count; k++)
				{
					if (list2[num3] == list[k])
					{
						if (dictionary.ContainsKey(list2[num3]))
						{
							dictionary[list2[num3]]++;
						}
						else
						{
							dictionary[list2[num3]] = 1;
						}
						list2.RemoveAt(num3);
						list.RemoveAt(k);
						break;
					}
				}
			}
			for (int num4 = list2.Count - 1; num4 >= 0; num4--)
			{
				CardData cardData = Globals.Instance.GetCardData(list2[num4], instantiate: false);
				if (cardData.CardUpgraded != Enums.CardUpgraded.No)
				{
					CardData cardData2 = Globals.Instance.GetCardData(cardData.UpgradedFrom, instantiate: false);
					for (int l = 0; l < list.Count; l++)
					{
						if (list[l].ToLower() == cardData2.UpgradesTo1.ToLower() || list[l].ToLower() == cardData2.UpgradesTo2.ToLower())
						{
							list3.Add(list2[num4]);
							list2.RemoveAt(num4);
							list.RemoveAt(l);
							break;
						}
						if (list[l].ToLower() == cardData.UpgradedFrom.ToLower())
						{
							list4.Add(list2[num4]);
							list2.RemoveAt(num4);
							list.RemoveAt(l);
							break;
						}
					}
				}
			}
			for (int m = 0; m < list2.Count; m++)
			{
				int num5 = SetPrice("Craft", "", list2[m], townTier);
				num += num5;
			}
			for (int n = 0; n < list.Count; n++)
			{
				this.cardData = Globals.Instance.GetCardData(list[n], instantiate: false);
				int num6 = SetPrice("Remove", "");
				num2 += num6;
			}
			for (int num7 = 0; num7 < list3.Count; num7++)
			{
				this.cardData = Globals.Instance.GetCardData(list3[num7], instantiate: false);
				int num8 = SetPrice("Transform", Enum.GetName(typeof(Enums.CardRarity), this.cardData.CardRarity));
				num += num8;
			}
			for (int num9 = 0; num9 < list4.Count; num9++)
			{
				this.cardData = Globals.Instance.GetCardData(list4[num9], instantiate: false);
				int num10 = SetPrice("Upgrade", Enum.GetName(typeof(Enums.CardRarity), this.cardData.CardRarity));
				num += num10;
			}
			for (int num11 = 0; num11 < list2.Count; num11++)
			{
				if (!craftTimes.ContainsKey(list2[num11]))
				{
					craftTimes.Add(list2[num11], 1);
				}
				else
				{
					craftTimes[list2[num11]]++;
				}
			}
			string text = "";
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
			foreach (KeyValuePair<string, int> craftTime in craftTimes)
			{
				text = Functions.GetCardDataFromCardData(Globals.Instance.GetCardData(craftTime.Key, instantiate: false), "").Id;
				if (!dictionary2.ContainsKey(text))
				{
					dictionary2.Add(text, craftTime.Value);
				}
				else
				{
					dictionary2[text] += craftTime.Value;
				}
			}
			foreach (KeyValuePair<string, int> item in dictionary2)
			{
				if (!PlayerManager.Instance.IsCardUnlocked(item.Key))
				{
					StartCoroutine(LockTimeout(item.Key));
					flag = false;
					continue;
				}
				if (!CanCraftThisCard(Globals.Instance.GetCardData(item.Key, instantiate: false)))
				{
					StartCoroutine(LockTimeout(item.Key));
					flag = false;
					continue;
				}
				bool flag2 = false;
				if (SandboxManager.Instance.IsEnabled() && AtOManager.Instance.Sandbox_unlimitedAvailableCards)
				{
					flag2 = true;
				}
				if (flag2)
				{
					continue;
				}
				int[] cardAvailability = GetCardAvailability(item.Key, itemListId);
				if (cardAvailability[0] + item.Value > cardAvailability[1])
				{
					flag = false;
					int num12 = 0;
					if (dictionary.ContainsKey(item.Key))
					{
						num12 = dictionary[item.Key];
					}
					StartCoroutine(LockTimeout(item.Key, cardAvailability[1] - cardAvailability[0] + num12));
				}
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		if ((flag && deckAvailableForSaveLoad) || AtOManager.Instance.IsCombatTool)
		{
			if (GameManager.Instance.IsSingularity())
			{
				num = 0;
				num2 = 0;
			}
			stringBuilder.Append(Texts.Instance.GetText("craftWillCost"));
			stringBuilder.Append("<br><size=+1>");
			stringBuilder.Append("<sprite name=gold> ");
			stringBuilder.Append(num2);
			stringBuilder.Append("  ");
			stringBuilder.Append("<sprite name=dust> ");
			stringBuilder.Append(num);
			deckCraftPrice.text = stringBuilder.ToString();
			deckCraftingCostGold = num2;
			deckCraftingCostDust = num;
			int playerDust = AtOManager.Instance.GetPlayerDust();
			int playerGold = AtOManager.Instance.GetPlayerGold();
			if ((playerDust >= num && playerGold >= num2) || AtOManager.Instance.IsCombatTool)
			{
				botCraftingDeck.Enable();
			}
			else
			{
				botCraftingDeck.Disable();
			}
		}
		else
		{
			stringBuilder.Append("<sprite name=lock> ");
			stringBuilder.Append(Texts.Instance.GetText("craftCant"));
			deckCraftPrice.text = stringBuilder.ToString();
			botCraftingDeck.Disable();
		}
		stringBuilder = null;
	}

	private IEnumerator LockTimeout(string name, int index = 0)
	{
		yield return Globals.Instance.WaitForSeconds(0.05f);
		int num = 0;
		foreach (Transform item in loadDeckCardContainer)
		{
			if (item.gameObject.name == name)
			{
				if (num >= index)
				{
					item.GetComponent<CardVertical>().ShowLock(state: true);
				}
				num++;
			}
		}
		if (num != 0)
		{
			yield break;
		}
		foreach (Transform item2 in loadDeckCardContainer)
		{
			if (item2.gameObject.name == name + "a" || item2.gameObject.name == name + "b")
			{
				if (num >= index)
				{
					item2.GetComponent<CardVertical>().ShowLock(state: true);
				}
				num++;
			}
		}
	}

	public void CraftDeck()
	{
		AtOManager.Instance.PayGold(deckCraftingCostGold);
		AtOManager.Instance.PayDust(deckCraftingCostDust);
		string sourceName = AtOManager.Instance.GetHero(heroIndex).SourceName;
		List<string> list = PlayerManager.Instance.PlayerSavedDeck.DeckCards[sourceName][savingSlot];
		List<string> cards = AtOManager.Instance.GetHero(heroIndex).Cards;
		List<string> list2 = new List<string>();
		for (int i = 0; i < cards.Count; i++)
		{
			CardData cardData = Globals.Instance.GetCardData(cards[i], instantiate: false);
			if (cardData.CardClass == Enums.CardClass.Injury || cardData.CardClass == Enums.CardClass.Boon)
			{
				list2.Add(cards[i]);
			}
		}
		AtOManager.Instance.GetHero(heroIndex).Cards = new List<string>();
		for (int j = 0; j < list.Count; j++)
		{
			CardData cardData2 = Globals.Instance.GetCardData(list[j], instantiate: false);
			if (GameManager.Instance.IsSingularity() && cardData2.CardUpgraded != Enums.CardUpgraded.No && !cardData2.Starter)
			{
				list[j] = cardData2.BaseCard.ToLower();
			}
			list2.Add(list[j]);
		}
		AtOManager.Instance.GetHero(heroIndex).Cards = list2;
		loadDeckContainer.gameObject.SetActive(value: false);
		CreateDeck(heroIndex);
		AtOManager.Instance.SideBarRefreshCards(heroIndex);
		foreach (KeyValuePair<string, int> craftTime in craftTimes)
		{
			for (int k = 0; k < craftTime.Value; k++)
			{
				CardData cardDataFromCardData = Functions.GetCardDataFromCardData(Globals.Instance.GetCardData(craftTime.Key, instantiate: false), "");
				AtOManager.Instance.SaveCraftedCard(heroIndex, cardDataFromCardData.Id);
			}
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			AtOManager.Instance.AddDeckToHeroMP(heroIndex, list2);
		}
	}

	public int[] GetCardAvailability(string cardId, string shopId)
	{
		CardData cardData = Globals.Instance.GetCardData(cardId, instantiate: false);
		int num = 0;
		int num2 = 0;
		CardData cardData2 = null;
		if (cardData.CardUpgraded != Enums.CardUpgraded.No && cardData.UpgradedFrom != "")
		{
			num2 = AtOManager.Instance.HowManyCrafted(heroIndex, cardData.UpgradedFrom.ToLower());
			cardData2 = Globals.Instance.GetCardData(cardData.UpgradedFrom.ToLower(), instantiate: false);
		}
		else
		{
			num2 = AtOManager.Instance.HowManyCrafted(heroIndex, cardId);
			cardData2 = cardData;
		}
		if (GameManager.Instance.IsSingularity() && cardData2.CardClass != Enums.CardClass.Item)
		{
			Hero hero = AtOManager.Instance.GetHero(heroIndex);
			num2 = (Functions.ListHaveThisBaseCard(cardId, hero.Cards) ? 1 : 0);
			num = 1;
		}
		else if (cardData2.CardClass == Enums.CardClass.Item)
		{
			num = ((cardData2.CardType != Enums.CardType.Pet) ? 1 : (PlayerManager.Instance.IsCardUnlocked(cardData2.Id) ? 1 : 0));
			if (AtOManager.Instance.ItemBoughtOnThisShop(shopId, cardId) || (cardData2.CardType == Enums.CardType.Pet && AtOManager.Instance.TeamHaveItem(cardId, 5, _checkRareToo: true)))
			{
				num2 = 1;
			}
		}
		else if (cardData2.CardRarity == Enums.CardRarity.Common)
		{
			num = 1;
			if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_1_1"))
			{
				num = 2;
			}
		}
		else if (cardData2.CardRarity == Enums.CardRarity.Uncommon)
		{
			num = 1;
			if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_1_3") && AtOManager.Instance.GetNgPlus() < 5)
			{
				num = 2;
			}
		}
		else
		{
			num = 1;
		}
		if (num2 > num)
		{
			num2 = num;
		}
		return new int[2] { num2, num };
	}

	public void RemoveDeck(int slot)
	{
		savingSlot = slot;
		AlertManager.buttonClickDelegate = RemoveDeckAction;
		AlertManager.Instance.AlertConfirmDouble(Texts.Instance.GetText("savedDeckDeleteConfirm"));
	}

	public void RemoveDeckAction()
	{
		bool confirmAnswer = AlertManager.Instance.GetConfirmAnswer();
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(RemoveDeckAction));
		if (confirmAnswer)
		{
			string sourceName = AtOManager.Instance.GetHero(heroIndex).SourceName;
			PlayerManager.Instance.PlayerSavedDeck.DeckTitle[sourceName][savingSlot] = "";
			PlayerManager.Instance.PlayerSavedDeck.DeckCards[sourceName][savingSlot] = new List<string>();
			SaveManager.SavePlayerDeck();
			LoadDecks();
		}
	}

	public void SetWaitingPlayersText(string msg)
	{
		if (msg != "")
		{
			waitingMsg.gameObject.SetActive(value: true);
			waitingMsgText.text = msg;
		}
		else
		{
			waitingMsg.gameObject.SetActive(value: false);
		}
	}

	public void Ready()
	{
		if (!GameManager.Instance.IsMultiplayer() || TownManager.Instance != null)
		{
			ExitCardCraft();
			return;
		}
		if (manualReadyCo != null)
		{
			StopCoroutine(manualReadyCo);
		}
		statusReady = !statusReady;
		NetworkManager.Instance.SetManualReady(statusReady);
		if (statusReady)
		{
			exitCraftButton.GetComponent<BotonGeneric>().SetBackgroundColor(Functions.HexToColor(Globals.Instance.ClassColor["scout"]));
			exitCraftButton.GetComponent<BotonGeneric>().SetText(Texts.Instance.GetText("waitingForPlayers"));
			if (NetworkManager.Instance.IsMaster())
			{
				manualReadyCo = StartCoroutine(CheckForAllManualReady());
			}
		}
		else
		{
			exitCraftButton.GetComponent<BotonGeneric>().SetBackgroundColor(Functions.HexToColor(Globals.Instance.ClassColor["warrior"]));
			exitCraftButton.GetComponent<BotonGeneric>().SetText(Texts.Instance.GetText("exit"));
		}
	}

	private IEnumerator CheckForAllManualReady()
	{
		bool check = true;
		while (check)
		{
			if (!NetworkManager.Instance.AllPlayersManualReady())
			{
				yield return Globals.Instance.WaitForSeconds(1f);
			}
			else
			{
				check = false;
			}
		}
		photonView.RPC("NET_CloseCardCraft", RpcTarget.Others);
		ExitCardCraft();
	}

	private void ActivateButtonCorruption()
	{
		if (CICorruption == null)
		{
			BG_Corruption.Disable();
		}
		else if (CanBuy("Corruption"))
		{
			BG_Corruption.Enable();
		}
		else
		{
			BG_Corruption.Disable();
		}
	}

	public void InitCorruption()
	{
		RemoveCorruptionCards();
		DeactivateCorruptions();
		ActivateButtonCorruption();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<align=center><size=24>");
		stringBuilder.Append(string.Format(Texts.Instance.GetText("corruptionSacrifyDust"), 400, 10));
		corruptionButtons[0].SetPopupText(stringBuilder.ToString());
		stringBuilder.Clear();
		stringBuilder.Append("<sprite name=dust> -");
		stringBuilder.Append(400);
		corruptionButtons[0].SetText(stringBuilder.ToString());
		stringBuilder.Clear();
		stringBuilder.Append("<align=center><size=24>");
		stringBuilder.Append(string.Format(Texts.Instance.GetText("corruptionSacrifySpeed"), 2, 10));
		corruptionButtons[1].SetPopupText(stringBuilder.ToString());
		stringBuilder.Clear();
		stringBuilder.Append("<sprite name=speedMini> -");
		stringBuilder.Append(2);
		corruptionButtons[1].SetText(stringBuilder.ToString());
		stringBuilder.Clear();
		stringBuilder.Append("<align=center><size=24>");
		stringBuilder.Append(string.Format(Texts.Instance.GetText("corruptionSacrifyHealth"), 20, 10));
		corruptionButtons[2].SetPopupText(stringBuilder.ToString());
		stringBuilder.Clear();
		stringBuilder.Append("<sprite name=heart> -");
		stringBuilder.Append(20);
		corruptionButtons[2].SetText(stringBuilder.ToString());
		stringBuilder.Clear();
		stringBuilder.Append("<align=center><size=24>");
		stringBuilder.Append(string.Format(Texts.Instance.GetText("corruptionSacrifyResistance"), 10, 10));
		corruptionButtons[3].SetPopupText(stringBuilder.ToString());
		stringBuilder.Clear();
		stringBuilder.Append("<sprite name=ui_resistance> -");
		stringBuilder.Append(10);
		corruptionButtons[3].SetText(stringBuilder.ToString());
		stringBuilder = null;
	}

	private void CheckForCorruptableCards(CardVertical _cardVertical)
	{
		if (_cardVertical.cardData.CardClass == Enums.CardClass.Injury || _cardVertical.cardData.CardClass == Enums.CardClass.Boon || _cardVertical.cardData.CardUpgraded == Enums.CardUpgraded.Rare || _cardVertical.cardData.CardClass == Enums.CardClass.Special)
		{
			_cardVertical.ShowLock(state: true, paintItBlack: false);
		}
		else
		{
			_cardVertical.ShowLock(state: false);
		}
	}

	private void DeactivateCorruptions()
	{
		for (int i = 0; i < 4; i++)
		{
			ActivateCorruption(i, _state: false);
		}
		int begin = int.Parse(corruptionPercent.text.Replace("%", ""));
		StartCoroutine(IncrementCorruption(begin, CalculateCorruption()));
	}

	public void DoButtonCorruption(int _corruptionNum)
	{
		if (IsCorruptionEnabled(_corruptionNum))
		{
			ActivateCorruption(_corruptionNum, _state: false);
		}
		else
		{
			ActivateCorruption(_corruptionNum);
		}
		int begin = int.Parse(corruptionPercent.text.Replace("%", ""));
		StartCoroutine(IncrementCorruption(begin, CalculateCorruption()));
	}

	public void ActivateCorruption(int _corruptionNum, bool _state = true)
	{
		if (!_state || !(CICorruption == null))
		{
			corruptionArrows[_corruptionNum].gameObject.SetActive(_state);
			Color color = corruptionButtons[_corruptionNum].color;
			if (_state)
			{
				Color color2 = new Color(color.r, color.g, color.b, 1f);
				corruptionTexts[_corruptionNum].color = color2;
				corruptionButtons[_corruptionNum].SetColorAbsolute(color2);
			}
			else
			{
				Color color3 = new Color(color.r, color.g, color.b, 0.3f);
				corruptionTexts[_corruptionNum].color = color3;
				corruptionButtons[_corruptionNum].SetColorAbsolute(color3);
			}
			BG_Corruption.SetText(ButtonText(SetPrice("Corruption", "")));
			ActivateButtonCorruption();
		}
	}

	private bool IsCorruptionEnabled(int _index)
	{
		if (corruptionArrows[_index].gameObject.activeSelf)
		{
			return true;
		}
		return false;
	}

	public int CalculateCorruption()
	{
		if (corruptionTry[heroIndex] > 2)
		{
			return 0;
		}
		int num = Globals.Instance.CorruptionBasePercent[corruptionTry[heroIndex]];
		corruptionValue = num;
		for (int i = 0; i < 4; i++)
		{
			if (IsCorruptionEnabled(i))
			{
				corruptionValue += 10;
			}
		}
		if (corruptionValue > 100)
		{
			corruptionValue = 100;
		}
		else if (corruptionValue < 0)
		{
			corruptionValue = 0;
		}
		return corruptionValue;
	}

	private IEnumerator IncrementCorruption(int _begin, int _end)
	{
		if (_begin != _end)
		{
			bool goingUp = _begin < _end;
			int value = _begin;
			while (goingUp && value < _end)
			{
				value += 2;
				corruptionPercent.text = value + "%";
				yield return null;
			}
			while (!goingUp && value > _end)
			{
				value -= 2;
				corruptionPercent.text = value + "%";
				yield return null;
			}
			corruptionPercent.text = _end + "%";
		}
	}

	private IEnumerator CorruptionRollForChance(int _chanceRolled, int _chance)
	{
		for (int i = 0; i < 100; i++)
		{
			int num = UnityEngine.Random.Range(0, 100);
			if (num < 10)
			{
				corruptionPercentRoll.text = "0" + num + "%";
			}
			else
			{
				corruptionPercentRoll.text = num + "%";
			}
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
		corruptionPercentRoll.text = _chanceRolled + "%";
		if (_chanceRolled <= _chance)
		{
			corruptionPercentRollSuccess.gameObject.SetActive(value: true);
		}
		else
		{
			corruptionPercentRollFail.gameObject.SetActive(value: true);
		}
	}

	public void CorruptCard()
	{
		if (CanBuy("Corruption"))
		{
			if (craftType == 6)
			{
				craftCoroutine = StartCoroutine(BuyCorruptionCo());
			}
			else if (craftType == 7)
			{
				craftCoroutine = StartCoroutine(BuyItemCorruptionCo());
			}
		}
	}

	private IEnumerator BuyCorruptionCo()
	{
		if (blocked || CICorruption == null)
		{
			yield break;
		}
		SetBlocked(_status: true);
		BG_Corruption.Disable();
		int cardIndex = int.Parse(CICorruption.transform.gameObject.name.Split('_')[2]);
		AtOManager.Instance.PayDust(SetPrice("Corruption", ""));
		AtOManager.Instance.SubstractCraftReaminingUses(heroIndex);
		if (IsCorruptionEnabled(1))
		{
			AtOManager.Instance.AddPerkToHeroGlobal(heroIndex, "perkcorruptionspeed" + corruptionTry[heroIndex]);
		}
		if (IsCorruptionEnabled(2))
		{
			AtOManager.Instance.AddPerkToHeroGlobal(heroIndex, "perkcorruptionhealth" + corruptionTry[heroIndex]);
		}
		if (IsCorruptionEnabled(3))
		{
			AtOManager.Instance.AddPerkToHeroGlobal(heroIndex, "perkcorruptionresist" + corruptionTry[heroIndex]);
		}
		int num = CalculateCorruption();
		bool success = false;
		if (num < 100)
		{
			int num2 = UnityEngine.Random.Range(0, 100);
			StartCoroutine(CorruptionRollForChance(num2, num));
			if (num2 <= num)
			{
				success = true;
			}
			yield return Globals.Instance.WaitForSeconds(2.5f);
		}
		else
		{
			success = true;
		}
		CardData cardDataAux;
		if (success)
		{
			cardDataAux = Functions.GetCardDataFromCardData(cardData, "RARE");
		}
		else
		{
			cardDataAux = Globals.Instance.GetCardData("voidmemory");
			PlayerManager.Instance.CardUnlock("voidmemory");
		}
		AtOManager.Instance.ReplaceCardInDeck(heroIndex, cardIndex, cardDataAux.Id);
		corruptionTry[heroIndex]++;
		ShowRemainingUses();
		SetControllerIntoVerticalList();
		AtOManager.Instance.HeroCraftUpgraded(heroIndex);
		AtOManager.Instance.SideBarRefreshCards(heroIndex);
		CICorruption.cardrevealed = true;
		CICorruption.EnableTrail();
		CICorruption.TopLayeringOrder("UI", -2000);
		CICorruption.PlayDissolveParticle();
		CICorruption.SetDestinationScaleRotation(deckGOs[cardIndex].transform.position + new Vector3(0f, -1f, 0f), 0f, Quaternion.Euler(0f, 0f, 180f));
		yield return Globals.Instance.WaitForSeconds(0.2f);
		cardVerticalDeck[cardIndex].ReplaceWithCard(cardDataAux, "RARE");
		cardVerticalDeck[cardIndex].ShowLock(state: true, paintItBlack: false);
		corruptAnim.SetTrigger("hide");
		RemoveCorruptionCards();
		ActivateButtonCorruption();
		DeactivateCorruptions();
		SetBlocked(_status: false);
	}

	private IEnumerator BuyItemCorruptionCo()
	{
		if (blocked || CICorruption == null)
		{
			yield break;
		}
		SetBlocked(_status: true);
		BG_Corruption.Disable();
		int cardIndex = int.Parse(CICorruption.transform.gameObject.name.Split('_')[2]);
		AtOManager.Instance.PayDust(SetPrice("Corruption", ""));
		AtOManager.Instance.SubstractCraftReaminingUses(heroIndex);
		if (IsCorruptionEnabled(1))
		{
			AtOManager.Instance.AddPerkToHeroGlobal(heroIndex, "perkcorruptionspeed" + corruptionTry[heroIndex]);
		}
		if (IsCorruptionEnabled(2))
		{
			AtOManager.Instance.AddPerkToHeroGlobal(heroIndex, "perkcorruptionhealth" + corruptionTry[heroIndex]);
		}
		if (IsCorruptionEnabled(3))
		{
			AtOManager.Instance.AddPerkToHeroGlobal(heroIndex, "perkcorruptionresist" + corruptionTry[heroIndex]);
		}
		int num = CalculateCorruption();
		CardData cardDataAux = null;
		bool success = false;
		if (num < 100)
		{
			int num2 = UnityEngine.Random.Range(0, 100);
			StartCoroutine(CorruptionRollForChance(num2, num));
			if (num2 <= num)
			{
				success = true;
			}
			yield return Globals.Instance.WaitForSeconds(2.5f);
		}
		else
		{
			success = true;
		}
		if (success)
		{
			cardDataAux = Functions.GetCardDataFromCardData(cardData, "RARE");
			ItemCorruptionIcons[cardIndex].ShowRareParticles();
			if (GameManager.Instance.IsMultiplayer())
			{
				AtOManager.Instance.AddItemToHeroMP(heroIndex, cardDataAux.Id);
			}
			else
			{
				AtOManager.Instance.AddItemToHero(heroIndex, cardDataAux.Id);
			}
		}
		else
		{
			switch (cardData.CardType)
			{
			case Enums.CardType.Weapon:
				cardDataAux = Globals.Instance.GetCardData("brokenitem");
				break;
			case Enums.CardType.Armor:
				cardDataAux = Globals.Instance.GetCardData("burneditem");
				break;
			case Enums.CardType.Jewelry:
				cardDataAux = Globals.Instance.GetCardData("deformeditem");
				break;
			case Enums.CardType.Accesory:
				cardDataAux = Globals.Instance.GetCardData("uselessitem");
				break;
			}
			if (cardDataAux != null)
			{
				if (GameManager.Instance.IsMultiplayer())
				{
					AtOManager.Instance.AddItemToHeroMP(heroIndex, cardDataAux.Id);
				}
				else
				{
					AtOManager.Instance.AddItemToHero(heroIndex, cardDataAux.Id);
				}
			}
			else if (GameManager.Instance.IsMultiplayer())
			{
				AtOManager.Instance.RemoveItemFromHeroMP(_isHero: true, heroIndex, cardData.CardType.ToString().ToLower());
			}
			else
			{
				AtOManager.Instance.RemoveItemFromHero(_isHero: true, heroIndex, cardData.CardType.ToString().ToLower());
			}
			ShowCharacterCorruptionItems();
		}
		AssignCorruptionItemSlots();
		corruptionTry[heroIndex]++;
		ShowRemainingUses();
		if (CICorruption != null)
		{
			CICorruption.cardrevealed = true;
			CICorruption.EnableTrail();
			CICorruption.TopLayeringOrder("UI", -2000);
			CICorruption.PlayDissolveParticle();
		}
		yield return Globals.Instance.WaitForSeconds(0.2f);
		corruptAnim.SetTrigger("hide");
		RemoveCorruptionCards();
		ActivateButtonCorruption();
		DeactivateCorruptions();
		SetBlocked(_status: false);
	}

	private void SetControllerIntoVerticalList()
	{
		if (Gamepad.current != null && controllerHorizontalIndex != -1)
		{
			controllerHorizontalIndex = Functions.GetTransformIndexInList(_controllerList, cardListContainer.gameObject.name);
			controllerIsOnVerticalList = true;
			ControllerMovement(goingUp: true, goingRight: false, goingDown: false, goingLeft: false, controllerVerticalIndex);
		}
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false, int absolutePosition = -1)
	{
		if ((goingUp || goingDown) && controllerIsOnVerticalList)
		{
			if (!(cardListContainer != null))
			{
				return;
			}
			int childCount = cardListContainer.childCount;
			if (childCount > 0)
			{
				bool flag = false;
				int num = 0;
				while (!flag && num < 20)
				{
					if (absolutePosition > -1)
					{
						controllerVerticalIndex = absolutePosition;
					}
					else if (goingUp)
					{
						controllerVerticalIndex--;
						if (controllerVerticalIndex < 0)
						{
							controllerVerticalIndex = 0;
						}
					}
					else
					{
						controllerVerticalIndex++;
						if (controllerVerticalIndex > childCount - 1)
						{
							controllerVerticalIndex = childCount - 1;
						}
					}
					if (cardListContainer.childCount > controllerVerticalIndex && cardListContainer.GetChild(controllerVerticalIndex) != null && !cardListContainer.GetChild(controllerVerticalIndex).GetComponent<CardVertical>().IsLocked())
					{
						flag = true;
					}
					else
					{
						num++;
					}
				}
				Canvas.ForceUpdateCanvases();
				if (childCount > 15)
				{
					if (controllerVerticalIndex > 8)
					{
						auxVector.x = 0.5f;
						auxVector.y = -3.66f + 0.0025f * (float)controllerVerticalIndex;
						if (cardListContainerRectTransform.GetChild(0) != null && cardListContainer.GetChild(controllerVerticalIndex) != null)
						{
							cardListContainer.GetComponent<RectTransform>().anchoredPosition = (Vector2)cardListContainerRectTransform.GetChild(0).transform.InverseTransformPoint(cardListContainer.position + auxVector) - (Vector2)cardListContainerRectTransform.GetChild(0).transform.InverseTransformPoint(new Vector3(-4.9f, cardListContainer.GetChild(controllerVerticalIndex).position.y, 0f));
						}
					}
					else
					{
						auxVector.x = 0.5f;
						auxVector.y = -3f - (float)(childCount - 15) * 0.2f;
						cardListContainer.GetComponent<RectTransform>().anchoredPosition = auxVector;
					}
				}
				else
				{
					auxVector.x = 0.5f;
					auxVector.y = -3f;
					cardListContainer.GetComponent<RectTransform>().anchoredPosition = auxVector;
				}
				auxVector.x = 1f;
				auxVector.y = 0f;
				if (cardListContainer.GetChild(controllerVerticalIndex) != null)
				{
					warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(cardListContainer.GetChild(controllerVerticalIndex).position + auxVector);
					Mouse.current.WarpCursorPosition(warpPosition);
				}
			}
			if (craftType != 2)
			{
				controllerHorizontalIndex = 0;
			}
			return;
		}
		controllerIsOnVerticalList = false;
		_controllerList.Clear();
		if (craftType == 0)
		{
			_controllerList.Add(cardListContainer);
			if (Functions.TransformIsVisible(buttonL))
			{
				_controllerList.Add(buttonL);
			}
			if (Functions.TransformIsVisible(buttonR))
			{
				_controllerList.Add(buttonR);
			}
			_controllerList.Add(exitCraftButton);
			for (int i = 0; i < 4; i++)
			{
				if ((bool)TownManager.Instance)
				{
					if (Functions.TransformIsVisible(TownManager.Instance.sideCharacters.charArray[i].transform))
					{
						_controllerList.Add(TownManager.Instance.sideCharacters.charArray[i].transform.GetChild(0).transform);
					}
				}
				else if (Functions.TransformIsVisible(MapManager.Instance.sideCharacters.charArray[i].transform))
				{
					_controllerList.Add(MapManager.Instance.sideCharacters.charArray[i].transform.GetChild(0).transform);
				}
			}
			if (Functions.TransformIsVisible(PlayerUIManager.Instance.giveGold))
			{
				_controllerList.Add(PlayerUIManager.Instance.giveGold);
			}
			if (controllerHorizontalIndex == -1 || (controllerHorizontalIndex < _controllerList.Count && _controllerList[controllerHorizontalIndex] != cardListContainer))
			{
				controllerHorizontalIndex = Functions.GetListClosestIndexToMousePosition(_controllerList);
			}
			controllerHorizontalIndex = Functions.GetClosestIndexBasedOnDirection(_controllerList, controllerHorizontalIndex, goingUp, goingRight, goingDown, goingLeft);
			if (_controllerList[controllerHorizontalIndex] == cardListContainer)
			{
				controllerIsOnVerticalList = true;
				ControllerMovement(goingUp: true, goingRight: false, goingDown: false, goingLeft: false, controllerVerticalIndex);
			}
			else
			{
				warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerList[controllerHorizontalIndex].position);
				Mouse.current.WarpCursorPosition(warpPosition);
			}
		}
		else if (craftType == 1)
		{
			_controllerList.Add(cardListContainer);
			if (Functions.TransformIsVisible(buttonRemove))
			{
				_controllerList.Add(buttonRemove);
			}
			_controllerList.Add(exitCraftButton);
			for (int j = 0; j < 4; j++)
			{
				if ((bool)TownManager.Instance)
				{
					if (Functions.TransformIsVisible(TownManager.Instance.sideCharacters.charArray[j].transform))
					{
						_controllerList.Add(TownManager.Instance.sideCharacters.charArray[j].transform.GetChild(0).transform);
					}
				}
				else if (Functions.TransformIsVisible(MapManager.Instance.sideCharacters.charArray[j].transform))
				{
					_controllerList.Add(MapManager.Instance.sideCharacters.charArray[j].transform.GetChild(0).transform);
				}
			}
			if (Functions.TransformIsVisible(PlayerUIManager.Instance.giveGold))
			{
				_controllerList.Add(PlayerUIManager.Instance.giveGold);
			}
			if (controllerHorizontalIndex == -1 || _controllerList[controllerHorizontalIndex] != cardListContainer)
			{
				controllerHorizontalIndex = Functions.GetListClosestIndexToMousePosition(_controllerList);
			}
			controllerHorizontalIndex = Functions.GetClosestIndexBasedOnDirection(_controllerList, controllerHorizontalIndex, goingUp, goingRight, goingDown, goingLeft);
			if (_controllerList[controllerHorizontalIndex] == cardListContainer)
			{
				controllerIsOnVerticalList = true;
				ControllerMovement(goingUp: true, goingRight: false, goingDown: false, goingLeft: false, controllerVerticalIndex);
			}
			else
			{
				warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerList[controllerHorizontalIndex].position);
				Mouse.current.WarpCursorPosition(warpPosition);
			}
		}
		else if (craftType == 2)
		{
			if (filterWindow.gameObject.activeSelf)
			{
				for (int k = 0; k < craftBotonFilters.Count; k++)
				{
					_controllerList.Add(craftBotonFilters[k].transform);
				}
				_controllerList.Add(filterWindow.GetChild(8));
				_controllerList.Add(filterWindow.GetChild(7));
				_controllerList.Add(filterWindow.GetChild(6));
				controllerVerticalIndex = Functions.GetListClosestIndexToMousePosition(_controllerList);
				controllerVerticalIndex = Functions.GetClosestIndexBasedOnDirection(_controllerList, controllerVerticalIndex, goingUp, goingRight, goingDown, goingLeft);
				if (_controllerList[controllerVerticalIndex] != null)
				{
					warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerList[controllerVerticalIndex].position);
					Mouse.current.WarpCursorPosition(warpPosition);
				}
				return;
			}
			if (!Functions.TransformIsVisible(cardCraftSave.transform))
			{
				foreach (Transform item4 in cardCraftContainer)
				{
					if ((bool)item4.GetComponent<CardCraftItem>() && Functions.TransformIsVisible(item4))
					{
						_controllerList.Add(item4.GetChild(1).transform.GetChild(0).transform);
						_controllerList.Add(item4.GetChild(2).transform);
					}
				}
				foreach (Transform item5 in cardCraftEnergySelectorContainer)
				{
					if ((bool)item5.GetComponent<CardCraftSelectorEnergy>())
					{
						_controllerList.Add(item5);
					}
				}
				foreach (Transform item6 in cardCraftRaritySelectorContainer)
				{
					if ((bool)item6.GetComponent<CardCraftSelectorRarity>())
					{
						_controllerList.Add(item6);
					}
				}
				_controllerList.Add(buttonAffordableCraft.transform);
				_controllerList.Add(buttonAdvancedCraft.transform);
				_controllerList.Add(buttonFilterCraft.transform);
				foreach (Transform item7 in cardCraftPageContainer)
				{
					_controllerList.Add(item7);
				}
				_controllerList.Add(searchInput.transform);
				if (Functions.TransformIsVisible(canvasSearchCloseT))
				{
					_controllerList.Add(canvasSearchCloseT);
				}
			}
			else
			{
				for (int l = 0; l < deckSlot.Length; l++)
				{
					if (deckSlot[l].transform.GetComponent<BoxCollider2D>().enabled)
					{
						_controllerList.Add(deckSlot[l].transform);
						_controllerList.Add(deckSlot[l].transform.GetChild(5).transform);
					}
					else
					{
						_controllerList.Add(deckSlot[l].transform.GetChild(4).transform);
					}
				}
			}
			_controllerList.Add(botSaveLoad.transform);
			_controllerList.Add(exitCraftButton);
			_controllerList.Add(cardListContainer);
			for (int m = 0; m < 4; m++)
			{
				if ((bool)TownManager.Instance)
				{
					if (Functions.TransformIsVisible(TownManager.Instance.sideCharacters.charArray[m].transform))
					{
						_controllerList.Add(TownManager.Instance.sideCharacters.charArray[m].transform.GetChild(0).transform);
					}
				}
				else if (Functions.TransformIsVisible(MapManager.Instance.sideCharacters.charArray[m].transform))
				{
					_controllerList.Add(MapManager.Instance.sideCharacters.charArray[m].transform.GetChild(0).transform);
				}
			}
			if (Functions.TransformIsVisible(PlayerUIManager.Instance.giveGold))
			{
				_controllerList.Add(PlayerUIManager.Instance.giveGold);
			}
			if (controllerHorizontalIndex == -1 || controllerHorizontalIndex >= _controllerList.Count || _controllerList[controllerHorizontalIndex] != cardListContainer)
			{
				controllerHorizontalIndex = Functions.GetListClosestIndexToMousePosition(_controllerList);
			}
			controllerHorizontalIndex = Functions.GetClosestIndexBasedOnDirection(_controllerList, controllerHorizontalIndex, goingUp, goingRight, goingDown, goingLeft);
			if (_controllerList[controllerHorizontalIndex] == cardListContainer)
			{
				controllerIsOnVerticalList = true;
				ControllerMovement(goingUp: true, goingRight: false, goingDown: false, goingLeft: false, controllerVerticalIndex);
			}
			else
			{
				warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerList[controllerHorizontalIndex].position);
				Mouse.current.WarpCursorPosition(warpPosition);
			}
		}
		else if (craftType == 3)
		{
			foreach (Transform item8 in divinationButtonContainer)
			{
				if (Functions.TransformIsVisible(item8))
				{
					_controllerList.Add(item8);
				}
			}
			_controllerList.Add(exitCraftButton);
			controllerHorizontalIndex = Functions.GetListClosestIndexToMousePosition(_controllerList);
			controllerHorizontalIndex = Functions.GetClosestIndexBasedOnDirection(_controllerList, controllerHorizontalIndex, goingUp, goingRight, goingDown, goingLeft);
			if (_controllerList[controllerHorizontalIndex] != null)
			{
				warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerList[controllerHorizontalIndex].position);
				Mouse.current.WarpCursorPosition(warpPosition);
			}
		}
		else if (craftType == 4)
		{
			foreach (Transform item9 in cardItemContainer)
			{
				if (Functions.TransformIsVisible(item9) && (bool)item9.GetComponent<CardCraftItem>())
				{
					_controllerList.Add(item9.GetChild(1).transform.GetChild(0).transform);
					_controllerList.Add(item9.GetChild(3).transform);
				}
			}
			_controllerList.Add(exitCraftButton);
			if (Functions.TransformIsVisible(itemShopButton.transform))
			{
				_controllerList.Add(itemShopButton.transform);
			}
			if (Functions.TransformIsVisible(petShopButton.transform))
			{
				_controllerList.Add(petShopButton.transform);
			}
			if (Functions.TransformIsVisible(rerollButton.transform))
			{
				_controllerList.Add(rerollButton.transform);
			}
			if (iconWeapon.transform.GetChild(0).gameObject.activeSelf)
			{
				_controllerList.Add(iconWeapon.transform);
			}
			if (iconArmor.transform.GetChild(0).gameObject.activeSelf)
			{
				_controllerList.Add(iconArmor.transform);
			}
			if (iconJewelry.transform.GetChild(0).gameObject.activeSelf)
			{
				_controllerList.Add(iconJewelry.transform);
			}
			if (iconAccesory.transform.GetChild(0).gameObject.activeSelf)
			{
				_controllerList.Add(iconAccesory.transform);
			}
			if (iconPet.transform.GetChild(0).gameObject.activeSelf)
			{
				_controllerList.Add(iconPet.transform);
			}
			if (Functions.TransformIsVisible(shadyDealButton.transform))
			{
				_controllerList.Add(shadyDealButton.transform);
			}
			for (int n = 0; n < 4; n++)
			{
				if ((bool)TownManager.Instance)
				{
					if (Functions.TransformIsVisible(TownManager.Instance.sideCharacters.charArray[n].transform))
					{
						_controllerList.Add(TownManager.Instance.sideCharacters.charArray[n].transform.GetChild(0).transform);
					}
				}
				else if (Functions.TransformIsVisible(MapManager.Instance.sideCharacters.charArray[n].transform))
				{
					_controllerList.Add(MapManager.Instance.sideCharacters.charArray[n].transform.GetChild(0).transform);
				}
			}
			if (Functions.TransformIsVisible(PlayerUIManager.Instance.giveGold))
			{
				_controllerList.Add(PlayerUIManager.Instance.giveGold);
			}
			foreach (Transform item10 in itemsCraftPageContainer)
			{
				_controllerList.Add(item10);
			}
			controllerHorizontalIndex = Functions.GetListClosestIndexToMousePosition(_controllerList);
			controllerHorizontalIndex = Functions.GetClosestIndexBasedOnDirection(_controllerList, controllerHorizontalIndex, goingUp, goingRight, goingDown, goingLeft);
			if (_controllerList[controllerHorizontalIndex] != null)
			{
				warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerList[controllerHorizontalIndex].position);
				Mouse.current.WarpCursorPosition(warpPosition);
			}
		}
		else if (craftType == 6)
		{
			_controllerList.Add(buttonCorruption);
			for (int num2 = 0; num2 < 4; num2++)
			{
				_controllerList.Add(corruptionButtons[num2].transform);
			}
			_controllerList.Add(corruptionCharacterStats);
			_controllerList.Add(cardListContainer);
			_controllerList.Add(exitCraftButton);
			for (int num3 = 0; num3 < 4; num3++)
			{
				if ((bool)TownManager.Instance)
				{
					if (Functions.TransformIsVisible(TownManager.Instance.sideCharacters.charArray[num3].transform))
					{
						_controllerList.Add(TownManager.Instance.sideCharacters.charArray[num3].transform.GetChild(0).transform);
					}
				}
				else if (Functions.TransformIsVisible(MapManager.Instance.sideCharacters.charArray[num3].transform))
				{
					_controllerList.Add(MapManager.Instance.sideCharacters.charArray[num3].transform.GetChild(0).transform);
				}
			}
			if (Functions.TransformIsVisible(PlayerUIManager.Instance.giveGold))
			{
				_controllerList.Add(PlayerUIManager.Instance.giveGold);
			}
			if (controllerHorizontalIndex == -1 || _controllerList[controllerHorizontalIndex] != cardListContainer)
			{
				controllerHorizontalIndex = Functions.GetListClosestIndexToMousePosition(_controllerList);
			}
			controllerHorizontalIndex = Functions.GetClosestIndexBasedOnDirection(_controllerList, controllerHorizontalIndex, goingUp, goingRight, goingDown, goingLeft);
			if (_controllerList[controllerHorizontalIndex] == cardListContainer)
			{
				controllerIsOnVerticalList = true;
				ControllerMovement(goingUp: true, goingRight: false, goingDown: false, goingLeft: false, controllerVerticalIndex);
			}
			else
			{
				warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerList[controllerHorizontalIndex].position);
				Mouse.current.WarpCursorPosition(warpPosition);
			}
		}
		else
		{
			if (craftType != 5)
			{
				return;
			}
			_controllerVerticalList.Clear();
			if (Functions.TransformIsVisible(challengePerks))
			{
				for (int num4 = 0; num4 < perkChallengeItems.Length; num4++)
				{
					if (Functions.TransformIsVisible(perkChallengeItems[num4].transform))
					{
						_controllerList.Add(perkChallengeItems[num4].transform);
					}
				}
			}
			else if (tmpContainer.childCount > 0)
			{
				for (int num5 = 0; num5 < 3; num5++)
				{
					if (tmpContainer.GetChild(num5) != null)
					{
						_controllerList.Add(tmpContainer.GetChild(num5).transform);
						_controllerList.Add(cardChallengeButton[num5].transform);
					}
				}
			}
			else
			{
				for (int num6 = 0; num6 < cardChallengeContainer.Length; num6++)
				{
					foreach (Transform item11 in cardChallengeContainer[num6])
					{
						if ((bool)item11.GetComponent<CardVertical>())
						{
							_controllerList.Add(item11);
						}
					}
					_controllerList.Add(cardChallengeSelected[num6].transform);
				}
			}
			foreach (Transform item12 in cardListContainer)
			{
				_controllerVerticalList.Add(item12);
			}
			if (controllerHorizontalIndex == -1 && controllerBlock == -1)
			{
				controllerHorizontalIndex = 0;
				controllerBlock = 0;
			}
			else if (controllerBlock == 0)
			{
				if (Functions.TransformIsVisible(challengePerks))
				{
					if (goingUp)
					{
						if (controllerHorizontalIndex > 7)
						{
							controllerHorizontalIndex = Functions.GetClosestIndexFromList(_controllerList[controllerHorizontalIndex], _controllerList, controllerHorizontalIndex, new Vector3(0f, 0.5f, 0f));
						}
					}
					else if (goingDown)
					{
						if ((_controllerList.Count == 28 && controllerHorizontalIndex > 20) || (_controllerList.Count == 21 && controllerHorizontalIndex > 13))
						{
							controllerBlock = 2;
							warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(readyChallenge.position);
							Mouse.current.WarpCursorPosition(warpPosition);
							return;
						}
						controllerHorizontalIndex = Functions.GetClosestIndexFromList(_controllerList[controllerHorizontalIndex], _controllerList, controllerHorizontalIndex, new Vector3(0f, -0.5f, 0f));
					}
					else if (goingRight)
					{
						if (controllerHorizontalIndex != 6 && controllerHorizontalIndex != 13 && controllerHorizontalIndex != 19 && controllerHorizontalIndex != 25)
						{
							controllerHorizontalIndex = Functions.GetClosestIndexFromList(_controllerList[controllerHorizontalIndex], _controllerList, controllerHorizontalIndex, new Vector3(1.5f, 0f, 0f));
						}
					}
					else
					{
						if (controllerHorizontalIndex == 0 || controllerHorizontalIndex == 7 || controllerHorizontalIndex == 14 || controllerHorizontalIndex == 21)
						{
							controllerBlock = 3;
							controllerHorizontalIndex = 0;
							warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerVerticalList[0].position);
							Mouse.current.WarpCursorPosition(warpPosition);
							return;
						}
						controllerHorizontalIndex = Functions.GetClosestIndexFromList(_controllerList[controllerHorizontalIndex], _controllerList, controllerHorizontalIndex, new Vector3(-1.5f, 0f, 0f));
					}
				}
				else if (tmpContainer.childCount > 0)
				{
					if (goingUp)
					{
						if (controllerHorizontalIndex % 2 == 1)
						{
							controllerHorizontalIndex--;
						}
					}
					else if (goingDown)
					{
						if (controllerHorizontalIndex % 2 != 0)
						{
							controllerBlock = 2;
							warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(readyChallenge.position);
							Mouse.current.WarpCursorPosition(warpPosition);
							return;
						}
						controllerHorizontalIndex++;
					}
					else if (goingLeft)
					{
						if (controllerHorizontalIndex == 0 || controllerHorizontalIndex == 1)
						{
							controllerBlock = 3;
							controllerHorizontalIndex = 0;
							warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerVerticalList[0].position);
							Mouse.current.WarpCursorPosition(warpPosition);
							return;
						}
						controllerHorizontalIndex -= 2;
					}
					else if (goingRight && controllerHorizontalIndex != 4 && controllerHorizontalIndex != 5)
					{
						controllerHorizontalIndex += 2;
					}
				}
				else if (goingUp)
				{
					if (controllerHorizontalIndex % 4 == 0)
					{
						if (controllerHorizontalIndex < 16)
						{
							controllerBlock = 1;
							warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(rerollChallenge.transform.position);
							Mouse.current.WarpCursorPosition(warpPosition);
							return;
						}
						controllerHorizontalIndex -= 13;
					}
					else
					{
						controllerHorizontalIndex--;
					}
				}
				else if (goingDown)
				{
					if (controllerHorizontalIndex % 4 == 3)
					{
						if (controllerHorizontalIndex > 16)
						{
							controllerBlock = 2;
							warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(readyChallenge.position);
							Mouse.current.WarpCursorPosition(warpPosition);
							return;
						}
						controllerHorizontalIndex += 13;
					}
					else
					{
						controllerHorizontalIndex++;
					}
				}
				else if (goingRight)
				{
					if ((controllerHorizontalIndex < 12 || controllerHorizontalIndex >= 16) && (controllerHorizontalIndex < 28 || controllerHorizontalIndex >= 32))
					{
						controllerHorizontalIndex += 4;
					}
				}
				else if (goingLeft)
				{
					if ((controllerHorizontalIndex >= 0 && controllerHorizontalIndex < 4) || (controllerHorizontalIndex >= 16 && controllerHorizontalIndex < 20))
					{
						controllerBlock = 3;
						controllerHorizontalIndex = 0;
						warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerVerticalList[0].position);
						Mouse.current.WarpCursorPosition(warpPosition);
						return;
					}
					controllerHorizontalIndex -= 4;
				}
			}
			else if (controllerBlock == 1)
			{
				if (!(goingLeft || goingDown))
				{
					return;
				}
				controllerHorizontalIndex = 12;
				controllerBlock = 0;
			}
			else if (controllerBlock == 2)
			{
				if (!(goingLeft || goingUp))
				{
					return;
				}
				controllerHorizontalIndex = _controllerList.Count - 1;
				controllerBlock = 0;
			}
			else
			{
				if (controllerBlock == 3)
				{
					if (goingDown)
					{
						controllerHorizontalIndex++;
					}
					else if (goingUp)
					{
						controllerHorizontalIndex--;
					}
					else
					{
						if (goingLeft)
						{
							controllerBlock = 4;
							controllerHorizontalIndex = 0;
							controllerVerticalIndex = heroIndex;
							warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(ChallengeSelectionManager.Instance.sideCharacters.charArray[controllerVerticalIndex].transform.position);
							Mouse.current.WarpCursorPosition(warpPosition);
							return;
						}
						if (goingRight)
						{
							controllerBlock = 0;
							controllerHorizontalIndex = 0;
							warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerList[controllerHorizontalIndex].position);
							Mouse.current.WarpCursorPosition(warpPosition);
							return;
						}
					}
					if (controllerHorizontalIndex > _controllerVerticalList.Count - 1)
					{
						controllerHorizontalIndex = _controllerVerticalList.Count - 1;
					}
					else if (controllerHorizontalIndex < 0)
					{
						controllerHorizontalIndex = 0;
					}
					warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerVerticalList[controllerHorizontalIndex].position);
					Mouse.current.WarpCursorPosition(warpPosition);
					return;
				}
				if (controllerBlock == 4)
				{
					if (goingDown)
					{
						controllerVerticalIndex++;
					}
					else
					{
						if (!goingUp)
						{
							if (goingRight)
							{
								controllerBlock = 3;
								controllerHorizontalIndex = 0;
								warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerVerticalList[controllerHorizontalIndex].position);
								Mouse.current.WarpCursorPosition(warpPosition);
							}
							return;
						}
						controllerVerticalIndex--;
					}
					if (controllerVerticalIndex > 3)
					{
						controllerVerticalIndex = 3;
					}
					else if (controllerVerticalIndex < 0)
					{
						controllerVerticalIndex = 0;
					}
					warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(ChallengeSelectionManager.Instance.sideCharacters.charArray[controllerVerticalIndex].transform.position);
					Mouse.current.WarpCursorPosition(warpPosition);
					return;
				}
			}
			if (controllerHorizontalIndex > -1 && _controllerList[controllerHorizontalIndex] != null)
			{
				warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerList[controllerHorizontalIndex].position);
				Mouse.current.WarpCursorPosition(warpPosition);
			}
		}
	}

	public void ControllerMoveShoulder(bool _isRight = false)
	{
		if (craftType == 2 || craftType == 4)
		{
			if (_isRight)
			{
				DoNextPage();
			}
			else
			{
				DoPrevPage();
			}
		}
		else if (craftType == 5)
		{
			ChallengeSelectionManager.Instance.NextHeroFunc(_isRight);
		}
	}

	public void ControllerNextPage(bool _isNext = true)
	{
		if (craftType == 2 || craftType == 4)
		{
			if (_isNext)
			{
				DoNextPage();
			}
			else
			{
				DoPrevPage();
			}
		}
	}

	private void GetShadyDealRemaining()
	{
		if (!GameManager.Instance.IsMultiplayer())
		{
			shadyRemaining = 4;
			return;
		}
		shadyRemaining = 0;
		for (int i = 0; i < 4; i++)
		{
			if (AtOManager.Instance.GetHero(i)?.Owner == NetworkManager.Instance.GetPlayerNick())
			{
				shadyRemaining++;
			}
		}
	}

	private void SetShadyDealLeftUses()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (shadyRemaining <= 0)
		{
			stringBuilder.Append("<color=#A86C65>");
		}
		stringBuilder.Append("[");
		stringBuilder.Append(Texts.Instance.GetText("shadyDealLeft"));
		stringBuilder.Append("]");
		shadyDealLeft.text = string.Format(stringBuilder.ToString(), shadyRemaining);
		if (AtOManager.Instance.GetPlayerDust() < shadyDealDust || shadyRemaining <= 0)
		{
			shadyDealButton.GetComponent<BotonGeneric>().Disable();
		}
		else
		{
			shadyDealButton.GetComponent<BotonGeneric>().Enable();
		}
	}

	private void GetShadyDealDustGold(List<string> itemList)
	{
		float num = 0f;
		for (int i = 0; i < itemList.Count; i++)
		{
			float num2 = SetPrice("Item", Enum.GetName(typeof(Enums.CardRarity), Globals.Instance.GetCardData(itemList[i], instantiate: false).CardRarity), itemList[i], craftTierZone, useShopDiscount: false);
			num += num2;
		}
		float num3 = num / (float)itemList.Count / 4f;
		int num4 = UnityEngine.Random.Range(12, 16);
		shadyDealGold = Functions.FuncRoundToInt(num3 * ((float)num4 / 100f));
		int num5 = UnityEngine.Random.Range(30, 45);
		shadyDealDust = Functions.FuncRoundToInt((float)shadyDealGold * ((float)num5 / 10f));
	}

	private void SetShadyDealDustGoldValues()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<sprite name=dust> ");
		stringBuilder.Append(shadyDealDust);
		shadyDealButton.GetComponent<BotonGeneric>().SetText(stringBuilder.ToString());
		stringBuilder.Clear();
		stringBuilder.Append("<sprite name=gold> ");
		stringBuilder.Append(shadyDealGold);
		shadyDealResult.text = stringBuilder.ToString();
	}

	public void BuyShadyDeal()
	{
		if (blocked)
		{
			return;
		}
		SetBlocked(_status: true);
		if (shadyRemaining > 0 && shadyDealDust < AtOManager.Instance.GetPlayerDust())
		{
			shadyRemaining--;
			AtOManager.Instance.PayDust(shadyDealDust);
			if (GameManager.Instance.IsMultiplayer())
			{
				AtOManager.Instance.AskForGold(NetworkManager.Instance.GetPlayerNick(), shadyDealGold);
			}
			else
			{
				AtOManager.Instance.GivePlayer(0, shadyDealGold);
			}
			SetShadyDealLeftUses();
		}
		SetBlocked(_status: false);
	}
}
