// Decompiled with JetBrains decompiler
// Type: PolygonArsenal.PolygonRotation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace PolygonArsenal
{
  public class PolygonRotation : MonoBehaviour
  {
    [Header("Rotate axises by degrees per second")]
    public Vector3 rotateVector = Vector3.zero;
    public PolygonRotation.spaceEnum rotateSpace;

    private void Start()
    {
    }

    private void Update()
    {
      if (this.rotateSpace == PolygonRotation.spaceEnum.Local)
        this.transform.Rotate(this.rotateVector * Time.deltaTime);
      if (this.rotateSpace != PolygonRotation.spaceEnum.World)
        return;
      this.transform.Rotate(this.rotateVector * Time.deltaTime, Space.World);
    }

    public enum spaceEnum
    {
      Local,
      World,
    }
  }
}
