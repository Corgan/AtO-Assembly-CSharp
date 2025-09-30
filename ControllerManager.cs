// Decompiled with JetBrains decompiler
// Type: ControllerManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class ControllerManager
{
  public static void ControllerUp() => ControllerManager.DoControl(0.0f, 1f);

  public static void ControllerDown() => ControllerManager.DoControl(0.0f, -1f);

  public static void ControllerLeft() => ControllerManager.DoControl(-1f, 0.0f);

  public static void ControllerRight() => ControllerManager.DoControl(1f, 0.0f);

  private static void DoControl(float _x, float _y)
  {
    Debug.Log((object) ("Do Control " + _x.ToString() + "," + _y.ToString()));
    int num = (bool) (Object) MainMenuManager.Instance ? 1 : 0;
  }
}
