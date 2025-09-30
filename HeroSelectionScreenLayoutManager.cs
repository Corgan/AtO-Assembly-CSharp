// Decompiled with JetBrains decompiler
// Type: HeroSelectionScreenLayoutManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class HeroSelectionScreenLayoutManager : MonoBehaviour
{
  [SerializeField]
  private HeroSelectionScreenLayoutManager.LayoutElement[] layoutElements;

  private void Start()
  {
    if (GameManager.Instance.IsMultiplayer())
    {
      foreach (HeroSelectionScreenLayoutManager.LayoutElement layoutElement in this.layoutElements)
        layoutElement.SetMultiplayerLayout();
    }
    else
    {
      foreach (HeroSelectionScreenLayoutManager.LayoutElement layoutElement in this.layoutElements)
        layoutElement.SetSingleplayerLayout();
    }
  }

  [Serializable]
  public struct LayoutElement
  {
    public Transform transform;
    public bool useCustomPosition;
    public Vector3 singlePlayerPosition;
    public Vector3 multiPlayerPosition;
    public bool useCustomScale;
    public Vector3 singlePlayerScale;
    public Vector3 multiPlayerScale;

    public void SetMultiplayerLayout()
    {
      if (this.useCustomPosition)
        this.transform.localPosition = this.multiPlayerPosition;
      if (!this.useCustomScale)
        return;
      this.transform.localScale = this.multiPlayerScale;
    }

    public void SetSingleplayerLayout()
    {
      if (this.useCustomPosition)
        this.transform.localPosition = this.singlePlayerPosition;
      if (!this.useCustomScale)
        return;
      this.transform.localScale = this.singlePlayerScale;
    }
  }
}
