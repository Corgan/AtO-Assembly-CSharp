// Decompiled with JetBrains decompiler
// Type: AlertPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
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
    this.playerSlot = _playerSlot;
    this.playerNick = _playerNick;
    this.playerName.color = Functions.HexToColor(NetworkManager.Instance.GetColorFromNick(this.playerNick));
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(_playerNick);
    if (_playerSlot == 0)
    {
      stringBuilder.Append(" (");
      stringBuilder.Append(Texts.Instance.GetText("master"));
      stringBuilder.Append(")");
    }
    this.playerName.text = stringBuilder.ToString();
    this.playerPlatformImage.sprite = NetworkManager.Instance.GetSlotPlatformImage(_playerSlot);
    if (NetworkManager.Instance.GetPlayerNick() != this.playerNick)
    {
      if (NetworkManager.Instance.IsPlayerMutedBySlot(_playerSlot))
      {
        this.HideMute();
        this.ShowUnmute();
      }
      else
      {
        this.ShowMute();
        this.HideUnmute();
      }
    }
    else
    {
      this.HideMute();
      this.HideUnmute();
    }
  }

  public void SetDescription()
  {
    StringBuilder stringBuilder = new StringBuilder();
    if (NetworkManager.Instance.PlayerPing.ContainsKey(this.playerNick))
    {
      if (stringBuilder.Length > 0)
        stringBuilder.Append(" | ");
      stringBuilder.Append(Texts.Instance.GetText("ping"));
      stringBuilder.Append(": ");
      stringBuilder.Append(NetworkManager.Instance.PlayerPing[this.playerNick]);
      stringBuilder.Append("ms");
    }
    this.playerDescription.text = stringBuilder.ToString();
  }

  public void ShowMute()
  {
    if (this.muteButton.gameObject.activeSelf)
      return;
    this.muteButton.gameObject.SetActive(true);
  }

  public void ShowUnmute()
  {
    if (this.unmuteButton.gameObject.activeSelf)
      return;
    this.unmuteButton.gameObject.SetActive(true);
  }

  public void HideMute()
  {
    if (!this.muteButton.gameObject.activeSelf)
      return;
    this.muteButton.gameObject.SetActive(false);
  }

  public void HideUnmute()
  {
    if (!this.unmuteButton.gameObject.activeSelf)
      return;
    this.unmuteButton.gameObject.SetActive(false);
  }

  public void DoMute()
  {
    NetworkManager.Instance.DoMute(this.playerSlot);
    if (!NetworkManager.Instance.IsPlayerMutedBySlot(this.playerSlot))
      return;
    this.HideMute();
    this.ShowUnmute();
  }

  public void DoUnmute()
  {
    NetworkManager.Instance.DoUnmute(this.playerSlot);
    if (NetworkManager.Instance.IsPlayerMutedBySlot(this.playerSlot))
      return;
    this.HideUnmute();
    this.ShowMute();
  }

  public void Show()
  {
    if (this.gameObject.activeSelf)
      return;
    this.gameObject.SetActive(true);
  }

  public void Hide()
  {
    if (!this.gameObject.activeSelf)
      return;
    this.gameObject.SetActive(false);
  }
}
