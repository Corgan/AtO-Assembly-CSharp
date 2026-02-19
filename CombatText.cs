using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CombatText : MonoBehaviour
{
	public GameObject CTI_Prefab;

	private Queue<Action> ctQueue = new Queue<Action>();

	private Queue<Action> ctQueueDamage = new Queue<Action>();

	private int indexQueue;

	private int indexQueueDamage;

	private bool isWaitingQueueDamage;

	private bool isWaitingQueue;

	private CharacterItem characterItem;

	private Dictionary<string, float> TextShowed = new Dictionary<string, float>();

	private float timePassed = 0.7f;

	private void Awake()
	{
		characterItem = base.transform.parent.transform.parent.transform.parent.GetComponent<CharacterItem>();
	}

	private void Start()
	{
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private void SetTimer()
	{
		if ((bool)this)
		{
			StartCoroutine(TimerCo());
		}
	}

	private IEnumerator TimerCo()
	{
		if (isWaitingQueue)
		{
			indexQueue++;
			if (indexQueue > 7)
			{
				isWaitingQueue = false;
				if (ctQueue.Count > 0)
				{
					ctQueue.Dequeue()();
				}
			}
		}
		if (isWaitingQueueDamage)
		{
			indexQueueDamage++;
			if (indexQueueDamage > 4)
			{
				isWaitingQueueDamage = false;
				if (ctQueueDamage.Count > 0)
				{
					ctQueueDamage.Dequeue()();
				}
			}
		}
		yield return Globals.Instance.WaitForSeconds(0.1f);
		if (isWaitingQueue || isWaitingQueueDamage)
		{
			SetTimer();
		}
	}

	private void LaunchInstance(CastResolutionForCombatText _cast)
	{
		if ((bool)this && (bool)characterItem && (bool)MatchManager.Instance && (bool)MatchManager.Instance.combattextTransform && (bool)CTI_Prefab)
		{
			GameObject obj = UnityEngine.Object.Instantiate(CTI_Prefab, new Vector3(base.transform.position.x, 1.5f, 0f), Quaternion.identity, MatchManager.Instance.combattextTransform);
			CombatTextInstance component = obj.GetComponent<CombatTextInstance>();
			obj.transform.position = new Vector3(characterItem.transform.position.x, 1.2f, 0f);
			component.ShowDamage(this, characterItem, _cast);
		}
	}

	private void LaunchInstanceText(string text, Enums.CombatScrollEffectType type)
	{
		if (this != null && base.transform != null && characterItem != null)
		{
			GameObject obj = UnityEngine.Object.Instantiate(CTI_Prefab, new Vector3(base.transform.position.x, 1.5f, 0f), Quaternion.identity, MatchManager.Instance.combattextTransform);
			CombatTextInstance component = obj.GetComponent<CombatTextInstance>();
			obj.transform.position = new Vector3(characterItem.transform.position.x, 1.2f, 0f);
			component.ShowText(this, characterItem, text, type);
		}
	}

	public void SetDamageNew(CastResolutionForCombatText _cast)
	{
		if (!isWaitingQueueDamage)
		{
			isWaitingQueueDamage = true;
			LaunchInstance(_cast);
			indexQueueDamage = 0;
		}
		else
		{
			ctQueueDamage.Enqueue(delegate
			{
				SetDamageNew(_cast);
			});
		}
		SetTimer();
	}

	public void SetText(string text, Enums.CombatScrollEffectType type, bool forceIt = false)
	{
		bool flag = true;
		StringBuilder stringBuilder = new StringBuilder();
		if (!forceIt && type != Enums.CombatScrollEffectType.Damage && type != Enums.CombatScrollEffectType.Heal)
		{
			stringBuilder.Append(text);
			stringBuilder.Append("_");
			stringBuilder.Append(Enum.GetName(typeof(Enums.CombatScrollEffectType), type));
			if (TextShowed.TryGetValue(stringBuilder.ToString(), out var _) && Time.time < TextShowed[stringBuilder.ToString()] + timePassed)
			{
				flag = false;
				TextShowed[stringBuilder.ToString()] = Time.time;
			}
		}
		if (!flag)
		{
			return;
		}
		if (!isWaitingQueue)
		{
			isWaitingQueue = true;
			LaunchInstanceText(text, type);
			indexQueue = 0;
		}
		else
		{
			ctQueue.Enqueue(delegate
			{
				SetText(text, type, forceIt: true);
			});
		}
		SetTimer();
		if (stringBuilder.ToString() != "")
		{
			if (TextShowed.ContainsKey(stringBuilder.ToString()))
			{
				TextShowed[stringBuilder.ToString()] = Time.time;
			}
			else
			{
				TextShowed.Add(stringBuilder.ToString(), Time.time);
			}
		}
	}

	public bool IsPlaying()
	{
		if (isWaitingQueue || ctQueue.Count > 0)
		{
			return true;
		}
		return false;
	}
}
