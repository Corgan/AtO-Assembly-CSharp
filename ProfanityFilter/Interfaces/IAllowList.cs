// Decompiled with JetBrains decompiler
// Type: ProfanityFilter.Interfaces.IAllowList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
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
