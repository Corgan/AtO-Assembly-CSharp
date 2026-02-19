using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CorruptionManager : MonoBehaviour
{
	public SpriteRenderer[] monsterSprite;

	public Transform monsterSpriteFrontChampion;

	public Transform monsterSpriteBackChampion;

	public Transform boxBorder;

	public Transform bgHand;

	public Transform bgCards;

	public Transform bgCorruptionOn;

	public TMP_Text textReward;

	public TMP_Text textAcceptScore;

	public TMP_Text textDifficulty;

	public Transform corruptionButton;

	public Transform corruptionContinue;

	public Transform corruptionOnlyMaster;

	private GameObject cardCorruption;

	private GameObject cardReward;

	private string corruptionIdCard;

	private CardData cDataCorruption;

	public Transform corruptionBoxX;

	private bool clicked;

	private List<string> basicCorruptions = new List<string>();

	private List<string> advancedCorruptions = new List<string>();

	private Hero[] teamAtO;

	public BotonGeneric botonGenericX;

	public BotonGeneric rewardBotA;

	public BotonGeneric rewardBotB;

	private int corruptionRewardType = -1;

	private string corruptionRewardId = "";

	private string corruptionRewardIdB = "";

	private int corruptionRewardChar = -1;

	private string corruptionRewardCard = "";

	private Node _nodeSelected;

	private int randomCorruptionIndex = -1;

	private string nodeSelectedAssignedId = "";

	private string nodeSelectedDataId = "";

	private int mpReadyRetryIndex;

	private PhotonView photonView;

	private Coroutine coroutineReward;

	public PopupText corruptionIconPopup;

	public Transform[] corruptionIcon;

	public Transform elements;

	public TMP_Text buttonShowText;

	private bool showStatus;

	private List<Transform> controllerList = new List<Transform>();

	private int controllerHorizontalIndex = -1;

	private Vector2 warpPosition;

	private void Awake()
	{
		if (MapManager.Instance != null)
		{
			photonView = MapManager.Instance.GetPhotonView();
		}
		basicCorruptions = new List<string>();
		basicCorruptions.Add("goldshards0");
		basicCorruptions.Add("freecardremove");
		basicCorruptions.Add("rareshop");
		basicCorruptions.Add("altarupgrade");
		basicCorruptions.Add("heal20");
		basicCorruptions.Add("herocard");
		advancedCorruptions = new List<string>();
		advancedCorruptions.Add("goldshards1");
		advancedCorruptions.Add("freecardupgrade");
		advancedCorruptions.Add("freecardremove2");
		advancedCorruptions.Add("exoticshop");
		advancedCorruptions.Add("increasedqualityofcardrewards");
		advancedCorruptions.Add("herocard");
	}

	public void ShowHide()
	{
		if (showStatus)
		{
			elements.gameObject.SetActive(value: false);
			buttonShowText.text = Texts.Instance.GetText("show").ToUpper();
		}
		else
		{
			elements.gameObject.SetActive(value: true);
			buttonShowText.text = Texts.Instance.GetText("hide").ToUpper();
		}
		showStatus = !showStatus;
	}

	public void InitCorruption(Node _node, bool next = false)
	{
		if (_node == null)
		{
			return;
		}
		base.gameObject.SetActive(value: true);
		showStatus = false;
		ShowHide();
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, -100f);
		int deterministicHashCode = (_node.nodeData.NodeId + AtOManager.Instance.GetGameId()).GetDeterministicHashCode();
		UnityEngine.Random.InitState(deterministicHashCode);
		_nodeSelected = _node;
		nodeSelectedAssignedId = _node.GetNodeAssignedId();
		nodeSelectedDataId = _node.nodeData.NodeId;
		List<string> list = new List<string>();
		bool flag = false;
		if (GameManager.Instance.IsWeeklyChallenge())
		{
			ChallengeData weeklyData = Globals.Instance.GetWeeklyData(AtOManager.Instance.GetWeekly());
			if (weeklyData.CorruptionList != null && weeklyData.CorruptionList.Count > 0)
			{
				for (int i = 0; i < weeklyData.CorruptionList.Count; i++)
				{
					if (weeklyData.CorruptionList[i] != null)
					{
						list.Add(weeklyData.CorruptionList[i].Id);
					}
				}
				flag = true;
			}
		}
		if (!flag || list.Count == 0)
		{
			CardData cardData = null;
			for (int j = 0; j < Globals.Instance.CardListByType[Enums.CardType.Corruption].Count; j++)
			{
				cardData = Globals.Instance.GetCardData(Globals.Instance.CardListByType[Enums.CardType.Corruption][j], instantiate: false);
				if (cardData != null && !cardData.OnlyInWeekly)
				{
					list.Add(Globals.Instance.CardListByType[Enums.CardType.Corruption][j]);
				}
			}
		}
		int num = -1;
		bool flag2 = false;
		while (!flag2)
		{
			if (!next)
			{
				num = UnityEngine.Random.Range(0, list.Count);
			}
			else
			{
				num = randomCorruptionIndex + 1;
				if (num >= list.Count)
				{
					num = 0;
				}
			}
			randomCorruptionIndex = num;
			corruptionIdCard = list[num];
			if (corruptionIdCard == "resurrection" || corruptionIdCard == "resurrectiona" || corruptionIdCard == "resurrectionb" || corruptionIdCard == "resurrectionrare")
			{
				continue;
			}
			for (int k = 0; k < deterministicHashCode % 10; k++)
			{
				UnityEngine.Random.Range(0, 100);
			}
			cDataCorruption = Globals.Instance.GetCardData(corruptionIdCard, instantiate: false);
			if (!(cDataCorruption == null) && (GameManager.Instance.IsWeeklyChallenge() || !cDataCorruption.OnlyInWeekly))
			{
				flag2 = true;
				if (!next && PlayerManager.Instance.MonstersKilled < 30 && cDataCorruption.CardRarity != Enums.CardRarity.Common && cDataCorruption.CardRarity != Enums.CardRarity.Uncommon)
				{
					flag2 = false;
				}
			}
		}
		if (AtOManager.Instance.GetTownTier() == 0)
		{
			cDataCorruption = Functions.GetCardDataFromCardData(cDataCorruption, "");
			if (cDataCorruption != null)
			{
				corruptionIdCard = cDataCorruption.Id;
			}
		}
		else if (AtOManager.Instance.GetTownTier() == 1)
		{
			cDataCorruption = Functions.GetCardDataFromCardData(cDataCorruption, "A");
			if (cDataCorruption != null)
			{
				corruptionIdCard = cDataCorruption.Id;
			}
		}
		else if (AtOManager.Instance.GetTownTier() == 2)
		{
			if (cDataCorruption != null)
			{
				cDataCorruption = Functions.GetCardDataFromCardData(cDataCorruption, "B");
			}
			corruptionIdCard = cDataCorruption.Id;
		}
		else if (AtOManager.Instance.GetTownTier() > 2)
		{
			if (cDataCorruption != null)
			{
				cDataCorruption = Functions.GetCardDataFromCardData(cDataCorruption, "RARE");
			}
			corruptionIdCard = cDataCorruption.Id;
		}
		if (cDataCorruption == null)
		{
			cDataCorruption = Globals.Instance.GetCardData(corruptionIdCard, instantiate: false);
		}
		if (cDataCorruption.CardRarity == Enums.CardRarity.Common || cDataCorruption.CardRarity == Enums.CardRarity.Uncommon)
		{
			corruptionRewardType = 0;
		}
		else
		{
			corruptionRewardType = 1;
		}
		bool flag3 = false;
		if (corruptionRewardType == 0)
		{
			while (!flag3)
			{
				corruptionRewardId = (corruptionRewardIdB = basicCorruptions[UnityEngine.Random.Range(0, basicCorruptions.Count)]);
				while (corruptionRewardIdB == corruptionRewardId)
				{
					corruptionRewardIdB = basicCorruptions[UnityEngine.Random.Range(0, basicCorruptions.Count)];
				}
				flag3 = true;
				if (GameManager.Instance.IsObeliskChallenge() && (corruptionRewardId == "freecardremove" || corruptionRewardId == "rareshop" || corruptionRewardId == "randomcardupgrade" || corruptionRewardId == "altarupgrade" || corruptionRewardId == "freecardupgrade" || corruptionRewardId == "freecardremove2" || corruptionRewardId == "exoticshop" || corruptionRewardIdB == "freecardremove" || corruptionRewardIdB == "rareshop" || corruptionRewardIdB == "randomcardupgrade" || corruptionRewardIdB == "altarupgrade" || corruptionRewardIdB == "freecardupgrade" || corruptionRewardIdB == "freecardremove2" || corruptionRewardIdB == "exoticshop"))
				{
					flag3 = false;
				}
			}
		}
		else
		{
			while (!flag3)
			{
				corruptionRewardId = (corruptionRewardIdB = advancedCorruptions[UnityEngine.Random.Range(0, advancedCorruptions.Count)]);
				while (corruptionRewardIdB == corruptionRewardId)
				{
					corruptionRewardIdB = advancedCorruptions[UnityEngine.Random.Range(0, advancedCorruptions.Count)];
				}
				flag3 = true;
				if (GameManager.Instance.IsObeliskChallenge() && (corruptionRewardId == "freecardremove" || corruptionRewardId == "rareshop" || corruptionRewardId == "randomcardupgrade" || corruptionRewardId == "altarupgrade" || corruptionRewardId == "freecardupgrade" || corruptionRewardId == "freecardremove2" || corruptionRewardIdB == "freecardremove" || corruptionRewardIdB == "rareshop" || corruptionRewardIdB == "randomcardupgrade" || corruptionRewardIdB == "altarupgrade" || corruptionRewardIdB == "freecardupgrade" || corruptionRewardIdB == "freecardremove2"))
				{
					flag3 = false;
				}
			}
		}
		CorruptionPackData corruptionPackData = null;
		teamAtO = AtOManager.Instance.GetTeam();
		bool flag4 = false;
		while (!flag4)
		{
			corruptionRewardChar = UnityEngine.Random.Range(0, 4);
			if (teamAtO[corruptionRewardChar] != null && teamAtO[corruptionRewardChar].HeroData != null)
			{
				flag4 = true;
			}
		}
		Enums.CardClass cardClass = (Enums.CardClass)Enum.Parse(typeof(Enums.CardClass), Enum.GetName(typeof(Enums.HeroClass), teamAtO[corruptionRewardChar].HeroData.HeroClass));
		foreach (KeyValuePair<string, CorruptionPackData> item in Globals.Instance.CorruptionPackDataSource)
		{
			corruptionPackData = item.Value;
			if (corruptionPackData.PackClass == cardClass && corruptionPackData.PackTier == AtOManager.Instance.GetTownTier())
			{
				break;
			}
		}
		if (corruptionPackData != null)
		{
			if (corruptionRewardType == 0)
			{
				corruptionRewardCard = corruptionPackData.LowPack[UnityEngine.Random.Range(0, corruptionPackData.LowPack.Count)].Id;
			}
			else
			{
				corruptionRewardCard = corruptionPackData.HighPack[UnityEngine.Random.Range(0, corruptionPackData.HighPack.Count)].Id;
			}
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			string text = corruptionRewardId;
			string text2 = corruptionRewardIdB;
			string text3 = corruptionIdCard;
			int num2 = corruptionRewardChar;
			string text4 = corruptionRewardCard;
			string text5 = nodeSelectedAssignedId;
			string text6 = nodeSelectedDataId;
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("Send corruption NET data");
			}
			photonView.RPC("NET_ShareCorruption", RpcTarget.Others, text, text2, text3, num2, text4, text5, text6);
		}
		StartCoroutine(DrawCorruptionCo());
	}

	public void DrawCorruptionFromNet(string _corruptionRewardId, string _corruptionRewardIdB, string _corruptionIdCard, int _corruptionRewardChar, string _corruptionRewardCard, string _nodeSelectedAssignedId, string _nodeSelectedDataId)
	{
		corruptionRewardId = _corruptionRewardId;
		corruptionRewardIdB = _corruptionRewardIdB;
		corruptionIdCard = _corruptionIdCard;
		corruptionRewardChar = _corruptionRewardChar;
		corruptionRewardCard = _corruptionRewardCard;
		nodeSelectedAssignedId = _nodeSelectedAssignedId;
		nodeSelectedDataId = _nodeSelectedDataId;
		teamAtO = AtOManager.Instance.GetTeam();
		DrawCorruption();
	}

	public void NextCorruption()
	{
		InitCorruption(_nodeSelected, next: true);
	}

	private void DrawCorruption()
	{
		base.gameObject.SetActive(value: true);
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, -100f);
		StartCoroutine(DrawCorruptionCo());
	}

	private IEnumerator DrawCorruptionCo()
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			if (NetworkManager.Instance.IsMaster())
			{
				while (!NetworkManager.Instance.AllPlayersReady("drawCorruption"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked");
				}
				NetworkManager.Instance.PlayersNetworkContinue("drawCorruption");
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("drawCorruption", status: true);
				NetworkManager.Instance.SetStatusReady("drawCorruption");
				mpReadyRetryIndex = 0;
				while (NetworkManager.Instance.WaitingSyncro["drawCorruption"])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
					mpReadyRetryIndex++;
					if (mpReadyRetryIndex > 10)
					{
						NetworkManager.Instance.SetStatusReady("drawCorruption");
						mpReadyRetryIndex = 0;
					}
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("drawCorruption, we can continue!");
				}
			}
		}
		corruptionOnlyMaster.gameObject.SetActive(value: false);
		corruptionButton.gameObject.SetActive(value: true);
		if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
		{
			corruptionOnlyMaster.gameObject.SetActive(value: true);
			corruptionButton.gameObject.SetActive(value: false);
		}
		cDataCorruption = Globals.Instance.GetCardData(corruptionIdCard, instantiate: false);
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, 0f);
		UnityEngine.Object.Destroy(cardCorruption);
		cardCorruption = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, elements);
		cardCorruption.transform.localPosition = new Vector3(-3.35f, 0.04f, -2f);
		cardCorruption.transform.localScale = new Vector3(1.52f, 1.52f, 1f);
		UnityEngine.Object.Destroy(cardReward);
		cardReward = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, elements);
		cardReward.transform.localPosition = new Vector3(6.65f, -1.35f, -2f);
		cardReward.transform.localScale = new Vector3(1.35f, 1.35f, 1f);
		cardReward.gameObject.SetActive(value: false);
		CombatData combatData = Globals.Instance.GetCombatData(nodeSelectedAssignedId);
		bool flag = false;
		if (MadnessManager.Instance.IsMadnessTraitActive("randomcombats") || GameManager.Instance.IsObeliskChallenge() || AtOManager.Instance.IsChallengeTraitActive("randomcombats"))
		{
			flag = true;
		}
		monsterSpriteFrontChampion.gameObject.SetActive(value: false);
		monsterSpriteBackChampion.gameObject.SetActive(value: false);
		NodeData nodeData = Globals.Instance.GetNodeData(nodeSelectedDataId);
		if (flag && nodeData != null && nodeData.NodeCombatTier != Enums.CombatTier.T0)
		{
			string text = "";
			if (combatData != null)
			{
				text = combatData.CombatId;
			}
			int deterministicHashCode = (nodeSelectedDataId + AtOManager.Instance.GetGameId() + text).GetDeterministicHashCode();
			NPCData[] randomCombat = Functions.GetRandomCombat(nodeData.NodeCombatTier, deterministicHashCode, nodeSelectedDataId);
			if (randomCombat != null)
			{
				int num = 0;
				for (int i = 0; i < randomCombat.Length; i++)
				{
					if (randomCombat[i] != null)
					{
						monsterSprite[i].gameObject.SetActive(value: true);
						monsterSprite[i].sprite = randomCombat[i].SpriteSpeed;
						num++;
					}
					else
					{
						monsterSprite[i].gameObject.SetActive(value: false);
					}
				}
				if (randomCombat[0] != null && randomCombat[0].IsNamed)
				{
					string auraCurseImmune = Functions.GetAuraCurseImmune(randomCombat[0], nodeSelectedDataId);
					AuraCurseData auraCurseData = Globals.Instance.GetAuraCurseData(auraCurseImmune);
					if (auraCurseData != null)
					{
						monsterSpriteFrontChampion.gameObject.SetActive(value: true);
						monsterSpriteFrontChampion.GetComponent<SpriteRenderer>().sprite = auraCurseData.Sprite;
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.Append("<size=+2>");
						stringBuilder.Append(Texts.Instance.GetText("championMonster"));
						stringBuilder.Append("</size><br><color=#F3404E>");
						stringBuilder.Append(Texts.Instance.GetText("immune"));
						stringBuilder.Append("  <voffset=-2><size=+5><sprite name=");
						stringBuilder.Append(auraCurseImmune);
						stringBuilder.Append(">");
						monsterSpriteFrontChampion.GetComponent<PopupText>().text = stringBuilder.ToString();
					}
				}
				if (randomCombat[3] != null && randomCombat[3].IsNamed)
				{
					string auraCurseImmune2 = Functions.GetAuraCurseImmune(randomCombat[3], nodeSelectedDataId);
					AuraCurseData auraCurseData2 = Globals.Instance.GetAuraCurseData(auraCurseImmune2);
					if (auraCurseData2 != null)
					{
						monsterSpriteBackChampion.gameObject.SetActive(value: true);
						monsterSpriteBackChampion.GetComponent<SpriteRenderer>().sprite = auraCurseData2.Sprite;
						StringBuilder stringBuilder2 = new StringBuilder();
						stringBuilder2.Append("<size=+2>");
						stringBuilder2.Append(Texts.Instance.GetText("championMonster"));
						stringBuilder2.Append("</size><br><color=#F3404E>");
						stringBuilder2.Append(Texts.Instance.GetText("immune"));
						stringBuilder2.Append("  <voffset=-2><size=+5><sprite name=");
						stringBuilder2.Append(auraCurseImmune2);
						stringBuilder2.Append(">");
						monsterSpriteBackChampion.GetComponent<PopupText>().text = stringBuilder2.ToString();
					}
				}
				if (AtOManager.Instance.Sandbox_lessNPCs != 0)
				{
					SortedDictionary<int, int> sortedDictionary = new SortedDictionary<int, int>();
					for (int j = 0; j < randomCombat.Length; j++)
					{
						if (randomCombat[j] != null && !randomCombat[j].IsNamed && !randomCombat[j].IsBoss)
						{
							sortedDictionary.Add(randomCombat[j].Hp * 10000 + j, j);
						}
					}
					int num2 = AtOManager.Instance.Sandbox_lessNPCs;
					if (num2 >= num)
					{
						num2 = num - 1;
					}
					if (num2 > sortedDictionary.Count)
					{
						num2 = sortedDictionary.Count;
					}
					for (int k = 0; k < num2; k++)
					{
						monsterSprite[sortedDictionary.ElementAt(k).Value].gameObject.SetActive(value: false);
					}
				}
			}
		}
		else
		{
			int num3 = 0;
			for (int l = 0; l < combatData.NPCList.Length; l++)
			{
				if (((GameManager.Instance.IsGameAdventure() && AtOManager.Instance.GetMadnessDifficulty() == 0) || (GameManager.Instance.IsSingularity() && AtOManager.Instance.GetSingularityMadness() == 0)) && combatData.NpcRemoveInMadness0Index == l && AtOManager.Instance.GetActNumberForText() < 3)
				{
					monsterSprite[l].gameObject.SetActive(value: false);
				}
				else if (combatData.NPCList[l] != null)
				{
					monsterSprite[l].gameObject.SetActive(value: true);
					monsterSprite[l].sprite = combatData.NPCList[l].SpriteSpeed;
					num3++;
				}
				else
				{
					monsterSprite[l].gameObject.SetActive(value: false);
				}
			}
			if (AtOManager.Instance.Sandbox_lessNPCs != 0)
			{
				SortedDictionary<int, int> sortedDictionary2 = new SortedDictionary<int, int>();
				for (int m = 0; m < combatData.NPCList.Length; m++)
				{
					if (combatData.NPCList[m] != null && !combatData.NPCList[m].IsNamed && !combatData.NPCList[m].IsBoss)
					{
						sortedDictionary2.Add(combatData.NPCList[m].Hp * 10000 + m, m);
					}
				}
				int num4 = AtOManager.Instance.Sandbox_lessNPCs;
				if (num4 >= num3)
				{
					num4 = num3 - 1;
				}
				if (num4 > sortedDictionary2.Count)
				{
					num4 = sortedDictionary2.Count;
				}
				for (int n = 0; n < num4; n++)
				{
					monsterSprite[sortedDictionary2.ElementAt(n).Value].gameObject.SetActive(value: false);
				}
			}
		}
		CardItem component = cardCorruption.GetComponent<CardItem>();
		component.SetCard(corruptionIdCard, deckScale: false);
		component.TopLayeringOrder("Book", 1000);
		component.cardmakebig = true;
		component.CreateColliderAdjusted();
		component.DrawBorder("purple");
		component.cardmakebigSize = 1.52f;
		component.cardmakebigSizeMax = 1.62f;
		CardItem component2 = cardReward.GetComponent<CardItem>();
		component2.SetCard(corruptionRewardCard, deckScale: false);
		component2.TopLayeringOrder("Book", 1000);
		component2.cardmakebig = true;
		component2.CreateColliderAdjusted();
		component2.cardmakebigSize = 1.35f;
		component2.cardmakebigSizeMax = 1.45f;
		StringBuilder stringBuilder3 = new StringBuilder();
		stringBuilder3.Append("<size=+5>");
		string text2;
		int num5;
		if (cDataCorruption.CardRarity == Enums.CardRarity.Common)
		{
			text2 = string.Format(Texts.Instance.GetText("corruptionRewardScore"), "40");
			textDifficulty.text = Texts.Instance.GetText("easy");
			textDifficulty.color = Functions.HexToColor("#FFFFFF");
			stringBuilder3.Append(Texts.Instance.GetText("easy"));
			num5 = 0;
		}
		else if (cDataCorruption.CardRarity == Enums.CardRarity.Uncommon)
		{
			text2 = string.Format(Texts.Instance.GetText("corruptionRewardScore"), "80");
			textDifficulty.text = Texts.Instance.GetText("average");
			textDifficulty.color = Globals.Instance.RarityColor["uncommon"];
			stringBuilder3.Append(Texts.Instance.GetText("average"));
			num5 = 1;
		}
		else if (cDataCorruption.CardRarity == Enums.CardRarity.Rare)
		{
			text2 = string.Format(Texts.Instance.GetText("corruptionRewardScore"), "130");
			textDifficulty.text = Texts.Instance.GetText("hard");
			textDifficulty.color = Globals.Instance.RarityColor["rare"];
			stringBuilder3.Append(Texts.Instance.GetText("hard"));
			num5 = 2;
		}
		else
		{
			text2 = string.Format(Texts.Instance.GetText("corruptionRewardScore"), "200");
			textDifficulty.text = Texts.Instance.GetText("extreme");
			textDifficulty.color = Globals.Instance.RarityColor["epic"];
			stringBuilder3.Append(Texts.Instance.GetText("extreme"));
			num5 = 3;
		}
		corruptionIconPopup.text = stringBuilder3.ToString();
		for (int num6 = 0; num6 < 4; num6++)
		{
			if (num6 != num5)
			{
				corruptionIcon[num6].gameObject.SetActive(value: false);
			}
			else
			{
				corruptionIcon[num6].gameObject.SetActive(value: true);
			}
		}
		textAcceptScore.text = text2;
		ShowClicked();
		AtOManager.Instance.corruptionId = "";
		AtOManager.Instance.corruptionType = corruptionRewardType;
		AtOManager.Instance.corruptionRewardChar = corruptionRewardChar;
		AtOManager.Instance.corruptionIdCard = corruptionIdCard;
		AtOManager.Instance.corruptionRewardCard = corruptionRewardCard;
		string text3 = CorruptionText(corruptionRewardId);
		rewardBotA.SetText(text3);
		text3 = CorruptionText(corruptionRewardIdB);
		rewardBotB.SetText(text3);
		if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
		{
			rewardBotA.Disable();
			rewardBotB.Disable();
			botonGenericX.Disable();
			rewardBotA.ShowDisableMask(state: false);
			rewardBotB.ShowDisableMask(state: false);
			botonGenericX.ShowDisableMask(state: false);
			corruptionContinue.gameObject.SetActive(value: false);
		}
	}

	public void ChooseReward(string choosed)
	{
		if (choosed == "A")
		{
			rewardBotA.color = Functions.HexToColor("#E0A44E");
			rewardBotA.SetColor();
			rewardBotA.PermaBorder(state: true);
			rewardBotB.color = Functions.HexToColor("#FFFFFF");
			rewardBotB.SetColor();
			rewardBotB.PermaBorder(state: false);
			if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
			{
				AtOManager.Instance.corruptionId = corruptionRewardId;
			}
		}
		else if (choosed == "B")
		{
			rewardBotA.color = Functions.HexToColor("#FFFFFF");
			rewardBotA.SetColor();
			rewardBotA.PermaBorder(state: false);
			rewardBotB.color = Functions.HexToColor("#E0A44E");
			rewardBotB.SetColor();
			rewardBotB.PermaBorder(state: true);
			if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
			{
				AtOManager.Instance.corruptionId = corruptionRewardIdB;
			}
		}
		if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
		{
			if (coroutineReward != null)
			{
				StopCoroutine(coroutineReward);
			}
			short choosed2 = 0;
			if (choosed == "A")
			{
				choosed2 = 1;
			}
			else if (choosed == "B")
			{
				choosed2 = 2;
			}
			coroutineReward = StartCoroutine(coroutineRewardFunc(choosed2));
		}
		if (!clicked)
		{
			BoxClicked();
		}
	}

	private IEnumerator coroutineRewardFunc(short choosed)
	{
		yield return Globals.Instance.WaitForSeconds(0.25f);
		photonView.RPC("NET_ChooseRewardCorruption", RpcTarget.Others, choosed);
	}

	public bool CorruptionOk()
	{
		if (clicked && AtOManager.Instance.corruptionId == "")
		{
			AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("corruptionSelect"));
			return false;
		}
		return true;
	}

	private string CorruptionText(string _corruption)
	{
		StringBuilder stringBuilder = new StringBuilder();
		string text = "";
		int num = 0;
		int num2 = 0;
		switch (_corruption)
		{
		case "goldshards0":
			text = Texts.Instance.GetText("corruptionGainGold");
			if (cDataCorruption.CardRarity == Enums.CardRarity.Common)
			{
				num = 320;
				num2 = 320;
				num = AtOManager.Instance.ModifyQuantityObeliskTraits(0, num);
				num2 = AtOManager.Instance.ModifyQuantityObeliskTraits(1, num2);
				stringBuilder.Append(string.Format(text, num, num2));
			}
			else
			{
				num = 520;
				num2 = 520;
				num = AtOManager.Instance.ModifyQuantityObeliskTraits(0, num);
				num2 = AtOManager.Instance.ModifyQuantityObeliskTraits(1, num2);
				stringBuilder.Append(string.Format(text, num, num2));
			}
			break;
		case "freecardremove":
			text = Texts.Instance.GetText("corruptionRemoveCard");
			stringBuilder.Append(text);
			break;
		case "rareshop":
			text = ((cDataCorruption.CardRarity != Enums.CardRarity.Common) ? Texts.Instance.GetText("corruptionRareShopDiscount") : Texts.Instance.GetText("corruptionRareShop"));
			stringBuilder.Append(text);
			break;
		case "altarupgrade":
			text = Texts.Instance.GetText("corruptionAltarUpgrade");
			stringBuilder.Append(text);
			break;
		case "randomcardupgrade":
			text = Texts.Instance.GetText("corruptionRandomCardUpgrade");
			stringBuilder.Append(text);
			break;
		case "heal20":
			text = Texts.Instance.GetText("corruptionHeal20");
			stringBuilder.Append(text);
			break;
		case "herocard":
		{
			textReward.margin = new Vector4(0f, 0f, 1f, 0f);
			cardReward.gameObject.SetActive(value: true);
			bgCards.gameObject.SetActive(value: true);
			bgHand.gameObject.SetActive(value: false);
			text = Texts.Instance.GetText("corruptionHeroFreeCard");
			string text2 = "";
			CardData cardData = Globals.Instance.GetCardData(corruptionRewardCard, instantiate: false);
			stringBuilder.Append(string.Format(arg2: (cardData.CardUpgraded != Enums.CardUpgraded.No) ? Globals.Instance.GetCardData(cardData.UpgradedFrom, instantiate: false).CardName : cardData.CardName, format: text, arg0: teamAtO[corruptionRewardChar].SourceName, arg1: Globals.Instance.ClassColor[Enum.GetName(typeof(Enums.HeroClass), teamAtO[corruptionRewardChar].HeroData.HeroClass)]));
			break;
		}
		case "goldshards1":
			text = Texts.Instance.GetText("corruptionGainGold2");
			if (cDataCorruption.CardRarity == Enums.CardRarity.Rare)
			{
				num = 720;
				num2 = 720;
				num = AtOManager.Instance.ModifyQuantityObeliskTraits(0, num);
				num2 = AtOManager.Instance.ModifyQuantityObeliskTraits(1, num2);
				stringBuilder.Append(string.Format(text, num, num2, "1"));
			}
			else
			{
				num = 1000;
				num2 = 1000;
				num = AtOManager.Instance.ModifyQuantityObeliskTraits(0, num);
				num2 = AtOManager.Instance.ModifyQuantityObeliskTraits(1, num2);
				stringBuilder.Append(string.Format(text, num, num2, "2"));
			}
			break;
		case "freecardupgrade":
			text = Texts.Instance.GetText("corruptionFreeCardUpgrade");
			stringBuilder.Append(text);
			break;
		case "freecardremove2":
			text = Texts.Instance.GetText("corruptionRemoveCard2");
			stringBuilder.Append(text);
			break;
		case "exoticshop":
			text = ((cDataCorruption.CardRarity != Enums.CardRarity.Rare) ? Texts.Instance.GetText("corruptionExoticShopDiscount") : Texts.Instance.GetText("corruptionExoticShop"));
			stringBuilder.Append(text);
			break;
		case "increasedqualityofcardrewards":
			text = Texts.Instance.GetText("corruptionIncreasedQuality");
			stringBuilder.Append(text);
			break;
		}
		return stringBuilder.ToString();
	}

	public bool IsActive()
	{
		return base.gameObject.activeSelf;
	}

	public void HideButton()
	{
		corruptionButton.gameObject.SetActive(value: false);
	}

	public void BoxClicked(bool setStatus = false, bool status = false)
	{
		if (!setStatus)
		{
			clicked = !clicked;
		}
		else
		{
			clicked = status;
		}
		botonGenericX.PermaBorder(clicked);
		if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
		{
			AtOManager.Instance.corruptionAccepted = clicked;
		}
		ShowClicked();
		if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
		{
			photonView.RPC("NET_BoxClicked", RpcTarget.Others, clicked);
		}
	}

	private void ShowClicked()
	{
		corruptionBoxX.gameObject.SetActive(clicked);
		bgCorruptionOn.gameObject.SetActive(clicked);
		if (!clicked)
		{
			rewardBotB.color = Functions.HexToColor("#FFFFFF");
			rewardBotB.SetColor();
			rewardBotB.PermaBorder(state: false);
			rewardBotA.color = Functions.HexToColor("#FFFFFF");
			rewardBotA.SetColor();
			rewardBotA.PermaBorder(state: false);
			if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
			{
				AtOManager.Instance.corruptionId = "";
			}
		}
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false, bool shoulderLeft = false, bool shoulderRight = false)
	{
		controllerList.Clear();
		if ((!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster()) && Functions.TransformIsVisible(elements))
		{
			if (Functions.TransformIsVisible(cardCorruption.transform))
			{
				controllerList.Add(cardCorruption.transform);
			}
			controllerList.Add(rewardBotA.transform);
			controllerList.Add(rewardBotB.transform);
			if (cardReward != null && cardReward.activeSelf)
			{
				controllerList.Add(cardReward.transform);
			}
			controllerList.Add(corruptionBoxX.transform);
			controllerList.Add(corruptionContinue.transform);
		}
		controllerList.Add(buttonShowText.transform);
		for (int i = 0; i < 4; i++)
		{
			if (Functions.TransformIsVisible(MapManager.Instance.sideCharacters.charArray[i].transform))
			{
				controllerList.Add(MapManager.Instance.sideCharacters.charArray[i].transform.GetChild(0).transform);
			}
		}
		if (Functions.TransformIsVisible(PlayerUIManager.Instance.giveGold))
		{
			controllerList.Add(PlayerUIManager.Instance.giveGold);
		}
		controllerHorizontalIndex = Functions.GetListClosestIndexToMousePosition(controllerList);
		controllerHorizontalIndex = Functions.GetClosestIndexBasedOnDirection(controllerList, controllerHorizontalIndex, goingUp, goingRight, goingDown, goingLeft);
		if (controllerList[controllerHorizontalIndex] != null)
		{
			warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(controllerList[controllerHorizontalIndex].position);
			Mouse.current.WarpCursorPosition(warpPosition);
		}
	}

	public void ControllerMoveBlock(bool _isRight)
	{
	}
}
