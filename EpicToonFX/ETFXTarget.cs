// Decompiled with JetBrains decompiler
// Type: EpicToonFX.ETFXTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace EpicToonFX
{
  public class ETFXTarget : MonoBehaviour
  {
    [Header("Effect shown on target hit")]
    public GameObject hitParticle;
    [Header("Effect shown on target respawn")]
    public GameObject respawnParticle;
    private Renderer targetRenderer;
    private Collider targetCollider;

    private void Start()
    {
      this.targetRenderer = this.GetComponent<Renderer>();
      this.targetCollider = this.GetComponent<Collider>();
    }

    private void SpawnTarget()
    {
      this.targetRenderer.enabled = true;
      this.targetCollider.enabled = true;
      Object.Destroy((Object) Object.Instantiate<GameObject>(this.respawnParticle, this.transform.position, this.transform.rotation), 3.5f);
    }

    private void OnTriggerEnter(Collider col)
    {
      if (!(col.tag == "Missile") || !(bool) (Object) this.hitParticle)
        return;
      Object.Destroy((Object) Object.Instantiate<GameObject>(this.hitParticle, this.transform.position, this.transform.rotation), 2f);
      this.targetRenderer.enabled = false;
      this.targetCollider.enabled = false;
      this.StartCoroutine(this.Respawn());
    }

    private IEnumerator Respawn()
    {
      yield return (object) new WaitForSeconds(3f);
      this.SpawnTarget();
    }
  }
}
