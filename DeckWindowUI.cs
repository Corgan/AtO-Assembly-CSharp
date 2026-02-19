using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckWindowUI : MonoBehaviour
{
	public Transform injuryTitle;

	public TMP_Text injuryText;

	public Transform injuryContent;

	public Transform deckTitle;

	public TMP_Text deckText;

	public Transform deckContent;

	public Transform unlockedTitle;

	public Transform unlockedContent;

	public Transform upgradedTitle;

	public Transform scrollContent;

	private List<string> deckCards = new List<string>();

	private List<string> injuryCards = new List<string>();

	private int currentIndex = -1;

	private void Start()
	{
		Resize();
	}

	public bool IsActive()
	{
		return base.gameObject.activeSelf;
	}

	public void ShowUnlockedCards(List<string> _unlockedCards)
	{
		if (_unlockedCards == null || _unlockedCards.Count == 0)
		{
			return;
		}
		List<string> list = new List<string>();
		CardData cardData = null;
		for (int i = 0; i < _unlockedCards.Count; i++)
		{
			cardData = Globals.Instance.GetCardData(_unlockedCards[i], instantiate: false);
			if (cardData != null && cardData.ShowInTome)
			{
				list.Add(_unlockedCards[i]);
			}
		}
		unlockedTitle.gameObject.SetActive(value: true);
		upgradedTitle.gameObject.SetActive(value: false);
		deckTitle.gameObject.SetActive(value: false);
		injuryTitle.gameObject.SetActive(value: false);
		injuryContent.gameObject.SetActive(value: false);
		Show(-1, list);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Texts.Instance.GetText("unlockedCards"));
		stringBuilder.Append(" <color=#AAA>[<color=#888>");
		stringBuilder.Append(list.Count);
		stringBuilder.Append("</color>]</color>");
		if (unlockedTitle.GetChild(0) != null && unlockedTitle.GetChild(0).GetComponent<TMP_Text>() != null)
		{
			unlockedTitle.GetChild(0).GetComponent<TMP_Text>().text = stringBuilder.ToString();
		}
	}

	public void ShowUpgradedCards(List<string> upgradedCards)
	{
		upgradedTitle.gameObject.SetActive(value: true);
		unlockedTitle.gameObject.SetActive(value: false);
		deckTitle.gameObject.SetActive(value: false);
		injuryTitle.gameObject.SetActive(value: false);
		injuryContent.gameObject.SetActive(value: false);
		List<string> list = new List<string>();
		for (int i = 0; i < upgradedCards.Count; i++)
		{
			list.Add("char_" + i + "_" + upgradedCards[i]);
		}
		Show(-1, list, discard: false, sort: false);
		AtOManager.Instance.upgradedCardsList = new List<string>();
	}

	public void Show(int index = -1, List<string> listCards = null, bool discard = false, bool sort = true, bool isHero = true)
	{
		if (!MatchManager.Instance)
		{
			if (index > -1)
			{
				SetDecks(index);
			}
			if (listCards != null)
			{
				SetList(listCards, sort);
			}
		}
		else if (listCards != null)
		{
			SetList(listCards, sort);
		}
		else if (index > -1)
		{
			StartCoroutine(SetCombatDeck(index, discard));
		}
	}

	public void DestroyDeck()
	{
		if (deckContent == null)
		{
			return;
		}
		foreach (Transform item in deckContent)
		{
			Object.Destroy(item.gameObject);
		}
		GameManager.Instance.CleanTempContainer();
		PopupManager.Instance.ClosePopup();
	}

	public void Hide()
	{
		DestroyDeck();
	}

	public void Resize()
	{
	}

	public void SetList(List<string> cardList, bool sort)
	{
		if (sort)
		{
			cardList.Sort();
		}
		foreach (Transform item in deckContent)
		{
			Object.Destroy(item.gameObject);
		}
		for (int i = 0; i < cardList.Count; i++)
		{
			SetCard(null, 0, cardList[i]);
		}
	}

	private string formatNum(int num)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(" <color=#AAA>[<color=#888>");
		stringBuilder.Append(num);
		stringBuilder.Append("</color>]</color>");
		return stringBuilder.ToString();
	}

	public void HideInjury()
	{
		injuryTitle.gameObject.SetActive(value: false);
		injuryContent.gameObject.SetActive(value: false);
	}

	public void SetTitle(string title, int num = -1)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(title);
		if (num > -1)
		{
			stringBuilder.Append(formatNum(num));
		}
		deckText.text = stringBuilder.ToString();
	}

	public void SetDecks(int heroIndex)
	{
		currentIndex = heroIndex;
		Hero hero = AtOManager.Instance.GetHero(heroIndex);
		if (hero == null)
		{
			return;
		}
		deckCards.Clear();
		injuryCards.Clear();
		for (int i = 0; i < hero.Cards.Count; i++)
		{
			CardData cardData = Globals.Instance.GetCardData(hero.Cards[i], instantiate: false);
			if (cardData != null)
			{
				if (cardData.CardClass != Enums.CardClass.Injury && cardData.CardClass != Enums.CardClass.Boon)
				{
					deckCards.Add(cardData.Id);
				}
				else
				{
					injuryCards.Add(cardData.Id);
				}
			}
		}
		deckCards.Sort();
		injuryCards.Sort();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Texts.Instance.GetText("heroCards").Replace("<hero>", hero.SourceName));
		stringBuilder.Append(formatNum(deckCards.Count));
		deckText.text = stringBuilder.ToString();
		foreach (Transform item in deckContent)
		{
			Object.Destroy(item.gameObject);
		}
		for (int j = 0; j < deckCards.Count; j++)
		{
			SetCard(hero, 0, deckCards[j]);
		}
		if (injuryCards.Count == 0)
		{
			injuryTitle.gameObject.SetActive(value: false);
			injuryContent.gameObject.SetActive(value: false);
			return;
		}
		stringBuilder.Clear();
		stringBuilder.Append(Texts.Instance.GetText("heroInjuriesBoons").Replace("<hero>", hero.SourceName));
		stringBuilder.Append(formatNum(injuryCards.Count));
		injuryText.text = stringBuilder.ToString();
		injuryTitle.gameObject.SetActive(value: true);
		injuryContent.gameObject.SetActive(value: true);
		foreach (Transform item2 in injuryContent)
		{
			Object.Destroy(item2.gameObject);
		}
		for (int k = 0; k < injuryCards.Count; k++)
		{
			SetCard(hero, 1, injuryCards[k]);
		}
	}

	private void SetCard(Hero hero, int type, string cardId)
	{
		Transform transform = null;
		GameObject obj = Object.Instantiate(parent: unlockedTitle.gameObject.activeSelf ? unlockedContent : ((type != 0) ? injuryContent : deckContent), original: GameManager.Instance.CardPrefab, position: base.transform.position, rotation: Quaternion.identity);
		obj.AddComponent(typeof(ContentSizeFitter));
		CardItem component = obj.GetComponent<CardItem>();
		obj.name = "TMP_" + type + "_" + cardId;
		string[] array = cardId.Split('_');
		if (array[0] == "char")
		{
			component.SetCard(array[2], deckScale: true, hero);
		}
		else
		{
			component.SetCard(cardId, deckScale: true, hero);
		}
		component.TopLayeringOrder("UI", 20000);
		component.transform.localScale = Vector3.zero;
		component.SetDestinationLocalScale(1.02f);
		component.cardmakebig = true;
		component.cardmakebigSize = 1.02f;
		component.cardmakebigSizeMax = 1.2f;
		component.active = true;
		component.lockPosition = true;
		component.DisableTrail();
		component.CreateColliderAdjusted();
	}

	public IEnumerator SetCombatDeck(int heroIndex, bool discard)
	{
		currentIndex = heroIndex;
		Hero hero = AtOManager.Instance.GetHero(heroIndex);
		if (hero == null)
		{
			yield break;
		}
		foreach (Transform item in deckContent)
		{
			Object.Destroy(item.gameObject);
		}
		injuryText.gameObject.SetActive(value: false);
		injuryTitle.gameObject.SetActive(value: false);
		StringBuilder stringBuilder = new StringBuilder();
		if (!discard)
		{
			List<string> heroDeck = MatchManager.Instance.GetHeroDeck(currentIndex);
			deckCards.Clear();
			for (int i = 0; i < heroDeck.Count; i++)
			{
				deckCards.Add(heroDeck[i]);
			}
			deckCards.Sort();
			stringBuilder.Append(Texts.Instance.GetText("heroDrawPile").Replace("<hero>", hero.SourceName));
		}
		else
		{
			List<string> heroDiscard = MatchManager.Instance.GetHeroDiscard(currentIndex);
			List<string> list = new List<string>();
			for (int num = heroDiscard.Count - 1; num >= 0; num--)
			{
				list.Add(heroDiscard[num]);
			}
			deckCards.Clear();
			for (int j = 0; j < list.Count; j++)
			{
				deckCards.Add(list[j]);
			}
			stringBuilder.Append(Texts.Instance.GetText("heroDiscardPile").Replace("<hero>", hero.SourceName));
		}
		if (deckCards != null)
		{
			stringBuilder.Append(formatNum(deckCards.Count));
			deckText.text = stringBuilder.ToString();
			int totalCards = deckCards.Count;
			for (int k = 0; k < deckCards.Count; k++)
			{
				SetCombatCard(hero, deckCards[k], k, totalCards);
				yield return null;
			}
		}
	}

	private void SetCombatCard(Hero hero, string cardId, int position, int total)
	{
		GameObject obj = Object.Instantiate(GameManager.Instance.CardPrefab, base.transform.position, Quaternion.identity, deckContent);
		obj.AddComponent(typeof(ContentSizeFitter));
		CardItem component = obj.GetComponent<CardItem>();
		obj.name = "TMP_" + cardId;
		component.SetCard(cardId, deckScale: true, hero);
		component.TopLayeringOrder("UI", 20000);
		component.transform.localScale = Vector3.zero;
		component.SetDestinationLocalScale(1.02f);
		component.cardmakebig = true;
		component.cardmakebigSize = 1.02f;
		component.cardmakebigSizeMax = 1.2f;
		component.active = true;
		component.lockPosition = true;
		component.DisableTrail();
		component.CreateColliderAdjusted();
	}
}
