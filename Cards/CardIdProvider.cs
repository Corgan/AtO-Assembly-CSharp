using UnityEngine;

namespace Cards;

public abstract class CardIdProvider : ScriptableObject
{
	public abstract string GetCardID(CardData cardData, Character caster, Character target);
}
