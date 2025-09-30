// Decompiled with JetBrains decompiler
// Type: CardUtils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
public static class CardUtils
{
  private static readonly string[] Suffixes = new string[3]
  {
    "",
    "a",
    "b"
  };
  public static readonly string[] MasterReweaverKeys = ((IEnumerable<string>) CardUtils.Suffixes).Select<string, string>((Func<string, string>) (suffix => "masterreweaver" + suffix)).ToArray<string>();

  public static int GetMaxPlaceholderFormattedStringIndex(string text)
  {
    int val1 = -1;
    for (int index = 0; index < text.Length; ++index)
    {
      if (text[index] == '{')
      {
        int startIndex = index + 1;
        int num = text.IndexOf('}', startIndex);
        if (num > startIndex)
        {
          int result;
          if (int.TryParse(text.Substring(startIndex, num - startIndex), out result))
            val1 = Math.Max(val1, result);
          index = num;
        }
      }
    }
    return val1;
  }
}
