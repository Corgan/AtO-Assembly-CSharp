using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TeamManagement : MonoBehaviour
{
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
		public TeamHero[] teamHero;

		public string[] npcs;

		public bool sirenQueenCombat;

		public CombatConfig Clone()
		{
			CombatConfig combatConfig = new CombatConfig
			{
				teamHero = new TeamHero[this.teamHero.Length],
				npcs = new string[npcs.Length],
				sirenQueenCombat = sirenQueenCombat
			};
			int num = 0;
			TeamHero[] array = this.teamHero;
			foreach (TeamHero teamHero in array)
			{
				combatConfig.teamHero[num++] = new TeamHero
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
			}
			num = 0;
			string[] array2 = npcs;
			foreach (string text in array2)
			{
				combatConfig.npcs[num++] = text;
			}
			return combatConfig;
		}
	}

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

	private List<CombatConfig> combatPrefsData;

	private CombatConfig currentCombatConfig;

	private int currentHeroID = -1;

	[SerializeField]
	private Toggle sirenQueenToggle;

	public static TeamManagement Instance { get; private set; }

	public void EnableDisableTestingPanels(bool state)
	{
		combatBuildPanel.transform.parent.gameObject.SetActive(state);
		heroNpcContainer.SetActive(state && combatBuildPanel.activeSelf);
	}

	public void SetSirenQueenBattleState(bool state)
	{
		AtOManager.Instance.SirenQueenBattle = state;
		currentCombatConfig.sirenQueenCombat = state;
	}

	public void AddToSelectedList(string cardId)
	{
		if (CardsSelectedFromTome.ContainsKey(cardId))
		{
			CardsSelectedFromTome[cardId]++;
		}
		else
		{
			CardsSelectedFromTome.Add(cardId, 1);
		}
	}

	public void UpdateDeck()
	{
		string text = string.Join(",", CardsSelectedFromTome.Select((KeyValuePair<string, int> kv) => $"{kv.Key}:{kv.Value}"));
		if (string.IsNullOrEmpty(inputHeroDeck.text) || inputHeroDeck.text.EndsWith(","))
		{
			inputHeroDeck.text += text;
		}
		else
		{
			TMP_InputField tMP_InputField = inputHeroDeck;
			tMP_InputField.text = tMP_InputField.text + "," + text;
		}
		CardsSelectedFromTome.Clear();
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
		combatBuildPanel.SetActive(value: true);
		heroBuildPanel.SetActive(value: false);
		heroNpcContainer.SetActive(value: true);
		GameManager.Instance.SetCamera();
		GameManager.Instance.SceneLoaded();
	}

	private void Start()
	{
		if (CardsSelectedFromTome == null)
		{
			CardsSelectedFromTome = new Dictionary<string, int>();
		}
		CardsSelectedFromTome.Clear();
		string value = SaveManager.LoadPrefsString("_CombatConfig");
		if (string.IsNullOrEmpty(value))
		{
			combatPrefsData = new List<CombatConfig>();
		}
		else
		{
			combatPrefsData = JsonConvert.DeserializeObject<List<CombatConfig>>(value);
		}
		GenerateHeroOptions();
		GenerateNPCOptions();
		PopulateHeroNPCDropdown(heroOptions, dropHero);
		PopulateHeroNPCDropdown(npcOptions, dropNPC);
		PopulateSavedTeamsDropdown();
		LoadConfigs();
		dropTeamConfigs.onValueChanged.AddListener(OnTeamDropdownValueChangedHandler);
	}

	private void GenerateHeroOptions()
	{
		heroOptions.Add("-----------");
		foreach (string key in GameManager.Instance.GameHeroes.Keys)
		{
			heroOptions.Add(key);
		}
		heroOptions.Sort();
	}

	private void GenerateNPCOptions()
	{
		npcOptions.Add("-----------");
		foreach (string key in Globals.Instance.NPCs.Keys)
		{
			npcOptions.Add(Globals.Instance.NPCs[key].Id);
		}
		npcOptions.Sort();
	}

	private void PopulateHeroNPCDropdown(List<string> options, Dropdown[] dropdowns)
	{
		foreach (Dropdown drop in dropdowns)
		{
			drop.options.Clear();
			drop.AddOptions(options);
			drop.value = 0;
			drop.onValueChanged.AddListener(delegate
			{
				heroNPCDropdownValueChangedHandler(drop);
			});
		}
	}

	private void PopulateSavedTeamsDropdown()
	{
		dropTeamConfigs.options.Clear();
		dropTeamConfigs.options.Add(new Dropdown.OptionData("-------"));
		for (int i = 0; i < combatPrefsData.Count; i++)
		{
			dropTeamConfigs.options.Add(new Dropdown.OptionData("Config_" + i));
		}
		if (SaveManager.PrefsHasKey("_SelectedConfig"))
		{
			dropTeamConfigs.value = SaveManager.LoadPrefsInt("_SelectedConfig");
		}
		else
		{
			dropTeamConfigs.value = 0;
		}
	}

	private void LoadConfigs(CombatConfig import = null)
	{
		if (import != null)
		{
			dropTeamConfigs.value = 0;
			currentCombatConfig = import;
		}
		else if (SaveManager.PrefsHasKey("_SelectedConfig"))
		{
			currentCombatConfig = combatPrefsData[SaveManager.LoadPrefsInt("_SelectedConfig") - 1].Clone();
		}
		else
		{
			currentCombatConfig = new CombatConfig
			{
				npcs = new string[4],
				teamHero = new TeamHero[4],
				sirenQueenCombat = false
			};
			for (int i = 0; i < currentCombatConfig.teamHero.Length; i++)
			{
				currentCombatConfig.teamHero[i] = new TeamHero();
			}
		}
		if (currentCombatConfig == null)
		{
			return;
		}
		for (int j = 0; j < dropHero.Length; j++)
		{
			if (!string.IsNullOrEmpty(currentCombatConfig.teamHero[j].hero))
			{
				dropHero[j].value = heroOptions.IndexOf(currentCombatConfig.teamHero[j].hero);
			}
			else
			{
				dropHero[j].value = 0;
			}
		}
		for (int k = 0; k < dropNPC.Length; k++)
		{
			GameObject.Find("NPC" + (k + 1) + "Input").GetComponent<TMP_InputField>().text = currentCombatConfig.npcs[k];
			if (string.IsNullOrEmpty(currentCombatConfig.npcs[k]))
			{
				dropNPC[k].value = 0;
			}
		}
		sirenQueenToggle.isOn = currentCombatConfig.sirenQueenCombat;
		AtOManager.Instance.SirenQueenBattle = currentCombatConfig.sirenQueenCombat;
	}

	public void SelectByName(int order)
	{
		string text = GameObject.Find("NPC" + order + "Input").GetComponent<TMP_InputField>().text;
		Dropdown component = GameObject.Find("NPC" + order).GetComponent<Dropdown>();
		int num = 0;
		foreach (Dropdown.OptionData option in component.options)
		{
			if (option.text.StartsWith(text))
			{
				component.value = num;
				break;
			}
			num++;
		}
		if (num >= component.options.Count)
		{
			component.value = 0;
		}
		else
		{
			component.value = num;
		}
	}

	private void OnTeamDropdownValueChangedHandler(int value)
	{
		if (value > 0 && value <= combatPrefsData.Count)
		{
			SaveManager.SaveIntoPrefsInt("_SelectedConfig", value);
			LoadConfigs();
		}
	}

	private void heroNPCDropdownValueChangedHandler(Dropdown target)
	{
		string obj = (target.name.Contains("Hero") ? "Hero" : "NPC");
		SpriteRenderer component = GameObject.Find("Item" + target.name).GetComponent<SpriteRenderer>();
		TMP_Text component2 = GameObject.Find("Item" + target.name + "/Name").GetComponent<TMP_Text>();
		if (obj == "Hero")
		{
			int.TryParse(target.name.Replace("Hero", ""), out var result);
			if (target.value == 0)
			{
				component.sprite = null;
				component2.text = "";
				currentCombatConfig.teamHero[result - 1] = new TeamHero
				{
					hero = string.Empty
				};
				return;
			}
			if (currentCombatConfig.teamHero[result - 1].hero != target.options[target.value].text)
			{
				currentCombatConfig.teamHero[result - 1] = new TeamHero
				{
					hero = target.options[target.value].text
				};
			}
			component.sprite = GameManager.Instance.GameHeroes[target.options[target.value].text].BorderSprite;
			component2.text = target.options[target.value].text;
		}
		else
		{
			int.TryParse(target.name.Replace("NPC", ""), out var result2);
			if (target.value == 0)
			{
				component.sprite = null;
				component2.text = "";
				currentCombatConfig.npcs[result2 - 1] = string.Empty;
			}
			else
			{
				currentCombatConfig.npcs[result2 - 1] = target.options[target.value].text;
				component.sprite = Globals.Instance.NPCs[target.options[target.value].text].SpritePortrait;
				component2.text = target.options[target.value].text;
			}
			if (string.IsNullOrEmpty(component2.text))
			{
				GameObject.Find("NPC" + result2 + "Input").GetComponent<TMP_InputField>().text = string.Empty;
			}
		}
	}

	public void OnUpdateTeamButtonClickEvent()
	{
		if (combatPrefsData.Count <= 0)
		{
			OnAddTeamButtonClickEvent();
			return;
		}
		combatPrefsData[dropTeamConfigs.value - 1] = currentCombatConfig;
		PersistCombatConfigs();
	}

	public void OnAddTeamButtonClickEvent()
	{
		combatPrefsData.Add(currentCombatConfig);
		PersistCombatConfigs();
		SaveManager.SaveIntoPrefsInt("_SelectedConfig", dropTeamConfigs.value + 1);
		PopulateSavedTeamsDropdown();
		dropTeamConfigs.value = combatPrefsData.Count;
	}

	public void LaunchCombat()
	{
		SetHeroTeam();
		SetNPCTeam();
		ApplyHeroConfigs();
		SceneStatic.LoadByName("Combat");
	}

	private void SetHeroTeam()
	{
		int num = 0;
		Dropdown[] array = dropHero;
		foreach (Dropdown dropdown in array)
		{
			if (dropdown.value > 0)
			{
				Hero hero = GameManager.Instance.GameHeroes[heroOptions[dropdown.value]];
				AtOManager.Instance.SetTeamSingle(hero, num);
			}
			else
			{
				AtOManager.Instance.SetTeamSingle(null, num);
			}
			num++;
		}
	}

	private void SetNPCTeam()
	{
		int num = 0;
		Dropdown[] array = dropNPC;
		foreach (Dropdown dropdown in array)
		{
			if (dropdown.value > 0)
			{
				AtOManager.Instance.SetTeamNPCSingle(npcOptions[dropdown.value], num);
			}
			else
			{
				AtOManager.Instance.SetTeamNPCSingle("", num);
			}
			num++;
		}
	}

	private void ApplyHeroConfigs()
	{
		for (int i = 0; i < 4; i++)
		{
			BuildHero(i);
		}
	}

	public void ShowHeroBuildPanel(int id)
	{
		currentHeroID = id;
		int num = currentHeroID - 1;
		string hero = currentCombatConfig.teamHero[num].hero;
		if (!hero.Contains("---"))
		{
			txtHeroBuildTitle.text = "Hero: " + hero;
			LoadHeroBuildConfigs(num);
			txtHeroTraits.text = GetTraitOptions(hero);
			combatBuildPanel.SetActive(value: false);
			heroBuildPanel.SetActive(value: true);
			heroNpcContainer.SetActive(value: false);
		}
	}

	private string GetTraitOptions(string heroSubclass)
	{
		SubClassData subClassData = Resources.Load<SubClassData>("SubClass/" + heroSubclass);
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
		int heroPosition = currentHeroID - 1;
		SaveHeroBuildConfigs(heroPosition);
		combatBuildPanel.SetActive(value: true);
		heroBuildPanel.SetActive(value: false);
		heroNpcContainer.SetActive(value: true);
	}

	private void BuildHero(int heroPosition)
	{
		if (string.IsNullOrEmpty(currentCombatConfig.teamHero[heroPosition].hero))
		{
			return;
		}
		Hero hero = GameManager.Instance.GameHeroes[currentCombatConfig.teamHero[heroPosition].hero];
		if (hero == null)
		{
			return;
		}
		hero.InitData();
		string text = (hero.Pet = string.Empty);
		string text2 = (hero.Accesory = text);
		string text4 = (hero.Jewelry = text2);
		string weapon = (hero.Armor = text4);
		hero.Weapon = weapon;
		AtOManager.Instance.SetTeamSingle(hero, heroPosition);
		string[] array = RemoveWhitespace(currentCombatConfig.teamHero[heroPosition].traits, toLower: true).Split(",");
		hero.Level = 0;
		for (int i = 0; i < array.Length; i++)
		{
			AtOManager.Instance.HeroLevelUp(heroPosition, array[i]);
		}
		if (hero.Level == 0)
		{
			hero.Level = 1;
		}
		if (!string.IsNullOrEmpty(currentCombatConfig.teamHero[heroPosition].deck))
		{
			hero.Cards = GetDeck(RemoveWhitespace(currentCombatConfig.teamHero[heroPosition].deck.Trim(), toLower: true));
		}
		AtOManager.Instance.AddItemToHero(heroPosition, RemoveWhitespace(currentCombatConfig.teamHero[heroPosition].weapon, toLower: true), "TeamManagement");
		AtOManager.Instance.AddItemToHero(heroPosition, RemoveWhitespace(currentCombatConfig.teamHero[heroPosition].armor, toLower: true), "TeamManagement");
		AtOManager.Instance.AddItemToHero(heroPosition, RemoveWhitespace(currentCombatConfig.teamHero[heroPosition].jewellery, toLower: true), "TeamManagement");
		AtOManager.Instance.AddItemToHero(heroPosition, RemoveWhitespace(currentCombatConfig.teamHero[heroPosition].accessory, toLower: true), "TeamManagement");
		AtOManager.Instance.AddItemToHero(heroPosition, RemoveWhitespace(currentCombatConfig.teamHero[heroPosition].pet, toLower: true), "TeamManagement");
		if (currentCombatConfig.teamHero[heroPosition].perks == null)
		{
			return;
		}
		foreach (string item in ImportPerkTree(currentCombatConfig.teamHero[heroPosition].perks.Trim()))
		{
			AtOManager.Instance.AddPerkToHero(heroPosition, item);
		}
	}

	private void LoadHeroBuildConfigs(int heroPosition)
	{
		TeamHero teamHero = currentCombatConfig.teamHero[heroPosition];
		inputHeroTraits.text = teamHero.traits;
		inputHeroDeck.text = teamHero.deck;
		inputHeroWeapon.text = teamHero.weapon;
		inputHeroArmor.text = teamHero.armor;
		inputHeroJewellery.text = teamHero.jewellery;
		inputHeroAccessory.text = teamHero.accessory;
		inputHeroPet.text = teamHero.pet;
		inputHeroPerks.text = teamHero.perks;
	}

	private void SaveHeroBuildConfigs(int heroPosition)
	{
		currentCombatConfig.teamHero[heroPosition].traits = inputHeroTraits.text;
		currentCombatConfig.teamHero[heroPosition].deck = inputHeroDeck.text;
		currentCombatConfig.teamHero[heroPosition].weapon = inputHeroWeapon.text;
		currentCombatConfig.teamHero[heroPosition].armor = inputHeroArmor.text;
		currentCombatConfig.teamHero[heroPosition].jewellery = inputHeroJewellery.text;
		currentCombatConfig.teamHero[heroPosition].accessory = inputHeroAccessory.text;
		currentCombatConfig.teamHero[heroPosition].pet = inputHeroPet.text;
		currentCombatConfig.teamHero[heroPosition].perks = inputHeroPerks.text;
	}

	private void PersistCombatConfigs()
	{
		SaveManager.SaveIntoPrefsString("_CombatConfig", JsonConvert.SerializeObject(combatPrefsData, Formatting.None));
	}

	private List<string> GetDeck(string deck)
	{
		List<string> list = new List<string>();
		if (string.IsNullOrEmpty(deck))
		{
			return list;
		}
		string[] array = deck.Split(',');
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].Contains(":"))
			{
				array[i] += ":";
			}
			string[] array2 = array[i].Split(':');
			string item = array2[0].Trim();
			int.TryParse(array2[1].Trim(), out var result);
			for (int j = 0; j < result; j++)
			{
				list.Add(item);
			}
		}
		return list;
	}

	private static string RemoveWhitespace(string str, bool toLower = false)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}
		string text = Regex.Replace(str, "[\\s']+", "");
		if (toLower)
		{
			return text.ToLower();
		}
		return text;
	}

	private List<string> ImportPerkTree(string code)
	{
		List<string> list = Functions.ParseCompressedCodeList(code);
		if (list == null)
		{
			Debug.LogError("Invalid Perk Tree Code");
			return new List<string>();
		}
		return list;
	}

	public void OnExportButtonClickEvent()
	{
		inputImportExport.text = "";
		inputImportExport.text = JsonConvert.SerializeObject(currentCombatConfig);
		inputImportExport.readOnly = true;
		importExportPanel.SetActive(value: true);
	}

	public void OnImportButtonClickEvent()
	{
		inputImportExport.text = "";
		inputImportExport.readOnly = false;
		importExportPanel.SetActive(value: true);
	}

	public void OnImportInputSubmitted()
	{
		importExportPanel.SetActive(value: false);
		if (!string.IsNullOrEmpty(inputImportExport.text))
		{
			for (int i = 1; i <= 4; i++)
			{
				GameObject.Find("NPC" + i + "Input").GetComponent<TMP_InputField>().text = string.Empty;
			}
			LoadConfigs(JsonConvert.DeserializeObject<CombatConfig>(inputImportExport.text));
		}
	}
}
