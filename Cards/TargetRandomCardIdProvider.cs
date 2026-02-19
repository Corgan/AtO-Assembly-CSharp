using System.Linq;
using UnityEngine;

namespace Cards;

[CreateAssetMenu(menuName = "CardProviders/TargetRandomCardIdProvider")]
public class TargetRandomCardIdProvider : CardIdProvider
{
	public override string GetCardID(CardData cardData, Character caster, Character target)
	{
		return target?.Cards.ToList().ShuffleList().FirstOrDefault();
	}
}
