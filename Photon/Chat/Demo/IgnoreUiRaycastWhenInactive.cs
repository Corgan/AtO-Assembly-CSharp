// Decompiled with JetBrains decompiler
// Type: Photon.Chat.Demo.IgnoreUiRaycastWhenInactive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
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
