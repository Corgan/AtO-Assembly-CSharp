using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using WebSocketSharp;

public class HeroSelectionManager : MonoBehaviour
{
	public Transform sceneCamera;

	private GameObject box;

	public GameObject[] boxGO;

	private BoxSelection[] boxSelection;

	public Dictionary<string, HeroSelection> heroSelectionDictionary = new Dictionary<string, HeroSelection>();

	private Dictionary<GameObject, bool> boxFilled = new Dictionary<GameObject, bool>();

	private Dictionary<GameObject, HeroSelection> boxHero = new Dictionary<GameObject, HeroSelection>();

	public GameObject heroSelectionPrefab;

	public GameObject cursorDragPrefab;

	public bool dragging;

	public TitleMovement titleMovement;

	public Transform titleGroupDefault;

	public Transform titleWeeklyDefault;

	public Transform boxCharacters;

	public GameObject charPopupPrefab;

	public GameObject charPopupGO;

	public CharPopup charPopup;

	public CharPopupMini charPopupMini;

	public Transform charContainerBg;

	public GameObject charContainerGO;

	public GameObject allGO;

	public GameObject warriorsGO;

	public GameObject healersGO;

	public GameObject magesGO;

	public GameObject scoutsGO;

	public GameObject dlcsGO;

	public GameObject lockedGO;

	public TMP_Text _ClassWarriors;

	public TMP_Text _ClassHealers;

	public TMP_Text _ClassMages;

	public TMP_Text _ClassScouts;

	public TMP_Text _ClassMagicKnights;

	public Transform masterDescription;

	public Transform gameSeed;

	public Transform gameSeedModify;

	public TMP_Text gameSeedTxt;

	public BotonGeneric botonBegin;

	public BotonGeneric botonFollow;

	public Transform popupMask;

	public Transform ngTransform;

	public BotonGeneric ngButton;

	public Transform ngSelectionX;

	public Transform ngLock;

	public Transform separator;

	public TMP_Text madnessLevel;

	public Transform madnessParticle;

	public Transform madnessButton;

	public Transform weeklyModifiersButton;

	public Transform sandboxButton;

	public Transform sandboxButtonCircleOn;

	public Transform sandboxButtonCircleOff;

	public Transform weeklyT;

	public TMP_Text weeklyNumber;

	public TMP_Text weeklyLeft;

	private bool setWeekly;

	private int ngValue;

	private int ngValueMaster;

	private string ngCorruptors = "";

	private int obeliskMadnessValue;

	private int obeliskMadnessValueMaster;

	private int singularityMadnessValue;

	private int singularityMadnessValueMaster;

	private Dictionary<string, SubClassData[]> subclassDictionary = new Dictionary<string, SubClassData[]>();

	private Dictionary<string, SubClassData> nonHistorySubclassDictionary = new Dictionary<string, SubClassData>();

	private PhotonView photonView;

	private Dictionary<string, string> SubclassByName = new Dictionary<string, string>();

	private Dictionary<string, List<string>> playerHeroPerksDict;

	public Dictionary<string, string> playerHeroSkinsDict;

	public Dictionary<string, string> playerHeroCardbackDict;

	public Transform beginAdventureButton;

	public Transform readyButtonText;

	public Transform readyButton;

	private bool statusReady;

	private Coroutine manualReadyCo;

	public Transform weeklySelector;

	private int controllerCurrentOption = -1;

	private int controllerCurrentBlock;

	public List<Transform> menuController;

	public List<Transform> positionsController;

	public HeroSelection controllerCurrentHS;

	public int controllerHorizontalIndex = -1;

	private Vector2 warpPosition = Vector2.zero;

	private List<Transform> _controllerList = new List<Transform>();

	private List<Transform> _controllerVerticalList = new List<Transform>();

	private Coroutine coroutineMask;

	private GameObject currentSelectionTab;

	public GameObject CardBacksPopUp;

	[SerializeField]
	private GameObject charPageLeftButton;

	[SerializeField]
	private GameObject charPageRightButton;

	public GameObject EnemySelectionButton;

	public GameObject MadnessButton;

	public GameObject SandboxButton;

	public GameObject TraitsTab;

	public GameObject CardsandItemsTab;

	public GameObject ResistancesTab;

	public GameObject SingularityCardsTabText;

	public CardItem[] cardsCI = new CardItem[7];

	public TMP_Text _CardN1;

	public TMP_Text _CardN2;

	public TMP_Text _CardN3;

	public TMP_Text _CardN4;

	public TMP_Text _CardN5;

	public TMP_Text _CardN6;

	public TMP_Text _CardN7;

	public TMP_Text _Trait0;

	public TMP_Text _Trait1A;

	public TMP_Text _Trait1B;

	public TMP_Text _Trait2A;

	public TMP_Text _Trait2B;

	public TMP_Text _Trait3A;

	public TMP_Text _Trait3B;

	public TMP_Text _Trait4A;

	public TMP_Text _Trait4B;

	private TraitRollOver _Trait0RO;

	private TraitRollOver _Trait1ARO;

	private TraitRollOver _Trait1BRO;

	private TraitRollOver _Trait2ARO;

	private TraitRollOver _Trait2BRO;

	private TraitRollOver _Trait3ARO;

	private TraitRollOver _Trait3BRO;

	private TraitRollOver _Trait4ARO;

	private TraitRollOver _Trait4BRO;

	public BotonGeneric TraitsTabBtn;

	public BotonGeneric ResistanceTabBtn;

	public BotonGeneric CardsItemsTabBtn;

	public string CurrentFilter;

	private Transform[] cardsNumT = new Transform[7];

	private TMP_Text[] cardsNumText = new TMP_Text[7];

	public static HeroSelectionManager Instance { get; private set; }

	public string NgCorruptors
	{
		get
		{
			return ngCorruptors;
		}
		set
		{
			ngCorruptors = value;
		}
	}

	public int NgValue
	{
		get
		{
			return ngValue;
		}
		set
		{
			ngValue = value;
		}
	}

	public int NgValueMaster
	{
		get
		{
			return ngValueMaster;
		}
		set
		{
			ngValueMaster = value;
		}
	}

	public int ObeliskMadnessValueMaster
	{
		get
		{
			return obeliskMadnessValueMaster;
		}
		set
		{
			obeliskMadnessValueMaster = value;
		}
	}

	public int ObeliskMadnessValue
	{
		get
		{
			return obeliskMadnessValue;
		}
		set
		{
			obeliskMadnessValue = value;
		}
	}

	public int SingularityMadnessValueMaster
	{
		get
		{
			return singularityMadnessValueMaster;
		}
		set
		{
			singularityMadnessValueMaster = value;
		}
	}

	public int SingularityMadnessValue
	{
		get
		{
			return singularityMadnessValue;
		}
		set
		{
			singularityMadnessValue = value;
		}
	}

	public Dictionary<string, List<string>> PlayerHeroPerksDict
	{
		get
		{
			return playerHeroPerksDict;
		}
		set
		{
			playerHeroPerksDict = value;
		}
	}

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
		photonView = PhotonView.Get(this);
		sceneCamera.gameObject.SetActive(value: false);
		NetworkManager.Instance.StartStopQueue(state: true);
		currentSelectionTab = allGO;
		if (!AtOManager.Instance.IsCombatTool)
		{
			EnemySelectionButton.SetActive(value: false);
		}
		else
		{
			PlayerManager.Instance.GainSupply(500);
		}
	}

	private void Start()
	{
		StartCoroutine(StartCo());
		RefreshSelectedCharPortraits();
		if (AtOManager.Instance.IsCombatTool)
		{
			SetCombatToolUI();
			return;
		}
		MadnessButton.GetComponent<BotonGeneric>().Enable();
		sandboxButton.GetComponent<BotonGeneric>().Enable();
		gameSeedModify.gameObject.SetActive(value: true);
		resetMadnessOriginal();
	}

	private void resetMadnessOriginal()
	{
		string text = "";
		if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
		{
			text = "Coop";
		}
		if (!GameManager.Instance.IsGameAdventure() || !SaveManager.PrefsHasKey("madnessLevel" + text) || !SaveManager.PrefsHasKey("madnessCorruptors" + text))
		{
			return;
		}
		int num = 0;
		num = SaveManager.LoadPrefsInt("madnessLevel");
		string text2 = "";
		text2 = SaveManager.LoadPrefsString("madnessCorruptors");
		if (PlayerManager.Instance.NgLevel >= num)
		{
			ngValueMaster = (ngValue = num);
			if (text2 != "")
			{
				ngCorruptors = text2;
			}
		}
		else
		{
			ngValueMaster = (ngValue = 0);
			ngCorruptors = "";
		}
		SetMadnessLevel();
	}

	public void Refresh()
	{
		StartCoroutine(StartCo());
	}

	private void RefreshSelectedCharPortraits()
	{
		foreach (KeyValuePair<string, HeroSelection> item in heroSelectionDictionary)
		{
			if (item.Value.gameObject.transform.parent != item.Value.DefaultParent)
			{
				item.Value.gameObject.SetActive(value: true);
				item.Value.spriteSR.enabled = true;
				item.Value.nameOver.gameObject.SetActive(value: true);
				item.Value.rankOver.gameObject.SetActive(value: true);
			}
		}
	}

	public void SetDefaultMiniPopupHero(int index = 0)
	{
		HeroSelection heroSelection = new HeroSelection();
		heroSelection = GetBoxHeroFromIndex(index);
		if (heroSelection != null)
		{
			charPopupMini.SetSubClassData(heroSelection.subClassData);
		}
		else if (!GameManager.Instance.IsWeeklyChallenge() && PlayerManager.Instance.LastUsedTeam != null && PlayerManager.Instance.LastUsedTeam.Count() > 0)
		{
			charPopupMini.SetSubClassData(Globals.Instance.GetSubClassData(PlayerManager.Instance.LastUsedTeam[0]));
		}
		else if (!GameManager.Instance.IsWeeklyChallenge())
		{
			charPopupMini.SetSubClassData(Globals.Instance.GetSubClassData("mercenary"));
		}
		SelectTab("TraitsTab");
	}

	private IEnumerator StartCo()
	{
		HeroSelectionManager heroSelectionManager = this;
		HeroSelectionManager heroSelectionManager2 = this;
		int num = 0;
		heroSelectionManager2.ngValue = 0;
		heroSelectionManager.ngValueMaster = num;
		ngCorruptors = "";
		HeroSelectionManager heroSelectionManager3 = this;
		HeroSelectionManager heroSelectionManager4 = this;
		num = 0;
		heroSelectionManager4.obeliskMadnessValueMaster = 0;
		heroSelectionManager3.obeliskMadnessValue = num;
		HeroSelectionManager heroSelectionManager5 = this;
		HeroSelectionManager heroSelectionManager6 = this;
		num = 0;
		heroSelectionManager6.singularityMadnessValueMaster = 0;
		heroSelectionManager5.singularityMadnessValue = num;
		madnessLevel.text = string.Format(Texts.Instance.GetText("madnessNumber"), 0);
		if (GameManager.Instance.IsMultiplayer())
		{
			if (NetworkManager.Instance.IsMaster())
			{
				NetworkManager.Instance.PlayerSkuList.Clear();
				while (!NetworkManager.Instance.AllPlayersReady("heroSelection"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked heroSelection");
				}
				if (GameManager.Instance.IsLoadingGame())
				{
					photonView.RPC("NET_SetLoadingGame", RpcTarget.Others);
				}
				NetworkManager.Instance.PlayersNetworkContinue("heroSelection", AtOManager.Instance.GetWeekly().ToString());
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
			else
			{
				GameManager.Instance.SetGameStatus(Enums.GameStatus.NewGame);
				NetworkManager.Instance.SetWaitingSyncro("heroSelection", status: true);
				NetworkManager.Instance.SetStatusReady("heroSelection");
				while (NetworkManager.Instance.WaitingSyncro["heroSelection"])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (NetworkManager.Instance.netAuxValue != "")
				{
					AtOManager.Instance.SetWeekly(int.Parse(NetworkManager.Instance.netAuxValue));
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("heroSelection, we can continue!");
				}
			}
		}
		StartCoroutine(StartCoContinue());
	}

	private IEnumerator StartCoContinue()
	{
		MadnessManager.Instance.ShowMadness();
		MadnessManager.Instance.RefreshValues();
		MadnessManager.Instance.ShowMadness();
		playerHeroSkinsDict = new Dictionary<string, string>();
		playerHeroCardbackDict = new Dictionary<string, string>();
		boxSelection = new BoxSelection[boxGO.Length];
		for (int i = 0; i < boxGO.Length; i++)
		{
			boxHero[boxGO[i]] = null;
			boxFilled[boxGO[i]] = false;
			boxSelection[i] = boxGO[i].GetComponent<BoxSelection>();
		}
		ShowDrag(state: false, Vector3.zero);
		int num = 7;
		int num2 = 5;
		foreach (KeyValuePair<string, SubClassData> item3 in Globals.Instance.SubClass)
		{
			if (!item3.Value.MainCharacter)
			{
				if (!nonHistorySubclassDictionary.ContainsKey(item3.Key))
				{
					nonHistorySubclassDictionary.Add(item3.Key, Globals.Instance.SubClass[item3.Key]);
				}
			}
			else if (item3.Value.IsMultiClass())
			{
				string key = "dlc";
				if (!subclassDictionary.ContainsKey(key))
				{
					subclassDictionary.Add(key, new SubClassData[num]);
				}
				subclassDictionary[key][Globals.Instance.SubClass[item3.Key].OrderInList] = Globals.Instance.SubClass[item3.Key];
			}
			else
			{
				string key2 = Enum.GetName(typeof(Enums.HeroClass), Globals.Instance.SubClass[item3.Key].HeroClass).ToLower().Replace(" ", "");
				if (!subclassDictionary.ContainsKey(key2))
				{
					subclassDictionary.Add(key2, new SubClassData[num]);
				}
				subclassDictionary[key2][Globals.Instance.SubClass[item3.Key].OrderInList] = Globals.Instance.SubClass[item3.Key];
			}
		}
		_ClassWarriors.color = Functions.HexToColor(Globals.Instance.ClassColor["warrior"]);
		_ClassHealers.color = Functions.HexToColor(Globals.Instance.ClassColor["healer"]);
		_ClassMages.color = Functions.HexToColor(Globals.Instance.ClassColor["mage"]);
		_ClassScouts.color = Functions.HexToColor(Globals.Instance.ClassColor["scout"]);
		_ClassMagicKnights.color = Functions.HexToColor(Globals.Instance.ClassColor["magicknight"]);
		float num3 = 1f;
		float num4 = 0.55f;
		float num5 = 1.75f;
		float y = -0.65f;
		for (int j = 0; j < num2; j++)
		{
			for (int k = 0; k < num; k++)
			{
				SubClassData subClassData = null;
				GameObject gameObject = null;
				switch (j)
				{
				case 0:
					subClassData = subclassDictionary["warrior"][k];
					gameObject = warriorsGO;
					break;
				case 1:
					subClassData = subclassDictionary["scout"][k];
					gameObject = scoutsGO;
					break;
				case 2:
					subClassData = subclassDictionary["mage"][k];
					gameObject = magesGO;
					break;
				case 3:
					subClassData = subclassDictionary["healer"][k];
					gameObject = healersGO;
					break;
				case 4:
				case 5:
					if (subclassDictionary.ContainsKey("dlc"))
					{
						subClassData = subclassDictionary["dlc"][k];
						gameObject = dlcsGO;
					}
					break;
				}
				if (subClassData == null)
				{
					continue;
				}
				GameObject gameObject2 = UnityEngine.Object.Instantiate(heroSelectionPrefab, Vector3.zero, Quaternion.identity, gameObject.transform);
				gameObject2.transform.localPosition = new Vector3(num4 + num5 * (float)k, y, 0f);
				gameObject2.transform.localScale = new Vector3(num3, num3, 1f);
				gameObject2.name = subClassData.Id;
				HeroSelection component = gameObject2.transform.Find("Portrait").transform.GetComponent<HeroSelection>();
				heroSelectionDictionary.Add(gameObject2.name, component);
				component.blocked = !PlayerManager.Instance.IsHeroUnlocked(subClassData.Id);
				if (component.blocked && GameManager.Instance.IsObeliskChallenge() && !GameManager.Instance.IsWeeklyChallenge())
				{
					component.blocked = false;
				}
				if (component.blocked && GameManager.Instance.IsWeeklyChallenge())
				{
					ChallengeData weeklyData = Globals.Instance.GetWeeklyData(Functions.GetCurrentWeeklyWeek());
					if (weeklyData != null && (subClassData.Id == weeklyData.Hero1.Id || subClassData.Id == weeklyData.Hero2.Id || subClassData.Id == weeklyData.Hero3.Id || subClassData.Id == weeklyData.Hero4.Id))
					{
						component.blocked = false;
					}
				}
				component.SetSubclass(subClassData);
				string text = PlayerManager.Instance.GetActiveSkin(subClassData.Id);
				if (text != "")
				{
					SkinData skinData = Globals.Instance.GetSkinData(text);
					if (skinData == null)
					{
						text = Globals.Instance.GetSkinBaseIdBySubclass(subClassData.Id);
						skinData = Globals.Instance.GetSkinData(text);
					}
					AddToPlayerHeroSkin(subClassData.Id, text);
					component.SetSprite(skinData.SpritePortrait, skinData.SpriteSilueta, subClassData.SpriteBorderLocked);
				}
				else
				{
					component.SetSprite(subClassData.SpriteSpeed, subClassData.SpriteBorderSmall, subClassData.SpriteBorderLocked);
				}
				component.SetName(subClassData.CharacterName);
				component.Init();
				if (subClassData.SpriteBorderLocked != null && subClassData.SpriteBorderLocked.name == "regularBorderSmall")
				{
					component.ShowComingSoon();
				}
				SubclassByName.Add(subClassData.Id, subClassData.SubClassName);
				if (GameManager.Instance.IsWeeklyChallenge())
				{
					component.blocked = true;
				}
				menuController.Add(component.transform);
			}
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			SetDefaultMiniPopupHero();
			List<string> list = new List<string>();
			for (int l = 0; l < Globals.Instance.SkuAvailable.Count; l++)
			{
				if (SteamManager.Instance.PlayerHaveDLC(Globals.Instance.SkuAvailable[l]))
				{
					list.Add(Globals.Instance.SkuAvailable[l]);
				}
			}
			string text2 = "";
			if (list.Count > 0)
			{
				text2 = JsonHelper.ToJson(list.ToArray());
			}
			if (NetworkManager.Instance.IsMaster())
			{
				photonView.RPC("NET_SetSku", RpcTarget.All, NetworkManager.Instance.GetPlayerNick(), text2);
			}
			else
			{
				string roomName = NetworkManager.Instance.GetRoomName();
				if (roomName != "")
				{
					SaveManager.SaveIntoPrefsString("coopRoomId", roomName);
					SaveManager.SavePrefs();
				}
				NetworkManager.Instance.SetWaitingSyncro("skuWait", status: true);
				photonView.RPC("NET_SetSku", RpcTarget.All, NetworkManager.Instance.GetPlayerNick(), text2);
			}
			if (NetworkManager.Instance.IsMaster())
			{
				while (!NetworkManager.Instance.AllPlayersHaveSkuList())
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked skuWait");
				}
				NetworkManager.Instance.PlayersNetworkContinue("skuWait");
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
			else
			{
				while (NetworkManager.Instance.WaitingSyncro["skuWait"])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("skuWait, we can continue!");
				}
			}
		}
		RearrangeHerosData();
		if (GameManager.Instance.IsMultiplayer() && GameManager.Instance.IsLoadingGame())
		{
			foreach (KeyValuePair<string, SubClassData> item4 in nonHistorySubclassDictionary)
			{
				SubClassData value = item4.Value;
				GameObject gameObject3 = UnityEngine.Object.Instantiate(heroSelectionPrefab, Vector3.zero, Quaternion.identity);
				gameObject3.transform.localPosition = new Vector3(-10f, -10f, 100f);
				gameObject3.name = value.Id;
				HeroSelection component2 = gameObject3.transform.Find("Portrait").transform.GetComponent<HeroSelection>();
				heroSelectionDictionary.Add(gameObject3.name, component2);
				component2.blocked = true;
				component2.SetSubclass(value);
				component2.SetSprite(value.SpriteSpeed, value.SpriteBorderSmall, value.SpriteBorderLocked);
				component2.SetName(value.CharacterName);
				component2.Init();
				SubclassByName.Add(value.Id, value.SubClassName);
			}
		}
		if (GameManager.Instance.IsGameAdventure() && AtOManager.Instance.IsFirstGame() && !GameManager.Instance.IsMultiplayer())
		{
			AtOManager.Instance.SetGameId("tuto");
			heroSelectionDictionary["mercenary"].AssignHeroToBox(boxGO[0]);
			heroSelectionDictionary["ranger"].AssignHeroToBox(boxGO[1]);
			heroSelectionDictionary["elementalist"].AssignHeroToBox(boxGO[2]);
			heroSelectionDictionary["cleric"].AssignHeroToBox(boxGO[3]);
			SandboxManager.Instance.DisableSandbox();
			yield return Globals.Instance.WaitForSeconds(1f);
			BeginAdventure();
			yield break;
		}
		charPopupGO = charPopup.gameObject;
		charPopup = charPopupGO.GetComponent<CharPopup>();
		charPopup.HideNow();
		if (!GameManager.Instance.IsWeeklyChallenge())
		{
			titleGroupDefault.gameObject.SetActive(value: true);
			titleWeeklyDefault.gameObject.SetActive(value: false);
			weeklyModifiersButton.gameObject.SetActive(value: false);
			weeklyT.gameObject.SetActive(value: false);
		}
		else
		{
			titleGroupDefault.gameObject.SetActive(value: false);
			titleWeeklyDefault.gameObject.SetActive(value: true);
			weeklyModifiersButton.gameObject.SetActive(value: true);
			weeklyT.gameObject.SetActive(value: true);
			setWeekly = true;
			if (!GameManager.Instance.IsLoadingGame())
			{
				AtOManager.Instance.SetWeekly(Functions.GetCurrentWeeklyWeek());
			}
			weeklyNumber.text = AtOManager.Instance.GetWeeklyName(AtOManager.Instance.GetWeekly());
		}
		if (GameManager.Instance.IsGameAdventure())
		{
			if (AtOManager.Instance.IsCombatTool)
			{
				titleMovement.SetText(Texts.Instance.GetText("modePractice"));
			}
			else
			{
				titleMovement.SetText(Texts.Instance.GetText("modeAdventure"));
			}
			madnessButton.gameObject.SetActive(value: true);
			if (GameManager.Instance.IsMultiplayer())
			{
				if (NetworkManager.Instance.IsMaster())
				{
					if (GameManager.Instance.IsLoadingGame())
					{
						ngValueMaster = (ngValue = AtOManager.Instance.GetNgPlus());
						ngCorruptors = AtOManager.Instance.GetMadnessCorruptors();
						SetMadnessLevel();
					}
					else if (SaveManager.PrefsHasKey("madnessLevelCoop") && SaveManager.PrefsHasKey("madnessCorruptorsCoop"))
					{
						int num6 = SaveManager.LoadPrefsInt("madnessLevelCoop");
						string text3 = SaveManager.LoadPrefsString("madnessCorruptorsCoop");
						if (PlayerManager.Instance.NgLevel >= num6)
						{
							ngValueMaster = (ngValue = num6);
							if (text3 != "")
							{
								ngCorruptors = text3;
							}
						}
						else
						{
							HeroSelectionManager heroSelectionManager = this;
							HeroSelectionManager heroSelectionManager2 = this;
							int num7 = 0;
							heroSelectionManager2.ngValue = 0;
							heroSelectionManager.ngValueMaster = num7;
							ngCorruptors = "";
						}
						SetMadnessLevel();
					}
				}
			}
			else if (SaveManager.PrefsHasKey("madnessLevel") && SaveManager.PrefsHasKey("madnessCorruptors"))
			{
				int num8 = SaveManager.LoadPrefsInt("madnessLevel");
				string text4 = SaveManager.LoadPrefsString("madnessCorruptors");
				if (PlayerManager.Instance.NgLevel >= num8)
				{
					ngValueMaster = (ngValue = num8);
					if (text4 != "")
					{
						ngCorruptors = text4;
					}
				}
				else
				{
					HeroSelectionManager heroSelectionManager3 = this;
					HeroSelectionManager heroSelectionManager4 = this;
					int num7 = 0;
					heroSelectionManager4.ngValue = 0;
					heroSelectionManager3.ngValueMaster = num7;
					ngCorruptors = "";
				}
				SetMadnessLevel();
			}
		}
		else if (GameManager.Instance.IsSingularity())
		{
			titleMovement.SetText(Texts.Instance.GetText("singularity"));
			madnessButton.gameObject.SetActive(value: true);
			if (GameManager.Instance.IsMultiplayer())
			{
				if (NetworkManager.Instance.IsMaster())
				{
					if (GameManager.Instance.IsLoadingGame())
					{
						singularityMadnessValue = (singularityMadnessValueMaster = AtOManager.Instance.GetSingularityMadness());
						SetSingularityMadnessLevel();
					}
					else if (SaveManager.PrefsHasKey("singularityMadnessCoop"))
					{
						int num9 = SaveManager.LoadPrefsInt("singularityMadnessCoop");
						if (PlayerManager.Instance.ObeliskMadnessLevel >= num9)
						{
							singularityMadnessValue = (singularityMadnessValueMaster = num9);
						}
						else
						{
							HeroSelectionManager heroSelectionManager5 = this;
							HeroSelectionManager heroSelectionManager6 = this;
							int num7 = 0;
							heroSelectionManager6.singularityMadnessValueMaster = 0;
							heroSelectionManager5.singularityMadnessValue = num7;
						}
						SetSingularityMadnessLevel();
					}
				}
			}
			else if (SaveManager.PrefsHasKey("singularityMadness"))
			{
				int num10 = SaveManager.LoadPrefsInt("singularityMadness");
				if (PlayerManager.Instance.SingularityMadnessLevel >= num10)
				{
					singularityMadnessValue = (singularityMadnessValueMaster = num10);
				}
				else
				{
					HeroSelectionManager heroSelectionManager7 = this;
					HeroSelectionManager heroSelectionManager8 = this;
					int num7 = 0;
					heroSelectionManager8.singularityMadnessValueMaster = 0;
					heroSelectionManager7.singularityMadnessValue = num7;
				}
				SetSingularityMadnessLevel();
			}
		}
		else if (!GameManager.Instance.IsWeeklyChallenge())
		{
			titleMovement.SetText(Texts.Instance.GetText("modeObelisk"));
			madnessButton.gameObject.SetActive(value: true);
			if (GameManager.Instance.IsMultiplayer())
			{
				if (NetworkManager.Instance.IsMaster())
				{
					if (GameManager.Instance.IsLoadingGame())
					{
						obeliskMadnessValue = (obeliskMadnessValueMaster = AtOManager.Instance.GetObeliskMadness());
						SetObeliskMadnessLevel();
					}
					else if (SaveManager.PrefsHasKey("obeliskMadnessCoop"))
					{
						int num11 = SaveManager.LoadPrefsInt("obeliskMadnessCoop");
						if (PlayerManager.Instance.ObeliskMadnessLevel >= num11)
						{
							obeliskMadnessValue = (obeliskMadnessValueMaster = num11);
						}
						else
						{
							HeroSelectionManager heroSelectionManager9 = this;
							HeroSelectionManager heroSelectionManager10 = this;
							int num7 = 0;
							heroSelectionManager10.obeliskMadnessValueMaster = 0;
							heroSelectionManager9.obeliskMadnessValue = num7;
						}
						SetObeliskMadnessLevel();
					}
				}
			}
			else if (SaveManager.PrefsHasKey("obeliskMadness"))
			{
				int num12 = SaveManager.LoadPrefsInt("obeliskMadness");
				if (PlayerManager.Instance.ObeliskMadnessLevel >= num12)
				{
					obeliskMadnessValue = (obeliskMadnessValueMaster = num12);
				}
				else
				{
					HeroSelectionManager heroSelectionManager11 = this;
					HeroSelectionManager heroSelectionManager12 = this;
					int num7 = 0;
					heroSelectionManager12.obeliskMadnessValueMaster = 0;
					heroSelectionManager11.obeliskMadnessValue = num7;
				}
				SetObeliskMadnessLevel();
			}
		}
		else
		{
			titleMovement.SetText(Texts.Instance.GetText("modeWeekly"));
			madnessButton.gameObject.SetActive(value: false);
		}
		Resize();
		if (GameManager.Instance.IsWeeklyChallenge() && !GameManager.Instance.IsLoadingGame())
		{
			gameSeedModify.gameObject.SetActive(value: false);
			ChallengeData weeklyData2 = Globals.Instance.GetWeeklyData(Functions.GetCurrentWeeklyWeek());
			if (weeklyData2 != null)
			{
				heroSelectionDictionary[weeklyData2.Hero1.Id].AssignHeroToBox(boxGO[0]);
				charPopupMini.SetSubClassData(heroSelectionDictionary[weeklyData2.Hero1.Id].subClassData);
				heroSelectionDictionary[weeklyData2.Hero2.Id].AssignHeroToBox(boxGO[1]);
				heroSelectionDictionary[weeklyData2.Hero3.Id].AssignHeroToBox(boxGO[2]);
				heroSelectionDictionary[weeklyData2.Hero4.Id].AssignHeroToBox(boxGO[3]);
			}
			if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
			{
				if (weeklyData2 != null)
				{
					AtOManager.Instance.SetGameId(weeklyData2.Seed);
				}
				else
				{
					AtOManager.Instance.SetGameId();
				}
			}
			GameManager.Instance.SceneLoaded();
		}
		else if (GameManager.Instance.IsLoadingGame() || (AtOManager.Instance.IsFirstGame() && !GameManager.Instance.IsMultiplayer() && GameManager.Instance.IsGameAdventure()))
		{
			gameSeedModify.gameObject.SetActive(value: false);
			if (AtOManager.Instance.IsFirstGame())
			{
				AtOManager.Instance.SetGameId("tuto");
			}
		}
		else
		{
			if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
			{
				AtOManager.Instance.SetGameId();
			}
			gameSeed.gameObject.SetActive(value: true);
		}
		if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
		{
			SetSeed(AtOManager.Instance.GetGameId());
		}
		if (GameManager.Instance.IsWeeklyChallenge() || (GameManager.Instance.IsObeliskChallenge() && obeliskMadnessValue > 7) || AtOManager.Instance.IsCombatTool)
		{
			gameSeed.gameObject.SetActive(value: false);
		}
		playerHeroPerksDict = new Dictionary<string, List<string>>();
		if (GameManager.Instance.IsMultiplayer())
		{
			masterDescription.gameObject.SetActive(value: true);
			if (NetworkManager.Instance.IsMaster())
			{
				DrawBoxSelectionNames();
				botonBegin.gameObject.SetActive(value: true);
				botonBegin.Disable();
				botonFollow.transform.parent.gameObject.SetActive(value: false);
			}
			else
			{
				gameSeedModify.gameObject.SetActive(value: false);
				botonBegin.gameObject.SetActive(value: false);
				botonFollow.transform.parent.gameObject.SetActive(value: true);
				ShowFollowStatus();
			}
			if (NetworkManager.Instance.IsMaster() && GameManager.Instance.IsLoadingGame())
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int m = 0; m < 4; m++)
				{
					Hero hero = AtOManager.Instance.GetHero(m);
					if (hero == null || hero.HeroData == null)
					{
						continue;
					}
					string subclassName = hero.SubclassName;
					int perkRank = hero.PerkRank;
					string skinUsed = hero.SkinUsed;
					string cardbackUsed = hero.CardbackUsed;
					AddToPlayerHeroSkin(subclassName, skinUsed);
					AddToPlayerHeroCardback(subclassName, cardbackUsed);
					if (heroSelectionDictionary.ContainsKey(subclassName))
					{
						heroSelectionDictionary[subclassName].AssignHeroToBox(boxGO[m]);
						if (hero.HeroData.HeroSubClass.MainCharacter)
						{
							heroSelectionDictionary[subclassName].SetRankBox(perkRank);
							heroSelectionDictionary[subclassName].SetSkin(skinUsed);
						}
					}
					stringBuilder.Append(hero.SubclassName.ToLower());
					stringBuilder.Append("#");
					stringBuilder.Append(m);
					stringBuilder.Append("#");
					stringBuilder.Append(perkRank);
					stringBuilder.Append("#");
					stringBuilder.Append(skinUsed);
					stringBuilder.Append("#");
					stringBuilder.Append(cardbackUsed);
					stringBuilder.Append("&");
				}
				photonView.RPC("NET_AssignAllHeroToBox", RpcTarget.Others, stringBuilder.ToString());
			}
		}
		else
		{
			masterDescription.gameObject.SetActive(value: false);
			botonFollow.transform.parent.gameObject.SetActive(value: false);
			botonBegin.gameObject.SetActive(value: true);
			botonBegin.Disable();
			if (!GameManager.Instance.IsWeeklyChallenge())
			{
				PreAssign();
			}
		}
		RearrangeHerosData();
		ShowHeroesByFilterAsync("all");
		yield return Globals.Instance.WaitForSeconds(0.1f);
		RefreshSandboxButton();
		if (!GameManager.Instance.IsWeeklyChallenge())
		{
			sandboxButton.gameObject.SetActive(value: true);
			if (!GameManager.Instance.IsMultiplayer() || (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster()))
			{
				string sandboxMods;
				if (GameManager.Instance.GameStatus != Enums.GameStatus.LoadGame)
				{
					if ((!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster()) && PlayerManager.Instance.NgLevel == 0)
					{
						SandboxManager.Instance.DisableSandbox();
						AtOManager.Instance.ClearSandbox();
					}
					else
					{
						sandboxMods = (GameManager.Instance.IsObeliskChallenge() ? SaveManager.LoadPrefsString("sandboxSettingsObelisk") : SaveManager.LoadPrefsString("sandboxSettings"));
						AtOManager.Instance.SetSandboxMods(sandboxMods);
					}
					SandboxManager.Instance.LoadValuesFromAtOManager();
					SandboxManager.Instance.AdjustTotalHeroesBoxToCoop();
					SandboxManager.Instance.SaveValuesToAtOManager();
					sandboxMods = AtOManager.Instance.GetSandboxMods();
				}
				else
				{
					sandboxMods = AtOManager.Instance.GetSandboxMods();
					SandboxManager.Instance.LoadValuesFromAtOManager();
				}
				if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
				{
					photonView.RPC("NET_ShareSandbox", RpcTarget.Others, Functions.CompressString(sandboxMods));
				}
				RefreshCharBoxesBySandboxHeroes();
			}
		}
		else
		{
			sandboxButton.gameObject.SetActive(value: false);
			madnessButton.localPosition = new Vector3(3.8f, madnessButton.localPosition.y, madnessButton.localPosition.z);
			SandboxManager.Instance.DisableSandbox();
		}
		readyButtonText.gameObject.SetActive(value: false);
		readyButton.gameObject.SetActive(value: false);
		if (GameManager.Instance.IsMultiplayer())
		{
			if (NetworkManager.Instance.IsMaster())
			{
				NetworkManager.Instance.ClearAllPlayerManualReady();
				NetworkManager.Instance.SetManualReady(status: true);
			}
			else
			{
				readyButtonText.gameObject.SetActive(value: true);
				readyButton.gameObject.SetActive(value: true);
			}
		}
		GameManager.Instance.SceneLoaded();
		if (0 == 0 && !GameManager.Instance.TutorialWatched("characterPerks"))
		{
			foreach (KeyValuePair<string, HeroSelection> item5 in heroSelectionDictionary)
			{
				if (item5.Value.perkPointsT.gameObject.activeSelf)
				{
					GameManager.Instance.ShowTutorialPopup("characterPerks", item5.Value.perkPointsT.gameObject.transform.position, Vector3.zero);
					break;
				}
			}
		}
		if (GameManager.Instance.IsMultiplayer() && GameManager.Instance.IsLoadingGame() && NetworkManager.Instance.IsMaster())
		{
			bool flag = true;
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			for (int n = 0; n < 4; n++)
			{
				Hero hero2 = AtOManager.Instance.GetHero(n);
				if (hero2 != null && !(hero2.HeroData == null))
				{
					if (hero2.OwnerOriginal == null)
					{
						break;
					}
					string item = hero2.OwnerOriginal.ToLower();
					if (!list2.Contains(item))
					{
						list2.Add(item);
					}
				}
			}
			Player[] playerList = NetworkManager.Instance.PlayerList;
			foreach (Player player in playerList)
			{
				string item2 = NetworkManager.Instance.GetPlayerNickReal(player.NickName).ToLower();
				if (!list3.Contains(item2))
				{
					list3.Add(item2);
				}
			}
			if (list2.Count != list3.Count)
			{
				flag = false;
			}
			else
			{
				for (int num13 = 0; num13 < list3.Count; num13++)
				{
					if (!list2.Contains(list3[num13]))
					{
						flag = false;
						break;
					}
				}
			}
			if (!flag)
			{
				photonView.RPC("NET_SetNotOriginal", RpcTarget.All);
			}
		}
		SetCombatToolUI();
		SetCharacterSelectionTabCount();
	}

	private void SetCombatToolUI()
	{
		if (AtOManager.Instance.IsCombatTool)
		{
			GameManager.Instance.GameType = Enums.GameType.Adventure;
			CombatToolManager.Instance.CurrentCombat = null;
			sandboxButton.GetComponent<BotonGeneric>().Disable();
			sandboxButton.gameObject.SetActive(value: false);
			MadnessButton.GetComponent<BotonGeneric>().Disable();
			MadnessButton.gameObject.SetActive(value: false);
			gameSeedModify.gameObject.SetActive(value: false);
			MadnessManager.Instance.SelectMadness(0);
			ngValue = 0;
			ngCorruptors = "000000000";
			SetMadnessLevel();
			weeklyModifiersButton.gameObject.SetActive(value: false);
		}
	}

	private void Update()
	{
		if (setWeekly && Time.frameCount % 24 == 0)
		{
			SetWeeklyLeft();
		}
	}

	public void ForceWeekly(string _weekly)
	{
		AtOManager.Instance.weeklyForcedId = _weekly;
		if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
		{
			photonView.RPC("NET_ForceWeekly", RpcTarget.Others, _weekly);
		}
		SceneStatic.LoadByName("HeroSelection");
	}

	[PunRPC]
	private void NET_ForceWeekly(string _weekly)
	{
		ForceWeekly(_weekly);
	}

	public void IncreaseHeroProgressDev(int _slot)
	{
		if (!PerkTree.Instance.IsActive() && boxHero[boxGO[_slot]] != null)
		{
			PlayerManager.Instance.ModifyProgress(boxHero[boxGO[_slot]].Id, 5000, _slot);
			PlayerManager.Instance.ModifyPlayerRankProgress(5000);
			RefreshPerkPoints(boxHero[boxGO[_slot]].Id);
			SaveManager.SavePlayerData();
		}
	}

	public void IncreaseHeroProgressSupplies(string _scdId)
	{
		int num = 400;
		PlayerManager.Instance.ModifyProgress(_scdId, num);
		PlayerManager.Instance.ModifyPlayerRankProgress(num);
		RefreshPerkPoints(_scdId);
		SaveManager.SavePlayerData();
	}

	[PunRPC]
	private void NET_SetSku(string _nick, string _playerSkuList)
	{
		List<string> list = new List<string>();
		if (_playerSkuList != "")
		{
			list = JsonHelper.FromJson<string>(_playerSkuList).ToList();
		}
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("Got list from " + _nick + " --> " + _playerSkuList);
		}
		NetworkManager.Instance.AddPlayerSkuList(_nick, list);
	}

	[PunRPC]
	private void NET_SetNotOriginal()
	{
		masterDescription.GetComponent<TMP_Text>().text = Texts.Instance.GetText("notOriginalPlayers");
	}

	private void SetWeeklyLeft()
	{
		if (AtOManager.Instance.weeklyForcedId == "")
		{
			TimeSpan timeSpan = Functions.TimeSpanLeftWeekly();
			string text = $"{(int)timeSpan.TotalHours:D2}h. {timeSpan.Minutes:D2}m. {timeSpan.Seconds:D2}s.";
			weeklyLeft.text = text;
		}
		else
		{
			weeklyLeft.text = "-- [test] --";
		}
	}

	[PunRPC]
	private void NET_SetLoadingGame()
	{
		GameManager.Instance.SetGameStatus(Enums.GameStatus.LoadGame);
	}

	public void AssignHeroCardback(string _subclass, string _cardbackId)
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			SendHeroCardbackMP(_subclass, _cardbackId);
		}
		else
		{
			AddToPlayerHeroCardback(_subclass, _cardbackId);
		}
	}

	public void SendHeroCardbackMP(string _subclass, string _cardbackId)
	{
		Debug.Log("SendHeroCardback for " + _subclass + " => " + _cardbackId);
		if (GameManager.Instance.IsMultiplayer())
		{
			photonView.RPC("NET_SetHeroCardback", RpcTarget.All, NetworkManager.Instance.GetPlayerNick(), _subclass, _cardbackId);
		}
	}

	[PunRPC]
	private void NET_SetHeroCardback(string _nick, string _subclass, string _cardbackId)
	{
		Debug.Log("NET_SetHeroCardback " + _nick + " " + _subclass + " " + _cardbackId);
		SetHeroCardbackToBoxOwner(_nick, _subclass, _cardbackId);
	}

	private void SetHeroCardbackToBoxOwner(string _nick, string _subclass, string _cardbackId)
	{
		for (int i = 0; i < boxHero.Count; i++)
		{
			if (boxSelection[i].GetOwner() == _nick && boxHero[boxGO[i]] != null && boxHero[boxGO[i]].GetSubclassName().ToLower() == _subclass.ToLower())
			{
				AddToPlayerHeroCardback(_subclass, _cardbackId);
			}
		}
	}

	private void AddToPlayerHeroCardback(string _subclass, string _cardbackId)
	{
		string key = Functions.RemoveWhitespace(_subclass, toLower: true);
		if (!playerHeroCardbackDict.ContainsKey(key))
		{
			playerHeroCardbackDict.Add(key, _cardbackId);
		}
		else
		{
			playerHeroCardbackDict[key] = _cardbackId;
		}
	}

	public void SendHeroSkinMP(string _subclass, string _skinId)
	{
		Debug.Log("SendHeroSkinMP for " + _subclass + " => " + _skinId);
		if (GameManager.Instance.IsMultiplayer())
		{
			photonView.RPC("NET_SetHeroSkin", RpcTarget.All, NetworkManager.Instance.GetPlayerNick(), _subclass, _skinId);
		}
	}

	[PunRPC]
	private void NET_SetHeroSkin(string _nick, string _subclass, string _skinId)
	{
		Debug.Log("NET_SetHeroSkin " + _nick + " " + _subclass + " " + _skinId);
		SetHeroSkinToBoxOwner(_nick, _subclass, _skinId);
	}

	private void AddToPlayerHeroSkin(string _subclass, string _skinId)
	{
		string key = _subclass.ToLower();
		if (!playerHeroSkinsDict.ContainsKey(key))
		{
			playerHeroSkinsDict.Add(key, _skinId);
		}
		else
		{
			playerHeroSkinsDict[key] = _skinId;
		}
	}

	private void SetHeroSkinToBoxOwner(string _nick, string _subclass, string _skinId)
	{
		for (int i = 0; i < boxHero.Count; i++)
		{
			if (boxSelection[i].GetOwner() == _nick && boxHero[boxGO[i]] != null && boxHero[boxGO[i]].GetSubclassName().ToLower() == _subclass.ToLower())
			{
				boxHero[boxGO[i]].SetSkin(_skinId);
				AddToPlayerHeroSkin(_subclass, _skinId);
			}
		}
	}

	public void SendHeroPerksMP(string _hero)
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			List<string> heroPerks = PlayerManager.Instance.GetHeroPerks(_hero);
			if (heroPerks != null)
			{
				string text = JsonHelper.ToJson(heroPerks.ToArray());
				photonView.RPC("NET_SetHeroPerk", RpcTarget.All, NetworkManager.Instance.GetPlayerNick(), _hero, text);
			}
			else
			{
				photonView.RPC("NET_SetHeroPerk", RpcTarget.All, NetworkManager.Instance.GetPlayerNick(), _hero, "");
			}
		}
	}

	[PunRPC]
	private void NET_SetHeroPerk(string _nick, string _hero, string _perkListStr)
	{
		List<string> value = new List<string>();
		if (_perkListStr != "")
		{
			value = JsonHelper.FromJson<string>(_perkListStr).ToList();
		}
		string key = (_nick + "_" + _hero).ToLower();
		if (!playerHeroPerksDict.ContainsKey(key))
		{
			playerHeroPerksDict.Add(key, value);
		}
		else
		{
			playerHeroPerksDict[key] = value;
		}
		if (PerkTree.Instance.IsActive())
		{
			PerkTree.Instance.DoTeamPerks();
		}
	}

	private void PreAssign()
	{
		if (PlayerManager.Instance.LastUsedTeam == null || PlayerManager.Instance.LastUsedTeam.Length != 4)
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			if (heroSelectionDictionary.ContainsKey(PlayerManager.Instance.LastUsedTeam[i]) && (GameManager.Instance.IsObeliskChallenge() || (PlayerManager.Instance.IsHeroUnlocked(PlayerManager.Instance.LastUsedTeam[i]) && !heroSelectionDictionary[PlayerManager.Instance.LastUsedTeam[i]].DlcBlocked)))
			{
				heroSelectionDictionary[PlayerManager.Instance.LastUsedTeam[i]].AssignHeroToBox(boxGO[i]);
			}
		}
	}

	private void AlfaPreAssign()
	{
		if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
		{
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				AssignPlayerToBox(NetworkManager.Instance.PlayerList[num].NickName, i);
				num++;
				if (num >= NetworkManager.Instance.PlayerList.Length)
				{
					num = 0;
				}
			}
		}
		heroSelectionDictionary["mercenary"].AssignHeroToBox(boxGO[0]);
		heroSelectionDictionary["ranger"].AssignHeroToBox(boxGO[1]);
		heroSelectionDictionary["elementalist"].AssignHeroToBox(boxGO[2]);
		heroSelectionDictionary["cleric"].AssignHeroToBox(boxGO[3]);
	}

	private void DrawBoxSelectionNames()
	{
		int num = 0;
		Player[] playerList = NetworkManager.Instance.PlayerList;
		foreach (Player player in playerList)
		{
			for (int j = 0; j < 4; j++)
			{
				boxSelection[j].ShowPlayer(num);
				boxSelection[j].SetPlayerPosition(num, player.NickName);
			}
			num++;
		}
		for (int k = num; k < 4; k++)
		{
			for (int l = 0; l < 4; l++)
			{
				boxSelection[l].SetPlayerPosition(k, "");
			}
		}
		playerList = NetworkManager.Instance.PlayerList;
		foreach (Player player2 in playerList)
		{
			string playerNickReal = NetworkManager.Instance.GetPlayerNickReal(player2.NickName);
			if (playerNickReal == NetworkManager.Instance.Owner0)
			{
				AssignPlayerToBox(player2.NickName, 0);
			}
			if (playerNickReal == NetworkManager.Instance.Owner1)
			{
				AssignPlayerToBox(player2.NickName, 1);
			}
			if (playerNickReal == NetworkManager.Instance.Owner2)
			{
				AssignPlayerToBox(player2.NickName, 2);
			}
			if (playerNickReal == NetworkManager.Instance.Owner3)
			{
				AssignPlayerToBox(player2.NickName, 3);
			}
		}
	}

	public void AssignPlayerToBox(string playerNick, int boxId)
	{
		if (!GameManager.Instance.IsWeeklyChallenge() && !GameManager.Instance.IsLoadingGame())
		{
			ClearBox(boxId);
		}
		boxSelection[boxId].SetOwner(playerNick);
		if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
		{
			photonView.RPC("NET_AssignPlayerToBox", RpcTarget.Others, playerNick, boxId);
			NetworkManager.Instance.AssignHeroPlayerPositionOwner(boxId, playerNick);
		}
		if (GameManager.Instance.IsWeeklyChallenge() && playerNick == NetworkManager.Instance.GetPlayerNick())
		{
			ChallengeData weeklyData = Globals.Instance.GetWeeklyData(AtOManager.Instance.GetWeekly());
			if (weeklyData != null)
			{
				string text = "";
				switch (boxId)
				{
				case 0:
					AssignHeroToBox(weeklyData.Hero1.Id, 0);
					text = weeklyData.Hero1.Id;
					break;
				case 1:
					AssignHeroToBox(weeklyData.Hero2.Id, 1);
					text = weeklyData.Hero2.Id;
					break;
				case 2:
					AssignHeroToBox(weeklyData.Hero3.Id, 2);
					text = weeklyData.Hero3.Id;
					break;
				default:
					AssignHeroToBox(weeklyData.Hero4.Id, 3);
					text = weeklyData.Hero4.Id;
					break;
				}
				if (!GameManager.Instance.IsLoadingGame())
				{
					SubClassData subClassData = Globals.Instance.GetSubClassData(text);
					string activeSkin = PlayerManager.Instance.GetActiveSkin(subClassData.Id);
					heroSelectionDictionary[text].SetSkin(activeSkin);
				}
			}
		}
		CheckButtonEnabled();
		SetPlayersReady();
	}

	[PunRPC]
	private void NET_AssignPlayerToBox(string playerNick, int boxId)
	{
		AssignPlayerToBox(playerNick, boxId);
	}

	public void ResetHero(string _heroId)
	{
		photonView.RPC("NET_ResetHero", RpcTarget.Others, _heroId);
	}

	[PunRPC]
	private void NET_ResetHero(string _heroId)
	{
		for (int i = 0; i < boxHero.Count; i++)
		{
			if (boxHero[boxGO[i]] != null && boxHero[boxGO[i]].Id == _heroId)
			{
				ClearBox(i);
				break;
			}
		}
	}

	public bool IsYourBox(string boxName)
	{
		if (!GameManager.Instance.IsMultiplayer())
		{
			return true;
		}
		int num = int.Parse(boxName.Split('_')[1]);
		if (boxSelection[num].GetOwner() == NetworkManager.Instance.GetPlayerNick())
		{
			return true;
		}
		return false;
	}

	public void PopupTrait(string traitId)
	{
		TraitData traitData = Globals.Instance.GetTraitData(traitId);
		PopupManager.Instance.SetTrait(traitData);
	}

	public void Resize()
	{
		charPopup.RepositionResolution();
	}

	public void ShowMask(bool state)
	{
		if (coroutineMask != null)
		{
			StopCoroutine(coroutineMask);
		}
		coroutineMask = StartCoroutine(ShowMaskCo(state));
	}

	private IEnumerator ShowMaskCo(bool state)
	{
		Color colorBg = new Color(0f, 0f, 0f, 0f);
		SpriteRenderer imageBg = popupMask.GetComponent<SpriteRenderer>();
		float index = imageBg.color.a;
		float maxAlplha = 0.5f;
		float increment = 0.01f;
		if (!state)
		{
			while (index > 0f)
			{
				colorBg.a = index;
				imageBg.color = colorBg;
				index -= increment;
				yield return null;
			}
			colorBg.a = 0f;
			popupMask.gameObject.SetActive(value: false);
		}
		else
		{
			popupMask.gameObject.SetActive(value: true);
			while (index < maxAlplha)
			{
				colorBg.a = index;
				imageBg.color = colorBg;
				index += increment;
				yield return null;
			}
			colorBg.a = maxAlplha;
		}
		imageBg.color = colorBg;
	}

	public bool IsBoxFilled(GameObject _box)
	{
		if (boxFilled.ContainsKey(_box))
		{
			return boxFilled[_box];
		}
		boxFilled[_box] = false;
		return false;
	}

	private bool AllBoxWithHeroes()
	{
		if (!GameManager.Instance.IsWeeklyChallenge() && SandboxManager.Instance.IsEnabled())
		{
			switch (SandboxManager.Instance.GetSandboxBoxValue("sbTotalHeroes"))
			{
			case 3:
				if (!boxFilled[boxGO[0]] || !boxFilled[boxGO[1]] || !boxFilled[boxGO[2]])
				{
					return false;
				}
				return true;
			case 2:
				if (!boxFilled[boxGO[0]] || !boxFilled[boxGO[1]])
				{
					return false;
				}
				return true;
			case 1:
				if (!boxFilled[boxGO[0]])
				{
					return false;
				}
				return true;
			}
		}
		if (boxFilled.Count > 0)
		{
			int num = 0;
			foreach (GameObject key in boxFilled.Keys)
			{
				if (boxFilled[key])
				{
					num++;
				}
			}
			return num == 4;
		}
		return false;
	}

	private bool AllBoxWithOwners()
	{
		if (!GameManager.Instance.IsMultiplayer())
		{
			return true;
		}
		int num = 4;
		if (!GameManager.Instance.IsObeliskChallenge() && SandboxManager.Instance.IsEnabled() && SandboxManager.Instance.GetSandboxBoxValue("sbTotalHeroes") > 0)
		{
			num = SandboxManager.Instance.GetSandboxBoxValue("sbTotalHeroes");
		}
		int num2 = 0;
		for (int i = 0; i < 4; i++)
		{
			if (boxSelection[i].gameObject.activeSelf && boxSelection[i].GetOwner() != null && boxSelection[i].GetOwner() != "")
			{
				num2++;
			}
		}
		return num2 == num;
	}

	public void ShowDrag(bool state, Vector3 position)
	{
		cursorDragPrefab.gameObject.SetActive(state);
		if (state)
		{
			cursorDragPrefab.transform.position = position;
			GameManager.Instance.PlayLibraryAudio("castnpccardfast", 0.1f);
		}
	}

	public void FillBox(GameObject _box, HeroSelection _heroSelection, bool _state)
	{
		boxFilled[_box] = _state;
		boxHero[_box] = _heroSelection;
		int num = int.Parse(_box.name.Split('_')[1]);
		if (_heroSelection != null)
		{
			string text = SubclassByName[_heroSelection.Id];
			SubClassData subClassData = Globals.Instance.GetSubClassData(text);
			if (IsYourBox("Box_" + int.Parse(_box.name.Split('_')[1])))
			{
				AddToPlayerHeroCardback(text, PlayerManager.Instance.GetActiveCardback(subClassData.Id));
			}
			if (GameManager.Instance.IsMultiplayer())
			{
				AssignHeroToBox(text, num);
			}
		}
		else if (GameManager.Instance.IsMultiplayer())
		{
			ClearBox(num, moveBackHero: false);
			photonView.RPC("NET_ClearBox", RpcTarget.Others, num, false);
		}
		CheckButtonEnabled();
	}

	private void CheckButtonEnabled()
	{
		bool flag = true;
		if (GameManager.Instance.IsMultiplayer())
		{
			flag = false;
			List<string> list = new List<string>();
			for (int i = 0; i < 4; i++)
			{
				if (boxSelection[i].gameObject.activeSelf)
				{
					string owner = boxSelection[i].GetOwner();
					if (owner == "")
					{
						flag = false;
						break;
					}
					if (!list.Contains(owner))
					{
						list.Add(owner);
					}
				}
			}
			if (list.Count == NetworkManager.Instance.PlayerList.Length)
			{
				flag = true;
			}
			if (flag && !NetworkManager.Instance.AllPlayersManualReady())
			{
				flag = false;
			}
		}
		if (flag && AllBoxWithOwners() && AllBoxWithHeroes())
		{
			botonBegin.Enable();
		}
		else
		{
			botonBegin.Disable();
		}
	}

	private void AssignHeroToBox(string _hero, int _boxId)
	{
		if (IsYourBox("Box_" + _boxId) && !GameManager.Instance.IsLoadingGame())
		{
			SubClassData subClassData = Globals.Instance.GetSubClassData(_hero);
			subClassData.CharacterName.ToLower();
			string text = subClassData.Id.ToLower();
			if (SubclassByName.ContainsKey(text))
			{
				text = SubclassByName[text];
			}
			text = text.ToLower().Replace(" ", "");
			int perkRank = PlayerManager.Instance.GetPerkRank(subClassData.Id);
			heroSelectionDictionary[text].SetRankBox(perkRank);
			AddToPlayerHeroCardback(_hero, PlayerManager.Instance.GetActiveCardback(subClassData.Id));
			if (GameManager.Instance.IsMultiplayer())
			{
				photonView.RPC("NET_AssignHeroToBox", RpcTarget.Others, _hero, _boxId, perkRank, PlayerManager.Instance.GetActiveSkin(subClassData.Id), PlayerManager.Instance.GetActiveCardback(subClassData.Id));
				SendHeroPerksMP(subClassData.Id);
			}
		}
		if (PerkTree.Instance.IsActive())
		{
			PerkTree.Instance.DoTeamPerks();
		}
		if (GameManager.Instance.IsMultiplayer() && GameManager.Instance.IsLoadingGame() && !GameManager.Instance.IsObeliskChallenge() && NetworkManager.Instance.IsMaster())
		{
			for (int i = 0; i < boxSelection.Length; i++)
			{
				boxSelection[i].CheckSkuForHero();
			}
		}
	}

	[PunRPC]
	private void NET_AssignAllHeroToBox(string _str)
	{
		string[] array = _str.Split('&');
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].IsNullOrEmpty())
			{
				string[] array2 = array[i].Split("#");
				if (array2.Length == 5)
				{
					NET_AssignHeroToBox(array2[0], int.Parse(array2[1]), int.Parse(array2[2]), array2[3], array2[4]);
				}
			}
		}
		RearrangeHerosForMultiplayer();
		ShowHeroesByFilterAsync(CurrentFilter);
	}

	[PunRPC]
	private void NET_AssignHeroToBox(string _hero, int _boxId, int _perkRank, string _skinId, string _cardbackId)
	{
		_hero = _hero.ToLower();
		if (SubclassByName.ContainsKey(_hero))
		{
			_hero = SubclassByName[_hero];
		}
		_hero = _hero.ToLower().Replace(" ", "");
		GameObject key = boxGO[_boxId];
		if (heroSelectionDictionary[_hero].selected)
		{
			heroSelectionDictionary[_hero].Reset();
		}
		if (!boxHero.ContainsKey(key) || boxHero[key] == null)
		{
			heroSelectionDictionary[_hero].AssignHeroToBox(boxGO[_boxId]);
			heroSelectionDictionary[_hero].SetRankBox(_perkRank);
			heroSelectionDictionary[_hero].SetSkin(_skinId);
			AddToPlayerHeroSkin(_hero, _skinId);
			AddToPlayerHeroCardback(_hero, _cardbackId);
		}
		else if (boxHero[key].nameTM.text != _hero)
		{
			boxHero[key].GoBackToOri();
			heroSelectionDictionary[_hero].AssignHeroToBox(boxGO[_boxId]);
			heroSelectionDictionary[_hero].SetRankBox(_perkRank);
			heroSelectionDictionary[_hero].SetSkin(_skinId);
			AddToPlayerHeroSkin(_hero, _skinId);
			AddToPlayerHeroCardback(_hero, _cardbackId);
		}
		RearrangeHerosForMultiplayer();
		ShowHeroesByFilterAsync(CurrentFilter);
	}

	public void RearrangeHerosData()
	{
		heroSelectionDictionary = (from kv in heroSelectionDictionary
			orderby kv.Value.lockIcon.gameObject.activeSelf, kv.Value.HeroPicked descending, kv.Value.subClassData.CharacterName
			select kv).ToDictionary((KeyValuePair<string, HeroSelection> kv) => kv.Key, (KeyValuePair<string, HeroSelection> kv) => kv.Value);
	}

	public void RearrangeHerosForMultiplayer()
	{
		RearrangeHerosData();
		GetHeroSelectionParentGO(CurrentFilter);
		GetHeroSelectionFilter(CurrentFilter);
	}

	public async void ShowHeroesByFilterAsync(string type)
	{
		if (type != null)
		{
			CurrentFilter = type;
			RearrangeHerosData();
			DisableCharacterSelectionParents();
			HeroSelectionTabsManager.Instance.EnableTab(type);
			GameObject heroSelectionParentGO = GetHeroSelectionParentGO(type);
			Func<HeroSelection, bool> heroSelectionFilter = GetHeroSelectionFilter(type);
			ArrangeHeroSelections(heroSelectionParentGO, heroSelectionFilter);
			currentSelectionTab = heroSelectionParentGO;
			heroSelectionParentGO.SetActive(value: true);
			await UpdateCharSelectArrowStates(100);
		}
	}

	public async Task UpdateCharSelectArrowStates(int delay = 0)
	{
		await Task.Delay(delay);
		GameObject activeHeroSelectionTab = GetActiveHeroSelectionTab();
		BotonGeneric component = charPageLeftButton.GetComponent<BotonGeneric>();
		BotonGeneric component2 = charPageRightButton.GetComponent<BotonGeneric>();
		Color backgroundColor = Functions.HexToColor("#FFBB00");
		Color backgroundColor2 = Functions.HexToColor("#9D9D9D");
		int childCount = activeHeroSelectionTab.transform.childCount;
		if (childCount == 1)
		{
			component.SetBackgroundColor(backgroundColor2);
			component2.SetBackgroundColor(backgroundColor2);
			component.buttonEnabled = false;
			component2.buttonEnabled = false;
		}
		else if (childCount > 1)
		{
			if (activeHeroSelectionTab.transform.GetChild(0).gameObject.activeSelf)
			{
				component.SetBackgroundColor(backgroundColor2);
				component2.SetBackgroundColor(backgroundColor);
				component.buttonEnabled = false;
				component2.buttonEnabled = true;
			}
			else if (activeHeroSelectionTab.transform.GetChild(childCount - 1).gameObject.activeSelf)
			{
				component.SetBackgroundColor(backgroundColor);
				component2.SetBackgroundColor(backgroundColor2);
				component.buttonEnabled = true;
				component2.buttonEnabled = false;
			}
			else
			{
				component.SetBackgroundColor(backgroundColor);
				component2.SetBackgroundColor(backgroundColor);
				component.buttonEnabled = true;
				component2.buttonEnabled = true;
			}
		}
	}

	private GameObject GetActiveHeroSelectionTab()
	{
		if (dlcsGO.activeSelf)
		{
			return dlcsGO;
		}
		if (lockedGO.activeSelf)
		{
			return lockedGO;
		}
		if (warriorsGO.activeSelf)
		{
			return warriorsGO;
		}
		if (scoutsGO.activeSelf)
		{
			return scoutsGO;
		}
		if (magesGO.activeSelf)
		{
			return magesGO;
		}
		if (healersGO.activeSelf)
		{
			return healersGO;
		}
		return allGO;
	}

	private GameObject GetHeroSelectionParentGO(string type)
	{
		return type.ToLower() switch
		{
			"dlc" => dlcsGO, 
			"locked" => lockedGO, 
			"warrior" => warriorsGO, 
			"scout" => scoutsGO, 
			"mage" => magesGO, 
			"healer" => healersGO, 
			_ => allGO, 
		};
	}

	private void ArrangeHeroSelections(GameObject parent, Func<HeroSelection, bool> filter)
	{
		Vector3 vector = new Vector3(2f, 0f, 0f);
		Vector3 vector2 = new Vector3(-0.45f, 0.09f, 0f);
		int num = 0;
		int num2 = 0;
		bool flag = false;
		if (parent.transform.childCount == 0)
		{
			flag = true;
		}
		if (flag)
		{
			CreateChildInTransform(parent.transform, "Page" + num2);
		}
		foreach (KeyValuePair<string, HeroSelection> item in heroSelectionDictionary)
		{
			HeroSelection value = item.Value;
			if (!filter(value))
			{
				continue;
			}
			Transform defaultParent = value.DefaultParent;
			defaultParent.parent = parent.transform.GetChild(num2);
			defaultParent.gameObject.SetActive(value: true);
			defaultParent.localPosition = Vector3.zero + vector2;
			num++;
			vector2 += vector;
			if (num % 8 == 0)
			{
				vector2 = new Vector3(-0.45f, -1.52f, 0f);
			}
			if (num % 16 == 0)
			{
				vector2 = new Vector3(-0.45f, 0.09f, 0f);
				num2++;
				if (flag)
				{
					CreateChildInTransform(parent.transform, "Page" + num2, enable: false);
				}
			}
		}
	}

	private Func<HeroSelection, bool> GetHeroSelectionFilter(string type)
	{
		string typeLower = type.ToLower();
		if (typeLower == "all")
		{
			return (HeroSelection _) => true;
		}
		if (typeLower == "dlc")
		{
			return (HeroSelection hero) => hero.GetHeroClass().ToLower() != "none" && hero.GetHeroClassSecondary().ToLower() != "none";
		}
		if (typeLower == "locked")
		{
			return (HeroSelection hero) => hero.blocked;
		}
		return (HeroSelection hero) => hero.GetHeroClass().ToLower() == typeLower || hero.GetHeroClassSecondary().ToLower() == typeLower;
	}

	private void DisableCharacterSelectionParents()
	{
		allGO.SetActive(value: false);
		warriorsGO.SetActive(value: false);
		scoutsGO.SetActive(value: false);
		magesGO.SetActive(value: false);
		healersGO.SetActive(value: false);
		dlcsGO.SetActive(value: false);
		lockedGO.SetActive(value: false);
	}

	private void SetCharacterSelectionTabCount()
	{
		string[] array = new string[6] { "all", "warrior", "scout", "mage", "healer", "dlc" };
		for (int i = 0; i < array.Length; i++)
		{
			int num = 0;
			Func<HeroSelection, bool> heroSelectionFilter = GetHeroSelectionFilter(array[i]);
			foreach (KeyValuePair<string, HeroSelection> item in heroSelectionDictionary)
			{
				HeroSelection value = item.Value;
				if (heroSelectionFilter(value))
				{
					num++;
				}
			}
			HeroSelectionTabsManager.Instance.SetTabText(num.ToString(), i);
		}
	}

	private void CreateChildInTransform(Transform _transform, string name, bool enable = true)
	{
		GameObject obj = new GameObject(name);
		obj.transform.parent = _transform;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localScale = Vector3.one;
		obj.transform.localRotation = Quaternion.identity;
		obj.SetActive(enable);
	}

	public void moveHeroPageLeft()
	{
		int childCount = currentSelectionTab.transform.childCount;
		if (currentSelectionTab.transform.childCount <= 1)
		{
			return;
		}
		for (int i = 1; i < childCount; i++)
		{
			if (currentSelectionTab.transform.GetChild(i).gameObject.activeSelf)
			{
				currentSelectionTab.transform.GetChild(i).gameObject.SetActive(value: false);
				currentSelectionTab.transform.GetChild(i - 1).gameObject.SetActive(value: true);
			}
		}
	}

	public void moveHeroPageRight()
	{
		int childCount = currentSelectionTab.transform.childCount;
		if (currentSelectionTab.transform.childCount <= 1)
		{
			return;
		}
		for (int i = 0; i < childCount - 1; i++)
		{
			if (currentSelectionTab.transform.GetChild(i).gameObject.activeSelf)
			{
				currentSelectionTab.transform.GetChild(i).gameObject.SetActive(value: false);
				currentSelectionTab.transform.GetChild(i + 1).gameObject.SetActive(value: true);
			}
		}
	}

	[PunRPC]
	private void NET_ClearBox(int _boxId, bool _moveBackHero)
	{
		ClearBox(_boxId, _moveBackHero);
		if (PerkTree.Instance.IsActive())
		{
			PerkTree.Instance.DoTeamPerks();
		}
	}

	public void ClearBox(int id, bool moveBackHero = true)
	{
		if (IsBoxFilled(boxGO[id]))
		{
			HeroSelection heroSelection = GetBoxHero(boxGO[id]);
			if (heroSelection != null)
			{
				heroSelection.GoBackToOri();
			}
			FillBox(boxGO[id], null, _state: false);
		}
	}

	public bool IsHeroSelected(string heroName)
	{
		for (int i = 0; i < 4; i++)
		{
			HeroSelection heroSelection = GetBoxHero(boxGO[i]);
			if (heroSelection != null && heroSelection.nameTM.text == heroName)
			{
				return true;
			}
		}
		return false;
	}

	public string GetBoxOwnerFromIndex(int _index)
	{
		if (boxSelection[_index] != null)
		{
			return boxSelection[_index].GetOwner();
		}
		return "";
	}

	public HeroSelection GetBoxHeroFromIndex(int _index)
	{
		if (boxGO[_index] != null && boxHero.ContainsKey(boxGO[_index]))
		{
			return boxHero[boxGO[_index]];
		}
		return null;
	}

	public HeroSelection GetBoxHero(GameObject _box)
	{
		if (boxHero.ContainsKey(_box))
		{
			return boxHero[_box];
		}
		return null;
	}

	public void MouseOverBox(GameObject _box)
	{
		box = _box;
	}

	public GameObject GetOverBox()
	{
		return box;
	}

	public void BeginAdventure()
	{
		botonBegin.gameObject.SetActive(value: false);
		if (GameManager.Instance.IsMultiplayer() && (!GameManager.Instance.IsMultiplayer() || !NetworkManager.Instance.IsMaster()))
		{
			return;
		}
		if (GameManager.Instance.GameStatus == Enums.GameStatus.LoadGame)
		{
			if (!AtOManager.Instance.CheckLoadGameUserHaveAllContent())
			{
				botonBegin.gameObject.SetActive(value: true);
			}
			else
			{
				AtOManager.Instance.DoLoadGameFromMP();
			}
			return;
		}
		string[] array = new string[4];
		for (int i = 0; i < boxHero.Count; i++)
		{
			if (boxHero[boxGO[i]] != null)
			{
				array[i] = Functions.RemoveWhitespace(boxHero[boxGO[i]].GetSubclassName(), toLower: true);
			}
			else
			{
				array[i] = "";
			}
		}
		if (!GameManager.Instance.IsMultiplayer() && !GameManager.Instance.IsWeeklyChallenge())
		{
			PlayerManager.Instance.LastUsedTeam = new string[4];
			for (int j = 0; j < 4; j++)
			{
				PlayerManager.Instance.LastUsedTeam[j] = array[j].ToLower();
			}
			SaveManager.SavePlayerData();
		}
		if (GameManager.Instance.IsGameAdventure())
		{
			AtOManager.Instance.SetPlayerPerks(playerHeroPerksDict, array);
			AtOManager.Instance.SetNgPlus(ngValue);
			AtOManager.Instance.SetMadnessCorruptors(ngCorruptors);
		}
		else if (GameManager.Instance.IsSingularity())
		{
			AtOManager.Instance.SetPlayerPerks(playerHeroPerksDict, array);
			AtOManager.Instance.SetSingularityMadness(singularityMadnessValue);
		}
		else if (!GameManager.Instance.IsWeeklyChallenge())
		{
			AtOManager.Instance.SetObeliskMadness(obeliskMadnessValue);
		}
		AtOManager.Instance.SetTeamFromArray(array);
		if (AtOManager.Instance.IsCombatTool)
		{
			SceneStatic.LoadByName("Town");
			CombatToolManager.Instance.SaveCombatToolConfig();
		}
		else
		{
			AtOManager.Instance.BeginAdventure();
		}
	}

	public void ChangeSeed()
	{
		if (!GameManager.Instance.IsLoadingGame() && !AtOManager.Instance.IsFirstGame() && (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster()))
		{
			AlertManager.buttonClickDelegate = SetSeedId;
			AlertManager.Instance.AlertInput(Texts.Instance.GetText("gameSeedInput"), Texts.Instance.GetText("accept").ToUpper());
		}
	}

	public void SetSeedId()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(SetSeedId));
		if (AlertManager.Instance.GetInputValue() != null)
		{
			string text = AlertManager.Instance.GetInputValue().ToUpper();
			if (text.Trim() != "")
			{
				SetSeed(text, stablishGameId: true);
			}
		}
	}

	private void SetSeed(string _seed, bool stablishGameId = false)
	{
		gameSeedTxt.text = _seed;
		if (stablishGameId)
		{
			AtOManager.Instance.SetGameId(_seed);
		}
		if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
		{
			photonView.RPC("NET_SetSeed", RpcTarget.Others, _seed);
		}
	}

	[PunRPC]
	private void NET_SetSeed(string _seed)
	{
		if (GameManager.Instance.GetDeveloperMode())
		{
			Debug.Log("Net_SeetSeed " + _seed);
		}
		gameSeedTxt.text = _seed;
	}

	public void RefreshPerkPoints(string _hero)
	{
		heroSelectionDictionary[_hero].SetPerkPoints();
		if (charPopup != null && PerkTree.Instance.IsActive())
		{
			charPopup.RefreshBecauseOfPerks();
		}
	}

	public void DoCharPopMenu(string type)
	{
		switch (type)
		{
		case "stats":
			charPopup.ShowStats();
			break;
		case "perks":
			charPopup.ShowPerks();
			break;
		case "skins":
			charPopup.ShowSkins();
			break;
		case "rank":
			charPopup.ShowRank();
			break;
		case "cardbacks":
			charPopup.ShowCardbacks();
			break;
		case "singularityCards":
			charPopup.ShowSingularityCards();
			break;
		}
		charPopup.Show();
	}

	public void NGBox(int value = -1)
	{
	}

	[PunRPC]
	private void NET_SetNGBox(int _value)
	{
		NGBox(_value);
	}

	public void SetMadnessLevel()
	{
		int num = MadnessManager.Instance.CalculateMadnessTotal(ngValue, ngCorruptors);
		madnessLevel.text = string.Format(Texts.Instance.GetText("madnessNumber"), num);
		if (num == 0)
		{
			madnessParticle.gameObject.SetActive(value: false);
			madnessButton.GetComponent<BotonGeneric>().ShowBackgroundDisable(state: true);
		}
		else
		{
			madnessParticle.gameObject.SetActive(value: true);
			madnessButton.GetComponent<BotonGeneric>().ShowBackgroundDisable(state: false);
		}
		MadnessManager.Instance.RefreshValues(ngCorruptors);
		if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
		{
			photonView.RPC("NET_SetMadness", RpcTarget.Others, ngValue, ngCorruptors);
		}
	}

	[PunRPC]
	private void NET_SetMadness(int mLevel, string mCorruptors)
	{
		ngValue = (ngValueMaster = mLevel);
		ngCorruptors = mCorruptors;
		SetMadnessLevel();
	}

	public void RefreshSandboxButton()
	{
		if (sandboxButton.gameObject.activeSelf)
		{
			if (SandboxManager.Instance.IsEnabled())
			{
				sandboxButton.GetComponent<BotonGeneric>().ShowBackgroundDisable(state: false);
				sandboxButtonCircleOn.gameObject.SetActive(value: true);
				sandboxButtonCircleOff.gameObject.SetActive(value: false);
			}
			else
			{
				sandboxButton.GetComponent<BotonGeneric>().ShowBackgroundDisable(state: true);
				sandboxButtonCircleOn.gameObject.SetActive(value: false);
				sandboxButtonCircleOff.gameObject.SetActive(value: true);
			}
		}
	}

	public void RefreshCharBoxesBySandboxHeroes()
	{
		if (SandboxManager.Instance.IsEnabled())
		{
			switch (SandboxManager.Instance.GetSandboxBoxValue("sbTotalHeroes"))
			{
			case 1:
				ClearBox(1, moveBackHero: false);
				ClearBox(2, moveBackHero: false);
				ClearBox(3, moveBackHero: false);
				boxSelection[1].gameObject.SetActive(value: false);
				boxSelection[2].gameObject.SetActive(value: false);
				boxSelection[3].gameObject.SetActive(value: false);
				CheckButtonEnabled();
				return;
			case 2:
				ClearBox(2, moveBackHero: false);
				ClearBox(3, moveBackHero: false);
				boxSelection[1].gameObject.SetActive(value: true);
				boxSelection[2].gameObject.SetActive(value: false);
				boxSelection[3].gameObject.SetActive(value: false);
				CheckButtonEnabled();
				return;
			case 3:
				ClearBox(3, moveBackHero: false);
				boxSelection[1].gameObject.SetActive(value: true);
				boxSelection[2].gameObject.SetActive(value: true);
				boxSelection[3].gameObject.SetActive(value: false);
				CheckButtonEnabled();
				return;
			}
		}
		boxSelection[1].gameObject.SetActive(value: true);
		boxSelection[2].gameObject.SetActive(value: true);
		boxSelection[3].gameObject.SetActive(value: true);
		CheckButtonEnabled();
	}

	public void ShareResetSandbox()
	{
		photonView.RPC("NET_ShareResetSandbox", RpcTarget.Others);
	}

	[PunRPC]
	private void NET_ShareResetSandbox()
	{
		SandboxManager.Instance.SbReset();
	}

	[PunRPC]
	private void NET_ShareSandbox(string json)
	{
		AtOManager.Instance.SetSandboxMods(Functions.DecompressString(json));
		SandboxManager.Instance.LoadValuesFromAtOManager();
		RefreshCharBoxesBySandboxHeroes();
	}

	public void ShareSandboxCombo(string key, int value)
	{
		photonView.RPC("NET_ShareSandboxCombo", RpcTarget.Others, key, value);
	}

	[PunRPC]
	private void NET_ShareSandboxCombo(string key, int value)
	{
		SandboxManager.Instance.SetComboValueByVal(key, value);
	}

	public void ShareSandboxBox(string key, int value)
	{
		photonView.RPC("NET_ShareSandboxBox", RpcTarget.Others, key, value);
	}

	[PunRPC]
	private void NET_ShareSandboxBox(string key, int value)
	{
		SandboxManager.Instance.SetBoxValueByVal(key, value);
		RefreshCharBoxesBySandboxHeroes();
	}

	public void ShareSandboxEnabledState(bool state)
	{
		photonView.RPC("NET_ShareSandboxEnabledState", RpcTarget.Others, state);
	}

	[PunRPC]
	private void NET_ShareSandboxEnabledState(bool state)
	{
		if (state)
		{
			SandboxManager.Instance.EnableSandbox();
		}
		else
		{
			SandboxManager.Instance.DisableSandbox();
		}
		RefreshSandboxButton();
	}

	public void SetObeliskMadnessLevel()
	{
		int num = obeliskMadnessValue;
		madnessLevel.text = string.Format(Texts.Instance.GetText("madnessNumber"), num);
		if (num == 0)
		{
			madnessParticle.gameObject.SetActive(value: false);
		}
		else
		{
			madnessParticle.gameObject.SetActive(value: true);
		}
		MadnessManager.Instance.RefreshValues();
		if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
		{
			photonView.RPC("NET_SetObeliskMadness", RpcTarget.Others, num);
		}
		if (obeliskMadnessValue > 7)
		{
			gameSeed.gameObject.SetActive(value: false);
		}
		else
		{
			gameSeed.gameObject.SetActive(value: true);
		}
	}

	[PunRPC]
	private void NET_SetObeliskMadness(int mLevel)
	{
		obeliskMadnessValue = (obeliskMadnessValueMaster = mLevel);
		SetObeliskMadnessLevel();
	}

	public void SetSingularityMadnessLevel()
	{
		int num = singularityMadnessValue;
		madnessLevel.text = string.Format(Texts.Instance.GetText("madnessNumber"), num);
		if (num == 0)
		{
			madnessParticle.gameObject.SetActive(value: false);
		}
		else
		{
			madnessParticle.gameObject.SetActive(value: true);
		}
		MadnessManager.Instance.RefreshValues();
		if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
		{
			photonView.RPC("NET_SetSingularityMadness", RpcTarget.Others, num);
		}
		if (singularityMadnessValue > 7)
		{
			gameSeed.gameObject.SetActive(value: false);
		}
		else
		{
			gameSeed.gameObject.SetActive(value: true);
		}
	}

	[PunRPC]
	private void NET_SetSingularityMadness(int mLevel)
	{
		singularityMadnessValue = (singularityMadnessValueMaster = mLevel);
		SetSingularityMadnessLevel();
	}

	public void SetSkinIntoSubclassData(string _subclass, string _skinId)
	{
		SubClassData subClassData = Globals.Instance.GetSubClassData(_subclass);
		SkinData skinData = Globals.Instance.GetSkinData(_skinId);
		foreach (KeyValuePair<string, HeroSelection> item in heroSelectionDictionary)
		{
			if (item.Key == subClassData.Id)
			{
				if (!GameManager.Instance.IsMultiplayer())
				{
					item.Value.SetSprite(skinData.SpritePortrait, skinData.SpriteSilueta, subClassData.SpriteBorderLocked);
					AddToPlayerHeroSkin(_subclass, _skinId);
				}
				else
				{
					item.Value.SetSpriteSilueta(skinData.SpriteSilueta);
					SendHeroSkinMP(_subclass, _skinId);
				}
				break;
			}
		}
	}

	public void FollowTheLeader()
	{
		AtOManager.Instance.followingTheLeader = !AtOManager.Instance.followingTheLeader;
		SaveManager.SaveIntoPrefsBool("followLeader", AtOManager.Instance.followingTheLeader);
		SaveManager.SavePrefs();
		ShowFollowStatus();
	}

	public void ShowFollowStatus()
	{
		if (AtOManager.Instance.followingTheLeader)
		{
			botonFollow.SetText("X");
		}
		else
		{
			botonFollow.SetText("");
		}
	}

	public void Ready()
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			if (manualReadyCo != null)
			{
				StopCoroutine(manualReadyCo);
			}
			statusReady = !statusReady;
			NetworkManager.Instance.SetManualReady(statusReady);
			if (statusReady)
			{
				ReadySetButton(state: true);
			}
			else
			{
				ReadySetButton(state: false);
			}
		}
	}

	public void SetPlayersReady()
	{
		for (int i = 0; i < 4; i++)
		{
			boxSelection[i].SetReady(NetworkManager.Instance.IsPlayerReady(boxSelection[i].GetOwner()));
		}
		if (NetworkManager.Instance.IsMaster())
		{
			CheckButtonEnabled();
		}
	}

	public void ReadySetButton(bool state)
	{
		if (state)
		{
			readyButtonText.gameObject.SetActive(value: false);
			readyButton.GetComponent<BotonGeneric>().SetColorAbsolute(Functions.HexToColor("#15A42E"));
			if (GameManager.Instance.IsMultiplayer())
			{
				readyButton.GetComponent<BotonGeneric>().SetText(Texts.Instance.GetText("waitingForPlayers"));
			}
			else
			{
				readyButton.GetComponent<BotonGeneric>().SetText(Texts.Instance.GetText("ready"));
			}
		}
		else
		{
			readyButtonText.gameObject.SetActive(value: true);
			readyButton.GetComponent<BotonGeneric>().SetColorAbsolute(Functions.HexToColor(Globals.Instance.ClassColor["warrior"]));
			readyButton.GetComponent<BotonGeneric>().SetText(Texts.Instance.GetText("ready"));
		}
	}

	public void SelectTab(string TabName)
	{
		switch (TabName)
		{
		case "TraitsTab":
			CardsItemsTabBtn.Enable();
			ResistanceTabBtn.Enable();
			TraitsTabBtn.Disable();
			TraitsTab.SetActive(value: true);
			CardsandItemsTab.SetActive(value: false);
			ResistancesTab.SetActive(value: false);
			SingularityCardsTabText.SetActive(value: false);
			TraitsTabUpdate(charPopupMini.subClassData);
			break;
		case "CardsandItemsTab":
			TraitsTabBtn.Enable();
			ResistanceTabBtn.Enable();
			CardsItemsTabBtn.Disable();
			if (!GameManager.Instance.IsSingularity())
			{
				CardsandItemsTab.SetActive(value: true);
			}
			else
			{
				SingularityCardsTabText.SetActive(value: true);
			}
			TraitsTab.SetActive(value: false);
			ResistancesTab.SetActive(value: false);
			CardsandItemsTabUpdate(charPopupMini.subClassData);
			break;
		case "ResistancesTab":
			TraitsTabBtn.Enable();
			CardsItemsTabBtn.Enable();
			ResistanceTabBtn.Disable();
			ResistancesTab.SetActive(value: true);
			TraitsTab.SetActive(value: false);
			CardsandItemsTab.SetActive(value: false);
			SingularityCardsTabText.SetActive(value: false);
			break;
		}
	}

	public void TraitsTabUpdate(SubClassData subclass)
	{
		if (!(subclass == null))
		{
			int characterTier = PlayerManager.Instance.GetCharacterTier(subclass.Id, "trait");
			_Trait0RO = _Trait0.transform.parent.GetComponent<TraitRollOver>();
			_Trait1ARO = _Trait1A.transform.parent.GetComponent<TraitRollOver>();
			_Trait1BRO = _Trait1B.transform.parent.GetComponent<TraitRollOver>();
			_Trait2ARO = _Trait2A.transform.parent.GetComponent<TraitRollOver>();
			_Trait2BRO = _Trait2B.transform.parent.GetComponent<TraitRollOver>();
			_Trait3ARO = _Trait3A.transform.parent.GetComponent<TraitRollOver>();
			_Trait3BRO = _Trait3B.transform.parent.GetComponent<TraitRollOver>();
			_Trait4ARO = _Trait4A.transform.parent.GetComponent<TraitRollOver>();
			_Trait4BRO = _Trait4B.transform.parent.GetComponent<TraitRollOver>();
			_Trait0RO.SetTrait(subclass.Trait0.Id, characterTier);
			_Trait1ARO.SetTrait(subclass.Trait1A.Id, characterTier);
			_Trait1BRO.SetTrait(subclass.Trait1B.Id, characterTier);
			_Trait2ARO.SetTrait(subclass.Trait2A.Id, characterTier);
			_Trait2BRO.SetTrait(subclass.Trait2B.Id, characterTier);
			_Trait3ARO.SetTrait(subclass.Trait3A.Id, characterTier);
			_Trait3BRO.SetTrait(subclass.Trait3B.Id, characterTier);
			_Trait4ARO.SetTrait(subclass.Trait4A.Id, characterTier);
			_Trait4BRO.SetTrait(subclass.Trait4B.Id, characterTier);
		}
	}

	public void CardsandItemsTabUpdate(SubClassData subclass)
	{
		int characterTier = PlayerManager.Instance.GetCharacterTier(subclass.Id, "card");
		int characterTier2 = PlayerManager.Instance.GetCharacterTier(subclass.Id, "item");
		cardsNumT[0] = _CardN1.transform.parent;
		cardsNumText[0] = _CardN1;
		cardsNumT[1] = _CardN2.transform.parent;
		cardsNumText[1] = _CardN2;
		cardsNumT[2] = _CardN3.transform.parent;
		cardsNumText[2] = _CardN3;
		cardsNumT[3] = _CardN4.transform.parent;
		cardsNumText[3] = _CardN4;
		cardsNumT[4] = _CardN5.transform.parent;
		cardsNumText[4] = _CardN5;
		cardsNumT[5] = _CardN6.transform.parent;
		cardsNumText[5] = _CardN6;
		cardsNumT[6] = _CardN7.transform.parent;
		cardsNumText[6] = _CardN7;
		for (int i = 0; i < 7; i++)
		{
			if (i >= subclass.Cards.Length || subclass.Cards[i] == null)
			{
				continue;
			}
			string id = subclass.Cards[i].Card.Id;
			if (subclass.Cards[i].Card.Starter)
			{
				switch (characterTier)
				{
				case 1:
					id = Globals.Instance.GetCardData(id, instantiate: false).UpgradesTo1.ToLower();
					break;
				case 2:
					id = Globals.Instance.GetCardData(id, instantiate: false).UpgradesTo2.ToLower();
					break;
				}
			}
			cardsCI[i].SetCard(id, deckScale: false);
			cardsCI[i].cardoutsidecombat = true;
			cardsCI[i].cardoutsideselection = true;
			cardsCI[i].ChangeLayer();
			cardsNumText[i].text = subclass.Cards[i].UnitsInDeck.ToString();
			cardsNumT[i].gameObject.SetActive(value: true);
		}
		string id2 = subclass.Item.Id;
		switch (characterTier2)
		{
		case 1:
			id2 = Globals.Instance.GetCardData(id2, instantiate: false).UpgradesTo1;
			break;
		case 2:
			id2 = Globals.Instance.GetCardData(id2, instantiate: false).UpgradesTo2;
			break;
		}
		cardsCI[7].SetCard(id2);
		cardsCI[7].cardoutsidecombat = true;
		cardsCI[7].cardoutsideselection = true;
		cardsCI[7].ChangeLayer();
		cardsCI[7].transform.localScale = new Vector3(0.45f, 0.45f, 1f);
	}

	public void SetSkinFromNetPlayer(string _nick, string _subclass, string _skinId)
	{
	}

	public void SetRandomHero(int _boxId)
	{
		int num = 0;
		foreach (KeyValuePair<string, HeroSelection> item in heroSelectionDictionary)
		{
			if (item.Value.RandomAvailable())
			{
				num++;
			}
		}
		int num2 = 0;
		for (int i = 0; i < 4; i++)
		{
			if (boxHero[boxGO[i]] != null && boxHero[boxGO[i]].GetSubclassName() != "")
			{
				num2++;
			}
		}
		if (num <= 4 && num2 == 4)
		{
			return;
		}
		int count = heroSelectionDictionary.Count;
		HeroSelection heroSelection = null;
		while (true)
		{
			int num3 = UnityEngine.Random.Range(0, count);
			int num4 = 0;
			foreach (KeyValuePair<string, HeroSelection> item2 in heroSelectionDictionary)
			{
				if (num4 == num3)
				{
					heroSelection = item2.Value;
					break;
				}
				num4++;
			}
			if (!(heroSelection != null))
			{
				break;
			}
			if (!heroSelection.RandomAvailable())
			{
				continue;
			}
			bool flag = true;
			for (int j = 0; j < boxGO.Length; j++)
			{
				if (boxHero[boxGO[j]] != null && boxHero[boxGO[j]].GetSubclassName().ToLower() == heroSelection.Id)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				heroSelection.PickHero(_comingFromRandom: true);
				heroSelection.PickStop(_boxId);
				heroSelection.HeroPicked = true;
				heroSelection.spriteSR.enabled = true;
				break;
			}
		}
	}

	public void LevelWithSupplies(string _scdId)
	{
		PlayerManager.Instance.SpendSupply(1);
		IncreaseHeroProgressSupplies(_scdId);
		UpdateSubclassRank(_scdId);
		charPopup.DoRank();
	}

	private void UpdateSubclassRank(string _scdId)
	{
		SubClassData subClassData = Globals.Instance.GetSubClassData(_scdId);
		foreach (KeyValuePair<string, HeroSelection> item in heroSelectionDictionary)
		{
			if (item.Key == subClassData.Id)
			{
				if (!GameManager.Instance.IsMultiplayer())
				{
					item.Value.SetRank();
				}
				else
				{
					item.Value.SetRank();
				}
			}
		}
	}

	public void ControllerMoveBlock(bool _isRight)
	{
	}

	public void ResetController()
	{
		Debug.Log("ResetController");
		controllerCurrentBlock = -1;
		controllerCurrentOption = -1;
		HideCharacterArrowController();
	}

	public void BackToControllerCharacterSelection()
	{
		Debug.Log("BackToControllerCharacterSelection");
		controllerCurrentBlock = 0;
		HideCharacterArrowController();
		ControllerMovement();
	}

	public void MoveAbsoluteToCharactersAfterClick()
	{
		Debug.Log("MoveAbsoluteToCharactersAfterClick");
		controllerHorizontalIndex = 0;
		warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(boxSelection[0].transform.position);
		Mouse.current.WarpCursorPosition(warpPosition);
	}

	private void HideCharacterArrowController()
	{
		for (int i = 0; i < 4; i++)
		{
			boxSelection[i].ShowHideArrowController(_state: false);
		}
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false, int absolutePosition = -1)
	{
		_controllerList.Clear();
		if (!dragging)
		{
			for (int i = 0; i < menuController.Count; i++)
			{
				_controllerList.Add(menuController[i]);
			}
			if (Functions.TransformIsVisible(botonFollow.transform))
			{
				_controllerList.Add(botonFollow.transform);
			}
			if (Functions.TransformIsVisible(beginAdventureButton))
			{
				_controllerList.Add(beginAdventureButton.transform);
			}
			if (Functions.TransformIsVisible(readyButton))
			{
				_controllerList.Add(readyButton.transform);
			}
			if (Functions.TransformIsVisible(madnessButton))
			{
				_controllerList.Add(madnessButton);
			}
			if (Functions.TransformIsVisible(sandboxButton))
			{
				_controllerList.Add(sandboxButton);
			}
			if (Functions.TransformIsVisible(gameSeed))
			{
				_controllerList.Add(gameSeed);
			}
			if (Functions.TransformIsVisible(weeklyModifiersButton))
			{
				_controllerList.Add(weeklyModifiersButton);
			}
		}
		for (int j = 0; j < 4; j++)
		{
			if (!Functions.TransformIsVisible(boxSelection[j].transform))
			{
				continue;
			}
			_controllerList.Add(boxSelection[j].transform);
			if (dragging)
			{
				continue;
			}
			if (Functions.TransformIsVisible(boxSelection[j].dice))
			{
				_controllerList.Add(boxSelection[j].dice);
			}
			for (int k = 0; k < 4; k++)
			{
				if (Functions.TransformIsVisible(boxSelection[j].boxPlayer[k].transform))
				{
					_controllerList.Add(boxSelection[j].boxPlayer[k].transform);
				}
			}
		}
		controllerHorizontalIndex = Functions.GetListClosestIndexToMousePosition(_controllerList);
		controllerHorizontalIndex = Functions.GetClosestIndexBasedOnDirection(_controllerList, controllerHorizontalIndex, goingUp, goingRight, goingDown, goingLeft);
		if (_controllerList[controllerHorizontalIndex] != null)
		{
			warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerList[controllerHorizontalIndex].position);
			Mouse.current.WarpCursorPosition(warpPosition);
		}
	}

	public void ControllerMovementOLD(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false, int absolutePosition = -1)
	{
		if (absolutePosition != -1)
		{
			controllerCurrentOption = absolutePosition;
		}
		if (controllerCurrentBlock == 0)
		{
			bool flag = false;
			Transform transform = null;
			List<Transform> list = menuController;
			while (!flag)
			{
				if (list != null)
				{
					if (goingLeft)
					{
						controllerCurrentOption--;
					}
					else if (goingUp)
					{
						controllerCurrentOption -= 4;
					}
					else if (goingDown)
					{
						controllerCurrentOption += 4;
					}
					else
					{
						controllerCurrentOption++;
						if (controllerCurrentOption % 4 == 0 || controllerCurrentOption == list.Count - 1)
						{
							controllerCurrentBlock = 1;
							controllerCurrentOption = -1;
							ControllerMovement(goingUp: false, goingRight: true);
							break;
						}
					}
					if (controllerCurrentOption < 0)
					{
						controllerCurrentOption = list.Count - 1;
					}
					if (controllerCurrentOption > list.Count - 1)
					{
						controllerCurrentOption = 0;
					}
					transform = list[controllerCurrentOption];
					if (transform != null && Functions.TransformIsVisible(transform) && !(transform.parent == boxCharacters))
					{
						warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(transform.position);
						Mouse.current.WarpCursorPosition(warpPosition);
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
			}
		}
		else if (controllerCurrentBlock == 1)
		{
			bool flag2 = false;
			int num = 0;
			if (absolutePosition != -1)
			{
				absolutePosition--;
				goingLeft = true;
			}
			while (!flag2)
			{
				if (goingRight)
				{
					controllerCurrentOption--;
					if (controllerCurrentOption < 0)
					{
						controllerCurrentOption = 3;
					}
				}
				else if (goingLeft)
				{
					controllerCurrentOption++;
					if (controllerCurrentOption > 3)
					{
						controllerCurrentOption = 0;
					}
				}
				else
				{
					if (!goingUp)
					{
						controllerCurrentBlock = 4;
						ControllerMovement(goingUp: true);
						return;
					}
					if (boxSelection[controllerCurrentOption].dice.gameObject.activeSelf)
					{
						warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(boxSelection[controllerCurrentOption].dice.position);
						Mouse.current.WarpCursorPosition(warpPosition);
						return;
					}
				}
				flag2 = (Functions.TransformIsVisible(boxSelection[controllerCurrentOption].transform) ? true : false);
				num++;
				if (num > 10)
				{
					break;
				}
			}
			HideCharacterArrowController();
			boxSelection[controllerCurrentOption].ShowHideArrowController(_state: true);
			warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(boxSelection[controllerCurrentOption].transform.position);
			Mouse.current.WarpCursorPosition(warpPosition);
		}
		else if (controllerCurrentBlock == 2)
		{
			if (goingUp)
			{
				controllerCurrentBlock = 1;
				controllerCurrentOption = -1;
				ControllerMovement(goingUp: false, goingRight: true);
			}
			else if (goingDown)
			{
				controllerCurrentBlock = 4;
				ControllerMovement(goingUp: false, goingRight: true);
			}
			else
			{
				warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(beginAdventureButton.position);
				Mouse.current.WarpCursorPosition(warpPosition);
			}
		}
		else if (controllerCurrentBlock == 3)
		{
			if (goingUp)
			{
				controllerCurrentBlock = 1;
				controllerCurrentOption = -1;
				ControllerMovement(goingUp: false, goingRight: true);
			}
			else if (goingDown)
			{
				controllerCurrentBlock = 4;
				ControllerMovement(goingUp: false, goingRight: true);
			}
			else
			{
				warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(botonFollow.transform.parent.position);
				Mouse.current.WarpCursorPosition(warpPosition);
			}
		}
		else if (controllerCurrentBlock == 4)
		{
			if (goingUp)
			{
				if (Functions.TransformIsVisible(botonFollow.transform))
				{
					controllerCurrentBlock = 3;
					ControllerMovement(goingUp: false, goingRight: true);
				}
				else
				{
					controllerCurrentBlock = 2;
					ControllerMovement(goingUp: false, goingRight: true);
				}
			}
			else if (goingDown)
			{
				controllerCurrentBlock = 5;
				ControllerMovement(goingUp: false, goingRight: true);
			}
			else
			{
				warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(madnessButton.position);
				Mouse.current.WarpCursorPosition(warpPosition);
			}
		}
		else if (controllerCurrentBlock == 5)
		{
			warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(sandboxButton.position);
			Mouse.current.WarpCursorPosition(warpPosition);
		}
		else if (controllerCurrentBlock == 6)
		{
			if (gameSeed.gameObject.activeSelf)
			{
				warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(gameSeed.position);
				Mouse.current.WarpCursorPosition(warpPosition);
			}
			else
			{
				ControllerMovement(goingUp, goingRight, goingDown, goingLeft, absolutePosition);
			}
		}
	}
}
