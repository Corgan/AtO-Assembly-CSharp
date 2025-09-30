// Decompiled with JetBrains decompiler
// Type: EpicToonFX.ETFXEffectCycler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace EpicToonFX
{
  public class ETFXEffectCycler : MonoBehaviour
  {
    public List<GameObject> listOfEffects;
    private int effectIndex;
    [Header("Spawn Settings")]
    [SerializeField]
    [Space(10f)]
    public float loopLength = 1f;
    public float startDelay = 1f;
    public bool disableLights = true;
    public bool disableSound = true;

    private void Start() => this.Invoke("PlayEffect", this.startDelay);

    public void PlayEffect()
    {
      this.StartCoroutine("EffectLoop");
      if (this.effectIndex < this.listOfEffects.Count - 1)
        ++this.effectIndex;
      else
        this.effectIndex = 0;
    }

    private IEnumerator EffectLoop()
    {
      ETFXEffectCycler etfxEffectCycler = this;
      GameObject instantiatedEffect = Object.Instantiate<GameObject>(etfxEffectCycler.listOfEffects[etfxEffectCycler.effectIndex], etfxEffectCycler.transform.position, etfxEffectCycler.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 0.0f));
      if (etfxEffectCycler.disableLights && (bool) (Object) instantiatedEffect.GetComponent<Light>())
        instantiatedEffect.GetComponent<Light>().enabled = false;
      if (etfxEffectCycler.disableSound && (bool) (Object) instantiatedEffect.GetComponent<AudioSource>())
        instantiatedEffect.GetComponent<AudioSource>().enabled = false;
      yield return (object) new WaitForSeconds(etfxEffectCycler.loopLength);
      Object.Destroy((Object) instantiatedEffect);
      etfxEffectCycler.PlayEffect();
    }
  }
}
