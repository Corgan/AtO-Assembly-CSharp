using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class CharacterReward : MonoBehaviour
{
	public Transform buttonCharacterDeck;

	public TMP_Text buttonCharacterDeckText;

	public Transform characterImg;

	private SpriteRenderer characterImgSR;

	public Transform chooseCards;

	private TMP_Text chooseCardsText;

	public Transform chooseDust;

	private TMP_Text chooseDustText;

	public Transform quantityDust;

	public TMP_Text quantityDustText;

	public SpriteRenderer borderSPR;

	public Transform cardsTransform;

	public TMP_Text playerNick;

	public Transform combatRewardData;

	public Transform goldT;

	public TMP_Text combatRewardGold;

	public Transform experienceT;

	public TMP_Text combatRewardExperience;

	public Dictionary<string, CardItem> cardsByInternalId;

	private CardItem cardSelected;

	public BotonGeneric botonDust;

	private string ownerNick = "";

	private int index;

	private Dictionary<string, GameObject> cardsGO;

	private bool selected;

	private bool isMyReward = true;

	private Hero hero;

	private void Awake()
	{
		characterImgSR = characterImg.GetComponent<SpriteRenderer>();
		chooseCardsText = chooseCards.GetComponent<TMP_Text>();
		chooseDustText = chooseDust.GetComponent<TMP_Text>();
		buttonCharacterDeck.gameObject.SetActive(value: false);
	}

	public void Init(int _index)
	{
		index = _index;
		hero = RewardsManager.Instance.theTeam[_index];
		ownerNick = hero.Owner;
		buttonCharacterDeck.GetComponent<BotonRollover>().auxInt = index;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Texts.Instance.GetText("deck"));
		stringBuilder.Append("\n<color=#bbb><size=-.5>");
		stringBuilder.Append(string.Format(Texts.Instance.GetText("cardsNum"), hero.Cards.Count));
		buttonCharacterDeckText.text = stringBuilder.ToString();
		StartCoroutine(Show());
	}

	private IEnumerator Show()
	{
		cardsGO = new Dictionary<string, GameObject>();
		quantityDust.gameObject.SetActive(value: false);
		if (GameManager.Instance.IsMultiplayer())
		{
			if (ownerNick != NetworkManager.Instance.GetPlayerNick())
			{
				isMyReward = false;
			}
			playerNick.gameObject.SetActive(value: true);
			playerNick.text = "<" + NetworkManager.Instance.GetPlayerNickReal(ownerNick) + ">";
			TMP_Text tMP_Text = playerNick;
			Color color = (borderSPR.color = Functions.HexToColor(NetworkManager.Instance.GetColorFromNick(ownerNick)));
			tMP_Text.color = color;
		}
		else
		{
			playerNick.gameObject.SetActive(value: false);
		}
		if (RewardsManager.Instance.typeOfReward == 1)
		{
			combatRewardData.gameObject.SetActive(value: true);
			if (RewardsManager.Instance.goldEach > 0)
			{
				combatRewardGold.text = RewardsManager.Instance.goldEach.ToString();
				AtOManager.Instance.GivePlayer(0, RewardsManager.Instance.goldEach, ownerNick);
			}
			else
			{
				goldT.gameObject.SetActive(value: false);
			}
			if (RewardsManager.Instance.experienceEach > 0)
			{
				int experience = hero.CalculateRewardForCharacter(RewardsManager.Instance.experienceEach);
				combatRewardExperience.text = experience.ToString();
				hero.GrantExperience(experience);
			}
			else
			{
				experienceT.gameObject.SetActive(value: false);
			}
		}
		else
		{
			combatRewardData.gameObject.SetActive(value: false);
		}
		if (RewardsManager.Instance.combatScarabDust > 0)
		{
			AtOManager.Instance.GivePlayer(1, RewardsManager.Instance.combatScarabDust, ownerNick);
		}
		quantityDustText.text = RewardsManager.Instance.dustQuantity.ToString();
		characterImgSR.sprite = RewardsManager.Instance.theTeam[index].HeroData.HeroSubClass.SpriteBorder;
		cardsByInternalId = new Dictionary<string, CardItem>();
		yield return Globals.Instance.WaitForSeconds(0.1f);
		characterImg.gameObject.SetActive(value: true);
		buttonCharacterDeck.transform.localPosition = new Vector3(-7f, 0.5f, buttonCharacterDeck.transform.localPosition.z);
		buttonCharacterDeck.gameObject.SetActive(value: true);
		if (isMyReward)
		{
			characterImgSR.color = new Color(1f, 1f, 1f, 1f);
			chooseCards.gameObject.SetActive(value: true);
		}
		else
		{
			characterImgSR.color = new Color(1f, 1f, 1f, 0.3f);
		}
		int _cardIndexPosition = 0;
		for (int i = 0; i < 4; i++)
		{
			yield return Globals.Instance.WaitForSeconds(0.05f);
			if (RewardsManager.Instance.cardsByOrder[index].Length > i && RewardsManager.Instance.cardsByOrder[index][i] != null)
			{
				yield return Globals.Instance.WaitForSeconds(0.1f);
				DoCard(RewardsManager.Instance.cardsByOrder[index][i], _cardIndexPosition);
				_cardIndexPosition++;
			}
		}
		yield return Globals.Instance.WaitForSeconds(0.05f);
		if (!(chooseDust == null) && !(quantityDust == null) && isMyReward)
		{
			chooseDust.gameObject.SetActive(value: true);
			yield return Globals.Instance.WaitForSeconds(0.1f);
			quantityDust.gameObject.SetActive(value: true);
			PlayerManager.Instance.GoldGainedSum(RewardsManager.Instance.goldEach, save: false);
		}
	}

	private void DoCard(string cardName, int position)
	{
		GameObject gameObject = Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, cardsTransform);
		cardsGO.Add(cardName, gameObject);
		CardItem component = gameObject.GetComponent<CardItem>();
		gameObject.name = "card_" + cardName + "_" + index + "_" + position;
		component.SetCard(cardName, deckScale: false, hero);
		component.DisableCollider();
		gameObject.transform.localPosition = new Vector3(-1.9f + (float)position * 1.95f, 0f, 0f);
		component.DoReward();
		if (!isMyReward)
		{
			component.ShowDisableReward();
		}
		cardsByInternalId.Add(gameObject.name, component);
		PlayerManager.Instance.CardUnlock(cardName, save: false, component);
	}

	public void DustSelected(string playerNick)
	{
		if (playerNick == "" || RewardsManager.Instance.theTeam[index].Owner == playerNick)
		{
			ShowSelected("dust");
			RewardsManager.Instance.DustSelected(index);
			PlayerManager.Instance.DustGainedSum(RewardsManager.Instance.dustQuantity, save: false);
		}
	}

	public void CardSelected(string playerNick, string internalId)
	{
		if (cardsByInternalId != null && cardsByInternalId.ContainsKey(internalId) && (playerNick == "" || RewardsManager.Instance.theTeam[index].Owner == playerNick))
		{
			if (cardSelected != null)
			{
				cardSelected.DrawBorder("");
			}
			cardSelected = cardsByInternalId[internalId];
			ShowSelected(cardSelected.CardData.Id);
			RewardsManager.Instance.CardSelected(index, cardSelected.CardData.Id);
			PlayerManager.Instance.CardUnlock(cardSelected.CardData.Id, save: false, cardSelected);
		}
	}

	public void ShowSelected(string rewardId)
	{
		if (chooseDust == null || quantityDust == null || chooseCards == null || combatRewardData == null || selected)
		{
			return;
		}
		selected = true;
		chooseDust.gameObject.SetActive(value: false);
		chooseCards.gameObject.SetActive(value: false);
		combatRewardData.gameObject.SetActive(value: false);
		playerNick.gameObject.SetActive(value: false);
		if (rewardId == "dust")
		{
			if (cardsGO != null)
			{
				foreach (KeyValuePair<string, GameObject> item in cardsGO)
				{
					Object.Destroy(item.Value);
				}
			}
			quantityDust.gameObject.SetActive(value: true);
			botonDust.buttonEnabled = false;
			botonDust.ShowBorder(state: false);
			botonDust.ShowBackgroundPlain(state: false);
			quantityDust.localPosition = new Vector3(-4f, -0.3f, 0f);
			quantityDust.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
			return;
		}
		Object.Destroy(quantityDust.gameObject);
		Transform transform = null;
		if (cardsGO != null)
		{
			foreach (KeyValuePair<string, GameObject> item2 in cardsGO)
			{
				if (item2.Key != rewardId)
				{
					Object.Destroy(item2.Value);
				}
				else
				{
					transform = item2.Value.transform;
				}
			}
		}
		if (transform != null)
		{
			transform.localPosition = new Vector3(-4f, 0f, 0f);
			PlayerManager.Instance.CardUnlock(rewardId, save: false, transform.GetComponent<CardItem>());
		}
	}

	private void HideGO()
	{
		base.gameObject.SetActive(value: false);
	}
}
