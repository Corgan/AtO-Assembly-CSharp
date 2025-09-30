// Decompiled with JetBrains decompiler
// Type: CustomAbilities.MindSpikeAbility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using Cards;

#nullable disable
namespace CustomAbilities
{
  public class MindSpikeAbility
  {
    private SpecialCardEnum _collectedSpecialCard = SpecialCardEnum.NightmareEcho;
    private int _currentSpecialCardsUsedInMatch;

    public SpecialCardEnum CollectedSpecialCard => this._collectedSpecialCard;

    public int CurrentSpecialCardsUsedInMatch
    {
      get => this._currentSpecialCardsUsedInMatch;
      set => this._currentSpecialCardsUsedInMatch = value;
    }

    public void IncreaseSpecialCardCount() => ++this._currentSpecialCardsUsedInMatch;

    public void Reset() => this._currentSpecialCardsUsedInMatch = 0;
  }
}
