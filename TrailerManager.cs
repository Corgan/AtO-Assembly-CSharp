// Decompiled with JetBrains decompiler
// Type: TrailerManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
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
