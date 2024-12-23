using Fanex.COPullData.Business.Models;
using Fanex.COPullData.Business.Services;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.Data.SqlServer;
using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using Fanex.MailDelivery.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OfficeOpenXml;
using SharpRaven;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fanex.COPullData.Service;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Services { get; set; }

    public static void LoadConfiguration(this IServiceCollection services)
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        var text = File.ReadAllText("appsettings.json");
        AppConfigService.Config = JsonConvert.DeserializeObject<ConfigSettingModel>(text);
    }

    public static void SentryRegister(this IServiceCollection services)
    {
        var sentry = new SentryLogging(new SentryEngineOptions
        {
            Dsn = new Dsn(AppConfigService.Config.SentryClientKey)
        });

        LogManager
            .SetDefaultLogCategory(AppConfigService.Config.CategorySentryLog)
            .Use(new List<ILogEngine>
            {
                sentry
            });
    }

    public static void FanexDataRegister(this IServiceCollection services, IConfiguration config)
    {
        try
        {
            DbSettingProviderManager.StartNewSession()
                .WithSqlServer(config)
                .UseDefaultDbSettingProvider("App_Data", true)
                .Run();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static void InternalServiceRegister(this IServiceCollection services)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        services.AddSingleton<Log, LogImp>();
        services.AddSingleton<MainService, MainServiceImp>();
        services.AddSingleton<IDynamicRepository, DynamicRepository>();
        services.AddSingleton<DataService, DataServiceImp>();
        services.AddSingleton<DateServiceProvider, DateServiceProviderImp>();
        services.AddSingleton<ExcelService, ExcelServiceImp>();

        services.AddSingleton<MailService, MailServiceImp>();

        Services = services;
        MailClientHandler.Start();
    }
}