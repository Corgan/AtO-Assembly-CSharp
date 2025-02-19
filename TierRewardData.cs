// Decompiled with JetBrains decompiler
// Type: TierRewardData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "Tier Reward", menuName = "Tier Reward Data", order = 61)]
public class TierRewardData : ScriptableObject
{
  [SerializeField]
  private int tierNum;
  [SerializeField]
  private int common;
  [SerializeField]
  private int uncommon;
  [SerializeField]
  private int rare;
  [SerializeField]
  private int epic;
  [SerializeField]
  private int mythic;
  [SerializeField]
  private int dust;

  public int TierNum
  {
    get => this.tierNum;
    set => this.tierNum = value;
  }

  public int Common
  {
    get => this.common;
    set => this.common = value;
  }

  public int Uncommon
  {
    get => this.uncommon;
    set => this.uncommon = value;
  }

  public int Rare
  {
    get => this.rare;
    set => this.rare = value;
  }

  public int Epic
  {
    get => this.epic;
    set => this.epic = value;
  }

  public int Mythic
  {
    get => this.mythic;
    set => this.mythic = value;
  }

  public int Dust
  {
    get => this.dust;
    set => this.dust = value;
  }
}
