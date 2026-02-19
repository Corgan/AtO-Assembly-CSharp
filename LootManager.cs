using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class LootManager : MonoBehaviour
{
	private PhotonView photonView;

	public Transform sceneCamera;

	public CharacterWindowUI characterWindowUI;

	public Transform cardContainer;

	public CharacterLoot[] characterLootArray = new CharacterLoot[4];

	public Transform[] characterPotraitArray = new Transform[4];

	private List<string> lootedItems = new List<string>();

	private string lootListId = "";

	private int goldQuantity;

	private int goldSelected;

	private int activeCharacter = -1;

	private Dictionary<string, CardItem> cardCI = new Dictionary<string, CardItem>();

	private Dictionary<string, string> cardType = new Dictionary<string, string>();

	private List<int> characterOrder;

	public BotonGeneric botonGold;

	public TMP_Text subtitle;

	public TMP_Text description;

	private List<int> listCharacterOrder;

	private bool isMyLoot;

	private bool[] looted = new bool[4];

	private bool finishLoot;

	private bool reseting;

	public Transform buttonRestart;

	private string teamAtOToJson;

	private string[] keyListGold;

	private int[] valueListGold;

	private int playerGold;

	private string[] keyListDust;

	private int[] valueListDust;

	private int playerDust;

	private int totalGoldGained;

	private int totalDustGained;

	private int expGained;

	private int atoGoldGained;

	private int atoDustGained;

	private int clientGold;

	private int clientDust;

	public int controllerHorizontalIndex = -1;

	private Vector2 warpPosition = Vector2.zero;

	private List<Transform> _controllerList = new List<Transform>();

	public static LootManager Instance { get; private set; }

	public bool IsMyLoot
	{
		get
		{
			return isMyLoot;
		}
		set
		{
			isMyLoot = value;
		}
	}

	private void Awake()
	{
		if (GameManager.Instance == null)
		{
			SceneStatic.LoadByName("Loot");
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
		NetworkManager.Instance.StartStopQueue(state: true);
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
			totalGoldGained = AtOManager.Instance.totalGoldGained;
			totalDustGained = AtOManager.Instance.totalDustGained;
			atoGoldGained = PlayerManager.Instance.GoldGained;
			atoDustGained = PlayerManager.Instance.DustGained;
			expGained = PlayerManager.Instance.ExpGained;
		}
		else
		{
			clientGold = AtOManager.Instance.GetPlayerGold();
			clientDust = AtOManager.Instance.GetPlayerDust();
		}
		for (int i = 0; i < 4; i++)
		{
			characterLootArray[i].gameObject.SetActive(value: false);
			looted[i] = false;
		}
		description.transform.gameObject.SetActive(value: false);
		botonGold.transform.gameObject.SetActive(value: false);
		AtOManager.Instance.ClearReroll();
		StartCoroutine(SetLoot());
	}

	private IEnumerator SetLoot()
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			if (NetworkManager.Instance.IsMaster())
			{
				while (!NetworkManager.Instance.AllPlayersReady("doLoot"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked doLoot");
				}
				NetworkManager.Instance.PlayersNetworkContinue("doLoot");
				lootListId = AtOManager.Instance.GetLootListId();
				UnityEngine.Random.InitState((AtOManager.Instance.GetGameId() + "_" + AtOManager.Instance.mapVisitedNodes.Count + "_" + AtOManager.Instance.currentMapNode + "_" + lootListId).GetDeterministicHashCode());
				listCharacterOrder = AtOManager.Instance.GetLootCharacterOrder();
				SetLootOrder();
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("doLoot", status: true);
				NetworkManager.Instance.SetWaitingSyncro("setloot", status: true);
				NetworkManager.Instance.SetStatusReady("doLoot");
				while (NetworkManager.Instance.WaitingSyncro["doLoot"])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("doLoot, we can continue!");
				}
			}
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("SetWaitingSyncro setloot");
			}
			if (NetworkManager.Instance.IsMaster())
			{
				while (!NetworkManager.Instance.AllPlayersReady("setloot"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked setloot");
				}
				NetworkManager.Instance.PlayersNetworkContinue("setloot");
			}
			else
			{
				while (NetworkManager.Instance.WaitingSyncro["setloot"])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("setloot, we can continue!");
				}
			}
			SetCharacters();
		}
		else
		{
			lootListId = AtOManager.Instance.GetLootListId();
			characterOrder = new List<int>();
			characterOrder.Add(0);
			characterOrder.Add(1);
			characterOrder.Add(2);
			characterOrder.Add(3);
			SetCharacters();
		}
		GameManager.Instance.SceneLoaded();
	}

	private void SetLootOrder()
	{
		characterOrder = new List<int>();
		characterOrder.Add(listCharacterOrder[0]);
		characterOrder.Add(listCharacterOrder[1]);
		characterOrder.Add(listCharacterOrder[2]);
		characterOrder.Add(listCharacterOrder[3]);
		photonView.RPC("NET_ShareLootIdAndOrder", RpcTarget.Others, lootListId, (byte)characterOrder[0], (byte)characterOrder[1], (byte)characterOrder[2], (byte)characterOrder[3]);
	}

	[PunRPC]
	private void NET_ShareLootIdAndOrder(string _lootListId, byte char0, byte char1, byte char2, byte char3)
	{
		lootListId = _lootListId;
		characterOrder = new List<int>();
		characterOrder.Add(char0);
		characterOrder.Add(char1);
		characterOrder.Add(char2);
		characterOrder.Add(char3);
		NetworkManager.Instance.SetStatusReady("setloot");
	}

	private void SetCharacters()
	{
		for (int i = 0; i < 4; i++)
		{
			int heroIndex = characterOrder[i];
			characterLootArray[i].AssignHero(heroIndex);
		}
		ContinueSetCharacters();
	}

	private void ContinueSetCharacters()
	{
		StartCoroutine(ShowCharacters());
		StartCoroutine(ShowItemsForLoot(lootListId));
	}

	public void ChangeCharacter(int _heroIndex)
	{
		if (!GameManager.Instance.IsMultiplayer() && activeCharacter != -1 && !looted[_heroIndex])
		{
			activeCharacter = _heroIndex;
			ActivateCharacter(activeCharacter);
		}
	}

	private void ActivateCharacter(int charToActivate)
	{
		StartCoroutine(ActivateCharacterCo(charToActivate));
	}

	private IEnumerator ActivateCharacterCo(int charToActivate)
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			yield return Globals.Instance.WaitForSeconds(0.1f);
			if (NetworkManager.Instance.IsMaster())
			{
				while (!NetworkManager.Instance.AllPlayersReady("activateCharacter" + charToActivate))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked activateCharacter" + charToActivate);
				}
				NetworkManager.Instance.PlayersNetworkContinue("activateCharacter" + charToActivate);
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("activateCharacter" + charToActivate, status: true);
				NetworkManager.Instance.SetStatusReady("activateCharacter" + charToActivate);
				while (NetworkManager.Instance.WaitingSyncro["activateCharacter" + charToActivate])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("activateCharacter" + charToActivate + ", we can continue!");
				}
			}
		}
		activeCharacter = charToActivate;
		bool flag = false;
		while (!flag)
		{
			Hero hero = AtOManager.Instance.GetHero(characterOrder[activeCharacter]);
			if (hero == null || hero.HeroData == null)
			{
				activeCharacter++;
			}
			else
			{
				flag = true;
			}
		}
		for (int i = 0; i < 4; i++)
		{
			if (i == activeCharacter)
			{
				Hero hero2 = AtOManager.Instance.GetHero(characterOrder[i]);
				if (hero2 == null || hero2.HeroData == null)
				{
					continue;
				}
				characterLootArray[i].Activate(state: true);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(Texts.Instance.GetText("itemRewardsChosing"));
				stringBuilder.Append(" <size=+.5><color=#FFF>");
				stringBuilder.Append(hero2.HeroData.HeroSubClass.CharacterName);
				stringBuilder.Append("</color></size>");
				subtitle.text = stringBuilder.ToString();
				if (GameManager.Instance.IsMultiplayer())
				{
					if (hero2.Owner == NetworkManager.Instance.GetPlayerNick())
					{
						isMyLoot = true;
					}
					else
					{
						isMyLoot = false;
					}
					yield return Globals.Instance.WaitForSeconds(0.2f);
					EnableLoot(isMyLoot);
				}
				else
				{
					isMyLoot = true;
				}
			}
			else
			{
				characterLootArray[i].Activate(state: false);
			}
		}
	}

	private IEnumerator ShowCharacters()
	{
		for (int i = 0; i < 4; i++)
		{
			if (AtOManager.Instance.GetHero(characterOrder[i]) != null && AtOManager.Instance.GetHero(characterOrder[i]).HeroData != null)
			{
				yield return Globals.Instance.WaitForSeconds(0.1f);
				characterLootArray[i].gameObject.SetActive(value: true);
			}
		}
	}

	private IEnumerator ShowItemsForLoot(string itemListId)
	{
		yield return Globals.Instance.WaitForSeconds(0.1f);
		new List<string>();
		List<string> itemList = AtOManager.Instance.GetItemList(itemListId);
		if (itemList == null)
		{
			yield break;
		}
		goldQuantity = Globals.Instance.GetLootData(itemListId).GoldQuantity;
		if (MadnessManager.Instance.IsMadnessTraitActive("poverty") || AtOManager.Instance.IsChallengeTraitActive("poverty"))
		{
			if (!GameManager.Instance.IsObeliskChallenge())
			{
				goldQuantity -= Functions.FuncRoundToInt((float)goldQuantity * 0.5f);
			}
			else
			{
				goldQuantity -= Functions.FuncRoundToInt((float)goldQuantity * 0.3f);
			}
		}
		description.transform.gameObject.SetActive(value: true);
		botonGold.transform.gameObject.SetActive(value: true);
		botonGold.SetText(goldQuantity.ToString());
		int position = 0;
		Functions.FuncRoundToInt(itemList.Count / 2);
		for (int i = 0; i < itemList.Count; i++)
		{
			string text = itemList[i] + "_" + i;
			GameObject obj = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, cardContainer);
			CardItem component = obj.GetComponent<CardItem>();
			cardCI.Add(text, component);
			obj.name = text;
			component.SetCard(itemList[i], deckScale: false);
			component.DisableCollider();
			Vector3 cardPosition = Functions.GetCardPosition(new Vector3(-2f, -1.9f, 0f), position, itemList.Count);
			float x = cardPosition.x;
			float y = cardPosition.y;
			component.transform.position = new Vector3(x, y, 0f);
			component.DoReward(fromReward: false, fromEvent: false, fromLoot: true);
			component.SetDestination(new Vector3(x, y, 0f));
			component.SetLocalScale(new Vector3(1.15f, 1.15f, 1f));
			component.SetDestinationLocalScale(1.15f);
			component.cardmakebig = true;
			component.cardoutsideloot = true;
			component.lootId = text;
			CardData cardData = Globals.Instance.GetCardData(itemList[i]);
			if (cardData != null)
			{
				if (cardData.CardType == Enums.CardType.Weapon)
				{
					cardType.Add(text, "Weapon");
				}
				else if (cardData.CardType == Enums.CardType.Armor)
				{
					cardType.Add(text, "Armor");
				}
				else if (cardData.CardType == Enums.CardType.Jewelry)
				{
					cardType.Add(text, "Jewelry");
				}
				else if (cardData.CardType == Enums.CardType.Accesory)
				{
					cardType.Add(text, "Accesory");
				}
				else if (cardData.CardType == Enums.CardType.Pet)
				{
					cardType.Add(text, "Pet");
				}
			}
			if (GameManager.Instance.IsMultiplayer() && !isMyLoot)
			{
				component.ShowDisableReward();
			}
			PlayerManager.Instance.CardUnlock(itemList[i], save: false, component);
			position++;
			yield return Globals.Instance.WaitForSeconds(0.1f);
		}
		ActivateCharacter(0);
	}

	private void EnableLoot(bool state)
	{
		if (state)
		{
			botonGold.Enable();
		}
		else
		{
			botonGold.Disable();
		}
		foreach (KeyValuePair<string, CardItem> item in cardCI)
		{
			bool flag = state;
			if (lootedItems.Contains(item.Key))
			{
				flag = false;
			}
			item.Value.ShowDisable(!flag);
			item.Value.CreateColliderAdjusted();
		}
	}

	public void Looted(string lootId)
	{
		if (activeCharacter >= 4 || lootedItems.Contains(lootId))
		{
			return;
		}
		looted[activeCharacter] = true;
		lootedItems.Add(lootId);
		cardCI[lootId].ShowDisableReward();
		Transform theTransform = cardCI[lootId].transform;
		string itemToAddName = lootId.Split('_')[0];
		StartCoroutine(LootTrail(activeCharacter, lootId, theTransform));
		AtOManager.Instance.AddItemToHero(characterOrder[activeCharacter], itemToAddName);
		characterPotraitArray[activeCharacter].gameObject.SetActive(value: true);
		characterPotraitArray[activeCharacter].GetComponent<SpriteRenderer>().sprite = AtOManager.Instance.GetHero(characterOrder[activeCharacter]).HeroData.HeroSubClass.SpriteSpeed;
		characterPotraitArray[activeCharacter].position = cardCI[lootId].transform.position + new Vector3(0.85f, 0.85f, 0f);
		activeCharacter = 4;
		if (GameManager.Instance.IsMultiplayer())
		{
			if (isMyLoot)
			{
				photonView.RPC("NET_Looted", RpcTarget.Others, lootId);
				isMyLoot = false;
				EnableLoot(state: false);
			}
			StartCoroutine(NextCharacterMP());
		}
		else
		{
			NextCharacter();
		}
	}

	[PunRPC]
	private void NET_Looted(string lootId)
	{
		if (!reseting)
		{
			Looted(lootId);
		}
	}

	private IEnumerator LootTrail(int character, string cardId, Transform theTransform)
	{
		HighLight(state: false, cardType[cardId]);
		Transform child = characterLootArray[activeCharacter].transform.GetChild(2);
		if (!(child == null))
		{
			Vector3 to = Vector3.zero;
			if (cardType[cardId] == "Weapon")
			{
				to = child.GetChild(0).transform.position;
			}
			else if (cardType[cardId] == "Armor")
			{
				to = child.GetChild(1).transform.position;
			}
			else if (cardType[cardId] == "Jewelry")
			{
				to = child.GetChild(2).transform.position;
			}
			else if (cardType[cardId] == "Accesory")
			{
				to = child.GetChild(3).transform.position;
			}
			else if (cardType[cardId] == "Pet")
			{
				to = child.GetChild(4).transform.position;
			}
			GameManager.Instance.GenerateParticleTrail(0, theTransform.position, to);
			yield return Globals.Instance.WaitForSeconds(0.15f);
			characterLootArray[character].ShowItems();
		}
	}

	public void LootGold(bool comingFromNet = false)
	{
		if ((!comingFromNet && !isMyLoot) || looted == null || activeCharacter > looted.Length || activeCharacter >= 4)
		{
			return;
		}
		looted[activeCharacter] = true;
		Hero hero = AtOManager.Instance.GetHero(characterOrder[activeCharacter]);
		characterPotraitArray[activeCharacter].gameObject.SetActive(value: true);
		characterPotraitArray[activeCharacter].GetComponent<SpriteRenderer>().sprite = hero.HeroData.HeroSubClass.SpriteSpeed;
		characterPotraitArray[activeCharacter].position = botonGold.transform.position + new Vector3(-0.35f + 0.8f * (float)goldSelected, -1f, 0f);
		goldSelected++;
		activeCharacter = 4;
		if (GameManager.Instance.IsMultiplayer())
		{
			if (isMyLoot)
			{
				photonView.RPC("NET_LootGold", RpcTarget.Others);
				isMyLoot = false;
				EnableLoot(state: false);
			}
			if (NetworkManager.Instance.IsMaster())
			{
				AtOManager.Instance.GivePlayer(0, goldQuantity, hero.Owner);
			}
			StartCoroutine(NextCharacterMP());
		}
		else
		{
			AtOManager.Instance.GivePlayer(0, goldQuantity, hero.Owner);
			NextCharacter();
		}
	}

	[PunRPC]
	private void NET_LootGold()
	{
		if (!reseting)
		{
			LootGold(comingFromNet: true);
		}
	}

	private IEnumerator NextCharacterMP()
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			yield return Globals.Instance.WaitForSeconds(0.1f);
			if (NetworkManager.Instance.IsMaster())
			{
				while (!NetworkManager.Instance.AllPlayersReady("nextCharacterMP"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("Game ready, Everybody checked nextCharacterMP");
				}
				NetworkManager.Instance.PlayersNetworkContinue("nextCharacterMP");
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("nextCharacterMP", status: true);
				NetworkManager.Instance.SetStatusReady("nextCharacterMP");
				while (NetworkManager.Instance.WaitingSyncro["nextCharacterMP"])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (Globals.Instance.ShowDebug)
				{
					Functions.DebugLogGD("nextCharacterMP, we can continue!");
				}
			}
		}
		NextCharacter();
	}

	private void NextCharacter()
	{
		int charToActivate = -1;
		bool flag = true;
		for (int i = 0; i < 4; i++)
		{
			Hero hero = AtOManager.Instance.GetHero(characterOrder[i]);
			if (hero != null && !(hero.HeroData == null) && !looted[i])
			{
				flag = false;
				charToActivate = i;
				break;
			}
		}
		if (flag)
		{
			StartCoroutine(FinishLoot());
		}
		else
		{
			ActivateCharacter(charToActivate);
		}
	}

	private IEnumerator FinishLoot()
	{
		finishLoot = true;
		buttonRestart.gameObject.SetActive(value: false);
		botonGold.Disable();
		SaveManager.SavePlayerData();
		if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
		{
			yield return Globals.Instance.WaitForSeconds(1f);
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("Load finish loot", "trace");
			}
			GameManager.Instance.SetMaskLoading();
			AtOManager.Instance.FinishLoot();
		}
	}

	public void HighLight(bool state, string itemType)
	{
		if (activeCharacter < 4 && activeCharacter > -1 && characterLootArray != null && activeCharacter < characterLootArray.Length && characterLootArray[activeCharacter] != null)
		{
			switch (itemType)
			{
			case "Weapon":
				characterLootArray[activeCharacter].item0.DoHover(state);
				break;
			case "Armor":
				characterLootArray[activeCharacter].item1.DoHover(state);
				break;
			case "Jewelry":
				characterLootArray[activeCharacter].item2.DoHover(state);
				break;
			case "Accesory":
				characterLootArray[activeCharacter].item3.DoHover(state);
				break;
			case "Pet":
				characterLootArray[activeCharacter].item4.DoHover(state);
				break;
			}
		}
	}

	public void ShowCharacterWindow(string type = "", bool isHero = true, int characterIndex = -1)
	{
		characterWindowUI.Show(type, characterIndex);
	}

	public void ShowDeck(int auxInt)
	{
		characterWindowUI.Show("deck", auxInt);
	}

	public void RestartLoot()
	{
		if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
		{
			reseting = true;
			StartCoroutine(RestartLootMaster());
			return;
		}
		buttonRestart.gameObject.SetActive(value: false);
		string playerNickReal = NetworkManager.Instance.GetPlayerNickReal(NetworkManager.Instance.GetPlayerNick());
		photonView.RPC("NET_RestartLoot", RpcTarget.MasterClient, playerNickReal);
	}

	private IEnumerator RestartLootMaster()
	{
		if (!finishLoot)
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
			AtOManager.Instance.totalGoldGained = totalGoldGained;
			AtOManager.Instance.totalDustGained = totalDustGained;
			PlayerManager.Instance.GoldGained = atoGoldGained;
			PlayerManager.Instance.DustGained = atoDustGained;
			PlayerManager.Instance.ExpGained = expGained;
			AtOManager.Instance.DoLoot(lootListId);
		}
	}

	[PunRPC]
	private void NET_RestartLoot(string _nick)
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
		AtOManager.Instance.SetPlayerGold(clientGold);
		AtOManager.Instance.SetPlayerDust(clientDust);
	}

	private void WantToRestart()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(WantToRestart));
		if (AlertManager.Instance.GetConfirmAnswer())
		{
			RestartLoot();
		}
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false, int absolutePosition = -1)
	{
		_controllerList.Clear();
		_controllerList.Add(buttonRestart);
		_controllerList.Add(botonGold.transform);
		for (int i = 0; i < 4; i++)
		{
			if (!Functions.TransformIsVisible(characterLootArray[i].transform))
			{
				continue;
			}
			_controllerList.Add(characterLootArray[i].transform.GetChild(3).transform);
			_controllerList.Add(characterLootArray[i].transform.GetChild(7).transform);
			foreach (Transform item2 in characterLootArray[i].transform.GetChild(2).transform)
			{
				if (item2.GetComponent<BoxCollider2D>().enabled)
				{
					_controllerList.Add(item2);
				}
			}
		}
		foreach (Transform item3 in cardContainer)
		{
			_controllerList.Add(item3);
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
