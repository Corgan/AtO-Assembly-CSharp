// Decompiled with JetBrains decompiler
// Type: BotonAdvancedCraft
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BotonAdvancedCraft : MonoBehaviour
{
  public Transform circleOn;
  public Transform circleOff;

  public void SetState(bool state)
  {
    if (state)
    {
      this.circleOn.gameObject.SetActive(true);
      this.circleOff.gameObject.SetActive(false);
    }
    else
    {
      this.circleOn.gameObject.SetActive(false);
      this.circleOff.gameObject.SetActive(true);
    }
  }
}
