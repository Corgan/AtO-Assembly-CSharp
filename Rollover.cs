// Decompiled with JetBrains decompiler
// Type: Rollover
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
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
