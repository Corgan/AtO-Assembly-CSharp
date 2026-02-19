using UnityEngine;

public class DeckPile : MonoBehaviour
{
	private SpriteRenderer spr;

	private void Awake()
	{
		spr = GetComponent<SpriteRenderer>();
	}

	private void OnMouseUp()
	{
		if (AlertManager.Instance.IsActive() || SettingsManager.Instance.IsActive() || MatchManager.Instance.characterWindow.IsActive())
		{
			return;
		}
		GameManager.Instance.SetCursorPlain();
		if (base.gameObject.name == "discardpile")
		{
			if (MatchManager.Instance.CountHeroDiscard() > 0)
			{
				MatchManager.Instance.ShowCharacterWindow("combatdiscard", isHero: true, MatchManager.Instance.GetHeroActive());
			}
		}
		else
		{
			MatchManager.Instance.ShowCharacterWindow("combatdeck", isHero: true, MatchManager.Instance.GetHeroActive());
		}
	}

	private void OnMouseEnter()
	{
		if (!AlertManager.Instance.IsActive() && !SettingsManager.Instance.IsActive() && !MatchManager.Instance.characterWindow.IsActive() && (!MatchManager.Instance || !MatchManager.Instance.CardDrag))
		{
			if (base.gameObject.name != "discardpile" || MatchManager.Instance.CountHeroDiscard() > 0)
			{
				GameManager.Instance.SetCursorHover();
				GameManager.Instance.PlayLibraryAudio("castnpccardfast");
			}
			if (base.gameObject.name == "discardpile")
			{
				MatchManager.Instance.DeckParticlesShow(1, state: true);
				return;
			}
			MatchManager.Instance.DeckParticlesShow(0, state: true);
			spr.color = new Color(0.9f, 0.8f, 0.1f, 1f);
		}
	}

	private void OnMouseExit()
	{
		if (!AlertManager.Instance.IsActive() && !SettingsManager.Instance.IsActive() && (!MatchManager.Instance || !MatchManager.Instance.CardDrag))
		{
			GameManager.Instance.SetCursorPlain();
			if (base.gameObject.name == "discardpile")
			{
				MatchManager.Instance.DeckParticlesShow(1, state: false);
				return;
			}
			MatchManager.Instance.DeckParticlesShow(0, state: false);
			spr.color = new Color(1f, 1f, 1f, 1f);
		}
	}
}
