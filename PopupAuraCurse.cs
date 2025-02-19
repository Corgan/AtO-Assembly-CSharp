// Decompiled with JetBrains decompiler
// Type: PopupAuraCurse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PopupAuraCurse : MonoBehaviour
{
  private AuraCurseData acData;
  private int charges;
  private bool fast;

  public void SetAuraCurse(AuraCurseData _acData, int _charges, bool _fast)
  {
    this.acData = _acData;
    this.charges = _charges;
    this.fast = _fast;
  }

  private void OnMouseEnter()
  {
    if (!((Object) this.acData != (Object) null))
      return;
    PopupManager.Instance.SetAuraCurse(this.transform, this.acData.Id, this.charges.ToString(), this.fast);
  }

  private void OnMouseExit() => PopupManager.Instance.ClosePopup();
}
