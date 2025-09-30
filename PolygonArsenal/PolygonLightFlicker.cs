// Decompiled with JetBrains decompiler
// Type: PolygonArsenal.PolygonLightFlicker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace PolygonArsenal
{
  public class PolygonLightFlicker : MonoBehaviour
  {
    public string waveFunction = "sin";
    public float startValue;
    public float amplitude = 1f;
    public float phase;
    public float frequency = 0.5f;
    private Color originalColor;

    private void Start() => this.originalColor = this.GetComponent<Light>().color;

    private void Update()
    {
      Light component = this.GetComponent<Light>();
      Color color1 = this.originalColor * this.EvalWave();
      color1.r = Mathf.Clamp(color1.r, 0.0f, 1f);
      color1.g = Mathf.Clamp(color1.g, 0.0f, 1f);
      color1.b = Mathf.Clamp(color1.b, 0.0f, 1f);
      Color color2 = color1;
      component.color = color2;
    }

    private float EvalWave()
    {
      float f = (Time.time + this.phase) * this.frequency;
      float num = f - Mathf.Floor(f);
      return (!(this.waveFunction == "sin") ? (!(this.waveFunction == "tri") ? (!(this.waveFunction == "sqr") ? (!(this.waveFunction == "saw") ? (!(this.waveFunction == "inv") ? (!(this.waveFunction == "noise") ? 1f : (float) (1.0 - (double) Random.value * 2.0)) : 1f - num) : num) : ((double) num >= 0.5 ? -1f : 1f)) : ((double) num >= 0.5 ? (float) (-4.0 * (double) num + 3.0) : (float) (4.0 * (double) num - 1.0))) : Mathf.Sin((float) ((double) num * 2.0 * 3.1415927410125732))) * this.amplitude + this.startValue;
    }
  }
}
