using System;
using System.Collections.Generic;

[Serializable]
public class PlayerDeck
{
	public Dictionary<string, string[]> DeckTitle = new Dictionary<string, string[]>();

	public Dictionary<string, List<string>[]> DeckCards = new Dictionary<string, List<string>[]>();
}
