// Decompiled with JetBrains decompiler
// Type: PlayerPerk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class PlayerPerk
{
  public Dictionary<string, string[]> PerkConfigTitle = new Dictionary<string, string[]>();
  public Dictionary<string, List<string>[]> PerkConfigPerks = new Dictionary<string, List<string>[]>();
  public Dictionary<string, int[]> PerkConfigPoints = new Dictionary<string, int[]>();
}
