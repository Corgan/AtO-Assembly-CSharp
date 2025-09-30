// Decompiled with JetBrains decompiler
// Type: PerkColumn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class PerkColumn : MonoBehaviour
{
  public TMP_Text columnTitle;
  public Transform[] perkTransform;
  private PerkColumnItem[] perkColumnItem;
  public Transform lockIcon;
  public Transform disableMask;

  private void AwakeInit()
  {
    this.perkColumnItem = new PerkColumnItem[this.perkTransform.Length];
    for (int index = 0; index < this.perkTransform.Length; ++index)
      this.perkColumnItem[index] = this.perkTransform[index].GetComponent<PerkColumnItem>();
  }

  public void Init(
    string _className,
    string _heroName,
    int _index,
    int _pointsAvailable,
    int _pointsUsed)
  {
    if (this.perkColumnItem == null)
      this.AwakeInit();
    for (int _subindex = 0; _subindex < this.perkColumnItem.Length; ++_subindex)
      this.perkColumnItem[_subindex].SetPerk(_className, _heroName, _index, _subindex, _pointsAvailable, _pointsUsed);
  }

  public void DisableColumn(bool state)
  {
    this.lockIcon.gameObject.SetActive(state);
    this.disableMask.gameObject.SetActive(state);
    if (this.perkColumnItem == null)
      this.AwakeInit();
    for (int index = 0; index < this.perkColumnItem.Length; ++index)
      this.perkColumnItem[index].EnablePerk(!state);
  }
}
