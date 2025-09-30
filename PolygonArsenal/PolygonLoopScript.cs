// Decompiled with JetBrains decompiler
// Type: PolygonArsenal.PolygonLoopScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
namespace PolygonArsenal
{
  public class PolygonLoopScript : MonoBehaviour
  {
    public GameObject chosenEffect;
    public float loopTimeLimit = 2f;

    private void Start() => this.PlayEffect();

    public void PlayEffect() => this.StartCoroutine("EffectLoop");

    private IEnumerator EffectLoop()
    {
      // ISSUE: reference to a compiler-generated field
      int num = this.\u003C\u003E1__state;
      PolygonLoopScript polygonLoopScript = this;
      GameObject effectPlayer;
      if (num != 0)
      {
        if (num != 1)
          return false;
        // ISSUE: reference to a compiler-generated field
        this.\u003C\u003E1__state = -1;
        Object.Destroy((Object) effectPlayer);
        polygonLoopScript.PlayEffect();
        return false;
      }
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      effectPlayer = Object.Instantiate<GameObject>(polygonLoopScript.chosenEffect);
      effectPlayer.transform.position = polygonLoopScript.transform.position;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E2__current = (object) new WaitForSeconds(polygonLoopScript.loopTimeLimit);
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = 1;
      return true;
    }
  }
}
