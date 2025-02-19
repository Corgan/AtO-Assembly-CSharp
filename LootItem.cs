// Decompiled with JetBrains decompiler
// Type: LootItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class LootItem
{
  [SerializeField]
  private CardData lootCard;
  [SerializeField]
  private float lootPercent;
  [SerializeField]
  private Enums.CardType lootType;
  [SerializeField]
  private Enums.CardRarity lootRarity;
  [SerializeField]
  private string lootMisc;

  public CardData LootCard
  {
    get => this.lootCard;
    set => this.lootCard = value;
  }

  public float LootPercent
  {
    get => this.lootPercent;
    set => this.lootPercent = value;
  }

  public Enums.CardType LootType
  {
    get => this.lootType;
    set => this.lootType = value;
  }

  public Enums.CardRarity LootRarity
  {
    get => this.lootRarity;
    set => this.lootRarity = value;
  }

  public string LootMisc
  {
    get => this.lootMisc;
    set => this.lootMisc = value;
  }
}
