using UnityEngine;

public class PhantomArmor : BossNPC
{
	private GameObject phantomArmor;

	private GameObject vfx;

	public PhantomArmor(NPC npc)
		: base(npc)
	{
		Initialize();
	}

	public void Initialize()
	{
		Transform nPCParent = MatchManager.Instance.GetNPCParent();
		phantomArmor = Object.Instantiate(MatchManager.Instance.phantomArmorPrefab, nPCParent);
		phantomArmor.SetActive(value: false);
		vfx = Object.Instantiate(MatchManager.Instance.phantomArmorVfxPrefab, nPCParent);
		vfx.SetActive(value: false);
	}

	public bool IsSpecialCard(string cardId)
	{
		return cardId.StartsWith("divineexecutionmnp");
	}

	public void TriggerSpecialEffect()
	{
		Debug.Log("TriggerSpecialEffect called");
		NPC[] teamNPC = MatchManager.Instance.GetTeamNPC();
		for (int i = 0; i < teamNPC.Length; i++)
		{
			teamNPC[i]?.NPCItem?.Anim?.SetTrigger("ability");
		}
		phantomArmor.SetActive(value: true);
		vfx.SetActive(value: true);
	}

	public void SpecialEffectFinish()
	{
		phantomArmor.SetActive(value: false);
		vfx.SetActive(value: false);
	}

	public bool IsSpecialEffectFinished()
	{
		return !phantomArmor.activeSelf;
	}
}
