// Decompiled with JetBrains decompiler
// Type: Photon.Chat.Demo.ChannelSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
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
