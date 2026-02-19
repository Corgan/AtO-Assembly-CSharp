using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GiveManager : MonoBehaviour
{
	public Transform elements;

	public TMP_Text quantityText;

	public TMP_Text target;

	public TMP_Text descriptionText;

	public int playerTarget;

	public int quantity;

	public Transform prevPlayer;

	public Transform nextPlayer;

	public Transform bgGold;

	public Transform bgShards;

	public BotonGeneric botonGold;

	public BotonGeneric botonDust;

	public Transform botonGive;

	private int type;

	public List<Transform> buttonsController;

	public int controllerHorizontalIndex = -1;

	private Vector2 warpPosition = Vector2.zero;

	private List<Transform> _controllerList = new List<Transform>();

	public static GiveManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public bool IsActive()
	{
		return elements.gameObject.activeSelf;
	}

	public void ShowGive(bool state, int _type = 0)
	{
		if (GameManager.Instance.IsMultiplayer() && state && AtOManager.Instance.townDivinationCreator == NetworkManager.Instance.GetPlayerNick())
		{
			AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("cantGiveGoldDivination"));
			return;
		}
		elements.gameObject.SetActive(state);
		if (state)
		{
			if ((bool)MapManager.Instance)
			{
				MapManager.Instance.characterWindow.Hide();
			}
			else if ((bool)TownManager.Instance)
			{
				TownManager.Instance.characterWindow.Hide();
				TownManager.Instance.ShowTownUpgrades(state: false);
			}
			botonGive.gameObject.SetActive(value: false);
			playerTarget = 0;
			if (playerTarget == NetworkManager.Instance.GetMyPosition())
			{
				NextTarget();
			}
			SetQuantity(0);
			if (NetworkManager.Instance.GetNumPlayers() > 2)
			{
				prevPlayer.gameObject.SetActive(value: true);
				nextPlayer.gameObject.SetActive(value: true);
			}
			else
			{
				prevPlayer.gameObject.SetActive(value: false);
				nextPlayer.gameObject.SetActive(value: false);
			}
			type = _type;
			if (type == 0)
			{
				bgGold.gameObject.SetActive(value: true);
				bgShards.gameObject.SetActive(value: false);
				descriptionText.text = Texts.Instance.GetText("selectGiveGold");
				botonGold.Disable();
				botonDust.Enable();
			}
			else
			{
				bgGold.gameObject.SetActive(value: false);
				bgShards.gameObject.SetActive(value: true);
				descriptionText.text = Texts.Instance.GetText("selectGiveShards");
				botonGold.Enable();
				botonDust.Disable();
			}
			RefreshTargetName();
			if ((bool)CardCraftManager.Instance)
			{
				CardCraftManager.Instance.ShowSearch(state: false);
			}
		}
		else
		{
			target.text = "";
			if ((bool)CardCraftManager.Instance && CardCraftManager.Instance.craftType == 2)
			{
				CardCraftManager.Instance.ShowSearch(state: true);
			}
		}
	}

	private void SetQuantity(int q)
	{
		quantity = q;
		if (quantity < 0)
		{
			quantity = 0;
		}
		else if (type == 0 && quantity > AtOManager.Instance.GetPlayerGold())
		{
			quantity = AtOManager.Instance.GetPlayerGold();
		}
		else if (type == 1 && quantity > AtOManager.Instance.GetPlayerDust())
		{
			quantity = AtOManager.Instance.GetPlayerDust();
		}
		quantityText.text = quantity.ToString();
		if (quantity != 0)
		{
			botonGive.gameObject.SetActive(value: true);
		}
		else
		{
			botonGive.gameObject.SetActive(value: false);
		}
	}

	public void Give(int q)
	{
		SetQuantity(quantity + q);
	}

	public void NextTarget()
	{
		playerTarget++;
		if (playerTarget >= NetworkManager.Instance.GetNumPlayers())
		{
			playerTarget = 0;
		}
		if (playerTarget == NetworkManager.Instance.GetMyPosition())
		{
			NextTarget();
		}
		else
		{
			RefreshTargetName();
		}
	}

	private void RefreshTargetName()
	{
		StringBuilder stringBuilder = new StringBuilder();
		string playerNickPosition = NetworkManager.Instance.GetPlayerNickPosition(playerTarget);
		stringBuilder.Append(NetworkManager.Instance.GetPlayerNickReal(playerNickPosition));
		stringBuilder.Append("<br><size=3.5><color=#BBB>(");
		Hero[] team = AtOManager.Instance.GetTeam();
		for (int i = 0; i < team.Length; i++)
		{
			if (team[i].Owner == playerNickPosition)
			{
				stringBuilder.Append(team[i].HeroData.HeroSubClass.CharacterName);
				stringBuilder.Append(", ");
			}
		}
		stringBuilder.Remove(stringBuilder.Length - 2, 2);
		stringBuilder.Append(")");
		target.text = stringBuilder.ToString();
	}

	public void PrevTarget()
	{
		playerTarget--;
		if (playerTarget < 0)
		{
			playerTarget = NetworkManager.Instance.GetNumPlayers() - 1;
		}
		if (playerTarget == NetworkManager.Instance.GetMyPosition())
		{
			PrevTarget();
		}
		else
		{
			RefreshTargetName();
		}
	}

	public void GiveAction()
	{
		if (quantity > 0)
		{
			if (NetworkManager.Instance.IsMaster())
			{
				AtOManager.Instance.GivePlayer(type, quantity, NetworkManager.Instance.GetPlayerNickPosition(playerTarget), NetworkManager.Instance.GetPlayerNick(), anim: true, save: true);
			}
			else
			{
				AtOManager.Instance.AskGivePlayerToPlayer(type, quantity, NetworkManager.Instance.GetPlayerNickPosition(playerTarget), NetworkManager.Instance.GetPlayerNick());
			}
			ShowGive(state: false);
		}
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false)
	{
		_controllerList.Clear();
		if (Functions.TransformIsVisible(nextPlayer))
		{
			_controllerList.Add(nextPlayer);
			_controllerList.Add(prevPlayer);
		}
		if (Functions.TransformIsVisible(botonGive))
		{
			_controllerList.Add(botonGive);
		}
		for (int i = 0; i < buttonsController.Count; i++)
		{
			if (Functions.TransformIsVisible(buttonsController[i]))
			{
				_controllerList.Add(buttonsController[i]);
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
