// Decompiled with JetBrains decompiler
// Type: CurvedLinePoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CurvedLinePoint : MonoBehaviour
{
  [HideInInspector]
  public bool showGizmo = true;
  [HideInInspector]
  public float gizmoSize = 0.1f;
  [HideInInspector]
  public Color gizmoColor = new Color(1f, 0.0f, 0.0f, 0.5f);

  private void OnDrawGizmos()
  {
    if (!this.showGizmo)
      return;
    Gizmos.color = this.gizmoColor;
    Gizmos.DrawSphere(this.transform.position, this.gizmoSize);
  }

  private void OnDrawGizmosSelected()
  {
    CurvedLineRenderer component = this.transform.parent.GetComponent<CurvedLineRenderer>();
    if (!((Object) component != (Object) null))
      return;
    component.Update();
  }
}
