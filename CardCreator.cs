using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CardCreator : MonoBehaviour
{
	public Dropdown[] dropElements;

	private List<string> cardList = new List<string>();

	private bool created;

	private void OnEnable()
	{
		dropElements[1].onValueChanged.AddListener(delegate(int index)
		{
			dropElements[3].gameObject.SetActive(index == 5);
		});
	}

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
			GenerateCards();
			Dropdown[] array = dropElements;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].options.Clear();
			}
			dropElements[0].AddOptions(cardList);
			List<string> list = new List<string>();
			list.Add("Hand");
			list.Add("Discard");
			list.Add("TopDeck");
			list.Add("BottomDeck");
			list.Add("RandomDeck");
			list.Add("EnemyDeck");
			dropElements[1].AddOptions(list);
			List<string> list2 = new List<string>();
			list2.Add("1");
			list2.Add("2");
			list2.Add("3");
			dropElements[2].AddOptions(list2);
			dropElements[3].AddOptions(new List<string> { "0", "1", "2", "3" });
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

	private void GenerateCards()
	{
		StartCoroutine(GenerateCardsWait());
	}

	private IEnumerator GenerateCardsWait()
	{
		while (Globals.Instance.Cards == null)
		{
			yield return Globals.Instance.WaitForSeconds(0.1f);
		}
		foreach (KeyValuePair<string, CardData> card in Globals.Instance.Cards)
		{
			cardList.Add(card.Value.Id);
		}
		cardList.Sort();
	}

	public async void GenerateAction()
	{
		string text = dropElements[0].options[dropElements[0].value].text;
		string text2 = dropElements[2].options[dropElements[2].value].text;
		Enums.CardPlace cardPlace;
		switch (dropElements[1].options[dropElements[1].value].text)
		{
		case "Hand":
			cardPlace = Enums.CardPlace.Hand;
			break;
		case "Discard":
			cardPlace = Enums.CardPlace.Discard;
			break;
		case "TopDeck":
			cardPlace = Enums.CardPlace.TopDeck;
			break;
		case "BottomDeck":
			cardPlace = Enums.CardPlace.BottomDeck;
			break;
		case "EnemyDeck":
			await CastNpcCard(text);
			return;
		default:
			cardPlace = Enums.CardPlace.RandomDeck;
			break;
		}
		MatchManager.Instance.CardCreatorAction(int.Parse(text2), text, createCard: true, cardPlace, fromNet: false);
	}

	private async Task CastNpcCard(string card)
	{
		int npcIndex = dropElements[3].value;
		float castDelay = 0f;
		MatchManager.Instance.GetNPCHand(npcIndex).Clear();
		MatchManager.Instance.GetNPCHand(npcIndex).Add(card);
		NPC nPC = MatchManager.Instance.GetTeamNPC()[npcIndex];
		nPC.CreateOverDeck(getCardFromDeck: true, maxOneCard: true);
		nPC.NPCItem.cardsCI[0].SetCard(card, deckScale: false, null, nPC);
		AI.DoAI(MatchManager.Instance.GetNPCCharacter(npcIndex), MatchManager.Instance.GetTeamHero(), MatchManager.Instance.GetTeamNPC(), ref castDelay);
		await Task.Delay(1000);
		MatchManager.Instance.GetNPCCharacter(npcIndex).CastCardNPCEnd();
	}
}
