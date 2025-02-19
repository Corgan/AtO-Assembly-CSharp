// Decompiled with JetBrains decompiler
// Type: ChallengeTrait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "Challenge Trait", menuName = "Challenge Trait", order = 64)]
public class ChallengeTrait : ScriptableObject
{
  [SerializeField]
  private string id;
  [SerializeField]
  private string name;
  [SerializeField]
  private Sprite icon;
  [SerializeField]
  private bool isMadnessTrait;
  [SerializeField]
  private int order;
  [SerializeField]
  private bool isSingularityTrait;
  [SerializeField]
  private int orderSingularity;

  public string Id
  {
    get => this.id;
    set => this.id = value;
  }

  public string Name
  {
    get => this.name;
    set => this.name = value;
  }

  public int Order
  {
    get => this.order;
    set => this.order = value;
  }

  public bool IsMadnessTrait
  {
    get => this.isMadnessTrait;
    set => this.isMadnessTrait = value;
  }

  public Sprite Icon
  {
    get => this.icon;
    set => this.icon = value;
  }

  public bool IsSingularityTrait
  {
    get => this.isSingularityTrait;
    set => this.isSingularityTrait = value;
  }

  public int OrderSingularity
  {
    get => this.orderSingularity;
    set => this.orderSingularity = value;
  }
}
