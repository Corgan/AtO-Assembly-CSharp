using System.Collections;
using TMPro;
using UnityEngine;

public class TextFade : MonoBehaviour
{
	private TMP_Text m_TextComponent;

	public float FadeSpeed = 1f;

	public int RolloverCharacterSpread = 10;

	public Color ColorTint;

	private void Awake()
	{
		m_TextComponent = GetComponent<TMP_Text>();
	}

	private void Start()
	{
		m_TextComponent.color = new Color(m_TextComponent.color.r, m_TextComponent.color.g, m_TextComponent.color.b, 0f);
		StartCoroutine(AnimateVertexColors());
	}

	private void OnEnable()
	{
		m_TextComponent.color = new Color(m_TextComponent.color.r, m_TextComponent.color.g, m_TextComponent.color.b, 0f);
		StartCoroutine(AnimateVertexColors());
	}

	private IEnumerator AnimateVertexColors()
	{
		m_TextComponent.ForceMeshUpdate();
		TMP_TextInfo textInfo = m_TextComponent.textInfo;
		_ = (Color32)m_TextComponent.color;
		int wordStart = 0;
		int characterCount = textInfo.characterCount;
		if (characterCount == 0)
		{
			m_TextComponent.color = new Color(m_TextComponent.color.r, m_TextComponent.color.g, m_TextComponent.color.b, 1f);
			yield break;
		}
		while (true)
		{
			int num = wordStart + 3;
			if (num > characterCount)
			{
				num = characterCount - 1;
			}
			float num2 = 1f;
			for (float num3 = 0f; num3 < num2; num3 += 1f)
			{
				byte a = (byte)(255f * ((num3 + 1f) / num2));
				for (int i = wordStart; i <= num && i < textInfo.characterInfo.Length; i++)
				{
					int materialReferenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
					Color32[] colors = textInfo.meshInfo[materialReferenceIndex].colors32;
					int vertexIndex = textInfo.characterInfo[i].vertexIndex;
					if (colors.Length >= vertexIndex + 4)
					{
						colors[vertexIndex].a = a;
						colors[vertexIndex + 1].a = a;
						colors[vertexIndex + 2].a = a;
						colors[vertexIndex + 3].a = a;
					}
				}
				m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
			}
			wordStart = num + 1;
			if (wordStart > characterCount)
			{
				break;
			}
			yield return null;
		}
		m_TextComponent.color = new Color(m_TextComponent.color.r, m_TextComponent.color.g, m_TextComponent.color.b, 1f);
	}
}
