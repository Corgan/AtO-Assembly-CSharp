using System.Collections;
using UnityEngine;

public class TownBuilding : MonoBehaviour
{
	public Sprite[] bgShaderLevel;

	public Sprite[] imgOverLevel;

	public Transform[] specialLevel;

	public SpriteRenderer bgShader;

	public SpriteRenderer bgPlain;

	public Transform imgOver;

	private SpriteRenderer imgOverSprite;

	private SpriteRenderer imgBaseSprite;

	public string idTitle;

	public string idDescription;

	public Sprite spriteDisabled;

	private bool disabled;

	private Color colorOri;

	private Coroutine shadowCo;

	private void Awake()
	{
		imgOver.gameObject.SetActive(value: false);
		imgOverSprite = imgOver.GetComponent<SpriteRenderer>();
		imgBaseSprite = GetComponent<SpriteRenderer>();
		HideShadow(instant: true);
	}

	public void DisableThis()
	{
		if (spriteDisabled != null)
		{
			SpriteRenderer spriteRenderer = imgOverSprite;
			Sprite sprite = (imgBaseSprite.sprite = spriteDisabled);
			spriteRenderer.sprite = sprite;
		}
		disabled = true;
	}

	public void Init(int level)
	{
		SpriteRenderer spriteRenderer = bgShader;
		Sprite sprite = (bgPlain.sprite = bgShaderLevel[level]);
		spriteRenderer.sprite = sprite;
		SpriteRenderer spriteRenderer2 = imgOverSprite;
		sprite = (imgBaseSprite.sprite = imgOverLevel[level]);
		spriteRenderer2.sprite = sprite;
		if (specialLevel != null && level < specialLevel.Length && specialLevel[level] != null && !specialLevel[level].gameObject.activeSelf)
		{
			specialLevel[level].gameObject.SetActive(value: true);
		}
		colorOri = new Color(0f, 0.68f, 1f, 1f);
		StartCoroutine(UpdateShapeToSprite());
	}

	private void HideShadow(bool instant)
	{
		if (instant)
		{
			SpriteRenderer spriteRenderer = bgShader;
			Color color = (bgPlain.color = new Color(colorOri.r, colorOri.g, colorOri.b, 0f));
			spriteRenderer.color = color;
			bgShader.gameObject.SetActive(value: false);
			bgPlain.gameObject.SetActive(value: false);
		}
		else
		{
			if (shadowCo != null)
			{
				StopCoroutine(shadowCo);
			}
			shadowCo = StartCoroutine(AnimationShadow(0));
		}
	}

	private void ShowShadow()
	{
		if (shadowCo != null)
		{
			StopCoroutine(shadowCo);
		}
		shadowCo = StartCoroutine(AnimationShadow(1));
	}

	private IEnumerator AnimationShadow(int direction)
	{
		float currentAlpha = bgShader.color.a;
		if (direction == 0)
		{
			while (currentAlpha > 0f)
			{
				currentAlpha -= 0.035f;
				Color color = new Color(colorOri.r, colorOri.g, colorOri.b, currentAlpha);
				Color color2 = new Color(colorOri.r, colorOri.g, colorOri.b, color.a * 2f);
				bgShader.color = color;
				bgPlain.color = color2;
				yield return null;
			}
			bgShader.gameObject.SetActive(value: false);
			bgPlain.gameObject.SetActive(value: false);
		}
		else
		{
			bgShader.gameObject.SetActive(value: true);
			bgPlain.gameObject.SetActive(value: true);
			while ((double)currentAlpha < 0.3)
			{
				currentAlpha += 0.035f;
				Color color = new Color(colorOri.r, colorOri.g, colorOri.b, currentAlpha);
				Color color2 = new Color(colorOri.r, colorOri.g, colorOri.b, color.a * 2f);
				bgShader.color = color;
				bgPlain.color = color2;
				yield return null;
			}
		}
	}

	private void OnMouseEnter()
	{
		if (!AlertManager.Instance.IsActive() && !GameManager.Instance.IsTutorialActive() && !SettingsManager.Instance.IsActive() && !DamageMeterManager.Instance.IsActive() && (!AtOManager.Instance.IsCombatTool || !(idTitle == "divinationCards")))
		{
			ShowShadow();
			imgOver.gameObject.SetActive(value: true);
			GameManager.Instance.PlayLibraryAudio("ui_click");
			PopupManager.Instance.SetTown(idTitle, idDescription, disabled);
			GameManager.Instance.SetCursorHover();
		}
	}

	private void OnMouseExit()
	{
		fHide();
	}

	private void OnMouseUp()
	{
		if (!Functions.ClickedThisTransform(base.transform) || AlertManager.Instance.IsActive() || GameManager.Instance.IsTutorialActive() || SettingsManager.Instance.IsActive() || DamageMeterManager.Instance.IsActive() || TownManager.Instance.AreTreasuresLocked() || disabled)
		{
			return;
		}
		bool flag = false;
		if (AtOManager.Instance.TownTutorialStep > -1 && AtOManager.Instance.TownTutorialStep < 3)
		{
			flag = true;
		}
		if (idTitle == "craftCards")
		{
			if (flag && AtOManager.Instance.TownTutorialStep != 0)
			{
				AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("tutorialTownNeedComplete"));
				return;
			}
			AtOManager.Instance.DoCardCraft();
		}
		else if (idTitle == "upgradeCards")
		{
			if (flag && AtOManager.Instance.TownTutorialStep != 1)
			{
				AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("tutorialTownNeedComplete"));
				return;
			}
			AtOManager.Instance.DoCardUpgrade();
		}
		else if (idTitle == "removeCards")
		{
			if (flag)
			{
				AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("tutorialTownNeedComplete"));
				return;
			}
			AtOManager.Instance.DoCardHealer();
		}
		else if (idTitle == "divinationCards")
		{
			if (flag)
			{
				AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("tutorialTownNeedComplete"));
				return;
			}
			AtOManager.Instance.DoCardDivination();
		}
		else if (idTitle == "buyItems")
		{
			if (flag && AtOManager.Instance.TownTutorialStep != 2)
			{
				AlertManager.Instance.AlertConfirm(Texts.Instance.GetText("tutorialTownNeedComplete"));
				return;
			}
			AtOManager.Instance.DoItemShop("");
		}
		fHide();
	}

	private void fHide()
	{
		HideShadow(instant: false);
		imgOver.gameObject.SetActive(value: false);
		PopupManager.Instance.ClosePopup();
		GameManager.Instance.SetCursorPlain();
	}

	public IEnumerator UpdateShapeToSprite()
	{
		if (base.gameObject.GetComponent<PolygonCollider2D>() != null)
		{
			Object.Destroy(base.gameObject.GetComponent<PolygonCollider2D>());
		}
		yield return Globals.Instance.WaitForSeconds(0.1f);
		base.gameObject.AddComponent<PolygonCollider2D>();
	}
}
