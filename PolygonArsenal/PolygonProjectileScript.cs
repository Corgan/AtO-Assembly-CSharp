// Decompiled with JetBrains decompiler
// Type: PolygonArsenal.PolygonProjectileScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace PolygonArsenal
{
  public class PolygonProjectileScript : MonoBehaviour
  {
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject muzzleParticle;
    public GameObject[] trailParticles;
    [Header("Adjust if not using Sphere Collider")]
    public float colliderRadius = 1f;
    [Range(0.0f, 1f)]
    public float collideOffset = 0.15f;
    private Rigidbody rb;
    private Transform myTransform;
    private SphereCollider sphereCollider;
    private float destroyTimer;
    private bool destroyed;

    private void Start()
    {
      this.rb = this.GetComponent<Rigidbody>();
      this.myTransform = this.transform;
      this.sphereCollider = this.GetComponent<SphereCollider>();
      this.projectileParticle = Object.Instantiate<GameObject>(this.projectileParticle, this.myTransform.position, this.myTransform.rotation);
      this.projectileParticle.transform.parent = this.myTransform;
      if (!(bool) (Object) this.muzzleParticle)
        return;
      this.muzzleParticle = Object.Instantiate<GameObject>(this.muzzleParticle, this.myTransform.position, this.myTransform.rotation);
      Object.Destroy((Object) this.muzzleParticle, 1.5f);
    }

    private void FixedUpdate()
    {
      if (this.destroyed)
        return;
      float radius = (bool) (Object) this.sphereCollider ? this.sphereCollider.radius : this.colliderRadius;
      Vector3 velocity = this.rb.velocity;
      float maxDistance = velocity.magnitude * Time.deltaTime;
      if (this.rb.useGravity)
      {
        velocity += Physics.gravity * Time.deltaTime;
        maxDistance = velocity.magnitude * Time.deltaTime;
      }
      RaycastHit hitInfo;
      if (Physics.SphereCast(this.myTransform.position, radius, velocity, out hitInfo, maxDistance))
      {
        this.myTransform.position = hitInfo.point + hitInfo.normal * this.collideOffset;
        GameObject gameObject1 = Object.Instantiate<GameObject>(this.impactParticle, this.myTransform.position, Quaternion.FromToRotation(Vector3.up, hitInfo.normal));
        if (hitInfo.transform.tag == "Destructible")
          Object.Destroy((Object) hitInfo.transform.gameObject);
        foreach (Object trailParticle in this.trailParticles)
        {
          GameObject gameObject2 = this.myTransform.Find(this.projectileParticle.name + "/" + trailParticle.name).gameObject;
          gameObject2.transform.parent = (Transform) null;
          Object.Destroy((Object) gameObject2, 3f);
        }
        Object.Destroy((Object) this.projectileParticle, 3f);
        Object.Destroy((Object) gameObject1, 5f);
        this.DestroyMissile();
      }
      else
      {
        this.destroyTimer += Time.deltaTime;
        if ((double) this.destroyTimer >= 5.0)
          this.DestroyMissile();
      }
      this.RotateTowardsDirection();
    }

    private void DestroyMissile()
    {
      this.destroyed = true;
      foreach (Object trailParticle in this.trailParticles)
      {
        GameObject gameObject = this.myTransform.Find(this.projectileParticle.name + "/" + trailParticle.name).gameObject;
        gameObject.transform.parent = (Transform) null;
        Object.Destroy((Object) gameObject, 3f);
      }
      Object.Destroy((Object) this.projectileParticle, 3f);
      Object.Destroy((Object) this.gameObject);
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
    }

    private void RotateTowardsDirection()
    {
      if (!(this.rb.velocity != Vector3.zero))
        return;
      this.myTransform.rotation = Quaternion.Slerp(this.myTransform.rotation, Quaternion.LookRotation(this.rb.velocity.normalized, Vector3.up), Vector3.Angle(this.myTransform.forward, this.rb.velocity.normalized) * Time.deltaTime);
    }
  }
}
