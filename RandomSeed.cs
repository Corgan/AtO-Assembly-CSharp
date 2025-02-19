// Decompiled with JetBrains decompiler
// Type: RandomSeed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class RandomSeed : MonoBehaviour
{
  private void Start()
  {
    if ((Object) this.GetComponent<SpriteRenderer>() != (Object) null)
    {
      Renderer component = this.GetComponent<Renderer>();
      if ((Object) component != (Object) null && (Object) component.material != (Object) null)
        component.material.SetFloat("_RandomSeed", Random.Range(0.0f, 1000f));
      else
        Debug.LogError((object) ("Missing Renderer or Material: " + this.gameObject.name));
    }
    else
    {
      Image component = this.GetComponent<Image>();
      if ((Object) component != (Object) null)
      {
        if ((Object) component.material != (Object) null)
          component.material.SetFloat("_RandomSeed", Random.Range(0.0f, 1000f));
        else
          Debug.LogError((object) ("Missing Material on UI Image: " + this.gameObject.name));
      }
      else
        Debug.LogError((object) ("Missing Sprite Renderer or UI Image on: " + this.gameObject.name));
    }
  }
}
