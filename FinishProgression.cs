using System.Collections;
using TMPro;
using UnityEngine;

public class FinishProgression : MonoBehaviour
{
	public TMP_Text charName;

	public TMP_Text charProgress;

	public TMP_Text charMin;

	public TMP_Text charMax;

	public TMP_Text charRank;

	public TMP_Text charPoints;

	public Transform progressBarMask;

	public SpriteRenderer progressBar;

	public Transform progressBarParticles;

	private string characterName = "";

	private string className = "";

	private string subclassId = "";

	private int iniProgress;

	private int maxProgress;

	private int charIndex = -1;

	private Coroutine coIncrement;

	public void SetCharacter(string _charName, string _className, string _subclassId, int _index)
	{
		characterName = _charName;
		className = _className;
		subclassId = _subclassId;
		charIndex = _index;
		charName.text = _charName;
		DoProgress();
	}

	public void DoProgress()
	{
		int num = (iniProgress = PlayerManager.Instance.GetProgress(subclassId));
		int perkPrevLevelPoints = PlayerManager.Instance.GetPerkPrevLevelPoints(subclassId);
		int num2 = (maxProgress = PlayerManager.Instance.GetPerkNextLevelPoints(subclassId));
		charProgress.text = num.ToString();
		if (num2 != 0)
		{
			charMin.text = perkPrevLevelPoints.ToString();
			charMax.text = num2.ToString();
		}
		else
		{
			charMax.text = "";
			charMin.text = "";
		}
		float x = ((float)num - (float)perkPrevLevelPoints) / ((float)num2 - (float)perkPrevLevelPoints) * 1.648945f;
		if (num2 == 0)
		{
			x = 1.648945f;
			progressBarParticles.gameObject.SetActive(value: false);
			StopBlockProgress();
		}
		progressBarMask.localScale = new Vector3(x, progressBarMask.localScale.y, progressBarMask.localScale.z);
		SpriteRenderer spriteRenderer = progressBar;
		Color color = (charProgress.color = Functions.HexToColor(Globals.Instance.ClassColor[className]));
		spriteRenderer.color = color;
		charRank.text = string.Format(Texts.Instance.GetText("rankProgress"), PlayerManager.Instance.GetPerkRank(subclassId));
		int perkPointsAvailable = PlayerManager.Instance.GetPerkPointsAvailable(subclassId);
		if (perkPointsAvailable > 0)
		{
			charPoints.text = string.Format(Texts.Instance.GetText("rankPerkPoints"), perkPointsAvailable + " <sprite name=experience>");
			charPoints.gameObject.SetActive(value: true);
		}
		else
		{
			charPoints.gameObject.SetActive(value: false);
		}
	}

	public void Increment(int finalProgress)
	{
		coIncrement = StartCoroutine(IncrementTimeout(finalProgress));
	}

	private IEnumerator IncrementTimeout(int destine)
	{
		yield return Globals.Instance.WaitForSeconds(0.03f);
		int num = 0;
		if (destine > 500)
		{
			num = 500;
		}
		else if (destine > 100)
		{
			num = 100;
		}
		else if (destine > 25)
		{
			num = 25;
		}
		else if (destine > 10)
		{
			num = 10;
		}
		else if (destine > 0)
		{
			num = 1;
		}
		iniProgress += num;
		destine -= num;
		PlayerManager.Instance.ModifyProgress(subclassId, num, charIndex);
		if (destine <= 0)
		{
			if (coIncrement != null)
			{
				StopCoroutine(coIncrement);
			}
			StopBlockProgress();
		}
		else
		{
			DoProgress();
			Increment(destine);
		}
	}

	private void StopBlockProgress()
	{
		progressBarParticles.gameObject.SetActive(value: false);
		charProgress.text = PlayerManager.Instance.GetProgress(subclassId).ToString();
		FinishRunManager.Instance.UnlockState(charIndex);
	}

	public bool IsActive()
	{
		return base.gameObject.activeSelf;
	}

	public void Hide()
	{
		base.gameObject.SetActive(value: false);
	}
}
