// Decompiled with JetBrains decompiler
// Type: PlayerDeck
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class PlayerDeck
{
  public Dictionary<string, string[]> DeckTitle = new Dictionary<string, string[]>();
  public Dictionary<string, List<string>[]> DeckCards = new Dictionary<string, List<string>[]>();
}
