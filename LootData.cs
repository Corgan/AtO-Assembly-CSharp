// Decompiled with JetBrains decompiler
// Type: LootData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "New Loot Table", menuName = "Loot Table Data", order = 57)]
public class LootData : ScriptableObject
{
  [SerializeField]
  private string id;
  [SerializeField]
  private int numItems;
  [SerializeField]
  private int goldQuantity;
  [SerializeField]
  private LootItem[] lootItemTable;
  [Header("Upgrade Percents")]
  [SerializeField]
  private float defaultPercentUncommon;
  [SerializeField]
  private float defaultPercentRare;
  [SerializeField]
  private float defaultPercentEpic;
  [SerializeField]
  private float defaultPercentMythic;
  [Header("Shady Deal")]
  [SerializeField]
  private GameObject shadyModel;
  [SerializeField]
  private float shadyScaleX = 1f;
  [SerializeField]
  private float shadyScaleY = 1f;
  [SerializeField]
  private float shadyOffsetX;
  [SerializeField]
  private float shadyOffsetY;

  public string Id
  {
    get => this.id;
    set => this.id = value;
  }

  public int NumItems
  {
    get => this.numItems;
    set => this.numItems = value;
  }

  public LootItem[] LootItemTable
  {
    get => this.lootItemTable;
    set => this.lootItemTable = value;
  }

  public float DefaultPercentUncommon
  {
    get => this.defaultPercentUncommon;
    set => this.defaultPercentUncommon = value;
  }

  public float DefaultPercentRare
  {
    get => this.defaultPercentRare;
    set => this.defaultPercentRare = value;
  }

  public float DefaultPercentEpic
  {
    get => this.defaultPercentEpic;
    set => this.defaultPercentEpic = value;
  }

  public float DefaultPercentMythic
  {
    get => this.defaultPercentMythic;
    set => this.defaultPercentMythic = value;
  }

  public int GoldQuantity
  {
    get => this.goldQuantity;
    set => this.goldQuantity = value;
  }

  public GameObject ShadyModel
  {
    get => this.shadyModel;
    set => this.shadyModel = value;
  }

  public float ShadyScaleX
  {
    get => this.shadyScaleX;
    set => this.shadyScaleX = value;
  }

  public float ShadyScaleY
  {
    get => this.shadyScaleY;
    set => this.shadyScaleY = value;
  }

  public float ShadyOffsetX
  {
    get => this.shadyOffsetX;
    set => this.shadyOffsetX = value;
  }

  public float ShadyOffsetY
  {
    get => this.shadyOffsetY;
    set => this.shadyOffsetY = value;
  }
}
