// Decompiled with JetBrains decompiler
// Type: TMPro.Examples.ShaderPropAnimator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace TMPro.Examples
{
  public class ShaderPropAnimator : MonoBehaviour
  {
    private Renderer m_Renderer;
    private Material m_Material;
    public AnimationCurve GlowCurve;
    public float m_frame;

    private void Awake()
    {
      this.m_Renderer = this.GetComponent<Renderer>();
      this.m_Material = this.m_Renderer.material;
    }

    private void Start() => this.StartCoroutine(this.AnimateProperties());

    private IEnumerator AnimateProperties()
    {
      this.m_frame = Random.Range(0.0f, 1f);
      while (true)
      {
        float num = this.GlowCurve.Evaluate(this.m_frame);
        this.m_Material.SetFloat(ShaderUtilities.ID_GlowPower, num);
        this.m_frame += Time.deltaTime * Random.Range(0.2f, 0.3f);
        yield return (object) new WaitForEndOfFrame();
      }
    }
  }
}
