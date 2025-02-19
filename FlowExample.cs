// Decompiled with JetBrains decompiler
// Type: FlowExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A7FF4DC-8758-4E86-8AC4-2226379516BE
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using PDX.SDK;
using PDX.SDK.Contracts;
using PDX.SDK.Contracts.Configuration;
using PDX.SDK.Contracts.Service.Account.Result;
using PDX.SDK.Contracts.Service.Legal.Models;
using UnityEngine;

#nullable disable
public static class FlowExample
{
  public static async void StartPDX()
  {
    IContext context = await Context.Create(PDX.SDK.Contracts.Enums.Platform.Windows, "namespace", new Config());
    StartupResult message = await context.Account.Startup();
    Debug.Log((object) message);
    if (message.Success)
    {
      Debug.Log((object) "startup Success");
      if (message.LegalDocuments.Count != 0)
        context = (IContext) null;
      else if (message.IsLoggedIn)
        context = (IContext) null;
      else
        context = (IContext) null;
    }
    else
    {
      foreach (Document legalDocument in message.LegalDocuments)
      {
        int num = (await context.Legal.MarkAsViewed(legalDocument)).Success ? 1 : 0;
      }
      context = (IContext) null;
    }
  }
}
