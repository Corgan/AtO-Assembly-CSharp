using System;
using TMPro;
using UnityEngine;

public class BotonSupply : MonoBehaviour
{
	public int column;

	public int row;

	private BotonGeneric bGeneric;

	private TMP_Text textBoton;

	public string supplyId = "";

	public bool available;

	private void Awake()
	{
		bGeneric = GetComponent<BotonGeneric>();
		textBoton = base.transform.GetChild(0).GetComponent<TMP_Text>();
	}

	private void Start()
	{
		SetId();
		SetText();
	}

	private void SetId()
	{
		supplyId = TownManager.Instance.GetUpgradeButtonId(column, row);
	}

	public void ShowAvailable()
	{
		bGeneric.Enable();
		bGeneric.color = Functions.HexToColor("#426C41");
		bGeneric.SetColor();
		bGeneric.SetTextColor(Functions.HexToColor("#FFFFFF"));
		available = true;
	}

	public void ShowSelected()
	{
		bGeneric.Disable();
		bGeneric.color = Functions.HexToColor("#967443");
		bGeneric.SetColor();
		bGeneric.ShowDisableMask(state: false);
		bGeneric.SetTextColor(Functions.HexToColor("#FFFFFF"));
		available = false;
	}

	public void ShowLocked()
	{
		bGeneric.Disable();
		bGeneric.color = Functions.HexToColor("#9C9C9C");
		bGeneric.SetColor();
		bGeneric.SetTextColor(Functions.HexToColor("#999999"));
		available = false;
	}

	private void SetText()
	{
		textBoton.text = Texts.Instance.GetText(supplyId).Replace("<c>", "<color=#E0A44E>").Replace("</c>", "</color>");
	}

	public void BuySupply()
	{
		bool confirmAnswer = AlertManager.Instance.GetConfirmAnswer();
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(BuySupply));
		if (confirmAnswer)
		{
			PlayerManager.Instance.PlayerBuySupply(supplyId);
			TownManager.Instance.RefreshTownUpgrades();
		}
	}

	private void OnMouseUp()
	{
		if (Functions.ClickedThisTransform(base.transform) && !AlertManager.Instance.IsActive() && !GameManager.Instance.IsTutorialActive() && !SettingsManager.Instance.IsActive() && !DamageMeterManager.Instance.IsActive() && available)
		{
			AlertManager.buttonClickDelegate = BuySupply;
			AlertManager.Instance.AlertConfirmDouble(Texts.Instance.GetText("townAssignWarning"));
			bGeneric.HideBorderNow();
		}
	}
}
