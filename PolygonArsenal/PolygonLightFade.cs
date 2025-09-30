// Decompiled with JetBrains decompiler
// Type: PolygonArsenal.PolygonLightFade
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace PolygonArsenal
{
  public class PolygonLightFade : MonoBehaviour
  {
    [Header("Seconds to dim the light")]
    public float life = 0.2f;
    public bool killAfterLife = true;
    private Light li;
    private float initIntensity;

    private void Start()
    {
      if ((bool) (Object) this.gameObject.GetComponent<Light>())
      {
        this.li = this.gameObject.GetComponent<Light>();
        this.initIntensity = this.li.intensity;
      }
      else
        MonoBehaviour.print((object) ("No light object found on " + this.gameObject.name));
    }

    private void Update()
    {
      if (!(bool) (Object) this.gameObject.GetComponent<Light>())
        return;
      this.li.intensity -= this.initIntensity * (Time.deltaTime / this.life);
      if (!this.killAfterLife || (double) this.li.intensity > 0.0)
        return;
      Object.Destroy((Object) this.gameObject);
    }
  }
}
