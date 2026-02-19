using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeeklySelector : MonoBehaviour
{
	public Dropdown[] dropElements;

	private List<string> weeklyList = new List<string>();

	private SortedDictionary<string, string> weeklyCorrespondence = new SortedDictionary<string, string>();

	private bool created;

	private void Start()
	{
	}

	public void Draw(bool force = false)
	{
		if (base.gameObject.activeSelf && !force)
		{
			base.gameObject.SetActive(value: false);
		}
		else
		{
			base.gameObject.SetActive(value: true);
			if (!created)
			{
				GenerateWeekly();
				Dropdown[] array = dropElements;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].options.Clear();
				}
				dropElements[0].AddOptions(weeklyList);
				created = true;
			}
		}
		if (AtOManager.Instance.weeklyForcedId != "")
		{
			int num = 0;
			foreach (KeyValuePair<string, string> item in weeklyCorrespondence)
			{
				if (item.Value == AtOManager.Instance.weeklyForcedId)
				{
					dropElements[0].value = num;
					return;
				}
				num++;
			}
		}
		dropElements[0].value = -1;
	}

	private void GenerateWeekly()
	{
		StartCoroutine(GenerateWeeklyWait());
	}

	private IEnumerator GenerateWeeklyWait()
	{
		foreach (KeyValuePair<string, ChallengeData> item in Globals.Instance.WeeklyDataSource)
		{
			int week = int.Parse(item.Value.Id.Replace("week", ""));
			string text = AtOManager.Instance.GetWeeklyName(week);
			if (text.Length > 14)
			{
				text = text.Substring(0, 14) + ".";
			}
			text = text + " (" + week + ")";
			weeklyCorrespondence.Add(text, item.Value.Id);
			weeklyList.Add(text);
		}
		weeklyList.Sort();
		yield break;
	}

	public void GenerateAction()
	{
	}
}
