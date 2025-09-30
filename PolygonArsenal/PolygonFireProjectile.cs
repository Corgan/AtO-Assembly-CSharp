// Decompiled with JetBrains decompiler
// Type: PolygonArsenal.PolygonFireProjectile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace PolygonArsenal
{
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
      if ((Object) this.gunPrefab != (Object) null)
      {
        this.instantiatedGun = Object.Instantiate<GameObject>(this.gunPrefab, Vector3.zero, Quaternion.identity);
        this.instantiatedGun.transform.SetParent(this.transform);
        this.instantiatedGun.transform.localPosition = Vector3.zero;
      }
      if ((Object) this.speedSlider != (Object) null)
      {
        this.speedSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnSpeedSliderChanged));
        this.speed = this.speedSlider.value;
      }
      this.UpdateDisplayName();
    }

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        this.nextEffect();
      else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        this.previousEffect();
      if ((Object) this.fullAutoButton != (Object) null)
        this.isFullAuto = this.fullAutoButton.isOn;
      if ((Object) this.instantiatedGun != (Object) null)
        this.UpdateGunPositionAndRotation();
      if (this.isFullAuto)
      {
        if (this.canShoot && Input.GetKey(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
          this.StartCoroutine(this.Shoot());
      }
      else if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        this.ShootProjectile();
      if ((Object) this.speedSlider != (Object) null)
      {
        this.speedSlider.onValueChanged.AddListener(new UnityAction<float>(this.OnSpeedSliderChanged));
        this.speed = this.speedSlider.value;
      }
      Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction * 100f, Color.yellow);
    }

    private IEnumerator Shoot()
    {
      this.canShoot = false;
      this.ShootProjectile();
      yield return (object) new WaitForSeconds(this.fireRate);
      this.canShoot = true;
    }

    private void ShootProjectile()
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hitInfo;
      Vector3 vector3 = !Physics.Raycast(ray, out hitInfo, 100f) ? ray.direction.normalized : (hitInfo.point - this.spawnPosition.position).normalized;
      Vector3 position = this.spawnPosition.position + vector3 * this.spawnOffset;
      GameObject gameObject = Object.Instantiate<GameObject>(this.projectiles[this.currentProjectile], position, Quaternion.identity);
      gameObject.transform.LookAt(position + vector3 * 10f);
      gameObject.GetComponent<Rigidbody>().AddForce(vector3 * this.speed);
    }

    private void UpdateGunPositionAndRotation()
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hitInfo;
      Vector3 normalized = ((!Physics.Raycast(ray, out hitInfo) ? ray.origin + ray.direction * 100f : hitInfo.point) - this.transform.position).normalized;
      float y = Mathf.Atan2(normalized.x, normalized.z) * 57.29578f;
      Quaternion b = Quaternion.Euler((float) (-(double) Mathf.Asin(normalized.y / normalized.magnitude) * 57.295780181884766), y, 0.0f);
      if (!((Object) this.instantiatedGun != (Object) null))
        return;
      this.instantiatedGun.transform.rotation = Quaternion.Slerp(this.instantiatedGun.transform.rotation, b, Time.deltaTime * 10f);
      this.instantiatedGun.transform.position = this.spawnPosition.position - this.instantiatedGun.transform.forward * this.gunOffset;
    }

    public void nextEffect()
    {
      if (this.currentProjectile < this.projectiles.Length - 1)
        ++this.currentProjectile;
      else
        this.currentProjectile = 0;
      this.UpdateDisplayName();
    }

    public void previousEffect()
    {
      if (this.currentProjectile > 0)
        --this.currentProjectile;
      else
        this.currentProjectile = this.projectiles.Length - 1;
      this.UpdateDisplayName();
    }

    private void UpdateDisplayName()
    {
      Text text = (Object) this.missileNameText != (Object) null ? this.missileNameText : this.GetComponentInChildren<Text>();
      if (!((Object) text != (Object) null))
        return;
      string name = this.projectiles[this.currentProjectile].GetComponent<PolygonProjectileScript>().projectileParticle.name;
      text.text = name;
    }

    private void OnSpeedSliderChanged(float value) => this.speed = value;
  }
}
