// Decompiled with JetBrains decompiler
// Type: StringComparerWithZeroPadding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
public class StringComparerWithZeroPadding : IComparer<string>
{
  public int Compare(string x, string y)
  {
    string[] strArray1 = x.Split('_', StringSplitOptions.None);
    string[] strArray2 = y.Split('_', StringSplitOptions.None);
    for (int index = 0; index < Math.Min(strArray1.Length, strArray2.Length); ++index)
    {
      string s1 = strArray1[index];
      string s2 = strArray2[index];
      string str = this.FillWithZeros(s1, Math.Max(s1.Length, s2.Length));
      string strB = this.FillWithZeros(s2, Math.Max(str.Length, s2.Length));
      int num = str.CompareTo(strB);
      if (num != 0)
        return num;
    }
    return x.Length.CompareTo(y.Length);
  }

  private string FillWithZeros(string s, int maxLength)
  {
    if (s.Length == maxLength)
      return s;
    int num = -1;
    for (int index = 0; index < s.Length; ++index)
    {
      if (char.IsDigit(s[index]))
      {
        num = index;
        break;
      }
    }
    if (num == -1)
      return s;
    string str1 = s.Substring(0, num);
    string str2 = s.Substring(num).PadLeft(maxLength - str1.Length, '0');
    return str1 + str2;
  }

  private bool IsNumeric(string value) => int.TryParse(value, out int _);
}
