using System;
using UnityEngine;

public class HeroSelectionTabsManager : MonoBehaviour
{
	[Serializable]
	public struct Tabs
	{
		public string catergory;

		public BotonGeneric button;

		public Color textColor;
	}

	public static HeroSelectionTabsManager Instance;

	public Tabs[] heroTabs;

	public float defaultButtonSize;

	public float selectedButtonSize;

	private void Awake()
	{
		Instance = this;
	}

	public void EnableTab(string catergory)
	{
		int num = 0;
		for (int i = 0; i < heroTabs.Length; i++)
		{
			heroTabs[i].button.Enable();
			heroTabs[i].button.transform.localScale = Vector3.one * defaultButtonSize;
			heroTabs[i].button.text.color = heroTabs[i].textColor;
			if (catergory == heroTabs[i].catergory)
			{
				num = i;
			}
		}
		heroTabs[num].button.Disable();
		heroTabs[num].button.transform.localScale = Vector3.one * selectedButtonSize;
		heroTabs[num].button.text.color = heroTabs[0].textColor;
	}

	public void SetTabText(string text, int tab)
	{
		string text2 = heroTabs[tab].button.text.text;
		heroTabs[tab].button.text.SetText(text2 + " (" + text + ")");
	}
}
