// Decompiled with JetBrains decompiler
// Type: HeroCards
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
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
