// Decompiled with JetBrains decompiler
// Type: TMPro.TMP_DigitValidator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace TMPro
{
  [Serializable]
  public class TMP_DigitValidator : TMP_InputValidator
  {
    public override char Validate(ref string text, ref int pos, char ch)
    {
      if (ch < '0' || ch > '9')
        return char.MinValue;
      ++pos;
      return ch;
    }
  }
}
