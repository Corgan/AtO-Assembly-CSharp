using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
	public NodeData nodeData;

	public SpriteRenderer background;

	public Transform plainT;

	public Transform currentT;

	public Transform blockedT;

	public Transform availableT;

	public Transform visitedT;

	public Transform chestT;

	public Transform requisiteLayer;

	public Transform requisite;

	public SpriteRenderer requisiteSprite;

	public SpriteRenderer requisiteSpriteShadow;

	public Transform requisite2;

	public SpriteRenderer requisite2Sprite;

	public SpriteRenderer requisite2SpriteShadow;

	public Transform requisite3;

	public SpriteRenderer requisite3Sprite;

	public SpriteRenderer requisite3SpriteShadow;

	public SpriteRenderer nodeImage;

	public SpriteRenderer nodeDecor;

	public Sprite nodeImageTown;

	public Sprite nodeImageCombat;

	public Sprite nodeImageCombatRare;

	public Sprite nodeImageFinalBoss;

	public Sprite nodeImageEvent;

	public Transform nodeImageParticlesT;

	private ParticleSystem nodeImageParticlesSystem;

	private SpriteRenderer availableSR;

	private SpriteRenderer plainSR;

	private Animator anim;

	private bool highlightedNode;

	private Color colorReset = new Color(1f, 1f, 1f, 1f);

	private Color colorAvailable = new Color(0.27f, 0.85f, 0.27f);

	private Color colorAvailableOff = new Color(1f, 0.46f, 0.82f, 0.85f);

	private int nodeNumeral;

	public string action;

	public string actionId;

	public Transform playersMarkT;

	public Transform[] playersMark;

	public SpriteRenderer[] playersMarkSpr;

	private void Awake()
	{
		plainSR = plainT.GetComponent<SpriteRenderer>();
		availableSR = availableT.GetComponent<SpriteRenderer>();
		nodeImageParticlesSystem = nodeImageParticlesT.GetComponent<ParticleSystem>();
		anim = nodeImage.transform.GetComponent<Animator>();
		requisiteLayer.gameObject.SetActive(value: false);
		requisite.gameObject.SetActive(value: false);
		requisite2.gameObject.SetActive(value: false);
		requisite3.gameObject.SetActive(value: false);
	}

	private void StartNode()
	{
		nodeDecor.sprite = null;
		chestT.gameObject.SetActive(value: false);
		if (plainSR == null)
		{
			plainSR = plainT.GetComponent<SpriteRenderer>();
			availableSR = availableT.GetComponent<SpriteRenderer>();
			anim = nodeImage.transform.GetComponent<Animator>();
		}
	}

	public void InitNode()
	{
		StartNode();
		if (nodeData != null)
		{
			string text = nodeData.NodeId.ToLower();
			base.gameObject.name = text;
			nodeNumeral = int.Parse(text.Split('_')[1]);
		}
	}

	public bool Exists()
	{
		if (GetNodeAssignedId() == "")
		{
			return false;
		}
		return true;
	}

	public string GetNodeAction()
	{
		if (AtOManager.Instance.gameNodeAssigned.ContainsKey(nodeData.NodeId))
		{
			return AtOManager.Instance.gameNodeAssigned[nodeData.NodeId].Split(':')[0];
		}
		return "";
	}

	public string GetNodeAssignedId()
	{
		if (AtOManager.Instance.gameNodeAssigned != null && nodeData != null && AtOManager.Instance.gameNodeAssigned.ContainsKey(nodeData.NodeId))
		{
			string[] array = AtOManager.Instance.gameNodeAssigned[nodeData.NodeId].Split(':');
			if (array != null && array.Length > 1)
			{
				return array[1];
			}
			return "";
		}
		return "";
	}

	public void AssignNode()
	{
		chestT.gameObject.SetActive(value: false);
		bool flag = false;
		if (!(nodeData != null))
		{
			return;
		}
		action = GetNodeAction();
		actionId = GetNodeAssignedId();
		if (actionId == "")
		{
			SetHidden();
			return;
		}
		if (nodeData.NodeId == "of1_10" || nodeData.NodeId == "of2_10")
		{
			flag = true;
		}
		if (nodeData.GoToTown)
		{
			nodeImage.sprite = nodeImageTown;
		}
		else if (action == "combat")
		{
			if (GameManager.Instance.IsObeliskChallenge() && (AtOManager.Instance.NodeHaveBossRare(nodeData.NodeId) || flag))
			{
				if (nodeImageParticlesT != null)
				{
					nodeImageParticlesT.gameObject.SetActive(value: true);
					if (nodeImageParticlesSystem == null)
					{
						nodeImageParticlesSystem = nodeImageParticlesT.GetComponent<ParticleSystem>();
					}
					if (nodeImageParticlesSystem != null)
					{
						ParticleSystem.MainModule main = nodeImageParticlesSystem.main;
						if (nodeData.NodeCombatTier == Enums.CombatTier.T8 || nodeData.NodeCombatTier == Enums.CombatTier.T9 || flag)
						{
							main.startColor = new Color(1f, 0.1f, 0.15f);
						}
						else
						{
							main.startColor = new Color(0.1f, 0.6f, 1f);
						}
					}
				}
				if (nodeData.NodeCombatTier == Enums.CombatTier.T8 || nodeData.NodeCombatTier == Enums.CombatTier.T9 || flag)
				{
					nodeImage.sprite = nodeImageFinalBoss;
				}
				else
				{
					nodeImage.sprite = nodeImageCombatRare;
					chestT.gameObject.SetActive(value: true);
				}
			}
			else
			{
				nodeImage.sprite = nodeImageCombat;
			}
		}
		else if (action == "event")
		{
			EventData eventData = Globals.Instance.GetEventData(actionId);
			if (eventData != null && eventData.EventSpriteMap != null)
			{
				nodeImage.sprite = eventData.EventSpriteMap;
			}
			else
			{
				nodeImage.sprite = nodeImageEvent;
			}
			if (eventData != null && eventData.EventSpriteDecor != null)
			{
				nodeDecor.sprite = eventData.EventSpriteDecor;
			}
			if (eventData != null && eventData.EventIconShader != Enums.MapIconShader.None)
			{
				_ = anim != null;
				if (nodeImageParticlesT != null)
				{
					nodeImageParticlesT.gameObject.SetActive(value: true);
					if (nodeImageParticlesSystem == null)
					{
						nodeImageParticlesSystem = nodeImageParticlesT.GetComponent<ParticleSystem>();
					}
					if (nodeImageParticlesSystem != null)
					{
						ParticleSystem.MainModule main2 = nodeImageParticlesSystem.main;
						if (eventData.EventIconShader == Enums.MapIconShader.Green)
						{
							main2.startColor = Color.green;
						}
						else if (eventData.EventIconShader == Enums.MapIconShader.Blue)
						{
							main2.startColor = new Color(0.1f, 0.6f, 1f);
						}
						else if (eventData.EventIconShader == Enums.MapIconShader.Purple)
						{
							main2.startColor = new Color(0.91f, 0.05f, 0.87f);
						}
						else if (eventData.EventIconShader == Enums.MapIconShader.Orange)
						{
							main2.startColor = new Color(1f, 0.69f, 0f);
						}
						else if (eventData.EventIconShader == Enums.MapIconShader.Red)
						{
							main2.startColor = Color.red;
						}
						else if (eventData.EventIconShader == Enums.MapIconShader.Black)
						{
							main2.startColor = Color.black;
						}
					}
				}
			}
			else if (nodeImageParticlesT != null)
			{
				nodeImageParticlesT.gameObject.SetActive(value: false);
			}
			requisiteLayer.gameObject.SetActive(value: false);
			requisite.gameObject.SetActive(value: false);
			requisite2.gameObject.SetActive(value: false);
			requisite3.gameObject.SetActive(value: false);
			if (!currentT.gameObject.activeSelf && !blockedT.gameObject.activeSelf && eventData != null)
			{
				bool flag2 = false;
				bool flag3 = false;
				for (int i = 0; i < eventData.Replys.Length; i++)
				{
					EventReplyData eventReplyData = eventData.Replys[i];
					if (!(eventReplyData.Requirement != null) || !(eventReplyData.Requirement.ItemSprite != null) || !AtOManager.Instance.PlayerHasRequirement(eventReplyData.Requirement))
					{
						continue;
					}
					if (!flag2)
					{
						requisiteSprite.GetComponent<EventItemTrack>().SetItemTrack(Globals.Instance.GetRequirementData(eventReplyData.Requirement.RequirementId));
						SpriteRenderer spriteRenderer = requisiteSprite;
						Sprite sprite = (requisiteSpriteShadow.sprite = eventReplyData.Requirement.ItemSprite);
						spriteRenderer.sprite = sprite;
						requisite.gameObject.SetActive(value: true);
						flag2 = true;
					}
					else if (!flag3)
					{
						if (eventReplyData.Requirement.ItemSprite != requisiteSprite.sprite)
						{
							requisite2Sprite.GetComponent<EventItemTrack>().SetItemTrack(Globals.Instance.GetRequirementData(eventReplyData.Requirement.RequirementId));
							SpriteRenderer spriteRenderer2 = requisite2Sprite;
							Sprite sprite = (requisite2SpriteShadow.sprite = eventReplyData.Requirement.ItemSprite);
							spriteRenderer2.sprite = sprite;
							requisite2.gameObject.SetActive(value: true);
							flag3 = true;
						}
					}
					else if (eventReplyData.Requirement.ItemSprite != requisiteSprite.sprite && eventReplyData.Requirement.ItemSprite != requisite2Sprite.sprite)
					{
						requisite3Sprite.GetComponent<EventItemTrack>().SetItemTrack(Globals.Instance.GetRequirementData(eventReplyData.Requirement.RequirementId));
						SpriteRenderer spriteRenderer3 = requisite3Sprite;
						Sprite sprite = (requisite3SpriteShadow.sprite = eventReplyData.Requirement.ItemSprite);
						spriteRenderer3.sprite = sprite;
						requisite3.gameObject.SetActive(value: true);
					}
					if (requisite.gameObject.activeSelf || requisite2.gameObject.activeSelf || requisite3.gameObject.activeSelf)
					{
						requisiteLayer.gameObject.SetActive(value: true);
					}
				}
			}
		}
		AssignBackground();
	}

	public void AssignBackground()
	{
		if (nodeData.NodeBackgroundImg != null)
		{
			background.sprite = nodeData.NodeBackgroundImg;
		}
	}

	public IEnumerator ShowNode()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (base.transform.GetChild(i).gameObject.name.ToLower() == "mappiece")
			{
				yield break;
			}
		}
		base.transform.localScale = Vector3.zero;
		yield return Globals.Instance.WaitForSeconds(0.2f);
		float size = 0f;
		float increment = 0.06f;
		float delay = 0.01f;
		while (size < 1.2f)
		{
			size += increment;
			base.transform.localScale = new Vector3(size, size, 1f);
			yield return Globals.Instance.WaitForSeconds(delay);
		}
		while (size > 1f)
		{
			size -= increment;
			base.transform.localScale = new Vector3(size, size, 1f);
			yield return Globals.Instance.WaitForSeconds(delay);
		}
		base.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	public void SetHidden()
	{
		base.gameObject.SetActive(value: false);
	}

	public void SetPlain()
	{
		if (Exists())
		{
			HideStates();
			plainT.gameObject.SetActive(value: true);
			nodeImage.transform.gameObject.SetActive(value: true);
			plainSR.color = colorReset;
		}
		else
		{
			SetHidden();
		}
	}

	public void SetAvailable()
	{
		if (Exists())
		{
			plainT.gameObject.SetActive(value: false);
			HideStates();
			availableT.gameObject.SetActive(value: true);
			if (anim != null)
			{
				anim.enabled = true;
			}
			if (base.gameObject.activeSelf && base.transform.parent.transform.parent.gameObject.activeSelf)
			{
				StartCoroutine(ShowNode());
			}
		}
		else
		{
			SetHidden();
		}
	}

	public void SetActive()
	{
		if (Exists())
		{
			plainT.gameObject.SetActive(value: false);
			nodeImage.transform.gameObject.SetActive(value: false);
			HideStates();
			currentT.gameObject.SetActive(value: true);
			requisiteLayer.gameObject.SetActive(value: false);
			requisite.gameObject.SetActive(value: false);
			requisite2.gameObject.SetActive(value: false);
			requisite3.gameObject.SetActive(value: false);
		}
		else
		{
			SetHidden();
		}
	}

	public void SetVisited()
	{
		if (Exists())
		{
			plainT.gameObject.SetActive(value: false);
			nodeImage.transform.gameObject.SetActive(value: false);
			HideStates();
			visitedT.gameObject.SetActive(value: true);
			requisiteLayer.gameObject.SetActive(value: false);
			requisite.gameObject.SetActive(value: false);
			requisite2.gameObject.SetActive(value: false);
			requisite3.gameObject.SetActive(value: false);
		}
		else
		{
			SetHidden();
		}
	}

	public void SetBlocked()
	{
		if (Exists())
		{
			plainT.gameObject.SetActive(value: false);
			nodeImage.transform.gameObject.SetActive(value: false);
			HideStates();
			blockedT.gameObject.SetActive(value: true);
			requisiteLayer.gameObject.SetActive(value: false);
			requisite.gameObject.SetActive(value: false);
			requisite2.gameObject.SetActive(value: false);
			requisite3.gameObject.SetActive(value: false);
		}
		else
		{
			SetHidden();
		}
	}

	private void HideStates()
	{
		availableT.gameObject.SetActive(value: false);
		blockedT.gameObject.SetActive(value: false);
		currentT.gameObject.SetActive(value: false);
		visitedT.gameObject.SetActive(value: false);
	}

	public void HighlightNode(bool status)
	{
		MapManager.Instance.DrawArrow(MapManager.Instance.nodeActive, this, status);
		highlightedNode = status;
		if (status)
		{
			SpriteRenderer spriteRenderer = availableSR;
			Color color = (plainSR.color = colorAvailable);
			spriteRenderer.color = color;
		}
		else
		{
			SpriteRenderer spriteRenderer2 = availableSR;
			Color color = (plainSR.color = colorReset);
			spriteRenderer2.color = color;
		}
	}

	private void HoverNode(bool status)
	{
		if (status)
		{
			if (plainT.gameObject.activeSelf)
			{
				plainSR.color = colorAvailableOff;
			}
		}
		else if (plainT.gameObject.activeSelf)
		{
			plainSR.color = colorReset;
		}
	}

	public void ShowSelectedNode(string _nick)
	{
		for (int i = 0; i < playersMark.Length; i++)
		{
			if (!playersMark[i].gameObject.activeSelf)
			{
				playersMark[i].gameObject.SetActive(value: true);
				playersMarkSpr[i].color = Functions.HexToColor(NetworkManager.Instance.GetColorFromNick(_nick));
				playersMarkT.gameObject.SetActive(value: true);
				break;
			}
		}
	}

	public void ClearSelectedNode()
	{
		for (int i = 0; i < playersMark.Length; i++)
		{
			playersMark[i].gameObject.SetActive(value: false);
		}
		playersMarkT.gameObject.SetActive(value: false);
	}

	public void OnMouseUp()
	{
		if (Functions.ClickedThisTransform(base.transform) && !AlertManager.Instance.IsActive() && !GameManager.Instance.IsTutorialActive() && !SettingsManager.Instance.IsActive() && !DamageMeterManager.Instance.IsActive() && (!MapManager.Instance || !MapManager.Instance.IsCharacterUnlock()) && (!MapManager.Instance || (!MapManager.Instance.IsCorruptionOver() && !MapManager.Instance.IsConflictOver() && !MapManager.Instance.characterWindow.IsActive())) && (!MapManager.Instance || !MapManager.Instance.selectedNode) && !EventManager.Instance)
		{
			GameManager.Instance.SetCursorPlain();
			MapManager.Instance.HidePopup();
			if (MapManager.Instance.CanTravelToThisNode(this))
			{
				MapManager.Instance.PlayerSelectedNode(this);
			}
			GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
		}
	}

	private void OnMouseOver()
	{
		if (GameManager.Instance.GetDeveloperMode() && !AlertManager.Instance.IsActive() && !GameManager.Instance.IsTutorialActive() && !SettingsManager.Instance.IsActive() && !DamageMeterManager.Instance.IsActive() && (!MapManager.Instance || !MapManager.Instance.IsCharacterUnlock()) && (!MapManager.Instance || (!MapManager.Instance.IsCorruptionOver() && !MapManager.Instance.IsConflictOver() && !MapManager.Instance.characterWindow.IsActive())) && !EventManager.Instance && !EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonUp(1) && (!GameManager.Instance.IsMultiplayer() || (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())))
		{
			MapManager.Instance.TravelToThisNode(this);
		}
	}

	private void OnMouseEnter()
	{
		if (!AlertManager.Instance.IsActive() && !GameManager.Instance.IsTutorialActive() && !SettingsManager.Instance.IsActive() && !DamageMeterManager.Instance.IsActive() && (!MapManager.Instance || !MapManager.Instance.IsCharacterUnlock()) && (!MapManager.Instance || (!MapManager.Instance.IsCorruptionOver() && !MapManager.Instance.IsConflictOver() && !MapManager.Instance.characterWindow.IsActive())) && !EventManager.Instance && !EventSystem.current.IsPointerOverGameObject() && GetNodeAssignedId() != "")
		{
			MapManager.Instance.ShowPopup(this);
			GameManager.Instance.PlayLibraryAudio("ui_click");
			if (MapManager.Instance.CanTravelToThisNode(this))
			{
				GameManager.Instance.SetCursorHover();
				HighlightNode(status: true);
			}
			else
			{
				HoverNode(status: true);
			}
			if (MapManager.Instance.nodeActive != this && (!(nodeData.NodeId == "sen_41") || (!(AtOManager.Instance.currentMapNode == "tutorial_0") && !(AtOManager.Instance.currentMapNode == "tutorial_1") && !(AtOManager.Instance.currentMapNode == "tutorial_2"))))
			{
				MapManager.Instance.DrawArrowsTemp(this);
			}
		}
	}

	public void OnMouseExit()
	{
		if (AlertManager.Instance.IsActive() || (bool)EventManager.Instance)
		{
			return;
		}
		GameManager.Instance.SetCursorPlain();
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			MapManager.Instance.HidePopup();
			if (highlightedNode)
			{
				HighlightNode(status: false);
			}
			else
			{
				HoverNode(status: false);
			}
			if (MapManager.Instance.nodeActive != this)
			{
				MapManager.Instance.HideArrowsTemp(this);
			}
		}
	}
}
