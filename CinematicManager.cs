using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CinematicManager : MonoBehaviour
{
	private PhotonView photonView;

	private CinematicData cinematicData;

	private GameObject cinematicGO;

	private Animator cinematicGOAnim;

	private bool playCinematic = true;

	private int totalPlayersReady;

	public TMP_Text textBottom;

	public TMP_Text textMiddle;

	public TMP_Text UIPlayers;

	public Transform buttonSkip;

	private bool skipable;

	private bool finishCinematic;

	private Coroutine hideTextCo;

	public static CinematicManager Instance { get; private set; }

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
		if (GameManager.Instance == null)
		{
			SceneStatic.LoadByName("Game");
			return;
		}
		photonView = PhotonView.Get(this);
		GameManager.Instance.SetCamera();
		NetworkManager.Instance.StartStopQueue(state: true);
		DoCinematic();
	}

	private void Update()
	{
		if (playCinematic && cinematicGOAnim != null)
		{
			float num = 0f;
			float normalizedTime = cinematicGOAnim.GetCurrentAnimatorStateInfo(0).normalizedTime;
			num = normalizedTime - Mathf.Floor(normalizedTime);
			if (finishCinematic || num > 0.95f)
			{
				EndCinematic();
			}
		}
	}

	public void DoText(string _cinematic, int _index, int _position)
	{
		if (hideTextCo != null)
		{
			StopCoroutine(hideTextCo);
			textBottom.gameObject.SetActive(value: false);
			textMiddle.gameObject.SetActive(value: false);
		}
		if (_cinematic == "intro")
		{
			switch (_position)
			{
			case 0:
				textBottom.text = Texts.Instance.GetText("cinematicIntro" + _index);
				break;
			case 1:
				textMiddle.text = Texts.Instance.GetText("cinematicIntro" + _index);
				break;
			}
		}
		else if (_cinematic == "outro")
		{
			switch (_position)
			{
			case 0:
				textBottom.text = Texts.Instance.GetText("cinematicOutro" + _index);
				break;
			case 1:
				textMiddle.text = Texts.Instance.GetText("cinematicOutro" + _index);
				break;
			}
		}
		switch (_position)
		{
		case 0:
			textBottom.color = Color.white;
			textBottom.gameObject.SetActive(value: true);
			break;
		case 1:
			textMiddle.color = Color.white;
			textMiddle.gameObject.SetActive(value: true);
			break;
		}
		hideTextCo = StartCoroutine(HideText());
	}

	private IEnumerator HideText()
	{
		yield return Globals.Instance.WaitForSeconds(6f);
		textBottom.gameObject.SetActive(value: false);
		textMiddle.gameObject.SetActive(value: false);
	}

	private void FadeOutBSO()
	{
		if (cinematicData != null && cinematicData.CinematicBSO != null)
		{
			AudioManager.Instance.FadeOutBSO();
		}
	}

	private void DoCinematic()
	{
		UIPlayers.text = "";
		buttonSkip.gameObject.SetActive(value: true);
		if (AtOManager.Instance.CinematicId != "")
		{
			cinematicData = Globals.Instance.GetCinematicData(AtOManager.Instance.CinematicId);
			if (cinematicData != null)
			{
				if (cinematicData.CinematicEndAdventure && !GameManager.Instance.IsObeliskChallenge())
				{
					AtOManager.Instance.SetAdventureCompleted(state: true);
				}
				if (cinematicData.CinematicBSO != null)
				{
					AudioManager.Instance.DoBSO("", cinematicData.CinematicBSO);
				}
				if (cinematicData.CinematicId == "intro")
				{
					StartCoroutine(SendActStartTelemetryCo());
				}
			}
		}
		AtOManager.Instance.CinematicId = "";
		if (cinematicData == null)
		{
			GameManager.Instance.ChangeScene("Map");
			return;
		}
		cinematicGO = Object.Instantiate(cinematicData.CinematicGo, Vector3.zero, Quaternion.identity);
		cinematicGOAnim = cinematicGO.GetComponent<Animator>();
		if (cinematicGOAnim == null)
		{
			cinematicGOAnim = cinematicGO.GetComponentInChildren<Animator>();
		}
		GameManager.Instance.SceneLoaded();
		AnimatorStateInfo currentAnimatorStateInfo = cinematicGOAnim.GetCurrentAnimatorStateInfo(0);
		cinematicGOAnim.Play(currentAnimatorStateInfo.shortNameHash);
		if (cinematicGOAnim != null && cinematicGOAnim.GetCurrentAnimatorClipInfo(0) != null && cinematicGOAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length > 5f)
		{
			skipable = true;
			buttonSkip.gameObject.SetActive(value: true);
		}
		else
		{
			finishCinematic = true;
			buttonSkip.gameObject.SetActive(value: false);
		}
		ControllerMovement(goingUp: true);
	}

	private IEnumerator SendActStartTelemetryCo()
	{
		yield return Globals.Instance.WaitForSeconds(1f);
	}

	private void EndCinematic()
	{
		playCinematic = false;
		cinematicGOAnim.enabled = false;
		FadeOutBSO();
		StartCoroutine(EndCinematicCo());
	}

	private IEnumerator EndCinematicCo()
	{
		yield return Globals.Instance.WaitForSeconds(0.5f);
		if (cinematicData != null)
		{
			if (cinematicData.CinematicEndAdventure && !GameManager.Instance.IsObeliskChallenge())
			{
				AtOManager.Instance.FinishGame();
				yield break;
			}
			AtOManager.Instance.fromEventCombatData = cinematicData.CinematicCombat;
			AtOManager.Instance.fromEventEventData = cinematicData.CinematicEvent;
		}
		GameManager.Instance.ChangeScene("Map");
	}

	public void SkipCinematic()
	{
		if (skipable && buttonSkip.gameObject.activeSelf)
		{
			buttonSkip.gameObject.SetActive(value: false);
			if (!GameManager.Instance.IsMultiplayer())
			{
				EndCinematic();
			}
			else
			{
				photonView.RPC("NET_SkipCinematic", RpcTarget.All);
			}
		}
	}

	[PunRPC]
	private void NET_SkipCinematic()
	{
		totalPlayersReady++;
		SetTotalPlayersReady();
	}

	private void SetTotalPlayersReady()
	{
		int numPlayers = NetworkManager.Instance.GetNumPlayers();
		UIPlayers.text = NetworkManager.Instance.GetWaitingPlayersString(totalPlayersReady, numPlayers);
		if (totalPlayersReady >= numPlayers)
		{
			EndCinematic();
		}
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false, bool shoulderLeft = false, bool shoulderRight = false)
	{
		if (goingUp || goingLeft || goingRight || goingDown)
		{
			Vector2 position = GameManager.Instance.cameraMain.WorldToScreenPoint(buttonSkip.GetChild(0).position);
			Mouse.current.WarpCursorPosition(position);
		}
	}
}
