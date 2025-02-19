// Decompiled with JetBrains decompiler
// Type: SkinData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "New Skin Data", menuName = "Skin Data", order = 64)]
public class SkinData : ScriptableObject
{
  [SerializeField]
  private string skinId;
  [SerializeField]
  private string skinName;
  [SerializeField]
  private SubClassData skinSubclass;
  [Header("Base skins for characters")]
  [SerializeField]
  private bool baseSkin;
  [Header("Order in charpopup")]
  [SerializeField]
  private int skinOrder;
  [Header("Skin Requeriments")]
  [SerializeField]
  private int perkLevel;
  [Header("DLC Requeriment")]
  [SerializeField]
  private string sku;
  [Header("SteamStat Requeriment")]
  [SerializeField]
  private string steamStat;
  [Header("Prefab")]
  [SerializeField]
  private GameObject skinGo;
  [Header("Sprites")]
  [SerializeField]
  private Sprite spriteSilueta;
  [SerializeField]
  private Sprite spriteSiluetaGrande;
  [SerializeField]
  private Sprite spritePortrait;
  [SerializeField]
  private Sprite spritePortraitGrande;
  [Header("Text Id")]
  [SerializeField]
  private string skinTextId;

  public string SkinId
  {
    get => this.skinId;
    set => this.skinId = value;
  }

  public string SkinName
  {
    get => this.skinName;
    set => this.skinName = value;
  }

  public SubClassData SkinSubclass
  {
    get => this.skinSubclass;
    set => this.skinSubclass = value;
  }

  public bool BaseSkin
  {
    get => this.baseSkin;
    set => this.baseSkin = value;
  }

  public int PerkLevel
  {
    get => this.perkLevel;
    set => this.perkLevel = value;
  }

  public GameObject SkinGo
  {
    get => this.skinGo;
    set => this.skinGo = value;
  }

  public Sprite SpriteSilueta
  {
    get => this.spriteSilueta;
    set => this.spriteSilueta = value;
  }

  public Sprite SpriteSiluetaGrande
  {
    get => this.spriteSiluetaGrande;
    set => this.spriteSiluetaGrande = value;
  }

  public Sprite SpritePortrait
  {
    get => this.spritePortrait;
    set => this.spritePortrait = value;
  }

  public Sprite SpritePortraitGrande
  {
    get => this.spritePortraitGrande;
    set => this.spritePortraitGrande = value;
  }

  public int SkinOrder
  {
    get => this.skinOrder;
    set => this.skinOrder = value;
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

  public string SkinTextId
  {
    get => this.skinTextId;
    set => this.skinTextId = value;
  }
}
