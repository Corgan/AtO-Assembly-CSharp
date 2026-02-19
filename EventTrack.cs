using System.Collections;
using TMPro;
using UnityEngine;

public class EventTrack : MonoBehaviour
{
	public TMP_Text trackName;

	private string description;

	public Transform textT;

	public Transform bgT;

	public SpriteRenderer iconSprite;

	private BoxCollider2D collider;

	private void Awake()
	{
		collider = GetComponent<BoxCollider2D>();
	}

	public void SetTrack(string erq)
	{
		EventRequirementData requirementData = Globals.Instance.GetRequirementData(erq);
		trackName.text = requirementData.RequirementName;
		description = requirementData.Description;
		if (requirementData.TrackSprite != null)
		{
			iconSprite.sprite = requirementData.TrackSprite;
		}
		StartCoroutine(ShowTrack(state: true));
	}

	private void OnMouseEnter()
	{
		if (!AlertManager.Instance.IsActive() && !GameManager.Instance.IsTutorialActive() && !SettingsManager.Instance.IsActive() && !DamageMeterManager.Instance.IsActive() && (!MapManager.Instance || !MapManager.Instance.IsCharacterUnlock()) && (!EventManager.Instance || !textT.gameObject.activeSelf))
		{
			if (textT.gameObject.activeSelf)
			{
				StartCoroutine(ShowTrack(state: true));
			}
			else
			{
				collider.offset = new Vector2(0f, 0f);
			}
			PopupManager.Instance.SetText(description, fast: true);
		}
	}

	private IEnumerator ShowTrack(bool state)
	{
		_ = Vector3.zero;
		_ = Vector3.zero;
		int steps = 10;
		Vector3 stepBg = new Vector3(2.54f / (float)steps, 0f, 0f);
		Vector3 stepText = new Vector3(4.1f / (float)steps, 0f, 0f);
		float posEndText;
		float posEndBg;
		if (state)
		{
			posEndText = -0.12f;
			posEndBg = -0.5f;
			collider.offset = new Vector2(-0.45f, -0.03f);
		}
		else
		{
			posEndText = -4.1f;
			posEndBg = -3.4f;
			collider.offset = new Vector2(-2.4f, -0.03f);
		}
		for (int i = 0; i < steps; i++)
		{
			if (state)
			{
				if (textT.localPosition.x < posEndText)
				{
					textT.localPosition += stepText;
				}
				if (bgT.localPosition.x < posEndBg)
				{
					bgT.localPosition += stepBg;
				}
			}
			else
			{
				if (textT.localPosition.x > posEndText)
				{
					textT.localPosition -= stepText;
				}
				if (bgT.localPosition.x > posEndBg)
				{
					bgT.localPosition -= stepBg;
				}
			}
			yield return Globals.Instance.WaitForSeconds(0.01f);
		}
	}

	private void OnMouseExit()
	{
		PopupManager.Instance.ClosePopup();
	}
}
