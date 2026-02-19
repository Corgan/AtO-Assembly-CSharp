using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardPlayerPairsManager : MonoBehaviour
{
	private PhotonView photonView;

	public CharacterWindowUI characterWindowUI;

	public SideCharacters sideCharacters;

	public TMP_Text playerTurnText;

	public Transform cardContainer;

	public Transform onlyMaster;

	public Transform selection;

	public Transform finishBlock;

	public BotonGeneric finishButton;

	private List<string> cardList;

	private List<CardItem> cards;

	private List<Vector3> positions;

	private List<bool> moving;

	public Transform botShuffle;

	private Dictionary<string, int> playerSelectedCard = new Dictionary<string, int>();

	private CardPlayerPairsPackData _pack;

	private int cardSelected1 = -1;

	private int cardSelected2 = -1;

	private List<int> cardsSelectedList = new List<int>();

	private bool canSelect;

	private int currentRound = -1;

	private int currentRoundGlobal = -1;

	private int maxRounds = 8;

	private int[] orderArray;

	public SpriteRenderer[] charSpr;

	private Hero[] theTeam;

	private Vector3 spriteBig = new Vector3(0.8f, 0.8f, 1f);

	private Vector3 spriteSmall = new Vector3(0.6f, 0.6f, 1f);

	public int controllerHorizontalIndex = -1;

	private Vector2 warpPosition = Vector2.zero;

	private List<Transform> _controllerList = new List<Transform>();

	public static CardPlayerPairsManager Instance { get; private set; }

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
			UnityEngine.Object.Destroy(this);
		}
		GameManager.Instance.SetCamera();
		photonView = PhotonView.Get(this);
		NetworkManager.Instance.StartStopQueue(state: true);
	}

	public bool CanExit()
	{
		return botShuffle.gameObject.activeSelf;
	}

	private void Start()
	{
		theTeam = AtOManager.Instance.GetTeam();
		new List<Hero>();
		List<int> list = new List<int>();
		for (int i = 0; i < 4; i++)
		{
			if (theTeam[i] != null && theTeam[i].HeroData != null)
			{
				list.Add(i);
			}
		}
		if (list.Count < 4)
		{
			list = list.ShuffleList();
			int num = 0;
			while (list.Count < 4)
			{
				list.Add(list[num]);
				num++;
			}
		}
		orderArray = new int[4];
		for (int j = 0; j < 4; j++)
		{
			orderArray[j] = list[j];
		}
		selection.gameObject.SetActive(value: false);
		finishBlock.gameObject.SetActive(value: false);
		StartCoroutine(StartSync());
	}

	private IEnumerator StartSync()
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			Debug.Log("WaitingSyncro cardplayerpairs");
			if (NetworkManager.Instance.IsMaster())
			{
				while (!NetworkManager.Instance.AllPlayersReady("cardplayerpairs"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				Functions.DebugLogGD("Game ready, Everybody checked cardplayerpairs");
				NetworkManager.Instance.PlayersNetworkContinue("cardplayerpairs");
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("cardplayerpairs", status: true);
				NetworkManager.Instance.SetStatusReady("cardplayerpairs");
				while (NetworkManager.Instance.WaitingSyncro["cardplayerpairs"])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				Functions.DebugLogGD("cardplayerpairs, we can continue!");
			}
		}
		GameManager.Instance.SceneLoaded();
		sideCharacters.EnableAll();
		botShuffle.GetComponent<BotonGeneric>().Disable();
		if (!GameManager.Instance.IsMultiplayer())
		{
			onlyMaster.gameObject.SetActive(value: false);
		}
		UnityEngine.Random.InitState(string.Concat(AtOManager.Instance.currentMapNode + AtOManager.Instance.GetGameId(), MapManager.Instance.GetRandomString()).GetDeterministicHashCode());
		SetCards();
		NextRound(initGame: true);
	}

	private Vector3 GetPosition(int _index, bool _initial)
	{
		float num = 0f;
		float num2 = 0f;
		num = Mathf.Ceil(_index / 4);
		num2 = _index % 4;
		return new Vector3(-3.3f + 2.2f * num2, -3.2f + num * 3f, 0f);
	}

	private void PreFinishGame()
	{
		if (cardSelected1 != -1)
		{
			cards[cardSelected1].ShowDisable(state: true);
			cards[cardSelected2].ShowDisable(state: true);
		}
		for (int i = 0; i < cards.Count; i++)
		{
			if (!cardsSelectedList.Contains(i))
			{
				cards[i].DoReward(fromReward: false, fromEvent: false, fromLoot: true, selectable: false);
				cards[i].SetLocalScale(new Vector3(1f, 1f, 1f));
				cards[i].SetDestinationLocalScale(1f);
				cards[i].ShowDisable(state: true);
			}
		}
		selection.gameObject.SetActive(value: false);
		finishButton.Enable();
	}

	public void FinishPairGame()
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			if (NetworkManager.Instance.IsMaster())
			{
				photonView.RPC("NET_FinishSelection", RpcTarget.All);
			}
		}
		else
		{
			FinishSelection();
		}
	}

	private void NextRound(bool initGame = false)
	{
		currentRound++;
		currentRoundGlobal = currentRound / 2;
		if (currentRound / 2 >= maxRounds)
		{
			PreFinishGame();
			return;
		}
		if (currentRound % 4 == 0)
		{
			SetCharacterOrder();
		}
		if (currentRound % 2 == 0)
		{
			finishButton.Enable();
		}
		if (!initGame)
		{
			ShowCurrentCharacter();
			canSelect = true;
		}
	}

	private void SetCharacterOrder()
	{
		orderArray = Functions.ShuffleArray(orderArray);
		for (int i = 0; i < 4; i++)
		{
			charSpr[i].sprite = theTeam[orderArray[i]].HeroData.HeroSubClass.SpriteSpeed;
		}
	}

	private void ShowCurrentCharacter()
	{
		for (int i = 0; i < 4; i++)
		{
			if (i != currentRound % 4)
			{
				charSpr[i].transform.localScale = spriteSmall;
				charSpr[i].color = Globals.Instance.ColorColor["grey"];
			}
			else
			{
				charSpr[i].transform.localScale = spriteBig;
				charSpr[i].color = Globals.Instance.ColorColor["white"];
			}
		}
		ShowPlayerName();
	}

	private void SetCards()
	{
		cardList = new List<string>();
		_pack = AtOManager.Instance.cardPlayerPairsPackData;
		if (_pack != null)
		{
			if (_pack.Card0 != null)
			{
				cardList.Add(_pack.Card0.Id);
				cardList.Add(_pack.Card0.Id);
			}
			if (_pack.Card1 != null)
			{
				cardList.Add(_pack.Card1.Id);
				cardList.Add(_pack.Card1.Id);
			}
			if (_pack.Card2 != null)
			{
				cardList.Add(_pack.Card2.Id);
				cardList.Add(_pack.Card2.Id);
			}
			if (_pack.Card3 != null)
			{
				cardList.Add(_pack.Card3.Id);
				cardList.Add(_pack.Card3.Id);
			}
			if (_pack.Card4 != null)
			{
				cardList.Add(_pack.Card4.Id);
				cardList.Add(_pack.Card4.Id);
			}
			if (_pack.Card5 != null)
			{
				cardList.Add(_pack.Card5.Id);
				cardList.Add(_pack.Card5.Id);
			}
		}
		if (cardList.Count < 12)
		{
			FinishPairGame();
			return;
		}
		cardList.Reverse();
		cards = new List<CardItem>();
		positions = new List<Vector3>();
		moving = new List<bool>();
		for (int i = 0; i < cardList.Count; i++)
		{
			moving.Add(item: false);
		}
		StartCoroutine(SetCardsCo());
	}

	private IEnumerator SetCardsCo()
	{
		for (int i = 0; i < cardList.Count; i++)
		{
			string text = cardList[i];
			GameObject obj = UnityEngine.Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, cardContainer);
			CardItem component = obj.GetComponent<CardItem>();
			component.DisableCollider();
			cards.Add(component);
			obj.name = text;
			component.SetCard(cardList[i], deckScale: false);
			Vector3 position = GetPosition(i, _initial: true);
			float x = position.x;
			float y = position.y;
			positions.Add(position);
			component.transform.localPosition = new Vector3(x, y, 0f);
			component.DoReward(fromReward: false, fromEvent: false, fromLoot: true, selectable: false);
			component.SetDestination(new Vector3(x, y, 0f));
			component.SetLocalScale(new Vector3(1f, 1f, 1f));
			component.SetDestinationLocalScale(1f);
			component.CardPlayerIndex = i;
			component.GetComponent<Floating>().enabled = true;
			component.HideLock();
			yield return Globals.Instance.WaitForSeconds(0.025f);
		}
		if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
		{
			botShuffle.GetComponent<BotonGeneric>().Enable();
		}
	}

	public void Shuffle(bool fromNet = false)
	{
		if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster() && !fromNet)
		{
			return;
		}
		botShuffle.gameObject.SetActive(value: false);
		if (GameManager.Instance.IsMultiplayer())
		{
			if (NetworkManager.Instance.IsMaster())
			{
				photonView.RPC("NET_Shuffle", RpcTarget.Others);
			}
			onlyMaster.gameObject.SetActive(value: false);
		}
		UnityEngine.Random.InitState(string.Concat(AtOManager.Instance.currentMapNode + AtOManager.Instance.GetGameId(), MapManager.Instance.GetRandomString()).GetDeterministicHashCode());
		StartCoroutine(ShuffleCo());
	}

	[PunRPC]
	private void NET_Shuffle()
	{
		Shuffle(fromNet: true);
	}

	private IEnumerator ShuffleCo()
	{
		for (int i = 0; i < cards.Count; i++)
		{
			Vector3 position = GetPosition(i, _initial: true);
			positions[i] = position;
			cards[i].GetComponent<Floating>().enabled = false;
			cards[i].cardrevealed = true;
			cards[i].DisableCollider();
		}
		yield return Globals.Instance.WaitForSeconds(0.1f);
		for (int j = 0; j < cards.Count; j++)
		{
			cards[j].TurnBack();
			yield return Globals.Instance.WaitForSeconds(0.025f);
		}
		yield return Globals.Instance.WaitForSeconds(0.2f);
		for (int k = 0; k < cards.Count; k++)
		{
			cards[k].enabled = false;
		}
		yield return Globals.Instance.WaitForSeconds(0.5f);
		Move();
	}

	private void Move()
	{
		StartCoroutine(MoveCo());
	}

	private IEnumerator MoveCo()
	{
		int objectToMove = 0;
		int lastObject = 0;
		int iterations = cardList.Count * 2;
		int speed = 10;
		int speedIncrement = 5;
		int maxSpeed = 60;
		bool[] objMoved = new bool[cardList.Count];
		for (int i = 0; i < objMoved.Length; i++)
		{
			objMoved[i] = false;
		}
		for (int j = 0; j < iterations; j++)
		{
			if (speed < maxSpeed)
			{
				speed += speedIncrement;
			}
			bool flag = false;
			while (!flag)
			{
				objectToMove = UnityEngine.Random.Range(0, objMoved.Length);
				if (objectToMove != lastObject)
				{
					flag = true;
				}
			}
			int destineToMove;
			for (destineToMove = objectToMove; destineToMove == objectToMove; destineToMove = UnityEngine.Random.Range(0, objMoved.Length))
			{
			}
			StartCoroutine(MoveIndividualCo(objectToMove, destineToMove, speed));
			yield return Globals.Instance.WaitForSeconds(0.01f);
			StartCoroutine(MoveIndividualCo(destineToMove, objectToMove, speed));
			lastObject = destineToMove;
			while (moving[destineToMove])
			{
				yield return null;
			}
			objMoved[objectToMove] = true;
			yield return Globals.Instance.WaitForSeconds(0.05f - 0.04f * ((float)j / (float)iterations));
		}
		EnableCards();
	}

	[PunRPC]
	private void NET_SelectCard(int _index)
	{
		SelectCard(_index, _fromNet: true);
	}

	public void SelectCard(int _index, bool _fromNet = false)
	{
		if (!CanClick() && !_fromNet)
		{
			return;
		}
		if (!_fromNet && GameManager.Instance.IsMultiplayer())
		{
			photonView.RPC("NET_SelectCard", RpcTarget.Others, _index);
		}
		canSelect = false;
		for (int i = 0; i < cards.Count; i++)
		{
			if (!cardsSelectedList.Contains(i) && cards[i].CardPlayerIndex == _index)
			{
				cards[i].DoReward(fromReward: false, fromEvent: false, fromLoot: true, selectable: false);
				cards[i].SetLocalScale(new Vector3(1f, 1f, 1f));
				cards[i].SetDestinationLocalScale(1f);
				if (cardSelected1 == -1)
				{
					cardSelected1 = i;
				}
				else
				{
					cardSelected2 = i;
				}
				cardsSelectedList.Add(i);
				break;
			}
		}
		StartCoroutine(SelectCardCo());
	}

	private IEnumerator SelectCardCo()
	{
		finishButton.Disable();
		if (cardSelected2 != -1)
		{
			yield return Globals.Instance.WaitForSeconds(1f);
			if (cards[cardSelected1].CardData.Id == cards[cardSelected2].CardData.Id)
			{
				Debug.Log("MATCH!!!");
				for (int i = 0; i < 4; i++)
				{
					if (theTeam[i] != null && theTeam[i].HeroData != null)
					{
						if (theTeam[i].HeroData.HeroSubClass.SpriteSpeed == charSpr[currentRound % 4 - 1].sprite)
						{
							GiveCardToHero(i, cardSelected1);
						}
						else if (theTeam[i].HeroData.HeroSubClass.SpriteSpeed == charSpr[currentRound % 4].sprite)
						{
							GiveCardToHero(i, cardSelected2);
						}
					}
				}
				CardPlayerPairsManager cardPlayerPairsManager = this;
				CardPlayerPairsManager cardPlayerPairsManager2 = this;
				int num = -1;
				cardPlayerPairsManager2.cardSelected2 = -1;
				cardPlayerPairsManager.cardSelected1 = num;
			}
			else if (currentRoundGlobal + 1 < maxRounds)
			{
				cards[cardSelected1].TurnBack();
				cards[cardSelected2].TurnBack();
				cardsSelectedList.Remove(cardSelected1);
				cardsSelectedList.Remove(cardSelected2);
				yield return Globals.Instance.WaitForSeconds(0.2f);
				int destineToMove = UnityEngine.Random.Range(0, cards.Count);
				int num2 = 0;
				while (destineToMove == cardSelected1 || destineToMove == cardSelected2 || cardsSelectedList.Contains(destineToMove))
				{
					destineToMove = UnityEngine.Random.Range(0, cards.Count);
					num2++;
					if (num2 > 20)
					{
						destineToMove = cardSelected1;
						break;
					}
				}
				if (destineToMove != cardSelected1)
				{
					StartCoroutine(MoveIndividualCo(cardSelected1, destineToMove, 10f));
					yield return Globals.Instance.WaitForSeconds(0.01f);
					StartCoroutine(MoveIndividualCo(destineToMove, cardSelected1, 10f));
					yield return Globals.Instance.WaitForSeconds(0.5f);
				}
				destineToMove = UnityEngine.Random.Range(0, cards.Count);
				num2 = 0;
				while (destineToMove == cardSelected2 || destineToMove == cardSelected1 || cardsSelectedList.Contains(destineToMove))
				{
					destineToMove = UnityEngine.Random.Range(0, cards.Count);
					num2++;
					if (num2 > 20)
					{
						destineToMove = cardSelected2;
						break;
					}
				}
				if (destineToMove != cardSelected2)
				{
					StartCoroutine(MoveIndividualCo(cardSelected2, destineToMove, 10f));
					yield return Globals.Instance.WaitForSeconds(0.01f);
					StartCoroutine(MoveIndividualCo(destineToMove, cardSelected2, 10f));
				}
				CardPlayerPairsManager cardPlayerPairsManager3 = this;
				CardPlayerPairsManager cardPlayerPairsManager4 = this;
				int num = -1;
				cardPlayerPairsManager4.cardSelected2 = -1;
				cardPlayerPairsManager3.cardSelected1 = num;
			}
		}
		if (GameManager.Instance.IsMultiplayer())
		{
			Debug.Log("WaitingSyncro selectCard" + currentRound);
			if (NetworkManager.Instance.IsMaster())
			{
				while (!NetworkManager.Instance.AllPlayersReady("selectCard" + currentRound))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				Functions.DebugLogGD("Game ready, Everybody checked selectCard" + currentRound);
				NetworkManager.Instance.PlayersNetworkContinue("selectCard" + currentRound);
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("selectCard" + currentRound, status: true);
				NetworkManager.Instance.SetStatusReady("selectCard" + currentRound);
				while (NetworkManager.Instance.WaitingSyncro["selectCard" + currentRound])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				Functions.DebugLogGD("selectCard" + currentRound + ", we can continue!");
			}
			yield return Globals.Instance.WaitForSeconds(0.3f);
		}
		else
		{
			yield return Globals.Instance.WaitForSeconds(0.5f);
		}
		for (int j = 0; j < cards.Count; j++)
		{
			if (!cardsSelectedList.Contains(j))
			{
				cards[j].DisableCollider();
			}
		}
		yield return Globals.Instance.WaitForSeconds(0.1f);
		for (int k = 0; k < cards.Count; k++)
		{
			if (!cardsSelectedList.Contains(k))
			{
				cards[k].EnableCollider();
			}
		}
		NextRound();
	}

	private void ShowPlayerName()
	{
		if (!selection.gameObject.activeSelf)
		{
			selection.gameObject.SetActive(value: true);
		}
		if (!finishBlock.gameObject.activeSelf)
		{
			finishBlock.gameObject.SetActive(value: true);
		}
		playerTurnText.text = string.Format(Texts.Instance.GetText("cardPlayerPairsRound"), Mathf.Floor((float)currentRound / 2f) + 1f, maxRounds, Globals.Instance.ClassColor[Enum.GetName(typeof(Enums.HeroClass), theTeam[orderArray[currentRound % 4]].HeroData.HeroClass)], theTeam[orderArray[currentRound % 4]].SourceName);
	}

	private void GiveCardToHero(int heroIndex, int cardIndex)
	{
		CardItem cardItem = cards[cardIndex];
		string id = cardItem.CardData.Id;
		Debug.Log("GiveCardToHero card " + id + " hero " + heroIndex);
		bool flag = true;
		if (cardItem.CardData.GoldGainQuantity != 0)
		{
			flag = false;
			AtOManager.Instance.GivePlayer(0, cardItem.CardData.GoldGainQuantity, theTeam[heroIndex].Owner);
		}
		if (cardItem.CardData.ShardsGainQuantity != 0)
		{
			flag = false;
			AtOManager.Instance.GivePlayer(1, cardItem.CardData.ShardsGainQuantity, theTeam[heroIndex].Owner);
		}
		if (flag)
		{
			AtOManager.Instance.AddCardToHero(heroIndex, id);
		}
		sideCharacters.RefreshCards(heroIndex);
		cardItem.ShowPortrait(theTeam[heroIndex].HeroData.HeroSubClass.SpriteSpeed, 1);
	}

	[PunRPC]
	private void NET_FinishSelection()
	{
		FinishSelection();
	}

	private void FinishSelection()
	{
		StartCoroutine(FinishSelectionCo());
	}

	private IEnumerator FinishSelectionCo()
	{
		finishBlock.gameObject.SetActive(value: false);
		if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
		{
			SaveManager.SavePlayerData();
			yield return Globals.Instance.WaitForSeconds(0.5f);
			AtOManager.Instance.FinishCardPlayer();
		}
	}

	private void EnableCards()
	{
		for (int i = 0; i < cards.Count; i++)
		{
			cards[i].enabled = false;
			cards[i].CreateColliderAdjusted();
			cards[i].GetComponent<Floating>().enabled = true;
		}
		canSelect = true;
		ShowCurrentCharacter();
	}

	public bool CanClick()
	{
		if (GameManager.Instance.IsMultiplayer() && theTeam[orderArray[currentRound % 4]].Owner != NetworkManager.Instance.GetPlayerNick())
		{
			return false;
		}
		if (canSelect)
		{
			return true;
		}
		return false;
	}

	private IEnumerator MoveIndividualCo(int sourceIndex, int targetIndex, float speed = 24f)
	{
		moving[sourceIndex] = true;
		Transform cardT = cards[sourceIndex].transform;
		Vector3 vector = positions[sourceIndex];
		Vector3 vector2 = positions[targetIndex];
		float num = Globals.Instance.sizeW * 0.8f;
		float num2 = Globals.Instance.sizeW * 0.2f;
		float iterationDelay = 0.5f;
		speed = ((!GameManager.Instance.IsMultiplayer() && GameManager.Instance.configGameSpeed != Enums.ConfigSpeed.Fast && GameManager.Instance.configGameSpeed != Enums.ConfigSpeed.Ultrafast) ? (speed * 1.2f) : (speed * 1.4f));
		Vector3 startPos = vector;
		Vector3 targetPos = vector2;
		float num3 = Mathf.Abs(startPos.x - targetPos.x);
		bool finished = false;
		float num4 = 0.1f;
		float num5 = 4f;
		float arcHeight = Mathf.Clamp(num4 + (num3 - num2) / (num - num2) * (num5 - num4), num4, num5);
		if (vector2.x < vector.x)
		{
			arcHeight *= -1f;
		}
		while (!finished)
		{
			float x = startPos.x;
			float x2 = targetPos.x;
			float num6 = x2 - x;
			float num7 = Mathf.MoveTowards(cardT.localPosition.x, x2, speed * Time.deltaTime);
			Mathf.Lerp(startPos.y, targetPos.y, (num7 - x) / num6);
			_ = arcHeight * (num7 - x) * (num7 - x2) / (-0.25f * num6 * num6);
			float y = Mathf.MoveTowards(cardT.localPosition.y, targetPos.y, speed * Time.deltaTime);
			Vector3 vector3 = (cardT.localPosition = new Vector3(num7, y, cardT.localPosition.z));
			if (Mathf.Abs(vector3.x - targetPos.x) < 0.01f && Mathf.Abs(vector3.y - targetPos.y) < 0.01f)
			{
				finished = true;
			}
			yield return Globals.Instance.WaitForSeconds(Time.deltaTime * iterationDelay);
		}
		cardT.localPosition = targetPos;
		cards[targetIndex] = cardT.GetComponent<CardItem>();
		cards[targetIndex].CardPlayerIndex = targetIndex;
		moving[sourceIndex] = false;
	}

	private Quaternion LookAt2D(Vector2 forward)
	{
		return Quaternion.Euler(0f, 0f, Mathf.Atan2(forward.y, forward.x) * 57.29578f);
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false, int absolutePosition = -1)
	{
		_controllerList.Clear();
		if (Functions.TransformIsVisible(botShuffle))
		{
			_controllerList.Add(botShuffle);
		}
		if (Functions.TransformIsVisible(finishButton.transform))
		{
			_controllerList.Add(finishButton.transform);
		}
		for (int i = 0; i < 4; i++)
		{
			if (Functions.TransformIsVisible(sideCharacters.charArray[i].transform))
			{
				_controllerList.Add(sideCharacters.charArray[i].transform.GetChild(0).transform);
			}
		}
		if (Functions.TransformIsVisible(PlayerUIManager.Instance.giveGold))
		{
			_controllerList.Add(PlayerUIManager.Instance.giveGold);
		}
		if (CanClick())
		{
			foreach (Transform item in cardContainer)
			{
				_controllerList.Add(item);
			}
		}
		controllerHorizontalIndex = Functions.GetListClosestIndexToMousePosition(_controllerList);
		controllerHorizontalIndex = Functions.GetClosestIndexBasedOnDirection(_controllerList, controllerHorizontalIndex, goingUp, goingRight, goingDown, goingLeft);
		if (_controllerList[controllerHorizontalIndex] != null)
		{
			warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerList[controllerHorizontalIndex].position);
			Mouse.current.WarpCursorPosition(warpPosition);
		}
	}
}
