// Decompiled with JetBrains decompiler
// Type: EpicToonFX.ETFXLightFade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace EpicToonFX
{
  public class ETFXLightFade : MonoBehaviour
  {
    [Header("Seconds to dim the light")]
    public float life = 0.2f;
    public ETFXLightFade.OnLifeEnd onLifeEnd = ETFXLightFade.OnLifeEnd.Destroy;
    private Light li;
    private float initIntensity;

    private void Start()
    {
      this.li = this.GetComponent<Light>();
      if (!((Object) this.li != (Object) null))
        return;
      this.initIntensity = this.li.intensity;
    }

    private void Update()
    {
      if (!((Object) this.li != (Object) null))
        return;
      this.li.intensity -= this.initIntensity * (Time.deltaTime / this.life);
      if ((double) this.li.intensity > 0.0)
        return;
      switch (this.onLifeEnd)
      {
        case ETFXLightFade.OnLifeEnd.Disable:
          this.li.enabled = false;
          break;
        case ETFXLightFade.OnLifeEnd.Destroy:
          Object.Destroy((Object) this.li);
          break;
      }
    }

    public enum OnLifeEnd
    {
      DoNothing,
      Disable,
      Destroy,
    }
  }
}
