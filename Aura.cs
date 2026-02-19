using System;
using UnityEngine;

[Serializable]
public class Aura
{
	[SerializeField]
	private AuraCurseData acData;

	[SerializeField]
	private int auraCharges;

	public AuraCurseData ACData
	{
		get
		{
			return acData;
		}
		set
		{
			acData = value;
		}
	}

	public int AuraCharges
	{
		get
		{
			return auraCharges;
		}
		set
		{
			auraCharges = value;
		}
	}

	public void SetAura(AuraCurseData _acData, int _auraCharges)
	{
		acData = _acData;
		auraCharges = _auraCharges;
	}

	public void AddCharges(int charges)
	{
		try
		{
			checked
			{
				_ = auraCharges + charges;
			}
			auraCharges += charges;
		}
		catch (OverflowException ex)
		{
			Debug.LogWarning("AddCharges exception-> " + ex);
			auraCharges = int.MaxValue;
		}
	}

	public int GetCharges()
	{
		return auraCharges;
	}

	public void ConsumeAura()
	{
		auraCharges--;
	}

	public void ConsumeAll()
	{
		auraCharges = 0;
	}

	public Aura DeepClone()
	{
		Aura aura = new Aura();
		aura.ACData = acData.DeepClone();
		aura.SetAura(acData, auraCharges);
		return aura;
	}
}
