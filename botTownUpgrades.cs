// Decompiled with JetBrains decompiler
// Type: botTownUpgrades
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class botTownUpgrades : MonoBehaviour
{
  public SpriteRenderer bgRenderer;

  private void OnMouseEnter()
  {
    if (AlertManager.Instance.IsActive() || SettingsManager.Instance.IsActive())
      return;
    this.transform.localScale = new Vector3(0.52f, 0.52f, 1f);
    this.bgRenderer.color = new Color(0.7f, 0.7f, 0.7f);
    GameManager.Instance.PlayLibraryAudio("ui_click");
  }

  private void OnMouseExit()
  {
    if (AlertManager.Instance.IsActive() || SettingsManager.Instance.IsActive())
      return;
    this.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
    this.bgRenderer.color = Color.white;
  }

  private void OnMouseUp()
  {
    if (!Functions.ClickedThisTransform(this.transform) || AlertManager.Instance.IsActive() || SettingsManager.Instance.IsActive())
      return;
    if (AtOManager.Instance.TownTutorialStep > -1 && AtOManager.Instance.TownTutorialStep < 3)
      AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("tutorialTownNeedComplete"));
    else
      TownManager.Instance.ShowTownUpgrades(true);
  }
}
