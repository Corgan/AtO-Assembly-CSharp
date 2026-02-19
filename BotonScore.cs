using TMPro;
using UnityEngine;

public class BotonScore : MonoBehaviour
{
	public SpriteRenderer background;

	public TMP_Text text;

	public Color textColorOver;

	public Color textColorOff;

	public Color bgColorOver;

	public Color bgColorOff;

	public string idTranslate = "";

	public int auxInt = -1000;

	private void Awake()
	{
		if (idTranslate != "" && Texts.Instance != null)
		{
			SetText(Texts.Instance.GetText(idTranslate));
		}
		if (text.transform.childCount > 0)
		{
			for (int i = 0; i < text.transform.childCount; i++)
			{
				MeshRenderer component = text.transform.GetChild(i).GetComponent<MeshRenderer>();
				component.sortingLayerName = text.GetComponent<MeshRenderer>().sortingLayerName;
				component.sortingOrder = text.GetComponent<MeshRenderer>().sortingOrder;
			}
		}
	}

	private void Start()
	{
		SetPlainColors();
	}

	private void SetPlainColors()
	{
		background.color = bgColorOff;
		text.color = textColorOff;
	}

	public void SetText(string _text)
	{
		text.text = _text;
	}

	private void OnMouseEnter()
	{
		if (!AlertManager.Instance.IsActive() && !SettingsManager.Instance.IsActive())
		{
			background.color = bgColorOver;
			text.color = textColorOver;
			GameManager.Instance.SetCursorHover();
		}
	}

	private void OnMouseExit()
	{
		if (!AlertManager.Instance.IsActive() && !SettingsManager.Instance.IsActive())
		{
			SetPlainColors();
			GameManager.Instance.SetCursorPlain();
		}
	}

	public void OnMouseUp()
	{
		if (!AlertManager.Instance.IsActive() && !SettingsManager.Instance.IsActive())
		{
			TomeManager.Instance.ShowScoreboard(auxInt);
		}
	}
}
