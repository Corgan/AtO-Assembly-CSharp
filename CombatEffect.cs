using System;
using UnityEngine;

[Serializable]
public class CombatEffect
{
	[SerializeField]
	private AuraCurseData auraCurse;

	[SerializeField]
	private int auraCurseCharges;

	[SerializeField]
	private Enums.CombatUnit auraCurseTarget;

	public AuraCurseData AuraCurse
	{
		get
		{
			return auraCurse;
		}
		set
		{
			auraCurse = value;
		}
	}

	public int AuraCurseCharges
	{
		get
		{
			return auraCurseCharges;
		}
		set
		{
			auraCurseCharges = value;
		}
	}

	public Enums.CombatUnit AuraCurseTarget
	{
		get
		{
			return auraCurseTarget;
		}
		set
		{
			auraCurseTarget = value;
		}
	}
}
