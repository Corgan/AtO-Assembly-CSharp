using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class NewTurn : MonoBehaviour
{
	public TMP_Text textTM;

	public SpriteRenderer spr;

	private Transform textT;

	private bool end;

	private bool passTurn;

	private bool cantDraw;

	private Coroutine co;

	private void Awake()
	{
		textT = textTM.transform;
		textTM.text = "";
		textT.gameObject.SetActive(value: false);
	}

	private void GoToEnd()
	{
		HideMask();
	}

	private void ShowMask()
	{
		MatchManager.Instance.ShowMask(state: true, hardMask: false);
	}

	private void HideMask()
	{
		MatchManager.Instance.ShowMask(state: false);
	}

	public void FinishCombat(bool won)
	{
		end = true;
		if (won)
		{
			SetTurn(Texts.Instance.GetText("combatVictory"));
		}
		else
		{
			SetTurn(Texts.Instance.GetText("combatFail"));
		}
	}

	public void CantDraw(string name)
	{
		passTurn = true;
		cantDraw = true;
		if (co != null)
		{
			StopCoroutine(co);
		}
		co = StartCoroutine(SetTurnCo(name));
	}

	public void PassTurn(string name)
	{
		passTurn = true;
		cantDraw = false;
		if (co != null)
		{
			StopCoroutine(co);
		}
		co = StartCoroutine(SetTurnCo(name));
	}

	public void SetTurn(string name)
	{
		if (co != null)
		{
			StopCoroutine(co);
		}
		co = StartCoroutine(SetTurnCo(name));
	}

	private IEnumerator SetTurnCo(string name)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (end)
		{
			stringBuilder.Append("<color=#FC0>");
			stringBuilder.Append(name);
			stringBuilder.Append("</color>");
		}
		else if (passTurn)
		{
			stringBuilder.Append(name);
			stringBuilder.Append("<br><color=#FC0>");
			if (cantDraw)
			{
				stringBuilder.Append(Texts.Instance.GetText("cantDrawAnyCard"));
				cantDraw = false;
			}
			else
			{
				stringBuilder.Append(Texts.Instance.GetText("missesThisTurn"));
			}
			stringBuilder.Append("</color>");
		}
		else
		{
			stringBuilder.Append("<size=4.5>");
			stringBuilder.Append(Texts.Instance.GetText("newTurn"));
			stringBuilder.Append("</size>\n<b><color=#FFF>");
			stringBuilder.Append(name.ToLower());
			stringBuilder.Append("</color></b>");
		}
		textTM.text = stringBuilder.ToString();
		textT.localScale = Vector3.zero;
		textT.gameObject.SetActive(value: true);
		Vector3 vectorScale = new Vector3(0.1f, 0.1f, 0f);
		while (textT.localScale.x < 1.1f)
		{
			textT.localScale += vectorScale;
			yield return null;
		}
		while (textT.localScale.x > 1f)
		{
			textT.localScale -= vectorScale;
			yield return null;
		}
		textT.localScale = new Vector3(1f, 1f, 1f);
		if (!end)
		{
			if (passTurn)
			{
				passTurn = false;
				yield return Globals.Instance.WaitForSeconds(1f);
			}
			else
			{
				yield return Globals.Instance.WaitForSeconds(0.5f);
			}
			while (textT.localScale.x > 0f)
			{
				textT.localScale -= vectorScale;
				yield return null;
			}
			textT.gameObject.SetActive(value: false);
		}
		else if (end)
		{
			yield return Globals.Instance.WaitForSeconds(1f);
			MatchManager.Instance.BackToFinishGame();
		}
	}
}
