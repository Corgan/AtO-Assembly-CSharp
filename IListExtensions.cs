// Decompiled with JetBrains decompiler
// Type: IListExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public static class IListExtensions
{
  public static void Shuffle<T>(this IList<T> ts, int seed = -1)
  {
    if (seed != -1)
      Random.InitState(seed);
    int count = ts.Count;
    int num = count - 1;
    for (int index1 = 0; index1 < num; ++index1)
    {
      int index2 = Random.Range(index1, count);
      T t = ts[index1];
      ts[index1] = ts[index2];
      ts[index2] = t;
    }
  }
}
