// Decompiled with JetBrains decompiler
// Type: MenuButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public class MenuButton : MonoBehaviour
{
  public TMP_Text buttonText;

  private bool IsMouseOverThis()
  {
    PointerEventData eventData = new PointerEventData(EventSystem.current);
    eventData.position = (Vector2) Input.mousePosition;
    List<RaycastResult> raycastResults = new List<RaycastResult>();
    EventSystem.current.RaycastAll(eventData, raycastResults);
    for (int index = 0; index < raycastResults.Count; ++index)
    {
      if (raycastResults[index].gameObject.name == this.gameObject.name)
        return true;
    }
    return false;
  }

  public void HoverOn()
  {
    GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonHover, 0.1f);
    this.buttonText.margin = new Vector4(6f, 0.0f, 0.0f, 0.0f);
    this.buttonText.color = new Color(1f, 0.6f, 0.0f);
    GameManager.Instance.SetCursorHover();
  }

  public void HoverOff()
  {
    this.buttonText.margin = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
    this.buttonText.color = Color.white;
    GameManager.Instance.SetCursorPlain();
  }
}
