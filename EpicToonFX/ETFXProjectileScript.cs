// Decompiled with JetBrains decompiler
// Type: EpicToonFX.ETFXProjectileScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace EpicToonFX
{
  public class ETFXProjectileScript : MonoBehaviour
  {
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject muzzleParticle;
    [Header("Adjust if not using Sphere Collider")]
    public float colliderRadius = 1f;
    [Range(0.0f, 1f)]
    public float collideOffset = 0.15f;

    private void Start()
    {
      this.projectileParticle = Object.Instantiate<GameObject>(this.projectileParticle, this.transform.position, this.transform.rotation);
      this.projectileParticle.transform.parent = this.transform;
      if (!(bool) (Object) this.muzzleParticle)
        return;
      this.muzzleParticle = Object.Instantiate<GameObject>(this.muzzleParticle, this.transform.position, this.transform.rotation);
      Object.Destroy((Object) this.muzzleParticle, 1.5f);
    }

    private void FixedUpdate()
    {
      if ((double) this.GetComponent<Rigidbody>().velocity.magnitude != 0.0)
        this.transform.rotation = Quaternion.LookRotation(this.GetComponent<Rigidbody>().velocity);
      float radius = !(bool) (Object) this.transform.GetComponent<SphereCollider>() ? this.colliderRadius : this.transform.GetComponent<SphereCollider>().radius;
      Vector3 direction = this.transform.GetComponent<Rigidbody>().velocity;
      if (this.transform.GetComponent<Rigidbody>().useGravity)
        direction += Physics.gravity * Time.deltaTime;
      direction = direction.normalized;
      float maxDistance = this.transform.GetComponent<Rigidbody>().velocity.magnitude * Time.deltaTime;
      RaycastHit hitInfo;
      if (!Physics.SphereCast(this.transform.position, radius, direction, out hitInfo, maxDistance))
        return;
      this.transform.position = hitInfo.point + hitInfo.normal * this.collideOffset;
      GameObject gameObject = Object.Instantiate<GameObject>(this.impactParticle, this.transform.position, Quaternion.FromToRotation(Vector3.up, hitInfo.normal));
      ParticleSystem[] componentsInChildren = this.GetComponentsInChildren<ParticleSystem>();
      for (int index = 1; index < componentsInChildren.Length; ++index)
      {
        ParticleSystem particleSystem = componentsInChildren[index];
        if (particleSystem.gameObject.name.Contains("Trail"))
        {
          particleSystem.transform.SetParent((Transform) null);
          Object.Destroy((Object) particleSystem.gameObject, 2f);
        }
      }
      Object.Destroy((Object) this.projectileParticle, 3f);
      Object.Destroy((Object) gameObject, 3.5f);
      Object.Destroy((Object) this.gameObject);
    }
  }
}
