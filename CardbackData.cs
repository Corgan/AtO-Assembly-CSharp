// Decompiled with JetBrains decompiler
// Type: CardbackData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "New Cardback Data", menuName = "Cardback Data", order = 64)]
public class CardbackData : ScriptableObject
{
  [SerializeField]
  private string cardbackId;
  [SerializeField]
  private string cardbackName;
  [SerializeField]
  private SubClassData cardbackSubclass;
  [SerializeField]
  private bool locked;
  [SerializeField]
  private bool showIfLocked;
  [Header("Base cardback for characters")]
  [SerializeField]
  private bool baseCardback;
  [Header("Order in charpopup")]
  [SerializeField]
  private int cardbackOrder = 1000;
  [Header("Rank Requeriment")]
  [SerializeField]
  private int rankLevel;
  [Header("DLC Requeriment")]
  [SerializeField]
  private string sku;
  [Header("Steam Stat Requeriment")]
  [SerializeField]
  private string steamStat;
  [Header("Adventure Level Requeriment")]
  [SerializeField]
  private int adventureLevel;
  [Header("Obelisk Level Requeriment")]
  [SerializeField]
  private int obeliskLevel;
  [Header("Singularity Level Requeriment")]
  [SerializeField]
  private int singularityLevel;
  [Header("PDX Account Required")]
  [SerializeField]
  private bool pdxAccountRequired;
  [Header("Sprites")]
  [SerializeField]
  private Sprite cardbackSprite;
  [Header("Text Id")]
  [SerializeField]
  private string cardbackTextId;

  public string CardbackId
  {
    get => this.cardbackId;
    set => this.cardbackId = value;
  }

  public string CardbackName
  {
    get => this.cardbackName;
    set => this.cardbackName = value;
  }

  public SubClassData CardbackSubclass
  {
    get => this.cardbackSubclass;
    set => this.cardbackSubclass = value;
  }

  public bool BaseCardback
  {
    get => this.baseCardback;
    set => this.baseCardback = value;
  }

  public int CardbackOrder
  {
    get => this.cardbackOrder;
    set => this.cardbackOrder = value;
  }

  public int RankLevel
  {
    get => this.rankLevel;
    set => this.rankLevel = value;
  }

  public Sprite CardbackSprite
  {
    get => this.cardbackSprite;
    set => this.cardbackSprite = value;
  }

  public bool ShowIfLocked
  {
    get => this.showIfLocked;
    set => this.showIfLocked = value;
  }

  public bool Locked
  {
    get => this.locked;
    set => this.locked = value;
  }

  public int AdventureLevel
  {
    get => this.adventureLevel;
    set => this.adventureLevel = value;
  }

  public int ObeliskLevel
  {
    get => this.obeliskLevel;
    set => this.obeliskLevel = value;
  }

  public string Sku
  {
    get => this.sku;
    set => this.sku = value;
  }

  public string SteamStat
  {
    get => this.steamStat;
    set => this.steamStat = value;
  }

  public bool PdxAccountRequired
  {
    get => this.pdxAccountRequired;
    set => this.pdxAccountRequired = value;
  }

  public int SingularityLevel
  {
    get => this.singularityLevel;
    set => this.singularityLevel = value;
  }

  public string CardbackTextId
  {
    get => this.cardbackTextId;
    set => this.cardbackTextId = value;
  }
}
