using Fanex.COPullData.Business.Criteria;
using Fanex.COPullData.Business.Models;
using Fanex.Data.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Fanex.COPullData.Business.Services;

public interface DataService
{
    List<COModel> PullData(DateTime startDate, DateTime endDate);
}

public class DataServiceImp : DataService
{
    private static readonly MediaTypeWithQualityHeaderValue Media = new("application/json");
    private readonly IDynamicRepository repository;

    public DataServiceImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public List<COModel> PullData(DateTime startDate, DateTime endDate)
    {
        var json = GetDataFromApi(startDate, endDate);
        var data = JsonConvert.DeserializeObject<List<COModel>>(json)!;
        var dbs = new List<string> { "DBWH_61", "DBWH_62" };

        dbs.ForEach(db =>
        {
            ImportData(db, json);
        });

        return data;
    }

    private static string GetDataFromApi(DateTime startDate, DateTime endDate)
    {
        var action = $"api/JackTimeProxy/GetReportDataFromJackTime?startDate={startDate}&endDate={endDate}";
        var url = $"{AppConfigService.Config.UrlApi}{action}";
        var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(Media);
        var rawData = client.GetStringAsync(url).Result;
        client.Dispose();

        return rawData;
    }

    private void ImportData(string db, string json)
    {
        var cmd = new CoCommand(db) { Json = json };
        repository.Execute(cmd);
    }
}