using TMPro;
using UnityEngine;

public class UIEnergySelector : MonoBehaviour
{
	public TMP_Text textAssignEnergy;

	public TMP_Text textInstructions;

	public Transform mask;

	public Canvas canvas;

	public Transform buttonMore;

	public Transform buttonLess;

	public Transform buttonAccept;

	private int maxEnergy = 10;

	private int maxEnergyToBeAssigned = 10;

	private int currentEnergy;

	private string instructions;

	private string instructionsMax;

	private void Awake()
	{
		canvas.gameObject.SetActive(value: false);
	}

	private void Start()
	{
		string text = "110%";
		if (Globals.Instance?.CurrentLang == "zh-CN" || Globals.Instance?.CurrentLang == "zh-TW" || Globals.Instance?.CurrentLang == "jp")
		{
			text = "50%";
		}
		instructions = Texts.Instance.GetText("chooseEnergy") + "<line-height=" + text + "><br></line-height><size=3><color=#bbb>" + Texts.Instance.GetText("chooseEnergyAvailable") + " <color=green>%energy%</color></size></color>";
		instructionsMax = "\n<size=3><color=#bbb>" + Texts.Instance.GetText("chooseEnergyMax") + " <color=green>%energyMax%</color>";
	}

	public bool IsActive()
	{
		return canvas.gameObject.activeSelf;
	}

	private void TextEnergy()
	{
		textAssignEnergy.text = currentEnergy.ToString();
	}

	public void TurnOn(int energy, int maxToBeAssigned = 0)
	{
		MatchManager.Instance.ShowMask(state: true);
		MatchManager.Instance.lockHideMask = true;
		if (GameManager.Instance.IsMultiplayer() && !MatchManager.Instance.IsYourTurn())
		{
			buttonMore.gameObject.SetActive(value: false);
			buttonLess.gameObject.SetActive(value: false);
			buttonAccept.gameObject.SetActive(value: false);
		}
		else
		{
			buttonMore.gameObject.SetActive(value: true);
			buttonLess.gameObject.SetActive(value: true);
			buttonAccept.gameObject.SetActive(value: true);
		}
		currentEnergy = 0;
		maxEnergy = energy;
		if (maxToBeAssigned == 0)
		{
			maxToBeAssigned = 10;
		}
		maxEnergyToBeAssigned = maxToBeAssigned;
		string text = instructions.Replace("%energy%", energy.ToString());
		if (maxToBeAssigned > 0)
		{
			text += instructionsMax.Replace("%energyMax%", maxToBeAssigned.ToString());
		}
		textInstructions.text = text;
		TextEnergy();
		canvas.gameObject.SetActive(value: true);
	}

	public void TurnOff()
	{
		MatchManager.Instance.lockHideMask = false;
		MatchManager.Instance.ShowMask(state: false);
		canvas.gameObject.SetActive(value: false);
	}

	public void AssignEnergyZero()
	{
		if ((!(CardScreenManager.Instance != null) || !CardScreenManager.Instance.IsActive()) && !TomeManager.Instance.IsActive() && buttonAccept.gameObject.activeSelf)
		{
			buttonMore.gameObject.SetActive(value: false);
			buttonLess.gameObject.SetActive(value: false);
			buttonAccept.gameObject.SetActive(value: false);
			MatchManager.Instance.AssignEnergyAction(0);
		}
	}

	public void AssignEnergyAction()
	{
		if ((!(CardScreenManager.Instance != null) || !CardScreenManager.Instance.IsActive()) && !TomeManager.Instance.IsActive() && buttonAccept.gameObject.activeSelf)
		{
			buttonMore.gameObject.SetActive(value: false);
			buttonLess.gameObject.SetActive(value: false);
			buttonAccept.gameObject.SetActive(value: false);
			MatchManager.Instance.AssignEnergyAction(currentEnergy);
		}
	}

	public void AssignEnergyFromOutside(int _energy)
	{
		if (_energy > maxEnergy)
		{
			_energy = maxEnergy;
		}
		if (_energy > maxEnergyToBeAssigned)
		{
			_energy = maxEnergyToBeAssigned;
		}
		currentEnergy = _energy;
		TextEnergy();
	}

	public void AssignEnergyMore()
	{
		if ((!(CardScreenManager.Instance != null) || !CardScreenManager.Instance.IsActive()) && !TomeManager.Instance.IsActive())
		{
			currentEnergy++;
			if (currentEnergy > maxEnergy)
			{
				currentEnergy = maxEnergy;
			}
			if (currentEnergy > maxEnergyToBeAssigned)
			{
				currentEnergy = maxEnergyToBeAssigned;
			}
			ShareEnergy();
			TextEnergy();
		}
	}

	public void AssignEnergyLess()
	{
		if ((!(CardScreenManager.Instance != null) || !CardScreenManager.Instance.IsActive()) && !TomeManager.Instance.IsActive())
		{
			currentEnergy--;
			if (currentEnergy < 0)
			{
				currentEnergy = 0;
			}
			ShareEnergy();
			TextEnergy();
		}
	}

	private void ShareEnergy()
	{
		if (GameManager.Instance.IsMultiplayer() && MatchManager.Instance.IsYourTurn())
		{
			MatchManager.Instance.AssignEnergyMultiplayer(currentEnergy);
		}
	}
}
