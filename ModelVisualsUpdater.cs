using UnityEngine;

public class ModelVisualsUpdater : MonoBehaviour
{
	public Character thisCharacter;

	public void SetCharacter(Character charcter)
	{
		thisCharacter = charcter;
	}

	internal virtual void UpdateVisuals()
	{
	}
}
