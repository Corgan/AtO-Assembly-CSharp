// Decompiled with JetBrains decompiler
// Type: SlashEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class SlashEffect : MonoBehaviour
{
  public Transform theMask;
  private Vector3 initialScale = new Vector3(0.0f, 0.33f, 1f);

  public void Init() => this.StartCoroutine(this.Animation());

  private void Shake()
  {
  }

  private IEnumerator Animation()
  {
    SlashEffect slashEffect = this;
    float scaleY = 0.01f;
    while ((double) scaleY < 0.75)
    {
      slashEffect.theMask.localScale = new Vector3(1f, scaleY, 1f);
      scaleY += 0.035f;
      yield return (object) new WaitForSeconds(0.01f);
    }
    yield return (object) new WaitForSeconds(0.5f);
    while ((double) scaleY > 0.0)
    {
      slashEffect.theMask.localScale = new Vector3(1f, scaleY, 1f);
      scaleY -= 0.035f;
      yield return (object) new WaitForSeconds(0.01f);
    }
    Object.Destroy((Object) slashEffect.gameObject);
  }
}
