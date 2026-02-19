using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PerkTree : MonoBehaviour
{
	public Transform elements;

	public Transform nodes;

	public GameObject perkNodeGO;

	public GameObject perkNodeAmplifyGO;

	public Transform posBegin;

	public Transform posEnd;

	public SpriteRenderer charSprite;

	public TMP_Text availablePerksPoints;

	public TMP_Text usedPerksPoints;

	public PerkBackgroundRow[] perkBgRow;

	public Transform[] categoryT;

	public BotonGeneric[] buttonType;

	public BotonGeneric buttonConfirm;

	public BotonGeneric buttonReset;

	public BotonGeneric buttonImport;

	public BotonGeneric buttonExport;

	public Transform buttonExit;

	public Transform saveSlots;

	public Sprite iconPerkMultiple;

	private float oriX;

	private float endX;

	private float oriY;

	private float endY;

	private float cols = 12f;

	private float rows = 7f;

	private float offsetX;

	private float offsetY;

	private Dictionary<string, PerkNodeData> perkNodeDatas;

	private Dictionary<string, PerkNode> perkNodes;

	private Dictionary<string, GameObject> perkNodesGO;

	private Dictionary<string, List<string>> perkChildIncompatible;

	private Dictionary<string, List<string>> teamPerks;

	private List<string> selectedPerks;

	private int totalAvailablePoints;

	private int availablePoints;

	private int usedPoints;

	private string subClassId = "";

	private int[] usedPointsArray;

	private int[] usedPointsCategory;

	private bool canModify;

	private int savingSlot;

	private int loadingSlot;

	public PerkSlot[] perkSlot;

	public bool IsOwner;

	public TMP_Text rankProgress;

	public TMP_Text maxProgress;

	public Transform perkBarMask;

	public SpriteRenderer perkBar;

	public int controllerHorizontalIndex = -1;

	private Vector2 warpPosition = Vector2.zero;

	private List<Transform> _controllerList = new List<Transform>();

	private List<Transform> _controllerVerticalList = new List<Transform>();

	public static PerkTree Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		usedPointsArray = new int[7];
		usedPointsArray[0] = 0;
		usedPointsArray[1] = 3;
		usedPointsArray[2] = 6;
		usedPointsArray[3] = 10;
		usedPointsArray[4] = 16;
		usedPointsArray[5] = 22;
		usedPointsArray[6] = 30;
		usedPointsCategory = new int[4];
	}

	public void InitPerkTree()
	{
		oriX = posBegin.localPosition.x;
		oriY = posBegin.localPosition.y;
		endX = posEnd.localPosition.x;
		endY = posEnd.localPosition.y;
		if (posBegin.gameObject.activeSelf)
		{
			posBegin.gameObject.SetActive(value: false);
		}
		if (posEnd.gameObject.activeSelf)
		{
			posEnd.gameObject.SetActive(value: false);
		}
		offsetX = Mathf.Abs(oriX - endX) / (cols - 1f);
		offsetY = Mathf.Abs(oriY - endY) / (rows - 1f);
		for (int i = 0; i < perkBgRow.Length; i++)
		{
			perkBgRow[i].SetRequired(usedPointsArray[i]);
		}
		buttonConfirm.Disable();
		if (buttonConfirm.gameObject.activeSelf)
		{
			buttonConfirm.gameObject.SetActive(value: false);
		}
		DrawTree();
		StartCoroutine(HideCo());
	}

	private IEnumerator HideCo()
	{
		yield return Globals.Instance.WaitForSeconds(0.01f);
		Hide();
	}

	public int GetPointsNeeded(int _row)
	{
		return usedPointsArray[_row];
	}

	public int GetPointsAvailable()
	{
		return availablePoints;
	}

	public string GetActiveHero()
	{
		return subClassId;
	}

	public void ExportTree()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(usedPoints);
		stringBuilder.Append("_");
		for (int i = 0; i < selectedPerks.Count; i++)
		{
			stringBuilder.Append(selectedPerks[i]);
			if (i < selectedPerks.Count - 1)
			{
				stringBuilder.Append("-");
			}
		}
		string inputText = Functions.CompressString(stringBuilder.ToString());
		AlertManager.Instance.AlertCopyPaste(Texts.Instance.GetText("pressForCopyPaste"), inputText);
	}

	public void ImportTree()
	{
		AlertManager.buttonClickDelegate = ImportTreeAction;
		AlertManager.Instance.AlertPasteCopy(Texts.Instance.GetText("pressForImportTree"));
	}

	public void ImportTreeAction()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(ImportTreeAction));
		if (!AlertManager.Instance.GetConfirmAnswer())
		{
			return;
		}
		string compressedText = Functions.OnlyAscii(AlertManager.Instance.GetInputPCValue()).Trim();
		string text = "";
		bool flag = false;
		try
		{
			text = Functions.DecompressString(compressedText);
		}
		catch
		{
			flag = true;
		}
		if (flag)
		{
			ErrorImport();
		}
		else
		{
			if (!(text != ""))
			{
				return;
			}
			string[] array = text.Split('_');
			if (array.Length == 2)
			{
				int num = int.Parse(array[0]);
				if (totalAvailablePoints < num)
				{
					ErrorImport(1, num);
					return;
				}
				string[] array2 = array[1].Split('-');
				selectedPerks = new List<string>();
				for (int i = 0; i < array2.Length; i++)
				{
					selectedPerks.Add(array2[i]);
				}
				Refresh();
				ErrorImport(-1);
				buttonConfirm.Enable();
			}
			else
			{
				ErrorImport();
			}
		}
	}

	private void ErrorImport(int _error = 0, int _points = 0)
	{
		StartCoroutine(ErrorImportCo(_error, _points));
	}

	private IEnumerator ErrorImportCo(int _error, int _points)
	{
		yield return Globals.Instance.WaitForSeconds(0.1f);
		switch (_error)
		{
		case -1:
			AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("importedTree"));
			break;
		case 0:
			AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("invalidImportTalentTreeCode"));
			break;
		case 1:
		{
			string text = string.Format(Texts.Instance.GetText("importPerkTreeNotPoints"), _points);
			AlertManager.Instance.AlertConfirm(text);
			break;
		}
		}
	}

	private void DoPerkRank()
	{
		int playerRankProgress = PlayerManager.Instance.GetPlayerRankProgress();
		int perkPrevLevelPoints = PlayerManager.Instance.GetPerkPrevLevelPoints("");
		int perkNextLevelPoints = PlayerManager.Instance.GetPerkNextLevelPoints("");
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<color=#FFF>");
		stringBuilder.Append(playerRankProgress.ToString());
		if (perkNextLevelPoints != 0)
		{
			stringBuilder.Append("</color><size=-.5> / ");
			stringBuilder.Append(perkNextLevelPoints.ToString());
		}
		maxProgress.text = stringBuilder.ToString();
		int highestCharacterRank = PlayerManager.Instance.GetHighestCharacterRank();
		rankProgress.text = string.Format(Texts.Instance.GetText("rankProgress"), highestCharacterRank);
		float x = ((float)playerRankProgress - (float)perkPrevLevelPoints) / ((float)perkNextLevelPoints - (float)perkPrevLevelPoints) * 3.365f;
		perkBarMask.localScale = new Vector3(x, perkBarMask.localScale.y, perkBarMask.localScale.z);
	}

	public void Show(string _subClassId = "", int currentHeroIndex = -1)
	{
		DoPerkRank();
		if (((bool)HeroSelectionManager.Instance && !GameManager.Instance.IsLoadingGame()) || (AtOManager.Instance.CharInTown() && AtOManager.Instance.GetTownTier() == 0))
		{
			canModify = true;
		}
		else
		{
			canModify = false;
		}
		if (!GameManager.Instance.IsMultiplayer() || ((bool)HeroSelectionManager.Instance && !GameManager.Instance.IsLoadingGame()) || (currentHeroIndex > -1 && AtOManager.Instance.GetHero(currentHeroIndex).Owner == NetworkManager.Instance.GetPlayerNick()))
		{
			IsOwner = true;
			if (!buttonReset.gameObject.activeSelf)
			{
				buttonReset.gameObject.SetActive(value: true);
			}
		}
		else
		{
			IsOwner = false;
			if (buttonReset.gameObject.activeSelf)
			{
				buttonReset.gameObject.SetActive(value: false);
			}
		}
		selectedPerks = new List<string>();
		subClassId = _subClassId;
		SubClassData subClassData = Globals.Instance.GetSubClassData(subClassId);
		charSprite.sprite = subClassData.SpritePortrait;
		if (AtOManager.Instance.IsCombatTool)
		{
			totalAvailablePoints = 50;
		}
		else
		{
			totalAvailablePoints = PlayerManager.Instance.GetHighestCharacterRank();
		}
		if (totalAvailablePoints > Globals.MaxPerkPoints)
		{
			totalAvailablePoints = Globals.MaxPerkPoints;
		}
		buttonConfirm.Disable();
		if ((bool)HeroSelectionManager.Instance && !GameManager.Instance.IsLoadingGame())
		{
			if (!buttonReset.gameObject.activeSelf)
			{
				buttonReset.gameObject.SetActive(value: true);
			}
			if (!buttonImport.gameObject.activeSelf)
			{
				buttonImport.gameObject.SetActive(value: true);
			}
			if (!buttonExport.gameObject.activeSelf)
			{
				buttonExport.gameObject.SetActive(value: true);
			}
			if (!saveSlots.gameObject.activeSelf)
			{
				saveSlots.gameObject.SetActive(value: true);
			}
		}
		else
		{
			if (buttonReset.gameObject.activeSelf)
			{
				buttonReset.gameObject.SetActive(value: false);
			}
			if (buttonImport.gameObject.activeSelf)
			{
				buttonImport.gameObject.SetActive(value: false);
			}
			if (buttonExport.gameObject.activeSelf)
			{
				buttonExport.gameObject.SetActive(value: false);
			}
			if (saveSlots.gameObject.activeSelf)
			{
				saveSlots.gameObject.SetActive(value: false);
			}
		}
		if (buttonConfirm.gameObject.activeSelf != canModify)
		{
			buttonConfirm.gameObject.SetActive(canModify);
		}
		GetHeroPerks();
		SetCategory();
		LoadSavedPerks();
		DoTeamPerks();
		if (!elements.gameObject.activeSelf)
		{
			elements.gameObject.SetActive(value: true);
		}
		controllerHorizontalIndex = -1;
	}

	public void Hide()
	{
		if (buttonConfirm.gameObject.activeSelf && buttonConfirm.buttonEnabled && SceneStatic.GetSceneName() != "MainMenu")
		{
			AlertManager.buttonClickDelegate = HideConfirm;
			AlertManager.Instance.AlertConfirmDouble(Texts.Instance.GetText("wantExitPerks"));
		}
		else
		{
			HideAction();
		}
	}

	public void HideConfirm()
	{
		bool confirmAnswer = AlertManager.Instance.GetConfirmAnswer();
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(HideConfirm));
		if (confirmAnswer)
		{
			HideAction();
		}
	}

	public void HideAction(bool checkSubclass = true)
	{
		if (checkSubclass && subClassId != "")
		{
			if (HeroSelectionManager.Instance != null)
			{
				HeroSelectionManager.Instance.RefreshPerkPoints(subClassId);
			}
			else
			{
				Hero[] team = AtOManager.Instance.GetTeam();
				if (team != null)
				{
					for (int i = 0; i < team.Length; i++)
					{
						if (team[i].HeroData.HeroSubClass.Id == subClassId)
						{
							AtOManager.Instance.SideBarCharacterClicked(i);
							break;
						}
					}
				}
			}
		}
		buttonConfirm.Disable();
		if (elements.gameObject.activeSelf)
		{
			elements.gameObject.SetActive(value: false);
		}
		PopupManager.Instance.ClosePopup();
	}

	public bool IsActive()
	{
		return elements.gameObject.activeSelf;
	}

	public void SetCategory(int _type = 0)
	{
		for (int i = 0; i < buttonType.Length; i++)
		{
			if (_type == i)
			{
				buttonType[i].transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
				buttonType[i].transform.localPosition = new Vector3(buttonType[i].transform.localPosition.x, 0.05f, buttonType[i].transform.localPosition.z);
				buttonType[i].Disable();
			}
			else
			{
				buttonType[i].transform.localScale = new Vector3(1f, 1f, 1f);
				buttonType[i].transform.localPosition = new Vector3(buttonType[i].transform.localPosition.x, 0f, buttonType[i].transform.localPosition.z);
				buttonType[i].Enable();
			}
		}
		for (int j = 0; j < categoryT.Length; j++)
		{
			if (_type != j)
			{
				if (categoryT[j].gameObject.activeSelf)
				{
					categoryT[j].gameObject.SetActive(value: false);
				}
			}
			else if (!categoryT[j].gameObject.activeSelf)
			{
				categoryT[j].gameObject.SetActive(value: true);
			}
		}
		Refresh();
	}

	private void DrawTree()
	{
		perkNodeDatas = new Dictionary<string, PerkNodeData>();
		perkNodes = new Dictionary<string, PerkNode>();
		perkNodesGO = new Dictionary<string, GameObject>();
		perkChildIncompatible = new Dictionary<string, List<string>>();
		foreach (KeyValuePair<string, PerkNodeData> item in Globals.Instance.PerksNodesSource)
		{
			perkNodeDatas.Add(item.Value.Id, item.Value);
		}
		foreach (KeyValuePair<string, PerkNodeData> perkNodeData in perkNodeDatas)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(perkNodeGO, new Vector3(0f, 0f, 0f), Quaternion.identity, categoryT[perkNodeData.Value.Type]);
			gameObject.name = perkNodeData.Key;
			gameObject.transform.localPosition = new Vector3(oriX + offsetX * (float)perkNodeData.Value.Column, oriY - offsetY * (float)perkNodeData.Value.Row, 0f);
			perkNodesGO.Add(perkNodeData.Key, gameObject);
		}
		foreach (KeyValuePair<string, PerkNodeData> perkNodeData2 in perkNodeDatas)
		{
			List<string> list = new List<string>();
			if (perkNodeData2.Value.PerksConnected.Length != 0)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(perkNodeAmplifyGO, new Vector3(0f, -1f, 0f), Quaternion.identity, categoryT[perkNodeData2.Value.Type]);
				gameObject2.name = perkNodeData2.Key + "_amplify";
				gameObject2.transform.localPosition = new Vector3(oriX + offsetX * (float)perkNodeData2.Value.Column, oriY - offsetY * (float)perkNodeData2.Value.Row - 1.1f, -1f);
				for (int i = 0; i < perkNodeData2.Value.PerksConnected.Length; i++)
				{
					perkNodesGO[perkNodeData2.Value.PerksConnected[i].Id].transform.parent = gameObject2.transform.GetComponent<PerkNodeAmplify>().amplifyNodes;
					perkNodesGO[perkNodeData2.Value.PerksConnected[i].Id].transform.localPosition = new Vector3((float)i * 1.4f, 0f, -1f);
					perkNodesGO[perkNodeData2.Value.PerksConnected[i].Id].transform.GetComponent<PerkNode>().SetNodeAsChild();
					list.Add(perkNodeData2.Value.PerksConnected[i].Id);
				}
				gameObject2.transform.GetComponent<PerkNodeAmplify>().SetForNodes(perkNodeData2.Value.PerksConnected.Length);
			}
			for (int j = 0; j < list.Count; j++)
			{
				perkChildIncompatible.Add(list[j], new List<string>());
				for (int k = 0; k < list.Count; k++)
				{
					if (j != k)
					{
						perkChildIncompatible[list[j]].Add(list[k]);
					}
				}
			}
		}
		foreach (KeyValuePair<string, PerkNodeData> perkNodeData3 in perkNodeDatas)
		{
			PerkNode component = perkNodesGO[perkNodeData3.Value.Id].GetComponent<PerkNode>();
			component.SetPND(perkNodeData3.Value);
			if (perkNodeData3.Value.PerkRequired != null && perkNodesGO.ContainsKey(perkNodeData3.Value.PerkRequired.Id))
			{
				GameObject gameObject3 = perkNodesGO[perkNodeData3.Value.Id];
				GameObject gameObject4 = perkNodesGO[perkNodeData3.Value.PerkRequired.Id];
				component.SetArrow(new Vector3(0f, gameObject4.transform.localPosition.y - gameObject3.transform.localPosition.y, 0f), Vector3.zero);
			}
			perkNodes.Add(perkNodeData3.Value.Id, component);
		}
		perkNodeGO.SetActive(value: false);
	}

	private void GetHeroPerks()
	{
		List<string> heroPerks = PlayerManager.Instance.GetHeroPerks(subClassId);
		if (heroPerks != null)
		{
			for (int i = 0; i < heroPerks.Count; i++)
			{
				selectedPerks.Add(heroPerks[i]);
			}
		}
	}

	public void DoTeamPerks()
	{
		teamPerks = new Dictionary<string, List<string>>();
		if ((bool)HeroSelectionManager.Instance && !GameManager.Instance.IsLoadingGame())
		{
			if (!GameManager.Instance.IsMultiplayer())
			{
				for (int i = 0; i < 4; i++)
				{
					if (!(HeroSelectionManager.Instance.GetBoxHeroFromIndex(i) != null))
					{
						continue;
					}
					string subclassName = HeroSelectionManager.Instance.GetBoxHeroFromIndex(i).GetSubclassName();
					subclassName = subclassName.Replace(" ", "");
					if (!(subclassName != "") || !(subclassName != subClassId))
					{
						continue;
					}
					List<string> heroPerks = PlayerManager.Instance.GetHeroPerks(subclassName, forceFromPlayerManager: true);
					if (heroPerks == null)
					{
						continue;
					}
					for (int j = 0; j < heroPerks.Count; j++)
					{
						if (!teamPerks.ContainsKey(heroPerks[j]))
						{
							teamPerks[heroPerks[j]] = new List<string>();
						}
						teamPerks[heroPerks[j]].Add(subclassName);
					}
				}
			}
			else
			{
				for (int k = 0; k < 4; k++)
				{
					if (!(HeroSelectionManager.Instance.GetBoxHeroFromIndex(k) != null))
					{
						continue;
					}
					string subclassName2 = HeroSelectionManager.Instance.GetBoxHeroFromIndex(k).GetSubclassName();
					string key = (HeroSelectionManager.Instance.GetBoxOwnerFromIndex(k) + "_" + subclassName2).ToLower();
					if (!(subclassName2 != "") || !(subclassName2 != subClassId) || HeroSelectionManager.Instance.PlayerHeroPerksDict == null || !HeroSelectionManager.Instance.PlayerHeroPerksDict.ContainsKey(key))
					{
						continue;
					}
					List<string> list = HeroSelectionManager.Instance.PlayerHeroPerksDict[key];
					if (list == null)
					{
						continue;
					}
					for (int l = 0; l < list.Count; l++)
					{
						if (!teamPerks.ContainsKey(list[l]))
						{
							teamPerks[list[l]] = new List<string>();
						}
						teamPerks[list[l]].Add(subclassName2);
					}
				}
			}
		}
		else
		{
			Hero[] team = AtOManager.Instance.GetTeam();
			if (team != null)
			{
				for (int m = 0; m < team.Length; m++)
				{
					if (team[m] == null || team[m].HeroData == null)
					{
						continue;
					}
					if (team[m].HeroData.HeroSubClass.Id != subClassId && team[m].PerkList != null)
					{
						for (int n = 0; n < team[m].PerkList.Count; n++)
						{
							if (!teamPerks.ContainsKey(team[m].PerkList[n]))
							{
								teamPerks[team[m].PerkList[n]] = new List<string>();
							}
							teamPerks[team[m].PerkList[n]].Add(team[m].HeroData.HeroSubClass.Id);
						}
					}
					team[m].ClearCaches();
				}
			}
		}
		foreach (KeyValuePair<string, PerkNode> perkNode in perkNodes)
		{
			if (!(perkNode.Value.PND != null))
			{
				continue;
			}
			string text = "";
			if (perkNode.Value.PND.Perk != null)
			{
				text = perkNode.Value.PND.Perk.Id;
			}
			if (text != "" && teamPerks.ContainsKey(text))
			{
				perkNode.Value.TeamSelected(teamPerks[text].Count, teamPerks[text]);
			}
			else if (perkNode.Value.PND.PerksConnected.Length != 0)
			{
				int num = 0;
				for (int num2 = 0; num2 < perkNode.Value.PND.PerksConnected.Length; num2++)
				{
					if (teamPerks.ContainsKey(perkNode.Value.PND.PerksConnected[num2].Perk.Id))
					{
						num += teamPerks[perkNode.Value.PND.PerksConnected[num2].Perk.Id].Count;
					}
				}
				perkNode.Value.TeamSelected(num);
			}
			else
			{
				perkNode.Value.TeamSelected(0);
			}
		}
	}

	private void Refresh()
	{
		SetUsedPerks();
		SetNodes();
		SetRows();
		SetButtons();
	}

	private void SetRows()
	{
		for (int i = 0; i < perkBgRow.Length; i++)
		{
			if (usedPoints >= usedPointsArray[i])
			{
				perkBgRow[i].ShowLockIcon(_state: false);
			}
			else
			{
				perkBgRow[i].ShowLockIcon(_state: true);
			}
		}
	}

	private void SetUsedPerks()
	{
		usedPoints = 0;
		for (int i = 0; i < 4; i++)
		{
			usedPointsCategory[i] = 0;
		}
		foreach (KeyValuePair<string, PerkNodeData> perkNodeData in perkNodeDatas)
		{
			PerkNode component = perkNodesGO[perkNodeData.Value.Id].GetComponent<PerkNode>();
			if (IsThisPerkNodeDataSelected(perkNodeData.Value))
			{
				int pointsForNode = GetPointsForNode(perkNodeData.Value);
				usedPoints += pointsForNode;
				usedPointsCategory[perkNodeData.Value.Type] += pointsForNode;
				component.SetSelected(_status: true);
				continue;
			}
			component.SetSelected(_status: false);
			bool flag = false;
			for (int j = 0; j < perkNodeData.Value.PerksConnected.Length; j++)
			{
				if (!IsThisPerkNodeDataSelected(perkNodeData.Value.PerksConnected[j]))
				{
					continue;
				}
				foreach (KeyValuePair<string, PerkNode> perkNode in perkNodes)
				{
					if (perkNode.Value.PND.Id == perkNodeData.Value.PerksConnected[j].Id)
					{
						component.SetValuesAsChildNode(perkNode.Value);
						flag = true;
					}
				}
				break;
			}
			if (!flag)
			{
				component.SetDefaultIcon();
			}
		}
		availablePoints = totalAvailablePoints - usedPoints;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<voffset=.2><size=-.4><sprite name=perk></size></voffset>");
		stringBuilder.Append(Texts.Instance.GetText("perks"));
		if (IsOwner && canModify)
		{
			stringBuilder.Append("<br><space=-.7><b><size=+2><color=#FFF>");
			stringBuilder.Append(availablePoints);
			stringBuilder.Append("</color></size>/");
			stringBuilder.Append(totalAvailablePoints);
		}
		availablePerksPoints.text = stringBuilder.ToString();
		stringBuilder.Clear();
		stringBuilder.Append("<size=+.2>[<color=#FFF>");
		stringBuilder.Append(usedPoints);
		stringBuilder.Append("</color>]");
		usedPerksPoints.text = string.Format(Texts.Instance.GetText("usedPoints"), stringBuilder.ToString());
	}

	public int GetPointsForNode(PerkNodeData _pnd)
	{
		string value = Enum.GetName(typeof(Enums.PerkCost), _pnd.Cost);
		return (int)Enum.Parse(typeof(Enums.PerkCost), value);
	}

	private bool IsThisPerkNodeDataSelected(PerkNodeData _pnd)
	{
		if (_pnd.Perk != null && selectedPerks.Contains(_pnd.Perk.Id.ToLower()))
		{
			return true;
		}
		return false;
	}

	private void SetButtons()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Texts.Instance.GetText("general"));
		stringBuilder.Append(" (<color=#FFF><size=+.2>");
		stringBuilder.Append(usedPointsCategory[0].ToString());
		stringBuilder.Append("</size></color>)");
		buttonType[0].SetText(stringBuilder.ToString());
		stringBuilder.Clear();
		stringBuilder.Append(Texts.Instance.GetText("physical"));
		stringBuilder.Append(" (<color=#FFF><size=+.2>");
		stringBuilder.Append(usedPointsCategory[1].ToString());
		stringBuilder.Append("</size></color>)");
		buttonType[1].SetText(stringBuilder.ToString());
		stringBuilder.Clear();
		stringBuilder.Append(Texts.Instance.GetText("elemental"));
		stringBuilder.Append(" (<color=#FFF><size=+.2>");
		stringBuilder.Append(usedPointsCategory[2].ToString());
		stringBuilder.Append("</size></color>)");
		buttonType[2].SetText(stringBuilder.ToString());
		stringBuilder.Clear();
		stringBuilder.Append(Texts.Instance.GetText("mystical"));
		stringBuilder.Append(" (<color=#FFF><size=+.2>");
		stringBuilder.Append(usedPointsCategory[3].ToString());
		stringBuilder.Append("</size></color>)");
		buttonType[3].SetText(stringBuilder.ToString());
	}

	private void SetNodes()
	{
		foreach (KeyValuePair<string, PerkNode> perkNode in perkNodes)
		{
			if (usedPoints >= usedPointsArray[perkNode.Value.GetRow()])
			{
				perkNode.Value.SetLocked(_status: false);
			}
			else
			{
				perkNode.Value.SetLocked(_status: true);
			}
			perkNode.Value.SetRequired(_status: false);
			if (AtOManager.Instance.CharInTown() && AtOManager.Instance.GetTownTier() == 0 && perkNode.Value.PND.LockedInTown)
			{
				perkNode.Value.SetIconLock(_state: true);
			}
			else
			{
				perkNode.Value.SetIconLock();
			}
		}
		foreach (KeyValuePair<string, PerkNode> perkNode2 in perkNodes)
		{
			if (!perkNode2.Value.IsLocked() && perkNode2.Value.PND.PerkRequired != null && !IsThisPerkNodeDataSelected(perkNode2.Value.PND.PerkRequired))
			{
				perkNode2.Value.SetLocked(_status: true);
				perkNode2.Value.SetRequired(_status: true);
			}
		}
	}

	public void SelectPerk(string _perkId, PerkNode _perkNode)
	{
		if ((!AtOManager.Instance.CharInTown() || AtOManager.Instance.GetTownTier() != 0 || _perkNode.PND.LockedInTown) && (!HeroSelectionManager.Instance || GameManager.Instance.IsLoadingGame()))
		{
			return;
		}
		bool flag = false;
		_perkId = _perkId.ToLower();
		if (!selectedPerks.Contains(_perkId))
		{
			bool flag2 = false;
			if (_perkNode.IsChildNode() && perkChildIncompatible.ContainsKey(_perkNode.PND.Id))
			{
				for (int i = 0; i < perkChildIncompatible[_perkNode.PND.Id].Count; i++)
				{
					string text = perkChildIncompatible[_perkNode.PND.Id][i];
					if (text != "")
					{
						string item = perkNodes[text].PND.Perk.Id.ToLower();
						if (selectedPerks.Contains(item))
						{
							selectedPerks.Remove(item);
							selectedPerks.Add(_perkId);
							flag2 = true;
							flag = true;
							break;
						}
					}
				}
			}
			if (!flag2 && availablePoints >= GetPointsForNode(_perkNode.PND))
			{
				selectedPerks.Add(_perkId);
				flag = true;
			}
		}
		else
		{
			PerkNodeData pND = _perkNode.PND;
			foreach (KeyValuePair<string, PerkNodeData> perkNodeData in perkNodeDatas)
			{
				if (perkNodeData.Value.PerkRequired != null && perkNodeData.Value.PerkRequired.Id == pND.Id && IsThisPerkNodeDataSelected(perkNodeData.Value))
				{
					return;
				}
			}
			int num = 0;
			foreach (KeyValuePair<string, PerkNode> perkNode in perkNodes)
			{
				if (perkNode.Value.PND.Id != pND.Id && perkNode.Value.IsSelected() && perkNode.Value.GetRow() <= _perkNode.GetRow())
				{
					num += GetPointsForNode(_perkNode.PND);
				}
			}
			foreach (KeyValuePair<string, PerkNode> perkNode2 in perkNodes)
			{
				if (!perkNode2.Value.IsSelected() || !(perkNode2.Value.PND.Id != pND.Id))
				{
					continue;
				}
				int row = perkNode2.Value.GetRow();
				if (row <= _perkNode.GetRow())
				{
					continue;
				}
				num = 0;
				foreach (KeyValuePair<string, PerkNode> perkNode3 in perkNodes)
				{
					if (perkNode3.Value.PND.Id != pND.Id && perkNode3.Value.PND.Id != perkNode2.Value.PND.Id && perkNode3.Value.IsSelected() && perkNode3.Value.GetRow() < row)
					{
						num += GetPointsForNode(perkNode3.Value.PND);
					}
				}
				if (usedPointsArray[row] > num)
				{
					return;
				}
			}
			selectedPerks.Remove(_perkId);
			flag = true;
		}
		if (flag)
		{
			buttonConfirm.Enable();
		}
		Refresh();
		if (availablePoints < 0)
		{
			buttonConfirm.Disable();
		}
	}

	public void PerksAssignConfirm()
	{
		if ((bool)HeroSelectionManager.Instance)
		{
			PlayerManager.Instance.AssignPerkList(subClassId, selectedPerks);
			if (!GameManager.Instance.IsMultiplayer())
			{
				DoTeamPerks();
			}
		}
		else
		{
			Hero[] team = AtOManager.Instance.GetTeam();
			for (int i = 0; i < team.Length; i++)
			{
				if (team[i].HeroData.HeroSubClass.Id == subClassId)
				{
					AtOManager.Instance.AddPerkToHeroGlobalList(i, selectedPerks);
					if (!GameManager.Instance.IsMultiplayer())
					{
						DoTeamPerks();
					}
					break;
				}
			}
		}
		buttonConfirm.Disable();
	}

	public void PerksReset()
	{
		if (selectedPerks.Count > 0)
		{
			selectedPerks = new List<string>();
			buttonConfirm.Enable();
			Refresh();
		}
	}

	public Dictionary<string, PerkNodeData> GetPerkNodeDatas()
	{
		return perkNodeDatas;
	}

	public bool CanModify()
	{
		return canModify;
	}

	public void SavePerkSlot(int slot)
	{
		savingSlot = slot;
		AlertManager.buttonClickDelegate = SavePerkSlotAction;
		AlertManager.Instance.AlertInput(Texts.Instance.GetText("inputConfigSaveName"), Texts.Instance.GetText("accept").ToUpper());
	}

	public void SavePerkSlotAction()
	{
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(SavePerkSlotAction));
		string text = Functions.OnlyAscii(AlertManager.Instance.GetInputValue()).Trim();
		if (!(text == ""))
		{
			if (!PlayerManager.Instance.PlayerSavedPerk.PerkConfigTitle.ContainsKey(subClassId))
			{
				PlayerManager.Instance.PlayerSavedPerk.PerkConfigTitle.Add(subClassId, new string[10]);
			}
			if (!PlayerManager.Instance.PlayerSavedPerk.PerkConfigPerks.ContainsKey(subClassId))
			{
				PlayerManager.Instance.PlayerSavedPerk.PerkConfigPerks.Add(subClassId, new List<string>[10]);
			}
			if (!PlayerManager.Instance.PlayerSavedPerk.PerkConfigPoints.ContainsKey(subClassId))
			{
				PlayerManager.Instance.PlayerSavedPerk.PerkConfigPoints.Add(subClassId, new int[10]);
			}
			List<string> list = new List<string>();
			for (int i = 0; i < selectedPerks.Count; i++)
			{
				list.Add(selectedPerks[i]);
			}
			PlayerManager.Instance.PlayerSavedPerk.PerkConfigTitle[subClassId][savingSlot] = text;
			PlayerManager.Instance.PlayerSavedPerk.PerkConfigPerks[subClassId][savingSlot] = list;
			PlayerManager.Instance.PlayerSavedPerk.PerkConfigPoints[subClassId][savingSlot] = usedPoints;
			SaveManager.SavePlayerPerkConfig();
			LoadSavedPerks();
		}
	}

	public void RemovePerkSlot(int slot)
	{
		savingSlot = slot;
		AlertManager.buttonClickDelegate = RemovePerkSlotAction;
		AlertManager.Instance.AlertConfirmDouble(Texts.Instance.GetText("savedConfigDeleteConfirm"));
	}

	public void RemovePerkSlotAction()
	{
		bool confirmAnswer = AlertManager.Instance.GetConfirmAnswer();
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(RemovePerkSlotAction));
		if (confirmAnswer)
		{
			PlayerManager.Instance.PlayerSavedPerk.PerkConfigTitle[subClassId][savingSlot] = "";
			PlayerManager.Instance.PlayerSavedPerk.PerkConfigPerks[subClassId][savingSlot] = new List<string>();
			PlayerManager.Instance.PlayerSavedPerk.PerkConfigPoints[subClassId][savingSlot] = 0;
			SaveManager.SavePlayerPerkConfig();
			LoadSavedPerks();
		}
	}

	private void LoadSavedPerks()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < perkSlot.Length; i++)
		{
			if (perkSlot[i] != null)
			{
				stringBuilder.Clear();
				if (PlayerManager.Instance.PlayerSavedPerk.PerkConfigTitle.ContainsKey(subClassId) && PlayerManager.Instance.PlayerSavedPerk.PerkConfigTitle[subClassId][i] != null && PlayerManager.Instance.PlayerSavedPerk.PerkConfigTitle[subClassId][i] != "")
				{
					stringBuilder.Append(PlayerManager.Instance.PlayerSavedPerk.PerkConfigTitle[subClassId][i]);
					perkSlot[i].SetActive(stringBuilder.ToString(), PlayerManager.Instance.PlayerSavedPerk.PerkConfigPoints[subClassId][i].ToString());
				}
				else
				{
					perkSlot[i].SetEmpty(state: true);
				}
				if ((bool)HeroSelectionManager.Instance)
				{
					perkSlot[i].SetDisable(_state: false);
				}
				else
				{
					perkSlot[i].SetDisable(_state: true);
				}
			}
		}
	}

	public void LoadPerkConfig(int slot)
	{
		loadingSlot = slot;
		AlertManager.buttonClickDelegate = LoadPerkConfigAction;
		AlertManager.Instance.AlertConfirmDouble(Texts.Instance.GetText("wantOverwritePerks"));
	}

	public void LoadPerkConfigAction()
	{
		bool confirmAnswer = AlertManager.Instance.GetConfirmAnswer();
		AlertManager.buttonClickDelegate = (AlertManager.OnButtonClickDelegate)Delegate.Remove(AlertManager.buttonClickDelegate, new AlertManager.OnButtonClickDelegate(LoadPerkConfigAction));
		if (confirmAnswer)
		{
			selectedPerks = new List<string>();
			for (int i = 0; i < PlayerManager.Instance.PlayerSavedPerk.PerkConfigPerks[subClassId][loadingSlot].Count; i++)
			{
				selectedPerks.Add(PlayerManager.Instance.PlayerSavedPerk.PerkConfigPerks[subClassId][loadingSlot][i]);
			}
			Refresh();
			if (availablePoints < 0)
			{
				buttonConfirm.Disable();
			}
			else
			{
				buttonConfirm.Enable();
			}
		}
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false, int absolutePosition = -1)
	{
		_controllerList.Clear();
		for (int i = 0; i < buttonType.Length; i++)
		{
			_controllerList.Add(buttonType[i].transform);
		}
		foreach (KeyValuePair<string, GameObject> item in perkNodesGO)
		{
			if (Functions.TransformIsVisible(item.Value.transform))
			{
				_controllerList.Add(item.Value.transform);
			}
		}
		if (Functions.TransformIsVisible(buttonConfirm.transform))
		{
			_controllerList.Add(buttonConfirm.transform);
		}
		if (Functions.TransformIsVisible(buttonReset.transform))
		{
			_controllerList.Add(buttonReset.transform);
		}
		if (Functions.TransformIsVisible(buttonExport.transform))
		{
			_controllerList.Add(buttonExport.transform);
			_controllerList.Add(buttonImport.transform);
		}
		_controllerList.Add(buttonExit.transform);
		if (Functions.TransformIsVisible(perkSlot[0].transform))
		{
			for (int j = 0; j < perkSlot.Length; j++)
			{
				if (perkSlot[j].GetComponent<BoxCollider2D>().enabled)
				{
					_controllerList.Add(perkSlot[j].transform);
					if (Functions.TransformIsVisible(perkSlot[j].transform.GetChild(5).transform))
					{
						_controllerList.Add(perkSlot[j].transform.GetChild(5).transform);
					}
				}
				else if (Functions.TransformIsVisible(perkSlot[j].transform.GetChild(4).transform))
				{
					_controllerList.Add(perkSlot[j].transform.GetChild(4).transform);
				}
			}
		}
		controllerHorizontalIndex = Functions.GetListClosestIndexToMousePosition(_controllerList);
		controllerHorizontalIndex = Functions.GetClosestIndexBasedOnDirection(_controllerList, controllerHorizontalIndex, goingUp, goingRight, goingDown, goingLeft);
		if (_controllerList[controllerHorizontalIndex] != null)
		{
			warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerList[controllerHorizontalIndex].position);
			Mouse.current.WarpCursorPosition(warpPosition);
		}
	}
}
