using System;

namespace NPCs;

[Serializable]
public class NPCState
{
	public string Id;

	public bool IsIllusion;

	public bool IsIllusionExposed;

	public string IllusionCharacterId;

	public bool IsSummon;
}
