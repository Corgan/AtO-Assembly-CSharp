using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class PerkNode : MonoBehaviour
{
	public PerkNodeData PND;

	public LineRenderer nodeLine;

	public Transform iconLock;

	public Transform bgSquarePlain;

	public Transform bgSquareGold;

	public Transform bgSquareSelected;

	public Transform bgSquareHover;

	public Transform bgSquareLocked;

	public Transform bgCirclePlain;

	public Transform bgCirclePlainSelected;

	public Transform bgCirclePlainHover;

	public Transform bgCirclePlainLocked;

	public Transform bgCircleGold;

	public Transform bgCircleGoldSelected;

	public Transform bgCircleGoldHover;

	public Transform bgCircleGoldLocked;

	public Transform teamSelected;

	public TMP_Text teamSelectedNum;

	private List<string> teamSelectedCharacters;

	public Transform amplify;

	public SpriteRenderer nodeSprite;

	private BoxCollider2D boxCollider;

	private Vector2 boxColliderSizeOri;

	private Vector2 bcOffset = new Vector2(0f, -1.1f);

	private Vector2 bcSize2 = new Vector2(4.5f, 3.6f);

	private Vector2 bcSize3 = new Vector2(5.4f, 3.6f);

	private Vector2 bcSize4 = new Vector2(6.3f, 3.6f);

	private string nodeId;

	private PerkNode perkNodeConnected;

	private int nodeType;

	private int nodeRow;

	private int nodeCost;

	private bool nodeLocked;

	private bool nodeRequired;

	private bool nodeSelected;

	private bool nodeIsChild;

	private string textPopup = "";

	private Coroutine amplifyCo;

	private float oriScale;

	private PerkNodeAmplify nodeAmplify;

	private void Awake()
	{
		boxCollider = GetComponent<BoxCollider2D>();
		boxColliderSizeOri = boxCollider.size;
	}

	private void Update()
	{
		if (nodeAmplify != null && base.transform.localPosition.z == -2f && !nodeAmplify.gameObject.activeSelf)
		{
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, 0f);
		}
	}

	public void SetIconLock(bool _state = false)
	{
		if (_state)
		{
			iconLock.gameObject.SetActive(value: true);
		}
		else
		{
			iconLock.gameObject.SetActive(value: false);
		}
	}

	public void SetNodeAsChild(bool _state = true)
	{
		nodeIsChild = _state;
		if (_state)
		{
			SetChildLayers();
		}
	}

	public bool IsChildNode()
	{
		return nodeIsChild;
	}

	public void SetValuesAsChildNode(PerkNode _pn)
	{
		nodeSprite.sprite = _pn.nodeSprite.sprite;
		perkNodeConnected = _pn;
		SetSelected(_status: true);
	}

	public void SetDefaultIcon()
	{
		nodeSprite.sprite = PND.Sprite;
		perkNodeConnected = null;
	}

	public void SetPND(PerkNodeData _pnd)
	{
		oriScale = base.transform.localScale.x;
		nodeLine.transform.parent = base.transform.parent;
		PND = _pnd;
		nodeId = PND.Id;
		nodeRow = PND.Row;
		nodeCost = PerkTree.Instance.GetPointsForNode(PND);
		if (nodeIsChild)
		{
			nodeType = 3;
		}
		else if (_pnd.PerksConnected.Length != 0)
		{
			if (_pnd.Cost == Enums.PerkCost.PerkCostBase)
			{
				nodeType = 2;
			}
			else
			{
				nodeType = 3;
			}
		}
		else if (_pnd.Cost == Enums.PerkCost.PerkCostBase)
		{
			nodeType = 0;
		}
		else
		{
			nodeType = 1;
		}
		nodeSprite.sprite = _pnd.Sprite;
		Init();
	}

	public void TeamSelected(int _number, List<string> _members = null)
	{
		if (_number > 0)
		{
			teamSelected.gameObject.SetActive(value: true);
			teamSelectedNum.text = _number.ToString();
			teamSelectedCharacters = _members;
		}
		else
		{
			teamSelected.gameObject.SetActive(value: false);
			teamSelectedCharacters = null;
		}
	}

	public void Init()
	{
		DeselectNode();
		TeamSelected(0);
		foreach (Transform item in PerkTree.Instance.nodes.GetChild(PND.Type))
		{
			if (item.gameObject.name == base.gameObject.name + "_amplify")
			{
				amplify = item;
				nodeAmplify = amplify.gameObject.GetComponent<PerkNodeAmplify>();
				break;
			}
		}
		if (amplify != null)
		{
			amplify.gameObject.SetActive(value: false);
		}
		SetNodeGraphic();
	}

	private void SetNodeGraphic()
	{
		if (nodeType == 0)
		{
			bgSquarePlain.gameObject.SetActive(value: false);
			bgSquareGold.gameObject.SetActive(value: false);
			bgCirclePlain.gameObject.SetActive(value: true);
			bgCircleGold.gameObject.SetActive(value: false);
		}
		else if (nodeType == 1)
		{
			bgSquarePlain.gameObject.SetActive(value: false);
			bgSquareGold.gameObject.SetActive(value: false);
			bgCirclePlain.gameObject.SetActive(value: false);
			bgCircleGold.gameObject.SetActive(value: true);
		}
		else if (nodeType == 2)
		{
			bgSquarePlain.gameObject.SetActive(value: true);
			bgSquareGold.gameObject.SetActive(value: false);
			bgCirclePlain.gameObject.SetActive(value: false);
			bgCircleGold.gameObject.SetActive(value: false);
		}
		else if (nodeType == 3)
		{
			bgSquarePlain.gameObject.SetActive(value: false);
			bgSquareGold.gameObject.SetActive(value: true);
			bgCirclePlain.gameObject.SetActive(value: false);
			bgCircleGold.gameObject.SetActive(value: false);
		}
	}

	public void SetArrow(Vector3 _from, Vector3 _to)
	{
		if (!nodeIsChild)
		{
			nodeLine.SetPosition(0, _from);
			nodeLine.SetPosition(1, _to);
			nodeLine.transform.gameObject.SetActive(value: true);
		}
		else
		{
			nodeLine.transform.gameObject.SetActive(value: false);
		}
	}

	public void SetRequired(bool _status)
	{
		nodeRequired = _status;
	}

	public void SetLocked(bool _status)
	{
		nodeLocked = _status;
		if (nodeType == 0)
		{
			bgCirclePlainLocked.gameObject.SetActive(_status);
		}
		else if (nodeType == 1)
		{
			bgCircleGoldLocked.gameObject.SetActive(_status);
		}
		else
		{
			bgSquareLocked.gameObject.SetActive(_status);
		}
		Color startColor;
		if (_status)
		{
			nodeSprite.color = new Color(0.5f, 0.5f, 0.5f);
			LineRenderer lineRenderer = nodeLine;
			startColor = (nodeLine.endColor = new Color(nodeLine.startColor.r, nodeLine.startColor.g, nodeLine.startColor.b, 0.3f));
			lineRenderer.startColor = startColor;
			return;
		}
		if (nodeSelected)
		{
			nodeSprite.color = Color.white;
		}
		else
		{
			nodeSprite.color = new Color(0.8f, 0.8f, 0.8f);
		}
		LineRenderer lineRenderer2 = nodeLine;
		startColor = (nodeLine.endColor = new Color(nodeLine.startColor.r, nodeLine.startColor.g, nodeLine.startColor.b, 1f));
		lineRenderer2.startColor = startColor;
	}

	public bool IsLocked()
	{
		return nodeLocked;
	}

	public void SetSelected(bool _status)
	{
		nodeSelected = _status;
		if (nodeType == 0)
		{
			bgCirclePlainSelected.gameObject.SetActive(_status);
		}
		else if (nodeType == 1)
		{
			bgCircleGoldSelected.gameObject.SetActive(_status);
		}
		else
		{
			bgSquareSelected.gameObject.SetActive(_status);
		}
	}

	public bool IsSelected()
	{
		return nodeSelected;
	}

	public int GetRow()
	{
		return nodeRow;
	}

	private void ResetNode()
	{
		DeselectNode();
	}

	private void DeselectNode()
	{
		bgSquareSelected.gameObject.SetActive(value: false);
		bgCirclePlainSelected.gameObject.SetActive(value: false);
		bgCircleGoldSelected.gameObject.SetActive(value: false);
	}

	public string NewPerkDescription(PerkData perkData)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (PND.PerksConnected.Length != 0)
		{
			if (PND.PerksConnected != null && PND.PerksConnected.Length != 0 && perkNodeConnected != null)
			{
				perkData = perkNodeConnected.PND.Perk;
			}
			else
			{
				stringBuilder.Append("<size=+5>");
				stringBuilder.Append(Texts.Instance.GetText("chooseOnePerk"));
				stringBuilder.Append("</size>");
				perkData = null;
			}
		}
		if (perkData != null)
		{
			if (perkData.CustomDescription != null && perkData.CustomDescription.Trim() != "")
			{
				stringBuilder.Append(Texts.Instance.GetText(perkData.CustomDescription));
			}
			else if (perkData.MaxHealth > 0)
			{
				stringBuilder.Append(string.Format(Texts.Instance.GetText("itemMaxHp"), perkData.IconTextValue));
			}
			else if (perkData.AdditionalCurrency > 0)
			{
				stringBuilder.Append(string.Format(Functions.UppercaseFirst(Texts.Instance.GetText("itemInitialCurrencySingle")), perkData.IconTextValue));
			}
			else if (perkData.AdditionalShards > 0)
			{
				stringBuilder.Append(string.Format(Functions.UppercaseFirst(Texts.Instance.GetText("itemInitialShardsSingle")), perkData.IconTextValue));
			}
			else if (perkData.SpeedQuantity > 0)
			{
				stringBuilder.Append(string.Format(Functions.UppercaseFirst(Texts.Instance.GetText("itemSpeed")), perkData.IconTextValue));
			}
			else if (perkData.HealQuantity > 0)
			{
				stringBuilder.Append(string.Format(Texts.Instance.GetText("itemHealDone").Replace("<c>", "").OnlyFirstCharToUpper(), perkData.IconTextValue));
			}
			else if (perkData.EnergyBegin > 0)
			{
				stringBuilder.Append(string.Format(Texts.Instance.GetText("itemInitialEnergy").OnlyFirstCharToUpper(), perkData.IconTextValue));
			}
			else if (perkData.AuracurseBonus != null)
			{
				stringBuilder.Append(string.Format(Texts.Instance.GetText("perkAuraDescription").OnlyFirstCharToUpper(), perkData.IconTextValue));
			}
			else if (perkData.ResistModified == Enums.DamageType.All)
			{
				stringBuilder.Append(string.Format(Texts.Instance.GetText("itemAllResistances").OnlyFirstCharToUpper(), perkData.IconTextValue));
			}
			else if (perkData.DamageFlatBonus == Enums.DamageType.All)
			{
				stringBuilder.Append(string.Format(Texts.Instance.GetText("itemAllDamages").OnlyFirstCharToUpper(), perkData.IconTextValue));
			}
			else if (perkData.DamageFlatBonus == Enums.DamageType.All)
			{
				stringBuilder.Append(string.Format(Texts.Instance.GetText("itemAllDamages").OnlyFirstCharToUpper(), perkData.IconTextValue));
			}
			else if (perkData.DamageFlatBonus != Enums.DamageType.None && perkData.DamageFlatBonus != Enums.DamageType.All)
			{
				stringBuilder.Append(string.Format(Texts.Instance.GetText("itemSingleDamage").OnlyFirstCharToUpper(), Texts.Instance.GetText(Enum.GetName(typeof(Enums.DamageType), perkData.DamageFlatBonus)), perkData.IconTextValue));
			}
			if (stringBuilder.Length > 0)
			{
				if (PND.NotStack || (perkNodeConnected != null && perkNodeConnected.PND != null && perkNodeConnected.PND.NotStack))
				{
					stringBuilder.Append("  <nobr><color=#A0A0A0>");
					stringBuilder.Append(Texts.Instance.GetText("notStack"));
					stringBuilder.Append("</color></nobr>");
				}
				stringBuilder.Append("  <nobr><color=#A0A0A0>[");
				stringBuilder.Append(string.Format(Texts.Instance.GetText("cardsCost"), "<space=-3>: " + nodeCost));
				stringBuilder.Append("]</color></nobr>");
				if (nodeLocked)
				{
					stringBuilder.Append("<br></size><line-height=5><color=#FF6666>");
					if (!nodeRequired)
					{
						stringBuilder.Append(string.Format(Texts.Instance.GetText("requiredPoints"), PerkTree.Instance.GetPointsNeeded(nodeRow)));
					}
					else
					{
						stringBuilder.Append(Texts.Instance.GetText("previousRequired"));
					}
				}
				else if (!nodeSelected)
				{
					if (PerkTree.Instance.GetPointsAvailable() > 0)
					{
						if (PerkTree.Instance.GetPointsAvailable() < nodeCost)
						{
							stringBuilder.Append("<br></size><line-height=5><color=#FF6666>");
							stringBuilder.Append(string.Format(Texts.Instance.GetText("requiredPoints"), nodeCost - PerkTree.Instance.GetPointsAvailable()));
						}
						else if (PerkTree.Instance.CanModify() && !iconLock.gameObject.activeSelf)
						{
							stringBuilder.Append("<br></size><line-height=5><color=#3BA12A>");
							stringBuilder.Append(Texts.Instance.GetText("rankPerkPress"));
						}
					}
					else
					{
						stringBuilder.Append("<br></size><line-height=5><color=#FF6666>");
						stringBuilder.Append(Texts.Instance.GetText("rankPerkNotEnough"));
						base.enabled = false;
					}
				}
				stringBuilder.Insert(0, "<color=#FFF><size=+5>");
			}
		}
		if (teamSelectedCharacters != null)
		{
			stringBuilder.Append("</line-height><br><line-height=18><color=#E5A462><size=20>");
			stringBuilder.Append(Texts.Instance.GetText("partyMembersWithPerk"));
			stringBuilder.Append("<br>(");
			stringBuilder.Append(teamSelectedCharacters.Count);
			stringBuilder.Append(")  ");
			for (int i = 0; i < teamSelectedCharacters.Count; i++)
			{
				if (teamSelectedCharacters[i] != null && Globals.Instance.GetSubClassData(teamSelectedCharacters[i]) != null)
				{
					stringBuilder.Append(Globals.Instance.GetSubClassData(teamSelectedCharacters[i]).CharacterName);
					if (i < teamSelectedCharacters.Count - 1)
					{
						stringBuilder.Append(", ");
					}
				}
			}
			stringBuilder.Append("</size></color></line-height>");
		}
		if (stringBuilder.Length > 0)
		{
			stringBuilder.Replace("<c>", "");
			stringBuilder.Replace("</c>", "");
			return stringBuilder.ToString();
		}
		return "";
	}

	private void DoPopup()
	{
		if ((PND.PerksConnected == null || PND.PerksConnected.Length == 0) && (PND.Perk == null || !PND.Perk.MainPerk))
		{
			return;
		}
		textPopup = NewPerkDescription(PND.Perk);
		if (textPopup != "" && !(nodeSprite == null))
		{
			AuraCurseData auraCurseData = Globals.Instance.GetAuraCurseData(nodeSprite.sprite.name);
			string keynote = "";
			if (auraCurseData != null)
			{
				keynote = nodeSprite.sprite.name;
			}
			PopupManager.Instance.SetPerk(nodeId, textPopup, keynote);
			if (!nodeLocked)
			{
				PopupManager.Instance.SetBackgroundColor("#226529");
			}
			else
			{
				PopupManager.Instance.SetBackgroundColor("#652523");
			}
		}
	}

	public void SetChildLayers()
	{
		int num = 150;
		foreach (Transform item in base.transform)
		{
			SpriteRenderer component = item.GetComponent<SpriteRenderer>();
			if (component != null)
			{
				component.sortingOrder += num;
			}
			int num2 = 0;
			foreach (Transform item2 in item)
			{
				SpriteRenderer component2 = item2.GetComponent<SpriteRenderer>();
				if (component2 != null)
				{
					component2.sortingOrder += num;
					if (item2.name == "Circle")
					{
						num2 = component2.sortingOrder;
					}
				}
				else if (item2.GetComponent<MeshRenderer>() != null)
				{
					item2.GetComponent<MeshRenderer>().sortingOrder = num2 + 1;
				}
				foreach (Transform item3 in item2)
				{
					SpriteRenderer component3 = item3.GetComponent<SpriteRenderer>();
					if (component3 != null)
					{
						component3.sortingOrder += num;
					}
				}
			}
		}
	}

	public void OnMouseUp()
	{
		if (Functions.ClickedThisTransform(base.transform) && !SettingsManager.Instance.IsActive() && !AlertManager.Instance.IsActive() && !MadnessManager.Instance.IsActive() && !SandboxManager.Instance.IsActive() && PerkTree.Instance.IsOwner && !nodeLocked && PerkTree.Instance.CanModify() && !iconLock.gameObject.activeSelf)
		{
			if (PND.Perk != null)
			{
				PerkTree.Instance.SelectPerk(PND.Perk.Id, this);
				DoPopup();
			}
			else if (PND.PerksConnected != null && PND.PerksConnected.Length != 0 && perkNodeConnected != null)
			{
				PerkTree.Instance.SelectPerk(perkNodeConnected.PND.Perk.Id, perkNodeConnected);
				DoPopup();
			}
		}
	}

	private void OnMouseEnter()
	{
		if (SettingsManager.Instance.IsActive() || AlertManager.Instance.IsActive() || MadnessManager.Instance.IsActive() || SandboxManager.Instance.IsActive())
		{
			return;
		}
		DoPopup();
		if (PND.PerksConnected.Length != 0)
		{
			if (amplify != null)
			{
				amplifyCo = StartCoroutine(OpenAmplify());
			}
		}
		else if (!nodeLocked && PerkTree.Instance.CanModify() && !iconLock.gameObject.activeSelf)
		{
			if (nodeType == 0)
			{
				bgCirclePlainHover.gameObject.SetActive(value: true);
			}
			else if (nodeType == 1)
			{
				bgCircleGoldHover.gameObject.SetActive(value: true);
			}
			else
			{
				bgSquareHover.gameObject.SetActive(value: true);
			}
			base.transform.localScale = new Vector3(oriScale + 0.1f, oriScale + 0.1f, 1f);
			GameManager.Instance.PlayLibraryAudio("ui_menu_popup_01");
			GameManager.Instance.SetCursorHover();
		}
	}

	private IEnumerator OpenAmplify()
	{
		yield return Globals.Instance.WaitForSeconds(0.2f);
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, -2f);
		nodeAmplify.Show();
	}

	private void OnMouseExit()
	{
		GameManager.Instance.SetCursorPlain();
		PopupManager.Instance.ClosePopup();
		if (amplifyCo != null)
		{
			StopCoroutine(amplifyCo);
		}
		if (SettingsManager.Instance.IsActive() || AlertManager.Instance.IsActive() || MadnessManager.Instance.IsActive() || SandboxManager.Instance.IsActive() || !PerkTree.Instance.IsOwner)
		{
			return;
		}
		if (PND.PerksConnected.Length != 0)
		{
			if (!(amplify != null))
			{
			}
		}
		else if (!nodeLocked && PerkTree.Instance.CanModify() && !iconLock.gameObject.activeSelf)
		{
			if (nodeType == 0)
			{
				bgCirclePlainHover.gameObject.SetActive(value: false);
			}
			else if (nodeType == 1)
			{
				bgCircleGoldHover.gameObject.SetActive(value: false);
			}
			else
			{
				bgSquareHover.gameObject.SetActive(value: false);
			}
		}
		base.transform.localScale = new Vector3(oriScale, oriScale, 1f);
	}
}
