// Decompiled with JetBrains decompiler
// Type: TeamManagement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

#nullable disable
public class TeamManagement : MonoBehaviour
{
  public Dictionary<string, int> CardsSelectedFromTome;
  [SerializeField]
  private GameObject combatBuildPanel;
  [SerializeField]
  private GameObject heroNpcContainer;
  public Dropdown[] dropHero;
  public Dropdown[] dropNPC;
  private List<string> heroOptions = new List<string>();
  private List<string> npcOptions = new List<string>();
  [FormerlySerializedAs("dropTeams")]
  [SerializeField]
  private Dropdown dropTeamConfigs;
  [SerializeField]
  private GameObject heroBuildPanel;
  [SerializeField]
  private TMP_Text txtHeroBuildTitle;
  [SerializeField]
  private TMP_InputField inputHeroTraits;
  [SerializeField]
  private TMP_Text txtHeroTraits;
  [SerializeField]
  private TMP_InputField inputHeroDeck;
  [SerializeField]
  private TMP_InputField inputHeroWeapon;
  [SerializeField]
  private TMP_InputField inputHeroArmor;
  [SerializeField]
  private TMP_InputField inputHeroJewellery;
  [SerializeField]
  private TMP_InputField inputHeroAccessory;
  [SerializeField]
  private TMP_InputField inputHeroPet;
  [SerializeField]
  private TMP_InputField inputHeroPerks;
  [SerializeField]
  private GameObject importExportPanel;
  [SerializeField]
  private TMP_InputField inputImportExport;
  private List<TeamManagement.CombatConfig> combatPrefsData;
  private TeamManagement.CombatConfig currentCombatConfig;
  private int currentHeroID = -1;
  [SerializeField]
  private Toggle sirenQueenToggle;

  public static TeamManagement Instance { get; private set; }

  public void EnableDisableTestingPanels(bool state)
  {
    this.combatBuildPanel.transform.parent.gameObject.SetActive(state);
    this.heroNpcContainer.SetActive(state && this.combatBuildPanel.activeSelf);
  }

  public void SetSirenQueenBattleState(bool state)
  {
    AtOManager.Instance.SirenQueenBattle = state;
    this.currentCombatConfig.sirenQueenCombat = state;
  }

  public void AddToSelectedList(string cardId)
  {
    if (this.CardsSelectedFromTome.ContainsKey(cardId))
      this.CardsSelectedFromTome[cardId]++;
    else
      this.CardsSelectedFromTome.Add(cardId, 1);
  }

  public void UpdateDeck()
  {
    string str = string.Join(",", this.CardsSelectedFromTome.Select<KeyValuePair<string, int>, string>((Func<KeyValuePair<string, int>, string>) (kv => string.Format("{0}:{1}", (object) kv.Key, (object) kv.Value))));
    if (string.IsNullOrEmpty(this.inputHeroDeck.text) || this.inputHeroDeck.text.EndsWith(","))
    {
      this.inputHeroDeck.text += str;
    }
    else
    {
      TMP_InputField inputHeroDeck = this.inputHeroDeck;
      inputHeroDeck.text = inputHeroDeck.text + "," + str;
    }
    this.CardsSelectedFromTome.Clear();
  }

  private void Awake()
  {
    if ((UnityEngine.Object) GameManager.Instance == (UnityEngine.Object) null)
    {
      SceneStatic.LoadByName(nameof (TeamManagement));
    }
    else
    {
      if ((UnityEngine.Object) TeamManagement.Instance == (UnityEngine.Object) null)
        TeamManagement.Instance = this;
      else if ((UnityEngine.Object) TeamManagement.Instance != (UnityEngine.Object) this)
        UnityEngine.Object.Destroy((UnityEngine.Object) this);
      this.combatBuildPanel.SetActive(true);
      this.heroBuildPanel.SetActive(false);
      this.heroNpcContainer.SetActive(true);
      GameManager.Instance.SetCamera();
      GameManager.Instance.SceneLoaded();
    }
  }

  private void Start()
  {
    if (this.CardsSelectedFromTome == null)
      this.CardsSelectedFromTome = new Dictionary<string, int>();
    this.CardsSelectedFromTome.Clear();
    string str = SaveManager.LoadPrefsString("_CombatConfig");
    this.combatPrefsData = !string.IsNullOrEmpty(str) ? JsonConvert.DeserializeObject<List<TeamManagement.CombatConfig>>(str) : new List<TeamManagement.CombatConfig>();
    this.GenerateHeroOptions();
    this.GenerateNPCOptions();
    this.PopulateHeroNPCDropdown(this.heroOptions, this.dropHero);
    this.PopulateHeroNPCDropdown(this.npcOptions, this.dropNPC);
    this.PopulateSavedTeamsDropdown();
    this.LoadConfigs();
    this.dropTeamConfigs.onValueChanged.AddListener(new UnityAction<int>(this.OnTeamDropdownValueChangedHandler));
  }

  private void GenerateHeroOptions()
  {
    this.heroOptions.Add("-----------");
    foreach (string key in GameManager.Instance.GameHeroes.Keys)
      this.heroOptions.Add(key);
    this.heroOptions.Sort();
  }

  private void GenerateNPCOptions()
  {
    this.npcOptions.Add("-----------");
    foreach (string key in Globals.Instance.NPCs.Keys)
      this.npcOptions.Add(Globals.Instance.NPCs[key].Id);
    this.npcOptions.Sort();
  }

  private void PopulateHeroNPCDropdown(List<string> options, Dropdown[] dropdowns)
  {
    foreach (Dropdown dropdown in dropdowns)
    {
      Dropdown drop = dropdown;
      drop.options.Clear();
      drop.AddOptions(options);
      drop.value = 0;
      drop.onValueChanged.AddListener((UnityAction<int>) (_param1 => this.heroNPCDropdownValueChangedHandler(drop)));
    }
  }

  private void PopulateSavedTeamsDropdown()
  {
    this.dropTeamConfigs.options.Clear();
    this.dropTeamConfigs.options.Add(new Dropdown.OptionData("-------"));
    for (int index = 0; index < this.combatPrefsData.Count; ++index)
      this.dropTeamConfigs.options.Add(new Dropdown.OptionData("Config_" + index.ToString()));
    if (SaveManager.PrefsHasKey("_SelectedConfig"))
      this.dropTeamConfigs.value = SaveManager.LoadPrefsInt("_SelectedConfig");
    else
      this.dropTeamConfigs.value = 0;
  }

  private void LoadConfigs(TeamManagement.CombatConfig import = null)
  {
    if (import != null)
    {
      this.dropTeamConfigs.value = 0;
      this.currentCombatConfig = import;
    }
    else if (SaveManager.PrefsHasKey("_SelectedConfig"))
    {
      this.currentCombatConfig = this.combatPrefsData[SaveManager.LoadPrefsInt("_SelectedConfig") - 1].Clone();
    }
    else
    {
      this.currentCombatConfig = new TeamManagement.CombatConfig()
      {
        npcs = new string[4],
        teamHero = new TeamManagement.TeamHero[4],
        sirenQueenCombat = false
      };
      for (int index = 0; index < this.currentCombatConfig.teamHero.Length; ++index)
        this.currentCombatConfig.teamHero[index] = new TeamManagement.TeamHero();
    }
    if (this.currentCombatConfig == null)
      return;
    for (int index = 0; index < this.dropHero.Length; ++index)
      this.dropHero[index].value = string.IsNullOrEmpty(this.currentCombatConfig.teamHero[index].hero) ? 0 : this.heroOptions.IndexOf(this.currentCombatConfig.teamHero[index].hero);
    for (int index = 0; index < this.dropNPC.Length; ++index)
    {
      GameObject.Find("NPC" + (index + 1).ToString() + "Input").GetComponent<TMP_InputField>().text = this.currentCombatConfig.npcs[index];
      if (string.IsNullOrEmpty(this.currentCombatConfig.npcs[index]))
        this.dropNPC[index].value = 0;
    }
    this.sirenQueenToggle.isOn = this.currentCombatConfig.sirenQueenCombat;
    AtOManager.Instance.SirenQueenBattle = this.currentCombatConfig.sirenQueenCombat;
  }

  public void SelectByName(int order)
  {
    string text = GameObject.Find("NPC" + order.ToString() + "Input").GetComponent<TMP_InputField>().text;
    Dropdown component = GameObject.Find("NPC" + order.ToString()).GetComponent<Dropdown>();
    int num = 0;
    foreach (Dropdown.OptionData option in component.options)
    {
      if (option.text.StartsWith(text))
      {
        component.value = num;
        break;
      }
      ++num;
    }
    if (num >= component.options.Count)
      component.value = 0;
    else
      component.value = num;
  }

  private void OnTeamDropdownValueChangedHandler(int value)
  {
    if (value <= 0 || value > this.combatPrefsData.Count)
      return;
    SaveManager.SaveIntoPrefsInt("_SelectedConfig", value);
    this.LoadConfigs();
  }

  private void heroNPCDropdownValueChangedHandler(Dropdown target)
  {
    string str = target.name.Contains("Hero") ? "Hero" : "NPC";
    SpriteRenderer component1 = GameObject.Find("Item" + target.name).GetComponent<SpriteRenderer>();
    TMP_Text component2 = GameObject.Find("Item" + target.name + "/Name").GetComponent<TMP_Text>();
    if (str == "Hero")
    {
      int result;
      int.TryParse(target.name.Replace("Hero", ""), out result);
      if (target.value == 0)
      {
        component1.sprite = (Sprite) null;
        component2.text = "";
        this.currentCombatConfig.teamHero[result - 1] = new TeamManagement.TeamHero()
        {
          hero = string.Empty
        };
      }
      else
      {
        if (this.currentCombatConfig.teamHero[result - 1].hero != target.options[target.value].text)
          this.currentCombatConfig.teamHero[result - 1] = new TeamManagement.TeamHero()
          {
            hero = target.options[target.value].text
          };
        component1.sprite = GameManager.Instance.GameHeroes[target.options[target.value].text].BorderSprite;
        component2.text = target.options[target.value].text;
      }
    }
    else
    {
      int result;
      int.TryParse(target.name.Replace("NPC", ""), out result);
      if (target.value == 0)
      {
        component1.sprite = (Sprite) null;
        component2.text = "";
        this.currentCombatConfig.npcs[result - 1] = string.Empty;
      }
      else
      {
        this.currentCombatConfig.npcs[result - 1] = target.options[target.value].text;
        component1.sprite = Globals.Instance.NPCs[target.options[target.value].text].SpritePortrait;
        component2.text = target.options[target.value].text;
      }
      if (!string.IsNullOrEmpty(component2.text))
        return;
      GameObject.Find("NPC" + result.ToString() + "Input").GetComponent<TMP_InputField>().text = string.Empty;
    }
  }

  public void OnUpdateTeamButtonClickEvent()
  {
    if (this.combatPrefsData.Count <= 0)
    {
      this.OnAddTeamButtonClickEvent();
    }
    else
    {
      this.combatPrefsData[this.dropTeamConfigs.value - 1] = this.currentCombatConfig;
      this.PersistCombatConfigs();
    }
  }

  public void OnAddTeamButtonClickEvent()
  {
    this.combatPrefsData.Add(this.currentCombatConfig);
    this.PersistCombatConfigs();
    SaveManager.SaveIntoPrefsInt("_SelectedConfig", this.dropTeamConfigs.value + 1);
    this.PopulateSavedTeamsDropdown();
    this.dropTeamConfigs.value = this.combatPrefsData.Count;
  }

  public void LaunchCombat()
  {
    this.SetHeroTeam();
    this.SetNPCTeam();
    this.ApplyHeroConfigs();
    SceneStatic.LoadByName("Combat");
  }

  private void SetHeroTeam()
  {
    int position = 0;
    foreach (Dropdown dropdown in this.dropHero)
    {
      if (dropdown.value > 0)
        AtOManager.Instance.SetTeamSingle(GameManager.Instance.GameHeroes[this.heroOptions[dropdown.value]], position);
      else
        AtOManager.Instance.SetTeamSingle((Hero) null, position);
      ++position;
    }
  }

  private void SetNPCTeam()
  {
    int position = 0;
    foreach (Dropdown dropdown in this.dropNPC)
    {
      if (dropdown.value > 0)
        AtOManager.Instance.SetTeamNPCSingle(this.npcOptions[dropdown.value], position);
      else
        AtOManager.Instance.SetTeamNPCSingle("", position);
      ++position;
    }
  }

  private void ApplyHeroConfigs()
  {
    for (int heroPosition = 0; heroPosition < 4; ++heroPosition)
      this.BuildHero(heroPosition);
  }

  public void ShowHeroBuildPanel(int id)
  {
    this.currentHeroID = id;
    int heroPosition = this.currentHeroID - 1;
    string hero = this.currentCombatConfig.teamHero[heroPosition].hero;
    if (hero.Contains("---"))
      return;
    this.txtHeroBuildTitle.text = "Hero: " + hero;
    this.LoadHeroBuildConfigs(heroPosition);
    this.txtHeroTraits.text = this.GetTraitOptions(hero);
    this.combatBuildPanel.SetActive(false);
    this.heroBuildPanel.SetActive(true);
    this.heroNpcContainer.SetActive(false);
  }

  private string GetTraitOptions(string heroSubclass)
  {
    SubClassData subClassData = UnityEngine.Resources.Load<SubClassData>("SubClass/" + heroSubclass);
    StringBuilder stringBuilder = new StringBuilder("Trait Options: 0 - ");
    stringBuilder.Append(subClassData.Trait0.TraitName).Append(",");
    stringBuilder.Append(" 1A - ").Append(subClassData.Trait1A.TraitName).Append(",");
    stringBuilder.Append(" 1B - ").Append(subClassData.Trait1B.TraitName).Append(",");
    stringBuilder.Append(" 2A - ").Append(subClassData.Trait2A.TraitName).Append(",");
    stringBuilder.Append(" 2B - ").Append(subClassData.Trait2B.TraitName).Append(",");
    stringBuilder.Append(" 3A - ").Append(subClassData.Trait3A.TraitName).Append(",");
    stringBuilder.Append(" 3B - ").Append(subClassData.Trait3B.TraitName).Append(",");
    stringBuilder.Append(" 4A - ").Append(subClassData.Trait4A.TraitName).Append(",");
    stringBuilder.Append(" 4B - ").Append(subClassData.Trait4B.TraitName);
    return stringBuilder.ToString();
  }

  public void OnBuildHeroClickEvent()
  {
    this.SaveHeroBuildConfigs(this.currentHeroID - 1);
    this.combatBuildPanel.SetActive(true);
    this.heroBuildPanel.SetActive(false);
    this.heroNpcContainer.SetActive(true);
  }

  private void BuildHero(int heroPosition)
  {
    if (string.IsNullOrEmpty(this.currentCombatConfig.teamHero[heroPosition].hero))
      return;
    Hero gameHero = GameManager.Instance.GameHeroes[this.currentCombatConfig.teamHero[heroPosition].hero];
    if (gameHero == null)
      return;
    gameHero.InitData();
    gameHero.Weapon = gameHero.Armor = gameHero.Jewelry = gameHero.Accesory = gameHero.Pet = string.Empty;
    AtOManager.Instance.SetTeamSingle(gameHero, heroPosition);
    string[] strArray = TeamManagement.RemoveWhitespace(this.currentCombatConfig.teamHero[heroPosition].traits, true).Split(",", StringSplitOptions.None);
    gameHero.Level = 0;
    for (int index = 0; index < strArray.Length; ++index)
      AtOManager.Instance.HeroLevelUp(heroPosition, strArray[index]);
    if (gameHero.Level == 0)
      gameHero.Level = 1;
    if (!string.IsNullOrEmpty(this.currentCombatConfig.teamHero[heroPosition].deck))
      gameHero.Cards = this.GetDeck(TeamManagement.RemoveWhitespace(this.currentCombatConfig.teamHero[heroPosition].deck.Trim(), true));
    AtOManager.Instance.AddItemToHero(heroPosition, TeamManagement.RemoveWhitespace(this.currentCombatConfig.teamHero[heroPosition].weapon, true), nameof (TeamManagement));
    AtOManager.Instance.AddItemToHero(heroPosition, TeamManagement.RemoveWhitespace(this.currentCombatConfig.teamHero[heroPosition].armor, true), nameof (TeamManagement));
    AtOManager.Instance.AddItemToHero(heroPosition, TeamManagement.RemoveWhitespace(this.currentCombatConfig.teamHero[heroPosition].jewellery, true), nameof (TeamManagement));
    AtOManager.Instance.AddItemToHero(heroPosition, TeamManagement.RemoveWhitespace(this.currentCombatConfig.teamHero[heroPosition].accessory, true), nameof (TeamManagement));
    AtOManager.Instance.AddItemToHero(heroPosition, TeamManagement.RemoveWhitespace(this.currentCombatConfig.teamHero[heroPosition].pet, true), nameof (TeamManagement));
    if (this.currentCombatConfig.teamHero[heroPosition].perks == null)
      return;
    foreach (string _perkId in this.ImportPerkTree(this.currentCombatConfig.teamHero[heroPosition].perks.Trim()))
      AtOManager.Instance.AddPerkToHero(heroPosition, _perkId);
  }

  private void LoadHeroBuildConfigs(int heroPosition)
  {
    TeamManagement.TeamHero teamHero = this.currentCombatConfig.teamHero[heroPosition];
    this.inputHeroTraits.text = teamHero.traits;
    this.inputHeroDeck.text = teamHero.deck;
    this.inputHeroWeapon.text = teamHero.weapon;
    this.inputHeroArmor.text = teamHero.armor;
    this.inputHeroJewellery.text = teamHero.jewellery;
    this.inputHeroAccessory.text = teamHero.accessory;
    this.inputHeroPet.text = teamHero.pet;
    this.inputHeroPerks.text = teamHero.perks;
  }

  private void SaveHeroBuildConfigs(int heroPosition)
  {
    this.currentCombatConfig.teamHero[heroPosition].traits = this.inputHeroTraits.text;
    this.currentCombatConfig.teamHero[heroPosition].deck = this.inputHeroDeck.text;
    this.currentCombatConfig.teamHero[heroPosition].weapon = this.inputHeroWeapon.text;
    this.currentCombatConfig.teamHero[heroPosition].armor = this.inputHeroArmor.text;
    this.currentCombatConfig.teamHero[heroPosition].jewellery = this.inputHeroJewellery.text;
    this.currentCombatConfig.teamHero[heroPosition].accessory = this.inputHeroAccessory.text;
    this.currentCombatConfig.teamHero[heroPosition].pet = this.inputHeroPet.text;
    this.currentCombatConfig.teamHero[heroPosition].perks = this.inputHeroPerks.text;
  }

  private void PersistCombatConfigs()
  {
    SaveManager.SaveIntoPrefsString("_CombatConfig", JsonConvert.SerializeObject((object) this.combatPrefsData, Formatting.None));
  }

  private List<string> GetDeck(string deck)
  {
    List<string> deck1 = new List<string>();
    if (string.IsNullOrEmpty(deck))
      return deck1;
    string[] strArray1 = deck.Split(',', StringSplitOptions.None);
    for (int index1 = 0; index1 < strArray1.Length; ++index1)
    {
      if (!strArray1[index1].Contains(":"))
      {
        // ISSUE: explicit reference operation
        ^ref strArray1[index1] += ":";
      }
      string[] strArray2 = strArray1[index1].Split(':', StringSplitOptions.None);
      string str = strArray2[0].Trim();
      int result;
      int.TryParse(strArray2[1].Trim(), out result);
      for (int index2 = 0; index2 < result; ++index2)
        deck1.Add(str);
    }
    return deck1;
  }

  private static string RemoveWhitespace(string str, bool toLower = false)
  {
    if (string.IsNullOrEmpty(str))
      return string.Empty;
    string str1 = Regex.Replace(str, "[\\s']+", "");
    return toLower ? str1.ToLower() : str1;
  }

  private List<string> ImportPerkTree(string code)
  {
    List<string> stringList = new List<string>();
    if (string.IsNullOrEmpty(code))
      return stringList;
    try
    {
      string str1 = Functions.DecompressString(Functions.OnlyAscii(code).Trim());
      if (str1 != "")
      {
        string[] strArray = str1.Split('_', StringSplitOptions.None);
        if (strArray.Length == 2)
        {
          foreach (string str2 in strArray[1].Split('-', StringSplitOptions.None))
            stringList.Add(str2);
        }
        else
          Debug.LogError((object) "Invalid Perk Tree Code");
      }
      return stringList;
    }
    catch
    {
      Debug.LogError((object) "Invalid Perk Tree Code");
      return stringList;
    }
  }

  public void OnExportButtonClickEvent()
  {
    this.inputImportExport.text = "";
    this.inputImportExport.text = JsonConvert.SerializeObject((object) this.currentCombatConfig);
    this.inputImportExport.readOnly = true;
    this.importExportPanel.SetActive(true);
  }

  public void OnImportButtonClickEvent()
  {
    this.inputImportExport.text = "";
    this.inputImportExport.readOnly = false;
    this.importExportPanel.SetActive(true);
  }

  public void OnImportInputSubmitted()
  {
    this.importExportPanel.SetActive(false);
    if (string.IsNullOrEmpty(this.inputImportExport.text))
      return;
    for (int index = 1; index <= 4; ++index)
      GameObject.Find("NPC" + index.ToString() + "Input").GetComponent<TMP_InputField>().text = string.Empty;
    this.LoadConfigs(JsonConvert.DeserializeObject<TeamManagement.CombatConfig>(this.inputImportExport.text));
  }

  [Serializable]
  private class TeamHero
  {
    public string hero;
    public string traits;
    public string deck;
    public string weapon;
    public string armor;
    public string jewellery;
    public string accessory;
    public string pet;
    public string perks;
  }

  [Serializable]
  private class CombatConfig
  {
    public TeamManagement.TeamHero[] teamHero;
    public string[] npcs;
    public bool sirenQueenCombat;

    public TeamManagement.CombatConfig Clone()
    {
      TeamManagement.CombatConfig combatConfig = new TeamManagement.CombatConfig()
      {
        teamHero = new TeamManagement.TeamHero[this.teamHero.Length],
        npcs = new string[this.npcs.Length],
        sirenQueenCombat = this.sirenQueenCombat
      };
      int num1 = 0;
      foreach (TeamManagement.TeamHero teamHero in this.teamHero)
        combatConfig.teamHero[num1++] = new TeamManagement.TeamHero()
        {
          hero = teamHero.hero,
          traits = teamHero.traits,
          deck = teamHero.deck,
          weapon = teamHero.weapon,
          armor = teamHero.armor,
          jewellery = teamHero.jewellery,
          accessory = teamHero.accessory,
          pet = teamHero.pet,
          perks = teamHero.perks
        };
      int num2 = 0;
      foreach (string npc in this.npcs)
        combatConfig.npcs[num2++] = npc;
      return combatConfig;
    }
  }
}
