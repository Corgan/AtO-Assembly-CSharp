using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MadnessManager : MonoBehaviour
{
	public Transform madnessWindow;

	public Transform madnessChallengeWindow;

	public Transform madnessWeeklyWindow;

	public Transform madnessSingularityWindow;

	public Transform buttonSandbox;

	public Transform buttonExit;

	public Transform buttonChallengeExit;

	public Transform buttonWeeklyExit;

	public Transform buttonSingularityExit;

	public Transform madnessConfirmButton;

	public Transform madnessChallengeConfirmButton;

	public Transform madnessSingularityConfirmButton;

	public TMP_Text madnessChallengeDescription;

	public TMP_Text madnessSingularityDescription;

	public TMP_Text mContent;

	public BotonGeneric[] mButton;

	public TMP_Text[] mCorruptorText;

	public BotonGeneric[] mCorruptor;

	public TMP_Text mFinalLevel;

	public TMP_Text mScoreMod;

	public TMP_Text mChallengeFinalLevel;

	public TMP_Text mChallengeScoreMod;

	public TMP_Text mSingularityFinalLevel;

	public TMP_Text mSingularityScoreMod;

	public Transform corruptorLocks;

	public TMP_Text[] mChallengeText;

	public BotonGeneric[] mChallengeButton;

	public TMP_Text[] mChallengeWeeklyText;

	public TMP_Text weeklyModificators;

	public TMP_Text[] mSingularityText;

	public BotonGeneric[] mSingularityButton;

	private string madnessColorOn = "FFC77E";

	private string madnessColorOff = "956984";

	private string madnessCorruptors = "";

	private int madnessSelected;

	private Coroutine showCo;

	public int controllerHorizontalIndex = -1;

	private Vector2 warpPosition = Vector2.zero;

	private List<Transform> _controllerList = new List<Transform>();

	private List<Transform> _controllerVerticalList = new List<Transform>();

	public static MadnessManager Instance { get; private set; }

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

	public void InitMadness()
	{
		string[] array = new string[11];
		string[] array2 = new string[11];
		string[] array3 = new string[11];
		string[] array4 = new string[11];
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<string, ChallengeTrait> item in Globals.Instance.ChallengeTraitsSource)
		{
			if (item.Value.IsMadnessTrait)
			{
				if (item.Value.Icon != null)
				{
					mChallengeButton[item.Value.Order].transform.parent.GetComponent<ChallengeMadness>().SetIcon(item.Value.Icon);
				}
				stringBuilder.Clear();
				stringBuilder.Append("<color=#FFF>");
				stringBuilder.Append(Texts.Instance.GetText(item.Value.Id));
				stringBuilder.Append("</color>");
				array[item.Value.Order] = stringBuilder.ToString();
				array2[item.Value.Order] = Texts.Instance.GetText(item.Value.Id + "desc");
			}
			if (item.Value.IsSingularityTrait)
			{
				if (item.Value.Icon != null)
				{
					mSingularityButton[item.Value.OrderSingularity].transform.parent.GetComponent<ChallengeMadness>().SetIcon(item.Value.Icon);
				}
				stringBuilder.Clear();
				stringBuilder.Append("<color=#FFF>");
				stringBuilder.Append(Texts.Instance.GetText(item.Value.Id));
				stringBuilder.Append("</color>");
				array3[item.Value.OrderSingularity] = stringBuilder.ToString();
				array4[item.Value.OrderSingularity] = Texts.Instance.GetText(item.Value.Id + "desc");
			}
		}
		for (int i = 0; i < mChallengeText.Length; i++)
		{
			mChallengeText[i].text = array[i];
			mChallengeText[i].GetComponent<PopupText>().text = array2[i];
		}
		for (int j = 0; j < mSingularityText.Length; j++)
		{
			mSingularityText[j].text = array3[j];
			mSingularityText[j].GetComponent<PopupText>().text = array4[j];
		}
		madnessWindow.gameObject.SetActive(value: false);
		madnessChallengeWindow.gameObject.SetActive(value: false);
		madnessWeeklyWindow.gameObject.SetActive(value: false);
		madnessSingularityWindow.gameObject.SetActive(value: false);
	}

	public bool IsActive()
	{
		if (GameManager.Instance.IsGameAdventure())
		{
			return madnessWindow.gameObject.activeSelf;
		}
		if (GameManager.Instance.IsSingularity())
		{
			return madnessSingularityWindow.gameObject.activeSelf;
		}
		if (GameManager.Instance.IsWeeklyChallenge())
		{
			return madnessWeeklyWindow.gameObject.activeSelf;
		}
		return madnessChallengeWindow.gameObject.activeSelf;
	}

	public void RefreshValues(string masterCorruptors = "")
	{
		SetMadnessMaster();
		madnessCorruptors = masterCorruptors;
		if (madnessCorruptors.Length != mCorruptor.Length)
		{
			madnessCorruptors = "";
		}
		SetCorruptors("", fromMaster: true);
		if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
		{
			if (GameManager.Instance.IsSingularity())
			{
				SetMadnessSingularityRows(HeroSelectionManager.Instance.SingularityMadnessValueMaster);
			}
			else if (GameManager.Instance.IsObeliskChallenge() && !GameManager.Instance.IsWeeklyChallenge())
			{
				SetMadnessChallengeRows(HeroSelectionManager.Instance.ObeliskMadnessValueMaster);
			}
		}
		SetFinalLevel();
	}

	public void CloseMadness()
	{
		if (IsActive())
		{
			if (madnessWindow.gameObject.activeSelf)
			{
				madnessWindow.gameObject.SetActive(value: false);
			}
			else if (madnessChallengeWindow.gameObject.activeSelf)
			{
				madnessChallengeWindow.gameObject.SetActive(value: false);
			}
			else if (madnessWeeklyWindow.gameObject.activeSelf)
			{
				madnessWeeklyWindow.gameObject.SetActive(value: false);
			}
			else if (madnessSingularityWindow.gameObject.activeSelf)
			{
				madnessSingularityWindow.gameObject.SetActive(value: false);
			}
		}
		PopupManager.Instance.ClosePopup();
	}

	public void ShowMadness()
	{
		if (showCo != null)
		{
			StopCoroutine(showCo);
		}
		showCo = StartCoroutine(ShowMadnessCo());
	}

	private IEnumerator ShowMadnessCo()
	{
		if (IsActive())
		{
			CloseMadness();
			if ((bool)CardCraftManager.Instance)
			{
				CardCraftManager.Instance.ShowSearch(state: true);
			}
			yield break;
		}
		mScoreMod.gameObject.SetActive(value: false);
		if (GameManager.Instance.IsGameAdventure())
		{
			madnessWindow.gameObject.SetActive(value: true);
			yield return new WaitForSeconds(0.01f);
			if ((bool)HeroSelectionManager.Instance && !GameManager.Instance.IsLoadingGame() && (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster()))
			{
				madnessConfirmButton.gameObject.SetActive(value: true);
			}
			else
			{
				madnessConfirmButton.gameObject.SetActive(value: false);
			}
			SetMadness();
			SetCorruptors();
			if (PlayerManager.Instance.NgLevel == 0)
			{
				corruptorLocks.gameObject.SetActive(value: true);
				for (int i = 0; i < mCorruptor.Length; i++)
				{
					mCorruptor[i].Disable();
					TurnOffCorruptor(i);
				}
			}
			else
			{
				corruptorLocks.gameObject.SetActive(value: false);
				for (int j = 0; j < mCorruptor.Length; j++)
				{
					mCorruptor[j].Enable();
				}
			}
			if (!HeroSelectionManager.Instance || GameManager.Instance.IsLoadingGame())
			{
				for (int k = 0; k < mCorruptor.Length; k++)
				{
					mCorruptor[k].Disable();
				}
				for (int l = 0; l < mButton.Length; l++)
				{
					mButton[l].Disable();
				}
				if (madnessSelected > -1)
				{
					mButton[madnessSelected].ShowBackgroundDisable(state: false);
				}
			}
		}
		else if (GameManager.Instance.IsSingularity())
		{
			madnessSingularityWindow.gameObject.SetActive(value: true);
			if ((bool)HeroSelectionManager.Instance && !GameManager.Instance.IsLoadingGame() && (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster()))
			{
				madnessSingularityConfirmButton.gameObject.SetActive(value: true);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(Texts.Instance.GetText("madnessChallengeSelect"));
				stringBuilder.Append(" ");
				stringBuilder.Append(Texts.Instance.GetText("madnessSingularityBeat"));
				madnessSingularityDescription.text = stringBuilder.ToString();
			}
			else
			{
				madnessSingularityConfirmButton.gameObject.SetActive(value: false);
				madnessSingularityDescription.text = "";
			}
			SetMadness();
			if (!HeroSelectionManager.Instance || GameManager.Instance.IsLoadingGame() || (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster()))
			{
				for (int m = 0; m < mSingularityButton.Length; m++)
				{
					mSingularityButton[m].Disable();
				}
				if (madnessSelected > -1)
				{
					mSingularityButton[madnessSelected].ShowBackgroundDisable(state: false);
				}
			}
		}
		else if (GameManager.Instance.IsWeeklyChallenge())
		{
			ChallengeData weeklyData = Globals.Instance.GetWeeklyData(AtOManager.Instance.GetWeekly());
			if (weeklyData != null && weeklyData.Traits != null)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				for (int n = 0; n < weeklyData.Traits.Count; n++)
				{
					mChallengeWeeklyText[n].transform.parent.gameObject.SetActive(value: true);
					ChallengeTrait challengeTrait = weeklyData.Traits[n];
					if (challengeTrait.Icon != null)
					{
						mChallengeWeeklyText[n].transform.parent.GetComponent<ChallengeMadness>().SetIcon(challengeTrait.Icon);
					}
					stringBuilder2.Append("<color=#FFF>");
					stringBuilder2.Append(Texts.Instance.GetText(challengeTrait.Id));
					stringBuilder2.Append("</color>");
					mChallengeWeeklyText[n].text = stringBuilder2.ToString();
					mChallengeWeeklyText[n].GetComponent<PopupText>().text = Texts.Instance.GetText(challengeTrait.Id + "desc");
					stringBuilder2.Clear();
				}
				for (int num = weeklyData.Traits.Count; num < mChallengeWeeklyText.Length; num++)
				{
					mChallengeWeeklyText[num].transform.parent.gameObject.SetActive(value: false);
				}
				stringBuilder2.Clear();
				stringBuilder2.Append("<size=+2><color=#C19ED9>");
				stringBuilder2.Append(AtOManager.Instance.GetWeeklyName(AtOManager.Instance.GetWeekly()));
				stringBuilder2.Append("</color></size>\n");
				stringBuilder2.Append(Texts.Instance.GetText("weeklyModificatorsDescription"));
				weeklyModificators.text = stringBuilder2.ToString();
			}
			madnessWeeklyWindow.gameObject.SetActive(value: true);
		}
		else
		{
			madnessChallengeWindow.gameObject.SetActive(value: true);
			if ((bool)HeroSelectionManager.Instance && !GameManager.Instance.IsLoadingGame() && (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster()))
			{
				madnessChallengeConfirmButton.gameObject.SetActive(value: true);
				StringBuilder stringBuilder3 = new StringBuilder();
				stringBuilder3.Append(Texts.Instance.GetText("madnessChallengeSelect"));
				stringBuilder3.Append(" ");
				stringBuilder3.Append(Texts.Instance.GetText("madnessChallengeBeat"));
				madnessChallengeDescription.text = stringBuilder3.ToString();
			}
			else
			{
				madnessChallengeDescription.text = "";
				madnessChallengeConfirmButton.gameObject.SetActive(value: false);
			}
			SetMadness();
			if (!HeroSelectionManager.Instance || GameManager.Instance.IsLoadingGame() || (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster()))
			{
				for (int num2 = 0; num2 < mChallengeButton.Length; num2++)
				{
					mChallengeButton[num2].Disable();
				}
				if (madnessSelected > -1)
				{
					mChallengeButton[madnessSelected].ShowBackgroundDisable(state: false);
				}
			}
		}
		if ((bool)CardCraftManager.Instance)
		{
			CardCraftManager.Instance.ShowSearch(state: false);
		}
	}

	public void MadnessConfirm()
	{
		if ((bool)HeroSelectionManager.Instance)
		{
			string text = "";
			if (GameManager.Instance.IsMultiplayer() && NetworkManager.Instance.IsMaster())
			{
				text = "Coop";
			}
			if (GameManager.Instance.IsGameAdventure())
			{
				HeroSelectionManager.Instance.NgValue = madnessSelected;
				HeroSelectionManager.Instance.NgValueMaster = madnessSelected;
				HeroSelectionManager.Instance.NgCorruptors = madnessCorruptors;
				SaveManager.SaveIntoPrefsInt("madnessLevel" + text, madnessSelected);
				SaveManager.SaveIntoPrefsString("madnessCorruptors" + text, madnessCorruptors);
				HeroSelectionManager.Instance.SetMadnessLevel();
			}
			else if (GameManager.Instance.IsSingularity())
			{
				HeroSelectionManager.Instance.SingularityMadnessValue = madnessSelected;
				HeroSelectionManager.Instance.SingularityMadnessValueMaster = madnessSelected;
				SaveManager.SaveIntoPrefsInt("singularityMadness" + text, madnessSelected);
				HeroSelectionManager.Instance.SetSingularityMadnessLevel();
			}
			else if (!GameManager.Instance.IsWeeklyChallenge())
			{
				HeroSelectionManager.Instance.ObeliskMadnessValue = madnessSelected;
				HeroSelectionManager.Instance.ObeliskMadnessValueMaster = madnessSelected;
				SaveManager.SaveIntoPrefsInt("obeliskMadness" + text, madnessSelected);
				HeroSelectionManager.Instance.SetObeliskMadnessLevel();
			}
			ShowMadness();
		}
	}

	public string InitCorruptors()
	{
		string text = "";
		for (int i = 0; i < mCorruptor.Length; i++)
		{
			text += "0";
			mCorruptorText[i].text = Functions.PregReplaceIcon(Texts.Instance.GetText("madnessCorruptor" + i));
			if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
			{
				mCorruptor[i].Disable();
			}
		}
		return text;
	}

	private void SetMadnessMaster()
	{
		if (!GameManager.Instance.IsMultiplayer() || !(HeroSelectionManager.Instance != null))
		{
			return;
		}
		if (GameManager.Instance.IsGameAdventure())
		{
			for (int i = 0; i < mButton.Length; i++)
			{
				if (HeroSelectionManager.Instance.NgValueMaster == i)
				{
					mButton[i].transform.Find("Master").gameObject.SetActive(value: true);
				}
				else
				{
					mButton[i].transform.Find("Master").gameObject.SetActive(value: false);
				}
			}
		}
		else if (GameManager.Instance.IsSingularity())
		{
			for (int j = 0; j < mSingularityButton.Length; j++)
			{
				if (HeroSelectionManager.Instance.SingularityMadnessValueMaster == j)
				{
					mSingularityButton[j].transform.Find("Master").gameObject.SetActive(value: true);
					if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
					{
						mSingularityButton[j].SetBackgroundColor(new Color(1f, 0.78f, 0.49f));
						mSingularityButton[j].SetBorderColor(new Color(1f, 0.78f, 0.49f));
						mSingularityButton[j].transform.localPosition = new Vector3(-5.8f, mSingularityButton[j].transform.localPosition.y, mSingularityButton[j].transform.localPosition.z);
					}
				}
				else
				{
					mSingularityButton[j].transform.Find("Master").gameObject.SetActive(value: false);
					if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
					{
						mSingularityButton[j].ShowBackgroundDisable(state: true);
						mSingularityButton[j].SetColor();
						mSingularityButton[j].transform.localPosition = new Vector3(-5.6f, mSingularityButton[j].transform.localPosition.y, mSingularityButton[j].transform.localPosition.z);
					}
				}
			}
		}
		else
		{
			if (GameManager.Instance.IsWeeklyChallenge())
			{
				return;
			}
			for (int k = 0; k < mChallengeButton.Length; k++)
			{
				if (HeroSelectionManager.Instance.ObeliskMadnessValueMaster == k)
				{
					mChallengeButton[k].transform.Find("Master").gameObject.SetActive(value: true);
					if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
					{
						mChallengeButton[k].SetBackgroundColor(new Color(1f, 0.78f, 0.49f));
						mChallengeButton[k].SetBorderColor(new Color(1f, 0.78f, 0.49f));
						mChallengeButton[k].transform.localPosition = new Vector3(-5.8f, mChallengeButton[k].transform.localPosition.y, mChallengeButton[k].transform.localPosition.z);
					}
				}
				else
				{
					mChallengeButton[k].transform.Find("Master").gameObject.SetActive(value: false);
					if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
					{
						mChallengeButton[k].ShowBackgroundDisable(state: true);
						mChallengeButton[k].SetColor();
						mChallengeButton[k].transform.localPosition = new Vector3(-5.6f, mChallengeButton[k].transform.localPosition.y, mChallengeButton[k].transform.localPosition.z);
					}
				}
			}
		}
	}

	private void SetMadness()
	{
		if (GameManager.Instance.IsGameAdventure())
		{
			if ((bool)HeroSelectionManager.Instance)
			{
				SelectMadness(HeroSelectionManager.Instance.NgValue);
			}
			else
			{
				SelectMadness(AtOManager.Instance.GetNgPlus());
			}
		}
		else if (GameManager.Instance.IsSingularity())
		{
			if ((bool)HeroSelectionManager.Instance)
			{
				SelectMadness(HeroSelectionManager.Instance.SingularityMadnessValue);
			}
			else
			{
				SelectMadness(AtOManager.Instance.GetSingularityMadness());
			}
		}
		else if (!GameManager.Instance.IsWeeklyChallenge())
		{
			if ((bool)HeroSelectionManager.Instance)
			{
				SelectMadness(HeroSelectionManager.Instance.ObeliskMadnessValue);
			}
			else
			{
				SelectMadness(AtOManager.Instance.GetObeliskMadness());
			}
		}
	}

	public void SetCorruptors(string strCorruptors = "", bool fromMaster = false)
	{
		if (!GameManager.Instance.IsGameAdventure())
		{
			return;
		}
		if (strCorruptors == "" && madnessCorruptors == "")
		{
			strCorruptors = InitCorruptors();
		}
		for (int i = 0; i < mCorruptor.Length; i++)
		{
			if (IsMadnessCorruptorSelected(i))
			{
				TurnOnCorruptor(i, fromMaster);
			}
			else
			{
				TurnOffCorruptor(i, fromMaster);
			}
		}
	}

	private void ResetMadnessButtons()
	{
		if (GameManager.Instance.IsGameAdventure())
		{
			for (int i = 0; i < mButton.Length; i++)
			{
				mButton[i].Enable();
				mButton[i].SetColor();
				mButton[i].transform.localPosition = new Vector3(0f, mButton[i].transform.localPosition.y, mButton[i].transform.localPosition.z);
				mButton[i].transform.Find("Lock").gameObject.SetActive(value: false);
				mButton[i].transform.Find("Master").gameObject.SetActive(value: false);
			}
		}
		else if (GameManager.Instance.IsSingularity())
		{
			for (int j = 0; j < mSingularityButton.Length; j++)
			{
				mSingularityButton[j].Enable();
				mSingularityButton[j].SetColor();
				mSingularityButton[j].transform.localPosition = new Vector3(-5.6f, mSingularityButton[j].transform.localPosition.y, mSingularityButton[j].transform.localPosition.z);
				mSingularityButton[j].transform.Find("Lock").gameObject.SetActive(value: false);
				mSingularityButton[j].transform.Find("Master").gameObject.SetActive(value: false);
			}
		}
		else if (!GameManager.Instance.IsWeeklyChallenge())
		{
			for (int k = 0; k < mChallengeButton.Length; k++)
			{
				mChallengeButton[k].Enable();
				mChallengeButton[k].SetColor();
				mChallengeButton[k].transform.localPosition = new Vector3(-5.6f, mChallengeButton[k].transform.localPosition.y, mChallengeButton[k].transform.localPosition.z);
				mChallengeButton[k].transform.Find("Lock").gameObject.SetActive(value: false);
				mChallengeButton[k].transform.Find("Master").gameObject.SetActive(value: false);
			}
		}
		SetMadnessMaster();
	}

	private void DisableMadnessButton(int value)
	{
		if (GameManager.Instance.IsGameAdventure())
		{
			mButton[value].ShowBorder(state: false);
			mButton[value].SetBackgroundColor(new Color(0.3f, 0.3f, 0.3f));
			mButton[value].SetBorderColor(new Color(0.3f, 0.3f, 0.3f));
			mButton[value].transform.Find("Lock").gameObject.SetActive(value: true);
		}
		else if (GameManager.Instance.IsSingularity())
		{
			mSingularityButton[value].ShowBorder(state: false);
			mSingularityButton[value].SetBackgroundColor(new Color(0.3f, 0.3f, 0.3f));
			mSingularityButton[value].SetBorderColor(new Color(0.3f, 0.3f, 0.3f));
			mSingularityButton[value].transform.Find("Lock").gameObject.SetActive(value: true);
			mSingularityButton[value].transform.parent.GetComponent<ChallengeMadness>().SetDisable();
		}
		else if (!GameManager.Instance.IsWeeklyChallenge())
		{
			mChallengeButton[value].ShowBorder(state: false);
			mChallengeButton[value].SetBackgroundColor(new Color(0.3f, 0.3f, 0.3f));
			mChallengeButton[value].SetBorderColor(new Color(0.3f, 0.3f, 0.3f));
			mChallengeButton[value].transform.Find("Lock").gameObject.SetActive(value: true);
			mChallengeButton[value].transform.parent.GetComponent<ChallengeMadness>().SetDisable();
		}
	}

	public void SelectMadness(int value)
	{
		ResetMadnessButtons();
		int num = 0;
		if (GameManager.Instance.IsGameAdventure())
		{
			mButton[value].transform.localPosition = new Vector3(-0.25f, mButton[value].transform.localPosition.y, mButton[value].transform.localPosition.z);
			mButton[value].SetBackgroundColor(new Color(1f, 0.78f, 0.49f));
			mButton[value].SetBorderColor(new Color(1f, 0.78f, 0.49f));
			num = PlayerManager.Instance.NgLevel;
			for (int i = 1; i < mButton.Length; i++)
			{
				if (num < i)
				{
					DisableMadnessButton(i);
				}
			}
		}
		else if (GameManager.Instance.IsSingularity())
		{
			mSingularityButton[value].transform.localPosition = new Vector3(-5.8f, mSingularityButton[value].transform.localPosition.y, mSingularityButton[value].transform.localPosition.z);
			mSingularityButton[value].SetBackgroundColor(new Color(1f, 0.78f, 0.49f));
			mSingularityButton[value].SetBorderColor(new Color(1f, 0.78f, 0.49f));
			num = PlayerManager.Instance.SingularityMadnessLevel;
			SetMadnessSingularityRows(value);
		}
		else if (!GameManager.Instance.IsWeeklyChallenge())
		{
			mChallengeButton[value].transform.localPosition = new Vector3(-5.8f, mChallengeButton[value].transform.localPosition.y, mChallengeButton[value].transform.localPosition.z);
			mChallengeButton[value].SetBackgroundColor(new Color(1f, 0.78f, 0.49f));
			mChallengeButton[value].SetBorderColor(new Color(1f, 0.78f, 0.49f));
			num = PlayerManager.Instance.ObeliskMadnessLevel;
			SetMadnessChallengeRows(value);
		}
		mContent.text = Functions.GetMadnessBonusText(value);
		if (value <= num)
		{
			madnessSelected = value;
		}
		SetFinalLevel();
	}

	private void SetFinalLevel()
	{
		int lvl = madnessSelected;
		string corr = madnessCorruptors;
		int num = CalculateMadnessTotal(lvl, corr);
		if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
		{
			if (GameManager.Instance.IsGameAdventure())
			{
				lvl = ((!(HeroSelectionManager.Instance != null)) ? AtOManager.Instance.GetNgPlus() : HeroSelectionManager.Instance.NgValueMaster);
				num = CalculateMadnessTotal(lvl, corr);
			}
			else if (GameManager.Instance.IsSingularity())
			{
				lvl = ((!(HeroSelectionManager.Instance != null)) ? AtOManager.Instance.GetSingularityMadness() : HeroSelectionManager.Instance.SingularityMadnessValueMaster);
				num = lvl;
			}
			else if (!GameManager.Instance.IsWeeklyChallenge())
			{
				lvl = ((!(HeroSelectionManager.Instance != null)) ? AtOManager.Instance.GetObeliskMadness() : HeroSelectionManager.Instance.ObeliskMadnessValueMaster);
				num = lvl;
			}
		}
		TMP_Text tMP_Text = mFinalLevel;
		TMP_Text tMP_Text2 = mChallengeFinalLevel;
		string text = (mSingularityFinalLevel.text = num.ToString());
		string text3 = (tMP_Text2.text = text);
		tMP_Text.text = text3;
		if (num > 0)
		{
			mScoreMod.gameObject.SetActive(value: true);
			mChallengeScoreMod.gameObject.SetActive(value: true);
			mSingularityScoreMod.gameObject.SetActive(value: true);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Texts.Instance.GetText("finalScore"));
			stringBuilder.Append(" <color=#AAA>(+");
			stringBuilder.Append(Functions.GetMadnessScoreMultiplier(num, GameManager.Instance.IsGameAdventure()));
			if (Functions.SpaceBeforePercentSign())
			{
				stringBuilder.Append(" ");
			}
			stringBuilder.Append("%)");
			TMP_Text tMP_Text3 = mScoreMod;
			TMP_Text tMP_Text4 = mChallengeScoreMod;
			text = (mSingularityScoreMod.text = stringBuilder.ToString());
			text3 = (tMP_Text4.text = text);
			tMP_Text3.text = text3;
		}
		else
		{
			mScoreMod.gameObject.SetActive(value: false);
			mChallengeScoreMod.gameObject.SetActive(value: false);
			mSingularityScoreMod.gameObject.SetActive(value: false);
		}
	}

	private void SetMadnessChallengeRows(int _value)
	{
		int obeliskMadnessLevel = PlayerManager.Instance.ObeliskMadnessLevel;
		for (int i = 0; i < mChallengeButton.Length; i++)
		{
			if (i < _value)
			{
				mChallengeButton[i].transform.parent.GetComponent<ChallengeMadness>().SetActive();
			}
			if (i == _value)
			{
				mChallengeButton[i].transform.parent.GetComponent<ChallengeMadness>().SetActive();
			}
			if (i > _value)
			{
				mChallengeButton[i].transform.parent.GetComponent<ChallengeMadness>().SetDefault();
			}
			if (obeliskMadnessLevel < i)
			{
				DisableMadnessButton(i);
				mChallengeButton[i].Disable();
			}
			else
			{
				mChallengeButton[i].Enable();
			}
			if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
			{
				mChallengeButton[i].Disable();
				mChallengeButton[_value].ShowBackgroundDisable(state: false);
			}
		}
	}

	private void SetMadnessSingularityRows(int _value)
	{
		int singularityMadnessLevel = PlayerManager.Instance.SingularityMadnessLevel;
		for (int i = 0; i < mSingularityButton.Length; i++)
		{
			if (i < _value)
			{
				mSingularityButton[i].transform.parent.GetComponent<ChallengeMadness>().SetActive();
			}
			if (i == _value)
			{
				mSingularityButton[i].transform.parent.GetComponent<ChallengeMadness>().SetActive();
			}
			if (i > _value)
			{
				mSingularityButton[i].transform.parent.GetComponent<ChallengeMadness>().SetDefault();
			}
			if (singularityMadnessLevel < i)
			{
				DisableMadnessButton(i);
				mSingularityButton[i].Disable();
			}
			else
			{
				mSingularityButton[i].Enable();
			}
			if (GameManager.Instance.IsMultiplayer() && !NetworkManager.Instance.IsMaster())
			{
				mSingularityButton[i].Disable();
				mSingularityButton[_value].ShowBackgroundDisable(state: false);
			}
		}
	}

	public int CalculateMadnessTotal(int lvl, string corr = "")
	{
		int num = 0;
		if (corr != "")
		{
			num = GetMadnessCorruptorNumber(corr);
		}
		return lvl + num;
	}

	public int GetMadnessCorruptorNumber(string corr = "")
	{
		if (corr == "" || corr == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < corr.Length; i++)
		{
			if (corr[i] == '1')
			{
				num++;
			}
		}
		return num;
	}

	public void SelectMadnessCorruptor(int index, bool fromButton = true)
	{
		if (!(!HeroSelectionManager.Instance && fromButton))
		{
			if (IsMadnessCorruptorSelected(index))
			{
				TurnOffCorruptor(index);
			}
			else
			{
				TurnOnCorruptor(index);
			}
			CalculateMadnessTotal(madnessSelected, madnessCorruptors);
		}
	}

	private void TurnOnCorruptor(int index, bool fromMaster = false)
	{
		if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster() || fromMaster)
		{
			SetMadnessCorruptor(index, value: true);
			mCorruptor[index].SetText("X");
			mCorruptor[index].SetBackgroundColor(Functions.HexToColor(madnessColorOn));
			mCorruptorText[index].color = Functions.HexToColor(madnessColorOn);
		}
	}

	private void TurnOffCorruptor(int index, bool fromMaster = false)
	{
		if (!GameManager.Instance.IsMultiplayer() || NetworkManager.Instance.IsMaster() || fromMaster)
		{
			SetMadnessCorruptor(index, value: false);
			mCorruptor[index].SetText("");
			mCorruptor[index].SetBackgroundColor(Functions.HexToColor(madnessColorOff));
			mCorruptorText[index].color = Functions.HexToColor(madnessColorOff);
		}
	}

	public bool IsMadnessTraitActive(string corruptor)
	{
		if (!AtOManager.Instance.IsZoneAffectedByMadness())
		{
			return false;
		}
		int num = -1;
		switch (corruptor)
		{
		case "impedingdoom":
			num = 0;
			break;
		case "decadence":
			num = 1;
			break;
		case "restrictedpower":
			num = 2;
			break;
		case "resistantmonsters":
			num = 3;
			break;
		case "poverty":
			num = 4;
			break;
		case "overchargedmonsters":
			num = 5;
			break;
		case "randomcombats":
			num = 6;
			break;
		case "despair":
			num = 7;
			break;
		case "equalizer":
			num = 8;
			break;
		}
		if (num > -1)
		{
			return IsMadnessCorruptorSelected(num);
		}
		return false;
	}

	public bool IsMadnessCorruptorSelected(int index)
	{
		string text = "";
		text = ((!(HeroSelectionManager.Instance != null)) ? AtOManager.Instance.GetMadnessCorruptors() : madnessCorruptors);
		if (text == "" || text == null)
		{
			return false;
		}
		if (index < text.Length)
		{
			_ = text[index];
			if (text[index] == '1')
			{
				return true;
			}
		}
		return false;
	}

	private void SetMadnessCorruptor(int index, bool value)
	{
		if (madnessCorruptors == null)
		{
			madnessCorruptors = "";
		}
		if (madnessCorruptors.Trim() == "")
		{
			for (int i = 0; i < mCorruptor.Length; i++)
			{
				madnessCorruptors += "0";
			}
		}
		if (!(madnessCorruptors == ""))
		{
			StringBuilder stringBuilder = new StringBuilder(madnessCorruptors);
			if (value)
			{
				stringBuilder[index] = '1';
			}
			else
			{
				stringBuilder[index] = '0';
			}
			madnessCorruptors = stringBuilder.ToString();
			SetFinalLevel();
		}
	}

	public void ControllerMovement(bool goingUp = false, bool goingRight = false, bool goingDown = false, bool goingLeft = false, int absolutePosition = -1)
	{
		_controllerList.Clear();
		for (int i = 0; i < mButton.Length; i++)
		{
			if (Functions.TransformIsVisible(mButton[i].transform))
			{
				_controllerList.Add(mButton[i].transform);
			}
		}
		for (int j = 0; j < mCorruptor.Length; j++)
		{
			if (Functions.TransformIsVisible(mCorruptor[j].transform))
			{
				_controllerList.Add(mCorruptor[j].transform);
			}
		}
		for (int k = 0; k < mChallengeButton.Length; k++)
		{
			if (Functions.TransformIsVisible(mChallengeButton[k].transform))
			{
				_controllerList.Add(mChallengeButton[k].transform);
			}
		}
		if (Functions.TransformIsVisible(buttonSandbox))
		{
			_controllerList.Add(buttonSandbox);
		}
		if (Functions.TransformIsVisible(madnessConfirmButton))
		{
			_controllerList.Add(madnessConfirmButton);
		}
		if (Functions.TransformIsVisible(madnessChallengeConfirmButton))
		{
			_controllerList.Add(madnessChallengeConfirmButton);
		}
		if (Functions.TransformIsVisible(buttonExit))
		{
			_controllerList.Add(buttonExit);
		}
		if (Functions.TransformIsVisible(buttonChallengeExit))
		{
			_controllerList.Add(buttonChallengeExit);
		}
		if (Functions.TransformIsVisible(buttonWeeklyExit))
		{
			_controllerList.Add(buttonWeeklyExit);
		}
		controllerHorizontalIndex = Functions.GetListClosestIndexToMousePosition(_controllerList);
		controllerHorizontalIndex = Functions.GetClosestIndexBasedOnDirection(_controllerList, controllerHorizontalIndex, goingUp, goingRight, goingDown, goingLeft);
		if (_controllerList[controllerHorizontalIndex] != null)
		{
			warpPosition = GameManager.Instance.cameraMain.WorldToScreenPoint(_controllerList[controllerHorizontalIndex].position);
			Mouse.current.WarpCursorPosition(warpPosition);
		}
	}
}
