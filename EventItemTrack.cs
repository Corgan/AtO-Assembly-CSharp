using System.Collections;
using System.Text;
using UnityEngine;

public class EventItemTrack : MonoBehaviour
{
	private string description;

	private Coroutine timeoutCardCo;

	private EventRequirementData requirementData;

	public void SetItemTrack(EventRequirementData _requirementData)
	{
		requirementData = _requirementData;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<size=+3><color=#FFF>");
		stringBuilder.Append(requirementData.RequirementName);
		stringBuilder.Append("</color></size><line-height=30><br></line-height>");
		stringBuilder.Append(requirementData.Description);
		description = stringBuilder.ToString();
		GetComponent<SpriteRenderer>().sprite = requirementData.ItemSprite;
		if (base.transform.GetChild(0).GetComponent<SpriteMask>() != null)
		{
			base.transform.GetChild(0).GetComponent<SpriteMask>().sprite = requirementData.ItemSprite;
		}
	}

	private void OnMouseEnter()
	{
		if (!AlertManager.Instance.IsActive() && !GameManager.Instance.IsTutorialActive() && !SettingsManager.Instance.IsActive() && !DamageMeterManager.Instance.IsActive() && (!MapManager.Instance || !MapManager.Instance.IsCharacterUnlock()))
		{
			PopupManager.Instance.SetText(description, fast: true, "followdown", alwaysCenter: true);
			if (timeoutCardCo != null)
			{
				StopCoroutine(timeoutCardCo);
			}
			if (requirementData.TrackCard != null)
			{
				timeoutCardCo = StartCoroutine(TimeOutCard());
			}
		}
	}

	private IEnumerator TimeOutCard()
	{
		yield return Globals.Instance.WaitForSeconds(0.2f);
		CardData cardData = Globals.Instance.GetCardData(requirementData.TrackCard.Id, instantiate: false);
		CardItem component = GetComponent<CardItem>();
		component.cardFromEventTracker = true;
		component.CreateAmplifyOutsideCard(Globals.Instance.GetCardData(cardData.Id, instantiate: false), GetComponent<BoxCollider2D>(), null, disableCollider: true);
	}

	private void OnMouseExit()
	{
		PopupManager.Instance.ClosePopup();
		if (timeoutCardCo != null)
		{
			StopCoroutine(timeoutCardCo);
		}
		if (requirementData.TrackCard != null)
		{
			GetComponent<CardItem>().DestroyReveleadOutside();
		}
	}
}
