public class TeamBonusHotline
{
	private const string wildLander = "wildlander";

	public static int GetCharacterDamageBonus(Character character, int dmg, Enums.DamageType dmgType, Enums.CardClass cardClass, int _energyOnCard)
	{
		if (character != null && character.Alive)
		{
			return character.DamageWithCharacterBonus(dmg, dmgType, cardClass, _energyOnCard);
		}
		return 0;
	}

	public static int GetCharacterAuraCurseBonus(Character character, string acId, Enums.CardClass cardClass)
	{
		if (character != null && character.Alive)
		{
			return character.GetAuraCurseQuantityModification(acId, cardClass);
		}
		return 0;
	}

	public static int GetDamageBonusFromTeamHeroesForPets(CardData cardData, Character caster, int dmg, Enums.DamageType dmgType, Enums.CardClass cardClass, int _energyOnCard)
	{
		if (cardData == null)
		{
			return 0;
		}
		if (!cardData.IsPetCast && !cardData.IsPetAttack)
		{
			return 0;
		}
		if (!caster.IsHero)
		{
			return 0;
		}
		Character heroWithTrait = AtOManager.Instance.GetHeroWithTrait("wildlander");
		if (heroWithTrait != null && caster != null && heroWithTrait.Id != caster.Id)
		{
			return GetCharacterDamageBonus(heroWithTrait, dmg, dmgType, cardClass, _energyOnCard) - dmg;
		}
		return 0;
	}

	public static int GetAuraBonusFromTeamHeroesForPets(Character character, CardData cardData, string acId)
	{
		if (cardData == null)
		{
			return 0;
		}
		if (!cardData.IsPetCast && !cardData.IsPetAttack)
		{
			return 0;
		}
		if (!character.IsHero)
		{
			return 0;
		}
		Character heroWithTrait = AtOManager.Instance.GetHeroWithTrait("wildlander");
		if (heroWithTrait != null && character != null && heroWithTrait.Id != character.Id)
		{
			return heroWithTrait.GetAuraCurseQuantityModification(acId, cardData.CardClass);
		}
		return 0;
	}
}
