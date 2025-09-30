// Decompiled with JetBrains decompiler
// Type: SirenQueen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

#nullable disable
public class SirenQueen : BossNPC, IDisposable
{
  private NPCData npcData;
  private string[] teamNpcInitial = new string[4];
  private float healthThreshold;
  private const string SirenQueenID = "s_queen";
  private const string SpawnFx = "stealth";
  private bool hasAppeared;
  private bool isEscaped;
  private bool isSpawning;
  private bool isDead;
  private IEnumerator onCharacterKilledCo;
  private IEnumerator onCharacterDamagedCo;
  private List<string> npcDeck;
  private List<string> npcHand;

  public SirenQueen(NPC npc = null)
    : base(npc)
  {
    this.npcData = Globals.Instance.GetNPC("s_queen");
    this.npc = new NPC();
    this.npc.NpcData = this.npcData;
    this.npc.InitData();
    MatchManager.OnCharacterKilled += new Action<NPCData, HeroData, int>(this.OnCharacterKilled);
    MatchManager.OnCharacterDamaged += new Action<Character, int>(this.OnCharacterDamaged);
    this.healthThreshold = (float) this.npc.GetMaxHP() * 0.75f;
    this.GetInitialNpcTeam();
  }

  public override void Dispose()
  {
    base.Dispose();
    MatchManager.OnCharacterKilled -= new Action<NPCData, HeroData, int>(this.OnCharacterKilled);
    MatchManager.OnCharacterDamaged -= new Action<Character, int>(this.OnCharacterDamaged);
  }

  private void GetInitialNpcTeam()
  {
    NPC[] teamNpc = MatchManager.Instance.GetTeamNPC();
    for (int index = 0; index < teamNpc.Length; ++index)
      this.teamNpcInitial[index] = teamNpc[index] == null ? "" : teamNpc[index].NpcData.Id;
  }

  private void OnCharacterKilled(NPCData npcData, HeroData heroData, int position)
  {
    Globals.Instance.StartCoroutine(this.OnCharacterKilledCo(npcData, heroData, position));
  }

  private IEnumerator OnCharacterKilledCo(NPCData npcData, HeroData heroData, int position)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    SirenQueen sirenQueen = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (sirenQueen.isDead)
      return false;
    if ((UnityEngine.Object) npcData != (UnityEngine.Object) null && npcData.Id == "s_queen")
    {
      sirenQueen.isDead = true;
      return false;
    }
    if ((UnityEngine.Object) npcData != (UnityEngine.Object) null && npcData.Id != "s_queen" && !sirenQueen.isDead && !sirenQueen.IsThisBossSpawned() && !sirenQueen.isSpawning)
    {
      MatchManager.Instance.GetTeamNPC()[position] = (NPC) null;
      sirenQueen.isSpawning = true;
      if (sirenQueen.hasAppeared && sirenQueen.npc.HpCurrent > 0)
      {
        Globals.Instance.WaitForSeconds(0.65f);
        sirenQueen.RespawnBoss(position);
        Globals.Instance.WaitForSeconds(0.5f);
        if (MatchManager.Instance.GetCharacterActive() != null && !MatchManager.Instance.GetCharacterActive().IsHero)
          MatchManager.Instance.NextTurnFunction();
      }
      else
      {
        sirenQueen.hasAppeared = true;
        Globals.Instance.WaitForSeconds(0.65f);
        MatchManager.Instance.CreateNPC(Globals.Instance.GetNPC("s_queen"), "stealth", position, delay: 0.65f);
        sirenQueen.isSpawning = false;
        sirenQueen.npc = MatchManager.Instance.GetTeamNPC()[position];
        sirenQueen.npc.NPCIndex = position;
        sirenQueen.npc.Position = position;
        Globals.Instance.WaitForSeconds(0.7f);
        if (MatchManager.Instance.GetCharacterActive() != null && !MatchManager.Instance.GetCharacterActive().IsHero)
          MatchManager.Instance.NextTurnFunction();
        sirenQueen.npcDeck = MatchManager.Instance?.GetNPCDeck(sirenQueen.npc.NPCIndex);
        sirenQueen.npcHand = new List<string>();
        EffectsManager.Instance.PlayEffect("stealth", MatchManager.Instance.GetTeamNPC()[position].NPCItem.CharImageT, false, false);
      }
    }
    return false;
  }

  private void OnCharacterDamaged(Character character, int damage)
  {
    this.onCharacterDamagedCo = this.OnCharacterDamagedCo(character, damage);
    MatchManager.Instance.StartCoroutine(this.onCharacterDamagedCo);
  }

  private IEnumerator OnCharacterDamagedCo(Character character, int damage)
  {
    yield return (object) new WaitForSeconds(0.0f);
    if ((UnityEngine.Object) character.NPCItem != (UnityEngine.Object) null && (UnityEngine.Object) character.NpcData != (UnityEngine.Object) null && character.NpcData.Id == "s_queen" && (double) character.HpCurrent < (double) this.healthThreshold && !character.NPCItem.IsDying && !this.isEscaped && character.HpCurrent > 0)
    {
      this.isEscaped = true;
      this.UpdateHealthThreshold(character.HpCurrent);
      int npcPos = character.NPCIndex;
      this.npcDeck = MatchManager.Instance.GetNPCDeck(npcPos);
      this.npcHand = MatchManager.Instance.GetNPCHand(npcPos);
      MatchManager.Instance.GetTeamNPC()[npcPos] = (NPC) null;
      this.SetCharacterVisible(false);
      yield return (object) new WaitForSeconds(0.65f);
      if (!this.teamNpcInitial[npcPos].IsNullOrEmpty())
      {
        MatchManager.Instance.CreateNPC(Globals.Instance.GetNPC(this.teamNpcInitial[npcPos]), "stealth", npcPos, delay: 0.5f);
        EffectsManager.Instance.PlayEffect("stealth", MatchManager.Instance.GetTeamNPC()[npcPos].NPCItem.CharImageT, false, false, 0.5f);
        int leftMostEmptySpace = this.LeftMostEmptyIndexInTeamNpc();
        if (leftMostEmptySpace > -1)
        {
          yield return (object) new WaitForSeconds(0.65f);
          this.RespawnBoss(leftMostEmptySpace);
          yield return (object) new WaitForSeconds(0.5f);
          if (!MatchManager.Instance.GetCharacterActive().IsHero)
            MatchManager.Instance.NextTurnFunction();
        }
      }
    }
  }

  private int LeftMostEmptyIndexInTeamNpc()
  {
    NPC[] teamNpc = MatchManager.Instance.GetTeamNPC();
    for (int index = 0; index < teamNpc.Length; ++index)
    {
      if ((teamNpc[index] == null || !teamNpc[index].Alive) && !this.teamNpcInitial[index].IsNullOrEmpty())
        return index;
    }
    return -1;
  }

  private void RespawnBoss(int position)
  {
    this.isSpawning = false;
    this.isEscaped = false;
    MatchManager.Instance.GetTeamNPC()[position] = this.npc;
    this.npc.NPCIndex = position;
    this.npc.Position = position;
    this.npc.NPCItem.SetPosition(true, position);
    this.SetCharacterVisible(true);
    MatchManager.Instance.SetInitiatives();
    MatchManager.Instance.RepositionCharacters();
    MatchManager.Instance.SetNPCDeck(this.npc.NPCIndex, this.npcDeck);
    MatchManager.Instance.SetNPCHand(this.npc.NPCIndex, this.npcHand);
    EffectsManager.Instance.PlayEffect("stealth", MatchManager.Instance.GetTeamNPC()[position].NPCItem.CharImageT, false, false);
  }

  private bool IsThisBossSpawned()
  {
    foreach (NPC npc in MatchManager.Instance.GetTeamNPC())
    {
      if (npc != null && npc.Alive && npc.NpcData.Id == "s_queen")
        return true;
    }
    return false;
  }

  private void UpdateHealthThreshold(int currHp)
  {
    int maxHp = this.npc.GetMaxHP();
    if ((double) currHp < (double) maxHp * 0.25)
      this.healthThreshold = 0.0f;
    else if ((double) currHp < (double) maxHp * 0.5)
      this.healthThreshold = (float) maxHp * 0.25f;
    else if ((double) currHp < (double) maxHp * 0.75)
      this.healthThreshold = (float) maxHp * 0.5f;
    else
      this.healthThreshold = (float) maxHp * 0.75f;
  }

  private void SetCharacterVisible(bool state)
  {
    try
    {
      this.npc.NPCItem.CharImageT.gameObject.SetActive(state);
    }
    catch
    {
      Debug.Log((object) "CharImageT not found");
    }
    try
    {
      this.npc.NPCItem.CharImageShadowT.gameObject.SetActive(state);
    }
    catch
    {
      Debug.Log((object) "CharImageShadowT not found");
    }
    try
    {
      this.npc.NPCItem.healthBar.gameObject.SetActive(state);
    }
    catch
    {
      Debug.Log((object) "healthBar not found");
    }
    if (state)
    {
      this.npc.NPCItem.ShowEnchantments();
    }
    else
    {
      try
      {
        this.npc.NPCItem.HideEnchatmentIcons();
      }
      catch
      {
      }
    }
  }

  public override bool IsValidTarget() => !this.isEscaped;
}
