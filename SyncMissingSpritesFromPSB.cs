using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.U2D.Animation;

[ExecuteInEditMode]
public class SyncMissingSpritesFromPSB : MonoBehaviour
{
	[Header("Drop the NEW PSB prefab here")]
	public GameObject referencePSB;

	public void SyncMissingSprites()
	{
		if (referencePSB == null)
		{
			Debug.LogError("Reference PSB prefab is not assigned.");
			return;
		}
		Transform transform = base.transform;
		Transform transform2 = referencePSB.transform;
		Dictionary<string, Transform> dictionary = new Dictionary<string, Transform>();
		Dictionary<string, Transform> dictionary2 = new Dictionary<string, Transform>();
		Transform[] componentsInChildren = transform.GetComponentsInChildren<Transform>(includeInactive: true);
		foreach (Transform transform3 in componentsInChildren)
		{
			dictionary[transform3.name] = transform3;
		}
		componentsInChildren = transform2.GetComponentsInChildren<Transform>(includeInactive: true);
		foreach (Transform transform4 in componentsInChildren)
		{
			dictionary2[transform4.name] = transform4;
		}
		int num = 0;
		foreach (KeyValuePair<string, Transform> item in dictionary2)
		{
			string key = item.Key;
			Transform value = item.Value;
			if (dictionary.ContainsKey(key))
			{
				continue;
			}
			GameObject gameObject = Object.Instantiate(value.gameObject);
			gameObject.name = key;
			if (value.parent != null && dictionary.TryGetValue(value.parent.name, out var value2))
			{
				gameObject.transform.SetParent(value2, worldPositionStays: false);
			}
			else
			{
				gameObject.transform.SetParent(transform, worldPositionStays: false);
			}
			gameObject.transform.localPosition = value.localPosition;
			gameObject.transform.localRotation = value.localRotation;
			gameObject.transform.localScale = value.localScale;
			SpriteRenderer component = value.GetComponent<SpriteRenderer>();
			SpriteRenderer component2 = gameObject.GetComponent<SpriteRenderer>();
			if ((bool)component && (bool)component2)
			{
				component2.sortingOrder = component.sortingOrder;
			}
			SpriteSkin component3 = value.GetComponent<SpriteSkin>();
			if (component3 != null)
			{
				SpriteSkin spriteSkin = gameObject.GetComponent<SpriteSkin>() ?? gameObject.AddComponent<SpriteSkin>();
				PropertyInfo property = typeof(SpriteSkin).GetProperty("rootBone", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (property != null && component3.rootBone != null && dictionary.TryGetValue(component3.rootBone.name, out var value3))
				{
					property.SetValue(spriteSkin, value3);
				}
				PropertyInfo property2 = typeof(SpriteSkin).GetProperty("boneTransforms", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (property2 != null)
				{
					List<Transform> list = new List<Transform>();
					componentsInChildren = component3.boneTransforms;
					foreach (Transform transform5 in componentsInChildren)
					{
						if (transform5 != null && dictionary.TryGetValue(transform5.name, out var value4))
						{
							list.Add(value4);
						}
						else
						{
							list.Add(null);
						}
					}
					property2.SetValue(spriteSkin, list.ToArray());
				}
				(typeof(SpriteSkin).Assembly.GetType("UnityEngine.U2D.Animation.SpriteSkinUtility")?.GetMethod("ResetBindPose", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))?.Invoke(null, new object[1] { spriteSkin });
				spriteSkin.enabled = true;
			}
			gameObject.SetActive(value: true);
			gameObject.hideFlags = HideFlags.None;
			num++;
		}
		Debug.Log($"✅ Synced {num} missing sprite(s).");
	}
}
