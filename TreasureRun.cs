using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class TreasureRun : MonoBehaviour
{
	public BotonGeneric bGeneric;

	public TMP_Text qText;

	public string treasureId;

	public Transform particles;

	public Transform separator;

	public bool claimed;

	private int communityGold;

	private int communityDust;

	private PlayerRun playerRun;

	private void Awake()
	{
		base.gameObject.SetActive(value: false);
		particles.gameObject.SetActive(value: false);
	}

	public void SetTreasure(PlayerRun _playerRun, int _index)
	{
		particles.gameObject.SetActive(value: false);
		if (_playerRun.GoldGained < 0)
		{
			_playerRun.GoldGained = 0;
		}
		if (_playerRun.DustGained < 0)
		{
			_playerRun.DustGained = 0;
		}
		playerRun = _playerRun;
		treasureId = _playerRun.Id;
		claimed = false;
		bGeneric.gameObject.SetActive(value: true);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<sprite name=gold> ");
		stringBuilder.Append(_playerRun.GoldGained);
		stringBuilder.Append("<br>");
		stringBuilder.Append("<sprite name=dust> ");
		stringBuilder.Append(_playerRun.DustGained);
		qText.text = stringBuilder.ToString();
		bGeneric.auxString = _playerRun.Id;
		base.gameObject.SetActive(value: true);
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, -0.6f - 1.35f * (float)_index, base.transform.localPosition.z);
		if (base.gameObject.activeSelf)
		{
			StartCoroutine(ShowTreasure());
		}
	}

	public void SetTreasureCommunity(string _id, int _gold, int _dust, int _index)
	{
		treasureId = _id;
		communityGold = _gold;
		communityDust = _dust;
		claimed = false;
		bGeneric.gameObject.SetActive(value: true);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<sprite name=gold>");
		stringBuilder.Append(_gold);
		stringBuilder.Append("<br>");
		stringBuilder.Append("<sprite name=dust>");
		stringBuilder.Append(_dust);
		qText.text = stringBuilder.ToString();
		bGeneric.auxString = _id;
		bGeneric.auxInt = 1000;
		base.gameObject.SetActive(value: true);
		if (base.transform.parent.gameObject.activeSelf && base.transform.parent.transform.parent.gameObject.activeSelf)
		{
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, -1.2f * (float)_index, base.transform.localPosition.z);
			StartCoroutine(ShowTreasure());
		}
	}

	private IEnumerator ShowTreasure()
	{
		Vector3 vectorDestination = new Vector3(Globals.Instance.sizeW * 0.2f - 3.88f * Globals.Instance.multiplierX, base.transform.localPosition.y, base.transform.localPosition.z);
		base.transform.localPosition = new Vector3(Globals.Instance.sizeW * 0.2f, base.transform.localPosition.y, base.transform.localPosition.z);
		yield return Globals.Instance.WaitForSeconds(0.2f);
		while (Vector3.Distance(base.transform.localPosition, vectorDestination) > 0.02f)
		{
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, vectorDestination, 0.5f);
			yield return null;
		}
		base.transform.localPosition = vectorDestination;
	}

	public void ClaimCommunity()
	{
		if (!claimed)
		{
			claimed = true;
			bGeneric.gameObject.SetActive(value: false);
			StartCoroutine(ClaimCo(isCommunity: true));
		}
	}

	public void Claim()
	{
		if (!claimed)
		{
			claimed = true;
			bGeneric.gameObject.SetActive(value: false);
			StartCoroutine(ClaimCo());
		}
	}

	private IEnumerator ClaimCo(bool isCommunity = false)
	{
		GameManager.Instance.PlayLibraryAudio("action_openBox");
		yield return Globals.Instance.WaitForSeconds(0.2f);
		int goldGained;
		int dust;
		if (!isCommunity)
		{
			goldGained = playerRun.GoldGained;
			dust = playerRun.DustGained;
		}
		else
		{
			goldGained = communityGold;
			dust = communityDust;
		}
		if (!GameManager.Instance.IsMultiplayer())
		{
			AtOManager.Instance.GivePlayer(0, goldGained);
			AtOManager.Instance.GivePlayer(1, dust);
			AtOManager.Instance.SaveGame();
		}
		else
		{
			AtOManager.Instance.AskForGold(NetworkManager.Instance.GetPlayerNick(), goldGained);
			yield return Globals.Instance.WaitForSeconds(0.01f);
			AtOManager.Instance.AskForDust(NetworkManager.Instance.GetPlayerNick(), dust);
		}
		SaveManager.SavePlayerData();
		particles.gameObject.SetActive(value: true);
		GameManager.Instance.PlayLibraryAudio("ui_cardupgrade");
		yield return Globals.Instance.WaitForSeconds(0.2f);
		GameManager.Instance.GenerateParticleTrail(0, base.transform.position, PlayerUIManager.Instance.GoldIconPosition());
		yield return Globals.Instance.WaitForSeconds(0.1f);
		GameManager.Instance.GenerateParticleTrail(1, base.transform.position, PlayerUIManager.Instance.DustIconPosition());
		Vector3 vectorDestination = new Vector3(Globals.Instance.sizeW * 0.3f, base.transform.localPosition.y, base.transform.localPosition.z);
		while (Vector3.Distance(base.transform.localPosition, vectorDestination) > 0.02f)
		{
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, vectorDestination, 0.5f);
			yield return null;
		}
		base.transform.localPosition = vectorDestination;
		TownManager.Instance.MoveTreasuresUp(treasureId, isCommunity);
		TownManager.Instance.LockTreasures(state: false);
		base.transform.gameObject.SetActive(value: false);
	}

	public void Hide()
	{
		base.transform.gameObject.SetActive(value: false);
	}

	public void MoveUp()
	{
		StartCoroutine(MoveUpCo());
	}

	private IEnumerator MoveUpCo()
	{
		Vector3 vectorDestination = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y + 1.4f, base.transform.localPosition.z);
		while (Vector3.Distance(base.transform.localPosition, vectorDestination) > 0.01f)
		{
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, vectorDestination, 0.5f);
			yield return null;
		}
		base.transform.localPosition = vectorDestination;
	}
}
