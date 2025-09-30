// Decompiled with JetBrains decompiler
// Type: NenukilLayers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class NenukilLayers : MonoBehaviour
{
  public List<Transform> layersToHide = new List<Transform>();
  public List<Transform> layersCannon = new List<Transform>();
  public List<Transform> layersGun = new List<Transform>();
  public List<Transform> layersDoubleGun = new List<Transform>();

  private void Start()
  {
    if ((bool) (Object) MatchManager.Instance)
    {
      Hero[] teamHero = MatchManager.Instance.GetTeamHero();
      for (int index = 0; index < teamHero.Length; ++index)
      {
        if (teamHero[index] != null && (Object) teamHero[index].HeroData != (Object) null && teamHero[index].SubclassName == "engineer")
        {
          this.HideLayers();
          if (teamHero[index].HaveTrait("doublebarrel"))
            this.ShowLayersDoubleGun();
          else if (teamHero[index].HaveTrait("mountedcannon"))
            this.ShowLayersCannon();
          else
            this.ShowLayersGun();
        }
      }
    }
    else
    {
      this.HideLayers();
      this.ShowLayersGun();
    }
  }

  private void HideLayers()
  {
    for (int index = 0; index < this.layersToHide.Count; ++index)
    {
      if (this.layersToHide[index].gameObject.activeSelf)
        this.layersToHide[index].gameObject.SetActive(false);
    }
  }

  private void ShowLayersGun()
  {
    for (int index = 0; index < this.layersGun.Count; ++index)
    {
      if (!this.layersGun[index].gameObject.activeSelf)
        this.layersGun[index].gameObject.SetActive(true);
    }
  }

  private void ShowLayersDoubleGun()
  {
    for (int index = 0; index < this.layersDoubleGun.Count; ++index)
    {
      if (!this.layersDoubleGun[index].gameObject.activeSelf)
        this.layersDoubleGun[index].gameObject.SetActive(true);
    }
  }

  private void ShowLayersCannon()
  {
    for (int index = 0; index < this.layersCannon.Count; ++index)
    {
      if (!this.layersCannon[index].gameObject.activeSelf)
        this.layersCannon[index].gameObject.SetActive(true);
    }
  }
}
