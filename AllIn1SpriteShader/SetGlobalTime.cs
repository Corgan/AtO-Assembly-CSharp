// Decompiled with JetBrains decompiler
// Type: AllIn1SpriteShader.SetGlobalTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
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
