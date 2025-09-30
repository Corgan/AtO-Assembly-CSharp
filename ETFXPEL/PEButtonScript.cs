// Decompiled with JetBrains decompiler
// Type: ETFXPEL.PEButtonScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace ETFXPEL
{
  public class PEButtonScript : 
    MonoBehaviour,
    IEventSystemHandler,
    IPointerEnterHandler,
    IPointerExitHandler
  {
    private Button myButton;
    public ButtonTypes ButtonType;

    private void Start() => this.myButton = this.gameObject.GetComponent<Button>();

    public void OnPointerEnter(PointerEventData eventData)
    {
      UICanvasManager.GlobalAccess.MouseOverButton = true;
      UICanvasManager.GlobalAccess.UpdateToolTip(this.ButtonType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      UICanvasManager.GlobalAccess.MouseOverButton = false;
      UICanvasManager.GlobalAccess.ClearToolTip();
    }

    public void OnButtonClicked() => UICanvasManager.GlobalAccess.UIButtonClick(this.ButtonType);
  }
}
