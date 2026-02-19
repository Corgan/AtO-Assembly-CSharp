using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Paradox;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class FinishRunManager : MonoBehaviour
{
	public int gameScore;

	private int goldSummary;

	private int dustSummary;

	public Transform flag;

	public BotonGeneric mainMenuButton;

	public List<TMP_Text> titlesTexts;

	public TMP_Text subHeader;

	public TMP_Text placesText;

	public TMP_Text placesTextGold;

	public TMP_Text deathText;

	public TMP_Text deathTextGold;

	public TMP_Text deathsText;

	public TMP_Text deathsTextGold;

	public TMP_Text experienceText;

	public TMP_Text experienceTextGold;

	public TMP_Text bossesText;

	public TMP_Text bossesTextGold;

	public TMP_Text corruptionsText;

	public TMP_Text corruptionsTextGold;

	public Transform completedBlock;

	public TMP_Text completedText;

	public TMP_Text completedTextGold;

	public TMP_Text retentionText;

	public TMP_Text retentionTextGold;

	public TMP_Text finalScore;

	public TMP_Text playedTimeText;

	public TMP_Text finalScoreMadness;

	public TMP_Text gameScoreSub;

	public TMP_Text gameScoreTextGold;

	public TMP_Text totalTextGold;

	public TMP_Text gameReward;

	public TMP_Text mpBonus;

	public Transform bestScore;

	public Transform rewardsT;

	public Transform retentionTransform;

	public Transform rewardGroup;

	public SpriteRenderer spr0;

	public FinishProgression fp0;

	public SpriteRenderer spr1;

	public FinishProgression fp1;

	public SpriteRenderer spr2;

	public FinishProgression fp2;

	public SpriteRenderer spr3;

	public FinishProgression fp3;

	public CharacterWindowUI characterWindow;

	private bool[] lockedSate = new bool[4];

	private List<string> unlockedCards = new List<string>();

	private Coroutine finishCo;

	public int controllerHorizontalIndex = -1;

	private Vector2 warpPosition = Vector2.zero;

	private List<Transform> _controllerList = new List<Transform>();

	public static FinishRunManager Instance { get; private set; }

	private void Awake()
	{
		if (GameManager.Instance == null)
		{
			SceneStatic.LoadByName("FinishRun");
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
		GameManager.Instance.SetCamera();
		rewardsT.gameObject.SetActive(value: false);
		bestScore.gameObject.SetActive(value: false);
		NetworkManager.Instance.StartStopQueue(state: true);
	}

	private void Start()
	{
		if (AtOManager.Instance.currentMapNode == null || AtOManager.Instance.currentMapNode == "")
		{
			if (AtOManager.Instance.IsCombatTool)
			{
				SceneStatic.LoadByName("HeroSelection");
			}
			else
			{
				SceneStatic.LoadByName("MainMenu");
			}
		}
		else
		{
			StartCoroutine(StartCo());
		}
	}

	private IEnumerator StartCo()
	{
		mainMenuButton.Disable();
		CalculateFinishRunReward();
		Telemetry.SendPlaysessionEnd();
		if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
		{
			AtOManager.Instance.RemoveSave();
		}
		GameManager.Instance.SceneLoaded();
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			if (PlayerManager.Instance.UnlockedCardsByGame.ContainsKey(AtOManager.Instance.GetGameId()))
			{
				unlockedCards = PlayerManager.Instance.UnlockedCardsByGame[AtOManager.Instance.GetGameId()];
				if (unlockedCards != null && unlockedCards.Count > 0)
				{
					characterWindow.ShowUnlockedCards(unlockedCards);
					yield return Globals.Instance.WaitForSeconds(0.15f);
				}
			}
			while (characterWindow.IsActive())
			{
				yield return Globals.Instance.WaitForSeconds(0.05f);
			}
		}
		yield return Globals.Instance.WaitForSeconds(1f);
		mainMenuButton.Enable();
	}

	private string GoldDust(int gold, int dust)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(gold);
		stringBuilder.Append(" <sprite name=gold>  ");
		stringBuilder.Append(dust);
		stringBuilder.Append(" <sprite name=dust>");
		return stringBuilder.ToString();
	}

	public void CalculateFinishRunReward()
	{
		int townTier = AtOManager.Instance.GetTownTier();
		string text = "";
		if (AtOManager.Instance.currentMapNode != "" && Globals.Instance.GetNodeData(AtOManager.Instance.currentMapNode) != null)
		{
			text = Globals.Instance.GetNodeData(AtOManager.Instance.currentMapNode).NodeName;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<color=#FFFFFF>");
		stringBuilder.Append(Texts.Instance.GetText("questEndsIn"));
		stringBuilder.Append("</color>\n<size=+2>");
		if (text != "")
		{
			stringBuilder.Append(text);
			stringBuilder.Append("</size>");
			stringBuilder.Append("<br>");
		}
		string value = string.Format(Texts.Instance.GetText("actNumber"), townTier switch
		{
			0 => "1", 
			1 => "2", 
			2 => "3", 
			_ => "4", 
		});
		stringBuilder.Append(value);
		subHeader.text = stringBuilder.ToString();
		goldSummary = 0;
		dustSummary = 0;
		Hero[] array = new Hero[4];
		ZoneData zoneData = null;
		if (AtOManager.Instance.currentMapNode != "" && Globals.Instance.GetNodeData(AtOManager.Instance.currentMapNode) != null)
		{
			zoneData = Globals.Instance.GetNodeData(AtOManager.Instance.currentMapNode).NodeZone;
		}
		array = ((!(zoneData != null) || zoneData.ChangeTeamOnEntrance) ? AtOManager.Instance.GetTeamBackup() : AtOManager.Instance.GetTeam());
		int num = 0;
		if (array != null)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && (array[i].Owner == NetworkManager.Instance.GetPlayerNick() || array[i].Owner == "" || array[i].Owner == null))
				{
					num++;
				}
			}
		}
		int num2 = 0;
		for (int j = 0; j < AtOManager.Instance.mapVisitedNodes.Count; j++)
		{
			if (Globals.Instance.GetNodeData(AtOManager.Instance.mapVisitedNodes[j]) != null && Globals.Instance.GetNodeData(AtOManager.Instance.mapVisitedNodes[j]).NodeZone != null && !Globals.Instance.GetNodeData(AtOManager.Instance.mapVisitedNodes[j]).NodeZone.DisableExperienceOnThisZone)
			{
				num2++;
			}
		}
		int num3 = num2 - 2;
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			num3--;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		int num4 = num3 * 36;
		placesText.text = num3.ToString();
		placesTextGold.text = num4.ToString();
		int num5 = AtOManager.Instance.combatExpertise;
		if (num5 < 0)
		{
			num5 = 0;
		}
		int num6 = num5 * 13;
		deathText.text = num5.ToString();
		deathTextGold.text = num6.ToString();
		int num7 = 0;
		int num8 = 0;
		for (int k = 0; k < 4; k++)
		{
			num8 += array[k].Experience;
			num7 += array[k].TotalDeaths;
		}
		int num9 = -num7 * 100;
		deathsText.text = num7.ToString();
		deathsTextGold.text = num9.ToString();
		int num10 = Functions.FuncRoundToInt((float)num8 * 0.5f);
		experienceText.text = num8.ToString();
		experienceTextGold.text = num10.ToString();
		int bossesKilled = AtOManager.Instance.bossesKilled;
		bossesText.text = bossesKilled.ToString();
		int num11 = bossesKilled * 80;
		bossesTextGold.text = num11.ToString();
		int num12 = AtOManager.Instance.corruptionCommonCompleted + AtOManager.Instance.corruptionUncommonCompleted + AtOManager.Instance.corruptionRareCompleted + AtOManager.Instance.corruptionEpicCompleted;
		corruptionsText.text = num12.ToString();
		int num13 = AtOManager.Instance.corruptionCommonCompleted * 40 + AtOManager.Instance.corruptionUncommonCompleted * 80 + AtOManager.Instance.corruptionRareCompleted * 130 + AtOManager.Instance.corruptionEpicCompleted * 200;
		corruptionsTextGold.text = num13.ToString();
		int num14 = 0;
		if (AtOManager.Instance.IsAdventureCompleted())
		{
			completedBlock.gameObject.SetActive(value: true);
			num14 = 500;
			completedTextGold.text = num14.ToString();
		}
		else
		{
			completedBlock.gameObject.SetActive(value: false);
		}
		int num15 = num4 + num6 + num9 + num10 + num11 + num13 + num14;
		if (num15 < 0)
		{
			num15 = 0;
		}
		int num16 = 0;
		if (GameManager.Instance.IsSingularity())
		{
			num16 = AtOManager.Instance.GetSingularityMadness();
		}
		else if (!GameManager.Instance.IsObeliskChallenge())
		{
			num16 = AtOManager.Instance.GetMadnessDifficulty();
		}
		else if (!GameManager.Instance.IsWeeklyChallenge())
		{
			num16 = AtOManager.Instance.GetObeliskMadness();
		}
		int madnessScoreMultiplier = Functions.GetMadnessScoreMultiplier(num16, GameManager.Instance.IsGameAdventure());
		if (num16 > 0)
		{
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append("+");
			stringBuilder2.Append(madnessScoreMultiplier);
			if (Functions.SpaceBeforePercentSign())
			{
				stringBuilder2.Append(" ");
			}
			stringBuilder2.Append("% <size=-.5>");
			stringBuilder2.Append(Texts.Instance.GetText("madness"));
			stringBuilder2.Append("</size>");
			finalScoreMadness.text = stringBuilder2.ToString();
		}
		else
		{
			finalScoreMadness.text = "";
		}
		gameScore = num15 + Functions.FuncRoundToInt(num15 * madnessScoreMultiplier / 100);
		finalScore.text = gameScore.ToString();
		StartCoroutine(RemoveDoubleDots());
		bool flag = true;
		if (SandboxManager.Instance.IsEnabled())
		{
			flag = false;
		}
		else if (GameManager.Instance.IsMultiplayer())
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			string text2 = "";
			for (int l = 0; l < 4; l++)
			{
				if (array[l] != null && !(array[l].HeroData == null))
				{
					if (array[l].OwnerOriginal == null)
					{
						break;
					}
					text2 = array[l].OwnerOriginal.ToLower();
					if (!list.Contains(text2))
					{
						list.Add(text2);
					}
				}
			}
			for (int m = 0; m < 4; m++)
			{
				if (array[m] == null || array[m].HeroData == null)
				{
					continue;
				}
				text2 = NetworkManager.Instance.GetPlayerNickReal(array[m].Owner);
				if (text2 != "")
				{
					text2 = text2.ToLower();
					if (!list2.Contains(text2))
					{
						list2.Add(text2);
					}
				}
			}
			if (list.Count != list2.Count)
			{
				flag = false;
			}
			else
			{
				for (int n = 0; n < list2.Count; n++)
				{
					if (!list.Contains(list2[n]))
					{
						flag = false;
						break;
					}
				}
			}
		}
		if (flag)
		{
			if (GameManager.Instance.IsGameAdventure())
			{
				if (gameScore > PlayerManager.Instance.BestScore)
				{
					bestScore.gameObject.SetActive(value: true);
					PlayerManager.Instance.SetBestScore(gameScore);
				}
				if ((gameScore <= 170000 || num16 >= 18) && (gameScore <= 250000 || num16 < 18))
				{
					SteamManager.Instance.SetScore(gameScore, !GameManager.Instance.IsMultiplayer());
				}
			}
			else if (GameManager.Instance.IsSingularity())
			{
				if (gameScore < 170000)
				{
					SteamManager.Instance.SetSingularityScore(gameScore, !GameManager.Instance.IsMultiplayer());
				}
			}
			else if (gameScore <= 120000)
			{
				if (GameManager.Instance.IsWeeklyChallenge())
				{
					string steamName = SteamManager.Instance.steamName;
					string text3 = "";
					if (GameManager.Instance.IsMultiplayer())
					{
						Player[] playerList = NetworkManager.Instance.PlayerList;
						foreach (Player player in playerList)
						{
							text3 = text3 + player.NickName + "-";
						}
					}
					int weekly = AtOManager.Instance.GetWeekly();
					SteamManager.Instance.SetWeeklyScore(gameScore, weekly, steamName, text3, !GameManager.Instance.IsMultiplayer());
					ChallengeData weeklyData = Globals.Instance.GetWeeklyData(weekly);
					if (weeklyData != null && weeklyData.IdSteam != "")
					{
						SteamManager.Instance.SetStatInt(weeklyData.IdSteam, 1);
					}
					weeklyData = Globals.Instance.GetWeeklyData(weekly, _getSecondary: true);
					if (weeklyData != null && weeklyData.IdSteam != "")
					{
						SteamManager.Instance.SetStatInt(weeklyData.IdSteam, 1);
					}
				}
				else
				{
					SteamManager.Instance.SetObeliskScore(gameScore, !GameManager.Instance.IsMultiplayer());
				}
			}
		}
		else if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("DON'T SEND Steam Score", "trace");
		}
		playedTimeText.text = string.Format(Texts.Instance.GetText("timePlayed"), Functions.FloatToTime(AtOManager.Instance.playedTime));
		if (num == 4)
		{
			goldSummary = Functions.FuncRoundToInt((float)num15 * 0.6f);
			switch (townTier)
			{
			case 0:
				dustSummary = Functions.FuncRoundToInt((float)num15 * 1f);
				break;
			case 1:
				goldSummary = Functions.FuncRoundToInt((float)num15 * 0.5f);
				dustSummary = Functions.FuncRoundToInt((float)num15 * 0.75f);
				break;
			case 2:
				goldSummary = Functions.FuncRoundToInt((float)num15 * 0.5f);
				dustSummary = Functions.FuncRoundToInt((float)num15 * 0.7f);
				break;
			case 3:
				goldSummary = Functions.FuncRoundToInt((float)num15 * 0.5f);
				dustSummary = Functions.FuncRoundToInt((float)num15 * 0.6f);
				break;
			default:
				dustSummary = num15;
				break;
			}
		}
		else
		{
			goldSummary = num * Functions.FuncRoundToInt((float)num15 * 0.3f);
			dustSummary = num * Functions.FuncRoundToInt((float)num15 * 0.4f);
			switch (townTier)
			{
			case 1:
				goldSummary = num * Functions.FuncRoundToInt((float)num15 * 0.25f);
				dustSummary = num * Functions.FuncRoundToInt((float)num15 * 0.35f);
				break;
			case 2:
				goldSummary = num * Functions.FuncRoundToInt((float)num15 * 0.2f);
				dustSummary = num * Functions.FuncRoundToInt((float)num15 * 0.25f);
				break;
			case 3:
				goldSummary = num * Functions.FuncRoundToInt((float)num15 * 0.2f);
				dustSummary = num * Functions.FuncRoundToInt((float)num15 * 0.25f);
				break;
			}
		}
		int num18 = 0;
		string madnessCorruptors = "";
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			num18 = AtOManager.Instance.GetNgPlus();
			madnessCorruptors = AtOManager.Instance.GetMadnessCorruptors();
		}
		if (num18 > 0 && num18 < 3)
		{
			goldSummary *= 2;
			dustSummary *= 2;
			StringBuilder stringBuilder3 = new StringBuilder();
			stringBuilder3.Append("+100");
			if (Functions.SpaceBeforePercentSign())
			{
				stringBuilder3.Append(" ");
			}
			stringBuilder3.Append("%<br><size=-.5>");
			stringBuilder3.Append(Texts.Instance.GetText("madness"));
			stringBuilder3.Append("</size>");
			gameScoreSub.text = stringBuilder3.ToString();
		}
		else
		{
			gameScoreSub.text = "";
		}
		gameScoreTextGold.text = GoldDust(goldSummary, dustSummary);
		float num19 = 5f;
		if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_6_5"))
		{
			num19 = 30f;
		}
		else if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_6_3"))
		{
			num19 = 20f;
		}
		else if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_6_1"))
		{
			num19 = 10f;
		}
		int num20 = 0;
		for (int num21 = 0; num21 < 4; num21++)
		{
			num20 += array[num21].GetItemFinalRewardRetentionModification();
		}
		num19 += (float)num20;
		retentionText.text = num19 + (Functions.SpaceBeforePercentSign() ? " %" : "%");
		if (num3 == 0)
		{
			int num22 = 0;
			retentionTransform.gameObject.SetActive(value: false);
		}
		else
		{
			int num23 = Functions.FuncRoundToInt((float)AtOManager.Instance.GetPlayerGold() * num19 * 0.01f);
			int num22 = Functions.FuncRoundToInt((float)AtOManager.Instance.GetPlayerDust() * num19 * 0.01f);
			goldSummary += num23;
			dustSummary += num22;
			retentionTextGold.text = GoldDust(num23, num22);
		}
		if (goldSummary < 0)
		{
			goldSummary = 0;
		}
		if (dustSummary < 0)
		{
			dustSummary = 0;
		}
		if (GameManager.Instance.IsSingularity() || num18 > 2 || GameManager.Instance.IsObeliskChallenge() || SandboxManager.Instance.IsEnabled())
		{
			rewardGroup.gameObject.SetActive(value: false);
			this.flag.transform.localPosition = new Vector3(this.flag.transform.localPosition.x, 0f, this.flag.transform.localPosition.z);
		}
		else
		{
			rewardGroup.gameObject.SetActive(value: true);
		}
		rewardsT.gameObject.SetActive(value: true);
		totalTextGold.text = GoldDust(goldSummary, dustSummary);
		int num24 = 1;
		if (GameManager.Instance.IsMultiplayer())
		{
			num24 = NetworkManager.Instance.PlayerList.Length;
		}
		if (num14 > 0)
		{
			num14 = ((!GameManager.Instance.IsObeliskChallenge()) ? (num14 + 80) : Functions.FuncRoundToInt((float)num14 * 0.5f));
		}
		spr0.sprite = array[0].SpritePortrait;
		spr1.sprite = array[1].SpritePortrait;
		spr2.sprite = array[2].SpritePortrait;
		spr3.sprite = array[3].SpritePortrait;
		if (num14 > 0)
		{
			PlayerManager.Instance.ModifyProgress(array[0].SubclassName, num14, 0);
			PlayerManager.Instance.ModifyProgress(array[1].SubclassName, num14, 1);
			PlayerManager.Instance.ModifyProgress(array[2].SubclassName, num14, 2);
			PlayerManager.Instance.ModifyProgress(array[3].SubclassName, num14, 3);
			PlayerManager.Instance.ModifyPlayerRankProgress(num14);
		}
		SaveManager.SavePlayerData();
		if (array[0] != null && PlayerManager.Instance.IsHeroUnlocked(array[0].SubclassName))
		{
			lockedSate[0] = true;
			fp0.SetCharacter(array[0].SourceName, array[0].ClassName, array[0].SubclassName, 0);
		}
		else
		{
			fp0.Hide();
			lockedSate[0] = false;
		}
		if (array[1] != null && PlayerManager.Instance.IsHeroUnlocked(array[1].SubclassName))
		{
			lockedSate[1] = true;
			fp1.SetCharacter(array[1].SourceName, array[1].ClassName, array[1].SubclassName, 1);
		}
		else
		{
			fp1.Hide();
			lockedSate[1] = false;
		}
		if (array[2] != null && PlayerManager.Instance.IsHeroUnlocked(array[2].SubclassName))
		{
			lockedSate[2] = true;
			fp2.SetCharacter(array[2].SourceName, array[2].ClassName, array[2].SubclassName, 2);
		}
		else
		{
			fp2.Hide();
			lockedSate[2] = false;
		}
		if (array[3] != null && PlayerManager.Instance.IsHeroUnlocked(array[3].SubclassName))
		{
			lockedSate[3] = true;
			fp3.SetCharacter(array[3].SourceName, array[3].ClassName, array[3].SubclassName, 3);
		}
		else
		{
			fp3.Hide();
			lockedSate[3] = false;
		}
		PlayerRun playerRun = new PlayerRun();
		playerRun.Id = AtOManager.Instance.GetGameId() + "_" + DateTime.UtcNow.Millisecond + "_" + num15;
		playerRun.Version = GameManager.Instance.gameVersion;
		playerRun.GameUniqueId = AtOManager.Instance.GetGameUniqueId();
		playerRun.GameSeed = AtOManager.Instance.GetGameId();
		playerRun.gameDate = DateTime.Now.ToString();
		playerRun.Singularity = GameManager.Instance.IsSingularity();
		playerRun.ObeliskChallenge = GameManager.Instance.IsObeliskChallenge();
		playerRun.WeeklyChallenge = GameManager.Instance.IsWeeklyChallenge();
		playerRun.WeekChallenge = AtOManager.Instance.GetWeekly();
		playerRun.FinalScore = gameScore;
		playerRun.PlayedTime = AtOManager.Instance.playedTime;
		playerRun.MadnessLevel = num18;
		playerRun.MadnessCorruptors = madnessCorruptors;
		playerRun.ObeliskMadness = AtOManager.Instance.GetObeliskMadness();
		playerRun.SingularityMadness = AtOManager.Instance.GetSingularityMadness();
		playerRun.ActTier = townTier;
		playerRun.TotalPlayers = num24;
		playerRun.PlaceOfDeath = text;
		playerRun.PlacesVisited = num3;
		playerRun.ExperienceGained = num8;
		playerRun.TotalGoldGained = AtOManager.Instance.totalGoldGained;
		playerRun.TotalDustGained = AtOManager.Instance.totalDustGained;
		playerRun.GoldGained = goldSummary;
		playerRun.DustGained = dustSummary;
		playerRun.SandboxEnabled = SandboxManager.Instance.IsEnabled();
		playerRun.SandboxConfig = AtOManager.Instance.GetSandboxMods();
		int length = AtOManager.Instance.combatStats.GetLength(1);
		int[] array2 = new int[length];
		int[] array3 = new int[length];
		int[] array4 = new int[length];
		int[] array5 = new int[length];
		for (int num25 = 0; num25 < length; num25++)
		{
			array2[num25] = AtOManager.Instance.combatStats[0, num25];
			array3[num25] = AtOManager.Instance.combatStats[1, num25];
			array4[num25] = AtOManager.Instance.combatStats[2, num25];
			array5[num25] = AtOManager.Instance.combatStats[3, num25];
		}
		playerRun.CombatStats0 = array2;
		playerRun.CombatStats1 = array3;
		playerRun.CombatStats2 = array4;
		playerRun.CombatStats3 = array5;
		playerRun.MonstersKilled = AtOManager.Instance.monstersKilled;
		playerRun.BossesKilled = bossesKilled;
		playerRun.BossesKilledName = AtOManager.Instance.bossesKilledName;
		playerRun.UnlockedCards = unlockedCards;
		playerRun.VisitedNodes = AtOManager.Instance.mapVisitedNodes;
		playerRun.VisitedNodesAction = AtOManager.Instance.mapVisitedNodesAction;
		playerRun.CommonCorruptions = AtOManager.Instance.corruptionCommonCompleted;
		playerRun.UncommonCorruptions = AtOManager.Instance.corruptionUncommonCompleted;
		playerRun.RareCorruptions = AtOManager.Instance.corruptionRareCompleted;
		playerRun.EpicCorruptions = AtOManager.Instance.corruptionEpicCompleted;
		playerRun.TotalDeaths = num7;
		playerRun.Char0 = array[0].SubclassName;
		playerRun.Char0Skin = array[0].SkinUsed;
		playerRun.Char0Rank = array[0].PerkRank;
		playerRun.Char0Cards = array[0].Cards;
		if (num24 > 1)
		{
			playerRun.Char0Owner = NetworkManager.Instance.GetPlayerNickReal(array[0].Owner);
		}
		playerRun.Char0Items = new List<string>();
		playerRun.Char0Items.Add(array[0].Weapon);
		playerRun.Char0Items.Add(array[0].Armor);
		playerRun.Char0Items.Add(array[0].Jewelry);
		playerRun.Char0Items.Add(array[0].Accesory);
		playerRun.Char0Items.Add(array[0].Pet);
		playerRun.Char0Traits = new List<string>();
		playerRun.Char0Traits.Add(array[0].Traits[0]);
		playerRun.Char0Traits.Add(array[0].Traits[1]);
		playerRun.Char0Traits.Add(array[0].Traits[2]);
		playerRun.Char0Traits.Add(array[0].Traits[3]);
		playerRun.Char0Traits.Add(array[0].Traits[4]);
		if (array[1] != null && array[1].HeroData != null)
		{
			playerRun.Char1 = array[1].SubclassName;
			playerRun.Char1Skin = array[1].SkinUsed;
			playerRun.Char1Rank = array[1].PerkRank;
			playerRun.Char1Cards = array[1].Cards;
			if (num24 > 1)
			{
				playerRun.Char1Owner = NetworkManager.Instance.GetPlayerNickReal(array[1].Owner);
			}
			playerRun.Char1Items = new List<string>();
			playerRun.Char1Items.Add(array[1].Weapon);
			playerRun.Char1Items.Add(array[1].Armor);
			playerRun.Char1Items.Add(array[1].Jewelry);
			playerRun.Char1Items.Add(array[1].Accesory);
			playerRun.Char1Items.Add(array[1].Pet);
			playerRun.Char1Traits = new List<string>();
			playerRun.Char1Traits.Add(array[1].Traits[0]);
			playerRun.Char1Traits.Add(array[1].Traits[1]);
			playerRun.Char1Traits.Add(array[1].Traits[2]);
			playerRun.Char1Traits.Add(array[1].Traits[3]);
			playerRun.Char1Traits.Add(array[1].Traits[4]);
		}
		if (array[2] != null && array[2].HeroData != null)
		{
			playerRun.Char2 = array[2].SubclassName;
			playerRun.Char2Skin = array[2].SkinUsed;
			playerRun.Char2Rank = array[2].PerkRank;
			playerRun.Char2Cards = array[2].Cards;
			if (num24 > 1)
			{
				playerRun.Char2Owner = NetworkManager.Instance.GetPlayerNickReal(array[2].Owner);
			}
			playerRun.Char2Items = new List<string>();
			playerRun.Char2Items.Add(array[2].Weapon);
			playerRun.Char2Items.Add(array[2].Armor);
			playerRun.Char2Items.Add(array[2].Jewelry);
			playerRun.Char2Items.Add(array[2].Accesory);
			playerRun.Char2Items.Add(array[2].Pet);
			playerRun.Char2Traits = new List<string>();
			playerRun.Char2Traits.Add(array[2].Traits[0]);
			playerRun.Char2Traits.Add(array[2].Traits[1]);
			playerRun.Char2Traits.Add(array[2].Traits[2]);
			playerRun.Char2Traits.Add(array[2].Traits[3]);
			playerRun.Char2Traits.Add(array[2].Traits[4]);
		}
		if (array[3] != null && array[3].HeroData != null)
		{
			playerRun.Char3 = array[3].SubclassName;
			playerRun.Char3Skin = array[3].SkinUsed;
			playerRun.Char3Rank = array[3].PerkRank;
			playerRun.Char3Cards = array[3].Cards;
			if (num24 > 1)
			{
				playerRun.Char3Owner = NetworkManager.Instance.GetPlayerNickReal(array[3].Owner);
			}
			playerRun.Char3Items = new List<string>();
			playerRun.Char3Items.Add(array[3].Weapon);
			playerRun.Char3Items.Add(array[3].Armor);
			playerRun.Char3Items.Add(array[3].Jewelry);
			playerRun.Char3Items.Add(array[3].Accesory);
			playerRun.Char3Items.Add(array[3].Pet);
			playerRun.Char3Traits = new List<string>();
			playerRun.Char3Traits.Add(array[3].Traits[0]);
			playerRun.Char3Traits.Add(array[3].Traits[1]);
			playerRun.Char3Traits.Add(array[3].Traits[2]);
			playerRun.Char3Traits.Add(array[3].Traits[3]);
			playerRun.Char3Traits.Add(array[3].Traits[4]);
		}
		SaveManager.SaveRun(playerRun);
	}

	private IEnumerator RemoveDoubleDots()
	{
		yield return Globals.Instance.WaitForSeconds(0.1f);
		for (int i = 0; i < titlesTexts.Count; i++)
		{
			titlesTexts[i].text.Replace(':', ' ').Replace('：', ' ');
		}
	}

	private IEnumerator ProgressCo(FinishProgression _fp, int _increment)
	{
		if (_increment > 0)
		{
			yield return Globals.Instance.WaitForSeconds(0.3f);
			GameManager.Instance.GenerateParticleTrail(0, finalScore.transform.position, _fp.gameObject.transform.position);
		}
		yield return Globals.Instance.WaitForSeconds(0.4f);
		_fp.Increment(_increment);
	}

	public void UnlockState(int index)
	{
		lockedSate[index] = false;
		for (int i = 0; i < 4; i++)
		{
			if (lockedSate[i])
			{
				return;
			}
		}
		if (finishCo != null)
		{
			StopCoroutine(finishCo);
		}
		finishCo = StartCoroutine(FinishThisCo());
	}

	private IEnumerator FinishThisCo()
	{
		yield return Globals.Instance.WaitForSeconds(1f);
		mainMenuButton.Enable();
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false, int absolutePosition = -1)
	{
		_controllerList.Clear();
		_controllerList.Add(mainMenuButton.transform);
		controllerHorizontalIndex = Functions.GetListClosestIndexToMousePosition(_controllerList);
		controllerHorizontalIndex = Functions.GetClosestIndexBasedOnDirection(_controllerList, controllerHorizontalIndex, goingUp, goingRight, goingDown, goingLeft);
		if (_controllerList[controllerHorizontalIndex] != null)
		{
			warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerList[controllerHorizontalIndex].position);
			Mouse.current.WarpCursorPosition(warpPosition);
		}
	}
}
