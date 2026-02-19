using System;
using System.Collections.Generic;

[Serializable]
public class PlayerPerk
{
	public Dictionary<string, string[]> PerkConfigTitle = new Dictionary<string, string[]>();

	public Dictionary<string, List<string>[]> PerkConfigPerks = new Dictionary<string, List<string>[]>();

	public Dictionary<string, int[]> PerkConfigPoints = new Dictionary<string, int[]>();
}
