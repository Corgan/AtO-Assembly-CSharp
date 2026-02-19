using System;

[Serializable]
public struct SpecialValues
{
	public Enums.SpecialValueModifierName Name;

	public bool Use;

	public float Multiplier;

	public SpecialValues(Enums.SpecialValueModifierName name, bool use, float multiplier)
	{
		Name = name;
		Use = use;
		Multiplier = multiplier;
	}

	public SpecialValues(SpecialValues specialValues)
	{
		Name = specialValues.Name;
		Use = specialValues.Use;
		Multiplier = specialValues.Multiplier;
	}
}
