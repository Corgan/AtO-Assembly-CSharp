using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class OptionsManager : MonoBehaviour
{
	public Transform elements;

	public Transform iconExit;

	public Transform iconStats;

	public Transform iconResign;

	public Transform iconSettings;

	public Transform iconRetry;

	public Transform iconTome;

	public Transform iconCombatLog;

	public Transform madness;

	public Transform madnessParticles;

	public TMP_Text madnessText;

	public Transform version;

	public Transform score;

	public TMP_Text scoreText;

	public Transform position;

	private float distanceBetweenButton = 0.65f;

	private float positionRightButton = 0.95f;

	private float adjustmentForBigButtons = 1.2f;

	private List<GameObject> buttonOrder = new List<GameObject>();

	private int _indexController = -1;

	public static OptionsManager Instance { get; private set; }

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
		buttonOrder.Add(iconExit.gameObject);
		buttonOrder.Add(iconResign.gameObject);
		buttonOrder.Add(iconRetry.gameObject);
		buttonOrder.Add(iconSettings.gameObject);
		buttonOrder.Add(iconStats.gameObject);
		buttonOrder.Add(iconCombatLog.gameObject);
		buttonOrder.Add(iconTome.gameObject);
		buttonOrder.Add(score.gameObject);
		buttonOrder.Add(madness.gameObject);
	}

	public void Show()
	{
		if (!elements.gameObject.activeSelf)
		{
			elements.gameObject.SetActive(value: true);
		}
		iconRetry.gameObject.SetActive(value: false);
		iconCombatLog.gameObject.SetActive(value: false);
		iconResign.gameObject.SetActive(value: false);
		iconTome.gameObject.SetActive(value: true);
		iconExit.gameObject.SetActive(value: true);
		if ((bool)MatchManager.Instance || (bool)MapManager.Instance || (bool)TownManager.Instance || (bool)FinishRunManager.Instance || (bool)RewardsManager.Instance)
		{
			iconStats.gameObject.SetActive(value: true);
		}
		else
		{
			iconStats.gameObject.SetActive(value: false);
		}
		if ((bool)FinishRunManager.Instance)
		{
			iconSettings.gameObject.SetActive(value: false);
			iconExit.gameObject.SetActive(value: false);
			iconTome.gameObject.SetActive(value: false);
		}
		else
		{
			iconSettings.gameObject.SetActive(value: true);
		}
		if ((bool)MatchManager.Instance)
		{
			version.gameObject.SetActive(value: true);
			iconCombatLog.gameObject.SetActive(value: true);
			if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
			{
				iconResign.gameObject.SetActive(value: true);
				iconRetry.gameObject.SetActive(value: true);
			}
			else
			{
				iconExit.gameObject.SetActive(value: false);
			}
		}
		else
		{
			version.gameObject.SetActive(value: false);
		}
		float num = positionRightButton;
		bool flag = false;
		for (int i = 0; i < buttonOrder.Count; i++)
		{
			if (buttonOrder[i].activeSelf)
			{
				if (buttonOrder[i] == score.gameObject || buttonOrder[i] == madness.gameObject)
				{
					num = (flag ? (num - adjustmentForBigButtons) : (num - adjustmentForBigButtons * 0.5f));
					flag = true;
				}
				else
				{
					flag = false;
				}
				buttonOrder[i].transform.localPosition = new Vector3(num, buttonOrder[i].transform.localPosition.y, buttonOrder[i].transform.localPosition.z);
				num -= distanceBetweenButton;
			}
		}
	}

	public void Hide()
	{
		if (elements.gameObject.activeSelf)
		{
			elements.gameObject.SetActive(value: false);
			ResetIndexController();
		}
	}

	public bool IsActive()
	{
		return elements.gameObject.activeSelf;
	}

	public void SetMadness()
	{
		string value = string.Format(Texts.Instance.GetText("actNumber"), AtOManager.Instance.GetActNumberForText());
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(value);
		stringBuilder.Append("<br><size=-.8><color=#EFA0FF>");
		int num = 0;
		if (GameManager.Instance.IsGameAdventure())
		{
			num = AtOManager.Instance.GetMadnessDifficulty();
		}
		else if (GameManager.Instance.IsSingularity())
		{
			num = AtOManager.Instance.GetSingularityMadness();
		}
		else if (!GameManager.Instance.IsWeeklyChallenge())
		{
			num = AtOManager.Instance.GetObeliskMadness();
		}
		bool flag = false;
		if (num > 0)
		{
			stringBuilder.Append(string.Format(Texts.Instance.GetText("madnessNumber"), num.ToString()));
			flag = true;
		}
		else if (GameManager.Instance.IsWeeklyChallenge())
		{
			stringBuilder.Append(AtOManager.Instance.GetWeeklyName(AtOManager.Instance.GetWeekly()));
			flag = true;
		}
		madnessText.text = stringBuilder.ToString();
		if ((bool)MatchManager.Instance || (bool)MapManager.Instance || (bool)TownManager.Instance || (bool)FinishRunManager.Instance)
		{
			madness.gameObject.SetActive(value: true);
			if (flag)
			{
				madnessParticles.gameObject.SetActive(value: true);
			}
			else
			{
				madnessParticles.gameObject.SetActive(value: false);
			}
		}
		else
		{
			madness.gameObject.SetActive(value: false);
		}
	}

	public void ShowScore(bool state)
	{
		score.gameObject.SetActive(state);
		if (state)
		{
			SetScore();
		}
	}

	public void Resize()
	{
		base.transform.localScale = Globals.Instance.scaleV;
		base.transform.position = new Vector3(Globals.Instance.sizeW * 0.5f - 1.5f * Globals.Instance.scale, Globals.Instance.sizeH * 0.5f - 0.32f * Globals.Instance.scale, base.transform.position.z);
		position.localPosition = new Vector3((0f - Globals.Instance.sizeW) * 0.5f + 1.4f * Globals.Instance.scale, -0.045f * Globals.Instance.scale, position.localPosition.z);
		version.transform.position = new Vector3(Globals.Instance.sizeW * 0.5f - 2.35f * Globals.Instance.scale, (0f - Globals.Instance.sizeH) * 0.5f + 0.22f * Globals.Instance.scale, version.transform.position.z);
	}

	public void SetPositionText(string str = "")
	{
	}

	public void CantExitBecauseRewards()
	{
		AlertManager.buttonClickDelegate = CantExitBecauseRewardsAction;
		AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("exitGameRewards"));
	}

	public void CantExitBecauseRewardsAction()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(CantExitBecauseRewardsAction));
	}

	public void CantExitBecauseEvent()
	{
		AlertManager.buttonClickDelegate = CantExitBecauseEventAction;
		AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("exitGameEvent"));
	}

	public void CantExitBecauseEventAction()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(CantExitBecauseEventAction));
	}

	public void SetScore()
	{
		int num = AtOManager.Instance.CalculateScore();
		score.gameObject.SetActive(value: true);
		scoreText.text = Functions.ScoreFormat(num);
	}

	public void Exit()
	{
		if (GameManager.Instance.IsMaskActive() || AtOManager.Instance.saveLoadStatus)
		{
			return;
		}
		if (RewardsManager.Instance != null || LootManager.Instance != null)
		{
			CantExitBecauseRewards();
			return;
		}
		if (EventManager.Instance != null)
		{
			CantExitBecauseEvent();
			return;
		}
		if (CardPlayerManager.Instance != null && !CardPlayerManager.Instance.CanExit())
		{
			CantExitBecauseEvent();
			return;
		}
		if (CardPlayerPairsManager.Instance != null && !CardPlayerPairsManager.Instance.CanExit())
		{
			CantExitBecauseEvent();
			return;
		}
		bool flag = false;
		if (MapManager.Instance != null || HeroSelectionManager.Instance != null || TownManager.Instance != null || LobbyManager.Instance != null || ChallengeSelectionManager.Instance != null || MatchManager.Instance != null)
		{
			flag = true;
		}
		if (!flag)
		{
			SceneStatic.LoadByName("MainMenu");
			return;
		}
		AlertManager.buttonClickDelegate = ExitGameAction;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Texts.Instance.GetText("exitGameConfirm"));
		stringBuilder.Append("<br><size=-2><color=#aaa>");
		if (!(LobbyManager.Instance != null))
		{
			if (HeroSelectionManager.Instance != null)
			{
				stringBuilder.Append(Texts.Instance.GetText("exitGameConfirmLoss"));
			}
			else if (AtOManager.Instance != null && AtOManager.Instance.IsCombatTool)
			{
				stringBuilder.Append(Texts.Instance.GetText("exitGameConfirmLoss"));
			}
			else
			{
				stringBuilder.Append(Texts.Instance.GetText("exitGameConfirmSave"));
			}
		}
		stringBuilder.Append("</color></size>");
		AlertManager.Instance.AlertConfirmDouble(stringBuilder.ToString(), Texts.Instance.GetText("accept").ToUpper(), Texts.Instance.GetText("cancel").ToUpper());
		AlertManager.Instance.ShowDoorIcon();
	}

	public void ExitGameAction()
	{
		bool confirmAnswer = AlertManager.Instance.GetConfirmAnswer();
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(ExitGameAction));
		if (!GameManager.Instance.IsMaskActive() && !AtOManager.Instance.saveLoadStatus && confirmAnswer)
		{
			DoExit();
		}
	}

	public void DoExit()
	{
		if (GameManager.Instance.IsMaskActive() || AtOManager.Instance.saveLoadStatus)
		{
			return;
		}
		if (!(MatchManager.Instance != null))
		{
			if (TownManager.Instance != null)
			{
				AtOManager.Instance.SaveGame();
			}
			else if (TownManager.Instance != null)
			{
				AtOManager.Instance.SaveGame();
			}
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			NetworkManager.Instance.Disconnect();
		}
		if (ChatManager.Instance != null)
		{
			ChatManager.Instance.DisableChat();
		}
		SceneStatic.LoadByName("MainMenu");
	}

	public void InputMoveController(bool goingRight)
	{
		bool flag = false;
		while (!flag)
		{
			if (!goingRight)
			{
				_indexController++;
				if (_indexController > buttonOrder.Count - 1)
				{
					_indexController = 0;
				}
			}
			else
			{
				_indexController--;
				if (_indexController < 0)
				{
					_indexController = buttonOrder.Count - 1;
				}
			}
			if (buttonOrder[_indexController].activeSelf)
			{
				flag = true;
			}
		}
		Vector2 vector = GameManager.Instance.cameraMain.WorldToScreenPoint(buttonOrder[_indexController].transform.position);
		Mouse.current.WarpCursorPosition(vector);
	}

	private void ResetIndexController()
	{
		_indexController = -1;
	}
}
