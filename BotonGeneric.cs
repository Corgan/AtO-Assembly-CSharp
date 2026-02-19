using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

public class BotonGeneric : MonoBehaviour
{
	public Transform border;

	public Transform backgroundPlain;

	public Transform backgroundOver;

	public Transform backgroundDisable;

	public TMP_Text text;

	public string idIcon = "";

	public float iconSizePercentRelative;

	public string idTranslate = "";

	public string idPopup = "";

	private string popupText = "";

	public string auxString = "";

	public int auxInt = -1000;

	public Color color;

	private SpriteRenderer borderSR;

	private SpriteRenderer backgroundOverSR;

	private SpriteRenderer backgroundPlainSR;

	private Animator anim;

	private Color totalAlpha;

	private Color textColor;

	private Color textColorOri;

	private bool show;

	private bool permaBorder;

	public bool buttonEnabled = true;

	private float scaleOriX;

	private float scaleOriY;

	private Coroutine borderCo;

	private Color incrementColor;

	private void Awake()
	{
		borderSR = border.GetComponent<SpriteRenderer>();
		backgroundOverSR = backgroundOver.GetComponent<SpriteRenderer>();
		if (backgroundPlain != null)
		{
			backgroundPlainSR = backgroundPlain.GetComponent<SpriteRenderer>();
		}
		anim = GetComponent<Animator>();
	}

	private void Start()
	{
		textColor = (textColorOri = text.color);
		incrementColor = new Color(0f, 0f, 0f, 0.025f);
		totalAlpha = new Color(color.r, color.g, color.b, 0f);
		SetColor();
		StartCoroutine(SetTextCoroutine());
		if (scaleOriX == 0f)
		{
			scaleOriX = base.transform.localScale.x;
		}
		if (scaleOriY == 0f)
		{
			scaleOriY = base.transform.localScale.y;
		}
		if (text.transform.childCount > 0)
		{
			for (int i = 0; i < text.transform.childCount; i++)
			{
				MeshRenderer component = text.transform.GetChild(i).GetComponent<MeshRenderer>();
				component.sortingLayerName = text.GetComponent<MeshRenderer>().sortingLayerName;
				component.sortingOrder = text.GetComponent<MeshRenderer>().sortingOrder;
			}
		}
		DoShow(_show: false);
	}

	private IEnumerator SetTextCoroutine()
	{
		int limit = 0;
		if (idTranslate != "")
		{
			while (Texts.Instance == null || Texts.Instance.GetText(idTranslate) == "")
			{
				yield return Globals.Instance.WaitForSeconds(0.02f);
				limit++;
				if (limit > 100)
				{
					break;
				}
			}
			if (idTranslate != "" && Texts.Instance != null)
			{
				SetText(Texts.Instance.GetText(idTranslate));
			}
		}
		limit = 0;
		if (!(idPopup != ""))
		{
			yield break;
		}
		while (Texts.Instance == null || Texts.Instance.GetText(idPopup) == "")
		{
			yield return Globals.Instance.WaitForSeconds(0.02f);
			limit++;
			if (limit > 100)
			{
				break;
			}
		}
		if (idPopup != "" && Texts.Instance != null)
		{
			SetPopupText(Texts.Instance.GetText(idPopup));
		}
	}

	public void ShowBackgroundPlain(bool state)
	{
		if (backgroundPlainSR != null)
		{
			backgroundPlainSR.gameObject.SetActive(state);
		}
	}

	public void ShowBackground(bool state)
	{
		backgroundOverSR.gameObject.SetActive(state);
	}

	public void ShowBackgroundDisable(bool state)
	{
		backgroundDisable.gameObject.SetActive(state);
	}

	public void SetColorAbsolute(Color _color)
	{
		backgroundOverSR.color = _color;
		borderSR.color = new Color(_color.r, _color.g, _color.b, 0f);
	}

	public void SetColor()
	{
		backgroundOverSR.color = color;
		borderSR.color = totalAlpha;
	}

	public void SetBackgroundColor(Color _color)
	{
		backgroundOverSR.color = _color;
	}

	public void SetBorderColor(Color _color)
	{
		borderSR.color = _color;
	}

	public void SetBorderColorFromColor()
	{
		borderSR.color = new Color(color.r, color.g, color.b, 0f);
	}

	public void ResetText()
	{
		if (idTranslate != "")
		{
			SetText(Texts.Instance.GetText(idTranslate));
		}
	}

	public void SetText(string _text)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (iconSizePercentRelative != 0f)
		{
			if (!idIcon.IsNullOrEmpty())
			{
				stringBuilder.Append($"<size={iconSizePercentRelative}%><sprite name=" + idIcon + "></size>");
			}
		}
		else if (!idIcon.IsNullOrEmpty())
		{
			stringBuilder.Append("<sprite name=" + idIcon + "> ");
		}
		stringBuilder.Append(_text);
		text.text = stringBuilder.ToString();
	}

	public string GetText()
	{
		return text.text;
	}

	public void SetTextColor(Color color)
	{
		text.color = color;
	}

	public void ResetTextColor()
	{
		text.color = textColorOri;
	}

	public void SetPopupId(string _id)
	{
		idPopup = _id;
		SetPopupText(Texts.Instance.GetText(_id));
	}

	public void SetPopupText(string text)
	{
		popupText = text;
	}

	public void ShowDisableMask(bool state)
	{
		backgroundDisable.gameObject.SetActive(state);
	}

	public void ShowBorder(bool state)
	{
		border.gameObject.SetActive(state);
		if (state)
		{
			borderSR.color = new Color(borderSR.color.r, borderSR.color.g, borderSR.color.b, 1f);
		}
		else
		{
			borderSR.color = new Color(borderSR.color.r, borderSR.color.g, borderSR.color.b, 0f);
		}
	}

	public void HideBorderNow()
	{
		DoShow(_show: false);
	}

	public void Disable()
	{
		buttonEnabled = false;
		border.gameObject.SetActive(value: false);
		backgroundDisable.gameObject.SetActive(value: true);
	}

	public void Enable()
	{
		buttonEnabled = true;
		border.gameObject.SetActive(value: true);
		backgroundDisable.gameObject.SetActive(value: false);
	}

	public bool IsEnabled()
	{
		return buttonEnabled;
	}

	public void EnabledButton(bool state)
	{
		buttonEnabled = state;
	}

	public void PermaBorder(bool state)
	{
		permaBorder = state;
		DoShow(state);
	}

	private void OnMouseEnter()
	{
		if (AlertManager.Instance.IsActive() || SettingsManager.Instance.IsActive() || (PerkTree.Instance.IsActive() && base.gameObject.name.Contains("Character_")) || ((bool)MatchManager.Instance && MatchManager.Instance.CardDrag))
		{
			return;
		}
		if (popupText != "")
		{
			PopupManager.Instance.SetText(popupText, fast: true, "", alwaysCenter: true);
		}
		if (buttonEnabled)
		{
			GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonHover);
			GameManager.Instance.SetCursorHover();
			if (scaleOriX == 0f)
			{
				scaleOriX = base.transform.localScale.x;
			}
			if (scaleOriY == 0f)
			{
				scaleOriY = base.transform.localScale.y;
			}
			if (scaleOriX != 0f && scaleOriY != 0f)
			{
				base.transform.localScale = new Vector3(scaleOriX + 0.05f, scaleOriY + 0.05f, 1f);
			}
			if (!permaBorder)
			{
				DoShow(_show: true);
			}
		}
	}

	private void OnMouseExit()
	{
		if (AlertManager.Instance.IsActive() || SettingsManager.Instance.IsActive() || (PerkTree.Instance.IsActive() && base.gameObject.name.Contains("Character_")) || ((bool)MatchManager.Instance && MatchManager.Instance.CardDrag))
		{
			return;
		}
		GameManager.Instance.SetCursorPlain();
		if (popupText != "")
		{
			PopupManager.Instance.ClosePopup();
		}
		if (buttonEnabled)
		{
			if (scaleOriX != 0f && scaleOriY != 0f)
			{
				base.transform.localScale = new Vector3(scaleOriX, scaleOriY, 1f);
			}
			if (!permaBorder)
			{
				DoShow(_show: false);
			}
		}
	}

	public void OnMouseUp()
	{
		if (Functions.ClickedThisTransform(base.transform) && !AlertManager.Instance.IsActive() && !SettingsManager.Instance.IsActive() && (!PerkTree.Instance.IsActive() || !base.gameObject.name.Contains("Character_")))
		{
			if (idPopup != "")
			{
				PopupManager.Instance.ClosePopup();
			}
			if (buttonEnabled)
			{
				Clicked();
			}
		}
	}

	public async void Clicked()
	{
		GameManager.Instance.SetCursorPlain();
		if (!GameManager.Instance.ConfigUseLegacySounds)
		{
			GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
		}
		if (borderSR != null)
		{
			borderSR.color = totalAlpha;
		}
		if (scaleOriX != 0f && scaleOriY != 0f)
		{
			base.transform.localScale = new Vector3(scaleOriX, scaleOriY, 1f);
		}
		DoShow(_show: false);
		Scene activeScene = SceneManager.GetActiveScene();
		string botName = base.gameObject.name;
		if ((bool)CardCraftManager.Instance)
		{
			bool flag = true;
			switch (botName)
			{
			case "UpgradeA":
				CardCraftManager.Instance.BuyUpgrade("A");
				break;
			case "UpgradeB":
				CardCraftManager.Instance.BuyUpgrade("B");
				break;
			case "RemoveCard":
				CardCraftManager.Instance.RemoveCard();
				break;
			case "ExitCraft":
				CardCraftManager.Instance.Ready();
				break;
			case "CraftBuyButton":
				CardCraftManager.Instance.BuyCraft(auxString);
				break;
			case "CraftItemBuyButton":
				CardCraftManager.Instance.BuyItem(auxString);
				break;
			case "CraftPage":
				CardCraftManager.Instance.ChangePage(auxInt);
				break;
			case "AdvancedCraftButton":
				CardCraftManager.Instance.AdvancedCraft(change: true);
				break;
			case "FilterButton":
				CardCraftManager.Instance.ShowFilter(state: true);
				break;
			case "AffordableCraftButton":
				CardCraftManager.Instance.AffordableCraft(change: true);
				break;
			case "RerollItems":
				CardCraftManager.Instance.RerollItems();
				break;
			case "ApplyFilterButton":
				CardCraftManager.Instance.ApplyFilter();
				break;
			case "ResetFilterButton":
				CardCraftManager.Instance.ResetFilter();
				break;
			case "CloseFilterButton":
				CardCraftManager.Instance.ShowFilter(state: false);
				break;
			case "CraftDivination":
				CardCraftManager.Instance.BuyDivination(auxInt);
				break;
			case "CorruptCard":
				CardCraftManager.Instance.CorruptCard();
				break;
			case "CorruptCharacterStats":
				if ((bool)MapManager.Instance)
				{
					MapManager.Instance.ShowCharacterStats();
				}
				break;
			case "CorruptButtonAction":
				CardCraftManager.Instance.DoButtonCorruption(auxInt);
				break;
			case "ChallengeSelect1":
				ChallengeSelectionManager.Instance.SelectPack(auxInt, 0);
				break;
			case "ChallengeSelect2":
				ChallengeSelectionManager.Instance.SelectPack(auxInt, 1);
				break;
			case "ChallengeSelect3":
				ChallengeSelectionManager.Instance.SelectPack(auxInt, 2);
				break;
			case "ChallengeSelect4":
				ChallengeSelectionManager.Instance.SelectPack(auxInt, 3);
				break;
			case "ChallengeSelect5":
				ChallengeSelectionManager.Instance.SelectPack(auxInt, 4);
				break;
			case "ChallengeSelect6":
				ChallengeSelectionManager.Instance.SelectPack(auxInt, 5);
				break;
			case "ChallengeSelect7":
				ChallengeSelectionManager.Instance.SelectPack(auxInt, 6);
				break;
			case "ChallengeSelect8":
				ChallengeSelectionManager.Instance.SelectPack(auxInt, 7);
				break;
			case "ChallengeReroll":
				ChallengeSelectionManager.Instance.RerollFromButton();
				break;
			case "ChallengeReady":
				ChallengeSelectionManager.Instance.Ready();
				break;
			case "ShadyButton":
				CardCraftManager.Instance.BuyShadyDeal();
				break;
			default:
				flag = false;
				break;
			}
			if (flag)
			{
				return;
			}
		}
		switch (botName)
		{
		case "Tutorial_Continue":
			GameManager.Instance.HideTutorialPopup();
			return;
		case "MainMenu":
			SceneStatic.LoadByName("MainMenu");
			return;
		case "CloseCardScreen":
			CardScreenManager.Instance.ShowCardScreen(_state: false);
			return;
		}
		if (TomeManager.Instance.IsActive())
		{
			switch (botName)
			{
			case "TomeExit":
				if (TeamManagement.Instance != null)
				{
					TeamManagement.Instance.EnableDisableTestingPanels(state: true);
					TeamManagement.Instance.UpdateDeck();
				}
				TomeManager.Instance.ShowTome(_status: false);
				return;
			case "TomeMainButton":
				TomeManager.Instance.DoTomeMain();
				return;
			case "TomeCardsButton":
				TomeManager.Instance.DoTomeCards();
				return;
			case "TomeItemsButton":
				TomeManager.Instance.DoTomeItems();
				return;
			case "TomeGlossaryButton":
				TomeManager.Instance.DoTomeGlossary();
				return;
			case "TomeRunsButton":
				TomeManager.Instance.DoTomeRuns();
				return;
			case "TomeScoreboardButton":
				TomeManager.Instance.DoTomeScoreboard();
				return;
			}
			if (auxString == "GlossaryItemIndex")
			{
				TomeManager.Instance.SetGlossaryPageFromButton(auxInt);
				return;
			}
			switch (botName)
			{
			case "ButtonPrevWeekly":
				TomeManager.Instance.PrevWeekly();
				break;
			case "ButtonNextWeekly":
				TomeManager.Instance.NextWeekly();
				break;
			case "RunDetailClose":
				TomeManager.Instance.RunDetailClose();
				break;
			case "tomeCharactersButton":
				TomeManager.Instance.RunDetailOpenCharacter(auxInt);
				break;
			case "ButtonPrevPath":
				TomeManager.Instance.PrevPath();
				break;
			case "ButtonNextPath":
				TomeManager.Instance.NextPath();
				break;
			case "TomeMonstersButton":
				break;
			}
			return;
		}
		if ((bool)PerkTree.Instance && PerkTree.Instance.IsActive())
		{
			switch (botName)
			{
			case "PerkTreeType":
				PerkTree.Instance.SetCategory(auxInt);
				break;
			case "ClosePerkTree":
				PerkTree.Instance.Hide();
				break;
			case "PerkTreeReset":
				PerkTree.Instance.PerksReset();
				break;
			case "PerkTreeImport":
				PerkTree.Instance.ImportTree();
				break;
			case "PerkTreeExport":
				PerkTree.Instance.ExportTree();
				break;
			case "PerkConfirm":
				PerkTree.Instance.PerksAssignConfirm();
				break;
			case "PerkSaveSlot":
				PerkTree.Instance.SavePerkSlot(auxInt);
				break;
			case "PerkRemoveSlot":
				PerkTree.Instance.RemovePerkSlot(auxInt);
				break;
			}
			return;
		}
		if (botName == "ExitSideCharacter")
		{
			Transform parent = base.transform.parent.transform.parent;
			if (parent != null)
			{
				parent.GetComponent<CharacterWindowUI>().Hide();
			}
			return;
		}
		if ((bool)LobbyManager.Instance)
		{
			switch (botName)
			{
			case "EUregion":
				LobbyManager.Instance.SetRegion("eu");
				break;
			case "USregion":
				LobbyManager.Instance.SetRegion("us");
				break;
			case "ASIAregion":
				LobbyManager.Instance.SetRegion("asia");
				break;
			case "DisconnectRegion":
				LobbyManager.Instance.DisconnectRegion(_fromButton: true);
				break;
			}
			return;
		}
		switch (botName)
		{
		case "MadnessLevel":
			MadnessManager.Instance.ShowMadness();
			return;
		case "SandboxMode":
			SandboxManager.Instance.ShowSandbox();
			return;
		case "WeeklyModifiers":
			MadnessManager.Instance.ShowMadness();
			return;
		}
		if (MadnessManager.Instance.IsActive())
		{
			switch (botName)
			{
			case "Madness":
				MadnessManager.Instance.SelectMadness(auxInt);
				break;
			case "MadnessBox":
				MadnessManager.Instance.SelectMadnessCorruptor(auxInt);
				break;
			case "MadnessExit":
				MadnessManager.Instance.ShowMadness();
				break;
			case "MadnessConfirm":
				MadnessManager.Instance.MadnessConfirm();
				break;
			case "MadnessGoSandbox":
				AtOManager.Instance.GoSandboxFromMadness();
				break;
			}
			return;
		}
		if (SandboxManager.Instance.IsActive())
		{
			switch (botName)
			{
			case "SandboxBox":
			case "SandboxBoxCheck":
				SandboxManager.Instance.BoxClick(auxString, auxInt);
				break;
			case "SandboxReset":
				SandboxManager.Instance.SbReset();
				break;
			case "SandboxExit":
				SandboxManager.Instance.CloseSandbox();
				break;
			case "SandboxEnable":
				SandboxManager.Instance.EnableSandbox();
				break;
			case "SandboxDisable":
				SandboxManager.Instance.DisableSandbox();
				break;
			case "SandboxGoMadness":
				AtOManager.Instance.GoMadnessFromSandbox();
				break;
			}
			return;
		}
		if (botName == "GiveGold")
		{
			GiveManager.Instance.ShowGive(state: true);
			return;
		}
		if ((bool)GiveManager.Instance && GiveManager.Instance.IsActive())
		{
			switch (botName)
			{
			case "GiveShards":
				GiveManager.Instance.ShowGive(state: true, 1);
				break;
			case "GiveClose":
				GiveManager.Instance.ShowGive(state: false);
				break;
			case "GiveWindowGold":
				GiveManager.Instance.ShowGive(state: true);
				break;
			case "GiveWindowDust":
				GiveManager.Instance.ShowGive(state: true, 1);
				break;
			case "GiveMinus1":
				GiveManager.Instance.Give(-1);
				break;
			case "GiveMinus20":
				GiveManager.Instance.Give(-20);
				break;
			case "GiveMinus100":
				GiveManager.Instance.Give(-100);
				break;
			case "GiveMinus1000":
				GiveManager.Instance.Give(-1000);
				break;
			case "GivePlus1":
				GiveManager.Instance.Give(1);
				break;
			case "GivePlus20":
				GiveManager.Instance.Give(20);
				break;
			case "GivePlus100":
				GiveManager.Instance.Give(100);
				break;
			case "GivePlus1000":
				GiveManager.Instance.Give(1000);
				break;
			case "GivePrev":
				GiveManager.Instance.PrevTarget();
				break;
			case "GiveNext":
				GiveManager.Instance.NextTarget();
				break;
			case "GiveAction":
				GiveManager.Instance.GiveAction();
				break;
			}
			return;
		}
		if (activeScene.name == "CardPlayer")
		{
			if (botName == "ShuffleGame")
			{
				CardPlayerManager.Instance.Shuffle();
			}
			return;
		}
		if (activeScene.name == "CardPlayerPairs")
		{
			if (botName == "ShuffleGame")
			{
				CardPlayerPairsManager.Instance.Shuffle();
			}
			else if (botName == "FinishPairGame")
			{
				CardPlayerPairsManager.Instance.FinishPairGame();
			}
			return;
		}
		if (activeScene.name == "HeroSelection")
		{
			switch (botName)
			{
			case "BeginAdventure":
				HeroSelectionManager.Instance.BeginAdventure();
				break;
			case "EnemySelection":
				CombatToolManager.Instance.ShowCombatTool();
				break;
			case "ChangeSeed":
				HeroSelectionManager.Instance.ChangeSeed();
				break;
			case "CharPopStats":
				HeroSelectionManager.Instance.DoCharPopMenu("stats");
				break;
			case "CharPopRank":
				HeroSelectionManager.Instance.DoCharPopMenu("rank");
				break;
			case "CharPopSkins":
				HeroSelectionManager.Instance.DoCharPopMenu("skins");
				break;
			case "CharPopPerks":
				HeroSelectionManager.Instance.DoCharPopMenu("perks");
				break;
			case "CharPopCardbacks":
				HeroSelectionManager.Instance.DoCharPopMenu("cardbacks");
				break;
			case "CharSingularityCards":
				HeroSelectionManager.Instance.DoCharPopMenu("singularityCards");
				break;
			case "SupplyLevelingAction":
				HeroSelectionManager.Instance.LevelWithSupplies(auxString);
				break;
			case "NGBox":
				HeroSelectionManager.Instance.NGBox();
				break;
			case "FollowBox":
				HeroSelectionManager.Instance.FollowTheLeader();
				break;
			case "HeroSelectionReady":
				HeroSelectionManager.Instance.Ready();
				break;
			case "PageLeft":
				HeroSelectionManager.Instance.moveHeroPageLeft();
				await HeroSelectionManager.Instance.UpdateCharSelectArrowStates();
				break;
			case "PageRight":
				HeroSelectionManager.Instance.moveHeroPageRight();
				await HeroSelectionManager.Instance.UpdateCharSelectArrowStates();
				break;
			case "CardBackSectionTab":
				HeroSelectionManager.Instance.CardBacksPopUp.GetComponent<CardBackSelectionPanel>().EnableTab(auxInt);
				break;
			case "CardBackSelectionPage":
				HeroSelectionManager.Instance.CardBacksPopUp.GetComponent<CardBackSelectionPanel>().EnablePage(auxInt);
				break;
			case "CloseCardBackPanel":
				HeroSelectionManager.Instance.CardBacksPopUp.SetActive(value: false);
				break;
			}
			switch (botName)
			{
			case "ShowAllHeroes":
				HeroSelectionManager.Instance.ShowHeroesByFilterAsync("all");
				break;
			case "ShowAllWarriors":
				HeroSelectionManager.Instance.ShowHeroesByFilterAsync("warrior");
				break;
			case "ShowAllScouts":
				HeroSelectionManager.Instance.ShowHeroesByFilterAsync("scout");
				break;
			case "ShowAllMages":
				HeroSelectionManager.Instance.ShowHeroesByFilterAsync("mage");
				break;
			case "ShowAllHealers":
				HeroSelectionManager.Instance.ShowHeroesByFilterAsync("healer");
				break;
			case "ShowAllMultiClass":
				HeroSelectionManager.Instance.ShowHeroesByFilterAsync("dlc");
				break;
			case "ShowAllLocked":
				HeroSelectionManager.Instance.ShowHeroesByFilterAsync("locked");
				break;
			case "TraitsTab":
				HeroSelectionManager.Instance.SelectTab(botName);
				break;
			case "CardsandItemsTab":
				HeroSelectionManager.Instance.SelectTab(botName);
				break;
			case "ResistancesTab":
				HeroSelectionManager.Instance.SelectTab(botName);
				break;
			}
			return;
		}
		if (activeScene.name == "IntroNewGame")
		{
			if (botName == "Skip")
			{
				IntroNewGameManager.Instance.SkipIntro();
			}
			return;
		}
		if (activeScene.name == "Combat")
		{
			switch (botName)
			{
			case "Character_CombatDeck":
				MatchManager.Instance.ShowCharacterWindow("combatdeck");
				break;
			case "Character_CombatDiscard":
				MatchManager.Instance.ShowCharacterWindow("combatdiscard");
				break;
			case "Character_CombatVanish":
				MatchManager.Instance.ShowCharacterWindow("combatvanish");
				break;
			case "Character_Level":
				MatchManager.Instance.ShowCharacterWindow("level");
				break;
			case "Character_Items":
				MatchManager.Instance.ShowCharacterWindow("items");
				break;
			case "Character_Stats":
				MatchManager.Instance.ShowCharacterWindow("stats");
				break;
			case "Character_Perks":
				MatchManager.Instance.ShowCharacterWindow("perks");
				break;
			case "NPC_Casted":
				MatchManager.Instance.ShowCharacterWindow("combatdiscard", isHero: false);
				break;
			case "NPC_Stats":
				MatchManager.Instance.ShowCharacterWindow("stats", isHero: false);
				break;
			case "CombatConsoleClose":
				MatchManager.Instance.ShowLog();
				break;
			}
			return;
		}
		if (activeScene.name == "Town")
		{
			switch (botName)
			{
			case "Ready":
				if (!(AtOManager.Instance.GetTownDivinationTier() != null))
				{
					if (AtOManager.Instance.IsCombatTool)
					{
						CombatToolManager.Instance.LaunchCombat();
					}
					else
					{
						TownManager.Instance.Ready();
					}
				}
				break;
			case "PointsTownUpgrade":
				PlayerManager.Instance.GainSupply(10);
				break;
			case "ResetTownUpgrade":
				PlayerManager.Instance.ResetTownUpgrade();
				TownManager.Instance.RefreshTownUpgrades();
				break;
			case "DevLevelUpCharacter":
			{
				CharacterWindowUI componentInParent = GetComponentInParent<CharacterWindowUI>();
				if (componentInParent != null)
				{
					componentInParent.GrantExperienceForLevelUp();
				}
				break;
			}
			case "ClaimTreasure":
				if (auxInt == 1000)
				{
					TownManager.Instance.ClaimTreasureCommunity(auxString);
				}
				else
				{
					TownManager.Instance.ClaimTreasure(auxString);
				}
				break;
			case "Join_Divination":
				AtOManager.Instance.JoinCardDivination();
				break;
			case "Character_Deck":
				TownManager.Instance.ShowCharacterWindow("deck");
				break;
			case "Character_Level":
				TownManager.Instance.ShowCharacterWindow("level");
				break;
			case "Character_Items":
				TownManager.Instance.ShowCharacterWindow("items");
				break;
			case "Character_Stats":
				TownManager.Instance.ShowCharacterWindow("stats");
				break;
			case "Character_Perks":
				TownManager.Instance.ShowCharacterWindow("perks");
				break;
			case "PetShop":
				CardCraftManager.Instance.DoPetShop();
				break;
			case "ItemShop":
				CardCraftManager.Instance.DoItemShop();
				break;
			case "ExitTownUpgrades":
				TownManager.Instance.ShowTownUpgrades(state: false);
				break;
			case "SaveLoad":
				CardCraftManager.Instance.ShowSaveLoad();
				break;
			case "SaveSlot":
				CardCraftManager.Instance.SaveDeck(auxInt);
				break;
			case "RemoveSlot":
				CardCraftManager.Instance.RemoveDeck(auxInt);
				break;
			case "BotCraftingDeck":
				CardCraftManager.Instance.CraftDeck();
				break;
			case "SellSupply":
				TownManager.Instance.SellSupply();
				break;
			case "SupplyClose":
				TownManager.Instance.CloseSupply();
				break;
			case "SupplyMinus1":
				TownManager.Instance.ModifySupplyQuantity(-1);
				break;
			case "SupplyMinus5":
				TownManager.Instance.ModifySupplyQuantity(-5);
				break;
			case "SupplyPlus1":
				TownManager.Instance.ModifySupplyQuantity(1);
				break;
			case "SupplyPlus5":
				TownManager.Instance.ModifySupplyQuantity(5);
				break;
			case "SellSupplyAction":
				TownManager.Instance.SellSupplyAction();
				break;
			}
			return;
		}
		if (activeScene.name == "Rewards")
		{
			switch (botName)
			{
			case "Dust":
				if (GameManager.Instance.IsMultiplayer())
				{
					base.transform.parent.transform.parent.GetComponent<CharacterReward>().DustSelected(NetworkManager.Instance.GetPlayerNick());
				}
				else
				{
					base.transform.parent.transform.parent.GetComponent<CharacterReward>().DustSelected("");
				}
				break;
			case "Character_Deck":
				RewardsManager.Instance.ShowCharacterWindow("deck");
				break;
			case "Character_Level":
				RewardsManager.Instance.ShowCharacterWindow("level");
				break;
			case "Character_Items":
				RewardsManager.Instance.ShowCharacterWindow("items");
				break;
			case "Character_Stats":
				RewardsManager.Instance.ShowCharacterWindow("stats");
				break;
			case "Character_Perks":
				RewardsManager.Instance.ShowCharacterWindow("perks");
				break;
			case "RestartRewards":
				RewardsManager.Instance.RestartRewards();
				break;
			}
			return;
		}
		if (activeScene.name == "Loot")
		{
			switch (botName)
			{
			case "GoldLoot":
				LootManager.Instance.LootGold();
				break;
			case "Character_Deck":
				LootManager.Instance.ShowCharacterWindow("deck");
				break;
			case "Character_Level":
				LootManager.Instance.ShowCharacterWindow("level");
				break;
			case "Character_Items":
				LootManager.Instance.ShowCharacterWindow("items");
				break;
			case "Character_Stats":
				LootManager.Instance.ShowCharacterWindow("stats");
				break;
			case "Character_Perks":
				LootManager.Instance.ShowCharacterWindow("perks");
				break;
			case "RestartLoot":
				LootManager.Instance.RestartLoot();
				break;
			}
			return;
		}
		if (!(activeScene.name == "Map"))
		{
			return;
		}
		switch (botName)
		{
		case "EventContinue":
			MapManager.Instance.EventReady();
			break;
		case "CharacterUnlockClose":
			MapManager.Instance.CharacterUnlockClose();
			break;
		case "Character_Deck":
			MapManager.Instance.ShowCharacterWindow("deck");
			break;
		case "Character_Level":
			MapManager.Instance.ShowCharacterWindow("level");
			break;
		case "Character_Items":
			MapManager.Instance.ShowCharacterWindow("items");
			break;
		case "Character_Stats":
			MapManager.Instance.ShowCharacterWindow("stats");
			break;
		case "Character_Perks":
			MapManager.Instance.ShowCharacterWindow("perks");
			break;
		case "DevLevelUpCharacter":
		{
			CharacterWindowUI componentInParent2 = GetComponentInParent<CharacterWindowUI>();
			if (componentInParent2 != null)
			{
				componentInParent2.GrantExperienceForLevelUp();
			}
			break;
		}
		case "CorruptionBox":
			MapManager.Instance.CorruptionBox();
			break;
		case "CorruptionContinue":
			MapManager.Instance.CorruptionContinue();
			break;
		case "CorruptionHide":
			MapManager.Instance.CorruptionShowHide();
			break;
		case "CorruptionRewardA":
			MapManager.Instance.CorruptionSelectReward("A");
			break;
		case "CorruptionRewardB":
			MapManager.Instance.CorruptionSelectReward("B");
			break;
		case "ConflictButton":
			MapManager.Instance.ConflictSelection(auxInt);
			break;
		case "EventShowHide":
			MapManager.Instance.ShowHideEvent();
			break;
		}
	}

	public void DoShow(bool _show)
	{
		show = _show;
		if (borderCo != null)
		{
			StopCoroutine(borderCo);
		}
		borderCo = StartCoroutine(DoShowCo());
	}

	private IEnumerator DoShowCo()
	{
		bool exit = false;
		while (!exit)
		{
			if (buttonEnabled)
			{
				if (show && borderSR.color.a < 0.6f)
				{
					borderSR.color += incrementColor;
				}
				else if (!show && borderSR.color.a > 0f)
				{
					borderSR.color -= incrementColor;
				}
				else
				{
					exit = true;
				}
			}
			else
			{
				exit = true;
			}
			yield return null;
		}
	}
}
