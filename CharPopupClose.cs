// Decompiled with JetBrains decompiler
// Type: CharPopupClose
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CharPopupClose : MonoBehaviour
{
  public CharPopup charPopup;
  public GameObject closeRollver;

  private void OnMouseEnter() => this.closeRollver.gameObject.SetActive(true);

  private void OnMouseExit() => this.closeRollver.gameObject.SetActive(false);

  public void OnMouseUp()
  {
    if (!Functions.ClickedThisTransform(this.transform))
      return;
    this.charPopup.Close();
  }
}
