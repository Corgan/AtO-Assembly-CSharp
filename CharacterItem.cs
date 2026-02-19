using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

public class CharacterItem : MonoBehaviour
{
	public SpriteOutline spriteOutline;

	[SerializeField]
	private Transform charImageT;

	[SerializeField]
	private Transform charImageShadowT;

	[SerializeField]
	private TMP_Text hpText;

	[SerializeField]
	private Hero _hero;

	[SerializeField]
	private NPC _npc;

	[SerializeField]
	private Animator anim;

	public Animator animPet;

	private PopupSheet popupSheet;

	private Coroutine KillCoroutine;

	public Transform characterTransform;

	public Transform keyTransform;

	public Transform keyRed;

	public Transform keyBackground;

	public TMP_Text keyNumber;

	public ItemCombatIcon iconEnchantment;

	public ItemCombatIcon iconEnchantment2;

	public ItemCombatIcon iconEnchantment3;

	public Transform healthBar;

	public Transform hpRed;

	public Transform hpPoison;

	public Transform hpBleed;

	public Transform hpRegen;

	public SpriteRenderer hpSR;

	public Transform activeMarkTR;

	public Transform hpT;

	public Transform skull;

	public ParticleSystem skullParticle;

	public TMP_Text purgedispel;

	public TMP_Text purgedispelTitle;

	public TMP_Text purgedispelQuantity;

	public TMP_Text overDebuff;

	public TMP_Text hpBlockText;

	public Transform hpBlockT;

	public Transform hpBackground;

	public Transform hpBackgroundHigh;

	public Transform hpBlockIconT;

	public Transform blockBorderT;

	public TMP_Text hpShieldText;

	public Transform hpShieldT;

	public Transform hpDoomIconT;

	public TMP_Text hpDoomText;

	public TMP_Text dmgPreviewText;

	public Transform thornsTransform;

	public Transform tauntTextTransform;

	private TMP_Text tauntTextT;

	public GameObject GO_Buffs;

	private List<Buff> GoBuffs = new List<Buff>();

	public GameObject GO_Taunt;

	public GameObject BuffPrefab;

	public Shader energyDefaultShader;

	public Shader energyShader;

	public ParticleSystem trailParticle;

	public ParticleSystem stealthParticle;

	public Material defaultMaterial;

	public Material stealthMaterial;

	public Material paralyzeMaterial;

	public Material tauntMaterial;

	public GameObject combatTextPrefab;

	private CombatText CT;

	private SpriteRenderer charImageSR;

	private bool isHero;

	public bool IsDying;

	private Color colorFade;

	private Vector3 vectorFade;

	private Vector3 originalLocalPosition;

	private bool charIsMoving;

	public Transform energyT;

	public TMP_Text energyTxt;

	private Transform[] energyArr;

	private SpriteRenderer[] energySR;

	private Animator[] energySRAnimator;

	private EnergyPoint[] energyPoint;

	internal List<SpriteRenderer> animatedSprites;

	private Dictionary<string, Material> animatedSpritesDefaultMaterial;

	private List<SetSpriteLayerFromBase> animatedSpritesOutOfCharacter;

	private Transform shadowSprite;

	public Transform heroDecks;

	public TMP_Text heroDecksCounter;

	public TMP_Text heroDecksDeckText;

	public TMP_Text heroDecksDeckTextBg;

	public TMP_Text heroDecksDiscardText;

	public TMP_Text heroDecksDiscardTextBg;

	private Coroutine moveCenterCo;

	private Coroutine moveBackCo;

	private Coroutine helpCo;

	private Coroutine blockCo;

	private Coroutine hitCo;

	private bool characterBeingHitted;

	private Dictionary<string, int> buffAnimationList = new Dictionary<string, int>();

	private Coroutine buffAnimationCo;

	public Transform transformForCombatText;

	private bool isActive;

	public float heightModel;

	private int petMagnusCounter;

	private Coroutine petMagnusCoroutine;

	private int petMagnusAnswer;

	private int petYoggerCounter;

	private Coroutine petYoggerCoroutine;

	private int petYoggerAnswer;

	public NPCItem PetItem;

	public bool PetItemFront = true;

	public NPCItem PetItemEnchantment;

	public bool PetItemEnchantmentFront = true;

	private Coroutine drawBuffsCoroutine;

	private int counterEffectItemOwner;

	private int indexEffectItemOwner;

	public EmoteCharacterPing emoteCharacterPing;

	private bool animationBusy;

	private Coroutine animationBusyCo;

	private GameObject[] swordSprites;

	public SpriteRenderer CharImageSR
	{
		get
		{
			return charImageSR;
		}
		set
		{
			charImageSR = value;
		}
	}

	public Transform CharImageT
	{
		get
		{
			return charImageT;
		}
		set
		{
			charImageT = value;
		}
	}

	public Transform CharImageShadowT
	{
		get
		{
			return charImageShadowT;
		}
		set
		{
			charImageShadowT = value;
		}
	}

	public TMP_Text HpText
	{
		get
		{
			return hpText;
		}
		set
		{
			hpText = value;
		}
	}

	public bool IsHero
	{
		get
		{
			return isHero;
		}
		set
		{
			isHero = value;
		}
	}

	public Hero Hero
	{
		get
		{
			return _hero;
		}
		set
		{
			_hero = value;
		}
	}

	public NPC NPC
	{
		get
		{
			return _npc;
		}
		set
		{
			_npc = value;
		}
	}

	public Animator Anim
	{
		get
		{
			return anim;
		}
		set
		{
			anim = value;
		}
	}

	public GameObject[] SwordSprites
	{
		get
		{
			return swordSprites;
		}
		set
		{
			swordSprites = value;
		}
	}

	public virtual void Awake()
	{
		animatedSprites = new List<SpriteRenderer>();
		animatedSpritesDefaultMaterial = new Dictionary<string, Material>();
		animatedSpritesOutOfCharacter = new List<SetSpriteLayerFromBase>();
		colorFade = new Color(0f, 0f, 0f, 0.02f);
		vectorFade = new Vector3(0.001f, 0f, 0f);
		if (!(charImageT == null))
		{
			charImageSR = charImageT.GetComponent<SpriteRenderer>();
			GameObject gameObject = Object.Instantiate(combatTextPrefab, Vector3.zero, Quaternion.identity, charImageT);
			CT = gameObject.GetComponent<CombatText>();
			if ((bool)MatchManager.Instance)
			{
				popupSheet = MatchManager.Instance.popupSheet;
			}
			energyArr = new Transform[10];
			energyArr[0] = energyT;
			energyPoint = new EnergyPoint[10];
			energyPoint[0] = energyT.GetComponent<EnergyPoint>();
			energySR = new SpriteRenderer[10];
			energySR[0] = energyT.GetComponent<SpriteRenderer>();
			energySRAnimator = new Animator[10];
			energySRAnimator[0] = energyT.GetComponent<Animator>();
			tauntTextTransform.gameObject.SetActive(value: false);
			tauntTextT = tauntTextTransform.GetComponent<TMP_Text>();
			thornsTransform.gameObject.SetActive(value: false);
			skull.gameObject.SetActive(value: false);
			Vector3 localPosition = energyT.localPosition;
			for (int i = 1; i < 10; i++)
			{
				GameObject gameObject2 = Object.Instantiate(energyT.gameObject, energyT.parent);
				gameObject2.transform.localPosition = new Vector3(localPosition.x + 0.11f * (float)i, localPosition.y, localPosition.z - 0.01f * (float)i);
				energyArr[i] = gameObject2.transform;
				energyPoint[i] = gameObject2.transform.GetComponent<EnergyPoint>();
				energySR[i] = gameObject2.transform.GetComponent<SpriteRenderer>();
				energySRAnimator[i] = gameObject2.transform.GetComponent<Animator>();
			}
			for (int j = 0; j < 20; j++)
			{
				GameObject gameObject3 = Object.Instantiate(BuffPrefab, Vector3.zero, Quaternion.identity, GO_Buffs.transform);
				GoBuffs.Add(gameObject3.GetComponent<Buff>());
				GoBuffs[j].CleanBuff();
			}
			if (keyTransform != null && keyTransform.gameObject.activeSelf)
			{
				keyTransform.gameObject.SetActive(value: false);
			}
		}
	}

	public virtual void Start()
	{
	}

	public List<string> CalculateDamagePrePostForThisCharacter()
	{
		Character character = null;
		character = ((_hero == null) ? ((Character)_npc) : ((Character)_hero));
		if (MatchManager.Instance.prePostDamageDictionary != null && character != null && MatchManager.Instance.prePostDamageDictionary.ContainsKey(character.Id))
		{
			return MatchManager.Instance.prePostDamageDictionary[character.Id];
		}
		int num = 0;
		int num2 = 0;
		bool flag = false;
		bool flag2 = false;
		int num3 = character.GetHp();
		int auraCharges = character.GetAuraCharges("block");
		int auraCharges2 = character.GetAuraCharges("shield");
		int num4 = 0;
		bool flag3 = false;
		string text = Globals.Instance.ClassColor["warrior"];
		string text2 = "#1B9604";
		string value = "#00A49E";
		string text3 = "";
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<size=+26><sprite name=skull></size><size=+5>");
		stringBuilder.Append(string.Format(Texts.Instance.GetText("characterDies"), character.SourceName));
		stringBuilder.Append("</size><size=0><br><br></size>");
		text3 = stringBuilder.ToString();
		if (character.RoundMoved >= MatchManager.Instance.GetCurrentRound())
		{
			flag3 = true;
		}
		List<string> list = new List<string>();
		list.Add("0");
		int num5 = 0;
		list.Add("0");
		int num6 = 0;
		list.Add("0");
		bool flag4 = true;
		for (int i = 0; i < character.AuraList.Count; i++)
		{
			if (i < character.AuraList.Count && character.AuraList[i] != null && character.AuraList[i].ACData != null && character.AuraList[i].ACData.NoRemoveBlockAtTurnEnd)
			{
				flag4 = false;
			}
		}
		if (!flag3)
		{
			num4 = auraCharges;
		}
		else
		{
			num4 = auraCharges2;
			if (!flag4)
			{
				num4 += auraCharges;
			}
		}
		StringBuilder stringBuilder2 = new StringBuilder();
		int num7 = 0;
		Character characterActive = MatchManager.Instance.GetCharacterActive();
		for (int j = 0; j < 2; j++)
		{
			switch (j)
			{
			case 0:
				num7 = ((characterActive != null && !(characterActive.Id != character.Id)) ? 1 : 0);
				break;
			case 1:
				num7 = ((characterActive == null || characterActive.Id != character.Id) ? 1 : 0);
				break;
			}
			for (int k = 0; k < character.AuraList.Count; k++)
			{
				if (character.AuraList[k].ACData == null)
				{
					continue;
				}
				AuraCurseData auraCurseData = AtOManager.Instance.GlobalAuraCurseModificationByTraitsAndItems("consume", character.AuraList[k].ACData.Id, character, null);
				if (auraCurseData == null)
				{
					auraCurseData = character.AuraList[k].ACData;
				}
				if (auraCurseData.ProduceHealWhenConsumed && ((auraCurseData.ConsumedAtTurnBegin && num7 == 0) || (auraCurseData.ConsumedAtTurn && num7 == 1)))
				{
					int num8 = 0;
					num8 += auraCurseData.HealWhenConsumed;
					num8 += Functions.FuncRoundToInt((float)character.AuraList[k].AuraCharges * auraCurseData.HealWhenConsumedPerCharge);
					if (num8 > 0)
					{
						num8 = character.HealReceivedFinal(num8);
						if (character.GetHpLeftForMax() < num8)
						{
							num8 = character.GetHpLeftForMax();
						}
						if (num8 > 0)
						{
							num3 += num8;
							if (num == 0 && num7 == 0)
							{
								stringBuilder2.Append("<size=-1><color=#FFF>");
								stringBuilder2.Append(Texts.Instance.GetText("preturnEffects"));
								stringBuilder2.Append("</color></size>");
								stringBuilder2.Append("<br>");
								num = 1;
							}
							if (num2 == 0 && num7 == 1)
							{
								stringBuilder2.Append("<size=-1><color=#FFF>");
								stringBuilder2.Append(Texts.Instance.GetText("postturnEffects"));
								stringBuilder2.Append("</color></size>");
								stringBuilder2.Append("<br>");
								num2 = 1;
							}
							stringBuilder2.Append("<color=");
							stringBuilder2.Append(value);
							stringBuilder2.Append("> ");
							stringBuilder2.Append("+");
							stringBuilder2.Append(num8);
							stringBuilder2.Append("  <sprite name=" + auraCurseData.Id);
							stringBuilder2.Append(">");
							stringBuilder2.Append("</color>");
							stringBuilder2.Append("<size=-1.5><voffset=2> <color=#666>|</color>   <voffset=0>");
							stringBuilder2.Append(num3);
							stringBuilder2.Append("  <sprite name=heart>");
							stringBuilder2.Append("</size>");
							stringBuilder2.Append("<br>");
							num5 += num8;
						}
					}
				}
				if (!auraCurseData.ProduceDamageWhenConsumed || ((!auraCurseData.ConsumedAtTurnBegin || num7 != 0) && (!auraCurseData.ConsumedAtTurn || num7 != 1)) || (auraCurseData.DamageWhenConsumed <= 0 && !(auraCurseData.DamageWhenConsumedPerCharge > 0f)))
				{
					continue;
				}
				int auraCharges3 = character.AuraList[k].AuraCharges;
				int num9 = 0;
				int num10 = 0;
				int num11 = 0;
				float num12 = 0f;
				num9 += auraCurseData.DamageWhenConsumed;
				int num13 = auraCharges3;
				if (auraCurseData.ConsumedDamageChargesBasedOnACCharges != null)
				{
					num13 = character.GetAuraCharges(auraCurseData.ConsumedDamageChargesBasedOnACCharges.Id);
				}
				if (auraCurseData.ConsumeDamageChargesIfACApplied != null && character.GetAuraCharges(auraCurseData.ConsumeDamageChargesIfACApplied.Id) <= 0)
				{
					num13 = 0;
				}
				num9 += Functions.FuncRoundToInt(auraCurseData.DamageWhenConsumedPerCharge * (float)num13);
				if (!IsHero && auraCurseData.Id == "scourge")
				{
					if (AtOManager.Instance.TeamHavePerk("mainperkscourge0b"))
					{
						num9 += character.GetAuraCharges("burn");
					}
					if (AtOManager.Instance.TeamHavePerk("mainperkscourge0c"))
					{
						num9 += character.GetAuraCharges("insane");
					}
				}
				if (auraCurseData.DoubleDamageIfCursesLessThan > 0 && character.GetAuraCurseTotal(_auras: false, _curses: true) < auraCurseData.DoubleDamageIfCursesLessThan)
				{
					num9 *= 2;
				}
				if (auraCurseData.DamageTypeWhenConsumed != Enums.DamageType.None)
				{
					num11 = ((num9 <= num4) ? num9 : num4);
					num9 -= num11;
					num4 -= num11;
					num12 = -1 * character.BonusResists(auraCurseData.DamageTypeWhenConsumed, character.AuraList[k].ACData.Id, num7 == 0, num7 == 1);
					num10 = Functions.FuncRoundToInt((float)num9 + (float)num9 * num12 * 0.01f);
				}
				else
				{
					num10 = num9;
				}
				num3 -= num10;
				if (num7 == 0)
				{
					num5 -= num10;
				}
				else
				{
					num6 -= num10;
				}
				if (num == 0 && num7 == 0)
				{
					stringBuilder2.Append("<size=-1><color=#FFF>");
					stringBuilder2.Append(Texts.Instance.GetText("preturnEffects"));
					stringBuilder2.Append("</color></size>");
					stringBuilder2.Append("<br>");
					num = 1;
				}
				if (num2 == 0 && num7 == 1)
				{
					stringBuilder2.Append("<size=-1><color=#FFF>");
					stringBuilder2.Append(Texts.Instance.GetText("postturnEffects"));
					stringBuilder2.Append("</color></size>");
					stringBuilder2.Append("<br>");
					num2 = 1;
				}
				if (num7 == 0)
				{
					stringBuilder2.Append("<color=");
					stringBuilder2.Append(text);
					stringBuilder2.Append("> ");
				}
				else
				{
					stringBuilder2.Append("<color=");
					stringBuilder2.Append(text2);
					stringBuilder2.Append("> ");
				}
				if (num10 != 0)
				{
					stringBuilder2.Append("-");
				}
				stringBuilder2.Append(num10);
				stringBuilder2.Append(" ");
				stringBuilder2.Append(" <sprite name=" + auraCurseData.Id);
				stringBuilder2.Append(">");
				stringBuilder2.Append("</color>");
				stringBuilder2.Append("<size=-1.5><voffset=2> <color=#666>|</color>   <voffset=0>");
				if (num11 > 0)
				{
					stringBuilder2.Append(num11);
					stringBuilder2.Append(" <sprite name=block> ");
				}
				int num14 = (num10 - num9) * -1;
				if (num14 != 0)
				{
					stringBuilder2.Append(num14);
					stringBuilder2.Append("  <sprite name=ui_resistance>  ");
				}
				stringBuilder2.Append(num3);
				stringBuilder2.Append("  <sprite name=heart>");
				stringBuilder2.Append("</size>");
				stringBuilder2.Append("<br>");
				if (num3 <= 0 && !flag)
				{
					stringBuilder2.Append(text3);
					if (num7 == 0)
					{
						flag2 = true;
					}
					flag = true;
				}
			}
		}
		list[0] = num5.ToString();
		list[1] = num6.ToString();
		if (flag)
		{
			list[2] = "1";
		}
		else
		{
			list[2] = "0";
		}
		if (skull != null)
		{
			if (list[2] == "1")
			{
				if (!skull.gameObject.activeSelf)
				{
					skull.gameObject.SetActive(value: true);
				}
				ParticleSystem.MainModule main = skullParticle.main;
				if (flag2)
				{
					main.startColor = Functions.HexToColor(text + "50");
				}
				else
				{
					main.startColor = Functions.HexToColor(text2 + "50");
				}
			}
			else if (skull.gameObject.activeSelf)
			{
				skull.gameObject.SetActive(value: false);
			}
		}
		if (stringBuilder2.Length > 0)
		{
			stringBuilder2.Append("<line-height=160%><space=62><sprite name=sepwhite>");
			stringBuilder2.Append("<line-height=16><br><line-height=100%><size=-1.5>");
			stringBuilder2.Append("<sprite name=block>");
			stringBuilder2.Append(Texts.Instance.GetText("blocked"));
			stringBuilder2.Append("   <sprite name=ui_resistance> ");
			stringBuilder2.Append(Texts.Instance.GetText("resisted"));
			stringBuilder2.Append("   <sprite name=heart>");
			stringBuilder2.Append(Texts.Instance.GetText("currentHp"));
			stringBuilder2.Append("</size>");
			stringBuilder2.Insert(0, "<size=+2>");
			list.Add(stringBuilder2.ToString());
		}
		if (!MatchManager.Instance.prePostDamageDictionary.ContainsKey(character.Id))
		{
			MatchManager.Instance.prePostDamageDictionary.Add(character.Id, list);
		}
		else
		{
			MatchManager.Instance.prePostDamageDictionary[character.Id] = list;
		}
		return list;
	}

	public void DeleteShadow(GameObject GO)
	{
		foreach (Transform item in GO.transform)
		{
			if ((bool)item.GetComponent<SpriteRenderer>() && item.gameObject.name.ToLower() == "shadow")
			{
				if (item.gameObject.activeSelf)
				{
					item.gameObject.SetActive(value: false);
				}
				break;
			}
		}
	}

	public void GetSwordSprites(GameObject GO)
	{
		if (GO.TryGetComponent<AmeliaSwords>(out var component))
		{
			swordSprites = new GameObject[5];
			for (int i = 0; i < component.swordSprites.Length; i++)
			{
				swordSprites[i] = component.swordSprites[i];
			}
		}
	}

	public void CleanSwordSprites()
	{
		if (!(_hero.SubclassName == "queen") || swordSprites == null)
		{
			return;
		}
		for (int i = 0; i < swordSprites.Length; i++)
		{
			if (swordSprites[i] != null && swordSprites[i].activeSelf)
			{
				ShowHideSwordSprite(i, state: false);
			}
		}
	}

	public void ShowHideSwordSprite(int i, bool state)
	{
		if (_hero.SubclassName == "queen" && swordSprites != null && swordSprites[i] != null && swordSprites[i].activeSelf != state)
		{
			swordSprites[i].SetActive(state);
		}
	}

	public void GetSpritesFromAnimated(GameObject GO, bool recursive = false)
	{
		foreach (Transform item in GO.transform)
		{
			if ((bool)item.GetComponent<SpriteRenderer>())
			{
				animatedSprites.Add(item.GetComponent<SpriteRenderer>());
				if (!animatedSpritesDefaultMaterial.ContainsKey(item.name))
				{
					animatedSpritesDefaultMaterial.Add(item.name, item.GetComponent<SpriteRenderer>().sharedMaterial);
				}
				if (item.gameObject.name.ToLower() == "shadow")
				{
					shadowSprite = item;
				}
			}
			if (item.GetComponent<SetSpriteLayerFromBase>() != null)
			{
				animatedSpritesOutOfCharacter.Add(item.GetComponent<SetSpriteLayerFromBase>());
			}
			if (item.childCount > 0)
			{
				GetSpritesFromAnimated(item.gameObject, recursive: true);
			}
		}
		if (!recursive)
		{
			charImageT = GO.transform;
		}
	}

	public void SetOverDebuff(string msg = "")
	{
		if (overDebuff == null)
		{
			return;
		}
		if (msg == "")
		{
			if (overDebuff.gameObject.activeSelf)
			{
				overDebuff.gameObject.SetActive(value: false);
			}
			return;
		}
		overDebuff.text = msg;
		if (!overDebuff.gameObject.activeSelf)
		{
			overDebuff.gameObject.SetActive(value: true);
		}
	}

	public void SetDoomIcon()
	{
		if (_hero != null)
		{
			int num = _hero.EffectCharges("doom");
			if (num > 0)
			{
				if (!hpDoomIconT.gameObject.activeSelf)
				{
					hpDoomIconT.gameObject.SetActive(value: true);
				}
				hpDoomText.text = num.ToString();
				hpDoomIconT.GetComponent<PopupAuraCurse>().SetAuraCurse(Globals.Instance.GetAuraCurseData("doom"), num, _fast: true);
				return;
			}
			if (hpDoomIconT.gameObject.activeSelf)
			{
				hpDoomIconT.gameObject.SetActive(value: false);
			}
		}
		if (_npc == null)
		{
			return;
		}
		int num2 = _npc.EffectCharges("doom");
		if (num2 > 0)
		{
			if (!hpDoomIconT.gameObject.activeSelf)
			{
				hpDoomIconT.gameObject.SetActive(value: true);
			}
			hpDoomText.text = num2.ToString();
			hpDoomIconT.GetComponent<PopupAuraCurse>().SetAuraCurse(Globals.Instance.GetAuraCurseData("doom"), num2, _fast: true);
		}
		else if (hpDoomIconT.gameObject.activeSelf)
		{
			hpDoomIconT.gameObject.SetActive(value: false);
		}
	}

	public bool IsItemStealth()
	{
		if (animatedSprites != null)
		{
			for (int i = 0; i < animatedSprites.Count; i++)
			{
				if (!animatedSprites[i].transform.GetComponent("StealthHide"))
				{
					return animatedSprites[i].sharedMaterial.name.Split(' ')[0] == "stealth";
				}
			}
		}
		return false;
	}

	public bool IsItemTaunt()
	{
		if (animatedSprites != null)
		{
			for (int i = 0; i < animatedSprites.Count; i++)
			{
				if (!animatedSprites[i].transform.GetComponent("StealthHide"))
				{
					return animatedSprites[i].sharedMaterial.name.Split(' ')[0] == "taunt";
				}
			}
		}
		return false;
	}

	public bool IsItemParalyzed()
	{
		if (animatedSprites != null)
		{
			for (int i = 0; i < animatedSprites.Count; i++)
			{
				if (!animatedSprites[i].transform.GetComponent("StealthHide"))
				{
					return animatedSprites[i].sharedMaterial == paralyzeMaterial;
				}
			}
		}
		return false;
	}

	public void SetStealth(bool state)
	{
		if ((state && IsItemStealth()) || (!state && !IsItemStealth()) || animatedSprites == null || animatedSprites.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < animatedSprites.Count; i++)
		{
			if (state)
			{
				if ((bool)animatedSprites[i].transform.GetComponent("StealthHide"))
				{
					if (animatedSprites[i].gameObject.activeSelf)
					{
						animatedSprites[i].transform.gameObject.SetActive(value: false);
					}
				}
				else
				{
					animatedSprites[i].sharedMaterial = stealthMaterial;
				}
			}
			else if ((bool)animatedSprites[i].transform.GetComponent("StealthHide"))
			{
				if (!animatedSprites[i].gameObject.activeSelf)
				{
					animatedSprites[i].transform.gameObject.SetActive(value: true);
				}
			}
			else
			{
				animatedSprites[i].sharedMaterial = animatedSpritesDefaultMaterial[animatedSprites[i].name];
			}
		}
		if (state)
		{
			if (shadowSprite != null && shadowSprite.gameObject.activeSelf)
			{
				shadowSprite.gameObject.SetActive(value: false);
			}
			if (stealthParticle != null && !stealthParticle.gameObject.activeSelf)
			{
				stealthParticle.gameObject.SetActive(value: true);
			}
		}
		else
		{
			if (shadowSprite != null && !shadowSprite.gameObject.activeSelf)
			{
				shadowSprite.gameObject.SetActive(value: true);
			}
			if (stealthParticle != null && stealthParticle.gameObject.activeSelf)
			{
				stealthParticle.gameObject.SetActive(value: false);
			}
		}
	}

	public void SetTaunt(bool state)
	{
		if ((state && IsItemTaunt()) || (!state && !IsItemTaunt()))
		{
			return;
		}
		if (animatedSprites != null && animatedSprites.Count > 0)
		{
			for (int i = 0; i < animatedSprites.Count; i++)
			{
				if (state)
				{
					animatedSprites[i].sharedMaterial = tauntMaterial;
				}
				else
				{
					animatedSprites[i].sharedMaterial = animatedSpritesDefaultMaterial[animatedSprites[i].name];
				}
			}
		}
		else if (state)
		{
			charImageSR.sharedMaterial = tauntMaterial;
		}
		else
		{
			charImageSR.sharedMaterial = animatedSpritesDefaultMaterial[charImageSR.name];
		}
	}

	public void SetParalyze(bool state)
	{
		if (state && IsItemParalyzed())
		{
			return;
		}
		if (!state && !IsItemParalyzed())
		{
			if (anim != null)
			{
				anim.speed = 1f;
			}
			if (IsItemStealth() || IsItemTaunt())
			{
				return;
			}
		}
		if (animatedSprites != null && animatedSprites.Count > 0)
		{
			int num = 0;
			for (int i = 0; i < animatedSprites.Count && !animatedSprites[i].gameObject.activeSelf; i++)
			{
				num++;
			}
			if (num == animatedSprites.Count || (state && animatedSprites[num].sharedMaterial == paralyzeMaterial) || (!state && animatedSprites[num].sharedMaterial == animatedSpritesDefaultMaterial[animatedSprites[num].name]))
			{
				return;
			}
			if (state)
			{
				if (anim.speed > 0f)
				{
					anim.SetTrigger("hit");
					StartCoroutine(StopAnim());
				}
			}
			else
			{
				anim.speed = 1f;
			}
			for (int j = 0; j < animatedSprites.Count; j++)
			{
				if (state)
				{
					if ((bool)animatedSprites[j].transform.GetComponent("StealthHide"))
					{
						if (animatedSprites[j].gameObject.activeSelf)
						{
							animatedSprites[j].transform.gameObject.SetActive(value: false);
						}
					}
					else
					{
						animatedSprites[j].sharedMaterial = paralyzeMaterial;
					}
				}
				else if ((bool)animatedSprites[j].transform.GetComponent("StealthHide"))
				{
					if (!animatedSprites[j].gameObject.activeSelf)
					{
						animatedSprites[j].transform.gameObject.SetActive(value: true);
					}
				}
				else
				{
					animatedSprites[j].sharedMaterial = animatedSpritesDefaultMaterial[animatedSprites[j].name];
				}
			}
		}
		else if (state)
		{
			charImageSR.sharedMaterial = paralyzeMaterial;
		}
		else
		{
			charImageSR.sharedMaterial = animatedSpritesDefaultMaterial[charImageSR.name];
		}
		if (!state && shadowSprite != null && !shadowSprite.gameObject.activeSelf)
		{
			shadowSprite.gameObject.SetActive(value: true);
		}
	}

	private IEnumerator StopAnim()
	{
		if (anim.speed == 0f)
		{
			yield break;
		}
		for (int i = 0; i < 100; i++)
		{
			yield return null;
			anim.speed *= 0.92f;
			if (anim.speed < 0.01f)
			{
				anim.speed = 0f;
				break;
			}
		}
	}

	public void SetDamagePreview(bool state, int dmg = 0, string dmgType = "", int dmg2 = 0, string dmgType2 = "", int heal = 0, int blocked = 0, CardData _cardData = null)
	{
		if (dmgPreviewText == null)
		{
			return;
		}
		if (!state)
		{
			if (dmgPreviewText.transform.gameObject.activeSelf)
			{
				dmgPreviewText.transform.gameObject.SetActive(value: false);
			}
			if (purgedispel.transform.gameObject.activeSelf)
			{
				purgedispel.transform.gameObject.SetActive(value: false);
			}
			AmplifyBuffs(null);
			if (_npc != null)
			{
				ShowTauntText(state: false);
			}
			return;
		}
		Character character = null;
		if (_npc != null)
		{
			character = _npc;
			if (_cardData != null && _cardData.TargetSide != Enums.CardTargetSide.Anyone && _cardData.TargetSide != Enums.CardTargetSide.Enemy)
			{
				return;
			}
		}
		else if (_hero != null)
		{
			character = _hero;
			if (_cardData != null && _cardData.TargetSide == Enums.CardTargetSide.Enemy)
			{
				return;
			}
		}
		if (character == null)
		{
			return;
		}
		bool flag = false;
		int num = heal;
		if (_hero != null)
		{
			Hero heroHeroActive = MatchManager.Instance.GetHeroHeroActive();
			if (heroHeroActive != null && heroHeroActive.Id == character.Id)
			{
				flag = true;
			}
			else if (_cardData != null && _cardData.TargetSide == Enums.CardTargetSide.Self)
			{
				return;
			}
			int hpLeftForMax = _hero.GetHpLeftForMax();
			if (hpLeftForMax < heal)
			{
				heal = hpLeftForMax;
			}
		}
		else
		{
			int hpLeftForMax2 = _npc.GetHpLeftForMax();
			if (hpLeftForMax2 < heal)
			{
				heal = hpLeftForMax2;
			}
		}
		if (_npc != null)
		{
			ShowTauntText(state: false);
		}
		if (!state)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		if (_cardData != null && (_cardData.Aura != null || _cardData.HealCurses > 0 || _cardData.DispelAuras > 0 || _cardData.StealAuras > 0 || _cardData.HealAuraCurseSelf != null || _cardData.HealAuraCurseName != null || _cardData.HealAuraCurseName2 != null || _cardData.HealAuraCurseName3 != null || _cardData.HealAuraCurseName4 != null || _cardData.Curse != null))
		{
			num2 = _cardData.HealCurses;
			num3 = _cardData.DispelAuras;
			num4 = _cardData.StealAuras;
			List<string> list = new List<string>();
			if (num2 > 0)
			{
				list = character.GetAuraCurseByOrder(1, num2, onlyDispeleable: true);
			}
			else if (num3 > 0)
			{
				list = character.GetAuraCurseByOrder(0, num3, onlyDispeleable: true);
			}
			else if (num4 > 0)
			{
				list = character.GetAuraCurseByOrder(0, num4, onlyDispeleable: true);
			}
			if (_cardData.HealAuraCurseSelf != null && flag && character.HasEffect(_cardData.HealAuraCurseSelf.Id) && character.EffectCharges(_cardData.HealAuraCurseSelf.Id) > 0)
			{
				list.Add(_cardData.HealAuraCurseSelf.Id);
			}
			if (_cardData.HealAuraCurseName != null && character.HasEffect(_cardData.HealAuraCurseName.Id) && character.EffectCharges(_cardData.HealAuraCurseName.Id) > 0)
			{
				list.Add(_cardData.HealAuraCurseName.Id);
			}
			if (_cardData.HealAuraCurseName2 != null && character.HasEffect(_cardData.HealAuraCurseName2.Id) && character.EffectCharges(_cardData.HealAuraCurseName2.Id) > 0)
			{
				list.Add(_cardData.HealAuraCurseName2.Id);
			}
			if (_cardData.HealAuraCurseName3 != null && character.HasEffect(_cardData.HealAuraCurseName3.Id) && character.EffectCharges(_cardData.HealAuraCurseName3.Id) > 0)
			{
				list.Add(_cardData.HealAuraCurseName3.Id);
			}
			if (_cardData.HealAuraCurseName4 != null && character.HasEffect(_cardData.HealAuraCurseName4.Id) && character.EffectCharges(_cardData.HealAuraCurseName4.Id) > 0)
			{
				list.Add(_cardData.HealAuraCurseName4.Id);
			}
			if (list.Count > 0)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				StringBuilder stringBuilder3 = new StringBuilder();
				for (int i = 0; i < list.Count; i++)
				{
					stringBuilder2.Append("<sprite name=");
					stringBuilder2.Append(list[i]);
					stringBuilder2.Append(">");
					if (stringBuilder3.Length > 0)
					{
						stringBuilder3.Append("<space=2.5>");
					}
					stringBuilder3.Append(character.GetAuraCharges(list[i]));
				}
				if (stringBuilder2.Length > 0)
				{
					purgedispel.text = stringBuilder2.ToString();
					purgedispelQuantity.text = stringBuilder3.ToString();
					if (!purgedispel.gameObject.activeSelf)
					{
						purgedispel.transform.gameObject.SetActive(value: true);
					}
					if (num4 > 0)
					{
						purgedispelTitle.text = Texts.Instance.GetText("steal");
					}
					else
					{
						purgedispelTitle.text = Texts.Instance.GetText("remove");
					}
				}
			}
			if (_cardData.Aura != null || _cardData.Aura2 != null || _cardData.Aura3 != null || (_cardData.Auras != null && _cardData.Auras.Length != 0) || _cardData.Curse != null || _cardData.Curse2 != null || _cardData.Curse3 != null || (_cardData.Curses != null && _cardData.Curses.Length != 0))
			{
				List<string> list2 = new List<string>();
				if (_cardData.Aura != null)
				{
					list2.Add(_cardData.Aura.Id.ToLower());
				}
				if (_cardData.Aura2 != null)
				{
					list2.Add(_cardData.Aura2.Id.ToLower());
				}
				if (_cardData.Aura3 != null)
				{
					list2.Add(_cardData.Aura3.Id.ToLower());
				}
				if (_cardData.Auras != null && _cardData.Auras.Length != 0)
				{
					list2.AddRange(from x in _cardData.Auras
						where x.aura != null
						select x.aura.Id.ToLower());
				}
				if (_cardData.Curse != null)
				{
					list2.Add(_cardData.Curse.Id.ToLower());
				}
				if (_cardData.Curse2 != null)
				{
					list2.Add(_cardData.Curse2.Id.ToLower());
				}
				if (_cardData.Curse3 != null)
				{
					list2.Add(_cardData.Curse3.Id.ToLower());
				}
				if (_cardData.Curses != null && _cardData.Curses.Length != 0)
				{
					list2.AddRange(from x in _cardData.Curses
						where x.curse != null
						select x.curse.Id.ToLower());
				}
				AmplifyBuffs(list2);
			}
		}
		if (_cardData != null)
		{
			List<string> list3 = character.CharacterImmunitiesList();
			StringBuilder stringBuilder4 = new StringBuilder();
			if (_cardData.Curse != null && list3.Contains(_cardData.Curse.Id))
			{
				stringBuilder4.Append("<size=4><sprite name=");
				stringBuilder4.Append(_cardData.Curse.Id);
				stringBuilder4.Append("></size><size=-1><color=#e88>");
				stringBuilder4.Append(Texts.Instance.GetText("immune"));
				stringBuilder4.Append("</color></size><br>");
			}
			if (_cardData.Curse2 != null && list3.Contains(_cardData.Curse2.Id))
			{
				stringBuilder4.Append("<size=4><sprite name=");
				stringBuilder4.Append(_cardData.Curse2.Id);
				stringBuilder4.Append("></size><size=-1><color=#e88>");
				stringBuilder4.Append(Texts.Instance.GetText("immune"));
				stringBuilder4.Append("</color></size><br>");
			}
			if (_cardData.Curse3 != null && list3.Contains(_cardData.Curse3.Id))
			{
				stringBuilder4.Append("<size=4><sprite name=");
				stringBuilder4.Append(_cardData.Curse3.Id);
				stringBuilder4.Append("></size><size=-1><color=#e88>");
				stringBuilder4.Append(Texts.Instance.GetText("immune"));
				stringBuilder4.Append("</color></size><br>");
			}
			if (_cardData.Curses != null && _cardData.Curses.Length != 0)
			{
				for (int num5 = 0; num5 < _cardData.Curses.Length; num5++)
				{
					CardData.CurseDebuffs curseDebuffs = _cardData.Curses[num5];
					if (curseDebuffs != null && curseDebuffs.curse != null)
					{
						stringBuilder4.Append("<size=4><sprite name=");
						stringBuilder4.Append(curseDebuffs.curse.Id);
						stringBuilder4.Append("></size><size=-1><color=#e88>");
						stringBuilder4.Append(Texts.Instance.GetText("immune"));
						stringBuilder4.Append("</color></size><br>");
					}
				}
			}
			if (stringBuilder4.Length > 0)
			{
				stringBuilder.Append(stringBuilder4.ToString());
			}
		}
		if (heal == 0 && _npc != null && _npc.HasEffect("evasion"))
		{
			stringBuilder.Append("<sprite name=evasion><color=#559F2B>");
			stringBuilder.Append(Functions.Substring(Texts.Instance.GetText("evasion"), 5));
		}
		else if (heal == 0 && _npc != null && _npc.HasEffect("invulnerable"))
		{
			stringBuilder.Append("<sprite name=invulnerable><color=#DECD02>");
			stringBuilder.Append(Functions.Substring(Texts.Instance.GetText("invulnerable"), 5));
		}
		else if (dmg >= 1 && dmgType != "" && dmgType != "heart" && character.HasEffect("evasion"))
		{
			stringBuilder.Append("<sprite name=evasion><color=#559F2B>");
			stringBuilder.Append(Functions.Substring(Texts.Instance.GetText("evasion"), 5));
		}
		else
		{
			if ((dmg >= 1 || dmg2 >= 1) && blocked > 0)
			{
				stringBuilder.Append(blocked);
				stringBuilder.Append(" <sprite name=block>");
			}
			if (dmg > 0)
			{
				stringBuilder.Append(dmg);
				if (dmgType != "")
				{
					stringBuilder.Append(" <sprite name=");
					stringBuilder.Append(dmgType);
					stringBuilder.Append(">");
				}
			}
			if (dmg2 > 0)
			{
				stringBuilder.Append("\n");
				stringBuilder.Append(dmg2);
				if (dmgType2 != "")
				{
					stringBuilder.Append(" <sprite name=");
					stringBuilder.Append(dmgType2);
					stringBuilder.Append(">");
				}
			}
			if (dmg < 1 && dmg2 < 1 && blocked > 0)
			{
				stringBuilder.Append(blocked);
				stringBuilder.Append(" <sprite name=block>");
			}
			if (_cardData != null && _cardData.Damage > 0 && _npc != null && _npc.HasEffect("thorns"))
			{
				int value = _npc.EffectCharges("thorns");
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Insert(0, "<sprite name=thorns><br>");
					stringBuilder.Insert(0, "</color> ");
					stringBuilder.Insert(0, value);
					stringBuilder.Insert(0, "<color=#E59F40>");
				}
				else
				{
					stringBuilder.Append("<color=#E59F40>");
					stringBuilder.Append(value);
					stringBuilder.Append("</color> ");
					stringBuilder.Append("<sprite name=thorns><br>0");
				}
			}
		}
		if (heal > 0 || num > 0)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append("\n");
			}
			stringBuilder.Append(heal);
			stringBuilder.Append(" <sprite name=heal>");
		}
		if (stringBuilder.Length == 0 && dmg <= 0 && _cardData != null && ((_cardData.Damage > 0 && !flag) || (_cardData.DamageSelf > 0 && flag)))
		{
			stringBuilder.Append(0);
		}
		if (!dmgPreviewText.transform.gameObject.activeSelf)
		{
			dmgPreviewText.transform.gameObject.SetActive(value: true);
		}
		if (stringBuilder.Length > 0)
		{
			dmgPreviewText.text = stringBuilder.ToString();
		}
		else
		{
			dmgPreviewText.transform.gameObject.SetActive(value: false);
		}
		if (_npc != null && _npc.IsTaunted())
		{
			ShowTauntText(state: true);
		}
	}

	private void ShowThornsText(bool state)
	{
		if (!(thornsTransform == null) && thornsTransform.gameObject.activeSelf != state)
		{
			thornsTransform.gameObject.SetActive(state);
		}
	}

	private void ShowTauntText(bool state)
	{
		if (!(tauntTextTransform == null))
		{
			if (tauntTextTransform.gameObject.activeSelf != state)
			{
				tauntTextTransform.gameObject.SetActive(state);
			}
			if (state)
			{
				tauntTextT.text = "<size=+.5><sprite name=taunt></size>" + Texts.Instance.GetText("taunt");
			}
		}
	}

	public void DisableCollider()
	{
		GetComponent<BoxCollider2D>().enabled = false;
	}

	public void DrawOrderSprites(bool goToFront, int _order)
	{
		if (!isHero)
		{
			string text = base.gameObject.name.Trim();
			if (text.StartsWith("flamethrower_"))
			{
				_order = 3;
				goToFront = false;
			}
			else if (text.StartsWith("dwarfface_"))
			{
				_order = 0;
				goToFront = false;
			}
			else if (text.StartsWith("dt800_"))
			{
				_order = 2;
				goToFront = false;
			}
			else if (text.StartsWith("launcher_"))
			{
				_order = 1;
				goToFront = false;
			}
			else if (text.StartsWith("rustking_"))
			{
				_order = 0;
				goToFront = false;
				base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, -1f);
			}
			else if (text.StartsWith("pitt_"))
			{
				_order = 1;
				goToFront = false;
			}
		}
		else if (base.gameObject.name.Trim() == "tombstone")
		{
			_order = 0;
			goToFront = false;
		}
		int num = 0;
		if (goToFront)
		{
			num = 1000 * _order;
			if (PetItem != null)
			{
				if (PetItemFront)
				{
					PetItem.DrawOrderSprites(goToFront: true, _order + 1);
				}
				else
				{
					PetItem.DrawOrderSprites(goToFront: false, _order - 1);
				}
			}
			if (PetItemEnchantment != null)
			{
				if (PetItemEnchantmentFront)
				{
					PetItemEnchantment.DrawOrderSprites(goToFront: true, _order + 1000);
				}
				else
				{
					PetItemEnchantment.DrawOrderSprites(goToFront: false, _order);
				}
			}
		}
		else
		{
			num = -10000 + 1000 * (8 - _order);
			if (PetItem != null)
			{
				if (PetItemFront)
				{
					PetItem.DrawOrderSprites(goToFront: true, _order - 1);
				}
				else
				{
					PetItem.DrawOrderSprites(goToFront: false, _order + 1);
				}
			}
			if (PetItemEnchantment != null)
			{
				if (PetItemEnchantmentFront)
				{
					PetItemEnchantment.DrawOrderSprites(goToFront: true, _order - 1);
				}
				else
				{
					PetItemEnchantment.DrawOrderSprites(goToFront: false, _order + 1);
				}
			}
		}
		if (animatedSprites != null)
		{
			for (int i = 0; i < animatedSprites.Count; i++)
			{
				if (animatedSprites[i] != null)
				{
					animatedSprites[i].sortingOrder = num - i;
					animatedSprites[i].sortingLayerName = "Characters";
				}
			}
		}
		else
		{
			charImageSR.sortingOrder = num;
		}
		if (animatedSpritesOutOfCharacter == null)
		{
			return;
		}
		for (int j = 0; j < animatedSpritesOutOfCharacter.Count; j++)
		{
			if (animatedSpritesOutOfCharacter[j] != null)
			{
				animatedSpritesOutOfCharacter[j].ReOrderLayer();
			}
		}
	}

	public void DrawBlock(bool state)
	{
		if (blockCo != null)
		{
			StopCoroutine(blockCo);
		}
		if (state)
		{
			blockCo = StartCoroutine(ShowBlock(state: true));
		}
		else
		{
			blockCo = StartCoroutine(ShowBlock(state: false));
		}
	}

	private IEnumerator ShowBlock(bool state)
	{
		float scaleMax = 1f;
		if (_npc != null && _npc.NpcData != null && _npc.NpcData.BigModel)
		{
			scaleMax = 1.5f;
		}
		if (hpBlockIconT.gameObject.activeSelf == state && hpBlockT.gameObject.activeSelf == state)
		{
			yield break;
		}
		hpBlockT.gameObject.SetActive(value: false);
		if (state)
		{
			hpBlockIconT.gameObject.SetActive(value: true);
			float scale = 0f;
			while (scale < scaleMax + 0.25f && !(hpBlockIconT == null) && !(hpBlockIconT.gameObject == null))
			{
				hpBlockIconT.localScale = new Vector3(scale, scale, 1f);
				scale += 0.1f;
				yield return Globals.Instance.WaitForSeconds(0.01f);
			}
			while (scale > scaleMax && !(hpBlockIconT == null) && !(hpBlockIconT.gameObject == null))
			{
				hpBlockIconT.localScale = new Vector3(scale, scale, 1f);
				scale -= 0.1f;
				yield return Globals.Instance.WaitForSeconds(0.005f);
			}
		}
		else
		{
			blockBorderT.gameObject.SetActive(value: false);
			hpBlockIconT.localScale = new Vector3(0f, 0f, 1f);
			hpBlockIconT.gameObject.SetActive(value: false);
		}
		StartCoroutine(ShowBlockHP(state));
	}

	private IEnumerator ShowBlockHP(bool state, bool animation = true)
	{
		if (state)
		{
			float percent = hpRed.localScale.x;
			if (percent > 1f)
			{
				percent = 1f;
			}
			else if (percent < 0f)
			{
				percent = 0f;
			}
			if (animation)
			{
				float t = 0f;
				float seconds = 0.5f;
				hpBlockT.localScale = Vector3.zero;
				if (!hpBlockT.gameObject.activeSelf)
				{
					hpBlockT.gameObject.SetActive(value: true);
				}
				blockBorderT.localScale = Vector3.zero;
				if (!blockBorderT.gameObject.activeSelf)
				{
					blockBorderT.gameObject.SetActive(value: true);
				}
				while (t <= 1f)
				{
					t += Time.deltaTime / seconds;
					hpBlockT.localScale = Vector3.Lerp(new Vector3(0f, 1f, 1f), new Vector3(percent, 1f, 1f), Mathf.SmoothStep(0f, 1f, t));
					blockBorderT.localScale = Vector3.Lerp(new Vector3(0f, 1f, 1f), new Vector3(1f, 1f, 1f), Mathf.SmoothStep(0f, 1f, t));
					yield return null;
				}
			}
			else
			{
				hpBlockT.localScale = new Vector3(percent, hpBlockT.localScale.y, hpBlockT.localScale.z);
				if (!hpBlockT.gameObject.activeSelf)
				{
					hpBlockT.gameObject.SetActive(value: true);
				}
			}
		}
		else
		{
			if (hpBlockT.gameObject.activeSelf)
			{
				hpBlockT.gameObject.SetActive(value: false);
			}
			if (blockBorderT.gameObject.activeSelf)
			{
				blockBorderT.gameObject.SetActive(value: false);
			}
		}
	}

	private IEnumerator ShowShieldHP(int charges)
	{
		if (hpShieldT == null)
		{
			yield break;
		}
		hpShieldText.text = charges.ToString();
		if (!hpShieldT.gameObject.activeSelf)
		{
			float scaleMax = 1.1f;
			if (_npc != null && _npc.NpcData != null && _npc.NpcData.BigModel)
			{
				scaleMax = 1.55f;
			}
			float t = 0f;
			float seconds = 0.5f;
			hpShieldT.localScale = Vector3.zero;
			if (!hpShieldT.gameObject.activeSelf)
			{
				hpShieldT.gameObject.SetActive(value: true);
			}
			while (t <= 1f)
			{
				t += Time.deltaTime / seconds;
				if (hpShieldT != null)
				{
					hpShieldT.localScale = Vector3.Lerp(new Vector3(0f, 0f, 1f), new Vector3(scaleMax + 0.1f, scaleMax + 0.1f, 1f), Mathf.SmoothStep(0f, 1f, t));
				}
				yield return null;
			}
			hpShieldT.localScale = new Vector3(scaleMax, scaleMax, 1f);
		}
		yield return null;
	}

	public void DrawEnergy()
	{
		if (!isHero)
		{
			return;
		}
		int energy;
		int energyTurn;
		if (isHero)
		{
			energy = _hero.GetEnergy();
			energyTurn = _hero.GetEnergyTurn();
		}
		else
		{
			energy = _npc.GetEnergy();
			energyTurn = _npc.GetEnergyTurn();
		}
		energyTxt.text = energy.ToString();
		for (int i = 0; i < 10; i++)
		{
			if (!(energySR[i] == null))
			{
				if (i < energy)
				{
					energySR[i].color = Color.white;
					energySRAnimator[i].enabled = false;
				}
				else if (i < energy + energyTurn)
				{
					energySR[i].gameObject.SetActive(value: false);
					energySR[i].gameObject.SetActive(value: true);
					energySRAnimator[i].enabled = true;
					energySR[i].color = new Color(0f, 1f, 0.5f, 1f);
				}
				else
				{
					energySR[i].color = new Color(0f, 0f, 0f, 0.45f);
					energySRAnimator[i].enabled = false;
				}
			}
		}
	}

	public void ScrollCombatText(string text, Enums.CombatScrollEffectType type)
	{
		CT.SetText(text, type);
	}

	public void ScrollCombatTextDamageNew(CastResolutionForCombatText _cast)
	{
		CT.SetDamageNew(_cast);
	}

	public bool IsCombatScrollEffectActive()
	{
		return CT.IsPlaying();
	}

	public void ActivateMark(bool state)
	{
		if (base.transform != null)
		{
			isActive = state;
			if (activeMarkTR != null && activeMarkTR.gameObject.activeSelf != state)
			{
				activeMarkTR.gameObject.SetActive(state);
			}
		}
	}

	public bool AnimNameIs(string name)
	{
		if (anim == null)
		{
			return false;
		}
		return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
	}

	public void CharacterAttackAnim()
	{
		if (anim != null)
		{
			if (animationBusyCo != null)
			{
				StopCoroutine(animationBusyCo);
			}
			animationBusyCo = StartCoroutine(SetAnimationBusy());
			anim.ResetTrigger("attack");
			anim.SetTrigger("attack");
			PetCastAnim("cast");
		}
	}

	public void CharacterCastAnim()
	{
		if (anim != null)
		{
			if (animationBusyCo != null)
			{
				StopCoroutine(animationBusyCo);
			}
			animationBusyCo = StartCoroutine(SetAnimationBusy());
			anim.ResetTrigger("cast");
			anim.SetTrigger("cast");
			PetCastAnim("cast");
		}
	}

	private IEnumerator SetAnimationBusy()
	{
		animationBusy = true;
		yield return Globals.Instance.WaitForSeconds(1f);
		animationBusy = false;
	}

	private IEnumerator PetCastAnimTO(string animState)
	{
		List<Animator> petAnimators = new List<Animator>();
		for (int i = 0; i < 3; i++)
		{
			Transform transform = i switch
			{
				0 => (!isHero) ? base.transform.Find("thePetEnchantment" + _npc.Enchantment) : base.transform.Find("thePetEnchantment" + _hero.Enchantment), 
				1 => (!isHero) ? base.transform.Find("thePetEnchantment" + _npc.Enchantment2) : base.transform.Find("thePetEnchantment" + _hero.Enchantment2), 
				_ => (!isHero) ? base.transform.Find("thePetEnchantment" + _npc.Enchantment3) : base.transform.Find("thePetEnchantment" + _hero.Enchantment3), 
			};
			if (transform != null && transform.GetComponent<Animator>() != null)
			{
				petAnimators.Add(transform.GetComponent<Animator>());
			}
		}
		for (int j = 0; j < petAnimators.Count; j++)
		{
			if (animState != "hit")
			{
				yield return Globals.Instance.WaitForSeconds((float)Random.Range(0, 30) * 0.01f);
			}
			if (petAnimators[j] != null)
			{
				petAnimators[j].ResetTrigger(animState);
				petAnimators[j].SetTrigger(animState);
			}
		}
		yield return null;
	}

	public void PetCastAnim(string animState)
	{
		if (!(animPet != null))
		{
			return;
		}
		if (!isHero)
		{
			StartCoroutine(PetCastAnimTO(animState));
			return;
		}
		animPet.ResetTrigger(animState);
		animPet.SetTrigger(animState);
		if (animState == "attack")
		{
			StartCoroutine(PetMoveToCenter());
		}
	}

	private IEnumerator PetMoveToCenter()
	{
		if (!(PetItem == null))
		{
			PetItem.MoveToCenter();
			yield return Globals.Instance.WaitForSeconds(0.8f);
			PetItem.MoveToCenterBack();
		}
	}

	public void CharacterEnableAnim(bool state)
	{
		if (anim != null)
		{
			if (state)
			{
				anim.enabled = true;
			}
			else
			{
				anim.enabled = false;
			}
		}
	}

	public void CharacterHitted(bool fromHit = false)
	{
		if (characterBeingHitted)
		{
			return;
		}
		if (_hero != null)
		{
			if (_hero.IsParalyzed())
			{
				GameManager.Instance.PlayLibraryAudio("hit_metal2");
				return;
			}
		}
		else if (_npc != null)
		{
			if (_npc.IsParalyzed())
			{
				GameManager.Instance.PlayLibraryAudio("hit_metal2");
				return;
			}
			if (fromHit && _npc.NpcData != null)
			{
				switch (_npc.NpcData.Id.ToLower().Split('_')[0])
				{
				case "trunky":
				case "taintedtrunky":
				case "sapling":
				case "taintedsapling":
				case "ylmer":
					GameManager.Instance.PlayLibraryAudio("impact_wood");
					break;
				default:
					if (MatchManager.Instance.CardActive != null)
					{
						if (MatchManager.Instance.CardActive.DamageType == Enums.DamageType.Slashing)
						{
							GameManager.Instance.PlayLibraryAudio("impact_slashing");
						}
						else if (MatchManager.Instance.CardActive.DamageType == Enums.DamageType.Blunt)
						{
							GameManager.Instance.PlayLibraryAudio("impact_crushing");
						}
						else if (MatchManager.Instance.CardActive.DamageType == Enums.DamageType.Piercing)
						{
							GameManager.Instance.PlayLibraryAudio("impact_piercing");
						}
					}
					break;
				}
			}
		}
		characterBeingHitted = true;
		if (anim != null && !animationBusy)
		{
			anim.ResetTrigger("hit");
			anim.SetTrigger("hit");
			PetCastAnim("hit");
		}
		if (fromHit)
		{
			if (_npc != null && _npc.NpcData != null && _npc.NpcData.GetHitSound(MatchManager.Instance.HitSoundIndex) != null)
			{
				GameManager.Instance.PlayAudio(_npc.NpcData.GetHitSound(MatchManager.Instance.HitSoundIndex), 0.1f);
				MatchManager.Instance.HitSoundIndex++;
			}
			if (_hero != null && _hero.HeroData != null && _hero.HeroData.HeroSubClass.GetHitSound(MatchManager.Instance.HitSoundIndex) != null)
			{
				GameManager.Instance.PlayAudio(_hero.HeroData.HeroSubClass.GetHitSound(MatchManager.Instance.HitSoundIndex), 0.1f);
				MatchManager.Instance.HitSoundIndex++;
			}
		}
		if (hitCo != null)
		{
			StopCoroutine(hitCo);
		}
		hitCo = StartCoroutine(CharacterBeingHittedCo());
	}

	private IEnumerator CharacterBeingHittedCo()
	{
		yield return Globals.Instance.WaitForSeconds(0.1f);
		characterBeingHitted = false;
	}

	public void MoveToCenter()
	{
		if (!(charImageT == null))
		{
			if (moveCenterCo != null)
			{
				StopCoroutine(moveCenterCo);
			}
			if (moveBackCo != null)
			{
				StopCoroutine(moveBackCo);
			}
			moveCenterCo = StartCoroutine(SetPositionCO(charImageT, new Vector3(0f - charImageT.parent.transform.localPosition.x + charImageT.localPosition.x, charImageT.localPosition.y, 0f), returnPosition: false));
		}
	}

	public void MoveToPosition(Transform targetTransform, Vector3 targetPosition, bool returnBack, bool playSmokeEffect, float movementTimeS, MotionGenerator.EasingType easingType = MotionGenerator.EasingType.EaseOut)
	{
		if (!(charImageT == null))
		{
			StartCoroutine(MovePositionCo(targetTransform, targetPosition, returnBack, playSmokeEffect, movementTimeS, easingType));
		}
	}

	public void MoveToCenterBack()
	{
		if (!(charImageT == null) && Mathf.Abs(charImageT.localPosition.x - originalLocalPosition.x) > 0.02f)
		{
			if (moveCenterCo != null)
			{
				StopCoroutine(moveCenterCo);
			}
			if (moveBackCo != null)
			{
				StopCoroutine(moveBackCo);
			}
			moveBackCo = StartCoroutine(SetPositionCO(charImageT, new Vector3(originalLocalPosition.x, charImageT.localPosition.y, 0f), returnPosition: true));
		}
	}

	public void SetOriginalLocalPosition(Vector3 pos)
	{
		originalLocalPosition = pos;
	}

	public void SetPosition(bool instant, int _position = -10)
	{
		if (instant || _npc == null || !_npc.NpcData.IsBoss)
		{
			int position = ((_hero != null && _position == -10) ? _hero.Position : ((_npc == null || _position != -10) ? _position : _npc.Position));
			if (_npc != null && _npc.NPCIsBoss() && _npc.Id.Contains("faebor"))
			{
				float x = 2.4f;
				healthBar.transform.localPosition = new Vector3(x, healthBar.transform.localPosition.y, healthBar.transform.localPosition.z);
				Vector3 vector = new Vector3(x, 0f, 0f);
				iconEnchantment.transform.position += vector;
				iconEnchantment2.transform.position += vector;
				iconEnchantment3.transform.position += vector;
			}
			Vector3 localPosition = CalculateLocalPosition(position);
			if (!Mathf.Approximately(base.transform.localPosition.x, localPosition.x) && instant)
			{
				base.transform.localPosition = localPosition;
			}
		}
	}

	public Vector3 CalculateLocalPosition(int position)
	{
		float x = CalculatePositionX(position);
		float z = (float)position * 0.001f;
		charImageT.transform.localPosition = new Vector3(charImageT.transform.localPosition.x, charImageT.transform.localPosition.y, z);
		if (!isHero)
		{
			string text = base.gameObject.name.Trim();
			if (text.StartsWith("flamethrower_"))
			{
				z = 0.03f;
			}
			else if (text.StartsWith("dwarfface_"))
			{
				z = 0f;
			}
			else if (text.StartsWith("dt800_"))
			{
				z = 0.02f;
			}
			else if (text.StartsWith("launcher_"))
			{
				z = 0.01f;
			}
		}
		return new Vector3(x, 0f, z);
	}

	public float CalculatePositionX(int _position)
	{
		float num = 0f;
		float num2 = 2.4f;
		float num3 = 1.9f;
		int num4 = 0;
		if (_hero != null)
		{
			num4 = ((_position != -10) ? _position : _hero.Position);
			num = 0f - num2 - (float)num4 * num3;
		}
		else if (_npc != null)
		{
			num4 = ((_position != -10) ? _position : _npc.Position);
			num = num2 + (float)num4 * num3;
			if (_npc.IsBigModel())
			{
				num = ((num4 != 0 && num4 != 1) ? (num + 0.5f * num3) : (num + 0.35f * num3));
			}
		}
		return num;
	}

	public bool CharIsMoving()
	{
		return charIsMoving;
	}

	private IEnumerator SetPositionCO(Transform theTransform, Vector3 vectorPosition, bool returnPosition)
	{
		charIsMoving = true;
		if (!IsMovementAllowed(theTransform, vectorPosition))
		{
			charIsMoving = false;
			yield break;
		}
		PrepareMovement(theTransform, vectorPosition, returnPosition, playSmokeEffect: true);
		charIsMoving = true;
		if (!GameManager.Instance.IsMultiplayer() && GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Ultrafast)
		{
			if (theTransform != null)
			{
				theTransform.localPosition = vectorPosition;
			}
		}
		else
		{
			bool flag = false;
			while (!flag && theTransform != null)
			{
				while (theTransform != null && Mathf.Abs(theTransform.localPosition.x - vectorPosition.x) > 0.05f)
				{
					theTransform.localPosition = Vector3.Lerp(theTransform.localPosition, vectorPosition, 0.5f);
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
				if (theTransform != null)
				{
					theTransform.localPosition = vectorPosition;
				}
				flag = true;
			}
		}
		if (!isActive)
		{
			CharacterEnableAnim(state: true);
		}
		charIsMoving = false;
	}

	private IEnumerator MovePositionCo(Transform theTransform, Vector3 vectorPosition, bool returnPosition, bool playSmokeEffect, float movementTimeS, MotionGenerator.EasingType easingType)
	{
		charIsMoving = true;
		if (!IsMovementAllowed(theTransform, vectorPosition))
		{
			charIsMoving = false;
			yield break;
		}
		PrepareMovement(theTransform, vectorPosition, returnPosition, playSmokeEffect);
		yield return StartCoroutine(MotionGenerator.MoveWithEasing(theTransform, vectorPosition, movementTimeS, easingType, MotionGenerator.Axis.X));
		if (!isActive)
		{
			CharacterEnableAnim(state: true);
		}
		charIsMoving = false;
	}

	private bool IsMovementAllowed(Transform theTransform, Vector3 vectorPosition)
	{
		bool flag = false;
		if (Mathf.Abs(theTransform.localPosition.x - vectorPosition.x) < 0.02f)
		{
			flag = true;
		}
		else if (_hero != null && !_hero.Alive)
		{
			flag = true;
		}
		else if (_npc != null && !_npc.Alive)
		{
			flag = true;
		}
		if (flag)
		{
			return false;
		}
		return true;
	}

	private void PrepareMovement(Transform theTransform, Vector3 targetPosition, bool returnPosition, bool playSmokeEffect)
	{
		bool flip = !(theTransform.localPosition.x < targetPosition.x);
		if (!returnPosition)
		{
			MatchManager.Instance.DoStepSound();
		}
		if (playSmokeEffect)
		{
			EffectsManager.Instance.PlayEffectAC("smoke", isHero, theTransform, flip);
		}
		if (!isActive)
		{
			CharacterEnableAnim(state: false);
		}
	}

	public void SetHP()
	{
		Character character;
		float num;
		float num2;
		int block;
		int auraCharges;
		if (_hero != null)
		{
			character = _hero;
			num = _hero.HpCurrent;
			num2 = _hero.GetMaxHP();
			if (num > num2)
			{
				_hero.HpCurrent = (int)num2;
				num = num2;
			}
			block = _hero.GetBlock();
			auraCharges = _hero.GetAuraCharges("shield");
		}
		else
		{
			if (_npc == null)
			{
				return;
			}
			character = _npc;
			num = _npc.HpCurrent;
			num2 = _npc.GetMaxHP();
			if (num > num2)
			{
				_npc.HpCurrent = (int)num2;
				num = num2;
			}
			block = _npc.GetBlock();
			auraCharges = _npc.GetAuraCharges("shield");
		}
		if (block > 0)
		{
			if (hpBlockText != null)
			{
				hpBlockText.text = block.ToString();
			}
			if (block > 49)
			{
				PlayerManager.Instance.AchievementUnlock("CHARGES_DEFENDER");
			}
			if (block > 199)
			{
				PlayerManager.Instance.AchievementUnlock("CHARGES_FORTRESS");
			}
			if (hpBlockIconT != null)
			{
				hpBlockIconT.GetComponent<PopupAuraCurse>().SetAuraCurse(Globals.Instance.GetAuraCurseData("block"), block, _fast: true);
			}
		}
		if (hpShieldT != null && hpShieldT.gameObject != null)
		{
			if (auraCharges > 0)
			{
				hpShieldT.GetComponent<PopupAuraCurse>().SetAuraCurse(Globals.Instance.GetAuraCurseData("shield"), auraCharges, _fast: true);
				StartCoroutine(ShowShieldHP(auraCharges));
			}
			else
			{
				hpShieldT.gameObject.SetActive(value: false);
				hpShieldText.text = "";
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(num);
		stringBuilder.Append("<size=-.5><color=#bbb>/");
		stringBuilder.Append(num2);
		hpText.text = stringBuilder.ToString();
		stringBuilder = null;
		if (character.GetHp() < 1)
		{
			if (hpRed != null)
			{
				hpRed.localScale = new Vector3(0f, 1f, 1f);
			}
			if (hpBleed != null)
			{
				hpBleed.localScale = new Vector3(0f, 1f, 1f);
			}
			if (hpPoison != null)
			{
				hpPoison.localScale = new Vector3(0f, 1f, 1f);
			}
			if (hpRegen != null)
			{
				hpRegen.localScale = new Vector3(0f, 1f, 1f);
			}
			if (!IsDying)
			{
				KillCoroutine = StartCoroutine(KillCharacterCO());
			}
			return;
		}
		MatchManager.Instance.CleanPrePostDamageDictionary(character.Id);
		CalculateDamagePrePostForThisCharacter();
		int num3 = int.Parse(MatchManager.Instance.prePostDamageDictionary[character.Id][0]);
		int num4 = int.Parse(MatchManager.Instance.prePostDamageDictionary[character.Id][1]);
		int num5 = 0;
		if (num3 < 0)
		{
			num3 = -1 * num3;
		}
		else
		{
			num5 = num3;
			num3 = 0;
		}
		num4 = ((num4 < 0) ? (-1 * num4) : 0);
		float num6 = (num - (float)num3 - (float)num4) / num2;
		if (num6 < 0f)
		{
			num6 = 0f;
		}
		else if (num6 > 1f)
		{
			num6 = 1f;
		}
		if (hpRed != null)
		{
			hpRed.localScale = new Vector3(num6, 1f, 1f);
		}
		if (hpBlockT != null)
		{
			hpBlockT.localScale = hpRed.localScale;
		}
		float x = 0f;
		if (num3 > 0)
		{
			x = num / num2;
		}
		if (hpBleed != null)
		{
			hpBleed.localScale = new Vector3(x, 1f, 1f);
		}
		if (num5 > 0 && hpRegen != null)
		{
			float num7 = (num + (float)num5) / num2;
			if (num7 > 1f)
			{
				num7 = 1f;
			}
			hpRegen.localScale = new Vector3(num7, 1f, 1f);
		}
		else if (hpRegen != null)
		{
			hpRegen.localScale = Vector3.zero;
		}
		float num8 = 0f;
		if (num4 > 0)
		{
			num8 = (num - (float)num3) / num2;
		}
		if (num8 < 0f)
		{
			num8 = 0f;
		}
		if (hpPoison != null)
		{
			hpPoison.localScale = new Vector3(num8, 1f, 1f);
		}
	}

	private void AmplifyBuffs(List<string> _listAuraCurse)
	{
		if (_listAuraCurse != null)
		{
			for (int i = 0; i < GoBuffs.Count; i++)
			{
				if (_listAuraCurse.Contains(GoBuffs[i].buffId))
				{
					GoBuffs[i].DisplayBecauseCard(_status: true);
				}
				else
				{
					GoBuffs[i].DisplayBecauseCard(_status: false);
				}
			}
		}
		else
		{
			for (int j = 0; j < GoBuffs.Count; j++)
			{
				GoBuffs[j].RestoreBecauseCard();
			}
		}
	}

	public void DrawBuffs(AuraCurseData auraIncluded = null, int auraIncludedCharges = 0, int previousCharges = -1)
	{
		if (!(GO_Buffs == null))
		{
			if (drawBuffsCoroutine != null)
			{
				StopCoroutine(drawBuffsCoroutine);
			}
			drawBuffsCoroutine = StartCoroutine(DrawBuffsCo(auraIncluded, auraIncludedCharges, previousCharges));
		}
	}

	private IEnumerator DrawBuffsCo(AuraCurseData auraIncluded = null, int auraIncludedCharges = 0, int previousCharges = -1)
	{
		if (GO_Buffs == null)
		{
			yield break;
		}
		int count;
		string id;
		if (isHero)
		{
			count = _hero.AuraList.Count;
			id = _hero.Id;
		}
		else
		{
			count = _npc.AuraList.Count;
			id = _npc.Id;
		}
		int num = -1;
		for (int i = 0; i < count; i++)
		{
			if (GO_Buffs == null)
			{
				yield break;
			}
			Aura aura = ((!isHero) ? _npc.AuraList[i] : _hero.AuraList[i]);
			if (aura == null || !(aura.ACData != null) || !aura.ACData.IconShow)
			{
				continue;
			}
			if (GO_Buffs == null)
			{
				yield break;
			}
			num++;
			Buff buff = GoBuffs[num];
			if (buff != null)
			{
				buff.SetBuff(aura.ACData, aura.GetCharges(), "", id);
			}
			if (buff == null || buff.gameObject == null)
			{
				continue;
			}
			buff.gameObject.name = aura.ACData.Id;
			if (auraIncluded != null && buff.gameObject.name == auraIncluded.Id)
			{
				if (buffAnimationList.ContainsKey(buff.gameObject.name))
				{
					buffAnimationList[buff.gameObject.name] += auraIncludedCharges;
				}
				else
				{
					buffAnimationList.Add(buff.gameObject.name, auraIncludedCharges);
				}
			}
			string text = buff.name.ToLower();
			int charges = aura.GetCharges();
			switch (text)
			{
			case "poison":
				if (charges > 49)
				{
					PlayerManager.Instance.AchievementUnlock("CHARGES_POISONOUS");
				}
				if (charges > 199)
				{
					PlayerManager.Instance.AchievementUnlock("CHARGES_TOXICDISASTER");
				}
				break;
			case "bleed":
				if (charges > 49)
				{
					PlayerManager.Instance.AchievementUnlock("CHARGES_BLOODY");
				}
				if (charges > 199)
				{
					PlayerManager.Instance.AchievementUnlock("CHARGES_BLOODBATH");
				}
				break;
			case "burn":
				if (charges > 49)
				{
					PlayerManager.Instance.AchievementUnlock("CHARGES_INCENDIARY");
				}
				if (charges > 199)
				{
					PlayerManager.Instance.AchievementUnlock("CHARGES_INFERNO");
				}
				break;
			case "crack":
				if (charges > 49)
				{
					PlayerManager.Instance.AchievementUnlock("CHARGES_WRECKINGBALL");
				}
				if (charges > 199)
				{
					PlayerManager.Instance.AchievementUnlock("CHARGES_SIEGEBREAKER");
				}
				break;
			case "fury":
				if (charges > 49)
				{
					PlayerManager.Instance.AchievementUnlock("CHARGES_FURIOUS");
				}
				if (charges > 199)
				{
					PlayerManager.Instance.AchievementUnlock("CHARGES_ENDLESSFURY");
				}
				break;
			case "sanctify":
				if (charges > 39)
				{
					PlayerManager.Instance.AchievementUnlock("CHARGES_HOLYGROUND");
				}
				break;
			case "bless":
				if (charges > 39)
				{
					PlayerManager.Instance.AchievementUnlock("CHARGES_DIVINE");
				}
				break;
			case "regeneration":
				if (charges > 39)
				{
					PlayerManager.Instance.AchievementUnlock("CHARGES_DRYAD");
				}
				break;
			case "thorns":
				if (charges > 39)
				{
					PlayerManager.Instance.AchievementUnlock("CHARGES_SPIKY");
				}
				break;
			case "chill":
				if (charges > 49)
				{
					PlayerManager.Instance.AchievementUnlock("CHARGES_CHILLY");
				}
				if (charges > 199)
				{
					PlayerManager.Instance.AchievementUnlock("CHARGES_GLACIER");
				}
				break;
			case "spark":
				if (charges > 49)
				{
					PlayerManager.Instance.AchievementUnlock("CHARGES_SPARKLY");
				}
				if (charges > 199)
				{
					PlayerManager.Instance.AchievementUnlock("CHARGES_ELECTROCUTER");
				}
				break;
			case "insane":
				if (charges > 39)
				{
					PlayerManager.Instance.AchievementUnlock("CHARGES_INSANE");
				}
				break;
			}
		}
		for (int j = num + 1; j < GoBuffs.Count - 1; j++)
		{
			if (GoBuffs[j] != null && GoBuffs[j].gameObject != null)
			{
				GoBuffs[j].CleanBuff();
			}
		}
		if (buffAnimationCo != null)
		{
			StopCoroutine(buffAnimationCo);
		}
		if (buffAnimationList.Count > 0)
		{
			buffAnimationCo = StartCoroutine(BuffAnimationCoroutine());
		}
	}

	private IEnumerator BuffAnimationCoroutine()
	{
		yield return Globals.Instance.WaitForSeconds(0.01f);
		for (int i = 0; i < GoBuffs.Count; i++)
		{
			if (GoBuffs[i] != null && GoBuffs[i].gameObject != null && GoBuffs[i].gameObject.name != "" && buffAnimationList != null && buffAnimationList.ContainsKey(GoBuffs[i].gameObject.name))
			{
				Buff component = GoBuffs[i].transform.GetComponent<Buff>();
				if (component != null)
				{
					component.Amplify(buffAnimationList[GoBuffs[i].gameObject.name]);
				}
			}
		}
		buffAnimationList.Clear();
	}

	public void InstantFadeOutCharacter()
	{
		if (animatedSprites == null)
		{
			return;
		}
		for (int i = 0; i < animatedSprites.Count; i++)
		{
			if (animatedSprites[i] != null)
			{
				animatedSprites[i].color = new Color(1f, 1f, 1f, 0f);
			}
		}
	}

	public IEnumerator FadeOutCharacter()
	{
		float index = 1f;
		while (index > 0f)
		{
			yield return Globals.Instance.WaitForSeconds(0.03f);
			index -= 0.1f;
			if (animatedSprites != null && animatedSprites.Count > 0)
			{
				for (int i = 0; i < animatedSprites.Count; i++)
				{
					animatedSprites[i].color = new Color(1f, 1f, 1f, index);
				}
			}
			else if (charImageSR != null)
			{
				charImageSR.color = new Color(1f, 1f, 1f, index);
			}
		}
		yield return null;
	}

	public IEnumerator FadeInCharacter(float delay = 0f)
	{
		Globals.Instance.WaitForSeconds(delay);
		float index = 0f;
		while (index < 1f)
		{
			yield return Globals.Instance.WaitForSeconds(0.03f);
			index += 0.1f;
			if (animatedSprites != null && animatedSprites.Count > 0)
			{
				for (int i = 0; i < animatedSprites.Count; i++)
				{
					animatedSprites[i].color = new Color(1f, 1f, 1f, index);
				}
			}
			else if (charImageSR != null)
			{
				charImageSR.color = new Color(1f, 1f, 1f, index);
			}
		}
		yield return null;
	}

	public void KillCharacterFromOutside()
	{
		KillCoroutine = StartCoroutine(KillCharacterCO());
	}

	private IEnumerator KillCharacterCO()
	{
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[KILLCHARACTERCO] isDying -> " + IsDying, "trace");
		}
		if (IsDying)
		{
			yield break;
		}
		if ((isHero && _hero == null) || (!isHero && _npc == null))
		{
			Debug.LogError("[KILLCHARACTERCO] STOP because NULL");
			yield break;
		}
		IsDying = true;
		MatchManager.Instance.SetWaitingKill(_state: true);
		StringBuilder stringBuilder = new StringBuilder();
		if (isHero && _hero != null)
		{
			stringBuilder.Append(_hero.HpCurrent);
			stringBuilder.Append("<size=-.5><color=#bbb>/");
			stringBuilder.Append(_hero.GetMaxHP());
			hpText.text = stringBuilder.ToString();
			MatchManager.Instance.CreateLogEntry(_initial: true, "", "", _hero, null, null, null, MatchManager.Instance.GameRound(), Enums.EventActivation.Killed);
			for (int i = 0; i < 4; i++)
			{
				Hero hero = MatchManager.Instance.GetHero(i);
				if (hero != null && hero.Alive)
				{
					hero.SetEvent(Enums.EventActivation.CharacterKilled, _hero);
				}
			}
			if (_hero.HaveResurrectItem())
			{
				_hero.ActivateItem(Enums.EventActivation.Killed, _hero, 0, "");
				MatchManager.Instance.waitingDeathScreen = false;
				MatchManager.Instance.SetWaitingKill(_state: false);
				IsDying = false;
				Debug.Log("Item resurrect");
				Debug.Log(_hero.Alive);
				MatchManager.Instance.CreateLogEntry(_initial: true, "", "", _hero, null, null, null, MatchManager.Instance.GameRound(), Enums.EventActivation.Resurrect);
				yield break;
			}
			_hero.SetEvent(Enums.EventActivation.Killed);
			yield return Globals.Instance.WaitForSeconds(0.3f);
			if (_hero.GetHp() > 0)
			{
				IsDying = false;
				MatchManager.Instance.SetWaitingKill(_state: false);
				MatchManager.Instance.waitingDeathScreen = false;
				yield break;
			}
			_hero.Alive = false;
			if (MatchManager.Instance.AnyHeroAlive())
			{
				if (!MatchManager.Instance.waitingDeathScreen)
				{
					MatchManager.Instance.waitingDeathScreen = true;
					MatchManager.Instance.ShowDeathScreen(_hero);
				}
				while (MatchManager.Instance.waitingDeathScreen)
				{
					yield return Globals.Instance.WaitForSeconds(0.01f);
				}
			}
			else
			{
				MatchManager.Instance.waitingDeathScreen = false;
				MatchManager.Instance.SetWaitingKill(_state: false);
			}
		}
		else if (_npc != null)
		{
			stringBuilder.Append(_npc.HpCurrent);
			stringBuilder.Append("<size=-.5><color=#bbb>/");
			stringBuilder.Append(_npc.GetMaxHP());
			hpText.text = stringBuilder.ToString();
			MatchManager.Instance.CreateLogEntry(_initial: true, "", "", null, _npc, null, null, MatchManager.Instance.GameRound(), Enums.EventActivation.Killed);
			for (int j = 0; j < 4; j++)
			{
				Hero hero2 = MatchManager.Instance.GetHero(j);
				if (hero2 != null && hero2.Alive)
				{
					hero2.SetEvent(Enums.EventActivation.CharacterKilled, _npc);
				}
			}
			if (_npc.HaveResurrectItem())
			{
				_npc.ActivateItem(Enums.EventActivation.Killed, _npc, 0, "");
				yield return Globals.Instance.WaitForSeconds(0.8f);
				IsDying = false;
				MatchManager.Instance.SetWaitingKill(_state: false);
				yield break;
			}
			_npc.SetEvent(Enums.EventActivation.Killed);
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[KILLCHARACTERCO] Kill NPC _ Life -> " + _npc.GetHp());
			}
			if (_npc.GetHp() > 0)
			{
				Debug.Log("[KILLCHARACTERCO] npc resurrect");
				IsDying = false;
				MatchManager.Instance.SetWaitingKill(_state: false);
				yield break;
			}
			_npc.Alive = false;
			if (Globals.Instance.ShowDebug)
			{
				Functions.DebugLogGD("[KILLCHARACTERCO] Kill 2 NPC _ Life -> " + _npc.GetHp(), "trace");
			}
		}
		for (int num = base.transform.childCount - 1; num >= 0; num--)
		{
			Transform child = base.transform.GetChild(num);
			if (child != null && child.name != "Character" && child.name != base.transform.gameObject.name)
			{
				Object.Destroy(child.gameObject);
			}
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[KILLCHARACTERCO] Kill 3", "trace");
		}
		if (_npc != null && _npc.Id.StartsWith("pitt"))
		{
			yield return Globals.Instance.WaitForSeconds(0.1f);
			MatchManager.Instance.DoSahtiRustBackground(showPittAlive: false, justKilled: true);
		}
		float index = 1f;
		Color hideColor = new Color(1f, 1f, 1f, 0f);
		while (index > 0f)
		{
			yield return Globals.Instance.WaitForSeconds(0.02f);
			index -= 0.1f;
			if (anim != null)
			{
				for (int k = 0; k < animatedSprites.Count; k++)
				{
					if (animatedSprites[k] != null)
					{
						hideColor.a = index;
						animatedSprites[k].color = hideColor;
					}
				}
			}
			else if (charImageSR != null)
			{
				hideColor.a = index;
				charImageSR.color = hideColor;
			}
		}
		if (isHero && MatchManager.Instance != null)
		{
			MatchManager.Instance.RemoveFromTransformDict(_hero.Id);
		}
		if (!isHero && MatchManager.Instance != null)
		{
			MatchManager.Instance.RemoveFromTransformDict(_npc.Id);
		}
		if (Globals.Instance.ShowDebug)
		{
			Functions.DebugLogGD("[KILLCHARACTERCO] Kill 4 - Call KillHero or KillNPC", "trace");
		}
		if (isHero)
		{
			yield return Globals.Instance.WaitForSeconds(0.01f * (float)_hero.Position);
			MatchManager.Instance.SetWaitingKill(_state: false);
			MatchManager.Instance.KillHero(_hero);
		}
		else
		{
			yield return Globals.Instance.WaitForSeconds(0.01f * (float)_npc.Position);
			MatchManager.Instance.SetWaitingKill(_state: false);
			MatchManager.Instance.KillNPC(_npc);
		}
	}

	public void ShowOverCards()
	{
		if (!(heroDecks == null) && _hero != null)
		{
			TMP_Text tMP_Text = heroDecksDeckText;
			string text = (heroDecksDeckTextBg.text = MatchManager.Instance.CountHeroDeck(_hero.HeroIndex).ToString());
			tMP_Text.text = text;
			TMP_Text tMP_Text2 = heroDecksDiscardText;
			text = (heroDecksDiscardTextBg.text = MatchManager.Instance.CountHeroDiscard(_hero.HeroIndex).ToString());
			tMP_Text2.text = text;
			if (0 > 0)
			{
				heroDecksDeckText.color = new Color(1f, 0.41f, 0.56f);
			}
			else
			{
				heroDecksDeckText.color = new Color(0.88f, 0.71f, 0.3f);
			}
			if (!heroDecks.gameObject.activeSelf)
			{
				heroDecks.gameObject.SetActive(value: true);
			}
		}
	}

	public void HideOverCards()
	{
		if (!(heroDecks == null) && heroDecks.gameObject.activeSelf)
		{
			heroDecks.gameObject.SetActive(value: false);
		}
	}

	private void OnMouseExit()
	{
		fOnMouseExit();
	}

	private void OnMouseEnter()
	{
		fOnMouseEnter();
	}

	private void OnMouseOver()
	{
		fOnMouseOver();
	}

	public void fOnMouseEnter()
	{
		if (AlertManager.Instance.IsActive() || SettingsManager.Instance.IsActive())
		{
			return;
		}
		if (MatchManager.Instance.CardDrag)
		{
			if (GameManager.Instance.IsMultiplayer() && MatchManager.Instance.IsYourTurn() && !MatchManager.Instance.CanInstaCast(MatchManager.Instance.CardItemActive.CardData) && MatchManager.Instance.CardItemActive != null)
			{
				bool flag = false;
				byte characterIndex = 0;
				if (Hero != null)
				{
					flag = true;
					characterIndex = (byte)Hero.HeroIndex;
				}
				if (NPC != null)
				{
					characterIndex = (byte)NPC.NPCIndex;
				}
				MatchManager.Instance.DrawArrowNet(MatchManager.Instance.CardItemActive.tablePosition, MatchManager.Instance.CardItemActive.transform.position, base.transform.position + new Vector3(0f, GetComponent<BoxCollider2D>().size.y * 0.7f, 0f), flag, characterIndex);
			}
			if (!MatchManager.Instance.CanInstaCast(MatchManager.Instance.CardActive))
			{
				if (MatchManager.Instance.CheckTarget(base.transform))
				{
					if (MatchManager.Instance.CardItemActive != null)
					{
						MatchManager.Instance.CardItemActive.SetColorArrow("green");
					}
					OutlineWhite();
					if (_hero != null)
					{
						MatchManager.Instance.combatTarget.SetTargetTMP(_hero);
					}
					else
					{
						MatchManager.Instance.combatTarget.SetTargetTMP(_npc);
					}
				}
				else if (MatchManager.Instance.CardItemActive != null)
				{
					MatchManager.Instance.CardItemActive.SetColorArrow("red");
				}
			}
		}
		else
		{
			if (_hero != null)
			{
				MatchManager.Instance.combatTarget.SetTargetTMP(_hero);
				if (_hero.SourceName == "Magnus")
				{
					PetMagnus();
				}
				if (_hero.SourceName == "Yogger")
				{
					PetYogger();
				}
			}
			else
			{
				MatchManager.Instance.combatTarget.SetTargetTMP(_npc);
			}
			OutlineGray();
			GameManager.Instance.SetCursorHover();
			GameManager.Instance.PlayLibraryAudio("castnpccardfast");
			ShowHelp(state: true);
		}
		MatchManager.Instance.PortraitHighlight(state: true, this);
	}

	private void PetMagnus()
	{
		if (!MatchManager.Instance)
		{
			return;
		}
		petMagnusCounter++;
		if (petMagnusCounter > 5)
		{
			anim.ResetTrigger("pet");
			anim.SetTrigger("pet");
			petMagnusCounter = 0;
			if (petMagnusAnswer == 0)
			{
				MatchManager.Instance.DoComic(_hero, Texts.Instance.GetText("magnusPet1"), 2f);
				petMagnusAnswer = 1;
			}
			else if (petMagnusAnswer == 1)
			{
				MatchManager.Instance.DoComic(_hero, Texts.Instance.GetText("magnusPet2"), 2f);
				petMagnusAnswer = 0;
			}
		}
		if (petMagnusCoroutine != null)
		{
			StopCoroutine(petMagnusCoroutine);
		}
		petMagnusCoroutine = StartCoroutine(PetMagnusStop());
	}

	private IEnumerator PetMagnusStop()
	{
		yield return Globals.Instance.WaitForSeconds(1.5f);
		petMagnusCounter = 0;
	}

	private void PetYogger()
	{
		if (!MatchManager.Instance)
		{
			return;
		}
		petYoggerCounter++;
		if (petYoggerCounter > 5)
		{
			anim.ResetTrigger("pet");
			anim.SetTrigger("pet");
			petYoggerCounter = 0;
			if (petYoggerAnswer == 0)
			{
				MatchManager.Instance.DoComic(_hero, Texts.Instance.GetText("yoggerPet1"), 2f);
				petYoggerAnswer = 1;
			}
			else if (petYoggerAnswer == 1)
			{
				MatchManager.Instance.DoComic(_hero, Texts.Instance.GetText("yoggerPet2"), 2f);
				petYoggerAnswer = 0;
			}
		}
		if (petYoggerCoroutine != null)
		{
			StopCoroutine(petYoggerCoroutine);
		}
		petYoggerCoroutine = StartCoroutine(PetYoggerStop());
	}

	private IEnumerator PetYoggerStop()
	{
		yield return Globals.Instance.WaitForSeconds(1.5f);
		petYoggerCounter = 0;
	}

	public void fOnMouseUp()
	{
		if (!Functions.ClickedThisTransform(base.transform) || !(MatchManager.Instance != null))
		{
			return;
		}
		if (!MatchManager.Instance.CardDrag)
		{
			if (!MatchManager.Instance.justCasted)
			{
				ShowHelp(state: false);
				if (_hero != null)
				{
					MatchManager.Instance.ShowCharacterWindow("stats", isHero: true, _hero.HeroIndex);
				}
				else
				{
					MatchManager.Instance.ShowCharacterWindow("stats", isHero: false, _npc.NPCIndex);
				}
			}
		}
		else if (MatchManager.Instance.controllerClickedCard)
		{
			MatchManager.Instance.ControllerExecute();
		}
	}

	public void fOnMouseOver()
	{
		if (!SettingsManager.Instance.IsActive() && !AlertManager.Instance.IsActive() && !MadnessManager.Instance.IsActive() && !SandboxManager.Instance.IsActive() && !EventSystem.current.IsPointerOverGameObject() && MatchManager.Instance != null && !MatchManager.Instance.CardDrag && !MatchManager.Instance.justCasted && Input.GetMouseButtonUp(1))
		{
			ShowHelp(state: false);
			if (_hero != null)
			{
				MatchManager.Instance.ShowCharacterWindow("combatdeck", isHero: true, _hero.HeroIndex);
			}
			else
			{
				MatchManager.Instance.ShowCharacterWindow("combatdiscard", isHero: false, _npc.NPCIndex);
			}
		}
	}

	public void fOnMouseExit()
	{
		if (MatchManager.Instance != null)
		{
			MatchManager.Instance.combatTarget.ClearTarget();
			ShowHelp(state: false);
		}
		if (MatchManager.Instance.CardDrag)
		{
			if (MatchManager.Instance.CardItemActive != null)
			{
				if (GameManager.Instance.IsMultiplayer() && MatchManager.Instance.IsYourTurn())
				{
					MatchManager.Instance.StopArrowNet(MatchManager.Instance.CardItemActive.tablePosition);
				}
				if (!MatchManager.Instance.CanInstaCast(MatchManager.Instance.CardActive))
				{
					MatchManager.Instance.CardItemActive.SetColorArrow("gold");
					MatchManager.Instance.SetGlobalOutlines(state: true, MatchManager.Instance.CardActive);
				}
			}
		}
		else
		{
			if (popupSheet != null)
			{
				popupSheet.ClosePopup();
			}
			OutlineHide();
			GameManager.Instance.SetCursorPlain();
		}
		MatchManager.Instance.PortraitHighlight(state: false, this);
	}

	public void ShowHelp(bool state)
	{
		if (!state)
		{
			if (helpCo != null)
			{
				StopCoroutine(helpCo);
			}
			MatchManager.Instance.helpCharacterTransform.gameObject.SetActive(value: false);
		}
		else
		{
			helpCo = StartCoroutine(ShowHelpCo());
		}
	}

	private IEnumerator ShowHelpCo()
	{
		if (helpCo != null)
		{
			StopCoroutine(helpCo);
		}
		yield return Globals.Instance.WaitForSeconds(0.5f);
		if (_hero != null)
		{
			MatchManager.Instance.helpRight.text = Texts.Instance.GetText("helpDeck");
		}
		else
		{
			MatchManager.Instance.helpRight.text = Texts.Instance.GetText("helpCasted");
		}
		MatchManager.Instance.helpCharacterTransform.gameObject.SetActive(value: true);
	}

	private void ResetMaterial()
	{
		if (_hero != null)
		{
			_hero.SetTaunt();
			_hero.SetStealth();
			_hero.SetParalyze();
		}
		else if (_npc != null)
		{
			_npc.SetTaunt();
			_npc.SetStealth();
			_npc.SetParalyze();
		}
	}

	public void OutlineGreen()
	{
		if (animatedSprites != null)
		{
			for (int i = 0; i < animatedSprites.Count; i++)
			{
				if (animatedSprites[i].gameObject.name.ToLower() != "shadow")
				{
					animatedSprites[i].color = new Color(1f, 1f, 1f, 1f);
				}
			}
			ResetMaterial();
		}
		else
		{
			spriteOutline.EnableGreen();
			charImageSR.color = new Color(1f, 1f, 1f, 1f);
		}
	}

	public void OutlineRed()
	{
		if (animatedSprites != null)
		{
			for (int i = 0; i < animatedSprites.Count; i++)
			{
				if (animatedSprites[i].gameObject.name.ToLower() != "shadow")
				{
					animatedSprites[i].color = new Color(0.3f, 0.3f, 0.3f, 1f);
				}
				animatedSprites[i].sharedMaterial = animatedSpritesDefaultMaterial[animatedSprites[i].name];
			}
		}
		else
		{
			spriteOutline.EnableRed();
			charImageSR.color = new Color(0.5f, 0.5f, 0.5f, 1f);
		}
	}

	public void OutlineGray()
	{
		if (animatedSprites != null)
		{
			for (int i = 0; i < animatedSprites.Count; i++)
			{
				if (animatedSprites[i] != null)
				{
					if (animatedSprites[i].gameObject.name.ToLower() != "shadow")
					{
						animatedSprites[i].color = new Color(0.6f, 0.6f, 0.6f, 1f);
					}
					animatedSprites[i].sharedMaterial = animatedSpritesDefaultMaterial[animatedSprites[i].name];
				}
			}
		}
		else
		{
			spriteOutline.EnableWhite();
			charImageSR.color = new Color(0.6f, 0.6f, 0.6f, 1f);
		}
	}

	public void OutlineWhite()
	{
		if (animatedSprites != null)
		{
			for (int i = 0; i < animatedSprites.Count; i++)
			{
				if (animatedSprites[i] != null)
				{
					if (animatedSprites[i].gameObject.name.ToLower() != "shadow")
					{
						animatedSprites[i].color = new Color(0.55f, 1f, 0.56f, 1f);
					}
					animatedSprites[i].sharedMaterial = animatedSpritesDefaultMaterial[animatedSprites[i].name];
				}
			}
		}
		else
		{
			spriteOutline.EnableWhite();
			charImageSR.color = new Color(1f, 1f, 1f, 1f);
		}
	}

	public void OutlineHide()
	{
		if (animatedSprites != null)
		{
			for (int i = 0; i < animatedSprites.Count; i++)
			{
				if (animatedSprites[i] != null)
				{
					animatedSprites[i].color = new Color(1f, 1f, 1f, 1f);
				}
			}
			ResetMaterial();
		}
		else
		{
			spriteOutline.Hide();
			charImageSR.color = new Color(1f, 1f, 1f, 1f);
		}
	}

	public void ShowEnchantments()
	{
		if (isHero)
		{
			if (iconEnchantment != null)
			{
				if (_hero == null || _hero.Enchantment == "")
				{
					if (iconEnchantment.gameObject.activeSelf)
					{
						iconEnchantment.gameObject.SetActive(value: false);
						iconEnchantment.StopCardAnimation();
					}
				}
				else if (_hero.Alive)
				{
					iconEnchantment.gameObject.SetActive(value: true);
					iconEnchantment.ShowIconExternal("enchantment", _hero);
					iconEnchantment.TheHero = _hero;
				}
			}
			if (iconEnchantment2 != null)
			{
				if (_hero == null || _hero.Enchantment2 == "")
				{
					if (iconEnchantment2.gameObject.activeSelf)
					{
						iconEnchantment2.gameObject.SetActive(value: false);
						iconEnchantment2.StopCardAnimation();
					}
				}
				else if (_hero.Alive)
				{
					iconEnchantment2.gameObject.SetActive(value: true);
					iconEnchantment2.ShowIconExternal("enchantment2", _hero);
					iconEnchantment2.TheHero = _hero;
				}
			}
			if (iconEnchantment3 != null)
			{
				if (_hero == null || _hero.Enchantment3 == "")
				{
					if (iconEnchantment3.gameObject.activeSelf)
					{
						iconEnchantment3.gameObject.SetActive(value: false);
						iconEnchantment3.StopCardAnimation();
					}
				}
				else if (_hero.Alive)
				{
					iconEnchantment3.gameObject.SetActive(value: true);
					iconEnchantment3.ShowIconExternal("enchantment3", _hero);
					iconEnchantment3.TheHero = _hero;
				}
			}
			_hero.ShowPetsFromEnchantments();
			return;
		}
		if (iconEnchantment != null)
		{
			if (_npc == null || _npc.Enchantment == "")
			{
				if (iconEnchantment.gameObject.activeSelf)
				{
					iconEnchantment.gameObject.SetActive(value: false);
					iconEnchantment.StopCardAnimation();
				}
			}
			else if (_npc.Alive)
			{
				iconEnchantment.gameObject.SetActive(value: true);
				iconEnchantment.ShowIconExternal("enchantment", _npc);
				iconEnchantment.TheNPC = _npc;
			}
		}
		if (iconEnchantment2 != null)
		{
			if (_npc == null || _npc.Enchantment2 == "")
			{
				if (iconEnchantment2.gameObject.activeSelf)
				{
					iconEnchantment2.gameObject.SetActive(value: false);
					iconEnchantment2.StopCardAnimation();
				}
			}
			else if (_npc.Alive)
			{
				iconEnchantment2.gameObject.SetActive(value: true);
				iconEnchantment2.ShowIconExternal("enchantment2", _npc);
				iconEnchantment2.TheNPC = _npc;
			}
		}
		if (iconEnchantment3 != null)
		{
			if (_npc == null || _npc.Enchantment3 == "")
			{
				if (iconEnchantment3.gameObject.activeSelf)
				{
					iconEnchantment3.gameObject.SetActive(value: false);
					iconEnchantment3.StopCardAnimation();
				}
			}
			else if (_npc.Alive)
			{
				iconEnchantment3.gameObject.SetActive(value: true);
				iconEnchantment3.ShowIconExternal("enchantment3", _npc);
				iconEnchantment3.TheNPC = _npc;
			}
		}
		_npc.ShowPetsFromEnchantments();
	}

	public void HideEnchatmentIcons()
	{
		iconEnchantment?.gameObject.SetActive(value: false);
		iconEnchantment2?.gameObject.SetActive(value: false);
		iconEnchantment3?.gameObject.SetActive(value: false);
	}

	public IEnumerator EnchantEffectCo()
	{
		yield return Globals.Instance.WaitForSeconds(0.3f);
		counterEffectItemOwner++;
		if (counterEffectItemOwner >= 10)
		{
			ItemData itemData = null;
			if (_hero != null)
			{
				if (indexEffectItemOwner == 0 && _hero.Enchantment != "")
				{
					itemData = Globals.Instance.GetItemData(_hero.Enchantment);
				}
				else if (indexEffectItemOwner == 1 && _hero.Enchantment2 != "")
				{
					itemData = Globals.Instance.GetItemData(_hero.Enchantment2);
				}
				else if (indexEffectItemOwner == 2 && _hero.Enchantment3 != "")
				{
					itemData = Globals.Instance.GetItemData(_hero.Enchantment3);
				}
				if (itemData != null && itemData.EffectItemOwner != "")
				{
					EffectsManager.Instance.PlayEffectAC(itemData.EffectItemOwner, isHero: true, CharImageT, flip: false);
				}
				counterEffectItemOwner = 0;
				indexEffectItemOwner++;
				if (indexEffectItemOwner > 2)
				{
					indexEffectItemOwner = 0;
				}
				if (indexEffectItemOwner == 0 && _hero.Enchantment == "")
				{
					indexEffectItemOwner = 1;
				}
				if (indexEffectItemOwner == 1 && _hero.Enchantment2 == "")
				{
					indexEffectItemOwner = 2;
				}
				if (indexEffectItemOwner == 2 && _hero.Enchantment3 == "")
				{
					indexEffectItemOwner = 0;
				}
			}
			else if (_npc != null)
			{
				if (indexEffectItemOwner == 0 && _npc.Enchantment != "")
				{
					itemData = Globals.Instance.GetItemData(_npc.Enchantment);
				}
				else if (indexEffectItemOwner == 1 && _npc.Enchantment2 != "")
				{
					itemData = Globals.Instance.GetItemData(_npc.Enchantment2);
				}
				else if (indexEffectItemOwner == 2 && _npc.Enchantment3 != "")
				{
					itemData = Globals.Instance.GetItemData(_npc.Enchantment3);
				}
				if (itemData != null && itemData.EffectItemOwner != "")
				{
					EffectsManager.Instance.PlayEffectAC(itemData.EffectItemOwner, isHero: true, CharImageT, flip: false);
				}
				counterEffectItemOwner = 0;
				indexEffectItemOwner++;
				if (indexEffectItemOwner > 2)
				{
					indexEffectItemOwner = 0;
				}
				if (indexEffectItemOwner == 0 && _npc.Enchantment == "")
				{
					indexEffectItemOwner = 1;
				}
				if (indexEffectItemOwner == 1 && _npc.Enchantment2 == "")
				{
					indexEffectItemOwner = 2;
				}
				if (indexEffectItemOwner == 2 && _npc.Enchantment3 == "")
				{
					indexEffectItemOwner = 0;
				}
			}
		}
		StartCoroutine(EnchantEffectCo());
	}

	public void EnchantmentExecute(int index)
	{
		if ((_hero == null || _hero.Alive) && (_npc == null || _npc.Alive))
		{
			switch (index)
			{
			case 0:
				iconEnchantment.SetTimesExecuted(1);
				break;
			case 1:
				iconEnchantment2.SetTimesExecuted(1);
				break;
			case 2:
				iconEnchantment3.SetTimesExecuted(1);
				break;
			}
		}
	}

	public void ShowCharacterPing(int _action)
	{
		Character character = null;
		character = ((_hero == null) ? ((Character)_npc) : ((Character)_hero));
		emoteCharacterPing.Show(character.Id, _action);
	}

	public void HideCharacterPing()
	{
		emoteCharacterPing.Hide();
	}

	public void ShowKeyNum(bool _state, string _num = "", bool _disabled = false)
	{
		if (keyTransform.gameObject.activeSelf != _state)
		{
			keyTransform.gameObject.SetActive(_state);
		}
		if (!_state)
		{
			return;
		}
		keyNumber.text = _num;
		if (_disabled)
		{
			if (!keyRed.gameObject.activeSelf)
			{
				keyRed.gameObject.SetActive(value: true);
			}
			if (keyBackground.gameObject.activeSelf)
			{
				keyBackground.gameObject.SetActive(value: false);
			}
		}
		else
		{
			if (keyRed.gameObject.activeSelf)
			{
				keyRed.gameObject.SetActive(value: false);
			}
			if (!keyBackground.gameObject.activeSelf)
			{
				keyBackground.gameObject.SetActive(value: true);
			}
		}
	}
}
