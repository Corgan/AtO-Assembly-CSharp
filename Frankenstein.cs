// Decompiled with JetBrains decompiler
// Type: Frankenstein
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class Frankenstein : BossNPC
{
  private const string LIGHTNING_BOLT_VFX_NAME = "frankylightning";
  private const int LIGHTNING_SLOT_JUMP_STEP = 1;
  private int _currentLightningSlot;

  public Frankenstein(NPC npc)
    : base(npc)
  {
    MatchManager.Instance.OnLoadCombatFinished += new Action(this.SetupCombatStartEffects);
    MatchManager.Instance.OnCardCastedByHero += new Action<Hero, CardData>(this.TriggerLightningBolt);
  }

  public override void Dispose()
  {
    base.Dispose();
    MatchManager.Instance.OnLoadCombatFinished -= new Action(this.SetupCombatStartEffects);
    MatchManager.Instance.OnCardCastedByHero -= new Action<Hero, CardData>(this.TriggerLightningBolt);
  }

  private void SetupCombatStartEffects()
  {
    this.npc.SetAura((Character) null, Globals.Instance.GetAuraCurseData("invulnerable"), 1);
  }

  private void TriggerLightningBolt(Hero hero, CardData card)
  {
    EffectsManager.Instance.PlayEffect("frankylightning", this.npc.NPCItem.CalculatePositionX(this._currentLightningSlot));
    this.DamageIfLightningHit();
    this._currentLightningSlot = (this._currentLightningSlot + 1) % 4;
  }

  private void DamageIfLightningHit()
  {
    if (this._currentLightningSlot != this.npc.NPCIndex)
      return;
    Debug.Log((object) "Lightning has hit the Frankenstein.");
    this.npc.HealCursesName(singleCurse: "invulnerable");
    this.npc.IndirectDamage(Enums.DamageType.Lightning, 11);
    this.npc.SetAura((Character) null, Globals.Instance.GetAuraCurseData("spark"), 3);
    this.npc.NPCItem.CharacterHitted(true);
  }
}
