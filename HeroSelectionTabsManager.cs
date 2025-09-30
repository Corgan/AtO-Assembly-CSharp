// Decompiled with JetBrains decompiler
// Type: HeroSelectionTabsManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class HeroSelectionTabsManager : MonoBehaviour
{
  public static HeroSelectionTabsManager Instance;
  public HeroSelectionTabsManager.Tabs[] heroTabs;
  public float defaultButtonSize;
  public float selectedButtonSize;

  private void Awake() => HeroSelectionTabsManager.Instance = this;

  public void EnableTab(string catergory)
  {
    int index1 = 0;
    for (int index2 = 0; index2 < this.heroTabs.Length; ++index2)
    {
      this.heroTabs[index2].button.Enable();
      this.heroTabs[index2].button.transform.localScale = Vector3.one * this.defaultButtonSize;
      this.heroTabs[index2].button.text.color = this.heroTabs[index2].textColor;
      if (catergory == this.heroTabs[index2].catergory)
        index1 = index2;
    }
    this.heroTabs[index1].button.Disable();
    this.heroTabs[index1].button.transform.localScale = Vector3.one * this.selectedButtonSize;
    this.heroTabs[index1].button.text.color = this.heroTabs[0].textColor;
  }

  [Serializable]
  public struct Tabs
  {
    public string catergory;
    public BotonGeneric button;
    public Color textColor;
  }
}
