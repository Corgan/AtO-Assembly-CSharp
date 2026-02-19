using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
	public Image lineImg;

	public TMP_Text playerName;

	public Image[] characterImg;

	public Transform arrowOnline;

	public Transform arrowOffline;

	public Image statusImg;

	public void SetPlayer(string nick)
	{
		playerName.text = nick;
		TMP_Text tMP_Text = playerName;
		Color color = (lineImg.color = Functions.HexToColor(NetworkManager.Instance.GetColorFromNick(nick)));
		tMP_Text.color = color;
		HideStatusReady();
	}

	public void SetOnline()
	{
		arrowOnline.gameObject.SetActive(value: true);
		arrowOffline.gameObject.SetActive(value: false);
	}

	public void SetOffline()
	{
		arrowOnline.gameObject.SetActive(value: false);
		arrowOffline.gameObject.SetActive(value: true);
	}

	public void HideStatusReady()
	{
		statusImg.gameObject.SetActive(value: false);
	}

	public void ShowStatusReady()
	{
		statusImg.gameObject.SetActive(value: true);
		statusImg.color = Functions.HexToColor(Globals.Instance.ClassColor["warrior"]);
	}

	public void SetStatus(bool status)
	{
		if (status)
		{
			statusImg.color = Functions.HexToColor(Globals.Instance.ClassColor["scout"]);
		}
		else
		{
			statusImg.color = Functions.HexToColor(Globals.Instance.ClassColor["warrior"]);
		}
	}
}
