using Fanex.MailDelivery.Core.Models;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;

namespace Fanex.COPullData.Business.Services;

public interface MailService
{
    Mail CreateMail(bool hasData, List<string> fileNames);
}

public class MailServiceImp : MailService
{
    public Mail CreateMail(bool hasData, List<string> fileNames)
    {
        var att = new List<MailAttachment>();
        var shortNames = new List<string>();
        fileNames.ForEach(fileName =>
        {
            att.Add(new(fileName, FileExtension.MS_EXCEL));
            shortNames.Add(new FileInfo(fileName).Name);
        });

        var mail = new Mail
        {
            Title = "ONEworks Auto Generated Report Administrator",
            Subject = $"Automail Report - Daily JackTime Data Report - {DateTimeOffset.Now.DateTime:yyyy/MM/dd}",
            Body = LoadBody(hasData, shortNames),
            Attachments = hasData ? att : new List<MailAttachment>()
        };

        return mail;
    }

    private static string LoadBody(bool hasData, List<string> fileName)
    {
        var template = File.ReadAllText(@"Templates/EmailTemplate.cshtml");
        var tuple = new Tuple<bool, List<string>>(hasData, fileName);
        var modelType = typeof(Tuple<bool, List<string>>);
        const string tplKey = "Fanex.COPullData.Service";
        var result = Engine.Razor.IsTemplateCached(tplKey, modelType)
            ? Engine.Razor.Run(tplKey, modelType, tuple)
            : Engine.Razor.RunCompile(template, tplKey, modelType, tuple);

        return result;
    }
}