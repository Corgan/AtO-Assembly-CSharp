using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverCharacterDeck : MonoBehaviour
{
	private OverCharacter overCharacter;

	private int heroIndex;

	private Transform deckImage;

	private Transform deckText;

	private TMP_Text textGO;

	private Vector3 oriImgScale;

	private Scene scene;

	private string botName;

	private void Awake()
	{
		overCharacter = base.transform.parent.transform.GetComponent<OverCharacter>();
		deckImage = base.transform.GetChild(0);
		deckText = base.transform.GetChild(1);
		textGO = deckText.GetComponent<TMP_Text>();
		oriImgScale = deckImage.localScale;
		scene = SceneManager.GetActiveScene();
		botName = base.gameObject.name;
	}

	public void SetIndex(int index)
	{
		heroIndex = index;
	}
}
