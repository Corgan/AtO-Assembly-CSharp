// Decompiled with JetBrains decompiler
// Type: AllIn1SpriteShader.SetGlobalTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace AllIn1SpriteShader
{
  [ExecuteInEditMode]
  public class SetGlobalTime : MonoBehaviour
  {
    private int globalTime;

    private void Start() => this.globalTime = Shader.PropertyToID("globalUnscaledTime");

    private void Update() => Shader.SetGlobalFloat(this.globalTime, Time.unscaledTime / 20f);
  }
}
