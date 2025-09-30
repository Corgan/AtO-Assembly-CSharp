// Decompiled with JetBrains decompiler
// Type: TitleMovement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
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
