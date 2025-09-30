// Decompiled with JetBrains decompiler
// Type: PopupAuraCurse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
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
