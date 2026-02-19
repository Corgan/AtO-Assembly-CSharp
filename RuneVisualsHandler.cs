using UnityEngine;

public class RuneVisualsHandler : ModelVisualsUpdater
{
	[SerializeField]
	private GameObject redRuneVfx;

	[SerializeField]
	private GameObject blueRuneVfx;

	[SerializeField]
	private GameObject greenRuneVfx;

	internal override void UpdateVisuals()
	{
		if (thisCharacter == null || !thisCharacter.Alive)
		{
			return;
		}
		if (blueRuneVfx != null)
		{
			GameObject obj = blueRuneVfx;
			if ((object)obj != null)
			{
				Character character = thisCharacter;
				obj.SetActive(character != null && character.GetAuraCharges("runeblue") > 0);
			}
		}
		if (redRuneVfx != null)
		{
			GameObject obj2 = redRuneVfx;
			if ((object)obj2 != null)
			{
				Character character2 = thisCharacter;
				obj2.SetActive(character2 != null && character2.GetAuraCharges("runered") > 0);
			}
		}
		if (greenRuneVfx != null)
		{
			GameObject obj3 = greenRuneVfx;
			if ((object)obj3 != null)
			{
				Character character3 = thisCharacter;
				obj3.SetActive(character3 != null && character3.GetAuraCharges("runegreen") > 0);
			}
		}
	}
}
