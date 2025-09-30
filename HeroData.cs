// Decompiled with JetBrains decompiler
// Type: HeroData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using System.Text.RegularExpressions;
using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "New Hero", menuName = "Hero Data", order = 52)]
[Serializable]
public class HeroData : ScriptableObject
{
  [SerializeField]
  private string heroName;
  [SerializeField]
  private string id;
  [SerializeField]
  private Enums.HeroClass heroClass;
  [SerializeField]
  private SubClassData heroSubClass;

  public string HeroName
  {
    get => this.heroName;
    set => this.heroName = value;
  }

  public string Id
  {
    get => this.id;
    set => this.id = value;
  }

  public Enums.HeroClass HeroClass
  {
    get => this.heroClass;
    set => this.heroClass = value;
  }

  public SubClassData HeroSubClass
  {
    get => this.heroSubClass;
    set => this.heroSubClass = value;
  }

  private void Awake() => this.id = Regex.Replace(this.heroName, "\\s+", "").ToLower();
}
