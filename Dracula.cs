using System;
using System.Collections;
using UnityEngine;

public class Dracula : BossNPC
{
	private const string NPC_SWAP_VFX_NAME = "lightningstrike";

	private const string TRANSFORMATION_VFX_NAME = "CountTransitionVFX";

	public int _nextSlotJumpStep = 1;

	private float _transformationHealthThreshold = 0.4f;

	private bool transformed;

	public Dracula(NPC npc)
		: base(npc)
	{
		MatchManager instance = MatchManager.Instance;
		instance.OnLoadCombatFinished = (Action)Delegate.Combine(instance.OnLoadCombatFinished, new Action(SetupCombatStartEffects));
		MatchManager instance2 = MatchManager.Instance;
		instance2.OnCardCastByHeroBegins = (Action<Hero, CardData>)Delegate.Combine(instance2.OnCardCastByHeroBegins, new Action<Hero, CardData>(SwapPosition));
		MatchManager.OnCharacterDamaged = (Action<Character, int, int>)Delegate.Combine(MatchManager.OnCharacterDamaged, new Action<Character, int, int>(OnCharacterDamaged));
		MatchManager.Instance.DoComic(npc, Texts.Instance.GetText("fluffCountDialogStart"), 9f);
	}

	public override void Dispose()
	{
		base.Dispose();
		MatchManager instance = MatchManager.Instance;
		instance.OnLoadCombatFinished = (Action)Delegate.Remove(instance.OnLoadCombatFinished, new Action(SetupCombatStartEffects));
		MatchManager instance2 = MatchManager.Instance;
		instance2.OnCardCastByHeroBegins = (Action<Hero, CardData>)Delegate.Remove(instance2.OnCardCastByHeroBegins, new Action<Hero, CardData>(SwapPosition));
		MatchManager.OnCharacterDamaged = (Action<Character, int, int>)Delegate.Remove(MatchManager.OnCharacterDamaged, new Action<Character, int, int>(OnCharacterDamaged));
	}

	private void SetupCombatStartEffects()
	{
		NPC[] teamNPC = MatchManager.Instance.GetTeamNPC();
		for (int i = 0; i < teamNPC.Length; i++)
		{
			if (teamNPC[i] != null && teamNPC[i].Alive && teamNPC[i].InternalId != npc.InternalId)
			{
				teamNPC[i].SetAura(null, Globals.Instance.GetAuraCurseData("invulnerableunremovable"), 1);
			}
		}
	}

	private void SwapPosition(Hero arg1, CardData arg2)
	{
		int position = npc.Position;
		int nPCIndex = npc.NPCIndex;
		int position2 = (position + _nextSlotJumpStep) % 4;
		int num = (nPCIndex + _nextSlotJumpStep) % 4;
		NPC[] teamNPC = MatchManager.Instance.GetTeamNPC();
		NPC nPC = teamNPC[num];
		MatchManager.Instance.SwapNPCDeck(nPCIndex, num);
		MatchManager.Instance.SwapNPCDeckDiscard(nPCIndex, num);
		MatchManager.Instance.SwapNPCHand(nPCIndex, num);
		MatchManager.Instance.SwapCharacterOrder(npc.Id, nPC.Id, nPCIndex, num);
		nPC.Position = npc.Position;
		nPC.NPCIndex = npc.NPCIndex;
		npc.Position = position2;
		npc.NPCIndex = num;
		NPC[] array = teamNPC;
		int num2 = nPCIndex;
		int num3 = num;
		NPC nPC2 = teamNPC[num];
		NPC nPC3 = teamNPC[nPCIndex];
		array[num2] = nPC2;
		teamNPC[num3] = nPC3;
		for (int i = 0; i < teamNPC.Length; i++)
		{
			if (i == nPCIndex || i == num)
			{
				EffectsManager.Instance.PlayEffect("lightningstrike", teamNPC[i].NPCItem.CharImageT);
			}
			teamNPC[i].NPCItem.SetPosition(instant: true);
		}
		Transform target = MatchManager.Instance.GetTarget();
		if (target != null)
		{
			if (target == nPC.GO.transform)
			{
				MatchManager.Instance.SetTarget(npc.GO.transform);
			}
			else if (target == npc.GO.transform)
			{
				MatchManager.Instance.SetTarget(nPC.GO.transform);
			}
		}
		_nextSlotJumpStep = 3 - _nextSlotJumpStep;
	}

	public override void OnCharacterDamaged(Character character, int damage, int hpCurrent)
	{
		if ((bool)character.NpcData && character.NpcData.Id.StartsWith("count"))
		{
			DoDraculaTransformation(character);
		}
	}

	public void DoDraculaTransformation(Character character, bool instantTransition = false)
	{
		if (!((float)character.HpCurrent / (float)character.GetMaxHP() < _transformationHealthThreshold) || transformed)
		{
			return;
		}
		transformed = true;
		CharacterGOItem[] array = UnityEngine.Object.FindObjectsOfType<CharacterGOItem>();
		foreach (CharacterGOItem characterGOItem in array)
		{
			if (characterGOItem.name.Contains("count"))
			{
				character.TransformedModel = true;
				character.NPCItem.Init(MatchManager.Instance.GetTeamNPC()[character.Position], useAltModels: true);
				character.SpriteSpeed = character.NpcData.SpriteSpeedAlternate;
				character.SpritePortrait = character.NpcData.SpritePortraitAlternate;
				if (instantTransition)
				{
					GameManager.Instance.StartCoroutine(DoDraculaTransitionInstant(characterGOItem, character));
				}
				else
				{
					GameManager.Instance.StartCoroutine(DoDraculaTransitionSequence(characterGOItem, character));
				}
				MatchManager.Instance.SortCharacterSprites();
				MatchManager.Instance.SetInitiatives();
				MatchManager.Instance.RepositionCharacters();
				if (!instantTransition)
				{
					GameObject obj = UnityEngine.Object.Instantiate(Globals.Instance.GetResourceEffect("CountTransitionVFX"), MatchManager.Instance.GetTeamNPC()[character.Position].NPCItem.CharImageT.position + new Vector3(0.75f, 0f, 0f), Quaternion.identity);
					UnityEngine.Object.Instantiate(MatchManager.Instance.VignetteSprite, MatchManager.Instance.GetTeamNPC()[character.Position].NPCItem.CharImageT.position, Quaternion.identity);
					EffectsManager.Instance.PlayEffect("CountTransitionVFX", MatchManager.Instance.GetTeamNPC()[character.Position].NPCItem.CharImageT, isHero: false, castInCenter: false);
					UnityEngine.Object.Destroy(obj, 10f);
					UnityEngine.Object.Destroy(characterGOItem.gameObject, 10f);
				}
				else
				{
					UnityEngine.Object.Destroy(characterGOItem.gameObject);
				}
			}
		}
	}

	private IEnumerator DoDraculaTransitionSequence(CharacterGOItem humanDracula, Character beastDracula)
	{
		GameManager.Instance.DisableCardCast = true;
		GameManager.Instance.ConfigAutoEnd = false;
		SetDraculaSpriteAlpha(beastDracula, 0);
		yield return new WaitForSeconds(1f);
		beastDracula.NPCItem.animatedTransform.gameObject.SetActive(value: true);
		yield return new WaitForSeconds(0.1f);
		humanDracula.GetComponent<Animator>().SetTrigger("transitionOut");
		beastDracula.NPCItem.Anim.SetTrigger("transitionIn");
		beastDracula.SetAuraTrait(beastDracula, "powerful", 3);
		beastDracula.SetAuraTrait(beastDracula, "fury", 6);
		yield return new WaitForSeconds(0.01f);
		SetDraculaSpriteAlpha(beastDracula, 1);
		yield return new WaitForSeconds(7f);
		GameManager.Instance.DisableCardCast = false;
		if (SaveManager.PrefsHasKey("autoEnd"))
		{
			GameManager.Instance.ConfigAutoEnd = SaveManager.LoadPrefsBool("autoEnd");
		}
		MatchManager.Instance.RedrawCardsBorder();
	}

	private IEnumerator DoDraculaTransitionInstant(CharacterGOItem humanDracula, Character beastDracula)
	{
		yield return new WaitForSeconds(0.1f);
		SetDraculaSpriteAlpha(beastDracula, 0);
		beastDracula.NPCItem.animatedTransform.gameObject.SetActive(value: true);
		yield return new WaitForSeconds(0.1f);
		SetDraculaSpriteAlpha(beastDracula, 1);
	}

	private void SetDraculaSpriteAlpha(Character beastDracula, int alpha)
	{
		foreach (SpriteRenderer animatedSprite in beastDracula.NPCItem.animatedSprites)
		{
			animatedSprite.enabled = alpha == 1;
		}
	}
}
