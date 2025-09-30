// Decompiled with JetBrains decompiler
// Type: SpecialValues
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public struct SpecialValues
{
  public Enums.SpecialValueModifierName Name;
  public bool Use;
  public float Multiplier;

  public SpecialValues(Enums.SpecialValueModifierName name, bool use, float multiplier)
  {
    this.Name = name;
    this.Use = use;
    this.Multiplier = multiplier;
  }

  public SpecialValues(SpecialValues specialValues)
  {
    this.Name = specialValues.Name;
    this.Use = specialValues.Use;
    this.Multiplier = specialValues.Multiplier;
  }
}
