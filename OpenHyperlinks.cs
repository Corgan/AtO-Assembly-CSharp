using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class OpenHyperlinks : MonoBehaviour
{
	public bool doesColorChangeOnHover = true;

	public Color hoverColor = new Color(0.23529412f, 0.47058824f, 1f);

	public TMP_Text pTextMeshPro;

	private int pCurrentLink = -1;

	private List<Color32[]> pOriginalVertexColors = new List<Color32[]>();

	private string currentInfo = "";

	public bool isLinkHighlighted => pCurrentLink != -1;

	private void LateUpdate()
	{
		if (Time.frameCount % 2 == 0)
		{
			return;
		}
		int num = (TMP_TextUtilities.IsIntersectingRectTransform(pTextMeshPro.rectTransform, GameManager.Instance.cameraMain.ScreenToWorldPoint(Input.mousePosition), null) ? TMP_TextUtilities.FindIntersectingLink(pTextMeshPro, GameManager.Instance.cameraMain.ScreenToWorldPoint(Input.mousePosition), null) : (-1));
		if (pCurrentLink != -1 && num != pCurrentLink)
		{
			try
			{
				SetLinkToColor(pCurrentLink, (int linkIdx, int vertIdx) => pOriginalVertexColors[linkIdx][vertIdx]);
			}
			catch (Exception ex)
			{
				if (GameManager.Instance.GetDeveloperMode())
				{
					Debug.LogWarning("AddCharges exception-> " + ex);
				}
				pOriginalVertexColors.Clear();
				pCurrentLink = -1;
				currentInfo = "";
				return;
			}
			pOriginalVertexColors.Clear();
			pCurrentLink = -1;
		}
		if (num != -1 && num != pCurrentLink)
		{
			pCurrentLink = num;
			if (doesColorChangeOnHover)
			{
				pOriginalVertexColors = SetLinkToColor(num, (int _linkIdx, int _vertIdx) => hoverColor);
			}
			TMP_LinkInfo tMP_LinkInfo = pTextMeshPro.textInfo.linkInfo[num];
			if (currentInfo != tMP_LinkInfo.GetLinkID())
			{
				currentInfo = tMP_LinkInfo.GetLinkID();
				if ((bool)MatchManager.Instance && MatchManager.Instance.console != null)
				{
					MatchManager.Instance.console.ShowCard(currentInfo, tMP_LinkInfo.GetLinkText());
				}
			}
		}
		else if (currentInfo != "")
		{
			currentInfo = "";
		}
	}

	private List<Color32[]> SetLinkToColor(int linkIndex, Func<int, int, Color32> colorForLinkAndVert)
	{
		TMP_LinkInfo tMP_LinkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];
		List<Color32[]> list = new List<Color32[]>();
		for (int i = 0; i < tMP_LinkInfo.linkTextLength; i++)
		{
			int num = tMP_LinkInfo.linkTextfirstCharacterIndex + i;
			TMP_CharacterInfo tMP_CharacterInfo = pTextMeshPro.textInfo.characterInfo[num];
			int materialReferenceIndex = tMP_CharacterInfo.materialReferenceIndex;
			int vertexIndex = tMP_CharacterInfo.vertexIndex;
			Color32[] colors = pTextMeshPro.textInfo.meshInfo[materialReferenceIndex].colors32;
			list.Add(colors.ToArray());
			if (tMP_CharacterInfo.isVisible)
			{
				colors[vertexIndex] = colorForLinkAndVert(i, vertexIndex);
				colors[vertexIndex + 1] = colorForLinkAndVert(i, vertexIndex + 1);
				colors[vertexIndex + 2] = colorForLinkAndVert(i, vertexIndex + 2);
				colors[vertexIndex + 3] = colorForLinkAndVert(i, vertexIndex + 3);
			}
		}
		pTextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
		return list;
	}
}
