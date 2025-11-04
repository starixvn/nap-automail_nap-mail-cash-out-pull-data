using System;
using System.IO;
using Fanex.COPullData.Business.Models;
using Fanex.COPullData.Business.Services;
using Fanex.Data.Repository;
using Fanex.MailDelivery.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace Fanex.COPullData.Service;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Services { get; set; }

    public static void AddConfig(this IServiceCollection services)
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        var text = File.ReadAllText("appsettings.json");
        AppConfigService.Config = JsonConvert.DeserializeObject<ConfigSettingModel>(text);
    }

    public static void AddInternalServices(this IServiceCollection services)
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