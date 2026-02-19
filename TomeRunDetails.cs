using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TomeRunDetails : MonoBehaviour
{
	public PlayerRun playerRun;

	private int runIndex;

	private List<string> characterCards;

	public int[,] combatStats;

	public Transform cardListContainer;

	public GameObject cardVerticalPrefab;

	public TomeButton[] tomeButtons;

	public Transform[] characters;

	public Transform[] charactersButtons;

	private SpriteRenderer[] charactersSpr;

	private TMP_Text[] charactersPlayerName;

	public TMP_Text type;

	public TMP_Text typeRight;

	public TMP_Text description;

	public TMP_Text date;

	public TMP_Text score;

	public TMP_Text cards;

	public TMP_Text characterActiveName;

	public Transform closeButton;

	public Transform madness;

	public TMP_Text madnessText;

	public SpriteRenderer characterActiveSpr;

	public ItemCombatIcon iconWeapon;

	public ItemCombatIcon iconArmor;

	public ItemCombatIcon iconAccesory;

	public ItemCombatIcon iconJewelry;

	public ItemCombatIcon iconPet;

	public TraitRollOver[] traits;

	public Transform characterBlock;

	public Transform travelBlock;

	public Transform travelGroups;

	public TMP_Text timePlayed;

	private List<string> characterItems;

	private List<string> characterTraits;

	public GameObject travelPlaceGO;

	public GameObject travelPlaceTitleGO;

	public Transform adventureTitle;

	public Transform pathNext;

	public Transform pathPrev;

	private List<Transform> pathGroupsList;

	public TMP_Text pathPaginator;

	public Transform bossesBlock;

	public TMP_Text bossesDescription;

	public Transform[] bosses;

	private SpriteRenderer[] bossesSpr;

	private TMP_Text[] bossesName;

	private int activeCharacter = -1;

	private int pathIndex;

	private int pathIndexMax;

	private float pathColumnDistance = 3.4f;

	private void Awake()
	{
		if (characters.Length != 0)
		{
			charactersSpr = new SpriteRenderer[characters.Length];
			charactersPlayerName = new TMP_Text[characters.Length];
			for (int i = 0; i < characters.Length; i++)
			{
				charactersSpr[i] = characters[i].GetComponent<SpriteRenderer>();
				charactersPlayerName[i] = characters[i].GetChild(0).GetComponent<TMP_Text>();
			}
		}
		if (bosses.Length != 0)
		{
			bossesSpr = new SpriteRenderer[bosses.Length];
			bossesName = new TMP_Text[bosses.Length];
			for (int j = 0; j < bosses.Length; j++)
			{
				bossesSpr[j] = bosses[j].GetComponent<SpriteRenderer>();
				bossesName[j] = bosses[j].GetChild(0).GetComponent<TMP_Text>();
			}
		}
	}

	public void SetRun(int _index)
	{
		runIndex = _index;
		characterBlock.gameObject.SetActive(value: false);
		if (_index > -1)
		{
			travelBlock.gameObject.SetActive(value: true);
			travelBlock.gameObject.SetActive(value: false);
			characterBlock.gameObject.SetActive(value: true);
			characterBlock.gameObject.SetActive(value: false);
			playerRun = TomeManager.Instance.playerRunsList[_index];
			if (playerRun.CombatStats0 != null)
			{
				TomeManager.Instance.TomeButtons[21].gameObject.SetActive(value: true);
			}
			else
			{
				TomeManager.Instance.TomeButtons[21].gameObject.SetActive(value: false);
			}
			DoPortraits();
			SetPaths();
			SetDescription();
			DoBosses();
		}
	}

	private void DoBosses()
	{
		if (playerRun.BossesKilledName.Count < 1)
		{
			bossesBlock.gameObject.SetActive(value: false);
			return;
		}
		bossesBlock.gameObject.SetActive(value: true);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Texts.Instance.GetText("bossesKilledTome"));
		stringBuilder.Append(" <color=#303030>");
		stringBuilder.Append(playerRun.BossesKilledName.Count);
		stringBuilder.Append("</color>");
		bossesDescription.text = stringBuilder.ToString();
		int num = bosses.Length;
		if (playerRun.BossesKilledName.Count < num)
		{
			num = playerRun.BossesKilledName.Count;
		}
		for (int i = 0; i < num; i++)
		{
			NPCData nPC = Globals.Instance.GetNPC(playerRun.BossesKilledName[i]);
			bosses[i].gameObject.SetActive(value: true);
			bossesSpr[i].sprite = nPC.SpriteSpeed;
			bossesName[i].text = nPC.NPCName;
		}
		for (int j = num; j < bosses.Length; j++)
		{
			bosses[j].gameObject.SetActive(value: false);
		}
	}

	private void SetDescription()
	{
		StringBuilder stringBuilder = new StringBuilder();
		madness.gameObject.SetActive(value: false);
		if (playerRun.TotalPlayers > 1)
		{
			stringBuilder.Append("<size=2><color=#24646D>");
			stringBuilder.Append(Texts.Instance.GetText("menuMultiplayer"));
			stringBuilder.Append("</color></size>\n");
		}
		if (!playerRun.ObeliskChallenge)
		{
			if (!playerRun.Singularity)
			{
				stringBuilder.Append(Texts.Instance.GetText("adventure"));
			}
			else
			{
				stringBuilder.Append(Texts.Instance.GetText("singularity"));
			}
		}
		else if (playerRun.WeeklyChallenge)
		{
			stringBuilder.Append(Texts.Instance.GetText("menuWeekly"));
		}
		else
		{
			stringBuilder.Append(Texts.Instance.GetText("menuChallenge"));
		}
		type.text = stringBuilder.ToString();
		stringBuilder.Clear();
		if (playerRun.WeeklyChallenge)
		{
			stringBuilder.Append(string.Format(Texts.Instance.GetText("weekNumber"), playerRun.WeekChallenge.ToString()));
		}
		else
		{
			stringBuilder.Append("<size=2><color=#24646D>");
			stringBuilder.Append(Texts.Instance.GetText("gameSeed"));
			stringBuilder.Append("</color></size>\n");
			stringBuilder.Append(playerRun.GameSeed);
		}
		typeRight.text = stringBuilder.ToString();
		stringBuilder.Clear();
		int num = 0;
		if (!playerRun.ObeliskChallenge)
		{
			num = (playerRun.Singularity ? playerRun.SingularityMadness : MadnessManager.Instance.CalculateMadnessTotal(playerRun.MadnessLevel, playerRun.MadnessCorruptors));
			if (num > 0)
			{
				madness.gameObject.SetActive(value: true);
				madnessText.text = num.ToString();
			}
		}
		else if (!playerRun.WeeklyChallenge && playerRun.ObeliskMadness > 0)
		{
			num = playerRun.ObeliskMadness;
			madness.gameObject.SetActive(value: true);
			madnessText.text = num.ToString();
		}
		date.text = Convert.ToDateTime(playerRun.gameDate).ToString("d");
		stringBuilder.Append(Functions.ScoreFormat(playerRun.FinalScore));
		stringBuilder.Append("  <voffset=.3><size=-.8><sprite name=experience></size></voffset>");
		score.text = stringBuilder.ToString();
		stringBuilder.Clear();
		int num2 = playerRun.CommonCorruptions + playerRun.UncommonCorruptions + playerRun.RareCorruptions + playerRun.EpicCorruptions;
		stringBuilder.Append(Texts.Instance.GetText("corruptionsCompleted"));
		stringBuilder.Append(" <color=#222>");
		stringBuilder.Append(num2);
		stringBuilder.Append("</color>        ");
		if (num2 > 0)
		{
			string value = "<space=1.1><voffset=.3><size=-.5><color=#333>|</color></size></voffset><space=1.1>";
			stringBuilder.Append("<sprite name=rarityCommon><space=.6>");
			stringBuilder.Append(playerRun.CommonCorruptions);
			stringBuilder.Append(value);
			stringBuilder.Append("<sprite name=rarityUncommon><space=.6>");
			stringBuilder.Append(playerRun.UncommonCorruptions);
			stringBuilder.Append(value);
			stringBuilder.Append("<sprite name=rarityRare><space=.6>");
			stringBuilder.Append(playerRun.RareCorruptions);
			stringBuilder.Append(value);
			stringBuilder.Append("<sprite name=rarityEpic><space=.6>");
			stringBuilder.Append(playerRun.EpicCorruptions);
		}
		stringBuilder.Append("\n");
		stringBuilder.Append(Texts.Instance.GetText("placesVisited"));
		stringBuilder.Append(": <color=#222>");
		stringBuilder.Append(playerRun.VisitedNodes.Count);
		stringBuilder.Append("</color>\n");
		stringBuilder.Append(Texts.Instance.GetText("monstersKilledTome"));
		stringBuilder.Append(" <color=#222>");
		stringBuilder.Append(playerRun.MonstersKilled);
		stringBuilder.Append("</color>\n");
		stringBuilder.Append(Texts.Instance.GetText("heroDeaths"));
		stringBuilder.Append(" <color=#222>");
		stringBuilder.Append(playerRun.TotalDeaths);
		stringBuilder.Append("</color>\n");
		stringBuilder.Append(Texts.Instance.GetText("experienceGainedTome"));
		stringBuilder.Append("  <size=-.2><sprite name=experience></size> <color=#222>");
		stringBuilder.Append(Functions.ScoreFormat(playerRun.ExperienceGained));
		stringBuilder.Append("  <color=#505050><size=-.3>(");
		stringBuilder.Append(Functions.ScoreFormat(Functions.FuncRoundToInt((float)playerRun.ExperienceGained / 4f)));
		stringBuilder.Append("/e)</size></color>");
		stringBuilder.Append("</color>\n");
		stringBuilder.Append(Texts.Instance.GetText("totalResources"));
		stringBuilder.Append("  <color=#222><size=-.2><sprite name=gold></size> ");
		stringBuilder.Append(Functions.ScoreFormat(playerRun.TotalGoldGained));
		stringBuilder.Append("   ");
		stringBuilder.Append("<size=-.2><sprite name=dust></size> ");
		stringBuilder.Append(Functions.ScoreFormat(playerRun.TotalDustGained));
		stringBuilder.Append("</color>\n");
		description.text = stringBuilder.ToString();
	}

	private void SetPaths()
	{
		characterBlock.gameObject.SetActive(value: false);
		travelBlock.gameObject.SetActive(value: true);
		timePlayed.text = string.Format(Texts.Instance.GetText("timePlayed"), Functions.FloatToTime(playerRun.PlayedTime));
		foreach (Transform travelGroup in travelGroups)
		{
			if (travelGroup.gameObject.name == "tGroup")
			{
				UnityEngine.Object.Destroy(travelGroup.gameObject);
			}
		}
		if (playerRun.VisitedNodes.Count == 0 || playerRun.VisitedNodesAction.Count == 0 || playerRun.VisitedNodes.Count != playerRun.VisitedNodesAction.Count)
		{
			tomeButtons[0].gameObject.SetActive(value: false);
			travelBlock.gameObject.SetActive(value: false);
			return;
		}
		tomeButtons[0].gameObject.SetActive(value: true);
		ActivateButtons(0);
		pathGroupsList = new List<Transform>();
		string text = "";
		int num = -1;
		GameObject gameObject = null;
		pathIndex = 0;
		pathIndexMax = 0;
		float num2 = 0f;
		float num3 = 14f;
		float num4 = 0f;
		for (int i = 0; i < playerRun.VisitedNodes.Count; i++)
		{
			string text2 = playerRun.VisitedNodes[i];
			string[] array = playerRun.VisitedNodesAction[i].Split('|');
			if (array.Length < 2 || array[0] != text2)
			{
				continue;
			}
			string text3 = array[1];
			string obeliskIcon = "";
			if (array.Length > 2)
			{
				obeliskIcon = array[2];
			}
			NodeData nodeData = Globals.Instance.GetNodeData(text2);
			string text4 = Texts.Instance.GetText(Globals.Instance.ZoneDataSource[nodeData.NodeZone.ZoneId.ToLower()].ZoneName);
			bool flag = false;
			string[] array2 = text2.Split('_');
			if (playerRun.ObeliskChallenge && (text3 == "destination" || (array2 != null && array2.Length == 2 && array2[1] == "0")))
			{
				text4 = (array2[0].Contains("h") ? Texts.Instance.GetText("upperObelisk") : ((!array2[0].Contains("l")) ? Texts.Instance.GetText("finalObelisk") : Texts.Instance.GetText("lowerObelisk")));
				flag = true;
			}
			if (num2 == 0f || num2 > num3 || (num2 + 2.5f > num3 && text != text4))
			{
				num++;
				num4 = 0f;
				num2 = 0f;
				gameObject = new GameObject();
				gameObject.name = "tGroup";
				gameObject.transform.parent = travelGroups;
				gameObject.transform.localPosition = new Vector3(7.75f + (float)num * pathColumnDistance, 7f, 0f);
				pathGroupsList.Add(gameObject.transform);
			}
			if ((text != text4 && !playerRun.ObeliskChallenge) || flag)
			{
				if (num4 != 0f)
				{
					num4 -= 0.12f;
				}
				GameObject obj = UnityEngine.Object.Instantiate(travelPlaceTitleGO, Vector3.zero, Quaternion.identity, gameObject.transform);
				obj.transform.localPosition = new Vector3(0.6f, num4, 0f);
				obj.transform.GetChild(0).GetComponent<TMP_Text>().text = text4;
				text = text4;
				num2 += 2f;
				num4 -= 0.5f;
			}
			if (num4 == 0f)
			{
				num4 -= 0.09f;
			}
			GameObject obj2 = UnityEngine.Object.Instantiate(travelPlaceGO, Vector3.zero, Quaternion.identity, gameObject.transform);
			obj2.transform.localPosition = new Vector3(-0.2f, num4, 0f);
			obj2.GetComponent<TomeTravelPlace>().SetNode(playerRun.ObeliskChallenge, text2, text3, obeliskIcon);
			num2 += 1f;
			num4 -= 0.48f;
		}
		pathIndexMax = num - 1;
		if (pathIndexMax < 0)
		{
			pathIndexMax = 0;
		}
		SetPathVisibility();
	}

	private void SetPathVisibility()
	{
		if (pathIndex > 0)
		{
			pathPrev.gameObject.SetActive(value: true);
		}
		else
		{
			pathPrev.gameObject.SetActive(value: false);
		}
		if (pathIndex < pathIndexMax)
		{
			pathNext.gameObject.SetActive(value: true);
		}
		else
		{
			pathNext.gameObject.SetActive(value: false);
		}
		for (int i = 0; i < pathGroupsList.Count; i++)
		{
			if (i < pathIndex || i > pathIndex + 1)
			{
				pathGroupsList[i].gameObject.SetActive(value: false);
			}
			else
			{
				pathGroupsList[i].gameObject.SetActive(value: true);
			}
		}
		if (pathIndexMax > 0)
		{
			pathPaginator.text = Functions.FuncRoundToInt((float)pathIndex * 0.5f) + 1 + "/" + (Functions.FuncRoundToInt((float)pathIndexMax * 0.5f) + 1);
		}
		else
		{
			pathPaginator.text = "";
		}
		travelGroups.localPosition = new Vector3((0f - pathColumnDistance) * (float)pathIndex, travelGroups.localPosition.y, travelGroups.localPosition.z);
	}

	public void NextPath()
	{
		if (pathIndex < pathIndexMax)
		{
			pathIndex++;
			pathIndex++;
		}
		SetPathVisibility();
	}

	public void PrevPath()
	{
		if (pathIndex > 0)
		{
			pathIndex--;
			pathIndex--;
		}
		SetPathVisibility();
	}

	public void RunDetailButton(int _index)
	{
		if (_index == 0)
		{
			characterBlock.gameObject.SetActive(value: false);
			travelBlock.gameObject.SetActive(value: true);
			ActivateButtons(0);
		}
		else
		{
			int num = 0;
			DoCharacter(_index switch
			{
				1 => 3, 
				2 => 2, 
				3 => 1, 
				_ => 0, 
			});
		}
	}

	private void DoPortraits()
	{
		SkinData skinData = null;
		string[] source = new string[4] { "warrior", "scout", "mage", "healer" };
		bool flag = false;
		tomeButtons[1].gameObject.SetActive(value: false);
		tomeButtons[2].gameObject.SetActive(value: false);
		tomeButtons[3].gameObject.SetActive(value: false);
		tomeButtons[4].gameObject.SetActive(value: false);
		if (playerRun.Char0Skin != "")
		{
			skinData = Globals.Instance.GetSkinData(playerRun.Char0Skin);
			if (skinData != null)
			{
				charactersSpr[0].sprite = skinData.SpritePortraitGrande;
				flag = true;
			}
		}
		if (!flag)
		{
			if (source.Contains(playerRun.Char0))
			{
				playerRun.Char0 = Functions.GetClassFromCards(playerRun.Char0Cards);
			}
			if (playerRun.Char0 != "")
			{
				skinData = Globals.Instance.GetSkinData(playerRun.Char0);
				if (skinData != null)
				{
					charactersSpr[0].sprite = skinData.SpritePortraitGrande;
				}
				else
				{
					charactersSpr[0].sprite = null;
				}
			}
		}
		charactersPlayerName[0].text = playerRun.Char0Owner;
		SetCharacterButton(skinData, 4);
		flag = false;
		if (playerRun.Char1Skin != "")
		{
			skinData = Globals.Instance.GetSkinData(playerRun.Char1Skin);
			if (skinData != null)
			{
				charactersSpr[1].sprite = skinData.SpritePortraitGrande;
				flag = true;
			}
		}
		if (!flag)
		{
			if (source.Contains(playerRun.Char1))
			{
				playerRun.Char1 = Functions.GetClassFromCards(playerRun.Char1Cards);
			}
			if (playerRun.Char1 != "")
			{
				skinData = Globals.Instance.GetSkinData(playerRun.Char1);
				if (skinData != null)
				{
					charactersSpr[1].sprite = skinData.SpritePortraitGrande;
				}
				else
				{
					charactersSpr[1].sprite = null;
				}
			}
		}
		charactersPlayerName[1].text = playerRun.Char1Owner;
		SetCharacterButton(skinData, 3);
		flag = false;
		if (playerRun.Char2Skin != "")
		{
			skinData = Globals.Instance.GetSkinData(playerRun.Char2Skin);
			if (skinData != null)
			{
				charactersSpr[2].sprite = skinData.SpritePortraitGrande;
				flag = true;
			}
		}
		if (!flag)
		{
			if (source.Contains(playerRun.Char2))
			{
				playerRun.Char2 = Functions.GetClassFromCards(playerRun.Char2Cards);
			}
			if (playerRun.Char2 != "")
			{
				skinData = Globals.Instance.GetSkinData(playerRun.Char2);
				if (skinData != null)
				{
					charactersSpr[2].sprite = skinData.SpritePortraitGrande;
				}
				else
				{
					charactersSpr[2].sprite = null;
				}
			}
		}
		charactersPlayerName[2].text = playerRun.Char2Owner;
		SetCharacterButton(skinData, 2);
		flag = false;
		if (playerRun.Char3Skin != "")
		{
			skinData = Globals.Instance.GetSkinData(playerRun.Char3Skin);
			if (skinData != null)
			{
				charactersSpr[3].sprite = skinData.SpritePortraitGrande;
				flag = true;
			}
		}
		if (!flag)
		{
			if (source.Contains(playerRun.Char3))
			{
				playerRun.Char3 = Functions.GetClassFromCards(playerRun.Char3Cards);
			}
			if (playerRun.Char3 != "")
			{
				skinData = Globals.Instance.GetSkinData(playerRun.Char3);
				if (skinData != null)
				{
					charactersSpr[3].sprite = skinData.SpritePortraitGrande;
				}
				else
				{
					charactersSpr[3].sprite = null;
				}
			}
		}
		charactersPlayerName[3].text = playerRun.Char3Owner;
		SetCharacterButton(skinData, 1);
		if (playerRun.Char0 == "")
		{
			characters[0].gameObject.SetActive(value: false);
			charactersButtons[0].gameObject.SetActive(value: false);
			tomeButtons[4].gameObject.SetActive(value: false);
		}
		else
		{
			characters[0].gameObject.SetActive(value: true);
			charactersButtons[0].gameObject.SetActive(value: true);
			tomeButtons[4].gameObject.SetActive(value: true);
		}
		if (playerRun.Char1 == "")
		{
			characters[1].gameObject.SetActive(value: false);
			charactersButtons[1].gameObject.SetActive(value: false);
			tomeButtons[3].gameObject.SetActive(value: false);
		}
		else
		{
			characters[1].gameObject.SetActive(value: true);
			charactersButtons[1].gameObject.SetActive(value: true);
			tomeButtons[3].gameObject.SetActive(value: true);
		}
		if (playerRun.Char2 == "")
		{
			characters[2].gameObject.SetActive(value: false);
			charactersButtons[2].gameObject.SetActive(value: false);
			tomeButtons[2].gameObject.SetActive(value: false);
		}
		else
		{
			characters[2].gameObject.SetActive(value: true);
			charactersButtons[2].gameObject.SetActive(value: true);
			tomeButtons[2].gameObject.SetActive(value: true);
		}
		if (playerRun.Char3 == "")
		{
			characters[3].gameObject.SetActive(value: false);
			charactersButtons[3].gameObject.SetActive(value: false);
			tomeButtons[1].gameObject.SetActive(value: false);
		}
		else
		{
			characters[3].gameObject.SetActive(value: true);
			charactersButtons[3].gameObject.SetActive(value: true);
			tomeButtons[1].gameObject.SetActive(value: true);
		}
	}

	private void SetCharacterButton(SkinData skd, int _index)
	{
		if (skd != null)
		{
			SubClassData subClassData = Globals.Instance.GetSubClassData(skd.SkinSubclass.Id);
			if (subClassData != null)
			{
				tomeButtons[_index].gameObject.SetActive(value: true);
				tomeButtons[_index].SetText(subClassData.CharacterName);
				tomeButtons[_index].SetColor(Globals.Instance.ClassColor[subClassData.HeroClass.ToString().ToLower()]);
			}
		}
	}

	private void ActivateButtons(int _index)
	{
		tomeButtons[_index].Activate();
		for (int i = 0; i < 5; i++)
		{
			if (i != _index)
			{
				tomeButtons[i].Deactivate();
			}
			tomeButtons[i].transform.localPosition = new Vector3(tomeButtons[i].transform.localPosition.x, tomeButtons[i].transform.localPosition.y, -3f);
		}
	}

	public void DoCharacter(int _index)
	{
		characterBlock.gameObject.SetActive(value: true);
		travelBlock.gameObject.SetActive(value: false);
		activeCharacter = _index;
		characterItems = null;
		int num = 1;
		SubClassData subClassData = null;
		if (activeCharacter == 0)
		{
			characterCards = playerRun.Char0Cards;
			if (playerRun.Char0Items != null)
			{
				characterItems = playerRun.Char0Items;
			}
			if (playerRun.Char0Traits != null)
			{
				characterTraits = playerRun.Char0Traits;
			}
			Globals.Instance.GetSkinData(playerRun.Char0Skin);
			num = playerRun.Char0Rank;
			subClassData = Globals.Instance.GetSubClassData(playerRun.Char0);
			ActivateButtons(4);
		}
		else if (activeCharacter == 1)
		{
			characterCards = playerRun.Char1Cards;
			if (playerRun.Char1Items != null)
			{
				characterItems = playerRun.Char1Items;
			}
			if (playerRun.Char1Traits != null)
			{
				characterTraits = playerRun.Char1Traits;
			}
			Globals.Instance.GetSkinData(playerRun.Char1Skin);
			num = playerRun.Char1Rank;
			subClassData = Globals.Instance.GetSubClassData(playerRun.Char1);
			ActivateButtons(3);
		}
		else if (activeCharacter == 2)
		{
			characterCards = playerRun.Char2Cards;
			if (playerRun.Char2Items != null)
			{
				characterItems = playerRun.Char2Items;
			}
			if (playerRun.Char2Traits != null)
			{
				characterTraits = playerRun.Char2Traits;
			}
			Globals.Instance.GetSkinData(playerRun.Char2Skin);
			num = playerRun.Char2Rank;
			subClassData = Globals.Instance.GetSubClassData(playerRun.Char2);
			ActivateButtons(2);
		}
		else if (activeCharacter == 3)
		{
			characterCards = playerRun.Char3Cards;
			if (playerRun.Char3Items != null)
			{
				characterItems = playerRun.Char3Items;
			}
			if (playerRun.Char3Traits != null)
			{
				characterTraits = playerRun.Char3Traits;
			}
			Globals.Instance.GetSkinData(playerRun.Char3Skin);
			num = playerRun.Char3Rank;
			subClassData = Globals.Instance.GetSubClassData(playerRun.Char3);
			ActivateButtons(1);
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Texts.Instance.GetText("cards"));
		stringBuilder.Append(" <size=-.3>(");
		stringBuilder.Append(characterCards.Count);
		stringBuilder.Append(")</size>");
		cards.text = stringBuilder.ToString();
		characterActiveSpr.sprite = charactersSpr[activeCharacter].sprite;
		if (subClassData != null)
		{
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append(subClassData.CharacterName);
			stringBuilder2.Append("<br><size=2.6><color=#303030>");
			if (!playerRun.ObeliskChallenge)
			{
				stringBuilder2.Append(string.Format(Texts.Instance.GetText("rankProgress"), num));
			}
			characterActiveName.text = stringBuilder2.ToString();
		}
		else
		{
			characterActiveName.text = "";
		}
		SetCards();
		SetItems();
		SetTraits();
		characterBlock.gameObject.SetActive(value: true);
	}

	private void SetItems()
	{
		if (characterItems != null && characterItems.Count >= 1 && characterItems[0] != null)
		{
			iconWeapon.gameObject.SetActive(value: true);
			iconWeapon.ShowIcon("weapon", characterItems[0], fromTome: true);
		}
		else
		{
			iconWeapon.gameObject.SetActive(value: false);
		}
		if (characterItems != null && characterItems.Count >= 2 && characterItems[1] != null)
		{
			iconArmor.gameObject.SetActive(value: true);
			iconArmor.ShowIcon("armor", characterItems[1], fromTome: true);
		}
		else
		{
			iconArmor.gameObject.SetActive(value: false);
		}
		if (characterItems != null && characterItems.Count >= 3 && characterItems[2] != null)
		{
			iconJewelry.gameObject.SetActive(value: true);
			iconJewelry.ShowIcon("jewelry", characterItems[2], fromTome: true);
		}
		else
		{
			iconJewelry.gameObject.SetActive(value: false);
		}
		if (characterItems != null && characterItems.Count >= 4 && characterItems[3] != null)
		{
			iconAccesory.gameObject.SetActive(value: true);
			iconAccesory.ShowIcon("accesory", characterItems[3], fromTome: true);
		}
		else
		{
			iconAccesory.gameObject.SetActive(value: false);
		}
		if (characterItems != null && characterItems.Count >= 5 && characterItems[4] != null)
		{
			iconPet.gameObject.SetActive(value: true);
			iconPet.ShowIcon("pet", characterItems[4], fromTome: true);
		}
		else
		{
			iconPet.gameObject.SetActive(value: false);
		}
	}

	private void SetCards()
	{
		ClearCardListContainer();
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		int count = characterCards.Count;
		SortedList sortedList = new SortedList();
		for (int i = 0; i < count; i++)
		{
			sortedList.Add(Globals.Instance.GetCardData(characterCards[i], instantiate: false).CardName + characterCards[i] + i, characterCards[i] + "_" + i);
		}
		GameObject[] array = new GameObject[count];
		for (int j = 0; j < count; j++)
		{
			CardData cardData = Globals.Instance.GetCardData(sortedList.GetByIndex(j).ToString().Split('_')[0], instantiate: false);
			int num = 0;
			dictionary.Add(value: (cardData.CardClass != Enums.CardClass.Injury) ? ((cardData.CardClass != Enums.CardClass.Boon) ? cardData.EnergyCost : (-2)) : (-1), key: cardData.Id + "_" + sortedList.GetByIndex(j).ToString().Split('_')[1]);
		}
		dictionary = dictionary.OrderBy((KeyValuePair<string, int> x) => x.Value).ToDictionary((KeyValuePair<string, int> x) => x.Key, (KeyValuePair<string, int> x) => x.Value);
		CardVertical[] array2 = new CardVertical[dictionary.Count * 2];
		for (int num2 = 0; num2 < dictionary.Count; num2++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(cardVerticalPrefab, new Vector3(0f, 0f, -1f), Quaternion.identity, cardListContainer);
			gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, 0f);
			string key = dictionary.ElementAt(num2).Key;
			int num3 = int.Parse(key.Split('_')[1]);
			gameObject.name = "deckcard_" + num3;
			array[num3] = gameObject;
			array2[num3] = gameObject.GetComponent<CardVertical>();
			array2[num3].SetCard(key);
		}
		cardListContainer.GetComponent<GridLayoutGroup>().enabled = false;
		cardListContainer.GetComponent<GridLayoutGroup>().enabled = true;
	}

	private void SetTraits()
	{
		for (int i = 0; i < 5; i++)
		{
			if (characterTraits != null && characterTraits.Count > i && characterTraits[i] != "")
			{
				traits[i].gameObject.SetActive(value: true);
				traits[i].SetTrait(characterTraits[i]);
			}
			else
			{
				traits[i].gameObject.SetActive(value: false);
			}
		}
	}

	private void ClearCardListContainer()
	{
		foreach (Transform item in cardListContainer)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
	}
}
