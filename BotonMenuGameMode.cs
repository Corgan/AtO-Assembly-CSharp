using System.Collections;
using TMPro;
using TMPro.Examples;
using UnityEngine;

public class BotonMenuGameMode : MonoBehaviour
{
	public TMP_Text optionText;

	public SpriteRenderer cenefa;

	public Transform description;

	public Transform grayMask;

	public int gameMode;

	private void Awake()
	{
		grayMask.gameObject.SetActive(value: true);
		description.gameObject.SetActive(value: false);
	}

	private void Start()
	{
		TurnOffState();
	}

	public void TurnOnState()
	{
		grayMask.gameObject.SetActive(value: false);
		cenefa.color = new Color(1f, 1f, 1f, 0.85f);
	}

	public void TurnOffState()
	{
		grayMask.gameObject.SetActive(value: true);
		cenefa.color = new Color(0.78f, 0.55f, 0.56f, 0.75f);
	}

	private void OnMouseEnter()
	{
		GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonHover);
		description.gameObject.SetActive(value: true);
		TurnOnState();
	}

	private void OnMouseExit()
	{
		description.gameObject.SetActive(value: false);
		TurnOffState();
	}

	private IEnumerator RebuildWarp()
	{
		yield return Globals.Instance.WaitForSeconds(0.01f);
		optionText.GetComponent<WarpTextExample>().CurveScale = 4.8f;
	}

	public void OnMouseUp()
	{
		if (!AlertManager.Instance.IsActive())
		{
			MainMenuManager.Instance.ShowSaveGame(status: true, gameMode);
			GameManager.Instance.PlayAudio(AudioManager.Instance.soundButtonClick);
		}
	}
}
