// Decompiled with JetBrains decompiler
// Type: ChallengeMadness
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ChallengeMadness : MonoBehaviour
{
  public SpriteRenderer background;
  public SpriteRenderer icon;

  public void SetBackground(string _color) => this.background.color = Functions.HexToColor(_color);

  public void SetDisable() => this.SetBackground("#353535");

  public void SetActive() => this.SetBackground("#AD844D");

  public void SetDefault() => this.SetBackground("#5D3578");

  public void SetIcon(Sprite _sprite) => this.icon.sprite = _sprite;
}
