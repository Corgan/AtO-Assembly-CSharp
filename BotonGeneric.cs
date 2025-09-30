// Decompiled with JetBrains decompiler
// Type: BotonGeneric
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

#nullable disable
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
    this.borderSR = this.border.GetComponent<SpriteRenderer>();
    this.backgroundOverSR = this.backgroundOver.GetComponent<SpriteRenderer>();
    if ((Object) this.backgroundPlain != (Object) null)
      this.backgroundPlainSR = this.backgroundPlain.GetComponent<SpriteRenderer>();
    this.anim = this.GetComponent<Animator>();
  }

  private void Start()
  {
    this.textColor = this.textColorOri = this.text.color;
    this.incrementColor = new Color(0.0f, 0.0f, 0.0f, 0.025f);
    this.totalAlpha = new Color(this.color.r, this.color.g, this.color.b, 0.0f);
    this.SetColor();
    this.StartCoroutine(this.SetTextCoroutine());
    if ((double) this.scaleOriX == 0.0)
      this.scaleOriX = this.transform.localScale.x;
    if ((double) this.scaleOriY == 0.0)
      this.scaleOriY = this.transform.localScale.y;
    if (this.text.transform.childCount > 0)
    {
      for (int index = 0; index < this.text.transform.childCount; ++index)
      {
        MeshRenderer component = this.text.transform.GetChild(index).GetComponent<MeshRenderer>();
        component.sortingLayerName = this.text.GetComponent<MeshRenderer>().sortingLayerName;
        component.sortingOrder = this.text.GetComponent<MeshRenderer>().sortingOrder;
      }
    }
    this.DoShow(false);
  }

  private IEnumerator SetTextCoroutine()
  {
    int limit = 0;
    if (this.idTranslate != "")
    {
      while ((Object) Texts.Instance == (Object) null || Texts.Instance.GetText(this.idTranslate) == "")
      {
        yield return (object) Globals.Instance.WaitForSeconds(0.02f);
        ++limit;
        if (limit > 100)
          break;
      }
      if (this.idTranslate != "" && (Object) Texts.Instance != (Object) null)
        this.SetText(Texts.Instance.GetText(this.idTranslate));
    }
    limit = 0;
    if (this.idPopup != "")
    {
      while ((Object) Texts.Instance == (Object) null || Texts.Instance.GetText(this.idPopup) == "")
      {
        yield return (object) Globals.Instance.WaitForSeconds(0.02f);
        ++limit;
        if (limit > 100)
          break;
      }
      if (this.idPopup != "" && (Object) Texts.Instance != (Object) null)
        this.SetPopupText(Texts.Instance.GetText(this.idPopup));
    }
  }

  public void ShowBackgroundPlain(bool state)
  {
    if (!((Object) this.backgroundPlainSR != (Object) null))
      return;
    this.backgroundPlainSR.gameObject.SetActive(state);
  }

  public void ShowBackground(bool state) => this.backgroundOverSR.gameObject.SetActive(state);

  public void ShowBackgroundDisable(bool state)
  {
    this.backgroundDisable.gameObject.SetActive(state);
  }

  public void SetColorAbsolute(Color _color)
  {
    this.backgroundOverSR.color = _color;
    this.borderSR.color = new Color(_color.r, _color.g, _color.b, 0.0f);
  }

  public void SetColor()
  {
    this.backgroundOverSR.color = this.color;
    this.borderSR.color = this.totalAlpha;
  }

  public void SetBackgroundColor(Color _color) => this.backgroundOverSR.color = _color;

  public void SetBorderColor(Color _color) => this.borderSR.color = _color;

  public void SetBorderColorFromColor()
  {
    this.borderSR.color = new Color(this.color.r, this.color.g, this.color.b, 0.0f);
  }

  public void ResetText()
  {
    if (!(this.idTranslate != ""))
      return;
    this.SetText(Texts.Instance.GetText(this.idTranslate));
  }

  public void SetText(string _text)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if ((double) this.iconSizePercentRelative != 0.0)
    {
      if (!this.idIcon.IsNullOrEmpty())
        stringBuilder.Append(string.Format("<size={0}%><sprite name=", (object) this.iconSizePercentRelative) + this.idIcon + "></size>");
    }
    else if (!this.idIcon.IsNullOrEmpty())
      stringBuilder.Append("<sprite name=" + this.idIcon + "> ");
    stringBuilder.Append(_text);
    this.text.text = stringBuilder.ToString();
  }

  public string GetText() => this.text.text;

  public void SetTextColor(Color color) => this.text.color = color;

  public void ResetTextColor() => this.text.color = this.textColorOri;

  public void SetPopupId(string _id)
  {
    this.idPopup = _id;
    this.SetPopupText(Texts.Instance.GetText(_id));
  }

  public void SetPopupText(string text) => this.popupText = text;

  public void ShowDisableMask(bool state) => this.backgroundDisable.gameObject.SetActive(state);

  public void ShowBorder(bool state)
  {
    this.border.gameObject.SetActive(state);
    if (state)
      this.borderSR.color = new Color(this.borderSR.color.r, this.borderSR.color.g, this.borderSR.color.b, 1f);
    else
      this.borderSR.color = new Color(this.borderSR.color.r, this.borderSR.color.g, this.borderSR.color.b, 0.0f);
  }

  public void HideBorderNow() => this.DoShow(false);

  public void Disable()
  {
    this.buttonEnabled = false;
    this.border.gameObject.SetActive(false);
    this.backgroundDisable.gameObject.SetActive(true);
  }

  public void Enable()
  {
    this.buttonEnabled = true;
    this.border.gameObject.SetActive(true);
    this.backgroundDisable.gameObject.SetActive(false);
  }

  public bool IsEnabled() => this.buttonEnabled;

  public void EnabledButton(bool state) => this.buttonEnabled = state;

  public void PermaBorder(bool state)
  {
    this.permaBorder = state;
    this.DoShow(state);
  }

  private void OnMouseEnter()
  {
    if (AlertManager.Instance.IsActive() || SettingsManager.Instance.IsActive() || PerkTree.Instance.IsActive() && this.gameObject.name.Contains("Character_") || (bool) (Object) MatchManager.Instance && MatchManager.Instance.CardDrag)
      return;
    if (this.popupText != "")
      PopupManager.Instance.SetText(this.popupText, true, alwaysCenter: true);
    if (!this.buttonEnabled)
      return;
    GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonHover);
    GameManager.Instance.SetCursorHover();
    if ((double) this.scaleOriX == 0.0)
      this.scaleOriX = this.transform.localScale.x;
    if ((double) this.scaleOriY == 0.0)
      this.scaleOriY = this.transform.localScale.y;
    if ((double) this.scaleOriX != 0.0 && (double) this.scaleOriY != 0.0)
      this.transform.localScale = new Vector3(this.scaleOriX + 0.05f, this.scaleOriY + 0.05f, 1f);
    if (this.permaBorder)
      return;
    this.DoShow(true);
  }

  private void OnMouseExit()
  {
    if (AlertManager.Instance.IsActive() || SettingsManager.Instance.IsActive() || PerkTree.Instance.IsActive() && this.gameObject.name.Contains("Character_") || (bool) (Object) MatchManager.Instance && MatchManager.Instance.CardDrag)
      return;
    GameManager.Instance.SetCursorPlain();
    if (this.popupText != "")
      PopupManager.Instance.ClosePopup();
    if (!this.buttonEnabled)
      return;
    if ((double) this.scaleOriX != 0.0 && (double) this.scaleOriY != 0.0)
      this.transform.localScale = new Vector3(this.scaleOriX, this.scaleOriY, 1f);
    if (this.permaBorder)
      return;
    this.DoShow(false);
  }

  public void OnMouseUp()
  {
    if (!Functions.ClickedThisTransform(this.transform) || AlertManager.Instance.IsActive() || SettingsManager.Instance.IsActive() || PerkTree.Instance.IsActive() && this.gameObject.name.Contains("Character_"))
      return;
    if (this.idPopup != "")
      PopupManager.Instance.ClosePopup();
    if (!this.buttonEnabled)
      return;
    this.Clicked();
  }

  public async void Clicked()
  {
    BotonGeneric botonGeneric = this;
    GameManager.Instance.SetCursorPlain();
    if (!GameManager.Instance.ConfigUseLegacySounds)
      GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
    if ((Object) botonGeneric.borderSR != (Object) null)
      botonGeneric.borderSR.color = botonGeneric.totalAlpha;
    if ((double) botonGeneric.scaleOriX != 0.0 && (double) botonGeneric.scaleOriY != 0.0)
      botonGeneric.transform.localScale = new Vector3(botonGeneric.scaleOriX, botonGeneric.scaleOriY, 1f);
    botonGeneric.DoShow(false);
    Scene activeScene = SceneManager.GetActiveScene();
    string botName = botonGeneric.gameObject.name;
    if ((bool) (Object) CardCraftManager.Instance)
    {
      bool flag = true;
      switch (botName)
      {
        case "AdvancedCraftButton":
          CardCraftManager.Instance.AdvancedCraft(true);
          break;
        case "AffordableCraftButton":
          CardCraftManager.Instance.AffordableCraft(true);
          break;
        case "ApplyFilterButton":
          CardCraftManager.Instance.ApplyFilter();
          break;
        case "ChallengeReady":
          ChallengeSelectionManager.Instance.Ready();
          break;
        case "ChallengeReroll":
          ChallengeSelectionManager.Instance.RerollFromButton();
          break;
        case "ChallengeSelect1":
          ChallengeSelectionManager.Instance.SelectPack(botonGeneric.auxInt, 0);
          break;
        case "ChallengeSelect2":
          ChallengeSelectionManager.Instance.SelectPack(botonGeneric.auxInt, 1);
          break;
        case "ChallengeSelect3":
          ChallengeSelectionManager.Instance.SelectPack(botonGeneric.auxInt, 2);
          break;
        case "ChallengeSelect4":
          ChallengeSelectionManager.Instance.SelectPack(botonGeneric.auxInt, 3);
          break;
        case "ChallengeSelect5":
          ChallengeSelectionManager.Instance.SelectPack(botonGeneric.auxInt, 4);
          break;
        case "ChallengeSelect6":
          ChallengeSelectionManager.Instance.SelectPack(botonGeneric.auxInt, 5);
          break;
        case "ChallengeSelect7":
          ChallengeSelectionManager.Instance.SelectPack(botonGeneric.auxInt, 6);
          break;
        case "ChallengeSelect8":
          ChallengeSelectionManager.Instance.SelectPack(botonGeneric.auxInt, 7);
          break;
        case "CloseFilterButton":
          CardCraftManager.Instance.ShowFilter(false);
          break;
        case "CorruptButtonAction":
          CardCraftManager.Instance.DoButtonCorruption(botonGeneric.auxInt);
          break;
        case "CorruptCard":
          CardCraftManager.Instance.CorruptCard();
          break;
        case "CorruptCharacterStats":
          if ((bool) (Object) MapManager.Instance)
          {
            MapManager.Instance.ShowCharacterStats();
            break;
          }
          break;
        case "CraftBuyButton":
          CardCraftManager.Instance.BuyCraft(botonGeneric.auxString);
          break;
        case "CraftDivination":
          CardCraftManager.Instance.BuyDivination(botonGeneric.auxInt);
          break;
        case "CraftItemBuyButton":
          CardCraftManager.Instance.BuyItem(botonGeneric.auxString);
          break;
        case "CraftPage":
          CardCraftManager.Instance.ChangePage(botonGeneric.auxInt);
          break;
        case "ExitCraft":
          CardCraftManager.Instance.Ready();
          break;
        case "FilterButton":
          CardCraftManager.Instance.ShowFilter(true);
          break;
        case "RemoveCard":
          CardCraftManager.Instance.RemoveCard();
          break;
        case "RerollItems":
          CardCraftManager.Instance.RerollItems();
          break;
        case "ResetFilterButton":
          CardCraftManager.Instance.ResetFilter();
          break;
        case "ShadyButton":
          CardCraftManager.Instance.BuyShadyDeal();
          break;
        case "UpgradeA":
          CardCraftManager.Instance.BuyUpgrade("A");
          break;
        case "UpgradeB":
          CardCraftManager.Instance.BuyUpgrade("B");
          break;
        default:
          flag = false;
          break;
      }
      if (flag)
      {
        botName = (string) null;
        return;
      }
    }
    switch (botName)
    {
      case "Tutorial_Continue":
        GameManager.Instance.HideTutorialPopup();
        botName = (string) null;
        break;
      case "MainMenu":
        SceneStatic.LoadByName("MainMenu");
        botName = (string) null;
        break;
      case "CloseCardScreen":
        CardScreenManager.Instance.ShowCardScreen(false);
        botName = (string) null;
        break;
      default:
        if (TomeManager.Instance.IsActive())
        {
          switch (botName)
          {
            case "TomeExit":
              if ((Object) TeamManagement.Instance != (Object) null)
              {
                TeamManagement.Instance.EnableDisableTestingPanels(true);
                TeamManagement.Instance.UpdateDeck();
              }
              TomeManager.Instance.ShowTome(false);
              botName = (string) null;
              return;
            case "TomeMainButton":
              TomeManager.Instance.DoTomeMain();
              botName = (string) null;
              return;
            case "TomeCardsButton":
              TomeManager.Instance.DoTomeCards();
              botName = (string) null;
              return;
            case "TomeItemsButton":
              TomeManager.Instance.DoTomeItems();
              botName = (string) null;
              return;
            case "TomeGlossaryButton":
              TomeManager.Instance.DoTomeGlossary();
              botName = (string) null;
              return;
            case "TomeRunsButton":
              TomeManager.Instance.DoTomeRuns();
              botName = (string) null;
              return;
            case "TomeScoreboardButton":
              TomeManager.Instance.DoTomeScoreboard();
              botName = (string) null;
              return;
            default:
              if (botonGeneric.auxString == "GlossaryItemIndex")
              {
                TomeManager.Instance.SetGlossaryPageFromButton(botonGeneric.auxInt);
                botName = (string) null;
                return;
              }
              switch (botName)
              {
                case "TomeChallengeButton":
                  botName = (string) null;
                  return;
                case "TomeMonstersButton":
                  botName = (string) null;
                  return;
                case "ButtonPrevWeekly":
                  TomeManager.Instance.PrevWeekly();
                  botName = (string) null;
                  return;
                case "ButtonNextWeekly":
                  TomeManager.Instance.NextWeekly();
                  botName = (string) null;
                  return;
                case "RunDetailClose":
                  TomeManager.Instance.RunDetailClose();
                  botName = (string) null;
                  return;
                case "tomeCharactersButton":
                  TomeManager.Instance.RunDetailOpenCharacter(botonGeneric.auxInt);
                  botName = (string) null;
                  return;
                case "ButtonPrevPath":
                  TomeManager.Instance.PrevPath();
                  botName = (string) null;
                  return;
                case "ButtonNextPath":
                  TomeManager.Instance.NextPath();
                  botName = (string) null;
                  return;
                default:
                  botName = (string) null;
                  return;
              }
          }
        }
        else if ((bool) (Object) PerkTree.Instance && PerkTree.Instance.IsActive())
        {
          switch (botName)
          {
            case "PerkTreeType":
              PerkTree.Instance.SetCategory(botonGeneric.auxInt);
              botName = (string) null;
              return;
            case "ClosePerkTree":
              PerkTree.Instance.Hide();
              botName = (string) null;
              return;
            case "PerkTreeReset":
              PerkTree.Instance.PerksReset();
              botName = (string) null;
              return;
            case "PerkTreeImport":
              PerkTree.Instance.ImportTree();
              botName = (string) null;
              return;
            case "PerkTreeExport":
              PerkTree.Instance.ExportTree();
              botName = (string) null;
              return;
            case "PerkConfirm":
              PerkTree.Instance.PerksAssignConfirm();
              botName = (string) null;
              return;
            case "PerkSaveSlot":
              PerkTree.Instance.SavePerkSlot(botonGeneric.auxInt);
              botName = (string) null;
              return;
            case "PerkRemoveSlot":
              PerkTree.Instance.RemovePerkSlot(botonGeneric.auxInt);
              botName = (string) null;
              return;
            default:
              botName = (string) null;
              return;
          }
        }
        else
        {
          if (botName == "ExitSideCharacter")
          {
            Transform parent = botonGeneric.transform.parent.transform.parent;
            if (!((Object) parent != (Object) null))
            {
              botName = (string) null;
              break;
            }
            parent.GetComponent<CharacterWindowUI>().Hide();
            botName = (string) null;
            break;
          }
          if ((bool) (Object) LobbyManager.Instance)
          {
            switch (botName)
            {
              case "EUregion":
                LobbyManager.Instance.SetRegion("eu");
                botName = (string) null;
                return;
              case "USregion":
                LobbyManager.Instance.SetRegion("us");
                botName = (string) null;
                return;
              case "ASIAregion":
                LobbyManager.Instance.SetRegion("asia");
                botName = (string) null;
                return;
              case "DisconnectRegion":
                LobbyManager.Instance.DisconnectRegion(true);
                botName = (string) null;
                return;
              default:
                botName = (string) null;
                return;
            }
          }
          else
          {
            switch (botName)
            {
              case "MadnessLevel":
                MadnessManager.Instance.ShowMadness();
                botName = (string) null;
                return;
              case "SandboxMode":
                SandboxManager.Instance.ShowSandbox();
                botName = (string) null;
                return;
              case "WeeklyModifiers":
                MadnessManager.Instance.ShowMadness();
                botName = (string) null;
                return;
              default:
                if (MadnessManager.Instance.IsActive())
                {
                  switch (botName)
                  {
                    case "Madness":
                      MadnessManager.Instance.SelectMadness(botonGeneric.auxInt);
                      botName = (string) null;
                      return;
                    case "MadnessBox":
                      MadnessManager.Instance.SelectMadnessCorruptor(botonGeneric.auxInt);
                      botName = (string) null;
                      return;
                    case "MadnessExit":
                      MadnessManager.Instance.ShowMadness();
                      botName = (string) null;
                      return;
                    case "MadnessConfirm":
                      MadnessManager.Instance.MadnessConfirm();
                      botName = (string) null;
                      return;
                    case "MadnessGoSandbox":
                      AtOManager.Instance.GoSandboxFromMadness();
                      botName = (string) null;
                      return;
                    default:
                      botName = (string) null;
                      return;
                  }
                }
                else if (SandboxManager.Instance.IsActive())
                {
                  if (botName == "SandboxBox" || botName == "SandboxBoxCheck")
                  {
                    SandboxManager.Instance.BoxClick(botonGeneric.auxString, botonGeneric.auxInt);
                    botName = (string) null;
                    return;
                  }
                  switch (botName)
                  {
                    case "SandboxReset":
                      SandboxManager.Instance.SbReset();
                      botName = (string) null;
                      return;
                    case "SandboxExit":
                      SandboxManager.Instance.CloseSandbox();
                      botName = (string) null;
                      return;
                    case "SandboxEnable":
                      SandboxManager.Instance.EnableSandbox();
                      botName = (string) null;
                      return;
                    case "SandboxDisable":
                      SandboxManager.Instance.DisableSandbox();
                      botName = (string) null;
                      return;
                    case "SandboxGoMadness":
                      AtOManager.Instance.GoMadnessFromSandbox();
                      botName = (string) null;
                      return;
                    default:
                      botName = (string) null;
                      return;
                  }
                }
                else
                {
                  if (botName == "GiveGold")
                  {
                    GiveManager.Instance.ShowGive(true);
                    botName = (string) null;
                    return;
                  }
                  if ((bool) (Object) GiveManager.Instance && GiveManager.Instance.IsActive())
                  {
                    switch (botName)
                    {
                      case "GiveShards":
                        GiveManager.Instance.ShowGive(true, 1);
                        botName = (string) null;
                        return;
                      case "GiveClose":
                        GiveManager.Instance.ShowGive(false);
                        botName = (string) null;
                        return;
                      case "GiveWindowGold":
                        GiveManager.Instance.ShowGive(true);
                        botName = (string) null;
                        return;
                      case "GiveWindowDust":
                        GiveManager.Instance.ShowGive(true, 1);
                        botName = (string) null;
                        return;
                      case "GiveMinus1":
                        GiveManager.Instance.Give(-1);
                        botName = (string) null;
                        return;
                      case "GiveMinus20":
                        GiveManager.Instance.Give(-20);
                        botName = (string) null;
                        return;
                      case "GiveMinus100":
                        GiveManager.Instance.Give(-100);
                        botName = (string) null;
                        return;
                      case "GiveMinus1000":
                        GiveManager.Instance.Give(-1000);
                        botName = (string) null;
                        return;
                      case "GivePlus1":
                        GiveManager.Instance.Give(1);
                        botName = (string) null;
                        return;
                      case "GivePlus20":
                        GiveManager.Instance.Give(20);
                        botName = (string) null;
                        return;
                      case "GivePlus100":
                        GiveManager.Instance.Give(100);
                        botName = (string) null;
                        return;
                      case "GivePlus1000":
                        GiveManager.Instance.Give(1000);
                        botName = (string) null;
                        return;
                      case "GivePrev":
                        GiveManager.Instance.PrevTarget();
                        botName = (string) null;
                        return;
                      case "GiveNext":
                        GiveManager.Instance.NextTarget();
                        botName = (string) null;
                        return;
                      case "GiveAction":
                        GiveManager.Instance.GiveAction();
                        botName = (string) null;
                        return;
                      default:
                        botName = (string) null;
                        return;
                    }
                  }
                  else
                  {
                    if (activeScene.name == "CardPlayer")
                    {
                      if (!(botName == "ShuffleGame"))
                      {
                        botName = (string) null;
                        return;
                      }
                      CardPlayerManager.Instance.Shuffle();
                      botName = (string) null;
                      return;
                    }
                    if (activeScene.name == "CardPlayerPairs")
                    {
                      switch (botName)
                      {
                        case "ShuffleGame":
                          CardPlayerPairsManager.Instance.Shuffle();
                          botName = (string) null;
                          return;
                        case "FinishPairGame":
                          CardPlayerPairsManager.Instance.FinishPairGame();
                          botName = (string) null;
                          return;
                        default:
                          botName = (string) null;
                          return;
                      }
                    }
                    else if (activeScene.name == "HeroSelection")
                    {
                      switch (botName)
                      {
                        case "BeginAdventure":
                          HeroSelectionManager.Instance.BeginAdventure();
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
                          HeroSelectionManager.Instance.LevelWithSupplies(botonGeneric.auxString);
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
                          HeroSelectionManager.Instance.CardBacksPopUp.GetComponent<CardBackSelectionPanel>().EnableTab(botonGeneric.auxInt);
                          break;
                        case "CardBackSelectionPage":
                          HeroSelectionManager.Instance.CardBacksPopUp.GetComponent<CardBackSelectionPanel>().EnablePage(botonGeneric.auxInt);
                          break;
                        case "CloseCardBackPanel":
                          HeroSelectionManager.Instance.CardBacksPopUp.SetActive(false);
                          break;
                      }
                      string s = botName;
                      // ISSUE: reference to a compiler-generated method
                      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(s))
                      {
                        case 450629136:
                          if (!(s == "ShowAllWarriors"))
                          {
                            botName = (string) null;
                            return;
                          }
                          HeroSelectionManager.Instance.ShowHeroesByFilterAsync("warrior");
                          botName = (string) null;
                          return;
                        case 531191382:
                          if (!(s == "ShowAllMages"))
                          {
                            botName = (string) null;
                            return;
                          }
                          HeroSelectionManager.Instance.ShowHeroesByFilterAsync("mage");
                          botName = (string) null;
                          return;
                        case 1856084312:
                          if (!(s == "ShowAllMultiClass"))
                          {
                            botName = (string) null;
                            return;
                          }
                          HeroSelectionManager.Instance.ShowHeroesByFilterAsync("dlc");
                          botName = (string) null;
                          return;
                        case 3045918655:
                          if (!(s == "ShowAllHealers"))
                          {
                            botName = (string) null;
                            return;
                          }
                          HeroSelectionManager.Instance.ShowHeroesByFilterAsync("healer");
                          botName = (string) null;
                          return;
                        case 3181515171:
                          if (!(s == "ShowAllHeroes"))
                          {
                            botName = (string) null;
                            return;
                          }
                          HeroSelectionManager.Instance.ShowHeroesByFilterAsync("all");
                          botName = (string) null;
                          return;
                        case 3565395377:
                          if (!(s == "ShowAllLocked"))
                          {
                            botName = (string) null;
                            return;
                          }
                          HeroSelectionManager.Instance.ShowHeroesByFilterAsync("locked");
                          botName = (string) null;
                          return;
                        case 3988766812:
                          if (!(s == "ShowAllScouts"))
                          {
                            botName = (string) null;
                            return;
                          }
                          HeroSelectionManager.Instance.ShowHeroesByFilterAsync("scout");
                          botName = (string) null;
                          return;
                        default:
                          botName = (string) null;
                          return;
                      }
                    }
                    else
                    {
                      if (activeScene.name == "IntroNewGame")
                      {
                        if (!(botName == "Skip"))
                        {
                          botName = (string) null;
                          return;
                        }
                        IntroNewGameManager.Instance.SkipIntro();
                        botName = (string) null;
                        return;
                      }
                      if (activeScene.name == "Combat")
                      {
                        switch (botName)
                        {
                          case "Character_CombatDeck":
                            MatchManager.Instance.ShowCharacterWindow("combatdeck");
                            botName = (string) null;
                            return;
                          case "Character_CombatDiscard":
                            MatchManager.Instance.ShowCharacterWindow("combatdiscard");
                            botName = (string) null;
                            return;
                          case "Character_CombatVanish":
                            MatchManager.Instance.ShowCharacterWindow("combatvanish");
                            botName = (string) null;
                            return;
                          case "Character_Level":
                            MatchManager.Instance.ShowCharacterWindow("level");
                            botName = (string) null;
                            return;
                          case "Character_Items":
                            MatchManager.Instance.ShowCharacterWindow("items");
                            botName = (string) null;
                            return;
                          case "Character_Stats":
                            MatchManager.Instance.ShowCharacterWindow("stats");
                            botName = (string) null;
                            return;
                          case "Character_Perks":
                            MatchManager.Instance.ShowCharacterWindow("perks");
                            botName = (string) null;
                            return;
                          case "NPC_Casted":
                            MatchManager.Instance.ShowCharacterWindow("combatdiscard", false);
                            botName = (string) null;
                            return;
                          case "NPC_Stats":
                            MatchManager.Instance.ShowCharacterWindow("stats", false);
                            botName = (string) null;
                            return;
                          case "CombatConsoleClose":
                            MatchManager.Instance.ShowLog();
                            botName = (string) null;
                            return;
                          default:
                            botName = (string) null;
                            return;
                        }
                      }
                      else if (activeScene.name == "Town")
                      {
                        switch (botName)
                        {
                          case "Ready":
                            if ((Object) AtOManager.Instance.GetTownDivinationTier() != (Object) null)
                            {
                              botName = (string) null;
                              return;
                            }
                            TownManager.Instance.Ready();
                            botName = (string) null;
                            return;
                          case "PointsTownUpgrade":
                            PlayerManager.Instance.GainSupply(10);
                            botName = (string) null;
                            return;
                          case "ResetTownUpgrade":
                            PlayerManager.Instance.ResetTownUpgrade();
                            TownManager.Instance.RefreshTownUpgrades();
                            botName = (string) null;
                            return;
                          case "ClaimTreasure":
                            if (botonGeneric.auxInt == 1000)
                            {
                              TownManager.Instance.ClaimTreasureCommunity(botonGeneric.auxString);
                              botName = (string) null;
                              return;
                            }
                            TownManager.Instance.ClaimTreasure(botonGeneric.auxString);
                            botName = (string) null;
                            return;
                          case "Join_Divination":
                            AtOManager.Instance.JoinCardDivination();
                            botName = (string) null;
                            return;
                          case "Character_Deck":
                            TownManager.Instance.ShowCharacterWindow("deck");
                            botName = (string) null;
                            return;
                          case "Character_Level":
                            TownManager.Instance.ShowCharacterWindow("level");
                            botName = (string) null;
                            return;
                          case "Character_Items":
                            TownManager.Instance.ShowCharacterWindow("items");
                            botName = (string) null;
                            return;
                          case "Character_Stats":
                            TownManager.Instance.ShowCharacterWindow("stats");
                            botName = (string) null;
                            return;
                          case "Character_Perks":
                            TownManager.Instance.ShowCharacterWindow("perks");
                            botName = (string) null;
                            return;
                          case "PetShop":
                            CardCraftManager.Instance.DoPetShop();
                            botName = (string) null;
                            return;
                          case "ItemShop":
                            CardCraftManager.Instance.DoItemShop();
                            botName = (string) null;
                            return;
                          case "ExitTownUpgrades":
                            TownManager.Instance.ShowTownUpgrades(false);
                            botName = (string) null;
                            return;
                          case "SaveLoad":
                            CardCraftManager.Instance.ShowSaveLoad();
                            botName = (string) null;
                            return;
                          case "SaveSlot":
                            CardCraftManager.Instance.SaveDeck(botonGeneric.auxInt);
                            botName = (string) null;
                            return;
                          case "RemoveSlot":
                            CardCraftManager.Instance.RemoveDeck(botonGeneric.auxInt);
                            botName = (string) null;
                            return;
                          case "BotCraftingDeck":
                            CardCraftManager.Instance.CraftDeck();
                            botName = (string) null;
                            return;
                          case "SellSupply":
                            TownManager.Instance.SellSupply();
                            botName = (string) null;
                            return;
                          case "SupplyClose":
                            TownManager.Instance.CloseSupply();
                            botName = (string) null;
                            return;
                          case "SupplyMinus1":
                            TownManager.Instance.ModifySupplyQuantity(-1);
                            botName = (string) null;
                            return;
                          case "SupplyMinus5":
                            TownManager.Instance.ModifySupplyQuantity(-5);
                            botName = (string) null;
                            return;
                          case "SupplyPlus1":
                            TownManager.Instance.ModifySupplyQuantity(1);
                            botName = (string) null;
                            return;
                          case "SupplyPlus5":
                            TownManager.Instance.ModifySupplyQuantity(5);
                            botName = (string) null;
                            return;
                          case "SellSupplyAction":
                            TownManager.Instance.SellSupplyAction();
                            botName = (string) null;
                            return;
                          default:
                            botName = (string) null;
                            return;
                        }
                      }
                      else if (activeScene.name == "Rewards")
                      {
                        switch (botName)
                        {
                          case "Dust":
                            if (GameManager.Instance.IsMultiplayer())
                            {
                              botonGeneric.transform.parent.transform.parent.GetComponent<CharacterReward>().DustSelected(NetworkManager.Instance.GetPlayerNick());
                              botName = (string) null;
                              return;
                            }
                            botonGeneric.transform.parent.transform.parent.GetComponent<CharacterReward>().DustSelected("");
                            botName = (string) null;
                            return;
                          case "Character_Deck":
                            RewardsManager.Instance.ShowCharacterWindow("deck");
                            botName = (string) null;
                            return;
                          case "Character_Level":
                            RewardsManager.Instance.ShowCharacterWindow("level");
                            botName = (string) null;
                            return;
                          case "Character_Items":
                            RewardsManager.Instance.ShowCharacterWindow("items");
                            botName = (string) null;
                            return;
                          case "Character_Stats":
                            RewardsManager.Instance.ShowCharacterWindow("stats");
                            botName = (string) null;
                            return;
                          case "Character_Perks":
                            RewardsManager.Instance.ShowCharacterWindow("perks");
                            botName = (string) null;
                            return;
                          case "RestartRewards":
                            RewardsManager.Instance.RestartRewards();
                            botName = (string) null;
                            return;
                          default:
                            botName = (string) null;
                            return;
                        }
                      }
                      else if (activeScene.name == "Loot")
                      {
                        switch (botName)
                        {
                          case "GoldLoot":
                            LootManager.Instance.LootGold();
                            botName = (string) null;
                            return;
                          case "Character_Deck":
                            LootManager.Instance.ShowCharacterWindow("deck");
                            botName = (string) null;
                            return;
                          case "Character_Level":
                            LootManager.Instance.ShowCharacterWindow("level");
                            botName = (string) null;
                            return;
                          case "Character_Items":
                            LootManager.Instance.ShowCharacterWindow("items");
                            botName = (string) null;
                            return;
                          case "Character_Stats":
                            LootManager.Instance.ShowCharacterWindow("stats");
                            botName = (string) null;
                            return;
                          case "Character_Perks":
                            LootManager.Instance.ShowCharacterWindow("perks");
                            botName = (string) null;
                            return;
                          case "RestartLoot":
                            LootManager.Instance.RestartLoot();
                            botName = (string) null;
                            return;
                          default:
                            botName = (string) null;
                            return;
                        }
                      }
                      else
                      {
                        if (!(activeScene.name == "Map"))
                        {
                          botName = (string) null;
                          return;
                        }
                        switch (botName)
                        {
                          case "EventContinue":
                            MapManager.Instance.EventReady();
                            botName = (string) null;
                            return;
                          case "CharacterUnlockClose":
                            MapManager.Instance.CharacterUnlockClose();
                            botName = (string) null;
                            return;
                          case "Character_Deck":
                            MapManager.Instance.ShowCharacterWindow("deck");
                            botName = (string) null;
                            return;
                          case "Character_Level":
                            MapManager.Instance.ShowCharacterWindow("level");
                            botName = (string) null;
                            return;
                          case "Character_Items":
                            MapManager.Instance.ShowCharacterWindow("items");
                            botName = (string) null;
                            return;
                          case "Character_Stats":
                            MapManager.Instance.ShowCharacterWindow("stats");
                            botName = (string) null;
                            return;
                          case "Character_Perks":
                            MapManager.Instance.ShowCharacterWindow("perks");
                            botName = (string) null;
                            return;
                          case "DevLevelUpCharacter":
                            CharacterWindowUI componentInParent = botonGeneric.GetComponentInParent<CharacterWindowUI>();
                            if (!((Object) componentInParent != (Object) null))
                            {
                              botName = (string) null;
                              return;
                            }
                            componentInParent.GrantExperienceForLevelUp();
                            botName = (string) null;
                            return;
                          case "CorruptionBox":
                            MapManager.Instance.CorruptionBox();
                            botName = (string) null;
                            return;
                          case "CorruptionContinue":
                            MapManager.Instance.CorruptionContinue();
                            botName = (string) null;
                            return;
                          case "CorruptionHide":
                            MapManager.Instance.CorruptionShowHide();
                            botName = (string) null;
                            return;
                          case "CorruptionRewardA":
                            MapManager.Instance.CorruptionSelectReward("A");
                            botName = (string) null;
                            return;
                          case "CorruptionRewardB":
                            MapManager.Instance.CorruptionSelectReward("B");
                            botName = (string) null;
                            return;
                          case "ConflictButton":
                            MapManager.Instance.ConflictSelection(botonGeneric.auxInt);
                            botName = (string) null;
                            return;
                          case "EventShowHide":
                            MapManager.Instance.ShowHideEvent();
                            botName = (string) null;
                            return;
                          default:
                            botName = (string) null;
                            return;
                        }
                      }
                    }
                  }
                }
            }
          }
        }
    }
  }

  public void DoShow(bool _show)
  {
    this.show = _show;
    if (this.borderCo != null)
      this.StopCoroutine(this.borderCo);
    this.borderCo = this.StartCoroutine(this.DoShowCo());
  }

  private IEnumerator DoShowCo()
  {
    bool exit = false;
    while (!exit)
    {
      if (this.buttonEnabled)
      {
        if (this.show && (double) this.borderSR.color.a < 0.60000002384185791)
          this.borderSR.color += this.incrementColor;
        else if (!this.show && (double) this.borderSR.color.a > 0.0)
          this.borderSR.color -= this.incrementColor;
        else
          exit = true;
      }
      else
        exit = true;
      yield return (object) null;
    }
  }
}
