using System.Text;
using Paradox;
using TMPro;
using UnityEngine;

public class BotonCardback : MonoBehaviour
{
	public SpriteRenderer spriteCardback;

	public Transform overT;

	public Transform overRedT;

	public Transform overHoverT;

	public Transform lockT;

	public TMP_Text cardbackName;

	public int auxInt = -1000;

	private CardbackData cardbackData;

	private bool unlocked;

	private bool active;

	private string subclassName = "";

	private Color colorCardbackActive = new Color(1f, 0.68f, 0.09f);

	private Color colorCardbackAvailable = new Color(1f, 1f, 1f, 1f);

	private Color colorCardbackLocked = new Color(0.92f, 0.28f, 0.33f, 1f);

	private static BotonCardback currentActive;

	private void Awake()
	{
		if (cardbackName.transform.childCount > 0)
		{
			for (int i = 0; i < cardbackName.transform.childCount; i++)
			{
				MeshRenderer component = cardbackName.transform.GetChild(i).GetComponent<MeshRenderer>();
				component.sortingLayerName = cardbackName.GetComponent<MeshRenderer>().sortingLayerName;
				component.sortingOrder = cardbackName.GetComponent<MeshRenderer>().sortingOrder;
			}
		}
		if (cardbackName.color != colorCardbackActive)
		{
			overT.gameObject.SetActive(value: false);
		}
	}

	public void SetSelected(bool state)
	{
		overT.gameObject.SetActive(state);
	}

	public void SetCardbackData(string _cardbackDataId, bool _unlocked, string _subclassName)
	{
		cardbackData = Globals.Instance.GetCardbackData(_cardbackDataId);
		if (cardbackData == null)
		{
			return;
		}
		active = false;
		overT.gameObject.SetActive(value: false);
		overRedT.gameObject.SetActive(value: false);
		overHoverT.gameObject.SetActive(value: false);
		spriteCardback.sprite = cardbackData.CardbackSprite;
		string text = Texts.Instance.GetText(cardbackData.CardbackName);
		if (text == "")
		{
			text = cardbackData.CardbackName;
		}
		cardbackName.text = text.OnlyFirstCharToUpper();
		unlocked = _unlocked;
		subclassName = _subclassName;
		overT.gameObject.SetActive(value: false);
		if (unlocked)
		{
			lockT.gameObject.SetActive(value: false);
			if (PlayerManager.Instance.GetActiveCardback(_subclassName) != null && PlayerManager.Instance.GetActiveCardback(_subclassName) == cardbackData.CardbackId.ToLower())
			{
				overT.gameObject.SetActive(value: true);
				active = true;
				cardbackName.color = colorCardbackActive;
				currentActive = this;
			}
			else
			{
				cardbackName.color = colorCardbackAvailable;
			}
		}
		else
		{
			cardbackName.color = colorCardbackLocked;
			lockT.gameObject.SetActive(value: true);
		}
	}

	private void OnMouseEnter()
	{
		if (cardbackData == null || AlertManager.Instance.IsActive() || SettingsManager.Instance.IsActive())
		{
			return;
		}
		string text = "";
		if (!unlocked)
		{
			overRedT.gameObject.SetActive(value: true);
			if (cardbackData.PdxAccountRequired && !Startup.isLoggedIn)
			{
				text = Texts.Instance.GetText("loggedPDXitem");
			}
			else if (cardbackData.Sku != "")
			{
				if (!SteamManager.Instance.PlayerHaveDLC(cardbackData.Sku))
				{
					text = string.Format(Texts.Instance.GetText("requiredDLC"), SteamManager.Instance.GetDLCName(cardbackData.Sku));
				}
			}
			else if (cardbackData.SteamStat != "")
			{
				text = string.Format(Texts.Instance.GetText("requiredWeekly"), Texts.Instance.GetText(cardbackData.SteamStat));
			}
			else if (cardbackData.AdventureLevel > 0)
			{
				text = ((cardbackData.AdventureLevel != 1) ? string.Format(Texts.Instance.GetText("requiredAdventureLevel"), cardbackData.AdventureLevel) : Texts.Instance.GetText("requiredAdventureComplete"));
			}
			else if (cardbackData.ObeliskLevel > 0)
			{
				text = ((cardbackData.ObeliskLevel != 1) ? string.Format(Texts.Instance.GetText("requiredObeliskLevel"), cardbackData.ObeliskLevel) : Texts.Instance.GetText("requiredObeliskComplete"));
			}
			else if (cardbackData.SingularityLevel > 0)
			{
				text = ((cardbackData.SingularityLevel != 1) ? string.Format(Texts.Instance.GetText("requiredSingularityLevel"), cardbackData.SingularityLevel) : Texts.Instance.GetText("requiredSingularityComplete"));
			}
			else if (cardbackData.RankLevel > 0)
			{
				text = string.Format(Texts.Instance.GetText("skinRequiredRankLevel"), cardbackData.RankLevel);
			}
		}
		else if (!active)
		{
			GameManager.Instance.SetCursorHover();
			overHoverT.gameObject.SetActive(value: true);
		}
		if (unlocked && cardbackData.CardbackTextId != "" && cardbackData.CardbackTextId != string.Empty && Texts.Instance.GetText(cardbackData.CardbackTextId) != string.Empty)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Texts.Instance.GetText(cardbackData.CardbackTextId));
			text = stringBuilder.ToString();
		}
		if (text != "")
		{
			PopupManager.Instance.SetText(text, fast: true, "", alwaysCenter: true);
		}
	}

	private void OnMouseExit()
	{
		if (!AlertManager.Instance.IsActive() && !SettingsManager.Instance.IsActive())
		{
			overRedT.gameObject.SetActive(value: false);
			overHoverT.gameObject.SetActive(value: false);
			GameManager.Instance.SetCursorPlain();
			PopupManager.Instance.ClosePopup();
		}
	}

	public void OnMouseUp()
	{
		if (cardbackData == null || AlertManager.Instance.IsActive() || SettingsManager.Instance.IsActive() || !unlocked)
		{
			return;
		}
		PlayerManager.Instance.SetCardback(subclassName, cardbackData.CardbackId);
		HeroSelectionManager.Instance.AssignHeroCardback(subclassName, cardbackData.CardbackId);
		if (unlocked)
		{
			if (currentActive != null && currentActive != this)
			{
				currentActive.Deselect();
			}
			lockT.gameObject.SetActive(value: false);
			overT.gameObject.SetActive(value: true);
			active = true;
			cardbackName.color = colorCardbackActive;
			currentActive = this;
		}
		else
		{
			cardbackName.color = colorCardbackLocked;
			lockT.gameObject.SetActive(value: true);
		}
	}

	private void Deselect()
	{
		active = false;
		overT.gameObject.SetActive(value: false);
		cardbackName.color = colorCardbackAvailable;
	}
}
