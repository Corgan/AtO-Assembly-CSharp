// Decompiled with JetBrains decompiler
// Type: RoundThermometer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class RoundThermometer
{
  [SerializeField]
  private int round;
  [SerializeField]
  private ThermometerData thermometerData;

  public int Round
  {
    get => this.round;
    set => this.round = value;
  }

  public ThermometerData ThermometerData
  {
    get => this.thermometerData;
    set => this.thermometerData = value;
  }
}
