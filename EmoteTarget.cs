using System.Collections;
using UnityEngine;

public class EmoteTarget : MonoBehaviour
{
	public Transform characterT;

	public SpriteRenderer portraitStickerBase;

	public SpriteRenderer icon;

	public SpriteRenderer iconStickerBase;

	private Animator animator;

	private void Awake()
	{
		base.gameObject.SetActive(value: false);
		animator = GetComponent<Animator>();
	}

	private void Start()
	{
		StartCoroutine(DestroyEmote());
	}

	public void SetActiveHeroOnCardEmoteButton()
	{
		if (MatchManager.Instance != null)
		{
			Hero hero = MatchManager.Instance.GetHero(MatchManager.Instance.emoteManager.heroActive);
			if (hero != null && hero.HeroData != null && hero.HeroData.HeroSubClass != null)
			{
				icon.sprite = hero.HeroData.HeroSubClass.StickerBase;
			}
		}
	}

	public void SetIcons(int _heroIndex, int _action)
	{
		if (!(MatchManager.Instance != null))
		{
			return;
		}
		Hero hero = MatchManager.Instance.GetHero(_heroIndex);
		if (hero != null && !(hero.HeroData == null) && !(hero.HeroData.HeroSubClass == null))
		{
			if (MatchManager.Instance.emoteManager.EmoteNeedsTarget(_action))
			{
				characterT.gameObject.SetActive(value: true);
				icon.sprite = MatchManager.Instance.emoteManager.emotesSprite[_action];
				portraitStickerBase.sprite = hero.HeroData.HeroSubClass.GetEmoteBase();
			}
			else
			{
				characterT.gameObject.SetActive(value: false);
				icon.sprite = hero.HeroData.HeroSubClass.GetEmote(_action);
				iconStickerBase.sprite = hero.HeroData.HeroSubClass.GetEmoteBase();
				base.transform.localPosition += new Vector3(hero.HeroData.HeroSubClass.StickerOffsetX, 0f, 0f);
			}
			base.gameObject.SetActive(value: true);
			if (_action != 2 && _action != 3)
			{
				animator.SetTrigger("sticker");
			}
		}
	}

	private IEnumerator DestroyEmote()
	{
		yield return Globals.Instance.WaitForSeconds(4f);
		Dest();
	}

	private void Dest()
	{
		Object.Destroy(base.gameObject);
	}
}
