// Decompiled with JetBrains decompiler
// Type: Paradox.Startup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 713BD5C6-193C-41A7-907D-A952E5D7E149
// Assembly location: D:\Steam\steamapps\common\Across the Obelisk\AcrossTheObelisk_Data\Managed\Assembly-CSharp.dll

using PDX.SDK;
using PDX.SDK.Contracts;
using PDX.SDK.Contracts.Configuration;
using PDX.SDK.Contracts.Enums;
using PDX.SDK.Contracts.Service.Account.Result;
using PDX.SDK.Contracts.Service.Legal.Models;
using System;
using System.Threading.Tasks;
using UnityEngine;

#nullable disable
namespace Paradox
{
  public class Startup
  {
    private static IContext context;
    private static StartupResult startup;
    private static bool usingLauncher = true;
    public static bool loginFinished = false;
    public static bool waitingForLoginDocuments = false;
    public static bool isLoggedIn = false;
    public static string userId = "";
    public static string userToken = "";
    public static string userNamespaceName = "";
    public static string userSocialName = "";
    private static int userDocuments = 0;
    private static int currentDocument = 0;
    private static string Namespace = "across_the_obelisk";
    private static PDX.SDK.Contracts.Enums.Platform Platform = PDX.SDK.Contracts.Enums.Platform.Windows;

    public static IContext PDXContext => Startup.context;

    public static async Task Start()
    {
      if (Startup.context != null)
        return;
      if (GameManager.Instance.GetDeveloperMode())
        Debug.Log((object) "--------- Startup ---------");
      Config config = new Config()
      {
        GameVersion = GameManager.Instance.gameVersion,
        GameChecksum = "",
        Environment = new BackendEnvironment?(BackendEnvironment.Live),
        Ecosystem = new Ecosystem?(Ecosystem.Steam),
        UserIdType = "steam",
        UserId = SteamManager.Instance.steamId.ToString(),
        Telemetry = new TelemetryConfig(),
        Language = new Language?(Startup.GetLangFromGlobalLang())
      };
      if (GameManager.Instance.PDXCliToken == "")
        Startup.usingLauncher = false;
      int num = Startup.usingLauncher ? 1 : 0;
      Startup.context = await Context.Create(Startup.Platform, Startup.Namespace, config);
      if (!Startup.usingLauncher)
      {
        Startup.startup = await Startup.context.Account.Startup();
        if (Startup.startup.Success)
        {
          if (GameManager.Instance.GetDeveloperMode())
            Debug.Log((object) "Startup Success");
          Startup.userId = Startup.startup.UserId;
          Startup.userToken = Startup.startup.SessionToken;
          if (Startup.startup.LegalDocuments != null)
            Startup.userDocuments = Startup.startup.LegalDocuments.Count;
          if (Startup.userDocuments == 0)
          {
            if (Startup.startup.IsLoggedIn)
            {
              if (GameManager.Instance.GetDeveloperMode())
                Debug.Log((object) "Startup IsLoggedIn");
              await Profile.ProfileGet();
              await Paradox.Account.GetEmail();
            }
            else if (GameManager.Instance.GetDeveloperMode())
              Debug.Log((object) "Startup NOT LoggedIn");
          }
          else
          {
            if (GameManager.Instance.GetDeveloperMode())
              Debug.Log((object) "Showing documents");
            if (GameManager.Instance.GetDeveloperMode())
              Debug.Log((object) Startup.startup.IsLoggedIn);
            Startup.waitingForLoginDocuments = true;
            Startup.currentDocument = -1;
            GameManager.Instance.SceneLoaded();
            Startup.ShowDocumentFromStartup();
            while (Startup.waitingForLoginDocuments)
              await Task.Delay(10);
          }
          Startup.isLoggedIn = Startup.startup.IsLoggedIn;
        }
      }
      else
        await Paradox.Account.LoginWithSessionToken();
      GameManager.Instance.GetDisabledDLCs();
      Telemetry.SendStartGame();
      await SettingsManager.Instance.InitTelemetryToggle();
    }

    public static void Shutdown()
    {
      if (Startup.context == null)
        return;
      Startup.context.Shutdown();
    }

    public static void ShowDocumentFromStartup()
    {
      ++Startup.currentDocument;
      if (Startup.currentDocument < Startup.startup.LegalDocuments.Count)
      {
        Startup.ShowDocumentAction(Startup.startup.LegalDocuments[Startup.currentDocument], true);
      }
      else
      {
        Startup.waitingForLoginDocuments = false;
        MainMenuManager.Instance.ClosePDXDocument();
      }
    }

    public static async Task ShowDocumentFromForm(int document)
    {
      switch (document)
      {
        case 0:
          await Startup.ShowDocumentAction((await Startup.context.Legal.GetEula(new Language?((Language) Enum.Parse(typeof (Language), Globals.Instance.CurrentLang.Substring(0, 2))))).Document);
          break;
        case 1:
          await Startup.ShowDocumentAction((await Startup.context.Legal.GetPrivacy(new Language?((Language) Enum.Parse(typeof (Language), Globals.Instance.CurrentLang.Substring(0, 2))))).Document);
          break;
      }
    }

    public static async Task ShowDocumentAction(Document doc, bool isFromStartup = false)
    {
      if (GameManager.Instance.GetDeveloperMode())
        Debug.Log((object) ("ShowDocumentAction " + doc.Title));
      MainMenuManager.Instance.paradoxDocumentText.text = doc.Text;
      MainMenuManager.Instance.ShowPDXDocument();
      if (!isFromStartup)
        return;
      PDX.SDK.Contracts.Result result = await Startup.context.Legal.MarkAsViewed(doc);
    }

    public static Language GetLangFromGlobalLang()
    {
      string currentLang = Globals.Instance.CurrentLang;
      Language langFromGlobalLang;
      switch (currentLang)
      {
        case "zh-TW":
          langFromGlobalLang = (Language) Enum.Parse(typeof (Language), "zh_Hant");
          break;
        case "jp":
          langFromGlobalLang = (Language) Enum.Parse(typeof (Language), "ja");
          break;
        default:
          langFromGlobalLang = (Language) Enum.Parse(typeof (Language), currentLang.Substring(0, 2));
          break;
      }
      return langFromGlobalLang;
    }
  }
}
