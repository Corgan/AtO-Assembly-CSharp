// Decompiled with JetBrains decompiler
// Type: TomeCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TomeCard : MonoBehaviour
{
  public Transform buttonGold;
  public Transform buttonBlue;
  public Transform buttonRare;

  public void ShowButtons(bool state)
  {
    if (this.buttonGold.gameObject.activeSelf != state)
      this.buttonGold.gameObject.SetActive(state);
    if (this.buttonBlue.gameObject.activeSelf == state)
      return;
    this.buttonBlue.gameObject.SetActive(state);
  }

  public void ShowButtonRare(bool state)
  {
    if (this.buttonRare.gameObject.activeSelf == state)
      return;
    this.buttonRare.gameObject.SetActive(state);
  }
}
