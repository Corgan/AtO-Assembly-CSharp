using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InitiativePortrait : MonoBehaviour
{
	public Hero Hero;

	public NPCData NpcData;

	public string id;

	public HeroItem heroItem;

	public NPCItem npcItem;

	public int portraitPosition;

	public int portraitElements;

	public Transform activeTransform;

	public TMP_Text speedTM;

	public SpriteRenderer charSprite;

	public Transform playing;

	private SpriteRenderer spriteRenderer;

	private Renderer[] childRenderers;

	private bool isHero;

	private bool isNPC;

	private int position = -1;

	public Vector3 destinationLocalPosition = Vector3.zero;

	private Coroutine movePortraitCoroutine;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		childRenderers = new Renderer[base.transform.childCount];
		int num = 0;
		foreach (Transform item in base.transform)
		{
			Renderer component = item.GetComponent<Renderer>();
			childRenderers[num] = component;
			num++;
		}
	}

	private void MovePortrait()
	{
		if (movePortraitCoroutine != null)
		{
			StopCoroutine(movePortraitCoroutine);
		}
		movePortraitCoroutine = StartCoroutine(MovePortraitCo());
	}

	private IEnumerator MovePortraitCo()
	{
		while (base.transform.localPosition != destinationLocalPosition)
		{
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, destinationLocalPosition, Time.deltaTime * 8f);
			if (Mathf.Abs(base.transform.localPosition.x - destinationLocalPosition.x) < 0.01f)
			{
				base.transform.localPosition = destinationLocalPosition;
			}
			yield return null;
		}
		SortingOrder();
	}

	public void AdjustForRoundSeparator()
	{
		destinationLocalPosition += new Vector3(0.35999998f, 0f, base.transform.localPosition.z);
		MovePortrait();
	}

	private void SortingOrder()
	{
		int num = 10;
		for (int i = 0; i < childRenderers.Length; i++)
		{
			childRenderers[i].sortingOrder = position * 10 + num;
			num--;
		}
	}

	public void Init(int _position)
	{
		if (Hero != null)
		{
			charSprite.sprite = Hero.SpriteSpeed;
		}
		else if (NpcData != null)
		{
			charSprite.sprite = (npcItem.NPC.TransformedModel ? NpcData.SpriteSpeedAlternate : NpcData.SpriteSpeed);
		}
		position = _position;
		Vector3 vector = CalcVectorPosition(position);
		if (destinationLocalPosition == Vector3.zero)
		{
			base.transform.localPosition = (destinationLocalPosition = vector);
		}
		else
		{
			destinationLocalPosition = vector;
		}
		SortingOrder();
		activeTransform.gameObject.SetActive(value: false);
	}

	public void SetPlaying(bool state)
	{
		playing.gameObject.SetActive(state);
	}

	public void SetActive(bool state)
	{
		activeTransform.gameObject.SetActive(state);
	}

	public int GetPos()
	{
		return position;
	}

	public void RedoPos(int _position, bool adjust)
	{
		destinationLocalPosition = CalcVectorPosition(_position);
		if (adjust)
		{
			destinationLocalPosition += new Vector3(0.35999998f, 0f, base.transform.localPosition.z);
		}
		position = _position;
		MovePortrait();
		SortingOrder();
	}

	private Vector3 CalcVectorPosition(int _position)
	{
		return new Vector3((float)_position * 0.48f + (float)_position * 0.24f, 0f, 0f);
	}

	public void SetSpeed(int[] speed)
	{
		string text = speed[0].ToString();
		int num = speed[2];
		Color color = new Color(1f, 1f, 1f, 1f);
		if (num > 0)
		{
			ColorUtility.TryParseHtmlString(Globals.Instance.ClassColor["scout"], out color);
		}
		else if (num < 0)
		{
			ColorUtility.TryParseHtmlString(Globals.Instance.ClassColor["warrior"], out color);
		}
		speedTM.color = color;
		speedTM.text = text;
	}

	public void SetHero(Hero _hero, HeroItem _heroItem)
	{
		Hero = _hero;
		heroItem = _heroItem;
		NpcData = null;
		npcItem = null;
		isHero = true;
	}

	public void SetNPC(NPCData _npcData, NPCItem _npcItem)
	{
		Hero = null;
		heroItem = null;
		NpcData = _npcData;
		npcItem = _npcItem;
		isNPC = true;
	}

	public void OnMouseUp()
	{
		if ((!MatchManager.Instance || !MatchManager.Instance.console.IsActive()) && !EventSystem.current.IsPointerOverGameObject())
		{
			GameManager.Instance.SetCursorPlain();
			int num = 0;
			num = ((!isHero) ? npcItem.NPC.NPCIndex : Hero.HeroIndex);
			MatchManager.Instance.ShowCharacterWindow("stats", isHero, num);
			MatchManager.Instance.combatTarget.ClearTarget();
		}
	}

	private void OnMouseEnter()
	{
		if (!AlertManager.Instance.IsActive() && !SettingsManager.Instance.IsActive() && (!MatchManager.Instance || !MatchManager.Instance.console.IsActive()) && !EventSystem.current.IsPointerOverGameObject())
		{
			if (isHero)
			{
				heroItem.OutlineGray();
				MatchManager.Instance.combatTarget.SetTargetTMP(Hero);
			}
			else if (isNPC)
			{
				npcItem.OutlineGray();
				MatchManager.Instance.combatTarget.SetTargetTMP(npcItem.NPC);
			}
			GameManager.Instance.SetCursorHover();
			SetActive(state: true);
		}
	}

	private void OnMouseExit()
	{
		if (!EventSystem.current.IsPointerOverGameObject())
		{
			if (MatchManager.Instance.CardDrag)
			{
				MatchManager.Instance.SetGlobalOutlines(state: true, MatchManager.Instance.CardActive);
			}
			else if (isHero)
			{
				heroItem.OutlineHide();
			}
			else if (isNPC)
			{
				npcItem.OutlineHide();
			}
			GameManager.Instance.SetCursorPlain();
			MatchManager.Instance.combatTarget.ClearTarget();
		}
		SetActive(state: false);
	}
}
