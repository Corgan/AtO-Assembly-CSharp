// Decompiled with JetBrains decompiler
// Type: UnlockedBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

#nullable disable
public class UnlockedBar : MonoBehaviour
{
  public string type;
  public Transform maskTransform;
  public SpriteRenderer barSprite;
  public TMP_Text titleText;
  public TMP_Text cardsText;
  public SpriteRenderer sigil0;
  public SpriteRenderer sigil1;
  public SpriteRenderer sigil2;
  public SpriteRenderer sigil3;
  public SpriteRenderer sigil4;
  private float scale100 = 3.38f;
  private int cardsTotal = -1;
  private int cardsUnlocked;

  public void InitBar()
  {
    this.cardsTotal = -1;
    this.cardsUnlocked = 0;
    this.SetBasics();
    this.CalculateUnlock();
  }

  private void SetBasics()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("<size=+.3>");
    if (this.type == "warriorCards")
    {
      this.titleText.color = this.cardsText.color = this.barSprite.color = Functions.HexToColor(Globals.Instance.ClassColor["warrior"]);
      stringBuilder.Append("<sprite name=slashing>");
    }
    else if (this.type == "scoutCards")
    {
      this.titleText.color = this.cardsText.color = this.barSprite.color = Functions.HexToColor(Globals.Instance.ClassColor["scout"]);
      stringBuilder.Append("<sprite name=piercing>");
    }
    else if (this.type == "mageCards")
    {
      this.titleText.color = this.cardsText.color = this.barSprite.color = Functions.HexToColor(Globals.Instance.ClassColor["mage"]);
      stringBuilder.Append("<sprite name=fire>");
    }
    else if (this.type == "healerCards")
    {
      this.titleText.color = this.cardsText.color = this.barSprite.color = Functions.HexToColor(Globals.Instance.ClassColor["healer"]);
      stringBuilder.Append("<sprite name=heal>");
    }
    else if (this.type == "equipment")
    {
      this.titleText.color = this.cardsText.color = this.barSprite.color = Functions.HexToColor(Globals.Instance.ClassColor["item"]);
      stringBuilder.Append("<sprite name=jewelry>");
    }
    else if (this.type == "mapNodes")
    {
      this.titleText.color = this.cardsText.color = this.barSprite.color = Functions.HexToColor(Globals.Instance.ClassColor["boon"]);
      stringBuilder.Append("<sprite name=node>");
    }
    else if (this.type == "uniqueBosses")
    {
      this.titleText.color = this.cardsText.color = this.barSprite.color = Functions.HexToColor("#784FD4");
      stringBuilder.Append("<sprite name=bossIcon>");
    }
    stringBuilder.Append("</size> ");
    stringBuilder.Append(Texts.Instance.GetText(this.type));
    this.titleText.text = stringBuilder.ToString();
  }

  private void CalculateUnlock()
  {
    if (this.cardsTotal == -1)
    {
      Enums.CardClass key = Enums.CardClass.Warrior;
      bool flag1 = false;
      bool flag2 = false;
      if (this.type == "warriorCards")
        key = Enums.CardClass.Warrior;
      else if (this.type == "scoutCards")
        key = Enums.CardClass.Scout;
      else if (this.type == "mageCards")
        key = Enums.CardClass.Mage;
      else if (this.type == "healerCards")
        key = Enums.CardClass.Healer;
      else if (this.type == "equipment")
        key = Enums.CardClass.Item;
      else if (this.type == "mapNodes")
        flag1 = true;
      else if (this.type == "uniqueBosses")
        flag2 = true;
      if (flag1)
      {
        List<string> stringList1 = new List<string>();
        List<string> stringList2 = new List<string>();
        this.cardsUnlocked = 0;
        foreach (KeyValuePair<string, NodeData> message in Globals.Instance.NodeDataSource)
        {
          try
          {
            if (message.Key != "tutorial_0")
            {
              if (message.Key != "tutorial_1")
              {
                if (message.Key != "tutorial_2")
                {
                  if (!(message.Value.NodeZone.ZoneId == "Aquarfall") && !(message.Value.NodeZone.ZoneId == "Sectarium") && !(message.Value.NodeZone.ZoneId == "Senenthia") && !(message.Value.NodeZone.ZoneId == "Spiderlair") && !(message.Value.NodeZone.ZoneId == "Velkarath") && !(message.Value.NodeZone.ZoneId == "Voidhigh") && !(message.Value.NodeZone.ZoneId == "Voidlow") && !(message.Value.NodeZone.ZoneId == "Faeborg") && !(message.Value.NodeZone.ZoneId == "Frozensewers") && !(message.Value.NodeZone.ZoneId == "Blackforge") && !(message.Value.NodeZone.ZoneId == "Ulminin"))
                  {
                    if (!(message.Value.NodeZone.ZoneId == "Pyramid"))
                      continue;
                  }
                  if (!message.Value.GoToTown)
                  {
                    if (!message.Value.TravelDestination)
                      stringList1.Add(message.Key);
                  }
                }
              }
            }
          }
          catch (Exception ex)
          {
            Debug.Log((object) message);
            throw;
          }
        }
        if (PlayerManager.Instance.UnlockedNodes != null)
        {
          for (int index = 0; index < PlayerManager.Instance.UnlockedNodes.Count; ++index)
          {
            if (Globals.Instance.NodeCombatEventRelation.ContainsKey(PlayerManager.Instance.UnlockedNodes[index]))
            {
              string str = Globals.Instance.NodeCombatEventRelation[PlayerManager.Instance.UnlockedNodes[index]];
              if (stringList1.Contains(str))
                stringList2.Add(str);
            }
          }
        }
        this.cardsTotal = stringList1.Count;
        this.cardsUnlocked = stringList2.Count;
      }
      else if (flag2)
      {
        this.cardsUnlocked = PlayerManager.Instance.BossesKilledName == null ? 0 : PlayerManager.Instance.BossesKilledName.Count;
        this.cardsTotal = 0;
        List<string> stringList = new List<string>();
        foreach (KeyValuePair<string, NPCData> npC in Globals.Instance.NPCs)
        {
          if (npC.Value.IsBoss && !stringList.Contains(npC.Value.NPCName))
          {
            stringList.Add(npC.Value.NPCName);
            ++this.cardsTotal;
          }
        }
      }
      else
      {
        List<string> stringList = Globals.Instance.CardListNotUpgradedByClass[key];
        this.cardsTotal = stringList.Count;
        for (int index = 0; index < this.cardsTotal; ++index)
        {
          if (PlayerManager.Instance.IsCardUnlocked(stringList[index]))
            ++this.cardsUnlocked;
        }
      }
    }
    if (this.cardsUnlocked > this.cardsTotal)
      this.cardsUnlocked = this.cardsTotal;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("<size=+1.4>");
    stringBuilder.Append(this.cardsUnlocked);
    stringBuilder.Append("</size> <voffset=.4><color=#333>/");
    stringBuilder.Append(this.cardsTotal);
    stringBuilder.Append("</color>");
    this.cardsText.text = stringBuilder.ToString();
    this.maskTransform.localScale = new Vector3((float) this.cardsUnlocked / (float) this.cardsTotal * this.scale100, 2.03f, this.maskTransform.localScale.z);
  }
}
