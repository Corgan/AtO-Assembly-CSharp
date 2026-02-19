using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paradox;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
	public GameObject canvas;

	public TMP_Text seedText;

	public AudioMixer audioMixer;

	public Transform graphicsTab;

	public Transform audioTab;

	public Transform gameplayTab;

	public Button[] optionButton;

	public Transform[] optionSelector;

	public Transform resetSavedT;

	public Toggle vsyncToggle;

	public Toggle fullscreenToggle;

	public Toggle resetSavedToggle;

	public Toggle resetTutorialToggle;

	public Toggle extendedDescriptionsToggle;

	[SerializeField]
	private Toggle telemetryToggle;

	[SerializeField]
	private GameObject telemetryContainerGO;

	public Toggle backgroundMuteToggle;

	public Toggle legacySoundsToggle;

	public Toggle legacySoundsSheepOwlToggle;

	public Slider masterVolumeSlider;

	public Slider effectsVolumeSlider;

	public Slider bsoVolumeSlider;

	public Slider ambienceVolumeSlider;

	public Toggle fastModeToggle;

	public Toggle autoEndToggle;

	public Toggle showEffectsToggle;

	public Toggle acbackgroundEffectsToggle;

	public Toggle restartCombatOptionToggle;

	public Toggle screenShakeOptionToggle;

	public Toggle keyboardShortcutsToggle;

	public Toggle followingTheLeaderToggle;

	private Resolution[] resolutions;

	private List<string> resolutionsList;

	public TMP_Dropdown resolutionDropdown;

	public TMP_Dropdown languageDropdown;

	private bool languageDropdownInitiated;

	public Transform langCommunity;

	private int actualTabIndex = -1;

	public static SettingsManager Instance { get; private set; }

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
		GetResolutions();
	}

	public void Resize()
	{
		canvas.GetComponent<CanvasScaler>().matchWidthOrHeight = 1f;
	}

	public bool IsActive()
	{
		return canvas.gameObject.activeSelf;
	}

	public void ShowSettings(bool _state)
	{
		canvas.gameObject.SetActive(_state);
		if (!_state)
		{
			SaveManager.SavePrefs();
			if ((bool)CardCraftManager.Instance)
			{
				CardCraftManager.Instance.ShowSearch(state: true);
			}
			if (TomeManager.Instance.IsActive())
			{
				TomeManager.Instance.ShowSearch(state: true);
			}
			return;
		}
		if ((bool)PopupManager.Instance)
		{
			PopupManager.Instance.ClosePopup();
		}
		if ((bool)CardCraftManager.Instance)
		{
			CardCraftManager.Instance.ShowSearch(state: false);
		}
		if (TomeManager.Instance.IsActive())
		{
			TomeManager.Instance.ShowSearch(state: false);
		}
		string text = "";
		if (!GameManager.Instance.IsWeeklyChallenge())
		{
			text = AtOManager.Instance.GetGameId();
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<color=#AAA><size=-10>");
		stringBuilder.Append("AtO #");
		stringBuilder.Append(GameManager.Instance.gameVersion);
		if (text != "")
		{
			stringBuilder.Append("<br>");
			stringBuilder.Append(Texts.Instance.GetText("gameSeed"));
			stringBuilder.Append("</size></color><br>");
			stringBuilder.Append(text);
		}
		seedText.text = stringBuilder.ToString();
		graphicsTab.gameObject.SetActive(value: true);
		audioTab.gameObject.SetActive(value: false);
		gameplayTab.gameObject.SetActive(value: false);
		if (SceneStatic.GetSceneName() == "MainMenu")
		{
			resetSavedT.gameObject.SetActive(value: true);
		}
		else
		{
			resetSavedT.gameObject.SetActive(value: false);
		}
		optionButton[0].Select();
	}

	public void SelectTab(int _tabIndex)
	{
		if (actualTabIndex != _tabIndex)
		{
			ShowTab(actualTabIndex, state: false);
			actualTabIndex = _tabIndex;
			ShowTab(_tabIndex, state: true);
		}
	}

	public void ShowTab(int _tabIndex, bool state)
	{
		switch (_tabIndex)
		{
		case -1:
			return;
		case 0:
			graphicsTab.gameObject.SetActive(state);
			break;
		case 1:
			audioTab.gameObject.SetActive(state);
			break;
		case 2:
			gameplayTab.gameObject.SetActive(state);
			break;
		}
		optionSelector[_tabIndex].gameObject.SetActive(state);
	}

	public static List<Resolution> GetResolutionsList()
	{
		Resolution[] array = Screen.resolutions;
		HashSet<Tuple<int, int>> hashSet = new HashSet<Tuple<int, int>>();
		Dictionary<Tuple<int, int>, int> dictionary = new Dictionary<Tuple<int, int>, int>();
		for (int i = 0; i < array.GetLength(0); i++)
		{
			Tuple<int, int> tuple = new Tuple<int, int>(array[i].width, array[i].height);
			hashSet.Add(tuple);
			if (!dictionary.ContainsKey(tuple))
			{
				dictionary.Add(tuple, array[i].refreshRate);
			}
			else
			{
				dictionary[tuple] = array[i].refreshRate;
			}
		}
		List<Resolution> list = new List<Resolution>(hashSet.Count);
		foreach (Tuple<int, int> item2 in hashSet)
		{
			Resolution item = new Resolution
			{
				width = item2.Item1,
				height = item2.Item2
			};
			if (dictionary.TryGetValue(item2, out var value))
			{
				item.refreshRate = value;
			}
			list.Add(item);
		}
		return list;
	}

	private void GetResolutions()
	{
		resolutions = (from resolution in Screen.resolutions
			select new Resolution
			{
				width = resolution.width,
				height = resolution.height
			} into resolution
			where resolution.width >= 1024
			select resolution).Distinct().ToArray();
		resolutionDropdown.ClearOptions();
		resolutionsList = new List<string>();
		StringBuilder stringBuilder = new StringBuilder();
		for (int num = 0; num < resolutions.Length; num++)
		{
			stringBuilder.Clear();
			stringBuilder.Append(resolutions[num].width);
			stringBuilder.Append(" x ");
			stringBuilder.Append(resolutions[num].height);
			resolutionsList.Add(stringBuilder.ToString());
		}
		resolutionDropdown.AddOptions(resolutionsList);
		SelectCurrentResolution();
	}

	private void SelectCurrentResolution()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Screen.width);
		stringBuilder.Append(" x ");
		stringBuilder.Append(Screen.height);
		for (int i = 0; i < resolutionsList.Count; i++)
		{
			if (resolutionsList[i] == stringBuilder.ToString())
			{
				resolutionDropdown.value = i;
				resolutionDropdown.RefreshShownValue();
				break;
			}
		}
		fullscreenToggle.isOn = Screen.fullScreen;
	}

	public void LoadPrefs()
	{
		if (SaveManager.PrefsHasKey("language"))
		{
			Globals.Instance.SetLang(SaveManager.LoadPrefsInt("language"));
			languageDropdown.value = SaveManager.LoadPrefsInt("language");
			languageDropdown.RefreshShownValue();
		}
		langCommunity.gameObject.SetActive(value: false);
		languageDropdownInitiated = true;
		if (SaveManager.PrefsHasKey("vsync"))
		{
			GameManager.Instance.ConfigVsync = SaveManager.LoadPrefsBool("vsync");
			GameManager.Instance.SetVsync();
		}
		vsyncToggle.isOn = GameManager.Instance.ConfigVsync;
		if (SaveManager.PrefsHasKey("masterVolume"))
		{
			float value = SaveManager.LoadPrefsFloat("masterVolume");
			audioMixer.SetFloat("masterVolume", value);
			masterVolumeSlider.value = value;
		}
		if (SaveManager.PrefsHasKey("effectsVolume"))
		{
			float value2 = SaveManager.LoadPrefsFloat("effectsVolume");
			audioMixer.SetFloat("effectsVolume", value2);
			effectsVolumeSlider.value = value2;
		}
		if (SaveManager.PrefsHasKey("bsoVolume"))
		{
			float value3 = SaveManager.LoadPrefsFloat("bsoVolume");
			audioMixer.SetFloat("bsoVolume", value3);
			bsoVolumeSlider.value = value3;
		}
		if (SaveManager.PrefsHasKey("ambienceVolume"))
		{
			float value4 = SaveManager.LoadPrefsFloat("ambienceVolume");
			audioMixer.SetFloat("ambienceVolume", value4);
			ambienceVolumeSlider.value = value4;
		}
		if (SaveManager.PrefsHasKey("backgroundMute"))
		{
			GameManager.Instance.ConfigBackgroundMute = SaveManager.LoadPrefsBool("backgroundMute");
		}
		backgroundMuteToggle.isOn = GameManager.Instance.ConfigBackgroundMute;
		if (SaveManager.PrefsHasKey("useLegacySounds"))
		{
			GameManager.Instance.ConfigUseLegacySounds = SaveManager.LoadPrefsBool("useLegacySounds");
		}
		legacySoundsToggle.isOn = GameManager.Instance.ConfigUseLegacySounds;
		if (SaveManager.PrefsHasKey("useLegacySoundsSheepOwl"))
		{
			GameManager.Instance.ConfigUseLegacySoundsSheepOwl = SaveManager.LoadPrefsBool("useLegacySoundsSheepOwl");
		}
		legacySoundsSheepOwlToggle.isOn = GameManager.Instance.ConfigUseLegacySoundsSheepOwl;
		bool flag = GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Fast;
		if (SaveManager.PrefsHasKey("fastMode"))
		{
			flag = SaveManager.LoadPrefsBool("fastMode");
		}
		fastModeToggle.isOn = flag;
		if (flag)
		{
			GameManager.Instance.configGameSpeed = Enums.ConfigSpeed.Fast;
		}
		else
		{
			GameManager.Instance.configGameSpeed = Enums.ConfigSpeed.Slow;
		}
		bool flag2 = GameManager.Instance.ConfigAutoEnd;
		if (SaveManager.PrefsHasKey("autoEnd"))
		{
			flag2 = SaveManager.LoadPrefsBool("autoEnd");
		}
		autoEndToggle.isOn = flag2;
		GameManager.Instance.ConfigAutoEnd = flag2;
		bool flag3 = GameManager.Instance.ConfigShowEffects;
		if (SaveManager.PrefsHasKey("showEffects"))
		{
			flag3 = SaveManager.LoadPrefsBool("showEffects");
		}
		showEffectsToggle.isOn = flag3;
		GameManager.Instance.ConfigShowEffects = flag3;
		bool flag4 = GameManager.Instance.ConfigACBackgrounds;
		if (SaveManager.PrefsHasKey("acBackgrounds"))
		{
			flag4 = SaveManager.LoadPrefsBool("acBackgrounds");
		}
		acbackgroundEffectsToggle.isOn = flag4;
		GameManager.Instance.ConfigACBackgrounds = flag4;
		bool flag5 = GameManager.Instance.ConfigRestartCombatOption;
		if (SaveManager.PrefsHasKey("restartCombatOptionNew"))
		{
			flag5 = SaveManager.LoadPrefsBool("restartCombatOptionNew");
		}
		restartCombatOptionToggle.isOn = flag5;
		GameManager.Instance.ConfigRestartCombatOption = flag5;
		bool flag6 = GameManager.Instance.ConfigScreenShakeOption;
		if (SaveManager.PrefsHasKey("screenShakeOption"))
		{
			flag6 = SaveManager.LoadPrefsBool("screenShakeOption");
		}
		screenShakeOptionToggle.isOn = flag6;
		GameManager.Instance.ConfigScreenShakeOption = flag6;
		bool flag7 = GameManager.Instance.ConfigKeyboardShortcuts;
		if (SaveManager.PrefsHasKey("keyboardShortcuts"))
		{
			flag7 = SaveManager.LoadPrefsBool("keyboardShortcuts");
		}
		keyboardShortcutsToggle.isOn = flag7;
		GameManager.Instance.ConfigKeyboardShortcuts = flag7;
		bool flag8 = false;
		if (SaveManager.PrefsHasKey("followLeader"))
		{
			flag8 = SaveManager.LoadPrefsBool("followLeader");
		}
		followingTheLeaderToggle.isOn = flag8;
		AtOManager.Instance.followingTheLeader = flag8;
		SetResolutionDefault();
		if (SaveManager.PrefsHasKey("crossPlayEnabled"))
		{
			GameManager.Instance.ConfigCrossPlayEnabled = SaveManager.LoadPrefsBool("crossPlayEnabled");
		}
	}

	public void SetLanguage(int _langIndex)
	{
		if (IsActive())
		{
			SaveManager.SaveIntoPrefsInt("language", _langIndex);
			if (languageDropdownInitiated)
			{
				AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("selectLanguageChanged"));
			}
		}
	}

	public void SetResolution(int _resolutionIndex)
	{
		if (!IsActive())
		{
			return;
		}
		Resolution resolution = resolutions[_resolutionIndex];
		if (resolution.width != Screen.width || resolution.height != Screen.height)
		{
			if (resolution.width < 1024)
			{
				resolution.width = 1024;
			}
			if (resolution.height < 768)
			{
				resolution.height = 768;
			}
			Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
			SaveManager.SaveIntoPrefsInt("widthResolution", resolution.width);
			SaveManager.SaveIntoPrefsInt("heightResolution", resolution.height);
			SaveManager.SaveIntoPrefsBool("fullScreen", Screen.fullScreen);
		}
	}

	public void SetResolutionDefault()
	{
		if (SaveManager.PrefsHasKey("widthResolution") && SaveManager.PrefsHasKey("heightResolution"))
		{
			int num = SaveManager.LoadPrefsInt("widthResolution");
			int num2 = SaveManager.LoadPrefsInt("heightResolution");
			bool fullscreen = true;
			if (num < 1024)
			{
				num = 1024;
			}
			if (num2 < 768)
			{
				num2 = 768;
			}
			if (SaveManager.PrefsHasKey("fullScreen"))
			{
				fullscreen = SaveManager.LoadPrefsBool("fullScreen");
			}
			Screen.SetResolution(num, num2, fullscreen);
		}
	}

	public void SetQuality(int _qualityIndex)
	{
		QualitySettings.SetQualityLevel(_qualityIndex);
	}

	public void SetFullscreen(bool _isFullscreen)
	{
		Screen.fullScreen = _isFullscreen;
		SaveManager.SaveIntoPrefsBool("fullScreen", _isFullscreen);
	}

	public void SetVsync(bool _isVsync)
	{
		SaveManager.SaveIntoPrefsBool("vsync", _isVsync);
		GameManager.Instance.ConfigVsync = _isVsync;
		GameManager.Instance.SetVsync();
	}

	public void ResetSavedData()
	{
		AlertManager.buttonClickDelegate = ResetSavedDataAction;
		AlertManager.Instance.AlertConfirmDouble(Texts.Instance.GetText("removeProgressQuestion"));
		resetSavedToggle.isOn = false;
	}

	public void OnTelemetryToggleChanged(bool isOn)
	{
		UpdateTelemetryConsent(isOn);
	}

	public void ShowTelemetryTooltip()
	{
		PopupManager.Instance.SetText(Texts.Instance.GetText("optionalTelemetryTooltip"), fast: true, "followdown");
	}

	public void HideTelemetryTooltip()
	{
		PopupManager.Instance.ClosePopup();
	}

	public async Task InitTelemetryToggle()
	{
		bool flag = await Telemetry.IsConsentAvailable();
		telemetryContainerGO.SetActive(flag);
		if (flag)
		{
			bool isOn = await Telemetry.GetConsentChoice();
			telemetryToggle.isOn = isOn;
		}
	}

	private async void UpdateTelemetryConsent(bool value)
	{
		await Telemetry.SetConsentChoice(value);
	}

	private void ResetSavedDataAction()
	{
		bool confirmAnswer = AlertManager.Instance.GetConfirmAnswer();
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(ResetSavedDataAction));
		if (confirmAnswer)
		{
			SaveManager.CleanSavePlayerData();
		}
	}

	public void ResetTutorial()
	{
		AlertManager.buttonClickDelegate = ResetTutorialAction;
		AlertManager.Instance.AlertConfirmDouble(Texts.Instance.GetText("resetTutorialQuestion"));
		resetTutorialToggle.isOn = false;
	}

	private void ResetTutorialAction()
	{
		bool confirmAnswer = AlertManager.Instance.GetConfirmAnswer();
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(ResetTutorialAction));
		if (confirmAnswer)
		{
			SaveManager.ResetTutorial();
		}
	}

	public void SetMasterVolume(float _volume)
	{
		audioMixer.SetFloat("masterVolume", Mathf.Log10(_volume) * 20f);
		SaveManager.SaveIntoPrefsFloat("masterVolume", _volume);
	}

	public void SetEffectsVolume(float _volume)
	{
		audioMixer.SetFloat("effectsVolume", Mathf.Log10(_volume) * 20f);
		SaveManager.SaveIntoPrefsFloat("effectsVolume", _volume);
	}

	public void SetBSOVolume(float _volume)
	{
		audioMixer.SetFloat("bsoVolume", Mathf.Log10(_volume) * 20f);
		SaveManager.SaveIntoPrefsFloat("bsoVolume", _volume);
	}

	public void SetAmbienceVolume(float _volume)
	{
		audioMixer.SetFloat("ambienceVolume", Mathf.Log10(_volume) * 20f);
		SaveManager.SaveIntoPrefsFloat("ambienceVolume", _volume);
	}

	public void SetBackgroundMute(bool _backgroundMute)
	{
		SaveManager.SaveIntoPrefsBool("backgroundMute", _backgroundMute);
		GameManager.Instance.ConfigBackgroundMute = _backgroundMute;
	}

	public void SetUseLegacySounds(bool _useLegacy)
	{
		SaveManager.SaveIntoPrefsBool("useLegacySounds", _useLegacy);
		GameManager.Instance.ConfigUseLegacySounds = _useLegacy;
	}

	public void SetUseLegacySoundsSheepOwl(bool _useLegacySheepOwl)
	{
		SaveManager.SaveIntoPrefsBool("useLegacySoundsSheepOwl", _useLegacySheepOwl);
		GameManager.Instance.ConfigUseLegacySoundsSheepOwl = _useLegacySheepOwl;
	}

	public void SetFastMode(bool _fastMode)
	{
		SaveManager.SaveIntoPrefsBool("fastMode", _fastMode);
		if (_fastMode)
		{
			GameManager.Instance.configGameSpeed = Enums.ConfigSpeed.Fast;
		}
		else
		{
			GameManager.Instance.configGameSpeed = Enums.ConfigSpeed.Slow;
		}
	}

	public void SetAutoEnd(bool _autoEnd)
	{
		SaveManager.SaveIntoPrefsBool("autoEnd", _autoEnd);
		GameManager.Instance.ConfigAutoEnd = _autoEnd;
	}

	public void SetShowEffects(bool _showEffects)
	{
		SaveManager.SaveIntoPrefsBool("showEffects", _showEffects);
		GameManager.Instance.ConfigShowEffects = _showEffects;
	}

	public void SetRestartCombat(bool _restartCombat)
	{
		SaveManager.SaveIntoPrefsBool("restartCombatOptionNew", _restartCombat);
		GameManager.Instance.ConfigRestartCombatOption = _restartCombat;
	}

	public void SetScreenShake(bool _screenShake)
	{
		SaveManager.SaveIntoPrefsBool("screenShakeOption", _screenShake);
		GameManager.Instance.ConfigScreenShakeOption = _screenShake;
	}

	public void SetKeyboardShortcuts(bool _keyShortcuts)
	{
		SaveManager.SaveIntoPrefsBool("keyboardShortcuts", _keyShortcuts);
		GameManager.Instance.ConfigKeyboardShortcuts = _keyShortcuts;
		if ((bool)MatchManager.Instance)
		{
			MatchManager.Instance.ShowCombatKeyboardByConfig();
		}
		keyboardShortcutsToggle.isOn = _keyShortcuts;
	}

	public void SetExtendedDescriptions(bool _extendedDescriptions)
	{
		SaveManager.SaveIntoPrefsBool("extendedDescriptionsNew", _extendedDescriptions);
		GameManager.Instance.ConfigExtendedDescriptions = _extendedDescriptions;
		PopupManager.Instance.RefreshKeyNotes();
	}

	public void SetFollowingTheLeader(bool _followingTheLeader)
	{
		SaveManager.SaveIntoPrefsBool("followLeader", _followingTheLeader);
		AtOManager.Instance.followingTheLeader = _followingTheLeader;
		if ((bool)HeroSelectionManager.Instance)
		{
			HeroSelectionManager.Instance.ShowFollowStatus();
		}
	}

	public void SetACBackgrounds(bool _acBackgrounds)
	{
		SaveManager.SaveIntoPrefsBool("acBackgrounds", _acBackgrounds);
		GameManager.Instance.ConfigACBackgrounds = _acBackgrounds;
		if (MatchManager.Instance != null)
		{
			MatchManager.Instance.RefreshStatusEffects();
		}
	}

	public void SetCrossPlayEnabled(bool _enabled)
	{
		SaveManager.SaveIntoPrefsBool("crossPlayEnabled", _enabled);
		GameManager.Instance.ConfigCrossPlayEnabled = _enabled;
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false)
	{
	}
}
