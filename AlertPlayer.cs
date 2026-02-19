using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlertPlayer : MonoBehaviour
{
	public TMP_Text playerName;

	public TMP_Text playerDescription;

	public Image playerPlatformImage;

	public Transform muteButton;

	public Transform unmuteButton;

	private string playerNick = "";

	private int playerSlot = -1;

	private Enums.Platform playerPlatform;

	public void SetPlayer(int _playerSlot, string _playerNick)
	{
		playerSlot = _playerSlot;
		playerNick = _playerNick;
		playerName.color = Functions.HexToColor(NetworkManager.Instance.GetColorFromNick(playerNick));
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(_playerNick);
		if (_playerSlot == 0)
		{
			stringBuilder.Append(" (");
			stringBuilder.Append(Texts.Instance.GetText("master"));
			stringBuilder.Append(")");
		}
		playerName.text = stringBuilder.ToString();
		playerPlatformImage.sprite = NetworkManager.Instance.GetSlotPlatformImage(_playerSlot);
		if (NetworkManager.Instance.GetPlayerNick() != playerNick)
		{
			if (NetworkManager.Instance.IsPlayerMutedBySlot(_playerSlot))
			{
				HideMute();
				ShowUnmute();
			}
			else
			{
				ShowMute();
				HideUnmute();
			}
		}
		else
		{
			HideMute();
			HideUnmute();
		}
	}

	public void SetDescription()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (NetworkManager.Instance.PlayerPing.ContainsKey(playerNick))
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(" | ");
			}
			stringBuilder.Append(Texts.Instance.GetText("ping"));
			stringBuilder.Append(": ");
			stringBuilder.Append(NetworkManager.Instance.PlayerPing[playerNick]);
			stringBuilder.Append("ms");
		}
		playerDescription.text = stringBuilder.ToString();
	}

	public void ShowMute()
	{
		if (!muteButton.gameObject.activeSelf)
		{
			muteButton.gameObject.SetActive(value: true);
		}
	}

	public void ShowUnmute()
	{
		if (!unmuteButton.gameObject.activeSelf)
		{
			unmuteButton.gameObject.SetActive(value: true);
		}
	}

	public void HideMute()
	{
		if (muteButton.gameObject.activeSelf)
		{
			muteButton.gameObject.SetActive(value: false);
		}
	}

	public void HideUnmute()
	{
		if (unmuteButton.gameObject.activeSelf)
		{
			unmuteButton.gameObject.SetActive(value: false);
		}
	}

	public void DoMute()
	{
		NetworkManager.Instance.DoMute(playerSlot);
		if (NetworkManager.Instance.IsPlayerMutedBySlot(playerSlot))
		{
			HideMute();
			ShowUnmute();
		}
	}

	public void DoUnmute()
	{
		NetworkManager.Instance.DoUnmute(playerSlot);
		if (!NetworkManager.Instance.IsPlayerMutedBySlot(playerSlot))
		{
			HideUnmute();
			ShowMute();
		}
	}

	public void Show()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
		}
	}

	public void Hide()
	{
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
