using TMPro;
using UnityEngine;

public class BoxPlayer : MonoBehaviour
{
	public TMP_Text playerName;

	public Transform border;

	public SpriteRenderer background;

	public SpriteRenderer borderSPR;

	private bool boxEnabled;

	private bool active;

	public string playerNick = "";

	private bool skuDisabled;

	private void Awake()
	{
		HideBorder();
	}

	public void Activate(bool state)
	{
		active = state;
		if (state)
		{
			DrawBorder();
		}
		else
		{
			HideBorder();
		}
	}

	public void SetName(string name)
	{
		playerNick = name;
		playerName.text = NetworkManager.Instance.GetPlayerNickReal(name);
		if (name != "")
		{
			background.color = Functions.HexToColor(NetworkManager.Instance.GetColorFromNick(name));
			boxEnabled = true;
		}
		else
		{
			boxEnabled = false;
		}
	}

	public void DrawBorder()
	{
		border.gameObject.SetActive(value: true);
	}

	public void HideBorder()
	{
		border.gameObject.SetActive(value: false);
	}

	public void DisableSKU(string _sku)
	{
		skuDisabled = true;
		background.color = Functions.HexToColor("#666666");
		borderSPR.color = Functions.HexToColor("#300000");
		GetComponent<PopupText>().text = string.Format(Texts.Instance.GetText("playerDontHaveDLC"), SteamManager.Instance.GetDLCName(_sku));
	}

	private void OnMouseEnter()
	{
		if (skuDisabled)
		{
			DrawBorder();
		}
		else if (boxEnabled)
		{
			DrawBorder();
		}
	}

	private void OnMouseExit()
	{
		if (skuDisabled)
		{
			HideBorder();
		}
		else if (boxEnabled && !active)
		{
			HideBorder();
		}
	}

	private void OnMouseUp()
	{
		if (!skuDisabled)
		{
			HeroSelectionManager.Instance.AssignPlayerToBox(playerNick, base.transform.parent.transform.parent.GetComponent<BoxSelection>().GetId());
		}
	}
}
