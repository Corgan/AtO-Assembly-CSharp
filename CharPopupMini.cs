// Decompiled with JetBrains decompiler
// Type: CharPopupMini
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using WebSocketSharp;

#nullable disable
public class CharPopupMini : MonoBehaviour
{
  [SerializeField]
  private SpriteRenderer portrait;
  [SerializeField]
  private SpriteRenderer bgGradient;
  [SerializeField]
  private TMP_Text heroNameTxt;
  [SerializeField]
  private TMP_Text heroClassTxt;
  [SerializeField]
  private TMP_Text rankTxt;
  [SerializeField]
  private TMP_Text descriptionTxt;
  [SerializeField]
  private TMP_Text[] resistTxts;
  [SerializeField]
  private TMP_Text healthTxt;
  [SerializeField]
  private TMP_Text energyTxt;
  [SerializeField]
  private TMP_Text speedTxt;
  [SerializeField]
  private GameObject DlcLogo;
  [HideInInspector]
  public SubClassData subClassData;
  private Dictionary<string, string> heroClassIcons;
  private Color colorWhite = Color.white;
  private Color colorGray = new Color(0.665f, 0.665f, 0.665f);

  private void Awake()
  {
    this.heroClassIcons = new Dictionary<string, string>();
    this.heroClassIcons.Add("warrior", "block");
    this.heroClassIcons.Add("scout", "piercing");
    this.heroClassIcons.Add("healer", "regeneration");
    this.heroClassIcons.Add("mage", "fire");
  }

  private void Start() => Debug.Log((object) "heloo");

  public void SetSubClassData(SubClassData subclass)
  {
    this.subClassData = subclass;
    this.heroNameTxt.text = subclass.CharacterName;
    this.SetPortrait(this.subClassData);
    this.SetBGColor(this.subClassData);
    this.SetNameAndClass(this.subClassData);
    this.SetRank(this.subClassData);
    this.SetDescription(this.subClassData);
    this.SetResistances(this.subClassData);
    this.SetHealth(this.subClassData);
    this.SetEnergy(this.subClassData);
    this.SetSpeed(this.subClassData);
    this.gameObject.SetActive(true);
    if (!this.subClassData.Sku.IsNullOrEmpty())
    {
      this.DlcLogo.SetActive(true);
      if (!this.subClassData.Sku.IsNullOrEmpty())
      {
        this.DlcLogo.SetActive(true);
        this.DlcLogo.GetComponent<PopupText>().text = string.Format(Texts.Instance.GetText("requiredDLC").Replace("#FFF", "#CEA843"), (object) SteamManager.Instance.GetDLCName(this.subClassData.Sku));
      }
    }
    else
      this.DlcLogo.SetActive(false);
    HeroSelectionManager.Instance.charPopup.Init(this.subClassData, showNothing: true);
    HeroSelectionManager.Instance.charPopup.ShowStats();
    HeroSelectionManager.Instance.charPopup.AllowAllButtons();
    HeroSelectionManager.Instance.charPopup.Close();
  }

  private void SetNameAndClass(SubClassData subClassData)
  {
    this.heroClassTxt.text = subClassData.SubClassName;
    string str1 = Functions.ClassIconFromString(Enum.GetName(typeof (Enums.HeroClass), (object) subClassData.HeroClass));
    this.heroClassTxt.color = this.GetClassColor(subClassData);
    TMP_Text heroClassTxt1 = this.heroClassTxt;
    heroClassTxt1.text = heroClassTxt1.text + " <sprite name=" + str1 + ">";
    if (subClassData.HeroClassSecondary != Enums.HeroClass.None)
    {
      string str2 = Functions.ClassIconFromString(Enum.GetName(typeof (Enums.HeroClass), (object) subClassData.HeroClassSecondary));
      TMP_Text heroClassTxt2 = this.heroClassTxt;
      heroClassTxt2.text = heroClassTxt2.text + "<sprite name=" + str2 + ">";
    }
    if (subClassData.HeroClassThird == Enums.HeroClass.None)
      return;
    string str3 = Functions.ClassIconFromString(Enum.GetName(typeof (Enums.HeroClass), (object) subClassData.HeroClassSecondary));
    TMP_Text heroClassTxt3 = this.heroClassTxt;
    heroClassTxt3.text = heroClassTxt3.text + "<sprite name=" + str3 + ">";
  }

  private Color GetClassColor(SubClassData subClassData)
  {
    return Functions.HexToColor(Globals.Instance.ClassColor[Enum.GetName(typeof (Enums.HeroClass), (object) subClassData.HeroClass)]);
  }

  private void SetRank(SubClassData subClassData)
  {
    this.rankTxt.text = string.Format(Texts.Instance.GetText("rankProgress"), (object) PlayerManager.Instance.GetPerkRank(subClassData.Id));
  }

  private void SetBGColor(SubClassData subClassData)
  {
    Color classColor = this.GetClassColor(subClassData);
    this.bgGradient.color = new Color(classColor.r, classColor.g, classColor.b, 0.2f);
  }

  private void SetDescription(SubClassData subClassData)
  {
    this.descriptionTxt.text = subClassData.CharacterDescriptionStrength;
  }

  private void SetResistances(SubClassData _scd)
  {
    this.SetResistText(_scd, this.resistTxts[0], Enums.DamageType.Slashing);
    this.SetResistText(_scd, this.resistTxts[1], Enums.DamageType.Blunt);
    this.SetResistText(_scd, this.resistTxts[2], Enums.DamageType.Piercing);
    this.SetResistText(_scd, this.resistTxts[3], Enums.DamageType.Fire);
    this.SetResistText(_scd, this.resistTxts[4], Enums.DamageType.Cold);
    this.SetResistText(_scd, this.resistTxts[5], Enums.DamageType.Lightning);
    this.SetResistText(_scd, this.resistTxts[6], Enums.DamageType.Mind);
    this.SetResistText(_scd, this.resistTxts[7], Enums.DamageType.Holy);
    this.SetResistText(_scd, this.resistTxts[8], Enums.DamageType.Shadow);
  }

  private void SetResistText(SubClassData _scd, TMP_Text resistText, Enums.DamageType damageType)
  {
    int num = 0;
    switch (damageType)
    {
      case Enums.DamageType.Slashing:
        num = _scd.ResistSlashing;
        break;
      case Enums.DamageType.Blunt:
        num = _scd.ResistBlunt;
        break;
      case Enums.DamageType.Piercing:
        num = _scd.ResistPiercing;
        break;
      case Enums.DamageType.Fire:
        num = _scd.ResistFire;
        break;
      case Enums.DamageType.Cold:
        num = _scd.ResistCold;
        break;
      case Enums.DamageType.Lightning:
        num = _scd.ResistLightning;
        break;
      case Enums.DamageType.Mind:
        num = _scd.ResistMind;
        break;
      case Enums.DamageType.Holy:
        num = _scd.ResistHoly;
        break;
      case Enums.DamageType.Shadow:
        num = _scd.ResistShadow;
        break;
    }
    if (!GameManager.Instance.IsObeliskChallenge())
      num += PlayerManager.Instance.GetPerkResistBonus(_scd.Id, damageType);
    resistText.text = !Functions.SpaceBeforePercentSign() ? num.ToString() + "%" : num.ToString() + " %";
    if (num > 0)
      resistText.color = this.colorWhite;
    else
      resistText.color = this.colorGray;
  }

  private void SetHealth(SubClassData _scd)
  {
    int hp = _scd.Hp;
    if (!GameManager.Instance.IsObeliskChallenge())
      hp += PlayerManager.Instance.GetPerkMaxHealth(_scd.Id);
    this.healthTxt.text = hp.ToString();
  }

  private void SetEnergy(SubClassData _scd)
  {
    StringBuilder stringBuilder = new StringBuilder();
    int energy = _scd.Energy;
    if (!GameManager.Instance.IsObeliskChallenge())
      energy += PlayerManager.Instance.GetPerkEnergyBegin(_scd.Id);
    stringBuilder.Append(energy);
    stringBuilder.Append(" <size=1.3>");
    stringBuilder.Append(Texts.Instance.GetText("dataPerTurn").Replace("<%>", _scd.EnergyTurn.ToString()));
    stringBuilder.Append("</size>");
    this.energyTxt.text = stringBuilder.ToString();
  }

  private void SetSpeed(SubClassData _scd)
  {
    int speed = _scd.Speed;
    if (!GameManager.Instance.IsObeliskChallenge())
      speed += PlayerManager.Instance.GetPerkSpeed(_scd.Id);
    this.speedTxt.text = speed.ToString();
  }

  private void SetPortrait(SubClassData subClassData)
  {
    string activeSkin = PlayerManager.Instance.GetActiveSkin(subClassData.Id);
    if (activeSkin != "")
    {
      SkinData skinData = Globals.Instance.GetSkinData(activeSkin);
      if ((UnityEngine.Object) skinData == (UnityEngine.Object) null)
        skinData = Globals.Instance.GetSkinData(Globals.Instance.GetSkinBaseIdBySubclass(subClassData.Id));
      this.portrait.sprite = skinData.SpriteSiluetaGrande;
    }
    else
      this.portrait.sprite = subClassData.SpriteBorder;
  }

  public void ClosePopup() => this.gameObject.SetActive(false);
}
