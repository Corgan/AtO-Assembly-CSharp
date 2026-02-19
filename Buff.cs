using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class Buff : MonoBehaviour
{
	public string buffId = "";

	public AuraCurseData acData;

	public SpriteRenderer spriteSR;

	public SpriteRenderer spriteSRShadow;

	public TMP_Text chargesTM;

	private MeshRenderer chargesTMMesh;

	public Transform spriteAnim;

	private Animator spriteAnimator;

	private TMP_Text chargesAnimTM;

	private int charges;

	public string buffStatus = "";

	public SpriteRenderer bgIconSprite;

	private bool auraImmunity;

	private Color colorAura = new Color(0f, 0.26f, 1f, 1f);

	private Color colorCurse = new Color(0.81f, 0.04f, 0.72f, 1f);

	private Color colorOver = new Color(0.6f, 0.6f, 0.6f, 1f);

	private Coroutine AmplifyCo;

	private string charId = "";

	private void Awake()
	{
		chargesAnimTM = spriteAnim.GetChild(0).transform.GetComponent<TMP_Text>();
		chargesTMMesh = chargesTM.GetComponent<MeshRenderer>();
		spriteAnimator = spriteAnim.GetComponent<Animator>();
		spriteAnimator.enabled = false;
	}

	public void DisplayBecauseCard(bool _status)
	{
		if (base.gameObject.activeSelf)
		{
			if (_status)
			{
				base.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
				bgIconSprite.color = new Color(1f, 0.69f, 0f, 1f);
			}
			else
			{
				base.transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
	}

	public void RestoreBecauseCard()
	{
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		RestoreColor();
	}

	public void CleanBuff()
	{
		if (base.gameObject != null)
		{
			buffStatus = "";
			base.gameObject.name = "";
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(value: false);
			}
		}
	}

	public void SetBuffInStats(bool _auraImmunity = false)
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
		}
		auraImmunity = _auraImmunity;
		spriteSR.sortingOrder = 1001;
		spriteSR.sortingLayerName = "UI";
		if (spriteSRShadow.transform.gameObject.activeSelf)
		{
			spriteSRShadow.gameObject.SetActive(value: false);
		}
		if (auraImmunity)
		{
			if (chargesTM.gameObject.activeSelf)
			{
				chargesTM.gameObject.SetActive(value: false);
			}
		}
		else
		{
			if (!chargesTM.gameObject.activeSelf)
			{
				chargesTM.gameObject.SetActive(value: true);
			}
			chargesTMMesh.sortingOrder = 1005;
			chargesTMMesh.sortingLayerName = "UI";
		}
		base.transform.localScale = new Vector3(1.7f, 1.7f, 1f);
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, -5f);
	}

	public void SetBuffInStatsCharges()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
		}
		spriteSR.sortingOrder = 1601;
		spriteSR.sortingLayerName = "UI";
		if (spriteSRShadow.gameObject.activeSelf)
		{
			spriteSRShadow.gameObject.SetActive(value: false);
		}
		if (!chargesTM.gameObject.activeSelf)
		{
			chargesTM.gameObject.SetActive(value: true);
		}
		chargesTMMesh.sortingOrder = 1605;
		chargesTMMesh.sortingLayerName = "UI";
		chargesTM.transform.localPosition = new Vector3(0f, chargesTM.transform.localPosition.y, chargesTM.transform.localPosition.z);
		base.transform.localScale = new Vector3(1.7f, 1.7f, 1f);
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, -5f);
	}

	public void SetBuff(AuraCurseData _acData, int _charges, string _chargesStr = "", string _charId = "")
	{
		charId = _charId;
		if (spriteSR == null)
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(value: false);
			}
		}
		else if (_acData != null)
		{
			acData = _acData;
			SpriteRenderer spriteRenderer = spriteSR;
			Sprite sprite = (spriteSRShadow.sprite = _acData.Sprite);
			spriteRenderer.sprite = sprite;
			buffId = _acData.Id.ToLower();
			if (_charges == 0 && _chargesStr == "")
			{
				buffId = "";
				if (base.gameObject.activeSelf)
				{
					base.gameObject.SetActive(value: false);
				}
				return;
			}
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(value: true);
			}
			RestoreColor();
			if (_chargesStr == "")
			{
				chargesTM.text = _charges.ToString();
			}
			else
			{
				chargesTM.text = _chargesStr;
			}
			charges = _charges;
			if (!spriteSR.transform.gameObject.activeSelf)
			{
				spriteSR.transform.gameObject.SetActive(value: true);
			}
			if (!chargesTM.transform.gameObject.activeSelf)
			{
				chargesTM.transform.gameObject.SetActive(value: true);
			}
		}
		else
		{
			buffId = "";
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(value: false);
			}
		}
	}

	public void SetTauntSize()
	{
		if (spriteSR != null)
		{
			spriteAnim.localScale = new Vector3(1.4f, 1.4f, 1f);
			chargesAnimTM.GetComponent<MeshRenderer>().sortingOrder = 32021;
			if (spriteSR.transform.gameObject.activeSelf)
			{
				spriteSR.transform.gameObject.SetActive(value: false);
			}
			if (chargesTM.transform.gameObject.activeSelf)
			{
				chargesTM.transform.gameObject.SetActive(value: false);
			}
			spriteSR.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
			spriteSR.sortingOrder = 32010;
			chargesTM.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
			chargesTM.transform.localPosition = new Vector3(-0.1f, -0.13f, 0f);
			chargesTMMesh.sortingOrder = 32011;
		}
	}

	public void Amplify(int charges)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("+");
		stringBuilder.Append(charges);
		chargesAnimTM.text = stringBuilder.ToString();
		stringBuilder = null;
		if (!spriteAnim.transform.gameObject.activeSelf)
		{
			spriteAnim.gameObject.SetActive(value: true);
		}
		spriteAnimator.enabled = true;
		if (AmplifyCo != null)
		{
			StopCoroutine(AmplifyCo);
		}
		AmplifyCo = StartCoroutine(AmplifyHiddenCo());
	}

	private IEnumerator AmplifyHiddenCo()
	{
		yield return Globals.Instance.WaitForSeconds(1f);
		if (spriteAnim.transform.gameObject.activeSelf)
		{
			spriteAnim.gameObject.SetActive(value: false);
		}
		spriteAnimator.enabled = false;
	}

	private void OnMouseEnter()
	{
		if (acData != null)
		{
			if (!auraImmunity)
			{
				PopupManager.Instance.SetAuraCurse(base.transform, acData.Id, chargesTM.text, fast: true, charId);
			}
			else
			{
				PopupManager.Instance.ShowKeyNote(base.transform, acData.Id, "center", fast: true);
			}
		}
		bgIconSprite.color = colorOver;
	}

	private void OnMouseExit()
	{
		PopupManager.Instance.ClosePopup();
		RestoreColor();
	}

	private void RestoreColor()
	{
		if (!(acData == null))
		{
			if (acData.IsAura)
			{
				bgIconSprite.color = colorAura;
			}
			else
			{
				bgIconSprite.color = colorCurse;
			}
			Color color = bgIconSprite.color;
			if (GameManager.Instance.ConfigACBackgrounds)
			{
				color.a = 1f;
			}
			else
			{
				color.a = 0f;
			}
			bgIconSprite.color = color;
		}
	}

	public void SetSortingInCombatHUD()
	{
		spriteSR.sortingOrder += 601;
		spriteSRShadow.sortingOrder += 601;
		chargesTMMesh.sortingOrder += 601;
		chargesTMMesh.sortingOrder += 601;
		chargesAnimTM.GetComponent<MeshRenderer>().sortingOrder += 601;
		chargesTMMesh.sortingOrder += 601;
		bgIconSprite.sortingOrder += 601;
	}
}
