// Decompiled with JetBrains decompiler
// Type: RoomList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Text;
using TMPro;
using UnityEngine;

#nullable disable
public class RoomList : MonoBehaviour
{
  [SerializeField]
  private string _roomName = "";
  private string _roomPwd = "";
  public TMP_Text _Name;
  public TMP_Text _Creator;
  public TMP_Text _Players;
  public TMP_Text _Version;
  public GameObject lfm;
  private string _crossplay;
  private string _platform;
  private int _ActivePlayers;
  private int _MaxPlayers;
  public Transform _Lock;

  public string RoomName => this._roomName;

  public void SetRoomName(string text) => this._roomName = text;

  public void SetLfm(string state)
  {
    if (state != "")
      this.lfm.SetActive(true);
    else
      this.lfm.SetActive(false);
  }

  public void SetRoomDescription(string text) => this._Name.text = text;

  public void SetRoomPlayers(int numPlayers, int maxPlayers)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(numPlayers).Append("<size=-10><voffset=4>/</voffset></size>").Append(maxPlayers);
    this._Players.text = stringBuilder.ToString();
    this._ActivePlayers = numPlayers;
    this._MaxPlayers = maxPlayers;
  }

  public void SetRoomCrossplayPlatform(string crossplay, string platform)
  {
    this._crossplay = crossplay;
    this._platform = platform;
  }

  public void SetRoomVersion(string version, string platform = null)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if (platform != null && platform.Trim() != string.Empty)
      stringBuilder.Append(platform).Append("<space=8><size=-4><voffset=3>|</voffset></size><space=8>");
    stringBuilder.Append(version);
    this._Version.text = stringBuilder.ToString();
  }

  public bool IsEmpty() => this._ActivePlayers == 0;

  public bool IsFull() => this._ActivePlayers == this._MaxPlayers;

  private int GetRoomPlayers() => this._ActivePlayers;

  public void SetRoomCreator(string text) => this._Creator.text = text;

  public void SetRoomPassword(string text)
  {
    this._roomPwd = text;
    if (text != "")
      this.SetLock(true);
    else
      this.SetLock(false);
  }

  private void SetLock(bool value)
  {
    if ((Object) this._Lock == (Object) null || (Object) this._Lock.gameObject == (Object) null)
      return;
    if (value)
      this._Lock.gameObject.SetActive(true);
    else
      this._Lock.gameObject.SetActive(false);
  }

  public void JoinRoom() => LobbyManager.Instance.JoinRoom(this._roomName, this._roomPwd);

  public bool Updated { get; set; }

  public string Crossplay
  {
    get => this._crossplay;
    set => this._crossplay = value;
  }

  public string Platform
  {
    get => this._platform;
    set => this._platform = value;
  }
}
