// Decompiled with JetBrains decompiler
// Type: BotonDeck
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class BotonDeck : MonoBehaviour
{
  public SpriteRenderer backgroundSR;
  public SpriteRenderer cardSR;
  public int index;

  public void Show(bool state) => this.gameObject.SetActive(state);

  private void OnMouseExit()
  {
    this.backgroundSR.color = new Color(1f, 0.0f, 0.0f);
    this.cardSR.color = new Color(1f, 1f, 1f);
  }

  private void OnMouseEnter()
  {
    this.backgroundSR.color = new Color(0.3f, 0.3f, 0.3f);
    this.cardSR.color = new Color(0.7f, 0.7f, 0.7f);
  }
}
