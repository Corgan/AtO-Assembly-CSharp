// Decompiled with JetBrains decompiler
// Type: Photon.Chat.Demo.IgnoreUiRaycastWhenInactive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Photon.Chat.Demo
{
  public class IgnoreUiRaycastWhenInactive : MonoBehaviour, ICanvasRaycastFilter
  {
    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
      return this.gameObject.activeInHierarchy;
    }
  }
}
