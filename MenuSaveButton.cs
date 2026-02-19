using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuSaveButton : MonoBehaviour
{
	public TMP_Text slotText;

	public TMP_Text usethisText;

	public TMP_Text typeText;

	public TMP_Text descriptionText;

	public TMP_Text playersText;

	public TMP_Text madnessText;

	public Image[] imgHero;

	public TMP_Text[] playerText;

	public Transform incompatibleT;

	public TMP_Text versionText;

	public Transform ngPlus;

	public Transform deleteButton;

	public Transform portraitIcons;

	private int slotNum = -1;

	private bool active;

	private GameData gameData;

	private Coroutine coExit;

	private Image imageBg;

	private Color colorHover;

	private void Awake()
	{
		ShowDeleteButton(_state: false);
		imageBg = GetComponent<Image>();
		colorHover = new Color(1f, 1f, 1f, 0.5f);
	}

	public void SetActive(bool _state)
	{
		active = _state;
		ShowNGPlus(_state: false);
		ShowPortraitIcons(_state);
		slotText.gameObject.SetActive(!_state);
		usethisText.gameObject.SetActive(!_state);
		descriptionText.gameObject.SetActive(_state);
		deleteButton.gameObject.SetActive(value: false);
		playersText.gameObject.SetActive(value: false);
		incompatibleT.gameObject.SetActive(value: false);
		versionText.text = "";
		RectTransform component = GetComponent<RectTransform>();
		if (!_state)
		{
			slotText.text = Texts.Instance.GetText("mainMenuCreateNewGame");
			component.sizeDelta = new Vector2(520f, 220f);
		}
		else
		{
			slotText.text = Texts.Instance.GetText("menuLoadGame");
			component.sizeDelta = new Vector2(520f, 310f);
		}
	}

	public void SetGameData(GameData _gameData)
	{
		gameData = _gameData;
		if (gameData.Version == null)
		{
			gameData.Version = "0.6.82";
		}
		if (Functions.CheckIfSavegameIsCompatible(gameData) != "")
		{
			GetComponent<Button>().interactable = false;
			incompatibleT.gameObject.SetActive(value: true);
		}
		else
		{
			GetComponent<Button>().interactable = true;
			incompatibleT.gameObject.SetActive(value: false);
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("v.");
		stringBuilder.Append(gameData.Version);
		versionText.text = stringBuilder.ToString();
		stringBuilder.Clear();
		NodeData nodeData = Globals.Instance.GetNodeData(gameData.CurrentMapNode);
		if (_gameData.GameType == Enums.GameType.Adventure || _gameData.GameType == Enums.GameType.Singularity)
		{
			if (nodeData != null && nodeData.NodeZone != null)
			{
				stringBuilder.Append(nodeData.NodeName);
				stringBuilder.Append(" <voffset=3><color=#666>|</color></voffset> <color=#AAA>");
				if (Globals.Instance.ZoneDataSource.ContainsKey(nodeData.NodeZone.ZoneId.ToLower()))
				{
					stringBuilder.Append(Texts.Instance.GetText(Globals.Instance.ZoneDataSource[nodeData.NodeZone.ZoneId.ToLower()].ZoneName.Replace(" ", "")));
				}
				else
				{
					stringBuilder.Append(Texts.Instance.GetText(nodeData.NodeZone.ZoneId.ToLower()));
				}
				int num = _gameData.TownTier + 1;
				if (num > 4)
				{
					num = 4;
				}
				string value = string.Format(Texts.Instance.GetText("actNumber"), num);
				stringBuilder.Append(" <size=-2>(");
				stringBuilder.Append(value);
				stringBuilder.Append(")</size>");
				stringBuilder.Append("</color>");
			}
		}
		else if (_gameData.GameType == Enums.GameType.WeeklyChallenge)
		{
			stringBuilder.Append(AtOManager.Instance.GetWeeklyName(_gameData.Weekly));
			stringBuilder.Append(" <voffset=3><color=#666>|</color></voffset> <color=#AAA>");
			if (nodeData != null && nodeData.NodeZone != null)
			{
				if (nodeData.NodeZone.ObeliskLow)
				{
					stringBuilder.Append(Texts.Instance.GetText("lowerObelisk"));
				}
				else if (nodeData.NodeZone.ObeliskHigh)
				{
					stringBuilder.Append(Texts.Instance.GetText("upperObelisk"));
				}
				else
				{
					stringBuilder.Append(Texts.Instance.GetText("finalObelisk"));
				}
			}
			stringBuilder.Append("</color>");
		}
		else if (nodeData != null && nodeData.NodeZone != null)
		{
			if (nodeData.NodeZone.ObeliskLow)
			{
				stringBuilder.Append(Texts.Instance.GetText("lowerObelisk"));
			}
			else if (nodeData.NodeZone.ObeliskHigh)
			{
				stringBuilder.Append(Texts.Instance.GetText("upperObelisk"));
			}
			else
			{
				stringBuilder.Append(Texts.Instance.GetText("finalObelisk"));
			}
		}
		CultureInfo cultureInfoByTwoLetterCode = Functions.GetCultureInfoByTwoLetterCode(Globals.Instance.CurrentLang);
		string text = ((cultureInfoByTwoLetterCode == null) ? _gameData.GameDate : DateTime.Parse(_gameData.GameDate).ToString("g", cultureInfoByTwoLetterCode));
		string[] array = text.Split(' ');
		stringBuilder.Append("   <nobr><size=-2><color=#ffffff>");
		stringBuilder.Append(array[0]);
		stringBuilder.Append(" ");
		stringBuilder.Append(array[1]);
		if (array.Length > 2)
		{
			stringBuilder.Append(" ");
			stringBuilder.Append(array[2]);
		}
		stringBuilder.Append("</color></nobr>");
		descriptionText.text = stringBuilder.ToString();
		int num2 = 0;
		int lvl = _gameData.NgPlus;
		string madnessCorruptors = _gameData.MadnessCorruptors;
		num2 = MadnessManager.Instance.CalculateMadnessTotal(lvl, madnessCorruptors);
		if (num2 == 0)
		{
			num2 = _gameData.ObeliskMadness;
		}
		if (num2 > 0)
		{
			ShowNGPlus(_state: true);
			if (madnessCorruptors == null)
			{
				madnessCorruptors = "";
			}
			madnessText.text = "M" + num2;
		}
		if (gameData.GameMode == Enums.GameMode.Multiplayer)
		{
			stringBuilder.Clear();
			if (_gameData.Owner0 != null && _gameData.Owner0 != "")
			{
				int num3 = 0;
				string text2 = "";
				text2 = _gameData.Owner0;
				using (Dictionary<string, string>.Enumerator enumerator = _gameData.PlayerNickRealDict.GetEnumerator())
				{
					while (enumerator.MoveNext() && !(enumerator.Current.Value == text2))
					{
						num3++;
					}
				}
				stringBuilder.Append("<color=");
				stringBuilder.Append(NetworkManager.Instance.ColorFromPosition(num3));
				stringBuilder.Append(">");
				stringBuilder.Append(_gameData.Owner0);
				stringBuilder.Append("</color>");
				playerText[0].text = stringBuilder.ToString();
				text2 = _gameData.Owner1;
				num3 = 0;
				using (Dictionary<string, string>.Enumerator enumerator = _gameData.PlayerNickRealDict.GetEnumerator())
				{
					while (enumerator.MoveNext() && !(enumerator.Current.Value == text2))
					{
						num3++;
					}
				}
				stringBuilder.Clear();
				stringBuilder.Append("<color=");
				stringBuilder.Append(NetworkManager.Instance.ColorFromPosition(num3));
				stringBuilder.Append(">");
				stringBuilder.Append(_gameData.Owner1);
				stringBuilder.Append("</color>");
				playerText[1].text = stringBuilder.ToString();
				text2 = _gameData.Owner2;
				num3 = 0;
				using (Dictionary<string, string>.Enumerator enumerator = _gameData.PlayerNickRealDict.GetEnumerator())
				{
					while (enumerator.MoveNext() && !(enumerator.Current.Value == text2))
					{
						num3++;
					}
				}
				stringBuilder.Clear();
				stringBuilder.Append("<color=");
				stringBuilder.Append(NetworkManager.Instance.ColorFromPosition(num3));
				stringBuilder.Append(">");
				stringBuilder.Append(_gameData.Owner2);
				stringBuilder.Append("</color>");
				playerText[2].text = stringBuilder.ToString();
				text2 = _gameData.Owner3;
				num3 = 0;
				using (Dictionary<string, string>.Enumerator enumerator = _gameData.PlayerNickRealDict.GetEnumerator())
				{
					while (enumerator.MoveNext() && !(enumerator.Current.Value == text2))
					{
						num3++;
					}
				}
				stringBuilder.Clear();
				stringBuilder.Append("<color=");
				stringBuilder.Append(NetworkManager.Instance.ColorFromPosition(num3));
				stringBuilder.Append(">");
				stringBuilder.Append(_gameData.Owner3);
				stringBuilder.Append("</color>");
				playerText[3].text = stringBuilder.ToString();
				for (int i = 0; i < 4; i++)
				{
					playerText[i].gameObject.SetActive(value: true);
				}
			}
			else
			{
				List<string> list = new List<string>();
				if (_gameData.PlayerNickRealDict != null)
				{
					int num4 = 0;
					for (int j = 0; j < 4; j++)
					{
						playerText[j].gameObject.SetActive(value: false);
					}
					foreach (KeyValuePair<string, string> item in _gameData.PlayerNickRealDict)
					{
						if (!list.Contains(item.Value))
						{
							stringBuilder.Append("<color=");
							stringBuilder.Append(NetworkManager.Instance.ColorFromPosition(num4));
							stringBuilder.Append(">");
							stringBuilder.Append(item.Value);
							stringBuilder.Append("</color>, ");
							num4++;
							list.Add(item.Value);
						}
					}
				}
			}
		}
		else
		{
			for (int k = 0; k < 4; k++)
			{
				playerText[k].gameObject.SetActive(value: false);
			}
		}
		Hero[] array2 = JsonHelper.FromJson<Hero>(_gameData.TeamAtO);
		for (int l = 0; l < array2.Length; l++)
		{
			if (array2[l].SubclassName != null && Globals.Instance.GetSubClassData(array2[l].SubclassName) != null)
			{
				SkinData skinData = null;
				skinData = ((array2[l].SkinUsed != null && !(array2[l].SkinUsed == "")) ? Globals.Instance.GetSkinData(array2[l].SkinUsed) : Globals.Instance.GetSkinData(Globals.Instance.GetSkinBaseIdBySubclass(array2[l].SubclassName)));
				if (skinData != null)
				{
					imgHero[l].sprite = skinData.SpritePortrait;
				}
				else
				{
					imgHero[l].sprite = Globals.Instance.GetSubClassData(array2[l].SubclassName).SpriteSpeed;
				}
				imgHero[l].gameObject.SetActive(value: true);
			}
			else
			{
				imgHero[l].gameObject.SetActive(value: false);
			}
		}
	}

	public void SetSlot(int _num)
	{
		slotNum = _num;
		GetComponent<Button>().interactable = true;
		incompatibleT.gameObject.SetActive(value: false);
	}

	private void ShowDeleteButton(bool _state)
	{
		deleteButton.gameObject.SetActive(_state);
	}

	private void ShowNGPlus(bool _state)
	{
		ngPlus.gameObject.SetActive(_state);
	}

	private void ShowPortraitIcons(bool _state)
	{
		portraitIcons.gameObject.SetActive(_state);
	}

	public void SelectThis()
	{
		if (!AlertManager.Instance.IsActive())
		{
			AtOManager.Instance.SetSaveSlot(slotNum);
			GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
			if (!active)
			{
				SceneStatic.LoadByName((slotNum % 12 < 6) ? "HeroSelection" : "Lobby");
			}
			else
			{
				AtOManager.Instance.LoadGame(slotNum);
			}
		}
	}

	public void DeleteThis()
	{
		if (!AlertManager.Instance.IsActive())
		{
			AlertManager.buttonClickDelegate = DeleteAction;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Texts.Instance.GetText("wantToRemoveSave"));
			stringBuilder.Append(" <br><size=-4><color=#AAAAAA>");
			stringBuilder.Append(Texts.Instance.GetText("wantToRemoveSavePermanent"));
			stringBuilder.Append("</color></size>");
			AlertManager.Instance.AlertConfirmDouble(stringBuilder.ToString(), Texts.Instance.GetText("accept").ToUpper(), Texts.Instance.GetText("cancel").ToUpper());
		}
	}

	private void DeleteAction()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(DeleteAction));
		if (AlertManager.Instance.GetConfirmAnswer())
		{
			SaveManager.DeleteGame(slotNum, sendTelemetry: true);
			ShowNGPlus(_state: false);
			SetActive(_state: false);
			GetComponent<Button>().interactable = true;
		}
	}

	public void HoverOn()
	{
		if (!AlertManager.Instance.IsActive())
		{
			if (coExit != null)
			{
				StopCoroutine(coExit);
			}
			imageBg.color = colorHover;
			GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonHover);
			if (active)
			{
				ShowDeleteButton(_state: true);
			}
		}
	}

	public void HoverOff()
	{
		if (!AlertManager.Instance.IsActive())
		{
			ShowDeleteButton(_state: false);
			imageBg.color = new Color(1f, 1f, 1f, 0f);
		}
	}
}
