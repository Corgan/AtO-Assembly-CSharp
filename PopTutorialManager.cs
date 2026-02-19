using System.Text;
using TMPro;
using UnityEngine;

public class PopTutorialManager : MonoBehaviour
{
	public Transform content;

	public Transform box;

	public Transform circle;

	public Transform circle2;

	public Transform continueButton;

	public TMP_Text popText;

	private void Awake()
	{
		ShowContent(state: false);
	}

	private string FormatText(string title, string body)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<color=#FFF><size=+.6>");
		stringBuilder.Append(title);
		stringBuilder.Append("</color></size><line-height=160%><br></line-height>");
		stringBuilder.Append(body);
		return stringBuilder.ToString();
	}

	public void Show(string type, Vector3 position, Vector3 position2)
	{
		switch (type)
		{
		case "characterUnlock":
			box.transform.position = new Vector3(3.1f, 0f, box.transform.position.z);
			popText.text = FormatText(Texts.Instance.GetText("tutorialCharactersTitle"), Texts.Instance.GetText("tutorialCharacters"));
			MoveCircle(position);
			MoveCircle2(new Vector3(3.1f, -1f, 0f));
			SizeCircle2(new Vector3(1f, 2f, 1f));
			break;
		case "characterPerks":
			box.transform.position = new Vector3(3.1f, 0f, box.transform.position.z);
			popText.text = FormatText(Texts.Instance.GetText("tutorialPerksTitle"), Texts.Instance.GetText("tutorialPerks"));
			MoveCircle(position);
			MoveCircle2(new Vector3(3.1f, -1f, 0f));
			SizeCircle2(new Vector3(1f, 2f, 1f));
			break;
		case "town":
			box.transform.position = new Vector3(0f, 0f, box.transform.position.z);
			popText.text = FormatText(Texts.Instance.GetText("tutorialTownTitle"), Texts.Instance.GetText("tutorialTown"));
			MoveCircle(Vector3.zero);
			HideCircle2();
			break;
		case "combatSpeed":
			box.transform.position = new Vector3(0f, 0f, box.transform.position.z);
			popText.text = FormatText(Texts.Instance.GetText("tutorialCombatSpeedTitle"), Texts.Instance.GetText("tutorialCombatSpeed"));
			MoveCircle(position);
			SizeCircle(new Vector3(1.5f, 1.5f, 1f));
			HideCircle2();
			break;
		case "firstTurnEnergy":
			box.transform.position = new Vector3(Globals.Instance.sizeW * 0.25f, Globals.Instance.sizeH * 0.05f, box.transform.position.z);
			popText.text = FormatText(Texts.Instance.GetText("tutorialFirstTurnEnergyTitle"), Texts.Instance.GetText("tutorialFirstTurnEnergy"));
			MoveCircle(position);
			SizeCircle(new Vector3(1.5f, 1.5f, 1f));
			MoveCircle2(position2);
			SizeCircle2(new Vector3(1.5f, 1.5f, 1f));
			break;
		case "cardTarget":
			box.transform.position = new Vector3(Globals.Instance.sizeW * 0.25f, Globals.Instance.sizeH * 0.05f, box.transform.position.z);
			popText.text = FormatText(Texts.Instance.GetText("tutorialCombatCardTargetTitle"), Texts.Instance.GetText("tutorialCombatCardTarget"));
			MoveCircle(position);
			SizeCircle(new Vector3(2.8f, 0.7f, 1f));
			HideCircle2();
			break;
		case "combatResists":
			box.transform.position = new Vector3(Globals.Instance.sizeW * 0.25f, Globals.Instance.sizeH * 0.05f, box.transform.position.z);
			popText.text = FormatText(Texts.Instance.GetText("tutorialCombatResistsTitle"), Texts.Instance.GetText("tutorialCombatResists"));
			MoveCircle(position);
			SizeCircle(new Vector3(5f, 2.5f, 1f));
			MoveCircle2(position2);
			SizeCircle2(new Vector3(3f, 3f, 1f));
			break;
		case "castNPC":
			box.transform.position = new Vector3(0f, 0f, box.transform.position.z);
			popText.text = FormatText(Texts.Instance.GetText("castNPCTitle"), Texts.Instance.GetText("castNPC"));
			MoveCircle(position);
			SizeCircle(new Vector3(2f, 2f, 1f));
			HideCircle2();
			break;
		case "eventRolls":
			box.transform.position = new Vector3((0f - Globals.Instance.sizeW) * 0.12f, Globals.Instance.sizeH * 0.05f, box.transform.position.z);
			popText.text = FormatText(Texts.Instance.GetText("tutorialEventRollsTitle"), Texts.Instance.GetText("tutorialEventRolls"));
			MoveCircle(position);
			SizeCircle(new Vector3(5f, 1.3f, 1f));
			HideCircle2();
			break;
		case "townReward":
			box.transform.position = new Vector3(0f, 0f, box.transform.position.z);
			popText.text = FormatText(Texts.Instance.GetText("tutorialTownRewardsTitle"), Texts.Instance.GetText("tutorialTownRewards"));
			MoveCircle(position);
			SizeCircle(new Vector3(5f, 1.8f, 1f));
			HideCircle2();
			break;
		case "cardsReward":
			box.transform.position = new Vector3(0f, -1f, box.transform.position.z);
			popText.text = FormatText(Texts.Instance.GetText("tutorialCardsRewardTitle"), Texts.Instance.GetText("tutorialCardsReward"));
			MoveCircle(new Vector3(0f, -1f, 0f));
			HideCircle2();
			break;
		case "townItemCraft":
		{
			box.transform.position = new Vector3(0f, 0f, box.transform.position.z);
			string body3 = string.Format(Texts.Instance.GetText("tutorialTownCraft"), Globals.Instance.GetCardData("fireball", instantiate: false).CardName, "Evelyn");
			popText.text = FormatText(Texts.Instance.GetText("tutorialTownCraftTitle"), body3);
			MoveCircle(Vector3.zero);
			HideCircle2();
			break;
		}
		case "townItemUpgrade":
		{
			box.transform.position = new Vector3(0f, 0f, box.transform.position.z);
			string body2 = string.Format(Texts.Instance.GetText("tutorialTownUpgrade"), Globals.Instance.GetCardData("faststrike", instantiate: false).CardName, "Magnus");
			popText.text = FormatText(Texts.Instance.GetText("tutorialTownUpgradeTitle"), body2);
			MoveCircle(Vector3.zero);
			HideCircle2();
			break;
		}
		case "townItemLoot":
		{
			box.transform.position = new Vector3(0f, 0f, box.transform.position.z);
			string body = string.Format(Texts.Instance.GetText("tutorialTownLoot"), Globals.Instance.GetCardData("spyglass", instantiate: false).CardName, "Andrin");
			popText.text = FormatText(Texts.Instance.GetText("tutorialTownLootTitle"), body);
			MoveCircle(position);
			SizeCircle(new Vector3(2.2f, 1.5f, 1f));
			HideCircle2();
			break;
		}
		case "illusionAbility":
			box.transform.position = new Vector3(Globals.Instance.sizeW * 0.25f, Globals.Instance.sizeH * 0.05f, box.transform.position.z);
			popText.text = FormatText(Texts.Instance.GetText("illusionTutorialTitle"), Texts.Instance.GetText("illusionTutorialText"));
			HideCircle2();
			MoveCircle(position);
			SizeCircle(new Vector3(4.78f, 1.56f, 1f));
			break;
		}
		CardScreenManager.Instance.ShowCardScreen(_state: false);
		ShowContent();
	}

	private void MoveCircle(Vector3 position)
	{
		circle.transform.position = position;
	}

	private void MoveCircle2(Vector3 position)
	{
		circle2.transform.position = position;
	}

	private void SizeCircle(Vector3 scale)
	{
		circle.transform.localScale = scale;
	}

	private void SizeCircle2(Vector3 scale)
	{
		circle2.transform.localScale = scale;
	}

	private void HideCircle()
	{
		circle.gameObject.SetActive(value: false);
	}

	private void HideCircle2()
	{
		circle2.gameObject.SetActive(value: false);
	}

	private void ShowContent(bool state = true)
	{
		content.gameObject.SetActive(state);
	}
}
