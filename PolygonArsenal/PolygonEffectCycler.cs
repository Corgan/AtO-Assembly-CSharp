// Decompiled with JetBrains decompiler
// Type: PolygonArsenal.PolygonEffectCycler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace PolygonArsenal
{
  public class PolygonEffectCycler : MonoBehaviour
  {
    [SerializeField]
    private List<GameObject> listOfEffects;
    [Header("Loop length in seconds")]
    [SerializeField]
    private float loopTimeLength = 5f;
    private float timeOfLastInstantiate;
    private GameObject instantiatedEffect;
    private int effectIndex;

    private void Start()
    {
      this.instantiatedEffect = Object.Instantiate<GameObject>(this.listOfEffects[this.effectIndex], this.transform.position, this.transform.rotation);
      ++this.effectIndex;
      this.timeOfLastInstantiate = Time.time;
    }

    private void Update()
    {
      if ((double) Time.time < (double) this.timeOfLastInstantiate + (double) this.loopTimeLength)
        return;
      Object.Destroy((Object) this.instantiatedEffect);
      this.instantiatedEffect = Object.Instantiate<GameObject>(this.listOfEffects[this.effectIndex], this.transform.position, this.transform.rotation);
      this.timeOfLastInstantiate = Time.time;
      if (this.effectIndex < this.listOfEffects.Count - 1)
        ++this.effectIndex;
      else
        this.effectIndex = 0;
    }
  }
}
