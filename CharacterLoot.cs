using System.Text;
using TMPro;
using UnityEngine;

public class CharacterLoot : MonoBehaviour
{
	public ItemCombatIcon item0;

	public ItemCombatIcon item1;

	public ItemCombatIcon item2;

	public ItemCombatIcon item3;

	public ItemCombatIcon item4;

	public SpriteRenderer heroSpr;

	public SpriteRenderer heroSprMask;

	public Transform mark;

	public TMP_Text playerNick;

	public TMP_Text playerNickShadow;

	public Transform buttonCharacterDeck;

	public TMP_Text buttonCharacterDeckText;

	private int heroIndex;

	private Hero hero;

	private void Awake()
	{
		buttonCharacterDeck.gameObject.SetActive(value: false);
	}

	public void AssignHero(int _heroIndex)
	{
		if (AtOManager.Instance.GetTeam().Length == 0)
		{
			return;
		}
		heroIndex = _heroIndex;
		hero = AtOManager.Instance.GetHero(heroIndex);
		if (!(heroSpr == null) && hero != null && !(hero.HeroData == null))
		{
			SpriteRenderer spriteRenderer = heroSpr;
			Sprite sprite = (heroSprMask.sprite = hero.HeroData.HeroSubClass.SpriteBorder);
			spriteRenderer.sprite = sprite;
			buttonCharacterDeck.GetComponent<BotonRollover>().auxInt = _heroIndex;
			buttonCharacterDeck.gameObject.SetActive(value: true);
			if (GameManager.Instance.IsMultiplayer())
			{
				SetNick(hero.Owner);
			}
			ShowItems();
		}
	}

	public void ShowItems()
	{
		item0.ShowIconExternal("weapon", hero);
		item1.ShowIconExternal("armor", hero);
		item2.ShowIconExternal("jewelry", hero);
		item3.ShowIconExternal("accesory", hero);
		item4.ShowIconExternal("pet", hero);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Texts.Instance.GetText("deck"));
		stringBuilder.Append("\n<color=#bbb><size=-.3>");
		stringBuilder.Append(string.Format(Texts.Instance.GetText("cardsNum"), hero.Cards.Count));
		buttonCharacterDeckText.text = stringBuilder.ToString();
	}

	public void SetNick(string ownerNick)
	{
		playerNick.gameObject.SetActive(value: true);
		TMP_Text tMP_Text = playerNick;
		string text = (playerNickShadow.text = "<" + NetworkManager.Instance.GetPlayerNickReal(ownerNick) + ">");
		tMP_Text.text = text;
		playerNick.color = Functions.HexToColor(NetworkManager.Instance.GetColorFromNick(ownerNick));
	}

	public void Activate(bool state)
	{
		if (state)
		{
			heroSprMask.transform.gameObject.SetActive(value: false);
			heroSpr.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
			mark.gameObject.SetActive(value: true);
		}
		else
		{
			heroSprMask.transform.gameObject.SetActive(value: true);
			heroSpr.transform.localScale = new Vector3(0.525f, 0.525f, 1f);
			mark.gameObject.SetActive(value: false);
		}
	}

	private void OnMouseEnter()
	{
		if (!GameManager.Instance.IsMultiplayer())
		{
			GameManager.Instance.SetCursorHover();
		}
	}

	private void OnMouseUp()
	{
		if (!GameManager.Instance.IsMultiplayer())
		{
			LootManager.Instance.ChangeCharacter(heroIndex);
		}
	}

	private void OnMouseExit()
	{
		GameManager.Instance.SetCursorPlain();
	}
}
