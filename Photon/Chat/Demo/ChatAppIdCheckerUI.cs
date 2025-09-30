// Decompiled with JetBrains decompiler
// Type: Photon.Chat.Demo.ChatAppIdCheckerUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Photon.Chat.Demo
{
  [ExecuteInEditMode]
  public class ChatAppIdCheckerUI : MonoBehaviour
  {
    public Text Description;

    public void Update()
    {
      if (string.IsNullOrEmpty(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat))
      {
        if (!((Object) this.Description != (Object) null))
          return;
        this.Description.text = "<Color=Red>WARNING:</Color>\nPlease setup a Chat AppId in the PhotonServerSettings file.";
      }
      else
      {
        if (!((Object) this.Description != (Object) null))
          return;
        this.Description.text = string.Empty;
      }
    }
  }
}
