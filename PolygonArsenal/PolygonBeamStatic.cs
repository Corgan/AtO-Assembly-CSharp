// Decompiled with JetBrains decompiler
// Type: PolygonArsenal.PolygonBeamStatic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace PolygonArsenal
{
  public class PolygonBeamStatic : MonoBehaviour
  {
    [Header("Prefabs")]
    public GameObject beamLineRendererPrefab;
    public GameObject beamStartPrefab;
    public GameObject beamEndPrefab;
    private GameObject beamStart;
    private GameObject beamEnd;
    private GameObject beam;
    private LineRenderer line;
    [Header("Beam Options")]
    public bool beamCollides = true;
    public float beamLength = 100f;
    public float beamEndOffset;
    public float textureScrollSpeed;
    public float textureLengthScale = 1f;
    [Header("Width Pulse Options")]
    public float widthMultiplier = 1.5f;
    private float customWidth;
    private float originalWidth;
    private float lerpValue;
    public float pulseSpeed = 1f;
    private bool pulseExpanding = true;

    private void Start()
    {
      this.SpawnBeam();
      this.originalWidth = this.line.startWidth;
      this.customWidth = this.originalWidth * this.widthMultiplier;
    }

    private void FixedUpdate()
    {
      if ((bool) (Object) this.beam)
      {
        this.line.SetPosition(0, this.transform.position);
        Vector3 vector3_1 = this.transform.position + this.transform.forward * this.beamLength;
        RaycastHit hitInfo;
        Vector3 vector3_2;
        if (this.beamCollides && Physics.Raycast(this.transform.position, this.transform.forward, out hitInfo))
        {
          Vector3 b = hitInfo.point - this.transform.forward * this.beamEndOffset;
          vector3_2 = (double) Vector3.Distance(this.transform.position, b) > (double) this.beamLength ? this.transform.position + this.transform.forward * this.beamLength : b;
        }
        else
          vector3_2 = this.transform.position + this.transform.forward * this.beamLength;
        this.line.SetPosition(1, vector3_2);
        this.beamStart.transform.position = this.transform.position;
        this.beamStart.transform.LookAt(vector3_2);
        this.beamEnd.transform.position = vector3_2;
        this.beamEnd.transform.LookAt(this.beamStart.transform.position);
        this.line.material.mainTextureScale = new Vector2(Vector3.Distance(this.transform.position, vector3_2) / this.textureLengthScale, 1f);
        this.line.material.mainTextureOffset -= new Vector2(Time.deltaTime * this.textureScrollSpeed, 0.0f);
      }
      if (this.pulseExpanding)
        this.lerpValue += Time.deltaTime * this.pulseSpeed;
      else
        this.lerpValue -= Time.deltaTime * this.pulseSpeed;
      if ((double) this.lerpValue >= 1.0)
      {
        this.pulseExpanding = false;
        this.lerpValue = 1f;
      }
      else if ((double) this.lerpValue <= 0.0)
      {
        this.pulseExpanding = true;
        this.lerpValue = 0.0f;
      }
      float num = Mathf.Lerp(this.originalWidth, this.customWidth, Mathf.Sin(this.lerpValue * 3.14159274f));
      this.line.startWidth = num;
      this.line.endWidth = num;
    }

    public void SpawnBeam()
    {
      if ((bool) (Object) this.beamLineRendererPrefab)
      {
        this.beam = Object.Instantiate<GameObject>(this.beamLineRendererPrefab);
        this.beam.transform.position = this.transform.position;
        this.beam.transform.parent = this.transform;
        this.beam.transform.rotation = this.transform.rotation;
        this.line = this.beam.GetComponent<LineRenderer>();
        this.line.useWorldSpace = true;
        this.line.positionCount = 2;
        this.beamStart = (bool) (Object) this.beamStartPrefab ? Object.Instantiate<GameObject>(this.beamStartPrefab, this.beam.transform) : (GameObject) null;
        this.beamEnd = (bool) (Object) this.beamEndPrefab ? Object.Instantiate<GameObject>(this.beamEndPrefab, this.beam.transform) : (GameObject) null;
      }
      else
        Debug.LogError((object) ("A prefab with a line renderer must be assigned to the `beamLineRendererPrefab` field in the PolygonArsenalBeamStatic script on " + this.gameObject.name));
    }
  }
}
