using UnityEngine;

namespace PolygonArsenal;

public class PolygonProjectileScript : MonoBehaviour
{
	public GameObject impactParticle;

	public GameObject projectileParticle;

	public GameObject muzzleParticle;

	public GameObject[] trailParticles;

	[Header("Adjust if not using Sphere Collider")]
	public float colliderRadius = 1f;

	[Range(0f, 1f)]
	public float collideOffset = 0.15f;

	private Rigidbody rb;

	private Transform myTransform;

	private SphereCollider sphereCollider;

	private float destroyTimer;

	private bool destroyed;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		myTransform = base.transform;
		sphereCollider = GetComponent<SphereCollider>();
		projectileParticle = Object.Instantiate(projectileParticle, myTransform.position, myTransform.rotation);
		projectileParticle.transform.parent = myTransform;
		if ((bool)muzzleParticle)
		{
			muzzleParticle = Object.Instantiate(muzzleParticle, myTransform.position, myTransform.rotation);
			Object.Destroy(muzzleParticle, 1.5f);
		}
	}

	private void FixedUpdate()
	{
		if (destroyed)
		{
			return;
		}
		float radius = (sphereCollider ? sphereCollider.radius : colliderRadius);
		Vector3 velocity = rb.velocity;
		float maxDistance = velocity.magnitude * Time.deltaTime;
		if (rb.useGravity)
		{
			velocity += Physics.gravity * Time.deltaTime;
			maxDistance = velocity.magnitude * Time.deltaTime;
		}
		if (Physics.SphereCast(myTransform.position, radius, velocity, out var hitInfo, maxDistance))
		{
			myTransform.position = hitInfo.point + hitInfo.normal * collideOffset;
			GameObject obj = Object.Instantiate(impactParticle, myTransform.position, Quaternion.FromToRotation(Vector3.up, hitInfo.normal));
			if (hitInfo.transform.tag == "Destructible")
			{
				Object.Destroy(hitInfo.transform.gameObject);
			}
			GameObject[] array = trailParticles;
			foreach (GameObject gameObject in array)
			{
				GameObject obj2 = myTransform.Find(projectileParticle.name + "/" + gameObject.name).gameObject;
				obj2.transform.parent = null;
				Object.Destroy(obj2, 3f);
			}
			Object.Destroy(projectileParticle, 3f);
			Object.Destroy(obj, 5f);
			DestroyMissile();
		}
		else
		{
			destroyTimer += Time.deltaTime;
			if (destroyTimer >= 5f)
			{
				DestroyMissile();
			}
		}
		RotateTowardsDirection();
	}

	private void DestroyMissile()
	{
		destroyed = true;
		GameObject[] array = trailParticles;
		foreach (GameObject gameObject in array)
		{
			GameObject obj = myTransform.Find(projectileParticle.name + "/" + gameObject.name).gameObject;
			obj.transform.parent = null;
			Object.Destroy(obj, 3f);
		}
		Object.Destroy(projectileParticle, 3f);
		Object.Destroy(base.gameObject);
		ParticleSystem[] componentsInChildren = GetComponentsInChildren<ParticleSystem>();
		for (int j = 1; j < componentsInChildren.Length; j++)
		{
			ParticleSystem particleSystem = componentsInChildren[j];
			if (particleSystem.gameObject.name.Contains("Trail"))
			{
				particleSystem.transform.SetParent(null);
				Object.Destroy(particleSystem.gameObject, 2f);
			}
		}
	}

	private void RotateTowardsDirection()
	{
		if (rb.velocity != Vector3.zero)
		{
			Quaternion b = Quaternion.LookRotation(rb.velocity.normalized, Vector3.up);
			float t = Vector3.Angle(myTransform.forward, rb.velocity.normalized) * Time.deltaTime;
			myTransform.rotation = Quaternion.Slerp(myTransform.rotation, b, t);
		}
	}
}
