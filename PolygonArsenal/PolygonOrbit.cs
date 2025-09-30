// Decompiled with JetBrains decompiler
// Type: PolygonArsenal.PolygonOrbit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace PolygonArsenal
{
  [AddComponentMenu("Camera-Control/Mouse drag Orbit with zoom")]
  public class PolygonOrbit : MonoBehaviour
  {
    public Transform target;
    public float distance = 5f;
    public float xSpeed = 120f;
    public float ySpeed = 120f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;
    public float distanceMin = 0.5f;
    public float distanceMax = 15f;
    public float smoothTime = 2f;
    private float rotationYAxis;
    private float rotationXAxis;
    private float velocityX;
    private float velocityY;

    private void Start()
    {
      Vector3 eulerAngles = this.transform.eulerAngles;
      this.rotationYAxis = eulerAngles.y;
      this.rotationXAxis = eulerAngles.x;
      if (!(bool) (Object) this.GetComponent<Rigidbody>())
        return;
      this.GetComponent<Rigidbody>().freezeRotation = true;
    }

    private void LateUpdate()
    {
      if (!(bool) (Object) this.target)
        return;
      if (Input.GetMouseButton(1))
      {
        this.velocityX += (float) ((double) this.xSpeed * (double) Input.GetAxis("Mouse X") * (double) this.distance * 0.019999999552965164);
        this.velocityY += (float) ((double) this.ySpeed * (double) Input.GetAxis("Mouse Y") * 0.019999999552965164);
      }
      this.rotationYAxis += this.velocityX;
      this.rotationXAxis -= this.velocityY;
      this.rotationXAxis = PolygonOrbit.ClampAngle(this.rotationXAxis, this.yMinLimit, this.yMaxLimit);
      Quaternion quaternion = Quaternion.Euler(this.rotationXAxis, this.rotationYAxis, 0.0f);
      this.distance = Mathf.Clamp(this.distance - Input.GetAxis("Mouse ScrollWheel") * 5f, this.distanceMin, this.distanceMax);
      RaycastHit hitInfo;
      if (Physics.Linecast(this.target.position, this.transform.position, out hitInfo))
        this.distance -= hitInfo.distance;
      Vector3 vector3_1 = new Vector3(0.0f, 0.0f, -this.distance);
      Vector3 vector3_2 = quaternion * vector3_1 + this.target.position;
      this.transform.rotation = quaternion;
      this.transform.position = vector3_2;
      this.velocityX = Mathf.Lerp(this.velocityX, 0.0f, Time.deltaTime * this.smoothTime);
      this.velocityY = Mathf.Lerp(this.velocityY, 0.0f, Time.deltaTime * this.smoothTime);
    }

    public static float ClampAngle(float angle, float min, float max)
    {
      if ((double) angle < -360.0)
        angle += 360f;
      if ((double) angle > 360.0)
        angle -= 360f;
      return Mathf.Clamp(angle, min, max);
    }
  }
}
