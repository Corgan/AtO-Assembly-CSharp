using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCreator : MonoBehaviour
{
	public Dropdown[] dropElements;

	private List<string> cardList = new List<string>();

	private bool created;

	public void Draw()
	{
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		base.gameObject.SetActive(value: true);
		if (!created)
		{
			GenerateItems();
			Dropdown[] array = dropElements;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].options.Clear();
			}
			dropElements[0].AddOptions(cardList);
			List<string> list = new List<string>();
			list.Add("Hero1");
			list.Add("Hero2");
			list.Add("Hero3");
			list.Add("Hero4");
			dropElements[1].AddOptions(list);
			created = true;
		}
	}

	public void SelectByName(string value)
	{
		int num = -1;
		for (int i = 0; i < cardList.Count; i++)
		{
			if (cardList[i].StartsWith(value))
			{
				num = i;
				break;
			}
		}
		if (num > -1)
		{
			dropElements[0].value = num;
		}
	}

	private void GenerateItems()
	{
		StartCoroutine(GenerateItemsWait());
	}

	private IEnumerator GenerateItemsWait()
	{
		while (Globals.Instance.Cards == null)
		{
			yield return Globals.Instance.WaitForSeconds(0.1f);
		}
		for (int i = 0; i < Globals.Instance.CardListByClass[Enums.CardClass.Item].Count; i++)
		{
			cardList.Add(Globals.Instance.CardListByClass[Enums.CardClass.Item][i]);
		}
		cardList.Sort();
	}

	public void GenerateAction()
	{
		string text = dropElements[0].options[dropElements[0].value].text;
		string text2 = dropElements[1].options[dropElements[1].value].text;
		int heroIndex = 0;
		switch (text2)
		{
		case "Hero1":
			heroIndex = 0;
			break;
		case "Hero2":
			heroIndex = 1;
			break;
		case "Hero3":
			heroIndex = 2;
			break;
		case "Hero4":
			heroIndex = 3;
			break;
		}
		AtOManager.Instance.AddItemToHero(heroIndex, text);
	}
}
