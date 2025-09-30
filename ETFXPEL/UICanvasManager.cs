// Decompiled with JetBrains decompiler
// Type: ETFXPEL.UICanvasManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace ETFXPEL
{
  public class UICanvasManager : MonoBehaviour
  {
    public static UICanvasManager GlobalAccess;
    public bool MouseOverButton;
    public Text PENameText;
    public Text ToolTipText;
    private RaycastHit rayHit;

    private void Awake() => UICanvasManager.GlobalAccess = this;

    private void Start()
    {
      if (!((Object) this.PENameText != (Object) null))
        return;
      this.PENameText.text = ParticleEffectsLibrary.GlobalAccess.GetCurrentPENameString();
    }

    private void Update()
    {
      if (!this.MouseOverButton && Input.GetMouseButtonUp(0))
        this.SpawnCurrentParticleEffect();
      if (Input.GetKeyUp(KeyCode.A))
        this.SelectPreviousPE();
      if (!Input.GetKeyUp(KeyCode.D))
        return;
      this.SelectNextPE();
    }

    public void UpdateToolTip(ButtonTypes toolTipType)
    {
      if (!((Object) this.ToolTipText != (Object) null))
        return;
      if (toolTipType == ButtonTypes.Previous)
      {
        this.ToolTipText.text = "Select Previous Particle Effect";
      }
      else
      {
        if (toolTipType != ButtonTypes.Next)
          return;
        this.ToolTipText.text = "Select Next Particle Effect";
      }
    }

    public void ClearToolTip()
    {
      if (!((Object) this.ToolTipText != (Object) null))
        return;
      this.ToolTipText.text = "";
    }

    private void SelectPreviousPE()
    {
      ParticleEffectsLibrary.GlobalAccess.PreviousParticleEffect();
      if (!((Object) this.PENameText != (Object) null))
        return;
      this.PENameText.text = ParticleEffectsLibrary.GlobalAccess.GetCurrentPENameString();
    }

    private void SelectNextPE()
    {
      ParticleEffectsLibrary.GlobalAccess.NextParticleEffect();
      if (!((Object) this.PENameText != (Object) null))
        return;
      this.PENameText.text = ParticleEffectsLibrary.GlobalAccess.GetCurrentPENameString();
    }

    private void SpawnCurrentParticleEffect()
    {
      if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out this.rayHit))
        return;
      ParticleEffectsLibrary.GlobalAccess.SpawnParticleEffect(this.rayHit.point);
    }

    public void UIButtonClick(ButtonTypes buttonTypeClicked)
    {
      if (buttonTypeClicked != ButtonTypes.Previous)
      {
        if (buttonTypeClicked != ButtonTypes.Next)
          return;
        this.SelectNextPE();
      }
      else
        this.SelectPreviousPE();
    }
  }
}
