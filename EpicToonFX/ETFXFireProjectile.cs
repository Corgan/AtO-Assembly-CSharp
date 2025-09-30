// Decompiled with JetBrains decompiler
// Type: EpicToonFX.ETFXFireProjectile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
namespace EpicToonFX
{
  public class ETFXFireProjectile : MonoBehaviour
  {
    [SerializeField]
    public GameObject[] projectiles;
    [Header("Missile spawns at attached game object")]
    public Transform spawnPosition;
    [HideInInspector]
    public int currentProjectile;
    public float speed = 500f;
    private ETFXButtonScript selectedProjectileButton;
    private RaycastHit hit;

    private void Start()
    {
      this.selectedProjectileButton = GameObject.Find("Button").GetComponent<ETFXButtonScript>();
    }

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.RightArrow))
        this.nextEffect();
      if (Input.GetKeyDown(KeyCode.D))
        this.nextEffect();
      if (Input.GetKeyDown(KeyCode.A))
        this.previousEffect();
      else if (Input.GetKeyDown(KeyCode.LeftArrow))
        this.previousEffect();
      if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out this.hit, 100f))
      {
        GameObject gameObject = Object.Instantiate<GameObject>(this.projectiles[this.currentProjectile], this.spawnPosition.position, Quaternion.identity);
        gameObject.transform.LookAt(this.hit.point);
        gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * this.speed);
      }
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      Vector3 origin = ray.origin;
      ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      Vector3 dir = ray.direction * 100f;
      Color yellow = Color.yellow;
      Debug.DrawRay(origin, dir, yellow);
    }

    public void nextEffect()
    {
      if (this.currentProjectile < this.projectiles.Length - 1)
        ++this.currentProjectile;
      else
        this.currentProjectile = 0;
      this.selectedProjectileButton.getProjectileNames();
    }

    public void previousEffect()
    {
      if (this.currentProjectile > 0)
        --this.currentProjectile;
      else
        this.currentProjectile = this.projectiles.Length - 1;
      this.selectedProjectileButton.getProjectileNames();
    }

    public void AdjustSpeed(float newSpeed) => this.speed = newSpeed;
  }
}
