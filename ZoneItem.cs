using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ZoneItem : MonoBehaviour
{
	public TMP_Text ZoneName;

	private CombatToolManager toolManager;

	public Sprite BG;

	public string zoneNameForSearch;

	private void Awake()
	{
		toolManager = CombatToolManager.Instance;
	}

	public void SetZoneName(string zoneName, Sprite bg)
	{
		if (zoneName == "Necropolis")
		{
			zoneName = "Witch Woods";
		}
		else if (zoneName == "Voidlow")
		{
			zoneName = "The Void";
		}
		zoneName = Texts.Instance.GetText(zoneName.Replace(" ", ""));
		ZoneName.text = zoneName;
		BG = bg;
	}

	public void SetZoneNameForSearch(string zoneName)
	{
		if (zoneName == "Necropolis")
		{
			zoneName = "Dusk Lands";
		}
		else if (zoneName == "Voidlow")
		{
			zoneName = "Void";
		}
		zoneNameForSearch = zoneName;
	}

	public void SetSelectedState()
	{
		ZoneName.color = new Color32(206, 153, 104, byte.MaxValue);
		toolManager.PrevoiusSelectedZoneName = ZoneName;
		toolManager.ZoneName.text = zoneNameForSearch;
		Image component = GetComponent<Image>();
		if (component != null)
		{
			component.color = new Color32(55, 48, 60, byte.MaxValue);
		}
		toolManager.SetZoneBG(BG);
		toolManager.LoadAllNodes(zoneNameForSearch);
	}

	public void ShowZoneNodeData()
	{
		toolManager.ResetPrevoiusZoneState();
		Debug.LogError(zoneNameForSearch);
		toolManager.ZoneName.text = zoneNameForSearch;
		SetSelectedState();
		toolManager.LoadAllNodes(zoneNameForSearch);
	}
}
