using UnityEngine;

namespace UI;

public class UICharacterScale : MonoBehaviour
{
	[SerializeField]
	private float _uiCharacterScale;

	public float uiCharacterScale => _uiCharacterScale;
}
