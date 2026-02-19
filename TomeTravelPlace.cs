using TMPro;
using UnityEngine;

public class TomeTravelPlace : MonoBehaviour
{
	public SpriteRenderer icon;

	public new TMP_Text name;

	public Sprite townSprite;

	public Sprite energySprite;

	public Sprite namedSprite;

	public Sprite bossSprite;

	public void SetNode(bool isObeliskChallenge, string nodeId, string nodeAction, string obeliskIcon)
	{
		string[] array = nodeId.Split('_');
		if (!isObeliskChallenge)
		{
			NodeData nodeData = Globals.Instance.GetNodeData(nodeId);
			if (nodeData != null)
			{
				name.text = nodeData.NodeName;
			}
		}
		else
		{
			EventData eventData = Globals.Instance.GetEventData(nodeAction);
			if (eventData != null)
			{
				name.text = Texts.Instance.GetText(eventData.EventId + "_nm", "events");
			}
			else if (nodeAction == "destination" || (array != null && array.Length == 2 && array[1] == "0"))
			{
				name.text = Texts.Instance.GetText("mapEntryPoint");
			}
		}
		if (Globals.Instance.GetCombatData(nodeAction) != null)
		{
			if (isObeliskChallenge)
			{
				if (obeliskIcon == "boss")
				{
					icon.sprite = namedSprite;
				}
				else if (obeliskIcon == "finalboss")
				{
					icon.sprite = bossSprite;
				}
				name.text = Texts.Instance.GetText("combat");
			}
		}
		else if (nodeAction == "destination" || (array != null && array.Length == 2 && array[1] == "0"))
		{
			icon.sprite = energySprite;
		}
		else if (nodeAction.ToLower() == "town")
		{
			icon.sprite = townSprite;
		}
		else
		{
			EventData eventData2 = Globals.Instance.GetEventData(nodeAction);
			if (eventData2 != null)
			{
				icon.sprite = eventData2.EventSpriteMap;
			}
		}
	}
}
