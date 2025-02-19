// Decompiled with JetBrains decompiler
// Type: CinematicControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
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
