using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterWindowUI : MonoBehaviour
{
	public SpriteRenderer portraitSR;

	public Transform exitButton;

	public Transform nonCombatButtons;

	public Transform combatButtons;

	public Transform globalButtons;

	public Transform npcButtons;

	public Transform mainCharacterButtons;

	public DeckEnergy deckEnergy;

	public DeckWindowUI deckWindow;

	public LevelWindowUI levelWindow;

	public ItemsWindowUI itemsWindow;

	public StatsWindowUI statsWindow;

	public PerksWindowUI perksWindow;

	public Image imageBg;

	public Transform elements;

	public Transform buttons;

	public SpriteRenderer borderDecoBg;

	public SpriteRenderer characterLevelSprite;

	public TMP_Text characterLevelText;

	public TMP_Text perkText;

	public TraitLevel[] traitLevel;

	public TMP_Text[] traitLevelText;

	public Transform[] itemCardsBack;

	public CardItem[] itemCardsCI;

	public BotonGeneric botDeck;

	public BotonGeneric botLevel;

	public BotonGeneric botItems;

	public BotonGeneric botStats;

	public BotonGeneric botPerks;

	public Transform botPerksSeparator;

	public BotonGeneric botCombatDeck;

	public BotonGeneric botCombatDiscard;

	public BotonGeneric botCombatVanish;

	public BotonGeneric botNPCCasted;

	public BotonGeneric botNPCStats;

	[SerializeField]
	private GameObject botLevelupCharacter;

	private SubClassData currentSCD;

	private Hero currentHero;

	private NPC currentNPC;

	public int heroIndex;

	private int npcIndex = -1;

	private string activeTab = "deck";

	private Coroutine coroutineMask;

	private bool windowActive;

	public List<BotonGeneric> allButtons;

	public int controllerHorizontalIndex = -1;

	private Vector2 warpPosition = Vector2.zero;

	private List<Transform> _controllerList = new List<Transform>();

	private void Awake()
	{
		HideAllWindows();
	}

	private void Start()
	{
		Resize();
		botLevelupCharacter.SetActive(value: false);
		if (GameManager.Instance.GetDeveloperMode() || AtOManager.Instance.IsCombatTool || (GameManager.Instance.CheatMode && GameManager.Instance.EnableButtons))
		{
			botLevelupCharacter.SetActive(value: true);
		}
	}

	public void Resize()
	{
		GetComponent<RectTransform>().sizeDelta = new Vector2(Globals.Instance.sizeW, Globals.Instance.sizeH);
		exitButton.transform.localPosition = new Vector3((0f - Globals.Instance.sizeW) * 0.5f + 1f * Globals.Instance.multiplierX, (0f - Globals.Instance.sizeH) * 0.5f + 3.9f * Globals.Instance.multiplierY, exitButton.transform.localPosition.z);
	}

	public bool IsActive()
	{
		return elements.gameObject.activeSelf;
	}

	public void ShowUpgradedCards(List<string> upgradedCards)
	{
		if (upgradedCards.Count > 0)
		{
			HideButtons();
			Show("unlockedCards", -2);
			deckWindow.ShowUpgradedCards(upgradedCards);
		}
	}

	public void ShowUnlockedCards(List<string> unlockedCards)
	{
		if (unlockedCards.Count > 0)
		{
			HideButtons();
			Show("unlockedCards", -2);
			deckWindow.ShowUnlockedCards(unlockedCards);
		}
	}

	public void Show(string _element = "", int _heroIndex = -1, bool isHero = true)
	{
		if (!IsActive() && _element != "unlockedCards")
		{
			GameManager.Instance.PlayLibraryAudio("action_bag");
		}
		base.gameObject.SetActive(value: true);
		PopupManager.Instance.ClosePopup();
		portraitSR.gameObject.SetActive(value: false);
		npcButtons.gameObject.SetActive(value: false);
		globalButtons.gameObject.SetActive(value: true);
		if ((bool)MatchManager.Instance)
		{
			globalButtons.transform.localPosition = new Vector3(globalButtons.transform.localPosition.x, -0.7f, globalButtons.transform.localPosition.z);
		}
		else
		{
			globalButtons.transform.localPosition = new Vector3(globalButtons.transform.localPosition.x, 0f, globalButtons.transform.localPosition.z);
		}
		if (GameManager.Instance.IsObeliskChallenge())
		{
			botPerks.gameObject.SetActive(value: false);
			botPerksSeparator.gameObject.SetActive(value: false);
		}
		else
		{
			botPerks.gameObject.SetActive(value: true);
			botPerksSeparator.gameObject.SetActive(value: true);
		}
		if (isHero)
		{
			npcIndex = -1;
		}
		if (_element != "unlockedCards")
		{
			if (!MatchManager.Instance)
			{
				combatButtons.gameObject.SetActive(value: false);
				nonCombatButtons.gameObject.SetActive(value: true);
			}
			else
			{
				combatButtons.gameObject.SetActive(value: true);
				nonCombatButtons.gameObject.SetActive(value: false);
			}
			ShowMask(state: true);
			if (isHero)
			{
				buttons.gameObject.SetActive(value: true);
				if (_heroIndex == -1)
				{
					_heroIndex = heroIndex;
				}
				if (_heroIndex > -1)
				{
					heroIndex = _heroIndex;
					currentHero = AtOManager.Instance.GetHero(heroIndex);
					if (currentHero.HeroData == null)
					{
						return;
					}
					UpdateLevelUpButtonState();
					currentSCD = currentHero.HeroData.HeroSubClass;
					if (currentSCD.MainCharacter)
					{
						mainCharacterButtons.gameObject.SetActive(value: true);
					}
					else
					{
						mainCharacterButtons.gameObject.SetActive(value: false);
					}
				}
			}
			else if (MatchManager.Instance != null)
			{
				if (_heroIndex == -1)
				{
					npcIndex = heroIndex;
				}
				else
				{
					npcIndex = (heroIndex = _heroIndex);
				}
				currentNPC = MatchManager.Instance.GetNPCCharacter(npcIndex);
				if (currentNPC == null)
				{
					return;
				}
			}
			deckEnergy.gameObject.SetActive(value: true);
		}
		else
		{
			ShowMask(state: true, instant: true);
			deckEnergy.gameObject.SetActive(value: false);
		}
		if (_element == "")
		{
			_element = ((windowActive || !currentHero.IsReadyForLevelUp()) ? activeTab : "level");
		}
		if (_element != "perks")
		{
			activeTab = _element;
			HideAllWindows(_element);
		}
		switch (_element)
		{
		case "deck":
			deckWindow.Show(_heroIndex);
			botDeck.Disable();
			botDeck.SetTextColor(new Color(1f, 0.6f, 0.07f));
			botDeck.transform.localPosition = new Vector3(7.4f, botDeck.transform.localPosition.y, botDeck.transform.localPosition.z);
			if (!(RewardsManager.Instance != null))
			{
				_ = LootManager.Instance != null;
			}
			if (CardCraftManager.Instance != null)
			{
				CardCraftManager.Instance.gameObject.SetActive(value: false);
			}
			deckEnergy.WriteEnergy(_heroIndex, 0);
			break;
		case "combatdeck":
			deckWindow.Show(_heroIndex);
			botCombatDeck.Disable();
			botCombatDeck.SetTextColor(new Color(1f, 0.6f, 0.07f));
			botCombatDeck.transform.localPosition = new Vector3(7.4f, botCombatDeck.transform.localPosition.y, botCombatDeck.transform.localPosition.z);
			deckEnergy.WriteEnergy(_heroIndex, 1);
			break;
		case "combatdiscard":
		{
			if (isHero)
			{
				deckWindow.Show(_heroIndex, null, discard: true);
				botCombatDiscard.Disable();
				botCombatDiscard.SetTextColor(new Color(1f, 0.6f, 0.07f));
				botCombatDiscard.transform.localPosition = new Vector3(7.4f, botCombatDiscard.transform.localPosition.y, botCombatDiscard.transform.localPosition.z);
				deckEnergy.WriteEnergy(_heroIndex, 2);
				break;
			}
			deckEnergy.gameObject.SetActive(value: false);
			portraitSR.gameObject.SetActive(value: true);
			portraitSR.sprite = currentNPC.SpritePortrait;
			List<string> list = new List<string>();
			List<string> nPCCardsCastedList = MatchManager.Instance.GetNPCCardsCastedList(currentNPC.InternalId);
			for (int num = nPCCardsCastedList.Count - 1; num >= 0; num--)
			{
				list.Add(nPCCardsCastedList[num]);
			}
			deckWindow.Show(npcIndex, list, discard: false, sort: false);
			deckWindow.HideInjury();
			deckWindow.SetTitle(Texts.Instance.GetText("heroCastedCards").Replace("<hero>", currentNPC.SourceName), list.Count);
			buttons.gameObject.SetActive(value: true);
			nonCombatButtons.gameObject.SetActive(value: false);
			combatButtons.gameObject.SetActive(value: false);
			globalButtons.gameObject.SetActive(value: false);
			npcButtons.gameObject.SetActive(value: true);
			botNPCCasted.Disable();
			botNPCCasted.SetTextColor(new Color(1f, 0.6f, 0.07f));
			botNPCCasted.transform.localPosition = new Vector3(7.4f, botNPCCasted.transform.localPosition.y, botNPCCasted.transform.localPosition.z);
			break;
		}
		case "combatvanish":
		{
			deckEnergy.WriteEnergy(_heroIndex, 3);
			List<string> heroVanish = MatchManager.Instance.GetHeroVanish(_heroIndex);
			deckWindow.Show(0, heroVanish, discard: false, sort: false);
			deckWindow.HideInjury();
			deckWindow.SetTitle(Texts.Instance.GetText("heroVanishedCards").Replace("<hero>", currentHero.SourceName), heroVanish.Count);
			botCombatVanish.Disable();
			botCombatVanish.SetTextColor(new Color(1f, 0.6f, 0.07f));
			botCombatVanish.transform.localPosition = new Vector3(7.4f, botCombatVanish.transform.localPosition.y, botCombatVanish.transform.localPosition.z);
			break;
		}
		case "level":
			levelWindow.Show(_heroIndex);
			botLevel.Disable();
			botLevel.SetTextColor(new Color(1f, 0.6f, 0.07f));
			botLevel.transform.localPosition = new Vector3(7.4f, botLevel.transform.localPosition.y, botLevel.transform.localPosition.z);
			DoLevelWindow();
			break;
		case "perks":
			PerkTree.Instance.Show(currentHero.HeroData.HeroSubClass.Id, heroIndex);
			break;
		case "items":
			botItems.Disable();
			botItems.SetTextColor(new Color(1f, 0.6f, 0.07f));
			botItems.transform.localPosition = new Vector3(7.4f, botItems.transform.localPosition.y, botItems.transform.localPosition.z);
			DoItemsWindow();
			break;
		case "deckreward":
			deckWindow.Show(_heroIndex);
			botDeck.Disable();
			botDeck.SetTextColor(new Color(1f, 0.6f, 0.07f));
			botDeck.transform.localPosition = new Vector3(7.4f, botDeck.transform.localPosition.y, botDeck.transform.localPosition.z);
			break;
		case "stats":
			if (isHero)
			{
				statsWindow.DoStats(currentHero);
				botStats.Disable();
				botStats.SetTextColor(new Color(1f, 0.6f, 0.07f));
				botStats.transform.localPosition = new Vector3(7.4f, botStats.transform.localPosition.y, botStats.transform.localPosition.z);
				break;
			}
			portraitSR.gameObject.SetActive(value: true);
			portraitSR.sprite = currentNPC.SpritePortrait;
			buttons.gameObject.SetActive(value: true);
			nonCombatButtons.gameObject.SetActive(value: false);
			combatButtons.gameObject.SetActive(value: false);
			globalButtons.gameObject.SetActive(value: false);
			npcButtons.gameObject.SetActive(value: true);
			statsWindow.DoStats(currentNPC);
			botNPCStats.Disable();
			botNPCStats.SetTextColor(new Color(1f, 0.6f, 0.07f));
			botNPCStats.transform.localPosition = new Vector3(7.4f, botNPCStats.transform.localPosition.y, botNPCStats.transform.localPosition.z);
			break;
		default:
			_ = _element == "unlockedCards";
			break;
		}
		windowActive = true;
		if (isHero && _element != "unlockedCards")
		{
			if (MapManager.Instance != null)
			{
				MapManager.Instance.sideCharacters.InCharacterScreen(state: true);
			}
			else if (TownManager.Instance != null)
			{
				TownManager.Instance.sideCharacters.InCharacterScreen(state: true);
			}
			else if (MatchManager.Instance != null)
			{
				MatchManager.Instance.sideCharacters.InCharacterScreen(state: true);
			}
		}
	}

	public void GrantExperienceForLevelUp()
	{
		if (currentHero != null)
		{
			int experience = Globals.Instance.GetExperienceByLevel(currentHero.Level) - currentHero.Experience;
			currentHero.GrantExperience(experience);
			UpdateLevelUpButtonState();
		}
	}

	private void UpdateLevelUpButtonState()
	{
		if ((GameManager.Instance.GetDeveloperMode() || (GameManager.Instance.CheatMode && GameManager.Instance.EnableButtons) || AtOManager.Instance.IsCombatTool) && currentHero != null)
		{
			botLevelupCharacter.SetActive(currentHero.Level != 5);
		}
	}

	public void ShowMask(bool state, bool instant = false)
	{
		if (coroutineMask != null)
		{
			StopCoroutine(coroutineMask);
		}
		coroutineMask = StartCoroutine(ShowMaskCo(state, instant: true));
	}

	private IEnumerator ShowMaskCo(bool state, bool instant)
	{
		float maxAlplha = 0.935f;
		float step = 0.07f;
		if (instant)
		{
			imageBg.color = new Color(0f, 0f, 0f, maxAlplha);
			yield break;
		}
		float index = imageBg.color.a;
		if (!state)
		{
			while (index > 0f)
			{
				imageBg.color = new Color(0f, 0f, 0f, index);
				index -= step;
				yield return null;
			}
			imageBg.color = new Color(0f, 0f, 0f, 0f);
		}
		else
		{
			while (index < maxAlplha)
			{
				imageBg.color = new Color(0f, 0f, 0f, index);
				index += step;
				yield return null;
			}
			imageBg.color = new Color(0f, 0f, 0f, maxAlplha);
		}
	}

	public void ReDrawLevel()
	{
		if (currentHero != null)
		{
			DrawLevelButtons(currentHero.Level, currentHero.IsReadyForLevelUp());
		}
	}

	private void DoPerksWindow()
	{
		if (currentHero == null)
		{
			Hide();
		}
		else
		{
			perksWindow.DoPerks(currentHero);
		}
	}

	private void DoLevelWindow()
	{
		if (currentHero == null)
		{
			Hide();
			return;
		}
		int level = currentHero.Level;
		bool levelUp = currentHero.IsReadyForLevelUp();
		_ = Globals.Instance.ClassColor[currentHero.ClassName];
		characterLevelSprite.sprite = currentHero.HeroData.HeroSubClass.SpriteBorder;
		DrawLevelButtons(level, levelUp);
	}

	private void DrawLevelButtons(int heroLevel, bool levelUp)
	{
		string color = Globals.Instance.ClassColor[currentHero.ClassName];
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<size=+2>");
		stringBuilder.Append(Texts.Instance.GetText("levelNumber").Replace("<N>", heroLevel.ToString()));
		stringBuilder.Append("</size>");
		stringBuilder.Append("\n");
		stringBuilder.Append("<sprite name=experience> <color=#FC0>");
		stringBuilder.Append(currentHero.Experience);
		stringBuilder.Append("/");
		stringBuilder.Append(Globals.Instance.GetExperienceByLevel(heroLevel));
		stringBuilder.Append("</color>");
		characterLevelText.text = stringBuilder.ToString();
		int characterTier = PlayerManager.Instance.GetCharacterTier("", "trait", currentHero.PerkRank);
		TraitData traitData = GetTraitData(0, 2);
		traitLevel[0].SetColor(color);
		traitLevel[0].SetPosition(0);
		traitLevel[0].SetEnable(_state: true);
		traitLevel[0].SetTrait(traitData, characterTier);
		for (int i = 1; i < 5; i++)
		{
			bool active = false;
			bool enable = false;
			int num = i * 2;
			int num2 = num + 1;
			TraitData traitData2 = GetTraitData(i);
			if (i < heroLevel)
			{
				if (currentHero.HaveTrait(traitData2.Id))
				{
					enable = true;
				}
			}
			else if (i == heroLevel && levelUp && (currentHero.Owner == null || currentHero.Owner == "" || currentHero.Owner == NetworkManager.Instance.GetPlayerNick()))
			{
				active = true;
			}
			traitLevel[num].SetHeroIndex(heroIndex);
			traitLevel[num].SetColor(color);
			traitLevel[num].SetPosition(1);
			traitLevel[num].SetEnable(enable);
			traitLevel[num].SetActive(active);
			traitLevel[num].SetTrait(traitData2, characterTier);
			TraitData traitData3 = GetTraitData(i, 1);
			enable = false;
			active = false;
			if (i < heroLevel)
			{
				if (currentHero.HaveTrait(traitData3.Id))
				{
					enable = true;
				}
			}
			else if (i == heroLevel && levelUp && (currentHero.Owner == null || currentHero.Owner == "" || currentHero.Owner == NetworkManager.Instance.GetPlayerNick()))
			{
				active = true;
			}
			traitLevel[num2].SetHeroIndex(heroIndex);
			traitLevel[num2].SetColor(color);
			traitLevel[num2].SetPosition(2);
			traitLevel[num2].SetEnable(enable);
			traitLevel[num2].SetActive(active);
			traitLevel[num2].SetTrait(traitData3, characterTier);
			StringBuilder stringBuilder2 = new StringBuilder();
			bool flag = false;
			if ((i < heroLevel || (i == heroLevel && levelUp)) && (currentHero.Owner == null || currentHero.Owner == "" || currentHero.Owner == NetworkManager.Instance.GetPlayerNick()))
			{
				flag = true;
			}
			stringBuilder2.Append("<size=+.4>");
			if (flag)
			{
				stringBuilder2.Append("<color=#FC0>");
			}
			stringBuilder2.Append(Texts.Instance.GetText("levelNumber").Replace("<N>", (i + 1).ToString()));
			if (flag)
			{
				stringBuilder2.Append("</color>");
			}
			stringBuilder2.Append("</size>");
			stringBuilder2.Append("\n");
			if (flag)
			{
				stringBuilder2.Append("<color=#EE5A3C>");
			}
			stringBuilder2.Append(Texts.Instance.GetText("incrementMaxHp").Replace("<N>", currentSCD.MaxHp[i].ToString()));
			if (flag)
			{
				stringBuilder2.Append("</color>");
			}
			traitLevelText[i].text = stringBuilder2.ToString();
		}
	}

	private TraitData GetTraitData(int level, int index = 0)
	{
		if (index == 2 && level > 0)
		{
			return Globals.Instance.GetTraitData(currentHero.Traits[level]);
		}
		switch (level)
		{
		case 0:
			return currentSCD.Trait0;
		case 1:
			if (index == 0)
			{
				return currentSCD.Trait1A;
			}
			return currentSCD.Trait1B;
		case 2:
			if (index == 0)
			{
				return currentSCD.Trait2A;
			}
			return currentSCD.Trait2B;
		case 3:
			if (index == 0)
			{
				return currentSCD.Trait3A;
			}
			return currentSCD.Trait3B;
		case 4:
			if (index == 0)
			{
				return currentSCD.Trait4A;
			}
			return currentSCD.Trait4B;
		default:
			return null;
		}
	}

	private void SetCardbacks()
	{
		string cardbackUsed = currentHero.CardbackUsed;
		if (!(cardbackUsed != ""))
		{
			return;
		}
		CardbackData cardbackData = Globals.Instance.GetCardbackData(cardbackUsed);
		if (cardbackData == null)
		{
			cardbackData = Globals.Instance.GetCardbackData(Globals.Instance.GetCardbackBaseIdBySubclass(currentHero.HeroData.HeroSubClass.Id));
		}
		if (cardbackData == null)
		{
			return;
		}
		Sprite cardbackSprite = cardbackData.CardbackSprite;
		if (!(cardbackSprite != null))
		{
			return;
		}
		for (int i = 0; i < itemCardsBack.Length; i++)
		{
			if (itemCardsBack[i] != null)
			{
				itemCardsBack[i].GetComponent<SpriteRenderer>().sprite = cardbackSprite;
			}
		}
	}

	private void DoItemsWindow()
	{
		SetCardbacks();
		for (int i = 0; i < 5; i++)
		{
			string text = "";
			switch (i)
			{
			case 0:
				text = currentHero.Weapon;
				break;
			case 1:
				text = currentHero.Armor;
				break;
			case 2:
				text = currentHero.Jewelry;
				break;
			case 3:
				text = currentHero.Accesory;
				break;
			case 4:
				text = currentHero.Pet;
				break;
			}
			if (text != "")
			{
				CardItem obj = itemCardsCI[i];
				obj.SetCard(text, deckScale: true, currentHero, null, GetFromGlobal: true);
				obj.TopLayeringOrder("UI", 20000);
				obj.transform.localScale = Vector3.zero;
				obj.SetDestinationLocalScale(1.25f);
				obj.cardmakebig = true;
				obj.cardmakebigSize = 1.25f;
				obj.cardmakebigSizeMax = 1.4f;
				obj.active = true;
				obj.lockPosition = true;
				itemCardsBack[i].gameObject.SetActive(value: false);
				itemCardsCI[i].transform.gameObject.SetActive(value: true);
			}
			else
			{
				itemCardsBack[i].gameObject.SetActive(value: true);
				itemCardsCI[i].transform.gameObject.SetActive(value: false);
			}
		}
	}

	public void Hide(bool goToDivination = false)
	{
		if (!IsActive())
		{
			return;
		}
		Functions.DebugLogGD("CHARACTERWINDOW Hide", "trace");
		deckWindow.DestroyDeck();
		HideAllWindows();
		heroIndex = 0;
		activeTab = "deck";
		windowActive = false;
		if (goToDivination)
		{
			return;
		}
		if (CardCraftManager.Instance != null)
		{
			CardCraftManager.Instance.gameObject.SetActive(value: true);
			if (TownManager.Instance != null)
			{
				TownManager.Instance.sideCharacters.ResetCharacters();
			}
			else if (MapManager.Instance != null)
			{
				MapManager.Instance.sideCharacters.ResetCharacters();
			}
		}
		else if (MapManager.Instance != null)
		{
			MapManager.Instance.sideCharacters.ResetCharacters();
			if (!EventManager.Instance)
			{
				MapManager.Instance.sideCharacters.InCharacterScreen(state: false);
			}
			MapManager.Instance.HideCharacterWindow();
		}
		else if (TownManager.Instance != null)
		{
			TownManager.Instance.sideCharacters.ResetCharacters();
			TownManager.Instance.HideCharacterWindow();
		}
		else if (MatchManager.Instance != null)
		{
			MatchManager.Instance.sideCharacters.ResetCharacters();
			MatchManager.Instance.sideCharacters.InCharacterScreen(state: false);
			MatchManager.Instance.HideCharacterWindow();
			MatchManager.Instance.ResetController();
		}
	}

	private void HideButtons()
	{
		buttons.gameObject.SetActive(value: false);
	}

	public void HideAllWindows(string _element = "")
	{
		if ((bool)GameManager.Instance)
		{
			GameManager.Instance.CleanTempContainer();
		}
		if ((bool)PopupManager.Instance)
		{
			PopupManager.Instance.ClosePopup();
		}
		if (buttons.gameObject.activeSelf)
		{
			botDeck.Enable();
			botDeck.SetTextColor(Color.white);
			botDeck.transform.localPosition = new Vector3(7.6f, botDeck.transform.localPosition.y, botDeck.transform.localPosition.z);
			botLevel.Enable();
			botLevel.SetTextColor(Color.white);
			botLevel.transform.localPosition = new Vector3(7.6f, botLevel.transform.localPosition.y, botLevel.transform.localPosition.z);
			botItems.Enable();
			botItems.SetTextColor(Color.white);
			botItems.transform.localPosition = new Vector3(7.6f, botItems.transform.localPosition.y, botItems.transform.localPosition.z);
			botStats.Enable();
			botStats.SetTextColor(Color.white);
			botStats.transform.localPosition = new Vector3(7.6f, botStats.transform.localPosition.y, botStats.transform.localPosition.z);
			botPerks.Enable();
			botPerks.SetTextColor(Color.white);
			botPerks.transform.localPosition = new Vector3(7.6f, botPerks.transform.localPosition.y, botPerks.transform.localPosition.z);
			botCombatDeck.Enable();
			botCombatDeck.SetTextColor(Color.white);
			botCombatDeck.transform.localPosition = new Vector3(7.6f, botCombatDeck.transform.localPosition.y, botCombatDeck.transform.localPosition.z);
			botCombatDiscard.Enable();
			botCombatDiscard.SetTextColor(Color.white);
			botCombatDiscard.transform.localPosition = new Vector3(7.6f, botCombatDiscard.transform.localPosition.y, botCombatDiscard.transform.localPosition.z);
			botCombatVanish.Enable();
			botCombatVanish.SetTextColor(Color.white);
			botCombatVanish.transform.localPosition = new Vector3(7.6f, botCombatVanish.transform.localPosition.y, botCombatVanish.transform.localPosition.z);
			botNPCStats.Enable();
			botNPCStats.SetTextColor(Color.white);
			botNPCStats.transform.localPosition = new Vector3(7.6f, botNPCStats.transform.localPosition.y, botNPCStats.transform.localPosition.z);
			botNPCCasted.Enable();
			botNPCCasted.SetTextColor(Color.white);
			botNPCCasted.transform.localPosition = new Vector3(7.6f, botNPCCasted.transform.localPosition.y, botNPCCasted.transform.localPosition.z);
		}
		if (_element != "deck" && _element != "combatdeck" && _element != "combatdiscard" && _element != "combatvanish")
		{
			deckWindow.gameObject.SetActive(value: false);
		}
		else
		{
			deckWindow.gameObject.SetActive(value: true);
		}
		if (_element != "level")
		{
			levelWindow.gameObject.SetActive(value: false);
		}
		else
		{
			levelWindow.gameObject.SetActive(value: true);
		}
		if (_element != "items")
		{
			itemsWindow.gameObject.SetActive(value: false);
		}
		else
		{
			itemsWindow.gameObject.SetActive(value: true);
		}
		if (_element != "stats")
		{
			statsWindow.gameObject.SetActive(value: false);
		}
		else
		{
			statsWindow.gameObject.SetActive(value: true);
		}
		if (_element != "perks")
		{
			perksWindow.gameObject.SetActive(value: false);
		}
		else
		{
			perksWindow.gameObject.SetActive(value: true);
		}
		if (_element == "unlockedCards")
		{
			deckWindow.gameObject.SetActive(value: true);
		}
		if (_element == "")
		{
			elements.gameObject.SetActive(value: false);
		}
		else
		{
			elements.gameObject.SetActive(value: true);
		}
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false, bool shoulderLeft = false, bool shoulderRight = false)
	{
		_controllerList.Clear();
		for (int i = 0; i < allButtons.Count; i++)
		{
			if (!(allButtons[i] == null) && allButtons[i].IsEnabled() && Functions.TransformIsVisible(allButtons[i].transform))
			{
				_controllerList.Add(allButtons[i].transform);
			}
		}
		for (int j = 0; j < 4; j++)
		{
			if ((bool)TownManager.Instance)
			{
				if (Functions.TransformIsVisible(TownManager.Instance.sideCharacters.charArray[j].transform))
				{
					_controllerList.Add(TownManager.Instance.sideCharacters.charArray[j].transform.GetChild(0).transform);
				}
			}
			else if ((bool)MapManager.Instance)
			{
				if (Functions.TransformIsVisible(MapManager.Instance.sideCharacters.charArray[j].transform))
				{
					_controllerList.Add(MapManager.Instance.sideCharacters.charArray[j].transform.GetChild(0).transform);
				}
			}
			else if ((bool)MatchManager.Instance && Functions.TransformIsVisible(MatchManager.Instance.sideCharacters.charArray[j].transform))
			{
				_controllerList.Add(MatchManager.Instance.sideCharacters.charArray[j].transform.GetChild(0).transform);
			}
		}
		if (deckWindow.gameObject.activeSelf)
		{
			for (int k = 0; k < deckWindow.injuryContent.transform.childCount; k++)
			{
				_controllerList.Add(deckWindow.injuryContent.transform.GetChild(k));
			}
			for (int l = 0; l < deckWindow.deckContent.transform.childCount; l++)
			{
				_controllerList.Add(deckWindow.deckContent.transform.GetChild(l));
			}
		}
		else if (levelWindow.gameObject.activeSelf)
		{
			_controllerList.Add(traitLevel[0].transform);
			_controllerList.Add(traitLevel[2].transform);
			_controllerList.Add(traitLevel[3].transform);
			_controllerList.Add(traitLevel[4].transform);
			_controllerList.Add(traitLevel[5].transform);
			_controllerList.Add(traitLevel[6].transform);
			_controllerList.Add(traitLevel[7].transform);
			_controllerList.Add(traitLevel[8].transform);
			_controllerList.Add(traitLevel[9].transform);
		}
		else if (itemsWindow.gameObject.activeSelf)
		{
			for (int m = 0; m < itemCardsCI.Length; m++)
			{
				if (itemCardsCI[m].transform.gameObject.activeSelf)
				{
					_controllerList.Add(itemCardsCI[m].transform);
				}
			}
		}
		_controllerList.Add(exitButton);
		controllerHorizontalIndex = Functions.GetListClosestIndexToMousePosition(_controllerList);
		controllerHorizontalIndex = Functions.GetClosestIndexBasedOnDirection(_controllerList, controllerHorizontalIndex, goingUp, goingRight, goingDown, goingLeft);
		if (_controllerList[controllerHorizontalIndex] != null)
		{
			if ((bool)_controllerList[controllerHorizontalIndex].GetComponent<CardItem>())
			{
				Canvas.ForceUpdateCanvases();
				Vector3 zero = Vector3.zero;
				zero.y = -1.425f - _controllerList[controllerHorizontalIndex].localPosition.y;
				deckWindow.scrollContent.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
				deckWindow.scrollContent.GetComponent<RectTransform>().anchoredPosition = zero;
			}
			warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerList[controllerHorizontalIndex].position);
			Mouse.current.WarpCursorPosition(warpPosition);
		}
	}

	private void ControllerNextCharacter()
	{
		if (npcIndex != -1)
		{
			return;
		}
		int num = heroIndex;
		if ((bool)CardCraftManager.Instance)
		{
			num = CardCraftManager.Instance.heroIndex;
		}
		bool flag = false;
		while (!flag)
		{
			num++;
			if (num > 3)
			{
				num = 0;
			}
			if (AtOManager.Instance.GetHero(num) != null && AtOManager.Instance.GetHero(num).HeroData != null)
			{
				flag = true;
			}
		}
		GameObject gameObject = GameObject.Find("/SideCharacters/OverCharacter" + num);
		if (gameObject != null)
		{
			gameObject.transform.GetComponent<OverCharacter>().Clicked();
		}
		if (PerkTree.Instance.IsActive())
		{
			ControllerNextOption();
		}
	}

	private void ControllerNextOption()
	{
		Debug.Log("ControllerNextOption" + PerkTree.Instance.IsActive());
		bool flag = false;
		if (PerkTree.Instance.IsActive())
		{
			flag = true;
		}
		int num = -1;
		if (flag)
		{
			PerkTree.Instance.HideAction(checkSubclass: false);
			for (int i = 0; i < allButtons.Count; i++)
			{
				if (!(allButtons[i] == null) && allButtons[i].IsEnabled() && Functions.TransformIsVisible(allButtons[i].transform))
				{
					allButtons[i].Clicked();
					break;
				}
			}
			return;
		}
		for (int j = 0; j < allButtons.Count; j++)
		{
			if (!(allButtons[j] == null) && !allButtons[j].IsEnabled() && Functions.TransformIsVisible(allButtons[j].transform))
			{
				num = j;
				break;
			}
		}
		if (num <= -1)
		{
			return;
		}
		for (int k = num; k < allButtons.Count; k++)
		{
			if (!(allButtons[k] == null) && allButtons[k].IsEnabled() && Functions.TransformIsVisible(allButtons[k].transform))
			{
				allButtons[k].Clicked();
				return;
			}
		}
		for (int l = 0; l < num; l++)
		{
			if (!(allButtons[l] == null) && allButtons[l].IsEnabled() && Functions.TransformIsVisible(allButtons[l].transform))
			{
				allButtons[l].Clicked();
				break;
			}
		}
	}

	public void ControllerMoveShoulder(bool _isRight = false)
	{
		Debug.Log("ControllerMoveShoulder " + _isRight);
		if (!_isRight)
		{
			ControllerNextCharacter();
		}
		else
		{
			ControllerNextOption();
		}
		ControllerMovement(goingUp: false, goingRight: true);
	}
}
