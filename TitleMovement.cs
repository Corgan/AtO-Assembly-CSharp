// Decompiled with JetBrains decompiler
// Type: TitleMovement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class TitleMovement : MonoBehaviour
{
  public TMP_Text titleText;
  public Color titleColor;
  public string idTranslate = "";
  public string directText = "";

  private void Awake() => this.titleText.color = this.titleColor;

  private void Start()
  {
    if (this.idTranslate != "")
    {
      this.SetText(Texts.Instance.GetText(this.idTranslate));
    }
    else
    {
      if (!(this.directText != ""))
        return;
      this.SetText(this.directText);
    }
  }

  public void SetText(string text) => this.titleText.text = text;

  public void SetColor(Color color) => this.titleText.color = color;
}
