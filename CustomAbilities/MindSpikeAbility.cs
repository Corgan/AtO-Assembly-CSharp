using Cards;

namespace CustomAbilities;

public class MindSpikeAbility
{
	private SpecialCardEnum _collectedSpecialCard = SpecialCardEnum.NightmareEcho;

	private int _currentSpecialCardsUsedInMatch;

	public SpecialCardEnum CollectedSpecialCard => _collectedSpecialCard;

	public int CurrentSpecialCardsUsedInMatch
	{
		get
		{
			return _currentSpecialCardsUsedInMatch;
		}
		set
		{
			_currentSpecialCardsUsedInMatch = value;
		}
	}

	public void IncreaseSpecialCardCount()
	{
		_currentSpecialCardsUsedInMatch++;
	}

	public void Reset()
	{
		_currentSpecialCardsUsedInMatch = 0;
	}
}
