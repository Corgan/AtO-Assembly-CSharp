using System.Collections;
using TMPro;
using UnityEngine;

public class Translate : MonoBehaviour
{
	public string textId = "";

	private TMP_Text textObj;

	private void Awake()
	{
		textObj = GetComponent<TMP_Text>();
	}

	private void Start()
	{
		SetText();
	}

	public void SetText()
	{
		if (!(textObj == null) && !(Texts.Instance == null) && textId != "")
		{
			StartCoroutine(SetTextCo());
		}
	}

	private IEnumerator SetTextCo()
	{
		int breakInt = 0;
		while (!Texts.Instance.GotTranslations())
		{
			yield return Globals.Instance.WaitForSeconds(0.001f);
			breakInt++;
			if (breakInt > 1000)
			{
				Debug.Log(textId + " BROKE " + Globals.Instance.CurrentLang);
				yield break;
			}
		}
		textObj.text = Texts.Instance.GetText(textId);
	}
}
