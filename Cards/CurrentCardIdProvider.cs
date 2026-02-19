using UnityEngine;

namespace Cards;

[CreateAssetMenu(menuName = "CardProviders/CurrentCardIdProvider")]
public class CurrentCardIdProvider : CardIdProvider
{
	public override string GetCardID(CardData cardData, Character caster, Character target)
	{
		return cardData?.Id;
	}
}
