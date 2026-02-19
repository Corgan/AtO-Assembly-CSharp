using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PopupHPBar : MonoBehaviour
{
	private CharacterItem charItem;

	private void Awake()
	{
		charItem = base.transform.parent.transform.parent.GetComponent<HeroItem>();
		if (charItem == null)
		{
			charItem = base.transform.parent.transform.parent.GetComponent<NPCItem>();
		}
	}

	private void OnMouseEnter()
	{
		List<string> list = charItem.CalculateDamagePrePostForThisCharacter();
		if (list.Count > 3)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<size=+2><align=left>");
			for (int i = 3; i < list.Count; i++)
			{
				stringBuilder.Append(list[i]);
				stringBuilder.Append("<br>");
			}
			PopupManager.Instance.SetText(stringBuilder.ToString(), fast: true, "", alwaysCenter: true);
		}
	}

	private void OnMouseExit()
	{
		PopupManager.Instance.ClosePopup();
	}
}
