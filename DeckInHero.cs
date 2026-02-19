using UnityEngine;

public class DeckInHero : MonoBehaviour
{
	public bool isNormalDeck;

	private SpriteRenderer cardSprite;

	public int heroIndex;

	private void Awake()
	{
		cardSprite = GetComponent<SpriteRenderer>();
	}

	private void OnMouseEnter()
	{
		if (!AlertManager.Instance.IsActive() && !SettingsManager.Instance.IsActive() && !MatchManager.Instance.CardDrag)
		{
			base.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
		}
	}

	private void OnMouseExit()
	{
		if (!AlertManager.Instance.IsActive() && !SettingsManager.Instance.IsActive() && !MatchManager.Instance.CardDrag)
		{
			base.transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	private void OnMouseUp()
	{
		if (!AlertManager.Instance.IsActive() && !SettingsManager.Instance.IsActive() && !MatchManager.Instance.CardDrag)
		{
			if (!isNormalDeck)
			{
				MatchManager.Instance.ShowCharacterWindow("combatdiscard", isHero: true, heroIndex);
			}
			else
			{
				MatchManager.Instance.ShowCharacterWindow("combatdeck", isHero: true, heroIndex);
			}
		}
	}
}
