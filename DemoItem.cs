// Decompiled with JetBrains decompiler
// Type: DemoItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DemoItem : MonoBehaviour
{
  private static Vector3 lookAtZ = new Vector3(0.0f, 0.0f, 1f);

  private void Update() => this.transform.LookAt(this.transform.position + DemoItem.lookAtZ);
}
