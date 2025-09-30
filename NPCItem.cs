// Decompiled with JetBrains decompiler
// Type: NPCItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class NPCItem : CharacterItem
{
  [SerializeField]
  private NPCData npcData;
  public Transform animatedTransform;
  public Transform bossParticles;
  public Transform bossSmallParticles;
  public Transform namedSmallParticles;
  public Transform cardsGOT;
  public Transform[] cardsT;
  public CardItem[] cardsCI;

  public override void Awake() => base.Awake();

  public override void Start() => base.Start();

  public void Init(NPC _npc, bool useAltModels = false)
  {
    if ((Object) this.npcData == (Object) null)
      return;
    this.NPC = _npc;
    this.Hero = (Hero) null;
    GameObject original = useAltModels ? this.npcData.GameObjectAnimatedAlternate : this.npcData.GameObjectAnimated;
    this.IsHero = false;
    this.energyT.parent.gameObject.SetActive(false);
    this.GO_Buffs.transform.localPosition = new Vector3(0.03f, -1.05f, 0.0f);
    if ((Object) original != (Object) null)
    {
      GameObject GO = Object.Instantiate<GameObject>(original, Vector3.zero, Quaternion.identity, this.transform);
      this.animatedTransform = GO.transform;
      GO.transform.localPosition = original.transform.localPosition;
      GO.transform.localRotation = original.transform.localRotation;
      this.GetComponent<CharacterItem>().SetOriginalLocalPosition(GO.transform.localPosition);
      GO.name = this.transform.name;
      this.DisableCollider();
      CharacterGOItem characterGoItem = GO.GetComponent<CharacterGOItem>();
      if ((Object) characterGoItem == (Object) null)
        characterGoItem = GO.AddComponent(typeof (CharacterGOItem)) as CharacterGOItem;
      characterGoItem._characterItem = this.GetComponent<CharacterItem>();
      this.CharImageSR.sprite = (Sprite) null;
      this.Anim = GO.GetComponent<Animator>();
      this.animatedSprites.Clear();
      this.GetSpritesFromAnimated(GO);
      this.transformForCombatText = GO.transform;
      BoxCollider2D boxCollider2D = GO.GetComponent<BoxCollider2D>();
      if ((Object) boxCollider2D == (Object) null)
        boxCollider2D = GO.AddComponent(typeof (BoxCollider2D)) as BoxCollider2D;
      this.heightModel = boxCollider2D.size.y;
    }
    else
    {
      this.CharImageSR.sprite = this.npcData.Sprite;
      if ((double) this.npcData.PosBottom != 0.0)
        this.CharImageSR.transform.localPosition = new Vector3(this.CharImageT.localPosition.x, (float) ((double) this.npcData.PosBottom * (double) Screen.height * (1.0 / 1000.0)), this.CharImageT.localPosition.z);
      this.transformForCombatText = this.transform;
    }
    this.cardsGOT.position = this.npcData.BigModel ? new Vector3(this.cardsGOT.position.x, 4.8f, this.cardsGOT.position.z) : new Vector3(this.cardsGOT.position.x, 4.2f, this.cardsGOT.position.z);
    if (this.npcData.IsBoss)
    {
      if (this.npcData.BigModel)
        this.bossParticles.gameObject.SetActive(true);
      else
        this.bossSmallParticles.gameObject.SetActive(true);
    }
    else if (this.npcData.IsNamed)
      this.namedSmallParticles.gameObject.SetActive(true);
    if (this.npcData.BigModel)
    {
      this.hpBackground.gameObject.SetActive(false);
      this.hpBackgroundHigh.gameObject.SetActive(true);
      this.hpT.localScale = new Vector3(2.5f, 1.2f, 1f);
      this.hpT.localPosition = new Vector3(-2.16f, -0.5f, 0.0f);
      this.hpShieldT.localScale = new Vector3(1.5f, 1.5f, 1f);
      this.hpShieldT.localPosition = new Vector3(-0.79f, -0.15f, 0.0f);
      this.hpBlockT.localPosition = new Vector3(0.31f, 0.07f, 0.0f);
      this.hpBlockIconT.localScale = new Vector3(1.5f, 1.5f, 1f);
      this.hpBlockIconT.localPosition = new Vector3(-1.19f, -0.1f, 0.0f);
      this.HpText.fontSize = 3f;
      this.HpText.transform.localPosition = new Vector3(1.38f, 0.07f, 1f);
      this.GO_Buffs.transform.localScale = new Vector3(1.4f, 1.4f, 1f);
      this.GO_Buffs.transform.localPosition = new Vector3(0.39f, -1.3f, 0.0f);
      this.GO_Buffs.GetComponent<GridLayoutGroup>().cellSize = (Vector2) new Vector3(0.32f, 0.3f, 0.0f);
      this.GO_Buffs.GetComponent<GridLayoutGroup>().constraintCount = 8;
      this.skull.transform.localPosition = new Vector3(0.4f, this.skull.transform.localPosition.y, this.skull.transform.localPosition.z);
      this.skull.transform.localScale = new Vector3(0.45f, 0.45f, 1f);
    }
    this.ActivateMark(false);
    this.SetHP();
    this.DrawEnergy();
    if (!((Object) MatchManager.Instance != (Object) null))
      return;
    this.StartCoroutine(this.EnchantEffectCo());
  }

  public void RemoveAllCards()
  {
    for (int index = 0; index < this.cardsT.Length; ++index)
    {
      if ((Object) this.cardsT[index] != (Object) null)
        Object.Destroy((Object) this.cardsT[index].gameObject);
    }
    this.cardsT = (Transform[]) null;
  }

  public NPCData NpcData
  {
    get => this.npcData;
    set => this.npcData = value;
  }
}
