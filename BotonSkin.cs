using System.Text;
using TMPro;
using UnityEngine;

public class BotonSkin : MonoBehaviour
{
	public SpriteRenderer spriteSkin;

	public Transform overT;

	public Transform overRedT;

	public Transform overHoverT;

	public Transform lockT;

	public TMP_Text skinName;

	public int auxInt = -1000;

	private string dlcSku = "";

	private SkinData skinData;

	private bool locked;

	private bool active;

	private bool mustQuest;

	private void Awake()
	{
		if (skinName.transform.childCount > 0)
		{
			for (int i = 0; i < skinName.transform.childCount; i++)
			{
				MeshRenderer component = skinName.transform.GetChild(i).GetComponent<MeshRenderer>();
				component.sortingLayerName = skinName.GetComponent<MeshRenderer>().sortingLayerName;
				component.sortingOrder = skinName.GetComponent<MeshRenderer>().sortingOrder;
			}
		}
		overT.gameObject.SetActive(value: false);
	}

	public void SetSelected(bool state)
	{
		overT.gameObject.SetActive(state);
	}

	public void SetSkinData(SkinData _skinData)
	{
		skinData = _skinData;
		active = false;
		overT.gameObject.SetActive(value: false);
		overRedT.gameObject.SetActive(value: false);
		overHoverT.gameObject.SetActive(value: false);
		spriteSkin.sprite = skinData.SpritePortrait;
		string text = Texts.Instance.GetText(skinData.SkinName);
		if (text == "")
		{
			text = skinData.SkinName;
		}
		skinName.text = text.OnlyFirstCharToUpper();
		locked = false;
		if (!skinData.BaseSkin)
		{
			if (skinData.Sku != "")
			{
				if (!SteamManager.Instance.PlayerHaveDLC(skinData.Sku))
				{
					locked = true;
					dlcSku = skinData.Sku;
				}
				else if (skinData.SteamStat != "" && SteamManager.Instance.GetStatInt(skinData.SteamStat) != 1)
				{
					locked = true;
					dlcSku = skinData.Sku;
					mustQuest = true;
				}
			}
			if (!locked && skinData.PerkLevel > 0 && PlayerManager.Instance.GetPerkRank(skinData.SkinSubclass.SubClassName) < skinData.PerkLevel)
			{
				locked = true;
			}
		}
		overT.gameObject.SetActive(value: false);
		if (!locked)
		{
			lockT.gameObject.SetActive(value: false);
			if (PlayerManager.Instance.SkinUsed.ContainsKey(skinData.SkinSubclass.SubClassName.Replace(" ", "")) && PlayerManager.Instance.SkinUsed[skinData.SkinSubclass.SubClassName.Replace(" ", "")] == skinData.SkinId)
			{
				overT.gameObject.SetActive(value: true);
				active = true;
				skinName.color = new Color(1f, 0.68f, 0.09f);
			}
			else
			{
				skinName.color = new Color(1f, 1f, 1f, 1f);
			}
		}
		else
		{
			skinName.color = Functions.HexToColor(Globals.Instance.ClassColor["warrior"]);
			lockT.gameObject.SetActive(value: true);
		}
	}

	private void OnMouseEnter()
	{
		if (AlertManager.Instance.IsActive() || SettingsManager.Instance.IsActive())
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (skinData != null)
		{
			HeroSelectionManager.Instance.charPopup.ShowSkin(skinData.SkinGo);
			if (skinData.SkinTextId != string.Empty && Texts.Instance.GetText(skinData.SkinTextId) != string.Empty)
			{
				stringBuilder.Append("<line-height=10><br></line-height><color=#FC0>~ ");
				stringBuilder.Append(Texts.Instance.GetText(skinData.SkinTextId));
				stringBuilder.Append(" ~</color><line-height=6><br></line-height><br>");
			}
		}
		if (locked)
		{
			overRedT.gameObject.SetActive(value: true);
			if (dlcSku != "")
			{
				if (mustQuest)
				{
					stringBuilder.Append(string.Format(Texts.Instance.GetText("requiredDLCandQuest"), SteamManager.Instance.GetDLCName(dlcSku)));
				}
				else
				{
					stringBuilder.Append(string.Format(Texts.Instance.GetText("requiredDLC"), SteamManager.Instance.GetDLCName(dlcSku)));
				}
			}
			else
			{
				stringBuilder.Append(string.Format(Texts.Instance.GetText("skinRequiredRankLevel"), skinData.PerkLevel));
			}
		}
		else if (!active)
		{
			GameManager.Instance.SetCursorHover();
			overHoverT.gameObject.SetActive(value: true);
		}
		if (stringBuilder.Length > 0)
		{
			PopupManager.Instance.SetText(stringBuilder.ToString(), fast: true, "", alwaysCenter: true);
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
			HeroSelectionManager.Instance.charPopup.ResetSkin();
		}
	}

	public void OnMouseUp()
	{
		if (!AlertManager.Instance.IsActive() && !SettingsManager.Instance.IsActive() && !locked)
		{
			PlayerManager.Instance.SetSkin(skinData.SkinSubclass.SubClassName.Replace(" ", ""), skinData.SkinId);
			HeroSelectionManager.Instance.SetSkinIntoSubclassData(skinData.SkinSubclass.SubClassName.Replace(" ", ""), skinData.SkinId);
			HeroSelectionManager.Instance.charPopup.DoSkins();
			HeroSelectionManager.Instance.charPopupMini.SetSubClassData(skinData.SkinSubclass);
		}
	}
}
