// Decompiled with JetBrains decompiler
// Type: CharPopupClose
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
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
