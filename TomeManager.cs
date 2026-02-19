using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Steamworks;
using Steamworks.Data;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TomeManager : MonoBehaviour
{
	public GameObject content;

	public GameObject unlockedPiece;

	public Transform exitButton;

	public Transform cardsMask;

	public Transform mainSection;

	public Transform mainSectionButtonT;

	private BotonGeneric mainSectionButton;

	public Transform cardsSection;

	public Transform cardsSectionButtonT;

	private BotonGeneric cardsSectionButton;

	public Transform itemsSection;

	public Transform itemsSectionButtonT;

	private BotonGeneric itemsSectionButton;

	public Transform challengeSection;

	public Transform challengeSectionButtonT;

	private BotonGeneric challengeSectionButton;

	public Transform monsterSection;

	public Transform monsterSectionButtonT;

	private BotonGeneric monsterSectionButton;

	public Transform scoreboardSection;

	public Transform scoreboardSectionButtonT;

	private BotonGeneric scoreboardSectionButton;

	public Transform glossarySection;

	public Transform glossarySectionButtonT;

	private BotonGeneric glossarySectionButton;

	public Transform glossaryIndex;

	public Transform glossaryPageIndex;

	public Transform glossaryPageIndex2;

	public Transform glossaryPageLeft;

	public Transform glossaryPageRight;

	public GameObject glossaryIndexItem;

	private int glossaryTermSelected = -1;

	private SortedDictionary<string, string> glossaryTerms;

	private Dictionary<string, string> glossaryTermsTitle;

	public TMP_Text[] glossaryTexts;

	private Coroutine glossaryCo;

	private int glossaryItemsPerSection = 10;

	public Transform tomeSideButtons;

	public Transform scores;

	public Transform runsSection;

	public Transform runsSectionButtonT;

	private BotonGeneric runsSectionButton;

	public Transform page;

	private Animator pageAnim;

	public TomeButton[] TomeButtons;

	public TomeButton monsterButton;

	public Transform cardContainer;

	public GameObject TomeCard;

	public GameObject TomeNumber;

	public Transform Paginator;

	public GameObject MoreNumbersIcon;

	public TMP_Text[] statisticsTexts;

	public UnlockedBar[] unlockedBars;

	private TomeCard[] tomeCards;

	private Transform[] tomeTs;

	private GameObject[] cardGO;

	private GameObject[] cardGOBlue;

	private GameObject[] cardGOGold;

	private GameObject[] cardGORare;

	private TomeNumber[] tomeNumbers = new TomeNumber[30];

	private int pageAct;

	private int pageMax = 1;

	private int pageOld;

	private int numCards = 18;

	private int maxTomeNumbers = 30;

	private int activeTomeCards = -100;

	private List<string> cardList = new List<string>();

	private Coroutine SetTomeCardsCo;

	private Coroutine TomeCardsMasterCo;

	public Transform searchBox;

	public TMP_InputField searchInput;

	public TMP_Text searchInputPlaceholder;

	public TextMeshProUGUI searchInputPlaceholderGUI;

	public TMP_Text searchInputText;

	public Transform canvasSearchCloseT;

	private string searchTerm = "";

	private Coroutine searchCo;

	public Score[] scoresName;

	private Coroutine SetScoresCo;

	private Coroutine ShowCoroutine;

	private int numScoreRows = 20;

	private int scoreboardType;

	private int currentScoreboard;

	private int week;

	public bool playerOnScoreboard;

	public TMP_Text scoreTitle;

	public TMP_Text scoreTitleSub;

	public TMP_Text scoreStatus;

	public Transform[] scoreButtons;

	public BotonGeneric buttonPrevWeekly;

	public BotonGeneric buttonNextWeekly;

	public List<PlayerRun> playerRunsList;

	public GameObject run;

	public Transform runsContainer;

	public Transform runDetails;

	private int activeRun = -1;

	public Transform runPathMask;

	private int numRunsPerPage = 8;

	public int controllerHorizontalIndex = -1;

	private Vector2 warpPosition = Vector2.zero;

	private List<Transform> _controllerList = new List<Transform>();

	public static TomeManager Instance { get; private set; }

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
		pageAnim = page.GetComponent<Animator>();
		mainSectionButton = mainSectionButtonT.GetComponent<BotonGeneric>();
		cardsSectionButton = cardsSectionButtonT.GetComponent<BotonGeneric>();
		itemsSectionButton = itemsSectionButtonT.GetComponent<BotonGeneric>();
		challengeSectionButton = challengeSectionButtonT.GetComponent<BotonGeneric>();
		monsterSectionButton = monsterSectionButtonT.GetComponent<BotonGeneric>();
		scoreboardSectionButton = scoreboardSectionButtonT.GetComponent<BotonGeneric>();
		glossarySectionButton = glossarySectionButtonT.GetComponent<BotonGeneric>();
		runsSectionButton = runsSectionButtonT.GetComponent<BotonGeneric>();
		cardGO = new GameObject[18];
		cardGOBlue = new GameObject[18];
		cardGOGold = new GameObject[18];
		cardGORare = new GameObject[18];
	}

	public void Resize()
	{
	}

	public void InitTome()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<voffset=-4><size=+10><sprite name=cards></size></voffset>");
		stringBuilder.Append(Texts.Instance.GetText("searchCards"));
		searchInputPlaceholder.text = stringBuilder.ToString();
		CreateNumbers();
		CreateTomeCards();
	}

	public void ShowTome(bool _status)
	{
		if (!_status && SceneStatic.GetSceneName() == "TomeOfKnowledge")
		{
			content.gameObject.SetActive(value: false);
			SceneStatic.LoadByName("MainMenu");
			return;
		}
		content.gameObject.SetActive(_status);
		if (_status)
		{
			for (int i = 0; i < unlockedBars.Length; i++)
			{
				unlockedBars[i].InitBar();
			}
			playerRunsList = new List<PlayerRun>();
			for (int num = PlayerManager.Instance.PlayerRuns.Count - 1; num >= 0; num--)
			{
				PlayerRun playerRun = JsonUtility.FromJson<PlayerRun>(PlayerManager.Instance.PlayerRuns[num]);
				if (playerRun.Version != null && playerRun.Version != "" && playerRun.FinalScore > 0 && Functions.GameVersionToNumber(playerRun.Version) >= Functions.GameVersionToNumber("0.7.0"))
				{
					playerRunsList.Add(playerRun);
				}
			}
			DoTomeMain();
			SelectTomeCards(0);
		}
		else
		{
			GameManager.Instance.CleanTempContainer();
			PopupManager.Instance.ClosePopup();
		}
	}

	public bool IsActive()
	{
		if (content == null || content.gameObject == null)
		{
			return false;
		}
		return content.gameObject.activeSelf;
	}

	public void ShowCardsMask(bool _status, bool _instant = false)
	{
	}

	private IEnumerator ShowCardsMaskCo(bool _status, bool _instant)
	{
		float num = 0.4f;
		float destineAlpha = 0f;
		if (_status)
		{
			destineAlpha = num;
		}
		SpriteRenderer SPR = cardsMask.GetComponent<SpriteRenderer>();
		if (_instant)
		{
			SPR.color = new UnityEngine.Color(1f, 1f, 1f, destineAlpha);
			yield break;
		}
		if (!_status)
		{
			yield return new WaitForSeconds(0.1f);
		}
		while (SPR.color.a != destineAlpha)
		{
			if (_status)
			{
				SPR.color = new UnityEngine.Color(SPR.color.r, SPR.color.g, SPR.color.b, SPR.color.a + 0.05f);
				if (SPR.color.a > destineAlpha)
				{
					break;
				}
			}
			else
			{
				SPR.color = new UnityEngine.Color(SPR.color.r, SPR.color.g, SPR.color.b, SPR.color.a - 0.05f);
				if (SPR.color.a < destineAlpha)
				{
					break;
				}
			}
			yield return new WaitForSeconds(0.01f);
		}
		SPR.color = new UnityEngine.Color(SPR.color.r, SPR.color.g, SPR.color.b, destineAlpha);
	}

	public void DoMouseScroll(Vector2 vectorScroll)
	{
		if (!IsActive() || CardScreenManager.Instance.IsActive() || (!cardsSection.gameObject.activeSelf && !itemsSection.gameObject.activeSelf && !glossarySection.gameObject.activeSelf && !scoreboardSection.gameObject.activeSelf && !runsSection.gameObject.activeSelf))
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
			DoNextPage();
		}
	}

	public void DoTomeMain()
	{
		ActivateSection("main");
		statisticsTexts[0].text = PlayerManager.Instance.BestScore.ToString();
		statisticsTexts[1].text = playerRunsList.Count.ToString();
		if (PlayerManager.Instance.TreasuresClaimed != null)
		{
			statisticsTexts[2].text = PlayerManager.Instance.TreasuresClaimed.Count.ToString();
		}
		else
		{
			statisticsTexts[2].text = "0";
		}
		statisticsTexts[3].text = PlayerManager.Instance.MonstersKilled.ToString();
		statisticsTexts[4].text = PlayerManager.Instance.BossesKilled.ToString();
		statisticsTexts[5].text = PlayerManager.Instance.ExpGained.ToString();
		statisticsTexts[6].text = PlayerManager.Instance.CardsCrafted.ToString();
		statisticsTexts[7].text = PlayerManager.Instance.CardsUpgraded.ToString();
		statisticsTexts[8].text = PlayerManager.Instance.GoldGained.ToString();
		statisticsTexts[9].text = PlayerManager.Instance.DustGained.ToString();
		statisticsTexts[10].text = PlayerManager.Instance.PurchasedItems.ToString();
		statisticsTexts[11].text = PlayerManager.Instance.CorruptionsCompleted.ToString();
	}

	private void ActivateSection(string theSection)
	{
		activeRun = -1;
		runDetails.gameObject.SetActive(value: false);
		runPathMask.gameObject.SetActive(value: false);
		ShowCardsMask(_status: false, _instant: true);
		if (SetTomeCardsCo != null)
		{
			StopCoroutine(SetTomeCardsCo);
		}
		if (ShowCoroutine != null)
		{
			StopCoroutine(ShowCoroutine);
		}
		Paginator.gameObject.SetActive(value: false);
		if (theSection == "main")
		{
			mainSection.gameObject.SetActive(value: true);
			mainSectionButton.Disable();
			mainSectionButton.ShowDisableMask(state: false);
			mainSectionButton.ShowBorder(state: true);
		}
		else
		{
			mainSection.gameObject.SetActive(value: false);
			mainSectionButton.Enable();
			mainSectionButton.ShowDisableMask(state: true);
			mainSectionButton.ShowBorder(state: false);
		}
		if (theSection == "cards")
		{
			cardsSection.gameObject.SetActive(value: true);
			cardsSectionButton.Disable();
			cardsSectionButton.ShowDisableMask(state: false);
			cardsSectionButton.ShowBorder(state: true);
			cardContainer.gameObject.SetActive(value: true);
			Paginator.gameObject.SetActive(value: true);
		}
		else
		{
			if (cardContainer.gameObject.activeSelf)
			{
				for (int num = cardContainer.childCount - 1; num >= 0; num--)
				{
					cardContainer.GetChild(num).gameObject.SetActive(value: false);
				}
			}
			cardsSection.gameObject.SetActive(value: false);
			cardsSectionButton.Enable();
			cardsSectionButton.ShowDisableMask(state: true);
			cardsSectionButton.ShowBorder(state: false);
			cardContainer.gameObject.SetActive(value: false);
		}
		if (theSection == "items")
		{
			itemsSection.gameObject.SetActive(value: true);
			itemsSectionButton.Disable();
			itemsSectionButton.ShowDisableMask(state: false);
			itemsSectionButton.ShowBorder(state: true);
			cardContainer.gameObject.SetActive(value: true);
			Paginator.gameObject.SetActive(value: true);
		}
		else
		{
			if (cardContainer.gameObject.activeSelf)
			{
				for (int num2 = cardContainer.childCount - 1; num2 >= 0; num2--)
				{
					cardContainer.GetChild(num2).gameObject.SetActive(value: false);
				}
			}
			itemsSection.gameObject.SetActive(value: false);
			itemsSectionButton.Enable();
			itemsSectionButton.ShowDisableMask(state: true);
			itemsSectionButton.ShowBorder(state: false);
			if (theSection != "cards")
			{
				cardContainer.gameObject.SetActive(value: false);
			}
		}
		if (theSection == "glossary")
		{
			glossarySection.gameObject.SetActive(value: true);
			glossarySectionButton.Disable();
			glossarySectionButton.ShowDisableMask(state: false);
			glossarySectionButton.ShowBorder(state: true);
			Paginator.gameObject.SetActive(value: true);
		}
		else
		{
			glossarySection.gameObject.SetActive(value: false);
			glossarySectionButton.Enable();
			glossarySectionButton.ShowDisableMask(state: true);
			glossarySectionButton.ShowBorder(state: false);
		}
		if (theSection == "scoreboard")
		{
			scoreboardSection.gameObject.SetActive(value: true);
			scoreboardSectionButton.Disable();
			scoreboardSectionButton.ShowDisableMask(state: false);
			scoreboardSectionButton.ShowBorder(state: true);
			Paginator.gameObject.SetActive(value: true);
		}
		else
		{
			scoreboardSection.gameObject.SetActive(value: false);
			scoreboardSectionButton.Enable();
			scoreboardSectionButton.ShowDisableMask(state: true);
			scoreboardSectionButton.ShowBorder(state: false);
		}
		if (theSection == "runs")
		{
			runsSection.gameObject.SetActive(value: true);
			runsSectionButton.Disable();
			runsSectionButton.ShowDisableMask(state: false);
			runsSectionButton.ShowBorder(state: true);
			Paginator.gameObject.SetActive(value: true);
		}
		else
		{
			runsSection.gameObject.SetActive(value: false);
			runsSectionButton.Enable();
			runsSectionButton.ShowDisableMask(state: true);
			runsSectionButton.ShowBorder(state: false);
		}
		if (theSection == "cards" || theSection == "items")
		{
			ShowSearch(state: true);
		}
		else
		{
			ShowSearch(state: false);
		}
	}

	public void DoTomeCards()
	{
		ActivateSection("cards");
		ShowTomeCardsButtons();
	}

	public void DoTomeItems()
	{
		ActivateSection("items");
		ShowTomeItemsButtons();
	}

	public void DoTomeScoreboard()
	{
		ActivateSection("scoreboard");
		ShowScoreboard(0);
	}

	public void DoTomeRuns()
	{
		ActivateSection("runs");
		ShowRuns(0);
	}

	public void DoTomeGlossary()
	{
		if (glossaryTerms == null)
		{
			CreateGlossary();
		}
		ActivateSection("glossary");
		SetPage(0, absolute: true);
	}

	public void DoTomeChallenge()
	{
		ActivateSection("challenge");
	}

	public void DoTomeMonsters()
	{
		ActivateSection("monsters");
	}

	private void NextPage()
	{
		pageAct++;
	}

	private void PrevPage()
	{
		pageAct--;
	}

	public void CreateGlossary()
	{
		_ = new KeyNotesData[Globals.Instance.KeyNotes.Count];
		glossaryTerms = new SortedDictionary<string, string>();
		glossaryTermsTitle = new Dictionary<string, string>();
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		int num = 0;
		int num2 = 0;
		string text = "";
		Transform parent = glossaryPageIndex;
		HashSet<string> hashSet = new HashSet<string> { "daze", "haste", "frail", "reloading", "nightmareecho", "nightmareechoa", "nightmareechob", "transferpain", "corruptedecho", "invulnerableunremovable" };
		foreach (KeyValuePair<string, KeyNotesData> keyNote in Globals.Instance.KeyNotes)
		{
			if (hashSet.Contains(keyNote.Value.Id))
			{
				continue;
			}
			stringBuilder.Clear();
			stringBuilder2.Clear();
			stringBuilder.Append("<voffset=.1><sprite name=");
			if (keyNote.Value.Id == "chain" || keyNote.Value.Id == "jump" || keyNote.Value.Id == "jump(bonus%)" || keyNote.Value.Id == "overcharge" || keyNote.Value.Id == "repeat" || keyNote.Value.Id == "repeatupto" || keyNote.Value.Id == "dispel" || keyNote.Value.Id == "purge" || keyNote.Value.Id == "discover" || keyNote.Value.Id == "reveal" || keyNote.Value.Id == "transfer" || keyNote.Value.Id == "steal" || keyNote.Value.Id == "aura" || keyNote.Value.Id == "curse" || keyNote.Value.Id == "escapes" || keyNote.Value.Id == "metamorph" || keyNote.Value.Id == "nightmareecho" || keyNote.Value.Id == "nightmareimage" || keyNote.Value.Id == "transferpain" || keyNote.Value.Id == "corruptedecho")
			{
				stringBuilder.Append("cards");
			}
			else if (keyNote.Value.Id == "resistance")
			{
				stringBuilder.Append("ui_resistance");
			}
			else if (keyNote.Value.Id == "speed")
			{
				stringBuilder.Append("speedMini");
			}
			else
			{
				stringBuilder.Append(keyNote.Value.Id);
			}
			stringBuilder.Append("></voffset>");
			stringBuilder.Append(keyNote.Value.KeynoteName);
			if (keyNote.Value.Id == "overcharge")
			{
				stringBuilder.Append(" [");
				stringBuilder.Append(Texts.Instance.GetText("overchargeAcronym"));
				stringBuilder.Append("]");
			}
			stringBuilder2.Append(stringBuilder.ToString());
			if (glossaryTermsTitle.ContainsKey(keyNote.Value.KeynoteName))
			{
				continue;
			}
			glossaryTermsTitle.Add(keyNote.Value.KeynoteName, stringBuilder2.ToString());
			stringBuilder.Replace("<voffset=.1>", "<size=+.3><voffset=-.1>");
			stringBuilder.Replace("</voffset>", "</voffset></size>");
			string id = keyNote.Value.Id;
			AuraCurseData auraCurseData = Globals.Instance.GetAuraCurseData(id);
			if (auraCurseData != null)
			{
				if (auraCurseData.IsAura)
				{
					stringBuilder.Append("  <size=-.4><color=#3765A9>(");
					stringBuilder.Append(Texts.Instance.GetText("aura"));
					stringBuilder.Append(")</color></size>");
				}
				else
				{
					stringBuilder.Append("  <size=-.4><color=#B0363E>(");
					stringBuilder.Append(Texts.Instance.GetText("curse"));
					stringBuilder.Append(")</color></size>");
				}
			}
			stringBuilder.Append("<line-height=30%><br></line-height><br><size=-.3><color=#794D31>");
			stringBuilder.Append(Functions.StripTags(keyNote.Value.DescriptionExtended.Replace("<br3>", " ").Trim()));
			if (auraCurseData != null && !auraCurseData.GainCharges)
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(Texts.Instance.GetText("notStackPlain"));
				stringBuilder.Append(".");
			}
			stringBuilder.Append("</color></size>");
			glossaryTerms.Add(keyNote.Value.KeynoteName, stringBuilder.ToString());
		}
		glossaryTermsTitle.Add("TerrainCondition", "<sprite name=node>" + Texts.Instance.GetText("terrainCondition"));
		stringBuilder.Clear();
		stringBuilder.Append("<size=+.3><voffset=-.1><sprite name=node></voffset></size>");
		stringBuilder.Append(Texts.Instance.GetText("terrainCondition"));
		stringBuilder.Append("<line-height=30%><br></line-height><br><size=-.5><color=#794D31>");
		stringBuilder.Append(Texts.Instance.GetText("terrainConditionDesc"));
		stringBuilder.Append("  ");
		bool flag = true;
		foreach (Enums.NodeGround value in Enum.GetValues(typeof(Enums.NodeGround)))
		{
			if (value != Enums.NodeGround.None)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append(",  ");
				}
				stringBuilder.Append(Functions.GetNodeGroundSprite(value));
				stringBuilder.Append(Texts.Instance.GetText(Enum.GetName(typeof(Enums.NodeGround), value)));
			}
		}
		glossaryTerms.Add("TerrainCondition", stringBuilder.ToString());
		stringBuilder = null;
		int num3 = 48;
		foreach (KeyValuePair<string, string> glossaryTerm in glossaryTerms)
		{
			if (Globals.Instance.CurrentLang == "en" || Globals.Instance.CurrentLang == "es")
			{
				if (num2 >= num3)
				{
					parent = glossaryPageIndex2;
				}
				if (glossaryTerm.Key.ToString() != "" && glossaryTerm.Key[0].ToString() != text)
				{
					text = glossaryTerm.Key[0].ToString();
					GameObject gameObject = UnityEngine.Object.Instantiate(glossaryIndexItem, Vector3.zero, Quaternion.identity, parent);
					gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 0f);
					gameObject.transform.GetChild(0).GetComponent<BotonGeneric>().SetText("<voffset=-1.5><size=3.8><color=#8C5B34><b>" + text + "</b></color></size>");
					gameObject.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
					num2++;
				}
			}
			if (num2 >= num3)
			{
				parent = glossaryPageIndex2;
			}
			GameObject gameObject2 = UnityEngine.Object.Instantiate(glossaryIndexItem, Vector3.zero, Quaternion.identity, parent);
			gameObject2.transform.localPosition = new Vector3(gameObject2.transform.localPosition.x, gameObject2.transform.localPosition.y, 0f);
			gameObject2.transform.GetChild(0).GetComponent<BotonGeneric>().SetText(glossaryTermsTitle[glossaryTerm.Key]);
			gameObject2.transform.GetChild(0).GetComponent<BotonGeneric>().auxInt = num;
			gameObject2.transform.GetChild(0).GetComponent<BotonGeneric>().auxString = "GlossaryItemIndex";
			num++;
			num2++;
		}
	}

	public void SetGlossaryPageFromButton(int _buttonIndex)
	{
		glossaryTermSelected = _buttonIndex;
		SetPage(Mathf.CeilToInt((float)(_buttonIndex + 1) / (float)glossaryItemsPerSection), absolute: true);
		if (glossaryCo != null)
		{
			StopCoroutine(glossaryCo);
		}
		glossaryCo = StartCoroutine(ClearGlossaryTermCo());
	}

	private IEnumerator ClearGlossaryTermCo()
	{
		yield return new WaitForSeconds(1.5f);
		if (IsActive())
		{
			ClearGlossaryTerm();
		}
	}

	private void ClearGlossaryTerm()
	{
		glossaryTermSelected = -1;
		for (int i = 0; i < glossaryItemsPerSection; i++)
		{
			glossaryTexts[i].transform.parent.GetChild(1).gameObject.SetActive(value: false);
		}
		if (glossaryCo != null)
		{
			StopCoroutine(glossaryCo);
		}
	}

	public void ShowGlossary()
	{
		pageMax = Mathf.CeilToInt((float)glossaryTerms.Count / (float)glossaryItemsPerSection);
		RedoPageNumbers();
		if (pageAct == 0)
		{
			glossaryIndex.gameObject.SetActive(value: true);
			glossaryPageLeft.gameObject.SetActive(value: false);
			glossaryPageRight.gameObject.SetActive(value: false);
			TomeButtons[23].Activate();
			ClearGlossaryTerm();
			return;
		}
		glossaryIndex.gameObject.SetActive(value: false);
		glossaryPageLeft.gameObject.SetActive(value: true);
		glossaryPageRight.gameObject.SetActive(value: true);
		TomeButtons[23].Deactivate();
		int num = 0;
		int num2 = 0;
		int num3 = (pageAct - 1) * glossaryItemsPerSection;
		int num4 = pageAct * glossaryItemsPerSection;
		if (num3 < 0)
		{
			num3 = 0;
			num4 = glossaryItemsPerSection;
		}
		foreach (KeyValuePair<string, string> glossaryTerm in glossaryTerms)
		{
			if (glossaryTerms.Count > num && num >= num3 && num < num4)
			{
				glossaryTexts[num2].text = glossaryTerm.Value;
				if (Globals.Instance.CurrentLang != "en" && Globals.Instance.CurrentLang != "es")
				{
					glossaryTexts[num2].text = glossaryTexts[num2].text.Replace("<line-height=30%><br></line-height>", "");
				}
				glossaryTexts[num2].transform.parent.gameObject.SetActive(value: true);
				if (glossaryTermSelected > -1 && glossaryTermSelected == num)
				{
					glossaryTexts[num2].transform.parent.GetChild(1).gameObject.SetActive(value: true);
				}
				num2++;
			}
			num++;
		}
		for (int i = num2; i < glossaryItemsPerSection; i++)
		{
			glossaryTexts[i].transform.parent.gameObject.SetActive(value: false);
		}
	}

	public void ShowRuns(int _index, int _subindex = -1)
	{
		pageOld = (pageAct = 0);
		pageMax = Mathf.CeilToInt((float)playerRunsList.Count / (float)numRunsPerPage);
		RedoPageNumbers();
		if (pageMax > 0)
		{
			RedoPageNumbers();
			TomeButtons[14].gameObject.SetActive(value: true);
			TomeButtons[15].gameObject.SetActive(value: true);
			if (scoreboardType == 0)
			{
				TomeButtons[14].Activate();
				TomeButtons[15].Deactivate();
			}
			else
			{
				TomeButtons[15].Activate();
				TomeButtons[14].Deactivate();
			}
			SetPage(1, absolute: true);
		}
	}

	private void SetRuns()
	{
		GameManager.Instance.PlayLibraryAudio("ui_book_page", 0.2f);
		runsContainer.gameObject.SetActive(value: true);
		for (int num = runsContainer.childCount - 1; num >= 0; num--)
		{
			UnityEngine.Object.Destroy(runsContainer.GetChild(num).gameObject);
		}
		int num2 = (pageAct - 1) * numRunsPerPage;
		int num3 = num2 + numRunsPerPage;
		int num4 = numRunsPerPage / 2;
		for (int i = num2; i < num3; i++)
		{
			if (i < playerRunsList.Count)
			{
				GameObject obj = UnityEngine.Object.Instantiate(run, Vector3.zero, Quaternion.identity, runsContainer);
				obj.name = "run_" + i;
				Mathf.FloorToInt(i / 3);
				bool num5 = num3 - i > num4;
				float num6 = 0f;
				int num7 = 0;
				if (num5)
				{
					num6 = -5.48f;
					num7 = i - num2;
				}
				else
				{
					num6 = 0.7f;
					num7 = num4 - (num3 - i);
				}
				obj.transform.localPosition = new Vector3(num6, 2.65f - 1.9f * (float)num7, -1f);
				obj.GetComponent<TomeRun>().SetRun(i);
			}
		}
	}

	public void DoRun(int _index)
	{
		activeRun = _index;
		runPathMask.gameObject.SetActive(value: true);
		runDetails.gameObject.SetActive(value: true);
		Paginator.gameObject.SetActive(value: false);
		runsContainer.gameObject.SetActive(value: false);
		runDetails.GetComponent<TomeRunDetails>().SetRun(_index);
	}

	public void RunDetailOpenCharacter(int _index)
	{
		runDetails.GetComponent<TomeRunDetails>().DoCharacter(_index);
	}

	public void RunDetailClose()
	{
		activeRun = -1;
		runPathMask.gameObject.SetActive(value: false);
		runDetails.gameObject.SetActive(value: false);
		Paginator.gameObject.SetActive(value: true);
		runsContainer.gameObject.SetActive(value: true);
	}

	public void RunDetailButton(int _index)
	{
		runDetails.GetComponent<TomeRunDetails>().RunDetailButton(_index);
	}

	public void RunCombatStats()
	{
		DamageMeterManager.Instance.SaveATOStats();
		TomeRunDetails component = runDetails.GetComponent<TomeRunDetails>();
		AtOManager.Instance.combatStats = new int[4, component.playerRun.CombatStats0.GetLength(0)];
		for (int i = 0; i < component.playerRun.CombatStats0.GetLength(0); i++)
		{
			AtOManager.Instance.combatStats[0, i] = component.playerRun.CombatStats0[i];
			AtOManager.Instance.combatStats[1, i] = component.playerRun.CombatStats1[i];
			AtOManager.Instance.combatStats[2, i] = component.playerRun.CombatStats2[i];
			AtOManager.Instance.combatStats[3, i] = component.playerRun.CombatStats3[i];
		}
		DamageMeterManager.Instance.Show();
	}

	public void NextPath()
	{
		runDetails.GetComponent<TomeRunDetails>().NextPath();
	}

	public void PrevPath()
	{
		runDetails.GetComponent<TomeRunDetails>().PrevPath();
	}

	public async void ShowScoreboard(int _index, int _subindex = -1)
	{
		for (int i = 0; i < scoresName.Length; i++)
		{
			scoresName[i].gameObject.SetActive(value: false);
		}
		scoreStatus.text = "";
		pageMax = 0;
		RedoPageNumbers();
		TMP_Text tMP_Text = scoreTitle;
		string text = (scoreTitleSub.text = "");
		tMP_Text.text = text;
		buttonPrevWeekly.transform.gameObject.SetActive(value: false);
		buttonNextWeekly.transform.gameObject.SetActive(value: false);
		week = _subindex;
		currentScoreboard = _index;
		int currentWeek = Functions.GetCurrentWeeklyWeek();
		if (week == -1)
		{
			week = currentWeek;
		}
		else if (week == 0)
		{
			week = 1;
		}
		switch (_index)
		{
		case 0:
		{
			await SteamManager.Instance.GetLeaderboards(0);
			TMP_Text tMP_Text9 = scoreTitle;
			text = (scoreTitleSub.text = Texts.Instance.GetText("singleplayer"));
			tMP_Text9.text = text;
			break;
		}
		case 1:
		{
			await SteamManager.Instance.GetLeaderboards(1);
			TMP_Text tMP_Text8 = scoreTitle;
			text = (scoreTitleSub.text = Texts.Instance.GetText("multiplayer"));
			tMP_Text8.text = text;
			break;
		}
		case 2:
		{
			await SteamManager.Instance.GetLeaderboards(2);
			TMP_Text tMP_Text7 = scoreTitle;
			text = (scoreTitleSub.text = Texts.Instance.GetText("obeliskChallengeSingleplayer"));
			tMP_Text7.text = text;
			break;
		}
		case 3:
		{
			await SteamManager.Instance.GetLeaderboards(3);
			TMP_Text tMP_Text6 = scoreTitle;
			text = (scoreTitleSub.text = Texts.Instance.GetText("obeliskChallengeMultiplayer"));
			tMP_Text6.text = text;
			break;
		}
		case 4:
		{
			await SteamManager.Instance.GetLeaderboards(4, week);
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append(Texts.Instance.GetText("menuWeekly"));
			stringBuilder2.Append("<br><size=-.5><color=#FFAE88>");
			stringBuilder2.Append(AtOManager.Instance.GetWeeklyName(week));
			stringBuilder2.Append(" (");
			stringBuilder2.Append(Texts.Instance.GetText("singleplayer"));
			stringBuilder2.Append(")");
			TMP_Text tMP_Text5 = scoreTitle;
			text = (scoreTitleSub.text = stringBuilder2.ToString());
			tMP_Text5.text = text;
			break;
		}
		case 5:
		{
			await SteamManager.Instance.GetLeaderboards(5, week);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Texts.Instance.GetText("menuWeekly"));
			stringBuilder.Append("<br><size=-.5><color=#FFAE88>");
			stringBuilder.Append(AtOManager.Instance.GetWeeklyName(week));
			stringBuilder.Append(" (");
			stringBuilder.Append(Texts.Instance.GetText("multiplayer"));
			stringBuilder.Append(")");
			TMP_Text tMP_Text4 = scoreTitle;
			text = (scoreTitleSub.text = stringBuilder.ToString());
			tMP_Text4.text = text;
			break;
		}
		case 6:
		{
			await SteamManager.Instance.GetLeaderboards(6);
			TMP_Text tMP_Text3 = scoreTitle;
			text = (scoreTitleSub.text = Texts.Instance.GetText("singularitySingleplayer"));
			tMP_Text3.text = text;
			break;
		}
		case 7:
		{
			await SteamManager.Instance.GetLeaderboards(7);
			TMP_Text tMP_Text2 = scoreTitle;
			text = (scoreTitleSub.text = Texts.Instance.GetText("singularityMultiplayer"));
			tMP_Text2.text = text;
			break;
		}
		}
		buttonPrevWeekly.transform.gameObject.SetActive(value: false);
		buttonNextWeekly.transform.gameObject.SetActive(value: false);
		if (_index == 4 || _index == 5)
		{
			if (week > 1)
			{
				buttonPrevWeekly.transform.gameObject.SetActive(value: true);
			}
			if (week < currentWeek)
			{
				buttonNextWeekly.transform.gameObject.SetActive(value: true);
			}
		}
		scoreStatus.text = "[" + Texts.Instance.GetText("loading") + "]";
		DoScoreboard();
	}

	public void PrevWeekly()
	{
		week--;
		ShowScoreboard(currentScoreboard, week);
	}

	public void NextWeekly()
	{
		week++;
		ShowScoreboard(currentScoreboard, week);
	}

	private void ScoresNotFound()
	{
		scoreStatus.text = Texts.Instance.GetText("scoresNotFound");
	}

	private void DoScoreboard()
	{
		pageMax = (pageOld = (pageAct = 0));
		if (scoreboardType == 0)
		{
			if (SteamManager.Instance.scoreboardGlobal != null)
			{
				pageMax = Mathf.CeilToInt((float)SteamManager.Instance.scoreboardGlobal.Length / (float)numScoreRows);
			}
		}
		else if (SteamManager.Instance.scoreboardFriends != null)
		{
			pageMax = Mathf.CeilToInt((float)SteamManager.Instance.scoreboardFriends.Length / (float)numScoreRows);
		}
		RedoPageNumbers();
		if (scoreboardType == 0)
		{
			TomeButtons[14].Activate();
			TomeButtons[15].Deactivate();
		}
		else
		{
			TomeButtons[15].Activate();
			TomeButtons[14].Deactivate();
		}
		if (pageMax == 0)
		{
			ScoresNotFound();
			scores.gameObject.SetActive(value: false);
			return;
		}
		scores.gameObject.SetActive(value: true);
		scoreStatus.text = "";
		if (pageMax > 10)
		{
			pageMax = 10;
		}
		SetPage(1, absolute: true);
	}

	public void SetPage(int page, bool absolute = false)
	{
		if (absolute || pageAct != page)
		{
			pageOld = pageAct;
			pageAct = page;
			ChangePage();
		}
	}

	private void ChangePage()
	{
		if (runsSection.gameObject.activeSelf)
		{
			SetRuns();
		}
		else if (scoreboardSection.gameObject.activeSelf)
		{
			if (SetScoresCo != null)
			{
				StopCoroutine(SetScoresCo);
			}
			SetScoresCo = StartCoroutine(SetScores());
		}
		else if (glossarySection.gameObject.activeSelf)
		{
			ShowGlossary();
		}
		else
		{
			if (SetTomeCardsCo != null)
			{
				StopCoroutine(SetTomeCardsCo);
			}
			SetTomeCardsCo = StartCoroutine(SetTomeCards());
		}
		RedoPageNumbersPositions();
	}

	public bool IsThereNext()
	{
		if (pageAct < pageMax)
		{
			return true;
		}
		return false;
	}

	public bool IsLastPage()
	{
		if (pageAct + 1 == pageMax)
		{
			return true;
		}
		return false;
	}

	public void DoNextPage()
	{
		if (activeRun <= -1 && pageAct < pageMax)
		{
			pageOld = pageAct;
			_ = pageOld;
			_ = 0;
			NextPage();
			ChangePage();
		}
	}

	public bool IsTherePrev()
	{
		if (pageAct > 1)
		{
			return true;
		}
		return false;
	}

	public bool IsFirstPage()
	{
		if (pageAct == 2)
		{
			return true;
		}
		return false;
	}

	public void DoPrevPage()
	{
		if (activeRun <= -1 && pageAct > 1)
		{
			pageOld = pageAct;
			PrevPage();
			ChangePage();
		}
	}

	private void CreateTomeCards()
	{
		tomeTs = new Transform[18];
		tomeCards = new TomeCard[18];
		for (int i = 0; i < numCards; i++)
		{
			GameObject obj = UnityEngine.Object.Instantiate(TomeCard, Vector3.zero, Quaternion.identity, cardContainer);
			obj.name = "card_" + i;
			Transform transform = obj.transform;
			int num = Mathf.FloorToInt(i / 3);
			int num2 = i % 3;
			if (i < 9)
			{
				transform.localPosition = new Vector3(-5.1f + (float)num2 * 2.15f, 2.1f - (float)num * 3f, 0f);
			}
			else
			{
				num -= 3;
				transform.localPosition = new Vector3(2.2f + (float)num2 * 2.15f, 2.1f - (float)num * 3f, 0f);
			}
			tomeTs[i] = transform;
			tomeCards[i] = transform.GetComponent<TomeCard>();
		}
	}

	private void RedoPageNumbersPositions()
	{
		CreateCraftPages(pageAct, pageMax);
	}

	private void CreateCraftPages(int page, int total)
	{
		int num = 12;
		int num2 = 2;
		int num3 = 0;
		bool flag = false;
		if (total <= 1)
		{
			return;
		}
		for (int i = 0; i < total; i++)
		{
			flag = false;
			GameObject obj = tomeNumbers[num3].transform.gameObject;
			obj.gameObject.SetActive(value: true);
			obj.transform.localPosition = new Vector3(-6.75f, 8f - 0.55f * (float)num3, 0f);
			obj.name = "TomePage";
			TomeNumber component = obj.transform.GetComponent<TomeNumber>();
			component.Deactivate();
			if (total < num)
			{
				component.Init(i + 1);
				if (page - 1 == i)
				{
					tomeNumbers[i].Activate();
				}
				flag = true;
			}
			else if (i == num2 && i != page && (float)page > Mathf.Ceil((float)num * 0.5f))
			{
				component.SetText("...");
				flag = true;
			}
			else if (i == total - num2 - 1 && i != page && (float)page < (float)total - Mathf.Ceil((float)num * 0.5f) + (float)num2)
			{
				component.SetText("...");
				flag = true;
			}
			else if ((i <= num2 || i >= page - num2 - 1 || i >= total - Functions.FuncRoundToInt((float)num * 0.5f) - num2) && (i <= page + num2 - 1 || i >= total - num2 || i <= Functions.FuncRoundToInt((float)num * 0.5f) + 1))
			{
				component.Init(i + 1);
				if (page - 1 == i)
				{
					component.Activate();
				}
				flag = true;
			}
			if (flag)
			{
				num3++;
			}
		}
	}

	private void CreateNumbers()
	{
		for (int i = 0; i < maxTomeNumbers; i++)
		{
			GameObject obj = UnityEngine.Object.Instantiate(TomeNumber, Vector3.zero, Quaternion.identity, Paginator);
			obj.transform.localPosition = new Vector3(-6.75f, 8f - 0.55f * (float)i, 0f);
			TomeNumber component = obj.GetComponent<TomeNumber>();
			tomeNumbers[i] = component;
			component.Init(i + 1);
			obj.transform.gameObject.SetActive(value: false);
			component.Deactivate();
		}
	}

	public void ShowTomeCardsButtons()
	{
		if (GameManager.Instance.GetDeveloperMode() || GameManager.Instance.CheatMode || GameManager.Instance.EnableButtons)
		{
			TomeButtons[5].transform.gameObject.SetActive(value: true);
		}
		else
		{
			TomeButtons[5].transform.gameObject.SetActive(value: false);
		}
		activeTomeCards = -100;
		SelectTomeCards(0);
	}

	public void ShowTomeItemsButtons()
	{
		activeTomeCards = -100;
		SelectTomeCards(7);
	}

	public void SelectTomeCards(int index = -1, bool absolute = false)
	{
		if (TomeCardsMasterCo != null)
		{
			StopCoroutine(TomeCardsMasterCo);
		}
		TomeCardsMasterCo = StartCoroutine(SelectTomeCardsCo(index, absolute));
	}

	private IEnumerator SelectTomeCardsCo(int index, bool absolute)
	{
		yield return new WaitForSeconds(0f);
		if (index == activeTomeCards && !absolute)
		{
			yield break;
		}
		activeTomeCards = index;
		List<string> list = new List<string>();
		bool flag = GameManager.Instance.GetDeveloperMode() || GameManager.Instance.CheatMode || GameManager.Instance.EnableButtons;
		switch (index)
		{
		case -1:
			list = Globals.Instance.CardListNotUpgraded;
			break;
		case 0:
			list = Globals.Instance.CardListNotUpgradedByClass[Enums.CardClass.Warrior];
			break;
		case 1:
			list = Globals.Instance.CardListNotUpgradedByClass[Enums.CardClass.Mage];
			break;
		case 2:
			list = Globals.Instance.CardListNotUpgradedByClass[Enums.CardClass.Healer];
			break;
		case 3:
			list = Globals.Instance.CardListNotUpgradedByClass[Enums.CardClass.Scout];
			break;
		default:
			if (index == 4 && flag)
			{
				list = Globals.Instance.CardListNotUpgradedByClass[Enums.CardClass.Monster];
				break;
			}
			switch (index)
			{
			case 5:
				list = Globals.Instance.CardListNotUpgradedByClass[Enums.CardClass.Boon];
				break;
			case 6:
				list = Globals.Instance.CardListNotUpgradedByClass[Enums.CardClass.Injury];
				break;
			case 7:
				list = Globals.Instance.CardItemByType[Enums.CardType.Weapon];
				break;
			case 8:
				list = Globals.Instance.CardItemByType[Enums.CardType.Armor];
				break;
			case 9:
				list = Globals.Instance.CardItemByType[Enums.CardType.Jewelry];
				break;
			case 10:
				list = Globals.Instance.CardItemByType[Enums.CardType.Accesory];
				break;
			case 11:
				list = Globals.Instance.CardItemByType[Enums.CardType.Pet];
				break;
			case 22:
				list = Globals.Instance.CardListByType[Enums.CardType.Enchantment];
				break;
			}
			break;
		}
		monsterButton?.transform.gameObject.SetActive(flag);
		cardList.Clear();
		for (int i = 0; i < list.Count; i++)
		{
			CardData cardData = Globals.Instance.GetCardData(list[i], instantiate: false);
			if (cardData != null && !cardData.ShowInTome)
			{
				continue;
			}
			if (searchTerm.Trim() != "")
			{
				if (index == 22 && cardData.CardClass == Enums.CardClass.Monster)
				{
					continue;
				}
				if (Globals.Instance.IsInSearch(searchTerm, list[i]))
				{
					cardList.Add(list[i]);
				}
				if (cardData != null && index != 22)
				{
					if (cardData.UpgradesTo1 != "" && Globals.Instance.IsInSearch(searchTerm, cardData.UpgradesTo1))
					{
						cardList.Add(cardData.UpgradesTo1);
					}
					if (cardData.UpgradesTo2 != "" && Globals.Instance.IsInSearch(searchTerm, cardData.UpgradesTo2))
					{
						cardList.Add(cardData.UpgradesTo2);
					}
					if (cardData.UpgradesToRare != null && Globals.Instance.IsInSearch(searchTerm, cardData.UpgradesToRare.Id))
					{
						cardList.Add(cardData.UpgradesToRare.Id);
					}
				}
			}
			else if (index != 22 || (cardData.CardUpgraded == Enums.CardUpgraded.No && cardData.CardClass != Enums.CardClass.Monster))
			{
				cardList.Add(list[i]);
			}
		}
		cardList.Sort();
		TomeManager tomeManager = this;
		TomeManager tomeManager2 = this;
		int num = 0;
		tomeManager2.pageAct = 0;
		tomeManager.pageOld = num;
		pageMax = Mathf.CeilToInt((float)cardList.Count / (float)numCards);
		RedoPageNumbers();
		ActivateDeactivateButtons(index);
		SetPage(1, absolute: true);
	}

	private void ActivateDeactivateButtons(int index)
	{
		for (int i = 0; i < TomeButtons.Length; i++)
		{
			if (TomeButtons[i].transform.gameObject.activeSelf)
			{
				if (TomeButtons[i].tomeClass == index)
				{
					TomeButtons[i].Activate();
				}
				else
				{
					TomeButtons[i].Deactivate();
				}
			}
		}
	}

	private void RedoPageNumbers()
	{
		int num = 14;
		if (pageMax < num)
		{
			num = pageMax;
		}
		for (int i = 0; i < num; i++)
		{
			if (IsActive() && tomeNumbers[i].gameObject.activeSelf)
			{
				tomeNumbers[i].Show();
			}
			tomeNumbers[i].Deactivate();
		}
		for (int j = num; j < maxTomeNumbers; j++)
		{
			tomeNumbers[j].Hide();
		}
		tomeNumbers[0].Activate();
	}

	public void SelectTomeScores(int tomeButton)
	{
		if (tomeButton == 14 && scoreboardType == 1)
		{
			scoreboardType = 0;
			DoScoreboard();
		}
		else if (tomeButton == 15 && scoreboardType == 0)
		{
			scoreboardType = 1;
			DoScoreboard();
		}
	}

	private IEnumerator SetScores()
	{
		if (scoreboardType == 0)
		{
			if (SteamManager.Instance.scoreboardGlobal == null)
			{
				yield break;
			}
		}
		else if (SteamManager.Instance.scoreboardFriends == null)
		{
			yield break;
		}
		GameManager.Instance.PlayLibraryAudio("ui_book_page", 0.2f);
		int num = (pageAct - 1) * numScoreRows;
		int num2 = num + numScoreRows;
		int num3 = pageAct;
		int num4 = 0;
		int num5 = 0;
		playerOnScoreboard = false;
		if (scoreboardType == 0)
		{
			LeaderboardEntry[] scoreboardGlobal = SteamManager.Instance.scoreboardGlobal;
			for (int i = 0; i < scoreboardGlobal.Length; i++)
			{
				LeaderboardEntry leaderboardEntry = scoreboardGlobal[i];
				if (num3 != pageAct)
				{
					break;
				}
				if ((ulong)leaderboardEntry.User.Id != 0L)
				{
					if (num4 >= num && num4 < num2)
					{
						scoresName[num5].gameObject.SetActive(value: true);
						Score obj = scoresName[num5];
						int index = num4 + 1;
						Friend user = leaderboardEntry.User;
						obj.SetScore(index, user.Name, leaderboardEntry.Score, leaderboardEntry.User.Id);
						num5++;
					}
					num4++;
					if (num4 >= num2)
					{
						break;
					}
				}
			}
		}
		else
		{
			LeaderboardEntry[] scoreboardGlobal = SteamManager.Instance.scoreboardFriends;
			for (int i = 0; i < scoreboardGlobal.Length; i++)
			{
				LeaderboardEntry leaderboardEntry2 = scoreboardGlobal[i];
				if (num3 != pageAct)
				{
					break;
				}
				if (num4 >= num && num4 < num2)
				{
					scoresName[num5].gameObject.SetActive(value: true);
					Score obj2 = scoresName[num5];
					int index2 = num4 + 1;
					Friend user = leaderboardEntry2.User;
					obj2.SetScore(index2, user.Name, leaderboardEntry2.Score, leaderboardEntry2.User.Id);
					num5++;
				}
				num4++;
				if (num4 >= num2)
				{
					break;
				}
			}
		}
		for (int j = num5; j < scoresName.Length; j++)
		{
			scoresName[j].gameObject.SetActive(value: false);
		}
		if (!playerOnScoreboard && SteamManager.Instance.scoreboardSingle != null)
		{
			LeaderboardEntry[] scoreboardGlobal = SteamManager.Instance.scoreboardSingle;
			for (int i = 0; i < scoreboardGlobal.Length; i++)
			{
				LeaderboardEntry leaderboardEntry3 = scoreboardGlobal[i];
				scoresName[scoresName.Length - 2].gameObject.SetActive(value: true);
				scoresName[scoresName.Length - 2].SetScore(-1, ".....");
				scoresName[scoresName.Length - 1].gameObject.SetActive(value: true);
				Score obj3 = scoresName[scoresName.Length - 1];
				int globalRank = leaderboardEntry3.GlobalRank;
				Friend user = leaderboardEntry3.User;
				obj3.SetScore(globalRank, user.Name, leaderboardEntry3.Score);
			}
		}
	}

	private IEnumerator SetTomeCards()
	{
		if (cardList == null)
		{
			yield break;
		}
		if (!mainSection.gameObject.activeSelf)
		{
			if (!KeyboardManager.Instance.IsActive())
			{
				GameManager.Instance.PlayLibraryAudio("ui_book_page", 0.1f);
			}
			yield return Globals.Instance.WaitForSeconds(0.06f);
		}
		int indexAct = (pageAct - 1) * numCards;
		int indexTomeCard = -1;
		int pageFor = pageAct;
		int cardCount = cardList.Count;
		GameManager.Instance.CleanTempContainer();
		PopupManager.Instance.ClosePopup();
		for (int i = indexAct; i < indexAct + numCards; i++)
		{
			if (pageFor != pageAct)
			{
				break;
			}
			indexTomeCard++;
			if (i < cardList.Count)
			{
				if (!tomeTs[indexTomeCard].gameObject.activeSelf)
				{
					tomeTs[indexTomeCard].gameObject.SetActive(value: true);
				}
				GameObject gameObject;
				if (cardGO[indexTomeCard] == null)
				{
					gameObject = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, tomeTs[indexTomeCard]);
				}
				else
				{
					gameObject = cardGO[indexTomeCard];
					if (!gameObject.gameObject.activeSelf)
					{
						gameObject.gameObject.SetActive(value: true);
					}
				}
				CardItem component = gameObject.GetComponent<CardItem>();
				gameObject.name = "tomecd_" + i;
				string text = cardList[i];
				component.SetCard(text, deckScale: true, null, null, GetFromGlobal: true);
				component.SetLocalScale(Vector3.one);
				component.CreateColliderAdjusted();
				component.cardoutsidecombat = true;
				component.cardoutsidelibary = true;
				component.TopLayeringOrder("UI", 30100);
				component.HideRarityParticles();
				component.HideCardIconParticles();
				component.ShowSpriteOverCard();
				bool num = PlayerManager.Instance.IsCardUnlocked(text);
				if (!num)
				{
					component.ShowLockedBackground(status: true);
				}
				else
				{
					component.ShowDisable(state: false);
					component.ShowBackImage(state: false);
					component.ShowLockedBackground(status: false);
				}
				component.PlaySightParticle();
				gameObject.transform.localPosition = Vector3.zero;
				cardGO[indexTomeCard] = gameObject;
				if (!num || searchTerm != "")
				{
					tomeCards[indexTomeCard].ShowButtons(state: false);
					tomeCards[indexTomeCard].ShowButtonRare(state: false);
					if (cardGOBlue[indexTomeCard] != null && cardGOBlue[indexTomeCard].gameObject.activeSelf)
					{
						cardGOBlue[indexTomeCard].gameObject.SetActive(value: false);
					}
					if (cardGOGold[indexTomeCard] != null && cardGOGold[indexTomeCard].gameObject.activeSelf)
					{
						cardGOGold[indexTomeCard].gameObject.SetActive(value: false);
					}
					if (cardGORare[indexTomeCard] != null && cardGORare[indexTomeCard].gameObject.activeSelf)
					{
						cardGORare[indexTomeCard].gameObject.SetActive(value: false);
					}
					if (searchTerm == "")
					{
						component.DoTome(showName: true, 1 + i, cardCount);
					}
					else
					{
						component.DoTome(showName: false, 1 + i, cardCount);
					}
				}
				else
				{
					bool flag = false;
					bool flag2 = false;
					bool flag3 = false;
					component.DoTome(showName: false, 1 + i, cardCount);
					if (searchTerm == "")
					{
						tomeCards[indexTomeCard].ShowButtons(state: true);
						float y = -1.452f;
						GameObject gameObject2 = ((!(cardGOBlue[indexTomeCard] == null)) ? cardGOBlue[indexTomeCard] : UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, tomeTs[indexTomeCard]));
						string text2 = Globals.Instance.Cards[text]?.UpgradesTo1.ToLower();
						if (!string.IsNullOrEmpty(text2) && Globals.Instance.Cards.ContainsKey(text2))
						{
							flag = true;
							if (!gameObject2.gameObject.activeSelf)
							{
								gameObject2.gameObject.SetActive(value: true);
							}
							component = gameObject2.GetComponent<CardItem>();
							gameObject2.name = "cardblue_" + i;
							component.SetCard(text2, deckScale: true, null, null, GetFromGlobal: true);
							component.SetLocalScale(new Vector3(0.25f, 0.08f, 1f));
							component.CreateColliderAdjusted();
							component.ShowBackImage(state: true);
							component.cardoutsidecombat = true;
							component.cardoutsidelibary = true;
							component.TopLayeringOrder("UI", 30100);
							gameObject2.transform.localPosition = new Vector3(-0.58f, y, 0f);
							cardGOBlue[indexTomeCard] = gameObject2;
						}
						else
						{
							if (gameObject2.gameObject.activeSelf)
							{
								gameObject2.gameObject.SetActive(value: false);
							}
							tomeCards[indexTomeCard].ShowButtons(state: false);
						}
						GameObject gameObject3 = ((!(cardGOGold[indexTomeCard] == null)) ? cardGOGold[indexTomeCard] : UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, tomeTs[indexTomeCard]));
						text2 = Globals.Instance.Cards[text]?.UpgradesTo2.ToLower();
						if (!string.IsNullOrEmpty(text2) && Globals.Instance.Cards.ContainsKey(text2))
						{
							flag2 = true;
							if (!gameObject3.gameObject.activeSelf)
							{
								gameObject3.gameObject.SetActive(value: true);
							}
							component = gameObject3.GetComponent<CardItem>();
							gameObject3.name = "cardGold_" + i;
							component.SetCard(text2, deckScale: true, null, null, GetFromGlobal: true);
							component.SetLocalScale(new Vector3(0.25f, 0.08f, 1f));
							component.CreateColliderAdjusted();
							component.ShowBackImage(state: true);
							component.cardoutsidecombat = true;
							component.cardoutsidelibary = true;
							component.TopLayeringOrder("UI", 30100);
							gameObject3.transform.localPosition = new Vector3(0.02f, y, 0f);
							cardGOGold[indexTomeCard] = gameObject3;
						}
						else
						{
							if (gameObject3.gameObject.activeSelf)
							{
								gameObject3.gameObject.SetActive(value: false);
							}
							tomeCards[indexTomeCard].ShowButtons(state: false);
						}
						GameObject gameObject4 = ((!(cardGORare[indexTomeCard] == null)) ? cardGORare[indexTomeCard] : UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, tomeTs[indexTomeCard]));
						text2 = Globals.Instance.Cards[text]?.UpgradesToRare?.Id;
						if (!string.IsNullOrEmpty(text2) && Globals.Instance.Cards.ContainsKey(text2))
						{
							flag3 = true;
							if (!gameObject4.gameObject.activeSelf)
							{
								gameObject4.gameObject.SetActive(value: true);
							}
							tomeCards[indexTomeCard].ShowButtonRare(state: true);
							component = gameObject4.GetComponent<CardItem>();
							gameObject4.name = "cardRare_" + i;
							component.SetCard(text2, deckScale: true, null, null, GetFromGlobal: true);
							component.SetLocalScale(new Vector3(0.25f, 0.08f, 1f));
							component.CreateColliderAdjusted();
							component.ShowBackImage(state: true);
							component.cardoutsidecombat = true;
							component.cardoutsidelibary = true;
							component.TopLayeringOrder("UI", 30100);
							gameObject4.transform.localPosition = new Vector3(0.6f, y, 0f);
							cardGORare[indexTomeCard] = gameObject4;
						}
						else
						{
							if (gameObject4.gameObject.activeSelf)
							{
								gameObject4.gameObject.SetActive(value: false);
							}
							tomeCards[indexTomeCard].ShowButtonRare(state: false);
						}
						if (flag && flag2 && flag3)
						{
							tomeCards[indexTomeCard].buttonBlue.transform.localPosition = new Vector3(-0.55f, y, 0f);
							gameObject2.transform.localPosition = new Vector3(-0.58f, y, 0f);
							tomeCards[indexTomeCard].buttonGold.transform.localPosition = new Vector3(0.05f, y, 0f);
							gameObject3.transform.localPosition = new Vector3(0.02f, y, 0f);
							tomeCards[indexTomeCard].buttonRare.transform.localPosition = new Vector3(0.65f, y, 0f);
							gameObject4.transform.localPosition = new Vector3(0.62f, y, 0f);
						}
						else if (flag && flag2 && !flag3)
						{
							tomeCards[indexTomeCard].buttonBlue.transform.localPosition = new Vector3(-0.25f, y, 0f);
							gameObject2.transform.localPosition = new Vector3(-0.28f, y, 0f);
							tomeCards[indexTomeCard].buttonGold.transform.localPosition = new Vector3(0.41f, y, 0f);
							gameObject3.transform.localPosition = new Vector3(0.38f, y, 0f);
						}
						else if (!flag && !flag2 && flag3)
						{
							tomeCards[indexTomeCard].buttonRare.transform.localPosition = new Vector3(0.03f, y, 0f);
							gameObject4.transform.localPosition = new Vector3(0f, y, 0f);
						}
					}
				}
				yield return null;
			}
			else if (tomeTs[indexTomeCard].gameObject.activeSelf)
			{
				tomeTs[indexTomeCard].gameObject.SetActive(value: false);
			}
		}
	}

	public void ShowSearchFocus()
	{
		StartCoroutine(ShowSearchFocusCo());
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
		if (searchBox.gameObject.activeSelf != state)
		{
			searchBox.gameObject.SetActive(state);
		}
	}

	public void ResetSearch()
	{
		searchInput.text = "";
	}

	public void Search(string _term)
	{
		_term = _term.Trim();
		if (searchCo != null)
		{
			StopCoroutine(searchCo);
		}
		searchCo = StartCoroutine(SearchCoroutine(_term.Trim()));
	}

	private IEnumerator SearchCoroutine(string _term)
	{
		if (_term != "")
		{
			yield return Globals.Instance.WaitForSeconds(0.35f);
			searchTerm = _term;
			canvasSearchCloseT.gameObject.SetActive(value: true);
		}
		else
		{
			searchTerm = "";
			canvasSearchCloseT.gameObject.SetActive(value: false);
		}
		SelectTomeCards(activeTomeCards, absolute: true);
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false, int controllerBlockFrom = -1)
	{
		_controllerList.Clear();
		foreach (Transform tomeSideButton in tomeSideButtons)
		{
			if (Functions.TransformIsVisible(tomeSideButton))
			{
				_controllerList.Add(tomeSideButton);
			}
		}
		if (Functions.TransformIsVisible(cardContainer))
		{
			foreach (Transform item2 in cardContainer)
			{
				if (item2.gameObject.activeSelf)
				{
					_controllerList.Add(item2);
				}
			}
		}
		if (Functions.TransformIsVisible(searchInput.transform))
		{
			_controllerList.Add(searchInput.transform);
			if (Functions.TransformIsVisible(canvasSearchCloseT.transform))
			{
				_controllerList.Add(canvasSearchCloseT.transform);
			}
		}
		if (Functions.TransformIsVisible(glossarySection))
		{
			foreach (Transform item3 in glossaryPageIndex)
			{
				if (Functions.TransformIsVisible(item3) && item3.GetChild(0).GetComponent<BotonGeneric>().auxInt > -1)
				{
					_controllerList.Add(item3.GetChild(0).transform);
				}
			}
			foreach (Transform item4 in glossaryPageIndex2)
			{
				if (Functions.TransformIsVisible(item4) && item4.GetChild(0).GetComponent<BotonGeneric>().auxInt > -1)
				{
					_controllerList.Add(item4.GetChild(0).transform);
				}
			}
		}
		if (Functions.TransformIsVisible(runDetails))
		{
			TomeRunDetails component = runDetails.GetComponent<TomeRunDetails>();
			for (int i = 0; i < component.tomeButtons.Length; i++)
			{
				if (Functions.TransformIsVisible(component.tomeButtons[i].transform))
				{
					_controllerList.Add(component.tomeButtons[i].transform);
				}
			}
			for (int j = 0; j < component.characters.Length; j++)
			{
				if (Functions.TransformIsVisible(component.characters[j].transform))
				{
					_controllerList.Add(component.charactersButtons[j].transform);
				}
			}
			if (Functions.TransformIsVisible(component.pathNext))
			{
				_controllerList.Add(component.pathNext);
			}
			if (Functions.TransformIsVisible(component.pathPrev))
			{
				_controllerList.Add(component.pathPrev);
			}
			_controllerList.Add(component.closeButton);
		}
		else if (Functions.TransformIsVisible(runsSection))
		{
			foreach (Transform item5 in runsContainer)
			{
				_controllerList.Add(item5);
			}
		}
		if (Functions.TransformIsVisible(scoreboardSection))
		{
			for (int k = 0; k < scoreButtons.Length; k++)
			{
				_controllerList.Add(scoreButtons[k]);
			}
		}
		for (int l = 0; l < TomeButtons.Length; l++)
		{
			if (Functions.TransformIsVisible(TomeButtons[l].transform))
			{
				_controllerList.Add(TomeButtons[l].transform);
			}
		}
		for (int m = 0; m < tomeNumbers.Length; m++)
		{
			if (Functions.TransformIsVisible(tomeNumbers[m].transform) && tomeNumbers[m].GetComponent<TomeNumber>().IsVisible())
			{
				_controllerList.Add(tomeNumbers[m].transform);
			}
		}
		_controllerList.Add(exitButton);
		controllerHorizontalIndex = Functions.GetListClosestIndexToMousePosition(_controllerList);
		controllerHorizontalIndex = Functions.GetClosestIndexBasedOnDirection(_controllerList, controllerHorizontalIndex, goingUp, goingRight, goingDown, goingLeft);
		if (_controllerList[controllerHorizontalIndex] != null)
		{
			warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerList[controllerHorizontalIndex].position);
			Mouse.current.WarpCursorPosition(warpPosition);
		}
	}

	public void ControllerMoveShoulder(bool _isRight = false)
	{
	}
}
