// Decompiled with JetBrains decompiler
// Type: TMPro.TMP_DigitValidator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
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
