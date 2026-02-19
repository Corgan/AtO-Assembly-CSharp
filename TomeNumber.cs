using TMPro;
using UnityEngine;

public class TomeNumber : MonoBehaviour
{
	public Transform backgroundT;

	public Transform activeT;

	public SpriteRenderer background;

	public SpriteRenderer border;

	public TMP_Text numberTxt;

	private string number;

	private Color colorActive;

	private Color colorDeactive;

	private bool active;

	private bool visible;

	private Vector3 positionOri;

	private Vector3 positionShow;

	private Vector3 positionHide;

	private Vector3 positionActiveVector = new Vector3(0.2f, 0f, 0f);

	private void Awake()
	{
		colorActive = Functions.HexToColor("#FFA400");
		colorDeactive = new Color(1f, 1f, 1f, 1f);
	}

	public void Activate()
	{
		active = true;
		activeT.gameObject.SetActive(value: true);
		border.color = colorActive;
		background.color = colorActive;
		base.transform.localPosition = base.transform.localPosition - positionActiveVector;
	}

	public void Deactivate()
	{
		active = false;
		background.color = colorDeactive;
		activeT.gameObject.SetActive(value: false);
		base.transform.localPosition = positionOri;
	}

	public bool IsActive()
	{
		return active;
	}

	public void Init(int _number)
	{
		number = _number.ToString();
		numberTxt.text = number;
		positionOri = base.transform.localPosition;
		positionShow = new Vector3(positionOri.x, base.transform.localPosition.y, 0f);
		positionHide = new Vector3(positionOri.x + 1f, base.transform.localPosition.y, 100f);
	}

	public void SetText(string _text)
	{
		numberTxt.text = _text;
	}

	public void Show()
	{
		if (base.gameObject.activeSelf && base.transform.parent.gameObject.activeSelf)
		{
			base.transform.localPosition = positionShow;
			visible = true;
		}
	}

	public void Hide()
	{
		if (base.gameObject.activeSelf && base.transform.parent.gameObject.activeSelf)
		{
			base.transform.localPosition = positionHide;
			visible = false;
		}
	}

	public bool IsVisible()
	{
		return visible;
	}

	private void OnMouseEnter()
	{
		if (!active)
		{
			border.color = Functions.HexToColor("#BBBBBB");
			activeT.gameObject.SetActive(value: true);
			GameManager.Instance.SetCursorHover();
		}
	}

	private void OnMouseExit()
	{
		if (!active)
		{
			activeT.gameObject.SetActive(value: false);
		}
		GameManager.Instance.SetCursorPlain();
	}

	public void OnMouseUp()
	{
		if (Functions.ClickedThisTransform(base.transform))
		{
			TomeManager.Instance.SetPage(int.Parse(number));
		}
	}
}
