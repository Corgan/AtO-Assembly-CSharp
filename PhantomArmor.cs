// Decompiled with JetBrains decompiler
// Type: PhantomArmor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class PhantomArmor : BossNPC
{
  private GameObject phantomArmor;
  private GameObject vfx;

  public PhantomArmor(NPC npc)
    : base(npc)
  {
    Transform npcParent = MatchManager.Instance.GetNPCParent();
    this.phantomArmor = Object.Instantiate<GameObject>(MatchManager.Instance.phantomArmorPrefab, npcParent);
    this.phantomArmor.SetActive(false);
    this.vfx = Object.Instantiate<GameObject>(MatchManager.Instance.phantomArmorVfxPrefab, npcParent);
    this.vfx.SetActive(false);
  }

  public bool IsSpecialCard(string cardId) => cardId.ToLower() == "divineexecutionmnp";

  public void TriggerSpecialEffect()
  {
    Debug.Log((object) "TriggerSpecialEffect called");
    foreach (NPC npc in MatchManager.Instance.GetTeamNPC())
      npc?.NPCItem?.Anim?.SetTrigger("ability");
    this.phantomArmor.SetActive(true);
    this.vfx.SetActive(true);
  }

  public void SpecialEffectFinish()
  {
    this.phantomArmor.SetActive(false);
    this.vfx.SetActive(false);
  }

  public bool IsSpecialEffectFinished() => !this.phantomArmor.activeSelf;
}
