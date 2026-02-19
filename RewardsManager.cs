using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RewardsManager : MonoBehaviour
{
	private PhotonView photonView;

	public Transform sceneCamera;

	public CharacterWindowUI characterWindowUI;

	public Transform[] characterRewardArray = new Transform[4];

	public Dictionary<int, string[]> cardsByOrder;

	public Hero[] theTeam;

	public string[] cardSelectedArr;

	private string[] combatScarab;

	private int combatScarabGold;

	public int combatScarabDust;

	private int combatScarabExp;

	public int dustQuantity;

	private ThermometerData thermometerData;

	public TMP_Text title;

	public TMP_Text subtitle;

	public TMP_Text corruptionRewardText;

	public Transform corruptionReward;

	public Transform corruptionRewardBgText;

	public Transform corruptionRewardBgCard;

	private int numRewards;

	private int numCardsReward = 3;

	public int typeOfReward;

	private TierRewardData tierReward;

	private TierRewardData tierRewardBase;

	private TierRewardData tierRewardInf;

	public int experienceEach;

	public int goldEach;

	private int cardTierModFromCorruption;

	private bool finishReward;

	private bool reseting;

	public Transform buttonRestart;

	private string teamAtOToJson;

	private string[] keyListGold;

	private int[] valueListGold;

	private int playerGold;

	private string[] keyListDust;

	private int[] valueListDust;

	private int playerDust;

	private int divinationsNumber;

	private int totalGoldGained;

	private int totalDustGained;

	private int expGained;

	private int atoGoldGained;

	private int atoDustGained;

	public int controllerHorizontalIndex = -1;

	private Vector2 warpPosition = Vector2.zero;

	private List<Transform> _controllerList = new List<Transform>();

	public static RewardsManager Instance { get; private set; }

	private void Awake()
	{
		if (GameManager.Instance == null)
		{
			SceneStatic.LoadByName("Rewards");
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
		sceneCamera.gameObject.SetActive(value: false);
		photonView = PhotonView.Get(this);
		corruptionReward.gameObject.SetActive(value: false);
		NetworkManager.Instance.StartStopQueue(state: true);
	}

	public void RestartRewards()
	{
		if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
		{
			reseting = true;
			StartCoroutine(RestartRewardsMaster());
			return;
		}
		buttonRestart.gameObject.SetActive(value: false);
		string playerNickReal = NetworkManager.Instance.GetPlayerNickReal(NetworkManager.Instance.GetPlayerNick());
		photonView.RPC("NET_RestartRewards", RpcTarget.MasterClient, playerNickReal);
	}

	private IEnumerator RestartRewardsMaster()
	{
		if (!finishReward)
		{
			if (GameManager.Instance.IsMultiplayer())
			{
				photonView.RPC("NET_ShowMaskLoading", RpcTarget.Others);
				GameManager.Instance.SetMaskLoading();
				yield return Globals.Instance.WaitForSeconds(1f);
			}
			AtOManager.Instance.SetTeamFromTeamHero(JsonHelper.FromJson<Hero>(teamAtOToJson));
			AtOManager.Instance.SetPlayerGold(playerGold);
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			for (int i = 0; i < keyListGold.Length; i++)
			{
				dictionary.Add(keyListGold[i], valueListGold[i]);
			}
			AtOManager.Instance.SetMpPlayersGold(dictionary);
			AtOManager.Instance.SetPlayerDust(playerDust);
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
			for (int j = 0; j < keyListDust.Length; j++)
			{
				dictionary2.Add(keyListDust[j], valueListDust[j]);
			}
			AtOManager.Instance.SetMpPlayersDust(dictionary2);
			AtOManager.Instance.divinationsNumber = divinationsNumber;
			AtOManager.Instance.totalGoldGained = totalGoldGained;
			AtOManager.Instance.totalDustGained = totalDustGained;
			PlayerManager.Instance.GoldGained = atoGoldGained;
			PlayerManager.Instance.DustGained = atoDustGained;
			PlayerManager.Instance.ExpGained = expGained;
			AtOManager.Instance.RelaunchRewards();
		}
	}

	[PunRPC]
	private void NET_RestartRewards(string _nick)
	{
		AlertManager.Instance.AlertConfirmDouble(string.Format(Texts.Instance.GetText("restartClient"), _nick));
		AlertManager.buttonClickDelegate = WantToRestart;
		AlertManager.Instance.ShowReloadIcon();
	}

	[PunRPC]
	private void NET_ShowMaskLoading()
	{
		reseting = true;
		GameManager.Instance.SetMaskLoading();
	}

	private void WantToRestart()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(WantToRestart));
		if (AlertManager.Instance.GetConfirmAnswer())
		{
			RestartRewards();
		}
	}

	private void Start()
	{
		if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
		{
			teamAtOToJson = JsonHelper.ToJson(AtOManager.Instance.GetTeam());
			playerGold = AtOManager.Instance.GetPlayerGold();
			Dictionary<string, int> mpPlayersGold = AtOManager.Instance.GetMpPlayersGold();
			keyListGold = new string[mpPlayersGold.Count];
			mpPlayersGold.Keys.CopyTo(keyListGold, 0);
			valueListGold = new int[mpPlayersGold.Count];
			mpPlayersGold.Values.CopyTo(valueListGold, 0);
			playerDust = AtOManager.Instance.GetPlayerDust();
			Dictionary<string, int> mpPlayersDust = AtOManager.Instance.GetMpPlayersDust();
			keyListDust = new string[mpPlayersDust.Count];
			mpPlayersDust.Keys.CopyTo(keyListDust, 0);
			valueListDust = new int[mpPlayersDust.Count];
			mpPlayersDust.Values.CopyTo(valueListDust, 0);
			divinationsNumber = AtOManager.Instance.divinationsNumber;
			totalGoldGained = AtOManager.Instance.totalGoldGained;
			totalDustGained = AtOManager.Instance.totalDustGained;
			atoGoldGained = PlayerManager.Instance.GoldGained;
			atoDustGained = PlayerManager.Instance.DustGained;
			expGained = PlayerManager.Instance.ExpGained;
		}
		cardSelectedArr = new string[4];
		theTeam = AtOManager.Instance.GetTeam();
		AudioManager.Instance.DoBSO("Rewards");
		StartCoroutine(SetRewards());
	}

	private IEnumerator SetRewards()
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			if (NetworkManager.Instance.IsMaster())
			{
				while (!NetworkManager.Instance.AllPlayersReady("setrewards"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked setrewards");
				}
				NetworkManager.Instance.PlayersNetworkContinue("setrewards");
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("setrewards", status: true);
				NetworkManager.Instance.SetStatusReady("setrewards");
				while (NetworkManager.Instance.WaitingSyncro["setrewards"])
				{
					yield return Globals.Instance.WaitForSeconds(0.1f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("setrewards, we can continue!");
				}
			}
		}
		GameManager.Instance.SceneLoaded();
		if (AtOManager.Instance.corruptionAccepted)
		{
			AtOManager.Instance.comingFromCombatDoRewards = true;
			CardData cardData = Globals.Instance.GetCardData(AtOManager.Instance.corruptionIdCard, instantiate: false);
			if (cardData != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				Animator component = corruptionReward.GetComponent<Animator>();
				switch (AtOManager.Instance.corruptionId)
				{
				case "increasedqualityofcardrewards":
					stringBuilder.Append("<sprite name=cards> +1 ");
					stringBuilder.Append(Texts.Instance.GetText("cardsTier"));
					corruptionRewardText.text = stringBuilder.ToString();
					corruptionRewardBgText.gameObject.SetActive(value: true);
					corruptionReward.gameObject.SetActive(value: true);
					cardTierModFromCorruption = 1;
					numCardsReward = 4;
					component.SetTrigger("gold");
					break;
				case "goldshards0":
					if (cardData.CardRarity == Enums.CardRarity.Common)
					{
						int quantity = 320;
						quantity = AtOManager.Instance.ModifyQuantityObeliskTraits(0, quantity);
						stringBuilder.Append("<sprite name=gold> ");
						stringBuilder.Append(quantity);
						quantity = 320;
						quantity = AtOManager.Instance.ModifyQuantityObeliskTraits(1, quantity);
						stringBuilder.Append("  <sprite name=dust> ");
						stringBuilder.Append(quantity);
					}
					else
					{
						int quantity = 520;
						quantity = AtOManager.Instance.ModifyQuantityObeliskTraits(0, quantity);
						stringBuilder.Append("<sprite name=gold> ");
						stringBuilder.Append(quantity);
						quantity = 520;
						quantity = AtOManager.Instance.ModifyQuantityObeliskTraits(1, quantity);
						stringBuilder.Append("  <sprite name=dust> ");
						stringBuilder.Append(quantity);
					}
					corruptionRewardText.text = stringBuilder.ToString();
					corruptionRewardBgText.gameObject.SetActive(value: true);
					corruptionReward.gameObject.SetActive(value: true);
					component.SetTrigger("gold");
					break;
				case "goldshards1":
					if (cardData.CardRarity == Enums.CardRarity.Rare)
					{
						int quantity = 720;
						quantity = AtOManager.Instance.ModifyQuantityObeliskTraits(0, quantity);
						stringBuilder.Append("<sprite name=gold> ");
						stringBuilder.Append(quantity);
						quantity = 720;
						quantity = AtOManager.Instance.ModifyQuantityObeliskTraits(1, quantity);
						stringBuilder.Append("  <sprite name=dust> ");
						stringBuilder.Append(quantity);
						stringBuilder.Append("  <sprite name=supply> 1");
					}
					else
					{
						int quantity = 1000;
						quantity = AtOManager.Instance.ModifyQuantityObeliskTraits(0, quantity);
						stringBuilder.Append("<sprite name=gold> ");
						stringBuilder.Append(quantity);
						quantity = 1000;
						quantity = AtOManager.Instance.ModifyQuantityObeliskTraits(1, quantity);
						stringBuilder.Append("  <sprite name=dust> ");
						stringBuilder.Append(quantity);
						stringBuilder.Append("  <sprite name=supply> 2");
					}
					corruptionRewardText.text = stringBuilder.ToString();
					corruptionRewardBgText.gameObject.SetActive(value: true);
					corruptionReward.gameObject.SetActive(value: true);
					component.SetTrigger("gold");
					break;
				case "herocard":
				{
					stringBuilder.Append(theTeam[AtOManager.Instance.corruptionRewardChar].SourceName);
					corruptionRewardText.text = stringBuilder.ToString();
					corruptionRewardBgCard.gameObject.SetActive(value: true);
					corruptionReward.gameObject.SetActive(value: true);
					GameObject obj = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, corruptionRewardBgCard);
					obj.transform.localPosition = new Vector3(0f, -0.9f, 0f);
					obj.transform.localScale = new Vector3(1f, 1f, 1f);
					CardItem component2 = obj.GetComponent<CardItem>();
					component2.SetCard(AtOManager.Instance.corruptionRewardCard, deckScale: false, theTeam[AtOManager.Instance.corruptionRewardChar]);
					component2.TopLayeringOrder("Book", 2000);
					component2.cardmakebig = true;
					component2.CreateColliderAdjusted();
					component2.cardmakebigSize = 1f;
					component2.cardmakebigSizeMax = 1.1f;
					if (!PlayerManager.Instance.IsCardUnlocked(AtOManager.Instance.corruptionRewardCard))
					{
						PlayerManager.Instance.CardUnlock(AtOManager.Instance.corruptionRewardCard, save: true);
						component2.ShowUnlocked(showEffects: false);
					}
					component.SetTrigger("card");
					break;
				}
				}
			}
			else
			{
				AtOManager.Instance.ClearCorruption();
			}
		}
		if (AtOManager.Instance.combatScarab != "")
		{
			combatScarab = AtOManager.Instance.combatScarab.Split('%');
			if (combatScarab.Length == 2 && combatScarab[1] == "1")
			{
				if (combatScarab[0] == "goldenscarab")
				{
					combatScarabGold = 150;
				}
				else if (combatScarab[0] == "jadescarab")
				{
					combatScarabGold = 50;
					combatScarabDust = 50;
					combatScarabExp = 50;
				}
				else if (combatScarab[0] == "crystalscarab")
				{
					combatScarabDust = 150;
				}
			}
		}
		TierRewardData eventRewardTier = AtOManager.Instance.GetEventRewardTier();
		TierRewardData townDivinationTier = AtOManager.Instance.GetTownDivinationTier();
		subtitle.text = Texts.Instance.GetText("eventRewardsSubtitle");
		if (townDivinationTier != null)
		{
			title.text = Texts.Instance.GetText("divinationRoundRewards");
			if (townDivinationTier.TierNum > 5)
			{
				numCardsReward = 4;
			}
		}
		else if (eventRewardTier != null)
		{
			title.text = Texts.Instance.GetText("eventRewards");
		}
		else if (AtOManager.Instance.GetTeamNPC().Length != 0)
		{
			title.text = Texts.Instance.GetText("combatRewards");
			thermometerData = AtOManager.Instance.GetCombatThermometerData();
		}
		else
		{
			title.text = "";
		}
		if (thermometerData != null)
		{
			subtitle.text = Functions.ThermometerTextForRewards(thermometerData);
		}
		if (combatScarabGold > 0 || combatScarabDust > 0 || combatScarabExp > 0)
		{
			StringBuilder stringBuilder2 = new StringBuilder();
			StringBuilder stringBuilder3 = new StringBuilder();
			if (combatScarabGold > 0)
			{
				stringBuilder3.Append("<space=1><sprite name=gold>+");
				stringBuilder3.Append(combatScarabGold);
			}
			if (combatScarabDust > 0)
			{
				stringBuilder3.Append("<space=1><sprite name=dust>+");
				stringBuilder3.Append(combatScarabDust);
			}
			if (combatScarabExp > 0)
			{
				stringBuilder3.Append("<space=1><sprite name=experience>+");
				stringBuilder3.Append(combatScarabExp);
			}
			stringBuilder2.Append("\n<size=-.5><color=#FFEBA5><color=#A48D3D>[</color>");
			stringBuilder2.Append(string.Format(Texts.Instance.GetText("scarabBonus"), stringBuilder3.ToString()));
			stringBuilder2.Append("<color=#A48D3D>]</color></size></color>");
			subtitle.text += stringBuilder2.ToString();
		}
		bool flag = false;
		if (GameManager.Instance.IsObeliskChallenge() && Globals.Instance.ZoneDataSource[AtOManager.Instance.GetTownZoneId().ToLower()].ObeliskLow)
		{
			flag = true;
		}
		if (GameManager.Instance.IsMultiplayer() && (!GameManager.Instance.IsMultiplayer() || !NetworkManager.Instance.IsMaster()))
		{
			yield break;
		}
		UnityEngine.Random.InitState((AtOManager.Instance.GetGameId() + "_" + AtOManager.Instance.mapVisitedNodes.Count + "_" + AtOManager.Instance.currentMapNode + "_" + AtOManager.Instance.divinationsNumber).GetDeterministicHashCode());
		AtOManager.Instance.divinationsNumber++;
		cardsByOrder = new Dictionary<int, string[]>();
		if (townDivinationTier != null)
		{
			tierRewardBase = townDivinationTier;
			typeOfReward = 2;
		}
		else if (eventRewardTier != null)
		{
			tierRewardBase = eventRewardTier;
			typeOfReward = 2;
		}
		else if (AtOManager.Instance.GetTeamNPC().Length != 0)
		{
			tierRewardBase = AtOManager.Instance.GetTeamNPCReward();
			typeOfReward = 1;
		}
		else
		{
			tierRewardBase = Globals.Instance.GetTierRewardData(0);
			typeOfReward = 0;
		}
		dustQuantity = tierRewardBase.Dust;
		int num = tierRewardBase.TierNum;
		AtOManager.Instance.currentRewardTier = num;
		if (thermometerData != null)
		{
			num += thermometerData.CardBonus + cardTierModFromCorruption;
		}
		if (num < 0)
		{
			num = 0;
		}
		tierRewardBase = Globals.Instance.GetTierRewardData(num);
		if (GameManager.Instance.IsObeliskChallenge())
		{
			num = ((!flag) ? (num + 1) : (num + 2));
		}
		if (num > 0)
		{
			tierRewardInf = Globals.Instance.GetTierRewardData(num - 1);
		}
		else
		{
			tierRewardInf = tierRewardBase;
		}
		CardData cardData2 = null;
		for (int i = 0; i < theTeam.Length; i++)
		{
			if (theTeam[i] == null || theTeam[i].HeroData == null)
			{
				cardsByOrder[i] = new string[3] { "", "", "" };
				continue;
			}
			Hero hero = theTeam[i];
			Enums.CardClass result = Enums.CardClass.None;
			Enum.TryParse<Enums.CardClass>(Enum.GetName(typeof(Enums.HeroClass), hero.HeroData.HeroClass), out result);
			Enums.CardClass result2 = Enums.CardClass.None;
			Enum.TryParse<Enums.CardClass>(Enum.GetName(typeof(Enums.HeroClass), hero.HeroData.HeroSubClass.HeroClassSecondary), out result2);
			int num2 = numCardsReward;
			if (numCardsReward == 3 && result2 != Enums.CardClass.None)
			{
				num2 = 4;
			}
			string[] array = new string[num2];
			List<string> list = Globals.Instance.CardListNotUpgradedByClass[result];
			List<string> list2 = ((result2 == Enums.CardClass.None) ? new List<string>() : Globals.Instance.CardListNotUpgradedByClass[result2]);
			for (int j = 0; j < num2; j++)
			{
				if (j == 0)
				{
					tierReward = tierRewardBase;
				}
				else
				{
					tierReward = tierRewardInf;
				}
				int num3 = UnityEngine.Random.Range(0, 100);
				bool flag2 = true;
				while (flag2)
				{
					flag2 = false;
					bool flag3 = false;
					while (!flag3)
					{
						flag2 = false;
						string id = ((j < 2 || result2 == Enums.CardClass.None) ? list[UnityEngine.Random.Range(0, list.Count)] : list2[UnityEngine.Random.Range(0, list2.Count)]);
						cardData2 = Globals.Instance.GetCardData(id, instantiate: false);
						if (flag2)
						{
							continue;
						}
						if (num3 < tierReward.Common)
						{
							if (cardData2.CardRarity == Enums.CardRarity.Common)
							{
								flag3 = true;
							}
						}
						else if (num3 < tierReward.Common + tierReward.Uncommon)
						{
							if (cardData2.CardRarity == Enums.CardRarity.Uncommon)
							{
								flag3 = true;
							}
						}
						else if (num3 < tierReward.Common + tierReward.Uncommon + tierReward.Rare)
						{
							if (cardData2.CardRarity == Enums.CardRarity.Rare)
							{
								flag3 = true;
							}
						}
						else if (num3 < tierReward.Common + tierReward.Uncommon + tierReward.Rare + tierReward.Epic)
						{
							if (cardData2.CardRarity == Enums.CardRarity.Epic)
							{
								flag3 = true;
							}
						}
						else if (cardData2.CardRarity == Enums.CardRarity.Mythic)
						{
							flag3 = true;
						}
					}
					int rarity = UnityEngine.Random.Range(0, 100);
					_ = cardData2.Id;
					cardData2 = Globals.Instance.GetCardData(Functions.GetCardByRarity(rarity, cardData2), instantiate: false);
					if (cardData2 == null)
					{
						flag2 = true;
						continue;
					}
					for (int k = 0; k < array.Length; k++)
					{
						if (array[k] == cardData2.Id)
						{
							flag2 = true;
							break;
						}
					}
				}
				array[j] = cardData2.Id;
			}
			cardsByOrder[i] = Functions.ShuffleArray(array);
		}
		experienceEach = 0;
		goldEach = 0;
		if (typeOfReward == 1)
		{
			experienceEach = Functions.FuncRoundToInt(AtOManager.Instance.GetExperienceFromCombat() / 4);
			goldEach = Functions.FuncRoundToInt(AtOManager.Instance.GetGoldFromCombat() / 4);
			if (thermometerData != null)
			{
				experienceEach += Functions.FuncRoundToInt((float)experienceEach * thermometerData.ExpBonus / 100f);
				goldEach += Functions.FuncRoundToInt((float)goldEach * thermometerData.GoldBonus / 100f);
			}
		}
		if (GameManager.Instance.IsObeliskChallenge() && flag)
		{
			goldEach *= 2;
			dustQuantity *= 2;
		}
		if (MadnessManager.Instance.IsMadnessTraitActive("poverty") || AtOManager.Instance.IsChallengeTraitActive("poverty"))
		{
			if (!GameManager.Instance.IsObeliskChallenge())
			{
				dustQuantity -= Functions.FuncRoundToInt((float)dustQuantity * 0.5f);
				goldEach -= Functions.FuncRoundToInt((float)goldEach * 0.5f);
			}
			else
			{
				dustQuantity -= Functions.FuncRoundToInt((float)dustQuantity * 0.3f);
				goldEach -= Functions.FuncRoundToInt((float)goldEach * 0.3f);
			}
		}
		if (AtOManager.Instance.IsChallengeTraitActive("prosperity"))
		{
			dustQuantity += Functions.FuncRoundToInt((float)dustQuantity * 0.5f);
			goldEach += Functions.FuncRoundToInt((float)dustQuantity * 0.5f);
		}
		goldEach += combatScarabGold;
		experienceEach += combatScarabExp;
		if (GameManager.Instance.IsMultiplayer())
		{
			photonView.RPC("NET_ShareRewards", RpcTarget.Others, cardsByOrder[0], cardsByOrder[1], cardsByOrder[2], cardsByOrder[3], dustQuantity, typeOfReward, experienceEach, goldEach, combatScarabDust);
		}
		ShowRewards();
	}

	public void ShowCharacterWindow(string type = "", bool isHero = true, int characterIndex = -1)
	{
		characterWindowUI.Show(type, characterIndex);
	}

	public void ShowDeck(int auxInt)
	{
		characterWindowUI.Show("deck", auxInt);
	}

	[PunRPC]
	private void NET_ShareRewards(string[] cards0, string[] cards1, string[] cards2, string[] cards3, int _dustQuantity, int _typeOfReward, int _experienceEach, int _goldEach, int _combatScarabDust)
	{
		cardsByOrder = new Dictionary<int, string[]>();
		cardsByOrder.Add(0, cards0);
		cardsByOrder.Add(1, cards1);
		cardsByOrder.Add(2, cards2);
		cardsByOrder.Add(3, cards3);
		dustQuantity = _dustQuantity;
		typeOfReward = _typeOfReward;
		experienceEach = _experienceEach;
		goldEach = _goldEach;
		combatScarabDust = _combatScarabDust;
		ShowRewards();
	}

	private void ShowRewards()
	{
		StartCoroutine(ShowRewardsCo());
	}

	private IEnumerator ShowRewardsCo()
	{
		for (int i = 0; i < 4; i++)
		{
			if (theTeam[i] != null && !(theTeam[i].HeroData == null))
			{
				yield return Globals.Instance.WaitForSeconds(0.15f);
				characterRewardArray[i].gameObject.SetActive(value: true);
				characterRewardArray[i].GetComponent<CharacterReward>().Init(i);
				numRewards++;
			}
		}
		GameManager.Instance.ShowTutorialPopup("cardsReward", Vector3.zero, Vector3.zero);
	}

	public void SetCardReward(string playerNick, string internalId)
	{
		for (int i = 0; i < 4; i++)
		{
			characterRewardArray[i].GetComponent<CharacterReward>().CardSelected(playerNick, internalId);
		}
	}

	public void CardSelected(int index, string cardId)
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			photonView.RPC("MASTER_CardSelected", RpcTarget.MasterClient, (short)index, cardId);
		}
		else
		{
			NET_CardSelected((short)index, cardId);
		}
	}

	[PunRPC]
	private void MASTER_CardSelected(short index, string cardId)
	{
		if (!reseting)
		{
			photonView.RPC("NET_CardSelected", RpcTarget.All, index, cardId);
		}
	}

	[PunRPC]
	private void NET_CardSelected(short _index, string cardId)
	{
		cardSelectedArr[_index] = cardId;
		characterRewardArray[_index].GetComponent<CharacterReward>().ShowSelected(cardId);
		CheckAllAssigned();
	}

	public void DustSelected(int index)
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			photonView.RPC("MASTER_DustSelected", RpcTarget.MasterClient, (short)index);
		}
		else
		{
			NET_DustSelected((short)index);
		}
	}

	[PunRPC]
	private void MASTER_DustSelected(short index)
	{
		if (!reseting)
		{
			photonView.RPC("NET_DustSelected", RpcTarget.All, index);
		}
	}

	[PunRPC]
	private void NET_DustSelected(short _index)
	{
		cardSelectedArr[_index] = "dust";
		characterRewardArray[_index].GetComponent<CharacterReward>().ShowSelected("dust");
		CheckAllAssigned();
	}

	private void CheckAllAssigned()
	{
		for (int i = 0; i < numRewards; i++)
		{
			if (cardSelectedArr[i] == null)
			{
				return;
			}
		}
		if (!GameManager.Instance.IsMultiplayer() || (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster()))
		{
			finishReward = true;
			buttonRestart.gameObject.SetActive(value: false);
			StartCoroutine(CloseWindow());
		}
		SetCombatCorruption();
		SaveManager.SavePlayerData();
	}

	public void Reload()
	{
		SceneStatic.LoadByName("Rewards");
	}

	private void SetCombatCorruption()
	{
		AtOManager.Instance.SetCombatCorruptionForScore();
	}

	private IEnumerator CloseWindow()
	{
		yield return Globals.Instance.WaitForSeconds(0.4f);
		AtOManager.Instance.DeleteSaveGameTurn();
		AtOManager.Instance.FinishCardRewards(cardSelectedArr);
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false, bool shoulderLeft = false, bool shoulderRight = false)
	{
		_controllerList.Clear();
		_controllerList.Add(buttonRestart);
		for (int i = 0; i < 4; i++)
		{
			if (!Functions.TransformIsVisible(characterRewardArray[i].transform))
			{
				continue;
			}
			foreach (Transform item in characterRewardArray[i].GetComponent<CharacterReward>().cardsTransform)
			{
				if (item.gameObject.activeSelf)
				{
					_controllerList.Add(item);
				}
			}
			if (characterRewardArray[i].GetComponent<CharacterReward>().quantityDust != null && characterRewardArray[i].GetComponent<CharacterReward>().quantityDust.GetChild(0).GetComponent<BoxCollider2D>().enabled)
			{
				_controllerList.Add(characterRewardArray[i].GetComponent<CharacterReward>().quantityDust);
			}
			_controllerList.Add(characterRewardArray[i].GetComponent<CharacterReward>().buttonCharacterDeck);
		}
		controllerHorizontalIndex = Functions.GetListClosestIndexToMousePosition(_controllerList);
		controllerHorizontalIndex = Functions.GetClosestIndexBasedOnDirection(_controllerList, controllerHorizontalIndex, goingUp, goingRight, goingDown, goingLeft);
		if (_controllerList[controllerHorizontalIndex] != null)
		{
			warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerList[controllerHorizontalIndex].position);
			Mouse.current.WarpCursorPosition(warpPosition);
		}
	}

	public void ControllerMoveShoulder(bool _isRight = false)
	{
	}
}
