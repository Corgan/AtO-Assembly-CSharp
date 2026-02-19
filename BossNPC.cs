using System;

public class BossNPC : IDisposable
{
	internal NPC npc;

	public BossNPC(NPC npc)
	{
		this.npc = npc;
	}

	public virtual void Dispose()
	{
		npc = null;
	}

	public virtual bool IsValidTarget()
	{
		return true;
	}

	public virtual void OnCharacterDamaged(Character character, int damage, int hpCurrent)
	{
	}

	public virtual void OnCharacterKilled(NPCData npcData, HeroData heroData, int position)
	{
	}

	public virtual void OnRepeatCardCast()
	{
	}

	public virtual void OnRepeatCardIterationsFinished()
	{
	}
}
