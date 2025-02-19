// Decompiled with JetBrains decompiler
// Type: PopupHPBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
public class PopupHPBar : MonoBehaviour
{
  private CharacterItem charItem;

  private void Awake()
  {
    this.charItem = (CharacterItem) this.transform.parent.transform.parent.GetComponent<HeroItem>();
    if (!((Object) this.charItem == (Object) null))
      return;
    this.charItem = (CharacterItem) this.transform.parent.transform.parent.GetComponent<NPCItem>();
  }

  private void OnMouseEnter()
  {
    List<string> forThisCharacter = this.charItem.CalculateDamagePrePostForThisCharacter();
    if (forThisCharacter.Count <= 3)
      return;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("<size=+2><align=left>");
    for (int index = 3; index < forThisCharacter.Count; ++index)
    {
      stringBuilder.Append(forThisCharacter[index]);
      stringBuilder.Append("<br>");
    }
    PopupManager.Instance.SetText(stringBuilder.ToString(), true, alwaysCenter: true);
  }

  private void OnMouseExit() => PopupManager.Instance.ClosePopup();
}
