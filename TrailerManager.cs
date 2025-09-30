// Decompiled with JetBrains decompiler
// Type: TrailerManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TrailerManager : MonoBehaviour
{
  public static TrailerManager Instance { get; private set; }

  private void Awake()
  {
    if ((Object) GameManager.Instance == (Object) null)
    {
      SceneStatic.LoadByName("TrailerEnd");
    }
    else
    {
      if ((Object) TrailerManager.Instance == (Object) null)
        TrailerManager.Instance = this;
      else if ((Object) TrailerManager.Instance != (Object) this)
        Object.Destroy((Object) this);
      GameManager.Instance.SetCamera();
      NetworkManager.Instance.StartStopQueue(true);
      GameManager.Instance.SceneLoaded();
    }
  }
}
