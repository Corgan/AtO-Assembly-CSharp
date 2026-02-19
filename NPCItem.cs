using UnityEngine;
using UnityEngine.UI;

public class NPCItem : CharacterItem
{
	[SerializeField]
	private NPCData npcData;

	public Transform animatedTransform;

	public Transform bossParticles;

	public Transform bossSmallParticles;

	public Transform namedSmallParticles;

	public Transform cardsGOT;

	public Transform[] cardsT;

	public CardItem[] cardsCI;

	public NPCData NpcData
	{
		get
		{
			return npcData;
		}
		set
		{
			npcData = value;
		}
	}

	public override void Awake()
	{
		base.Awake();
	}

	public override void Start()
	{
		base.Start();
	}

	public void Init(NPC _npc, bool useAltModels = false)
	{
		if (npcData == null)
		{
			return;
		}
		base.NPC = _npc;
		base.Hero = null;
		GameObject gameObject = (useAltModels ? npcData.GameObjectAnimatedAlternate : npcData.GameObjectAnimated);
		base.IsHero = false;
		energyT.parent.gameObject.SetActive(value: false);
		GO_Buffs.transform.localPosition = new Vector3(0.03f, -1.05f, 0f);
		if (gameObject != null)
		{
			GameObject gameObject2 = Object.Instantiate(gameObject, Vector3.zero, Quaternion.identity, base.transform);
			animatedTransform = gameObject2.transform;
			gameObject2.transform.localPosition = gameObject.transform.localPosition;
			gameObject2.transform.localRotation = gameObject.transform.localRotation;
			GetComponent<CharacterItem>().SetOriginalLocalPosition(gameObject2.transform.localPosition);
			gameObject2.name = base.transform.name;
			DisableCollider();
			CharacterGOItem characterGOItem = gameObject2.GetComponent<CharacterGOItem>();
			if (characterGOItem == null)
			{
				characterGOItem = gameObject2.AddComponent(typeof(CharacterGOItem)) as CharacterGOItem;
			}
			characterGOItem._characterItem = GetComponent<CharacterItem>();
			base.CharImageSR.sprite = null;
			base.Anim = gameObject2.GetComponent<Animator>();
			animatedSprites.Clear();
			GetSpritesFromAnimated(gameObject2);
			transformForCombatText = gameObject2.transform;
			BoxCollider2D boxCollider2D = gameObject2.GetComponent<BoxCollider2D>();
			if (boxCollider2D == null)
			{
				boxCollider2D = gameObject2.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
			}
			heightModel = boxCollider2D.size.y;
		}
		else
		{
			base.CharImageSR.sprite = npcData.Sprite;
			if (npcData.PosBottom != 0f)
			{
				float y = npcData.PosBottom * (float)Screen.height * 0.001f;
				base.CharImageSR.transform.localPosition = new Vector3(base.CharImageT.localPosition.x, y, base.CharImageT.localPosition.z);
			}
			transformForCombatText = base.transform;
		}
		if (!npcData.BigModel)
		{
			cardsGOT.position = new Vector3(cardsGOT.position.x, 4.2f, cardsGOT.position.z);
		}
		else
		{
			cardsGOT.position = new Vector3(cardsGOT.position.x, 4.8f, cardsGOT.position.z);
		}
		if (npcData.IsBoss)
		{
			if (npcData.BigModel)
			{
				bossParticles.gameObject.SetActive(value: true);
			}
			else
			{
				bossSmallParticles.gameObject.SetActive(value: true);
			}
		}
		else if (npcData.IsNamed)
		{
			namedSmallParticles.gameObject.SetActive(value: true);
		}
		if (npcData.BigModel)
		{
			hpBackground.gameObject.SetActive(value: false);
			hpBackgroundHigh.gameObject.SetActive(value: true);
			hpT.localScale = new Vector3(2.5f, 1.2f, 1f);
			hpT.localPosition = new Vector3(-2.16f, -0.5f, 0f);
			hpShieldT.localScale = new Vector3(1.5f, 1.5f, 1f);
			hpShieldT.localPosition = new Vector3(-0.79f, -0.15f, 0f);
			hpBlockT.localPosition = new Vector3(0.31f, 0.07f, 0f);
			hpBlockIconT.localScale = new Vector3(1.5f, 1.5f, 1f);
			hpBlockIconT.localPosition = new Vector3(-1.19f, -0.1f, 0f);
			base.HpText.fontSize = 3f;
			base.HpText.transform.localPosition = new Vector3(1.38f, 0.07f, 1f);
			GO_Buffs.transform.localScale = new Vector3(1.4f, 1.4f, 1f);
			GO_Buffs.transform.localPosition = new Vector3(0.39f, -1.3f, 0f);
			GO_Buffs.GetComponent<GridLayoutGroup>().cellSize = new Vector3(0.32f, 0.3f, 0f);
			GO_Buffs.GetComponent<GridLayoutGroup>().constraintCount = 8;
			skull.transform.localPosition = new Vector3(0.4f, skull.transform.localPosition.y, skull.transform.localPosition.z);
			skull.transform.localScale = new Vector3(0.45f, 0.45f, 1f);
		}
		ActivateMark(state: false);
		SetHP();
		DrawEnergy();
		if (MatchManager.Instance != null)
		{
			StartCoroutine(EnchantEffectCo());
		}
		if (useAltModels)
		{
			animatedTransform.gameObject.SetActive(value: false);
		}
	}

	public void RemoveAllCards()
	{
		for (int i = 0; i < cardsT.Length; i++)
		{
			if (cardsT[i] != null)
			{
				Object.Destroy(cardsT[i].gameObject);
			}
		}
		cardsT = null;
	}
}
