using System;
using UnityEngine;

[Serializable]
public class Frankenstein : BossNPC
{
	private const string LIGHTNING_BOLT_VFX_NAME = "frankylightning";

	private const int LIGHTNING_SLOT_JUMP_STEP = 1;

	private int _currentLightningSlot;

	public Frankenstein(NPC npc)
		: base(npc)
	{
		MatchManager instance = MatchManager.Instance;
		instance.OnLoadCombatFinished = (Action)Delegate.Combine(instance.OnLoadCombatFinished, new Action(SetupCombatStartEffects));
		MatchManager instance2 = MatchManager.Instance;
		instance2.OnCardCastedByHero = (Action<Hero, CardData>)Delegate.Combine(instance2.OnCardCastedByHero, new Action<Hero, CardData>(TriggerLightningBolt));
	}

	public override void Dispose()
	{
		base.Dispose();
		MatchManager instance = MatchManager.Instance;
		instance.OnLoadCombatFinished = (Action)Delegate.Remove(instance.OnLoadCombatFinished, new Action(SetupCombatStartEffects));
		MatchManager instance2 = MatchManager.Instance;
		instance2.OnCardCastedByHero = (Action<Hero, CardData>)Delegate.Remove(instance2.OnCardCastedByHero, new Action<Hero, CardData>(TriggerLightningBolt));
	}

	private void SetupCombatStartEffects()
	{
		npc.SetAura(null, Globals.Instance.GetAuraCurseData("invulnerable"), 1);
	}

	private void TriggerLightningBolt(Hero hero, CardData card)
	{
		float posX = npc.NPCItem.CalculatePositionX(_currentLightningSlot);
		EffectsManager.Instance.PlayEffect("frankylightning", posX);
		DamageIfLightningHit();
		_currentLightningSlot = (_currentLightningSlot + 1) % 4;
	}

	private void DamageIfLightningHit()
	{
		if (_currentLightningSlot == npc.NPCIndex)
		{
			Debug.Log("Lightning has hit the Frankenstein.");
			npc.HealCursesName(null, "invulnerable");
			npc.IndirectDamage(Enums.DamageType.Lightning, 11, null);
			npc.SetAura(null, Globals.Instance.GetAuraCurseData("spark"), 3);
			npc.NPCItem.CharacterHitted(fromHit: true);
		}
	}
}
