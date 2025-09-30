// Decompiled with JetBrains decompiler
// Type: HeroCards
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class HeroCards
{
  [SerializeField]
  private CardData card;
  [SerializeField]
  private int unitsInDeck;

  public CardData Card
  {
    get => this.card;
    set => this.card = value;
  }

  public int UnitsInDeck
  {
    get => this.unitsInDeck;
    set => this.unitsInDeck = value;
  }
}
