// Decompiled with JetBrains decompiler
// Type: TestingHeroBuild
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
public class TestingHeroBuild : MonoBehaviour
{
  [SerializeField]
  private int id;

  private void OnMouseUpAsButton()
  {
    if (EventSystem.current.IsPointerOverGameObject())
      return;
    TeamManagement.Instance.ShowHeroBuildPanel(this.id);
  }
}
