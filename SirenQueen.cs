using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class SirenQueen : BossNPC, IDisposable
{
	public class QueenStateData
	{
		public int HpCurrent;

		public List<string> AuraCurseIds;

		public List<int> AuraCurseCharges;

		public List<string> EnchantmentIds;

		public List<int> EnchantmentExecuted;

		public QueenStateData()
		{
			HpCurrent = 0;
			AuraCurseIds = new List<string>();
			AuraCurseCharges = new List<int>();
			EnchantmentIds = new List<string>();
			EnchantmentExecuted = new List<int>();
		}

		public void Save(NPC npc, int hp, List<Aura> auraList, string enchant1, string enchant2, string enchant3)
		{
			AuraCurseIds.Clear();
			AuraCurseCharges.Clear();
			EnchantmentIds.Clear();
			HpCurrent = hp;
			foreach (Aura aura in auraList)
			{
				AuraCurseIds.Add(aura.ACData.Id);
				AuraCurseCharges.Add(aura.AuraCharges);
			}
			EnchantmentIds.Add(enchant1);
			EnchantmentIds.Add(enchant2);
			EnchantmentIds.Add(enchant3);
			EnchantmentExecuted.Add(MatchManager.Instance.EnchantmentExecutedTimes(npc.Id, enchant1));
			EnchantmentExecuted.Add(MatchManager.Instance.EnchantmentExecutedTimes(npc.Id, enchant2));
			EnchantmentExecuted.Add(MatchManager.Instance.EnchantmentExecutedTimes(npc.Id, enchant3));
		}

		public void Apply(NPC npc)
		{
			npc.HpCurrent = HpCurrent;
			for (int i = 0; i < AuraCurseIds.Count; i++)
			{
				npc.SetAuraTrait(null, AuraCurseIds[i], AuraCurseCharges[i]);
			}
			bool flag = false;
			if (!EnchantmentIds[0].IsNullOrEmpty())
			{
				npc.AssignEnchantmentManual(EnchantmentIds[0], 0);
				MatchManager.Instance.enchantmentExecutedTotal.Add(npc.Id + "_" + EnchantmentIds[0], EnchantmentExecuted[0]);
				flag = true;
			}
			if (!EnchantmentIds[1].IsNullOrEmpty())
			{
				npc.AssignEnchantmentManual(EnchantmentIds[1], 1);
				MatchManager.Instance.enchantmentExecutedTotal.Add(npc.Id + "_" + EnchantmentIds[1], EnchantmentExecuted[1]);
				flag = true;
			}
			if (!EnchantmentIds[2].IsNullOrEmpty())
			{
				npc.AssignEnchantmentManual(EnchantmentIds[2], 2);
				MatchManager.Instance.enchantmentExecutedTotal.Add(npc.Id + "_" + EnchantmentIds[2], EnchantmentExecuted[2]);
				flag = true;
			}
			if (flag)
			{
				npc.NPCItem.ShowEnchantments();
			}
		}
	}

	private NPCData npcData;

	private string[] teamNpcInitial = new string[4];

	private float healthThreshold;

	private string SirenQueenID = "s_queen";

	private const string SpawnFx = "stealth";

	public bool hasAppeared;

	public bool isEscaped;

	public bool isSpawning;

	private bool isDead;

	public bool loadDataOnNextSpawn;

	private bool repeatCardCast;

	private bool isCoolingDown;

	private IEnumerator onCharacterKilledCo;

	private IEnumerator onCharacterDamagedCo;

	private Coroutine sirenQueenCooldownCo;

	private List<string> npcDeck;

	private List<string> npcHand;

	private List<string> discardPile;

	public QueenStateData queenStateData = new QueenStateData();

	public SirenQueen(NPC npc = null, string npcID = "")
		: base(npc)
	{
		if (npc == null)
		{
			SirenQueenID = ((!npcID.IsNullOrEmpty()) ? npcID : "s_queen");
			npcData = Globals.Instance.GetNPC(SirenQueenID);
			base.npc = new NPC();
			base.npc.NpcData = npcData;
			base.npc.InitData();
		}
		healthThreshold = (float)base.npc.GetMaxHP() * 0.75f;
		GetInitialNpcTeam();
	}

	private void GetInitialNpcTeam()
	{
		NPCData[] nPCList = AtOManager.Instance.GetCurrentCombatData().NPCList;
		for (int i = 0; i < nPCList.Length; i++)
		{
			teamNpcInitial[i] = ((nPCList[i] == null) ? "" : nPCList[i].Id);
		}
	}

	public override void OnCharacterKilled(NPCData npcData, HeroData heroData, int position)
	{
		Globals.Instance.StartCoroutine(OnCharacterKilledCo(npcData, heroData, position));
	}

	private IEnumerator OnCharacterKilledCo(NPCData npcData, HeroData heroData, int position)
	{
		if (isDead)
		{
			yield break;
		}
		if (npcData != null && npcData.Id == SirenQueenID)
		{
			isDead = true;
		}
		else
		{
			if (!(npcData != null) || !(npcData.Id != SirenQueenID) || isDead)
			{
				yield break;
			}
			yield return new WaitForSeconds(0.5f);
			MatchManager.Instance.GetTeamNPC()[position] = null;
			MatchManager.Instance.NPCDiscardPileClear(position);
			if (IsThisBossSpawned() || isSpawning)
			{
				yield break;
			}
			isSpawning = true;
			if (hasAppeared && npc.HpCurrent > 0)
			{
				Globals.Instance.WaitForSeconds(0.65f);
				RespawnBoss(position);
				Globals.Instance.WaitForSeconds(0.5f);
				if (MatchManager.Instance.GetCharacterActive() != null && !MatchManager.Instance.GetCharacterActive().IsHero)
				{
					MatchManager.Instance.NextTurnFunction();
				}
				yield break;
			}
			hasAppeared = true;
			isEscaped = false;
			Globals.Instance.WaitForSeconds(0.65f);
			MatchManager.Instance.CreateNPC(Globals.Instance.GetNPC(SirenQueenID), "stealth", position, generateFromReload: false, "", null, "", 0.65f);
			isSpawning = false;
			npc = MatchManager.Instance.GetTeamNPC()[position];
			npc.NPCIndex = position;
			npc.Position = position;
			if (loadDataOnNextSpawn)
			{
				queenStateData.Apply(npc);
				loadDataOnNextSpawn = false;
			}
			Globals.Instance.WaitForSeconds(0.7f);
			if (MatchManager.Instance.GetCharacterActive() != null && !MatchManager.Instance.GetCharacterActive().IsHero)
			{
				MatchManager.Instance.NextTurnFunction();
			}
			npcDeck = MatchManager.Instance?.GetNPCDeck(npc.NPCIndex);
			npcHand = new List<string>();
			EffectsManager.Instance.PlayEffect("stealth", MatchManager.Instance.GetTeamNPC()[position].NPCItem.CharImageT, isHero: false, castInCenter: false);
		}
	}

	public override void OnCharacterDamaged(Character character, int damage, int hpCurrent)
	{
		onCharacterDamagedCo = OnCharacterDamagedCo(character, damage, hpCurrent);
		GameManager.Instance.StartCoroutine(onCharacterDamagedCo);
	}

	private IEnumerator OnCharacterDamagedCo(Character character, int damage, int hpCurrent)
	{
		if (repeatCardCast)
		{
			yield return null;
		}
		if (!(character.NPCItem != null) || !(character.NpcData != null) || !(character.NpcData.Id == SirenQueenID) || !((float)character.HpCurrent < GetHealthThreshold(hpCurrent - damage)) || isCoolingDown)
		{
			yield break;
		}
		if (sirenQueenCooldownCo != null)
		{
			GameManager.Instance.StopCoroutine(sirenQueenCooldownCo);
		}
		sirenQueenCooldownCo = GameManager.Instance.StartCoroutine(SirenQueenCooldown(2f));
		isCoolingDown = true;
		if (character.NPCItem.IsDying || isEscaped || character.HpCurrent <= 0)
		{
			yield break;
		}
		isEscaped = true;
		npc.DiscardHand();
		int nPCIndex = character.NPCIndex;
		npcDeck = MatchManager.Instance.GetNPCDeck(nPCIndex);
		npcHand = MatchManager.Instance.GetNPCHand(nPCIndex);
		discardPile = MatchManager.Instance.GetNPCDiscardDeck(nPCIndex);
		MatchManager.Instance.GetTeamNPC()[nPCIndex] = null;
		SetCharacterVisible(state: false);
		if (teamNpcInitial[nPCIndex].IsNullOrEmpty())
		{
			yield break;
		}
		MatchManager.Instance.CreateNPC(Globals.Instance.GetNPC(teamNpcInitial[nPCIndex]), "stealth", nPCIndex, generateFromReload: false, "", null, "", 0.5f);
		EffectsManager.Instance.PlayEffect("stealth", MatchManager.Instance.GetTeamNPC()[nPCIndex].NPCItem.CharImageT, isHero: false, castInCenter: false, 0.5f);
		int num = LeftMostEmptyIndexInTeamNpc();
		if (num > -1)
		{
			RespawnBoss(num);
			if (!MatchManager.Instance.GetCharacterActive().IsHero)
			{
				MatchManager.Instance.NextTurnFunction();
			}
		}
	}

	private IEnumerator SirenQueenCooldown(float time = 0.5f)
	{
		yield return new WaitForSeconds(time);
		isCoolingDown = false;
	}

	private int LeftMostEmptyIndexInTeamNpc()
	{
		NPC[] teamNPC = MatchManager.Instance.GetTeamNPC();
		for (int i = 0; i < teamNPC.Length; i++)
		{
			if ((teamNPC[i] == null || !teamNPC[i].Alive) && !teamNpcInitial[i].IsNullOrEmpty())
			{
				return i;
			}
		}
		return -1;
	}

	private void RespawnBoss(int position)
	{
		isSpawning = false;
		isEscaped = false;
		npc.RoundMoved = MatchManager.Instance.GetCurrentRound();
		MatchManager.Instance.GetTeamNPC()[position] = npc;
		npc.NPCIndex = position;
		npc.Position = position;
		npc.NPCItem.SetPosition(instant: true, position);
		SetCharacterVisible(state: true);
		MatchManager.Instance.SetInitiatives();
		MatchManager.Instance.ReDrawInitiatives();
		MatchManager.Instance.RepositionCharacters();
		MatchManager.Instance.SetNPCDeck(npc.NPCIndex, npcDeck);
		MatchManager.Instance.SetNPCHand(npc.NPCIndex, npcHand);
		MatchManager.Instance.SetNPCDiscardDeck(npc.NPCIndex, discardPile);
		EffectsManager.Instance.PlayEffect("stealth", MatchManager.Instance.GetTeamNPC()[position].NPCItem.CharImageT, isHero: false, castInCenter: false);
	}

	private bool IsThisBossSpawned()
	{
		NPC[] teamNPC = MatchManager.Instance.GetTeamNPC();
		foreach (NPC nPC in teamNPC)
		{
			if (nPC != null && nPC.Alive && nPC.NpcData.Id == SirenQueenID)
			{
				return true;
			}
		}
		return false;
	}

	private float GetHealthThreshold(int currHp)
	{
		int maxHP = npc.GetMaxHP();
		if ((double)currHp < (double)maxHP * 0.25)
		{
			return 0f;
		}
		if ((double)currHp < (double)maxHP * 0.5)
		{
			return (float)maxHP * 0.25f;
		}
		if ((double)currHp < (double)maxHP * 0.75)
		{
			return (float)maxHP * 0.5f;
		}
		return (float)maxHP * 0.75f;
	}

	private void SetCharacterVisible(bool state)
	{
		try
		{
			npc.NPCItem.CharImageT.gameObject.SetActive(state);
		}
		catch
		{
			Debug.Log("CharImageT not found");
		}
		try
		{
			npc.NPCItem.CharImageShadowT.gameObject.SetActive(state);
		}
		catch
		{
			Debug.Log("CharImageShadowT not found");
		}
		try
		{
			npc.NPCItem.healthBar.gameObject.SetActive(state);
		}
		catch
		{
			Debug.Log("healthBar not found");
		}
		if (state)
		{
			npc.NPCItem.ShowEnchantments();
			return;
		}
		try
		{
			npc.NPCItem.HideEnchatmentIcons();
			npc.NPCItem.ActivateMark(state: false);
		}
		catch
		{
		}
	}

	public override bool IsValidTarget()
	{
		return !isEscaped;
	}

	public override void OnRepeatCardCast()
	{
		repeatCardCast = true;
	}

	public override void OnRepeatCardIterationsFinished()
	{
		repeatCardCast = false;
		OnCharacterDamaged(npc, 0, 0);
	}
}
