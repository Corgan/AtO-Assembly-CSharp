using UnityEngine;

public class Effects : MonoBehaviour
{
	public Transform childTransform;

	public Animator childAnimator;

	private SpriteRenderer childSpr;

	private void Awake()
	{
		childSpr = childTransform.GetComponent<SpriteRenderer>();
	}

	public void Play(string effect, Transform theTransform, bool isHero, bool flip, bool castInCenter)
	{
		if (!(base.transform == null) && !(theTransform == null) && !(effect == ""))
		{
			if (castInCenter)
			{
				base.transform.localPosition = new Vector3(0f, 1.4f, 1f);
			}
			else if (effect == "smoke" && ((isHero && flip) || (!isHero && !flip)))
			{
				base.transform.localPosition = new Vector3(0f, 1.4f, 0f);
			}
			else
			{
				base.transform.localPosition = new Vector3(theTransform.position.x, 1.4f, 1f);
				base.transform.localPosition -= new Vector3(theTransform.localPosition.x, 0f, 0f);
			}
			PlayEffect(effect, flip);
		}
	}

	public void Play(string effect, float posX, bool isHero, bool flip, bool castInCenter)
	{
		if (!string.IsNullOrEmpty(effect))
		{
			if (castInCenter)
			{
				base.transform.localPosition = new Vector3(0f, 1.4f, 1f);
			}
			else if (effect == "smoke" && ((isHero && flip) || (!isHero && !flip)))
			{
				base.transform.localPosition = new Vector3(0f, 1.4f, 0f);
			}
			else
			{
				base.transform.localPosition = new Vector3(posX, 1.4f, 1f);
			}
			PlayEffect(effect, flip);
		}
	}

	private void PlayEffect(string effect, bool flip)
	{
		GameObject resourceEffect = Globals.Instance.GetResourceEffect(effect);
		if (resourceEffect == null)
		{
			Debug.LogError("Effect doesn't exists => " + effect);
			return;
		}
		if ((bool)resourceEffect.GetComponentInChildren<ParticleSystem>() || resourceEffect.GetComponentsInChildren<SpriteRenderer>().Length > 1 || resourceEffect.GetComponentsInChildren<Animator>().Length > 1)
		{
			childTransform.rotation = Quaternion.identity;
			Object.Destroy(Object.Instantiate(resourceEffect, childTransform), 2.5f);
			return;
		}
		childTransform.localPosition = resourceEffect.transform.localPosition;
		childTransform.localRotation = resourceEffect.transform.localRotation;
		childTransform.localScale = resourceEffect.transform.localScale;
		if (resourceEffect.GetComponent<Animator>() != null)
		{
			childAnimator.runtimeAnimatorController = resourceEffect.GetComponent<Animator>().runtimeAnimatorController;
		}
		if (childSpr != null)
		{
			if (resourceEffect.GetComponent<SpriteRenderer>() != null)
			{
				childSpr.sortingOrder = resourceEffect.GetComponent<SpriteRenderer>().sortingOrder;
				childSpr.color = resourceEffect.GetComponent<SpriteRenderer>().color;
			}
			else
			{
				childAnimator.runtimeAnimatorController = null;
				Object.Destroy(Object.Instantiate(resourceEffect, childTransform.position, resourceEffect.transform.rotation), 2.5f);
			}
			if (flip)
			{
				childSpr.flipX = true;
			}
			else
			{
				childSpr.flipX = false;
			}
		}
	}

	public bool HasStopped()
	{
		if (childTransform.childCount > 0)
		{
			return false;
		}
		if (childAnimator == null || childAnimator.runtimeAnimatorController == null)
		{
			return true;
		}
		if (childAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
		{
			return true;
		}
		return false;
	}

	public void Stop(string effect)
	{
		childAnimator.runtimeAnimatorController = null;
	}
}
