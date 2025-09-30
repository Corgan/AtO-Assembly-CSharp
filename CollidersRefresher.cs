// Decompiled with JetBrains decompiler
// Type: CollidersRefresher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CollidersRefresher : MonoBehaviour
{
  private void Start()
  {
    foreach (Collider collider in Object.FindObjectsOfType<Collider>())
    {
      collider.enabled = !collider.enabled;
      collider.enabled = !collider.enabled;
    }
  }
}
