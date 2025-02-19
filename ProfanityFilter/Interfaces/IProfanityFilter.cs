// Decompiled with JetBrains decompiler
// Type: ProfanityFilter.Interfaces.IProfanityFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace ProfanityFilter.Interfaces
{
  public interface IProfanityFilter
  {
    bool IsProfanity(string word);

    ReadOnlyCollection<string> DetectAllProfanities(string sentence);

    ReadOnlyCollection<string> DetectAllProfanities(string sentence, bool removePartialMatches);

    bool ContainsProfanity(string term);

    IAllowList AllowList { get; }

    string CensorString(string sentence);

    string CensorString(string sentence, char censorCharacter);

    string CensorString(string sentence, char censorCharacter, bool ignoreNumbers);

    (int, int, string)? GetCompleteWord(string toCheck, string profanity);

    void AddProfanity(string profanity);

    void AddProfanity(string[] profanityList);

    void AddProfanity(List<string> profanityList);

    bool RemoveProfanity(string profanity);

    bool RemoveProfanity(List<string> profanities);

    bool RemoveProfanity(string[] profanities);

    void Clear();

    int Count { get; }
  }
}
