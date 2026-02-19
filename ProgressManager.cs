using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
	private int totalCardsUnlocked = -1;

	private int totalNPCsKilled = -1;

	private int totalBossKilled = -1;

	public Transform cardProgress;

	public TMP_Text cardProgressTxt;

	public Animator cardProgressAnim;

	public Transform equipmentProgress;

	public TMP_Text equipmentProgressTxt;

	public Animator equipmentProgressAnim;

	public Transform npcProgress;

	public TMP_Text npcProgressTxt;

	public Animator npcProgressAnim;

	public Transform bossProgress;

	public TMP_Text bossProgressTxt;

	public Animator bossProgressAnim;

	private float totalTimeout;

	private float waitTimeout = 4f;

	public static ProgressManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Object.Destroy(base.gameObject);
		}
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		HideAll();
	}

	private void Init()
	{
		totalCardsUnlocked = AtOManager.Instance.unlockedCards.Count;
		totalNPCsKilled = PlayerManager.Instance.MonstersKilled;
		totalBossKilled = PlayerManager.Instance.BossesKilled;
	}

	public void CheckProgress()
	{
		totalTimeout = 0f;
		if (totalCardsUnlocked == -1)
		{
			Init();
		}
		int num = 0;
		int count = AtOManager.Instance.unlockedCards.Count;
		if (count > totalCardsUnlocked)
		{
			num = count - totalCardsUnlocked;
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				if (Globals.Instance.GetCardData(AtOManager.Instance.unlockedCards[AtOManager.Instance.unlockedCards.Count - 1 - i], instantiate: false).CardClass == Enums.CardClass.Item)
				{
					num2++;
				}
			}
			int num3 = num - num2;
			if (num3 > 0)
			{
				DoCardProgress(num3);
			}
			if (num2 > 0)
			{
				DoItemProgress(num2);
			}
		}
		totalCardsUnlocked = AtOManager.Instance.unlockedCards.Count;
		int monstersKilled = PlayerManager.Instance.MonstersKilled;
		if (monstersKilled > totalNPCsKilled)
		{
			DoNPCProgress(monstersKilled - totalNPCsKilled);
		}
		totalNPCsKilled = PlayerManager.Instance.MonstersKilled;
		int bossesKilled = PlayerManager.Instance.BossesKilled;
		if (bossesKilled > totalBossKilled)
		{
			DoBossProgress(bossesKilled - totalBossKilled);
		}
		totalBossKilled = PlayerManager.Instance.BossesKilled;
	}

	private void DoItemProgress(int _items)
	{
		if (!(AtOManager.Instance.currentMapNode == "tutorial_0") && !(AtOManager.Instance.currentMapNode == "tutorial_1") && !(AtOManager.Instance.currentMapNode == "tutorial_2"))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Texts.Instance.GetText("equipmentDiscoveredTome"));
			stringBuilder.Append(" <size=+.1><color=#FFF>+");
			stringBuilder.Append(_items);
			equipmentProgressTxt.text = stringBuilder.ToString();
			equipmentProgress.gameObject.SetActive(value: true);
			StartCoroutine(ShowElement("equipment"));
		}
	}

	private void DoCardProgress(int _cards)
	{
		if (!(AtOManager.Instance.currentMapNode == "tutorial_0") && !(AtOManager.Instance.currentMapNode == "tutorial_1") && !(AtOManager.Instance.currentMapNode == "tutorial_2"))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Texts.Instance.GetText("cardsUnlockedTome"));
			stringBuilder.Append(" <size=+.1><color=#FFF>+");
			stringBuilder.Append(_cards);
			cardProgressTxt.text = stringBuilder.ToString();
			cardProgress.gameObject.SetActive(value: true);
			StartCoroutine(ShowElement("cards"));
		}
	}

	private void DoBossProgress(int _bosses)
	{
		if (!(AtOManager.Instance.currentMapNode == "tutorial_0") && !(AtOManager.Instance.currentMapNode == "tutorial_1") && !(AtOManager.Instance.currentMapNode == "tutorial_2"))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Texts.Instance.GetText("bossesKilledTome"));
			stringBuilder.Append(" <size=+.1><color=#FFF>+");
			stringBuilder.Append(_bosses);
			bossProgressTxt.text = stringBuilder.ToString();
			bossProgress.gameObject.SetActive(value: true);
			StartCoroutine(ShowElement("boss"));
		}
	}

	private void DoNPCProgress(int _npcs)
	{
		if (!(AtOManager.Instance.currentMapNode == "tutorial_0") && !(AtOManager.Instance.currentMapNode == "tutorial_1") && !(AtOManager.Instance.currentMapNode == "tutorial_2"))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Texts.Instance.GetText("monstersKilledTome"));
			stringBuilder.Append(" <size=+.1><color=#FFF>+");
			stringBuilder.Append(_npcs);
			npcProgressTxt.text = stringBuilder.ToString();
			npcProgress.gameObject.SetActive(value: true);
			StartCoroutine(ShowElement("npcs"));
		}
	}

	public void HideAll()
	{
		cardProgress.gameObject.SetActive(value: false);
		equipmentProgress.gameObject.SetActive(value: false);
		bossProgress.gameObject.SetActive(value: false);
		npcProgress.gameObject.SetActive(value: false);
	}

	private IEnumerator ShowElement(string item)
	{
		totalTimeout += 0.25f;
		yield return Globals.Instance.WaitForSeconds(totalTimeout);
		if (item == "cards")
		{
			cardProgressAnim.ResetTrigger("show");
			cardProgressAnim.SetTrigger("show");
			yield return Globals.Instance.WaitForSeconds(waitTimeout);
			cardProgressAnim.ResetTrigger("hide");
			cardProgressAnim.SetTrigger("hide");
			yield return Globals.Instance.WaitForSeconds(2f);
			cardProgress.gameObject.SetActive(value: false);
		}
		switch (item)
		{
		case "equipment":
			equipmentProgressAnim.ResetTrigger("show");
			equipmentProgressAnim.SetTrigger("show");
			yield return Globals.Instance.WaitForSeconds(waitTimeout);
			equipmentProgressAnim.ResetTrigger("hide");
			equipmentProgressAnim.SetTrigger("hide");
			yield return Globals.Instance.WaitForSeconds(2f);
			equipmentProgress.gameObject.SetActive(value: false);
			break;
		case "npcs":
			npcProgressAnim.ResetTrigger("show");
			npcProgressAnim.SetTrigger("show");
			yield return Globals.Instance.WaitForSeconds(waitTimeout);
			npcProgressAnim.ResetTrigger("hide");
			npcProgressAnim.SetTrigger("hide");
			yield return Globals.Instance.WaitForSeconds(2f);
			npcProgress.gameObject.SetActive(value: false);
			break;
		case "boss":
			bossProgressAnim.ResetTrigger("show");
			bossProgressAnim.SetTrigger("show");
			yield return Globals.Instance.WaitForSeconds(waitTimeout);
			bossProgressAnim.ResetTrigger("hide");
			bossProgressAnim.SetTrigger("hide");
			yield return Globals.Instance.WaitForSeconds(2f);
			bossProgress.gameObject.SetActive(value: false);
			break;
		}
	}
}
