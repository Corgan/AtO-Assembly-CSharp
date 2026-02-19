using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardPlayerManager : MonoBehaviour
{
	private PhotonView photonView;

	public CharacterWindowUI characterWindowUI;

	public SideCharacters sideCharacters;

	public Transform cardContainer;

	public Transform choose;

	public Transform onlyMaster;

	private List<string> cardList;

	private List<CardItem> cards;

	private List<Vector3> positions;

	private List<bool> moving;

	private bool cardsMoved;

	private bool cardSelected;

	public Transform botShuffle;

	private Dictionary<string, int> playerSelectedCard = new Dictionary<string, int>();

	private CardPlayerPackData _pack;

	public int controllerHorizontalIndex = -1;

	private Vector2 warpPosition = Vector2.zero;

	private List<Transform> _controllerList = new List<Transform>();

	public static CardPlayerManager Instance { get; private set; }

	private void Awake()
	{
		if (GameManager.Instance == null)
		{
			SceneStatic.LoadByName("CardPlayer");
			return;
		}
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Object.Destroy(this);
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
		choose.gameObject.SetActive(value: false);
		StartCoroutine(StartSync());
	}

	private IEnumerator StartSync()
	{
		if (GameManager.Instance.IsMultiplayer())
		{
			Debug.Log("WaitingSyncro cardplayer");
			if (NetworkManager.Instance.IsMaster())
			{
				while (!NetworkManager.Instance.AllPlayersReady("cardplayer"))
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				Functions.DebugLogGD("Game ready, Everybody checked cardplayer");
				NetworkManager.Instance.PlayersNetworkContinue("cardplayer");
			}
			else
			{
				NetworkManager.Instance.SetWaitingSyncro("cardplayer", status: true);
				NetworkManager.Instance.SetStatusReady("cardplayer");
				while (NetworkManager.Instance.WaitingSyncro["cardplayer"])
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				Functions.DebugLogGD("cardplayer, we can continue!");
			}
		}
		GameManager.Instance.SceneLoaded();
		sideCharacters.EnableAll();
		botShuffle.GetComponent<BotonGeneric>().Disable();
		if (!GameManager.Instance.IsMultiplayer())
		{
			onlyMaster.gameObject.SetActive(value: false);
		}
		Random.InitState(string.Concat(AtOManager.Instance.currentMapNode + AtOManager.Instance.GetGameId(), MapManager.Instance.GetRandomString()).GetDeterministicHashCode());
		SetCards();
	}

	private Vector3 GetPosition(int _index, bool _bigSize)
	{
		if (_bigSize)
		{
			return new Vector3(-4.5f + 3f * (float)_index, 0f, 0f);
		}
		return new Vector3(-5.5f + 3.7f * (float)_index, 0f, 0f);
	}

	private void SetCards()
	{
		cardList = new List<string>();
		_pack = AtOManager.Instance.cardPlayerPackData;
		if (_pack != null)
		{
			if (_pack.Card0 != null)
			{
				cardList.Add(_pack.Card0.Id);
			}
			if (_pack.Card1 != null)
			{
				cardList.Add(_pack.Card1.Id);
			}
			if (_pack.Card2 != null)
			{
				cardList.Add(_pack.Card2.Id);
			}
			if (_pack.Card3 != null)
			{
				cardList.Add(_pack.Card3.Id);
			}
		}
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
			GameObject obj = Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, cardContainer);
			CardItem component = obj.GetComponent<CardItem>();
			component.DisableCollider();
			cards.Add(component);
			obj.name = text;
			component.SetCard(cardList[i], deckScale: false);
			Vector3 position = GetPosition(i, _bigSize: true);
			float x = position.x;
			float y = position.y;
			positions.Add(position);
			component.transform.position = new Vector3(x, y, 0f);
			component.DoReward(fromReward: false, fromEvent: false, fromLoot: true, selectable: false);
			component.SetDestination(new Vector3(x, y, 0f));
			component.SetLocalScale(new Vector3(1.4f, 1.4f, 1f));
			component.SetDestinationLocalScale(1.4f);
			component.CardPlayerIndex = i;
			component.GetComponent<Floating>().enabled = true;
			yield return Globals.Instance.WaitForSeconds(0.1f);
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
		Random.InitState(string.Concat(AtOManager.Instance.currentMapNode + AtOManager.Instance.GetGameId(), MapManager.Instance.GetRandomString()).GetDeterministicHashCode());
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
			Vector3 position = GetPosition(i, _bigSize: false);
			float x = position.x;
			float y = position.y;
			positions[i] = position;
			cards[i].GetComponent<Floating>().enabled = false;
			cards[i].cardrevealed = true;
			cards[i].DisableCollider();
			cards[i].SetDestination(new Vector3(x, y, 0f));
			cards[i].SetDestinationLocalScale(1.1f);
		}
		yield return Globals.Instance.WaitForSeconds(0.25f);
		for (int j = 0; j < cards.Count; j++)
		{
			cards[j].TurnBack();
			yield return Globals.Instance.WaitForSeconds(0.1f);
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
		int lastObject = 0;
		int destineToMove = 0;
		int iterations = 10 + _pack.ModIterations;
		int speed = 10 + _pack.ModSpeed;
		for (int i = 0; i < iterations; i++)
		{
			speed += 4;
			int objectToMove = lastObject;
			while (objectToMove == lastObject)
			{
				objectToMove = Random.Range(0, cardList.Count);
				destineToMove = objectToMove;
			}
			while (destineToMove == objectToMove)
			{
				destineToMove = Random.Range(0, cardList.Count);
			}
			StartCoroutine(MoveIndividualCo(objectToMove, destineToMove, speed));
			yield return Globals.Instance.WaitForSeconds(0.01f);
			StartCoroutine(MoveIndividualCo(destineToMove, objectToMove, speed));
			while (moving[destineToMove])
			{
				yield return null;
			}
			yield return Globals.Instance.WaitForSeconds(0.1f);
		}
		EnableCards();
	}

	private void EnableCards()
	{
		choose.gameObject.SetActive(value: true);
		for (int i = 0; i < cards.Count; i++)
		{
			cards[i].enabled = false;
			cards[i].CreateColliderAdjusted();
			cards[i].GetComponent<Floating>().enabled = true;
		}
		cardsMoved = true;
	}

	public bool CanClick()
	{
		if (!cardsMoved)
		{
			return true;
		}
		if (cardsMoved && !cardSelected)
		{
			return true;
		}
		return false;
	}

	public void SelectCard(int _index)
	{
		if (!cardsMoved || cardSelected)
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			if (cards[_index].CardPlayerIndex == _index)
			{
				cardSelected = true;
				if (!GameManager.Instance.IsMultiplayer())
				{
					playerSelectedCard.Add(NetworkManager.Instance.GetPlayerNick(), _index);
					FinishSelection();
				}
				else
				{
					photonView.RPC("NET_AssignSelection", RpcTarget.MasterClient, NetworkManager.Instance.GetPlayerNick(), _index);
				}
				break;
			}
		}
	}

	[PunRPC]
	private void NET_ShareAssignSelection(string _keys, string _values)
	{
		playerSelectedCard.Clear();
		string[] array = JsonHelper.FromJson<string>(_keys);
		string[] array2 = JsonHelper.FromJson<string>(_values);
		for (int i = 0; i < array.Length; i++)
		{
			playerSelectedCard.Add(array[i], int.Parse(array2[i]));
		}
		for (int j = 0; j < 4; j++)
		{
			cards[j].ClearMPMark();
		}
		foreach (KeyValuePair<string, int> item in playerSelectedCard)
		{
			cards[item.Value].ShowMPMark(item.Key);
		}
		GameManager.Instance.PlayLibraryAudio("ui_mapnodeselection");
	}

	[PunRPC]
	private void NET_AssignSelection(string _nick, int _cardIndex)
	{
		if (!playerSelectedCard.ContainsKey(_nick))
		{
			playerSelectedCard.Add(_nick, _cardIndex);
			string[] array = new string[playerSelectedCard.Count];
			playerSelectedCard.Keys.CopyTo(array, 0);
			int[] array2 = new int[playerSelectedCard.Count];
			playerSelectedCard.Values.CopyTo(array2, 0);
			string text = JsonHelper.ToJson(array);
			string text2 = JsonHelper.ToJson(array2);
			photonView.RPC("NET_ShareAssignSelection", RpcTarget.All, text, text2);
			if (playerSelectedCard.Count == NetworkManager.Instance.GetNumPlayers())
			{
				photonView.RPC("NET_FinishSelection", RpcTarget.All);
			}
		}
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
		choose.gameObject.SetActive(value: false);
		for (int i = 0; i < 4; i++)
		{
			cards[i].DoReward(fromReward: false, fromEvent: false, fromLoot: true);
			cards[i].SetLocalScale(new Vector3(1.1f, 1.1f, 1f));
			cards[i].SetDestinationLocalScale(1.1f);
		}
		yield return Globals.Instance.WaitForSeconds(0.6f);
		Hero[] team = AtOManager.Instance.GetTeam();
		for (int j = 0; j < 4; j++)
		{
			if (team[j] == null || team[j].HeroData == null)
			{
				continue;
			}
			foreach (KeyValuePair<string, int> item in playerSelectedCard)
			{
				if (team[j].Owner == item.Key || team[j].Owner == "" || team[j].Owner == null)
				{
					Vector3 to = sideCharacters.CharacterIconPosition(j);
					GameManager.Instance.GenerateParticleTrail(0, cards[item.Value].transform.position, to);
					bool flag = true;
					if (cards[item.Value].CardData.GoldGainQuantity != 0)
					{
						flag = false;
						AtOManager.Instance.GivePlayer(0, cards[item.Value].CardData.GoldGainQuantity, team[j].Owner);
					}
					if (cards[item.Value].CardData.ShardsGainQuantity != 0)
					{
						flag = false;
						AtOManager.Instance.GivePlayer(1, cards[item.Value].CardData.ShardsGainQuantity, team[j].Owner);
					}
					if (cards[item.Value].CardData.Id == "success")
					{
						flag = false;
					}
					if (flag)
					{
						AtOManager.Instance.AddCardToHero(j, cards[item.Value].CardData.Id);
					}
					PlayerManager.Instance.CardUnlock(cards[item.Value].CardData.Id);
					cards[item.Value].ShowPortrait(team[j].HeroData.HeroSubClass.SpriteSpeed);
				}
			}
		}
		if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster())
		{
			SaveManager.SavePlayerData();
			yield return Globals.Instance.WaitForSeconds(4f);
			AtOManager.Instance.FinishCardPlayer();
		}
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
			float num7 = Mathf.MoveTowards(cardT.position.x, x2, speed * Time.deltaTime);
			float num8 = Mathf.Lerp(startPos.y, targetPos.y, (num7 - x) / num6);
			float num9 = arcHeight * (num7 - x) * (num7 - x2) / (-0.25f * num6 * num6);
			Vector3 vector3 = (cardT.position = new Vector3(num7, num8 + num9, cardT.position.z));
			if (Mathf.Abs(vector3.x - targetPos.x) < 0.01f && Mathf.Abs(vector3.y - targetPos.y) < 0.01f)
			{
				finished = true;
			}
			yield return Globals.Instance.WaitForSeconds(Time.deltaTime * iterationDelay);
		}
		cardT.position = targetPos;
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
