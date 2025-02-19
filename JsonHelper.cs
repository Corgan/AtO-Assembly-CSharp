// Decompiled with JetBrains decompiler
// Type: JsonHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public static class JsonHelper
{
  public static T[] FromJson<T>(string json)
  {
    return JsonUtility.FromJson<JsonHelper.Wrapper<T>>(json).Items;
  }

  public static string ToJson<T>(T[] array)
  {
    return JsonUtility.ToJson((object) new JsonHelper.Wrapper<T>()
    {
      Items = array
    });
  }

  public static string ToJson<T>(T[] array, bool prettyPrint)
  {
    return JsonUtility.ToJson((object) new JsonHelper.Wrapper<T>()
    {
      Items = array
    }, prettyPrint);
  }

  [Serializable]
  private class Wrapper<T>
  {
    public T[] Items;
  }
}
