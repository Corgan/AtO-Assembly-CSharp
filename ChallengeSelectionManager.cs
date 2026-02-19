using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class ChallengeSelectionManager : MonoBehaviour
{
	private PhotonView photonView;

	public ChallengeBossBanners challengeBossBanners;

	public SideCharacters sideCharacters;

	public int currentHeroIndex;

	private int maxRound = 4;

	public PerkChallengeItem[] perkChallengeItems;

	private int[] heroRoundArr = new int[4];

	private int[] heroRerolledTimesArr = new int[4];

	private int[,] heroPackSelectedRerolledTimesArr = new int[4, 8];

	private int maxSelectablePerks = 4;

	private int cardsForPack = 3;

	private Hero[] theTeam;

	private Dictionary<string, string[]> cardsDrafted = new Dictionary<string, string[]>();

	private Dictionary<string, string> cardsDraftedSpecial = new Dictionary<string, string>();

	private Dictionary<string, string> cardsDraftedPackname = new Dictionary<string, string>();

	private Dictionary<string, string> cardsDraftedPackid = new Dictionary<string, string>();

	private Dictionary<int, string> packsSelected = new Dictionary<int, string>();

	private Dictionary<int, List<int>> perkDrafted = new Dictionary<int, List<int>>();

	private Dictionary<string, int> dictBonus = new Dictionary<string, int>();

	private Dictionary<string, int> dictBonusSingle = new Dictionary<string, int>();

	private Dictionary<string, int> dictAura = new Dictionary<string, int>();

	private Dictionary<string, int> dictAuraSingle = new Dictionary<string, int>();

	private Dictionary<string, float> dictEnergyCost = new Dictionary<string, float>();

	private bool statusReady;

	private Coroutine manualReadyCo;

	public TMP_Text _HP;

	public TMP_Text _Energy;

	public TMP_Text _Speed;

	public Transform sceneCamera;

	public static ChallengeSelectionManager Instance { get; private set; }

	public int CardsForPack
	{
		get
		{
			return cardsForPack;
		}
		set
		{
			cardsForPack = value;
		}
	}

	private void Awake()
	{
		if (GameManager.Instance == null)
		{
			SceneStatic.LoadByName("Game");
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
		photonView = PhotonView.Get(this);
		sceneCamera.gameObject.SetActive(value: false);
		NetworkManager.Instance.StartStopQueue(state: true);
	}

	private void Start()
	{
		StartCoroutine(StartCo());
	}

	private IEnumerator StartCo()
	{
		AudioManager.Instance.DoBSO("Town");
		if (GameManager.Instance.IsMultiplayer())
		{
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("**************************");
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("WaitingSyncro startchallenge", "net");
			}
			yield return Globals.Instance.WaitForSeconds(0.1f);
			if (NetworkManager.Instance.IsMaster())
			{
				while (!NetworkManager.Instance.AllPlayersReady("startchallenge"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked startchallenge", "net");
				}
				NetworkManager.Instance.PlayersNetworkContinue("startchallenge");
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("startchallenge", status: true);
				NetworkManager.Instance.SetStatusReady("startchallenge");
				while (NetworkManager.Instance.WaitingSyncro["startchallenge"])
				{
					yield return Globals.Instance.WaitForSeconds(0.1f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("startchallenge, we can continue!", "net");
				}
			}
		}
		for (int i = 0; i < 4; i++)
		{
			heroRoundArr[i] = 0;
			heroRerolledTimesArr[i] = 0;
			for (int j = 0; j < 8; j++)
			{
				heroPackSelectedRerolledTimesArr[i, j] = 0;
			}
			if (!perkDrafted.ContainsKey(i))
			{
				perkDrafted.Add(i, new List<int>());
			}
			else
			{
				perkDrafted[i] = new List<int>();
			}
		}
		theTeam = AtOManager.Instance.GetTeam();
		SetDefaultCards(0);
		SetDefaultCards(1);
		SetDefaultCards(2);
		SetDefaultCards(3);
		AtOManager.Instance.DoChallengeShop();
		perkChallengeItems = CardCraftManager.Instance.perkChallengeItems;
		Debug.Log("SIDE CHARACTER MANHOOS BUG 1");
		sideCharacters.Show();
		Debug.Log("SIDE CHARACTER MANHOOS BUG 2");
		sideCharacters.ShowChallengeButtons(-1, state: false);
		Debug.Log("SIDE CHARACTER MANHOOS BUG 3");
		GeneratePacks();
		CardCraftManager.Instance.SetWaitingPlayerTextChallenge("");
		CardCraftManager.Instance.EnableChallengeReadyButton(state: false);
		if (GameManager.Instance.IsMultiplayer())
		{
			CardCraftManager.Instance.ChallengeReadySetButton(state: false);
		}
		challengeBossBanners.SetBosses();
		GameManager.Instance.SceneLoaded();
		StartCoroutine(NextHero(timeOut: false));
	}

	private void GeneratePacks()
	{
		UnityEngine.Random.InitState(AtOManager.Instance.GetGameId().GetDeterministicHashCode());
		for (int i = 0; i < 4; i++)
		{
			Hero hero = theTeam[i];
			if (hero == null || hero.HeroData == null)
			{
				continue;
			}
			Enums.CardClass result = Enums.CardClass.None;
			Enum.TryParse<Enums.CardClass>(Enum.GetName(typeof(Enums.HeroClass), hero.HeroData.HeroClass), out result);
			List<string> list = Globals.Instance.CardListNotUpgradedByClass[result];
			Enums.CardClass result2 = Enums.CardClass.None;
			Enum.TryParse<Enums.CardClass>(Enum.GetName(typeof(Enums.HeroClass), hero.HeroData.HeroSubClass.HeroClassSecondary), out result2);
			List<string> list2 = null;
			if (result2 != Enums.CardClass.None)
			{
				list2 = Globals.Instance.CardListNotUpgradedByClass[result2];
			}
			for (int j = 0; j < 3; j++)
			{
				List<PackData> packListForClass = GetPackListForClass(result, hero.HeroData.HeroSubClass.Id, j);
				for (int k = 0; k < 8; k++)
				{
					List<CardData> list3 = new List<CardData>();
					if (k < 7)
					{
						if (packListForClass[k].Card0 != null)
						{
							list3.Add(packListForClass[k].Card0);
						}
						if (packListForClass[k].Card1 != null)
						{
							list3.Add(packListForClass[k].Card1);
						}
						if (packListForClass[k].Card2 != null)
						{
							list3.Add(packListForClass[k].Card2);
						}
						if (packListForClass[k].Card3 != null)
						{
							list3.Add(packListForClass[k].Card3);
						}
						if (packListForClass[k].Card4 != null)
						{
							list3.Add(packListForClass[k].Card4);
						}
						if (packListForClass[k].Card5 != null)
						{
							list3.Add(packListForClass[k].Card5);
						}
					}
					else
					{
						List<string> list4 = new List<string>();
						string text = "";
						bool flag = false;
						bool flag2 = true;
						for (int l = 0; l < 6; l++)
						{
							flag = false;
							while (!flag)
							{
								flag2 = list2 == null || UnityEngine.Random.Range(0, 2) == 0;
								text = ((!flag2) ? list2[UnityEngine.Random.Range(0, list2.Count)] : list[UnityEngine.Random.Range(0, list.Count)]);
								if (!list4.Contains(text))
								{
									list4.Add(text);
									list3.Add(Globals.Instance.GetCardData(text));
									flag = true;
								}
							}
						}
						list4 = null;
					}
					List<CardData> list5 = new List<CardData>();
					if (k < 7)
					{
						if (packListForClass[k].CardSpecial0 != null)
						{
							list5.Add(packListForClass[k].CardSpecial0);
						}
						if (packListForClass[k].CardSpecial1 != null)
						{
							list5.Add(packListForClass[k].CardSpecial1);
						}
					}
					else
					{
						List<string> list6 = new List<string>();
						string text2 = "";
						bool flag3 = false;
						for (int m = 0; m < 2; m++)
						{
							flag3 = false;
							while (!flag3)
							{
								text2 = list[UnityEngine.Random.Range(0, list.Count)];
								if (!list6.Contains(text2))
								{
									CardData cardData = Globals.Instance.GetCardData(text2, instantiate: false);
									if (cardData.CardRarity == Enums.CardRarity.Rare || cardData.CardRarity == Enums.CardRarity.Epic)
									{
										list6.Add(text2);
										list5.Add(cardData);
										flag3 = true;
									}
								}
							}
						}
						list6 = null;
					}
					list3 = list3.ShuffleList();
					list5 = list5.ShuffleList();
					string[] array = new string[cardsForPack];
					for (int n = 0; n < cardsForPack; n++)
					{
						array[n] = Functions.GetCardByRarity(UnityEngine.Random.Range(0, 100), list3[n], isChallenge: true);
					}
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(i);
					stringBuilder.Append("_");
					stringBuilder.Append(j);
					stringBuilder.Append("_");
					stringBuilder.Append(k);
					cardsDrafted.Add(stringBuilder.ToString(), array);
					if (k < 7)
					{
						cardsDraftedPackname.Add(stringBuilder.ToString(), packListForClass[k].PackName);
					}
					else
					{
						cardsDraftedPackname.Add(stringBuilder.ToString(), "random");
					}
					cardsDraftedPackid.Add(stringBuilder.ToString(), k.ToString());
					bool flag4 = false;
					string text3 = "";
					int num = UnityEngine.Random.Range(0, list5.Count);
					int num2 = 0;
					while (!flag4 && num2 < 500)
					{
						text3 = Functions.GetCardByRarity(UnityEngine.Random.Range(0, 100), list5[num], isChallenge: true) + "_" + hero.HeroData.HeroSubClass.Id;
						while (cardsDraftedSpecial.ContainsValue(text3))
						{
							num++;
							if (num >= list5.Count)
							{
								num = 0;
							}
							text3 = Functions.GetCardByRarity(UnityEngine.Random.Range(0, 100), list5[num], isChallenge: true) + "_" + hero.HeroData.HeroSubClass.Id;
						}
						string[] array2 = text3.Split('_');
						if (array2 != null && array2.Length >= 1)
						{
							CardData cardData2 = Globals.Instance.GetCardData(array2[0], instantiate: false);
							if (cardData2 != null && cardData2.CardUpgraded != Enums.CardUpgraded.No)
							{
								flag4 = true;
							}
						}
						num2++;
					}
					stringBuilder.Clear();
					stringBuilder.Append(i);
					stringBuilder.Append("_");
					stringBuilder.Append(j);
					stringBuilder.Append("_");
					stringBuilder.Append(k);
					stringBuilder.Append("_special");
					cardsDraftedSpecial.Add(stringBuilder.ToString(), text3);
				}
			}
		}
	}

	public void Resize()
	{
		sideCharacters.Resize();
	}

	private void SetCurrentHeroAndRound()
	{
		AtOManager.Instance.SideBarCharacterClicked(currentHeroIndex);
	}

	public void RerollFromButton()
	{
		if (!GameManager.Instance.IsMultiplayer() || AtOManager.Instance.GetHero(currentHeroIndex).Owner == NetworkManager.Instance.GetPlayerNick())
		{
			Reroll(currentHeroIndex);
		}
	}

	public void Reroll(int _heroId, bool fromMP = false)
	{
		if (!fromMP && GameManager.Instance.IsMultiplayer() && AtOManager.Instance.GetHero(_heroId).Owner == NetworkManager.Instance.GetPlayerNick())
		{
			photonView.RPC("NET_Reroll", RpcTarget.Others, _heroId);
		}
		if (heroRerolledTimesArr[_heroId] < 1)
		{
			heroRerolledTimesArr[_heroId]++;
			if (_heroId == currentHeroIndex)
			{
				ShowCardList(_heroId, refreshAllPacks: false, comingFromReroll: true);
			}
		}
	}

	[PunRPC]
	private void NET_Reroll(int _heroId)
	{
		Reroll(_heroId, fromMP: true);
	}

	public void ChangeCharacter(int _heroIndex)
	{
		CardCraftManager.Instance.CleanChallengeBlocks();
		SubClassData heroSubClass = theTeam[_heroIndex].HeroData.HeroSubClass;
		int hp = heroSubClass.Hp;
		_HP.text = hp.ToString();
		int energy = heroSubClass.Energy;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(energy);
		stringBuilder.Append(" <size=1.3>");
		stringBuilder.Append(Texts.Instance.GetText("dataPerTurn").Replace("<%>", heroSubClass.EnergyTurn.ToString()));
		stringBuilder.Append("</size>");
		_Energy.text = stringBuilder.ToString();
		stringBuilder = null;
		int speed = heroSubClass.Speed;
		_Speed.text = speed.ToString();
		ShowCardList(_heroIndex, refreshAllPacks: true);
	}

	public int GetCurrentRound()
	{
		return heroRoundArr[currentHeroIndex];
	}

	private void ShowCardList(int heroId, bool refreshAllPacks = false, bool comingFromReroll = false)
	{
		currentHeroIndex = heroId;
		GameManager.Instance.CleanTempContainer();
		CardCraftManager.Instance.ReassignChallengeButtons();
		WriteBonus();
		if (heroRoundArr[heroId] < 3)
		{
			List<int> list = new List<int>();
			if (packsSelected != null && packsSelected.ContainsKey(heroId))
			{
				string[] array = packsSelected[heroId].Split('_');
				for (int i = 0; i < array.Length; i++)
				{
					if (int.TryParse(array[i], out var result))
					{
						list.Add(result);
					}
				}
			}
			CardCraftManager.Instance.AssignChallengeRoundCards(heroRoundArr[heroId] + 1, maxRound);
			int num = 8;
			int num2 = 0;
			for (int j = 0; j < num; j++)
			{
				bool flag = true;
				bool selectedPack = false;
				string key = heroId + "_" + heroRerolledTimesArr[heroId] + "_" + j;
				if (list.Contains(j))
				{
					key = heroId + "_" + heroPackSelectedRerolledTimesArr[heroId, j] + "_" + j;
					selectedPack = true;
					flag = false;
				}
				else
				{
					CardCraftManager.Instance.ShowChallengePackSelected(_state: false, j);
				}
				if (refreshAllPacks || (comingFromReroll && flag))
				{
					CardCraftManager.Instance.CleanChallengeBlocks(num2);
					CardCraftManager.Instance.AssignChallengeTitle(num2, Functions.UppercaseFirst(Texts.Instance.GetText(cardsDraftedPackname[key])));
					for (int k = 0; k < cardsForPack; k++)
					{
						CardCraftManager.Instance.AssignChallengeCard(heroId, num2, k, cardsDrafted[key][k], selectedPack);
					}
				}
				num2++;
			}
			CardCraftManager.Instance.ShowChallengeRerollFully(state: true);
			CardCraftManager.Instance.ShowChallengePerks(state: false);
		}
		else if (heroRoundArr[heroId] == 3)
		{
			CardCraftManager.Instance.CleanChallengeBlocks();
			CardCraftManager.Instance.AssignChallengeRoundCards(heroRoundArr[heroId] + 1, maxRound);
			string[] array2 = packsSelected[heroId].Split('_');
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			for (int l = 0; l < 3; l++)
			{
				stringBuilder2.Clear();
				stringBuilder2.Append(heroId);
				stringBuilder2.Append("_");
				stringBuilder2.Append(heroPackSelectedRerolledTimesArr[heroId, int.Parse(array2[l])]);
				stringBuilder2.Append("_");
				stringBuilder2.Append(array2[l]);
				stringBuilder2.Append("_special");
				string value = cardsDraftedSpecial[stringBuilder2.ToString()].Split('_')[0];
				stringBuilder.Append(value);
				stringBuilder.Append('_');
				string key2 = heroId + "_" + l + "_" + array2[l];
				CardCraftManager.Instance.AssignChallengeTitle(l, Functions.UppercaseFirst(Texts.Instance.GetText(cardsDraftedPackname[key2])));
			}
			bool showButtons = false;
			if (!GameManager.Instance.IsMultiplayer() || AtOManager.Instance.GetHero(heroId).Owner == NetworkManager.Instance.GetPlayerNick())
			{
				showButtons = true;
			}
			CardCraftManager.Instance.AssignChallengeCardSpecial(stringBuilder.ToString(), showButtons);
			CardCraftManager.Instance.ShowChallengeRerollFully(state: false);
			CardCraftManager.Instance.ShowChallengePerks(state: false);
			CardCraftManager.Instance.controllerHorizontalIndex = -1;
			stringBuilder = null;
			stringBuilder2 = null;
		}
		else if (heroRoundArr[heroId] == 4)
		{
			CardCraftManager.Instance.CleanChallengeBlocks();
			CardCraftManager.Instance.ShowChallengeRerollFully(state: false);
			CardCraftManager.Instance.ShowChallengePerks(state: true);
			AssignPerkButtons();
		}
		CardCraftManager.Instance.ShowChallengeReroll(heroRerolledTimesArr[heroId] < 1);
		CardCraftManager.Instance.ActivateChallengeReroll(state: true);
		if (GameManager.Instance.IsMultiplayer() && AtOManager.Instance.GetHero(heroId).Owner != NetworkManager.Instance.GetPlayerNick())
		{
			CardCraftManager.Instance.ActivateChallengeReroll(state: false);
		}
	}

	private void AssignPerkButtons()
	{
		Enums.CardClass cardClass = (Enums.CardClass)Enum.Parse(typeof(Enums.CardClass), Enum.GetName(typeof(Enums.HeroClass), theTeam[currentHeroIndex].HeroData.HeroClass));
		if (theTeam[currentHeroIndex].HeroData.HeroSubClass.Id == Globals.Instance.GetSubClassData("engineer").Id)
		{
			cardClass = Enums.CardClass.Warrior;
		}
		List<PerkData> perkDataClass = Globals.Instance.GetPerkDataClass(cardClass);
		int num = 0;
		int num2 = 0;
		SortedDictionary<int, PerkData> sortedDictionary = new SortedDictionary<int, PerkData>();
		for (int i = 0; i < perkDataClass.Count; i++)
		{
			if (perkDataClass[i].ObeliskPerk && !sortedDictionary.ContainsKey(perkDataClass[i].Level))
			{
				sortedDictionary.Add(perkDataClass[i].Level, perkDataClass[i]);
			}
		}
		foreach (KeyValuePair<int, PerkData> item in sortedDictionary)
		{
			PerkChallengeItem perkChallengeItem = perkChallengeItems[num];
			perkChallengeItem.SetPerk(currentHeroIndex, num, item.Value.Id);
			if (perkDrafted.ContainsKey(currentHeroIndex) && perkDrafted[currentHeroIndex].Contains(num))
			{
				perkChallengeItem.SetActive(state: true);
				num2++;
			}
			else
			{
				perkChallengeItem.SetActive(state: false);
			}
			num++;
		}
		Enums.CardClass result = Enums.CardClass.None;
		Enum.TryParse<Enums.CardClass>(Enum.GetName(typeof(Enums.HeroClass), theTeam[currentHeroIndex].HeroData.HeroSubClass.HeroClassSecondary), out result);
		for (int j = 0; j < perkChallengeItems.Length; j++)
		{
			if (j > 20)
			{
				if (result != Enums.CardClass.None)
				{
					perkChallengeItems[j].gameObject.SetActive(value: true);
				}
				else
				{
					perkChallengeItems[j].gameObject.SetActive(value: false);
				}
			}
		}
		WriteSelectedPerks(currentHeroIndex);
		FinishDraw();
	}

	public void AssignPerk(int _heroId, int _perkIndex, bool fromMP = false)
	{
		if (!fromMP && GameManager.Instance.IsMultiplayer())
		{
			if (!(AtOManager.Instance.GetHero(_heroId).Owner == NetworkManager.Instance.GetPlayerNick()))
			{
				return;
			}
			photonView.RPC("NET_AssignPerk", RpcTarget.Others, _heroId, _perkIndex);
		}
		if (!perkDrafted.ContainsKey(_heroId))
		{
			perkDrafted.Add(_heroId, new List<int>());
		}
		if (perkDrafted[_heroId].Contains(_perkIndex))
		{
			perkDrafted[_heroId].Remove(_perkIndex);
			if (GameManager.Instance.IsMultiplayer() && statusReady && !fromMP)
			{
				Ready();
			}
		}
		else if (perkDrafted[_heroId].Count < maxSelectablePerks)
		{
			perkDrafted[_heroId].Add(_perkIndex);
		}
		if (currentHeroIndex == _heroId)
		{
			AssignPerkButtons();
		}
	}

	[PunRPC]
	private void NET_AssignPerk(int _heroId, int _perk)
	{
		AssignPerk(_heroId, _perk, fromMP: true);
	}

	private void WriteSelectedPerks(int _heroId)
	{
		CardCraftManager.Instance.AssignChallengeRoundPerks(perkDrafted[_heroId].Count, maxSelectablePerks);
	}

	public int GetActiveHero()
	{
		return currentHeroIndex;
	}

	private void SetDefaultCards(int _heroIndex)
	{
		Hero hero = theTeam[_heroIndex];
		if (hero == null || hero.HeroData == null)
		{
			return;
		}
		foreach (KeyValuePair<string, PackData> item in Globals.Instance.PackDataSource)
		{
			if (item.Value.RequiredClass != null && item.Value.RequiredClass.Id == hero.HeroData.HeroSubClass.Id)
			{
				if (item.Value.Card0 != null)
				{
					AtOManager.Instance.AddCardToHero(_heroIndex, StarterUpgradeCard(item.Value.Card0.Id));
				}
				if (item.Value.Card1 != null)
				{
					AtOManager.Instance.AddCardToHero(_heroIndex, StarterUpgradeCard(item.Value.Card1.Id));
				}
				if (item.Value.Card2 != null)
				{
					AtOManager.Instance.AddCardToHero(_heroIndex, StarterUpgradeCard(item.Value.Card2.Id));
				}
				if (item.Value.Card3 != null)
				{
					AtOManager.Instance.AddCardToHero(_heroIndex, StarterUpgradeCard(item.Value.Card3.Id));
				}
				if (item.Value.Card4 != null)
				{
					AtOManager.Instance.AddCardToHero(_heroIndex, StarterUpgradeCard(item.Value.Card4.Id));
				}
				break;
			}
		}
	}

	private string StarterUpgradeCard(string _cardId)
	{
		if (!GameManager.Instance.IsWeeklyChallenge())
		{
			int obeliskMadness = AtOManager.Instance.GetObeliskMadness();
			if (obeliskMadness >= 5)
			{
				CardData cardData = Globals.Instance.GetCardData(_cardId, instantiate: false);
				if (cardData.Starter)
				{
					if (obeliskMadness <= 7)
					{
						return cardData.UpgradesTo1;
					}
					return cardData.UpgradesTo2;
				}
			}
		}
		else
		{
			CardData cardData2 = Globals.Instance.GetCardData(_cardId, instantiate: false);
			if (cardData2.Starter)
			{
				return cardData2.UpgradesTo2;
			}
		}
		return _cardId;
	}

	public List<PackData> GetPackListForClass(Enums.CardClass cardClass, string subclassId, int round)
	{
		List<PackData> list = new List<PackData>();
		SubClassData subClassData = Globals.Instance.GetSubClassData(subclassId);
		list.Add(subClassData.ChallengePack0);
		list.Add(subClassData.ChallengePack1);
		list.Add(subClassData.ChallengePack2);
		list.Add(subClassData.ChallengePack3);
		list.Add(subClassData.ChallengePack4);
		list.Add(subClassData.ChallengePack5);
		list.Add(subClassData.ChallengePack6);
		return list;
	}

	public void SelectPack(int _heroId, int _pack, bool fromMP = false)
	{
		if (!fromMP && GameManager.Instance.IsMultiplayer() && AtOManager.Instance.GetHero(_heroId).Owner == NetworkManager.Instance.GetPlayerNick())
		{
			photonView.RPC("NET_SelectPack", RpcTarget.Others, _heroId, _pack);
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (heroRoundArr[_heroId] < 3)
		{
			stringBuilder.Append(_heroId);
			stringBuilder.Append("_");
			stringBuilder.Append(heroRerolledTimesArr[_heroId]);
			stringBuilder.Append("_");
			stringBuilder.Append(_pack);
			string[] array = cardsDrafted[stringBuilder.ToString()];
			for (int i = 0; i < array.Length; i++)
			{
				AtOManager.Instance.AddCardToHero(_heroId, array[i]);
			}
			if (!packsSelected.ContainsKey(_heroId))
			{
				packsSelected.Add(_heroId, "");
			}
			Dictionary<int, string> dictionary = packsSelected;
			dictionary[_heroId] = dictionary[_heroId] + _pack + "_";
			heroPackSelectedRerolledTimesArr[_heroId, _pack] = heroRerolledTimesArr[_heroId];
			if (currentHeroIndex == _heroId)
			{
				CardCraftManager.Instance.ShowChallengePackSelected(_state: true, _pack);
			}
		}
		else
		{
			string[] array2 = packsSelected[_heroId].Split('_');
			stringBuilder.Append(_heroId);
			stringBuilder.Append("_");
			stringBuilder.Append(heroPackSelectedRerolledTimesArr[_heroId, int.Parse(array2[_pack])]);
			stringBuilder.Append("_");
			stringBuilder.Append(array2[_pack]);
			stringBuilder.Append("_special");
			string cardName = cardsDraftedSpecial[stringBuilder.ToString()].Split('_')[0];
			AtOManager.Instance.AddCardToHero(_heroId, cardName);
		}
		heroRoundArr[_heroId]++;
		sideCharacters.RefreshCards(_heroId);
		if (currentHeroIndex == _heroId)
		{
			CardCraftManager.Instance.ShowChallengeButtons(state: false, _pack);
			CardCraftManager.Instance.CreateDeck(currentHeroIndex);
			if (heroRoundArr[currentHeroIndex] <= maxRound)
			{
				StartCoroutine(NextRound());
			}
			else
			{
				StartCoroutine(NextHero());
			}
		}
	}

	[PunRPC]
	private void NET_SelectPack(int _heroId, int _pack)
	{
		SelectPack(_heroId, _pack, fromMP: true);
	}

	private IEnumerator NextRound()
	{
		yield return Globals.Instance.WaitForSeconds(0.15f);
		ShowCardList(currentHeroIndex);
	}

	public void NextHeroFunc(bool _isRight)
	{
		int num = currentHeroIndex;
		num = ((!_isRight) ? (num - 1) : (num + 1));
		if (num > 3)
		{
			num = 0;
		}
		else if (num < 0)
		{
			num = 3;
		}
		GameObject gameObject = GameObject.Find("/SideCharacters/OverCharacter" + num);
		if (gameObject != null)
		{
			gameObject.transform.GetComponent<OverCharacter>().Clicked();
		}
	}

	private IEnumerator NextHero(bool timeOut = true)
	{
		if (timeOut)
		{
			yield return Globals.Instance.WaitForSeconds(0.15f);
		}
		bool flag = false;
		currentHeroIndex = 0;
		while (currentHeroIndex < 4 && !flag)
		{
			if (heroRoundArr[currentHeroIndex] < 3 && (!GameManager.Instance.IsMultiplayer() || AtOManager.Instance.GetHero(currentHeroIndex).Owner == NetworkManager.Instance.GetPlayerNick()))
			{
				flag = true;
			}
			if (!flag)
			{
				currentHeroIndex++;
			}
		}
		if (currentHeroIndex < 4)
		{
			SetCurrentHeroAndRound();
			ShowCardList(currentHeroIndex);
		}
		else
		{
			FinishDraw();
		}
	}

	private void FinishDraw()
	{
		bool flag = true;
		for (int i = 0; i < 4; i++)
		{
			sideCharacters.ShowChallengeButtons(i);
			if (perkDrafted != null && perkDrafted[i] != null && perkDrafted[i].Count < 4)
			{
				sideCharacters.ShowChallengeButtons(i, state: false);
			}
		}
		if (!GameManager.Instance.IsMultiplayer())
		{
			for (int j = 0; j < 4; j++)
			{
				if (theTeam[j] != null && !(theTeam[j].HeroData == null) && perkDrafted != null && perkDrafted[j] != null && perkDrafted[j].Count < 4)
				{
					flag = false;
					break;
				}
			}
		}
		else
		{
			for (int k = 0; k < 4; k++)
			{
				if (theTeam[k] != null && !(theTeam[k].HeroData == null) && AtOManager.Instance.GetHero(k).Owner == NetworkManager.Instance.GetPlayerNick() && perkDrafted != null && perkDrafted[k] != null && perkDrafted[k].Count < 4)
				{
					flag = false;
					break;
				}
			}
		}
		if (flag)
		{
			if (!GameManager.Instance.IsMultiplayer())
			{
				CardCraftManager.Instance.ChallengeReadySetButton(state: true);
				CardCraftManager.Instance.EnableChallengeReadyButton(state: true);
			}
			else
			{
				CardCraftManager.Instance.EnableChallengeReadyButton(state: true);
			}
		}
		else
		{
			CardCraftManager.Instance.EnableChallengeReadyButton(state: false);
			CardCraftManager.Instance.ChallengeReadySetButton(state: false);
		}
	}

	private void FinishObeliskDraft()
	{
		AtOManager.Instance.SetPlayerPerksChallenge(perkDrafted);
		AtOManager.Instance.FinishObeliskDraft();
	}

	public void Ready()
	{
		if (!GameManager.Instance.IsMultiplayer())
		{
			FinishObeliskDraft();
			return;
		}
		if (manualReadyCo != null)
		{
			StopCoroutine(manualReadyCo);
		}
		statusReady = !statusReady;
		NetworkManager.Instance.SetManualReady(statusReady);
		if (statusReady)
		{
			CardCraftManager.Instance.ChallengeReadySetButton(state: true);
			if (NetworkManager.Instance.IsMaster())
			{
				manualReadyCo = StartCoroutine(CheckForAllManualReady());
			}
		}
		else
		{
			CardCraftManager.Instance.ChallengeReadySetButton(state: false);
		}
	}

	public void SetWaitingPlayersText(string msg)
	{
		CardCraftManager.Instance.SetWaitingPlayerTextChallenge(msg);
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
		FinishObeliskDraft();
	}

	private void AddToDictDT(string dt, int index)
	{
		if (!(dt != ""))
		{
			return;
		}
		dt = dt.ToLower();
		if (!dictBonus.ContainsKey(dt))
		{
			dictBonus.Add(dt, 0);
		}
		if (index <= -1)
		{
			return;
		}
		dictBonus[dt]++;
		if (currentHeroIndex == index)
		{
			if (!dictBonusSingle.ContainsKey(dt))
			{
				dictBonusSingle.Add(dt, 0);
			}
			dictBonusSingle[dt]++;
		}
	}

	private void AddToDictAU(string au, int index)
	{
		if (!(au != ""))
		{
			return;
		}
		au = au.ToLower();
		if (!dictAura.ContainsKey(au))
		{
			dictAura.Add(au, 0);
		}
		if (index <= -1)
		{
			return;
		}
		dictAura[au]++;
		if (currentHeroIndex == index)
		{
			if (!dictAuraSingle.ContainsKey(au))
			{
				dictAuraSingle.Add(au, 0);
			}
			dictAuraSingle[au]++;
		}
	}

	private void AddToDictEnergy(string value)
	{
		if (int.Parse(value) > 5)
		{
			value = "5";
		}
		if (dictEnergyCost.ContainsKey(value))
		{
			dictEnergyCost[value]++;
		}
		else
		{
			dictEnergyCost.Add(value, 1f);
		}
	}

	private void WriteBonusFullParty()
	{
		dictBonus.Clear();
		dictBonusSingle.Clear();
		dictAura.Clear();
		dictAuraSingle.Clear();
		dictEnergyCost.Clear();
		AddToDictDT("Slashing", -1);
		AddToDictDT("Blunt", -1);
		AddToDictDT("Piercing", -1);
		AddToDictDT("Fire", -1);
		AddToDictDT("Cold", -1);
		AddToDictDT("Lightning", -1);
		AddToDictDT("Mind", -1);
		AddToDictDT("Holy", -1);
		AddToDictDT("Shadow", -1);
		AddToDictAU("block", -1);
		AddToDictAU("shield", -1);
		for (int i = 0; i < 4; i++)
		{
			List<string> cards = AtOManager.Instance.GetHero(i).Cards;
			for (int j = 0; j < cards.Count; j++)
			{
				CardData cardData = Globals.Instance.GetCardData(cards[j], instantiate: false);
				if (i == currentHeroIndex)
				{
					AddToDictEnergy(cardData.EnergyCost.ToString());
				}
				string text = "";
				if (cardData.DamageType != Enums.DamageType.None)
				{
					text = Enum.GetName(typeof(Enums.DamageType), cardData.DamageType);
					AddToDictDT(text, i);
				}
				if (cardData.DamageType2 != Enums.DamageType.None)
				{
					text = Enum.GetName(typeof(Enums.DamageType), cardData.DamageType2);
					AddToDictDT(text, i);
				}
				string text2 = "";
				if (cardData.Aura != null)
				{
					text2 = cardData.Aura.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.AuraSelf != null)
				{
					text2 = cardData.AuraSelf.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.Aura2 != null)
				{
					text2 = cardData.Aura2.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.AuraSelf2 != null)
				{
					text2 = cardData.AuraSelf2.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.Aura3 != null)
				{
					text2 = cardData.Aura3.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.AuraSelf3 != null)
				{
					text2 = cardData.AuraSelf3.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.Auras != null && cardData.Auras.Length != 0)
				{
					foreach (string item in from x in cardData.Auras.SelectMany((CardData.AuraBuffs x) => new AuraCurseData[2] { x.aura, x.auraSelf })
						where x != null
						select x.Id)
					{
						AddToDictAU(item, i);
					}
				}
				if (cardData.Curse != null)
				{
					text2 = cardData.Curse.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.CurseSelf != null)
				{
					text2 = cardData.CurseSelf.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.Curse2 != null)
				{
					text2 = cardData.Curse2.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.CurseSelf2 != null)
				{
					text2 = cardData.CurseSelf2.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.Curse3 != null)
				{
					text2 = cardData.Curse3.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.CurseSelf3 != null)
				{
					text2 = cardData.CurseSelf3.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.Curses == null || cardData.Curses.Length == 0)
				{
					continue;
				}
				foreach (string item2 in from x in cardData.Curses.SelectMany((CardData.CurseDebuffs x) => new AuraCurseData[2] { x.curse, x.curseSelf })
					where x != null
					select x.Id)
				{
					AddToDictAU(item2, i);
				}
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<sprite name=cards><size=+.4>");
		stringBuilder.Append(Texts.Instance.GetText("damageTypes"));
		stringBuilder.Append("</size><br><indent=4>");
		int num = 0;
		foreach (KeyValuePair<string, int> dictBonu in dictBonus)
		{
			if (dictBonu.Value > 0)
			{
				stringBuilder.Append("<mspace=3>");
				if (num > 0)
				{
					stringBuilder.Append("<color=#666><voffset=.4><size=-.8>|</size></voffset></color>");
				}
				stringBuilder.Append("<size=+.2><sprite name=");
				stringBuilder.Append(dictBonu.Key.ToLower());
				stringBuilder.Append("></size><space=1><mspace=1>");
				stringBuilder.Append("<color=#FC0><size=+.2>");
				if (dictBonusSingle.ContainsKey(dictBonu.Key))
				{
					stringBuilder.Append(dictBonusSingle[dictBonu.Key]);
				}
				else
				{
					stringBuilder.Append("0");
				}
				stringBuilder.Append("</size></color>");
				stringBuilder.Append("/");
				stringBuilder.Append(dictBonu.Value);
				num++;
			}
		}
		stringBuilder.Append("</mspace>");
		CardCraftManager.Instance.cardChallengeBonus.text = stringBuilder.ToString();
		stringBuilder.Clear();
		stringBuilder.Append("<line-height=60%><br><br><line-height=100%>");
		stringBuilder.Append("<indent=0><sprite name=cards><size=+.4>");
		stringBuilder.Append(Texts.Instance.GetText("combatEffects"));
		stringBuilder.Append("</size><br><indent=4>");
		num = 0;
		foreach (KeyValuePair<string, int> item3 in dictAura)
		{
			if (item3.Value > 0)
			{
				stringBuilder.Append("<nobr>");
				stringBuilder.Append("<mspace=3>");
				if (num % 8 == 0)
				{
					stringBuilder.Append("<br>");
				}
				else if (num > 0)
				{
					stringBuilder.Append("<color=#666><voffset=.4><size=-.8>|</size></voffset></color>");
				}
				stringBuilder.Append("<size=+.2><sprite name=");
				stringBuilder.Append(item3.Key.ToLower());
				stringBuilder.Append("></size><space=1><mspace=1>");
				stringBuilder.Append("<color=#FC0><size=+.2>");
				if (dictAuraSingle.ContainsKey(item3.Key))
				{
					stringBuilder.Append(dictAuraSingle[item3.Key]);
				}
				else
				{
					stringBuilder.Append("0");
				}
				stringBuilder.Append("</size></color>");
				stringBuilder.Append("/");
				stringBuilder.Append(item3.Value);
				stringBuilder.Append("</nobr>");
				num++;
			}
		}
		stringBuilder.Append("</mspace>");
		CardCraftManager.Instance.cardChallengeBonus.text += stringBuilder.ToString();
	}

	private void WriteBonus()
	{
		dictBonus.Clear();
		dictBonusSingle.Clear();
		dictAura.Clear();
		dictAuraSingle.Clear();
		dictEnergyCost.Clear();
		AddToDictDT("Slashing", -1);
		AddToDictDT("Blunt", -1);
		AddToDictDT("Piercing", -1);
		AddToDictDT("Fire", -1);
		AddToDictDT("Cold", -1);
		AddToDictDT("Lightning", -1);
		AddToDictDT("Mind", -1);
		AddToDictDT("Holy", -1);
		AddToDictDT("Shadow", -1);
		AddToDictAU("heal", -1);
		AddToDictAU("energy", -1);
		AddToDictAU("block", -1);
		AddToDictAU("shield", -1);
		for (int i = 0; i < 4; i++)
		{
			if (i != currentHeroIndex)
			{
				continue;
			}
			List<string> cards = AtOManager.Instance.GetHero(i).Cards;
			for (int j = 0; j < cards.Count; j++)
			{
				CardData cardData = Globals.Instance.GetCardData(cards[j], instantiate: false);
				AddToDictEnergy(cardData.EnergyCost.ToString());
				string text = "";
				if (cardData.DamageType != Enums.DamageType.None)
				{
					text = Enum.GetName(typeof(Enums.DamageType), cardData.DamageType);
					AddToDictDT(text, i);
				}
				if (cardData.DamageType2 != Enums.DamageType.None)
				{
					text = Enum.GetName(typeof(Enums.DamageType), cardData.DamageType2);
					AddToDictDT(text, i);
				}
				string text2 = "";
				if (cardData.EnergyRecharge > 0)
				{
					text2 = "energy";
					AddToDictAU(text2, i);
				}
				if (cardData.Heal > 0)
				{
					text2 = "heal";
					AddToDictAU(text2, i);
				}
				if (cardData.Aura != null)
				{
					text2 = cardData.Aura.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.AuraSelf != null)
				{
					text2 = cardData.AuraSelf.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.Aura2 != null)
				{
					text2 = cardData.Aura2.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.AuraSelf2 != null)
				{
					text2 = cardData.AuraSelf2.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.Aura3 != null)
				{
					text2 = cardData.Aura3.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.AuraSelf3 != null)
				{
					text2 = cardData.AuraSelf3.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.Auras != null && cardData.Auras.Length != 0)
				{
					foreach (string item in from x in cardData.Auras.SelectMany((CardData.AuraBuffs x) => new AuraCurseData[2] { x.aura, x.auraSelf })
						where x != null
						select x.Id)
					{
						AddToDictAU(item, i);
					}
				}
				if (cardData.Curse != null)
				{
					text2 = cardData.Curse.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.CurseSelf != null)
				{
					text2 = cardData.CurseSelf.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.Curse2 != null)
				{
					text2 = cardData.Curse2.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.CurseSelf2 != null)
				{
					text2 = cardData.CurseSelf2.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.Curse3 != null)
				{
					text2 = cardData.Curse3.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.CurseSelf3 != null)
				{
					text2 = cardData.CurseSelf3.Id;
					AddToDictAU(text2, i);
				}
				if (cardData.Curses == null || cardData.Curses.Length == 0)
				{
					continue;
				}
				foreach (string item2 in from x in cardData.Curses.SelectMany((CardData.CurseDebuffs x) => new AuraCurseData[2] { x.curse, x.curseSelf })
					where x != null
					select x.Id)
				{
					AddToDictAU(item2, i);
				}
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<sprite name=cards><size=-.6><color=#FFF>");
		stringBuilder.Append(Texts.Instance.GetText("damageTypes"));
		stringBuilder.Append("</color></size><br><indent=4>");
		int num = 0;
		foreach (KeyValuePair<string, int> dictBonu in dictBonus)
		{
			if (dictBonu.Value > 0)
			{
				stringBuilder.Append("<mspace=2.6>");
				if (num > 0)
				{
					stringBuilder.Append("<color=#666><voffset=.6><size=-1.5>|</size></voffset></color>");
				}
				stringBuilder.Append("<sprite name=");
				stringBuilder.Append(dictBonu.Key.ToLower());
				stringBuilder.Append("><space=1><mspace=1.4>");
				stringBuilder.Append("<color=#FC0>");
				if (dictBonusSingle.ContainsKey(dictBonu.Key))
				{
					stringBuilder.Append(dictBonusSingle[dictBonu.Key]);
				}
				else
				{
					stringBuilder.Append("0");
				}
				stringBuilder.Append("</color>");
				num++;
			}
		}
		stringBuilder.Append("</mspace>");
		CardCraftManager.Instance.cardChallengeBonus.text = stringBuilder.ToString();
		stringBuilder.Clear();
		stringBuilder.Append("<line-height=70%><br><br></line-height><indent=0><sprite name=cards><size=-.6><color=#FFF>");
		stringBuilder.Append(Texts.Instance.GetText("combatEffects"));
		stringBuilder.Append("</color></size><br><indent=4>");
		num = 0;
		foreach (KeyValuePair<string, int> item3 in dictAura)
		{
			if (item3.Value <= 0)
			{
				continue;
			}
			stringBuilder.Append("<nobr>");
			stringBuilder.Append("<mspace=2.6>");
			if (num > 0)
			{
				if (num % 14 == 0)
				{
					stringBuilder.Append("<br>");
				}
				else
				{
					stringBuilder.Append("<color=#666><voffset=.6><size=-1.5>|</size></voffset></color>");
				}
			}
			stringBuilder.Append("<sprite name=");
			stringBuilder.Append(item3.Key.ToLower());
			stringBuilder.Append("><space=1><mspace=1.4>");
			stringBuilder.Append("<color=#FC0>");
			if (dictAuraSingle.ContainsKey(item3.Key))
			{
				stringBuilder.Append(dictAuraSingle[item3.Key]);
			}
			else
			{
				stringBuilder.Append("0");
			}
			stringBuilder.Append("</color>");
			stringBuilder.Append("</nobr>");
			num++;
		}
		stringBuilder.Append("</mspace>");
		CardCraftManager.Instance.cardChallengeBonus.text += stringBuilder.ToString();
	}
}
