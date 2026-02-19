using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatToolManager : MonoBehaviour
{
	public GameObject ZoneItem;

	public GameObject NodeItem;

	public Transform parentContainerZone;

	public Transform parentContainerNode;

	public TMP_Text ZoneCount;

	public TMP_Text ZoneName;

	public TMP_Text PrevoiusSelectedZoneName;

	public GameObject PanelParent;

	private CombatData[] combats;

	private NPCData[] npcList;

	public Button ConfirmButton;

	public CombatData CurrentCombat;

	public PopupNodeCombatTool LastCombatButtonSelected;

	public List<ZoneData> LoadedZones = new List<ZoneData>();

	public Transform paginationContainer;

	public GameObject pageButtonPrefab;

	private int itemsPerPage = 9;

	private int currentPage;

	private int totalPages;

	public Image ZoneBG;

	private string lastZoneSignature = string.Empty;

	public static CombatToolManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			HideCombatTool();
		}
	}

	private void OnMouseDown()
	{
		Debug.Log("Mouse Down on " + base.gameObject.name);
	}

	private void OnMouseUp()
	{
		Debug.Log("Mouse Up on " + base.gameObject.name);
	}

	public void SetHeroTraits()
	{
		AtOManager.Instance.SetHeroTraitsCombatTool();
	}

	public void ShowCombatTool()
	{
		PanelParent.gameObject.SetActive(value: true);
		LoadZones();
	}

	public void HideCombatTool()
	{
		if (PanelParent.activeSelf)
		{
			CurrentCombat = null;
			ResetPrevoiusZoneState();
			ConfirmButton.interactable = false;
			PanelParent.SetActive(value: false);
		}
	}

	public void Confirm()
	{
		ResetPrevoiusZoneState();
		ConfirmButton.interactable = false;
		PanelParent.SetActive(value: false);
	}

	public void LoadZones()
	{
		LoadedZones.Clear();
		ZoneData[] array = Resources.LoadAll<ZoneData>("World/Zones");
		foreach (ZoneData zoneData in array)
		{
			if (zoneData != null && zoneData.CombatBackground != null)
			{
				LoadedZones.Add(zoneData);
			}
		}
		string text = string.Join("|", LoadedZones.Select((ZoneData z) => z.name));
		if (text != lastZoneSignature)
		{
			foreach (ZoneData loadedZone in LoadedZones)
			{
				Instance.SpawnZone(loadedZone.name, loadedZone.ZoneName, loadedZone.CombatBackground);
				Debug.Log("Loaded zone: " + loadedZone.name);
			}
			lastZoneSignature = text;
		}
		else
		{
			Debug.Log("Zones unchanged, skipping SpawnZone.");
		}
		Instance.SetZoneCounter(LoadedZones.Count.ToString());
		SelectFirstZone();
	}

	public void SetZoneCounter(string Count)
	{
		ZoneCount.text = Count;
	}

	public void SetZoneBG(Sprite sprite)
	{
		ZoneBG.sprite = sprite;
	}

	public void SpawnZone(string ZoneNameForSearch, string ZoneName, Sprite BG)
	{
		if (ZoneItem == null || parentContainerZone == null)
		{
			Debug.LogWarning("ZoneItem or parentContainer is not assigned!");
			return;
		}
		GameObject obj = UnityEngine.Object.Instantiate(ZoneItem, parentContainerZone);
		obj.GetComponent<ZoneItem>().SetZoneNameForSearch(ZoneNameForSearch);
		obj.GetComponent<ZoneItem>().SetZoneName(ZoneName, BG);
		obj.transform.localScale = Vector3.one;
	}

	private void SelectFirstZone()
	{
		if (parentContainerZone.childCount > 0)
		{
			ZoneItem component = parentContainerZone.GetChild(0).gameObject.GetComponent<ZoneItem>();
			component.SetSelectedState();
			SetZoneBG(component.BG);
		}
	}

	public void ResetPrevoiusZoneState()
	{
		if (PrevoiusSelectedZoneName != null)
		{
			PrevoiusSelectedZoneName.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}
		Image componentInParent = PrevoiusSelectedZoneName.GetComponentInParent<Image>();
		if (componentInParent != null)
		{
			componentInParent.color = new Color32(84, 71, 94, byte.MaxValue);
		}
	}

	public void LoadAllNodes(string NodeName)
	{
		combats = Resources.LoadAll<CombatData>("World/CombatToolCombats/" + NodeName);
		combats = combats.Where((CombatData c) => c.NpcRemoveInMadness0Index == -1).ToArray();
		totalPages = Mathf.CeilToInt((float)combats.Length / (float)itemsPerPage);
		currentPage = 0;
		ClearNodes();
		for (int num = 0; num < combats.Length; num++)
		{
			SpawnNodes(combats[num].name, combats[num], num);
		}
		CreatePageButtons();
		ShowPage(currentPage);
	}

	public void SpawnNodes(string NodeName, CombatData Combat, int index)
	{
		if (NodeItem == null || parentContainerNode == null)
		{
			Debug.LogWarning("NodeItem or parentContainerNode is not assigned!");
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(NodeItem, parentContainerNode);
		PopupNodeCombatTool component = gameObject.GetComponent<PopupNodeCombatTool>();
		component.SetTitle(Texts.Instance.GetText("combat") + " " + (index + 1));
		gameObject.transform.localScale = Vector3.one;
		npcList = Combat.NPCList;
		npcList = Array.FindAll(npcList, (NPCData npc) => npc != null);
		component.SetCombatData(Combat);
		for (int num = 0; num < npcList.Length; num++)
		{
			component.SetIcon(npcList[num].SpriteSpeed, num);
		}
		if (CurrentCombat != null && CurrentCombat == Combat)
		{
			component.SelectedIcon.SetActive(value: true);
			LastCombatButtonSelected = component;
			ConfirmButton.interactable = true;
		}
		string currentZoneName = ((ZoneName != null) ? CorrectedZoneNameForSearch(ZoneName.text) : "");
		ZoneData zoneData = LoadedZones.FirstOrDefault((ZoneData z) => z != null && z.name == currentZoneName);
		if (!(zoneData != null) || string.IsNullOrEmpty(zoneData.Sku))
		{
			return;
		}
		if (!SteamManager.Instance.PlayerHaveDLC(zoneData.Sku) && !(zoneData.Sku == "3905570"))
		{
			Button component2 = gameObject.GetComponent<Button>();
			if (component2 != null)
			{
				component2.interactable = false;
			}
			component.CombatIcon.gameObject.SetActive(value: false);
			component.LockIcon.gameObject.SetActive(value: true);
		}
		else
		{
			component.CombatIcon.gameObject.SetActive(value: true);
			component.LockIcon.gameObject.SetActive(value: false);
		}
	}

	private string CorrectedZoneNameForSearch(string zoneName)
	{
		if (zoneName == "Dusk Lands")
		{
			zoneName = "Necropolis";
		}
		else if (zoneName == "Void")
		{
			zoneName = "Voidlow";
		}
		return zoneName;
	}

	private void ClearNodes()
	{
		foreach (Transform item in parentContainerNode)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
	}

	public void ConfirmPressed()
	{
		HideCombatTool();
	}

	private void CreatePageButtons()
	{
		foreach (Transform item in paginationContainer)
		{
			UnityEngine.Object.Destroy(item.gameObject);
		}
		List<Button> list = new List<Button>();
		for (int num = totalPages - 1; num >= 0; num--)
		{
			int pageIndex = num;
			GameObject obj = UnityEngine.Object.Instantiate(pageButtonPrefab, paginationContainer);
			obj.GetComponentInChildren<TMP_Text>().text = (num + 1).ToString();
			Button component = obj.GetComponent<Button>();
			component.onClick.AddListener(delegate
			{
				ShowPage(pageIndex);
			});
			list.Add(component);
		}
		if (list.Count > 0)
		{
			ShowPage(0);
			list[list.Count - 1].Select();
		}
		else
		{
			Debug.Log("No pages to create, combats list empty.");
		}
	}

	private void ShowPage(int pageIndex)
	{
		ClearNodes();
		currentPage = pageIndex;
		int num = pageIndex * itemsPerPage;
		int num2 = Mathf.Min(num + itemsPerPage, combats.Length);
		for (int i = num; i < num2; i++)
		{
			SpawnNodes(combats[i].name, combats[i], i);
		}
	}

	public void MovePage(int direction)
	{
		int num = currentPage + direction;
		if (num < 0 || num >= totalPages)
		{
			Debug.Log("No more pages in this direction");
			return;
		}
		ShowPage(num);
		if (paginationContainer.childCount > 0)
		{
			int num2 = totalPages - 1 - num;
			if (num2 >= 0 && num2 < paginationContainer.childCount)
			{
				paginationContainer.GetChild(num2).GetComponent<Button>().Select();
			}
		}
	}

	public void LaunchCombat()
	{
		if (CurrentCombat != null)
		{
			AtOManager.Instance.LaunchCombat(CurrentCombat);
		}
		else
		{
			LaunchRandomCombat();
		}
		SetHeroTraits();
	}

	private void LaunchRandomCombat()
	{
		ZoneData[] array = Resources.LoadAll<ZoneData>("World/Zones");
		List<ZoneData> list = new List<ZoneData>();
		ZoneData[] array2 = array;
		foreach (ZoneData zoneData in array2)
		{
			if (zoneData != null && zoneData.CombatBackground != null)
			{
				if (string.IsNullOrEmpty(zoneData.Sku))
				{
					list.Add(zoneData);
				}
				else if (SteamManager.Instance.PlayerHaveDLC(zoneData.Sku))
				{
					list.Add(zoneData);
				}
			}
		}
		if (list.Count == 0)
		{
			Debug.LogWarning("No valid zones found!");
			return;
		}
		bool flag = false;
		int num = 0;
		while (!flag && num < 50)
		{
			num++;
			ZoneData zoneData2 = list[UnityEngine.Random.Range(0, list.Count)];
			Debug.Log("Random Zone selected: " + zoneData2.name);
			CombatData[] array3 = Resources.LoadAll<CombatData>("World/Combats/" + zoneData2.name);
			if (array3.Length != 0)
			{
				int num2 = UnityEngine.Random.Range(0, array3.Length);
				Debug.Log("Random Combat selected: " + array3[num2].name);
				AtOManager.Instance.LaunchCombat(array3[num2]);
				flag = true;
			}
			else
			{
				Debug.LogWarning("No combats found for zone: " + zoneData2?.ToString() + ", retrying...");
			}
		}
		if (!flag)
		{
			Debug.LogError("Failed to find any combat after multiple attempts!");
		}
	}

	private void LaunchRandomCombat1()
	{
		List<ZoneData> list = new List<ZoneData>();
		ZoneData[] array = Resources.LoadAll<ZoneData>("World/Zones");
		foreach (ZoneData zoneData in array)
		{
			if (zoneData != null && zoneData.CombatBackground != null)
			{
				list.Add(zoneData);
				Debug.Log("Loaded zone: " + zoneData.name);
			}
		}
		if (list.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			ZoneData zoneData2 = list[index];
			Debug.Log("Random Zone selected: " + zoneData2.name);
			Resources.LoadAll<CombatData>("World/Combats/" + zoneData2);
		}
		AtOManager.Instance.LaunchCombat(combats[6]);
	}

	public void LoadCombatToolConfig()
	{
		if (!AtOManager.Instance.IsCombatTool)
		{
			return;
		}
		SaveManager.playerDataCombatTool = PlayerDataCombatTool.Load();
		if (SaveManager.playerDataCombatTool.profileId == GameManager.Instance.ProfileId)
		{
			if (SaveManager.playerDataCombatTool.LastUsedTeam != null && SaveManager.playerDataCombatTool.LastUsedTeam.Count() > 0)
			{
				PlayerManager.Instance.LastUsedTeam = SaveManager.playerDataCombatTool.LastUsedTeam;
			}
			if (SaveManager.playerDataCombatTool.HeroPerks != null && SaveManager.playerDataCombatTool.HeroPerks.Count() > 0)
			{
				PlayerManager.Instance.HeroPerks = SaveManager.playerDataCombatTool.HeroPerks;
			}
		}
	}

	public void SaveCombatToolConfig()
	{
		if (AtOManager.Instance.IsCombatTool)
		{
			SaveManager.playerDataCombatTool.LastUsedTeam = PlayerManager.Instance.LastUsedTeam;
			SaveManager.playerDataCombatTool.HeroPerks = PlayerManager.Instance.HeroPerks;
			SaveManager.playerDataCombatTool.profileId = GameManager.Instance.ProfileId;
			SaveManager.playerDataCombatTool.Save();
		}
	}
}
