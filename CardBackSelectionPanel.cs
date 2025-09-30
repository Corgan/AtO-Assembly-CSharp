// Decompiled with JetBrains decompiler
// Type: CardBackSelectionPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

#nullable disable
public class CardBackSelectionPanel : MonoBehaviour
{
  public CardBackSelectionPanel.CardStartingPositions[] cardStartingPositions;
  public List<CardBackSelectionPanel.CardPage> cardPages;
  public int cardPerRow = 8;
  public float cardSize = 1.2f;
  public float distX = 1.75f;
  public float distY = 2.7f;
  public float defaultCategoryButtonSize = 1f;
  public float selectedCategoryButtonSize = 1.1f;
  public GameObject PageButtonPrefab;
  public Transform pageButtonStartingPos;

  private void OnEnable()
  {
    this.cardStartingPositions[0].refTransform.gameObject.SetActive(true);
    for (int index = 1; index < this.cardStartingPositions.Length; ++index)
      this.cardStartingPositions[index].refTransform.gameObject.SetActive(false);
    this.EnableTabFirst();
  }

  public async void EnableTabFirst() => await this.ApplyEnableTabFirst(0);

  private async Task ApplyEnableTabFirst(int index)
  {
    await Task.Delay(10);
    for (int index1 = 0; index1 < this.cardStartingPositions.Length; ++index1)
    {
      this.cardStartingPositions[index1].refTransform.gameObject.SetActive(false);
      this.cardStartingPositions[index1].botonGeneric.Enable();
      this.cardStartingPositions[index1].botonGeneric.transform.localScale = Vector3.one * this.defaultCategoryButtonSize;
    }
    this.cardStartingPositions[index].refTransform.gameObject.SetActive(true);
    this.cardStartingPositions[index].botonGeneric.transform.localScale = Vector3.one * this.selectedCategoryButtonSize;
    this.cardStartingPositions[index].botonGeneric.Disable();
    this.clearRefChildren(this.pageButtonStartingPos);
    int childCount = this.cardStartingPositions[index].refTransform.childCount;
    for (int index2 = 0; index2 < childCount; ++index2)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.PageButtonPrefab, this.pageButtonStartingPos);
      gameObject.transform.localPosition = new Vector3(0.0f, (float) index2 * -1.2f, -1f);
      gameObject.GetComponentInChildren<TMP_Text>().text = (index2 + 1).ToString();
      gameObject.name = "CardBackSelectionPage";
      gameObject.GetComponent<BotonGeneric>().auxInt = index2;
      if (index2 == 0)
        gameObject.GetComponent<BotonGeneric>().Disable();
    }
    if (!((UnityEngine.Object) this.cardStartingPositions[0].refTransform != (UnityEngine.Object) null))
      return;
    this.EnablePage(0);
  }

  public void EnableTab(int index)
  {
    for (int index1 = 0; index1 < this.cardStartingPositions.Length; ++index1)
    {
      this.cardStartingPositions[index1].refTransform.gameObject.SetActive(false);
      this.cardStartingPositions[index1].botonGeneric.Enable();
      this.cardStartingPositions[index1].botonGeneric.transform.localScale = Vector3.one * this.defaultCategoryButtonSize;
    }
    this.cardStartingPositions[index].refTransform.gameObject.SetActive(true);
    this.cardStartingPositions[index].botonGeneric.transform.localScale = Vector3.one * this.selectedCategoryButtonSize;
    this.cardStartingPositions[index].botonGeneric.Disable();
    this.clearRefChildren(this.pageButtonStartingPos);
    int childCount = this.cardStartingPositions[index].refTransform.childCount;
    for (int index2 = 0; index2 < childCount; ++index2)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.PageButtonPrefab, this.pageButtonStartingPos);
      gameObject.transform.localPosition = new Vector3(0.0f, (float) index2 * -1.2f, -1f);
      gameObject.GetComponentInChildren<TMP_Text>().text = (index2 + 1).ToString();
      gameObject.name = "CardBackSelectionPage";
      gameObject.GetComponent<BotonGeneric>().auxInt = index2;
      if (index2 == 0)
        gameObject.GetComponent<BotonGeneric>().Disable();
    }
    if (!((UnityEngine.Object) this.cardStartingPositions[0].refTransform != (UnityEngine.Object) null))
      return;
    this.EnablePage(0);
  }

  public void EnablePage(int index)
  {
    for (int index1 = 0; index1 < this.cardStartingPositions.Length; ++index1)
    {
      if (this.cardStartingPositions[index1].refTransform.gameObject.activeSelf)
      {
        for (int index2 = 0; index2 < this.cardStartingPositions[index1].refTransform.childCount; ++index2)
          this.cardStartingPositions[index1].refTransform.GetChild(index2).gameObject.SetActive(false);
        if (index >= 0 && index < this.cardStartingPositions[index1].refTransform.childCount)
          this.cardStartingPositions[index1].refTransform.GetChild(index).gameObject.SetActive(true);
      }
    }
    foreach (BotonGeneric componentsInChild in this.pageButtonStartingPos.GetComponentsInChildren<BotonGeneric>())
    {
      if (componentsInChild.auxInt == index)
        componentsInChild.Disable();
      else
        componentsInChild.Enable();
    }
  }

  public void clearRefChildren(Transform transform)
  {
    for (int index = 0; index < transform.childCount; ++index)
      UnityEngine.Object.Destroy((UnityEngine.Object) transform.GetChild(index).gameObject);
  }

  public Transform GetStartingRefTransform(string category)
  {
    foreach (CardBackSelectionPanel.CardStartingPositions startingPosition in this.cardStartingPositions)
    {
      if (startingPosition.category == category)
        return startingPosition.refTransform;
    }
    return (Transform) null;
  }

  public void Close()
  {
    if (this.gameObject.activeSelf)
      this.gameObject.SetActive(false);
    PopupManager.Instance.ClosePopup();
  }

  public async void UpdateButtonText(string category)
  {
  }

  private async Task ApplyButtonText(string category)
  {
    await Task.Delay(10);
    for (int index = 0; index < this.cardStartingPositions.Length; ++index)
    {
      if (category == this.cardStartingPositions[index].category)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(Texts.Instance.GetText(this.cardStartingPositions[index].localStringID));
        stringBuilder.Append(" (<color=#FFF><size=+.2>");
        stringBuilder.Append(this.cardStartingPositions[index].refTransform.childCount);
        stringBuilder.Append("</size></color>)");
        this.cardStartingPositions[index].botonGeneric.SetText(stringBuilder.ToString());
        break;
      }
    }
  }

  [Serializable]
  public struct CardStartingPositions
  {
    public string category;
    public string localStringID;
    public Transform refTransform;
    public BotonGeneric botonGeneric;
  }

  [Serializable]
  public struct CardPage
  {
    public GameObject pageButton;
    public GameObject contentContainer;
  }
}
