// Decompiled with JetBrains decompiler
// Type: CinematicControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CinematicControl : MonoBehaviour
{
  public void SetTextIntro(int _textIndex)
  {
    CinematicManager.Instance.DoText("intro", _textIndex, 0);
  }

  public void SetTextIntroBig(int _textIndex)
  {
    CinematicManager.Instance.DoText("intro", _textIndex, 1);
  }

  public void SetTextOutro(int _textIndex)
  {
    CinematicManager.Instance.DoText("outro", _textIndex, 0);
  }

  public void SetTextOutroBig(int _textIndex)
  {
    CinematicManager.Instance.DoText("outro", _textIndex, 1);
  }
}
