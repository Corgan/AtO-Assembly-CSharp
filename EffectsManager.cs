using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
	public GameObject effectsPrefab;

	private List<GameObject> listEffects;

	public GameObject slashPrefab;

	private Dictionary<string, float> EffectPlayedTimePassed = new Dictionary<string, float>();

	public static EffectsManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		CreateEffects(60, effectsPrefab);
	}

	public void CreateEffects(int size, GameObject prefab)
	{
		listEffects = new List<GameObject>();
		for (int i = 0; i < size; i++)
		{
			GameObject gameObject = Object.Instantiate(prefab, base.transform);
			listEffects.Add(gameObject);
			gameObject.SetActive(value: false);
		}
	}

	public void PlayEffect(string effectName, float posX)
	{
		StartCoroutine(PlayEffectCo(effectName, posX));
	}

	private IEnumerator PlayEffectCo(string effectName, float posX)
	{
		GameObject effectGO = GetEffect();
		Effects effects = effectGO.GetComponent<Effects>();
		if (!effectGO.gameObject.activeSelf)
		{
			effectGO.gameObject.SetActive(value: true);
		}
		effects.Play(effectName, posX, isHero: false, flip: false, castInCenter: false);
		int exhaust = 0;
		while (!effects.HasStopped() && exhaust < 50)
		{
			yield return Globals.Instance.WaitForSeconds(0.05f);
			exhaust++;
		}
		effects.Stop(effectName);
		DestroyEffect(effectGO);
		yield return null;
	}

	public void PlayEffect(string effectName, Transform theTransform)
	{
		StartCoroutine(PlayEffectCo(effectName, theTransform));
	}

	private IEnumerator PlayEffectCo(string effectName, Transform theTransform)
	{
		GameObject effectGO = GetEffect();
		Effects effects = effectGO.GetComponent<Effects>();
		if (!effectGO.gameObject.activeSelf)
		{
			effectGO.gameObject.SetActive(value: true);
		}
		effects.Play(effectName, theTransform, isHero: false, flip: false, castInCenter: false);
		int exhaust = 0;
		while (!effects.HasStopped() && exhaust < 50)
		{
			yield return Globals.Instance.WaitForSeconds(0.05f);
			exhaust++;
		}
		effects.Stop(effectName);
		DestroyEffect(effectGO);
		yield return null;
	}

	public void PlayEffect(CardData card, bool isCaster, bool isHero, Transform theTransform, float delay = 0f)
	{
		if (GameManager.Instance.ConfigShowEffects)
		{
			StartCoroutine(PlayEffectCo(card, isCaster, isHero, theTransform));
		}
	}

	public void PlayEffect(string TargetEffect, Transform TargetTransform, bool isHero, bool castInCenter, float delay = 0f)
	{
		StartCoroutine(PlayEffectCo(TargetEffect, TargetTransform, isHero, castInCenter, delay));
	}

	private IEnumerator PlayEffectCo(string TargetEffect, Transform TargetTransform, bool isHero, bool castInCenter, float delay = 0f)
	{
		yield return new WaitForSeconds(delay);
		GameObject effect = GetEffect();
		Effects component = effect.GetComponent<Effects>();
		if (!effect.gameObject.activeSelf)
		{
			effect.gameObject.SetActive(value: true);
		}
		component.Play(TargetEffect, TargetTransform, isHero, !isHero, castInCenter);
		StartCoroutine(StopEffectWithDelay(effect, component, TargetEffect, 3f));
	}

	private IEnumerator PlayEffectCo(CardData card, bool isCaster, bool isHero, Transform theTransform, float delay = 0f)
	{
		GameObject effectGO = GetEffect();
		Effects effects = effectGO.GetComponent<Effects>();
		if (!effectGO.gameObject.activeSelf)
		{
			effectGO.gameObject.SetActive(value: true);
		}
		bool flip = false;
		if (card.CardClass == Enums.CardClass.Monster)
		{
			flip = true;
		}
		string effect = ((!isCaster) ? card.EffectTarget : card.EffectCaster);
		string key = effect + theTransform.name;
		float value = 0f;
		float num = 0.2f;
		if (EffectPlayedTimePassed.TryGetValue(key, out value))
		{
			if (Time.time < EffectPlayedTimePassed[key] + num)
			{
				yield break;
			}
			EffectPlayedTimePassed[key] = Time.time;
		}
		else
		{
			EffectPlayedTimePassed.Add(key, Time.time);
		}
		bool castInCenter = false;
		if (isCaster && card.EffectCastCenter)
		{
			castInCenter = true;
		}
		if (isHero)
		{
			effects.Play(effect, theTransform, isHero: true, flip, castInCenter);
		}
		else
		{
			effects.Play(effect, theTransform, isHero: false, flip, castInCenter);
		}
		int _exhaust = 0;
		while (!effects.HasStopped() && _exhaust < 50)
		{
			yield return Globals.Instance.WaitForSeconds(0.05f);
			_exhaust++;
		}
		effects.Stop(effect);
		DestroyEffect(effectGO);
		yield return null;
	}

	public void PlayEffectAC(string effect, bool isHero, Transform theTransform, bool flip, float delay = 0f, bool casterInCenter = false)
	{
		if (GameManager.Instance.ConfigShowEffects)
		{
			StartCoroutine(PlayEffectACCo(effect, isHero, theTransform, flip, delay, casterInCenter));
		}
	}

	private IEnumerator PlayEffectACCo(string effect, bool isHero, Transform theTransform, bool flip, float delay, bool casterInCenter = false)
	{
		yield return Globals.Instance.WaitForSeconds(Time.deltaTime * 60f * delay);
		if (theTransform == null)
		{
			yield break;
		}
		string key = effect + theTransform.name;
		float value = 0f;
		float num = 1f;
		if (EffectPlayedTimePassed.TryGetValue(key, out value))
		{
			if (Time.time < EffectPlayedTimePassed[key] + num)
			{
				yield break;
			}
			EffectPlayedTimePassed[key] = Time.time;
		}
		else
		{
			EffectPlayedTimePassed.Add(key, Time.time);
		}
		GameObject effectGO = GetEffect();
		if (effectGO != null)
		{
			Effects effects = effectGO.GetComponent<Effects>();
			effectGO.gameObject.SetActive(value: true);
			effects.Play(effect, theTransform, isHero, flip, casterInCenter);
			int _exhaust = 0;
			while (!effects.HasStopped() && _exhaust < 50)
			{
				yield return Globals.Instance.WaitForSeconds(Time.deltaTime * 60f * 0.1f);
				_exhaust++;
			}
			effects.Stop(effect);
			DestroyEffect(effectGO);
		}
	}

	public void PlayEffectTrail(CardData card, bool isHero, Transform from, Transform to, int distance)
	{
		if (!GameManager.Instance.ConfigShowEffects || (!GameManager.Instance.IsMultiplayer() && GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Ultrafast))
		{
			MatchManager.Instance.waitingTrail = false;
		}
		else
		{
			StartCoroutine(PlayEffectTrailCo(card, isHero, from, to, distance));
		}
	}

	private IEnumerator PlayEffectTrailCo(CardData card, bool isHero, Transform from, Transform to, int distance)
	{
		if (!GameManager.Instance.IsMultiplayer() && GameManager.Instance.configGameSpeed == Enums.ConfigSpeed.Ultrafast)
		{
			MatchManager.Instance.waitingTrail = false;
			yield break;
		}
		GameObject effectGO = GetEffect();
		if (effectGO == null)
		{
			yield break;
		}
		Effects effects = effectGO.GetComponent<Effects>();
		if (effects == null)
		{
			Debug.LogError("Effects is null");
			yield break;
		}
		if (!effectGO.gameObject.activeSelf)
		{
			effectGO.gameObject.SetActive(value: true);
		}
		if (isHero)
		{
			effects.Play(card.EffectTrail, from, isHero: true, flip: false, castInCenter: false);
		}
		else
		{
			effects.Play(card.EffectTrail, from, isHero: false, flip: true, castInCenter: false);
		}
		new Vector3(to.position.x, 1.4f, to.position.z);
		Transform effectT = effectGO.transform;
		effectT.position = new Vector3(from.position.x, 1.4f, from.position.z);
		if (card.EffectTrailAngle == Enums.EffectTrailAngle.Parabolic || card.EffectTrailAngle == Enums.EffectTrailAngle.Horizontal)
		{
			if (to.position.x > from.position.x)
			{
				effectT.position += new Vector3(0.6f, 0f, 0f);
			}
			else
			{
				effectT.position -= new Vector3(0.6f, 0f, 0f);
			}
		}
		if (card.CardClass == Enums.CardClass.Monster)
		{
			effectT.localScale = new Vector3(effectT.localScale.x * -1f, effectT.localScale.y * -1f, effectT.localScale.z);
		}
		if (card.EffectTrailAngle == Enums.EffectTrailAngle.Diagonal || card.EffectTrailAngle == Enums.EffectTrailAngle.Vertical)
		{
			float num = (float)Screen.height * 0.005f;
			float num2 = (float)Screen.width * 0.0015f;
			bool goingRight = true;
			float num3 = 0f - effectT.localPosition.y + 1.8f;
			if (card.EffectTrailAngle == Enums.EffectTrailAngle.Vertical)
			{
				float iterationDelay = 0.01f;
				float speed = 6f * card.EffectTrailSpeed;
				speed = ((!GameManager.Instance.IsMultiplayer() && GameManager.Instance.configGameSpeed != Enums.ConfigSpeed.Fast && GameManager.Instance.configGameSpeed != Enums.ConfigSpeed.Ultrafast) ? (speed * 1.2f) : (speed * 1.5f));
				Vector3 position = new Vector3(to.position.x, num + 2f, to.position.z);
				effectT.position = position;
				Vector3 targetPos = new Vector3(to.position.x, 1.4f, to.position.z);
				bool finished = false;
				effectT.rotation = Quaternion.Euler(0f, 0f, 90f);
				while (!finished)
				{
					Vector3 position2 = Vector3.MoveTowards(effectT.position, targetPos, speed * Time.deltaTime);
					Debug.Log(position2.y);
					effectT.position = position2;
					if (Mathf.Abs(position2.y - targetPos.y) < 0.1f)
					{
						finished = true;
					}
					yield return Globals.Instance.WaitForSeconds(Time.deltaTime * 60f * iterationDelay);
				}
			}
			else
			{
				num3 -= 0.2f;
				if (to.position.x > from.position.x)
				{
					effectT.localRotation = Quaternion.Euler(0f, 0f, -45f);
					effectT.position = to.position + new Vector3(0f - num2 - num3, num + 2f, 0f);
				}
				else
				{
					goingRight = false;
					effectT.localRotation = Quaternion.Euler(0f, 0f, 45f);
					effectT.position = to.position + new Vector3(num2 + num3, num + 2f, 0f);
				}
				float speed = 40f;
				float iterationDelay = num2 / speed;
				float stepY = num / speed;
				effectT.position += new Vector3(0f, stepY, 0f);
				for (int i = 0; (float)i < speed; i++)
				{
					if (card.EffectTrailAngle == Enums.EffectTrailAngle.Vertical)
					{
						effectT.position += new Vector3(0f, 0f - stepY, 0f);
					}
					else if (goingRight)
					{
						effectT.position += new Vector3(iterationDelay, 0f - stepY, 0f);
					}
					else
					{
						effectT.position += new Vector3(0f - iterationDelay, 0f - stepY, 0f);
					}
					yield return Globals.Instance.WaitForSeconds(Time.deltaTime * 60f * 0.01f);
				}
			}
		}
		else
		{
			float num4 = Globals.Instance.sizeW * 0.8f;
			float num5 = Globals.Instance.sizeW * 0.2f;
			float stepY = 0.01f;
			float iterationDelay = 14f * card.EffectTrailSpeed;
			iterationDelay = ((!GameManager.Instance.IsMultiplayer() && GameManager.Instance.configGameSpeed != Enums.ConfigSpeed.Fast && GameManager.Instance.configGameSpeed != Enums.ConfigSpeed.Ultrafast) ? (iterationDelay * 1.7f) : (iterationDelay * 1.85f));
			float x = 0.4f;
			Vector3 targetPos = new Vector3(from.position.x, 1.4f, from.position.z);
			Vector3 targetPos2 = new Vector3(to.position.x - to.localPosition.x, 1.4f, to.position.z);
			if (targetPos.x < targetPos2.x)
			{
				targetPos += new Vector3(x, 0f, 0f);
				targetPos2 -= new Vector3(x, 0f, 0f);
			}
			else
			{
				targetPos -= new Vector3(x, 0f, 0f);
				targetPos2 += new Vector3(x, 0f, 0f);
			}
			float num6 = Mathf.Abs(targetPos.x - targetPos2.x);
			effectT.position = targetPos;
			if (card.EffectTrailAngle == Enums.EffectTrailAngle.Parabolic)
			{
				bool goingRight = false;
				float num7 = 0.1f;
				float num8 = 4f;
				float speed = Mathf.Clamp(num7 + (num6 - num5) / (num4 - num5) * (num8 - num7), num7, num8);
				while (!goingRight)
				{
					float x2 = targetPos.x;
					float x3 = targetPos2.x;
					float num9 = x3 - x2;
					float num10 = Mathf.MoveTowards(effectT.position.x, x3, iterationDelay * Time.deltaTime);
					float num11 = Mathf.Lerp(targetPos.y, targetPos2.y, (num10 - x2) / num9);
					float num12 = speed * (num10 - x2) * (num10 - x3) / (-0.25f * num9 * num9);
					Vector3 vector = new Vector3(num10, num11 + num12, effectT.position.z);
					effectT.rotation = LookAt2D(vector - effectT.position);
					effectT.position = vector;
					if (Mathf.Abs(vector.x - targetPos2.x) < 0.1f)
					{
						goingRight = true;
					}
					yield return Globals.Instance.WaitForSeconds(Time.deltaTime * 60f * stepY);
				}
			}
			else
			{
				bool goingRight = false;
				while (!goingRight)
				{
					Vector3 vector2 = Vector3.MoveTowards(effectT.position, targetPos2, iterationDelay * Time.deltaTime);
					effectT.rotation = LookAt2D(vector2 - effectT.position);
					effectT.position = vector2;
					if (Mathf.Abs(vector2.x - targetPos2.x) < 0.1f)
					{
						goingRight = true;
					}
					yield return Globals.Instance.WaitForSeconds(Time.deltaTime * 60f * stepY);
				}
			}
		}
		effectT.localRotation = Quaternion.Euler(0f, 0f, 0f);
		MatchManager.Instance.waitingTrail = false;
		effects.Stop(card.EffectTrail);
		DestroyEffect(effectGO);
	}

	private Quaternion LookAt2D(Vector2 forward)
	{
		return Quaternion.Euler(0f, 0f, Mathf.Atan2(forward.y, forward.x) * 57.29578f);
	}

	public GameObject GetEffect()
	{
		if (listEffects.Count > 0)
		{
			GameObject gameObject = listEffects[0];
			if (gameObject.transform.localScale.x < 0f)
			{
				gameObject.transform.localScale = new Vector3(Mathf.Abs(gameObject.transform.localScale.x), Mathf.Abs(gameObject.transform.localScale.y), gameObject.transform.localScale.z);
			}
			listEffects.RemoveAt(0);
			return gameObject;
		}
		return null;
	}

	public void DestroyEffect(GameObject obj)
	{
		if (obj.gameObject.activeSelf)
		{
			obj.gameObject.SetActive(value: false);
		}
		listEffects.Add(obj);
	}

	private IEnumerator StopEffectWithDelay(GameObject obj, Effects effects, string effect, float delay)
	{
		yield return new WaitForSeconds(delay);
		effects.Stop(effect);
		DestroyEffect(obj);
	}

	public void ClearEffects()
	{
		listEffects.Clear();
		listEffects = null;
	}
}
