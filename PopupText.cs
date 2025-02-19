// Decompiled with JetBrains decompiler
// Type: PopupText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PopupText : MonoBehaviour
{
  public string id;
  public string position = "";
  public string text = "";

  public void SetId(string _id) => this.id = _id;

  private void OnMouseEnter()
  {
    if (AlertManager.Instance.IsActive() || GameManager.Instance.IsTutorialActive() || SettingsManager.Instance.IsActive() || DamageMeterManager.Instance.IsActive())
      return;
    if (this.id.Trim() != "")
    {
      PopupManager.Instance.SetText(Texts.Instance.GetText(this.id), true, this.position, true);
    }
    else
    {
      if (!(this.text.Trim() != ""))
        return;
      PopupManager.Instance.SetText(this.text.Trim(), true, this.position, true);
    }
  }

  private void OnMouseExit() => PopupManager.Instance.ClosePopup();
}
