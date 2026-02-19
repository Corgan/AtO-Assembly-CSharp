using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConflictManager : MonoBehaviour
{
	public SpriteRenderer[] cardbacks;

	public SpriteRenderer[] characterSPR;

	public TMP_Text[] nicks;

	public Transform[] xSymbol;

	public Transform buttons;

	public BotonGeneric[] botonConflict;

	public TMP_Text nickChoosing;

	public TMP_Text charWins;

	public Transform charWinsTransform;

	private int optionSelected = -1;

	public int playerChoosing = -1;

	private int cardOrder;

	private Hero[] heroes = new Hero[4];

	private bool[] charRoll;

	private int[] charRollIterations;

	private int[] charRollResult;

	private bool[] charWinner;

	public TMP_Text[] characterTnum;

	public Transform[] characterTcards;

	private int finalTieTimes;

	private PhotonView photonView;

	public int controllerHorizontalIndex = -1;

	private Vector2 warpPosition = Vector2.zero;

	private List<Transform> _controllerList = new List<Transform>();

	private void Awake()
	{
		photonView = MapManager.Instance.GetPhotonView();
	}

	public bool IsActive()
	{
		return base.gameObject.activeSelf;
	}

	private void OnEnable()
	{
		EventManager.Instance?.SetListGameObjectsState(state: false);
	}

	private void OnDisable()
	{
		EventManager.Instance?.SetListGameObjectsState(state: true);
	}

	public void Show()
	{
		if (EventManager.Instance == null)
		{
			base.transform.localPosition = new Vector3(0f, 0.5f, base.transform.localPosition.z);
		}
		else
		{
			base.transform.localPosition = new Vector3(1.1f, 0.5f, base.transform.localPosition.z);
		}
		base.gameObject.SetActive(value: true);
		EventManager.Instance?.SetListGameObjectsState(state: false);
		Random.InitState((AtOManager.Instance.currentMapNode + AtOManager.Instance.GetGameId()).GetDeterministicHashCode());
		SetConflict();
		SetButtons();
		StartCoroutine(ShowCo());
	}

	private IEnumerator ShowCo()
	{
		if (NetworkManager.Instance.IsMaster())
		{
			while (!NetworkManager.Instance.AllPlayersReady("startConflict"))
			{
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			Functions.DebugLogGD("Game ready, Everybody checked startConflict");
			NetworkManager.Instance.PlayersNetworkContinue("startConflict");
			List<int> conflictResolutionOrder = AtOManager.Instance.GetConflictResolutionOrder();
			int i;
			for (i = 0; heroes[conflictResolutionOrder[i]] == null || heroes[conflictResolutionOrder[i]].HeroData == null; i++)
			{
			}
			playerChoosing = conflictResolutionOrder[i];
			photonView.RPC("NET_ShareConflictOrder", RpcTarget.Others, playerChoosing);
			while (!NetworkManager.Instance.AllPlayersReady("settingConflictOrder"))
			{
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			Functions.DebugLogGD("Game ready, Everybody checked settingConflictOrder");
			NetworkManager.Instance.PlayersNetworkContinue("settingConflictOrder");
			EnableButtonsForPlayerChoosing();
		}
		else
		{
			NetworkManager.Instance.SetStatusReady("startConflict");
			Functions.DebugLogGD("startConflict, we can continue!");
		}
	}

	public void Hide()
	{
		EventManager.Instance?.SetListGameObjectsState(state: true);
		base.gameObject.SetActive(value: false);
	}

	private void SetConflict()
	{
		heroes = AtOManager.Instance.GetTeam();
		SetCardbacks();
		for (int i = 0; i < heroes.Length; i++)
		{
			characterSPR[i].sprite = heroes[i].SpritePortrait;
		}
		charRollIterations = new int[4];
		charRollResult = new int[4];
		cardOrder = 1100;
		finalTieTimes = 0;
		playerChoosing = -1;
		charRoll = new bool[4];
		charWinner = new bool[4];
		charWins.text = "";
		nickChoosing.text = "";
		for (int j = 0; j < 4; j++)
		{
			charRoll[j] = true;
			charWinner[j] = false;
			xSymbol[j].gameObject.SetActive(value: false);
		}
	}

	private void SetCardbacks()
	{
		for (int i = 0; i < 4; i++)
		{
			Hero hero = AtOManager.Instance.GetHero(i);
			string cardbackUsed = hero.CardbackUsed;
			if (!(cardbackUsed != ""))
			{
				continue;
			}
			CardbackData cardbackData = Globals.Instance.GetCardbackData(cardbackUsed);
			Debug.Log(cardbackData);
			if (cardbackData == null)
			{
				cardbackData = Globals.Instance.GetCardbackData(Globals.Instance.GetCardbackBaseIdBySubclass(hero.HeroData.HeroSubClass.Id));
				if (cardbackData == null)
				{
					cardbackData = Globals.Instance.GetCardbackData("defaultCardback");
				}
			}
			Sprite cardbackSprite = cardbackData.CardbackSprite;
			if (cardbackSprite != null)
			{
				cardbacks[i].sprite = cardbackSprite;
			}
		}
	}

	private void EnableButtonsForPlayerChoosing()
	{
		nickChoosing.text = string.Format(Texts.Instance.GetText("conflictPlayerChooses"), NetworkManager.Instance.GetPlayerNickReal(heroes[playerChoosing].Owner));
		for (int i = 0; i < 4; i++)
		{
			if (heroes[i] != null && heroes[i].HeroData != null)
			{
				nicks[i].text = NetworkManager.Instance.GetPlayerNickReal(heroes[i].Owner);
				nicks[i].color = Functions.HexToColor(NetworkManager.Instance.GetColorFromNick(heroes[i].Owner));
			}
		}
		if (heroes[playerChoosing].Owner == NetworkManager.Instance.GetPlayerNick())
		{
			botonConflict[0].Enable();
			botonConflict[1].Enable();
			botonConflict[2].Enable();
		}
	}

	public void SetButtons()
	{
		botonConflict[0].Disable();
		botonConflict[1].Disable();
		botonConflict[2].Disable();
	}

	private IEnumerator SyncroOrder()
	{
		Functions.DebugLogGD("WaitingSyncro settingConflictOrder");
		if (NetworkManager.Instance.IsMaster())
		{
			List<int> conflictResolutionOrder = AtOManager.Instance.GetConflictResolutionOrder();
			playerChoosing = conflictResolutionOrder[0];
			photonView.RPC("NET_ShareConflictOrder", RpcTarget.Others, playerChoosing);
			while (!NetworkManager.Instance.AllPlayersReady("settingConflictOrder"))
			{
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			Functions.DebugLogGD("Game ready, Everybody checked settingConflictOrder");
			NetworkManager.Instance.PlayersNetworkContinue("settingConflictOrder");
			EnableButtonsForPlayerChoosing();
		}
	}

	public IEnumerator NET_ShareConflictOrderCo()
	{
		NetworkManager.Instance.SetWaitingSyncro("settingConflictOrder", status: true);
		NetworkManager.Instance.SetStatusReady("settingConflictOrder");
		Functions.DebugLogGD("settingConflictOrder, waiting");
		while (NetworkManager.Instance.WaitingSyncro["settingConflictOrder"])
		{
			yield return Globals.Instance.WaitForSeconds(0.1f);
		}
		Functions.DebugLogGD("settingConflictOrder, we can continue!");
		EnableButtonsForPlayerChoosing();
	}

	public void SelectOption(int option)
	{
		for (int i = 0; i < 3; i++)
		{
			botonConflict[i].Disable();
		}
		botonConflict[option].ShowDisableMask(state: false);
		botonConflict[option].color = Functions.HexToColor("#E0A44E");
		botonConflict[option].SetColor();
		botonConflict[option].PermaBorder(state: true);
		if (!NetworkManager.Instance.IsMaster())
		{
			photonView.RPC("NET_SelectConflictOptionFromSlave", RpcTarget.MasterClient, option);
		}
		else
		{
			StartCoroutine(SelectOptionCo(option));
		}
	}

	public void SelectOptionFromOutside(int option)
	{
		StartCoroutine(SelectOptionCo(option));
	}

	private IEnumerator SelectOptionCo(int option)
	{
		yield return Globals.Instance.WaitForSeconds(0.1f);
		if (NetworkManager.Instance.IsMaster())
		{
			photonView.RPC("NET_SelectConflictOption", RpcTarget.Others, option);
			while (!NetworkManager.Instance.AllPlayersReady("selectConflictOption"))
			{
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			Functions.DebugLogGD("Game ready, Everybody checked selectConflictOption");
			NetworkManager.Instance.PlayersNetworkContinue("selectConflictOption");
		}
		else
		{
			NetworkManager.Instance.SetWaitingSyncro("selectConflictOption", status: true);
			NetworkManager.Instance.SetStatusReady("selectConflictOption");
			while (NetworkManager.Instance.WaitingSyncro["selectConflictOption"])
			{
				yield return Globals.Instance.WaitForSeconds(0.1f);
			}
			Functions.DebugLogGD("selectConflictOption, we can continue!");
		}
		optionSelected = option;
		for (int i = 0; i < 3; i++)
		{
			botonConflict[i].Disable();
		}
		botonConflict[option].ShowDisableMask(state: false);
		botonConflict[option].color = Functions.HexToColor("#E0A44E");
		botonConflict[option].SetColor();
		botonConflict[option].PermaBorder(state: true);
		StartCoroutine(RollCards());
	}

	private IEnumerator RollCards()
	{
		yield return Globals.Instance.WaitForSeconds(0.1f);
		int rolls = 0;
		for (int i = 0; i < 4; i++)
		{
			if (heroes[i] != null && heroes[i].HeroData != null && charRoll[i])
			{
				yield return Globals.Instance.WaitForSeconds(0.25f);
				DoCard(i);
				rolls++;
			}
		}
		yield return Globals.Instance.WaitForSeconds(2.3f + (float)rolls * 0.1f);
		StartCoroutine(RollResult());
	}

	private void DoCard(int heroIndex)
	{
		charRollIterations[heroIndex]++;
		if (charRollIterations[heroIndex] > 3)
		{
			charRollIterations[heroIndex] = 3;
		}
		charRollResult[heroIndex] = -1;
		List<string> list = new List<string>();
		for (int i = 0; i < heroes[heroIndex].Cards.Count; i++)
		{
			list.Add(heroes[heroIndex].Cards[i]);
		}
		bool flag = false;
		string id = "";
		while (!flag)
		{
			id = list[MapManager.Instance.GetRandomIntRange(0, list.Count)];
			CardData cardData = Globals.Instance.GetCardData(id, instantiate: false);
			if (cardData.CardClass != Enums.CardClass.Injury && cardData.CardClass != Enums.CardClass.Boon)
			{
				flag = true;
			}
		}
		GameObject obj = Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, characterTcards[heroIndex]);
		CardItem component = obj.GetComponent<CardItem>();
		component.SetCard(id, deckScale: false);
		component.SetCardback(heroes[heroIndex]);
		obj.transform.localPosition = new Vector3(0f, 0.1f - 0.1f * (float)charRollIterations[heroIndex], 0f);
		component.DoReward(fromReward: false, fromEvent: true, fromLoot: false, selectable: false);
		component.SetDestinationLocalScale(1f);
		component.TopLayeringOrder("Book", cardOrder);
		cardOrder += 50;
		component.DisableCollider();
		charRollResult[heroIndex] = Globals.Instance.GetCardData(id, instantiate: false).EnergyCost;
		StartCoroutine(ShowNumRoll(heroIndex));
	}

	private IEnumerator ShowNumRoll(int heroIndex)
	{
		if (!charRoll[heroIndex])
		{
			characterTnum[heroIndex].text = "";
			yield break;
		}
		yield return Globals.Instance.WaitForSeconds(2.2f);
		characterTcards[heroIndex].GetChild(0).GetComponent<CardItem>().active = true;
		characterTnum[heroIndex].text = charRollResult[heroIndex].ToString();
	}

	private IEnumerator RollResult()
	{
		bool flag = false;
		while (!flag)
		{
			yield return Globals.Instance.WaitForSeconds(0.1f);
			int num = 0;
			flag = true;
			for (int i = 0; i < 4; i++)
			{
				if (charRoll[i] && charRollResult[i] == -1)
				{
					flag = false;
					break;
				}
				if (charRoll[i])
				{
					num += charRollResult[i];
				}
			}
		}
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		for (int j = 0; j < 4; j++)
		{
			charWinner[j] = false;
			if (heroes[j] != null && !(heroes[j].HeroData == null) && charRoll[j])
			{
				dictionary.Add(j, charRollResult[j]);
			}
		}
		dictionary = dictionary.OrderBy((KeyValuePair<int, int> x) => x.Value).ToDictionary((KeyValuePair<int, int> x) => x.Key, (KeyValuePair<int, int> x) => x.Value);
		int num2 = -1;
		int num3 = -1;
		int num4 = dictionary.Count - 1;
		if (optionSelected == 2)
		{
			if (dictionary.ElementAt(num4).Value == dictionary.ElementAt(num4 - 1).Value)
			{
				num2 = dictionary.ElementAt(num4).Value;
			}
			else
			{
				num3 = dictionary.ElementAt(num4).Key;
			}
		}
		else if (optionSelected == 0)
		{
			if (dictionary.ElementAt(0).Value == dictionary.ElementAt(1).Value)
			{
				num2 = dictionary.ElementAt(0).Value;
			}
			else
			{
				num3 = dictionary.ElementAt(0).Key;
			}
		}
		else if (optionSelected == 1)
		{
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
			foreach (KeyValuePair<int, int> item in dictionary)
			{
				dictionary2.Add(item.Key, Mathf.Abs(item.Value - 2));
			}
			dictionary = dictionary2.OrderBy((KeyValuePair<int, int> x) => x.Value).ToDictionary((KeyValuePair<int, int> x) => x.Key, (KeyValuePair<int, int> x) => x.Value);
			if (dictionary.ElementAt(0).Value == dictionary.ElementAt(1).Value)
			{
				num2 = dictionary.ElementAt(0).Value;
			}
			else
			{
				num3 = dictionary.ElementAt(0).Key;
			}
		}
		if (num3 == -1)
		{
			string text = "";
			bool flag2 = true;
			for (int num5 = 0; num5 < dictionary.Count; num5++)
			{
				if (num5 == 0)
				{
					text = heroes[dictionary.ElementAt(num5).Key].Owner;
				}
				else if (text != heroes[dictionary.ElementAt(num5).Key].Owner)
				{
					flag2 = false;
					break;
				}
				Functions.DebugLogGD("sameOwner " + flag2 + " _ " + text);
			}
			if (flag2)
			{
				num3 = dictionary.ElementAt(0).Key;
			}
			else if (dictionary.Count == 2)
			{
				finalTieTimes++;
				if (finalTieTimes >= 3)
				{
					num3 = dictionary.ElementAt(Random.Range(0, dictionary.Count)).Key;
				}
			}
		}
		int heroWinner = -1;
		if (num3 == -1)
		{
			for (int num6 = 0; num6 < dictionary.Count; num6++)
			{
				int key = dictionary.ElementAt(num6).Key;
				if (charRoll[key] && dictionary.ElementAt(num6).Value == num2)
				{
					charRoll[key] = true;
				}
				else
				{
					charRoll[key] = false;
					TurnOffCharacter(key);
				}
				ClearNumRoll(key);
			}
			StartCoroutine(RollCards());
			yield break;
		}
		charWinner[num3] = true;
		for (int num7 = 0; num7 < dictionary.Count; num7++)
		{
			int key2 = dictionary.ElementAt(num7).Key;
			if (key2 != num3)
			{
				TurnOffCharacter(key2);
			}
			else
			{
				heroWinner = key2;
			}
			ClearNumRoll(key2);
		}
		FinalResolution(heroWinner);
	}

	private void ClearNumRoll(int heroIndex)
	{
		characterTnum[heroIndex].text = "";
	}

	private void FinalResolution(int heroWinner)
	{
		buttons.gameObject.SetActive(value: false);
		charWinsTransform.gameObject.SetActive(value: true);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<size=+1>");
		stringBuilder.Append("<color=");
		stringBuilder.Append(NetworkManager.Instance.GetColorFromNick(heroes[heroWinner].Owner));
		stringBuilder.Append(">");
		stringBuilder.Append(NetworkManager.Instance.GetPlayerNickReal(heroes[heroWinner].Owner));
		stringBuilder.Append("</color></size><br>");
		charWins.text = string.Format(Texts.Instance.GetText("conflictPlayerWins"), stringBuilder.ToString());
		MapManager.Instance.ResultConflict(heroes[heroWinner].Owner);
	}

	private void TurnOffCharacter(int heroIndex)
	{
		characterSPR[heroIndex].color = new Color(0.3f, 0.3f, 0.3f, 1f);
		xSymbol[heroIndex].gameObject.SetActive(value: true);
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false, int absolutePosition = -1)
	{
		_controllerList.Clear();
		if (botonConflict[0].IsEnabled())
		{
			_controllerList.Add(botonConflict[0].transform);
			_controllerList.Add(botonConflict[1].transform);
			_controllerList.Add(botonConflict[2].transform);
		}
		for (int i = 0; i < 4; i++)
		{
			if (Functions.TransformIsVisible(MapManager.Instance.sideCharacters.charArray[i].transform))
			{
				_controllerList.Add(MapManager.Instance.sideCharacters.charArray[i].transform.GetChild(0).transform);
			}
		}
		if (Functions.TransformIsVisible(PlayerUIManager.Instance.giveGold))
		{
			_controllerList.Add(PlayerUIManager.Instance.giveGold);
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
