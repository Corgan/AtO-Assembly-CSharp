using System.Collections;
using UnityEngine;

public class HeroItem : CharacterItem
{
	[SerializeField]
	private HeroData heroData;

	public Transform animatedTransform;

	public int indexInTeam;

	public HeroData HeroData
	{
		get
		{
			return heroData;
		}
		set
		{
			heroData = value;
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

	public void Init(Hero _hero)
	{
		base.Hero = _hero;
		base.NPC = null;
		base.IsHero = true;
		if (_hero.GameObjectAnimated != null)
		{
			GameObject gameObject = Object.Instantiate(_hero.GameObjectAnimated, Vector3.zero, Quaternion.identity, base.transform);
			ModelVisualsUpdater component = gameObject.GetComponent<ModelVisualsUpdater>();
			if (component != null)
			{
				component.SetCharacter(_hero);
				MatchManager.Instance?.modelVisualsUpdaters.Add(component);
			}
			animatedTransform = gameObject.transform;
			gameObject.transform.localPosition = _hero.GameObjectAnimated.transform.localPosition;
			GetComponent<CharacterItem>().SetOriginalLocalPosition(gameObject.transform.localPosition);
			gameObject.name = base.transform.name;
			DisableCollider();
			gameObject.GetComponent<CharacterGOItem>()._characterItem = GetComponent<CharacterItem>();
			base.CharImageSR.sprite = null;
			base.Anim = gameObject.GetComponent<Animator>();
			GetSpritesFromAnimated(gameObject);
			GetSwordSprites(gameObject);
			transformForCombatText = gameObject.transform;
			heightModel = gameObject.GetComponent<BoxCollider2D>().size.y;
		}
		else
		{
			base.CharImageSR.sprite = _hero.HeroSprite;
			transformForCombatText = base.transform;
		}
		ActivateMark(state: false);
		SetHP();
		DrawEnergy();
		CleanSwordSprites();
		if (MatchManager.Instance != null)
		{
			StartCoroutine(EnchantEffectCo());
		}
	}

	private IEnumerator TurnOnOffEnergy(SpriteRenderer SR, bool state, bool incoming = false)
	{
		Color colorFade = new Color(0f, 0f, 0f, 0.1f);
		if (incoming)
		{
			SR.color = new Color(0f, 0.9f, 1f, SR.color.a);
		}
		else
		{
			SR.color = new Color(1f, 1f, 1f, SR.color.a);
		}
		if (!state)
		{
			while (SR.color.a > 0.2f)
			{
				SR.color -= colorFade;
				yield return Globals.Instance.WaitForSeconds(0.05f);
			}
		}
		else
		{
			while (SR.color.a < 1f)
			{
				SR.color += colorFade;
				yield return Globals.Instance.WaitForSeconds(0.05f);
			}
		}
	}
}
