// Decompiled with JetBrains decompiler
// Type: Cards.CurrentCardIdProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Cards
{
  [CreateAssetMenu(menuName = "CardProviders/CurrentCardIdProvider")]
  public class CurrentCardIdProvider : CardIdProvider
  {
    public override string GetCardID(CardData cardData, Character caster, Character target)
    {
      return cardData?.Id;
    }
  }
}
