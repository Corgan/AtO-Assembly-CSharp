using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class PopupSheet : MonoBehaviour
{
	public GameObject popupPrefab;

	private GameObject GO_Popup;

	private Transform popupT;

	private Coroutine coroutine;

	public bool activated;

	public TMP_Text _name;

	public TMP_Text _class;

	public TMP_Text _life;

	public TMP_Text _cards;

	public TMP_Text _energy;

	public TMP_Text _speed;

	public TMP_Text _resistSlashing;

	public TMP_Text _resistBlunt;

	public TMP_Text _resistPiercing;

	public TMP_Text _resistFire;

	public TMP_Text _resistCold;

	public TMP_Text _resistLightning;

	public TMP_Text _resistMind;

	public TMP_Text _resistHoly;

	public TMP_Text _resistShadow;

	private string sHealth = "<size=2.2><color=#FFF>{0}</color></size>\n{1}";

	private string sStats = "<size=2.4>{0}</size>\n{1}";

	private string sResists = "<size=1.4><color=#FFF>{0}</color></size>\n{1}";

	private string colorRed = "#C34738";

	private string colorGreen = "#2FBA23";

	private void Awake()
	{
		GO_Popup = Object.Instantiate(popupPrefab, Vector3.zero, Quaternion.identity, base.transform);
		popupT = GO_Popup.transform;
		_name = popupT.Find("Personal/Name").GetComponent<TMP_Text>();
		_class = popupT.Find("Personal/Class").GetComponent<TMP_Text>();
		_life = popupT.Find("Personal/HealthV").GetComponent<TMP_Text>();
		_cards = popupT.Find("Personal/CardV").GetComponent<TMP_Text>();
		_energy = popupT.Find("Personal/EnergyV").GetComponent<TMP_Text>();
		_speed = popupT.Find("Personal/SpeedV").GetComponent<TMP_Text>();
		_resistSlashing = popupT.Find("Resists/Slashing").GetComponent<TMP_Text>();
		_resistBlunt = popupT.Find("Resists/Blunt").GetComponent<TMP_Text>();
		_resistPiercing = popupT.Find("Resists/Piercing").GetComponent<TMP_Text>();
		_resistFire = popupT.Find("Resists/Fire").GetComponent<TMP_Text>();
		_resistCold = popupT.Find("Resists/Cold").GetComponent<TMP_Text>();
		_resistLightning = popupT.Find("Resists/Lightning").GetComponent<TMP_Text>();
		_resistMind = popupT.Find("Resists/Mind").GetComponent<TMP_Text>();
		_resistHoly = popupT.Find("Resists/Holy").GetComponent<TMP_Text>();
		_resistShadow = popupT.Find("Resists/Shadow").GetComponent<TMP_Text>();
	}

	private void Start()
	{
		ClosePopup();
	}

	public void ShowPopup(Character _character)
	{
		coroutine = StartCoroutine(ShowPopupCo(_character));
	}

	private IEnumerator ShowPopupCo(Character _character)
	{
		SetPopup(_character);
		popupT.localPosition = Vector3.zero;
		GO_Popup.SetActive(value: true);
		yield return null;
	}

	private void SetPopup(Character _character)
	{
		_name.text = _character.GameName;
		_class.text = _character.SubclassName;
		_life.text = string.Format(sHealth, _character.HpCurrent, _character.Hp);
		_cards.text = string.Format(sStats, FormatColorBig("", _character.GetDrawCardsTurn(), _character.GetAuraDrawModifiers()), FormatColorSmall("", _character.GetAuraDrawModifiers()));
		_energy.text = string.Format(sStats, FormatColorBig("", _character.GetEnergy(), _character.GetAuraStatModifiers(_character.GetEnergyTurn(), Enums.CharacterStat.Energy)), FormatColorSmall("", _character.GetEnergyTurn()));
		int[] speed = _character.GetSpeed();
		_speed.text = string.Format(sStats, FormatColorBig("", speed[0], speed[2]), FormatColorSmall("", speed[2]));
		_resistSlashing.text = string.Format(sResists, FormatColorBig("percent", _character.BonusResists(Enums.DamageType.Slashing), _character.GetAuraResistModifiers(Enums.DamageType.Slashing)), FormatColorSmall("percent", _character.GetAuraResistModifiers(Enums.DamageType.Slashing)));
		_resistBlunt.text = string.Format(sResists, FormatColorBig("percent", _character.BonusResists(Enums.DamageType.Blunt), _character.GetAuraResistModifiers(Enums.DamageType.Blunt)), FormatColorSmall("percent", _character.GetAuraResistModifiers(Enums.DamageType.Blunt)));
		_resistPiercing.text = string.Format(sResists, FormatColorBig("percent", _character.BonusResists(Enums.DamageType.Piercing), _character.GetAuraResistModifiers(Enums.DamageType.Piercing)), FormatColorSmall("percent", _character.GetAuraResistModifiers(Enums.DamageType.Piercing)));
		_resistFire.text = string.Format(sResists, FormatColorBig("percent", _character.BonusResists(Enums.DamageType.Fire), _character.GetAuraResistModifiers(Enums.DamageType.Fire)), FormatColorSmall("percent", _character.GetAuraResistModifiers(Enums.DamageType.Fire)));
		_resistCold.text = string.Format(sResists, FormatColorBig("percent", _character.BonusResists(Enums.DamageType.Cold), _character.GetAuraResistModifiers(Enums.DamageType.Cold)), FormatColorSmall("percent", _character.GetAuraResistModifiers(Enums.DamageType.Cold)));
		_resistLightning.text = string.Format(sResists, FormatColorBig("percent", _character.BonusResists(Enums.DamageType.Lightning), _character.GetAuraResistModifiers(Enums.DamageType.Lightning)), FormatColorSmall("percent", _character.GetAuraResistModifiers(Enums.DamageType.Lightning)));
		_resistMind.text = string.Format(sResists, FormatColorBig("percent", _character.BonusResists(Enums.DamageType.Mind), _character.GetAuraResistModifiers(Enums.DamageType.Mind)), FormatColorSmall("percent", _character.GetAuraResistModifiers(Enums.DamageType.Mind)));
		_resistHoly.text = string.Format(sResists, FormatColorBig("percent", _character.BonusResists(Enums.DamageType.Holy), _character.GetAuraResistModifiers(Enums.DamageType.Holy)), FormatColorSmall("percent", _character.GetAuraResistModifiers(Enums.DamageType.Holy)));
		_resistShadow.text = string.Format(sResists, FormatColorBig("percent", _character.BonusResists(Enums.DamageType.Shadow), _character.GetAuraResistModifiers(Enums.DamageType.Shadow)), FormatColorSmall("percent", _character.GetAuraResistModifiers(Enums.DamageType.Shadow)));
	}

	private string FormatColorBig(string type, int value, int mod)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("<color=");
		if (mod > 0)
		{
			stringBuilder.Append(colorGreen);
		}
		else if (mod < 0)
		{
			stringBuilder.Append(colorRed);
		}
		else
		{
			stringBuilder.Append("#FFF");
		}
		stringBuilder.Append(">");
		stringBuilder.Append(value);
		if (type == "percent")
		{
			stringBuilder.Append("%");
		}
		stringBuilder.Append("</color>");
		return stringBuilder.ToString();
	}

	private string FormatColorSmall(string type, int value)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (value > 0)
		{
			stringBuilder.Append("+");
		}
		else if (value == 0)
		{
			stringBuilder.Append("--");
			return stringBuilder.ToString();
		}
		stringBuilder.Append(value);
		if (type == "percent")
		{
			stringBuilder.Append("%");
		}
		return stringBuilder.ToString();
	}

	public void ClosePopup()
	{
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
		}
		GO_Popup.SetActive(value: false);
		activated = false;
	}

	private Vector3 Position()
	{
		Vector3 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 vector2 = new Vector3(3f, 1f, 10f);
		Vector3 vector3 = new Vector3(-1.5f, 1f, 10f);
		if (vector.x < 5.2f)
		{
			return vector + vector2;
		}
		return vector + vector3;
	}
}
