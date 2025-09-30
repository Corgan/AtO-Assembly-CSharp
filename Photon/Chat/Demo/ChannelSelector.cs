// Decompiled with JetBrains decompiler
// Type: Photon.Chat.Demo.ChannelSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace Photon.Chat.Demo
{
  public class ChannelSelector : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
  {
    public string Channel;

    public void SetChannel(string channel)
    {
      this.Channel = channel;
      this.GetComponentInChildren<Text>().text = this.Channel;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      Object.FindObjectOfType<ChatGui>().ShowChannel(this.Channel);
    }
  }
}
