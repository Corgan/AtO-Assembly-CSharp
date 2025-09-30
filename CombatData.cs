// Decompiled with JetBrains decompiler
// Type: CombatData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "New Combat", menuName = "Combat Data", order = 61)]
public class CombatData : ScriptableObject
{
  [SerializeField]
  private string combatId;
  [TextArea]
  [SerializeField]
  private string description;
  [SerializeField]
  private Enums.CombatBackground combatBackground;
  [SerializeField]
  private AudioClip combatMusic;
  [SerializeField]
  private Enums.CombatStepSound stepSound;
  [SerializeField]
  private NPCData[] npcList;
  [SerializeField]
  private NPCData npcToSummonOnNpcKilled;
  [SerializeField]
  private bool randomizeNpcPosition;
  [SerializeField]
  private int npcRemoveInMadness0Index = -1;
  [Header("Thermometer")]
  [SerializeField]
  private ThermometerTierData thermometerTierData;
  [Header("Combat Effects")]
  [SerializeField]
  private global::CombatEffect[] combatEffect;
  [SerializeField]
  private bool healHeroes;
  [Header("Combat Tier")]
  [SerializeField]
  private Enums.CombatTier combatTier;
  [Header("Launch event at the end of combat (w/ requisite)")]
  [SerializeField]
  private EventData eventData;
  [SerializeField]
  private EventRequirementData eventRequirementData;
  [Header("Cinematic after the combat")]
  [SerializeField]
  private CinematicData cinematicData;
  [Header("Is this a node with a Rift?")]
  [SerializeField]
  private bool isRift;

  public NPCData[] NPCList
  {
    get => this.npcList;
    set => this.npcList = value;
  }

  public string CombatId
  {
    get => this.combatId;
    set => this.combatId = value;
  }

  public void DoStepSound()
  {
    if (this.stepSound == Enums.CombatStepSound.Walk_Stone)
      GameManager.Instance.PlayAudio(AudioManager.Instance.walkStone);
    else if (this.stepSound == Enums.CombatStepSound.Walk_Grass)
    {
      GameManager.Instance.PlayAudio(AudioManager.Instance.walkGrass);
    }
    else
    {
      if (this.stepSound != Enums.CombatStepSound.Walk_Water)
        return;
      GameManager.Instance.PlayAudio(AudioManager.Instance.walkWater);
    }
  }

  public string Description
  {
    get => this.description;
    set => this.description = value;
  }

  public Enums.CombatBackground CombatBackground
  {
    get => this.combatBackground;
    set => this.combatBackground = value;
  }

  public global::CombatEffect[] CombatEffect
  {
    get => this.combatEffect;
    set => this.combatEffect = value;
  }

  public Enums.CombatTier CombatTier
  {
    get => this.combatTier;
    set => this.combatTier = value;
  }

  public EventData EventData
  {
    get => this.eventData;
    set => this.eventData = value;
  }

  public EventRequirementData EventRequirementData
  {
    get => this.eventRequirementData;
    set => this.eventRequirementData = value;
  }

  public ThermometerTierData ThermometerTierData
  {
    get => this.thermometerTierData;
    set => this.thermometerTierData = value;
  }

  public AudioClip CombatMusic
  {
    get => this.combatMusic;
    set => this.combatMusic = value;
  }

  public CinematicData CinematicData
  {
    get => this.cinematicData;
    set => this.cinematicData = value;
  }

  public bool HealHeroes
  {
    get => this.healHeroes;
    set => this.healHeroes = value;
  }

  public int NpcRemoveInMadness0Index
  {
    get => this.npcRemoveInMadness0Index;
    set => this.npcRemoveInMadness0Index = value;
  }

  public bool IsRift
  {
    get => this.isRift;
    set => this.isRift = value;
  }

  public bool RandomizeNpcPosition
  {
    get => this.randomizeNpcPosition;
    set => this.randomizeNpcPosition = value;
  }

  public NPCData NpcToSummonOnNpcKilled
  {
    get => this.npcToSummonOnNpcKilled;
    set => this.npcToSummonOnNpcKilled = value;
  }
}
