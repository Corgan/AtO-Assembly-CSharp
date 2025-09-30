// Decompiled with JetBrains decompiler
// Type: EpicToonFX.ETFXSpriteBouncer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace EpicToonFX
{
  public class ETFXSpriteBouncer : MonoBehaviour
  {
    public float scaleAmount = 1.1f;
    public float scaleDuration = 1f;
    private Vector3 startScale;
    private float scaleTimer;

    private void Start()
    {
      this.startScale = this.transform.localScale;
      if ((double) this.startScale.y == 1.0)
        return;
      this.startScale = new Vector3(this.startScale.x, this.startScale.y / this.scaleAmount, this.startScale.z);
    }

    private void Update()
    {
      this.scaleTimer += Time.deltaTime;
      this.transform.localScale = new Vector3(this.startScale.x, Mathf.Lerp(this.startScale.y, this.startScale.y * this.scaleAmount, Mathf.Clamp01(this.scaleTimer / this.scaleDuration)) + Mathf.PingPong(this.scaleTimer / this.scaleDuration, 0.1f), this.startScale.z);
    }
  }
}
