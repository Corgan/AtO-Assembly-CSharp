// Decompiled with JetBrains decompiler
// Type: ProfanityFilter.Interfaces.IAllowList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections.ObjectModel;

#nullable disable
namespace ProfanityFilter.Interfaces
{
  public interface IAllowList
  {
    void Add(string wordToAllowlist);

    bool Contains(string wordToCheck);

    bool Remove(string wordToRemove);

    void Clear();

    int Count { get; }

    ReadOnlyCollection<string> ToList { get; }
  }
}
