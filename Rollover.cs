// Decompiled with JetBrains decompiler
// Type: Rollover
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Rollover : MonoBehaviour
{
  public AudioClip rollOverSound;

  public void PlayRollOver()
  {
    if (AlertManager.Instance.IsActive())
      return;
    GameManager.Instance.PlayAudio(this.rollOverSound);
  }

  private void OnMouseEnter()
  {
    if (!((Object) AlertManager.Instance == (Object) null) || !((Object) this.rollOverSound != (Object) null))
      return;
    BotonGeneric component = this.transform.GetComponent<BotonGeneric>();
    if ((Object) component != (Object) null && !component.buttonEnabled)
      return;
    this.PlayRollOver();
  }
}
