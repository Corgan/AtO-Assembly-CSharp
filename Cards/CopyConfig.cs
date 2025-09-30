// Decompiled with JetBrains decompiler
// Type: Cards.CopyConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Cards
{
  [Serializable]
  public class CopyConfig
  {
    [SerializeField]
    private bool copyNameFromOriginal;
    [SerializeField]
    private bool copyImageFromOriginal;
    [SerializeField]
    private bool copyCardUpgradedFromOriginal;
    [SerializeField]
    private bool copyCardTypeFromOriginal;

    public bool CopyNameFromOriginal => this.copyNameFromOriginal;

    public bool CopyImageFromOriginal => this.copyImageFromOriginal;

    public bool CopyCardUpgradedFromOriginal => this.copyCardUpgradedFromOriginal;

    public bool CopyCardTypeFromOriginal => this.copyCardTypeFromOriginal;
  }
}
