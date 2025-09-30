// Decompiled with JetBrains decompiler
// Type: Dracula
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class Dracula : BossNPC
{
  private const string NPC_SWAP_VFX_NAME = "lightningstrike";
  private int _nextSlotJumpStep = 1;
  private bool transformed;

  public Dracula(NPC npc)
    : base(npc)
  {
    MatchManager.Instance.OnLoadCombatFinished += new Action(this.SetupCombatStartEffects);
    MatchManager.Instance.OnCardCastByHeroBegins += new Action<Hero, CardData>(this.SwapPosition);
    MatchManager.OnCharacterDamaged += new Action<Character, int>(this.OnCharacterDamaged);
  }

  public override void Dispose()
  {
    base.Dispose();
    MatchManager.Instance.OnLoadCombatFinished -= new Action(this.SetupCombatStartEffects);
    MatchManager.Instance.OnCardCastByHeroBegins -= new Action<Hero, CardData>(this.SwapPosition);
    MatchManager.OnCharacterDamaged -= new Action<Character, int>(this.OnCharacterDamaged);
  }

  private void SetupCombatStartEffects()
  {
    NPC[] teamNpc = MatchManager.Instance.GetTeamNPC();
    for (int index = 0; index < teamNpc.Length; ++index)
    {
      if (teamNpc[index] != null && teamNpc[index].Alive && teamNpc[index].InternalId != this.npc.InternalId)
        teamNpc[index].SetAura((Character) null, Globals.Instance.GetAuraCurseData("invulnerable"), 1);
    }
  }

  private void SwapPosition(Hero arg1, CardData arg2)
  {
    int position = this.npc.Position;
    int npcIndex = this.npc.NPCIndex;
    int nextSlotJumpStep = this._nextSlotJumpStep;
    int num1 = (position + nextSlotJumpStep) % 4;
    int index2 = (npcIndex + this._nextSlotJumpStep) % 4;
    NPC[] teamNpc = MatchManager.Instance.GetTeamNPC();
    NPC npc1 = teamNpc[index2];
    MatchManager.Instance.SwapNPCDeck(npcIndex, index2);
    MatchManager.Instance.SwapNPCDeckDiscard(npcIndex, index2);
    MatchManager.Instance.SwapNPCHand(npcIndex, index2);
    MatchManager.Instance.SwapCharacterOrder(this.npc.Id, npc1.Id, npcIndex, index2);
    npc1.Position = this.npc.Position;
    npc1.NPCIndex = this.npc.NPCIndex;
    this.npc.Position = num1;
    this.npc.NPCIndex = index2;
    Transform target = MatchManager.Instance.GetTarget();
    if ((UnityEngine.Object) target == (UnityEngine.Object) npc1.GO.transform)
      MatchManager.Instance.SetTarget(this.npc.GO.transform);
    else if ((UnityEngine.Object) target == (UnityEngine.Object) this.npc.GO.transform)
      MatchManager.Instance.SetTarget(npc1.GO.transform);
    NPC[] npcArray1 = teamNpc;
    int index1 = npcIndex;
    NPC[] npcArray2 = teamNpc;
    int num2 = index2;
    NPC npc2 = teamNpc[index2];
    NPC npc3 = teamNpc[npcIndex];
    npcArray1[index1] = npc2;
    int index3 = num2;
    NPC npc4 = npc3;
    npcArray2[index3] = npc4;
    for (int index4 = 0; index4 < teamNpc.Length; ++index4)
    {
      if (index4 == npcIndex || index4 == index2)
        EffectsManager.Instance.PlayEffect("lightningstrike", teamNpc[index4].NPCItem.CharImageT);
      teamNpc[index4].NPCItem.SetPosition(true);
    }
    this._nextSlotJumpStep = 3 - this._nextSlotJumpStep;
  }

  private void OnCharacterDamaged(Character character, int damage)
  {
    if (!(bool) (UnityEngine.Object) character.NpcData || !(character.NpcData.Id == "count") || (double) character.HpCurrent / (double) character.GetMaxHP() >= 0.40000000596046448 || this.transformed)
      return;
    this.transformed = true;
    foreach (CharacterGOItem characterGoItem in UnityEngine.Object.FindObjectsOfType<CharacterGOItem>())
    {
      if (characterGoItem.name.Contains("count"))
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) characterGoItem.gameObject);
        character.TransformedModel = true;
        character.NPCItem.Init(MatchManager.Instance.GetTeamNPC()[character.Position], true);
        character.SpriteSpeed = character.NpcData.SpriteSpeedAlternate;
        character.SpritePortrait = character.NpcData.SpritePortraitAlternate;
        MatchManager.Instance.SortCharacterSprites();
        MatchManager.Instance.SetInitiatives();
        MatchManager.Instance.RepositionCharacters();
        EffectsManager.Instance.PlayEffect("fireburst", MatchManager.Instance.GetTeamNPC()[character.Position].NPCItem.CharImageT, false, false);
      }
    }
  }
}
