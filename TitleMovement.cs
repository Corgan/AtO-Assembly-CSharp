using TMPro;
using UnityEngine;

public class TitleMovement : MonoBehaviour
{
	public TMP_Text titleText;

	public Color titleColor;

	public string idTranslate = "";

	public string directText = "";

	private void Awake()
	{
		titleText.color = titleColor;
	}

	private void Start()
	{
		if (idTranslate != "")
		{
			SetText(Texts.Instance.GetText(idTranslate));
		}
		else if (directText != "")
		{
			SetText(directText);
		}
	}

	public void SetText(string text)
	{
		titleText.text = text;
	}

	public void SetColor(Color color)
	{
		titleText.color = color;
	}
}
