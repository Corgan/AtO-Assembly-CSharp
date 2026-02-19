using PDX.SDK;
using PDX.SDK.Contracts;
using PDX.SDK.Contracts.Configuration;
using PDX.SDK.Contracts.Enums;
using PDX.SDK.Contracts.Service.Account.Result;
using PDX.SDK.Contracts.Service.Legal.Models;
using UnityEngine;

public static class FlowExample
{
	public static async void StartPDX()
	{
		Config config = new Config();
		IContext context = await Context.Create(Platform.Windows, "namespace", config);
		StartupResult startupResult = await context.Account.Startup();
		Debug.Log(startupResult);
		if (startupResult.Success)
		{
			Debug.Log("startup Success");
			if (startupResult.LegalDocuments.Count == 0 && startupResult.IsLoggedIn)
			{
			}
			return;
		}
		foreach (Document legalDocument in startupResult.LegalDocuments)
		{
			_ = (await context.Legal.MarkAsViewed(legalDocument)).Success;
		}
	}
}
