// Decompiled with JetBrains decompiler
// Type: EpicToonFX.ETFXLoopScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace EpicToonFX
{
  public class ETFXLoopScript : MonoBehaviour
  {
    public GameObject chosenEffect;
    public float loopTimeLimit = 2f;
    [Header("Spawn options")]
    public bool disableLights = true;
    public bool disableSound = true;
    public float spawnScale = 1f;

    private void Start() => this.PlayEffect();

    public void PlayEffect() => this.StartCoroutine("EffectLoop");

    private IEnumerator EffectLoop()
    {
      ETFXLoopScript etfxLoopScript = this;
      GameObject effectPlayer = Object.Instantiate<GameObject>(etfxLoopScript.chosenEffect, etfxLoopScript.transform.position, etfxLoopScript.transform.rotation);
      effectPlayer.transform.localScale = new Vector3(etfxLoopScript.spawnScale, etfxLoopScript.spawnScale, etfxLoopScript.spawnScale);
      if (etfxLoopScript.disableLights && (bool) (Object) effectPlayer.GetComponent<Light>())
        effectPlayer.GetComponent<Light>().enabled = false;
      if (etfxLoopScript.disableSound && (bool) (Object) effectPlayer.GetComponent<AudioSource>())
        effectPlayer.GetComponent<AudioSource>().enabled = false;
      yield return (object) new WaitForSeconds(etfxLoopScript.loopTimeLimit);
      Object.Destroy((Object) effectPlayer);
      etfxLoopScript.PlayEffect();
    }
  }
}
