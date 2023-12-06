using System;

namespace Fanex.COPullData.Business.Models;

public class ConfigSettingModel
{
    public string SentryClientKey { get; set; }

    public string CategorySentryLog { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public bool IsManual { get; set; }

    public string Path { get; set; }

    public string UrlApi { get; set; }

    public string Password { get; set; }
}