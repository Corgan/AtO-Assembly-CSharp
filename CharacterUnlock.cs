using System;
using TMPro;
using UnityEngine;

public class CharacterUnlock : MonoBehaviour
{
	public TMP_Text nameTMP;

	public SpriteRenderer bgSPR;

	public SpriteRenderer characterSPR;

	public SpriteRenderer whirlSPR;

	private void Start()
	{
	}

	public void ShowUnlock(SubClassData _scd, SkinData _skd = null)
	{
		Color color = Functions.HexToColor(Globals.Instance.ClassColor[Enum.GetName(typeof(Enums.HeroClass), _scd.HeroClass)]);
		nameTMP.text = _scd.CharacterName;
		if (_scd.CharacterName.ToLower() == "thuls")
		{
			characterSPR.transform.localPosition = new Vector3(-0.5f, 0f, 0f);
		}
		else if (_scd.CharacterName.ToLower() == "nyrada")
		{
			characterSPR.transform.localPosition = new Vector3(-0.15f, 0f, 0f);
		}
		else if (_scd.CharacterName.ToLower() == "malukah")
		{
			characterSPR.transform.localPosition = new Vector3(-0.75f, 0f, 0f);
		}
		else if (_scd.CharacterName.ToLower() == "ottis")
		{
			if (_skd != null && _skd.SkinId == "ottiswolfwars")
			{
				characterSPR.transform.localPosition = new Vector3(-0.37f, -0.13f, 0f);
			}
			else
			{
				characterSPR.transform.localPosition = new Vector3(0f, 0.2f, 0f);
			}
		}
		else if (_scd.CharacterName.ToLower() == "heiner")
		{
			characterSPR.transform.localPosition = new Vector3(-0.2f, -0.1f, 0f);
		}
		else if (_scd.CharacterName.ToLower() == "grukli")
		{
			characterSPR.transform.localPosition = new Vector3(-0.2f, 0f, 0f);
		}
		else if (_scd.CharacterName.ToLower() == "wilbur")
		{
			characterSPR.transform.localPosition = new Vector3(-0.4f, 0f, 0f);
		}
		else if (_scd.CharacterName.ToLower() == "nezglekt")
		{
			characterSPR.transform.localPosition = new Vector3(0.2f, -0.1f, 0f);
		}
		else if (_scd.CharacterName.ToLower() == "yogger")
		{
			characterSPR.transform.localPosition = new Vector3(-0.2f, 0f, 0f);
		}
		else
		{
			characterSPR.transform.localPosition = new Vector3(0f, 0f, 0f);
		}
		if (_skd != null)
		{
			nameTMP.text = Texts.Instance.GetText("characterSkin");
		}
		else
		{
			nameTMP.text = Globals.Instance.GetSubClassData(_scd.Id).CharacterName;
		}
		bgSPR.color = color;
		whirlSPR.color = new Color(color.r, color.g, color.b, 0.7f);
		SkinData skinData = null;
		skinData = ((!(_skd == null)) ? _skd : Globals.Instance.GetSkinData(Globals.Instance.GetSkinBaseIdBySubclass(_scd.Id)));
		if (skinData != null)
		{
			characterSPR.sprite = skinData.SpriteSiluetaGrande;
		}
	}
}
