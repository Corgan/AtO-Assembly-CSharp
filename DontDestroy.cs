// Decompiled with JetBrains decompiler
// Type: DontDestroy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class DontDestroy : MonoBehaviour
{
  private void Awake() => Object.DontDestroyOnLoad((Object) this.gameObject);
}
