// Decompiled with JetBrains decompiler
// Type: PopupText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using WebSocketSharp;

#nullable disable
public class PopupText : MonoBehaviour
{
  public string id;
  public string position = "";
  public string text = "";

  public void SetId(string _id) => this.id = _id;

  private void OnMouseEnter()
  {
    if ((AlertManager.Instance.IsActive() || GameManager.Instance.IsTutorialActive() || SettingsManager.Instance.IsActive() || DamageMeterManager.Instance.IsActive()) && this.id.Trim() != "optionalTelemetryTooltip")
      return;
    if (this.id.Trim() == "tutorialCharacters")
      this.transform.parent.GetComponentInChildren<HeroSelection>();
    if (this.id.Trim() != "")
    {
      string text = Texts.Instance.GetText(this.id);
      if (text.IsNullOrEmpty())
        return;
      PopupManager.Instance.SetText(text, true, this.position, true);
    }
    else
    {
      if (!(this.text.Trim() != ""))
        return;
      PopupManager.Instance.SetText(this.text.Trim(), true, this.position, true);
    }
  }

  private void OnMouseDown()
  {
    if (this.id.Trim() == "tutorialCharacters")
      HeroSelectionManager.Instance?.charPopupMini.SetSubClassData(this.transform.parent.GetComponentInChildren<HeroSelection>().subClassData);
    if (!(this.id.Trim() == "charMiniPopupShow"))
      return;
    Transform parent = this.transform.parent;
    HeroSelectionManager.Instance?.charPopupMini.SetSubClassData(this.GetComponent<HeroSelectionInfoHover>().heroSelection.subClassData);
  }

  private void OnMouseExit() => PopupManager.Instance.ClosePopup();
}
