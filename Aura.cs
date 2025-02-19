// Decompiled with JetBrains decompiler
// Type: Aura
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class Aura
{
  [SerializeField]
  private AuraCurseData acData;
  [SerializeField]
  private int auraCharges;

  public void SetAura(AuraCurseData _acData, int _auraCharges)
  {
    this.acData = _acData;
    this.auraCharges = _auraCharges;
  }

  public void AddCharges(int charges)
  {
    try
    {
      int num = checked (this.auraCharges + charges);
      this.auraCharges += charges;
    }
    catch (OverflowException ex)
    {
      Debug.LogWarning((object) ("AddCharges exception-> " + ex?.ToString()));
      this.auraCharges = int.MaxValue;
    }
  }

  public int GetCharges() => this.auraCharges;

  public void ConsumeAura() => --this.auraCharges;

  public void ConsumeAll() => this.auraCharges = 0;

  public AuraCurseData ACData
  {
    get => this.acData;
    set => this.acData = value;
  }

  public int AuraCharges
  {
    get => this.auraCharges;
    set => this.auraCharges = value;
  }
}
