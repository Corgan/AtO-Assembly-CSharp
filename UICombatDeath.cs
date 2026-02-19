using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICombatDeath : MonoBehaviour
{
	public TMP_Text textCharDeath;

	public TMP_Text textInstructions;

	public Transform mask;

	public Image imageChar;

	public Canvas canvas;

	public Transform button;

	private List<GameObject> cardsGO;

	private void Awake()
	{
		canvas.gameObject.SetActive(value: false);
		cardsGO = new List<GameObject>();
	}

	public bool IsActive()
	{
		return canvas.gameObject.activeSelf;
	}

	public void TurnOn(bool showButton = true)
	{
		MatchManager.Instance.lockHideMask = true;
		canvas.gameObject.SetActive(value: true);
		if (showButton)
		{
			button.gameObject.SetActive(value: true);
		}
		else
		{
			button.gameObject.SetActive(value: false);
		}
	}

	public void SetCharacter(Hero _hero)
	{
		string text = string.Format(Texts.Instance.GetText("deathScreenTitle"), _hero.SourceName);
		string text2 = string.Format(Texts.Instance.GetText("deathScreenBody"), 70.ToString());
		textCharDeath.text = text;
		textInstructions.text = text2;
		if (_hero.BorderSprite != null)
		{
			imageChar.gameObject.SetActive(value: true);
			imageChar.sprite = _hero.BorderSprite;
		}
		else
		{
			imageChar.gameObject.SetActive(value: false);
		}
	}

	public void SetCard(string theCard)
	{
		int num = 0;
		string text = MatchManager.Instance.CreateCardInDictionary(theCard);
		GameObject gameObject = Object.Instantiate(GameManager.Instance.CardPrefab, Vector3.zero, Quaternion.identity, base.transform);
		cardsGO.Add(gameObject);
		CardItem component = gameObject.GetComponent<CardItem>();
		gameObject.name = "TMP_" + text;
		component.SetCard(text, deckScale: false);
		component.TopLayeringOrder("UI", 32100 + 40 * num);
		component.SetLocalScale(new Vector3(1.2f, 1.2f, 1f));
		component.SetLocalPosition(new Vector3(4.9f, 1.9f, 0f));
		component.DisableTrail();
		component.HideRarityParticles();
		component.HideCardIconParticles();
		component.cardfordisplay = true;
		component.DrawBorder("red");
	}

	public void TurnOffFromButton()
	{
		button.gameObject.SetActive(value: false);
		MatchManager.Instance.DeathScreenOff();
	}

	public void TurnOff()
	{
		MatchManager.Instance.lockHideMask = false;
		canvas.gameObject.SetActive(value: false);
		for (int i = 0; i < cardsGO.Count; i++)
		{
			Object.Destroy(cardsGO[i]);
		}
		cardsGO.Clear();
		MatchManager.Instance.waitingDeathScreen = false;
	}
}
