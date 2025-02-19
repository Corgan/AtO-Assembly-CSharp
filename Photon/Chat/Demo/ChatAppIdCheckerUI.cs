// Decompiled with JetBrains decompiler
// Type: Photon.Chat.Demo.ChatAppIdCheckerUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
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
