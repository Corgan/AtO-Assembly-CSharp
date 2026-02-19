using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Paradox;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TownManager : MonoBehaviour
{
	public CharacterWindowUI characterWindow;

	public SideCharacters sideCharacters;

	public Transform townButtons;

	public Transform tutorialBanner;

	public TMP_Text tutorialBannerText;

	public TreasureRun[] treasureItems;

	public Transform treasureDescription;

	public Transform treasureButtons;

	public Transform treasureHeader;

	public TreasureRun[] treasureItemsCommunity;

	public Transform treasureDescriptionCommunity;

	public Transform treasureButtonsCommunity;

	public Transform treasureHeaderCommunity;

	public Transform treasureBannerDown;

	public TMP_Text treasureTitleCommunity;

	public TMP_Text treasureSubtitleCommunity;

	public BotonGeneric townReady;

	public Transform townUpgrades;

	public TownUpgradeWindow townUpgradeWindow;

	public Transform bgSenenthia;

	public Transform bgVelkarath;

	public Transform bgAquarfall;

	public Transform bgFaeborg;

	public Transform bgVoid;

	public Transform bgUlminin;

	public Transform bgSahti;

	public Transform bgWitchWoods;

	public Transform bgCastleCourtyard;

	public Transform bgCastleSpire;

	public SpriteRenderer treeSR;

	private TreasureRun treasureTarget;

	private bool isThereTreasure;

	public Transform waitingMsg;

	public TMP_Text waitingMsgText;

	private bool statusReady;

	private Coroutine manualReadyCo;

	public Transform joinDivination;

	public TMP_Text joinDivinationText;

	public Transform ItemCreator;

	private bool treasureLock;

	public int LastUsedCharacter = -1;

	public TownBuilding buildingForge;

	public TownBuilding buildingAltar;

	public TownBuilding buildingChurch;

	public TownBuilding buildingCart;

	public TownBuilding buildingArmory;

	public Transform arrowForge;

	public Transform arrowAltar;

	public Transform arrowArmory;

	private int controllerHorizontalIndex = -1;

	private int controllerTreasureIndex = -1;

	private Vector2 warpPosition = Vector2.zero;

	private List<Transform> _controllerList = new List<Transform>();

	public static TownManager Instance { get; private set; }

	private void Awake()
	{
		if (GameManager.Instance == null)
		{
			SceneStatic.LoadByName("MainMenu");
			return;
		}
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		GameManager.Instance.SetCamera();
		NetworkManager.Instance.StartStopQueue(state: true);
	}

	private void Start()
	{
		Resize();
		ShowTownUpgrades(state: false, forceIt: true);
		AudioManager.Instance.DoBSO("Town");
		AtOManager.Instance.ResetTownDivination();
		treasureButtonsCommunity.gameObject.SetActive(value: false);
		treasureHeaderCommunity.gameObject.SetActive(value: false);
		treasureDescriptionCommunity.gameObject.SetActive(value: false);
		waitingMsg.gameObject.SetActive(value: false);
		string townZoneId = AtOManager.Instance.GetTownZoneId();
		bgSenenthia.gameObject.SetActive(value: false);
		bgVelkarath.gameObject.SetActive(value: false);
		bgAquarfall.gameObject.SetActive(value: false);
		bgFaeborg.gameObject.SetActive(value: false);
		bgVoid.gameObject.SetActive(value: false);
		bgUlminin.gameObject.SetActive(value: false);
		bgSahti.gameObject.SetActive(value: false);
		bgWitchWoods.gameObject.SetActive(value: false);
		bgCastleCourtyard.gameObject.SetActive(value: false);
		bgCastleSpire.gameObject.SetActive(value: false);
		if (AtOManager.Instance.IsCombatTool)
		{
			bgVoid.gameObject.SetActive(value: true);
		}
		else
		{
			switch (townZoneId)
			{
			case "Velkarath":
				bgVelkarath.gameObject.SetActive(value: true);
				treeSR.color = Functions.HexToColor("#E99F94");
				break;
			case "Aquarfall":
				bgAquarfall.gameObject.SetActive(value: true);
				break;
			case "Senenthia":
				bgSenenthia.gameObject.SetActive(value: true);
				break;
			case "Faeborg":
				bgFaeborg.gameObject.SetActive(value: true);
				break;
			case "Ulminin":
				bgUlminin.gameObject.SetActive(value: true);
				break;
			case "Sahti":
				bgSahti.gameObject.SetActive(value: true);
				break;
			case "WitchWoods":
				bgWitchWoods.gameObject.SetActive(value: true);
				break;
			case "CastleSpire":
				bgCastleSpire.gameObject.SetActive(value: true);
				break;
			case "CastleCourtyard":
				bgCastleCourtyard.gameObject.SetActive(value: true);
				break;
			default:
				bgVoid.gameObject.SetActive(value: true);
				break;
			}
		}
		AtOManager.Instance.SetPositionText(string.Format(Texts.Instance.GetText("townPlace"), townZoneId));
		SetTownBuildings();
		if (GameManager.Instance.IsMultiplayer())
		{
			statusReady = false;
			townReady.color = Functions.HexToColor(Globals.Instance.ClassColor["warrior"]);
			townReady.SetColor();
			if (NetworkManager.Instance.IsMaster())
			{
				NetworkManager.Instance.ClearAllPlayerManualReady();
			}
		}
		AtOManager.Instance.SetCharInTown(_state: true);
		if (!GameManager.Instance.IsMultiplayer() || (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster()))
		{
			AtOManager.Instance.GenerateTownItemList();
			AtOManager.Instance.SaveGame();
		}
		if (GameManager.Instance.IsSingularity())
		{
			buildingCart.DisableThis();
			townUpgrades.gameObject.SetActive(value: false);
		}
		if (AtOManager.Instance.IsCombatTool)
		{
			buildingCart.DisableThis();
		}
		AtOManager.Instance.NodeScore();
		GameManager.Instance.SceneLoaded();
		GameManager.Instance.ShowTutorialPopup("town", Vector3.zero, Vector3.zero);
		StartCoroutine(TutorialReward());
	}

	private IEnumerator TutorialReward()
	{
		while (GameManager.Instance.IsTutorialActive())
		{
			yield return Globals.Instance.WaitForSeconds(0.1f);
		}
		if (isThereTreasure)
		{
			GameManager.Instance.ShowTutorialPopup("townReward", treasureButtons.position + new Vector3(0.6f, -0.5f, 0f), Vector3.zero);
		}
	}

	public void Resize()
	{
		float num = 1f;
		tutorialBanner.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
		tutorialBanner.transform.position = new Vector3(Globals.Instance.sizeW * 0.5f - 2.4f * num * Globals.Instance.multiplierX, Globals.Instance.sizeH * 0.5f - 2.2f * num * Globals.Instance.multiplierY, tutorialBanner.transform.position.z);
		treasureButtons.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
		treasureButtons.transform.position = new Vector3(Globals.Instance.sizeW * 0.5f - 2.4f * num * Globals.Instance.multiplierX, Globals.Instance.sizeH * 0.5f - 2.2f * num * Globals.Instance.multiplierY, treasureButtons.transform.position.z);
		treasureButtonsCommunity.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
		treasureButtonsCommunity.transform.position = new Vector3(Globals.Instance.sizeW * 0.5f - 2.4f * num * Globals.Instance.multiplierX, Globals.Instance.sizeH * 0.5f - 6.2f * num * Globals.Instance.multiplierY, treasureButtonsCommunity.transform.position.z);
		townReady.transform.position = new Vector3(Globals.Instance.sizeW * 0.5f - 1.6f * num * Globals.Instance.multiplierX, (0f - Globals.Instance.sizeH) * 0.5f + 0.9f * num * Globals.Instance.multiplierY, townReady.transform.position.z);
		sideCharacters.Resize();
		characterWindow.Resize();
	}

	public void DisableReady()
	{
		if (statusReady)
		{
			Ready();
		}
	}

	public void Ready()
	{
		if (!GameManager.Instance.IsMultiplayer())
		{
			if (AtOManager.Instance.TownTutorialStep > -1 && AtOManager.Instance.TownTutorialStep < 3)
			{
				AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("tutorialTownNeedComplete"));
				return;
			}
			ExitTown();
		}
		else
		{
			if (manualReadyCo != null)
			{
				StopCoroutine(manualReadyCo);
			}
			statusReady = !statusReady;
			NetworkManager.Instance.SetManualReady(statusReady);
			if (statusReady)
			{
				townReady.color = Functions.HexToColor(Globals.Instance.ClassColor["scout"]);
				townReady.SetColor();
				townReady.SetText(Texts.Instance.GetText("waitingForPlayers"));
				if (NetworkManager.Instance.IsMaster())
				{
					manualReadyCo = StartCoroutine(CheckForAllManualReady());
				}
			}
			else
			{
				townReady.color = Functions.HexToColor(Globals.Instance.ClassColor["warrior"]);
				townReady.SetColor();
				townReady.SetText(Texts.Instance.GetText("ready"));
			}
		}
		Telemetry.SendActStart();
	}

	private IEnumerator CheckForAllManualReady()
	{
		bool check = true;
		while (check)
		{
			if (!NetworkManager.Instance.AllPlayersManualReady())
			{
				yield return Globals.Instance.WaitForSeconds(1f);
			}
			else
			{
				check = false;
			}
		}
		ExitTown();
	}

	public void SetWaitingPlayersText(string msg)
	{
		if (msg != "")
		{
			waitingMsg.gameObject.SetActive(value: true);
			waitingMsgText.text = msg;
		}
		else
		{
			waitingMsg.gameObject.SetActive(value: false);
		}
	}

	private void ExitTown()
	{
		AtOManager.Instance.LaunchMap();
	}

	public void ShowCharacterWindow(string type = "", int heroIndex = -1)
	{
		ShowButtons(state: false);
		characterWindow.Show(type, heroIndex);
	}

	public void HideCharacterWindow()
	{
		if (!(CardCraftManager.Instance != null) || CardCraftManager.Instance.craftType != 3)
		{
			ShowButtons(state: true);
		}
	}

	private void CreateTutorialBanner()
	{
		bool flag = false;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<line-height=110%><br>-<br></line-height>");
		StringBuilder stringBuilder2 = new StringBuilder();
		int townTutorialStep = AtOManager.Instance.TownTutorialStep;
		if (townTutorialStep == 0)
		{
			flag = true;
			stringBuilder2.Append("<color=");
			stringBuilder2.Append("#461518");
			stringBuilder2.Append(">");
			stringBuilder2.Append("<size=+.5><sprite name=questBegin></size> <size=+.3>");
		}
		stringBuilder2.Append(string.Format(Texts.Instance.GetText("townTutorialStep0"), Globals.Instance.GetCardData("fireball", instantiate: false).CardName, "Evelyn"));
		if (flag)
		{
			stringBuilder2.Append("</size></color>");
		}
		flag = false;
		stringBuilder2.Append(stringBuilder.ToString());
		if (townTutorialStep == 1)
		{
			flag = true;
			stringBuilder2.Append("<color=");
			stringBuilder2.Append("#461518");
			stringBuilder2.Append(">");
			stringBuilder2.Append("<size=+.5><sprite name=questBegin></size> <size=+.3>");
		}
		stringBuilder2.Append(string.Format(Texts.Instance.GetText("townTutorialStep1"), Globals.Instance.GetCardData("faststrike", instantiate: false).CardName, "Magnus"));
		if (flag)
		{
			stringBuilder2.Append("</size></color>");
		}
		flag = false;
		stringBuilder2.Append(stringBuilder.ToString());
		if (townTutorialStep == 2)
		{
			flag = true;
			stringBuilder2.Append("<color=");
			stringBuilder2.Append("#461518");
			stringBuilder2.Append(">");
			stringBuilder2.Append("<size=+.5><sprite name=questBegin></size> <size=+.3>");
		}
		stringBuilder2.Append(string.Format(Texts.Instance.GetText("townTutorialStep2"), Globals.Instance.GetCardData("spyglass", instantiate: false).CardName, "Andrin"));
		if (flag)
		{
			stringBuilder2.Append("</size></color>");
		}
		tutorialBannerText.text = stringBuilder2.ToString();
	}

	public void ShowButtons(bool state)
	{
		townButtons.gameObject.SetActive(state);
		treasureButtons.gameObject.SetActive(state);
		arrowForge.gameObject.SetActive(value: false);
		arrowAltar.gameObject.SetActive(value: false);
		arrowArmory.gameObject.SetActive(value: false);
		bool flag = false;
		if (state && AtOManager.Instance.TownTutorialStep >= 0 && AtOManager.Instance.TownTutorialStep < 3)
		{
			tutorialBanner.gameObject.SetActive(value: true);
			CreateTutorialBanner();
			if (AtOManager.Instance.TownTutorialStep == 0)
			{
				arrowForge.gameObject.SetActive(value: true);
			}
			else if (AtOManager.Instance.TownTutorialStep == 1)
			{
				arrowAltar.gameObject.SetActive(value: true);
			}
			else
			{
				arrowArmory.gameObject.SetActive(value: true);
			}
			flag = true;
		}
		else
		{
			tutorialBanner.gameObject.SetActive(value: false);
		}
		if (flag || AtOManager.Instance.GetNgPlus() > 2 || GameManager.Instance.IsSingularity() || AtOManager.Instance.IsCombatTool)
		{
			treasureButtons.gameObject.SetActive(value: false);
			treasureButtonsCommunity.gameObject.SetActive(value: false);
		}
		else if (state)
		{
			StartCoroutine(SetTreasures());
		}
		else
		{
			treasureButtons.transform.position = new Vector3(treasureButtons.transform.position.x, treasureButtons.transform.position.y, -100f);
		}
	}

	public void ShowJoinDivination(bool state = true)
	{
		if (state)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<color=");
			stringBuilder.Append(NetworkManager.Instance.GetColorFromNick(AtOManager.Instance.townDivinationCreator));
			stringBuilder.Append(">");
			stringBuilder.Append(AtOManager.Instance.townDivinationCreator);
			stringBuilder.Append("</color>");
			joinDivinationText.text = string.Format(Texts.Instance.GetText("divinationRoundJoin"), stringBuilder.ToString());
		}
		joinDivination.gameObject.SetActive(state);
	}

	public void ShowDeck(int auxInt)
	{
		characterWindow.Show("deck", auxInt);
	}

	private IEnumerator SetTreasures()
	{
		int index = 0;
		isThereTreasure = false;
		LockTreasures(state: false);
		treasureButtons.transform.position = new Vector3(treasureButtons.transform.position.x, treasureButtons.transform.position.y, 0f);
		treasureButtons.gameObject.SetActive(value: false);
		treasureButtons.gameObject.SetActive(value: true);
		treasureDescription.gameObject.SetActive(value: false);
		treasureItems[0].Hide();
		treasureItems[1].Hide();
		treasureItems[2].Hide();
		treasureBannerDown.localPosition = new Vector3(treasureBannerDown.localPosition.x, 0.8f, treasureBannerDown.localPosition.z);
		if (PlayerManager.Instance.PlayerRuns != null)
		{
			for (int i = PlayerManager.Instance.PlayerRuns.Count - 1; i >= 0; i--)
			{
				if (i >= 0)
				{
					PlayerRun playerRun = JsonUtility.FromJson<PlayerRun>(PlayerManager.Instance.PlayerRuns[i]);
					if (playerRun.MadnessLevel <= 2 && !playerRun.ObeliskChallenge && !playerRun.Singularity && !playerRun.SandboxEnabled && (PlayerManager.Instance.TreasuresClaimed == null || !PlayerManager.Instance.IsTreasureClaimed(playerRun.Id)))
					{
						if (index <= 2)
						{
							if (playerRun.GoldGained > 0 || playerRun.DustGained > 0)
							{
								isThereTreasure = true;
								if (!(treasureItems[index] == null))
								{
									treasureItems[index].SetTreasure(playerRun, index);
									yield return Globals.Instance.WaitForSeconds(0.1f);
									index++;
									if (!treasureButtons.gameObject.activeSelf)
									{
										treasureButtons.gameObject.SetActive(value: true);
										treasureHeader.gameObject.SetActive(value: true);
									}
									switch (index)
									{
									case 3:
										treasureBannerDown.localPosition = new Vector3(treasureBannerDown.localPosition.x, -1.86f, treasureBannerDown.localPosition.z);
										break;
									case 2:
										treasureBannerDown.localPosition = new Vector3(treasureBannerDown.localPosition.x, -0.56f, treasureBannerDown.localPosition.z);
										break;
									case 1:
										treasureBannerDown.localPosition = new Vector3(treasureBannerDown.localPosition.x, 0.8f, treasureBannerDown.localPosition.z);
										break;
									}
								}
							}
						}
						else
						{
							PlayerManager.Instance.ClaimTreasure(playerRun.Id, save: false);
						}
					}
				}
			}
		}
		if (index == 0)
		{
			ShowTreasureDescription();
		}
		treasureItemsCommunity[0].Hide();
		treasureItemsCommunity[1].Hide();
		treasureItemsCommunity[2].Hide();
		if (GameManager.Instance.communityRewards != null && GameManager.Instance.communityRewards.Count > 0 && !Functions.Expired(GameManager.Instance.communityRewardsExpire))
		{
			int num = 0;
			for (int num2 = GameManager.Instance.communityRewards.Count - 1; num2 >= 0; num2--)
			{
				string[] array = GameManager.Instance.communityRewards[num2].Split(',');
				if (array == null || array.Length != 3)
				{
					yield break;
				}
				string text = array[0].Trim();
				int num3 = int.Parse(array[1].Trim());
				int num4 = int.Parse(array[2].Trim());
				if (num3 < 0 && num4 < 0)
				{
					yield break;
				}
				if (PlayerManager.Instance.TreasuresClaimed == null || !PlayerManager.Instance.IsTreasureClaimed(text))
				{
					treasureItemsCommunity[num].SetTreasureCommunity(text, num3, num4, num);
					num++;
				}
			}
			if (num > 0)
			{
				treasureButtonsCommunity.gameObject.SetActive(value: true);
				treasureHeaderCommunity.gameObject.SetActive(value: true);
				treasureDescriptionCommunity.gameObject.SetActive(value: true);
			}
			else
			{
				treasureButtonsCommunity.gameObject.SetActive(value: false);
				treasureHeaderCommunity.gameObject.SetActive(value: false);
				treasureDescriptionCommunity.gameObject.SetActive(value: false);
			}
		}
		else
		{
			treasureButtonsCommunity.gameObject.SetActive(value: false);
			treasureHeaderCommunity.gameObject.SetActive(value: false);
			treasureDescriptionCommunity.gameObject.SetActive(value: false);
		}
	}

	private void ShowTreasureDescription()
	{
		treasureButtons.gameObject.SetActive(value: false);
		treasureDescription.gameObject.SetActive(value: false);
		treasureHeader.gameObject.SetActive(value: false);
	}

	public void ClaimTreasureCommunity(string _treasureId)
	{
		if (treasureLock)
		{
			return;
		}
		for (int i = 0; i < 3; i++)
		{
			if (treasureItemsCommunity[i] != null && treasureItemsCommunity[i].treasureId == _treasureId)
			{
				treasureTarget = treasureItemsCommunity[i];
				AlertManager.buttonClickDelegate = OpenTreasureCommunity;
				AlertManager.Instance.AlertConfirmDouble(Texts.Instance.GetText("wantToClaimTreasure"));
				break;
			}
		}
	}

	private void OpenTreasureCommunity()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(OpenTreasureCommunity));
		if (AlertManager.Instance.GetConfirmAnswer())
		{
			LockTreasures(state: true);
			PlayerManager.Instance.ClaimTreasure(treasureTarget.treasureId);
			treasureTarget.ClaimCommunity();
		}
	}

	public void ClaimTreasure(string _treasureId)
	{
		if (treasureLock)
		{
			return;
		}
		for (int i = 0; i < 3; i++)
		{
			treasureButtonsCommunity.gameObject.SetActive(value: false);
			if (treasureItems[i] != null && treasureItems[i].treasureId == _treasureId)
			{
				treasureTarget = treasureItems[i];
				AlertManager.buttonClickDelegate = OpenTreasure;
				AlertManager.Instance.AlertConfirmDouble(Texts.Instance.GetText("wantToClaimTreasure"));
				break;
			}
		}
	}

	private void OpenTreasure()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(OpenTreasure));
		if (AlertManager.Instance.GetConfirmAnswer())
		{
			LockTreasures(state: true);
			PlayerManager.Instance.ClaimTreasure(treasureTarget.treasureId, save: false);
			treasureTarget.Claim();
		}
	}

	public void LockTreasures(bool state)
	{
		treasureLock = state;
	}

	public bool AreTreasuresLocked()
	{
		return treasureLock;
	}

	public void MoveTreasuresUp(string _treasureFromId, bool _isCommunity)
	{
		int num = -1;
		for (int i = 0; i < 3; i++)
		{
			if (!_isCommunity)
			{
				if (treasureItems[i] != null && treasureItems[i].treasureId == _treasureFromId)
				{
					num = i;
					break;
				}
			}
			else if (treasureItemsCommunity[i] != null && treasureItemsCommunity[i].treasureId == _treasureFromId)
			{
				num = i;
				break;
			}
		}
		TreasureRun treasureRun = null;
		if (num > -1)
		{
			for (int j = 0; j < 3; j++)
			{
				if (!_isCommunity)
				{
					if (j != num && j > num && treasureItems[j] != null && treasureItems[j].treasureId != "" && treasureItems[j].gameObject.activeSelf)
					{
						treasureItems[j].MoveUp();
						treasureRun = treasureItems[j];
					}
				}
				else if (j != num && j > num && treasureItemsCommunity[j] != null && treasureItemsCommunity[j].treasureId != "" && treasureItemsCommunity[j].gameObject.activeSelf)
				{
					treasureItemsCommunity[j].MoveUp();
				}
			}
		}
		bool flag = true;
		int num2 = 0;
		for (int k = 0; k < 3; k++)
		{
			if (!_isCommunity)
			{
				if (treasureItems[k] != null && treasureItems[k].treasureId != "" && !treasureItems[k].claimed)
				{
					flag = false;
					num2++;
				}
			}
			else if (treasureItemsCommunity[k] != null && treasureItemsCommunity[k].treasureId != "" && !treasureItemsCommunity[k].claimed)
			{
				flag = false;
			}
		}
		if (flag)
		{
			if (!_isCommunity)
			{
				treasureButtons.gameObject.SetActive(value: false);
				treasureHeader.gameObject.SetActive(value: false);
			}
			else
			{
				treasureHeaderCommunity.gameObject.SetActive(value: false);
			}
			return;
		}
		switch (num2)
		{
		case 3:
			treasureBannerDown.localPosition = new Vector3(treasureBannerDown.localPosition.x, -1.56f, treasureBannerDown.localPosition.z);
			break;
		case 2:
			treasureBannerDown.localPosition = new Vector3(treasureBannerDown.localPosition.x, -0.26f, treasureBannerDown.localPosition.z);
			break;
		case 1:
			treasureBannerDown.localPosition = new Vector3(treasureBannerDown.localPosition.x, 1.1f, treasureBannerDown.localPosition.z);
			if (treasureRun != null && treasureRun.separator != null)
			{
				treasureRun.separator.gameObject.SetActive(value: false);
			}
			break;
		default:
			ShowTreasureDescription();
			break;
		}
	}

	public void SellSupply()
	{
		townUpgradeWindow.ShowSellSupply(state: true);
	}

	public void SellSupplyAction()
	{
		townUpgradeWindow.SellSupplyAction();
	}

	public void CloseSupply()
	{
		townUpgradeWindow.ShowSellSupply(state: false);
	}

	public void ModifySupplyQuantity(int quantity)
	{
		townUpgradeWindow.ModifySupplyQuantity(quantity);
	}

	public void ShowTownUpgrades(bool state, bool forceIt = false)
	{
		if (townUpgradeWindow.IsActive() != state || forceIt)
		{
			townUpgradeWindow.Show(state);
			if (state)
			{
				sideCharacters.gameObject.SetActive(value: false);
				return;
			}
			sideCharacters.gameObject.SetActive(value: true);
			sideCharacters.Show();
		}
	}

	public void RefreshTownUpgrades()
	{
		townUpgradeWindow.Refresh();
	}

	public string GetUpgradeButtonId(int column, int row)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("townUpgrade_");
		stringBuilder.Append(column);
		stringBuilder.Append("_");
		stringBuilder.Append(row);
		return stringBuilder.ToString();
	}

	public void SetTownBuildings()
	{
		int[] array = new int[5];
		for (int i = 0; i < 5; i++)
		{
			array[i] = 0;
		}
		string text = "";
		for (int j = 0; j < townUpgradeWindow.botonSupply.Count; j++)
		{
			text = GetUpgradeButtonId(Mathf.CeilToInt(j / 6) + 1, Mathf.CeilToInt(j % 6) + 1);
			if (!PlayerManager.Instance.PlayerHaveSupply(text))
			{
				continue;
			}
			switch (j)
			{
			case 2:
			case 3:
				array[0] = 1;
				continue;
			case 0:
			case 1:
			case 4:
			case 5:
				if (j >= 4)
				{
					array[0] = 2;
				}
				continue;
			}
			switch (j)
			{
			case 8:
			case 9:
				array[1] = 1;
				continue;
			case 6:
			case 7:
			case 10:
			case 11:
				if (j >= 10)
				{
					array[1] = 2;
				}
				continue;
			}
			switch (j)
			{
			case 14:
			case 15:
				array[2] = 1;
				continue;
			case 12:
			case 13:
			case 16:
			case 17:
				if (j >= 16)
				{
					array[2] = 2;
				}
				continue;
			}
			switch (j)
			{
			case 20:
			case 21:
				array[3] = 1;
				continue;
			case 18:
			case 19:
			case 22:
			case 23:
				if (j >= 22)
				{
					array[3] = 2;
				}
				continue;
			}
			switch (j)
			{
			case 26:
			case 27:
				array[4] = 1;
				break;
			case 24:
			case 25:
			case 28:
			case 29:
				if (j >= 28)
				{
					array[4] = 2;
				}
				break;
			}
		}
		buildingForge.Init(array[0]);
		buildingAltar.Init(array[1]);
		buildingChurch.Init(array[2]);
		buildingCart.Init(array[3]);
		buildingArmory.Init(array[4]);
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false, bool shoulderLeft = false, bool shoulderRight = false)
	{
		if (townUpgradeWindow.IsActive())
		{
			townUpgradeWindow.ControllerMovement(goingUp, goingRight, goingDown, goingLeft);
			return;
		}
		_controllerList.Clear();
		_controllerList.Add(buildingForge.transform);
		_controllerList.Add(buildingChurch.transform);
		_controllerList.Add(buildingAltar.transform);
		if (buildingCart.transform.gameObject.activeSelf)
		{
			_controllerList.Add(buildingCart.transform);
		}
		_controllerList.Add(buildingArmory.transform);
		_controllerList.Add(townReady.transform);
		if (townUpgrades.gameObject.activeSelf)
		{
			_controllerList.Add(townUpgrades.transform);
		}
		controllerTreasureIndex = -1;
		for (int i = 0; i < treasureItems.Length; i++)
		{
			if (Functions.TransformIsVisible(treasureItems[i].transform))
			{
				_controllerList.Add(treasureItems[i].transform);
				controllerTreasureIndex = _controllerList.Count - 1;
			}
		}
		for (int j = 0; j < sideCharacters.transform.childCount; j++)
		{
			if (Functions.TransformIsVisible(sideCharacters.transform.GetChild(j).transform))
			{
				_controllerList.Add(sideCharacters.transform.GetChild(j).transform);
			}
		}
		if (Functions.TransformIsVisible(PlayerUIManager.Instance.giveGold))
		{
			_controllerList.Add(PlayerUIManager.Instance.giveGold);
		}
		controllerHorizontalIndex = Functions.GetListClosestIndexToMousePosition(_controllerList);
		if ((_controllerList[controllerHorizontalIndex] == townUpgrades.transform && goingRight) || (_controllerList[controllerHorizontalIndex] == buildingArmory.transform && goingUp))
		{
			controllerHorizontalIndex = controllerTreasureIndex;
		}
		else
		{
			controllerHorizontalIndex = Functions.GetClosestIndexBasedOnDirection(_controllerList, controllerHorizontalIndex, goingUp, goingRight, goingDown, goingLeft);
		}
		if (_controllerList[controllerHorizontalIndex] != null)
		{
			if (controllerHorizontalIndex != 1)
			{
				warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerList[controllerHorizontalIndex].position);
			}
			else
			{
				warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerList[controllerHorizontalIndex].position - new Vector3(0f, 2f, 0f));
			}
			Mouse.current.WarpCursorPosition(warpPosition);
		}
	}

	public void ControllerMoveBlock(bool _isRight)
	{
		characterWindow.IsActive();
	}
}
