using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class CardBackSelectionPanel : MonoBehaviour
{
	[Serializable]
	public struct CardStartingPositions
	{
		public string category;

		public string localStringID;

		public Transform refTransform;

		public BotonGeneric botonGeneric;
	}

	[Serializable]
	public struct CardPage
	{
		public GameObject pageButton;

		public GameObject contentContainer;
	}

	public CardStartingPositions[] cardStartingPositions;

	public List<CardPage> cardPages;

	public int cardPerRow = 8;

	public float cardSize = 1.2f;

	public float distX = 1.75f;

	public float distY = 2.7f;

	public float defaultCategoryButtonSize = 1f;

	public float selectedCategoryButtonSize = 1.1f;

	public GameObject PageButtonPrefab;

	public Transform pageButtonStartingPos;

	private void OnEnable()
	{
		cardStartingPositions[0].refTransform.gameObject.SetActive(value: true);
		for (int i = 1; i < cardStartingPositions.Length; i++)
		{
			cardStartingPositions[i].refTransform.gameObject.SetActive(value: false);
		}
		EnableTabFirst();
	}

	public async void EnableTabFirst()
	{
		await ApplyEnableTabFirst(0);
	}

	private async Task ApplyEnableTabFirst(int index)
	{
		await Task.Delay(10);
		for (int i = 0; i < cardStartingPositions.Length; i++)
		{
			cardStartingPositions[i].refTransform.gameObject.SetActive(value: false);
			cardStartingPositions[i].botonGeneric.Enable();
			cardStartingPositions[i].botonGeneric.transform.localScale = Vector3.one * defaultCategoryButtonSize;
		}
		cardStartingPositions[index].refTransform.gameObject.SetActive(value: true);
		cardStartingPositions[index].botonGeneric.transform.localScale = Vector3.one * selectedCategoryButtonSize;
		cardStartingPositions[index].botonGeneric.Disable();
		clearRefChildren(pageButtonStartingPos);
		int childCount = cardStartingPositions[index].refTransform.childCount;
		for (int j = 0; j < childCount; j++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(PageButtonPrefab, pageButtonStartingPos);
			gameObject.transform.localPosition = new Vector3(0f, (float)j * -1.2f, -1f);
			gameObject.GetComponentInChildren<TMP_Text>().text = (j + 1).ToString();
			gameObject.name = "CardBackSelectionPage";
			gameObject.GetComponent<BotonGeneric>().auxInt = j;
			if (j == 0)
			{
				gameObject.GetComponent<BotonGeneric>().Disable();
			}
		}
		if (cardStartingPositions[0].refTransform != null)
		{
			EnablePage(0);
		}
	}

	public void EnableTab(int index)
	{
		for (int i = 0; i < cardStartingPositions.Length; i++)
		{
			cardStartingPositions[i].refTransform.gameObject.SetActive(value: false);
			cardStartingPositions[i].botonGeneric.Enable();
			cardStartingPositions[i].botonGeneric.transform.localScale = Vector3.one * defaultCategoryButtonSize;
		}
		cardStartingPositions[index].refTransform.gameObject.SetActive(value: true);
		cardStartingPositions[index].botonGeneric.transform.localScale = Vector3.one * selectedCategoryButtonSize;
		cardStartingPositions[index].botonGeneric.Disable();
		clearRefChildren(pageButtonStartingPos);
		int childCount = cardStartingPositions[index].refTransform.childCount;
		for (int j = 0; j < childCount; j++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(PageButtonPrefab, pageButtonStartingPos);
			gameObject.transform.localPosition = new Vector3(0f, (float)j * -1.2f, -1f);
			gameObject.GetComponentInChildren<TMP_Text>().text = (j + 1).ToString();
			gameObject.name = "CardBackSelectionPage";
			gameObject.GetComponent<BotonGeneric>().auxInt = j;
			if (j == 0)
			{
				gameObject.GetComponent<BotonGeneric>().Disable();
			}
		}
		if (cardStartingPositions[0].refTransform != null)
		{
			EnablePage(0);
		}
	}

	public void EnablePage(int index)
	{
		for (int i = 0; i < cardStartingPositions.Length; i++)
		{
			if (cardStartingPositions[i].refTransform.gameObject.activeSelf)
			{
				for (int j = 0; j < cardStartingPositions[i].refTransform.childCount; j++)
				{
					cardStartingPositions[i].refTransform.GetChild(j).gameObject.SetActive(value: false);
				}
				if (index >= 0 && index < cardStartingPositions[i].refTransform.childCount)
				{
					cardStartingPositions[i].refTransform.GetChild(index).gameObject.SetActive(value: true);
				}
			}
		}
		BotonGeneric[] componentsInChildren = pageButtonStartingPos.GetComponentsInChildren<BotonGeneric>();
		foreach (BotonGeneric botonGeneric in componentsInChildren)
		{
			if (botonGeneric.auxInt == index)
			{
				botonGeneric.Disable();
			}
			else
			{
				botonGeneric.Enable();
			}
		}
	}

	public void clearRefChildren(Transform transform)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(transform.GetChild(i).gameObject);
		}
	}

	public Transform GetStartingRefTransform(string category)
	{
		CardStartingPositions[] array = this.cardStartingPositions;
		for (int i = 0; i < array.Length; i++)
		{
			CardStartingPositions cardStartingPositions = array[i];
			if (cardStartingPositions.category == category)
			{
				return cardStartingPositions.refTransform;
			}
		}
		return null;
	}

	public void Close()
	{
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: false);
		}
		PopupManager.Instance.ClosePopup();
	}

	public async void UpdateButtonText(string category)
	{
		await ApplyButtonText(category);
	}

	private async Task ApplyButtonText(string category)
	{
		await Task.Delay(10);
		for (int i = 0; i < cardStartingPositions.Length; i++)
		{
			if (category == cardStartingPositions[i].category)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(Texts.Instance.GetText(cardStartingPositions[i].localStringID));
				stringBuilder.Append(" (<color=#FFF><size=+.2>");
				stringBuilder.Append(CountGrandChildren(cardStartingPositions[i].refTransform));
				stringBuilder.Append("</size></color>)");
				cardStartingPositions[i].botonGeneric.SetText(stringBuilder.ToString());
				break;
			}
		}
	}

	private int CountGrandChildren(Transform refTransform)
	{
		int num = 0;
		foreach (Transform item in refTransform)
		{
			num += item.childCount;
		}
		return num;
	}
}
