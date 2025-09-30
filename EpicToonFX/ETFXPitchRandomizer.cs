// Decompiled with JetBrains decompiler
// Type: EpicToonFX.ETFXPitchRandomizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace EpicToonFX
{
  public class ETFXPitchRandomizer : MonoBehaviour
  {
    public float randomPercent = 10f;

    private void Start()
    {
      this.transform.GetComponent<AudioSource>().pitch *= 1f + Random.Range((float) (-(double) this.randomPercent / 100.0), this.randomPercent / 100f);
    }
  }
}
