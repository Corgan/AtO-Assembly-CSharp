using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
	public Transform elements;

	public Transform goldIcon;

	public Transform dustIcon;

	public Transform supplyIcon;

	public TMP_Text goldText;

	public TMP_Text dustText;

	public TMP_Text supplyText;

	public Transform goldTextAnim;

	public Transform dustTextAnim;

	public Transform supplyTextAnim;

	public Transform giveGold;

	private TMP_Text goldTextAnimText;

	private TMP_Text dustTextAnimText;

	private TMP_Text supplyTextAnimText;

	public TMP_Text bagQuantityText;

	public Transform bagQuantity;

	private int itemsNum;

	public Transform eventItems;

	public Transform bagItems;

	public GameObject eventItemPrefab;

	private bool eventItemsBagOpened = true;

	private List<string> itemsOnBag = new List<string>();

	private Coroutine goldCo;

	private Coroutine dustCo;

	private Coroutine supplyCo;

	public static PlayerUIManager Instance { get; private set; }

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
		Object.DontDestroyOnLoad(base.gameObject);
		goldTextAnimText = goldTextAnim.GetComponent<TMP_Text>();
		dustTextAnimText = dustTextAnim.GetComponent<TMP_Text>();
		supplyTextAnimText = supplyTextAnim.GetComponent<TMP_Text>();
	}

	public void Resize()
	{
		base.transform.localScale = Globals.Instance.scaleV;
		base.transform.position = new Vector3((0f - Globals.Instance.sizeW) * 0.5f + 1.5f * Globals.Instance.scale, Globals.Instance.sizeH * 0.5f - 0.35f * Globals.Instance.scale, base.transform.position.z);
	}

	public bool IsActive()
	{
		return elements.gameObject.activeSelf;
	}

	public void Show()
	{
		if (!elements.gameObject.activeSelf)
		{
			elements.gameObject.SetActive(value: true);
		}
		string sceneName = SceneStatic.GetSceneName();
		if (sceneName == "Rewards" || sceneName == "Loot")
		{
			ShowItems(state: false);
		}
		else
		{
			ShowItems(state: true);
			SetItems();
			ShowBagItems();
			SetSupply();
		}
		if (GameManager.Instance.IsMultiplayer() && (sceneName == "Map" || sceneName == "Town"))
		{
			if (!giveGold.gameObject.activeSelf)
			{
				giveGold.gameObject.SetActive(value: true);
			}
		}
		else if (giveGold.gameObject.activeSelf)
		{
			giveGold.gameObject.SetActive(value: false);
		}
		SetGold();
		SetDust();
	}

	public void Hide()
	{
		if (elements.gameObject.activeSelf)
		{
			elements.gameObject.SetActive(value: false);
		}
	}

	public void ShowItems(bool state)
	{
		if (eventItems.gameObject.activeSelf != state)
		{
			eventItems.gameObject.SetActive(state);
		}
	}

	public void BagToggle()
	{
		eventItemsBagOpened = !eventItemsBagOpened;
		ShowBagItems();
	}

	private void ShowBagItems()
	{
		if (bagItems.gameObject.activeSelf != eventItemsBagOpened)
		{
			bagItems.gameObject.SetActive(eventItemsBagOpened);
		}
		if (itemsNum > 0)
		{
			bagQuantity.gameObject.SetActive(!eventItemsBagOpened);
		}
		else
		{
			bagQuantity.gameObject.SetActive(value: true);
		}
		if (eventItemsBagOpened && itemsNum > 0 && bagItems.childCount == 0)
		{
			SetItems();
		}
	}

	public void RemoveItems()
	{
		foreach (Transform bagItem in bagItems)
		{
			Object.Destroy(bagItem.gameObject);
		}
	}

	public void ClearBag()
	{
		RemoveItems();
		itemsOnBag.Clear();
		eventItemsBagOpened = true;
		itemsNum = 0;
	}

	public void SetItems()
	{
		List<string> playerRequeriments = AtOManager.Instance.GetPlayerRequeriments();
		if (playerRequeriments == null)
		{
			bagQuantityText.text = "0";
			itemsNum = 0;
			RemoveItems();
			return;
		}
		if (eventItemsBagOpened)
		{
			RemoveItems();
		}
		int num = 0;
		for (int i = 0; i < playerRequeriments.Count; i++)
		{
			EventRequirementData requirementData = Globals.Instance.GetRequirementData(playerRequeriments[i]);
			if (!(requirementData != null) || !requirementData.ItemTrack)
			{
				continue;
			}
			if (eventItemsBagOpened)
			{
				GameObject gameObject = Object.Instantiate(eventItemPrefab, Vector3.zero, Quaternion.identity, bagItems);
				gameObject.name = requirementData.RequirementId;
				gameObject.transform.GetChild(0).GetComponent<EventItemTrack>().SetItemTrack(requirementData);
				if (!itemsOnBag.Contains(requirementData.RequirementId))
				{
					itemsOnBag.Add(requirementData.RequirementId);
					gameObject.transform.GetChild(0).transform.GetComponent<Animator>().enabled = true;
					gameObject.transform.localPosition = new Vector3(0.8f * (float)num, 0f, 0f);
				}
				else
				{
					gameObject.transform.localPosition = new Vector3(0.8f * (float)num, -0.416f, 0f);
				}
			}
			num++;
		}
		itemsNum = num;
		bagQuantityText.text = itemsNum.ToString();
	}

	public Vector3 GoldIconPosition()
	{
		return goldIcon.position;
	}

	public Vector3 DustIconPosition()
	{
		return dustIcon.position;
	}

	public void SetGold(bool animation = false)
	{
		goldTextAnimText.text = "";
		if (!animation)
		{
			goldText.text = AtOManager.Instance.GetPlayerGold().ToString();
		}
		else if (base.gameObject.activeSelf)
		{
			StartCoroutine(QuantityAnimation("gold"));
		}
		else
		{
			goldText.text = AtOManager.Instance.GetPlayerGold().ToString();
		}
	}

	public void SetDust(bool animation = false)
	{
		dustTextAnimText.text = "";
		if (!animation)
		{
			dustText.text = AtOManager.Instance.GetPlayerDust().ToString();
			return;
		}
		if (dustCo != null)
		{
			StopCoroutine(dustCo);
		}
		if (base.gameObject.activeSelf)
		{
			dustCo = StartCoroutine(QuantityAnimation("dust"));
		}
		else
		{
			dustText.text = AtOManager.Instance.GetPlayerDust().ToString();
		}
	}

	public void SetSupply(bool animation = false)
	{
		supplyTextAnimText.text = "";
		if (!animation)
		{
			supplyText.text = PlayerManager.Instance.GetPlayerSupplyActual().ToString();
			return;
		}
		if (supplyCo != null)
		{
			StopCoroutine(supplyCo);
		}
		if (base.gameObject.activeSelf)
		{
			supplyCo = StartCoroutine(QuantityAnimation("supply"));
		}
		else
		{
			supplyText.text = PlayerManager.Instance.GetPlayerSupplyActual().ToString();
		}
	}

	private IEnumerator QuantityAnimation(string type, float delay = 0f)
	{
		yield return Globals.Instance.WaitForSeconds(delay + 0.1f);
		int value;
		int end;
		if (type == "gold")
		{
			value = ((!(goldText.text == "")) ? int.Parse(goldText.text) : 0);
			end = AtOManager.Instance.GetPlayerGold();
		}
		else if (type == "dust")
		{
			value = ((!(dustText.text == "")) ? int.Parse(dustText.text) : 0);
			end = AtOManager.Instance.GetPlayerDust();
		}
		else
		{
			value = ((!(supplyText.text == "")) ? int.Parse(supplyText.text) : 0);
			end = PlayerManager.Instance.GetPlayerSupplyActual();
		}
		int increment;
		if (end > value)
		{
			increment = 1;
		}
		else
		{
			if (end >= value)
			{
				yield break;
			}
			increment = -1;
		}
		switch (type)
		{
		case "gold":
			GameManager.Instance.PlayLibraryAudio("ui_coins");
			break;
		case "dust":
			GameManager.Instance.PlayLibraryAudio("ui_gems");
			break;
		case "supply":
			GameManager.Instance.PlayLibraryAudio("ui_gems");
			break;
		}
		int difference = end - value;
		while (value != end)
		{
			int num = ((Mathf.Abs(value - end) > 1000) ? (increment * 104) : ((Mathf.Abs(value - end) > 100) ? (increment * 53) : ((Mathf.Abs(value - end) > 50) ? (increment * 22) : ((Mathf.Abs(value - end) <= 10) ? increment : (increment * 5)))));
			value += num;
			if (type == "gold")
			{
				goldText.text = value.ToString();
			}
			else if (type == "dust")
			{
				dustText.text = value.ToString();
			}
			else
			{
				supplyText.text = value.ToString();
			}
			yield return null;
		}
		if (type == "gold")
		{
			ShowAnim(0, difference);
		}
		else if (type == "dust")
		{
			ShowAnim(1, difference);
		}
		else
		{
			ShowAnim(2, difference);
		}
		yield return null;
	}

	private void ShowAnim(int type, int quantity)
	{
		if (quantity != 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (quantity > 0)
			{
				stringBuilder.Append("+");
			}
			stringBuilder.Append(quantity);
			Transform transform;
			switch (type)
			{
			case 0:
				transform = Object.Instantiate(goldTextAnim);
				transform.parent = goldTextAnim.parent;
				transform.localPosition = goldTextAnim.localPosition;
				break;
			case 1:
				transform = Object.Instantiate(dustTextAnim);
				transform.parent = dustTextAnim.parent;
				transform.localPosition = dustTextAnim.localPosition;
				break;
			default:
				transform = Object.Instantiate(supplyTextAnim);
				transform.parent = supplyTextAnim.parent;
				transform.localPosition = supplyTextAnim.localPosition;
				break;
			}
			transform.gameObject.SetActive(value: false);
			transform.gameObject.SetActive(value: true);
			transform.GetComponent<TMP_Text>().text = stringBuilder.ToString();
			StartCoroutine(HideAnim(transform, type));
		}
	}

	private IEnumerator HideAnim(Transform animText, int type)
	{
		yield return Globals.Instance.WaitForSeconds(1.5f);
		Object.Destroy(animText.gameObject);
		yield return null;
	}
}
