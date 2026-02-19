using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PolygonArsenal;

public class PolygonFireProjectile : MonoBehaviour
{
	public GameObject[] projectiles;

	[Header("GUI Links")]
	public Text missileNameText;

	public Toggle fullAutoButton;

	public Slider speedSlider;

	[Header("Projectile Settings")]
	public Transform spawnPosition;

	[HideInInspector]
	public int currentProjectile;

	public float speed = 1000f;

	public float spawnOffset = 0.3f;

	[Header("Firing Settings")]
	public float fireRate = 0.13f;

	public bool isFullAuto = true;

	[Header("Gun Settings")]
	public GameObject gunPrefab;

	public float gunOffset = 0.5f;

	private bool canShoot = true;

	private GameObject instantiatedGun;

	private void Start()
	{
		if (gunPrefab != null)
		{
			instantiatedGun = Object.Instantiate(gunPrefab, Vector3.zero, Quaternion.identity);
			instantiatedGun.transform.SetParent(base.transform);
			instantiatedGun.transform.localPosition = Vector3.zero;
		}
		if (speedSlider != null)
		{
			speedSlider.onValueChanged.AddListener(OnSpeedSliderChanged);
			speed = speedSlider.value;
		}
		UpdateDisplayName();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
		{
			nextEffect();
		}
		else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			previousEffect();
		}
		if (fullAutoButton != null)
		{
			isFullAuto = fullAutoButton.isOn;
		}
		if (instantiatedGun != null)
		{
			UpdateGunPositionAndRotation();
		}
		if (isFullAuto)
		{
			if (canShoot && Input.GetKey(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
			{
				StartCoroutine(Shoot());
			}
		}
		else if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
		{
			ShootProjectile();
		}
		if (speedSlider != null)
		{
			speedSlider.onValueChanged.AddListener(OnSpeedSliderChanged);
			speed = speedSlider.value;
		}
		Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction * 100f, Color.yellow);
	}

	private IEnumerator Shoot()
	{
		canShoot = false;
		ShootProjectile();
		yield return new WaitForSeconds(fireRate);
		canShoot = true;
	}

	private void ShootProjectile()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		Vector3 vector = ((!Physics.Raycast(ray, out hitInfo, 100f)) ? ray.direction.normalized : (hitInfo.point - spawnPosition.position).normalized);
		Vector3 vector2 = spawnPosition.position + vector * spawnOffset;
		GameObject obj = Object.Instantiate(projectiles[currentProjectile], vector2, Quaternion.identity);
		obj.transform.LookAt(vector2 + vector * 10f);
		obj.GetComponent<Rigidbody>().AddForce(vector * speed);
	}

	private void UpdateGunPositionAndRotation()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		Vector3 vector = ((!Physics.Raycast(ray, out hitInfo)) ? (ray.origin + ray.direction * 100f) : hitInfo.point);
		Vector3 normalized = (vector - base.transform.position).normalized;
		float y = Mathf.Atan2(normalized.x, normalized.z) * 57.29578f;
		Quaternion b = Quaternion.Euler((0f - Mathf.Asin(normalized.y / normalized.magnitude)) * 57.29578f, y, 0f);
		if (instantiatedGun != null)
		{
			instantiatedGun.transform.rotation = Quaternion.Slerp(instantiatedGun.transform.rotation, b, Time.deltaTime * 10f);
			instantiatedGun.transform.position = spawnPosition.position - instantiatedGun.transform.forward * gunOffset;
		}
	}

	public void nextEffect()
	{
		if (currentProjectile < projectiles.Length - 1)
		{
			currentProjectile++;
		}
		else
		{
			currentProjectile = 0;
		}
		UpdateDisplayName();
	}

	public void previousEffect()
	{
		if (currentProjectile > 0)
		{
			currentProjectile--;
		}
		else
		{
			currentProjectile = projectiles.Length - 1;
		}
		UpdateDisplayName();
	}

	private void UpdateDisplayName()
	{
		Text text = ((missileNameText != null) ? missileNameText : GetComponentInChildren<Text>());
		if (text != null)
		{
			string text2 = projectiles[currentProjectile].GetComponent<PolygonProjectileScript>().projectileParticle.name;
			text.text = text2;
		}
	}

	private void OnSpeedSliderChanged(float value)
	{
		speed = value;
	}
}
