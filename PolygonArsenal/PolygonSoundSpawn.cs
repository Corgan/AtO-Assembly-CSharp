// Decompiled with JetBrains decompiler
// Type: PolygonArsenal.PolygonSoundSpawn
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace PolygonArsenal
{
  public class PolygonSoundSpawn : MonoBehaviour
  {
    public GameObject prefabSound;
    public bool destroyWhenDone = true;
    public bool soundPrefabIsChild;
    [Range(0.01f, 10f)]
    public float pitchRandomMultiplier = 1f;

    private void Start()
    {
      GameObject gameObject = Object.Instantiate<GameObject>(this.prefabSound, this.transform.position, Quaternion.identity);
      AudioSource component = gameObject.GetComponent<AudioSource>();
      if (this.soundPrefabIsChild)
        gameObject.transform.SetParent(this.transform);
      if ((double) this.pitchRandomMultiplier != 1.0)
      {
        if ((double) Random.value < 0.5)
          component.pitch *= Random.Range(1f / this.pitchRandomMultiplier, 1f);
        else
          component.pitch *= Random.Range(1f, this.pitchRandomMultiplier);
      }
      if (!this.destroyWhenDone)
        return;
      float t = component.clip.length / component.pitch;
      Object.Destroy((Object) gameObject, t);
    }
  }
}
