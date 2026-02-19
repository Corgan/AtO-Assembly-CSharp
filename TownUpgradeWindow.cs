using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TownUpgradeWindow : MonoBehaviour
{
	public Transform elements;

	public List<BotonSupply> botonSupply;

	public TMP_Text requiredTM;

	public Transform exitButton;

	public Transform resetButton;

	public Transform pointsButton;

	public Transform sellSupplyT;

	public Transform sellSupplyButton;

	public TMP_Text supplySellQuantity;

	public TMP_Text supplySellResult;

	public Transform supplySellBg;

	public List<Transform> sellSupplyTransforms;

	public int controllerHorizontalIndex = -1;

	private Vector2 warpPosition = Vector2.zero;

	private List<Transform> _controllerList = new List<Transform>();

	private void Start()
	{
		if (GameManager.Instance.GetDeveloperMode())
		{
			resetButton.gameObject.SetActive(value: true);
			pointsButton.gameObject.SetActive(value: true);
		}
		else
		{
			resetButton.gameObject.SetActive(value: false);
			pointsButton.gameObject.SetActive(value: false);
		}
		sellSupplyT.gameObject.SetActive(value: false);
	}

	public void Show(bool state)
	{
		elements.gameObject.SetActive(state);
		if (state)
		{
			StartCoroutine(SetButtonsCo());
			TownManager.Instance.ShowButtons(state: false);
		}
		else
		{
			TownManager.Instance.ShowButtons(state: true);
		}
	}

	public void Refresh()
	{
		SetButtons();
		TownManager.Instance.SetTownBuildings();
	}

	public void ShowSellSupply(bool state)
	{
		sellSupplyT.gameObject.SetActive(state);
		supplySellQuantity.text = "0";
		ModifySupplyQuantity(0);
	}

	public void ModifySupplyQuantity(int _quantity)
	{
		int num = int.Parse(supplySellQuantity.text.Split(' ')[0]);
		num += _quantity;
		if (num < 0)
		{
			num = 0;
		}
		else if (num > PlayerManager.Instance.SupplyActual)
		{
			num = PlayerManager.Instance.SupplyActual;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(num);
		stringBuilder.Append(" <voffset=-.2><size=-.1><sprite name=supply>");
		supplySellQuantity.text = stringBuilder.ToString();
		ConvertSupply(num);
	}

	private void ConvertSupply(int quantity)
	{
		int value = quantity * 100;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(value);
		stringBuilder.Append(" <sprite name=gold>");
		stringBuilder.Append(" ");
		stringBuilder.Append(value);
		stringBuilder.Append(" <sprite name=dust>");
		supplySellResult.text = stringBuilder.ToString();
		supplySellBg.gameObject.SetActive(value: true);
	}

	public void SellSupplyAction()
	{
		int quantity = int.Parse(supplySellQuantity.text.Split(' ')[0]);
		AtOManager.Instance.SellSupply(quantity);
		ShowSellSupply(state: false);
	}

	private IEnumerator SetButtonsCo()
	{
		yield return Globals.Instance.WaitForSeconds(0.01f);
		SetButtons();
	}

	private void SetButtons()
	{
		int playerSupplyActual = PlayerManager.Instance.GetPlayerSupplyActual();
		int num = PlayerManager.Instance.TotalPointsSpentInSupplys();
		if (num < 30)
		{
			requiredTM.gameObject.SetActive(value: true);
			requiredTM.text = string.Format(Texts.Instance.GetText("townRequired"), 30 - num);
		}
		else
		{
			requiredTM.gameObject.SetActive(value: false);
		}
		for (int i = 0; i < botonSupply.Count; i++)
		{
			if (PlayerManager.Instance.PlayerHaveSupply(botonSupply[i].supplyId))
			{
				botonSupply[i].ShowSelected();
			}
			else if (botonSupply[i].row == 1 && playerSupplyActual > 0)
			{
				botonSupply[i].ShowAvailable();
			}
			else if (PlayerManager.Instance.PointsRequiredForSupply(botonSupply[i].supplyId) <= playerSupplyActual && PlayerManager.Instance.PlayerHaveSupply(PlayerManager.Instance.SupplyRequiredForSupply(botonSupply[i].supplyId)))
			{
				if (botonSupply[i].row <= 3 || (botonSupply[i].row > 3 && num >= 30))
				{
					botonSupply[i].ShowAvailable();
				}
				else
				{
					botonSupply[i].ShowLocked();
				}
			}
			else
			{
				botonSupply[i].ShowLocked();
			}
		}
		if (PlayerManager.Instance.PlayerHaveSupply("townUpgrade_6_6") && AtOManager.Instance.GetNgPlus() < 6)
		{
			sellSupplyButton.gameObject.SetActive(value: true);
		}
		else
		{
			sellSupplyButton.gameObject.SetActive(value: false);
		}
		if (GameManager.Instance.GetDeveloperMode())
		{
			sellSupplyButton.gameObject.SetActive(value: true);
		}
	}

	public bool IsActive()
	{
		return elements.gameObject.activeSelf;
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false, int absolutePosition = -1)
	{
		_controllerList.Clear();
		if (Functions.TransformIsVisible(sellSupplyT))
		{
			for (int i = 0; i < sellSupplyTransforms.Count; i++)
			{
				_controllerList.Add(sellSupplyTransforms[i].transform);
			}
		}
		else
		{
			for (int j = 0; j < botonSupply.Count; j++)
			{
				_controllerList.Add(botonSupply[j].transform);
			}
			if (Functions.TransformIsVisible(sellSupplyButton))
			{
				_controllerList.Add(sellSupplyButton);
				_controllerList.Add(exitButton);
			}
			else
			{
				_controllerList.Add(exitButton);
				_controllerList.Add(exitButton);
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
