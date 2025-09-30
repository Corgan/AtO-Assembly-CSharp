// Decompiled with JetBrains decompiler
// Type: ETFXPEL.ParticleEffectsLibrary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace ETFXPEL
{
  public class ParticleEffectsLibrary : MonoBehaviour
  {
    public static ParticleEffectsLibrary GlobalAccess;
    public int TotalEffects;
    public int CurrentParticleEffectIndex;
    public int CurrentParticleEffectNum;
    public Vector3[] ParticleEffectSpawnOffsets;
    public float[] ParticleEffectLifetimes;
    public GameObject[] ParticleEffectPrefabs;
    private string effectNameString = "";
    private List<Transform> currentActivePEList;
    private Vector3 spawnPosition = Vector3.zero;

    private void Awake()
    {
      ParticleEffectsLibrary.GlobalAccess = this;
      this.currentActivePEList = new List<Transform>();
      this.TotalEffects = this.ParticleEffectPrefabs.Length;
      this.CurrentParticleEffectNum = 1;
      if (this.ParticleEffectSpawnOffsets.Length != this.TotalEffects)
        Debug.LogError((object) "ParticleEffectsLibrary-ParticleEffectSpawnOffset: Not all arrays match length, double check counts.");
      if (this.ParticleEffectPrefabs.Length != this.TotalEffects)
        Debug.LogError((object) "ParticleEffectsLibrary-ParticleEffectPrefabs: Not all arrays match length, double check counts.");
      this.effectNameString = this.ParticleEffectPrefabs[this.CurrentParticleEffectIndex].name + " (" + this.CurrentParticleEffectNum.ToString() + " of " + this.TotalEffects.ToString() + ")";
    }

    private void Start()
    {
    }

    public string GetCurrentPENameString()
    {
      return this.ParticleEffectPrefabs[this.CurrentParticleEffectIndex].name + " (" + this.CurrentParticleEffectNum.ToString() + " of " + this.TotalEffects.ToString() + ")";
    }

    public void PreviousParticleEffect()
    {
      if ((double) this.ParticleEffectLifetimes[this.CurrentParticleEffectIndex] == 0.0 && this.currentActivePEList.Count > 0)
      {
        for (int index = 0; index < this.currentActivePEList.Count; ++index)
        {
          if ((Object) this.currentActivePEList[index] != (Object) null)
            Object.Destroy((Object) this.currentActivePEList[index].gameObject);
        }
        this.currentActivePEList.Clear();
      }
      if (this.CurrentParticleEffectIndex > 0)
        --this.CurrentParticleEffectIndex;
      else
        this.CurrentParticleEffectIndex = this.TotalEffects - 1;
      this.CurrentParticleEffectNum = this.CurrentParticleEffectIndex + 1;
      this.effectNameString = this.ParticleEffectPrefabs[this.CurrentParticleEffectIndex].name + " (" + this.CurrentParticleEffectNum.ToString() + " of " + this.TotalEffects.ToString() + ")";
    }

    public void NextParticleEffect()
    {
      if ((double) this.ParticleEffectLifetimes[this.CurrentParticleEffectIndex] == 0.0 && this.currentActivePEList.Count > 0)
      {
        for (int index = 0; index < this.currentActivePEList.Count; ++index)
        {
          if ((Object) this.currentActivePEList[index] != (Object) null)
            Object.Destroy((Object) this.currentActivePEList[index].gameObject);
        }
        this.currentActivePEList.Clear();
      }
      if (this.CurrentParticleEffectIndex < this.TotalEffects - 1)
        ++this.CurrentParticleEffectIndex;
      else
        this.CurrentParticleEffectIndex = 0;
      this.CurrentParticleEffectNum = this.CurrentParticleEffectIndex + 1;
      this.effectNameString = this.ParticleEffectPrefabs[this.CurrentParticleEffectIndex].name + " (" + this.CurrentParticleEffectNum.ToString() + " of " + this.TotalEffects.ToString() + ")";
    }

    public void SpawnParticleEffect(Vector3 positionInWorldToSpawn)
    {
      this.spawnPosition = positionInWorldToSpawn + this.ParticleEffectSpawnOffsets[this.CurrentParticleEffectIndex];
      GameObject gameObject = Object.Instantiate<GameObject>(this.ParticleEffectPrefabs[this.CurrentParticleEffectIndex], this.spawnPosition, this.ParticleEffectPrefabs[this.CurrentParticleEffectIndex].transform.rotation);
      gameObject.name = "PE_" + this.ParticleEffectPrefabs[this.CurrentParticleEffectIndex]?.ToString();
      if ((double) this.ParticleEffectLifetimes[this.CurrentParticleEffectIndex] == 0.0)
        this.currentActivePEList.Add(gameObject.transform);
      this.currentActivePEList.Add(gameObject.transform);
      if ((double) this.ParticleEffectLifetimes[this.CurrentParticleEffectIndex] == 0.0)
        return;
      Object.Destroy((Object) gameObject, this.ParticleEffectLifetimes[this.CurrentParticleEffectIndex]);
    }
  }
}
