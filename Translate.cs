// Decompiled with JetBrains decompiler
// Type: Translate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using TMPro;
using UnityEngine;

#nullable disable
public class Translate : MonoBehaviour
{
  public string textId = "";
  private TMP_Text textObj;

  private void Awake() => this.textObj = this.GetComponent<TMP_Text>();

  private void Start() => this.SetText();

  public void SetText()
  {
    if ((Object) this.textObj == (Object) null || (Object) Texts.Instance == (Object) null || !(this.textId != ""))
      return;
    this.StartCoroutine(this.SetTextCo());
  }

  private IEnumerator SetTextCo()
  {
    int breakInt = 0;
    while (!Texts.Instance.GotTranslations())
    {
      yield return (object) Globals.Instance.WaitForSeconds(1f / 1000f);
      ++breakInt;
      if (breakInt > 1000)
      {
        Debug.Log((object) (this.textId + " BROKE " + Globals.Instance.CurrentLang));
        yield break;
      }
    }
    this.textObj.text = Texts.Instance.GetText(this.textId);
  }
}
