using UnityEngine;

public class SkillLog : MonoBehaviour
{
	public Transform borderRed;

	public Transform borderGreen;

	public SpriteRenderer sprite;

	private SkillForLog SFL;

	private CardItem CI;

	private float distanceBetweenIcons = 0.55f;

	private void Position(int index, bool isHero)
	{
		float num = -0.2f - (float)index * distanceBetweenIcons;
		if (isHero)
		{
			base.transform.localPosition = new Vector3(num, 0f, -1f);
		}
		else
		{
			base.transform.localPosition = new Vector3(0f - num, 0f, -1f);
		}
	}

	public void SetSkill(SkillForLog _SFL, int index, bool isHero)
	{
		SFL = _SFL;
		sprite.sprite = SFL.cardData.Sprite;
		if (SFL.isFromHero)
		{
			borderRed.gameObject.SetActive(value: false);
		}
		else
		{
			borderGreen.gameObject.SetActive(value: false);
		}
		Position(index, isHero);
	}

	private void DestroyCard()
	{
		if (CI != null)
		{
			CI.HideKeyNotes();
		}
		Object.Destroy(MatchManager.Instance.skillLogCard);
		MatchManager.Instance.ShowMask(state: false);
	}

	private void ShowCard()
	{
		if (MatchManager.Instance.skillLogCard != null)
		{
			Object.Destroy(MatchManager.Instance.skillLogCard);
		}
		Vector3 position = base.transform.position + new Vector3(0f, 2.6f, 0f);
		MatchManager.Instance.skillLogCard = Object.Instantiate(GameManager.Instance.CardPrefab, position, Quaternion.identity);
		CI = MatchManager.Instance.skillLogCard.GetComponent<CardItem>();
		CI.SetCard(SFL.cardData.InternalId, deckScale: false);
		MatchManager.Instance.skillLogCard.transform.localScale = Vector3.zero;
		CI.DisableCollider();
		CI.DisableTrail();
		CI.TopLayeringOrder("UI");
		CI.SetDestinationScaleRotation(position, 1.4f, Quaternion.Euler(0f, 0f, 0f));
		CI.discard = true;
		CI.ShowKeyNotes();
		MatchManager.Instance.ShowMask(state: true);
	}

	private void OnMouseEnter()
	{
		ShowCard();
	}

	private void OnMouseExit()
	{
		DestroyCard();
	}
}
