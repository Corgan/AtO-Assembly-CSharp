using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public class PopupNode : MonoBehaviour
{
	public Transform Element1;

	public Transform Element2;

	public Transform Ground;

	public TMP_Text GroundText;

	public Transform Bg0;

	public Transform Bg1;

	public Transform Bg2;

	public TMP_Text Title;

	public SpriteRenderer Icon;

	public SpriteRenderer[] CharIcon;

	public TMP_Text Element1Title;

	public TMP_Text Element2Title;

	public Transform monsterSpriteFrontChampion;

	public SpriteRenderer monsterSpriteFrontChampionIcoBack;

	public Transform monsterSpriteBackChampion;

	public SpriteRenderer monsterSpriteBackChampionIcoBack;

	private bool show;

	private bool popupDone;

	private int elementsNum;

	private float popW = 1.5f;

	private Vector3 popDestination;

	private void Start()
	{
	}

	private void Update()
	{
		if (show)
		{
			if (base.transform.localPosition.x - popW < Globals.Instance.sizeW * -0.5f)
			{
				popDestination = new Vector3(Globals.Instance.sizeW * -0.5f + popW, base.transform.localPosition.y, base.transform.localPosition.z);
			}
			else if (base.transform.localPosition.x + popW > Globals.Instance.sizeW * 0.5f)
			{
				popDestination = new Vector3(Globals.Instance.sizeW * 0.5f - popW, base.transform.localPosition.y, base.transform.localPosition.z);
			}
			else
			{
				Vector3 ori = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				popDestination = CalcDestination(ori);
			}
			if (popDestination.y > Globals.Instance.sizeH * 0.5f)
			{
				popDestination = new Vector3(popDestination.x, Globals.Instance.sizeH * 0.5f, popDestination.z);
			}
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, popDestination, 0.06f);
		}
	}

	private Vector3 CalcDestination(Vector3 ori)
	{
		float num = ((elementsNum == 0) ? 1f : ((elementsNum != 1) ? 1.5f : 1.25f));
		return new Vector3(ori.x + 0.2f, ori.y + num + 0.3f, 0f);
	}

	private void ShowHideElement(int num, bool state)
	{
		switch (num)
		{
		case 0:
			Bg0.gameObject.SetActive(state);
			break;
		case 1:
			Bg1.gameObject.SetActive(state);
			Element1.gameObject.SetActive(state);
			break;
		case 2:
			Bg2.gameObject.SetActive(state);
			Element2.gameObject.SetActive(state);
			break;
		}
	}

	private void WriteTitle(int num, string text, Enums.MapIconShader shader = Enums.MapIconShader.None, Sprite spriteMap = null)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(text);
		if (!GameManager.Instance.IsObeliskChallenge())
		{
			string text2 = "";
			if (spriteMap != null)
			{
				text2 = spriteMap.name.ToLower();
			}
			if (shader != Enums.MapIconShader.None || !(spriteMap == null))
			{
				stringBuilder.Append("\n");
				stringBuilder.Append("<size=-.2><color=#");
				switch (text2)
				{
				case "nodeiconeventgreen":
					stringBuilder.Append(ColorUtility.ToHtmlStringRGBA(Globals.Instance.RarityColor["uncommon"]));
					stringBuilder.Append(">");
					stringBuilder.Append(Texts.Instance.GetText("eventUncommon"));
					break;
				case "nodeiconeventblue":
					stringBuilder.Append(ColorUtility.ToHtmlStringRGBA(Globals.Instance.RarityColor["rare"]));
					stringBuilder.Append(">");
					stringBuilder.Append(Texts.Instance.GetText("eventRare"));
					break;
				case "nodeiconeventpurple":
					stringBuilder.Append(ColorUtility.ToHtmlStringRGBA(Globals.Instance.ColorColor["purple"]));
					stringBuilder.Append(">");
					stringBuilder.Append(Texts.Instance.GetText("eventEpic"));
					break;
				case "nodeiconmap":
					stringBuilder.Append(ColorUtility.ToHtmlStringRGBA(Globals.Instance.RarityColor["mythic"]));
					stringBuilder.Append(">");
					stringBuilder.Append(Texts.Instance.GetText("mapLegendMapTransition"));
					break;
				case "nodeicontimerift":
					stringBuilder.Append(ColorUtility.ToHtmlStringRGBA(Globals.Instance.ColorColor["purple"]));
					stringBuilder.Append(">");
					if (AtOManager.Instance.GetTownZoneId().ToLower() == "aquarfall")
					{
						stringBuilder.Append(Texts.Instance.GetText("sunken"));
					}
					else if (AtOManager.Instance.GetTownZoneId().ToLower() == "velkarath")
					{
						stringBuilder.Append(Texts.Instance.GetText("uprising"));
					}
					break;
				case "quest-yogger":
					stringBuilder.Append(ColorUtility.ToHtmlStringRGBA(Globals.Instance.RarityColor["mythic"]));
					stringBuilder.Append(">");
					stringBuilder.Append(Texts.Instance.GetText("wolfwars"));
					break;
				default:
					if (shader == Enums.MapIconShader.Orange)
					{
						stringBuilder.Append(ColorUtility.ToHtmlStringRGBA(Globals.Instance.RarityColor["mythic"]));
						stringBuilder.Append(">");
						stringBuilder.Append(Texts.Instance.GetText("eventCharacter"));
					}
					else if (text2 == "nodeiconeventteal")
					{
						stringBuilder.Append("AAAAAA>");
						stringBuilder.Append(Texts.Instance.GetText("eventCommon"));
					}
					else
					{
						stringBuilder.Append("FFFFFF>");
					}
					break;
				}
				stringBuilder.Append("</color>");
			}
		}
		switch (num)
		{
		case 0:
			Title.text = stringBuilder.ToString();
			break;
		case 1:
			Element1Title.text = stringBuilder.ToString();
			break;
		default:
			Element2Title.text = stringBuilder.ToString();
			break;
		}
	}

	private void DoPopup(Node _node)
	{
		monsterSpriteFrontChampion.gameObject.SetActive(value: false);
		monsterSpriteBackChampion.gameObject.SetActive(value: false);
		Enums.MapIconShader shader = Enums.MapIconShader.None;
		Sprite spriteMap = null;
		if (!popupDone)
		{
			if (_node.nodeData == null)
			{
				Hide();
				return;
			}
			if (_node.nodeData.NodeName == "")
			{
				Hide();
				return;
			}
			string text = "";
			text = ((!GameManager.Instance.IsObeliskChallenge()) ? Globals.Instance.GetNodeData(_node.nodeData.NodeId).NodeName : Texts.Instance.GetText("ObeliskChallenge"));
			if (text == "")
			{
				text = _node.nodeData.NodeName;
			}
			WriteTitle(0, text);
			Icon.gameObject.SetActive(value: true);
			Icon.sprite = _node.nodeImage.sprite;
			string text2 = "";
			bool flag = false;
			if (PlayerManager.Instance.IsNodeUnlocked(_node.GetNodeAssignedId()) || PlayerManager.Instance.IsNodeUnlocked(_node.nodeData.NodeId))
			{
				flag = true;
			}
			bool flag2 = false;
			if (_node.GetNodeAction() == "combat" && _node.nodeData.NodeCombatTier != Enums.CombatTier.T0 && (MadnessManager.Instance.IsMadnessTraitActive("randomcombats") || GameManager.Instance.IsObeliskChallenge() || AtOManager.Instance.IsChallengeTraitActive("randomcombats")))
			{
				flag2 = true;
			}
			if (_node.nodeData != null && _node.nodeData.DisableRandom)
			{
				flag2 = false;
			}
			if (flag2)
			{
				string nodeId = _node.nodeData.NodeId;
				string nodeAssignedId = _node.GetNodeAssignedId();
				NodeData nodeData = Globals.Instance.GetNodeData(nodeId);
				string text3 = "";
				CombatData combatData = Globals.Instance.GetCombatData(nodeAssignedId);
				if (combatData != null)
				{
					text3 = combatData.CombatId;
				}
				NPCData[] array = Functions.GetRandomCombat(seed: (nodeId + AtOManager.Instance.GetGameId() + text3).GetDeterministicHashCode(), combatTier: nodeData.NodeCombatTier, nodeSelectedId: nodeId);
				if (array != null)
				{
					elementsNum = 2;
					ShowHideElement(1, state: false);
					ShowHideElement(2, state: true);
					float num = -1.02f;
					float num2 = 0.54f;
					int num3 = 0;
					int num4 = 0;
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] != null)
						{
							num4++;
						}
					}
					if (num4 == 3)
					{
						num = -0.75f;
					}
					for (int j = 0; j < 4; j++)
					{
						if (j < array.Length && array[j] != null)
						{
							CharIcon[j].sprite = array[j].SpriteSpeed;
							CharIcon[j].gameObject.SetActive(value: true);
							CharIcon[j].transform.localPosition = new Vector3(num + num2 * (float)num3, CharIcon[j].transform.localPosition.y, CharIcon[j].transform.localPosition.z);
							num3++;
						}
						else
						{
							CharIcon[j].gameObject.SetActive(value: false);
						}
					}
					if (array[0] != null && array[0].IsNamed)
					{
						string auraCurseImmune = Functions.GetAuraCurseImmune(array[0], nodeId);
						AuraCurseData auraCurseData = Globals.Instance.GetAuraCurseData(auraCurseImmune);
						if (auraCurseData != null)
						{
							monsterSpriteFrontChampion.gameObject.SetActive(value: true);
							monsterSpriteFrontChampion.GetComponent<SpriteRenderer>().sprite = auraCurseData.Sprite;
							monsterSpriteFrontChampionIcoBack.sprite = auraCurseData.Sprite;
						}
					}
					if (array[3] != null && array[3].IsNamed)
					{
						string auraCurseImmune2 = Functions.GetAuraCurseImmune(array[3], nodeId);
						AuraCurseData auraCurseData2 = Globals.Instance.GetAuraCurseData(auraCurseImmune2);
						if (auraCurseData2 != null)
						{
							monsterSpriteBackChampion.gameObject.SetActive(value: true);
							monsterSpriteBackChampion.GetComponent<SpriteRenderer>().sprite = auraCurseData2.Sprite;
							monsterSpriteBackChampionIcoBack.sprite = auraCurseData2.Sprite;
						}
					}
					if (AtOManager.Instance.Sandbox_lessNPCs != 0)
					{
						SortedDictionary<int, int> sortedDictionary = new SortedDictionary<int, int>();
						for (int k = 0; k < array.Length; k++)
						{
							if (array[k] != null && !array[k].IsNamed && !array[k].IsBoss)
							{
								sortedDictionary.Add(array[k].Hp * 10000 + k, k);
							}
						}
						int num5 = AtOManager.Instance.Sandbox_lessNPCs;
						if (num5 >= num4)
						{
							num5 = num4 - 1;
						}
						if (num5 > sortedDictionary.Count)
						{
							num5 = sortedDictionary.Count;
						}
						for (int l = 0; l < num5; l++)
						{
							CharIcon[sortedDictionary.ElementAt(l).Value].gameObject.SetActive(value: false);
						}
					}
				}
			}
			else if ((!GameManager.Instance.IsObeliskChallenge() && !flag) || (GameManager.Instance.IsObeliskChallenge() && !AtOManager.Instance.mapVisitedNodes.Contains(_node.nodeData.NodeId)))
			{
				if (_node != null)
				{
					EventData eventData = Globals.Instance.GetEventData(_node.GetNodeAssignedId());
					if (eventData != null)
					{
						shader = eventData.EventIconShader;
						spriteMap = eventData.EventSpriteMap;
					}
				}
				text2 = "?????";
				elementsNum = 1;
				ShowHideElement(1, state: true);
				ShowHideElement(2, state: false);
			}
			else if (_node.GetNodeAction() == "combat")
			{
				elementsNum = 2;
				ShowHideElement(1, state: false);
				ShowHideElement(2, state: true);
				CombatData combatData2 = Globals.Instance.GetCombatData(_node.GetNodeAssignedId());
				float num6 = -1.02f;
				float num7 = 0.54f;
				int num8 = 0;
				int num9 = 0;
				for (int m = 0; m < combatData2.NPCList.Length; m++)
				{
					if (combatData2.NPCList[m] != null)
					{
						num9++;
					}
				}
				if (((GameManager.Instance.IsGameAdventure() && AtOManager.Instance.GetMadnessDifficulty() == 0) || (GameManager.Instance.IsSingularity() && AtOManager.Instance.GetSingularityMadness() == 0)) && combatData2.NpcRemoveInMadness0Index > -1 && AtOManager.Instance.GetActNumberForText() < 3)
				{
					num9--;
				}
				switch (num9)
				{
				case 3:
					num6 = -0.75f;
					break;
				case 2:
					num6 = -0.48f;
					break;
				}
				for (int n = 0; n < 4; n++)
				{
					CharIcon[n].gameObject.SetActive(value: false);
				}
				for (int num10 = 0; num10 < combatData2.NPCList.Length; num10++)
				{
					if ((((!GameManager.Instance.IsGameAdventure() || AtOManager.Instance.GetMadnessDifficulty() != 0) && (!GameManager.Instance.IsSingularity() || AtOManager.Instance.GetSingularityMadness() != 0)) || combatData2.NpcRemoveInMadness0Index != num10 || AtOManager.Instance.GetActNumberForText() >= 3) && combatData2.NPCList[num10] != null)
					{
						CharIcon[num10].sprite = combatData2.NPCList[num10].SpriteSpeed;
						CharIcon[num10].gameObject.SetActive(value: true);
						CharIcon[num10].transform.localPosition = new Vector3(num6 + num7 * (float)num8, CharIcon[num10].transform.localPosition.y, CharIcon[num10].transform.localPosition.z);
						num8++;
					}
				}
				if (AtOManager.Instance.Sandbox_lessNPCs != 0)
				{
					SortedDictionary<int, int> sortedDictionary2 = new SortedDictionary<int, int>();
					for (int num11 = 0; num11 < combatData2.NPCList.Length; num11++)
					{
						if (combatData2.NPCList[num11] != null && !combatData2.NPCList[num11].IsNamed && !combatData2.NPCList[num11].IsBoss)
						{
							sortedDictionary2.Add(combatData2.NPCList[num11].Hp * 10000 + num11, num11);
						}
					}
					int num12 = AtOManager.Instance.Sandbox_lessNPCs;
					if (num12 >= num8)
					{
						num12 = num8 - 1;
					}
					if (num12 > sortedDictionary2.Count)
					{
						num12 = sortedDictionary2.Count;
					}
					for (int num13 = 0; num13 < num12; num13++)
					{
						CharIcon[sortedDictionary2.ElementAt(num13).Value].gameObject.SetActive(value: false);
					}
				}
			}
			else
			{
				string nodeAssignedId2 = _node.GetNodeAssignedId();
				if (!(nodeAssignedId2 != ""))
				{
					return;
				}
				if (nodeAssignedId2 != "town" && nodeAssignedId2 != "destination")
				{
					EventData eventData2 = Globals.Instance.GetEventData(nodeAssignedId2);
					string text4 = Texts.Instance.GetText(eventData2.EventId + "_nm", "events");
					text2 = ((!(text4 != "")) ? eventData2.EventName : text4);
					shader = eventData2.EventIconShader;
					spriteMap = eventData2.EventSpriteMap;
					elementsNum = 1;
					ShowHideElement(1, state: true);
					ShowHideElement(2, state: false);
				}
				else
				{
					string text5 = AtOManager.Instance.GetTownZoneId().ToLower();
					string text6 = ((!Globals.Instance.ZoneDataSource.ContainsKey(text5)) ? Texts.Instance.GetText(text5) : Texts.Instance.GetText(Globals.Instance.ZoneDataSource[text5].ZoneName));
					text2 = ((!(text6 != "")) ? text5 : text6);
					Icon.gameObject.SetActive(value: false);
					elementsNum = 0;
					ShowHideElement(1, state: true);
					ShowHideElement(2, state: false);
				}
			}
			if (_node.nodeData.NodeGround != Enums.NodeGround.None)
			{
				if (!Ground.gameObject.activeSelf)
				{
					Ground.gameObject.SetActive(value: true);
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(Functions.GetNodeGroundSprite(_node.nodeData.NodeGround));
				if (stringBuilder.ToString() != "")
				{
					stringBuilder.Append(Texts.Instance.GetText(Enum.GetName(typeof(Enums.NodeGround), _node.nodeData.NodeGround)));
				}
				GroundText.text = stringBuilder.ToString();
				if (_node.GetNodeAction() == "combat")
				{
					Ground.localPosition = new Vector3(Ground.localPosition.x, -1.32f, Ground.localPosition.z);
				}
				else
				{
					Ground.localPosition = new Vector3(Ground.localPosition.x, -1.02f, Ground.localPosition.z);
				}
			}
			else if (Ground.gameObject.activeSelf)
			{
				Ground.gameObject.SetActive(value: false);
			}
			WriteTitle(1, text2, shader, spriteMap);
		}
		popupDone = true;
		ShowAction();
	}

	public void Show(Node _node)
	{
		DoPopup(_node);
	}

	private void ShowAction()
	{
		base.gameObject.SetActive(value: true);
		show = true;
		Vector3 ori = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		base.transform.localPosition = CalcDestination(ori);
	}

	public void Hide()
	{
		show = false;
		popupDone = false;
		base.gameObject.SetActive(value: false);
	}
}
