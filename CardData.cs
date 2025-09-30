// Decompiled with JetBrains decompiler
// Type: CardData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using Cards;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using WebSocketSharp;

#nullable disable
[CreateAssetMenu(fileName = "New Card", menuName = "Card Data", order = 51)]
[Serializable]
public class CardData : ScriptableObject
{
  [Header("General Attributes")]
  [SerializeField]
  private string cardName;
  [SerializeField]
  private string id;
  [SerializeField]
  [HideInInspector]
  private string internalId;
  [SerializeField]
  [HideInInspector]
  private bool visible;
  [SerializeField]
  private string upgradesTo1 = "";
  [SerializeField]
  private string upgradesTo2 = "";
  [SerializeField]
  private Enums.CardUpgraded cardUpgraded;
  [SerializeField]
  private string upgradedFrom = "";
  [SerializeField]
  private string baseCard = "";
  [SerializeField]
  private int cardNumber;
  [SerializeField]
  private SpecialCardEnum specialCardEnum;
  [SerializeField]
  private CardData upgradesToRare;
  [Header("Description and Image")]
  [TextArea]
  [SerializeField]
  private string description = "";
  [SerializeField]
  private string fluff = "";
  [SerializeField]
  private float fluffPercent;
  [SerializeField]
  private string descriptionNormalized = "";
  [SerializeField]
  private string relatedCard;
  [SerializeField]
  private string relatedCard2;
  [SerializeField]
  private string relatedCard3;
  [SerializeField]
  private List<KeyNotesData> keyNotes;
  [SerializeField]
  private Sprite sprite;
  [SerializeField]
  private bool flipSprite;
  [SerializeField]
  private bool showInTome = true;
  [Header("DLC Requeriment")]
  [SerializeField]
  private string sku;
  [Header("Description CUSTOM")]
  [SerializeField]
  private string preDescriptionId = "";
  [SerializeField]
  private string descriptionId = "";
  [SerializeField]
  private string postDescriptionId = "";
  [SerializeField]
  private bool useDescriptionFromCard;
  [SerializeField]
  private string[] preDescriptionArgs;
  [SerializeField]
  private string[] descriptionArgs;
  [SerializeField]
  private string[] postDescriptionArgs;
  [Header("PET Data")]
  [SerializeField]
  private GameObject petModel;
  [SerializeField]
  private bool petFront = true;
  [SerializeField]
  private bool petInvert = true;
  [SerializeField]
  private Vector2 petOffset = new Vector2(0.0f, 0.0f);
  [SerializeField]
  private Vector2 petSize = new Vector2(1f, 1f);
  [Header("PET Effect")]
  [SerializeField]
  private bool isPetAttack;
  [SerializeField]
  private bool isPetCast;
  [Header("Kill PET")]
  [SerializeField]
  private bool killPet;
  [Header("Temporal PET")]
  [SerializeField]
  private bool petTemporal;
  [SerializeField]
  private bool petTemporalAttack;
  [SerializeField]
  private bool petTemporalCast;
  [SerializeField]
  private bool petTemporalMoveToCenter;
  [SerializeField]
  private bool petTemporalMoveToBack;
  [SerializeField]
  private float petTemporalFadeOutDelay;
  [Header("Game Parameters")]
  [SerializeField]
  private int maxInDeck;
  [SerializeField]
  private Enums.CardRarity cardRarity;
  [SerializeField]
  private Enums.CardType cardType;
  [SerializeField]
  private Enums.CardType[] cardTypeAux;
  private List<Enums.CardType> cardTypeList;
  [SerializeField]
  private Enums.CardClass cardClass;
  [SerializeField]
  private ItemData item;
  [SerializeField]
  private ItemData itemEnchantment;
  [SerializeField]
  private int energyCost;
  [SerializeField]
  private int exhaustCounter;
  [SerializeField]
  [HideInInspector]
  private int energyReductionPermanent;
  [SerializeField]
  [HideInInspector]
  private int energyReductionTemporal;
  [SerializeField]
  [HideInInspector]
  private bool energyReductionToZeroPermanent;
  [SerializeField]
  [HideInInspector]
  private bool energyReductionToZeroTemporal;
  [SerializeField]
  [HideInInspector]
  private int energyCostOriginal;
  [SerializeField]
  [HideInInspector]
  private int energyCostForShow;
  [SerializeField]
  private bool playable = true;
  [SerializeField]
  private bool autoplayDraw;
  [SerializeField]
  private bool autoplayEndTurn;
  [Header("Required Conditions")]
  [SerializeField]
  private string effectRequired = "";
  [Header("Custom")]
  [SerializeField]
  private bool vanish;
  [SerializeField]
  private bool innate;
  [SerializeField]
  private bool lazy;
  [SerializeField]
  private bool endTurn;
  [SerializeField]
  private bool moveToCenter;
  [SerializeField]
  private bool corrupted;
  [SerializeField]
  private bool starter;
  [SerializeField]
  private bool onlyInWeekly;
  [SerializeField]
  [HideInInspector]
  private bool modifiedByTrait;
  [Header("Self Kill Hidden (seconds)")]
  [SerializeField]
  private float selfKillHiddenSeconds;
  [Header("Target Data")]
  [SerializeField]
  private Enums.CardTargetType targetType;
  [SerializeField]
  private Enums.CardTargetSide targetSide;
  [SerializeField]
  private Enums.CardTargetPosition targetPosition;
  [Header("Special Value")]
  [SerializeField]
  private Enums.CardSpecialValue specialValueGlobal;
  [SerializeField]
  private AuraCurseData specialAuraCurseNameGlobal;
  [SerializeField]
  private float specialValueModifierGlobal;
  [SerializeField]
  private Enums.CardSpecialValue specialValue1;
  [SerializeField]
  private AuraCurseData specialAuraCurseName1;
  [SerializeField]
  private float specialValueModifier1;
  [SerializeField]
  private Enums.CardSpecialValue specialValue2;
  [SerializeField]
  private AuraCurseData specialAuraCurseName2;
  [SerializeField]
  private float specialValueModifier2;
  [Header("Repeat Cast")]
  [SerializeField]
  private int effectRepeat = 1;
  [SerializeField]
  private float effectRepeatDelay;
  [SerializeField]
  private int effectRepeatEnergyBonus;
  [SerializeField]
  private int effectRepeatMaxBonus;
  [SerializeField]
  private Enums.EffectRepeatTarget effectRepeatTarget;
  [SerializeField]
  private int effectRepeatModificator;
  [Header("Damage")]
  [SerializeField]
  private Enums.DamageType damageType;
  [SerializeField]
  private Enums.DamageType damageTypeOriginal;
  [SerializeField]
  private int damage;
  [SerializeField]
  [HideInInspector]
  private int damagePreCalculated;
  [SerializeField]
  private int damageSides;
  [SerializeField]
  [HideInInspector]
  private int damageSidesPreCalculated;
  [SerializeField]
  private int damageSelf;
  [SerializeField]
  private bool damageSpecialValueGlobal;
  [SerializeField]
  private bool damageSpecialValue1;
  [SerializeField]
  private bool damageSpecialValue2;
  [SerializeField]
  [HideInInspector]
  private int damageSelfPreCalculated;
  [SerializeField]
  [HideInInspector]
  private int damageSelfPreCalculated2;
  [SerializeField]
  private bool ignoreBlock;
  [SerializeField]
  private Enums.DamageType damageType2;
  [SerializeField]
  private Enums.DamageType damageType2Original;
  [SerializeField]
  private int damage2;
  [SerializeField]
  [HideInInspector]
  private int damagePreCalculated2;
  [SerializeField]
  private int damageSides2;
  [SerializeField]
  [HideInInspector]
  private int damageSidesPreCalculated2;
  [SerializeField]
  private int damageSelf2;
  [SerializeField]
  private bool damage2SpecialValueGlobal;
  [SerializeField]
  private bool damage2SpecialValue1;
  [SerializeField]
  private bool damage2SpecialValue2;
  [SerializeField]
  private bool ignoreBlock2;
  [SerializeField]
  private int selfHealthLoss;
  [SerializeField]
  private bool selfHealthLossSpecialGlobal;
  [SerializeField]
  private bool selfHealthLossSpecialValue1;
  [SerializeField]
  private bool selfHealthLossSpecialValue2;
  private int damagePreCalculatedCombined;
  [Header("Damage Energy Bonus")]
  [SerializeField]
  private int damageEnergyBonus;
  [Header("Aura/Curse Energy Bonus")]
  [SerializeField]
  private AuraCurseData acEnergyBonus;
  [SerializeField]
  private int acEnergyBonusQuantity;
  [SerializeField]
  private AuraCurseData acEnergyBonus2;
  [SerializeField]
  private int acEnergyBonus2Quantity;
  [Header("Heal")]
  [SerializeField]
  private int heal;
  [SerializeField]
  [HideInInspector]
  private int healPreCalculated;
  [SerializeField]
  private int healSides;
  [SerializeField]
  private int healSelf;
  [SerializeField]
  [HideInInspector]
  private int healSelfPreCalculated;
  [SerializeField]
  private int healEnergyBonus;
  [SerializeField]
  private float healSelfPerDamageDonePercent;
  [SerializeField]
  private bool healSpecialValueGlobal;
  [SerializeField]
  private bool healSpecialValue1;
  [SerializeField]
  private bool healSpecialValue2;
  [SerializeField]
  private bool healSelfSpecialValueGlobal;
  [SerializeField]
  private bool healSelfSpecialValue1;
  [SerializeField]
  private bool healSelfSpecialValue2;
  [Header("Aura Curse Dispels")]
  [SerializeField]
  private int healCurses;
  [SerializeField]
  private int dispelAuras;
  [SerializeField]
  private int transferCurses;
  [SerializeField]
  private int stealAuras;
  [SerializeField]
  private int reduceCurses;
  [SerializeField]
  private int reduceAuras;
  [SerializeField]
  private int increaseCurses;
  [SerializeField]
  private int increaseAuras;
  [SerializeField]
  private AuraCurseData healAuraCurseSelf;
  [SerializeField]
  private AuraCurseData healAuraCurseName;
  [SerializeField]
  private AuraCurseData healAuraCurseName2;
  [SerializeField]
  private AuraCurseData healAuraCurseName3;
  [SerializeField]
  private AuraCurseData healAuraCurseName4;
  [Header("Energy")]
  [SerializeField]
  private int energyRecharge;
  [SerializeField]
  private bool energyRechargeSpecialValueGlobal;
  [Header("Aura / Buffs")]
  [SerializeField]
  private bool chooseOneOfAvailableAuras;
  [SerializeField]
  private AuraCurseData aura;
  [SerializeField]
  private AuraCurseData auraSelf;
  [SerializeField]
  private int auraCharges;
  [SerializeField]
  private bool auraChargesSpecialValueGlobal;
  [SerializeField]
  private bool auraChargesSpecialValue1;
  [SerializeField]
  private bool auraChargesSpecialValue2;
  [SerializeField]
  private AuraCurseData aura2;
  [SerializeField]
  private AuraCurseData auraSelf2;
  [SerializeField]
  private int auraCharges2;
  [SerializeField]
  private bool auraCharges2SpecialValueGlobal;
  [SerializeField]
  private bool auraCharges2SpecialValue1;
  [SerializeField]
  private bool auraCharges2SpecialValue2;
  [SerializeField]
  private AuraCurseData aura3;
  [SerializeField]
  private AuraCurseData auraSelf3;
  [SerializeField]
  private int auraCharges3;
  [SerializeField]
  private bool auraCharges3SpecialValueGlobal;
  [SerializeField]
  private bool auraCharges3SpecialValue1;
  [SerializeField]
  private bool auraCharges3SpecialValue2;
  [SerializeField]
  private CardData.AuraBuffs[] auras;
  [Header("Curse / DeBuffs")]
  [SerializeField]
  private AuraCurseData curse;
  [SerializeField]
  private AuraCurseData curseSelf;
  [SerializeField]
  private int curseCharges;
  [SerializeField]
  private int curseChargesSides;
  [SerializeField]
  private bool curseChargesSpecialValueGlobal;
  [SerializeField]
  private bool curseChargesSpecialValue1;
  [SerializeField]
  private bool curseChargesSpecialValue2;
  [SerializeField]
  private AuraCurseData curse2;
  [SerializeField]
  private AuraCurseData curseSelf2;
  [SerializeField]
  private int curseCharges2;
  [SerializeField]
  private bool curseCharges2SpecialValueGlobal;
  [SerializeField]
  private bool curseCharges2SpecialValue1;
  [SerializeField]
  private bool curseCharges2SpecialValue2;
  [SerializeField]
  private AuraCurseData curse3;
  [SerializeField]
  private AuraCurseData curseSelf3;
  [SerializeField]
  private int curseCharges3;
  [SerializeField]
  private bool curseCharges3SpecialValueGlobal;
  [SerializeField]
  private bool curseCharges3SpecialValue1;
  [SerializeField]
  private bool curseCharges3SpecialValue2;
  [SerializeField]
  private CardData.CurseDebuffs[] curses;
  [Header("Character Interation")]
  [SerializeField]
  private int pushTarget;
  [SerializeField]
  private int pullTarget;
  [Header("Card Management")]
  [SerializeField]
  private int drawCard;
  [SerializeField]
  private bool drawCardSpecialValueGlobal;
  [SerializeField]
  private int discardCard;
  [SerializeField]
  private Enums.CardType discardCardType;
  [SerializeField]
  private Enums.CardType[] discardCardTypeAux;
  [SerializeField]
  private bool discardCardAutomatic;
  [SerializeField]
  private Enums.CardPlace discardCardPlace;
  [SerializeField]
  private int addCard;
  [SerializeField]
  private string addCardId = "";
  [SerializeField]
  private Enums.CardType addCardType;
  [SerializeField]
  private bool addCardOnlyCheckAuxTypes;
  [SerializeField]
  public List<CardData.CardToGainTypeBasedOnHeroClass> AddCardTypeBasedOnHeroClass;
  [SerializeField]
  private Enums.CardType[] addCardTypeAux;
  [SerializeField]
  public List<CardData.CardToGainListBasedOnHeroClass> AddCardListBasedOnHeroClass;
  [SerializeField]
  private CardData[] addCardList;
  [SerializeField]
  private int addCardChoose;
  [SerializeField]
  private Enums.CardFrom addCardFrom;
  [SerializeField]
  private Enums.CardPlace addCardPlace;
  [SerializeField]
  private int addCardReducedCost;
  [SerializeField]
  private bool addCardCostTurn;
  [SerializeField]
  private bool addCardVanish;
  [SerializeField]
  private CardIdProvider addCardForModify;
  [SerializeField]
  private CopyConfig copyConfig;
  [Header("Look cards")]
  [SerializeField]
  private int lookCards;
  [SerializeField]
  private int lookCardsDiscardUpTo;
  [SerializeField]
  private int lookCardsVanishUpTo;
  [Header("Vanish cards")]
  [SerializeField]
  private bool addVanishToDeck;
  [Header("Summon Units")]
  [SerializeField]
  private NPCData summonUnit;
  [SerializeField]
  private int summonUnitNum;
  [Header("Summon Unit State")]
  [Tooltip("Clean all their status (f.e. Rebirth into a new creature)")]
  [SerializeField]
  private bool metamorph;
  [Tooltip("Keep all their status/hp%/etc...")]
  [SerializeField]
  private bool evolve;
  [Header("Summon Unit Auras")]
  [SerializeField]
  private AuraCurseData summonAura;
  [SerializeField]
  private int summonAuraCharges;
  [SerializeField]
  private AuraCurseData summonAura2;
  [SerializeField]
  private int summonAuraCharges2;
  [SerializeField]
  private AuraCurseData summonAura3;
  [SerializeField]
  private int summonAuraCharges3;
  [Header("Sounds")]
  [SerializeField]
  private AudioClip sound;
  [SerializeField]
  private AudioClip soundPreAction;
  [SerializeField]
  private AudioClip soundPreActionFemale;
  [Header("Sounds (new)")]
  [SerializeField]
  private AudioClip soundDragRework;
  [SerializeField]
  private AudioClip soundReleaseRework;
  [SerializeField]
  private AudioClip soundHitRework;
  [SerializeField]
  private float soundHitReworkDelay;
  [Header("Sound Release exceptions (new)")]
  [SerializeField]
  private List<SubClassData> srException0Class;
  [SerializeField]
  private AudioClip srException0Audio;
  [SerializeField]
  private List<SubClassData> srException1Class;
  [SerializeField]
  private AudioClip srException1Audio;
  [SerializeField]
  private List<SubClassData> srException2Class;
  [SerializeField]
  private AudioClip srException2Audio;
  [Header("InGame Effects")]
  [SerializeField]
  private string effectPreAction = "";
  [SerializeField]
  private string effectCaster = "";
  [SerializeField]
  private bool effectCasterRepeat;
  [SerializeField]
  private float effectPostCastDelay;
  [SerializeField]
  private bool effectCastCenter;
  [SerializeField]
  private string effectTrail = "";
  [SerializeField]
  private bool effectTrailRepeat;
  [SerializeField]
  private float effectTrailSpeed = 1f;
  [SerializeField]
  private Enums.EffectTrailAngle effectTrailAngle;
  [SerializeField]
  private string effectTarget = "";
  [SerializeField]
  private float effectPostTargetDelay;
  [Header("InGame Effects")]
  [SerializeField]
  private int goldGainQuantity;
  [SerializeField]
  private int shardsGainQuantity;
  [SerializeField]
  [HideInInspector]
  private string target = "";
  [SerializeField]
  [HideInInspector]
  private int enchantDamagePreCalculated1;
  [SerializeField]
  [HideInInspector]
  private int enchantDamagePreCalculated2;
  private string colorUpgradePlain = "5E3016";
  private string colorUpgradeGold = "875700";
  private string colorUpgradeBlue = "215382";
  private string colorUpgradeRare = "7F15A6";
  private string colorUpgradePlainLight = "AE7F65";
  private string colorUpgradeBlueLight = "99DEFB";
  private string colorUpgradeGoldLight = "E5B04D";
  private string colorUpgradeRareLight = "E08BFF";
  private bool _tempAttackSelf;
  public static readonly Dictionary<string, Func<CardData, string>> CardValues;

  public bool TempAttackSelf
  {
    get => this._tempAttackSelf;
    set => this._tempAttackSelf = value;
  }

  public CardIdProvider AddCardForModify => this.addCardForModify;

  public CopyConfig CopyConfig => this.copyConfig;

  public SpecialCardEnum SpecialCardEnum
  {
    get => this.specialCardEnum;
    set => this.specialCardEnum = value;
  }

  public void Init(string newId) => this.id = newId;

  public void InitClone(string _internalId)
  {
    this.id = _internalId;
    this.internalId = _internalId;
    if (this.energyCostOriginal == 0)
      this.energyCostOriginal = this.energyCost;
    this.damageTypeOriginal = this.damageType;
    this.damageType2Original = this.damageType2;
    this.damagePreCalculated = this.damage;
    this.damagePreCalculated2 = this.damage2;
    this.damageSelfPreCalculated = this.damageSelf;
    this.damageSidesPreCalculated = this.damageSides;
    this.damageSidesPreCalculated2 = this.damageSides2;
    this.damageSelfPreCalculated2 = this.damageSelf2;
    this.effectRequired = this.effectRequired.ToLower();
    if ((UnityEngine.Object) this.itemEnchantment != (UnityEngine.Object) null)
    {
      this.enchantDamagePreCalculated1 = this.itemEnchantment.DamageToTarget1;
      this.enchantDamagePreCalculated2 = this.itemEnchantment.DamageToTarget2;
    }
    else if ((UnityEngine.Object) this.item != (UnityEngine.Object) null)
    {
      this.enchantDamagePreCalculated1 = this.item.DamageToTarget1;
      this.enchantDamagePreCalculated2 = this.item.DamageToTarget2;
    }
    this.healPreCalculated = this.heal;
    this.healSelfPreCalculated = this.healSelf;
    if (this.innate)
    {
      this.KeyNotes.Add(Globals.Instance.GetKeyNotesData("innate"));
      Globals.Instance.IncludeInSearch(Texts.Instance.GetText("innate"), this.id);
    }
    if (this.vanish)
    {
      this.KeyNotes.Add(Globals.Instance.GetKeyNotesData("vanish"));
      Globals.Instance.IncludeInSearch(Texts.Instance.GetText("vanish"), this.id);
    }
    if (this.energyRecharge > 0 || this.energyRechargeSpecialValueGlobal)
    {
      this.KeyNotes.Add(Globals.Instance.GetKeyNotesData("energy"));
      Globals.Instance.IncludeInSearch(Texts.Instance.GetText("energy"), this.id);
    }
    if ((UnityEngine.Object) this.aura != (UnityEngine.Object) null)
      this.KeyNotes.Add(Globals.Instance.GetKeyNotesData(this.aura.Id));
    if ((UnityEngine.Object) this.aura2 != (UnityEngine.Object) null)
      this.KeyNotes.Add(Globals.Instance.GetKeyNotesData(this.aura2.Id));
    if ((UnityEngine.Object) this.aura3 != (UnityEngine.Object) null)
      this.KeyNotes.Add(Globals.Instance.GetKeyNotesData(this.aura3.Id));
    if (this.auras != null && this.auras.Length != 0)
    {
      foreach (CardData.AuraBuffs aura in this.auras)
      {
        if (aura != null && (UnityEngine.Object) aura.aura != (UnityEngine.Object) null)
          this.KeyNotes.Add(Globals.Instance.GetKeyNotesData(aura.aura.Id));
      }
    }
    if ((UnityEngine.Object) this.auraSelf != (UnityEngine.Object) null)
      this.KeyNotes.Add(Globals.Instance.GetKeyNotesData(this.auraSelf.Id));
    if ((UnityEngine.Object) this.auraSelf2 != (UnityEngine.Object) null)
      this.KeyNotes.Add(Globals.Instance.GetKeyNotesData(this.auraSelf2.Id));
    if ((UnityEngine.Object) this.auraSelf3 != (UnityEngine.Object) null)
      this.KeyNotes.Add(Globals.Instance.GetKeyNotesData(this.auraSelf3.Id));
    if (this.auras != null && this.auras.Length != 0)
    {
      foreach (CardData.AuraBuffs aura in this.auras)
      {
        if (aura != null && (UnityEngine.Object) aura.auraSelf != (UnityEngine.Object) null)
          this.KeyNotes.Add(Globals.Instance.GetKeyNotesData(aura.auraSelf.Id));
      }
    }
    if ((UnityEngine.Object) this.curse != (UnityEngine.Object) null)
      this.KeyNotes.Add(Globals.Instance.GetKeyNotesData(this.curse.Id));
    if ((UnityEngine.Object) this.curse2 != (UnityEngine.Object) null)
      this.KeyNotes.Add(Globals.Instance.GetKeyNotesData(this.curse2.Id));
    if ((UnityEngine.Object) this.curse3 != (UnityEngine.Object) null)
      this.KeyNotes.Add(Globals.Instance.GetKeyNotesData(this.curse3.Id));
    if (this.curses != null && this.curses.Length != 0)
    {
      foreach (CardData.CurseDebuffs curse in this.curses)
      {
        if (curse != null && (UnityEngine.Object) curse.curse != (UnityEngine.Object) null)
          this.KeyNotes.Add(Globals.Instance.GetKeyNotesData(curse.curse.Id));
      }
    }
    if ((UnityEngine.Object) this.curseSelf != (UnityEngine.Object) null)
      this.KeyNotes.Add(Globals.Instance.GetKeyNotesData(this.curseSelf.Id));
    if ((UnityEngine.Object) this.curseSelf2 != (UnityEngine.Object) null)
      this.KeyNotes.Add(Globals.Instance.GetKeyNotesData(this.curseSelf2.Id));
    if ((UnityEngine.Object) this.curseSelf3 != (UnityEngine.Object) null)
      this.KeyNotes.Add(Globals.Instance.GetKeyNotesData(this.curseSelf3.Id));
    if (this.curses != null && this.curses.Length != 0)
    {
      foreach (CardData.CurseDebuffs curse in this.curses)
      {
        if (curse != null && (UnityEngine.Object) curse.curseSelf != (UnityEngine.Object) null)
          this.KeyNotes.Add(Globals.Instance.GetKeyNotesData(curse.curseSelf.Id));
      }
    }
    if ((UnityEngine.Object) this.healAuraCurseSelf != (UnityEngine.Object) null)
      this.KeyNotes.Add(Globals.Instance.GetKeyNotesData(this.healAuraCurseSelf.Id));
    if ((UnityEngine.Object) this.healAuraCurseName != (UnityEngine.Object) null)
      this.KeyNotes.Add(Globals.Instance.GetKeyNotesData(this.healAuraCurseName.Id));
    if ((UnityEngine.Object) this.healAuraCurseName2 != (UnityEngine.Object) null)
      this.KeyNotes.Add(Globals.Instance.GetKeyNotesData(this.healAuraCurseName2.Id));
    if ((UnityEngine.Object) this.healAuraCurseName3 != (UnityEngine.Object) null)
      this.KeyNotes.Add(Globals.Instance.GetKeyNotesData(this.healAuraCurseName3.Id));
    if (!((UnityEngine.Object) this.healAuraCurseName4 != (UnityEngine.Object) null))
      return;
    this.KeyNotes.Add(Globals.Instance.GetKeyNotesData(this.healAuraCurseName4.Id));
  }

  public void InitClone2()
  {
    if (this.effectRequired != "")
    {
      this.KeyNotes.Add(Globals.Instance.GetKeyNotesData(this.effectRequired));
      Globals.Instance.IncludeInSearch(Texts.Instance.GetText(this.effectRequired), this.id);
    }
    this.SetDescriptionNew();
    this.SetTarget();
    this.SetRarity();
  }

  public void ModifyDamageType(Enums.DamageType dt = Enums.DamageType.None, Character ch = null)
  {
    if (this.damageType == Enums.DamageType.None && this.damageType2 == Enums.DamageType.None)
      return;
    int damageType = (int) this.damageType;
    if (dt == Enums.DamageType.None)
    {
      this.damageType = this.damageTypeOriginal;
      this.damageType2 = this.damageType2Original;
    }
    else
    {
      this.damageType = dt;
      this.damageType2 = dt;
    }
  }

  public void SetEnchantDamagePrecalculated1(int damage)
  {
    this.enchantDamagePreCalculated1 = damage;
  }

  public void SetEnchantDamagePrecalculated2(int damage)
  {
    this.enchantDamagePreCalculated2 = damage;
  }

  public void SetDamagePrecalculated(int damage) => this.damagePreCalculated = damage;

  public void SetDamagePrecalculated2(int damage) => this.damagePreCalculated2 = damage;

  public void SetDamagePrecalculatedCombined(int damage)
  {
    this.damagePreCalculatedCombined = damage;
  }

  public void SetDamageSelfPrecalculated(int damage) => this.damageSelfPreCalculated = damage;

  public void SetDamageSelfPrecalculated2(int damage) => this.damageSelfPreCalculated2 = damage;

  public void SetDamageSidesPrecalculated(int damage) => this.damageSidesPreCalculated = damage;

  public void SetDamageSidesPrecalculated2(int damage) => this.damageSidesPreCalculated2 = damage;

  public void SetHealPrecalculated(int heal) => this.healPreCalculated = heal;

  public void SetHealSelfPrecalculated(int heal) => this.healSelfPreCalculated = heal;

  private string ColorTextArray(string type, params string[] text)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("<nobr>");
    switch (type)
    {
      case "":
        int num = 0;
        foreach (string str in text)
        {
          if (num > 0)
            stringBuilder.Append(" ");
          stringBuilder.Append(str);
          ++num;
        }
        if (type != "")
          stringBuilder.Append("</color>");
        stringBuilder.Append("</nobr> ");
        return stringBuilder.ToString();
      case "damage":
        stringBuilder.Append("<color=#B00A00>");
        goto case "";
      case "heal":
        stringBuilder.Append("<color=#1E650F>");
        goto case "";
      case "aura":
        stringBuilder.Append("<color=#263ABC>");
        goto case "";
      case "curse":
        stringBuilder.Append("<color=#720070>");
        goto case "";
      case "system":
        stringBuilder.Append("<color=#5E3016>");
        goto case "";
      default:
        stringBuilder.Append("<color=#5E3016");
        stringBuilder.Append(">");
        goto case "";
    }
  }

  private int GetFinalAuraCharges(string acId, int charges, Character character = null)
  {
    return character == null ? charges : charges + character.GetAuraCurseQuantityModification(acId, this.cardClass);
  }

  private string SpriteText(string sprite)
  {
    StringBuilder stringBuilder = new StringBuilder();
    string str = sprite.ToLower().Replace(" ", "");
    switch (str)
    {
      case "block":
      case "card":
        stringBuilder.Append("<space=.2>");
        break;
      case "piercing":
        stringBuilder.Append("<space=.4>");
        break;
      case "bleed":
        stringBuilder.Append("<space=.1>");
        break;
      case "bless":
        stringBuilder.Append("<space=.1>");
        break;
      default:
        stringBuilder.Append("<space=.3>");
        break;
    }
    stringBuilder.Append(" <space=-.2>");
    stringBuilder.Append("<size=+.1><sprite name=");
    stringBuilder.Append(str);
    stringBuilder.Append("></size>");
    switch (str)
    {
      case "bleed":
        stringBuilder.Append("<space=-.4>");
        goto case "reinforce";
      case "reinforce":
      case "fire":
        return stringBuilder.ToString();
      case "card":
        stringBuilder.Append("<space=-.2>");
        goto case "reinforce";
      case "powerful":
      case "fury":
        stringBuilder.Append("<space=-.1>");
        goto case "reinforce";
      default:
        stringBuilder.Append("<space=-.2>");
        goto case "reinforce";
    }
  }

  public string GetRequireText()
  {
    string requireText = "";
    if (this.effectRequired != "")
    {
      requireText = string.Format(Texts.Instance.GetText("requireSkill"), (object) Globals.Instance.GetKeyNotesData(this.effectRequired).KeynoteName);
      if (this.effectRequired.ToLower() == "stanzai" || this.effectRequired.ToLower() == "stanzaii")
        requireText += "+";
    }
    return requireText;
  }

  private void SetRarity()
  {
    if (this.cardRarity == Enums.CardRarity.Common)
      Globals.Instance.IncludeInSearch(Texts.Instance.GetText("common"), this.id);
    else if (this.cardRarity == Enums.CardRarity.Uncommon)
      Globals.Instance.IncludeInSearch(Texts.Instance.GetText("uncommon"), this.id);
    else if (this.cardRarity == Enums.CardRarity.Rare)
      Globals.Instance.IncludeInSearch(Texts.Instance.GetText("rare"), this.id);
    else if (this.cardRarity == Enums.CardRarity.Epic)
    {
      Globals.Instance.IncludeInSearch(Texts.Instance.GetText("epic"), this.id);
    }
    else
    {
      if (this.cardRarity != Enums.CardRarity.Mythic)
        return;
      Globals.Instance.IncludeInSearch(Texts.Instance.GetText("mythic"), this.id);
    }
  }

  public void SetTarget()
  {
    if (this.autoplayDraw)
      this.target = Texts.Instance.GetText("onDraw");
    else if (this.autoplayEndTurn)
      this.target = Texts.Instance.GetText("onEndTurn");
    else if (this.targetType == Enums.CardTargetType.Global && this.targetSide == Enums.CardTargetSide.Anyone)
      this.target = Texts.Instance.GetText("global");
    else if (this.targetSide == Enums.CardTargetSide.Self)
      this.target = Texts.Instance.GetText("self");
    else if (this.targetSide == Enums.CardTargetSide.Anyone && this.targetPosition == Enums.CardTargetPosition.Random)
      this.target = Texts.Instance.GetText("random");
    else if (this.targetType == Enums.CardTargetType.Single && this.targetSide == Enums.CardTargetSide.Anyone && this.targetPosition == Enums.CardTargetPosition.Anywhere)
      this.target = Texts.Instance.GetText("anyone");
    else if (this.cardClass != Enums.CardClass.Monster)
    {
      if (this.targetSide == Enums.CardTargetSide.Friend)
        this.target = this.targetType != Enums.CardTargetType.Global ? (this.targetPosition != Enums.CardTargetPosition.Random ? (this.targetPosition != Enums.CardTargetPosition.Front ? (this.targetPosition != Enums.CardTargetPosition.Back ? Texts.Instance.GetText("hero") : Texts.Instance.GetText("backHero")) : Texts.Instance.GetText("frontHero")) : Texts.Instance.GetText("randomHero")) : Texts.Instance.GetText("allHeroes");
      else if (this.targetSide == Enums.CardTargetSide.FriendNotSelf)
        this.target = Texts.Instance.GetText("otherHero");
      else if (this.targetSide == Enums.CardTargetSide.Enemy)
        this.target = this.targetType != Enums.CardTargetType.Global ? (this.targetPosition != Enums.CardTargetPosition.Random ? (this.targetPosition != Enums.CardTargetPosition.Front ? (this.targetPosition != Enums.CardTargetPosition.Back ? Texts.Instance.GetText("monster") : Texts.Instance.GetText("backMonster")) : Texts.Instance.GetText("frontMonster")) : Texts.Instance.GetText("randomMonster")) : Texts.Instance.GetText("allMonsters");
    }
    else if (this.cardClass == Enums.CardClass.Monster)
    {
      if (this.targetSide == Enums.CardTargetSide.Friend)
        this.target = this.targetType != Enums.CardTargetType.Global ? (this.targetPosition != Enums.CardTargetPosition.Random ? (this.targetPosition != Enums.CardTargetPosition.Front ? (this.targetPosition != Enums.CardTargetPosition.Back ? (this.targetPosition != Enums.CardTargetPosition.Middle ? (this.targetPosition != Enums.CardTargetPosition.Slowest ? (this.targetPosition != Enums.CardTargetPosition.Fastest ? (this.targetPosition != Enums.CardTargetPosition.LeastHP ? (this.targetPosition != Enums.CardTargetPosition.MostHP ? Texts.Instance.GetText("monster") : Texts.Instance.GetText("mostHPMonster")) : Texts.Instance.GetText("leastHPMonster")) : Texts.Instance.GetText("fastestMonster")) : Texts.Instance.GetText("slowestMonster")) : Texts.Instance.GetText("middleMonster")) : Texts.Instance.GetText("backMonster")) : Texts.Instance.GetText("frontMonster")) : Texts.Instance.GetText("randomMonster")) : Texts.Instance.GetText("allMonsters");
      else if (this.targetSide == Enums.CardTargetSide.FriendNotSelf)
        this.target = Texts.Instance.GetText("otherMonster");
      else if (this.targetSide == Enums.CardTargetSide.Enemy)
        this.target = this.targetType != Enums.CardTargetType.Global ? (this.targetPosition != Enums.CardTargetPosition.Random ? (this.targetPosition != Enums.CardTargetPosition.Front ? (this.targetPosition != Enums.CardTargetPosition.Back ? (this.targetPosition != Enums.CardTargetPosition.Middle ? (this.targetPosition != Enums.CardTargetPosition.Slowest ? (this.targetPosition != Enums.CardTargetPosition.Fastest ? (this.targetPosition != Enums.CardTargetPosition.LeastHP ? (this.targetPosition != Enums.CardTargetPosition.MostHP ? Texts.Instance.GetText("hero") : Texts.Instance.GetText("mostHPHero")) : Texts.Instance.GetText("leastHPHero")) : Texts.Instance.GetText("fastestHero")) : Texts.Instance.GetText("slowestHero")) : Texts.Instance.GetText("middleHero")) : Texts.Instance.GetText("backHero")) : Texts.Instance.GetText("frontHero")) : Texts.Instance.GetText("randomHero")) : Texts.Instance.GetText("allHeroes");
    }
    Globals.Instance.IncludeInSearch(this.target, this.id);
  }

  public void SetDescriptionNew(bool forceDescription = false, Character character = null, bool includeInSearch = true)
  {
    if (forceDescription || !Globals.Instance.CardsDescriptionNormalized.ContainsKey(this.id))
    {
      StringBuilder builder = new StringBuilder();
      StringBuilder aux = new StringBuilder();
      string br1 = "<line-height=15%><br></line-height>";
      string grColor = "<color=#444><size=-.15>";
      string endColor = "</size></color>";
      string goldColor = "<color=#5E3016><size=-.15>";
      if (!string.IsNullOrEmpty(this.preDescriptionId))
        this.AddFormattedDescription(builder, this.preDescriptionId, this.preDescriptionArgs);
      if (!string.IsNullOrEmpty(this.descriptionId))
        this.AddFormattedDescription(builder, this.descriptionId, this.descriptionArgs);
      else if ((UnityEngine.Object) this.item == (UnityEngine.Object) null && (UnityEngine.Object) this.itemEnchantment == (UnityEngine.Object) null || this.useDescriptionFromCard)
        this.AppendCardDescription(character, builder, aux, grColor, endColor, br1, goldColor);
      else
        this.AppendItemDescription(character, builder, aux, grColor, endColor, goldColor);
      if (!string.IsNullOrEmpty(this.postDescriptionId))
        this.AddFormattedDescription(builder, this.postDescriptionId, this.postDescriptionArgs);
      builder.Replace("<c>", "<color=#5E3016>");
      builder.Replace("</c>", "</color>");
      builder.Replace("<nb>", "<nobr>");
      builder.Replace("</nb>", "</nobr>");
      builder.Replace("<br1>", "<br><line-height=15%><br></line-height>");
      builder.Replace("<br2>", "<br><line-height=30%><br></line-height>");
      builder.Replace("<br3>", "<br><line-height=50%><br></line-height>");
      builder.Replace("\n", "<br>");
      builder.Replace("\r>", "<br>");
      this.descriptionNormalized = builder.ToString();
      this.descriptionNormalized = Regex.Replace(this.descriptionNormalized, "[,][ ]*(<(.*?)>)*(.)", (MatchEvaluator) (m => m.ToString().ToLower()));
      this.descriptionNormalized = Regex.Replace(this.descriptionNormalized, "<br>\\w", (MatchEvaluator) (m => m.ToString().ToUpper()));
      Globals.Instance.CardsDescriptionNormalized[this.id] = builder.ToString();
      if (includeInSearch)
        Globals.Instance.IncludeInSearch(Regex.Replace(Regex.Replace(this.descriptionNormalized, "<sprite name=(.*?)>", (MatchEvaluator) (m => Texts.Instance.GetText(m.Groups[1].Value))), "(<(.*?)>)*", ""), this.id, false);
    }
    else
      this.descriptionNormalized = Globals.Instance.CardsDescriptionNormalized[this.id];
  }

  private void AddFormattedDescription(
    StringBuilder builder,
    string descriptionId,
    string[] descriptionArgs)
  {
    if (descriptionArgs.Length != 0)
    {
      List<object> objectList = new List<object>();
      foreach (string descriptionArg in descriptionArgs)
      {
        string key = descriptionArg.Trim();
        Func<CardData, string> func;
        if (CardData.CardValues.TryGetValue(key, out func))
        {
          string str = func(this);
          objectList.Add((object) str);
        }
        else
        {
          if (!key.StartsWith("$"))
            Debug.LogWarning((object) ("Argument \"" + key + "\" not found. Did you mean \"$" + key + "\"?"));
          Debug.LogError((object) ("Argument \"" + key + "\" not recognized in the description arguments dictionary."));
        }
      }
      string text1 = Texts.Instance.GetText(descriptionId);
      int num = CardUtils.GetMaxPlaceholderFormattedStringIndex(text1) + 1;
      if (objectList.Count != num)
        return;
      string text2 = string.Format(text1, objectList.ToArray());
      builder.Append(Functions.FormatStringCard(text2));
    }
    else
      builder.Append(Functions.FormatStringCard(Texts.Instance.GetText(descriptionId)));
  }

  private bool TryGetItem(out ItemData theItem)
  {
    if ((UnityEngine.Object) this.item != (UnityEngine.Object) null)
    {
      theItem = this.item;
      return true;
    }
    if ((UnityEngine.Object) this.itemEnchantment != (UnityEngine.Object) null)
    {
      theItem = this.itemEnchantment;
      return true;
    }
    theItem = (ItemData) null;
    return false;
  }

  private void AppendItemDescription(
    Character character,
    StringBuilder builder,
    StringBuilder aux,
    string grColor,
    string endColor,
    string goldColor)
  {
    ItemData theItem;
    if (!this.TryGetItem(out theItem))
      return;
    if (theItem.MaxHealth != 0)
    {
      builder.Append(string.Format(Texts.Instance.GetText("itemMaxHp"), (object) this.NumFormatItem(theItem.MaxHealth, true)));
      builder.Append("\n");
    }
    if (theItem.ResistModified1 == Enums.DamageType.All)
    {
      builder.Append(string.Format(Texts.Instance.GetText("itemAllResistances"), (object) this.NumFormatItem(theItem.ResistModifiedValue1, true, true)));
      builder.Append("\n");
    }
    int num1 = 0;
    int num2 = 0;
    if (theItem.ResistModified1 != Enums.DamageType.None && theItem.ResistModified1 != Enums.DamageType.All)
    {
      aux.Append(this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) theItem.ResistModified1)));
      num2 = theItem.ResistModifiedValue1;
      ++num1;
    }
    if (theItem.ResistModified2 != Enums.DamageType.None && theItem.ResistModified2 != Enums.DamageType.All)
    {
      if (num2 != theItem.ResistModifiedValue2)
      {
        aux.Append(string.Format(Texts.Instance.GetText("itemXResistances"), (object) string.Empty, (object) this.NumFormatItem(num2, true, true)));
        aux.Append("\n");
        num1 = 0;
        num2 = 0;
      }
      aux.Append(this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) theItem.ResistModified2)));
      if (num2 == 0)
        num2 = theItem.ResistModifiedValue2;
      ++num1;
    }
    if (theItem.ResistModified3 != Enums.DamageType.None && theItem.ResistModified3 != Enums.DamageType.All)
    {
      if (num2 != theItem.ResistModifiedValue3)
      {
        aux.Append(string.Format(Texts.Instance.GetText("itemXResistances"), (object) string.Empty, (object) this.NumFormatItem(num2, true, true)));
        aux.Append("\n");
        num1 = 0;
        num2 = 0;
      }
      aux.Append(this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) theItem.ResistModified3)));
      if (num2 == 0)
        num2 = theItem.ResistModifiedValue3;
      ++num1;
    }
    if (num1 > 0)
    {
      if (num1 > 1)
        aux.Append("\n");
      builder.Append(string.Format(Texts.Instance.GetText("itemXResistances"), (object) aux.ToString(), (object) this.NumFormatItem(num2, true, true)));
      builder.Append("\n");
      aux.Clear();
    }
    if (theItem.CharacterStatModified == Enums.CharacterStat.Speed)
    {
      builder.Append(string.Format(Texts.Instance.GetText("itemSpeed"), (object) this.NumFormatItem(theItem.CharacterStatModifiedValue, true)));
      builder.Append("\n");
    }
    if (theItem.CharacterStatModified == Enums.CharacterStat.EnergyTurn)
    {
      builder.Append(string.Format(Texts.Instance.GetText("itemEnergyRegeneration"), (object) this.SpriteText("energy"), (object) this.NumFormatItem(theItem.CharacterStatModifiedValue, true)));
      builder.Append("\n");
    }
    if (theItem.CharacterStatModified2 == Enums.CharacterStat.Speed)
    {
      builder.Append(string.Format(Texts.Instance.GetText("itemSpeed"), (object) this.NumFormatItem(theItem.CharacterStatModifiedValue2, true)));
      builder.Append("\n");
    }
    if (theItem.CharacterStatModified2 == Enums.CharacterStat.EnergyTurn)
    {
      builder.Append(string.Format(Texts.Instance.GetText("itemEnergyRegeneration"), (object) this.SpriteText("energy"), (object) this.NumFormatItem(theItem.CharacterStatModifiedValue2, true)));
      builder.Append("\n");
    }
    if (theItem.DamageFlatBonus == Enums.DamageType.All)
    {
      builder.Append(string.Format(Texts.Instance.GetText("itemAllDamages"), (object) this.NumFormatItem(theItem.DamageFlatBonusValue, true)));
      builder.Append("\n");
    }
    int num3 = 0;
    int num4 = 0;
    if (theItem.DamageFlatBonus != Enums.DamageType.None && theItem.DamageFlatBonus != Enums.DamageType.All)
    {
      aux.Append(this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) theItem.DamageFlatBonus)));
      num4 = theItem.DamageFlatBonusValue;
      ++num3;
    }
    if (theItem.DamageFlatBonus2 != Enums.DamageType.None && theItem.DamageFlatBonus2 != Enums.DamageType.All)
    {
      aux.Append(this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) theItem.DamageFlatBonus2)));
      ++num3;
    }
    if (theItem.DamageFlatBonus3 != Enums.DamageType.None && theItem.DamageFlatBonus3 != Enums.DamageType.All)
    {
      aux.Append(this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) theItem.DamageFlatBonus3)));
      ++num3;
    }
    if (theItem.DamagePercentBonus == Enums.DamageType.All)
    {
      builder.Append(string.Format(Texts.Instance.GetText("itemAllDamages"), (object) this.NumFormatItem(Functions.FuncRoundToInt(theItem.DamagePercentBonusValue), true, true)));
      builder.Append("\n");
    }
    if (num3 > 0)
    {
      builder.Append(string.Format(Texts.Instance.GetText("itemXDamages"), (object) aux.ToString(), (object) this.NumFormatItem(num4, true)));
      builder.Append("\n");
      aux.Clear();
    }
    int index;
    if (theItem.UseTheNextInsteadWhenYouPlay && (double) theItem.HealPercentBonus != 0.0)
    {
      string str = "";
      if (theItem.DestroyAfterUses > 1)
      {
        index = theItem.DestroyAfterUses;
        str = "(" + index.ToString() + ") ";
      }
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("<size=-.15><color=#444>[");
      stringBuilder.Append(Texts.Instance.GetText("itemTheNext"));
      stringBuilder.Append("]</color></size><br>");
      aux.Append("<color=#5E3016>");
      aux.Append(Texts.Instance.GetText(Enum.GetName(typeof (Enums.CardType), (object) theItem.CastedCardType)));
      aux.Append("</color>");
      builder.Append(string.Format(stringBuilder.ToString(), (object) str, (object) aux.ToString()));
      aux.Clear();
      stringBuilder.Clear();
    }
    if (theItem.HealFlatBonus != 0)
    {
      builder.Append(this.SpriteText("heal"));
      builder.Append(" ");
      builder.Append(Functions.LowercaseFirst(Texts.Instance.GetText("healDone")));
      builder.Append(this.NumFormatItem(theItem.HealFlatBonus, true));
      builder.Append("\n");
    }
    if ((double) theItem.HealPercentBonus != 0.0)
    {
      builder.Append(this.SpriteText("heal"));
      builder.Append(" ");
      builder.Append(Functions.LowercaseFirst(Texts.Instance.GetText("healDone")));
      builder.Append(this.NumFormatItem(Functions.FuncRoundToInt(theItem.HealPercentBonus), true, true));
      builder.Append("\n");
    }
    if (theItem.HealReceivedFlatBonus != 0)
    {
      builder.Append(this.SpriteText("heal"));
      builder.Append(" ");
      builder.Append(Functions.LowercaseFirst(Texts.Instance.GetText("healTaken")));
      builder.Append(this.NumFormatItem(Functions.FuncRoundToInt((float) theItem.HealReceivedFlatBonus), true));
      builder.Append("\n");
    }
    if ((double) theItem.HealReceivedPercentBonus != 0.0)
    {
      builder.Append(this.SpriteText("heal"));
      builder.Append(" ");
      builder.Append(Functions.LowercaseFirst(Texts.Instance.GetText("healTaken")));
      builder.Append(this.NumFormatItem(Functions.FuncRoundToInt(theItem.HealReceivedPercentBonus), true, true));
      builder.Append("\n");
    }
    if ((UnityEngine.Object) theItem.AuracurseBonus1 != (UnityEngine.Object) null && theItem.AuracurseBonusValue1 > 0 && (UnityEngine.Object) theItem.AuracurseBonus2 != (UnityEngine.Object) null && theItem.AuracurseBonusValue2 > 0 && theItem.AuracurseBonusValue1 == theItem.AuracurseBonusValue2)
    {
      aux.Append(this.SpriteText(theItem.AuracurseBonus1.ACName));
      aux.Append(this.SpriteText(theItem.AuracurseBonus2.ACName));
      builder.Append(string.Format(Texts.Instance.GetText("itemCharges"), (object) aux.ToString(), (object) this.NumFormatItem(theItem.AuracurseBonusValue1, true)));
      builder.Append("\n");
      aux.Clear();
    }
    else
    {
      if ((UnityEngine.Object) theItem.AuracurseBonus1 != (UnityEngine.Object) null && theItem.AuracurseBonusValue1 > 0)
      {
        builder.Append(string.Format(Texts.Instance.GetText("itemCharges"), (object) this.SpriteText(theItem.AuracurseBonus1.ACName), (object) this.NumFormatItem(theItem.AuracurseBonusValue1, true)));
        builder.Append("\n");
      }
      if ((UnityEngine.Object) theItem.AuracurseBonus2 != (UnityEngine.Object) null && theItem.AuracurseBonusValue2 > 0)
      {
        builder.Append(string.Format(Texts.Instance.GetText("itemCharges"), (object) this.SpriteText(theItem.AuracurseBonus2.ACName), (object) this.NumFormatItem(theItem.AuracurseBonusValue2, true)));
        builder.Append("\n");
      }
    }
    int num5 = 0;
    if ((UnityEngine.Object) theItem.AuracurseImmune1 != (UnityEngine.Object) null)
    {
      ++num5;
      aux.Append(this.ColorTextArray("curse", this.SpriteText(theItem.AuracurseImmune1.Id)));
    }
    if ((UnityEngine.Object) theItem.AuracurseImmune2 != (UnityEngine.Object) null)
    {
      ++num5;
      aux.Append(this.ColorTextArray("curse", this.SpriteText(theItem.AuracurseImmune2.Id)));
    }
    if (num5 > 0)
    {
      builder.Append(string.Format(Texts.Instance.GetText("cardsImmuneTo"), (object) aux.ToString()));
      builder.Append("\n");
      aux.Clear();
    }
    if (theItem.PercentDiscountShop != 0)
    {
      builder.Append(string.Format(Texts.Instance.GetText("itemDiscount"), (object) this.NumFormatItem(theItem.PercentDiscountShop, true, true)));
      builder.Append("\n");
    }
    if (theItem.PercentRetentionEndGame != 0)
    {
      builder.Append(string.Format(Texts.Instance.GetText("itemDieRetain"), (object) this.NumFormatItem(theItem.PercentRetentionEndGame, true, true)));
      builder.Append("\n");
    }
    if (theItem.AuracurseCustomString != "" && (UnityEngine.Object) theItem.AuracurseCustomAC != (UnityEngine.Object) null)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if ((theItem.AuracurseCustomString == "itemCustomTextMaxChargesIncrasedOnEnemies" || theItem.AuracurseCustomString == "itemCustomTextMaxChargesIncrasedOnHeroes") && theItem.AuracurseCustomModValue1 > 0)
        stringBuilder.Append("+");
      stringBuilder.Append(theItem.AuracurseCustomModValue1);
      builder.Append(string.Format(Texts.Instance.GetText(theItem.AuracurseCustomString), (object) this.ColorTextArray("aura", this.SpriteText(theItem.AuracurseCustomAC.Id)), (object) stringBuilder.ToString(), (object) theItem.AuracurseCustomModValue2));
      builder.Append("\n");
    }
    if (theItem.Id == "harleyrare" || theItem.Id == "templelurkerpetrare" || theItem.Id == "mentalscavengerspetrare")
    {
      builder.Append(Texts.Instance.GetText("immortal"));
      builder.Append("\n");
    }
    if (theItem.ModifiedDamageType != Enums.DamageType.None)
    {
      builder.Append("<nobr>");
      builder.Append(string.Format(Texts.Instance.GetText("cardsTransformDamage"), (object) this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) theItem.ModifiedDamageType))));
      builder.Append("</nobr>");
      builder.Append("\n");
    }
    if (theItem.IsEnchantment && (theItem.CastedCardType != Enums.CardType.None || (theItem.Activation == Enums.EventActivation.PreFinishCast || theItem.Activation == Enums.EventActivation.FinishCast || theItem.Activation == Enums.EventActivation.FinishFinishCast) && !theItem.EmptyHand))
    {
      if (theItem.CastedCardType != Enums.CardType.None)
      {
        aux.Append("<color=#5E3016>");
        aux.Append(Texts.Instance.GetText(Enum.GetName(typeof (Enums.CardType), (object) theItem.CastedCardType)));
        aux.Append("</color>");
      }
      else
        aux.Append(" <sprite name=cards>");
      if (theItem.UseTheNextInsteadWhenYouPlay)
      {
        if ((double) theItem.HealPercentBonus == 0.0)
        {
          string str = "";
          if (theItem.DestroyAfterUses > 1)
          {
            index = theItem.DestroyAfterUses;
            str = "(" + index.ToString() + ") ";
          }
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append("<size=-.15><color=#444>[");
          stringBuilder.Append(Texts.Instance.GetText("itemTheNext"));
          stringBuilder.Append("]</color></size><br>");
          builder.Append(string.Format(stringBuilder.ToString(), (object) str, (object) aux.ToString()));
        }
      }
      else
      {
        builder.Append("<size=-.15>");
        builder.Append("<color=#444>[");
        builder.Append(string.Format(Texts.Instance.GetText("itemWhenYouPlay"), (object) aux.ToString()));
        builder.Append("]</color>");
        builder.Append("</size><br>");
      }
      aux.Clear();
    }
    if (theItem.Activation != Enums.EventActivation.None && theItem.Activation != Enums.EventActivation.PreBeginCombat)
    {
      if (builder.Length > 0)
        builder.Append("<line-height=15%><br></line-height>");
      StringBuilder stringBuilder1 = new StringBuilder();
      if (theItem.TimesPerTurn == 1)
        stringBuilder1.Append(Texts.Instance.GetText("itemOncePerTurn"));
      else if (theItem.TimesPerTurn == 2)
        stringBuilder1.Append(Texts.Instance.GetText("itemTwicePerTurn"));
      else if (theItem.TimesPerTurn == 3)
        stringBuilder1.Append(Texts.Instance.GetText("itemThricePerTurn"));
      else if (theItem.TimesPerTurn == 4)
        stringBuilder1.Append(Texts.Instance.GetText("itemFourPerTurn"));
      else if (theItem.TimesPerTurn == 5)
        stringBuilder1.Append(Texts.Instance.GetText("itemFivePerTurn"));
      else if (theItem.TimesPerTurn == 6)
        stringBuilder1.Append(Texts.Instance.GetText("itemSixPerTurn"));
      else if (theItem.TimesPerTurn == 7)
        stringBuilder1.Append(Texts.Instance.GetText("itemSevenPerTurn"));
      else if (theItem.TimesPerTurn == 8)
        stringBuilder1.Append(Texts.Instance.GetText("itemEightPerTurn"));
      StringBuilder stringBuilder2 = new StringBuilder();
      if (theItem.Activation == Enums.EventActivation.BeginCombat)
        stringBuilder2.Append(Texts.Instance.GetText("itemCombatStart"));
      else if (theItem.Activation == Enums.EventActivation.BeginCombatEnd)
        stringBuilder2.Append(Texts.Instance.GetText("itemCombatEnd"));
      else if (theItem.Activation == Enums.EventActivation.BeginTurnAboutToDealCards || theItem.Activation == Enums.EventActivation.BeginTurnCardsDealt)
      {
        if (theItem.RoundCycle > 1)
        {
          StringBuilder stringBuilder3 = stringBuilder2;
          string text = Texts.Instance.GetText("itemEveryNRounds");
          index = theItem.RoundCycle;
          string str1 = index.ToString();
          string str2 = string.Format(text, (object) str1);
          stringBuilder3.Append(str2);
        }
        else if (theItem.ExactRound == 1)
          stringBuilder2.Append(Texts.Instance.GetText("itemFirstTurn"));
        else
          stringBuilder2.Append(Texts.Instance.GetText("itemEveryRound"));
      }
      else if (theItem.Activation == Enums.EventActivation.Damage)
        stringBuilder2.Append(Texts.Instance.GetText("itemDamageDone"));
      else if (theItem.Activation == Enums.EventActivation.Damaged)
      {
        if ((double) theItem.LowerOrEqualPercentHP < 100.0)
          stringBuilder2.Append(string.Format(Texts.Instance.GetText("itemWhenDamagedBelow"), (object) (theItem.LowerOrEqualPercentHP.ToString() + (Functions.SpaceBeforePercentSign() ? " " : "") + "%")));
        else
          stringBuilder2.Append(Texts.Instance.GetText("itemWhenDamaged"));
      }
      else if (theItem.Activation == Enums.EventActivation.Hitted)
        stringBuilder2.Append(Texts.Instance.GetText("itemWhenHitted"));
      else if (theItem.Activation == Enums.EventActivation.Block)
        stringBuilder2.Append(Texts.Instance.GetText("itemWhenBlock"));
      else if (theItem.Activation == Enums.EventActivation.Heal)
        stringBuilder2.Append(Texts.Instance.GetText("itemHealDoneAction"));
      else if (theItem.Activation == Enums.EventActivation.Healed)
        stringBuilder2.Append(Texts.Instance.GetText("itemWhenHealed"));
      else if (theItem.Activation == Enums.EventActivation.Evaded)
        stringBuilder2.Append(Texts.Instance.GetText("itemWhenEvaded"));
      else if (theItem.Activation == Enums.EventActivation.Evade)
        stringBuilder2.Append(Texts.Instance.GetText("itemWhenEvade"));
      else if (theItem.Activation == Enums.EventActivation.BeginRound)
      {
        if (theItem.RoundCycle > 1)
        {
          StringBuilder stringBuilder4 = stringBuilder2;
          string text = Texts.Instance.GetText("itemEveryRoundRoundN");
          index = theItem.RoundCycle;
          string str3 = index.ToString();
          string str4 = string.Format(text, (object) str3);
          stringBuilder4.Append(str4);
        }
        else
          stringBuilder2.Append(Texts.Instance.GetText("itemEveryRoundRound"));
      }
      else if (theItem.Activation == Enums.EventActivation.BeginTurn || theItem.Activation == Enums.EventActivation.AfterDealCards)
      {
        if (theItem.RoundCycle > 1)
        {
          StringBuilder stringBuilder5 = stringBuilder2;
          string text = Texts.Instance.GetText("itemEveryNRounds");
          index = theItem.RoundCycle;
          string str5 = index.ToString();
          string str6 = string.Format(text, (object) str5);
          stringBuilder5.Append(str6);
        }
        else
          stringBuilder2.Append(Texts.Instance.GetText("itemEveryRound"));
      }
      else if (theItem.Activation == Enums.EventActivation.Killed)
        stringBuilder2.Append(Texts.Instance.GetText("itemWhenKilled"));
      else if (theItem.Activation == Enums.EventActivation.BlockReachedZero)
        stringBuilder2.Append(Texts.Instance.GetText("itemWhenBlockReachesZero"));
      else if ((UnityEngine.Object) theItem.AuraCurseSetted != (UnityEngine.Object) null && theItem.AuraCurseNumForOneEvent == 0)
      {
        string str = this.SpriteText(theItem.AuraCurseSetted.Id);
        if ((bool) (UnityEngine.Object) theItem.AuraCurseSetted2)
        {
          str += this.SpriteText(theItem.AuraCurseSetted2.Id);
          if ((bool) (UnityEngine.Object) theItem.AuraCurseSetted3)
            str += this.SpriteText(theItem.AuraCurseSetted3.Id);
        }
        stringBuilder2.Append(string.Format(Texts.Instance.GetText("itemWhenYouApply"), (object) this.ColorTextArray("curse", str)));
      }
      if (stringBuilder2.Length > 0)
      {
        builder.Append("<size=-.15>");
        builder.Append("<color=#444>[");
        builder.Append(stringBuilder2.ToString());
        builder.Append("]</color>");
        builder.Append("</size><br>");
      }
      StringBuilder stringBuilder6 = (StringBuilder) null;
      if (stringBuilder1.Length > 0)
      {
        builder.Append("<size=-.15>");
        builder.Append("<color=#444>[");
        builder.Append(stringBuilder1.ToString());
        builder.Append("]</color>");
        builder.Append("</size><br>");
      }
      if (theItem.UsedEnergy)
      {
        builder.Append(string.Format(Texts.Instance.GetText("itemApplyForEnergyUsed"), (object) this.ColorTextArray("system", this.SpriteText("energy"))));
        builder.Append("\n");
      }
      if (theItem.EmptyHand)
      {
        builder.Append(Texts.Instance.GetText("itemWhenHandEmpty"));
        builder.Append(":<br>");
      }
      if (theItem.ChanceToDispel > 0)
      {
        if (theItem.ChanceToDispelNum > 0)
        {
          if (theItem.ChanceToDispel < 100)
            builder.Append(string.Format(Texts.Instance.GetText("itemChanceToDispel"), (object) this.ColorTextArray("aura", this.NumFormatItem(theItem.ChanceToDispel, percent: true)), (object) this.ColorTextArray("curse", this.NumFormatItem(theItem.ChanceToDispelNum))));
          else
            builder.Append(string.Format(Texts.Instance.GetText("cardsDispel"), (object) this.ColorTextArray("curse", this.NumFormatItem(theItem.ChanceToDispelNum))));
        }
        else if (theItem.ChanceToDispelNum == -1)
          builder.Append(string.Format(Texts.Instance.GetText("cardsDispelAll")));
        builder.Append("\n");
      }
      if (theItem.ChanceToPurge > 0)
      {
        if (theItem.ChanceToPurgeNum > 0)
        {
          if (theItem.ChanceToPurge < 100)
            builder.Append(string.Format(Texts.Instance.GetText("itemChanceToPurge"), (object) this.ColorTextArray("aura", this.NumFormatItem(theItem.ChanceToPurge, percent: true)), (object) this.ColorTextArray("curse", this.NumFormatItem(theItem.ChanceToPurgeNum))));
          else
            builder.Append(string.Format(Texts.Instance.GetText("cardsPurge"), (object) this.ColorTextArray("curse", this.NumFormatItem(theItem.ChanceToPurgeNum))));
        }
        else if (theItem.ChanceToDispelNum == -1)
          builder.Append(string.Format(Texts.Instance.GetText("cardsPurgeAll")));
        builder.Append("\n");
      }
      if (theItem.ChanceToDispelSelf > 0 && theItem.ChanceToDispelNumSelf > 0)
      {
        if (theItem.ChanceToDispelSelf < 100)
          builder.Append(string.Format(Texts.Instance.GetText("itemChanceToDispelSelf"), (object) this.ColorTextArray("aura", this.NumFormatItem(theItem.ChanceToDispelSelf, percent: true)), (object) this.ColorTextArray("curse", this.NumFormatItem(theItem.ChanceToDispelNumSelf))));
        else
          builder.Append(string.Format(Texts.Instance.GetText("cardsDispelSelf"), (object) this.ColorTextArray("curse", this.NumFormatItem(theItem.ChanceToDispelNumSelf))));
        builder.Append("\n");
      }
      if (!theItem.IsEnchantment && theItem.CastedCardType != Enums.CardType.None)
      {
        aux.Append("<color=#5E3016>");
        aux.Append(Texts.Instance.GetText(Enum.GetName(typeof (Enums.CardType), (object) theItem.CastedCardType)));
        aux.Append("</color>");
        builder.Append(string.Format(Texts.Instance.GetText("itemWhenYouPlay"), (object) aux.ToString()));
        aux.Clear();
        builder.Append(":\n");
      }
      else if (!theItem.IsEnchantment && theItem.CastedCardType == Enums.CardType.None && (theItem.Activation == Enums.EventActivation.PreFinishCast || theItem.Activation == Enums.EventActivation.FinishCast || theItem.Activation == Enums.EventActivation.FinishFinishCast) && !theItem.EmptyHand)
      {
        builder.Append(string.Format(Texts.Instance.GetText("itemWhenYouPlay"), (object) "  <sprite name=cards>"));
        builder.Append(":\n");
      }
      if (theItem.DrawCards > 0)
      {
        builder.Append(string.Format(Texts.Instance.GetText("cardsDraw"), (object) this.ColorTextArray("", this.NumFormat(theItem.DrawCards), this.SpriteText("card"))));
        builder.Append("<br>");
      }
      if (theItem.HealQuantitySpecialValue.Use)
      {
        aux.Append("<color=#111111>X</color>");
        if (theItem.ItemTarget == Enums.ItemTarget.AllHero)
          builder.Append(string.Format(Texts.Instance.GetText("itemRecoverHeroes"), (object) aux.ToString()));
        else if (theItem.ItemTarget == Enums.ItemTarget.Self)
        {
          if (theItem.Activation == Enums.EventActivation.Killed)
            builder.Append(string.Format(Texts.Instance.GetText("itemResurrectHP"), (object) aux.ToString()));
          else
            builder.Append(string.Format(Texts.Instance.GetText("itemRecoverSelf"), (object) aux.ToString()));
        }
        else if (theItem.ItemTarget == Enums.ItemTarget.AllEnemy)
          builder.Append(string.Format(Texts.Instance.GetText("itemRecoverMonsters"), (object) aux.ToString()));
        builder.Append("<br>");
        aux.Clear();
      }
      else if (theItem.HealQuantity > 0)
      {
        aux.Append("<color=#111111>");
        aux.Append(this.NumFormatItem(theItem.HealQuantity, true));
        aux.Append("</color>");
        if (theItem.ItemTarget == Enums.ItemTarget.AllHero)
          builder.Append(string.Format(Texts.Instance.GetText("itemRecoverHeroes"), (object) aux.ToString()));
        else if (theItem.ItemTarget == Enums.ItemTarget.Self)
        {
          if (theItem.Activation == Enums.EventActivation.Killed)
            builder.Append(string.Format(Texts.Instance.GetText("itemResurrectHP"), (object) aux.ToString()));
          else
            builder.Append(string.Format(Texts.Instance.GetText("itemRecoverSelf"), (object) aux.ToString()));
        }
        else if (theItem.ItemTarget == Enums.ItemTarget.AllEnemy)
          builder.Append(string.Format(Texts.Instance.GetText("itemRecoverMonsters"), (object) aux.ToString()));
        builder.Append("<br>");
        aux.Clear();
      }
      else if (theItem.HealQuantity < 0)
      {
        aux.Append("<color=#B00A00>");
        aux.Append(this.NumFormatItem(theItem.HealQuantity, true));
        aux.Append("</color>");
        builder.Append(string.Format(Texts.Instance.GetText("itemLoseHPSelf"), (object) aux.ToString()));
        builder.Append("<br>");
        aux.Clear();
      }
      if (theItem.EnergyQuantity > 0 && theItem.ItemTarget == Enums.ItemTarget.Self)
        builder.Append(string.Format(Texts.Instance.GetText("cardsGain"), (object) this.ColorTextArray("system", this.NumFormat(theItem.EnergyQuantity), this.SpriteText("energy"))));
      if (theItem.HealPercentQuantity > 0)
      {
        aux.Append("<color=#111111>");
        aux.Append(this.NumFormatItem(theItem.HealPercentQuantity, true, true));
        aux.Append("</color>");
        if (theItem.ItemTarget == Enums.ItemTarget.AllHero)
          builder.Append(string.Format(Texts.Instance.GetText("itemRecoverHeroes"), (object) aux.ToString()));
        else if (theItem.ItemTarget == Enums.ItemTarget.Self)
        {
          if (theItem.Activation == Enums.EventActivation.Killed)
            builder.Append(string.Format(Texts.Instance.GetText("itemResurrectHP"), (object) aux.ToString()));
          else
            builder.Append(string.Format(Texts.Instance.GetText("itemRecoverSelf"), (object) aux.ToString()));
        }
        else if (theItem.ItemTarget == Enums.ItemTarget.LowestFlatHpEnemy)
          builder.Append(string.Format(Texts.Instance.GetText("itemRecoverLowestHPMonster"), (object) aux.ToString()));
        else if (theItem.ItemTarget == Enums.ItemTarget.LowestFlatHpHero)
          builder.Append(string.Format(Texts.Instance.GetText("itemRecoverLowestHPHero"), (object) aux.ToString()));
        else if (theItem.ItemTarget == Enums.ItemTarget.AllEnemy)
          builder.Append(string.Format(Texts.Instance.GetText("itemRecoverMonsters"), (object) aux.ToString()));
        else if (theItem.ItemTarget == Enums.ItemTarget.RandomHero)
          builder.Append(string.Format(Texts.Instance.GetText("itemRecoverRandomHPHero"), (object) aux.ToString()));
        else if (theItem.ItemTarget == Enums.ItemTarget.RandomEnemy)
          builder.Append(string.Format(Texts.Instance.GetText("itemRecoverRandomHPMonster"), (object) aux.ToString()));
        builder.Append("<br>");
        aux.Clear();
      }
      if (theItem.HealPercentQuantitySelf < 0)
      {
        builder.Append(string.Format(Texts.Instance.GetText("cardsYouLose"), (object) this.ColorTextArray("damage", this.NumFormat(Mathf.Abs(theItem.HealPercentQuantitySelf)), Functions.SpaceBeforePercentSign() ? "" : "<space=-.1>", "% HP")));
        builder.Append("<br>");
      }
      if (theItem.HealBasedOnAuraCurse > 0)
      {
        StringBuilder stringBuilder7 = new StringBuilder();
        stringBuilder7.Append("Heal ");
        stringBuilder7.Append(this.ColorTextArray("heal", "X", this.SpriteText("heal")));
        stringBuilder7.Append("\n");
        stringBuilder7.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYour"), (object) this.SpriteText(theItem.AuraCurseSetted.ACName), (object) " charges"));
        builder.Append(stringBuilder7);
        builder.Append("<br>");
      }
      if ((double) theItem.healSelfPerDamageDonePercent > 0.0)
      {
        string _id = "cardsHealSelfPerDamage";
        if (theItem.ItemTarget == Enums.ItemTarget.AllHero || theItem.healSelfTeamPerDamageDonePercent)
          _id = "cardHealAllHeroesPerDamage";
        builder.Append(string.Format(Texts.Instance.GetText(_id), (object) theItem.healSelfPerDamageDonePercent.ToString()));
        builder.Append("\n");
      }
      string str7 = "";
      if (theItem.DamageToTarget1 > 0)
        str7 += this.ColorTextArray("damage", this.NumFormat(this.enchantDamagePreCalculated1), this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) theItem.DamageToTargetType1)));
      else if (theItem.DttSpecialValues1.Use)
        str7 += this.ColorTextArray("damage", "X", this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) theItem.DamageToTargetType1)));
      if (theItem.DamageToTarget2 > 0)
        str7 += this.ColorTextArray("damage", this.NumFormat(this.enchantDamagePreCalculated2), this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) theItem.DamageToTargetType2)));
      else if (theItem.DttSpecialValues2.Use)
        str7 += this.ColorTextArray("damage", "X", this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) theItem.DamageToTargetType2)));
      if (!str7.IsNullOrEmpty())
      {
        string _id = "cardsDealDamage";
        if (theItem.ItemTarget == Enums.ItemTarget.AllEnemy)
          _id = "dealDamageToAllMonsters";
        else if (theItem.ItemTarget == Enums.ItemTarget.AllHero)
          _id = "dealDamageToAllHeroes";
        builder.Append(string.Format(Texts.Instance.GetText(_id), (object) str7));
        builder.Append("\n");
      }
      int num6 = 0;
      bool flag1 = true;
      if ((UnityEngine.Object) theItem.AuracurseGain1 != (UnityEngine.Object) null && (theItem.AuracurseGainValue1 > 0 || theItem.AuracurseGain1SpecialValue.Use))
      {
        ++num6;
        if (!theItem.AuracurseGain1SpecialValue.Use)
        {
          if (theItem.NotShowCharacterBonus)
            aux.Append(this.ColorTextArray("aura", this.NumFormat(theItem.AuracurseGainValue1), this.SpriteText(theItem.AuracurseGain1.Id)));
          else
            aux.Append(this.ColorTextArray("aura", this.NumFormat(this.GetFinalAuraCharges(theItem.AuracurseGain1.Id, theItem.AuracurseGainValue1, character)), this.SpriteText(theItem.AuracurseGain1.Id)));
        }
        else
          aux.Append(this.ColorTextArray("aura", "X", this.SpriteText(theItem.AuracurseGain1.Id)));
        if (!theItem.AuracurseGain1.IsAura)
          flag1 = false;
      }
      if ((UnityEngine.Object) theItem.AuracurseGain2 != (UnityEngine.Object) null && theItem.AuracurseGainValue2 > 0)
      {
        ++num6;
        if (theItem.NotShowCharacterBonus)
          aux.Append(this.ColorTextArray("aura", this.NumFormat(theItem.AuracurseGainValue2), this.SpriteText(theItem.AuracurseGain2.Id)));
        else
          aux.Append(this.ColorTextArray("aura", this.NumFormat(this.GetFinalAuraCharges(theItem.AuracurseGain2.Id, theItem.AuracurseGainValue2, character)), this.SpriteText(theItem.AuracurseGain2.Id)));
      }
      if ((UnityEngine.Object) theItem.AuracurseGain3 != (UnityEngine.Object) null && theItem.AuracurseGainValue3 > 0)
      {
        ++num6;
        if (theItem.NotShowCharacterBonus)
          aux.Append(this.ColorTextArray("aura", this.NumFormat(theItem.AuracurseGainValue3), this.SpriteText(theItem.AuracurseGain3.Id)));
        else
          aux.Append(this.ColorTextArray("aura", this.NumFormat(this.GetFinalAuraCharges(theItem.AuracurseGain3.Id, theItem.AuracurseGainValue3, character)), this.SpriteText(theItem.AuracurseGain3.Id)));
      }
      if (num6 > 0)
      {
        if (theItem.ItemTarget == Enums.ItemTarget.Self)
        {
          if (theItem.HealQuantity > 0 || theItem.EnergyQuantity > 0 || theItem.HealPercentQuantity > 0)
          {
            StringBuilder stringBuilder8 = new StringBuilder();
            if (flag1)
              stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsAnd"), (object) builder.ToString(), (object) string.Format(Functions.LowercaseFirst(Texts.Instance.GetText("cardsGain")), (object) aux.ToString())));
            else
              stringBuilder8.Append(string.Format(Texts.Instance.GetText("cardsAnd"), (object) builder.ToString(), (object) string.Format(Functions.LowercaseFirst(Texts.Instance.GetText("cardsSuffer")), (object) aux.ToString())));
            builder.Clear();
            builder.Append(stringBuilder8.ToString());
          }
          else
          {
            if ((UnityEngine.Object) theItem.AuraCurseSetted != (UnityEngine.Object) null && theItem.AuraCurseNumForOneEvent > 0)
            {
              string str8 = this.SpriteText(theItem.AuraCurseSetted.Id);
              if ((bool) (UnityEngine.Object) theItem.AuraCurseSetted2)
              {
                str8 += this.SpriteText(theItem.AuraCurseSetted2.Id);
                if ((bool) (UnityEngine.Object) theItem.AuraCurseSetted3)
                  str8 += this.SpriteText(theItem.AuraCurseSetted3.Id);
              }
              stringBuilder6 = stringBuilder6 ?? new StringBuilder();
              StringBuilder stringBuilder9 = stringBuilder6;
              string text = Texts.Instance.GetText("itemForEveryCharge");
              string[] strArray = new string[2];
              string str9;
              if (theItem.AuraCurseNumForOneEvent <= 1)
              {
                str9 = "";
              }
              else
              {
                index = theItem.AuraCurseNumForOneEvent;
                str9 = index.ToString();
              }
              strArray[0] = str9;
              strArray[1] = str8;
              string str10 = this.ColorTextArray("curse", strArray);
              string str11 = string.Format(text, (object) str10);
              stringBuilder9.Append(str11);
            }
            if (stringBuilder6 != null && stringBuilder6.Length > 0)
            {
              builder.Append("<size=-.15>");
              builder.Append("<color=#444>[");
              builder.Append(stringBuilder6.ToString());
              builder.Append("]</color>");
              builder.Append("</size><br>");
            }
            if (flag1)
            {
              string str12 = builder.ToString();
              if (str12.Length > 8 && str12.Substring(str12.Length - 9) == "<c>, </c>")
                builder.Append(string.Format(Functions.LowercaseFirst(Texts.Instance.GetText("cardsGain")), (object) aux.ToString()));
              else
                builder.Append(string.Format(Texts.Instance.GetText("cardsGain"), (object) aux.ToString()));
            }
            else if (builder.Length > 8 && builder.ToString().Substring(builder.ToString().Length - 9) == "<c>, </c>")
              builder.Append(string.Format(Functions.LowercaseFirst(Texts.Instance.GetText("cardsSuffer")), (object) aux.ToString()));
            else
              builder.Append(string.Format(Texts.Instance.GetText("cardsSuffer"), (object) aux.ToString()));
          }
        }
        else if (theItem.ItemTarget == Enums.ItemTarget.AllEnemy)
          builder.Append(string.Format(Texts.Instance.GetText("itemApplyEnemies"), (object) aux.ToString()));
        else if (theItem.ItemTarget == Enums.ItemTarget.AllHero)
        {
          if (this.cardClass == Enums.CardClass.Monster)
            builder.Append(string.Format(Texts.Instance.GetText("itemApplyHeroesFromMonster"), (object) aux.ToString()));
          else
            builder.Append(string.Format(Texts.Instance.GetText("itemApplyHeroes"), (object) aux.ToString()));
        }
        else if (theItem.ItemTarget == Enums.ItemTarget.RandomHero)
        {
          if ((UnityEngine.Object) theItem.AuraCurseSetted != (UnityEngine.Object) null && theItem.AuraCurseNumForOneEvent > 0)
          {
            string str13 = this.SpriteText(theItem.AuraCurseSetted.Id);
            if ((bool) (UnityEngine.Object) theItem.AuraCurseSetted2)
            {
              str13 += this.SpriteText(theItem.AuraCurseSetted2.Id);
              if ((bool) (UnityEngine.Object) theItem.AuraCurseSetted3)
                str13 += this.SpriteText(theItem.AuraCurseSetted3.Id);
            }
            builder.Append(string.Format(Texts.Instance.GetText("itemForEveryCharge"), (object) this.ColorTextArray("curse", str13)));
          }
          builder.Append(string.Format(Texts.Instance.GetText("itemApplyRandomHero"), (object) aux.ToString()));
        }
        else if (theItem.ItemTarget == Enums.ItemTarget.RandomEnemy)
        {
          if ((UnityEngine.Object) theItem.AuraCurseSetted != (UnityEngine.Object) null && theItem.AuraCurseNumForOneEvent > 0)
          {
            string str14 = this.SpriteText(theItem.AuraCurseSetted.Id);
            if ((bool) (UnityEngine.Object) theItem.AuraCurseSetted2)
            {
              str14 += this.SpriteText(theItem.AuraCurseSetted2.Id);
              if ((bool) (UnityEngine.Object) theItem.AuraCurseSetted3)
                str14 += this.SpriteText(theItem.AuraCurseSetted3.Id);
            }
            builder.Append(string.Format(Texts.Instance.GetText("itemForEveryCharge"), (object) this.ColorTextArray("curse", str14)));
          }
          builder.Append(string.Format(Texts.Instance.GetText("itemApplyRandomEnemy"), (object) aux.ToString()));
        }
        else if (theItem.ItemTarget == Enums.ItemTarget.HighestFlatHpEnemy)
        {
          if ((UnityEngine.Object) theItem.AuraCurseSetted != (UnityEngine.Object) null && theItem.AuraCurseNumForOneEvent > 0)
          {
            string str15 = this.SpriteText(theItem.AuraCurseSetted.Id);
            if ((bool) (UnityEngine.Object) theItem.AuraCurseSetted2)
            {
              str15 += this.SpriteText(theItem.AuraCurseSetted2.Id);
              if ((bool) (UnityEngine.Object) theItem.AuraCurseSetted3)
                str15 += this.SpriteText(theItem.AuraCurseSetted3.Id);
            }
            builder.Append(string.Format(Texts.Instance.GetText("itemForEveryCharge"), (object) this.ColorTextArray("curse", str15)));
          }
          builder.Append(string.Format(Texts.Instance.GetText("itemApplyHighestFlatHpEnemy"), (object) aux.ToString()));
        }
        else if (theItem.ItemTarget == Enums.ItemTarget.LowestFlatHpEnemy)
        {
          if ((UnityEngine.Object) theItem.AuraCurseSetted != (UnityEngine.Object) null && theItem.AuraCurseNumForOneEvent > 0)
          {
            string str16 = this.SpriteText(theItem.AuraCurseSetted.Id);
            if ((bool) (UnityEngine.Object) theItem.AuraCurseSetted2)
            {
              str16 += this.SpriteText(theItem.AuraCurseSetted2.Id);
              if ((bool) (UnityEngine.Object) theItem.AuraCurseSetted3)
                str16 += this.SpriteText(theItem.AuraCurseSetted3.Id);
            }
            StringBuilder stringBuilder10 = builder;
            string text = Texts.Instance.GetText("itemForEveryCharge");
            string[] strArray = new string[2];
            string str17;
            if (theItem.AuraCurseNumForOneEvent <= 1)
            {
              str17 = "";
            }
            else
            {
              index = theItem.AuraCurseNumForOneEvent;
              str17 = index.ToString();
            }
            strArray[0] = str17;
            strArray[1] = str16;
            string str18 = this.ColorTextArray("curse", strArray);
            string str19 = string.Format(text, (object) str18);
            stringBuilder10.Append(str19);
          }
          builder.Append(string.Format(Texts.Instance.GetText("itemApplyLowestFlatHpEnemy"), (object) aux.ToString()));
        }
        else if (theItem.ItemTarget == Enums.ItemTarget.HighestFlatHpHero)
        {
          if ((UnityEngine.Object) theItem.AuraCurseSetted != (UnityEngine.Object) null && theItem.AuraCurseNumForOneEvent > 0)
          {
            string str20 = this.SpriteText(theItem.AuraCurseSetted.Id);
            if ((bool) (UnityEngine.Object) theItem.AuraCurseSetted2)
            {
              str20 += this.SpriteText(theItem.AuraCurseSetted2.Id);
              if ((bool) (UnityEngine.Object) theItem.AuraCurseSetted3)
                str20 += this.SpriteText(theItem.AuraCurseSetted3.Id);
            }
            StringBuilder stringBuilder11 = builder;
            string text = Texts.Instance.GetText("itemForEveryCharge");
            string[] strArray = new string[2];
            string str21;
            if (theItem.AuraCurseNumForOneEvent <= 1)
            {
              str21 = "";
            }
            else
            {
              index = theItem.AuraCurseNumForOneEvent;
              str21 = index.ToString();
            }
            strArray[0] = str21;
            strArray[1] = str20;
            string str22 = this.ColorTextArray("curse", strArray);
            string str23 = string.Format(text, (object) str22);
            stringBuilder11.Append(str23);
          }
          builder.Append(string.Format(Texts.Instance.GetText("itemApplyHighestFlatHpHero"), (object) aux.ToString()));
        }
        else if (theItem.ItemTarget == Enums.ItemTarget.LowestFlatHpHero)
        {
          if ((UnityEngine.Object) theItem.AuraCurseSetted != (UnityEngine.Object) null && theItem.AuraCurseNumForOneEvent > 0)
          {
            string str24 = this.SpriteText(theItem.AuraCurseSetted.Id);
            if ((bool) (UnityEngine.Object) theItem.AuraCurseSetted2)
            {
              str24 += this.SpriteText(theItem.AuraCurseSetted2.Id);
              if ((bool) (UnityEngine.Object) theItem.AuraCurseSetted3)
                str24 += this.SpriteText(theItem.AuraCurseSetted3.Id);
            }
            StringBuilder stringBuilder12 = builder;
            string text = Texts.Instance.GetText("itemForEveryCharge");
            string[] strArray = new string[2];
            string str25;
            if (theItem.AuraCurseNumForOneEvent <= 1)
            {
              str25 = "";
            }
            else
            {
              index = theItem.AuraCurseNumForOneEvent;
              str25 = index.ToString();
            }
            strArray[0] = str25;
            strArray[1] = str24;
            string str26 = this.ColorTextArray("curse", strArray);
            string str27 = string.Format(text, (object) str26);
            stringBuilder12.Append(str27);
          }
          builder.Append(string.Format(Texts.Instance.GetText("itemApplyLowestFlatHpHero"), (object) aux.ToString()));
        }
        else if (this.targetSide == Enums.CardTargetSide.Enemy || theItem.ItemTarget == Enums.ItemTarget.CurrentTarget)
        {
          if ((UnityEngine.Object) theItem.AuraCurseSetted != (UnityEngine.Object) null && theItem.AuraCurseNumForOneEvent > 0)
          {
            string str28 = this.SpriteText(theItem.AuraCurseSetted.Id);
            if ((bool) (UnityEngine.Object) theItem.AuraCurseSetted2)
            {
              str28 += this.SpriteText(theItem.AuraCurseSetted2.Id);
              if ((bool) (UnityEngine.Object) theItem.AuraCurseSetted3)
                str28 += this.SpriteText(theItem.AuraCurseSetted3.Id);
            }
            StringBuilder stringBuilder13 = builder;
            string text = Texts.Instance.GetText("itemApplyForEvery");
            string[] strArray = new string[2];
            string str29;
            if (theItem.AuraCurseNumForOneEvent <= 1)
            {
              str29 = "";
            }
            else
            {
              index = theItem.AuraCurseNumForOneEvent;
              str29 = index.ToString();
            }
            strArray[0] = str29;
            strArray[1] = str28;
            string str30 = this.ColorTextArray("curse", strArray);
            string str31 = aux.ToString();
            string str32 = string.Format(text, (object) str30, (object) str31);
            stringBuilder13.Append(str32);
          }
          else if (theItem.ItemTarget == Enums.ItemTarget.Random)
            builder.Append(string.Format(Texts.Instance.GetText("itemApplyRandom"), (object) aux.ToString()));
          else
            builder.Append(string.Format(Texts.Instance.GetText("cardsApply"), (object) aux.ToString()));
        }
        else
          builder.Append(string.Format(Texts.Instance.GetText("cardsGrant"), (object) aux.ToString()));
        builder.Append("\n");
        aux.Clear();
      }
      GetXEqualsDecriptionValue();
      int num7 = 0;
      bool flag2 = true;
      if ((UnityEngine.Object) theItem.AuracurseGainSelf1 != (UnityEngine.Object) null && theItem.AuracurseGainSelfValue1 > 0)
      {
        ++num7;
        aux.Append(this.ColorTextArray("aura", this.NumFormat(this.GetFinalAuraCharges(theItem.AuracurseGainSelf1.Id, theItem.AuracurseGainSelfValue1, character)), this.SpriteText(theItem.AuracurseGainSelf1.Id)));
        if (!theItem.AuracurseGainSelf1.IsAura)
          flag2 = false;
      }
      if ((UnityEngine.Object) theItem.AuracurseGainSelf2 != (UnityEngine.Object) null && theItem.AuracurseGainSelfValue2 > 0)
      {
        ++num7;
        aux.Append(this.ColorTextArray("aura", this.NumFormat(this.GetFinalAuraCharges(theItem.AuracurseGainSelf2.Id, theItem.AuracurseGainSelfValue2, character)), this.SpriteText(theItem.AuracurseGainSelf2.Id)));
      }
      if ((UnityEngine.Object) theItem.AuracurseGainSelf3 != (UnityEngine.Object) null && theItem.AuracurseGainSelfValue3 > 0)
      {
        ++num7;
        aux.Append(this.ColorTextArray("aura", this.NumFormat(this.GetFinalAuraCharges(theItem.AuracurseGainSelf3.Id, theItem.AuracurseGainSelfValue3, character)), this.SpriteText(theItem.AuracurseGainSelf3.Id)));
      }
      string auraSprites = "";
      string curseSprites = "";
      SetAuraCurseSpritesText(ref auraSprites, ref curseSprites, theItem.auracurseHeal1);
      SetAuraCurseSpritesText(ref auraSprites, ref curseSprites, theItem.auracurseHeal2);
      SetAuraCurseSpritesText(ref auraSprites, ref curseSprites, theItem.auracurseHeal3);
      if (!string.IsNullOrEmpty(auraSprites))
      {
        builder.Append(string.Format(Texts.Instance.GetText("cardsPurge"), (object) auraSprites));
        builder.Append("\n");
      }
      if (!string.IsNullOrEmpty(curseSprites))
      {
        builder.Append(string.Format(Texts.Instance.GetText("cardsDispel"), (object) curseSprites));
        builder.Append("\n");
      }
      if (num7 > 0)
      {
        if (theItem.HealQuantity > 0 || theItem.EnergyQuantity > 0 || theItem.HealPercentQuantity > 0)
        {
          StringBuilder stringBuilder14 = new StringBuilder();
          if (flag2)
            stringBuilder14.Append(string.Format(Texts.Instance.GetText("cardsAnd"), (object) builder.ToString(), (object) string.Format(Functions.LowercaseFirst(Texts.Instance.GetText("cardsGain")), (object) aux.ToString())));
          else
            stringBuilder14.Append(string.Format(Texts.Instance.GetText("cardsAnd"), (object) builder.ToString(), (object) string.Format(Functions.LowercaseFirst(Texts.Instance.GetText("cardsSuffer")), (object) aux.ToString())));
          builder.Clear();
          builder.Append(stringBuilder14.ToString());
        }
        else if (flag2)
        {
          if (theItem.ChooseOneACToGain)
            builder.Append(string.Format(Texts.Instance.GetText("cardsGainOneOf"), (object) aux.ToString()));
          else
            builder.Append(string.Format(Texts.Instance.GetText("cardsGain"), (object) aux.ToString()));
        }
        else if (theItem.ChooseOneACToGain)
          builder.Append(string.Format(Texts.Instance.GetText("cardsSufferOneOf"), (object) aux.ToString()));
        else
          builder.Append(string.Format(Texts.Instance.GetText("cardsSuffer"), (object) aux.ToString()));
        builder.Append("\n");
        aux.Clear();
      }
      if (theItem.CardNum > 0)
      {
        string str33;
        if ((UnityEngine.Object) theItem.CardToGain != (UnityEngine.Object) null)
        {
          if (theItem.CardNum > 1)
            aux.Append(this.ColorTextArray("", this.NumFormat(theItem.CardNum), this.SpriteText("card")));
          else
            aux.Append(this.SpriteText("card"));
          CardData cardData = Globals.Instance.GetCardData(theItem.CardToGain.Id, false);
          if ((UnityEngine.Object) cardData != (UnityEngine.Object) null)
          {
            aux.Append(this.ColorFromCardDataRarity(cardData));
            aux.Append(cardData.CardName);
            aux.Append("</color>");
          }
          str33 = aux.ToString();
          aux.Clear();
        }
        else
        {
          if (theItem.CardNum > 1)
            aux.Append(this.ColorTextArray("", this.NumFormat(theItem.CardNum), this.SpriteText("card")));
          else
            aux.Append(this.SpriteText("card"));
          if (theItem.CardToGainType != Enums.CardType.None)
          {
            aux.Append("<color=#5E3016>");
            aux.Append(Texts.Instance.GetText(Enum.GetName(typeof (Enums.CardType), (object) theItem.CardToGainType)));
            aux.Append("</color>");
          }
          str33 = aux.ToString();
          aux.Clear();
        }
        string str34 = "";
        if (theItem.Permanent)
        {
          if (theItem.Vanish)
          {
            if (theItem.CostZero)
              str34 = string.Format(Texts.Instance.GetText("cardsAddCostVanish"), (object) 0);
            else if (theItem.CostReduction > 0)
              str34 = string.Format(Texts.Instance.GetText("cardsAddCostReducedVanish"), (object) this.NumFormatItem(theItem.CostReduction, true));
          }
          else if (theItem.CostZero)
            str34 = string.Format(Texts.Instance.GetText("cardsAddCost"), (object) 0);
          else if (theItem.CostReduction > 0)
            str34 = string.Format(Texts.Instance.GetText("cardsAddCostReduced"), (object) this.NumFormatItem(theItem.CostReduction, true));
        }
        else if (theItem.Vanish)
        {
          if (theItem.CostZero)
            str34 = string.Format(Texts.Instance.GetText("cardsAddCostVanishTurn"), (object) 0);
          else if (theItem.CostReduction > 0)
            str34 = string.Format(Texts.Instance.GetText("cardsAddCostReducedVanishTurn"), (object) this.NumFormatItem(theItem.CostReduction, true));
        }
        else if (theItem.CostZero)
          str34 = string.Format(Texts.Instance.GetText("cardsAddCostTurn"), (object) 0);
        else if (theItem.CostReduction > 0)
          str34 = string.Format(Texts.Instance.GetText("cardsAddCostReducedTurn"), (object) this.NumFormatItem(theItem.CostReduction, true));
        if (theItem.DuplicateActive)
        {
          if (theItem.CardPlace == Enums.CardPlace.Hand)
            builder.Append(Texts.Instance.GetText("cardsDuplicateHand"));
        }
        else if (theItem.CardPlace == Enums.CardPlace.RandomDeck)
          builder.Append(string.Format(Texts.Instance.GetText("cardsIDShuffleDeck"), (object) str33));
        else if (theItem.CardPlace == Enums.CardPlace.Cast)
        {
          if ((UnityEngine.Object) theItem.CardToGain != (UnityEngine.Object) null)
          {
            CardData cardData = Globals.Instance.GetCardData(theItem.CardToGain.Id, false);
            if ((UnityEngine.Object) cardData != (UnityEngine.Object) null)
            {
              aux.Append(this.ColorFromCardDataRarity(cardData));
              aux.Append(cardData.CardName);
              aux.Append("</color>");
            }
          }
          builder.Append(string.Format(Texts.Instance.GetText("cardsCast"), (object) aux.ToString()));
          aux.Clear();
        }
        else if (theItem.CardPlace == Enums.CardPlace.Hand || theItem.CardPlace == Enums.CardPlace.TopDeck)
        {
          if (theItem.CardNum > 1)
            aux.Append(this.ColorTextArray("", this.NumFormat(theItem.CardNum), this.SpriteText("card")));
          else
            aux.Append(this.SpriteText("card"));
          if ((UnityEngine.Object) theItem.CardToGain != (UnityEngine.Object) null)
          {
            CardData cardData = Globals.Instance.GetCardData(theItem.CardToGain.Id, false);
            if ((UnityEngine.Object) cardData != (UnityEngine.Object) null)
            {
              aux.Append(this.ColorFromCardDataRarity(cardData));
              aux.Append(cardData.CardName);
              aux.Append("</color>");
            }
          }
          aux.Clear();
          string _id = "cardsIDPlaceHand";
          if (theItem.CardPlace == Enums.CardPlace.TopDeck)
            _id = "cardsIDPlaceTopDeck";
          builder.Append(string.Format(Texts.Instance.GetText(_id), (object) str33));
        }
        if (str34 != "")
        {
          builder.Append(" ");
          builder.Append(grColor);
          builder.Append(str34);
          builder.Append(endColor);
        }
        if (theItem.CardsReduced == 0)
          builder.Append("\n");
        else
          builder.Append(" ");
      }
      if (theItem.CardsReduced > 0)
      {
        StringBuilder stringBuilder15 = new StringBuilder();
        stringBuilder15.Append("<color=#5E3016>");
        stringBuilder15.Append(theItem.CardsReduced);
        stringBuilder15.Append("</color>");
        string str35 = stringBuilder15.ToString();
        string str36;
        if (theItem.CardToReduceType == Enums.CardType.None)
        {
          str36 = "  <sprite name=cards>";
        }
        else
        {
          stringBuilder15.Clear();
          stringBuilder15.Append("<color=#5E3016>");
          stringBuilder15.Append(Texts.Instance.GetText(Enum.GetName(typeof (Enums.CardType), (object) theItem.CardToReduceType)));
          stringBuilder15.Append("</color>");
          str36 = stringBuilder15.ToString();
        }
        stringBuilder15.Clear();
        stringBuilder15.Append("<color=#111111>");
        stringBuilder15.Append(Mathf.Abs(theItem.CostReduceReduction));
        stringBuilder15.Append("</color>");
        string str37 = stringBuilder15.ToString();
        string str38 = theItem.CostReduceEnergyRequirement <= 0 ? "<space=-.2>" : "<color=#444><size=-.2>" + string.Format(Texts.Instance.GetText("itemReduceCost"), (object) theItem.CostReduceEnergyRequirement) + "</size></color>";
        if (theItem.CostReducePermanent && theItem.ReduceHighestCost)
        {
          string str39;
          if (theItem.CardsReduced == 1)
          {
            str39 = "";
          }
          else
          {
            index = theItem.CardsReduced;
            str39 = "<color=#111111>(" + index.ToString() + ")</color> ";
          }
          builder.Append(string.Format(Texts.Instance.GetText("itemReduceHighestPermanent"), (object) str39, (object) str36, (object) str37, (object) str38));
        }
        else if (theItem.CostReducePermanent)
        {
          if (theItem.CardsReduced != 10)
          {
            if (theItem.CostReduceReduction > 0)
              builder.Append(string.Format(Texts.Instance.GetText("itemReduce"), (object) str35, (object) str36, (object) str37, (object) str38));
            else
              builder.Append(string.Format(Texts.Instance.GetText("itemIncrease"), (object) str35, (object) str36, (object) str37, (object) str38));
          }
          else if (theItem.CostReduceReduction > 0)
            builder.Append(string.Format(Texts.Instance.GetText("itemReduceAll"), (object) str36, (object) str37, (object) str38));
          else
            builder.Append(string.Format(Texts.Instance.GetText("itemIncreaseAll"), (object) str36, (object) str37, (object) str38));
        }
        else if (theItem.ReduceHighestCost)
        {
          string str40;
          if (theItem.CardsReduced == 1)
          {
            str40 = "";
          }
          else
          {
            index = theItem.CardsReduced;
            str40 = "<color=#111111>(" + index.ToString() + ")</color> ";
          }
          if (theItem.CostReduceReduction > 0)
            builder.Append(string.Format(Texts.Instance.GetText("itemReduceHighestTurn"), (object) str40, (object) str36, (object) str37, (object) str38));
          else
            builder.Append(string.Format(Texts.Instance.GetText("itemIncreaseHighestDiscarded"), (object) str40, (object) str36, (object) str37, (object) str38));
        }
        else if (theItem.CardsReduced != 10)
        {
          if (theItem.CostReduceReduction > 0)
            builder.Append(string.Format(Texts.Instance.GetText("itemReduceTurn"), (object) str35, (object) str36, (object) str37, (object) str38));
          else
            builder.Append(string.Format(Texts.Instance.GetText("itemIncreaseTurn"), (object) str35, (object) str36, (object) str37, (object) str38));
        }
        else if (theItem.CostReduceReduction > 0)
          builder.Append(string.Format(Texts.Instance.GetText("itemReduceTurnAll"), (object) str36, (object) str37, (object) str38));
        else
          builder.Append(string.Format(Texts.Instance.GetText("itemIncreaseTurnAll"), (object) str36, (object) str37, (object) str38));
        builder.Append("\n");
      }
    }
    if (!string.IsNullOrEmpty(this.descriptionId))
    {
      List<object> objectList = new List<object>();
      string[] descriptionArgs = this.descriptionArgs;
      for (index = 0; index < descriptionArgs.Length; ++index)
      {
        if (string.Equals(descriptionArgs[index].Trim(), "DestroyAfterUses", StringComparison.OrdinalIgnoreCase))
          objectList.Add((object) theItem.DestroyAfterUses);
      }
      if (objectList.Count > 0)
        builder.Append(string.Format(Texts.Instance.GetText(this.descriptionId), objectList.ToArray()));
    }
    if (theItem.DestroyStartOfTurn || theItem.DestroyEndOfTurn)
    {
      builder.Append("<voffset=-.1><size=-.05><color=#1A505A>- ");
      builder.Append(Texts.Instance.GetText("itemDestroyStartTurn"));
      builder.Append(" -</color></size>");
    }
    if (theItem.DestroyAfterUses > 0 && !theItem.UseTheNextInsteadWhenYouPlay)
    {
      builder.Append("<nobr><size=-.05><color=#1A505A>- ");
      if (theItem.DestroyAfterUses > 1)
        builder.Append(string.Format(Texts.Instance.GetText("itemLastUses"), (object) theItem.DestroyAfterUses));
      else
        builder.Append(Texts.Instance.GetText("itemLastUse"));
      builder.Append(" -</color></size></nobr>");
    }
    if (theItem.TimesPerCombat > 0)
    {
      builder.Append("<nobr><size=-.05><color=#1A505A>- ");
      if (theItem.TimesPerCombat == 1)
        builder.Append(Texts.Instance.GetText("itemOncePerCombat"));
      else if (theItem.TimesPerCombat == 2)
        builder.Append(Texts.Instance.GetText("itemTwicePerCombat"));
      else if (theItem.TimesPerCombat == 3)
        builder.Append(Texts.Instance.GetText("itemThricePerCombat"));
      builder.Append(" -</color></size></nobr>");
    }
    if (theItem.StealAuras > 0)
    {
      string _id = "cardsStealAuras";
      if (theItem.ItemTarget == Enums.ItemTarget.RandomEnemy)
        _id = "cardsStealAurasFromRandomEnemy";
      builder.Append(string.Format(Texts.Instance.GetText(_id), (object) theItem.StealAuras.ToString()));
      builder.Append("\n");
    }
    if (!theItem.PassSingleAndCharacterRolls)
      return;
    builder.Append(Texts.Instance.GetText("cardsPassEventRoll"));
    builder.Append("\n");

    void GetXEqualsDecriptionValue()
    {
      SpecialValues specialValues = new SpecialValues();
      if (theItem.AuracurseGain1SpecialValue.Use)
        specialValues = theItem.AuracurseGain1SpecialValue;
      if (theItem.AuracurseGain2SpecialValue.Use)
        specialValues = theItem.AuracurseGain2SpecialValue;
      if (theItem.AuracurseGain3SpecialValue.Use)
        specialValues = theItem.AuracurseGain3SpecialValue;
      if (theItem.DttSpecialValues1.Use)
        specialValues = theItem.DttSpecialValues1;
      if (theItem.DttSpecialValues2.Use)
        specialValues = theItem.DttSpecialValues2;
      if (theItem.HealQuantitySpecialValue.Use)
        specialValues = theItem.HealQuantitySpecialValue;
      if (!specialValues.Use)
        return;
      builder.Append(goldColor);
      builder.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYour"), (object) this.SpecialModifierDescription(specialValues, this.item ?? this.ItemEnchantment), (object) string.Format(" x{0}", (object) specialValues.Multiplier)));
      builder.Append(endColor);
      builder.Append("\n");
    }

    void SetAuraCurseSpritesText(
      ref string auraSprites,
      ref string curseSprites,
      AuraCurseData acData)
    {
      if (!(bool) (UnityEngine.Object) acData)
        return;
      if (acData.IsAura)
        auraSprites += this.SpriteText(acData.Id);
      else
        curseSprites += this.SpriteText(acData.Id);
    }
  }

  private void AppendCardDescription(
    Character character,
    StringBuilder builder,
    StringBuilder aux,
    string grColor,
    string endColor,
    string br1,
    string goldColor)
  {
    int num1 = 0;
    string str1 = "";
    string str2 = "";
    string str3 = "";
    float num2 = 0.0f;
    string str4 = "";
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = true;
    StringBuilder stringBuilder1 = new StringBuilder();
    if (this.damage > 0 || this.damage2 > 0 || this.damageSelf > 0 || this.DamageSelf2 > 0)
      flag3 = false;
    if (this.drawCard != 0 && !this.drawCardSpecialValueGlobal)
    {
      builder.Append(string.Format(Texts.Instance.GetText("cardsDraw"), (object) this.ColorTextArray("", this.NumFormat(this.drawCard), this.SpriteText("card"))));
      builder.Append("<br>");
    }
    if (this.specialValueGlobal == Enums.CardSpecialValue.DiscardedCards)
    {
      if (this.discardCardPlace == Enums.CardPlace.Vanish)
        builder.Append(string.Format(Texts.Instance.GetText("cardsVanish"), (object) this.ColorTextArray("", "X", this.SpriteText("card"))));
      else
        builder.Append(string.Format(Texts.Instance.GetText("cardsDiscard"), (object) this.ColorTextArray("", "X", this.SpriteText("card"))));
      builder.Append("\n");
    }
    if (this.drawCardSpecialValueGlobal)
    {
      builder.Append(string.Format(Texts.Instance.GetText("cardsDraw"), (object) this.ColorTextArray("aura", "X", this.SpriteText("card"))));
      builder.Append("<br>");
    }
    if (this.addCard != 0)
    {
      string str5;
      if (this.addCardId != "")
      {
        aux.Append(this.ColorTextArray("", this.NumFormat(this.addCard), this.SpriteText("card")));
        CardData cardData = Globals.Instance.GetCardData(this.addCardId, false);
        if ((UnityEngine.Object) cardData != (UnityEngine.Object) null)
        {
          aux.Append(this.ColorFromCardDataRarity(cardData));
          aux.Append(cardData.CardName);
          aux.Append("</color>");
        }
        str5 = aux.ToString();
        aux.Clear();
      }
      else
      {
        if (this.addCardChoose > 0)
          aux.Append(this.ColorTextArray("", this.NumFormat(this.addCardChoose), this.SpriteText("card")));
        else
          aux.Append(this.ColorTextArray("", this.NumFormat(this.addCard), this.SpriteText("card")));
        if (this.addCardType != Enums.CardType.None)
        {
          aux.Append("<color=#5E3016>");
          aux.Append(Texts.Instance.GetText(Enum.GetName(typeof (Enums.CardType), (object) this.addCardType)));
          aux.Append("</color>");
        }
        str5 = aux.ToString();
        aux.Clear();
      }
      string str6 = "";
      if (this.addCardReducedCost == -1)
        str6 = !this.addCardVanish ? (!this.addCardCostTurn ? string.Format(Texts.Instance.GetText("cardsAddCost"), (object) 0) : string.Format(Texts.Instance.GetText("cardsAddCostTurn"), (object) 0)) : (!this.addCardCostTurn ? string.Format(Texts.Instance.GetText("cardsAddCostVanish"), (object) 0) : string.Format(Texts.Instance.GetText("cardsAddCostVanish"), (object) 0));
      else if (this.addCardReducedCost > 0)
        str6 = !this.addCardVanish ? (!this.addCardCostTurn ? string.Format(Texts.Instance.GetText("cardsAddCostReduced"), (object) this.addCardReducedCost) : string.Format(Texts.Instance.GetText("cardsAddCostReducedTurn"), (object) this.addCardReducedCost)) : (!this.addCardCostTurn ? string.Format(Texts.Instance.GetText("cardsAddCostReducedVanish"), (object) this.addCardReducedCost) : string.Format(Texts.Instance.GetText("cardsAddCostReducedVanishTurn"), (object) this.addCardReducedCost));
      string _id = "";
      if (this.addCardId != "")
      {
        if (this.addCardPlace == Enums.CardPlace.RandomDeck)
          _id = this.targetSide == Enums.CardTargetSide.Self || this.targetSide == Enums.CardTargetSide.Enemy && this.cardClass != Enums.CardClass.Monster ? "cardsIDShuffleDeck" : "cardsIDShuffleTargetDeck";
        else if (this.addCardPlace == Enums.CardPlace.Hand)
          _id = "cardsIDPlaceHand";
        else if (this.addCardPlace == Enums.CardPlace.TopDeck)
          _id = this.targetSide != Enums.CardTargetSide.Self ? "cardsIDPlaceTargetTopDeck" : "cardsIDPlaceTopDeck";
        else if (this.addCardPlace == Enums.CardPlace.Discard)
          _id = this.targetSide == Enums.CardTargetSide.Self || this.targetSide == Enums.CardTargetSide.Enemy && this.cardClass != Enums.CardClass.Monster ? "cardsIDPlaceDiscard" : "cardsIDPlaceTargetDiscard";
      }
      else if (this.addCardFrom == Enums.CardFrom.Game)
      {
        if (this.addCardPlace == Enums.CardPlace.RandomDeck)
          _id = this.addCardChoose != 0 ? "cardsDiscoverNumberToDeck" : "cardsDiscoverToDeck";
        else if (this.addCardPlace == Enums.CardPlace.Hand)
          _id = this.addCardChoose != 0 ? "cardsDiscoverNumberToHand" : "cardsDiscoverToHand";
        else if (this.addCardPlace == Enums.CardPlace.TopDeck && this.addCardChoose != 0)
          _id = "cardsDiscoverNumberToTopDeck";
      }
      else if (this.addCardFrom == Enums.CardFrom.Deck)
      {
        if (this.addCardPlace == Enums.CardPlace.Hand)
          _id = this.addCardChoose <= 0 ? (this.addCard <= 1 ? "cardsRevealItToHand" : "cardsRevealThemToHand") : "cardsRevealNumberToHand";
        else if (this.addCardPlace == Enums.CardPlace.TopDeck)
          _id = this.addCardChoose <= 0 ? (this.addCard <= 1 ? "cardsRevealItToTopDeck" : "cardsRevealThemToTopDeck") : "cardsRevealNumberToTopDeck";
      }
      else if (this.addCardFrom == Enums.CardFrom.Discard)
      {
        if (this.addCardPlace == Enums.CardPlace.TopDeck)
          _id = "cardsPickToTop";
        else if (this.addCardPlace == Enums.CardPlace.Hand)
          _id = "cardsPickToHand";
      }
      else if (this.addCardFrom == Enums.CardFrom.Hand)
      {
        if (this.targetSide == Enums.CardTargetSide.Friend)
        {
          if (this.addCardPlace == Enums.CardPlace.TopDeck)
            _id = "cardsDuplicateToTargetTopDeck";
          else if (this.addCardPlace == Enums.CardPlace.RandomDeck)
            _id = "cardsDuplicateToTargetRandomDeck";
        }
        else if (this.addCardPlace == Enums.CardPlace.Hand)
          _id = "cardsDuplicateToHand";
      }
      else if (this.addCardFrom == Enums.CardFrom.Vanish)
      {
        if (this.addCardPlace == Enums.CardPlace.TopDeck)
          _id = "cardsFromVanishToTop";
        else if (this.addCardPlace == Enums.CardPlace.Hand)
          _id = "cardsFromVanishToHand";
      }
      builder.Append(string.Format(Texts.Instance.GetText(_id), (object) str5, (object) this.ColorTextArray("", this.NumFormat(this.addCard))));
      if (str6 != "")
      {
        builder.Append(" ");
        builder.Append(grColor);
        builder.Append(str6);
        builder.Append(endColor);
      }
      builder.Append("\n");
    }
    if (this.discardCard != 0)
    {
      if (this.discardCardType != Enums.CardType.None)
      {
        aux.Append("<color=#5E3016>");
        aux.Append(Texts.Instance.GetText(Enum.GetName(typeof (Enums.CardType), (object) this.discardCardType)));
        aux.Append("</color>");
      }
      if (this.discardCardPlace == Enums.CardPlace.Discard)
      {
        if (!this.discardCardAutomatic)
        {
          builder.Append(string.Format(Texts.Instance.GetText("cardsDiscard"), (object) this.ColorTextArray("", this.NumFormat(this.discardCard), this.SpriteText("card"))));
          builder.Append(aux);
          builder.Append("\n");
        }
        else
        {
          builder.Append(string.Format(Texts.Instance.GetText("cardsDiscard"), (object) this.ColorTextArray("", this.NumFormat(this.discardCard), this.SpriteText("cardrandom"))));
          builder.Append(aux);
          builder.Append("\n");
        }
      }
      else if (this.discardCardPlace == Enums.CardPlace.TopDeck)
      {
        builder.Append(string.Format(Texts.Instance.GetText("cardsPlaceToTop"), (object) this.ColorTextArray("", this.NumFormat(this.discardCard), this.SpriteText("card"), aux.ToString().Trim())));
        builder.Append("\n");
      }
      else if (this.discardCardPlace == Enums.CardPlace.Vanish)
      {
        builder.Append(string.Format(Texts.Instance.GetText("cardsVanish"), (object) this.ColorTextArray("", this.NumFormat(this.discardCard), this.SpriteText("card"), aux.ToString().Trim())));
        builder.Append("\n");
      }
      aux.Clear();
    }
    if (this.lookCards != 0)
    {
      builder.Append(string.Format(Texts.Instance.GetText("cardsLook"), (object) this.ColorTextArray("", this.NumFormat(this.lookCards), this.SpriteText("card"))));
      builder.Append("\n");
      if (this.lookCardsDiscardUpTo == -1)
      {
        builder.Append(Texts.Instance.GetText("cardsDiscardAny"));
        builder.Append("\n");
      }
      else if (this.lookCardsDiscardUpTo > 0)
      {
        builder.Append(string.Format(Texts.Instance.GetText("cardsDiscardUpTo"), (object) this.ColorTextArray("", this.NumFormat(this.lookCardsDiscardUpTo))));
        builder.Append("\n");
      }
      else if (this.lookCardsVanishUpTo > 0)
      {
        builder.Append(string.Format(Texts.Instance.GetText("cardsVanishUpTo"), (object) this.ColorTextArray("", this.NumFormat(this.lookCardsVanishUpTo))));
        builder.Append("\n");
      }
    }
    num1 = 0;
    if ((UnityEngine.Object) this.summonUnit != (UnityEngine.Object) null && this.summonUnitNum > 0)
    {
      aux.Append("\n<color=#5E3016>");
      if (this.summonUnitNum > 1)
      {
        aux.Append(this.summonUnitNum);
        aux.Append(" ");
      }
      NPCData npc = Globals.Instance.GetNPC(this.summonUnit?.Id);
      if ((UnityEngine.Object) npc != (UnityEngine.Object) null)
        aux.Append(npc.NPCName);
      if (this.metamorph)
        builder.Append(string.Format(Texts.Instance.GetText("cardsMetamorph"), (object) aux.ToString()));
      else if (this.evolve)
        builder.Append(string.Format(Texts.Instance.GetText("cardsEvolve"), (object) aux.ToString()));
      else
        builder.Append(string.Format(Texts.Instance.GetText("cardsSummon"), (object) aux.ToString()));
      builder.Append("</color>\n");
      aux.Clear();
    }
    if (this.dispelAuras > 0)
    {
      builder.Append(string.Format(Texts.Instance.GetText("cardsPurge"), (object) this.ColorTextArray("aura", this.dispelAuras.ToString())));
      builder.Append("\n");
    }
    else if (this.dispelAuras == -1)
    {
      builder.Append(Texts.Instance.GetText("cardsPurgeAll"));
      builder.Append("\n");
    }
    num1 = 0;
    if (this.specialValueGlobal == Enums.CardSpecialValue.None && this.specialValue1 == Enums.CardSpecialValue.None && this.specialValue2 == Enums.CardSpecialValue.None)
    {
      StringBuilder stringBuilder2 = new StringBuilder();
      StringBuilder stringBuilder3 = new StringBuilder();
      if ((UnityEngine.Object) this.healAuraCurseName != (UnityEngine.Object) null)
      {
        if (this.healAuraCurseName.IsAura)
          stringBuilder2.Append(this.SpriteText(this.healAuraCurseName.ACName));
        else
          stringBuilder3.Append(this.SpriteText(this.healAuraCurseName.ACName));
      }
      if ((UnityEngine.Object) this.healAuraCurseName2 != (UnityEngine.Object) null)
      {
        if (this.healAuraCurseName2.IsAura)
          stringBuilder2.Append(this.SpriteText(this.healAuraCurseName2.ACName));
        else
          stringBuilder3.Append(this.SpriteText(this.healAuraCurseName2.ACName));
      }
      if ((UnityEngine.Object) this.healAuraCurseName3 != (UnityEngine.Object) null)
      {
        if (this.healAuraCurseName3.IsAura)
          stringBuilder2.Append(this.SpriteText(this.healAuraCurseName3.ACName));
        else
          stringBuilder3.Append(this.SpriteText(this.healAuraCurseName3.ACName));
      }
      if ((UnityEngine.Object) this.healAuraCurseName4 != (UnityEngine.Object) null)
      {
        if (this.healAuraCurseName4.IsAura)
          stringBuilder2.Append(this.SpriteText(this.healAuraCurseName4.ACName));
        else
          stringBuilder3.Append(this.SpriteText(this.healAuraCurseName4.ACName));
      }
      if (stringBuilder2.Length > 0)
      {
        string _id = "cardsPurge";
        if (this.targetSide == Enums.CardTargetSide.Self)
          _id = "cardsPurgeYour";
        builder.Append(string.Format(Texts.Instance.GetText(_id), (object) stringBuilder2.ToString()));
        builder.Append("\n");
      }
      if (stringBuilder3.Length > 0)
      {
        string _id = "cardsDispel";
        if (this.targetSide == Enums.CardTargetSide.Self)
          _id = "cardsDispelYour";
        builder.Append(string.Format(Texts.Instance.GetText(_id), (object) stringBuilder3.ToString()));
        builder.Append("\n");
      }
      if (this.healCurses > 0)
      {
        builder.Append(string.Format(Texts.Instance.GetText("cardsDispel"), (object) this.ColorTextArray("curse", this.healCurses.ToString())));
        builder.Append("\n");
      }
      if (this.healCurses == -1)
      {
        builder.Append(Texts.Instance.GetText("cardsDispelAll"));
        builder.Append("\n");
      }
      if ((UnityEngine.Object) this.healAuraCurseSelf != (UnityEngine.Object) null)
      {
        if (this.healAuraCurseSelf.IsAura)
          builder.Append(string.Format(Texts.Instance.GetText("cardsPurgeYour"), (object) this.SpriteText(this.healAuraCurseSelf.ACName)));
        else
          builder.Append(string.Format(Texts.Instance.GetText("cardsDispelYour"), (object) this.SpriteText(this.healAuraCurseSelf.ACName)));
        builder.Append("\n");
      }
    }
    else
    {
      if (this.healCurses > 0)
      {
        if (this.targetSide == Enums.CardTargetSide.Enemy)
          builder.Append(string.Format(Texts.Instance.GetText("cardsPurge"), (object) this.healCurses));
        else
          builder.Append(string.Format(Texts.Instance.GetText("cardsDispel"), (object) this.healCurses));
        builder.Append("\n");
      }
      if ((UnityEngine.Object) this.healAuraCurseName4 != (UnityEngine.Object) null && (UnityEngine.Object) this.healAuraCurseName3 == (UnityEngine.Object) null)
      {
        StringBuilder stringBuilder4 = new StringBuilder();
        StringBuilder stringBuilder5 = new StringBuilder();
        if (this.healAuraCurseName4.IsAura)
          stringBuilder4.Append(this.SpriteText(this.healAuraCurseName4.ACName));
        else
          stringBuilder5.Append(this.SpriteText(this.healAuraCurseName4.ACName));
        if (stringBuilder4.Length > 0)
        {
          builder.Append(string.Format(Texts.Instance.GetText("cardsPurge"), (object) stringBuilder4.ToString()));
          builder.Append("\n");
        }
        if (stringBuilder5.Length > 0)
        {
          builder.Append(string.Format(Texts.Instance.GetText("cardsDispel"), (object) stringBuilder5.ToString()));
          builder.Append("\n");
        }
      }
    }
    if (this.transferCurses > 0)
    {
      builder.Append(string.Format(Texts.Instance.GetText("cardsTransferCurse"), (object) this.transferCurses.ToString()));
      builder.Append("\n");
    }
    if (this.stealAuras > 0)
    {
      builder.Append(string.Format(Texts.Instance.GetText("cardsStealAuras"), (object) this.stealAuras.ToString()));
      builder.Append("\n");
    }
    if (this.increaseAuras > 0)
    {
      builder.Append(string.Format(Texts.Instance.GetText("cardsIncreaseAura"), (object) this.increaseAuras.ToString()));
      builder.Append("\n");
    }
    if (this.increaseCurses > 0)
    {
      builder.Append(string.Format(Texts.Instance.GetText("cardsIncreaseCurse"), (object) this.increaseCurses.ToString()));
      builder.Append("\n");
    }
    if (this.reduceAuras > 0)
    {
      builder.Append(string.Format(Texts.Instance.GetText("cardsReduceAura"), (object) this.reduceAuras.ToString()));
      builder.Append("\n");
    }
    if (this.reduceCurses > 0)
    {
      builder.Append(string.Format(Texts.Instance.GetText("cardsReduceCurse"), (object) this.reduceCurses.ToString()));
      builder.Append("\n");
    }
    bool flag4 = false;
    if (this.damage > 0)
    {
      if (this.damage > 0 && this.damage2 > 0 && this.damageType == this.damageType2 && !this.damageSpecialValueGlobal && !this.damageSpecialValue1 && !this.damageSpecialValue2 && !this.damage2SpecialValueGlobal && !this.damage2SpecialValue1 && !this.damage2SpecialValue2)
      {
        aux.Append(this.ColorTextArray("damage", this.NumFormat(this.damagePreCalculatedCombined), this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) this.damageType))));
        flag4 = true;
      }
      else
      {
        if (this.damage == 1 && (this.damageSpecialValueGlobal || this.damageSpecialValue1 || this.damageSpecialValue2))
          aux.Append(this.ColorTextArray("damage", "X", this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) this.damageType))));
        else
          aux.Append(this.ColorTextArray("damage", this.NumFormat(this.damagePreCalculated), this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) this.damageType))));
        if (this.damage2 > 0)
        {
          flag4 = true;
          if (this.damage2 == 1 && (this.damage2SpecialValue1 || this.damage2SpecialValue2 || this.damage2SpecialValueGlobal))
          {
            if (this.damageType == this.damageType2)
            {
              aux.Append("<space=-.3>");
              aux.Append("+");
              aux.Append(this.ColorTextArray("damage", "X", this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) this.damageType))));
            }
            else
              aux.Append(this.ColorTextArray("damage", "X", this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) this.damageType2))));
          }
          else
            aux.Append(this.ColorTextArray("damage", this.NumFormat(this.damagePreCalculated2), this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) this.damageType2))));
        }
      }
      builder.Append(string.Format(Texts.Instance.GetText("cardsDealDamage"), (object) aux.ToString()));
      builder.Append("\n");
      aux.Clear();
    }
    if (!flag4 && this.damage2 > 0)
    {
      if (this.damage2 == 1 && (this.damage2SpecialValueGlobal || this.damage2SpecialValue1 || this.damage2SpecialValue2))
        aux.Append(this.ColorTextArray("damage", "X", this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) this.damageType2))));
      else
        aux.Append(this.ColorTextArray("damage", this.NumFormat(this.damagePreCalculated2), this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) this.damageType2))));
      builder.Append(string.Format(Texts.Instance.GetText("cardsDealDamage"), (object) aux.ToString()));
      builder.Append("\n");
      aux.Clear();
    }
    int num3 = 0;
    if (this.damageSelf > 0)
    {
      ++num3;
      if (this.damageSpecialValueGlobal || this.damageSpecialValue1 || this.damageSpecialValue2)
        aux.Append(this.ColorTextArray("damage", "X", this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) this.damageType))));
      else
        aux.Append(this.ColorTextArray("damage", this.NumFormat(this.damageSelfPreCalculated), this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) this.damageType))));
    }
    if (this.damageSelf2 > 0)
    {
      ++num3;
      if (this.damage2SpecialValueGlobal || this.damage2SpecialValue1 || this.damage2SpecialValue2)
        aux.Append(this.ColorTextArray("damage", "X", this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) this.damageType2))));
      else
        aux.Append(this.ColorTextArray("damage", this.NumFormat(this.damageSelfPreCalculated2), this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) this.damageType2))));
    }
    if (num3 > 0)
    {
      if (num3 > 2)
      {
        aux.Insert(0, br1);
        aux.Insert(0, "\n");
      }
      if (this.targetSide == Enums.CardTargetSide.Self)
      {
        builder.Append(string.Format(Texts.Instance.GetText("cardsSuffer"), (object) aux.ToString()));
        builder.Append("\n");
      }
      else
      {
        stringBuilder1.Append(string.Format(Texts.Instance.GetText("cardsYouSuffer"), (object) aux.ToString()));
        stringBuilder1.Append("\n");
      }
      aux.Clear();
    }
    if (stringBuilder1.Length > 0)
    {
      builder.Append(stringBuilder1.ToString());
      stringBuilder1.Clear();
    }
    if ((double) this.healSelfPerDamageDonePercent > 0.0)
    {
      builder.Append(string.Format(Texts.Instance.GetText("cardsHealSelfPerDamage"), (object) this.healSelfPerDamageDonePercent.ToString()));
      builder.Append("\n");
    }
    if (this.selfHealthLoss != 0 && !this.selfHealthLossSpecialGlobal)
    {
      if (this.selfHealthLossSpecialValue1)
        builder.Append(string.Format(Texts.Instance.GetText("cardsLoseHp"), (object) this.ColorTextArray("damage", "X HP")));
      else
        builder.Append(string.Format(Texts.Instance.GetText("cardsLoseHp"), (object) this.ColorTextArray("damage", this.NumFormat(this.selfHealthLoss), "HP")));
      builder.Append("\n");
    }
    if ((this.targetSide == Enums.CardTargetSide.Friend || this.targetSide == Enums.CardTargetSide.FriendNotSelf) && this.SpecialValueGlobal == Enums.CardSpecialValue.AuraCurseYours && (UnityEngine.Object) this.SpecialAuraCurseNameGlobal != (UnityEngine.Object) null && (double) this.SpecialValueModifierGlobal > 0.0)
    {
      flag1 = true;
      builder.Append(string.Format(Texts.Instance.GetText("cardsShareYour"), (object) this.SpriteText(this.SpecialAuraCurseNameGlobal.ACName)));
      builder.Append("\n");
    }
    if (!this.Damage2SpecialValue1 && !flag1 && (this.specialValueGlobal != Enums.CardSpecialValue.None || this.specialValue1 != Enums.CardSpecialValue.None || this.specialValue2 != Enums.CardSpecialValue.None))
    {
      if (!this.damageSpecialValueGlobal && !this.damageSpecialValue1 && !this.damageSpecialValue2)
      {
        if (this.targetSide == Enums.CardTargetSide.Self && (this.specialValueGlobal == Enums.CardSpecialValue.AuraCurseTarget || this.specialValueGlobal == Enums.CardSpecialValue.AuraCurseYours))
        {
          if ((UnityEngine.Object) this.specialAuraCurseNameGlobal != (UnityEngine.Object) null)
            str1 = this.SpriteText(this.specialAuraCurseNameGlobal.ACName);
          if (this.auraChargesSpecialValueGlobal)
          {
            if ((UnityEngine.Object) this.aura != (UnityEngine.Object) null)
              str2 = this.SpriteText(this.aura.ACName);
            if ((UnityEngine.Object) this.aura2 != (UnityEngine.Object) null && this.auraCharges2SpecialValueGlobal)
              str3 = this.SpriteText(this.aura2.ACName);
          }
          else if (this.curseChargesSpecialValueGlobal)
          {
            if ((UnityEngine.Object) this.curse != (UnityEngine.Object) null)
              str2 = this.SpriteText(this.curse.ACName);
            if ((UnityEngine.Object) this.curse2 != (UnityEngine.Object) null && this.curseCharges2SpecialValueGlobal)
              str3 = this.SpriteText(this.curse2.ACName);
          }
        }
        else if (this.specialValue1 == Enums.CardSpecialValue.AuraCurseTarget)
        {
          if ((UnityEngine.Object) this.specialAuraCurseName1 != (UnityEngine.Object) null)
            str1 = this.SpriteText(this.specialAuraCurseName1.ACName);
          if (this.auraChargesSpecialValue1)
          {
            if ((UnityEngine.Object) this.aura != (UnityEngine.Object) null)
              str2 = this.SpriteText(this.aura.ACName);
            if ((UnityEngine.Object) this.aura2 != (UnityEngine.Object) null && this.auraCharges2SpecialValue1)
              str3 = this.SpriteText(this.aura2.ACName);
          }
          else if (this.curseChargesSpecialValue1)
          {
            if ((UnityEngine.Object) this.curse != (UnityEngine.Object) null)
              str2 = this.SpriteText(this.curse.ACName);
            if ((UnityEngine.Object) this.curse2 != (UnityEngine.Object) null && this.CurseCharges2SpecialValue1)
              str3 = this.SpriteText(this.curse2.ACName);
          }
        }
        if (str1 != "" && str2 != "")
        {
          flag2 = true;
          if (str1 == str2)
          {
            if (this.specialValueGlobal == Enums.CardSpecialValue.AuraCurseTarget)
            {
              if ((double) this.specialValueModifierGlobal == 100.0)
              {
                builder.Append(string.Format(Texts.Instance.GetText("cardsDoubleTarget"), (object) str1));
                builder.Append("\n");
              }
              else if ((double) this.specialValueModifierGlobal == 200.0)
              {
                builder.Append(string.Format(Texts.Instance.GetText("cardsTripleTarget"), (object) str1));
                builder.Append("\n");
              }
              else if ((double) this.specialValueModifierGlobal < 100.0)
              {
                builder.Append(string.Format(Texts.Instance.GetText("cardsLosePercentTarget"), (object) (float) (100.0 - (double) this.specialValueModifierGlobal), (object) str1));
                builder.Append("\n");
              }
            }
            else if (this.specialValueGlobal == Enums.CardSpecialValue.AuraCurseYours)
            {
              if ((double) this.specialValueModifierGlobal == 100.0)
              {
                builder.Append(string.Format(Texts.Instance.GetText("cardsDoubleYour"), (object) str1));
                builder.Append("\n");
              }
              else if ((double) this.specialValueModifierGlobal == 200.0)
              {
                builder.Append(string.Format(Texts.Instance.GetText("cardsTripleYour"), (object) str1));
                builder.Append("\n");
              }
              else if ((double) this.specialValueModifierGlobal < 100.0)
              {
                builder.Append(string.Format(Texts.Instance.GetText("cardsLosePercentYour"), (object) (float) (100.0 - (double) this.specialValueModifierGlobal), (object) str1));
                builder.Append("\n");
              }
            }
            else if (this.specialValue1 == Enums.CardSpecialValue.AuraCurseTarget)
            {
              if ((double) this.specialValueModifier1 == 100.0)
              {
                builder.Append(string.Format(Texts.Instance.GetText("cardsDoubleTarget"), (object) str1));
                builder.Append("\n");
              }
              else if ((double) this.specialValueModifier1 == 200.0)
              {
                builder.Append(string.Format(Texts.Instance.GetText("cardsTripleTarget"), (object) str1));
                builder.Append("\n");
              }
              else if ((double) this.specialValueModifier1 < 100.0 && (UnityEngine.Object) this.healAuraCurseName != (UnityEngine.Object) null)
              {
                builder.Append(string.Format(Texts.Instance.GetText("cardsLosePercentTarget"), (object) (float) (100.0 - (double) this.specialValueModifier1), (object) str1));
                builder.Append("\n");
              }
              else
                flag2 = false;
            }
            else if (this.specialValue1 == Enums.CardSpecialValue.AuraCurseYours)
            {
              if ((double) this.specialValueModifier1 == 100.0)
              {
                builder.Append(string.Format(Texts.Instance.GetText("cardsDoubleYour"), (object) str1));
                builder.Append("\n");
              }
              else if ((double) this.specialValueModifier1 == 200.0)
              {
                builder.Append(string.Format(Texts.Instance.GetText("cardsTripleYour"), (object) str1));
                builder.Append("\n");
              }
              else if ((double) this.specialValueModifier1 < 100.0)
              {
                builder.Append(string.Format(Texts.Instance.GetText("cardsLosePercentYour"), (object) (float) (100.0 - (double) this.specialValueModifier1), (object) str1));
                builder.Append("\n");
              }
            }
          }
          else
          {
            aux.Append(str2);
            if ((double) this.specialValueModifier1 > 0.0)
              num2 = this.specialValueModifier1 / 100f;
            if ((double) num2 > 0.0 && (double) num2 != 1.0)
              str4 = "x " + num2.ToString();
            if (str4 != "")
            {
              aux.Append(" <c>");
              aux.Append(str4);
              aux.Append("</c>");
            }
            if (str3 != "")
              builder.Append(string.Format(Texts.Instance.GetText("cardsTransformIntoAnd"), (object) str1, (object) aux.ToString(), (object) str3));
            else
              builder.Append(string.Format(Texts.Instance.GetText("cardsTransformInto"), (object) str1, (object) aux.ToString()));
            builder.Append("\n");
            aux.Clear();
            num2 = 0.0f;
            str4 = "";
          }
        }
      }
    }
    if (this.energyRechargeSpecialValueGlobal)
      aux.Append(this.ColorTextArray("aura", "X", this.SpriteText("energy")));
    AuraCurseData auraCurseData = (AuraCurseData) null;
    if (!flag1 && !flag2)
    {
      int num4 = 0;
      if ((UnityEngine.Object) this.aura != (UnityEngine.Object) null && (this.auraChargesSpecialValue1 || this.auraChargesSpecialValueGlobal))
      {
        ++num4;
        aux.Append(this.ColorTextArray("aura", "X", this.SpriteText(this.aura.ACName)));
        auraCurseData = this.aura;
      }
      if ((UnityEngine.Object) this.aura2 != (UnityEngine.Object) null && (this.auraCharges2SpecialValue1 || this.auraCharges2SpecialValueGlobal))
      {
        ++num4;
        if ((UnityEngine.Object) this.aura != (UnityEngine.Object) null && (UnityEngine.Object) this.aura == (UnityEngine.Object) this.aura2)
        {
          aux.Append(this.ColorTextArray("aura", this.NumFormat(this.GetFinalAuraCharges(this.aura.Id, this.auraCharges, character)), this.SpriteText(this.aura.ACName)));
          aux.Append("+");
        }
        aux.Append(this.ColorTextArray("aura", "X", this.SpriteText(this.aura2.ACName)));
        auraCurseData = this.aura2;
      }
      if ((UnityEngine.Object) this.aura3 != (UnityEngine.Object) null && (this.auraCharges3SpecialValue1 || this.auraCharges3SpecialValueGlobal))
      {
        ++num4;
        if ((UnityEngine.Object) this.aura != (UnityEngine.Object) null && (UnityEngine.Object) this.aura == (UnityEngine.Object) this.aura3)
        {
          aux.Append(this.ColorTextArray("aura", this.NumFormat(this.GetFinalAuraCharges(this.aura.Id, this.auraCharges, character)), this.SpriteText(this.aura.ACName)));
          aux.Append("+");
        }
        if ((UnityEngine.Object) this.aura2 != (UnityEngine.Object) null && (UnityEngine.Object) this.aura == (UnityEngine.Object) this.aura3)
        {
          aux.Append(this.ColorTextArray("aura", this.NumFormat(this.GetFinalAuraCharges(this.aura2.Id, this.auraCharges2, character)), this.SpriteText(this.aura3.ACName)));
          aux.Append("+");
        }
        aux.Append(this.ColorTextArray("aura", "X", this.SpriteText(this.aura3.ACName)));
        auraCurseData = this.aura3;
      }
      if (num4 > 0)
      {
        if (this.targetSide == Enums.CardTargetSide.Self)
          builder.Append(string.Format(Texts.Instance.GetText("cardsGain"), (object) aux.ToString()));
        else
          builder.Append(string.Format(Texts.Instance.GetText("cardsGrant"), (object) aux.ToString()));
        builder.Append("\n");
        aux.Clear();
      }
    }
    if (!flag1 && !flag2)
    {
      int num5 = 0;
      if ((UnityEngine.Object) this.curse != (UnityEngine.Object) null && (this.curseChargesSpecialValue1 || this.curseChargesSpecialValueGlobal))
      {
        ++num5;
        aux.Append(this.ColorTextArray("curse", "X", this.SpriteText(this.curse.ACName)));
      }
      if ((UnityEngine.Object) this.curse2 != (UnityEngine.Object) null && (this.curseCharges2SpecialValue1 || this.curseCharges2SpecialValueGlobal))
      {
        ++num5;
        aux.Append(this.ColorTextArray("curse", "X", this.SpriteText(this.curse2.ACName)));
      }
      if ((UnityEngine.Object) this.curse3 != (UnityEngine.Object) null && (this.curseCharges3SpecialValue1 || this.curseCharges3SpecialValueGlobal))
      {
        ++num5;
        aux.Append(this.ColorTextArray("curse", "X", this.SpriteText(this.curse3.ACName)));
      }
      if (num5 > 0)
      {
        if (this.targetSide == Enums.CardTargetSide.Self)
          builder.Append(string.Format(Texts.Instance.GetText("cardsSuffer"), (object) aux.ToString()));
        else
          builder.Append(string.Format(Texts.Instance.GetText("cardsApply"), (object) aux.ToString()));
        builder.Append("\n");
        aux.Clear();
      }
      int num6 = 0;
      if ((UnityEngine.Object) this.curseSelf != (UnityEngine.Object) null && (this.curseChargesSpecialValue1 || this.curseChargesSpecialValueGlobal))
      {
        ++num6;
        aux.Append(this.ColorTextArray("curse", "X", this.SpriteText(this.curseSelf.ACName)));
      }
      if ((UnityEngine.Object) this.curseSelf2 != (UnityEngine.Object) null && (this.curseCharges2SpecialValue1 || this.curseCharges2SpecialValueGlobal))
      {
        ++num6;
        aux.Append(this.ColorTextArray("curse", "X", this.SpriteText(this.curseSelf2.ACName)));
      }
      if ((UnityEngine.Object) this.curseSelf3 != (UnityEngine.Object) null && (this.curseCharges3SpecialValue1 || this.curseCharges3SpecialValueGlobal))
      {
        ++num6;
        aux.Append(this.ColorTextArray("curse", "X", this.SpriteText(this.curseSelf3.ACName)));
      }
      if (num6 > 0)
      {
        if (this.targetSide == Enums.CardTargetSide.Self || (UnityEngine.Object) this.curseSelf != (UnityEngine.Object) null || (UnityEngine.Object) this.curseSelf2 != (UnityEngine.Object) null || (UnityEngine.Object) this.curseSelf3 != (UnityEngine.Object) null)
        {
          if (this.targetSide == Enums.CardTargetSide.Self)
          {
            builder.Append(string.Format(Texts.Instance.GetText("cardsSuffer"), (object) aux.ToString()));
            builder.Append("\n");
          }
          else if (!flag3)
          {
            stringBuilder1.Append(string.Format(Texts.Instance.GetText("cardsYouSuffer"), (object) aux.ToString()));
            stringBuilder1.Append("\n");
          }
          else
          {
            builder.Append(string.Format(Texts.Instance.GetText("cardsYouSuffer"), (object) aux.ToString()));
            builder.Append("\n");
          }
        }
        else
        {
          builder.Append(string.Format(Texts.Instance.GetText("cardsApply"), (object) aux.ToString()));
          builder.Append("\n");
        }
        aux.Clear();
      }
    }
    int num7 = 0;
    if (this.heal > 0 && (this.healSpecialValue1 || this.healSpecialValueGlobal))
    {
      aux.Append(this.ColorTextArray("heal", "X", this.SpriteText("heal")));
      num1 = num7 + 1;
    }
    if (aux.Length > 0)
    {
      builder.Append(string.Format(Texts.Instance.GetText("cardsHeal"), (object) aux.ToString()));
      builder.Append("\n");
      aux.Clear();
    }
    int num8 = 0;
    if (this.healSelf > 0 && (this.healSelfSpecialValue1 || this.healSelfSpecialValueGlobal))
    {
      aux.Append(this.ColorTextArray("heal", "X", this.SpriteText("heal")));
      num1 = num8 + 1;
    }
    if (aux.Length > 0)
    {
      builder.Append(string.Format(Texts.Instance.GetText("cardsHealSelf"), (object) aux.ToString()));
      builder.Append("\n");
      aux.Clear();
    }
    if (this.selfHealthLoss > 0 && this.selfHealthLossSpecialGlobal)
    {
      builder.Append(string.Format(Texts.Instance.GetText("cardsYouLose"), (object) this.ColorTextArray("damage", "X", "HP")));
      builder.Append("\n");
      aux.Clear();
    }
    int num9 = 0;
    if ((UnityEngine.Object) this.curseSelf != (UnityEngine.Object) null && this.curseCharges > 0)
    {
      ++num9;
      aux.Append(this.ColorTextArray("curse", this.NumFormat(this.GetFinalAuraCharges(this.curseSelf.Id, this.curseCharges, character)), this.SpriteText(this.curseSelf.ACName)));
    }
    if ((UnityEngine.Object) this.curseSelf2 != (UnityEngine.Object) null && this.curseCharges2 > 0)
    {
      ++num9;
      aux.Append(this.ColorTextArray("curse", this.NumFormat(this.GetFinalAuraCharges(this.curseSelf2.Id, this.curseCharges2, character)), this.SpriteText(this.curseSelf2.ACName)));
    }
    if ((UnityEngine.Object) this.curseSelf3 != (UnityEngine.Object) null && this.curseCharges3 > 0)
    {
      ++num9;
      aux.Append(this.ColorTextArray("curse", this.NumFormat(this.GetFinalAuraCharges(this.curseSelf3.Id, this.curseCharges3, character)), this.SpriteText(this.curseSelf3.ACName)));
    }
    if (num9 > 0)
    {
      if (num9 > 2)
      {
        aux.Insert(0, br1);
        aux.Insert(0, "\n");
      }
      if (this.targetSide == Enums.CardTargetSide.Self)
      {
        builder.Append(string.Format(Texts.Instance.GetText("cardsSuffer"), (object) aux.ToString()));
        builder.Append("\n");
      }
      else
      {
        stringBuilder1.Append(string.Format(Texts.Instance.GetText("cardsYouSuffer"), (object) aux.ToString()));
        stringBuilder1.Append("\n");
      }
      aux.Clear();
    }
    int num10 = 0;
    if (this.energyRecharge < 0)
    {
      builder.Append(string.Format(Texts.Instance.GetText("cardsLoseHp"), (object) aux.Append(this.ColorTextArray("system", this.NumFormat(Mathf.Abs(this.energyRecharge)), this.SpriteText("energy")))));
      builder.Append("\n");
      aux.Clear();
    }
    if (this.energyRecharge > 0)
    {
      ++num10;
      aux.Append(this.ColorTextArray("system", this.NumFormat(this.energyRecharge), this.SpriteText("energy")));
    }
    if ((UnityEngine.Object) this.aura != (UnityEngine.Object) null && this.auraCharges > 0 && (UnityEngine.Object) this.aura != (UnityEngine.Object) auraCurseData)
    {
      ++num10;
      aux.Append(this.ColorTextArray("aura", this.NumFormat(this.GetFinalAuraCharges(this.aura.Id, this.auraCharges, character)), this.SpriteText(this.aura.ACName)));
    }
    if ((UnityEngine.Object) this.aura2 != (UnityEngine.Object) null && this.auraCharges2 > 0 && (UnityEngine.Object) this.aura2 != (UnityEngine.Object) auraCurseData)
    {
      ++num10;
      aux.Append(this.ColorTextArray("aura", this.NumFormat(this.GetFinalAuraCharges(this.aura2.Id, this.auraCharges2, character)), this.SpriteText(this.aura2.ACName)));
    }
    if ((UnityEngine.Object) this.aura3 != (UnityEngine.Object) null && this.auraCharges3 > 0 && (UnityEngine.Object) this.aura3 != (UnityEngine.Object) auraCurseData)
    {
      ++num10;
      aux.Append(this.ColorTextArray("aura", this.NumFormat(this.GetFinalAuraCharges(this.aura3.Id, this.auraCharges3, character)), this.SpriteText(this.aura3.ACName)));
    }
    if (this.auras != null && this.auras.Length != 0)
    {
      foreach (CardData.AuraBuffs aura in this.auras)
      {
        if (aura != null && (UnityEngine.Object) aura.aura != (UnityEngine.Object) null && aura.auraCharges > 0 && (UnityEngine.Object) aura.aura != (UnityEngine.Object) auraCurseData)
        {
          ++num10;
          aux.Append(this.ColorTextArray("aura", this.NumFormat(this.GetFinalAuraCharges(aura.aura.Id, aura.auraCharges, character)), this.SpriteText(aura.aura.ACName)));
        }
      }
    }
    if (num10 > 0)
    {
      if (num10 > 2)
      {
        aux.Insert(0, br1);
        aux.Insert(0, "\n");
      }
      if (this.targetSide == Enums.CardTargetSide.Self)
      {
        if (this.ChooseOneOfAvailableAuras)
          builder.Append(string.Format(Texts.Instance.GetText("cardsGainOneOf"), (object) aux.ToString()));
        else
          builder.Append(string.Format(Texts.Instance.GetText("cardsGain"), (object) aux.ToString()));
      }
      else if (this.ChooseOneOfAvailableAuras)
        builder.Append(string.Format(Texts.Instance.GetText("cardsGrantOneOf"), (object) aux.ToString()));
      else
        builder.Append(string.Format(Texts.Instance.GetText("cardsGrant"), (object) aux.ToString()));
      builder.Append("\n");
      aux.Clear();
    }
    int num11 = 0;
    if ((UnityEngine.Object) this.auraSelf != (UnityEngine.Object) null && this.auraCharges > 0)
    {
      ++num11;
      aux.Append(this.ColorTextArray("aura", this.NumFormat(this.GetFinalAuraCharges(this.auraSelf.Id, this.auraCharges, character)), this.SpriteText(this.auraSelf.ACName)));
    }
    if ((UnityEngine.Object) this.auraSelf2 != (UnityEngine.Object) null && this.auraCharges2 > 0)
    {
      ++num11;
      aux.Append(this.ColorTextArray("aura", this.NumFormat(this.GetFinalAuraCharges(this.auraSelf2.Id, this.auraCharges2, character)), this.SpriteText(this.auraSelf2.ACName)));
    }
    if ((UnityEngine.Object) this.auraSelf3 != (UnityEngine.Object) null && this.auraCharges3 > 0)
    {
      ++num11;
      aux.Append(this.ColorTextArray("aura", this.NumFormat(this.GetFinalAuraCharges(this.auraSelf3.Id, this.auraCharges3, character)), this.SpriteText(this.auraSelf3.ACName)));
    }
    if (this.auras != null && this.auras.Length != 0)
    {
      foreach (CardData.AuraBuffs aura in this.auras)
      {
        if (aura != null && (UnityEngine.Object) aura.auraSelf != (UnityEngine.Object) null && aura.auraCharges > 0)
        {
          ++num11;
          aux.Append(this.ColorTextArray("aura", this.NumFormat(this.GetFinalAuraCharges(aura.auraSelf.Id, aura.auraCharges, character)), this.SpriteText(aura.auraSelf.ACName)));
        }
      }
    }
    if (!flag1)
    {
      if ((UnityEngine.Object) this.auraSelf != (UnityEngine.Object) null && (this.auraChargesSpecialValue1 || this.auraChargesSpecialValueGlobal))
      {
        ++num11;
        aux.Append(this.ColorTextArray("aura", "X", this.SpriteText(this.auraSelf.ACName)));
      }
      if ((UnityEngine.Object) this.auraSelf2 != (UnityEngine.Object) null && (this.auraCharges2SpecialValue1 || this.auraCharges2SpecialValueGlobal))
      {
        ++num11;
        aux.Append(this.ColorTextArray("aura", "X", this.SpriteText(this.auraSelf2.ACName)));
      }
      if ((UnityEngine.Object) this.auraSelf3 != (UnityEngine.Object) null && (this.auraCharges3SpecialValue1 || this.auraCharges3SpecialValueGlobal))
      {
        ++num11;
        aux.Append(this.ColorTextArray("aura", "X", this.SpriteText(this.auraSelf3.ACName)));
      }
    }
    if (num11 > 0)
    {
      if (num11 > 2)
      {
        aux.Insert(0, br1);
        aux.Insert(0, "\n");
      }
      if (this.targetSide == Enums.CardTargetSide.Self)
      {
        builder.Append(string.Format(Texts.Instance.GetText("cardsGain"), (object) aux.ToString()));
        builder.Append("\n");
      }
      else
      {
        stringBuilder1.Append(string.Format(Texts.Instance.GetText("cardsYouGain"), (object) aux.ToString()));
        stringBuilder1.Append("\n");
      }
      aux.Clear();
    }
    if (!flag1 && !flag2)
    {
      if ((double) this.specialValueModifierGlobal > 0.0)
        num2 = this.specialValueModifierGlobal / 100f;
      else if ((double) this.specialValueModifier1 > 0.0)
        num2 = this.specialValueModifier1 / 100f;
      if ((double) num2 > 0.0 && (double) num2 != 1.0)
        str4 = "x" + num2.ToString();
      if (str4 == "")
        str4 = "<space=-.1>";
      if (this.specialValue1 == Enums.CardSpecialValue.AuraCurseYours)
        aux.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYour"), (object) this.SpriteText(this.SpecialAuraCurseName1.ACName), (object) str4));
      else if (this.specialValue1 == Enums.CardSpecialValue.AuraCurseTarget)
        aux.Append(string.Format(Texts.Instance.GetText("cardsXEqualsTarget"), (object) this.SpriteText(this.SpecialAuraCurseName1.ACName), (object) str4));
      else if (this.specialValueGlobal == Enums.CardSpecialValue.AuraCurseYours)
        aux.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYour"), (object) this.SpriteText(this.SpecialAuraCurseNameGlobal.ACName), (object) str4));
      else if (this.specialValueGlobal == Enums.CardSpecialValue.AuraCurseTarget)
        aux.Append(string.Format(Texts.Instance.GetText("cardsXEqualsTarget"), (object) this.SpriteText(this.SpecialAuraCurseNameGlobal.ACName), (object) str4));
      if (this.specialValueGlobal == Enums.CardSpecialValue.HealthYours)
        aux.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYourHp"), (object) str4));
      else if (this.specialValue1 == Enums.CardSpecialValue.HealthYours)
        aux.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYourHp"), (object) str4));
      else if (this.specialValue1 == Enums.CardSpecialValue.HealthTarget)
        aux.Append(string.Format(Texts.Instance.GetText("cardsXEqualsTargetHp"), (object) str4));
      if (this.specialValueGlobal == Enums.CardSpecialValue.SpeedYours)
        aux.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYourSpeed"), (object) str4));
      else if (this.specialValueGlobal == Enums.CardSpecialValue.SpeedTarget)
        aux.Append(string.Format(Texts.Instance.GetText("cardsXEqualsTargetSpeed"), (object) str4));
      else if (this.specialValueGlobal == Enums.CardSpecialValue.SpeedDifference || this.specialValue1 == Enums.CardSpecialValue.SpeedDifference)
        aux.Append(string.Format(Texts.Instance.GetText("cardsXEqualsDifferenceSpeed"), (object) str4));
      else if (this.specialValueGlobal == Enums.CardSpecialValue.DiscardedCards)
      {
        if (this.discardCardPlace == Enums.CardPlace.Vanish)
          aux.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYourVanishedCards"), (object) str4));
        else
          aux.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYourDiscardedCards"), (object) str4));
      }
      else if (this.specialValueGlobal == Enums.CardSpecialValue.VanishedCards)
        aux.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYourVanishedCards"), (object) str4));
      if (this.specialValueGlobal == Enums.CardSpecialValue.CardsHand || this.specialValue1 == Enums.CardSpecialValue.CardsHand || this.specialValue2 == Enums.CardSpecialValue.CardsHand)
        aux.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYourHand"), (object) this.SpriteText("card"), (object) str4));
      else if (this.specialValueGlobal == Enums.CardSpecialValue.CardsDeck || this.specialValue1 == Enums.CardSpecialValue.CardsDeck || this.specialValue2 == Enums.CardSpecialValue.CardsDeck)
        aux.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYourDeck"), (object) this.SpriteText("card"), (object) str4));
      else if (this.specialValueGlobal == Enums.CardSpecialValue.CardsDiscard || this.specialValue1 == Enums.CardSpecialValue.CardsDiscard || this.specialValue2 == Enums.CardSpecialValue.CardsDiscard)
        aux.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYourDiscard"), (object) this.SpriteText("card"), (object) str4));
      else if (this.specialValueGlobal == Enums.CardSpecialValue.CardsVanish || this.specialValue1 == Enums.CardSpecialValue.CardsVanish || this.specialValue2 == Enums.CardSpecialValue.CardsVanish)
        aux.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYourVanish"), (object) this.SpriteText("card"), (object) str4));
      else if (this.specialValueGlobal == Enums.CardSpecialValue.CardsDeckTarget || this.specialValue1 == Enums.CardSpecialValue.CardsDeckTarget || this.specialValue2 == Enums.CardSpecialValue.CardsDeckTarget)
        aux.Append(string.Format(Texts.Instance.GetText("cardsXEqualsTargetDeck"), (object) this.SpriteText("card"), (object) str4));
      else if (this.specialValueGlobal == Enums.CardSpecialValue.CardsDiscardTarget || this.specialValue1 == Enums.CardSpecialValue.CardsDiscardTarget || this.specialValue2 == Enums.CardSpecialValue.CardsDiscardTarget)
        aux.Append(string.Format(Texts.Instance.GetText("cardsXEqualsTargetDiscard"), (object) this.SpriteText("card"), (object) str4));
      else if (this.specialValueGlobal == Enums.CardSpecialValue.CardsVanishTarget || this.specialValue1 == Enums.CardSpecialValue.CardsVanishTarget || this.specialValue2 == Enums.CardSpecialValue.CardsVanishTarget)
        aux.Append(string.Format(Texts.Instance.GetText("cardsXEqualsTargetVanish"), (object) this.SpriteText("card"), (object) str4));
      else if (this.specialValueGlobal == Enums.CardSpecialValue.MissingHealthYours || this.specialValue1 == Enums.CardSpecialValue.MissingHealthYours)
        aux.Append(string.Format(Texts.Instance.GetText("cardsXEqualsYourMissingHealth"), (object) str4));
      else if (this.specialValueGlobal == Enums.CardSpecialValue.MissingHealthTarget || this.specialValue1 == Enums.CardSpecialValue.MissingHealthTarget)
        aux.Append(string.Format(Texts.Instance.GetText("cardsXEqualsTargetMissingHealth"), (object) str4));
      if (aux.Length > 0)
      {
        builder.Append(goldColor);
        builder.Append(aux);
        builder.Append(endColor);
        builder.Append("\n");
        aux.Clear();
        if ((UnityEngine.Object) this.healAuraCurseName != (UnityEngine.Object) null)
        {
          if (this.targetSide == Enums.CardTargetSide.Self)
          {
            if (this.healAuraCurseName.IsAura)
              builder.Append(string.Format(Texts.Instance.GetText("cardsPurgeYour"), (object) this.SpriteText(this.healAuraCurseName.ACName)));
            else
              builder.Append(string.Format(Texts.Instance.GetText("cardsDispelYour"), (object) this.SpriteText(this.healAuraCurseName.ACName)));
          }
          else if (this.healAuraCurseName.IsAura)
            builder.Append(string.Format(Texts.Instance.GetText("cardsPurge"), (object) this.SpriteText(this.healAuraCurseName.ACName)));
          else
            builder.Append(string.Format(Texts.Instance.GetText("cardsDispel"), (object) this.SpriteText(this.healAuraCurseName.ACName)));
          builder.Append("\n");
        }
        if ((UnityEngine.Object) this.healAuraCurseSelf != (UnityEngine.Object) null)
        {
          if (this.healAuraCurseSelf.IsAura)
            builder.Append(string.Format(Texts.Instance.GetText("cardsPurgeYour"), (object) this.SpriteText(this.healAuraCurseSelf.ACName)));
          else
            builder.Append(string.Format(Texts.Instance.GetText("cardsDispelYour"), (object) this.SpriteText(this.healAuraCurseSelf.ACName)));
          builder.Append("\n");
        }
      }
      num2 = 0.0f;
    }
    int num12 = 0;
    if (this.curseCharges > 0 && (UnityEngine.Object) this.curse != (UnityEngine.Object) null)
    {
      ++num12;
      aux.Append(this.ColorTextArray("curse", this.NumFormat(this.GetFinalAuraCharges(this.curse.Id, this.curseCharges, character)), this.SpriteText(this.curse.ACName)));
    }
    if (this.curseCharges2 > 0 && (UnityEngine.Object) this.curse2 != (UnityEngine.Object) null)
    {
      ++num12;
      aux.Append(this.ColorTextArray("curse", this.NumFormat(this.GetFinalAuraCharges(this.curse2.Id, this.curseCharges2, character)), this.SpriteText(this.curse2.ACName)));
    }
    if (this.curseCharges3 > 0 && (UnityEngine.Object) this.curse3 != (UnityEngine.Object) null)
    {
      ++num12;
      aux.Append(this.ColorTextArray("curse", this.NumFormat(this.GetFinalAuraCharges(this.curse3.Id, this.curseCharges3, character)), this.SpriteText(this.curse3.ACName)));
    }
    if (num12 > 0)
    {
      if (num12 > 2)
      {
        aux.Insert(0, br1);
        aux.Insert(0, "\n");
      }
      if (this.targetSide == Enums.CardTargetSide.Self)
        builder.Append(string.Format(Texts.Instance.GetText("cardsSuffer"), (object) aux.ToString()));
      else
        builder.Append(string.Format(Texts.Instance.GetText("cardsApply"), (object) aux.ToString()));
      builder.Append("\n");
      aux.Clear();
    }
    if (this.curseChargesSides > 0)
    {
      builder.Append(string.Format(Texts.Instance.GetText("cardsTargetSides"), (object) this.ColorTextArray("curse", this.NumFormat(this.GetFinalAuraCharges(this.curse.Id, this.curseChargesSides, character)), this.SpriteText(this.curse.ACName))));
      builder.Append("\n");
      aux.Clear();
    }
    if (this.heal > 0 && !this.healSpecialValue1 && !this.healSpecialValueGlobal)
    {
      aux.Append(this.ColorTextArray("heal", this.NumFormat(this.healPreCalculated), this.SpriteText("heal")));
      builder.Append(string.Format(Texts.Instance.GetText("cardsHeal"), (object) aux.ToString()));
      builder.Append("\n");
      aux.Clear();
    }
    if (this.healSelf > 0 && !this.healSelfSpecialValue1 && !this.healSelfSpecialValueGlobal)
    {
      aux.Append(this.ColorTextArray("heal", this.NumFormat(this.healSelfPreCalculated), this.SpriteText("heal")));
      builder.Append(string.Format(Texts.Instance.GetText("cardsHealSelf"), (object) aux.ToString()));
      builder.Append("\n");
      aux.Clear();
    }
    if (this.damageSides > 0)
    {
      if (this.damageSpecialValueGlobal)
        aux.Append(this.ColorTextArray("damage", "X", this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) this.damageType))));
      else
        aux.Append(this.ColorTextArray("damage", this.NumFormat(this.damageSidesPreCalculated), this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) this.damageType))));
    }
    if (this.damageSides2 > 0)
    {
      if (this.damage2SpecialValueGlobal)
        aux.Append(this.ColorTextArray("damage", "X", this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) this.damageType2))));
      else
        aux.Append(this.ColorTextArray("damage", this.NumFormat(this.damageSidesPreCalculated2), this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) this.damageType2))));
    }
    if (aux.Length > 0)
    {
      builder.Append(string.Format(Texts.Instance.GetText("cardsTargetSides"), (object) aux.ToString()));
      builder.Append("\n");
      aux.Clear();
    }
    if (stringBuilder1.Length > 0)
      builder.Append(stringBuilder1.ToString());
    if (this.killPet)
    {
      builder.Append(Texts.Instance.GetText("killPet"));
      builder.Append("\n");
    }
    if (this.damageEnergyBonus > 0 || this.healEnergyBonus > 0 || (UnityEngine.Object) this.acEnergyBonus != (UnityEngine.Object) null)
    {
      StringBuilder stringBuilder6 = new StringBuilder();
      stringBuilder6.Append("<line-height=30%><br></line-height>");
      StringBuilder stringBuilder7 = new StringBuilder();
      stringBuilder7.Append(grColor);
      stringBuilder7.Append("[");
      stringBuilder7.Append(Texts.Instance.GetText("overchargeAcronym"));
      stringBuilder7.Append("]");
      stringBuilder7.Append(endColor);
      stringBuilder7.Append("  ");
      if (this.damageEnergyBonus > 0)
      {
        stringBuilder6.Append(stringBuilder7.ToString());
        stringBuilder6.Append(string.Format(Texts.Instance.GetText("cardsDealDamage"), (object) this.ColorTextArray("damage", this.NumFormat(this.damageEnergyBonus), this.SpriteText(Enum.GetName(typeof (Enums.DamageType), (object) this.damageType)))));
        stringBuilder6.Append("\n");
      }
      if ((UnityEngine.Object) this.acEnergyBonus != (UnityEngine.Object) null)
      {
        aux.Append(this.ColorTextArray("aura", this.NumFormat(this.acEnergyBonusQuantity), this.SpriteText(this.acEnergyBonus.ACName)));
        if ((UnityEngine.Object) this.acEnergyBonus2 != (UnityEngine.Object) null)
        {
          aux.Append(" ");
          aux.Append(this.ColorTextArray("aura", this.NumFormat(this.acEnergyBonus2Quantity), this.SpriteText(this.acEnergyBonus2.ACName)));
        }
        if (this.acEnergyBonus.IsAura)
        {
          if (this.targetSide == Enums.CardTargetSide.Self)
          {
            stringBuilder6.Append(stringBuilder7.ToString());
            stringBuilder6.Append(string.Format(Texts.Instance.GetText("cardsGain"), (object) aux.ToString()));
          }
          else
          {
            stringBuilder6.Append(stringBuilder7.ToString());
            stringBuilder6.Append(string.Format(Texts.Instance.GetText("cardsGrant"), (object) aux.ToString()));
          }
        }
        else if (!this.acEnergyBonus.IsAura)
        {
          if (this.targetSide == Enums.CardTargetSide.Self)
          {
            stringBuilder6.Append(stringBuilder7.ToString());
            stringBuilder6.Append(string.Format(Texts.Instance.GetText("cardsSuffer"), (object) aux.ToString()));
          }
          else
          {
            stringBuilder6.Append(stringBuilder7.ToString());
            stringBuilder6.Append(string.Format(Texts.Instance.GetText("cardsApply"), (object) aux.ToString()));
          }
        }
        stringBuilder6.Append("\n");
      }
      if (this.healEnergyBonus > 0)
      {
        stringBuilder6.Append(stringBuilder7.ToString());
        stringBuilder6.Append(string.Format(Texts.Instance.GetText("cardsHeal"), (object) this.ColorTextArray("heal", this.NumFormat(this.healEnergyBonus), this.SpriteText("heal"))));
        stringBuilder6.Append("\n");
      }
      builder.Append(stringBuilder6.ToString());
      aux.Clear();
      stringBuilder6.Clear();
    }
    if (this.effectRepeat > 1 || this.effectRepeatMaxBonus > 0)
    {
      builder.Append(br1);
      builder.Append("<nobr><size=-.05><color=#1A505A>- ");
      if (this.effectRepeatMaxBonus > 0)
        builder.Append(string.Format(Texts.Instance.GetText("cardsRepeatUpTo"), (object) this.effectRepeatMaxBonus));
      else if (this.effectRepeatTarget == Enums.EffectRepeatTarget.Chain)
        builder.Append(string.Format(Texts.Instance.GetText("cardsRepeatChain"), (object) (this.effectRepeat - 1)));
      else if (this.effectRepeatTarget == Enums.EffectRepeatTarget.NoRepeat)
        builder.Append(string.Format(Texts.Instance.GetText("cardsRepeatJump"), (object) (this.effectRepeat - 1)));
      else
        builder.Append(string.Format(Texts.Instance.GetText("cardsRepeat"), (object) (this.effectRepeat - 1)));
      if (this.effectRepeatModificator != 0 && this.effectRepeatTarget != Enums.EffectRepeatTarget.Chain)
      {
        builder.Append(" (");
        if (this.effectRepeatModificator > 0)
          builder.Append("+");
        builder.Append(this.effectRepeatModificator);
        if (Functions.SpaceBeforePercentSign())
          builder.Append(" ");
        builder.Append("%)");
      }
      builder.Append(" -</color></size></nobr>");
      builder.Append("\n");
      aux.Clear();
    }
    if (this.ignoreBlock || this.ignoreBlock2)
    {
      builder.Append(br1);
      builder.Append(grColor);
      builder.Append(Texts.Instance.GetText("cardsIgnoreBlock"));
      builder.Append(endColor);
      builder.Append("\n");
      aux.Clear();
    }
    if (this.goldGainQuantity != 0 && this.shardsGainQuantity != 0)
    {
      builder.Append(string.Format(Texts.Instance.GetText("customGainPerHeroAnd"), (object) this.ColorTextArray("aura", this.goldGainQuantity.ToString(), this.SpriteText("gold")), (object) this.ColorTextArray("aura", this.shardsGainQuantity.ToString(), this.SpriteText("dust"))));
      builder.Append("\n");
    }
    else if (this.goldGainQuantity != 0)
    {
      builder.Append(string.Format(Texts.Instance.GetText("customGainPerHero"), (object) this.ColorTextArray("aura", this.goldGainQuantity.ToString(), this.SpriteText("gold"))));
      builder.Append("\n");
    }
    else if (this.shardsGainQuantity != 0)
    {
      builder.Append(string.Format(Texts.Instance.GetText("customGainPerHero"), (object) this.ColorTextArray("aura", this.shardsGainQuantity.ToString(), this.SpriteText("dust"))));
      builder.Append("\n");
    }
    if ((double) this.selfKillHiddenSeconds > 0.0)
    {
      builder.Append(Texts.Instance.GetText("escapes"));
      builder.Append("\n");
    }
  }

  public string SpecialModifierDescription(SpecialValues specialValues, ItemData itemData = null)
  {
    string str = "";
    switch (specialValues.Name)
    {
      case Enums.SpecialValueModifierName.RuneCharges:
        str = this.SpriteText("runered") + this.SpriteText("runeblue") + this.SpriteText("runegreen");
        break;
      case Enums.SpecialValueModifierName.DamageDealt:
        str = "Damage Dealt";
        break;
      case Enums.SpecialValueModifierName.AuracurseSetCharges:
      case Enums.SpecialValueModifierName.AuraCurseYours:
        str = this.SpriteText(itemData.AuraCurseSetted.Id);
        break;
    }
    return str;
  }

  private string NumFormatItem(int num, bool plus = false, bool percent = false)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(" <nobr>");
    if (num > 0)
    {
      stringBuilder.Append("<color=#263ABC><size=+.1>");
      if (plus)
        stringBuilder.Append("+");
    }
    else
    {
      stringBuilder.Append("<color=#720070><size=+.1>");
      if (plus)
        stringBuilder.Append("-");
    }
    stringBuilder.Append(Mathf.Abs(num));
    if (percent)
    {
      if (Functions.SpaceBeforePercentSign())
        stringBuilder.Append(" ");
      stringBuilder.Append("%");
    }
    stringBuilder.Append("</color></size></nobr>");
    return stringBuilder.ToString();
  }

  private string NumFormat(int num)
  {
    if (num < 0)
      num = 0;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("<size=+.1>");
    stringBuilder.Append(num);
    stringBuilder.Append("</size>");
    return stringBuilder.ToString();
  }

  public List<Enums.CardType> GetCardTypes()
  {
    if (this.cardTypeList == null)
    {
      this.cardTypeList = new List<Enums.CardType>();
      if (this.cardType != Enums.CardType.None)
      {
        this.cardTypeList.Add(this.cardType);
        for (int index = 0; index < this.cardTypeAux.Length; ++index)
        {
          if (this.cardTypeAux[index] != Enums.CardType.None)
            this.cardTypeList.Add(this.cardTypeAux[index]);
        }
      }
    }
    return this.cardTypeList;
  }

  public bool HasCardType(Enums.CardType type)
  {
    if (this.cardType == type)
      return true;
    for (int index = 0; index < this.cardTypeAux.Length; ++index)
    {
      if (this.cardTypeAux[index] == type)
        return true;
    }
    return false;
  }

  public void DoExhaust()
  {
    if (!(bool) (UnityEngine.Object) MatchManager.Instance)
      return;
    Hero heroHeroActive = MatchManager.Instance.GetHeroHeroActive();
    if (heroHeroActive == null)
      return;
    this.exhaustCounter += heroHeroActive.GetAuraCharges("Exhaust");
  }

  public bool GetIgnoreBlockBecausePurge() => this.IsGoingToPurgeThisAC("block");

  public bool IsGoingToPurgeThisAC(string acId)
  {
    acId = acId.ToLower();
    return this.HealAuraCurseName?.Id == acId || this.HealAuraCurseName2?.Id == acId || this.HealAuraCurseName3?.Id == acId || this.HealAuraCurseName4?.Id == acId;
  }

  public bool GetIgnoreBlock(int _index = 0)
  {
    if ((bool) (UnityEngine.Object) MatchManager.Instance)
    {
      Hero heroHeroActive = MatchManager.Instance.GetHeroHeroActive();
      if (heroHeroActive != null && heroHeroActive.SubclassName == "archer" && AtOManager.Instance.CharacterHaveTrait("archer", "perforatingshots") && this.HasCardType(Enums.CardType.Ranged_Attack))
        return true;
    }
    if (_index == 0)
      return this.ignoreBlock;
    return _index == 1 && this.ignoreBlock2;
  }

  public void ResetExhaust() => this.exhaustCounter = 0;

  public int GetCardFinalCost()
  {
    int cardFinalCost = this.EnergyCostOriginal;
    if ((bool) (UnityEngine.Object) MatchManager.Instance)
    {
      if (this.EnergyReductionToZeroPermanent || this.EnergyReductionToZeroTemporal)
        cardFinalCost = 0;
      int num = cardFinalCost - this.EnergyReductionPermanent - this.EnergyReductionTemporal;
      cardFinalCost = this.CardClass == Enums.CardClass.Special && !this.Playable || this.CardClass == Enums.CardClass.Boon || this.CardClass == Enums.CardClass.Injury || this.CardClass == Enums.CardClass.Monster && !this.Playable ? 0 : num + this.ExhaustCounter;
      if (cardFinalCost < 0)
        cardFinalCost = 0;
    }
    return cardFinalCost;
  }

  public string ColorFromCardDataRarity(CardData _cData = null, bool _useLightVersion = false)
  {
    if ((UnityEngine.Object) _cData == (UnityEngine.Object) null)
      _cData = this;
    if (!((UnityEngine.Object) _cData != (UnityEngine.Object) null))
      return "<color=#5E3016>";
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("<color=#");
    if (_cData.cardUpgraded == Enums.CardUpgraded.A)
    {
      if (!_useLightVersion)
        stringBuilder.Append(this.colorUpgradeBlue);
      else
        stringBuilder.Append(this.colorUpgradeBlueLight);
    }
    else if (_cData.cardUpgraded == Enums.CardUpgraded.B)
    {
      if (!_useLightVersion)
        stringBuilder.Append(this.colorUpgradeGold);
      else
        stringBuilder.Append(this.colorUpgradeGoldLight);
    }
    else if (_cData.cardUpgraded == Enums.CardUpgraded.Rare)
    {
      if (!_useLightVersion)
        stringBuilder.Append(this.colorUpgradeRare);
      else
        stringBuilder.Append(this.colorUpgradeRareLight);
    }
    else if (!_useLightVersion)
      stringBuilder.Append(this.colorUpgradePlain);
    else
      stringBuilder.Append(this.colorUpgradePlainLight);
    stringBuilder.Append(">");
    return stringBuilder.ToString();
  }

  public AudioClip GetSoundRelease(Hero hero, NPC npc)
  {
    bool flag = GameManager.Instance.ConfigUseLegacySounds;
    if (!flag && (UnityEngine.Object) this.soundHitRework == (UnityEngine.Object) null && (UnityEngine.Object) this.soundReleaseRework == (UnityEngine.Object) null)
      flag = true;
    if (flag)
    {
      if (hero != null)
      {
        if (hero.HeroData.HeroSubClass.Female && (UnityEngine.Object) this.soundPreActionFemale != (UnityEngine.Object) null)
          return this.soundPreActionFemale;
      }
      else if (npc != null && npc.NpcData.Female && (UnityEngine.Object) this.soundPreActionFemale != (UnityEngine.Object) null)
        return this.soundPreActionFemale;
      return this.soundPreAction;
    }
    if (hero != null)
    {
      if (this.srException2Class != null)
      {
        for (int index = 0; index < this.srException2Class.Count; ++index)
        {
          if ((UnityEngine.Object) this.srException2Class[index] != (UnityEngine.Object) null && this.srException2Class[index].Id == hero.HeroData.HeroSubClass.Id)
            return this.srException2Audio;
        }
      }
      if (this.srException1Class != null)
      {
        for (int index = 0; index < this.srException1Class.Count; ++index)
        {
          if ((UnityEngine.Object) this.srException1Class[index] != (UnityEngine.Object) null && this.srException1Class[index].Id == hero.HeroData.HeroSubClass.Id)
            return this.srException1Audio;
        }
      }
      if (this.srException0Class != null)
      {
        for (int index = 0; index < this.srException0Class.Count; ++index)
        {
          if ((UnityEngine.Object) this.srException0Class[index] != (UnityEngine.Object) null && this.srException0Class[index].Id == hero.HeroData.HeroSubClass.Id)
            return this.srException0Audio;
        }
      }
    }
    return this.soundReleaseRework;
  }

  public AudioClip GetSoundHit()
  {
    bool flag = GameManager.Instance.ConfigUseLegacySounds;
    if (!flag && (UnityEngine.Object) this.soundHitRework == (UnityEngine.Object) null && (UnityEngine.Object) this.soundReleaseRework == (UnityEngine.Object) null)
      flag = true;
    return flag ? this.sound : this.soundHitRework;
  }

  public AudioClip GetSoundDrag()
  {
    if (GameManager.Instance.ConfigUseLegacySounds)
      return (AudioClip) null;
    return (UnityEngine.Object) this.soundDragRework != (UnityEngine.Object) null ? this.soundDragRework : AudioManager.Instance.soundCardDrag;
  }

  public string CardName
  {
    get => this.cardName;
    set => this.cardName = value;
  }

  public string Id
  {
    get => this.id;
    set => this.id = value;
  }

  public string InternalId
  {
    get => this.internalId;
    set => this.internalId = value;
  }

  public bool Visible
  {
    get => this.visible;
    set => this.visible = value;
  }

  public string UpgradesTo1
  {
    get => this.upgradesTo1;
    set => this.upgradesTo1 = value;
  }

  public string UpgradesTo2
  {
    get => this.upgradesTo2;
    set => this.upgradesTo2 = value;
  }

  public Enums.CardUpgraded CardUpgraded
  {
    get => this.cardUpgraded;
    set => this.cardUpgraded = value;
  }

  public string UpgradedFrom
  {
    get => this.upgradedFrom;
    set => this.upgradedFrom = value;
  }

  public string BaseCard
  {
    get => this.baseCard;
    set => this.baseCard = value;
  }

  public int CardNumber
  {
    get => this.cardNumber;
    set => this.cardNumber = value;
  }

  public string Description
  {
    get => this.description;
    set => this.description = value;
  }

  public string Fluff
  {
    get => this.fluff;
    set => this.fluff = value;
  }

  public string DescriptionNormalized
  {
    get => this.descriptionNormalized;
    set => this.descriptionNormalized = value;
  }

  public List<KeyNotesData> KeyNotes
  {
    get => this.keyNotes;
    set => this.keyNotes = value;
  }

  public Sprite Sprite
  {
    get => this.sprite;
    set => this.sprite = value;
  }

  public AudioClip Sound
  {
    get => this.sound;
    set => this.sound = value;
  }

  public AudioClip SoundPreAction
  {
    get => this.soundPreAction;
    set => this.soundPreAction = value;
  }

  public string EffectPreAction
  {
    get => this.effectPreAction;
    set => this.effectPreAction = value;
  }

  public string EffectCaster
  {
    get => this.effectCaster;
    set => this.effectCaster = value;
  }

  public float EffectPostCastDelay
  {
    get => this.effectPostCastDelay;
    set => this.effectPostCastDelay = value;
  }

  public bool EffectCasterRepeat
  {
    get => this.effectCasterRepeat;
    set => this.effectCasterRepeat = value;
  }

  public bool EffectCastCenter
  {
    get => this.effectCastCenter;
    set => this.effectCastCenter = value;
  }

  public string EffectTrail
  {
    get => this.effectTrail;
    set => this.effectTrail = value;
  }

  public bool EffectTrailRepeat
  {
    get => this.effectTrailRepeat;
    set => this.effectTrailRepeat = value;
  }

  public float EffectTrailSpeed
  {
    get => this.effectTrailSpeed;
    set => this.effectTrailSpeed = value;
  }

  public Enums.EffectTrailAngle EffectTrailAngle
  {
    get => this.effectTrailAngle;
    set => this.effectTrailAngle = value;
  }

  public string EffectTarget
  {
    get => this.effectTarget;
    set => this.effectTarget = value;
  }

  public int MaxInDeck
  {
    get => this.maxInDeck;
    set => this.maxInDeck = value;
  }

  public Enums.CardRarity CardRarity
  {
    get => this.cardRarity;
    set => this.cardRarity = value;
  }

  public Enums.CardType CardType
  {
    get => this.cardType;
    set => this.cardType = value;
  }

  public Enums.CardType[] CardTypeAux
  {
    get => this.cardTypeAux;
    set => this.cardTypeAux = value;
  }

  public Enums.CardClass CardClass
  {
    get => this.cardClass;
    set => this.cardClass = value;
  }

  public int EnergyCost
  {
    get => this.energyCost;
    set => this.energyCost = value;
  }

  public int EnergyCostOriginal
  {
    get => this.energyCostOriginal;
    set => this.energyCostOriginal = value;
  }

  public int EnergyCostForShow
  {
    get => this.energyCostForShow;
    set => this.energyCostForShow = value;
  }

  public bool Playable
  {
    get => this.playable;
    set => this.playable = value;
  }

  public bool AutoplayDraw
  {
    get => this.autoplayDraw;
    set => this.autoplayDraw = value;
  }

  public bool AutoplayEndTurn
  {
    get => this.autoplayEndTurn;
    set => this.autoplayEndTurn = value;
  }

  public Enums.CardTargetType TargetType
  {
    get => this.targetType;
    set => this.targetType = value;
  }

  public Enums.CardTargetSide TargetSide
  {
    get => this.targetSide;
    set => this.targetSide = value;
  }

  public Enums.CardTargetPosition TargetPosition
  {
    get => this.targetPosition;
    set => this.targetPosition = value;
  }

  public string EffectRequired
  {
    get => this.effectRequired;
    set => this.effectRequired = value;
  }

  public int EffectRepeat
  {
    get => this.effectRepeat;
    set => this.effectRepeat = value;
  }

  public float EffectRepeatDelay
  {
    get => this.effectRepeatDelay;
    set => this.effectRepeatDelay = value;
  }

  public int EffectRepeatEnergyBonus
  {
    get => this.effectRepeatEnergyBonus;
    set => this.effectRepeatEnergyBonus = value;
  }

  public int EffectRepeatMaxBonus
  {
    get => this.effectRepeatMaxBonus;
    set => this.effectRepeatMaxBonus = value;
  }

  public Enums.EffectRepeatTarget EffectRepeatTarget
  {
    get => this.effectRepeatTarget;
    set => this.effectRepeatTarget = value;
  }

  public int EffectRepeatModificator
  {
    get => this.effectRepeatModificator;
    set => this.effectRepeatModificator = value;
  }

  public Enums.DamageType DamageType
  {
    get => this.damageType;
    set => this.damageType = value;
  }

  public int Damage
  {
    get => this.damage;
    set => this.damage = value;
  }

  public int DamagePreCalculated
  {
    get => this.damagePreCalculated;
    set => this.damagePreCalculated = value;
  }

  public int DamageSides
  {
    get => this.damageSides;
    set => this.damageSides = value;
  }

  public int DamageSidesPreCalculated
  {
    get => this.damageSidesPreCalculated;
    set => this.damageSidesPreCalculated = value;
  }

  public int DamageSelf
  {
    get => this.damageSelf;
    set => this.damageSelf = value;
  }

  public int DamageSelfPreCalculated
  {
    get => this.damageSelfPreCalculated;
    set => this.damageSelfPreCalculated = value;
  }

  public int DamageSelfPreCalculated2
  {
    get => this.damageSelfPreCalculated2;
    set => this.damageSelfPreCalculated2 = value;
  }

  public bool IgnoreBlock
  {
    get => this.ignoreBlock;
    set => this.ignoreBlock = value;
  }

  public Enums.DamageType DamageType2
  {
    get => this.damageType2;
    set => this.damageType2 = value;
  }

  public int Damage2
  {
    get => this.damage2;
    set => this.damage2 = value;
  }

  public int DamagePreCalculated2
  {
    get => this.damagePreCalculated2;
    set => this.damagePreCalculated2 = value;
  }

  public int DamageSides2
  {
    get => this.damageSides2;
    set => this.damageSides2 = value;
  }

  public int DamageSidesPreCalculated2
  {
    get => this.damageSidesPreCalculated2;
    set => this.damageSidesPreCalculated2 = value;
  }

  public int DamageSelf2
  {
    get => this.damageSelf2;
    set => this.damageSelf2 = value;
  }

  public bool IgnoreBlock2
  {
    get => this.ignoreBlock2;
    set => this.ignoreBlock2 = value;
  }

  public int SelfHealthLoss
  {
    get => this.selfHealthLoss;
    set => this.selfHealthLoss = value;
  }

  public int DamageEnergyBonus
  {
    get => this.damageEnergyBonus;
    set => this.damageEnergyBonus = value;
  }

  public int Heal
  {
    get => this.heal;
    set => this.heal = value;
  }

  public int HealSides
  {
    get => this.healSides;
    set => this.healSides = value;
  }

  public int HealSelf
  {
    get => this.healSelf;
    set => this.healSelf = value;
  }

  public int HealEnergyBonus
  {
    get => this.healEnergyBonus;
    set => this.healEnergyBonus = value;
  }

  public float HealSelfPerDamageDonePercent
  {
    get => this.healSelfPerDamageDonePercent;
    set => this.healSelfPerDamageDonePercent = value;
  }

  public int HealCurses
  {
    get => this.healCurses;
    set => this.healCurses = value;
  }

  public AuraCurseData HealAuraCurseSelf
  {
    get => this.healAuraCurseSelf;
    set => this.healAuraCurseSelf = value;
  }

  public AuraCurseData HealAuraCurseName
  {
    get => this.healAuraCurseName;
    set => this.healAuraCurseName = value;
  }

  public AuraCurseData HealAuraCurseName2
  {
    get => this.healAuraCurseName2;
    set => this.healAuraCurseName2 = value;
  }

  public AuraCurseData HealAuraCurseName3
  {
    get => this.healAuraCurseName3;
    set => this.healAuraCurseName3 = value;
  }

  public AuraCurseData HealAuraCurseName4
  {
    get => this.healAuraCurseName4;
    set => this.healAuraCurseName4 = value;
  }

  public int DispelAuras
  {
    get => this.dispelAuras;
    set => this.dispelAuras = value;
  }

  public int EnergyRecharge
  {
    get => this.energyRecharge;
    set => this.energyRecharge = value;
  }

  public AuraCurseData Aura
  {
    get => this.aura;
    set => this.aura = value;
  }

  public AuraCurseData AuraSelf
  {
    get => this.auraSelf;
    set => this.auraSelf = value;
  }

  public int AuraCharges
  {
    get => this.auraCharges;
    set => this.auraCharges = value;
  }

  public AuraCurseData Aura2
  {
    get => this.aura2;
    set => this.aura2 = value;
  }

  public AuraCurseData AuraSelf2
  {
    get => this.auraSelf2;
    set => this.auraSelf2 = value;
  }

  public int AuraCharges2
  {
    get => this.auraCharges2;
    set => this.auraCharges2 = value;
  }

  public AuraCurseData Aura3
  {
    get => this.aura3;
    set => this.aura3 = value;
  }

  public AuraCurseData AuraSelf3
  {
    get => this.auraSelf3;
    set => this.auraSelf3 = value;
  }

  public int AuraCharges3
  {
    get => this.auraCharges3;
    set => this.auraCharges3 = value;
  }

  public AuraCurseData Curse
  {
    get => this.curse;
    set => this.curse = value;
  }

  public AuraCurseData CurseSelf
  {
    get => this.curseSelf;
    set => this.curseSelf = value;
  }

  public int CurseCharges
  {
    get => this.curseCharges;
    set => this.curseCharges = value;
  }

  public AuraCurseData Curse2
  {
    get => this.curse2;
    set => this.curse2 = value;
  }

  public AuraCurseData CurseSelf2
  {
    get => this.curseSelf2;
    set => this.curseSelf2 = value;
  }

  public int CurseCharges2
  {
    get => this.curseCharges2;
    set => this.curseCharges2 = value;
  }

  public AuraCurseData Curse3
  {
    get => this.curse3;
    set => this.curse3 = value;
  }

  public AuraCurseData CurseSelf3
  {
    get => this.curseSelf3;
    set => this.curseSelf3 = value;
  }

  public int CurseCharges3
  {
    get => this.curseCharges3;
    set => this.curseCharges3 = value;
  }

  public int PushTarget
  {
    get => this.pushTarget;
    set => this.pushTarget = value;
  }

  public int PullTarget
  {
    get => this.pullTarget;
    set => this.pullTarget = value;
  }

  public int DrawCard
  {
    get => this.drawCard;
    set => this.drawCard = value;
  }

  public int DiscardCard
  {
    get => this.discardCard;
    set => this.discardCard = value;
  }

  public Enums.CardType DiscardCardType
  {
    get => this.discardCardType;
    set => this.discardCardType = value;
  }

  public Enums.CardType[] DiscardCardTypeAux
  {
    get => this.discardCardTypeAux;
    set => this.discardCardTypeAux = value;
  }

  public bool DiscardCardAutomatic
  {
    get => this.discardCardAutomatic;
    set => this.discardCardAutomatic = value;
  }

  public Enums.CardPlace DiscardCardPlace
  {
    get => this.discardCardPlace;
    set => this.discardCardPlace = value;
  }

  public int AddCard
  {
    get => this.addCard;
    set => this.addCard = value;
  }

  public string AddCardId
  {
    get => this.addCardId;
    set => this.addCardId = value;
  }

  public Enums.CardType AddCardType
  {
    get => this.addCardType;
    set => this.addCardType = value;
  }

  public Enums.CardType[] AddCardTypeAux
  {
    get => this.addCardTypeAux;
    set => this.addCardTypeAux = value;
  }

  public int AddCardChoose
  {
    get => this.addCardChoose;
    set => this.addCardChoose = value;
  }

  public Enums.CardFrom AddCardFrom
  {
    get => this.addCardFrom;
    set => this.addCardFrom = value;
  }

  public Enums.CardPlace AddCardPlace
  {
    get => this.addCardPlace;
    set => this.addCardPlace = value;
  }

  public int AddCardReducedCost
  {
    get => this.addCardReducedCost;
    set => this.addCardReducedCost = value;
  }

  public bool AddCardCostTurn
  {
    get => this.addCardCostTurn;
    set => this.addCardCostTurn = value;
  }

  public bool AddCardVanish
  {
    get => this.addCardVanish;
    set => this.addCardVanish = value;
  }

  public int LookCards
  {
    get => this.lookCards;
    set => this.lookCards = value;
  }

  public int LookCardsDiscardUpTo
  {
    get => this.lookCardsDiscardUpTo;
    set => this.lookCardsDiscardUpTo = value;
  }

  public NPCData SummonUnit
  {
    get => this.summonUnit;
    set => this.summonUnit = value;
  }

  public int SummonUnitNum
  {
    get => this.summonUnitNum;
    set => this.summonUnitNum = value;
  }

  public bool Vanish
  {
    get => this.vanish;
    set => this.vanish = value;
  }

  public bool Lazy
  {
    get => this.lazy;
    set => this.lazy = value;
  }

  public bool Innate
  {
    get => this.innate;
    set => this.innate = value;
  }

  public bool Corrupted
  {
    get => this.corrupted;
    set => this.corrupted = value;
  }

  public bool EndTurn
  {
    get => this.endTurn;
    set => this.endTurn = value;
  }

  public bool MoveToCenter
  {
    get => this.moveToCenter;
    set => this.moveToCenter = value;
  }

  public bool ModifiedByTrait
  {
    get => this.modifiedByTrait;
    set => this.modifiedByTrait = value;
  }

  public float EffectPostTargetDelay
  {
    get => this.effectPostTargetDelay;
    set => this.effectPostTargetDelay = value;
  }

  public Enums.CardSpecialValue SpecialValueGlobal
  {
    get => this.specialValueGlobal;
    set => this.specialValueGlobal = value;
  }

  public float SpecialValueModifierGlobal
  {
    get => this.specialValueModifierGlobal;
    set => this.specialValueModifierGlobal = value;
  }

  public Enums.CardSpecialValue SpecialValue1
  {
    get => this.specialValue1;
    set => this.specialValue1 = value;
  }

  public float SpecialValueModifier1
  {
    get => this.specialValueModifier1;
    set => this.specialValueModifier1 = value;
  }

  public Enums.CardSpecialValue SpecialValue2
  {
    get => this.specialValue2;
    set => this.specialValue2 = value;
  }

  public float SpecialValueModifier2
  {
    get => this.specialValueModifier2;
    set => this.specialValueModifier2 = value;
  }

  public bool DamageSpecialValueGlobal
  {
    get => this.damageSpecialValueGlobal;
    set => this.damageSpecialValueGlobal = value;
  }

  public bool DamageSpecialValue1
  {
    get => this.damageSpecialValue1;
    set => this.damageSpecialValue1 = value;
  }

  public bool DamageSpecialValue2
  {
    get => this.damageSpecialValue2;
    set => this.damageSpecialValue2 = value;
  }

  public bool Damage2SpecialValueGlobal
  {
    get => this.damage2SpecialValueGlobal;
    set => this.damage2SpecialValueGlobal = value;
  }

  public bool Damage2SpecialValue1
  {
    get => this.damage2SpecialValue1;
    set => this.damage2SpecialValue1 = value;
  }

  public bool Damage2SpecialValue2
  {
    get => this.damage2SpecialValue2;
    set => this.damage2SpecialValue2 = value;
  }

  public AuraCurseData SpecialAuraCurseNameGlobal
  {
    get => this.specialAuraCurseNameGlobal;
    set => this.specialAuraCurseNameGlobal = value;
  }

  public AuraCurseData SpecialAuraCurseName1
  {
    get => this.specialAuraCurseName1;
    set => this.specialAuraCurseName1 = value;
  }

  public AuraCurseData SpecialAuraCurseName2
  {
    get => this.specialAuraCurseName2;
    set => this.specialAuraCurseName2 = value;
  }

  public bool AuraChargesSpecialValue1
  {
    get => this.auraChargesSpecialValue1;
    set => this.auraChargesSpecialValue1 = value;
  }

  public bool AuraChargesSpecialValue2
  {
    get => this.auraChargesSpecialValue2;
    set => this.auraChargesSpecialValue2 = value;
  }

  public bool AuraChargesSpecialValueGlobal
  {
    get => this.auraChargesSpecialValueGlobal;
    set => this.auraChargesSpecialValueGlobal = value;
  }

  public bool AuraCharges2SpecialValue1
  {
    get => this.auraCharges2SpecialValue1;
    set => this.auraCharges2SpecialValue1 = value;
  }

  public bool AuraCharges2SpecialValue2
  {
    get => this.auraCharges2SpecialValue2;
    set => this.auraCharges2SpecialValue2 = value;
  }

  public bool AuraCharges2SpecialValueGlobal
  {
    get => this.auraCharges2SpecialValueGlobal;
    set => this.auraCharges2SpecialValueGlobal = value;
  }

  public bool AuraCharges3SpecialValue1
  {
    get => this.auraCharges3SpecialValue1;
    set => this.auraCharges3SpecialValue1 = value;
  }

  public bool AuraCharges3SpecialValue2
  {
    get => this.auraCharges3SpecialValue2;
    set => this.auraCharges3SpecialValue2 = value;
  }

  public bool AuraCharges3SpecialValueGlobal
  {
    get => this.auraCharges3SpecialValueGlobal;
    set => this.auraCharges3SpecialValueGlobal = value;
  }

  public bool CurseChargesSpecialValue1
  {
    get => this.curseChargesSpecialValue1;
    set => this.curseChargesSpecialValue1 = value;
  }

  public bool CurseChargesSpecialValue2
  {
    get => this.curseChargesSpecialValue2;
    set => this.curseChargesSpecialValue2 = value;
  }

  public bool CurseChargesSpecialValueGlobal
  {
    get => this.curseChargesSpecialValueGlobal;
    set => this.curseChargesSpecialValueGlobal = value;
  }

  public bool CurseCharges2SpecialValue1
  {
    get => this.curseCharges2SpecialValue1;
    set => this.curseCharges2SpecialValue1 = value;
  }

  public bool CurseCharges2SpecialValue2
  {
    get => this.curseCharges2SpecialValue2;
    set => this.curseCharges2SpecialValue2 = value;
  }

  public bool CurseCharges2SpecialValueGlobal
  {
    get => this.curseCharges2SpecialValueGlobal;
    set => this.curseCharges2SpecialValueGlobal = value;
  }

  public bool CurseCharges3SpecialValue1
  {
    get => this.curseCharges3SpecialValue1;
    set => this.curseCharges3SpecialValue1 = value;
  }

  public bool CurseCharges3SpecialValue2
  {
    get => this.curseCharges3SpecialValue2;
    set => this.curseCharges3SpecialValue2 = value;
  }

  public bool CurseCharges3SpecialValueGlobal
  {
    get => this.curseCharges3SpecialValueGlobal;
    set => this.curseCharges3SpecialValueGlobal = value;
  }

  public bool HealSpecialValueGlobal
  {
    get => this.healSpecialValueGlobal;
    set => this.healSpecialValueGlobal = value;
  }

  public bool HealSpecialValue1
  {
    get => this.healSpecialValue1;
    set => this.healSpecialValue1 = value;
  }

  public bool HealSpecialValue2
  {
    get => this.healSpecialValue2;
    set => this.healSpecialValue2 = value;
  }

  public bool SelfHealthLossSpecialGlobal
  {
    get => this.selfHealthLossSpecialGlobal;
    set => this.selfHealthLossSpecialGlobal = value;
  }

  public bool SelfHealthLossSpecialValue1
  {
    get => this.selfHealthLossSpecialValue1;
    set => this.selfHealthLossSpecialValue1 = value;
  }

  public bool SelfHealthLossSpecialValue2
  {
    get => this.selfHealthLossSpecialValue2;
    set => this.selfHealthLossSpecialValue2 = value;
  }

  public float FluffPercent
  {
    get => this.fluffPercent;
    set => this.fluffPercent = value;
  }

  public ItemData Item
  {
    get => this.item;
    set => this.item = value;
  }

  public AuraCurseData SummonAura
  {
    get => this.summonAura;
    set => this.summonAura = value;
  }

  public int SummonAuraCharges
  {
    get => this.summonAuraCharges;
    set => this.summonAuraCharges = value;
  }

  public AuraCurseData SummonAura2
  {
    get => this.summonAura2;
    set => this.summonAura2 = value;
  }

  public int SummonAuraCharges2
  {
    get => this.summonAuraCharges2;
    set => this.summonAuraCharges2 = value;
  }

  public AuraCurseData SummonAura3
  {
    get => this.summonAura3;
    set => this.summonAura3 = value;
  }

  public int SummonAuraCharges3
  {
    get => this.summonAuraCharges3;
    set => this.summonAuraCharges3 = value;
  }

  public bool HealSelfSpecialValueGlobal
  {
    get => this.healSelfSpecialValueGlobal;
    set => this.healSelfSpecialValueGlobal = value;
  }

  public bool HealSelfSpecialValue1
  {
    get => this.healSelfSpecialValue1;
    set => this.healSelfSpecialValue1 = value;
  }

  public bool HealSelfSpecialValue2
  {
    get => this.healSelfSpecialValue2;
    set => this.healSelfSpecialValue2 = value;
  }

  public GameObject PetModel
  {
    get => this.petModel;
    set => this.petModel = value;
  }

  public bool PetFront
  {
    get => this.petFront;
    set => this.petFront = value;
  }

  public Vector2 PetOffset
  {
    get => this.petOffset;
    set => this.petOffset = value;
  }

  public Vector2 PetSize
  {
    get => this.petSize;
    set => this.petSize = value;
  }

  public bool PetInvert
  {
    get => this.petInvert;
    set => this.petInvert = value;
  }

  public bool IsPetAttack
  {
    get => this.isPetAttack;
    set => this.isPetAttack = value;
  }

  public bool IsPetCast
  {
    get => this.isPetCast;
    set => this.isPetCast = value;
  }

  public CardData UpgradesToRare
  {
    get => this.upgradesToRare;
    set => this.upgradesToRare = value;
  }

  public int ExhaustCounter
  {
    get => this.exhaustCounter;
    set => this.exhaustCounter = value;
  }

  public bool Starter
  {
    get => this.starter;
    set => this.starter = value;
  }

  public string Target
  {
    get => this.target;
    set => this.target = value;
  }

  public ItemData ItemEnchantment
  {
    get => this.itemEnchantment;
    set => this.itemEnchantment = value;
  }

  public CardData[] AddCardList
  {
    get => this.addCardList;
    set => this.addCardList = value;
  }

  public bool ShowInTome
  {
    get => this.showInTome;
    set => this.showInTome = value;
  }

  public int LookCardsVanishUpTo
  {
    get => this.lookCardsVanishUpTo;
    set => this.lookCardsVanishUpTo = value;
  }

  public int TransferCurses
  {
    get => this.transferCurses;
    set => this.transferCurses = value;
  }

  public bool KillPet
  {
    get => this.killPet;
    set => this.killPet = value;
  }

  public int ReduceCurses
  {
    get => this.reduceCurses;
    set => this.reduceCurses = value;
  }

  public AuraCurseData AcEnergyBonus
  {
    get => this.acEnergyBonus;
    set => this.acEnergyBonus = value;
  }

  public int AcEnergyBonusQuantity
  {
    get => this.acEnergyBonusQuantity;
    set => this.acEnergyBonusQuantity = value;
  }

  public int EnergyReductionPermanent
  {
    get => this.energyReductionPermanent;
    set => this.energyReductionPermanent = value;
  }

  public int EnergyReductionTemporal
  {
    get => this.energyReductionTemporal;
    set => this.energyReductionTemporal = value;
  }

  public bool EnergyReductionToZeroPermanent
  {
    get => this.energyReductionToZeroPermanent;
    set => this.energyReductionToZeroPermanent = value;
  }

  public bool EnergyReductionToZeroTemporal
  {
    get => this.energyReductionToZeroTemporal;
    set => this.energyReductionToZeroTemporal = value;
  }

  public AuraCurseData AcEnergyBonus2
  {
    get => this.acEnergyBonus2;
    set => this.acEnergyBonus2 = value;
  }

  public int AcEnergyBonus2Quantity
  {
    get => this.acEnergyBonus2Quantity;
    set => this.acEnergyBonus2Quantity = value;
  }

  public int StealAuras
  {
    get => this.stealAuras;
    set => this.stealAuras = value;
  }

  public bool FlipSprite
  {
    get => this.flipSprite;
    set => this.flipSprite = value;
  }

  public AudioClip SoundPreActionFemale
  {
    get => this.soundPreActionFemale;
    set => this.soundPreActionFemale = value;
  }

  public int ReduceAuras
  {
    get => this.reduceAuras;
    set => this.reduceAuras = value;
  }

  public int IncreaseCurses
  {
    get => this.increaseCurses;
    set => this.increaseCurses = value;
  }

  public int IncreaseAuras
  {
    get => this.increaseAuras;
    set => this.increaseAuras = value;
  }

  public bool OnlyInWeekly
  {
    get => this.onlyInWeekly;
    set => this.onlyInWeekly = value;
  }

  public string RelatedCard
  {
    get => this.relatedCard;
    set => this.relatedCard = value;
  }

  public string RelatedCard2
  {
    get => this.relatedCard2;
    set => this.relatedCard2 = value;
  }

  public string RelatedCard3
  {
    get => this.relatedCard3;
    set => this.relatedCard3 = value;
  }

  public int GoldGainQuantity
  {
    get => this.goldGainQuantity;
    set => this.goldGainQuantity = value;
  }

  public int ShardsGainQuantity
  {
    get => this.shardsGainQuantity;
    set => this.shardsGainQuantity = value;
  }

  public string Sku
  {
    get => this.sku;
    set => this.sku = value;
  }

  public bool EnergyRechargeSpecialValueGlobal
  {
    get => this.energyRechargeSpecialValueGlobal;
    set => this.energyRechargeSpecialValueGlobal = value;
  }

  public bool DrawCardSpecialValueGlobal
  {
    get => this.drawCardSpecialValueGlobal;
    set => this.drawCardSpecialValueGlobal = value;
  }

  public float SelfKillHiddenSeconds
  {
    get => this.selfKillHiddenSeconds;
    set => this.selfKillHiddenSeconds = value;
  }

  public float SoundHitReworkDelay
  {
    get => this.soundHitReworkDelay;
    set => this.soundHitReworkDelay = value;
  }

  public bool Evolve
  {
    get => this.evolve;
    set => this.evolve = value;
  }

  public bool Metamorph
  {
    get => this.metamorph;
    set => this.metamorph = value;
  }

  public bool PetTemporal
  {
    get => this.petTemporal;
    set => this.petTemporal = value;
  }

  public bool PetTemporalAttack
  {
    get => this.petTemporalAttack;
    set => this.petTemporalAttack = value;
  }

  public bool PetTemporalCast
  {
    get => this.petTemporalCast;
    set => this.petTemporalCast = value;
  }

  public bool PetTemporalMoveToCenter
  {
    get => this.petTemporalMoveToCenter;
    set => this.petTemporalMoveToCenter = value;
  }

  public bool PetTemporalMoveToBack
  {
    get => this.petTemporalMoveToBack;
    set => this.petTemporalMoveToBack = value;
  }

  public float PetTemporalFadeOutDelay
  {
    get => this.petTemporalFadeOutDelay;
    set => this.petTemporalFadeOutDelay = value;
  }

  public int CurseChargesSides
  {
    get => this.curseChargesSides;
    set => this.curseChargesSides = value;
  }

  public bool ChooseOneOfAvailableAuras
  {
    get => this.chooseOneOfAvailableAuras;
    set => this.chooseOneOfAvailableAuras = value;
  }

  public bool AddVanishToDeck
  {
    get => this.addVanishToDeck;
    set => this.addVanishToDeck = value;
  }

  public bool AddCardOnlyCheckAuxTypes
  {
    get => this.addCardOnlyCheckAuxTypes;
    set => this.addCardOnlyCheckAuxTypes = value;
  }

  public AudioClip SoundDragRework
  {
    get => this.soundDragRework;
    set => this.soundDragRework = value;
  }

  public AudioClip SoundReleaseRework
  {
    get => this.soundReleaseRework;
    set => this.soundReleaseRework = value;
  }

  public AudioClip SoundHitRework
  {
    get => this.soundHitRework;
    set => this.soundHitRework = value;
  }

  public CardData.AuraBuffs[] Auras
  {
    get => this.auras;
    set => this.auras = value;
  }

  public CardData.CurseDebuffs[] Curses
  {
    get => this.curses;
    set => this.curses = value;
  }

  static CardData()
  {
    ItemData theItem;
    CardData.CardValues = new Dictionary<string, Func<CardData, string>>()
    {
      {
        "$DestroyAfterUses",
        (Func<CardData, string>) (data => !data.TryGetItem(out theItem) ? string.Empty : theItem.DestroyAfterUses.ToString())
      },
      {
        "$SpecialValueModifierGlobal",
        (Func<CardData, string>) (data => data.specialValueModifierGlobal.ToString("F0"))
      }
    };
  }

  [Serializable]
  public class AuraBuffs
  {
    public AuraCurseData aura;
    public AuraCurseData auraSelf;
    public int auraCharges;
    public bool auraChargesSpecialValueGlobal;
    public bool auraChargesSpecialValue1;
    public bool auraChargesSpecialValue2;
  }

  [Serializable]
  public class CurseDebuffs
  {
    public AuraCurseData curse;
    public AuraCurseData curseSelf;
    public int curseCharges;
    public int curseChargesSides;
    public bool curseChargesSpecialValueGlobal;
    public bool curseChargesSpecialValue1;
    public bool curseChargesSpecialValue2;
  }

  [Serializable]
  public struct CardToGainTypeBasedOnHeroClass
  {
    public Enums.HeroClass heroClass;
    public List<Enums.CardType> cardTypes;
  }

  [Serializable]
  public struct CardToGainListBasedOnHeroClass
  {
    public Enums.HeroClass heroClass;
    public List<CardData> cardsList;
  }
}
