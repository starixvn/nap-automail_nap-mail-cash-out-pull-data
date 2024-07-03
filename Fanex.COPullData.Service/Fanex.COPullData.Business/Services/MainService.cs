using Fanex.COPullData.Business.Models;
using Fanex.MailDelivery.Core.Models;
using System;
using System.Collections.Generic;

namespace Fanex.COPullData.Business.Services;

public interface MainService
{
    JobMessage Execute();
}

public class MainServiceImp : MainService
{
    private readonly Log log;
    private readonly DataService dataService;
    private readonly DateServiceProvider dateService;
    private readonly ExcelService excelService;
    private readonly MailService mailService;

    public MainServiceImp(
        Log log,
        DataService dataService,
        DateServiceProvider dateService,
        ExcelService excelService,
        MailService mailService
    )
    {
        this.log = log;
        this.dataService = dataService;
        this.dateService = dateService;
        this.excelService = excelService;
        this.mailService = mailService;
    }

    public JobMessage Execute()
    {
        var result = new ExecuteResultModel();
        var jobMessage = new JobMessage();
        try
        {
            result = SendNotification();
        }
        catch (Exception e)
        {
            result.Success = false;
            result.Message = e.Message;
            log.Log(e);
        }
        finally
        {
            jobMessage.Mail = result.Mail;
            jobMessage.JobStatus = result.Success ? JobStatus.Success : JobStatus.DataIsNotReady;
            jobMessage.TaskLog = new TaskLog
            {
                LogTime = DateTimeOffset.Now.DateTime,
                Message = result.Message
            };
        }

        return jobMessage;
    }

    private ExecuteResultModel SendNotification()
    {
        var date = dateService.DateService();
        var dateStart = date.FromDate;
        var dateEnd = date.ToDate;
        var fileNames = new List<string>();
        while (dateStart <= dateEnd)
        {
            var beginDate = dateStart;
            var endDate = beginDate.AddDays(1);
            dateStart = dateStart.AddDays(1);

            var data = dataService.PullData(beginDate, endDate);
            var fileName = excelService.Export(data, beginDate, endDate);
            fileNames.Add(fileName);
        }

        var hasData = fileNames.Count > 0;
        var mail = mailService.CreateMail(hasData, fileNames);

        return new ExecuteResultModel
        {
            Mail = mail,
            Success = true,
            Message = string.Empty
        };
    }
}