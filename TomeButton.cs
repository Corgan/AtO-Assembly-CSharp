using TMPro;
using UnityEngine;

public class TomeButton : MonoBehaviour
{
	public int tomeClass;

	private TMP_Text buttonTxt;

	private Transform border;

	private SpriteRenderer borderSpr;

	private Transform background;

	private SpriteRenderer backgroundSpr;

	private Color colorActive;

	private Color colorDefault = new Color(0.8f, 0.8f, 0.8f, 0.8f);

	private bool active;

	private Vector3 oriPosition;

	private float textSizeOri;

	private void Awake()
	{
		background = base.transform.GetChild(0).transform;
		backgroundSpr = background.GetComponent<SpriteRenderer>();
		border = background.GetChild(0).transform;
		borderSpr = border.GetComponent<SpriteRenderer>();
		colorActive = Functions.HexToColor("#DD5F07");
		buttonTxt = base.transform.GetChild(1).GetComponent<TMP_Text>();
		textSizeOri = buttonTxt.fontSize;
	}

	public void Init()
	{
	}

	private void Start()
	{
		if (tomeClass == -1)
		{
			buttonTxt.text = Texts.Instance.GetText("allcards");
			backgroundSpr.color = Functions.HexToColor("#FFCC00");
		}
		else if (tomeClass == 0)
		{
			buttonTxt.text = "<sprite name=slashing>";
			backgroundSpr.color = Functions.HexToColor(Globals.Instance.ClassColor["warrior"]);
		}
		else if (tomeClass == 1)
		{
			buttonTxt.text = "<sprite name=fire>";
			backgroundSpr.color = Functions.HexToColor(Globals.Instance.ClassColor["mage"]);
		}
		else if (tomeClass == 2)
		{
			buttonTxt.text = "<sprite name=heal>";
			backgroundSpr.color = Functions.HexToColor(Globals.Instance.ClassColor["healer"]);
		}
		else if (tomeClass == 3)
		{
			buttonTxt.text = "<sprite name=piercing>";
			backgroundSpr.color = Functions.HexToColor(Globals.Instance.ClassColor["scout"]);
		}
		else if (tomeClass == 4)
		{
			buttonTxt.text = "<sprite name=slash>";
			backgroundSpr.color = Functions.HexToColor(Globals.Instance.ClassColor["magicknight"]);
		}
		else if (tomeClass == 5)
		{
			buttonTxt.text = "<sprite name=bless>";
			backgroundSpr.color = Functions.HexToColor(Globals.Instance.ClassColor["boon"]);
		}
		else if (tomeClass == 6)
		{
			buttonTxt.text = "<sprite name=bleed>";
			backgroundSpr.color = Functions.HexToColor(Globals.Instance.ClassColor["injury"]);
		}
		else if (tomeClass == 7)
		{
			buttonTxt.text = "<sprite name=weapon>";
			backgroundSpr.color = Functions.HexToColor(Globals.Instance.ClassColor["item"]);
		}
		else if (tomeClass == 8)
		{
			buttonTxt.text = "<sprite name=armor>";
			backgroundSpr.color = Functions.HexToColor(Globals.Instance.ClassColor["item"]);
		}
		else if (tomeClass == 9)
		{
			buttonTxt.text = "<sprite name=jewelry>";
			backgroundSpr.color = Functions.HexToColor(Globals.Instance.ClassColor["item"]);
		}
		else if (tomeClass == 10)
		{
			buttonTxt.text = "<sprite name=accesory>";
			backgroundSpr.color = Functions.HexToColor(Globals.Instance.ClassColor["item"]);
		}
		else if (tomeClass == 11)
		{
			buttonTxt.text = "<sprite name=pet>";
			backgroundSpr.color = Functions.HexToColor(Globals.Instance.ClassColor["pet"]);
		}
		else if (tomeClass == 14)
		{
			buttonTxt.text = Texts.Instance.GetText("global");
		}
		else if (tomeClass == 15)
		{
			buttonTxt.text = Texts.Instance.GetText("friends");
		}
		else if (tomeClass == 16)
		{
			buttonTxt.text = "<sprite name=pathMap>";
			backgroundSpr.color = Functions.HexToColor("#FF9800");
		}
		else if (tomeClass != 17 && tomeClass != 18 && tomeClass != 19 && tomeClass != 20)
		{
			if (tomeClass == 21)
			{
				buttonTxt.text = Texts.Instance.GetText("combatStats");
			}
			else if (tomeClass == 22)
			{
				buttonTxt.text = "<sprite name=experience>";
				backgroundSpr.color = Functions.HexToColor("#FF9B00");
			}
			else if (tomeClass == 23)
			{
				buttonTxt.text = Texts.Instance.GetText("index");
				backgroundSpr.color = Functions.HexToColor("#76736F");
			}
		}
	}

	public void SetText(string _text)
	{
		buttonTxt.text = _text;
	}

	public void SetColor(string _colorHex)
	{
		backgroundSpr.color = Functions.HexToColor(_colorHex);
	}

	public void Activate()
	{
		_ = oriPosition;
		if (oriPosition == Vector3.zero)
		{
			oriPosition = base.transform.localPosition;
		}
		base.transform.localPosition = oriPosition + new Vector3(0f, 0.1f, 0f);
		active = true;
		if (borderSpr != null)
		{
			borderSpr.color = colorActive;
		}
		if (border != null)
		{
			border.gameObject.SetActive(value: true);
		}
		if ((bool)buttonTxt)
		{
			buttonTxt.transform.localPosition = new Vector3(buttonTxt.transform.localPosition.x, 0.04f, buttonTxt.transform.localPosition.z);
			buttonTxt.fontSize = textSizeOri + 0.5f;
		}
	}

	public void Deactivate()
	{
		_ = oriPosition;
		if (oriPosition == Vector3.zero)
		{
			oriPosition = base.transform.localPosition;
		}
		base.transform.localPosition = oriPosition;
		active = false;
		if (borderSpr != null)
		{
			borderSpr.color = colorDefault;
		}
		if (border != null)
		{
			border.gameObject.SetActive(value: false);
		}
		if ((bool)buttonTxt)
		{
			buttonTxt.transform.localPosition = new Vector3(buttonTxt.transform.localPosition.x, 0.08f, buttonTxt.transform.localPosition.z);
			buttonTxt.fontSize = textSizeOri;
		}
	}

	public void OnMouseUp()
	{
		if (Functions.ClickedThisTransform(base.transform))
		{
			if (tomeClass == 14 || tomeClass == 15)
			{
				TomeManager.Instance.SelectTomeScores(tomeClass);
			}
			else if (tomeClass >= 16 && tomeClass <= 20)
			{
				TomeManager.Instance.RunDetailButton(tomeClass - 16);
			}
			else if (tomeClass == 21)
			{
				TomeManager.Instance.RunCombatStats();
			}
			else if (tomeClass == 23)
			{
				TomeManager.Instance.SetPage(0);
			}
			else
			{
				TomeManager.Instance.SelectTomeCards(tomeClass);
			}
		}
	}

	private void OnMouseExit()
	{
		if (!active && border.gameObject.activeSelf)
		{
			border.gameObject.SetActive(value: false);
		}
		GameManager.Instance.SetCursorPlain();
	}

	private void OnMouseEnter()
	{
		if (!active)
		{
			borderSpr.color = colorDefault;
			if (!border.gameObject.activeSelf)
			{
				border.gameObject.SetActive(value: true);
			}
			GameManager.Instance.SetCursorHover();
		}
	}
}
