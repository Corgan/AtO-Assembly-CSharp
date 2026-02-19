using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SandboxManager : MonoBehaviour
{
	public SpriteRenderer background;

	public Transform sandboxWindow;

	public Transform buttonReset;

	public Transform buttonEnable;

	public Transform buttonDisable;

	public Transform buttonMadness;

	public Transform buttonExit;

	public Transform enabledBorder;

	public BotonGeneric boxTotalHeroes3;

	public BotonGeneric boxTotalHeroes2;

	public BotonGeneric boxTotalHeroes1;

	public Transform sandboxOptions;

	private Dictionary<string, BotonGeneric[]> boxs;

	private Dictionary<string, int> sandBoxValues;

	private Color bgOn = new Color(0.22f, 0.45f, 0.49f, 1f);

	private Color bgOff = new Color(0.5f, 0.48f, 0.48f, 1f);

	private Dictionary<string, int> currentValue;

	private Dictionary<string, List<int>> keyValue;

	private Dictionary<string, int> defaultValue;

	private Dictionary<string, TMP_Text> comboValues;

	private List<string> showPositiveSign;

	private List<string> valueIsPercent;

	private bool isEnabled;

	private List<Transform> allButons;

	public int controllerHorizontalIndex = -1;

	private Vector2 warpPosition = Vector2.zero;

	private List<Transform> _controllerList = new List<Transform>();

	private List<Transform> _controllerVerticalList = new List<Transform>();

	public Transform divinationCostDisable;

	public Transform maximumRerollsDisable;

	public Transform petsCostDisable;

	public static SandboxManager Instance { get; private set; }

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
	}

	private void Start()
	{
		string[] namesToFind = new string[2] { "SandboxBox", "SandboxBoxCheck" };
		allButons = Functions.FindChildrenByName(sandboxOptions, namesToFind);
	}

	public void InitSandbox()
	{
		InitCombos();
		InitOptions();
		CloseSandbox();
	}

	public bool IsEnabled()
	{
		return isEnabled;
	}

	public void EnableSandbox()
	{
		isEnabled = true;
		ShowEnableButtons();
		if ((bool)HeroSelectionManager.Instance)
		{
			if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
			{
				HeroSelectionManager.Instance.ShareSandboxEnabledState(state: true);
			}
			HeroSelectionManager.Instance.RefreshCharBoxesBySandboxHeroes();
		}
	}

	public void DisableSandbox()
	{
		isEnabled = false;
		ShowEnableButtons();
		if ((bool)HeroSelectionManager.Instance)
		{
			if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
			{
				HeroSelectionManager.Instance.ShareSandboxEnabledState(state: false);
			}
			HeroSelectionManager.Instance.RefreshCharBoxesBySandboxHeroes();
		}
	}

	private void ShowEnableButtons()
	{
		if (isEnabled)
		{
			enabledBorder.gameObject.SetActive(value: true);
			background.color = bgOn;
		}
		else
		{
			enabledBorder.gameObject.SetActive(value: false);
			background.color = bgOff;
		}
		if (CanClickOptions())
		{
			if (isEnabled)
			{
				buttonEnable.gameObject.SetActive(value: false);
				buttonDisable.gameObject.SetActive(value: true);
			}
			else
			{
				buttonEnable.gameObject.SetActive(value: true);
				buttonDisable.gameObject.SetActive(value: false);
			}
		}
		else
		{
			buttonEnable.gameObject.SetActive(value: false);
			buttonDisable.gameObject.SetActive(value: false);
		}
		if ((bool)HeroSelectionManager.Instance)
		{
			HeroSelectionManager.Instance.RefreshSandboxButton();
		}
	}

	private void InitCombos()
	{
		keyValue = new Dictionary<string, List<int>>();
		currentValue = new Dictionary<string, int>();
		defaultValue = new Dictionary<string, int>();
		comboValues = new Dictionary<string, TMP_Text>();
		valueIsPercent = new List<string>();
		showPositiveSign = new List<string>();
		SetCombo("sbEnergy", 0, -10, 10, 1, showPositive: true, isPercent: false);
		SetCombo("sbSpeed", 0, -20, 20, 1, showPositive: true, isPercent: false);
		SetCombo("sbGold", 0, -10000, 50000, 2500, showPositive: true, isPercent: false);
		SetCombo("sbShards", 0, -10000, 50000, 2500, showPositive: true, isPercent: false);
		SetCombo("sbCraftCost", 0, -100, 300, 25, showPositive: true, isPercent: true);
		SetCombo("sbUpgradeCost", 0, -100, 300, 25, showPositive: true, isPercent: true);
		SetCombo("sbTransformCost", 0, -100, 300, 25, showPositive: true, isPercent: true);
		SetCombo("sbRemoveCost", 0, -100, 300, 25, showPositive: true, isPercent: true);
		SetCombo("sbEquipmentCost", 0, -100, 300, 25, showPositive: true, isPercent: true);
		SetCombo("sbPetsCost", 0, -100, 300, 25, showPositive: true, isPercent: true);
		SetCombo("sbDivinationCost", 0, -100, 300, 25, showPositive: true, isPercent: true);
		SetCombo("sbMonstersHP", 0, -75, 300, 25, showPositive: true, isPercent: true);
		SetCombo("sbMonstersDamage", 0, -75, 300, 25, showPositive: true, isPercent: true);
	}

	private void InitOptions()
	{
		Transform[] array = FindChildren(sandboxWindow, "SandboxBoxCheck");
		boxs = new Dictionary<string, BotonGeneric[]>();
		sandBoxValues = new Dictionary<string, int>();
		int num = 5;
		Transform[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			BotonGeneric component = array2[i].GetComponent<BotonGeneric>();
			if (!(component != null))
			{
				continue;
			}
			if (!sandBoxValues.ContainsKey(component.auxString))
			{
				sandBoxValues.Add(component.auxString, 0);
			}
			if (!boxs.ContainsKey(component.auxString))
			{
				boxs.Add(component.auxString, new BotonGeneric[num]);
				for (int j = 0; j < num; j++)
				{
					boxs[component.auxString][j] = null;
				}
			}
			boxs[component.auxString][component.auxInt] = component;
		}
	}

	private void SetCombo(string key, int def, int min, int max, int step, bool showPositive, bool isPercent)
	{
		Transform transform = GameObject.Find(key)?.transform;
		if (!(transform != null))
		{
			return;
		}
		if (!keyValue.ContainsKey(key))
		{
			keyValue.Add(key, new List<int>());
		}
		int num = 0;
		for (int i = min; i <= max; i += step)
		{
			keyValue[key].Add(i);
			if (i == def)
			{
				defaultValue.Add(key, num);
				currentValue.Add(key, num);
			}
			num++;
		}
		comboValues.Add(key, transform.GetComponent<TMP_Text>());
		if (isPercent)
		{
			valueIsPercent.Add(key);
		}
		if (showPositive)
		{
			showPositiveSign.Add(key);
		}
		SetComboValue(key);
	}

	private void SetComboValue(string key)
	{
		if (!currentValue.ContainsKey(key) || !keyValue.ContainsKey(key) || !comboValues.ContainsKey(key))
		{
			return;
		}
		int num = keyValue[key][currentValue[key]];
		StringBuilder stringBuilder = new StringBuilder();
		if (showPositiveSign.Contains(key) && num > 0)
		{
			stringBuilder.Append("+");
		}
		stringBuilder.Append(num);
		if (valueIsPercent.Contains(key))
		{
			if (Functions.SpaceBeforePercentSign())
			{
				stringBuilder.Append(" ");
			}
			stringBuilder.Append("%");
		}
		comboValues[key].text = stringBuilder.ToString();
		stringBuilder = null;
	}

	public void SetComboValueByVal(string key, int value)
	{
		if (!keyValue.ContainsKey(key))
		{
			return;
		}
		for (int i = 0; i < keyValue[key].Count; i++)
		{
			if (keyValue[key][i] == value)
			{
				currentValue[key] = i;
				break;
			}
		}
		SetComboValue(key);
	}

	private void SetBoxValue(string key)
	{
		if (boxs == null || !boxs.ContainsKey(key))
		{
			return;
		}
		for (int i = 0; i < boxs[key].Length; i++)
		{
			if (boxs[key][i] != null)
			{
				if (boxs[key][i].auxInt == sandBoxValues[key])
				{
					boxs[key][i].SetText("X");
				}
				else
				{
					boxs[key][i].SetText("");
				}
			}
		}
	}

	public void SetBoxValueByVal(string key, int value)
	{
		if (sandBoxValues.ContainsKey(key))
		{
			sandBoxValues[key] = value;
			SetBoxValue(key);
		}
	}

	public void BoxClick(string auxString, int auxInt)
	{
		if (!CanClickOptions())
		{
			return;
		}
		if (currentValue.ContainsKey(auxString))
		{
			if (auxInt < 0)
			{
				if (currentValue[auxString] > 0)
				{
					currentValue[auxString]--;
				}
			}
			else if (auxInt > 0)
			{
				if (currentValue[auxString] < keyValue[auxString].Count - 1)
				{
					currentValue[auxString]++;
				}
			}
			else
			{
				currentValue[auxString] = defaultValue[auxString];
			}
			SetComboValue(auxString);
			if ((bool)HeroSelectionManager.Instance && GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
			{
				HeroSelectionManager.Instance.ShareSandboxCombo(auxString, keyValue[auxString][currentValue[auxString]]);
			}
			return;
		}
		if (sandBoxValues[auxString] == auxInt)
		{
			sandBoxValues[auxString] = 0;
		}
		else
		{
			sandBoxValues[auxString] = auxInt;
		}
		SetBoxValue(auxString);
		if ((bool)HeroSelectionManager.Instance)
		{
			if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
			{
				HeroSelectionManager.Instance.ShareSandboxBox(auxString, sandBoxValues[auxString]);
			}
			HeroSelectionManager.Instance.RefreshCharBoxesBySandboxHeroes();
		}
	}

	public int GetSandboxBoxValue(string auxString)
	{
		if (sandBoxValues.ContainsKey(auxString))
		{
			return sandBoxValues[auxString];
		}
		return 0;
	}

	public void SbReset()
	{
		foreach (KeyValuePair<string, List<int>> item in keyValue)
		{
			currentValue[item.Key] = defaultValue[item.Key];
			SetComboValue(item.Key);
		}
		foreach (string item2 in new List<string>(sandBoxValues.Keys))
		{
			sandBoxValues[item2] = 0;
			SetBoxValue(item2);
		}
		if ((bool)HeroSelectionManager.Instance)
		{
			if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
			{
				HeroSelectionManager.Instance.ShareResetSandbox();
			}
			HeroSelectionManager.Instance.RefreshCharBoxesBySandboxHeroes();
		}
	}

	public void SetObeliskSandbox(bool state)
	{
		if (state)
		{
			divinationCostDisable.gameObject.SetActive(value: true);
			maximumRerollsDisable.gameObject.SetActive(value: true);
			petsCostDisable.gameObject.SetActive(value: true);
		}
		else
		{
			divinationCostDisable.gameObject.SetActive(value: false);
			maximumRerollsDisable.gameObject.SetActive(value: false);
			petsCostDisable.gameObject.SetActive(value: false);
		}
	}

	public void SetObeliskSandboxMods()
	{
		foreach (KeyValuePair<string, List<int>> item in keyValue)
		{
			if (item.Key == "sbDivinationCost" || item.Key == "sbPetsCost")
			{
				currentValue[item.Key] = defaultValue[item.Key];
				SetComboValue(item.Key);
			}
		}
		foreach (string item2 in new List<string>(sandBoxValues.Keys))
		{
			if (item2 == "sbUnlimitedRerolls")
			{
				sandBoxValues[item2] = 0;
				SetBoxValue(item2);
			}
		}
	}

	public void LoadValuesFromAtOManager()
	{
		if (!AtOManager.Instance.IsCombatTool)
		{
			SetComboValueByVal("sbEnergy", AtOManager.Instance.Sandbox_startingEnergy);
			SetComboValueByVal("sbSpeed", AtOManager.Instance.Sandbox_startingSpeed);
			SetComboValueByVal("sbGold", AtOManager.Instance.Sandbox_additionalGold);
			SetComboValueByVal("sbShards", AtOManager.Instance.Sandbox_additionalShards);
			SetComboValueByVal("sbCraftCost", AtOManager.Instance.Sandbox_cardCraftPrice);
			SetComboValueByVal("sbUpgradeCost", AtOManager.Instance.Sandbox_cardUpgradePrice);
			SetComboValueByVal("sbTransformCost", AtOManager.Instance.Sandbox_cardTransformPrice);
			SetComboValueByVal("sbRemoveCost", AtOManager.Instance.Sandbox_cardRemovePrice);
			SetComboValueByVal("sbEquipmentCost", AtOManager.Instance.Sandbox_itemsPrice);
			SetComboValueByVal("sbPetsCost", AtOManager.Instance.Sandbox_petsPrice);
			SetComboValueByVal("sbDivinationCost", AtOManager.Instance.Sandbox_divinationsPrice);
			SetBoxValueByVal("sbCraftUnlocked", Convert.ToInt32(AtOManager.Instance.Sandbox_craftUnlocked));
			SetBoxValueByVal("sbCardCraftRarity", Convert.ToInt32(AtOManager.Instance.Sandbox_allRarities));
			SetBoxValueByVal("sbCraftAvailable", Convert.ToInt32(AtOManager.Instance.Sandbox_unlimitedAvailableCards));
			SetBoxValueByVal("sbArmoryRerolls", Convert.ToInt32(AtOManager.Instance.Sandbox_freeRerolls));
			SetBoxValueByVal("sbUnlimitedRerolls", Convert.ToInt32(AtOManager.Instance.Sandbox_unlimitedRerolls));
			SetBoxValueByVal("sbMinimumDeckSize", Convert.ToInt32(AtOManager.Instance.Sandbox_noMinimumDecksize));
			SetBoxValueByVal("sbEventRolls", Convert.ToInt32(AtOManager.Instance.Sandbox_alwaysPassEventRoll));
			SetBoxValueByVal("sbTotalHeroes", Convert.ToInt32(AtOManager.Instance.Sandbox_totalHeroes));
			SetBoxValueByVal("sbLessMonsters", Convert.ToInt32(AtOManager.Instance.Sandbox_lessNPCs));
			SetComboValueByVal("sbMonstersHP", AtOManager.Instance.Sandbox_additionalMonsterHP);
			SetComboValueByVal("sbMonstersDamage", AtOManager.Instance.Sandbox_additionalMonsterDamage);
			SetBoxValueByVal("sbDoubleChampions", Convert.ToInt32(AtOManager.Instance.Sandbox_doubleChampions));
		}
	}

	public void SetCombatToolCombo()
	{
		SetComboValueByVal("sbCraftCost", -100);
		SetComboValueByVal("sbUpgradeCost", -100);
		SetComboValueByVal("sbTransformCost", -100);
		SetComboValueByVal("sbRemoveCost", -100);
		SetComboValueByVal("sbEquipmentCost", -100);
		SetComboValueByVal("sbPetsCost", -100);
		SetBoxValueByVal("sbCraftUnlocked", 1);
		SetBoxValueByVal("sbMinimumDeckSize", 1);
		SetComboValueByVal("sbMonstersHP", 0);
		SetComboValueByVal("sbMonstersDamage", 0);
		if (keyValue.ContainsKey("sbCraftCost"))
		{
			AtOManager.Instance.Sandbox_cardCraftPrice = keyValue["sbCraftCost"][currentValue["sbCraftCost"]];
		}
		if (keyValue.ContainsKey("sbUpgradeCost"))
		{
			AtOManager.Instance.Sandbox_cardUpgradePrice = keyValue["sbUpgradeCost"][currentValue["sbUpgradeCost"]];
		}
		if (keyValue.ContainsKey("sbTransformCost"))
		{
			AtOManager.Instance.Sandbox_cardUpgradePrice = keyValue["sbTransformCost"][currentValue["sbTransformCost"]];
		}
		if (keyValue.ContainsKey("sbRemoveCost"))
		{
			AtOManager.Instance.Sandbox_cardRemovePrice = keyValue["sbRemoveCost"][currentValue["sbRemoveCost"]];
		}
		if (keyValue.ContainsKey("sbEquipmentCost"))
		{
			AtOManager.Instance.Sandbox_itemsPrice = keyValue["sbEquipmentCost"][currentValue["sbEquipmentCost"]];
		}
		if (keyValue.ContainsKey("sbPetsCost"))
		{
			AtOManager.Instance.Sandbox_petsPrice = keyValue["sbPetsCost"][currentValue["sbPetsCost"]];
		}
		if (sandBoxValues.ContainsKey("sbCraftUnlocked"))
		{
			AtOManager.Instance.Sandbox_craftUnlocked = Convert.ToBoolean(sandBoxValues["sbCraftUnlocked"]);
		}
		if (sandBoxValues.ContainsKey("sbMinimumDeckSize"))
		{
			AtOManager.Instance.Sandbox_noMinimumDecksize = Convert.ToBoolean(sandBoxValues["sbMinimumDeckSize"]);
		}
		if (keyValue.ContainsKey("sbMonstersHP"))
		{
			AtOManager.Instance.Sandbox_additionalMonsterHP = keyValue["sbMonstersHP"][currentValue["sbMonstersHP"]];
		}
		if (keyValue.ContainsKey("sbMonstersDamage"))
		{
			AtOManager.Instance.Sandbox_additionalMonsterDamage = keyValue["sbMonstersDamage"][currentValue["sbMonstersDamage"]];
		}
		AtOManager.Instance.GetSandboxMods();
	}

	public void SaveValuesToAtOManager()
	{
		if (keyValue.ContainsKey("sbEnergy"))
		{
			AtOManager.Instance.Sandbox_startingEnergy = keyValue["sbEnergy"][currentValue["sbEnergy"]];
		}
		if (keyValue.ContainsKey("sbSpeed"))
		{
			AtOManager.Instance.Sandbox_startingSpeed = keyValue["sbSpeed"][currentValue["sbSpeed"]];
		}
		if (keyValue.ContainsKey("sbGold"))
		{
			AtOManager.Instance.Sandbox_additionalGold = keyValue["sbGold"][currentValue["sbGold"]];
		}
		if (keyValue.ContainsKey("sbShards"))
		{
			AtOManager.Instance.Sandbox_additionalShards = keyValue["sbShards"][currentValue["sbShards"]];
		}
		if (keyValue.ContainsKey("sbCraftCost"))
		{
			AtOManager.Instance.Sandbox_cardCraftPrice = keyValue["sbCraftCost"][currentValue["sbCraftCost"]];
		}
		if (keyValue.ContainsKey("sbUpgradeCost"))
		{
			AtOManager.Instance.Sandbox_cardUpgradePrice = keyValue["sbUpgradeCost"][currentValue["sbUpgradeCost"]];
		}
		if (keyValue.ContainsKey("sbTransformCost"))
		{
			AtOManager.Instance.Sandbox_cardTransformPrice = keyValue["sbTransformCost"][currentValue["sbTransformCost"]];
		}
		if (keyValue.ContainsKey("sbRemoveCost"))
		{
			AtOManager.Instance.Sandbox_cardRemovePrice = keyValue["sbRemoveCost"][currentValue["sbRemoveCost"]];
		}
		if (keyValue.ContainsKey("sbEquipmentCost"))
		{
			AtOManager.Instance.Sandbox_itemsPrice = keyValue["sbEquipmentCost"][currentValue["sbEquipmentCost"]];
		}
		if (keyValue.ContainsKey("sbPetsCost"))
		{
			AtOManager.Instance.Sandbox_petsPrice = keyValue["sbPetsCost"][currentValue["sbPetsCost"]];
		}
		if (keyValue.ContainsKey("sbDivinationCost"))
		{
			AtOManager.Instance.Sandbox_divinationsPrice = keyValue["sbDivinationCost"][currentValue["sbDivinationCost"]];
		}
		if (sandBoxValues.ContainsKey("sbCraftUnlocked"))
		{
			AtOManager.Instance.Sandbox_craftUnlocked = Convert.ToBoolean(sandBoxValues["sbCraftUnlocked"]);
		}
		if (sandBoxValues.ContainsKey("sbCardCraftRarity"))
		{
			AtOManager.Instance.Sandbox_allRarities = Convert.ToBoolean(sandBoxValues["sbCardCraftRarity"]);
		}
		if (sandBoxValues.ContainsKey("sbCraftAvailable"))
		{
			AtOManager.Instance.Sandbox_unlimitedAvailableCards = Convert.ToBoolean(sandBoxValues["sbCraftAvailable"]);
		}
		if (sandBoxValues.ContainsKey("sbArmoryRerolls"))
		{
			AtOManager.Instance.Sandbox_freeRerolls = Convert.ToBoolean(sandBoxValues["sbArmoryRerolls"]);
		}
		if (sandBoxValues.ContainsKey("sbUnlimitedRerolls"))
		{
			AtOManager.Instance.Sandbox_unlimitedRerolls = Convert.ToBoolean(sandBoxValues["sbUnlimitedRerolls"]);
		}
		if (sandBoxValues.ContainsKey("sbMinimumDeckSize"))
		{
			AtOManager.Instance.Sandbox_noMinimumDecksize = Convert.ToBoolean(sandBoxValues["sbMinimumDeckSize"]);
		}
		if (sandBoxValues.ContainsKey("sbEventRolls"))
		{
			AtOManager.Instance.Sandbox_alwaysPassEventRoll = Convert.ToBoolean(sandBoxValues["sbEventRolls"]);
		}
		if (sandBoxValues.ContainsKey("sbTotalHeroes"))
		{
			AtOManager.Instance.Sandbox_totalHeroes = sandBoxValues["sbTotalHeroes"];
		}
		if (sandBoxValues.ContainsKey("sbLessMonsters"))
		{
			AtOManager.Instance.Sandbox_lessNPCs = sandBoxValues["sbLessMonsters"];
		}
		if (keyValue.ContainsKey("sbMonstersHP"))
		{
			AtOManager.Instance.Sandbox_additionalMonsterHP = keyValue["sbMonstersHP"][currentValue["sbMonstersHP"]];
		}
		if (keyValue.ContainsKey("sbMonstersDamage"))
		{
			AtOManager.Instance.Sandbox_additionalMonsterDamage = keyValue["sbMonstersDamage"][currentValue["sbMonstersDamage"]];
		}
		if (sandBoxValues.ContainsKey("sbDoubleChampions"))
		{
			AtOManager.Instance.Sandbox_doubleChampions = Convert.ToBoolean(sandBoxValues["sbDoubleChampions"]);
		}
		if (AtOManager.Instance.IsCombatTool)
		{
			SetCombatToolCombo();
		}
		else
		{
			AtOManager.Instance.GetSandboxMods();
		}
	}

	public bool IsActive()
	{
		return sandboxWindow.gameObject.activeSelf;
	}

	private IEnumerator CloseSandboxCo()
	{
		yield return Globals.Instance.WaitForSeconds(1f);
	}

	public void CloseSandbox()
	{
		if ((bool)HeroSelectionManager.Instance && (!GameManager.Instance.IsMultiplayer() || (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())) && GameManager.Instance.GameStatus != Enums.GameStatus.LoadGame)
		{
			SaveValuesToAtOManager();
			if (!AtOManager.Instance.IsCombatTool)
			{
				if (!GameManager.Instance.IsObeliskChallenge())
				{
					SaveManager.SaveIntoPrefsString("sandboxSettings", AtOManager.Instance.GetSandboxMods());
				}
				else
				{
					SaveManager.SaveIntoPrefsString("sandboxSettingsObelisk", AtOManager.Instance.GetSandboxMods());
				}
				SaveManager.SavePrefs();
			}
		}
		if (IsActive())
		{
			sandboxWindow.gameObject.SetActive(value: false);
		}
		PopupManager.Instance.ClosePopup();
	}

	public void ShowSandbox()
	{
		if (IsActive())
		{
			CloseSandbox();
			return;
		}
		if (!HeroSelectionManager.Instance)
		{
			LoadValuesFromAtOManager();
		}
		if (!CanClickOptions())
		{
			buttonReset.gameObject.SetActive(value: false);
		}
		else
		{
			buttonReset.gameObject.SetActive(value: true);
		}
		ShowEnableButtons();
		sandboxWindow.gameObject.SetActive(value: true);
	}

	private bool CanClickOptions()
	{
		if ((!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster()) && PlayerManager.Instance.NgLevel == 0)
		{
			return false;
		}
		if (!HeroSelectionManager.Instance || GameManager.Instance.IsLoadingGame() || (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster()))
		{
			return false;
		}
		return true;
	}

	public static Transform[] FindChildren(Transform parent, string name)
	{
		return Array.FindAll(parent.GetComponentsInChildren<Transform>(includeInactive: true), (Transform child) => child.name == name);
	}

	public void AdjustTotalHeroesBoxToCoop()
	{
		if (!GameManager.Instance.IsMultiplayer())
		{
			boxTotalHeroes3.Enable();
			boxTotalHeroes2.Enable();
			boxTotalHeroes1.Enable();
			return;
		}
		switch (NetworkManager.Instance.GetNumPlayers())
		{
		case 2:
			boxTotalHeroes1.Disable();
			if (GetSandboxBoxValue("sbTotalHeroes") == 1)
			{
				SetBoxValueByVal("sbTotalHeroes", 0);
			}
			break;
		case 3:
			boxTotalHeroes1.Disable();
			boxTotalHeroes2.Disable();
			if (GetSandboxBoxValue("sbTotalHeroes") == 1 || GetSandboxBoxValue("sbTotalHeroes") == 2)
			{
				SetBoxValueByVal("sbTotalHeroes", 0);
			}
			break;
		case 4:
			boxTotalHeroes1.Disable();
			boxTotalHeroes2.Disable();
			boxTotalHeroes3.Disable();
			if (GetSandboxBoxValue("sbTotalHeroes") == 1 || GetSandboxBoxValue("sbTotalHeroes") == 2 || GetSandboxBoxValue("sbTotalHeroes") == 3)
			{
				SetBoxValueByVal("sbTotalHeroes", 0);
			}
			break;
		}
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false, int absolutePosition = -1)
	{
		_controllerList.Clear();
		for (int i = 0; i < allButons.Count; i++)
		{
			if (Functions.TransformIsVisible(allButons[i]))
			{
				_controllerList.Add(allButons[i]);
			}
		}
		_controllerList.Add(buttonDisable);
		_controllerList.Add(buttonReset);
		if (Functions.TransformIsVisible(buttonMadness))
		{
			_controllerList.Add(buttonMadness);
		}
		_controllerList.Add(buttonExit);
		controllerHorizontalIndex = Functions.GetListClosestIndexToMousePosition(_controllerList);
		controllerHorizontalIndex = Functions.GetClosestIndexBasedOnDirection(_controllerList, controllerHorizontalIndex, goingUp, goingRight, goingDown, goingLeft);
		if (_controllerList[controllerHorizontalIndex] != null)
		{
			warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerList[controllerHorizontalIndex].position);
			Mouse.current.WarpCursorPosition(warpPosition);
		}
	}
}
